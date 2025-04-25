using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;

namespace RentalInRome.affiliatesarea
{
    public partial class masterPage : System.Web.UI.MasterPage
    {
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public string CURRENT_SESSION_ID
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                return m.CURRENT_SESSION_ID;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (agentAuth.CurrentID != 0)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentAuth.CurrentID);
                        if (agentTBL != null)
                        {
                            ltr_nameFull.Text = agentTBL.nameCompany;
                            PH_agentInfo.Visible = true;
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setToolTip", "setToolTip();", true);
        }
        public void hideMenu()
        {
            PH_menu.Visible = false;
        }
    }
}