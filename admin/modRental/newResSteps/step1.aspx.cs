using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental.newResSteps
{
    public partial class step1 : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        private dbRntReservationTMP TMPcurrTBL;
        protected dbRntReservationTMP currTBL
        {
            get
            {
                if (TMPcurrTBL == null)
                    if (ViewState["currTBL"] != null)
                        TMPcurrTBL = PConv.DeserObj((object[])ViewState["currTBL"], typeof(dbRntReservationTMP)) as dbRntReservationTMP;
                    else
                        TMPcurrTBL = new dbRntReservationTMP();
                return TMPcurrTBL;
            }
            set
            {
                ViewState["currTBL"] = PConv.SerialObj(value);
                TMPcurrTBL = value;
            }
        }
        protected void fillControls()
        {
            if (Request.QueryString["IdRequest"] != null)
            {
                int idReq = Request.QueryString["IdRequest"].objToInt32();
                if (idReq != 0)
                {
                    RNT_TBL_REQUEST currReq = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == idReq);
                    if (currReq != null)
                    {
                        txt_email.Text = currReq.email;
                        txt_name_full.Text = currReq.name_full;
                        txt_phone_mobile.Text = currReq.phone;
                        drp_lang.DataBind();
                        drp_lang.setSelectedValue(currReq.pid_lang);
                        drp_country.DataBind();
                        drp_country.setSelectedValue(currReq.request_country);
                    }
                }
            }
            if (currTBL.agentID != 0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == currTBL.agentID);
                    if (agentTBL != null)
                    {
                        txt_email.Text = agentTBL.contactEmail;
                        txt_name_full.Text = agentTBL.nameCompany;
                        txt_phone_mobile.Text = agentTBL.contactPhone;
                        drp_lang.DataBind();
                        drp_lang.setSelectedValue(agentTBL.pidLang);
                        drp_country.DataBind();
                        drp_country.setSelectedValue(agentTBL.locCountry);
                        txt_email.Enabled = false;
                        txt_name_full.Enabled = false;
                        txt_phone_mobile.Enabled = false;
                        drp_lang.Enabled = false;
                        drp_country.Enabled = false;
                    }
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                
                var tmpTBL = new dbRntReservationTMP();
                tmpTBL.createdDate = DateTime.Now;
                tmpTBL.createdUserID = UserAuthentication.CurrentUserID;
                tmpTBL.createdUserNameFull = UserAuthentication.CurrentUserName;
                tmpTBL.pidEstate = Request.QueryString["IdEstate"].objToInt32();
                var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == tmpTBL.pidEstate);
                if (currEstate == null)
                {
                    CloseRadWindow("");
                    currTBL = new dbRntReservationTMP();
                    UpdatePanel1.Visible = false;
                    return;
                }
                tmpTBL.agentID = Request.QueryString["agentID"].objToInt64();
                tmpTBL.pidEstateCity = currEstate.pid_city.objToInt32();
                tmpTBL.pidRelatedRequest = Request.QueryString["IdRequest"].objToInt32();
                tmpTBL.dtStart = Request.QueryString["dtStart"].JSCal_stringToDate();
                tmpTBL.dtEnd = Request.QueryString["dtEnd"].JSCal_stringToDate();
                tmpTBL.numPers_adult = Request.QueryString["numPers"].ToInt32();
                tmpTBL.numPers_childMin = 0;
                tmpTBL.numPers_childOver = 0;
                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                var currRequest = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == tmpTBL.pidRelatedRequest);
                if (currRequest != null)
                {

                    tmpTBL.pidOperator = currRequest.pid_operator.objToInt32();
                    tmpTBL.dtStart = currRequest.request_date_start.HasValue ? currRequest.request_date_start.Value : tmpTBL.dtStart;
                    tmpTBL.dtEnd = currRequest.request_date_end.HasValue ? currRequest.request_date_end.Value : tmpTBL.dtEnd;
                    tmpTBL.numPers_adult = currRequest.request_adult_num.objToInt32();
                    tmpTBL.numPers_childMin = currRequest.request_child_num_min.objToInt32();
                    tmpTBL.numPers_childOver = currRequest.request_child_num.objToInt32();
                    outPrice.isFreeMinStay = currRequest.isFreeMinStay;
                    outPrice.isFreeArrivalDay = currRequest.isFreeArrivalDay;
                    if (!string.IsNullOrEmpty(currRequest.IdAdMedia))
                        using (DCmodRental dc = new DCmodRental())
                        {
                            var agentTBL = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == currRequest.IdAdMedia);
                            if (agentTBL != null)
                                tmpTBL.agentID = agentTBL.id;
                        }
                }
                if (tmpTBL.dtStart < new DateTime(2010, 1, 1) || tmpTBL.dtEnd < new DateTime(2010, 1, 1))
                {
                    CloseRadWindow("");
                    currTBL = new dbRntReservationTMP();
                    UpdatePanel1.Visible = false;
                    return;
                }

                outPrice.dtStart = tmpTBL.dtStart.Value;
                outPrice.dtEnd = tmpTBL.dtEnd.Value;
                outPrice.numPersCount = tmpTBL.numPers_adult.objToInt32() + tmpTBL.numPers_childOver.objToInt32();
                decimal agentTotalResPrice = 0;
                if (tmpTBL.agentID.objToInt64() != 0)
                {
                    using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == tmpTBL.agentID.objToInt64());
                        if (agentTBL != null)
                        {
                            outPrice.fillAgentDetails(agentTBL);
                            DateTime checkDate = DateTime.Now;
                            DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                            DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                            var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTBL.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                            agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                        }
                    }
                }
                outPrice.agentTotalResPrice = agentTotalResPrice;
                outPrice.part_percentage = currEstate.pr_percentage.objToDecimal();
                tmpTBL.pr_total = rntUtils.rntEstate_getPrice(0, tmpTBL.pidEstate.objToInt32(), ref outPrice);
                lnk_save.Visible = tmpTBL.pr_total > 0;
                currTBL = tmpTBL;
                Bind_drp_honorific();
                fillControls();
                if (outPrice.dtStart < DateTime.Now.Date)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "warningAlert", "alert('Attenzione! La data del checkin è precedente alla data odierna.');", true);
                }
            }
        }
        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem("- - -", ""));
        }
        protected void drp_lang_DataBound(object sender, EventArgs e)
        {
            drp_lang.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
        protected void Bind_drp_honorific()
        {
            List<USR_LK_HONORIFIC> _list = maga_DataContext.DC_USER.USR_LK_HONORIFICs.OrderBy(x => x.title).ToList();
            drp_honorific.DataSource = _list;
            drp_honorific.DataTextField = "title";
            drp_honorific.DataValueField = "title";
            drp_honorific.DataBind();
        }
        protected long saveData(int pr_isManual)
        {
            var tmpTBL = currTBL;
            var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == tmpTBL.pidEstate);
            if (currEstate == null)
            {
                CloseRadWindow("");
                currTBL = new dbRntReservationTMP();
                UpdatePanel1.Visible = false;
                return 0;
            }
            tmpTBL.cl_loc_country = drp_country.SelectedValue;
            tmpTBL.cl_email = txt_email.Text;
            tmpTBL.cl_name_honorific = drp_honorific.SelectedValue;
            tmpTBL.cl_name_full = txt_name_full.Text;
            tmpTBL.cl_contact_phone_mobile = txt_phone_mobile.Text;
            tmpTBL.cl_pid_lang = drp_lang.getSelectedValueInt(0);

            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = tmpTBL.dtStart.Value;
            outPrice.dtEnd = tmpTBL.dtEnd.Value;
            outPrice.numPersCount = tmpTBL.numPers_adult.objToInt32() + tmpTBL.numPers_childOver.objToInt32();
            decimal agentTotalResPrice = 0;
            if (tmpTBL.agentID.objToInt64() != 0)
            {
                using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == tmpTBL.agentID.objToInt64());
                    if (agentTBL != null)
                    {
                        outPrice.fillAgentDetails(agentTBL);
                        DateTime checkDate = DateTime.Now;
                        DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                        DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                        var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTBL.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                        agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                    }
                }
            }
            outPrice.agentTotalResPrice = agentTotalResPrice;
            outPrice.part_percentage = currEstate.pr_percentage.objToDecimal();
            tmpTBL.pr_total = rntUtils.rntEstate_getPrice(0, tmpTBL.pidEstate.objToInt32(), ref outPrice);
            //tmpTBL.prOptioniExtra = outPrice.prOptioniExtra;
            //tmpTBL.pr_part_OptioniExtra = outPrice.pr_part_OptioniExtra;
            //tmpTBL.pr_optioni_feeling = outPrice.prOptioniFeeling;
            //tmpTBL.pr_optioni_owner = outPrice.prOptioniOwner;
            var currRequest = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == tmpTBL.pidRelatedRequest);
            if (currRequest != null)
            {

                outPrice.isFreeMinStay = currRequest.isFreeMinStay;
                outPrice.isFreeArrivalDay = currRequest.isFreeArrivalDay;
            }
            outPrice.CopyTo(ref tmpTBL);
            tmpTBL.pr_deposit = currEstate.pr_deposit;

            tmpTBL.pr_isManual = pr_isManual;
            using (DCmodRental dc = new DCmodRental())
            {
                dc.Add(tmpTBL);
                dc.SaveChanges();
                return tmpTBL.id;
            }
        }
        protected void lnk_saveManual_Click(object sender, EventArgs e)
        {
            Response.Redirect("step2.aspx?id=" + saveData(1));
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            Response.Redirect("step3.aspx?id=" + saveData(0));
        }

    }
}