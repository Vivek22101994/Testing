using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModBlog.admin.modBlog
{
    public partial class tagEdit : adminBasePage
    {
        protected dbBlogTagTB currTBL;
        private List<dbBlogTagLN> TMPcurrLangs;
        private List<dbBlogTagLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbBlogTagLN)).Cast<dbBlogTagLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbBlogTagLN>();

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
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "dett")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                HfId.Value = lbl_id.Text;
                fillData();
            }
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodBlog dc = new DCmodBlog())
                {
                    currTBL = dc.dbBlogTagTBs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                        blogProps.TagVIEW = dc.dbBlogTagVIEWs.ToList();
                    }
                }
                closeDetails(false);
            }
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            rwdDett.VisibleOnPageLoad = false;
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        private void fillData()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                currTBL = dc.dbBlogTagTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null) currTBL = new dbBlogTagTB();
                drp_isImportant.setSelectedValue(currTBL.isImportant);

                rwdDett.VisibleOnPageLoad = true;
                currLangs = dc.dbBlogTagLNs.Where(x => x.pidTag == currTBL.id).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            string errorStr = CheckAllLangs(HfId.Value.ToInt32());
            if (errorStr != "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"" + errorStr + "\", 340, 110);", true);
                return;
            }
            using (DCmodBlog dc = new DCmodBlog())
            {
                currTBL = dc.dbBlogTagTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbBlogTagTB();
                    dc.Add(currTBL);
                }
                currTBL.isImportant = drp_isImportant.getSelectedValueInt();

                dc.SaveChanges();
                SaveAllLangs(currTBL.id);
                blogUtils.createTag_pagePath(currTBL);
                AppUtils.FillUrlList();
            }
            closeDetails(true);
        }
        protected void lnkNewTmp_Click(object sender, EventArgs e)
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                var tmpTb = new dbBlogTagTB();
                dc.Add(tmpTb);
                dc.SaveChanges();
                var tmpLn = new dbBlogTagLN();
                tmpLn.pidTag = tmpTb.id;
                tmpLn.pidLang = 1;
                tmpLn.title = txt_newTmp.Text;
                dc.Add(tmpLn);
                dc.SaveChanges();
                tmpLn = new dbBlogTagLN();
                tmpLn.pidTag = tmpTb.id;
                tmpLn.pidLang = 2;
                tmpLn.title = txt_newTmp.Text;
                dc.Add(tmpLn);
                dc.SaveChanges();
                txt_newTmp.Text = "";
                LDS.DataBind();
                LV.SelectedIndex = -1;
                LV.DataBind();
            }
        }
        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "0";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
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
                rlLang = new dbBlogTagLN();
                rlLang.pidTag = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.title = txt_title.Text;
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbBlogTagLN();
            }
            txt_title.Text = rlLang.title;
        }
        protected string CheckAllLangs(long id)
        {
            string  errorStr = "";
            SaveLang();
            using (DCmodBlog dc = new DCmodBlog())
            {
                foreach (var rl in currLangs)
                {
                    var errorLN = dc.dbBlogTagLNs.FirstOrDefault(x => x.pidTag != id && x.pidLang == rl.pidLang && x.title.ToLower().Trim() == rl.title.ToLower().Trim());
                    if (errorLN != null)
                    {
                        if (errorStr == "")
                            errorStr = "Attenzione!";
                        errorStr = "<br/>Il tag '" + errorLN.title.Trim() + "' lang:'" + errorLN.pidLang + "' esiste gia.";
                    }
                }
                return errorStr;
            }
        }

        protected void SaveAllLangs(long id)
        {
            SaveLang();
            using (DCmodBlog dc = new DCmodBlog())
            {
                foreach (var rl in currLangs)
                {
                    if (dc.dbBlogTagLNs.SingleOrDefault(x => x.pidTag == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidTag = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbBlogTagLNs.Single(x => x.pidTag == id && x.pidLang == rl.pidLang);
                        rl.CopyTo(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }


    }

}