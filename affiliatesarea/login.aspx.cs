using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.affiliatesarea
{
    public partial class login : mainBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["logout"] == "true")
                {
                    agentAuth.CurrentID = 0;
                    Response.Redirect("login.aspx");
                }
                if (!String.IsNullOrEmpty(Request.QueryString["referer"]))
                {
                    HF_referer.Value = Server.UrlDecode(Request.QueryString["referer"]);
                }
                ((masterPage)this.Page.Master).hideMenu();
            }
        }
        protected void lnk_login_Click(object sender, EventArgs e)
        {
            if (agentAuth.Auth(txtUser.Text, txtPwd.Text))
            {
                Response.Redirect(HF_referer.Value);
            }
            else
            {
                txtPwd.Text = "";
                txtUser.Text = "";
                pnl_loginError.Visible = true;
            }
        }
        protected void lnk_pwdRecovery_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntAgentTBL user = dc.dbRntAgentTBLs.FirstOrDefault(x =>(x.authUsr == txt_pwdRecoveryEmail.Text || x.contactEmail == txt_pwdRecoveryEmail.Text) && x.isActive == 1);
                if (user != null)
                {
                    user.authPwd = string.Empty.createPassword(8, true, true, false);
                    rntUtils.agent_mailPwdRecovery(user.id);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"An E-mail with your new password was sent to '" + user.contactEmail + "'.\", 340, 110);", true);
                    return;
                }
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Sorry, we couldn't find your User Name or E-mail.\", 340, 110);", true);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://" + Request.Url.Authority);
        }
    }
}
