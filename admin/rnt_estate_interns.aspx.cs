using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class rnt_estate_interns : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UC_InternMain.IdEstate = Request.QueryString["id"].objToInt32();
                UC_rnt_estate_navlinks1.IdEstate = Request.QueryString["id"].objToInt32();
            }
        }
    }
}