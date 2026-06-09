using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

//using System.Runtime.InteropServices;
//Marshal.

using Interop.AdeptCAC;
using Synergis.Adept.MainApi;

namespace ACACTest1
{
#region IMyResizable
	public interface IMyResizable
	{
		void DoResize(int PanelWidth, int PanelHeight);
	}
#endregion
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
public class Form1 : System.Windows.Forms.Form
{

#region Main
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
#endregion

#region Construct and Dispose
		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
#endregion

        private IContainer components;

#region Windows Form Designer generated code


        private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.TreeView treeView1;

		private System.Windows.Forms.Splitter MainSplitter;
		private System.Windows.Forms.Panel EastPanel;
		private System.Windows.Forms.Panel SoutheastPanel;
		private System.Windows.Forms.Splitter EastSplitter;
        private MenuItem menuItem2;
        private MenuItem MI_Tools_TestLibCreate;
		private System.Windows.Forms.Panel NortheastPanel;

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.MainSplitter = new System.Windows.Forms.Splitter();
            this.EastPanel = new System.Windows.Forms.Panel();
            this.SoutheastPanel = new System.Windows.Forms.Panel();
            this.EastSplitter = new System.Windows.Forms.Splitter();
            this.NortheastPanel = new System.Windows.Forms.Panel();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.MI_Tools_TestLibCreate = new System.Windows.Forms.MenuItem();
            this.EastPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "File";
            // 
            // toolBar1
            // 
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(824, 42);
            this.toolBar1.TabIndex = 16;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 611);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(824, 22);
            this.statusBar1.TabIndex = 17;
            this.statusBar1.Text = "statusBar1";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 42);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(360, 569);
            this.treeView1.TabIndex = 21;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // MainSplitter
            // 
            this.MainSplitter.BackColor = System.Drawing.SystemColors.ControlText;
            this.MainSplitter.Location = new System.Drawing.Point(360, 42);
            this.MainSplitter.Name = "MainSplitter";
            this.MainSplitter.Size = new System.Drawing.Size(3, 569);
            this.MainSplitter.TabIndex = 22;
            this.MainSplitter.TabStop = false;
            // 
            // EastPanel
            // 
            this.EastPanel.AutoScroll = true;
            this.EastPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.EastPanel.Controls.Add(this.SoutheastPanel);
            this.EastPanel.Controls.Add(this.EastSplitter);
            this.EastPanel.Controls.Add(this.NortheastPanel);
            this.EastPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EastPanel.Location = new System.Drawing.Point(363, 42);
            this.EastPanel.Name = "EastPanel";
            this.EastPanel.Size = new System.Drawing.Size(461, 569);
            this.EastPanel.TabIndex = 23;
            this.EastPanel.Resize += new System.EventHandler(this.EastPanel_Resize);
            // 
            // SoutheastPanel
            // 
            this.SoutheastPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SoutheastPanel.Location = new System.Drawing.Point(0, 211);
            this.SoutheastPanel.Name = "SoutheastPanel";
            this.SoutheastPanel.Size = new System.Drawing.Size(457, 354);
            this.SoutheastPanel.TabIndex = 2;
            this.SoutheastPanel.Resize += new System.EventHandler(this.SoutheastPanel_Resize);
            // 
            // EastSplitter
            // 
            this.EastSplitter.BackColor = System.Drawing.SystemColors.ControlText;
            this.EastSplitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.EastSplitter.Location = new System.Drawing.Point(0, 208);
            this.EastSplitter.MinExtra = 0;
            this.EastSplitter.MinSize = 0;
            this.EastSplitter.Name = "EastSplitter";
            this.EastSplitter.Size = new System.Drawing.Size(457, 3);
            this.EastSplitter.TabIndex = 1;
            this.EastSplitter.TabStop = false;
            // 
            // NortheastPanel
            // 
            this.NortheastPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NortheastPanel.Location = new System.Drawing.Point(0, 0);
            this.NortheastPanel.Name = "NortheastPanel";
            this.NortheastPanel.Size = new System.Drawing.Size(457, 208);
            this.NortheastPanel.TabIndex = 0;
            this.NortheastPanel.Resize += new System.EventHandler(this.NortheastPanel_Resize);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MI_Tools_TestLibCreate});
            this.menuItem2.Text = "Tools";
            // 
            // MI_Tools_TestLibCreate
            // 
            this.MI_Tools_TestLibCreate.Index = 0;
            this.MI_Tools_TestLibCreate.Text = "Test Lib Create";
            this.MI_Tools_TestLibCreate.Click += new System.EventHandler(this.MI_Tools_TestLibCreate_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(824, 633);
            this.Controls.Add(this.EastPanel);
            this.Controls.Add(this.MainSplitter);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.statusBar1);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.EastPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

#region Member Variables
		NxCore m_Core;
		TreeNode m_CoreNode;
		bool m_bSoutheastPanelVisible = true;
		string _ViewContextId;
#endregion

#region Sizing
		// HideSoutheastPanel
		public void HideSE()
		{
			if(m_bSoutheastPanelVisible)
			{
				m_bSoutheastPanelVisible = false;
				NortheastPanel.Height = EastPanel.Height;
			}
		}
		// ShowSoutheastPanel
		public void ShowSE(double factor)
		{
			if(factor <= 0 || factor > 1)
				return;
			if(!m_bSoutheastPanelVisible)
			{
				m_bSoutheastPanelVisible = true;
				//ex. factor = .4
				// NE factor = 1 - factor  // .6 = 1 - .4
				// NE height = East height * NE factor // 600 = 1000 * .6
				double NEfactor = (1 - factor);
				NortheastPanel.Height = (int)(EastPanel.Height * NEfactor);
			}
			else
			{
				NortheastPanel_Resize(null, null);
				SoutheastPanel_Resize(null, null);
			}
		}
		public double GetSEFactor()
		{
			if(!m_bSoutheastPanelVisible)
				return 0;
			// ex. EastPanel Height = 1000, NorthEastPanel = 600
			// Northeast factor = NorthEast height / East height // .6 = 600 / 1000
			// SE factor = 1 - NE factor // .4 = 1 - .6
			double rtn = 1 - (NortheastPanel.Height / EastPanel.Height);
			return rtn;
		}
		private void EastPanel_Resize(object sender, System.EventArgs e)
		{
			if(m_bSoutheastPanelVisible == false)
			{
				NortheastPanel.Height = EastPanel.Height;
			}
		}
		private void NortheastPanel_Resize(object sender, System.EventArgs e)
		{
			Control ctrl;
			IMyResizable imr;
			if(NortheastPanel.Controls.Count > 0)
			{
				ctrl = NortheastPanel.Controls[0];
				if(ctrl is IMyResizable)
				{
					//NortheastPanel.AutoScroll = false;
					imr = ctrl as IMyResizable;
					imr.DoResize(NortheastPanel.Width, NortheastPanel.Height);
				}
				//else
				//	NortheastPanel.AutoScroll = true;
			}
		}
		private void SoutheastPanel_Resize(object sender, System.EventArgs e)
		{
			Control ctrl;
			IMyResizable imr;
			if(SoutheastPanel.Controls.Count > 0)
			{
				ctrl = SoutheastPanel.Controls[0];
				if(ctrl is IMyResizable)
				{
					//SoutheastPanel.AutoScroll = false;
					imr = ctrl as IMyResizable;
					imr.DoResize(SoutheastPanel.Width, SoutheastPanel.Height);
				}
				//else
				//	SoutheastPanel.AutoScroll = true;
			}
		}
#endregion

#region Sub Access
		//		public System.Windows.Forms.Panel GetEastPanel()
		//		{
		//			return EastPanel;
		//		}
		//		public System.Windows.Forms.Splitter GetEastSplitter()
		//		{
		//			return EastSplitter;
		//		}
		//		public System.Windows.Forms.Panel GetNortheastPanel()
		//		{
		//			return NortheastPanel;
		//		}
		//		public System.Windows.Forms.Panel GetSoutheastPanel()
		//		{
		//			return SoutheastPanel;
		//		}
#endregion

#region Form Events
		private void Form1_Load(object sender, System.EventArgs e)
		{
			try
			{
				m_Core = new NxCore();
				int ApiVersion = m_Core.GetApiVersion();
				m_Core.OnInitialize += new INxCoreEvents_OnInitializeEventHandler(NxCore_OnInitialize);
				m_Core.OnUninitialize += new INxCoreEvents_OnUninitializeEventHandler(NxCore_OnUninitialize);
				m_CoreNode = new TreeNode("Core");
				m_CoreNode.Tag = m_Core;
				treeView1.Nodes.Add(m_CoreNode);
				treeView1.SelectedNode = treeView1.Nodes[0];

				UCCore ucCore = (UCCore)NortheastPanel.Controls[0];
				ucCore.DoSwitch();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			m_Core.Uninitialize();
		}

		private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			GC.Collect();
			GC.WaitForPendingFinalizers();

			NortheastPanel.Controls.Clear();
			SoutheastPanel.Controls.Clear();

			TreeNode n = e.Node;
			System.Object o = n.Tag;

			if(o == null)
			{
				SelectSimpleNode(n);
			}
			else if (o is string)
			{
				SelectStringNode(n);
			}
			else
			{
				SelectObjectNode(n);
			}
			
			//DoResize();

			n.Expand(); // Paul.Ligowski 2013-02-22

			Cursor.Current = Cursors.Default;
		}
		#endregion

#region Event Reactors
		private void NxCore_OnInitialize(int bSilent)
		{
			
			NxTblDefList TblDefList = m_Core.TblDefList;
			TreeNode TblDefListNode = new TreeNode("TblDefList");
			TblDefListNode.Tag = TblDefList;
			m_CoreNode.Nodes.Add(TblDefListNode);
			m_CoreNode.Expand();

//			int TblDefCount = TblDefList.GetCount();
//			for(int i = 0; i < TblDefCount; i++)
//			{
//				NxTblDef TblDef = TblDefList.GetItem(i);
//			}
//			foreach(NxTblDef TblDef in TblDefList)
//			{	
//				TreeNode TblDefNode = new TreeNode(TblDef.Name);
//				TblDefNode.Tag = TblDef;
//				TblDefListNode.Nodes.Add(TblDefNode);
//			}


			
			NxDomainList DomainList = m_Core.DomainList;
			TreeNode DomainListNode = new TreeNode("DomainList");
			DomainListNode.Tag = DomainList;
			m_CoreNode.Nodes.Add(DomainListNode);
			m_CoreNode.Expand();

			int DomainCount = DomainList.GetCount();
			//for(int i = 0; i < DomainCount; i++)
			foreach(NxDomain Domain in DomainList)
			{
				//paullfyi Property, indexer, or event 'Item' is not supported by the language; try directly calling accessor method 'Interop.AdeptCAC.INxDomainList.get_Item(int)'
				//DomainList.Item(i) would have been DomainList.get_Item(i)
				//NxDomain Domain = (NxDomain)DomainList.GetItem(i);

				//NxDomainClass DC = (NxDomainClass)Domain;
				//DC.OnLogin += new INxDomainEvents_OnLoginEventHandler(NxDomain_OnLogin);
				
				try
				{
					Domain.OnLogin += new INxDomainEvents_OnLoginEventHandler(NxDomain_OnLogin);
					Domain.OnLogout += new INxDomainEvents_OnLogoutEventHandler(NxDomain_OnLogout);
				}
				catch (Exception e) 
				{
					System.Windows.Forms.MessageBox.Show(e.Message);
				}

				//Domain.OnLogin += new AdeptCAN.CNxDomain.OnLoginEventHandler(this.OnLogin);
				//Domain.OnLogout += new AdeptCAN.CNxDomain.OnLogoutEventHandler(this.OnLogout);

				TreeNode DomainNode = new TreeNode(Domain.Name);
				DomainNode.Tag = Domain;
				DomainListNode.Nodes.Add(DomainNode);
			}
			DomainListNode.Expand();
			if(DomainListNode.Nodes.Count > 0)
				DomainListNode.TreeView.SelectedNode = DomainListNode.Nodes[0];
			else
				DomainListNode.TreeView.SelectedNode = DomainListNode;
		}
		private void NxCore_OnUninitialize()
		{
			m_CoreNode.Nodes.Clear();
		}
		//private void NxDomain_OnLogin(Object DomainObj, Object LoginObj)
		private void NxDomain_OnLogin(NxDomain Domain, NxLogin Login)
		{
		//	//NxDomain Domain = (NxDomain)DomainObj;
		//	NxDomain Domain = DomainObj as NxDomain;
		//	NxLogin Login = (NxLogin)LoginObj;

			string dn = Domain.Name;
			string ln = Login.LoginName;

			Type tp1 = typeof(NxDomain);

			TreeNode DomainNode = FindNode(treeView1, Domain, Domain.Name);
			if(DomainNode != null)
			{
				TreeNode LoginNode = new TreeNode(Login.UserName);
				LoginNode.Tag = Login;
				DomainNode.Nodes.Add(LoginNode);

				NxLogin_OnAfterReset(Login);
			}
		}
		//private void NxDomain_OnLogout(Object DomainObj, Object LoginObj)
		private void NxDomain_OnLogout(NxDomain Domain, NxLogin Login)
		{
		//	NxDomain Domain = (NxDomain)DomainObj;
		//	NxLogin Login = (NxLogin)LoginObj;
			TreeNode DomainNode = FindNode(treeView1, Domain, Domain.Name);
			if(DomainNode != null)
			{
				for(int i = 0; i < DomainNode.Nodes.Count; i++)
				{
					if(DomainNode.Nodes[i].Text == Login.UserName)
						DomainNode.Nodes[i].Remove();
				}
			}
		}

		private void NxLogin_OnBeforeReset(Object LoginObj)
		{
			NxLogin Login = (NxLogin)LoginObj;

			TreeNode LoginNode = FindNode(treeView1, Login, Login.UserName);
			if(LoginNode != null)
			{
				LoginNode.Nodes.Clear();
			}
		}
		private void NxLogin_OnAfterReset(Object LoginObj)
		{
			NxLogin Login = (NxLogin)LoginObj;
			
			TreeNode LoginNode = FindNode(treeView1, Login, Login.UserName);
			if(LoginNode != null)
			{
				NxProject Project = (NxProject)Login.Project;
				TreeNode ProjectNode = new TreeNode(Project.Name);
				ProjectNode.Tag = Project;
				LoginNode.Nodes.Add(ProjectNode);

				TreeNode WorkAreaNode = new TreeNode("Work Area");
				ProjectNode.Nodes.Add(WorkAreaNode);

				TreeNode FileGuideNode = new TreeNode("File Guide");
				ProjectNode.Nodes.Add(FileGuideNode);

				//TreeNode InboxNode = new TreeNode("Inbox");
				
				foreach(NxFileGuideItem FGItem in Project.FileGuideManager.FileGuideList)
				{
                    //if(FGItem.Name == "Inbox")
                    //{
                    //    InboxNode.Tag = FGItem;
                    //}
                    //else
                    //{
						TreeNode FGItemNode = new TreeNode(FGItem.Name);
						FGItemNode.Tag = FGItem;
						FileGuideNode.Nodes.Add(FGItemNode);
					//}
				}
				//ProjectNode.Nodes.Add(InboxNode);
				//InboxNode.EnsureVisible();

				TreeNode SearchNode = new TreeNode("Search");
				ProjectNode.Nodes.Add(SearchNode);

				NxViewContextManager ViewContextManager = Project.ViewContextManager;
				NxViewContextList ViewContextList = ViewContextManager.ViewContextList;
				NxViewContext ViewContext = ViewContextList.Add();
				ViewContext.Name = "ACACTest1";
				ViewContext.TableName = "ACACTBL";
				ViewContext.NumberOfLines = 20;
				ViewContext.SortColumnSchemaId = "SCHEMA_S_LONGNAME";
				ViewContext.bSortAscending = 1;
				_ViewContextId = ViewContext.Id;

				NxContainerItem ci = Project.ContainerManager.ColumnSet_ContainerList.FindName("ACACTest1");
				if (ci == null)
				{
					ci = Project.ContainerManager.ColumnSet_ContainerList.Add("ACACTest1", "");
				}
				if(ci.ContentsList.GetCount() < 1)
				{
					NxContentsItem Col;
					Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LONGNAME"; Col.Width = 50;
					Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_STATUS"; Col.Width = 20;
					Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LIBNAME"; Col.Width = 50;
				}
				ViewContext.ColumnSetId = ci.Id;

				

			}
		}
#endregion

#region Utils
		private TreeNode FindNode(TreeView CurrentTree, System.Object obj, string txt)
		{
			// for each root node
			for(int i = 0; i < CurrentTree.Nodes.Count; i++)
			{
				TreeNode tmp = FindNode(CurrentTree.Nodes[i], obj, txt);
				if(tmp != null)
					return tmp;
			}
			return null;
		}
		private TreeNode FindNode(TreeNode CurrentNode, System.Object obj, string txt)
		{
			for(int i = 0; i < CurrentNode.Nodes.Count; i++)
			{
				if(CurrentNode.Nodes[i].Tag == obj)
				{
					if(CurrentNode.Nodes[i].Text == txt)
						return CurrentNode.Nodes[i];
				}
				TreeNode tmp = FindNode(CurrentNode.Nodes[i], obj, txt);
				if(tmp != null)
					return tmp;
			}
			return null;
		}
		private void AddFGNode(TreeNode ParentTreeNode, NxFileGuideNode FGNode)
		{
			string NewNodeText = FGNode.Value;
			//if(ParentTreeNode.Text == "Inbox" && ParentTreeNode.Tag is NxFileGuideItem)
			//{
			//    NxProject Project = (NxProject)ParentTreeNode.Parent.Tag;
			//    NxTbl UserTbl = Project.FindTbl("fm60usr");			
			//    //String Name;
			//    string Id;
			//    int es = 0;
			//    int cnt = UserTbl.GetCount();
			//    for(int i = 1; i <= cnt; i++)
			//    {
			//        es = UserTbl.GotoRecord(i);
			//        if(es == 0)
			//        {
			//            Id = UserTbl.GetString("S_USERID");
			//            if(Id == NewNodeText)
			//            {
			//                NewNodeText = UserTbl.GetString("S_NAME");
			//                break;
			//            }
			//        }
			//    }
			//}
			NewNodeText += " (" + FGNode.Count.ToString() + ")";

			TreeNode n = new TreeNode(NewNodeText);
			n.Tag = FGNode;
			ParentTreeNode.Nodes.Add(n);


			NxFileGuideNode FGFirstChild = FGNode.FirstChild;
			if (FGFirstChild != null)
			{
				AddFGNode(n, FGFirstChild);
			}
			
			NxFileGuideNode FGNextSibling = FGNode.NextSibling;
			if(FGNextSibling != null)
			{
				AddFGNode(ParentTreeNode, FGNextSibling);
			}
		}


#endregion

#region Select
		private void SelectSimpleNode(TreeNode n)
		{
			if(n.Text == "Work Area")
			{
				n.Nodes.Clear();

				NxProject Project = (NxProject)n.Parent.Tag;
				NxTbl WalTbl = (NxTbl)Project.OpenWorkAreaListTable("acacwal");

				string Name;
				string Path;
				string Comment;
				string Id;
				int es = 0;
				int cnt = WalTbl.GetCount();
				for(int i = 1; i <= cnt; i++)
				{
					es = WalTbl.GotoRecord(i);
					if(es == 0)
					{
						Path = WalTbl.GetString("S_PATH");
						Comment = WalTbl.GetString("S_COMMENT");
						Id = WalTbl.GetString("S_PATHID");
						if(Comment.Length < 1)
							Name = Path;
						else
							Name = Comment;
						TreeNode WANode = new TreeNode(Name);
						WANode.Tag = Id;
						n.Nodes.Add(WANode);
					}
				}
			}
			//if(n.Text == "Inbox")
			//{
                //n.Nodes.Clear();
                //NxProject Project = (NxProject)n.Parent.Tag;
                //NxTbl UserTbl = (NxTbl)Project.FindTbl("fm60usr");
                //String Name;
                //String Id;
                //int es = 0;
                //int cnt = UserTbl.GetCount();
                //for(int i = 1; i <= cnt; i++)
                //{
                //    es = UserTbl.GotoRecord(i);
                //    if(es == 0)
                //    {
                //        Name = UserTbl.GetString("S_NAME");
                //        Id = UserTbl.GetString("S_USERID");

                //        TreeNode UserNode = new TreeNode(Name);
                //        UserNode.Tag = Id;
                //        n.Nodes.Add(UserNode);
                //    }
                //}
			//}
		}
		private void SelectStringNode(TreeNode n)
		{
			TreeNode ParentNode = n.Parent;
			if(ParentNode.Text == "Work Area")
			{
				UCList ucList = new UCList();
				ucList._ViewContextId = _ViewContextId;
				ucList.Tag = n;
				ucList.Location = new Point(0,0);
				ucList.Size = new Size(NortheastPanel.Width, NortheastPanel.Height);
				NortheastPanel.Controls.Add(ucList);
				ucList.ReadObj(null);

				UCDI ucdi = new UCDI();
				ucdi.Tag = ucList;
				ucdi.Location = new Point(0,0);
				ucdi.Size = new Size(SoutheastPanel.Width, SoutheastPanel.Height);
				SoutheastPanel.Controls.Add(ucdi);

				ucList.m_ucdi = ucdi;

				ShowSE(0.5);
			}
            //if(ParentNode.Text == "Inbox")
            //{
            //    UCList ucList = new UCList();
            //    ucList._ViewContextId = _ViewContextId;
            //    ucList.Tag = n;
            //    ucList.Location = new Point(0,0);
            //    ucList.Size = new Size(NortheastPanel.Width, NortheastPanel.Height);
            //    NortheastPanel.Controls.Add(ucList);
            //    ucList.ReadObj(null);

            //    UCDI ucdi = new UCDI();
            //    ucdi.Tag = ucList;
            //    ucdi.Location = new Point(0,0);
            //    ucdi.Size = new Size(SoutheastPanel.Width, SoutheastPanel.Height);
            //    SoutheastPanel.Controls.Add(ucdi);

            //    ucList.m_ucdi = ucdi;

            //    ShowSE(0.5);
            //}
		}
		private void SelectObjectNode(TreeNode n)
		{
			System.Object o = n.Tag;
			if(o is NxCore)
			{	
				UCCore ucCore = new UCCore();
				ucCore.Tag = n;
				ucCore.Location = new Point(0,0);
				//ucCore.Size = new Size(NortheastPanel.Width, NortheastPanel.Height);
				NortheastPanel.Controls.Add(ucCore);
				ucCore.ReadObj();
				HideSE();
				//ucCore.DoSwitch();
			}
			if(o is NxTblDefList)
			{
				HideSE();
			}
			if(o is NxDomainList)
			{
				UCDomainList ucDomainList = new UCDomainList();
				ucDomainList.Tag = n;
				ucDomainList.Location = new Point(0,0);
				//ucDomainList.Size = new Size(NortheastPanel.Width, NortheastPanel.Height);
				NortheastPanel.Controls.Add(ucDomainList);
				ucDomainList.ReadObj();
				HideSE();
			}
			if(o is NxDomain)
			{
				UCDomain ucDomain = new UCDomain();
				ucDomain.Tag = n;
				ucDomain.Location = new Point(0,0);
				//ucDomain.Size = new Size(NortheastPanel.Width, NortheastPanel.Height);
				NortheastPanel.Controls.Add(ucDomain);
				ucDomain.ReadObj();
				HideSE();
			}
			if(o is NxLogin)
			{
				UCLogin ucLogin = new UCLogin();
				ucLogin.Tag = n;
				ucLogin.Location = new Point(0,0);
				//ucLogin.Size = new Size(NortheastPanel.Width, NortheastPanel.Height);
				NortheastPanel.Controls.Add(ucLogin);
				ucLogin.ReadObj();
				HideSE();
			}
			if(o is NxFileGuideItem)
			{
				n.Nodes.Clear();
				NxFileGuideItem FGItem = (NxFileGuideItem)o;

				string FGId = FGItem.Id;

				FGItem.BuildTreeForOnDemand(); // was BuildTree
				NxFileGuideNode FGNode = FGItem.RootNode;
				if (FGNode != null)
				{
					AddFGNode(n, FGNode);
					n.Expand();
				}
			}
			if(o is NxFileGuideNode)
			{
				// TREE
				NxFileGuideNode fgn = (NxFileGuideNode)o;

				int FGNodeType = fgn.FileGuideNodeType;

				if (fgn.bHasChildren == 1 && n.Nodes.Count < 1)
				{
					NxFileGuideNode fgnChild = fgn.FirstChild;
					while (fgnChild != null)
					{
						TreeNode tnChild = new TreeNode(fgnChild.Value);
						tnChild.Tag = fgnChild;
						n.Nodes.Add(tnChild);
						fgnChild = fgnChild.NextSibling;
					}
				}

				// LEAF
				NxFileGuideNode FGNode = (NxFileGuideNode)o;


                //NxTbl LeafTbl = FGNode.BuildLeaf("FGL");
                NxTbl LeafTbl = FGNode.BuildLeafEx(_ViewContextId);


				if(LeafTbl != null)
				{

                    int count = 0;
                    if (LeafTbl.IsServerScroll() == 1)
                        count = LeafTbl.SrvGetCount();
                    else
                        count = LeafTbl.GetCount();

                    if (count > 0)
                    {
                        UCList ucList = new UCList();
                        ucList._ViewContextId = _ViewContextId;
                        ucList.Tag = n;
                        ucList.Location = new Point(0, 0);
                        ucList.Size = new Size(NortheastPanel.Width, NortheastPanel.Height);
                        NortheastPanel.Controls.Add(ucList);
                        ucList.ReadObj(LeafTbl);

                        UCDI ucdi = new UCDI();
                        ucdi.Tag = ucList;
                        ucdi.Location = new Point(0, 0);
                        ucdi.Size = new Size(SoutheastPanel.Width, SoutheastPanel.Height);
                        SoutheastPanel.Controls.Add(ucdi);

                        ucList.m_ucdi = ucdi;

                        ShowSE(0.5);
                    }
				}
			}
		}
#endregion	

        // NxVaultList - FindName is not implemented and NxVault is only the Id, so we must fetch to look for a name.
        string FindVault(NxProject Project, string Name)
        {
            string VaultId = "";
            NxRecord VaultRec = Project.NewRecord();
            VaultRec.Create("VLT");
            NxVault Vault = null;
            NxVaultList VaultList = Project.VLGUManager.VaultList;
            int count = VaultList.GetCount();
            string ThisName = "";
            for (int i = 0; i < count; i++)
            {
                Vault = VaultList.GetItem(i);
                VaultRec.Clear();
                VaultRec.SetStringVal("S_VAULTID", Vault.Id);
                VaultRec.Fetch(0);
                ThisName = VaultRec.GetStringVal("S_NAME");
                if (string.Compare(ThisName, Name, true) == 0)
                {
                    VaultId = Vault.Id;
                    break;
                }

            }
            return VaultId;
        }

        private void MI_Tools_TestLibCreate_Click(object sender, EventArgs e)
        {
            TreeNode n = treeView1.SelectedNode;
            if(n == null)
            {
                MessageBox.Show("Pick project node.");
                return;
            }
            System.Object o = n.Tag;
            if(!(o is NxProject))
            {
                MessageBox.Show("Pick project node.");
                return;
            }

            NxProject Project = (NxProject)o;

            string VaultId = FindVault(Project, "USA");

            NxLibrary LibX1 = Project.VLGUManager.LibraryList.CreateRuntimeLibrary(VaultId, "X1");
            Project.VLGUManager.LibraryList.UpdateRuntimeLibrary(LibX1);

            NxLibrary LibY1 = Project.VLGUManager.LibraryList.CreateRuntimeLibrary(LibX1.Id, "Y1");
            Project.VLGUManager.LibraryList.UpdateRuntimeLibrary(LibY1);

            NxLibrary LibZ1 = Project.VLGUManager.LibraryList.CreateRuntimeLibrary(LibY1.Id, "Z1");
            Project.VLGUManager.LibraryList.UpdateRuntimeLibrary(LibZ1);

            Project.ContainerManager.AddLibIdToMRU(LibX1.Id);

            string LibId = LibX1.Id;


            //Project.Login.BumpTableGroupVersion((int)ApiTypes.TABLE_GROUP_NUMBER.TGN_VL);


            int STN = 0;
            string FileId = "";
            int MajRev = -1;
            int MinRev = -1;
            int bCreatedRecord = 0;

            Project.IsAdeptDocument(@"c:\AdeptWork\Adept10.0\Paul.Ligowski\CLTest.pdf", ref STN, ref FileId, ref MajRev, ref MinRev, ref bCreatedRecord);

            NxSelectionList sl = Project.NewSelectionList();
            sl.AddFromInfo(STN, FileId, MajRev, MinRev);
            Project.CommandsSL.SignIn(sl, 0, LibId, "", null, null);
        }


}
}
