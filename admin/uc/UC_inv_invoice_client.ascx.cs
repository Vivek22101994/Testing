using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_inv_invoice_client : System.Web.UI.UserControl
    {
        protected magaInvoice_DataContext DC_INVOICE;
        public event EventHandler onModify;
        public event EventHandler onSave;
        public event EventHandler onCancel;
        public bool IsLocked
        {
            get { return pnl_lock.Visible; }
            set { pnl_lock.Visible = value; }
        }
        public bool IsEdit
        {
            get { return HF_isEdit.Value == "1"; }
            set { HF_isEdit.Value = value ? "1" : "0"; }
        }
        public string cl_name_full
        {
            get { return txt_cl_name_full.Text; }
        }
        public string cl_loc_address
        {
            get { return txt_cl_loc_address.Text; }
        }
        public string cl_loc_zip_code
        {
            get { return txt_cl_loc_zip_code.Text; }
        }
        public string cl_loc_city
        {
            get { return txt_cl_loc_city.Text; }
        }
        public string cl_loc_state
        {
            get { return txt_cl_loc_state.Text; }
        }
        public string cl_loc_province
        {
            get { return txt_cl_loc_province.Text; }
        }
        public string codice_destinatario
        {
            get { return txt_codice_destinatario.Text; }
        }
        public string cl_doc_cf_num
        {
            get { return txt_cl_doc_cf_num.Text; }
        }
        public string cl_doc_vat_num
        {
            get { return txt_cl_doc_vat_num.Text; }
        }
        public string cl_loc_country
        {
            get { return drp_cl_loc_country.SelectedValue; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
            }
        }
        public void FillControls(INV_TBL_INVOICE _currTBL)
        {
            txt_cl_name_full.Text = _currTBL.cl_name_full;
            txt_cl_loc_address.Text = _currTBL.cl_loc_address;
            txt_cl_loc_zip_code.Text = _currTBL.cl_loc_zip_code;
            txt_cl_loc_city.Text = _currTBL.cl_loc_city;
            txt_cl_loc_state.Text = _currTBL.cl_loc_state;
            txt_cl_loc_province.Text = _currTBL.cl_loc_province;
            txt_codice_destinatario.Text = _currTBL.idCodice;
            txt_cl_doc_cf_num.Text = _currTBL.cl_doc_cf_num;
            txt_cl_doc_vat_num.Text = _currTBL.cl_doc_vat_num;
            drp_cl_loc_country.DataBind();
            drp_cl_loc_country.setSelectedValue(_currTBL.cl_loc_country);
            ltr_clientDetailsHTML.Text = _currTBL.clientDetailsHTML();

            showView();
        }
        public void showModify()
        {
            pnl_view.Visible = false;
            pnl_edit.Visible = true;
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        public void showView()
        {
            pnl_view.Visible = true;
            pnl_edit.Visible = false;
        }
        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            showView();
            if (onCancel != null) { onCancel(this, new EventArgs()); }
        }
        protected void lnk_edit_Click(object sender, EventArgs e)
        {
            showModify();
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            showView();
            if (onSave != null) { onSave(this, new EventArgs()); }
        }
    }
}