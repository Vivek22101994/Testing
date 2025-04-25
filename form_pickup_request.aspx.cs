using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class form_pickup_request : mainBasePage
    {
        //magaLimo_DataContext DC_LIMO;
        magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            TimeSpan _time = new TimeSpan(1, 2, 3);
            string _t = _time.ToString();
            //DC_LIMO = maga_DataContext.DC_LIMO;
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
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
                //HF_meeting_date.Value = DateTime.Now.AddDays(7).JSCal_dateToString();
                lnkSend.Text = CurrentSource.getSysLangValue("reqSubmit") != "" ? "<span>" + CurrentSource.getSysLangValue("reqSubmit") + "</span>" : "<span>Submit</span>";
            }
        }
        protected void lnk_send_Click(object sender, EventArgs e)
        {
            string _br = "";
            bool alternateOld = true;
            LIMO_TBL_REQUEST _request = new LIMO_TBL_REQUEST();
            _request.pid_lang = CurrentLang.ID;
            string _mailBody = "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
            string _row = "";
            _row += "<tr>";
            _row += "<td><strong>Lingua</strong></td>";
            _row += "<td>" + CurrentLang.TITLE + "</td>";
            _row += "</tr>";
            _mailBody += _row;
            _request.name_full = txt_name_full.Value;
            _mailBody += MailingUtilities.addMailRow("Nome/Cognome", _request.name_full, alternateOld, out alternateOld, false, false, false);
            _request.phone = txt_phone.Value;
            _mailBody += MailingUtilities.addMailRow("Telefono", "" + _request.phone, alternateOld, out alternateOld, false, false, false);
            _request.email = txt_email.Value;
            _mailBody += MailingUtilities.addMailRow("Email", "" + _request.email, alternateOld, out alternateOld, false, false, false);


            _mailBody += MailingUtilities.addMailRow("Arrival / departure ", "" + drp_trip_mode.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
            if (drp_trip_mode.getSelectedValueInt(0) == 1 || drp_trip_mode.getSelectedValueInt(0) == 3)
            {
                _mailBody += MailingUtilities.addMailRow("Arrival place ", "" + drp_in_place.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
                if (drp_in_place.SelectedValue=="air")
                {
                    _mailBody += MailingUtilities.addMailRow("Airport name", "" + drp_in_air_place.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
                    _mailBody += MailingUtilities.addMailRow("Airline / flight n.", "" + txt_in_air_dett.Text, alternateOld, out alternateOld, false, false, false);
                }
                if (drp_in_place.SelectedValue == "sea")
                {
                    _mailBody += MailingUtilities.addMailRow("Seaport name", "" + drp_in_sea_place.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
                    _mailBody += MailingUtilities.addMailRow("Shipping company", "" + txt_in_sea_dett.Text, alternateOld, out alternateOld, false, false, false);
                }
                if (drp_in_place.SelectedValue == "train")
                {
                    _mailBody += MailingUtilities.addMailRow("Railway stat. name", "" + drp_in_train_place.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
                    _mailBody += MailingUtilities.addMailRow("Train n.", "" + txt_in_train_dett.Text, alternateOld, out alternateOld, false, false, false);
                }
                if (drp_in_place.SelectedValue == "other")
                {
                    _mailBody += MailingUtilities.addMailRow("Other place", "" + txt_in_other_dett.Text, alternateOld, out alternateOld, false, false, false);
                }
                _mailBody += MailingUtilities.addMailRow("Arrival date", "" + HF_in_date.Value.JSCal_stringToDate().formatITA_Long(true,false), alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("Arrival time", "" + HF_in_time_cont.Value.JSTime_stringToTime().JSTime_toString(false,true), alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("Destination Apartment", "" + txt_dest_apt.Value, alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("or other address", "" + txt_dest_other.Value, alternateOld, out alternateOld, false, false, false);
            }
            if (drp_trip_mode.getSelectedValueInt(0) == 2 || drp_trip_mode.getSelectedValueInt(0) == 3)
            {
                _mailBody += MailingUtilities.addMailRow("Departure place ", "" + drp_out_place.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
                if (drp_out_place.SelectedValue == "air")
                {
                    _mailBody += MailingUtilities.addMailRow("Airport name", "" + drp_out_air_place.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
                    _mailBody += MailingUtilities.addMailRow("Airline / flight n.", "" + txt_out_air_dett.Text, alternateOld, out alternateOld, false, false, false);
                }
                if (drp_out_place.SelectedValue == "sea")
                {
                    _mailBody += MailingUtilities.addMailRow("Seaport name", "" + drp_out_sea_place.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
                    _mailBody += MailingUtilities.addMailRow("Shipping company", "" + txt_out_sea_dett.Text, alternateOld, out alternateOld, false, false, false);
                }
                if (drp_out_place.SelectedValue == "train")
                {
                    _mailBody += MailingUtilities.addMailRow("Railway stat. name", "" + drp_out_train_place.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
                    _mailBody += MailingUtilities.addMailRow("Train n.", "" + txt_out_train_dett.Text, alternateOld, out alternateOld, false, false, false);
                }
                if (drp_out_place.SelectedValue == "other")
                {
                    _mailBody += MailingUtilities.addMailRow("Other place", "" + txt_out_other_dett.Text, alternateOld, out alternateOld, false, false, false);
                }
                _mailBody += MailingUtilities.addMailRow("Departure date", "" + HF_out_date.Value.JSCal_stringToDate().formatITA_Long(true, false), alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("Departure time", "" + HF_out_time_cont.Value.JSTime_stringToTime().JSTime_toString(false, true), alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("Pickup Apartment", "" + txt_pick_apt.Value, alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("or other address", "" + txt_pick_other.Value, alternateOld, out alternateOld, false, false, false);
            }
            _request.num_adult = drp_num_adult.Value.ToInt32();
            _mailBody += MailingUtilities.addMailRow("num. Adulti", "" + _request.num_adult, alternateOld, out alternateOld, false, false, false);
            _request.num_child = drp_num_child.Value.ToInt32();
            _mailBody += MailingUtilities.addMailRow("num. Bambini", "" + _request.num_child, alternateOld, out alternateOld, false, false, false);
            _request.num_case_s = drp_num_case_s.Value.ToInt32();
            _mailBody += MailingUtilities.addMailRow("num. Valigie Piccole", "" + _request.num_case_s, alternateOld, out alternateOld, false, false, false);
            _request.num_case_m = drp_num_case_m.Value.ToInt32();
            _mailBody += MailingUtilities.addMailRow("num. Valigie Medie", "" + _request.num_case_m, alternateOld, out alternateOld, false, false, false);
            _request.num_case_l = drp_num_case_l.Value.ToInt32();
            _mailBody += MailingUtilities.addMailRow("num. Valigie Grosse", "" + _request.num_case_l, alternateOld, out alternateOld, false, false, false);

            //_request.notes = txt_note.Value.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            //_mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + _request.notes, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + txt_note.Value.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"), alternateOld, out alternateOld, false, false, false);
            
            _request.request_date_created = DateTime.Now;
            _mailBody += MailingUtilities.addMailRow("Data ora creazione", "" + _request.request_date_created, alternateOld, out alternateOld, false, false, false);
            _request.state_date = DateTime.Now;
            _request.state_pid = 1;
            _request.state_subject = "Creata Richiesta";
            _request.state_pid_user = 1;
            _request.request_ip = Request.ServerVariables.Get("REMOTE_HOST");
            _request.pid_creator = 1;


            //DC_LIMO.LIMO_TBL_REQUESTs.InsertOnSubmit(_request);
            //DC_LIMO.SubmitChanges();
            //rntUtils.rntRequest_addState(_request.id, _request.state_pid.Value, _request.state_pid_user.Value, _request.state_comments);
            pnl_request.Visible = false;
            pnl_request_sent.Visible = true;

            _mailBody += "</table>";
            _request.mail_body = _mailBody;
            string _mSubject = "";
            _mSubject = "Richiesta Pickup Service RiR - rif." + _request.id + " - " + _request.name_full;
            int pid_operator = 20;
            //MailingUtilities.autoSendMailTo(_mSubject, _mailBody, "info@rentalinrome.com", false);
            //_request.operator_date = DateTime.Now;
            //_request.pid_operator = pid_operator;
            //string _mailSend = "";
            //if (MailingUtilities.autoSendMailTo(_mSubject, _mailBody, AdminUtilities.usr_adminEmail(pid_operator, ""), false))
            //    _mailSend = "Assegnato e inviato mail a " + AdminUtilities.usr_adminName(pid_operator, "") + " (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
            //else
            //    _mailSend = "Assegnato " + AdminUtilities.usr_adminName(pid_operator, "") + " - Errore nel invio mail a (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
            //_mailBody = _mailSend + "<br/><br/>" + _mailBody;
            ////rntUtils.rntRequest_addState(_request.id, 0, _request.state_pid_user.Value, _mailSend);
            //DC_LIMO.SubmitChanges();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Show_inf_hotel_request_send", "setTimeout('parent.$(\".formreportapp\").colorbox.close();',3000);", true);
            MailingUtilities.autoSendMailTo(_mSubject, _mailBody, "pickupservice@rentalinrome.com", true, "form_pickup_request a pickupservice@rentalinrome.com");
        }


        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            //drp_country.Items.Insert(0, new ListItem("- - -", ""));
            //if (CurrentLang.ID != 2)
            //{
            //    LOC_LK_COUNTRY _c = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.FirstOrDefault(
            //            x => x.inner_notes.ToLower() == CurrentLang.NAME.Substring(3, 2).ToLower());
            //    if (_c != null)
            //        drp_country.setSelectedValue(_c.id.ToString());
            //}
        }
    }
}
