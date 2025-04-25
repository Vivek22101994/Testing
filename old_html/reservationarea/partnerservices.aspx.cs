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
    public partial class partnerservices : basePage
    {
        private magaRental_DataContext DC_RENTAL;
        private magaUser_DataContext DC_USER;
        private RNT_TBL_RESERVATION tmpReservationTBL;
        public RNT_TBL_RESERVATION currReservationTBL
        {
            get
            {
                if (tmpReservationTBL == null)
                {
                    DC_RENTAL = maga_DataContext.DC_RENTAL;
                    tmpReservationTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
                }
                return tmpReservationTBL ?? new RNT_TBL_RESERVATION();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                tmpReservationTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
                if (tmpReservationTBL == null)
                {
                    Response.Redirect("/reservationarea/login.aspx", true);
                    return;
                }
            }
        }
    }
}