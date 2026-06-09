using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

using Interop.AdeptCAC;
using Interop.AdeptGui;
using Synergis.Adept.MainApi;


// NOTE: If the program should allow the Admin to change the list of supported file extensions, enable DYNAMIC_EXT.
// Search for DYNAMIC_EXT in the project and follow each instruction.

namespace AP_Extract
{
	// If you copy this project change the guid in the PI_*.cs file and in the AssemblyInfo.cs file.
	// From the Visual Studio Tools menu, pick 'Create Guid'. Select 'Registry Format', click 'New Guid', then click 'Copy'.
	// Paste the value here and remove the { and }.
	[Guid("5D08DBC4-4B48-49CB-9B57-C8687C8304D5"),
	ClassInterface(ClassInterfaceType.AutoDispatch),
	ComVisible(true)]
	public class PI_Extract : INxPlugInInterface, INxPlugInCoreEvents, INxPlugInGuiEvents, INxPlugInCoreCommandSLEvents
	{
		#region Class Members
		const string c_PlugInName = "Extract PlugIn";
		const string c_CommandName_PlugInDesciption = "&Extract PlugIn...";
		//const string c_CommandName_HelloWorld = "&Hello World...";

		string _PlugInId;
		NxProject _Project;
		GuiApi _GuiApi;

		//ResourceManager _rm = null;
		//string _CurrentCulture = "";
		//CultureInfo _ci = null;

		string _PlugInIniPNE = "";
		CheckList _Extensions;
		CheckList _Options;

		int _CommandNumber_Administration;
		int _CommandNumber_About;
		//int _CommandNumber_HelloWorld;

		bool _bCreateAppFailed = false;
		bool _bBOSCreatedApp = false;
		bool _bInBatchUpdateCommand = false;
		bool _bBatchUpdateCreatedApp = false;
		bool _bExtractCreatedApp = false;

		Object _oApp;

		int _DocsToExtract = 0;
		int _DocsExtracted = 0;
		int _DocsWithExtractedFields = 0;

		#endregion

		#region INxPlugInInterface
		int INxPlugInInterface.Initialize(string PlugInId, NxProject Project, object GuiApiObj)
		{
			try
			{
				// See if the supported app or extraction tool is on the machine.
				if (!ExtractUtils.TestApp())
					return ApiTypes.eFalse;

				// remember these
				_PlugInId = PlugInId;
				_Project = Project;
				_GuiApi = (GuiApi)GuiApiObj;

                // Localization
                string CurrentCulture = _Project.Login.Core.GetCulture();
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(CurrentCulture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                MainApi_Strings.Culture = ci;

				// Localization
				//Assembly assy = Assembly.GetExecutingAssembly();
				//_rm = new ResourceManager("AP_Extract.Main_Strings", assy);

				// GuiApi will be null if the program running the Core is not Adept.exe
				if (_GuiApi != null)
				{
					// Initialize Menu Utilities
					MenuUtils.Init(_GuiApi);

					// add menu items
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Admin, MenuUtils.g_Local_MenuName_Admin_PlugInExtractionConfiguration, c_CommandName_PlugInDesciption, ref _CommandNumber_Administration);
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Help, MenuUtils.g_Local_MenuName_Help_AboutPlugIns, c_CommandName_PlugInDesciption, ref _CommandNumber_About);
					//MenuUtils.AddMenuPopupBefore(_GuiApi, MenuUtils.g_Local_MenuName_Tools, "", MenuUtils.g_Local_MenuName_Window);
					//MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_HelloWorld, ref _CommandNumber_HelloWorld);
				}

				// init
				_bBOSCreatedApp = false;
				_bInBatchUpdateCommand = false;
				_bBatchUpdateCreatedApp = false;
				_bExtractCreatedApp = false;
				_oApp = null;

				_Extensions = new CheckList();

				// DYNAMIC_EXT - To enable dynamic extenions, comment out the next two lines
				_Extensions.AddItem(ExtractUtils.c_TxtExtension, false);
				_Extensions.AddItem(ExtractUtils.c_IniExtension, false);

				_Options = new CheckList();
				_Options.AddItem(ExtractUtils.c_ExtractPropertiesOption, false);
				_Options.AddItem(ExtractUtils.c_ExtractAdditionalProperitesOption, false);
				_Options.AddItem(ExtractUtils.c_UpdateDocumentOption, false);
				_Options.AddItem(ExtractUtils.c_BatchOpenAndSaveOption, false);
				_Options.AddItem(ExtractUtils.c_ControlLaunchingOption, false);

				 // get ini file
				_PlugInIniPNE = _Project.TmpDirectory + "\\" + ExtractUtils.c_PlugInIniNE;
				PIUtils.WritePlugInMemoToFile(c_PlugInName, _PlugInId, _Project, "MUSER1", _PlugInIniPNE);

				// read ini settings
				if (ExtractUtils.ReadOurIniSettings(_PlugInIniPNE, _Extensions, _Options) == false)
				{
					string msg = "The .ini file has been changed by a newer version of this application.\n";
					msg += "Please contact your administrator.";
					MessageBox.Show(msg, c_PlugInName);

					INxPlugInInterface pii = (INxPlugInInterface)this;
					pii.Uninitialize();
					return ApiTypes.eFalse;
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
					//MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_HelloWorld);
					//MenuUtils.RemovePopupMenuIfEmpty(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK);
					//MenuUtils.RemovePopupMenuIfEmpty(_GuiApi, MenuUtils.g_Local_MenuName_Tools, "");
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

		#region INxPlugInCoreEvents

		int INxPlugInCoreEvents.AllowEdit(NxDocRecord DocRec, string FieldName, int bForUpdateDocument, ref int pbExtractSupported, ref int pbUpdateDocumentSupported)
		{
			try
			{
				pbExtractSupported = 0;
				pbUpdateDocumentSupported = 0;

				string FileNE = DocRec.FindField("S_LONGNAME").StringVal;
				string FileE = System.IO.Path.GetExtension(FileNE);
				if (_Extensions.IsChecked(FileE))
				{
					string FieldId = PIUtils.GetFieldId(_Project, FieldName);

					if (PIUtils.IsValidExtractionField(ref _Project, FieldId) == true)
					{
						if (!string.IsNullOrEmpty(IniUtils.GetPPS(_PlugInIniPNE, FieldId, "R0C0")))
						{
							
							if (_Options.IsChecked(ExtractUtils.c_ExtractPropertiesOption))
								pbExtractSupported = 1;
							if (_Options.IsChecked(ExtractUtils.c_UpdateDocumentOption))
								pbUpdateDocumentSupported = 1;

							if(bForUpdateDocument == 1 && pbUpdateDocumentSupported == 1)
								return ApiTypes.eAllow;

							return ApiTypes.eDisallow;
						}
					}
				}
				return ApiTypes.eAllow;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eAllow;
		}

		int INxPlugInCoreEvents.BatchOpenAndSave(NxDocRecord DocRec, string FilePNE)
		{
			try 
			{
				if (_Options.IsChecked(ExtractUtils.c_BatchOpenAndSaveOption) == false) 
					return ApiTypes.eContinue;

	            string Extension = System.IO.Path.GetExtension(FilePNE);
				if (_Extensions.IsChecked(Extension) == false)
					return ApiTypes.eContinue;

				if (ExtractUtils.AppConnect(ref _oApp, ref _bBOSCreatedApp, ref _bCreateAppFailed) == false)
					return ApiTypes.eContinue;

				ExtractUtils.OurBatchOpenAndSave(_Project, _oApp, FilePNE);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreEvents.BeginItem(int CommandNumber, string CommandName, NxDocId DocId, NxDocRecord DocRecord)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreEvents.CheckBatchOpenAndSaveSupport(ref int pSupported, ref int pEnabled, NxSTree STree)
		{
			try 
			{	        
				string Msg = "";

				pSupported = ApiTypes.eTrue;
				if (_Options.IsChecked(ExtractUtils.c_BatchOpenAndSaveOption))
				{
					pEnabled = ApiTypes.eTrue;
					Msg = "Enabled.";
				}
				else
				{
					pEnabled = ApiTypes.eFalse;
					Msg = "Disabled.";
				}
			
				NxSNode SNode = STree.AddNode();
				SNode.StringVal = c_PlugInName + " supports Batch Open and Save.";

				Msg += " To change settings, select Admin/PlugIn Administration/Extract.";
				NxSNode SSubNode = SNode.AddNode();
				SSubNode.AddNode();
				SSubNode.StringVal = Msg;

				SSubNode = null;
				SNode = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreEvents.ClearExtractedFields(NxDocRecord DocRec)
		{
			try
			{
				if (_Options.IsChecked(ExtractUtils.c_ExtractPropertiesOption) == false) 
					return ApiTypes.eContinue;

				string FileNE = DocRec.FindField("S_LONGNAME").StringVal;
				string FileE = System.IO.Path.GetExtension(FileNE);
				if (_Extensions.IsChecked(FileE))
				{
					bool bDoUpdate = false;
					int count = DocRec.FldValList.GetCount();
					for(int i = 0; i < count; i++)
					{
						NxFldVal FldVal = DocRec.FldValList.GetItem(i);
						ApiTypes.ADEPTT_FLD_DEF_TYPE FldDefType = (ApiTypes.ADEPTT_FLD_DEF_TYPE)FldVal.FldDef.FldDefType;
						string FieldName = FldVal.FldDef.Name;
						string FieldId = PIUtils.GetFieldId(_Project, FieldName);
						if (!string.IsNullOrEmpty(IniUtils.GetPPS(_PlugInIniPNE, FieldId, "R0C0"))) 
						{
							bDoUpdate = true;
							if (FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_DATE)
							{
								FldVal.DateVal = "";
							}
							if (FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_CHARACTER) 
							{
								FldVal.CharacterVal = "";
							}
							if (FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_STRING) 
							{
								FldVal.StringVal = "";
							}
							// user fields only
							if (FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_LOGICAL) 
							{ 
								FldVal.LogicalVal = ApiTypes.eFalse;
							}
							if (FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_INTEGER) 
							{
								FldVal.IntegerVal = 0;
							}
							if (FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_DOUBLE) 
							{
								FldVal.DoubleVal = 0;
							}
							if (FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_MEMO) 
							{
								FldVal.SetMemoFromString("");
							}
						}
						FldVal = null;
					}
					if (bDoUpdate) 
					{
						DocRec.Update();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreEvents.DoesFileNeedExtraction(string Extension)
		{
			try
			{
				  // if extracting nothing, then done
				if (_Options.IsChecked(ExtractUtils.c_ExtractPropertiesOption) == false)
					return ApiTypes.eFalse;
					
				if (_Extensions.IsChecked(Extension)) 
					return ApiTypes.eTrue;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eFalse;
		}

		void INxPlugInCoreEvents.EndItem(int es, int CommandNumber, string CommandName, NxDocId DocId, NxDocRecord DocRecord, NxDocId NewDocId, NxDocRecord NewDocRecord)
		{
			
		}

		int INxPlugInCoreEvents.ExtractFile(NxDocRecord DocRec, string FilePNE)
		{
			try
			{
				 // if extracting nothing, then done
				if (_Options.IsChecked(ExtractUtils.c_ExtractPropertiesOption) == false)
					return ApiTypes.eContinue;

				string Extension = System.IO.Path.GetExtension(FilePNE);
				if (_Extensions.IsChecked(Extension) == false) 
					return ApiTypes.eContinue;

				if (ExtractUtils.AppConnect(ref _oApp, ref _bExtractCreatedApp, ref _bCreateAppFailed) == false)
					return ApiTypes.eContinue;

				ExtractUtils.OurExtractFile(_Project, _PlugInIniPNE, _oApp, DocRec, FilePNE, ref _Options, ref _DocsToExtract, ref _DocsExtracted, ref _DocsWithExtractedFields);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreEvents.IsBatchOpenAndSaveHandled(NxDocRecord DocRec)
		{
			try
			{
				if (_Options.IsChecked(ExtractUtils.c_BatchOpenAndSaveOption) == false) 
					return ApiTypes.eFalse;

				string FileNE = DocRec.FindField("S_LONGNAME").StringVal;
				string Extension = System.IO.Path.GetExtension(FileNE);
				if (_Extensions.IsChecked(Extension) == false)
					return ApiTypes.eFalse;

				return ApiTypes.eTrue;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eFalse;
		}

		int INxPlugInCoreEvents.IsUpdateDocumentDownloadNeeded(NxDocRecord DocRec)
		{
			try
			{
				if (_Options.IsChecked(ExtractUtils.c_UpdateDocumentOption) == false)
					return ApiTypes.eFalse;

				string FileNE = DocRec.FindField("S_LONGNAME").StringVal;
				string Extension = System.IO.Path.GetExtension(FileNE);
				if (_Extensions.IsChecked(Extension) == false)
					return ApiTypes.eFalse;

                int count = DocRec.FldValList.GetCount();
                for(int i = 0; i < count; i++)
				{
                    NxFldVal FldVal = DocRec.FldValList.GetItem(i);
                    string FieldName = FldVal.FldDef.Name;
                    string FieldId = PIUtils.GetFieldId(_Project, FieldName);
                    if (!string.IsNullOrEmpty(IniUtils.GetPPS(_PlugInIniPNE, FieldId, "R0C0")))
					{
                        if (FldVal.bIsDirty == 1) 
						{
                            FldVal = null;
                            return ApiTypes.eTrue;
                        }
                    }
                    FldVal = null;
                }
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eFalse;
		}

		int INxPlugInCoreEvents.IsUpdateDocumentHandled(NxDocRecord DocRec)
		{
			try
			{
				if (_Options.IsChecked(ExtractUtils.c_UpdateDocumentOption) == false)
					return ApiTypes.eFalse;
				string FileNE = DocRec.FindField("S_LONGNAME").StringVal;
				string Extension = System.IO.Path.GetExtension(FileNE);
				if (_Extensions.IsChecked(Extension) == false)
					return ApiTypes.eFalse;
				return ApiTypes.eTrue;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eFalse;
		}

		void INxPlugInCoreEvents.LibraryCardSavedToRecord(NxDocRecord DocRec)
		{
			
		}

		int INxPlugInCoreEvents.StartProgram(string LaunchApplicationId, string FilePNE)
		{
			try
			{
				if (_Options.IsChecked(ExtractUtils.c_ControlLaunchingOption) == false) 
					return ApiTypes.eContinue;

				// is there a file to open
				bool bOpenFile = false;
				// this can be empty on a "Start Program"
				if (!string.IsNullOrEmpty(FilePNE)) 
				{
					string Extension = System.IO.Path.GetExtension(FilePNE);
					if(_Extensions.IsChecked(Extension) == true) 
						bOpenFile = true;
				}

				bool bAppIsOurs = false;
				// this can be 0 on a "Default Launch"
				if (!string.IsNullOrEmpty(LaunchApplicationId))
				{
					NxLaunchApplicationDef LnchApp = _Project.TLFDefManager.LaunchApplicationDefList.FindId(LaunchApplicationId);
					string CmdLine = LnchApp.CmdLine;
					// make the command line
					string cmdBuff = "";
					PIUtils.CallExpandEnvironmentStrings(CmdLine, ref cmdBuff, 260 * 3);
					cmdBuff = cmdBuff.ToUpper();
					string OurApp = ExtractUtils.c_AppExeNE;
					OurApp.ToUpper();
					// is this OurApp.exe
					if (cmdBuff.IndexOf(OurApp) > -1) 
					{
						bAppIsOurs = true;
					}
				}
				// if the Launch App is Inventor OR
				// there is no Launch App and the File is an Inventor File
				if (bAppIsOurs || (string.IsNullOrEmpty(LaunchApplicationId) && bOpenFile)) 
				{
					ExtractUtils.OurLaunch(FilePNE);
					return ApiTypes.eHandled;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreEvents.UpdateDocument(NxDocRecord DocRec, string FilePNE)
		{
			try 
			{
				if (_Options.IsChecked(ExtractUtils.c_UpdateDocumentOption) == false) 
					return ApiTypes.eContinue;

				string Extension = System.IO.Path.GetExtension(FilePNE);
				if (_Extensions.IsChecked(Extension) == false) 
					return ApiTypes.eContinue;

				if (_bInBatchUpdateCommand) 
				{
					if (ExtractUtils.AppConnect(ref _oApp, ref _bBatchUpdateCreatedApp, ref _bCreateAppFailed) == false) 
						return ApiTypes.eContinue;
				}
				ExtractUtils.OurUpdateDocument(_Project, _PlugInIniPNE, _oApp, ref _bCreateAppFailed, DocRec, FilePNE);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return ApiTypes.eContinue;
			
		}

		#endregion

		#region INxPlugInGuiEvents

		int INxPlugInGuiEvents.AllowLibCardEnable(int LibCardMode, NxDocRecord DocRec, string FieldName, int bForUpdateDocument, ref int pbExtractSupported, ref int pbUpdateDocumentSupported)
		{
			try
			{
				if (LibCardMode == (int)ApiTypes.ADEPTT_LIBCARD_MODE.LCM_LIBCARD || LibCardMode == (int)ApiTypes.ADEPTT_LIBCARD_MODE.LCM_BATCH_UPDATE)
				{
					INxPlugInCoreEvents pice = (INxPlugInCoreEvents)this;
					return pice.AllowEdit(
						DocRec, FieldName, bForUpdateDocument, ref pbExtractSupported, ref pbUpdateDocumentSupported);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

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
				if (CommandNumber == _CommandNumber_Administration)
				{
					ExtractUtils.ShowAdministrationForm(c_PlugInName, _PlugInId, _PlugInIniPNE, _Extensions, _Options, _Project);
				}
				else if (CommandNumber == _CommandNumber_About)
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
			return ApiTypes.eContinue;
		}

		#endregion

		#region INxPlugInCoreCommandSLEvents

		int INxPlugInCoreCommandSLEvents.BeginApprove(NxSelectionList SelectionList, string WorkflowComment, int bMakeRevision, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginArchive(NxSelectionList SelectionList, string Path, string Label, int bIncludeRevisions, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginAssign(NxSelectionList SelectionList, string AssignToId, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginAssignWorkflow(NxSelectionList SelectionList, string WorkflowId, string WorkflowComment, int bAlsoSetAsDefault, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginAssumeOwnership(NxSelectionList SelectionList, string LibId, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginAssumeOwnership2(NxSelectionList SelectionList, string LibId, int bCopyLibCard, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginBatchOpenAndSave(NxSelectionList SelectionList, int bClearLogBefore, int bProcessChildren, int bShowLogAfter, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginBatchUpdate(NxSelectionList SelectionList, NxQueryTable QueryTable, int bUpdateFile, NxCommandSettings CommandSettings)
		{
			_bInBatchUpdateCommand = true;
			_bBatchUpdateCreatedApp = false;

			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginClearExtractedInfo(NxSelectionList SelectionList, int bClearExtractionFields, int bClearExtractedRelationships, int bClearThumbnail, int bClearExtractionState, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginClearRedlines(NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginClearTags(NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginCopy(NxSelectionList SelectionList, int FromTo, string DestinationId, int bCopyLibCard, ref int pOverwriteDestinationFile, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginCreateRevision(NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginDelete(NxSelectionList SelectionList, int bDeleteFileToo, ref int pDeleteReferencedFile, ref int pLoseChangesOnHold, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginExpeditorAppove(NxSelectionList SelectionList, string StepId, string WorkflowComment, int bMakeRevision, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginExpeditorReject(NxSelectionList SelectionList, string StepId, string WorkflowComment, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginExtract(NxSelectionList SelectionList, int ExtractionType, int RelationshipResolutionType, int bClearState, NxSTree STree, int bUseLibVirtSetting, NxCommandSettings CommandSettings)
		{
			_DocsToExtract = 0;
			_DocsExtracted = 0;
			_DocsWithExtractedFields = 0;

			_bExtractCreatedApp = false;

			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginLaunch(NxSelectionList SelectionList, string LaunchAppId, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginLink(NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginMove(NxSelectionList SelectionList, int FromTo, string DestinationId, ref int pOverwriteDestinationFile, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginPrintFiles(NxSelectionList SelectionList, NxPrintFilesSettings PrintFilesSettings, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginReject(NxSelectionList SelectionList, string WorkflowComment, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginRejectTo(NxSelectionList SelectionList, string StepId, string WorkflowComment, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginReleaseOwnership(NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginRename(NxSelectionList SelectionList, string NewName, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginRestore(NxSelectionList SelectionList, string Path, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginRollbackRevision(NxSelectionList SelectionList, int MajRev, int MinRev, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginSendTo(NxSelectionList SelectionList, int bSendLinks, int bSendToEmailRecipient, int bIncludeDependencies, int bZip, string ZipPassword, string FileName, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginSetDefaultWorkflow(NxSelectionList SelectionList, string WorkflowId, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginSetFileType(NxSelectionList SelectionList, string FileTypeId, int bOnlyIfNotSet, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginSignIn(NxSelectionList SelectionList, int bMakeRevision, string LibId, string AssignedToUserId, NxSelectionList pResultsSL, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginSignOut(NxSelectionList SelectionList, string WorkAreaId, ref int pOverwriteDestinationFile, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginUnassign(NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginUnlink(NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		int INxPlugInCoreCommandSLEvents.BeginViewFiles(NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			return ApiTypes.eContinue;
		}

		void INxPlugInCoreCommandSLEvents.EndApprove(int es, NxSelectionList SelectionList, string WorkflowComment, int bMakeRevision, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndArchive(int es, NxSelectionList SelectionList, string Path, string Label, int bIncludeRevisions, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndAssign(int es, NxSelectionList SelectionList, string AssignToId, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndAssignWorkflow(int es, NxSelectionList SelectionList, string WorkflowId, string WorkflowComment, int bAlsoSetAsDefault, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndAssumeOwnership(int es, NxSelectionList SelectionList, string LibId, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndAssumeOwnership2(int es, NxSelectionList SelectionList, string LibId, int bCopyLibCard, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndBatchOpenAndSave(int es, NxSelectionList SelectionList, int bClearLogBefore, int bProcessChildren, int bShowLogAfter, NxCommandSettings CommandSettings)
		{
			try
			{
				if (_bBOSCreatedApp) 
				{
					ExtractUtils.AppDisconnect(ref _oApp);
					_bBOSCreatedApp = false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
		}

		void INxPlugInCoreCommandSLEvents.EndBatchUpdate(int es, NxSelectionList SelectionList, NxQueryTable QueryTable, int bUpdateFile, NxCommandSettings CommandSettings)
		{
			try
			{ 
				if (_bBatchUpdateCreatedApp)
				{
					ExtractUtils.AppDisconnect(ref _oApp);
					_bBatchUpdateCreatedApp = false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
		}

		void INxPlugInCoreCommandSLEvents.EndClearExtractedInfo(int es, NxSelectionList SelectionList, int bClearExtractionFields, int bClearExtractedRelationships, int bClearThumbnail, int bClearExtractionState, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndClearRedlines(int es, NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndClearTags(int es, NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndCopy(int es, NxSelectionList SelectionList, int FromTo, string DestinationId, int bCopyLibCard, ref int pOverwriteDestinationFile, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndCreateRevision(int es, NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndDelete(int es, NxSelectionList SelectionList, int bDeleteFileToo, ref int pDeleteReferencedFile, ref int pLoseChangesOnHold, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndExpeditorAppove(int es, NxSelectionList SelectionList, string StepId, string WorkflowComment, int bMakeRevision, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndExpeditorReject(int es, NxSelectionList SelectionList, string StepId, string WorkflowComment, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndExtract(int es, NxSelectionList SelectionList, int ExtractionType, int RelationshipResolutionType, int bClearState, NxSTree STree, int bUseLibVirtSetting, NxCommandSettings CommandSettings)
		{
			try
			{
				 if (_bExtractCreatedApp) 
				 {
					 ExtractUtils.AppDisconnect(ref _oApp);
					_bExtractCreatedApp = false;
				 }

				if (STree != null) 
				{
					NxSNode RootSNode = STree.AddNode();
					RootSNode.StringVal = "Extract";

					NxSNode SNode = RootSNode.AddNode();
					SNode.StringVal = _DocsToExtract.ToString() + " documents to extract.";
					SNode = RootSNode.AddNode();
					SNode.StringVal = _DocsExtracted.ToString() + " extracted.";
					SNode = RootSNode.AddNode();
					SNode.StringVal = _DocsWithExtractedFields.ToString() + " with extracted fields.";

					SNode = null;
					RootSNode = null;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
		}

		void INxPlugInCoreCommandSLEvents.EndLaunch(int es, NxSelectionList SelectionList, string LaunchAppId, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndLink(int es, NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndMove(int es, NxSelectionList SelectionList, int FromTo, string DestinationId, ref int pOverwriteDestinationFile, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndPrintFiles(int es, NxSelectionList SelectionList, NxPrintFilesSettings PrintFilesSettings, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndReject(int es, NxSelectionList SelectionList, string WorkflowComment, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndRejectTo(int es, NxSelectionList SelectionList, string StepId, string WorkflowComment, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndReleaseOwnership(int es, NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndRename(int es, NxSelectionList SelectionList, string NewName, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndRestore(int es, NxSelectionList SelectionList, string Path, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndRollbackRevision(int es, NxSelectionList SelectionList, int MajRev, int MinRev, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndSendTo(int es, NxSelectionList SelectionList, int bSendLinks, int bSendToEmailRecipient, int bIncludeDependencies, int bZip, string ZipPassword, string FileName, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndSetDefaultWorkflow(int es, NxSelectionList SelectionList, string WorkflowId, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndSetFileType(int es, NxSelectionList SelectionList, string FileTypeId, int bOnlyIfNotSet, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndSignIn(int es, NxSelectionList SelectionList, int bMakeRevision, string LibId, string AssignedToUserId, NxSelectionList pResultsSL, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndSignOut(int es, NxSelectionList SelectionList, string WorkAreaId, ref int pOverwriteDestinationFile, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndUnassign(int es, NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndUnlink(int es, NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			
		}

		void INxPlugInCoreCommandSLEvents.EndViewFiles(int es, NxSelectionList SelectionList, NxCommandSettings CommandSettings)
		{
			
		}

		#endregion

	}
}
