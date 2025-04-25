using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class util_MailErrorLogDett : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mailUtils.MailLogList _list = new mailUtils.MailLogList(Request.QueryString["dt"]);
                mailUtils.MailLogItem item = _list.Items.SingleOrDefault(x => x.ID == Request.QueryString["id"]);
                if (item == null) return;

                ltr_isResent.Text = item.isResent ? "<span style=\"color: #0f0;\">Reinviato</span>" : "<span style=\"color: #f00;\">da reinviare</span>";
                ltr_Date.Text = item.Date.JSCal_stringToDateTime().ToString();
                ltr_IP.Text = item.IP;
                ltr_mailDescrtiption.Text = item.mailDescrtiption;
                ltr_Subject.Text = item.Subject;
                ltr_From.Text = item.FROM_NAME + " &lt;" + item.FROM_MAIL + "&gt;";
                ltr_TOs.Text = item.TOs.listToString("; ");
                ltr_Body.Text = item.Body;
                txt_Value.Text = item.Value;
                lnk_resend.Visible = !item.isResent;
            }
        }
        protected void lnk_resend_Click(object sender, EventArgs e)
        {
            mailUtils.MailLogList _list = new mailUtils.MailLogList(Request.QueryString["dt"]);
            mailUtils.MailLogItem item = _list.Items.SingleOrDefault(x => x.ID == Request.QueryString["id"]);
            if (item == null) return;
            item.isResent = true;
            _list.Save();
            mailUtils._list = null;
            ltr_isResent.Text = item.isResent ? "<span style=\"color: #0f0;\">Reinviato</span>" : "<span style=\"color: #f00;\">da reinviare</span>";
            lnk_resend.Visible = !item.isResent;
            if (MailingUtilities.customSendMailTo(item.Subject, item.Body, item.TOs, item.CCs, item.Attachments, item.bcc, item.FROM_MAIL, item.FROM_NAME, "reinvio: "+item.mailDescrtiption))
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"La mail è stato reinviato con successo.\", 340, 110);", true);
            else
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Errore nel'invio mail,<br/>è stato creato un'altro log.\", 340, 110);", true);
        }
    }
}