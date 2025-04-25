using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.reservationarea.mobile
{
    public partial class details : basePage
    {
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        private RNT_TBL_RESERVATION _currTBL;
        public long CurrentIdReservation
        {
            get
            {
                basePage m = (basePage)this.Page;
                return m.CurrentIdReservation;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                fillData();
                ucHeader.PageTitle = "Reservation details"; contUtils.getLabel("lblPaymentSummary");
            }
        }
        protected void fillData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            resUtils.currCity = _estTB.pid_city.objToInt32();
            // scadenza
            if (_currTBL.state_pid != 4)
            {
                TimeSpan _expiresIn = (_currTBL.block_expire.Value - DateTime.Now);
                decimal _TotalHours_tmp = new decimal(_expiresIn.TotalHours);
                int _TotalHours = _TotalHours_tmp.objToInt32();
                if (_TotalHours > _TotalHours_tmp)
                    _TotalHours--;
                //lbl_hours.InnerText = _TotalHours.ToString();
                //lbl_minutes.InnerText = _expiresIn.Minutes.ToString();
                //HF_expiresInMinutes.Value = _expiresIn.TotalMinutes.objToInt32().ToString();
                //pnl_notComplete.Visible = true;
            }

            pnl_address.Visible = _currTBL.state_pid == 4;
            ltr_unique_id.Text = _currTBL.unique_id.ToString();
            HF_code.Value = _currTBL.code;
            ltr_cl_name_full.Text = _currTBL.cl_name_full;
            HF_dtStart.Value = _currTBL.dtStart.JSCal_dateToString();
            HF_dtEnd.Value = _currTBL.dtEnd.JSCal_dateToString();
            HF_num_adult.Value = _currTBL.num_adult.ToString();
            HF_num_child_over.Value = _currTBL.num_child_over.ToString();
            HF_num_child_min.Value = _currTBL.num_child_min.ToString();
            HF_pr_total.Value = _currTBL.pr_total.ToString();
            HF_pr_deposit.Value = _currTBL.pr_deposit.ToString();
            HF_pr_part_commission_tf.Value = _currTBL.pr_part_commission_tf.ToString();
            HF_pr_part_commission_total.Value = _currTBL.pr_part_commission_total.ToString();
            HF_pr_part_agency_fee.Value = _currTBL.pr_part_agency_fee.ToString();
            HF_pr_part_payment_total.Value = _currTBL.pr_part_payment_total.ToString();
            HF_pr_part_owner.Value = _currTBL.pr_part_owner.ToString();
            RNT_LN_ESTATE _estLN = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _currTBL.pid_estate && x.pid_lang == _currTBL.cl_pid_lang);
            if (_estLN == null)
                _estLN = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _currTBL.pid_estate && x.pid_lang == 2);
            if (_estLN != null)
            {
                ltr_est_code.Text = _estLN.title;
            }
            else
            {
                ltr_est_code.Text = _estTB.code;
            }
            ltr_est_address.Text = _estTB.loc_address + " " + _estTB.loc_zip_code + " <br/>" + CurrentSource.loc_cityTitle(_estTB.pid_city.objToInt32(), _currTBL.cl_pid_lang.objToInt32(), "Rome");
        }
        //protected void lnk_changeMode_Click(object sender, EventArgs e)
        //{
        //    if (((LinkButton)sender).CommandArgument == "visa_new")
        //    {
        //        _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
        //        if (_currTBL == null)
        //        {
        //            Response.Redirect("/reservationarea/login.aspx", true);
        //            return;
        //        }
        //        //_currTBL.visa_isRequested = 1;
        //        _currTBL.visa_persons = drp_visa_persons.getSelectedValueInt(0);
        //        DC_RENTAL.SubmitChanges();
        //        Response.Redirect("requestvisa.aspx", true);
        //    }
        //}
    }
}