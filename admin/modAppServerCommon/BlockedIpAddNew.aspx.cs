using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace ModAppServerCommon
{
    public partial class BlockedIpAddNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (DCmodAppServerCommon dc = new DCmodAppServerCommon())
                {
                    string pattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
                    if (string.IsNullOrEmpty(Request.QueryString["ip"]) || Request.QueryString["ip"].Trim() == "" || !Regex.IsMatch(Request.QueryString["ip"].Trim(), pattern))
                    {
                        Response.Write("notvalid");
                        return;
                    }
                    var tmp = dc.dbUtlsBlockedIpLSTs.SingleOrDefault(x => x.ip == Request.QueryString["ip"].Trim());
                    if (tmp != null)
                    {
                        Response.Write("exist");
                        return;
                    }
                    var item = new dbUtlsBlockedIpLST();
                    item.ip = Request.QueryString["ip"].Trim();
                    dc.Add(item);
                    dc.SaveChanges();
                    BlockedIpTool.CurrList = dc.dbUtlsBlockedIpLSTs.ToList();
                    Response.Write(item.ip);
                }
            }
        }
    }
}