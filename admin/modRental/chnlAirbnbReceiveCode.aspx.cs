using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental
{
    public partial class chnlAirbnbReceiveCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ErrorLog.addLog("", "authcode1", "open");

                if (Request.QueryString["code"] != null)
                {
                    ErrorLog.addLog("", "authcode", Request.QueryString["code"] + "");

                    //store in table athentication code
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var currAuthCode = dc.dbRntChnlAirbnbAuthenticationCodes.SingleOrDefault(x => x.hostId == "149513342");
                        if (currAuthCode == null)
                        {
                            currAuthCode = new dbRntChnlAirbnbAuthenticationCode();
                            currAuthCode.hostId = "149513342";
                            dc.Add(currAuthCode);
                            dc.SaveChanges();
                        }
                        currAuthCode.code = Request.QueryString["code"] + "";
                        dc.SaveChanges();
                        Response.Redirect("/admin/modRental/AddAirbnbHost.aspx");
                    }
                }
                else if (Request.QueryString["error_description"] != null)
                {
                    ErrorLog.addLog("", "authcode Error", Request.QueryString["error_description"] + "");
                }
            }
        }
    }
}
