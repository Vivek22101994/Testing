using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental
{
    public partial class EstateChnlRU_price : adminBasePage
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

        protected string featureType = "Price";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }

                ltr_apartment.Text = currEstate.code + " / " + "rif. " + IdEstate;
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                FillControls();
            }
        }

        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }

        protected void FillControls()
        {
            using (DCchnlRentalsUnited dc = new DCchnlRentalsUnited())
            {
                var currTbl = dc.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }

                ntxt_changeAmount.Value = currTbl.changeAmount.objToInt32();
                drp_changeIsDiscount.setSelectedValue(currTbl.changeIsDiscount.objToInt32());
                drp_changeIsPercentage.setSelectedValue(currTbl.changeIsPercentage.objToInt32());

                ntxt_rate_changeAmount.Value = currTbl.changeAmount.objToInt32();
                drp_rate_changeIsPercentage.setSelectedValue(currTbl.changeIsPercentage.objToInt32());

                var rateList = dc.dbRntChnlRentalsUnitedEstateRateChangesRLs.Where(x => x.pidEstate == IdEstate).OrderBy(x => x.changeDate).ToList();
                string rateScript = "";
                rateScript += "function rateCalDates(date){var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
                foreach (var tmp in rateList)
                {
                    if (tmp.changeIsDiscount < 0) continue;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setCal", "setCal();", true);
            }
        }

        protected void FillDataFromControls()
        {
            using (DCchnlRentalsUnited dc = new DCchnlRentalsUnited())
            {
                var currTbl = dc.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                currTbl.changeAmount = ntxt_changeAmount.Value.objToInt32();
                currTbl.changeIsDiscount = drp_changeIsDiscount.getSelectedValueInt();
                currTbl.changeIsPercentage = drp_changeIsPercentage.getSelectedValueInt();

                 dc.SaveChanges(); 
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            FillControls();
        }

        protected void lnk_rate_save_Click(object sender, EventArgs e)
        {
            using (DCchnlRentalsUnited dc = new DCchnlRentalsUnited())
            {
                var currTbl = dc.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
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
                    var tmpRate = dc.dbRntChnlRentalsUnitedEstateRateChangesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent);
                    if (tmpRate == null)
                    {
                        tmpRate = new dbRntChnlRentalsUnitedEstateRateChangesRL() { pidEstate = IdEstate, changeDate = dtCurrent };
                        dc.Add(tmpRate);
                    }
                    tmpRate.changeAmount = ntxt_rate_changeAmount.Value.objToInt32();
                    tmpRate.changeIsDiscount = drp_rate_changeIsDiscount.getSelectedValueInt();
                    tmpRate.changeIsPercentage = drp_rate_changeIsPercentage.getSelectedValueInt();
                    dc.SaveChanges();
                    dtCurrent = dtCurrent.AddDays(1);
                }
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            FillControls();
        }

        //Make changes if need to add Closed dates in HA
        protected void lnk_closed_save_Click(object sender, EventArgs e)
        {
            //using (DCchnlHoliday dc = new DCchnlHoliday())
            //{
            //    var currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
            //    if (currTbl == null)
            //    {
            //        Response.Redirect("/admin/rnt_estate_list.aspx");
            //        return;
            //    } string errorString = "";
            //    if (!rdp_closed_dtStart.SelectedDate.HasValue || !rdp_closed_dtEnd.SelectedDate.HasValue)
            //        errorString += "<br/>-specificare inizio e fine del range";
            //    else if (rdp_closed_dtStart.SelectedDate > rdp_closed_dtEnd.SelectedDate)
            //        errorString += "<br/>-la data di fine deve essere successiva alla data di inizio";
            //    if (errorString != "")
            //    {
            //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
            //        return;
            //    }
            //    DateTime dtCurrent = rdp_closed_dtStart.SelectedDate.Value.Date;
            //    while (dtCurrent <= rdp_closed_dtEnd.SelectedDate.Value.Date)
            //    {
            //        var tmp = dc.dbRntChnlHolidayEstateClosedDatesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.changeDate == dtCurrent);
            //        if (tmp == null)
            //        {
            //            tmp = new dbRntChnlHolidayEstateClosedDatesRL();
            //            tmp.pidEstate = IdEstate;
            //            tmp.changeDate = dtCurrent;
            //            dc.Add(tmp);
            //        }
            //        tmp.isClosed = drp_closed_isClosed.getSelectedValueInt();
            //        tmp.minStay = ntxt_closed_minStay.Value.objToInt32();
            //        if (UserAuthentication.CurrRoleTBL.viewOnlyNoSave.objToInt32() == 0) { dc.SaveChanges(); }
            //        dtCurrent = dtCurrent.AddDays(1);
            //    }

            //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);

            //    fillData();
        }

    }
}