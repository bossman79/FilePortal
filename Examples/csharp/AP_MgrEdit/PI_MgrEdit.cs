using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Interop.AdeptCAC;
using Interop.AdeptGui;
using Synergis.Adept.MainApi;

namespace AP_MgrEdit
{
	// If you copy this project change the guid in the PI_*.cs file and in the AssemblyInfo.cs file.
	// From the Visual Studio Tools menu, pick 'Create Guid'. Select 'Registry Format', click 'New Guid', then click 'Copy'.
	// Paste the value here and remove the { and }.
	[Guid("AAC4FB2C-D867-4A42-9E95-0F8DE0A07B7B"),
	ClassInterface(ClassInterfaceType.AutoDispatch),
	ComVisible(true)]
	public class PI_MgrEdit : INxPlugInInterface, INxPlugInGuiEvents
	{
		#region Class Members
		const string c_PlugInName = "Mgr Edit PlugIn";
		const string c_CommandName_PlugInDesciption = "&Mgr Edit PlugIn...";
		const string c_CommandName_MgrEdit = "&Mgr Edit...";

		string _PlugInId;
		NxProject _Project;
		GuiApi _GuiApi;

		int _CommandNumber_Administration;
		int _CommandNumber_About;
		int _CommandNumber_MgrEdit;
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

				// GuiApi will be null if the program running the Core is not Adept.exe
				if (_GuiApi != null)
				{
					// Initialize Menu Utilities
					MenuUtils.Init(_GuiApi);

					// add menu items
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Admin, MenuUtils.g_Local_MenuName_Admin_PlugInExtractionConfiguration, c_CommandName_PlugInDesciption, ref _CommandNumber_Administration);
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Help, MenuUtils.g_Local_MenuName_Help_AboutPlugIns, c_CommandName_PlugInDesciption, ref _CommandNumber_About);
					MenuUtils.AddMenuPopupBefore(_GuiApi, MenuUtils.g_Local_MenuName_Tools, "", MenuUtils.g_Local_MenuName_Window);
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_MgrEdit, ref _CommandNumber_MgrEdit);
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
					MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_MgrEdit);
					MenuUtils.RemovePopupMenuIfEmpty(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK);
					MenuUtils.RemovePopupMenuIfEmpty(_GuiApi, MenuUtils.g_Local_MenuName_Tools, "");
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
			return ApiTypes.eTrue;
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
					MessageBox.Show("Mgr Edit Administration");
				}
				else if (CommandNumber == _CommandNumber_About)
				{
                    Synergis.Adept.MainApi.SimpleAbout about = new Synergis.Adept.MainApi.SimpleAbout();
					about._PlugInName = c_PlugInName;
					about._Assy = Assembly.GetExecutingAssembly();
					about.ShowDialog();
				}
				else if(CommandNumber == _CommandNumber_MgrEdit)
				{
					MgrEditForm mef = new MgrEditForm();
					mef._Project = _Project;
					mef.ShowDialog();
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
			return ApiTypes.eContinue;
		}

		#endregion

	}
}
