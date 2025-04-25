using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.uc
{
    public partial class UC_footer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HF_pid_lang.Value = CurrentLang.ID.ToString();
            if (!IsPostBack)
            {
                System.Web.HttpBrowserCapabilities browser = Request.Browser;
                if (browser.Browser == "IE" && browser.MajorVersion < 7)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorBrowser", "showBrowserError();", true);
                }
                mainBasePage m = (mainBasePage)this.Page;
                PH_shinystat.Visible = m != null && m.PAGE_REF_ID == 1 && m.PAGE_TYPE == "stp";
            }
        }
    }
}