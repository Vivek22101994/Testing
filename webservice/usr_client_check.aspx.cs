using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class usr_client_check : System.Web.UI.Page
    {
        private magaUser_DataContext DC_USER;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "text/xml";
            XDocument _resource = new XDocument();
            XElement rootElement = new XElement("list");
            XElement record = new XElement("item");
            DC_USER = maga_DataContext.DC_USER;
            string _email = Request.QueryString["email"];
            USR_TBL_CLIENT _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.contact_email == _email);
            if (_client != null)
            {
                record.Add(new XElement("state", "exist"));
                record.Add(new XElement("id", _client.id));
            }
            else
            {
                record.Add(new XElement("state", "notexist"));
                record.Add(new XElement("id", "0"));
            }
            _resource.Add(rootElement);
            rootElement.Add(record);
            Response.Write(_resource.ToString());
        }
    }
}
