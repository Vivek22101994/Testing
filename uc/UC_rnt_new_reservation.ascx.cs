using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.uc
{
    public partial class UC_rnt_new_reservation : System.Web.UI.UserControl
    {
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public string CURRENT_SESSION_ID
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                return m.CURRENT_SESSION_ID;
            }
        }
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
        public int IdRequest
        {
            get
            {
                return HF_IdRequest.Value.ToInt32();
            }
            set
            {
                HF_IdRequest.Value = value.ToString();
            }
        }
        public int is_online_booking
        {
            get
            {
                return HF_is_online_booking.Value.ToInt32();
            }
            set
            {
                HF_is_online_booking.Value = value.ToString();
                lnk_payNow.Visible = HF_is_online_booking.Value == "1";
                lnk_payNow.Text = "<span>" + CurrentSource.getSysLangValue("reqBookNow") + "</span>";
                lnk_book.Text = HF_is_online_booking.Value == "1" ? "<span>" + CurrentSource.getSysLangValue("reqPayingByBankTransfer") + "</span>" : "<span>" + CurrentSource.getSysLangValue("reqBookingRequest") + "</span>";
            }
        }
        public int lpb_nights_min
        {
            get
            {
                return HF_lpb_nights_min.Value.ToInt32();
            }
            set
            {
                HF_lpb_nights_min.Value = value.ToString();
            }
        }
        public decimal pr_deposit
        {
            get
            {
                return HF_pr_deposit.Value.ToDecimal();
            }
            set
            {
                HF_pr_deposit.Value = value.ToString();
            }
        }
        public bool pr_depositWithCard
        {
            get
            {
                return HF_pr_depositWithCard.Value == "1";
            }
            set
            {
                HF_pr_depositWithCard.Value = value ? "1" : "0";
            }
        }
        public decimal pr_percentage
        {
            get
            {
                return HF_pr_percentage.Value.ToDecimal();
            }
            set
            {
                HF_pr_percentage.Value = value.ToString();
            }
        }
        public DateTime dtStart
        {
            get
            {
                return HF_dtStart.Value.JSCal_stringToDate();
            }
            set
            {
                HF_dtStart.Value = value.JSCal_dateToString();
            }
        }
        public DateTime dtEnd
        {
            get
            {
                return HF_dtEnd.Value.JSCal_stringToDate();
            }
            set
            {
                HF_dtEnd.Value = value.JSCal_dateToString();
            }
        }
        public int dtCount
        {
            get
            {
                return HF_dtCount.Value.ToInt32();
            }
            set
            {
                HF_dtCount.Value = value.ToString();
            }
        }
        public int num_adult
        {
            get
            {
                return HF_num_adult.Value.ToInt32();
            }
            set
            {
                HF_num_adult.Value = value.ToString();
            }
        }
        public int num_child_over
        {
            get
            {
                return HF_num_child_over.Value.ToInt32();
            }
            set
            {
                HF_num_child_over.Value = value.ToString();
            }
        }
        public int num_child_min
        {
            get
            {
                return HF_num_child_min.Value.ToInt32();
            }
            set
            {
                HF_num_child_min.Value = value.ToString();
            }
        }
        public string srs_ext_meetingPoint
        {
            get
            {
                return ltr_srs_ext_meetingPoint.Text;
            }
            set
            {
                ltr_srs_ext_meetingPoint.Text = value;
            }
        }
        private rntExts.PreReservationPrices TMPcurrOutPrice;
        public rntExts.PreReservationPrices currOutPrice
        {
            get
            {
                if (TMPcurrOutPrice == null)
                    TMPcurrOutPrice = (rntExts.PreReservationPrices)ViewState[Unique + "_currOutPrice"];
                return TMPcurrOutPrice ?? new rntExts.PreReservationPrices();
            }
            set { TMPcurrOutPrice = value; ViewState[Unique + "_currOutPrice"] = TMPcurrOutPrice; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                // get Browser info
                System.Web.HttpBrowserCapabilities browser = Request.Browser;
                string _cl_browserInfo = "Browser Capabilities\n"
                    + "Type = " + browser.Type + "\n"
                    + "Name = " + browser.Browser + "\n"
                    + "Version = " + browser.Version + "\n"
                    + "Major Version = " + browser.MajorVersion + "\n"
                    + "Minor Version = " + browser.MinorVersion + "\n"
                    + "Platform = " + browser.Platform + "\n"
                    + "Is Beta = " + browser.Beta + "\n"
                    + "Is Crawler = " + browser.Crawler + "\n"
                    + "Is AOL = " + browser.AOL + "\n"
                    + "Is Win16 = " + browser.Win16 + "\n"
                    + "Is Win32 = " + browser.Win32 + "\n"
                    + "Supports Frames = " + browser.Frames + "\n"
                    + "Supports Tables = " + browser.Tables + "\n"
                    + "Supports Cookies = " + browser.Cookies + "\n"
                    + "Supports VBScript = " + browser.VBScript + "\n"
                    + "Supports JavaScript = " +
                        browser.EcmaScriptVersion.ToString() + "\n"
                    + "Supports Java Applets = " + browser.JavaApplets + "\n"
                    + "Supports ActiveX Controls = " + browser.ActiveXControls
                          + "\n"
                    + "Supports JavaScript Version = " +
                        browser["JavaScriptVersion"] + "\n";
                ltr_cl_browserInfo.Text = _cl_browserInfo;
                string _cl_browserIP = "";
                try { _cl_browserIP = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                ltr_cl_browserIP.Text = _cl_browserIP;

                string url_voucher;
                string filename;
                RegisterScripts();
                CONT_VIEW_STP _stp = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == 19 && item.pid_lang == CurrentLang.ID);
                if (_stp == null)
                    _stp = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == 19 && item.pid_lang == 2);
                if (_stp != null)
                    ltr_terms.Text = _stp.description;
                url_voucher = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "stp_contentonly.aspx?id=19&lang=" + CurrentLang.ID;
                filename = "RiR_" + CurrentSource.getSysLangValue("lblTermsAndConditions").clearPathName() + ".pdf";
                HL_getPdf_terms.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher.urlEncode() + "&filename=" + filename.urlEncode();
                _stp = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == 2 && item.pid_lang == CurrentLang.ID);
                if (_stp == null)
                    _stp = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == 2 && item.pid_lang == 2);
                if (_stp != null)
                    ltr_privacy.Text = _stp.description;
                url_voucher = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "stp_contentonly.aspx?id=2&lang=" + CurrentLang.ID;
                filename = "RiR_" + CurrentSource.getSysLangValue("lblPrivacyPolicy").clearPathName() + ".pdf";
                HL_getPdf_privacy.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher.urlEncode() + "&filename=" + filename.urlEncode();
                showMode("new");
                Bind_drp_honorific();
                if (affiliatesarea.agentAuth.CurrentID != 0 && affiliatesarea.agentAuth.hasAcceptedContract == 1)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                        if (agentTBL != null)
                        {
                            showMode("view");
                            ltr_country.Text = agentTBL.locCountry;
                            ltr_email.Text = agentTBL.contactEmail;
                            ltr_honorific.Text = agentTBL.nameHonor;
                            ltr_name_full.Text = agentTBL.nameFull;
                            //ltr_phone_mobile.Text = _client.contact_phone_mobile;
                        }
                    }
                }

            }
            ltr_viewBookNow.Text = (HF_is_online_booking.Value == "1") ? CurrentSource.getSysLangValue("reqBookNow") : CurrentSource.getSysLangValue("reqBookingRequest");
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "_RNT_validateBook", "$(function() {$(\"#" + txt_email.ClientID + ",#" + txt_email_conf.ClientID + "\").bind(\"cut copy paste\", function(event) { event.preventDefault();}); });", true);
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
			         if ($.trim($(""#txt_email_conf.ClientID"").val()) != $.trim($(""#txt_email.ClientID "").val())) {
			             $(""#txt_email_conf.ClientID"").addClass(FORM_errorClass);
			             $(""#txt_email_conf_check"").css(""display"", ""block"");
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
            _RNT_newClient_check = _RNT_newClient_check.Replace("#reqRequiredField#", CurrentSource.getSysLangValue("reqRequiredField"));
            _RNT_newClient_check = _RNT_newClient_check.Replace("#reqInvalidEmailFormat#", CurrentSource.getSysLangValue("reqInvalidEmailFormat"));
            _RNT_newClient_check = _RNT_newClient_check.Replace("txt_email.ClientID", txt_email.ClientID);
            _RNT_newClient_check = _RNT_newClient_check.Replace("txt_email_conf.ClientID", txt_email_conf.ClientID);
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
            if (txt_pwdRecover.Text.Trim()=="")
            {
                lbl_errorAlert.Text = CurrentSource.getSysLangValue("reqRequiredField");
                lbl_errorAlert.Visible = true;
                return;
            }
            showMode("old");
            USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.FirstOrDefault(x => x.is_deleted != 1 && (x.login == txt_pwdRecover.Text.Trim() || x.contact_email == txt_pwdRecover.Text.Trim()));
            if (_client == null)
            {
                lbl_errorAlert.Text = CurrentSource.getSysLangValue("formUserNameOrEmailNotRegistered");
                lbl_errorAlert.Visible = true;
                return;
            }
            if (_client.is_active != 1)
            {
                lbl_errorAlert.Text = "Your Account was disabled. <br/>Please Contact us for more information.";
                lbl_errorAlert.Visible = true;
                return;
            }
            if(AdminUtilities.usrClient_mailPwdRecovery(_client.id))
            {
                lbl_errorAlert.Text = "You will receive an email with your username and password";
                lbl_errorAlert.Visible = true;
            }
        }
        protected void lnk_login_Click(object sender, EventArgs e)
        {
            showMode("view");
            USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.is_deleted != 1 && x.login == txt_old_email.Text.Trim() && x.password == txt_old_password.Text.Trim());
            if (_client == null)
            {
                showMode("old");
                lbl_errorAlert.Text = CurrentSource.getSysLangValue("formWrongUsernameOrPassword");
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
        protected void Bind_drp_honorific()
        {
            List<USR_LK_HONORIFIC> _listAll = maga_DataContext.DC_USER.USR_LK_HONORIFICs.ToList();
            List<USR_LK_HONORIFIC> _list = _listAll.Where(x => x.pid_lang == CurrentLang.ID).ToList();
            if (_list.Count() == 0)
                _list = _listAll.Where(x => x.pid_lang == 2).ToList();
            drp_honorific.DataSource = _list;
            drp_honorific.DataTextField = "title";
            drp_honorific.DataValueField = "title";
            drp_honorific.DataBind();
            //drp_honorific.Items.Insert(0, new ListItem("-sel-", ""));
        }
        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem("- - -", ""));
            if (CurrentLang.ID != 2)
            {
                LOC_LK_COUNTRY _c = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.FirstOrDefault(
                        x => x.inner_notes.ToLower() == CurrentLang.NAME.Substring(3, 2).ToLower());
                if (_c != null)
                    drp_country.setSelectedValue(_c.id.ToString());
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
                return  false;
            }
            if (txt_email.Text.Trim() != txt_email_conf.Text.Trim())
            {
                lbl_errorAlert.Text = "The two E-mail do not match.";
                lbl_errorAlert.Visible = true;
                return false;
            }

            USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.is_deleted != 1 && x.contact_email == txt_email.Text.Trim());
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


        public USR_TBL_CLIENT getIdClient(rntExts.PreReservationPrices outPrice)
        {
            USR_TBL_CLIENT _client;
            if (outPrice.agentID != 0 )
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == outPrice.agentID);
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
        public void checkAvailability(int _IdEstate, DateTime _dtStart, DateTime _dtEnd, int _dtCount, int _num_adult, int _num_child_over, int _num_child_min)
        {
            clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
            _config.lastSearch.numPers_adult = _num_adult;
            _config.lastSearch.numPers_childOver = _num_child_over;
            _config.lastSearch.numPers_childMin = _num_child_min;
            _config.lastSearch.dtStart = _dtStart;
            _config.lastSearch.dtEnd = _dtEnd;
            _config.dtLastUsed = DateTime.Now;
            clUtils.saveConfig(_config);
            IdEstate = _IdEstate;
            dtStart = _dtStart;
            dtEnd = _dtEnd;
            dtCount = (_dtEnd - _dtStart).Days;
            num_adult = _num_adult;
            num_child_over = _num_child_over;
            num_child_min = _num_child_min;
            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = dtStart;
            outPrice.dtEnd = dtEnd;
            outPrice.numPersCount = (num_adult + num_child_over);
            outPrice.pr_discount_owner = 0;
            outPrice.pr_discount_commission = 0;
            outPrice.part_percentage = pr_percentage;
            if (affiliatesarea.agentAuth.CurrentID != 0 && affiliatesarea.agentAuth.hasAcceptedContract == 1)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                    if (agentTBL != null)
                    {
                        outPrice.agentID = agentTBL.id;
                        outPrice.agentDiscountType = agentTBL.pidDiscountType.objToInt32();
                        outPrice.agentDiscountNotPayed = agentTBL.payDiscountNotPayed.objToInt32(); ;
                        outPrice.requestFullPayAccepted = agentTBL.payFullPayment.objToInt32();
                        if (outPrice.agentDiscountType == 0) outPrice.agentDiscountType = 1;
                    }
                }
            }
            decimal _pr_total = rntUtils.rntEstate_getPrice(0, IdEstate, ref outPrice);
            bool _isAvailable;
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                _isAvailable = dcOld.RNT_TBL_RESERVATION.Where(y => y.pid_estate == IdEstate //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart <= dtStart && y.dtEnd >= dtEnd) //
                                                                                || (y.dtStart >= dtStart && y.dtStart < dtEnd) //
                                                                                || (y.dtEnd > dtStart && y.dtEnd <= dtEnd))).Count() == 0;
            outPrice.prTotal = _pr_total;
            currOutPrice = outPrice;
            lbl_priceError0.Visible = outPrice.outError == 0;
            lbl_priceError1.Visible = outPrice.outError == 1;
            lbl_priceError2.Visible = outPrice.outError == 2;
            lbl_priceError3.Visible = outPrice.outError == 3;

            //PH_formBookignCont.Visible = _isAvailable;
            HF_tmp_prTotal.Value = _pr_total.objToInt32().ToString();
            PH_bookPriceOK.Visible = _pr_total != 0;
            PH_priceDetails.Visible = _pr_total != 0;
            PH_bookPriceError.Visible = _pr_total == 0;
            PH_bookAvailable.Visible = _isAvailable;
            PH_bookNotAvailable.Visible = !_isAvailable;

            pnl_dicountCont.Visible = (currOutPrice.prDiscountLongStay + currOutPrice.prDiscountSpecialOffer) != 0;


            ltr_sel_priceDetails.Text = "<ul class=\"dett_day\">";
            currOutPrice.priceDetails = currOutPrice.priceDetails.OrderBy(x => x.sequence).ToList();
            foreach (rntExts.RNT_estatePriceDetails _priceDetail in currOutPrice.priceDetails.Where(x => x.type == 1))
            {
                ltr_sel_priceDetails.Text += "<li>";
                ltr_sel_priceDetails.Text += "  <span class=\"data\">" + _priceDetail.dt.formatITA(false) + "</span>";
                ltr_sel_priceDetails.Text += "  <span class=\"euro\">€ " + (_priceDetail.price + _priceDetail.priceOpt).ToString("N2") + "</span>";
                ltr_sel_priceDetails.Text += "</li>";
            }
            foreach (rntExts.RNT_estatePriceDetails _priceDetail in currOutPrice.priceDetails.Where(x => x.type == 2))
            {
                ltr_sel_priceDetails.Text += "<li>";
                ltr_sel_priceDetails.Text += "  <span class=\"data\">" + _priceDetail.description+ "</span>";
                ltr_sel_priceDetails.Text += "  <span class=\"euro\">€ " + (_priceDetail.price).ToString("N2") + "</span>";
                ltr_sel_priceDetails.Text += "</li>";
            }
            foreach (rntExts.RNT_estatePriceDetails _priceDetail in currOutPrice.priceDetails.Where(x => x.type == 3))
            {
                ltr_sel_priceDetails.Text += "<li>";
                ltr_sel_priceDetails.Text += "  <span class=\"data\">" + _priceDetail.description + "</span>";
                ltr_sel_priceDetails.Text += "  <span class=\"euro\">€ " + (_priceDetail.price).ToString("N2") + "</span>";
                ltr_sel_priceDetails.Text += "</li>";
            }
            ltr_sel_priceDetails.Text += "</ul>";
            pnl_sendBooking.Visible = (is_online_booking == 1 && _isAvailable && _pr_total != 0);
            pnl_inquire.Visible = (_pr_total == 0 || is_online_booking != 1) && _isAvailable;
            this.Visible = true;
        }

        protected void saveReservation(bool payNow)
        {
            RNT_TB_ESTATE estateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            bool _isAvailable = DC_RENTAL.RNT_TBL_RESERVATION.Where(y => y.pid_estate == IdEstate //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart <= dtStart && y.dtEnd >= dtEnd) //
                                                                                || (y.dtStart >= dtStart && y.dtStart < dtEnd) //
                                                                                || (y.dtEnd > dtStart && y.dtEnd <= dtEnd))).Count() == 0;
            if (estateTB==null || !_isAvailable)
            {
                pnl_sendBooking.Visible = false;
                PH_bookAvailable.Visible = false;
                PH_bookNotAvailable.Visible = true;
                return;
            }
            if (HF_mode.Value == "new" && !newClient()) return;
            USR_TBL_CLIENT _client = getIdClient(currOutPrice);
            RNT_TBL_RESERVATION _currTBL = rntUtils.newReservation();
            _currTBL.pid_estate = IdEstate;
            _currTBL.pidEstateCity = estateTB.pid_city.objToInt32();
            _currTBL.is_deleted = 0;
            _currTBL.pid_creator = 1;
            _currTBL.state_pid = 6;
            _currTBL.state_body = "";
            _currTBL.state_date = DateTime.Now;
            _currTBL.state_pid_user = 1;
            _currTBL.state_subject = "Nuovo dal sito online";
            _currTBL.is_booking = 1;

            _currTBL.dtStart = dtStart;
            _currTBL.dtEnd = dtEnd;
            _currTBL.dtStartTime = "000000";
            _currTBL.dtEndTime = "000000";

            _currTBL.num_adult = num_adult;
            _currTBL.num_child_over = num_child_over;
            _currTBL.num_child_min = num_child_min;

            currOutPrice.CopyTo(ref _currTBL);

            _currTBL.pr_deposit = estateTB.pr_deposit;
            _currTBL.srs_ext_meetingPoint = srs_ext_meetingPoint;
            _currTBL.pr_depositWithCard = pr_depositWithCard ? 1 : 0;

            _currTBL.pr_discount_owner = 0;
            _currTBL.pr_discount_commission = 0;
            _currTBL.pr_discount_desc = "";

            _currTBL.IdAdMedia = App.IdAdMedia;
            _currTBL.IdLink = App.IdLink;
            // salviamo prima e aggiorniamo la cache per evitare overbooking
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

            _currTBL.cl_browserInfo = ltr_cl_browserInfo.Text;
            _currTBL.cl_browserIP = ltr_cl_browserIP.Text;

            RNT_TBL_REQUEST _request = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == IdRequest);
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
                // abbinare all'account dell'agenzia, in caso di scaccini o maurizio lecce a rental, id= 37
                int pidOperator = 0;
                if (_currTBL.agentID!=0)
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
                if (pidOperator == 15 && pidOperator == 31) pidOperator = 37;
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
            //INV_TBL_PAYMENT _pay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.FirstOrDefault(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete != 1);
            //if (payNow && _pay!=null)
            //{
            //    Response.Redirect("https://rentalinrome.com/util_paypal_redirect.aspx?type=payment&code=" + _pay.code + "&lang=" + CurrentLang.ID);
            //}
            Response.Redirect(CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id);
        }

        protected void lnk_book_Click(object sender, EventArgs e)
        {
            saveReservation(false);
        }
        protected void lnk_payNow_Click(object sender, EventArgs e)
        {
            saveReservation(true);
        }
    }
}