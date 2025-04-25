using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class agentMonthlyReportList : adminBasePage
    {
        protected dbRntAgentMonthlyReportTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                var tmp = rntProps.AgentTBL.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt64());
                if (tmp == null)
                {
                    pnlFascia.Visible = false;
                    Response.Redirect("agentList.aspx");
                    return;
                }
                HfId.Value = tmp.id.ToString();
                HF_title.Value = tmp.nameCompany + " (" + tmp.nameFull + ")";
                rntUtils.checkAgentMonthlyReport(tmp.id);
                closeDetails(false);
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            closeDetails(false);
        }
    }
}