using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.common
{
    public partial class SessionCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserAuthentication.CurrentUserID == 0 && !Request.Url.AbsoluteUri.Contains("http://localhost"))
                {
                    Response.Write("ko");
                }
                else
                {
                    Response.Write("ok");
                }
            }
        }
    }
}
