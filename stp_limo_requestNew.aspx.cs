using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome
{
    public partial class stp_limo_requestNew : contStpBasePage
    {
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
                ErrorLog.addLog(_ip, "stp", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
        }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                setPlaceHolder();


                drp_adult.bind_Numbers(1, 10, 1, 0);
                drp_adult.setSelectedValue("2");
                drp_child_over.bind_Numbers(0, 10, 1, 0);
                drp_child_min.bind_Numbers(0, 10, 1, 0);

                drp_num_case_l.bind_Numbers(0, 10, 1, 0);
                drp_num_case_l.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblLargeSuitcases"), "0"));

                drp_num_case_m.bind_Numbers(0, 10, 1, 0);
                drp_num_case_m.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblMediumSuitcases"), "0"));
                drp_num_case_m.setSelectedValue("1");

                drp_num_case_s.bind_Numbers(0, 10, 1, 0);
                drp_num_case_s.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblSmallSuitcases"), "0"));
                drp_num_case_s.setSelectedValue("2");

                Bind_drp_honorific();
                string setCal_ = @"
                    var cal_in_date_#Unique#;
                    var cal_out_date_#Unique#;
			        function setCal_#Unique#() {
                        cal_in_date_#Unique# = new JSCal.Single({ dtFormat: ""d MM yy"", Cont: ""#HF_in_date.ClientID"", View: ""#txt_in_date"", changeMonth: true, changeYear: true });
                        cal_out_date_#Unique# = new JSCal.Single({ dtFormat: ""d MM yy"", Cont: ""#HF_out_date.ClientID"", View: ""#txt_out_date"", changeMonth: true, changeYear: true });
			        }
                ";
                setCal_ = setCal_.Replace("#Unique#", Unique);
                setCal_ = setCal_.Replace("HF_in_date.ClientID", HF_in_date.ClientID);
                setCal_ = setCal_.Replace("HF_out_date.ClientID", HF_out_date.ClientID);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_setCal_" + Unique, setCal_, true);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal_" + Unique + "();", true);
        }

        protected void setPlaceHolder()
        {
            txt_name_full.Attributes.Add("placeholder", CurrentSource.getSysLangValue("reqFullName") + "*");
            txt_email.Attributes.Add("placeholder", CurrentSource.getSysLangValue("reqEmail") + "*");
            txt_email_conf.Attributes.Add("placeholder", CurrentSource.getSysLangValue("reqEmailConfirm") + "*");
            txt_phone_mobile.Attributes.Add("placeholder", CurrentSource.getSysLangValue("reqPhoneNumber") + "*");

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

        protected void lnk_send_Click(object sender, EventArgs e)
        {
            saveRequest();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "scrollTo_sent", "$.scrollTo($(\"#" + pnl_request_sent.ClientID + "\"), 500);", true);
        }

        protected void saveRequest()
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
            _request.name_full = txt_name_full.Text;
            _mailBody += MailingUtilities.addMailRow("Nome/Cognome", _request.name_full, alternateOld, out alternateOld, false, false, false);
            _request.phone = txt_phone_mobile.Text;
            _mailBody += MailingUtilities.addMailRow("Telefono", "" + _request.phone, alternateOld, out alternateOld, false, false, false);
            _request.email = txt_email.Text;
            _mailBody += MailingUtilities.addMailRow("Email", "" + _request.email, alternateOld, out alternateOld, false, false, false);


            _mailBody += MailingUtilities.addMailRow("Arrival / departure ", "" + drp_trip_mode.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
            if (drp_trip_mode.getSelectedValueInt(0) == 1 || drp_trip_mode.getSelectedValueInt(0) == 3)
            {
                _mailBody += MailingUtilities.addMailRow("Arrival place ", "" + drp_in_place.SelectedItem.Text, alternateOld, out alternateOld, false, false, false);
                if (drp_in_place.SelectedValue == "air")
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
                _mailBody += MailingUtilities.addMailRow("Arrival date", "" + HF_in_date.Value.JSCal_stringToDate().formatITA_Long(true, false), alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("Arrival time", "" + in_time_view1.Value, alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("Destination Apartment", "" + txt_dest_apt.Text, alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("or other address", "" + txt_dest_other.Text, alternateOld, out alternateOld, false, false, false);
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
                _mailBody += MailingUtilities.addMailRow("Arrival date", "" + HF_out_date.Value.JSCal_stringToDate().formatITA_Long(true, false), alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("Arrival time", "" + out_time_view1.Value, alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("Pickup Apartment", "" + txt_pick_apt.Text, alternateOld, out alternateOld, false, false, false);
                _mailBody += MailingUtilities.addMailRow("or other address", "" + txt_pick_other.Text, alternateOld, out alternateOld, false, false, false);
            }
            _request.num_adult = drp_adult.getSelectedValueInt(0);
            _mailBody += MailingUtilities.addMailRow("num. Adulti", "" + _request.num_adult, alternateOld, out alternateOld, false, false, false);
            _request.num_child = drp_child_over.getSelectedValueInt(0);
            _mailBody += MailingUtilities.addMailRow("num. Bambini", "" + _request.num_child, alternateOld, out alternateOld, false, false, false);
            _request.num_case_s = drp_num_case_s.getSelectedValueInt(0);
            _mailBody += MailingUtilities.addMailRow("num. Valigie Piccole", "" + _request.num_case_s, alternateOld, out alternateOld, false, false, false);
            _request.num_case_m = drp_num_case_m.getSelectedValueInt(0);
            _mailBody += MailingUtilities.addMailRow("num. Valigie Medie", "" + _request.num_case_m, alternateOld, out alternateOld, false, false, false);
            _request.num_case_l = drp_num_case_l.getSelectedValueInt(0);
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
            pnl_request_cont.Visible = false;
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
            MailingUtilities.autoSendMailTo(_mSubject, _mailBody, "pickupservice@rentalinrome.com", false, "stp_limo_request a pickupservice@rentalinrome.com");
        }


    }
}