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
    public partial class usr_mail_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_mail";
        }
        protected magaMail_DataContext DC_MAIL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_MAIL = maga_DataContext.DC_MAIL;
            if (!IsPostBack)
            {
                if (Request.QueryString["pg"].ToInt32() == 0)
                    RefreshMail(50);
                FillList();
            }
        }
        //[STAThread]
        protected void drpUpdateMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpUpdateMessages.getSelectedValueInt() > 0)
            {
                RefreshMail(drpUpdateMessages.getSelectedValueInt());
                drpUpdateMessages.setSelectedValue(0);
                FillList();
            }
        }

        protected bool IsNewMessage(string UID)
        {
            return true;
        }
        protected void RefreshMail(long msgCount)
        {
            USR_ADMIN_CONFIG _config = UserAuthentication.CURRENT_USR_ADMIN_CONFIG;
            if (_config == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    "alertError",
                                                    "alert('ricordati di modificare/inserire la password della tua e-mail');", true);
                HF_pid_user.Value = "0";
                return;
            }
            HF_usr_mail.Value = _config.mailing_imap_usr;
            HF_pid_user.Value = "" + _config.id;
            int userID = _config.id;
            string userName = UserAuthentication.CurrentUserName;

            // dati del utente
            Imap imp = new Imap();

            try
            {
                imp.Connect(_config.mailing_imap_host, _config.mailing_imap_port.objToInt32());
                imp.SslProtocol = SecurityProtocol.Auto;
                imp.Login(_config.mailing_imap_usr, _config.mailing_imap_pwd);
                imp.SelectFolder("INBOX");

            }
            catch (Exception ex)
            {
                try
                {
                    imp.Connect("imap.googlemail.com", _config.mailing_imap_port.objToInt32());
                    imp.SslProtocol = SecurityProtocol.Auto;
                    imp.Login(_config.mailing_imap_usr, _config.mailing_imap_pwd);
                    imp.SelectFolder("INBOX");

                }
                catch (Exception ex2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                        "alertError",
                                                        "alert('la password di accesso non risulta corretto, riprova ad inserirla');", true);
                    HF_pid_user.Value = "0";
                    return;
                }
            }

            string range;
            EnvelopeCollection _newMessages = new EnvelopeCollection();
            if (imp.MessageCount <= msgCount)
                msgCount = imp.MessageCount - 1;
            range = (imp.MessageCount - msgCount).ToString() + ":" + "*";
            _newMessages = imp.DownloadEnvelopes(range, false, EnvelopeParts.BodyStructure | EnvelopeParts.MessagePreview | EnvelopeParts.InternalDate, 999999999);
            UidCollection _uidCol = new UidCollection();
            // Get envelopes for the specified messages.

            // Make newer messages be displayed first.
            //_newMessages.Reverse();
            foreach (Envelope _message in _newMessages)
            {
                try
                {

                    MAIL_TBL_MESSAGE newMsg = DC_MAIL.MAIL_TBL_MESSAGE.FirstOrDefault(x => x.pid_user == userID && x.UidOnServer == (long)_message.MessagePreview.UidOnServer);
                    if (newMsg != null) continue;
                    newMsg = new MAIL_TBL_MESSAGE();
                    newMsg.date_imported = DateTime.Now;
                    newMsg.pid_user = userID;
                    newMsg.UidOnServer = (long)_message.MessagePreview.UidOnServer;
                    newMsg.date_received = _message.MessagePreview.DateReceived.sqlServerDateCheck();
                    newMsg.from_email = _message.MessagePreview.From.Email;
                    newMsg.from_name = _message.MessagePreview.From.DisplayName;
                    newMsg.body_html_text = _message.MessagePreview.BodyHtmlText;
                    newMsg.body_plain_text = _message.MessagePreview.BodyPlainText;
                    newMsg.date_sent = _message.MessagePreview.Date.sqlServerDateCheck();
                    newMsg.MessageID = _message.MessagePreview.MessageID;
                    newMsg.subject = _message.MessagePreview.Subject;
                    newMsg.to_email = _message.MessagePreview.To.ToString();
                    newMsg.to_name = userName;
                    newMsg.is_new = 1;
                    newMsg.pid_request = 0;
                    newMsg.pid_request_state = 0;
                    //_message.From.
                    if (DC_MAIL.MAIL_TBL_MESSAGE.FirstOrDefault(x => x.UidOnServer == newMsg.UidOnServer && x.pid_user == newMsg.pid_user) == null)
                    {
                        DC_MAIL.MAIL_TBL_MESSAGE.InsertOnSubmit(newMsg);
                        DC_MAIL.SubmitChanges();
                    }
                }
                catch (Exception exc)
                { }
            }
            imp.Disconnect();
        }
        protected void RefreshMail_old()
        {
            USR_ADMIN_CONFIG _config = UserAuthentication.CURRENT_USR_ADMIN_CONFIG;
            if (_config == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    "alertError",
                                                    "alert('ricordati di modificare/inserire la password della tua e-mail');", true);
                HF_pid_user.Value = "0";
                return;
            }
            HF_usr_mail.Value = _config.mailing_imap_usr;
            HF_pid_user.Value = "" + _config.id;
            int userID = _config.id;
            string userName = UserAuthentication.CurrentUserName;

            // dati del utente
            Imap imp = new Imap();

            try
            {
                imp.Connect(_config.mailing_imap_host, _config.mailing_imap_port.objToInt32());
                imp.SslProtocol = SecurityProtocol.Auto;
                imp.Login(_config.mailing_imap_usr, _config.mailing_imap_pwd);
                imp.SelectFolder("INBOX");

            }
            catch (Exception ex)
            {
                try
                {
                    imp.Connect("imap.googlemail.com", _config.mailing_imap_port.objToInt32());
                    imp.SslProtocol = SecurityProtocol.Auto;
                    imp.Login(_config.mailing_imap_usr, _config.mailing_imap_pwd);
                    imp.SelectFolder("INBOX");

                }
                catch (Exception ex2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                        "alertError",
                                                        "alert('la password di accesso non risulta corretto, riprova ad inserirla');", true);
                    HF_pid_user.Value = "0";
                    return;
                }
            }

            string range;
            EnvelopeCollection _newMessages = new EnvelopeCollection();
            MAIL_TBL_MESSAGE _lastMessage = DC_MAIL.MAIL_TBL_MESSAGE.Where(x => x.pid_user == userID).OrderByDescending(x => x.UidOnServer).FirstOrDefault();
            if (_lastMessage == null)
            {
                if (imp.MessageCount >= 200)
                {
                    // We'll get last 500 mails.
                    range = (imp.MessageCount - 100).ToString() + ":" + "*";
                    _newMessages = imp.DownloadEnvelopes(range, false, EnvelopeParts.BodyStructure | EnvelopeParts.MessagePreview | EnvelopeParts.InternalDate, 999999999);
                }
                else
                {
                    range = "1:*";
                    _newMessages = imp.DownloadEnvelopes(range, true, EnvelopeParts.BodyStructure | EnvelopeParts.MessagePreview | EnvelopeParts.InternalDate, 999999999);
                }
            }
            else
            {
                range = (_lastMessage.UidOnServer + 1) + ":*";
                _newMessages = imp.DownloadEnvelopes(range, true, EnvelopeParts.BodyStructure | EnvelopeParts.MessagePreview | EnvelopeParts.InternalDate, 999999999);
            }
            UidCollection _uidCol = new UidCollection();
            // Get envelopes for the specified messages.

            // Make newer messages be displayed first.
            //_newMessages.Reverse();
            foreach (Envelope _message in _newMessages)
            {
                try
                {

                    MAIL_TBL_MESSAGE _newMessage = new MAIL_TBL_MESSAGE();
                    _newMessage.date_imported = DateTime.Now;
                    _newMessage.pid_user = userID;
                    _newMessage.UidOnServer = (long)_message.MessagePreview.UidOnServer;
                    _newMessage.date_received = _message.MessagePreview.DateReceived.sqlServerDateCheck();
                    _newMessage.from_email = _message.MessagePreview.From.Email;
                    _newMessage.from_name = _message.MessagePreview.From.DisplayName;
                    _newMessage.body_html_text = _message.MessagePreview.BodyHtmlText;
                    _newMessage.body_plain_text = _message.MessagePreview.BodyPlainText;
                    _newMessage.date_sent = _message.MessagePreview.Date.sqlServerDateCheck();
                    _newMessage.MessageID = _message.MessagePreview.MessageID;
                    _newMessage.subject = _message.MessagePreview.Subject;
                    _newMessage.to_email = _message.MessagePreview.To.ToString();
                    _newMessage.to_name = userName;
                    _newMessage.is_new = 1;
                    _newMessage.pid_request = 0;
                    _newMessage.pid_request_state = 0;
                    //_message.From.
                    if (DC_MAIL.MAIL_TBL_MESSAGE.FirstOrDefault(x => x.UidOnServer == _newMessage.UidOnServer && x.pid_user == _newMessage.pid_user) == null)
                    {
                        DC_MAIL.MAIL_TBL_MESSAGE.InsertOnSubmit(_newMessage);
                        DC_MAIL.SubmitChanges();
                    }
                }
                catch (Exception exc)
                { }
            }
            imp.Disconnect();
        }

        protected void FillList()
        {
            LDS.DataBind();
            LV.DataBind();
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlTableRow tr_normal = e.Item.FindControl("tr_normal") as System.Web.UI.HtmlControls.HtmlTableRow;
            LinkButton lnk_select = e.Item.FindControl("lnk_select") as LinkButton;
            if (tr_normal != null && lnk_select != null)
            {
                tr_normal.Attributes.Add("onclick", "__doPostBack('" + lnk_select.UniqueID + "','')");
                return;
            }
            System.Web.UI.HtmlControls.HtmlTableRow tr_selected = e.Item.FindControl("tr_selected") as System.Web.UI.HtmlControls.HtmlTableRow;
            LinkButton lnk_close = e.Item.FindControl("lnk_close") as LinkButton;
            UC_mailbody UC_mailbody = e.Item.FindControl("UC_mailbody") as UC_mailbody;
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (tr_selected != null && lnk_close != null && UC_mailbody != null && lbl_id != null)
            {
                tr_selected.Attributes.Add("onclick", "__doPostBack('" + lnk_close.UniqueID + "','')");
                UC_mailbody.IdMessage = lbl_id.Text.ToInt32();
                UC_mailbody.FillControls();
                return;
            }
        }

        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            Label lbl_id = LV.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            if (lbl_id != null)
            {
                MAIL_TBL_MESSAGE _message = DC_MAIL.MAIL_TBL_MESSAGE.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (_message != null)
                {
                    _message.is_new = 0;
                    DC_MAIL.SubmitChanges();
                    LV.DataBind();
                }
            }
        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            LV.SelectedIndex = -1;
        }
    }
}
