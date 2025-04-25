using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MailBee;
using MailBee.DnsMX;
using MailBee.Mime;
using MailBee.SmtpMail;
using MailBee.Pop3Mail;
using MailBee.ImapMail;
using MailBee.Security;
using MailBee.AntiSpam;
using MailBee.Outlook;
using RentalInRome.admin.uc;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_mail_new : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_mail";
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnk_send_Click(object sender, EventArgs e)
        {
            int userID = UserAuthentication.CurrentUserID;
            string userName = UserAuthentication.CurrentUserName;
            USR_ADMIN_CONFIG _config = UserAuthentication.CURRENT_USR_ADMIN_CONFIG;
            if(_config==null)
                return;

            MailingUtilities.autoSendMailTo_from(txt_subject.Text, txt_body.Text, txt_to.Text, false, _config.mailing_from_mail, _config.mailing_from_name, "admin_usr_mail_new");
        }
    }
}
