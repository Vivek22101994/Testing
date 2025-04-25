using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class loc_point_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "loc_point";
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
