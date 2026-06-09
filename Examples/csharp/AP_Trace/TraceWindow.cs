using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Synergis.Adept.MainApi;

namespace AP_Trace
{
	public partial class TraceWindow : Form
	{
		public string m_PlugInName;

		public TraceWindow()
		{
			InitializeComponent();
		}

		public void AddLine(string Txt, int Tabs = 0)
		{
			string msg = null;
			int tabi = 0;
			for (tabi = 1; tabi <= Tabs; tabi++) {
				msg = msg + "\t";
			}
			msg = msg + Txt + "\r\n";
			TextBox1.AppendText(msg);
		}

		private void TraceWindow_Load(object sender, EventArgs e)
		{
			WindowPositionHelper.ApplySize(m_PlugInName, this, null);
			CH_TraceSelChg.Checked = WindowPositionHelper.GetSetting(m_PlugInName, this, null, "TraceSelChg", false);
			CH_TraceLibCardChg.Checked = WindowPositionHelper.GetSetting(m_PlugInName, this, null, "TraceLibCardChg", false);
			TraceWindow_Resize(null, null);
		}

		private void TraceWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			WindowPositionHelper.SaveSize(m_PlugInName, this, null);
			WindowPositionHelper.SaveSetting(m_PlugInName, this, null, "TraceSelChg", CH_TraceSelChg.Checked);
			WindowPositionHelper.SaveSetting(m_PlugInName, this, null, "TraceLibCardChg", CH_TraceLibCardChg.Checked);

			// Cancel the Closing event from closing the form.
			e.Cancel = true;
			this.Hide();
		}

		private void TraceWindow_Resize(object sender, EventArgs e)
		{
			TextBox1.Left = 10;
			TextBox1.Top = 40;
			TextBox1.Width = this.ClientSize.Width - 20;
			TextBox1.Height = this.ClientSize.Height - 10 - CH_TraceSelChg.Top - CH_TraceSelChg.Height;
		}




	}
}
