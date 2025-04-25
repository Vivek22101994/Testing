using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentalInRome.data;
using ModContent;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using HtmlAgilityPack;

public class contUtils
{
    public static string CleanHtmlText(string Value)
    {
        var cleanValue = "<body>" + Value.htmlDecode() + "</body>";
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(cleanValue);
        try { cleanValue = doc.DocumentNode.SelectSingleNode("//body").InnerText; }
        catch (Exception exxxx) { cleanValue = doc.DocumentNode.InnerText; }
        return cleanValue;
    }
    public static bool saveUploadedFile_old(string tmpFilesFolder, string uploadedFilePath, string imgExtension, ImageFormat imgFormat, string imgPath, int imgWidth, int imgHeight, string imgWatermarkPath, int imgWatermarkPadX, int imgWatermarkPadY, bool imgWatermarkFloatRight)
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
    public static bool saveUploadedFile(string tmpFilesFolder, string uploadedFilePath, string imgExtension, ImageFormat imgFormat, string imgPath, int imgWidth, int imgHeight, string imgWatermarkPath, int imgWatermarkPadX, int imgWatermarkPadY, bool imgWatermarkFloatRight)
    {
        if (!File.Exists(Path.Combine(App.SRP, uploadedFilePath)))
            return false;
        if (imgWidth > 0 || imgHeight > 0)
        {
            using (var currImg = System.Drawing.Image.FromFile(Path.Combine(App.SRP, uploadedFilePath)))
            {
                if (currImg.Width < imgWidth) imgWidth = currImg.Width;
                if (currImg.Height < imgHeight) imgHeight = currImg.Height;
                if (currImg.Width > imgWidth || (imgHeight != 0 && currImg.Height > imgHeight))
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
                    imgWidth = newWidth;
                    imgHeight = newHeight;
                }

                if (imgHeight == 0)
                    imgHeight = currImg.Height;
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
    public static string getImg(string imgPath, string imgAlternative, string formatString, bool checkFileExist)
    {
        imgPath = (imgPath + "").Trim();
        if (imgPath == "")
            imgPath = (imgAlternative + "").Trim();
        if (imgPath == "" || (checkFileExist && !File.Exists(Path.Combine(App.SRP, imgPath))))
            return "";

        return formatString.Replace("#imgPath#", "/" + imgPath);
    }
    public static string getLang_title(string id)
    {
        return getLang_title(id.ToInt32());
    }
    public static string getLang_title(int id)
    {
        CONT_TBL_LANG tmp = contProps.LangTBL.SingleOrDefault(x => x.id == id);
        if (tmp != null)
            return tmp.title;
        return "";
    }
    public static string getLang_folder(int id)
    {
        CONT_TBL_LANG _s = contProps.LangTBL.SingleOrDefault(x => x.id == id);
        if (_s != null && !string.IsNullOrEmpty(_s.abbr))
            return _s.abbr;
        return "";
    }
    public static string getLang_code(int id)
    {
        CONT_TBL_LANG _s = contProps.LangTBL.SingleOrDefault(x => x.id == id);
        if (_s != null && !string.IsNullOrEmpty(_s.abbr))
            return _s.abbr;
        return "";
    }
    public static string getLanguage_code(int id)
    {
        CONT_TBL_LANG _s = contProps.LangTBL.SingleOrDefault(x => x.id == id);
        if (_s != null && !string.IsNullOrEmpty(_s.code))
            return _s.code;
        return "";
    }
    public static string getLanguage_HAcode(int id)
    { 
        CONT_TBL_LANG _s = contProps.LangTBL.SingleOrDefault(x => x.id == id);
        if (_s != null && !string.IsNullOrEmpty(_s.HACode))
            return _s.HACode;
        return "";
    }
    public static string getLanguage_HAcodeByAbbr(string abbr)
    {
        CONT_TBL_LANG _s = contProps.LangTBL.SingleOrDefault(x => x.abbr == abbr);
        if (_s != null && !string.IsNullOrEmpty(_s.HACode))
            return _s.HACode;
        return "";
    }
    public static string getLang_commonName(int id)
    {
        var tmp = contProps.LangTBL.SingleOrDefault(x => x.id == id);
        if (tmp != null)
            return tmp.common_name;
        return "";
    }
    public static int getLangIdFromLocale(string locale)
    {
        var tmp = contProps.LangTBL.FirstOrDefault(x => x.common_name == locale);
        if (tmp == null) tmp = contProps.LangTBL.FirstOrDefault(x => x.abbr == locale);
        if (tmp == null && locale.Length > 2) tmp = contProps.LangTBL.FirstOrDefault(x => x.abbr == locale.Substring(0, 2));
        return (tmp != null) ? tmp.id : App.DefLangID;
    }
    public static string getLabel(string id)
    {
        return getLabel_title(id, CurrentLang.ID, id);
    }
    public static string getLabel(string id, int pidLang, string def)
    {
        return getLabel_title(id, pidLang, def);
    }
    public static string getLabel_title(string id, int pidLang, string def)
    {
        dbContLabelTBL tmp = contProps.LabelTBL.FirstOrDefault(x => x.id == id && x.pidLang == pidLang);
        if (tmp == null || string.IsNullOrEmpty(tmp.title)) tmp = contProps.LabelTBL.FirstOrDefault(x => x.id == id && x.pidLang == 2);
        if (tmp == null || string.IsNullOrEmpty(tmp.title)) tmp = contProps.LabelTBL.FirstOrDefault(x => x.id == id && x.pidLang == 1);
        if (tmp != null) return tmp.title;
        return def;
    }
    public static string getLang_jsCalFile(int id)
    {
        CONT_TBL_LANG _s = contProps.LangTBL.SingleOrDefault(x => x.id == id);
        if (_s != null && _s.js_cal_file != null && _s.js_cal_file != "")
            return _s.js_cal_file;
        return "en.js";
    }
    public static string contStp_title(int id, int lang, string alternate)
    {
        string tmp = getStp(id, lang, "#title#");
        if (tmp != "")
            return tmp;
        return alternate;
    }
    public static string getStp(int id, int pidLang, string format)
    {
        using (DCmodContent dc = new DCmodContent())
        {
            CONT_VIEW_STP tmp = contProps.CONT_STPs.SingleOrDefault(x => x.id == id && x.pid_lang == pidLang);
            if (tmp == null)
                tmp = contProps.CONT_STPs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
            if (tmp == null)
                tmp = contProps.CONT_STPs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
            if (tmp != null)
                return string.IsNullOrEmpty(format) ? tmp.description : format.Replace("#title#", tmp.title).Replace("#subTitle#", tmp.sub_title).Replace("#summary#", tmp.summary).Replace("#description#", tmp.description);
            return format;
        }
    }
    public static string getBlock_description(int id, int lang, string alternate)
    {
        string tmp = getBlock(id, lang, "#title#");
        if (tmp != "")
            return tmp;
        return alternate;
    }
    public static string getBlock(int id, int pidLang, string format)
    {
        using (DCmodContent dc = new DCmodContent())
        {
            var tmp = contProps.CONT_BLOCKs.SingleOrDefault(x => x.id == id && x.pid_lang == pidLang);
            if (tmp == null)
                tmp = contProps.CONT_BLOCKs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
            if (tmp == null)
                tmp = contProps.CONT_BLOCKs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
            if (tmp != null)
                return string.IsNullOrEmpty(format) ? tmp.description : format.Replace("#title#", tmp.title).Replace("#subTitle#", tmp.sub_title).Replace("#summary#", tmp.summary).Replace("#description#", tmp.description);
            return format;
        }
    }
}
public class contProps
{
    private static List<dbContLabelTBL> tmpLabelTBL; // refresh Auto
    public static List<dbContLabelTBL> LabelTBL
    {
        get
        {
            if (tmpLabelTBL == null)
            {
                using (DCmodContent dc = new DCmodContent())
                    tmpLabelTBL = dc.dbContLabelTBLs.ToList();
            }
            return new List<dbContLabelTBL>(tmpLabelTBL);
        }
        set { tmpLabelTBL = value; }
    }
    private static List<dbContLabelVIEW> tmpLabelVIEW; // refresh Auto
    public static List<dbContLabelVIEW> LabelVIEW
    {
        get
        {
            if (tmpLabelVIEW == null)
            {
                using (DCmodContent dc = new DCmodContent())
                    tmpLabelVIEW = dc.dbContLabelVIEWs.ToList();
            }
            return new List<dbContLabelVIEW>(tmpLabelVIEW);
        }
        set { tmpLabelVIEW = value; }
    }
    private static List<CONT_TBL_LANG> tmpLangTBL; // refresh Auto
    public static List<CONT_TBL_LANG> LangTBL
    {
        get
        {
            if (tmpLangTBL == null)
            {
                tmpLangTBL = maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.ToList();
            }
            return new List<CONT_TBL_LANG>(tmpLangTBL);
        }
        set { tmpLangTBL = value; }
    }
    private static List<CONT_VIEW_STP> tmpCONT_STPs; // refresh AUTO
    public static List<CONT_VIEW_STP> CONT_STPs
    {
        get
        {
            if (tmpCONT_STPs == null)
            {
                tmpCONT_STPs = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.ToList();
            }
            return new List<CONT_VIEW_STP>(tmpCONT_STPs.Select(x => x.Clone())); ;
        }
        set { tmpCONT_STPs = value; }
    }
    private static List<CONT_VIEW_TBL_BLOCK> tmpCONT_BLOCKs; // refresh AUTO
    public static List<CONT_VIEW_TBL_BLOCK> CONT_BLOCKs
    {
        get
        {
            if (tmpCONT_BLOCKs == null)
            {
                tmpCONT_BLOCKs = maga_DataContext.DC_CONTENT.CONT_VIEW_TBL_BLOCKs.ToList();
            }
            return new List<CONT_VIEW_TBL_BLOCK>(tmpCONT_BLOCKs); ;
        }
        set { tmpCONT_BLOCKs = value; }
    }
}
public static class contExts
{
    #region Label
    public static dbContLabelTBL Clone(this dbContLabelTBL source)
    {
        dbContLabelTBL clone = new dbContLabelTBL();
        clone.id = source.id;
        clone.pidLang = source.pidLang;
        clone.type = source.type;
        clone.title = source.title;
        return clone;
    }
    public static void CopyTo(this dbContLabelTBL source, ref dbContLabelTBL copyto)
    {
        copyto.id = source.id;
        copyto.pidLang = source.pidLang;
        copyto.type = source.type;
        copyto.title = source.title;
    }
    public static dbContLabelVIEW Clone(this dbContLabelVIEW source)
    {
        dbContLabelVIEW clone = new dbContLabelVIEW();
        clone.id = source.id;
        clone.type = source.type;
        clone.mTitle = source.mTitle;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        return clone;
    }
    #endregion
    #region RifStp
    public static dbRifContStpTB Clone(this dbRifContStpTB source)
    {
        dbRifContStpTB clone = new dbRifContStpTB();
        clone.id = source.id;
        clone.pageName = source.pageName;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        clone.endBody = source.endBody;
        clone.endHead = source.endHead;
        return clone;
    }
    public static void CopyTo(this dbRifContStpTB source, ref dbRifContStpTB copyto)
    {
        copyto.pageName = source.pageName;
        copyto.imgThumb = source.imgThumb;
        copyto.imgPreview = source.imgPreview;
        copyto.imgBanner = source.imgBanner;
        copyto.cssFile = source.cssFile;
        copyto.pageRewrite = source.pageRewrite;
        copyto.isActive = source.isActive;
        copyto.endBody = source.endBody;
        copyto.endHead = source.endHead;
    }
    public static dbRifContStpLN Clone(this dbRifContStpLN source)
    {
        dbRifContStpLN clone = new dbRifContStpLN();
        clone.pidStp = source.pidStp;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        clone.subTitle = source.subTitle;
        clone.summary = source.summary;
        clone.description = source.description;
        clone.pagePath = source.pagePath;
        clone.metaTitle = source.metaTitle;
        clone.metaKeywords = source.metaKeywords;
        clone.metaDescription = source.metaDescription;
        clone.menuTitle = source.menuTitle;
        clone.imgBannerTitle = source.imgBannerTitle;

        return clone;
    }
    public static void CopyTo(this dbRifContStpLN source, ref dbRifContStpLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.menuTitle = source.menuTitle;
        copyto.summary = source.summary;
        copyto.description = source.description;
        copyto.pagePath = source.pagePath;
        copyto.metaTitle = source.metaTitle;
        copyto.metaKeywords = source.metaKeywords;
        copyto.metaDescription = source.metaDescription;
        copyto.imgBannerTitle = source.imgBannerTitle;
    }
    public static dbRifContStpVIEW Clone(this dbRifContStpVIEW source)
    {
        dbRifContStpVIEW clone = new dbRifContStpVIEW();
        clone.id = source.id;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        clone.subTitle = source.subTitle;
        clone.summary = source.summary;
        clone.description = source.description;
        clone.pagePath = source.pagePath;
        clone.metaTitle = source.metaTitle;
        clone.metaKeywords = source.metaKeywords;
        clone.metaDescription = source.metaDescription;
        clone.pageName = source.pageName;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        clone.endBody = source.endBody;
        clone.endHead = source.endHead;
        clone.menuTitle = source.menuTitle;
        clone.imgBannerTitle = source.imgBannerTitle;
        return clone;
    }

    #endregion
    #region RiKenyaStp
    public static dbRiKenyaContStpTB Clone(this dbRiKenyaContStpTB source)
    {
        dbRiKenyaContStpTB clone = new dbRiKenyaContStpTB();
        clone.id = source.id;
        clone.pageName = source.pageName;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        clone.endBody = source.endBody;
        clone.endHead = source.endHead;
        return clone;
    }
    public static void CopyTo(this dbRiKenyaContStpTB source, ref dbRiKenyaContStpTB copyto)
    {
        copyto.pageName = source.pageName;
        copyto.imgThumb = source.imgThumb;
        copyto.imgPreview = source.imgPreview;
        copyto.imgBanner = source.imgBanner;
        copyto.cssFile = source.cssFile;
        copyto.pageRewrite = source.pageRewrite;
        copyto.isActive = source.isActive;
        copyto.endBody = source.endBody;
        copyto.endHead = source.endHead;
    }
    public static dbRiKenyaContStpLN Clone(this dbRiKenyaContStpLN source)
    {
        dbRiKenyaContStpLN clone = new dbRiKenyaContStpLN();
        clone.pidStp = source.pidStp;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        clone.subTitle = source.subTitle;
        clone.summary = source.summary;
        clone.description = source.description;
        clone.pagePath = source.pagePath;
        clone.metaTitle = source.metaTitle;
        clone.metaKeywords = source.metaKeywords;
        clone.metaDescription = source.metaDescription;
        clone.menuTitle = source.menuTitle;
        clone.imgBannerTitle = source.imgBannerTitle;

        return clone;
    }
    public static void CopyTo(this dbRiKenyaContStpLN source, ref dbRiKenyaContStpLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.menuTitle = source.menuTitle;
        copyto.summary = source.summary;
        copyto.description = source.description;
        copyto.pagePath = source.pagePath;
        copyto.metaTitle = source.metaTitle;
        copyto.metaKeywords = source.metaKeywords;
        copyto.metaDescription = source.metaDescription;
        copyto.imgBannerTitle = source.imgBannerTitle;
    }
    public static dbRiKenyaContStpVIEW Clone(this dbRiKenyaContStpVIEW source)
    {
        dbRiKenyaContStpVIEW clone = new dbRiKenyaContStpVIEW();
        clone.id = source.id;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        clone.subTitle = source.subTitle;
        clone.summary = source.summary;
        clone.description = source.description;
        clone.pagePath = source.pagePath;
        clone.metaTitle = source.metaTitle;
        clone.metaKeywords = source.metaKeywords;
        clone.metaDescription = source.metaDescription;
        clone.pageName = source.pageName;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        clone.endBody = source.endBody;
        clone.endHead = source.endHead;
        clone.menuTitle = source.menuTitle;
        clone.imgBannerTitle = source.imgBannerTitle;
        return clone;
    }

    #endregion
    #region RivStp
    public static dbRivContStpTB Clone(this dbRivContStpTB source)
    {
        dbRivContStpTB clone = new dbRivContStpTB();
        clone.id = source.id;
        clone.pageName = source.pageName;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        clone.endBody = source.endBody;
        clone.endHead = source.endHead;
        return clone;
    }
    public static void CopyTo(this dbRivContStpTB source, ref dbRivContStpTB copyto)
    {
        copyto.pageName = source.pageName;
        copyto.imgThumb = source.imgThumb;
        copyto.imgPreview = source.imgPreview;
        copyto.imgBanner = source.imgBanner;
        copyto.cssFile = source.cssFile;
        copyto.pageRewrite = source.pageRewrite;
        copyto.isActive = source.isActive;
        copyto.endBody = source.endBody;
        copyto.endHead = source.endHead;
    }
    public static dbRivContStpLN Clone(this dbRivContStpLN source)
    {
        dbRivContStpLN clone = new dbRivContStpLN();
        clone.pidStp = source.pidStp;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        clone.subTitle = source.subTitle;
        clone.summary = source.summary;
        clone.description = source.description;
        clone.pagePath = source.pagePath;
        clone.metaTitle = source.metaTitle;
        clone.metaKeywords = source.metaKeywords;
        clone.metaDescription = source.metaDescription;
        clone.menuTitle = source.menuTitle;
        clone.imgBannerTitle = source.imgBannerTitle;

        return clone;
    }
    public static void CopyTo(this dbRivContStpLN source, ref dbRivContStpLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.menuTitle = source.menuTitle;
        copyto.summary = source.summary;
        copyto.description = source.description;
        copyto.pagePath = source.pagePath;
        copyto.metaTitle = source.metaTitle;
        copyto.metaKeywords = source.metaKeywords;
        copyto.metaDescription = source.metaDescription;
        copyto.imgBannerTitle = source.imgBannerTitle;
    }
    public static dbRivContStpVIEW Clone(this dbRivContStpVIEW source)
    {
        dbRivContStpVIEW clone = new dbRivContStpVIEW();
        clone.id = source.id;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        clone.subTitle = source.subTitle;
        clone.summary = source.summary;
        clone.description = source.description;
        clone.pagePath = source.pagePath;
        clone.metaTitle = source.metaTitle;
        clone.metaKeywords = source.metaKeywords;
        clone.metaDescription = source.metaDescription;
        clone.pageName = source.pageName;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        clone.endBody = source.endBody;
        clone.endHead = source.endHead;
        clone.menuTitle = source.menuTitle;
        clone.imgBannerTitle = source.imgBannerTitle;
        return clone;
    }

    #endregion

    public static CONT_VIEW_STP Clone(this CONT_VIEW_STP source)
    {
        try
        {
            CONT_VIEW_STP _return = new CONT_VIEW_STP();
            _return.id = source.id;
            _return.pid_lang = source.pid_lang;
            _return.pid_collection = source.pid_collection;
            _return.page_name = source.page_name;
            _return.title = source.title;
            _return.sub_title = source.sub_title;
            _return.summary = source.summary;
            _return.description = source.description;
            _return.meta_title = source.meta_title;
            _return.meta_keywords = source.meta_keywords;
            _return.meta_description = source.meta_description;
            _return.img_thumb = source.img_thumb;
            _return.img_preview = source.img_preview;
            _return.img_banner = source.img_banner;
            _return.page_rewrite = source.page_rewrite;
            _return.holder_site = source.holder_site;
            _return.css_file = source.css_file;
            _return.page_path = source.page_path;
            return _return;
        }
        catch (Exception)
        {
            return new CONT_VIEW_STP();
        }
    }


}

public class contStpBasePage : mainBasePage
{
    private CONT_VIEW_STP TMPcurrStp;
    public CONT_VIEW_STP currStp
    {
        get
        {
            if (TMPcurrStp == null)
            {
                contProps.CONT_STPs = null; 
                TMPcurrStp = contProps.CONT_STPs.SingleOrDefault(x => x.id == PAGE_REF_ID && x.pid_lang == App.LangID);
            }
            if (TMPcurrStp == null)
            {
                Response.Redirect(App.ERROR_PAGE);
                Response.End();
            }
            return TMPcurrStp ?? new CONT_VIEW_STP();
        }
    }
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (contProps.LangTBL.SingleOrDefault(x => x.id == App.LangID && x.is_active == 1 && x.is_public == 1) == null)
        {
            Response.StatusCode = 301;
            Response.AddHeader("Location", "/");
            Response.End();
            return;
        }
        base.PAGE_TYPE = "stp";
        base.PAGE_REF_ID = 1;
        RewritePath();
    }
}
public class contStpHomeBasePage : mainBasePage
{
    private CONT_VIEW_STP TMPcurrStp;
    public CONT_VIEW_STP currStp
    {
        get
        {
            if (TMPcurrStp == null)
                TMPcurrStp = contProps.CONT_STPs.SingleOrDefault(x => x.id == PAGE_REF_ID && x.pid_lang == App.LangID);
            if (TMPcurrStp == null)
            {
                Response.Redirect(App.ERROR_PAGE);
                Response.End();
            }
            return TMPcurrStp ?? new CONT_VIEW_STP();
        }
    }
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (contProps.LangTBL.SingleOrDefault(x => x.id == App.LangID && x.is_active == 1 && x.is_public == 1) == null)
        {
            Response.StatusCode = 301;
            Response.AddHeader("Location", "/");
            Response.End();
            return;
        }
        //base.PAGE_TYPE = "stp";
        //base.PAGE_REF_ID = 1;
        //RewritePath();
        base.PAGE_TYPE = "stp";
        int id = Request.QueryString["id"].ToInt32();
        if (id != 0)
            base.PAGE_REF_ID = id;
        else
            Response.Redirect(App.ERROR_PAGE);
        RewritePath();
    }
}
