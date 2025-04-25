using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using ModRental;
using RentalInRome.data;
namespace RentalInRome.affiliatesarea
{
    public class agentAuth
    {
        public static bool Auth(string login, string pwd)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntAgentTBL user = dc.dbRntAgentTBLs.SingleOrDefault(x => x.authUsr == login && x.authPwd == pwd && x.isActive == 1);
                if (user != null)
                {
                    HttpContext.Current.Session.Timeout = 60;
                    CurrentID = user.id;
                    CurrentName = user.nameCompany;
                    CurrentLangID = user.pidLang.objToInt32();
                    hasAcceptedContract = user.hasAcceptedContract.objToInt32();
                    return true;
                }
                return false;
            }
        }
        public static int CurrentLangID
        {
            get { if (HttpContext.Current.Session["CurrentLangID_agentAuthRentalInRome"] != null)return (int)HttpContext.Current.Session["CurrentLangID_agentAuthRentalInRome"]; else return 0; }
            set { HttpContext.Current.Session["CurrentLangID_agentAuthRentalInRome"] = value; }
        }
        public static long CurrentID
        {
            get { if (HttpContext.Current.Session["CurrentID_agentAuthRentalInRome"] != null)return (long)HttpContext.Current.Session["CurrentID_agentAuthRentalInRome"]; else return 0; }
            set { HttpContext.Current.Session["CurrentID_agentAuthRentalInRome"] = value; }
        }
        public static string CurrentName
        {
            get { if (HttpContext.Current.Session["CurrentName_agentAuthRentalInRome"] != null)return (string)HttpContext.Current.Session["CurrentName_agentAuthRentalInRome"]; else return ""; }
            set { HttpContext.Current.Session["CurrentName_agentAuthRentalInRome"] = value; }
        }
        public static long hasAcceptedContract
        {
            get { if (HttpContext.Current.Session["hasAcceptedContract_agentAuthRentalInRome"] != null)return (long)HttpContext.Current.Session["hasAcceptedContract_agentAuthRentalInRome"]; else return 0; }
            set { HttpContext.Current.Session["hasAcceptedContract_agentAuthRentalInRome"] = value; }
        }
    }
    public class agentBasePage : System.Web.UI.Page
    {
        private string _pageType;
        public string PAGE_TYPE
        {
            get { if (this._pageType != null)return this._pageType; else return ""; }
            set { this._pageType = value; }
        }
        private string _sessionID;
        public string CURRENT_SESSION_ID
        {
            get { if (this._sessionID == null)_sessionID = _CURRENT_SESSION_ID; return _sessionID; }
            set { this._sessionID = value; }
        }
        private string _CURRENT_SESSION_ID
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_SESSION_ID"] == null)
                    HttpContext.Current.Session["CURRENT_SESSION_ID"] = Guid.NewGuid().ToString();
                return (string)HttpContext.Current.Session["CURRENT_SESSION_ID"];
            }
            set
            {
                HttpContext.Current.Session["CURRENT_SESSION_ID"] = value;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                string _tmp = CURRENT_SESSION_ID;
                if (agentAuth.CurrentID == 0 && !Request.Url.AbsoluteUri.Contains("http://-localhost"))
                {
                    Response.Redirect("login.aspx?referer=" + Server.UrlEncode(Request.RawUrl));
                    Response.End();
                    return;
                }
                if (agentAuth.hasAcceptedContract == 0 && !Request.Url.AbsoluteUri.ToLower().Contains("/affiliatesarea/terms.aspx"))
                {
                    Response.Redirect("/affiliatesarea/Terms.aspx");
                    Response.End();
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
            if (agentAuth.CurrentLangID != 0)
                CurrentLang.ID = agentAuth.CurrentLangID;
            else
                CurrentLang.ID = 2;
            CONT_TBL_LANG l = maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.SingleOrDefault(x => x.id == CurrentLang.ID);
            if (l != null)
            {
                CurrentLang.ID = l.id;
                CurrentLang.NAME = l.common_name;
                CurrentLang.ABBR = l.abbr;
                CurrentLang.TITLE = l.lang_title;
            }
            else
            {
                CurrentLang.ID = 2;
                CurrentLang.NAME = "en-GB";
                CurrentLang.ABBR = "eng";
                CurrentLang.TITLE = "English";
            }
        }
    }
}
