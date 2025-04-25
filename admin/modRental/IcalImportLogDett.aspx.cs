using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ModRental.admin.modRental
{
    public partial class IcalImportLogDett : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    Guid uid;
                    if(!Guid.TryParse(Request.QueryString["id"], out uid))
                        return;
                    var item = dc.dbRntIcalImportLOGs.SingleOrDefault(x=>x.uid==uid);
                    if(item==null)return;
                    ltrDate.Text = item.logDateTime + "";
                    ltr_errorCount.Text = item.errorCount+"";
                    ltr_iCalType.Text = "iCal"+item.iCalType;
                    ltr_iCalUrl.Text = item.iCalUrl;
                    LV.DataSource = dc.dbRntIcalImportErrorLOGs.Where(x => x.logUid == uid).ToList();
                    LV.DataBind();
                }
            }
        }
    }
}