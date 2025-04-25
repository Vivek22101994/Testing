using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_residence_config : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_RESIDENCE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                int id = Request.QueryString["id"].ToInt32();
                RNT_TB_RESIDENCE _est = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == id);
                if (_est != null)
                {
                    IdResidence = _est.id;
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                }
                else
                {
                    Response.Redirect("rnt_residence_list.aspx");
                }
            }
        }
        public int IdResidence
        {
            get
            {
                return HF_IdResidence.Value.ToInt32();
            }
            set
            {
                HF_IdResidence.Value = value.ToString();
                UC_rnt_residence_navlinks1.IdResidence = value;
                LV_config.DataBind();
            }
        }


        protected void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == IdResidence);
            if (_currTBL == null)
            {
            }
        }

        protected void FillDataFromControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == IdResidence);
            if (_currTBL == null) return;
            RNT_RL_RESIDENCE_CONFIG _conf = new RNT_RL_RESIDENCE_CONFIG();
            foreach (ListViewItem item in LV_config.Items)
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
                        _conf = new RNT_RL_RESIDENCE_CONFIG();
                        _conf.pid_config = _Id_conf;
                        _conf.pid_residence = IdResidence;
                        _conf.option_value = drp_options.SelectedValue;
                        DC_RENTAL.RNT_RL_RESIDENCE_CONFIGs.InsertOnSubmit(_conf);
                    }
                }
                else if (chk.Checked)
                {
                    _conf = DC_RENTAL.RNT_RL_RESIDENCE_CONFIGs.SingleOrDefault(x => x.pid_residence == IdResidence && x.pid_config == _Id_conf);
                    if (_conf == null)
                    {
                        _conf = new RNT_RL_RESIDENCE_CONFIG();
                        _conf.pid_config = _Id_conf;
                        _conf.pid_residence = IdResidence;
                        DC_RENTAL.RNT_RL_RESIDENCE_CONFIGs.InsertOnSubmit(_conf);
                    }
                }
                else
                {
                    _conf = DC_RENTAL.RNT_RL_RESIDENCE_CONFIGs.SingleOrDefault(x => x.pid_residence == IdResidence && x.pid_config == _Id_conf);
                    if (_conf != null)
                    {
                        DC_RENTAL.RNT_RL_RESIDENCE_CONFIGs.DeleteOnSubmit(_conf);
                    }
                }
            }
            DC_RENTAL.SubmitChanges();
            Response.Redirect("rnt_residence_list.aspx");
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LDS_config.DataBind();
            LV_config.DataBind();
        }
        protected void LV_config_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
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
                chk.Checked = DC_RENTAL.RNT_RL_RESIDENCE_CONFIGs.SingleOrDefault(x => x.pid_residence == IdResidence && x.pid_config == lbl_id.Text.ToInt32()) != null;
                drp_options.Visible = false;
            }
        }
    }
}