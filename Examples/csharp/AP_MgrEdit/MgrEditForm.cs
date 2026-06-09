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

namespace AP_MgrEdit
{
	public partial class MgrEditForm : Form
	{
		public NxProject _Project;

		public MgrEditForm()
		{
			InitializeComponent();
		}

		private void MgrEditForm_Load(object sender, EventArgs e)
		{
			TBValue.Text = DateTime.Now.ToString();
		}

		private void BTGo_Click(object sender, EventArgs e)
		{
			string FieldId = TBFieldId.Text;
			string Value = TBValue.Text;

			// make sure that the field exists before trying to go into edit mode
			NxFieldItem fi = _Project.FieldManager.FieldList.FindId(FieldId);
			if (fi == null)
			{
				MessageBox.Show("FieldId not found");
				return;
			}

			// go into edit mode
			string msg = "";
			int es = _Project.FieldManager.Edit(ref msg);
			if (es != 0 || !string.IsNullOrEmpty(msg))
			{
				MessageBox.Show("Error: " + es.ToString() + " -- " + msg);
				return;
			}

			// add the value
			es = fi.AddRestrictedValue(Value);
			if (es != 0)
			{
				MessageBox.Show("Error adding value: " + es.ToString());
				_Project.FieldManager.CancelEdit(); // on error, must cancel edit mode
				return;
			}

			// update
			es = _Project.FieldManager.Update();
			if (es != 0)
			{
				MessageBox.Show("Error adding value: " + es.ToString());
				_Project.FieldManager.CancelEdit(); // on error, must cancel edit mode
				return;
			}

		}
	}
}
