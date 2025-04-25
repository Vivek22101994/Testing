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
    public partial class ucBooking : System.Web.UI.UserControl
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
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                clSearch _ls = _config.lastSearch;
                _config.addTo_myLastVisitedEstateList(IdEstate);
                clUtils.saveConfig(_config);

                drp_country_DataBind();
                drp_honorific_DataBind();
                fillData();
                showMode("new");


                string url_voucher;
                string filename;
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
            else
            {
                if (Request["__EVENTARGUMENT"] == "calculatePrice")
                {
                    checkAvailability();
                }
                if (Request["__EVENTARGUMENT"] == "calculateNumPersons")
                {
                    if (currEstateTB.is_chidren_allowed == 1)
                        calculateNumPersons();
                    checkAvailability();
                }
            }
            setCal();
            RegisterScripts();

            ltr_viewBookNow.Text = (currEstateTB.is_online_booking == 1) ? CurrentSource.getSysLangValue("reqBookNow") : CurrentSource.getSysLangValue("reqBookingRequest");
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "_RNT_validateBook", "$(function() {$(\"#" + txt_email.ClientID + ",#" + txt_email_conf.ClientID + "\").bind(\"cut copy paste\", function(event) { event.preventDefault();}); });", true);
        }

        protected void fillData()
        {
            HF_dtStart.Value = curr_ls.dtStart.JSCal_dateToString();
            HF_dtEnd.Value = curr_ls.dtEnd.JSCal_dateToString();

            drp_adult.bind_Numbers(1, currEstateTB.num_persons_max.objToInt32(), 1, 0);
            drp_adult.setSelectedValue(curr_ls.numPers_adult.ToString());
            if (currEstateTB.is_chidren_allowed == 1)
            {
                drp_child_over.bind_Numbers(1, (currEstateTB.num_persons_max.objToInt32() - curr_ls.numPers_adult), 1, 0);
                drp_child_over.Items.Insert(0, new ListItem("---", "0"));
                drp_child_over.setSelectedValue(curr_ls.numPers_childOver.ToString());
                drp_child_min.bind_Numbers(1, currEstateTB.num_persons_child.objToInt32(), 1, 0);
                drp_child_min.Items.Insert(0, new ListItem("---", "0"));
                drp_child_min.setSelectedValue(curr_ls.numPers_childMin.ToString());
            }
            else
            {
                pnl_child_min.Visible = pnl_child_over.Visible = false;
                pnlChildrenNotAllowed.Visible = true;
            }


            calculateNumPersons();
            checkReservationsCal();
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

        protected void drp_adult_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculateNumPersons();
            checkAvailability();
        }
        protected void calculateNumPersons()
        {
            if (currEstateTB.is_chidren_allowed == 1)
            {
                int _num_persons_min = currEstateTB.num_persons_min.objToInt32();
                int _num_persons_max = currEstateTB.num_persons_max.objToInt32();
                int _selNum_adult = drp_adult.getSelectedValueInt(0).objToInt32();
                int _selNum_child_over = drp_child_over.getSelectedValueInt(0).objToInt32();
                drp_child_over.Items.Clear();
                int _minChildOver = _num_persons_min - _selNum_adult;
                if (_minChildOver <= 0)
                {
                    _minChildOver = 1;
                    drp_child_over.Items.Add(new ListItem("---", "0"));
                }
                for (int i = _minChildOver; i <= (_num_persons_max - _selNum_adult); i++)
                {
                    drp_child_over.Items.Add(new ListItem("" + i, "" + i));
                }
                if (_selNum_child_over > (_num_persons_max - _selNum_adult)) _selNum_child_over = (_num_persons_max - _selNum_adult);
                drp_child_over.setSelectedValue("" + _selNum_child_over);
            }
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
        protected void lnk_calculatePrice_Click(object sender, EventArgs e)
        {
            checkAvailability();
        }
        public void checkAvailability()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RNT_estateCheckAvvReset" + Unique, "RNT_estateCheckAvvReset();", true);
            clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
            _config.lastSearch.numPers_adult = drp_adult.getSelectedValueInt(0).objToInt32();
            _config.lastSearch.numPers_childOver = drp_child_over.getSelectedValueInt(0).objToInt32();
            _config.lastSearch.numPers_childMin = drp_child_min.getSelectedValueInt(0).objToInt32();
            _config.lastSearch.numPersCount = _config.lastSearch.numPers_adult + _config.lastSearch.numPers_childOver;
            _config.lastSearch.dtStart = HF_dtStart.Value.JSCal_stringToDate();
            _config.lastSearch.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            _config.lastSearch.dtCount = (_config.lastSearch.dtEnd - _config.lastSearch.dtStart).TotalDays.objToInt32();
            _config.dtLastUsed = DateTime.Now;
            clUtils.saveConfig(_config);
            curr_ls = _config.lastSearch;

            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = curr_ls.dtStart;
            outPrice.dtEnd = curr_ls.dtEnd;
            outPrice.dtCount = curr_ls.dtCount;
            outPrice.numPersCount = curr_ls.numPersCount;
            outPrice.numPers_adult = curr_ls.numPers_adult;
            outPrice.numPers_childOver = curr_ls.numPers_childOver;
            outPrice.numPers_childMin = curr_ls.numPers_childMin;
            outPrice.pr_discount_owner = 0;
            outPrice.pr_discount_commission = 0;
            outPrice.part_percentage = currEstateTB.pr_percentage.objToDecimal();
            if (affiliatesarea.agentAuth.CurrentID != 0 && affiliatesarea.agentAuth.hasAcceptedContract == 1)
                using (DCmodRental dc = new DCmodRental())
                    outPrice.fillAgentDetails(dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID));
            decimal _pr_total = rntUtils.rntEstate_getPrice(0, IdEstate, ref outPrice);
            bool _isAvailable;
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                _isAvailable = dcOld.RNT_TBL_RESERVATION.Where(y => y.pid_estate == IdEstate //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= curr_ls.dtStart && y.dtEnd.Value.Date >= curr_ls.dtEnd) //
                                                                                || (y.dtStart.Value.Date >= curr_ls.dtStart && y.dtStart.Value.Date < curr_ls.dtEnd) //
                                                                                || (y.dtEnd.Value.Date > curr_ls.dtStart && y.dtEnd.Value.Date <= curr_ls.dtEnd))).Count() == 0;
            if (txt_promotionalCode.Text.Trim() != "" && _pr_total>0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var promoTbl = dc.dbRntDiscountPromoCodeTBLs.FirstOrDefault(x => x.code.ToLower().Trim() == txt_promotionalCode.Text.Trim().ToLower());
                    if (promoTbl != null)
                    {
                        decimal promoDiscount = _pr_total * promoTbl.discountAmount.objToDecimal() / 100;
                        _pr_total = _pr_total - promoDiscount;
                        //outPrice.pr_discount_owner = 0;
                        outPrice.pr_discount_commission = promoDiscount;
                    }
                }
            }
            
            outPrice.prTotal = _pr_total;
            currOutPrice = outPrice;
            lbl_priceError0.Visible = outPrice.outError == 0;
            lbl_priceError1.Visible = outPrice.outError == 1;
            lbl_priceError2.Visible = outPrice.outError == 2;
            lbl_priceError3.Visible = outPrice.outError == 3;
            lbl_priceError4.Visible = outPrice.outError ==4;

            //PH_formBookignCont.Visible = _isAvailable;
            HF_tmp_prTotal.Value = _pr_total.objToInt32().ToString();
            PH_bookPriceOK.Visible = _pr_total != 0;
            PH_priceDetails.Visible = _pr_total != 0;
            PH_bookPriceError.Visible = _pr_total == 0;
            PH_bookAvailable.Visible = _isAvailable;
            PH_bookNotAvailable.Visible = !_isAvailable;

            pnl_dicountCont.Visible = (currOutPrice.prDiscountLongStay + currOutPrice.prDiscountSpecialOffer + currOutPrice.pr_discount_commission) != 0;


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
            pnl_sendBooking.Visible = (currEstateTB.is_online_booking == 1 && _isAvailable && _pr_total != 0);
            pnl_inquire.Visible = (_pr_total == 0 || currEstateTB.is_online_booking != 1) && _isAvailable;
            yourBookingPrice.Visible = true;
            pnl_mr_rental_yousave.Visible = currOutPrice.prDiscountSpecialOffer > 0;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RNT_alternativeEstate_fill" + Unique, "RNT_alternativeEstate_fill(" + outPrice.numPersCount + ", " + outPrice.dtStart.JSCal_dateToInt() + ", " + outPrice.dtEnd.JSCal_dateToInt() + ", " + _pr_total.objToInt32() + ");", true);
        }

        protected void saveReservation(bool payNow)
        {
            RNT_TB_ESTATE estateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            bool _isAvailable = DC_RENTAL.RNT_TBL_RESERVATION.Where(y => y.pid_estate == IdEstate //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= curr_ls.dtStart && y.dtEnd.Value.Date >= curr_ls.dtEnd) //
                                                                                || (y.dtStart.Value.Date >= curr_ls.dtStart && y.dtStart.Value.Date < curr_ls.dtEnd) //
                                                                                || (y.dtEnd.Value.Date > curr_ls.dtStart && y.dtEnd.Value.Date <= curr_ls.dtEnd))).Count() == 0;
            if (estateTB == null || !_isAvailable)
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

            _currTBL.dtStart = currOutPrice.dtStart;
            _currTBL.dtEnd = currOutPrice.dtEnd;
            _currTBL.dtStartTime = "000000";
            _currTBL.dtEndTime = "000000";

            _currTBL.num_adult = currOutPrice.numPers_adult;
            _currTBL.num_child_over = currOutPrice.numPers_childOver;
            _currTBL.num_child_min = currOutPrice.numPers_childMin;

            currOutPrice.CopyTo(ref _currTBL);

            _currTBL.pr_deposit = estateTB.pr_deposit;
            _currTBL.srs_ext_meetingPoint = currEstateTB.srs_ext_meetingPoint;
            _currTBL.pr_depositWithCard = currEstateTB.pr_depositWithCard;

            _currTBL.pr_discount_owner = 0;
            _currTBL.pr_discount_commission = 0;
            _currTBL.pr_discount_desc = "";
            if (txt_promotionalCode.Text.Trim() != "" && currOutPrice.pr_discount_commission > 0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var promoTbl = dc.dbRntDiscountPromoCodeTBLs.FirstOrDefault(x => x.code.ToLower().Trim() == txt_promotionalCode.Text.Trim().ToLower());
                    if (promoTbl != null)
                    {
                        _currTBL.pr_discount_owner = 0;
                        _currTBL.pr_discount_commission = currOutPrice.pr_discount_commission;
                        _currTBL.pr_discount_desc = "-" + promoTbl.discountAmount.objToDecimal() + "% PromoCode: " + promoTbl.code + "";
                        _currTBL.pr_discount_custom = 1;
                    }
                }
            }

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

            //_currTBL.cl_browserInfo = ltr_cl_browserInfo.Text;
            _currTBL.cl_browserIP = Request.browserIP();

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