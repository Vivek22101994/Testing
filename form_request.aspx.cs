using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class form_request : mainBasePage
    {
        magaRental_DataContext DC_RENTAL;
        protected class selApt
        {
            private int _id = 0;
            private string _apt = "";
            private int _pid_estate = 0;
            public selApt(int _ID, int _IdEstate, string _Apt)
            {
                this._id = _ID;
                this._apt = _Apt;
                this._pid_estate = _IdEstate;
            }
            public int ID { get { if (this._id != null)return this._id; else return 0; } set { this._id = value; } }
            public int IdEstate { get { if (this._pid_estate != null)return this._pid_estate; else return 0; } set { this._pid_estate = value; } }
            public string Apt { get { if (this._apt != null)return this._apt; else return ""; } set { this._apt = value; } }
        }
        protected List<selApt> selectedApts
        {
            get { if ((List<selApt>)Session["selectedApts"] != null) { return (List<selApt>)Session["selectedApts"]; } return new List<selApt>(); }
            set { Session["selectedApts"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                List<selApt> _list = new List<selApt>();
                _list.Add(new selApt(_list.Count + 1, 0, ""));
                _list.Add(new selApt(_list.Count + 1, 0, ""));
                selectedApts = _list;
                LV_apts.DataSource = selectedApts;
                LV_apts.DataBind();

                string _items = "";
                string _sep = "";
                List<RNT_VIEW_ESTATE> _estateList = DC_RENTAL.RNT_VIEW_ESTATEs.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.pid_lang == 2).OrderBy(x => x.title).ToList();
                foreach (RNT_VIEW_ESTATE _estate in _estateList)
                {
                    _items += _sep + "{idEstate: \"" + _estate.id + "\", idZone: \"0\",label: \"" + _estate.title + "\",desc: \"\"}";
                    _sep = ",";
                }
                ltr_items.Text = _items;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setAutocomplete", "setAutocomplete();", true);

                HF_dtStart.Value = DateTime.Now.AddDays(7).JSCal_dateToString();
                HF_dtEnd.Value = DateTime.Now.AddDays(10).JSCal_dateToString();
                lnkSend.Text = CurrentSource.getSysLangValue("reqSubmit") != "" ? "<span>" + CurrentSource.getSysLangValue("reqSubmit") + "</span>" : "<span>Submit</span>";
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

                if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqAreaOpt1")))
                    chkList_area.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqAreaOpt1"), CurrentSource.getSysLangValue("reqAreaOpt1")));
                if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqAreaOpt2")))
                    chkList_area.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqAreaOpt2"), CurrentSource.getSysLangValue("reqAreaOpt2")));
                if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqAreaOpt3")))
                    chkList_area.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqAreaOpt3"), CurrentSource.getSysLangValue("reqAreaOpt3")));
                if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqAreaOpt4")))
                    chkList_area.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqAreaOpt4"), CurrentSource.getSysLangValue("reqAreaOpt4")));
                if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqAreaOpt5")))
                    chkList_area.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqAreaOpt5"), CurrentSource.getSysLangValue("reqAreaOpt5")));
                if (!string.IsNullOrEmpty(CurrentSource.getSysLangValue("reqAreaOpt6")))
                    chkList_area.Items.Add(new ListItem(CurrentSource.getSysLangValue("reqAreaOpt6"), CurrentSource.getSysLangValue("reqAreaOpt6")));
            }
        }
        protected void lnk_send_Click(object sender, EventArgs e)
        {

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
            _request.name_first = txt_name_first.Value;
            _request.name_last = txt_name_last.Value;
            _request.name_full = _request.name_first + " " + _request.name_last;
            _mailBody += MailingUtilities.addMailRow("Nome/Cognome", _request.name_full, alternateOld, out alternateOld, false, false, false);
            _request.phone = txt_phone.Value;
            _mailBody += MailingUtilities.addMailRow("Telefono", "" + _request.phone, alternateOld, out alternateOld, false, false, false);
            _request.email = txt_email.Value;
            _mailBody += MailingUtilities.addMailRow("Email", "" + _request.email, alternateOld, out alternateOld, false, false, false);
            // start preferenze
            string _choices = "";
            _br = "";
            List<RNT_RL_REQUEST_ITEM> _items = new List<RNT_RL_REQUEST_ITEM>();
            foreach (ListViewDataItem _item in LV_apts.Items)
            {
                TextBox txt = _item.FindControl("txt_choice") as TextBox;
                Label lbl = _item.FindControl("lbl_id") as Label;
                Label lbl_idEstate = _item.FindControl("lbl_idEstate") as Label;
                if (txt != null && lbl != null && lbl_idEstate != null && txt.Text.Trim() != "")
                {
                    RNT_RL_REQUEST_ITEM _rItem = new RNT_RL_REQUEST_ITEM();
                    _rItem.sequence = lbl.Text.ToInt32();
                    _rItem.pid_estate = lbl_idEstate.Text.ToInt32();
                    _rItem.pid_zone = 0;
                    _rItem.notes = txt.Text;
                    _items.Add(_rItem);
                    _choices += _br + _rItem.sequence + " - " + _rItem.notes;
                    _br = "<br/>";
                }
            }
            _request.request_choices = _choices;
            _mailBody += MailingUtilities.addMailRow("Preferenze", "" + _choices, alternateOld, out alternateOld, false, false, false);
            // end prefenze

            // start area
            string _area = "";
            _br = "";
            foreach (ListItem _item in chkList_area.Items)
            {
                if (_item.Selected)
                {
                    _area += _br + _item.Value;
                    _br = "<br/>";
                }
            }
            _request.request_area = _area;
            _mailBody += MailingUtilities.addMailRow("e/o zona", "" + _request.request_area, alternateOld, out alternateOld, false, false, false);
            // end area
            _request.request_country = drp_country.SelectedItem.Text;
            _mailBody += MailingUtilities.addMailRow("Paese (Location)", "" + _request.request_country, alternateOld, out alternateOld, false, false, false);
            _request.request_date_start = HF_dtStart.Value.JSCal_stringToDate();
            _mailBody += MailingUtilities.addMailRow("Check-In", "" + _request.request_date_start.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _request.request_date_end = HF_dtEnd.Value.JSCal_stringToDate();
            _mailBody += MailingUtilities.addMailRow("Check-Out", "" + _request.request_date_end.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _request.request_date_is_flexible = chk_date_is_flexible.Checked ? 1 : 0;
            if (chk_date_is_flexible.Checked)
                _mailBody += MailingUtilities.addMailRow("Date Flessibili", "SI", alternateOld, out alternateOld, false, false, false);
            else
                _mailBody += MailingUtilities.addMailRow("Date Flessibil", "NO", alternateOld, out alternateOld, false, false, false);
            _request.request_adult_num = drp_adult_num.Value.ToInt32();
            _mailBody += MailingUtilities.addMailRow("num. Adulti", "" + _request.request_adult_num, alternateOld, out alternateOld, false, false, false);
            _request.request_child_num = drp_child_num.Value.ToInt32();
            _mailBody += MailingUtilities.addMailRow("num. Bambini", "" + _request.request_child_num, alternateOld, out alternateOld, false, false, false);
            _request.request_transport = drp_transport.Value;
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
            _request.request_notes = txt_note.Value.htmlNoWrap();
            _mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + _request.request_notes, alternateOld, out alternateOld, false, false, false);
            _request.request_date_created = DateTime.Now;
            _mailBody += MailingUtilities.addMailRow("Data ora creazione", "" + _request.request_date_created, alternateOld, out alternateOld, false, false, false);
            _request.state_date = DateTime.Now;
            _request.state_pid = 1;
            _request.state_subject = "Creata Richiesta";
            _request.state_pid_user = 1;
            _request.request_ip = Request.ServerVariables.Get("REMOTE_HOST");
            _request.pid_creator = 1;
            _request.pid_city = 1;


            DC_RENTAL.RNT_TBL_REQUEST.InsertOnSubmit(_request);
            DC_RENTAL.SubmitChanges();
            foreach (RNT_RL_REQUEST_ITEM _item in _items)
            {
                _item.pid_request = _request.id;
                DC_RENTAL.RNT_RL_REQUEST_ITEMs.InsertOnSubmit(_item);
            }
            DC_RENTAL.SubmitChanges();
            rntUtils.rntRequest_addState(_request.id, _request.state_pid.Value, _request.state_pid_user.Value, _request.state_subject, "");
            pnl_request.Visible = false;
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
                rntUtils.rntRequest_addState(_request.id, 0, UserAuthentication.CurrentUserID, "Correlazione alla richiesta Primaria rif. " + _relatedRequest.id,"");
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
                if (!Request.Url.AbsoluteUri.Contains("http://localhost"))
                {
                    if (MailingUtilities.autoSendMailTo(_mSubject, _mailBody, AdminUtilities.usr_adminEmail(pid_operator, ""), false, "form_request al account"))
                        _mailSend = "Assegnato e inviato mail a " + AdminUtilities.usr_adminName(pid_operator, "") + " (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                    else
                        _mailSend = "Assegnato " + AdminUtilities.usr_adminName(pid_operator, "") + " - Errore nel invio mail a (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                }
                else
                    _mailSend = "Assegnato e inviato mail a " + AdminUtilities.usr_adminName(pid_operator, "") + " (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                rntUtils.rntRequest_addState(_request.id, 0, _request.state_pid_user.Value, _mailSend, _mailBody);
                _mailBody = _mailSend + "<br/><br/>" + _mailBody;
            }
            _request.pid_operator = pid_operator;
            DC_RENTAL.SubmitChanges();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Show_inf_hotel_request_send", "setTimeout('parent.$(\".formreportapp\").colorbox.close();',3000);", true);
            //AdminUtilities.rntRequest_mailNewCreation(_request);
            if (!Request.Url.AbsoluteUri.Contains("http://localhost"))
                MailingUtilities.autoSendMailTo(_mSubject, _mailBody, MailingUtilities.ADMIN_MAIL, false, "form_request al admin");
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

        protected void lnk_addItem_Click(object sender, EventArgs e)
        {
            List<selApt> _list = new List<selApt>();
            foreach (ListViewDataItem _item in LV_apts.Items)
            {
                TextBox txt = _item.FindControl("txt_choice") as TextBox;
                Label lbl = _item.FindControl("lbl_id") as Label;
                if (txt != null && lbl != null)
                {
                    _list.Add(new selApt(_list.Count + 1, 0, txt.Text));
                }
            }
            _list.Add(new selApt(_list.Count + 1, 0, ""));
            selectedApts = _list;
            LV_apts.DataSource = selectedApts;
            LV_apts.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setAutocomplete", "setAutocomplete();", true);
        }

        protected void LV_apts_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            TextBox txt = null;
            Label lbl = null;
            List<selApt> _list = new List<selApt>();
            List<selApt> _listOld = new List<selApt>();
            foreach (ListViewDataItem _item in LV_apts.Items)
            {
                txt = _item.FindControl("txt_choice") as TextBox;
                lbl = _item.FindControl("lbl_id") as Label;
                if (txt != null && lbl != null)
                {
                    _listOld.Add(new selApt(lbl.Text.ToInt32(), 0, txt.Text));
                }
            }
            txt = e.Item.FindControl("txt_choice") as TextBox;
            lbl = e.Item.FindControl("lbl_id") as Label;
            if (txt != null && lbl != null)
            {
                selApt _sel = _listOld.SingleOrDefault(x => x.ID == lbl.Text.ToInt32());
                if (_sel != null)
                    _listOld.Remove(_sel);
            }
            foreach (selApt apt in _listOld)
            {
                _list.Add(new selApt(_list.Count + 1, 0, apt.Apt));
            }
            selectedApts = _list;
            LV_apts.DataSource = selectedApts;
            LV_apts.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setAutocomplete", "setAutocomplete();", true);
        }
    }
}
