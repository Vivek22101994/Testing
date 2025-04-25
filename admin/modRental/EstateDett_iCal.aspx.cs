using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class EstateDett_iCal : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE currTBL;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);

                if (_est!=null)
                {
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                    UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                }
                else
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                }
            }
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
        }

    }
}