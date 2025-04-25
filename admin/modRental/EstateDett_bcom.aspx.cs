using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;
using System.IO;
using System.Net;

namespace ModRental.admin.modRental
{
    public partial class EstateDett_bcom : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE currTBL;
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
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);

                if (_est != null)
                {
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                    UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                    fillData();
                }
                else
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                }
            }
        }
        private void fillData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                drp_bcomHotelId.Items.Clear();
                drp_bcomHotelId.Items.Add(new ListItem("- - -", ""));
                var tmpList = dc.dbRntBcomHotelTBLs.Where(x => x.isActive == 1).OrderBy(x => x.title).ToList();
                foreach (var tmp in tmpList)
                {
                    drp_bcomHotelId.Items.Add(new ListItem("" + tmp.title + " - #" + tmp.hotelId, tmp.hotelId));
                }


                currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currTBL == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                drp_bcomHotelId.setSelectedValue(currTBL.bcomHotelId);
                drp_bcomEnabled.setSelectedValue(currTBL.bcomEnabled);
                txt_bcomRoomId.Text = currTBL.bcomRoomId;
                txt_bcomName.Text = currTBL.bcomName;

                var ratesList = new List<dbRntBcomEstateRatesRL>();
                var tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 1 && x.rateType == 1);
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 1, rateType = 1 };
                ratesList.Add(tmpRate);

                #region New Standard Rates - Price Type Single
                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 1 && x.rateType == 4); //RateTypeStandard2
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 1, rateType = 4 };
                ratesList.Add(tmpRate);

                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 1 && x.rateType == 5); //RateTypeStandard3
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 1, rateType = 5 };
                ratesList.Add(tmpRate);

                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 1 && x.rateType == 6); //RateTypeStandard4
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 1, rateType = 6 };
                ratesList.Add(tmpRate);
                #endregion

                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 1 && x.rateType == 2);
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 1, rateType = 2 };
                ratesList.Add(tmpRate);
                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 1 && x.rateType == 3);
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 1, rateType = 3 };
                ratesList.Add(tmpRate);

                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 2 && x.rateType == 1);
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 2, rateType = 1 };
                ratesList.Add(tmpRate);

                #region New Standard Rates - Price Type Single
                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 2 && x.rateType == 4); //RateTypeStandard2
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 2, rateType = 4 };
                ratesList.Add(tmpRate);

                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 2 && x.rateType == 5); //RateTypeStandard3
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 2, rateType = 5 };
                ratesList.Add(tmpRate);

                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 2 && x.rateType == 6); //RateTypeStandard4
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 2, rateType = 6 };
                ratesList.Add(tmpRate);
                #endregion

                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 2 && x.rateType == 2);
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 2, rateType = 2 };
                ratesList.Add(tmpRate);
                tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == 2 && x.rateType == 3);
                if (tmpRate == null)
                    tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = IdEstate, priceType = 2, rateType = 3 };
                ratesList.Add(tmpRate);
                LvRates.DataSource = ratesList;
                LvRates.DataBind();
                var currRate = new BcomUpdate.BcomRates(DateTime.Now);
                foreach (ListViewDataItem item in LvRates.Items)
                {
                    var lbl_pidEstate = item.FindControl("lbl_pidEstate") as Label;
                    var lbl_priceType = item.FindControl("lbl_priceType") as Label;
                    var lbl_rateType = item.FindControl("lbl_rateType") as Label;
                    var ntxt_changeAmount = item.FindControl("ntxt_changeAmount") as RadNumericTextBox;
                    var drp_changeIsDiscount = item.FindControl("drp_changeIsDiscount") as DropDownList;
                    var drp_changeIsPercentage = item.FindControl("drp_changeIsPercentage") as DropDownList;
                    tmpRate = ratesList.SingleOrDefault(x => x.pidEstate == lbl_pidEstate.Text.ToInt32() && x.priceType == lbl_priceType.Text.ToInt32() && x.rateType == lbl_rateType.Text.ToInt32());
                    if (tmpRate == null)
                        tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = lbl_pidEstate.Text.ToInt32(), priceType = lbl_priceType.Text.ToInt32(), rateType = lbl_rateType.Text.ToInt32() };
                    ntxt_changeAmount.Value = tmpRate.changeAmount.objToDouble();
                    drp_changeIsDiscount.setSelectedValue(tmpRate.changeIsDiscount);
                    drp_changeIsPercentage.setSelectedValue(tmpRate.changeIsPercentage);
                    currRate.setRates(tmpRate);
                }
                LvRateChanges.DataSource = ratesList;
                LvRateChanges.DataBind();
                foreach (ListViewDataItem item in LvRateChanges.Items)
                {
                    var lbl_pidEstate = item.FindControl("lbl_pidEstate") as Label;
                    var lbl_priceType = item.FindControl("lbl_priceType") as Label;
                    var lbl_rateType = item.FindControl("lbl_rateType") as Label;
                    var ntxt_changeAmount = item.FindControl("ntxt_changeAmount") as RadNumericTextBox;
                    var drp_changeIsDiscount = item.FindControl("drp_changeIsDiscount") as DropDownList;
                    var drp_changeIsPercentage = item.FindControl("drp_changeIsPercentage") as DropDownList;
                    tmpRate = ratesList.SingleOrDefault(x => x.pidEstate == lbl_pidEstate.Text.ToInt32() && x.priceType == lbl_priceType.Text.ToInt32() && x.rateType == lbl_rateType.Text.ToInt32());
                    if (tmpRate == null)
                        tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = lbl_pidEstate.Text.ToInt32(), priceType = lbl_priceType.Text.ToInt32(), rateType = lbl_rateType.Text.ToInt32() };
                    ntxt_changeAmount.Value = tmpRate.changeAmount.objToDouble();
                    drp_changeIsPercentage.setSelectedValue(tmpRate.changeIsPercentage);
                }
                var bcomRatesList = new List<BcomUpdate.BcomRates>();
                var rateList = dc.dbRntBcomEstateRateChangesRLs.Where(x => x.pidEstate == IdEstate).OrderBy(x => x.changeDate).ToList();
                string rateScript = "";
                rateScript += "function rateCalDates(date){var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
                foreach (var tmp in rateList)
                {
                    var bcomRate = bcomRatesList.SingleOrDefault(x => x.Dt == tmp.changeDate);
                    if (bcomRate == null)
                    {
                        bcomRate = new BcomUpdate.BcomRates(tmp.changeDate, currRate);
                        bcomRatesList.Add(bcomRate);
                    }
                    bcomRate.setRates(tmp);
                    if (tmp.changeIsDiscount < 0) continue;
                    string intDate = "" + tmp.changeDate.JSCal_dateToInt();
                    string type = "opz";
                    rateScript += "if(dateint == " + intDate + ") { _controls += '<span class=\"rntCal " + type + "_f\"></span>'; _class = \"ttp\"; _tooltip = \"rateChange_" + intDate + "\"; }";
                }
                rateScript += rntUtils.rntEstate_availableDatesForJSCal(IdEstate, 0);
                rateScript += "return [_enabled, _class, _tooltip, _controls];";
                rateScript += "}";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "rateCalDates", rateScript, true);
                LvRateChangesTtp.DataSource = bcomRatesList;
                LvRateChangesTtp.DataBind();
                var closedList = dc.dbRntBcomEstateClosedDatesRLs.Where(x => x.pidEstate == IdEstate).OrderBy(x => x.changeDate).ToList();
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
            using (DCmodRental dc = new DCmodRental())
            {
                //currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id != IdEstate && x.bcomRoomId == txt_bcomRoomId.Text);
                //if (currTBL != null)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione RoomId duplicato.\", 340, 110);", true);
                //    return;
                //}
                currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currTBL == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                currTBL.bcomHotelId = drp_bcomHotelId.SelectedValue;
                currTBL.bcomEnabled = drp_bcomEnabled.SelectedValue.ToInt32();
                currTBL.bcomRoomId = txt_bcomRoomId.Text;
                currTBL.bcomName = txt_bcomName.Text;
                DC_RENTAL.SubmitChanges();

                foreach (ListViewDataItem item in LvRates.Items)
                {
                    var lbl_pidEstate = item.FindControl("lbl_pidEstate") as Label;
                    var lbl_priceType = item.FindControl("lbl_priceType") as Label;
                    var lbl_rateType = item.FindControl("lbl_rateType") as Label;
                    var ntxt_changeAmount = item.FindControl("ntxt_changeAmount") as RadNumericTextBox;
                    var drp_changeIsDiscount = item.FindControl("drp_changeIsDiscount") as DropDownList;
                    var drp_changeIsPercentage = item.FindControl("drp_changeIsPercentage") as DropDownList;
                    var tmpRate = dc.dbRntBcomEstateRatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == lbl_priceType.Text.ToInt32() && x.rateType == lbl_rateType.Text.ToInt32());
                    if (tmpRate == null)
                    {
                        tmpRate = new dbRntBcomEstateRatesRL() { pidEstate = lbl_pidEstate.Text.ToInt32(), priceType = lbl_priceType.Text.ToInt32(), rateType = lbl_rateType.Text.ToInt32() };
                        dc.Add(tmpRate);
                    }
                    tmpRate.changeAmount = ntxt_changeAmount.Value.objToInt32();
                    tmpRate.changeIsDiscount = drp_changeIsDiscount.SelectedValue.ToInt32();
                    tmpRate.changeIsPercentage = drp_changeIsPercentage.SelectedValue.ToInt32();
                    dc.SaveChanges();
                }
                if (currTBL.bcomEnabled == 1)
                {
                    BcomUpdate.BcomUpdate_start(IdEstate, "rates");
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.<br />Invio prezzi in corso..\", 340, 110);", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
            }

            AppSettings._refreshCache_RNT_ESTATEs();
            AppSettings.RELOAD_SESSION();
        }
        protected void lnk_rate_save_Click(object sender, EventArgs e)
        {
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
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            using (DCmodRental dc = new DCmodRental())
            {
                DateTime dtCurrent = rdp_rate_dtStart.SelectedDate.Value.Date;
                while (dtCurrent <= rdp_rate_dtEnd.SelectedDate.Value.Date)
                {

                    foreach (ListViewDataItem item in LvRateChanges.Items)
                    {
                        var lbl_pidEstate = item.FindControl("lbl_pidEstate") as Label;
                        var lbl_priceType = item.FindControl("lbl_priceType") as Label;
                        var lbl_rateType = item.FindControl("lbl_rateType") as Label;
                        var ntxt_changeAmount = item.FindControl("ntxt_changeAmount") as RadNumericTextBox;
                        var drp_changeIsDiscount = item.FindControl("drp_changeIsDiscount") as DropDownList;
                        var drp_changeIsPercentage = item.FindControl("drp_changeIsPercentage") as DropDownList;
                        var tmpRate = dc.dbRntBcomEstateRateChangesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent && x.priceType == lbl_priceType.Text.ToInt32() && x.rateType == lbl_rateType.Text.ToInt32());
                        if (tmpRate == null)
                        {
                            tmpRate = new dbRntBcomEstateRateChangesRL() { pidEstate = IdEstate, changeDate = dtCurrent, priceType = lbl_priceType.Text.ToInt32(), rateType = lbl_rateType.Text.ToInt32() };
                            dc.Add(tmpRate);
                        }
                        tmpRate.changeAmount = ntxt_changeAmount.Value.objToInt32();
                        tmpRate.changeIsDiscount = drp_changeIsDiscount.SelectedValue.ToInt32();
                        tmpRate.changeIsPercentage = drp_changeIsPercentage.SelectedValue.ToInt32();
                        dc.SaveChanges();
                    }
                    if (ntxt_rate_minStay.Value.objToInt32() > 0)
                    {
                        var tmp = dc.dbRntBcomEstateClosedDatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent);
                        if (tmp == null)
                        {
                            tmp = new dbRntBcomEstateClosedDatesRL();
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
                BcomUpdate.BcomUpdate_start(IdEstate, "availability", rdp_rate_dtStart.SelectedDate.Value.Date.AddDays(-30), rdp_rate_dtEnd.SelectedDate.Value.Date.AddDays(30));
                BcomUpdate.BcomUpdate_start(IdEstate, "rates", rdp_rate_dtStart.SelectedDate.Value.Date.AddDays(-30), rdp_rate_dtEnd.SelectedDate.Value.Date.AddDays(30));
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            fillData();
        }
        protected void lnk_closed_save_Click(object sender, EventArgs e)
        {
            string errorString = "";
            if (!rdp_closed_dtStart.SelectedDate.HasValue || !rdp_closed_dtEnd.SelectedDate.HasValue)
                errorString += "<br/>-specificare inizio e fine del range";
            else if (rdp_closed_dtStart.SelectedDate > rdp_closed_dtEnd.SelectedDate)
                errorString += "<br/>-la data di fine deve essere successiva alla data di inizio";
            if (errorString != "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                return;
            }
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            using (DCmodRental dc = new DCmodRental())
            {
                DateTime dtCurrent = rdp_closed_dtStart.SelectedDate.Value.Date;
                while (dtCurrent <= rdp_closed_dtEnd.SelectedDate.Value.Date)
                {
                    var tmp = dc.dbRntBcomEstateClosedDatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent);
                    if (tmp == null)
                    {
                        tmp = new dbRntBcomEstateClosedDatesRL();
                        tmp.pidEstate = IdEstate;
                        tmp.changeDate = dtCurrent;
                        dc.Add(tmp);
                    }
                    tmp.isClosed = drp_closed_isClosed.getSelectedValueInt();
                    tmp.minStay = ntxt_closed_minStay.Value.objToInt32();
                    dc.SaveChanges();
                    dtCurrent = dtCurrent.AddDays(1);
                }
                BcomUpdate.BcomUpdate_start(IdEstate, "availability", rdp_closed_dtStart.SelectedDate.Value.Date.AddDays(-30), rdp_closed_dtEnd.SelectedDate.Value.Date.AddDays(30));
                BcomUpdate.BcomUpdate_start(IdEstate, "rates", rdp_closed_dtStart.SelectedDate.Value.Date.AddDays(-30), rdp_closed_dtEnd.SelectedDate.Value.Date.AddDays(30));
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            fillData();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            saveData();
        }
        protected void lnkUpdateAvv_Click(object sender, EventArgs e)
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            if (currTBL.bcomEnabled == 1)
            {
                BcomUpdate.BcomUpdate_start(IdEstate, "availability");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Invio in corso...\", 340, 110);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Booking.com NON Attivo\", 340, 110);", true);
            }
            fillData();
        }

        protected void lnkUpdateRates_Click(object sender, EventArgs e)
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            if (currTBL.bcomEnabled == 1)
            {
                BcomUpdate.BcomUpdate_start(IdEstate, "rates");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Invio in corso...\", 340, 110);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Booking.com NON Attivo\", 340, 110);", true);
            }
            fillData();
        }
    }
}