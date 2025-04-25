using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_reservation_formWART : adminBasePage
    {
        protected string listPage = "rnt_reservation_list.aspx";
        private magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_RESERVATION _currTBL;
        private magaInvoice_DataContext DC_INVOICE;
        public long IdReservation
        {
            get { return HF_id.Value.ToInt32(); }
            set { HF_id.Value = value.ToString(); }
        }
        public int IdEstate
        {
            get { return HF_IdEstate.Value.ToInt32(); }
            set { HF_IdEstate.Value = value.ToString(); }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_reservation_list";

            UC_state.onModify += new EventHandler(UC_state_onModify);
            UC_state.onSave += new EventHandler(UC_state_onSave);
            UC_state.onCancel += new EventHandler(UC_state_onCancel);

            UC_notes.onModify += new EventHandler(UC_notes_onModify);
            UC_notes.onSave += new EventHandler(UC_notes_onSave);
            UC_notes.onCancel += new EventHandler(UC_notes_onCancel);

            UC_dt_pers.onModify += new EventHandler(UC_dt_pers_onModify);
            UC_dt_pers.onSave += new EventHandler(UC_dt_pers_onSave);
            UC_dt_pers.onCancel += new EventHandler(UC_dt_pers_onCancel);

            UC_inout.onModify += new EventHandler(UC_inout_onModify);
            UC_inout.onSave += new EventHandler(UC_inout_onSave);
            UC_inout.onCancel += new EventHandler(UC_inout_onCancel);
        }
        void UC_dt_pers_onModify(object sender, EventArgs e)
        {
            UC_dt_pers.IsEdit = true;
            UC_dt_pers.IsLocked = false;
        }
        void UC_dt_pers_onSave(object sender, EventArgs e)
        {
            UC_dt_pers.IsEdit = false;
            saveReservation("dt_pers");
        }
        void UC_dt_pers_onCancel(object sender, EventArgs e)
        {
            UC_dt_pers.IsEdit = false;
        }
        void UC_state_onModify(object sender, EventArgs e)
        {
            UC_state.IsEdit = true;
            UC_state.IsLocked = false;
        }
        void UC_state_onSave(object sender, EventArgs e)
        {
            UC_state.IsEdit = false;
            saveReservation("state");
        }
        void UC_state_onCancel(object sender, EventArgs e)
        {
            UC_state.IsEdit = false;
        }
        void UC_notes_onModify(object sender, EventArgs e)
        {
            UC_notes.IsEdit = true;
            UC_notes.IsLocked = false;
        }
        void UC_notes_onSave(object sender, EventArgs e)
        {
            saveReservation("notes");
            UC_notes.IsEdit = false;
            rntUtils.rntReservation_onChange(_currTBL);
        }
        void UC_notes_onCancel(object sender, EventArgs e)
        {
            UC_notes.IsEdit = false;
        }
        void UC_inout_onModify(object sender, EventArgs e)
        {
            UC_inout.IsEdit = true;
            UC_inout.IsLocked = false;
        }
        void UC_inout_onSave(object sender, EventArgs e)
        {
            UC_inout.IsEdit = false;
            saveReservation("inout");
        }
        void UC_inout_onCancel(object sender, EventArgs e)
        {
            UC_inout.IsEdit = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            string str = AppDomain.CurrentDomain.BaseDirectory;
            if (!IsPostBack)
            {
                IdReservation = Request.QueryString["IdRes"].ToInt64();
                fillData();
            }
        }
        protected void fillData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null)
            {
                Response.Redirect(listPage, true);
                return;
            }
            if (_currTBL.id > 150000)
            {
                Response.Redirect("rnt_reservation_form.aspx?IdRes=" + IdReservation);
                return;
            }

            //AdminUtilities.rntReservation_setDefaults(ref _currTBL);
            //DC_RENTAL.SubmitChanges();
            DateTime dtStart = _currTBL.dtStart.HasValue ? _currTBL.dtStart.Value : DateTime.Now.AddDays(7);
            DateTime dtEnd = _currTBL.dtEnd.HasValue ? _currTBL.dtEnd.Value : DateTime.Now.AddDays(10);

            UC_state.IdReservation = IdReservation;
            UC_state.FillControls();

            UC_notes.Body = _currTBL.notesInner;
            UC_notes.FillControls();

            UC_dt_pers._checkCalDates = rntUtils.rntEstate_availableDatesForJSCal(IdEstate, IdReservation);
            UC_dt_pers.num_persons_child = 10;
            UC_dt_pers.num_persons_min = 1;
            UC_dt_pers.num_persons_max = 20;
            UC_dt_pers.sel_num_adult = _currTBL.num_adult.objToInt32();
            UC_dt_pers.sel_num_child_min = _currTBL.num_child_min.objToInt32();
            UC_dt_pers.sel_num_child_over = _currTBL.num_child_over.objToInt32();
            UC_dt_pers.sel_dtStart = dtStart;
            UC_dt_pers.sel_dtEnd = dtEnd;
            UC_dt_pers.FillControls();

            UC_inout.IdReservation = IdReservation;
            UC_inout.UIdReservation = _currTBL.unique_id.ToString();
            UC_inout.in_pointType = _currTBL.limo_inPoint_type;
            UC_inout.in_transportType = _currTBL.limo_inPoint_transportType;
            UC_inout.in_pickupPlace = _currTBL.limo_inPoint_pickupPlace.objToInt32();
            UC_inout.in_pickupPlaceName = _currTBL.limo_inPoint_pickupPlaceName;
            UC_inout.in_pickupDetails = _currTBL.limo_inPoint_details;
            UC_inout.in_pickupDetailsType = _currTBL.limo_inPoint_detailsType.objToInt32();
            UC_inout.limo_in_datetime = _currTBL.limo_in_datetime;
            if (!_currTBL.dtIn.HasValue && !string.IsNullOrEmpty(_currTBL.dtStartTime) && _currTBL.dtStartTime != "000000")
                UC_inout.dtIn = _currTBL.dtStart.Value.Add(_currTBL.dtStartTime.JSTime_stringToTime());
            else
                UC_inout.dtIn = _currTBL.dtIn;
            UC_inout.out_pointType = _currTBL.limo_outPoint_type;
            UC_inout.out_transportType = _currTBL.limo_outPoint_transportType;
            UC_inout.out_pickupPlace = _currTBL.limo_outPoint_pickupPlace.objToInt32();
            UC_inout.out_pickupPlaceName = _currTBL.limo_outPoint_pickupPlaceName;
            UC_inout.out_pickupDetails = _currTBL.limo_outPoint_details;
            UC_inout.out_pickupDetailsType = _currTBL.limo_outPoint_detailsType.objToInt32();
            UC_inout.limo_out_datetime = _currTBL.limo_out_datetime;
            if (!_currTBL.dtOut.HasValue && !string.IsNullOrEmpty(_currTBL.dtEndTime) && _currTBL.dtEndTime != "000000")
                UC_inout.dtOut = _currTBL.dtEnd.Value.Add(_currTBL.dtEndTime.JSTime_stringToTime());
            else
                UC_inout.dtOut = _currTBL.dtOut;
            UC_inout.FillControls();

            HF_dtStart.Value = dtStart.JSCal_dateToString();
            HF_dtEnd.Value = dtEnd.JSCal_dateToString();
            IdEstate = _currTBL.pid_estate.objToInt32();
            txt_pr_total.Text = _currTBL.pr_total.objToDecimal().ToString();
            txt_pr_part_agency_fee.Text = _currTBL.pr_part_agency_fee.objToDecimal().ToString();
            txt_pr_part_commission_tf.Text = _currTBL.pr_part_commission_tf.objToDecimal().ToString();
            rntUtils.reservation_checkPartPayment(_currTBL, false);
        }
        protected void saveReservation(string type)
        {
            lblError.Text = "";
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null)
            {
                Response.Redirect(listPage, true);
                return;
            }
            if (type == "state")
            {
                rntUtils.rntReservation_onStateChange(_currTBL);
                _currTBL.state_pid = UC_state.state_pid;
                _currTBL.state_body = UC_state.state_body;
                _currTBL.state_date = DateTime.Now;
                _currTBL.state_pid_user = UserAuthentication.CurrentUserID;
                _currTBL.state_subject = UC_state.state_subject;
                lblError.Text = "Lo stato è stato aggiornato";
            }
            if (type == "price")
            {
                _currTBL.pr_total = txt_pr_total.Text.ToDecimal();
                _currTBL.pr_total_desc = "";
                _currTBL.pr_part_commission_tf = txt_pr_part_commission_tf.Text.ToDecimal();
                _currTBL.pr_part_commission_total = _currTBL.pr_part_commission_tf;
                _currTBL.pr_part_agency_fee = txt_pr_part_agency_fee.Text.ToDecimal();
                _currTBL.pr_reservation = _currTBL.pr_total - _currTBL.pr_part_commission_tf;
                _currTBL.pr_part_payment_total = _currTBL.pr_part_commission_total + _currTBL.pr_part_agency_fee;
                _currTBL.pr_part_owner = _currTBL.pr_total - _currTBL.pr_part_payment_total;

                _currTBL.pr_discount_owner = 0;
                _currTBL.pr_discount_commission = 0;
                _currTBL.pr_discount_desc ="";
                _currTBL.pr_discount_user = 0;

                txt_pr_total.Text = _currTBL.pr_total.objToDecimal().ToString();
                txt_pr_part_agency_fee.Text = _currTBL.pr_part_agency_fee.objToDecimal().ToString();
                txt_pr_part_commission_tf.Text = _currTBL.pr_part_commission_tf.objToDecimal().ToString();

                lblError.Text = "I prezzi sono stati aggiornati";
            }
            if (type == "notes")
            {
                _currTBL.notesInner = UC_notes.Body;
                lblError.Text = "Le note sono stati aggiornati";
            } 
            if (type == "dt_pers")
            {
                _currTBL.dtStart = UC_dt_pers.sel_dtStart;
                _currTBL.dtEnd = UC_dt_pers.sel_dtEnd;

                _currTBL.num_adult = UC_dt_pers.sel_num_adult;
                _currTBL.num_child_over = UC_dt_pers.sel_num_child_over;
                _currTBL.num_child_min = UC_dt_pers.sel_num_child_min;
            }
            if (type == "inout")
            {
                //_currTBL.limo_in_datetime = UC_inout.limo_in_datetime;
                //_currTBL.limo_inPoint_details = UC_inout.limo_inPoint_details;
                //_currTBL.limo_inPoint_pickupPlaceName = UC_inout.limo_inPoint_pickupPlaceName;
                //_currTBL.limo_inPoint_type = UC_inout.limo_inPoint_type;
                //_currTBL.dtStartTime = UC_inout.dtStartTime;
                //_currTBL.is_dtStartTimeChanged = 1;

                //_currTBL.limo_out_datetime = UC_inout.limo_out_datetime;
                //_currTBL.limo_outPoint_details = UC_inout.limo_outPoint_details;
                //_currTBL.limo_outPoint_pickupPlaceName = UC_inout.limo_outPoint_pickupPlaceName;
                //_currTBL.limo_outPoint_type = UC_inout.limo_outPoint_type;
                //_currTBL.dtEndTime = UC_inout.dtEndTime;
                //_currTBL.is_dtEndTimeChanged = 1;
                //lblError.Text = "I dati di arrivo e partenza sono stati aggiornati";
            }
            rntUtils.rntReservation_onChange(_currTBL);
            DC_RENTAL.SubmitChanges();
        }

        protected void lnk_save_Click(object sender, EventArgs e)
        {
            saveReservation("dt_pers");
        }
        protected void LV_payment_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_is_complete = e.Item.FindControl("lbl_is_complete") as Label;
            if (lbl_is_complete == null || lbl_id == null) return;
            if (e.CommandName == "salva")
            {
                DropDownList drp_is_complete = e.Item.FindControl("drp_is_complete") as DropDownList;
                DropDownList drp_pay_mode = e.Item.FindControl("drp_pay_mode") as DropDownList;
                if (
                    drp_is_complete != null
                    && drp_pay_mode != null
                    )
                {

                    INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (_pay == null || drp_is_complete.getSelectedValueInt(0) == 0) return;
                    _pay.is_complete = 1;
                    _pay.pay_date = DateTime.Now;
                    _pay.pay_mode = drp_pay_mode.SelectedValue;
                    _pay.state_pid = 1;
                    _pay.state_date = DateTime.Now;
                    _pay.state_subject = "Completato";
                    _pay.state_pid_user = UserAuthentication.CurrentUserID;
                    DC_INVOICE.SubmitChanges();
                    _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                    if (_currTBL != null && _currTBL.state_pid != 4)
                    {
                        rntUtils.rntReservation_onStateChange(_currTBL);
                        _currTBL.state_pid = 4;
                        _currTBL.state_date = DateTime.Now;
                        _currTBL.state_subject = "Prenotato";
                        _currTBL.state_pid_user = UserAuthentication.CurrentUserID;
                        DC_RENTAL.SubmitChanges();
                        rntUtils.rntReservation_onChange(_currTBL);
                        Response.Redirect("rnt_reservation_formWART.aspx?IdRes=" + IdReservation, true);
                        return;
                    }
                    LDS_payment.DataBind();
                    LV_payment.SelectedIndex = -1;
                    LV_payment.DataBind();
                }
            }
            if (e.CommandName == "annulla")
            {
                LDS_payment.DataBind();
                LV_payment.SelectedIndex = -1;
                LV_payment.DataBind();
            }
        }
        protected void LV_payment_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            Label lbl_id = LV_payment.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            if (lbl_id != null)
            {
            }
        }
        protected void LV_payment_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_is_complete = e.Item.FindControl("lbl_is_complete") as Label;
            if (lbl_is_complete == null || lbl_id == null) return;
            LinkButton lnk_edit = e.Item.FindControl("lnk_edit") as LinkButton;
            if (lnk_edit != null)
            {
                lnk_edit.Visible = lbl_is_complete.Text == "0";
                return;
            }
            DropDownList drp_is_complete = e.Item.FindControl("drp_is_complete") as DropDownList;
            DropDownList drp_pay_mode = e.Item.FindControl("drp_pay_mode") as DropDownList;
            if (drp_is_complete != null
                && drp_pay_mode != null
                )
            {
                drp_is_complete.Items.Clear();
                drp_is_complete.Items.Add(new ListItem("SI", "1"));
                drp_is_complete.Items.Add(new ListItem("NO", "0"));

                drp_pay_mode.DataSource = invProps.CashPlaceLK.OrderBy(x => x.title).ToList();
                drp_pay_mode.DataTextField = "title";
                drp_pay_mode.DataValueField = "code";
                drp_pay_mode.DataBind();
            }
        }
    }
}
