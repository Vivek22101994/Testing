using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;

namespace RentalInRome.affiliatesarea
{
    public partial class Terms : agentBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (agentAuth.hasAcceptedContract == 1)
                hideAccept();
            else
                ((masterPage)this.Page.Master).hideMenu();
        }
        protected void hideAccept()
        {
            lbl_DescrizioneNellaPaginaAccettazioneContratto.Visible = false;
            PH_1.Visible = false;
            PH_2.Visible = false;
            lnkAccept.Visible = false;
        }
        protected void lnkAccept_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntAgentTBL currTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentAuth.CurrentID);
                if (currTBL == null) return;
                currTBL.hasAcceptedContract = 1;
                dc.SaveChanges();
                agentAuth.hasAcceptedContract = 1;
                Response.Redirect("/affiliatesarea/");
            }
        }

    }
}
