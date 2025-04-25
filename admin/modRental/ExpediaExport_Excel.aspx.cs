using ModRental;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental
{
    public partial class ExpediaExport_Excel : adminBasePage
    {
        private magaRental_DataContext DC_Rental;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_Rental = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                bind_ListView();
                Bind_drp_zone();


                string _items = "";
                string _sep = "";

                var agentTBL = rntProps.AgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == "Expedia");
                if (agentTBL == null) return;

                var estateIds = new List<int>();
                using (DCmodRental dc = new DCmodRental())
                {
                    estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentTBL.id).Select(x => x.pidEstate).ToList();
                }

                List<RNT_TB_ESTATE> _estateList = DC_Rental.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted != 1 && estateIds.Contains(x.id)).OrderBy(x => x.code).ToList();
                foreach (RNT_TB_ESTATE _estate in _estateList)
                {
                    _items += _sep + "{idEstate: \"" + _estate.id + "\", idZone: \"0\",label: \"" + _estate.code + "\",desc: \"\"}";
                    _sep = ",";
                }
                ltr_items.Text = _items;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setAutocomplete", "setAutocomplete();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "bindCheck", "bindCheck();", true);


        }


        private void bind_ListView()
        {
            var agentTBL = rntProps.AgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == "Expedia");
            if (agentTBL == null) return;

            var estateIds = new List<int>();
            using (DCmodRental dc = new DCmodRental())
            {
                estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentTBL.id).Select(x => x.pidEstate).ToList();
            }
            var estateList = DC_Rental.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted != 1 && estateIds.Contains(x.id)).ToList();

            if (drp_zone.SelectedValue.objToInt32() > 0)
            {
                estateList = estateList.Where(x => x.pid_zone == drp_zone.getSelectedValueInt()).ToList();
            }
            if (!string.IsNullOrWhiteSpace(txt_code.Text))
            {
                estateList = estateList.Where(x => x.id == HF_Estate.Value.objToInt32()).ToList();
            }
            LV.Items.Clear();
            LV.DataSource = estateList;
            LV.DataBind();
        }
        protected void LV_DataBound(object sender, EventArgs e)
        {
            lbl_record_count_top.Text = "Total Rows  :- " + LV.Items.Count;
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            DC_Rental = maga_DataContext.DC_RENTAL;
            List<RNT_TB_ESTATE> _exportEstateList = new List<RNT_TB_ESTATE>();

            foreach (ListViewDataItem item in LV.Items)
            {
                Label lblid = item.FindControl("lbl_id") as Label;
                try
                {
                    CheckBox chkCtrl = item.FindControl("chkExport") as CheckBox;
                    if (chkCtrl.Checked)
                    {

                        var objEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == lblid.Text.ToInt32() && x.is_active == 1 && x.is_deleted != 1);
                        if (objEstate != null)
                        {
                            _exportEstateList.Add(objEstate);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            bool response = false;
            if (_exportEstateList.Count > 0)
            {
                response = ExportToExcel_Dec2015Update(_exportEstateList);
                if (!response)
                {
                    lt_exportError.Text = "error in Export data , Please contact support";
                    lt_exportError.Visible = true;
                }

            }
            else
            {
                lt_exportError.Text = "error in Export data , Please contact support";
                lt_exportError.Visible = true;
            }



        }

        private void Bind_drp_zone()
        {
            List<int> ZoneWithEstate = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1 && x.bcomEnabled == 1 && x.pid_zone.HasValue).Select(x => x.pid_zone.Value).Distinct().ToList();
            List<LOC_VIEW_ZONE> list = AppSettings.LOC_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1 && ZoneWithEstate.Contains(x.id)).ToList();
            drp_zone.Items.Clear();
            drp_zone.DataSource = list;
            drp_zone.DataTextField = "title";
            drp_zone.DataValueField = "id";
            drp_zone.DataBind();

            drp_zone.Items.Insert(0, new ListItem("- tutti -", "0"));
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            bind_ListView();
        }




        protected bool ExportToExcel_Final(List<RNT_TB_ESTATE> estateList)
        {
            try
            {
                string filename = "bcom_property_list_" + DateTime.Now.JSCal_dateToString();
                if (estateList.Count > 0)
                {

                    var workbook = new HSSFWorkbook();
                    var sheet0 = workbook.CreateSheet("General Information");
                    var sheet2 = workbook.CreateSheet("Invoice Details");
                    var sheet3 = workbook.CreateSheet("Contact Details");
                    var sheet1 = workbook.CreateSheet("Property Information");
                    var sheet4 = workbook.CreateSheet("Extra costs & Tax");
                    var sheet5 = workbook.CreateSheet("Policies");
                    var sheet6 = workbook.CreateSheet("Photos");


                    //header style
                    var filterLabelCellStyle = workbook.CreateCellStyle();
                    var filterLabelFont = workbook.CreateFont();
                    filterLabelCellStyle.SetFont(filterLabelFont);
                    filterLabelCellStyle.Alignment = HorizontalAlignment.LEFT;

                    var rowIndex = 0;
                    int colIndex = 0;

                    var row0 = sheet0.CreateRow(rowIndex);
                    var row1 = sheet1.CreateRow(rowIndex);
                    var row2 = sheet2.CreateRow(rowIndex);
                    var row3 = sheet3.CreateRow(rowIndex);
                    var row4 = sheet4.CreateRow(rowIndex);
                    var row5 = sheet5.CreateRow(rowIndex);
                    var row6 = sheet6.CreateRow(rowIndex);


                    var cell = row1.CreateCell(0);

                    #region Header of columns

                    var headerLabelCellStyle = workbook.CreateCellStyle();
                    var headerLabelFont = workbook.CreateFont();
                    headerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
                    headerLabelCellStyle.SetFont(headerLabelFont);
                    headerLabelCellStyle.Alignment = HorizontalAlignment.CENTER;

                    var style1 = headerLabelCellStyle;
                    style1.BorderRight = CellBorderType.MEDIUM;
                    style1.BorderBottom = CellBorderType.MEDIUM;
                    style1.BorderTop = CellBorderType.MEDIUM;
                    style1.BorderLeft = CellBorderType.MEDIUM;


                    var style2 = filterLabelCellStyle;
                    style2.BorderRight = CellBorderType.THIN;
                    style2.BorderBottom = CellBorderType.THIN;
                    style2.BorderTop = CellBorderType.THIN;
                    style2.BorderLeft = CellBorderType.THIN;

                    // first Sheet General Information
                    row0 = sheet0.CreateRow(rowIndex);
                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);

                    #region Data for First Tab

                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    addImageToSheet(sheet0, workbook);

                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    // First row to last row & first cell to last cell
                    NPOI.SS.Util.CellRangeAddress cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 1);
                    sheet0.AddMergedRegion(cra);

                    //style1.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                    //style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREEN.index;

                    AddNewColumn(row0, "Booking.com Representative (Internal use only)", ref colIndex, style1); // Apartment

                    rowIndex++;
                    colIndex = 0;
                    row0 = sheet0.CreateRow(rowIndex);

                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    AddNewColumn(row0, "Account Manager:", ref colIndex, style2);
                    AddNewColumn(row0, "", ref colIndex, style2);

                    rowIndex++;
                    colIndex = 0;
                    row0 = sheet0.CreateRow(rowIndex);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    AddNewColumn(row0, "Office:", ref colIndex, style2);
                    AddNewColumn(row0, "", ref colIndex, style2);

                    rowIndex++;
                    colIndex = 0;
                    row0 = sheet0.CreateRow(rowIndex);
                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);
                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);


                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    NPOI.SS.Util.CellRangeAddress cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 4);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "General Information", ref colIndex, style1);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 1);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "Connectivity (Internal use only)", ref colIndex, style1);

                    rowIndex++;
                    colIndex = 0;
                    row0 = sheet0.CreateRow(rowIndex);

                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    // First row to last row & first cell to last cell
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    sheet0.AddMergedRegion(cra1);

                    AddNewColumn(row0, "Company Name:", ref colIndex, style2);

                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "", ref colIndex, style2);

                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    AddNewColumn(row0, "Do you need a bulk switch? (Y/N)", ref colIndex, style2);
                    AddNewColumn(row0, "", ref colIndex, style2);


                    colIndex = 0;
                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "Company Country:", ref colIndex, style2);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "", ref colIndex, style2);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "Name of provider + Internal ID:", ref colIndex, style2);
                    AddNewColumn(row0, "Name:          ID:    ", ref colIndex, style2);


                    rowIndex++;
                    colIndex = 0;
                    row0 = sheet0.CreateRow(rowIndex);
                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);
                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 4);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "Partner´s Contact Person", ref colIndex, style1);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 1);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "General Configuration  (Internal use only)", ref colIndex, style1);


                    colIndex = 0;
                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "Name :", ref colIndex, style2);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, " ", ref colIndex, style2);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "(Existing or New) Name of partner?", ref colIndex, style2);
                    AddNewColumn(row0, " ", ref colIndex, style2);


                    colIndex = 0;
                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "Email:", ref colIndex, style2);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "", ref colIndex, style2);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "(Existing or New)  Internal Brand:", ref colIndex, style2);
                    AddNewColumn(row0, " ", ref colIndex, style2);



                    colIndex = 0;
                    rowIndex++;
                    row0 = sheet0.CreateRow(rowIndex);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, "Telephone:", ref colIndex, style2);
                    cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    sheet0.AddMergedRegion(cra1);
                    AddNewColumn(row0, " ", ref colIndex, style2);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row0, "(Existing or New)  Group Login:", ref colIndex, style2);
                    AddNewColumn(row0, " ", ref colIndex, style2);


                    #endregion

                    rowIndex = 0;
                    colIndex = 0;
                    // Second Sheet  Property Information
                    row1 = sheet1.CreateRow(rowIndex);
                    AddNewColumn(row1, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Property URL", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "VAT number", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Property Name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Address", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "City", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Latitude", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Longitude", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Zipcode", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Country code", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Accommodation Type ", ref colIndex, headerLabelCellStyle); // Apartment/Villa
                    AddNewColumn(row1, "Number of Rooms", ref colIndex, headerLabelCellStyle); // default 1
                    AddNewColumn(row1, "Currency Code", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Check in from", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Check in until", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Check out from", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Check out until", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Accepted Credit Cards", ref colIndex, headerLabelCellStyle);

                    // General Facilities - propety Level 

                    AddNewColumn(row1, "Rooms / facilities for disabled", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Private beach area", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Beach Front", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Bikes available (free)", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Heating", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Air Conditioning", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Garden", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Terrace", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Children play Ground", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Barbecue facilities", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Table tennis", ref colIndex, headerLabelCellStyle);

                    AddNewColumn(row1, "Hot tub(outdoor)", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Outdoor swimming pool", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Indoor swimming pool", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Fitness room", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Sauna", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Solarium", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Turkish / steam bath", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Billiard", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Darts", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Game room", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Library", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Safe deposit box", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Ski storage", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Ski-to-door access", ref colIndex, headerLabelCellStyle);

                    //  Private Facilities - Room/Unit level

                    AddNewColumn(row1, "Room/ Unit type", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Room / Property Name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Number of bedrooms", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Number of bathrooms", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Max. persons to stay in the property", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Minimum Size of Room/Unit", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Entire unit wheelchair accessible", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "air-conditioning", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "fan", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Smoking", ref colIndex, headerLabelCellStyle);  // smoking, non-smoking, unknown
                    AddNewColumn(row1, "Detached", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Semi-detached", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Private apartment in block of apartments", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Entire property on ground floor", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Upper floor reachable by elevator", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Upper floor reachable by stairs only", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Entire unit wheelchair accessible", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Private entrance", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "heating", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "carpeted floor", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "tiled/marble floor", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "wooden / parquet floor", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "soundproofing", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "safe deposit box", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Lake view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "River view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Sea view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Mountain view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Garden view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Pool view", ref colIndex, headerLabelCellStyle);  // 
                    AddNewColumn(row1, "Private balcony", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private terrace", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private patio", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "outdoor dining area", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "outdoor furniture", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private barbecue", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private pool", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "bathroom", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "toilet", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "bath", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "shower", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Bath or shower", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "spa bath", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "bidet", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "guest toilet", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private sauna", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private hot tub", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "hair dryer", ref colIndex, headerLabelCellStyle);

                    AddNewColumn(row1, "towels (free)", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "linen (free)", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "bath-robe", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "slippers", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "free toiletries", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "kitchen", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "kitchenette", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "kitchenware", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "refrigerator", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "stove", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "oven", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "dishwasher", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "coffee machine", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "electric kettle", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "toaster", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "microwave", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "cleaning products", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "children highchair", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "dining area", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "dining table", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "seating area", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "fireplace", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "sofa", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "TV", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "flat-screen TV", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "cable channels", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "satellite channels", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "pay-per-view channels", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "telephone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "radio", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "CD-player", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "blu-ray player", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "DVD-player", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console - Nintendo Wii", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console - Playstation 2", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console - Playstation 3", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console - Xbox 360", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "video games", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "computer", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "ipad", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "ipod docking station", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "laptop", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "laptop safe box", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "desk", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "wardrobe/ closet", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "clothes rack", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "mosquito net", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "ironing facilities", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "washing machine", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "tumble dryer (machine)", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "alarm-clock", ref colIndex, headerLabelCellStyle);


                    ////Third  Sheet - Invoicing Details
                    //rowIndex = 0;
                    //colIndex = 0;
                    //row2 = sheet2.CreateRow(rowIndex);
                    //AddNewColumn(row2, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Legal company name for invoicing", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Attention of", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Legal address", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Legal zip code", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Legal city", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Country", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "VAT or TAX number", ref colIndex, headerLabelCellStyle);

                    #region Fourth  Sheet - Invoicing Details
                    rowIndex = 0;
                    colIndex = 0;
                    row2 = sheet2.CreateRow(rowIndex);
                    //AddNewColumn(row3, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row2, "Legal company name for invoicing", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row2, "Attention of", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row2, "Legal address", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row2, "Legal zip code", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row2, "Legal city", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row2, "Country", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row2, "VAT or TAX number", ref colIndex, headerLabelCellStyle);


                    rowIndex++;
                    row2 = sheet2.CreateRow(rowIndex);
                    colIndex = 0;
                    //SetRowCellValue(row3, ref colIndex, tmp_data.id + "");
                    SetRowCellValue(row2, ref colIndex, "Rental in Rome Srl");
                    SetRowCellValue(row2, ref colIndex, "Jacopo  Calabrò");
                    SetRowCellValue(row2, ref colIndex, "Via Appia Nuova 677");
                    SetRowCellValue(row2, ref colIndex, "00179");
                    SetRowCellValue(row2, ref colIndex, "Roma");
                    SetRowCellValue(row2, ref colIndex, "Italy");
                    SetRowCellValue(row2, ref colIndex, "IT07824541002"); /// Vat Number


                    #endregion


                    //Fourth  Sheet - Contact Details
                    rowIndex = 0;
                    colIndex = 0;
                    row3 = sheet3.CreateRow(rowIndex);
                    AddNewColumn(row3, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Contracts contact name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Contracts contact email", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Contracts contact phone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Contracts contact language", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Invoices contact name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Invoices contact email", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Invoices contact phone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Invoices contact language", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Primary point of contact name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Primary point of  contact email", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Primary point of  contact phone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Primary point of  contact language", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Reservations contact name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Reservations contact email", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Reservations contact phone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Reservations contact language", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Central reservations contact name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Central reservations contact email", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Central reservations contact phone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Central reservations contact language", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, " Special request contact name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, " Special request contact email", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, " Special request contact phone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, " Special request contact language", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Availability contact name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Availability contact email", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Availability contact phone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Availability contact language", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Photos & Descriptions contact name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Photos & Descriptions contact email", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Photos & Descriptions contact phone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row3, "Photos & Descriptions contact language", ref colIndex, headerLabelCellStyle);


                    //Fifth  Sheet - Extra Cost & Tax Details
                    rowIndex = 0;
                    colIndex = 0;
                    row4 = sheet4.CreateRow(rowIndex);
                    AddNewColumn(row4, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row4, "VAT or TAX", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row4, "VAT/TAX", ref colIndex, headerLabelCellStyle); //(Included / Excluded)
                    AddNewColumn(row4, "VAT amount in %", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row4, "VAT Type ", ref colIndex, headerLabelCellStyle);  // NA - not applicable, PC - percentage, IC - incalculable
                    AddNewColumn(row4, "City tax ", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row4, "City tax specification", ref colIndex, headerLabelCellStyle); // (NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night)
                    AddNewColumn(row4, "City tax amount in %", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "servicecharge_type", ref colIndex, headerLabelCellStyle); //NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night, PC - percentage, PPNR - per person per night restricted, IC - incalculable
                    //AddNewColumn(row4, "Servicecharge", ref colIndex, headerLabelCellStyle); // (Included / Excluded) 
                    //AddNewColumn(row4, "servicecharge_amount", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row4, "Security deposit amount per stay", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row4, "Final cleaning fee", ref colIndex, headerLabelCellStyle);  // (Included / Optional / Mandatory / Done by guest)
                    AddNewColumn(row4, "Final cleaning fee amount", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row4, "Final cleaning specification", ref colIndex, headerLabelCellStyle); // (NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night, PC - percentage, PPNR - per person per night restricted) 
                    AddNewColumn(row4, "Bed linen and bath towels ", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row4, "Bed linen and bath towels amount per person per stay", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row4, "Energy cost", ref colIndex, headerLabelCellStyle);     // yes/no


                    //Six  Sheet - Extra Cost & Tax Details
                    rowIndex = 0;
                    colIndex = 0;
                    row5 = sheet5.CreateRow(rowIndex);
                    AddNewColumn(row5, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "How many children are allowed to stay for free using existing beds/cots?", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Up to which age can children stay free of charge when using existing bedding?", ref colIndex, headerLabelCellStyle); //(Included / Excluded)
                    AddNewColumn(row5, "How many baby cots are available at the property?", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Is there an extra charge for a baby cot?", ref colIndex, headerLabelCellStyle);  // NA - not applicable, PC - percentage, IC - incalculable
                    AddNewColumn(row5, "How many extra beds are available at the property?", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Is there an extra charge per extra bed?", ref colIndex, headerLabelCellStyle); // (NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night)
                    AddNewColumn(row5, "Do guests have access to the Internet?", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Which kind of internet access is available?", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Where is internet available?", ref colIndex, headerLabelCellStyle); //NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night, PC - percentage, PPNR - per person per night restricted, IC - incalculable
                    AddNewColumn(row5, "How much do you charge for internet?", ref colIndex, headerLabelCellStyle); // (Included / Excluded) 
                    AddNewColumn(row5, "Are parking spaces available for guests?", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Where is the parking located? ", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Private or public parking?", ref colIndex, headerLabelCellStyle);  // (Included / Optional / Mandatory / Done by guest)
                    AddNewColumn(row5, "How much do you charge for parking?", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Time frame of the parking cost", ref colIndex, headerLabelCellStyle); // (NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night, PC - percentage, PPNR - per person per night restricted) 
                    AddNewColumn(row5, "Is it necessary to make a reservation for parking?", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Are pets allowed? ", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row5, "Is there an additional charge for pets?", ref colIndex, headerLabelCellStyle);     // yes/no


                    //Six  Sheet - Extra Cost & Tax Details
                    rowIndex = 0;
                    colIndex = 0;
                    row6 = sheet6.CreateRow(rowIndex);
                    AddNewColumn(row6, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row6, "Tag of the photo", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row6, "URL of the photo", ref colIndex, headerLabelCellStyle);

                    #endregion


                    var listFacilites = DC_Rental.RNT_RL_ESTATE_CONFIG.Where(x => x.is_HomeAway == 0).ToList();
                    var listMedia = DC_Rental.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == "original").ToList();


                    rowIndex++;
                    colIndex = 0;
                    int imageIndex = 1;
                    #region Data

                    foreach (var tmp_data in estateList)
                    {
                        # region  Second Sheet - Property Detail
                        var tmpEstatefacility = listFacilites.Where(x => x.pid_estate == tmp_data.id).ToList();
                        row1 = sheet1.CreateRow(rowIndex);
                        colIndex = 0;
                        SetRowCellValue(row1, ref colIndex, tmp_data.id + "");

                        SetRowCellValue(row1, ref colIndex, "");
                        SetRowCellValue(row1, ref colIndex, "");
                        SetRowCellValue(row1, ref colIndex, tmp_data.code);
                        SetRowCellValue(row1, ref colIndex, tmp_data.loc_address);
                        SetRowCellValue(row1, ref colIndex, AdminUtilities.getCityTitle(tmp_data.pid_city));

                        if (!string.IsNullOrWhiteSpace(tmp_data.google_maps))
                        {
                            if (tmp_data.google_maps.Split('|').Length > 1)
                            {
                                SetRowCellValue(row1, ref colIndex, tmp_data.google_maps.Split('|')[0].Replace(",", "."));
                                SetRowCellValue(row1, ref colIndex, tmp_data.google_maps.Split('|')[1].Replace(",", "."));
                            }
                            else if (tmp_data.google_maps.Split(',').Length > 1)
                            {
                                SetRowCellValue(row1, ref colIndex, tmp_data.google_maps.Split(',')[0]);
                                SetRowCellValue(row1, ref colIndex, tmp_data.google_maps.Split(',')[1]);
                            }
                            else
                            {
                                SetRowCellValue(row1, ref colIndex, "0.00");
                                SetRowCellValue(row1, ref colIndex, "0.00");
                            }
                        }
                        else
                        {
                            SetRowCellValue(row1, ref colIndex, "0.00");
                            SetRowCellValue(row1, ref colIndex, "0.00");
                        }


                        SetRowCellValue(row1, ref colIndex, tmp_data.loc_zip_code + "");
                        SetRowCellValue(row1, ref colIndex, "it");
                        SetRowCellValue(row1, ref colIndex, tmp_data.category == "apt" ? "Apartment" : "Villa");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "EUR");
                        SetRowCellValue(row1, ref colIndex, "14:00"); //checkin from 
                        SetRowCellValue(row1, ref colIndex, "15:00");//checkin until
                        SetRowCellValue(row1, ref colIndex, "11:00"); //check out from 
                        SetRowCellValue(row1, ref colIndex, "00:00");//check out until
                        SetRowCellValue(row1, ref colIndex, "Visa|Mastercard");
                        //  General facilities - Property level
                        SetRowCellValue(row1, ref colIndex, "0"); //Rooms / facilities for disabled

                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 36));
                        SetRowCellValue(row1, ref colIndex, "1"); // heating 
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 1));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 54));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 30));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 25));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 22));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");

                        SetRowCellValue(row1, ref colIndex, tmp_data.category == "apt" ? "Apartment" : "Villa");
                        SetRowCellValue(row1, ref colIndex, tmp_data.code);
                        SetRowCellValue(row1, ref colIndex, tmp_data.num_rooms_bed + "");
                        SetRowCellValue(row1, ref colIndex, tmp_data.num_rooms_bath + "");
                        SetRowCellValue(row1, ref colIndex, tmp_data.num_persons_max + "");
                        SetRowCellValue(row1, ref colIndex, tmp_data.mq_inner + "");
                        SetRowCellValue(row1, ref colIndex, "0"); // WheelChair 



                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 1));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "unknown");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1"); // heating
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 22));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        //SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 54));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 54));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");

                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 25));
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1");

                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 46));
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 32));
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 18));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 15));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 47));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 52));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 19));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 35));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 20));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 8));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");

                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 12));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 49));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 34));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 14));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 48));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 6));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 53));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 24));

                        #endregion

                        //#region Third Sheet Invoice  Details

                        //row2 = sheet2.CreateRow(rowIndex);
                        //colIndex = 0;
                        //SetRowCellValue(row2, ref colIndex, tmp_data.id + "");
                        //SetRowCellValue(row2, ref colIndex, "Rental in Rome Srl");
                        //SetRowCellValue(row2, ref colIndex, "Jacopo  Calabrò");
                        //SetRowCellValue(row2, ref colIndex, "Via Appia Nuova 677");
                        //SetRowCellValue(row2, ref colIndex, "00179");
                        //SetRowCellValue(row2, ref colIndex, "Roma");
                        //SetRowCellValue(row2, ref colIndex, "Italy");
                        //SetRowCellValue(row2, ref colIndex, "IT07824541002"); /// Vat Number

                        //#endregion

                        #region Fourth Sheet Contact Details

                        row3 = sheet3.CreateRow(rowIndex);
                        colIndex = 0;
                        SetRowCellValue(row3, ref colIndex, tmp_data.id + "");
                        // Contracts contact
                        SetRowCellValue(row3, ref colIndex, "Matteo Calabrò");
                        SetRowCellValue(row3, ref colIndex, "matteo.calabro@rentalinrome.com");
                        SetRowCellValue(row3, ref colIndex, "+39 06 3220068");
                        SetRowCellValue(row3, ref colIndex, "IT");

                        // invocie contact
                        SetRowCellValue(row3, ref colIndex, "Matteo Calabrò");
                        SetRowCellValue(row3, ref colIndex, "matteo.calabro@rentalinrome.com");
                        SetRowCellValue(row3, ref colIndex, "+39 06 3220068");
                        SetRowCellValue(row3, ref colIndex, "IT");

                        // Primary point of contact
                        SetRowCellValue(row3, ref colIndex, "Matteo Calabrò");
                        SetRowCellValue(row3, ref colIndex, "matteo.calabro@rentalinrome.com");
                        SetRowCellValue(row3, ref colIndex, "+39 06 3220068");
                        SetRowCellValue(row3, ref colIndex, "IT");

                        // Reservations Contact
                        SetRowCellValue(row3, ref colIndex, "Francesca Sangiorgio ");
                        SetRowCellValue(row3, ref colIndex, "reception@rentalinrome.com");
                        SetRowCellValue(row3, ref colIndex, "+39 06 3220068");
                        SetRowCellValue(row3, ref colIndex, "IT");

                        // Central reservations Contact
                        SetRowCellValue(row3, ref colIndex, "Matteo Calabrò");
                        SetRowCellValue(row3, ref colIndex, "matteo.calabro@rentalinrome.com");
                        SetRowCellValue(row3, ref colIndex, "+39 06 3220068");
                        SetRowCellValue(row3, ref colIndex, "IT");


                        //  Special requests Contact
                        SetRowCellValue(row3, ref colIndex, "Matteo Calabrò");
                        SetRowCellValue(row3, ref colIndex, "matteo.calabro@rentalinrome.com");
                        SetRowCellValue(row3, ref colIndex, "+39 06 3220068");
                        SetRowCellValue(row3, ref colIndex, "IT");

                        //    Availability Contact
                        SetRowCellValue(row3, ref colIndex, "Matteo Calabrò");
                        SetRowCellValue(row3, ref colIndex, "matteo.calabro@rentalinrome.com");
                        SetRowCellValue(row3, ref colIndex, "+39 06 3220068");
                        SetRowCellValue(row3, ref colIndex, "IT");

                        //     Photos & Descriptions Contact
                        SetRowCellValue(row3, ref colIndex, "Matteo Calabrò");
                        SetRowCellValue(row3, ref colIndex, "matteo.calabro@rentalinrome.com");
                        SetRowCellValue(row3, ref colIndex, "+39 06 3220068");
                        SetRowCellValue(row3, ref colIndex, "IT");

                        #endregion

                        #region Fifth Sheet Extra cost

                        row4 = sheet4.CreateRow(rowIndex);
                        colIndex = 0;
                        SetRowCellValue(row4, ref colIndex, tmp_data.id + "");

                        SetRowCellValue(row4, ref colIndex, "VAT");
                        SetRowCellValue(row4, ref colIndex, "Included");
                        SetRowCellValue(row4, ref colIndex, "1.10");
                        SetRowCellValue(row4, ref colIndex, "PC");
                        SetRowCellValue(row4, ref colIndex, "Excluded");
                        SetRowCellValue(row4, ref colIndex, "PPN");
                        SetRowCellValue(row4, ref colIndex, "3.5"); // city  tax

                        // Service Charge
                        //SetRowCellValue(row4, ref colIndex, "NA");  
                        //SetRowCellValue(row4, ref colIndex, "");
                        //SetRowCellValue(row4, ref colIndex, "");

                        SetRowCellValue(row4, ref colIndex, "EUR " + tmp_data.pr_deposit); // Securtity Deposit
                        // Final cleaning 
                        SetRowCellValue(row4, ref colIndex, "Included");
                        SetRowCellValue(row4, ref colIndex, "EUR " + tmp_data.eco_ext_price); // 
                        SetRowCellValue(row4, ref colIndex, "PS");
                        // Bed Linen 
                        SetRowCellValue(row4, ref colIndex, "Included");
                        SetRowCellValue(row4, ref colIndex, "EUR 0.00");
                        SetRowCellValue(row4, ref colIndex, "No"); // Energy Cost


                        #endregion

                        #region sixth Sheet Policies

                        row5 = sheet5.CreateRow(rowIndex);
                        colIndex = 0;
                        SetRowCellValue(row5, ref colIndex, tmp_data.id + "");

                        SetRowCellValue(row5, ref colIndex, tmp_data.num_persons_child + "");
                        SetRowCellValue(row5, ref colIndex, "3");
                        SetRowCellValue(row5, ref colIndex, "0");
                        SetRowCellValue(row5, ref colIndex, "NA");
                        SetRowCellValue(row5, ref colIndex, "0");
                        SetRowCellValue(row5, ref colIndex, "NA");

                        // Internet
                        SetRowCellValue(row5, ref colIndex, "Yes");
                        SetRowCellValue(row5, ref colIndex, "WiFi");
                        SetRowCellValue(row5, ref colIndex, "All areas");
                        SetRowCellValue(row5, ref colIndex, "Free");
                        // Parking
                        SetRowCellValue(row5, ref colIndex, hasFacilities(tmpEstatefacility, 28) == "1" ? "Yes" : "No");
                        SetRowCellValue(row5, ref colIndex, hasFacilities(tmpEstatefacility, 31) == "1" ? "NearBy" : "On Site");
                        SetRowCellValue(row5, ref colIndex, "Private"); // 
                        SetRowCellValue(row5, ref colIndex, "Free");
                        SetRowCellValue(row5, ref colIndex, "NA");
                        SetRowCellValue(row5, ref colIndex, "No");

                        //pets
                        SetRowCellValue(row5, ref colIndex, hasFacilities(tmpEstatefacility, 4) == "1" ? "Allowed" : "Not allowed");
                        SetRowCellValue(row5, ref colIndex, "NA");

                        #endregion

                        #region Seven Sheet Photos

                        var lstEstatePhotos = listMedia.Where(x => x.pid_estate == tmp_data.id).OrderBy(x => x.sequence).ToList();
                        foreach (RNT_RL_ESTATE_MEDIA image in lstEstatePhotos)
                        {
                            row6 = sheet6.CreateRow(imageIndex);
                            colIndex = 0;
                            SetRowCellValue(row6, ref colIndex, tmp_data.id + "");
                            SetRowCellValue(row6, ref colIndex, string.IsNullOrWhiteSpace(image.code) ? "Other" : image.code);
                            SetRowCellValue(row6, ref colIndex, App.HOST + App.RP + image.img_banner);
                            imageIndex++;
                        }


                        #endregion
                        rowIndex++;
                    }
                    #endregion

                    for (int c = 0; c < 12; c++)
                    {
                        sheet0.AutoSizeColumn(c, true);
                        sheet1.AutoSizeColumn(c, true);
                        sheet2.AutoSizeColumn(c, true);
                        sheet3.AutoSizeColumn(c, true);
                        sheet4.AutoSizeColumn(c, true);
                        sheet5.AutoSizeColumn(c, true);
                        sheet6.AutoSizeColumn(c, true);
                    }

                    string path = Server.MapPath("/files");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string saveAsFileName = filename + ".xls";
                    string filePath = Path.Combine(path, saveAsFileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(stream);
                        Response.Clear();
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                    }
                    Response.WriteFile(filePath);
                    Response.Flush();
                    File.Delete(filePath);
                    Response.End();
                    return true;
                }
                return
                    false;
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "BookingExport", ex.Message);
                return false;
            }
        }

        protected bool ExportToExcel_Dec2015Update(List<RNT_TB_ESTATE> estateList)
        {
            try
            {
                string filename = "expedia_property_list_" + DateTime.Now.JSCal_dateToString();
                if (estateList.Count > 0)
                {

                    var workbook = new HSSFWorkbook();
                    // var sheet0 = workbook.CreateSheet("General Information");
                    var sheet1 = workbook.CreateSheet("Property Information");
                    // var sheet2 = workbook.CreateSheet("Contact Details");
                    // var sheet3 = workbook.CreateSheet("Invoice Details");
                    // var sheet4 = workbook.CreateSheet("Extra costs & Tax");
                    // var sheet5 = workbook.CreateSheet("Policies");
                    var sheet6 = workbook.CreateSheet("Photos");
                    var sheet7 = workbook.CreateSheet("Bedding");

                    //header style
                    var filterLabelCellStyle = workbook.CreateCellStyle();
                    var filterLabelFont = workbook.CreateFont();
                    filterLabelCellStyle.SetFont(filterLabelFont);
                    filterLabelCellStyle.Alignment = HorizontalAlignment.LEFT;

                    var rowIndex = 0;
                    int colIndex = 0;

                    //var row0 = sheet0.CreateRow(rowIndex);
                    var row1 = sheet1.CreateRow(rowIndex);
                    //var row2 = sheet2.CreateRow(rowIndex);
                    //var row3 = sheet3.CreateRow(rowIndex);
                   // var row4 = sheet4.CreateRow(rowIndex);
                    //var row5 = sheet5.CreateRow(rowIndex);
                    var row6 = sheet6.CreateRow(rowIndex);
                    var row7 = sheet7.CreateRow(rowIndex);

                    var cell = row1.CreateCell(0);

                    #region Header of columns

                    var headerLabelCellStyle = workbook.CreateCellStyle();
                    var headerLabelFont = workbook.CreateFont();
                    headerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
                    headerLabelCellStyle.SetFont(headerLabelFont);
                    headerLabelCellStyle.Alignment = HorizontalAlignment.CENTER;

                    var style1 = headerLabelCellStyle;
                    style1.BorderRight = CellBorderType.MEDIUM;
                    style1.BorderBottom = CellBorderType.MEDIUM;
                    style1.BorderTop = CellBorderType.MEDIUM;
                    style1.BorderLeft = CellBorderType.MEDIUM;


                    var style2 = filterLabelCellStyle;
                    style2.BorderRight = CellBorderType.THIN;
                    style2.BorderBottom = CellBorderType.THIN;
                    style2.BorderTop = CellBorderType.THIN;
                    style2.BorderLeft = CellBorderType.THIN;

                    #region first Sheet General Information
                    //row0 = sheet0.CreateRow(rowIndex);
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);

                    //#region Data for First Tab

                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    //addImageToSheet(sheet0, workbook);

                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    //// First row to last row & first cell to last cell
                    //NPOI.SS.Util.CellRangeAddress cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 1);
                    //sheet0.AddMergedRegion(cra);

                    ////style1.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                    ////style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREEN.index;

                    //AddNewColumn(row0, "Expedia Representative (Internal use only)", ref colIndex, style1); // Apartment

                    //rowIndex++;
                    //colIndex = 0;
                    //row0 = sheet0.CreateRow(rowIndex);

                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    //AddNewColumn(row0, "Account Manager:", ref colIndex, style2);
                    //AddNewColumn(row0, "", ref colIndex, style2);

                    //rowIndex++;
                    //colIndex = 0;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    //AddNewColumn(row0, "Office:", ref colIndex, style2);
                    //AddNewColumn(row0, "", ref colIndex, style2);

                    //rowIndex++;
                    //colIndex = 0;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);


                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //NPOI.SS.Util.CellRangeAddress cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 4);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "General Information", ref colIndex, style1);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 1);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "Connectivity (Internal use only)", ref colIndex, style1);

                    //rowIndex++;
                    //colIndex = 0;
                    //row0 = sheet0.CreateRow(rowIndex);

                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    //// First row to last row & first cell to last cell
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    //sheet0.AddMergedRegion(cra1);

                    //AddNewColumn(row0, "Company Name:", ref colIndex, style2);

                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "", ref colIndex, style2);

                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);

                    //AddNewColumn(row0, "Do you need a bulk switch? (Y/N)", ref colIndex, style2);
                    //AddNewColumn(row0, "", ref colIndex, style2);


                    //colIndex = 0;
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "Company Country:", ref colIndex, style2);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "", ref colIndex, style2);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "Name of provider + Internal ID:", ref colIndex, style2);
                    //AddNewColumn(row0, "Name:          ID:    ", ref colIndex, style2);


                    //rowIndex++;
                    //colIndex = 0;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 4);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "Partner´s Contact Person", ref colIndex, style1);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 1);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "General Configuration  (Internal use only)", ref colIndex, style1);


                    //colIndex = 0;
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "Name :", ref colIndex, style2);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, " ", ref colIndex, style2);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "(Existing or New) Name of partner?", ref colIndex, style2);
                    //AddNewColumn(row0, " ", ref colIndex, style2);


                    //colIndex = 0;
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "Email:", ref colIndex, style2);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "", ref colIndex, style2);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "(Existing or New)  Internal Brand:", ref colIndex, style2);
                    //AddNewColumn(row0, " ", ref colIndex, style2);



                    //colIndex = 0;
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 2);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "Telephone:", ref colIndex, style2);
                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 3);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, " ", ref colIndex, style2);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "", ref colIndex, filterLabelCellStyle);
                    //AddNewColumn(row0, "(Existing or New)  Group Login:", ref colIndex, style2);
                    //AddNewColumn(row0, " ", ref colIndex, style2);



                    ////  updated on 09 March

                    //rowIndex++;
                    //colIndex = 8;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);

                    //cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 1);
                    //sheet0.AddMergedRegion(cra1);
                    //AddNewColumn(row0, "Direct Debit ", ref colIndex, headerLabelCellStyle);


                    //colIndex = 8;
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, " Do you needDirect Debit? (Y/N)", ref colIndex, style2);
                    //AddNewColumn(row0, " ", ref colIndex, style2);


                    //colIndex = 8;
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, " Contact Person (New Partner) ", ref colIndex, style2);
                    //AddNewColumn(row0, " ", ref colIndex, style2);



                    //colIndex = 8;
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);

                    //AddNewColumn(row0, " Email ", ref colIndex, style2);
                    //AddNewColumn(row0, " ", ref colIndex, style2);

                    //colIndex = 8;
                    //rowIndex++;
                    //row0 = sheet0.CreateRow(rowIndex);
                    //AddNewColumn(row0, " Phone Number ", ref colIndex, style2);
                    //AddNewColumn(row0, " ", ref colIndex, style2);


                    //#endregion

                    #endregion

                    rowIndex = 0;
                    colIndex = 0;

                    #region Second Sheet  Property Information

                    row1 = sheet1.CreateRow(rowIndex);
                    AddNewColumn(row1, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Property URL", ref colIndex, headerLabelCellStyle);
                    //    AddNewColumn(row1, "VAT number", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Property Name", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Address", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "City", ref colIndex, headerLabelCellStyle);

                    AddNewColumn(row1, "Entrance Latitude", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Entrance Longitude", ref colIndex, headerLabelCellStyle);

                    // updated 09 March 2016
                    AddNewColumn(row1, "Center Latitude", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Center Longitude", ref colIndex, headerLabelCellStyle);
                    //

                    AddNewColumn(row1, "Zipcode", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Country code", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Accommodation Type ", ref colIndex, headerLabelCellStyle); // Apartment/Villa
                    AddNewColumn(row1, "Number of Rooms", ref colIndex, headerLabelCellStyle); // default 1
                    AddNewColumn(row1, "Currency Code", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Check in from", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Check in until", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Check out from", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Check out until", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Accepted Credit Cards", ref colIndex, headerLabelCellStyle);

                    // General Facilities - propety Level 

                    AddNewColumn(row1, "Rooms / facilities for disabled", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Private beach area", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Beach Front", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Bikes available (free)", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row1, "Heating", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row1, "Air Conditioning", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row1, "Garden", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row1, "Terrace", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row1, "Children play Ground", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Barbecue facilities", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Table tennis", ref colIndex, headerLabelCellStyle);

                    AddNewColumn(row1, "Hot tub(outdoor)", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Outdoor swimming pool", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Indoor swimming pool", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Fitness room", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Sauna", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Solarium", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Turkish / steam bath", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Billiard", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Darts", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Game room", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Library", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Safe deposit box", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Ski storage", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Ski-to-door access", ref colIndex, headerLabelCellStyle);

                    //  Private Facilities - Room/Unit level

                    AddNewColumn(row1, "Room/ Unit type", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Room / Property Name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row1, "Number of bedrooms", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row1, "Number of bathrooms", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Max. persons to stay in the property", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Minimum Size of Room/Unit", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Size Unit", ref colIndex, headerLabelCellStyle);// Default Sq. Mt
                    AddNewColumn(row1, "Entire unit wheelchair accessible", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Air-conditioning", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Fan", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Smoking", ref colIndex, headerLabelCellStyle);  // smoking, non-smoking, unknown
                    AddNewColumn(row1, "Detached", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Semi-detached", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Private apartment in block of apartments", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Entire property on ground floor", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Upper floor reachable by elevator", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Upper floor reachable by stairs only", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row1, "Entire unit wheelchair accessible", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Private entrance", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "heating", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "carpeted floor", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "tiled/marble floor", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "wooden / parquet floor", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "soundproofing", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "safe deposit box", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Lake view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "River view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Sea view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Mountain view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "City view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Garden view", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Pool view", ref colIndex, headerLabelCellStyle);  // 
                    AddNewColumn(row1, "Balcony", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Terrace", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Patio", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "outdoor dining area", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "outdoor furniture", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private barbecue", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private pool", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "bathroom", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "toilet", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "bath", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "shower", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Bath or shower", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "spa bath", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "bidet", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "guest toilet", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private sauna", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "private hot tub", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "hair dryer", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "towels (free)", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "linen (free)", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "Towels/Linens At Surcharge", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "bath-robe", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "slippers", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "free toiletries", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "kitchen", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "kitchenette", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "kitchenware", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "refrigerator", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "stove", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "oven", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "dishwasher", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "coffee machine", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "electric kettle", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "toaster", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "microwave", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "cleaning products", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "children highchair", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "dining area", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "dining table", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "seating area", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "fireplace", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "sofa", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "TV", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "flat-screen TV", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "cable channels", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "satellite channels", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "pay-per-view channels", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "telephone", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "radio", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "CD-player", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "blu-ray player", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "DVD-player", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console - Nintendo Wii", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console - Playstation 2", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console - Playstation 3", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "game console - Xbox 360", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "video games", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "computer", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "ipad", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "ipod docking station", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "laptop", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "laptop safe box", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "desk", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "wardrobe/ closet", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "clothes rack", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "mosquito net", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "ironing facilities", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "washing machine", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "tumble dryer (machine)", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row1, "alarm-clock", ref colIndex, headerLabelCellStyle);

                    #endregion

                    //#region Third  Sheet - Contact Details
                    //rowIndex = 0;
                    //colIndex = 0;
                    //row2 = sheet2.CreateRow(rowIndex);
                    //AddNewColumn(row2, "Property Reference ID", ref colIndex, headerLabelCellStyle);

                    //AddNewColumn(row2, "Primary point of contact name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Primary point of contact email", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Primary point of contact phone", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Primary point of contact fax", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Primary point of contact language", ref colIndex, headerLabelCellStyle);

                    //AddNewColumn(row2, "Reservations contact name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Reservations contact email", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Reservations contact phone", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Reservations contact fax", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Reservations contact language", ref colIndex, headerLabelCellStyle);

                    //AddNewColumn(row2, "Central reservations contact name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Central reservations contact email", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Central reservations contact phone", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Central reservations contact fax", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Central reservations contact language", ref colIndex, headerLabelCellStyle);


                    //AddNewColumn(row2, "Invoices contact name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Invoices contact email", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Invoices contact phone", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Invoices contact fax", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Invoices contact language", ref colIndex, headerLabelCellStyle);

                    //AddNewColumn(row2, "Special request contact name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Special request contact email", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Special request contact phone", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Special request contact fax", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Special request contact language", ref colIndex, headerLabelCellStyle);

                    //AddNewColumn(row2, "Availability contact name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Availability contact email", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Availability contact phone", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Availability contact fax", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Availability contact language", ref colIndex, headerLabelCellStyle);

                    //AddNewColumn(row2, "Photos & Descriptions contact name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Photos & Descriptions contact email", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Photos & Descriptions contact phone", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Photos & Descriptions contact fax", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Photos & Descriptions contact language", ref colIndex, headerLabelCellStyle);


                    //AddNewColumn(row2, "Pricing contact name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Pricing contact email", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Pricing contact phone", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Pricing contact fax", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Pricing contact language", ref colIndex, headerLabelCellStyle);

                    //AddNewColumn(row2, "Contracts contact name", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Contracts contact email", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Contracts contact phone", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Contracts contact fax", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row2, "Contracts contact language", ref colIndex, headerLabelCellStyle);
                    //#endregion

                    //#region Fourth  Sheet - Invoicing Details
                    //rowIndex = 0;
                    //colIndex = 0;
                    //row3 = sheet3.CreateRow(rowIndex);
                    ////AddNewColumn(row3, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row3, "Legal company name for invoicing", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row3, "Attention of", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row3, "Legal address", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row3, "Legal zip code", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row3, "Legal city", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row3, "Country", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row3, "VAT or TAX number", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row3, "Preferred invoice medium", ref colIndex, headerLabelCellStyle);

                    //rowIndex++;
                    //row3 = sheet3.CreateRow(rowIndex);
                    //colIndex = 0;
                    ////SetRowCellValue(row3, ref colIndex, tmp_data.id + "");
                    //SetRowCellValue(row3, ref colIndex, "Rental in Rome Srl");
                    //SetRowCellValue(row3, ref colIndex, "Jacopo  Calabrò");
                    //SetRowCellValue(row3, ref colIndex, "Via Appia Nuova 677");
                    //SetRowCellValue(row3, ref colIndex, "00179");
                    //SetRowCellValue(row3, ref colIndex, "Roma");
                    //SetRowCellValue(row3, ref colIndex, "Italy");
                    //SetRowCellValue(row3, ref colIndex, "IT07824541002"); /// Vat Number
                    //SetRowCellValue(row3, ref colIndex, "email"); //  "email" or "postal_mail".

                    //#endregion

                    //#region Fifth  Sheet - Extra Cost & Tax Details
                    //rowIndex = 0;
                    //colIndex = 0;
                    //row4 = sheet4.CreateRow(rowIndex);
                    //AddNewColumn(row4, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "VAT or TAX", ref colIndex, headerLabelCellStyle); // CT_VAT or CT_TAX
                    //AddNewColumn(row4, "VAT/TAX specifies the charge inclusion status", ref colIndex, headerLabelCellStyle); //(Included / Excluded)
                    //AddNewColumn(row4, "VAT/TAX amount ", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "VAT/TAX Mode ", ref colIndex, headerLabelCellStyle);  // NA - not applicable, PC - percentage, IC - incalculable

                    //AddNewColumn(row4, "City TAX ", ref colIndex, headerLabelCellStyle); // CT_CITYTAX
                    //AddNewColumn(row4, "City TAX specifies the charge inclusion status ", ref colIndex, headerLabelCellStyle); // (NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night)
                    //AddNewColumn(row4, "City TAX amount ", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "City TAX Mode", ref colIndex, headerLabelCellStyle);

                    //// Cleaning 
                    //AddNewColumn(row4, "Service Charge Type", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "Service Charge specifies the charge inclusion status", ref colIndex, headerLabelCellStyle); // (Included / Excluded) 
                    //AddNewColumn(row4, "Service Charge amount", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "Service Charge mode", ref colIndex, headerLabelCellStyle);

                    //// Bed  linen
                    //AddNewColumn(row4, "Service Charge Type", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "Service Charge specifies the charge inclusion status", ref colIndex, headerLabelCellStyle); // (Included / Excluded) 
                    //AddNewColumn(row4, "Service Charge amount", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "Service Charge mode", ref colIndex, headerLabelCellStyle);

                    //// Electricity
                    //AddNewColumn(row4, "Service Charge Type", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "Service Charge specifies the charge inclusion status", ref colIndex, headerLabelCellStyle); // (Included / Excluded) 
                    //AddNewColumn(row4, "Service Charge amount", ref colIndex, headerLabelCellStyle);
                    //AddNewColumn(row4, "Service Charge mode", ref colIndex, headerLabelCellStyle);


                    //#endregion

                    //#region Six  Sheet - Policies
                    //rowIndex = 0;
                    //colIndex = 0;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 5);
                    //sheet5.AddMergedRegion(cra);
                    //AddNewColumn(row5, "Please set Cancellation / Deposit and Children Policies below (All the same for ALL IDS)", ref colIndex, style2);


                    //#region Cancellation Policies
                    //rowIndex++;
                    //colIndex = 0;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //AddNewColumn(row5, "Cancellation Policies", ref colIndex, headerLabelCellStyle);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 5);
                    //sheet5.AddMergedRegion(cra);
                    //AddNewColumn(row5, "Please select the correct sentence that Applies", ref colIndex, headerLabelCellStyle);

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    ////AddNewColumn(row5, "Cancellation Policies", ref colIndex, headerLabelCellStyle);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, "If cancelled or modified until _____________________ o' clock before date of arrival,  the ______________ will be charged ");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, "If cancelled or modified until _____________________ o' clock before date of arrival,  the ______________ No Fee will be charged  ");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, " If cancelled or modified up to _____________________ days before date of arrival, the ______________ will be charged ");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, "If cancelled or modified up to _____________________ days before date of arrival, the ______________ No Fee will be charged");


                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, "If cancelled or modified later or in case of no-show, XX percent of the_______reservation will be charged ");


                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, " If cancelled or modified later or in case of no-show,  No fee will be charged ");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, " Please write here any other policy needed ");

                    //#endregion


                    //#region Deposit Policies
                    //rowIndex++;
                    //colIndex = 0;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //AddNewColumn(row5, "Deposit Policies", ref colIndex, headerLabelCellStyle);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 5);
                    //sheet5.AddMergedRegion(cra);
                    //AddNewColumn(row5, "Please select the correct sentence that Applies", ref colIndex, headerLabelCellStyle);

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    ////AddNewColumn(row5, "Cancellation Policies", ref colIndex, headerLabelCellStyle);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, "No deposit will be charged. ");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, " _____________________ of the total amount  will be charged on the day of booking ");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, " _____________________ of the  first night will be charged on the day of booking  ");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, " _______________________ will be charged  XX days prior to the arrivals ");


                    //#endregion


                    //#region Children Policies
                    //rowIndex++;
                    //colIndex = 0;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //AddNewColumn(row5, "Children Policies", ref colIndex, headerLabelCellStyle);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 5);
                    //sheet5.AddMergedRegion(cra);
                    //AddNewColumn(row5, "Please select the correct sentence that Applies", ref colIndex, headerLabelCellStyle);

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    ////AddNewColumn(row5, "Cancellation Policies", ref colIndex, headerLabelCellStyle);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, "All children are welcome");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, "____________ child(ren) under  ________________ years stays free of charge when using existing beds");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, " ________________ older child(ren)or adult(s) will be charged  ________________ per person per night for extra bed(s) ");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, " The maximum number of extra bed(s) in a room is _________________________ ");

                    //rowIndex++;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 5);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 0;
                    //SetRowCellValue(row5, ref colIndex, " Please write here any other policy needed");

                    //#endregion

                    //rowIndex++;
                    //colIndex = 0;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + 5);
                    //sheet5.AddMergedRegion(cra);
                    //AddNewColumn(row5, " Please specify below the Internet, Parking, and Pets policies that apply for each property ", ref colIndex, style2);

                    //rowIndex++;
                    //colIndex = 0;
                    //row5 = sheet5.CreateRow(rowIndex);
                    //AddNewColumn(row5, " ", ref colIndex, style1);


                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 1, 4);
                    //sheet5.AddMergedRegion(cra);
                    //AddNewColumn(row5, "INTERNET ", ref colIndex, headerLabelCellStyle);


                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 5, 10);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 5;
                    //AddNewColumn(row5, "PARKING ", ref colIndex, headerLabelCellStyle);


                    //cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 11, 12);
                    //sheet5.AddMergedRegion(cra);
                    //colIndex = 11;
                    //AddNewColumn(row5, "PETS ", ref colIndex, headerLabelCellStyle);

                    //rowIndex++;
                    //colIndex = 0;
                    //row5 = sheet5.CreateRow(rowIndex);

                    //AddNewColumn(row5, "Property Reference ID", ref colIndex, style2);

                    //AddNewColumn(row5, "Do guests have access to the Internet?", ref colIndex, style2); // ( Yes / No)
                    //AddNewColumn(row5, "Which kind of internet access is available?", ref colIndex, style2);
                    //AddNewColumn(row5, "Where is internet available?", ref colIndex, style2); //NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night, PC - percentage, PPNR - per person per night restricted, IC - incalculable
                    //AddNewColumn(row5, "How much do you charge for internet?", ref colIndex, style2); // (Included / Excluded) 

                    //AddNewColumn(row5, "Are parking spaces available for guests?", ref colIndex, style2);
                    //AddNewColumn(row5, "Where is the parking located? ", ref colIndex, style2);
                    //AddNewColumn(row5, "Private or public parking?", ref colIndex, style2);  // (Included / Optional / Mandatory / Done by guest)
                    //AddNewColumn(row5, "How much do you charge for parking?", ref colIndex, style2);
                    //AddNewColumn(row5, "Time frame of the parking cost", ref colIndex, style2); // (NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night, PC - percentage, PPNR - per person per night restricted) 
                    //AddNewColumn(row5, "Is it necessary to make a reservation for parking?", ref colIndex, style2);

                    //AddNewColumn(row5, "Are pets allowed? ", ref colIndex, style2);
                    //AddNewColumn(row5, "Is there an additional charge for pets?", ref colIndex, style2);     // yes/no

                    //#endregion

                    #region Seventh  Sheet - Photos
                    rowIndex = 0;
                    colIndex = 0;
                    row6 = sheet6.CreateRow(rowIndex);
                    AddNewColumn(row6, "Property Reference ID", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row6, "Tag of the photo", ref colIndex, headerLabelCellStyle);
                    AddNewColumn(row6, "URL of the photo", ref colIndex, headerLabelCellStyle);
                    #endregion

                    #region Eight Sheet  Bedding

                    rowIndex = 0;
                    colIndex = 0;
                    int maxRoomNumber = 0;
                    row7 = sheet7.CreateRow(rowIndex);
                    rowIndex++;
                    var tmp_row7 = sheet7.CreateRow(rowIndex);
                    rowIndex++;
                    var InternsList = new List<dbRntEstateInternsTB>();
                    using (DCmodRental _dc = new DCmodRental())
                    {
                        List<int> estateIds = estateList.Select(x => x.id).ToList();
                        InternsList = _dc.dbRntEstateInternsTBs.Where(x => x.pidInternsType == "Bedroom" || x.pidInternsType == "Livingroom").ToList();
                        var _tmpList = InternsList.Where(x => estateIds.Contains(x.pidEstate)).GroupBy(x => x.pidEstate).Select(g => new { Id = (int)g.Key, count = g.Count() }).ToList();
                        if (_tmpList != null && _tmpList.Count > 0)
                            maxRoomNumber = _tmpList.Max(x => x.count);
                    }
                    row7 = sheet7.CreateRow(rowIndex);
                    AddNewColumn(row7, "Property Reference ID", ref colIndex, filterLabelCellStyle);
                    AddNewColumn(row7, "Room ID ", ref colIndex, filterLabelCellStyle); // Filled by booking.com

                    for (int i = 1; i <= maxRoomNumber; i++)
                    {
                        NPOI.SS.Util.CellRangeAddress cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex - 1, rowIndex - 1, colIndex, colIndex + 9);
                        sheet7.AddMergedRegion(cra1);
                        int _tmpCol = colIndex;
                        AddNewColumn(tmp_row7, "Room " + i, ref _tmpCol, headerLabelCellStyle);

                        AddNewColumn(row7, "Room type", ref colIndex, filterLabelCellStyle);
                        AddNewColumn(row7, "Private Bathroom", ref colIndex, filterLabelCellStyle);
                        AddNewColumn(row7, "Maximum number of guests", ref colIndex, filterLabelCellStyle);
                        AddNewColumn(row7, "Single Bed", ref colIndex, filterLabelCellStyle);
                        AddNewColumn(row7, "Double Bed", ref colIndex, filterLabelCellStyle);
                        AddNewColumn(row7, "Large Double Bed (Queen)", ref colIndex, filterLabelCellStyle);
                        AddNewColumn(row7, "Extra Large Double Bed (King)", ref colIndex, filterLabelCellStyle);
                        AddNewColumn(row7, "Bunk Bed", ref colIndex, filterLabelCellStyle);
                        AddNewColumn(row7, "Sofa Bed", ref colIndex, filterLabelCellStyle);
                        AddNewColumn(row7, "Futon Mat", ref colIndex, filterLabelCellStyle);
                    }
                    #endregion

                    #endregion

                    DCmodRental DC = new DCmodRental();

                    var listFacilites = DC_Rental.RNT_RL_ESTATE_CONFIG.Where(x => x.is_HomeAway == 0).ToList();
                    var listMedia = DC_Rental.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == "original").ToList();
                    //var lstInternsFeatures = DC.dbRntEstateInternsFeatureVIEWs.Where(x => x.pidLang == App.LangID && x.isActive == 1).ToList();

                    rowIndex = 0;
                    colIndex = 0;
                    int imageIndex = 1;
                    #region Data

                    foreach (var tmp_data in estateList)
                    {
                        # region  Second Sheet - Property Detail

                        var tmpEstatefacility = listFacilites.Where(x => x.pid_estate == tmp_data.id).ToList();
                        row1 = sheet1.CreateRow(rowIndex + 1);
                        colIndex = 0;
                        SetRowCellValue(row1, ref colIndex, tmp_data.id + "");

                        SetRowCellValue(row1, ref colIndex, "");
                        //   SetRowCellValue(row1, ref colIndex, "");
                        SetRowCellValue(row1, ref colIndex, tmp_data.code);
                        SetRowCellValue(row1, ref colIndex, tmp_data.loc_address);
                        SetRowCellValue(row1, ref colIndex, AdminUtilities.getCityTitle(tmp_data.pid_city));

                        if (!string.IsNullOrWhiteSpace(tmp_data.google_maps))
                        {
                            if (tmp_data.google_maps.Split('|').Length > 1)
                            {
                                SetRowCellValue(row1, ref colIndex, tmp_data.google_maps.Split('|')[0].Replace(",", "."));
                                SetRowCellValue(row1, ref colIndex, tmp_data.google_maps.Split('|')[1].Replace(",", "."));
                            }
                            else if (tmp_data.google_maps.Split(',').Length > 1)
                            {
                                SetRowCellValue(row1, ref colIndex, tmp_data.google_maps.Split(',')[0]);
                                SetRowCellValue(row1, ref colIndex, tmp_data.google_maps.Split(',')[1]);
                            }
                            else
                            {
                                SetRowCellValue(row1, ref colIndex, "0.00");
                                SetRowCellValue(row1, ref colIndex, "0.00");
                            }
                        }
                        else
                        {
                            SetRowCellValue(row1, ref colIndex, "0.00");
                            SetRowCellValue(row1, ref colIndex, "0.00");
                        }

                        // updaetd 9  march 2016   Center Latitude and Center Longitude
                        SetRowCellValue(row1, ref colIndex, "");
                        SetRowCellValue(row1, ref colIndex, "");
                        //


                        SetRowCellValue(row1, ref colIndex, tmp_data.loc_zip_code + "");
                        SetRowCellValue(row1, ref colIndex, "it");
                        SetRowCellValue(row1, ref colIndex, tmp_data.category == "apt" ? "Apartment" : "Villa");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "EUR");
                        SetRowCellValue(row1, ref colIndex, "14:00"); //checkin from 
                        SetRowCellValue(row1, ref colIndex, "15:00");//checkin until
                        SetRowCellValue(row1, ref colIndex, "11:00"); //check out from 
                        SetRowCellValue(row1, ref colIndex, "00:00");//check out until
                        SetRowCellValue(row1, ref colIndex, "Visa|Mastercard");
                        //  General facilities - Property level
                        SetRowCellValue(row1, ref colIndex, "0"); //Rooms / facilities for disabled

                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 36));
                        //SetRowCellValue(row1, ref colIndex, "1"); // heating 
                        //SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 1));
                        //SetRowCellValue(row1, ref colIndex, "0");
                        //SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 54));
                        //SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 30));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 25));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 22));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");

                        SetRowCellValue(row1, ref colIndex, tmp_data.category == "apt" ? "Apartment" : "Villa");
                        SetRowCellValue(row1, ref colIndex, tmp_data.code);
                        //SetRowCellValue(row1, ref colIndex, tmp_data.num_rooms_bed + "");
                        //SetRowCellValue(row1, ref colIndex, tmp_data.num_rooms_bath + "");
                        SetRowCellValue(row1, ref colIndex, tmp_data.num_persons_max + "");
                        SetRowCellValue(row1, ref colIndex, tmp_data.mq_inner + "");
                        SetRowCellValue(row1, ref colIndex, "2");
                        SetRowCellValue(row1, ref colIndex, "0"); // WheelChair 



                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 1));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "unknown");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        //SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1"); // heating
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 22));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");// City View
                        SetRowCellValue(row1, ref colIndex, "0"); // Garden
                        //SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 54));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 54));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");

                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 25));
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1");

                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 46));
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "1");//Towel
                        SetRowCellValue(row1, ref colIndex, "0");//Linen
                        SetRowCellValue(row1, ref colIndex, "0");// Towel & Linen at Surcharge
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 32));
                        SetRowCellValue(row1, ref colIndex, "1");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 18));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 15));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 47));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 52));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 19));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 35));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 20));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 8));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");

                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 12));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 49));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 34));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 14));
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, "0");
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 48));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 6));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 53));
                        SetRowCellValue(row1, ref colIndex, hasFacilities(tmpEstatefacility, 24));

                        #endregion

                        //#region Third Sheet Contact Details

                        //row2 = sheet2.CreateRow(rowIndex + 1);
                        //colIndex = 0;
                        //SetRowCellValue(row2, ref colIndex, tmp_data.id + "");

                        //// Primary point of contact
                        //SetRowCellValue(row2, ref colIndex, "Matteo Calabrò");
                        //SetRowCellValue(row2, ref colIndex, "matteo.calabro@rentalinrome.com");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068"); // fax
                        //SetRowCellValue(row2, ref colIndex, "IT");

                        //// Reservations Contact
                        //SetRowCellValue(row2, ref colIndex, "Francesca Sangiorgio ");
                        //SetRowCellValue(row2, ref colIndex, "reception@rentalinrome.com");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068"); // fax
                        //SetRowCellValue(row2, ref colIndex, "IT");

                        //// Central reservations Contact
                        //SetRowCellValue(row2, ref colIndex, "Matteo Calabrò");
                        //SetRowCellValue(row2, ref colIndex, "matteo.calabro@rentalinrome.com");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068"); // fax
                        //SetRowCellValue(row2, ref colIndex, "IT");


                        //// invocie contact
                        //SetRowCellValue(row2, ref colIndex, "Matteo Calabrò");
                        //SetRowCellValue(row2, ref colIndex, "matteo.calabro@rentalinrome.com");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068"); // fax
                        //SetRowCellValue(row2, ref colIndex, "IT");



                        ////  Special requests Contact
                        //SetRowCellValue(row2, ref colIndex, "Matteo Calabrò");
                        //SetRowCellValue(row2, ref colIndex, "matteo.calabro@rentalinrome.com");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068"); // fax
                        //SetRowCellValue(row2, ref colIndex, "IT");

                        ////    Availability Contact
                        //SetRowCellValue(row2, ref colIndex, "Matteo Calabrò");
                        //SetRowCellValue(row2, ref colIndex, "matteo.calabro@rentalinrome.com");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068"); // fax
                        //SetRowCellValue(row2, ref colIndex, "IT");

                        ////     Photos & Descriptions Contact
                        //SetRowCellValue(row2, ref colIndex, "Matteo Calabrò");
                        //SetRowCellValue(row2, ref colIndex, "matteo.calabro@rentalinrome.com");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068"); // fax
                        //SetRowCellValue(row2, ref colIndex, "IT");


                        ////     Pricing  Contact
                        //SetRowCellValue(row2, ref colIndex, "Matteo Calabrò");
                        //SetRowCellValue(row2, ref colIndex, "matteo.calabro@rentalinrome.com");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068"); // fax
                        //SetRowCellValue(row2, ref colIndex, "IT");

                        //// Contracts contact
                        //SetRowCellValue(row2, ref colIndex, "Matteo Calabrò");
                        //SetRowCellValue(row2, ref colIndex, "matteo.calabro@rentalinrome.com");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068");
                        //SetRowCellValue(row2, ref colIndex, "+39 06 3220068"); // fax
                        //SetRowCellValue(row2, ref colIndex, "IT");

                        //#endregion

                        //#region Fourth Sheet Invoice  Details

                        //// Common data For all property
                        //#endregion

                        //#region Fifth Sheet Extra cost

                        ////(I - included, X - excluded, N - non mandatory, C - conditional
                        //row4 = sheet4.CreateRow(rowIndex + 1);
                        //colIndex = 0;
                        //SetRowCellValue(row4, ref colIndex, tmp_data.id + "");

                        //SetRowCellValue(row4, ref colIndex, "CT_VAT");
                        //SetRowCellValue(row4, ref colIndex, "I");
                        //SetRowCellValue(row4, ref colIndex, "1.10");
                        //SetRowCellValue(row4, ref colIndex, "PC");


                        //SetRowCellValue(row4, ref colIndex, "CT_CITYTAX");
                        //SetRowCellValue(row4, ref colIndex, "X");
                        //SetRowCellValue(row4, ref colIndex, "3.5"); // city  tax
                        //SetRowCellValue(row4, ref colIndex, "PPN");  // (NA - not applicable, PS - per stay, PPS - per person per stay, PN - per night, PPN - per person per night, PC - percentage, PPNR - per person per night restricted) 


                        //SetRowCellValue(row4, ref colIndex, "CT_CLEANINGFEE");
                        //SetRowCellValue(row4, ref colIndex, "I");
                        //SetRowCellValue(row4, ref colIndex, tmp_data.eco_ext_price.objToDecimal().ToString("N2"));
                        //SetRowCellValue(row4, ref colIndex, "PS");


                        //SetRowCellValue(row4, ref colIndex, "CT_BEDLINEN");
                        //SetRowCellValue(row4, ref colIndex, "I");
                        //SetRowCellValue(row4, ref colIndex, "0.00");
                        //SetRowCellValue(row4, ref colIndex, "NA");


                        //SetRowCellValue(row4, ref colIndex, "CT_ELECTRICITYFEE");
                        //SetRowCellValue(row4, ref colIndex, "I");
                        //SetRowCellValue(row4, ref colIndex, "0.00");
                        //SetRowCellValue(row4, ref colIndex, "NA");

                        //#endregion

                        //#region sixth Sheet Policies

                        //row5 = sheet5.CreateRow(rowIndex + 23);
                        //colIndex = 0;
                        //SetRowCellValue(row5, ref colIndex, tmp_data.id + "");

                        ////SetRowCellValue(row5, ref colIndex, tmp_data.num_persons_child + "");
                        ////SetRowCellValue(row5, ref colIndex, "3");
                        ////SetRowCellValue(row5, ref colIndex, "0");
                        ////SetRowCellValue(row5, ref colIndex, "NA");
                        ////SetRowCellValue(row5, ref colIndex, "0");
                        ////SetRowCellValue(row5, ref colIndex, "NA");

                        //// Internet
                        //SetRowCellValue(row5, ref colIndex, "Yes");
                        //SetRowCellValue(row5, ref colIndex, "WiFi");
                        //SetRowCellValue(row5, ref colIndex, "All areas");
                        //SetRowCellValue(row5, ref colIndex, "Free");

                        //// Parking
                        //SetRowCellValue(row5, ref colIndex, hasFacilities(tmpEstatefacility, 28) == "1" ? "Yes" : "No");
                        //SetRowCellValue(row5, ref colIndex, hasFacilities(tmpEstatefacility, 31) == "1" ? "NearBy" : "On Site");
                        //SetRowCellValue(row5, ref colIndex, "Private"); // 
                        //SetRowCellValue(row5, ref colIndex, "Free");
                        //SetRowCellValue(row5, ref colIndex, "NA");
                        //SetRowCellValue(row5, ref colIndex, "No");

                        ////pets
                        //SetRowCellValue(row5, ref colIndex, hasFacilities(tmpEstatefacility, 4) == "1" ? "Allowed" : "Not allowed");
                        //SetRowCellValue(row5, ref colIndex, "NA");

                        //#endregion

                        #region Seven Sheet Photos

                        var lstEstatePhotos = listMedia.Where(x => x.pid_estate == tmp_data.id).OrderBy(x => x.sequence).ToList();
                        foreach (RNT_RL_ESTATE_MEDIA image in lstEstatePhotos)
                        {
                            row6 = sheet6.CreateRow(imageIndex);
                            colIndex = 0;
                            SetRowCellValue(row6, ref colIndex, tmp_data.id + "");
                            SetRowCellValue(row6, ref colIndex, string.IsNullOrWhiteSpace(image.code) ? "Other" : image.code);
                            SetRowCellValue(row6, ref colIndex, App.HOST + App.RP + image.img_banner);
                            imageIndex++;
                        }


                        #endregion

                        #region Eight Sheet Bedding

                        row7 = sheet7.CreateRow(rowIndex + 3);
                        colIndex = 0;
                        var currInternsList = InternsList.Where(x => x.pidEstate == tmp_data.id).ToList();

                        var FeatureRL = DC.dbRntEstateInternsFeatureRLs.ToList();
                        SetRowCellValue(row7, ref colIndex, tmp_data.id + "");
                        SetRowCellValue(row7, ref colIndex, ""); //Room ID by booking.com

                        foreach (dbRntEstateInternsTB currIntern in currInternsList)
                        {
                            var tmpFeaturesList = FeatureRL.Where(x => x.pidEstateInterns == currIntern.id).ToList();
                            SetRowCellValue(row7, ref colIndex, currIntern.pidInternsType); //Room type

                            SetRowCellValue(row7, ref colIndex, "0"); //Private Bathroom
                            SetRowCellValue(row7, ref colIndex, "0"); //Maximum number of guests

                            // Note : Below Hard coded code are as per this list , 
                            //  http://magarental.cloud.magadesign.net/admin/modRental/EstateInternsFeatureList.aspx
                            // It Must be updated accrodingly for individual Project 
                            if (currIntern.pidInternsType == "Bedroom")
                            {
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 1) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 1).count.objToInt32() + ""); //Single Bed
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 3) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 3).count.objToInt32() + ""); //Double Bed
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 7) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 7).count.objToInt32() + ""); //Large Double Bed (Queen)
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 5) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 5).count.objToInt32() + ""); //Extra Large Double Bed (King)
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 11) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 11).count.objToInt32() + ""); //Bunk Bed
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 9) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 9).count.objToInt32() + ""); //"Sofa Bed
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 13) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 13).count.objToInt32() + ""); //Futon Mat

                            }
                            else if (currIntern.pidInternsType == "Livingroom")
                            {
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 2) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 2).count.objToInt32() + ""); //Single Bed
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 4) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 4).count.objToInt32() + ""); //Double Bed
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 8) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 8).count.objToInt32() + ""); //Large Double Bed (Queen)
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 6) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 6).count.objToInt32() + ""); //Extra Large Double Bed (King)
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 12) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 12).count.objToInt32() + ""); //Bunk Bed
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 10) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 10).count.objToInt32() + ""); //"Sofa Bed
                                SetRowCellValue(row7, ref colIndex, tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 14) == null ? "0" : +tmpFeaturesList.SingleOrDefault(x => x.pidInternsFeature == 14).count.objToInt32() + ""); //Futon Mat

                            }
                        }

                        #endregion
                        rowIndex++;
                    }
                    #endregion

                    //for (int c = 0; c < 12; c++)
                    //{
                    //    sheet0.AutoSizeColumn(c, true);
                    //    sheet1.AutoSizeColumn(c, true);
                    //    sheet2.AutoSizeColumn(c, true);
                    //    sheet3.AutoSizeColumn(c, true);
                    //    sheet4.AutoSizeColumn(c, true);
                    //    sheet5.AutoSizeColumn(c, true);
                    //    sheet6.AutoSizeColumn(c, true);
                    //}

                    string path = Server.MapPath("/files");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string saveAsFileName = filename + ".xls";
                    string filePath = Path.Combine(path, saveAsFileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(stream);
                        Response.Clear();
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                    }
                    Response.WriteFile(filePath);
                    Response.Flush();
                    File.Delete(filePath);
                    Response.End();
                    return true;
                }
                return
                    false;
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ExpediaExport", ex.Message);
                return false;
            }
        }

        public void SetRowCellValue(Row _currentRow, ref int colIndex, string value)
        {
            var objcell = _currentRow.CreateCell(colIndex);
            objcell.SetCellValue(value);
            colIndex++;
        }

        public string hasFacilities(List<RNT_RL_ESTATE_CONFIG> _estateFacilities, int facilityID)
        {
            return _estateFacilities.Any(x => x.pid_config == facilityID) ? "1" : "0";
        }

        public void AddNewColumn(Row _currentRow, string title, ref int index, CellStyle _currStyle, bool isFillcolor = false)
        {
            var objcell = _currentRow.CreateCell(index);
            objcell.SetCellValue(title);
            objcell.CellStyle = _currStyle;
            if (isFillcolor)
            {
                _currStyle.FillBackgroundColor = IndexedColors.ORANGE.Index;
                objcell.CellStyle.Alignment = HorizontalAlignment.LEFT;
            }
            else
                objcell.CellStyle.Alignment = HorizontalAlignment.CENTER;

            index++;
        }
        public void addImageToSheet(Sheet _sheet, Workbook _wb)
        {
            string imagesPath = "/images/logo-booking.png";

            //create an image from the path
            System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(imagesPath));
            MemoryStream ms = new MemoryStream();
            //pull the memory stream from the image (I need this for the byte array later)
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //the drawing patriarch will hold the anchor and the master information
            Drawing patriarch = _sheet.CreateDrawingPatriarch();
            //store the coordinates of which cell and where in the cell the image goes
            int intx1 = 1;
            int inty1 = 1;
            int intx2 = 4;
            int inty2 = 5;

            HSSFClientAnchor anchor = new HSSFClientAnchor(30, 30, 60, 30, intx1, inty1, intx2, inty2);
            //types are 0, 2, and 3. 0 resizes within the cell, 2 doesn’t
            anchor.AnchorType = 2;
            //add the byte array and encode it for the excel file
            int index = _wb.AddPicture(ms.ToArray(), PictureType.PNG);
            Picture signaturePicture = patriarch.CreatePicture(anchor, index);
        }
    }
}