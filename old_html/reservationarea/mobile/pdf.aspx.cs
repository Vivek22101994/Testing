using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.reservationarea.mobile
{
    public partial class pdf : basePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ucHeader.PageTitle = "Voucher/" + contUtils.getLabel("lblInvoice");
            }
        }
    }
}