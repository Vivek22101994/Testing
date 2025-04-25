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
    public partial class invCashDocumentList : adminBasePage
    {
        protected dbInvCashDocumentTBL currTBL;
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
        protected string getFirstCashBook(long id)
        {
            string date = "";
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                dbInvCashBookTBL currCashBook = dc.dbInvCashBookTBLs.Where(x => x.pidDocument == id).OrderBy(x=>x.cashDate).FirstOrDefault();
                if (currCashBook != null)
                {
                    return currCashBook.cashDate.formatCustom("#dd# #MM# #yy#", 1, "");
                }
            }
            return date;
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodInvoice dc = new DCmodInvoice())
                {
                    currTBL = dc.dbInvCashDocumentTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt64());
                    if (currTBL != null)
                    {
                        if (currTBL.extUid.HasValue)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Documento legato ad una fattura non puo essere eliminato.\", 340, 110);", true);
                            return;
                        }
                        dc.Delete(currTBL);
                        List<dbInvCashBookTBL> cashBookList = dc.dbInvCashBookTBLs.Where(x => x.pidDocument == currTBL.id).ToList();
                        foreach(dbInvCashBookTBL cashBook in cashBookList)
                            dc.Delete(cashBook);
                        dc.SaveChanges();
                    }
                }
                closeDetails(false);
            }
        }
        protected void LV_DataBind()
        {
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            LV_DataBind();
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            setfilters();
            LV_DataBind();
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        protected void setfilters()
        {
            string _filter = "";
            string _sep = "";
            if (txt_flt_code.Text.Trim() != "")
            {
                _filter += _sep + "code.Contains(\"" + txt_flt_code.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (drp_flt_cashInOut.SelectedValue != "-1")
            {
                _filter += _sep + "cashInOut = " + drp_flt_cashInOut.SelectedValue + "";
                _sep = " and ";
            }
            if (rdp_flt_cashDateFrom.SelectedDate.HasValue)
            {
                _filter += _sep + "docIssueDate >= DateTime.Parse(\"" + rdp_flt_cashDateFrom.SelectedDate + "\")";
                _sep = " and ";
            }
            if (rdp_flt_cashDateTo.SelectedDate.HasValue)
            {
                _filter += _sep + "docIssueDate < DateTime.Parse(\"" + rdp_flt_cashDateTo.SelectedDate + "\")";
                _sep = " and ";
            }
            if (txt_flt_docNum.Text.Trim() != "")
            {
                _filter += _sep + "docNum.Contains(\"" + txt_flt_docNum.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (drp_flt_ownerType.SelectedValue != "")
            {
                _filter += _sep + "ownerType.Contains(\"" + drp_flt_ownerType.SelectedValue + "\")";
                _sep = " and ";
            }
            if (txt_flt_ownerNameFull.Text.Trim() != "")
            {
                _filter += _sep + "ownerNameFull.Contains(\"" + txt_flt_ownerNameFull.Text.Trim() + "\")";
                _sep = " and ";
            }
            _filter += _sep + "1 = 1";
            ltrLDSfiltter.Text = _filter;
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