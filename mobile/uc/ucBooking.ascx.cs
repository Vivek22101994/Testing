using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModAuth;
using ModRental;
using RentalInRome.data;

namespace RentalInRome.mobile.uc
{
    public partial class ucBooking : System.Web.UI.UserControl
    {
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        public string CURRENT_SESSION_ID
        {
            get
            {
                if (HF_CURRENT_SESSION_ID.Value == "")
                {
                    mainBasePage m = (mainBasePage)this.Page;
                    HF_CURRENT_SESSION_ID.Value = m.CURRENT_SESSION_ID;
                }
                return HF_CURRENT_SESSION_ID.Value;
            }
            set
            {
                HF_CURRENT_SESSION_ID.Value = value;
            }
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = string.Empty.createUniqueID();
                return HF_unique.Value;
            }
        }
        public int IdEstate
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
            }
        }
        private RNT_TB_ESTATE tmpEstateTB;
        public RNT_TB_ESTATE currEstateTB
        {
            get
            {
                if (tmpEstateTB == null)
                    tmpEstateTB = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                return tmpEstateTB ?? new RNT_TB_ESTATE();
            }
        }
        private RNT_LN_ESTATE tmpEstateLN;
        public RNT_LN_ESTATE currEstateLN
        {
            get
            {
                if (tmpEstateLN == null)
                    tmpEstateLN = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == App.LangID);
                return tmpEstateLN ?? new RNT_LN_ESTATE();
            }
        }
        private clSearch tmp_ls;
        public clSearch curr_ls
        {
            get
            {
                if (tmp_ls == null)
                {
                    clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                    tmp_ls = _config.lastSearch;
                }
                return tmp_ls ?? new clSearch();
            }
            set { tmp_ls = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["IdRequest"]))
                {
                    RNT_TBL_REQUEST tblRequest = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == Request.QueryString["IdRequest"].ToInt32());
                    if (tblRequest != null)
                    {
                        HF_IdRequest.Value = tblRequest.id + "";
                        curr_ls.dtStart = tblRequest.request_date_start.Value;
                        curr_ls.dtEnd = tblRequest.request_date_end.Value;
                        curr_ls.numPers_adult = tblRequest.request_adult_num.objToInt32();
                        curr_ls.numPers_childOver = tblRequest.request_child_num.objToInt32();
                        curr_ls.numPers_childMin = tblRequest.request_child_num_min.objToInt32();
                        fillData();
                        if (Request.QueryString["booknow"] == "true")
                            BookNow(tblRequest.id);
                    }
                }
                fillData();
            }
            setCal();
        }
        protected void fillData()
        {
            HF_dtStart.Value = curr_ls.dtStart.JSCal_dateToString();
            HF_dtEnd.Value = curr_ls.dtEnd.JSCal_dateToString();
            drp_numPers_adult.bind_Numbers(1, currEstateTB.num_persons_max.objToInt32(), 1, 0);
            drp_numPers_adult.setSelectedValue(curr_ls.numPers_adult.ToString());
            drp_numPers_children.bind_Numbers(1, (currEstateTB.num_persons_max.objToInt32() - 2), 1, 0);
            drp_numPers_children.Items.Insert(0, new ListItem("---", "0"));
            drp_numPers_children.setSelectedValue(curr_ls.numPers_childOver.ToString());
            drp_numPers_infants.bind_Numbers(1, currEstateTB.num_persons_child.objToInt32(), 1, 0);
            drp_numPers_infants.Items.Insert(0, new ListItem("---", "0"));
            drp_numPers_infants.setSelectedValue(curr_ls.numPers_childMin.ToString());
            checkReservationsCal();
        }
        protected void setCal()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal_" + Unique, "setCal_" + Unique + "(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);
        }
        protected void checkReservationsCal()
        {
            string _script = "";
            _script += "function checkCalDates_" + Unique + "(date){var _dtStart = parseInt($('#" + HF_dtStart.ClientID + "').val()); var _dtEnd = parseInt($('#" + HF_dtEnd.ClientID + "').val()); var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
            _script += rntUtils.rntEstate_availableDatesForJSCal(IdEstate, 0);
            _script += "if (dateint > _dtStart && dateint < _dtEnd) { _controls += '<span class=\"rntCal sel_f\"></span>'; }";
            _script += "if (dateint == _dtStart) { _controls += '<span class=\"rntCal sel_1\"></span>'; }";
            _script += "if (dateint == _dtEnd) { _controls += '<span class=\"rntCal sel_2\"></span>'; }";
            _script += "if (_controls.indexOf('<span class=\"rntCal nd_2\"></span>') != -1 && _controls.indexOf('<span class=\"rntCal nd_1\"></span>') != -1) { _enabled = false; }";
            _script += "return [_enabled, _class, _tooltip, _controls];";
            _script += "}";
            ltr_checkCalDates.Text = _script;
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "_JSCal_Range_" + Unique, "var _JSCal_Range_" + Unique + " = new JSCal.Range();", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "checkCalDates_" + Unique, _script, true);
        }
        protected void BookNow(int pidRelatedRequest)
        {
            var tmpTBL = new dbRntReservationTMP();
            tmpTBL.createdDate = DateTime.Now;
            tmpTBL.createdUserID = UserAuthentication.CurrentUserID;
            tmpTBL.createdUserNameFull = UserAuthentication.CurrentUserName;
            tmpTBL.pidEstate = IdEstate;
            tmpTBL.pidEstateCountry = 108;
            tmpTBL.pidEstateCity = currEstateTB.pid_city.objToInt32();
            tmpTBL.pidRelatedRequest = pidRelatedRequest;
            tmpTBL.dtStart = HF_dtStart.Value.JSCal_stringToDate();
            tmpTBL.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            tmpTBL.numPers_adult = drp_numPers_adult.SelectedValue.ToInt32();
            tmpTBL.numPers_childMin = drp_numPers_children.SelectedValue.ToInt32();
            tmpTBL.numPers_childOver = drp_numPers_infants.SelectedValue.ToInt32();
            var currRequest = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == tmpTBL.pidRelatedRequest);
            if (currRequest != null)
            {

                tmpTBL.pidOperator = currRequest.pid_operator.objToInt32();
                tmpTBL.dtStart = currRequest.request_date_start.HasValue ? currRequest.request_date_start.Value : tmpTBL.dtStart;
                tmpTBL.dtEnd = currRequest.request_date_end.HasValue ? currRequest.request_date_end.Value : tmpTBL.dtEnd;
                tmpTBL.numPers_adult = currRequest.request_adult_num.objToInt32();
                tmpTBL.numPers_childMin = currRequest.request_child_num_min.objToInt32();
                tmpTBL.numPers_childOver = currRequest.request_child_num.objToInt32();
            }
            if (tmpTBL.dtStart < DateTime.Now || tmpTBL.dtEnd < DateTime.Now)
            {
                lnkBookNow.Visible = false;
                return;
            }


            tmpTBL.cl_pid_lang = App.LangID;

            //tmpTBL.cl_loc_country = drp_country.SelectedValue;
            //tmpTBL.cl_email = txt_email.Text;
            //tmpTBL.cl_name_honorific = drp_honorific.SelectedValue;
            //tmpTBL.cl_name_full = txt_name_full.Text;
            //tmpTBL.cl_contact_phone_mobile = txt_phone_mobile.Text;

            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = tmpTBL.dtStart.Value;
            outPrice.dtEnd = tmpTBL.dtEnd.Value;
            outPrice.numPersCount = tmpTBL.numPers_adult.objToInt32() + tmpTBL.numPers_childOver.objToInt32();
            outPrice.numPers_adult = tmpTBL.numPers_adult.objToInt32();
            outPrice.numPers_childOver = tmpTBL.numPers_childOver.objToInt32();

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
            outPrice.part_percentage = currEstateTB.pr_percentage.objToDecimal();
            tmpTBL.pr_total = rntUtils.rntEstate_getPrice(0, tmpTBL.pidEstate.objToInt32(), ref outPrice);
            outPrice.CopyTo(ref tmpTBL);

            tmpTBL.pr_deposit = currEstateTB.pr_deposit;
            tmpTBL.pr_isManual = 0;

            using (DCmodRental dc = new DCmodRental())
            {
                dc.Add(tmpTBL);
                dc.SaveChanges();
                Response.Redirect("/m/booking?tmpresid=" + tmpTBL.id);
            }
        }
        protected void lnkBookNow_Click(object sender, EventArgs e)
        {
            BookNow(HF_IdRequest.Value.ToInt32());
        }

    }
}