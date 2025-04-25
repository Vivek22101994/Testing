using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_inv_invoiceItems : System.Web.UI.UserControl
    {
        protected magaInvoice_DataContext DC_INVOICE;
        private List<INV_TBL_INVOICE_ITEM> CURRENT_ITEMS_;
        private List<INV_TBL_INVOICE_ITEM> CURRENT_ITEMS
        {
            get
            {
                if (CURRENT_ITEMS_ == null)
                    if (ViewState["CURRENT_ITEMS"] != null)
                    {
                        CURRENT_ITEMS_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_ITEMS"],
                                                 typeof(INV_TBL_INVOICE_ITEM)).Cast<INV_TBL_INVOICE_ITEM>().ToList();
                    }
                    else
                        CURRENT_ITEMS_ = new List<INV_TBL_INVOICE_ITEM>();

                return CURRENT_ITEMS_;
            }
            set
            {
                ViewState["CURRENT_ITEMS"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_ITEMS_ = value;
            }
        }

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
        public bool IsAddingBlocked
        {
            get { return HF_addingBlocked.Value == "1"; }
            set { lnk_add_new.Visible = !value; HF_addingBlocked.Value = value ? "1" : "0"; }
        }
        public bool IsChanged
        {
            get { return HF_isChanged.Value == "1"; }
            set { HF_isChanged.Value = value ? "1" : "0"; }
        }
        public long IdInvoice
        {
            get { return HF_IdInvoice.Value.ToInt32(); }
            set { HF_IdInvoice.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
        }
        public void FillControls(List<INV_TBL_INVOICE_ITEM> currList)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            CURRENT_ITEMS = currList;
            fillItems();
            showView();
        }
        protected void drp_cashTaxID_DataBind(ref DropDownList drp_cashTaxID)
        {
            drp_cashTaxID.DataSource = invProps.CashTaxLK.OrderBy(x => x.id);
            drp_cashTaxID.DataTextField = "code";
            drp_cashTaxID.DataValueField = "id";
            drp_cashTaxID.DataBind();
        }
        protected void fillItems()
        {
            LV_view.DataSource = CURRENT_ITEMS;
            LV_view.DataBind();
            LV.DataSource = CURRENT_ITEMS.OrderBy(x => x.sequence);
            LV.DataBind();
            LinkButton lnk_tmp = LV.FindControl("lnk_add_new") as LinkButton;
            if (lnk_tmp != null)
                lnk_tmp.Visible = !IsAddingBlocked;
            foreach (ListViewDataItem _item in LV.Items)
            {
                lnk_tmp = _item.FindControl("lnk_del") as LinkButton;
                TextBox txt_code = _item.FindControl("txt_code") as TextBox;
                TextBox txt_description = _item.FindControl("txt_description") as TextBox;
                TextBox txt_quantity = _item.FindControl("txt_quantity") as TextBox;
                TextBox txt_price_total = _item.FindControl("txt_price_total") as TextBox;
                DropDownList drp_cashTaxID = _item.FindControl("drp_cashTaxID") as DropDownList;
                Label lbl_cashTaxID = _item.FindControl("lbl_cashTaxID") as Label;
                drp_cashTaxID_DataBind(ref drp_cashTaxID);
                if (lbl_cashTaxID.Text.ToInt32() == 0) lbl_cashTaxID.Text = "1";
                drp_cashTaxID.setSelectedValue(lbl_cashTaxID.Text);
                if (IsAddingBlocked)
                {
                    lnk_tmp.Visible = false;
                    //txt_code.ReadOnly = true;
                    //txt_description.ReadOnly = true;
                    txt_quantity.ReadOnly = true;
                    txt_price_total.ReadOnly = true;
                }
            }
        }
        protected void showModify()
        {
            pnl_view.Visible = false;
            pnl_edit.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Unique + "_tinyEditor", "setTinyEditors( " + Unique + "_editors, true); ", true);
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void showView()
        {
            pnl_view.Visible = true;
            pnl_edit.Visible = false;
        }
        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            CURRENT_ITEMS = DC_INVOICE.INV_TBL_INVOICE_ITEM.Where(x => x.pid_invoice == IdInvoice).ToList();
            fillItems();
            showView();
            if (onCancel != null) { onCancel(this, new EventArgs()); }
        }
        protected void lnk_edit_Click(object sender, EventArgs e)
        {
            showModify();
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected string saveItems()
        {
            string _error = "";
            List<INV_TBL_INVOICE_ITEM> _list = new List<INV_TBL_INVOICE_ITEM>();
            foreach (ListViewDataItem _item in LV.Items)
            {
                TextBox txt_code = _item.FindControl("txt_code") as TextBox;
                TextBox txt_description = _item.FindControl("txt_description") as TextBox;
                TextBox txt_quantity = _item.FindControl("txt_quantity") as TextBox;
                TextBox txt_price_total = _item.FindControl("txt_price_total") as TextBox;
                DropDownList drp_cashTaxID = _item.FindControl("drp_cashTaxID") as DropDownList;
                Label lbl_id = _item.FindControl("lbl_id") as Label;
                Label lbl_sequence = _item.FindControl("lbl_sequence") as Label;
                if (txt_quantity.Text.ToInt32() == 0) txt_quantity.Text = "1";

                decimal taxPercentage = 0;
                int taxId = drp_cashTaxID.getSelectedValueInt();
                var currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId);
                if (currTax != null)
                {
                    taxPercentage = currTax.taxAmount.objToDecimal();
                }

                INV_TBL_INVOICE_ITEM _new = new INV_TBL_INVOICE_ITEM();
                _new.pid_invoice = 0;
                _new.is_deleted = 0;
                _new.code = txt_code.Text;
                _new.description = txt_description.Text;
                _new.id = new Guid(lbl_id.Text);
                _new.sequence = lbl_sequence.Text.ToInt32();
                _new.quantity = txt_quantity.Text.ToInt32();
                _new.quantity_type = "pz";

                _new.price_total = txt_price_total.Text.ToDecimal();
                _new.price_tax_id = taxId;
                decimal TMPcashTaxFreeDecimal = Decimal.Divide(_new.price_total.objToDecimal(), (1 + taxPercentage));
                decimal TMPcashTaxFreeRounded = Decimal.Round(TMPcashTaxFreeDecimal, 2);
                if (TMPcashTaxFreeRounded > TMPcashTaxFreeDecimal)
                    TMPcashTaxFreeRounded -= new Decimal(0.01);
                _new.price_tf = TMPcashTaxFreeRounded;
                _new.price_tax = Decimal.Subtract(_new.price_total.Value, TMPcashTaxFreeRounded);
                _new.price_unit = Decimal.Divide(TMPcashTaxFreeRounded, _new.quantity.Value);
                _list.Add(_new);
                if (_new.code == "")
                    _error += "- inserire 'Item' del oggetto #" + _new.sequence + "<br/>";
                if (_new.description == "")
                    _error += "- inserire 'Description' del oggetto #" + _new.sequence + "<br/>";
                if (_new.quantity == 0)
                    _error += "- inserire 'Quantity' del oggetto #" + _new.sequence + "<br/>";
                if (_new.price_total == 0)
                    _error += "- inserire 'Prezzo Totale' del oggetto #" + _new.sequence + "<br/>";
            }
            if (_list.Count == 0)
                _error += "- inserire Almeno 1 Oggetto della fattura<br/>";
            CURRENT_ITEMS = _list;
            return _error;
        }
        protected void lnk_add_new_Click(object sender, EventArgs e)
        {
            List<INV_TBL_INVOICE_ITEM> _list = CURRENT_ITEMS;
            INV_TBL_INVOICE_ITEM _new = new INV_TBL_INVOICE_ITEM();
            _new.pid_invoice = 0;
            _new.is_deleted = 0;
            _new.code = "";
            _new.description = "";
            _new.id = Guid.NewGuid();
            _new.sequence = _list.Count() + 1;
            _new.quantity = 1;
            _new.quantity_type = "pz";
            _new.price_total = 0;
            _new.price_tax_id = 0;
            _new.price_tf = Math.Round((_new.price_total.objToDecimal() / new decimal(1.1)), 2);
            _new.price_tax = _new.price_total - _new.price_tf;
            _new.price_unit = _new.price_tf / _new.quantity;
            _list.Add(_new);
            CURRENT_ITEMS = _list;
            fillItems();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "elimina")
            {
                saveItems();
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                List<INV_TBL_INVOICE_ITEM> _list = CURRENT_ITEMS;
                INV_TBL_INVOICE_ITEM _del = _list.SingleOrDefault(x => x.id == new Guid(lbl_id.Text));
                if (_del != null)
                    _list.Remove(_del);
                List<INV_TBL_INVOICE_ITEM> _listNew = new List<INV_TBL_INVOICE_ITEM>();
                foreach (INV_TBL_INVOICE_ITEM _item in _list)
                {
                    _item.sequence = _listNew.Count + 1;
                    _listNew.Add(_item);
                }
                CURRENT_ITEMS = _listNew;
                fillItems();
            }
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            string _error = saveItems();
            if (_error != "")
            {
                ltr_error.Text = _error;
                pnl_error.Visible = true;
                return;
            }
            else
            {
                pnl_error.Visible = false;
            }
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            List<INV_TBL_INVOICE_ITEM> _list = CURRENT_ITEMS;
            foreach (INV_TBL_INVOICE_ITEM _item in _list)
            {
                var currItem = DC_INVOICE.INV_TBL_INVOICE_ITEM.SingleOrDefault(x => x.id == _item.id);
                if (currItem == null)
                {
                    _item.pid_invoice = IdInvoice;
                    DC_INVOICE.INV_TBL_INVOICE_ITEM.InsertOnSubmit(_item);
                }
                else
                {
                    currItem.code = _item.code;
                    currItem.description = _item.description;
                    currItem.sequence = _item.sequence;
                    currItem.quantity = _item.quantity;
                    currItem.quantity_type = _item.quantity_type;
                    currItem.price_total = _item.price_total;
                    currItem.price_tax_id = _item.price_tax_id;
                    currItem.price_tf = _item.price_tf;
                    currItem.price_tax = _item.price_tax;
                    currItem.price_unit = _item.price_unit;
                }
                DC_INVOICE.SubmitChanges();
            }
            if (onSave != null) { onSave(this, new EventArgs()); }

        }

    }
}