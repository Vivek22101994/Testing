using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class edit_privacy : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        private magaContent_DataContext DC_CONTENT;
        private List<CONT_RL_PAGE_LANG> CURRENT_STATIC_PAGE_LANG_;
        private List<CONT_RL_PAGE_LANG> CURRENT_STATIC_PAGE_LANG
        {
            get
            {
                if (CURRENT_STATIC_PAGE_LANG_ == null)
                    if (ViewState["CURRENT_PAGE_LANG"] != null)
                    {
                        CURRENT_STATIC_PAGE_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_PAGE_LANG"],
                                                 typeof(CONT_RL_PAGE_LANG)).Cast<CONT_RL_PAGE_LANG>().ToList();
                    }
                    else
                        CURRENT_STATIC_PAGE_LANG_ = new List<CONT_RL_PAGE_LANG>();

                return CURRENT_STATIC_PAGE_LANG_;
            }
            set
            {
                ViewState["CURRENT_PAGE_LANG"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_STATIC_PAGE_LANG_ = value;
            }
        }
        private CONT_TBL_PAGE currentPage;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack)
            {
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    CONT_TBL_PAGE _page = DC_CONTENT.CONT_TBL_PAGEs.SingleOrDefault(x => x.id == _id);
                    if (_page == null)
                        Response.Redirect("manage_static_page.aspx");
                    HF_id.Value = Request.QueryString["id"];
                    FillControls();
                }
                else
                    Response.Redirect("manage_static_page.aspx");
                Bind_drp_static_collection();
            }
            txt_page_rewrite.Enabled = UserAuthentication.CurrentUserID == 2;
        }
        protected void Bind_drp_static_collection()
        {
            List<CONT_TBL_COLLECTION> _list = DC_CONTENT.CONT_TBL_COLLECTIONs.ToList();
            drp_static_collection.DataSource = _list;
            drp_static_collection.DataTextField = "title";
            drp_static_collection.DataValueField = "id";
            drp_static_collection.DataBind();
            drp_static_collection.Items.Insert(0, new ListItem("- - -", "0"));
        }
        private void FillControls()
        {
            currentPage = new CONT_TBL_PAGE();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    currentPage = DC_CONTENT.CONT_TBL_PAGEs.SingleOrDefault(item => item.id == id);
                }
            }

            CURRENT_STATIC_PAGE_LANG = DC_CONTENT.CONT_RL_PAGE_LANGs.Where(x => x.pid_page == currentPage.id).ToList();
            HF_lang.Value = "1";
            Fill_lang();
            LV_langs.DataBind();
            txt_page_name.Text = currentPage.page_name;
            txt_page_rewrite.Text = currentPage.page_rewrite;
            txt_img_banner.Text = currentPage.img_banner;
            txt_css.Text = currentPage.css_file;
            PH_url.Visible = currentPage.page_rewrite != null && currentPage.page_rewrite.Trim() != "";
            drp_static_collection.setSelectedValue(currentPage.pid_collection.ToString());
            Fill_lang();
            DisableControls();
        }
        private void FillDataFromControls()
        {
            currentPage = new CONT_TBL_PAGE();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    currentPage = DC_CONTENT.CONT_TBL_PAGEs.SingleOrDefault(item => item.id == id);
                }
            }
            else
            {
                DC_CONTENT.CONT_TBL_PAGEs.InsertOnSubmit(currentPage);
            }
            currentPage.css_file = txt_css.Text;
            currentPage.page_name = txt_page_name.Text;
            currentPage.page_rewrite = txt_page_rewrite.Text;
            currentPage.img_banner = txt_img_banner.Text;
            currentPage.pid_collection = drp_static_collection.getSelectedValueInt(null);
            DC_CONTENT.SubmitChanges();
            Save_RL_langs();
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
            txt_url.ReadOnly = true;
            txt_title.ReadOnly = true;
            txt_page_rewrite.ReadOnly = true;
            txt_img_banner.ReadOnly = true;
            //txt_description.ReadOnly = true;
            txt_sub_title.ReadOnly = true;
            txt_summary.ReadOnly = true;
            drp_static_collection.Enabled = false;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
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
            txt_page_rewrite.ReadOnly = false;
            txt_img_banner.ReadOnly = false;
            //txt_description.ReadOnly = false;
            txt_sub_title.ReadOnly = false;
            txt_summary.ReadOnly = false;
            drp_static_collection.Enabled = true;
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(false);", true);
        }

        protected void DeleteRecord(object sender, EventArgs args)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var page = DC_CONTENT.CONT_TBL_PAGEs.SingleOrDefault(item => item.id == id);
            if (page != null)
            {
                var rlLang =
                        DC_CONTENT.CONT_RL_PAGE_LANGs.Where(
                            item => item.pid_page == page.id);
                DC_CONTENT.CONT_RL_PAGE_LANGs.DeleteAllOnSubmit(rlLang);
                DC_CONTENT.CONT_TBL_PAGEs.DeleteOnSubmit(page);
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
                Fill_lang();
                LV_langs.DataBind();
            }
        }

        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_STATIC_PAGE_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new CONT_RL_PAGE_LANG();
                rlLang.pid_page = int.Parse(HF_id.Value);
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_STATIC_PAGE_LANG.Add(rlLang);

            }
            rlLang.description = txt_description.Text;
            rlLang.title = txt_title.Text;
            rlLang.sub_title = txt_sub_title.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.page_path = txt_url.Text;
            rlLang.meta_description = txt_meta_description.Text;
            rlLang.meta_keywords = txt_meta_keywords.Text;
            rlLang.meta_title = txt_meta_title.Text;
            CURRENT_STATIC_PAGE_LANG = curr_rl_langs;
        }

        protected void Fill_lang()
        {
            var rlLang = CURRENT_STATIC_PAGE_LANG.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new CONT_RL_PAGE_LANG();
            }
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
            var curr_rl_langs = DC_CONTENT.CONT_RL_PAGE_LANGs.Where(x => x.pid_page == currentPage.id).ToList();
            foreach (var rl in CURRENT_STATIC_PAGE_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_page = currentPage.id;
                    DC_CONTENT.CONT_RL_PAGE_LANGs.InsertOnSubmit(rl);
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
                    curr_rl.sub_title = rl.sub_title;
                    curr_rl.summary = rl.summary;
                    curr_rl.page_path = rl.page_path;
                }
                // --- genarate PagePath & Rewrite Tool
            }
            DC_CONTENT.SubmitChanges();
        }
    }
}
