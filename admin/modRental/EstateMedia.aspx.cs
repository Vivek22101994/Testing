using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace ModRental.admin.modRental
{
    public partial class EstateMedia : adminBasePage
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
                    tmp.Add(new TmpClassMediaType("original", "Immagini originali", "Carica le foto originali"));
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
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currTBL == null)
                {
                    Response.Redirect("../rnt_estate_list.aspx");
                    return;
                }

                ltr_apartment.Text = currTBL.code + " / " + "rif. " + currTBL.id;
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                if (currTBL.pid_city == 1)
                    HF_main_folder.Value = "romeapartmentsphoto";
                else if (currTBL.pid_city == 2)
                    HF_main_folder.Value = "florenceapartmentsphoto";
                else if (currTBL.pid_city == 3)
                    HF_main_folder.Value = "veniceapartmentsphoto";
                else if (currTBL.pid_city == 4)
                    HF_main_folder.Value = "kenyaapartmentsphoto";
                else
                    HF_main_folder.Value = "apartmentsphoto";

                HF_type.Value = "" + Request.QueryString["type"];
                var currType = MediaTypes.SingleOrDefault(x => x.type == HF_type.Value);
                if (currType == null)
                {
                    HF_type.Value = "gallery";
                    currType = MediaTypes.SingleOrDefault(x => x.type == HF_type.Value);
                }
                if (currType != null)
                {
                    ltrCurrType.Text = currType.currTitle;
                }
                LV_otherType.DataSource = MediaTypes.Where(x => x.type != HF_type.Value);
                LV_otherType.DataBind();
                foreach (var item in LV_otherType.Items)
                {
                    Label lblType = item.FindControl("lblType") as Label;
                    HyperLink HL = item.FindControl("HL") as HyperLink;
                    currType = MediaTypes.SingleOrDefault(x => x.type == lblType.Text);
                    if (currType != null)
                    {
                        HL.Text = currType.lnkTitle;
                        HL.NavigateUrl = "EstateMedia.aspx?id=" + IdEstate + "&type=" + currType.type;
                    }
                }
                if (HF_type.Value == "gallery")
                {
                    txt_imgWatermarkBigPath.Text = HF_main_folder.Value + "/img_watermark_big.png";
                    txt_imgWatermarkSmallPath.Text = HF_main_folder.Value + "/img_watermark_small.png";
                }
                else
                {
                    txt_imgWatermarkBigPath.Text = "";
                    txt_imgWatermarkSmallPath.Text = "";
                }
                if (HF_type.Value == "homeaway")
                {
                    txt_imgBigW.Text = "1024";
                    txt_imgBigH.Text = "768";
                } 
                
                // check folder
                if (string.IsNullOrEmpty(currTBL.media_folder))
                {
                    currTBL.media_folder = currTBL.code.clearPathName() + "_" + currTBL.id;
                    DC_RENTAL.SubmitChanges();
                }
                string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_main_folder.Value);
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                _path = Path.Combine(_path, currTBL.media_folder.Trim());
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                _path = Path.Combine(_path, HF_type.Value);
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                _path = Path.Combine(_path, "thumb");
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);

                txt_media_folder.Text = HF_main_folder.Value + "/" + currTBL.media_folder;
                LV_gallery_DataBind();

                ltr_imageSize.Text = txt_imgBigW.Text + " x " + txt_imgBigH.Text;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setSortable", "setSortable();", true);
            }
        }
        protected void pnlDett_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "sortable") saveSequence();
            LV_gallery_DataBind();
        }
        protected void LV_gallery_DataBind()
        {
            LV_gallery.DataSource = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == HF_type.Value).OrderBy(x => x.sequence).ToList();
            LV_gallery.DataBind();
        }
        protected void saveSequence()
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("rnt_estate_list.aspx");
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
                if (HF_type.Value == "gallery")
                {
                    if (i == 0)
                    {
                        currTBL.img_banner = currImg.img_banner;
                        currTBL.img_preview_1 = currImg.img_thumb;
                    }
                    if (i == 1)
                        currTBL.img_preview_2 = currImg.img_thumb;
                    if (i == 2)
                        currTBL.img_preview_3 = currImg.img_thumb;
                }
            }
            DC_RENTAL.SubmitChanges();
            AppSettings._refreshCache_RNT_ESTATEs();
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "radAlert", "radalert(\"Ordinamento salvato correttamente.\");", true);
        }
        protected void dragDropUpload_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("rnt_estate_list.aspx");
                return;
            }

            string tmpFilesFolder = "files";
            if (!Directory.Exists(Path.Combine(App.SRP, tmpFilesFolder))) Directory.CreateDirectory(Path.Combine(App.SRP, tmpFilesFolder));
            tmpFilesFolder += "/tmp";
            if (!Directory.Exists(Path.Combine(App.SRP, tmpFilesFolder))) Directory.CreateDirectory(Path.Combine(App.SRP, tmpFilesFolder));


            int imgThumb_width = 150;
            int imgThumb_height = 150;
            string ImgRoot = txt_media_folder.Text + "/" + HF_type.Value;
            string ImgThumbRoot = txt_media_folder.Text + "/" + HF_type.Value + "/thumb";
            int nextId = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Max(x => x.id).objToInt32() + 1;
            string ImgName = e.File.GetNameWithoutExtension() + "_" + nextId;
            string ImgExtension = e.File.GetExtension();
            string ImgPathTmp = tmpFilesFolder + "/" + string.Empty.createUniqueID() + ImgExtension;
            string ImgPathBig = ImgRoot + "/" + ImgName + ImgExtension;
            string ImgPathSmall = ImgThumbRoot + "/" + ImgName + ImgExtension;
            e.File.SaveAs(Path.Combine(App.SRP, ImgPathTmp), true);
            if (File.Exists(Path.Combine(App.SRP, ImgPathBig)) || File.Exists(Path.Combine(App.SRP, ImgPathSmall)))
            {
                int index = 1;
                while (File.Exists(Path.Combine(App.SRP, ImgPathBig)) || File.Exists(Path.Combine(App.SRP, ImgPathSmall)))
                {
                    ImgPathBig = ImgRoot + "/" + ImgName + "_" + index + ImgExtension;
                    ImgPathSmall = ImgThumbRoot + "/" + ImgName + "_" + index + ImgExtension;
                    index++;
                }
            }

            ImageFormat imgFormat = ImageFormat.Jpeg;
            if (ImgExtension == "png") imgFormat = ImageFormat.Png;
            if (ImgExtension == "gif") imgFormat = ImageFormat.Gif;
            contUtils.saveUploadedFile(tmpFilesFolder, ImgPathTmp, ImgExtension, imgFormat, ImgPathBig, txt_imgBigW.Text.ToInt32(), txt_imgBigH.Text.ToInt32(), txt_imgWatermarkBigPath.Text, txt_imgWatermarkBigPadX.Text.ToInt32(), txt_imgWatermarkBigPadY.Text.ToInt32(), drp_imgWatermarkBigFloat.SelectedValue == "Right");
            contUtils.saveUploadedFile(tmpFilesFolder, ImgPathTmp, ImgExtension, imgFormat, ImgPathSmall, txt_imgSmallW.Text.ToInt32(), txt_imgSmallH.Text.ToInt32(), txt_imgWatermarkSmallPath.Text, txt_imgWatermarkSmallPadX.Text.ToInt32(), txt_imgWatermarkSmallPadY.Text.ToInt32(), drp_imgWatermarkSmallFloat.SelectedValue == "Right");
            // salva
            int _sequence = 1;
            List<RNT_RL_ESTATE_MEDIA> _allCollection =
                DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == HF_type.Value).OrderBy(x => x.sequence).ToList();
            if (_allCollection.Count != 0)
            {
                _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
            }
            RNT_RL_ESTATE_MEDIA currImg = new RNT_RL_ESTATE_MEDIA();
            currImg.sequence = _sequence;
            currImg.pid_estate = IdEstate;
            currImg.code = "";
            currImg.type = HF_type.Value;
            currImg.img_banner = ImgPathBig;
            currImg.img_thumb = ImgPathSmall;
            DC_RENTAL.RNT_RL_ESTATE_MEDIAs.InsertOnSubmit(currImg);
            DC_RENTAL.SubmitChanges();


            if (HF_type.Value == "gallery")
            {
                if (_sequence == 1)
                {
                    currTBL.img_banner = currImg.img_banner;
                    currTBL.img_preview_1 = currImg.img_thumb;
                }
                if (_sequence == 2)
                    currTBL.img_preview_2 = currImg.img_thumb;
                if (_sequence == 3)
                    currTBL.img_preview_3 = currImg.img_thumb;
                DC_RENTAL.SubmitChanges();
            }
        }
    }
}