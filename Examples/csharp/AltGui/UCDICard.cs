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
	/// Summary description for UCDICard.
	/// </summary>
	public class UCDICard : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TabControl DICardTabs;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UCDICard()
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
			this.DICardTabs = new System.Windows.Forms.TabControl();
			this.SuspendLayout();
			// 
			// DICardTabs
			// 
			this.DICardTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DICardTabs.Location = new System.Drawing.Point(0, 0);
			this.DICardTabs.Name = "DICardTabs";
			this.DICardTabs.SelectedIndex = 0;
			this.DICardTabs.Size = new System.Drawing.Size(150, 150);
			this.DICardTabs.TabIndex = 7;
			// 
			// UCDICard
			// 
			this.Controls.Add(this.DICardTabs);
			this.Name = "UCDICard";
			this.Load += new System.EventHandler(this.UCDICard_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void AddTab(string name)
		{
			System.Windows.Forms.TabPage tab = new System.Windows.Forms.TabPage(name);
			DICardTabs.TabPages.Add(tab);
		}
		private void UCDICard_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
