using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateChnlAtraveo_price : adminBasePage
    {
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].objToInt32();
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                fillData();
            }
        }

        private void fillData()
        {
            using (DCchnlAtraveo dc = new DCchnlAtraveo())
            {
                var currTbl = dc.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                ntxt_weekday_changeAmount.Value = currTbl.weekday_changeAmount.objToInt32();
                ntxt_weekly_changeAmount.Value = currTbl.weekly_changeAmount.objToInt32();

                drp_weekday_changeIsDiscount.setSelectedValue(currTbl.weekday_changeIsDiscount.objToInt32());
                drp_weekly_changeIsDiscount.setSelectedValue(currTbl.weekly_changeIsDiscount.objToInt32());

                drp_weekday_changeIsPercentage.setSelectedValue(currTbl.weekday_changeIsPercentage.objToInt32());
                drp_weekly_changeIsPercentage.setSelectedValue(currTbl.weekly_changeIsPercentage.objToInt32());

                ntxt_rate_weekday_changeAmount.Value = currTbl.weekday_changeAmount.objToInt32();
                ntxt_rate_weekly_changeAmount.Value = currTbl.weekly_changeAmount.objToInt32();

                drp_rate_weekday_changeIsPercentage.setSelectedValue(currTbl.weekday_changeIsPercentage.objToInt32());
                drp_rate_weekly_changeIsPercentage.setSelectedValue(currTbl.weekly_changeIsPercentage.objToInt32());


                var rateList = dc.dbRntChnlAtraveoEstateRateChangesRLs.Where(x => x.pidEstate == IdEstate).OrderBy(x => x.changeDate).ToList();
                string rateScript = "";
                rateScript += "function rateCalDates(date){var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
                foreach (var tmp in rateList)
                {
                    if (tmp.weekday_changeIsDiscount < 0 && tmp.weekly_changeIsDiscount < 0) continue;
                    string intDate = "" + tmp.changeDate.JSCal_dateToInt();
                    string type = "opz";
                    rateScript += "if(dateint == " + intDate + ") { _controls += '<span class=\"rntCal " + type + "_f\"></span>'; _class = \"ttp\"; _tooltip = \"rateChange_" + intDate + "\"; }";
                }
                rateScript += rntUtils.rntEstate_availableDatesForJSCal(IdEstate, 0);
                rateScript += "return [_enabled, _class, _tooltip, _controls];";
                rateScript += "}";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "rateCalDates", rateScript, true);
                LvRateChangesTtp.DataSource = rateList;
                LvRateChangesTtp.DataBind();
                var closedList = dc.dbRntChnlAtraveoEstateClosedDatesRLs.Where(x => x.pidEstate == IdEstate).OrderBy(x => x.changeDate).ToList();
                string closedScript = "";
                closedScript += "function closedCalDates(date){var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
                foreach (var tmp in closedList)
                {
                    string intDate = "" + tmp.changeDate.JSCal_dateToInt();
                    string type = "";
                    if (tmp.isClosed == 1) type = "prt";
                    else if (tmp.minStay > 0) type = "opz";
                    closedScript += "if(dateint == " + intDate + ") { _controls += '<span class=\"rntCal " + type + "_f\"></span>'; " + (tmp.minStay > 0 ? " _class = \"ttp\"; _tooltip = 'MinStay: " + tmp.minStay + "'; " : "") + "}";
                }
                closedScript += rntUtils.rntEstate_availableDatesForJSCal(IdEstate, 0);
                closedScript += "return [_enabled, _class, _tooltip, _controls];";
                closedScript += "}";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "closedCalDates", closedScript, true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setCal", "setCal();", true);
            } 
        }
        private void saveData()
        {
            using (DCchnlAtraveo dc = new DCchnlAtraveo())
            {
                var currTbl = dc.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                currTbl.weekday_changeAmount = ntxt_weekday_changeAmount.Value.objToInt32();
                currTbl.weekly_changeAmount = ntxt_weekly_changeAmount.Value.objToInt32();

                currTbl.weekday_changeIsDiscount = drp_weekday_changeIsDiscount.getSelectedValueInt();
                currTbl.weekly_changeIsDiscount = drp_weekly_changeIsDiscount.getSelectedValueInt();

                currTbl.weekday_changeIsPercentage = drp_weekday_changeIsPercentage.getSelectedValueInt();
                currTbl.weekly_changeIsPercentage = drp_weekly_changeIsPercentage.getSelectedValueInt();
                dc.SaveChanges(); 
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
            } 
        }
        protected void lnk_rate_save_Click(object sender, EventArgs e)
        {
            using (DCchnlAtraveo dc = new DCchnlAtraveo())
            {
                var currTbl = dc.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                string errorString = "";
                if (!rdp_rate_dtStart.SelectedDate.HasValue || !rdp_rate_dtEnd.SelectedDate.HasValue)
                    errorString += "<br/>-specificare inizio e fine del range";
                else if (rdp_rate_dtStart.SelectedDate > rdp_rate_dtEnd.SelectedDate)
                    errorString += "<br/>-la data di fine deve essere successiva alla data di inizio";
                if (errorString != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                    return;
                }
                DateTime dtCurrent = rdp_rate_dtStart.SelectedDate.Value.Date;
                while (dtCurrent <= rdp_rate_dtEnd.SelectedDate.Value.Date)
                {
                    var tmpRate = dc.dbRntChnlAtraveoEstateRateChangesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent);
                    if (tmpRate == null)
                    {
                        tmpRate = new dbRntChnlAtraveoEstateRateChangesRL() { pidEstate = IdEstate, changeDate = dtCurrent};
                        dc.Add(tmpRate);
                    }
                    tmpRate.weekday_changeAmount = ntxt_rate_weekday_changeAmount.Value.objToInt32();
                    tmpRate.weekly_changeAmount = ntxt_rate_weekly_changeAmount.Value.objToInt32();

                    tmpRate.weekday_changeIsDiscount = drp_rate_weekday_changeIsDiscount.getSelectedValueInt();
                    tmpRate.weekly_changeIsDiscount = drp_rate_weekly_changeIsDiscount.getSelectedValueInt();

                    tmpRate.weekday_changeIsPercentage = drp_rate_weekday_changeIsPercentage.getSelectedValueInt();
                    tmpRate.weekly_changeIsPercentage = drp_rate_weekly_changeIsPercentage.getSelectedValueInt();
                    dc.SaveChanges(); 
                    if (ntxt_rate_minStay.Value.objToInt32() > 0)
                    {
                        var tmp = dc.dbRntChnlAtraveoEstateClosedDatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent);
                        if (tmp == null)
                        {
                            tmp = new dbRntChnlAtraveoEstateClosedDatesRL();
                            tmp.pidEstate = IdEstate;
                            tmp.changeDate = dtCurrent;
                            tmp.isClosed = 0;
                            dc.Add(tmp);
                        }
                        tmp.minStay = ntxt_rate_minStay.Value.objToInt32();
                        dc.SaveChanges(); 
                    }
                    dtCurrent = dtCurrent.AddDays(1);
                }
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);

            }
            fillData();
        }
        protected void lnk_closed_save_Click(object sender, EventArgs e)
        {
            using (DCchnlAtraveo dc = new DCchnlAtraveo())
            {
                var currTbl = dc.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                } string errorString = "";
                if (!rdp_closed_dtStart.SelectedDate.HasValue || !rdp_closed_dtEnd.SelectedDate.HasValue)
                    errorString += "<br/>-specificare inizio e fine del range";
                else if (rdp_closed_dtStart.SelectedDate > rdp_closed_dtEnd.SelectedDate)
                    errorString += "<br/>-la data di fine deve essere successiva alla data di inizio";
                if (errorString != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                    return;
                }
                    DateTime dtCurrent = rdp_closed_dtStart.SelectedDate.Value.Date;
                    while (dtCurrent <= rdp_closed_dtEnd.SelectedDate.Value.Date)
                    {
                        var tmp = dc.dbRntChnlAtraveoEstateClosedDatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent);
                        if (tmp == null)
                        {
                            tmp = new dbRntChnlAtraveoEstateClosedDatesRL();
                            tmp.pidEstate = IdEstate;
                            tmp.changeDate = dtCurrent;
                            dc.Add(tmp);
                        }
                        tmp.isClosed = drp_closed_isClosed.getSelectedValueInt();
                        tmp.minStay = ntxt_closed_minStay.Value.objToInt32();
                        dc.SaveChanges();
                        dtCurrent = dtCurrent.AddDays(1);
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                
                fillData();
            }
        }

        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }
}