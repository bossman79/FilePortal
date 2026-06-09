using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ADODB;

using Interop.AdeptCAC;

// NOTE1: Change the Connection String.

namespace SqlUserAdmin
{
	public partial class SqlUserAdminForm : Form
	{

		#region Class Members
		NxCore _Core = null;
		ADODB.Connection _cnn1 = null;
		ADODB.Recordset _rst1 = null;
		string _ConnectionString = "DSN=MSSQLAdept;DATABASE=adept100;UID=root;PWD=";
		string _sqlstr = "";
		#endregion

		public SqlUserAdminForm()
		{
			InitializeComponent();
		}

		#region "Utilities"

		private string ToYesNo(bool bChecked)
		{
			if ((bChecked))
				return "Y";
			return "N";
		}

		private bool ToBool(object strobj)
		{
			string rstr = SafeStr(strobj);
			rstr = rstr.ToUpper().Trim();
			if ((rstr == "Y" || rstr == "T" || rstr == "1"))
			{
				return true;
			}
			return false;
		}

		private string SafeStr(object strobj)
		{
			string rtn = "";
			try
			{
				rtn = (string)strobj;
			}
			catch (Exception)
			{
				// ignore
			}
			return rtn;
		}

		#endregion

		private void SqlUserAdminForm_Load(object sender, EventArgs e)
		{
			// Get the core
			_Core = new NxCore();

			// create the connection
			_cnn1 = new ADODB.Connection();

			// open the connection
			var _with1 = _cnn1;
			_with1.ConnectionString = _ConnectionString;
			_with1.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
			_with1.Mode = ADODB.ConnectModeEnum.adModeReadWrite;
			_with1.Open();

			// create the recordset
			_rst1 = new ADODB.Recordset();
			_rst1.CursorType = ADODB.CursorTypeEnum.adOpenKeyset;
			_rst1.LockType = ADODB.LockTypeEnum.adLockOptimistic;

			// Load LibCards
			LoadLibCards();

			DoEnables();
		}

		private void LoadLibCards()
		{
			CBLibraryCardName.Items.Add("");

			try
			{
				_sqlstr = "select S_CARDNAME from fm100cards";
				_rst1.Open(_sqlstr, _cnn1);
				_rst1.MoveFirst();
				while (true)
				{
					string tmp = SafeStr(_rst1.Fields["S_CARDNAME"].Value);
					if (!string.IsNullOrEmpty(tmp))
					{
						CBLibraryCardName.Items.Add(tmp);
					}
					_rst1.MoveNext();
				}
			}
			catch (Exception)
			{
				// ignore
			}
			finally
			{
				_rst1.Close();
			}
		}


		private void SqlUserAdminForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// release the recordset
			_rst1 = null;
			// close the connection
			_cnn1.Close();
			// release the connection
			_cnn1 = null;
			// release the core
			_Core = null;
		}

		private void BTClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void DoEnables()
		{
			if (CHAdmin.Checked)
			{
				CHWorkflowAdmin.Checked = true; 
				CHSharedWorkAreaAdmin.Checked = true;
				CHTransmittalAdmin.Checked = true;
			}
			CHWorkflowAdmin.Enabled = !CHAdmin.Checked;
			CHSharedWorkAreaAdmin.Enabled = !CHAdmin.Checked;
			CHTransmittalAdmin.Enabled = !CHAdmin.Checked;

			CBLibraryCardName.Enabled = !CHChooseLibraryCard.Checked;
		}

		private void BTClear_Click(object sender, EventArgs e)
		{
			EBUserId.Text = "";
			EBLoginName.Text = "";
			EBUserName.Text = "";
			EBPassword.Text = "";
			EBEmailAddress.Text = "";
			EBActiveDirectoryDomain.Text = "";
			
			CHAdmin.Checked = false;
			CHWorkflowAdmin.Checked = false;
			CHSharedWorkAreaAdmin.Checked = false;
			CHTransmittalAdmin.Checked = false;

			CHArchiver.Checked = false;
			CHChooseLibraryCard.Checked = false;
			CBLibraryCardName.SelectedIndex = 0;

			CHLockedLicense.Checked = false;
			CHReviewer.Checked = false;
			CHProViewer.Checked = false;
		}

		private void BTAddUser_Click(object sender, EventArgs e)
		{
			_rst1.Open("fm100usr", _cnn1);
			_rst1.AddNew();

			_rst1.Fields["S_Login"].Value = EBLoginName.Text;

			// generate a new UserId
			string UserId = _Core.CreateId();
			_rst1.Fields["S_UserId"].Value = UserId;
			EBUserId.Text = UserId;

			_rst1.Fields["S_Name"].Value = EBUserName.Text;
			_rst1.Fields["S_Password"].Value = EBPassword.Text;
			_rst1.Fields["S_EAddress"].Value = EBEmailAddress.Text;
			_rst1.Fields["S_EProfile"].Value = EBActiveDirectoryDomain.Text;
			_rst1.Fields["S_MaskName"].Value = CBLibraryCardName.Text;

			_rst1.Fields["S_Admin"].Value = ToYesNo(CHAdmin.Checked);
			_rst1.Fields["S_Arch"].Value = ToYesNo(CHArchiver.Checked);
			_rst1.Fields["S_ProView"].Value = ToYesNo(CHProViewer.Checked);
			_rst1.Fields["S_SWAdmin"].Value = ToYesNo(CHSharedWorkAreaAdmin.Checked);
			_rst1.Fields["S_WfAdmin"].Value = ToYesNo(CHWorkflowAdmin.Checked);
			_rst1.Fields["S_PickCard"].Value = ToYesNo(CHChooseLibraryCard.Checked);

			_rst1.Update();
			_rst1.Close();

			MessageBox.Show("Added user with LoginName: " + EBLoginName.Text);
		}

		private void BTGetUserInfo_Click(object sender, EventArgs e)
		{
			_sqlstr = "select * from fm100usr where S_login = '" + EBLoginName.Text + "'";
			_rst1.Open(_sqlstr, _cnn1);
			if (_rst1.RecordCount < 1) 
			{
				MessageBox.Show("LoginName not found: " + EBLoginName.Text);
				return;
			}
			_rst1.MoveFirst();

			// LoginName is the key

			EBUserId.Text = SafeStr(_rst1.Fields["S_USERID"].Value);

			EBUserName.Text = SafeStr(_rst1.Fields["S_Name"].Value);
			EBPassword.Text = SafeStr(_rst1.Fields["S_Password"].Value);
			EBEmailAddress.Text = SafeStr(_rst1.Fields["S_EAddress"].Value);
			EBActiveDirectoryDomain.Text = SafeStr(_rst1.Fields["S_EProfile"].Value);
			CBLibraryCardName.Text = SafeStr(_rst1.Fields["S_MaskName"].Value);

			CHAdmin.Checked = ToBool(_rst1.Fields["S_Admin"].Value);
			CHArchiver.Checked = ToBool(_rst1.Fields["S_Arch"].Value);
			CHProViewer.Checked = ToBool(_rst1.Fields["S_ProView"].Value);
			CHSharedWorkAreaAdmin.Checked = ToBool(_rst1.Fields["S_SWAdmin"].Value);
			CHWorkflowAdmin.Checked = ToBool(_rst1.Fields["S_WfAdmin"].Value);
			CHChooseLibraryCard.Checked = ToBool(_rst1.Fields["S_PickCard"].Value);

			_rst1.MoveFirst();
			_rst1.MoveLast();
			_rst1.Close();

			MessageBox.Show("Got user with LoginName: " + EBLoginName.Text);
		}

		private void BTModifyUser_Click(object sender, EventArgs e)
		{
			_sqlstr = "select * from fm100usr where S_USERID = '" + EBUserId.Text + "'";
			_rst1.Open(_sqlstr, _cnn1);
			_rst1.MoveFirst();

			_rst1.Fields["S_Login"].Value = EBLoginName.Text;

			// UserId is the key

			_rst1.Fields["S_Name"].Value = EBUserName.Text;
			_rst1.Fields["S_Password"].Value = EBPassword.Text;
			_rst1.Fields["S_EAddress"].Value = EBEmailAddress.Text;
			_rst1.Fields["S_EProfile"].Value = EBActiveDirectoryDomain.Text;
			_rst1.Fields["S_MaskName"].Value = CBLibraryCardName.SelectedValue;

			_rst1.Fields["S_Admin"].Value = ToYesNo(CHAdmin.Checked);
			_rst1.Fields["S_Arch"].Value = ToYesNo(CHArchiver.Checked);
			_rst1.Fields["S_ProView"].Value = ToYesNo(CHProViewer.Checked);
			_rst1.Fields["S_SWAdmin"].Value = ToYesNo(CHSharedWorkAreaAdmin.Checked);
			_rst1.Fields["S_WfAdmin"].Value = ToYesNo(CHWorkflowAdmin.Checked);
			_rst1.Fields["S_PickCard"].Value = ToYesNo(CHChooseLibraryCard.Checked);

			_rst1.Update();
			_rst1.Close();

			MessageBox.Show("Modified user with UserId: " + EBUserId.Text);
		}

		private void BTRemoveUser_Click(object sender, EventArgs e)
		{
			string Msg = "";
			Msg += "WARNING! Do not delete users who have ever logged in to Adept.\n";
			Msg += "Use Adept to delete a user that has logged in.\n";
			Msg += "Delete anyway?";
			if (MessageBox.Show(Msg, "WARNING!", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
				return;

			_sqlstr = "select * from fm100usr where S_UserId = '" + EBUserId.Text + " '";
			_rst1.Open(_sqlstr, _cnn1);
			_rst1.MoveFirst();

			// UserId is the key

			_rst1.Delete();
			_rst1.Close();

			MessageBox.Show("Deleted user with UserId: " + EBUserId.Text);
		}

		private void BTDisableUser_Click(object sender, EventArgs e)
		{
			_sqlstr = "select * from fm100usr where S_UserId = '" + EBUserId.Text + "'";
			_rst1.Open(_sqlstr, _cnn1);
			_rst1.MoveFirst();

			// UserId is the key

			_rst1.Fields["S_Command"].Value = "LOGOUT:";

			_rst1.Update();
			_rst1.Close();

			MessageBox.Show("User Disabled");
		}

		private void BTEnableUser_Click(object sender, EventArgs e)
		{
			_sqlstr = "select * from fm100usr where S_UserId = '" + EBUserId.Text + "'";
			_rst1.Open(_sqlstr, _cnn1);
			_rst1.MoveFirst();

			// UserId is the key

			_rst1.Fields["S_Command"].Value = "";

			_rst1.Update();
			_rst1.Close();

			MessageBox.Show("User Enabled");
		}

		private void BTIsUserEnabled_Click(object sender, EventArgs e)
		{
			_sqlstr = "select * from fm100usr where S_UserId = '" + EBUserId.Text + "'";
			_rst1.Open(_sqlstr, _cnn1);
			_rst1.MoveFirst();

			// UserId is the key
			string CommandStr = SafeStr(_rst1.Fields["S_Command"].Value);

			_rst1.Close();

			if(string.Compare(CommandStr, "LOGOUT:", true) != 0)
				MessageBox.Show("User Enabled");
			else
				MessageBox.Show("User Disabled");
		}

		private void CHAdmin_CheckedChanged(object sender, EventArgs e)
		{
			DoEnables();
		}

		private void CHChooseLibraryCard_CheckedChanged(object sender, EventArgs e)
		{
			DoEnables();
		}

		private void BTTriggerChanges_Click(object sender, EventArgs e)
		{
			string BumpGuid = _Core.CreateId();


			_sqlstr = "select * from fm100version where S_COMMENT = 'TGGroupUser'";
			_rst1.Open(_sqlstr, _cnn1);
			_rst1.MoveFirst();

			_rst1.Fields["S_VerId"].Value = BumpGuid;

			_rst1.Update();
			_rst1.Close();

			MessageBox.Show("User Table Group Bumped.");
		}

		


	}
}
