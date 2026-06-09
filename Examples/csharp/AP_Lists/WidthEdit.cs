using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AP_Lists
{
	public partial class WidthEdit : Form
	{
		public int _Width = 0;

		public WidthEdit()
		{
			InitializeComponent();
		}

		private void WidthEdit_Load(object sender, EventArgs e)
		{
			NUD1.Value = _Width;
		}

		private void BTOK_Click(object sender, EventArgs e)
		{
			_Width = System.Convert.ToInt32(NUD1.Value);
		}
	}
}
