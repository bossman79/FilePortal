import os
import sys

import clr

ADEPT_DIR = r"C:\Program Files (x86)\Synergis\Adept\Client"


def setup_adept():
    if ADEPT_DIR not in sys.path:
        sys.path.append(ADEPT_DIR)
    clr.AddReference(os.path.join(ADEPT_DIR, "Interop.AdeptCAC"))
    try:
        # For ApiTypes enums (ADEPTT_SEARCHOP, ADEPTT_COREALIAS, ...)
        clr.AddReference(os.path.join(ADEPT_DIR, "Synergis.Adept.MainApi"))
    except Exception as e:
        print(f"[ADEPT] Could not load Synergis.Adept.MainApi: {e}")


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

        Queries the LIB table (fm100lib) on the server via NxQueryTable -
        the documented SDK pattern from AP_Misc/PI_Misc.cs FieldAccessTest2,
        which queries fm100format the same way.

        Why not the other routes:
          * LibraryList.GetCount()/GetItem() and WriteXml only read/dump the
            client-side cache, which is empty on a headless login (the XML
            contained just <LibraryList/> while SrvGetCount reported 309).
          * Project.FindTbl("lib") hangs on a headless login.
          * For targeted lookups use LibraryList.SrvFindName(vault_id, name)
            or LibraryList.SrvFindPath(vault_id, path).
        """
        if not self.project:
            print("[GET_LIBRARIES] No project")
            return []

        libraries = []
        try:
            from Synergis.Adept.MainApi import ApiTypes

            qt = self.project.CreateQueryTable()
            qt.TableName = "fm100lib"
            qfi = qt.QueryFieldList.Add("S_LIBID")
            qfi.SearchOp = int(ApiTypes.ADEPTT_SEARCHOP.OP_NOTEMPTY)

            tbl = qt.Query("PortalLibraryList")
            if tbl is None:
                print("[GET_LIBRARIES] Query returned None")
                return []

            if tbl.IsServerScroll() == 1:
                srv_count = tbl.SrvGetCount()
                print(f"[GET_LIBRARIES] Server scroll, SrvGetCount={srv_count}")
                if srv_count > 0:
                    tbl.SrvScroll(1, srv_count)

            count = tbl.GetCount()
            print(f"[GET_LIBRARIES] fm100lib rows: {count}")

            for i in range(1, count + 1):
                if tbl.GotoRecord(i) != 0:
                    continue

                lib_vault = tbl.GetString("S_VAULTID")
                if vault_id and lib_vault != vault_id:
                    continue

                libraries.append(
                    {
                        "id": tbl.GetString("S_LIBID"),
                        "vault_id": lib_vault,
                        "name": tbl.GetString("S_NAME"),
                        "path": tbl.GetString("S_PATH"),
                        "folder": tbl.GetString("S_FOLDER"),
                        "comment1": tbl.GetString("S_COMMENT1"),
                        "comment2": tbl.GetString("S_COMMENT2"),
                        "comment3": tbl.GetString("S_COMMENT3"),
                    }
                )
        except Exception as e:
            print(f"[GET_LIBRARIES] Error: {type(e).__name__}: {e}")
            import traceback

            traceback.print_exc()

        suffix = f" for vault {vault_id}" if vault_id else ""
        print(f"[GET_LIBRARIES] Returning {len(libraries)} libraries{suffix}")
        return libraries

    def get_library_tree(self):
        """Build a nested library tree grouped by vault.

        Hierarchy is derived from each library's path: a library is a child
        of the library (same vault) whose path is its nearest path prefix.
        Returns [{id, name, libraries: [node]}] where each node is the
        get_libraries() dict plus a 'children' list.
        """
        vaults = self.get_vaults()
        libraries = self.get_libraries()

        def norm(path):
            return (path or "").replace("/", "\\").strip("\\").lower()

        nodes_by_vault = {}
        for lib in libraries:
            node = dict(lib)
            node["children"] = []
            vault_key = lib.get("vault_id") or ""
            nodes_by_vault.setdefault(vault_key, {})[norm(lib.get("path"))] = node

        vault_names = {v["id"]: (v.get("name") or v["id"]) for v in vaults}

        tree_vaults = []
        for vault_id, by_path in nodes_by_vault.items():
            roots = []
            # Shallowest paths first so parents exist before children attach.
            for path in sorted(by_path.keys(), key=lambda p: p.count("\\")):
                node = by_path[path]
                parent = None
                segments = path.split("\\")
                for i in range(len(segments) - 1, 0, -1):
                    candidate = "\\".join(segments[:i])
                    if candidate != path and candidate in by_path:
                        parent = by_path[candidate]
                        break
                if parent is not None:
                    parent["children"].append(node)
                else:
                    roots.append(node)

            def sort_children(n):
                n["children"].sort(key=lambda c: norm(c.get("path")))
                for c in n["children"]:
                    sort_children(c)

            roots.sort(key=lambda n: norm(n.get("path")))
            for r in roots:
                sort_children(r)

            tree_vaults.append(
                {
                    "id": vault_id,
                    "name": vault_names.get(vault_id, vault_id),
                    "libraries": roots,
                }
            )

        tree_vaults.sort(key=lambda v: (v["name"] or "").lower())
        print(f"[GET_LIBRARY_TREE] {len(tree_vaults)} vaults")
        return tree_vaults

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
