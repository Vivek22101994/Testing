using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.areariservataprop
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["logout"] == "true")
                {
                    OwnerAuthentication.CurrentID = 0;
                    Response.Redirect("login.aspx");
                }
                if (!String.IsNullOrEmpty(Request.QueryString["referer"]))
                {
                    HF_referer.Value = Server.UrlDecode(Request.QueryString["referer"]);
                }
            }
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (OwnerAuthentication.Auth(txtUser.Text, txtPwd.Text))
            {
                Response.Redirect(HF_referer.Value);
            }
            else
            {
                txtPwd.Text = "";
                txtUser.Text = "";
                lblError.Visible = true;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://" + Request.Url.Authority);
        }
    }
}
