using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.uc
{
    public partial class UC_rnt_reservation_client : System.Web.UI.UserControl
    {
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public long IdReservation
        {
            get
            {
                return HF_IdReservation.Value.ToInt32();
            }
            set
            {
                HF_IdReservation.Value = value.ToString();
            }
        }
        private RNT_TBL_RESERVATION _currTBL;
        private magaRental_DataContext DC_RENTAL;
        private magaUser_DataContext DC_USER;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                RegisterScripts();
                lnk_saveClientData.OnClientClick = "return _validateForm_" + Unique;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal_"+Unique, "setCal_"+Unique+"();", true);
                
            }
        }
        protected void Bind_drp_doc_type()
        {
            List<USR_TB_DOC_TYPE> _list = DC_USER.USR_TB_DOC_TYPEs.ToList();
            drp_doc_type.DataSource = _list;
            drp_doc_type.DataTextField = "title";
            drp_doc_type.DataValueField = "code";
            drp_doc_type.DataBind();
            drp_doc_type.Items.Insert(0, new ListItem("", ""));
        }

        protected void lnk_saveClientData_Click(object sender, EventArgs e)
        {
            saveData();
        }
        public void fillData()
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_USER = maga_DataContext.DC_USER;
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null) return;
            USR_TBL_CLIENT _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currTBL.cl_id);
            if (_client == null)
            {
                _client = new USR_TBL_CLIENT();
                _client.pid_lang = _currTBL.cl_pid_lang;
                _client.pid_discount = _currTBL.cl_pid_discount;
                _client.name_full = _currTBL.cl_name_full;
                _client.name_honorific = _currTBL.cl_name_honorific;
            }

            txt_birth_place.Text = _client.birth_place;
            HF_birth_date.Value =_client.birth_date.HasValue? _client.birth_date.JSCal_dateToString():"19800101";
            Bind_drp_doc_type();
            drp_doc_type.setSelectedValue(_client.doc_type);
            txt_doc_num.Text = _client.doc_num;
            txt_doc_issue_place.Text = _client.doc_issue_place;
            HF_doc_expiry_date.Value = _client.doc_expiry_date.HasValue ? _client.doc_expiry_date.JSCal_dateToString() : "20150101";
            txt_loc_address.Text = _client.loc_address;
            txt_loc_state.Text = _client.loc_state;
            txt_loc_zip_code.Text = _client.loc_zip_code;
            txt_loc_city.Text = _client.loc_city;
            txt_contact_phone_mobile.Text = _client.contact_phone_mobile;
            txt_contact_phone_trip.Text = _client.contact_phone_trip;
            txt_contact_phone.Text = _client.contact_phone;
            txt_contact_fax.Text = _client.contact_fax;
            txt_contact_email.Text = _currTBL.cl_email;

        }
        protected void saveData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null) return;
            USR_TBL_CLIENT _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currTBL.cl_id);
            if (_client == null)
            {
                _client = new USR_TBL_CLIENT();
                _client.pid_lang = _currTBL.cl_pid_lang;
                _client.pid_discount = _currTBL.cl_pid_discount;
                _client.name_full = _currTBL.cl_name_full;
                _client.name_honorific = _currTBL.cl_name_honorific;
                _client.login = txt_contact_email.Text;
                _client.password = CommonUtilities.CreatePassword(8, false, true, false);
                DC_USER.USR_TBL_CLIENTs.InsertOnSubmit(_client);
                DC_USER.SubmitChanges();
                _currTBL.cl_id = _client.id;
            }

            _client.birth_place = txt_birth_place.Text;
            _client.birth_date = HF_birth_date.Value.JSCal_stringToDate();
            _client.doc_type =drp_doc_type.SelectedValue;
            _client.doc_num =txt_doc_num.Text;
            _client.doc_issue_place =txt_doc_issue_place.Text;
            _client.doc_expiry_date = HF_doc_expiry_date.Value.JSCal_stringToDate();
            _client.loc_address = txt_loc_address.Text;
            _client.loc_state = txt_loc_state.Text;
            _client.loc_zip_code = txt_loc_zip_code.Text;
            _client.loc_city = txt_loc_city.Text;
            _client.contact_phone_mobile = txt_contact_phone_mobile.Text;
            _client.contact_phone_trip = txt_contact_phone_trip.Text;
            _client.contact_phone = txt_contact_phone.Text;
            _client.contact_fax = txt_contact_fax.Text;
            _client.contact_email = txt_contact_email.Text;
            _client.isCompleted = 1;
            _currTBL.cl_email = txt_contact_email.Text;
            _currTBL.cl_isCompleted = 1;
            DC_RENTAL.SubmitChanges();
            DC_USER.SubmitChanges();
        }
        protected void RegisterScripts()
        {
            string s = @"
			     function RNT_newClient_check() {
			         var _validate = true;
			         $(""#txt_email_check"").hide();
			         $(""#txt_email.ClientID "").removeClass(FORM_errorClass);
			         $(""#txt_email_conf_check"").hide();
			         $(""#txt_email_conf.ClientID "").removeClass(FORM_errorClass);
			         $(""#txt_name_full_check"").hide();
			         $(""#txt_name_full.ClientID "").removeClass(FORM_errorClass);
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
			         if ($.trim($(""#txt_name_full.ClientID"").val()) == """") {
			             $(""#txt_name_full.ClientID"").addClass(FORM_errorClass);
			             $(""#txt_name_full_check"").css(""display"", ""block"");
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
            //_RNT_newClient_check = _RNT_newClient_check.Replace("#reqRequiredField#", CurrentSource.getSysLangValue("reqRequiredField"));
            //_RNT_newClient_check = _RNT_newClient_check.Replace("#reqInvalidEmailFormat#", CurrentSource.getSysLangValue("reqInvalidEmailFormat"));
            //_RNT_newClient_check = _RNT_newClient_check.Replace("txt_email.ClientID", txt_email.ClientID);
            //_RNT_newClient_check = _RNT_newClient_check.Replace("txt_email_conf.ClientID", txt_email_conf.ClientID);
            //_RNT_newClient_check = _RNT_newClient_check.Replace("txt_name_full.ClientID", txt_name_full.ClientID);
            //_RNT_newClient_check = _RNT_newClient_check.Replace("txt_phone_mobile.ClientID", txt_phone_mobile.ClientID);
            //_RNT_newClient_check = _RNT_newClient_check.Replace("drp_country.ClientID", drp_country.ClientID);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_RNT_newClient_check", _RNT_newClient_check, true);
            string _validateForm = @"
			    function _validateForm_#Unique#() {
                   return true;
			    }
            ";
            _validateForm = _validateForm.Replace("#Unique#", Unique);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_validateForm_" + Unique, _validateForm, true);
            string setCal_ = @"
                var cal_birth_date_#Unique#;
                var cal_doc_expiry_date_#Unique#;
			    function setCal_#Unique#() {
                    cal_birth_date_#Unique# = new JSCal.Single({ dtFormat: ""d MM yy"", Cont: ""#HF_birth_date.ClientID"", View: ""#txt_birth_date"", changeMonth: true, changeYear: true });
                    cal_doc_expiry_date_#Unique# = new JSCal.Single({ dtFormat: ""d MM yy"", Cont: ""#HF_doc_expiry_date.ClientID"", View: ""#txt_doc_expiry_date"", changeMonth: true, changeYear: true });
			    }
            ";
            setCal_ = setCal_.Replace("#Unique#", Unique);
            setCal_ = setCal_.Replace("HF_birth_date.ClientID", HF_birth_date.ClientID);
            setCal_ = setCal_.Replace("HF_doc_expiry_date.ClientID", HF_doc_expiry_date.ClientID);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_setCal_" + Unique, setCal_, true);
        }
    }
}