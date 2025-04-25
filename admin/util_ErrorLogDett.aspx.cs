using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class util_ErrorLogDett : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LogList _list = new LogList(Request.QueryString["dt"]);
                LogItem item = _list.Items.SingleOrDefault(x => x.ID == Request.QueryString["id"]);
                if (item == null) return;
                txt_body.Text = item.Value;
                ltrDate.Text = item.Date.JSCal_stringToDateTime().ToString();
                ltrIP.Text = item.IP;
                ltrUrl.Text = item.Url;
            }
        }
    }
}