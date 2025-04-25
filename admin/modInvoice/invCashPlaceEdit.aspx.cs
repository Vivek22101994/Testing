using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class invCashPlaceEdit : adminBasePage
    {
        protected dbInvCashPlaceLK currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drp_invTaxId_DataBind(ref drp_chargeFeeTaxId);
                BindLV();
            }
        }
        protected void drp_invTaxId_DataBind(ref DropDownList drp)
        {
            drp.DataSource = invProps.CashTaxLK.OrderBy(x => x.id).ToList();
            drp.DataTextField = "code";
            drp.DataValueField = "id";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("Default", ""));
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "dett")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                HfId.Value = lbl_id.Text;
                fillData();
            }
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodInvoice dc = new DCmodInvoice())
                {
                    currTBL = dc.dbInvCashPlaceLKs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                        invProps.CashPlaceLK = dc.dbInvCashPlaceLKs.ToList();
                    }
                }
                closeDetails(false);
            }
        }
        protected void BindLV()
        {
            LV.DataSource = invProps.CashPlaceLK.OrderBy(x => x.title);
            LV.DataBind();
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            BindLV();
            rwdDett.VisibleOnPageLoad = false;
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        private void fillData()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                currTBL = dc.dbInvCashPlaceLKs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbInvCashPlaceLK();
                }
                txt_code.Text = currTBL.code;
                txt_title.Text = currTBL.title;
                txt_chargeFeeInvoiceDesc.Text = currTBL.chargeFeeInvoiceDesc;
                drp_type.setSelectedValue(currTBL.type);
                drp_isActive.setSelectedValue(currTBL.isActive);
                ntxt_chargeFeePart1.Value = currTBL.chargeFeePart1.objToDouble();
                ntxt_chargeFeePart2.Value = currTBL.chargeFeePart2.objToDouble();
                ntxt_chargeFeeFull.Value = currTBL.chargeFeeFull.objToDouble();
                drp_chargeFeeInvoice.setSelectedValue(currTBL.chargeFeeInvoice);
                drp_chargeFeeTaxId.setSelectedValue(currTBL.chargeFeeTaxId);
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodInvoice dcEcom = new DCmodInvoice())
            {
                currTBL = dcEcom.dbInvCashPlaceLKs.SingleOrDefault(x => x.id != HfId.Value.ToInt32() && x.code == txt_code.Text.Trim().ToLower());
                if (currTBL != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione Codice duplicato.\", 340, 110);", true);
                    return;
                }
                currTBL = dcEcom.dbInvCashPlaceLKs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbInvCashPlaceLK();
                    dcEcom.Add(currTBL);
                }
                currTBL.code = txt_code.Text.Trim().ToLower();
                currTBL.title = txt_title.Text;
                currTBL.chargeFeeInvoiceDesc = txt_chargeFeeInvoiceDesc.Text;
                currTBL.type = drp_type.SelectedValue;
                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.chargeFeePart1 = ntxt_chargeFeePart1.Value.objToInt32();
                currTBL.chargeFeePart2 = ntxt_chargeFeePart2.Value.objToInt32();
                currTBL.chargeFeeFull = ntxt_chargeFeeFull.Value.objToInt32();
                currTBL.chargeFeeInvoice = drp_chargeFeeInvoice.getSelectedValueInt(0);
                currTBL.chargeFeeTaxId = drp_chargeFeeTaxId.getSelectedValueInt(0);
                dcEcom.SaveChanges();
                invProps.CashPlaceLK = dcEcom.dbInvCashPlaceLKs.ToList();
                closeDetails(true);
            }
        }
        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "0";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }
}