using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModAuth.admin.modAuth
{
    public partial class docTypeEdit : adminBasePage
    {
        protected dbAuthDocTypeLK currTBL;
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
                pnlDett.Visible = true;
            }
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodAuth dc = new DCmodAuth())
                {
                    currTBL = dc.dbAuthDocTypeLKs.SingleOrDefault(x => x.code == lbl_id.Text);
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                        authProps.DocTypeLK = dc.dbAuthDocTypeLKs.ToList();
                    }
                }
                closeDetails(false);
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
            using (DCmodAuth dc = new DCmodAuth())
            {
                currTBL = dc.dbAuthDocTypeLKs.SingleOrDefault(x => x.code == HfId.Value);
                if (currTBL == null) currTBL = new dbAuthDocTypeLK();

                txt_code.Text = currTBL.code;
                txt_title.Text = currTBL.title;

                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                currTBL = dc.dbAuthDocTypeLKs.SingleOrDefault(x => x.code == HfId.Value);
                if (currTBL == null)
                {
                    currTBL = new dbAuthDocTypeLK();
                    dc.Add(currTBL);
                }
                currTBL.code = txt_code.Text;
                currTBL.title = txt_title.Text;
                dc.SaveChanges();
                authProps.DocTypeLK = dc.dbAuthDocTypeLKs.ToList();
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
            closeDetails(true);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }

}