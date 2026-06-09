using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Interop.AdeptCAC;
using Interop.AdeptGui;
using Synergis.Adept.MainApi;

namespace AP_LibCard
{
	// If you copy this project change the guid in the PI_*.cs file and in the AssemblyInfo.cs file.
	// From the Visual Studio Tools menu, pick 'Create Guid'. Select 'Registry Format', click 'New Guid', then click 'Copy'.
	// Paste the value here and remove the { and }.
	[Guid("8E0CC9FC-066A-4813-917B-A1BF6927C1ED"),
	ClassInterface(ClassInterfaceType.AutoDispatch),
	ComVisible(true)]
	public class PI_LibCard : INxPlugInInterface, INxPlugInGuiEvents
	{
		#region Class Members
		const string c_IniFileNE = "LibCardConfig.ini";
		const string c_PlugInName = "LibCard PlugIn";
		const string c_CommandName_PlugInDesciption = "&LibCard PlugIn...";

		string _PlugInId;
		NxProject _Project;
		GuiApi _GuiApi;

		int _CommandNumber_Administration;
		int _CommandNumber_About;

		string _IniPNE = "";
		Dictionary<string, string> _FieldsToDisallowDictionary = new Dictionary<string, string>();
		Dictionary<string, string> _FieldsToCustomUiDictionary = new Dictionary<string, string>();
        string _LoseFocus = "";
        string _LoseFocusUpdate = "";
		#endregion

		#region INxPlugInInterface
		int INxPlugInInterface.Initialize(string PlugInId, NxProject Project, object GuiApiObj)
		{
			try
			{
				// remember these
				_PlugInId = PlugInId;
				_Project = Project;
				_GuiApi = (GuiApi)GuiApiObj;

                // Localization
                string CurrentCulture = _Project.Login.Core.GetCulture();
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(CurrentCulture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                MainApi_Strings.Culture = ci;

				_IniPNE = Path.Combine(_Project.TmpDirectory, c_IniFileNE);
				PIUtils.WritePlugInMemoToFile(c_PlugInName, _PlugInId, _Project, "MUSER1", _IniPNE);

				int i = 0;
				string tmp = "";

				i = 0;
				tmp = IniUtils.GetPPS(_IniPNE, "FIELDS_TO_DISALLOW", i.ToString(), "");
				while(!string.IsNullOrEmpty(tmp))
				{
					_FieldsToDisallowDictionary.Add(tmp.ToUpper(), tmp);
					i++;
					tmp = IniUtils.GetPPS(_IniPNE, "FIELDS_TO_DISALLOW", i.ToString(), "");
				}

				i = 0;
				tmp = IniUtils.GetPPS(_IniPNE, "FIELDS_TO_CUSTOM_UI", i.ToString(), "");
				while (!string.IsNullOrEmpty(tmp))
				{
					_FieldsToCustomUiDictionary.Add(tmp.ToUpper(), tmp);
					i++;
					tmp = IniUtils.GetPPS(_IniPNE, "FIELDS_TO_CUSTOM_UI", i.ToString(), "");
				}

                _LoseFocus = IniUtils.GetPPS(_IniPNE, "Main", "LoseFocus");
                _LoseFocusUpdate = IniUtils.GetPPS(_IniPNE, "Main", "LoseFocusUpdate");

				// GuiApi will be null if the program running the Core is not Adept.exe
				if (_GuiApi != null)
				{
					// Initialize Menu Utilities
					MenuUtils.Init(_GuiApi);

					// add menu items
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Admin, MenuUtils.g_Local_MenuName_Admin_PlugInExtractionConfiguration, c_CommandName_PlugInDesciption, ref _CommandNumber_Administration);
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Help, MenuUtils.g_Local_MenuName_Help_AboutPlugIns, c_CommandName_PlugInDesciption, ref _CommandNumber_About);
				}
			}
			catch (Exception ex)
			{
				string exMsg;
				exMsg = String.Format("{0} had an exception: {1}", c_PlugInName, ex.Message);
				// don't use Windows Message Box, because we may be running in silent mode 
				_Project.Login.Core.CoreInterface.MsgBox(exMsg, c_PlugInName, (int)MessageBoxButtons.OK);
				return ApiTypes.eFalse;
			}
			return ApiTypes.eTrue;
		}

		void INxPlugInInterface.Uninitialize()
		{
			try
			{
				if (_GuiApi != null)
				{
					// remove menu items
					MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Admin, MenuUtils.g_Local_MenuName_Admin_PlugInExtractionConfiguration, c_CommandName_PlugInDesciption);
					MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Help, MenuUtils.g_Local_MenuName_Help_AboutPlugIns, c_CommandName_PlugInDesciption);
				}
			}
			catch (Exception ex)
			{
				string exMsg;
				exMsg = String.Format("{0} had an exception: {1}", c_PlugInName, ex.Message);
				// don't use Windows Message Box, because we may be running in silent mode
				_Project.Login.Core.CoreInterface.MsgBox(exMsg, c_PlugInName, (int)MessageBoxButtons.OK);
			}
		}
		#endregion

		#region INxPlugInGuiEvents

		int INxPlugInGuiEvents.AllowLibCardEnable(int LibCardMode, NxDocRecord DocRec, string FieldName, int bForUpdateDocument, ref int pbExtractSupported, ref int pbUpdateDocumentSupported)
		{
			string UpperFieldName = FieldName;
			UpperFieldName.ToUpper();
			if(_FieldsToDisallowDictionary.ContainsKey(UpperFieldName))
				return ApiTypes.eDisallow;

			return ApiTypes.eAllow;
		}

		int INxPlugInGuiEvents.BeginACommand(int CommandNumber, string CommandName, NxTbl WindowTbl, NxDetailedList DetailedList)
		{
			return ApiTypes.eContinue;
		}

		void INxPlugInGuiEvents.BeginCustomCommand(int CommandNumber, NxTbl WindowTbl, NxDetailedList DetailedList)
		{
			// It is OK to use Windows Message Box, because Gui events are fired from the UI.
			try 
			{	        
				if(CommandNumber == _CommandNumber_Administration)
				{
					LibCardConfig lcc = new LibCardConfig();
					lcc._FieldsToDisallowDictionary = _FieldsToDisallowDictionary;
					lcc._FieldsToCustomUiDictionary = _FieldsToCustomUiDictionary;
					if(lcc.ShowDialog() == DialogResult.OK)
					{
						int i = 0;

						i = 0;
						foreach(string tmp in _FieldsToDisallowDictionary.Values)
						{
							IniUtils.SetPPS(_IniPNE, "FIELDS_TO_DISALLOW", i.ToString(), tmp);
							i++;
						}
						IniUtils.SetPPS(_IniPNE, "FIELDS_TO_DISALLOW", i.ToString(), "");

						i = 0;
						foreach (string tmp in _FieldsToCustomUiDictionary.Values)
						{
							IniUtils.SetPPS(_IniPNE, "FIELDS_TO_CUSTOM_UI", i.ToString(), tmp);
							i++;
						}
						IniUtils.SetPPS(_IniPNE, "FIELDS_TO_CUSTOM_UI", i.ToString(), "");

                        _LoseFocus = lcc._LoseFocus;
                        _LoseFocusUpdate = lcc._LoseFocusUpdate;
                        IniUtils.SetPPS(_IniPNE, "Main", "LoseFocus", _LoseFocus);
                        IniUtils.SetPPS(_IniPNE, "Main", "LoseFocusUpdate", _LoseFocusUpdate);

						PIUtils.WritePlugInFileToMemo(c_PlugInName, _PlugInId, _Project, "MUSER1", _IniPNE);
					}
				}
				if (CommandNumber == _CommandNumber_About)
				{
                    Synergis.Adept.MainApi.SimpleAbout about = new Synergis.Adept.MainApi.SimpleAbout();
					about._PlugInName = c_PlugInName;
					about._Assy = Assembly.GetExecutingAssembly();
					about.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				string exMsg = String.Format("{0} had an exception: {1}", c_PlugInName, ex.Message);
				MessageBox.Show(exMsg);
			}
		}

		void INxPlugInGuiEvents.EndACommand(int CommandNumber, string CommandName, NxTbl WindowTbl, NxDetailedList DetailedList)
		{
			
		}

		void INxPlugInGuiEvents.EndCustomCommand(int CommandNumber, NxTbl WindowTbl, NxDetailedList DetailedList)
		{
			
		}

		int INxPlugInGuiEvents.LibraryCardFieldGetFocus(string FieldName)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInGuiEvents.LibraryCardFieldLoseFocus(string FieldName)
		{
            if(!string.IsNullOrEmpty(_LoseFocus) && string.Compare(FieldName, _LoseFocus) == 0)
            {
                if (!string.IsNullOrEmpty(_LoseFocusUpdate))
                {
                    string FieldValue = "";
                    int es = _GuiApi.GetLibraryCardFieldValue(_LoseFocus, ref FieldValue);
                    string NewValue = FieldValue + " : AP_LibCard";
                    es = _GuiApi.SetLibraryCardFieldValue(_LoseFocusUpdate, NewValue);
                    DialogResult dr = MessageBox.Show("Return True?", "Set to: " + NewValue, MessageBoxButtons.YesNo);
                    if(dr == DialogResult.Yes)
                        return ApiTypes.eTrue;
                }

            }
			return ApiTypes.eContinue;
		}

		#endregion

		#region Events not in an Interface yet

		public int LibcardFieldCustomUserInterfaceCheck(string FieldName)
		{
			string UpperFieldName = FieldName;
			UpperFieldName.ToUpper();
			if (_FieldsToCustomUiDictionary.ContainsKey(UpperFieldName))
				return 1;

			return 0;
		}

		public string LibcardFieldOnCustomUserInterface(string FieldName, string OrigValue, ref int pxbHasNewValue, ref int pxbRefreshLibCard)
		{
			string rtn = "";
			pxbHasNewValue = 0;
			pxbRefreshLibCard = 0;

			CustomUi cui = new CustomUi();
			cui._Project = _Project;
			cui._GuiApi = _GuiApi;
			cui._FieldName = FieldName;
			cui._NewValue = OrigValue;
			DialogResult dr = cui.ShowDialog();
			pxbRefreshLibCard = cui._bRefreshLibCard ? 1 : 0;
			if (dr == DialogResult.OK)
			{
				pxbHasNewValue = cui._bHasNewValue ? 1 : 0;
				rtn = cui._NewValue;
			}

			return rtn;
		}

		#endregion

	}
}
