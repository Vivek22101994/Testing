using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{
    public partial class transfer : basePage
    {
        private magaRental_DataContext DC_RENTAL;
        private magaUser_DataContext DC_USER;
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
            }
        }
    }
}