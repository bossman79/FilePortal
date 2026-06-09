using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Interop.AdeptCAC;
using Interop.AdeptGui;
using Synergis.Adept.MainApi;

// NOTE1: To use Login and Logout through the Core, App.config needs to contain the WCF information found in Adept.exe.config.

// NOTE2: See below for commenting out fields that may not exist in your database.

namespace LoginAndSearch
{
	public partial class LoginAndSearchForm : Form
	{

		#region Class Members

		GuiApi _GuiApi = null;		// Used by Connect
		NxCore _Core = null;		// Used from FormLoad until Closing
		NxDomain _Domain = null;	// Used by Login
		NxLogin _Login = null;		// Used by Login
		NxProject _Project = null;	// Used by Connect and Login
		NxTbl _Tbl = null;			// The current scroll table of the search results.
		bool _bHasResults = false;  // Is there a current search results.

		#endregion

		#region ComboBox Helpers and MyRelease

		// Helper calss for the Language combo.
		private class CBItemLang
		{
			public string _CultureName;
			public string _CultureDisplayName;
			public CBItemLang(string CultureName, string CultureDisplayName)
			{
				_CultureName = CultureName;
				_CultureDisplayName = CultureDisplayName;
			}
			public override string ToString()
			{
				return _CultureDisplayName;
			}
		}

		// Helper class for the Schema (Column List) combo.
		private class CBItemSchemaItem
		{
			public string _DisplayName;
			public string _Id;
			public CBItemSchemaItem(string DisplayName, string Id)
			{
				_DisplayName = DisplayName;
				_Id = Id;
			}
			public override string ToString()
			{
				return _DisplayName;
			}
		}

		// Helper class for the SearchOp combo.
		private class CBItemSearchOp
		{
			public string _OpName;
			public ApiTypes.ADEPTT_SEARCHOP _OpCode;
			public CBItemSearchOp(string OpName, ApiTypes.ADEPTT_SEARCHOP OpCode)
			{
				_OpName = OpName;
				_OpCode = OpCode;
			}
			public override string ToString()
			{
				return _OpName;
			}
		}

		// do a 'hard' release of an interop object
		void MyRelease(object obj)
		{
			try
			{
				if (obj != null)
				{
					System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
					//obj = null; // not a ref
				}
			}
			catch (Exception) // ex
			{
				//AddText("Exception: " + ex.Message);
			}
		}

		#endregion

		#region Form

		// Constructor
		public LoginAndSearchForm()
		{
			InitializeComponent();
		}

		// Load
		private void LoginAndSearchForm_Load(object sender, EventArgs e)
		{
			try
			{
				// Set default enables
				DoEnables();

				// connect to the Core in case the user wants to Login
				_Core = new NxCore();
				// Initialize the Core
				string errstr = "";
				int es = _Core.Initialize(0);
				if (es != 0)
				{	// Initialize Failed
					_Core.GetErrorString(es, ref errstr);
					return;
				}
				// Walk the list of available domains.
				NxDomainList dl = _Core.DomainList;
				foreach (NxDomain d in dl)
				{
					// Add to the Domain combo as a string
					int newIndex = CBDomain.Items.Add(d.Name);
					if (d.Name == dl.LastDomainName)
					{
						CBDomain.SelectedIndex = newIndex;
						TBLoginName.Text = d.LastLoginName;
					}
					MyRelease(d);
				}
				MyRelease(dl);
				dl = null;
				// Get the last language so that we can make it the default.
				string LastLang = _Core.GetLastLanguage();
				if (string.IsNullOrEmpty(LastLang))
					LastLang = "en-US";
				// Walk the list of installed languages.
				string tmp = _Core.GetInstalledLanguages();
				string[] langList = tmp.Split('|');
				foreach (string lang in langList)
				{
					// Add to the Language combo.
					string displayName = _Core.GetCultureDisplayName(lang);
					int index = CBLanguage.Items.Add(new CBItemLang(lang, displayName));
					if (string.Compare(lang, LastLang) == 0)
						CBLanguage.SelectedIndex = index;
				}
				// Fill SearchOp combos now (doesn't need a login).
				FillSearchOpCombo(CBSearchOpA);
				FillSearchOpCombo(CBSearchOpB);
				FillSearchOpCombo(CBSearchOpC);
				// Add our system test columns.
				LVMain.Columns.Add("Filename");
				LVMain.Columns.Add("Libname");
				// NOTE2:
				// Add client specific columns (that may not exist in every database).
				// The programmer should comment these out or change them to valid fields in their database.
				LVMain.Columns.Add("Client");
				LVMain.Columns.Add("Descrip");
				LVMain.Columns.Add("Design");
				LVMain.Columns.Add("Author");
				LVMain.Columns.Add("Company");
				LVMain.Columns.Add("Title");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void LoginAndSearchForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// We are done with the Core now.
			MyRelease(_Core);
			_Core = null;
		}

		#endregion

		#region Connect and Disconnect

		// Connect to a running Adept Client or start one up.
		private void BTConnect_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			// Connect to Client
			try
			{
				_GuiApi = new GuiApi();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				MyRelease(_GuiApi);
				_GuiApi = null;
				return;
			}	
			// Wait for Login to complete, or get the Project if already logged in
			try
			{
				// This will throw an exception if the user cancels the login.
				_Project = (NxProject)_GuiApi.GetProject();
				while (_Project == null)
				{
					_Project = (NxProject)_GuiApi.GetProject();
					System.Threading.Thread.Sleep(1000);
				}
				// Now that we are in, load the schema.
				FillFieldNameCombos();
			}
			catch (Exception)
			{
				MessageBox.Show("Login cancelled.");
				MyRelease(_Project);
				MyRelease(_GuiApi);
				_Project = null;
				_GuiApi = null;
			}
			Cursor.Current = Cursors.Default;
			// Enable or disable controls.
			DoEnables();
		}

		private void BTDisconnect_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				// Clear results.
				MyRelease(_Tbl);
				_Tbl = null;
				_bHasResults = false;
				LVMain.Items.Clear();
				// Disconnect.
				MyRelease(_Project);
				MyRelease(_GuiApi);
				_Project = null;
				_GuiApi = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			Cursor.Current = Cursors.Default;
			// Enable or disable controls.
			DoEnables();
		}

		#endregion

		#region Login and Logout

		// Login through the Core.
		private void BTLogin_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				// Find the selected domain.
				NxDomainList dl = _Core.DomainList;
				_Domain = dl.FindName(CBDomain.Text);
				MyRelease(dl);
				if (_Domain == null)
				{
					MessageBox.Show("Failed to find Domain Name.");
					return;
				}

				// Start the timer.
				DateTime dtStart = DateTime.Now;

				// Do the login.
				int bSystemLocked = 0;
				string SystemLockMsg = "";
				int bAlreadyLoggedIn = 7;
				int es = 0;
				string errstr = "";
				_Login = _Domain.Login(TBLoginName.Text, "", 1, ref bSystemLocked, ref SystemLockMsg, ref bAlreadyLoggedIn, ref es);
				if (_Login == null || es != 0)
				{
					MyRelease(_Domain);
					_Domain = null;
					_Core.GetErrorString(es, ref errstr);
					MessageBox.Show(string.Format("Login failed. es: {0}, msg: {1}", es.ToString(), errstr));
					return;
				}
				// Get the project.
				_Project = _Login.Project;
				if (_Project == null)
				{
					_Domain.Logout(TBLoginName.Text);
					MyRelease(_Domain);
					_Domain = null;
					MessageBox.Show("Failed to get Project.");
					return;
				}

				// Stop the timer.
				DateTime dtStop = DateTime.Now;

				// Update Login time label.
				TimeSpan ts = dtStop - dtStart;
				LBLLoginTime.Text = ts.ToString();

				// Now that we are in, load the schema.
				FillFieldNameCombos();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			Cursor.Current = Cursors.Default;
			// Enable or disable controls.
			DoEnables();
		}

		private void BTLogout_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				// Clear results.
				MyRelease(_Tbl);
				_Tbl = null;
				_bHasResults = false;
				LVMain.Items.Clear();
				// Logout.
				string LoginName = _Login.LoginName;
				_Domain.Logout(LoginName);
				MyRelease(_Project);
				MyRelease(_Login);
				MyRelease(_Domain);
				_Project = null;
				_Login = null;
				_Domain = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			Cursor.Current = Cursors.Default;
			// Enable or disable controls.
			DoEnables();
		}

		#endregion

		#region DoEnables and Fills

		void DoEnables()
		{
			// Init.
			bool bInGui = false;
			bool bInCore = false;

			// Are we in the Gui?
			if (_GuiApi != null)
				bInGui = true;
			// Otherwise, if there is a Project, then we are in the Core.
			else if(_Project != null)
				bInCore = true;

			// Are we in either way?
			bool bIn = bInGui || bInCore;

			// If not in anything, then enable either way to get in.
			BTConnect.Enabled = !bIn;
			BTLogin.Enabled = !bIn;

			// If in Gui then enable Disconnect.
			BTDisconnect.Enabled = bInGui;

			// If in Core then enable Logout.
			BTLogout.Enabled = bInCore;

			// If we are not In, then we can type into the Login controls.
			TBLoginName.Enabled = !bIn;
			CBDomain.Enabled = !bIn;
			CBLanguage.Enabled = !bIn;

			// If we are In, then enable the Search and Result controls.
			CBSchemaItemA.Enabled = bIn;
			CBSchemaItemB.Enabled = bIn;
			CBSchemaItemC.Enabled = bIn;
			CBSearchOpA.Enabled = bIn;
			CBSearchOpB.Enabled = bIn;
			CBSearchOpC.Enabled = bIn;
			TBValue1A.Enabled = bIn;
			TBValue1B.Enabled = bIn;
			TBValue1C.Enabled = bIn;
			TBValue2A.Enabled = bIn;
			TBValue2B.Enabled = bIn;
			TBValue2C.Enabled = bIn;
			RBAnd.Enabled = bIn;
			RBOr.Enabled = bIn;
			RBNew.Enabled = bIn;
			RBAppend.Enabled = bIn && _bHasResults;
			RBRefine.Enabled = bIn && _bHasResults;
			BTSearch.Enabled = bIn;
			BTGiveTable.Enabled = bInGui;
			LVMain.Enabled = bIn;
		}

		// Load the schema into the Field Name combo boxes.
		private void FillFieldNameCombos()
		{
			NxSchemaManager SchemaManager = null;
			NxSchemaList SchemaList = null;
			try
			{
				// They should be empty already.
				CBSchemaItemA.Items.Clear();
				CBSchemaItemB.Items.Clear();
				CBSchemaItemC.Items.Clear();
				// Allow the user to blank the last 2 controls.
				CBSchemaItemB.Items.Add(new CBItemSchemaItem("", ""));
				CBSchemaItemC.Items.Add(new CBItemSchemaItem("", ""));
				// Walk the schema.
				SchemaManager = _Project.SchemaManager;
				SchemaList = SchemaManager.SchemaList;
				foreach (NxSchemaItem SchemaItem in SchemaList)
				{
					// Make a new item.
					CBItemSchemaItem cbi_si = new CBItemSchemaItem(SchemaItem.DisplayName, SchemaItem.Id);
					MyRelease(SchemaItem);
					// Add it to the 1st control.
					int index = CBSchemaItemA.Items.Add(cbi_si);
					// If this is the Filename field, make it the default.
					if (string.Compare(cbi_si._Id, "SCHEMA_S_LONGNAME") == 0)
						CBSchemaItemA.SelectedIndex = index;
					// Add it to the other 2 controls.
					CBSchemaItemB.Items.Add(cbi_si);
					CBSchemaItemC.Items.Add(cbi_si);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				MyRelease(SchemaList);
				MyRelease(SchemaManager);
			}
		}

		private void FillSearchOpCombo(ComboBox CBCtrl)
		{
			// Load ops into 1 combo and make "Starts With" the default.
			CBCtrl.Items.Add(new CBItemSearchOp("None", ApiTypes.ADEPTT_SEARCHOP.OP_NONE));
			int iIndex = CBCtrl.Items.Add(new CBItemSearchOp("Starts With", ApiTypes.ADEPTT_SEARCHOP.OP_STARTS));
			CBCtrl.SelectedIndex = iIndex;
			CBCtrl.Items.Add(new CBItemSearchOp("Equals", ApiTypes.ADEPTT_SEARCHOP.OP_EQUALS));
			CBCtrl.Items.Add(new CBItemSearchOp("Does Not Start With", ApiTypes.ADEPTT_SEARCHOP.OP_DOESNOTSTART));
			CBCtrl.Items.Add(new CBItemSearchOp("Contains", ApiTypes.ADEPTT_SEARCHOP.OP_CONTAINS));
			CBCtrl.Items.Add(new CBItemSearchOp("Does Not Contain", ApiTypes.ADEPTT_SEARCHOP.OP_DOESNOTCONTAIN));
			CBCtrl.Items.Add(new CBItemSearchOp("Less Than", ApiTypes.ADEPTT_SEARCHOP.OP_LESSTHAN));
			CBCtrl.Items.Add(new CBItemSearchOp("Greater Than", ApiTypes.ADEPTT_SEARCHOP.OP_GREATERTHAN));
			CBCtrl.Items.Add(new CBItemSearchOp("Is Empty", ApiTypes.ADEPTT_SEARCHOP.OP_EMPTY));
			CBCtrl.Items.Add(new CBItemSearchOp("Is Not Empty", ApiTypes.ADEPTT_SEARCHOP.OP_NOTEMPTY));
		}

		#endregion

		#region Search and GiveTable

		// Add a search item to the query table's field list.
		private void AddSearchItem(NxSchemaList SchemaList, NxQueryFieldList qfl, ComboBox CBSchemaItem, ComboBox CBSearchOp, TextBox TBValue1, TextBox TBValue2)
		{
			NxSchemaItem SchemaItem = null;
			NxQueryFieldItem qfi = null;
			try
			{
				// Is a schema selected?
				if (CBSchemaItem.SelectedItem != null)
				{
					CBItemSchemaItem cbi_si = (CBItemSchemaItem)CBSchemaItem.SelectedItem;
					if (!string.IsNullOrEmpty(cbi_si._Id))
					{
						// Look up the schema by name.
						SchemaItem = SchemaList.FindId(cbi_si._Id);
						if (SchemaItem == null)
						{
							MessageBox.Show("Schema Item not found for " + cbi_si._DisplayName);
							return;
						}
						// Add an item with search op and value.
						qfi = qfl.Add(SchemaItem.Id);
						qfi.SearchOp = (int)((CBItemSearchOp)CBSearchOp.SelectedItem)._OpCode;
						qfi.Value = TBValue1.Text;
						qfi.Value2 = TBValue2.Text;
						qfi = null;
						MyRelease(SchemaItem);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				MyRelease(SchemaItem);
				MyRelease(qfi);
			}
		}

		private void BTSearch_Click(object sender, EventArgs e)
		{
			NxSchemaManager SchemaManager = null;
			NxSchemaList SchemaList = null;
			NxQueryTable qt = null;
			NxQueryFieldList qfl = null;
			try
			{
				SchemaManager = _Project.SchemaManager;
				SchemaList = SchemaManager.SchemaList;
				// Make a query table object.
				qt = _Project.CreateQueryTable();
				qt.TableName = "fm100fil";
				qfl = qt.QueryFieldList;
				// Add the criteria.
				AddSearchItem(SchemaList, qfl, CBSchemaItemA, CBSearchOpA, TBValue1A, TBValue2A);
				AddSearchItem(SchemaList, qfl, CBSchemaItemB, CBSearchOpB, TBValue1B, TBValue2B);
				AddSearchItem(SchemaList, qfl, CBSchemaItemC, CBSearchOpC, TBValue1C, TBValue2C);
				// Release any existing table.
				MyRelease(_Tbl);
				// Decide what kind of search to do and do it.
				int iOrSearch = RBOr.Checked ? 1 : 0;
				if (RBNew.Checked)
				{
					_Tbl = qt.DoSearchEx(iOrSearch, "MySearch");
				}
				else
				{
					if (RBAppend.Checked)
					{
						_Tbl = qt.DoSearchAppend("MySearch", iOrSearch, "MySearch");
					}
					else
					{
						_Tbl = qt.DoSearchRefine("MySearch", iOrSearch, "MySearch");
					}
				}
				// Are there any results?
				if (_Tbl == null)
				{
					LVMain.Items.Add("No table returned.");
					return;
				}
				// Remember that we have results.
				_bHasResults = true;
				// Show the results.
				ShowTable(_Tbl);
				// Enable or disable controls.
				DoEnables();
			}
			catch (Exception ex)
			{
				_bHasResults = false;
				MyRelease(_Tbl);
				_Tbl = null;
				MessageBox.Show(ex.Message);
			}
			finally
			{
				MyRelease(qfl);
				MyRelease(qt);
				MyRelease(SchemaList);
				MyRelease(SchemaManager);
			}
		}

		void ShowTable(NxTbl Tbl)
		{
			// Clear any existing results.
			LVMain.Items.Clear();
			// This should never be null be cause null was already handled.
			if (Tbl == null)
				return;
			// Get the total count in the server scroll table.
			int SrvCount = Tbl.SrvGetCount();
			if (SrvCount < 1)
			{
				LVMain.Items.Add("Empty table returned.");
				return;
			}
			// Walk this screenful.
			int i = 1;
			string FileNE = "";
			while(i <= Tbl.GetCount())
			{
				Tbl.GotoRecord(i);
				// Get the system columns.
				FileNE = Tbl.GetDisplayStringFromSchemaId("SCHEMA_S_LONGNAME");
				ListViewItem lvi = LVMain.Items.Add(FileNE);
				lvi.SubItems.Add(Tbl.GetDisplayStringFromSchemaId("SCHEMA_S_LIBNAME"));

				// NOTE2:
				// Add client specific columns (that may not exist in every database).
				// The programmer should comment these out or change them to valid fields in their database.
				// Older columns may have Schema Ids that are readable, but newer Ids are Guids.
				lvi.SubItems.Add(Tbl.GetDisplayStringFromSchemaId("SCHEMA_G_CLIENT"));
				lvi.SubItems.Add(Tbl.GetDisplayStringFromSchemaId("SCHEMA_G_DESCRIP"));
				lvi.SubItems.Add(Tbl.GetDisplayStringFromSchemaId("SCHEMA_G_DESIGN"));
				lvi.SubItems.Add(Tbl.GetDisplayStringFromSchemaId("04ebd417-120b-4560-84df-a7ff63fec664"));
				lvi.SubItems.Add(Tbl.GetDisplayStringFromSchemaId("214fdecf-6ee5-41d0-8cce-1e792f6b1160"));
				lvi.SubItems.Add(Tbl.GetDisplayStringFromSchemaId("233de711-8f6c-4cdc-8c53-17a662fbecb5"));

				// If on the last record in the screenful,...
				if(Tbl.GetCurrentRecordNumber() == Tbl.GetCount())
				{
					// Calc the next record.
					int nextrec = Tbl.SrvGetLastRecordNumber() + 1;
					// If the current last record in the screenful was the last record in the results, then we are done.
					if (nextrec > SrvCount) 
						return;
					// Get the next screenful.
					int ReturnRecordCount = 20;
					Tbl.SrvScroll(nextrec, ReturnRecordCount);
					// Reset the screenful position.
					i = 1;
				}
				else
				{
					// Set to the next row.
					i = i + 1;
				}
			}
		}

		// Give the results to the Adept UI's Search Results window.
		private void BTGiveTable_Click(object sender, EventArgs e)
		{
			try
			{
				// Scroll to the top.
				_Tbl.SrvScroll(1, 20);
				// Give the table to the Adept Client.
				_GuiApi.GiveTableToSearchWindow(_Tbl, 0, "Test");
				// Now we must release the table and have no further contact with it because Adept Client may close it.
				MyRelease(_Tbl);
				_Tbl = null;
				// Clear our results.
				_bHasResults = false;
				LVMain.Items.Clear();
				// Enable or disable controls.
				DoEnables();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion

	}
}
