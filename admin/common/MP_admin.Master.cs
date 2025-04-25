using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.common
{
    public partial class MP_admin : System.Web.UI.MasterPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setToolTip", "setToolTip();", true);
            PH_admin_menu.Visible = UserAuthentication.CurrentUserID == 2;
            if (!IsPostBack)
                rcbMenu.Visible = rcbChrome.Visible = Request.QueryString["nomenu"] != "true";
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "hiddenbeforload_show", "$(\".hiddenbeforload\").show();", true);
            }
        }
        protected string drawMenuItem(string permission, string page, string label)
        {
            string _str = "";
            if (UserAuthentication.hasPermission(permission))
                _str = "<a href=\"" + page + "\">" + label + "</a>";
            return _str;
        }
    }
}
