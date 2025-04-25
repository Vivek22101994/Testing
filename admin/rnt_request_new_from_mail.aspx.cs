using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_request_new_from_mail : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_request";
        }
        private magaRental_DataContext DC_RENTAL;
        protected magaMail_DataContext DC_MAIL;
        private MAIL_TBL_MESSAGE _currTBL;
        protected string listPage = "rnt_request_list.aspx";
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public int IdMessage
        {
            get
            {
                int _id;
                if (int.TryParse(HF_id.Value, out _id))
                    return _id;
                return 0;
            }
            set
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                HF_id.Value = value.ToString();
                FillControls();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_MAIL = maga_DataContext.DC_MAIL;
            if (!IsPostBack)
            {
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    _currTBL = DC_MAIL.MAIL_TBL_MESSAGE.SingleOrDefault(x => x.id == _id);
                    if (_currTBL == null)
                        Response.Redirect(listPage);
                    IdMessage = _id;
                    drp_pidCity_DataBind();
                    FillControls();
                }
                else
                    Response.Redirect(listPage);
            }
            RegisterScripts();
        }
        protected void drp_pidCity_DataBind()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_pidCity.Items.Clear();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_pidCity.Items.Add(new ListItem("" + t.title, "" + t.id));
            }
            drp_pidCity.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
        protected void Bind_drp_admin()
        {
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => (x.rnt_canHaveRequest == 1) && x.is_deleted == 0 && x.is_active == 1).ToList();
            drp_admin.Items.Clear();
            foreach (USR_ADMIN _utenti in _list)
            {
                drp_admin.Items.Add(new ListItem("" + _utenti.name + " " + _utenti.surname, "" + _utenti.id));
            }
            drp_admin.Items.Insert(0, new ListItem("-! non assegnato !-", "0"));
            if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedRequests.objToInt32() == 1)
                drp_admin.setSelectedValue(UserAuthentication.CurrentUserID.ToString());
        }
        public void FillControls()
        {
            DC_MAIL = maga_DataContext.DC_MAIL;
            if (IdMessage != 0)
                _currTBL = DC_MAIL.MAIL_TBL_MESSAGE.SingleOrDefault(x => x.id == IdMessage);
            if (_currTBL == null)
                return;
            ltr_body_html_text.Text = _currTBL.body_html_text;
            ltr_body_plain_text.Text = _currTBL.body_plain_text;
            txt_description.Text = (ltr_body_html_text.Text != "") ? ltr_body_html_text.Text : ltr_body_plain_text.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            
            txt_email.Text = _currTBL.from_email;
            HF_pid_request.Value = _currTBL.pid_request.ToString();
            HF_date_start.Value = DateTime.Now.AddDays(7).JSCal_dateToString();
            HF_date_end.Value = DateTime.Now.AddDays(10).JSCal_dateToString();

            Bind_drp_admin();

        }

        protected void FillDataFromControls()
        {
            DC_MAIL = maga_DataContext.DC_MAIL;
            if (IdMessage != 0)
                _currTBL = DC_MAIL.MAIL_TBL_MESSAGE.SingleOrDefault(x => x.id == IdMessage);
            if (_currTBL == null)
                return;
            string _br = "";
            bool alternateOld = true;
            RNT_TBL_REQUEST _request = new RNT_TBL_REQUEST();
            _request.pid_lang = drp_lang.getSelectedValueInt();
            string _mailBody = "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
            string _row = "";
            _row += "<tr>";
            _row += "<td><strong>Lingua</strong></td>";
            _row += "<td>" + CurrentLang.TITLE + "</td>";
            _row += "</tr>";
            _mailBody += _row;
            _request.name_first = "";
            _request.name_last = "";
            _request.name_full = txt_name_full.Text;
            _mailBody += MailingUtilities.addMailRow("Nome/Cognome", _request.name_full, alternateOld, out alternateOld, false, false, false);
            _request.phone = txt_phone.Text;
            _mailBody += MailingUtilities.addMailRow("Telefono", "" + _request.phone, alternateOld, out alternateOld, false, false, false);
            _request.email = txt_email.Text;
            _mailBody += MailingUtilities.addMailRow("Email", "" + _request.email, alternateOld, out alternateOld, false, false, false);


            // start area
            string _area = "";
            _request.request_area = _area;
            _mailBody += MailingUtilities.addMailRow("e/o zona", "" + _request.request_area, alternateOld, out alternateOld, false, false, false);
            // end area
            _request.request_country = drp_country.SelectedItem.Text;
            _mailBody += MailingUtilities.addMailRow("Paese (Location)", "" + _request.request_country, alternateOld, out alternateOld, false, false, false);
            _request.request_date_start = HF_date_start.Value.JSCal_stringToDate();
            _mailBody += MailingUtilities.addMailRow("Check-In", "" + _request.request_date_start.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _request.request_date_end = HF_date_end.Value.JSCal_stringToDate();
            _mailBody += MailingUtilities.addMailRow("Check-Out", "" + _request.request_date_end.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _request.request_date_is_flexible = drp_date_is_flexible.getSelectedValueInt(0);
            if (_request.request_date_is_flexible == 1)
                _mailBody += MailingUtilities.addMailRow("Date Flessibili", "SI", alternateOld, out alternateOld, false, false, false);
            else
                _mailBody += MailingUtilities.addMailRow("Date Flessibil", "NO", alternateOld, out alternateOld, false, false, false);
            _request.request_adult_num = drp_adult_num.getSelectedValueInt(0);
            _mailBody += MailingUtilities.addMailRow("num. Adulti", "" + _request.request_adult_num, alternateOld, out alternateOld, false, false, false);
            _request.request_child_num = drp_child_num.getSelectedValueInt(0);
            _mailBody += MailingUtilities.addMailRow("num. Bambini", "" + _request.request_child_num, alternateOld, out alternateOld, false, false, false);
            _request.request_transport = "";
            _mailBody += MailingUtilities.addMailRow("Trasporto", "" + _request.request_transport, alternateOld, out alternateOld, false, false, false);
            string _price_range = "";
            _request.request_price_range = _price_range;
            _mailBody += MailingUtilities.addMailRow("e/o con prezzo", "" + _request.request_price_range, alternateOld, out alternateOld, false, false, false);
            string _services = "";
            _br = "";
            _request.request_services = _services;
            _mailBody += MailingUtilities.addMailRow("Servizi", "" + _request.request_services, alternateOld, out alternateOld, false, false, false);
            _request.request_notes = txt_description.Text;
            _mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + _request.request_notes, alternateOld, out alternateOld, false, false, false);
            _request.request_date_created = DateTime.Now;
            _mailBody += MailingUtilities.addMailRow("Data ora creazione", "" + _request.request_date_created, alternateOld, out alternateOld, false, false, false);
            _request.state_date = DateTime.Now;
            _request.state_pid = 1;
            _request.state_subject = "Creata Richiesta";
            _request.state_pid_user = 1;
            _request.request_ip = Request.ServerVariables.Get("REMOTE_HOST");
            _request.pid_creator = UserAuthentication.CurrentUserID;
            _request.pid_city = drp_pidCity.getSelectedValueInt();


            DC_RENTAL.RNT_TBL_REQUEST.InsertOnSubmit(_request);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntRequest_addState(_request.id, _request.state_pid.Value, _request.state_pid_user.Value, _request.state_subject, "");

            //_mailBody += "<br/><br/><br/>Per magiorni informazioni entrate in <a href='http://62.149.238.119/admin/cont_request_details.aspx?id=" + _request.id + "' >area amministrativa del sito</a>";
            _mailBody += "</table>";
            _request.mail_body = _mailBody;
            string _mSubject = "";
            int pid_operator = 0;
            RNT_TBL_REQUEST _relatedRequest = rntUtils.rntRequest_getRelatedRequest(_request);
            if (_relatedRequest != null)
            {
                _request.IdAdMedia = _relatedRequest.IdAdMedia;
                _request.IdLink = _relatedRequest.IdLink;
                _request.pid_related_request = _relatedRequest.id;
                pid_operator = _relatedRequest.pid_operator.Value;
                _mSubject = "rif." + _request.id + " Correlata a rif." + _relatedRequest.id + " - " + CurrentSource.getSysLangValue("reqHeadTitle") + " " + _request.name_first + " " + _request.name_last + " - " + _request.request_country + " - " + _request.request_choice_1 + " - " + _request.request_choice_2 + " - Check-In date: " + _request.request_date_start.Value.formatITA(true);
                rntUtils.rntRequest_addState(_request.id, 0, UserAuthentication.CurrentUserID, "Correlazione alla richiesta Primaria rif. " + _relatedRequest.id, "");
                rntUtils.rntRequest_addState(_relatedRequest.id, 0, UserAuthentication.CurrentUserID, "Aggiunta Correlazione alla richiesta Secondaria rif. " + _request.id, "");
            }
            else
            {
                _request.pid_related_request = 0;
                pid_operator = AdminUtilities.usr_getAvailableOperator(drp_country.getSelectedValueInt(0).Value, CurrentLang.ID);
                _mSubject = "rif." + _request.id + " - " + CurrentSource.getSysLangValue("reqHeadTitle") + " " + _request.name_first + " " + _request.name_last + " - " + _request.request_country + " - " + _request.request_choice_1 + " - " + _request.request_choice_2 + " - Check-In date: " + _request.request_date_start.Value.formatITA(true);
            }
            pid_operator = drp_admin.getSelectedValueInt(0).objToInt32(); //AdminUtilities.usr_getAvailableOperator(drp_country.getSelectedValueInt(0).Value, CurrentLang.ID);
            _mSubject = "rif." + _request.id + " - " + CurrentSource.getSysLangValue("reqHeadTitle") + " " + _request.name_first + " " + _request.name_last + " - " + _request.request_country + " - " + _request.request_choice_1 + " - " + _request.request_choice_2 + " - Check-In date: " + _request.request_date_start.Value.formatITA(true);
            if (pid_operator == 0)
            {
                _mailBody = "Attenzione! Non è stato Assegnato a nessun account.<br/><br/>" + _mailBody;
                //MailingUtilities.autoSendMailTo(_mSubject, _mailBody, "info@rentalinrome.com", false);
            }
            else
            {
                _request.operator_date = DateTime.Now;
                string _mailSend = "";
                if (UserAuthentication.CurrentUserID != pid_operator)
                {
                    if (MailingUtilities.autoSendMailTo(_mSubject, _mailBody, AdminUtilities.usr_adminEmail(pid_operator, ""), false, "admin_rnt_request_new_from_mail al account"))
                        _mailSend = "Assegnato e inviato mail a " + AdminUtilities.usr_adminName(pid_operator, "") + " (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                    else
                        _mailSend = "Assegnato a " + AdminUtilities.usr_adminName(pid_operator, "") + " - Errore nel invio mail a (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                }
                else
                    _mailSend = "Assegnato a " + AdminUtilities.usr_adminName(pid_operator, "");
                rntUtils.rntRequest_addState(_request.id, 0, _request.state_pid_user.Value, _mailSend, _mailBody);
                _mailBody = _mailSend + "<br/><br/>" + _mailBody;
            }
            _request.pid_operator = pid_operator;
            DC_RENTAL.SubmitChanges();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Show_inf_hotel_request_send", "setTimeout('parent.$(\".formreportapp\").colorbox.close();',3000);", true);
            MailingUtilities.autoSendMailTo(_mSubject, _mailBody, MailingUtilities.ADMIN_MAIL, false, "admin_rnt_request_new_from_mail al admin");

            _currTBL.pid_request = _request.id;
            _currTBL.pid_request_state = 0;
            DC_MAIL.SubmitChanges();

            Response.Redirect("rnt_request_details.aspx?id=" + _request.id);
        }
        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem("- seleziona -", ""));
        }
        protected void drp_lang_DataBound(object sender, EventArgs e)
        {
            drp_lang.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cal_date_start_", "cal_date_start_ = new JSCal.Single({ dtFormat: \"d MM yy\", Cont: \"#" + HF_date_start.ClientID + "\", View: \"#cal_date_start\", changeMonth: true, changeYear: true });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cal_date_end_", "cal_date_end_ = new JSCal.Single({ dtFormat: \"d MM yy\", Cont: \"#" + HF_date_end.ClientID + "\", View: \"#cal_date_end\", changeMonth: true, changeYear: true });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tinyEditor", "setTinyEditor(true);", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setAutocomplete", "setAutocomplete();", true);
        }
        protected void lnk_create_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
    }
}
