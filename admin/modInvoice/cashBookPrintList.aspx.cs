using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class cashBookPrintList : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                rdp_flt_cashDateFrom.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                chkList_flt_cashPlace_DataBind();
                chkList_flt_cashType_DataBind();
                chkList_flt_docCaseId_DataBind();
            }
        }
        protected void chkList_flt_cashPlace_DataBind()
        {
            chkList_flt_cashPlace.Items.Clear();
            foreach (dbInvCashPlaceLK CashPlace in invProps.CashPlaceLK.Where(x => x.isActive == 1).OrderBy(x => x.title))
                chkList_flt_cashPlace.Items.Add(new ListItem(CashPlace.title, CashPlace.id + ""));
        }
        protected void chkList_flt_cashType_DataBind()
        {
            chkList_flt_cashType.DataSource = invProps.CashTypeLK.Where(x => x.isActive == 1).OrderBy(x => x.title);
            chkList_flt_cashType.DataTextField = "title";
            chkList_flt_cashType.DataValueField = "id";
            chkList_flt_cashType.DataBind();
        }
        protected void chkList_flt_docCaseId_DataBind()
        {
            chkList_flt_docCaseId.DataSource = invProps.CashDocCaseLK.OrderBy(x => x.code);
            chkList_flt_docCaseId.DataTextField = "code";
            chkList_flt_docCaseId.DataValueField = "id";
            chkList_flt_docCaseId.DataBind();
        }
        protected void printList(bool Preview)
        {
            string src = "admin/modInvoice/cashBookPrintPreview.aspx?Preview=" + (Preview ? "1" : "0");
            if (drp_flt_cashInOut.SelectedValue != "-1")
            {
                src += "&cashInOut=" + drp_flt_cashInOut.SelectedValue;
            }
            if (rdp_flt_cashDateFrom.SelectedDate.HasValue)
            {
                src += "&cashDateFrom=" + rdp_flt_cashDateFrom.SelectedDate.JSCal_dateToString();
            }
            if (rdp_flt_cashDateTo.SelectedDate.HasValue)
            {
                src += "&cashDateTo=" + rdp_flt_cashDateTo.SelectedDate.JSCal_dateToString();
            }
            List<string> cashPlaceList = chkList_flt_cashPlace.getSelectedValueList();
            if (cashPlaceList.Count != 0)
                src += "&cashPlaceList=" + cashPlaceList.listToString("|");
            List<string> cashTypeList = chkList_flt_cashType.getSelectedValueList();
            if (cashTypeList.Count != 0)
                src += "&cashTypeList=" + cashTypeList.listToString("|");
            List<string> docCaseCodeIdList = chkList_flt_docCaseId.getSelectedValueList();
            if (docCaseCodeIdList.Count != 0)
                src += "&docCaseCodeIdList=" + docCaseCodeIdList.listToString("|");
            ltrSrc.Text = (Preview ? App.RP + src : App.RP + "admin/modPdf/createPdf.aspx?url=" + (App.HOST + App.RP + src).urlEncode() + "&filename=riepilogoprimanota.pdf");
            pnlPreview.Visible = true;
        }
        protected void lnkPreview_Click(object sender, EventArgs e)
        {
            printList(true);
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            printList(false);
        }
        protected void lnk_chkListSelectAll_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (sender as LinkButton);
            string arg = lnk.CommandArgument;
            if (arg.Contains("docCaseId"))
                foreach (ListItem item in chkList_flt_docCaseId.Items)
                    item.Selected = !arg.Contains("deselect");
            if (arg.Contains("cashPlace"))
                foreach (ListItem item in chkList_flt_cashPlace.Items)
                    item.Selected = !arg.Contains("deselect");
            if (arg.Contains("cashType"))
                foreach (ListItem item in chkList_flt_cashType.Items)
                    item.Selected = !arg.Contains("deselect");
        }

    }

}