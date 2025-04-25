using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.affiliatesarea
{
    public partial class UC_sx : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillData();
            }
        }
        protected void fillData()
        {
            ltr_nameFull.Text = agentAuth.CurrentName;
        }
    }
}