using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Interop.AdeptCAC;

namespace AP_Cmd
{
	public partial class CmdForm : Form
	{
		public NxProject _Project = null;
		public NxDetailedList _DL = null;

		public CmdForm()
		{
			InitializeComponent();
		}

		private void CmdForm_Load(object sender, EventArgs e)
		{

		}

		private void BTCopyLibToLib_Click(object sender, EventArgs e)
		{
			CopyLibToLibForm l2l = new CopyLibToLibForm();
			l2l._Project = _Project;
			l2l._DL = _DL;
			l2l.ShowDialog();
		}
	}
}
