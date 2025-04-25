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
    public partial class EstateExtraCategoryDett : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "extras";
        }
        protected dbRntExtrasCategoryTB currTBL;
        private List<dbRntExtrasCategoryLN> TMPcurrLangs;
        private List<dbRntExtrasCategoryLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbRntExtrasCategoryLN)).Cast<dbRntExtrasCategoryLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbRntExtrasCategoryLN>();

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
                //txt_dtStart.Text = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");
                //txt_dtEnd.Text = DateTime.Now.AddDays(10).ToString("dd/MM/yyyy");
                HfId.Value = Request.QueryString["id"].ToInt64().ToString();
                HfMacroCat.Value = Request.QueryString["macrocategory"].ToInt64().ToString();
                if (HfMacroCat.Value != "0")
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        dbRntExtrasMacroCategoryTB currTBLMacroCategory = dc.dbRntExtrasMacroCategoryTBs.SingleOrDefault(x => x.id == HfMacroCat.Value.ToInt32());
                        lbl_title.Text = "Macro Categoria:" + " " + currTBLMacroCategory.code;
                    }
                }
                
                fillData();
            }
        }
        private void fillData()
        {
            string _folder = "images/ExtraCategory";
            if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntExtrasCategoryTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRntExtrasCategoryTB();
                    ltrTitle.Text = "Inserimento nuovo accessorio";
                }
                else
                {
                    ltrTitle.Text = "Modifca accessorio #" + currTBL.id;
                }
                Bind_drp_macroCategory();
                if (HfId.Value == "0")
                {
                    drp_macroCategory.SelectedValue = HfMacroCat.Value;
                }
                else
                {
                    drp_macroCategory.SelectedValue = Convert.ToString(currTBL.pidMacroCategory);
                }
                txt_code.Text = currTBL.code ?? "";
                drp_isActive.setSelectedValue(currTBL.isActive);
               
                imgPreview.ImgPathDef = "";
                imgPreview.ImgRoot = _folder;
                imgPreview.ImgPath = currTBL.imgThumb;
                currLangs = dc.dbRntExtrasCategoryLNs.Where(x => x.pidCategory == currTBL.id).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            if (drp_macroCategory.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"è richiesto macro categoria.\", 340, 110);", true);
            }
            else
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntExtrasCategoryTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                    if (currTBL == null)
                    {
                        currTBL = new dbRntExtrasCategoryTB();
                        dc.Add(currTBL);


                    }
                    var tmpTbl = new dbRntExtrasCategoryTB();

                    currTBL.code = txt_code.Text;
                    currTBL.pidMacroCategory = Convert.ToInt32(drp_macroCategory.SelectedValue);
                    currTBL.isActive = drp_isActive.getSelectedValueInt();
                    currTBL.imgThumb = imgPreview.ImgPath;
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
                rlLang = new dbRntExtrasCategoryLN();
                rlLang.pidCategory = HfId.Value.ToInt32();
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
                rlLang = new dbRntExtrasCategoryLN();
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
                    if (dc.dbRntExtrasCategoryLNs.SingleOrDefault(x => x.pidCategory == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidCategory = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbRntExtrasCategoryLNs.Single(x => x.pidCategory == id && x.pidLang == rl.pidLang);
                        rl.CopyToCategory(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }

        private void Bind_drp_macroCategory()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntExtrasMacroCategoryTB> macrocatList = dc.dbRntExtrasMacroCategoryTBs.Where(x => x.isActive == 1).ToList();
                drp_macroCategory.Items.Clear();
                foreach (dbRntExtrasMacroCategoryTB macrocat in macrocatList)
                {
                    drp_macroCategory.Items.Add(new ListItem(macrocat.code, "" + macrocat.id));
                }
                drp_macroCategory.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
        }

  
    }
}