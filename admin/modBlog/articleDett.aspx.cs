using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using RentalInRome.data;

namespace ModBlog.admin.modBlog
{
    public partial class articleDett : adminBasePage
    {
        protected dbBlogArticleTB currTBL;
        private List<dbBlogArticleLN> TMPcurrLangs;
        private List<dbBlogArticleLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbBlogArticleLN)).Cast<dbBlogArticleLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbBlogArticleLN>();

                return TMPcurrLangs;
            }
            set
            {
                ViewState["currLangs"] = PConv.SerialList(value.Cast<object>().ToList());
                TMPcurrLangs = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HfId.Value = Request.QueryString["id"].ToInt64().ToString();
                drp_createdUserID_DataBind();
                chkList_ArticleCategoryRL_DataBind();
                chkList_ArticleTagRL_DataBind();
                drp_pidZone_DataBind();
                drp_pidCategory_DataBind();
                fillData();
            }
        }
        protected void drp_createdUserID_DataBind()
        {
            drp_createdUserID.Items.Clear();
            var tmpList = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_deleted != 1 && x.is_active == 1 && x.id > 2).OrderBy(x => x.name).ToList();
            foreach (var tmp in tmpList)
            {
                drp_createdUserID.Items.Add(new ListItem(tmp.name + " " + tmp.surname, tmp.id.ToString()));
            }
        }
        protected void drp_pidCategory_DataBind()
        {
            drp_pidCategory.Items.Clear();
            List<dbBlogCategoryVIEW> tmpList = blogProps.CategoryVIEW.Where(x => x.isActive == 1 && x.pidLang == 1).OrderBy(x => x.title).ToList();
            foreach (dbBlogCategoryVIEW tmp in tmpList)
            {
                drp_pidCategory.Items.Add(new ListItem(tmp.title, tmp.id.ToString()));
            }
        }
        protected void chkList_ArticleCategoryRL_DataBind()
        {
            chkList_ArticleCategoryRL.Items.Clear();
            List<dbBlogCategoryVIEW> tmpList = blogProps.CategoryVIEW.Where(x => x.pidLang == 1).OrderBy(x => x.title).ToList();
            foreach (dbBlogCategoryVIEW tmp in tmpList)
            {
                chkList_ArticleCategoryRL.Items.Add(new ListItem(tmp.title, tmp.id.ToString()));
            }
        }
        protected void chkList_ArticleTagRL_DataBind()
        {
            chkList_ArticleTagRL.Items.Clear();
            List<dbBlogTagVIEW> tmpList = blogProps.TagVIEW.Where(x => x.pidLang == 1).OrderBy(x => x.title).ToList();
            foreach (dbBlogTagVIEW tmp in tmpList)
            {
                chkList_ArticleTagRL.Items.Add(new ListItem(tmp.title, tmp.id.ToString()));
            }
        }
        protected void drp_pidZone_DataBind()
        {
            drp_pidZone.DataSource = maga_DataContext.DC_LOCATION.LOC_TB_ZONEs.Where(x => x.pid_city == 1 && x.is_active == 1).ToList();
            drp_pidZone.DataTextField = "img_thumb";
            drp_pidZone.DataValueField = "id";
            drp_pidZone.DataBind();
            drp_pidZone.Items.Insert(0, new ListItem("-seleziona-", ""));
        }
        private void fillData()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                string _folder = "files";
                if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                    Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
                _folder = "files/blogArticle";
                if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                    Directory.CreateDirectory(Path.Combine(App.SRP, _folder));

                //re_description.ImageManager.ContentProviderTypeName = typeof(Telerik.Web.Examples.DBContentProvider).AssemblyQualifiedName;
                re_description.ImageManager.ViewPaths = new string[] { "/" + _folder };
                re_description.ImageManager.UploadPaths = new string[] { "/" + _folder };
                re_description.ImageManager.DeletePaths = new string[] { "/" + _folder };
                
                currTBL = dc.dbBlogArticleTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbBlogArticleTB();
                    ltrTitle.Text = "Nuovo Articolo nel Blog";
                    currTBL.publicDate = DateTime.Now;
                    drp_createdUserID.setSelectedValue(UserAuthentication.CurrentUserID);
                }
                else
                {
                    ltrTitle.Text = "Articolo nel Blog #" + currTBL.id;

                    imgPreview.ImgPathDef = "";
                    imgPreview.ImgRoot = _folder;
                    imgPreview.ImgPath = currTBL.imgPreview;

                    imgBanner.ImgPathDef = "";
                    imgBanner.ImgRoot = _folder;
                    imgBanner.ImgPath = currTBL.imgBanner;
                    pnlImg.Visible = true;
                    drp_createdUserID.setSelectedValue(currTBL.createdUserID);
                }
                pnlImg.Visible = false;
                drp_isActive.setSelectedValue(currTBL.isActive);
                drp_pidCategory.setSelectedValue(currTBL.pidCategory.ToString());
                rdp_publicDate.SelectedDate = currTBL.publicDate;
                drp_pidZone.setSelectedValue(currTBL.pidZone);
                List<long> tagList = dc.dbBlogArticleTagRLs.Where(x => x.pidArticle == currTBL.id).Select(x => x.pidTag).ToList();
                foreach (ListItem tmpItem in chkList_ArticleTagRL.Items)
                {
                    tmpItem.Selected = tagList.Contains(tmpItem.Value.ToInt64());
                }
                List<long> catList = dc.dbBlogArticleCategoryRLs.Where(x => x.pidArticle == currTBL.id).Select(x => x.pidCategory).ToList();
                foreach (ListItem tmpItem in chkList_ArticleCategoryRL.Items)
                {
                    tmpItem.Selected = catList.Contains(tmpItem.Value.ToInt64());
                }

                currLangs = dc.dbBlogArticleLNs.Where(x => x.pidArticle == currTBL.id).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                currTBL = dc.dbBlogArticleTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbBlogArticleTB();
                    currTBL.createdDate = DateTime.Now;
                    dc.Add(currTBL);
                    dc.SaveChanges();
                    HfId.Value = currTBL.id.ToString();
                }
                else
                {
                    currTBL.imgPreview = imgPreview.ImgPath;
                    currTBL.imgBanner = imgBanner.ImgPath;
                }
                currTBL.createdUserID = drp_createdUserID.getSelectedValueInt();
                currTBL.createdUserNameFull = drp_createdUserID.getSelectedText("");
                
                currTBL.pageRewrite = txt_pageRewrite.Text;
                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.pidCategory = drp_pidCategory.SelectedValue.objToInt64();
                currTBL.publicDate = rdp_publicDate.SelectedDate;
                currTBL.pidZone = drp_pidZone.getSelectedValueInt();
                dc.Delete(dc.dbBlogArticleTagRLs.Where(x => x.pidArticle == currTBL.id));
                dc.SaveChanges();
                int sequence = 1;
                foreach (ListItem tmpItem in chkList_ArticleTagRL.Items)
                {
                    if (tmpItem.Selected)
                    {
                        dbBlogArticleTagRL tmp = new dbBlogArticleTagRL();
                        tmp.pidArticle = currTBL.id;
                        tmp.pidTag = tmpItem.Value.objToInt64();
                        tmp.sequence = sequence;
                        dc.Add(tmp);
                        sequence++;
                    }
                }
                dc.SaveChanges();

                dc.Delete(dc.dbBlogArticleCategoryRLs.Where(x => x.pidArticle == currTBL.id));
                dc.SaveChanges();
                sequence = 1;
                foreach (ListItem tmpItem in chkList_ArticleCategoryRL.Items)
                {
                    if (tmpItem.Selected)
                    {
                        dbBlogArticleCategoryRL tmp = new dbBlogArticleCategoryRL();
                        tmp.pidArticle = currTBL.id;
                        tmp.pidCategory = tmpItem.Value.objToInt64();
                        tmp.sequence = sequence;
                        dc.Add(tmp);
                        sequence++;
                    }
                }
                dc.SaveChanges();

                SaveAllLangs(currTBL.id);

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            using (DCmodBlog dc = new DCmodBlog())
            {
                blogProps.ArticleVIEW = dc.dbBlogArticleVIEWs.ToList();
                blogProps.ArticleTagRL = dc.dbBlogArticleTagRLs.ToList();
                blogProps.ArticleCategoryRL = dc.dbBlogArticleCategoryRLs.ToList();
            }
            blogUtils.createArticle_pagePath(currTBL);
            AppUtils.FillUrlList();
            fillData();
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "0";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected void BindLvLangs()
        {
            LvLangs.DataSource = contProps.LangTBL.Where(x => x.is_active == 1).OrderBy(x => x.id);
            LvLangs.DataBind();
        }
        protected void LvLangs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnkLang");
            lnk.CssClass = HfLang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
        }
        protected void LvLangs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                SaveLang();
                HfLang.Value = lbl_id.Text;
                FillLang();
                BindLvLangs();
            }
        }
        protected void SaveLang()
        {
            var currLangsTmp = currLangs;
            var rlLang = currLangsTmp.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbBlogArticleLN();
                rlLang.pidArticle = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.description = re_description.Content;
            rlLang.title = txt_title.Text;
            rlLang.subTitle = txt_subTitle.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.pagePath = txt_url.Text;
            rlLang.metaDescription = txt_metaDescription.Text;
            rlLang.metaKeywords = txt_metaKeywords.Text;
            rlLang.metaTitle = txt_metaTitle.Text;
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbBlogArticleLN();
            }
            txt_title.Text = rlLang.title;
            re_description.Content = rlLang.description;
            txt_subTitle.Text = rlLang.subTitle;
            txt_summary.Text = rlLang.summary;
            txt_url.Text = rlLang.pagePath;
            txt_metaDescription.Text = rlLang.metaDescription;
            txt_metaKeywords.Text = rlLang.metaKeywords;
            txt_metaTitle.Text = rlLang.metaTitle;
        }
        protected void SaveAllLangs(long id)
        {
            SaveLang();
            using (DCmodBlog dc = new DCmodBlog())
            {
                foreach (var rl in currLangs)
                {
                    if (dc.dbBlogArticleLNs.SingleOrDefault(x => x.pidArticle == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidArticle = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbBlogArticleLNs.Single(x => x.pidArticle == id && x.pidLang == rl.pidLang);
                        rl.CopyTo(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }

    }
}

