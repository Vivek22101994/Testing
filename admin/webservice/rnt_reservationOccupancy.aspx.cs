using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using System.Text;
using System.Globalization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace MagaRentalCE.admin.webservice
{
    public partial class rnt_reservationOccupancy : System.Web.UI.Page
    {

        private List<int> _currCity;
        private List<int> _currZone;
        private List<int> _currEstate;
        private List<int> _months;
        private List<int> _ownerID;
        private List<int> _columns;
        private Int64 _agencyID;
        private List<int> _year;
        int _lang;
        private string _response = string.Empty;
        private magaRental_DataContext DC_Rental;
        private List<RNT_TBL_RESERVATION> resList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/html";

                DC_Rental = maga_DataContext.DC_RENTAL;

                #region Get Filters
                _currCity = Request.Form["currCity"].splitStringToList("|").Select(Int32.Parse).ToList();
                _currZone = Request.Form["currZone"].splitStringToList("|").Select(Int32.Parse).ToList();
                _currEstate = Request.Form["currEstate"].splitStringToList("|").Select(Int32.Parse).ToList();
                _months = Request.Form["months"].splitStringToList("|").Select(Int32.Parse).ToList();
                _ownerID = Request.Form["ownerID"].splitStringToList("|").Select(Int32.Parse).ToList();
                _agencyID = Request.Form["agencyID"].objToInt64();
                _year = Request.Form["year"].splitStringToList("|").Select(Int32.Parse).ToList();
                _lang = Request.Form["lang"].objToInt32();
                _columns = Request.Form["columns"].splitStringToList("|").Select(Int32.Parse).ToList();

                if (_lang == 0)
                    _lang = App.DefLangID;
                _response = "Success";
                #endregion


                #region Filter Result
                resList = new List<RNT_TBL_RESERVATION>();
                resList = DC_Rental.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.dtStart.HasValue && x.dtEnd.HasValue).ToList();

                //var ids = AppSettings.RNT_estateListAll.Select(x => x.ComplexId).Distinct();
                //List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateListAll.Where(x => !ids.Contains(x.id) && (x.ComplexType == "" || x.ComplexType == null || x.ComplexType == "0")).OrderBy(x => x.ComplexName).ThenBy(x => x.code).ToList();

                var estateList = AppSettings.RNT_TB_ESTATE.ToList();
                if (!_currEstate.Contains(0))
                {
                    estateList = estateList.Where(x => _currEstate.Contains(x.id)).ToList();
                }

                if (!_currCity.Contains(0))
                {
                    estateList = estateList.Where(x => _currCity.Contains(x.pid_city.objToInt32())).ToList();
                }

                if (_currZone.Count > 0 && !_currZone.Contains(0))
                {
                    estateList = estateList.Where(x => _currZone.Contains(x.pid_zone.objToInt32())).ToList();
                }

                if (!_ownerID.Contains(0))
                {
                    estateList = estateList.Where(x => _ownerID.Contains(x.pid_owner.objToInt32())).ToList();
                }

                if (_months.Count == 0 || _months.Contains(0))
                {
                    _months.Clear();
                    // add all months 
                    for (int i = 1; i <= 12; i++)
                    {
                        _months.Add(i);
                    }
                }

                if (_year.Count == 0)
                {
                    _year.Add(DateTime.Now.Year);
                }
                #endregion

                #region Create Output
                // resList = resList.Where(x => _year.Contains(x.dtStart.Value.Year) && _year.Contains(x.dtEnd.Value.Year)).ToList();

                bool toggle = true;
                var strResponseApt = new StringBuilder("<div class=\"tbleft\"> <table cellpadding=\"5\" cellspacing=\"5\"><tr class=\"head_row\" > <td rowspan=\"2\">" + contUtils.getLabel("reqApartment") + "</td></tr> ");
                strResponseApt.Append("<tr class=\"head_row\"></tr>");

                var strResponse = new StringBuilder("<div class=\"horiz-container\"> <div class=\"content\"> <table cellpadding=\"5\" cellspacing=\"5\"><tr class=\"head_row\">");
                string strRow = string.Empty;
                foreach (int month in _months)
                {
                    foreach (int year in _year)
                    {
                        #region column filter
                        int columnCnt = 0;
                        //1 - percentage
                        if (_columns.Contains(1))
                        {
                            strRow += "<td><a class='infoBtn ico_tooltip_right colTitle contTp' title='" + contUtils.getLabel("lbl_tpPer") + "'></a><span class='colTitle colHeader'>%</span></td>";

                        }
                        //2 - Number of booked days in month
                        if (_columns.Contains(2))
                        {
                            strRow += "<td><a class='infoBtn ico_tooltip_right colTitle contTp' title='" + contUtils.getLabel("lbl_tpDays") + "'></a><span class='colTitle colHeader'>" + contUtils.getLabel("lblDays") + "</span></td>";

                        }
                        //3 - AMount according to Number of booked days in month - Amount mix
                        if (_columns.Contains(3))
                        {
                            strRow += "<td><a class='infoBtn ico_tooltip_right colTitle contTp' title='" + contUtils.getLabel("lbl_tpAmountMix") + "'></a><span class='colTitle colHeader'>" + contUtils.getLabel("lbl_AmountMix") + "</span></td>";

                        }
                        //4 - Number of reservations of which start date is in current month
                        if (_columns.Contains(4))
                        {
                            strRow += "<td><a class='infoBtn ico_tooltip_right colTitle contTp' title='" + contUtils.getLabel("lbl_tpWholeNumRes") + "'></a><span class='colTitle colHeader'>" + contUtils.getLabel("lblWholeNumRes") + "</span></td>";

                        }
                        //5 - Total Reservation amount of which start date is in current month
                        if (_columns.Contains(5))
                        {
                            strRow += "<td><a class='infoBtn ico_tooltip_right colTitle contTp' title='" + contUtils.getLabel("lbl_tpAmountWhole") + "'></a><span class='colTitle colHeader'>" + contUtils.getLabel("lblAmountWhole") + "</span>";

                        }
                        //6 - Number of reservations of which creation date is in current month
                        if (_columns.Contains(6))
                        {
                            strRow += "<td><a class='infoBtn ico_tooltip_right colTitle contTp' title='" + contUtils.getLabel("lbl_tpCreatedNumRes") + "'></a><span class='colTitle colHeader'>" + contUtils.getLabel("lblCreatedNumRes") + "</span></td>";

                        }
                        //5 - Total Reservation amount of which creation date is in current month
                        if (_columns.Contains(7))
                        {
                            strRow += "<td><a class='infoBtn ico_tooltip_right colTitle contTp' title='" + contUtils.getLabel("lbl_tpCreatedResAmount") + "'></a><span class='colTitle colHeader'>" + contUtils.getLabel("lblCreatedResAmount") + "</span></td>";

                        }
                        #endregion

                        if (_lang == 1)
                            strResponse.AppendFormat("<td colspan=\"{0}\" class='colTitle'> {1}  {2} </td>", _columns.Count, new DateTime(year, month, 1).getMonthITA(false), year);
                        else
                            strResponse.AppendFormat("<td colspan=\"{0}\" class='colTitle'> {1}  {2} </td>", _columns.Count, CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month), year);

                    }
                }
                strResponse.Append("</tr>");
                strResponse.AppendFormat("<tr class=\"head_row\"> {0} </tr>", strRow);
                List<rntOccupancyReportUtils.Total> lstTotal = new List<rntOccupancyReportUtils.Total>();
                foreach (RNT_TB_ESTATE objEstate in estateList)
                {
                    decimal currPercentage = 0;
                    int currDays = 0;
                    int totalMonthdays = 0;

                    var tmpResList = resList.Where(x => x.pid_estate == objEstate.id).ToList();
                    if (_agencyID != 0)
                    {
                        if (_agencyID == -2)
                            tmpResList = tmpResList.Where(x => x.agentID != null && x.agentID > 0).ToList();
                        else if (_agencyID == -1)
                            tmpResList = tmpResList.Where(x => x.agentID == null || x.agentID == 0).ToList();
                        else
                            tmpResList = tmpResList.Where(x => x.agentID != null && x.agentID == _agencyID).ToList();
                    }
                    if (toggle)
                    {
                        strResponseApt.AppendFormat("<tr> <td><span class='aptName'>{0}</span></td>", objEstate.code);
                        strResponse.Append("<tr>");
                        toggle = false;
                    }
                    else
                    {
                        strResponseApt.AppendFormat("<tr class=\"alternate\"><td><span class='aptName'>{0}</span></td>", objEstate.code);
                        strResponse.Append("<tr class=\"alternate\"> ");
                        toggle = true;
                    }
                    strResponseApt.Append("</tr>");
                    foreach (int currMonth in _months)
                    {
                        foreach (int currYear in _year)
                        {
                            totalMonthdays = DateTime.DaysInMonth(currYear, currMonth);
                            var currMonthReservations = tmpResList.Where(x => (x.dtStart.Value.Year == currYear || x.dtEnd.Value.Year == currYear) && (x.dtStart.Value.Month == currMonth || x.dtEnd.Value.Month == currMonth)).ToList();
                            var currDayAmount = rntOccupancyReportUtils.GetTotalBookedDaysPerMonth(tmpResList, currMonth, currYear, totalMonthdays);
                            currDays = currDayAmount.days;
                            currPercentage = decimal.Divide((100 * currDays), totalMonthdays);

                            #region column filter
                            //1 - percentage
                            if (_columns.Contains(1))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0}</td>", currPercentage.ToString("N2"));
                            }
                            //2 - Number of booked days in month
                            if (_columns.Contains(2))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0}</td>", currDays);
                            }
                            //3 - AMount according to Number of booked days in month - Amount mix
                            if (_columns.Contains(3))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0} </td>", currDayAmount.amount.ToString("N2"));
                            }
                            //4 - Number of reservations of which start date is in current month
                            if (_columns.Contains(4))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0}  </td>", currDayAmount.numResStartDate);
                            }
                            //5 - Total Reservation amount of which start date is in current month
                            if (_columns.Contains(5))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0} </td>", currDayAmount.startDateAmount.ToString("N2"));
                            }
                            //6 - Number of reservations of which creation date is in current month
                            if (_columns.Contains(6))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0} </td>", currDayAmount.numRescreatedDate);
                            }
                            //5 - Total Reservation amount of which creation date is in current month
                            if (_columns.Contains(7))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0}  </td>", currDayAmount.createdDateAmount.ToString("N2"));
                            }
                            #endregion

                            //strResponse.AppendFormat("<td class=\"col\" > {0} % </td> <td class=\"col\"> {1} </td><td class=\"col\"> {2} </td><td class=\"col\"> {3} </td><td class=\"col\"> {4} </td><td class=\"col\"> {5} </td><td class=\"col\"> {6} </td>", currPercentage.ToString("N2"), currDays, "€" + currDayAmount.amount.ToString("N2"), currDayAmount.numResStartDate, "€" + currDayAmount.startDateAmount.ToString("N2"), currDayAmount.numRescreatedDate, currDayAmount.createdDateAmount.ToString("N2"));

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
                    strResponse.Append("</tr>");
                }


                #region display total
                if (toggle)
                    strResponseApt.Append("<tr><td><span class='total'>" + contUtils.getLabel("pdf_TOTALE") + "</span></td>");
                else
                    strResponseApt.AppendFormat("<tr class=\"alternate\"><td><span class='total'>" + contUtils.getLabel("pdf_TOTALE") + "</span></td>");

                foreach (int currMonth in _months)
                {
                    foreach (int currYear in _year)
                    {
                        var totalDisplay = lstTotal.FirstOrDefault(x => x.currMonth == currMonth && x.currYear == currYear);
                        if (totalDisplay != null)
                        {
                            #region column filter
                            //1 - percentage
                            if (_columns.Contains(1))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0}  </td>", decimal.Divide((totalDisplay.totDays * 100), totalDisplay.totMonthDays).ToString("N2"));
                            }
                            //2 - Number of booked days in month
                            if (_columns.Contains(2))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0}  </td>", totalDisplay.totDays);
                            }
                            //3 - AMount according to Number of booked days in month - Amount mix
                            if (_columns.Contains(3))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0}  </td>", totalDisplay.totAmount.ToString("N2"));
                            }
                            //4 - Number of reservations of which start date is in current month
                            if (_columns.Contains(4))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0}  </td>", totalDisplay.totnumResStartDate);
                            }
                            //5 - Total Reservation amount of which start date is in current month
                            if (_columns.Contains(5))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0} </td>", totalDisplay.totStartDateAmount.ToString("N2"));
                            }
                            //6 - Number of reservations of which creation date is in current month
                            if (_columns.Contains(6))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0} </td>", totalDisplay.totnumResCreatedDate);
                            }
                            //5 - Total Reservation amount of which creation date is in current month
                            if (_columns.Contains(7))
                            {
                                strResponse.AppendFormat("<td class=\"col\"> {0} </td>", totalDisplay.totCreatedDateAmount.ToString("N2"));
                            }
                            #endregion
                        }
                    }
                }
                #endregion

                strResponseApt.Append("</table></div>");
                strResponse.Append("</table></div></div>");

                _response = strResponseApt.ToString() + strResponse.ToString();
                #endregion

                Response.Write(_response);
            }
        }

    }
}