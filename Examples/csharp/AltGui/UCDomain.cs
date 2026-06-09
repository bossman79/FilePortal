using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Interop.AdeptCAC;

namespace ACACTest1
{
	/// <summary>
	/// Summary description for UCDomain.
	/// </summary>
	public class UCDomain : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox EBAccountPassword;
		private System.Windows.Forms.TextBox EBAccountName;
		private System.Windows.Forms.TextBox EBPort;
		private System.Windows.Forms.TextBox EBServerName;
		private System.Windows.Forms.TextBox EBName;
		private System.Windows.Forms.TextBox EBId;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox EBLoginPassword;
		private System.Windows.Forms.TextBox EBLoginName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BTLogin;
		private System.Windows.Forms.TextBox EBLastLoginName;
		private System.Windows.Forms.TextBox EBLastProjectId;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UCDomain()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.EBLastProjectId = new System.Windows.Forms.TextBox();
			this.EBLastLoginName = new System.Windows.Forms.TextBox();
			this.EBAccountPassword = new System.Windows.Forms.TextBox();
			this.EBAccountName = new System.Windows.Forms.TextBox();
			this.EBPort = new System.Windows.Forms.TextBox();
			this.EBServerName = new System.Windows.Forms.TextBox();
			this.EBName = new System.Windows.Forms.TextBox();
			this.EBId = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.EBLoginPassword = new System.Windows.Forms.TextBox();
			this.EBLoginName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.BTLogin = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// EBLastProjectId
			// 
			this.EBLastProjectId.Location = new System.Drawing.Point(128, 232);
			this.EBLastProjectId.Name = "EBLastProjectId";
			this.EBLastProjectId.Size = new System.Drawing.Size(272, 20);
			this.EBLastProjectId.TabIndex = 84;
			this.EBLastProjectId.Text = "";
			// 
			// EBLastLoginName
			// 
			this.EBLastLoginName.Location = new System.Drawing.Point(128, 200);
			this.EBLastLoginName.Name = "EBLastLoginName";
			this.EBLastLoginName.Size = new System.Drawing.Size(272, 20);
			this.EBLastLoginName.TabIndex = 83;
			this.EBLastLoginName.Text = "";
			// 
			// EBAccountPassword
			// 
			this.EBAccountPassword.Location = new System.Drawing.Point(128, 168);
			this.EBAccountPassword.Name = "EBAccountPassword";
			this.EBAccountPassword.Size = new System.Drawing.Size(272, 20);
			this.EBAccountPassword.TabIndex = 82;
			this.EBAccountPassword.Text = "";
			// 
			// EBAccountName
			// 
			this.EBAccountName.Location = new System.Drawing.Point(128, 136);
			this.EBAccountName.Name = "EBAccountName";
			this.EBAccountName.Size = new System.Drawing.Size(272, 20);
			this.EBAccountName.TabIndex = 81;
			this.EBAccountName.Text = "";
			// 
			// EBPort
			// 
			this.EBPort.Location = new System.Drawing.Point(128, 104);
			this.EBPort.Name = "EBPort";
			this.EBPort.Size = new System.Drawing.Size(272, 20);
			this.EBPort.TabIndex = 80;
			this.EBPort.Text = "";
			// 
			// EBServerName
			// 
			this.EBServerName.Location = new System.Drawing.Point(128, 72);
			this.EBServerName.Name = "EBServerName";
			this.EBServerName.Size = new System.Drawing.Size(272, 20);
			this.EBServerName.TabIndex = 79;
			this.EBServerName.Text = "";
			// 
			// EBName
			// 
			this.EBName.Location = new System.Drawing.Point(128, 40);
			this.EBName.Name = "EBName";
			this.EBName.Size = new System.Drawing.Size(272, 20);
			this.EBName.TabIndex = 78;
			this.EBName.Text = "";
			// 
			// EBId
			// 
			this.EBId.Location = new System.Drawing.Point(128, 8);
			this.EBId.Name = "EBId";
			this.EBId.Size = new System.Drawing.Size(272, 20);
			this.EBId.TabIndex = 77;
			this.EBId.Text = "";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 232);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(112, 23);
			this.label11.TabIndex = 76;
			this.label11.Text = "Last Project Id";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 200);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(112, 23);
			this.label10.TabIndex = 75;
			this.label10.Text = "Last Login Name";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 168);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(112, 23);
			this.label9.TabIndex = 74;
			this.label9.Text = "Account Password";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 136);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(112, 23);
			this.label8.TabIndex = 73;
			this.label8.Text = "Account Name";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 104);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(112, 23);
			this.label7.TabIndex = 72;
			this.label7.Text = "Port";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 72);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 23);
			this.label6.TabIndex = 71;
			this.label6.Text = "Server Name";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 40);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 23);
			this.label5.TabIndex = 70;
			this.label5.Text = "Name";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 23);
			this.label4.TabIndex = 69;
			this.label4.Text = "Id";
			// 
			// EBLoginPassword
			// 
			this.EBLoginPassword.Location = new System.Drawing.Point(128, 344);
			this.EBLoginPassword.Name = "EBLoginPassword";
			this.EBLoginPassword.Size = new System.Drawing.Size(272, 20);
			this.EBLoginPassword.TabIndex = 88;
			this.EBLoginPassword.Text = "";
			// 
			// EBLoginName
			// 
			this.EBLoginName.Location = new System.Drawing.Point(128, 312);
			this.EBLoginName.Name = "EBLoginName";
			this.EBLoginName.Size = new System.Drawing.Size(272, 20);
			this.EBLoginName.TabIndex = 87;
			this.EBLoginName.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 344);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 23);
			this.label1.TabIndex = 86;
			this.label1.Text = "Login Password";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 312);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 23);
			this.label2.TabIndex = 85;
			this.label2.Text = "Login Name";
			// 
			// BTLogin
			// 
			this.BTLogin.Location = new System.Drawing.Point(328, 376);
			this.BTLogin.Name = "BTLogin";
			this.BTLogin.TabIndex = 89;
			this.BTLogin.Text = "Login";
			this.BTLogin.Click += new System.EventHandler(this.BTLogin_Click);
			// 
			// UCDomain
			// 
			this.Controls.Add(this.BTLogin);
			this.Controls.Add(this.EBLoginPassword);
			this.Controls.Add(this.EBLoginName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.EBLastProjectId);
			this.Controls.Add(this.EBLastLoginName);
			this.Controls.Add(this.EBAccountPassword);
			this.Controls.Add(this.EBAccountName);
			this.Controls.Add(this.EBPort);
			this.Controls.Add(this.EBServerName);
			this.Controls.Add(this.EBName);
			this.Controls.Add(this.EBId);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Name = "UCDomain";
			this.Size = new System.Drawing.Size(416, 416);
			this.ResumeLayout(false);

		}
		#endregion

		public void ReadObj()
		{
			TreeNode n = (TreeNode)Tag;
			NxDomain Domain = (NxDomain)n.Tag;
			for(int i = 0; i < Controls.Count; i++)
			{
				Control c = Controls[i];
				if(c.Name == "EBId")
					c.Text = Domain.Id;
				if(c.Name == "EBName")
					c.Text = Domain.Name;
				if(c.Name == "EBServerName")
					c.Text = Domain.ServerName;
				if(c.Name == "EBPort")
					c.Text = Domain.Port.ToString();
				if(c.Name == "EBAccountName")
					c.Text = Domain.AccountName;
				if(c.Name == "EBAccountPassword")
					c.Text = Domain.AccountPassword;
				if(c.Name == "EBLastLoginName")
					c.Text = Domain.LastLoginName;
				if(c.Name == "EBLastProjectId")
					c.Text = Domain.LastProjectId;

				if(c.Name == "EBLoginName")
					c.Text = Domain.LastLoginName;
				if(c.Name == "EBLoginPassword")
					c.Text = "";
			}
		}
		private void BTLogin_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			int SystemLocked = 0;
			String SystemLockMsg = "";
			int AlreadyIn = 0;
			int es = 0;
			TreeNode n = (TreeNode)Tag;
			NxDomain Domain = (NxDomain)n.Tag;


            NxLogin Login = (NxLogin)Domain.Login(EBLoginName.Text, EBLoginPassword.Text,
                1, ref(SystemLocked), ref(SystemLockMsg), ref(AlreadyIn), ref(es));

            //NxLogin Login = (NxLogin)Domain.LoginEx(EBLoginName.Text, EBLoginPassword.Text,
            //    1, ref(SystemLocked), ref(SystemLockMsg), ref(AlreadyIn), ref(es), "fr-CA");

			Cursor.Current = Cursors.Default;

			if(Login == null)
			{
				string ErrorString = "";
				Domain.DomainList.Core.GetErrorString(es, ref(ErrorString));
				System.Windows.Forms.MessageBox.Show(ErrorString);
			}
			//if()
//			{
//				TreeNode LoginNode = new TreeNode(Login.Get_UserName());
//				LoginNode.Tag = Login;
//				n.Nodes.Add(LoginNode);
//			}
		}

		private void BTLogout_Click(object sender, System.EventArgs e)
		{
			TreeNode n = (TreeNode)Tag;
			NxDomain Domain = (NxDomain)n.Tag;
			Domain.Logout(EBLoginName.Text);
		}
	}
}
