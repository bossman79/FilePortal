# PowerApps Upload Page Implementation Guide

This guide shows how to recreate the Adept Portal upload functionality in PowerApps.

## Prerequisites
- The link UUID from your Adept Portal (e.g., `/s/abc123`)
- Your portal base URL (e.g., `http://192.168.1.100:5000`)

---

## 1. Screen Setup

Create a new screen called `UploadScreen` with these controls:

### Controls to Add:
1. **Label** - `lblProjectName` (project title)
2. **Gallery** - `galFolders` (show folders)
3. **Gallery** - `galFiles` (show files)
4. **Button** - `btnUpload` (trigger file picker)
5. **Attachment** control - `attUpload` (for file selection)
6. **Button** - `btnCreateFolder` (optional, for creating folders)
7. **Label** - `lblCurrentPath` (show current location)
8. **Button** - `btnRefresh` (reload file list)
9. **Label** - `lblStatus` (show upload status)

---

## 2. Variables and Collections

### OnVisible property of UploadScreen:

```power-fx
// Set your portal configuration
Set(varPortalBaseURL, "http://192.168.1.100:5000");
Set(varLinkUUID, "your-link-uuid-here");  // Get this from the share link

// Initialize current path
Set(varCurrentPath, "");

// Load project info and files
Set(varProjectInfo, 
    JSON(
        UploadAPI_GetExplore.Run(varLinkUUID, varCurrentPath),
        JSONFormat.IgnoreBinaryData
    )
);

// Refresh file and folder lists
ClearCollect(colFolders, varProjectInfo.folders);
ClearCollect(colFiles, varProjectInfo.files);
Set(varUploadStatus, "Ready");
```

---

## 3. Custom Connectors / Power Automate Flows

PowerApps can't directly call your REST API with file uploads easily. You have **two options**:

### **OPTION A: Power Automate Flows (Recommended)**

Create 3 flows in Power Automate:

#### Flow 1: **GetExploreData**
- **Trigger**: PowerApps
- **Input**: LinkUUID (text), Path (text)
- **Action**: HTTP Request
  - Method: `GET`
  - URI: `@{triggerBody()?['BaseURL']}/s/@{triggerBody()?['LinkUUID']}/api/explore?path=@{triggerBody()?['Path']}`
- **Response**: Respond to PowerApps with JSON

#### Flow 2: **UploadFile**
- **Trigger**: PowerApps
- **Input**: LinkUUID (text), Path (text), FileName (text), FileContent (file)
- **Action**: HTTP Request
  - Method: `POST`
  - URI: `@{triggerBody()?['BaseURL']}/s/@{triggerBody()?['LinkUUID']}/upload`
  - Body: Form data with file
  - Headers: `Content-Type: multipart/form-data`
- **Response**: Respond to PowerApps with result

#### Flow 3: **DeleteFile**
- **Trigger**: PowerApps
- **Input**: LinkUUID (text), FileName (text), Path (text)
- **Action**: HTTP Request
  - Method: `DELETE`
  - URI: `@{triggerBody()?['BaseURL']}/s/@{triggerBody()?['LinkUUID']}/file/@{triggerBody()?['FileName']}?path=@{triggerBody()?['Path']}`
- **Response**: Respond to PowerApps with result

---

### **OPTION B: Custom Connector (Advanced)**

Create a Custom Connector in PowerApps:

1. Go to **Data** → **Custom Connectors** → **New Custom Connector** → **Create from blank**
2. Name it "AdeptPortalAPI"
3. Host: Your portal IP (e.g., `192.168.1.100:5000`)
4. Base URL: `/`

**Add these operations:**

#### Operation 1: ExploreFiles
- **Verb**: GET
- **URL**: `/s/{linkUUID}/api/explore`
- **Parameters**: 
  - `linkUUID` (path, required)
  - `path` (query, optional)
- **Response**: Sample JSON from your API

#### Operation 2: UploadFile
- **Verb**: POST
- **URL**: `/s/{linkUUID}/upload`
- **Parameters**:
  - `linkUUID` (path, required)
  - `file` (body, file)
  - `path` (formData, optional)
- **Content-Type**: `multipart/form-data`

#### Operation 3: DeleteFile
- **Verb**: DELETE
- **URL**: `/s/{linkUUID}/file/{filename}`
- **Parameters**:
  - `linkUUID` (path, required)
  - `filename` (path, required)
  - `path` (query, optional)

---

## 4. Control Formulas

### lblProjectName (Label)
**Text property:**
```power-fx
"Project: " & varProjectInfo.project.name
```

### lblCurrentPath (Label)
**Text property:**
```power-fx
If(IsBlank(varCurrentPath), "Root", "Path: " & varCurrentPath)
```

---

### galFolders (Gallery - Vertical)
**Items property:**
```power-fx
colFolders
```

**Layout:** Use a template with:
- Icon (icon: Folder)
- Label showing `ThisItem.name`

**OnSelect property of folder item:**
```power-fx
// Navigate into folder
Set(varCurrentPath, ThisItem.path);

// Reload files
Set(varProjectInfo, 
    JSON(
        UploadAPI_GetExplore.Run(varLinkUUID, varCurrentPath),
        JSONFormat.IgnoreBinaryData
    )
);

ClearCollect(colFolders, varProjectInfo.folders);
ClearCollect(colFiles, varProjectInfo.files);
```

---

### galFiles (Gallery - Vertical)
**Items property:**
```power-fx
colFiles
```

**Template contents:**
- **Label** - File name: `ThisItem.name`
- **Label** - Upload date: `ThisItem.uploaded_at`
- **Icon** - Delete button (icon: Trash)

**OnSelect of delete icon:**
```power-fx
// Call delete API
UploadAPI_DeleteFile.Run(varLinkUUID, ThisItem.name, varCurrentPath);

// Remove from collection
Remove(colFiles, ThisItem);

Set(varUploadStatus, "File deleted: " & ThisItem.name);

// Refresh
Timer(2000, 
    Set(varUploadStatus, "Ready")
)
```

---

### btnUpload (Button)
**Text property:**
```power-fx
"📁 Upload Files"
```

**OnSelect property:**
```power-fx
// Show attachment control
Set(varShowUploader, true)
```

---

### attUpload (Attachments control)
**Visible property:**
```power-fx
varShowUploader
```

**OnAddFile property:**
```power-fx
// Upload each added file
ForAll(
    attUpload.Attachments,
    UploadAPI_UploadFile.Run(
        varLinkUUID,
        varCurrentPath,
        Value.Name,
        Value.Value
    )
);

// Clear attachments
Reset(attUpload);
Set(varShowUploader, false);

// Show status
Set(varUploadStatus, "Uploading " & CountRows(attUpload.Attachments) & " file(s)...");

// Refresh file list after 2 seconds
Timer(2000,
    Set(varProjectInfo, 
        JSON(
            UploadAPI_GetExplore.Run(varLinkUUID, varCurrentPath),
            JSONFormat.IgnoreBinaryData
        )
    );
    ClearCollect(colFiles, varProjectInfo.files);
    Set(varUploadStatus, "Upload complete!");
)
```

---

### btnRefresh (Button)
**Text property:**
```power-fx
"🔄 Refresh"
```

**OnSelect property:**
```power-fx
Set(varProjectInfo, 
    JSON(
        UploadAPI_GetExplore.Run(varLinkUUID, varCurrentPath),
        JSONFormat.IgnoreBinaryData
    )
);

ClearCollect(colFolders, varProjectInfo.folders);
ClearCollect(colFiles, varProjectInfo.files);
Set(varUploadStatus, "Refreshed");
```

---

### btnCreateFolder (Button) - Optional
**Text property:**
```power-fx
"+ New Folder"
```

**OnSelect property:**
```power-fx
// Show input dialog
Set(varNewFolderName, "");
// Use a popup or navigate to input screen
```

If you want folder creation, you'll need another flow for the `/api/folder` POST endpoint.

---

## 5. Power Automate Flow Details

Here's the detailed flow for **UploadFile**:

### Flow: UploadFile_PowerApps

1. **Trigger**: PowerApps (V2)
   - Add inputs:
     - `BaseURL` (Text)
     - `LinkUUID` (Text) 
     - `Path` (Text)
     - `FileName` (Text)
     - `FileContent` (File)

2. **Initialize Variable**: `FormBoundary`
   - Type: String
   - Value: `----WebKitFormBoundary7MA4YWxkTrZu0gW`

3. **Compose**: Build multipart body
   ```
   --@{variables('FormBoundary')}
   Content-Disposition: form-data; name="path"

   @{triggerBody()?['Path']}
   --@{variables('FormBoundary')}
   Content-Disposition: form-data; name="file"; filename="@{triggerBody()?['FileName']}"
   Content-Type: application/octet-stream

   @{triggerBody()?['FileContent']}
   --@{variables('FormBoundary')}--
   ```

4. **HTTP**: 
   - Method: `POST`
   - URI: `@{triggerBody()?['BaseURL']}/s/@{triggerBody()?['LinkUUID']}/upload`
   - Headers:
     ```json
     {
       "Content-Type": "multipart/form-data; boundary=@{variables('FormBoundary')}"
     }
     ```
   - Body: `@{outputs('Compose')}`

5. **Response**: 
   - Respond to PowerApps
   - Body: `@{body('HTTP')}`

---

## 6. Simplified Alternative: Use HTTP Request Directly in PowerFx

If you enable the **HTTP connector** in PowerApps (may require premium):

### Upload Button OnSelect:
```power-fx
// Direct HTTP call for file explore
Set(varExploreResult,
    JSON(
        HTTP.Get(
            varPortalBaseURL & "/s/" & varLinkUUID & "/api/explore?path=" & varCurrentPath
        ).Body,
        JSONFormat.IgnoreBinaryData
    )
);

ClearCollect(colFolders, varExploreResult.folders);
ClearCollect(colFiles, varExploreResult.files);
```

**Note**: Direct file upload via HTTP in PowerApps is complex. Power Automate is strongly recommended for file uploads.

---

## 7. Alternative: Embed in PowerApps using IFrame

The simplest solution might be to embed your existing web page:

1. Add **HTML Text** control
2. Set **HtmlText** property to:
```power-fx
"<iframe src='" & varPortalBaseURL & "/s/" & varLinkUUID & "' width='100%' height='800px' frameborder='0'></iframe>"
```

This embeds your entire upload page directly in PowerApps!

---

## 8. Recommended Approach

**For quickest implementation**: Use the **IFrame approach** (option 7)

**For native PowerApps experience**: Use **Power Automate flows** (option A in section 3) with the control formulas in section 4.

**For enterprise/production**: Build a **Custom Connector** (option B) for better reusability.

---

## Need Help?

If you need the actual `.msapp` file or specific flow JSON exports, let me know which approach you want to use and I can provide more detailed step-by-step instructions!
