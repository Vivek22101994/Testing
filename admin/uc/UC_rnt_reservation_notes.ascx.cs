using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_notes : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_RESERVATION _currTBL;
        public event EventHandler onModify;
        public event EventHandler onSave;
        public event EventHandler onCancel;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
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
        public bool IsChanged
        {
            get { return HF_isChanged.Value == "1"; }
            set { HF_isChanged.Value = value ? "1" : "0"; }
        }
        public long IdReservation
        {
            get { return HF_IdReservation.Value.ToInt32(); }
            set { HF_IdReservation.Value = value.ToString(); }
        }
        public string Body
        {
            get { return txt_body.Text; }
            set { txt_body.Text = value; }
        }
        public string Title
        {
            set { ltrTitle.Text = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public void FillControls()
        {
            if (Body != "")
            {
                pnl_ok.Visible = true;
                pnl_no.Visible = false;
            }
            else
            {
                pnl_ok.Visible = false;
                pnl_no.Visible = true;
            }
            txt_body.Text = Body;
            showView();
        }
        private void FillDataFromControls()
        {
            FillControls();
            if (onSave != null) { onSave(this, new EventArgs()); }
        }
        protected void showModify()
        {
            pnl_view.Visible = false;
            pnl_edit.Visible = true;
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void showView()
        {
            pnl_view.Visible = true;
            pnl_edit.Visible = false;
        }
        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            FillControls();
            if (onCancel != null) { onCancel(this, new EventArgs()); }
        }
        protected void lnk_edit_Click(object sender, EventArgs e)
        {
            showModify();
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
    }
}