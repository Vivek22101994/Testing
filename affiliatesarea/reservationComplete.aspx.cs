using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.affiliatesarea
{
    public partial class reservationComplete : agentBasePage
    {
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public string CURRENT_SESSION_ID
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                return m.CURRENT_SESSION_ID;
            }
        }
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }
        private rntExts.PreReservationPrices TMPcurrOutPrice;
        public rntExts.PreReservationPrices currOutPrice
        {
            get
            {
                if (TMPcurrOutPrice == null)
                    TMPcurrOutPrice = (rntExts.PreReservationPrices)ViewState[Unique + "_currOutPrice"];
                return TMPcurrOutPrice ?? new rntExts.PreReservationPrices();
            }
            set { TMPcurrOutPrice = value; ViewState[Unique + "_currOutPrice"] = TMPcurrOutPrice; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["IdEstate"].objToInt32();
                RNT_TB_ESTATE estateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (estateTB == null || estateTB.is_online_booking != 1)
                {
                    UpdatePanel1.Visible = false;
                    return;
                }
                ltr_estateTitle.Text = CurrentSource.rntEstate_title(IdEstate, CurrentLang.ID, estateTB.code);
                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                int dtStartInt = Request.QueryString["dtS"].objToInt32();
                int dtEndInt = Request.QueryString["dtE"].objToInt32();
                if (dtStartInt != 0 && dtEndInt != 0)
                {
                    outPrice.dtStart = dtStartInt.JSCal_intToDate();
                    outPrice.dtEnd = dtEndInt.JSCal_intToDate();
                }
                outPrice.dtCount = (outPrice.dtEnd - outPrice.dtStart).TotalDays.objToInt32();
                outPrice.numPers_adult = Request.QueryString["numPers_adult"].objToInt32() == 0 ? outPrice.numPers_adult : Request.QueryString["numPers_adult"].objToInt32();
                outPrice.numPers_childOver = Request.QueryString["numPers_childOver"].objToInt32() == 0 ? outPrice.numPers_childOver : Request.QueryString["numPers_childOver"].objToInt32();
                outPrice.numPers_childMin = Request.QueryString["numPers_childMin"].objToInt32() == 0 ? outPrice.numPers_childMin : Request.QueryString["numPers_childMin"].objToInt32();
                outPrice.numPersCount = outPrice.numPers_adult + outPrice.numPers_childOver;
                outPrice.pr_discount_owner = 0;
                outPrice.pr_discount_commission = 0;
                currOutPrice = outPrice;
                checkAvailability();
            }
        }
        public void checkAvailability()
        {
            RNT_TB_ESTATE estateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            rntExts.PreReservationPrices outPrice = currOutPrice;
            outPrice.part_percentage = estateTB.pr_percentage.objToDecimal();
            if (affiliatesarea.agentAuth.CurrentID != 0 && affiliatesarea.agentAuth.hasAcceptedContract == 1)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                    if (agentTBL != null)
                    {
                        outPrice.agentID = agentTBL.id;
                        outPrice.agentDiscountType = agentTBL.pidDiscountType.objToInt32();
                        outPrice.agentDiscountNotPayed = agentTBL.payDiscountNotPayed.objToInt32(); ;
                        outPrice.requestFullPayAccepted = agentTBL.payFullPayment.objToInt32();
                        if (outPrice.agentDiscountType == 0) outPrice.agentDiscountType = 1;
                    }
                }
            }
            decimal _pr_total = rntUtils.rntEstate_getPrice(0, IdEstate, ref outPrice);
            bool _isAvailable;
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                _isAvailable = dcOld.RNT_TBL_RESERVATION.Where(y => y.pid_estate == IdEstate //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= outPrice.dtStart && y.dtEnd.Value.Date >= outPrice.dtEnd) //
                                                                                || (y.dtStart.Value.Date >= outPrice.dtStart && y.dtStart.Value.Date < outPrice.dtEnd) //
                                                                                || (y.dtEnd.Value.Date > outPrice.dtStart && y.dtEnd.Value.Date <= outPrice.dtEnd))).Count() == 0;
            outPrice.prTotal = _pr_total;
            currOutPrice = outPrice;
            lbl_priceError0.Visible = outPrice.outError == 0;
            lbl_priceError1.Visible = outPrice.outError == 1;
            lbl_priceError2.Visible = outPrice.outError == 2;
            lbl_priceError3.Visible = outPrice.outError == 3;

            //PH_formBookignCont.Visible = _isAvailable;
            HF_tmp_prTotal.Value = _pr_total.objToInt32().ToString();
            PH_bookPriceOK.Visible = _pr_total != 0;
            PH_priceDetails.Visible = _pr_total != 0;
            PH_bookPriceError.Visible = _pr_total == 0;
            PH_bookNotAvailable.Visible = !_isAvailable;

            pnl_dicountCont.Visible = (currOutPrice.prDiscountLongStay + currOutPrice.prDiscountSpecialOffer) != 0;


            ltr_sel_priceDetails.Text = "<ul class=\"dett_day\">";
            currOutPrice.priceDetails = currOutPrice.priceDetails.OrderBy(x => x.sequence).ToList();
            foreach (rntExts.RNT_estatePriceDetails _priceDetail in currOutPrice.priceDetails.Where(x => x.type == 1))
            {
                ltr_sel_priceDetails.Text += "<li>";
                ltr_sel_priceDetails.Text += "  <span class=\"data\">" + _priceDetail.dt.formatITA(false) + "</span>";
                ltr_sel_priceDetails.Text += "  <span class=\"euro\">€ " + (_priceDetail.price + _priceDetail.priceOpt).ToString("N2") + "</span>";
                ltr_sel_priceDetails.Text += "</li>";
            }
            foreach (rntExts.RNT_estatePriceDetails _priceDetail in currOutPrice.priceDetails.Where(x => x.type == 2))
            {
                ltr_sel_priceDetails.Text += "<li>";
                ltr_sel_priceDetails.Text += "  <span class=\"data\">" + _priceDetail.description + "</span>";
                ltr_sel_priceDetails.Text += "  <span class=\"euro\">€ " + (_priceDetail.price).ToString("N2") + "</span>";
                ltr_sel_priceDetails.Text += "</li>";
            }
            foreach (rntExts.RNT_estatePriceDetails _priceDetail in currOutPrice.priceDetails.Where(x => x.type == 3))
            {
                ltr_sel_priceDetails.Text += "<li>";
                ltr_sel_priceDetails.Text += "  <span class=\"data\">" + _priceDetail.description + "</span>";
                ltr_sel_priceDetails.Text += "  <span class=\"euro\">€ " + (_priceDetail.price).ToString("N2") + "</span>";
                ltr_sel_priceDetails.Text += "</li>";
            }
            ltr_sel_priceDetails.Text += "</ul>";
            pnl_sendBooking.Visible = (estateTB.is_online_booking == 1 && _isAvailable && _pr_total != 0);
            this.Visible = true;
        }

        protected void saveReservation(bool payNow)
        {
            RNT_TB_ESTATE estateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            bool _isAvailable = DC_RENTAL.RNT_TBL_RESERVATION.Where(y => y.pid_estate == IdEstate //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= currOutPrice.dtStart && y.dtEnd.Value.Date >= currOutPrice.dtEnd) //
                                                                                || (y.dtStart.Value.Date >= currOutPrice.dtStart && y.dtStart.Value.Date < currOutPrice.dtEnd) //
                                                                                || (y.dtEnd.Value.Date > currOutPrice.dtStart && y.dtEnd.Value.Date <= currOutPrice.dtEnd))).Count() == 0;
            if (estateTB == null || !_isAvailable)
            {
                pnl_sendBooking.Visible = false;
                PH_bookNotAvailable.Visible = true;
                return;
            }
            RNT_TBL_RESERVATION _currTBL = rntUtils.newReservation();
            _currTBL.pid_estate = IdEstate;
            _currTBL.pidEstateCity = estateTB.pid_city.objToInt32();
            _currTBL.is_deleted = 0;
            _currTBL.pid_creator = 1;
            _currTBL.state_pid = 6;
            _currTBL.state_body = "";
            _currTBL.state_date = DateTime.Now;
            _currTBL.state_pid_user = 1;
            _currTBL.state_subject = "Nuovo dal sito online";
            _currTBL.is_booking = 1;

            _currTBL.dtStart = currOutPrice.dtStart;
            _currTBL.dtEnd = currOutPrice.dtEnd;
            _currTBL.dtStartTime = "000000";
            _currTBL.dtEndTime = "000000";

            _currTBL.num_adult = currOutPrice.numPers_adult;
            _currTBL.num_child_over = currOutPrice.numPers_childOver;
            _currTBL.num_child_min = currOutPrice.numPers_childMin;

            currOutPrice.CopyTo(ref _currTBL);

            _currTBL.pr_deposit = estateTB.pr_deposit;
            _currTBL.srs_ext_meetingPoint = estateTB.srs_ext_meetingPoint;
            _currTBL.pr_depositWithCard = estateTB.pr_depositWithCard.objToInt32();

            _currTBL.pr_discount_owner = 0;
            _currTBL.pr_discount_commission = 0;
            _currTBL.pr_discount_desc = "";
            // salviamo prima e aggiorniamo la cache per evitare overbooking
            DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);


            // scadenze instant booking
            int _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHoursOnline").ToInt32();
            _currTBL.block_comments = "Scadenza predefinita InstantBooking [" + _blockHours + " ore]";
            if (_currTBL.agentID != 0)
            {
                _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHoursAgent").ToInt32();
                _currTBL.block_comments = "Scadenza predefinita Agenzie [" + _blockHours + " ore]";
            }
            _currTBL.block_expire = DateTime.Now.AddHours(_blockHours);
            _currTBL.block_expire_hours = _blockHours;
            _currTBL.block_pid_user = 1;

            _currTBL.cl_id = -1;
            using (DCmodRental dc = new DCmodRental())
            {
                var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                if (agentTBL != null)
                {
                    _currTBL.cl_email = agentTBL.contactEmail;
                    _currTBL.cl_name_full = agentTBL.nameCompany;
                    _currTBL.cl_name_honorific = "";
                    _currTBL.cl_loc_country = agentTBL.locCountry;
                    _currTBL.cl_pid_discount = -1;
                    _currTBL.cl_pid_lang = agentTBL.pidLang;
                    _currTBL.cl_isCompleted = 1;
                    _currTBL.pid_operator = agentTBL.pidReferer;
                }
            }

            _currTBL.cl_browserInfo = Request.browserInfo();
            _currTBL.cl_browserIP = Request.browserIP();

            //_currTBL.pid_operator = AdminUtilities.usr_getAvailableOperator(AdminUtilities.zone_countryId(_currTBL.cl_loc_country), _currTBL.cl_pid_lang.objToInt32());

            _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
            _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            rntUtils.rntReservation_setDefaults(ref _currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            rntUtils.rntReservation_mailNewCreation(_currTBL, true, true, true, true, 1); // send mails
            rntUtils.reservation_checkPartPayment(_currTBL, false);
            INV_TBL_PAYMENT _pay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.FirstOrDefault(x => x.rnt_pid_reservation == _currTBL.id && x.rnt_reservation_part == "part");
            if (payNow && _pay != null)
            {
                Response.Redirect("https://rentalinrome.com/util_paypal_redirect.aspx?type=payment&code=" + _pay.code + "&lang=" + CurrentLang.ID);
            }
            PH_bookingSend.Visible = true;
            pnl_sendBooking.Visible = false;
            PH_bookingPrices.Visible = false;
        }

        protected void lnk_book_Click(object sender, EventArgs e)
        {
            saveReservation(false);
        }
        protected void lnk_payNow_Click(object sender, EventArgs e)
        {
            saveReservation(true);
        }
    }
}