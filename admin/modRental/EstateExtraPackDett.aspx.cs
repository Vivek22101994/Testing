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
    public partial class EstateExtraPackDett : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "extras";
        }
        protected dbRntExtrasPackTB currTBL;
        private List<dbRntExtrasPackLN> TMPcurrLangs;
        private List<dbRntExtrasPackLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbRntExtrasPackLN)).Cast<dbRntExtrasPackLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbRntExtrasPackLN>();

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
            string _folder = "images/ExtraPack";
            if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntExtrasPackTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRntExtrasPackTB();
                    ltrTitle.Text = "Inserimento nuovo accessorio";
                }
                else
                {
                    ltrTitle.Text = "Modifca accessorio #" + currTBL.id;
                }
                txt_code.Text = currTBL.code ?? "";
                drp_isActive.setSelectedValue(currTBL.isActive);

                imgPreview.ImgPathDef = "";
                imgPreview.ImgRoot = _folder;
                imgPreview.ImgPath = currTBL.imgThumb;
                currLangs = dc.dbRntExtrasPackLNs.Where(x => x.pidPack == currTBL.id).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntExtrasPackTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRntExtrasPackTB();
                    dc.Add(currTBL);
                }
                var tmpTbl = new dbRntExtrasCategoryTB();

                currTBL.code = txt_code.Text;
                currTBL.isActive = drp_isActive.getSelectedValueInt();
                currTBL.imgThumb = imgPreview.ImgPath;
                dc.SaveChanges();
                SaveAllLangs(currTBL.id);
                HfId.Value = Convert.ToString(currTBL.id);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
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
                rlLang = new dbRntExtrasPackLN();
                rlLang.pidPack = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.title = txt_title.Text;
            rlLang.subTitle = txt_sub_title.Text;
            rlLang.description = txt_description.Text;
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRntExtrasPackLN();
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
                    if (dc.dbRntExtrasPackLNs.SingleOrDefault(x => x.pidPack == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidPack = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbRntExtrasPackLNs.Single(x => x.pidPack == id && x.pidLang == rl.pidLang);
                        rl.CopyToPack(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }
    }
}