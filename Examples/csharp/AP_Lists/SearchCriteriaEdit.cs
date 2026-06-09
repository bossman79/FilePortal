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
using Synergis.Adept.MainApi;

namespace AP_Lists
{
	public partial class SearchCriteriaEdit : Form
	{
		public NxProject _Project;
		public   ApiTypes.ADEPTT_SEARCHOP _SearchOp = ApiTypes.ADEPTT_SEARCHOP.OP_STARTS;
		public string _Value = "";
		public string _Value2 = "";

		public SearchCriteriaEdit()
		{
			InitializeComponent();
		}

		private void SearchCriteriaEdit_Load(object sender, EventArgs e)
		{
			UIHelper.FillComboWithSearchOp(_Project, CBSearchOp, _SearchOp);
			TBValue.Text = _Value;
			TBValue2.Text = _Value2;

			DoEnables();
		}

		private void BTOK_Click(object sender, EventArgs e)
		{
			_SearchOp = ((CBItem_SearchOp)CBSearchOp.SelectedItem)._SearchOp;
			_Value = TBValue.Text;
			_Value2 = TBValue2.Text;
		}

		private void DoEnables()
		{
			bool bEnableValue2 = false;
			CBItem_SearchOp cbiSearchOp = (CBItem_SearchOp) CBSearchOp.SelectedItem;
			if (cbiSearchOp != null)
			{
				if (cbiSearchOp._SearchOp == ApiTypes.ADEPTT_SEARCHOP.OP_DATERANGE)
				{
					bEnableValue2 = true;
				}
			}
			TBValue2.Enabled = bEnableValue2;
		}

		private void CBSearchOp_SelectedIndexChanged(object sender, EventArgs e)
		{
			DoEnables();
		}



	}
}
