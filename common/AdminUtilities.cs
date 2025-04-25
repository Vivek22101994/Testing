using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Threading;
using System.Globalization;
using System.Web.UI.WebControls;
using RentalInRome.data;

public partial class AdminUtilities
{
    public static string common_changeMtVariables(string _str)
    {
        _str = _str.Replace("mt_common_line", "<hr width='100%' height='2px'/>");
        _str = _str.Replace("mt_current_date_dd/mm/yyyy", DateTime.Now.ToShortDateString());
        _str = _str.Replace("mt_current_user_name", UserAuthentication.CurrentUserName);
        _str = _str.Replace("/mt_current_server_root_path", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
        return _str;
    }
    public static void contTour_fillRewriteTool(ref UrlList uList, List<int> langIDs)
    {
        List<CONT_LN_TOUR> _lnList = maga_DataContext.DC_CONTENT.CONT_LN_TOURs.Where(x => langIDs.Contains(x.pid_lang) && x.page_path != "").ToList();
        foreach (CONT_LN_TOUR _ln in _lnList)
        {
            UrlItem item = new UrlItem("pg_tour", CurrentAppSettings.ROOT_PATH + _ln.page_path, "pg_tour_details_new.aspx?id=" + _ln.pid_tour + "&lang=" + _ln.pid_lang, "" + _ln.pid_lang);
            uList.Items.Add(item);
        }
    }
    public static void locCity_createPagePath(int id)
    {
    }
    public static string getCityTitle(int? pidCity)
    {
        var objCity = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.SingleOrDefault(x => x.pid_lang == App.LangID && x.id == pidCity);
        if (objCity != null)
            return objCity.title;
        else
            return "";
    }

    public static string getZoneTitle(int? pidZone)
    {
        var objZone = AppSettings.LOC_ZONEs.SingleOrDefault(x => x.pid_lang == App.LangID && x.id == pidZone);
        if (objZone != null)
            return objZone.title;
        else
            return "";
    }
    public static void locCity_fillRewriteTool(ref UrlList uList, List<int> langIDs)
    {
        List<LOC_VIEW_CITY> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => langIDs.Contains(x.pid_lang) && x.page_path != null && x.page_path.Trim() != "").ToList();
        foreach (LOC_VIEW_CITY _d in _list)
        {
            UrlItem item = new UrlItem("loc_city", CurrentAppSettings.ROOT_PATH + _d.page_path, "pg_city_details.aspx?id=" + _d.id + "&lang=" + _d.pid_lang, "" + _d.pid_lang);
            uList.Items.Add(item);
        }
    }
    public static void locZone_createPagePath_All()
    {
        List<LOC_TB_ZONE> _tbList = maga_DataContext.DC_LOCATION.LOC_TB_ZONEs.Where(x => x.is_active == 1).ToList();
        foreach (LOC_TB_ZONE _tb in _tbList)
        {
            locZone_createPagePath(_tb.id);
        }
    }
    public static void locZone_createPagePath(int id)
    {
        magaLocation_DataContext _dc = maga_DataContext.DC_LOCATION;
        LOC_TB_ZONE _tb = _dc.LOC_TB_ZONEs.SingleOrDefault(x => x.id == id);
        if (_tb == null) return;
        List<LOC_LN_ZONE> _lnList = _dc.LOC_LN_ZONEs.Where(x => x.pid_zone == id).ToList();
        foreach (LOC_LN_ZONE _ln in _lnList)
        {
            if (string.IsNullOrEmpty(_ln.folder_path)) continue;
            _ln.page_path = _ln.folder_path.clearPathName() + _tb.file_extension;
        }
        _dc.SubmitChanges();
    }
    public static void locZone_fillRewriteTool(ref UrlList uList, List<int> langIDs)
    {
        List<int> currIDs = maga_DataContext.DC_LOCATION.LOC_TB_ZONEs.Where(x => x.pid_city == AppSettings.RNT_currCity).Select(x => x.id).ToList();
        List<LOC_LN_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_LN_ZONEs.Where(x => langIDs.Contains(x.pid_lang) && currIDs.Contains(x.pid_zone) && x.page_path != null && x.page_path.Trim() != "").ToList();
        foreach (LOC_LN_ZONE _d in _list)
        {
            UrlItem item = new UrlItem("loc_zone", CurrentAppSettings.ROOT_PATH + _d.page_path, "pg_zone_details_new.aspx?id=" + _d.pid_zone + "&lang=" + _d.pid_lang, "" + _d.pid_lang);
            uList.Items.Add(item);

            //item = new UrlItem("loc_zone", CurrentAppSettings.ROOT_PATH + "new/" + _d.page_path, "pg_zone_details_new.aspx?id=" + _d.pid_zone + "&lang=" + _d.pid_lang, "" + _d.pid_lang);
            //uList.Items.Add(item);
        }
    }

    public static void locZone_fillRewriteTool_WL(ref UrlList uList, List<int> langIDs)
    {
        List<int> currIDs = maga_DataContext.DC_LOCATION.LOC_TB_ZONEs.Where(x => x.pid_city == AppSettings.RNT_currCity).Select(x => x.id).ToList();
        List<LOC_LN_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_LN_ZONEs.Where(x => langIDs.Contains(x.pid_lang) && currIDs.Contains(x.pid_zone) && x.page_path != null && x.page_path.Trim() != "").ToList();
        foreach (LOC_LN_ZONE _d in _list)
        {
            UrlItem item = new UrlItem("loc_zone", CurrentAppSettings.ROOT_PATH + _d.page_path, "WLRental/pg_zone_details.aspx?id=" + _d.pid_zone + "&lang=" + _d.pid_lang, "" + _d.pid_lang);
            uList.Items.Add(item);
        }
    }

    public static void contStp_fillRewriteTool(ref UrlList uList, List<int> langIDs)
    {
        List<CONT_TB_STP> currList = maga_DataContext.DC_CONTENT.CONT_TB_STP.ToList();
        currList = currList.Where(x => x.pid_city.objToInt32() == 1 || x.pid_city.objToInt32() == 0).ToList();
        List<int> currIDs = currList.Select(x => x.id).ToList();
        List<CONT_VIEW_STP> static_pages = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.Where(x => langIDs.Contains(x.pid_lang) && currIDs.Contains(x.id) && x.page_rewrite != null && x.page_rewrite.Trim() != "").ToList();
        foreach (CONT_VIEW_STP d in static_pages)
        {
            UrlItem item = new UrlItem("stp", CurrentAppSettings.ROOT_PATH + d.page_path, d.page_rewrite + "id=" + d.id + "&lang=" + d.pid_lang, "" + d.pid_lang);
            uList.Items.Add(item);
        }
    }

    public static void contStp_fillRewriteTool_WL(ref UrlList uList, List<int> langIDs)
    {
        List<CONT_TB_STP> currList = maga_DataContext.DC_CONTENT.CONT_TB_STP.ToList();
        currList = currList.Where(x => x.pid_city.objToInt32() == 1 || x.pid_city.objToInt32() == 0).ToList();
        List<int> currIDs = currList.Select(x => x.id).ToList();
        List<CONT_VIEW_STP> static_pages = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.Where(x => langIDs.Contains(x.pid_lang) && currIDs.Contains(x.id) && x.page_rewrite != null && x.page_rewrite.Trim() != "" && x.page_path != null && x.page_path.Trim() != "").ToList();
        foreach (CONT_VIEW_STP d in static_pages)
        {
            UrlItem item = new UrlItem("stp", CurrentAppSettings.ROOT_PATH + d.page_path, "WLRental/" + d.page_rewrite + "id=" + d.id + "&lang=" + d.pid_lang, "" + d.pid_lang);
            uList.Items.Add(item);
        }
    }

    public static void FillRewriteTool()
    {
        AppUtils.FillUrlList();
    }
    public static void Bind_drp_time(ref DropDownList drp, int start)
    {
        drp.Items.Clear();
        for (int i = start; i < 24; i++)
        {
            string _t = "" + i;
            if (_t.Length == 1) _t = "0" + _t;
            drp.Items.Add(new System.Web.UI.WebControls.ListItem(_t + ":00", "" + i));
        }
    }

    public static string zone_countryName(int id)
    {
        LOC_LK_COUNTRY _s = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.title;
        return "";
    }
    public static int zone_countryId(string name)
    {
        LOC_LK_COUNTRY _s = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.SingleOrDefault(x => x.title == name);
        if (_s != null)
            return _s.id;
        return 0;
    }
    public static void log_addConnection(int type, string comments, string ip_name, string host_name)
    {
        magaCommon_DataContext _dc = maga_DataContext.DC_COMMON;
        LOG_ADMIN_CONNECTION log = new LOG_ADMIN_CONNECTION();
        log.date_connection = DateTime.Now;
        log.admin_id = UserAuthentication.CurrentUserID;
        log.admin_name = UserAuthentication.CurrentUserName;
        log.ip_name = ip_name;
        log.host_name = host_name;
        log.comments = comments;
        log.type = type;
        _dc.LOG_ADMIN_CONNECTIONs.InsertOnSubmit(log);
        _dc.SubmitChanges();
    }
    public static void log_addClientConnection(int type, string comments, string ip_name, string host_name, string referer, string search_text)
    {
        magaCommon_DataContext _dc = maga_DataContext.DC_COMMON;
        LOG_CLIENT_CONNECTION log = new LOG_CLIENT_CONNECTION();
        log.date_connection = DateTime.Now;
        log.referer = referer;
        log.search_text = search_text;
        log.ip_name = ip_name;
        log.host_name = host_name;
        log.comments = comments;
        log.type = type;
        _dc.LOG_CLIENT_CONNECTIONs.InsertOnSubmit(log);
        _dc.SubmitChanges();
    }
}
public class UserPermission
{

    public int id { get; set; }
    public string code { get; set; }
    public string title { get; set; }
    public UserPermission()
    {
        id = 0;
        code = "";
        title = "";
    }
    public UserPermission(int Id, string Code, string Title)
    {
        id = Id;
        code = Code;
        title = Title;
    }
}
public class UserAuthentication
{
    public enum PERMISSIONS
    {
        ManageAllRequests = 1,
        ManageAllUsers = 2,
        MansageAllRequests = 3,
        ManageMailOptions = 4,
        ManageRoles = 5,
        ManageUsers = 6,
        EditProtocols = 7,
        CompleteProtocols = 8,
        ChangedProtocols = 9
    }
    private static List<UserPermission> fillUserPermissions()
    {
        List<UserPermission> _list = new List<UserPermission>();
        _list.Add(new UserPermission(1, "usr_mail", "Anagrafica - E-mail"));
        _list.Add(new UserPermission(1, "usr_mail_config", "Anagrafica - Configurazione E-mail"));
        _list.Add(new UserPermission(1, "usr_operator", "Anagrafica - Gestione Account"));
        _list.Add(new UserPermission(1, "usr_operator_avv", "Anagrafica - Disponibilità Account"));
        _list.Add(new UserPermission(1, "usr_country_lang", "Anagrafica - Gestione Lingue per Location"));
        _list.Add(new UserPermission(1, "usr_client", "Anagrafica - Elenco Clienti"));
        _list.Add(new UserPermission(1, "usr_owner", "Anagrafica - Elenco Proprietari"));
        _list.Add(new UserPermission(1, "usr_agent", "Anagrafica - Elenco Agenzie"));
        _list.Add(new UserPermission(1, "authUserReport", "Anagrafica - Gestione Report"));
        _list.Add(new UserPermission(1, "extras", "Rental - E-commerce servizi"));
        _list.Add(new UserPermission(1, "rnt_estate", "Rental - Appartamenti"));
        _list.Add(new UserPermission(1, "rnt_request", "Rental - Richieste"));
        _list.Add(new UserPermission(1, "rnt_reservation_planner", "Rental - Prenotazioni / Disponibilità - griglia"));
        _list.Add(new UserPermission(1, "rnt_reservation_list", "Rental - Prenotazioni / Disponibilità - elenco"));
        _list.Add(new UserPermission(1, "rnt_reservation_event", "Rental - Planner CheckIn / CheckOut"));
        _list.Add(new UserPermission(1, "rnt_period", "Rental - Stagioni"));
        _list.Add(new UserPermission(1, "rnt_estate_comment", "Rental - Commenti per Appartamenti"));
        _list.Add(new UserPermission(1, "rnt_special_offer", "Rental - Offerte Speciali"));
        _list.Add(new UserPermission(1, "RntDiscountPromoCode", "Rental - Discount PromoCode"));
        _list.Add(new UserPermission(1, "loc_city", "Location - Città"));
        _list.Add(new UserPermission(1, "loc_zone", "Location - Zone"));
        _list.Add(new UserPermission(1, "loc_point", "Location - Punti d'interesse"));
        _list.Add(new UserPermission(1, "loc_pointtype", "Location - Tipologie Punti d'interesse"));
        _list.Add(new UserPermission(1, "cont_stp", "Contenuti - Gestione Pagine Statiche"));
        _list.Add(new UserPermission(1, "cont_tour", "Contenuti - Gestione Tour"));
        _list.Add(new UserPermission(1, "cont_tour_item", "Contenuti - Escursioni-Tour"));
        _list.Add(new UserPermission(1, "blog", "Blog"));
        _list.Add(new UserPermission(1, "manage_lang", "Gestione - Variabili delle Lingue"));
        _list.Add(new UserPermission(1, "def_sys_setting", "Gestione - Variabili del sistema"));
        _list.Add(new UserPermission(1, "mail_template", "Gestione - Gestione Mail Template"));
        _list.Add(new UserPermission(1, "util_sendMail", "Gestione - Invio mail per agenzie"));
        _list.Add(new UserPermission(1, "inv_invoice", "Contabilità e Report - Fatture"));
        _list.Add(new UserPermission(1, "inv_payment", "Contabilità e Report - Prima nota"));
        _list.Add(new UserPermission(1, "rep_stats", "Contabilità e Report - Statistiche"));
        _list.Add(new UserPermission(1, "limo_pickupPlace", "Limo - Punti di trasporto"));
        _list.Add(new UserPermission(1, "limo_transportDuration", "Limo - durata di trasporto"));
        _list.Add(new UserPermission(1, "limo_transportType", "Limo - Tipi di trasporto"));
        _list.Add(new UserPermission(1, "Statistiche_Account", "Statistiche - Account"));
        _list.Add(new UserPermission(1, "Statistiche_Immobili", "Statistiche - Immobili"));
        _list.Add(new UserPermission(1, "Statistiche_Timing", "Statistiche - Tempi di Risposta"));
        return _list;
        return _list;
    }
    public static bool Auth(string login, string pwd)
    {
        USR_ADMIN user = maga_DataContext.DC_USER.USR_ADMIN.SingleOrDefault(x => x.login == login && x.password == pwd && x.is_active == 1 && x.is_deleted == 0);
        if (user != null)
        {
            HttpContext.Current.Session.Timeout = 60;
            UserAuthentication.CurrUserTbl = user;
            UserAuthentication.CurrRoleTBL = null;
            UserAuthentication.CurrentUserID = user.id;
            UserAuthentication.CURRENT_USER_ROLE = user.pid_role.Value;
            UserAuthentication.CURRENT_USER_PERMISSIONS = null;
            UserAuthentication.CurrentUserName = user.name + " " + user.surname;
            UserAuthentication.CURRENT_USR_ADMIN_CONFIG = maga_DataContext.DC_USER.USR_ADMIN_CONFIGs.SingleOrDefault(x => x.id == user.pid_config);
            string ip_name = HttpContext.Current.Request.ServerVariables.Get("REMOTE_HOST");
            string host_name = "";
            AdminUtilities.log_addConnection(1, "Login area amministrativa (login.aspx)", ip_name, host_name);
            return true;
        }
        return false;
    }
    public static List<UserPermission> PERMISSION_LIST
    {
        get
        {
            if (HttpContext.Current.Cache["USER_PERMISSION_LIST"] == null)
                HttpContext.Current.Cache["USER_PERMISSION_LIST"] = fillUserPermissions();
            return (List<UserPermission>)HttpContext.Current.Cache["USER_PERMISSION_LIST"];
        }
        set
        {
            HttpContext.Current.Cache["USER_PERMISSION_LIST"] = value;
        }
    }
    public static bool hasPermission(string code)
    {
        List<USR_RL_ROLE_PERMISSION> _list = CURRENT_USER_PERMISSIONS;
        return code == "" || (code == "superadmin" && CURRENT_USER_ROLE == 1) || _list.FirstOrDefault(x => x.permission == code) != null;
    }
    public static bool hasPermission(string code, string mode)
    {
        List<USR_RL_ROLE_PERMISSION> _list = CURRENT_USER_PERMISSIONS;
        USR_RL_ROLE_PERMISSION permission = _list.FirstOrDefault(x => x.permission == code);
        if (permission == null) return false;
        if (mode == "can_create") return permission.can_create == 1;
        if (mode == "can_delete") return permission.can_delete == 1;
        if (mode == "can_edit") return permission.can_edit == 1;
        if (mode == "can_read") return permission.can_read == 1;
        if (mode == "only_owned") return permission.only_owned == 1;
        return false;
    }
    //public static bool hasPermissionForAll(string code)
    //{
    //    List<USR_RL_ROLE_PERMISSION> _list = CURRENT_USER_PERMISSIONS;
    //    return code == "" || (code == "superadmin" && CURRENT_USER_ROLE == 1) || _list.FirstOrDefault(x => x.permission == code && x.) != null;
    //}

    private static List<USR_RL_ROLE_PERMISSION> fillCurrentUserPermissions()
    {
        return maga_DataContext.DC_USER.USR_RL_ROLE_PERMISSIONs.Where(x => x.pid_role == CURRENT_USER_ROLE).ToList();
    }
    public static List<USR_RL_ROLE_PERMISSION> CURRENT_USER_PERMISSIONS
    {
        get
        {
            if (HttpContext.Current.Session["CURRENT_USER_PERMISSIONs"] == null)
                HttpContext.Current.Session["CURRENT_USER_PERMISSIONs"] = fillCurrentUserPermissions();
            return (List<USR_RL_ROLE_PERMISSION>)HttpContext.Current.Session["CURRENT_USER_PERMISSIONs"];
        }
        set
        {
            HttpContext.Current.Session["CURRENT_USER_PERMISSIONs"] = value;
        }
    }
    public static int CURRENT_USER_ROLE
    {
        get
        {
            if (HttpContext.Current.Session["CURRENT_USER_ROLE"] == null)
            {
                USR_ADMIN _admin = maga_DataContext.DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == CurrentUserID);
                HttpContext.Current.Session["CURRENT_USER_ROLE"] = _admin != null && _admin.pid_role.HasValue ? _admin.pid_role.Value : 0;
            }
            return (int)HttpContext.Current.Session["CURRENT_USER_ROLE"];
        }
        set
        {
            HttpContext.Current.Session["CURRENT_USER_ROLE"] = value;
        }
    }
    public static USR_TBL_ROLE CurrRoleTBL
    {
        get
        {
            if (HttpContext.Current.Session["CURRENT_USR_TBL_ROLE"] == null)
                HttpContext.Current.Session["CURRENT_USR_TBL_ROLE"] = maga_DataContext.DC_USER.USR_TBL_ROLE.SingleOrDefault(x => x.id == CURRENT_USER_ROLE);
            if (HttpContext.Current.Session["CURRENT_USR_TBL_ROLE"] == null)
                return new USR_TBL_ROLE();
            return (USR_TBL_ROLE)HttpContext.Current.Session["CURRENT_USR_TBL_ROLE"];
        }
        set
        {
            HttpContext.Current.Session["CURRENT_USR_TBL_ROLE"] = value;
        }
    }
    public static USR_ADMIN CurrUserTbl
    {
        get
        {
            if (HttpContext.Current.Session["CURRENT_USR_TBL_ADMIN"] == null)
                HttpContext.Current.Session["CURRENT_USR_TBL_ADMIN"] = maga_DataContext.DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == CurrentUserID);
            if (HttpContext.Current.Session["CURRENT_USR_TBL_ADMIN"] == null)
                return new USR_ADMIN();
            return (USR_ADMIN)HttpContext.Current.Session["CURRENT_USR_TBL_ADMIN"];
        }
        set
        {
            HttpContext.Current.Session["CURRENT_USR_TBL_ADMIN"] = value;
        }
    }
    public static USR_ADMIN_CONFIG CURRENT_USR_ADMIN_CONFIG
    {
        get
        {
            if (HttpContext.Current.Session["CURRENT_USR_ADMIN_CONFIG"] == null)
                return null;
            return (USR_ADMIN_CONFIG)HttpContext.Current.Session["CURRENT_USR_ADMIN_CONFIG"];
        }
        set
        {
            HttpContext.Current.Session["CURRENT_USR_ADMIN_CONFIG"] = value;
        }
    }
    public static int CurrentUserID
    {
        get { if (HttpContext.Current.Session["CurrentUserID_adminRentalInRome"] != null)return (int)HttpContext.Current.Session["CurrentUserID_adminRentalInRome"]; else return 0; }
        set { HttpContext.Current.Session["CurrentUserID_adminRentalInRome"] = value; }
    }
    public static string CurrentUserName
    {
        get { if (HttpContext.Current.Session["CurrentUserName_adminRentalInRome"] != null)return (string)HttpContext.Current.Session["CurrentUserName_adminRentalInRome"]; else return ""; }
        set { HttpContext.Current.Session["CurrentUserName_adminRentalInRome"] = value; }
    }
}
public class adminBasePage : PageWithHandlers
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
            if (UserAuthentication.CurrentUserID == 0 && !Request.Url.AbsoluteUri.Contains("http://-localhost"))
            {
                Response.Redirect("/admin/login.aspx?referer=" + Server.UrlEncode(Request.RawUrl));
                return;
            }
            AdminUtilities.log_addConnection(1, Request.Url.PathAndQuery, Request.browserIP(), "");
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
    protected override void OnPreLoad(EventArgs e)
    {
        base.OnPreLoad(e);
        if (!UserAuthentication.hasPermission(PAGE_TYPE))
        {
            Response.Redirect("/admin/");
            return;
        }
    }
    protected void CloseRadWindow(string args)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CloseRadWindow", "CloseRadWindow('" + args + "');", true);
    }
}

public class PageWithHandlers : System.Web.UI.Page
{
    public void renderLiteral_setLabel(object sender, EventArgs e)
    {
        var elm = sender as Literal;
        if (elm.Text.StartsWith("#lbl#"))
            elm.Text = contUtils.getLabel(elm.Text.Replace("#lbl#", ""));
    }
    public void renderLinkButton_Delete(object sender, EventArgs e)
    {
        var elm = sender as LinkButton;
        elm.OnClientClick = "return confirm('" + contUtils.getLabel("adminDeleteConfirm") + "');";
    }
    public void renderLinkButton(object sender, EventArgs e)
    {
        var elm = sender as LinkButton;
        if (elm == null) return;
        if (!string.IsNullOrEmpty(elm.OnClientClick) && elm.OnClientClick.StartsWith("confirm%") && elm.OnClientClick.Replace("confirm%", "") != "")
        {
            elm.OnClientClick = "return confirm('" + contUtils.getLabel(elm.OnClientClick.Replace("confirm%", "")) + "');";
        }
        if (elm.Text.StartsWith("#lbl#"))
            elm.Text = contUtils.getLabel(elm.Text.Replace("#lbl#", ""));
    }

    protected void CheckBind(Control parentControl)
    {
        foreach (Control ctrl in parentControl.Controls)
        {
            if (ctrl is DataBoundLiteralControl && !(ctrl.Parent is IDataItemContainer) && ctrl.DataItemContainer == null && ctrl.Controls.Count == 0)
            {
                ctrl.DataBind();
                continue;
            }
            if (ctrl is Literal)
            {
                ctrl.DataBind();
                continue;
            }
            CheckBind(ctrl);
        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        CheckBind(this.Page);
    }
}