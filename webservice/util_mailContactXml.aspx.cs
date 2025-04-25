using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class util_mailContactXml : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        private string CURRENT_SESSION_ID;
        private int IdEstate;
        private RNT_TB_ESTATE currTBL;
        private int _currLang;
        private long agentID;
        private clSearch _ls;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                CURRENT_SESSION_ID = Request["SESSION_ID"];
                _ls = new clSearch();
                _currLang = Request["lang"].objToInt32() == 0 ? CurrentLang.ID : Request["lang"].objToInt32();
                CurrentLang.ID = _currLang;
                IdEstate = Request["id"].objToInt32();
                currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                agentID = Request["agentID"].objToInt64();

                int dtStartInt = Request["dtS"].objToInt32();
                int dtEndInt = Request["dtE"].objToInt32();
                if (dtStartInt != 0 && dtEndInt != 0)
                {
                    _ls.dtStart = dtStartInt.JSCal_intToDate();
                    _ls.dtEnd = dtEndInt.JSCal_intToDate();
                }
                _ls.dtCount = (_ls.dtEnd - _ls.dtStart).TotalDays.objToInt32();
                _ls.numPers_adult = Request["numPers_adult"].objToInt32() == 0 ? _ls.numPers_adult : Request["numPers_adult"].objToInt32();
                _ls.numPers_childOver = Request["numPers_childOver"].objToInt32() == 0 ? _ls.numPers_childOver : Request["numPers_childOver"].objToInt32();
                _ls.numPers_childMin = Request["numPers_childMin"].objToInt32() == 0 ? _ls.numPers_childMin : Request["numPers_childMin"].objToInt32();
                _ls.numPersCount = _ls.numPers_adult + _ls.numPers_childOver;
                if (Request["deviceinfo"] == "true")
                    Response.Write(sendDeviceInfo());
                else
                    Response.Write(sendMail());
                Response.End();
            }
        }
        private string sendDeviceInfo()
        {
            string special_request = Request["special_request"].htmlNoWrap();
            var senderName = CommonUtilities.getSYS_SETTING("mobileContact_senderName");
            var senderEmail = CommonUtilities.getSYS_SETTING("mobileContact_senderEmail");
            string _subject = "Rental in Rome Mobile: DeviceInfo " + DateTime.Now;
            return MailingUtilities.autoSendMailTo_from(_subject, special_request, "adilet@magadesign.net", false, senderEmail, senderName, "webservice.util_mailContactXml") ? "ok" : "ko";
        }
        private string sendMail()
        {
            string client_loc_country = "" + Request["client_loc_country"];
            string client_contact_email = "" + Request["client_contact_email"];
            string client_name_honorific = "" + Request["client_name_honorific"];
            string client_name_full = "" + Request["client_name_full"];
            string client_contact_phone_mobile = "" + Request["client_contact_phone_mobile"];
            string special_request = "" + Request["special_request"];
            var preferedEstateIds = Request["preflist"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).Where(x => x != 0).ToList();
            string prefList = "";
            foreach (var id in preferedEstateIds)
            {
                var tmp = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id);
                if (tmp != null)
                    prefList += tmp.code + "<br/>";
            }
            var senderName = CommonUtilities.getSYS_SETTING("mobileContact_senderName");
            var senderEmail = CommonUtilities.getSYS_SETTING("mobileContact_senderEmail");
            var receiver = CommonUtilities.getSYS_SETTING("mobileContact_receiver");
            bool bcc = CommonUtilities.getSYS_SETTING("mobileContact_bcc") != "false";
            string _subject = "Rental in Rome Mobile: Nuovo contatto da " + client_name_full + " " + DateTime.Now.formatITA(false);
            bool alternateOld = true;
            string _mailBody = "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
            _mailBody += MailingUtilities.addMailRow("Data Richiesta", "" + DateTime.Now, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Lingua", "" + contUtils.getLang_title(_currLang), alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Check-In", "" + _ls.dtStart.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Check-Out", "" + _ls.dtEnd.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("num. Pers", "" + _ls.numPersCount, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Preferiti", prefList, alternateOld, out alternateOld, false, false, false);
            if (currTBL != null && !preferedEstateIds.Contains(currTBL.id))
                _mailBody += MailingUtilities.addMailRow("Last visited apt", currTBL.code, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Nome/Cognome", client_name_full, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("E-mail", client_contact_email, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Telefono", client_contact_phone_mobile, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Paese (Location)", client_loc_country, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + special_request, alternateOld, out alternateOld, false, false, false);
            _mailBody += "</table>";
            return MailingUtilities.autoSendMailTo_from(_subject, _mailBody, receiver, bcc, senderEmail, senderName, "webservice.util_mailContactXml") ? "ok" : "ko";
        }
    }
}
