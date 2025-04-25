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
using EO.Pdf;
using Subgurim.Controles;
using RentalInRome.data;
using ListItem = System.Web.UI.WebControls.ListItem;
using ModRental;
using ModContent;
using System.Net;

public class CurrentAppSettings
{
    public static string EO_PDF_LICENSE
    {
        get
        {
            if (CommonUtilities.getSYS_SETTING("is_take_pdf_variable").objToInt32() == 1)
                return CommonUtilities.getSYS_SETTING("pdf_license");
            else
                return "Wp7l9/YQvUaBpLHLn3XY8P0a9neEjrHLn1mXpLHLu5rb6LEf+KncwbPwzmfH" + "6PfNn6/c9gQU7qe0psPbrWmZpMDpjEOXpLHLu2jY8P0a9neEjrHLn1mz8wMP" + "5KvA8vcan53Y+PbooXCmtsDdr2qvprEe9Ju8/AEU8Z7qxQXooXCmtsDdr2uo" + "prEh5Kvq7QAZvFuovLPLrneEjrHLn1mz9/oS7Zrr+QMQvYfFyP/27qPP9vIX" + "sJLG6eXg+Kmp/gjovHWm9/oS7Zrr+QMQvUaBwMAX6Jzc8gQQvUaBdePt9BDt" + "rNzCnrWfWZekzRfonNzyBBDInbW1x9y0cqy9wu/Abam8yuGzkbOz/RTinuX3" + "9vTjd4SOscufWbPw+g7kp+rp9unsmuz2+iXoqJfQ9g7inrOz/RTi";
            //return "F8idtbXH3LRyrL3C78BtqbzK4bORs7P9FOKe5ff29ON3hI6xy59Zs/D6DuSn" + "6un26eya7Pb6Jeiol9D2DuKes7P9FOKe5ff2EL1GgaSxy5912PD9GvZ3hI6x" + "y59Zl6Sxy7ua2+ixH/ip3MGz8M5nx+j3zZ+v3PYEFO6ntKbC461pmaTA6YxD" + "l6Sxy7to2PD9GvZ3hI6xy59Zs/MDD+SrwPL3Gp+d2Pj26KFwprbA3a9qr6ax" + "HvSbvPwBFPGe6sUF6KFwprbA3a9qsKaxIeSr6u0AGbxbqLyzy653hI6xy59Z" + "s/f6Eu2a6/kDEL1qr9YHD9mAwtH+/dSLv9UIFOZv7PrS6Lx1pvf6Eu2a6/kD" + "EL1GgcDAF+ic3PIEEL1GgXXj7fQQ7azcwp61n1mXpM0X6Jzc8gQQ";
        }
        //get
        //{
        //    return "F8idtbXH3LRyrL3C78BtqbzK4bORs7P9FOKe5ff29ON3hI6xy59Zs/D6DuSn" +
        //           "6un26eya7Pb6Jeiol9D2DuKes7P9FOKe5ff2EL1GgaSxy5912PD9GvZ3hI6x" +
        //           "y59Zl6Sxy7ua2+ixH/ip3MGz8M5nx+j3zZ+v3PYEFO6ntKbC461pmaTA6YxD" +
        //           "l6Sxy7to2PD9GvZ3hI6xy59Zs/MDD+SrwPL3Gp+d2Pj26KFwprbA3a9qr6ax" +
        //           "HvSbvPwBFPGe6sUF6KFwprbA3a9qsKaxIeSr6u0AGbxbqLyzy653hI6xy59Z" +
        //           "s/f6Eu2a6/kDEL1qr9YHD9mAwtH+/dSLv9UIFOZv7PrS6Lx1pvf6Eu2a6/kD" +
        //           "EL1GgcDAF+ic3PIEEL1GgXXj7fQQ7azcwp61n1mXpM0X6Jzc8gQQ";
        //    //return "Ugzrpeb7z7iJWZekscufWZfA8g/jWev9ARC8W7zTv/vjn5mkBxDxrODz/+ih" +
        //    //       "bKW0s8uud4SOscufWbOz8hfrqO7CnrWfWZekzRrxndz22hnlqJfo8h/kdpm6" +
        //    //       "wNywaKm0wtyhWe3pAx7oqOXBs96hWabCnrWfWZekzR7ooOXlBSDxnrXn+fPD" +
        //    //       "otvM+dzAna3I1vTuh87ewg7AdrTAwB7ooOXlBSDxnrWRm+eupeDn9hnynrWR" +
        //    //       "m3Xj7fQQ7azcwp61n1mXpM0X6Jzc8gQQyJ21t8fit3KqucriuHWm8PoO5Kfq" +
        //    //       "6doPvUaBpLHLn3Xj7fQQ7azc6c/nrqXg5/YZ8p7cwp61n1mXpM0=";
        //}
    }
    public static string ERROR_PAGE
    {
        get
        {
            return ROOT_PATH + "error_page.aspx";
        }
    }
    public static string ROOT_PATH
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
    public static string HOST
    {
        get
        {
            return App.HOST;
        }
    }
    public static string HOST_SSL
    {
        get
        {
            return App.HOST_SSL;
        }
    }
    public static string SERVER_ROOT_PATH
    {
        get
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
    public static string GOOGLE_MAPS_KEY
    {
        get
        {
            if (null != ConfigurationManager.AppSettings["google_maps_key"])
            {
                return (string)ConfigurationManager.AppSettings["google_maps_key"];
            }
            else
            {
                return "ABQIAAAA7xrK1im4yHotWElke3VsVxT2yXp_ZAY8_ufC3CFXhHIE1NvwkxQR82LGNsheJVArfkPqnWPg7Lhlgg";
            }
        }
    }
    public static bool CITY_HAS_AREA
    {
        get
        {
            if (null != ConfigurationManager.AppSettings["city_has_area"] && (string)ConfigurationManager.AppSettings["city_has_area"] == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public static string JSCAL_DATE_FORMAT
    {
        get
        {
            if (null != ConfigurationManager.AppSettings["jscal_date_format"])
            {
                return (string)ConfigurationManager.AppSettings["jscal_date_format"];
            }
            else
            {
                return "%d %B %Y";
            }
        }
    }
    public static string Date_DateToString(DateTime _date)
    {
        string _m = "" + _date.Month;
        string _d = "" + _date.Day;
        if (_m.Length == 1) _m = "0" + _m;
        if (_d.Length == 1) _d = "0" + _d;
        return "" + _d + "/" + _m + "/" + _date.Year;
    }
    public static DateTime Date_StringToDate(string _date)
    {
        return new DateTime(Convert.ToInt32(_date.Split('/')[2]), Convert.ToInt32(_date.Split('/')[1]), Convert.ToInt32(_date.Split('/')[0]));
    }
}


public class CurrentSource
{
    public static string contTour_title(int id, int lang, string alternate)
    {
        CONT_VIEW_TOUR _s = maga_DataContext.DC_CONTENT.CONT_VIEW_TOURs.SingleOrDefault(x => x.id == id && x.pid_lang == lang);
        if (_s == null)
            _s = maga_DataContext.DC_CONTENT.CONT_VIEW_TOURs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        if (_s == null)
            _s = maga_DataContext.DC_CONTENT.CONT_VIEW_TOURs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        if (_s != null)
            return _s.title;
        return alternate;
    }
    public static string loc_cityTitle(int id, int lang, string alternate)
    {        
        LOC_VIEW_CITY _s = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.SingleOrDefault(x => x.id == id && x.pid_lang == lang);
        if (_s == null)
            _s = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        if (_s == null)
            _s = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        if (_s != null)
            return _s.title;
        return alternate;
    }
    public static int locZone_countEstate(int id)
    {
        if (id != 0)
            return AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.pid_zone == id).Count();
        return AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.pid_zone.HasValue && !AppSettings._LOC_CUSTOM_ZONEs.Contains(x.pid_zone.Value)).Count();
    }

    public static int locZone_countEstate_WL(int id, List<int> estateIds)
    {
        if (id != 0)
            return AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.pid_zone == id && estateIds.Contains(x.id)).Count();
        return 0;// AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.pid_zone.HasValue && !AppSettings._LOC_CUSTOM_ZONEs.Contains(x.pid_zone.Value)).Count();
    }

    public static int locZone_countEstate_WL(int id)
    {
        if (id != 0)
        {
            List<int> lstAgentEstateIds = new List<int>();
            using (DCmodRental dc = new DCmodRental())
            {
                lstAgentEstateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == App.WLAgentId).Select(x => x.pidEstate).Distinct().ToList();
            }
            return AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.pid_zone == id && lstAgentEstateIds.Contains(x.id)).Count();
        }
        return 0;// AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.pid_zone.HasValue && !AppSettings._LOC_CUSTOM_ZONEs.Contains(x.pid_zone.Value)).Count();
    }

    public static string locZone_title(int id, int lang, string alternate)
    {
        //LOC_VIEW_ZONE _s = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == lang);
        //if (_s == null)
        //    _s = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        //if (_s == null)
        //    _s = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        LOC_VIEW_ZONE _s = AppSettings.LOC_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == lang);
        if (_s == null)
            _s = AppSettings.LOC_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        if (_s == null)
            _s = AppSettings.LOC_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        if (_s != null)
        {
            return _s.title;
        }
        return alternate;
    }
    public static string locZone_img_preview(int id, int lang, string alternate)
    {
        //LOC_VIEW_ZONE _s = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == lang);
        //if (_s == null)
        //    _s = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        //if (_s == null)
        //    _s = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        LOC_VIEW_ZONE _s = AppSettings.LOC_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == lang);
        if (_s == null)
            _s = AppSettings.LOC_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        if (_s == null)
            _s = AppSettings.LOC_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        if (_s != null)
        {
            return _s.img_preview;
        }
        return alternate;
    }
    //public static string getCity_titleOfRegion(int id, int pidLang, string def)
    //{
    //    //var tmp = locProps.CityTB.FirstOrDefault(x => x.id == id);
    //    //if (tmp == null) return def;
    //    //return getRegion_title(tmp.pidRegion.objToInt32(), pidLang, def);
    //}
    public static string rntEstate_code(int id, string alternate)
    {
        RNT_TB_ESTATE _s = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.code;
        return alternate;
    }
    public static string rntService_title(int id, int lang, string alternate)
    {
        //LOC_VIEW_ZONE _s = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == lang);
        //if (_s == null)
        //    _s = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        //if (_s == null)
        //    _s = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        using (DCmodRental dc = new DCmodRental())
        {
            dbRntEstateExtrasLN _s = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == id && x.pidLang == lang);
            if (_s == null)
                _s = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == id && x.pidLang == 2);
            if (_s == null)
                _s = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == id && x.pidLang == 1);
            if (_s != null)
            {
                return _s.title;
            }
            return alternate;
        }
    }
    public static string getresCode(long id)
    {
        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;

        RNT_TBL_RESERVATION currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == id);
        if (currRes != null)
        {
            return currRes.code;
        }
        else
        {
            return "";
        }
    }
    public static string rntEstate_title(int id, int lang, string alternate)
    {
        RNT_LN_ESTATE _s = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == lang);
        if (_s == null)
            _s = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == 2);
        if (_s == null)
            _s = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == 1);
        if (_s != null)
        {
            return _s.title;
        }
        return alternate;
    }

    public static string rntEstate_title_agency(RNT_TB_ESTATE estTB, long agentID, int lang, string alternate)
    {
        if (agentID == 1016)
        {
            #region Old Code
            //using (DCmodRental dc = new DCmodRental())
            //{
            //    string bcomHotelId = estTB.bcomHotelId + "";
            //    var bcomHotel = dc.dbRntBcomHotelTBLs.FirstOrDefault(x => x.hotelId == bcomHotelId);
            //    if (bcomHotel != null)
            //    {
            //        return bcomHotel.title;
            //    }
            //} 
            #endregion

            if (!string.IsNullOrEmpty(estTB.bcomName))
            {
                return estTB.bcomName;
            }
        }
        return rntEstate_title(estTB.id, lang, "---");
    }

    public static string rntEstate_summary(int id, int lang, string alternate)
    {
        RNT_LN_ESTATE _s = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == lang);
        if (_s == null)
            _s = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == 2);
        if (_s == null)
            _s = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == 1);
        if (_s != null)
        {
            return _s.summary;
        }
        return alternate;
    }
    public static string rntEstate_titleWithzone(int id, int lang, string separator, bool estateFirst, string alternate)
    {
        RNT_TB_ESTATE _est = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id);
        if (_est == null) return alternate;
        string _estTitle = rntEstate_title(id, lang, "");
        string _zoneTitle = locZone_title(_est.pid_zone.objToInt32(), lang, "");
        if (_estTitle == "") return alternate;
        if (_zoneTitle == "") return _estTitle;
        return estateFirst ? (_estTitle + separator + _zoneTitle) : (_zoneTitle + separator + _estTitle);
    }
    public static bool rntEstate_hasConfig(int pidEstate, int pidConfig)
    {
        return maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == pidEstate && x.pid_config == pidConfig && x.is_HomeAway == 0).Count() > 0;
    }
    public static string rnt_configTitle(int id, int lang, string alternate)
    {
        RNT_VIEW_CONFIG _s = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.SingleOrDefault(x => x.id == id && x.pid_lang == lang);
        if (_s == null)
            _s = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        if (_s == null)
            _s = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        if (_s != null)
            return _s.title;
        return alternate;
    }
    public static string rntPeriod_title(int id, int lang, string alternate)
    {
        RNT_VIEW_PERIOD _s = AppSettings.RNT_PERIODs.SingleOrDefault(x => x.id == id && x.pid_lang == lang);
        if (_s == null)
            _s = AppSettings.RNT_PERIODs.SingleOrDefault(x => x.id == id && x.pid_lang == 2);
        if (_s == null)
            _s = AppSettings.RNT_PERIODs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
        if (_s != null)
            return _s.title;
        return alternate;
    }
    public static string getSysLangValue(string Name)
    {
        return contUtils.getLabel_title(Name, CurrentLang.ID, Name);
    }
    public static string getSysLangValue(string Name, string alternate)
    {
        return contUtils.getLabel_title(Name, CurrentLang.ID, alternate);
    }
    public static string getSysLangValue(string Name, int _lang)
    {
        return contUtils.getLabel_title(Name, _lang, Name);
    }
    public static string getSysLangValue(string Name, int pid_lang, string alternate)
    {
        return contUtils.getLabel_title(Name, pid_lang, alternate);
    }
    public static string getPageName(string id, string type, string id_lang)
    {
        return AppUtils.getPageName(id + "", type, id_lang);
    }
    public static string getPagePath(string id, string type, string id_lang)
    {
        return AppUtils.getPagePath(id + "", type, id_lang);
    }
}
public class CommonUtilities
{
    public static void downloadPdfFromUrl(string url, string fileName, float marginLeft, float marginTop, float marginRight, float marginBottom)
    {
        EO.Pdf.Runtime.AddLicense(CurrentAppSettings.EO_PDF_LICENSE);

        //set new TLS protocol 1.1/1.2
        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  

        SizeF pageSize = PdfPageSizes.A4;
        bool autoFitWidth = true;

        //Set page layout arguments
        HtmlToPdf.Options.PageSize = pageSize;
        HtmlToPdf.Options.OutputArea = new RectangleF(
            marginLeft, marginTop,
            pageSize.Width - marginLeft - marginRight,
            pageSize.Height - marginTop - marginBottom);
        //HtmlToPdf.Options.AutoFitWidth = autoFitWidth;
        //HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.ScaleToFit;

        PdfDocument doc = new PdfDocument();


        //Convert the first Url into the PdfDocument object
        HtmlToPdf.Options.NoScript = false;
        HtmlToPdf.Options.NoCache = true;
        HtmlToPdf.Options.StartPosition = 0;
        HtmlToPdf.Options.ProxyInfo = EO.Base.ProxyInfo.Direct;
        //HtmlToPdf.ConvertUrl(url, doc);
        HtmlToPdf.Options.Follow(HtmlToPdf.ConvertUrl(url, doc));

        //Stream fileStream = Stream
        //PdfDocInfo _info = doc.Save()
        //Save the PDF file
        // get the pdf bytes from html string
        //byte[] downloadBytes = doc.Bookmarks.GetPdfBytesFromHtmlString(htmlCodeToConvert, baseUrl);

        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.Clear();
        response.AddHeader("Content-Type", "binary/octet-stream");
        response.AddHeader("Content-Disposition",
            "attachment; filename=" + fileName + ";");
        response.Flush();
        doc.Save(response.OutputStream);
        //response.BinaryWrite(downloadBytes);
        response.Flush();
        response.End();
    }
    public static string savePdfFromUrl(string url, string serverPath, float marginLeft, float marginTop, float marginRight, float marginBottom)
    {
        try
        {
            
            EO.Pdf.Runtime.AddLicense(CurrentAppSettings.EO_PDF_LICENSE);
            //EO.Pdf.Runtime.AddLicense("3678935979");

            //set new TLS protocol 1.1/1.2
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  

            SizeF pageSize = PdfPageSizes.A4;
            bool autoFitWidth = true;

            //Set page layout arguments
            HtmlToPdf.Options.PageSize = pageSize;
            HtmlToPdf.Options.OutputArea = new RectangleF(
                marginLeft, marginTop,
                pageSize.Width - marginLeft - marginRight,
                pageSize.Height - marginTop - marginBottom);
            //HtmlToPdf.Options.AutoFitWidth = autoFitWidth;
            //HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.ScaleToFit;
            //HtmlToPdf.Options.
            PdfDocument doc = new PdfDocument();


            //Convert the first Url into the PdfDocument object
            HtmlToPdf.Options.NoScript = false;
            HtmlToPdf.Options.NoCache = true;
            HtmlToPdf.Options.StartPosition = 0;
            HtmlToPdf.Options.ProxyInfo = EO.Base.ProxyInfo.Direct;
            HtmlToPdf.Options.Follow(HtmlToPdf.ConvertUrl(url, doc));

            //Stream fileStream = Stream
            //PdfDocInfo _info = doc.Save()
            //Save the PDF file
            // get the pdf bytes from html string
            //byte[] downloadBytes = doc.Bookmarks.GetPdfBytesFromHtmlString(htmlCodeToConvert, baseUrl);
            doc.Save(serverPath);
            return "ok";

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static string newUniqueID()
    {
        return Guid.NewGuid().ToString().Replace("-", "_").Replace("1", "q").Replace("2", "w").Replace("3", "e").Replace("4", "r").Replace("5", "t").Replace("6", "y").Replace("7", "u").Replace("8", "i").Replace("9", "o").Replace("0", "p");
    }
    public static string CURRENT_REFERRER
    {
        get
        {
            string returnValue = "";
            if (HttpContext.Current.Session["CURRENT_REFERRER"] != null)
            {
                returnValue = (string)HttpContext.Current.Session["CURRENT_REFERRER"];
            }
            return returnValue;
        }
        set
        {
            HttpContext.Current.Session["CURRENT_REFERRER"] = value;
        }
    }
    public static string getPager(int allCount, int numPerPage, int currPage, int pagerCount, string allCurrStr, string allLnkStr, string currStr, string lnkStr, string prevStr, string nextStr)
    {
        if (numPerPage <= 0) numPerPage = 1;
        int _pagesCount = 1;
        int _currPageCount = numPerPage;
        while (allCount > _currPageCount)
        {
            _pagesCount++;
            allCount -= numPerPage;
        }
        string _pager = "";
        if (_pagesCount <= pagerCount)
        {
            for (int i = 1; i <= _pagesCount; i++)
            {
                _pager += i == currPage ? string.Format(currStr, i) : string.Format(lnkStr, i);
            }
            if (_pagesCount > 1)
                _pager += 0 == currPage ? string.Format(allCurrStr, 0) : string.Format(allLnkStr, 0);
            return _pager;
        }
        int _grCount = 0;
        int tmpPage = currPage == 0 ? 1 : currPage;
        while (tmpPage > _grCount * pagerCount)
        {
            _grCount++;
        }
        int _tmpCurrPage = (_grCount - 1) * pagerCount;
        if (_grCount > 1)
        {
            _pager += string.Format(lnkStr, "1");
            _pager += string.Format(prevStr, "" + (_tmpCurrPage - 1));
        }
        for (int i = (_grCount - 1) * pagerCount; i <= _grCount * pagerCount && i <= _pagesCount; i++)
        {
            if (i != 0)
                _pager += i == currPage ? string.Format(currStr, i) : string.Format(lnkStr, i);
            _tmpCurrPage++;
        }
        if (_tmpCurrPage <= _pagesCount)
        {
            _pager += string.Format(nextStr, "" + (_tmpCurrPage));
            _pager += string.Format(lnkStr, "" + _pagesCount);
        }
        _pager += 0 == currPage ? string.Format(allCurrStr, 0) : string.Format(allLnkStr, 0);
        return _pager;
    }
    public static string GetSearchQuery(string u)
    {
        // 1
        // Try to match start of query with "&q=". These matches are ideal.
        int start = u.IndexOf("&q=", StringComparison.Ordinal);
        int length = 3;
        // 2
        // Try to match part with q=. This may be prefixed by another letter.
        if (start == -1)
        {
            start = u.IndexOf("q=", StringComparison.Ordinal);
            length = 2;
        }
        // 3
        // Try to match start of query with "p=".
        if (start == -1)
        {
            start = u.IndexOf("p=", StringComparison.Ordinal);
            length = 2;
        }
        // 4
        // Try to match start of query with "qs=".
        if (start == -1)
        {
            start = u.IndexOf("qs=", StringComparison.Ordinal);
            length = 3;
        }
        // Return if not possible
        if (start == -1)
        {
            return string.Empty;
        }
        // 5
        // Advance N characters
        start += length;
        // 6
        // Find first & after that
        int end = u.IndexOf('&', start);
        // 7
        // Use end index if no & was found
        if (end == -1)
        {
            end = u.Length;
        }
        // 8
        // Get substring between two parameters
        string sub = u.Substring(start, end - start);
        // 9
        // Get the decoded URL
        string result = HttpUtility.UrlDecode(sub);
        // 10
        // Get the HTML representation
        result = HttpUtility.HtmlEncode(result);
        // 11
        // Prepend sitesearch label to output
        if (u.IndexOf("sitesearch", StringComparison.Ordinal) != -1)
        {
            result = "sitesearch: " + result;
        }
        return result;
    }
    public static string GetUserName(string id)
    {
        int _id;
        if (int.TryParse(id, out _id))
        {
            USR_ADMIN user = maga_DataContext.DC_USER.USR_ADMIN.SingleOrDefault(item => item.id == _id);
            if (user != null)
                return user.name + " " + user.surname;
        }
        return "";
    }
    public static string Get_nomeUser_(string id_user)
    {
        if (id_user == "" || id_user == "null") return "";
        USR_ADMIN u = maga_DataContext.DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == Convert.ToInt32(id_user));
        if (u != null) return u.surname + " " + u.name;
        return "";
    }
    public static string GetRequestStateTitle(string pid_state)
    {
        int _id;
        if (int.TryParse(pid_state, out _id))
        {
            //LK_REQUEST_STATE state = maga_DataContext.DC_ITEM.LK_REQUEST_STATEs.SingleOrDefault(item => item.id == _id);
            //if (state != null)
            //    return state.title;
        }
        return "";
    }

    public static string GetDateShortString(string date)
    {
        DateTime dt;
        if (DateTime.TryParse(date, out dt))
        {
            return dt.ToShortDateString();
        }
        return "";
    }

    public static string Date_toJSCalString(DateTime date)
    {
        string returnValue = "";
        string _m = "" + date.Month;
        string _d = "" + date.Day;
        if (_m.Length == 1) _m = "0" + _m;
        if (_d.Length == 1) _d = "0" + _d;
        returnValue = "" + date.Year + "-" + _m + "-" + _d;
        return returnValue;
    }
    public static int Date_toJSCalInt(DateTime date)
    {
        int returnValue = 0;
        string _y = "" + date.Year;
        string _m = "" + date.Month;
        string _d = "" + date.Day;
        if (_m.Length == 1) _m = "0" + _m;
        if (_d.Length == 1) _d = "0" + _d;
        returnValue = Convert.ToInt32(_y + _m + _d);
        return returnValue;
    }
    public static DateTime JSCalString_toDate(string date)
    {
        return new DateTime(Convert.ToInt32(date.Split('-')[0]), Convert.ToInt32(date.Split('-')[1]), Convert.ToInt32(date.Split('-')[2]));
    }
    public static int GetIndexOfDrpValue(ref DropDownList drp, string value)
    {
        int index = -1;
        foreach (ListItem item in drp.Items)
        {
            if (item.Value == value)
            {
                index = drp.Items.IndexOf(item);
                break;
            }
        }
        return index;
    }
    public static int GetIndexOfDrpText(ref DropDownList drp, string text)
    {
        int index = -1;
        foreach (ListItem item in drp.Items)
        {
            if (item.Text == text)
            {
                index = drp.Items.IndexOf(item);
                break;
            }
        }
        return index;
    }

    public static string GetStaticBlockName(string id)
    {
        int _id;
        if (int.TryParse(id, out _id))
        {
            var c = maga_DataContext.DC_CONTENT.CONT_TBL_BLOCKs.SingleOrDefault(item => item.id == _id);
            if (c != null)
                return c.block_name;
        }
        return "";
    }

    public static string CutString(string str, int charNum)
    {
        if (str == null || str == "") { return ""; }
        if (str.Length > charNum)
        {
            str = str.Substring(0, charNum) + "";
            var strArray = str.Split(' ');
            str = "";
            for (var i = 0; i < strArray.Length - 1; i++)
            {
                str += strArray[i] + " ";
            }
            str += "...";
        }
        return str;
    }
    public static string GetAttivoString(string active)
    {
        return active == "1" ? "Attivo" : "Non Attivo";
    }
    public static string GetYesNoSpan(string active)
    {
        return active == "1" ? "<span style='color:green;'>SI</span>" : "<span style='color:red;'>NO</span>";
    }
    public static GIcon GetGoogleMapsIcon()
    {
        GIcon icon = new GIcon();
        icon.image = CurrentAppSettings.ROOT_PATH + "images/google_maps/google_icon_logo.png";
        icon.shadow = CurrentAppSettings.ROOT_PATH + "images/google_maps/google_icon_shadow.png";
        icon.iconSize = new GSize(82, 45);
        icon.shadowSize = new GSize(82, 45);
        icon.iconAnchor = new GPoint(6, 45);
        icon.infoWindowAnchor = new GPoint(5, 1);
        List<GPoint> _p = new List<GPoint>();
        _p.Add(new GPoint(1, 6));
        _p.Add(new GPoint(1, 45));
        _p.Add(new GPoint(82, 45));
        _p.Add(new GPoint(82, 6));
        icon.imageMap = _p;
        return icon;
    }
    public static GMarker GetGoogleMapsMarker(GLatLng latlng, bool graggable)
    {
        GMarkerOptions mkrOpts = new GMarkerOptions();
        mkrOpts.draggable = graggable;
        mkrOpts.icon = CommonUtilities.GetGoogleMapsIcon();
        GMarker mkr = new GMarker(latlng, mkrOpts);
        mkr.AddMarkerTracker();
        return mkr;
    }
    public static string getSYS_SETTING(string name)
    {
        var _d = AppSettings.DEF_SYS_SETTINGs.SingleOrDefault(x => x.name == name);
        if (_d != null)
            return _d.value;
        return "";
    }
    public static void setSYS_SETTING(string name, string value)
    {
        using (DCmodContent dc = new DCmodContent())
        {
            var currTBL = dc.dbContSysConfigTBs.SingleOrDefault(x => x.name == name);
            if (currTBL == null)
            {
                currTBL = new dbContSysConfigTB();
                currTBL.name = name;
                dc.Add(currTBL);
            }
            currTBL.value = value;
            dc.SaveChanges();
            AppSettings.DEF_SYS_SETTINGs = dc.dbContSysConfigTBs.ToList();
        }
    }
    public static string CreatePassword(int _charCount, bool _repeat, bool _numeric, bool _upperCase)
    {
        string _pwd = "";
        string _pwdLower = "qwertyuiopasdfghjklzxcvbnm";
        string _pwdNumeric = "0123456789";
        string _pwdUpper = "QWERTYUIOPASDFGHJKLZXCVBNM";
        string _allowedChars = _pwdLower;
        if (_numeric)
            _allowedChars += _pwdNumeric;
        if (_upperCase)
            _allowedChars += _pwdUpper;
        Random rnd = new Random();
        while (_pwd.Length < _charCount)
        {
            int i = rnd.Next(_allowedChars.Length - 1);
            _pwd += _allowedChars[i];
            if (!_repeat)
                _allowedChars.Remove(i, 1);
        }
        return _pwd;
    }

    public static decimal GetChangedAmt(decimal amt, int changeAmt, int changeIsDiscount, int changeIsPercentage)
    {
        decimal finalChangeAmt = 0;
        if (changeIsPercentage == 1)
        {
            finalChangeAmt = ((amt * changeAmt) / 100);
        }
        else
        {
            finalChangeAmt = changeAmt;
        }
        finalChangeAmt = (changeIsDiscount == 1) ? -finalChangeAmt : finalChangeAmt;
        return amt + finalChangeAmt;
    }

    public static Dictionary<int, string> GetIsActiveList()
    {
        Dictionary<int, string> dic = new Dictionary<int, string>();
        dic.Add(1, contUtils.getLabel("lblYes", App.LangID, "SI"));
        dic.Add(0, contUtils.getLabel("lblNo", App.LangID, "NO"));

        return dic;
    }

    public static Dictionary<int, string> GetIsActiveListWithDef()
    {
        Dictionary<int, string> dic = new Dictionary<int, string>();
        dic.Add(-1, "--");
        dic.Add(1, contUtils.getLabel("lblYes", App.LangID, "SI"));
        dic.Add(0, contUtils.getLabel("lblNo", App.LangID, "NO"));

        return dic;
    }

    public static Dictionary<string, string> GetIsActiveListWithDefault(string valDef, string valYes, string valNo, string txtDef = "--")
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add(valDef, txtDef);
        dic.Add(valYes, contUtils.getLabel("lblYes", App.LangID, "SI"));
        dic.Add(valNo, contUtils.getLabel("lblNo", App.LangID, "NO"));

        return dic;
    }

    public static Dictionary<int, string> GetIsActiveListExport()
    {
        Dictionary<int, string> dic = new Dictionary<int, string>();
        dic.Add(0, contUtils.getLabel("lblNo", App.LangID, "NO"));
        dic.Add(1, contUtils.getLabel("lblYes", App.LangID, "SI"));
        dic.Add(-1, contUtils.getLabel("lblAll", App.LangID, "--"));

        return dic;
    }
}

public class WL
{
    public static string getWLMainColor()
    {
        var agent = rntProps.AgentTBL.FirstOrDefault(x => x.id == App.WLAgentId);
        if (agent != null)
            return agent.WL_mainColor;
        return "";
    }

    public static string getWLLogo()
    {
        var agent = rntProps.AgentTBL.FirstOrDefault(x => x.id == App.WLAgentId);
        if (agent != null)
            return agent.WL_logo;
        return "";
    }

    public static string getWLCSS()
    {
        var agent = rntProps.AgentTBL.FirstOrDefault(x => x.id == App.WLAgentId);
        if (agent != null)
            return agent.WL_css;
        return "";
    }

    public static string getWLMapMarker()
    {
        var agent = rntProps.AgentTBL.FirstOrDefault(x => x.id == App.WLAgentId);
        if (agent != null)
            return agent.WL_mapMarker;
        return "";
    }

    public static string getWLName()
    {
        var agent = rntProps.AgentTBL.FirstOrDefault(x => x.id == App.WLAgentId);
        if (agent != null)
            return agent.WL_name;
        return "";
    }

 
}
