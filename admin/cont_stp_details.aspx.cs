using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class cont_stp_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "cont_stp";
        }
        private magaContent_DataContext DC_CONTENT;
        protected string listPage = "cont_stp_list.aspx";
        private CONT_TB_STP _currTBL;
        private List<CONT_LN_STP> CURRENT_LANG_;
        private List<CONT_LN_STP> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(CONT_LN_STP)).Cast<CONT_LN_STP>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<CONT_LN_STP>();

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
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    //CONT_TB_STP _page = DC_CONTENT.CONT_TB_STP.SingleOrDefault(x => x.id == _id);
                    //if (_page == null)
                    //    Response.Redirect(listPage);
                    HF_id.Value = Request.QueryString["id"];
                    FillControls();
                }
                else
                    Response.Redirect(listPage);
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
            _currTBL = new CONT_TB_STP();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_CONTENT.CONT_TB_STP.SingleOrDefault(item => item.id == id);
                }


                CURRENT_LANG = DC_CONTENT.CONT_LN_STPs.Where(x => x.pid_page == _currTBL.id).ToList();
                HF_lang.Value = "1";
                Fill_lang();
                LV_langs.DataBind();
                txt_page_name.Text = _currTBL.page_name;
                txt_page_rewrite.Text = _currTBL.page_rewrite;
                UC_get_img_banner.ImgPath = _currTBL.img_banner;
                txt_css.Text = _currTBL.css_file;
                PH_url.Visible = _currTBL.page_rewrite != null && _currTBL.page_rewrite.Trim() != "";
                drp_static_collection.setSelectedValue(_currTBL.pid_collection.ToString());
                Fill_lang();
            }
            else
            {
                txt_page_name.Enabled = true;
            }
        }
        private void FillDataFromControls()
        {
            _currTBL = new CONT_TB_STP();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_CONTENT.CONT_TB_STP.SingleOrDefault(item => item.id == id);
                }
            }
            else
            {
                DC_CONTENT.CONT_TB_STP.InsertOnSubmit(_currTBL);
            }
            _currTBL.css_file = txt_css.Text;
            _currTBL.page_name = txt_page_name.Text;
            _currTBL.page_rewrite = txt_page_rewrite.Text;
            _currTBL.img_banner = UC_get_img_banner.ImgPath;
            _currTBL.pid_collection = drp_static_collection.getSelectedValueInt(null);
            DC_CONTENT.SubmitChanges();
            Save_RL_langs();
            contProps.CONT_STPs = DC_CONTENT.CONT_VIEW_STPs.ToList();
            AdminUtilities.FillRewriteTool();
            HF_id.Value = _currTBL.id.ToString();
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            FillControls();
            //DisableControls();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void DeleteRecord(object sender, EventArgs args)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var page = DC_CONTENT.CONT_TB_STP.SingleOrDefault(item => item.id == id);
            if (page != null)
            {
                var rlLang =
                        DC_CONTENT.CONT_LN_STPs.Where(
                            item => item.pid_page == page.id);
                DC_CONTENT.CONT_LN_STPs.DeleteAllOnSubmit(rlLang);
                DC_CONTENT.CONT_TB_STP.DeleteOnSubmit(page);
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
            var curr_rl_langs = CURRENT_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new CONT_LN_STP();
                rlLang.pid_page = int.Parse(HF_id.Value);
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_LANG.Add(rlLang);

            }
            rlLang.description = re_description.Content;
            rlLang.title = txt_title.Text;
            rlLang.sub_title = txt_sub_title.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.page_path = txt_url.Text.clearPathName(false);
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
                rlLang = new CONT_LN_STP();
            }
            txt_title.Text = rlLang.title;
            re_description.Content = rlLang.description;
            txt_sub_title.Text = rlLang.sub_title;
            txt_summary.Text = rlLang.summary;
            txt_url.Text = rlLang.page_path;
            txt_meta_description.Text = rlLang.meta_description;
            txt_meta_keywords.Text = rlLang.meta_keywords;
            txt_meta_title.Text = rlLang.meta_title;
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                              "tinyEditor",
                                              "setTinyEditor(true);", true);
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_CONTENT.CONT_LN_STPs.Where(x => x.pid_page == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_page = _currTBL.id;
                    DC_CONTENT.CONT_LN_STPs.InsertOnSubmit(rl);
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
