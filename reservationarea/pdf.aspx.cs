using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{
    public partial class pdf : basePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (resUtils.CurrentIdReservation_gl != 0 && resUtils.CurrentIdReservation_gl < 150000)
            {
                Response.Redirect("/reservationarea/arrivaldeparture.aspx", true);
                return;
            }
        }
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
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
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
            HF_id.Value = _currTBL.id.ToString();

            if (_estTB.pid_zone == 9 || _currTBL.agentID.objToInt64() != 0)
                LV_invoice.Visible = false;

            if(_currTBL.isWL == 1)
                LV_invoice.Visible = true;

            HL_voucher.Enabled = false;
            HL_voucher.CssClass = "btnDownload inattivo";

            string _error = "";
            if (_currTBL.state_pid == 4)
            {
                if (_currTBL.limo_isCompleted != 1)
                    _error += "*Complete Arrival and Departure Information.<br/>";
                if ((_currTBL.agentID.objToInt64() == 0 && _currTBL.cl_isCompleted != 1) || (_currTBL.agentID.objToInt64() != 0 && _currTBL.agentClientID.objToInt64() == 0)
                    || (_currTBL.isWL ==1 && _currTBL.cl_isCompleted != 1))
                    _error += "*Complete Personal Data Information.<br/>";
                string url_voucher;
                string filename;
                if (_error != "")
                {
                    ltr_error.Text = "To download your voucher you have to complete following missing information:<br/><br/>" + _error;
                    return;
                }
                url_voucher = CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_voucher.aspx?uid=" + _currTBL.uid_2;
                filename = "reservation_voucher-code_" + _currTBL.code + ".pdf";
                HL_voucher.Enabled = true;
                HL_voucher.CssClass = "btnDownload";
                HL_voucher.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher.urlEncode() + "&filename=" + filename.urlEncode();
                return;
            }
            _error += "*Complete the Payment.<br/>";
            ltr_error.Text = "" + _error;
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
        protected void LV_invoice_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_uid = e.Item.FindControl("lbl_uid") as Label;
            HyperLink HL_pdf = e.Item.FindControl("HL_pdf") as HyperLink;
            if (lbl_uid == null || lbl_id == null || HL_pdf == null) return;

            HL_pdf.Enabled = false;
            HL_pdf.CssClass = "btnDownload inattivo";
            if (_currTBL.cl_isCompleted != 1) return;
            string url_voucher = CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=" + lbl_uid.Text;
            string filename = "RiR-reservation_invoice-code_" + _currTBL.code + ".pdf";
            HL_pdf.Enabled = true;
            HL_pdf.CssClass = "btnDownload";
            HL_pdf.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher.urlEncode() + "&filename=" + filename.urlEncode();


        }
    }
}