using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class invoiceList : adminBasePage
    {
        protected dbInvInvoiceTBL currTBL;
        private List<dbInvInvoiceTBL> tmpCurrList;
        private List<dbInvInvoiceTBL> CurrList
        {
            get
            {
                if (tmpCurrList == null)
                    if (ViewState["CurrList"] != null)
                    {
                        tmpCurrList =
                            PConv.DeserArrToList((object[])ViewState["CurrList"],
                                                 typeof(dbInvInvoiceTBL)).Cast<dbInvInvoiceTBL>().ToList();
                    }
                    else
                        tmpCurrList = new List<dbInvInvoiceTBL>();

                return tmpCurrList;
            }
            set
            {
                ViewState["CurrList"] = PConv.SerialList(value.Cast<object>().ToList());
                tmpCurrList = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                rdp_flt_cashDateFrom.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                closeDetails(false);
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodInvoice dc = new DCmodInvoice())
                {
                    currTBL = dc.dbInvInvoiceTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        List<dbInvInvoiceItemTBL> tmpList = dc.dbInvInvoiceItemTBLs.Where(x => x.pidInvoice == currTBL.id).ToList();
                        foreach (dbInvInvoiceItemTBL tmp in tmpList)
                            dc.Delete(tmp);
                        dc.SaveChanges();
                        List<dbInvCashDocumentTBL> tmpDocList = dc.dbInvCashDocumentTBLs.Where(x => x.extType == "invoice" && x.extUid == currTBL.uid).ToList();
                        foreach (dbInvCashDocumentTBL tmp in tmpDocList)
                            dc.Delete(tmp);
                        dc.SaveChanges();
                    }
                }
                closeDetails(false);
            }
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            setFilters();
            LV_DataBind();
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        protected void LV_DataBind()
        {
            LV.DataSource = CurrList.OrderBy(x => x.docNum).ToList();
            LV.DataBind();
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            LV_DataBind();
        }
        protected void setFilters()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {

                List<dbInvInvoiceTBL> list = dc.dbInvInvoiceTBLs.ToList();
                if (txt_flt_docNum.Text.Trim() != "")
                {
                    list = list.Where(x => x.docNum != null && x.docNum.ToLower().Contains(txt_flt_docNum.Text.ToLower().Trim())).ToList();
                }
                if (drp_flt_cashInOut.SelectedValue != "-1")
                {
                    list = list.Where(x => x.cashInOut == drp_flt_cashInOut.getSelectedValueInt()).ToList();
                }
                if (rdp_flt_cashDateFrom.SelectedDate.HasValue)
                {
                    list = list.Where(x => x.docIssueDate >= rdp_flt_cashDateFrom.SelectedDate).ToList();
                }
                if (rdp_flt_cashDateTo.SelectedDate.HasValue)
                {
                    list = list.Where(x => x.docIssueDate < rdp_flt_cashDateTo.SelectedDate).ToList();
                }
                if (drp_flt_ownerType.SelectedValue != "")
                {
                    list = list.Where(x => x.ownerType != null && x.ownerType.ToLower().Contains(drp_flt_ownerType.SelectedValue.ToLower().Trim())).ToList();
                }
                if (txt_flt_ownerNameFull.Text.Trim() != "")
                {
                    list = list.Where(x => x.ownerNameFull != null && x.ownerNameFull.ToLower().Contains(txt_flt_ownerNameFull.Text.ToLower().Trim())).ToList();
                }
                CurrList = list;
                setStats();
            }
        }
        protected void setStats()
        {
            int count_1 = CurrList.Where(x => x.cashInOut == 1).Count();
            decimal cashTotalAmount_1 = CurrList.Where(x => x.cashInOut == 1).Sum(x=>x.cashTotalAmount.objToDecimal());
            decimal cashTaxFree_1 = CurrList.Where(x => x.cashInOut == 1).Sum(x => x.cashTaxFree.objToDecimal());
            decimal cashTaxAmount_1 = CurrList.Where(x => x.cashInOut == 1).Sum(x => x.cashTaxAmount.objToDecimal());
            txt_count_1.Text = count_1.ToString();
            txt_cashTotalAmount_1.Text = cashTotalAmount_1.ToString("N2");
            txt_cashTaxFree_1.Text = cashTaxFree_1.ToString("N2");
            txt_cashTaxAmount_1.Text = cashTaxAmount_1.ToString("N2");

            int count_0 = CurrList.Where(x => x.cashInOut == 0).Count();
            decimal cashTotalAmount_0 = CurrList.Where(x => x.cashInOut == 0).Sum(x => x.cashTotalAmount.objToDecimal());
            decimal cashTaxFree_0 = CurrList.Where(x => x.cashInOut == 0).Sum(x => x.cashTaxFree.objToDecimal());
            decimal cashTaxAmount_0 = CurrList.Where(x => x.cashInOut == 0).Sum(x => x.cashTaxAmount.objToDecimal());
            txt_count_0.Text = count_0.ToString();
            txt_cashTotalAmount_0.Text = cashTotalAmount_0.ToString("N2");
            txt_cashTaxFree_0.Text = cashTaxFree_0.ToString("N2");
            txt_cashTaxAmount_0.Text = cashTaxAmount_0.ToString("N2");

            txt_count_diff.Text = (count_1 + count_0).ToString();
            txt_cashTotalAmount_diff.Text = (cashTotalAmount_1 - cashTotalAmount_0).ToString("N2");
            txt_cashTaxFree_diff.Text = (cashTaxFree_1 - cashTaxFree_0).ToString("N2");
            txt_cashTaxAmount_diff.Text = (cashTaxAmount_1 - cashTaxAmount_0).ToString("N2");
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            closeDetails(false);
        }
        protected void drp_flt_cashInOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            //drp_docCase_DataBind();
        }
        protected void drp_flt_ownerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //drp_owner_DataBind();
        }
    }

}