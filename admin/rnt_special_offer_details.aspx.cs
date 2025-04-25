using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_special_offer_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_special_offer";
        }
        private magaRental_DataContext DC_RENTAL;
        protected string listPage = "rnt_special_offer_list.aspx";
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        private RNT_TB_SPECIAL_OFFER _currTBL;
        private List<RNT_LN_SPECIAL_OFFER> CURRENT_LANG_;
        private List<RNT_LN_SPECIAL_OFFER> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(RNT_LN_SPECIAL_OFFER)).Cast<RNT_LN_SPECIAL_OFFER>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<RNT_LN_SPECIAL_OFFER>();

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
                Bind_drp_zone();
                Bind_drp_estate();
                HF_id.Value = Request.QueryString["id"].objToInt32().ToString();
                FillControls();
            }
        }
        protected void Bind_drp_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1).OrderBy(x => x.title).ToList();
            drp_zone.DataSource = _list;
            drp_zone.DataTextField = "title";
            drp_zone.DataValueField = "id";
            drp_zone.DataBind();
            drp_zone.Items.Insert(0, new ListItem("- tutti -", "-1"));
            drp_zone.Items.Insert(0, new ListItem("- seleziona -", "-2"));
        }
        protected void drp_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_drp_estate();
        }
        private void Bind_drp_estate()
        {
            List<RNT_TB_ESTATE> _list = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.pid_zone.HasValue && (x.pid_zone == drp_zone.getSelectedValueInt(0) || drp_zone.getSelectedValueInt(0)==-1)).OrderBy(x => x.code).ToList();
            drp_estate.DataSource = _list;
            drp_estate.DataTextField = "code";
            drp_estate.DataValueField = "id";
            drp_estate.DataBind();
            drp_estate.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
        private void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_SPECIAL_OFFER.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL==null)
            {
                _currTBL = new RNT_TB_SPECIAL_OFFER();
                ltr_pageTitle.Text = "Nuova Offerta speciale";
            }
            else
            {
                ltr_pageTitle.Text = "Scheda Offerta speciale della struttura: "+CurrentSource.rntEstate_code(_currTBL.pid_estate.objToInt32(),"---");
            }

            CURRENT_LANG = DC_RENTAL.RNT_LN_SPECIAL_OFFERs.Where(x => x.pid_special_offer == _currTBL.id).ToList();
            HF_lang.Value = "1";
            Fill_lang();
            LV_langs.DataBind();

            RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_est != null)
            {
                drp_zone.setSelectedValue(_est.pid_zone.ToString());
                Bind_drp_estate();
            }
            drp_estate.setSelectedValue(_currTBL.pid_estate.ToString());
            drp_class_type.setSelectedValue(_currTBL.class_type);
            txt_pr_discount.Text = _currTBL.pr_discount.objToInt32().ToString();

            HF_dtStart.Value = _currTBL.dtStart.HasValue ? _currTBL.dtStart.JSCal_dateToString() : DateTime.Now.AddDays(3).JSCal_dateToString();
            HF_dtEnd.Value = _currTBL.dtEnd.HasValue ? _currTBL.dtEnd.JSCal_dateToString() : DateTime.Now.AddDays(10).JSCal_dateToString();
            HF_dtPublicStart.Value = _currTBL.dtPublicStart.HasValue ? _currTBL.dtPublicStart.JSCal_dateToString() : DateTime.Now.JSCal_dateToString();
            HF_dtPublicEnd.Value = _currTBL.dtPublicEnd.HasValue ? _currTBL.dtPublicEnd.JSCal_dateToString() : DateTime.Now.AddDays(30).JSCal_dateToString();

            chk_is_active.Checked = _currTBL.is_active == 1;

            ltr_dtCreation.Text = _currTBL.dtCreation.formatCustom("#dd# #MM# #yy#", 1, DateTime.Now.formatCustom("#dd# #MM# #yy#", 1, ""));

            Fill_lang();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "_JSCal_Range", "setCal_Range();", true);
        }
        private void FillDataFromControls()
        {
            if (drp_estate.getSelectedValueInt(0)==0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    "alertError",
                                                    "alert('selezionare la struttura');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "_JSCal_Range", "setCal_Range();", true);
                return;
            }
            RNT_TB_ESTATE _est = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == drp_estate.getSelectedValueInt(0));
            if (_est == null)
                _est = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == drp_estate.getSelectedValueInt(0));
            if (_est == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    "alertError",
                                                    "alert('selezionare la struttura');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "_JSCal_Range", "setCal_Range();", true);
                return;
            }
            _currTBL = DC_RENTAL.RNT_TB_SPECIAL_OFFER.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new RNT_TB_SPECIAL_OFFER();
                _currTBL.dtCreation = DateTime.Now;
                DC_RENTAL.RNT_TB_SPECIAL_OFFER.InsertOnSubmit(_currTBL);
                DC_RENTAL.SubmitChanges();
                HF_id.Value = _currTBL.id.ToString();
            }
            _currTBL.pid_estate = drp_estate.getSelectedValueInt(0);
            _currTBL.class_type = drp_class_type.SelectedValue;
            _currTBL.pr_discount = txt_pr_discount.Text.ToDecimal();

            _currTBL.dtStart = HF_dtStart.Value.JSCal_stringToDate();
            _currTBL.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            _currTBL.dtPublicStart = HF_dtPublicStart.Value.JSCal_stringToDate();
            _currTBL.dtPublicEnd = HF_dtPublicEnd.Value.JSCal_stringToDate();

            _currTBL.is_active = chk_is_active.Checked ? 1 : 0;

            DC_RENTAL.SubmitChanges();
            Save_RL_langs();
            AppSettings.RNT_TB_SPECIAL_OFFERs = DC_RENTAL.RNT_TB_SPECIAL_OFFER.Where(x => x.dtPublicEnd >= DateTime.Now.AddMonths(-1) || x.dtEnd >= DateTime.Now.AddMonths(-1)).ToList();
            AppSettings.RNT_VIEW_SPECIAL_OFFERs = DC_RENTAL.RNT_VIEW_SPECIAL_OFFER.Where(x => x.dtPublicEnd >= DateTime.Now.AddMonths(-1) || x.dtEnd >= DateTime.Now.AddMonths(-1)).ToList();
            rntUtilsChnlAll.UpdateRates(_currTBL.pid_estate.objToInt32());
            //if (_est.bcomEnabled == 1)
              // BcomUpdate.BcomUpdate_start(_currTBL.pid_estate.objToInt32(), "rates");
            Response.Redirect(listPage, true);
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
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
                rlLang = new RNT_LN_SPECIAL_OFFER();
                rlLang.pid_special_offer = int.Parse(HF_id.Value);
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                curr_rl_langs.Add(rlLang);

            }
            rlLang.title = txt_title.Text;
            rlLang.summary = txt_summary.Text;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang()
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new RNT_LN_SPECIAL_OFFER();
            }
            txt_title.Text = rlLang.title;
            txt_summary.Text = rlLang.summary;
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_RENTAL.RNT_LN_SPECIAL_OFFERs.Where(x => x.pid_special_offer == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_special_offer = _currTBL.id;
                    DC_RENTAL.RNT_LN_SPECIAL_OFFERs.InsertOnSubmit(rl);
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
            DC_RENTAL.SubmitChanges();
        }
    }
}
