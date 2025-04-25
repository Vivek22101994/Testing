using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["logout"] == "true")
                {
                    UserAuthentication.CurrentUserID = 0;
                    UserAuthentication.CURRENT_USR_ADMIN_CONFIG = null;
                    Response.Redirect("login.aspx");
                }
                if (!String.IsNullOrEmpty(Request.QueryString["referer"]))
                {
                    HF_referer.Value = Server.UrlDecode(Request.QueryString["referer"]);
                }
                //magaCommon_DataContext _dc = maga_DataContext.DC_COMMON;
                //LOG_ADMIN_CONNECTION log = new LOG_ADMIN_CONNECTION();
                //_dc.LOG_ADMIN_CONNECTIONs.DeleteAllOnSubmit(_dc.LOG_ADMIN_CONNECTIONs.Where(x => x.date_connection < DateTime.Now.AddMonths(-1)));
                //_dc.SubmitChanges();

            }
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (UserAuthentication.Auth(txtUser.Text, txtPwd.Text))
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
