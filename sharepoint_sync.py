import os
import time
import requests
import msal
import threading
import hashlib
from datetime import datetime
from database import PortalDB

db = PortalDB()

class SPSyncEngine:
    def __init__(self):
        self.running = False
        self.thread = None

    def start(self):
        if not self.running:
            self.running = True
            self.thread = threading.Thread(target=self._loop, daemon=True)
            self.thread.start()
            print("SharePoint Sync Engine started.")

    def stop(self):
        self.running = False
        if self.thread:
            self.thread.join(timeout=2)

    def _get_access_token(self):
        # Refresh db instance to get latest settings
        db._load()
        
        # 1. Try Interactive Device Code Cache
        token_cache_str = db.get_setting('sp_token_cache')
        if token_cache_str:
            client_id = db.get_setting('sp_device_client_id', '04b07795-8ddb-461a-bbee-02f9e1bf7b46')
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
                    # For interactive flows, the scopes must match what was authorized, typically .default isn't ideal but we can try it,
                    # or we can pass the explicit scopes used in device flow.
                    result = app.acquire_token_silent(["Sites.ReadWrite.All", "Files.ReadWrite.All"], account=account)
                    if result and "access_token" in result:
                        if cache.has_state_changed:
                            db.set_setting('sp_token_cache', cache.serialize())
                        return result["access_token"]
                except Exception as e:
                    print(f"MSAL Silent Auth Error: {e}")

        # 2. Fallback to Client Credentials
        tenant_id = db.get_setting('sp_tenant_id')
        client_id = db.get_setting('sp_client_id')
        client_secret = db.get_setting('sp_client_secret')

        if not all([tenant_id, client_id, client_secret]):
            return None

        try:
            authority = f"https://login.microsoftonline.com/{tenant_id}"
            app = msal.ConfidentialClientApplication(
                client_id, authority=authority, client_credential=client_secret
            )
            result = app.acquire_token_for_client(scopes=["https://graph.microsoft.com/.default"])
            if "access_token" in result:
                return result["access_token"]
        except Exception as e:
            print(f"MSAL Auth Error: {e}")
        return None

    def _loop(self):
        while self.running:
            try:
                self.sync_all_projects()
            except Exception as e:
                print(f"Sync error: {e}")
            
            # Wait 60 seconds before checking again
            for _ in range(60):
                if not self.running:
                    break
                time.sleep(1)

    def sync_all_projects(self):
        token = self._get_access_token()
        if not token:
            return

        db._load()
        projects = db.get_projects()
        for proj in projects:
            sp_site_id = proj.get('sp_site_id')
            sp_drive_id = proj.get('sp_drive_id')
            sp_folder_id = proj.get('sp_folder_id')

            # Only sync if all SP parameters are provided
            if sp_site_id and sp_drive_id and sp_folder_id:
                self.sync_project(proj, token)

    def sync_project(self, project, token):
        headers = {
            'Authorization': f'Bearer {token}',
            'Accept': 'application/json'
        }
        site_id = project['sp_site_id']
        drive_id = project['sp_drive_id']
        folder_id = project['sp_folder_id']

        synced_ids = project.get('sp_synced_ids', [])
        
        # List children of the folder
        url = f"https://graph.microsoft.com/v1.0/sites/{site_id}/drives/{drive_id}/items/{folder_id}/children"
        
        try:
            response = requests.get(url, headers=headers, timeout=10)
            if response.status_code == 200:
                data = response.json()
                changes_made = False
                
                for item in data.get('value', []):
                    item_id = item['id']
                    # Process files that haven't been synced yet
                    if item_id not in synced_ids and 'file' in item:
                        download_url = item.get('@microsoft.graph.downloadUrl')
                        if download_url:
                            if self.download_file(project, item, download_url):
                                synced_ids.append(item_id)
                                changes_made = True
                
                if changes_made:
                    db._load()
                    for p in db.data['projects']:
                        if p['id'] == project['id']:
                            p['sp_synced_ids'] = synced_ids
                            break
                    db._save()
            else:
                print(f"SP API Error for {project['name']}: {response.status_code} - {response.text}")
        except Exception as e:
            print(f"Error syncing project {project['name']}: {e}")

    def download_file(self, project, item_data, download_url):
        filename = item_data['name']
        work_area = project['work_area_path']
        
        # Since it's from SharePoint and we flatten uploads by default for Adept
        target_path = os.path.join(work_area, filename)
        
        print(f"Downloading {filename} from SharePoint to {target_path}...")
        
        try:
            with requests.get(download_url, stream=True, timeout=30) as r:
                r.raise_for_status()
                with open(target_path, 'wb') as f:
                    for chunk in r.iter_content(chunk_size=8192): 
                        f.write(chunk)
                        
            # Calculate MD5 hash
            md5_hash = hashlib.md5()
            with open(target_path, "rb") as f:
                for chunk in iter(lambda: f.read(4096), b""):
                    md5_hash.update(chunk)
            file_md5 = md5_hash.hexdigest()
            
            # Register in database (Adept Work Area)
            db._load()
            db.add_file(project['id'], filename, file_md5, relative_path='')
            print(f"Successfully synced {filename} into Adept Portal DB")
            return True
        except Exception as e:
            print(f"Failed to download {filename}: {e}")
            return False

# Global instance
sync_engine = SPSyncEngine()
