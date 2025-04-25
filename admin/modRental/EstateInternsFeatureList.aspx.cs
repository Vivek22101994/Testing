using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateInternsFeatureList : adminBasePage
    {
        protected dbRntEstateInternsFeatureTB currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                drp_isActive.DataBind();
                drp_pidInternsType_DataBind();
            }
        }

        private void drp_pidInternsType_DataBind()
        {
            drp_pidInternsType.Items.Insert(0, new ListItem(contUtils.getLabel("lblSelect", App.LangID, "Seleziona"), ""));
            drp_pidInternsType.Items.Insert(1, new ListItem("Bedroom"));
            drp_pidInternsType.Items.Insert(2, new ListItem(contUtils.getLabel("lblBathRooms", App.LangID, "Bathroom")));
            drp_pidInternsType.Items.Insert(3, new ListItem(contUtils.getLabel("lblKitchenType", App.LangID, "Kitchen")));
            drp_pidInternsType.Items.Insert(4, new ListItem("Livingroom"));
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
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntEstateInternsFeatureTBs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        int cnt = dc.dbRntEstateInternsFeatureRLs.Count(x => x.pidInternsFeature == lbl_id.Text.ToInt32());
                        if (cnt > 0)
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"" + contUtils.getLabel("lblDelInternsFeature", App.LangID, "Attenzione! Non è possibile eliminare questo caratteristiche perché è collegato all'appartamento") + ".\", 340, 110);", true);
                        else
                        {
                            var currLnList = dc.dbRntEstateInternsSubTypeLNs.Where(x => x.pidInternsSubType == lbl_id.Text.ToInt32()).ToList();
                            if (currLnList != null && currLnList.Count > 0)
                                dc.Delete(currLnList);
                            dc.Delete(currTBL);
                            //if (UserAuthentication.CurrRoleTBL.viewOnlyNoSave.objToInt32() == 0) {  }
                            dc.SaveChanges();
                        }
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
                currTBL = dc.dbRntEstateInternsFeatureTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null) currTBL = new dbRntEstateInternsFeatureTB();
                drp_isActive.setSelectedValue(currTBL.isActive);
                txt_code.Text = currTBL.code;
                drp_pidInternsType.setSelectedValue(currTBL.pidInternsType);
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntEstateInternsFeatureTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRntEstateInternsFeatureTB();
                    dc.Add(currTBL);
                }
                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.code = txt_code.Text;
                currTBL.pidInternsType = drp_pidInternsType.SelectedValue;
                //if (UserAuthentication.CurrRoleTBL.viewOnlyNoSave.objToInt32() == 0) { dc.SaveChanges(); }
                dc.SaveChanges();
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