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
	public partial class ContainerEdit : Form
	{
		public bool _bAdd = false;
		public string _Name = "";
		public string _Comment = "";

		public ContainerEdit()
		{
			InitializeComponent();
		}

		private void ListEdit_Load(object sender, EventArgs e)
		{
			if (_bAdd)
				this.Text = "Add Container";
			else
				this.Text = "Edit Container";

			TBName.Text = _Name;
			TBComment.Text = _Comment;

			DoEnables();
		}

		void DoEnables()
		{
			BTOK.Enabled = TBName.Text.Length > 0;
		}

		private void BTOK_Click(object sender, EventArgs e)
		{
			_Name = TBName.Text;
			_Comment = TBComment.Text;
		}

		private void TBName_TextChanged(object sender, EventArgs e)
		{
			DoEnables();
		}

		private void TBComment_TextChanged(object sender, EventArgs e)
		{
			DoEnables();
		}
	}
}
