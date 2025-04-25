using ModAuth;
using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

public partial class rntUtils
{
    private class rntReservation_mailNewCreationWork
    {
        RNT_TBL_RESERVATION _currTBL;
        bool _toCL;
        bool _toAD;
        bool _toSRS;
        bool _toECO;
        int _currUser;
        void doThread()
        {
            if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return;
            DateTime _dtExpire = _currTBL.block_expire.HasValue ? _currTBL.block_expire.Value : _currTBL.dtCreation.Value.AddHours(24);
            var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);
            int _lang = _currTBL.cl_pid_lang.objToInt32();
            if (_lang == 0) _lang = 2;
            // mail to cl
            bool _bcc = CommonUtilities.getSYS_SETTING("rnt_reservationCreated_messagesReceiverBCC") == "true";
            string _messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_reservationCreated_messagesReceiver");
            string _body = "";
            string _subject = "";
            if (_toCL && (agentTbl == null || agentTbl.isMsgsEnabled == 1))
            {
                _subject = MailingUtilities.mailTemplate_subject("cl_res_created", _lang, "");
                if (_subject == "" && _lang != 2)
                {
                    _lang = 2;
                    _subject = MailingUtilities.mailTemplate_subject("cl_res_created", _lang, "");
                }
                if (_subject == "")
                {
                    _lang = 1;
                    _subject = MailingUtilities.mailTemplate_subject("cl_res_created", _lang, "Error");
                }
                _body = MailingUtilities.mailTemplate_body("cl_res_created", _lang, "Error");
                _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                _body = _body.Replace("#estate_name#", CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error"));
                _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                    _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
                _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#resCode#", "" + _currTBL.code);
                _body = _body.Replace("#resPwd#", "" + _currTBL.password);
                _body = _body.Replace("#expireDate#", "" + _dtExpire.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                _body = _body.Replace("#expireTime#", "" + _dtExpire.TimeOfDay.JSTime_toString(false, true));
                _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
                if (MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.cl_email, _bcc, "rntUtilities_rntReservation_mailNewCreationWork cl"))
                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della creazione al cliente", "");
            }
            // mail to admin
            if (_toAD)
            {
                _lang = 1;
                _subject = MailingUtilities.mailTemplate_subject("ad_res_created", _lang, "Error");
                _subject = _subject.Replace("#resCode#", "" + _currTBL.code);
                _body = MailingUtilities.mailTemplate_body("ad_res_created", _lang, "Error");
                _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                _body = _body.Replace("#estate_name#", CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), 1, "Error"));
                _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                    _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
                _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#resCode#", "" + _currTBL.code);
                _body = _body.Replace("#resPwd#", "" + _currTBL.password);
                _body = _body.Replace("#expireDate#", "" + _dtExpire.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                _body = _body.Replace("#expireTime#", "" + _dtExpire.TimeOfDay.JSTime_toString(false, true));
                _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
                if (MailingUtilities.autoSendMailTo(_subject, _body, _messagesReceiver, _bcc, "rntUtilities_rntReservation_mailNewCreationWork ad"))
                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della creazione al admin <" + _messagesReceiver + ">", "");
            }
        }
        public rntReservation_mailNewCreationWork(RNT_TBL_RESERVATION currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, int currUser)
        {
            _currTBL = currTBL;
            _toCL = toCL;
            _toAD = toAD;
            _toSRS = toSRS;
            _toECO = toECO;
            _currUser = currUser;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.rntReservation_mailNewCreationWork resId:" + currTBL.id);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static bool rntReservation_mailNewCreation(RNT_TBL_RESERVATION _currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, int currUser)
    {
        rntReservation_mailNewCreationWork _tmp = new rntReservation_mailNewCreationWork(_currTBL, toCL, toAD, toSRS, toECO, currUser);
        return true;
    }
    public static bool rntRequest_mailNewCreation(RNT_TBL_REQUEST _currTBL)
    {
        if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return false;
        int _lang = _currTBL.pid_lang.objToInt32();
        if (_lang == 0) _lang = 2;
        // mail to cl
        string _subject = MailingUtilities.mailTemplate_subject("cl_request_created", _lang, "");
        if (_subject == "" && _lang != 2)
        {
            _lang = 2;
            _subject = MailingUtilities.mailTemplate_subject("cl_request_created", _lang, "");
        }
        if (_subject == "")
        {
            _lang = 1;
            _subject = MailingUtilities.mailTemplate_subject("cl_request_created", _lang, "Error");
        }
        string _body = MailingUtilities.mailTemplate_body("cl_request_created", _lang, "Error");
        if (_currTBL.pid_city == 3)
        {
            _subject = _subject.Replace("Rental in Rome", "Rental in Venice");
            _body = _body.Replace("http://www.rentalinrome.com/images/css/logo.gif", "http://www.rentalinvenice.com/images/css/logo.gif");
            _body = _body.Replace("RentalInRome", "RentalInVenice");
        }
        _body = _body.Replace("#cl_name_honorific#", _currTBL.name_honorific);
        _body = _body.Replace("#cl_name_full#", _currTBL.name_full);
        _body = _body.Replace("#dtStart#", "" + _currTBL.request_date_start.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
        _body = _body.Replace("#dtEnd#", "" + _currTBL.request_date_end.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
        if (_currTBL.request_date_end.HasValue && _currTBL.request_date_start.HasValue)
            _body = _body.Replace("#dtCount#", "" + (_currTBL.request_date_end.Value.Date - _currTBL.request_date_start.Value.Date).Days);
        _body = _body.Replace("#num_pers_total#", "" + (_currTBL.request_adult_num + _currTBL.request_child_num + _currTBL.request_child_num_min));
        _body = _body.Replace("#num_adult#", "" + (_currTBL.request_adult_num.objToInt32()));
        _body = _body.Replace("#num_child_over#", "" + (_currTBL.request_child_num.objToInt32()));
        _body = _body.Replace("#num_child_min#", "" + (_currTBL.request_child_num_min.objToInt32()));
        _body = _body.Replace("#resCode#", "" + _currTBL.id);
        return MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.email, CommonUtilities.getSYS_SETTING("mailing_clRequestCreatedBCC") == "true", "rntUtilities_rntRequest_mailNewCreation");
    }
    private class rntReservation_mailPartPaymentReceiveWork
    {
        RNT_TBL_RESERVATION _currTBL;
        bool _toCL;
        bool _toAD;
        bool _toOWN;
        bool _toSRS;
        bool _toECO;

        int _currUser;
        void doThread()
        {
            if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return;
            bool _bcc = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiverBCC") == "true";
            string _messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiver");
            var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);
            RNT_TB_ESTATE _estate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estate == null)
            {
                // MailingUtilities.autoSendMailTo("!!! Errore invio Mail Notifica", "La struttura rif: " + _currTBL.pid_estate + " non si trova!!!<br/>IdRes=" + _currTBL.id + "<br/>IdPayment=" + _pay.id, "adilet@magadesign.net", _bcc);
                //return;
            }
            int _lang = _currTBL.cl_pid_lang.objToInt32();
            if (_lang == 0) _lang = 2;
            decimal pr_payed = 0;
            magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
            List<INV_TBL_PAYMENT> _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete == 1 && x.direction == 1).ToList();
            foreach (INV_TBL_PAYMENT _currPay in _listPay)
            {
                pr_payed += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
            }
            decimal pr_to_pay = _currTBL.pr_total.objToDecimal() - pr_payed;
            decimal pr_to_pay_owner = _currTBL.bcom_totalForOwner.objToDecimal() > 0 ? _currTBL.bcom_totalForOwner.objToDecimal() - pr_payed : pr_to_pay;
            decimal bcom_totalForOwner = _currTBL.bcom_totalForOwner.objToDecimal() > 0 ? _currTBL.bcom_totalForOwner.objToDecimal() : _currTBL.bcom_totalForOwner.objToDecimal();
            string _estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
            string _dtStartTime = _currTBL.dtStartTime.JSTime_stringToTime().JSTime_toString(false, true);
            string _body = "";
            string _subject = "";
            string extra_body = "";
            string extra_subject = "";
            string _subjectEcoSRS = "#dtStartTime# #estate_name# #resCode# IN: #dtStart# OUT: #dtEnd#";
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtStartTime#", _dtStartTime);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#estate_name#", _estate_name);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#resCode#", "" + _currTBL.code);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd#/#mm#/#yy#", _lang, " --/--/--"));
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd#/#mm#/#yy#", _lang, " --/--/--"));
            // mail to cl
            if (_toCL && (agentTbl == null || agentTbl.isMsgsEnabled == 1))
            {
                _subject = MailingUtilities.mailTemplate_subject("cl_payment_receive", _lang, "");
                if (_subject == "" && _lang != 2)
                {
                    _lang = 2;
                    _subject = MailingUtilities.mailTemplate_subject("cl_payment_receive", _lang, "");
                }
                if (_subject == "")
                {
                    _lang = 1;
                    _subject = MailingUtilities.mailTemplate_subject("cl_payment_receive", _lang, "Error");
                }
                _body = MailingUtilities.mailTemplate_body("cl_payment_receive", _lang, "Error");
                _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                _body = _body.Replace("#estate_name#", _estate_name);
                _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                _body = _body.Replace("#dtStartTime#", _dtStartTime);
                _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                    _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
                _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_total_owner#", "" + bcom_totalForOwner.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_to_pay#", "" + pr_to_pay.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_to_pay_owner#", "" + pr_to_pay_owner.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#resCode#", "" + _currTBL.code);
                _body = _body.Replace("#resPwd#", "" + _currTBL.password);
                _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
                if (MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.cl_email, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork cl"))
                    //if (MailingUtilities.autoSendMailTo(_subject, _body, "ns.narola@narolainfotech.com", false, "rntUtilities_rntReservation_mailPartPaymentReceiveWork cl"))
                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al cliente", "");


            }
            // mail to admin
            _lang = 1;
            _estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
            if (_toAD)
            {
                _subject = MailingUtilities.mailTemplate_subject("ad_payment_receive", _lang, "Error");
                _subject = _subject.Replace("#resCode#", "" + _currTBL.code);
                _body = MailingUtilities.mailTemplate_body("ad_payment_receive", _lang, "Error");
                _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                _body = _body.Replace("#estate_name#", _estate_name);
                _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                _body = _body.Replace("#dtStartTime#", _dtStartTime);
                _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                    _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
                _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_to_pay#", "" + pr_to_pay.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#resCode#", "" + _currTBL.code);
                _body = _body.Replace("#resPwd#", "" + _currTBL.password);
                _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
                if (MailingUtilities.autoSendMailTo(_subject, _body, _messagesReceiver, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ad"))
                    //if (MailingUtilities.autoSendMailTo(_subject, _body, "ns.narola@narolainfotech.com", false, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ad"))    
                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al admin <" + _messagesReceiver + ">", "");
                if (_currTBL.bcom_resId + "" != "" && _currTBL.bcom_roomResId + "" != "" && CommonUtilities.getSYS_SETTING("rntBcom_messagesConfirmedReceiver").isEmail() && MailingUtilities.autoSendMailTo(_subject, _body, CommonUtilities.getSYS_SETTING("rntBcom_messagesConfirmedReceiver"), _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ad"))
                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al admin <" + CommonUtilities.getSYS_SETTING("rntBcom_messagesConfirmedReceiver") + ">", "");

            }
            // mail to owner , srs
            _subject = MailingUtilities.mailTemplate_subject("owner_res_confirmed", _lang, "Error");
            _subject = _subject.Replace("#resCode#", "" + _currTBL.code);
            _body = MailingUtilities.mailTemplate_body("owner_res_confirmed", _lang, "Error");
            _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
            _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
            _body = _body.Replace("#cl_email#", "" + _currTBL.cl_email);
            _body = _body.Replace("#estate_name#", _estate_name);
            _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
            _body = _body.Replace("#estate_address#", _estate.loc_address);
            _body = _body.Replace("#dtStartTime#", _dtStartTime);
            _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
            _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
            _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
            _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
            _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
            _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
            _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
            _body = _body.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
            _body = _body.Replace("#pr_to_pay#", "" + pr_to_pay.ToString("N2") + "&nbsp;&euro;");
            _body = _body.Replace("#resCode#", "" + _currTBL.code);
            _body = _body.Replace("#resPwd#", "" + _currTBL.password);
            _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
            _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
            _body = _body.Replace("#link_voucher#", "<a href=\"http://www.rentalinrome.com/pdfgenerator/pdf_rnt_reservation_voucher.aspx?uid=" + _currTBL.uid_2 + "\">Link al Voucher</a>");

            var _bodyOwner = MailingUtilities.mailTemplate_body("owner_res_confirmed", _lang, "Error");
            _bodyOwner = _bodyOwner.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
            _bodyOwner = _bodyOwner.Replace("#cl_name_full#", _currTBL.cl_name_full);
            _bodyOwner = _bodyOwner.Replace("#cl_email#", "" + _currTBL.cl_email);
            _bodyOwner = _bodyOwner.Replace("#estate_name#", _estate_name);
            _bodyOwner = _bodyOwner.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
            _bodyOwner = _bodyOwner.Replace("#estate_address#", _estate.loc_address);
            _bodyOwner = _bodyOwner.Replace("#dtStartTime#", _dtStartTime);
            _bodyOwner = _bodyOwner.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            _bodyOwner = _bodyOwner.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                _bodyOwner = _bodyOwner.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
            _bodyOwner = _bodyOwner.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
            _bodyOwner = _bodyOwner.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
            _bodyOwner = _bodyOwner.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
            _bodyOwner = _bodyOwner.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
            _bodyOwner = _bodyOwner.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
            _bodyOwner = _bodyOwner.Replace("#pr_total#", "" + bcom_totalForOwner.objToDecimal().ToString("N2") + "&nbsp;&euro;");
            _bodyOwner = _bodyOwner.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
            _bodyOwner = _bodyOwner.Replace("#pr_to_pay#", "" + pr_to_pay_owner.ToString("N2") + "&nbsp;&euro;");
            _bodyOwner = _bodyOwner.Replace("#resCode#", "" + _currTBL.code);
            _bodyOwner = _bodyOwner.Replace("#resPwd#", "" + _currTBL.password);
            _bodyOwner = _bodyOwner.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
            _bodyOwner = _bodyOwner.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
            if (agentTbl == null || agentTbl.sentVoucherToOwner.objToInt32() == 1)
                _bodyOwner = _bodyOwner.Replace("#link_voucher#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/pdfgenerator/pdf_rnt_reservation_voucherfo.aspx?uid=" + _currTBL.unique_id + _currTBL.uid_2 + "\">Link al Voucher</a>");
            else
                _bodyOwner = _bodyOwner.Replace("#link_voucher#", "");
            USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currTBL.cl_id);
            if (_client != null)
            {
                _body = _body.Replace("#cl_phone#", "" + _client.contact_phone);
            }
            if (_estate.pid_owner.HasValue && _estate.pid_owner != 1)
            {
                // Accoglienza del proprietario
                if (_estate.is_srs != 1 && _toSRS && _estate.srs_ext_email != null && _estate.srs_ext_email != "")
                    if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _estate.srs_ext_email, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ownersrs"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al accoglienza del prop <" + _estate.srs_ext_email + ">", "");
                if (_toOWN)
                {
                    USR_TBL_OWNER _owner = maga_DataContext.DC_USER.USR_TBL_OWNER.SingleOrDefault(x => x.id == _estate.pid_owner);
                    if (_owner != null)
                    {
                        if (_owner.contact_email != null && _owner.contact_email != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner1"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email + ">", "");
                        if (_owner.contact_email_2 != null && _owner.contact_email_2 != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_2, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner2"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email_2 + ">", "");
                        if (_owner.contact_email_3 != null && _owner.contact_email_3 != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_3, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner3"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email_3 + ">", "");
                        if (_owner.contact_email_4 != null && _owner.contact_email_4 != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_4, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner4"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email_4 + ">", "");
                        if (_owner.contact_email_5 != null && _owner.contact_email_5 != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_5, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner5"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email_5 + ">", "");
                    }
                }
            }


            if (_toSRS && _estate.is_srs == 1)
            {
                //  Yefrey Guerrero Paredes;welcoming-4@areas.rentalinrome.com;1
                if (_estate.pid_zone == 1)
                    if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-4@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-4@areas.rentalinrome.com"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-4@areas.rentalinrome.com", "");
                //Vladimir Trbovic;welcoming-5@areas.rentalinrome.com;5;6;17
                if (_estate.pid_zone == 5 || _estate.pid_zone == 6 || _estate.pid_zone == 17)
                    if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-5@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-5@areas.rentalinrome.com"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-5@areas.rentalinrome.com", "");
                //Jonny;welcoming-3@areas.rentalinrome.com;4;18;19
                if (_estate.pid_zone == 4 || _estate.pid_zone == 9 || _estate.pid_zone == 18 || _estate.pid_zone == 19)
                    if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-3@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-3@areas.rentalinrome.com"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-3@areas.rentalinrome.com", "");
                //Filipe Marques;welcoming-1@areas.rentalinrome.com;2;16
                if (_estate.pid_zone == 2 || _estate.pid_zone == 16)
                    if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-1@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-1@areas.rentalinrome.com"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-1@areas.rentalinrome.com", "");
                //Alessandro Pancaldi;welcoming-2@areas.rentalinrome.com;3
                if (_estate.pid_zone == 3)
                    if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-2@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-2@areas.rentalinrome.com"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-2@areas.rentalinrome.com", "");
                //Alex;reception@rentalinrome.com;19
                if (_estate.pid_zone == 19)
                    if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "reception@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork reception@areas.rentalinrome.com"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS reception@areas.rentalinrome.com", "");
            }
            if (_toECO)
            {
                if (_estate.eco_ext_email != null && _estate.eco_ext_email != "")
                    if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _bodyOwner, _estate.eco_ext_email, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ownereco"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma alla pulizia del prop <" + _estate.eco_ext_email + ">", "");
                var mailing_ecoMessagesReceiver = CommonUtilities.getSYS_SETTING("mailing_ecoMessagesReceiver");
                if (_estate.is_ecopulizie == 1 && mailing_ecoMessagesReceiver.isEmail())
                    if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, mailing_ecoMessagesReceiver, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork " + mailing_ecoMessagesReceiver))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma alla pulizia eco <" + mailing_ecoMessagesReceiver + ">", "");
            }
        }
        public rntReservation_mailPartPaymentReceiveWork(RNT_TBL_RESERVATION currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, bool toOWN, int currUser)
        {
            _currTBL = currTBL;
            _toCL = toCL;
            _toAD = toAD;
            _toSRS = toSRS;
            _toECO = toECO;
            _toOWN = toOWN;
            _currUser = currUser;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.rntReservation_mailPartPaymentReceiveWork resId:" + currTBL.id);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }

    public static bool rntReservation_mailPartPaymentReceive(RNT_TBL_RESERVATION _currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, bool toOWN, int currUser)
    {
        rntReservation_mailPartPaymentReceiveWork _tmp = new rntReservation_mailPartPaymentReceiveWork(_currTBL, toCL, toAD, toSRS, toECO, toOWN, currUser);
        return true;
    }

    private class rntReservation_mailBalancePaymentReceiveWork
    {
        RNT_TBL_RESERVATION _currTBL;
        bool _toCL;
        bool _toAD;
        bool _toOWN;
        bool _toSRS;
        bool _toECO;

        int _currUser;
        void doThread()
        {
            if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return;
            bool _bcc = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiverBCC") == "true";
            string _messagesReceiver = "";
            if (_currUser > 0)
            {
                _messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiver");
                magaUser_DataContext dc_user = maga_DataContext.DC_USER;

                var objCurrAcc = dc_user.USR_ADMIN.SingleOrDefault(x => x.id == _currUser);
                if (objCurrAcc != null)
                {
                    _messagesReceiver += "," + objCurrAcc.email;
                }
            }
            else
                _messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiver");

            var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);
            RNT_TB_ESTATE _estate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estate == null)
            {
                // MailingUtilities.autoSendMailTo("!!! Errore invio Mail Notifica", "La struttura rif: " + _currTBL.pid_estate + " non si trova!!!<br/>IdRes=" + _currTBL.id + "<br/>IdPayment=" + _pay.id, "adilet@magadesign.net", _bcc);
                //return;
            }
            int _lang = _currTBL.cl_pid_lang.objToInt32();
            if (_lang == 0) _lang = 2;
            decimal pr_payed = 0;
            magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
            List<INV_TBL_PAYMENT> _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete == 1 && x.direction == 1).ToList();
            foreach (INV_TBL_PAYMENT _currPay in _listPay)
            {
                pr_payed += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
            }

            // decimal pr_to_pay = _currTBL.pr_total.objToDecimal() - pr_payed;
            // decimal pr_to_pay_owner = _currTBL.bcom_totalForOwner.objToDecimal() > 0 ? _currTBL.bcom_totalForOwner.objToDecimal() - pr_payed : pr_to_pay;
            decimal pr_balance = pr_payed - _currTBL.pr_part_payment_total.objToDecimal();
            decimal bcom_totalForOwner = _currTBL.bcom_totalForOwner.objToDecimal() > 0 ? _currTBL.bcom_totalForOwner.objToDecimal() : _currTBL.bcom_totalForOwner.objToDecimal();
            string _estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
            string _dtStartTime = _currTBL.dtStartTime.JSTime_stringToTime().JSTime_toString(false, true);
            string _body = "";
            string _subject = "";
            string extra_body = "";
            string extra_subject = "";
            string _subjectEcoSRS = "#dtStartTime# #estate_name# #resCode# IN: #dtStart# OUT: #dtEnd#";
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtStartTime#", _dtStartTime);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#estate_name#", _estate_name);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#resCode#", "" + _currTBL.code);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd#/#mm#/#yy#", _lang, " --/--/--"));
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd#/#mm#/#yy#", _lang, " --/--/--"));
            // mail to cl
            if (_toCL && (agentTbl == null || agentTbl.isMsgsEnabled == 1) && _currTBL.isSendBalancePayementEmail == 1)
            {
                _subject = MailingUtilities.mailTemplate_subject("cl_balance_payment_receive", _lang, "");
                if (_subject == "" && _lang != 2)
                {
                    _lang = 2;
                    _subject = MailingUtilities.mailTemplate_subject("cl_balance_payment_receive", _lang, "");
                }
                if (_subject == "")
                {
                    _lang = 1;
                    _subject = MailingUtilities.mailTemplate_subject("cl_balance_payment_receive", _lang, "Error");
                }
                _body = MailingUtilities.mailTemplate_body("cl_balance_payment_receive", _lang, "Error");
                _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                _body = _body.Replace("#estate_name#", _estate_name);
                _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                _body = _body.Replace("#dtStartTime#", _dtStartTime);
                _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                    _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
                _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_total_owner#", "" + bcom_totalForOwner.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_payed#", "" + _currTBL.pr_part_payment_total.Value.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_balance#", "" + pr_balance.ToString("N2") + "&nbsp;&euro;");
                //_body = _body.Replace("#pr_to_pay_owner#", "" + pr_to_pay_owner.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#resCode#", "" + _currTBL.code);
                _body = _body.Replace("#resPwd#", "" + _currTBL.password);
                _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
                if (MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.cl_email, _bcc, "rntUtilities_rntReservation_mailBalnacePaymentReceiveWork cl"))
                    //if (MailingUtilities.autoSendMailTo(_subject, _body, "ns.narola@narolainfotech.com", false, "rntUtilities_rntReservation_mailPartPaymentReceiveWork cl"))
                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al cliente", "");


            }
            // mail to admin
            _lang = 1;
            _estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
            if (_toAD)
            {
                _subject = MailingUtilities.mailTemplate_subject("ad_balance_payment_receive", _lang, "Error");
                _subject = _subject.Replace("#resCode#", "" + _currTBL.code);
                _body = MailingUtilities.mailTemplate_body("ad_balance_payment_receive", _lang, "Error");
                _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                _body = _body.Replace("#estate_name#", _estate_name);
                _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                _body = _body.Replace("#dtStartTime#", _dtStartTime);
                _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                    _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
                _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_payed#", "" + _currTBL.pr_part_payment_total.Value.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#pr_balance#", "" + pr_balance.ToString("N2") + "&nbsp;&euro;");
                _body = _body.Replace("#resCode#", "" + _currTBL.code);
                _body = _body.Replace("#resPwd#", "" + _currTBL.password);
                _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");

                if (_messagesReceiver.Contains(","))
                {
                    if (MailingUtilities.autoSendMultiMail(_subject, _body, _messagesReceiver.splitStringToList(","), _bcc, "rntUtilities_rntReservation_mailBalnacePaymentReceiveWork ad"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al admin <" + _messagesReceiver + ">", "");
                }
                else
                {
                    if (MailingUtilities.autoSendMailTo(_subject, _body, _messagesReceiver, _bcc, "rntUtilities_rntReservation_mailBalnacePaymentReceiveWork ad"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al admin <" + _messagesReceiver + ">", "");
                }
                if (_currTBL.bcom_resId + "" != "" && _currTBL.bcom_roomResId + "" != "" && CommonUtilities.getSYS_SETTING("rntBcom_messagesConfirmedReceiver").isEmail() && MailingUtilities.autoSendMailTo(_subject, _body, CommonUtilities.getSYS_SETTING("rntBcom_messagesConfirmedReceiver"), _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ad"))
                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al admin <" + CommonUtilities.getSYS_SETTING("rntBcom_messagesConfirmedReceiver") + ">", "");

            }


        }
        public rntReservation_mailBalancePaymentReceiveWork(RNT_TBL_RESERVATION currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, bool toOWN, int currUser)
        {
            _currTBL = currTBL;
            _toCL = toCL;
            _toAD = toAD;
            _toSRS = toSRS;
            _toECO = toECO;
            _toOWN = toOWN;
            _currUser = currUser;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.rntReservation_mailBalancePaymentReceiveWork resId:" + currTBL.id);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }

    public static bool rntReservation_mailBalancePaymentReceive(RNT_TBL_RESERVATION _currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, bool toOWN, int currUser)
    {
        rntReservation_mailBalancePaymentReceiveWork _tmp = new rntReservation_mailBalancePaymentReceiveWork(_currTBL, toCL, toAD, toSRS, toECO, toOWN, currUser);
        return true;
    }

    private class rntReservation_mailPayBalanceWork
    {
        RNT_TBL_RESERVATION _currTBL;
        bool _toCL;
        bool _toAD;
        bool _toOWN;
        bool _toSRS;
        bool _toECO;

        int _currUser;
        void doThread()
        {
            if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return;
            bool _bcc = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiverBCC") == "true";
            string _messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiver");
            var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);
            RNT_TB_ESTATE _estate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estate == null)
            {
                // MailingUtilities.autoSendMailTo("!!! Errore invio Mail Notifica", "La struttura rif: " + _currTBL.pid_estate + " non si trova!!!<br/>IdRes=" + _currTBL.id + "<br/>IdPayment=" + _pay.id, "adilet@magadesign.net", _bcc);
                //return;
            }
            int _lang = _currTBL.cl_pid_lang.objToInt32();
            if (_lang == 0) _lang = 2;
            decimal pr_payed = 0;
            magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
            List<INV_TBL_PAYMENT> _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete == 1 && x.direction == 1).ToList();
            foreach (INV_TBL_PAYMENT _currPay in _listPay)
            {
                pr_payed += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
            }
            decimal pr_to_pay = _currTBL.pr_total.objToDecimal() - pr_payed;
            decimal pr_to_pay_owner = _currTBL.bcom_totalForOwner.objToDecimal() > 0 ? _currTBL.bcom_totalForOwner.objToDecimal() - pr_payed : pr_to_pay;
            decimal bcom_totalForOwner = _currTBL.bcom_totalForOwner.objToDecimal() > 0 ? _currTBL.bcom_totalForOwner.objToDecimal() : _currTBL.bcom_totalForOwner.objToDecimal();
            string _estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
            string _dtStartTime = _currTBL.dtStartTime.JSTime_stringToTime().JSTime_toString(false, true);
            string _body = "";
            string _subject = "";
            string extra_body = "";
            string extra_subject = "";
            string _subjectEcoSRS = "#dtStartTime# #estate_name# #resCode# IN: #dtStart# OUT: #dtEnd#";
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtStartTime#", _dtStartTime);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#estate_name#", _estate_name);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#resCode#", "" + _currTBL.code);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd#/#mm#/#yy#", _lang, " --/--/--"));
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd#/#mm#/#yy#", _lang, " --/--/--"));
            // mail to cl
            if (_toCL && (agentTbl == null || agentTbl.isMsgsEnabled == 1))
            {
                _subject = MailingUtilities.mailTemplate_subject("cl_abilita_pagamento", _lang, "");
                if (_subject == "" && _lang != 2)
                {
                    _lang = 2;
                    _subject = MailingUtilities.mailTemplate_subject("cl_abilita_pagamento", _lang, "");
                }
                if (_subject == "")
                {
                    _lang = 1;
                    _subject = MailingUtilities.mailTemplate_subject("cl_abilita_pagamento", _lang, "Error");
                }
                _body = MailingUtilities.mailTemplate_body("cl_abilita_pagamento", _lang, "Error");
                _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                //_body = _body.Replace("#estate_name#", _estate_name);
                //_body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                //_body = _body.Replace("#dtStartTime#", _dtStartTime);
                //_body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                //_body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                //if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                //    _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                //_body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                //_body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                //_body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                //_body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                //_body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
                //_body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                //_body = _body.Replace("#pr_total_owner#", "" + bcom_totalForOwner.ToString("N2") + "&nbsp;&euro;");
                //_body = _body.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
                //_body = _body.Replace("#pr_to_pay#", "" + pr_to_pay.ToString("N2") + "&nbsp;&euro;");
                //_body = _body.Replace("#pr_to_pay_owner#", "" + pr_to_pay_owner.ToString("N2") + "&nbsp;&euro;");
                //_body = _body.Replace("#resCode#", "" + _currTBL.code);
                //_body = _body.Replace("#resPwd#", "" + _currTBL.password);
                _body = _body.Replace("#reservationarea_auth_link#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                //  _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
                if (MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.cl_email, _bcc, "rntUtilities_rntReservation_mailPayBalanceWork cl"))
                    //if (MailingUtilities.autoSendMailTo(_subject, _body, "ns.narola@narolainfotech.com", false, "rntUtilities_rntReservation_mailPartPaymentReceiveWork cl"))
                    rntUtils.rntReservation_addState(_currTBL.id, 0, UserAuthentication.CurrentUserID, "Abilitato pagamento del Saldo", "");

            }

            #region non use


            //// mail to admin
            //_lang = 1;
            //_estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
            //if (_toAD)
            //{
            //    _subject = MailingUtilities.mailTemplate_subject("ad_payment_receive", _lang, "Error");
            //    _subject = _subject.Replace("#resCode#", "" + _currTBL.code);
            //    _body = MailingUtilities.mailTemplate_body("ad_payment_receive", _lang, "Error");
            //    _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
            //    _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
            //    _body = _body.Replace("#estate_name#", _estate_name);
            //    _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
            //    _body = _body.Replace("#dtStartTime#", _dtStartTime);
            //    _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            //    _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            //    if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
            //        _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
            //    _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
            //    _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
            //    _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
            //    _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
            //    _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
            //    _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
            //    _body = _body.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
            //    _body = _body.Replace("#pr_to_pay#", "" + pr_to_pay.ToString("N2") + "&nbsp;&euro;");
            //    _body = _body.Replace("#resCode#", "" + _currTBL.code);
            //    _body = _body.Replace("#resPwd#", "" + _currTBL.password);
            //    _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
            //    _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
            //    if (MailingUtilities.autoSendMailTo(_subject, _body, _messagesReceiver, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ad"))
            //        //if (MailingUtilities.autoSendMailTo(_subject, _body, "ns.narola@narolainfotech.com", false, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ad"))    
            //        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al admin <" + _messagesReceiver + ">", "");
            //    if (_currTBL.bcom_resId + "" != "" && _currTBL.bcom_roomResId + "" != "" && CommonUtilities.getSYS_SETTING("rntBcom_messagesConfirmedReceiver").isEmail() && MailingUtilities.autoSendMailTo(_subject, _body, CommonUtilities.getSYS_SETTING("rntBcom_messagesConfirmedReceiver"), _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ad"))
            //        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al admin <" + CommonUtilities.getSYS_SETTING("rntBcom_messagesConfirmedReceiver") + ">", "");

            //}
            //// mail to owner , srs
            //_subject = MailingUtilities.mailTemplate_subject("owner_res_confirmed", _lang, "Error");
            //_subject = _subject.Replace("#resCode#", "" + _currTBL.code);
            //_body = MailingUtilities.mailTemplate_body("owner_res_confirmed", _lang, "Error");
            //_body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
            //_body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
            //_body = _body.Replace("#cl_email#", "" + _currTBL.cl_email);
            //_body = _body.Replace("#estate_name#", _estate_name);
            //_body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
            //_body = _body.Replace("#estate_address#", _estate.loc_address);
            //_body = _body.Replace("#dtStartTime#", _dtStartTime);
            //_body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            //_body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            //if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
            //    _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
            //_body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
            //_body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
            //_body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
            //_body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
            //_body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
            //_body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
            //_body = _body.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
            //_body = _body.Replace("#pr_to_pay#", "" + pr_to_pay.ToString("N2") + "&nbsp;&euro;");
            //_body = _body.Replace("#resCode#", "" + _currTBL.code);
            //_body = _body.Replace("#resPwd#", "" + _currTBL.password);
            //_body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
            //_body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
            //_body = _body.Replace("#link_voucher#", "<a href=\"http://www.rentalinrome.com/pdfgenerator/pdf_rnt_reservation_voucher.aspx?uid=" + _currTBL.uid_2 + "\">Link al Voucher</a>");

            //var _bodyOwner = MailingUtilities.mailTemplate_body("owner_res_confirmed", _lang, "Error");
            //_bodyOwner = _bodyOwner.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
            //_bodyOwner = _bodyOwner.Replace("#cl_name_full#", _currTBL.cl_name_full);
            //_bodyOwner = _bodyOwner.Replace("#cl_email#", "" + _currTBL.cl_email);
            //_bodyOwner = _bodyOwner.Replace("#estate_name#", _estate_name);
            //_bodyOwner = _bodyOwner.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
            //_bodyOwner = _bodyOwner.Replace("#estate_address#", _estate.loc_address);
            //_bodyOwner = _bodyOwner.Replace("#dtStartTime#", _dtStartTime);
            //_bodyOwner = _bodyOwner.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            //_bodyOwner = _bodyOwner.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            //if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
            //    _bodyOwner = _bodyOwner.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
            //_bodyOwner = _bodyOwner.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
            //_bodyOwner = _bodyOwner.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
            //_bodyOwner = _bodyOwner.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
            //_bodyOwner = _bodyOwner.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
            //_bodyOwner = _bodyOwner.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
            //_bodyOwner = _bodyOwner.Replace("#pr_total#", "" + bcom_totalForOwner.objToDecimal().ToString("N2") + "&nbsp;&euro;");
            //_bodyOwner = _bodyOwner.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
            //_bodyOwner = _bodyOwner.Replace("#pr_to_pay#", "" + pr_to_pay_owner.ToString("N2") + "&nbsp;&euro;");
            //_bodyOwner = _bodyOwner.Replace("#resCode#", "" + _currTBL.code);
            //_bodyOwner = _bodyOwner.Replace("#resPwd#", "" + _currTBL.password);
            //_bodyOwner = _bodyOwner.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
            //_bodyOwner = _bodyOwner.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
            //if (agentTbl == null || agentTbl.sentVoucherToOwner.objToInt32() == 1)
            //    _bodyOwner = _bodyOwner.Replace("#link_voucher#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/pdfgenerator/pdf_rnt_reservation_voucherfo.aspx?uid=" + _currTBL.unique_id + _currTBL.uid_2 + "\">Link al Voucher</a>");
            //else
            //    _bodyOwner = _bodyOwner.Replace("#link_voucher#", "");
            //USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currTBL.cl_id);
            //if (_client != null)
            //{
            //    _body = _body.Replace("#cl_phone#", "" + _client.contact_phone);
            //}
            //if (_estate.pid_owner.HasValue && _estate.pid_owner != 1)
            //{
            //    // Accoglienza del proprietario
            //    if (_estate.is_srs != 1 && _toSRS && _estate.srs_ext_email != null && _estate.srs_ext_email != "")
            //        if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _estate.srs_ext_email, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ownersrs"))
            //            rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al accoglienza del prop <" + _estate.srs_ext_email + ">", "");
            //    if (_toOWN)
            //    {
            //        USR_TBL_OWNER _owner = maga_DataContext.DC_USER.USR_TBL_OWNER.SingleOrDefault(x => x.id == _estate.pid_owner);
            //        if (_owner != null)
            //        {
            //            if (_owner.contact_email != null && _owner.contact_email != "")
            //                if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner1"))
            //                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email + ">", "");
            //            if (_owner.contact_email_2 != null && _owner.contact_email_2 != "")
            //                if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_2, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner2"))
            //                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email_2 + ">", "");
            //            if (_owner.contact_email_3 != null && _owner.contact_email_3 != "")
            //                if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_3, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner3"))
            //                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email_3 + ">", "");
            //            if (_owner.contact_email_4 != null && _owner.contact_email_4 != "")
            //                if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_4, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner4"))
            //                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email_4 + ">", "");
            //            if (_owner.contact_email_5 != null && _owner.contact_email_5 != "")
            //                if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_5, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork owner5"))
            //                    rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al prop <" + _owner.contact_email_5 + ">", "");
            //        }
            //    }
            //}


            //if (_toSRS && _estate.is_srs == 1)
            //{
            //    //  Yefrey Guerrero Paredes;welcoming-4@areas.rentalinrome.com;1
            //    if (_estate.pid_zone == 1)
            //        if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-4@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-4@areas.rentalinrome.com"))
            //            rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-4@areas.rentalinrome.com", "");
            //    //Vladimir Trbovic;welcoming-5@areas.rentalinrome.com;5;6;17
            //    if (_estate.pid_zone == 5 || _estate.pid_zone == 6 || _estate.pid_zone == 17)
            //        if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-5@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-5@areas.rentalinrome.com"))
            //            rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-5@areas.rentalinrome.com", "");
            //    //Jonny;welcoming-3@areas.rentalinrome.com;4;18;19
            //    if (_estate.pid_zone == 4 || _estate.pid_zone == 9 || _estate.pid_zone == 18 || _estate.pid_zone == 19)
            //        if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-3@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-3@areas.rentalinrome.com"))
            //            rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-3@areas.rentalinrome.com", "");
            //    //Filipe Marques;welcoming-1@areas.rentalinrome.com;2;16
            //    if (_estate.pid_zone == 2 || _estate.pid_zone == 16)
            //        if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-1@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-1@areas.rentalinrome.com"))
            //            rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-1@areas.rentalinrome.com", "");
            //    //Alessandro Pancaldi;welcoming-2@areas.rentalinrome.com;3
            //    if (_estate.pid_zone == 3)
            //        if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "welcoming-2@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork welcoming-2@areas.rentalinrome.com"))
            //            rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS welcoming-2@areas.rentalinrome.com", "");
            //    //Alex;reception@rentalinrome.com;19
            //    if (_estate.pid_zone == 19)
            //        if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, "reception@areas.rentalinrome.com", _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork reception@areas.rentalinrome.com"))
            //            rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al SRS reception@areas.rentalinrome.com", "");
            //}
            //if (_toECO)
            //{
            //    if (_estate.eco_ext_email != null && _estate.eco_ext_email != "")
            //        if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _bodyOwner, _estate.eco_ext_email, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork ownereco"))
            //            rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma alla pulizia del prop <" + _estate.eco_ext_email + ">", "");
            //    var mailing_ecoMessagesReceiver = CommonUtilities.getSYS_SETTING("mailing_ecoMessagesReceiver");
            //    if (_estate.is_ecopulizie == 1 && mailing_ecoMessagesReceiver.isEmail())
            //        if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, mailing_ecoMessagesReceiver, _bcc, "rntUtilities_rntReservation_mailPartPaymentReceiveWork " + mailing_ecoMessagesReceiver))
            //            rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma alla pulizia eco <" + mailing_ecoMessagesReceiver + ">", "");
            //}

            #endregion
        }
        public rntReservation_mailPayBalanceWork(RNT_TBL_RESERVATION currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, bool toOWN, int currUser)
        {
            _currTBL = currTBL;
            _toCL = toCL;
            _toAD = toAD;
            _toSRS = toSRS;
            _toECO = toECO;
            _toOWN = toOWN;
            _currUser = currUser;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.rntReservation_mailPayBalanceWork resId:" + _currTBL.id);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }

    public static bool rntReservation_mailPayBalance(RNT_TBL_RESERVATION _currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, bool toOWN, int currUser)
    {
        rntReservation_mailPayBalanceWork _tmp = new rntReservation_mailPayBalanceWork(_currTBL, toCL, toAD, toSRS, toECO, toOWN, currUser);
        return true;
    }


    private class rntReservation_mailCancelledWork
    {
        RNT_TBL_RESERVATION _currTBL;
        bool _toCL;
        bool _toAD;
        bool _toOWN;
        bool _toSRS;
        bool _toECO;

        int _currUser;
        void doThread()
        {
            if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return;
            bool _bcc = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiverBCC") == "true";
            string _messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiver");
            var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);
            RNT_TB_ESTATE _estate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estate == null)
            {
                // MailingUtilities.autoSendMailTo("!!! Errore invio Mail Notifica", "La struttura rif: " + _currTBL.pid_estate + " non si trova!!!<br/>IdRes=" + _currTBL.id + "<br/>IdPayment=" + _pay.id, "adilet@magadesign.net", _bcc);
                //return;
            }
            int _lang = _currTBL.cl_pid_lang.objToInt32();
            if (_lang == 0) _lang = 2;
            decimal pr_payed = 0;
            magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
            List<INV_TBL_PAYMENT> _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete == 1 && x.direction == 1).ToList();
            foreach (INV_TBL_PAYMENT _currPay in _listPay)
            {
                pr_payed += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
            }
            decimal pr_to_pay = _currTBL.pr_total.objToDecimal() - pr_payed;
            decimal pr_to_pay_owner = _currTBL.bcom_totalForOwner.objToDecimal() > 0 ? _currTBL.bcom_totalForOwner.objToDecimal() - pr_payed : pr_to_pay;
            decimal bcom_totalForOwner = _currTBL.bcom_totalForOwner.objToDecimal() > 0 ? _currTBL.bcom_totalForOwner.objToDecimal() : _currTBL.bcom_totalForOwner.objToDecimal();
            string _estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
            string _dtStartTime = _currTBL.dtStartTime.JSTime_stringToTime().JSTime_toString(false, true);
            string _body = "";
            string _subject = "";
            string extra_body = "";
            string extra_subject = "";
            string _subjectEcoSRS = "#dtStartTime# #estate_name# #resCode# IN: #dtStart# OUT: #dtEnd#";
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtStartTime#", _dtStartTime);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#estate_name#", _estate_name);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#resCode#", "" + _currTBL.code);
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd#/#mm#/#yy#", _lang, " --/--/--"));
            _subjectEcoSRS = _subjectEcoSRS.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd#/#mm#/#yy#", _lang, " --/--/--"));
            // mail to cl
            if (_toCL && (agentTbl == null || agentTbl.isMsgsEnabled == 1))
            {
            }
            // mail to admin
            _lang = 1;
            _estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
            if (_toAD)
            {
            }
            // mail to owner , srs
            _subject = MailingUtilities.mailTemplate_subject("owner_res_cancelled", _lang, "Error");
            _subject = _subject.Replace("#resCode#", "" + _currTBL.code);
            _body = MailingUtilities.mailTemplate_body("owner_res_cancelled", _lang, "Error");
            _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
            _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
            _body = _body.Replace("#cl_email#", "" + _currTBL.cl_email);
            _body = _body.Replace("#estate_name#", _estate_name);
            _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
            _body = _body.Replace("#estate_address#", _estate.loc_address);
            _body = _body.Replace("#dtStartTime#", _dtStartTime);
            _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
            _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
            _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
            _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
            _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
            _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
            _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
            _body = _body.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
            _body = _body.Replace("#pr_to_pay#", "" + pr_to_pay.ToString("N2") + "&nbsp;&euro;");
            _body = _body.Replace("#resCode#", "" + _currTBL.code);
            _body = _body.Replace("#resPwd#", "" + _currTBL.password);
            _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
            _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
            _body = _body.Replace("#link_voucher#", "<a href=\"http://www.rentalinrome.com/pdfgenerator/pdf_rnt_reservation_voucher.aspx?uid=" + _currTBL.uid_2 + "\">Link al Voucher</a>");

            var _bodyOwner = MailingUtilities.mailTemplate_body("owner_res_cancelled", _lang, "Error");
            _bodyOwner = _bodyOwner.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
            _bodyOwner = _bodyOwner.Replace("#cl_name_full#", _currTBL.cl_name_full);
            _bodyOwner = _bodyOwner.Replace("#cl_email#", "" + _currTBL.cl_email);
            _bodyOwner = _bodyOwner.Replace("#estate_name#", _estate_name);
            _bodyOwner = _bodyOwner.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
            _bodyOwner = _bodyOwner.Replace("#estate_address#", _estate.loc_address);
            _bodyOwner = _bodyOwner.Replace("#dtStartTime#", _dtStartTime);
            _bodyOwner = _bodyOwner.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            _bodyOwner = _bodyOwner.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
            if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                _bodyOwner = _bodyOwner.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
            _bodyOwner = _bodyOwner.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
            _bodyOwner = _bodyOwner.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
            _bodyOwner = _bodyOwner.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
            _bodyOwner = _bodyOwner.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
            _bodyOwner = _bodyOwner.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
            _bodyOwner = _bodyOwner.Replace("#pr_total#", "" + bcom_totalForOwner.objToDecimal().ToString("N2") + "&nbsp;&euro;");
            _bodyOwner = _bodyOwner.Replace("#pr_payed#", "" + pr_payed.ToString("N2") + "&nbsp;&euro;");
            _bodyOwner = _bodyOwner.Replace("#pr_to_pay#", "" + pr_to_pay_owner.ToString("N2") + "&nbsp;&euro;");
            _bodyOwner = _bodyOwner.Replace("#resCode#", "" + _currTBL.code);
            _bodyOwner = _bodyOwner.Replace("#resPwd#", "" + _currTBL.password);
            _bodyOwner = _bodyOwner.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
            _bodyOwner = _bodyOwner.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
            if (agentTbl == null || agentTbl.sentVoucherToOwner.objToInt32() == 1)
                _bodyOwner = _bodyOwner.Replace("#link_voucher#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/pdfgenerator/pdf_rnt_reservation_voucherfo.aspx?uid=" + _currTBL.unique_id + _currTBL.uid_2 + "\">Link al Voucher</a>");
            else
                _bodyOwner = _bodyOwner.Replace("#link_voucher#", "");
            USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currTBL.cl_id);
            if (_client != null)
            {
                _body = _body.Replace("#cl_phone#", "" + _client.contact_phone);
            }
            if (_estate.pid_owner.HasValue && _estate.pid_owner != 1)
            {
                // Accoglienza del proprietario
                if (_estate.is_srs != 1 && _toSRS && _estate.srs_ext_email != null && _estate.srs_ext_email != "")
                    if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _estate.srs_ext_email, _bcc, "rntUtilities_rntReservation_mailCancelledWork ownersrs"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della cancellazione al accoglienza del prop <" + _estate.srs_ext_email + ">", "");
                if (_toOWN)
                {
                    USR_TBL_OWNER _owner = maga_DataContext.DC_USER.USR_TBL_OWNER.SingleOrDefault(x => x.id == _estate.pid_owner);
                    if (_owner != null)
                    {
                        if (_owner.contact_email != null && _owner.contact_email != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email, _bcc, "rntUtilities_rntReservation_mailCancelledWork owner1"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della cancellazione al prop <" + _owner.contact_email + ">", "");
                        if (_owner.contact_email_2 != null && _owner.contact_email_2 != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_2, _bcc, "rntUtilities_rntReservation_mailCancelledWork owner2"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della cancellazione al prop <" + _owner.contact_email_2 + ">", "");
                        if (_owner.contact_email_3 != null && _owner.contact_email_3 != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_3, _bcc, "rntUtilities_rntReservation_mailCancelledWork owner3"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della cancellazione al prop <" + _owner.contact_email_3 + ">", "");
                        if (_owner.contact_email_4 != null && _owner.contact_email_4 != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_4, _bcc, "rntUtilities_rntReservation_mailCancelledWork owner4"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della cancellazione al prop <" + _owner.contact_email_4 + ">", "");
                        if (_owner.contact_email_5 != null && _owner.contact_email_5 != "")
                            if (MailingUtilities.autoSendMailTo(_subject, _bodyOwner, _owner.contact_email_5, _bcc, "rntUtilities_rntReservation_mailCancelledWork owner5"))
                                rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della cancellazione al prop <" + _owner.contact_email_5 + ">", "");
                    }
                }
            }


            if (_toSRS && _estate.is_srs == 1)
            {
            }
            if (_toECO)
            {
                if (_estate.eco_ext_email != null && _estate.eco_ext_email != "")
                    if (MailingUtilities.autoSendMailTo(_subjectEcoSRS, _body, _estate.eco_ext_email, _bcc, "rntUtilities_rntReservation_mailCancelledWork ownereco"))
                        rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della cancellazione alla pulizia del prop <" + _estate.eco_ext_email + ">", "");
            }
        }
        public rntReservation_mailCancelledWork(RNT_TBL_RESERVATION currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, bool toOWN, int currUser)
        {
            _currTBL = currTBL;
            _toCL = toCL;
            _toAD = toAD;
            _toSRS = toSRS;
            _toECO = toECO;
            _toOWN = toOWN;
            _currUser = currUser;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.rntReservation_mailCancelledWork resId:" + currTBL.id);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static bool rntReservation_mailCancelled(RNT_TBL_RESERVATION _currTBL, bool toCL, bool toAD, bool toSRS, bool toECO, bool toOWN, int currUser)
    {
        rntReservation_mailCancelledWork _tmp = new rntReservation_mailCancelledWork(_currTBL, toCL, toAD, toSRS, toECO, toOWN, currUser);
        return true;
    }
    private class rntExtraReservation_mailPartPaymentReceiveWork
    {
        RNT_TBL_RESERVATION _currTBL;
        bool _toCL;
        bool _toAD;
        List<string> _lstOwner;
        int _currUser;
        void doThread()
        {
            try
            {
                if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return;
                bool _bcc = CommonUtilities.getSYS_SETTING("rnt_reservationConfirmed_messagesReceiverBCC") == "true";
                string _messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_service_reservationConfirmed_messagesReceiver");
                string cc__messagesReceiver = CommonUtilities.getSYS_SETTING("rnt_service_resConfirmed_cc_messagesReceiver");
                var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);
                RNT_TB_ESTATE _estate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
                if (_estate == null)
                {
                    // MailingUtilities.autoSendMailTo("!!! Errore invio Mail Notifica", "La struttura rif: " + _currTBL.pid_estate + " non si trova!!!<br/>IdRes=" + _currTBL.id + "<br/>IdPayment=" + _pay.id, "adilet@magadesign.net", _bcc);
                    //return;
                }
                int _lang = _currTBL.cl_pid_lang.objToInt32();
                if (_lang == 0) _lang = 2;

                string _estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
                string _dtStartTime = _currTBL.dtStartTime.JSTime_stringToTime().JSTime_toString(false, true);

                string extra_body = "";
                string extra_subject = "";

                // mail to cl
                if (_toCL && (agentTbl == null || agentTbl.isMsgsEnabled == 1))
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        //mailUtils.addLog("0", "client", "test", "test", null, null, null, false, 0, "est", false, "test", "test", "test", "test", "test");
                        List<dbRntReservationExtras> currService = dc.dbRntReservationExtras.Where(x => x.pidReservation == _currTBL.id).ToList();
                        if (currService != null && currService.Count > 0)
                        {
                            decimal totalPrice = 0;
                            decimal paidPrice = 0;
                            decimal pendingprice = 0;

                            foreach (dbRntReservationExtras objPrice in currService)
                            {
                                totalPrice = totalPrice.objToDecimal() + objPrice.TotalPrice.objToDecimal();
                                paidPrice = paidPrice.objToDecimal() + objPrice.PayNow.objToDecimal();
                            }

                            pendingprice = totalPrice - paidPrice;

                            extra_subject = MailingUtilities.mailTemplate_subject("cl_extraservice_payment_receive", _lang, "");
                            if (extra_subject == "" && _lang != 2)
                            {
                                _lang = 2;
                                extra_subject = MailingUtilities.mailTemplate_subject("cl_extraservice_payment_receive", _lang, "");
                            }
                            if (extra_subject == "")
                            {
                                _lang = 1;
                                extra_subject = MailingUtilities.mailTemplate_subject("cl_extraservice_payment_receive", _lang, "Error");
                            }
                            extra_body = MailingUtilities.mailTemplate_body("cl_extraservice_payment_receive", _lang, "Error");
                            extra_body = extra_body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                            extra_body = extra_body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                            extra_body = extra_body.Replace("#estate_name#", _estate_name);
                            extra_body = extra_body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                            extra_body = extra_body.Replace("#dtStartTime#", _dtStartTime);
                            extra_body = extra_body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                            extra_body = extra_body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                            if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                                extra_body = extra_body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                            extra_body = extra_body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                            extra_body = extra_body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                            extra_body = extra_body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                            extra_body = extra_body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                            extra_body = extra_body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
                            extra_body = extra_body.Replace("#pr_total#", "" + totalPrice.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                            extra_body = extra_body.Replace("#pr_payed#", "" + paidPrice.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                            extra_body = extra_body.Replace("#pr_to_pay#", "" + pendingprice.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                            extra_body = extra_body.Replace("#resCode#", "" + _currTBL.code);
                            extra_body = extra_body.Replace("#resPwd#", "" + _currTBL.password);
                            extra_body = extra_body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                            extra_body = extra_body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
                            List<dbRntReservationExtrasTMP> currServices = dc.dbRntReservationExtrasTMPs.Where(x => x.pidReservation == _currTBL.id).ToList();
                            string services = "";
                            if (currServices != null && currServices.Count > 0)
                            {

                                if (_lang == 1)
                                {
                                    services = "<table><tr><th>Data</th><th>Servizio</th><th>Adulti</th><th>Bambini</th><th>Totale Importo</th><th>Importo pagato<th></tr>";
                                }
                                else
                                {
                                    services = "<table><tr><th>Date</th><th>Service</th><th>Adults</th><th>Children</th><th>Total Amount</th><th>Paid Amount<th></tr>";
                                }
                                foreach (dbRntReservationExtrasTMP objService in currServices)
                                {
                                    services += "<tr><td>" + objService.inDate.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--") + "</td><td>" + CurrentSource.rntService_title(objService.pidExtras.objToInt32(), _lang, "Error") + "</td><td>" + objService.numPersAdult + "</td><td>" + objService.numPersChild + "</td><td>" + objService.Price + "</td><td>" + objService.Commission + "</td></tr>";

                                }
                                services += "</table>";

                            }
                            extra_body = extra_body.Replace("#extra_services#", "" + services);
                            MailingUtilities.autoSendMailTo(extra_subject, extra_body, _currTBL.cl_email, _bcc, "rntUtilities_rntServiceReservation_mailPartPaymentReceiveWork cl");
                            //MailingUtilities.autoSendMailTo(extra_subject, extra_body, "ns.narola@narolainfotech.com", _bcc, "rntUtilities_rntServiceReservation_mailPartPaymentReceiveWork cl");


                        }
                    }

                }


                // mail to admin
                _lang = 1;
                _estate_name = CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error");
                if (_toAD)
                {

                    //extra service email

                    using (DCmodRental dc = new DCmodRental())
                    {

                        List<dbRntReservationExtras> currService = dc.dbRntReservationExtras.Where(x => x.pidReservation == _currTBL.id).ToList();
                        if (currService != null && currService.Count > 0)
                        {

                            decimal totalPrice = 0;
                            decimal paidPrice = 0;
                            decimal pendingprice = 0;

                            foreach (dbRntReservationExtras objPrice in currService)
                            {
                                totalPrice = totalPrice.objToDecimal() + objPrice.TotalPrice.objToDecimal();
                                paidPrice = paidPrice.objToDecimal() + objPrice.PayNow.objToDecimal();
                            }

                            pendingprice = totalPrice - paidPrice;

                            extra_subject = MailingUtilities.mailTemplate_subject("ad_extraservice_payment_receive", _lang, "Error");
                            extra_subject = extra_subject.Replace("#resCode#", "" + _currTBL.code);
                            extra_body = MailingUtilities.mailTemplate_body("ad_extraservice_payment_receive", _lang, "Error");
                            extra_body = extra_body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                            extra_body = extra_body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                            extra_body = extra_body.Replace("#estate_name#", _estate_name);
                            extra_body = extra_body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                            extra_body = extra_body.Replace("#dtStartTime#", _dtStartTime);
                            extra_body = extra_body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                            extra_body = extra_body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                            if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                                extra_body = extra_body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                            extra_body = extra_body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                            extra_body = extra_body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                            extra_body = extra_body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                            extra_body = extra_body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                            extra_body = extra_body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
                            extra_body = extra_body.Replace("#pr_total#", "" + totalPrice.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                            extra_body = extra_body.Replace("#pr_payed#", "" + paidPrice.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                            extra_body = extra_body.Replace("#pr_to_pay#", "" + pendingprice.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                            extra_body = extra_body.Replace("#resCode#", "" + _currTBL.code);
                            extra_body = extra_body.Replace("#resPwd#", "" + _currTBL.password);
                            extra_body = extra_body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                            extra_body = extra_body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
                            List<dbRntReservationExtrasTMP> currServices = dc.dbRntReservationExtrasTMPs.Where(x => x.pidReservation == _currTBL.id).ToList();
                            string services = "";
                            string service_id = "";
                            if (currServices != null && currServices.Count > 0)
                            {

                                if (_lang == 1)
                                {
                                    services = "<table><tr><th>Data</th><th>Servizio</th><th>Adulti</th><th>Bambini</th><th>Totale Importo</th><th>Importo pagato<th></tr>";
                                }
                                else
                                {
                                    services = "<table><tr><th>Date</th><th>Service</th><th>Adults</th><th>Children</th><th>Total Amount</th><th>Paid Amount<th></tr>";
                                }
                                foreach (dbRntReservationExtrasTMP objService in currServices)
                                {
                                    services += "<tr><td>" + objService.inDate.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--") + "</td><td>" + CurrentSource.rntService_title(objService.pidExtras.objToInt32(), _lang, "Error") + "</td><td>" + objService.numPersAdult + "</td><td>" + objService.numPersChild + "</td><td>" + objService.Price + "</td><td>" + objService.Commission + "</td></tr>";
                                    if (service_id == "")
                                    {
                                        service_id = Convert.ToString(objService.pidExtras);
                                    }
                                    else
                                    {
                                        service_id += "," + Convert.ToString(objService.pidExtras);
                                    }

                                }
                                services += "</table>";

                            }
                            extra_body = extra_body.Replace("#extra_services#", "" + services);
                            extra_body = extra_body.Replace("#extra_id#", "" + service_id);
                            MailingUtilities.autoSendMailTo(extra_subject, extra_body, _messagesReceiver, false, "rntUtilities_rntServiceReservation_mailPartPaymentReceiveWork ad");
                            MailingUtilities.autoSendMailTo(extra_subject, extra_body, cc__messagesReceiver, false, "rntUtilities_rntServiceReservation_mailPartPaymentReceiveWork ad");
                            //MailingUtilities.autoSendMailTo(extra_subject, extra_body, "ns.narola@narolainfotech.com", false, "rntUtilities_rntServiceReservation_mailPartPaymentReceiveWork ad");
                            //rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al admin <" + _messagesReceiver + ">", "");

                        }
                    }



                }

                //mail to extra service owner

                if (_lstOwner != null && _lstOwner.Count > 0)
                {
                    foreach (string objOwner in _lstOwner)
                    {
                        using (DCmodRental dc = new DCmodRental())
                        {
                            List<dbRntReservationExtras> currService = dc.dbRntReservationExtras.Where(x => x.pidReservation == _currTBL.id).ToList();
                            if (currService != null)
                            {
                                decimal totalPrice = 0;
                                decimal paidPrice = 0;
                                decimal pendingprice = 0;

                                extra_subject = MailingUtilities.mailTemplate_subject("owner_extraservice_payment_receive", _lang, "Error");
                                extra_subject = extra_subject.Replace("#resCode#", "" + _currTBL.code);
                                extra_body = MailingUtilities.mailTemplate_body("owner_extraservice_payment_receive", _lang, "Error");
                                extra_body = extra_body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
                                extra_body = extra_body.Replace("#cl_name_full#", _currTBL.cl_name_full);
                                extra_body = extra_body.Replace("#estate_name#", _estate_name);
                                extra_body = extra_body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
                                extra_body = extra_body.Replace("#dtStartTime#", _dtStartTime);
                                extra_body = extra_body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                                extra_body = extra_body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
                                if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
                                    extra_body = extra_body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
                                extra_body = extra_body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
                                extra_body = extra_body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
                                extra_body = extra_body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
                                extra_body = extra_body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
                                extra_body = extra_body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));

                                extra_body = extra_body.Replace("#resCode#", "" + _currTBL.code);
                                extra_body = extra_body.Replace("#resPwd#", "" + _currTBL.password);
                                extra_body = extra_body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
                                extra_body = extra_body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
                                string services = "";
                                string service_id = "";
                                //owner email
                                dbRntExtraOwnerTBL currOwner = dc.dbRntExtraOwnerTBLs.SingleOrDefault(x => x.id == objOwner.objToInt32());
                                if (currOwner != null)
                                {
                                    if (currOwner.contactEmail != null && currOwner.contactEmail != "")
                                    {


                                        List<dbRntEstateExtrasTB> lstExtra = dc.dbRntEstateExtrasTBs.Where(x => x.pidOwner == objOwner.objToInt32()).ToList();
                                        if (lstExtra != null && lstExtra.Count > 0)
                                        {
                                            List<int> lstextrasId = new List<int>();
                                            foreach (dbRntEstateExtrasTB extra in lstExtra)
                                            {
                                                lstextrasId.Add(extra.id);
                                            }

                                            List<dbRntReservationExtrasTMP> currServices = dc.dbRntReservationExtrasTMPs.Where(x => x.pidReservation == _currTBL.id && lstextrasId.Contains(x.pidExtras.Value)).ToList();
                                            if (currServices != null && currServices.Count > 0)
                                            {

                                                if (_lang == 1)
                                                {
                                                    services = "<table><tr><th>Data</th><th>Servizio</th><th>Adulti</th><th>Bambini</th><th>Totale Importo</th><th>Importo pagato<th></tr>";
                                                }
                                                else
                                                {
                                                    services = "<table><tr><th>Date</th><th>Service</th><th>Adults</th><th>Children</th><th>Total Amount</th><th>Paid Amount<th></tr>";
                                                }
                                                foreach (dbRntReservationExtrasTMP objService in currServices)
                                                {
                                                    services += "<tr><td>" + objService.inDate.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--") + "</td><td>" + CurrentSource.rntService_title(objService.pidExtras.objToInt32(), _lang, "Error") + "</td><td>" + objService.numPersAdult + "</td><td>" + objService.numPersChild + "</td><td>" + objService.Price + "</td><td>" + objService.Commission + "</td></tr>";
                                                    totalPrice = totalPrice.objToDecimal() + objService.Price.objToDecimal();
                                                    paidPrice = paidPrice.objToDecimal() + objService.Commission.objToDecimal();

                                                    if (service_id == "")
                                                    {
                                                        service_id = Convert.ToString(objService.pidExtras);
                                                    }
                                                    else
                                                    {
                                                        service_id += "," + Convert.ToString(objService.pidExtras);
                                                    }

                                                }
                                                pendingprice = totalPrice - paidPrice;
                                                services += "</table>";


                                                extra_body = extra_body.Replace("#pr_total#", "" + totalPrice.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                                                extra_body = extra_body.Replace("#pr_payed#", "" + paidPrice.objToDecimal().ToString("N2") + "&nbsp;&euro;");
                                                extra_body = extra_body.Replace("#pr_to_pay#", "" + pendingprice.ToString("N2") + "&nbsp;&euro;");
                                                extra_body = extra_body.Replace("#extra_services#", "" + services);
                                                extra_body = extra_body.Replace("#extra_id#", "" + service_id);
                                                MailingUtilities.autoSendMailTo(extra_subject, extra_body, currOwner.contactEmail, _bcc, "rntUtilities_rntServiceReservation_mailPartPaymentReceiveWork own");
                                                //MailingUtilities.autoSendMailTo(extra_subject, extra_body, "ns.narola@narolainfotech.com", false, "rntUtilities_rntServiceReservation_mailPartPaymentReceiveWork own");
                                                //rntUtils.rntReservation_addState(_currTBL.id, 0, _currUser, "Invio notifica della conferma al admin <" + _messagesReceiver + ">", "");

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                mailUtils.addLog("0", ex.ToString(), "test", "test", null, null, null, false, 0, "est", false, "test", "test", "test", "test", "test");
            }
        }
        public rntExtraReservation_mailPartPaymentReceiveWork(RNT_TBL_RESERVATION currTBL, bool toCL, bool toAD, List<string> lstowner, int currUser)
        {
            _currTBL = currTBL;
            _toCL = toCL;
            _toAD = toAD;
            _lstOwner = lstowner;
            _currUser = currUser;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.rntExtraReservation_mailPartPaymentReceiveWork resId:" + currTBL.id);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();



        }

    }
    public static bool rntExtraReservation_mailPartPaymentReceive(RNT_TBL_RESERVATION _currTBL, bool toCL, bool toAD, List<string> lstOwner, int currUser)
    {
        rntExtraReservation_mailPartPaymentReceiveWork _tmp = new rntExtraReservation_mailPartPaymentReceiveWork(_currTBL, toCL, toAD, lstOwner, currUser);
        return true;
    }

    public static bool rntReservationMailForComments(RNT_TBL_RESERVATION _currTBL, int currUser)
    {
        int _lang = _currTBL.cl_pid_lang.objToInt32();
        if (_lang == 0) _lang = 2;
        // mail to cl
        var email = _currTBL.cl_email;
        var cl_name_honorific = _currTBL.cl_name_honorific;
        var cl_name_full = _currTBL.cl_name_full;
        var agentClientsPromoCode = "";
        var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);
        if (agentTbl != null)
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                var client = dc.dbAuthClientTBLs.SingleOrDefault(x => x.id == _currTBL.agentClientID);

                if (client == null) return false;
                if (client.contactEmail + "" == "" || !client.contactEmail.isEmail()) return false;

                email = client.contactEmail;
                cl_name_honorific = client.nameHonorific;
                cl_name_full = client.nameFull;
                _lang = client.pidLang.objToInt32();
                if (_lang == 0) _lang = 2;
                agentClientsPromoCode = contUtils.getLabel_title("rntReservationMailForComments_agentClientsPromoCode", _lang, "");
            }
        }
        string _subject = MailingUtilities.mailTemplate_subject("cl_leavecomment_aftercheckout", _lang, "");
        if (_subject == "" && _lang != 2)
        {
            _lang = 2;
            _subject = MailingUtilities.mailTemplate_subject("cl_leavecomment_aftercheckout", _lang, "");
        }
        if (_subject == "")
        {
            _lang = 1;
            _subject = MailingUtilities.mailTemplate_subject("cl_leavecomment_aftercheckout", _lang, "Error");
        }
        _subject = _subject.Replace("#resCode#", _currTBL.code);
        _subject = _subject.Replace("#estate_name#", CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error"));
        string _body = MailingUtilities.mailTemplate_body("cl_leavecomment_aftercheckout", _lang, "Error");
        _body = _body.Replace("#cl_name_honorific#", cl_name_honorific);
        _body = _body.Replace("#cl_name_full#", cl_name_full);
        _body = _body.Replace("#estate_name#", CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error"));
        _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
        _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
        _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
        if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
            _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
        _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
        _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
        _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
        _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
        _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
        _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
        _body = _body.Replace("#resCode#", "" + _currTBL.code);
        _body = _body.Replace("#resPwd#", "" + _currTBL.password);
        _body = _body.Replace("#link_guestbook#", "<a href=\"" + CurrentAppSettings.HOST + CurrentSource.getPagePath("11", "stp", _lang.ToString()) + "?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
        _body = _body.Replace("#agentClientsPromoCode#", agentClientsPromoCode);
        if (MailingUtilities.autoSendMailTo(_subject, _body, email, false, "rntUtils.rntReservationMailForComments"))
            rntUtils.rntReservation_addState(_currTBL.id, 10, currUser, "Invitato cliente a lasciare commento dopo il checkout", "");
        return true;
    }
    public static bool rntReservation_mailReminder(RNT_TBL_RESERVATION _currTBL)
    {
        if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return true;
        var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);
        if (agentTbl != null && agentTbl.isMsgsEnabled != 1) return false;
        int _lang = _currTBL.cl_pid_lang.objToInt32();
        if (_lang == 0) _lang = 2;
        // mail to cl

        if (_currTBL.isSendBalancePayementEmail != 1)
            return true;

        string _subject = MailingUtilities.mailTemplate_subject("cl_res_reminder", _lang, "");
        if (_subject == "" && _lang != 2)
        {
            _lang = 2;
            _subject = MailingUtilities.mailTemplate_subject("cl_res_reminder", _lang, "");
        }
        if (_subject == "")
        {
            _lang = 1;
            _subject = MailingUtilities.mailTemplate_subject("cl_res_reminder", _lang, "Error");
        }
        _subject = _subject.Replace("#resCode#", _currTBL.code);
        string _body = MailingUtilities.mailTemplate_body("cl_res_reminder", _lang, "Error");
        if (_currTBL.id < 150000)
            _body = MailingUtilities.mailTemplate_body("cl_res_reminderOld", _lang, "Error");
        _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
        _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
        _body = _body.Replace("#estate_name#", CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error"));
        _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
        _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
        _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
        if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
            _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
        _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
        _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
        _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
        _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
        _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
        _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
        _body = _body.Replace("#resCode#", "" + _currTBL.code);
        _body = _body.Replace("#resPwd#", "" + _currTBL.password);
        _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
        _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST_SSL + "/reservationarea\">" + CurrentAppSettings.HOST_SSL + "/reservationarea</a>");
        if (MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.cl_email, false, "rntUtilities_rntReservation_mailReminder"))
            rntUtils.rntReservation_addState(_currTBL.id, 0, 1, "Invio reminder al cliente", "");
        return true;
    }
    public static bool rntReservation_mailExpired(RNT_TBL_RESERVATION _currTBL)
    {
        if (CommonUtilities.getSYS_SETTING("sys_online") == "false") return true;
        var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);
        if (agentTbl != null && agentTbl.isMsgsEnabled != 1) return false;
        int _lang = _currTBL.cl_pid_lang.objToInt32();
        if (_lang == 0) _lang = 2;
        // mail to cl
        string _subject = MailingUtilities.mailTemplate_subject("cl_res_expired", _lang, "");
        if (_subject == "" && _lang != 2)
        {
            _lang = 2;
            _subject = MailingUtilities.mailTemplate_subject("cl_res_expired", _lang, "");
        }
        if (_subject == "")
        {
            _lang = 1;
            _subject = MailingUtilities.mailTemplate_subject("cl_res_expired", _lang, "Error");
        }
        _subject = _subject.Replace("#resCode#", _currTBL.code);
        string _body = MailingUtilities.mailTemplate_body("cl_res_expired", _lang, "Error");
        _body = _body.Replace("#cl_name_honorific#", _currTBL.cl_name_honorific);
        _body = _body.Replace("#cl_name_full#", _currTBL.cl_name_full);
        _body = _body.Replace("#estate_name#", CurrentSource.rntEstate_title(_currTBL.pid_estate.objToInt32(), _lang, "Error"));
        _body = _body.Replace("#estate_id#", "" + _currTBL.pid_estate.objToInt32());
        _body = _body.Replace("#dtStart#", "" + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
        _body = _body.Replace("#dtEnd#", "" + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", _lang, " --/--/--"));
        if (_currTBL.dtEnd.HasValue && _currTBL.dtStart.HasValue)
            _body = _body.Replace("#dtCount#", "" + (_currTBL.dtEnd.Value.Date - _currTBL.dtStart.Value.Date).Days);
        _body = _body.Replace("#num_pers_total#", "" + (_currTBL.num_adult + _currTBL.num_child_over + _currTBL.num_child_min));
        _body = _body.Replace("#num_adult#", "" + (_currTBL.num_adult.objToInt32()));
        _body = _body.Replace("#num_child_over#", "" + (_currTBL.num_child_over.objToInt32()));
        _body = _body.Replace("#num_child_min#", "" + (_currTBL.num_child_min.objToInt32()));
        _body = _body.Replace("#payment_review#", "" + rntReservation_mailBody_prices(_currTBL));
        _body = _body.Replace("#pr_total#", "" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;");
        _body = _body.Replace("#resCode#", "" + _currTBL.code);
        _body = _body.Replace("#resPwd#", "" + _currTBL.password);
        _body = _body.Replace("#link_reservation_details_auto#", "<a href=\"" + CurrentAppSettings.HOST + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "\">" + CurrentSource.getSysLangValue("lblHere", _lang, "Here") + "</a>");
        _body = _body.Replace("#link_reservation_details#", "<a href=\"" + CurrentAppSettings.HOST + "/reservationarea\">" + CurrentAppSettings.HOST + "/reservationarea</a>");
        if (MailingUtilities.autoSendMailTo(_subject, _body, _currTBL.cl_email, false, "rntUtilities_rntReservation_mailExpired"))
            rntUtils.rntReservation_addState(_currTBL.id, 0, 1, "Invio notifica della cancellazione al cliente", "");
        return true;
    }
}