using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class stp_ArtCitiesTour : mainBasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Fill_data();
            }
        }
        protected void Fill_data()
        {
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
                ltr_img_banner.Text = _stp.img_banner;
            }
            _stp = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == 2 && item.pid_lang == CurrentLang.ID);
            if (_stp == null)
                _stp = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == 2 && item.pid_lang == 2);
            if (_stp != null)
                ltr_privacy.Text = _stp.description;
            var url_voucher = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "stp_contentonly.aspx?id=2&lang=" + CurrentLang.ID;
            var filename = "RiR_" + CurrentSource.getSysLangValue("lblPrivacyPolicy").clearPathName() + ".pdf";
            HL_getPdf_privacy.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher.urlEncode() + "&filename=" + filename.urlEncode();
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
            RNT_TBL_REQUEST _request = new RNT_TBL_REQUEST();
            _request.pid_lang = CurrentLang.ID;
            string _mailBody = "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
            string _row = "";
            _row += "<tr>";
            _row += "<td><strong>Lingua</strong></td>";
            _row += "<td>" + CurrentLang.TITLE + "</td>";
            _row += "</tr>";
            _mailBody += _row;
            _request.name_first = txt_name_first.Text;
            _mailBody += MailingUtilities.addMailRow("Nome", _request.name_first, alternateOld, out alternateOld, false, false, false);
            _request.name_last = txt_name_last.Text;
            _mailBody += MailingUtilities.addMailRow("Cognome", _request.name_last, alternateOld, out alternateOld, false, false, false);
            _request.email = txt_email.Text;
            _mailBody += MailingUtilities.addMailRow("Email", "" + _request.email, alternateOld, out alternateOld, false, false, false);
            _request.phone = txt_phone_mobile.Text;
            _mailBody += MailingUtilities.addMailRow("Telefono", "" + _request.phone, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Recall", "" + (chkRequestRecall.Checked?"Si":"No"), alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Formula", "" + drpFormula.SelectedValue, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Giorno", "" + (rdp_tourDay.SelectedDate.HasValue ? rdp_tourDay.SelectedDate.formatITA(true) : ""), alternateOld, out alternateOld, false, false, false);
            //_request.request_notes = txt_note.Value.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            //_mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + _request.request_notes, alternateOld, out alternateOld, false, false, false);
            _request.request_date_created = DateTime.Now;
            _mailBody += MailingUtilities.addMailRow("Data ora creazione", "" + _request.request_date_created, alternateOld, out alternateOld, false, false, false);

            pnl_request_cont.Visible = false;
            pnl_request_sent.Visible = true;

            _mailBody += "</table>";
            _request.mail_body = _mailBody;
            string _mSubject = "Nuova richiesta Tour città d'arte " + DateTime.Now;
            var email = CommonUtilities.getSYS_SETTING("email_richieste_tour_citta_d_arte").isEmail() ? CommonUtilities.getSYS_SETTING("email_richieste_tour_citta_d_arte") : HF_receiverEmail.Value;
            MailingUtilities.autoSendMailTo(_mSubject, _mailBody, email, false, "stp_ArtCitiesTour al admin");

        }
    }
}
