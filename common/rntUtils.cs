using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;
public partial class rntUtils
{
    public static ReservationExpire CURRENT_RESERVATION_EXPIRE;
    public class ReservationExpire
    {
        public bool isStarted { get; set; }
        public int countTotal { get; set; }
        public int countCurrent { get; set; }
        public string description { get; set; }
        public DateTime? lastSend { get; set; }
        public ReservationExpire()
        {
            isStarted = false;
            countTotal = 0;
            countCurrent = 0;
            description = "";
        }
    }
    private class rntReservation_onChangeWork
    {
        RNT_TBL_RESERVATION _currTBL;
        int pidCityCaller;
        HttpRequest _currRequest;
        bool _isStartDateTimeChanged;
        bool _isEndDateTimeChanged;
        bool _isSendExpediaAvailability;
        bool _isSendHolidayAvailability;

        void doThread()
        {
            //List<RNT_TBL_RESERVATION> _listTBL = AppSettings.RNT_TBL_RESERVATION;
            //RNT_TBL_RESERVATION _res = _listTBL.SingleOrDefault(x => x.id == _currTBL.id);
            //if (_res != null)
            //    _listTBL.Remove(_res);
            //_listTBL.Add(_currTBL);
            //AppSettings.RNT_TBL_RESERVATION = _listTBL;
            if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return;

            RNT_TB_ESTATE _est = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_est == null)
                _est = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_est == null)
                return;


            rntUtilsChnlAll.UpdateAvalability(_est.id, _currTBL.id, _isSendExpediaAvailability, _isSendHolidayAvailability);

            //if (_est.bcomEnabled == 1)                
            //  BcomUpdate.BcomUpdate_start(_est.id, "availability");
            //ChnlHolidayUpdate.UpdateAvailability_start(_est.id);

            if (_currRequest != null && _currRequest.Url.AbsoluteUri.Contains("http://localhost"))
                return;
            if (_currTBL.dtStart.HasValue && _currTBL.dtEnd.HasValue && (_currTBL.state_pid == 4 || _currTBL.state_pid == 2))
                Eco_WS.LocationEvent_Insert_Update(_currTBL);
            else if (_currTBL.state_pid == 3)
                Eco_WS.LocationEvent_Delete(_currTBL);
            if (_currTBL.dtStart.HasValue && _currTBL.dtEnd.HasValue && _currTBL.state_pid == 4)
                Srs_WS.LocationEvent_Insert_Update(_currTBL, _isStartDateTimeChanged, _isEndDateTimeChanged);
            else if (_currTBL.state_pid == 3)
                Srs_WS.LocationEvent_Delete(_currTBL);
        }
        public rntReservation_onChangeWork(RNT_TBL_RESERVATION currTBL, HttpRequest currRequest, bool isStartDateTimeChanged = false, bool isEndDateTimeChanged = false, bool isSendExpediaAvailability = true, bool isSendHolidayAvailability = true)
        {
            _currTBL = currTBL;
            _currRequest = currRequest;
            pidCityCaller = 1;
            _isStartDateTimeChanged = isStartDateTimeChanged;
            _isEndDateTimeChanged = isEndDateTimeChanged;
            _isSendExpediaAvailability = isSendExpediaAvailability;
            _isSendHolidayAvailability = isSendHolidayAvailability;

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
        public rntReservation_onChangeWork(RNT_TBL_RESERVATION currTBL, HttpRequest currRequest, int PidCityCaller)
        {
            _currTBL = currTBL;
            _currRequest = currRequest;
            pidCityCaller = PidCityCaller;
            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static void rntReservation_onChange(RNT_TBL_RESERVATION _currTBL, bool isStartDateTimeChanged = false, bool isEndDateTimeChanged = false, bool isSendExpediaAvailability = true, bool isSendHolidayAvailability = true)
    {
        ReservationLastUpdate(_currTBL.id);
        HttpRequest currRequest = null;
        if (HttpContext.Current != null && HttpContext.Current.Request != null)
            currRequest = HttpContext.Current.Request;
        rntReservation_onChangeWork _tmp = new rntReservation_onChangeWork(_currTBL, currRequest, isStartDateTimeChanged, isEndDateTimeChanged, isSendExpediaAvailability, isSendHolidayAvailability);
    }
    public static void rntReservation_onChange(RNT_TBL_RESERVATION _currTBL, int PidCityCaller)
    {
        ReservationLastUpdate(_currTBL.id);
        HttpRequest currRequest = null;
        if (HttpContext.Current != null && HttpContext.Current.Request != null)
            currRequest = HttpContext.Current.Request;
        rntReservation_onChangeWork _tmp = new rntReservation_onChangeWork(_currTBL, currRequest, PidCityCaller);
    }
    public static void rntRequest_updateOperator(int idRequest, int pid_operator, bool sendMail, bool sendMailToOld, int userID)
    {
        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
        RNT_TBL_REQUEST _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == idRequest);
        if (_currTBL == null)
        {
            return;
        }
        int oldOperator = _currTBL.pid_operator.objToInt32();
        string _mSubject = "";
        string _mailSend = "";
        if (oldOperator != pid_operator)
        {
            _currTBL.operator_date = DateTime.Now;
            _currTBL.pid_operator = pid_operator;
            DC_RENTAL.SubmitChanges();
            if (oldOperator != 0)
            {
                if (sendMailToOld)
                {
                    _mSubject = "Disdetta di Assegnazione della richiesta - rif." + _currTBL.id + " - (RIR) " + " " + _currTBL.name_first + " " + _currTBL.name_last + " - " + _currTBL.request_country + " - " + _currTBL.request_choice_1 + " - " + _currTBL.request_choice_2 + " - Check-In date: " + _currTBL.request_date_start.Value.formatITA(true);
                    if (MailingUtilities.autoSendMailTo(_mSubject, "<strong>!Attenzione! Questa Richiesta non è più gestita da Lei</strong><br/><br/>" + _currTBL.mail_body, AdminUtilities.usr_adminEmail(oldOperator, ""), false, "rntUtilities_rntRequest_updateOperator al vecchio"))
                        _mailSend = "Disdetta di Assegnazione e invio mail di Disdetta a " + AdminUtilities.usr_adminName(oldOperator, "") + " (" + AdminUtilities.usr_adminEmail(oldOperator, "") + ")";
                    else
                        _mailSend = "Disdetta di Assegnazione da " + AdminUtilities.usr_adminName(oldOperator, "") + " - Errore nel invio mail di Disdetta a (" + AdminUtilities.usr_adminEmail(oldOperator, "") + ")";
                }
                else
                {
                    _mailSend = "Disdetta di Assegnazione da " + AdminUtilities.usr_adminName(oldOperator, "") + "";
                }
                rntUtils.rntRequest_addState(idRequest, 0, userID, _mailSend, "");
            }
            if (sendMail)
            {
                _mSubject = "rif." + _currTBL.id + " - Assegnata nuova richiesta prenotazione - (RIR) " + " " + _currTBL.name_first + " " + _currTBL.name_last + " - " + _currTBL.request_country + " - " + _currTBL.request_choice_1 + " - " + _currTBL.request_choice_2 + " - Check-In date: " + _currTBL.request_date_start.Value.formatITA(true);
                if (MailingUtilities.autoSendMailTo(_mSubject, _currTBL.mail_body, AdminUtilities.usr_adminEmail(_currTBL.pid_operator.Value, ""), false, "rntUtilities_rntRequest_updateOperator al nuovo"))
                    _mailSend = "Assegnato e inviato mail a " + AdminUtilities.usr_adminName(_currTBL.pid_operator.Value, "") + " (" + AdminUtilities.usr_adminEmail(_currTBL.pid_operator.Value, "") + ")";
                else
                    _mailSend = "Assegnato a " + AdminUtilities.usr_adminName(_currTBL.pid_operator.Value, "") + " - Errore nel invio mail a (" + AdminUtilities.usr_adminEmail(_currTBL.pid_operator.Value, "") + ")";
            }
            else
            {
                _mailSend = "Assegnato a " + AdminUtilities.usr_adminName(_currTBL.pid_operator.Value, "") + " ";
            }
            rntUtils.rntRequest_addState(idRequest, 0, userID, _mailSend, "");
        }
        List<RNT_TBL_REQUEST> _relatedRequestList = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.pid_related_request == _currTBL.id).ToList();
        foreach (RNT_TBL_REQUEST _relatedRequest in _relatedRequestList)
        {
            oldOperator = _relatedRequest.pid_operator.objToInt32();
            if (oldOperator == _currTBL.pid_operator)
                continue;
            _relatedRequest.operator_date = DateTime.Now;
            _relatedRequest.pid_operator = _currTBL.pid_operator;
            if (oldOperator != 0)
            {
                if (sendMailToOld)
                {
                    _mSubject = "Disdetta di Assegnazione della richiesta - rif." + _relatedRequest.id + " Correlata a rif." + _currTBL.id + " - (RIR) " + " " + _relatedRequest.name_first + " " + _relatedRequest.name_last + " - " + _relatedRequest.request_country + " - " + _relatedRequest.request_choice_1 + " - " + _relatedRequest.request_choice_2 + " - Check-In date: " + _relatedRequest.request_date_start.Value.formatITA(true);
                    if (MailingUtilities.autoSendMailTo(_mSubject, "<strong>!Attenzione! Questa Richiesta non è più gestita da Lei</strong><br/><br/>" + _relatedRequest.mail_body, AdminUtilities.usr_adminEmail(oldOperator, ""), false, "rntUtilities_rntRequest_updateOperator al vecchio correlati"))
                        _mailSend = "Disdetta di Assegnazione e invio mail di Disdetta a " + AdminUtilities.usr_adminName(oldOperator, "") + " (" + AdminUtilities.usr_adminEmail(oldOperator, "") + ")";
                    else
                        _mailSend = "Disdetta di Assegnazione da " + AdminUtilities.usr_adminName(oldOperator, "") + " - Errore nel invio mail di Disdetta a (" + AdminUtilities.usr_adminEmail(oldOperator, "") + ")";
                }
                else
                {
                    _mailSend = "Disdetta di Assegnazione da " + AdminUtilities.usr_adminName(oldOperator, "") + "";
                }
                rntUtils.rntRequest_addState(_relatedRequest.id, 0, userID, _mailSend, "");
            }
            if (sendMail)
            {
                _mSubject = "rif." + _relatedRequest.id + " Correlata a rif." + _currTBL.id + " - Assegnata nuova richiesta prenotazione - (RIR) " + " " + _relatedRequest.name_first + " " + _relatedRequest.name_last + " - " + _relatedRequest.request_country + " - " + _relatedRequest.request_choice_1 + " - " + _relatedRequest.request_choice_2 + " - Check-In date: " + _relatedRequest.request_date_start.Value.formatITA(true);
                if (MailingUtilities.autoSendMailTo(_mSubject, _relatedRequest.mail_body, AdminUtilities.usr_adminEmail(_currTBL.pid_operator.Value, ""), false, "rntUtilities_rntRequest_updateOperator al nuovo correlati"))
                    _mailSend = "Assegnato e inviato mail a " + AdminUtilities.usr_adminName(_relatedRequest.pid_operator.Value, "") + " (" + AdminUtilities.usr_adminEmail(_relatedRequest.pid_operator.Value, "") + ")";
                else
                    _mailSend = "Assegnato a " + AdminUtilities.usr_adminName(_relatedRequest.pid_operator.Value, "") + " - Errore nel invio mail a (" + AdminUtilities.usr_adminEmail(_relatedRequest.pid_operator.Value, "") + ")";
            }
            else
            {
                _mailSend = "Assegnato a " + AdminUtilities.usr_adminName(_relatedRequest.pid_operator.Value, "") + "";
            }
            rntUtils.rntRequest_addState(_relatedRequest.id, 0, userID, _mailSend, "");
        }
        DC_RENTAL.SubmitChanges();
    }
    public static bool rntservice_Request(dbRntExtrasRequest _currTBL)
    {
        int _lang = 1;
        string _subject = "";
        string _body = "";
        bool _bcc = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiverBCC") == "true";
        string _messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_service_reservationConfirmed_messagesReceiver");
        string cc__messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_service_resConfirmed_cc_messagesReceiver");
        _subject = MailingUtilities.mailTemplate_subject("ad_service_res_created", _lang, "Error");
        _subject = _subject.Replace("#resCode#", "" + CurrentSource.getresCode(_currTBL.pidReservation.objToInt64()));
        _body = MailingUtilities.mailTemplate_body("ad_service_res_created", _lang, "Error");
        _body = _body.Replace("#service_name#", CurrentSource.rntService_title(_currTBL.pidExtra.objToInt32(), _lang, ""));
        _body = _body.Replace("#service_id#", "" + _currTBL.pidExtra.objToInt32());
        _body = _body.Replace("#note#", "" + _currTBL.note);
        _body = _body.Replace("#service_dt#", "" + _currTBL.createdDate.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
        _body = _body.Replace("#resCode#", "" + CurrentSource.getresCode(_currTBL.pidReservation.objToInt64()));
        bool result = MailingUtilities.autoSendMailTo(_subject, _body, _messagesReceiver, _bcc, "rntUtilities_rntService_request ad");
        bool result1 = MailingUtilities.autoSendMailTo(_subject, _body, cc__messagesReceiver, _bcc, "rntUtilities_rntService_request ad");
        if (result == true && result1 == true)
        {
            return true;
        }
        else
        {
            return false;
        }
        //return MailingUtilities.autoSendMailTo(_subject, _body, "ns.narola@narolainfotech.com", false, "rntUtilities_rntService_request ad");
    }

    public static void rntReservation_updateOperator(int IdReservation, int pid_operator, bool sendMail, bool sendMailToOld, int userID)
    {
        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
        RNT_TBL_RESERVATION _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
        if (_currTBL == null)
        {
            return;
        }
        int oldOperator = _currTBL.pid_operator.objToInt32();
        if (oldOperator != pid_operator)
        {
            rntUtils.rntReservation_addState(IdReservation, 0, userID, "Cambiato Account: da '" + AdminUtilities.usr_adminName(oldOperator, "System") + " '=> a '" + AdminUtilities.usr_adminName(pid_operator, "System") + "'", "");
            _currTBL.operator_date = DateTime.Now;
            _currTBL.pid_operator = pid_operator;
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
        }
    }
    public static string rntRequest_getStateName(string id)
    {
        int _id;
        if (id == "" || !int.TryParse(id, out _id) || _id == 0) return "Automatico";
        RNT_LK_REQUEST_STATE _d = maga_DataContext.DC_RENTAL.RNT_LK_REQUEST_STATEs.SingleOrDefault(x => x.id == _id);
        if (_d != null)
            return _d.title;
        return "";
    }

    public static void rntEstate_createPagePath_All()
    {
        List<RNT_TB_ESTATE> _tbList = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted != 1).ToList();
        foreach (RNT_TB_ESTATE _tb in _tbList)
        {
            rntEstate_createPagePath(_tb.id);
        }
    }
    public static void rntEstate_createPagePath(int id_estate)
    {
        magaRental_DataContext _dc = maga_DataContext.DC_RENTAL;
        RNT_TB_ESTATE _tb = _dc.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id_estate);
        if (_tb == null) return;
        List<RNT_LN_ESTATE> _lnList = _dc.RNT_LN_ESTATE.Where(x => x.pid_estate == id_estate).ToList();
        foreach (RNT_LN_ESTATE _ln in _lnList)
        {
            if (string.IsNullOrEmpty(_ln.title)) continue;
            if (_tb.category == "apt")
            {
                LOC_LN_ZONE _lnZone = maga_DataContext.DC_LOCATION.LOC_LN_ZONEs.SingleOrDefault(x => x.pid_zone == _tb.pid_zone && x.pid_lang == _ln.pid_lang);
                if (_lnZone == null) _lnZone = maga_DataContext.DC_LOCATION.LOC_LN_ZONEs.SingleOrDefault(x => x.pid_zone == _tb.pid_zone && x.pid_lang == 2);
                if (_lnZone == null) _lnZone = maga_DataContext.DC_LOCATION.LOC_LN_ZONEs.SingleOrDefault(x => x.pid_zone == _tb.pid_zone && x.pid_lang == 1);
                if (_lnZone != null && !string.IsNullOrEmpty(_lnZone.folder_path))
                    _ln.page_path = _lnZone.folder_path.clearPathName() + "/" + _ln.title.clearPathName().Replace("-", "");
            }
            if (_tb.category == "villa")
            {
                _ln.page_path = CurrentSource.getSysLangValue("urlRomeVillas", _ln.pid_lang).clearPathName() + "/" + _ln.title.clearPathName().Replace("-", "");
            }
        }


        _dc.SubmitChanges();
    }

}


