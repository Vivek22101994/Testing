using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ModBlog;

public class blogUtils
{
    public static string getTag_title(long id, int pidLang, string def)
    {
        dbBlogTagVIEW tmp = blogProps.TagVIEW.FirstOrDefault(x => x.id == id && x.pidLang == pidLang);
        if (tmp == null || string.IsNullOrEmpty(tmp.title)) tmp = blogProps.TagVIEW.FirstOrDefault(x => x.id == id && x.pidLang == 2);
        if (tmp == null || string.IsNullOrEmpty(tmp.title)) tmp = blogProps.TagVIEW.FirstOrDefault(x => x.id == id && x.pidLang == 1);
        if (tmp != null && !string.IsNullOrEmpty(tmp.title)) return tmp.title;
        return def;
    }
    public static string getCategory_title(long id, int pidLang, string def)
    {
        dbBlogCategoryVIEW tmp = blogProps.CategoryVIEW.FirstOrDefault(x => x.id == id && x.pidLang == pidLang);
        if (tmp == null || string.IsNullOrEmpty(tmp.title)) tmp = blogProps.CategoryVIEW.FirstOrDefault(x => x.id == id && x.pidLang == 2);
        if (tmp == null || string.IsNullOrEmpty(tmp.title)) tmp = blogProps.CategoryVIEW.FirstOrDefault(x => x.id == id && x.pidLang == 1);
        if (tmp != null && !string.IsNullOrEmpty(tmp.title)) return tmp.title;
        return def;
    }
    public static string getArticle_title(long id, int pidLang, string def)
    {
        dbBlogArticleVIEW tmp = blogProps.ArticleVIEW.FirstOrDefault(x => x.id == id && x.pidLang == pidLang);
        if (tmp == null || string.IsNullOrEmpty(tmp.title)) tmp = blogProps.ArticleVIEW.FirstOrDefault(x => x.id == id && x.pidLang == 2);
        if (tmp == null || string.IsNullOrEmpty(tmp.title)) tmp = blogProps.ArticleVIEW.FirstOrDefault(x => x.id == id && x.pidLang == 1);
        if (tmp != null && !string.IsNullOrEmpty(tmp.title)) return tmp.title;
        return def;
    }
    public static string getArticle_pagePath(long id, int pidLang)
    {
        dbBlogArticleVIEW tmp = blogProps.ArticleVIEW.FirstOrDefault(x => x.id == id && x.pidLang == pidLang);
        //if (tmp == null || string.IsNullOrEmpty(tmp.pagePath)) tmp = blogProps.ArticleVIEW.FirstOrDefault(x => x.id == id && x.pidLang == 2);
        //if (tmp == null || string.IsNullOrEmpty(tmp.pagePath)) tmp = blogProps.ArticleVIEW.FirstOrDefault(x => x.id == id && x.pidLang == 1);
        if (tmp != null && !string.IsNullOrEmpty(tmp.pagePath)) return App.RP + tmp.pagePath;
        return "";
    }
    public static void createArticle_pagePath(dbBlogArticleTB currTB)
    {
        using (DCmodBlog dc = new DCmodBlog())
        {
            List<dbBlogArticleLN> lnList = dc.dbBlogArticleLNs.Where(x => x.pidArticle == currTB.id).ToList();
            foreach (dbBlogArticleLN currLN in lnList)
            {
                if (string.IsNullOrEmpty(currLN.title)) { currLN.pagePath = ""; continue; }
                string path =  currLN.title.clearPathName();
                string folder = getCategory_title(currTB.pidCategory.objToInt64(), currLN.pidLang, "").clearPathName();
                if (folder != "")
                    path = folder + "/" + path;
                path = "blog/" + path;
                string langFolder = contUtils.getLang_folder(currLN.pidLang);
                if (langFolder != "en" && langFolder != "")
                    path = langFolder + "/" + path;
                currLN.pagePath = path;
            }
            dc.SaveChanges();
            blogProps.ArticleVIEW = dc.dbBlogArticleVIEWs.ToList();
        }
    }
    public static string getTagCloud(int pidLang, string itemTemplate)
    {
        string tagListString = "";
        var tagList = blogProps.TagVIEW.Where(x => x.pidLang == pidLang && !string.IsNullOrEmpty(x.pagePath)).OrderBy(x => x.title);
        var articleIds = blogProps.ArticleVIEW.Where(x => x.isActive == 1 && x.pidLang == pidLang && !string.IsNullOrEmpty(x.pagePath) && !string.IsNullOrEmpty(x.title)).Select(x=>x.id);

        foreach (var tmpTag in tagList)
        {
            var tagRL = blogProps.ArticleTagRL.Where(x => x.pidTag == tmpTag.id && articleIds.Contains(x.pidArticle)).Select(x => x.pidArticle).ToList();
            if (tagRL.Count > 0)
            {
                string tmpStr = itemTemplate;
                tmpStr = tmpStr.Replace("#pagePath#", "" + tmpTag.pagePath);
                tmpStr = tmpStr.Replace("#title#", "" + tmpTag.title);
                tmpStr = tmpStr.Replace("#count#", "" + tagRL.Count);
                string cssClass = "";
                if (tagRL.Count<=2)
                    cssClass = "tagNum_2";
                else if (tagRL.Count <= 4)
                    cssClass = "tagNum_4";
                else if (tagRL.Count <= 6)
                    cssClass = "tagNum_6";
                else if (tagRL.Count <= 8)
                    cssClass = "tagNum_8";
                else if (tagRL.Count <= 10)
                    cssClass = "tagNum_10";
                else if (tagRL.Count <= 15)
                    cssClass = "tagNum_15";
                else if (tagRL.Count <= 20)
                    cssClass = "tagNum_20";
                else if (tagRL.Count <= 25)
                    cssClass = "tagNum_25";
                else if (tagRL.Count <= 30)
                    cssClass = "tagNum_30";
                else if (tagRL.Count <= 35)
                    cssClass = "tagNum_35";
                else if (tagRL.Count <= 40)
                    cssClass = "tagNum_40";
                else if (tagRL.Count <= 45)
                    cssClass = "tagNum_45";
                else if (tagRL.Count <= 50)
                    cssClass = "tagNum_50";
                else
                    cssClass = "tagNum_51more";
                tmpStr = tmpStr.Replace("#cssClass#", "" + cssClass);
                tagListString += tmpStr;
            }
        }
        return tagListString;
    }
    public static void fillArticle_rewriteTool(ref UrlList uList, List<int> langIDs)
    {
        List<dbBlogArticleVIEW> tmpList = blogProps.ArticleVIEW.Where(x => langIDs.Contains(x.pidLang) && !string.IsNullOrEmpty(x.pagePath) && x.isActive == 1).ToList();
        foreach (dbBlogArticleVIEW tmp in tmpList)
        {
            UrlItem item = new UrlItem("stp", App.RP + tmp.pagePath, tmp.pageRewrite + "id=" + tmp.id + "&lang=" + tmp.pidLang, "" + tmp.pidLang);
            uList.Items.Add(item);
        }
    }
    public static void createTag_pagePath(dbBlogTagTB currTB)
    {
        using (DCmodBlog dc = new DCmodBlog())
        {
            List<dbBlogTagLN> lnList = dc.dbBlogTagLNs.Where(x => x.pidTag == currTB.id).ToList();
            foreach (dbBlogTagLN currLN in lnList)
            {
                if (string.IsNullOrEmpty(currLN.title)) { currLN.pagePath = ""; continue; }
                string path = currTB.id + "/" + contUtils.getLabel_title("urlStringaAggiuntivaDeiTag", currLN.pidLang, "about").clearPathName() + "-" + currLN.title.clearPathName();
                path = "blog/" + path;
                string langFolder = contUtils.getLang_folder(currLN.pidLang);
                if (langFolder != "en" && langFolder != "")
                    path = langFolder + "/" + path;
                currLN.pagePath = path;
            }
            dc.SaveChanges();
            blogProps.TagVIEW = dc.dbBlogTagVIEWs.ToList();
        }
    }
    public static void fillTag_rewriteTool(ref UrlList uList, List<int> langIDs)
    {
        List<dbBlogTagVIEW> tmpList = blogProps.TagVIEW.Where(x => langIDs.Contains(x.pidLang) && !string.IsNullOrEmpty(x.pagePath)).ToList();
        foreach (dbBlogTagVIEW tmp in tmpList)
        {
            UrlItem item = new UrlItem("stp", App.RP + tmp.pagePath, "stp_blogHomePage.aspx?id=11&tag=" + tmp.id + "&lang=" + tmp.pidLang, "" + tmp.pidLang);
            uList.Items.Add(item);
        }
    }
}
public class blogProps
{
    private static List<dbBlogArticleVIEW> tmpArticleVIEW; // refresh Auto
    public static List<dbBlogArticleVIEW> ArticleVIEW
    {
        get
        {
            if (tmpArticleVIEW == null)
            {
                using (DCmodBlog dc = new DCmodBlog())
                    tmpArticleVIEW = dc.dbBlogArticleVIEWs.ToList();
            }
            return new List<dbBlogArticleVIEW>(tmpArticleVIEW.Select(x => x.Clone()));
        }
        set { tmpArticleVIEW = value; }
    }
    private static List<dbBlogArticleCategoryRL> tmpArticleCategoryRL; // refresh Auto
    public static List<dbBlogArticleCategoryRL> ArticleCategoryRL
    {
        get
        {
            if (tmpArticleCategoryRL == null)
            {
                using (DCmodBlog dc = new DCmodBlog())
                    tmpArticleCategoryRL = dc.dbBlogArticleCategoryRLs.ToList();
            }
            return new List<dbBlogArticleCategoryRL>(tmpArticleCategoryRL.Select(x => x.Clone()));
        }
        set { tmpArticleCategoryRL = value; }
    }
    private static List<dbBlogArticleTagRL> tmpArticleTagRL; // refresh Auto
    public static List<dbBlogArticleTagRL> ArticleTagRL
    {
        get
        {
            if (tmpArticleTagRL == null)
            {
                using (DCmodBlog dc = new DCmodBlog())
                    tmpArticleTagRL = dc.dbBlogArticleTagRLs.ToList();
            }
            return new List<dbBlogArticleTagRL>(tmpArticleTagRL.Select(x => x.Clone()));
        }
        set { tmpArticleTagRL = value; }
    }
    private static List<dbBlogTagVIEW> tmpTagVIEW; // refresh Auto
    public static List<dbBlogTagVIEW> TagVIEW
    {
        get
        {
            if (tmpTagVIEW == null)
            {
                using (DCmodBlog dc = new DCmodBlog())
                    tmpTagVIEW = dc.dbBlogTagVIEWs.ToList();
            }
            return new List<dbBlogTagVIEW>(tmpTagVIEW.Select(x => x.Clone()));
        }
        set { tmpTagVIEW = value; }
    }
    private static List<dbBlogCategoryVIEW> tmpCategoryVIEW; // refresh Auto
    public static List<dbBlogCategoryVIEW> CategoryVIEW
    {
        get
        {
            if (tmpCategoryVIEW == null)
            {
                using (DCmodBlog dc = new DCmodBlog())
                    tmpCategoryVIEW = dc.dbBlogCategoryVIEWs.ToList();
            }
            return new List<dbBlogCategoryVIEW>(tmpCategoryVIEW.Select(x => x.Clone()));
        }
        set { tmpCategoryVIEW = value; }
    }
    private static List<dbBlogCommentTBL> tmpCommentTBL; // refresh Auto
    public static List<dbBlogCommentTBL> CommentTBL
    {
        get
        {
            if (tmpCommentTBL == null)
            {
                using (DCmodBlog dc = new DCmodBlog())
                    tmpCommentTBL = dc.dbBlogCommentTBLs.ToList();
            }
            return new List<dbBlogCommentTBL>(tmpCommentTBL.Select(x => x.Clone()));
        }
        set { tmpCommentTBL = value; }
    }

}
public static class blogExts
{
    public static void CopyTo(this dbBlogZoneLN source, ref dbBlogZoneLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.summary = source.summary;
        copyto.description = source.description;
        copyto.pagePath = source.pagePath;
        copyto.metaTitle = source.metaTitle;
        copyto.metaKeywords = source.metaKeywords;
        copyto.metaDescription = source.metaDescription;
    }
    public static dbBlogArticleTB Clone(this dbBlogArticleTB source)
    {
        dbBlogArticleTB clone = new dbBlogArticleTB();
        clone.id = source.id;
        clone.pidParentArticle = source.pidParentArticle;
        clone.pidCategory = source.pidCategory;
        clone.pidZone = source.pidZone;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        clone.publicDate = source.publicDate;
        clone.createdDate = source.createdDate;
        clone.createdUserID = source.createdUserID;
        clone.createdUserNameFull = source.createdUserNameFull;
        return clone;
    }
    public static void CopyTo(this dbBlogArticleTB source, ref dbBlogArticleTB copyto)
    {
        copyto.pidParentArticle = source.pidParentArticle;
        copyto.pidCategory = source.pidCategory;
        copyto.imgThumb = source.imgThumb;
        copyto.imgPreview = source.imgPreview;
        copyto.imgBanner = source.imgBanner;
        copyto.cssFile = source.cssFile;
        copyto.pageRewrite = source.pageRewrite;
        copyto.isActive = source.isActive;
        copyto.publicDate = source.publicDate;
        copyto.createdDate = source.createdDate;
        copyto.createdUserID = source.createdUserID;
        copyto.createdUserNameFull = source.createdUserNameFull;
    }
    public static dbBlogArticleLN Clone(this dbBlogArticleLN source)
    {
        dbBlogArticleLN clone = new dbBlogArticleLN();
        clone.pidArticle = source.pidArticle;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        clone.subTitle = source.subTitle;
        clone.summary = source.summary;
        clone.description = source.description;
        clone.pagePath = source.pagePath;
        clone.metaTitle = source.metaTitle;
        clone.metaKeywords = source.metaKeywords;
        clone.metaDescription = source.metaDescription;
        return clone;
    }
    public static void CopyTo(this dbBlogArticleLN source, ref dbBlogArticleLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.summary = source.summary;
        copyto.description = source.description;
        copyto.pagePath = source.pagePath;
        copyto.metaTitle = source.metaTitle;
        copyto.metaKeywords = source.metaKeywords;
        copyto.metaDescription = source.metaDescription;
    }
    public static dbBlogArticleVIEW Clone(this dbBlogArticleVIEW source)
    {
        dbBlogArticleVIEW clone = new dbBlogArticleVIEW();
        clone.id = source.id;
        clone.pidLang = source.pidLang;
        clone.pidZone = source.pidZone;
        clone.title = source.title;
        clone.subTitle = source.subTitle;
        clone.summary = source.summary;
        clone.description = source.description;
        clone.pagePath = source.pagePath;
        clone.metaTitle = source.metaTitle;
        clone.metaKeywords = source.metaKeywords;
        clone.metaDescription = source.metaDescription;
        clone.pidParentArticle = source.pidParentArticle;
        clone.pidCategory = source.pidCategory;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        clone.publicDate = source.publicDate;
        clone.createdDate = source.createdDate;
        clone.createdUserID = source.createdUserID;
        clone.createdUserNameFull = source.createdUserNameFull;
        return clone;
    }
    public static dbBlogArticleCategoryRL Clone(this dbBlogArticleCategoryRL source)
    {
        dbBlogArticleCategoryRL clone = new dbBlogArticleCategoryRL();
        clone.pidArticle = source.pidArticle;
        clone.pidCategory = source.pidCategory;
        clone.sequence = source.sequence;
        return clone;
    }
    public static dbBlogArticleTagRL Clone(this dbBlogArticleTagRL source)
    {
        dbBlogArticleTagRL clone = new dbBlogArticleTagRL();
        clone.pidArticle = source.pidArticle;
        clone.pidTag = source.pidTag;
        clone.sequence = source.sequence;
        return clone;
    }
    public static void CopyTo(this dbBlogTagLN source, ref dbBlogTagLN copyto)
    {
        copyto.title = source.title;
        copyto.pagePath = source.pagePath;
    }
    public static dbBlogTagVIEW Clone(this dbBlogTagVIEW source)
    {
        dbBlogTagVIEW clone = new dbBlogTagVIEW();
        clone.id = source.id;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        clone.isImportant = source.isImportant;
        clone.pagePath = source.pagePath;
        return clone;
    }

    public static dbBlogCategoryTB Clone(this dbBlogCategoryTB source)
    {
        dbBlogCategoryTB clone = new dbBlogCategoryTB();
        clone.id = source.id;
        clone.pidParentCategory = source.pidParentCategory;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        return clone;
    }
    public static void CopyTo(this dbBlogCategoryTB source, ref dbBlogCategoryTB copyto)
    {
        copyto.pidParentCategory = source.pidParentCategory;
        copyto.imgThumb = source.imgThumb;
        copyto.imgPreview = source.imgPreview;
        copyto.imgBanner = source.imgBanner;
        copyto.cssFile = source.cssFile;
        copyto.pageRewrite = source.pageRewrite;
        copyto.isActive = source.isActive;
    }
    public static dbBlogCategoryLN Clone(this dbBlogCategoryLN source)
    {
        dbBlogCategoryLN clone = new dbBlogCategoryLN();
        clone.pidCategory = source.pidCategory;
        clone.pidLang = source.pidLang;
        clone.title = source.title;
        clone.subTitle = source.subTitle;
        clone.summary = source.summary;
        clone.description = source.description;
        clone.pagePath = source.pagePath;
        clone.metaTitle = source.metaTitle;
        clone.metaKeywords = source.metaKeywords;
        clone.metaDescription = source.metaDescription;
        return clone;
    }
    public static void CopyTo(this dbBlogCategoryLN source, ref dbBlogCategoryLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.summary = source.summary;
        copyto.description = source.description;
        copyto.pagePath = source.pagePath;
        copyto.metaTitle = source.metaTitle;
        copyto.metaKeywords = source.metaKeywords;
        copyto.metaDescription = source.metaDescription;
    }
    public static dbBlogCategoryVIEW Clone(this dbBlogCategoryVIEW source)
    {
        dbBlogCategoryVIEW clone = new dbBlogCategoryVIEW();
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
        clone.pidParentCategory = source.pidParentCategory;
        clone.imgThumb = source.imgThumb;
        clone.imgPreview = source.imgPreview;
        clone.imgBanner = source.imgBanner;
        clone.cssFile = source.cssFile;
        clone.pageRewrite = source.pageRewrite;
        clone.isActive = source.isActive;
        return clone;
    }
    public static dbBlogCommentTBL Clone(this dbBlogCommentTBL source)
    {
        dbBlogCommentTBL clone = new dbBlogCommentTBL();
        clone.id = source.id;
        clone.uid = source.uid;
        clone.pidArticle = source.pidArticle;
        clone.typeCode = source.typeCode;
        clone.typeTitle = source.typeTitle;
        clone.pidLang = source.pidLang;
        clone.nameFull = source.nameFull;
        clone.nameFirst = source.nameFirst;
        clone.nameLast = source.nameLast;
        clone.country = source.country;
        clone.email = source.email;
        clone.phone = source.phone;
        clone.phoneMobile = source.phoneMobile;
        clone.fax = source.fax;
        clone.commentVote = source.commentVote;
        clone.commentSubject = source.commentSubject;
        clone.commentBody = source.commentBody;
        clone.commentDate = source.commentDate;
        clone.createdDate = source.createdDate;
        clone.createdUserID = source.createdUserID;
        clone.createdUserNameFull = source.createdUserNameFull;
        clone.isActive = source.isActive;
        return clone;
    }
}
public class blogArticleBasePage : mainBasePage
{
    private dbBlogArticleVIEW TMPcurrArticle;
    public dbBlogArticleVIEW currArticle
    {
        get
        {
            if (TMPcurrArticle == null)
                TMPcurrArticle = blogProps.ArticleVIEW.SingleOrDefault(x => x.id == PAGE_REF_ID && x.pidLang == App.LangID);
            if (TMPcurrArticle == null)
            {
                Response.Redirect(App.ERROR_PAGE);
                Response.End();
            }
            return TMPcurrArticle ?? new dbBlogArticleVIEW();
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
        base.PAGE_TYPE = "pg_blogArticle";
        int id = Request.QueryString["id"].ToInt32();
        if (id != 0)
            base.PAGE_REF_ID = id;
        else
            Response.Redirect(App.ERROR_PAGE);
        RewritePath();
    }
}
