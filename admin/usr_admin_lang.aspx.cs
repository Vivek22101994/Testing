using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_admin_lang : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected magaUser_DataContext DC_USER;
        protected USR_ADMIN _currTBL;
        public int IdAdmin
        {
            get
            {
                return HF_IdAdmin.Value.ToInt32();
            }
            set
            {
                HF_IdAdmin.Value = value.ToString();
                FillControls();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                int id = Request.QueryString["id"].ToInt32();
                _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == id && x.is_active == 1 && x.is_deleted != 1);
                if (_currTBL != null)
                {
                    IdAdmin = _currTBL.id;
                    ltr_apartment.Text = _currTBL.name + " " + _currTBL.surname;
                }
                else
                {
                    Response.Redirect("usr_admin_role.aspx");
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
                drp_lang.DataBind();
                drp_lang.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setAutocomplete", "setAutocomplete();", true);
        }

        protected void FillControls()
        {
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            if (_currTBL == null) return;
            txt_media_folder.Text = _currTBL.media_folder;
            checkFolder();
        }

        protected void FillDataFromControls()
        {
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            if (_currTBL == null) return;
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
            _currTBL = DC_USER.USR_ADMIN.FirstOrDefault(x => x.id != IdAdmin && x.media_folder == _folder);
            if (_currTBL != null)
            {
                pnl_folderAssignedToAnotherEstate.Visible = true;
                pnl_folderSave.Visible = true;
                HF_assignedEstateId.Value = _currTBL.id.ToString();
                HF_assignedEstateCode.Value = _currTBL.name + " " + _currTBL.surname;
                return;
            }
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            if (_currTBL == null) return;
            string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_main_folder.Value + "\\" + _folder);
            if (!Directory.Exists(_path))
            {
                pnl_folderNotExist.Visible = true;
                return;
            }
            _currTBL.media_folder = _folder;
            DC_USER.SubmitChanges();
            checkFolder();
        }
        protected void lnk_cancelFolder_Click(object sender, EventArgs e)
        {
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            if (_currTBL == null) return;
            txt_media_folder.Text = _currTBL.media_folder;
            checkFolder();
        }
        protected void lnk_changeFolder_Click(object sender, EventArgs e)
        {
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            if (_currTBL == null) return;
            _currTBL.media_folder = "";
            DC_USER.USR_RL_ADMIN_LANG.DeleteAllOnSubmit(DC_USER.USR_RL_ADMIN_LANG.Where(x => x.pid_admin == IdAdmin));
            DC_USER.SubmitChanges();

            Response.Redirect("usr_admin_lang.aspx?id=" + IdAdmin, true);
        }

        protected void lnk_createFolderOK_Click(object sender, EventArgs e)
        {
            string _folder = txt_media_folder.Text;
            string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_main_folder.Value + "\\" + _folder);
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            if (_currTBL == null) return;
            _currTBL.media_folder = _folder;
            DC_USER.SubmitChanges();
            checkFolder();
        }

        protected void lnk_createFolderNO_Click(object sender, EventArgs e)
        {
            txt_media_folder.Text = "";
            checkFolder();
        }
        protected void checkFolder()
        {
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            if (_currTBL == null) return;
            if (_currTBL.media_folder != null && _currTBL.media_folder.Trim() != "")
            {
                string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_main_folder.Value + "\\" + _currTBL.media_folder.Trim());
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
            UC_img_thumbGalleryItem.ImgRoot = HF_main_folder.Value + "/" + _currTBL.media_folder;
        }
        protected void FillGalleryItem()
        {
            USR_RL_ADMIN_LANG _lang = DC_USER.USR_RL_ADMIN_LANG.SingleOrDefault(x => x.pid_lang == drp_lang.getSelectedValueInt(0) && x.pid_admin == IdAdmin);
            if (_lang == null)
            {
                _lang = new USR_RL_ADMIN_LANG();
            }
            drp_lang.setSelectedValue(_lang.pid_lang.ToString());
            UC_img_thumbGalleryItem.ImgPath = _lang.img_thumb;
            pnl_editGalleryItem.Visible = true;
        }
        protected void SaveGalleryItem()
        {
            if (drp_lang.getSelectedValueInt(0) == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    "alertError",
                                                    "alert('selezionare una lingua');", true);
                return;
            }
            USR_RL_ADMIN_LANG _lang = DC_USER.USR_RL_ADMIN_LANG.SingleOrDefault(x => x.pid_lang == drp_lang.getSelectedValueInt(0) && x.pid_admin == IdAdmin);
            if (_lang == null)
            {
                int _sequence = 1;
                List<USR_RL_ADMIN_LANG> _allCollection =
                    DC_USER.USR_RL_ADMIN_LANG.Where(x => x.pid_admin == IdAdmin).OrderBy(x => x.sequence).ToList();
                if (_allCollection.Count != 0)
                {
                    _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                }
                _lang = new USR_RL_ADMIN_LANG();
                _lang.sequence = _sequence;
                _lang.pid_admin = IdAdmin;
                DC_USER.USR_RL_ADMIN_LANG.InsertOnSubmit(_lang);
            }
            _lang.pid_lang = drp_lang.getSelectedValueInt(0).Value;
            _lang.img_thumb = UC_img_thumbGalleryItem.ImgPath;
            DC_USER.SubmitChanges();
            pnl_editGalleryItem.Visible = false;
            LV_gallery.DataBind();
        }
        protected void LV_gallery_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_pid_lang = e.Item.FindControl("lbl_pid_lang") as Label;
            if (e.CommandName == "elimina")
            {
                var m = DC_USER.USR_RL_ADMIN_LANG.SingleOrDefault(x => x.pid_admin == IdAdmin && x.pid_lang == lbl_pid_lang.Text.ToInt32());
                if (m == null) return;
                DC_USER.USR_RL_ADMIN_LANG.DeleteOnSubmit(m);
                DC_USER.SubmitChanges();
                LV_gallery.DataBind();
            }
            if (e.CommandName == "seleziona")
            {
                drp_lang.setSelectedValue(lbl_pid_lang.Text);
                FillGalleryItem();
            }
            if (e.CommandName == "move_up")
            {
                var m = DC_USER.USR_RL_ADMIN_LANG.SingleOrDefault(x => x.pid_admin == IdAdmin && x.pid_lang == lbl_pid_lang.Text.ToInt32());
                var upper_block_list = DC_USER.USR_RL_ADMIN_LANG.Where(x => x.sequence < m.sequence && x.pid_admin == IdAdmin).OrderByDescending(x => x.sequence);
                if (upper_block_list.Count() != 0)
                {
                    var upper_block = upper_block_list.First();
                    m.sequence = upper_block.sequence;
                    upper_block.sequence = upper_block.sequence + 1;
                    DC_USER.SubmitChanges();
                    LDS_gallery.DataBind();
                    LV_gallery.DataBind();
                }
                else if (m.sequence > 1)
                {
                    m.sequence = 1;
                    DC_USER.SubmitChanges();
                    LDS_gallery.DataBind();
                    LV_gallery.DataBind();
                }
            }
            if (e.CommandName == "move_down")
            {
                var m = DC_USER.USR_RL_ADMIN_LANG.SingleOrDefault(x => x.pid_admin == IdAdmin && x.pid_lang == lbl_pid_lang.Text.ToInt32());
                var down_block_list = DC_USER.USR_RL_ADMIN_LANG.Where(x => x.sequence > m.sequence && x.pid_admin == IdAdmin).OrderBy(x => x.sequence);
                if (down_block_list.Count() != 0)
                {
                    var down_block = down_block_list.First();
                    m.sequence = down_block.sequence;
                    down_block.sequence = down_block.sequence - 1;
                    DC_USER.SubmitChanges();
                    LDS_gallery.DataBind();
                    LV_gallery.DataBind();
                }
            }
        }
        protected void lnk_newGalleryItem_Click(object sender, EventArgs e)
        {
            drp_lang.setSelectedValue("0");
            FillGalleryItem();
        }
        protected void lnk_saveGalleryItem_Click(object sender, EventArgs e)
        {
            SaveGalleryItem();
        }
        protected void lnk_cancelGalleryItem_Click(object sender, EventArgs e)
        {
            drp_lang.setSelectedValue("0");
            pnl_editGalleryItem.Visible = false;
        }
        protected void lnk_bindGalleryItems_Click(object sender, EventArgs e)
        {
            return;
            //_currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            //if (_currTBL == null) return;
            //DC_USER.USR_RL_ADMIN_LANG.DeleteAllOnSubmit(DC_USER.USR_RL_ADMIN_LANG.Where(x => x.pid_estate == IdAdmin && x.type == "gallery"));
            //DC_USER.SubmitChanges();
            //if (_currTBL.media_folder != null && _currTBL.media_folder.Trim() != "")
            //{
            //    string _root = HF_main_folder.Value + "/" + _currTBL.media_folder.Trim();
            //    string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, _root);
            //    if (!Directory.Exists(_path))
            //        Directory.CreateDirectory(_path);
            //    _path = Path.Combine(_path, "gallery");
            //    string _folder = _path;
            //    _root += "/gallery";
            //    if (!Directory.Exists(_path))
            //        Directory.CreateDirectory(_path);
            //    _path = Path.Combine(_path, "thumb");
            //    if (!Directory.Exists(_path))
            //        Directory.CreateDirectory(_path);
            //    txt_media_folder.ReadOnly = true;
            //    lnk_saveFolder.Visible = false;
            //    lnk_cancelFolder.Visible = false;
            //    lnk_changeFolder.Visible = true;
            //    string[] _files = Directory.GetFiles(_folder);
            //    List<string> _data = new List<string>();
            //    _data.AddRange(_files.ToList());
            //    int i = 1;
            //    foreach (string _f in _data)
            //    {
            //        FileInfo fi = new FileInfo(_f);
            //        USR_RL_ADMIN_LANG _media = new USR_RL_ADMIN_LANG();
            //        _media.sequence = i;
            //        _media.pid_estate = IdAdmin;
            //        _media.code = fi.Name;
            //        _media.type = "gallery";
            //        _media.img_banner = _root + "/" + fi.Name;
            //        _media.img_thumb = _root + "/thumb/" + fi.Name;
            //        DC_USER.USR_RL_ADMIN_LANG.InsertOnSubmit(_media);
            //        i++;
            //    }
            //    DC_USER.SubmitChanges();
            //    LV_gallery.DataBind();
            //}
        }
    }
}