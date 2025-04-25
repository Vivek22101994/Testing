using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.pdfgenerator
{
    public partial class pdf_rnt_reservation_invoice_download : mainBasePage
    {
        public long IdInvoice
        {
            get
            {
                return HF_IdInvoice.Value.ToInt64();
            }
            set
            {
                HF_IdInvoice.Value = value.ToString();
            }
        }
        public INV_TBL_INVOICE tblInvoice
        {
            get
            {
                if (_currInvoice == null)
                    _currInvoice = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
                return _currInvoice ?? new INV_TBL_INVOICE();
            }
        }
        private INV_TBL_INVOICE _currInvoice;
        public int _currLang
        {
            get
            {
                return HF_pid_lang.Value.ToInt32();
            }
            set
            {
                HF_pid_lang.Value = value.ToString();
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        private magaUser_DataContext DC_USER;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                if (Request.QueryString["uid"] != null)
                {
                    Guid _unique = new Guid(Request.QueryString["uid"]);
                    _currInvoice = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.uid == _unique);
                    if (_currInvoice != null)
                    {
                        IdInvoice = _currInvoice.id;
                        fillData();
                        return;
                    }
                }
                Response.Clear();
                Response.End();
                return;
            }
        }
        public void fillData()
        {
            _currInvoice = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currInvoice == null)
            {
                Response.Clear();
                Response.End();
                return;
            }
            else
            {
                var objInvPayment = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == _currInvoice.inv_pid_payment);
                if (objInvPayment != null)
                {
                    switch (objInvPayment.pay_mode)
                    {
                        case "paypal":
                            ltr_paymentType.Text = "carta di credito - paypal";
                            break;
                        case "sella":
                            ltr_paymentType.Text = "carta di credito - banca sella";
                            break;
                        case "bank":
                            ltr_paymentType.Text = "bonifico";
                            break;
                        case "cash":
                            ltr_paymentType.Text = "contanti";
                            break;
                        case "wordpay":
                            ltr_paymentType.Text = "wordpay";
                            break;
                    }
                }
            }
            _currLang = 2;
            //_currLang = _currInvoice.cl_pid_lang.HasValue ? _currInvoice.cl_pid_lang.Value : 2;


        }
    }
}