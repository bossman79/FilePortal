import os
import sys

import clr

ADEPT_DIR = r"C:\Program Files (x86)\Synergis\Adept\Client"


def setup_adept():
    if ADEPT_DIR not in sys.path:
        sys.path.append(ADEPT_DIR)
    clr.AddReference(os.path.join(ADEPT_DIR, "Interop.AdeptCAC"))


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
            domains.append({"name": d.Name, "last_login": d.LastLoginName})
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

            login = domain.AltLogin(username, password, 1, "en-US")

            print(f"[ADEPT_AUTH] AltLogin returned: {login}")

            if login is not None:
                self.login = login
                self.project = login.Project
                print(f"[ADEPT_AUTH] Login object set, Project: {self.project}")
                return True
            else:
                print("[ADEPT_AUTH] AltLogin returned None")
                return False
        except Exception as e:
            print(
                f"[ADEPT_AUTH] Exception during authentication: {type(e).__name__}: {e}"
            )
            import traceback

            traceback.print_exc()
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

            print(
                "[GET_WORK_AREAS] Table opened, calling GetCount (NOT SrvGetCount)..."
            )
            count = wa_table.GetCount()
            print(f"[GET_WORK_AREAS] Count: {count}")

            for i in range(1, count + 1):
                es = wa_table.GotoRecord(i)
                if es == 0:
                    wa_id = wa_table.GetString("S_PATHID")
                    wa_path = wa_table.GetString("S_PATH")
                    wa_comment = wa_table.GetString("S_COMMENT")
                    print(f"[GET_WORK_AREAS] Record {i}: {wa_id} - {wa_path}")
                    work_areas.append(
                        {"id": wa_id, "path": wa_path, "comment": wa_comment}
                    )
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
                vaults.append(
                    {
                        "id": v.Id,
                        "name": name,
                        "vault_type": int(v.VaultType)
                        if v.VaultType is not None
                        else None,
                    }
                )
        except Exception as e:
            print(f"[GET_VAULTS] Error: {type(e).__name__}: {e}")
            import traceback

            traceback.print_exc()
        return vaults

    def get_libraries(self, vault_id=None):
        """Get list of libraries from Adept, optionally filtered to one vault.

        Adept SDK quirks:
          * LibraryList.GetCount() / GetItem(i) only return data the GUI client
            has already browsed - on a fresh login they are empty.
          * LibraryList.SrvGetCount() asks the server how many libraries exist
            but does NOT fill the client cache that GetItem(i) reads from.
          * The reliable way to enumerate is LibraryList.WriteXml(path, sortBy,
            options): it asks the server for the whole list and dumps it to an
            XML file we can then parse. (SrvTypomaticXml is for single-field
            filtered lookups, not full enumeration.)

        For targeted lookups skip this and use SrvFindName / SrvFindPath.
        """
        if not self.project:
            print("[GET_LIBRARIES] No project")
            return []

        import tempfile
        import xml.etree.ElementTree as ET

        libraries = []
        xml_path = None
        try:
            lib_list = self.project.VLGUManager.LibraryList

            srv_count = lib_list.SrvGetCount()
            print(f"[GET_LIBRARIES] SrvGetCount={srv_count}")

            # Dump full server-side library list to a temp XML file.
            # SortBy: ADEPTT_COREALIAS - LIBPATH_ALIAS is a safe default.
            # Options: 0 (unused per the SDK docs).
            fd, xml_path = tempfile.mkstemp(prefix="adept_libs_", suffix=".xml")
            os.close(fd)
            es = lib_list.WriteXml(xml_path, 0, "")
            print(f"[GET_LIBRARIES] WriteXml -> {xml_path} (es={es})")

            if not os.path.exists(xml_path) or os.path.getsize(xml_path) == 0:
                print("[GET_LIBRARIES] WriteXml produced no file")
                return []

            tree = ET.parse(xml_path)
            root = tree.getroot()

            # The XML schema is a flat list of <Library .../> or <Lib .../>
            # elements; field names come from fm***Lib. We pull attributes
            # AND child elements, since the SDK can emit either form.
            def field(el, *names):
                for n in names:
                    v = el.get(n)
                    if v not in (None, ""):
                        return v
                    child = el.find(n)
                    if child is not None and child.text not in (None, ""):
                        return child.text
                return ""

            # Iterate any descendant that looks like a library record.
            candidates = []
            for el in root.iter():
                tag = el.tag.lower()
                if tag in ("library", "lib", "nxlibrary") or tag.endswith("library"):
                    candidates.append(el)
            # Fallback: if nothing matched by tag name, treat direct children as records.
            if not candidates:
                candidates = list(root)

            for el in candidates:
                lib_id = field(el, "Id", "ID", "S_LIBID", "LibId")
                lib_vault = field(el, "VaultId", "S_VAULTID")
                lib_name = field(el, "Name", "S_LIBNAME", "S_NAME")
                lib_path = field(el, "Path", "S_PATH", "S_LIBPATH")
                lib_folder = field(el, "Folder", "S_FOLDER")
                lib_c1 = field(el, "Comment1", "S_COMMENT1")
                lib_c2 = field(el, "Comment2", "S_COMMENT2")
                lib_c3 = field(el, "Comment3", "S_COMMENT3")

                # Skip rows that don't look like libraries at all
                if not (lib_id or lib_name or lib_path):
                    continue
                if vault_id and lib_vault and lib_vault != vault_id:
                    continue

                libraries.append(
                    {
                        "id": lib_id,
                        "vault_id": lib_vault,
                        "name": lib_name,
                        "path": lib_path,
                        "folder": lib_folder,
                        "comment1": lib_c1,
                        "comment2": lib_c2,
                        "comment3": lib_c3,
                    }
                )
        except Exception as e:
            print(f"[GET_LIBRARIES] Error: {type(e).__name__}: {e}")
            import traceback

            traceback.print_exc()
        finally:
            if xml_path and os.path.exists(xml_path):
                print(f"[GET_LIBRARIES] XML kept at: {xml_path}")
                # try:
                #  os.remove(xml_path)
                # except OSError:
                #   pass

        suffix = f" for vault {vault_id}" if vault_id else ""
        print(f"[GET_LIBRARIES] Returning {len(libraries)} libraries{suffix}")
        return libraries

    def get_work_area_files(self, work_area_id):
        """Get files in a specific work area"""
        if not self.project:
            return []

        files = []
        try:
            wa_table = self.project.OpenWorkAreaTable(
                work_area_id, f"acacwa{work_area_id}"
            )
            if not wa_table:
                return []

            count = wa_table.SrvGetCount()
            if count > 0:
                wa_table.SrvScroll(1, min(count, 100))
                for i in range(1, wa_table.GetCount() + 1):
                    wa_table.GotoRecord(i)
                    files.append(
                        {
                            "filename": wa_table.GetString("S_LONGNAME"),
                            "path": wa_table.GetString("S_PATH"),
                            "filetype": wa_table.GetString("S_FILETYPE"),
                            "status": wa_table.GetString("S_STATUS"),
                        }
                    )
        except Exception as e:
            print(f"Error getting work area files: {e}")

        return files

    def check_in_file(self, file_path, library_id, filename=None):
        """Check in a file from work area to Adept library"""
        if not self.project:
            return {"success": False, "error": "Not connected to Adept"}

        try:
            rec = self.project.NewRecord()
            rec.Create("fm100fil")

            if not filename:
                filename = os.path.basename(file_path)

            rec.SetStringVal("S_LONGNAME", filename)
            rec.SetStringVal("S_LIBNAME", library_id)
            rec.SetStringVal("S_PATH", file_path)

            result = rec.Append()
            if result == 0:
                return {"success": True, "filename": filename, "library": library_id}
            else:
                return {"success": False, "error": f"Append failed with code {result}"}
        except Exception as e:
            return {"success": False, "error": str(e)}
