using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_estate_config : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                lnk_save.Visible = lnk_annulla.Visible = UserAuthentication.hasPermission("rnt_estate", "can_edit");
                int id = Request.QueryString["id"].ToInt32();
                RNT_VIEW_ESTATE _est = DC_RENTAL.RNT_VIEW_ESTATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
                if (_est != null)
                {
                    IdEstate = _est.id;
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
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
                        _conf.is_HomeAway = 0;                
                        _conf.option_value = drp_options.SelectedValue;
                        DC_RENTAL.RNT_RL_ESTATE_CONFIG.InsertOnSubmit(_conf);
                    }
                }
                else if (chk.Checked)
                {
                    _conf = DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_config == _Id_conf &&  x.is_HomeAway==0);
                    if (_conf == null)
                    {
                        _conf = new RNT_RL_ESTATE_CONFIG();
                        _conf.pid_config = _Id_conf;
                        _conf.pid_estate = IdEstate;
                        _conf.is_HomeAway = 0;
                        DC_RENTAL.RNT_RL_ESTATE_CONFIG.InsertOnSubmit(_conf);
                    }
                }
                else
                {
                    _conf = DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_config == _Id_conf && x.is_HomeAway==0);
                    if (_conf!=null)
                    {
                        DC_RENTAL.RNT_RL_ESTATE_CONFIG.DeleteOnSubmit(_conf);
                    }
                }
            }

            foreach (ListViewItem item in LV_HA.Items)
            {
                Label lbl_HA_id = (Label)item.FindControl("lbl_HA_id");
                CheckBox chk_HA = (CheckBox)item.FindControl("chk_HA");
                int _Id_conf = lbl_HA_id.Text.ToInt32();
                DropDownList drp_count = (DropDownList)item.FindControl("drp_count");
                
                if (chk_HA.Checked)
                {
                    _conf = DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_config == _Id_conf && x.is_HomeAway==1);
                    if (_conf == null)
                    {
                        _conf = new RNT_RL_ESTATE_CONFIG();
                        _conf.pid_config = _Id_conf;
                        _conf.pid_estate = IdEstate;
                        _conf.is_HomeAway = 1;
                        _conf.Count = Convert.ToInt32(drp_count.SelectedValue);
                        DC_RENTAL.RNT_RL_ESTATE_CONFIG.InsertOnSubmit(_conf);
                    }
                    else
                    {
                        _conf.Count = Convert.ToInt32(drp_count.SelectedValue);
                    }
                }
                else
                {
                    _conf = DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_config == _Id_conf && x.is_HomeAway == 1);
                    if (_conf != null)
                    {
                        DC_RENTAL.RNT_RL_ESTATE_CONFIG.DeleteOnSubmit(_conf);
                    }
                }
            }
           
            DC_RENTAL.SubmitChanges();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            //Response.Redirect("rnt_estate_list.aspx");
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
                _currentList = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate).ToList();
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
                chk.Checked = _currentList.SingleOrDefault(x => x.pid_config == lbl_id.Text.ToInt32() && x.is_HomeAway==0) != null;
               
                drp_options.Visible = false;
            }
        }
        protected void LV_HA_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_HA_id = (Label)e.Item.FindControl("lbl_HA_id");
            CheckBox chk_HA = (CheckBox)e.Item.FindControl("chk_HA");
            DropDownList drp_count = (DropDownList)e.Item.FindControl("drp_count");
            RNT_RL_ESTATE_CONFIG _currentHA = DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == IdEstate &&  x.pid_config == lbl_HA_id.Text.ToInt32() && x.is_HomeAway == 1 );
            if (_currentHA != null)
            {
                chk_HA.Checked = true;
                if(_currentHA.Count!=null)
                drp_count.SelectedValue = Convert.ToString(_currentHA.Count);

            }
            //chk_HA.Checked = _currentList.SingleOrDefault(x => x.pid_config == lbl_HA_id.Text.ToInt32() && x.is_HomeAway == 1) != null;

        }
    }
}