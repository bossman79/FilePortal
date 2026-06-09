using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;

using Interop.AdeptCAC;

namespace ACACTest1
{
	/// <summary>
	/// Summary description for UCList.
	/// </summary>
	public class UCList : System.Windows.Forms.UserControl , IMyResizable
	{
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private ContextMenuStrip CMS1;
		private ToolStripMenuItem editColumnsToolStripMenuItem;
		private IContainer components;

		public string _ViewContextId = ""; 
		public NxProject _Project = null; // Paul.Ligowski 2014-01-28 made public for setting after a new, instead of making it lookup where it is during load
		NxTbl _Tbl = null;

		bool _bLoadingColumns = false;

		public UCList()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		private void UCList_Load(object sender, EventArgs e)
		{
			TreeNode n = (TreeNode)this.Tag;

            // Paul.Ligowski 2014-01-28 caller could have set _Project instead of tag
            if(n != null)
            { 
			    TreeNode tempnode = n.Parent;
			    while (tempnode != null)
			    {
				    if (tempnode.Tag is NxProject)
				    {
					    _Project = (NxProject)tempnode.Tag;
					    tempnode = null;
				    }
				    else
					    tempnode = tempnode.Parent;
			    }
            }
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.CMS1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.CMS1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.listView1.ContextMenuStrip = this.CMS1;
			this.listView1.FullRowSelect = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(8, 8);
			this.listView1.Name = "listView1";
			this.listView1.Scrollable = false;
			this.listView1.Size = new System.Drawing.Size(528, 328);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView1_ColumnWidthChanged);
			this.listView1.Click += new System.EventHandler(this.listView1_Click);
			// 
			// CMS1
			// 
			this.CMS1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editColumnsToolStripMenuItem});
			this.CMS1.Name = "CMS1";
			this.CMS1.Size = new System.Drawing.Size(146, 26);
			// 
			// editColumnsToolStripMenuItem
			// 
			this.editColumnsToolStripMenuItem.Name = "editColumnsToolStripMenuItem";
			this.editColumnsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.editColumnsToolStripMenuItem.Text = "Edit Columns";
			this.editColumnsToolStripMenuItem.Click += new System.EventHandler(this.editColumnsToolStripMenuItem_Click);
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Location = new System.Drawing.Point(552, 8);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(17, 328);
			this.vScrollBar1.TabIndex = 1;
			this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
			this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
			this.vScrollBar1.LocationChanged += new System.EventHandler(this.vScrollBar1_LocationChanged);
			// 
			// UCList
			// 
			this.Controls.Add(this.vScrollBar1);
			this.Controls.Add(this.listView1);
			this.Name = "UCList";
			this.Size = new System.Drawing.Size(576, 352);
			this.Load += new System.EventHandler(this.UCList_Load);
			this.Resize += new System.EventHandler(this.UCList_Resize);
			this.CMS1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public UCDI m_ucdi = null;
		public void DoResize(int w, int h)
		{
			this.Width = w;
			this.Height = h;

			listView1.Left = this.Left;
			listView1.Top = this.Top;

			vScrollBar1.Top = this.Top;

			int fudge = 5;

			int sw = vScrollBar1.Width;
			listView1.Width = this.Width - sw - fudge;
			listView1.Height = this.Height - fudge;
			vScrollBar1.Left = listView1.Right;
			vScrollBar1.Height = listView1.Height;
		}
		private void UCList_Resize(object sender, System.EventArgs e)
		{
//			int sw = vScrollBar1.Width;
//			listView1.Width = (int)((this.Width - sw));
//			listView1.Height = (int)(this.Height);
//			vScrollBar1.Left = listView1.Right;
//			vScrollBar1.Height = listView1.Height;
		}
		
		//int pos = 0;
		public void ReadObj(NxTbl xTbl)
		{
			if (_Project == null)
				return;

			if(xTbl != null)
				_Tbl = xTbl;
			else
			{
				TreeNode waNode = (TreeNode)Tag;
				TreeNode walNode = waNode.Parent;
				TreeNode ProjectNode = walNode.Parent;

				String WorkAreaId = (String)waNode.Tag;
				//String FileN = "acac" + WorkAreaId;
				String FileN = "acacwa" + waNode.Index;
				NxProject Project = (NxProject)ProjectNode.Tag;
				_Tbl = (NxTbl)Project.OpenWorkAreaTable(WorkAreaId, FileN);
			}
			
			LoadColSet();

			if(_Tbl != null)
			{
				vScrollBar1.Minimum = 1;
				vScrollBar1.Maximum = _Tbl.SrvGetCount();
				SrvScroll(1);
			}
		}
		public void SrvScroll(int Start)
		{
			if (_Project == null)
				return;

			listView1.Items.Clear();
			if(_Tbl == null)
				return;

			// BY XML

			//int ColCount = 0;
			//string DelimitedColumns = "Columns=";
			//foreach (ColumnHeader ch in listView1.Columns)
			//{
			//    string SchemaId = (string)ch.Tag;
			//    if (ColCount > 0)
			//        DelimitedColumns += ",";
			//    DelimitedColumns += SchemaId;
			//    ColCount++;
			//}
			//string xml = _Tbl.SrvXMLScroll(Start, 50, DelimitedColumns);
			//XmlDocument doc = new XmlDocument();
			//doc.LoadXml(xml);
			//XmlNodeList Rows = doc.GetElementsByTagName("XMLSrvScroll")[0].ChildNodes;
			//for (int i = 0; i < Rows.Count; i++)
			//{
			//    ListViewItem lvi = listView1.Items.Add("");
			//    for (int c = 1; c <= ColCount; c++) 
			//    {
			//        lvi.SubItems.Add("");
			//    }
			//    XmlNode Row = Rows[i];
			//    XmlNodeList Cols = Row.ChildNodes;
			//    for (int j = 0; j < Cols.Count; j++)
			//    {
			//        XmlNode Col = Cols[j];
			//        foreach (ColumnHeader ch in listView1.Columns)
			//        {
			//            string SchemaId = (string)ch.Tag;
			//            if (string.Compare(SchemaId, Col.Name) == 0)
			//            {
			//                lvi.SubItems[ch.Index].Text = Col.InnerText;
			//            }
			//        }
			//    }
			//}

			// BY TABLE

			int es = _Tbl.SrvScroll(Start, 50);
			if (es == 0)
			{
				for (int i = 1; i <= _Tbl.GetCount(); i++)
				{
					es = _Tbl.GotoRecord(i);
					if (es == 0)
					{
						//String name = Tbl.GetString("S_LONGNAME");
						//ListViewItem lvi = listView1.Items.Add(name);
						//lvi.SubItems.Add(Tbl.GetString("S_SRCDB"));
						//lvi.SubItems.Add(Tbl.GetString("S_FILEID"));
						//lvi.SubItems.Add(Tbl.GetString("S_MAJREV"));
						//lvi.SubItems.Add(Tbl.GetString("S_MINREV"));

						ListViewItem lvi = listView1.Items.Add("");
						for (int c = 1; c < listView1.Columns.Count; c++) // start at 1 because Add makes 0
						{
							lvi.SubItems.Add("");
						}
						foreach (ColumnHeader ch in listView1.Columns)
						{
							string SchemaId = (string)ch.Tag;
							string Text = _Tbl.GetDisplayStringFromSchemaId(SchemaId);
							lvi.SubItems[ch.Index].Text = Text;
						}
					}
				}
			}
		}

		private void vScrollBar1_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
		}
		private void vScrollBar1_ValueChanged(object sender, System.EventArgs e)
		{
			int pos = vScrollBar1.Value;
			SrvScroll(pos);
		}
		private void vScrollBar1_LocationChanged(object sender, System.EventArgs e)
		{
		}

		

		private void listView1_Click(object sender, System.EventArgs e)
		{
			if (_Project == null)
				return;

			if(listView1.SelectedItems.Count < 1)
				return;
			
			if ((this.Tag is TreeNode) == false)
				return;

			ListViewItem lvi = listView1.SelectedItems[0];

			_Tbl.GotoRecord(lvi.Index + 1); // +1 for tbl being ones based

			string sSTN = _Tbl.GetString("S_SRCDB"); if (string.IsNullOrEmpty(sSTN)) sSTN = "-1";
			string sMajRev = _Tbl.GetString("S_MAJREV"); if (string.IsNullOrEmpty(sMajRev)) sMajRev = "-1";
			string sMinRev = _Tbl.GetString("S_MINREV"); if (string.IsNullOrEmpty(sMinRev)) sMinRev = "-1";
			
			int STN = System.Convert.ToInt32(sSTN);
			string FileId = _Tbl.GetString("S_FILEID");
			int MajRev = System.Convert.ToInt32(sMajRev);
			int MinRev = System.Convert.ToInt32(sMinRev);

			if(m_ucdi != null)
				m_ucdi.Set(_Project, STN, FileId, MajRev, MinRev);
			
		}



		private void editColumnsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_Project == null)
				return;

			NxViewContext vc = _Project.ViewContextManager.ViewContextList.FindId(_ViewContextId);
			if(vc == null)
			{
				MessageBox.Show("Internal Error: View Context is null.");
				return;
			}
			string ColSetId = vc.ColumnSetId;
			NxContainerItem ColSet = _Project.ContainerManager.ColumnSet_ContainerList.FindId(ColSetId);
			if(ColSet == null)
			{
				MessageBox.Show("Internal Error: Column Set is null.");
				return;
			}

			ColumnPicker cp = new ColumnPicker();
			cp._Project = _Project;
			foreach (ColumnHeader ch in listView1.Columns)
			{
				cp._Cols.Add((string)ch.Tag);
			}
			if (cp.ShowDialog() == DialogResult.OK)
			{

				ColSet.ContentsList.Clear();
				NxSchemaItem si = null;
				foreach (string SchemaId in cp._Cols)
				{
					si = _Project.SchemaManager.SchemaList.FindId(SchemaId);
					if (si != null)
					{
						int width = FindColWidth(SchemaId); // get existing width

						NxContentsItem ContentsItem = ColSet.ContentsList.Add();
						ContentsItem.ObjId = SchemaId;
						ContentsItem.Width = width;
					}
				}
				ColSet.UpdateContentsList();

				LoadColSet();

				
				int curpos = _Tbl.SrvGetFirstRecordNumber();
				SrvScroll(curpos);

				
			}
		}

		int FindColWidth(string SchemaId)
		{
			foreach (ColumnHeader ch in listView1.Columns)
			{
				string thisSchemaId = (string) ch.Tag;
				if (string.Compare(thisSchemaId, SchemaId, true) == 0)
					return ch.Width;
			}
			return 50; // some default
		}

		void LoadColSet()
		{
			_bLoadingColumns = true;

			listView1.Columns.Clear();

			NxViewContext vc = _Project.ViewContextManager.ViewContextList.FindId(_ViewContextId);
			if (vc == null)
			{
				MessageBox.Show("Internal Error: View Context is null.");
				return;
			}
			string ColSetId = vc.ColumnSetId;
			NxContainerItem ColSet = _Project.ContainerManager.ColumnSet_ContainerList.FindId(ColSetId);
			if (ColSet == null)
			{
				MessageBox.Show("Internal Error: Column Set is null.");
				return;
			}

			NxContentsList cl = ColSet.ContentsList;
			NxContentsItem ci = null;
			string SchemaId = "";
			NxSchemaList sl = _Project.SchemaManager.SchemaList;
			NxSchemaItem si = null;
			ColumnHeader ch = null;
			int count = cl.GetCount();
			for (int i = 0; i < count; i++)
			{
				ci = cl.GetItem(i);
				SchemaId = ci.ObjId;
				si = sl.FindId(SchemaId);
				if(si != null)
					ch = listView1.Columns.Add(si.DisplayName, ci.Width, System.Windows.Forms.HorizontalAlignment.Left); ch.Tag = SchemaId;
			}

			_bLoadingColumns = false;
		}

		void FlushColSet()
		{
			if (_bLoadingColumns)
				return;

			NxViewContext vc = _Project.ViewContextManager.ViewContextList.FindId(_ViewContextId);
			if (vc == null)
			{
				MessageBox.Show("Internal Error: View Context is null.");
				return;
			}
			string ColSetId = vc.ColumnSetId;
			NxContainerItem ColSet = _Project.ContainerManager.ColumnSet_ContainerList.FindId(ColSetId);
			if (ColSet == null)
			{
				MessageBox.Show("Internal Error: Column Set is null.");
				return;
			}

			ColSet.ContentsList.Clear();
			foreach (ColumnHeader ch in listView1.Columns)
			{
				NxContentsItem ContentsItem = ColSet.ContentsList.Add();
				ContentsItem.ObjId = (string)ch.Tag;
				ContentsItem.Width = ch.Width;
			}
			ColSet.UpdateContentsList();
		}

		private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			FlushColSet();
		}

	}
}
