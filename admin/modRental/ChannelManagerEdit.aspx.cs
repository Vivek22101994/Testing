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
    public partial class ChannelManagerEdit : adminBasePage
    {
        protected dbRntChannelManagerTBL currTBL;
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
                    currTBL = dc.dbRntChannelManagerTBLs.SingleOrDefault(x => x.code == lbl_id.Text);
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
            string _folder = "images/ChannelManager";
            if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
            using (DCmodRental dc = new DCmodRental())
            {
                drp_pidAgent.DataSource = dc.dbRntAgentTBLs.OrderBy(x => x.nameCompany).ToList();
                drp_pidAgent.DataTextField = "nameCompany";
                drp_pidAgent.DataValueField = "id";
                drp_pidAgent.DataBind();
                drp_pidAgent.Items.Insert(0, new ListItem("- - -",""));
                currTBL = dc.dbRntChannelManagerTBLs.SingleOrDefault(x => x.code == HfId.Value);

                if (currTBL == null) { currTBL = new dbRntChannelManagerTBL(); txt_code.ReadOnly = false; }
                else { txt_code.ReadOnly = true; }

                drp_isActive.setSelectedValue(currTBL.isActive);
                txt_title.Text = currTBL.title;
                txt_code.Text = currTBL.code;
                drp_pidAgent.setSelectedValue(currTBL.pidAgent + "");
                imgLogo.ImgPathDef = "";
                imgLogo.ImgRoot = _folder;
                imgLogo.ImgPath = currTBL.imgLogo;

                docPath.FileRoot = _folder;
                docPath.FilePath = currTBL.filePdf;
                
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntChannelManagerTBLs.SingleOrDefault(x => x.code == HfId.Value);
                if (currTBL == null)
                {
                    currTBL = new dbRntChannelManagerTBL();
                    dc.Add(currTBL);
                }
                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.title = txt_title.Text;
                currTBL.code = txt_code.Text;
                currTBL.pidAgent = drp_pidAgent.SelectedValue.ToInt64();
                currTBL.imgLogo = imgLogo.ImgPath;
                currTBL.filePdf = docPath.FilePath;

                dc.SaveChanges();

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