using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_discount : System.Web.UI.UserControl
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
        public decimal pr_total
        {
            get { return HF_pr_total.Value.ToDecimal(); }
            set { HF_pr_total.Value = value.ToString(); }
        }
        public decimal pr_percentage
        {
            get { return HF_pr_percentage.Value.ToDecimal(); }
            set { HF_pr_percentage.Value = value.ToString(); }
        }
        public decimal pr_discount_owner
        {
            get { return HF_pr_discount_owner.Value.ToDecimal(); }
            set { HF_pr_discount_owner.Value = value.ToString(); }
        }
        public decimal pr_discount_commission
        {
            get { return HF_pr_discount_commission.Value.ToDecimal(); }
            set { HF_pr_discount_commission.Value = value.ToString(); }
        }
        public int pr_discount_user
        {
            get { return HF_pr_discount_user.Value.ToInt32(); }
            set { HF_pr_discount_user.Value = value.ToString(); }
        }
        public int pr_discount_custom
        {
            get { return HF_pr_discount_custom.Value.ToInt32(); }
            set { HF_pr_discount_custom.Value = value.ToString(); }
        }
        public string pr_discount_desc
        {
            get { return HF_pr_discount_desc.Value; }
            set { HF_pr_discount_desc.Value = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void checkMode()
        {
            PH_default.Visible = drp_custom.SelectedValue == "0";
            PH_custom.Visible = !PH_default.Visible;
        }
        public void FillControls()
        {
            if (pr_discount_commission != 0 || pr_discount_owner != 0)
            {
                pnl_ok.Visible = true;
                pnl_no.Visible = false;
            }
            else
            {
                pnl_ok.Visible = false;
                pnl_no.Visible = true;
            }
            drp_custom.setSelectedValue(pr_discount_custom.ToString());
            txt_pr_discount_commission.Text = pr_discount_commission.ToString();
            txt_pr_discount_owner.Text = pr_discount_owner.ToString();
            txt_pr_discount.Text = (pr_discount_commission + pr_discount_owner).ToString();
            chk_commission.Checked = pr_discount_commission != 0;
            chk_owner.Checked = pr_discount_owner != 0;
            ltr_user.Text = AdminUtilities.usr_adminName(pr_discount_user, "");
            ltr_body.Text = pr_discount_desc;
            txt_body.Text = pr_discount_desc;
            checkMode();
            showView();
        }
        private void FillDataFromControls()
        {
            if (drp_custom.SelectedValue == "1")
            {
                pr_discount_commission = txt_pr_discount_commission.Text.ToDecimal();
                pr_discount_owner = txt_pr_discount_owner.Text.ToDecimal();
            }
            else
            {
                if (!chk_commission.Checked && !chk_owner.Checked)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                                        "alertError",
                                        "alert('si prega di selezionare a chi applicare lo sconto');", true);
                    return;
                }
                decimal _dicount = txt_pr_discount.Text.ToDecimal();
                if (_dicount == 0)
                {
                    pr_discount_commission = 0;
                    pr_discount_owner = 0;
                }
                else
                {
                    if (!chk_commission.Checked)
                    {
                        pr_discount_owner = _dicount;
                        pr_discount_commission = 0;
                    }
                    else if (!chk_owner.Checked)
                    {
                        pr_discount_commission = _dicount;
                        pr_discount_owner = 0;
                    }
                    else
                    {
                        pr_discount_commission = _dicount * pr_percentage / 100;
                        pr_discount_owner = _dicount - pr_discount_commission;
                    }
                }
            }
            pr_discount_custom = drp_custom.getSelectedValueInt(0).objToInt32();
            pr_discount_desc = txt_body.Text;
            pr_discount_user = UserAuthentication.CurrentUserID;
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

        protected void drp_custom_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkMode();
        }
    }
}