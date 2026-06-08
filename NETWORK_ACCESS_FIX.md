# How to Make Adept Portal Accessible on Your Network

Your Flask app is configured correctly (`host='0.0.0.0'`), but Windows is blocking access. Here's how to fix it.

---

## Quick Fix (Run as Administrator)

1. **Right-click `SETUP_PUBLIC_ACCESS.bat`**
2. **Select "Run as administrator"**
3. Done! This opens the firewall for port 5000 and Python

---

## Manual Steps (if the script doesn't work)

### Step 1: Open Windows Firewall

1. Press **Windows Key**
2. Type: `Windows Defender Firewall`
3. Click **"Advanced settings"** (left sidebar)

### Step 2: Create Inbound Rule for Port 5000

1. Click **"Inbound Rules"** (left sidebar)
2. Click **"New Rule..."** (right sidebar)
3. **Rule Type**: Select **"Port"** → Next
4. **Protocol**: Select **"TCP"**
5. **Specific local ports**: Enter `5000` → Next
6. **Action**: Select **"Allow the connection"** → Next
7. **Profile**: Check all three boxes (Domain, Private, Public) → Next
8. **Name**: Enter `Adept Portal` → Finish

### Step 3: Allow Python Through Firewall

1. Still in Windows Firewall → **"Inbound Rules"**
2. Click **"New Rule..."**
3. **Rule Type**: Select **"Program"** → Next
4. **This program path**: Browse to your Python executable
   - Usually: `C:\Users\YourName\AppData\Local\Programs\Python\Python313\python.exe`
   - Or: `C:\Python313\python.exe`
5. **Action**: Select **"Allow the connection"** → Next
6. **Profile**: Check all three → Next
7. **Name**: Enter `Python - Adept Portal` → Finish

---

## Testing Access

### Test 1: Localhost (Same Computer)

1. Open browser
2. Go to: `http://localhost:5000`
3. Should see the login page

### Test 2: Local Network (Another Device on Same WiFi)

1. Find your **local IP address**:
   - Open Command Prompt
   - Type: `ipconfig`
   - Look for **IPv4 Address** (usually 192.168.x.x)

2. On another device (phone, laptop), open browser
3. Go to: `http://192.168.x.x:5000` (use your actual IP)
4. Should see the login page

### Test 3: External Access (Internet)

1. Find your **public IP address**:
   - Go to: https://whatismyipaddress.com
   - Note the IPv4 address shown

2. **Configure Router Port Forwarding**:
   - Log into your router (usually http://192.168.1.1 or http://192.168.0.1)
   - Find "Port Forwarding" or "Virtual Server" settings
   - Add new rule:
     - **External Port**: 5000
     - **Internal IP**: Your computer's local IP (from Test 2)
     - **Internal Port**: 5000
     - **Protocol**: TCP
   - Save/Apply

3. From outside your network (use mobile data or ask friend):
   - Go to: `http://YOUR_PUBLIC_IP:5000`
   - Should see login page

---

## Update Your Portal Settings

Once working:

1. Login to Adept Portal Dashboard
2. Go to **System Settings**
3. Update **Portal Public URL** to: `http://YOUR_PUBLIC_IP:5000`
4. Save Settings

Now all share links will use the correct public IP!

---

## Common Issues

### "Connection Refused" or "Can't Reach This Page"

**Cause**: Firewall is blocking
**Fix**: Re-run the setup script as Administrator, or manually add firewall rules above

### "Works Locally But Not on Network"

**Cause**: Windows Firewall or antivirus blocking
**Fix**: 
1. Temporarily disable Windows Firewall to test
2. If it works, the firewall is the issue - add the rules properly
3. Check if antivirus has its own firewall (Norton, McAfee, etc.)

### "Works on Network But Not from Internet"

**Cause**: Router port forwarding not configured
**Fix**: 
1. Log into your router
2. Set up port forwarding for port 5000
3. Make sure your ISP doesn't block port 5000 (some do)
4. Consider using a different port (like 8080) if 5000 is blocked

### "Public IP Keeps Changing"

**Cause**: Dynamic IP from ISP
**Fix**: 
1. Use a Dynamic DNS service (like No-IP, DuckDNS, Dynu)
2. Or pay your ISP for a static IP
3. Update portal settings when IP changes

---

## Security Considerations

**For Production Use:**

1. **Use HTTPS** (not HTTP)
   - Get SSL certificate (Let's Encrypt is free)
   - Use a reverse proxy like nginx or Caddy

2. **Don't expose port 5000 directly**
   - Use a reverse proxy on port 80/443
   - Flask debug mode should be OFF in production

3. **Restrict access**
   - Use firewall rules to limit which IPs can connect
   - Implement rate limiting
   - Add authentication beyond share links

4. **Use a domain name**
   - Easier to remember than IP address
   - Can use Cloudflare for DDoS protection

---

## For PowerApps Users

Once the network is accessible:

1. Get your public IP: https://whatismyipaddress.com
2. Update Portal Public URL in dashboard
3. Create a share link for your project
4. Copy the **full URL** (e.g., `http://207.67.98.3:5000/s/abc123xyz`)
5. Use that exact URL in your PowerApps iframe

**PowerApps iframe code:**
```powerfx
"<iframe src='http://YOUR_PUBLIC_IP:5000/s/YOUR_LINK_UUID' width='100%' height='100%' frameborder='0' style='border:none;'></iframe>"
```

Replace with your actual values!

---

## Still Not Working?

**Check these:**

1. ✅ Flask server is running (`python app.py`)
2. ✅ Flask says "Running on http://0.0.0.0:5000"
3. ✅ Windows Firewall rules added (run setup script)
4. ✅ Port 5000 forwarded in router (for external access)
5. ✅ Using correct IP address (local vs public)
6. ✅ Antivirus isn't blocking
7. ✅ ISP doesn't block port 5000

**Test with:**
```cmd
netstat -ano | findstr :5000
```

Should show Python listening on `0.0.0.0:5000` or `[::]:5000`

If it shows `127.0.0.1:5000` only, Flask isn't configured correctly (but yours is already correct).
