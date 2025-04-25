using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;
using Telerik.Web.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Globalization;
using NPOI.SS.Util;

namespace MagaRentalCE.admin.modRental
{
    public partial class ReservationOccupancyReport : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind_drp_flt_city();
                Bind_flt_zone();
                Bind_Estate();
                Bind_drp_owner();
                Bind_drp_flt_Agency();
                bind_drp_columns();
                Bind_Year();
                checkAllItems(ref drp_flt_Zone);
                drp_months.SelectedValue = DateTime.Now.Month + "";
                drp_months.SelectedItem.Checked = true;
                //checkAllItems(ref drp_months);
            }
            else
            {
                if (Request["__EVENTARGUMENT"] == "drp_city_ItemChecked")
                {
                    Bind_flt_zone();
                    Bind_Estate();
                    Bind_drp_owner();
                }
                if (Request["__EVENTARGUMENT"] == "drp_zone_ItemChecked")
                {
                    Bind_Estate();
                    Bind_drp_owner();
                }
                if (Request["__EVENTARGUMENT"] == "drp_estate_ItemChecked")
                {
                    Bind_drp_owner();
                }
            }
        }

        protected void Bind_Estate()
        {
            drp_estate.Items.Clear();
            List<int> selectedCity = drp_flt_city.CheckedItems.Where(x => x.Value.objToInt32() > 0).Select(x => x.Value.ToInt32()).ToList();
            List<int> selectedZones = drp_flt_Zone.CheckedItems.Where(x => x.Value.objToInt32() > 0).Select(x => x.Value.ToInt32()).ToList();

            var estateList = AppSettings.RNT_TB_ESTATE.ToList();

            if (selectedCity.Count > 0)
                estateList = estateList.Where(x => x.pid_city.HasValue && selectedCity.Contains(x.pid_city.Value)).ToList();

            if (selectedZones.Count > 0)
                estateList = estateList.Where(x => x.pid_zone.HasValue && selectedZones.Contains(x.pid_zone.Value)).ToList();

            drp_estate.DataSource = estateList.OrderBy(x => x.code).ToList();
            drp_estate.DataTextField = "code";
            drp_estate.DataValueField = "id";
            drp_estate.DataBind();
            checkAllItems(ref drp_estate);
        }

        private void Bind_drp_flt_Agency()
        {
            var _list = rntProps.AgentTBL.Where(x => x.isActive == 1).OrderBy(x => x.nameCompany).ToList();
            drp_flt_agency.DataSource = _list;
            drp_flt_agency.DataTextField = "nameCompany";
            drp_flt_agency.DataValueField = "id";
            drp_flt_agency.DataBind();
            drp_flt_agency.Items.Insert(0, new ListItem("-- Without all Agency --", "-1"));
            drp_flt_agency.Items.Insert(0, new ListItem("-- With all Agency --", "-2"));
            drp_flt_agency.Items.Insert(0, new ListItem("-- All --", "0"));
            drp_flt_agency.SelectedValue = "0";
        }

        private void Bind_Year()
        {
            drp_flt_year.Items.Clear();
            using (magaRental_DataContext DC_Rental = new magaRental_DataContext())
            {
                var Yearlist = DC_Rental.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.dtStart.HasValue && x.dtEnd.HasValue).Select(x => x.dtStart.Value.Year).Distinct().ToList();
                foreach (int y in Yearlist)
                {
                    drp_flt_year.Items.Add(new RadComboBoxItem(y + "", y + ""));
                }
                if (Yearlist.Contains(DateTime.Now.Year))
                {
                    drp_flt_year.SelectedValue = DateTime.Now.Year + "";
                    drp_flt_year.SelectedItem.Checked = true;
                }              
                
                //drp_flt_year.Items.Insert(0, new RadComboBoxItem("-- All --", "0"));
            }

        }

        private void Bind_drp_flt_city()
        {
            drp_flt_city.DataSource = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.pid_lang == App.LangID && x.is_active == 1).OrderBy(x => x.title);
            drp_flt_city.DataTextField = "title";
            drp_flt_city.DataValueField = "id";
            drp_flt_city.DataBind();

            drp_flt_city.SelectedValue = "1";
            drp_flt_city.SelectedItem.Checked = true;          
        }

        protected void Bind_flt_zone()
        {
            drp_flt_Zone.Items.Clear();
            List<int> selectedCity = drp_flt_city.CheckedItems.Where(x => x.Value.objToInt32() > 0).Select(x => x.Value.ToInt32()).ToList();
            var zoneList = AppSettings.LOC_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == App.LangID).ToList();
            if (selectedCity.Count > 0)
                zoneList = zoneList.Where(x => selectedCity.Contains(x.pid_city.Value)).OrderBy(x => x.title).ToList();
            else
                return;
            drp_flt_Zone.DataSource = zoneList;
            drp_flt_Zone.DataTextField = "title";
            drp_flt_Zone.DataValueField = "id";
            drp_flt_Zone.DataBind();
            checkAllItems(ref drp_flt_Zone);
        }

        private void Bind_drp_owner()
        {
            List<int> selectedEstates = new List<int>();
            selectedEstates = drp_estate.CheckedItems.Where(x => x.Value.objToInt32() > 0).Select(x => x.Value.ToInt32()).ToList();

            List<int> owners = AppSettings.RNT_TB_ESTATE.Where(x => selectedEstates.Contains(x.id) && x.pid_owner.HasValue).Select(x => x.pid_owner.Value).ToList();

            List<USR_TBL_OWNER> list = maga_DataContext.DC_USER.USR_TBL_OWNER.Where(x => x.is_active == 1 && (x.is_deleted == 0 || x.is_deleted == null)).OrderBy(x => x.name_full).ToList();
            if (owners.Count > 0)
                list = list.Where(x => owners.Contains(x.id)).ToList();

            drp_flt_Owner.Items.Clear();
            drp_flt_Owner.DataSource = list;
            drp_flt_Owner.DataTextField = "name_full";
            drp_flt_Owner.DataValueField = "id";
            drp_flt_Owner.DataBind();
            checkAllItems(ref drp_flt_Owner);
        }

        protected void drp_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_flt_zone();
        }

        private void bind_drp_columns()
        {
            drp_columns.Items.Add(new RadComboBoxItem(contUtils.getLabel("%"), "1"));
            drp_columns.Items.Add(new RadComboBoxItem(contUtils.getLabel("lblDays"), "2"));
            drp_columns.Items.Add(new RadComboBoxItem(contUtils.getLabel("lbl_AmountMix"), "3"));
            drp_columns.Items.Add(new RadComboBoxItem(contUtils.getLabel("lblWholeNumRes"), "4"));
            drp_columns.Items.Add(new RadComboBoxItem(contUtils.getLabel("lblAmountWhole"), "5"));
            drp_columns.Items.Add(new RadComboBoxItem(contUtils.getLabel("lblCreatedNumRes"), "6"));
            drp_columns.Items.Add(new RadComboBoxItem(contUtils.getLabel("lblCreatedResAmount"), "7"));
            checkAllItems(ref drp_columns);
        }

        private void checkAllItems(ref RadComboBox drp)
        {
            foreach (RadComboBoxItem itm in drp.Items)
            {
                itm.Checked = true;
            }
        }

        protected void lnk_export_excel_Click(object sender, EventArgs e)
        {
            #region Get Filters
            var cities = drp_flt_city.CheckedItems.Select(x => x.Value.objToInt32()).ToList();
            var zones = drp_flt_Zone.CheckedItems.Select(x => x.Value.objToInt32()).ToList();
            var estates = drp_estate.CheckedItems.Select(x => x.Value.objToInt32()).ToList();
            var months = drp_months.CheckedItems.Select(x => x.Value.objToInt32()).ToList();
            var owners = drp_flt_Owner.CheckedItems.Select(x => x.Value.objToInt32()).ToList();
            int agent = drp_flt_agency.getSelectedValueInt();
            var years = drp_flt_year.CheckedItems.Select(x => x.Value.objToInt32()).ToList();
            var columns = drp_columns.CheckedItems.Select(x => x.Value.objToInt32()).ToList();
            #endregion

            #region Filter Result
            var resList = new List<RNT_TBL_RESERVATION>();
            resList = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.dtStart.HasValue && x.dtEnd.HasValue).ToList();

            var estateList = AppSettings.RNT_TB_ESTATE.ToList();
            if (!estates.Contains(0))
            {
                estateList = estateList.Where(x => estates.Contains(x.id)).ToList();
            }

            if (!cities.Contains(0))
            {
                estateList = estateList.Where(x => x.pid_city.HasValue && cities.Contains(x.pid_city.Value)).ToList();
            }

            if (zones.Count > 0 && !zones.Contains(0))
            {
                estateList = estateList.Where(x => x.pid_zone.HasValue && zones.Contains(x.pid_zone.Value)).ToList();
            }

            if (!owners.Contains(0))
            {
                estateList = estateList.Where(x => x.pid_owner.HasValue && owners.Contains(x.pid_owner.Value)).ToList();
            }

            if (months.Count == 0 || months.Contains(0))
            {
                months.Clear();
                // add all months 
                for (int i = 1; i <= 12; i++)
                {
                    months.Add(i);
                }
            }
            if (years.Count == 0)
            {
                years.Add(DateTime.Now.Year);
            }
            #endregion

            #region create excel output
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("OccupancyReport");

            #region header
            var headerLabelCellStyle = workbook.CreateCellStyle();
            var headerLabelFont = workbook.CreateFont();
            headerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerLabelCellStyle.SetFont(headerLabelFont);
            headerLabelCellStyle.Alignment = HorizontalAlignment.CENTER;

            var leftAlginCellStyle = workbook.CreateCellStyle();
            leftAlginCellStyle.Alignment = HorizontalAlignment.LEFT;

            var rightAlginCellStyle = workbook.CreateCellStyle();
            rightAlginCellStyle.Alignment = HorizontalAlignment.RIGHT;

            var rightAlginTotalCellStyle = workbook.CreateCellStyle();
            var LabelFont = workbook.CreateFont();
            LabelFont.Boldweight = (short)FontBoldWeight.BOLD;
            rightAlginTotalCellStyle.SetFont(LabelFont);
            rightAlginTotalCellStyle.Alignment = HorizontalAlignment.RIGHT;

            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);

            var cell = row.CreateCell(0);
            cell.SetCellValue("Apartment");
            cell.CellStyle = headerLabelCellStyle;
            CellRangeAddress craRow = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + 1, 0, 0);
            sheet.AddMergedRegion(craRow);
            sheet.SetColumnWidth(0, 35 * 256);

            int cnt = 1;
            foreach (int month in months)
            {
                foreach (int year in years)
                {

                    cell = row.CreateCell(cnt);
                    cell.SetCellValue(CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month) + " " + year);
                    cell.CellStyle = headerLabelCellStyle;
                    cell.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                    CellRangeAddress cra = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, cnt, ((cnt + columns.Count) - 1));
                    sheet.AddMergedRegion(cra);
                    cnt = cnt + columns.Count;

                }
            }

            rowIndex++;
            row = sheet.CreateRow(rowIndex);

            int columnCnt = 1;
            foreach (int month in months)
            {
                foreach (int year in years)
                {
                    //1 - percentage
                    if (columns.Contains(1))
                    {
                        cell = row.CreateCell(columnCnt);
                        cell.SetCellValue("%");
                        cell.CellStyle = headerLabelCellStyle;
                        columnCnt++;
                    }
                    //2 - Number of booked days in month
                    if (columns.Contains(2))
                    {
                        cell = row.CreateCell(columnCnt);
                        cell.SetCellValue(contUtils.getLabel("lblDays"));
                        cell.CellStyle = headerLabelCellStyle;
                        columnCnt++;
                    }
                    //3 - AMount according to Number of booked days in month - Amount mix
                    if (columns.Contains(3))
                    {
                        cell = row.CreateCell(columnCnt);
                        cell.SetCellValue(contUtils.getLabel("lbl_AmountMix"));
                        cell.CellStyle = headerLabelCellStyle;
                        sheet.SetColumnWidth(columnCnt, 15 * 256);
                        columnCnt++;
                    }
                    //4 - Number of reservations of which start date is in current month
                    if (columns.Contains(4))
                    {
                        cell = row.CreateCell(columnCnt);
                        cell.SetCellValue(contUtils.getLabel("lblWholeNumRes"));
                        cell.CellStyle = headerLabelCellStyle;
                        //sheet.SetColumnWidth(columnCnt, 20 * 256);
                        columnCnt++;
                    }
                    //5 - Total Reservation amount of which start date is in current month
                    if (columns.Contains(5))
                    {
                        cell = row.CreateCell(columnCnt);
                        cell.SetCellValue(contUtils.getLabel("lblAmountWhole"));
                        cell.CellStyle = headerLabelCellStyle;
                        sheet.SetColumnWidth(columnCnt, 12 * 256);
                        columnCnt++;
                    }
                    //6 - Number of reservations of which creation date is in current month
                    if (columns.Contains(6))
                    {
                        cell = row.CreateCell(columnCnt);
                        cell.SetCellValue(contUtils.getLabel("lblCreatedNumRes"));
                        cell.CellStyle = headerLabelCellStyle;
                        sheet.SetColumnWidth(columnCnt, 15 * 256);
                        columnCnt++;
                    }
                    //5 - Total Reservation amount of which creation date is in current month
                    if (columns.Contains(7))
                    {
                        cell = row.CreateCell(columnCnt);
                        cell.SetCellValue(contUtils.getLabel("lblCreatedResAmount"));
                        cell.CellStyle = headerLabelCellStyle;
                        sheet.SetColumnWidth(columnCnt, 25 * 256);
                        columnCnt++;
                    }
                }
            }
            #endregion

            #region middle data
            List<rntOccupancyReportUtils.Total> lstTotal = new List<rntOccupancyReportUtils.Total>();
            foreach (RNT_TB_ESTATE objEstate in estateList)
            {
                rowIndex++;
                row = sheet.CreateRow(rowIndex);

                decimal currPercentage = 0;
                int currDays = 0;
                int totalMonthdays = 0;

                var tmpResList = resList.Where(x => x.pid_estate == objEstate.id).ToList();
                if (agent != 0)
                {
                    if (agent == -2)
                        tmpResList = tmpResList.Where(x => x.agentID != null && x.agentID > 0).ToList();
                    else if (agent == -1)
                        tmpResList = tmpResList.Where(x => x.agentID == null || x.agentID == 0).ToList();
                    else
                        tmpResList = tmpResList.Where(x => x.agentID != null && x.agentID == agent).ToList();
                }

                cell = row.CreateCell(0);
                cell.SetCellValue(objEstate.code);
                cell.CellStyle = leftAlginCellStyle;

                int columnCntData = 1;
                foreach (int currMonth in months)
                {
                    foreach (int currYear in years)
                    {
                        totalMonthdays = DateTime.DaysInMonth(currYear, currMonth);
                        var currMonthReservations = tmpResList.Where(x => (x.dtStart.Value.Year == currYear || x.dtEnd.Value.Year == currYear) && (x.dtStart.Value.Month == currMonth || x.dtEnd.Value.Month == currMonth)).ToList();
                        var currDayAmount = rntOccupancyReportUtils.GetTotalBookedDaysPerMonth(tmpResList, currMonth, currYear, totalMonthdays);
                        currDays = currDayAmount.days;
                        currPercentage = decimal.Divide((100 * currDays), totalMonthdays);

                        #region column filter
                        //1 - percentage
                        if (columns.Contains(1))
                        {
                            cell = row.CreateCell(columnCntData);
                            cell.SetCellValue(currPercentage.ToString("N2"));
                            cell.CellStyle = rightAlginCellStyle;
                            columnCntData++;
                        }
                        //2 - Number of booked days in month
                        if (columns.Contains(2))
                        {
                            cell = row.CreateCell(columnCntData);
                            cell.SetCellValue(currDays);
                            cell.CellStyle = rightAlginCellStyle;
                            columnCntData++;
                        }
                        //3 - AMount according to Number of booked days in month - Amount mix
                        if (columns.Contains(3))
                        {
                            cell = row.CreateCell(columnCntData);
                            cell.SetCellValue(currDayAmount.amount.ToString("N2"));
                            cell.CellStyle = rightAlginCellStyle;
                            columnCntData++;
                        }
                        //4 - Number of reservations of which start date is in current month
                        if (columns.Contains(4))
                        {
                            cell = row.CreateCell(columnCntData);
                            cell.SetCellValue(currDayAmount.numResStartDate);
                            cell.CellStyle = rightAlginCellStyle;
                            columnCntData++;
                        }
                        //5 - Total Reservation amount of which start date is in current month
                        if (columns.Contains(5))
                        {
                            cell = row.CreateCell(columnCntData);
                            cell.SetCellValue(currDayAmount.startDateAmount.ToString("N2"));
                            cell.CellStyle = rightAlginCellStyle;
                            columnCntData++;
                        }
                        //6 - Number of reservations of which creation date is in current month
                        if (columns.Contains(6))
                        {
                            cell = row.CreateCell(columnCntData);
                            cell.SetCellValue(currDayAmount.numRescreatedDate);
                            cell.CellStyle = rightAlginCellStyle;
                            columnCntData++;
                        }
                        //5 - Total Reservation amount of which creation date is in current month
                        if (columns.Contains(7))
                        {
                            cell = row.CreateCell(columnCntData);
                            cell.SetCellValue(currDayAmount.createdDateAmount.ToString("N2"));
                            cell.CellStyle = rightAlginCellStyle;
                            columnCntData++;
                        }
                        #endregion

                        #region calculate total
                        var total = lstTotal.FirstOrDefault(x => x.currMonth == currMonth && x.currYear == currYear);
                        if (total == null)
                        {
                            total = new rntOccupancyReportUtils.Total();
                            total.currMonth = currMonth;
                            total.currYear = currYear;
                            lstTotal.Add(total);
                        }
                        total.totnumResStartDate += currDayAmount.numResStartDate.objToInt32();
                        total.totDays += currDays;
                        total.totMonthDays += totalMonthdays;
                        total.totAmount += currDayAmount.amount.objToDecimal();
                        total.totStartDateAmount += currDayAmount.startDateAmount.objToDecimal();

                        total.totnumResCreatedDate += currDayAmount.numRescreatedDate.objToInt32();
                        total.totCreatedDateAmount += currDayAmount.createdDateAmount.objToDecimal();
                        #endregion
                    }
                }

            }

            #endregion

            #region display total

            rowIndex++;
            row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(0);
            cell.SetCellValue(contUtils.getLabel("pdf_TOTALE"));
            cell.CellStyle = headerLabelCellStyle;

            int columnCntTotal = 1;
            foreach (int currMonth in months)
            {
                foreach (int currYear in years)
                {
                    var totalDisplay = lstTotal.FirstOrDefault(x => x.currMonth == currMonth && x.currYear == currYear);
                    if (totalDisplay != null)
                    {
                        #region column filter
                        //1 - percentage
                        if (columns.Contains(1))
                        {
                            cell = row.CreateCell(columnCntTotal);
                            cell.SetCellValue(decimal.Divide((totalDisplay.totDays * 100), totalDisplay.totMonthDays).ToString("N2"));
                            cell.CellStyle = rightAlginTotalCellStyle;
                            columnCntTotal++;
                        }
                        //2 - Number of booked days in month
                        if (columns.Contains(2))
                        {
                            cell = row.CreateCell(columnCntTotal);
                            cell.SetCellValue(totalDisplay.totDays);
                            cell.CellStyle = rightAlginTotalCellStyle;
                            columnCntTotal++;
                        }
                        //3 - AMount according to Number of booked days in month - Amount mix
                        if (columns.Contains(3))
                        {
                            cell = row.CreateCell(columnCntTotal);
                            cell.SetCellValue(totalDisplay.totAmount.ToString("N2"));
                            cell.CellStyle = rightAlginTotalCellStyle;
                            columnCntTotal++;
                        }
                        //4 - Number of reservations of which start date is in current month
                        if (columns.Contains(4))
                        {
                            cell = row.CreateCell(columnCntTotal);
                            cell.SetCellValue(totalDisplay.totnumResStartDate);
                            cell.CellStyle = rightAlginTotalCellStyle;
                            columnCntTotal++;
                        }
                        //5 - Total Reservation amount of which start date is in current month
                        if (columns.Contains(5))
                        {
                            cell = row.CreateCell(columnCntTotal);
                            cell.SetCellValue(totalDisplay.totStartDateAmount.ToString("N2"));
                            cell.CellStyle = rightAlginTotalCellStyle;
                            columnCntTotal++;
                        }
                        //6 - Number of reservations of which creation date is in current month
                        if (columns.Contains(6))
                        {
                            cell = row.CreateCell(columnCntTotal);
                            cell.SetCellValue(totalDisplay.totnumResCreatedDate);
                            cell.CellStyle = rightAlginTotalCellStyle;
                            columnCntTotal++;
                        }
                        //5 - Total Reservation amount of which creation date is in current month
                        if (columns.Contains(7))
                        {
                            cell = row.CreateCell(columnCntTotal);
                            cell.SetCellValue(totalDisplay.totCreatedDateAmount.ToString("N2"));
                            cell.CellStyle = rightAlginTotalCellStyle;
                            columnCntTotal++;
                        }
                        #endregion
                    }
                }
            }
            #endregion

            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                string saveAsFileName = "OccupancyReport-" + DateTime.Now.JSCal_dateTimeToString() + ".xls";
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                Response.Clear();
                Response.BinaryWrite(exportData.GetBuffer());
                Response.End();
            }
            #endregion
        }
    }
}