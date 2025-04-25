using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_inv_invoiceNotes : System.Web.UI.UserControl
    {
        protected magaInvoice_DataContext DC_INVOICE;
        public event EventHandler onModify;
        public event EventHandler onSave;
        public event EventHandler onCancel;
        public string inv_notesPublic
        {
            get { return re_description.Content; }
            set { re_description.Content = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
            }
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