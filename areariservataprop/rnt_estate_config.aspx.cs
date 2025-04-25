using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.areariservataprop
{
    public partial class rnt_estate_config : ownerBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                int id = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id && x.pid_owner == OwnerAuthentication.CurrentID);
                if (_est != null)
                {
                    IdEstate = _est.id;
                    ltr_apartment.Text = (_est.pid_owner == 385 ? _est.inner_notes : _est.code) + " / " + "rif. " + _est.id;
                }
                else
                {
                    Response.Redirect("rnt_estate_list.aspx");
                }
            }
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
                UC_rnt_estate_navlinks1.IdEstate = value;
                LV.DataBind();
            }
        }


        protected void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null)
            {
            }
        }

        protected void FillDataFromControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            RNT_RL_ESTATE_CONFIG _conf = new RNT_RL_ESTATE_CONFIG();
            foreach (ListViewItem item in LV.Items)
            {
                Label lbl_id = (Label)item.FindControl("lbl_id");
                CheckBox chk = (CheckBox)item.FindControl("chk");
                var drp_options = (DropDownList)item.FindControl("drp_options");
                var lbl_cat = (Label)item.FindControl("lbl_inner_category");
                int _Id_conf = lbl_id.Text.ToInt32();
                if (lbl_cat.Text == "2")
                {
                    if (drp_options.SelectedIndex > 0)
                    {
                        _conf = new RNT_RL_ESTATE_CONFIG();
                        _conf.pid_config = _Id_conf;
                        _conf.pid_estate = IdEstate;
                        _conf.option_value = drp_options.SelectedValue;
                        DC_RENTAL.RNT_RL_ESTATE_CONFIG.InsertOnSubmit(_conf);
                    }
                }
                else if (chk.Checked)
                {
                    _conf = DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_config == _Id_conf && x.is_HomeAway==0);
                    if (_conf == null)
                    {
                        _conf = new RNT_RL_ESTATE_CONFIG();
                        _conf.pid_config = _Id_conf;
                        _conf.pid_estate = IdEstate;
                        DC_RENTAL.RNT_RL_ESTATE_CONFIG.InsertOnSubmit(_conf);
                    }
                }
                else
                {
                    _conf = DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_config == _Id_conf && x.is_HomeAway == 0);
                    if (_conf!=null)
                    {
                        DC_RENTAL.RNT_RL_ESTATE_CONFIG.DeleteOnSubmit(_conf);
                    }
                }
            }
            DC_RENTAL.SubmitChanges();
            Response.Redirect("rnt_estate_list.aspx");
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LDS.DataBind();
            LV.DataBind();
        }
        private List<RNT_RL_ESTATE_CONFIG> _currentList;
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (_currentList == null)
                _currentList = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.is_HomeAway==0).ToList();
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            CheckBox chk = (CheckBox)e.Item.FindControl("chk");
            var drp_options = (DropDownList)e.Item.FindControl("drp_options");
            var lbl_cat = (Label)e.Item.FindControl("lbl_inner_category");
            if (lbl_cat.Text == "2")
            {
                int id = int.Parse(lbl_id.Text);
                chk.Visible = false;
                List<RNT_VIEW_CONFIG_OPTION> _configOptList = DC_RENTAL.RNT_VIEW_CONFIG_OPTIONs.Where(item => item.pid_config == lbl_id.Text.ToInt32() && item.pid_lang == 1).ToList();
                drp_options.DataSource = _configOptList;
                drp_options.DataBind();
                drp_options.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
            else
            {
                chk.Checked = _currentList.SingleOrDefault(x => x.pid_config == lbl_id.Text.ToInt32()) != null;
                drp_options.Visible = false;
            }
        }
    }
}