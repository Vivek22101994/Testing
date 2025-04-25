using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental.ucEstateDett
{
    public partial class EstateInternsMain : System.Web.UI.UserControl
    {
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
                EstateInternsBedroom.pidInternsType = "Bedroom";
                EstateInternsBedroom.IdEstate = value;

                EstateInternsBathroom.pidInternsType = "Bathroom";
                EstateInternsBathroom.IdEstate = value;

                EstateInternsKitchen.pidInternsType = "Kitchen";
                EstateInternsKitchen.IdEstate = value;

                EstateInternsLivingRoom.pidInternsType = "Livingroom";
                EstateInternsLivingRoom.IdEstate = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}