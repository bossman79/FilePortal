namespace SqlUserAdmin
{
	partial class SqlUserAdminForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.CHTransmittalAdmin = new System.Windows.Forms.CheckBox();
			this.BTClose = new System.Windows.Forms.Button();
			this.BTIsUserEnabled = new System.Windows.Forms.Button();
			this.BTEnableUser = new System.Windows.Forms.Button();
			this.BTDisableUser = new System.Windows.Forms.Button();
			this.BTRemoveUser = new System.Windows.Forms.Button();
			this.BTModifyUser = new System.Windows.Forms.Button();
			this.BTGetUserInfo = new System.Windows.Forms.Button();
			this.BTAddUser = new System.Windows.Forms.Button();
			this.BTClear = new System.Windows.Forms.Button();
			this.CHChooseLibraryCard = new System.Windows.Forms.CheckBox();
			this.CHWorkflowAdmin = new System.Windows.Forms.CheckBox();
			this.CHSharedWorkAreaAdmin = new System.Windows.Forms.CheckBox();
			this.CHProViewer = new System.Windows.Forms.CheckBox();
			this.CHArchiver = new System.Windows.Forms.CheckBox();
			this.CHAdmin = new System.Windows.Forms.CheckBox();
			this.EBActiveDirectoryDomain = new System.Windows.Forms.TextBox();
			this.EBEmailAddress = new System.Windows.Forms.TextBox();
			this.EBPassword = new System.Windows.Forms.TextBox();
			this.EBUserName = new System.Windows.Forms.TextBox();
			this.EBUserId = new System.Windows.Forms.TextBox();
			this.EBLoginName = new System.Windows.Forms.TextBox();
			this.Label7 = new System.Windows.Forms.Label();
			this.Label6 = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.CHReviewer = new System.Windows.Forms.CheckBox();
			this.CHLockedLicense = new System.Windows.Forms.CheckBox();
			this.GBAdministrationTypes = new System.Windows.Forms.GroupBox();
			this.GBOtherPermissions = new System.Windows.Forms.GroupBox();
			this.CBLibraryCardName = new System.Windows.Forms.ComboBox();
			this.GBOther = new System.Windows.Forms.GroupBox();
			this.BTTriggerChanges = new System.Windows.Forms.Button();
			this.GBAdministrationTypes.SuspendLayout();
			this.GBOtherPermissions.SuspendLayout();
			this.GBOther.SuspendLayout();
			this.SuspendLayout();
			// 
			// CHTransmittalAdmin
			// 
			this.CHTransmittalAdmin.Location = new System.Drawing.Point(6, 97);
			this.CHTransmittalAdmin.Name = "CHTransmittalAdmin";
			this.CHTransmittalAdmin.Size = new System.Drawing.Size(200, 20);
			this.CHTransmittalAdmin.TabIndex = 59;
			this.CHTransmittalAdmin.Text = "TransmittalAdmin";
			// 
			// BTClose
			// 
			this.BTClose.Location = new System.Drawing.Point(387, 9);
			this.BTClose.Name = "BTClose";
			this.BTClose.Size = new System.Drawing.Size(96, 23);
			this.BTClose.TabIndex = 58;
			this.BTClose.Text = "Close";
			this.BTClose.Click += new System.EventHandler(this.BTClose_Click);
			// 
			// BTIsUserEnabled
			// 
			this.BTIsUserEnabled.Location = new System.Drawing.Point(387, 377);
			this.BTIsUserEnabled.Name = "BTIsUserEnabled";
			this.BTIsUserEnabled.Size = new System.Drawing.Size(96, 23);
			this.BTIsUserEnabled.TabIndex = 57;
			this.BTIsUserEnabled.Text = "Is User Enabled";
			this.BTIsUserEnabled.Click += new System.EventHandler(this.BTIsUserEnabled_Click);
			// 
			// BTEnableUser
			// 
			this.BTEnableUser.Location = new System.Drawing.Point(387, 345);
			this.BTEnableUser.Name = "BTEnableUser";
			this.BTEnableUser.Size = new System.Drawing.Size(96, 23);
			this.BTEnableUser.TabIndex = 56;
			this.BTEnableUser.Text = "Enable User";
			this.BTEnableUser.Click += new System.EventHandler(this.BTEnableUser_Click);
			// 
			// BTDisableUser
			// 
			this.BTDisableUser.Location = new System.Drawing.Point(387, 313);
			this.BTDisableUser.Name = "BTDisableUser";
			this.BTDisableUser.Size = new System.Drawing.Size(96, 23);
			this.BTDisableUser.TabIndex = 55;
			this.BTDisableUser.Text = "Disable User";
			this.BTDisableUser.Click += new System.EventHandler(this.BTDisableUser_Click);
			// 
			// BTRemoveUser
			// 
			this.BTRemoveUser.Location = new System.Drawing.Point(387, 257);
			this.BTRemoveUser.Name = "BTRemoveUser";
			this.BTRemoveUser.Size = new System.Drawing.Size(96, 23);
			this.BTRemoveUser.TabIndex = 54;
			this.BTRemoveUser.Text = "Remove User";
			this.BTRemoveUser.Click += new System.EventHandler(this.BTRemoveUser_Click);
			// 
			// BTModifyUser
			// 
			this.BTModifyUser.Location = new System.Drawing.Point(387, 225);
			this.BTModifyUser.Name = "BTModifyUser";
			this.BTModifyUser.Size = new System.Drawing.Size(96, 23);
			this.BTModifyUser.TabIndex = 53;
			this.BTModifyUser.Text = "Modify User";
			this.BTModifyUser.Click += new System.EventHandler(this.BTModifyUser_Click);
			// 
			// BTGetUserInfo
			// 
			this.BTGetUserInfo.Location = new System.Drawing.Point(387, 193);
			this.BTGetUserInfo.Name = "BTGetUserInfo";
			this.BTGetUserInfo.Size = new System.Drawing.Size(96, 23);
			this.BTGetUserInfo.TabIndex = 52;
			this.BTGetUserInfo.Text = "Get User Info";
			this.BTGetUserInfo.Click += new System.EventHandler(this.BTGetUserInfo_Click);
			// 
			// BTAddUser
			// 
			this.BTAddUser.Location = new System.Drawing.Point(387, 161);
			this.BTAddUser.Name = "BTAddUser";
			this.BTAddUser.Size = new System.Drawing.Size(96, 23);
			this.BTAddUser.TabIndex = 51;
			this.BTAddUser.Text = "Add User";
			this.BTAddUser.Click += new System.EventHandler(this.BTAddUser_Click);
			// 
			// BTClear
			// 
			this.BTClear.Location = new System.Drawing.Point(387, 105);
			this.BTClear.Name = "BTClear";
			this.BTClear.Size = new System.Drawing.Size(96, 23);
			this.BTClear.TabIndex = 50;
			this.BTClear.Text = "Clear";
			this.BTClear.Click += new System.EventHandler(this.BTClear_Click);
			// 
			// CHChooseLibraryCard
			// 
			this.CHChooseLibraryCard.Location = new System.Drawing.Point(6, 45);
			this.CHChooseLibraryCard.Name = "CHChooseLibraryCard";
			this.CHChooseLibraryCard.Size = new System.Drawing.Size(200, 20);
			this.CHChooseLibraryCard.TabIndex = 49;
			this.CHChooseLibraryCard.Text = "Choose Library Card";
			this.CHChooseLibraryCard.CheckedChanged += new System.EventHandler(this.CHChooseLibraryCard_CheckedChanged);
			// 
			// CHWorkflowAdmin
			// 
			this.CHWorkflowAdmin.Location = new System.Drawing.Point(6, 45);
			this.CHWorkflowAdmin.Name = "CHWorkflowAdmin";
			this.CHWorkflowAdmin.Size = new System.Drawing.Size(200, 20);
			this.CHWorkflowAdmin.TabIndex = 48;
			this.CHWorkflowAdmin.Text = "Workflow Admin";
			// 
			// CHSharedWorkAreaAdmin
			// 
			this.CHSharedWorkAreaAdmin.Location = new System.Drawing.Point(6, 71);
			this.CHSharedWorkAreaAdmin.Name = "CHSharedWorkAreaAdmin";
			this.CHSharedWorkAreaAdmin.Size = new System.Drawing.Size(200, 20);
			this.CHSharedWorkAreaAdmin.TabIndex = 47;
			this.CHSharedWorkAreaAdmin.Text = "Shared WorkArea Admin";
			// 
			// CHProViewer
			// 
			this.CHProViewer.Location = new System.Drawing.Point(6, 71);
			this.CHProViewer.Name = "CHProViewer";
			this.CHProViewer.Size = new System.Drawing.Size(200, 20);
			this.CHProViewer.TabIndex = 46;
			this.CHProViewer.Text = "Adept ProViewer";
			// 
			// CHArchiver
			// 
			this.CHArchiver.Location = new System.Drawing.Point(6, 19);
			this.CHArchiver.Name = "CHArchiver";
			this.CHArchiver.Size = new System.Drawing.Size(200, 20);
			this.CHArchiver.TabIndex = 45;
			this.CHArchiver.Text = "Archiver";
			// 
			// CHAdmin
			// 
			this.CHAdmin.Location = new System.Drawing.Point(6, 19);
			this.CHAdmin.Name = "CHAdmin";
			this.CHAdmin.Size = new System.Drawing.Size(200, 20);
			this.CHAdmin.TabIndex = 44;
			this.CHAdmin.Text = "Admin";
			this.CHAdmin.CheckedChanged += new System.EventHandler(this.CHAdmin_CheckedChanged);
			// 
			// EBActiveDirectoryDomain
			// 
			this.EBActiveDirectoryDomain.Location = new System.Drawing.Point(116, 142);
			this.EBActiveDirectoryDomain.Name = "EBActiveDirectoryDomain";
			this.EBActiveDirectoryDomain.Size = new System.Drawing.Size(248, 20);
			this.EBActiveDirectoryDomain.TabIndex = 42;
			// 
			// EBEmailAddress
			// 
			this.EBEmailAddress.Location = new System.Drawing.Point(116, 116);
			this.EBEmailAddress.Name = "EBEmailAddress";
			this.EBEmailAddress.Size = new System.Drawing.Size(248, 20);
			this.EBEmailAddress.TabIndex = 41;
			// 
			// EBPassword
			// 
			this.EBPassword.Location = new System.Drawing.Point(116, 90);
			this.EBPassword.Name = "EBPassword";
			this.EBPassword.Size = new System.Drawing.Size(248, 20);
			this.EBPassword.TabIndex = 40;
			// 
			// EBUserName
			// 
			this.EBUserName.Location = new System.Drawing.Point(116, 64);
			this.EBUserName.Name = "EBUserName";
			this.EBUserName.Size = new System.Drawing.Size(248, 20);
			this.EBUserName.TabIndex = 39;
			// 
			// EBUserId
			// 
			this.EBUserId.Enabled = false;
			this.EBUserId.Location = new System.Drawing.Point(116, 12);
			this.EBUserId.Name = "EBUserId";
			this.EBUserId.Size = new System.Drawing.Size(248, 20);
			this.EBUserId.TabIndex = 38;
			// 
			// EBLoginName
			// 
			this.EBLoginName.Location = new System.Drawing.Point(116, 38);
			this.EBLoginName.Name = "EBLoginName";
			this.EBLoginName.Size = new System.Drawing.Size(248, 20);
			this.EBLoginName.TabIndex = 37;
			// 
			// Label7
			// 
			this.Label7.Location = new System.Drawing.Point(6, 71);
			this.Label7.Name = "Label7";
			this.Label7.Size = new System.Drawing.Size(100, 20);
			this.Label7.TabIndex = 36;
			this.Label7.Text = "Library Card Name";
			// 
			// Label6
			// 
			this.Label6.Location = new System.Drawing.Point(10, 145);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(100, 15);
			this.Label6.TabIndex = 35;
			this.Label6.Text = "Active Directory Domain";
			// 
			// Label5
			// 
			this.Label5.Location = new System.Drawing.Point(10, 119);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(100, 15);
			this.Label5.TabIndex = 34;
			this.Label5.Text = "Email Address";
			// 
			// Label4
			// 
			this.Label4.Location = new System.Drawing.Point(10, 93);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(100, 15);
			this.Label4.TabIndex = 33;
			this.Label4.Text = "Password";
			// 
			// Label3
			// 
			this.Label3.Location = new System.Drawing.Point(10, 67);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(100, 15);
			this.Label3.TabIndex = 32;
			this.Label3.Text = "User Name";
			// 
			// Label2
			// 
			this.Label2.Location = new System.Drawing.Point(10, 15);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(100, 15);
			this.Label2.TabIndex = 31;
			this.Label2.Text = "User Id";
			// 
			// Label1
			// 
			this.Label1.Location = new System.Drawing.Point(10, 41);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(100, 15);
			this.Label1.TabIndex = 30;
			this.Label1.Text = "Login Name";
			// 
			// CHReviewer
			// 
			this.CHReviewer.Location = new System.Drawing.Point(6, 45);
			this.CHReviewer.Name = "CHReviewer";
			this.CHReviewer.Size = new System.Drawing.Size(200, 20);
			this.CHReviewer.TabIndex = 60;
			this.CHReviewer.Text = "Adept Reviewer";
			// 
			// CHLockedLicense
			// 
			this.CHLockedLicense.Location = new System.Drawing.Point(6, 19);
			this.CHLockedLicense.Name = "CHLockedLicense";
			this.CHLockedLicense.Size = new System.Drawing.Size(200, 20);
			this.CHLockedLicense.TabIndex = 61;
			this.CHLockedLicense.Text = "Locked License";
			// 
			// GBAdministrationTypes
			// 
			this.GBAdministrationTypes.Controls.Add(this.CHWorkflowAdmin);
			this.GBAdministrationTypes.Controls.Add(this.CHAdmin);
			this.GBAdministrationTypes.Controls.Add(this.CHSharedWorkAreaAdmin);
			this.GBAdministrationTypes.Controls.Add(this.CHTransmittalAdmin);
			this.GBAdministrationTypes.Location = new System.Drawing.Point(12, 168);
			this.GBAdministrationTypes.Name = "GBAdministrationTypes";
			this.GBAdministrationTypes.Size = new System.Drawing.Size(349, 127);
			this.GBAdministrationTypes.TabIndex = 62;
			this.GBAdministrationTypes.TabStop = false;
			this.GBAdministrationTypes.Text = "Administration Types";
			// 
			// GBOtherPermissions
			// 
			this.GBOtherPermissions.Controls.Add(this.CBLibraryCardName);
			this.GBOtherPermissions.Controls.Add(this.CHChooseLibraryCard);
			this.GBOtherPermissions.Controls.Add(this.Label7);
			this.GBOtherPermissions.Controls.Add(this.CHArchiver);
			this.GBOtherPermissions.Location = new System.Drawing.Point(12, 301);
			this.GBOtherPermissions.Name = "GBOtherPermissions";
			this.GBOtherPermissions.Size = new System.Drawing.Size(349, 100);
			this.GBOtherPermissions.TabIndex = 63;
			this.GBOtherPermissions.TabStop = false;
			this.GBOtherPermissions.Text = "Other Permissions";
			// 
			// CBLibraryCardName
			// 
			this.CBLibraryCardName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBLibraryCardName.FormattingEnabled = true;
			this.CBLibraryCardName.Location = new System.Drawing.Point(113, 71);
			this.CBLibraryCardName.Name = "CBLibraryCardName";
			this.CBLibraryCardName.Size = new System.Drawing.Size(197, 21);
			this.CBLibraryCardName.TabIndex = 50;
			// 
			// GBOther
			// 
			this.GBOther.Controls.Add(this.CHLockedLicense);
			this.GBOther.Controls.Add(this.CHProViewer);
			this.GBOther.Controls.Add(this.CHReviewer);
			this.GBOther.Location = new System.Drawing.Point(12, 407);
			this.GBOther.Name = "GBOther";
			this.GBOther.Size = new System.Drawing.Size(349, 102);
			this.GBOther.TabIndex = 64;
			this.GBOther.TabStop = false;
			this.GBOther.Text = "Other";
			// 
			// BTTriggerChanges
			// 
			this.BTTriggerChanges.Location = new System.Drawing.Point(387, 426);
			this.BTTriggerChanges.Name = "BTTriggerChanges";
			this.BTTriggerChanges.Size = new System.Drawing.Size(96, 23);
			this.BTTriggerChanges.TabIndex = 65;
			this.BTTriggerChanges.Text = "Trigger Changes";
			this.BTTriggerChanges.Click += new System.EventHandler(this.BTTriggerChanges_Click);
			// 
			// SqlUserAdminForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(506, 526);
			this.Controls.Add(this.BTTriggerChanges);
			this.Controls.Add(this.GBOther);
			this.Controls.Add(this.GBOtherPermissions);
			this.Controls.Add(this.GBAdministrationTypes);
			this.Controls.Add(this.BTClose);
			this.Controls.Add(this.BTIsUserEnabled);
			this.Controls.Add(this.BTEnableUser);
			this.Controls.Add(this.BTDisableUser);
			this.Controls.Add(this.BTRemoveUser);
			this.Controls.Add(this.BTModifyUser);
			this.Controls.Add(this.BTGetUserInfo);
			this.Controls.Add(this.BTAddUser);
			this.Controls.Add(this.BTClear);
			this.Controls.Add(this.EBActiveDirectoryDomain);
			this.Controls.Add(this.EBEmailAddress);
			this.Controls.Add(this.EBPassword);
			this.Controls.Add(this.EBUserName);
			this.Controls.Add(this.EBUserId);
			this.Controls.Add(this.EBLoginName);
			this.Controls.Add(this.Label6);
			this.Controls.Add(this.Label5);
			this.Controls.Add(this.Label4);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.Label1);
			this.Name = "SqlUserAdminForm";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SqlUserAdminForm_FormClosing);
			this.Load += new System.EventHandler(this.SqlUserAdminForm_Load);
			this.GBAdministrationTypes.ResumeLayout(false);
			this.GBOtherPermissions.ResumeLayout(false);
			this.GBOther.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.CheckBox CHTransmittalAdmin;
		internal System.Windows.Forms.Button BTClose;
		internal System.Windows.Forms.Button BTIsUserEnabled;
		internal System.Windows.Forms.Button BTEnableUser;
		internal System.Windows.Forms.Button BTDisableUser;
		internal System.Windows.Forms.Button BTRemoveUser;
		internal System.Windows.Forms.Button BTModifyUser;
		internal System.Windows.Forms.Button BTGetUserInfo;
		internal System.Windows.Forms.Button BTAddUser;
		internal System.Windows.Forms.Button BTClear;
		internal System.Windows.Forms.CheckBox CHChooseLibraryCard;
		internal System.Windows.Forms.CheckBox CHWorkflowAdmin;
		internal System.Windows.Forms.CheckBox CHSharedWorkAreaAdmin;
		internal System.Windows.Forms.CheckBox CHProViewer;
		internal System.Windows.Forms.CheckBox CHArchiver;
		internal System.Windows.Forms.CheckBox CHAdmin;
		internal System.Windows.Forms.TextBox EBActiveDirectoryDomain;
		internal System.Windows.Forms.TextBox EBEmailAddress;
		internal System.Windows.Forms.TextBox EBPassword;
		internal System.Windows.Forms.TextBox EBUserName;
		internal System.Windows.Forms.TextBox EBUserId;
		internal System.Windows.Forms.TextBox EBLoginName;
		internal System.Windows.Forms.Label Label7;
		internal System.Windows.Forms.Label Label6;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.CheckBox CHReviewer;
		internal System.Windows.Forms.CheckBox CHLockedLicense;
		private System.Windows.Forms.GroupBox GBAdministrationTypes;
		private System.Windows.Forms.GroupBox GBOtherPermissions;
		private System.Windows.Forms.GroupBox GBOther;
		private System.Windows.Forms.ComboBox CBLibraryCardName;
		internal System.Windows.Forms.Button BTTriggerChanges;
	}
}

