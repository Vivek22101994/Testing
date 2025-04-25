using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_estate_position : System.Web.UI.UserControl
    {
        public string Position
        {
            get
            {
                return HF_position.Value;
            }
            set
            {
                HF_position.Value = value;
            }
        }
        public void RefreshList()
        {
            LV.Visible = true;
            pnl_content.Visible = false;
            LV.SelectedIndex = -1;
            LDS.DataBind();
            LV.DataBind();
        }

        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE_POSITION _currTBL;

        protected List<RNT_LN_ESTATE_POSITION> CURRENT_LANG_;
        protected List<RNT_LN_ESTATE_POSITION> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(RNT_LN_ESTATE_POSITION)).Cast<RNT_LN_ESTATE_POSITION>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<RNT_LN_ESTATE_POSITION>();

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
                LV.SelectedIndex = -1;
                LDS.DataBind();
                LV.DataBind();
                Bind_drp_zone();
                Bind_drp_estate();
            }
        }
        protected void Bind_drp_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1).OrderBy(x => x.title).ToList();
            drp_zone.DataSource = _list;
            drp_zone.DataTextField = "title";
            drp_zone.DataValueField = "id";
            drp_zone.DataBind();
            drp_zone.Items.Insert(0,new ListItem("- seleziona -","-1"));
        }
        protected void drp_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_drp_estate();
        }
        private void Bind_drp_estate()
        {
            List<RNT_TB_ESTATE> _list = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.pid_zone.HasValue && x.is_active == 1 && x.is_deleted != 1 && x.pid_zone == drp_zone.getSelectedValueInt(0)).OrderBy(x => x.code).ToList();
            drp_estate.DataSource = _list;
            drp_estate.DataTextField = "code";
            drp_estate.DataValueField = "id";
            drp_estate.DataBind();
            drp_estate.Items.Insert(0, new ListItem("- seleziona -", "-1"));
        }

        private void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE_POSITION.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                HF_id.Value = "0";
                _currTBL = new RNT_TB_ESTATE_POSITION();
            }
            RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_est != null)
            {
                drp_zone.setSelectedValue(_est.pid_zone.ToString());
                Bind_drp_estate();
            }
            drp_estate.setSelectedValue(_currTBL.pid_estate.ToString());
            drp_class_type.setSelectedValue(_currTBL.css_class);
            CURRENT_LANG = DC_RENTAL.RNT_LN_ESTATE_POSITION.Where(x => x.pid_position == _currTBL.id).ToList();
            HF_lang.Value = "1";
            Fill_lang();
            LV_langs.DataBind();
          
        }

        protected void FillDataFromControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE_POSITION.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                int _sequence = 1;
                List<RNT_TB_ESTATE_POSITION> _allCollection =
                    DC_RENTAL.RNT_TB_ESTATE_POSITION.Where(x => x.position == Position).OrderBy(x => x.sequence).ToList();
                if (_allCollection.Count != 0)
                {
                    _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                }
                _currTBL = new RNT_TB_ESTATE_POSITION();
                _currTBL.position = Position;
                _currTBL.sequence = _sequence;
                DC_RENTAL.RNT_TB_ESTATE_POSITION.InsertOnSubmit(_currTBL);
            }
            _currTBL.pid_estate = drp_estate.getSelectedValueInt(0);
            _currTBL.css_class = drp_class_type.SelectedValue;
            DC_RENTAL.SubmitChanges();
            Save_RL_langs();
            AppSettings.RNT_VIEW_ESTATE_POSITION = maga_DataContext.DC_RENTAL.RNT_VIEW_ESTATE_POSITION.ToList();
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            LV.Visible = true;
            pnl_content.Visible = false;
            LDS.DataBind();
            LV.DataBind();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LV.Visible = true;
            pnl_content.Visible = false;
        }

        protected void lnk_new_Click(object sender, EventArgs e)
        {
            LV.Visible = false;
            pnl_content.Visible = true;
            HF_id.Value = "0";
            FillControls();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_pid_estate = e.Item.FindControl("lbl_pid_estate") as Label;
            if (e.CommandName == "move_up")
            {
                var _curr = DC_RENTAL.RNT_TB_ESTATE_POSITION.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                var upper_block_list = DC_RENTAL.RNT_TB_ESTATE_POSITION.Where(x => x.sequence < _curr.sequence && x.position == HF_position.Value).OrderByDescending(x => x.sequence);
                if (upper_block_list.Count() != 0)
                {
                    var upper_block = upper_block_list.First();
                    _curr.sequence = upper_block.sequence;
                    upper_block.sequence = upper_block.sequence + 1;
                    DC_RENTAL.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();
                }
                else if (_curr.sequence > 1)
                {
                    _curr.sequence = 1;
                    DC_RENTAL.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();

                }
            }
            if (e.CommandName == "move_down")
            {
                var _curr = DC_RENTAL.RNT_TB_ESTATE_POSITION.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                var down_block_list = DC_RENTAL.RNT_TB_ESTATE_POSITION.Where(x => x.sequence > _curr.sequence && x.position == HF_position.Value).OrderBy(x => x.sequence);
                if (down_block_list.Count() != 0)
                {
                    var down_block = down_block_list.First();
                    _curr.sequence = down_block.sequence;
                    down_block.sequence = down_block.sequence - 1;
                    DC_RENTAL.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();
                }
            }
            if (e.CommandName == "elimina")
            {
                var _curr = DC_RENTAL.RNT_TB_ESTATE_POSITION.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (_curr != null)
                {
                    DC_RENTAL.RNT_TB_ESTATE_POSITION.DeleteOnSubmit(_curr);
                    DC_RENTAL.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();
                }
            }
            if (e.CommandName == "change")
            {
                LV.Visible = false;
                pnl_content.Visible = true;
                HF_id.Value = lbl_id.Text;
                FillControls();
            }
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }

        protected void LV_DataBound(object sender, EventArgs e)
        {
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
                rlLang = new RNT_LN_ESTATE_POSITION();
                rlLang.pid_position = int.Parse(HF_id.Value);
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_LANG.Add(rlLang);
            }
            rlLang.title = txt_title.Text;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang()
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new RNT_LN_ESTATE_POSITION();
            }
            txt_title.Text = rlLang.title;
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_RENTAL.RNT_LN_ESTATE_POSITION.Where(x => x.pid_position == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_position = _currTBL.id;
                    DC_RENTAL.RNT_LN_ESTATE_POSITION.InsertOnSubmit(rl);
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

    }
}