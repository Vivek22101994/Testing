using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.areariservataprop.common
{
    public partial class MP : System.Web.UI.MasterPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
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
