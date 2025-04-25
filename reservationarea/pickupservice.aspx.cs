using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{
    public partial class pickupservice : basePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (resUtils.CurrentIdReservation_gl != 0 && resUtils.CurrentIdReservation_gl < 150000)
            {
                Response.Redirect("/reservationarea/arrivaldeparture.aspx", true);
                return;
            }
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                fillData();
            }
        }
        protected void fillData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
        }
        protected void saveData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
        }

    }
}