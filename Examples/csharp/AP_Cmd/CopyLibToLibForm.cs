using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Interop.AdeptCAC;
using Synergis.Adept.MainApi;

namespace AP_Cmd
{
	public partial class CopyLibToLibForm : Form
	{
		public NxProject _Project = null;
		public NxDetailedList _DL = null;

		public CopyLibToLibForm()
		{
			InitializeComponent();
		}

		private void CopyLibToLibForm_Load(object sender, EventArgs e)
		{
			//LPCMain._Project = _Project; // too late for Load()
			LPCMain.MyLoad(_Project);

			int cnt = _DL.GetCount();
			LBLIncoming.Text = string.Format("{0} items selected", cnt.ToString());
			if (cnt < 1)
				BTOK.Enabled = false;
		}

		private void BTOK_Click(object sender, EventArgs e)
		{
			string Name = "";
			string Id = "";
			LPCMain.GetSelected(ref Name, ref Id);
			if (string.IsNullOrEmpty(Name))
			{
				MessageBox.Show("Pick a destination library.");
				return;
			}
			NxSelectionList sl = _Project.NewSelectionList();
			foreach (NxDetailedItem di in _DL)
			{
				sl.AddFromInfo(di.SourceTableNumber, di.FileId, di.MajRev, di.MinRev);
			}
			int f2 = (int)  ApiTypes.ADEPTT_FROM_TO.FT_LIB_TO_LIB;
			int bCopyLibCard = CHCopyLibCard.Checked ? 1 : 0;
			int OverwriteDestinationFile = (int)  ApiTypes.ADEPTT_QDO.QDO_UNKNOWN;
			NxCommandSettings cs = _Project.Login.Core.CreateCommandSettings();
			_Project.CommandsSL.Copy(sl, f2, Id, bCopyLibCard, ref OverwriteDestinationFile, cs);
		}
	}
}
