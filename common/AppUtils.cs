using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;
public class CurrentLang
{
    public static string JS_CAL_FILE
    {
        get
        {
            return contUtils.getLang_jsCalFile(ID);
        }
    }
    public static string ABBR
    {
        get { return App.LangCode; }
        set { App.LangCode = value; }
    }
    public static string NAME
    {
        get { return App.LangCultureName; }
        set { App.LangCultureName = value; }
    }
    public static string TITLE
    {
        get { return App.LangNameTitle; }
        set { App.LangNameTitle = value; }
    }
    public static int ID
    {
        get { return App.LangID; }
        set { App.LangID = value; }
    }
}
public partial class App
{
    public static string ProjectName
    {
        get
        {
            return "RentalInRome";
        }
    }
    public static string EO_PDF_LICENSE
    {
        get
        {
            return CurrentAppSettings.EO_PDF_LICENSE;
            //return "F8idtbXH3LRyrL3C78BtqbzK4bORs7P9FOKe5ff29ON3hI6xy59Zs/D6DuSn" +
            //       "6un26eya7Pb6Jeiol9D2DuKes7P9FOKe5ff2EL1GgaSxy5912PD9GvZ3hI6x" +
            //       "y59Zl6Sxy7ua2+ixH/ip3MGz8M5nx+j3zZ+v3PYEFO6ntKbC461pmaTA6YxD" +
            //       "l6Sxy7to2PD9GvZ3hI6xy59Zs/MDD+SrwPL3Gp+d2Pj26KFwprbA3a9qr6ax" +
            //       "HvSbvPwBFPGe6sUF6KFwprbA3a9qsKaxIeSr6u0AGbxbqLyzy653hI6xy59Z" +
            //       "s/f6Eu2a6/kDEL1qr9YHD9mAwtH+/dSLv9UIFOZv7PrS6Lx1pvf6Eu2a6/kD" +
            //       "EL1GgcDAF+ic3PIEEL1GgXXj7fQQ7azcwp61n1mXpM0X6Jzc8gQQ";
        }
    }
    public static string ERROR_PAGE
    {
        get
        {
            return RP + "error_page.aspx";
        }
    }
    public static string RP
    {
        get
        {
            if (null != ConfigurationManager.AppSettings["root_path"])
            {
                return (string)ConfigurationManager.AppSettings["root_path"];
            }
            else
            {
                return "/";
            }
        }
    }
    public static string SRP
    {
        get
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
    public static string HOST
    {
        get
        {
            string _tmp = CommonUtilities.getSYS_SETTING("sys_hostName");
            try { return _tmp != "" ? _tmp : HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); }
            catch (Exception exc) { return _tmp; }
        }
    }
    public static string HOST_SSL
    {
        get
        {
            string _tmp = CommonUtilities.getSYS_SETTING("sys_hostNameSSL");
            return _tmp;// != "" ? _tmp : HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }
    }
    public static string HOST_HA
    {
        get
        {
            string _tmp = CommonUtilities.getSYS_SETTING("rntHA_url");
            return _tmp;
        }
    }
    public static string GOOGLE_MAPS_KEY
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(CommonUtilities.getSYS_SETTING("google_maps_key")))
            {
                return CommonUtilities.getSYS_SETTING("google_maps_key");
            }
            else
            {
                return (string)ConfigurationManager.AppSettings["google_maps_key"];
            }
            //if (null != ConfigurationManager.AppSettings["google_maps_key"])
            //{
            //    return (string)ConfigurationManager.AppSettings["google_maps_key"];
            //}
            //else
            //{
            //    return "ABQIAAAA7xrK1im4yHotWElke3VsVxT2yXp_ZAY8_ufC3CFXhHIE1NvwkxQR82LGNsheJVArfkPqnWPg7Lhlgg";
            //}
        }
    }
    // Lang
    public static string LangCultureName
    {
        get { if (HttpContext.Current.Session["AppLangCultureName"] != null)return (string)HttpContext.Current.Session["AppLangCultureName"]; else return "it-IT"; }
        set { HttpContext.Current.Session["AppLangCultureName"] = value; }
    }
    public static string LangNameTitle
    {
        get { if (HttpContext.Current.Session["AppLangNameTitle"] != null)return (string)HttpContext.Current.Session["AppLangNameTitle"]; else return "Italiano"; }
        set { HttpContext.Current.Session["AppLangNameTitle"] = value; }
    }
    public static string LangCode
    {
        get { if (HttpContext.Current.Session["AppLangCode"] != null)return (string)HttpContext.Current.Session["AppLangCode"]; else return "en"; }
        set { HttpContext.Current.Session["AppLangCode"] = value; }
    }
    public static int LangID
    {
        get { if (HttpContext.Current == null || HttpContext.Current.Session == null) return DefLangID; if (HttpContext.Current.Session["AppLangID"] != null)return (int)HttpContext.Current.Session["AppLangID"]; else return DefLangID; }
        set { HttpContext.Current.Session["AppLangID"] = value; }
    }
    public static int DefLangID
    {
        get { return 2; }
    }
    public static string IdAdMedia
    {
        get { if (HttpContext.Current.Session["IdAdMedia"] != null && !string.IsNullOrEmpty("" + HttpContext.Current.Session["IdAdMedia"])) return "" + HttpContext.Current.Session["IdAdMedia"]; else return ""; }
        set { HttpContext.Current.Session["IdAdMedia"] = value; }
    }
    public static string IdLink
    {
        get { if (HttpContext.Current.Session["IdLink"] != null)return (string)HttpContext.Current.Session["IdLink"]; else return ""; }
        set { HttpContext.Current.Session["IdLink"] = value; }
    }
    public static string IdLastOperator
    {
        get { if (HttpContext.Current.Session["IdLastOperator"] != null)return (string)HttpContext.Current.Session["IdLastOperator"]; else return ""; }
        set { HttpContext.Current.Session["IdLastOperator"] = value; }
    }
    private static UrlList tmpCurrUrlList;
    public static UrlList CurrUrlList
    {
        get
        {
            if (tmpCurrUrlList == null)
                tmpCurrUrlList = new UrlList(App.SRP);
            return tmpCurrUrlList;
        }
        set
        {
            tmpCurrUrlList = value;
        }
    }

    public static UrlList getCurrUrlList(long agentID)
    {
        return new UrlList(App.SRP, "utilsUrlList_WL_" + agentID + ".xml");
    }

    public static long WLAgentId
    {
        get
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                return 0;
            if (HttpContext.Current.Session["WLAgentId"] != null)
                return (long)HttpContext.Current.Session["WLAgentId"];
            else return 0;
        }
        set { HttpContext.Current.Session["WLAgentId"] = value; }
    }

    public static dbRntAgentTBL WLAgent
    {
        get
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                return null;
            if (HttpContext.Current.Session["WLAgent"] != null)
                return (dbRntAgentTBL)HttpContext.Current.Session["WLAgent"];
            else return null;
        }
        set { HttpContext.Current.Session["WLAgent"] = value; }
    }
}
public class AppUtils
{
    public static void FillSiteMapList()
    {
        SiteMapList uList = new SiteMapList(App.SRP);
        uList.Items.Clear();
        UrlList urlList = App.CurrUrlList;
        foreach (UrlItem i in urlList.Items)
        {
            SiteMapItem itemBase = new SiteMapItem(App.HOST + i.Url, DateTime.Now.formatCustom("#yy#-#mm#-#dd#", 1, "2011-01-01"), "daily", "0.8");
            uList.Items.Add(itemBase);
        }
        uList.Save();

        var agentList = rntProps.AgentTBL.Where(x => x.isActive == 1 && !string.IsNullOrEmpty(x.WL_name) && !string.IsNullOrEmpty(x.WL_domainName)).ToList();
        ErrorLog.addLog("", "Count Agent", agentList.Count + "");
        foreach (var tmp in agentList)
        {
            //if (tmp.Key == "localhost") continue;
            uList = new SiteMapList(App.SRP, "sitemap_" + tmp.id + ".xml");
            uList.Items.Clear();
            urlList = App.getCurrUrlList(tmp.id);
            foreach (UrlItem i in urlList.Items)
            {
                SiteMapItem itemBase = new SiteMapItem("http://" + tmp.WL_domainName + i.Url, DateTime.Now.formatCustom("#yy#-#mm#-#dd#", 1, "2011-01-01"), "daily", "0.8");
                uList.Items.Add(itemBase);
            }
            uList.Save();
        }
    }
    public static void FillUrlList()
    {
        List<int> _langIDs = contProps.LangTBL.Where(x => x.is_active == 1 && x.is_public == 1).Select(x => x.id).ToList();
        UrlList uList = App.CurrUrlList;
        uList.Items.Clear();
        AdminUtilities.contStp_fillRewriteTool(ref uList, _langIDs);
        AdminUtilities.locZone_fillRewriteTool(ref uList, _langIDs);
        rntUtils.rntEstate_fillRewriteTool(ref uList, _langIDs);
        AdminUtilities.contTour_fillRewriteTool(ref uList, _langIDs);
        blogUtils.fillArticle_rewriteTool(ref uList, _langIDs);
        blogUtils.fillTag_rewriteTool(ref uList, _langIDs);

        uList.Save();
        App.CurrUrlList = uList;

        FillUrlList_WL();

        FillSiteMapList();
        RiV_WS.fillRewriteTool();
        RiF_WS.fillRewriteTool();
    }

    public static void FillUrlList_WL()
    {
        var agentList = rntProps.AgentTBL.Where(x => x.isActive == 1 && !string.IsNullOrEmpty(x.WL_name) && !string.IsNullOrEmpty(x.WL_domainName)).ToList();
        foreach (var tmp in agentList)
        {
            List<int> _langIDs = contProps.LangTBL.Where(x => x.is_active == 1 && x.is_public == 1).Select(x => x.id).ToList();
            UrlList uList = App.getCurrUrlList(tmp.id);
            uList.Items.Clear();
            AdminUtilities.contStp_fillRewriteTool_WL(ref uList, _langIDs);
            AdminUtilities.locZone_fillRewriteTool_WL(ref uList, _langIDs);
            rntUtils.rntEstate_fillRewriteTool_WL(ref uList, _langIDs, tmp.id);
            uList.Save();
        }
    }


    public static string getStpPageSumary(int id, int pid_lang)
    {
        CONT_VIEW_STP stp = contProps.CONT_STPs.SingleOrDefault(x => x.id == id && x.pid_lang == pid_lang);

        return stp != null ? stp.summary : "";
    }

    public static string getPagePath(string id, string type, string id_lang)
    {
        string pt = type;
        int pid = id.ToInt32();
        //if (AppSettings.CONT_PAGE_PATHs == null) AppSettings.CONT_PAGE_PATHs = new List<AppSettings.CONT_PAGE_PATH>();
        //AppSettings.CONT_PAGE_PATH _path = AppSettings.CONT_PAGE_PATHs.SingleOrDefault(x => x.id == pid && x.lang == id_lang.ToInt32() && x.type == pt);
        //if (_path != null) return _path.path;
        if (pt == "stp")
        {
            if (pid == 1 && id_lang == "2") return "/";
            CONT_VIEW_STP d = contProps.CONT_STPs.SingleOrDefault(x => x.id == pid && x.pid_lang == id_lang.ToInt32());
            if (d != null && !string.IsNullOrEmpty(d.page_path))
            {
                //if (AppSettings.CONT_PAGE_PATHs.SingleOrDefault(x => x.id == pid && x.lang == id_lang.ToInt32() && x.type == pt) == null)
                //    AppSettings.CONT_PAGE_PATHs.Add(new AppSettings.CONT_PAGE_PATH(pid, id_lang.ToInt32(), pt, d.page_path.Trim() == "/" ? CurrentAppSettings.ROOT_PATH : CurrentAppSettings.ROOT_PATH + d.page_path));
                return CurrentAppSettings.ROOT_PATH + d.page_path;
            }
            return "";
        }
        if (pt == "pg_estate")
        {
            RNT_VIEW_ESTATE d = maga_DataContext.DC_RENTAL.RNT_VIEW_ESTATEs.SingleOrDefault(x => x.id == pid && x.pid_lang == id_lang.ToInt32());
            if (d != null && !string.IsNullOrEmpty(d.page_path))
            {
                if (d.pid_city == 2)
                    return "http://www.rentalinflorence.com/" + d.page_path;
                if (d.pid_city == 3)
                    return "http://www.rentalinvenice.com/" + d.page_path;
                if (d.pid_city == 4)
                    return "http://www.rentalinkenya.com/" + d.page_path;
                return CurrentAppSettings.ROOT_PATH + d.page_path;
            }
            return "";
        }
        if (pt == "pg_zone")
        {
            LOC_VIEW_ZONE d = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == pid && x.pid_lang == id_lang.ToInt32());
            if (d != null && !string.IsNullOrEmpty(d.page_path))
            {
                //if (AppSettings.CONT_PAGE_PATHs.SingleOrDefault(x => x.id == pid && x.lang == id_lang.ToInt32() && x.type == pt) == null)
                //    AppSettings.CONT_PAGE_PATHs.Add(new AppSettings.CONT_PAGE_PATH(pid, id_lang.ToInt32(), pt, CurrentAppSettings.ROOT_PATH + d.page_path));
                return CurrentAppSettings.ROOT_PATH + d.page_path;
            }
            return "";
        }
        if (pt == "pg_tour")
        {
            CONT_VIEW_TOUR d = maga_DataContext.DC_CONTENT.CONT_VIEW_TOURs.SingleOrDefault(x => x.id == pid && x.pid_lang == id_lang.ToInt32());
            if (d != null && !string.IsNullOrEmpty(d.page_path))
            {
                //if (AppSettings.CONT_PAGE_PATHs.SingleOrDefault(x => x.id == pid && x.lang == id_lang.ToInt32() && x.type == pt) == null)
                //    AppSettings.CONT_PAGE_PATHs.Add(new AppSettings.CONT_PAGE_PATH(pid, id_lang.ToInt32(), pt, CurrentAppSettings.ROOT_PATH + d.page_path));
                return CurrentAppSettings.ROOT_PATH + d.page_path;
            }
            return "";
        }
        return "";
    }
    public static string getEstatePagePath(int pidEstate)
    {
        var objEstate = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == pidEstate && x.pid_lang == App.LangID);
        if (objEstate != null)
            return App.HOST + App.RP + objEstate.page_path;
        else
            return "#";
    }
    public static string getPageName(string id, string type, string id_lang)
    {
        string pt = type;
        int pid = id.ToInt32();
        if (pt == "stp")
            return contUtils.contStp_title(pid, id_lang.ToInt32(), "");
        if (pt == "pg_estate")
            return CurrentSource.rntEstate_title(pid, id_lang.ToInt32(), "");
        if (pt == "pg_zone")
            return CurrentSource.locZone_title(pid, id_lang.ToInt32(), "");
        if (pt == "pg_tour")
            return CurrentSource.contTour_title(pid, id_lang.ToInt32(), "");
        return "";
    }
}