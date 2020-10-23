using System;
using System.Text;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Extensions;
//using DBConnection;
using MDS00;
using System.Drawing;

namespace M11
{
    public partial class XtraForm1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        //private Functionality.Function FUNC = new Functionality.Function();
        public XtraForm1()
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
            //StringBuilder sbSQL = new StringBuilder();
            //sbSQL.Append("SELECT OIDGParts AS No, GarmentParts, CreatedBy, CreatedDate ");
            //sbSQL.Append("FROM GarmentParts ");
            //sbSQL.Append("ORDER BY OIDGParts, GarmentParts ");
            //new ObjDevEx.setGridControl(gcGarment, gvGarment, sbSQL).getData(false, false, true, true);

        }

        private void NewData()
        {
            //txeGarment.Text = "";
            //lblStatus.Text = "* Add Garment";
            //lblStatus.ForeColor = Color.Green;

            //txeID.Text = new DBQuery("SELECT CASE WHEN ISNULL(MAX(OIDGParts), '') = '' THEN 1 ELSE MAX(OIDGParts) + 1 END AS NewNo FROM GarmentParts").getString();

            //txeCREATE.Text = "0";
            //txeDATE.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            ////txeID.Focus();
        }

        private void bbiNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
            NewData();
        }

        private void gvGarment_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //lblStatus.Text = "* Edit Garment";
            //lblStatus.ForeColor = Color.Red;

            //txeID.Text = gvGarment.GetFocusedRowCellValue("No").ToString();
            //txeGarment.Text = gvGarment.GetFocusedRowCellValue("GarmentParts").ToString();

            //txeCREATE.Text = gvGarment.GetFocusedRowCellValue("CreatedBy").ToString();
            //txeDATE.Text = gvGarment.GetFocusedRowCellValue("CreatedDate").ToString();
        }

        //private bool chkDuplicate()
        //{
        //    bool chkDup = true;
        //    if (txeGarment.Text != "")
        //    {
        //        txeGarment.Text = txeGarment.Text.Trim();
        //        if (txeGarment.Text.Trim() != "" && lblStatus.Text == "* Add Garment")
        //        {
        //            StringBuilder sbSQL = new StringBuilder();
        //            sbSQL.Append("SELECT TOP(1) GarmentParts FROM GarmentParts WHERE (GarmentParts = N'" + txeGarment.Text.Trim() + "') ");
        //            if (new DBQuery(sbSQL).getString() != "")
        //            {
        //                FUNC.msgWarning("Duplicate garment parts. !! Please Change.");
        //                txeGarment.Text = "";
        //                chkDup = false;
        //            }
        //        }
        //        else if (txeGarment.Text.Trim() != "" && lblStatus.Text == "* Edit Garment")
        //        {
        //            StringBuilder sbSQL = new StringBuilder();
        //            sbSQL.Append("SELECT TOP(1) OIDGParts ");
        //            sbSQL.Append("FROM GarmentParts ");
        //            sbSQL.Append("WHERE (GarmentParts = N'" + txeGarment.Text.Trim() + "') ");
        //            string strCHK = new DBQuery(sbSQL).getString();
        //            if (strCHK != "" && strCHK != txeID.Text.Trim())
        //            {
        //                FUNC.msgWarning("Duplicate garment parts. !! Please Change.");
        //                txeGarment.Text = "";
        //                chkDup = false;
        //            }
        //        }
        //    }
        //    return chkDup;
        //}

        //private void txeGarment_Leave(object sender, EventArgs e)
        //{
        //    bool chkDup = chkDuplicate();
        //    if (chkDup == false)
        //    {
        //        txeGarment.Text = "";
        //        txeGarment.Focus();
        //    }
        //}

        //private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (txeGarment.Text.Trim() == "")
        //    {
        //        FUNC.msgWarning("Please input garment parts.");
        //        txeGarment.Focus();
        //    }
        //    else
        //    {
        //        txeGarment.Text = txeGarment.Text.Trim();
        //        bool chkGMP = chkDuplicate();

        //        if (chkGMP == true)
        //        {
        //            if (FUNC.msgQuiz("Confirm save data ?") == true)
        //            {
        //                StringBuilder sbSQL = new StringBuilder();
        //                string strCREATE = "0";
        //                if (txeCREATE.Text.Trim() != "")
        //                {
        //                    strCREATE = txeCREATE.Text.Trim();
        //                }

        //                sbSQL.Append("IF NOT EXISTS(SELECT OIDGParts FROM GarmentParts WHERE OIDGParts = N'" + txeID.Text.Trim() + "') ");
        //                sbSQL.Append(" BEGIN ");
        //                sbSQL.Append("  INSERT INTO GarmentParts(GarmentParts, CreatedBy, CreatedDate) ");
        //                sbSQL.Append("  VALUES(N'" + txeGarment.Text.Trim() + "', '" + strCREATE + "', GETDATE()) ");
        //                sbSQL.Append(" END ");
        //                sbSQL.Append("ELSE ");
        //                sbSQL.Append(" BEGIN ");
        //                sbSQL.Append("  UPDATE GarmentParts SET ");
        //                sbSQL.Append("      GarmentParts = N'" + txeGarment.Text.Trim() + "' ");
        //                sbSQL.Append("  WHERE(OIDGParts = '" + txeID.Text.Trim() + "') ");
        //                sbSQL.Append(" END ");
        //                //MessageBox.Show(sbSQL.ToString());
        //                if (sbSQL.Length > 0)
        //                {
        //                    try
        //                    {
        //                        bool chkSAVE = new DBQuery(sbSQL).runSQL();
        //                        if (chkSAVE == true)
        //                        {
        //                            FUNC.msgInfo("Save complete.");
        //                            bbiNew.PerformClick();
        //                        }
        //                    }
        //                    catch (Exception)
        //                    { }
        //                }
        //            }
        //        }
        //    }
        //}

        //private void bbiExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    string pathFile = new ObjSet.Folder(@"C:\MDS\Export\").GetPath() + "GarmentPartsList_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
        //    gvGarment.ExportToXlsx(pathFile);
        //    System.Diagnostics.Process.Start(pathFile);
        //}
    }
}