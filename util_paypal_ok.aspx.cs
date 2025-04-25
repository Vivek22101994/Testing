using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class util_paypal_ok : mainBasePage
    {
        protected magaInvoice_DataContext DC_INVOICE;
        protected magaPayPal_DataContext DC_PAYPAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            DC_PAYPAL = maga_DataContext.DC_PAYPAL;

            if (!IsPostBack)
            {
                string tx = Request.QueryString["tx"];
                if (string.IsNullOrEmpty(tx)) return;
                string cm = Request.QueryString["cm"];
                if (string.IsNullOrEmpty(cm))
                { 
                    // todo ha pagato un'altra cosa
                    return;
                }
                PP_TBL_TRANSACTION _txn = DC_PAYPAL.PP_TBL_TRANSACTIONs.FirstOrDefault(x => x.txn_id == tx && x.txn_custom == cm.ToLower() && x.pay_payment_status.ToUpper() == "COMPLETED");
                if (_txn == null)
                {
                    // transazione non esiste
                    ltr_title.Text = "Error: Transaction doesn't exist!!!";
                    return;
                }
                int _id = _txn.txn_custom.Replace("invpay_", "").ToInt32();
                INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == _id);
                if (_pay != null && _pay.pay_pid_txn == _txn.id && _pay.is_complete == 1)
                {
                    RNT_TBL_RESERVATION _currTBL = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _pay.rnt_pid_reservation);
                    if (_currTBL != null)
                    {
                        ltr_title.Text = "Dear "+_currTBL.cl_name_honorific+" "+_currTBL.cl_name_full+",<br/>";
                        ltr_title.Text += "We have received your payment for Booking confirmation in amount of " + _pay.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;<br/>";
                        ltr_title.Text += "for the reservation number: " + _currTBL.code + "<br/>";
                        ltr_title.Text += "<br/>";
                        ltr_title.Text += "<br/>";
                        ltr_title.Text += "<br/>";
                        HL_goToArea.NavigateUrl = CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id;
                        HL_goToArea.Visible = true;
                    }
                    else
                    {
                        // prenotazione non esiste
                        ltr_title.Text = "Error: Reservation id:" + _pay.rnt_pid_reservation + " doesn't exist or payment is not complete!!!";
                        return;
                    }
                }
                else
                {
                    // pagamento non esiste oppure non e lagato a questa transazione
                    ltr_title.Text = "Error: Payment id:" + _id + " doesn't exist or is not complete!!!";
                    return;
                }
            }
        }
    }
}
