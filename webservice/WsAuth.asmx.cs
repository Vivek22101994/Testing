using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Services;
using ModContent;
using System.Text;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    /// <summary>
    /// Descrizione di riepilogo per WS
    /// </summary>
    [WebService(Namespace = "http://www.rentalinrome.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class WsAuth : System.Web.Services.WebService
    {
        public class AuthHeader : SoapHeader
        {
            public string Username;
            public string Password;
        }
        public AuthHeader Authentication;
        private bool IsAuthOK(string login, string pwd)
        {
            USR_ADMIN user = maga_DataContext.DC_USER.USR_ADMIN.SingleOrDefault(x => x.login == login && x.password == pwd && x.is_active == 1 && x.is_deleted == 0);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "Test di autenticazione")]
        public bool Authentication_Test()
        {
            return IsAuthOK(Authentication.Username, Authentication.Password);
        }
    }
}
