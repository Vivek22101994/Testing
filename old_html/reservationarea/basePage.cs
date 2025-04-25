using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Threading;
using System.Globalization;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{
    public class resUtils
    {
        public static void log_addConnection()
        {
            string _url = HttpContext.Current.Request.Url.PathAndQuery;
            string ip_name = HttpContext.Current.Request.ServerVariables.Get("REMOTE_HOST");
            string host_name = "";
            magaCommon_DataContext _dc = maga_DataContext.DC_COMMON;
            LOG_AREARES_CONNECTION log = new LOG_AREARES_CONNECTION();
            log.date_connection = DateTime.Now;
            log.pid_reservation = CurrentIdReservation_gl;
            log.code = CurrentReservationCode;
            log.cl_full_name = CurrentCLFullName;
            log.ip_name = ip_name;
            log.host_name = host_name;
            log.comments = "Pagina (" + _url + ")";
            log.type = 1;
            _dc.LOG_AREARES_CONNECTION.InsertOnSubmit(log);
            _dc.SubmitChanges();
        }
        public static long CurrentIdReservation_gl
        {
            get { if (HttpContext.Current.Session["CurrentIdReservation"] != null)return (long)HttpContext.Current.Session["CurrentIdReservation"]; else return 0; }
            set
            {
                HttpContext.Current.Session["CurrentIdReservation"] = value;
            }
        }
        public static string CurrentReservationCode
        {
            get { if (HttpContext.Current.Session["CurrentReservationCode"] != null) return (string)HttpContext.Current.Session["CurrentReservationCode"]; else return ""; }
            set { HttpContext.Current.Session["CurrentReservationCode"] = value; }
        }
        public static string CurrentCLFullName
        {
            get { if (HttpContext.Current.Session["CurrentCLFullName"] != null)return (string)HttpContext.Current.Session["CurrentCLFullName"]; else return ""; }
            set
            {
                HttpContext.Current.Session["CurrentCLFullName"] = value;
            }
        }
        public static int CurrentLang_ID
        {
            get { if (HttpContext.Current.Session["CurrentLang_ID"] != null)return (int)HttpContext.Current.Session["CurrentLang_ID"]; else return 2; }
            set
            {
                HttpContext.Current.Session["CurrentLang_ID"] = value;
            }
        }
        public static string CurrentLang_NAME
        {
            get { if (HttpContext.Current.Session["CurrentLang_NAME"] != null)return (string)HttpContext.Current.Session["CurrentLang_NAME"]; else return ""; }
            set
            {
                HttpContext.Current.Session["CurrentLang_NAME"] = value;
            }
        }
        public static string CurrentLang_ABBR
        {
            get { if (HttpContext.Current.Session["CurrentLang_ABBR"] != null)return (string)HttpContext.Current.Session["CurrentLang_ABBR"]; else return ""; }
            set
            {
                HttpContext.Current.Session["CurrentLang_ABBR"] = value;
            }
        }
        public static string CurrentLang_TITLE
        {
            get { if (HttpContext.Current.Session["CurrentLang_TITLE"] != null)return (string)HttpContext.Current.Session["CurrentLang_TITLE"]; else return ""; }
            set
            {
                HttpContext.Current.Session["CurrentLang_TITLE"] = value;
            }
        }
        public static int currCity
        {
            get { if (HttpContext.Current.Session["reservationarea_currCity"] != null)return (int)HttpContext.Current.Session["reservationarea_currCity"]; else return 1; }
            set
            {
                HttpContext.Current.Session["reservationarea_currCity"] = value;
            }
        }
    }
    public class basePage : System.Web.UI.Page
    {
        private string _pageType;
        public bool adminAccess = false;
        public int _referenceId;
        private string _sessionID;
        public string PAGE_TYPE
        {
            get { if (this._pageType != null)return this._pageType; else return ""; }
            set { this._pageType = value; }
        }
        public int PAGE_REF_ID
        {
            get { if (this._referenceId != null)return this._referenceId; else return 0; }
            set { this._referenceId = value; }
        }
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
        public long CurrentIdReservation
        {
            get 
            {
                if (adminAccess && Request.QueryString["usr"].ToInt32() != 0 && !string.IsNullOrEmpty(Request.QueryString["authtmp"]))
                {
                    Guid _unique = Guid.NewGuid();
                    try { _unique = new Guid(Request.QueryString["authtmp"]); }
                    catch (Exception ex) { }
                    using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                    {
                        RNT_TBL_RESERVATION _res = dcOld.RNT_TBL_RESERVATION.SingleOrDefault(x => x.unique_id == _unique && x.state_pid == 4);
                        if (_res != null)
                        {
                            resUtils.CurrentLang_ID = 2;
                            resUtils.CurrentLang_NAME = "en-GB";
                            resUtils.CurrentLang_ABBR = "eng";
                            resUtils.CurrentLang_TITLE = "English";
                            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _res.pid_estate);
                            if (_estTB == null)
                            {
                                Response.Redirect("/reservationarea/login.aspx", true);
                            }
                            resUtils.currCity = _estTB.pid_city.objToInt32();
                            return _res.id;
                        }
                    }
                }
                return resUtils.CurrentIdReservation_gl;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (CurrentIdReservation == 0)
            {
               Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            CurrentLang.ID = resUtils.CurrentLang_ID;
            CurrentLang.NAME = resUtils.CurrentLang_NAME;
            CurrentLang.ABBR = resUtils.CurrentLang_ABBR;
            CurrentLang.TITLE = resUtils.CurrentLang_TITLE;
            resUtils.log_addConnection();
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (!Request.Url.AbsoluteUri.ToLower().Contains(".magadesign.net") && !Request.Url.AbsoluteUri.Contains("http://localhost") && !IsPostBack && Request.Url.AbsoluteUri.Contains("http://") && !Request.Url.AbsoluteUri.ToLower().Contains(".dev.magarental.com"))
            {
                //string _auth = Request.QueryString["auth"];
                //string _qs = "";
                //if (!string.IsNullOrEmpty(_auth))
                //    _qs = "?auth=" + _auth;
                //else if (Request.QueryString["usr"].ToInt32() == UserAuthentication.CurrentUserID && UserAuthentication.CurrentUserID != 0 && Request.QueryString["resId"].ToInt64() != 0)
                //    _qs = "?usr=" + Request.QueryString["usr"] + "&resId=" + Request.QueryString["resId"];

                Response.Redirect(Request.Url.AbsoluteUri.Replace("http://www.","https://"), true);
                return;
            }

            string _tmp = CURRENT_SESSION_ID;
            int _id;
            CONT_TBL_LANG l = null;
            if (Request.QueryString["lang"] != null && Request.QueryString["lang"] != "" && int.TryParse(Request.QueryString["lang"], out _id))
            {
                l = maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.SingleOrDefault(x => x.id == _id);
            }
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
            CultureInfo objCultureInfo = new CultureInfo(CurrentLang.NAME);
            objCultureInfo.NumberFormat.NumberDecimalSeparator = ",";
            objCultureInfo.NumberFormat.NumberGroupSeparator = ".";
            objCultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";
            objCultureInfo.NumberFormat.CurrencyGroupSeparator = ".";
            objCultureInfo.NumberFormat.CurrencySymbol = "€";
            Thread.CurrentThread.CurrentUICulture = objCultureInfo;
            Thread.CurrentThread.CurrentCulture = objCultureInfo;
            if (!IsPostBack)
            {
                string _referrerString = "No referrer!";
                Uri referrer = HttpContext.Current.Request.UrlReferrer;
                if (referrer != null)
                {
                    _referrerString = referrer.OriginalString.ToLower();
                    if (!_referrerString.StartsWith(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)) && !Request.Url.AbsoluteUri.Contains("http://localhost"))
                    {
                        CommonUtilities.CURRENT_REFERRER = _referrerString.Remove(0, 7);
                        //string _url = Request.Url.PathAndQuery;
                        //string ip_name = Request.ServerVariables.Get("REMOTE_HOST");
                        //string host_name = "";
                        //AdminUtilities.log_addClientConnection(1, "Pagina (" + _url + ")", ip_name, host_name, CommonUtilities.CURRENT_REFERRER, CommonUtilities.GetSearchQuery(CommonUtilities.CURRENT_REFERRER));
                    }
                }
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            DisablePageCaching();
        }
        protected void RewritePath()
        {
            string clientFilePath = Request.RawUrl;
            int index = clientFilePath.IndexOf('?');
            if (index >= 0)
                clientFilePath = clientFilePath.Substring(0, index);
            if (clientFilePath != Request.CurrentExecutionFilePath)
            {
                // This request is done using a mapped URL. This has set the action of the html-form (<form action=...>) back
                // to the physical aspx file. This results in a navigation problem navigating away from the mapped url to
                // the physical url.
                // For example: whithout this code, a postback from /Quote/MyQuotes.aspx would result in the URL /Order/MyOrders.aspx?Type=Quotes
                // This is not a problem for the actual order/quote page, but the navigation will point to Order/MyOrders.aspx.
                // Thus the crumbtrail, menu and URL are misleading.
                // Fix: Change the 'action' of the form back to the virtual admin page
                string action = clientFilePath;
                index = action.LastIndexOf('/');
                if (index >= 0)
                    action = action.Substring(index + 1, action.Length - index - 1);
                string script = string.Format("theForm.action='{0}';", action);
                Page.ClientScript.RegisterClientScriptBlock(typeof(string), "ResetAction", script, true);
                // Bugfix for UpdatePanels. The call-back would reset the action of the form.
                script += string.Format("theForm._initialAction='{0}';", action);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "ResetAction", script, true);
            }
        }
        protected void DisablePageCaching()
        {
            DateTime _lastModified = DateTime.Now;
            string _lastModifiedStr = _lastModified.formatCustom("#D#, #dd# #M# #yy#", 2, "Wed, 09 Apr 2008") + " 23:55:38 GMT";
            DateTime _expires = DateTime.Now.AddDays(8);
            string _expiresStr = _expires.formatCustom("#D#, #dd# #M# #yy#", 2, "Wed, 09 Apr 2008") + " 23:55:38 GMT";

            HttpContext.Current.Response.Cache.SetLastModified(_lastModified);
            HttpContext.Current.Response.Cache.SetExpires(_lastModified);
            HttpContext.Current.Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
            HttpContext.Current.Response.AppendHeader("Cache-Control", "no-store"); // HTTP 1.1
            HttpContext.Current.Response.AppendHeader("Cache-Control", "must-revalidate"); // HTTP 1.1
            HttpContext.Current.Response.AppendHeader("Cache-Control", "max-stale=0"); // HTTP 1.1 
            HttpContext.Current.Response.AppendHeader("Cache-Control", "post-check=0"); // HTTP 1.1 
            HttpContext.Current.Response.AppendHeader("Cache-Control", "pre-check=0"); // HTTP 1.1 
            HttpContext.Current.Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.1 
            HttpContext.Current.Response.AppendHeader("Keep-Alive", "timeout=3, max=993"); // HTTP 1.1 
        }
    }

}