import json
import os
import uuid
from datetime import datetime

class PortalDB:
    def __init__(self, db_path='data.json'):
        self.db_path = db_path
        self.data = self._load()

    def _load(self):
        if os.path.exists(self.db_path):
            try:
                with open(self.db_path, 'r') as f:
                    data = json.load(f)
                    if "settings" not in data:
                        data["settings"] = {}
                    return data
            except Exception:
                pass
        return {
            "settings": {},
            "projects": [], # { "id": str, "name": str, "work_area_path": str, "created_at": str, "flatten_uploads": bool }
            "links": [],    # { "uuid": str, "project_id": str, "created_by": str, "created_at": str }
            "files": []     # { "project_id": str, "filename": str, "md5": str, "uploaded_at": str }
        }

    def _save(self):
        with open(self.db_path, 'w') as f:
            json.dump(self.data, f, indent=4)

    def set_setting(self, key, value):
        if "settings" not in self.data:
            self.data["settings"] = {}
        self.data["settings"][key] = value
        self._save()

    def get_setting(self, key, default=None):
        if "settings" not in self.data:
            return default
        return self.data["settings"].get(key, default)

    def create_project(self, name, work_area_path, flatten_uploads=True, sp_site_id='', sp_drive_id='', sp_folder_id='', 
                      adept_work_area_id='', adept_library_id='', auto_checkin=False, 
                      name_filter_pattern='', file_type_filter=''):
        project = {
            "id": str(uuid.uuid4()),
            "name": name,
            "work_area_path": work_area_path,
            "flatten_uploads": flatten_uploads,
            "sp_site_id": sp_site_id,
            "sp_drive_id": sp_drive_id,
            "sp_folder_id": sp_folder_id,
            "sp_synced_ids": [],
            "adept_work_area_id": adept_work_area_id,
            "adept_library_id": adept_library_id,
            "auto_checkin": auto_checkin,
            "name_filter_pattern": name_filter_pattern,
            "file_type_filter": file_type_filter,
            "created_at": datetime.now().isoformat()
        }
        self.data["projects"].append(project)
        self._save()
        return project
    
    def update_project(self, project_id, **kwargs):
        """Update project fields"""
        for p in self.data["projects"]:
            if p["id"] == project_id:
                for key, value in kwargs.items():
                    p[key] = value
                self._save()
                return p
        return None
    
    def delete_project(self, project_id):
        """Delete a project and its associated links"""
        self.data["projects"] = [p for p in self.data["projects"] if p["id"] != project_id]
        self.data["links"] = [l for l in self.data["links"] if l["project_id"] != project_id]
        self.data["files"] = [f for f in self.data["files"] if f["project_id"] != project_id]
        self._save()

    def get_projects(self):
        return self.data["projects"]

    def get_project_by_id(self, project_id):
        for p in self.data["projects"]:
            if p["id"] == project_id:
                return p
        return None

    def create_link(self, project_id, username):
        link = {
            "uuid": str(uuid.uuid4()),
            "project_id": project_id,
            "created_by": username,
            "created_at": datetime.now().isoformat()
        }
        self.data["links"].append(link)
        self._save()
        return link

    def get_links(self):
        return self.data["links"]

    def get_link(self, link_uuid):
        for l in self.data["links"]:
            if l["uuid"] == link_uuid:
                return l
        return None

    def get_links_for_project(self, project_id):
        return [l for l in self.data["links"] if l["project_id"] == project_id]

    def add_or_update_file(self, project_id, filename, file_md5, relative_path=""):
        if "files" not in self.data:
            self.data["files"] = []
            
        for f in self.data["files"]:
            if f["project_id"] == project_id and f["filename"] == filename and f.get("relative_path", "") == relative_path:
                f["md5"] = file_md5
                f["uploaded_at"] = datetime.now().isoformat()
                self._save()
                return f
                
        new_file = {
            "project_id": project_id,
            "filename": filename,
            "relative_path": relative_path,
            "md5": file_md5,
            "uploaded_at": datetime.now().isoformat()
        }
        self.data["files"].append(new_file)
        self._save()
        return new_file

    def get_file(self, project_id, filename, relative_path=""):
        if "files" not in self.data:
            return None
        for f in self.data["files"]:
            if f["project_id"] == project_id and f["filename"] == filename and f.get("relative_path", "") == relative_path:
                return f
        return None

    def get_files_for_project(self, project_id):
        if "files" not in self.data:
            return []
        return [f for f in self.data["files"] if f["project_id"] == project_id]

    def remove_file(self, project_id, filename, relative_path=""):
        if "files" not in self.data:
            return
        self.data["files"] = [f for f in self.data["files"] if not (f["project_id"] == project_id and f["filename"] == filename and f.get("relative_path", "") == relative_path)]
        self._save()
