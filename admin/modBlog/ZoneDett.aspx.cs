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
    public partial class ZoneDett : adminBasePage
    {
        protected LOC_TB_ZONE currTBL;
        private List<dbBlogZoneLN> TMPcurrLangs;
        private List<dbBlogZoneLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbBlogZoneLN)).Cast<dbBlogZoneLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbBlogZoneLN>();

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
                HfId.Value = Request.QueryString["id"].ToInt32().ToString();
                fillData();
            }
        }
        private void fillData()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                string _folder = "files";
                if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                    Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
                _folder = "files/blogZone";
                if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                    Directory.CreateDirectory(Path.Combine(App.SRP, _folder));

                //re_description.ImageManager.ContentProviderTypeName = typeof(Telerik.Web.Examples.DBContentProvider).AssemblyQualifiedName;
                re_description.ImageManager.ViewPaths = new string[] { "/" + _folder };
                re_description.ImageManager.UploadPaths = new string[] { "/" + _folder };
                re_description.ImageManager.DeletePaths = new string[] { "/" + _folder };
                
                currTBL = maga_DataContext.DC_LOCATION.LOC_TB_ZONEs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    CloseRadWindow("");
                    pnlDett.Visible = false;
                    return;
                }
                txt_img_thumb.Text = currTBL.img_thumb;
                currLangs = dc.dbBlogZoneLNs.Where(x => x.pidZone == currTBL.id).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                currTBL = maga_DataContext.DC_LOCATION.LOC_TB_ZONEs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    CloseRadWindow("");
                    pnlDett.Visible = false;
                    return;
                }
                SaveAllLangs(currTBL.id);

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
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
                rlLang = new dbBlogZoneLN();
                rlLang.pidZone = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.description = re_description.Content;
            rlLang.title = txt_title.Text;
            rlLang.subTitle = txt_subTitle.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.pagePath = "blog/zones/" + rlLang.title.clearPathName();
            if (rlLang.pidLang != App.DefLangID)
                rlLang.pagePath = contUtils.getLang_folder(rlLang.pidLang) + "/" + rlLang.pagePath;
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
                rlLang = new dbBlogZoneLN();
            }
            txt_title.Text = !string.IsNullOrEmpty(rlLang.title) ? rlLang.title : txt_img_thumb.Text;
            re_description.Content = rlLang.description;
            txt_subTitle.Text = rlLang.subTitle;
            txt_summary.Text = rlLang.summary;
            txt_url.Text = rlLang.pagePath;
            txt_metaDescription.Text = rlLang.metaDescription;
            txt_metaKeywords.Text = rlLang.metaKeywords;
            txt_metaTitle.Text = rlLang.metaTitle;
        }
        protected void SaveAllLangs(int id)
        {
            SaveLang();
            using (DCmodBlog dc = new DCmodBlog())
            {
                foreach (var rl in currLangs)
                {
                    if (dc.dbBlogZoneLNs.SingleOrDefault(x => x.pidZone == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidZone = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbBlogZoneLNs.Single(x => x.pidZone == id && x.pidLang == rl.pidLang);
                        rl.CopyTo(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }

    }
}

