using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class invCashDocCaseEdit : adminBasePage
    {
        protected dbInvCashDocCaseLK currTBL;

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
                    currTBL = dc.dbInvCashDocCaseLKs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                        invProps.CashDocCaseLK = dc.dbInvCashDocCaseLKs.ToList();
                    }
                }
                closeDetails(false);
            }
        }
        protected void BindLV()
        {
            LV.DataSource = invProps.CashDocCaseLK.OrderBy(x => x.code);
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
                currTBL = dc.dbInvCashDocCaseLKs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbInvCashDocCaseLK();
                }
                txt_code.Text = currTBL.code;
                drp_viewIn.setSelectedValue(currTBL.viewIn);
                drp_viewOut.setSelectedValue(currTBL.viewOut);
                re_description.Content = currTBL.description;
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodInvoice dcEcom = new DCmodInvoice())
            {
                currTBL = dcEcom.dbInvCashDocCaseLKs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbInvCashDocCaseLK();
                    dcEcom.Add(currTBL);
                }
                currTBL.code = txt_code.Text;
                currTBL.viewIn = drp_viewIn.getSelectedValueInt(0);
                currTBL.viewOut = drp_viewOut.getSelectedValueInt(0);
                currTBL.description = re_description.Content;
                dcEcom.SaveChanges();
                invProps.CashDocCaseLK = dcEcom.dbInvCashDocCaseLKs.ToList();
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