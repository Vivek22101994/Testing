using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class invCashDocumentDett : adminBasePage
    {
        protected dbInvCashDocumentTBL currTBL;
        protected dbInvCashBookTBL currTBLCashBook;
        protected long IdDocument
        {
            get { return HfId.Value.ToInt64(); }
            set { HfId.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdDocument = Request.QueryString["id"].ToInt64();
                drp_BookNew_cashPlace_DataBind();
                drp_BookNew_cashType_DataBind();
                drp_ownerType_DataBind();
                fillData();
                rdtp_BookNew_cashDate.SelectedDate = DateTime.Now;
            }
        }
        protected void rapOwnerCont_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            long returnID = e.Argument.objToInt64();
            if (returnID == 0) return;
            drp_owner_DataBind();
            drp_owner.setSelectedValue(returnID.ToString());
        }
        protected void drp_BookNew_cashPlace_DataBind()
        {
            drp_BookNew_cashPlace.Items.Clear();
            drp_BookNew_cashPlace.Items.Add(new ListItem("-non definito-", "0|"));
            foreach (dbInvCashPlaceLK CashPlace in invProps.CashPlaceLK.Where(x => x.isActive == 1).OrderBy(x => x.title))
                drp_BookNew_cashPlace.Items.Add(new ListItem(CashPlace.title, CashPlace.id + "|" + CashPlace.type));
        }
        protected void drp_BookNew_cashPlace_SelectedIndexChanged(object sender, EventArgs e)
        {
            drp_BookNew_cashType_DataBind();
        }
        protected void drp_BookNew_cashType_DataBind()
        {
            drp_BookNew_cashType.DataSource = invProps.CashTypeLK.Where(x => x.isActive == 1 && x.placeTypes.Contains(drp_BookNew_cashPlace.SelectedValue.splitStringToList("|")[1])).OrderBy(x => x.title);
            drp_BookNew_cashType.DataTextField = "title";
            drp_BookNew_cashType.DataValueField = "id";
            drp_BookNew_cashType.DataBind();
            drp_BookNew_cashType.Items.Insert(0, new ListItem("-non definito-", "0"));
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
        private void fillData()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                currTBL = dc.dbInvCashDocumentTBLs.SingleOrDefault(x => x.id == IdDocument);
                if (currTBL == null)
                {
                    IdDocument = 0;
                    currTBL = new dbInvCashDocumentTBL();

                    pnl_cashBookNew.Visible = true;
                    pnl_cashBookHandle.Visible = false;
                    HfcashPayed.Value = "0";
                    ltrTitle.Text = "Nuovo Documento Prima Nota";
                    pnl_docPath.Visible = false;
                    pnl_docFiles.Visible = false;
                }
                else 
                {
                    // creazione cartella
                    string _folder = "files";
                    if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                        Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
                    _folder = "files/invCashDocument";
                    if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                        Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
                    if (!Directory.Exists(Path.Combine(App.SRP, _folder + "/docFiles")))
                        Directory.CreateDirectory(Path.Combine(App.SRP, _folder + "/docFiles"));
                    docPath.FileRoot = _folder;
                    docPath.FileName = currTBL.uid.ToString();
                    docPath.FilePath = currTBL.docPath;
                    LVfiles_DataBind();
                    pnl_docPath.Visible = true;
                    pnl_docFiles.Visible = true;

                    if (currTBL.extUid.HasValue && currTBL.extType == "invoice")
                    {
                        drp_cashInOut.Enabled = false;
                        txt_docNum.Enabled = false;
                        rdp_docIssueDate.Enabled = false;
                        txt_cashAmount.Enabled = false;
                        drp_docCase.Enabled = false;
                        drp_ownerType.Enabled = false;
                        drp_owner.Enabled = false;
                        re_docBody.Enabled = false;

                        HL_invoiceDett.Visible = true;
                        HL_invoiceDett.NavigateUrl = "invoiceDett.aspx?id=" + currTBL.extId;
                    }
                    ltrTitle.Text = "Documento Prima Nota #:" + currTBL.code;
                    pnl_cashBookNew.Visible = false;
                    List<dbInvCashBookTBL> cashBook = dc.dbInvCashBookTBLs.Where(x => x.pidDocument == IdDocument).ToList();
                    LV_cashBook.DataSource = dc.dbInvCashBookTBLs.Where(x => x.pidDocument == IdDocument);
                    LV_cashBook.DataBind();
                    HfcashPayed.Value = cashBook.Count > 0 ? cashBook.Sum(x => x.cashAmount).objToDecimal().ToString() : "0";
                    pnl_cashBookHandle.Visible = HfcashPayed.Value.ToDecimal() < currTBL.cashAmount;
                    Label lblPayed = LV_cashBook.FindControl("lblPayed") as Label;
                    if (lblPayed != null) lblPayed.Visible = HfcashPayed.Value.ToDecimal() >= currTBL.cashAmount;
                    lnk_BookNew_show.Visible = true;
                    lnk_BookNew_hide.Visible = false;
                    if (Request.QueryString["add"] == "true" && pnl_cashBookHandle.Visible)
                    {
                        pnl_cashBookNew.Visible = true;
                        lnk_BookNew_show.Visible = false;
                        lnk_BookNew_hide.Visible = true;
                    }
                }
                drp_cashInOut.setSelectedValue(currTBL.cashInOut);
                drp_docCase_DataBind();
                txt_docNum.Text = currTBL.docNum;
                rdp_docIssueDate.SelectedDate = currTBL.docIssueDate;
                txt_cashAmount.Text = currTBL.cashAmount.ToString();
                drp_docCase.setSelectedValue(currTBL.docCaseId);
                re_docBody.Content = currTBL.docBody;

                drp_ownerType.setSelectedValue(currTBL.ownerType);
                drp_owner_DataBind();
                drp_owner.setSelectedValue(currTBL.ownerId.ToString());
            }
        }
        private void saveData()
        {
            if (drp_docCase.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Seleziona  'Causale'.\", 340, 110);", true);
                return;
            }
            if (drp_owner.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Seleziona  'al nome di'.\", 340, 110);", true);
                return;
            }
            if (txt_BookNew_cashAmount.Text.ToDecimal() > 0)
            {
                if (drp_BookNew_cashPlace.SelectedValue == "0|")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Seleziona  'Movimento su'.\", 340, 110);", true);
                    return;
                }
                if (drp_BookNew_cashType.getSelectedValueInt() == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Seleziona 'Tipo Movimento'.\", 340, 110);", true);
                    return;
                }
            }
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                currTBL = dc.dbInvCashDocumentTBLs.SingleOrDefault(x => x.id == IdDocument);
                if (currTBL == null)
                {
                    currTBL = new dbInvCashDocumentTBL();
                    currTBL.uid = Guid.NewGuid();
                    currTBL.createdDate = DateTime.Now;
                    currTBL.createdUserID = authUtils.CurrentUserID;
                    currTBL.createdUserNameFull = authUtils.CurrentUserName;
                    dc.Add(currTBL);
                    dc.SaveChanges();
                    currTBL.code = currTBL.id.ToString().fillString("0", 6, false);
                    IdDocument = currTBL.id;
                }
                currTBL.docPath = docPath.FilePath;
                currTBL.cashInOut = drp_cashInOut.getSelectedValueInt();
                currTBL.docNum = txt_docNum.Text;
                currTBL.docIssueDate = rdp_docIssueDate.SelectedDate;
                currTBL.cashAmount = txt_cashAmount.Text.ToDecimal();
                currTBL.docCaseId = drp_docCase.getSelectedValueInt();
                currTBL.docCaseCode = drp_docCase.getSelectedText("-non definito-");
                currTBL.docBody = re_docBody.Content;

                currTBL.ownerType = drp_ownerType.SelectedValue;
                currTBL.ownerId = drp_owner.getSelectedValueInt();
                currTBL.ownerNameFull = drp_owner.getSelectedText("-non definito-");

                if (currTBL.cashAmount - HfcashPayed.Value.ToDecimal() < txt_BookNew_cashAmount.Text.ToDecimal())
                    txt_BookNew_cashAmount.Text = (currTBL.cashAmount - HfcashPayed.Value.ToDecimal()).ToString();
                if (txt_BookNew_cashAmount.Text.ToDecimal() > 0)
                {
                    currTBLCashBook = new dbInvCashBookTBL();
                    currTBLCashBook.uid = Guid.NewGuid();
                    dc.Add(currTBLCashBook);
                    dc.SaveChanges();
                    currTBLCashBook.code = currTBLCashBook.id.ToString().fillString("0", 6, false);
                    currTBLCashBook.pidDocument = currTBL.id;
                    currTBLCashBook.cashInOut = currTBL.cashInOut;
                    currTBLCashBook.cashAmount = txt_BookNew_cashAmount.Text.ToDecimal();
                    currTBLCashBook.cashDate = rdtp_BookNew_cashDate.SelectedDate;
                    currTBLCashBook.cashPlace = drp_BookNew_cashPlace.SelectedValue.splitStringToList("|")[0].ToInt32();
                    currTBLCashBook.cashPlaceType = drp_BookNew_cashPlace.SelectedValue.splitStringToList("|")[1];
                    currTBLCashBook.cashType = drp_BookNew_cashType.getSelectedValueInt();
                    currTBLCashBook.createdDate = DateTime.Now;
                    currTBLCashBook.createdUserID = authUtils.CurrentUserID;
                    currTBLCashBook.createdUserNameFull = authUtils.CurrentUserName;
                    dc.SaveChanges();
                }
                dc.SaveChanges();
                currTBL.OnChanged();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
            }
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

        protected void lnk_BookNew_fullCash_Click(object sender, EventArgs e)
        {
            txt_BookNew_cashAmount.Text = (txt_cashAmount.Text.ToDecimal() - HfcashPayed.Value.ToDecimal()).ToString();
        }
        protected void lnk_BookNew_show_Click(object sender, EventArgs e)
        {
            pnl_cashBookNew.Visible = true;
            lnk_BookNew_show.Visible = false;
            lnk_BookNew_hide.Visible = true;
        }
        protected void lnk_BookNew_hide_Click(object sender, EventArgs e)
        {
            pnl_cashBookNew.Visible = false;
            lnk_BookNew_show.Visible = true;
            lnk_BookNew_hide.Visible = false;
            txt_BookNew_cashAmount.Text = "0";
        }

        protected void LV_cashBook_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lblID = e.Item.FindControl("lblID") as Label;
                if (lblID == null) return;
                using (DCmodInvoice dc = new DCmodInvoice())
                {
                    currTBL = dc.dbInvCashDocumentTBLs.SingleOrDefault(x => x.id == IdDocument);
                    dbInvCashBookTBL currCashBook = dc.dbInvCashBookTBLs.SingleOrDefault(x => x.id == lblID.Text.ToInt32());
                    if (currCashBook != null && currTBL != null)
                    {
                        dc.Delete(currCashBook);
                        dc.SaveChanges();
                        currTBL.OnChanged();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Il pagamento è stato eliminato con successo.\", 340, 110);", true);
                        fillData();
                    }
                    else 
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Errore imprevisto.\", 340, 110);", true);
                    }
                }
            }
        }
        protected void LVfiles_DataBind()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                LVfiles.DataSource = dc.dbInvCashDocumentFilesTBLs.Where(x => x.pidDocument == IdDocument).OrderBy(x => x.sequence);
                LVfiles.DataBind();
                pnl_fileEdit.Visible = false;
            }
        }
        protected void LVfiles_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (e.CommandName == "move_up")
            {
                using (DCmodInvoice dc = new DCmodInvoice())
                {

                    dbInvCashDocumentFilesTBL currFile = dc.dbInvCashDocumentFilesTBLs.SingleOrDefault(x => x.uid == new Guid(lbl_id.Text));
                    var listAll = dc.dbInvCashDocumentFilesTBLs.Where(x => x.sequence < currFile.sequence && x.pidDocument == IdDocument).OrderByDescending(x => x.sequence);
                    if (listAll.Count() != 0)
                    {
                        var tmpFile = listAll.First();
                        currFile.sequence = tmpFile.sequence;
                        tmpFile.sequence = tmpFile.sequence + 1;
                        dc.SaveChanges();
                        LVfiles_DataBind();
                    }
                    else if (currFile.sequence > 1)
                    {
                        currFile.sequence = 1;
                        dc.SaveChanges();
                        LVfiles_DataBind();
                    }
                }
            }
            if (e.CommandName == "move_down")
            {
                using (DCmodInvoice dc = new DCmodInvoice())
                {

                    dbInvCashDocumentFilesTBL currFile = dc.dbInvCashDocumentFilesTBLs.SingleOrDefault(x => x.uid == new Guid(lbl_id.Text));
                    var listAll = dc.dbInvCashDocumentFilesTBLs.Where(x => x.sequence > currFile.sequence && x.pidDocument == IdDocument).OrderBy(x => x.sequence);
                    if (listAll.Count() != 0)
                    {
                        var tmpFile = listAll.First();
                        currFile.sequence = tmpFile.sequence;
                        tmpFile.sequence = tmpFile.sequence - 1;
                        dc.SaveChanges();
                        LVfiles_DataBind();
                    }
                }
            }
            if (e.CommandName == "elimina")
            {
                using (DCmodInvoice dc = new DCmodInvoice())
                {

                    dbInvCashDocumentFilesTBL currFile = dc.dbInvCashDocumentFilesTBLs.SingleOrDefault(x => x.uid == new Guid(lbl_id.Text));
                    if (currFile != null)
                    {
                        dc.Delete(currFile);
                        dc.SaveChanges();
                        LVfiles_DataBind();
                    }
                }
            }
            if (e.CommandName == "change")
            {
                HF_currFileUid.Value = lbl_id.Text;
                fillFile();
            }
        }
        protected void lnk_fileNew_Click(object sender, EventArgs e)
        {
            HF_currFileUid.Value = "";
            fillFile();
        }
        protected void lnk_fileSave_Click(object sender, EventArgs e)
        {
            saveFile();
            HF_currFileUid.Value = "";
            LVfiles_DataBind();
        }
        protected void lnk_fileCancel_Click(object sender, EventArgs e)
        {
            HF_currFileUid.Value = "";
            LVfiles_DataBind();
        }
        private void fillFile()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {

                dbInvCashDocumentFilesTBL currFile = null;
                if (HF_currFileUid.Value != "")
                {
                    currFile = dc.dbInvCashDocumentFilesTBLs.SingleOrDefault(x => x.uid == new Guid(HF_currFileUid.Value));
                }
                if (currFile == null)
                {
                    currFile = new dbInvCashDocumentFilesTBL();
                    currFile.uid = Guid.NewGuid();
                    HF_currFileUid.Value = currFile.uid.ToString();
                }
                txt_fileDocName.Text = currFile.docNum;
                fileDocPath.FileRoot = "files/invCashDocument/docFiles";
                fileDocPath.FileName = currFile.uid.ToString();
                fileDocPath.FilePath = currFile.docPath;
                pnl_fileEdit.Visible = true;
            }

        }

        protected void saveFile()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {

                dbInvCashDocumentFilesTBL currFile = null;
                if (HF_currFileUid.Value != "")
                {
                    currFile = dc.dbInvCashDocumentFilesTBLs.SingleOrDefault(x => x.uid == new Guid(HF_currFileUid.Value));
                }
                if (currFile == null)
                {
                    currFile = new dbInvCashDocumentFilesTBL();
                    currFile.uid = new Guid(HF_currFileUid.Value);
                    currFile.pidDocument = IdDocument;
                    HF_currFileUid.Value = currFile.uid.ToString();
                    int _sequence = 1;
                    List<dbInvCashDocumentFilesTBL> _allCollection = dc.dbInvCashDocumentFilesTBLs.Where(x => x.pidDocument == IdDocument).OrderBy(x => x.sequence).ToList();
                    if (_allCollection.Count != 0)
                        _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                    currFile.sequence = _sequence;
                    dc.Add(currFile);
                }
                currFile.docPath = fileDocPath.FilePath;
                currFile.docNum = txt_fileDocName.Text;
                dc.SaveChanges();
            }

        }

    }
}

