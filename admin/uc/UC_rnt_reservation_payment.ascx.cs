using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_payment : System.Web.UI.UserControl
    {
        protected magaInvoice_DataContext DC_INVOICE;
        public event EventHandler onSave;
        public long IdReservation
        {
            get { return HF_id.Value.ToInt32(); }
            set
            {
                DC_INVOICE = maga_DataContext.DC_INVOICE;
                HF_id.Value = value.ToString();
            }
        }
        public bool Reload
        {
            get
            {
                return HF_reload.Value == "1";
            }
            set
            {
                HF_reload.Value = value ? "1" : "0";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
            }
        }
        protected void LV_payment_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_is_complete = e.Item.FindControl("lbl_is_complete") as Label;
            if (lbl_is_complete == null || lbl_id == null) return;
            if (e.CommandName == "salva")
            {
                DropDownList drp_is_complete = e.Item.FindControl("drp_is_complete") as DropDownList;
                DropDownList drp_pay_mode = e.Item.FindControl("drp_pay_mode") as DropDownList;
                RadNumericTextBox ntxt_pr_invoice = e.Item.FindControl("ntxt_pr_invoice") as RadNumericTextBox;
                DropDownList drp_chargeFeeInvoice = e.Item.FindControl("drp_chargeFeeInvoice") as DropDownList;
                RadNumericTextBox ntxt_chargeFeePerc = e.Item.FindControl("ntxt_chargeFeePerc") as RadNumericTextBox;
                if (
                    drp_is_complete != null
                    && drp_pay_mode != null
                    )
                {

                    INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (_pay == null) return;
                    Reload = false;
                    if (drp_is_complete.getSelectedValueInt(0) == 1 && _pay.is_complete != 1)
                    {
                        _pay.is_complete = 1;
                        _pay.pay_date = DateTime.Now;
                        _pay.state_pid = 1;
                        _pay.state_date = DateTime.Now;
                        _pay.state_subject = "Completato";
                        _pay.state_pid_user = UserAuthentication.CurrentUserID;
                        Reload = true;
                    }
                    _pay.pay_mode = drp_pay_mode.SelectedValue;
                    _pay.pr_noInvoice = _pay.pr_total.objToDecimal() - ntxt_pr_invoice.Value.objToDecimal();
                    _pay.chargeFeePerc = ntxt_chargeFeePerc.Value.objToDecimal();
                    _pay.chargeFee = 0;
                    if (_pay.chargeFeePerc != 0)
                    {
                        decimal chargeFeeDecimal = Decimal.Divide(Decimal.Multiply(_pay.pr_total.objToDecimal(), _pay.chargeFeePerc.objToDecimal()), 100);
                        decimal chargeFeeRounded = Decimal.Round(chargeFeeDecimal, 2);
                        if (chargeFeeRounded < chargeFeeDecimal)
                            chargeFeeRounded += new Decimal(0.01);
                        _pay.chargeFee = chargeFeeRounded;
                    }
                    _pay.chargeFeeInvoice = drp_chargeFeeInvoice.getSelectedValueInt();
                    DC_INVOICE.SubmitChanges();
                    RNT_TBL_RESERVATION tmp = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                    if (tmp != null)
                        invUtils.payment_onChange(tmp, false);
                    LDS_payment.DataBind();
                    LV_payment.SelectedIndex = -1;
                    LV_payment.DataBind();
                    if (onSave != null) { onSave(this, new EventArgs()); }
                }
            } 
            if (e.CommandName == "del")
            {
                INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (_pay == null) return;
                Reload = false;
                DC_INVOICE.INV_TBL_PAYMENT.DeleteOnSubmit(_pay);
                DC_INVOICE.SubmitChanges();
                RNT_TBL_RESERVATION tmp = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                if (tmp != null)
                    invUtils.payment_onChange(tmp, false);
                LDS_payment.DataBind();
                LV_payment.SelectedIndex = -1;
                LV_payment.DataBind();
                if (onSave != null) { onSave(this, new EventArgs()); }
            }
            if (e.CommandName == "annulla")
            {
                LDS_payment.DataBind();
                LV_payment.SelectedIndex = -1;
                LV_payment.DataBind();
            }
        }
        protected void LV_payment_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            Label lbl_id = LV_payment.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            if (lbl_id != null)
            {
            }
        }
        protected void LV_payment_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_is_complete = e.Item.FindControl("lbl_is_complete") as Label;
            Label lbl_pr_noInvoice = e.Item.FindControl("lbl_pr_noInvoice") as Label;
            Label lbl_pr_total = e.Item.FindControl("lbl_pr_total") as Label;

            if (lbl_is_complete == null || lbl_id == null) return;
            if (lbl_is_complete.Text == "1")
            {

            }
            LinkButton lnk_delete = e.Item.FindControl("lnk_delete") as LinkButton;
            if (lnk_delete != null)
            {
                lnk_delete.Visible = UserAuthentication.CURRENT_USER_ROLE == 1;
            }
            LinkButton lnk_edit = e.Item.FindControl("lnk_edit") as LinkButton;
            if (lnk_edit != null)
            {
                lnk_edit.Text = lbl_is_complete.Text == "0" ? "Completa" : "Modifica";
                return;
            }
            DropDownList drp_is_complete = e.Item.FindControl("drp_is_complete") as DropDownList;
            DropDownList drp_pay_mode = e.Item.FindControl("drp_pay_mode") as DropDownList;
            RadNumericTextBox ntxt_pr_invoice = e.Item.FindControl("ntxt_pr_invoice") as RadNumericTextBox;
            DropDownList drp_chargeFeeInvoice = e.Item.FindControl("drp_chargeFeeInvoice") as DropDownList;
            RadNumericTextBox ntxt_chargeFeePerc = e.Item.FindControl("ntxt_chargeFeePerc") as RadNumericTextBox;
            Label lbl_pay_mode = e.Item.FindControl("lbl_pay_mode") as Label;
            Label lbl_chargeFeePerc = e.Item.FindControl("lbl_chargeFeePerc") as Label;
            Label lbl_chargeFeeInvoice = e.Item.FindControl("lbl_chargeFeeInvoice") as Label;
            if (drp_is_complete != null
                && drp_pay_mode != null
                && ntxt_pr_invoice != null
                && lbl_pay_mode != null
                )
            {
                drp_is_complete.Enabled = lbl_is_complete.Text == "0";
                ntxt_pr_invoice.Value = (lbl_pr_total.Text.ToDecimal() - lbl_pr_noInvoice.Text.ToDecimal()).objToDouble();

                drp_is_complete.Items.Clear();
                drp_is_complete.Items.Add(new ListItem("SI", "1"));
                drp_is_complete.Items.Add(new ListItem("NO", "0"));

                drp_pay_mode.DataSource = invProps.CashPlaceLK.OrderBy(x => x.title).ToList();
                drp_pay_mode.DataTextField = "title";
                drp_pay_mode.DataValueField = "code";
                drp_pay_mode.DataBind();
                drp_pay_mode.setSelectedValue(lbl_pay_mode.Text);

                drp_chargeFeeInvoice.Items.Clear();
                drp_chargeFeeInvoice.Items.Add(new ListItem("NO", "0"));
                drp_chargeFeeInvoice.Items.Add(new ListItem("SI", "1"));
                drp_chargeFeeInvoice.setSelectedValue(lbl_chargeFeeInvoice.Text);

                ntxt_chargeFeePerc.Value = lbl_chargeFeePerc.Text.objToDecimal().objToDouble();
            }
        }
    }
}