using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFileManager.wfm
{
    public partial class MP : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setParentBackgroundColor", "setParentBackgroundColor();", true);
            }
        }
    }
}
