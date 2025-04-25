using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class loc_zone_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "loc_zone";
        }
        private magaLocation_DataContext DC_LOCATION;
        protected string listPage = "loc_zone_list.aspx";
        private LOC_TB_ZONE _currTBL;
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
        private List<LOC_LN_ZONE> CURRENT_LANG_;
        private List<LOC_LN_ZONE> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(LOC_LN_ZONE)).Cast<LOC_LN_ZONE>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<LOC_LN_ZONE>();

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
                Id_currTBL = Request.QueryString["id"].ToInt32();
                Bind_drp_city();
                FillControls();
            }
        }
        protected void Bind_drp_city()
        {
            List<LOC_VIEW_CITY> _list = DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_city.DataSource = _list;
            drp_city.DataTextField = "title";
            drp_city.DataValueField = "id";
            drp_city.DataBind();
        }

        private void FillControls()
        {
            _currTBL = DC_LOCATION.LOC_TB_ZONEs.SingleOrDefault(item => item.id == Id_currTBL);
            if (_currTBL == null)
            {
                _currTBL = new LOC_TB_ZONE();
                EnableControls();
            }
            else
            {
                DisableControls();
            }
            imgBanner.ImgRoot = "images";
            imgPreview.ImgRoot = "images";
            chk_is_active.Checked = _currTBL.is_active == 1;
            txt_img_thumb.Text = _currTBL.img_thumb;
            drp_city.setSelectedValue(_currTBL.pid_city.ToString());
            imgBanner.ImgPath = _currTBL.img_banner;
            imgPreview.ImgPath = _currTBL.img_preview;

            CURRENT_LANG = DC_LOCATION.LOC_LN_ZONEs.Where(x => x.pid_zone == _currTBL.id).ToList();
            HF_lang.Value = "1";
            Fill_lang();
            LV_langs.DataBind();
            //txt_file_extension.Text = _currTBL.file_extension;
            Fill_lang();
        }
        private void FillDataFromControls()
        {
            _currTBL = DC_LOCATION.LOC_TB_ZONEs.SingleOrDefault(item => item.id == Id_currTBL);
            if (_currTBL == null)
            {
                _currTBL = new LOC_TB_ZONE();
                DC_LOCATION.LOC_TB_ZONEs.InsertOnSubmit(_currTBL);
                ;
            }
            _currTBL.is_active = chk_is_active.Checked ? 1 : 0;
            _currTBL.img_thumb = txt_img_thumb.Text;
            _currTBL.img_banner = imgBanner.ImgPath;
            _currTBL.img_preview = imgPreview.ImgPath;
            _currTBL.pid_city = drp_city.getSelectedValueInt(0);
            _currTBL.file_extension = txt_file_extension.Text;
            DC_LOCATION.SubmitChanges();
            Save_RL_langs();
            AdminUtilities.locZone_createPagePath(_currTBL.id);
            AdminUtilities.FillRewriteTool();
            AppSettings.RNT_activeZones = null;
            AppSettings.LOC_ZONEs = DC_LOCATION.LOC_VIEW_ZONEs.ToList();
            if (Id_currTBL == 0)
                Response.Redirect("" + _currTBL.id);
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
            drp_city.Enabled = false;
            txt_file_extension.ReadOnly = true;
            txt_meta_title.ReadOnly = true;
            //txt_meta_keywords.ReadOnly = true;
            txt_meta_description.ReadOnly = true;
            txt_title.ReadOnly = true;
            re_description.Enabled = false;
            txt_summary.ReadOnly = true;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
            RegisterScripts();
        }
        protected void EnableControls()
        {
            drp_city.Enabled = true;
            txt_file_extension.ReadOnly = false;
            txt_meta_title.ReadOnly = false;
            //txt_meta_keywords.ReadOnly = false;
            txt_meta_description.ReadOnly = false;
            txt_title.ReadOnly = false;
            re_description.Enabled = true;
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
            var page = DC_LOCATION.LOC_TB_ZONEs.SingleOrDefault(item => item.id == id);
            if (page != null)
            {
                var rlLang =
                        DC_LOCATION.LOC_LN_ZONEs.Where(
                            item => item.pid_zone == page.id);
                DC_LOCATION.LOC_LN_ZONEs.DeleteAllOnSubmit(rlLang);
                DC_LOCATION.LOC_TB_ZONEs.DeleteOnSubmit(page);
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
                rlLang = new LOC_LN_ZONE();
                rlLang.pid_zone = Id_currTBL;
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_LANG.Add(rlLang);

            }
            rlLang.description = re_description.Content;
            rlLang.title = txt_title.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.meta_description = txt_meta_description.Text;
            //rlLang.meta_keywords = txt_meta_keywords.Text;
            rlLang.meta_title = txt_meta_title.Text;
            rlLang.folder_path = txt_folder_path.Text;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang()
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new LOC_LN_ZONE();
            }
            txt_title.Text = rlLang.title;
            re_description.Content = rlLang.description;
            txt_summary.Text = rlLang.summary;
            txt_meta_description.Text = rlLang.meta_description;
            //txt_meta_keywords.Text = rlLang.meta_keywords;
            txt_meta_title.Text = rlLang.meta_title;
            txt_folder_path.Text = rlLang.folder_path;
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_LOCATION.LOC_LN_ZONEs.Where(x => x.pid_zone == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_zone = _currTBL.id;
                    DC_LOCATION.LOC_LN_ZONEs.InsertOnSubmit(rl);
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
                    curr_rl.folder_path = rl.folder_path;
                }
                // --- genarate PagePath & Rewrite Tool
            }
            DC_LOCATION.SubmitChanges();
        }
    }
}
