using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ModAuth.admin.modAuth
{
    public partial class errorEcoLogDett : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (DCmodAuth dc = new DCmodAuth())
                {
                    Guid uid;
                    if(!Guid.TryParse(Request.QueryString["id"], out uid))
                        return;
                    var item = dc.dbAuthErrorEcoLOGs.SingleOrDefault(x=>x.uid==uid);
                    if(item==null)return;
                    txt_body.Text = item.errorContent;
                    ltrDate.Text = item.logDateTime+"";
                    ltrIP.Text = item.logIp;
                    ltrUrl.Text = item.logUrl;
                }
            }
        }
    }
}