using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AP_Extract
{
	public partial class DataForm : Form
	{
		public ListView _LV;
		public bool _bAddItem;

		enum Mode {Add, Edit, EditMultiple};
		private Mode _mode;

		public DataForm()
		{
			InitializeComponent();
		}

		private void DataForm_Load(object sender, EventArgs e)
		{
			CBType.Items.Add(ExtractUtils.c_TypeIniEntry);
			CBType.Items.Add("");

			if (_bAddItem || _LV.SelectedItems.Count < 1)
			{
				_mode = Mode.Add;
				this.Text = "Add New Item";
				SetCBItem(CBType, ExtractUtils.c_TypeIniEntry);
				EBData1.Text = "";
				EBData2.Text = "";
			}
			else if (_LV.SelectedItems.Count == 1)
			{
				_mode = Mode.Edit;
				this.Text = "Edit Item";
				SetCBItem(CBType, _LV.SelectedItems[0].Text);
				EBData1.Text = _LV.SelectedItems[0].SubItems[1].Text;
				EBData2.Text = _LV.SelectedItems[0].SubItems[2].Text;
			}
			else if (_LV.SelectedItems.Count > 1)
			{
				_mode = Mode.EditMultiple;
				this.Text = "Edit Multiple Items";
				SetCBItem(CBType, "");
				EBData1.Text = "";
				EBData2.Text = "";
			}

			SetEnables();
		}

		//public void Init()
		//{
		//    CBType.Items.Add(ExtractUtils.c_TypeIniEntry);
		//    CBType.Items.Add("");
		//}
		public void SetEnables()
		{
			lblData1.Text = "Section";
			lblData2.Text = "Item";
			if (EBData1.Text.Length > 0 && EBData2.Text.Length > 0)
			{
				BTOK.Enabled = true;
			}
			else
			{
				BTOK.Enabled = false;
			}
		}

		private void BTOK_Click(object sender, EventArgs e)
		{
			string c0 = CBType.Text;
			string c1 = EBData1.Text;
			string c2 = EBData2.Text;
			ListViewItem LI = null;
			if (_mode == Mode.Add)
			{
				LI = _LV.Items.Add(c0);
				LI.SubItems.Add(c1);
				LI.SubItems.Add(c2);
				LI.SubItems.Add("");
				LI.SubItems.Add("");
				LI = null;
			}
			else if (_mode == Mode.Edit)
			{
				_LV.SelectedItems[0].Text = c0;
				_LV.SelectedItems[0].SubItems[1].Text = c1;
				_LV.SelectedItems[0].SubItems[2].Text = c2;
			}
			else
			{
				int i = 0;
				for (i = 0; i < _LV.SelectedItems.Count; i++)
				{
					LI = _LV.SelectedItems[i];
					LI.Text = c0;
					LI.SubItems[1].Text = c1;
					LI.SubItems[2].Text = c2;
					LI = null;
				}
			}
			this.Close();
		}

		private void BTCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void SetCBItem(ComboBox CB, string Find)
		{
			int i = 0;
			for (i = 0; i < CB.Items.Count; i++)
			{
				if (string.Compare(CB.Items[i].ToString(), Find, true) == 0)
				{
					CB.SelectedIndex = i;
					return;
				}
			}
		}

		private void CBType_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetEnables();
		}

		private void EBData1_TextChanged(object sender, EventArgs e)
		{
			SetEnables();
		}

		private void EBData2_TextChanged(object sender, EventArgs e)
		{
			SetEnables();
		}


	}
}
