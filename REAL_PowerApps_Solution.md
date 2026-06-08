# PowerApps File Upload - THE REAL SOLUTION

Based on actual PowerApps capabilities and the proven Matthew Devaney tutorial.

---

## THE TRUTH About PowerApps File Upload

**What actually works:**
1. **Add Picture control** - For uploading images (jpg, png, etc.)
2. **Power Automate flow** - Required to save files to SharePoint
3. **Image data** - Passed as base64 dataURI from PowerApps

**What DOESN'T work:**
- ❌ No "Attachments control" outside of Forms connected to SharePoint lists
- ❌ No direct file upload from PowerApps to custom APIs easily
- ❌ No simple HTTP POST with files from PowerApps

---

## Your Best Options

### OPTION A: Use SharePoint as Intermediate Storage (RECOMMENDED)

Instead of posting directly to your Flask API, use SharePoint:

1. **PowerApps** → Uploads file via Power Automate → **SharePoint Document Library**
2. **Your Flask App** → Reads files from SharePoint → **Processes them**

This is the standard Microsoft way and actually works reliably.

### OPTION B: Embed Your Web Page in PowerApps (EASIEST - RECOMMENDED)

Your upload page already works perfectly. Just embed it in PowerApps using an iframe.

#### Step 1: Get Your Public URL from Portal Dashboard

1. Go to your Adept Portal dashboard (as admin)
2. Look at the **Portal Public URL** setting - this is your public IP
3. Create a project and generate a share link
4. Copy the full link (e.g., `http://207.67.98.3:5000/s/abc123xyz`)

#### Step 2: Add to PowerApps

1. In PowerApps Studio, add an **HTML text** control
2. Set the **HtmlText** property to:

```powerfx
"<iframe src='http://207.67.98.3:5000/s/YOUR-LINK-UUID' width='100%' height='100%' frameborder='0' style='border:none;'></iframe>"
```

**Replace:**
- `207.67.98.3` with your actual public IP
- `YOUR-LINK-UUID` with the UUID from your share link

#### Step 3: Configure Control

**HTML Text Control Properties:**
- **X**: `0`
- **Y**: `0`  
- **Width**: `Parent.Width`
- **Height**: `Parent.Height`

**That's it!** Your users get:
- ✅ File upload (works exactly like the web page)
- ✅ File browsing
- ✅ Folder creation (if enabled)
- ✅ File deletion
- ✅ All existing functionality

#### Making It Dynamic (Optional)

If you want to switch between projects, use a variable:

**OnVisible of Screen:**
```powerfx
Set(varPublicURL, "http://207.67.98.3:5000");
Set(varLinkUUID, "your-default-uuid");
```

**HTML Text HtmlText Property:**
```powerfx
"<iframe src='" & varPublicURL & "/s/" & varLinkUUID & "' width='100%' height='100%' frameborder='0' style='border:none;'></iframe>"
```

Then you can change `varLinkUUID` to show different projects.

#### Advantages of This Approach:
- ⚡ Zero development time
- 🔒 Uses your existing authentication/security
- 📱 Works on mobile PowerApps
- 🚫 No Power Automate flows needed
- 💾 Files go directly to your Adept work areas
- 🔄 SharePoint sync still works if configured

#### Important: Public IP vs LAN IP

Your portal must be accessible from wherever users run the PowerApp:
- **LAN IP** (192.168.x.x) - Only works on same network
- **Public IP** - Works from anywhere (make sure port 5000 is forwarded in your router)

The portal dashboard shows your public IP in the settings. Use that!

### OPTION C: Use Power Apps Portals / Power Pages

If you need external users to upload files, consider using Power Pages (formerly PowerApps Portals). They have built-in file upload capabilities.

---

## If You MUST Use Option A - Here's The Real Flow

### Step 1: Create SharePoint Document Library

1. Go to your SharePoint site
2. Create new Document Library: "AdeptUploads"
3. Add columns:
   - ProjectID (text)
   - UploadedBy (text)
   - Status (choice: New, Processed)

### Step 2: Power Automate Flow

**Flow Name:** UploadImageToSharePoint

**Trigger:** PowerApps (V2)
- Add input: `Picture` (File type) - REQUIRED
- Add input: `FileName` (Text) - REQUIRED
- Add input: `ProjectID` (Text) - REQUIRED

**Action 1:** Create file (SharePoint)
- Site: Your SharePoint site
- Folder Path: `/AdeptUploads`
- File Name: `FileName` from trigger
- File Content: `Picture` from trigger (it handles base64 automatically)

**Action 2:** Update file properties (SharePoint)
- Site: Your SharePoint site
- Library: AdeptUploads
- Id: Use "Identifier" from previous Create file step
- ProjectID: `ProjectID` from trigger
- Status: "New"

**Action 3:** Respond to PowerApp
- Add text output: "FileID"
- Value: ID from Create file step

### Step 3: PowerApps Code

**Add Picture Control** (name: `uplImage`)

**Button to Upload** (OnSelect):
```powerfx
UploadImageToSharePoint.Run(
    {
        contentBytes: uplImage.Image,
        name: Text(Now(), "yyyymmdd_hhmmss") & ".jpg"
    },
    Text(Now(), "yyyymmdd_hhmmss") & ".jpg",
    "YOUR-PROJECT-ID"
);

Notify("Image uploaded to SharePoint!", NotificationType.Success)
```

**CRITICAL:** The file input must be a record/object with `contentBytes` and `name` properties.

### Step 4: Flask App Reads from SharePoint

Your Flask app would need to:
1. Connect to SharePoint using Microsoft Graph API or SharePoint REST API
2. Poll for new files (Status = "New")
3. Download them
4. Process them
5. Update Status to "Processed"

This requires SharePoint authentication in your Flask app.

---

## The Honest Assessment

**If you're trying to replace your web upload page with PowerApps**, here's what I recommend:

### For Internal Users (on your network):
Use **Option B** (iframe embedding). Your web page already works. Don't rebuild it.

### For External Users or Mobile:
1. Keep your existing web upload page
2. Make it mobile-responsive if needed
3. OR use Power Pages for a proper external portal

### For Full PowerApps Integration:
Use SharePoint as intermediate storage (Option A above), but understand this adds complexity and doesn't directly integrate with your Flask app without significant work.

---

## What Matthew Devaney's Tutorial Actually Shows

The tutorial I found shows:
1. Add Picture control in PowerApps
2. Power Automate flow with PowerApps V2 trigger
3. Trigger accepts FILE input type
4. Uses SharePoint "Create file" action
5. File content is passed directly (Power Automate handles base64 conversion)

**Key Formula from Real Tutorial:**
```powerfx
'FlowName'.Run(
    {
        contentBytes: img_SingleImage_Image.Image,
        name: Text(Now(), "yyMMddhhmmss")
    },
    txt_Caption.Text
)
```

The record structure `{contentBytes, name}` is required for file type inputs.

---

## Sorry for the Confusion

I should have researched properly before giving you instructions. PowerApps file handling is more limited than I suggested:

- Can't easily POST files to custom REST APIs
- Requires Power Automate for most file operations
- Works best with SharePoint/OneDrive/Dataverse

Your Flask app would need significant changes to work with the Microsoft ecosystem, or you'd need to use SharePoint as a bridge.

**My recommendation:** Stick with your web interface (Option B iframe) or keep it separate. PowerApps isn't the right tool for this specific use case unless you're willing to add SharePoint to your architecture.

