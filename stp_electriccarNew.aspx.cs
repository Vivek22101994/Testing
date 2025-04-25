using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome
{
    public partial class stp_electriccarNew : contStpBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "stp";
            int id = Request.QueryString["id"].ToInt32();
            if (id != 0)
                base.PAGE_REF_ID = id;
            else
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "stp_electriccar", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Fill_data();
            }
        }
        protected void Fill_data()
        {
            if (currStp != null)
            {
                ltr_meta_description.Text = currStp.meta_description;
                ltr_meta_keywords.Text = currStp.meta_keywords;
                ltr_meta_title.Text = currStp.meta_title;
                ltr_title.Text = currStp.title;
                ltr_sub_title.Text = currStp.sub_title;
                ltr_description.Text = currStp.description;
            }
        }
    }
}