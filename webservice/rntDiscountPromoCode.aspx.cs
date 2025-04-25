using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;

namespace RentalInRome.webservice
{
    public partial class rntDiscountPromoCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/html";
                using (DCmodRental dc = new DCmodRental())
                {
                    var promoTbl = dc.dbRntDiscountPromoCodeTBLs.FirstOrDefault(x => x.code.ToLower().Trim() == (Request["promocode"] + "").Trim().ToLower());
                    if (promoTbl != null)
                    {
                        Response.Write(promoTbl.discountAmount.objToDecimal().ToString("N2"));
                    }
                }
                Response.End();
            }
        }
    }
}
