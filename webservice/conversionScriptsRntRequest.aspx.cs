using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.webservice
{
    public partial class conversionScriptsRntRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            if (IsPostBack || Request.Url.AbsoluteUri.ToLower().Contains("http://localhost") || Request.Url.AbsoluteUri.ToLower().Contains(".magadesign.net") || Request.Url.AbsoluteUri.ToLower().Contains(".dev.magarental.com")) return;
            var DC_RENTAL = maga_DataContext.DC_RENTAL;
            var currTbl = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
            if (currTbl == null || currTbl.conversionScriptsShown.objToInt32() != 0) return;
            currTbl.conversionScriptsShown = 1;
            DC_RENTAL.SubmitChanges();
            this.Visible = true;
        }
    }
}