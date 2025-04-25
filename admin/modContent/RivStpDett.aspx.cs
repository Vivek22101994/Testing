using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.IO;

namespace ModContent.admin.modContent
{
    public partial class RivStpDett : adminBasePage
    {
        protected dbRivContStpTB currTBL;
        private List<dbRivContStpLN> TMPcurrLangs;
        private List<dbRivContStpLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbRivContStpLN)).Cast<dbRivContStpLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbRivContStpLN>();

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
            string _folder = "images/Stp";
            if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
            re_description.ImageManager.ViewPaths = new string[] { "/" + _folder };
            re_description.ImageManager.UploadPaths = new string[] { "/" + _folder };
            re_description.ImageManager.DeletePaths = new string[] { "/" + _folder };
            using (DCmodContent dc = new DCmodContent())
            {
                                currTBL = dc.dbRivContStpTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    CloseRadWindow("reload");
                    currTBL = new dbRivContStpTB();
                    return;
                }
                else
                {
                    imgPreview.ImgPathDef = "";
                    imgPreview.ImgRoot = _folder;
                    imgPreview.ImgPath = currTBL.imgPreview;

                    imgBanner.ImgPathDef = "";
                    imgBanner.ImgRoot = _folder;
                    imgBanner.ImgPath = currTBL.imgBanner;
                }
                txt_pageName.Text = currTBL.pageName;
                txt_pageRewrite.Text = currTBL.pageRewrite;
                drp_isActive.setSelectedValue(currTBL.isActive);
                txt_endHead.Text = currTBL.endHead;
                txt_endBody.Text = currTBL.endBody;

                currLangs = dc.dbRivContStpLNs.Where(x => x.pidStp == currTBL.id).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            using (DCmodContent dc = new DCmodContent())
            {
                currTBL = dc.dbRivContStpTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRivContStpTB();
                    dc.Add(currTBL);
                }
                currTBL.pageName = txt_pageName.Text;
                currTBL.pageRewrite = txt_pageRewrite.Text;
                currTBL.imgPreview = imgPreview.ImgPath;
                currTBL.imgBanner = imgBanner.ImgPath;
                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.endHead = txt_endHead.Text;
                currTBL.endBody = txt_endBody.Text;

                dc.SaveChanges();
                SaveAllLangs(currTBL.id);

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            RiV_WS.refreshCache("stp");
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
                rlLang = new dbRivContStpLN();
                rlLang.pidStp = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.description = re_description.Content;
            rlLang.menuTitle = txt_menuTitle.Text;
            rlLang.title = txt_title.Text;
            rlLang.subTitle = txt_subTitle.Text;
            rlLang.imgBannerTitle = txt_imgBannerTitle.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.pagePath = txt_url.Text.clearPathName(false);
            rlLang.metaDescription = txt_metaDescription.Text;
            //rlLang.metaKeywords = txt_metaKeywords.Text;
            rlLang.metaTitle = txt_metaTitle.Text;
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRivContStpLN();
            }
            txt_menuTitle.Text = rlLang.menuTitle;
            txt_title.Text = rlLang.title;
            re_description.Content = rlLang.description;
            txt_subTitle.Text = rlLang.subTitle;
            txt_summary.Text = rlLang.summary;
            txt_imgBannerTitle.Text = rlLang.imgBannerTitle;
            txt_url.Text = rlLang.pagePath;
            txt_metaDescription.Text = rlLang.metaDescription;
            //txt_metaKeywords.Text = rlLang.metaKeywords;
            txt_metaTitle.Text = rlLang.metaTitle;
        }
        protected void SaveAllLangs(int id)
        {
            SaveLang();
            using (DCmodContent dc = new DCmodContent())
            {
                foreach (var rl in currLangs)
                {
                    if (dc.dbRivContStpLNs.SingleOrDefault(x => x.pidStp == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidStp = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbRivContStpLNs.Single(x => x.pidStp == id && x.pidLang == rl.pidLang);
                        rl.CopyTo(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }
    }
}

