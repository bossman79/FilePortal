import os
import shutil
import hashlib
import socket
import uuid
import msal
import requests
from flask import Flask, render_template, request, redirect, url_for, session, jsonify, flash
from werkzeug.utils import secure_filename
from database import PortalDB
from adept_wrapper import AdeptWrapper
from sharepoint_sync import sync_engine

app = Flask(__name__)
app.secret_key = 'adept_portal_super_secret_key_change_in_production'

# Start background sync engine
sync_engine.start()

# Setup directories
BASE_DIR = os.path.dirname(os.path.abspath(__name__))
UPLOAD_FOLDER = os.path.join(BASE_DIR, 'temp_uploads')
os.makedirs(UPLOAD_FOLDER, exist_ok=True)
app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER
app.config['MAX_CONTENT_LENGTH'] = 500 * 1024 * 1024 # 500 MB max upload

db = PortalDB()

try:
    adept = AdeptWrapper()
    print("Adept API Initialized successfully.")
except Exception as e:
    print(f"Error initializing Adept API: {e}")
    adept = None

def get_adept_domains():
    if adept:
        try:
            return adept.get_domains()
        except:
            return []
    return []

import urllib.request

def get_wan_ip():
    """Get the public WAN IP address"""
    # Try multiple services in case one fails
    services = [
        'https://api.ipify.org',
        'https://api.my-ip.io/ip',
        'https://checkip.amazonaws.com',
        'https://icanhazip.com'
    ]
    
    for service in services:
        try:
            req = urllib.request.Request(service)
            req.add_header('User-Agent', 'Mozilla/5.0')
            with urllib.request.urlopen(req, timeout=5) as response:
                ip = response.read().decode('utf-8').strip()
                # Validate it's actually a public IP (not private/local)
                if ip and not ip.startswith(('127.', '10.', '172.16.', '192.168.')):
                    return ip
        except Exception as e:
            print(f"Failed to get IP from {service}: {e}")
            continue
    
    # All services failed - return placeholder
    print("WARNING: Could not determine public IP address")
    return 'YOUR-PUBLIC-IP-HERE'

# --- Admin Routes ---

@app.route('/')
def index():
    if 'username' in session:
        return redirect(url_for('dashboard'))
    return redirect(url_for('login'))

@app.route('/admin/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        domain = request.form.get('domain')
        username = request.form.get('username')
        password = request.form.get('password')
        
        if adept:
            if adept.authenticate(domain, username, password):
                session['username'] = username
                session['domain'] = domain
                return redirect(url_for('dashboard'))
            else:
                flash('Invalid Adept credentials or domain.', 'error')
        else:
            # Fallback for dev if Adept not available
            if username == 'admin' and password == 'admin':
                session['username'] = username
                session['domain'] = 'Dev'
                return redirect(url_for('dashboard'))
            flash('Adept API not loaded. Use admin/admin for dev.', 'error')
            
    domains = get_adept_domains()
    # Fallback to defaults if Adept isn't loading domains
    if not domains:
        domains = [{"name": "Adept", "last_login": "admin"}]
        
    return render_template('login.html', domains=domains)

@app.route('/admin/logout')
def logout():
    session.pop('username', None)
    session.pop('domain', None)
    return redirect(url_for('login'))

@app.route('/admin/dashboard')
def dashboard():
    if 'username' not in session:
        return redirect(url_for('login'))
        
    projects = db.get_projects()
    links = db.get_links()
    
    default_base_url = f"http://{get_wan_ip()}:5000"
    base_url = db.get_setting('public_base_url', default_base_url).rstrip('/')
    
    sp_tenant_id = db.get_setting('sp_tenant_id', '')
    sp_client_id = db.get_setting('sp_client_id', '')
    sp_client_secret = db.get_setting('sp_client_secret', '')
    sp_account_username = db.get_setting('sp_account_username', '')
    
    # Map project names to links for UI convenience
    for link in links:
        proj = db.get_project_by_id(link['project_id'])
        link['project_name'] = proj['name'] if proj else 'Unknown'
        link['full_url'] = f"{base_url}/s/{link['uuid']}"
        
    return render_template('dashboard.html', projects=projects, links=links, base_url=base_url,
                           sp_tenant_id=sp_tenant_id, sp_client_id=sp_client_id, sp_client_secret=sp_client_secret,
                           sp_account_username=sp_account_username, upload_page_enabled=(db.get_setting('upload_page_enabled', 'true').lower() == 'true'))

@app.route('/admin/api/detect_public_ip', methods=['GET'])
def detect_public_ip():
    if 'username' not in session:
        return jsonify({"error": "Unauthorized"}), 401
    
    public_ip = get_wan_ip()
    
    if public_ip and not public_ip.startswith(('10.', '172.16.', '192.168.', '127.', 'YOUR-PUBLIC-IP')):
        # Automatically save it
        db.set_setting('public_base_url', f'http://{public_ip}:5000')
        return jsonify({"ip": public_ip, "url": f'http://{public_ip}:5000', "saved": True})
    else:
        return jsonify({"error": "Could not detect valid public IP", "ip": public_ip}), 400

@app.route('/admin/api/settings', methods=['POST'])
def update_settings():
    if 'username' not in session:
        return jsonify({"error": "Unauthorized"}), 401
    data = request.json
    db.set_setting('public_base_url', data.get('public_base_url', '').strip())
    db.set_setting('sp_tenant_id', data.get('sp_tenant_id', '').strip())
    db.set_setting('sp_client_id', data.get('sp_client_id', '').strip())
    db.set_setting('sp_client_secret', data.get('sp_client_secret', '').strip())
    db.set_setting('upload_page_enabled', 'true' if data.get('upload_page_enabled') else 'false')
    return jsonify({"message": "Settings updated"})

DEFAULT_PUBLIC_CLIENT_ID = "04b07795-8ddb-461a-bbee-02f9e1bf7b46" # Azure CLI Client ID
device_flows = {}

@app.route('/admin/api/sp/device_code', methods=['POST'])
def get_device_code():
    if 'username' not in session: return jsonify({"error": "Unauthorized"}), 401
    
    client_id = db.get_setting('sp_client_id') or DEFAULT_PUBLIC_CLIENT_ID
    tenant_id = db.get_setting('sp_tenant_id') or "common"
    authority = f"https://login.microsoftonline.com/{tenant_id}"
    
    msal_app = msal.PublicClientApplication(client_id, authority=authority)
    flow = msal_app.initiate_device_flow(scopes=["Sites.ReadWrite.All", "Files.ReadWrite.All"])
    if "user_code" not in flow:
        return jsonify({"error": "Failed to create device flow"}), 400
        
    flow_id = str(uuid.uuid4())
    device_flows[flow_id] = flow
    return jsonify({
        "flow_id": flow_id,
        "user_code": flow["user_code"],
        "verification_uri": flow["verification_uri"],
        "message": flow["message"]
    })

@app.route('/admin/api/sp/poll_token', methods=['POST'])
def poll_device_token():
    if 'username' not in session: return jsonify({"error": "Unauthorized"}), 401
    flow_id = request.json.get('flow_id')
    flow = device_flows.get(flow_id)
    if not flow: return jsonify({"error": "Invalid flow"}), 400
        
    client_id = db.get_setting('sp_client_id') or DEFAULT_PUBLIC_CLIENT_ID
    tenant_id = db.get_setting('sp_tenant_id') or "common"
    authority = f"https://login.microsoftonline.com/{tenant_id}"
    
    cache = msal.SerializableTokenCache()
    msal_app = msal.PublicClientApplication(client_id, authority=authority, token_cache=cache)
    
    result = msal_app.acquire_token_by_device_flow(flow)
    if "access_token" in result:
        db.set_setting('sp_token_cache', cache.serialize())
        db.set_setting('sp_device_client_id', client_id)
        db.set_setting('sp_device_tenant_id', tenant_id)
        
        accounts = msal_app.get_accounts()
        if accounts:
            db.set_setting('sp_account_username', accounts[0]['username'])
            db.set_setting('sp_account_home_id', accounts[0]['home_account_id'])
            
        del device_flows[flow_id]
        return jsonify({"success": True})
        
    return jsonify({"error": "Failed to acquire token", "details": result}), 400

def get_sp_access_token():
    """Helper function to get SharePoint access token for API calls"""
    token_cache_str = db.get_setting('sp_token_cache')
    if not token_cache_str:
        return None
        
    client_id = db.get_setting('sp_device_client_id', DEFAULT_PUBLIC_CLIENT_ID)
    tenant_id = db.get_setting('sp_device_tenant_id', 'common')
    authority = f"https://login.microsoftonline.com/{tenant_id}"
    
    cache = msal.SerializableTokenCache()
    cache.deserialize(token_cache_str)
    
    app = msal.PublicClientApplication(client_id, authority=authority, token_cache=cache)
    
    home_id = db.get_setting('sp_account_home_id')
    accounts = app.get_accounts()
    account = next((a for a in accounts if a.get('home_account_id') == home_id), None)
    
    if account:
        try:
            result = app.acquire_token_silent(["Sites.ReadWrite.All", "Files.ReadWrite.All"], account=account)
            if result and "access_token" in result:
                if cache.has_state_changed:
                    db.set_setting('sp_token_cache', cache.serialize())
                return result["access_token"]
        except Exception as e:
            print(f"Token refresh error: {e}")
    return None

@app.route('/admin/api/sp/sites', methods=['GET'])
def list_sharepoint_sites():
    if 'username' not in session: return jsonify({"error": "Unauthorized"}), 401
    
    token = get_sp_access_token()
    if not token:
        return jsonify({"error": "Not authenticated to SharePoint"}), 401
    
    headers = {'Authorization': f'Bearer {token}', 'Accept': 'application/json'}
    
    try:
        # Get sites the user has access to
        response = requests.get('https://graph.microsoft.com/v1.0/sites?search=*', headers=headers, timeout=10)
        if response.status_code == 200:
            data = response.json()
            sites = []
            for site in data.get('value', []):
                sites.append({
                    'id': site['id'],
                    'name': site.get('displayName', site.get('name', 'Unknown')),
                    'webUrl': site.get('webUrl', '')
                })
            return jsonify({"sites": sites})
        else:
            return jsonify({"error": f"API error: {response.status_code}", "details": response.text}), response.status_code
    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/admin/api/sp/sites/<site_id>/drives', methods=['GET'])
def list_site_drives(site_id):
    if 'username' not in session: return jsonify({"error": "Unauthorized"}), 401
    
    token = get_sp_access_token()
    if not token:
        return jsonify({"error": "Not authenticated to SharePoint"}), 401
    
    headers = {'Authorization': f'Bearer {token}', 'Accept': 'application/json'}
    
    try:
        response = requests.get(f'https://graph.microsoft.com/v1.0/sites/{site_id}/drives', headers=headers, timeout=10)
        if response.status_code == 200:
            data = response.json()
            drives = []
            for drive in data.get('value', []):
                drives.append({
                    'id': drive['id'],
                    'name': drive.get('name', 'Unknown'),
                    'driveType': drive.get('driveType', 'documentLibrary')
                })
            return jsonify({"drives": drives})
        else:
            return jsonify({"error": f"API error: {response.status_code}", "details": response.text}), response.status_code
    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/admin/api/sp/sites/<site_id>/drives/<drive_id>/items', methods=['GET'])
def list_drive_items(site_id, drive_id):
    if 'username' not in session: return jsonify({"error": "Unauthorized"}), 401
    
    token = get_sp_access_token()
    if not token:
        return jsonify({"error": "Not authenticated to SharePoint"}), 401
    
    headers = {'Authorization': f'Bearer {token}', 'Accept': 'application/json'}
    
    # Get item_id from query param (defaults to root)
    item_id = request.args.get('item_id', 'root')
    
    try:
        url = f'https://graph.microsoft.com/v1.0/sites/{site_id}/drives/{drive_id}/items/{item_id}/children'
        response = requests.get(url, headers=headers, timeout=10)
        if response.status_code == 200:
            data = response.json()
            items = []
            for item in data.get('value', []):
                items.append({
                    'id': item['id'],
                    'name': item.get('name', 'Unknown'),
                    'isFolder': 'folder' in item,
                    'webUrl': item.get('webUrl', '')
                })
            return jsonify({"items": items})
        else:
            return jsonify({"error": f"API error: {response.status_code}", "details": response.text}), response.status_code
    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/admin/api/projects', methods=['POST'])
def create_project():
    if 'username' not in session:
        return jsonify({"error": "Unauthorized"}), 401
        
    data = request.json
    name = data.get('name')
    work_area_path = data.get('work_area_path')
    flatten_uploads = data.get('flatten_uploads', True)
    sp_site_id = data.get('sp_site_id', '').strip()
    sp_drive_id = data.get('sp_drive_id', '').strip()
    sp_folder_id = data.get('sp_folder_id', '').strip()
    
    if not name or not work_area_path:
        return jsonify({"error": "Name and Work Area Path are required"}), 400
        
    # Ensure physical path exists
    os.makedirs(work_area_path, exist_ok=True)
        
    project = db.create_project(name, work_area_path, flatten_uploads, sp_site_id, sp_drive_id, sp_folder_id)
    return jsonify(project)

@app.route('/admin/api/links', methods=['POST'])
def create_link():
    if 'username' not in session:
        return jsonify({"error": "Unauthorized"}), 401
        
    data = request.json
    project_id = data.get('project_id')
    
    if not project_id:
        return jsonify({"error": "Project ID is required"}), 400
        
    link = db.create_link(project_id, session['username'])
    return jsonify(link)

# --- Public Routes ---

@app.route('/s/<link_uuid>')
def public_upload(link_uuid):
    link = db.get_link(link_uuid)
    if not link:
        return "Invalid or expired share link", 404
        
    project = db.get_project_by_id(link['project_id'])
    if not project:
        return "Project not found", 404
    
    # Check if upload page is enabled (default to True for backward compatibility)
    upload_enabled = db.get_setting('upload_page_enabled', 'true').lower() == 'true'
        
    return render_template('upload.html', project=project, link_uuid=link_uuid, upload_enabled=upload_enabled)

@app.route('/s/<link_uuid>/api/explore')
def explore_project(link_uuid):
    link = db.get_link(link_uuid)
    if not link:
        return jsonify({"error": "Invalid link"}), 404
        
    project = db.get_project_by_id(link['project_id'])
    if not project:
        return jsonify({"error": "Project not found"}), 404
        
    rel_path = request.args.get('path', '').strip('/')
    flatten_uploads = project.get('flatten_uploads', True)
    
    if flatten_uploads:
        rel_path = ''
        
    base_dir = project['work_area_path']
    target_dir = os.path.join(base_dir, rel_path)
    
    # Security check
    if not os.path.abspath(target_dir).startswith(os.path.abspath(base_dir)):
        return jsonify({"error": "Invalid path"}), 403
        
    if not os.path.exists(target_dir):
        return jsonify({"folders": [], "files": []})
        
    folders = []
    files = []
    
    db_files = db.get_files_for_project(project['id'])
    
    for item in os.listdir(target_dir):
        item_path = os.path.join(target_dir, item)
        item_rel_path = os.path.join(rel_path, item).replace('\\', '/').strip('/')
        
        if os.path.isdir(item_path):
            if not flatten_uploads:
                folders.append({
                    "name": item,
                    "path": item_rel_path
                })
        else:
            db_info = next((f for f in db_files if f['filename'] == item and f.get('relative_path', '') == rel_path), None)
            uploaded_at = db_info['uploaded_at'] if db_info else None
            files.append({
                "name": item,
                "path": item_rel_path,
                "uploaded_at": uploaded_at
            })
            
    return jsonify({"folders": sorted(folders, key=lambda x: x['name'].lower()), "files": sorted(files, key=lambda x: x['name'].lower())})

@app.route('/s/<link_uuid>/api/folder', methods=['POST'])
def create_folder(link_uuid):
    link = db.get_link(link_uuid)
    if not link:
        return jsonify({"error": "Invalid link"}), 404
        
    project = db.get_project_by_id(link['project_id'])
    if not project:
        return jsonify({"error": "Project not found"}), 404
        
    data = request.json or {}
    rel_path = data.get('path', '').strip('/')
    folder_name = data.get('name', '').strip()
    flatten_uploads = project.get('flatten_uploads', True)
    
    if flatten_uploads:
        return jsonify({"error": "Folder creation is disabled when flatten uploads is enabled"}), 400
        
    if not folder_name:
        return jsonify({"error": "Folder name required"}), 400
        
    base_dir = project['work_area_path']
    # Use secure_filename to prevent path traversal in the new folder name
    target_dir = os.path.join(base_dir, rel_path, secure_filename(folder_name))
    
    if not os.path.abspath(target_dir).startswith(os.path.abspath(base_dir)):
        return jsonify({"error": "Invalid path"}), 403
        
    os.makedirs(target_dir, exist_ok=True)
    return jsonify({"message": "Folder created"})

@app.route('/s/<link_uuid>/upload_binary', methods=['POST'])
def handle_binary_upload(link_uuid):
    """Binary upload endpoint for PowerApps/Power Automate integration"""
    link = db.get_link(link_uuid)
    if not link:
        return jsonify({"error": "Invalid link"}), 404
        
    project = db.get_project_by_id(link['project_id'])
    if not project:
        return jsonify({"error": "Project not found"}), 404
    
    # Get filename and path from headers
    filename = request.headers.get('X-Filename')
    relative_path = request.headers.get('X-Path', '').strip('/')
    
    if not filename:
        return jsonify({"error": "No filename provided"}), 400
    
    filename = secure_filename(filename)
    flatten_uploads = project.get('flatten_uploads', True)
    
    if flatten_uploads:
        relative_path = ''
    
    # Security check
    work_area_path = project['work_area_path']
    target_dir = os.path.join(work_area_path, relative_path)
    if not os.path.abspath(target_dir).startswith(os.path.abspath(work_area_path)):
        return jsonify({"error": "Invalid path"}), 403
    
    # Get binary data
    file_data = request.get_data()
    
    if not file_data:
        return jsonify({"error": "No file data received"}), 400
    
    # Save to temp
    temp_path = os.path.join(app.config['UPLOAD_FOLDER'], filename)
    with open(temp_path, 'wb') as f:
        f.write(file_data)
    
    # Calculate MD5
    md5_hash = hashlib.md5()
    with open(temp_path, "rb") as f:
        for chunk in iter(lambda: f.read(4096), b""):
            md5_hash.update(chunk)
    file_md5 = md5_hash.hexdigest()
    
    # Check duplicates
    existing_file = db.get_file(project['id'], filename, relative_path)
    if existing_file and existing_file['md5'] == file_md5:
        os.remove(temp_path)
        return jsonify({
            "message": "File unchanged, upload skipped.", 
            "filename": filename,
            "status": "skipped"
        })
    
    # Move to work area
    os.makedirs(target_dir, exist_ok=True)
    final_path = os.path.join(target_dir, filename)
    shutil.copy2(temp_path, final_path)
    os.remove(temp_path)
    
    # Update database
    db.add_or_update_file(project['id'], filename, file_md5, relative_path)
    
    return jsonify({
        "message": "File uploaded successfully", 
        "filename": filename,
        "status": "uploaded"
    })

@app.route('/s/<link_uuid>/upload', methods=['POST'])
def handle_upload(link_uuid):
    link = db.get_link(link_uuid)
    if not link:
        return jsonify({"error": "Invalid link"}), 404
        
    project = db.get_project_by_id(link['project_id'])
    if not project:
        return jsonify({"error": "Project not found"}), 404
        
    if 'file' not in request.files:
        return jsonify({"error": "No file part"}), 400
        
    file = request.files['file']
    if file.filename == '':
        return jsonify({"error": "No selected file"}), 400
        
    # Handle folder upload: client can pass 'path' form field (e.g. "myfolder/subfolder")
    # or the relative path might be encoded in the file payload. We'll use the 'path' form field + 'webkitRelativePath' logic.
    relative_path = request.form.get('path', '').strip('/')
    flatten_uploads = project.get('flatten_uploads', True)
    if flatten_uploads:
        relative_path = ''
        
    # If the file object itself has a nested path via secure_filename (we'll just use the basename of what's passed)
    # The client will pass `path` indicating where to put it.
    
    if file:
        filename = secure_filename(file.filename)
        
        # Security check on path
        work_area_path = project['work_area_path']
        target_dir = os.path.join(work_area_path, relative_path)
        if not os.path.abspath(target_dir).startswith(os.path.abspath(work_area_path)):
            return jsonify({"error": "Invalid path"}), 403
        
        # 1. Save to temp
        temp_path = os.path.join(app.config['UPLOAD_FOLDER'], filename)
        file.save(temp_path)
        
        # 2. Calculate MD5 hash
        md5_hash = hashlib.md5()
        with open(temp_path, "rb") as f:
            for chunk in iter(lambda: f.read(4096), b""):
                md5_hash.update(chunk)
        file_md5 = md5_hash.hexdigest()
        
        # 3. Check for duplicates in DB
        existing_file = db.get_file(project['id'], filename, relative_path)
        if existing_file and existing_file['md5'] == file_md5:
            # File is exactly the same, skip upload
            os.remove(temp_path)
            return jsonify({
                "message": "File unchanged, upload skipped.", 
                "filename": filename,
                "status": "skipped"
            })
        
        # 4. Move to Work Area path
        os.makedirs(target_dir, exist_ok=True)
        final_path = os.path.join(target_dir, filename)
        
        # Copy to work area (handle overwriting if file already exists)
        shutil.copy2(temp_path, final_path)
        os.remove(temp_path) # Clean up temp
        
        # 5. Update Database
        db.add_or_update_file(project['id'], filename, file_md5, relative_path)
        
        return jsonify({
            "message": "File uploaded successfully", 
            "filename": filename,
            "status": "uploaded"
        })

@app.route('/s/<link_uuid>/file/<filename>', methods=['DELETE'])
def delete_file(link_uuid, filename):
    link = db.get_link(link_uuid)
    if not link:
        return jsonify({"error": "Invalid link"}), 404
        
    project = db.get_project_by_id(link['project_id'])
    if not project:
        return jsonify({"error": "Project not found"}), 404
        
    relative_path = request.args.get('path', '').strip('/')
    
    # Security check
    work_area_path = project['work_area_path']
    target_dir = os.path.join(work_area_path, relative_path)
    if not os.path.abspath(target_dir).startswith(os.path.abspath(work_area_path)):
        return jsonify({"error": "Invalid path"}), 403
        
    # Remove from DB
    db.remove_file(project['id'], filename, relative_path)
    
    # Remove physical file if it exists
    file_path = os.path.join(target_dir, filename)
    if os.path.exists(file_path):
        try:
            os.remove(file_path)
        except OSError:
            pass # Ignore if locked by Adept or system
            
    return jsonify({"message": "File deleted successfully"})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)
