using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental
{
    public partial class ChnlAirbnbRequestLogDett : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
                if (!IsPostBack)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        Guid uid;
                        if (!Guid.TryParse(Request.QueryString["id"], out uid))
                            return;
                        var item = dc.dbRntChnlAirbnbRequestLOGs.SingleOrDefault(x => x.uid == uid);
                        if (item == null) return;
                        txt_requestContent.Text = item.requestContent;
                        txt_responseContent.Text = item.responseContent;
                        ltrDate.Text = item.logDateTime + "";
                        ltr_requestType.Text = item.requestType;
                        //ltr_requesUrl.Text = item.requesUrl;
                        ltr_requestComments.Text = item.requestComments;
                    }
               
            }
        }
    }
}