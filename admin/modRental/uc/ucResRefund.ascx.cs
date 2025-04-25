using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

/*
 Attenzione!
 * questo file
 * + filtro: x.is_complete == 1 && x.direction == 1
 * + payment_checkInvoiceWork: if (!_pay.pr_total.HasValue || _pay.pr_total <= 0 || _pay.direction != 1) return;
 * + RNT_RESERVATION_PAYMENT_PARTs: _tmp.Add(new customType(1, "res_refund", "Rimborso del pagamento"));
 * 
 
 
 */
namespace ModRental.admin.modRental.uc
{
    public partial class ucResRefund : System.Web.UI.UserControl
    {
        protected magaInvoice_DataContext DC_INVOICE;
        public event EventHandler onSave;
        public long IdReservation
        {
            get { return HF_id.Value.ToInt32(); }
            set
            {
                DC_INVOICE = maga_DataContext.DC_INVOICE;
                HF_id.Value = value.ToString();
            }
        }
        public bool Reload
        {
            get { return HF_reload.Value == "1"; }
            set
            {
                HF_reload.Value = value ? "1" : "0";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                drp_prRefundPayMode.DataSource = invProps.CashPlaceLK.OrderBy(x => x.title).ToList();
                drp_prRefundPayMode.DataTextField = "title";
                drp_prRefundPayMode.DataValueField = "code";
                drp_prRefundPayMode.DataBind();
                drp_prRefundPayMode.setSelectedValue("");
            }
        }
        protected void LV_payment_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_is_complete = e.Item.FindControl("lbl_is_complete") as Label;
            if (lbl_is_complete == null || lbl_id == null) return;
            if (e.CommandName == "annulla")
            {
                LDS_payment.DataBind();
                LV_payment.SelectedIndex = -1;
                LV_payment.DataBind();
            }
            if (e.CommandName == "deletepayment")
            {
                var DC_RENTAL = maga_DataContext.DC_RENTAL;
                INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (_pay == null) return;
                DC_INVOICE.INV_TBL_PAYMENT.DeleteOnSubmit(_pay);
                DC_INVOICE.SubmitChanges();
                RNT_TBL_RESERVATION currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                if (currRes != null)
                {
                    var listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == currRes.id && x.is_complete == 1 && x.direction == 0 && x.pr_total.HasValue).ToList();
                    decimal totalPayed = listPay.Count == 0 ? 0 : listPay.Sum(x => x.pr_total.Value);

                    _pay = DC_INVOICE.INV_TBL_PAYMENT.FirstOrDefault(x => x.rnt_pid_reservation == IdReservation && x.is_complete == 1 && x.direction == 0);
                    if (_pay != null)
                    {
                        currRes.prRefundTotal = totalPayed;
                        currRes.prRefundDate = _pay.pay_date;
                        currRes.prRefundPayMode = _pay.pay_mode;
                    }
                    else
                    {
                        currRes.prRefundTotal = 0;
                        currRes.prRefundDate = (DateTime?)null;
                        currRes.prRefundPayMode = "";
                    }
                    DC_RENTAL.SubmitChanges();
                }
                LDS_payment.DataBind();
                LV_payment.SelectedIndex = -1;
                LV_payment.DataBind();

            }
            if (e.CommandName == "createinvoice")
            {
                INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (_pay == null) return;
                RNT_TBL_RESERVATION currRes = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                if (currRes == null) return;
                invUtils.payment_checkInvoice(_pay, currRes);
                LDS_payment.DataBind();
                LV_payment.SelectedIndex = -1;
                LV_payment.DataBind();
                if (onSave != null) { onSave(this, new EventArgs()); }
            }
        }
        protected string getInvoiceLink(long id)
        {
            INV_TBL_INVOICE _inv = DC_INVOICE.INV_TBL_INVOICE.FirstOrDefault(x => x.inv_pid_payment == id);
            if (_inv == null) return "";
            return "<a onclick=\"INV_openSelection('"+_inv.id+"'); return false;\" href='#' style=\"margin-top: 6px; margin-right: 5px;\">Fattura</a>";
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
            Label lbl_pr_noInvoice = e.Item.FindControl("lbl_pr_noInvoice") as Label;
            Label lbl_pr_total = e.Item.FindControl("lbl_pr_total") as Label;
            Label lbl_pay_date = e.Item.FindControl("lbl_pay_date") as Label;
            LinkButton lnk_createinvoice = e.Item.FindControl("lnk_createinvoice") as LinkButton;

            INV_TBL_INVOICE _inv = DC_INVOICE.INV_TBL_INVOICE.FirstOrDefault(x => x.inv_pid_payment == lbl_id.Text.ToInt64());
            if (lnk_createinvoice != null)
                lnk_createinvoice.Visible = _inv == null;

            if (lbl_is_complete == null || lbl_id == null) return;
            if (lbl_is_complete.Text == "1")
            {
 
            }
            LinkButton lnk_edit = e.Item.FindControl("lnk_edit") as LinkButton;
            if (lnk_edit != null)
            {
                lnk_edit.Text = lbl_is_complete.Text == "0" ? "Completa" : "Modifica";
                return;
            }

            DropDownList drp_is_complete = e.Item.FindControl("drp_is_complete") as DropDownList;
            DropDownList drp_pay_mode = e.Item.FindControl("drp_pay_mode") as DropDownList;
            DropDownList drp_rnt_reservation_part = e.Item.FindControl("drp_rnt_reservation_part") as DropDownList;
            RadNumericTextBox ntxt_pr_invoice = e.Item.FindControl("ntxt_pr_invoice") as RadNumericTextBox;
            RadDateTimePicker rdtp_pay_date = e.Item.FindControl("rdtp_pay_date") as RadDateTimePicker;
            Label lbl_pay_mode = e.Item.FindControl("lbl_pay_mode") as Label;
            Label lbl_rnt_reservation_part = e.Item.FindControl("lbl_rnt_reservation_part") as Label;
            
            if (drp_is_complete != null
                && drp_pay_mode != null
                && ntxt_pr_invoice != null
                && lbl_pay_mode != null
                )
            {
                rdtp_pay_date.SelectedDate = lbl_pay_date.Text != "" ? lbl_pay_date.Text.JSCal_stringToDateTime() : DateTime.Now;

                drp_is_complete.Enabled = drp_pay_mode.Enabled = lbl_is_complete.Text == "0";
                ntxt_pr_invoice.Value = (lbl_pr_total.Text.ToDecimal() - lbl_pr_noInvoice.Text.ToDecimal()).objToDouble();
             
                drp_is_complete.Items.Clear();
                drp_is_complete.Items.Add(new ListItem("SI", "1"));
                drp_is_complete.Items.Add(new ListItem("NO", "0"));

                drp_pay_mode.DataSource = invProps.CashPlaceLK.OrderBy(x => x.title).ToList();
                drp_pay_mode.DataTextField = "title";
                drp_pay_mode.DataValueField = "code";
                drp_pay_mode.DataBind();
                drp_pay_mode.setSelectedValue(lbl_pay_mode.Text);

                drp_rnt_reservation_part.DataSource = AppSettings.RNT_RESERVATION_PAYMENT_PARTs;
                drp_rnt_reservation_part.DataTextField = "title";
                drp_rnt_reservation_part.DataValueField = "code";
                drp_rnt_reservation_part.DataBind();
                drp_rnt_reservation_part.setSelectedValue(lbl_rnt_reservation_part.Text);
            }
        }


        protected void lnk_saveRefund_Click(object sender, EventArgs e)
        {
            if (ntxt_prRefundTotal.Value.objToDecimal() == 0) return;
            if (!rdtp_prRefundDate.SelectedDate.HasValue) return;
            string pay_cause = "res_refund";
            var DC_RENTAL = maga_DataContext.DC_RENTAL;
            RNT_TBL_RESERVATION currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currRes != null)
            {
                INV_TBL_PAYMENT _pay = new INV_TBL_PAYMENT();
                _pay.rnt_pid_reservation = currRes.id;
                _pay.rnt_reservation_code = currRes.code;
                _pay.pay_cause = pay_cause;
                _pay.direction = 0;
                _pay.pay_mode = "";
                _pay.pid_invoice = 0;
                _pay.pid_supplier = 0;
                _pay.pid_client = 0;
                _pay.pid_place = 0;
                _pay.description = "";
                _pay.pr_tf = 0;
                _pay.pr_tax = 0;
                _pay.is_complete = 0;
                _pay.pay_pid_txn = 0;
                _pay.pay_txn_gross = 0;
                _pay.pay_date = null;
                _pay.dtExpire = currRes.block_expire;
                _pay.creation_pid_user = 1;
                _pay.dtCreation = DateTime.Now;

                _pay.state_pid = 1;
                _pay.state_body = "";
                _pay.state_date = DateTime.Now;
                _pay.state_pid_user = 1;
                _pay.state_subject = "Nuovo pagamento";
                _pay.state_pid_pptxn = 0;
                _pay.pr_total = ntxt_prRefundTotal.Value.objToDecimal();
                _pay.rnt_reservation_part = pay_cause;
                _pay.is_complete = 1;
                _pay.state_pid = 1;
                _pay.state_subject = "Completato";
                _pay.state_pid_user = UserAuthentication.CurrentUserID;
                _pay.pay_date = rdtp_prRefundDate.SelectedDate;
                _pay.pay_mode = drp_prRefundPayMode.SelectedValue;
                _pay.state_date = rdtp_prRefundDate.SelectedDate;
                _pay.pr_noInvoice = _pay.pr_total.objToDecimal();

                DC_INVOICE.INV_TBL_PAYMENT.InsertOnSubmit(_pay);
                DC_INVOICE.SubmitChanges();
                _pay.code = _pay.id.ToString().fillString("0", 7, false);
                DC_INVOICE.SubmitChanges();

                var listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == currRes.id && x.is_complete == 1 && x.direction == 0 && x.pr_total.HasValue).ToList();
                decimal totalPayed = listPay.Count == 0 ? 0 : listPay.Sum(x => x.pr_total.Value);

                currRes.prRefundTotal = totalPayed;
                currRes.prRefundDate = rdtp_prRefundDate.SelectedDate;
                currRes.prRefundPayMode = drp_prRefundPayMode.SelectedValue;
                DC_RENTAL.SubmitChanges();
                rdtp_prRefundDate.SelectedDate = (DateTime?)null;
                drp_prRefundPayMode.setSelectedValue("");
                ntxt_prRefundTotal.Value = 0;
                rntUtils.reservation_checkPartPayment(currRes, false);
            }
            LDS_payment.DataBind();
            LV_payment.SelectedIndex = -1;
            LV_payment.DataBind();
        }
    }
}