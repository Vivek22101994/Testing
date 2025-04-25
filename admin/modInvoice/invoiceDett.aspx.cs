using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ModAuth;
using RentalInRome.data;

namespace ModInvoice.admin.modInvoice
{
    public partial class invoiceDett : adminBasePage
    {
        protected dbInvInvoiceTBL currTBL;
        protected long IdInvoice
        {
            get { return HfId.Value.ToInt64(); }
            set { HfId.Value = value.ToString(); }
        }
        private List<dbInvInvoiceItemTBL> TMPcurrItemList;
        private List<dbInvInvoiceItemTBL> currItemList
        {
            get
            {
                if (TMPcurrItemList == null)
                    if (ViewState["currItemList"] != null)
                    {
                        TMPcurrItemList =
                            PConv.DeserArrToList((object[])ViewState["currItemList"],
                                                 typeof(dbInvInvoiceItemTBL)).Cast<dbInvInvoiceItemTBL>().ToList();
                    }
                    else
                        TMPcurrItemList = new List<dbInvInvoiceItemTBL>();
                return TMPcurrItemList;
            }
            set
            {
                ViewState["currItemList"] = PConv.SerialList(value.Cast<object>().ToList());
                TMPcurrItemList = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdInvoice = Request.QueryString["id"].ToInt64();
                drp_ownerType_DataBind();
                drp_owner_locCountry_DataBind();
                fillData();
            }
        }
        protected void drp_owner_locCountry_DataBind()
        {
            drp_owner_locCountry.DataSource = authProps.CountryLK.OrderBy(x => x.title);
            drp_owner_locCountry.DataTextField = "title";
            drp_owner_locCountry.DataValueField = "title";
            drp_owner_locCountry.DataBind();
        }
        protected void drp_cashInOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            drp_docCase_DataBind();
        }
        protected void drp_docCase_DataBind()
        {
            drp_docCase.DataSource = invProps.CashDocCaseLK.Where(x => (drp_cashInOut.getSelectedValueInt() == 1 && x.viewIn == 1) || (drp_cashInOut.getSelectedValueInt() != 1 && x.viewOut == 1));
            drp_docCase.DataTextField = "code";
            drp_docCase.DataValueField = "id";
            drp_docCase.DataBind();
            drp_docCase.Items.Insert(0, new ListItem("-non definito-", "0"));
        }
        protected void drp_cashTaxID_DataBind(ref DropDownList drp_cashTaxID)
        {
            drp_cashTaxID.DataSource = invProps.CashTaxLK.OrderBy(x => x.id);
            drp_cashTaxID.DataTextField = "code";
            drp_cashTaxID.DataValueField = "id";
            drp_cashTaxID.DataBind();
        }
        protected void drp_ownerType_DataBind()
        {
            drp_ownerType.DataSource = authProps.ClientTypeLK.Where(x => x.isActive == 1);
            drp_ownerType.DataTextField = "title";
            drp_ownerType.DataValueField = "code";
            drp_ownerType.DataBind();
            //drp_ownerType.Items.Insert(0, new ListItem("-tutti-", ""));
        }
        protected void drp_ownerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            drp_owner_DataBind();
        }
        protected void drp_owner_DataBind()
        {
            drp_owner.DataSource = authProps.ClientTBL.Where(x => x.isActive == 1 && x.typeCode == drp_ownerType.SelectedValue).OrderBy(x => x.nameFull);
            drp_owner.DataTextField = "nameFull";
            drp_owner.DataValueField = "id";
            drp_owner.DataBind();
            drp_owner.Items.Insert(0, new ListItem("-non definito-", "0"));
        }
        protected void drp_owner_SelectedIndexChanged(object sender, EventArgs e)
        {
            ownerSelect();
        }
        private void fillData()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                currTBL = dc.dbInvInvoiceTBLs.SingleOrDefault(x => x.id == IdInvoice);
                if (currTBL == null)
                {
                    CloseRadWindow("");
                    pnlDett.Visible = false;
                    return;
                    IdInvoice = 0;
                    currTBL = new dbInvInvoiceTBL();
                    dbInvInvoiceTBL lastTBL = dc.dbInvInvoiceTBLs.Where(x => x.docType == txt_docType.Text).OrderByDescending(x => x.docYear).ThenByDescending(x => x.docYearCounter).FirstOrDefault();
                    if (lastTBL != null)
                    {
                        txt_docNum.Text = DateTime.Now.Year + "-" + ((lastTBL.docYearCounter + 1) + "").fillString("0", 5, false);
                        rdp_docIssueDate.MinDate = lastTBL.docIssueDate.Value.Date;
                    }
                    else
                    {
                        txt_docNum.Text = DateTime.Now.Year + "-" + ("1").fillString("0", 5, false);
                    }
                    ltrTitle.Text = "Emmissione di una Fattura Nuova";
                    lnkSave.Text = "<span>Crea Fattura</span>";

                    rdp_docIssueDate.SelectedDate = DateTime.Now.Date;
                }
                else
                {
                    ltrTitle.Text = "Fattura #:" + currTBL.docNum;
                    lnkSave.Text = "<span>Salva Modifiche</span>";
                    HL_pdfView.Visible = true;
                    HL_pdfView.NavigateUrl = "invoicePdf.aspx?uid=" + currTBL.uid;
                    HL_pdfGet.Visible = true;
                    HL_pdfGet.CssClass = "btnDownload";
                    HL_pdfGet.NavigateUrl = App.RP + "admin/modPdf/createPdf.aspx?url=" + (App.HOST + App.RP + "admin/modInvoice/invoicePdf.aspx?uid=" + currTBL.uid).urlEncode() + "&filename=" + (currTBL.docNum + ".pdf").urlEncode();

                    txt_docNum.Text = currTBL.docNum;
                    txt_docNum.ReadOnly = true;
                    rdp_docIssueDate.SelectedDate = currTBL.docIssueDate;
                    rdp_docIssueDate.Enabled = false;
                    rdp_docExpiryDate.SelectedDate = currTBL.docExpiryDate;
                    rdp_docExpiryDate.MinDate = currTBL.docIssueDate.Value.Date;
                        
                    dbInvCashDocumentTBL docTBL = dc.dbInvCashDocumentTBLs.SingleOrDefault(x => x.extType == "invoice" && x.extUid == currTBL.uid);
                    if (docTBL != null)
                    {
                        HL_cashDoc.Visible = true;
                        HL_cashDoc.NavigateUrl = "invCashDocumentDett.aspx?id=" + docTBL.id;
                    }

                }
                // owner
                drp_ownerType.setSelectedValue(currTBL.ownerType);
                drp_owner_DataBind();
                drp_owner.setSelectedValue(currTBL.ownerId.ToString());
                //currTBL.ownerUid = txt_ownerUid.Text;
                //currTBL.ownerCode = txt_ownerCode.Text;
                txt_ownerNameFull.Text = currTBL.ownerNameFull;
                //currTBL.owner_docType = txt_owner_docType.Text;
                //currTBL.owner_docNum = txt_owner_docNum.Text;
                //currTBL.owner_docIssuePlace = txt_owner_docIssuePlace.Text;
                //currTBL.owner_docIssueDate = txt_owner_docIssueDate.Text;
                //currTBL.owner_docExpiryDate = txt_owner_docExpiryDate.Text;
                txt_owner_docVat.Text = currTBL.owner_docVat;
                txt_owner_docCf.Text = currTBL.owner_docCf;
                drp_owner_locCountry.setSelectedValue(currTBL.owner_locCountry, "Italy");
                txt_owner_locState.Text = currTBL.owner_locState;
                txt_owner_locCity.Text = currTBL.owner_locCity;
                txt_owner_locAddress.Text = currTBL.owner_locAddress;
                txt_owner_locZipCode.Text = currTBL.owner_locZipCode;
                //currTBL.xxx = txt_.Text;
                //currTBL.xxx = txt_.Text;

                // cash
                drp_cashInOut.setSelectedValue(currTBL.cashInOut);
                drp_docCase_DataBind();
                drp_docCase.setSelectedValue(currTBL.docCaseId);
                HF_cashTaxFree.Value = currTBL.cashTaxFree.ToString();
                HF_cashTaxAmount.Value = currTBL.cashTaxAmount.ToString();
                HF_cashTotalAmount.Value = currTBL.cashTotalAmount.ToString();
                currTBL.cashPayed = 0;
                currTBL.cashUnpayed = 0;

                re_notesInner.Content = currTBL.notesInner;
                re_notesPublic.Content = currTBL.notesPublic;

                currItemList = dc.dbInvInvoiceItemTBLs.Where(x => x.pidInvoice == currTBL.id).ToList();
                fillItems();
            }
        }
        private void saveData()
        {
            string _error = saveItems();
            if (_error != "")
            {
                ltr_error.Text = _error;
                pnl_btnSave.Visible = false;
                pnl_error.Visible = true;
                return;
            }
            else
            {
                pnl_btnSave.Visible = true;
                pnl_error.Visible = false;
            }

            using (DCmodInvoice dc = new DCmodInvoice())
            {
                currTBL = dc.dbInvInvoiceTBLs.SingleOrDefault(x => x.id == IdInvoice);
                if (currTBL == null)
                {
                    currTBL = new dbInvInvoiceTBL();
                    currTBL.uid = Guid.NewGuid();
                    currTBL.createdDate = DateTime.Now;
                    currTBL.createdUserID = authUtils.CurrentUserID;
                    currTBL.createdUserNameFull = authUtils.CurrentUserName;
                    currTBL.docIssueDate = rdp_docIssueDate.SelectedDate.Value.Date;
                    currTBL.docYear = currTBL.docIssueDate.Value.Year;
                    currTBL.docType = txt_docType.Text;
                    int? _lastCounter = dc.dbInvInvoiceTBLs.Where(x => x.docType == txt_docType.Text && x.docYear.HasValue && x.docYear == currTBL.docYear && x.docYearCounter.HasValue).Max(x => x.docYearCounter);
                    if (_lastCounter == null) _lastCounter = 0;
                    currTBL.docYearCounter = _lastCounter + 1;
                    currTBL.docNum = currTBL.docYear + "-" + (currTBL.docYearCounter + "").fillString("0", 5, false);
                    dc.Add(currTBL);
                    dc.SaveChanges();
                    IdInvoice = currTBL.id;
                }
                currTBL.docExpiryDate = rdp_docExpiryDate.SelectedDate;
                currTBL.docCaseId = drp_docCase.getSelectedValueInt();
                currTBL.docCaseCode = drp_docCase.getSelectedText("-non definito-");

                // owner
                currTBL.ownerType = drp_ownerType.SelectedValue;
                currTBL.ownerId = drp_owner.getSelectedValueInt();
                //currTBL.ownerUid = txt_ownerUid.Text;
                //currTBL.ownerCode = txt_ownerCode.Text;
                currTBL.ownerNameFull = txt_ownerNameFull.Text;
                //currTBL.owner_docType = txt_owner_docType.Text;
                //currTBL.owner_docNum = txt_owner_docNum.Text;
                //currTBL.owner_docIssuePlace = txt_owner_docIssuePlace.Text;
                //currTBL.owner_docIssueDate = txt_owner_docIssueDate.Text;
                //currTBL.owner_docExpiryDate = txt_owner_docExpiryDate.Text;
                currTBL.owner_docVat = txt_owner_docVat.Text;
                currTBL.owner_docCf = txt_owner_docCf.Text;
                currTBL.owner_locCountry = drp_owner_locCountry.SelectedValue;
                currTBL.owner_locState = txt_owner_locState.Text;
                currTBL.owner_locCity = txt_owner_locCity.Text;
                currTBL.owner_locAddress = txt_owner_locAddress.Text;
                currTBL.owner_locZipCode = txt_owner_locZipCode.Text;
                //currTBL.xxx = txt_.Text;
                //currTBL.xxx = txt_.Text;

                // cash
                currTBL.cashInOut = drp_cashInOut.getSelectedValueInt();
                currTBL.cashTaxFree = HF_cashTaxFree.Value.ToDecimal();
                currTBL.cashTaxAmount = HF_cashTaxAmount.Value.ToDecimal();
                currTBL.cashTotalAmount = HF_cashTotalAmount.Value.ToDecimal();
                currTBL.cashPayed = 0;
                currTBL.cashUnpayed = 0;

                currTBL.notesInner = re_notesInner.Content;
                currTBL.notesPublic = re_notesPublic.Content;

                // items
                List<dbInvInvoiceItemTBL> currlist = dc.dbInvInvoiceItemTBLs.Where(x => x.pidInvoice == currTBL.id).ToList();
                foreach (dbInvInvoiceItemTBL currItem in currlist)
                {
                    dc.Delete(currItem);
                }
                dc.SaveChanges();
                currlist = currItemList;
                foreach (dbInvInvoiceItemTBL currItem in currlist)
                {
                    currItem.pidInvoice = currTBL.id;
                    dc.Add(currItem);
                }
                dc.SaveChanges();
                currTBL.OnChanged();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
            }
        }
        protected void ownerSelect()
        {
            AuthClientTBL tmpTBL = authProps.ClientTBL.SingleOrDefault(x => x.id == drp_owner.getSelectedValueInt());
            if (tmpTBL == null)
                tmpTBL = new  AuthClientTBL();
            txt_ownerNameFull.Text = tmpTBL.nameFull;
            // doc
            txt_owner_docVat.Text = tmpTBL.docVat;
            txt_owner_docCf.Text = tmpTBL.docCf;

            // loc
            txt_owner_locAddress.Text = tmpTBL.locAddress;
            txt_owner_locZipCode.Text = tmpTBL.locZipCode;
            txt_owner_locCity.Text = tmpTBL.locCity;
            txt_owner_locState.Text = tmpTBL.locState;
            drp_owner_locCountry.setSelectedValue(tmpTBL.locCountry, "Italy");

            re_notesPublic.Content = tmpTBL.notesInvoice;

        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected void lnk_ownerSelect_Click(object sender, EventArgs e)
        {
            ownerSelect();
        }
        protected void fillItems()
        {
            LV.DataSource = currItemList.OrderBy(x => x.sequence);
            LV.DataBind();
            foreach (ListViewDataItem _item in LV.Items)
            {
                Label lbl_cashTaxID = _item.FindControl("lbl_cashTaxID") as Label;
                DropDownList drp_cashTaxID = _item.FindControl("drp_cashTaxID") as DropDownList;
                drp_cashTaxID_DataBind(ref drp_cashTaxID);
                drp_cashTaxID.setSelectedValue(lbl_cashTaxID.Text);
            }
        }
        protected string saveItems()
        {
            string _error = "";
            decimal cashTaxFree = 0;
            decimal cashTaxAmount = 0;
            decimal cashTotalAmount = 0;
            if (txt_ownerNameFull.Text == "")
                _error += "- inserire 'Nome Completo' di intestatario<br/>";
            if (txt_owner_locAddress.Text == "")
                _error += "- inserire 'Indirizzo' di intestatario<br/>";
            if (txt_owner_locZipCode.Text == "")
                _error += "- inserire 'CAP' di intestatario<br/>";
            if (txt_owner_locCity.Text == "")
                _error += "- inserire 'Città' di intestatario<br/>";
            if (txt_owner_locState.Text == "")
                _error += "- inserire 'Provincia/Stato' di intestatario<br/>";
            if (drp_owner_locCountry.SelectedValue == "")
                _error += "- inserire 'Nazione/Location' di intestatario<br/>";
            List<dbInvInvoiceItemTBL> _list = new List<dbInvInvoiceItemTBL>();
            foreach (ListViewDataItem _item in LV.Items)
            {
                TextBox txt_code = _item.FindControl("txt_code") as TextBox;
                TextBox txt_description = _item.FindControl("txt_description") as TextBox;
                TextBox txt_quantity = _item.FindControl("txt_quantity") as TextBox;
                TextBox txt_cashTotalAmount = _item.FindControl("txt_cashTotalAmount") as TextBox;
                DropDownList drp_cashTaxID = _item.FindControl("drp_cashTaxID") as DropDownList;
                Label lbl_id = _item.FindControl("lbl_id") as Label;
                Label lbl_sequence = _item.FindControl("lbl_sequence") as Label;

                decimal taxPercentage = 0;
                int taxId = drp_cashTaxID.getSelectedValueInt();
                dbInvCashTaxLK currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId);
                if (currTax != null)
                {
                    taxPercentage = currTax.taxAmount.objToDecimal();
                }

                dbInvInvoiceItemTBL _new = new dbInvInvoiceItemTBL();
                _new.pidInvoice = 0;
                _new.description = txt_description.Text;
                _new.id = new Guid(lbl_id.Text);
                _new.sequence = lbl_sequence.Text.ToInt32();
                _new.quantityAmount = txt_quantity.Text.ToInt32();
                _new.quantityType = "pz";
                _new.cashTotalAmount = txt_cashTotalAmount.Text.ToDecimal();
                _new.cashTaxID = taxId;
                decimal TMPcashTaxFreeDecimal = Decimal.Divide(_new.cashTotalAmount.objToDecimal(), (1 + taxPercentage));
                decimal TMPcashTaxFreeRounded = Decimal.Round(TMPcashTaxFreeDecimal, 2);
                if (TMPcashTaxFreeRounded > TMPcashTaxFreeDecimal)
                    TMPcashTaxFreeRounded -= new Decimal(0.01);
                _new.cashTaxFree = TMPcashTaxFreeRounded;
                _new.cashTaxAmount = Decimal.Subtract(_new.cashTotalAmount.Value, TMPcashTaxFreeRounded);
                _new.singleUnitPrice = Decimal.Divide(TMPcashTaxFreeRounded, _new.quantityAmount.Value);
                _list.Add(_new);
                cashTaxFree += _new.cashTaxFree.objToDecimal();
                cashTaxAmount += _new.cashTaxAmount.objToDecimal();
                cashTotalAmount += _new.cashTotalAmount.objToDecimal();
                if (_new.description == "")
                    _error += "- inserire 'Descrizione' del oggetto #" + _new.sequence + "<br/>";
                if (_new.quantityAmount == 0)
                    _error += "- inserire 'Quantità' del oggetto #" + _new.sequence + "<br/>";
                if (_new.singleUnitPrice == 0)
                    _error += "- inserire 'Prezzo Totale Ivato' del oggetto #" + _new.sequence + "<br/>";
            }
            if (_list.Count == 0)
                _error += "- inserire Almeno 1 Oggetto della fattura<br/>";
            currItemList = _list;
            ltr_taxList.Text = _list.taxListHTML(ltr_taxListTemplate.Text);
            HF_cashTaxFree.Value = cashTaxFree.ToString();
            HF_cashTaxAmount.Value = cashTaxAmount.ToString();
            HF_cashTotalAmount.Value = cashTotalAmount.ToString();
            fillItems();
            return _error;
        }
        protected void lnk_add_new_Click(object sender, EventArgs e)
        {
            saveItems();
            List<dbInvInvoiceItemTBL> _list = currItemList;
            dbInvInvoiceItemTBL _new = new dbInvInvoiceItemTBL();
            _new.pidInvoice = 0;
            _new.description = "";
            _new.id = Guid.NewGuid();
            _new.sequence = _list.Count() + 1;
            _new.quantityAmount = 1;
            _new.quantityType = "pz";
            _new.cashTotalAmount = 0;
            _new.cashTaxID = 0;
            _new.cashTaxFree = 0;
            _new.cashTaxAmount = 0;
            _new.singleUnitPrice = 0;
            _list.Add(_new);
            currItemList = _list;
            fillItems();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                saveItems();
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                List<dbInvInvoiceItemTBL> _list = currItemList;
                dbInvInvoiceItemTBL _del = _list.SingleOrDefault(x => x.id == new Guid(lbl_id.Text));
                if (_del != null)
                    _list.Remove(_del);
                List<dbInvInvoiceItemTBL> _listNew = new List<dbInvInvoiceItemTBL>();
                foreach (dbInvInvoiceItemTBL _item in _list)
                {
                    _item.sequence = _listNew.Count + 1;
                    _listNew.Add(_item);
                }
                currItemList = _listNew;
                fillItems();
            }
        }
        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            string _error = saveItems();
            if (_error != "")
            {
                ltr_error.Text = _error;
                pnl_btnSave.Visible = false;
                pnl_error.Visible = true;
            }
            else
            {
                pnl_btnSave.Visible = true;
                pnl_error.Visible = false;
            }
            fillItems();
        }
    }
}

