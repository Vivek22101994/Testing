using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.pdfgenerator
{
    public partial class generator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string _url = Request.QueryString["url"].urlDecode();
            string _filename = Request.QueryString["filename"].urlDecode();
            string _serverPath = Request.QueryString["serverpath"].urlDecode();
            if (String.IsNullOrEmpty(_serverPath))
                CommonUtilities.downloadPdfFromUrl(_url, _filename, 0.3f, 0.3f, 0.3f, 0.3f);
            else
            {
                Response.Write(CommonUtilities.savePdfFromUrl(_url, _serverPath, 0.3f, 0.3f, 0.3f, 0.3f));
            }
        }
    }
}
