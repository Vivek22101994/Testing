using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;
using System.IO;

namespace ModRental.admin.modRental
{
    public partial class EstateDett_mediaOriginal : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected class TmpClassMediaType
        {
            public string type { get; set; }
            public string currTitle { get; set; }
            public string lnkTitle { get; set; }
            public TmpClassMediaType(string Type, string CurrTitle, string LnkTitle)
            {
                type = Type;
                currTitle = CurrTitle;
                lnkTitle = LnkTitle;
            }
        }
        private List<TmpClassMediaType> tmpMediaTypes;
        protected List<TmpClassMediaType> MediaTypes
        {
            get
            {
                if (tmpMediaTypes == null)
                {
                    List<TmpClassMediaType> tmp = new List<TmpClassMediaType>();
                    tmp.Add(new TmpClassMediaType("gallery", "Gallery del Appartamento", "Carica le foto del Appartamento"));
                    tmp.Add(new TmpClassMediaType("video", "Video Gallery del Appartamento", "Carica Video del Appartamento"));
                    tmp.Add(new TmpClassMediaType("floorplans", "Gallery delle Piantine", "Carica le foto delle Piantine"));
                    tmp.Add(new TmpClassMediaType("homeaway", "Gallery HomeAway", "Carica le foto HomeAway"));
                    tmpMediaTypes = tmp;
                }
                return tmpMediaTypes;
            }
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE currTBL;
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
                UC_rnt_estate_navlinks1.IdEstate = value;
            }
        }
        public string CurrType
        {
            get
            {
                return "original";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            using (DCmodRental dc = new DCmodRental())
            {
                if (!IsPostBack)
                {
                    IdEstate = Request.QueryString["id"].ToInt32();
                    currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currTBL == null)
                    {
                        Response.Redirect("/admin/rnt_estate_list.aspx");
                        return;
                    }
                    LV_otherType.DataSource = MediaTypes;
                    LV_otherType.DataBind();
                    foreach (var item in LV_otherType.Items)
                    {
                        Label lblType = item.FindControl("lblType") as Label;
                        HyperLink HL = item.FindControl("HL") as HyperLink;
                        var currType = MediaTypes.SingleOrDefault(x => x.type == lblType.Text);
                        if (currType != null)
                        {
                            HL.Text = currType.lnkTitle;
                            HL.NavigateUrl = "EstateMedia.aspx?id=" + IdEstate + "&type=" + currType.type;
                        }
                    }

                    ltr_apartment.Text = currTBL.code + " / " + "rif. " + currTBL.id;
                    UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                    HF_main_folder.Value = "originalphotos";
                    string _path = Path.Combine(App.SRP, HF_main_folder.Value);
                    if (!Directory.Exists(_path))
                        Directory.CreateDirectory(_path);


                    if (string.IsNullOrEmpty(currTBL.mediaFolderOriginalPhotos) || !Directory.Exists(Path.Combine(App.SRP, HF_main_folder.Value + "/" + currTBL.mediaFolderOriginalPhotos)))
                    {
                        changeFolder();
                    }
                    else
                    {
                        pnlFolderEdit.Visible = false;
                        pnlFolderView.Visible = true;
                        LV_gallery_DataBind();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setSortable", "setSortable();", true);
                }
            }
        }
        protected void lnk_saveFolder_Click(object sender, EventArgs e)
        {
            if (drp_folderList.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Seleziona la cartella\", 340, 110);", true);
                return;
            }
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            currTBL.mediaFolderOriginalPhotos = drp_folderList.SelectedValue;
            DC_RENTAL.SubmitChanges();
            pnlFolderEdit.Visible = false;
            pnlFolderView.Visible = true;
            LV_gallery_DataBind();
        }
        protected void lnk_cancelFolder_Click(object sender, EventArgs e)
        {
            pnlFolderEdit.Visible = false;
            pnlFolderView.Visible = true;
            LV_gallery_DataBind();
        }
        protected void changeFolder()
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            drp_folderList.Items.Clear();
            drp_folderList.Items.Add("");
            var folderList = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.id != IdEstate && x.mediaFolderOriginalPhotos != null && x.mediaFolderOriginalPhotos != "").Select(x => x.mediaFolderOriginalPhotos).ToList();
            string[] _folders = Directory.GetDirectories(Path.Combine(App.SRP, HF_main_folder.Value));
            foreach (string _folder in _folders)
            {
                DirectoryInfo finfo = new DirectoryInfo(_folder);
                if (folderList.Contains(finfo.Name)) continue;
                drp_folderList.Items.Add(new ListItem(finfo.Name, finfo.Name));
            }
            drp_folderList.setSelectedValue(currTBL.mediaFolderOriginalPhotos);
            pnlFolderEdit.Visible = true;
            pnlFolderView.Visible = false;
            LV_gallery.Visible = false;
            LvFolders.Visible = false;
        }
        protected void lnk_changeFolder_Click(object sender, EventArgs e)
        {
            changeFolder();
        }
        protected void pnlDett_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "sortable") saveSequence();
            LV_gallery_DataBind();
        }
        protected void LV_gallery_DataBind()
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            if (string.IsNullOrEmpty(currTBL.mediaFolderOriginalPhotos) || !Directory.Exists(Path.Combine(App.SRP, HF_main_folder.Value + "/" + currTBL.mediaFolderOriginalPhotos)))
            {
                changeFolder();
                return;
            }
            txt_mediaFolderOriginalPhotos.Text = currTBL.mediaFolderOriginalPhotos;
            LV_gallery.Visible = true;
            LvFolders.Visible = true;
            var allPhotos = new List<RNT_RL_ESTATE_MEDIA>();
            var folders = new List<RNT_RL_ESTATE_MEDIA>();
            folders.Add(new RNT_RL_ESTATE_MEDIA() { code = "- - -", type = "", img_banner = "" });
            string[] _files = Directory.GetFiles(Path.Combine(App.SRP, HF_main_folder.Value + "/" + currTBL.mediaFolderOriginalPhotos));
            int i = 1;
            foreach (string _f in _files)
            {
                FileInfo fi = new FileInfo(_f);
                RNT_RL_ESTATE_MEDIA _media = new RNT_RL_ESTATE_MEDIA();
                _media.sequence = i;
                _media.pid_estate = IdEstate;
                _media.code = fi.Name;
                _media.type = "";
                _media.img_banner = HF_main_folder.Value + "/" + currTBL.mediaFolderOriginalPhotos + "/" + fi.Name;
                _media.img_thumb = _media.img_banner;
                allPhotos.Add(_media);
                i++;
            }
            string[] _folders = Directory.GetDirectories(Path.Combine(App.SRP, HF_main_folder.Value + "/" + currTBL.mediaFolderOriginalPhotos));
            foreach (string _folder in _folders)
            {
                DirectoryInfo finfo = new DirectoryInfo(_folder);
                _files = Directory.GetFiles(Path.Combine(App.SRP, HF_main_folder.Value + "/" + currTBL.mediaFolderOriginalPhotos + "/" + finfo.Name));
                i = 1;
                foreach (string _f in _files)
                {
                    FileInfo fi = new FileInfo(_f);
                    RNT_RL_ESTATE_MEDIA _media = new RNT_RL_ESTATE_MEDIA();
                    _media.sequence = i;
                    _media.pid_estate = IdEstate;
                    _media.code = fi.Name;
                    _media.type = finfo.Name;
                    _media.img_banner = HF_main_folder.Value + "/" + currTBL.mediaFolderOriginalPhotos + "/" + finfo.Name + "/" + fi.Name;
                    _media.img_thumb = _media.img_banner;
                    allPhotos.Add(_media);
                    i++;
                }
                folders.Add(new RNT_RL_ESTATE_MEDIA() { code = finfo.Name, type = finfo.Name, img_banner = finfo.Name });
            }
            var currList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CurrType + "").OrderBy(x => x.sequence).ToList();
            var currPaths = currList.Count > 0 ? currList.Select(x => x.img_banner).ToList() : new List<string>();
            if (currPaths.Count > 0)
                allPhotos = allPhotos.Where(x => !currPaths.Contains(x.img_banner)).OrderBy(x => x.sequence).ToList();
            LV_gallery.DataSource = currList;
            LV_gallery.DataBind();
            LvFolders.DataSource = folders;
            LvFolders.DataBind();
            foreach (ListViewDataItem item in LvFolders.Items)
            {
                var lbl_type = item.FindControl("lbl_type") as Label;
                var LvGallery = item.FindControl("LvGallery") as ListView;
                LvGallery.DataSource = allPhotos.Where(x => x.type == lbl_type.Text);
                LvGallery.DataBind();
            }
        }
        protected void lnkRemoveAll_Click(object sender, EventArgs e)
        {
            var tmpList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CurrType).ToList();
            DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteAllOnSubmit(tmpList);
                        DC_RENTAL.SubmitChanges();
            LV_gallery_DataBind();
        }
        protected void LvFolders_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            if (e.CommandName == "SelectAll")
            {
                var LvGallery = e.Item.FindControl("LvGallery") as ListView;
                foreach (ListViewDataItem item in LvGallery.Items)
                {
                    var lbl_id = item.FindControl("lbl_id") as Label;
                    var lbl_path = item.FindControl("lbl_path") as Label;
                    int _sequence = 1;
                    List<RNT_RL_ESTATE_MEDIA> _allCollection =
                        DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CurrType + "").OrderBy(x => x.sequence).ToList();
                    if (_allCollection.Count != 0)
                    {
                        _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                    }
                    RNT_RL_ESTATE_MEDIA currImg = new RNT_RL_ESTATE_MEDIA();
                    currImg.sequence = _sequence;
                    currImg.pid_estate = IdEstate;
                    currImg.code = "";
                    currImg.type = CurrType + "";
                    currImg.img_banner = lbl_path.Text;
                    currImg.img_thumb = lbl_path.Text;
                    DC_RENTAL.RNT_RL_ESTATE_MEDIAs.InsertOnSubmit(currImg);
                                DC_RENTAL.SubmitChanges();
                }
                LV_gallery_DataBind();
            }
        }
        protected void LV_gallery_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            if (e.CommandName == "additem")
            {
                var lbl_id = e.Item.FindControl("lbl_id") as Label;
                var lbl_path = e.Item.FindControl("lbl_path") as Label;
                int _sequence = 1;
                List<RNT_RL_ESTATE_MEDIA> _allCollection =
                    DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CurrType + "").OrderBy(x => x.sequence).ToList();
                if (_allCollection.Count != 0)
                {
                    _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                }
                RNT_RL_ESTATE_MEDIA currImg = new RNT_RL_ESTATE_MEDIA();
                currImg.sequence = _sequence;
                currImg.pid_estate = IdEstate;
                currImg.code = "";
                currImg.type = CurrType + "";
                currImg.img_banner = lbl_path.Text;
                currImg.img_thumb = lbl_path.Text;
                DC_RENTAL.RNT_RL_ESTATE_MEDIAs.InsertOnSubmit(currImg);
                            DC_RENTAL.SubmitChanges();
                LV_gallery_DataBind();
            }
            if (e.CommandName == "deleteitem")
            {
                var lbl_id = e.Item.FindControl("lbl_id") as Label;
                var tmp = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (tmp != null)
                {
                    DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteOnSubmit(tmp);
                                DC_RENTAL.SubmitChanges();
                }
                LV_gallery_DataBind();
            }
        }
        protected void saveSequence()
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            List<string> _list = HF_order.Value.splitStringToList("|");
            for (int i = 0; i < _list.Count; i++)
            {
                RNT_RL_ESTATE_MEDIA currImg = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == _list[i].ToInt32());
                if (currImg != null)
                {
                    currImg.sequence = i + 1;
                }
            }
                        DC_RENTAL.SubmitChanges();
            AppSettings._refreshCache_RNT_ESTATEs();
        }
    }
}