using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.admin
{
    public partial class rnt_reservation_form : adminBasePage
    {
        private magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_RESERVATION _currTBL;
        private magaInvoice_DataContext DC_INVOICE;
        public long IdReservation
        {
            get { return HF_id.Value.ToInt32(); }
            set { HF_id.Value = value.ToString(); }
        }
        public long IdRequest
        {
            get { return HF_IdRequest.Value.ToInt64(); }
            set { HF_IdRequest.Value = value.ToString(); }
        }
        public int dtCount
        {
            get
            {
                return HF_dtCount.Value.ToInt32();
            }
            set
            {
                HF_dtCount.Value = value.ToString();
            }
        }
        private rntExts.PreReservationPrices TMPcurrOutPrice;
        public rntExts.PreReservationPrices currOutPrice
        {
            get
            {
                if (TMPcurrOutPrice == null)
                    TMPcurrOutPrice = (rntExts.PreReservationPrices)ViewState["_currOutPrice"];
                return TMPcurrOutPrice ?? new rntExts.PreReservationPrices();
            }
            set { TMPcurrOutPrice = value; ViewState["_currOutPrice"] = TMPcurrOutPrice; }
        }
        protected List<RNT_TBL_RESERVATION> _resList;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_reservation_planner";
            UC_estate.onModify += new EventHandler(UC_estate_onModify);
            UC_estate.onSave += new EventHandler(UC_estate_onSave);
            UC_estate.onCancel += new EventHandler(UC_estate_onCancel);

            UC_state.onModify += new EventHandler(UC_state_onModify);
            UC_state.onSave += new EventHandler(UC_state_onSave);
            UC_state.onCancel += new EventHandler(UC_state_onCancel);

            UC_client.onModify += new EventHandler(UC_client_onModify);
            UC_client.onSave += new EventHandler(UC_client_onSave);
            UC_client.onCancel += new EventHandler(UC_client_onCancel);

            UC_dt_pers.onModify += new EventHandler(UC_dt_pers_onModify);
            UC_dt_pers.onSave += new EventHandler(UC_dt_pers_onSave);
            UC_dt_pers.onCancel += new EventHandler(UC_dt_pers_onCancel);

            UC_notes.onModify += new EventHandler(UC_notes_onModify);
            UC_notes.onSave += new EventHandler(UC_notes_onSave);
            UC_notes.onCancel += new EventHandler(UC_notes_onCancel);

            ucAgent.onModify += new EventHandler(ucAgent_onModify);
            ucAgent.onSave += new EventHandler(ucAgent_onSave);
            ucAgent.onCancel += new EventHandler(ucAgent_onCancel);

        }
        void UC_estate_onModify(object sender, EventArgs e)
        {
            UC_estate.IsEdit = true;
            lockAll(true);
            UC_estate.IsLocked = false;
            lnk_calculatePrice.Visible = false;
            checkSave();
        }
        void UC_estate_onSave(object sender, EventArgs e)
        {
            UC_estate.IsEdit = false;
            UC_dt_pers._checkCalDates = rntUtils.rntEstate_availableDatesForJSCal(UC_estate.IdEstate, IdReservation);
            UC_dt_pers.num_persons_child = UC_estate.num_persons_child;
            UC_dt_pers.num_persons_min = UC_estate.num_persons_min;
            UC_dt_pers.num_persons_max = UC_estate.num_persons_max;
            UC_dt_pers.nights_min = UC_estate.nights_min;
            lockAll(false);
            lnk_calculatePrice.Visible = true;
            checkAvailability();
            checkSave();
        }
        void UC_estate_onCancel(object sender, EventArgs e)
        {
            UC_estate.IsEdit = false;
            lockAll(false);
            lnk_calculatePrice.Visible = true;
            checkSave();
        }
        void UC_state_onModify(object sender, EventArgs e)
        {
            UC_state.IsEdit = true;
            lockAll(true);
            UC_state.IsLocked = false;
            lnk_calculatePrice.Visible = false;
            checkSave();
        }
        void UC_state_onSave(object sender, EventArgs e)
        {
            UC_state.IsEdit = false;
            lockAll(false);
            lnk_calculatePrice.Visible = true;
            checkAvailability();
            checkSave();
        }
        void UC_state_onCancel(object sender, EventArgs e)
        {
            UC_state.IsEdit = false;
            lockAll(false);
            lnk_calculatePrice.Visible = true;
            checkSave();
        }
        void UC_client_onModify(object sender, EventArgs e)
        {
            lockAll(true);
            UC_client.IsLocked = false;
            UC_client.IsEdit = true;
            checkSave();
        }
        void UC_client_onSave(object sender, EventArgs e)
        {
            UC_client.IsEdit = false;
            lockAll(false);
            checkSave();
        }
        void UC_client_onCancel(object sender, EventArgs e)
        {
            UC_client.IsEdit = false;
            lockAll(false);
            checkSave();
        }
        void ucAgent_onModify(object sender, EventArgs e)
        {
            ucAgent.IsEdit = true;
            lockAll(true);
            ucAgent.IsLocked = false;
        }
        void ucAgent_onSave(object sender, EventArgs e)
        {
            ucAgent.IsEdit = false;
            lockAll(false);
            checkAvailability();
            checkSave();
        }
        void ucAgent_onCancel(object sender, EventArgs e)
        {
            ucAgent.IsEdit = false;
            lockAll(false);
            checkSave();
        }
        void UC_dt_pers_onModify(object sender, EventArgs e)
        {
            UC_dt_pers.IsEdit = true;
            lockAll(true);
            UC_dt_pers.IsLocked = false;
            lnk_calculatePrice.Visible = false;
            checkSave();
        }
        void UC_dt_pers_onSave(object sender, EventArgs e)
        {
            UC_dt_pers.IsEdit = false;
            UC_estate.sel_dtStart = UC_dt_pers.sel_dtStart;
            UC_estate.sel_dtEnd = UC_dt_pers.sel_dtEnd;
            UC_estate.sel_num_persons = UC_dt_pers.sel_num_adult + UC_dt_pers.sel_num_child_over;
            lockAll(false);
            lnk_calculatePrice.Visible = true;
            checkAvailability();
            checkSave();
        }
        void UC_dt_pers_onCancel(object sender, EventArgs e)
        {
            UC_dt_pers.IsEdit = false;
            lockAll(false);
            lnk_calculatePrice.Visible = true;
            checkSave();
        }
        void UC_notes_onModify(object sender, EventArgs e)
        {
            UC_notes.IsEdit = true;
            lockAll(true);
            UC_notes.IsLocked = false;
        }
        void UC_notes_onSave(object sender, EventArgs e)
        {
            UC_notes.IsEdit = false;
            lockAll(false);
        }
        void UC_notes_onCancel(object sender, EventArgs e)
        {
            UC_notes.IsEdit = false;
            lockAll(false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                DateTime dtStart = DateTime.Now.AddDays(7);
                DateTime dtEnd = DateTime.Now.AddDays(10);
                HF_dtDate.Value = Request.QueryString["dtDate"];
                HF_dtPart.Value = Request.QueryString["dtPart"];
                int _dtStartInt = Request.QueryString["dtStart"].objToInt32();
                int _dtEndInt = Request.QueryString["dtEnd"].objToInt32();
                if (_dtStartInt != 0)
                    dtStart = _dtStartInt.JSCal_intToDate();
                if (_dtEndInt != 0)
                    dtEnd = _dtEndInt.JSCal_intToDate();
                else
                    dtEnd = dtStart.AddDays(3);
                IdReservation = Request.QueryString["IdRes"].ToInt64();
                IdRequest = Request.QueryString["IdRequest"].ToInt64();
                int IdClient = Request.QueryString["IdClient"].objToInt32();
                int IdEstate = Request.QueryString["IdEstate"].objToInt32();
                int sel_num_adult = 2;
                int sel_num_child_min = 0;
                int sel_num_child_over = 0;
                if (IdRequest != 0)
                {
                    RNT_TBL_REQUEST _request = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == IdRequest);
                    if (_request != null)
                    {
                        dtStart = _request.request_date_start.HasValue ? _request.request_date_start.Value : dtStart;
                        dtEnd = _request.request_date_end.HasValue ? _request.request_date_end.Value : dtEnd;
                        sel_num_adult = _request.request_adult_num.objToInt32();
                        sel_num_child_min = _request.request_child_num_min.objToInt32();
                        sel_num_child_over = _request.request_child_num.objToInt32();
                        UC_client.IdRequest = IdRequest;
                        IdClient = IdClient == 0 ? -2 : IdClient;
                    }

                }
                if (IdReservation != 0)
                    _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                if (_currTBL != null)
                {
                    if (_currTBL.id < 150000)
                    {
                        Response.Redirect("rnt_reservation_formWART.aspx?IdRes=" + IdReservation + "&nomenu=" + Request.QueryString["nomenu"]);
                        return;
                    }
                    Response.Redirect("rnt_reservation_details.aspx?id=" + IdReservation + "&nomenu=" + Request.QueryString["nomenu"]);
                    return;
                }
                IdReservation = 0;
                UC_estate.IdEstate = IdEstate;
                UC_estate.sel_dtStart = dtStart;
                UC_estate.sel_dtEnd = dtEnd;
                UC_estate.sel_num_persons = 2;
                UC_estate.FillControls();
                ucAgent.FillControls();
                UC_client.IdReservation = IdReservation;
                UC_client.IdClient = IdClient;
                UC_client.FillControls();
                UC_dt_pers._checkCalDates = rntUtils.rntEstate_availableDatesForJSCal(UC_estate.IdEstate, IdReservation);
                UC_dt_pers.num_persons_child = UC_estate.num_persons_child;
                UC_dt_pers.num_persons_min = UC_estate.num_persons_min;
                UC_dt_pers.num_persons_max = UC_estate.num_persons_max;
                UC_dt_pers.nights_min = UC_estate.nights_min;
                UC_dt_pers.sel_dtStart = dtStart;
                UC_dt_pers.sel_dtEnd = dtEnd;
                if (UC_estate.num_persons_min > (sel_num_adult + sel_num_child_over))
                    sel_num_adult = UC_estate.num_persons_min - sel_num_child_over;
                if (UC_estate.num_persons_max < (sel_num_adult + sel_num_child_over))
                    sel_num_adult = UC_estate.num_persons_max - sel_num_child_over;
                UC_dt_pers.sel_num_adult = sel_num_adult;
                UC_dt_pers.sel_num_child_min = sel_num_child_min;
                UC_dt_pers.sel_num_child_over = sel_num_child_over;
                UC_dt_pers.FillControls();
                UC_state.IdReservation = IdReservation;
                UC_state.FillControls();
                UC_notes.Body = "";
                UC_notes.FillControls();
                checkAvailability();
                checkSave();

                if (dtStart < DateTime.Now.Date)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "warningAlert", "alert('Attenzione! La data del checkin è precedente alla data odierna.');", true);
                }
            }
        }
        protected string getStateClass(int? id)
        {
            if (id == 1)
                return "rntCal_nd";
            if (id == 2)
                return "rntCal_opz";
            if (id == 3)
                return "rntCal_can";
            if (id == 4)
                return "rntCal_prt";
            if (id == 5)
                return "rntCal_mv";
            return "rntCal_xxx";
        }

        protected void lockAll(bool _lock)
        {
            UC_client.IsLocked = _lock;
            UC_dt_pers.IsLocked = _lock;
            UC_estate.IsLocked = _lock;
            UC_state.IsLocked = _lock;
            if (UC_dt_pers.sel_dtStart <= DateTime.Now.Date.AddDays(-7))
            {
                UC_client.IsLocked = true;
                UC_dt_pers.IsLocked = true;
                UC_estate.IsLocked = true;
            }
        }

        protected void checkSave()
        {
            pnl_notifyNew.Visible = IdReservation == 0;
            lbl_avvError.Visible = false;
            lbl_periodError.Visible = false;
            lbl_priceError.Visible = false;
            lbl_clientError.Visible = false;
            lbl_estateError.Visible = false;
            lbl_stateError.Visible = false;
            bool _ok = true;
            if (HF_avvOK.Value == "0")
            {
                lbl_avvError.Visible = true; 
                _ok = false;
            }
            if (HF_periodOK.Value == "0" || UC_dt_pers.IsEdit)
            {
                lbl_periodError.Visible = true;
                _ok = false;
            }
            if (HF_priceOK.Value == "0")
            {
                lbl_priceError.Visible = true;
                _ok = false;
            }
            if (ucAgent.IdAgent <= 0)
            {
                if (UC_client.IdClient == 0 || UC_client.IsEdit)
                {
                    lbl_clientError.Visible = true;
                    _ok = false;
                }
                UC_client.Visible = true;
            }
            else
            {
                UC_client.Visible = false;
            }
            if (UC_estate.IdEstate == 0 || UC_estate.IsEdit)
            {
                lbl_estateError.Visible = true;
                _ok = false;
            }
            if (UC_state.state_pid == 0 || UC_state.IsEdit)
            {
                lbl_stateError.Visible = true;
                _ok = false;
            }
            UC_dt_pers.IsBooking = true;
            lnk_calculatePrice.Visible = true;
            pnl_btnSave.Visible = _ok;
            UpdatePanel_main.Update();
        }

        protected void lnk_calculatePrice_Click(object sender, EventArgs e)
        {
            checkAvailability();
        }
        public void checkAvailability()
        {
            HF_periodOK.Value = "1";
            HF_avvOK.Value = "1";
            HF_priceOK.Value = "0";
            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = UC_dt_pers.sel_dtStart;
            outPrice.dtEnd = UC_dt_pers.sel_dtEnd;
            outPrice.numPersCount = (UC_dt_pers.sel_num_adult + UC_dt_pers.sel_num_child_over);
            outPrice.pr_discount_owner = 0;
            outPrice.pr_discount_commission = 0;
            decimal agentTotalResPrice = 0;
            if (ucAgent.IdAgent > 0)
            {
                using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == ucAgent.IdAgent);
                    if (agentTBL != null)
                    {
                        outPrice.fillAgentDetails(agentTBL);
                        DateTime checkDate = DateTime.Now;
                        DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                        DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                        var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTBL.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                        agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                    }
                }
            }
            outPrice.agentTotalResPrice = agentTotalResPrice;
            outPrice.part_percentage = UC_estate.pr_percentage;
            decimal _pr_total = rntUtils.rntEstate_getPrice(IdReservation, UC_estate.IdEstate, ref outPrice);
            bool _isAvailable = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(y => y.pid_estate == UC_estate.IdEstate && y.id != IdReservation //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= outPrice.dtStart && y.dtEnd.Value.Date >= outPrice.dtEnd) //
                                                                                || (y.dtStart.Value.Date >= outPrice.dtStart && y.dtStart.Value.Date < outPrice.dtEnd) //
                                                                                || (y.dtEnd.Value.Date > outPrice.dtStart && y.dtEnd.Value.Date <= outPrice.dtEnd))).Count() == 0;
            outPrice.prTotal = _pr_total;
            currOutPrice = outPrice;
            lbl_priceError1.Visible = outPrice.outError == 1;
            if (!_isAvailable)
            {
                HF_avvOK.Value = "0";
                return;
            }
            UC_estate.sel_pr_total = _pr_total;
            HF_priceOK.Value = "1";
        }

        protected void saveReservation()
        {
            //if (HF_is_booking.Value != "1")
            //{
            //    _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            //    if (_currTBL == null)
            //    {
            //        _currTBL = rntUtils.newReservation();
            //        _currTBL.pid_creator = UserAuthentication.CurrentUserID;
            //        _currTBL.state_pid = UC_state.state_pid;
            //        _currTBL.state_body = UC_state.state_body;
            //        _currTBL.state_date = DateTime.Now;
            //        _currTBL.state_pid_user = UserAuthentication.CurrentUserID;
            //        _currTBL.state_subject = UC_state.state_subject;
            //        _currTBL.is_booking = 0;

            //        _currTBL.dtStartTime = "140000";
            //        _currTBL.dtEndTime = "110000";
            //        _currTBL.is_dtStartTimeChanged = 0;
            //        _currTBL.is_dtEndTimeChanged = 0;
            //        DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
            //        DC_RENTAL.SubmitChanges();
            //        _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
            //        _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            //    }
            //    _currTBL.dtStart = UC_dt_pers.sel_dtStart;
            //    _currTBL.dtEnd = UC_dt_pers.sel_dtEnd;

            //    _currTBL.pid_estate = UC_estate.IdEstate;
            //    _currTBL.cl_id = -1;
            //    _currTBL.cl_email = "";
            //    _currTBL.cl_name_full = "Sua";
            //    _currTBL.cl_name_honorific = "Mr.";
            //    _currTBL.cl_pid_discount = 0;
            //    _currTBL.cl_pid_lang = 1;
            //    _currTBL.inner_notes = UC_notes.inner_notes;

            //    AdminUtilities.rntReservation_setDefaults(ref _currTBL);
            //    DC_RENTAL.SubmitChanges();
            //    rntUtils.rntReservation_onChange(_currTBL);
            //    pnl_saveOK.Visible = true;
            //    pnl_saveNO.Visible = false;
            //    lockAll(true);
            //    lnk_calculatePrice.Visible = false;
            //    return;
            //}
            RNT_TB_ESTATE _estate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == UC_estate.IdEstate);
            bool _isAvailable = DC_RENTAL.RNT_TBL_RESERVATION.Where(y => y.pid_estate == UC_estate.IdEstate && y.id != IdReservation //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= UC_dt_pers.sel_dtStart && y.dtEnd.Value.Date >= UC_dt_pers.sel_dtEnd) //
                                                                                || (y.dtStart.Value.Date >= UC_dt_pers.sel_dtStart && y.dtStart.Value.Date < UC_dt_pers.sel_dtEnd) //
                                                                                || (y.dtEnd.Value.Date > UC_dt_pers.sel_dtStart && y.dtEnd.Value.Date <= UC_dt_pers.sel_dtEnd))).Count() == 0;
            if (_estate == null || !_isAvailable)
            {
                checkAvailability();
                checkSave();
                return;
            }
            RNT_TBL_RESERVATION _currTBL = null;
            USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == UC_client.IdClient);
            if (ucAgent.IdAgent > 0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == ucAgent.IdAgent);
                    if (agentTBL != null)
                    {
                        _client = new USR_TBL_CLIENT();
                        _client.id = -1;
                        _client.contact_email = agentTBL.contactEmail;
                        _client.name_full = agentTBL.nameCompany;
                        _client.name_honorific = "";
                        _client.loc_country = agentTBL.locCountry;
                        _client.pid_discount = -1;
                        _client.pid_lang = agentTBL.pidLang;
                        _client.isCompleted = 0;
                    }
                }
            }
            if (_client == null)
            {
                UC_client.FillControls();
                checkSave();
                return;
            }

            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null)
            {
                IdReservation = 0;
                _currTBL = rntUtils.newReservation();
                _currTBL.agentID = ucAgent.IdAgent;
                _currTBL.pid_creator = UserAuthentication.CurrentUserID;
                _currTBL.state_pid = UC_state.state_pid;
                _currTBL.state_body = UC_state.state_body;
                _currTBL.state_date = DateTime.Now;
                _currTBL.state_pid_user = UserAuthentication.CurrentUserID;
                _currTBL.state_subject = UC_state.state_subject;
                _currTBL.is_booking = 1;
                _currTBL.pid_operator = UserAuthentication.CurrentUserID;
                _currTBL.pid_estate = UC_estate.IdEstate;
                _currTBL.pidEstateCity = UC_estate.PidCity;
                _currTBL.dtStart = UC_dt_pers.sel_dtStart;
                _currTBL.dtEnd = UC_dt_pers.sel_dtEnd;
                
                // scadenze instant booking
                int _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHours").ToInt32();
                _currTBL.block_expire = DateTime.Now.AddHours(_blockHours);
                _currTBL.block_expire_hours = _blockHours;
                _currTBL.block_comments = "Scadenza predefinita [" + _blockHours + " ore]";
                _currTBL.block_pid_user = 1;

                _currTBL.dtStartTime = "000000";
                _currTBL.dtEndTime = "000000";
                _currTBL.is_dtStartTimeChanged = 0;
                _currTBL.is_dtEndTimeChanged = 0;
                DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
                DC_RENTAL.SubmitChanges();
                rntUtils.rntReservation_onChange(_currTBL);
                if (IdRequest != 0)
                {
                    RNT_TBL_REQUEST _request = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == IdRequest);
                    if (_request != null)
                    {
                        _request.pid_reservation = _currTBL.id;
                        _request.state_body = "";
                        _request.state_subject = "Diventato prenotazione";
                        _request.state_date = DateTime.Now;
                        _request.state_pid = 5;
                        _request.state_pid_user = UserAuthentication.CurrentUserID;
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
                        _currTBL.pid_related_request = _request.id;
                        _currTBL.pid_operator = _request.pid_operator;
                        _currTBL.IdAdMedia = _request.IdAdMedia;
                        _currTBL.IdLink = _request.IdLink;
                    }

                }
                _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
                _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            }
            else
            {
                if (UC_state.IsChanged)
                {
                    rntUtils.rntReservation_onStateChange(_currTBL);
                    _currTBL.state_pid = UC_state.state_pid;
                    _currTBL.state_body = UC_state.state_body;
                    _currTBL.state_date = DateTime.Now;
                    _currTBL.state_pid_user = UserAuthentication.CurrentUserID;
                    _currTBL.state_subject = UC_state.state_subject;
                }
            }

            _currTBL.dtStart = UC_dt_pers.sel_dtStart;
            _currTBL.dtEnd = UC_dt_pers.sel_dtEnd;

            _currTBL.num_adult = UC_dt_pers.sel_num_adult;
            _currTBL.num_child_over = UC_dt_pers.sel_num_child_over;
            _currTBL.num_child_min = UC_dt_pers.sel_num_child_min;

            currOutPrice.CopyTo(ref _currTBL);

            _currTBL.pr_deposit = UC_estate.pr_deposit;
            _currTBL.srs_ext_meetingPoint = _estate.srs_ext_meetingPoint;
            _currTBL.pr_depositWithCard = UC_estate.pr_depositWithCard ? 1 : 0;

            _currTBL.pid_estate = UC_estate.IdEstate;
            _currTBL.cl_id = _client.id;
            _currTBL.cl_email = _client.contact_email;
            _currTBL.cl_name_full = _client.name_full;
            _currTBL.cl_name_honorific = _client.name_honorific;
            _currTBL.cl_loc_country = _client.loc_country;
            _currTBL.cl_pid_discount = _client.pid_discount;
            _currTBL.cl_pid_lang = _client.pid_lang;
            _currTBL.cl_isCompleted = _client.isCompleted;
            _currTBL.notesInner = UC_notes.Body;

            rntUtils.rntReservation_setDefaults(ref _currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            if (IdReservation == 0 && drp_sendMailCreated.SelectedValue == "1")
            {
                rntUtils.rntReservation_mailNewCreation(_currTBL, true, true, true, true, 1);
            }
            rntUtils.reservation_checkPartPayment(_currTBL, true);
            pnl_saveOK.Visible = true;
            pnl_saveNO.Visible = false;
            lockAll(true);
            lnk_calculatePrice.Visible = false;
        }

        protected void lnk_save_Click(object sender, EventArgs e)
        {
            saveReservation();
        }
    }
}
