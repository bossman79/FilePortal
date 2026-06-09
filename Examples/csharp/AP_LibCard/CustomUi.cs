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

namespace AP_LibCard
{
	public partial class CustomUi : Form
	{
		public Interop.AdeptCAC.NxProject _Project;
		public Interop.AdeptGui.GuiApi _GuiApi;
		public string _FieldName; 
		public string _NewValue;
		public bool _bHasNewValue;
		public bool _bRefreshLibCard;

		public CustomUi()
		{
			InitializeComponent();
		}

		private void CustomUi_Load(object sender, EventArgs e)
		{
			TBFieldName.Text = _FieldName;
			TBNewValue.Text = _NewValue;

			foreach (NxSchemaItem SchemaItem in _Project.SchemaManager.SchemaList)
			{
				NxFldDef fd = SchemaItem.BaseFieldItem.FldDef;
				if(fd.bIsSystem != 1)
					CBOtherField.Items.Add(fd.Name);
			}
		}

		private void BTApply_Click(object sender, EventArgs e)
		{
			_GuiApi.SetLibraryCardFieldValue(CBOtherField.Text, TBOtherValue.Text);
			_bRefreshLibCard = true;
		}

		private void BTOK_Click(object sender, EventArgs e)
		{
			_NewValue = TBNewValue.Text;
			_bHasNewValue = true;
		}

		private void BTCancel_Click(object sender, EventArgs e)
		{
			_bHasNewValue = false;
		}
	}
}
