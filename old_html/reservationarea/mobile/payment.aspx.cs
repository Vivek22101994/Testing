using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.reservationarea.mobile
{
    public partial class payment : basePage
    {
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                ucHeader.PageTitle = contUtils.getLabel("lblPaymentSummary");
                fillData();
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
            LV_DataBind();

            HF_id.Value = _currTBL.id.ToString();
            HF_code.Value = _currTBL.code;
            HF_pr_total.Value = _currTBL.pr_total.ToString();
            HF_pr_deposit.Value = _currTBL.pr_deposit.ToString();
            HF_pr_part_commission_tf.Value = _currTBL.pr_part_commission_tf.ToString();
            HF_pr_part_commission_total.Value = _currTBL.pr_part_commission_total.ToString();
            HF_pr_part_agency_fee.Value = _currTBL.pr_part_agency_fee.ToString();
            HF_pr_part_payment_total.Value = _currTBL.pr_part_payment_total.ToString();
            HF_pr_part_owner.Value = _currTBL.pr_part_owner.ToString();

        }
        protected void saveData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
        }
        protected void LV_DataBind()
        {
            List<INV_TBL_PAYMENT> listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && x.rnt_reservation_part.StartsWith("part") && x.pr_total.HasValue && x.pr_total > 0).ToList();
            LV_partPayment.DataSource = listPay.OrderBy(x => x.dtCreation);
            LV_partPayment.DataBind();
            foreach (ListViewDataItem item in LV_partPayment.Items)
            {
                Label lbl_id = item.FindControl("lbl_id") as Label;
                Label lbl_is_complete = item.FindControl("lbl_is_complete") as Label;
                PlaceHolder PH_payed = item.FindControl("PH_payed") as PlaceHolder;
                PlaceHolder PH_toPay = item.FindControl("PH_toPay") as PlaceHolder;
                if (lbl_is_complete == null || lbl_id == null || PH_payed == null || PH_toPay == null) return;
                PH_payed.Visible = lbl_is_complete.Text == "1";
                PH_toPay.Visible = !PH_payed.Visible;
            }
            pnl_partPaymentView.Visible = LV_partPayment.Items.Count == 0;

            listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && (x.rnt_reservation_part.StartsWith("owner") || x.rnt_reservation_part.StartsWith("full_")) && x.pr_total.HasValue && x.pr_total > 0).ToList();
            LV_partOwner.DataSource = listPay.OrderBy(x => x.dtCreation);
            LV_partOwner.DataBind();
            foreach (ListViewDataItem item in LV_partOwner.Items)
            {
                Label lbl_id = item.FindControl("lbl_id") as Label;
                Label lbl_is_complete = item.FindControl("lbl_is_complete") as Label;
                PlaceHolder PH_payed = item.FindControl("PH_payed") as PlaceHolder;
                PlaceHolder PH_toPay = item.FindControl("PH_toPay") as PlaceHolder;
                if (lbl_is_complete == null || lbl_id == null || PH_payed == null || PH_toPay == null) return;
                PH_payed.Visible = lbl_is_complete.Text == "1";
                PH_toPay.Visible = !PH_payed.Visible;
            }
            pnl_ownerPaymentView.Visible = LV_partOwner.Items.Count == 0;

            pnl_fullPaymentNO.Visible = true;
            pnl_fullPaymentOK.Visible = false;
            pnl_fullPaymentKO.Visible = false;
            if (!pnl_partPaymentView.Visible || !pnl_ownerPaymentView.Visible)
                return;
            INV_TBL_PAYMENT fullPayment = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.rnt_pid_reservation == _currTBL.id && x.rnt_reservation_part == "full");
            if (fullPayment != null)
            {
                HF_fullPaymentCode.Value = fullPayment.code;
                pnl_fullPaymentNO.Visible = false;
                pnl_fullPaymentOK.Visible = fullPayment.is_complete == 1;
                pnl_fullPaymentKO.Visible = pnl_ownerPaymentKO.Visible = pnl_partPaymentKO.Visible = !pnl_fullPaymentOK.Visible;
            }
        }
        protected void LV_payment_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }
    }
}