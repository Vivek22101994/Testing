using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;

namespace RentalInRome.common
{
    public partial class proxy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string proxyURL = Request.QueryString["u"].urlDecode();
            if (!string.IsNullOrEmpty(proxyURL))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(proxyURL);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode.ToString().ToLower() == "ok")
                {
                    string contentType = response.ContentType;
                    Stream content = response.GetResponseStream();
                    StreamReader contentReader = new StreamReader(content);
                    Response.Clear();
                    Response.ContentType = contentType;
                    Response.Write(contentReader.ReadToEnd());
                }
            }
        }
    }
}