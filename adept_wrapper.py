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
        dl = self.core.DomainList
        domain = dl.FindName(domain_name)
        if not domain:
            return False
            
        try:
            # Adept login (AltLogin)
            login = domain.AltLogin(username, password, 1, 'en-US')
            if login is not None:
                domain.Logout(login.LoginName)
                return True
        except Exception:
            pass
        return False
