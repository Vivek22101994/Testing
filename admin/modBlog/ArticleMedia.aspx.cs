using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace ModBlog.admin.modBlog
{
    public partial class ArticleMedia : adminBasePage
    {
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
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "blog";
        }
        protected dbBlogArticleTB currTBL;
        public long IdArticle
        {
            get
            {
                return HF_IdArticle.Value.ToInt64();
            }
            set
            {
                HF_IdArticle.Value = value.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdArticle = Request.QueryString["id"].ToInt64();
                using (DCmodBlog dc = new DCmodBlog())
                {
                    currTBL = dc.dbBlogArticleTBs.SingleOrDefault(x => x.id == IdArticle);
                    if (currTBL == null)
                    {
                        CloseRadWindow("");
                        pnlDett.Visible = false;
                        return;
                    }
                    var tmpTbl = dc.dbBlogArticleVIEWs.SingleOrDefault(x => x.id == IdArticle && x.pidLang == 1);
                    if (tmpTbl == null) tmpTbl = dc.dbBlogArticleVIEWs.SingleOrDefault(x => x.id == IdArticle && x.pidLang == 2);
                    if (tmpTbl == null)
                    {
                        CloseRadWindow("");
                        pnlDett.Visible = false;
                        return;
                    }
                    ltr_apartment.Text = tmpTbl.title + " / " + "rif. " + tmpTbl.id;
                }
                string currFolder = "images/blog";
                if (!Directory.Exists(Path.Combine(App.SRP, currFolder)))
                    Directory.CreateDirectory(Path.Combine(App.SRP, currFolder));
                currFolder += "/article";
                if (!Directory.Exists(Path.Combine(App.SRP, currFolder)))
                    Directory.CreateDirectory(Path.Combine(App.SRP, currFolder));
                currFolder += "/" + currTBL.id;
                if (!Directory.Exists(Path.Combine(App.SRP, currFolder)))
                    Directory.CreateDirectory(Path.Combine(App.SRP, currFolder));
                currFolder += "/gallery";
                if (!Directory.Exists(Path.Combine(App.SRP, currFolder)))
                    Directory.CreateDirectory(Path.Combine(App.SRP, currFolder));
                HF_currFolder.Value = currFolder + "";
                currFolder += "/thumb";
                if (!Directory.Exists(Path.Combine(App.SRP, currFolder)))
                    Directory.CreateDirectory(Path.Combine(App.SRP, currFolder));

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
            using (DCmodBlog dc = new DCmodBlog())
            {
                LV_gallery.DataSource = dc.dbBlogArticleMediaTBLs.Where(x => x.pidArticle == IdArticle).OrderBy(x => x.sequence).ToList();
                LV_gallery.DataBind();
            }
        }
        protected void saveSequence()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                List<string> _list = HF_order.Value.splitStringToList("|");
                for (int i = 0; i < _list.Count; i++)
                {
                    dbBlogArticleMediaTBL currImg = dc.dbBlogArticleMediaTBLs.SingleOrDefault(x => x.id == _list[i].ToInt64());
                    if (currImg != null)
                    {
                        currImg.sequence = i + 1;
                    }
                    if (i == 0)
                    {
                        currTBL = dc.dbBlogArticleTBs.SingleOrDefault(x => x.id == IdArticle);
                        if (currTBL != null)
                        {
                            currTBL.imgBanner = currImg.img_banner;
                            currTBL.imgPreview = currImg.img_thumb;
                        }
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
            using (DCmodBlog dc = new DCmodBlog())
            {
                currTBL = dc.dbBlogArticleTBs.SingleOrDefault(x => x.id == IdArticle);
                if (currTBL == null)
                {
                    CloseRadWindow("");
                    pnlDett.Visible = false;
                    return;
                }
                var tmpTbl = dc.dbBlogArticleVIEWs.SingleOrDefault(x => x.id == IdArticle && x.pidLang == 1);
                if (tmpTbl == null) tmpTbl = dc.dbBlogArticleVIEWs.SingleOrDefault(x => x.id == IdArticle && x.pidLang == 2);
                if (tmpTbl == null)
                {
                    CloseRadWindow("");
                    pnlDett.Visible = false;
                    return;
                }

                string tmpFilesFolder = "files";
                if (!Directory.Exists(Path.Combine(App.SRP, tmpFilesFolder))) Directory.CreateDirectory(Path.Combine(App.SRP, tmpFilesFolder));
                tmpFilesFolder += "/tmp";
                if (!Directory.Exists(Path.Combine(App.SRP, tmpFilesFolder))) Directory.CreateDirectory(Path.Combine(App.SRP, tmpFilesFolder));


                int imgThumb_width = 150;
                int imgThumb_height = 150;
                string ImgRoot = HF_currFolder.Value;
                string ImgThumbRoot = HF_currFolder.Value + "/thumb";
                string ImgName = tmpTbl.title.clearPathName();
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
                List<dbBlogArticleMediaTBL> _allCollection =
                    dc.dbBlogArticleMediaTBLs.Where(x => x.pidArticle == IdArticle).OrderBy(x => x.sequence).ToList();
                if (_allCollection.Count != 0)
                {
                    _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                }
                dbBlogArticleMediaTBL currImg = new dbBlogArticleMediaTBL();
                currImg.sequence = _sequence;
                currImg.pidArticle = IdArticle;
                currImg.code = "";
                currImg.img_banner = ImgPathBig;
                currImg.img_thumb = ImgPathSmall;
                dc.Add(currImg);
                dc.SaveChanges();

                if (_sequence == 1)
                {
                    currTBL.imgBanner = currImg.img_banner;
                    currTBL.imgPreview = currImg.img_thumb;
                    dc.SaveChanges();
                }
            }
        }
    }
}