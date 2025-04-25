using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class stp_contacts : mainBasePage
    {
        magaRental_DataContext DC_RENTAL;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "stp";
            int id = Request.QueryString["id"].ToInt32();
            if (id != 0)
                base.PAGE_REF_ID = id;
            else
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "stp_contacts", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                checkReservationsCal();
                Fill_data();
                Bind_drp_honorific();
                Bind_chkList_area();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal_" + Unique + "(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "_RNT_validateBook", "$(function() {$(\"#" + txt_email.ClientID + ",#" + txt_email_conf.ClientID + "\").bind(\"cut copy paste\", function(event) { event.preventDefault();}); });", true);
        }
        protected void Fill_data()
        {
            clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
            DateTime _dtStart = _config.lastSearch.dtStart;
            DateTime _dtEnd = _config.lastSearch.dtEnd;
            HF_dtStart.Value = _dtStart.JSCal_dateToString();
            HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
            int IdEstate = Request.QueryString["IdEstate"].objToInt32();
            if (IdEstate != 0)
                _config.addTo_myPreferedEstateList(IdEstate);
            clUtils.saveConfig(_config);
            // page details
            CONT_VIEW_STP _stp =
                maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == PAGE_REF_ID && item.pid_lang == CurrentLang.ID);
            if (_stp != null)
            {
                ltr_meta_description.Text = _stp.meta_description;
                ltr_meta_keywords.Text = _stp.meta_keywords;
                ltr_meta_title.Text = _stp.meta_title;
                ltr_title.Text = _stp.title;
                ltr_sub_title.Text = _stp.sub_title;
                ltr_description.Text = _stp.description;
            }
            // privacy and terms
            string url_voucher;
            string filename;
            _stp = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == 19 && item.pid_lang == CurrentLang.ID);
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

                       
            // drp_persons
            int _max = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.num_persons_max.HasValue).Max(x => x.num_persons_max.Value);
            HF_num_persons_max.Value = _max.ToString();
            drp_adult.Items.Clear();
            for (int i = 1; i <= _max; i++)
            {
                drp_adult.Items.Add(new ListItem("" + i, "" + i));
            }
            drp_adult.setSelectedValue(_config.lastSearch.numPers_adult.ToString());
            drp_child_over.Items.Clear();
            drp_child_over.Items.Add(new ListItem("---", "0"));
            for (int i = 1; i <= (_max - _config.lastSearch.numPers_adult); i++)
            {
                drp_child_over.Items.Add(new ListItem("" + i, "" + i));
            }
            drp_child_min.setSelectedValue(_config.lastSearch.numPers_childOver.ToString());
            drp_child_min.Items.Clear();
            drp_child_min.Items.Add(new ListItem("---", "0"));
            for (int i = 1; i <= 10; i++)
            {
                drp_child_min.Items.Add(new ListItem("" + i, "" + i));
            }
            drp_child_min.setSelectedValue(_config.lastSearch.numPers_childMin.ToString());


            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqPriceRange1")))
                chkList_price_range.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqPriceRange1"), CurrentSource.getSysLangValue("reqPriceRange1")));
            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqPriceRange2")))
                chkList_price_range.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqPriceRange2"), CurrentSource.getSysLangValue("reqPriceRange2")));
            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqPriceRange3")))
                chkList_price_range.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqPriceRange3"), CurrentSource.getSysLangValue("reqPriceRange3")));
            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqPriceRange4")))
                chkList_price_range.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqPriceRange4"), CurrentSource.getSysLangValue("reqPriceRange4")));

            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqTransport1")))
                drp_transport.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqTransport1"), CurrentSource.getSysLangValue("reqTransport1")));
            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqTransport2")))
                drp_transport.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqTransport2"), CurrentSource.getSysLangValue("reqTransport2")));
            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqTransport3")))
                drp_transport.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqTransport3"), CurrentSource.getSysLangValue("reqTransport3")));
            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqTransport4")))
                drp_transport.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqTransport4"), CurrentSource.getSysLangValue("reqTransport4")));

            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqServices1")))
                chkList_services.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqServices1"), CurrentSource.getSysLangValue("reqServices1")));
            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqServices2")))
                chkList_services.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqServices2"), CurrentSource.getSysLangValue("reqServices2")));
            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqServices3")))
                chkList_services.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqServices3"), CurrentSource.getSysLangValue("reqServices3")));
            if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqServices4")))
                chkList_services.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqServices4"), CurrentSource.getSysLangValue("reqServices4")));

        }
        protected void checkReservationsCal()
        {
            string _script = "";
            _script += "function checkCalDates_" + Unique + "(date){var _dtStart = parseInt($('#" + HF_dtStart.ClientID + "').val()); var _dtEnd = parseInt($('#" + HF_dtEnd.ClientID + "').val()); var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
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
        protected void Bind_chkList_area()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.pid_lang == CurrentLang.ID && AppSettings.RNT_activeZones.Contains(x.id)).OrderBy(x => x.title).ToList();
            chkList_area.DataSource = _list;
            chkList_area.DataValueField = "id";
            chkList_area.DataTextField = "title";
            chkList_area.DataBind();
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
        protected void lnk_send_Click(object sender, EventArgs e)
        {
            saveRequest();
            if (!Global.isOnDev)
            {
                pnl_adRollScript.Visible = true;
            }
      
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "scrollTo_sent", "$.scrollTo($(\"#" + pnl_request_sent.ClientID + "\"), 500);", true);
        }

        protected void saveRequest()
        {
            clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
            _config.lastSearch.dtStart = HF_dtStart.Value.JSCal_stringToDate();
            _config.lastSearch.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            _config.lastSearch.numPers_adult = drp_adult.getSelectedValueInt(0).objToInt32();
            _config.lastSearch.numPers_childOver = drp_child_over.getSelectedValueInt(0).objToInt32();
            _config.lastSearch.numPers_childMin = drp_child_min.getSelectedValueInt(0).objToInt32();
            clUtils.saveConfig(_config);

            string _br = "";
            bool alternateOld = true;
            RNT_TBL_REQUEST _request = new RNT_TBL_REQUEST();

            //for (int i = 0; i < 99999999999999; i++)
            //{
            //        string p = "";
            //}
            //return;
            _request.pid_lang = CurrentLang.ID;
            string _mailBody = "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
            string _row = "";
            _row += "<tr>";
            _row += "<td><strong>Lingua</strong></td>";
            _row += "<td>" + CurrentLang.TITLE + "</td>";
            _row += "</tr>";
            _mailBody += _row;
            _request.name_honorific = drp_honorific.SelectedItem.Text;
            _request.name_full = txt_name_full.Text;
            _mailBody += MailingUtilities.addMailRow("Nome/Cognome", _request.name_full, alternateOld, out alternateOld, false, false, false);
            _request.phone = txt_phone_mobile.Text;
            _mailBody += MailingUtilities.addMailRow("Telefono", "" + _request.phone, alternateOld, out alternateOld, false, false, false);
            _request.email = txt_email.Text;
            _mailBody += MailingUtilities.addMailRow("Email", "" + _request.email, alternateOld, out alternateOld, false, false, false);
            // start preferenze
            string _choices = "";
            _br = "";
            List<RNT_RL_REQUEST_ITEM> _items = new List<RNT_RL_REQUEST_ITEM>();
            foreach (int id in _config.myPreferedEstateList)
            {
                RNT_RL_REQUEST_ITEM _rItem = new RNT_RL_REQUEST_ITEM();
                _rItem.sequence = _config.myPreferedEstateList.IndexOf(id) + 1;
                _rItem.pid_estate = id;
                _rItem.pid_zone = 0;
                _rItem.notes = CurrentSource.rntEstate_title(id, CurrentLang.ID, "");
                _items.Add(_rItem);
                _choices += _br + _rItem.sequence + " - " + _rItem.notes;
                _br = "<br/>";
            }
            _request.request_choices = _choices;
            _mailBody += MailingUtilities.addMailRow("Preferenze", "" + _choices, alternateOld, out alternateOld, false, false, false);
            _config.myPreferedEstateList = new List<int>();
            clUtils.saveConfig(_config);
            // end prefenze

            // start area
            string _area = "";
            _br = "";
            foreach (ListItem _item in chkList_area.Items)
            {
                if (_item.Selected)
                {
                    _area += _br + _item.Text;
                    _br = "<br/>";
                }
            }
            _request.request_area = _area;
            _mailBody += MailingUtilities.addMailRow("e/o zona", "" + _request.request_area, alternateOld, out alternateOld, false, false, false);
            // end area
            _request.request_country = drp_country.SelectedItem.Text;
            _mailBody += MailingUtilities.addMailRow("Paese (Location)", "" + _request.request_country, alternateOld, out alternateOld, false, false, false);
            _request.request_date_start = _config.lastSearch.dtStart;
            _mailBody += MailingUtilities.addMailRow("Check-In", "" + _request.request_date_start.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _request.request_date_end = _config.lastSearch.dtEnd;
            _mailBody += MailingUtilities.addMailRow("Check-Out", "" + _request.request_date_end.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _request.request_date_is_flexible = chk_date_is_flexible.Checked ? 1 : 0;
            if (chk_date_is_flexible.Checked)
                _mailBody += MailingUtilities.addMailRow("Date Flessibili", "SI", alternateOld, out alternateOld, false, false, false);
            else
                _mailBody += MailingUtilities.addMailRow("Date Flessibil", "NO", alternateOld, out alternateOld, false, false, false);
            _request.request_adult_num = _config.lastSearch.numPers_adult;
            _mailBody += MailingUtilities.addMailRow("num. Adulti", "" + _request.request_adult_num, alternateOld, out alternateOld, false, false, false);
            _request.request_child_num = _config.lastSearch.numPers_childOver;
            _mailBody += MailingUtilities.addMailRow("num. Bambini", "" + _request.request_child_num, alternateOld, out alternateOld, false, false, false);
            _request.request_child_num_min = _config.lastSearch.numPers_childMin;
            _mailBody += MailingUtilities.addMailRow("num. Neonati", "" + _request.request_child_num_min, alternateOld, out alternateOld, false, false, false);
            _request.request_transport = drp_transport.SelectedValue;
            _mailBody += MailingUtilities.addMailRow("Trasporto", "" + _request.request_transport, alternateOld, out alternateOld, false, false, false);
            string _price_range = "";
            _br = "";
            foreach (ListItem _item in chkList_price_range.Items)
            {
                if (_item.Selected)
                {
                    _price_range += _br + _item.Value;
                    _br = "<br/>";
                }
            }
            _request.request_price_range = _price_range;
            _mailBody += MailingUtilities.addMailRow("e/o con prezzo", "" + _request.request_price_range, alternateOld, out alternateOld, false, false, false);
            string _services = "";
            _br = "";
            foreach (ListItem _item in chkList_services.Items)
            {
                if (_item.Selected)
                {
                    _services += _br + _item.Value;
                    _br = "<br/>";
                }
            }
            _request.request_services = _services;
            _mailBody += MailingUtilities.addMailRow("Servizi", "" + _request.request_services, alternateOld, out alternateOld, false, false, false);
            _request.request_notes = txt_note.Value.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            _mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + _request.request_notes, alternateOld, out alternateOld, false, false, false);
            _request.request_date_created = DateTime.Now;
            _mailBody += MailingUtilities.addMailRow("Data ora creazione", "" + _request.request_date_created, alternateOld, out alternateOld, false, false, false);
            _request.state_date = DateTime.Now;
            _request.state_pid = 1;
            _request.state_subject = "Creata Richiesta";
            _request.state_pid_user = 1;
            _request.request_ip = Request.ServerVariables.Get("REMOTE_HOST");
            _request.pid_creator = 1;
            _request.pid_city = AppSettings.RNT_currCity;
            _request.IdAdMedia = App.IdAdMedia;
            _request.IdLink = App.IdLink;


            DC_RENTAL.RNT_TBL_REQUEST.InsertOnSubmit(_request);
            DC_RENTAL.SubmitChanges();
            foreach (RNT_RL_REQUEST_ITEM _item in _items)
            {
                _item.pid_request = _request.id;
                DC_RENTAL.RNT_RL_REQUEST_ITEMs.InsertOnSubmit(_item);
            }
            DC_RENTAL.SubmitChanges();
            rntUtils.rntRequest_addState(_request.id, _request.state_pid.Value, _request.state_pid_user.Value, _request.state_subject, "");
            pnl_request_cont.Visible = false;
            pnl_request_sent.Visible = true;

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
            if (pid_operator == 0)
            {
                _mailBody = "Attenzione! Non è stato Assegnato a nessun account.<br/><br/>" + _mailBody;
            }
            else
            {
                _request.operator_date = DateTime.Now;
                string _mailSend = "";
                if (MailingUtilities.autoSendMailTo(_mSubject, _mailBody, AdminUtilities.usr_adminEmail(pid_operator, ""), false, "stp_contacts al account"))
                    _mailSend = "Assegnato e inviato mail a " + AdminUtilities.usr_adminName(pid_operator, "") + " (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                else
                    _mailSend = "Assegnato " + AdminUtilities.usr_adminName(pid_operator, "") + " - Errore nel invio mail a (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                rntUtils.rntRequest_addState(_request.id, 0, _request.state_pid_user.Value, _mailSend, _mailBody);
                _mailBody = _mailSend + "<br/><br/>" + _mailBody;
            }
            _request.pid_operator = pid_operator;
            DC_RENTAL.SubmitChanges();
          
            HF_IdRequest.Value = _request.id + "";
            rntUtils.rntRequest_mailNewCreation(_request);
          
            MailingUtilities.autoSendMailTo(_mSubject, _mailBody, MailingUtilities.ADMIN_MAIL, false, "stp_contacts al admin");
        }
    }
}
