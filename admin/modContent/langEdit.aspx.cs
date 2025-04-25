using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using RentalInRome.data;

namespace ModContent.admin.modContent
{
    public partial class langEdit : adminBasePage
    {
        protected CONT_TBL_LANG currTBL;
        private magaContent_DataContext DC_CONTENT;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_CONTENT = maga_DataContext.DC_CONTENT;

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
            currTBL = DC_CONTENT.CONT_TBL_LANGs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
            if (currTBL == null) currTBL = new CONT_TBL_LANG();
            drp_isActive.setSelectedValue(currTBL.is_active);
            drp_isPublic.setSelectedValue(currTBL.is_public);
            txt_title.Text = currTBL.title;
            txt_langTitle.Text = currTBL.lang_title;
            txt_commonName.Text = currTBL.common_name;
            txt_abbr.Text = currTBL.abbr;
            txt_jsCalFile.Text = currTBL.js_cal_file;
            txt_imgThumb.Text = currTBL.img_thumb;
            txt_imgPreview.Text = currTBL.img_preview;
            rwdDett.VisibleOnPageLoad = true;
        }
        private void saveData()
        {
            currTBL = DC_CONTENT.CONT_TBL_LANGs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
            if (currTBL == null)
            {
                currTBL = new CONT_TBL_LANG();
                DC_CONTENT.CONT_TBL_LANGs.InsertOnSubmit(currTBL);
            }
            currTBL.is_active = drp_isActive.getSelectedValueInt(0);
            currTBL.is_public = drp_isPublic.getSelectedValueInt(0);
            currTBL.title = txt_title.Text;
            currTBL.lang_title = txt_langTitle.Text;
            currTBL.common_name = txt_commonName.Text;
            currTBL.abbr = txt_abbr.Text;
            currTBL.js_cal_file = txt_jsCalFile.Text;
            currTBL.img_thumb = txt_imgThumb.Text;
            currTBL.img_preview = txt_imgPreview.Text;
            rwdDett.VisibleOnPageLoad = true;

            DC_CONTENT.SubmitChanges();
            contProps.LangTBL = DC_CONTENT.CONT_TBL_LANGs.ToList();
            AppUtils.FillUrlList();
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