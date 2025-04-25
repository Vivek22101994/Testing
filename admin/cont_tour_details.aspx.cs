using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class cont_tour_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "cont_tour";
        }
        private magaContent_DataContext DC_CONTENT;
        protected string listPage = "cont_tour_list.aspx";
        private CONT_TB_TOUR _currTBL;
        private List<CONT_LN_TOUR> CURRENT_LANG_;
        private List<CONT_LN_TOUR> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(CONT_LN_TOUR)).Cast<CONT_LN_TOUR>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<CONT_LN_TOUR>();

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
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack)
            {
                HF_id.Value = Request.QueryString["id"].objToInt32().ToString();
                FillControls();
            }
        }
        private void FillControls()
        {
            _currTBL = DC_CONTENT.CONT_TB_TOURs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new CONT_TB_TOUR();
            }

            CURRENT_LANG = DC_CONTENT.CONT_LN_TOURs.Where(x => x.pid_tour == _currTBL.id).ToList();
            HF_lang.Value = "1";
            LV_langs.DataBind();
            UC_get_img_preview.ImgPath = _currTBL.img_banner;
            chk_is_active.Checked = _currTBL.is_acitve == 1;
            lnk_pasteLang.Visible = false;
            Fill_lang(HF_lang.Value.ToInt32());
            if (HF_id.Value != "0")
                DisableControls();
            else
                EnableControls();
        }
        private void FillDataFromControls()
        {
            _currTBL = _currTBL = DC_CONTENT.CONT_TB_TOURs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if(_currTBL==null)
            {
                HF_id.Value = "0";
                _currTBL=new CONT_TB_TOUR();
                DC_CONTENT.CONT_TB_TOURs.InsertOnSubmit(_currTBL);
            }
            _currTBL.img_banner = UC_get_img_preview.ImgPath;
            _currTBL.is_acitve = chk_is_active.Checked ? 1 : 0;
            DC_CONTENT.SubmitChanges();
            Save_RL_langs();
            AdminUtilities.FillRewriteTool();
            if (HF_id.Value != "0")
            {
                DisableControls();
            }
            else
            {
                Response.Redirect("cont_tour_details.aspx?id=" + _currTBL.id);
            }
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
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
            txt_url.ReadOnly = true;
            txt_title.ReadOnly = true;
            //txt_description.ReadOnly = true;
            txt_sub_title.ReadOnly = true;
            txt_summary.ReadOnly = true;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
            lnk_copyLang.Enabled = false;
            lnk_pasteLang.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(true);", true);
        }
        protected void EnableControls()
        {
            txt_meta_title.ReadOnly = false;
            txt_meta_keywords.ReadOnly = false;
            txt_meta_description.ReadOnly = false;
            txt_url.ReadOnly = false;
            txt_title.ReadOnly = false;
            //txt_description.ReadOnly = false;
            txt_sub_title.ReadOnly = false;
            txt_summary.ReadOnly = false;
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
            lnk_copyLang.Enabled = true;
            lnk_pasteLang.Enabled = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(false);", true);
        }

        protected void DeleteRecord(object sender, EventArgs args)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var page = DC_CONTENT.CONT_TB_TOURs.SingleOrDefault(item => item.id == id);
            if (page != null)
            {
                var rlLang =
                        DC_CONTENT.CONT_LN_TOURs.Where(
                            item => item.pid_tour == page.id);
                DC_CONTENT.CONT_LN_TOURs.DeleteAllOnSubmit(rlLang);
                DC_CONTENT.CONT_TB_TOURs.DeleteOnSubmit(page);
                DC_CONTENT.SubmitChanges();
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
                Fill_lang(HF_lang.Value.ToInt32());
                LV_langs.DataBind();
            }
        }

        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new CONT_LN_TOUR();
                rlLang.pid_tour = int.Parse(HF_id.Value);
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_LANG.Add(rlLang);

            }
            rlLang.price_description = txt_price_description.Text;
            rlLang.description = txt_description.Text;
            rlLang.title = txt_title.Text;
            rlLang.sub_title = txt_sub_title.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.page_path = txt_url.Text;
            rlLang.meta_description = txt_meta_description.Text;
            rlLang.meta_keywords = txt_meta_keywords.Text;
            rlLang.meta_title = txt_meta_title.Text;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang(int pid_lang)
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == pid_lang);
            if (rlLang == null)
            {
                rlLang = new CONT_LN_TOUR();
            }
            txt_price_description.Text = rlLang.price_description;
            txt_title.Text = rlLang.title;
            txt_description.Text = rlLang.description;
            txt_sub_title.Text = rlLang.sub_title;
            txt_summary.Text = rlLang.summary;
            txt_url.Text = rlLang.page_path;
            txt_meta_description.Text = rlLang.meta_description;
            txt_meta_keywords.Text = rlLang.meta_keywords;
            txt_meta_title.Text = rlLang.meta_title;
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_CONTENT.CONT_LN_TOURs.Where(x => x.pid_tour == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_tour = _currTBL.id;
                    DC_CONTENT.CONT_LN_TOURs.InsertOnSubmit(rl);
                }
                else
                {
                    var curr_rl = curr_rl_langs.Single(x => x.pid_lang == rl.pid_lang);
                    curr_rl.price_description = rl.price_description;
                    curr_rl.description = rl.description;
                    curr_rl.meta_description = rl.meta_description;
                    curr_rl.meta_keywords = rl.meta_keywords;
                    curr_rl.meta_title = rl.meta_title;
                    curr_rl.page_path = rl.page_path;
                    curr_rl.title = rl.title;
                    curr_rl.sub_title = rl.sub_title;
                    curr_rl.summary = rl.summary;
                    curr_rl.page_path = rl.page_path;
                }
                // --- genarate PagePath & Rewrite Tool
            }
            DC_CONTENT.SubmitChanges();
        }
        protected void lnk_copyLang_Click(object sender, EventArgs e)
        {
            HF_copyLang.Value = HF_lang.Value;
            lnk_pasteLang.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                               "tinyEditor",
                                               "setTinyEditor(false);", true);
        }
        protected void lnk_pasteLang_Click(object sender, EventArgs e)
        {
            Fill_lang(HF_copyLang.Value.ToInt32());
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(false);", true);
        }
    }
}
