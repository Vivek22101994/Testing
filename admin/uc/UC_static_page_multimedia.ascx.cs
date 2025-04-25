using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_static_page_multimedia : System.Web.UI.UserControl
    {
        private magaContent_DataContext DC_CONTENT;
        public string StaticPageID
        {
            get
            {
                return HF_id_statpage.Value;
            }
            set
            {
                HF_id_statpage.Value = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack)
            {
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "public/images/multimedia")))
                {
                    Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "public/images/multimedia"));
                }
            }

        }
        protected void lnk_new_2_Click(object sender, EventArgs e)
        {
            HF_type.Value = "2";
            HF_id_multimedia.Value = "0";
            FillControls();
        }
        private void FillControls()
        {
            CONT_RL_PAGE_MULTIMEDIA m = new CONT_RL_PAGE_MULTIMEDIA();
            if (HF_id_multimedia.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id_multimedia.Value, out id))
                {
                    m = DC_CONTENT.CONT_RL_PAGE_MULTIMEDIAs.SingleOrDefault(item => item.id == id);
                }
            }
            txt_titolo.Text = m.title;
            img_preview.ImageUrl = CurrentAppSettings.ROOT_PATH + m.img_thumb;
            HF_img_preview.Value = m.img_thumb;
            img_banner.ImageUrl = CurrentAppSettings.ROOT_PATH + m.path;
            HF_img_banner.Value = m.path;
            pnl_edit.Visible = true;
        }
        private void FillDataFromControls()
        {
            CONT_RL_PAGE_MULTIMEDIA m = new CONT_RL_PAGE_MULTIMEDIA();
            if (HF_id_multimedia.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id_multimedia.Value, out id))
                {
                    m = DC_CONTENT.CONT_RL_PAGE_MULTIMEDIAs.SingleOrDefault(item => item.id == id);
                }
            }
            else
            {
                DC_CONTENT.CONT_RL_PAGE_MULTIMEDIAs.InsertOnSubmit(m);
            }
            m.pid_page = Convert.ToInt32(HF_id_statpage.Value);
            m.title = txt_titolo.Text;
            m.type = 2;
            m.path = HF_img_banner.Value;
            m.img_thumb = HF_img_preview.Value;
            DC_CONTENT.SubmitChanges();
            pnl_edit.Visible = false;
            LV_video.DataBind();
        }

        protected void LV_video_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl = e.Item.FindControl("lbl_id") as Label;
            if (e.CommandName == "elimina")
            {
                var m = DC_CONTENT.CONT_RL_PAGE_MULTIMEDIAs.SingleOrDefault(x => x.id == Convert.ToInt32(lbl.Text));
                if (m == null) return;
                DC_CONTENT.CONT_RL_PAGE_MULTIMEDIAs.DeleteOnSubmit(m);
                DC_CONTENT.SubmitChanges();
                LV_video.DataBind();
            }
            else if (e.CommandName == "seleziona")
            {
                HF_id_multimedia.Value = lbl.Text;
                FillControls();
            }
        }
        protected void lnk_upload_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            HF_id_multimedia.Value = "0";
            pnl_edit.Visible = false;
        }
        protected void lnk_load_preview_Click(object sender, EventArgs e)
        {
            if (FU_img_preview.HasFile)
            {
                int i = 1;
                while (File.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "public/images/multimedia/preview_" + i + "_" + FU_img_preview.FileName)))
                {
                    i++;
                }
                HF_img_preview.Value = "public/images/multimedia/preview_" + i + "_" + FU_img_preview.FileName;
                FU_img_preview.SaveAs(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_img_preview.Value));
                img_preview.ImageUrl = CurrentAppSettings.ROOT_PATH + HF_img_preview.Value;
            }
        }
        protected void lnk_delete_preview_Click(object sender, EventArgs e)
        {
            HF_img_preview.Value = "";
            img_preview.ImageUrl = "";
        }
        protected void lnk_load_banner_Click(object sender, EventArgs e)
        {
            if (FU_img_banner.HasFile)
            {
                int i = 1;
                while (File.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "public/images/multimedia/banner_" + i + "_" + FU_img_banner.FileName)))
                {
                    i++;
                }
                HF_img_banner.Value = "public/images/multimedia/banner_" + i + "_" + FU_img_banner.FileName;
                FU_img_banner.SaveAs(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_img_banner.Value));
                img_banner.ImageUrl = CurrentAppSettings.ROOT_PATH + HF_img_banner.Value;
            }
        }
        protected void lnk_delete_banner_Click(object sender, EventArgs e)
        {
            HF_img_banner.Value = "";
            img_banner.ImageUrl = "";
        }
    }
}