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
using ModRental;

namespace RentalInRome.admin.modRental
{
    public partial class EstateExtraMedia : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "extras";
        }
        protected dbRntEstateExtrasTB currTBL;
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
               // UC_rnt_estate_navlinks1.IdEstate = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == IdEstate);
                    if (currTBL == null)
                    {
                        //Response.Redirect("../rnt_estate_list.aspx");
                        Response.Redirect("EstateExtrasList.aspx");
                        return;
                    }
                }
                
                   ltrCurrType.Text = "Gallery del Appartamento";
                   //HL_otherType.Text = "Carica le foto del Appartamento";
                   
                string media_folder = Convert.ToString(currTBL.id);
             
                string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "images/ExtraServices");
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                _path = Path.Combine(_path,media_folder.Trim());
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
                _path = Path.Combine(_path, "thumb");
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
               
                txt_media_folder.Text = "images/ExtraServices" + "/" + media_folder;
                LV_gallery_DataBind();
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
            using (DCmodRental dc = new DCmodRental())
            {
                LV_gallery.DataSource = dc.dbRntExtrasMediaRLs.Where(x => x.pid_estate_extra == IdEstate).OrderBy(x => x.sequence).ToList();
                LV_gallery.DataBind();
            }
        }
        protected void saveSequence()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTBL == null)
                {
                    //Response.Redirect("../rnt_estate_list.aspx");
                    Response.Redirect("EstateExtrasList.aspx");
                    return;
                }
                List<string> _list = HF_order.Value.splitStringToList("|");
                for (int i = 0; i < _list.Count; i++)
                {
                    var currImg = dc.dbRntExtrasMediaRLs.SingleOrDefault(x => x.id == _list[i].ToInt32());
                    if (currImg != null)
                    {
                        currImg.sequence = i + 1;
                    }
                    if (i == 0)
                    {
                        //currTBL.img_banner = currImg.img_banner;
                        currTBL.imgPreview = currImg.img_banner;
                        currTBL.imgThumb = currTBL.imgThumb;
                    }
                }
                dc.SaveChanges();
            }
        }
        protected bool saveUploadedFile(string tmpFilesFolder, string uploadedFilePath, string imgExtension, ImageFormat imgFormat, string imgPath, int imgWidth, int imgHeight, string imgWatermarkPath, int imgWatermarkPadX, int imgWatermarkPadY, bool imgWatermarkFloatRight)
        {
            if (!File.Exists(Path.Combine(App.SRP, uploadedFilePath)))
                return false;
            if (imgWidth > 0 && imgHeight > 0)
            {
                using (var currImg = System.Drawing.Image.FromFile(Path.Combine(App.SRP, uploadedFilePath)))
                {
                    if (currImg.Width < imgWidth) imgWidth = currImg.Width;
                    if (currImg.Height < imgHeight) imgHeight = currImg.Height;
                    if (currImg.Width > imgWidth || currImg.Height > imgHeight)
                    {
                        int newWidth = imgWidth;
                        int newHeight = decimal.Multiply(decimal.Divide(imgWidth, currImg.Width), currImg.Height).objToInt32();
                        if (newHeight < imgHeight)
                        {
                            newHeight = imgHeight;
                            newWidth = decimal.Multiply(decimal.Divide(currImg.Width, currImg.Height), imgHeight).objToInt32();
                        }

                        using (var oBitmap = new Bitmap(newWidth, newHeight))
                        using (var thumbGraph = Graphics.FromImage(oBitmap))
                        {
                            uploadedFilePath = tmpFilesFolder + "/" + string.Empty.createUniqueID() + imgExtension;
                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                            thumbGraph.DrawImage(currImg, imageRectangle);
                            oBitmap.Save(Path.Combine(App.SRP, uploadedFilePath), imgFormat);
                        }
                    }
                }
            }
            else
            {
                using (var newImg = System.Drawing.Image.FromFile(Path.Combine(App.SRP, uploadedFilePath)))
                {
                    imgWidth = newImg.Width;
                    imgHeight = newImg.Height;
                }
            }
            using (var newImg = System.Drawing.Image.FromFile(Path.Combine(App.SRP, uploadedFilePath)))
            {
                using (var oBitmap = new Bitmap(imgWidth, imgHeight))
                using (var thumbGraph = Graphics.FromImage(oBitmap))
                {
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, imgWidth, imgHeight);
                    thumbGraph.DrawImage(newImg, imageRectangle, 0, 0, imgWidth, imgHeight, GraphicsUnit.Pixel);
                    if (imgWatermarkPath != "" && File.Exists(Path.Combine(App.SRP, imgWatermarkPath)))
                        using (var imgWatermark = System.Drawing.Image.FromFile(Path.Combine(App.SRP, imgWatermarkPath)))
                            thumbGraph.DrawImage(imgWatermark, new Rectangle((imgWatermarkFloatRight ? (imgWidth - imgWatermark.Width - imgWatermarkPadX) : imgWatermarkPadX), imgWatermarkPadY, imgWatermark.Width, imgWatermark.Height));
                    oBitmap.Save(Path.Combine(App.SRP, imgPath), imgFormat);
                }
            }
            return true;
        }
        protected void dragDropUpload_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTBL == null)
                {
                    //Response.Redirect("../rnt_estate_list.aspx");
                    Response.Redirect("EstateExtrasList.aspx");
                    return;
                }
                string tmpFilesFolder = "files";
                if (!Directory.Exists(Path.Combine(App.SRP, tmpFilesFolder))) Directory.CreateDirectory(Path.Combine(App.SRP, tmpFilesFolder));
                tmpFilesFolder += "/tmp";
                if (!Directory.Exists(Path.Combine(App.SRP, tmpFilesFolder))) Directory.CreateDirectory(Path.Combine(App.SRP, tmpFilesFolder));


                int imgThumb_width = 150;
                int imgThumb_height = 150;
                string ImgRoot = txt_media_folder.Text;
                string ImgThumbRoot = txt_media_folder.Text + "/thumb";
                string ImgName = e.File.GetNameWithoutExtension();
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
                saveUploadedFile(tmpFilesFolder, ImgPathTmp, ImgExtension, imgFormat, ImgPathBig, txt_imgBigW.Text.ToInt32(), txt_imgBigH.Text.ToInt32(), txt_imgWatermarkBigPath.Text, txt_imgWatermarkBigPadX.Text.ToInt32(), txt_imgWatermarkBigPadY.Text.ToInt32(), drp_imgWatermarkBigFloat.SelectedValue == "Right");
                saveUploadedFile(tmpFilesFolder, ImgPathTmp, ImgExtension, imgFormat, ImgPathSmall, txt_imgSmallW.Text.ToInt32(), txt_imgSmallH.Text.ToInt32(), txt_imgWatermarkSmallPath.Text, txt_imgWatermarkSmallPadX.Text.ToInt32(), txt_imgWatermarkSmallPadY.Text.ToInt32(), drp_imgWatermarkSmallFloat.SelectedValue == "Right");
                // salva
                int _sequence = 1;
                var _allCollection =
                    dc.dbRntExtrasMediaRLs.Where(x => x.pid_estate_extra == IdEstate).OrderBy(x => x.sequence).ToList();
                if (_allCollection.Count != 0)
                {
                    _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                }
                var currImg = new dbRntExtrasMediaRL();
                currImg.sequence = _sequence;
                currImg.pid_estate_extra = IdEstate;
                currImg.code = "";
                //currImg.type = HF_type.Value;
                currImg.img_banner = ImgPathBig;
                currImg.img_thumb = ImgPathSmall;
                dc.Add(currImg);
                dc.SaveChanges();
                if (_sequence == 1)
                {
                    //currTBL.img_banner = currImg.img_banner;
                    currTBL.imgPreview = currImg.img_banner;
                    currTBL.imgThumb = currTBL.imgThumb;
                    dc.SaveChanges();
                }
            }

        }
    }
}