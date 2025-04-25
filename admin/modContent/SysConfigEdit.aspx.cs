using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModContent.admin.modContent
{
    public partial class SysConfigEdit : adminBasePage
    {
        protected dbContSysConfigTB currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
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
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            rwdDett.VisibleOnPageLoad = false;
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        private void fillData()
        {
            using (DCmodContent dc = new DCmodContent())
            {
                currTBL = dc.dbContSysConfigTBs.SingleOrDefault(x => x.name == HfId.Value);
                if (currTBL == null)
                {
                    currTBL = new dbContSysConfigTB();
                    txt_name.ReadOnly = false;
                    txt_name.Text = "inserire nome";
                    txt_value.Text = "";
                }
                else
                {
                    txt_name.ReadOnly = true;
                    txt_name.Text = currTBL.name;
                    txt_value.Text = currTBL.value;
                }
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodContent dc = new DCmodContent())
            {
                if (HfId.Value == "")
                {
                    if (txt_name.Text.Trim().Length < 5)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"La lunghezza del nome deve essere almeno 5 caratteri.\", 340, 110);", true);
                        return;
                    }
                    var tmpTbl = dc.dbContSysConfigTBs.SingleOrDefault(x => x.name == txt_name.Text.Trim());
                    if (tmpTbl!=null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Nome dupplicato.\", 340, 110);", true);
                        return;
                    }
                    currTBL = new dbContSysConfigTB();
                    currTBL.name = txt_name.Text.Trim();
                    currTBL.value = txt_value.Text;
                    dc.Add(currTBL);
                    dc.SaveChanges();
                    return;
                }
                currTBL = dc.dbContSysConfigTBs.SingleOrDefault(x => x.name == HfId.Value);
                if (currTBL == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Errore imprevisto.\", 340, 110);", true);
                    return;
                }
                currTBL.value = txt_value.Text;
                dc.SaveChanges();
            }
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            using (DCmodContent dc = new DCmodContent())
                AppSettings.DEF_SYS_SETTINGs = dc.dbContSysConfigTBs.ToList();
            closeDetails(true);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }

}