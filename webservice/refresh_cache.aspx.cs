using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.webservice
{
    public partial class refresh_cache : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string _action = "" + Request.QueryString["action"];
                RiV_WS.refreshCache(_action);
                if (_action == "rnt_estate" || _action=="all")
                    AppSettings._refreshCache_RNT_ESTATEs();
                if (_action == "rnt_comment" || _action == "all")
                    AppSettings.RNT_TBL_ESTATE_COMMENTs = null;
                //if (_action == "rnt_reservation" || _action == "all")
                //    AppSettings.RNT_TBL_RESERVATION = null;
                if (_action == "def_sys_setting" || _action == "all")
                    AppSettings.DEF_SYS_SETTINGs = null;
                Response.Write("ok");
                Response.End();
                return;
            }
        }
    }
}
