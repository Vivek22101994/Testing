using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_estate_point : adminBasePage
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
                    HF_pid_city.Value = _est.pid_city.objToInt32() == 0 ? "1" : _est.pid_city.objToInt32().ToString();
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
            _currentList = DC_RENTAL.RNT_RL_ESTATE_POINTs.Where(x => x.pid_estate == IdEstate).ToList();
            DC_RENTAL.RNT_RL_ESTATE_POINTs.DeleteAllOnSubmit(_currentList);
            DC_RENTAL.SubmitChanges();
            foreach (ListViewItem Item in LV.Items)
            {
                Label lbl_id = Item.FindControl("lbl_id") as Label;
                CheckBox chk = Item.FindControl("chk") as CheckBox;
                TextBox txt = Item.FindControl("txt") as TextBox;
                if(!chk.Checked) continue;
                RNT_RL_ESTATE_POINT _curr = new RNT_RL_ESTATE_POINT();
                _curr.pid_point = lbl_id.Text.ToInt32();
                _curr.pid_estate = IdEstate;
                _curr.distance = txt.Text;
                DC_RENTAL.RNT_RL_ESTATE_POINTs.InsertOnSubmit(_curr);
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

        private List<RNT_RL_ESTATE_POINT> _currentList;
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if(_currentList==null)
                _currentList = DC_RENTAL.RNT_RL_ESTATE_POINTs.Where(x => x.pid_estate == IdEstate).ToList();
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            CheckBox chk = e.Item.FindControl("chk") as CheckBox;
            TextBox txt = e.Item.FindControl("txt") as TextBox;
            RNT_RL_ESTATE_POINT _curr = _currentList.SingleOrDefault(x => x.pid_point == lbl_id.Text.ToInt32());
            if(_curr!=null)
            {
                chk.Checked = true;
                txt.Text = _curr.distance;
            }
            else
            {
                chk.Checked = false;
                txt.Text = "";
            }
        }
    }
}