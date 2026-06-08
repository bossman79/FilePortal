# PowerApps: Embed Adept Portal Upload Page

## Quick Setup (5 Minutes)

### Step 1: Get Your Share Link

1. Open your **Adept Portal Dashboard** (login as admin)
2. Go to the project you want to share
3. Click **"+ Share Link"** button
4. **Copy the entire URL** shown (it will look like: `http://207.67.98.3:5000/s/abc123xyz`)

**Note:** The portal automatically uses your public IP address. If you need to change it, update the "Portal Public URL" in System Settings.

---

### Step 2: Create PowerApps Screen

1. Open **PowerApps Studio** (https://make.powerapps.com)
2. Create a new **Canvas app** (Tablet or Phone layout)
3. Name your app: "Adept Portal Upload"

---

### Step 3: Add HTML Control

1. Click **Insert** → **Text** → **HTML text**
2. Rename the control to: `htmlUploadPage`
3. In the **HtmlText** property (top formula bar), paste:

```powerfx
"<iframe src='http://207.67.98.3:5000/s/abc123xyz' width='100%' height='100%' frameborder='0' style='border:none;'></iframe>"
```

**Replace the URL with your actual share link from Step 1**

---

### Step 4: Size the Control

1. Select the HTML control
2. Set these properties:
   - **X**: `0`
   - **Y**: `0`
   - **Width**: `Parent.Width`
   - **Height**: `Parent.Height`

This makes it fill the entire screen.

---

### Step 5: Test It

1. Click **▶ Play** (top right corner)
2. You should see your upload page
3. Test uploading a file
4. Files should appear in your Adept work area

---

## Multiple Projects (Optional)

If you want users to select which project to upload to:

### Add a Dropdown

1. **Insert** → **Input** → **Dropdown**
2. Set **Items** property to:

```powerfx
Table(
    {Name: "Project A", UUID: "abc123xyz"},
    {Name: "Project B", UUID: "def456uvw"},
    {Name: "Project C", UUID: "ghi789rst"}
)
```

3. Set **Value** field to `Name`

### Update HTML Control

Change the **HtmlText** property to:

```powerfx
"<iframe src='http://207.67.98.3:5000/s/" & Dropdown1.Selected.UUID & "' width='100%' height='100%' frameborder='0' style='border:none;'></iframe>"
```

Now users can switch between projects using the dropdown!

---

## Troubleshooting

### "Page not loading" or blank iframe

**Possible causes:**

1. **Wrong IP address** - Your public IP may have changed
   - Go to Adept Portal Dashboard
   - Check the "Portal Public URL" setting
   - Update the URL in PowerApps

2. **Port not accessible** - Port 5000 needs to be open/forwarded
   - Check your router's port forwarding settings
   - Forward port 5000 to your Flask server's internal IP
   - Test by opening the URL in a regular browser first

3. **Flask not running** - Server needs to be started
   - Run `python app.py` on your server
   - Check that it says "Running on http://0.0.0.0:5000"

4. **HTTPS/HTTP mixed content** - PowerApps might require HTTPS
   - If PowerApps is served over HTTPS but your portal is HTTP, some browsers block it
   - Consider setting up SSL/HTTPS for your portal (optional but recommended for production)

### Test Your URL First

Before adding to PowerApps, test the URL in a regular browser:
- Open a browser
- Paste your share link
- Make sure the upload page loads
- Try uploading a test file
- If it works in the browser, it will work in PowerApps

---

## Network Requirements

### For Users on Same Network (LAN):
- Flask server running on your local machine/server
- Users on same WiFi/network
- Works with LAN IP (192.168.x.x) or public IP

### For Users Outside Your Network (Remote):
- Flask server accessible via public IP
- Router port forwarding configured (port 5000 → server IP)
- Firewall allows port 5000
- **OR** use VPN so remote users connect to your network

### For Mobile Users:
- If on company WiFi: use LAN IP
- If outside: need public IP with port forwarding
- PowerApps mobile app supports iframes

---

## What Users Can Do

Once embedded in PowerApps, users have **full functionality**:

✅ Upload single or multiple files
✅ Browse existing files  
✅ Create folders (if not flattened)
✅ Navigate folder structure
✅ Delete files
✅ See upload timestamps
✅ All security from your portal (link-based access)

**Bonus:** If you configured SharePoint sync, those files automatically sync too!

---

## Security Notes

- Each share link (UUID) is unique per project
- Links can only access the specific project they're created for
- No login required (link acts as authentication)
- To revoke access: delete the link from dashboard (future feature)
- Files go to Adept work areas, not visible to other projects

---

## Updating the URL

If your public IP changes:

1. Update "Portal Public URL" in Adept Portal Dashboard
2. Create new share links (old ones will use old IP)
3. Update the URL in your PowerApps iframe
4. **OR** use a dynamic DNS service to keep a consistent domain name

---

## Why This Works Better Than Custom Flows

**Advantages:**
- ⚡ **Instant setup** - 5 minutes vs hours of coding
- 🎯 **100% feature parity** - Everything works exactly as designed
- 🔒 **Existing security** - No new authentication to build
- 🛠️ **No maintenance** - Update portal once, all PowerApps reflect changes
- 📱 **Mobile friendly** - Your upload page already works on mobile
- 🔄 **SharePoint sync** - Keeps working as configured

**vs. Custom Power Automate Flows:**
- ❌ Complex to build
- ❌ Limited functionality
- ❌ Requires SharePoint as intermediary
- ❌ Extra points of failure
- ❌ Doesn't integrate with Adept directly

---

## You're Done! 🎉

Your PowerApps now has a fully functional file upload interface with zero coding required.
