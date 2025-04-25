using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.admin.modRental
{
    public partial class EstateChnlRU_adContent : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";            
        }
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }
        private List<dbRntChnlRentalsUnitedEstateLN> CURRENT_LANG_;
        private List<dbRntChnlRentalsUnitedEstateLN> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(dbRntChnlRentalsUnitedEstateLN)).Cast<dbRntChnlRentalsUnitedEstateLN>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<dbRntChnlRentalsUnitedEstateLN>();

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
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                ltr_apartment.Text = currEstate.code + " / " + "rif. " + IdEstate;
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                FillControls();
            }
        }
        protected void FillControls()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var currTbl = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                txt_year.Text = currTbl.year_purchased;
                CURRENT_LANG = dcChnl.dbRntChnlRentalsUnitedEstateLNs.Where(x => x.pid_estate == currTbl.id).ToList();
                HF_lang.Value = "ita";
                Fill_lang(HF_lang.Value);
                LV_langs.DataBind();
            }
        }

        protected void FillDataFromControls()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var currTbl = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                currTbl.year_purchased = txt_year.Text;
                 dcChnl.SaveChanges(); 
                Save_RL_langs();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate correttamente.\", 340, 110);", true);
                FillControls();
            }
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            //LV.SelectedIndex = -1;
            //LV.DataBind();
            //pnlContent.Visible = false;
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
            lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current lang_" + lbl_id.Text : "tab_item lang_" + lbl_id.Text;
        }

        protected void LV_langs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                Save_lang();
                HF_lang.Value = lbl_id.Text;
                Fill_lang(HF_lang.Value);
                LV_langs.DataBind();
            }
        }

        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == HF_lang.Value);
            if (rlLang == null)
            {
                rlLang = new dbRntChnlRentalsUnitedEstateLN();
                rlLang.pid_estate = IdEstate;
                rlLang.pid_lang = HF_lang.Value;
                CURRENT_LANG.Add(rlLang);

            }

            rlLang.title = txt_title.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.description = re_description.Content;
            rlLang.listing_story = re_listing_story.Content;
            rlLang.unique_benifits = re_unique_benifits.Content;
            rlLang.why_purchased = re_why_purchased.Content;
            rlLang.price_note = re_rate_notes.Content;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang(string pid_lang)
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == pid_lang);
            if (rlLang == null)
            {
                rlLang = new dbRntChnlRentalsUnitedEstateLN();
            }

            txt_title.Text = rlLang.title;
            txt_summary.Text = rlLang.summary;
            re_description.Content = rlLang.description;
            re_listing_story.Content = rlLang.listing_story;
            re_unique_benifits.Content = rlLang.unique_benifits;
            re_why_purchased.Content = rlLang.why_purchased;
            re_rate_notes.Content = rlLang.price_note;

        }

        protected void Save_RL_langs()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                Save_lang();
                var curr_rl_langs = dcChnl.dbRntChnlRentalsUnitedEstateLNs.Where(x => x.pid_estate == IdEstate).ToList();
                foreach (var rl in CURRENT_LANG)
                {
                    if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                    {
                        rl.pid_estate = IdEstate;
                        dcChnl.Add(rl);
                    }
                    else
                    {
                        var curr_rl = curr_rl_langs.Single(x => x.pid_lang == rl.pid_lang);
                        curr_rl.description = rl.description;
                        curr_rl.title = rl.title;
                        curr_rl.summary = rl.summary;
                        curr_rl.listing_story = rl.listing_story;
                        curr_rl.unique_benifits = rl.unique_benifits;
                        curr_rl.why_purchased = rl.why_purchased;
                        curr_rl.price_note = rl.price_note;

                    }

                }
                dcChnl.SaveChanges(); 
                curr_rl_langs = dcChnl.dbRntChnlRentalsUnitedEstateLNs.Where(x => x.pid_estate == IdEstate).ToList();
                foreach (var _lang in contProps.LangTBL.Where(x => x.is_active == 1))
                {
					var code = contUtils.getLang_code(_lang.id);
                    if (!curr_rl_langs.Exists(x => x.pid_lang == code))
                    {
                        dbRntChnlRentalsUnitedEstateLN _newRL = new dbRntChnlRentalsUnitedEstateLN();
                        _newRL.pid_lang = code;
                        _newRL.pid_estate = IdEstate;
                        //_newRL.title = txt_code.Text;
                        dcChnl.Add(_newRL);
                    }
                }
                dcChnl.SaveChanges(); 
            }
        }
        protected void lnk_copyLang_Click(object sender, EventArgs e)
        {
            HF_copyLang.Value = HF_lang.Value;
            lnk_pasteLang.Visible = true;
            //ScriptManager.RegisterStartupScript(this, this.GetType(),"tinyEditor", "setTinyEditor(false);", true);
        }
        protected void lnk_pasteLang_Click(object sender, EventArgs e)
        {
            Fill_lang(HF_copyLang.Value);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "tinyEditor","setTinyEditor(false);", true);
        }
    }
}