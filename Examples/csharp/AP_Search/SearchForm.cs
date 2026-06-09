using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

using Interop.AdeptCAC;
using Interop.AdeptGui;
using Synergis.Adept.MainApi;

namespace AP_Search
{
	public partial class SearchForm : Form
	{
		public GuiApi _GuiApi = null;
		public NxProject _Project = null;
        public NxTbl _WindowTbl = null;

		NxTbl _Tbl = null;
        NxViewContext _ViewContext = null;

        string _ColSetId = "";

        bool _bUseStandardScrolling = false; // ShowTable can be done different ways. Xml Scroll is faster when out of process.

		public SearchForm()
		{
			InitializeComponent();
		}

		private void SearchForm_Load(object sender, EventArgs e)
		{
            _Project.ContainerManager.LibrarySet_ContainerList.Refresh();
            int LibrarySetIndex = -1;
            string LibrarySetId = _Project.GetCurrentLibrarySet();
            int LibrarySetCount = _Project.ContainerManager.LibrarySet_ContainerList.GetCount();
            for (int i = 0; i < LibrarySetCount; i++)
            {
                NxContainerItem ci = _Project.ContainerManager.LibrarySet_ContainerList.GetItem(i);
                LibrarySetIndex = CBLibrarySet.Items.Add(new CBItem_NameId(ci.Name, ci.Id));
               
                try
                {
                    if (!string.IsNullOrEmpty(LibrarySetId) && string.Compare(ci.Id, LibrarySetId, true) == 0)
                        CBLibrarySet.SelectedIndex = LibrarySetIndex;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception setting default Library Set: {0}", ex.Message);
                }

            }
            LibrarySetIndex = CBLibrarySet.Items.Add(new CBItem_NameId("<None>", Guid.Empty.ToString()));
            try
            {
                if (string.IsNullOrEmpty(LibrarySetId))
                    CBLibrarySet.SelectedIndex = LibrarySetIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception setting default Library Set: {0}", ex.Message);
            }
  
            

            LVMain.Columns.Add("File Name");
            LVMain.Columns.Add("Status");
            LVMain.Columns.Add("Lib Name");
            LVMain.Columns.Add("Work Note");
            //LVMain.Columns.Add("Work Note 2");

			RBNewSearch.Checked = true;
			RBSearchAnd.Checked = true;

            // Find our column set
            NxContainerItem MyColumns = _Project.ContainerManager.ColumnSet_ContainerList.FindName("AP_Search Columns");
            if (MyColumns == null)
            {
                // Or make it now
                MyColumns = _Project.ContainerManager.ColumnSet_ContainerList.Add("AP_Search Columns", "AP_Search Columns Defaults");
            }
            // clear existing columns
            MyColumns.ContentsList.Clear();
            // add our columns
            NxContentsItem MyCol1 = MyColumns.ContentsList.Add(); MyCol1.ObjId = "SCHEMA_S_LONGNAME"; MyCol1.Width = 50;
            NxContentsItem MyCol2 = MyColumns.ContentsList.Add(); MyCol2.ObjId = "SCHEMA_S_STATUS"; MyCol2.Width = 10;
            NxContentsItem MyCol3 = MyColumns.ContentsList.Add(); MyCol3.ObjId = "SCHEMA_S_LIBNAME"; MyCol3.Width = 40;
            NxContentsItem MyCol4 = MyColumns.ContentsList.Add(); MyCol4.ObjId = "SCHEMA_G_WRKNOTE"; MyCol4.Width = 50;
            // update the columns
            MyColumns.UpdateContentsList();
            _ColSetId = MyColumns.Id;

            // Find our view context
            _ViewContext = _Project.ViewContextManager.ViewContextList.FindName("AP_Search");
            if (_ViewContext == null)
            {
                // Or make it now
                _ViewContext = _Project.ViewContextManager.ViewContextList.Add();
                _ViewContext.Name = "AP_Search";
                _ViewContext.TableName = "MySearch";
                _ViewContext.ColumnSetId = _ColSetId; // HOOK UP COLUMNS TO VIEW CONTEXT
            }

			//LoadSchema();
			UIHelper.FillComboWithSchema(_Project, ApiTypes.NFM_DB_NUM.C_FILES, CBSchemaA, "SCHEMA_S_LONGNAME");
			UIHelper.FillComboWithSchema(_Project, ApiTypes.NFM_DB_NUM.C_FILES, CBSchemaB, "");
			UIHelper.FillComboWithSchema(_Project, ApiTypes.NFM_DB_NUM.C_FILES, CBSchemaC, "");
			
			//LoadSearchOp();
			UIHelper.FillComboWithSearchOp(_Project, CBSearchOpA, ApiTypes.ADEPTT_SEARCHOP.OP_STARTS);
			UIHelper.FillComboWithSearchOp(_Project, CBSearchOpB, ApiTypes.ADEPTT_SEARCHOP.OP_STARTS);
			UIHelper.FillComboWithSearchOp(_Project, CBSearchOpC, ApiTypes.ADEPTT_SEARCHOP.OP_STARTS);

            // No UI Helper for ColumnSets, so implement it ourselves
            FIllComboWithColumnSets(_Project, CBColumnSet, MyColumns.Id);

            FillComboWithSavedSearchCriteria(_Project, CBSavedSearchCriteria);

			EnableControls();
		}

        void FIllComboWithColumnSets(NxProject Project, ComboBox cb, string DefaultColumnSetId)
        {
            try
            {
                foreach (NxContainerItem Container in Project.ContainerManager.ColumnSet_ContainerList)
                {
                    int ndx = cb.Items.Add(new CBItem_NameId(Container.Name, Container.Id));
                    if (string.Compare(Container.Id, DefaultColumnSetId) == 0)
                        cb.SelectedIndex = ndx;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Exception loading column sets. {0}", ex.Message));
            }
        }

        void FillComboWithSavedSearchCriteria(NxProject Project, ComboBox cb)
        {
            try
            {
                foreach (NxContainerItem Container in Project.ContainerManager.SavedSearchCriteria_ContainerList)
                {
                    cb.Items.Add(new CBItem_NameId(Container.Name, Container.Id));
                }
                if (cb.Items.Count > 0)
                    cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Exception loading saved search criteria. {0}", ex.Message));
            }
        }

        #region Stale example code on loading combos. Replaced by UIHelper

        //private void LoadSchema()
		//{
		//    NxSchemaList SchemaList = _Project.SchemaManager.SchemaList;
		//    NxSchemaItem SchemaItem = null;
		//    CBItem_Schema cbItem = null;
		//    int Count = SchemaList.GetCount();
		//    for (int i = 0; i < Count; i++)
		//    {
		//        SchemaItem = SchemaList.GetItem(i);
		//        cbItem = new CBItem_Schema(SchemaItem.DisplayName, SchemaItem.Id);
		//        CBSchemaA.Items.Add(cbItem);
		//        CBSchemaB.Items.Add(cbItem);
		//        CBSchemaC.Items.Add(cbItem);
        //
		//        if (string.Compare(SchemaItem.Id, "SCHEMA_S_LONGNAME", true) == 0)
		//        {
		//            CBSchemaA.SelectedItem = cbItem;
		//        }
		//    }
		//}
        //
		//private void LoadSearchOp()
		//{
		//    LoadOneSearchOp(new CBItem_SearchOp("None", ApiTypes.ADEPTT_SEARCHOP.OP_NONE));
		//    LoadOneSearchOp(new CBItem_SearchOp("Starts With", ApiTypes.ADEPTT_SEARCHOP.OP_STARTS));
		//    LoadOneSearchOp(new CBItem_SearchOp("Equals", ApiTypes.ADEPTT_SEARCHOP.OP_EQUALS));
		//    LoadOneSearchOp(new CBItem_SearchOp("Does Not Start With", ApiTypes.ADEPTT_SEARCHOP.OP_DOESNOTSTART));
		//    LoadOneSearchOp(new CBItem_SearchOp("Contains", ApiTypes.ADEPTT_SEARCHOP.OP_CONTAINS));
		//    LoadOneSearchOp(new CBItem_SearchOp("Does Not Contail", ApiTypes.ADEPTT_SEARCHOP.OP_DOESNOTCONTAIN));
		//    LoadOneSearchOp(new CBItem_SearchOp("Less Than", ApiTypes.ADEPTT_SEARCHOP.OP_LESSTHAN));
		//    LoadOneSearchOp(new CBItem_SearchOp("Less Than Or Equal", ApiTypes.ADEPTT_SEARCHOP.OP_LESSTHANEQUAL));
		//    LoadOneSearchOp(new CBItem_SearchOp("Greater Than", ApiTypes.ADEPTT_SEARCHOP.OP_GREATERTHAN));
		//    LoadOneSearchOp(new CBItem_SearchOp("Greater Than Or Equal", ApiTypes.ADEPTT_SEARCHOP.OP_GREATERTHANEQUAL));
		//    LoadOneSearchOp(new CBItem_SearchOp("Is Empty", ApiTypes.ADEPTT_SEARCHOP.OP_EMPTY));
		//    LoadOneSearchOp(new CBItem_SearchOp("Is Not Empty", ApiTypes.ADEPTT_SEARCHOP.OP_NOTEMPTY));
		//    LoadOneSearchOp(new CBItem_SearchOp("Fuzzy", ApiTypes.ADEPTT_SEARCHOP.OP_FUZZY));
		//    LoadOneSearchOp(new CBItem_SearchOp("Date Range", ApiTypes.ADEPTT_SEARCHOP.OP_DATERANGE));
		//    LoadOneSearchOp(new CBItem_SearchOp("After Date", ApiTypes.ADEPTT_SEARCHOP.OP_AFTER_DATE));
		//    LoadOneSearchOp(new CBItem_SearchOp("Before Date", ApiTypes.ADEPTT_SEARCHOP.OP_BEFORE_DATE));
		//    LoadOneSearchOp(new CBItem_SearchOp("On Date", ApiTypes.ADEPTT_SEARCHOP.OP_ON_DATE));
		//}
        //
		//private void LoadOneSearchOp(CBItem_SearchOp cbItem)
		//{
		//    CBSearchOpA.Items.Add(cbItem);
		//    CBSearchOpB.Items.Add(cbItem);
		//    CBSearchOpC.Items.Add(cbItem);
		//    if (cbItem._SearchOp == ApiTypes.ADEPTT_SEARCHOP.OP_STARTS)
		//    {
		//        CBSearchOpA.SelectedItem = cbItem;
		//        CBSearchOpB.SelectedItem = cbItem;
		//        CBSearchOpC.SelectedItem = cbItem;
		//    }
        //}

        #endregion

        private void EnableControls()
		{
			bool bHasResults = _Tbl != null;
			if (!bHasResults)
			{
				RBNewSearch.Checked = true;
			}
			RBAppendSearch.Enabled = bHasResults;
			RBRefineSearch.Enabled = bHasResults;
			BTGiveTable.Enabled = bHasResults;
		}

		private void BTSearch_Click(object sender, EventArgs e)
		{
			LVMain.Items.Clear();

			NxSchemaList SchemaList = _Project.SchemaManager.SchemaList;

			NxQueryTable QueryTable = _Project.CreateQueryTable();
			QueryTable.TableName = "fm100fil";
			NxQueryFieldList QueryFieldList = QueryTable.QueryFieldList;
			NxQueryFieldItem QueryFieldItem = null;

			CBItem_Schema cbiSchema = (CBItem_Schema)CBSchemaA.SelectedItem;
			CBItem_SearchOp cbiSearchOp = (CBItem_SearchOp)CBSearchOpA.SelectedItem;
			QueryFieldItem = QueryFieldList.Add(cbiSchema._Id);
			QueryFieldItem.SearchOp = (int)cbiSearchOp._SearchOp;
			QueryFieldItem.Value = TBValue1A.Text;
			QueryFieldItem.Value2 = TBValue2A.Text;

			if (CBSchemaB.SelectedIndex > -1)
			{
				cbiSchema = (CBItem_Schema)CBSchemaB.SelectedItem;
				cbiSearchOp = (CBItem_SearchOp)CBSearchOpB.SelectedItem;
				QueryFieldItem = QueryFieldList.Add(cbiSchema._Id);
				QueryFieldItem.SearchOp = (int)cbiSearchOp._SearchOp;
				QueryFieldItem.Value = TBValue1B.Text;
				QueryFieldItem.Value2 = TBValue2B.Text;

				if (CBSchemaC.SelectedIndex > -1)
				{
					cbiSchema = (CBItem_Schema)CBSchemaC.SelectedItem;
					cbiSearchOp = (CBItem_SearchOp)CBSearchOpC.SelectedItem;
					QueryFieldItem = QueryFieldList.Add(cbiSchema._Id);
					QueryFieldItem.SearchOp = (int)cbiSearchOp._SearchOp;
					QueryFieldItem.Value = TBValue1C.Text;
					QueryFieldItem.Value2 = TBValue2C.Text;
				}
			}

			int bOrSearch = 0;
			if (RBSearchOr.Checked == true)
				bOrSearch = 1;

            // QueryTable upgrades with ViewContext
            CBItem_NameId cbItem = (CBItem_NameId)CBColumnSet.SelectedItem;            
            _ViewContext.ColumnSetId =  cbItem._Id;
            _ViewContext.SortColumnSchemaId = "SCHEMA_S_LONGNAME";
            QueryTable.ViewContextId = _ViewContext.Id;
            QueryTable.bOrSearch = bOrSearch;

            // TEST
            //_ViewContext.SortColumnSchemaId = "SCHEMA_S_LIBNAME";

			if (RBNewSearch.Checked == true)
			{
                // QueryTable upgrades with ViewContext
				//_Tbl = QueryTable.DoSearchEx(bOrSearch, "MySearch");
                _Tbl = QueryTable.VcDoSearch();
			}
			else
			{
                if (RBAppendSearch.Checked == true)
                {
                    // QueryTable upgrades with ViewContext
                    //_Tbl = QueryTable.DoSearchAppend("MySearch", bOrSearch, "MySearch");
                    _Tbl = QueryTable.VcDoSearchAppend("MySearch");
                }
                else
                {
                    // QueryTable upgrades with ViewContext
                    //_Tbl = QueryTable.DoSearchRefine("MySearch", bOrSearch, "MySearch");
                    _Tbl = QueryTable.VcDoSearchRefine("MySearch");
                }
			}

            ShowTable();
			
		}

        
        void ShowTable()
        {
            LVMain.Items.Clear();
            LVMain.Columns.Clear();

            if (_Tbl == null)
            {
                LVMain.Columns.Add("Message.");
                LVMain.Items.Add("No Table Returned.");
                return;
            }
            int SrvCount = _Tbl.SrvGetCount();
            if (SrvCount < 1)
            {
                LVMain.Columns.Add("Message.");
                LVMain.Items.Add("Empty Table Returned.");
                _Tbl = null;
                return;
            }

            NxContainerItem Columns = null;
            string ViewContextId = _Tbl.ViewContextId;
            // work down to the columns
            if (!string.IsNullOrEmpty(ViewContextId))
            {
                NxViewContext vc = _Project.ViewContextManager.ViewContextList.FindId(ViewContextId);
                if (vc != null)
                {
                    string ColumnSetId = vc.ColumnSetId;
                    if (!string.IsNullOrEmpty(ColumnSetId))
                    {
                        NxContainerItem ci = _Project.ContainerManager.ColumnSet_ContainerList.FindId(ColumnSetId);
                        if (ci != null)
                        {
                            if (ci.ContentsList.GetCount() > 0)
                                Columns = ci; // there is something to work with
                        }
                    }
                }
            }

            // if we don't have columns, use defaults
            if (Columns == null)
            {
                LVMain.Columns.Add("File Name");
                LVMain.Columns.Add("Status");
                LVMain.Columns.Add("Lib Name");
            }
            else
            {
                foreach (NxContentsItem cnt in Columns.ContentsList)
                {
                    string SchemaId = cnt.ObjId;
                    NxSchemaItem si = _Project.SchemaManager.SchemaList.FindId(SchemaId);
                    if (si != null)
                        LVMain.Columns.Add(si.DisplayName);
                }
            }

            if(_bUseStandardScrolling)
                ShowTableStd(Columns);
            else
                ShowTableXmlScroll(Columns);
        }


        // This pages through the results and puts everything in the window.
        // If implementing Virtual Scrolling, Scroll to the current record and only show _Tbl.GetCount items (without any paging).
        void ShowTableStd(NxContainerItem Columns)
        {
            int SrvCount = _Tbl.SrvGetCount();

            int i = 1;
            string FileNE = "";
            while (i <= _Tbl.GetCount())
            {
                _Tbl.GotoRecord(i);

                //FileNE = _Tbl.GetDisplayStringFromSchemaId("SCHEMA_S_LONGNAME");
                //ListViewItem lvi = LVMain.Items.Add(FileNE);
                //lvi.SubItems.Add(_Tbl.GetDisplayStringFromSchemaId("SCHEMA_S_STATUS"));
                //lvi.SubItems.Add(_Tbl.GetDisplayStringFromSchemaId("SCHEMA_S_LIBNAME"));
                //lvi.SubItems.Add(_Tbl.GetDisplayStringFromSchemaId("SCHEMA_G_WRKNOTE"));
                ////lvi.SubItems.Add(_Tbl.GetDisplayStringFromSchemaId("SCHEMA_G_WRKNOT2"));

                ListViewItem lvi = null;

                if (Columns == null)
                {
                    FileNE = _Tbl.GetDisplayStringFromSchemaId("SCHEMA_S_LONGNAME");
                    lvi = LVMain.Items.Add(FileNE);
                    lvi.SubItems.Add(_Tbl.GetDisplayStringFromSchemaId("SCHEMA_S_STATUS"));
                    lvi.SubItems.Add(_Tbl.GetDisplayStringFromSchemaId("SCHEMA_S_LIBNAME"));
                }
                else
                {
                    // just for troubleshooting
                    FileNE = _Tbl.GetDisplayStringFromSchemaId("SCHEMA_S_LONGNAME");

                    int colcount = 0;
                    foreach (NxContentsItem cnt in Columns.ContentsList)
                    {
                        string SchemaId = cnt.ObjId;
                        NxSchemaItem si = _Project.SchemaManager.SchemaList.FindId(SchemaId);
                        if (si != null)
                        {
                            // if first column
                            if(colcount < 1)
                            {
                                // add to list
                                lvi = LVMain.Items.Add(_Tbl.GetDisplayStringFromSchemaId(si.Id));
                                colcount++;
                            }
                            else
                            {
                                // add column
                                lvi.SubItems.Add(_Tbl.GetDisplayStringFromSchemaId(si.Id));
                            }
                        }
                    }
                }

                // Page to the next screenful.
                if (_Tbl.GetCurrentRecordNumber() == _Tbl.GetCount())
                {
                    int NextRec = _Tbl.SrvGetLastRecordNumber() + 1;
                    if (NextRec > SrvCount)
                        break;

                    //LVMain.Items.Add("----------"); // Show Paging

                    _Tbl.SrvScroll(NextRec, 20);
                    i = 1;
                }
                else
                {
                    i++;
                }
            }

            EnableControls();
        }

        // This will get all records and read the xml.
        void ShowTableXmlScroll(NxContainerItem Columns)
        {
            string xml = _Tbl.VcSrvXMLScroll(1, -1, 1, "ADDBASE"); // force refresh

            string FileNE = "";

            XDocument xdoc = XDocument.Parse(xml);
            foreach (XElement Main in xdoc.Descendants("XMLSrvScroll"))
            {
                foreach (XElement row in Main.Descendants("row"))
                {
                    //string Field = Column.Element("Field").Value;
                    //string Desc = Column.Element("Desc").Value;
                    //string Type = Column.Element("Type").Value;

                    ListViewItem lvi = null;

                    if (Columns == null)
                    {
                        FileNE = row.Element("S_LONGNAME").Value;
                        lvi = LVMain.Items.Add(FileNE);
                        lvi.SubItems.Add(row.Element("S_STATUS").Value);
                        lvi.SubItems.Add(row.Element("S_LIBNAME").Value);
                    }
                    else
                    {
                        // just for troubleshooting
                        FileNE = row.Element("S_LONGNAME").Value;

                        int colcount = 0;
                        foreach (NxContentsItem cnt in Columns.ContentsList)
                        {
                            string SchemaId = cnt.ObjId;
                            NxSchemaItem si = _Project.SchemaManager.SchemaList.FindId(SchemaId);
                            if (si != null)
                            {
                                string uname = si.UniqueName;
                                // if first column
                                if (colcount < 1)
                                {
                                    // add to list
                                    lvi = LVMain.Items.Add(row.Element(uname).Value);
                                    colcount++;
                                }
                                else
                                {
                                    // add column
                                    lvi.SubItems.Add(row.Element(uname).Value);
                                }
                            }
                        }
                    }
                    
                }
            }
            
            EnableControls();
        }


		private void BTGiveTable_Click(object sender, EventArgs e)
		{
			if (_Tbl != null)
			{
				_Tbl.SrvScroll(1, 20);
				_GuiApi.GiveTableToSearchWindow(_Tbl, 0, "Test");
				_Tbl = null;
			}
			EnableControls();
		}

        // Test to merge tables using Append and Refine
        // No error checking implemented
        private void BTTest1_Click(object sender, EventArgs e)
        {
            // TABLE 1
            NxViewContext vc1 = _Project.ViewContextManager.ViewContextList.FindName("AP_SearchVc1");
            if(vc1 == null)
            {
                vc1 = _Project.ViewContextManager.ViewContextList.Add();
                vc1.Name = "AP_SearchVc1";
                vc1.TableName = "AP_Search1";
            }

            NxQueryTable QueryTable1 = _Project.CreateQueryTable();
            QueryTable1.TableName = "fm100fil";
            NxQueryFieldList QueryFieldList1 = QueryTable1.QueryFieldList;
            NxQueryFieldItem QueryFieldItem1 = QueryFieldList1.Add("SCHEMA_S_LONGNAME");
            QueryFieldItem1.SearchOp = (int)ApiTypes.ADEPTT_SEARCHOP.OP_STARTS;     // 1st Search - OP_STARTS with B (as in book and box)
            QueryFieldItem1.Value = "b";
            QueryTable1.ViewContextId = vc1.Id;

            NxTbl Tbl1 = QueryTable1.VcDoSearch();

            // TABLE 2
            NxViewContext vc2 = _Project.ViewContextManager.ViewContextList.FindName("AP_SearchVc2");
            if(vc2 == null)
            {
                vc2 = _Project.ViewContextManager.ViewContextList.Add();
                vc2.Name = "AP_SearchVc2";
                vc2.TableName = "AP_Search2";
            }

            NxQueryTable QueryTable2 = _Project.CreateQueryTable();
            QueryTable2.TableName = "fm100fil";
            NxQueryFieldList QueryFieldList2 = QueryTable2.QueryFieldList;
            NxQueryFieldItem QueryFieldItem2 = QueryFieldList2.Add("SCHEMA_S_LONGNAME");
            QueryFieldItem2.SearchOp = (int)ApiTypes.ADEPTT_SEARCHOP.OP_STARTS;         // FOR APPEND - OP_STARTS with C
            QueryFieldItem2.Value = "c";
            //QueryFieldItem2.SearchOp = (int)ApiTypes.ADEPTT_SEARCHOP.OP_CONTAINS;     // FOR REFINE - OP_CONTAINS ox (as in box, with the b coming from 1st search)
            //QueryFieldItem2.Value = "ox";
            QueryTable2.ViewContextId = vc2.Id;

            NxTbl Tbl2 = QueryTable2.VcDoSearch();

            // TABLE 3

            int Operation = 0; // append - FOR APPEND - Match with 'OP_STARTS with "c"' above
            //int Operation = 1; // refine - FOR REFINE - Match with 'OP_CONTAINS "ox"' above

            NxQueryTable QueryTable3 = _Project.CreateQueryTable();
            QueryTable3.TableName = "fm100fil";
            QueryTable3.ViewContextId = _ViewContext.Id;
            _Tbl = QueryTable3.VcUpdateScrollTable("AP_Search1", "AP_Search2", Operation);

            ShowTable();
        }

        // Take a UI window table and transform it into our own table.
        // ViewContext TableName should be a different name than the window table.
        // Also, we can switch the columns to what we need, and we can get back the whole table if we would like.
        private void BTTest2_Click(object sender, EventArgs e)
        {
            int CurrentCount = _WindowTbl.GetCount();
            string CurrentName = "*" + _WindowTbl.Name; // MUST ADD A * TO INFORM THE CORE THAT WE KNOW THAT THIS COULD BE A RESERVED NAME

            // Find our view context
            NxViewContext Test2_ViewContext = _Project.ViewContextManager.ViewContextList.FindName("AP_Search_Test2");
            if (Test2_ViewContext == null)
            {
                // Or make it now
                Test2_ViewContext = _Project.ViewContextManager.ViewContextList.Add();
                Test2_ViewContext.Name = "AP_Search_Test2";
                Test2_ViewContext.TableName = "SRCHTEST2";
                
                // Uncomment to get back all of the rows. 
                // ShowTable will do the paging if necessary anyway. (Since the list window doesn't implement virtual scrolling.)
                // This is just an example for getting back all rows.
                Test2_ViewContext.NumberOfLines = -1; 
            }

            Test2_ViewContext.ColumnSetId = _ColSetId; // may have changed since it is in a combo box now

            NxQueryTable Test2_QueryTable = _Project.CreateQueryTable();
            Test2_QueryTable.TableName = "fm100fil";
            Test2_QueryTable.ViewContextId = Test2_ViewContext.Id;

            NxQueryFieldList Test2_QueryFieldList = Test2_QueryTable.QueryFieldList;
            NxQueryFieldItem Test2_QueryFieldItem = Test2_QueryFieldList.Add("SCHEMA_S_LONGNAME");
            Test2_QueryFieldItem.SearchOp = (int)ApiTypes.ADEPTT_SEARCHOP.OP_NOTEMPTY;
            _Tbl = Test2_QueryTable.VcDoSearchRefine(CurrentName);

            //int Operation = 0; 
            //_Tbl = Test2_QueryTable.VcUpdateScrollTable(CurrentName, "", Operation);

            
            // pass this columns set back to the main view context
            _ViewContext.ColumnSetId = _ColSetId;

            ShowTable();

        }

        private void CBColumnSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            CBItem_NameId cbItem = (CBItem_NameId)CBColumnSet.SelectedItem;

            _ColSetId = cbItem._Id;

            _ViewContext.ColumnSetId = cbItem._Id;
            _ViewContext.SortColumnSchemaId = "SCHEMA_S_LONGNAME";

            // if no table yet, then done
            if (_Tbl == null)
                return;

            NxQueryTable QueryTable = _Project.CreateQueryTable();
            QueryTable.TableName = "fm100fil";
            QueryTable.ViewContextId = _ViewContext.Id;

            _Tbl.SrvScroll(1, 20);
            ShowTable();
        }

        private void BTSavedSearchCriteria_Click(object sender, EventArgs e)
        {
            CBItem_NameId cbItem = (CBItem_NameId)CBSavedSearchCriteria.SelectedItem;

            _Tbl = _Project.SearchCriteriaClick(cbItem._Id, _ViewContext.Id);
            ShowTable();
        }

        private void CBLibrarySet_SelectedIndexChanged(object sender, EventArgs e)
        {
            CBItem_NameId item = (CBItem_NameId)CBLibrarySet.SelectedItem;
            if(item != null)
            {
                string LibrarySetId = "";
                if(string.Compare(item._Id, Guid.Empty.ToString(), true) != 0)
                    LibrarySetId = item._Id;
                _Project.SetCurrentLibrarySet(LibrarySetId);
            }
        }

        private void LVMain_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column < -1)
                return;

            string SchemaId = "";
            ColumnHeader ch = LVMain.Columns[e.Column];
            string name = ch.Text;
            int count = _Project.SchemaManager.SchemaList.GetCount();
            for(int i =0; i < count; i++)
            {
                NxSchemaItem si = _Project.SchemaManager.SchemaList.GetItem(i);
                if(string.Compare(si.DisplayName, name) == 0)
                {
                    SchemaId = si.Id;
                }
            }

            if(!string.IsNullOrEmpty(SchemaId))
            {
                //_ViewContext.SortColumnSchemaId = SchemaId;
                _Tbl.ReSort(SchemaId, 1);
                //_Tbl.VcSrvXMLScroll(1, 20, 0, "");
                ShowTable();

            }
        }

        private void BTClearContainers_Click(object sender, EventArgs e)
        {
            _Project.ContainerManager.Refresh();
        }

        private void BTReScroll_Click(object sender, EventArgs e)
        {
            ShowTable();
        }
        

	}

    public class CBItem_NameId
    {
        public string _Name;
        public string _Id;
        public CBItem_NameId(string Name, string Id)
        {
            _Name = Name;
            _Id = Id;
        }
        public override string ToString()
        {
            return _Name;
        }
    }
}
