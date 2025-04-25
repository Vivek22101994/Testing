using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;
using ModAuth;
using System.IO;
using MailBee.Security;
using MailBee.ImapMail;
using RentalInRome.admin.uc;


namespace ModRental.admin.modRental.ucResDett
{
    public partial class resSendEmailCustom : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        protected magaMail_DataContext DC_Mail;
        protected RNT_TBL_RESERVATION currTbl;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        public long IdReservation
        {
            get { return HF_id.Value.ToInt32(); }
            set
            {
                HF_id.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

        }

        public void FillControls()
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                this.Visible = false;
                return;
            }
            string _estate_name = CurrentSource.rntEstate_title(currTbl.pid_estate.objToInt32(), currTbl.cl_pid_lang.Value, "Error");

            string body = MailingUtilities.mailTemplate_body("cl_payment_receive_Booking", currTbl.cl_pid_lang.Value, "");


            body = body.Replace("#res_id#", currTbl.id + "");

            body = body.Replace("#cl_name_honorific#", currTbl.cl_name_honorific);
            body = body.Replace("#cl_name_full#", currTbl.cl_name_full);
            body = body.Replace("#estate_name#", _estate_name);
            body = body.Replace("#estate_id#", "" + currTbl.pid_estate.objToInt32());

            body = body.Replace("#dtStart#", "" + currTbl.dtStart.formatCustom("#dd# #MM# #yy#", currTbl.cl_pid_lang.Value, " --/--/--"));
            body = body.Replace("#dtEnd#", "" + currTbl.dtEnd.formatCustom("#dd# #MM# #yy#", currTbl.cl_pid_lang.Value, " --/--/--"));
            if (currTbl.dtEnd.HasValue && currTbl.dtStart.HasValue)
                body = body.Replace("#dtCount#", "" + (currTbl.dtEnd.Value.Date - currTbl.dtStart.Value.Date).Days);
            body = body.Replace("#num_pers_total#", "" + (currTbl.num_adult + currTbl.num_child_over + currTbl.num_child_min));
            body = body.Replace("#num_adult#", "" + (currTbl.num_adult.objToInt32()));
            body = body.Replace("#num_child_over#", "" + (currTbl.num_child_over.objToInt32()));
            body = body.Replace("#num_child_min#", "" + (currTbl.num_child_min.objToInt32()));
            body = body.Replace("#pr_total#", "" + currTbl.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
            body = body.Replace("#pr_half_amount#", "" + decimal.Divide(currTbl.pr_total.objToDecimal(), 2).objToDecimal().ToString("N2") + "&nbsp;&euro;");
            body = body.Replace("#pr_payed#", "" + currTbl.pr_part_forPayment.objToDecimal().ToString("N2") + "&nbsp;&euro;");
            body = body.Replace("#pr_to_pay#", "" + (currTbl.pr_total - currTbl.pr_part_payment_total).objToDecimal().ToString("N2") + "&nbsp;&euro;");
            body = body.Replace("#link_reservation_area_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + currTbl.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", currTbl.cl_pid_lang.Value, "Here") + "</a>");

            txt_mail_subject.Text = MailingUtilities.mailTemplate_subject("cl_payment_receive_Booking", currTbl.cl_pid_lang.Value, "");
            txt_mailBody.Content = body;

            Bind_Mail_LV();

        }

        protected void lnkSendMail_Click(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('Errore imprvisto contattare assistenza!')", true);
                return;
            }
            string _senderName = "Rental In Rome";
            string _senderEmail = CommonUtilities.getSYS_SETTING("reception_messagesSender_email");
            string _replyEmail = _senderEmail.Replace("@", "-" + currTbl.id + "@");

            // add on 2015-08-31
            if (MailingUtilities.mannualSendMailTo_From(txt_mail_subject.Text, txt_mailBody.Content, currTbl.cl_email, false, _senderEmail, _senderName, "cl_payment_receive_Booking", currTbl.unique_id.ToString()))
            {
                DC_Mail = maga_DataContext.DC_MAIL;
                //rntUtils.rntReservation_addState(IdReservation, 8, UserAuthentication.CurrentUserID, txt_mail_subject.Text, txt_mailBody.Content);
                var newMsg = new MAIL_TBL_MESSAGE();
                newMsg.date_imported = DateTime.Now;
                newMsg.reservationID = IdReservation;
                newMsg.pid_user = UserAuthentication.CurrentUserID;
                // newMsg.UidOnServer = (long)_message.MessagePreview.UidOnServer;
                newMsg.date_received = DateTime.Now;
                newMsg.from_email = _senderEmail;
                newMsg.from_name = _senderName;
                newMsg.body_html_text = txt_mailBody.Content;
                newMsg.body_plain_text = txt_mailBody.Content;
                newMsg.date_sent = DateTime.Now;
                newMsg.subject = txt_mail_subject.Text;
                newMsg.to_email = currTbl.cl_email;
                newMsg.to_name = currTbl.cl_name_full;
                newMsg.is_new = 1;
                newMsg.pid_request = 0;
                newMsg.pid_request_state = 0;
                newMsg.IsReply = 0;
                DC_Mail.MAIL_TBL_MESSAGE.InsertOnSubmit(newMsg);
                DC_Mail.SubmitChanges();

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('e-mail inviata con successo');", true);
            }
            else
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('Errore imprvisto contattare assistenza!')", true);

            FillControls();

        }

        protected void Bind_Mail_LV()
        {
            DC_Mail = maga_DataContext.DC_MAIL;
            string fromEmail = CommonUtilities.getSYS_SETTING("reception_messagesSender_email");
            //  string replyToEmail = fromEmail.Replace("@", "-" + IdReservation + "@");
            RefreshMail(currTbl.unique_id.ToString(), currTbl.cl_email);

            var listEmails = DC_Mail.MAIL_TBL_MESSAGE.Where(x => x.reservationID == IdReservation && (x.IsReply.Value == 0 || x.IsReply.Value == 1)).OrderBy(x => x.date_received).ToList();
            LV.DataSource = listEmails;
            LV.DataBind();

            if (listEmails.Count > 0)
            {
                //PH_SendNew.Visible = false;
                PH_history.Visible = true;
            }
            else
            {
                // PH_SendNew.Visible = true;
                PH_history.Visible = false;
            }

        }

        protected void RefreshMail(string uniqeResid, string clientEmail)
        {
            DC_Mail = maga_DataContext.DC_MAIL;
            USR_ADMIN_CONFIG _config = UserAuthentication.CURRENT_USR_ADMIN_CONFIG;
            if (_config == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertError", "alert('ricordati di modificare/inserire la password della tua e-mail');", true);
                //  HF_pid_user.Value = "0";
                return;
            }
            // HF_usr_mail.Value = _config.mailing_imap_usr;
            //HF_pid_user.Value = "" + _config.id;
            int userID = _config.id;
            string userName = UserAuthentication.CurrentUserName;
            string mailing_imap_email = CommonUtilities.getSYS_SETTING("reception_messagesSender_email");
            string mailing_imap_password = CommonUtilities.getSYS_SETTING("reception_messagesSender_password");
            // dati del utente
            Imap imp = new Imap();

            try
            {
                imp.Connect(_config.mailing_imap_host, _config.mailing_imap_port.Value);
                imp.SslProtocol = SecurityProtocol.Auto;
                imp.Login(mailing_imap_email, mailing_imap_password);
                imp.SelectFolder("INBOX");
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", Request.Url.AbsoluteUri, "Host:" + _config.mailing_imap_host + ", Port :-" + _config.mailing_imap_port.Value + "Email :-" + mailing_imap_email + "Password :" + mailing_imap_password + "<br/>" + ex.Message.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertError", "alert('la password di accesso non risulta corretto, riprova ad inserirla');", true);
                return;
            }

            string range;
            EnvelopeCollection _newMessages = new EnvelopeCollection();
            int msgCount = 50; // default
            if (imp.MessageCount <= msgCount)
                msgCount = imp.MessageCount - 1;
            range = (imp.MessageCount - msgCount).ToString() + ":" + "*";
            UidCollection _uidCol = new UidCollection();
            string strquery = string.Format("FROM \"{0}\"", clientEmail);
            _uidCol = (UidCollection)imp.Search(true, strquery, null);

            // _newMessages = imp.DownloadEnvelopes(range, false, EnvelopeParts.BodyStructure | EnvelopeParts.MessagePreview | EnvelopeParts.InternalDate, 999999999, new string[] { "Reply-To", "X-Ref-ID" }, null);
            if (_uidCol.Count > 0)
                _newMessages = imp.DownloadEnvelopes(_uidCol.ToString(), true, EnvelopeParts.All, 999999999, null, null);


            foreach (Envelope _message in _newMessages)
            {
                try
                {
                    MAIL_TBL_MESSAGE newMsg = DC_Mail.MAIL_TBL_MESSAGE.FirstOrDefault(x => x.reservationID == IdReservation && x.UidOnServer == (long)_message.MessagePreview.UidOnServer);
                    if (newMsg == null)
                    {
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
                        newMsg.IsReply = 1;
                        newMsg.reservationID = IdReservation;
                        DC_Mail.MAIL_TBL_MESSAGE.InsertOnSubmit(newMsg);
                        DC_Mail.SubmitChanges();
                    }
                }

                catch (Exception exc)
                {
                    ErrorLog.addLog("", Request.Url.AbsolutePath, exc.Message.ToString());
                }
            }
            imp.Disconnect();
        }


    }
}