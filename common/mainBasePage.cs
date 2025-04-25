using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Threading;
using System.Globalization;
using RentalInRome.data;
using System.Text;
public class BC_item
{
    public string Title { get; set; }
    public string Url { get; set; }
    public BC_item()
    {
    }
    public BC_item(string Title, string Url)
    {
        this.Title = Title;
        this.Url = Url;
    }
}

public class mainBasePage : System.Web.UI.Page
{
    public List<BC_item> BC_list
    {
        get { if (HttpContext.Current.Session["BC_list"] != null)return (List<BC_item>)HttpContext.Current.Session["BC_list"]; else return new List<BC_item>(); }
        set { HttpContext.Current.Session["BC_list"] = value; }
    }
    private string _pageType;
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
    private string _SITE_LOADER;
    public string SITE_LOADER
    {
        get
        {
            if (_SITE_LOADER == null)
            {
                RentalInRome.common.MP_main _master = (RentalInRome.common.MP_main)this.Page.Master;
                _SITE_LOADER = (_master != null && (UpdateProgress)_master.FindControl("loading_cont") != null) ? _master.FindControl("loading_cont").ClientID : "ctl00_loading_cont";
            } return _SITE_LOADER;
        }
        set { this._pageType = value; }
    }
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        if (!IsPostBack)
            checkCookiesAndSession();
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
        if (!IsPostBack)
        {
            List<BC_item> _list = BC_list;
            if (PAGE_REF_ID == 1 && PAGE_TYPE == "stp")
            {
                // prevent Adding Home page link Twice
                return;
            }
            BC_item _current = new BC_item(AppUtils.getPageName(PAGE_REF_ID.ToString(), PAGE_TYPE, App.LangID.ToString()), AppUtils.getPagePath(PAGE_REF_ID + "", PAGE_TYPE, App.LangID.ToString()));
            BC_item _exist = _list.SingleOrDefault(x => x.Url == _current.Url);
            if (_exist != null)
            {
                _list.Remove(_exist);
            }
            else
            {
                if (_list.Count == 4) _list.Remove(_list[3]);
            }
            _list.Insert(0, _current);
            BC_list = _list;
        }

    }
    private static string[] aspNetFormElements = new string[] 
      { 
        "__EVENTTARGET",
        "__EVENTARGUMENT",
        "__VIEWSTATE",
        "__EVENTVALIDATION",
        "__VIEWSTATEENCRYPTED",
      };



    protected override void Render(HtmlTextWriter writer)
    {
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
        base.Render(htmlWriter);
        string html = stringWriter.ToString();
        int formStart = html.IndexOf("<form");
        int endForm = -1;
        if (formStart >= 0)
            endForm = html.IndexOf(">", formStart);
        if (endForm >= 0)
        {
            StringBuilder viewStateBuilder = new StringBuilder();
            foreach (string element in aspNetFormElements)
            {
                int startPoint = html.IndexOf("<input type=\"hidden\" name=\"" + element + "\"");
                if (startPoint >= 0 && startPoint > endForm)
                {
                    int endPoint = html.IndexOf("/>", startPoint);
                    if (endPoint >= 0)
                    {
                        endPoint += 2;
                        string viewStateInput = html.Substring(startPoint, endPoint - startPoint);
                        html = html.Remove(startPoint, endPoint - startPoint);
                        viewStateBuilder.Append(viewStateInput).Append("\r\n");
                    }
                }
            }
            if (viewStateBuilder.Length > 0)
            {
                viewStateBuilder.Insert(0, "\r\n");
                html = html.Insert(endForm + 1, viewStateBuilder.ToString());
            }
        }
        writer.Write(html);
    }

    protected void checkCookiesAndSession()
    {
        string tmpCookiesName = "www.rentalinrome.com";
        var tmpCookies = new HttpCookie(tmpCookiesName);
        if (Request.Cookies[tmpCookiesName] != null)
        {
            tmpCookies = Request.Cookies[tmpCookiesName];
        }
        else
        {
            tmpCookies = new HttpCookie(tmpCookiesName, DateTime.Now.JSCal_dateTimeToString() + "_" + CURRENT_SESSION_ID);
            tmpCookies.Expires = DateTime.Now.AddDays(60);
        }
        App.IdAdMedia = tmpCookies["IdAdMedia"] != null ? tmpCookies["IdAdMedia"] : "";
        App.IdLink = tmpCookies["IdLink"] != null ? tmpCookies["IdLink"] : "";
        if (!string.IsNullOrEmpty(Request.QueryString["id_ad_media"]))
            App.IdAdMedia = Request.QueryString["id_ad_media"];
        if (!string.IsNullOrEmpty(Request.QueryString["id_link"]))
            App.IdLink = Request.QueryString["id_link"];
        tmpCookies.Expires = DateTime.Now.AddDays(60);
        tmpCookies["IdAdMedia"] = App.IdAdMedia;
        tmpCookies["IdLink"] = App.IdLink;
        Response.Cookies.Remove(tmpCookiesName);
        Response.Cookies.Add(tmpCookies);
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

        //if (HttpContext.Current.Request.Url.AbsoluteUri.StartsWith("https://") || PAGE_TYPE == "pg_estate" || PAGE_TYPE == "stp" && PAGE_REF_ID == 6)
        // disabilita cache
        if (6 == 6)
        {
            // in area riservata e ricerca NO cache
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
        else
        {
            HttpContext.Current.Response.Cache.SetLastModified(_lastModified);
            HttpContext.Current.Response.Cache.SetExpires(_expires);
        }

        //Used for disabling page caching
        return;
        // non va bene
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
        HttpContext.Current.Response.AppendHeader("Cache-Control", "no-store"); // HTTP 1.1
        HttpContext.Current.Response.AppendHeader("Cache-Control", "must-revalidate"); // HTTP 1.1
        HttpContext.Current.Response.AppendHeader("Cache-Control", "max-stale=0"); // HTTP 1.1 
        HttpContext.Current.Response.AppendHeader("Cache-Control", "post-check=0"); // HTTP 1.1 
        HttpContext.Current.Response.AppendHeader("Cache-Control", "pre-check=0"); // HTTP 1.1 
        HttpContext.Current.Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.1 
        HttpContext.Current.Response.AppendHeader("Keep-Alive", "timeout=3, max=993"); // HTTP 1.1 
        HttpContext.Current.Response.AppendHeader("Expires", "Mon, 26 Jul 1997 05:00:00 GMT"); //}
        HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
        HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetNoStore();
    }
}
