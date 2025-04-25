using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using RentalInRome.data;

namespace RentalInRome.areariservataprop
{
    public class ownerUtils
    {
    }
    public class OwnerAuthentication
    {
        public static bool Auth(string login, string pwd)
        {
            USR_TBL_OWNER user = maga_DataContext.DC_USER.USR_TBL_OWNER.SingleOrDefault(x => x.login == login && x.password == pwd && x.is_active == 1 && x.is_deleted == 0);
            if (user != null)
            {
                HttpContext.Current.Session.Timeout = 60;
                CurrentID = user.id;
                CurrentName = user.name_honorific + " " + user.name_full;
                //string ip_name = HttpContext.Current.Request.ServerVariables.Get("REMOTE_HOST");
                //string host_name = "";
                //AdminUtilities.log_addConnection(1, "Login area amministrativa (login.aspx)", ip_name, host_name);
                return true;
            }
            return false;
        }
        public static int CurrentID
        {
            get { if (HttpContext.Current.Session["CurrentID_ownerRentalInRome"] != null)return (int)HttpContext.Current.Session["CurrentID_ownerRentalInRome"]; else return 0; }
            set { HttpContext.Current.Session["CurrentID_ownerRentalInRome"] = value; }
        }
        public static string CurrentName
        {
            get { if (HttpContext.Current.Session["CurrentName_ownerRentalInRome"] != null)return (string)HttpContext.Current.Session["CurrentName_ownerRentalInRome"]; else return ""; }
            set { HttpContext.Current.Session["CurrentName_ownerRentalInRome"] = value; }
        }
    }
    public class ownerBasePage : System.Web.UI.Page
    {
        private string _pageType;
        public string PAGE_TYPE
        {
            get { if (this._pageType != null)return this._pageType; else return ""; }
            set { this._pageType = value; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                if (OwnerAuthentication.CurrentID == 0 && !Request.Url.AbsoluteUri.Contains("http://-localhost"))
                {
                    Response.Redirect("login.aspx?referer=" + Server.UrlEncode(Request.RawUrl));
                    return;
                }
                //string _url = Request.Url.PathAndQuery;
                //string ip_name = Request.ServerVariables.Get("REMOTE_HOST");
                //string host_name = "";
                //AdminUtilities.log_addConnection(1, "Pagina (" + _url + ")", ip_name, host_name);
            }
            CultureInfo objCultureInfo = new CultureInfo("it-IT");
            objCultureInfo.NumberFormat.NumberDecimalSeparator = ",";
            objCultureInfo.NumberFormat.NumberGroupSeparator = ".";
            objCultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";
            objCultureInfo.NumberFormat.CurrencyGroupSeparator = ".";
            objCultureInfo.NumberFormat.CurrencySymbol = "€";
            Thread.CurrentThread.CurrentUICulture = objCultureInfo;
            Thread.CurrentThread.CurrentCulture = objCultureInfo;
            CurrentLang.ID = 1;
        }
    }
}
