using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental.uc
{
    public partial class ucEstateChnlAtraveoTab : System.Web.UI.UserControl
    {
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
                {
                    var currHomeaway = dcChnl.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == value);
                    if (currHomeaway == null)
                    {
                        RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == value);
                        if (currEstate != null)
                            ChnlAtraveoUtils.updateEstateFromMagarental(currEstate);
                    }
                } 
                HF_IdEstate.Value = value.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
    }
}