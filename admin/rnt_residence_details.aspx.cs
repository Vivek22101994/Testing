using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_residence_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        private magaRental_DataContext DC_RENTAL;
        protected string listPage = "rnt_residence_list.aspx";
        private RNT_TB_RESIDENCE _currTBL;
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
        private List<RNT_LN_RESIDENCE> CURRENT_LANG_;
        private List<RNT_LN_RESIDENCE> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(RNT_LN_RESIDENCE)).Cast<RNT_LN_RESIDENCE>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<RNT_LN_RESIDENCE>();

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
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                lnk_saveOnly.Visible = UserAuthentication.CurrentUserID == 2;
                _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
                if (_currTBL == null)
                    Response.Redirect(listPage);
                Id_currTBL = Request.QueryString["id"].ToInt32();
                UC_rnt_residence_navlinks1.IdResidence = Id_currTBL;
                Bind_drp_city();
                Bind_drp_zone();
                FillControls();
                Bind_drp_tempLang();
            }
        }
        private void Bind_drp_tempLang()
        {
            drp_tempLang.Items.Insert(0, new ListItem("-seleziona-", "0"));
        }

        protected void lnk_save_tempLang_Click(object sender, EventArgs e)
        {
            if (drp_tempLang.getSelectedValueInt(0) == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    "error",
                                                    "alert('seleziona la risorsa di lingue');", true);
                return;
            }
            Response.Redirect("rnt_residence_details.aspx?id=" + Id_currTBL);
        }
        private void Bind_drp_city()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_city.Items.Clear();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_city.Items.Add(new ListItem(t.title, "" + t.id));
            }
            drp_city.Items.Insert(0, new ListItem("-seleziona-", "0"));
        }
        private void Bind_drp_zone()
        {
            List<LOC_VIEW_ZONE> list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1 && x.pid_city == drp_city.getSelectedValueInt(0)).ToList();
            drp_zone.Items.Clear();
            foreach (LOC_VIEW_ZONE t in list)
            {
                drp_zone.Items.Add(new ListItem(t.title, "" + t.id));
            }
            drp_zone.Items.Insert(0, new ListItem("-seleziona-", "0"));
        }
        protected void drp_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_drp_zone();
        }
        private void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(item => item.id == Id_currTBL);
            if (_currTBL == null)
            {
                _currTBL = new RNT_TB_RESIDENCE();
            }


            CURRENT_LANG = DC_RENTAL.RNT_LN_RESIDENCEs.Where(x => x.pid_residence == _currTBL.id).ToList();
            HF_lang.Value = "1";

            drp_owner.setSelectedValue(_currTBL.pid_owner.ToString());
            txt_code.Text = _currTBL.code ?? "";
            txt_address.Text = _currTBL.loc_address;
            txt_loc_inner_bell.Text = _currTBL.loc_inner_bell;
            txt_phone_1.Text = _currTBL.loc_phone_1;
            txt_phone_2.Text = _currTBL.loc_phone_2;
            txt_zip_code.Text = _currTBL.loc_zip_code;
            txt_inner_notes.Text = _currTBL.inner_notes ?? "";
            drp_city.setSelectedValue(_currTBL.pid_city.ToString());
            Bind_drp_zone();
            drp_zone.setSelectedValue(_currTBL.pid_zone.ToString());

            txt_mq_inner.Text = _currTBL.mq_inner.objToInt32().ToString();
            txt_mq_outer.Text = _currTBL.mq_outer.objToInt32().ToString();
            txt_mq_terrace.Text = _currTBL.mq_terrace.objToInt32().ToString();


            chk_is_active.Checked = _currTBL.is_active == 1;
            chk_is_google_maps.Checked = _currTBL.is_google_maps == 1;

            Fill_lang(HF_lang.Value.ToInt32());
            LV_langs.DataBind();
            RegisterScripts();
            //DisableControls();
        }
        private void FillDataFromControls()
        {
            _currTBL = new RNT_TB_RESIDENCE();
            if (Id_currTBL != 0)
            {
                _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(item => item.id == Id_currTBL);
            }
            else
            {
                DC_RENTAL.RNT_TB_RESIDENCEs.InsertOnSubmit(_currTBL);
            }
            _currTBL.code = txt_code.Text.Trim();
            _currTBL.inner_notes = txt_inner_notes.Text.Trim();
            _currTBL.mq_inner = txt_mq_inner.Text.ToInt32();
            _currTBL.mq_outer = txt_mq_outer.Text.ToInt32();
            _currTBL.mq_terrace = txt_mq_terrace.Text.ToInt32();


            _currTBL.loc_inner_bell = txt_loc_inner_bell.Text;
            _currTBL.loc_address = txt_address.Text;
            _currTBL.loc_zip_code = txt_zip_code.Text;
            _currTBL.loc_phone_1 = txt_phone_1.Text;
            _currTBL.loc_phone_2 = txt_phone_2.Text;

            _currTBL.pid_agent = 0;
            _currTBL.pid_city = drp_city.getSelectedValueInt(0);
            _currTBL.pid_zone = drp_zone.getSelectedValueInt(0);
            _currTBL.pid_owner = drp_owner.getSelectedValueInt(0);
            _currTBL.pid_category = 0;
            _currTBL.pid_type = 0;


            _currTBL.is_active = chk_is_active.Checked ? 1 : 0;
            _currTBL.is_google_maps = chk_is_google_maps.Checked ? 1 : 0;

            DC_RENTAL.SubmitChanges();
            Save_RL_langs();
            AdminUtilities.FillRewriteTool();
            AdminUtilities.locCity_createPagePath(_currTBL.id);
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            //DisableControls();
            Response.Redirect(listPage);
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            RegisterScripts();
        }
        protected void DisableControlsa()
        {
            //txt_meta_title.ReadOnly = true;
            //txt_meta_keywords.ReadOnly = true;
            //txt_meta_description.ReadOnly = true;
            //txt_title.ReadOnly = true;
            //txt_description.ReadOnly = true;
            // txt_summary.ReadOnly = true;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(true);", true);
        }
        protected void EnableControls()
        {
            RegisterScripts();
            return;
            // txt_meta_title.ReadOnly = false;
            //txt_meta_keywords.ReadOnly = false;
            //txt_meta_description.ReadOnly = false;
            //txt_title.ReadOnly = false;
            //txt_description.ReadOnly = false;
            // txt_summary.ReadOnly = false;
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(false);", true);
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
            var page = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(item => item.id == id);
            if (page != null)
            {
                var rlLang =
                        DC_RENTAL.RNT_LN_RESIDENCEs.Where(
                            item => item.pid_residence == page.id);
                DC_RENTAL.RNT_LN_RESIDENCEs.DeleteAllOnSubmit(rlLang);
                DC_RENTAL.RNT_TB_RESIDENCEs.DeleteOnSubmit(page);
                DC_RENTAL.SubmitChanges();
            }
        }
        protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
            lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
            List<RNT_LN_RESIDENCE> _rList = DC_RENTAL.RNT_LN_RESIDENCEs.Where(x => x.pid_lang == lbl_id.Text.ToInt32() && x.pid_residence == Id_currTBL).ToList();
            if (_rList.Count == 0) lnk.CssClass += " important1";
            RNT_LN_RESIDENCE _lang = _rList.FirstOrDefault(x => x.title == null || x.title.Trim() == "" || x.meta_title == null || x.meta_title.Trim() == "" || x.meta_description == null || x.meta_description.Trim() == "" || x.description == null || x.description.Trim() == "");
            if (_lang != null) lnk.CssClass += " important2";
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
                RegisterScripts();
            }
        }

        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == HF_lang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new RNT_LN_RESIDENCE();
                rlLang.pid_residence = Id_currTBL;
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_LANG.Add(rlLang);

            }
            rlLang.description = txt_description.Text;
            rlLang.title = txt_title.Text;
            rlLang.sub_title = txt_sub_title.Text;
            rlLang.summary = txt_summary.Text;
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
                rlLang = new RNT_LN_RESIDENCE();
            }
            txt_title.Text = rlLang.title;
            txt_sub_title.Text = rlLang.sub_title;
            txt_description.Text = rlLang.description;
            txt_summary.Text = rlLang.summary;
            txt_meta_description.Text = rlLang.meta_description;
            txt_meta_keywords.Text = rlLang.meta_keywords;
            txt_meta_title.Text = rlLang.meta_title;
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_RENTAL.RNT_LN_RESIDENCEs.Where(x => x.pid_residence == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_residence = _currTBL.id;
                    DC_RENTAL.RNT_LN_RESIDENCEs.InsertOnSubmit(rl);
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
                }
                // --- genarate PagePath & Rewrite Tool
            }
            DC_RENTAL.SubmitChanges();
            curr_rl_langs = DC_RENTAL.RNT_LN_RESIDENCEs.Where(x => x.pid_residence == _currTBL.id).ToList();
            foreach (CONT_TBL_LANG _lang in maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.Where(x => x.is_active == 1))
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == _lang.id))
                {
                    RNT_LN_RESIDENCE _newRL = new RNT_LN_RESIDENCE();
                    _newRL.pid_lang = _lang.id;
                    _newRL.pid_residence = _currTBL.id;
                    //_newRL.title = txt_code.Text;
                    DC_RENTAL.RNT_LN_RESIDENCEs.InsertOnSubmit(_newRL);
                }
            }
            DC_RENTAL.SubmitChanges();
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
