using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome {
    public partial class form_request_flight_date : mainBasePage {
        magaContent_DataContext DC_CONTENT;
        protected void Page_Load(object sender, EventArgs e) {
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack) {
                HF_date_start.Value = DateTime.Now.AddDays(7).JSCal_dateToString();
                HF_date_end.Value = DateTime.Now.AddDays(10).JSCal_dateToString();
            }
        }
        protected void lnk_send_Click(object sender, EventArgs e) {
            var flight_req = new TBL_REQUEST_FLIGHT_DATE();
            flight_req.email = txt_email.Value;
            flight_req.name_first = txt_name_first.Value;
            flight_req.name_last = txt_name_last.Value;
            flight_req.in_date = HF_date_start.Value.JSCal_stringToDate().AddHours(drp_hour_in.SelectedValue.ToInt32()).
                AddMinutes(drp_minute_in.SelectedValue.ToInt32());
            
            flight_req.out_date = HF_date_end.Value.JSCal_stringToDate().AddHours(drp_hour_out.SelectedValue.ToInt32())
                .AddMinutes(drp_min_out.SelectedValue.ToInt32());

            flight_req.in_airport_name = txt_aiport_in.Value;
            flight_req.out_airport_name = txt_airport_out.Value;

            //DC_CONTENT.RNT_TBL_REQUEST.InsertOnSubmit(_request);
            DC_CONTENT.TBL_REQUEST_FLIGHT_DATEs.InsertOnSubmit(flight_req);
            DC_CONTENT.SubmitChanges();
            pnl_request.Visible = false;
            pnl_request_sent.Visible = true;
            string _mSubject = "Creata Nuova richiesta prenotazione";
            string _mBody = "Nome: " + txt_name_first.Value;
            _mBody += "<br/>Cognome: " + txt_name_last.Value;
            _mBody += "<br/>Email: " + txt_email.Value;
            _mBody += "<br/>Telefono: " + txt_phone.Value;
            //_mBody += "<br/>" + txt_note.Value.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            _mBody += "<br/><br/><br/>Per magiorni informazioni entrate in area amministrativa del sito...";
            //AdminUtilities.itemRequest_addState(_request.id, _request.state_pid.Value, _request.state_pid_user.Value, _request.state_comments);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Show_inf_hotel_request_send", "setTimeout('parent.$(\".formreportapp\").colorbox.close();',3000);", true);
            //MailingUtilities.autoSendMailTo(_mSubject, _mBody, MailingUtilities.ADMIN_MAIL);
        }
        protected void drp_country_DataBound(object sender, EventArgs e) {
            //drp_country.Items.Insert(0, new ListItem("- - -", ""));
            //if (CurrentLang.ID != 2) {
            //    ZONE_LK_COUNTRY _c = maga_DataContext.DC_ZONE.ZONE_LK_COUNTRies.FirstOrDefault(
            //            x => x.inner_notes.ToLower() == CurrentLang.NAME.Substring(3, 2).ToLower());
            //    if (_c != null)
            //        drp_country.setSelectedValue(_c.title);
            //}
        }
    }
}
