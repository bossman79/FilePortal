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
	/// Summary description for UCDomainList.
	/// </summary>
	public class UCDomainList : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox EBAutoLoginPassword;
		private System.Windows.Forms.TextBox EBAutoLogin;
		private System.Windows.Forms.TextBox EBLastDomainName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UCDomainList()
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
			this.EBAutoLoginPassword = new System.Windows.Forms.TextBox();
			this.EBAutoLogin = new System.Windows.Forms.TextBox();
			this.EBLastDomainName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// EBAutoLoginPassword
			// 
			this.EBAutoLoginPassword.Location = new System.Drawing.Point(128, 72);
			this.EBAutoLoginPassword.Name = "EBAutoLoginPassword";
			this.EBAutoLoginPassword.Size = new System.Drawing.Size(272, 20);
			this.EBAutoLoginPassword.TabIndex = 85;
			this.EBAutoLoginPassword.Text = "";
			// 
			// EBAutoLogin
			// 
			this.EBAutoLogin.Location = new System.Drawing.Point(128, 40);
			this.EBAutoLogin.Name = "EBAutoLogin";
			this.EBAutoLogin.Size = new System.Drawing.Size(272, 20);
			this.EBAutoLogin.TabIndex = 84;
			this.EBAutoLogin.Text = "";
			// 
			// EBLastDomainName
			// 
			this.EBLastDomainName.Location = new System.Drawing.Point(128, 8);
			this.EBLastDomainName.Name = "EBLastDomainName";
			this.EBLastDomainName.Size = new System.Drawing.Size(272, 20);
			this.EBLastDomainName.TabIndex = 83;
			this.EBLastDomainName.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 72);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 23);
			this.label6.TabIndex = 82;
			this.label6.Text = "Auto Login Password";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 40);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 23);
			this.label5.TabIndex = 81;
			this.label5.Text = "Auto Login";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 23);
			this.label4.TabIndex = 80;
			this.label4.Text = "Last Domain Name";
			// 
			// UCDomainList
			// 
			this.Controls.Add(this.EBAutoLoginPassword);
			this.Controls.Add(this.EBAutoLogin);
			this.Controls.Add(this.EBLastDomainName);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Name = "UCDomainList";
			this.Size = new System.Drawing.Size(416, 112);
			this.ResumeLayout(false);

		}
		#endregion

		public void ReadObj()
		{
			TreeNode n = (TreeNode)Tag;
			NxDomainList DomainList = (NxDomainList)n.Tag;
			for(int i = 0; i < Controls.Count; i++)
			{
				Control c = Controls[i];
				if(c.Name == "EBLastDomainName")
					c.Text = DomainList.LastDomainName;
				if(c.Name == "EBAutoLogin")
					c.Text = DomainList.AutoLogin.ToString();
				if(c.Name == "EBAutoLoginPassword")
					c.Text = DomainList.AutoLoginPassword;
			}
		}
	}
}
