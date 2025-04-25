using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.areariservataprop
{
    public partial class utilForm_sendMailToStaff : ownerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txt_from.Text = OwnerAuthentication.CurrentName;
        }
        protected void lnk_send_Click(object sender, EventArgs e)
        {
            string _to = CommonUtilities.getSYS_SETTING("mailing_ownerMessagesReceiver");
            bool _bcc = CommonUtilities.getSYS_SETTING("mailing_ownerMessagesReceiverBCC") == "true";
            string _subject = "RiR - Nuovo messaggio dal Proprietario " + txt_from.Text + " il " + DateTime.Now.formatCustom("#dd#/#mm#/#yy#", 1, "") + " ore " + DateTime.Now.TimeOfDay.JSTime_toString(false, true);
            bool alternateOld = true;
            string _mailBody = "";
            _mailBody += MailingUtilities.addMailRow("", "", alternateOld, out alternateOld, true, false, true);
            _mailBody += MailingUtilities.addMailRow("Nome/Cognome", "" + txt_from.Text, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Oggetto", "" + txt_subject.Text, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Messaggio", "" + txt_body.Text.htmlNoWrap(), alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("", "", alternateOld, out alternateOld, false, true, true);
            bool _ok = MailingUtilities.autoSendMailTo(_subject, _mailBody, _to, _bcc, "messaggio del proprietario");
            if(!_ok)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('ATTENZIONE!\nSi è verificato un errore imprevisto, si prega di contattare assistenza, o riprovare piu tardi.');", true);
                return;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Il suo messagio è stato inviato correttamente.');", true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ReloadContent", "setTimeout(\"parent.Shadowbox.close()\",0);", true);
        }
    }
}