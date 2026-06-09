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
	/// Summary description for UCCore.
	/// </summary>
	public class UCCore : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button BTSwitch;
		private System.Windows.Forms.TextBox EBWorkHomeDirectory;
		private System.Windows.Forms.TextBox EBTmpDirectory;
		private System.Windows.Forms.TextBox EBSupportDirectory;
		private System.Windows.Forms.TextBox EBWorkstationIniPNE;
		private System.Windows.Forms.TextBox EBWindowsDirectory;
		private System.Windows.Forms.TextBox EBComputerName;
		private System.Windows.Forms.TextBox EBPathToCore;
		private System.Windows.Forms.TextBox EBPathToExe;
		private System.Windows.Forms.TextBox EBIsInitialized;
		private System.Windows.Forms.TextBox EBApiVersion;
		private System.Windows.Forms.TextBox EBAppVersion;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox EBErrorCode;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox EBErrorString;
		private System.Windows.Forms.Button BTGetErrorString;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox EBRtnErrorCode;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UCCore()
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
			this.BTSwitch = new System.Windows.Forms.Button();
			this.EBWorkHomeDirectory = new System.Windows.Forms.TextBox();
			this.EBTmpDirectory = new System.Windows.Forms.TextBox();
			this.EBSupportDirectory = new System.Windows.Forms.TextBox();
			this.EBWorkstationIniPNE = new System.Windows.Forms.TextBox();
			this.EBWindowsDirectory = new System.Windows.Forms.TextBox();
			this.EBComputerName = new System.Windows.Forms.TextBox();
			this.EBPathToCore = new System.Windows.Forms.TextBox();
			this.EBPathToExe = new System.Windows.Forms.TextBox();
			this.EBIsInitialized = new System.Windows.Forms.TextBox();
			this.EBApiVersion = new System.Windows.Forms.TextBox();
			this.EBAppVersion = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.EBErrorCode = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.EBErrorString = new System.Windows.Forms.TextBox();
			this.BTGetErrorString = new System.Windows.Forms.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.EBRtnErrorCode = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// BTSwitch
			// 
			this.BTSwitch.Location = new System.Drawing.Point(152, 104);
			this.BTSwitch.Name = "BTSwitch";
			this.BTSwitch.Size = new System.Drawing.Size(80, 24);
			this.BTSwitch.TabIndex = 69;
			this.BTSwitch.Text = "Switch";
			this.BTSwitch.Click += new System.EventHandler(this.BTSwitch_Click);
			// 
			// EBWorkHomeDirectory
			// 
			this.EBWorkHomeDirectory.Location = new System.Drawing.Point(128, 384);
			this.EBWorkHomeDirectory.Name = "EBWorkHomeDirectory";
			this.EBWorkHomeDirectory.Size = new System.Drawing.Size(272, 20);
			this.EBWorkHomeDirectory.TabIndex = 68;
			this.EBWorkHomeDirectory.Text = "";
			// 
			// EBTmpDirectory
			// 
			this.EBTmpDirectory.Location = new System.Drawing.Point(128, 352);
			this.EBTmpDirectory.Name = "EBTmpDirectory";
			this.EBTmpDirectory.Size = new System.Drawing.Size(272, 20);
			this.EBTmpDirectory.TabIndex = 67;
			this.EBTmpDirectory.Text = "";
			// 
			// EBSupportDirectory
			// 
			this.EBSupportDirectory.Location = new System.Drawing.Point(128, 320);
			this.EBSupportDirectory.Name = "EBSupportDirectory";
			this.EBSupportDirectory.Size = new System.Drawing.Size(272, 20);
			this.EBSupportDirectory.TabIndex = 66;
			this.EBSupportDirectory.Text = "";
			// 
			// EBWorkstationIniPNE
			// 
			this.EBWorkstationIniPNE.Location = new System.Drawing.Point(128, 288);
			this.EBWorkstationIniPNE.Name = "EBWorkstationIniPNE";
			this.EBWorkstationIniPNE.Size = new System.Drawing.Size(272, 20);
			this.EBWorkstationIniPNE.TabIndex = 65;
			this.EBWorkstationIniPNE.Text = "";
			// 
			// EBWindowsDirectory
			// 
			this.EBWindowsDirectory.Location = new System.Drawing.Point(128, 256);
			this.EBWindowsDirectory.Name = "EBWindowsDirectory";
			this.EBWindowsDirectory.Size = new System.Drawing.Size(272, 20);
			this.EBWindowsDirectory.TabIndex = 64;
			this.EBWindowsDirectory.Text = "";
			// 
			// EBComputerName
			// 
			this.EBComputerName.Location = new System.Drawing.Point(128, 224);
			this.EBComputerName.Name = "EBComputerName";
			this.EBComputerName.Size = new System.Drawing.Size(272, 20);
			this.EBComputerName.TabIndex = 63;
			this.EBComputerName.Text = "";
			// 
			// EBPathToCore
			// 
			this.EBPathToCore.Location = new System.Drawing.Point(128, 192);
			this.EBPathToCore.Name = "EBPathToCore";
			this.EBPathToCore.Size = new System.Drawing.Size(272, 20);
			this.EBPathToCore.TabIndex = 62;
			this.EBPathToCore.Text = "";
			// 
			// EBPathToExe
			// 
			this.EBPathToExe.Location = new System.Drawing.Point(128, 160);
			this.EBPathToExe.Name = "EBPathToExe";
			this.EBPathToExe.Size = new System.Drawing.Size(272, 20);
			this.EBPathToExe.TabIndex = 61;
			this.EBPathToExe.Text = "";
			// 
			// EBIsInitialized
			// 
			this.EBIsInitialized.Location = new System.Drawing.Point(128, 72);
			this.EBIsInitialized.Name = "EBIsInitialized";
			this.EBIsInitialized.Size = new System.Drawing.Size(272, 20);
			this.EBIsInitialized.TabIndex = 60;
			this.EBIsInitialized.Text = "";
			// 
			// EBApiVersion
			// 
			this.EBApiVersion.Location = new System.Drawing.Point(128, 40);
			this.EBApiVersion.Name = "EBApiVersion";
			this.EBApiVersion.Size = new System.Drawing.Size(272, 20);
			this.EBApiVersion.TabIndex = 59;
			this.EBApiVersion.Text = "";
			// 
			// EBAppVersion
			// 
			this.EBAppVersion.Location = new System.Drawing.Point(128, 8);
			this.EBAppVersion.Name = "EBAppVersion";
			this.EBAppVersion.Size = new System.Drawing.Size(272, 20);
			this.EBAppVersion.TabIndex = 58;
			this.EBAppVersion.Text = "";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 384);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(112, 23);
			this.label11.TabIndex = 57;
			this.label11.Text = "Work Home Directory";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 352);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(112, 23);
			this.label10.TabIndex = 56;
			this.label10.Text = "Tmp Directory";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 320);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(112, 23);
			this.label9.TabIndex = 55;
			this.label9.Text = "Support Directory";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 288);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(112, 23);
			this.label8.TabIndex = 54;
			this.label8.Text = "Workstation Ini PNE";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 256);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(112, 23);
			this.label7.TabIndex = 53;
			this.label7.Text = "Windows Directory";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 224);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 23);
			this.label6.TabIndex = 52;
			this.label6.Text = "Computer Name";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 192);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 23);
			this.label5.TabIndex = 51;
			this.label5.Text = "Path To Core";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 160);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 23);
			this.label4.TabIndex = 50;
			this.label4.Text = "Path To Exe";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 23);
			this.label3.TabIndex = 49;
			this.label3.Text = "Is Initialized";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 23);
			this.label2.TabIndex = 48;
			this.label2.Text = "Api Version";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 23);
			this.label1.TabIndex = 47;
			this.label1.Text = "App Version";
			// 
			// EBErrorCode
			// 
			this.EBErrorCode.Location = new System.Drawing.Point(128, 424);
			this.EBErrorCode.Name = "EBErrorCode";
			this.EBErrorCode.Size = new System.Drawing.Size(64, 20);
			this.EBErrorCode.TabIndex = 71;
			this.EBErrorCode.Text = "5028";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 424);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(112, 23);
			this.label12.TabIndex = 70;
			this.label12.Text = "Error Code";
			// 
			// EBErrorString
			// 
			this.EBErrorString.Location = new System.Drawing.Point(128, 480);
			this.EBErrorString.Name = "EBErrorString";
			this.EBErrorString.Size = new System.Drawing.Size(272, 20);
			this.EBErrorString.TabIndex = 72;
			this.EBErrorString.Text = "";
			// 
			// BTGetErrorString
			// 
			this.BTGetErrorString.Location = new System.Drawing.Point(200, 424);
			this.BTGetErrorString.Name = "BTGetErrorString";
			this.BTGetErrorString.Size = new System.Drawing.Size(128, 23);
			this.BTGetErrorString.TabIndex = 73;
			this.BTGetErrorString.Text = "Get Error String";
			this.BTGetErrorString.Click += new System.EventHandler(this.BTGetErrorString_Click);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 464);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(112, 32);
			this.label13.TabIndex = 74;
			this.label13.Text = "Return Error Code/String";
			// 
			// EBRtnErrorCode
			// 
			this.EBRtnErrorCode.Location = new System.Drawing.Point(128, 456);
			this.EBRtnErrorCode.Name = "EBRtnErrorCode";
			this.EBRtnErrorCode.Size = new System.Drawing.Size(64, 20);
			this.EBRtnErrorCode.TabIndex = 75;
			this.EBRtnErrorCode.Text = "";
			// 
			// UCCore
			// 
			this.Controls.Add(this.EBRtnErrorCode);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.BTGetErrorString);
			this.Controls.Add(this.EBErrorString);
			this.Controls.Add(this.EBErrorCode);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.BTSwitch);
			this.Controls.Add(this.EBWorkHomeDirectory);
			this.Controls.Add(this.EBTmpDirectory);
			this.Controls.Add(this.EBSupportDirectory);
			this.Controls.Add(this.EBWorkstationIniPNE);
			this.Controls.Add(this.EBWindowsDirectory);
			this.Controls.Add(this.EBComputerName);
			this.Controls.Add(this.EBPathToCore);
			this.Controls.Add(this.EBPathToExe);
			this.Controls.Add(this.EBIsInitialized);
			this.Controls.Add(this.EBApiVersion);
			this.Controls.Add(this.EBAppVersion);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "UCCore";
			this.Size = new System.Drawing.Size(416, 512);
			this.ResumeLayout(false);

		}
		#endregion

		public void ReadObj()
		{
			TreeNode n = (TreeNode)Tag;
			NxCore Core = (NxCore)n.Tag;
			for(int i = 0; i < Controls.Count; i++)
			{
				Control c = Controls[i];
				if(c.Name == "EBAppVersion")
					c.Text = Core.GetAppVersion().ToString();
				if(c.Name == "EBApiVersion")
					c.Text = Core.GetApiVersion().ToString();
				if(c.Name == "EBIsInitialized")
					c.Text = Core.IsInitialized().ToString();
				if(c.Name == "EBPathToExe")
					c.Text = Core.PathToExe;
				if(c.Name == "EBPathToCore")
					c.Text = Core.PathToCore;
				if(c.Name == "EBComputerName")
					c.Text = Core.ComputerName;
				if(c.Name == "EBWindowsDirectory")
					c.Text = Core.WindowsDirectory;
				if(c.Name == "EBWorkstationIniPNE")
					c.Text = Core.WorkstationIniPNE;
				if(c.Name == "EBSupportDirectory")
					c.Text = Core.SupportDirectory;
				if(c.Name == "EBTmpDirectory")
					c.Text = Core.TmpDirectory;
				if(c.Name == "EBWorkHomeDirectory")
					c.Text = Core.WorkHomeDirectory;
			}
		}

		public void DoSwitch()
		{
			BTSwitch_Click(null, null);
		}

		private void BTSwitch_Click(object sender, System.EventArgs e)
		{
			TreeNode n = (TreeNode)Tag;
			NxCore Core = (NxCore)n.Tag;
			if(Core.IsInitialized() == 1)
				Core.Uninitialize();
			else
			{
				Core.Initialize(0);
				//String str = Core.GetDomainList().GetItem(0).Name;
				//System.Windows.Forms.MessageBox.Show(str);
			}
			ReadObj();
		}

		private void BTGetErrorString_Click(object sender, System.EventArgs e)
		{
			int es = int.Parse(EBErrorCode.Text);
			String eString = "";
			TreeNode n = (TreeNode)Tag;
			NxCore Core = (NxCore)n.Tag;
			int res = Core.GetErrorString(es, ref(eString));

			EBRtnErrorCode.Text = res.ToString();
			EBErrorString.Text = eString;
		}

	}
}
