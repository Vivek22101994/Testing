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
    public partial class checkReservationExpire : System.Web.UI.Page
    {
        protected magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Url.AbsoluteUri.Contains("http://localhost")) return;
                int rnt_reservationExpire_checkEachHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_checkEachHours").ToInt32();
                if (rnt_reservationExpire_checkEachHours == 0)
                    rnt_reservationExpire_checkEachHours = 3;
                if (rntUtils.CURRENT_RESERVATION_EXPIRE == null)
                {
                    rntUtils.CURRENT_RESERVATION_EXPIRE = new rntUtils.ReservationExpire();
                    AppSettings.DEF_SYS_SETTINGs = null;
                }
                rntUtils.CURRENT_RESERVATION_EXPIRE.lastSend = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_checkLastOccurred").JSCal_stringToDateTime();
                if (!rntUtils.CURRENT_RESERVATION_EXPIRE.isStarted
                    && rntUtils.CURRENT_RESERVATION_EXPIRE.lastSend.Value.AddHours(rnt_reservationExpire_checkEachHours) < DateTime.Now)
                {
                    startThread();
                }
                rntUtils.ReservationExpire _current = rntUtils.CURRENT_RESERVATION_EXPIRE;
                Response.Write(_current.isStarted ? "1" : "0");
                Response.Write("|" + _current.countTotal + "|" + _current.countCurrent + "|" + _current.description);
            }
        }
        protected void startThread()
        {
           rntUtils.CURRENT_RESERVATION_EXPIRE.isStarted = true;
           //Action<object> action = (object obj) => { check_reservationExpire(); };
           //AppUtilsTaskScheduler.AddTask(action, "checkReservationExpire");
           ThreadStart start = new ThreadStart(check_reservationExpire);
           Thread t = new Thread(start);
           t.Priority = ThreadPriority.Lowest;
           t.Start();
            
        }
        protected void check_reservationExpire()
        {
            try
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                bool canSend = CommonUtilities.getSYS_SETTING("cl_reminder_canSend") == "true";
                List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.dtEnd.HasValue && x.dtStart.HasValue && x.state_pid == 6 && x.block_expire < DateTime.Now).ToList();
                rntUtils.CURRENT_RESERVATION_EXPIRE.countTotal = _list.Count;
                rntUtils.CURRENT_RESERVATION_EXPIRE.countCurrent = 0;
                int mailsSent = 0;
                int mailsNotSent = 0;
                bool alternateOld = true;
                string _mailBody = "";
                _mailBody += MailingUtilities.addMailRow("Sono state cancellate " + rntUtils.CURRENT_RESERVATION_EXPIRE.countTotal + " prenotazioni da confermare", "<br/>di seguito elenco delle pren.", alternateOld, out alternateOld, true, false, true);
                foreach (RNT_TBL_RESERVATION _currTBL in _list)
                {
                    // todo invia mail al cl
                    rntUtils.CURRENT_RESERVATION_EXPIRE.countCurrent++;
                    rntUtils.rntReservation_onStateChange(_currTBL);
                    _currTBL.state_pid = 3;
                    _currTBL.state_body = "Cancellato automaticamente dal sistema";
                    _currTBL.state_date = DateTime.Now;
                    _currTBL.state_pid_user = 1;
                    _currTBL.state_subject = "CAN";
                    DC_RENTAL.SubmitChanges();
                    rntUtils.rntReservation_onChange(_currTBL);
                    string _body = "codice Pren: #" + _currTBL.code;
                    MailingUtilities.CURRENT_MAILING.countCurrent++;
                    if (canSend || _currTBL.id > 150000)
                    {
                        rntUtils.rntReservation_mailExpired(_currTBL); // send mails
                        mailsSent++;
                    }
                    else
                    {
                        _body += "<br/>NON INVIATO! Motivo: non siamo online";
                        mailsNotSent++;
                    }
                    _body += "<br/>nome cl: " + _currTBL.cl_name_honorific + " " + _currTBL.cl_name_full;
                    _body += "<br/>checkin:" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", 1, "");
                    _body += "<br/>checkout:" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", 1, "");
                    _mailBody += MailingUtilities.addMailRow("", _body, alternateOld, out alternateOld, false, false, true);
                }
                _mailBody += MailingUtilities.addMailRow("", "", alternateOld, out alternateOld, false, true, true);
                string _to = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_messagesReceiver");
                bool _bcc = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_messagesReceiverBCC") == "true";
                string _subject = "RiR: prenotazioni cancellati automaticamente del " + DateTime.Now.formatCustom("#dd# #MM# #yy#", 1, "") + " ore " + DateTime.Now.TimeOfDay.JSTime_toString(false, true);
                bool _ok = MailingUtilities.autoSendMailTo(_subject, _mailBody, _to, _bcc, "admin_checkReservationExpire notifica al admin");
                rntUtils.CURRENT_RESERVATION_EXPIRE.countTotal = 0;
                rntUtils.CURRENT_RESERVATION_EXPIRE.countCurrent = 0;
                rntUtils.CURRENT_RESERVATION_EXPIRE.lastSend = DateTime.Now;
                rntUtils.CURRENT_RESERVATION_EXPIRE.isStarted = false;
                CommonUtilities.setSYS_SETTING("rnt_reservationExpire_checkLastOccurred", DateTime.Now.JSCal_dateTimeToString());
            }
            catch (Exception exc)
            {
                rntUtils.CURRENT_RESERVATION_EXPIRE.countTotal = 0;
                rntUtils.CURRENT_RESERVATION_EXPIRE.countCurrent = 0;
                rntUtils.CURRENT_RESERVATION_EXPIRE.isStarted = false;
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                ErrorLog.addLog(_ip, "checkReservationExpire", exc.ToString());
            }
        }
    }
}
