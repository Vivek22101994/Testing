using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace ModAppServerCommon
{
    public partial class BlockedIpGetList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Write("" + BlockedIpTool.CurrList.Select(x => x.ip).ToList().listToString("|"));
            }
        }
    }
}