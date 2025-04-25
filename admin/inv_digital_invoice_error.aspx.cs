using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class inv_digital_invoice_error : adminBasePage
    {
        magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                filldata();
            }
        }
        protected void filldata()
        {
            LV.DataSource = DC_INVOICE.INV_TBL_DIGITAL_INVOICE_ERROR.OrderByDescending(x=>x.datetime).ToList();
            LV.DataBind();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "sendInvoice")
            {
                Label lbl_invoice_id = e.Item.FindControl("lbl_invoice_id") as Label;                     
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;

                var _inv = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == lbl_invoice_id.Text.objToInt64());
                if (_inv != null)
                {
                    var itemRnt = DC_INVOICE.INV_TBL_INVOICE_ITEM.FirstOrDefault(x => x.pid_invoice == lbl_invoice_id.Text.objToInt64() && x.sequence == 1);
                    if (itemRnt != null)
                    {
                        string token = digital_invoice.Fill_data();
                        string response = digital_invoice.Callinvoicefunction(_inv, itemRnt, token,digital_invoice.TipoDocumento.fattura);
                        if (response != "")
                        {
                            _inv.responseUniqueId = response;
                            DC_INVOICE.SubmitChanges();
                        }

                        var invoiceError = DC_INVOICE.INV_TBL_DIGITAL_INVOICE_ERROR.SingleOrDefault(x => x.id == lbl_id.Text.objToInt64());
                        if (invoiceError != null)
                        {
                            DC_INVOICE.INV_TBL_DIGITAL_INVOICE_ERROR.DeleteOnSubmit(invoiceError);
                            DC_INVOICE.SubmitChanges();
                            filldata();
                        }

                    }
                    
                }
            }
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;

                Label lbl_invoice_id = e.Item.FindControl("lbl_invoice_id") as Label;
                Label lbl_invoice_code = e.Item.FindControl("lbl_invoice_code") as Label;
                HtmlAnchor lbl_res_area = e.Item.FindControl("lbl_res_area") as HtmlAnchor;
                Label lbl_res_code = e.Item.FindControl("lbl_res_code") as Label;           

                var _inv = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == lbl_invoice_id.Text.objToInt64());
                if (_inv != null)
                {
                    lbl_invoice_code.Text = _inv.code;
                    lbl_res_code.Text = _inv.rnt_reservation_code;

                    var currReservation = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _inv.rnt_pid_reservation);
                    if (currReservation != null)
                    {
                        lbl_res_area.HRef = App.HOST_SSL + "/reservationarea/login.aspx?auth=" + currReservation.unique_id + "&personal=true";
                    }
                }
            }
        }
    }
}