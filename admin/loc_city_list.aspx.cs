using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class loc_city_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "loc_city";
        }
        private magaLocation_DataContext DC_LOCATION;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_LOCATION = maga_DataContext.DC_LOCATION;
            if (!IsPostBack)
            {
            }
        }
        protected void lnk_nuovo_Click(object sender, EventArgs e)
        {
        }
    }
}