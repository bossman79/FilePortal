using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AP_LibCard
{
	public partial class LibCardConfig : Form
	{

		public Dictionary<string, string> _FieldsToDisallowDictionary = null;
		public Dictionary<string, string> _FieldsToCustomUiDictionary = null;
        public string _LoseFocus = "";
        public string _LoseFocusUpdate = "";

		public LibCardConfig()
		{
			InitializeComponent();
		}

		private void LibCardConfig_Load(object sender, EventArgs e)
		{
			foreach (string tmp in _FieldsToDisallowDictionary.Values)
			{
				TBFieldsToDisallow.AppendText(tmp + "\n");
			}

			foreach (string tmp in _FieldsToCustomUiDictionary.Values)
			{
				TBFieldsToCustomUi.AppendText(tmp + "\n");
			}

            TBLoseFocus.Text = _LoseFocus;
            TBLoseFocusUpdate.Text = _LoseFocusUpdate;
		}

		private void BTOK_Click(object sender, EventArgs e)
		{
			_FieldsToDisallowDictionary.Clear();
			_FieldsToCustomUiDictionary.Clear();

			foreach (string line in TBFieldsToDisallow.Lines)
			{
				if(!string.IsNullOrEmpty(line))
					_FieldsToDisallowDictionary.Add(line.ToUpper(), line);
			}

			foreach (string line in TBFieldsToCustomUi.Lines)
			{
				if (!string.IsNullOrEmpty(line))
					_FieldsToCustomUiDictionary.Add(line.ToUpper(), line);
			}

            _LoseFocus = TBLoseFocus.Text;
            _LoseFocusUpdate = TBLoseFocusUpdate.Text;
		}
	}
}
