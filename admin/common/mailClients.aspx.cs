using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.common
{
    public partial class mailClients : System.Web.UI.Page
    {
        protected magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Url.AbsoluteUri.ToLower().Contains("http://localhost") || Request.Url.AbsoluteUri.ToLower().Contains(".magadesign.net") || Request.Url.AbsoluteUri.ToLower().Contains(".dev.magarental.com")) return;
                int cl_reminder_checkEachHours = CommonUtilities.getSYS_SETTING("cl_reminder_checkEachHours").ToInt32();
                if (cl_reminder_checkEachHours == 0)
                    cl_reminder_checkEachHours = 3;
                if (MailingUtilities.CURRENT_MAILING == null)
                {
                    MailingUtilities.CURRENT_MAILING = new MailingUtilities.MassMailing();
                    AppSettings.DEF_SYS_SETTINGs = null;
                }
                MailingUtilities.CURRENT_MAILING.lastSend = CommonUtilities.getSYS_SETTING("cl_reminder_checkLastOccurred").JSCal_stringToDateTime();
                if (!MailingUtilities.CURRENT_MAILING.isStarted
                    && MailingUtilities.CURRENT_MAILING.lastSend.AddHours(cl_reminder_checkEachHours) < DateTime.Now)
                {
                    startMailingThread();
                }
                MailingUtilities.MassMailing _current = MailingUtilities.CURRENT_MAILING;
                Response.Write(_current.isStarted ? "1" : "0");
                Response.Write("|" + _current.countTotal + "|" + _current.countCurrent + "|" + _current.description);
            }
        }
        protected void startMailingThread()
        {
            MailingUtilities.CURRENT_MAILING.isStarted = true;
            //Action<object> action = (object obj) => { startMailingThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "startMailingThread");
            ThreadStart start = new ThreadStart(mailAllClients);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        protected void mailAllClients()
        {
            try
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                int cl_reminderNormal_daysBeforCheckin = CommonUtilities.getSYS_SETTING("cl_reminderNormal_daysBeforCheckin").ToInt32();
                if (cl_reminderNormal_daysBeforCheckin == 0)
                    cl_reminderNormal_daysBeforCheckin = 28;
                int cl_reminderNormal_eachDay = CommonUtilities.getSYS_SETTING("cl_reminderNormal_eachDay").ToInt32();
                if (cl_reminderNormal_eachDay == 0)
                    cl_reminderNormal_eachDay = 7;
                int cl_reminderExtra_daysBeforCheckin = CommonUtilities.getSYS_SETTING("cl_reminderExtra_daysBeforCheckin").ToInt32();
                if (cl_reminderExtra_daysBeforCheckin == 0)
                    cl_reminderExtra_daysBeforCheckin = 7;
                int cl_reminderExtra_eachDay = CommonUtilities.getSYS_SETTING("cl_reminderExtra_eachDay").ToInt32();
                if (cl_reminderExtra_eachDay == 0)
                    cl_reminderExtra_eachDay = 1;
                DateTime _dtCheckinFromNormal = DateTime.Now.Date.AddDays(cl_reminderNormal_daysBeforCheckin).AddHours(1);
                DateTime _dtLastMailNormal = DateTime.Now.AddDays(-cl_reminderNormal_eachDay).AddHours(1);
                DateTime _dtCheckinFromExtra = DateTime.Now.Date.AddDays(cl_reminderExtra_daysBeforCheckin).AddHours(1);
                DateTime _dtLastMailExtra = DateTime.Now.AddDays(-cl_reminderExtra_eachDay).AddHours(1);

                List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.dtEnd.HasValue && x.dtStart.HasValue && x.state_pid == 4 && x.id>150000 //
                                                                                            && x.dtStart > DateTime.Now
                                                                                            && (!x.agentID.HasValue || x.agentID == 0)
                                                                                            && (x.cl_isCompleted != 1 || x.is_dtStartTimeChanged != 1 || x.is_dtEndTimeChanged != 1)
                                                                                            && ((_dtCheckinFromNormal > x.dtStart && (x.cl_reminderLast == null || _dtLastMailNormal > x.cl_reminderLast))
                                                                                                || (_dtCheckinFromExtra > x.dtStart && (x.cl_reminderLast == null || _dtLastMailExtra > x.cl_reminderLast)))
                    ).ToList();
                MailingUtilities.CURRENT_MAILING.countTotal = _list.Count;
                MailingUtilities.CURRENT_MAILING.countCurrent = 0;
                int mailsSent = 0;
                int mailsNotSent = 0;
                bool alternateOld = true;
                string _mailBody = "";
                _mailBody += MailingUtilities.addMailRow("Sono state inviate " + MailingUtilities.CURRENT_MAILING.countTotal + " mail di notifica", "<br/>di seguito elenco delle pren.", alternateOld, out alternateOld, true, false, true);
                foreach (RNT_TBL_RESERVATION _res in _list)
                {
                    MailingUtilities.CURRENT_MAILING.countCurrent++;
                    rntUtils.rntReservation_mailReminder(_res); // send mails
                    mailsSent++;
                    _res.cl_reminderCount = _res.cl_reminderCount.HasValue ? _res.cl_reminderCount.Value + 1 : 1;
                    _res.cl_reminderLast = DateTime.Now;
                    DC_RENTAL.SubmitChanges();
                    string _body = "codice Pren: #" + _res.code;
                    _body += "<br/>nome cl: " + _res.cl_name_honorific + " " + _res.cl_name_full;
                    _body += "<br/>checkin:" + _res.dtStart.formatCustom("#dd# #MM# #yy#", 1, "");
                    _body += "<br/>checkout:" + _res.dtEnd.formatCustom("#dd# #MM# #yy#", 1, "");
                    _body += "<br/>numero delle reminder inviate: " + _res.cl_reminderCount;
                    _mailBody += MailingUtilities.addMailRow("", _body, alternateOld, out alternateOld, false, false, true);
                }
                _mailBody += MailingUtilities.addMailRow("", "", alternateOld, out alternateOld, false, true, true);
                _mailBody += MailingUtilities.addMailRow("totale inviate:" + mailsSent, "<br/>non inviate:" + mailsNotSent, alternateOld, out alternateOld, false, true, true);
                string _to = CommonUtilities.getSYS_SETTING("cl_reminder_messagesReceiver");
                bool _bcc = CommonUtilities.getSYS_SETTING("cl_reminder_messagesReceiverBCC") == "true";
                string _subject = "RiR: invio reminder ai clienti del " + DateTime.Now.formatCustom("#dd# #MM# #yy#", 1, "") + " ore " + DateTime.Now.TimeOfDay.JSTime_toString(false, true);
                bool _ok = MailingUtilities.autoSendMailTo(_subject, _mailBody, _to, _bcc, "admin_mailClients notifica al admin");
                MailingUtilities.CURRENT_MAILING.countTotal = 0;
                MailingUtilities.CURRENT_MAILING.countCurrent = 0;
                MailingUtilities.CURRENT_MAILING.lastSend = DateTime.Now;
                MailingUtilities.CURRENT_MAILING.isStarted = false;
                CommonUtilities.setSYS_SETTING("cl_reminder_checkLastOccurred", DateTime.Now.JSCal_dateTimeToString());
            }
            catch(Exception exc)
            {
                MailingUtilities.CURRENT_MAILING.countTotal = 0;
                MailingUtilities.CURRENT_MAILING.countCurrent = 0;
                MailingUtilities.CURRENT_MAILING.isStarted = false;
                string _ip = "";
                try { _ip = HttpContext.Current.Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                ErrorLog.addLog(_ip, "mailClients", exc.ToString());
            }
        }
    }
}
