using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_estate_media : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            bool canEdit = UserAuthentication.hasPermission("rnt_estate", "can_edit");
            lnk_closeVideoItem.Visible = canEdit;
            lnk_cancelVideoItem.Visible = canEdit;
            lnk_saveVideoItem.Visible = canEdit;
            lnk_changeVideoItem.Visible = canEdit;
            lnk_newVideoItem.Visible = canEdit;
            lnk_cancelGalleryItem.Visible = canEdit;
            lnk_saveGalleryItem.Visible = canEdit;
            lnk_bindGalleryItems.Visible = canEdit;
            lnk_newGalleryItem.Visible = canEdit;
            lnk_changeFolder.Visible = canEdit;
            lnk_save.Visible = canEdit;
            lnk_annulla.Visible = canEdit;
            lnk_changeFolder.Visible = canEdit;
            lnk_cancelFolder.Visible = canEdit;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {

                Response.Redirect("modRental/EstateMedia.aspx?id=" + Request.QueryString["id"].ToInt32());
                return;
                int id = Request.QueryString["id"].ToInt32();
                RNT_VIEW_ESTATE _est = DC_RENTAL.RNT_VIEW_ESTATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
                if (_est != null)
                {
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                    UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                    if (_est.pid_city == 2)
                        HF_main_folder.Value = "florenceapartmentsphoto";
                    if (_est.pid_city == 3)
                        HF_main_folder.Value = "veniceapartmentsphoto";
                    IdEstate = _est.id;
                }
                else
                {
                    Response.Redirect("rnt_estate_list.aspx");
                }
                string _items = "";
                string _sep = "";
                string[] _folders = Directory.GetDirectories(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_main_folder.Value));
                List<string> _data = new List<string>();
                _data.AddRange(_folders.ToList());
                foreach (string _f in _data)
                {
                    FileInfo fi = new FileInfo(_f);
                    _items += _sep + "{label: \"" + fi.Name + "\"}";
                    _sep = ",";
                }
                ltr_folderItems.Text = _items;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setAutocomplete", "setAutocomplete();", true);
        }
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
                FillControls();
            }
        }


        protected void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null)return;
            txt_media_folder.Text = _currTBL.media_folder;
            UC_img_banner.ImgPath = _currTBL.img_banner;
            UC_img_preview_1.ImgPath = _currTBL.img_preview_1;
            UC_img_preview_2.ImgPath = _currTBL.img_preview_2;
            UC_img_preview_3.ImgPath = _currTBL.img_preview_3;
            UC_img_thumb.ImgPath = _currTBL.img_thumb;
            txt_mobileVideoFilePath.Text = _currTBL.mobileVideoFilePath;
            checkFolder();
        }

        protected void FillDataFromControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            _currTBL.img_banner = UC_img_banner.ImgPath;
            _currTBL.img_preview_1 = UC_img_preview_1.ImgPath;
            _currTBL.img_preview_2 = UC_img_preview_2.ImgPath;
            _currTBL.img_preview_3 = UC_img_preview_3.ImgPath;
            _currTBL.img_thumb = UC_img_thumb.ImgPath;
            _currTBL.mobileVideoFilePath = txt_mobileVideoFilePath.Text;
            DC_RENTAL.SubmitChanges();
            Response.Redirect("rnt_estate_list.aspx");
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
            pnl_folderAssignedToAnotherEstate.Visible = false;
            pnl_folderNotExist.Visible = false;
            pnl_folderSave.Visible = false;
            lblFolderError.Visible = false;
            string _folder = txt_media_folder.Text.clearPathName();
            txt_media_folder.Text = _folder;
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.FirstOrDefault(x => x.id != IdEstate && x.media_folder == _folder);
            if (_currTBL != null)
            {
                pnl_folderAssignedToAnotherEstate.Visible = true;
                pnl_folderSave.Visible = true;
                HF_assignedEstateId.Value = _currTBL.id.ToString();
                HF_assignedEstateCode.Value = _currTBL.code;
                return;
            }
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_main_folder.Value + "\\" + _folder);
            if (!Directory.Exists(_path))
            {
                pnl_folderNotExist.Visible = true;
                return;
            }
            _currTBL.media_folder = _folder;
            DC_RENTAL.SubmitChanges();
            checkFolder();
        }
        protected void lnk_cancelFolder_Click(object sender, EventArgs e)
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            txt_media_folder.Text = _currTBL.media_folder;
            checkFolder();
        }
        protected void lnk_changeFolder_Click(object sender, EventArgs e)
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            _currTBL.media_folder = "";
            _currTBL.img_banner = "";
            _currTBL.img_preview_1 = "";
            _currTBL.img_preview_2 = "";
            _currTBL.img_preview_3 = "";
            _currTBL.img_thumb = "";
            DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteAllOnSubmit(DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == "gallery"));
            DC_RENTAL.SubmitChanges();

            Response.Redirect("rnt_estate_media.aspx?id=" + IdEstate, true);
        }

        protected void lnk_createFolderOK_Click(object sender, EventArgs e)
        {
            string _folder = txt_media_folder.Text;
            string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_main_folder.Value + "\\" + _folder);
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            _path = Path.Combine(_path, "gallery");
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            _path = Path.Combine(_path, "thumb");
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            _currTBL.media_folder = _folder;
            DC_RENTAL.SubmitChanges();
            checkFolder();
        }

        protected void lnk_createFolderNO_Click(object sender, EventArgs e)
        {
            txt_media_folder.Text = "";
            checkFolder();
        }
        protected void checkFolder()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            if (_currTBL.media_folder != null && _currTBL.media_folder.Trim() != "")
            {
                string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_main_folder.Value + "\\" + _currTBL.media_folder.Trim());
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                _path = Path.Combine(_path, "gallery");
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                _path = Path.Combine(_path, "thumb");
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
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
            pnl_folderAssignedToAnotherEstate.Visible = false;
            pnl_folderNotExist.Visible = false;
            pnl_folderSave.Visible = true;
            pnlContent.Visible = _currTBL.media_folder != null && _currTBL.media_folder.Trim() != "";
            UC_img_banner.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder + "/gallery";
            UC_img_preview_1.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder + "/gallery/thumb";
            UC_img_preview_2.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder + "/gallery/thumb";
            UC_img_preview_3.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder + "/gallery/thumb";
            UC_img_thumb.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder;
            UC_img_bannerGalleryItem.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder + "/gallery";
            UC_img_thumbGalleryItem.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder + "/gallery/thumb";
        }
        protected void FillGalleryItem()
        {
            RNT_RL_ESTATE_MEDIA _media = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == HF_IdGalleryItem.Value.ToInt32());
            if(_media==null)
            {
                _media = new RNT_RL_ESTATE_MEDIA();
                HF_IdGalleryItem.Value = "0";
            }
            txt_codeGalleryItem.Text = _media.code;
            UC_img_thumbGalleryItem.ImgPath = _media.img_thumb;
            UC_img_bannerGalleryItem.ImgPath = _media.img_banner;
            pnl_editGalleryItem.Visible = true;
        }
        protected void SaveGalleryItem()
        {
            RNT_RL_ESTATE_MEDIA _media = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == HF_IdGalleryItem.Value.ToInt32());
            if (_media == null)
            {
                int _sequence = 1;
                List<RNT_RL_ESTATE_MEDIA> _allCollection =
                    DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == "gallery" && x.sequence.HasValue).OrderBy(x => x.sequence).ToList();
                if (_allCollection.Count != 0)
                {
                    _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                }
                _media = new RNT_RL_ESTATE_MEDIA();
                _media.sequence = _sequence;
                DC_RENTAL.RNT_RL_ESTATE_MEDIAs.InsertOnSubmit(_media);
            }
            _media.pid_estate = IdEstate;
            _media.pid_estate = IdEstate;
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
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (e.CommandName == "elimina")
            {
                var m = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (m == null) return;
                DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteOnSubmit(m);
                DC_RENTAL.SubmitChanges();
                LV_gallery.DataBind();
            }
            if (e.CommandName == "seleziona")
            {
                HF_IdGalleryItem.Value = lbl_id.Text;
                FillGalleryItem();
            }
            if (e.CommandName == "move_up")
            {
                var block = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                var upper_block_list = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.sequence < block.sequence && x.pid_estate == IdEstate).OrderByDescending(x => x.sequence);
                if (upper_block_list.Count() != 0)
                {
                    var upper_block = upper_block_list.First();
                    block.sequence = upper_block.sequence;
                    upper_block.sequence = upper_block.sequence + 1;
                    DC_RENTAL.SubmitChanges();
                    LDS_gallery.DataBind();
                    LV_gallery.DataBind();
                }
                else if (block.sequence > 1)
                {
                    block.sequence = 1;
                    DC_RENTAL.SubmitChanges();
                    LDS_gallery.DataBind();
                    LV_gallery.DataBind();
                }
            }
            if (e.CommandName == "move_down")
            {
                var block = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                var down_block_list = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.sequence > block.sequence && x.pid_estate == IdEstate).OrderBy(x => x.sequence);
                if (down_block_list.Count() != 0)
                {
                    var down_block = down_block_list.First();
                    block.sequence = down_block.sequence;
                    down_block.sequence = down_block.sequence - 1;
                    DC_RENTAL.SubmitChanges();
                    LDS_gallery.DataBind();
                    LV_gallery.DataBind();
                }
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
        protected void lnk_bindGalleryItems_Click(object sender, EventArgs e)
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteAllOnSubmit(DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == "gallery"));
            DC_RENTAL.SubmitChanges();
            if (_currTBL.media_folder != null && _currTBL.media_folder.Trim() != "")
            {
                string _root = HF_main_folder.Value + "/" + _currTBL.media_folder.Trim();
                string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, _root);
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                _path = Path.Combine(_path, "gallery");
                string _folder = _path;
                _root += "/gallery";
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                _path = Path.Combine(_path, "thumb");
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                txt_media_folder.ReadOnly = true;
                lnk_saveFolder.Visible = false;
                lnk_cancelFolder.Visible = false;
                lnk_changeFolder.Visible = true;
                string[] _files = Directory.GetFiles(_folder);
                List<string> _data = new List<string>();
                _data.AddRange(_files.ToList());
                int i = 1;
                foreach (string _f in _data)
                {
                    FileInfo fi = new FileInfo(_f);
                    RNT_RL_ESTATE_MEDIA _media = new RNT_RL_ESTATE_MEDIA();
                    _media.sequence = i;
                    _media.pid_estate = IdEstate;
                    _media.code = fi.Name;
                    _media.type = "gallery";
                    _media.img_banner = _root + "/" + fi.Name;
                    _media.img_thumb = _root + "/thumb/" + fi.Name;
                    DC_RENTAL.RNT_RL_ESTATE_MEDIAs.InsertOnSubmit(_media);
                    i++;
                }
                DC_RENTAL.SubmitChanges();
                LV_gallery.DataBind();
            }
        }
        
        protected void FillVideoItem()
        {
            RNT_RL_ESTATE_MEDIA _media = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == HF_IdVideoItem.Value.ToInt32());
            if (_media == null)
            {
                _media = new RNT_RL_ESTATE_MEDIA();
                HF_IdVideoItem.Value = "0";
                EnableVideoControls();
            }
            else
            {
                DisableVideoControls();
            }
            txt_video_embed.Text = _media.video_embed;
            ltr_video_embed.Text = _media.video_embed;
            pnl_editVideoItem.Visible = true;
        }
        protected void SaveVideoItem()
        {
            RNT_RL_ESTATE_MEDIA _media = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == HF_IdVideoItem.Value.ToInt32());
            if (_media == null)
            {
                _media = new RNT_RL_ESTATE_MEDIA();
                DC_RENTAL.RNT_RL_ESTATE_MEDIAs.InsertOnSubmit(_media);
            }
            _media.pid_estate = IdEstate;
            _media.type = "video";
            _media.video_embed = txt_video_embed.Text;
            _media.video_path = "";
            ltr_video_embed.Text = _media.video_embed;
            DC_RENTAL.SubmitChanges();
            DisableVideoControls();
            LV_video.DataBind();
        }
        protected void LV_video_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl = e.Item.FindControl("lbl_id") as Label;
            if (e.CommandName == "elimina")
            {
                var m = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == lbl.Text.ToInt32());
                if (m == null) return;
                DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteOnSubmit(m);
                DC_RENTAL.SubmitChanges();
                LV_video.DataBind();
            }
            else if (e.CommandName == "seleziona")
            {
                HF_IdVideoItem.Value = lbl.Text;
                FillVideoItem();
            }
        }
        protected void lnk_newVideoItem_Click(object sender, EventArgs e)
        {
            HF_IdVideoItem.Value = "0";
            FillVideoItem();
        }
        protected void lnk_saveVideoItem_Click(object sender, EventArgs e)
        {
            SaveVideoItem();
        }
        protected void lnk_changeVideoItem_Click(object sender, EventArgs e)
        {
            EnableVideoControls();
        }
        protected void lnk_cancelVideoItem_Click(object sender, EventArgs e)
        {
            FillVideoItem();
        }
        protected void lnk_closeVideoItem_Click(object sender, EventArgs e)
        {
            HF_IdVideoItem.Value = "0";
            pnl_editVideoItem.Visible = false;
        }
        protected void DisableVideoControls()
        {
            ltr_video_embed.Visible = true;
            txt_video_embed.Visible = false;
            lnk_saveVideoItem.Visible = false;
            lnk_cancelVideoItem.Visible = false;
            lnk_changeVideoItem.Visible = true;
        }
        protected void EnableVideoControls()
        {
            ltr_video_embed.Visible = false;
            txt_video_embed.Visible = true;
            lnk_saveVideoItem.Visible = true;
            lnk_cancelVideoItem.Visible = true;
            lnk_changeVideoItem.Visible = false;
        }
    }
}