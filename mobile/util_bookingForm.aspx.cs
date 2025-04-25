using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.mobile
{
    public partial class util_bookingForm : mainBasePage
    {
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        private dbRntReservationTMP TMPcurrResTmp;
        public dbRntReservationTMP currResTmp
        {
            get
            {
                if (TMPcurrResTmp == null)
                    using (DCmodRental dc = new DCmodRental())
                        TMPcurrResTmp = dc.dbRntReservationTMPs.SingleOrDefault(x => x.id == Request.QueryString["tmpresid"].ToInt64());
                if (TMPcurrResTmp == null)
                {
                    Response.Redirect(App.ERROR_PAGE);
                    Response.End();
                }
                return TMPcurrResTmp ?? new dbRntReservationTMP();
            }
            set { TMPcurrResTmp = value; }
        }
        private RNT_TB_ESTATE tmpEstateTB;
        public RNT_TB_ESTATE currEstateTB
        {
            get
            {
                if (tmpEstateTB == null)
                    tmpEstateTB = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currResTmp.pidEstate);
                return tmpEstateTB ?? new RNT_TB_ESTATE();
            }
        }
        private RNT_LN_ESTATE tmpEstateLN;
        public RNT_LN_ESTATE currEstateLN
        {
            get
            {
                if (tmpEstateLN == null)
                    tmpEstateLN = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == currResTmp.pidEstate && x.pid_lang == App.LangID);
                return tmpEstateLN ?? new RNT_LN_ESTATE();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                drp_country_DataBind();
                drp_honorific_DataBind();
                RegisterScripts();
                fillData();
                showMode("new");
            }

        }
        protected void fillData()
        {
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            using (DCmodRental dc = new DCmodRental())
            {
                var tmpTBL = dc.dbRntReservationTMPs.SingleOrDefault(x => x.id == Request.QueryString["tmpresid"].ToInt64());
                if (tmpTBL == null)
                {
                    Response.Redirect("/");
                    return;
                }
                App.LangID = tmpTBL.cl_pid_lang.objToInt32();
                bool _isAvailable = rntUtils.rntEstate_isAvailable(tmpTBL.pidEstate.objToInt32(), tmpTBL.dtStart.Value, tmpTBL.dtEnd.Value, 0) == null;
                var currEstate = dcOld.RNT_TB_ESTATE.SingleOrDefault(x => x.id == tmpTBL.pidEstate);
                if (currEstate == null || !_isAvailable)
                {
                    Response.Redirect("/");
                    return;
                }
                //fill optioni extra
                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                outPrice.dtStart = tmpTBL.dtStart.Value;
                outPrice.dtEnd = tmpTBL.dtEnd.Value;
                outPrice.dtCount = (tmpTBL.dtEnd.Value - tmpTBL.dtStart.Value).TotalDays.objToInt32();
                outPrice.numPersCount = (tmpTBL.numPers_adult + tmpTBL.numPers_childOver).objToInt32();
                outPrice.numPers_adult = tmpTBL.numPers_adult.objToInt32();
                outPrice.numPers_childOver = tmpTBL.numPers_childOver.objToInt32();
                outPrice.numPers_childMin = tmpTBL.numPers_childMin.objToInt32();
                outPrice.pr_discount_owner = 0;
                outPrice.pr_discount_commission = 0;
                outPrice.part_percentage = currEstateTB.pr_percentage.objToDecimal();
                if (affiliatesarea.agentAuth.CurrentID != 0 && affiliatesarea.agentAuth.hasAcceptedContract == 1)
                {

                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                    if (agentTBL != null)
                        outPrice.fillAgentDetails(agentTBL);

                }

                string pr_total = rntUtils.rntEstate_getPrice(tmpTBL.id.objToInt64(), tmpTBL.pidEstate.objToInt32(), ref outPrice).ToString("N2");
                

            }
        }
        protected void RegisterScripts()
        {
            string _RNT_newClient_check = @"
			     function RNT_newClient_check() {
			         var _validate = true;
			         $(""#txt_email_check"").hide();
			         $(""#txt_email.ClientID "").removeClass(FORM_errorClass);
			         $(""#txt_email_conf_check"").hide();
			         $(""#txt_email_conf.ClientID "").removeClass(FORM_errorClass);
			         $(""#txt_name_first_check"").hide();
			         $(""#txt_name_first.ClientID "").removeClass(FORM_errorClass);
			         $(""#txt_name_last_check"").hide();
			         $(""#txt_name_last.ClientID "").removeClass(FORM_errorClass);
			         $(""#txt_phone_mobile_check"").hide();
			         $(""#txt_phone_mobile.ClientID "").removeClass(FORM_errorClass);
			         $(""#drp_country_check"").hide();
			         $(""#drp_country.ClientID "").removeClass(FORM_errorClass);
			         $(""#chk_privacyAgree_check"").hide();
			         $(""#chk_privacyAgree_cont"").removeClass(FORM_errorClass);
			         $(""#chk_termsOfService_check"").hide();
			         $(""#chk_termsOfService_cont"").removeClass(FORM_errorClass);
                        
			         if ($.trim($(""#txt_email.ClientID"").val()) == """") {
			             $(""#txt_email.ClientID"").addClass(FORM_errorClass);
			             $(""#txt_email_check"").html('#reqRequiredField#');
			             $(""#txt_email_check"").css(""display"", ""block"");
			             _validate = false;
			         }
			         else if (!FORM_validateEmail($(""#txt_email.ClientID"").val())) {
			             $(""#txt_email.ClientID"").addClass(FORM_errorClass);
			             $(""#txt_email_check"").html('#reqInvalidEmailFormat#');
			             $(""#txt_email_check"").css(""display"", ""block"");
			             _validate = false;
			         }
			         if ($.trim($(""#txt_name_first.ClientID"").val()) == """") {
			             $(""#txt_name_first.ClientID"").addClass(FORM_errorClass);
			             $(""#txt_name_first_check"").css(""display"", ""block"");
			             _validate = false;
			         }
			         if ($.trim($(""#txt_name_last.ClientID"").val()) == """") {
			             $(""#txt_name_last.ClientID"").addClass(FORM_errorClass);
			             $(""#txt_name_last_check"").css(""display"", ""block"");
			             _validate = false;
			         }
			         if ($.trim($(""#txt_phone_mobile.ClientID"").val()) == """") {
			             $(""#txt_phone_mobile.ClientID"").addClass(FORM_errorClass);
			             $(""#txt_phone_mobile_check"").css(""display"", ""block"");
			             _validate = false;
			         }
			         if ($.trim($(""#drp_country.ClientID"").val()) == """") {
			             $(""#drp_country.ClientID"").addClass(FORM_errorClass);
			             $(""#drp_country_check"").css(""display"", ""block"");
			             _validate = false;
			         }
			         if (!$(""#chk_privacyAgree"").is(':checked')) {
			             $(""#chk_privacyAgree_cont"").addClass(FORM_errorClass);
			             $(""#chk_privacyAgree_check"").css(""display"", ""block"");
			             _validate = false;
			         }
			         if (!$(""#chk_termsOfService"").is(':checked')) {
			             $(""#chk_termsOfService_cont"").addClass(FORM_errorClass);
			             $(""#chk_termsOfService_check"").css(""display"", ""block"");
			             _validate = false;
			         }
			         return _validate;
			     }
            ";
            _RNT_newClient_check = _RNT_newClient_check.Replace("#reqRequiredField#", contUtils.getLabel("reqRequiredField"));
            _RNT_newClient_check = _RNT_newClient_check.Replace("#reqInvalidEmailFormat#", contUtils.getLabel("reqInvalidEmailFormat"));
            _RNT_newClient_check = _RNT_newClient_check.Replace("txt_email.ClientID", txt_email.ClientID);
            _RNT_newClient_check = _RNT_newClient_check.Replace("txt_name_first.ClientID", txt_name_first.ClientID);
            _RNT_newClient_check = _RNT_newClient_check.Replace("txt_name_last.ClientID", txt_name_last.ClientID);
            _RNT_newClient_check = _RNT_newClient_check.Replace("txt_phone_mobile.ClientID", txt_phone_mobile.ClientID);
            _RNT_newClient_check = _RNT_newClient_check.Replace("drp_country.ClientID", drp_country.ClientID);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_RNT_newClient_check", _RNT_newClient_check, true);
            string _RNT_validateBook = @"
			    function RNT_validateBook() {
			        if($(""#HF_mode.ClientID"").val()==""new"")
                        return RNT_newClient_check()
			        if($(""#HF_mode.ClientID"").val()==""old"")
                        return true()
			    }
            "; 
            _RNT_validateBook = _RNT_validateBook.Replace("HF_mode.ClientID", HF_mode.ClientID);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_RNT_validateBook", _RNT_validateBook, true);
        }
        protected void lnk_goPwd_Click(object sender, EventArgs e)
        {
            showMode("pwd");
        }
        protected void lnk_pwdRevover_Click(object sender, EventArgs e)
        {
            if (txt_pwdRecover.Text.Trim() == "")
            {
                lbl_errorAlert.Text = contUtils.getLabel("reqRequiredField");
                lbl_errorAlert.Visible = true;
                return;
            }
            showMode("old");
            using (magaUser_DataContext dcAuth = maga_DataContext.DC_USER)
            using (DCmodRental dcRental = new DCmodRental())
            {
                var _client = dcAuth.USR_TBL_CLIENTs.FirstOrDefault(x => x.login == txt_pwdRecover.Text.Trim() || x.contact_email == txt_pwdRecover.Text.Trim());
                if (_client == null)
                {
                    lbl_errorAlert.Text = contUtils.getLabel("formUserNameOrEmailNotRegistered");
                    lbl_errorAlert.Visible = true;
                    return;
                }
                if (_client.is_active != 1)
                {
                    lbl_errorAlert.Text = "Your Account was disabled. <br/>Please Contact us for more information.";
                    lbl_errorAlert.Visible = true;
                    return;
                }
                if (AdminUtilities.usrClient_mailPwdRecovery(_client.id))
                {
                    lbl_errorAlert.Text = "You will receive an email with your username and password";
                    lbl_errorAlert.Visible = true;
                }
                else
                {
                    lbl_errorAlert.Text = "Error on sending E-mail";
                    lbl_errorAlert.Visible = true;
                }
            }
        }
        protected void lnk_login_Click(object sender, EventArgs e)
        {
            showMode("view");
            using (magaUser_DataContext dcAuth = maga_DataContext.DC_USER)
            using (DCmodRental dcRental = new DCmodRental())
            {
                var _client = dcAuth.USR_TBL_CLIENTs.FirstOrDefault(x => x.login == txt_old_email.Text.Trim() && x.password == txt_old_password.Text.Trim());
                if (_client == null)
                {
                    showMode("old");
                    lbl_errorAlert.Text = contUtils.getLabel("formWrongUsernameOrPassword");
                    lbl_errorAlert.Visible = true;
                    return;
                }
                if (_client.is_active != 1)
                {
                    showMode("old");
                    lbl_errorAlert.Text = "Your Account was disabled. <br/>Please Contact us for more information.";
                    lbl_errorAlert.Visible = true;
                    return;
                }
                HF_IdClient.Value = _client.id.ToString();
                ltr_country.Text = _client.loc_country;
                ltr_email.Text = _client.contact_email;
                ltr_honorific.Text = _client.name_honorific;
                ltr_name_full.Text = _client.name_full;
                ltr_phone_mobile.Text = _client.contact_phone_mobile;
            }
        }
        protected void drp_honorific_DataBind()
        {
            List<USR_LK_HONORIFIC> _listAll = maga_DataContext.DC_USER.USR_LK_HONORIFICs.ToList();
            List<USR_LK_HONORIFIC> _list = _listAll.Where(x => x.pid_lang == CurrentLang.ID).ToList();
            if (_list.Count() == 0)
                _list = _listAll.Where(x => x.pid_lang == 2).ToList();
            drp_honorific.DataSource = _list;
            drp_honorific.DataTextField = "title";
            drp_honorific.DataValueField = "title";
            drp_honorific.DataBind();
        }
        protected void drp_country_DataBind()
        {
            drp_country.DataSource = authProps.CountryLK.OrderBy(x => x.title);
            drp_country.DataTextField = "title";
            drp_country.DataValueField = "title";
            drp_country.DataBind();
            drp_country.Items.Insert(0, new ListItem("- - -", ""));
            if (App.LangID != 2)
            {
                var currCountry = authProps.CountryLK.SingleOrDefault(x => x.code.ToLower() == App.LangCultureName.Substring(3, 2).ToLower());
                if (currCountry != null)
                    drp_country.setSelectedValue(currCountry.title);
            }
        }
        protected void showMode(string mode)
        {
            PH_viewClient.Visible = mode == "view";
            PH_oldClient.Visible = mode == "old";
            PH_newClient.Visible = mode == "new";
            PH_pwdRecover.Visible = mode == "pwd";
            PH_bookingForm.Visible = mode != "old" && mode != "pwd";
            HF_mode.Value = mode;
            lbl_errorAlert.Visible = false;
        }
        protected void lnk_goNewClient_Click(object sender, EventArgs e)
        {
            showMode("new");
        }
        protected void lnk_goOldClient_Click(object sender, EventArgs e)
        {
            showMode("old");
        }
        protected bool newClient()
        {
            if (txt_email.Text.Trim() == "")
            {
                lbl_errorAlert.Text = "The Email field cannot be empty.";
                lbl_errorAlert.Visible = true;
                return false;
            }

            using (magaUser_DataContext dcAuth = maga_DataContext.DC_USER)
            using (DCmodRental dcRental = new DCmodRental())
            {
                var _client = dcAuth.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == 0 && x.contact_email == txt_email.Text.Trim());// cerchiamo id=0, cosi non trova mai un doppione
                if (_client != null && _client.is_active == 1)
                {
                    lbl_errorAlert.Text = "The Email is already registered Please try to login.";
                    showMode("old");
                    lbl_errorAlert.Visible = true;
                    return false;
                }
                if (_client != null && _client.is_active != 1)
                {
                    lbl_errorAlert.Text = "The Email was already registered previously, but was disabled by Administration.<br/>Please Contact us for more information.";
                    lbl_errorAlert.Visible = true;
                    return false;
                }
                HF_IdClient.Value = "0";
                ltr_country.Text = drp_country.SelectedValue;
                ltr_email.Text = txt_email.Text;
                ltr_honorific.Text = drp_honorific.SelectedItem.Text;
                ltr_name_full.Text = txt_name_first.Text + " " + txt_name_last.Text;
                ltr_phone_mobile.Text = txt_phone_mobile.Text;
                return true;
            }
        }
        public USR_TBL_CLIENT getIdClient()
        {
            USR_TBL_CLIENT _client;
            if (affiliatesarea.agentAuth.CurrentID != 0 && affiliatesarea.agentAuth.hasAcceptedContract == 1)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                    if (agentTBL != null)
                    {
                        _client = new USR_TBL_CLIENT();
                        _client.id = -1;
                        _client.contact_email = agentTBL.contactEmail;
                        _client.name_full = agentTBL.nameCompany;
                        _client.name_honorific = "";
                        _client.loc_country = agentTBL.locCountry;
                        _client.pid_discount = -1;
                        _client.pid_lang = agentTBL.pidLang;
                        _client.isCompleted = 0;
                        return _client;
                    }
                }
            }
            magaUser_DataContext DC_USER = maga_DataContext.DC_USER;
            _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == HF_IdClient.Value.ToInt32());
            if (_client != null) return _client;
            _client = new USR_TBL_CLIENT();
            _client.loc_country = ltr_country.Text;
            _client.contact_email = ltr_email.Text;
            _client.name_honorific = ltr_honorific.Text;
            _client.name_full = ltr_name_full.Text;
            _client.contact_phone_mobile = ltr_phone_mobile.Text;
            _client.loc_country = ltr_country.Text;
            _client.pid_lang = CurrentLang.ID;
            _client.isCompleted = 0;
            _client.is_deleted = 0;
            _client.is_active = 1;
            _client.date_created = DateTime.Now;
            _client.login = _client.contact_email;
            _client.password = CommonUtilities.CreatePassword(8, false, true, false);
            DC_USER.USR_TBL_CLIENTs.InsertOnSubmit(_client);
            DC_USER.SubmitChanges();
            _client.code = _client.id.ToString().fillString("0", 7, false);
            DC_USER.SubmitChanges();
            AdminUtilities.usrClient_mailNewCreation(_client.id); // send mails
            return _client;
        }
        protected void saveReservation(bool payNow)
        {
            var tmpTBL = currResTmp;
            RNT_TB_ESTATE estateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currEstateTB.id);
            bool _isAvailable = rntUtils.rntEstate_isAvailable(currEstateTB.id, tmpTBL.dtStart.Value, tmpTBL.dtEnd.Value, 0) == null;
            if (estateTB == null || !_isAvailable)
            {
                return;
            }
            if (HF_mode.Value == "new" && !newClient()) return;
            USR_TBL_CLIENT _client = getIdClient();
            RNT_TBL_RESERVATION _currTBL = rntUtils.newReservation();
            tmpTBL.CopyTo(ref _currTBL);

            _currTBL.pid_creator = 55;
            _currTBL.pid_operator = 55;

            _currTBL.state_pid = 6;
            _currTBL.state_body = "";
            _currTBL.state_date = DateTime.Now;
            _currTBL.state_pid_user = 1;
            _currTBL.state_subject = "Nuovo dal sito Mobile";
            _currTBL.is_booking = 1;
            _currTBL.creatorHost = "mobile";


            _currTBL.dtStartTime = "000000";
            _currTBL.dtEndTime = "000000";

            _currTBL.srs_ext_meetingPoint = currEstateTB.srs_ext_meetingPoint;
            _currTBL.pr_depositWithCard = currEstateTB.pr_depositWithCard;

            DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);


            // scadenze instant booking
            int _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHoursOnline").ToInt32();
            _currTBL.block_comments = "Scadenza predefinita InstantBooking [" + _blockHours + " ore]";
            if (_currTBL.agentID != 0)
            {
                _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHoursAgent").ToInt32();
                _currTBL.block_comments = "Scadenza predefinita Agenzie [" + _blockHours + " ore]";
            }
            _currTBL.block_expire = DateTime.Now.AddHours(_blockHours);
            _currTBL.block_expire_hours = _blockHours;
            _currTBL.block_pid_user = 1;
            
            _currTBL.cl_id = _client.id;
            _currTBL.cl_email = _client.contact_email;
            _currTBL.cl_name_full = _client.name_full;
            _currTBL.cl_name_honorific = _client.name_honorific;
            _currTBL.cl_loc_country = _client.loc_country;
            _currTBL.cl_pid_discount = _client.pid_discount;
            _currTBL.cl_pid_lang = _client.pid_lang;
            _currTBL.cl_isCompleted = _client.isCompleted;

            _currTBL.IdAdMedia = App.IdAdMedia;
            _currTBL.IdLink = App.IdLink;
            _currTBL.IdLastOperator = App.IdLastOperator;

            //_currTBL.cl_browserInfo = ltr_cl_browserInfo.Text;
            //_currTBL.cl_browserIP = ltr_cl_browserIP.Text;

            RNT_TBL_REQUEST _request = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == Request.QueryString["IdRequest"].ToInt32());
            if (_request != null)
            {
                _currTBL.pid_related_request = _request.id;
                _currTBL.pid_operator = _request.pid_operator;
                _request.pid_reservation = _currTBL.id;
                _request.state_body = "";
                _request.state_subject = "Diventato prenotazione";
                _request.state_date = DateTime.Now;
                _request.state_pid = 5;
                _request.state_pid_user = 1;
                List<RNT_TBL_REQUEST> _list = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.pid_related_request == _request.id).ToList();
                foreach (RNT_TBL_REQUEST _req in _list)
                {
                    _req.state_body = _request.state_body;
                    _req.state_subject = _request.state_subject;
                    _req.state_date = _request.state_date;
                    _req.state_pid = _request.state_pid;
                    _req.state_pid_user = _request.state_pid_user;
                    _req.pid_reservation = _request.pid_reservation;
                }
            }
            else
            {
                int pidOperator = 0;
                if (_currTBL.agentID != 0)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                        if (agentTBL != null)
                        {
                            pidOperator = agentTBL.pidReferer.objToInt32();
                        }
                    }
                }
                if (pidOperator == 0) pidOperator = AdminUtilities.usr_getAvailableOperator(AdminUtilities.zone_countryId(_client.loc_country), _currTBL.cl_pid_lang.objToInt32());
                _currTBL.pid_operator = pidOperator;
            }
            _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
            _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            rntUtils.rntReservation_setDefaults(ref _currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            rntUtils.rntReservation_mailNewCreation(_currTBL, true, true, true, true, 1); // send mails
            rntUtils.reservation_checkPartPayment(_currTBL, false);
            using (DCmodRental dc = new DCmodRental())
            {
                tmpTBL = dc.dbRntReservationTMPs.SingleOrDefault(x => x.id == Request.QueryString["tmpresid"].ToInt64());
                if (tmpTBL != null)
                {
                    dc.Delete(tmpTBL);
                    dc.SaveChanges();
                }
            }
            INV_TBL_PAYMENT _pay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.FirstOrDefault(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete != 1);
            if (payNow && _pay != null)
            {
                Response.Redirect(App.HOST_SSL + "/util_paypal_redirect.aspx?type=payment&code=" + _pay.code + "&lang=" + CurrentLang.ID);
                return;
            }
        }

        protected void lnk_payNow_Click(object sender, EventArgs e)
        {
            saveReservation(true);
        }
    }
}