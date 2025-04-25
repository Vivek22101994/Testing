
using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateChnlAirbnb_main : adminBasePage
    {
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
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                ltr_apartment.Text = currEstate.code;
                ucNav.IdEstate = IdEstate;

                using (DCmodRental dc = new DCmodRental())
                {
                    dbRntChnlAirbnbEstateTBL objAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == IdEstate);
                    if (objAirbnbEstate != null)
                    {

                    }
                }
            }
        }

        
    }
}