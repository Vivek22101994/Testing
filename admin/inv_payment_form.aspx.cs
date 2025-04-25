using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class inv_payment_form : adminBasePage
    {
        private magaRental_DataContext DC_RENTAL;
        protected INV_TBL_PAYMENT _currTBL;
        private magaInvoice_DataContext DC_INVOICE;
        public long IdPayment
        {
            get { return HF_id.Value.ToInt64(); }
            set { HF_id.Value = value.ToString(); }
        }
        public long IdReservation
        {
            get { return HF_IdReservation.Value.ToInt32(); }
            set { HF_IdReservation.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                IdPayment = Request.QueryString["id"].ToInt64();
                _currTBL = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == IdPayment);
                if (_currTBL == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Shadowbox.close", "parent.Shadowbox.close();", true);
                    return;
                }
                HF_code.Value = _currTBL.code;
                HF_pay_cause.Value = _currTBL.pay_cause;
                HF_pr_total.Value = _currTBL.pr_total.objToDecimal().ToString("N2");
                HF_is_complete.Value = _currTBL.is_complete.ToString();
            }
        }

        protected void lnk_save_Click(object sender, EventArgs e)
        {
        }
    }
}
