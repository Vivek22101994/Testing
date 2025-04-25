using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.uc
{
    public partial class UC_rnt_reservation_pdf : System.Web.UI.UserControl
    {
        private RNT_TBL_RESERVATION _currTBL;
        private magaRental_DataContext DC_RENTAL;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public long IdReservation
        {
            get
            {
                return HF_IdReservation.Value.ToInt32();
            }
            set
            {
                HF_IdReservation.Value = value.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
            }
        }
        public void fillData()
        {
            HL_voucher.Enabled = false;
            HL_voucher.CssClass = "btnDownload inattivo";

            HL_invoice.Enabled = false;
            HL_invoice.CssClass = "btnDownload inattivo";

            DC_RENTAL = maga_DataContext.DC_RENTAL;
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert_" + Unique, "alert('" + IdReservation + "');", true);
                return;
            }
            if (_currTBL.state_pid == 4)
            {
                string url_voucher;
                string filename;
                if (_currTBL.limo_isCompleted != 1 || _currTBL.cl_isCompleted != 1) 
                {
                    string _error = "";
                    if(_currTBL.limo_isCompleted != 1)
                        _error += "*Complete Arrival and Departure Information.<br/>";
                    if (_currTBL.cl_isCompleted != 1)
                        _error += "*Complete Personal Data Information.<br/>";
                    ltr_error.Text = "To download your voucher you have to complete following missing information:<br/><br/>" + _error;
                    return;
                }
                url_voucher = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_voucher.aspx?id=" + IdReservation;
                filename = "reservation_voucher-code_" + _currTBL.code + ".pdf";
                HL_voucher.Enabled = true;
                HL_voucher.CssClass = "btnDownload";
                HL_voucher.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher.urlEncode() + "&filename=" + filename.urlEncode();

                INV_TBL_PAYMENT _pay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.FirstOrDefault(x => x.is_complete == 1 && x.direction == 1 && x.rnt_pid_reservation == _currTBL.id);
                if (_pay != null) 
                {
                    INV_TBL_INVOICE _inv = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE.FirstOrDefault(x => x.is_payed == 1 && x.rnt_pid_reservation == _currTBL.id && x.inv_pid_payment == _pay.id);
                    if (_inv != null)
                    {
                        url_voucher = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?id=" + _inv.id;
                        filename = "RiR-reservation_invoice-code_" + _currTBL.code + ".pdf";
                        HL_invoice.Enabled = true;
                        HL_invoice.CssClass = "btnDownload";
                        HL_invoice.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher.urlEncode() + "&filename=" + filename.urlEncode();
                    }
                }
            }
        }


        protected void lnk_voucher_Click(object sender, EventArgs e)
        {
        }

        protected void lnk_invoice_Click(object sender, EventArgs e)
        {

        }
    }
}