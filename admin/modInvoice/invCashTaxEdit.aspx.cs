using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class invCashTaxEdit : adminBasePage
    {
        protected dbInvCashTaxLK currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLV();
            }
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
                    currTBL = dc.dbInvCashTaxLKs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                        invProps.CashTaxLK = dc.dbInvCashTaxLKs.ToList();
                    }
                }
                closeDetails(false);
            }
        }
        protected void BindLV()
        {
            LV.DataSource = invProps.CashTaxLK.OrderByDescending(x => x.taxAmount);
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
                currTBL = dc.dbInvCashTaxLKs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbInvCashTaxLK();
                }
                txt_code.Text = currTBL.code;
                ntxt_taxAmount.Value = decimal.Multiply(currTBL.taxAmount.objToDecimal(), 100).objToDouble();
                //drp_possibleVatFreeInvoice.setSelectedValue(currTBL.isPossibleVatFreeInvoice.objToInt32());
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodInvoice dcEcom = new DCmodInvoice())
            {
                currTBL = dcEcom.dbInvCashTaxLKs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbInvCashTaxLK();
                    dcEcom.Add(currTBL);
                }
                currTBL.code = txt_code.Text;
                currTBL.taxAmount = decimal.Divide(ntxt_taxAmount.Value.objToDecimal(), 100);
                //currTBL.isPossibleVatFreeInvoice = drp_possibleVatFreeInvoice.getSelectedValueInt();
                dcEcom.SaveChanges();
                invProps.CashTaxLK = dcEcom.dbInvCashTaxLKs.ToList();
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