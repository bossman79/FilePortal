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
	/// Summary description for UCLogin.
	/// </summary>
	public class UCLogin : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox EBDomainName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox EBUserName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox EBLoginName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BTLogout;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UCLogin()
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
			this.EBDomainName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.EBUserName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.EBLoginName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BTLogout = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// EBDomainName
			// 
			this.EBDomainName.Location = new System.Drawing.Point(128, 8);
			this.EBDomainName.Name = "EBDomainName";
			this.EBDomainName.Size = new System.Drawing.Size(272, 20);
			this.EBDomainName.TabIndex = 80;
			this.EBDomainName.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 23);
			this.label5.TabIndex = 79;
			this.label5.Text = "Domain Name";
			// 
			// EBUserName
			// 
			this.EBUserName.Location = new System.Drawing.Point(128, 72);
			this.EBUserName.Name = "EBUserName";
			this.EBUserName.Size = new System.Drawing.Size(272, 20);
			this.EBUserName.TabIndex = 82;
			this.EBUserName.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 23);
			this.label1.TabIndex = 81;
			this.label1.Text = "User Name";
			// 
			// EBLoginName
			// 
			this.EBLoginName.Location = new System.Drawing.Point(128, 40);
			this.EBLoginName.Name = "EBLoginName";
			this.EBLoginName.Size = new System.Drawing.Size(272, 20);
			this.EBLoginName.TabIndex = 84;
			this.EBLoginName.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 23);
			this.label2.TabIndex = 83;
			this.label2.Text = "Login Name";
			// 
			// BTLogout
			// 
			this.BTLogout.Location = new System.Drawing.Point(320, 104);
			this.BTLogout.Name = "BTLogout";
			this.BTLogout.TabIndex = 91;
			this.BTLogout.Text = "Logout";
			this.BTLogout.Click += new System.EventHandler(this.BTLogout_Click);
			// 
			// UCLogin
			// 
			this.Controls.Add(this.BTLogout);
			this.Controls.Add(this.EBLoginName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.EBUserName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.EBDomainName);
			this.Controls.Add(this.label5);
			this.Name = "UCLogin";
			this.Size = new System.Drawing.Size(424, 144);
			this.ResumeLayout(false);

		}
		#endregion

		public void ReadObj()
		{
			TreeNode n = (TreeNode)Tag;
			NxLogin Login = (NxLogin)n.Tag;
			for(int i = 0; i < Controls.Count; i++)
			{
				Control c = Controls[i];
				if(c.Name == "EBDomainName")
					c.Text = Login.DomainName;
				if(c.Name == "EBLoginName")
					c.Text = Login.LoginName;
				if(c.Name == "EBUserName")
					c.Text = Login.UserName;
			}
		}

		private void BTLogout_Click(object sender, System.EventArgs e)
		{
			//TreeNode n = (TreeNode)Tag;
			//NxLogin Login = (NxLogin)n.Tag;
			//NxLoginList LoginList = (NxLoginList)Login.GetLoginList();
			//NxDomain Domain = (NxDomain)LoginList.GetDomain();
			//Domain.Logout(Login.LoginName);
			NxDomain Domain = null;
			string LoginName = "";
			if(true)
			{
				TreeNode LoginNode = (TreeNode)Tag;
				NxLogin Login = (NxLogin)LoginNode.Tag;
				TreeNode DomainNode = LoginNode.Parent;
				Domain = (NxDomain)DomainNode.Tag;
				LoginName = Login.LoginName;
			}
			if(Domain != null && LoginName.Length > 0)
				Domain.Logout(LoginName);
		}

	}
}
