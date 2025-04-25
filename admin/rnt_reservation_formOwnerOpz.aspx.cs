using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_reservation_formOwnerOpz : adminBasePage
    {
        private magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_RESERVATION _currTBL;
        public int IdEstate
        {
            get { return HF_IdEstate.Value.ToInt32(); }
            set { HF_IdEstate.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                DateTime dtStart = DateTime.Now.AddDays(7);
                DateTime dtEnd = DateTime.Now.AddDays(10);
                int _dtStartInt = Request.QueryString["dtStart"].objToInt32();
                int _dtEndInt = Request.QueryString["dtEnd"].objToInt32();
                if (_dtStartInt != 0)
                    dtStart = _dtStartInt.JSCal_intToDate();
                if (_dtEndInt != 0)
                    dtEnd = _dtEndInt.JSCal_intToDate();
                else
                    dtEnd = dtStart.AddDays(3);
                HF_dtEnd.Value = dtEnd.JSCal_dateToString();
                HF_dtStart.Value = dtStart.JSCal_dateToString();
                IdEstate = Request.QueryString["IdEstate"].ToInt32();
                RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (_est != null)
                {
                    IdEstate = _est.id;
                    //ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                    //Bind_drp_state_pid();
                    //LoadContent();
                    HF_ext_ownerdaysinyear.Value = _est.ext_ownerdaysinyear.ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "erorrAlert()", "alert('errore imprevisto!');parent.refreshDates();", true);
                }
            }
            checkReservationsCal();
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
        protected void FillDataFromControls()
        {
            lblError.Visible = false;
            DateTime _dtStart = HF_dtStart.Value.JSCal_stringToDate();
            DateTime _dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.pid_estate == IdEstate && x.state_pid != 3 && x.dtEnd.HasValue && x.dtStart.HasValue && ((x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                                                                                                                   || (x.dtStart.Value.Date >= _dtStart && x.dtStart.Value.Date < _dtEnd) //
                                                                                                                                                                                   || (x.dtEnd.Value.Date > _dtStart && x.dtEnd.Value.Date <= _dtEnd))).ToList();

            if (_list.Count != 0)
            {
                lblError.Visible = true;
                lblError.Text = "La struttura risulta occupata nel periodo selezionato. <br/>Si prega di riprovare con altre date, oppure contattare l'assistenza.";
                return;
            }
            int _resCount = getResCount(_dtStart.Year);
            if ((HF_ext_ownerdaysinyear.Value.ToInt32() - _resCount) <= 0)
            {
                lblError.Visible = true;
                lblError.Text = "Avete superato il massimo dei giorni disponibili per anno " + _dtStart.Year + ". <br/>Si prega di contattare l'assistenza.";
                return;
            }
            if ((HF_ext_ownerdaysinyear.Value.ToInt32() - _resCount - (_dtStart.Date - _dtEnd.Date).Days) <= 0)
            {
                lblError.Visible = true;
                lblError.Text = "Con il periodo selezionato state superando il massimo dei giorni disponibili per anno " + _dtStart.Year + ". <br/>Si prega di selezionare " + (HF_ext_ownerdaysinyear.Value.ToInt32() - _resCount) + " giorni oppure contattare l'assistenza.";
                return;
            }
            _currTBL = rntUtils.newReservation();
            _currTBL.pid_creator = UserAuthentication.CurrentUserID;
            _currTBL.state_pid = 2;
            _currTBL.state_body = "";
            _currTBL.state_date = DateTime.Now;
            _currTBL.state_pid_user = UserAuthentication.CurrentUserID;
            _currTBL.state_subject = txt_subject.Text;
            _currTBL.is_booking = 0;

            _currTBL.dtStartTime = "140000";
            _currTBL.dtEndTime = "110000";
            _currTBL.is_dtStartTimeChanged = 0;
            _currTBL.is_dtEndTimeChanged = 0;

            _currTBL.dtStart = _dtStart;
            _currTBL.dtEnd = _dtEnd;

            _currTBL.pid_estate = IdEstate;
            _currTBL.cl_id = -1;
            _currTBL.cl_email = "";
            _currTBL.cl_name_full = "Sua";
            _currTBL.cl_name_honorific = "Mr.";
            _currTBL.cl_pid_discount = 0;
            _currTBL.cl_pid_lang = 1;

            _currTBL.num_adult = 0;
            _currTBL.num_child_over = 0;
            _currTBL.num_child_min = 0;

            _currTBL.pr_reservation = 0;
            _currTBL.pr_total = 0;
            _currTBL.pr_total_desc = "";
            _currTBL.pr_part_commission_tf = 0;
            _currTBL.pr_part_commission_total = 0;
            _currTBL.pr_part_agency_fee = 0;
            _currTBL.pr_part_payment_total = 0;
            _currTBL.pr_part_owner = 0;

            DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
            DC_RENTAL.SubmitChanges();
            _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
            _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            lblError.Visible = true;
            lblError.Text = "Avete prenotato la struttura con successo."
                                    + "<br/>Inizio: " + _dtStart.formatCustom("#DD# #dd# #MM# #yy#", 1, "__/__/____")
                                    + "<br/>Fine: " + _dtEnd.formatCustom("#DD# #dd# #MM# #yy#", 1, "__/__/____")
                                    + "<br/>Codice Pren.: " + _currTBL.code;
            pnl_btnSave.Visible = false;
        }
        protected void checkReservationsCal()
        {
            string _script = "";
            _script += "function checkCalDates(date){var _dtStart = parseInt($('#" + HF_dtStart.ClientID + "').val()); var _dtEnd = parseInt($('#" + HF_dtEnd.ClientID + "').val()); var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
            _script += rntUtils.rntEstate_availableDatesForJSCal(IdEstate, 0);
            _script += "if (dateint > _dtStart && dateint < _dtEnd) { _controls += '<span class=\"rntCal sel_f\"></span>'; }";
            _script += "if (dateint == _dtStart) { _controls += '<span class=\"rntCal sel_1\"></span>'; }";
            _script += "if (dateint == _dtEnd) { _controls += '<span class=\"rntCal sel_2\"></span>'; }";
            _script += "if (_controls.indexOf('<span class=\"rntCal nd_2\"></span>') != -1 && _controls.indexOf('<span class=\"rntCal nd_1\"></span>') != -1) { _enabled = false; }";
            _script += "return [_enabled, _class, _tooltip, _controls];";
            _script += "}";
            ltr_checkCalDates.Text = _script;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "checkCalDates", ltr_checkCalDates.Text, true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setCal", "setCal();", true);
        }
        protected int getResCount(int year)
        {
            int _count = 0;
            DateTime _dtStart = new DateTime(year, 1, 1);
            DateTime _dtEnd = new DateTime(year, 12, 31);
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.pid_estate == IdEstate && x.state_pid == 2 && x.dtEnd.HasValue && x.dtStart.HasValue && ((x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                                                                                       || (x.dtStart.Value.Date >= _dtStart && x.dtStart.Value.Date < _dtEnd) //
                                                                                                                                                       || (x.dtEnd.Value.Date > _dtStart && x.dtEnd.Value.Date <= _dtEnd))).ToList();
            foreach (RNT_TBL_RESERVATION _res in _list)
            {
                DateTime _start = _res.dtStart.Value.Date;
                DateTime _end = _res.dtEnd.Value.Date;
                if (_start < _dtStart)
                    _start = _dtStart;
                if (_end > _dtEnd)
                    _end = new DateTime(year, 12, 31);
                _count += (_end - _start).Days;
            }
            return _count;
        }
    }
}