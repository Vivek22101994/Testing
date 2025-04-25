using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class invoicePrintList : adminBasePage
    {
        protected dbInvInvoiceTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                chkList_ownerIds_DataBind();
                rdp_flt_cashDateFrom.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
        }
        protected void printList(bool Preview)
        {
            string src = "admin/modInvoice/invoicePrintPreview.aspx?Preview=" + (Preview ? "1" : "0");
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
            List<string> ownerIdsList = chkList_ownerIds.getSelectedValueList();
            if (ownerIdsList.Count != 0)
                src += "&ownerIdsList=" + ownerIdsList.listToString("|");
            ltrSrc.Text = (Preview ? App.RP + src : App.RP + "admin/modPdf/createPdf.aspx?url=" + (App.HOST + App.RP + src).urlEncode() + "&filename=riepilogofatture.pdf");
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
        protected void drp_flt_cashInOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            //drp_docCase_DataBind();
        }
        protected void drp_flt_ownerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkList_ownerIds_DataBind();
        }
        protected void chkList_ownerIds_DataBind()
        {
            chkList_ownerIds.DataSource = authProps.ClientTBL.Where(x => x.isActive == 1 && x.typeCode == "clienti").OrderBy(x => x.nameFull);
            chkList_ownerIds.DataTextField = "nameFull";
            chkList_ownerIds.DataValueField = "id";
            chkList_ownerIds.DataBind();
        }

        protected void lnk_chkList_ownerIdsSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in chkList_ownerIds.Items)
                item.Selected = (sender as LinkButton).CommandArgument == "select";
        }
    }

}