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
    public partial class EstateChnlExpedia_priceChange : System.Web.UI.Page
    {
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

        public string RoomTypeId
        {
            get
            {
                return HF_RoomTypeId.Value;
            }
            set
            {
                HF_RoomTypeId.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                ltr_apartment.Text = currEstate.code;
                ucNav.IdEstate = IdEstate;
                filldata();
            }
        }

        private void filldata()
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                var currTbl = dcChnl.dbRntChnlExpediaEstateTBLs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    currTbl = new dbRntChnlExpediaEstateTBL() { id = IdEstate, HotelId = 0, RoomTypeId = "" };
                    dcChnl.Add(currTbl);
                    dcChnl.SaveChanges();
                }
                pnlNotConnected.Visible = currTbl.RoomTypeId == "";
                if (currTbl.RoomTypeId == "") return;
                RoomTypeId = currTbl.RoomTypeId;
                var roomTypeTbl = dcChnl.dbRntChnlExpediaRoomTypeTBLs.SingleOrDefault(x => x.id == RoomTypeId);
                if (roomTypeTbl == null)
                {
                    currTbl.RoomTypeId = "";
                    currTbl.HotelId = 0;
                    dcChnl.SaveChanges();
                    pnlNotConnected.Visible = currTbl.RoomTypeId == "";
                    return;
                }
                pnlPriceChange.Visible = true;                
                bind_chk_expRatePlans();

            }
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
                return;
            }
            List<string> ratePlans = new List<string>();
            using (DCchnlExpedia dc = new DCchnlExpedia())
            {
                DateTime dtCurrent = rdp_rate_dtStart.SelectedDate.Value.Date;
                while (dtCurrent <= rdp_rate_dtEnd.SelectedDate.Value.Date)
                {
                    foreach (ListViewDataItem item in LvRateChanges.Items)
                    {
                        var ntxt_changeAmount = item.FindControl("ntxt_changeAmount") as RadNumericTextBox;
                        var drp_changeIsDiscount = item.FindControl("drp_changeIsDiscount") as DropDownList;
                        var drp_changeIsPercentage = item.FindControl("drp_changeIsPercentage") as DropDownList;
                        var chk_send_price = item.FindControl("chk_send_price") as CheckBox;
                        var lbl_ratePlanId = item.FindControl("lbl_ratePlanId") as Label;

                        var chk_inDay1 = item.FindControl("chk_inDay1") as CheckBox;
                        var chk_inDay2 = item.FindControl("chk_inDay2") as CheckBox;
                        var chk_inDay3 = item.FindControl("chk_inDay3") as CheckBox;
                        var chk_inDay4 = item.FindControl("chk_inDay4") as CheckBox;
                        var chk_inDay5 = item.FindControl("chk_inDay5") as CheckBox;
                        var chk_inDay6 = item.FindControl("chk_inDay6") as CheckBox;
                        var chk_inDay7 = item.FindControl("chk_inDay7") as CheckBox;

                        if (chk_send_price.Checked)
                        {
                            var tmpRate = dc.dbRntChnlExpediaEstateRateChangesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent && x.RoomTypeId == RoomTypeId && x.RatePlanId == lbl_ratePlanId.Text);
                            if (tmpRate == null)
                            {
                                tmpRate = new dbRntChnlExpediaEstateRateChangesRL() { pidEstate = IdEstate, changeDate = dtCurrent, RoomTypeId = RoomTypeId, RatePlanId = lbl_ratePlanId.Text };
                                dc.Add(tmpRate);
                            }
                            if (chk_inDay1.Checked || chk_inDay2.Checked || chk_inDay3.Checked || chk_inDay4.Checked || chk_inDay5.Checked || chk_inDay6.Checked || chk_inDay7.Checked)
                            {
                                if ((chk_inDay1.Checked && dtCurrent.DayOfWeek == DayOfWeek.Monday) || (chk_inDay2.Checked && dtCurrent.DayOfWeek == DayOfWeek.Tuesday) || (chk_inDay3.Checked && dtCurrent.DayOfWeek == DayOfWeek.Wednesday) || (chk_inDay4.Checked && dtCurrent.DayOfWeek == DayOfWeek.Thursday) || (chk_inDay5.Checked && dtCurrent.DayOfWeek == DayOfWeek.Friday) || (chk_inDay6.Checked && dtCurrent.DayOfWeek == DayOfWeek.Saturday) || (chk_inDay7.Checked && dtCurrent.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    tmpRate.rate_changeAmount = ntxt_changeAmount.Value.objToInt32();
                                    tmpRate.rate_changeIsDiscount = drp_changeIsDiscount.SelectedValue.ToInt32();
                                    tmpRate.rate_changeIsPercentage = drp_changeIsPercentage.SelectedValue.ToInt32();
                                    tmpRate.rate_chnageDay1 = chk_inDay1.Checked ? 1 : 0;
                                    tmpRate.rate_chnageDay2 = chk_inDay2.Checked ? 1 : 0;
                                    tmpRate.rate_chnageDay3 = chk_inDay3.Checked ? 1 : 0;
                                    tmpRate.rate_chnageDay4 = chk_inDay4.Checked ? 1 : 0;
                                    tmpRate.rate_chnageDay5 = chk_inDay5.Checked ? 1 : 0;
                                    tmpRate.rate_chnageDay6 = chk_inDay6.Checked ? 1 : 0;
                                    tmpRate.rate_chnageDay7 = chk_inDay7.Checked ? 1 : 0;
                                }
                            }
                            else
                            {
                                tmpRate.rate_changeAmount = ntxt_changeAmount.Value.objToInt32();
                                tmpRate.rate_changeIsDiscount = drp_changeIsDiscount.SelectedValue.ToInt32();
                                tmpRate.rate_changeIsPercentage = drp_changeIsPercentage.SelectedValue.ToInt32();
                                tmpRate.rate_chnageDay1 = chk_inDay1.Checked ? 1 : 0;
                                tmpRate.rate_chnageDay2 = chk_inDay2.Checked ? 1 : 0;
                                tmpRate.rate_chnageDay3 = chk_inDay3.Checked ? 1 : 0;
                                tmpRate.rate_chnageDay4 = chk_inDay4.Checked ? 1 : 0;
                                tmpRate.rate_chnageDay5 = chk_inDay5.Checked ? 1 : 0;
                                tmpRate.rate_chnageDay6 = chk_inDay6.Checked ? 1 : 0;
                                tmpRate.rate_chnageDay7 = chk_inDay7.Checked ? 1 : 0;
                            }
                            dc.SaveChanges();
                            ratePlans.Add(lbl_ratePlanId.Text);
                        }
                    }
                    dtCurrent = dtCurrent.AddDays(1);
                }
                ChnlExpediaUpdate.UpdateRates_start(IdEstate, rdp_rate_dtStart.SelectedDate.Value.Date, rdp_rate_dtEnd.SelectedDate.Value, ratePlans);
                //ChnlExpediaUpdate.UpdateRates_start(IdEstate, rdp_rate_dtStart.SelectedDate.Value.Date.AddDays(-30), rdp_rate_dtEnd.SelectedDate.Value.AddDays(30), ratePlans);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.<br />Invio prezzi in corso..\", 340, 110);", true);
            }
            filldata();
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
                return;
            }
            List<string> ratePlans = new List<string>();
            using (DCchnlExpedia dc = new DCchnlExpedia())
            {
                DateTime dtCurrent = rdp_closed_dtStart.SelectedDate.Value.Date;
                while (dtCurrent <= rdp_closed_dtEnd.SelectedDate.Value.Date)
                {
                    foreach (ListViewDataItem item in LVRateRestrictions.Items)
                    {
                        var chk_send_price = item.FindControl("chk_send_price") as CheckBox;
                        var lbl_ratePlanId = item.FindControl("lbl_ratePlanId") as Label;

                        var drp_closed_isClosed = item.FindControl("drp_closed_isClosed") as DropDownList;
                        var drp_closedOnArrival_isClosed = item.FindControl("drp_closedOnArrival_isClosed") as DropDownList;
                        var drp_closedOnDeparture_isClosed = item.FindControl("drp_closedOnDeparture_isClosed") as DropDownList;
                        var ntxt_closed_minStay = item.FindControl("ntxt_closed_minStay") as RadNumericTextBox;
                        var ntxt_closed_maxStay = item.FindControl("ntxt_closed_maxStay") as RadNumericTextBox;

                        var chk_inDay1 = item.FindControl("chk_inDay1") as CheckBox;
                        var chk_inDay2 = item.FindControl("chk_inDay2") as CheckBox;
                        var chk_inDay3 = item.FindControl("chk_inDay3") as CheckBox;
                        var chk_inDay4 = item.FindControl("chk_inDay4") as CheckBox;
                        var chk_inDay5 = item.FindControl("chk_inDay5") as CheckBox;
                        var chk_inDay6 = item.FindControl("chk_inDay6") as CheckBox;
                        var chk_inDay7 = item.FindControl("chk_inDay7") as CheckBox;

                        if (chk_send_price.Checked)
                        {
                            var tmpRateRestriction = dc.dbRntChnlExpediaEstateClosedDatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent && x.RoomTypeId == RoomTypeId && x.RatePlanId == lbl_ratePlanId.Text);
                            if (tmpRateRestriction == null)
                            {
                                tmpRateRestriction = new dbRntChnlExpediaEstateClosedDatesRL() { pidEstate = IdEstate, changeDate = dtCurrent, RoomTypeId = RoomTypeId, RatePlanId = lbl_ratePlanId.Text };
                                dc.Add(tmpRateRestriction);
                            }

                            if (chk_inDay1.Checked || chk_inDay2.Checked || chk_inDay3.Checked || chk_inDay4.Checked || chk_inDay5.Checked || chk_inDay6.Checked || chk_inDay7.Checked)
                            {
                                if ((chk_inDay1.Checked && dtCurrent.DayOfWeek == DayOfWeek.Monday) || (chk_inDay2.Checked && dtCurrent.DayOfWeek == DayOfWeek.Tuesday) || (chk_inDay3.Checked && dtCurrent.DayOfWeek == DayOfWeek.Wednesday) || (chk_inDay4.Checked && dtCurrent.DayOfWeek == DayOfWeek.Thursday) || (chk_inDay5.Checked && dtCurrent.DayOfWeek == DayOfWeek.Friday) || (chk_inDay6.Checked && dtCurrent.DayOfWeek == DayOfWeek.Saturday) || (chk_inDay7.Checked && dtCurrent.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    if (chk_is_close_ratePlan.Checked)
                                        tmpRateRestriction.isClosed = drp_closed_isClosed.getSelectedValueInt();

                                    if (chk_is_close_onArrival.Checked)
                                        tmpRateRestriction.isClosedOnArrival = drp_closedOnArrival_isClosed.getSelectedValueInt();

                                    if (chk_is_close_onDepartue.Checked)
                                        tmpRateRestriction.isClosedOnDeparture = drp_closedOnDeparture_isClosed.getSelectedValueInt();

                                    if (chk_is_minStay.Checked)
                                        tmpRateRestriction.minStay = ntxt_closed_minStay.Value.objToInt32();

                                    if (chk_is_maxStay.Checked)
                                        tmpRateRestriction.maxStay = ntxt_closed_maxStay.Value.objToInt32();
                                    tmpRateRestriction.isDay1 = chk_inDay1.Checked ? 1 : 0;
                                    tmpRateRestriction.isDay2 = chk_inDay2.Checked ? 1 : 0;
                                    tmpRateRestriction.isDay3 = chk_inDay3.Checked ? 1 : 0;
                                    tmpRateRestriction.isDay4 = chk_inDay4.Checked ? 1 : 0;
                                    tmpRateRestriction.isDay5 = chk_inDay5.Checked ? 1 : 0;
                                    tmpRateRestriction.isDay6 = chk_inDay6.Checked ? 1 : 0;
                                    tmpRateRestriction.isDay7 = chk_inDay7.Checked ? 1 : 0;
                                }
                            }
                            else
                            {
                                if (chk_is_close_ratePlan.Checked)
                                    tmpRateRestriction.isClosed = drp_closed_isClosed.getSelectedValueInt();

                                if (chk_is_close_onArrival.Checked)
                                    tmpRateRestriction.isClosedOnArrival = drp_closedOnArrival_isClosed.getSelectedValueInt();

                                if (chk_is_close_onDepartue.Checked)
                                    tmpRateRestriction.isClosedOnDeparture = drp_closedOnDeparture_isClosed.getSelectedValueInt();

                                if (chk_is_minStay.Checked)
                                    tmpRateRestriction.minStay = ntxt_closed_minStay.Value.objToInt32();

                                if (chk_is_maxStay.Checked)
                                    tmpRateRestriction.maxStay = ntxt_closed_maxStay.Value.objToInt32();

                                tmpRateRestriction.isDay1 = chk_inDay1.Checked ? 1 : 0;
                                tmpRateRestriction.isDay2 = chk_inDay2.Checked ? 1 : 0;
                                tmpRateRestriction.isDay3 = chk_inDay3.Checked ? 1 : 0;
                                tmpRateRestriction.isDay4 = chk_inDay4.Checked ? 1 : 0;
                                tmpRateRestriction.isDay5 = chk_inDay5.Checked ? 1 : 0;
                                tmpRateRestriction.isDay6 = chk_inDay6.Checked ? 1 : 0;
                                tmpRateRestriction.isDay7 = chk_inDay7.Checked ? 1 : 0;
                            }
                            dc.SaveChanges();
                            ratePlans.Add(lbl_ratePlanId.Text);
                        }
                    }
                    dtCurrent = dtCurrent.AddDays(1);
                }
                ChnlExpediaUpdate.UpdateRates_start(IdEstate, rdp_closed_dtStart.SelectedDate.Value.Date, rdp_closed_dtEnd.SelectedDate.Value, ratePlans);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.<br />Invio prezzi in corso..\", 340, 110);", true);
            }
        }

        protected void bind_chk_expRatePlans()
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                var ratePlans = dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.Where(x => x.RoomTypeId == RoomTypeId && x.status == 1 && x.rateAcquisitionType == "SellRate").ToList();
                tbl_ratePlan.Visible = true;
                if (ratePlans != null && ratePlans.Count > 0)
                {
                    tbl_ratePlan.Visible = true;
                    LvRateChanges.DataSource = ratePlans;
                    LvRateChanges.DataBind();

                    tbl_rateRestrictions.Visible = true;
                    LVRateRestrictions.DataSource = ratePlans;
                    LVRateRestrictions.DataBind();
                }
                else
                {
                    tbl_ratePlan.Visible = false;
                    tbl_rateRestrictions.Visible = false;

                }
            }
        }

        protected void lnkUpdateAvv_Click(object sender, EventArgs e)
        {
            ChnlExpediaUpdate.UpdateAvailability_start(IdEstate);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.<br />Invio disponibilita in corso..\", 340, 110);", true);
        }

        protected void lnkUpdateRates_Click(object sender, EventArgs e)
        {
            ChnlExpediaUpdate.UpdateSplitRates_start(IdEstate, new List<string>());
            //ChnlExpediaUpdate.UpdateRatesWithSplitDates(IdEstate, new List<string>());
            //ChnlExpediaUpdate.UpdateRates_start(IdEstate, new List<string>());
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.<br />Invio prezzi in corso..\", 340, 110);", true);
        }

        protected void lnk_send_allotment_Click(object sender, EventArgs e)
        {
            string errorString = "";
            if (!rdp_allotment_dtStart.SelectedDate.HasValue || !rdp_allotment_dtEnd.SelectedDate.HasValue)
                errorString += "<br/>-specificare inizio e fine del range";
            else if (rdp_allotment_dtStart.SelectedDate > rdp_allotment_dtEnd.SelectedDate)
                errorString += "<br/>-la data di fine deve essere successiva alla data di inizio";
            if (errorString != "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                return;
            }
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                return;
            }
            List<string> ratePlans = new List<string>();
            using (DCchnlExpedia dc = new DCchnlExpedia())
            {
                DateTime dtCurrent = rdp_allotment_dtStart.SelectedDate.Value.Date;
                while (dtCurrent <= rdp_allotment_dtEnd.SelectedDate.Value.Date)
                {
                    var tmpAllotment = dc.dbRntChnlExpediaEstateAllotmentChangeRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent && x.RoomTypeId == RoomTypeId);
                    if (tmpAllotment == null)
                    {
                        tmpAllotment = new dbRntChnlExpediaEstateAllotmentChangeRL() { pidEstate = IdEstate, changeDate = dtCurrent, RoomTypeId = RoomTypeId };
                        dc.Add(tmpAllotment);
                    }
                    tmpAllotment.Units = ntxt_changeAllotment.Value.objToInt32();
                    tmpAllotment.change = drp_change.getSelectedValueInt();
                    dc.SaveChanges();
                    dtCurrent = dtCurrent.AddDays(1);
                }
            }
            ChnlExpediaUpdate.UpdateAvailability_start(IdEstate, rdp_allotment_dtStart.SelectedDate.Value.Date, rdp_allotment_dtEnd.SelectedDate.Value);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.<br />Invio disponibilita in corso..\", 340, 110);", true);
        }
    }
}