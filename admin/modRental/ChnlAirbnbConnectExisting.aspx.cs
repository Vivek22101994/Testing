using RentalInRome.data;//sing MagaRentalCE.data;
//using ModLocation;
using ModRental;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using RentalInRome.data;

namespace MagaRentalCE.admin.modRental
{
    public partial class ChnlAirbnbConnectExisting : System.Web.UI.Page
    {
        public int cntTotalRows = 0;
        public int cntTotal = 0;
        public int cntTotalNotImported = 0;
        public int cntEstateNotFound = 0;
        public List<int> lstOldPropId = new List<int>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnImportData_Click(object sender, EventArgs e)
        {
            string file_directory = string.Empty;
            string filename = string.Empty;
            bool importstatus = false;
            if (ctrl_upload.HasFile)
            {
                try
                {
                    lblStatus.Text = "Please Wait...Processing";
                    filename = Path.GetFileName(ctrl_upload.FileName);

                    file_directory = Server.MapPath("~/admin/modImport/FilesToImport/");

                    ctrl_upload.SaveAs(file_directory + filename);
                    DataTable dt = Import_To_DataTable(file_directory + filename);

                    DataView view = new DataView(dt);
                    //DataTable distinctValues = view.ToTable(true, "Property ID");

                    importstatus = ImportImagesFromFileData(dt);//true;
                    if (importstatus)
                    {
                        string log = "Data Imported Successfully..Rows" + cntTotalRows + "<br/>" +
                                        "Total Images Imported: " + cntTotal + "<br/>" +
                                         "<br/><br/>" +
                                         "Total Images Not Imported: " + cntTotalNotImported + "<br/>" +
                                         "<br/><br/>" +
                                         "Images Excluded For Missing PropertyId: " + cntEstateNotFound + "<br/>" +
                                         "Missing PropertyIds: " + lstOldPropId.Distinct().ToList().listToString(",") + "<br/>" +
                                         "<br/><br/>";

                        lblStatus.Text = log;

                        ErrorLog.addLog("", "Import Old Images", log);

                        btnImportData.Visible = false;
                    }
                    else
                        lblStatus.Text = "Data Import failed";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Import Status: could not be done. The following error occured: " + ex.Message;
                }
                if (File.Exists(file_directory + filename))
                {
                    File.Delete(file_directory + filename);
                }
            }
        }

        private bool ImportImagesFromFileData(DataTable dt)
        {
            string _path = Path.Combine(App.SRP, HF_main_folder.Value);
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            magaRental_DataContext DC_RENTAL = new magaRental_DataContext();
            //var estateList = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.olDbId.HasValue && x.is_new_import == 1 && (x.mediaFolderOriginalPhotos == null || x.mediaFolderOriginalPhotos == "" || x.id == 2175)).ToList();
            int estateId = 0;
            try
            {
                DC_RENTAL.CommandTimeout = 0;

                bool isFirst = true;

                foreach (DataRow dr in dt.Rows)
                {
                    if (isFirst) { isFirst = false; continue; }

                    try
                    {
                        cntTotalRows = cntTotalRows + 1;

                        estateId = (GetValue(dr, "Magarental ID")).objToInt32();
                        if (estateId == 0) continue;

                        var currEstate = AppSettings.RNT_TB_ESTATE.FirstOrDefault(x => x.id == estateId);
                        if (currEstate == null)
                        {
                            cntEstateNotFound = cntEstateNotFound + 1;
                            lstOldPropId.Add(currEstate.id);
                            continue;
                        }

                        using (DCmodRental dc = new DCmodRental())
                        {
                            var agetContract = dc.dbRntEstateAgentContractRLs.SingleOrDefault(x => x.pidAgent == 12 && x.pidAgentContract == 5 && x.pidEstate == currEstate.id);
                            if (agetContract == null)
                            {
                                dbRntEstateAgentContractRL tmp = new dbRntEstateAgentContractRL();
                                tmp.pidEstate = currEstate.id;
                                tmp.pidAgent = 12;
                                tmp.pidAgentContract = 5;
                                dc.Add(tmp);
                                dc.SaveChanges();
                            }
                            var currAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == currEstate.id);
                            if (currAirbnbEstate == null)
                            {
                                currAirbnbEstate = new dbRntChnlAirbnbEstateTBL();
                                currAirbnbEstate.mr_id = currEstate.id;
                                dc.Add(currAirbnbEstate);
                                dc.SaveChanges();
                            }
                            currAirbnbEstate.airbnb_id = GetValue(dr, "LISTINGS ID").objToInt32();
                            currAirbnbEstate.isExisting = 1;
                            dc.SaveChanges();
                        }

                        magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                        var currPropertyHost = DC_AIRBNB.RntChnlAirbnbPropertyHostRL.SingleOrDefault(x => x.pidEstate == currEstate.id);
                        if (currPropertyHost == null)
                        {
                            currPropertyHost = new RntChnlAirbnbPropertyHostRL();
                            currPropertyHost.pidEstate = currEstate.id;
                            currPropertyHost.hostId = GetValue(dr, "HOST ID");
                            DC_AIRBNB.RntChnlAirbnbPropertyHostRL.InsertOnSubmit(currPropertyHost);
                        }
                        else
                        {
                            currPropertyHost.hostId = GetValue(dr, "HOST ID");
                            DC_AIRBNB.SubmitChanges();
                        }
                        DC_AIRBNB.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.addLog("", "ConnecExisting " + estateId, ex.ToString());
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ConnecExisting", ex.ToString());
                //continue;
            }

            return true;
        }

        #region Helper Methods

        private string GetValue(DataRow dr, string colName)
        {
            return (dr[colName] + "").Trim();
        }

        #endregion

        private DataTable Import_To_DataTable(string FilePath)
        {
            string conStr = string.Empty;
            if (CommonUtilities.getSYS_SETTING("OLEDB_Version").objToInt32() == 4)
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties='Excel 8.0;HDR=YES'";
            else
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'";

            conStr = String.Format(conStr, FilePath);

            OleDbConnection connExcel = new OleDbConnection(conStr);

            OleDbCommand cmdExcel = new OleDbCommand();

            OleDbDataAdapter oda = new OleDbDataAdapter();

            DataTable dt = new DataTable();

            cmdExcel.Connection = connExcel;

            cmdExcel.CommandTimeout = 0;
            //Get the name of First Sheet
            connExcel.Open();

            DataTable dtExcelSchema;

            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            string SheetName = HF_SheetName.Value;// dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

            connExcel.Close();

            //Read Data from First Sheet

            connExcel.Open();

            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

            oda.SelectCommand = cmdExcel;

            oda.Fill(dt);

            connExcel.Close();

            return dt;

        }
    }
}