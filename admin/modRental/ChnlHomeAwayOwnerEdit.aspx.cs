using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class ChnlHomeAwayOwnerEdit : adminBasePage
    {
        protected dbRntChnlHomeAwayOwnerTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {


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
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCchnlHomeAway dc = new DCchnlHomeAway())
                {
                    currTBL = dc.dbRntChnlHomeAwayOwnerTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt64());
                    if (currTBL != null)
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
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            rwdDett.VisibleOnPageLoad = false;
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        private void fillData()
        {
            using (DCchnlHomeAway dc = new DCchnlHomeAway())
            {
                currTBL = dc.dbRntChnlHomeAwayOwnerTBLs.SingleOrDefault(x => x.id == HfId.Value.ToInt64());

                if (currTBL == null) { currTBL = new dbRntChnlHomeAwayOwnerTBL(); currTBL.username = "Default";  }

                txt_username.Text = currTBL.username;
                txt_advertiserAssignedId.Text = currTBL.advertiserAssignedId;
                txt_ppb_advertiserAssignedId.Text = currTBL.ppb_advertiserAssignedId;
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCchnlHomeAway dc = new DCchnlHomeAway())
            {
                string errorString = "";
                //if (txt_id.Text.Trim() == "")
                //    errorString += "<br/>- Inserire OwnerId ";
                //else if (txt_id.Text != HfId.Value && dc.dbRntChnlHomeAwayOwnerTBLs.SingleOrDefault(x => x.id == txt_id.Text.ToInt64()) != null)
                //    errorString += "<br/>- OwnerId duplicato";
                if (errorString != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                    return;
                }
                currTBL = dc.dbRntChnlHomeAwayOwnerTBLs.SingleOrDefault(x => x.id == HfId.Value.ToInt64());
                if (currTBL == null)
                {
                    currTBL = new dbRntChnlHomeAwayOwnerTBL();
                    dc.Add(currTBL);
                }
                currTBL.username = txt_username.Text;
                currTBL.advertiserAssignedId = txt_advertiserAssignedId.Text;
                currTBL.ppb_advertiserAssignedId = txt_ppb_advertiserAssignedId.Text;
                dc.SaveChanges();
                HfId.Value = currTBL.id + "";
                rwdDett.VisibleOnPageLoad = true;

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