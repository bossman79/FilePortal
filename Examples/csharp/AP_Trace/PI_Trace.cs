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

namespace AP_Trace
{
	// If you copy this project change the guid in the PI_*.cs file and in the AssemblyInfo.cs file.
	// From the Visual Studio Tools menu, pick 'Create Guid'. Select 'Registry Format', click 'New Guid', then click 'Copy'.
	// Paste the value here and remove the { and }.
	[Guid("2A7D35E0-3D15-4026-AA14-F979A907CEB3"),
	ClassInterface(ClassInterfaceType.AutoDispatch),
	ComVisible(true)]
	public class PI_Trace : INxPlugInInterface, INxPlugInCoreCommandSLEvents, INxPlugInCoreCommandELEvents, INxPlugInCoreEvents, INxPlugInGuiEvents
	{
		#region Class Members
		const string c_PlugInName = "Trace PlugIn";
		const string c_CommandName_PlugInDesciption = "&Trace PlugIn...";
		const string c_ModuleName = "PI_Trace";

		string _PlugInId = null;
		NxProject _Project = null;
		GuiApi _GuiApi = null;
		TraceWindow _TW = null;
		int _Tabs = 0;
		string msg = null;
		#endregion

		#region INxPlugInInterface
		int INxPlugInInterface.Initialize(string PlugInId, NxProject Project, object GuiApiObj)
		{
			try
			{
                // remember these
				_PlugInId = PlugInId;
				_Project = Project;
				_GuiApi = (Interop.AdeptGui.GuiApi)GuiApiObj;

                // Localization
                string CurrentCulture = _Project.Login.Core.GetCulture();
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(CurrentCulture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                MainApi_Strings.Culture = ci;

				_TW = new TraceWindow();
				_TW.m_PlugInName = c_PlugInName;
				_TW.Show();
				_Tabs = 0;
				_TW.AddLine("Initialize", _Tabs);
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
				_TW.AddLine("Uninitialize", _Tabs);
				_TW.Close();
				_TW = null;
				_GuiApi = null;
				_Project = null;
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
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eAllow;
			msg = "AllowEdit: FieldName=" + FieldName + " bForUpdateDocument=" + bForUpdateDocument.ToString();
			_TW.AddLine(msg, _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreEvents.BatchOpenAndSave(NxDocRecord pDocRec, string FilePNE)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_TW.AddLine("BatchOpenAndSave", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreEvents.BeginItem(int CommandNumber, string CommandName, NxDocId pDocId, NxDocRecord pDocRecord)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			msg = "BeginItem: CommandNumber=" + CommandNumber.ToString() + " CommandName=" + CommandName;
			_TW.AddLine(msg, _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreEvents.CheckBatchOpenAndSaveSupport(ref int pSupported, ref int pEnabled, NxSTree STree)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_TW.AddLine("CheckBatchOpenAndSaveSupport", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreEvents.ClearExtractedFields(NxDocRecord pDocRec)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_TW.AddLine("ClearExtractedFields", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreEvents.DoesFileNeedExtraction(string Extension)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eFalse;
			msg = "DoesFileNeedExtraction: Extension=" + Extension;
			_TW.AddLine(msg, _Tabs);
			return functionReturnValue;
		}
		void INxPlugInCoreEvents.EndItem(int es, int CommandNumber, string CommandName, NxDocId pDocId, NxDocRecord pDocRecord, NxDocId pNewDocId, NxDocRecord pNewDocRecord)
		{
			msg = "EndItem: es=" + es.ToString() + " CommandNumber=" + CommandNumber.ToString() + " CommandName=" + CommandName;
			_TW.AddLine(msg, _Tabs);
			_Tabs = _Tabs - 1;
		}
		int INxPlugInCoreEvents.ExtractFile(NxDocRecord DocRec, string FilePNE)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_TW.AddLine("ExtractFile", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreEvents.IsBatchOpenAndSaveHandled(NxDocRecord DocRec)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eFalse;
			_TW.AddLine("IsBatchOpenAndSaveHandled", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreEvents.IsUpdateDocumentHandled(NxDocRecord pDocRec)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eFalse;
			_TW.AddLine("IsUpdateDocumentHandled", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreEvents.IsUpdateDocumentDownloadNeeded(NxDocRecord pDocRec)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eFalse;
			_TW.AddLine("IsUpdateDocumentDownloadNeeded", _Tabs);
			return functionReturnValue;
		}
		void INxPlugInCoreEvents.LibraryCardSavedToRecord(NxDocRecord pDocRec)
		{
			_TW.AddLine("LibraryCardSavedToRecord", _Tabs);
		}
		int INxPlugInCoreEvents.StartProgram(string LaunchApplicationId, string FilePNE)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_TW.AddLine("StartProgram", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreEvents.UpdateDocument(NxDocRecord pDocRec, string UpdateFilePNE)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_TW.AddLine("UpdateDocument", _Tabs);
			return functionReturnValue;
		}
		#endregion

		#region INxPlugInGuiEvents

		int INxPlugInGuiEvents.AllowLibCardEnable(int LibCardMode, NxDocRecord DocRec, string FieldName, int bForUpdateDocument, ref int pbExtractSupported, ref int pbUpdateDocumentSupported)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eAllow;
			msg = "AllowLibCardEnable: LibCardMode=" + LibCardMode.ToString() + " FieldName=" + FieldName + " bForUpdateDocument=" + bForUpdateDocument.ToString();
			_TW.AddLine(msg, _Tabs);
			return functionReturnValue;
		}
		int INxPlugInGuiEvents.BeginACommand(int CommandNumber, string CommandName, NxTbl WindowTbl, NxDetailedList DetailedList)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;

			if ((CommandNumber == CommandCodes.ACN_EDIT__SELECTION_CHANGED))
			{
				if ((_TW.CH_TraceSelChg.Checked == false)) {
					return functionReturnValue;
				}
			}
			if ((CommandNumber == CommandCodes.ACN_VIEW__CREATE_LIBRARY_CARD | CommandNumber == CommandCodes.ACN_VIEW__CANCEL_LIBRARY_CARD | CommandNumber == CommandCodes.ACN_VIEW__UPDATE_LIBRARY_CARD))
			{
				if ((_TW.CH_TraceLibCardChg.Checked == false)) {
					return functionReturnValue;
				}
			}

			//Try
			_Tabs = _Tabs + 1;
			msg = "BeginACommand: CommandNumber=" + CommandNumber.ToString() + " CommandName=" + CommandName;
			_TW.AddLine(msg, _Tabs);
			return functionReturnValue;
			// Catch ex As Exception
			//    MsgBox(ex.ToString())
			//End Try
		}
		void INxPlugInGuiEvents.BeginCustomCommand(int CommandNumber, NxTbl WindowTbl, NxDetailedList DetailedList)
		{
			_Tabs = _Tabs + 1;
			msg = "BeginCustomCommand: CommandNumber=" + CommandNumber.ToString();
			_TW.AddLine(msg, _Tabs);
		}

		void INxPlugInGuiEvents.EndACommand(int CommandNumber, string CommandName, NxTbl WindowTbl, NxDetailedList DetailedList)
		{
			if ((CommandNumber == CommandCodes.ACN_EDIT__SELECTION_CHANGED)) {
				if ((_TW.CH_TraceSelChg.Checked == false)) {
					return;
				}
			}
			if ((CommandNumber == CommandCodes.ACN_VIEW__CREATE_LIBRARY_CARD | CommandNumber == CommandCodes.ACN_VIEW__CANCEL_LIBRARY_CARD | CommandNumber == CommandCodes.ACN_VIEW__UPDATE_LIBRARY_CARD))
			{
				if ((_TW.CH_TraceLibCardChg.Checked == false)) {
					return;
				}
			}

			msg = "EndACommand: CommandNumber=" + CommandNumber.ToString() + " CommandName=" + CommandName;
			_TW.AddLine(msg, _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInGuiEvents.EndCustomCommand(int CommandNumber, NxTbl WindowTbl, NxDetailedList DetailedList)
		{
			msg = "EndCustomCommand: CommandNumber=" + CommandNumber.ToString();
			_TW.AddLine(msg, _Tabs);
			_Tabs = _Tabs - 1;
		}
		int INxPlugInGuiEvents.LibraryCardFieldGetFocus(string FieldName)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			msg = "LibraryCardFieldGetFocus: FieldName=" + FieldName;
			_TW.AddLine(msg, _Tabs);
			return functionReturnValue;
		}
		int INxPlugInGuiEvents.LibraryCardFieldLoseFocus(string FieldName)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			msg = "LibraryCardFieldLoseFocus: FieldName=" + FieldName;
			_TW.AddLine(msg, _Tabs);
			return functionReturnValue;
		}

		#endregion

		#region INxPlugInCoreCommandSLEvents - Begin
		int INxPlugInCoreCommandSLEvents.BeginApprove(NxSelectionList SelectionList, string WorkflowComment, int bMakeRevision, NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginApprove", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginArchive(NxSelectionList pSL, string Path, string Label, int bIncludeRevisions, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginArchive", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginAssign(NxSelectionList pSL, string AssignToId, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginAssign", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginAssignWorkflow(NxSelectionList SelectionList, string WorkflowId, string WorkflowComment, int bAlsoSetAsDefault, NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginAssignWorkflow", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginAssumeOwnership(NxSelectionList pSL, string LibId, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginAssumeOwnership", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginAssumeOwnership2(NxSelectionList SelectionList, string LibId, int bCopyLibCard, NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginAssumeOwnership2", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginBatchOpenAndSave(NxSelectionList pSL, int bClearLogBefore, int bProcessChildren, int bShowLogAfter, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginBatchOpenAndSave", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginBatchUpdate(NxSelectionList SelectionList, NxQueryTable QueryTable, int bUpdateFile, NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginBatchUpdate", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginClearExtractedInfo(NxSelectionList pSL, int bClearExtractionFields, int bClearExtractedRelationships, int bClearThumbnail, int bClearExtractionState, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginClearExtractedInfo", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginClearRedlines(NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginClearRedlines", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginClearTags(NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginClearTags", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginCopy(NxSelectionList pSL, int FromTo, string DestinationId, int bCopyLibCard, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			pOverwriteDestinationFile = -99;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginCopy", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginCreateRevision(NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginCreateRevision", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginDelete(NxSelectionList pSL, int bDeleteFileToo, ref int pDeleteReferencedFile, ref int pLoseChangesOnHold, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginDelete", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginExpeditorAppove(NxSelectionList pSL, string StepId, string WorkflowComment, int bMakeRevision, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginExpeditorAppove", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginExpeditorReject(NxSelectionList pSL, string StepId, string WorkflowComment, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginExpeditorReject", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginExtract(NxSelectionList SelectionList, int ExtractionType, int RelationshipResolutionType, int bClearState, NxSTree STree, int bUseLibVirtSetting, NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginExtract", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginLaunch(NxSelectionList pSL, string LaunchAppId, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginLaunch", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginLink(NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginLink", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginMove(NxSelectionList pSL, int FromTo, string DestinationId, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			pOverwriteDestinationFile = -11;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginMove", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginPrintFiles(NxSelectionList pSL, NxPrintFilesSettings pPrintFilesSettings, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginPrintFiles", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginReject(NxSelectionList pSL, string WorkflowComment, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginReject", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginRejectTo(NxSelectionList pSL, string StepId, string WorkflowComment, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginRejectTo", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginReleaseOwnership(NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginReleaseOwnership", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginRename(NxSelectionList pSL, string NewName, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginRename", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginRestore(NxSelectionList pSL, string Path, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginRestore", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginRollbackRevision(NxSelectionList pSL, int MajRev, int MinRev, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginRollbackRevision", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginSendTo(NxSelectionList SelectionList, int bSendLinks, int bSendToEmailRecipient, int bIncludeDependencies, int bZip, string ZipPassword, string FileName, NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginSendTo", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginSetDefaultWorkflow(NxSelectionList pSL, string WorkflowId, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginSetDefaultWorkflow", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginSetFileType(NxSelectionList pSL, string FileTypeId, int bOnlyIfNotSet, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginSetFileType", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginSignIn(NxSelectionList SelectionList, int bMakeRevision, string LibId, string AssignedToUserId, NxSelectionList pResultsSL, NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginSignIn", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginSignOut(NxSelectionList pSL, string WorkAreaId, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginSignOut", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginUnassign(NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginUnassign", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginUnlink(NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginUnlink", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandSLEvents.BeginViewFiles(NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginViewFiles", _Tabs);
			return functionReturnValue;
		}
		#endregion

		#region INxPlugInCoreCommandSLEvents - End
		void INxPlugInCoreCommandSLEvents.EndApprove(int es, NxSelectionList SelectionList, string WorkflowComment, int bMakeRevision, NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndApprove", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndArchive(int es, NxSelectionList pSL, string Path, string Label, int bIncludeRevisions, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndArchive", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndAssign(int es, NxSelectionList pSL, string AssignToId, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndAssign", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndAssignWorkflow(int es, NxSelectionList SelectionList, string WorkflowId, string WorkflowComment, int bAlsoSetAsDefault, NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndAssignWorkflow", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndAssumeOwnership(int es, NxSelectionList pSL, string LibId, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndAssumeOwnership", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndAssumeOwnership2(int es, NxSelectionList SelectionList, string LibId, int bCopyLibCard, NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndAssumeOwnership2", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndBatchOpenAndSave(int es, NxSelectionList pSL, int bClearLogBefore, int bProcessChildren, int bShowLogAfter, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndBatchOpenAndSave", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndBatchUpdate(int es, NxSelectionList SelectionList, NxQueryTable QueryTable, int bUpdateFile, NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndBatchUpdate", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndClearExtractedInfo(int es, NxSelectionList pSL, int bClearExtractionFields, int bClearExtractedRelationships, int bClearThumbnail, int bClearExtractionState, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndClearExtractedInfo", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndClearRedlines(int es, NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndClearRedlines", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndClearTags(int es, NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndClearTags", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndCopy(int es, NxSelectionList pSL, int FromTo, string DestinationId, int bCopyLibCard, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndCopy", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndCreateRevision(int es, NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndCreateRevision", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndDelete(int es, NxSelectionList pSL, int bDeleteFileToo, ref int pDeleteReferencedFile, ref int pLoseChangesOnHold, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndDelete", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndExpeditorAppove(int es, NxSelectionList pSL, string StepId, string WorkflowComment, int bMakeRevision, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndExpeditorAppove", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndExpeditorReject(int es, NxSelectionList pSL, string StepId, string WorkflowComment, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndExpeditorReject", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndExtract(int es, NxSelectionList SelectionList, int ExtractionType, int RelationshipResolutionType, int bClearState, NxSTree STree, int bUseLibVirtSetting, NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndExtract", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndLaunch(int es, NxSelectionList pSL, string LaunchAppId, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndLaunch", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndLink(int es, NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndLink", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndMove(int es, NxSelectionList pSL, int FromTo, string DestinationId, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			pOverwriteDestinationFile = -22;
			_TW.AddLine("EndMove", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndPrintFiles(int es, NxSelectionList pSL, NxPrintFilesSettings pPrintFilesSettings, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndPrintFiles", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndReject(int es, NxSelectionList pSL, string WorkflowComment, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndReject", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndRejectTo(int es, NxSelectionList pSL, string StepId, string WorkflowComment, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndRejectTo", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndReleaseOwnership(int es, NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndReleaseOwnership", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndRename(int es, NxSelectionList pSL, string NewName, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndRename", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndRestore(int es, NxSelectionList pSL, string Path, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndRestore", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndRollbackRevision(int es, NxSelectionList pSL, int MajRev, int MinRev, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndRollbackRevision", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndSendTo(int es, NxSelectionList SelectionList, int bSendLinks, int bSendToEmailRecipient, int bIncludeDependencies, int bZip, string ZipPassword, string FileName, NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndSendTo", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndSetDefaultWorkflow(int es, NxSelectionList pSL, string WorkflowId, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndSetDefaultWorkflow", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndSetFileType(int es, NxSelectionList pSL, string FileTypeId, int bOnlyIfNotSet, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndSetFileType", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndSignIn(int es, NxSelectionList SelectionList, int bMakeRevision, string LibId, string AssignedToUserId, NxSelectionList pResultsSL, NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndSignIn", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndSignOut(int es, NxSelectionList pSL, string WorkAreaId, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndSignOut", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndUnassign(int es, NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndUnassign", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndUnlink(int es, NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndUnlink", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandSLEvents.EndViewFiles(int es, NxSelectionList pSL, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndViewFiles", _Tabs);
			_Tabs = _Tabs - 1;
		}
		#endregion

		#region INxPlugInCoreCommandELEvents - Begin
		public int BeginApproveEL(NxExtendedList ExtendedList, NxCommandSettings CommandSettings)
		{
			return 0;
		}
		int INxPlugInCoreCommandELEvents.BeginApprove(NxExtendedList ExtendedList, NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginApproveEL", _Tabs);
			return functionReturnValue;
		}
		public int BeginCopyEL(NxExtendedList pEL, int FromTo, string DestinationId, int bCopyLibCard, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginCopyEL", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandELEvents.BeginCopy(NxExtendedList pEL, int FromTo, string DestinationId, int bCopyLibCard, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			return BeginCopyEL(pEL, FromTo, DestinationId, bCopyLibCard, ref pOverwriteDestinationFile, pCmdSettings);
		}
		int INxPlugInCoreCommandELEvents.BeginExpeditorApprove(Interop.AdeptCAC.NxExtendedList ExtendedList, Interop.AdeptCAC.NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginExpeditorApproveEL", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandELEvents.BeginExpeditorReject(Interop.AdeptCAC.NxExtendedList ExtendedList, Interop.AdeptCAC.NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginExpeditorRejectEL", _Tabs);
			return functionReturnValue;
		}
		public int BeginNewEL(NxExtendedList pEL, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginNewEL", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandELEvents.BeginNew(NxExtendedList pEL, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			return BeginNewEL(pEL, ref pOverwriteDestinationFile, pCmdSettings);
		}
		int INxPlugInCoreCommandELEvents.BeginReject(Interop.AdeptCAC.NxExtendedList ExtendedList, Interop.AdeptCAC.NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginRejectEL", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandELEvents.BeginRejectTo(Interop.AdeptCAC.NxExtendedList ExtendedList, Interop.AdeptCAC.NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginRejectToEL", _Tabs);
			return functionReturnValue;
		}
		public int BeginRenameEL(NxExtendedList pEL, NxCommandSettings pCmdSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginRenameEL", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandELEvents.BeginRename(NxExtendedList pEL, NxCommandSettings pCmdSettings)
		{
			return BeginRenameEL(pEL, pCmdSettings);
		}
		public int BeginSignInEL(NxExtendedList ExtendedList, NxSelectionList pResultsSL, NxCommandSettings CommandSettings)
		{
			int functionReturnValue = 0;
			functionReturnValue = ApiTypes.eContinue;
			_Tabs = _Tabs + 1;
			_TW.AddLine("BeginSignInEL", _Tabs);
			return functionReturnValue;
		}
		int INxPlugInCoreCommandELEvents.BeginSignIn(NxExtendedList ExtendedList, NxSelectionList pResultsSL, NxCommandSettings CommandSettings)
		{
			return BeginSignInEL(ExtendedList, pResultsSL, CommandSettings);
		}
		#endregion

		#region INxPlugInCoreCommandELEvents - End
		public void EndApproveEL(int es, NxExtendedList ExtendedList, NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndApproveEL", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandELEvents.EndApprove(int es, NxExtendedList ExtendedList, NxCommandSettings CommandSettings)
		{
			EndApproveEL(es, ExtendedList, CommandSettings);
		}
		public void EndCopyEL(int es, NxExtendedList pEL, int FromTo, string DestinationId, int bCopyLibCard, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndCopyEL", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandELEvents.EndCopy(int es, NxExtendedList pEL, int FromTo, string DestinationId, int bCopyLibCard, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			EndCopyEL(es, pEL, FromTo, DestinationId, bCopyLibCard,ref  pOverwriteDestinationFile, pCmdSettings);
		}
		void INxPlugInCoreCommandELEvents.EndExpeditorApprove(int es, Interop.AdeptCAC.NxExtendedList ExtendedList, Interop.AdeptCAC.NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndExpeditorApproveEL", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandELEvents.EndExpeditorReject(int es, Interop.AdeptCAC.NxExtendedList ExtendedList, Interop.AdeptCAC.NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndExpeditorRejectEL", _Tabs);
			_Tabs = _Tabs - 1;
		}
		public void EndNewEL(int es, NxExtendedList pEL, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndNewEL", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandELEvents.EndNew(int es, NxExtendedList pEL, ref int pOverwriteDestinationFile, NxCommandSettings pCmdSettings)
		{
			EndNewEL(es, pEL, ref pOverwriteDestinationFile, pCmdSettings);
		}
		public void EndRenameEL(int es, NxExtendedList pEL, NxCommandSettings pCmdSettings)
		{
			_TW.AddLine("EndRenameEL", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandELEvents.EndReject(int es, Interop.AdeptCAC.NxExtendedList ExtendedList, Interop.AdeptCAC.NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndRejectEL", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandELEvents.EndRejectTo(int es, Interop.AdeptCAC.NxExtendedList ExtendedList, Interop.AdeptCAC.NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndRejectToEL", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandELEvents.EndRename(int es, NxExtendedList pEL, NxCommandSettings pCmdSettings)
		{
			EndRenameEL(es, pEL, pCmdSettings);
		}
		public void EndSignInEL(int es, NxExtendedList ExtendedList, NxSelectionList pResultsSL, NxCommandSettings CommandSettings)
		{
			_TW.AddLine("EndSignInEL", _Tabs);
			_Tabs = _Tabs - 1;
		}
		void INxPlugInCoreCommandELEvents.EndSignIn(int es, NxExtendedList ExtendedList, NxSelectionList pResultsSL, NxCommandSettings CommandSettings)
		{
			EndSignInEL(es, ExtendedList, pResultsSL, CommandSettings);
		}
		#endregion

        public int TLFTemplateLoad(string FilePNE)
        {
            //DialogResult dr = MessageBox.Show("Allow?", "", MessageBoxButtons.YesNo);
            //if (dr == DialogResult.No)
            //    return -1;
            return 0;
        }

    }
}
