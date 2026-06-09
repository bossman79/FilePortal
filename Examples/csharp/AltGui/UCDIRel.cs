using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Interop.AdeptCAC;
using Synergis.Adept.MainApi;

namespace ACACTest1
{
	/// <summary>
	/// Summary description for UCDIRel.
	/// </summary>
	public class UCDIRel : System.Windows.Forms.UserControl , IMyResizable
	{
		private System.Windows.Forms.TabControl DIRelTabs;
		private System.Windows.Forms.Splitter DIRelSplit;
		private System.Windows.Forms.TreeView DIRelTree;
		private ContextMenuStrip CMIRelTree;
		private ToolStripMenuItem TSMIVirtualize;
		private ToolStripMenuItem TSMIVirtualizePoll;
		private IContainer components;

		NxProject _Project;
		NxRelTree _RelTree;

		public UCDIRel()
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
            this.components = new System.ComponentModel.Container();
            this.DIRelTabs = new System.Windows.Forms.TabControl();
            this.DIRelSplit = new System.Windows.Forms.Splitter();
            this.DIRelTree = new System.Windows.Forms.TreeView();
            this.CMIRelTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TSMIVirtualize = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIVirtualizePoll = new System.Windows.Forms.ToolStripMenuItem();
            this.CMIRelTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // DIRelTabs
            // 
            this.DIRelTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DIRelTabs.Location = new System.Drawing.Point(124, 0);
            this.DIRelTabs.Name = "DIRelTabs";
            this.DIRelTabs.SelectedIndex = 0;
            this.DIRelTabs.Size = new System.Drawing.Size(220, 150);
            this.DIRelTabs.TabIndex = 5;
            this.DIRelTabs.SelectedIndexChanged += new System.EventHandler(this.DIRelTabs_SelectedIndexChanged);
            // 
            // DIRelSplit
            // 
            this.DIRelSplit.BackColor = System.Drawing.SystemColors.ControlText;
            this.DIRelSplit.Location = new System.Drawing.Point(121, 0);
            this.DIRelSplit.Name = "DIRelSplit";
            this.DIRelSplit.Size = new System.Drawing.Size(3, 150);
            this.DIRelSplit.TabIndex = 4;
            this.DIRelSplit.TabStop = false;
            // 
            // DIRelTree
            // 
            this.DIRelTree.ContextMenuStrip = this.CMIRelTree;
            this.DIRelTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.DIRelTree.Location = new System.Drawing.Point(0, 0);
            this.DIRelTree.Name = "DIRelTree";
            this.DIRelTree.Size = new System.Drawing.Size(121, 150);
            this.DIRelTree.TabIndex = 3;
            this.DIRelTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DIRelTree_AfterSelect);
            // 
            // CMIRelTree
            // 
            this.CMIRelTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIVirtualize,
            this.TSMIVirtualizePoll});
            this.CMIRelTree.Name = "CMIRelTree";
            this.CMIRelTree.Size = new System.Drawing.Size(146, 48);
            // 
            // TSMIVirtualize
            // 
            this.TSMIVirtualize.Name = "TSMIVirtualize";
            this.TSMIVirtualize.Size = new System.Drawing.Size(145, 22);
            this.TSMIVirtualize.Text = "Virtualize";
            this.TSMIVirtualize.Click += new System.EventHandler(this.TSMIVirtualize_Click);
            // 
            // TSMIVirtualizePoll
            // 
            this.TSMIVirtualizePoll.Name = "TSMIVirtualizePoll";
            this.TSMIVirtualizePoll.Size = new System.Drawing.Size(145, 22);
            this.TSMIVirtualizePoll.Text = "Virtualize Poll";
            this.TSMIVirtualizePoll.Click += new System.EventHandler(this.TSMIVirtualizePoll_Click);
            // 
            // UCDIRel
            // 
            this.Controls.Add(this.DIRelTabs);
            this.Controls.Add(this.DIRelSplit);
            this.Controls.Add(this.DIRelTree);
            this.Name = "UCDIRel";
            this.Size = new System.Drawing.Size(344, 150);
            this.Load += new System.EventHandler(this.UCDIRel_Load);
            this.CMIRelTree.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void AddTab(string name)
		{
			System.Windows.Forms.TabPage tab = new System.Windows.Forms.TabPage(name);
			DIRelTabs.TabPages.Add(tab);
		}
		public void DoResize(int w, int h)
		{
			this.Width = w;
			this.Height = h;

            // This control has 0 or 1 control on each tab
            TabPage tab = DIRelTabs.SelectedTab;
            if (tab.Controls.Count > 0)
            {
                //tab.Controls[0].Width = tab.Width;
                //tab.Controls[0].Height = tab.Height;

                UCList uclist = (UCList)tab.Controls[0];
                uclist.DoResize(tab.ClientSize.Width - 10, tab.ClientSize.Height - 30);

            }
		}
		private void UCDIRel_Load(object sender, System.EventArgs e)
		{
			AddTab("Where Used");
			AddTab("Children");
            AddTab("Revisions");
            AddTab("WIP: Revisions");
			AddTab("Rel. Info");
		}
		private void AddRelNode(TreeNode ParentTreeNode, NxRelNode RelNode)
		{
			string NewNodeText = "Missing: " + RelNode.Name;
			if(RelNode.ChildRelFile != null)
				NewNodeText = RelNode.ChildRelFile.FileNE;
			string RelType = RelNode.RelType;
			NewNodeText += " (";
			if(RelType == "DW")
				NewNodeText += "E";
			if(RelType == "GP")
				NewNodeText += "M";
			if(RelType == "PR")
				NewNodeText += "P";
			if(RelType == "AP")
				NewNodeText += "A";
			NewNodeText += ")";

			//int ChildSourceTableNumber = RelNode.ChildSourceTableNumber;
			//string ChildFileId = RelNode.ChildFileId;
			
			TreeNode n = new TreeNode(NewNodeText);
			n.Tag = RelNode;
			if(ParentTreeNode == null)
				DIRelTree.Nodes.Add(n);
			else
				ParentTreeNode.Nodes.Add(n);

			NxRelNode RelFirstChild = RelNode.FirstChild;
			if(RelFirstChild != null)
			{
				AddRelNode(n, RelFirstChild);
			}

			// paull 20110906 loop siblings instead of recursing
			//NxRelNode RelNextSibling = RelNode.NextSibling;
			//if(RelNextSibling != null)
			//	AddRelNode(ParentTreeNode, RelNextSibling);

			// if I have a prev sib, then he is responsible
			if(RelNode.PrevSibling != null)
				return;
			// I am the first sib, so walk all my siblings
			NxRelNode NextSibling = RelNode.NextSibling;
			while(NextSibling != null)
			{
				AddRelNode(ParentTreeNode, NextSibling);
				NextSibling = NextSibling.NextSibling;
			}
			
		}
		public void Set(NxProject Project, int STN, string FileId, int MajRev, int MinRev)
		{
			_Project = Project;

			DIRelTree.Nodes.Clear();
			int TreeStyle = 2; // RT_WIP
			_RelTree = Project.GetRelTree(STN, FileId, MajRev, MinRev, TreeStyle, 0);
			NxRelNode RootNode = _RelTree.RootNode;
			AddRelNode(null, RootNode);
			DIRelTree.ExpandAll();
            if (DIRelTree.Nodes.Count > 0)
                DIRelTree.SelectedNode = DIRelTree.Nodes[0];
		}

		private void TSMIVirtualize_Click(object sender, EventArgs e)
		{
			_RelTree.VirtualizeEx(_Project, 1, 0, 1);
		}

		private void TSMIVirtualizePoll_Click(object sender, EventArgs e)
		{
			_Project.Login.Core.CoreInterface.ClearDeferredErrors();

			int bContinue = 1;
			string CurrentFileNE = "";
			int Count = _RelTree.VirtualizePollStartEx(_Project, 1, 0, 1);
			if(Count > 0)
			{
				int progress = 0;
				ProgressBar pb = new ProgressBar();
				pb.Value = 0;
				pb.Maximum = Count;
				pb.Show();
				pb.Visible = true;
				while(progress >= 0)
				{
					//System.Threading.Thread.Sleep(250);
					progress = _RelTree.VirtualizePollQueryEx(bContinue, ref CurrentFileNE);
					if(progress >= 0)
					{
						pb.Value = progress;
						pb.Update();
					}
					System.Windows.Forms.Application.DoEvents();
				}
			}

			_Project.Login.Core.CoreInterface.FlushDeferredErrors("Virtualize");

		}

        private void DIRelTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DIRelTabs_SelectedIndexChanged(null, null);
        }

        private void DIRelTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tab = DIRelTabs.SelectedTab;
            if (tab != null)
            {
                tab.Controls.Clear();

                if (tab.Text == "Where Used")
                {
                    NxContainerItem ci = _Project.ContainerManager.ColumnSet_ContainerList.FindName("AltGui_UCDIRel_WhereUsed");
                    if (ci == null)
                    {
                        ci = _Project.ContainerManager.ColumnSet_ContainerList.Add("AltGui_UCDIRel_WhereUsed", "");
                    }
                    if (ci.ContentsList.GetCount() < 1)
                    {
                        NxContentsItem Col;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LONGNAME"; Col.Width = 50;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_STATUS"; Col.Width = 20;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_VERSION"; Col.Width = 20;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LIBNAME"; Col.Width = 50;
                    }

                    NxViewContextManager ViewContextManager = _Project.ViewContextManager;
                    NxViewContextList ViewContextList = ViewContextManager.ViewContextList;
                    NxViewContext ViewContext = ViewContextList.FindName("AltGui_UCDIRel_WhereUsed");
                    if (ViewContext == null)
                    {
                        ViewContext = ViewContextList.Add();
                        ViewContext.Name = "AltGui_UCDIRel_WhereUsed";
                        ViewContext.TableName = "WhereUsed";
                        ViewContext.NumberOfLines = 20;
                        ViewContext.SortColumnSchemaId = "SCHEMA_S_LONGNAME";
                        ViewContext.bSortAscending = 1;
                        ViewContext.ColumnSetId = ci.Id;
                    }

                    UCList ucList = new UCList();
                    ucList._Project = _Project;
                    ucList._ViewContextId = ViewContext.Id;
                    //ucList.Tag = n;
                    ucList.Location = new Point(0, 0);
                    //ucList.Size = new Size(tab.Width, tab.Height);
                    tab.Controls.Add(ucList);

                    DoResize(this.Width, this.Height);

                    TreeNode tn = DIRelTree.SelectedNode;
                    if (tn != null)
                    {
                        NxRelNode relnode = (NxRelNode)tn.Tag;
                        
                        if(relnode.ChildRelFile != null)
                        {
                            //NxTbl WhereUsedTbl = _Project.GetRBWhereUsed(relnode.ChildSourceTableNumber, relnode.ChildFileId, relnode.ChildMajRev, relnode.ChildMinRev, "RBWhere", (int)ApiTypes.ADEPTT_RTTREE_STYLE.RT_WIP);
                            //if (WhereUsedTbl != null && WhereUsedTbl.GetCount() > 0)
                            //{
                            //    ucList.ReadObj(WhereUsedTbl);
                            //}
                            NxTbl WhereUsedTbl = _Project.VcGetRBWhereUsed(relnode.ChildSourceTableNumber, relnode.ChildFileId, relnode.ChildMajRev, relnode.ChildMinRev, "RWU", (int)ApiTypes.ADEPTT_RTTREE_STYLE.RT_WIP, ViewContext.Id);
                            if (WhereUsedTbl != null && WhereUsedTbl.GetCount() > 0)
                            {
                                ucList.ReadObj(WhereUsedTbl);
                            }
                        }
                    }
                }
                if (tab.Text == "Children")
                {
                    NxContainerItem ci = _Project.ContainerManager.ColumnSet_ContainerList.FindName("AltGui_UCDIRel_WhereUsed");
                    if (ci == null)
                    {
                        ci = _Project.ContainerManager.ColumnSet_ContainerList.Add("AltGui_UCDIRel_Children", "");
                    }
                    if (ci.ContentsList.GetCount() < 1)
                    {
                        NxContentsItem Col;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LONGNAME"; Col.Width = 50;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_STATUS"; Col.Width = 20;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_VERSION"; Col.Width = 20;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LIBNAME"; Col.Width = 50;
                    }

                    NxViewContextManager ViewContextManager = _Project.ViewContextManager;
                    NxViewContextList ViewContextList = ViewContextManager.ViewContextList;
                    NxViewContext ViewContext = ViewContextList.FindName("AltGui_UCDIRel_Children");
                    if (ViewContext == null)
                    {
                        ViewContext = ViewContextList.Add();
                        ViewContext.Name = "AltGui_UCDIRel_Children";
                        ViewContext.TableName = "Children";
                        ViewContext.NumberOfLines = 20;
                        ViewContext.SortColumnSchemaId = "SCHEMA_S_LONGNAME";
                        ViewContext.bSortAscending = 1;
                        ViewContext.ColumnSetId = ci.Id;
                    }

                    UCList ucList = new UCList();
                    ucList._Project = _Project;
                    ucList._ViewContextId = ViewContext.Id;
                    //ucList.Tag = n;
                    ucList.Location = new Point(0, 0);
                    //ucList.Size = new Size(tab.Width, tab.Height);
                    tab.Controls.Add(ucList);

                    DoResize(this.Width, this.Height);

                    TreeNode tn = DIRelTree.SelectedNode;
                    if (tn != null)
                    {
                        NxRelNode relnode = (NxRelNode)tn.Tag;

                        if (relnode.ChildRelFile != null)
                        {
                            NxTbl WhereUsedTbl = _Project.VcGetRBChildren(relnode.ChildSourceTableNumber, relnode.ChildFileId, relnode.ChildMajRev, relnode.ChildMinRev, "RCH", (int)ApiTypes.ADEPTT_RTTREE_STYLE.RT_WIP, ViewContext.Id);
                            if (WhereUsedTbl != null && WhereUsedTbl.GetCount() > 0)
                            {
                                ucList.ReadObj(WhereUsedTbl);
                            }
                        }
                    }
                }
                if (tab.Text == "Revisions")
                {
                    NxContainerItem ci = _Project.ContainerManager.ColumnSet_ContainerList.FindName("AltGui_UCDIRel_Revisions");
                    if (ci == null)
                    {
                        ci = _Project.ContainerManager.ColumnSet_ContainerList.Add("AltGui_UCDIRel_Revisions", "");
                    }
                    if (ci.ContentsList.GetCount() < 1)
                    {
                        NxContentsItem Col;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LONGNAME"; Col.Width = 50;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_STATUS"; Col.Width = 20;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_VERSION"; Col.Width = 20;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LIBNAME"; Col.Width = 50;
                    }

                    NxViewContextManager ViewContextManager = _Project.ViewContextManager;
                    NxViewContextList ViewContextList = ViewContextManager.ViewContextList;
                    NxViewContext ViewContext = ViewContextList.FindName("AltGui_UCDIRel_Revisions");
                    if (ViewContext == null)
                    {
                        ViewContext = ViewContextList.Add();
                        ViewContext.Name = "AltGui_UCDIRel_Revisions";
                        ViewContext.TableName = "RRV";
                        ViewContext.NumberOfLines = 20;
                        ViewContext.SortColumnSchemaId = "SCHEMA_S_LONGNAME";
                        ViewContext.bSortAscending = 1;
                        ViewContext.ColumnSetId = ci.Id;
                    }

                    UCList ucList = new UCList();
                    ucList._Project = _Project;
                    ucList._ViewContextId = ViewContext.Id;
                    //ucList.Tag = n;
                    ucList.Location = new Point(0, 0);
                    //ucList.Size = new Size(tab.Width, tab.Height);
                    tab.Controls.Add(ucList);

                    DoResize(this.Width, this.Height);

                    TreeNode tn = DIRelTree.SelectedNode;
                    if (tn != null)
                    {
                        NxRelNode relnode = (NxRelNode)tn.Tag;

                        if (relnode.ChildRelFile != null)
                        {
                            //NxTbl WhereUsedTbl = _Project.GetRBWhereUsed(relnode.ChildSourceTableNumber, relnode.ChildFileId, relnode.ChildMajRev, relnode.ChildMinRev, "RBWhere", (int)ApiTypes.ADEPTT_RTTREE_STYLE.RT_WIP);
                            //if (WhereUsedTbl != null && WhereUsedTbl.GetCount() > 0)
                            //{
                            //    ucList.ReadObj(WhereUsedTbl);
                            //}
                            NxTbl RevTbl = _Project.VcGetRBVersions(relnode.ChildSourceTableNumber, relnode.ChildFileId, relnode.ChildMajRev, relnode.ChildMinRev, "RRV", ViewContext.Id);
                            if (RevTbl != null && RevTbl.GetCount() > 0)
                            {
                                ucList.ReadObj(RevTbl);
                            }
                        }
                    }
                }
                if (tab.Text == "WIP: Revisions")
                {
                    NxContainerItem ci = _Project.ContainerManager.ColumnSet_ContainerList.FindName("AltGui_UCDIRel_WipRev");
                    if (ci == null)
                    {
                        ci = _Project.ContainerManager.ColumnSet_ContainerList.Add("AltGui_UCDIRel_WipRev", "");
                    }
                    if (ci.ContentsList.GetCount() < 1)
                    {
                        NxContentsItem Col;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LONGNAME"; Col.Width = 50;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_STATUS"; Col.Width = 20;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_VERSION"; Col.Width = 20;
                        Col = ci.ContentsList.Add(); Col.ObjId = "SCHEMA_S_LIBNAME"; Col.Width = 50;
                    }

                    NxViewContextManager ViewContextManager = _Project.ViewContextManager;
                    NxViewContextList ViewContextList = ViewContextManager.ViewContextList;
                    NxViewContext ViewContext = ViewContextList.FindName("AltGui_UCDIRel_WipRev");
                    if (ViewContext == null)
                    {
                        ViewContext = ViewContextList.Add();
                        ViewContext.Name = "AltGui_UCDIRel_WipRev";
                        ViewContext.TableName = "RWR";
                        ViewContext.NumberOfLines = 20;
                        ViewContext.SortColumnSchemaId = "SCHEMA_S_LONGNAME";
                        ViewContext.bSortAscending = 1;
                        ViewContext.ColumnSetId = ci.Id;
                    }

                    UCList ucList = new UCList();
                    ucList._Project = _Project;
                    ucList._ViewContextId = ViewContext.Id;
                    //ucList.Tag = n;
                    ucList.Location = new Point(0, 0);
                    //ucList.Size = new Size(tab.Width, tab.Height);
                    tab.Controls.Add(ucList);

                    DoResize(this.Width, this.Height);

                    TreeNode tn = DIRelTree.SelectedNode;
                    if (tn != null)
                    {
                        NxRelNode relnode = (NxRelNode)tn.Tag;

                        if (relnode.ChildRelFile != null)
                        {
                            NxTbl WipRevTbl = _Project.VcGetRBWipVersions(relnode.ChildSourceTableNumber, relnode.ChildFileId, relnode.ChildMajRev, relnode.ChildMinRev, "RWR", ViewContext.Id);
                            if (WipRevTbl != null && WipRevTbl.GetCount() > 0)
                            {
                                ucList.ReadObj(WipRevTbl);
                            }
                        }
                    }
                }
                if (tab.Text == "Rel. Info")
                {
                    
                }
                
                
            }


        }


	}
}
