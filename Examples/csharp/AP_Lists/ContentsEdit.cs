using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

using Interop.AdeptCAC;
using Interop.AdeptGui;
using Synergis.Adept.MainApi;

namespace AP_Lists
{
	public partial class ContentsEdit : Form
	{
		public NxProject _Project;
		public NxDetailedList _dl;
		public ListsForm.ListType _lt;
		public NxContainerItem _ci;

		int lvoutObjIdCol = 0;
		int lvoutNameCol = 1;
		//int lvoutSTN = -1;

		public ContentsEdit()
		{
			InitializeComponent();
		}

		private void ContentsEdit_Load(object sender, EventArgs e)
		{
			try
			{
				LoadAvailable();

				LVIn.Columns.Add("Id");
				LVIn.Columns.Add("ObjId");
				LVIn.Columns.Add("Name");
                if (_lt == ListsForm.ListType.ColumnSetList_ContainerList)
                {
                    LVIn.Columns.Add("Width");
                    LVIn.Columns.Add("Value"); // // Just to see. I'm putting up just to see what is in there. S_OBJID is the real SchemaId.
                }
                else if (_lt == ListsForm.ListType.SavedSearchCriteria_ContainerList || _lt == ListsForm.ListType.TransmittalSavedSearchCriteria_ContainerList)
                {
                    LVIn.Columns.Add("Search Op");
                    LVIn.Columns.Add("Value");
                    LVIn.Columns.Add("Value2");
                }
				NxContentsItem ContentsItem = null;
				if (_ci.ContentsList != null)
				{
					int count = _ci.ContentsList.GetCount();
					for (int i = 0; i < count; i++)
					{
						ContentsItem = _ci.ContentsList.GetItem(i);
						ListViewItem lvi = LVIn.Items.Add(ContentsItem.Id);
						//lvi.Tag = ContentsItem;
						lvi.SubItems.Add(ContentsItem.ObjId);

						// Name
						lvi.SubItems.Add(GetNameForId(ContentsItem.ObjId));

						if (_lt == ListsForm.ListType.ColumnSetList_ContainerList)
						{
							int local = ContentsItem.Width;
							lvi.SubItems.Add(local.ToString());
                            lvi.SubItems.Add(ContentsItem.Value); // Just to see.
						}
						else if (_lt == ListsForm.ListType.SavedSearchCriteria_ContainerList || _lt == ListsForm.ListType.TransmittalSavedSearchCriteria_ContainerList)
						{
							ListViewItem.ListViewSubItem lvsi = lvi.SubItems.Add(UIHelper.SearchOpToDisplayString(_Project, (ApiTypes.ADEPTT_SEARCHOP)ContentsItem.SearchOp)); // ContentsItem.SearchOp.ToString());
							lvsi.Tag = ContentsItem.SearchOp;

							lvi.SubItems.Add(ContentsItem.Value);

							lvi.SubItems.Add(ContentsItem.Value2);
						}
					}
				}

				if (_lt != ListsForm.ListType.ColumnSetList_ContainerList && _lt != ListsForm.ListType.SavedSearchCriteria_ContainerList && _lt != ListsForm.ListType.TransmittalSavedSearchCriteria_ContainerList)
					BTEdit.Visible = false;

				TVOut.Visible = _lt == ListsForm.ListType.LibrarySet_ContainerList;
				LVOut.Visible = _lt != ListsForm.ListType.LibrarySet_ContainerList;

				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			ContentsEdit_Resize(null, null);
		}

		private void ContentsEdit_Resize(object sender, EventArgs e)
		{
			LVIn.Left = 10;
			LVIn.Width = (this.ClientSize.Width - BTAdd.Width - 40) / 2;
			
			BTAdd.Left = LVIn.Left + LVIn.Width + 10;
			BTRemove.Left = BTAdd.Left;
            BTRemoveAll.Left = BTAdd.Left;

            BTUp.Left = BTAdd.Left;
            BTDown.Left = BTAdd.Left;
			
			LVOut.Left = BTAdd.Left + BTAdd.Width + 10;
			LVOut.Width = LVIn.Width;

			LVIn.Height = this.ClientSize.Height - LVIn.Top - 10;
			LVOut.Height = LVIn.Height;

			BTClose.Left = this.ClientSize.Width - BTClose.Width - 10;
		}

		private void ContentsEdit_FormClosing(object sender, FormClosingEventArgs e)
		{
            _ci.ContentsList.Clear();
            foreach(ListViewItem lvi in LVIn.Items)
            {
                NxContentsItem con = _ci.ContentsList.Add();
                con.ObjId = lvi.SubItems[1].Text; // ALWAYS 1

                if (_lt == ListsForm.ListType.LibrarySet_ContainerList)
                {
                    con.Value = lvi.SubItems[2].Text;
                }
                else if (_lt == ListsForm.ListType.TransmittalSavedSearchCriteria_ContainerList)
                {
                   con.SearchOp = System.Convert.ToInt32(lvi.SubItems[3].Text);
                   con.Value = lvi.SubItems[4].Text;
                }
                else
                {
                    if (_lt == ListsForm.ListType.ColumnSetList_ContainerList)
                    {
                        con.Width = System.Convert.ToInt32(lvi.SubItems[3].Text);
                    }
                    else if (_lt == ListsForm.ListType.SavedSearchCriteria_ContainerList)
                    {
                        con.SearchOp = (int)lvi.SubItems[3].Tag;
                        con.Value = lvi.SubItems[4].Text;
                        con.Value2 = lvi.SubItems[5].Text;
                    }
                }  
            }
			_ci.UpdateContentsList();
		}

		string GetNameForId(string Id)
		{
			string rtn = "";
			int es = 0;
			NxDocRecord DocRec = null;
			NxUser User = null;
			NxSchemaItem SchemaItem = null;
			NxLibrary Library = null;
			
			switch (_lt)
			{
				case(ListsForm.ListType.DocList_ContainerList):
					DocRec = _Project.NewDocRecord();
					DocRec.CreateFromNumber((int)  ApiTypes.NFM_DB_NUM.C_FILES);
					DocRec.SetWip(Id);
					es = DocRec.Fetch(0);
					if (es != 0) // ADEPT_ERROR_CODE.EC_NO_ERROR
						_Project.Login.Core.GetErrorString(es, rtn);
					else
						rtn = DocRec.GetDisplayStringFromSchemaId("SCHEMA_S_LONGNAME");
					break;
				case (ListsForm.ListType.UserList_ContainerList):
					User = _Project.VLGUManager.UserList.FindId(Id);
					if (User == null)
						rtn = "<User not found>";
					else
						rtn = User.UserName;
					break;
				case (ListsForm.ListType.ColumnSetList_ContainerList):
					SchemaItem = _Project.SchemaManager.SchemaList.FindId(Id);
					if(SchemaItem == null)
						rtn = "<Schema not found>";
					else
						rtn = SchemaItem.DisplayName;
					break;
				case (ListsForm.ListType.SavedSearchCriteria_ContainerList):
                    if (string.Compare(Id, ApiTypes.SEARCH_SEPARATOR_APPEND, true) != 0 && string.Compare(Id, ApiTypes.SEARCH_SPARATOR_REFINE, true) != 0)
                    {
                        SchemaItem = _Project.SchemaManager.SchemaList.FindId(Id);
                        if (SchemaItem == null)
                            rtn = "<Schema not found>";
                        else
                            rtn = SchemaItem.DisplayName;
                    }
					break;
				case (ListsForm.ListType.LibrarySet_ContainerList):
					Library = _Project.VLGUManager.LibraryList.FindId(Id);
					if (Library == null)
						rtn = "<Library not found>";
					else
                    {
						rtn = Library.Name;
                        rtn += " (Comment1=" + Library.Comment1 + ")";
                    }
					break;
				case(ListsForm.ListType.TransmittalSavedSearchCriteria_ContainerList):
					foreach(ListViewItem lvi in LVOut.Items)
					{
						if(string.Compare(lvi.SubItems[lvoutObjIdCol].Text, Id) == 0)
						{
							rtn = lvi.SubItems[lvoutNameCol].Text;
						}
					}
					break;
			}
			return rtn;
		}


        private void BTUp_Click(object sender, EventArgs e)
        {
            if(LVIn.SelectedItems.Count < 1 || LVIn.SelectedItems.Count > 1)
            {
                MessageBox.Show("Select only 1 item.");
                return;
            }
            if (LVIn.SelectedItems[0].Index > 0) 
            {
                int index = LVIn.SelectedItems[0].Index;
                ListViewItem lvi = LVIn.SelectedItems[0];
                LVIn.Items.RemoveAt(index);
                LVIn.Items.Insert(index - 1, lvi);
            }
        }

        private void BTDown_Click(object sender, EventArgs e)
        {
            if (LVIn.SelectedItems.Count < 1 || LVIn.SelectedItems.Count > 1)
            {
                MessageBox.Show("Select only 1 item.");
                return;
            }
            if (LVIn.SelectedItems[0].Index < LVIn.Items.Count -1)
            {
                int index = LVIn.SelectedItems[0].Index;
                ListViewItem lvi = LVIn.SelectedItems[0];
                LVIn.Items.RemoveAt(index);
                LVIn.Items.Insert(index + 1, lvi);
            }
        }

		private void BTAdd_Click(object sender, EventArgs e)
		{
			if (_lt == ListsForm.ListType.LibrarySet_ContainerList)
			{
				if (TVOut.SelectedNode == null)
				{
					MessageBox.Show("Nothing selected.");
					return;
				}
				if (TVOut.SelectedNode.Parent == null)
				{
					MessageBox.Show("Can't add a Vault.");
					return;
				}
				NxFileGuideNode fgn = (NxFileGuideNode)TVOut.SelectedNode.Tag;
				//NxContentsItem con = _ci.ContentsList.Add();
				//con.ObjId = fgn.Id;
				ListViewItem newlvi = LVIn.Items.Add(""); //con.Id
				//newlvi.Tag = con;
                newlvi.SubItems.Add(fgn.Id);
				newlvi.SubItems.Add(fgn.Value);
			}
			else if (_lt == ListsForm.ListType.TransmittalSavedSearchCriteria_ContainerList)
			{
				if (LVOut.SelectedItems.Count < 1)
				{
					MessageBox.Show("Nothing selected.");
					return;
				}

				foreach (ListViewItem lviout in LVOut.SelectedItems)
				{
					//NxContentsItem con = _ci.ContentsList.Add();
					//con.ObjId = lviout.SubItems[lvoutObjIdCol].Text;
					//con.SourceTableNumber = lvoutSTN;
					ListViewItem newlvi = LVIn.Items.Add(""); // con.Id
					//newlvi.Tag = con;
                    newlvi.SubItems.Add(lviout.SubItems[lvoutObjIdCol].Text);
					newlvi.SubItems.Add(lviout.SubItems[lvoutNameCol].Text);
					newlvi.SubItems.Add(""); // SearchOp
					newlvi.SubItems.Add(""); // Value
				}
			}
			else
			{
				if (LVOut.SelectedItems.Count < 1)
				{
					MessageBox.Show("Nothing selected.");
					return;
				}
				foreach (ListViewItem lviout in LVOut.SelectedItems)
				{
					//NxContentsItem con = _ci.ContentsList.Add();
					//con.ObjId = lviout.SubItems[lvoutObjIdCol].Text;
					ListViewItem newlvi = LVIn.Items.Add(""); // con.Id
					//newlvi.Tag = con;
                    newlvi.SubItems.Add(lviout.SubItems[lvoutObjIdCol].Text);
					newlvi.SubItems.Add(lviout.SubItems[lvoutNameCol].Text);
                    if (_lt == ListsForm.ListType.ColumnSetList_ContainerList)
                    {
                        //con.Width = 50;
                        newlvi.SubItems.Add("50");
                        newlvi.SubItems.Add(""); // Just to see.
                    }
                    else if (_lt == ListsForm.ListType.SavedSearchCriteria_ContainerList)
                    {
                        ListViewItem.ListViewSubItem lvsi = newlvi.SubItems.Add(""); // SearchOp
                        lvsi.Tag = ApiTypes.ADEPTT_SEARCHOP.OP_STARTS;
                        newlvi.SubItems.Add(""); // Value
                        newlvi.SubItems.Add(""); // Value2
                    }
				}
			}
		}

		private void BTRemove_Click(object sender, EventArgs e)
		{
			if (LVIn.SelectedItems.Count < 1)
			{
				MessageBox.Show("Nothing selected.");
				return;
			}
			foreach (ListViewItem lvi in LVIn.SelectedItems)
			{
				// delete if from the list
				//string Id = lvi.SubItems[0].Text;
				//_ci.ContentsList.Delete(Id);

				// remove it from the window
				LVIn.Items.Remove(lvi);
			}
		}

        private void BTRemoveAll_Click(object sender, EventArgs e)
        {
            //_ci.ContentsList.Clear();
            LVIn.Items.Clear();
        }

		private void BTEdit_Click(object sender, EventArgs e)
		{
			if (LVIn.SelectedItems.Count < 1)
			{
				MessageBox.Show("Nothing selected.");
				return;
			}
			if (_lt == ListsForm.ListType.ColumnSetList_ContainerList)
			{
				int width = System.Convert.ToInt32(LVIn.SelectedItems[0].SubItems[3].Text);
				WidthEdit we = new WidthEdit();
				we._Width = width;
				if (we.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					foreach (ListViewItem lvi in LVIn.SelectedItems)
					{
						// local variable required or get ...
						// warning CS1690: Accessing a member on 'AP_Lists.WidthEdit._Width' may cause a runtime exception because it is a field of a marshal-by-reference class
						//lvi.SubItems[3].Text = we._Width.ToString();
						int local = we._Width;

						//NxContentsItem con = (NxContentsItem)lvi.Tag;
						//con.Width = local;

						lvi.SubItems[3].Text = local.ToString();
					}
				}
			}
			else if (_lt == ListsForm.ListType.SavedSearchCriteria_ContainerList || _lt == ListsForm.ListType.TransmittalSavedSearchCriteria_ContainerList)
			{
                ApiTypes.ADEPTT_SEARCHOP iSearchOp = ApiTypes.ADEPTT_SEARCHOP.OP_STARTS;
                if (LVIn.SelectedItems[0].SubItems[3].Tag != null)
                {
                    iSearchOp = (ApiTypes.ADEPTT_SEARCHOP)(int)(LVIn.SelectedItems[0].SubItems[3].Tag);
                }
				string value = LVIn.SelectedItems[0].SubItems[4].Text;
				string value2 = LVIn.SelectedItems[0].SubItems[5].Text;

				SearchCriteriaEdit se = new SearchCriteriaEdit();
				se._Project = _Project;
				se._SearchOp = iSearchOp; // UIHelper.StringToSearchOp(sSearchOp);
				se._Value = value;
				se._Value2 = value2;
				if (se.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					foreach (ListViewItem lvi in LVIn.SelectedItems)
					{
						//NxContentsItem con = (NxContentsItem)lvi.Tag;
                        string ObjId = lvi.SubItems[1].Text;

                        if (string.Compare(ObjId, ApiTypes.SEARCH_SEPARATOR_APPEND, true) != 0 && string.Compare(ObjId, ApiTypes.SEARCH_SPARATOR_REFINE, true) != 0)
                        {
                            //con.SearchOp = (int)se._SearchOp;
                            //con.Value = se._Value;
                            //con.Value2 = se._Value2;

                            lvi.SubItems[3].Text = UIHelper.SearchOpToDisplayString(_Project, se._SearchOp); //UIHelper.SearchOpToString(se._SearchOp);
                            lvi.SubItems[3].Tag = se._SearchOp; // Tag
                            lvi.SubItems[4].Text = se._Value;
                            lvi.SubItems[5].Text = se._Value2;
                        }
					}
				}
			}
		}

		void LoadAvailable()
		{
			// default
			LVOut.Columns.Clear();
			LVOut.Columns.Add("ObjId");
			LVOut.Columns.Add("Name");
			lvoutObjIdCol = 0;
			lvoutNameCol = 1;
			//lvoutSTN = -1;

			LBLAvailableMessage.Text = "";
			switch (_lt)
			{
				case (ListsForm.ListType.DocList_ContainerList):
					LoadAvailableDocs();
					break;
				case (ListsForm.ListType.UserList_ContainerList):
					LoadAvailableUsers();
					break;
				case (ListsForm.ListType.ColumnSetList_ContainerList):
					LoadAvailableColumns();
					break;
				case (ListsForm.ListType.SavedSearchCriteria_ContainerList):
					LoadAvailableColumns();
					break;
				case (ListsForm.ListType.LibrarySet_ContainerList):
					LoadAvailableLibraries();
					break;
				case (ListsForm.ListType.TransmittalSavedSearchCriteria_ContainerList):
					LoadAvailableTransmittalColumns();
					break;
			}
		}
		void LoadAvailableDocs()
		{
			if (_dl == null)
			{
				LBLAvailableMessage.Text = "No selection list is active in the client.";
				return;
			}
			if (_dl.GetCount() < 1)
			{
				LBLAvailableMessage.Text = "The active selection list is empty in the client.";
				return;
			}
			ListViewItem lvi = null;
			NxDetailedItem di = null;
			int count = _dl.GetCount();
			for (int i = 0; i < count; i++)
			{
				di = _dl.GetItem(i);
				lvi = LVOut.Items.Add(di.FileId);
				lvi.SubItems.Add(di.FileNE);
			}
		}
		void LoadAvailableUsers()
		{
			ListViewItem lvi = null;
			NxUser User = null;
			int count = _Project.VLGUManager.UserList.GetCount();
			for (int i = 0; i < count; i++)
			{
				User = _Project.VLGUManager.UserList.GetItem(i);
				lvi = LVOut.Items.Add(User.Id);
				lvi.SubItems.Add(User.UserName);
			}
		}
		void LoadAvailableColumns()
		{
			ListViewItem lvi = null;

            lvi = LVOut.Items.Add(ApiTypes.SEARCH_SEPARATOR_APPEND);
            lvi.SubItems.Add("");
            lvi = LVOut.Items.Add(ApiTypes.SEARCH_SPARATOR_REFINE);
            lvi.SubItems.Add("");

			NxSchemaItem SchemaItem = null;
			int count = _Project.SchemaManager.SchemaList.GetCount();
			for (int i = 0; i < count; i++)
			{
				SchemaItem = _Project.SchemaManager.SchemaList.GetItem(i);
				lvi = LVOut.Items.Add(SchemaItem.Id);
				lvi.SubItems.Add(SchemaItem.DisplayName);
			}
		}
		void LoadAvailableLibraries()
		{
			//ListViewItem lvi = null;
			//NxLibrary Library = null;
			//int count = _Project.VLGUManager.LibraryList.GetCount();
			//if (count < 1)
			//{
			//    LBLAvailableMessage.Text = "No libraries have been added to the client cache yet.";
			//    return;
			//}
			//for (int i = 0; i < count; i++)
			//{
			//    Library = _Project.VLGUManager.LibraryList.GetItem(i);
			//    lvi = LVOut.Items.Add(Library.Id);
			//    lvi.SubItems.Add(Library.Name);
			//}

			//int count = _Project.FileGuideManager.FileGuideList.GetCount();
			//for(int i = 0; i < count; i++)
			//{
			//    NxFileGuideItem tmp =  _Project.FileGuideManager.FileGuideList.GetItem(i);
			//    string Id = tmp.Id;
			//    string Name = tmp.Name;
			//}

			NxFileGuideItem fgi = _Project.FileGuideManager.FileGuideList.FindId("__E_VIEW_ID__");

			if (fgi == null)
				LBLAvailableMessage.Text = "Failed to find Library Browser View.";
			else
			{
				fgi.BuildTreeForOnDemand();
				AddNode(null, fgi.RootNode);
			}

		}

		void LoadAvailableTransmittalColumns()
		{
			LVOut.Columns.Clear();
			LVOut.Columns.Add("Set Name");
			LVOut.Columns.Add("STN");
			LVOut.Columns.Add("Field");
			LVOut.Columns.Add("Desc");
			LVOut.Columns.Add("Type");
			lvoutObjIdCol = 2;
			lvoutNameCol = 3;
			//lvoutSTN = 1;

			string xml = _Project.GetXmlTransmittalSearchColumns("");
			XDocument xdoc = XDocument.Parse(xml);
			foreach (XElement Main in xdoc.Descendants("TransmittalSearchColumns"))
			{
				foreach (XElement Set in Main.Descendants("Set"))
				{
					string Name = Set.Attribute("Name").Value;
					string sSTN = Set.Attribute("STN").Value;
					foreach (XElement Column in Set.Descendants("Column"))
					{
						string Field = Column.Element("Field").Value;
						string Desc = Column.Element("Desc").Value;
						string Type = Column.Element("Type").Value;

						ListViewItem lvi = LVOut.Items.Add(Name);
						lvi.SubItems.Add(sSTN);
						lvi.SubItems.Add(Field);
						lvi.SubItems.Add(Desc);
						lvi.SubItems.Add(Type);
					}
				}
			}
		}

		void AddNode(TreeNode tn, NxFileGuideNode fgn)
		{
			if (fgn == null)
				return;

			TreeNode tnnew = null;
			if (tn == null)
				tnnew = TVOut.Nodes.Add(fgn.Value);
			else
				tnnew = tn.Nodes.Add(fgn.Value);

			tnnew.Tag = fgn;
			if (fgn.bHasChildren == 1)
			{
				TreeNode placeholder = tnnew.Nodes.Add("Placeholder");
			}

			AddNode(tn, fgn.NextSibling);
		}

		private void TVOut_AfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode tn = e.Node;
			if (tn.Nodes.Count == 1)
			{
				object tmp = tn.Nodes[0].Tag;
				if (tmp == null)
				{
					tn.Nodes.Clear();

					NxFileGuideNode fgn = (NxFileGuideNode)tn.Tag;

					AddNode(tn, fgn.FirstChild);
				}
			}
		}








	}
}

