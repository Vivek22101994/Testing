using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Services;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    /// <summary>
    /// Descrizione di riepilogo per WS
    /// </summary>
    [WebService(Namespace = "http://www.rentalinvenice.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class WS : System.Web.Services.WebService
    {
        public class AuthHeader : SoapHeader
        {
            public string Username;
            public string Password;
        }
        public AuthHeader Authentication;

        private bool IsAuthOK(string user, string psw)
        {
            return (user == "Rir" && psw == "Fer90PLkir3W£_,MR");
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

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "rntReservation_onChange")]
        public void rntReservation_onChange(int pidCityCaller, long id)
        {
            RNT_TBL_RESERVATION currTBL = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == id);
            if (currTBL != null)
                rntUtils.rntReservation_onChange(currTBL, pidCityCaller);
        }
    }
}
