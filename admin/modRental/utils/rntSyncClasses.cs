using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentalInRome.data;
using System.Net;
using System.IO;
using System.Threading;
using ModRental;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

public class rntSyncUtils
{
    public static void SetTimers()
    {
        timerSendMailForComments = new System.Timers.Timer();
        timerSendMailForComments.Interval = (1000 * 60 * 2); // first after 2 mins
        timerSendMailForComments.Elapsed += new System.Timers.ElapsedEventHandler(timerSendMailForComments_Elapsed);
        timerSendMailForComments.Start();

        timerICalImportReservations = new System.Timers.Timer();
        timerICalImportReservations.Interval = (1000 * 60 * 2); // first after 3 mins
        timerICalImportReservations.Elapsed += new System.Timers.ElapsedEventHandler(timerICalImportReservations_Elapsed);
        timerICalImportReservations.Start();

        if (CommonUtilities.getSYS_SETTING("is_clear_log") == "true" || CommonUtilities.getSYS_SETTING("is_clear_log") == "1")
        {
            timerClearIcalImportLog = new System.Timers.Timer();
            timerClearIcalImportLog.Interval = (1000 * 60 * 30); // first after 30 mins
            timerClearIcalImportLog.Elapsed += new System.Timers.ElapsedEventHandler(timerClearIcalImportLog_Elapsed);
            timerClearIcalImportLog.Start();
        }
    }
    public static System.Timers.Timer timerSendMailForComments;
    static void timerSendMailForComments_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        if (RentalInRome.Global.isOnDev) return;
        rntSyncClasses.SendMailForComments_start();
        timerSendMailForComments.Dispose();
        timerSendMailForComments = new System.Timers.Timer();
        timerSendMailForComments.Interval = (1000 * 60 * 60 * 2); // each 2 hours
        timerSendMailForComments.Elapsed += new System.Timers.ElapsedEventHandler(timerSendMailForComments_Elapsed);
        timerSendMailForComments.Start();
    }
    public static System.Timers.Timer timerICalImportReservations;
    static void timerICalImportReservations_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        if (RentalInRome.Global.isOnDev) return;
        int rntICalImportReservationsCheckEachMins = CommonUtilities.getSYS_SETTING("rntICalImportReservationsCheckEachMins").ToInt32();
        if (rntICalImportReservationsCheckEachMins == 0) rntICalImportReservationsCheckEachMins = 60;
        rntSyncClasses.ICalImportReservations_start();
        timerICalImportReservations.Dispose();
        timerICalImportReservations = new System.Timers.Timer();
        timerICalImportReservations.Interval = (1000 * 60 * rntICalImportReservationsCheckEachMins); // each X mins
        timerICalImportReservations.Elapsed += new System.Timers.ElapsedEventHandler(timerICalImportReservations_Elapsed);
        timerICalImportReservations.Start();
    }
    public static System.Timers.Timer timerClearIcalImportLog;
    static void timerClearIcalImportLog_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        rntSyncClasses.ClearIcalImportLog_start(); // only once
    }
}
public class rntSyncClasses
{
    private static MailingUtilities.MassMailing SendMailForComments_mailing;
    private class SendMailForComments_process
    {
        void doThread()
        {
            if (SendMailForComments_mailing == null)
            {
                SendMailForComments_mailing = new MailingUtilities.MassMailing();
            }
            //ErrorLog.addLog("", "rntSyncClasses.test", "");
            SendMailForComments_mailing.lastSend = CommonUtilities.getSYS_SETTING("cl_reminder_checkLastOccurred").JSCal_stringToDateTime();
            if (SendMailForComments_mailing.isStarted) return;
            SendMailForComments_mailing.isStarted = true;
            try
            {
                var DC_RENTAL = maga_DataContext.DC_RENTAL;
                int rntSendMailForCommentsAfterHours = CommonUtilities.getSYS_SETTING("rntSendMailForCommentsAfterHours").ToInt32();
                int rntSendMailForCommentsMaxHours = CommonUtilities.getSYS_SETTING("rntSendMailForCommentsMaxHours").ToInt32();
                if (rntSendMailForCommentsAfterHours == 0) return;
                if (rntSendMailForCommentsMaxHours == 0) rntSendMailForCommentsMaxHours = (24 * 30);
                DateTime dtCheckOut = DateTime.Now.AddHours(-rntSendMailForCommentsAfterHours);
                DateTime dtCheckOutMax = DateTime.Now.AddHours(-rntSendMailForCommentsMaxHours);
                List<long> excludeAlreadySent = DC_RENTAL.RNT_RL_RESERVATION_STATEs.Where(x => x.pid_state == 10).Select(x => x.pid_reservation.objToInt64()).Distinct().ToList();

                List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.dtEnd.HasValue && x.dtEnd < dtCheckOut && x.dtEnd > dtCheckOutMax && x.state_pid == 4 && x.id > 150000).ToList();
                _list = _list.Where(x => !excludeAlreadySent.Contains(x.id)).ToList();
                SendMailForComments_mailing.countTotal = _list.Count;
                SendMailForComments_mailing.countCurrent = 0;
                int mailsSent = 0;
                int mailsNotSent = 0;
                bool alternateOld = true;
                string _mailBody = "";
                _mailBody += MailingUtilities.addMailRow("Sono state inviate " + SendMailForComments_mailing.countTotal + " mail di notifica", "<br/>di seguito elenco delle pren.", alternateOld, out alternateOld, true, false, true);
                foreach (RNT_TBL_RESERVATION _res in _list)
                {
                    if (_res.cl_email + "" == "") continue;
                    SendMailForComments_mailing.countCurrent++;
                    rntUtils.rntReservationMailForComments(_res, 1); // send mails
                    mailsSent++;
                    string _body = "codice Pren: #" + _res.code;
                    _body += "<br/>nome cl: " + _res.cl_name_honorific + " " + _res.cl_name_full;
                    _body += "<br/>checkin:" + _res.dtStart.formatCustom("#dd# #MM# #yy#", 1, "");
                    _body += "<br/>checkout:" + _res.dtEnd.formatCustom("#dd# #MM# #yy#", 1, "");
                    _mailBody += MailingUtilities.addMailRow("", _body, alternateOld, out alternateOld, false, false, true);
                }
                _mailBody += MailingUtilities.addMailRow("", "", alternateOld, out alternateOld, false, true, true);
                _mailBody += MailingUtilities.addMailRow("totale inviate:" + mailsSent, "<br/>non inviate:" + mailsNotSent, alternateOld, out alternateOld, false, true, true);
                string _to = CommonUtilities.getSYS_SETTING("cl_reminder_messagesReceiver");
                bool _bcc = CommonUtilities.getSYS_SETTING("cl_reminder_messagesReceiverBCC") == "true";
                string _subject = "Invio reminder ai clienti del " + DateTime.Now.formatCustom("#dd# #MM# #yy#", 1, "") + " ore " + DateTime.Now.TimeOfDay.JSTime_toString(false, true);
                bool _ok = MailingUtilities.autoSendMailTo(_subject, _mailBody, _to, _bcc, "rntSyncClasses.SendMailForComments_start notifica al admin");
                SendMailForComments_mailing.countTotal = 0;
                SendMailForComments_mailing.countCurrent = 0;
                SendMailForComments_mailing.lastSend = DateTime.Now;
                SendMailForComments_mailing.isStarted = false;
            }
            catch (Exception exc)
            {
                SendMailForComments_mailing.countTotal = 0;
                SendMailForComments_mailing.countCurrent = 0;
                SendMailForComments_mailing.isStarted = false;
                ErrorLog.addLog("", "rntSyncClasses.SendMailForComments_start", exc.ToString());
            }

        }
        public SendMailForComments_process()
        {
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntSyncClasses.SendMailForComments_process");

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static void SendMailForComments_start()
    {
        SendMailForComments_process _tmp = new SendMailForComments_process();
    }

    private static MailingUtilities.MassMailing ICalImportReservations_mailing;
    private class ICalImportReservations_process
    {
        private void DownloadAndImport(int estateId, string channelManager, string iCalUrl)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(iCalUrl);
            try
            {
                if (iCalUrl.Contains(".housetrip.") || iCalUrl.Contains(".airbnb.") || iCalUrl.Contains(".tripadvisor."))
                {
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    request.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
                    //request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    request.Headers.Add("Accept-Language", "en-GB,en;q=0.5");

                    if (iCalUrl.Contains(".airbnb."))
                        request.ContentType = "application/x-www-form-urlencoded";
                    else
                        request.Headers.Add("Accept-Encoding", "gzip, deflate");
                }
                else
                {
                    request.Method = "GET";
                    if (iCalUrl.Contains(".homeaway."))
                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    else
                        request.ContentType = "application/x-www-form-urlencoded";
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string icalData = "";
                if (response.StatusCode.ToString().ToLower() == "ok")
                {
                    Stream content = response.GetResponseStream();
                    StreamReader contentReader = new StreamReader(content);
                    icalData = contentReader.ReadToEnd();
                }
                if (icalData == "")
                {
                    return; // TODO: log
                }

                if (!Directory.Exists(Path.Combine(App.SRP, "files"))) Directory.CreateDirectory(Path.Combine(App.SRP, "files"));
                if (!Directory.Exists(Path.Combine(App.SRP, "files/tmp"))) Directory.CreateDirectory(Path.Combine(App.SRP, "files/tmp"));
                string filePath = "files/tmp/" + string.Empty.createUniqueID() + ".ics";
                StreamWriter ccWriter = new StreamWriter(Path.Combine(App.SRP, filePath), true);
                ccWriter.WriteLine(icalData); // Write the file.
                ccWriter.Flush();
                ccWriter.Close(); // Close the instance of StreamWriter.
                ccWriter.Dispose(); // Dispose from memory.

                if (!File.Exists(Path.Combine(App.SRP, filePath)))
                {
                    return; // TODO: log
                }
                rntImportFromiCal xx = new rntImportFromiCal(estateId, Path.Combine(App.SRP, filePath), iCalUrl, channelManager);
                xx.StartImport();
                var log = xx.ErrorDates; // TODO: log
                try
                {
                    File.Delete(Path.Combine(App.SRP, filePath));
                }
                catch (Exception exc1)
                { }
            }
            catch (Exception exc)
            {
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var CurrLog = new RntIcalImportLOG();
                    CurrLog.uid = Guid.NewGuid();
                    CurrLog.logDateTime = DateTime.Now;
                    CurrLog.pidEstate = estateId;
                    CurrLog.iCalType = channelManager;
                    CurrLog.iCalUrl = iCalUrl;
                    CurrLog.errorCount = 9999;
                    dc.RntIcalImportLOG.InsertOnSubmit(CurrLog);
                    dc.SubmitChanges();
                    var log = new RntIcalImportErrorLOG();
                    log.uid = Guid.NewGuid();
                    log.iCalDtStart = DateTime.Now;
                    log.logDateTime = DateTime.Now;
                    log.iCalComment = exc.ToString();
                    log.logUid = CurrLog.uid;
                    dc.RntIcalImportErrorLOG.InsertOnSubmit(log);
                    dc.SubmitChanges();
                }
                //TextWriter wr = new StringWriter();
                //JsonSerializer serializer = new JsonSerializer();
                //serializer.NullValueHandling = NullValueHandling.Ignore;
                //serializer.Serialize(wr, request);
                //string requestContent = wr.ToString();

                //ErrorLog.addLog("", "rntSyncClasses.ICalImportReservations_start estateId:" + estateId + " channelManager:" + channelManager + "<br/>iCalUrl:" + iCalUrl, requestContent);
            }
        }
        void doThread()
        {
            if (ICalImportReservations_mailing == null)
            {
                ICalImportReservations_mailing = new MailingUtilities.MassMailing();
            }
            //ErrorLog.addLog("", "rntSyncClasses.test", "");

            if (ICalImportReservations_mailing.isStarted) return;
            ICalImportReservations_mailing.isStarted = true;
            try
            {
                var DC_RENTAL = maga_DataContext.DC_RENTAL;
                using (DCmodRental dc = new DCmodRental())
                {
                    var channelManagerIds = dc.dbRntChannelManagerTBLs.Where(x => x.isActive == 1).Select(x => x.code).ToList();
                    var tmpList = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted == 0).ToList();
                    foreach (var tmp in tmpList)
                    {
                        if (tmp.iCalUrl != null && tmp.iCalUrl != "")
                            DownloadAndImport(tmp.id, "", tmp.iCalUrl);
                        var rlList = dc.dbRntChannelManagerEstateRLs.Where(x => x.pidEstate == tmp.id && x.iCalImportEnabled == 1 && x.iCalImportUrl != null && x.iCalImportUrl != "" && channelManagerIds.Contains(x.pidChannelManager)).ToList();
                        foreach (var rl in rlList)
                        {
                            DownloadAndImport(tmp.id, "_" + rl.pidChannelManager, rl.iCalImportUrl);
                        }
                    }
                }
                ICalImportReservations_mailing.countTotal = 0;
                ICalImportReservations_mailing.countCurrent = 0;
                ICalImportReservations_mailing.lastSend = DateTime.Now;
                ICalImportReservations_mailing.isStarted = false;
            }
            catch (Exception exc)
            {
                ICalImportReservations_mailing.countTotal = 0;
                ICalImportReservations_mailing.countCurrent = 0;
                ICalImportReservations_mailing.isStarted = false;
                ErrorLog.addLog("", "rntSyncClasses.ICalImportReservations_start", exc.ToString());
            }
        }
        public ICalImportReservations_process()
        {
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntSyncClasses.ICalImportReservations_process");

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }
    }
    public static void ICalImportReservations_start()
    {
        ICalImportReservations_process _tmp = new ICalImportReservations_process();
    }
    private class ClearIcalImportLog_process
    {
        void doThread()
        {
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                dc.RntIcalImportErrorLOG.DeleteAllOnSubmit(dc.RntIcalImportErrorLOG.Where(x => x.logDateTime <= DateTime.Now.AddDays(-30)));
                dc.SubmitChanges();
                dc.RntIcalImportLOG.DeleteAllOnSubmit(dc.RntIcalImportLOG.Where(x => x.logDateTime <= DateTime.Now.AddDays(-30)));
                dc.SubmitChanges();
            }
        }
        public ClearIcalImportLog_process()
        {
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntSyncClasses.ClearIcalImportLog_process");
            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static void ClearIcalImportLog_start()
    {
        ClearIcalImportLog_process _tmp = new ClearIcalImportLog_process();
    }
}
