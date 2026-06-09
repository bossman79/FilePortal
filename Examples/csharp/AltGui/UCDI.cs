// UCDI - (User Control) Doc Info

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
	/// Summary description for UCDI.
	/// </summary>
	public class UCDI : System.Windows.Forms.UserControl , IMyResizable
	{
		private NxProject m_Project;
		private int m_STN;
		private string m_FileId;
		private int m_MajRev;
		private int m_MinRev;

		private System.Windows.Forms.TabControl DITabs;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UCDI()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			m_Project = null;
			m_STN = -1;
			m_FileId = "";
			m_MajRev = -1;
			m_MinRev = -1;
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
			this.DITabs = new System.Windows.Forms.TabControl();
			this.SuspendLayout();
			// 
			// DITabs
			// 
			this.DITabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DITabs.Location = new System.Drawing.Point(0, 0);
			this.DITabs.Name = "DITabs";
			this.DITabs.SelectedIndex = 0;
			this.DITabs.Size = new System.Drawing.Size(408, 224);
			this.DITabs.TabIndex = 6;
			this.DITabs.SelectedIndexChanged += new System.EventHandler(this.DITabs_SelectedIndexChanged);
			// 
			// UCDI
			// 
			this.Controls.Add(this.DITabs);
			this.Name = "UCDI";
			this.Size = new System.Drawing.Size(408, 224);
			this.Load += new System.EventHandler(this.UCDI_Load);
			this.ResumeLayout(false);

		}
		#endregion

		public void DoResize(int w, int h)
		{
			this.Width = w;
			this.Height = h;

			//listView1.Left = this.Left;
			//listView1.Top = this.Top;

			//vScrollBar1.Top = this.Top;

			//int fudge = 5;

			//int sw = vScrollBar1.Width;
			//listView1.Width = this.Width - sw - fudge;
			//listView1.Height = this.Height - fudge;
			//vScrollBar1.Left = listView1.Right;
			//vScrollBar1.Height = listView1.Height;

			TabPage tab = DITabs.SelectedTab;
			if(tab != null)
			{
				if(tab.Controls.Count > 0)
				{
					Control ctrl = tab.Controls[0];
					if(ctrl is IMyResizable)
					{
						IMyResizable imr;
						imr = ctrl as IMyResizable;
						imr.DoResize(this.Width, this.Height);
					}
				}
			}
		}
		private void UCDI_Load(object sender, System.EventArgs e)
		{
			AddTab("Relationships");
			//AddTab("Revisions");
            AddTab("Workflow");
            AddTab("Audit Trail");
			AddTab("Library Cards");
			AddTab("Viewing");
			//AddTab("Chat");

			DITabs_SelectedIndexChanged(null, null);

			/*
			AddTab("Where Used");
			AddTab("Children");
			AddTab("Rel. Info");

			AddTab("Revisions");
			AddTab("WIP: Revisions");
			AddTab("Workflow");
			AddTab("Audit Trail");
			
			AddTab("Thumbnail");
			*/
		}
		private void AddTab(string name)
		{
			System.Windows.Forms.TabPage tab = new System.Windows.Forms.TabPage(name);
			DITabs.TabPages.Add(tab);
		}
		
		public void Set(NxProject Project, int STN, string FileId, int MajRev, int MinRev)
		{
			//string Msg = STN.ToString();
			//Msg += ":"; 	Msg += FileId;	
			//Msg += ":";		Msg += MajRev.ToString() ;
			//Msg += ":";		Msg += MinRev.ToString();
			//System.Windows.Forms.MessageBox.Show(Msg);

//			DITree.Nodes.Clear();
//			int TreeStyle = 2; // RT_WIP
//			NxRelTree RelTree = Project.GetRelTree(STN, FileId, MajRev, MinRev, TreeStyle, 0);
//			NxRelNode RootNode = RelTree.RootNode;
//			AddRelNode(null, RootNode);
//			DITree.ExpandAll();
			m_Project = Project;
			m_STN = STN;
			m_FileId = FileId;
			m_MajRev = MajRev;
			m_MinRev = MinRev;

			TabSet();
		}

		private void TabSet()
		{
			TabPage tab = DITabs.SelectedTab;
			if(tab != null && m_Project != null)
			{
				if(tab.Text == "Relationships")
				{
					UCDIRel RelTab = (UCDIRel)tab.Controls[0];
					RelTab.Set(m_Project, m_STN, m_FileId, m_MajRev, m_MinRev);
					DoResize(this.Width, this.Height);
				}
				if(tab.Text == "Library Cards")
				{
				}
				if(tab.Text == "Viewing")
				{
					
				}
				if(tab.Text == "Chat")				
				{
					
				}
			}
		}

		private void DITabs_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			TabPage tab = DITabs.SelectedTab;
			if(tab != null)
			{
				if(tab.Text == "Relationships")
				{
					tab.Controls.Clear();
					tab.Controls.Add(new UCDIRel());
				}
				if(tab.Text == "Library Cards")
				{
				}
				if(tab.Text == "Viewing")
				{
					
				}
				if(tab.Text == "Chat")				
				{
					
				}
				TabSet();
			}
		}
	}
}
