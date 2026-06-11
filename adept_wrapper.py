
/app/adept_wrapper.py (full file — 260 lines)
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
    
    def get_vaults(self):
        """Get list of all vaults visible to the current Adept login.

        NxVault only exposes Id + VaultType, so the human-readable name has to
        be pulled from the VLT table record (see AltGui/Form1.cs::FindVault).
        """
        if not self.project:
            print("[GET_VAULTS] No project")
            return []

        vaults = []
        try:
            vault_list = self.project.VLGUManager.VaultList
            count = vault_list.GetCount()
            print(f"[GET_VAULTS] VaultList.GetCount() = {count}")

            rec = self.project.NewRecord()
            rec.Create("VLT")
            for i in range(count):
                v = vault_list.GetItem(i)
                if v is None:
                    continue
                name = ""
                try:
                    rec.Clear()
                    rec.SetStringVal("S_VAULTID", v.Id)
                    if rec.Fetch(0) == 0:
                        name = rec.GetStringVal("S_NAME")
                except Exception as e:
                    print(f"[GET_VAULTS] Could not fetch VLT name for {v.Id}: {e}")
                vaults.append({
                    "id": v.Id,
                    "name": name,
                    "vault_type": int(v.VaultType) if v.VaultType is not None else None,
                })
        except Exception as e:
            print(f"[GET_VAULTS] Error: {type(e).__name__}: {e}")
            import traceback
            traceback.print_exc()
        return vaults

    def get_libraries(self, vault_id=None):
        """Get list of libraries from Adept, optionally filtered to one vault.

        Per the Adept SDK (Synergis.AdeptAPI.chm -> NxLibraryList):
          * LibraryList.GetCount() reads ONLY the client-side cache, which is
            empty after a fresh login. That is why the previous implementation
            saw zero items.
          * LibraryList.SrvGetCount() asks the server for the list, populates
            the cache, and returns the total. After that, GetItem(i) returns
            fully-hydrated NxLibrary objects (Id, VaultId, Name, Path, ...).

        Filtering by vault is just `lib.VaultId == vault_id`; the SDK does not
        expose a per-vault enumerator. For targeted lookups instead of a full
        listing, use SrvFindName / SrvFindPath / SrvTypomaticXml.
        """
        if not self.project:
            print("[GET_LIBRARIES] No project")
            return []

        libraries = []
        try:
            lib_list = self.project.VLGUManager.LibraryList

            # Force the server fetch first - this is the line that was missing.
            srv_count = lib_list.SrvGetCount()
            cached_count = lib_list.GetCount()
            print(f"[GET_LIBRARIES] SrvGetCount={srv_count}, GetCount={cached_count}")

            for i in range(cached_count):
                lib = lib_list.GetItem(i)
                if lib is None:
                    continue
                if vault_id and lib.VaultId != vault_id:
                    continue
                libraries.append({
                    "id": lib.Id,
                    "vault_id": lib.VaultId,
                    "name": lib.Name,
                    "path": lib.Path,
                    "folder": lib.Folder,
                    "comment1": lib.Comment1,
                    "comment2": lib.Comment2,
                    "comment3": lib.Comment3,
                    "launchable": bool(lib.bLaunchable),
                    "virtual": bool(lib.bVirtual),
                    "history_depth": int(lib.HistoryDepth) if lib.HistoryDepth is not None else None,
                    "revision_depth": int(lib.RevisionDepth) if lib.RevisionDepth is not None else None,
                })
        except Exception as e:
            print(f"[GET_LIBRARIES] Error: {type(e).__name__}: {e}")
            import traceback
            traceback.print_exc()

        print(f"[GET_LIBRARIES] Returning {len(libraries)} libraries"
              + (f" for vault {vault_id}" if vault_id else ""))
        return libraries
    
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
