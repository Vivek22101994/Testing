using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModInvoice;

namespace RentalInRome.admin
{
    public partial class inv_invoice_form : adminBasePage
    {
        protected INV_TBL_INVOICE _currTBL;
        private magaInvoice_DataContext DC_INVOICE;
        public long IdInvoice
        {
            get { return HF_id.Value.ToInt64(); }
            set { HF_id.Value = value.ToString(); }
        }
        private INV_TBL_INVOICE TMPcurrInvoice;
        public INV_TBL_INVOICE tblInvoice
        {
            get
            {
                if (TMPcurrInvoice == null)
                {
                    DC_INVOICE = maga_DataContext.DC_INVOICE;
                    TMPcurrInvoice = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
                }
                return TMPcurrInvoice ?? new INV_TBL_INVOICE();
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
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_reservation_list";
            UC_items.onModify += new EventHandler(UC_items_onModify);
            UC_items.onSave += new EventHandler(UC_items_onSave);
            UC_items.onCancel += new EventHandler(UC_items_onCancel);
            UC_cl.onSave += new EventHandler(UC_cl_onSave);
            UC_inv_invoiceNotes.onSave += new EventHandler(UC_inv_invoiceNotes_onSave);
        }
        void UC_inv_invoiceNotes_onSave(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Shadowbox.close", "parent.Shadowbox.close();", true);
                return;
            }
            _currTBL.inv_notesPublic = UC_inv_invoiceNotes.inv_notesPublic;
            DC_INVOICE.SubmitChanges();
            UC_inv_invoiceNotes.showView();
        }
        void UC_cl_onSave(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Shadowbox.close", "parent.Shadowbox.close();", true);
                return;
            }
            _currTBL.cl_name_full = UC_cl.cl_name_full;
            _currTBL.cl_loc_address = UC_cl.cl_loc_address;
            _currTBL.cl_loc_zip_code = UC_cl.cl_loc_zip_code;
            _currTBL.cl_loc_city = UC_cl.cl_loc_city;
            _currTBL.cl_loc_state = UC_cl.cl_loc_state;
            _currTBL.cl_loc_province = UC_cl.cl_loc_province;
            _currTBL.cl_doc_cf_num = UC_cl.cl_doc_cf_num;
            _currTBL.cl_doc_vat_num = UC_cl.cl_doc_vat_num;
            _currTBL.idCodice = UC_cl.codice_destinatario;
            _currTBL.cl_loc_country = UC_cl.cl_loc_country;
            DC_INVOICE.SubmitChanges();

            //string token = digital_invoice.Fill_data();
            //INV_TBL_INVOICE_ITEM itemRnt = DC_INVOICE.INV_TBL_INVOICE_ITEM.SingleOrDefault(x => x.pid_invoice == _inv.id && x.sequence == 1);
            //if (itemRnt != null)
            //    digital_invoice.Callinvoicefunction(_currTBL, itemRnt, token);`       

            UC_cl.FillControls(_currTBL);
        }
        void UC_items_onModify(object sender, EventArgs e)
        {
        }
        void UC_items_onSave(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            var currItemList = DC_INVOICE.INV_TBL_INVOICE_ITEM.Where(x => x.pid_invoice == IdInvoice).ToList();
            decimal pr_total = 0;
            decimal pr_tf = 0;
            decimal pr_tax = 0;
            foreach (var _new in currItemList)
            {
                pr_total += _new.price_total.objToDecimal();
                pr_tf += _new.price_tf.objToDecimal();
                pr_tax += _new.price_tax.objToDecimal();
            }
            HF_pr_total.Value = pr_total.ToString();
            HF_pr_tf.Value = pr_tf.ToString();
            HF_pr_tax.Value = pr_tax.ToString();
            _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Shadowbox.close", "parent.Shadowbox.close();", true);
                return;
            }
            _currTBL.pr_total = HF_pr_total.Value.objToDecimal();
            _currTBL.pr_tf = HF_pr_tf.Value.objToDecimal();
            _currTBL.pr_tax = HF_pr_tax.Value.objToDecimal();
            DC_INVOICE.SubmitChanges();
            UC_items.FillControls(currItemList);
            TMPcurrInvoice = _currTBL;
        }
        void UC_items_onCancel(object sender, EventArgs e)
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                IdInvoice = Request.QueryString["id"].ToInt64();
                _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
                if (_currTBL == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Shadowbox.close", "parent.Shadowbox.close();", true);
                    return;
                }
                TMPcurrInvoice = _currTBL;
                UC_cl.FillControls(_currTBL);
                UC_inv_invoiceNotes.inv_notesPublic = _currTBL.inv_notesPublic;
                UC_inv_invoiceNotes.showView();

                UC_items.IdInvoice = IdInvoice;
                UC_items.IsAddingBlocked = (_currTBL.rnt_pid_reservation.HasValue && _currTBL.rnt_pid_reservation != 0);
                var currItemList = DC_INVOICE.INV_TBL_INVOICE_ITEM.Where(x => x.pid_invoice == IdInvoice).ToList();
                UC_items.FillControls(currItemList);
                pnl_lnkUpdate.Visible = (_currTBL.rnt_pid_reservation.HasValue && _currTBL.rnt_pid_reservation != 0);
                HF_code.Value = txt_invoice_number.Text = _currTBL.code;
                HF_inv_dtInvoice.Value = HF_invoice_date.Value = _currTBL.inv_dtInvoice.JSCal_dateToString();
                HF_pr_tf.Value = _currTBL.pr_tf.objToDecimal().ToString("N2");
                HF_pr_tax.Value = _currTBL.pr_tax.objToDecimal().ToString("N2");
                HF_pr_total.Value = _currTBL.pr_total.objToDecimal().ToString("N2");
                //HF_is_complete.Value = _currTBL.is_complete.ToString();
                string url_voucher = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=" + _currTBL.uid;
                string url_voucher_pdf = CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=" + _currTBL.uid;

                HL_view.Enabled = true;
                HL_view.CssClass = "btnDownload";
                HL_view.NavigateUrl = url_voucher;

                string filename = "RiR-reservation_invoice-code_" + _currTBL.code + ".pdf";
                HL_pdf.Enabled = true;
                HL_pdf.CssClass = "btnDownload";
                HL_pdf.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher_pdf.urlEncode() + "&filename=" + filename.urlEncode();

                using (DCmodInvoice dc = new DCmodInvoice())
                {
                    dbInvInvoiceTBL currTBL = dc.dbInvInvoiceTBLs.SingleOrDefault(x => x.docType == "notaDiCredito" && x.refererInvoiceID == _currTBL.id);
                    if (currTBL != null)
                    {
                        lnkNotaDiCredito.Text = "<span>Aggiorna Nota Credito</span>";
                        lnkNotaDiCredito.ToolTip = "Aggiorna la nota di Credito per questa fattura";
                    }
                    else
                    {
                        lnkNotaDiCredito.Text = "<span>Crea Nota Credito</span>";
                        lnkNotaDiCredito.ToolTip = "Crea la nota di Credito per questa fattura";
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal_" + Unique + "();", true);
        }

        protected void lnk_save_Click(object sender, EventArgs e)
        {
            _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('errore imprevisto, contattare assistenza!');", true);
                return;
            }
            DC_INVOICE.SubmitChanges();

        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('errore imprevisto, contattare assistenza!');", true);
                return;
            }
            DC_INVOICE.INV_TBL_INVOICE.DeleteOnSubmit(_currTBL);
            DC_INVOICE.SubmitChanges();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Shadowbox.close", "parent.Shadowbox.close();", true);
        }
        protected void lnkNotaDiCredito_Click(object sender, EventArgs e)
        {
            _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('errore imprevisto, contattare assistenza!');", true);
                return;
            }
            _currTBL.Create_notaDiCredito();
            Response.Redirect("inv_invoice_form.aspx?id=" + IdInvoice);

        }
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('errore imprevisto, contattare assistenza!');", true);
                return;
            }
            INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == _currTBL.inv_pid_payment);
            if (_pay == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('errore imprevisto, contattare assistenza!');", true);
                return;
            }
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            {
                RNT_TBL_RESERVATION _res = dcOld.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _currTBL.rnt_pid_reservation);
                if (_res == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('errore imprevisto, contattare assistenza!');", true);
                    return;
                }
                invUtils.payment_checkInvoice(_pay, _res);
                Response.Redirect("inv_invoice_form.aspx?id=" + IdInvoice);
            }
        }

        protected void lnk_update_invoice_number_Click(object sender, EventArgs e)
        {
            _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('errore inaspettato, contattare l'assistenza!');", true);
                return;
            }
            var otherNumber = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.code == txt_invoice_number.Text && x.id != _currTBL.id);
            if (otherNumber != null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('Lo stesso numero di fattura esiste già!');", true);
                return;
            }

            _currTBL.code = txt_invoice_number.Text;
            DC_INVOICE.SubmitChanges();
            HF_code.Value = txt_invoice_number.Text = _currTBL.code;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('Numero fattura aggiornato correttamente');", true);
        }

        protected void lnk_update_invoice_date_Click(object sender, EventArgs e)
        {
            _currTBL = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == IdInvoice);
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('errore inaspettato, contattare l'assistenza!');", true);
                return;
            }
            _currTBL.inv_dtInvoice = HF_invoice_date.Value.JSCal_stringToDate();
            DC_INVOICE.SubmitChanges();
            HF_inv_dtInvoice.Value = HF_invoice_date.Value = _currTBL.inv_dtInvoice.JSCal_dateToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('Data fattura aggiornata correttamente');", true);
        }
    }
}
