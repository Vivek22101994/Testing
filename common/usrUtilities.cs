using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModAuth;

public static class usrUtils
{
    private static List<USR_ADMIN> _USR_ADMINs; // refresh OK
    private static DateTime _USR_ADMINs_lastUpdate; // refresh OK
    public static List<USR_ADMIN> USR_ADMINs
    {
        get
        {
            if (_USR_ADMINs == null || _USR_ADMINs_lastUpdate == null || _USR_ADMINs_lastUpdate < DateTime.Now.AddHours(-3))
            {
                _USR_ADMINs = maga_DataContext.DC_USER.USR_ADMIN.ToList();
                _USR_ADMINs_lastUpdate = DateTime.Now;
            }
            return _USR_ADMINs;
        }
        set { _USR_ADMINs = value; }
    }
}
public partial class AdminUtilities
{
    public static string usr_adminAvailabilityTitle(int id)
    {
        USR_LK_ADMIN_AVAILABILITY _s = maga_DataContext.DC_USER.USR_LK_ADMIN_AVAILABILITies.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.title;
        return "";
    }
    public static int usr_getAvailableOperator(int id_country, int id_lang)
    {
        int pid_operator = 0;
        List<USR_RL_COUNTRY_LANG> _allCollection =
           maga_DataContext.DC_USER.USR_RL_COUNTRY_LANGs.Where(x => x.pid_country == id_country && x.pid_lang == id_lang).OrderBy(x => x.sequence).ToList();
        if (_allCollection.Count != 0)
        {
            foreach (USR_RL_COUNTRY_LANG _usr in _allCollection)
            {
                if (AdminUtilities.usr_adminIsAvailable(_usr.pid_admin))
                {
                    pid_operator = _usr.pid_admin;
                    return pid_operator;
                }
            }
        }
        List<USR_RL_COUNTRY_LANG> _langCollection =
           maga_DataContext.DC_USER.USR_RL_COUNTRY_LANGs.Where(x => x.pid_lang == id_lang && x.pid_country == 0).OrderBy(x => x.sequence).ToList();
        if (pid_operator == 0 && _langCollection.Count != 0)
        {
            foreach (USR_RL_COUNTRY_LANG _usr in _langCollection)
            {
                if (AdminUtilities.usr_adminIsAvailable(_usr.pid_admin))
                {
                    pid_operator = _usr.pid_admin;
                    return pid_operator;
                }
            }
        }
        List<USR_RL_COUNTRY_LANG> _locCollection =
           maga_DataContext.DC_USER.USR_RL_COUNTRY_LANGs.Where(x => x.pid_lang == 0 && x.pid_country == id_country).OrderBy(x => x.sequence).ToList();
        if (pid_operator == 0 && _locCollection.Count != 0)
        {
            foreach (USR_RL_COUNTRY_LANG _usr in _locCollection)
            {
                if (AdminUtilities.usr_adminIsAvailable(_usr.pid_admin))
                {
                    pid_operator = _usr.pid_admin;
                    return pid_operator;
                }
            }
        }
        return pid_operator;
    }

    public static bool usr_adminIsAvailable(int id)
    {
        // usr Availability
        USR_TBL_ADMIN_AVAILABILITY _avv = maga_DataContext.DC_USER.USR_TBL_ADMIN_AVAILABILITies.SingleOrDefault(x => x.pid_admin == id && x.pid_availability != 0 && x.date_availability == DateTime.Now.Date && x.is_mailing_day != 1);
        if (_avv != null)
            return false;
        int _dayStart = 0;
        int _dayEnd = 0;
        USR_ADMIN _admin = maga_DataContext.DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == id);
        if (_admin != null)
        {
            // usr WorkDay Time
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && _admin.day_1_start.HasValue && _admin.day_1_end.HasValue)
            {
                _dayStart = _admin.day_1_start.Value;
                _dayEnd = _admin.day_1_end.Value; ;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday && _admin.day_2_start.HasValue && _admin.day_2_end.HasValue)
            {
                _dayStart = _admin.day_2_start.Value;
                _dayEnd = _admin.day_2_end.Value; ;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday && _admin.day_3_start.HasValue && _admin.day_3_end.HasValue)
            {
                _dayStart = _admin.day_3_start.Value;
                _dayEnd = _admin.day_3_end.Value; ;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday && _admin.day_4_start.HasValue && _admin.day_4_end.HasValue)
            {
                _dayStart = _admin.day_4_start.Value;
                _dayEnd = _admin.day_4_end.Value; ;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday && _admin.day_5_start.HasValue && _admin.day_5_end.HasValue)
            {
                _dayStart = _admin.day_5_start.Value;
                _dayEnd = _admin.day_5_end.Value; ;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday && _admin.day_6_start.HasValue && _admin.day_6_end.HasValue)
            {
                _dayStart = _admin.day_6_start.Value;
                _dayEnd = _admin.day_6_end.Value; ;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && _admin.day_7_start.HasValue && _admin.day_7_end.HasValue)
            {
                _dayStart = _admin.day_7_start.Value;
                _dayEnd = _admin.day_7_end.Value; ;
            }
            // se orario non e stato messo tutto 0 significa lavora 24 ore
            if (_dayStart == _dayEnd && _dayEnd == 0)
                return true;

            if (_dayStart > DateTime.Now.Hour // se adesso sono le 7:00 e la persona inizia alle 8:00
                || _dayEnd < DateTime.Now.Hour) // se adesso sono le 18:00 e la persona finisce alle 17:00
                return false;

            // max MailCount
            int _limitDays = _admin.mailing_days.HasValue ? _admin.mailing_days.Value : 0;
            int _limitMail = _admin.mailing_days.HasValue ? _admin.mailing_max.Value : 0;
            if (_limitMail > 0)
            {
                int _mailCount = usr_adminMailCount(_admin.id, _limitDays);
                if (_limitMail <= _mailCount)
                    return false;
            }
        }
        return true;
    }
    public static int usr_adminMailCount(int idAdmin, int _limitDays)
    {
        int _count = 0;
        if (_limitDays > 0)
        {
            DateTime _limiteDate = DateTime.Now.AddDays(-_limitDays);
            _count = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Count(x => x.pid_operator == idAdmin && x.pid_related_request == 0 && x.request_date_created >= _limiteDate);
        }
        else if (_limitDays == -1)
        {
            DateTime _limiteDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _count = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Count(x => x.pid_operator == idAdmin && x.pid_related_request == 0 && x.request_date_created >= _limiteDate);
        }
        else
        {
            _count = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Count(x => x.pid_operator == idAdmin && x.pid_related_request == 0);
        }
        return _count;
    }

    public static string usr_ownerName(int id, string alternate)
    {
        USR_TBL_OWNER _s = maga_DataContext.DC_USER.USR_TBL_OWNER.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.name_full;
        return alternate;
    }

    public static string usr_adminName(int id, string alternate)
    {
        USR_ADMIN _s = usrUtils.USR_ADMINs.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.name + " " + _s.surname;
        return string.Format(alternate,id);
    }
    public static string usr_adminEmail(int id, string alternate)
    {
        USR_ADMIN _s = usrUtils.USR_ADMINs.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.email;
        return alternate;
    }

    public static List<USR_TBL_ROLE> _USR_TBL_ROLE;
    public static string usrRole_title(int id, string alternate)
    {
        if (_USR_TBL_ROLE == null) _USR_TBL_ROLE = maga_DataContext.DC_USER.USR_TBL_ROLE.ToList();
        USR_TBL_ROLE _s = _USR_TBL_ROLE.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.title;
        return alternate;
    }
    public static bool usrClient_mailNewCreation(int id)
    {
        USR_TBL_CLIENT _currTBL = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == id);
        if (_currTBL == null) return false;
        int _lang = _currTBL.pid_lang.objToInt32();
        if (_lang == 0) _lang = 2;
        string _subject = MailingUtilities.mailTemplate_subject("cl_new_created", _lang, "Error");
        if (_subject == "Error" && _lang != 2)
        {
            _lang = 2;
            _subject = MailingUtilities.mailTemplate_subject("cl_new_created", _lang, "Error");
        }
        if (_subject == "Error")
        {
            _lang = 1;
            _subject = MailingUtilities.mailTemplate_subject("cl_new_created", _lang, "Error");
        }
        string _body = MailingUtilities.mailTemplate_body("cl_new_created", _lang, "v");
        _body = _body.Replace("#cl_name_honorific#", _currTBL.name_honorific);
        _body = _body.Replace("#cl_name_full#", _currTBL.name_full);
        _body = _body.Replace("#cl_login#", "" + _currTBL.login);
        _body = _body.Replace("#cl_password#", "" + _currTBL.password);
        MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.contact_email, false, "usrClient_mailNewCreation");
        return true;
    }
    public static bool usrClient_mailPwdRecovery(int id)
    {
        USR_TBL_CLIENT _currTBL = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == id);
        if (_currTBL == null) return false;
        int _lang = _currTBL.pid_lang.objToInt32();
        if (_lang == 0) _lang = 2;
        string _subject = MailingUtilities.mailTemplate_subject("cl_pwd_recovery", _lang, "Error");
        if (_subject == "Error" && _lang != 2)
        {
            _lang = 2;
            _subject = MailingUtilities.mailTemplate_subject("cl_pwd_recovery", _lang, "Error");
        }
        if (_subject == "Error")
        {
            _lang = 1;
            _subject = MailingUtilities.mailTemplate_subject("cl_pwd_recovery", _lang, "Error");
        }
        string _body = MailingUtilities.mailTemplate_body("cl_pwd_recovery", _lang, "v");
        _body = _body.Replace("#cl_name_honorific#", _currTBL.name_honorific);
        _body = _body.Replace("#cl_name_full#", _currTBL.name_full);
        _body = _body.Replace("#cl_login#", "" + _currTBL.login);
        _body = _body.Replace("#cl_password#", "" + _currTBL.password);
        MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.contact_email, false, "usrClient_mailPwdRecovery");
        return true;
    }
    public static bool usrResCode_mailPwdRecovery(Int64 id)
    {
        RNT_TBL_RESERVATION _currTBL = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == id);
        if (_currTBL == null) return false;
        int _lang = _currTBL.cl_pid_lang.objToInt32();
        if (_lang == 0) _lang = 2;
        string _subject = MailingUtilities.mailTemplate_subject("cl_pwd_recovery", _lang, "Error");
        if (_subject == "Error" && _lang != 2)
        {
            _lang = 2;
            _subject = MailingUtilities.mailTemplate_subject("cl_pwd_recovery", _lang, "Error");
        }
        if (_subject == "Error")
        {
            _lang = 1;
            _subject = MailingUtilities.mailTemplate_subject("cl_pwd_recovery", _lang, "Error");
        }
        string _body = MailingUtilities.mailTemplate_body("cl_pwd_recovery", _lang, "v");
        _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
        _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
        _body = _body.Replace("#cl_login#", "" + _currTBL.code);
        _body = _body.Replace("#cl_password#", "" + _currTBL.password);
        MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.cl_email, false, "usrClient_mailPwdRecovery");
        return true;
    }
    #region AuthClient White Label
     public static bool usrAgentClient_mailNewCreation(long id)
    {
        dbAuthClientTBL currAgentClient = (dbAuthClientTBL)null;
        using (DCmodAuth dc = new DCmodAuth())
        {
            currAgentClient = dc.dbAuthClientTBLs.FirstOrDefault(x => x.id == id);
        }
        //USR_TBL_CLIENT _currTBL = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == id);
        if (currAgentClient == null) return false;
        int _lang = currAgentClient.pidLang.objToInt32();
        if (_lang == 0) _lang = 2;
        string _subject = MailingUtilities.mailTemplate_subject("cl_new_created", _lang, "Error");
        if (_subject == "Error" && _lang != 2)
        {
            _lang = 2;
            _subject = MailingUtilities.mailTemplate_subject("cl_new_created", _lang, "Error");
        }
        if (_subject == "Error")
        {
            _lang = 1;
            _subject = MailingUtilities.mailTemplate_subject("cl_new_created", _lang, "Error");
        }
        string _body = MailingUtilities.mailTemplate_body("cl_new_created", _lang, "v");
        _body = _body.Replace("#cl_name_honorific#", currAgentClient.nameHonorific);
        _body = _body.Replace("#cl_name_full#", currAgentClient.nameFull);
        _body = _body.Replace("#cl_login#", "" + currAgentClient.authUsr);
        _body = _body.Replace("#cl_password#", "" + currAgentClient.authPwd);
        MailingUtilities.autoSendMailTo(_subject, _body, currAgentClient.contactEmail, false, "usrAgentClient_mailNewCreation");
        return true;
    }

     public static bool usrAgentClient_mailPwdRecovery(long id)
     {
         dbAuthClientTBL currAgentClient = (dbAuthClientTBL)null;
         using (DCmodAuth dc = new DCmodAuth())
         {
             currAgentClient = dc.dbAuthClientTBLs.FirstOrDefault(x => x.id == id);
             if (currAgentClient != null)
             {
                 if (currAgentClient.authUsr == null || currAgentClient.authUsr == "")
                     currAgentClient.authUsr = currAgentClient.contactEmail;
                 if (currAgentClient.authPwd == null || currAgentClient.authPwd == "")
                     currAgentClient.authPwd = CommonUtilities.CreatePassword(8, false, true, false);
                 dc.SaveChanges();
                 authProps.ClientTBL = null;
             }
         }

         //USR_TBL_CLIENT _currTBL = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == id);
         if (currAgentClient == null) return false;
         
         
         int _lang = currAgentClient.pidLang.objToInt32();
         if (_lang == 0) _lang = 2;
         string _subject = MailingUtilities.mailTemplate_subject("cl_pwd_recovery", _lang, "Error");
         if (_subject == "Error" && _lang != 2)
         {
             _lang = 2;
             _subject = MailingUtilities.mailTemplate_subject("cl_pwd_recovery", _lang, "Error");
         }
         if (_subject == "Error")
         {
             _lang = 1;
             _subject = MailingUtilities.mailTemplate_subject("cl_pwd_recovery", _lang, "Error");
         }
         string _body = MailingUtilities.mailTemplate_body("cl_pwd_recovery", _lang, "v");
         _body = _body.Replace("#cl_name_honorific#", currAgentClient.nameHonorific);
         _body = _body.Replace("#cl_name_full#", currAgentClient.nameFull);
         _body = _body.Replace("#cl_login#", "" + currAgentClient.authUsr);
         _body = _body.Replace("#cl_password#", "" + currAgentClient.authPwd);
         MailingUtilities.autoSendMailTo(_subject, _body, currAgentClient.contactEmail, false, "usrAgentClient_mailPwdRecovery");
         return true;
     }

    #endregion
}
