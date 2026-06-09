using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Interop.AdeptCAC;

namespace ACACTest1
{
	public partial class ColumnPicker : Form
	{
		public NxProject _Project = null;

		public List<string> _Cols = new List<string>();

		public int _LastUserCol = 0;

		//public List<string> _Required = new List<string>();

		public ColumnPicker()
		{
			InitializeComponent();
		}

		private void ColumnPicker_Load(object sender, EventArgs e)
		{
			//_Required.Add("SCHEMA_S_LONGNAME");
			//_Required.Add("SCHEMA_S_SRCDB");
			//_Required.Add("SCHEMA_S_FILEID");
			//_Required.Add("SCHEMA_S_MAJREV");
			//_Required.Add("SCHEMA_S_MINREV");

			LVIn.Columns.Add("Schema Name");
			LVAll.Columns.Add("Schema Name");

			NxSchemaList sl = _Project.SchemaManager.SchemaList;
			NxSchemaItem si = null;
			ListViewItem lvi = null;

			foreach (string sn in _Cols)
			{
				si = sl.FindId(sn);
				if (si != null)
				{
					lvi = LVIn.Items.Add(si.DisplayName);
					lvi.Tag = sn;
				}
			}

			foreach (NxSchemaItem siall in _Project.SchemaManager.SchemaList)
			{
				lvi = LVAll.Items.Add(siall.DisplayName);
				lvi.Tag = siall.Id;
			}
		}

		private void BTAdd_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem lvi in LVAll.SelectedItems)
			{
				ListViewItem lvinew = LVIn.Items.Add(lvi.Text);
				lvinew.Tag = lvi.Tag;
			}
		}

		private void BTRemove_Click(object sender, EventArgs e)
		{
			for(int i = LVIn.Items.Count - 1; i > -1; i--) 
			{
				if(LVIn.Items[i].Selected == true)
				{
					LVIn.Items.RemoveAt(i);
				}
			}
		}

		private void BTOK_Click(object sender, EventArgs e)
		{
			_Cols.Clear();
			foreach (ListViewItem lvi in LVIn.Items)
			{
				_Cols.Add((string)lvi.Tag);
			}
			_LastUserCol = _Cols.Count;
			//foreach (string req in _Required)
			//{
			//    bool bFound = false;
			//    foreach (string col in _Cols)
			//    {
			//        if (string.Compare(req, col, true) == 0)
			//        {
			//            bFound = true;
			//            break;
			//        }
			//    }
			//    if (!bFound)
			//        _Cols.Add(req);
			//}
		}
	}
}
