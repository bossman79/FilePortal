import clr
import sys
import os

ADEPT_DIR = r'C:\Program Files (x86)\Synergis\Adept\Client'

def setup_adept():
    if ADEPT_DIR not in sys.path:
        sys.path.append(ADEPT_DIR)
    clr.AddReference(os.path.join(ADEPT_DIR, 'Interop.AdeptCAC'))
    
def create_com_object(dotnet_class):
    from System import Activator
    return Activator.CreateInstance(clr.GetClrType(dotnet_class))

class AdeptWrapper:
    def __init__(self):
        setup_adept()
        from Interop.AdeptCAC import NxCoreClass
        self.core = create_com_object(NxCoreClass)
        result = self.core.Initialize(0)
        if result != 0:
            raise Exception(f"Failed to initialize Adept Core. Error code: {result}")
        self.project = None
        self.login = None
            
    def get_domains(self):
        domains = []
        dl = self.core.DomainList
        for idx in range(dl.GetCount()):
            d = dl.GetItem(idx)
            domains.append({
                "name": d.Name,
                "last_login": d.LastLoginName
            })
        return domains
        
    def authenticate(self, domain_name, username, password):
        """Authenticates an Adept user. Returns True if successful, False otherwise."""
        try:
            dl = self.core.DomainList
            domain = dl.FindName(domain_name)
            if not domain:
                print(f"[ADEPT_AUTH] Domain '{domain_name}' not found")
                return False
            
            print(f"[ADEPT_AUTH] Found domain: {domain_name}")
            print(f"[ADEPT_AUTH] Calling AltLogin for user: {username}")
            
            # Adept login (AltLogin)
            login = domain.AltLogin(username, password, 1, 'en-US')
            
            print(f"[ADEPT_AUTH] AltLogin returned: {login}")
            
            if login is not None:
                self.login = login
                self.project = login.Project
                print(f"[ADEPT_AUTH] Login object set, Project: {self.project}")
                return True
            else:
                print(f"[ADEPT_AUTH] AltLogin returned None")
                return False
        except Exception as e:
            print(f"[ADEPT_AUTH] Exception during authentication: {type(e).__name__}: {e}")
            import traceback
            traceback.print_exc()
            return False
            pass
        return False
    
    def get_work_areas(self):
        """Get list of all work areas from Adept"""
        if not self.project:
            print("[GET_WORK_AREAS] No project")
            return []
        
        work_areas = []
        try:
            print("[GET_WORK_AREAS] Calling OpenWorkAreaListTable...")
            wa_table = self.project.OpenWorkAreaListTable("acacwal")
            if not wa_table:
                print("[GET_WORK_AREAS] OpenWorkAreaListTable returned None")
                return []
            
            print(f"[GET_WORK_AREAS] Table opened, calling GetCount (NOT SrvGetCount)...")
            count = wa_table.GetCount()
            print(f"[GET_WORK_AREAS] Count: {count}")
            
            for i in range(1, count + 1):
                es = wa_table.GotoRecord(i)
                if es == 0:
                    wa_id = wa_table.GetString("S_PATHID")
                    wa_path = wa_table.GetString("S_PATH")
                    wa_comment = wa_table.GetString("S_COMMENT")
                    print(f"[GET_WORK_AREAS] Record {i}: {wa_id} - {wa_path}")
                    work_areas.append({
                        "id": wa_id,
                        "path": wa_path,
                        "comment": wa_comment
                    })
        except Exception as e:
            print(f"[GET_WORK_AREAS] Error: {type(e).__name__}: {e}")
            import traceback
            traceback.print_exc()
        
        print(f"[GET_WORK_AREAS] Returning {len(work_areas)} work areas")
        return work_areas
    
    def get_libraries(self):
        """Get list of all libraries from Adept
        
        Note: The Adept API's LibraryList cache is only populated when libraries 
        are browsed in the Adept GUI client. Since we can't directly query the 
        VLT table via the API, users must enter library IDs manually in the portal.
        
        To find library IDs: Open Adept client > File Guide > right-click library > Properties
        """
        if not self.project:
            print("[GET_LIBRARIES] No project")
            return []
        
        print("[GET_LIBRARIES] Note: Library enumeration requires GUI interaction.")
        print("[GET_LIBRARIES] Returning empty - users must enter library IDs manually.")
        print("[GET_LIBRARIES] Library IDs can be found in Adept client > File Guide > Library Properties")
        
        # Return empty list - users will enter library IDs manually
        return []
    
    def get_work_area_files(self, work_area_id):
        """Get files in a specific work area"""
        if not self.project:
            return []
        
        files = []
        try:
            wa_table = self.project.OpenWorkAreaTable(work_area_id, f"acacwa{work_area_id}")
            if not wa_table:
                return []
            
            count = wa_table.SrvGetCount()
            if count > 0:
                wa_table.SrvScroll(1, min(count, 100))  # Limit to 100 files
                for i in range(1, wa_table.GetCount() + 1):
                    wa_table.GotoRecord(i)
                    files.append({
                        "filename": wa_table.GetString("S_LONGNAME"),
                        "path": wa_table.GetString("S_PATH"),
                        "filetype": wa_table.GetString("S_FILETYPE"),
                        "status": wa_table.GetString("S_STATUS")
                    })
        except Exception as e:
            print(f"Error getting work area files: {e}")
        
        return files
    
    def check_in_file(self, file_path, library_id, filename=None):
        """Check in a file from work area to Adept library"""
        if not self.project:
            return {"success": False, "error": "Not connected to Adept"}
        
        try:
            # Create a new document record
            rec = self.project.NewRecord()
            rec.Create("fm100fil")  # File table
            
            if not filename:
                filename = os.path.basename(file_path)
            
            # Set file properties
            rec.SetStringVal("S_LONGNAME", filename)
            rec.SetStringVal("S_LIBNAME", library_id)
            rec.SetStringVal("S_PATH", file_path)
            
            # Append the record
            result = rec.Append()
            if result == 0:
                return {"success": True, "filename": filename, "library": library_id}
            else:
                return {"success": False, "error": f"Append failed with code {result}"}
        except Exception as e:
            return {"success": False, "error": str(e)}
