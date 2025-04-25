using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net.Mail;
using System.Web;
using RentalInRome.data;
using MailBee;
using MailBee.SmtpMail;
using MailBee.Security;
using System.Threading;

public class mailUtils
{
    public static MailLogList _list;
    public static void __addLog(string ip, string message, string _Subject, string _Body, List<string> _TOs, List<string> _CCs, List<AttachmentItem> _Attachments, bool _bcc, int _SMTP_PORT, string _SMTP_HOST, bool _SMTP_SSL, string _SMTP_USR, string _SMTP_PWD, string _FROM_MAIL, string _FROM_NAME, string _mailDescrtiption)
    {
        try
        {
            using (ModAuth.DCmodAuth dc = new ModAuth.DCmodAuth())
            {
                var item = new ModAuth.dbAuthMailErrorLOG();
                item.uid = Guid.NewGuid();
                item.logIp = ip;
                item.logDateTime = DateTime.Now;
                item.errorContent = message;
                item.mailFromEmail = _FROM_MAIL;
                item.mailFromName = _FROM_NAME;
                item.mailSubject = _Subject;
                item.mailBody = _Body;
                item.mailTOs = _TOs.listToString(";");
                item.mailCCs = _CCs.listToString(";");
                item.mailAttachments = _Attachments.Select(x => x.filePath).ToList().listToString(";");
                item.mailBcc = _bcc;
                item.mailDetails = "";
                item.mailDetails += "\n_SMTP_PORT=" + _SMTP_PORT;
                item.mailDetails += "\n_SMTP_HOST=" + _SMTP_HOST;
                item.mailDetails += "\n_SMTP_SSL=" + _SMTP_SSL;
                item.mailDetails += "\n_SMTP_USR=" + _SMTP_USR;
                item.mailDetails += "\n_SMTP_PWD=" + _SMTP_PWD;
                item.mailDescrtiption = _mailDescrtiption;
                item.mailIsResent = 0;
                dc.Add(item);
                dc.SaveChanges();
            }
        }
        catch (Exception Ex)
        {
            ErrorLog.addLog(ip, "mailUtils.addLog", Ex.ToString());
            //addLogOld(ip, message, _Subject, _Body, _TOs, _CCs, _Attachments, _bcc, _SMTP_PORT, _SMTP_HOST, _SMTP_SSL, _SMTP_USR, _SMTP_PWD, _FROM_MAIL, _FROM_NAME, _mailDescrtiption);
        }
    }
    public static void addLog(string ip, string message, string _Subject, string _Body, List<string> _TOs, List<string> _CCs, List<AttachmentItem> _Attachments, bool _bcc, int _SMTP_PORT, string _SMTP_HOST, bool _SMTP_SSL, string _SMTP_USR, string _SMTP_PWD, string _FROM_MAIL, string _FROM_NAME, string _mailDescrtiption)
    {
        string _fileName = DateTime.Now.JSCal_dateToString() + ".xml";
        if (_list == null)
            _list = new MailLogList();
        if (_list._fileName != _fileName)
            _list = new MailLogList();
        MailLogItem _item = new MailLogItem(ip, message, DateTime.Now.JSCal_dateTimeToString(), _Subject, _Body, _TOs, _CCs, _Attachments, _bcc, _SMTP_PORT, _SMTP_HOST, _SMTP_SSL, _SMTP_USR, _SMTP_PWD, _FROM_MAIL, _FROM_NAME, _mailDescrtiption);
        _list.Items.Add(_item);
        _list.Save();
    }
    public class MailLogList
    {
        private string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/mailerrors");
        public string _fileName;
        private List<MailLogItem> _items;

        public List<MailLogItem> Items { get { if (this._items != null)return this._items; else return new List<MailLogItem>(); } set { this._items = value; } }
        private void fillList()
        {
            this._items = new List<MailLogItem>();
            try
            {
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log"))) Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log"));
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/mailerrors"))) Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/mailerrors"));
                if (!File.Exists(Path.Combine(this._path, _fileName))) return;
                XDocument _resource = XDocument.Load(Path.Combine(this._path, _fileName));
                var ds = from XElement e in _resource.Descendants("server")
                         select e;
                foreach (XElement e in ds)
                {
                    MailLogItem item = new MailLogItem();
                    item.ID = e.Element("ID").Value;
                    item.IP = e.Element("IP").Value.htmlDecode();
                    item.Value = e.Element("Value").Value.htmlDecode();
                    item.Date = e.Element("Date").Value.htmlDecode();
                    item.isResent = e.Element("isResent").Value == "1";

                    item.Subject = e.Element("Subject").Value.htmlDecode();
                    item.Body = e.Element("Body").Value.htmlDecode();
                    item.SMTP_HOST = e.Element("SMTP_HOST").Value.htmlDecode();
                    item.SMTP_USR = e.Element("SMTP_USR").Value.htmlDecode();
                    item.SMTP_PWD = e.Element("SMTP_PWD").Value.htmlDecode();
                    item.FROM_MAIL = e.Element("FROM_MAIL").Value.htmlDecode();
                    item.FROM_NAME = e.Element("FROM_NAME").Value.htmlDecode();
                    item.mailDescrtiption = e.Element("mailDescrtiption").Value.htmlDecode();
                    item.SMTP_PORT = e.Element("SMTP_PORT").Value.ToInt32();
                    item.SMTP_SSL = e.Element("SMTP_SSL").Value == "1";
                    item.bcc = e.Element("bcc").Value == "1";

                    List<string> tmpList;

                    tmpList = new List<string>();
                    foreach (XElement tmp in e.Element("TOs").Descendants())
                        tmpList.Add(tmp.Value.htmlDecode());
                    item.TOs = tmpList;

                    tmpList = new List<string>();
                    foreach (XElement tmp in e.Element("CCs").Descendants())
                        tmpList.Add(tmp.Value.htmlDecode());
                    item.CCs = tmpList;

                    var tmpAttachmentItemList = new List<AttachmentItem>();
                    foreach (XElement tmp in e.Element("Attachments").Descendants())
                        tmpAttachmentItemList.Add(new AttachmentItem(tmp.Value.htmlDecode()));
                    item.Attachments = tmpAttachmentItemList;

                    this._items.Add(item);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public MailLogList()
        {
            _fileName = DateTime.Now.JSCal_dateToString() + ".xml";
            fillList();
        }
        public MailLogList(string fileName)
        {
            _fileName = fileName;
            fillList();
        }
        public void Save()
        {
            try
            {
                XElement elmList;
                XDocument _resource = new XDocument();
                XElement rootElement = new XElement("list");
                foreach (MailLogItem item in this._items)
                {
                    XElement record = new XElement("server");
                    record.Add(new XElement("ID", item.ID));
                    record.Add(new XElement("IP", item.IP.htmlEncode()));
                    record.Add(new XElement("Value", item.Value.htmlEncode()));
                    record.Add(new XElement("Date", item.Date.htmlEncode()));
                    record.Add(new XElement("isResent", (item.isResent ? "1" : "0")));

                    record.Add(new XElement("Subject", item.Subject.htmlEncode()));
                    record.Add(new XElement("Body", item.Body.htmlEncode()));
                    record.Add(new XElement("SMTP_HOST", item.SMTP_HOST.htmlEncode()));
                    record.Add(new XElement("SMTP_USR", item.SMTP_USR.htmlEncode()));
                    record.Add(new XElement("SMTP_PWD", item.SMTP_PWD.htmlEncode()));
                    record.Add(new XElement("FROM_MAIL", item.FROM_MAIL.htmlEncode()));
                    record.Add(new XElement("FROM_NAME", item.FROM_NAME.htmlEncode()));
                    record.Add(new XElement("mailDescrtiption", item.mailDescrtiption.htmlEncode()));
                    record.Add(new XElement("SMTP_PORT", item.SMTP_PORT));
                    record.Add(new XElement("SMTP_SSL", (item.SMTP_SSL ? "1" : "0")));
                    record.Add(new XElement("bcc", (item.bcc ? "1" : "0")));

                    elmList = new XElement("TOs");
                    foreach (string tmp in item.TOs)
                        elmList.Add(new XElement("item", tmp));
                    record.Add(elmList);

                    elmList = new XElement("CCs");
                    foreach (string tmp in item.CCs)
                        elmList.Add(new XElement("item", tmp));
                    record.Add(elmList);

                    elmList = new XElement("Attachments");
                    foreach (var tmp in item.Attachments)
                        elmList.Add(new XElement("item", tmp.filePath));
                    record.Add(elmList);

                    rootElement.Add(record);
                }
                _resource.Add(rootElement);
                try
                {
                    _resource.Save(Path.Combine(this._path, _fileName));
                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            {
            }
        }
    }
    public class MailLogItem
    {
        public string ID { get; set; }
        public string IP { get; set; }
        public string Value { get; set; }
        public string Date { get; set; }
        public bool isResent { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public string SMTP_HOST { get; set; }
        public string SMTP_USR { get; set; }
        public string SMTP_PWD { get; set; }
        public string FROM_MAIL { get; set; }
        public string FROM_NAME { get; set; }
        public string mailDescrtiption { get; set; }
        public int SMTP_PORT { get; set; }
        public bool bcc { get; set; }
        public bool SMTP_SSL { get; set; }

        public List<string> TOs { get; set; }
        public List<string> CCs { get; set; }
        public List<AttachmentItem> Attachments { get; set; }

        public MailLogItem()
        {
            ID = Guid.NewGuid().ToString();
            IP = "";
            Value = "";
            Date = DateTime.Now.JSCal_dateTimeToString();
            isResent = false;
        }
        public MailLogItem(string _IP, string _Value, string _Date, string _Subject, string _Body, List<string> _TOs, List<string> _CCs, List<AttachmentItem> _Attachments, bool _bcc, int _SMTP_PORT, string _SMTP_HOST, bool _SMTP_SSL, string _SMTP_USR, string _SMTP_PWD, string _FROM_MAIL, string _FROM_NAME, string _mailDescrtiption)
        {
            ID = Guid.NewGuid().ToString();
            IP = _IP;
            Value = _Value;
            Date = _Date;
            isResent = false;
            Subject = _Subject;
            Body = _Body;
            TOs = _TOs;
            CCs = _CCs;
            Attachments = _Attachments;
            bcc = _bcc;
            SMTP_PORT = _SMTP_PORT;
            SMTP_HOST = _SMTP_HOST;
            SMTP_SSL = _SMTP_SSL;
            SMTP_USR = _SMTP_USR;
            SMTP_PWD = _SMTP_PWD;
            FROM_MAIL = _FROM_MAIL;
            FROM_NAME = _FROM_NAME;
            mailDescrtiption = _mailDescrtiption;
        }
    }
    public class AttachmentItem
    {
        public string filePath { get; set; }
        public string fileName { get; set; }
        public Stream fileStream { get; set; }
        public AttachmentItem(string FilePath)
        {
            filePath = FilePath;
            fileName = null;
            fileStream = null;
        }
        //public AttachmentItem(Stream FileStream, string FileName)
        //{
        //    filePath = null;
        //    fileName = FileName;
        //    fileStream = FileStream;
        //}
    }
}
public class MailingUtilities
{
    public class MassMailing
    {
        public bool isStarted { get; set; }
        public int countTotal { get; set; }
        public int countCurrent { get; set; }
        public string description { get; set; }
        public DateTime lastSend { get; set; }
        public MassMailing()
        {
            isStarted = false;
            countTotal = 0;
            countCurrent = 0;
            description = "";
        }
    }

    public static MassMailing CURRENT_MAILING;
    private static List<MAIL_VIEW_TEMPLATE> _MAIL_VIEW_TEMPLATEs; // refresh OK
    public static List<MAIL_VIEW_TEMPLATE> MAIL_VIEW_TEMPLATEs
    {
        get
        {
            if (_MAIL_VIEW_TEMPLATEs == null)
            {
                _MAIL_VIEW_TEMPLATEs = maga_DataContext.DC_MAIL.MAIL_VIEW_TEMPLATEs.ToList();
            }
            return _MAIL_VIEW_TEMPLATEs;
        }
        set { _MAIL_VIEW_TEMPLATEs = value; }
    }
    public static string mailTemplate_subject(string code, int lang, string alternate)
    {
        MAIL_VIEW_TEMPLATE _s = MAIL_VIEW_TEMPLATEs.SingleOrDefault(x => x.code == code && x.pid_lang == lang);
        if (_s == null)
            _s = MAIL_VIEW_TEMPLATEs.SingleOrDefault(x => x.code == code && x.pid_lang == 2);
        if (_s == null)
            _s = MAIL_VIEW_TEMPLATEs.SingleOrDefault(x => x.code == code && x.pid_lang == 1);
        if (_s != null)
            return _s.subject;
        return alternate;
    }
    public static string mailTemplate_body(string code, int lang, string alternate)
    {
        MAIL_VIEW_TEMPLATE _s = MAIL_VIEW_TEMPLATEs.SingleOrDefault(x => x.code == code && x.pid_lang == lang);
        if (_s == null)
            _s = MAIL_VIEW_TEMPLATEs.SingleOrDefault(x => x.code == code && x.pid_lang == 2);
        if (_s == null)
            _s = MAIL_VIEW_TEMPLATEs.SingleOrDefault(x => x.code == code && x.pid_lang == 1);
        if (_s != null)
            return _s.body;
        return alternate;
    }
    public static string addMailRow(string title, string content, bool alternateOld, out bool alternate, bool open, bool close, bool colspan)
    {
        string _row = "";
        if (open)
            _row += "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
        _row += "<tr>";
        if (!colspan)
        {
            if (!alternateOld)
                _row += "<td style=\"vertical-align: top;\"><strong>" + title + "</strong></td><td>" + content + "</td>";
            else
                _row += "<td style=\"vertical-align: top;\" bgcolor=\"#E8E8E8\"><strong>" + title + "</strong></td><td bgcolor=\"#E8E8E8\">" + content + "</td>";
        }
        else
        {
            if (!alternateOld)
                _row += "<td style=\"vertical-align: top;\" colspan=\"2\"><strong>" + title + "</strong>" + content + "</td>";
            else
                _row += "<td style=\"vertical-align: top;\" colspan=\"2\" bgcolor=\"#E8E8E8\"><strong>" + title + "</strong>" + content + "</td>";
        }
        _row += "</tr>";
        if (close)
            _row += "</table>";
        alternate = !alternateOld;
        return _row;
    }
    public static string ADMIN_MAIL
    {
        get
        {
            string _str = CommonUtilities.getSYS_SETTING("mailing_admin_mail");
            if (_str != "") return _str;
            return "adilet.nas@gmail.com";
        }
    }
    public static string BCC_MAIL
    {
        get
        {
            string _str = CommonUtilities.getSYS_SETTING("mailing_bcc_mail");
            if (_str != "") return _str;
            return "adilet@magadesign.net";
        }
    }
    public static string FROM_MAIL
    {
        get
        {
            string _str = CommonUtilities.getSYS_SETTING("mailing_from_mail");
            if (_str != "") return _str;
            return "magadesign.mailing@gmail.com";
        }
    }
    public static string FROM_NAME
    {
        get
        {
            string _str = CommonUtilities.getSYS_SETTING("mailing_from_name");
            if (_str != "") return _str;
            return "MagaDesign Mailing";
        }
    }
    /*
     * default smtp
     * 
    <add key="mailing_smtp_ssl" value="true"/>
    <add key="mailing_smtp_usr" value="magadesign.mailing@gmail.com"/>
    <add key="mailing_smtp_pwd" value="magamailing2010"/>
    <add key="mailing_smtp_host" value="smtp.gmail.com"/>
    <add key="mailing_smtp_port" value="587"/>
     * 
     * 
     * alternate smtp
     * 
    <add key="mailing_smtp_ssl" value="false"/>
    <add key="mailing_smtp_usr" value="info@magadesign.net"/>
    <add key="mailing_smtp_pwd" value="1202011966"/>
    <add key="mailing_smtp_host" value="smtp.magadesign.net"/>
    <add key="mailing_smtp_port" value="25"/>
    */
    public static string getMailTemplate_Subject(int id, int pid_lang)
    {
        //VIEW_TBL_MAIL_TEMPLATE _s = maga_DataContext.DC_CONTENT.VIEW_TBL_MAIL_TEMPLATEs.SingleOrDefault(x => x.id == id && x.pid_lang == pid_lang);
        //if (_s == null || _s.subject == null || _s.subject.Trim() == "")
        //    _s = maga_DataContext.DC_CONTENT.VIEW_TBL_MAIL_TEMPLATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        //if (_s == null || _s.subject == null || _s.subject.Trim() == "")
        //    _s = maga_DataContext.DC_CONTENT.VIEW_TBL_MAIL_TEMPLATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        //if (_s != null)
        //    return _s.subject;
        return "";
    }
    public static string getMailTemplate_MailBody(int id, int pid_lang)
    {
        //VIEW_TBL_MAIL_TEMPLATE _s = maga_DataContext.DC_CONTENT.VIEW_TBL_MAIL_TEMPLATEs.SingleOrDefault(x => x.id == id && x.pid_lang == pid_lang);
        //if (_s == null || _s.mail_body == null || _s.mail_body.Trim() == "")
        //    _s = maga_DataContext.DC_CONTENT.VIEW_TBL_MAIL_TEMPLATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        //if (_s == null || _s.mail_body == null || _s.mail_body.Trim() == "")
        //    _s = maga_DataContext.DC_CONTENT.VIEW_TBL_MAIL_TEMPLATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        //if (_s != null)
        //    return _s.mail_body;
        return "";
    }
    private class SendMailToManyWork
    {
        void doThread()
        {
            SendMailToMany(_Subject, _Body, _TOs, _Attachments, _bcc, _FromMail, _FromName, _mailDescrtiption, false);
        }
        string _Subject;
        string _Body;
        List<string> _TOs;
        List<string> _Attachments;
        bool _bcc;
        string _FromMail;
        string _FromName;
        string _mailDescrtiption;
        public SendMailToManyWork(string Subject, string Body, List<string> TOs, List<string> Attachments, bool bcc, string FromMail, string FromName, string mailDescrtiption)
        {
            _Subject = Subject;
            _Body = Body;
            _TOs = TOs;
            _Attachments = Attachments;
            _bcc = bcc;
            _FromMail = FromMail;
            _FromName = FromName;
            _mailDescrtiption = mailDescrtiption;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "SendMailToManyWork");
            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static bool SendMultiMail(string Subject, string Body, List<string> TOs, List<string> CCs, bool bcc, string mailDescrtiption)
    {
        return customSendMailTo(Subject, Body, TOs, CCs, new List<mailUtils.AttachmentItem>(), bcc, FROM_MAIL, FROM_NAME, mailDescrtiption);
    }
    public static bool SendMultiMail(string Subject, string Body, List<string> TOs, List<string> CCs, List<mailUtils.AttachmentItem> Attachments, bool bcc, string mailDescrtiption)
    {
        return customSendMailTo(Subject, Body, TOs, CCs, Attachments, bcc, FROM_MAIL, FROM_NAME, mailDescrtiption);
    }
    public static bool SendMultiMail(string Subject, string Body, List<string> TOs, List<string> CCs, List<mailUtils.AttachmentItem> Attachments, bool bcc, string FromMail, string FromName, string mailDescrtiption)
    {
        return customSendMailTo(Subject, Body, TOs, CCs, Attachments, bcc, FromMail, FromName, mailDescrtiption);
    }
    public static void SendMailToMany(string Subject, string Body, List<string> TOs, List<string> Attachments, bool bcc, string FromMail, string FromName, string mailDescrtiption, bool DoThread)
    {
        if (DoThread)
        {
            var tmp = new SendMailToManyWork(Subject, Body, TOs, Attachments, bcc, FromMail, FromName, mailDescrtiption);
            return;
        }
        foreach (string To in TOs)
            customSendMailTo(Subject, Body, new List<string>() { To }, new List<string>(), new List<mailUtils.AttachmentItem>(), bcc, FromMail, FromName, mailDescrtiption);
    }
    public static bool autoSendMailTo(string Subject, string Body, string To, bool bcc, string mailDescrtiption)
    {
        return customSendMailTo(Subject, Body, new List<string>() { To }, new List<string>(), new List<mailUtils.AttachmentItem>(), bcc, FROM_MAIL, FROM_NAME, mailDescrtiption);
    }
    public static bool autoSendMailTo(string Subject, string Body, string To, List<mailUtils.AttachmentItem> Attachments, bool bcc, string mailDescrtiption)
    {
        return customSendMailTo(Subject, Body, new List<string>() { To }, new List<string>(), Attachments, bcc, FROM_MAIL, FROM_NAME, mailDescrtiption);
    }
    public static bool autoSendMultiMail(string Subject, string Body, List<string> Tos, bool bcc, string mailDescrtiption)
    {
        return customSendMailTo(Subject, Body, Tos, new List<string>(), new List<mailUtils.AttachmentItem>(), bcc, FROM_MAIL, FROM_NAME, mailDescrtiption);
    }

    public static bool autoSendMailTo_from(string Subject, string Body, string To, bool bcc, string _FROM_MAIL, string _FROM_NAME, string mailDescrtiption)
    {
        return customSendMailTo(Subject, Body, new List<string>() { To }, new List<string>(), new List<mailUtils.AttachmentItem>(), bcc, _FROM_MAIL, _FROM_NAME, mailDescrtiption);
    }
    public static bool mannualSendMailTo_From(string Subject, string Body, string To, bool bcc, string _FROM_MAIL, string _FROM_NAME, string mailDescrtiption, string _uniqueResID)
    {
        // add on 2015-08-31
        return mannualSendMailTo(Subject, Body, new List<string>() { To }, new List<string>(), new List<mailUtils.AttachmentItem>(), bcc, _FROM_MAIL, _FROM_NAME, mailDescrtiption, _uniqueResID);
    }
    public static bool mannualSendMailTo(string Subject, string Body, List<string> TOs, List<string> CCs, List<mailUtils.AttachmentItem> Attachments, bool bcc, string _FROM_MAIL, string _FROM_NAME, string mailDescrtiption, string uniqueResID)
    {
        var currSmtpConfig = ModAppServerCommon.utils.getSmtpConfig();
        try
        {
            string bodyHeader = "<Html><body>";
            string bodyFooter = "</body></html>";
            SmtpClient smtp = new SmtpClient();
            smtp.Port = currSmtpConfig.SmtpPort;
            smtp.Host = currSmtpConfig.SmtpHost;
            smtp.EnableSsl = currSmtpConfig.SmtpSsl;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (currSmtpConfig.SmtpAuth && currSmtpConfig.SmtpUsr != "" && currSmtpConfig.SmtpPwd != "")
                smtp.Credentials = new System.Net.NetworkCredential(currSmtpConfig.SmtpUsr, currSmtpConfig.SmtpPwd);
            MailMessage mail = new MailMessage();
            //if (CommonUtilities.getSYS_SETTING("is_email_test") == "true")
            //{
            //    mail.To.Add(new MailAddress(CommonUtilities.getSYS_SETTING("email_test")));
            //}
            //else
            //{
            //    foreach (string _to in TOs)
            //        if (_to.Trim() != "")
            //            mail.To.Add(new MailAddress(_to));
            //    foreach (string _to in CCs)
            //        if (_to.Trim() != "")
            //            mail.To.Add(new MailAddress(_to));
            //    if (BCC_MAIL.isEmail() && bcc)
            //        mail.Bcc.Add(BCC_MAIL);
            //}

            foreach (string _to in TOs)
                if (_to.Trim() != "")
                    mail.To.Add(new MailAddress(_to));
            foreach (string _to in CCs)
                if (_to.Trim() != "")
                    mail.To.Add(new MailAddress(_to));
            if (BCC_MAIL.isEmail() && bcc)
                mail.Bcc.Add(BCC_MAIL);

            mail.From = new MailAddress(_FROM_MAIL, _FROM_NAME);
            mail.IsBodyHtml = true;
            string body = bodyHeader + Body + bodyFooter;
            mail.Subject = Subject;
            mail.Body = body;

            //mailUtils.addLog("0", "success1", Subject, Body, TOs, CCs, Attachments, bcc, currSmtpConfig.SmtpPort, currSmtpConfig.SmtpHost, currSmtpConfig.SmtpSsl, currSmtpConfig.SmtpUsr, currSmtpConfig.SmtpPwd, _FROM_MAIL, _FROM_NAME, mailDescrtiption);
            foreach (var _Attachment in Attachments)
                if (_Attachment.filePath != null && _Attachment.filePath.Trim() != "")
                    mail.Attachments.Add(new Attachment(_Attachment.filePath.Trim()));
            //else if (_Attachment.fileStream != null && _Attachment.fileStream != Stream.Null && _Attachment.fileName != null)
            //    mail.Attachments.Add(new Attachment(_Attachment.fileStream, _Attachment.fileName, "application/pdf"));
            smtp.Timeout = 300000;
            smtp.Send(mail);
            //mailUtils.addLog("0","success" , Subject, Body, TOs, CCs, Attachments, bcc, currSmtpConfig.SmtpPort, currSmtpConfig.SmtpHost, currSmtpConfig.SmtpSsl, currSmtpConfig.SmtpUsr, currSmtpConfig.SmtpPwd, _FROM_MAIL, _FROM_NAME, mailDescrtiption);
            return true;
        }
        catch (Exception ex)
        {
            string _ip = "";
            try { _ip = HttpContext.Current.Request.ServerVariables.Get("REMOTE_HOST"); }
            catch (Exception ex1) { }
            mailUtils.addLog(_ip, ex.ToString(), Subject, Body, TOs, CCs, Attachments, bcc, currSmtpConfig.SmtpPort, currSmtpConfig.SmtpHost, currSmtpConfig.SmtpSsl, currSmtpConfig.SmtpUsr, currSmtpConfig.SmtpPwd, _FROM_MAIL, _FROM_NAME, mailDescrtiption);
            return false;
        }
    }

    public static bool customSendMailTo(string Subject, string Body, List<string> TOs, List<string> CCs, List<mailUtils.AttachmentItem> Attachments, bool bcc, string _FROM_MAIL, string _FROM_NAME, string mailDescrtiption)
    {
        var currSmtpConfig = ModAppServerCommon.utils.getSmtpConfig();
        try
        {
            string bodyHeader = "<Html><body>";
            string bodyFooter = "</body></html>";
            SmtpClient smtp = new SmtpClient();
            smtp.Port = currSmtpConfig.SmtpPort;
            smtp.Host = currSmtpConfig.SmtpHost;
            smtp.EnableSsl = currSmtpConfig.SmtpSsl;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (currSmtpConfig.SmtpAuth && currSmtpConfig.SmtpUsr != "" && currSmtpConfig.SmtpPwd != "")
                smtp.Credentials = new System.Net.NetworkCredential(currSmtpConfig.SmtpUsr, currSmtpConfig.SmtpPwd);
            MailMessage mail = new MailMessage();
            if (CommonUtilities.getSYS_SETTING("is_email_test") == "true")
            {
                mail.To.Add(new MailAddress(CommonUtilities.getSYS_SETTING("email_test")));
            }
            else
            {
                foreach (string _to in TOs)
                    if (_to.Trim() != "")
                        mail.To.Add(new MailAddress(_to));
                foreach (string _to in CCs)
                    if (_to.Trim() != "")
                        mail.To.Add(new MailAddress(_to));
                if (BCC_MAIL.isEmail() && bcc)
                    mail.Bcc.Add(BCC_MAIL);
            }
            mail.From = new MailAddress(_FROM_MAIL, _FROM_NAME);
            mail.IsBodyHtml = true;
            string body = bodyHeader + Body + bodyFooter;
            mail.Subject = Subject;
            mail.Body = body;
            //mailUtils.addLog("0", "success1", Subject, Body, TOs, CCs, Attachments, bcc, currSmtpConfig.SmtpPort, currSmtpConfig.SmtpHost, currSmtpConfig.SmtpSsl, currSmtpConfig.SmtpUsr, currSmtpConfig.SmtpPwd, _FROM_MAIL, _FROM_NAME, mailDescrtiption);
            foreach (var _Attachment in Attachments)
                if (_Attachment.filePath != null && _Attachment.filePath.Trim() != "")
                    mail.Attachments.Add(new Attachment(_Attachment.filePath.Trim()));
            //else if (_Attachment.fileStream != null && _Attachment.fileStream != Stream.Null && _Attachment.fileName != null)
            //    mail.Attachments.Add(new Attachment(_Attachment.fileStream, _Attachment.fileName, "application/pdf"));
            smtp.Timeout = 300000;
            smtp.Send(mail);
            //mailUtils.addLog("0","success" , Subject, Body, TOs, CCs, Attachments, bcc, currSmtpConfig.SmtpPort, currSmtpConfig.SmtpHost, currSmtpConfig.SmtpSsl, currSmtpConfig.SmtpUsr, currSmtpConfig.SmtpPwd, _FROM_MAIL, _FROM_NAME, mailDescrtiption);
            return true;
        }
        catch (Exception ex)
        {
            string _ip = "";
            try { _ip = HttpContext.Current.Request.ServerVariables.Get("REMOTE_HOST"); }
            catch (Exception ex1) { }
            mailUtils.addLog(_ip, ex.ToString(), Subject, Body, TOs, CCs, Attachments, bcc, currSmtpConfig.SmtpPort, currSmtpConfig.SmtpHost, currSmtpConfig.SmtpSsl, currSmtpConfig.SmtpUsr, currSmtpConfig.SmtpPwd, _FROM_MAIL, _FROM_NAME, mailDescrtiption);
            return false;
        }
    }

}
