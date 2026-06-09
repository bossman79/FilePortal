using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Interop.AdeptCAC;
using Interop.AdeptGui;

namespace AP_Lists
{
	public partial class ListsForm : Form
	{
		public enum ListType { None, DocList_ContainerList, UserList_ContainerList, ColumnSetList_ContainerList, SavedSearchCriteria_ContainerList, LibrarySet_ContainerList, TransmittalSavedSearchCriteria_ContainerList };

		//public GuiApi _GuiApi = null;
		public NxProject _Project = null;
		public NxDetailedList _dl;

		NxContainerManager _ContainerManager;
		ListType _lt = ListType.None;
		NxContainerList _ContainerList;

		public ListsForm()
		{
			InitializeComponent();
		}

		private void ListsForm_Load(object sender, EventArgs e)
		{
			_ContainerManager = _Project.ContainerManager;

			_ContainerManager.Refresh();

			int index = CBListType.Items.Add(new CBListTypeItem("Doc Lists", ListType.DocList_ContainerList));
			CBListType.Items.Add(new CBListTypeItem("User Lists", ListType.UserList_ContainerList));
			CBListType.Items.Add(new CBListTypeItem("Column Set Lists", ListType.ColumnSetList_ContainerList));
			CBListType.Items.Add(new CBListTypeItem("Saved Search Criteria Lists", ListType.SavedSearchCriteria_ContainerList));
			CBListType.Items.Add(new CBListTypeItem("Library Set Lists", ListType.LibrarySet_ContainerList));
			CBListType.Items.Add(new CBListTypeItem("Transmittal Saved Search Criteria Lists", ListType.TransmittalSavedSearchCriteria_ContainerList));
			

			LVMain.Columns.Add("Id");
			LVMain.Columns.Add("Name");
			LVMain.Columns.Add("Comment");
			LVMain.Columns.Add("OwnerId");
			LVMain.Columns.Add("IsGlobal");
			LVMain.Columns.Add("IsShared");
			LVMain.Columns.Add("CanEdit");
			LVMain.Columns.Add("CanAdmin");

			CBListType.SelectedIndex = index;

			DoEnables();

			ListsForm_Resize(null, null);
		}

		private void ListsForm_Resize(object sender, EventArgs e)
		{
			BTClose.Left = this.ClientSize.Width - BTClose.Width - 10;
			BTAdd.Left = this.ClientSize.Width - BTClose.Width - 10;
			BTEdit.Left = this.ClientSize.Width - BTClose.Width - 10;
			BTEditContents.Left = this.ClientSize.Width - BTClose.Width - 10;
			BTDelete.Left = this.ClientSize.Width - BTClose.Width - 10;

			LVMain.Left = 10;
			LVMain.Width = this.ClientSize.Width - BTAdd.Width - 30;

			LVMain.Height = this.ClientSize.Height - LVMain.Top - 10;
		}

		private void CBListType_SelectedIndexChanged(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			LVMain.Items.Clear();
			_lt = ListType.None;
			_ContainerList = null;

			CBListTypeItem lti = (CBListTypeItem) CBListType.SelectedItem;
			if (lti != null)
			{
				_lt = lti._lt;
				switch (_lt)
				{
					case(ListType.DocList_ContainerList):
						_ContainerList = _ContainerManager.DocList_ContainerList;
						break;
					case (ListType.UserList_ContainerList):
						_ContainerList = _ContainerManager.UserList_ContainerList;
						break;
					case (ListType.ColumnSetList_ContainerList):
						_ContainerList = _ContainerManager.ColumnSet_ContainerList;
						break;
					case (ListType.SavedSearchCriteria_ContainerList):
						_ContainerList = _ContainerManager.SavedSearchCriteria_ContainerList;
						break;
					case (ListType.LibrarySet_ContainerList):
						_ContainerList = _ContainerManager.LibrarySet_ContainerList;
						break;
					case (ListType.TransmittalSavedSearchCriteria_ContainerList):
						_ContainerList = _ContainerManager.TransmittalSavedSearchCriteria_ContainerList;
						break;
				}
			}
			NxContainerItem ci = null;
			int count = _ContainerList.GetCount();
			for (int i = 0; i < count; i++)
			{
				ci = _ContainerList.GetItem(i);
				AddContainer(ci);
			}

			Cursor.Current = Cursors.Default;
		}

		private void LVMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			DoEnables();
		}

		void DoEnables()
		{
			bool bButtonEnable = _ContainerList != null;
			BTAdd.Enabled = _ContainerList != null;
			BTEdit.Enabled = LVMain.SelectedItems.Count == 1;
			BTEditContents.Enabled = LVMain.SelectedItems.Count == 1;
			BTDelete.Enabled = LVMain.SelectedItems.Count > 0;
		}

		void AddContainer(NxContainerItem ci)
		{
			ListViewItem lvi = LVMain.Items.Add(ci.Id);
			lvi.Tag = ci;
			lvi.SubItems.Add(ci.Name);
			lvi.SubItems.Add(ci.Comment);
			lvi.SubItems.Add(ci.OwnerId);
			lvi.SubItems.Add(ci.bIsGlobal.ToString());
			lvi.SubItems.Add(ci.bIsShared.ToString());
			lvi.SubItems.Add(ci.bCanEdit.ToString());
			lvi.SubItems.Add(ci.bCanAdmin.ToString());
		}

		private void BTAdd_Click(object sender, EventArgs e)
		{
			ContainerEdit containerEdit = new ContainerEdit();
			containerEdit._bAdd = true;
			if (containerEdit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				NxContainerItem ci = _ContainerList.Add(containerEdit._Name, containerEdit._Comment);
				AddContainer(ci);
			}
		}

		private void BTEdit_Click(object sender, EventArgs e)
		{
			if(LVMain.SelectedItems.Count < 1)
			{
				MessageBox.Show("Nothing selected.");
				return;
			}
			ListViewItem lvi = LVMain.SelectedItems[0];
			NxContainerItem ci = (NxContainerItem) lvi.Tag;
			ContainerEdit containerEdit = new ContainerEdit();
			containerEdit._bAdd = false;
			containerEdit._Name = ci.Name;
			containerEdit._Comment = ci.Comment;
			if (containerEdit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				ci.Update(containerEdit._Name, containerEdit._Comment);

				lvi.SubItems[1].Text = ci.Name;
				lvi.SubItems[2].Text = ci.Comment;
			}
		}

		private void BTEditContents_Click(object sender, EventArgs e)
		{
			if (LVMain.SelectedItems.Count < 1)
			{
				MessageBox.Show("Nothing selected.");
				return;
			}
			ListViewItem lvi = LVMain.SelectedItems[0];
			NxContainerItem ci = (NxContainerItem)lvi.Tag;
			ContentsEdit contentsEdit = new ContentsEdit();
			contentsEdit._Project = _Project;
			contentsEdit._dl = _dl;
			contentsEdit._lt = _lt;
			contentsEdit._ci = ci;
			contentsEdit.ShowDialog();
		}

		private void BTDelete_Click(object sender, EventArgs e)
		{
			if (LVMain.SelectedItems.Count < 1)
			{
				MessageBox.Show("Nothing selected.");
				return;
			}
			NxContainerItem ci = null;
			foreach (ListViewItem lvi in LVMain.SelectedItems)
			{
				ci = (NxContainerItem)lvi.Tag;
				string Id = ci.Id;
				ci = null;
				_ContainerList.Delete(Id);
				LVMain.Items.Remove(lvi);
			}
		}


		class CBListTypeItem
		{
			public string _DisplayName;
			public ListType _lt;
			public CBListTypeItem(string DisplayName, ListType lt)
			{
				_DisplayName = DisplayName;
				_lt = lt;
			}
			public override string ToString()
			{
				return _DisplayName;
			}
		}





	}
}
