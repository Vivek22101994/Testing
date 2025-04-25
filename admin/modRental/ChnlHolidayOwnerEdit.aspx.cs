using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class ChnlHolidayOwnerEdit : adminBasePage
    {
        protected dbRntChnlHolidayOwnerTBL currTBL;
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
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == lbl_id.Text.ToInt64());
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
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == HfId.Value.ToInt64());

                if (currTBL == null) { currTBL = new dbRntChnlHolidayOwnerTBL(); txt_ownerId.ReadOnly = false; pnl_update.Visible = false; currTBL.username = ""; currTBL.password = ""; }
                else { txt_ownerId.ReadOnly = true; pnl_update.Visible = true; rdp_updateDate.SelectedDate = DateTime.Now.AddDays(-1); }

                txt_ownerId.Text = currTBL.ownerId + "";
                drp_isActive.setSelectedValue(currTBL.isActive);
                txt_title.Text = currTBL.title;
                txt_username.Text = currTBL.username;
                txt_password.Text = currTBL.password;
                txt_ownerKey.Text = currTBL.ownerKey;
                
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                string errorString = "";
                if (txt_ownerId.Text.Trim()=="" )
                    errorString += "<br/>- Inserire OwnerId ";
                else if (txt_ownerId.Text != HfId.Value && dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == txt_ownerId.Text.ToInt64()) != null)
                    errorString += "<br/>- OwnerId duplicato";
                if (errorString != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                    return;
                }
                currTBL = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == HfId.Value.ToInt64());
                if (currTBL == null)
                {
                    currTBL = new dbRntChnlHolidayOwnerTBL();
                    dc.Add(currTBL);
                }
                currTBL.ownerId = txt_ownerId.Text.Trim().ToInt64();
                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.title = txt_title.Text;
                currTBL.username = txt_username.Text;
                currTBL.password = txt_password.Text;
                currTBL.ownerKey = txt_ownerKey.Text;

                dc.SaveChanges();
                HfId.Value = txt_ownerId.Text.Trim();
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
        protected void lnk_update_Click(object sender, EventArgs e)
        {
            if (!rdp_updateDate.SelectedDate.HasValue)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Seleziona una data\", 340, 110);", true);
                return;
            }
            //BcomImport.BcomImport_start(txt_username.Text, txt_password.Text, txt_ownerId.Text, rdp_updateDate.SelectedDate.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----"));
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Operazione in corso...\", 340, 110);", true);
        }
    }

}