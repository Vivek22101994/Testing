using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class loc_city_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "loc_city";
        }
        private magaLocation_DataContext DC_LOCATION;
        protected string listPage = "loc_city_list.aspx";
        private LOC_TB_CITY _currTBL;
        public int Id_currTBL
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
            }
        }
        private List<LOC_LN_CITY> CURRENT_LANG_;
        private List<LOC_LN_CITY> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(LOC_LN_CITY)).Cast<LOC_LN_CITY>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<LOC_LN_CITY>();

                return CURRENT_LANG_;
            }
            set
            {
                ViewState["CURRENT_LANG"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_LANG_ = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_LOCATION = maga_DataContext.DC_LOCATION;
            if (!IsPostBack)
            {
                _currTBL = DC_LOCATION.LOC_TB_CITies.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
                if (_currTBL == null)
                    Response.Redirect(listPage);
                Id_currTBL = Request.QueryString["id"].ToInt32();
                FillControls();
            }
        }
        private void FillControls()
        {
            _currTBL = new LOC_TB_CITY();
            if (Id_currTBL!=0)
            {
                _currTBL = DC_LOCATION.LOC_TB_CITies.SingleOrDefault(item => item.id == Id_currTBL);
            }

            CURRENT_LANG = DC_LOCATION.LOC_LN_CITies.Where(x => x.pid_city == _currTBL.id).ToList();
            HF_lang.Value = "1";
            Fill_lang();
            LV_langs.DataBind();
            UC_get_img_banner.ImgPath = _currTBL.img_banner;
            Fill_lang();
            DisableControls();
        }
        private void FillDataFromControls()
        {
            _currTBL = new LOC_TB_CITY();
            if (Id_currTBL != 0)
            {
                _currTBL = DC_LOCATION.LOC_TB_CITies.SingleOrDefault(item => item.id == Id_currTBL);
            }
            else
            {
                DC_LOCATION.LOC_TB_CITies.InsertOnSubmit(_currTBL);
            }
            _currTBL.img_banner = UC_get_img_banner.ImgPath;            
            DC_LOCATION.SubmitChanges();
            Save_RL_langs();
            AdminUtilities.locCity_createPagePath(_currTBL.id);
            AdminUtilities.FillRewriteTool();
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            DisableControls();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_modify_Click(object sender, EventArgs e)
        {
            EnableControls();
        }
        protected void DisableControls()
        {
            txt_meta_title.ReadOnly = true;
            txt_meta_keywords.ReadOnly = true;
            txt_meta_description.ReadOnly = true;
            txt_title.ReadOnly = true;
            //txt_description.ReadOnly = true;
            txt_summary.ReadOnly = true;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
            RegisterScripts();
        }
        protected void EnableControls()
        {
            txt_meta_title.ReadOnly = false;
            txt_meta_keywords.ReadOnly = false;
            txt_meta_description.ReadOnly = false;
            txt_title.ReadOnly = false;
            //txt_description.ReadOnly = false;
            txt_summary.ReadOnly = false;
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
            RegisterScripts();
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(false);", true);
        }

        protected void DeleteRecord(object sender, EventArgs args)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var page = DC_LOCATION.LOC_TB_CITies.SingleOrDefault(item => item.id == id);
            if (page != null)
            {
                var rlLang =
                        DC_LOCATION.LOC_LN_CITies.Where(
                            item => item.pid_city == page.id);
                DC_LOCATION.LOC_LN_CITies.DeleteAllOnSubmit(rlLang);
                DC_LOCATION.LOC_TB_CITies.DeleteOnSubmit(page);
                DC_LOCATION.SubmitChanges();
            }
        }
        protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
            lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
        }

        protected void LV_langs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                Save_lang();
                HF_lang.Value = lbl_id.Text;
                Fill_lang();
                LV_langs.DataBind();
                RegisterScripts();
            }
        }

        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new LOC_LN_CITY();
                rlLang.pid_city = Id_currTBL;
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_LANG.Add(rlLang);

            }
            rlLang.description = txt_description.Text;
            rlLang.title = txt_title.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.meta_description = txt_meta_description.Text;
            rlLang.meta_keywords = txt_meta_keywords.Text;
            rlLang.meta_title = txt_meta_title.Text;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang()
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new LOC_LN_CITY();
            }
            txt_title.Text = rlLang.title;
            txt_description.Text = rlLang.description;
            txt_summary.Text = rlLang.summary;
            txt_meta_description.Text = rlLang.meta_description;
            txt_meta_keywords.Text = rlLang.meta_keywords;
            txt_meta_title.Text = rlLang.meta_title;
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_LOCATION.LOC_LN_CITies.Where(x => x.pid_city == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_city = _currTBL.id;
                    DC_LOCATION.LOC_LN_CITies.InsertOnSubmit(rl);
                }
                else
                {
                    var curr_rl = curr_rl_langs.Single(x => x.pid_lang == rl.pid_lang);
                    curr_rl.description = rl.description;
                    curr_rl.meta_description = rl.meta_description;
                    curr_rl.meta_keywords = rl.meta_keywords;
                    curr_rl.meta_title = rl.meta_title;
                    curr_rl.page_path = rl.page_path;
                    curr_rl.title = rl.title;
                    curr_rl.summary = rl.summary;
                }
                // --- genarate PagePath & Rewrite Tool
            }
            DC_LOCATION.SubmitChanges();
        }
    }
}
