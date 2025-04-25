using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using System.IO;

namespace RentalInRome.admin.modRental
{
    public partial class EstateExtraSubCategoryDett : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "extras";
        }
        protected dbRntExtrasSubCategoryTB currTBL;
        private List<dbRntExtrasSubCategoryLN> TMPcurrLangs;
        private List<dbRntExtrasSubCategoryLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbRntExtrasSubCategoryLN)).Cast<dbRntExtrasSubCategoryLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbRntExtrasSubCategoryLN>();

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
                HfCat.Value = Request.QueryString["category"].ToInt64().ToString();
                if (HfCat.Value != "0")
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        dbRntExtrasCategoryTB currTBLCategory = dc.dbRntExtrasCategoryTBs.SingleOrDefault(x => x.id == HfCat.Value.ToInt32());
                        lbl_title.Text = "Categoria:" + " " + currTBLCategory.code;
                    }
                }
                
                fillData();
            }
        }
        private void fillData()
        {
            string _folder = "images/ExtraSubCategory";
            if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntExtrasSubCategoryTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRntExtrasSubCategoryTB();
                    ltrTitle.Text = "Inserimento nuovo accessorio";
                }
                else
                {
                    ltrTitle.Text = "Modifca accessorio #" + currTBL.id;
                }
                Bind_drp_category();
                if (HfId.Value == "0")
                {
                    drp_category.SelectedValue = HfCat.Value;
                }
                else
                {
                    drp_category.SelectedValue = Convert.ToString(currTBL.pidCategory);
                }
                txt_code.Text = currTBL.code ?? "";
                drp_isActive.setSelectedValue(currTBL.isActive);

                imgPreview.ImgPathDef = "";
                imgPreview.ImgRoot = _folder;
                imgPreview.ImgPath = currTBL.imgThumb;
                currLangs = dc.dbRntExtrasSubCategoryLNs.Where(x => x.pidSubCategory == currTBL.id).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            if (drp_category.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"è richiesto categoria.\", 340, 110);", true);
            }
            else
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntExtrasSubCategoryTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                    if (currTBL == null)
                    {
                        currTBL = new dbRntExtrasSubCategoryTB();
                        dc.Add(currTBL);
                    }
                    var tmpTbl = new dbRntExtrasSubCategoryTB();
                    currTBL.code = txt_code.Text;
                    currTBL.isActive = drp_isActive.getSelectedValueInt();
                    currTBL.imgThumb = imgPreview.ImgPath;
                    currTBL.pidCategory = Convert.ToInt32(drp_category.SelectedValue);
                    dc.SaveChanges();
                    SaveAllLangs(currTBL.id);
                    HfId.Value = Convert.ToString(currTBL.id);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                }
                fillData();
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
                rlLang = new dbRntExtrasSubCategoryLN();
                rlLang.pidSubCategory = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.title = txt_title.Text;
            rlLang.subTitle = txt_sub_title.Text;
            rlLang.description = txt_description.Content;
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRntExtrasSubCategoryLN();
            }
            txt_title.Text = rlLang.title;
            txt_sub_title.Text = rlLang.subTitle;
            txt_description.Content = rlLang.description;
        }
        protected void SaveAllLangs(int id)
        {
            SaveLang();
            using (DCmodRental dc = new DCmodRental())
            {
                foreach (var rl in currLangs)
                {
                    if (dc.dbRntExtrasSubCategoryLNs.SingleOrDefault(x => x.pidSubCategory == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidSubCategory = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbRntExtrasSubCategoryLNs.Single(x => x.pidSubCategory == id && x.pidLang == rl.pidLang);
                        rl.CopyToSubCategory(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }

        private void Bind_drp_category()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntExtrasCategoryTB> catList = dc.dbRntExtrasCategoryTBs.Where(x => x.isActive == 1).ToList();
                drp_category.Items.Clear();
                foreach (dbRntExtrasCategoryTB cat in catList)
                {
                    drp_category.Items.Add(new ListItem(cat.code, "" + cat.id));
                }
                drp_category.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
        }

    }
}