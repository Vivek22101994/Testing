using ModAuth;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace RentalInRome.reservationarea
{
    public partial class personaldetails : basePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (resUtils.CurrentIdReservation_gl != 0 && resUtils.CurrentIdReservation_gl < 150000)
            {
                Response.Redirect("/reservationarea/arrivaldeparture.aspx", true);
                return;
            }
        }
      
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        
        private magaRental_DataContext DC_RENTAL;        
        private RNT_TBL_RESERVATION _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
          
            if (!IsPostBack)
            {
                RegisterScripts();
                fillData();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal_" + Unique, "setCal_" + Unique + "();", true);
        }
     
        protected void pnlSendMailWithVoucher_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "sendmail")
            {
                _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
                if (_currTBL == null)
                {
                    Response.Redirect("/reservationarea/login.aspx", true);
                    return;
                }
                bool mailBCC = false;
                string filePath = Path.Combine(App.SRP, "files");
                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                filePath = Path.Combine(filePath, "tmp");
                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                filePath = Path.Combine(filePath, ("").createUniqueID());
                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                filePath = Path.Combine(filePath, "reservation_voucher-code_" + _currTBL.code + ".pdf");

                string pdfUrl = "";
                string mailSubject = "";
                
                if (App.WLAgentId > 0)
                {
                    string WL_domainName = "http://" + Request.Url.Host;
                    pdfUrl = WL_domainName + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_voucher.aspx?uid=" + _currTBL.uid_2;

                    mailSubject = "Rental in Rome: Reservation Voucher #" + _currTBL.code;
                }
                else
                {
                    pdfUrl = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_voucher.aspx?uid=" + _currTBL.uid_2;

                    mailSubject = WL.getWLName() +": Reservation Voucher #" + _currTBL.code;
                    
                }
                string mailBody = "See the attachment file or download your voucher directly from the link below<br/><br/><a href='" + pdfUrl + "'>" + pdfUrl + "</a>";
                if (pdfUtils.savePdfFromUrl(pdfUrl, filePath, 0.3f, 0.3f, 0.3f, 0.3f) == "ok" && MailingUtilities.autoSendMailTo(mailSubject, mailBody, _currTBL.cl_email, new List<mailUtils.AttachmentItem>() { new mailUtils.AttachmentItem(filePath) }, mailBCC, "WL reservationarea.personaldata.pnlSendMailWithVoucher_AjaxRequest"))
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "callAlert(\"The e-mail with voucher was sent to " + _currTBL.cl_email + ". \");", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "callAlert(\"There was an error in sending e-mail. \");", true);
            }
        }

        #region 
        //protected void drp_country_DataBound(object sender, EventArgs e)
        //{           
        //    drp_country.Items.Insert(0, new ListItem("- - -", ""));
        //}

        //protected void drp_inv_loc_country_DataBound(object sender, EventArgs e)
        //{
        //    drp_inv_loc_country.Items.Insert(0, new ListItem("- - -", ""));
        //} 
        #endregion

        #region Bind Dropdowns
        protected void FillDropdown()
        {
            var CountryLK = authProps.CountryLK.OrderBy(x => x.title);
            drp_country.DataSource = CountryLK;
            drp_country.DataTextField = "title";
            drp_country.DataValueField = "title";
            drp_country.DataBind();
            drp_country.Items.Insert(0, new ListItem("- - -", ""));


            drp_inv_loc_country.DataSource = CountryLK;
            drp_inv_loc_country.DataTextField = "title";
            drp_inv_loc_country.DataValueField = "title";
            drp_inv_loc_country.DataBind();
            drp_inv_loc_country.Items.Insert(0, new ListItem("- - -", ""));
        }
        #endregion

        protected void fillData()
        {
            //drp_country.DataBind();
            //drp_inv_loc_country.DataBind();
            FillDropdown();

            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            using (DCmodAuth dc = new DCmodAuth())
            {
                var currAgentClient = dc.dbAuthClientTBLs.FirstOrDefault(x => x.id == _currTBL.agentClientID);
                //USR_TBL_CLIENT _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currTBL.cl_id);
                if (currAgentClient == null)
                {
                    currAgentClient = new dbAuthClientTBL();
                    currAgentClient.pidLang = _currTBL.cl_pid_lang + "";
                    //_client.pid_discount = _currTBL.cl_pid_discount;
                    currAgentClient.nameFull = _currTBL.cl_name_full;
                    currAgentClient.nameHonorific = _currTBL.cl_name_honorific;
                }
                Hf_nameFull.Value = _currTBL.cl_name_full;

                txt_birth_place.Text = currAgentClient.birthPlace;
                HF_birth_date.Value = currAgentClient.birthDate.HasValue ? currAgentClient.birthDate.JSCal_dateToString() : "19800101";
                Bind_drp_doc_type();
                drp_doc_type.setSelectedValue(currAgentClient.docType);
                txt_doc_cf_num.Text = currAgentClient.docCf;
                txt_doc_vat_num.Text = currAgentClient.docVat;
                txt_doc_num.Text = currAgentClient.docNum;
                txt_doc_issue_place.Text = currAgentClient.docIssuePlace;
                HF_doc_issue_date.Value = currAgentClient.docIssueDate.HasValue ? currAgentClient.docIssueDate.JSCal_dateToString() : "0";
                HF_doc_expiry_date.Value = currAgentClient.docExpiryDate.HasValue ? currAgentClient.docExpiryDate.JSCal_dateToString() : "20150101";
                txt_loc_address.Text = currAgentClient.locAddress;
                txt_loc_state.Text = currAgentClient.locState;
                txt_loc_zip_code.Text = currAgentClient.locZipCode;
                txt_loc_city.Text = currAgentClient.locCity;
                txt_contact_phone_mobile.Text = currAgentClient.contactPhoneMobile;
                txt_contact_phone_trip.Text = currAgentClient.contactPhoneTrip;
                txt_contact_phone.Text = currAgentClient.contactPhone;
                txt_contact_fax.Text = currAgentClient.contactFax;
                txt_contact_email.Text = _currTBL.cl_email;
                drp_country.setSelectedValue(currAgentClient.locCountry);

                drp_inv_isDifferent.setSelectedValue(_currTBL.inv_isDifferent);
                invDetailsCheck();
                //txt_inv_name_honorific.Text = _currTBL.inv_name_honorific;
                txt_inv_name_full.Text = _currTBL.inv_name_full;
                drp_inv_loc_country.setSelectedValue(_currTBL.inv_loc_country);
                txt_inv_loc_state.Text = _currTBL.inv_loc_state;
                txt_inv_loc_city.Text = _currTBL.inv_loc_city;
                txt_inv_loc_address.Text = _currTBL.inv_loc_address;
                txt_inv_loc_zip_code.Text = _currTBL.inv_loc_zip_code;
                txt_inv_doc_vat_num.Text = _currTBL.inv_doc_vat_num;
                txt_inv_doc_cf_num.Text = _currTBL.inv_doc_cf_num;
            }

        }
        
        protected void saveData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            using (DCmodAuth dc = new DCmodAuth())
            {
                var currAgentClient = dc.dbAuthClientTBLs.FirstOrDefault(x => x.id == _currTBL.agentClientID);
                //USR_TBL_CLIENT _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currTBL.cl_id);
                if (currAgentClient == null)
                {
                    currAgentClient = new dbAuthClientTBL();

                    currAgentClient.uid = Guid.NewGuid();
                    currAgentClient.createdDate = DateTime.Now;
                    currAgentClient.createdUserID = 1;
                    currAgentClient.createdUserNameFull = "System";

                    currAgentClient.isActive = 1;
                    currAgentClient.typeCode = "clientiagenzie";
                    currAgentClient.pidAgent = App.WLAgentId;

                    currAgentClient.pidLang = _currTBL.cl_pid_lang + "";
                    //_client.pid_discount = _currTBL.cl_pid_discount;
                    currAgentClient.nameFull = _currTBL.cl_name_full;
                    currAgentClient.nameHonorific = _currTBL.cl_name_honorific;
                    currAgentClient.authUsr = txt_contact_email.Text;
                    currAgentClient.authPwd = CommonUtilities.CreatePassword(8, false, true, false);
                    dc.Add(currAgentClient);
                    dc.SaveChanges();
                    currAgentClient.code = currAgentClient.id.ToString().fillString("0", 6, false);
                    _currTBL.agentClientID = currAgentClient.id;
                }

                currAgentClient.birthPlace = txt_birth_place.Text;
                currAgentClient.birthDate = HF_birth_date.Value.JSCal_stringToDate();
                currAgentClient.docType = drp_doc_type.SelectedValue;
                currAgentClient.docCf = txt_doc_cf_num.Text;
                currAgentClient.docVat = txt_doc_vat_num.Text;
                currAgentClient.docNum = txt_doc_num.Text;
                currAgentClient.docIssuePlace = txt_doc_issue_place.Text;
                currAgentClient.docIssueDate = HF_doc_issue_date.Value.JSCal_stringToDate();
                currAgentClient.docExpiryDate = HF_doc_expiry_date.Value.JSCal_stringToDate();
                currAgentClient.locAddress = txt_loc_address.Text;
                currAgentClient.locState = txt_loc_state.Text;
                currAgentClient.locZipCode = txt_loc_zip_code.Text;
                currAgentClient.locCity = txt_loc_city.Text;
                currAgentClient.contactPhoneMobile = txt_contact_phone_mobile.Text;
                currAgentClient.contactPhoneTrip = txt_contact_phone_trip.Text;
                currAgentClient.contactPhone = txt_contact_phone.Text;
                currAgentClient.contactFax = txt_contact_fax.Text;
                currAgentClient.contactEmail = txt_contact_email.Text;
                currAgentClient.locCountry = drp_country.SelectedValue;
                dc.SaveChanges();

                _currTBL.cl_email = txt_contact_email.Text;
                _currTBL.cl_isCompleted = 1;

                _currTBL.inv_isDifferent = drp_inv_isDifferent.getSelectedValueInt();
                if (_currTBL.inv_isDifferent == 1)
                {
                    //_currTBL.inv_name_honorific = source.inv_name_honorific;
                    _currTBL.inv_name_full = txt_inv_name_full.Text;
                    _currTBL.inv_loc_country = drp_inv_loc_country.SelectedValue;
                    _currTBL.inv_loc_state = txt_inv_loc_state.Text;
                    _currTBL.inv_loc_city = txt_inv_loc_city.Text;
                    _currTBL.inv_loc_address = txt_inv_loc_address.Text;
                    _currTBL.inv_loc_zip_code = txt_inv_loc_zip_code.Text;
                    _currTBL.inv_doc_vat_num = txt_inv_doc_vat_num.Text;
                    _currTBL.inv_doc_cf_num = txt_inv_doc_cf_num.Text;
                }
                else
                {
                    //_currTBL.inv_name_honorific = source.inv_name_honorific;
                    _currTBL.inv_name_full = _currTBL.cl_name_full;
                    _currTBL.inv_loc_country = currAgentClient.locCountry;
                    _currTBL.inv_loc_state = currAgentClient.locState;
                    _currTBL.inv_loc_city = currAgentClient.locCity;
                    _currTBL.inv_loc_address = currAgentClient.locAddress;
                    _currTBL.inv_loc_zip_code = currAgentClient.locZipCode;
                    _currTBL.inv_doc_vat_num = currAgentClient.docVat;
                    _currTBL.inv_doc_cf_num = currAgentClient.docCf;

                    txt_inv_name_full.Text = _currTBL.inv_name_full;
                    drp_inv_loc_country.setSelectedValue(_currTBL.inv_loc_country);
                    txt_inv_loc_state.Text = _currTBL.inv_loc_state;
                    txt_inv_loc_city.Text = _currTBL.inv_loc_city;
                    txt_inv_loc_address.Text = _currTBL.inv_loc_address;
                    txt_inv_loc_zip_code.Text = _currTBL.inv_loc_zip_code;
                    txt_inv_doc_vat_num.Text = _currTBL.inv_doc_vat_num;
                    txt_inv_doc_cf_num.Text = _currTBL.inv_doc_cf_num;
                }
                DC_RENTAL.SubmitChanges();
            }
            rntUtils.rntReservation_onChange(_currTBL);
            rntUtils.reservation_checkPartPayment(_currTBL, false);
            List<INV_TBL_PAYMENT> _listPay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete == 1 && x.direction == 1).ToList();
            foreach (INV_TBL_PAYMENT _pay in _listPay)
            {
                invUtils.payment_checkInvoice(_pay, _currTBL);
            }
            authProps.ClientTBL = null;
            if (_currTBL.state_pid == 4)
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "confirmSendMailWithVoucher();", true);
            else
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "callAlert(\"Data was successfully saved. \");", true);


            //lbl_ok.Visible = true;
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
                var cal_doc_issue_date_#Unique#;
			    function setCal_#Unique#() {
                    cal_birth_date_#Unique# = new JSCal.Single({ dtFormat: ""d MM yy"", Cont: ""#HF_birth_date.ClientID"", View: ""#txt_birth_date"", changeMonth: true, changeYear: true, yearRange: '1920:#currYear#' });
                    cal_doc_expiry_date_#Unique# = new JSCal.Single({ dtFormat: ""d MM yy"", Cont: ""#HF_doc_expiry_date.ClientID"", View: ""#txt_doc_expiry_date"", changeMonth: true, changeYear: true, yearRange: '#currYear#:2030' });
                    cal_doc_issue_date_#Unique# = new JSCal.Single({ dtFormat: ""d MM yy"", Cont: ""#HF_doc_issue_date.ClientID"", View: ""#txt_doc_issue_date"", changeMonth: true, changeYear: true, yearRange: '1950:#currYear#' });
			    }
            ";
            setCal_ = setCal_.Replace("#Unique#", Unique);
            setCal_ = setCal_.Replace("#currYear#", "" + DateTime.Now.Year);
            setCal_ = setCal_.Replace("HF_birth_date.ClientID", HF_birth_date.ClientID);
            setCal_ = setCal_.Replace("HF_doc_expiry_date.ClientID", HF_doc_expiry_date.ClientID);
            setCal_ = setCal_.Replace("HF_doc_issue_date.ClientID", HF_doc_issue_date.ClientID);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_setCal_" + Unique, setCal_, true);
        }
        
        protected void Bind_drp_doc_type()
        {
            //List<USR_TB_DOC_TYPE> _list = DC_USER.USR_TB_DOC_TYPEs.ToList();
            drp_doc_type.DataSource = authProps.DocTypeLK.OrderBy(x => x.title); 
            drp_doc_type.DataTextField = "title";
            drp_doc_type.DataValueField = "code";
            drp_doc_type.DataBind();
            drp_doc_type.Items.Insert(0, new ListItem("", ""));
        }

        protected void lnk_save_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void drp_inv_isSameAsPersData_SelectedIndexChanged(object sender, EventArgs e)
        {
            invDetailsCheck();
        }
        
        protected void invDetailsCheck()
        {
            PH_invDetails.Visible = drp_inv_isDifferent.SelectedValue == "1";
        }
    }
}