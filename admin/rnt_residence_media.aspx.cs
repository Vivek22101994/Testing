using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_residence_media : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_RESIDENCE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                int id = Request.QueryString["id"].ToInt32();
                RNT_TB_RESIDENCE _est = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == id);
                if (_est != null)
                {
                    IdResidence = _est.id;
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                    UC_rnt_residence_navlinks1.IdResidence = IdResidence;
                }
                else
                {
                    Response.Redirect("rnt_residence_list.aspx");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Shadowbox_init", "Shadowbox_init();", true);
            }
        }
        public int IdResidence
        {
            get
            {
                return HF_IdResidence.Value.ToInt32();
            }
            set
            {
                HF_IdResidence.Value = value.ToString();
                FillControls();
            }
        }


        protected void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == IdResidence);
            if (_currTBL == null) return;
            txt_media_folder.Text = _currTBL.media_folder;
            UC_img_banner.ImgPath = _currTBL.img_banner;
            UC_img_preview_1.ImgPath = _currTBL.img_preview_1;
            UC_img_preview_2.ImgPath = _currTBL.img_preview_2;
            UC_img_preview_3.ImgPath = _currTBL.img_preview_3;
            UC_img_thumb.ImgPath = _currTBL.img_thumb;
            checkFolder();
            FillVideoItem();
        }

        protected void FillDataFromControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == IdResidence);
            if (_currTBL == null) return;
            _currTBL.img_banner = UC_img_banner.ImgPath;
            _currTBL.img_preview_1 = UC_img_preview_1.ImgPath;
            _currTBL.img_preview_2 = UC_img_preview_2.ImgPath;
            _currTBL.img_preview_3 = UC_img_preview_3.ImgPath;
            _currTBL.img_thumb = UC_img_thumb.ImgPath;
            DC_RENTAL.SubmitChanges();
            Response.Redirect("rnt_residence_list.aspx");
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
        }
        protected void lnk_saveFolder_Click(object sender, EventArgs e)
        {
            _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == IdResidence);
            if (_currTBL == null) return;
            _currTBL.media_folder = txt_media_folder.Text;
            DC_RENTAL.SubmitChanges();
            checkFolder();
        }
        protected void lnk_cancelFolder_Click(object sender, EventArgs e)
        {
            _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == IdResidence);
            if (_currTBL == null) return;
            txt_media_folder.Text = _currTBL.media_folder;
            checkFolder();
        }
        protected void lnk_changeFolder_Click(object sender, EventArgs e)
        {
            txt_media_folder.ReadOnly = false;
            lnk_saveFolder.Visible = true;
            lnk_cancelFolder.Visible = true;
            lnk_changeFolder.Visible = false;
        }

        protected void checkFolder()
        {
            _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == IdResidence);
            if (_currTBL == null) return;
            if (_currTBL.media_folder != null && _currTBL.media_folder.Trim() != "")
            {
                txt_media_folder.ReadOnly = true;
                lnk_saveFolder.Visible = false;
                lnk_cancelFolder.Visible = false;
                lnk_changeFolder.Visible = true;
            }
            else
            {
                txt_media_folder.ReadOnly = false;
                lnk_saveFolder.Visible = true;
                lnk_cancelFolder.Visible = true;
                lnk_changeFolder.Visible = false;
            }
            pnlContent.Visible = _currTBL.media_folder != null && _currTBL.media_folder.Trim() != "";
            UC_img_banner.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder;
            UC_img_preview_1.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder;
            UC_img_preview_2.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder;
            UC_img_preview_3.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder;
            UC_img_thumb.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder;
            UC_img_bannerGalleryItem.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder + "/gallery";
            UC_img_thumbGalleryItem.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder + "/gallery/thumb";
        }
        protected void FillGalleryItem()
        {
            RNT_RL_RESIDENCE_MEDIA _media = DC_RENTAL.RNT_RL_RESIDENCE_MEDIAs.SingleOrDefault(x => x.id == HF_IdGalleryItem.Value.ToInt32());
            if (_media == null)
            {
                _media = new RNT_RL_RESIDENCE_MEDIA();
                HF_IdGalleryItem.Value = "0";
            }
            txt_codeGalleryItem.Text = _media.code;
            UC_img_thumbGalleryItem.ImgPath = _media.img_thumb;
            UC_img_bannerGalleryItem.ImgPath = _media.img_banner;
            pnl_editGalleryItem.Visible = true;
        }
        protected void SaveGalleryItem()
        {
            RNT_RL_RESIDENCE_MEDIA _media = DC_RENTAL.RNT_RL_RESIDENCE_MEDIAs.SingleOrDefault(x => x.id == HF_IdGalleryItem.Value.ToInt32());
            if (_media == null)
            {
                _media = new RNT_RL_RESIDENCE_MEDIA();
                DC_RENTAL.RNT_RL_RESIDENCE_MEDIAs.InsertOnSubmit(_media);
            }
            _media.pid_residence = IdResidence;
            _media.code = txt_codeGalleryItem.Text;
            _media.type = "gallery";
            _media.img_banner = UC_img_bannerGalleryItem.ImgPath;
            _media.img_thumb = UC_img_thumbGalleryItem.ImgPath;
            DC_RENTAL.SubmitChanges();
            pnl_editGalleryItem.Visible = false;
            LV_gallery.DataBind();
        }
        protected void LV_gallery_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl = e.Item.FindControl("lbl_id") as Label;
            if (e.CommandName == "elimina")
            {
                var m = DC_RENTAL.RNT_RL_RESIDENCE_MEDIAs.SingleOrDefault(x => x.id == lbl.Text.ToInt32());
                if (m == null) return;
                DC_RENTAL.RNT_RL_RESIDENCE_MEDIAs.DeleteOnSubmit(m);
                DC_RENTAL.SubmitChanges();
                LV_gallery.DataBind();
            }
            else if (e.CommandName == "seleziona")
            {
                HF_IdGalleryItem.Value = lbl.Text;
                FillGalleryItem();
            }
        }
        protected void lnk_newGalleryItem_Click(object sender, EventArgs e)
        {
            HF_IdGalleryItem.Value = "0";
            FillGalleryItem();
        }
        protected void lnk_saveGalleryItem_Click(object sender, EventArgs e)
        {
            SaveGalleryItem();
        }
        protected void lnk_cancelGalleryItem_Click(object sender, EventArgs e)
        {
            HF_IdGalleryItem.Value = "0";
            pnl_editGalleryItem.Visible = false;
        }
        protected void FillVideoItem()
        {
            RNT_RL_RESIDENCE_MEDIA _media = DC_RENTAL.RNT_RL_RESIDENCE_MEDIAs.SingleOrDefault(x => x.pid_residence == IdResidence && x.type == "video");
            if (_media == null)
            {
                _media = new RNT_RL_RESIDENCE_MEDIA();
            }
            //txt_codeGalleryItem.Text = _media.code;
            txt_video_embed.Text = _media.video_embed;
            ltr_video_embed.Text = _media.video_embed;
            //UC_img_thumbGalleryItem.ImgPath = _media.img_thumb;
            //UC_img_bannerGalleryItem.ImgPath = _media.img_banner;
        }
        protected void SaveVideoItem()
        {
            RNT_RL_RESIDENCE_MEDIA _media = DC_RENTAL.RNT_RL_RESIDENCE_MEDIAs.SingleOrDefault(x => x.pid_residence == IdResidence && x.type == "video");
            if (_media == null)
            {
                _media = new RNT_RL_RESIDENCE_MEDIA();
                DC_RENTAL.RNT_RL_RESIDENCE_MEDIAs.InsertOnSubmit(_media);
            }
            _media.pid_residence = IdResidence;
            //_media.code = txt_codeGalleryItem.Text;
            _media.type = "video";
            _media.video_embed = txt_video_embed.Text;
            _media.video_path = "";
            //_media.img_banner = UC_img_bannerGalleryItem.ImgPath;
            //_media.img_thumb = UC_img_thumbGalleryItem.ImgPath;
            DC_RENTAL.SubmitChanges();
        }
        protected void lnk_change_video_Click(object sender, EventArgs e)
        {
            txt_video_embed.Text = ltr_video_embed.Text;
            EnableVideoControls();
        }
        protected void lnk_save_video_Click(object sender, EventArgs e)
        {
            SaveVideoItem();
            ltr_video_embed.Text = txt_video_embed.Text;
            DisableVideoControls();
        }
        protected void lnk_cancel_video_Click(object sender, EventArgs e)
        {
            txt_video_embed.Text = ltr_video_embed.Text;
            DisableVideoControls();
        }
        protected void DisableVideoControls()
        {
            ltr_video_embed.Visible = true;
            txt_video_embed.Visible = false;
            lnk_save_video.Visible = false;
            lnk_cancel_video.Visible = false;
            lnk_change_video.Visible = true;
        }
        protected void EnableVideoControls()
        {
            ltr_video_embed.Visible = false;
            txt_video_embed.Visible = true;
            lnk_save_video.Visible = true;
            lnk_cancel_video.Visible = true;
            lnk_change_video.Visible = false;
        }
    }
}