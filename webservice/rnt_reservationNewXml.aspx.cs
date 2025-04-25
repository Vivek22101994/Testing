using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.webservice
{
    public partial class rnt_reservationNewXml : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        private string CURRENT_SESSION_ID;
        private int IdEstate;
        private RNT_TB_ESTATE currTBL;
        private int _currLang;
        private long agentID;
        private clSearch _ls;
        private rntExts.PreReservationPrices currOutPrice;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                CURRENT_SESSION_ID = Request["SESSION_ID"];
                _ls = new clSearch();
                _currLang = Request["lang"].objToInt32() == 0 ? CurrentLang.ID : Request["lang"].objToInt32();
                IdEstate = Request["id"].objToInt32();
                currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currTBL == null)
                {
                    Response.Write("<error>No data found</error>");
                    Response.End();
                }
                agentID = Request["agentID"].objToInt64();
                int dtStartInt = Request["dtS"].objToInt32();
                int dtEndInt = Request["dtE"].objToInt32();
                if (dtStartInt != 0 && dtEndInt != 0)
                {
                    _ls.dtStart = dtStartInt.JSCal_intToDate();
                    _ls.dtEnd = dtEndInt.JSCal_intToDate();
                }
                _ls.dtCount = (_ls.dtEnd - _ls.dtStart).TotalDays.objToInt32();
                _ls.numPers_adult = Request["numPers_adult"].objToInt32() == 0 ? _ls.numPers_adult : Request["numPers_adult"].objToInt32();
                _ls.numPers_childOver = Request["numPers_childOver"].objToInt32() == 0 ? _ls.numPers_childOver : Request["numPers_childOver"].objToInt32();
                _ls.numPers_childMin = Request["numPers_childMin"].objToInt32() == 0 ? _ls.numPers_childMin : Request["numPers_childMin"].objToInt32();
                _ls.numPersCount = _ls.numPers_adult + _ls.numPers_childOver;
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                _config.lastSearch = _ls;
                _config.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(_config);

                saveReservation();
            }
        }
        protected void saveReservation()
        {
            if (!checkAvailability())
            {
                Response.Write("<error>Not Available</error>");
                Response.End();
                return;
            }
            int client_id = Request["client_id"].ToInt32();

            string client_loc_country = "" + Request["client_loc_country"];
            string client_contact_email = "" + Request["client_contact_email"];
            string client_name_honorific = "" + Request["client_name_honorific"];
            string client_name_full = "" + Request["client_name_full"];
            string client_contact_phone_mobile = "" + Request["client_contact_phone_mobile"];
            USR_TBL_CLIENT _client;
            var DC_USER = maga_DataContext.DC_USER;
            if (client_id == 0)
            {
                _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.is_deleted != 1 && x.contact_email == client_contact_email);
                if (_client != null && _client.is_active == 1)
                {
                    Response.Write("<error>Exists</error>");
                    Response.End();
                    return;
                }
                if (_client != null && _client.is_active != 1)
                {
                    Response.Write("<error>Disabled</error>");
                    Response.End();
                    return;
                }
                _client = new USR_TBL_CLIENT();
                _client.loc_country = client_loc_country;
                _client.contact_email = client_contact_email;
                _client.name_honorific = client_name_honorific;
                _client.name_full = client_name_full;
                _client.contact_phone_mobile = client_contact_phone_mobile;
                _client.pid_lang = CurrentLang.ID;
                _client.isCompleted = 0;
                _client.is_deleted = 0;
                _client.is_active = 1;
                _client.date_created = DateTime.Now;
                _client.login = _client.contact_email;
                _client.password = CommonUtilities.CreatePassword(8, false, true, false);
                DC_USER.USR_TBL_CLIENTs.InsertOnSubmit(_client);
                DC_USER.SubmitChanges();
                _client.code = _client.id.ToString().fillString("0", 7, false);
                DC_USER.SubmitChanges();
                AdminUtilities.usrClient_mailNewCreation(_client.id); // send mails
            }
            else
            {
                _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.is_deleted != 1 && x.contact_email == client_contact_email && x.id == client_id);
                if (_client == null)
                {
                    Response.Write("<error>WrongUsernameOrPassword</error>");
                    Response.End();
                    return;
                }
                if (_client.is_active != 1)
                {
                    Response.Write("<error>Disabled</error>");
                    Response.End();
                    return;
                }
            }
            RNT_TBL_RESERVATION _currTBL = rntUtils.newReservation();
            _currTBL.pid_estate = IdEstate;
            _currTBL.pidEstateCity = currTBL.pid_city.objToInt32();
            _currTBL.is_deleted = 0;
            _currTBL.pid_creator = 5; // creato da Mobile App
            _currTBL.state_pid = 6;
            _currTBL.state_body = "";
            _currTBL.state_date = DateTime.Now;
            _currTBL.state_pid_user = 1;
            _currTBL.state_subject = "Nuovo dal app mobile";
            _currTBL.is_booking = 1;

            _currTBL.dtStart = _ls.dtStart;
            _currTBL.dtEnd = _ls.dtEnd;
            _currTBL.dtStartTime = "000000";
            _currTBL.dtEndTime = "000000";

            _currTBL.num_adult = _ls.numPers_adult;
            _currTBL.num_child_over = _ls.numPers_childOver;
            _currTBL.num_child_min = _ls.numPers_childMin;

            currOutPrice.CopyTo(ref _currTBL);

            _currTBL.pr_deposit = currTBL.pr_deposit;
            _currTBL.srs_ext_meetingPoint = currTBL.srs_ext_meetingPoint;
            _currTBL.pr_depositWithCard = currTBL.pr_depositWithCard;

            _currTBL.pr_discount_owner = 0;
            _currTBL.pr_discount_commission = 0;
            _currTBL.pr_discount_desc = "";

            _currTBL.IdAdMedia = App.IdAdMedia;
            _currTBL.IdLink = App.IdLink;
            // salviamo prima e aggiorniamo la cache per evitare overbooking
            DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);


            // scadenze instant booking
            int _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHoursMobile").ToInt32();
            if (_blockHours == 0) _blockHours = 2;
            _currTBL.block_comments = "Scadenza predefinita MobileBooking [" + _blockHours + " ore]";
            if (_currTBL.agentID != 0)
            {
                _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHoursAgent").ToInt32();
                _currTBL.block_comments = "Scadenza predefinita Agenzie [" + _blockHours + " ore]";
            }
            _currTBL.block_expire = DateTime.Now.AddHours(_blockHours);
            _currTBL.block_expire_hours = _blockHours;
            _currTBL.block_pid_user = 1;

            _currTBL.cl_id = _client.id;
            _currTBL.cl_email = _client.contact_email;
            _currTBL.cl_name_full = _client.name_full;
            _currTBL.cl_name_honorific = _client.name_honorific;
            _currTBL.cl_loc_country = _client.loc_country;
            _currTBL.cl_pid_discount = _client.pid_discount;
            _currTBL.cl_pid_lang = _client.pid_lang;
            _currTBL.cl_isCompleted = _client.isCompleted;

            _currTBL.cl_browserInfo = "Mobile App";
            _currTBL.cl_browserIP = Request.browserIP();

            // non puo venire da una richiesta
            if (1==2)
            {
                RNT_TBL_REQUEST _request = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == 0);
                _currTBL.pid_related_request = _request.id;
                _currTBL.pid_operator = _request.pid_operator;
                _request.pid_reservation = _currTBL.id;
                _request.state_body = "";
                _request.state_subject = "Diventato prenotazione";
                _request.state_date = DateTime.Now;
                _request.state_pid = 5;
                _request.state_pid_user = 1;
                List<RNT_TBL_REQUEST> _list = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.pid_related_request == _request.id).ToList();
                foreach (RNT_TBL_REQUEST _req in _list)
                {
                    _req.state_body = _request.state_body;
                    _req.state_subject = _request.state_subject;
                    _req.state_date = _request.state_date;
                    _req.state_pid = _request.state_pid;
                    _req.state_pid_user = _request.state_pid_user;
                    _req.pid_reservation = _request.pid_reservation;
                }
            }
            else
            {
                // abbinare all'account dell'agenzia, in caso di scaccini o maurizio lecce a rental, id= 37
                int pidOperator = 0;
                if (_currTBL.agentID != 0)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                        if (agentTBL != null)
                        {
                            pidOperator = agentTBL.pidReferer.objToInt32();
                        }
                    }
                }
                if (pidOperator == 0) pidOperator = AdminUtilities.usr_getAvailableOperator(AdminUtilities.zone_countryId(_client.loc_country), _currTBL.cl_pid_lang.objToInt32());
                if (pidOperator == 15 && pidOperator == 31) pidOperator = 37;
                _currTBL.pid_operator = pidOperator;
            }
            _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
            _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            rntUtils.rntReservation_setDefaults(ref _currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            rntUtils.rntReservation_mailNewCreation(_currTBL, true, true, true, true, 1); // send mails
            rntUtils.reservation_checkPartPayment(_currTBL, false);
            Response.Write("<okurl>" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "</okurl>");
            //Response.Write("<okurl>" + CurrentAppSettings.HOST_SSL + "/reservationarea/mobile/login.aspx?auth=" + _currTBL.unique_id + "</okurl>");
            //Response.Write("<okurl>" + CurrentAppSettings.HOST_SSL + "/util_paypal_redirect.aspx?mobile=true&type=payment&code=" + _pay.code + "&lang=" + CurrentLang.ID + "</okurl>");
            Response.End();
        }
        public bool checkAvailability()
        {
            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = _ls.dtStart;
            outPrice.dtEnd = _ls.dtEnd;
            outPrice.numPersCount = _ls.numPersCount;
            outPrice.pr_discount_owner = 0;
            outPrice.pr_discount_commission = 0;
            outPrice.part_percentage = currTBL.pr_percentage.objToDecimal();

            decimal _pr_total = rntUtils.rntEstate_getPrice(0, IdEstate, ref outPrice);
            bool _isAvailable = DC_RENTAL.RNT_TBL_RESERVATION.Where(y => y.pid_estate == IdEstate //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= _ls.dtStart && y.dtEnd.Value.Date >= _ls.dtEnd) //
                                                                                || (y.dtStart.Value.Date >= _ls.dtStart && y.dtStart.Value.Date < _ls.dtEnd) //
                                                                                || (y.dtEnd.Value.Date > _ls.dtStart && y.dtEnd.Value.Date <= _ls.dtEnd))).Count() == 0;
            outPrice.prTotal = _pr_total;
            currOutPrice = outPrice;
            return _isAvailable && _pr_total > 0;
        }
    }
}
