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

namespace AP_Misc
{
	// If you copy this project change the guid in the PI_*.cs file and in the AssemblyInfo.cs file.
	// From the Visual Studio Tools menu, pick 'Create Guid'. Select 'Registry Format', click 'New Guid', then click 'Copy'.
	// Paste the value here and remove the { and }.
	[Guid("ECE34E53-4D72-4876-8967-6EFE1E28A6D8"),
	ClassInterface(ClassInterfaceType.AutoDispatch),
	ComVisible(true)]
	public class PI_Misc : INxPlugInInterface, INxPlugInGuiEvents
	{
		#region Class Members
		const string c_PlugInName = "Misc PlugIn";
		const string c_CommandName_PlugInDesciption = "&Misc PlugIn...";
		const string c_CommandName_AuditTrailTest = "&Audit Trail Test...";
		const string c_CommandName_UpdateOnSendTest = "&UpdateOnSend Test...";
		const string c_CommandName_CreateLibraryTest = "&Create Library Test...";
        const string c_CommandName_XmlScrollTest = "&XML Scroll Test...";
        const string c_CommandName_LibPropTest = "&Library Properties Test...";
        const string c_CommandName_FieldAccessTest = "&Field Access Test";
        const string c_CommandName_FieldAccessTest2 = "&Field Access Test2";
        const string c_CommandName_DirectToTable = "&DirectToTable";
        const string c_CommandName_ReleaseUnchangedChildren = "&Release Unchanged Children";
        const string c_CommandName_CIHelloWorld = "&Core Interface Hello World...";
        const string c_CommandName_BatchUpdateText = "&Batch Update Test...";

		string _PlugInId;
		NxProject _Project;
		GuiApi _GuiApi;

		int _CommandNumber_Administration;
		int _CommandNumber_About;
		int _CommandNumber_AuditTrailTest;
		int _CommandNumber_UpdateOnSendTest;
		int _CommandNumber_CreateLibraryTest;
        int _CommandNumber_XmlScrollTest;
        int _CommandNumber_LibPropTest;
        int _CommandNumber_FieldAccessTest;
        int _CommandNumber_FieldAccessTest2;
        int _CommandNumber_DirectToTable;
        int _CommandNumber_ReleaseUnchangedChildren;
        int _CommandNumber_CIHelloWorld;
        int _CommandNumber_BatchUpdate;
		#endregion

        // Test direct to GuiApi
        int MainIndex = 0;
        int SubIndex = 0;
        int CmdIndex1 = 0;
        int CmdNumber1 = 0;
        int CmdIndex2 = 0;
        int CmdNumber2 = 0;

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
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_AuditTrailTest, ref _CommandNumber_AuditTrailTest);
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_UpdateOnSendTest, ref _CommandNumber_UpdateOnSendTest);
					MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_CreateLibraryTest, ref _CommandNumber_CreateLibraryTest);
                    MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_XmlScrollTest, ref _CommandNumber_XmlScrollTest);
                    MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_LibPropTest, ref _CommandNumber_LibPropTest);
                    MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_FieldAccessTest, ref _CommandNumber_FieldAccessTest);
                    MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_FieldAccessTest2, ref _CommandNumber_FieldAccessTest2);
                    MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_DirectToTable, ref _CommandNumber_DirectToTable);
                    MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_ReleaseUnchangedChildren, ref _CommandNumber_ReleaseUnchangedChildren);
                    MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_CIHelloWorld, ref _CommandNumber_CIHelloWorld);
                    MenuUtils.AddMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_BatchUpdateText, ref _CommandNumber_BatchUpdate);

                    // Test direct to GuiApi
                    int MainCount = 0;
                    _GuiApi.GetMenuCount(-1, -1, ref MainCount);
                    bool bFoundTest = false;
                    string MainName = "";
                    for (int i = 0; i < MainCount; i++)
                    {
                        _GuiApi.GetMenuName(-1, -1, i, ref MainName);
                        if(string.Compare("Test", MainName, true) == 0)
                        {
                            MainIndex = i;
                            bFoundTest = true;
                            break;
                        }
                    }
                    if(!bFoundTest)
                        _GuiApi.AddMenuPopUp(-1, -1, "Test", ref MainIndex);

                    int SubCount = 0;
                    _GuiApi.GetMenuCount(MainIndex, -1, ref SubCount);
                    bool bFoundSubTest = false;
                    string SubName = "";
                    for (int i = 0; i < SubCount; i++ )
                    {
                        _GuiApi.GetMenuName(MainIndex, -1, i, ref SubName);
                        if(string.Compare("SubTest", SubName, true) == 0)
                        {
                            SubIndex = i;
                            bFoundSubTest = true;
                            break;
                        }
                    }
                    if(!bFoundSubTest)
                        _GuiApi.AddMenuPopUp(MainIndex, -1, "SubTest", ref SubIndex);

                    _GuiApi.AddMenuCommand(MainIndex, SubIndex, -1, "CmdTest1", ref CmdIndex1, ref CmdNumber1);
                    _GuiApi.AddMenuCommand(MainIndex, SubIndex, -1, "CmdTest2", ref CmdIndex2, ref CmdNumber2);
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

                    // Test direct to GuiApi
                    _GuiApi.DeleteMenu(MainIndex, SubIndex, -1);



					// remove menu items
					MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Admin, MenuUtils.g_Local_MenuName_Admin_PlugInExtractionConfiguration, c_CommandName_PlugInDesciption);
					MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Help, MenuUtils.g_Local_MenuName_Help_AboutPlugIns, c_CommandName_PlugInDesciption);
					MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_AuditTrailTest);
					MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_UpdateOnSendTest);
					MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_CreateLibraryTest);
                    MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_XmlScrollTest);
                    MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_LibPropTest);
                    MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_FieldAccessTest);
                    MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_FieldAccessTest2);
                    MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_DirectToTable);
                    MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_ReleaseUnchangedChildren);
                    MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_CIHelloWorld);
                    MenuUtils.RemoveMenuCommand(_GuiApi, MenuUtils.g_Local_MenuName_Tools, MenuUtils.g_Local_MenuName_Tools_SDK, c_CommandName_BatchUpdateText);

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

		// NxVaultList - FindName is not implemented and NxVault is only the Id, so we must fetch to look for a name.
		string FindVault(string Name)
		{
			string VaultId = "";
			NxRecord VaultRec = _Project.NewRecord();
			VaultRec.Create("VLT");
			NxVault Vault = null;
			NxVaultList VaultList = _Project.VLGUManager.VaultList;
			int count = VaultList.GetCount();
			string ThisName = "";
			for (int i = 0; i < count; i++)
			{
				Vault = VaultList.GetItem(i);
				VaultRec.Clear();
				VaultRec.SetStringVal("S_VAULTID", Vault.Id);
				VaultRec.Fetch(0);
				ThisName = VaultRec.GetStringVal("S_NAME");
				if (string.Compare(ThisName, Name, true) == 0)
				{
					VaultId = Vault.Id;
					break;
				}

			}
			return VaultId;
		}

		void INxPlugInGuiEvents.BeginCustomCommand(int CommandNumber, NxTbl WindowTbl, NxDetailedList DetailedList)
		{
			// It is OK to use Windows Message Box, because Gui events are fired from the UI.
			try 
			{	        
				if(CommandNumber == _CommandNumber_Administration)
				{
					MessageBox.Show("Misc Administration");
				}
				else if (CommandNumber == _CommandNumber_About)
				{
                    Synergis.Adept.MainApi.SimpleAbout about = new Synergis.Adept.MainApi.SimpleAbout();
					about._PlugInName = c_PlugInName;
					about._Assy = Assembly.GetExecutingAssembly();
					about.ShowDialog();
				}
				else if(CommandNumber == _CommandNumber_AuditTrailTest)
				{
                    if(WindowTbl == null)
                    {
                        MessageBox.Show("No active window.");
                        return;
                    }
					//string tmp = WindowTbl.AuditTrailXMLResults(1, 1, "CMD_S_OPNUM:146,CMD_S_LONGNAME:89,CMD_S_ACTIVUTC:125,CMD_S_USERID:129,CMD_S_MAJREV:87,CMD_S_COMMENT:49,CMD_S_LIBID:100");
					string cols = _GuiApi.GetWindowColumnList(6);
					string tmp = WindowTbl.AuditTrailXMLResults(1, 1, cols);
					MessageBox.Show(tmp);
				}
				else if (CommandNumber == _CommandNumber_UpdateOnSendTest)
				{
					NxDetailedItem di = DetailedList.GetItem(0);
                    if(di == null)
                    {
                        MessageBox.Show("Nothing selected.");
                        return;
                    }
					int es = _Project.TransmittalMgr.UpdateTransmittalRecordOnSend(di.FileId, di.MajRev, di.MinRev, "SentTo@Synergis.com", "");
					if (es != ErrorCodes.EC_NO_ERR)
					{
						string errmsg = "";
						_Project.Login.Core.GetErrorString(es, ref errmsg);
						MessageBox.Show(string.Format("Error: {0}, {1}", es.ToString(), errmsg));
					}
				}
				else if (CommandNumber == _CommandNumber_CreateLibraryTest)
				{
                    string VaultId = FindVault("USA");
                    NxLibrary LibX1 = _Project.VLGUManager.LibraryList.CreateRuntimeLibrary(VaultId, "X1");
                    _Project.VLGUManager.LibraryList.UpdateRuntimeLibrary(LibX1);
                    NxLibrary LibY1 = _Project.VLGUManager.LibraryList.CreateRuntimeLibrary(LibX1.Id, "Y1");
                    _Project.VLGUManager.LibraryList.UpdateRuntimeLibrary(LibY1);
                    NxLibrary LibZ1 = _Project.VLGUManager.LibraryList.CreateRuntimeLibrary(LibY1.Id, "Z1");
                    _Project.VLGUManager.LibraryList.UpdateRuntimeLibrary(LibZ1);
					_Project.Login.BumpTableGroupVersion((int)ApiTypes.TABLE_GROUP_NUMBER.TGN_VL);
				}
                else if (CommandNumber == _CommandNumber_XmlScrollTest)
                {
                    NxQueryTable qt = _Project.CreateQueryTable();
                    NxQueryFieldItem qfi = qt.QueryFieldList.Add("SCHEMA_S_LONGNAME");
                    qfi.SearchOp = (int)ApiTypes.ADEPTT_SEARCHOP.OP_STARTS;
                    qfi.Value = "City";
                    NxTbl tbl = qt.DoSearch("ScrollTest");
                    if (tbl == null)
                        MessageBox.Show("No window.");
                    else if (tbl.IsServerScroll() != 1)
                        MessageBox.Show("Not Server Scroll.");
                    else if (tbl.SrvGetCount() < 1)
                        MessageBox.Show("Empty table.");
                    else
                    {
                        string xml = tbl.SrvXMLScroll(1, 20, "Columns=SCHEMA_S_LONGNAME,SCHEMA_S_EXT,SCHEMA_S_LIBNAME,SCHEMA_S_FLTPID,SCHEMA_S_FLTPNAME");
                        MessageBox.Show(xml);
                    }
                }
                else if (CommandNumber == _CommandNumber_LibPropTest)
                {
                    string Prop1 = "1310e05d-3cd2-40ea-89ea-cf414c092c60";
                    string Prop2 = "ccfc9bb3-cff0-409a-a732-5fa99b857616";
                    string Prop3 = "78f36cc8-7272-4bd4-b305-cd640045b159";
                    string Prop4 = "fae7c51d-d6df-4208-8485-47986f633bd4";

                    string PlugInId1 = "S_ADEPT_EXCELPLUGINID"; // MUST BE VALID
                    string PlugInId2 = "S_ADEPT_WORDPLUGINID"; // MUST BE VALID

                    int es = 0;

                    es = _Project.VLGUManager.ClearPropertiesForPlugIn(_PlugInId);
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);

                    // set shapes

                    es = _Project.VLGUManager.SetProperties(Prop1, PlugInId1, "SHAPE", "circle");
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);

                    es = _Project.VLGUManager.SetProperties(Prop3, PlugInId1, "SHAPE", "square");
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);

                    // set colors

                    es = _Project.VLGUManager.SetProperties(Prop1, PlugInId2, "COLOR", "green");
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);

                    es = _Project.VLGUManager.SetProperties(Prop3, PlugInId2, "COLOR", "blue");
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);

    

                    string msg = "";
                    
                    int WasExplicit = 0; 
                    string PropString = "";

                    // get shapes

                    WasExplicit = 0; PropString = "";
                    es = _Project.VLGUManager.GetProperties(Prop1, PlugInId1, "SHAPE", ref WasExplicit, ref PropString);
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);
                    msg += string.Format("Prop1\tExplict:{0}\tProp:{1}\n", WasExplicit, PropString);

                    WasExplicit = 0; PropString = "";
                    es = _Project.VLGUManager.GetProperties(Prop2, PlugInId1, "SHAPE", ref WasExplicit, ref PropString);
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);
                    msg += string.Format("Prop2\tExplict:{0}\tProp:{1}\n", WasExplicit, PropString);

                    WasExplicit = 0; PropString = "";
                    es = _Project.VLGUManager.GetProperties(Prop3, PlugInId1, "SHAPE", ref WasExplicit, ref PropString);
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);
                    msg += string.Format("Prop3\tExplict:{0}\tProp:{1}\n", WasExplicit, PropString);

                    WasExplicit = 0; PropString = "";
                    es = _Project.VLGUManager.GetProperties(Prop4, PlugInId1, "SHAPE", ref WasExplicit, ref PropString);
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);
                    msg += string.Format("Prop4\tExplict:{0}\tProp:{1}\n", WasExplicit, PropString);

                    // get colors

                    WasExplicit = 0; PropString = "";
                    es = _Project.VLGUManager.GetProperties(Prop1, PlugInId2, "COLOR", ref WasExplicit, ref PropString);
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);
                    msg += string.Format("Prop1\tExplict:{0}\tProp:{1}\n", WasExplicit, PropString);

                    WasExplicit = 0; PropString = "";
                    es = _Project.VLGUManager.GetProperties(Prop2, PlugInId2, "COLOR", ref WasExplicit, ref PropString);
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);
                    msg += string.Format("Prop2\tExplict:{0}\tProp:{1}\n", WasExplicit, PropString);

                    WasExplicit = 0; PropString = "";
                    es = _Project.VLGUManager.GetProperties(Prop3, PlugInId2, "COLOR", ref WasExplicit, ref PropString);
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);
                    msg += string.Format("Prop3\tExplict:{0}\tProp:{1}\n", WasExplicit, PropString);

                    WasExplicit = 0; PropString = "";
                    es = _Project.VLGUManager.GetProperties(Prop4, PlugInId2, "COLOR", ref WasExplicit, ref PropString);
                    if (es != 0)
                        PIUtils.CIShowError(_Project, c_PlugInName, es);
                    msg += string.Format("Prop4\tExplict:{0}\tProp:{1}\n", WasExplicit, PropString);

                    MessageBox.Show(msg);
                }
                else if (CommandNumber == _CommandNumber_FieldAccessTest)
                {
                    if (WindowTbl == null)
                        MessageBox.Show("Window is null.");
                    else if (WindowTbl.GetCount() < 1)
                        MessageBox.Show("Window is empty.");
                    else
                    {
                        int es = 0;
                        string tmp = "";

                        WindowTbl.GotoRecord(1);
                        tmp = WindowTbl.GetString("S_SRCDB");
                        int stn = Convert.ToInt32(tmp);
                        string FileId = WindowTbl.GetString("S_FILEID");
                        tmp = WindowTbl.GetString("S_MAJREV");
                        int MajRev = Convert.ToInt32(tmp);
                        tmp = WindowTbl.GetString("S_MINREV");
                        int MinRev = Convert.ToInt32(tmp);

                        // ---------------------

                        string a1 = WindowTbl.GetString("A1");
                        es = WindowTbl.LastError;
                        if(es != 0)
                        {
                            PIUtils.CIShowError(_Project, "Tbl - A1", es);
                        }

                        string a2 = WindowTbl.GetString("A2");
                        es = WindowTbl.LastError;
                        if (es != 0)
                        {
                            PIUtils.CIShowError(_Project, "Tbl - A2", es);
                        }

                        // ---------------------

                        NxDocRecord DocRec = _Project.NewDocRecord();
                        DocRec.CreateFromNumber(1);
                        DocRec.Set(FileId, MajRev, MinRev);
                        DocRec.Fetch(0);

                        a1 = DocRec.GetStringVal("A1");
                        es = DocRec.LastError;
                        if (es != 0)
                        {
                            PIUtils.CIShowError(_Project, "DocRec - A1", es);
                        }

                        a2 = DocRec.GetStringVal("A2");
                        es = DocRec.LastError;
                        if (es != 0)
                        {
                            PIUtils.CIShowError(_Project, "DocRec - A2", es);
                        }

                        // ---------------------

                        NxFldVal FldVal = DocRec.FindField("A2");
                        a2 = FldVal.StringVal;

                        es = FldVal.LastError;
                        if (es != 0)
                        {
                            PIUtils.CIShowError(_Project, "DocRec FldVal - A2", es);
                        }

                        // ---------------------

                        NxRecord Rec = _Project.NewRecord();
                        Rec.CreateFromNumber(1);
                        Rec.SetStringVal("S_FILEID", FileId);
                        Rec.SetIntegerVal("S_MAJREV", MajRev);
                        Rec.SetIntegerVal("S_MINREV", MinRev);
                        Rec.Fetch(0);

                        a2 = Rec.GetStringVal("A2");
                        es = Rec.LastError;
                        if (es != 0)
                        {
                            PIUtils.CIShowError(_Project, "Rec - A2", es);
                        }

                        // ---------------------

                        FldVal = Rec.FindField("A2");
                        a2 = FldVal.StringVal;

                        es = FldVal.LastError;
                        if (es != 0)
                        {
                            PIUtils.CIShowError(_Project, "Rec FldVal - A2", es);
                        }

                    }
                }
                else if (CommandNumber == _CommandNumber_FieldAccessTest2)
                {

                    int nRet = 0;
                    int NumRec = -1;
                    string Name = String.Empty;
                    string Id = String.Empty;
                    NxTbl Tbl;
                    NxQueryTable QueryTable;
                    NxQueryFieldList QueryFieldList;
                    NxQueryFieldItem QueryFieldItem;

                    QueryTable = _Project.CreateQueryTable();
                    QueryTable.TableName = "fm100format";
                    QueryFieldList = QueryTable.QueryFieldList;

                    QueryFieldItem = QueryFieldList.Add("ID");
                    QueryFieldItem.SearchOp = (int)ApiTypes.ADEPTT_SEARCHOP.OP_NOTEMPTY; //  (int)Synergis.AdeptApi.SearchOperator.NotEmpty;

                    Tbl = QueryTable.Query("AutoNameFormats");
                    if (Tbl != null && Tbl.GetCount() != 0)
                    {
                        NumRec = Tbl.GetCount();
                        for (int RecCnt = 1; RecCnt <= Tbl.GetCount(); RecCnt++)
                        {
                            nRet = Tbl.GotoRecord(RecCnt);
                            if (nRet == ErrorCodes.EC_NO_ERROR) // ErrorCodes.NoError
                            {
                                Name = Tbl.GetString("FORMATNAME");
                                Id = Tbl.GetString("ID");
                            }
                        }
                    }
                }
                else if (CommandNumber == _CommandNumber_DirectToTable)
                {
                    NxTbl Tbl = _Project.FindTbl("vlt");
                    if (Tbl == null)
                    {
                        MessageBox.Show("Tbl is null.");
                    }
                    else
                    {
                        int Count = Tbl.GetCount();
                        MessageBox.Show(string.Format("{0} Vaults found.", Count.ToString()));
                    }
                }
                else if (CommandNumber == _CommandNumber_ReleaseUnchangedChildren)
                {
                    if (DetailedList.GetCount() < 1)
                    {
                        MessageBox.Show("Nothing selected. Select an OUT parent.");
                        return;
                    }
                    NxDetailedList DLUnchangedChildren = _Project.NewDetailedList();
                    int es = _Project.BuildUnchangedChildrenList(DetailedList, DLUnchangedChildren, 0, 0);
                    if(es != ErrorCodes.EC_NO_ERR)
                    {
                        PIUtils.CIShowError(_Project,"AP_MISC", es);
                        return;
                    }
                    int Count = DLUnchangedChildren.GetCount();
                    if(Count < 1)
                    {
                        MessageBox.Show("No unchanged children.");
                        return;
                    }
                    string files = "";
                    for(int i = 0; i < Count; i++)
                    {
                        NxDetailedItem di = DLUnchangedChildren.GetItem(i);
                        files += di.FileNE + "\n";
                        DialogResult dr = MessageBox.Show(files, "", MessageBoxButtons.OKCancel);
                        if(dr == DialogResult.OK)
                        {
                            NxCommandSettings CmdSettings = _Project.Login.Core.CreateCommandSettings();
                            es = _Project.CommandsSL.UndoSignOutOfUnchangedChildren(DLUnchangedChildren, CmdSettings);
                            if (es != ErrorCodes.EC_NO_ERR)
                            {
                                PIUtils.CIShowError(_Project, "AP_MISC", es);
                                return;
                            }
                        }
                    }
                }
                else if(CommandNumber == _CommandNumber_CIHelloWorld)
                {
                    PIUtils.CIMsgBox(_Project, "AP_Misc", "HelloWorld");
                }
                else if (CommandNumber == _CommandNumber_BatchUpdate)
                {
                    if (DetailedList.GetCount() < 1)
                    {
                        MessageBox.Show("Nothing selected.");
                        return;
                    }
                    // transform dl into sl
                    NxSelectionList sl = _Project.NewSelectionList();
                    foreach(NxDetailedItem di in DetailedList)
                    {
                        sl.AddFromObject(di);
                    }
                    NxQueryTable qt = _Project.CreateQueryTable();
                    NxQueryFieldItem qfi = qt.QueryFieldList.Add("SCHEMA_G_WRKNOTE");
                    qfi.SearchOp = (int)ApiTypes.ADEPTT_SEARCHOP.OP_NONE;
                    qfi.Value = "My BU Value as of " + DateTime.Now.ToString();
                    NxCommandSettings CmdSettings = _Project.Login.Core.CreateCommandSettings();
                    _Project.CommandsSL.BatchUpdate(sl, qt, 0, CmdSettings);
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


		//public int IsMenuEnabled(int CommandNumber, string CommandName, NxDetailedList DList)
		//{
		//    MessageBox.Show("IsMenuEnabled was called.");
		//    return ApiTypes.eDisallow;
		//}
	}
}
