using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class rnt_estate_details_save : adminBasePage
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


                txt_nights_min.Text = _currTBL.nights_min.objToInt32()+"";
                txt_nights_minVHSeason.Text = _currTBL.nights_minVHSeason.objToInt32() + "";
            }
        }

        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
            if (_currTBL == null)
                Response.Redirect(listPage);

            _currTBL.nights_min = txt_nights_min.Text.objToInt32();
            _currTBL.nights_minVHSeason = txt_nights_minVHSeason.Text.objToInt32();
            DC_RENTAL.SubmitChanges();
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
            if (_currTBL == null)
                Response.Redirect(listPage);

            _currTBL.nights_min = txt_nights_min.Text.objToInt32();
            DC_RENTAL.SubmitChanges();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {

        }
        protected void filldata()
        {

        }

    }
}