using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Interop.AdeptCAC;
using Interop.AdeptGui;
using Synergis.Adept.MainApi;

// NOTE1: To use Login and Logout through the Core, App.config needs to contain the WCF information found in Adept.exe.config.

// NOTE2: See below for changing test data to a record in your database.


namespace OutboardMemo
{
	public partial class OutboardMemoForm : Form
	{

		#region Class Members

		// NOTE2: Change this to C_REVISION to work with a REV record.
		const ApiTypes.NFM_DB_NUM _SourceTableNumber = ApiTypes.NFM_DB_NUM.C_FILES;
		
		// NOTE2: Change these to a valid record in your database for an IN, HOLD, OUT, or NEW record.
		const string _FileId = "f186226a-79c5-4c9c-8714-dfbad3ce65fa";
		const int _MajRev = 0;
		const int _MinRev = 1;

		GuiApi _GuiApi = null;
		NxProject _Project = null;
		NxDocRecord _DocRec = null;
		string _StartUpPath = null;
		string _FilePath = null;

		#endregion

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

		// Connect to a running Adept Client or start one up.
		private bool ConnectToAdept()
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
				return false;
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
			}
			catch (Exception)
			{
				MessageBox.Show("Login cancelled.");
				MyRelease(_Project);
				MyRelease(_GuiApi);
				_Project = null;
				_GuiApi = null;
				return false;
			}
			Cursor.Current = Cursors.Default;
			return true;
		}

		private bool CheckError(int es)
		{
			if (es != 0)
			{
				string ErrorString = null;
				_Project.Login.Core.GetErrorString(es, ErrorString);
				MessageBox.Show(string.Format("Error: {0}\n{1}", es.ToString(), ErrorString));
				return true;
			}
			return false;
		}


 
		public OutboardMemoForm()
		{
			InitializeComponent();
		}

		private void OutboardMemoForm_Load(object sender, EventArgs e)
		{
			bool bConnected = false;
			bConnected = ConnectToAdept();
			if ((!bConnected))
			{
				this.Close(); // System.Environment.Exit(0);
			}
			_DocRec = _Project.NewDocRecord();

			// Fetch the DocRecord
			int es = 0;
			es = _DocRec.CreateFromNumber((int)_SourceTableNumber);
			es = _DocRec.Set(_FileId, _MajRev, _MinRev);
			es = _DocRec.Fetch(1);

			if ((es != 0))
			{
				MyRelease(_DocRec);
				MyRelease(_Project);
				MyRelease(_GuiApi);
				MessageBox.Show("Record not found. Modify constants in OutboardMemoFrm.frm");
				this.Close(); //  System.Environment.Exit(0);
			}

			// Display the StartupPath
			_StartUpPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			_FilePath = System.IO.Path.Combine(_StartUpPath, "OutboardMemoFiles");
			if (!System.IO.Directory.Exists(_FilePath))
				System.IO.Directory.CreateDirectory(_FilePath);
			STStartUpPath.Text = _StartUpPath;
			STFilePath.Text = _FilePath;
		}

		private void OutboardMemoForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			 // release member objects
			MyRelease(_DocRec);
			MyRelease(_Project);
			MyRelease(_GuiApi);
		}

		private void BTClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		private void BTSetBmp_Click(object sender, EventArgs e)
		{
			string FilePNE = System.IO.Path.Combine(_FilePath, "Set.bmp");
			int es = _DocRec.SetThumbnail(FilePNE);
			if (CheckError(es) == true) return;
			MessageBox.Show("Done.");
		}

		private void BTSetEmf_Click(object sender, EventArgs e)
		{
			string FilePNE = System.IO.Path.Combine(_FilePath, "Set.emf");
			int es = _DocRec.SetThumbnail(FilePNE);
			if (CheckError(es) == true) return;
			MessageBox.Show("Done.");
		}

		private void BTClearThumbnail_Click(object sender, EventArgs e)
		{
			int es = _DocRec.SetThumbnail("");
			if (CheckError(es) == true) return;
			MessageBox.Show("Done.");
		}

		private void BTGetThumbnail_Click(object sender, EventArgs e)
		{
			string FilePNE = System.IO.Path.Combine(_FilePath, "Get");
			int es = _DocRec.GetThumbnail(ref FilePNE);
			if (CheckError(es) == true) return;
			MessageBox.Show("Created " + FilePNE);
		}

		private void BTSetTextMemo_Click(object sender, EventArgs e)
		{
			int es = _DocRec.SetTextMemoValue(RTBRichTextMemo.Rtf); // EBTextMemo.Text
			if (CheckError(es) == true) return;
			MessageBox.Show("Done.");
		}

		private void BTGetTextMemo_Click(object sender, EventArgs e)
		{
			RTBRichTextMemo.Text = "Pending Get..."; // EBTextMemo.Text = "Pending Get...";
			this.Refresh();
			System.Threading.Thread.Sleep(1000);

			int es = 0;
			string Value = _DocRec.GetTextMemoValue();
			RTBRichTextMemo.Rtf = Value; // EBTextMemo.Text = Value;
			if (CheckError(es) == true) return;
			MessageBox.Show("Done.");
		}

		private void BTSetApiMemo_Click(object sender, EventArgs e)
		{
			int es = _DocRec.SetApiMemoValue("T", EBApiMemo.Text);
			if (CheckError(es) == true) return;
			MessageBox.Show("Done.");
		}

		private void BTGetApiMemo_Click(object sender, EventArgs e)
		{
			EBApiMemo.Text = "Pending Get...";
			this.Refresh();
			System.Threading.Thread.Sleep(1000);

			int es = 0;
			string Value = _DocRec.GetApiMemoValue("T");
			EBApiMemo.Text = Value;
			if (CheckError(es) == true) return;
			MessageBox.Show("Done.");
		}

		private void BTSetJpg_Click(object sender, EventArgs e)
		{
			string FilePNE = System.IO.Path.Combine(_FilePath, "Set.jpg");
			int es = _DocRec.SetApiMemoFromFile("F", FilePNE);
			if (CheckError(es) == true) return;
			MessageBox.Show("Done.");
		}

		private void BTGetJpg_Click(object sender, EventArgs e)
		{
			string FilePNE = System.IO.Path.Combine(_FilePath, "Get.jpg");
			int es = _DocRec.GetFileFromApiMemo("F", FilePNE);
			if (CheckError(es) == true) return;
			MessageBox.Show("Created " + FilePNE);
		}



	}
}
