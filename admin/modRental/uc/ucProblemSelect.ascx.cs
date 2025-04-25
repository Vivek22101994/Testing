using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental.uc
{
    public partial class ucProblemSelect : System.Web.UI.UserControl
    {
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
        public int ProblemID
        {
            get { return HF_problemID.Value.ToInt32(); }
            set { HF_problemID.Value = value.ToString(); }
        }
        public string ProblemDesc
        {
            get { return ltr_problemDesc.Text; }
            set { ltr_problemDesc.Text = value; }
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
            drp_problemID.DataSource = rntProps.ProblemTBL.Where(x => x.type == "rnt_res" || x.type == "all").OrderBy(x => x.sequence);
            drp_problemID.DataTextField = "title";
            drp_problemID.DataValueField = "id";
            drp_problemID.DataBind();
            drp_problemID.Items.Insert(0, new ListItem("-nessun problema-", "0"));
            drp_problemID.setSelectedValue(ProblemID);
            re_problemDesc.Content = ProblemDesc;
            showView();
        }
        private void FillDataFromControls()
        {
            ProblemID = drp_problemID.getSelectedValueInt();
            ProblemDesc = re_problemDesc.Content;
            showView();
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
            if (drp_problemID.getSelectedValueInt() != 0)
            {
                pnl_ok.Visible = true;
                pnl_no.Visible = false;
            }
            else
            {
                pnl_ok.Visible = false;
                pnl_no.Visible = true;
            }
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