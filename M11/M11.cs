using System;
using System.Text;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Extensions;
using DBConnection;
using MDS00;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace M11
{
    public partial class M11 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Functionality.Function FUNC = new Functionality.Function();
        public M11()
        {
            InitializeComponent();
            UserLookAndFeel.Default.StyleChanged += MyStyleChanged;
            iniConfig = new IniFile("Config.ini");
            UserLookAndFeel.Default.SetSkinStyle(iniConfig.Read("SkinName", "DevExpress"), iniConfig.Read("SkinPalette", "DevExpress"));
        }

        private IniFile iniConfig;

        private void MyStyleChanged(object sender, EventArgs e)
        {
            UserLookAndFeel userLookAndFeel = (UserLookAndFeel)sender;
            LookAndFeelChangedEventArgs lookAndFeelChangedEventArgs = (DevExpress.LookAndFeel.LookAndFeelChangedEventArgs)e;
            //MessageBox.Show("MyStyleChanged: " + lookAndFeelChangedEventArgs.Reason.ToString() + ", " + userLookAndFeel.SkinName + ", " + userLookAndFeel.ActiveSvgPaletteName);
            iniConfig.Write("SkinName", userLookAndFeel.SkinName, "DevExpress");
            iniConfig.Write("SkinPalette", userLookAndFeel.ActiveSvgPaletteName, "DevExpress");
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            bbiNew.PerformClick();
        }

        private void LoadData()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("SELECT PS.OIDSTYLE AS No, PS.StyleName, PS.OIDGCATEGORY AS CategoryID, GC.CategoryName, PS.CreatedBy, PS.CreatedDate ");
            sbSQL.Append("FROM   ProductStyle AS PS INNER JOIN ");
            sbSQL.Append("       GarmentCategory AS GC ON PS.OIDGCATEGORY = GC.OIDGCATEGORY ");
            sbSQL.Append("ORDER BY PS.OIDSTYLE ");
            new ObjDevEx.setGridControl(gcStyle, gvStyle, sbSQL).getData(false, false, false, true);

            sbSQL.Clear();
            sbSQL.Append("SELECT CategoryName, OIDGCATEGORY AS ID ");
            sbSQL.Append("FROM GarmentCategory ");
            sbSQL.Append("ORDER BY CategoryName ");
            new ObjDevEx.setGridLookUpEdit(glueCategory, sbSQL, "CategoryName", "ID").getData(true);

        }

        private void NewData()
        {
            txeID.EditValue = new DBQuery("SELECT CASE WHEN ISNULL(MAX(OIDSTYLE), '') = '' THEN 1 ELSE MAX(OIDSTYLE) + 1 END AS NewNo FROM ProductStyle").getString();
            txeStyleNo.Text = "";
            glueCategory.EditValue = "";

            lblStatus.Text = "* Add Style";
            lblStatus.ForeColor = Color.Green;

            txeCREATE.EditValue = "0";
            txeDATE.EditValue = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            txeStyleNo.Focus();
        }

        private void bbiNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
            NewData();
        }

        private void gvGarment_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            
        }

        private bool chkDuplicateName()
        {
            bool chkDup = true;
            if (txeStyleNo.Text != "")
            {
                txeStyleNo.Text = txeStyleNo.Text.Trim();
                if (txeStyleNo.Text.Trim() != "" && lblStatus.Text == "* Add Style")
                {
                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append("SELECT TOP(1) StyleName FROM ProductStyle WHERE (StyleName = N'" + txeStyleNo.Text.Trim() + "') ");
                    if (new DBQuery(sbSQL).getString() != "")
                    {
                        chkDup = false;
                    }
                }
                else if (txeStyleNo.Text.Trim() != "" && lblStatus.Text == "* Edit Style")
                {
                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append("SELECT TOP(1) OIDSTYLE ");
                    sbSQL.Append("FROM ProductStyle ");
                    sbSQL.Append("WHERE (StyleName = N'" + txeStyleNo.Text.Trim().Replace("'", "''") + "') ");
                    string strCHK = new DBQuery(sbSQL).getString();

                    if (strCHK != "" && strCHK != txeID.Text.Trim())
                    {
                        chkDup = false;
                    }
                }
            }
            return chkDup;
        }

        private void txeStyleNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                glueCategory.Focus();
            }
        }

        private void txeStyleNo_LostFocus(object sender, EventArgs e)
        {
            
        }

        private void gvStyle_RowStyle(object sender, RowStyleEventArgs e)
        {
            
        }

        private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txeStyleNo.Text.Trim() == "")
            {
                FUNC.msgWarning("Please input style no.");
                txeStyleNo.Focus();
            }
            else if (glueCategory.Text.Trim() == "")
            {
                FUNC.msgWarning("Please input product category.");
                glueCategory.Focus();
            }
            else
            {
                bool chkDup = chkDuplicateName();
                if (chkDup == true)
                {
                    if (FUNC.msgQuiz("Confirm save data ?") == true)
                    {
                        StringBuilder sbSQL = new StringBuilder();

                        string strCREATE = "0";
                        if (txeCREATE.Text.Trim() != "")
                        {
                            strCREATE = txeCREATE.Text.Trim();
                        }

                        if (lblStatus.Text == "* Add Style")
                        {
                            sbSQL.Append("  INSERT INTO ProductStyle(StyleName, OIDGCATEGORY, CreatedBy, CreatedDate) ");
                            sbSQL.Append("  VALUES(N'" + txeStyleNo.Text.Trim().Replace("'", "''") + "', '" + glueCategory.EditValue.ToString() + "', '" + strCREATE + "', GETDATE()) ");
                        }
                        else if (lblStatus.Text == "* Edit Style")
                        {
                            sbSQL.Append("  UPDATE ProductStyle SET ");
                            sbSQL.Append("      StyleName = N'" + txeStyleNo.Text.Trim().Replace("'", "''") + "', OIDGCATEGORY = '" + glueCategory.EditValue.ToString() + "' ");
                            sbSQL.Append("  WHERE (OIDSTYLE = '" + txeID.Text.Trim() + "') ");
                        }

                        //sbSQL.Append("IF NOT EXISTS(SELECT OIDSTYLE FROM ProductStyle WHERE OIDSTYLE = '" + txeID.Text.Trim() + "') ");
                        //sbSQL.Append(" BEGIN ");
                        //sbSQL.Append("  INSERT INTO ProductStyle(StyleName, OIDGCATEGORY, CreatedBy, CreatedDate) ");
                        //sbSQL.Append("  VALUES(N'" + txeStyleNo.Text.Trim().Replace("'", "''") + "', '" + glueCategory.EditValue.ToString() + "', '" + strCREATE + "', GETDATE()) ");
                        //sbSQL.Append(" END ");
                        //sbSQL.Append("ELSE ");
                        //sbSQL.Append(" BEGIN ");
                        //sbSQL.Append("  UPDATE ProductStyle SET ");
                        //sbSQL.Append("      StyleName = N'" + txeStyleNo.Text.Trim().Replace("'", "''") + "', OIDGCATEGORY = '" + glueCategory.EditValue.ToString() + "' ");
                        //sbSQL.Append("  WHERE (OIDSTYLE = '" + txeID.Text.Trim() + "') ");
                        //sbSQL.Append(" END ");
                        //MessageBox.Show(sbSQL.ToString());
                        if (sbSQL.Length > 0)
                        {
                            try
                            {
                                bool chkSAVE = new DBQuery(sbSQL).runSQL();
                                if (chkSAVE == true)
                                {
                                    bbiNew.PerformClick();
                                    FUNC.msgInfo("Save complete.");
                                }
                            }
                            catch (Exception)
                            { }
                        }
                    }
                }
                else
                {
                    txeStyleNo.Text = "";
                    txeStyleNo.Focus();
                    FUNC.msgWarning("Duplicate style name. !! Please Change.");
                }
            }
        }

        private void bbiExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string pathFile = new ObjSet.Folder(@"C:\MDS\Export\").GetPath() + "StyleList_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            gvStyle.ExportToXlsx(pathFile);
            System.Diagnostics.Process.Start(pathFile);
        }

        private void gvStyle_RowClick(object sender, RowClickEventArgs e)
        {
            if (gvStyle.IsFilterRow(e.RowHandle)) return;
            lblStatus.Text = "* Edit Style";
            lblStatus.ForeColor = Color.Red;

            txeID.Text = gvStyle.GetFocusedRowCellValue("No").ToString();
            txeStyleNo.Text = gvStyle.GetFocusedRowCellValue("StyleName").ToString();
            glueCategory.EditValue = gvStyle.GetFocusedRowCellValue("CategoryID").ToString();

            txeCREATE.Text = gvStyle.GetFocusedRowCellValue("CreatedBy").ToString();
            txeDATE.Text = gvStyle.GetFocusedRowCellValue("CreatedDate").ToString();
        }

        private void bbiPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gcStyle.ShowPrintPreview();
        }

        private void bbiPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gcStyle.Print();
        }

        private void txeStyleNo_Leave(object sender, EventArgs e)
        {
            if (txeStyleNo.Text.Trim() != "")
            {
                txeStyleNo.Text = txeStyleNo.Text.ToUpper().Trim();
                bool chkDup = chkDuplicateName();
                if (chkDup == false)
                {
                    txeStyleNo.Text = "";
                    txeStyleNo.Focus();
                    FUNC.msgWarning("Duplicate style name. !! Please Change.");  
                }
            }
        }
    }
}