using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class CompanyList : adminBasePage
    {
        protected dbInvCompanyTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
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
                    currTBL = dc.dbInvCompanyTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null && currTBL.id > 1)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                    }
                }
                closeDetails(false);
            }
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            setfilters();
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
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
            if (drp_flt_isActive.SelectedValue != "-1")
            {
                _filter += _sep + "isActive = " + drp_flt_isActive.SelectedValue + "";
                _sep = " and ";
            }
            if (txt_flt_nameFull.Text.Trim() != "")
            {
                _filter += _sep + "nameFull.Contains(\"" + txt_flt_nameFull.Text.Trim() + "\")";
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