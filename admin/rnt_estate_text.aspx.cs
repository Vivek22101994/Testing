using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class rnt_estate_text : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        private magaRental_DataContext DC_RENTAL;
        protected string listPage = "rnt_estate_list.aspx";
        private RNT_TB_ESTATE _currTBL;
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
        private List<RNT_LN_ESTATE> CURRENT_LANG_;
        private List<RNT_LN_ESTATE> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(RNT_LN_ESTATE)).Cast<RNT_LN_ESTATE>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<RNT_LN_ESTATE>();

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
                lnk_salva.Visible = lnk_annulla.Visible = UserAuthentication.hasPermission("rnt_estate", "can_edit");
                _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
                if (_currTBL == null)
                    Response.Redirect(listPage);
                Id_currTBL = Request.QueryString["id"].ToInt32();
                UC_rnt_estate_navlinks1.IdEstate = Id_currTBL;
                
                FillControls();
            }
        }
        private void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(item => item.id == Id_currTBL);
            if (_currTBL == null)
            {
                _currTBL = new RNT_TB_ESTATE();
                // default values
                _currTBL.num_persons_adult = 2;
                _currTBL.num_persons_child = 2;
            }


            CURRENT_LANG = DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == _currTBL.id).ToList();
            HF_lang.Value = "1";            
            Fill_lang(HF_lang.Value.ToInt32());
            LV_langs.DataBind();
            RegisterScripts();
            //DisableControls();
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(false);", true);
        }
        protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
            lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
            List<RNT_LN_ESTATE> _rList = DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_lang == lbl_id.Text.ToInt32() && x.pid_estate == Id_currTBL).ToList();
            if (_rList.Count == 0) lnk.CssClass += " important1";
            RNT_LN_ESTATE _lang = _rList.FirstOrDefault(x => x.title == null || x.title.Trim() == "" || x.meta_title == null || x.meta_title.Trim() == "" || x.meta_description == null || x.meta_description.Trim() == "" || x.description == null || x.description.Trim() == "");
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
                rlLang = new RNT_LN_ESTATE();
                rlLang.pid_estate = Id_currTBL;
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_LANG.Add(rlLang);

            }
            rlLang.description = re_description.Content;
            rlLang.title = txt_title.Text;
            rlLang.sub_title = txt_sub_title.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.meta_description = txt_meta_description.Text;
            rlLang.meta_keywords = txt_meta_keywords.Text;
            rlLang.meta_title = txt_meta_title.Text;
            rlLang.mobileDescription = txt_mobileDescription.Text;
            rlLang.haHeadLine = txt_headLine.Text;
            rlLang.haDescription = re_haDescription.Content;
            rlLang.haOtherActivities = re_haOtherActivities.Content;
            rlLang.haRateNote = re_rateNotes.Content;

            //rlLang.haR


            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang(int pid_lang)
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == pid_lang);
            if (rlLang == null)
            {
                rlLang = new RNT_LN_ESTATE();
            }
            txt_title.Text = rlLang.title;
            txt_sub_title.Text = rlLang.sub_title;
            re_description.Content = rlLang.description;
            txt_summary.Text = rlLang.summary;
            txt_meta_description.Text = rlLang.meta_description;
            txt_meta_keywords.Text = rlLang.meta_keywords;
            txt_meta_title.Text = rlLang.meta_title;
            txt_mobileDescription.Text = rlLang.mobileDescription;
            txt_headLine.Text = rlLang.haHeadLine;
            re_haDescription.Content = rlLang.haDescription;
            re_haOtherActivities.Content = rlLang.haOtherActivities;
            re_rateNotes.Content = rlLang.haRateNote;

        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_estate = _currTBL.id;
                    DC_RENTAL.RNT_LN_ESTATE.InsertOnSubmit(rl);
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
                    curr_rl.mobileDescription = rl.mobileDescription;
                    curr_rl.haHeadLine = rl.haHeadLine;
                    curr_rl.haDescription = rl.haDescription;
                    curr_rl.haOtherActivities = rl.haOtherActivities;
                    curr_rl.haRateNote = rl.haRateNote;


                }
                // --- genarate PagePath & Rewrite Tool
            }
            DC_RENTAL.SubmitChanges();
            curr_rl_langs = DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == _currTBL.id).ToList();
            foreach (CONT_TBL_LANG _lang in maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.Where(x => x.is_active == 1))
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == _lang.id))
                {
                    RNT_LN_ESTATE _newRL = new RNT_LN_ESTATE();
                    _newRL.pid_lang = _lang.id;
                    _newRL.pid_estate = _currTBL.id;
                    //_newRL.title = txt_code.Text;
                    DC_RENTAL.RNT_LN_ESTATE.InsertOnSubmit(_newRL);
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
        private void FillDataFromControls()
        {
            RNT_TB_ESTATE tbBefore = null;
            DC_RENTAL.CommandTimeout = 0;
            _currTBL = new RNT_TB_ESTATE();
            if (Id_currTBL != 0)
            {
                _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(item => item.id == Id_currTBL);
                tbBefore = _currTBL.Clone();
            }
            else
            {
                DC_RENTAL.RNT_TB_ESTATE.InsertOnSubmit(_currTBL);
            }
            
            DC_RENTAL.CommandTimeout = 0;
            DC_RENTAL.SubmitChanges();
            Save_RL_langs();
            if (tbBefore != null) rntUtils.estate_addLog(tbBefore, _currTBL, UserAuthentication.CurrentUserID, UserAuthentication.CurrentUserName);
            rntUtils.rntEstate_createPagePath(_currTBL.id);
            AdminUtilities.FillRewriteTool();
            AppSettings._refreshCache_RNT_ESTATEs();
            AppSettings.RELOAD_SESSION();
            

        }
    }
}