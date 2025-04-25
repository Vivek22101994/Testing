using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModBlog.admin.modBlog
{
    public partial class categoryDett : adminBasePage
    {
        protected dbBlogCategoryTB currTBL;
        private List<dbBlogCategoryLN> TMPcurrLangs;
        private List<dbBlogCategoryLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbBlogCategoryLN)).Cast<dbBlogCategoryLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbBlogCategoryLN>();

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
                fillData();
            }
        }
        private void fillData()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                currTBL = dc.dbBlogCategoryTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbBlogCategoryTB();
                    ltrTitle.Text = "Nuova Categoria d'Articoli";
                }
                else
                {
                    ltrTitle.Text = "Categoria d'Articoli #:" + currTBL.id;
                    string _folder = "files";
                    if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                        Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
                    _folder = "files/blogCategory";
                    if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                        Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
                    if (!Directory.Exists(Path.Combine(App.SRP, _folder + "/" + currTBL.id)))
                        Directory.CreateDirectory(Path.Combine(App.SRP, _folder + "/" + currTBL.id));
                }
                drp_isActive.setSelectedValue(currTBL.isActive);

                currLangs = dc.dbBlogCategoryLNs.Where(x => x.pidCategory == currTBL.id).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                currTBL = dc.dbBlogCategoryTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbBlogCategoryTB();
                    dc.Add(currTBL);
                    dc.SaveChanges();
                    HfId.Value = currTBL.id.ToString();
                }
                currTBL.pageRewrite = txt_pageRewrite.Text;
                currTBL.isActive = drp_isActive.getSelectedValueInt(0);

                dc.SaveChanges();
                SaveAllLangs(currTBL.id);

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            using (DCmodBlog dc = new DCmodBlog())
                blogProps.CategoryVIEW = dc.dbBlogCategoryVIEWs.ToList();
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
                rlLang = new dbBlogCategoryLN();
                rlLang.pidCategory = HfId.Value.ToInt32();
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
                rlLang = new dbBlogCategoryLN();
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
                    if (dc.dbBlogCategoryLNs.SingleOrDefault(x => x.pidCategory == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidCategory = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbBlogCategoryLNs.Single(x => x.pidCategory == id && x.pidLang == rl.pidLang);
                        rl.CopyTo(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }
    }
}

