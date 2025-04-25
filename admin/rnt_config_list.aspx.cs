using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_config_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;

        protected RNT_TB_CONFIG _currTBL;
        private List<RNT_LN_CONFIG> CURRENT_LANG_;
        private List<RNT_LN_CONFIG> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(RNT_LN_CONFIG)).Cast<RNT_LN_CONFIG>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<RNT_LN_CONFIG>();

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
            }
        }

        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            var lbl_id = (Label)LV.Items[e.NewSelectedIndex].FindControl("lbl_id");
            HF_id.Value = lbl_id.Text;
            FillControls();
        }

        private void FillControls()
        {
            _currTBL = new RNT_TB_CONFIG();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_RENTAL.RNT_TB_CONFIGs.SingleOrDefault(item => item.id == id);
                }
                _currTBL.img_thumb = "";
            }

            CURRENT_LANG = DC_RENTAL.RNT_LN_CONFIGs.Where(x => x.pid_config == _currTBL.id).ToList();

            HF_lang.Value = "1";
            Fill_lang();
            LV_langs.DataBind();

            pnlContent.Visible = true;                                  
            Fill_lang();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scrolTo", "$.scrollTo($(\"#" + pnlContent.ClientID + "\"), 500);", true);
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            _currTBL = new RNT_TB_CONFIG();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_RENTAL.RNT_TB_CONFIGs.SingleOrDefault(item => item.id == id);
                }
            }
            else
            {
                DC_RENTAL.RNT_TB_CONFIGs.InsertOnSubmit(_currTBL);
            }
            DC_RENTAL.SubmitChanges();
            Save_RL_langs();
            pnlContent.Visible = false;
            LDS.DataBind();
            LV.DataBind();
            LV.SelectedIndex = -1;
        }

        protected void lnk_nuovo_Click(object sender, EventArgs e)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            HF_id.Value = "0";
            FillControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
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
                rlLang = new RNT_LN_CONFIG();
                rlLang.pid_config = int.Parse(HF_id.Value);
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                curr_rl_langs.Add(rlLang);
            }
            rlLang.title = txt_title.Text;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang()
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new RNT_LN_CONFIG();
            }
            txt_title.Text = rlLang.title;
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_RENTAL.RNT_LN_CONFIGs.Where(x => x.pid_config == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_config = _currTBL.id;
                    DC_RENTAL.RNT_LN_CONFIGs.InsertOnSubmit(rl);
                }
                else
                {
                    var curr_rl = curr_rl_langs.Single(x => x.pid_lang == rl.pid_lang);
                    curr_rl.title = rl.title;
                }
                // --- genarate PagePath & Rewrite Tool
            }
            DC_RENTAL.SubmitChanges();
        }

        protected void LV_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }
    }
}
