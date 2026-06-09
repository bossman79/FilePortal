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

namespace AP_Extract
{
	public class ExtractUtils
	{
		#region Declare Functions

		[DllImport("user32", EntryPoint = "WinHelpA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int WinHelp(int hwnd, string HelpFilePNE, int cmd, int dwData);

		#endregion

		#region Class Members

		// App
		public const string c_PlugInName = "Adept PlugIn for Extract";
		public const string c_PlugInInterfaceName = "AP_Extract.PI_Extract";
		public const string c_PlugInIniNE = "AP_Extract.ini";
		public const string c_AppProgId = "Extract.Application";
		public const string c_AppExeNE = "Extract.exe";
		public const int c_ApiVersionMin = 60000;
		public const int c_ApiVersionMax = 60099;
		public const int c_AppVersionMin = 60000;
		public const int c_AppVersionMax = 60099;
		// Help
		public const string c_HelpFileNE = "Adept.hlp";
		public const int c_AdministrationFormHelpId = 17510;
		public const int c_ScannerFormHelpId = 17511;
		// Ini Main
		public const string c_IniMainSection = "Main";
		public const string c_IniVersionKey = "Version";
		public const int c_IniVersion = 100000; // like 10.0.0.00
		// Extensions
		public const string c_IniExtensionsSection = "Extensions";
		//public const string c_IniExtensionExtract = "Extract";
		public const string c_TxtExtension = ".txt";
		public const string c_IniExtension = ".ini";
		// Options
		public const string c_IniOptionsSection = "Options";
		public const string c_ExtractPropertiesOption = "Extract Properties";
		public const string c_ExtractAdditionalProperitesOption = "Extract Additional Properties";
		//Public Const c_ExtractRelationshipsOption As String = "Extract Relationships"
		//Public Const c_ExtractThumbnailsOption As String = "Extract Thumbnails"
		public const string c_UpdateDocumentOption = "Update Document";
		public const string c_BatchOpenAndSaveOption = "Batch Open and Save";
		public const string c_ControlLaunchingOption = "Control Launching";
		// Other
		public const string c_IniFilesToScanSection = "Files To Scan";
		public const string c_IniFieldsSection = "Fields";
		public const string c_IniAvailableDataSection = "Available Data";
		// Data Source Root
		public const string c_TypeIniEntry = "IniEntry";

		#endregion

		#region App Connect

		// See if the supported app or extraction tool is on the machine.
		public static bool TestApp()
		{
			// For this sample, there is no external app.
			// Otherwise, the test code may look like the following:
			// Microsoft.Win32.RegistryKey myKey= Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(c_AppProgId);
			// if (myKey == null)
			//    return false;

			return true;
		}

		public static bool AppConnect(ref object oApp, ref bool bCreatedApp, ref bool bCreateAppFailed)
		{
			// If app uses interop, garbage collect now.
			// GC.Collect()
			// GC.WaitForPendingFinalizers()

			// if already connected
			if ((oApp != null))
				return true;

			// if having trouble connecting
			if ((bCreateAppFailed))
				return false;

			try
			{
				// Create the app and connect. This example just using a string.
				// oApp = CreateObject(c_AppProgId)
				oApp = "App";
			}
			catch (Exception)
			{
				// The Create failed.
				oApp = null;
				bCreateAppFailed = true;
				return false;
			}
			
			// Create was a success.
			bCreatedApp = true;
			return true;
		}

		public static bool AppOpenFile(ref object oApp, string FilePNE, bool bReadOnly, ref object oAppFile, ref string ErrMsg)
		{
			// If app uses interop, garbage collect now.
			// GC.Collect()
			// GC.WaitForPendingFinalizers()

			try
			{
				// Open the file. This example just using a string.
				// oApp.Open(FilePNE)
				oAppFile = FilePNE;
			}
			catch (Exception ex)
			{
				// The Open failed.
				ErrMsg = ex.Message;
				return false;
			}

			// Open was a success.
			return true;
		}

		public static bool AppCloseFile(ref object oAppFile, bool bSave, ref string ErrMsg)
		{
			try
			{
				// If there is an open file,
				if ((oAppFile != null))
				{
					// (Save if told to, and) close the file. This example just sets the fake file handle to null.
					// oAppFile.Close(bSave)
					oAppFile = null;
				}
			}
			catch (Exception ex)
			{
				// The Close failed.
				ErrMsg = ex.Message;
				return false;
			}
			

			// if app uses interop, garbage collect now
			// GC.Collect()
			// GC.WaitForPendingFinalizers()

			// The Close was a success.
			return true;
		}

		public static void AppDisconnect(ref object oApp)
		{
			try
			{
				// If app requires an explicit close, do it now. The example just set the fake object to null.
				//if (oApp != null)
				//    oApp.Quit();
				oApp = null;
			}
			catch (Exception)
			{
				// ignore
			}
	
			// if app uses interop, garbage collect now
			// GC.Collect()
			// GC.WaitForPendingFinalizers()
		}

		#endregion

		#region Ini Settings

		// Read our Ini settings for Extensions and Options.
		public static bool ReadOurIniSettings(string PlugInIniPNE, CheckList Extensions, CheckList Options)
		{
			// Check Ini file version.
			int Ver = IniUtils.GetPPI(PlugInIniPNE, c_IniMainSection, c_IniVersionKey);
			if ((Ver > c_IniVersion))
			{
				// The Ini file has been changed by a newer version of this application.
				return false;
			}

			Extensions.ReadIni(PlugInIniPNE, c_IniExtensionsSection, false); // DYNAMIC_EXT - To enable dynamic extenions, set SectionIsCount to true

			Options.ReadIni(PlugInIniPNE, c_IniOptionsSection, false);

			return true;
		}

		// Write our Ini settings for Extensions and Options.
		public static void WriteOurIniSettings(string PlugInIniPNE, CheckList Extensions, CheckList Options)
		{
			IniUtils.SetPPI(PlugInIniPNE, c_IniMainSection, c_IniVersionKey, c_IniVersion);

			Extensions.WriteIni(PlugInIniPNE, c_IniExtensionsSection, false); // DYNAMIC_EXT - To enable dynamic extenions, set SectionIsCount to true
			
			Options.WriteIni(PlugInIniPNE, c_IniOptionsSection, false);
		}

		#endregion

		#region Admin Form

		public static void ShowAdministrationForm(
			string PlugInName, string PlugInId, string PlugInIniPNE, CheckList Extensions, CheckList Options, NxProject Project)
		{
			// Make a new instance of the common extraction mapping PlugIn Admin form.
			ExtractionAdminForm AdminFrm = new ExtractionAdminForm();
			// Set the members.
			AdminFrm._LocalPlugInName = PlugInName;
			AdminFrm._PlugInName = PlugInName;
			AdminFrm._PlugInId = PlugInId;
			AdminFrm._PlugInIniPNE = PlugInIniPNE;
			AdminFrm._Project = Project;
			AdminFrm._bExtensionListReadWrite = false; // DYNAMIC_EXT - To enable dynamic extenions, set m_bExtensionListReadWrite to true
			// Hook up to the delegates
			AdminFrm.ExtensionInsert +=new ExtractionAdminForm.ExtensionInsertEventHandler(ExtractionAdminForm_ExtensionInsert_Handler);
			AdminFrm.OptionChange += new ExtractionAdminForm.OptionChangeEventHandler(ExtractionAdminForm_OptionChange_Handler);
			AdminFrm.EditItem += new ExtractionAdminForm.EditItemEventHandler(ExtractionAdminForm_EditItem_Handler);
			AdminFrm.HelpClick += new ExtractionAdminForm.HelpClickEventHandler(ExtractionAdminForm_Help_Handler);
			AdminFrm.ScannerClick += new ExtractionAdminForm.ScannerClickEventHandler(ExtractionAdminForm_Scanner_Handler);

			// Get updated ini to make sure that we are up to date.
			PIUtils.WritePlugInMemoToFile(PlugInName, PlugInId, Project, "MUSER1", PlugInIniPNE);
			// Read ini settings.
			if ((ReadOurIniSettings(PlugInIniPNE, Extensions, Options) == false)) 
			{
				string msg = null;
				msg = "The .ini file has been changed by a newer version of this application." + "\n";;
				msg = msg + "Please confirm that you are loading the correct version.";
				MessageBox.Show(msg, c_PlugInName);
				return;
			}

			// Push Extensions and Options to the form.
			Extensions.ToControl(AdminFrm.LVExtensions);
			Options.ToControl(AdminFrm.LVOptions);


			string FieldId = null;
			string Value = null;
			// Read each Adept FieldId and its mapped data.
			int i = 0;
			bool bContinue = true;
			while (bContinue) 
			{
				// Get a FieldId.
				FieldId = IniUtils.GetPPS(PlugInIniPNE, c_IniFieldsSection, i.ToString());
				// If there is a field there, ...
				if (FieldId.Length > 0) 
				{
					// Add it to the form's mapped data list.
					DataItem DataItem = AdminFrm._MappedData.AddItem(FieldId);
					int j = 0;
					bool bMapContinue = true;
					while (bMapContinue)
					{
						// Get the 1st value of mapping.
						Value = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C0");
						// If there is a mapping value there, ...
						if (Value.Length > 0) 
						{
							// Add it to the form's list of mappings for that field, and load the remaining values.
							DataRow DataRow = DataItem.AddRow();
							DataRow.m_Columns[0] = Value;
							DataRow.m_Columns[1] = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C1");
							DataRow.m_Columns[2] = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C2");
							DataRow.m_Columns[3] = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C3");
							DataRow.m_Columns[4] = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C4");

							// This example is not localized, so just pass the same data as the localized strings.
							DataRow.m_LocalColumns[0] = DataRow.m_Columns[0];
							DataRow.m_LocalColumns[1] = DataRow.m_Columns[1];
							DataRow.m_LocalColumns[2] = DataRow.m_Columns[2];
							DataRow.m_LocalColumns[3] = DataRow.m_Columns[3];
							DataRow.m_LocalColumns[4] = DataRow.m_Columns[4];
						} 
						else 
						{
							// Otherwise, we are done loading mappings.
							bMapContinue = false;
						}
						// Increment mapping position in case we are continuing.
						j = j + 1;
					}
				} 
				else 
				{
					// Otherwise, we are done loading fields.
					bContinue = false;
				}
				// Increment field position in case we are continuing.
				i = i + 1;
			}

			// Load the list of known Available Data into the form.
			i = 0;
			bContinue = true;
			while (bContinue) 
			{
				// Get the 1st value of a map source.
				Value = IniUtils.GetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C0");
				// If there is a map source there, ...
				if (Value.Length > 0)
				{
					// Add the available items to the form.
					// This example is not localized, so just pass the same data as the localized strings.
					AdminFrm.AddAvailableData(
						Value, 
						IniUtils.GetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C1"), 
						IniUtils.GetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C2"), 
						IniUtils.GetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C3"), 
						IniUtils.GetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C4"),
						Value, 
						IniUtils.GetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C1"), 
						IniUtils.GetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C2"), 
						IniUtils.GetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C3"), 
						IniUtils.GetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C4"));
				}
				else 
				{
					// Otherwise, we are done loading available data.
					bContinue = false;
				}
				// Increment available data position in case we are continuing.
				i = i + 1;
			}

			// Load columns into the mapped data list control.
			AdminFrm._MappedDataColumnHeaderText[0] = "Type";
			AdminFrm._MappedDataColumnHeaderText[1] = "Section";
			AdminFrm._MappedDataColumnHeaderText[2] = "Item";
			AdminFrm._MappedDataColumnHeaderText[3] = "";
			AdminFrm._MappedDataColumnHeaderText[4] = "";
			AdminFrm._MappedDataColumnHeaderScale[0] = 0.3;
			AdminFrm._MappedDataColumnHeaderScale[1] = 0.3;
			AdminFrm._MappedDataColumnHeaderScale[2] = 0.3;
			AdminFrm._MappedDataColumnHeaderScale[3] = 0.0;
			AdminFrm._MappedDataColumnHeaderScale[4] = 0.0;

			// Load columns into the available data list control.
			AdminFrm._AvailableDataColumnHeaderText[0] = "Type";
			AdminFrm._AvailableDataColumnHeaderText[1] = "Section";
			AdminFrm._AvailableDataColumnHeaderText[2] = "Item";
			AdminFrm._AvailableDataColumnHeaderText[3] = "";
			AdminFrm._AvailableDataColumnHeaderText[4] = "";
			AdminFrm._AvailableDataColumnHeaderScale[0] = 0.3;
			AdminFrm._AvailableDataColumnHeaderScale[1] = 0.3;
			AdminFrm._AvailableDataColumnHeaderScale[2] = 0.3;
			AdminFrm._AvailableDataColumnHeaderScale[3] = 0.0;
			AdminFrm._AvailableDataColumnHeaderScale[4] = 0.0;

			DialogResult dr = AdminFrm.ShowDialog();

			if (dr == DialogResult.OK)
			{
				// Pull any changes that were made to Extensions and Options from the form.
				Extensions.FromControl(AdminFrm.LVExtensions);
				Options.FromControl(AdminFrm.LVOptions);
				// Write to ini.
				WriteOurIniSettings(PlugInIniPNE, Extensions, Options);

				// Blank out the Fields section.
				IniUtils.WritePrivateProfileSection(c_IniFieldsSection, "", PlugInIniPNE);
				// Walk the Fields from the form.
				int MapCount = AdminFrm._MappedData.Count();
				for (i = 0; i < MapCount; i++)
				{
					// Get a Field Item.
					DataItem DataItem = AdminFrm._MappedData.m_Items[i];
					FieldId = DataItem.m_Name;
					// Add that Field to the ini.
					IniUtils.SetPPS(PlugInIniPNE, c_IniFieldsSection, i.ToString(), FieldId);
					// Blank that Field's mappings in the ini.
					IniUtils.WritePrivateProfileSection(FieldId, "", PlugInIniPNE);
					// Walk the Mappings from the form.
					int RowCount = DataItem.RowCount();
					for (int j = 0; j < RowCount; j++)
					{
						// Write one mapping to the ini.
						IniUtils.SetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C0", DataItem.m_Rows[j].m_Columns[0]);
						IniUtils.SetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C1", DataItem.m_Rows[j].m_Columns[1]);
						IniUtils.SetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C2", DataItem.m_Rows[j].m_Columns[2]);
						IniUtils.SetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C3", DataItem.m_Rows[j].m_Columns[3]);
						IniUtils.SetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C4", DataItem.m_Rows[j].m_Columns[4]);
					}
					// Blank out the next mapping in the ini, just to be sure that we don't get extra items left over from when there were more mappings.
					if ((RowCount < 0))
						RowCount = 0;
					IniUtils.SetPPS(PlugInIniPNE, FieldId, "R" + RowCount.ToString() + "C0", "");
				}
				// Blank out the next field in the ini, just to be sure that we don't get extra items left over from when there were more fields.
				if ((MapCount < 0))
					MapCount = 0;
				IniUtils.SetPPS(PlugInIniPNE, c_IniFieldsSection, MapCount.ToString(), "");

				// Blank Available Data in the ini.
				IniUtils.WritePrivateProfileSection(c_IniAvailableDataSection, "", PlugInIniPNE);
				// Walk the Available Data.
				int AvailableCount = AdminFrm.LVAvailableData.Items.Count;
				for (i = 0; i < AvailableCount; i++)
				{
					// Write each to the ini.
					IniUtils.SetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C0", AdminFrm.LVAvailableData.Items[i].Text);
					IniUtils.SetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C1", AdminFrm.LVAvailableData.Items[i].SubItems[1].Text);
					IniUtils.SetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C2", AdminFrm.LVAvailableData.Items[i].SubItems[2].Text);
					IniUtils.SetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C3", AdminFrm.LVAvailableData.Items[i].SubItems[3].Text);
					IniUtils.SetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + i.ToString() + "C4", AdminFrm.LVAvailableData.Items[i].SubItems[4].Text);
				}
				// Blank out the next item in the ini, just to be sure that we don't get extra items left over from when there were more available data items.
				if ((AvailableCount < 0))
					AvailableCount = 0;
				IniUtils.SetPPS(PlugInIniPNE, c_IniAvailableDataSection, "R" + AvailableCount.ToString() + "C0", "");

				// Update the PlugIn's Memo with the new ini info.
				PIUtils.WritePlugInFileToMemo(PlugInName, PlugInId, Project, "MUSER1", PlugInIniPNE);
			}
		}

		public static void ExtractionAdminForm_ExtensionInsert_Handler(object sender, EventArgs e)
		{
			// DYNAMIC_EXT - To enable dynamic extenions, uncomment this function's contents
			//EditBox EB = new EditBox();
			//EB.Text = "Add Extension to Extract";
			//EB.lbl1.Text = "Extension";
			//EB.ShowDialog();
			//if (EB.DialogResult == DialogResult.OK)
			//{
			//    string tmp = EB.EB1.Text;
			//    if(tmp.Substring(0,1) != ".")
			//        tmp = "." + tmp;
			//    ListView LVExt = (ListView) sender;
			//    foreach(ListViewItem lvi in LVExt.Items)
			//    {
			//        if(string.Compare(lvi.Text, tmp, true) == 0)
			//        {
			//            lvi.Checked = true;
			//            return;
			//        }
			//    }
			//    ListViewItem lviAdded = LVExt.Items.Add(tmp);
			//    lviAdded.Tag = tmp; // Not localized so name and tag are the same.
			//    lviAdded.Checked = true;
			//}
		}

		// Handle when the admin changes an option.
		// For example, make options dependent on each other.
		public static void ExtractionAdminForm_OptionChange_Handler(object sender, ExtractionAdminForm.OptionChangeEventArgs e)
		{
			// if the one changing is the Extract Properties option.
			if (string.Compare(e.OptionName, c_ExtractPropertiesOption, true) == 0)
			{
				// if it is now unchecked
				if (e.bChecked == false)
				{
					// Also uncheck Extract Additional Properties
					ListView lvOptions = (ListView)sender;
					foreach (ListViewItem lvi in lvOptions.Items)
					{
						if (string.Compare(lvi.Text, c_ExtractAdditionalProperitesOption, true) == 0)
						{
							lvi.Checked = false;
						}
					}
				}
			}
		}

		public static void ExtractionAdminForm_EditItem_Handler(object sender, ExtractionAdminForm.EditItemEventArgs e)
		{
			DataForm df = new DataForm();
			df._LV = (ListView)sender;
			df._bAddItem = e._bAddItem;
			df.ShowDialog();
		}

		public static void ExtractionAdminForm_Help_Handler(object sender, EventArgs e)
		{
			System.Reflection.Assembly Assy = Assembly.GetExecutingAssembly();
			// get full pne
			string AssyPNE = Assy.Location;
			// get \adept\plugins\something dir
			string Path = System.IO.Path.GetDirectoryName(AssyPNE);

			string HelpFilePNE = System.IO.Path.Combine(Path, c_HelpFileNE);
			WinHelp(0, HelpFilePNE, 1, c_AdministrationFormHelpId);
		}

		public static void ExtractionAdminForm_Scanner_Handler(object sender, EventArgs e)
		{
			ShowScannerForm((ExtractionAdminForm)sender);
		}

		#endregion

		#region "Scanner Form"

		public static void ShowScannerForm(ExtractionAdminForm AdminForm)
		{
			ExtractionScannerForm ScannerForm = new ExtractionScannerForm();
			ScannerForm._LocalPlugInName = AdminForm._LocalPlugInName;
			ScannerForm._PlugInName = AdminForm._PlugInName;
            ScannerForm._Project = AdminForm._Project;

			ScannerForm.LVExtensionsToScan.Items.Add(c_TxtExtension);
			ScannerForm.LVExtensionsToScan.Items.Add(c_IniExtension);

			ScannerForm.HelpClick += new ExtractionScannerForm.HelpClickEventHandler(ExtractionScannerForm_Help_Handler);

			string Value = null;
			int i = 0;
			bool bContinue = false;
			i = 0;
			bContinue = true;
			while (bContinue) 
			{
				Value = IniUtils.GetPPS(AdminForm._PlugInIniPNE, c_IniFilesToScanSection, i.ToString());
				if (Value.Length > 0) 
				{
					ScannerForm.LVFilesToScan.Items.Add(Value);
				} else 
				{
					bContinue = false;
				}
				i = i + 1;
			}

			DialogResult dr = ScannerForm.ShowDialog();

			if (dr == DialogResult.OK)
			{
				// Update Ini for FilesToScan
				for (i = 0; i < ScannerForm.LVFilesToScan.Items.Count; i++) 
				{
					IniUtils.SetPPS(AdminForm._PlugInIniPNE, c_IniFilesToScanSection, i.ToString(), ScannerForm.LVFilesToScan.Items[i].Text);
				}
				i = ScannerForm.LVFilesToScan.Items.Count;
				IniUtils.SetPPS(AdminForm._PlugInIniPNE, c_IniFilesToScanSection, i.ToString(), "");
				// Update Memo
				PIUtils.WritePlugInFileToMemo(c_PlugInName, AdminForm._PlugInId, AdminForm._Project, "MUSER1", AdminForm._PlugInIniPNE);
				// Open App
				object oApp = null;
				bool bThisCreatedApp = true;
				bool bCreateAppFailed = false;
				if ((AppConnect(ref oApp, ref bThisCreatedApp, ref bCreateAppFailed) == false)) 
				{
					MessageBox.Show("Failed to connect to App.");
					return;
				}
				string FilePNE = null;
				object oAppFile = null;
				string ErrMsg = null;
				for (i = 0; i < ScannerForm.LVFilesToScan.Items.Count; i++) 
				{
					// open the file
					FilePNE = ScannerForm.LVFilesToScan.Items[i].Text;
					if ((AppOpenFile(ref oApp, FilePNE, true, ref oAppFile, ref ErrMsg) == false)) 
					{
						MessageBox.Show(ErrMsg);
						continue;
					}

					// pretend that we scanned the file for available extraction sources
					//AdminForm.AddAvailableData(c_TypeIniEntry, "Main", "Item1", "", "", c_TypeIniEntry, "Main", "Item1", "", "");
					//AdminForm.AddAvailableData(c_TypeIniEntry, "Main", "Item2", "", "", c_TypeIniEntry, "Main", "Item2", "", "");
					//AdminForm.AddAvailableData(c_TypeIniEntry, "Main", "Item3", "", "", c_TypeIniEntry, "Main", "Item3", "", "");
					// or walk any ini
					using (System.IO.StreamReader sr = new System.IO.StreamReader(FilePNE))
					{
						string section = "";
						string key = "";
						string tmp = "";
						int rb_pos = -1;
						int e_pos = -1;
						while (!string.IsNullOrEmpty(tmp = sr.ReadLine()))
						{
							if (string.Compare(tmp.Substring(0, 1), "[", true) == 0
								&& (rb_pos = tmp.IndexOf("]")) > -1)
							{
								section = tmp.Substring(1, rb_pos - 1);
							}
							else if(!string.IsNullOrEmpty(section) && (e_pos = tmp.IndexOf("=")) > 0)
							{
								key = tmp.Substring(0, e_pos);
								AdminForm.AddAvailableData(c_TypeIniEntry, section, key, "", "", c_TypeIniEntry, section, key, "", "");
							}
						}
					}

					// close the file
					AppCloseFile(ref oAppFile, false, ref ErrMsg);

				}
				// Close App
				AppDisconnect(ref oApp);
			}
		}

		public static void ExtractionScannerForm_Help_Handler(object sender, EventArgs e)
		{
			System.Reflection.Assembly Assy = Assembly.GetExecutingAssembly();
			// get full pne
			string AssyPNE = Assy.Location;
			// get \adept\plugins\something dir
			string Path = System.IO.Path.GetDirectoryName(AssyPNE);

			string HelpFilePNE = System.IO.Path.Combine(Path, c_HelpFileNE);
			WinHelp(0, HelpFilePNE, 1, c_ScannerFormHelpId);
		}

		#endregion

		#region "Batch Open and Save"
		public static void BOS_Log(NxProject Project, string Msg)
		{
			string rMsg = null;
			rMsg = c_PlugInName + ": " + Msg + "\n";;
			Project.LogMessage((int)ApiTypes.ADEPTT_LOG_NUMBER.LOG__BATCH_OPEN_AND_SAVE, rMsg);
		}
		public static void OurBatchOpenAndSave(NxProject Project, object oApp, string FilePNE)
		{
			// open 
			object oAppFile = null;

			string ErrMsg = null;
			BOS_Log(Project, "Opening file.");
			if ((AppOpenFile(ref oApp, FilePNE, false, ref oAppFile, ref ErrMsg) == false))
			{
				BOS_Log(Project, ErrMsg);
				return;
			}
			// close and save
			if ((AppCloseFile(ref oAppFile, true, ref ErrMsg) == false))
			{
				BOS_Log(Project, ErrMsg);
				return;
			}
			BOS_Log(Project, "Processing completed.");
		}
		#endregion

		#region "Extraction"
		public static void GetMappedData(object oApp, object oAppFile, string c0, string c1, string c2, string c3, string c4, ref bool bFound, ref string FieldValue)
		{
			//BEGIN
			bFound = false;
			FieldValue = "";
			string Temp = null;
			string NotFoundGuid = "{AF5FF26A-5318-4239-8294-96A0F8E05039}";

			if ((c0 == c_TypeIniEntry))
			{
				// For the sample, the "file handle" is really a string to a FilePNE that is an ini file
				Temp = IniUtils.GetPPS((string)oAppFile, c1, c2, NotFoundGuid);
				if ((Temp != NotFoundGuid))
				{
					bFound = true;
					FieldValue = Temp;
				}
			}
		}
		public static void AddRel(ref string RelInfo, string Data)
		{
			string Info = Data;
			int qpos = 0;
			// Trim Left upto inc. |
			qpos = Info.IndexOf("|"); // Strings.InStr(1, Info, "|");
			if ((qpos > -1)) // 0
				Info = Info.Substring(qpos + 1, Info.Length - qpos -1);  // Strings.Right(Info, Strings.Len(Info) - qpos);
			// Trim Right upto inc !
			qpos = Info.IndexOf("!"); // Strings.InStr(1, Info, "!");
			if ((qpos > -1)) // 0
				Info = Info.Substring(0, qpos); // Strings.Left(Info, qpos - 1);
			// add to rel list
			PIUtils.AppendRelationship(ref RelInfo, System.IO.Path.GetFileName(Info), Info);
		}
		public static void OurExtractFile(NxProject Project, string PlugInIniPNE, object oApp, NxDocRecord DocRec, string FilePNE, ref CheckList Options, ref int DocsToExtract, ref int DocsExtracted, ref int DocsWithExtractedFields)
		{
			//BEGIN
			int es = 0;
			DocsToExtract = DocsToExtract + 1;

			bool bThisHasExtractedFields = false;
			//bool bThisHasRelationships = false;
			//bool bThisHasThumbnail = false;

			// if extracting properties or relationships, then open file
			if ((Options.IsChecked(c_ExtractPropertiesOption)))
			{
				// Open
				object oAppFile = null;
				string ErrMsg = null;
				if ((AppOpenFile(ref oApp, FilePNE, true, ref oAppFile, ref ErrMsg) == false))
				{
					MessageBox.Show(ErrMsg);
					return;
				}
				// BlankExtract
				ApiTypes.BLANK_EXTRACT BlankExtract = (ApiTypes.BLANK_EXTRACT)PIUtils.GetSystem_BlankExtract_Setting(Project);

				int i = 0;
				int j = 0;
				if ((Options.IsChecked(c_ExtractPropertiesOption)))
				{
					// Properties
					string FieldId = null;
					string FieldName = null;
					string c0 = null;
					string c1 = null;
					string c2 = null;
					string c3 = null;
					string c4 = null;
					// fields and mapped data
					bool bContinue = true;
					i = 0;
					while (bContinue)
					{
						FieldId = IniUtils.GetPPS(PlugInIniPNE, c_IniFieldsSection, i.ToString());
						if (FieldId.Length < 1)
							goto donefields;
						FieldName = PIUtils.GetFieldName(Project, FieldId);
						NxFieldItem Field = Project.FieldManager.FieldList.FindId(FieldId);
						if ((Field == null))
							goto cont;
						NxFldVal FldVal = DocRec.FindField(FieldName);
						if ((FldVal == null))
							goto cont;
						NxFldDef FldDef = FldVal.FldDef;
						if ((FldDef == null))
							goto cont;
						ApiTypes.ADEPTT_FLD_DEF_TYPE FldDefType = (ApiTypes.ADEPTT_FLD_DEF_TYPE)FldDef.FldDefType;
						bool bMapContinue = true;
						j = 0;
						while (bMapContinue)
						{
							c0 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C0");
							if (string.IsNullOrEmpty(c0))
							{
								j = j + 1;
								break;
							}
							c1 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C1");
							c2 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C2");
							c3 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C3");
							c4 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C4");
							bool bFound = false;
							string FValue = "";
							GetMappedData(oApp, oAppFile, c0, c1, c2, c3, c4, ref bFound, ref FValue);

							FValue = FValue.Trim();
							int FValLen = FValue.Length;

							// if got a value OR found something and blank is ok
							if ((FValLen > 0 | (bFound & BlankExtract == ApiTypes.BLANK_EXTRACT.BLANK_EXTRACT_EXIST)))
							{
								bThisHasExtractedFields = true;
								if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_DATE))
								{
									string AdeptDate = "";
									Project.Login.Core.StrToAdeptDate(FValue, AdeptDate);
									FValue = AdeptDate;
									FldVal.DateVal = FValue;
								}
								if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_CHARACTER))
								{
									FldVal.CharacterVal = FValue;
								}
								if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_STRING))
								{
									FldVal.StringVal = FValue;
								}
								// user fields only
								if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_LOGICAL))
								{
									string FValue1 = FValue.Substring(0, 1).ToUpper(); //Strings.UCase(Strings.Left(FValue, 1));
									if ((FValue1 == "Y" | FValue1 == "T" | FValue1 == "1"))
									{
										FldVal.LogicalVal = ApiTypes.eTrue;
									}
									else
									{
										FldVal.LogicalVal = ApiTypes.eFalse;
									}
								}
								if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_INTEGER))
								{
									FldVal.IntegerVal = System.Convert.ToInt32(FValue);
								}
								if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_DOUBLE))
								{
									FldVal.DoubleVal = System.Convert.ToDouble(FValue);
								}
								if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_MEMO))
								{
									FldVal.SetMemoFromString(FValue);
								}
								goto cont;
							}
							j = j + 1;
						}
					cont:
						i = i + 1;
					}
				donefields:
					es = DocRec.Update();
				}



				// Close
				AppCloseFile(ref oAppFile, false, ref ErrMsg);
			}

			if ((bThisHasExtractedFields))
				DocsWithExtractedFields = DocsWithExtractedFields + 1;
			DocsExtracted = DocsExtracted + 1;
		}
		#endregion

		#region "Library Card (and Batch Update)"
		public static void SetMappedData(string PlugInIniPNE, object oApp, object oAppFile, string FieldId, object FieldValue)
		{
			//BEGIN
			long j = 0;
			bool bMapContinue = false;
			string c0 = null;
			string c1 = null;
			string c2 = null;
			string c3 = null;
			string c4 = null;
			j = 0;
			bMapContinue = true;
			while (bMapContinue) {
				c0 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C0");
				if (c0.Length > 0) {
					c1 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C1");
					c2 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C2");
					c3 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C3");
					c4 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C4");
					if ((c0 == c_TypeIniEntry)) {
						// for this sample, only set if the section-item already exists
						bool bFound = false;
						string TmpFieldValue = "";
						GetMappedData(oApp, oAppFile, c0, c1, c2, c3, c4, ref bFound, ref TmpFieldValue);
						if ((bFound)) 
						{
							// For the sample, the "file handle" is really a string to a FilePNE that is an ini file
							IniUtils.SetPPS((string)oAppFile, c1, c2, FieldValue.ToString());
						}
					}
				} else {
					bMapContinue = false;
				}
				j = j + 1;
			}
		}
		public static void OurUpdateDocument(NxProject Project, string PlugInIniPNE, object oApp, ref bool bCreateAppFailed, NxDocRecord DocRec, string FilePNE)
		{
			//BEGIN
			int es = 0;
			// Create
			bool bThisCreatedApp = false;
			if ((AppConnect(ref oApp, ref bThisCreatedApp, ref bCreateAppFailed) == false))
				return;
			// Open
			object oAppFile = null;
			string ErrMsg = null;
			if ((AppOpenFile(ref oApp, FilePNE, false, ref oAppFile, ref ErrMsg) == false))
			{
				MessageBox.Show(ErrMsg);
				return;
			}
			// Properties
			int i = 0;
			int j = 0;
			string FieldId = null;
			string FieldName = null;
			string c0 = null;
			//string c1 = null;
			//string c2 = null;
			//string c3 = null;
			//string c4 = null;
			// fields and mapped data
			bool bContinue = true;
			i = 0;
			while (bContinue)
			{
				FieldId = IniUtils.GetPPS(PlugInIniPNE, c_IniFieldsSection, i.ToString());
				if (FieldId.Length < 1)
					break;
				FieldName = PIUtils.GetFieldName(Project, FieldId);
				NxFieldItem Field = Project.FieldManager.FieldList.FindId(FieldId);
				if ((Field == null)){ i = i + 1; continue; }
				NxFldVal FldVal = DocRec.FindField(FieldName);
				if ((FldVal == null)) { i = i + 1; continue; }
				NxFldDef FldDef = FldVal.FldDef;
				if ((FldDef == null)) { i = i + 1; continue; }
				ApiTypes.ADEPTT_FLD_DEF_TYPE FldDefType = (ApiTypes.ADEPTT_FLD_DEF_TYPE)FldDef.FldDefType;
				bool bMapContinue = true;
				j = 0;
				while (bMapContinue)
				{
					c0 = IniUtils.GetPPS(PlugInIniPNE, FieldId, "R" + j.ToString() + "C0");
					if (string.IsNullOrEmpty(c0))
						break;
					object FValue = null;
					if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_DATE))
					{
						FValue = FldVal.DateVal;
						string sFValue = FValue.ToString();
						string StrDate = "";
						Project.Login.Core.AdeptDateToStr(sFValue, StrDate);
						FValue = StrDate;
					}
					if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_CHARACTER))
					{
						FValue = FldVal.CharacterVal;
					}
					if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_STRING))
					{
						FValue = FldVal.StringVal;
					}
					// user fields only
					if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_LOGICAL))
					{
						FValue = FldVal.LogicalVal;
					}
					if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_INTEGER))
					{
						FValue = FldVal.IntegerVal;
					}
					if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_DOUBLE))
					{
						FValue = FldVal.DoubleVal;
					}
					if ((FldDefType == ApiTypes.ADEPTT_FLD_DEF_TYPE.FDT_MEMO))
					{
						FValue = FldVal.GetStringFromMemo();
					}
					SetMappedData(PlugInIniPNE, oApp, oAppFile, FieldId, FValue);
					j = j + 1;
				}
			
				i = i + 1;
			}
		
			es = DocRec.Update();
			// Close
			AppCloseFile(ref oAppFile, true, ref ErrMsg);
			// Release
			if ((bThisCreatedApp))
				AppDisconnect(ref oApp);
		}
		#endregion

		#region "Launch"
		public static void OurLaunch(string FilePNE)
		{
			 // ERROR: Not supported in C#: OnErrorStatement

			bool bCreated = false;
			object oApp = null;
			
			//oApp = GetObject(null, c_AppProgId);
			if ((oApp == null)) {
				bCreated = true;
				oApp = "Test";
				// oApp = CreateObject(c_AppProgId)
			}
			if ((oApp == null))
				return;

			// many apps will startup invisible if created with CreateObject 
			// oApp.Visible = True

			if (!string.IsNullOrEmpty(FilePNE))
			{
				//oApp.Open(FilePNE)
			} 
			else 
			{
				// be nice and open a new file
				if (bCreated)
				{
					// oApp.NewFile() 
				}
			}

			// since we have made the app visible, it shouldn't shutdown when we release our reference
			oApp = null;
		}
		#endregion



	}
}
