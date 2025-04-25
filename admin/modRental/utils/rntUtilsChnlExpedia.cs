using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ModInvoice;
using ModRental;
using System.Net.Security;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using RentalInRome.data;
using Newtonsoft.Json.Converters;
using System.Xml.Linq;
using ModAuth;
using HtmlAgilityPack;
using System.Xml;
using System.Web.Script.Serialization;
using System.Globalization;
using ModContent;

public class ChnlExpediaUtils
{
    //This One Is Updated. for Older check FTP.
    public static bool CheckForService()
    {
        if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Response == null) return false;
        var Request = HttpContext.Current.Request;
        var Response = HttpContext.Current.Response;
        if (!Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/expedia/")) return false;
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetAllowResponseInBrowserHistory(true);
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "text/xml";
        byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
        string xml = Encoding.ASCII.GetString(param);
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/expedia/masterdata"))
        {
            //Response.Write(ChnlExpediaService.MasterDataRequest());
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/expedia/priceinfo"))
        {
            //.Write(ChnlExpediaService.PriceInformationDataRequest());
        }

        Response.End();
        return true;
    }

    public static void SetTimers()
    {
        timerChnlExpediaImport = new System.Timers.Timer();
        timerChnlExpediaImport.Interval = (1000 * 60 * 2); // first after 2 mins
        timerChnlExpediaImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlExpediaImport_Elapsed);
        timerChnlExpediaImport.Start();

        if (CommonUtilities.getSYS_SETTING("is_clear_log") == "true" || CommonUtilities.getSYS_SETTING("is_clear_log") == "1")
        {
            timerClearLog = new System.Timers.Timer();
            timerClearLog.Interval = (1000 * 60 * 5); // first after 5 mins
            timerClearLog.Elapsed += new System.Timers.ElapsedEventHandler(timerClearLog_Elapsed);
            timerClearLog.Start();
        }
    }

    public static void StopTimers()
    {
        try
        {
            if (timerChnlExpediaImport != null)
            {
                timerChnlExpediaImport.Stop();
                timerChnlExpediaImport.Dispose();
            }

            if (timerClearLog != null)
            {
                timerClearLog.Stop();
                timerClearLog.Dispose();
            }
        }
        catch (Exception ex)
        {
            ErrorLog.addLog("", "ChnlExpediaUtils.StopTimers", ex.ToString() + "");
        }
    }

    static System.Timers.Timer timerClearLog;
    static void timerClearLog_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        ClearLog_start(); // only once
        timerClearLog.Dispose();
        timerClearLog = new System.Timers.Timer();
        timerClearLog.Interval = (1000 * 60 * 30); // each 15 mins
        timerClearLog.Elapsed += new System.Timers.ElapsedEventHandler(timerClearLog_Elapsed);
        timerClearLog.Start();
    }
    private class ClearLog_process
    {
        void doThread()
        {
            try
            {
                var dt = DateTime.Now.AddDays(-4);
                using (magaChnlExpediaDataContext dc = maga_DataContext.DC_EXP)
                {
                    dc.RntChnlExpediaRequestLOG.DeleteAllOnSubmit(dc.RntChnlExpediaRequestLOG.Where(x => x.logDateTime <= dt));
                    dc.SubmitChanges();
                }
                addLog("CLEAR LOG", "till " + dt, "", "", "");
            }
            catch (Exception Ex)
            {
                ErrorLog.addLog("", "ChnlExpediaUtils.ClearLog_process", Ex.ToString() + "");
            }
        }
        public ClearLog_process()
        {
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaUtils.ClearLog_process");

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }
    }
    public static void ClearLog_start()
    {
        ClearLog_process _tmp = new ClearLog_process();
    }

    static System.Timers.Timer timerChnlExpediaImport;
    static void timerChnlExpediaImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        if (RentalInRome.Global.isOnDev && CommonUtilities.getSYS_SETTING("rntChnlExpediaImportOnDev") != "true" && CommonUtilities.getSYS_SETTING("rntChnlExpediaImportOnDev").ToInt32() != 1) return;
        int rntChnlExpediaImportEachMins = CommonUtilities.getSYS_SETTING("rntChnlExpediaImportEachMins").ToInt32();
        if (rntChnlExpediaImportEachMins == 0) rntChnlExpediaImportEachMins = 15;
        ChnlExpediaImport.BookingRetrieval_all(1);
        timerChnlExpediaImport.Dispose();
        timerChnlExpediaImport = new System.Timers.Timer();
        timerChnlExpediaImport.Interval = (1000 * 60 * rntChnlExpediaImportEachMins); // each 15 mins
        timerChnlExpediaImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlExpediaImport_Elapsed);
        timerChnlExpediaImport.Start();
    }

    public static void addLog(string requestType, string requestComments, string requestContent, string responseContent, string requesUrl)
    {
        try
        {
            using (magaChnlExpediaDataContext dc = maga_DataContext.DC_EXP)
            {
                var item = new RntChnlExpediaRequestLOG();
                item.uid = Guid.NewGuid();
                item.requesUrl = requesUrl;
                item.requestType = requestType;
                item.requestComments = requestComments;
                item.requestContent = requestContent;
                item.responseContent = responseContent;
                item.logDateTime = DateTime.Now;
                dc.RntChnlExpediaRequestLOG.InsertOnSubmit(item);
                dc.SubmitChanges();
            }
        }
        catch (Exception Ex)
        {
            ErrorLog.addLog("", "ChnlExpediaUtils.addLog", Ex.ToString() + "");
        }
    }
    public static string SendRequest(String requesUrl, String requestContent, out string ErrorString)
    {
        ErrorString = "";
        try
        {
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            obj.ContentType = "application/json";
            obj.Method = "GET";
            if (requestContent != "")
            {
                obj.Method = "POST";
                Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestContent);
                obj.ContentLength = bytes.Length;
                Stream os = obj.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();
            }
            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            String strRSString = _Answer.ReadToEnd().ToString();
            if (CommonUtilities.getSYS_SETTING("rntChnlExpediaDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlExpediaDebug").ToInt32() == 1)
                addLog("Debug", "SendRequest", requestContent, strRSString, requesUrl);
            return strRSString;
        }
        catch (WebException ex)
        {
            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                ErrorString = reader.ReadToEnd();
                addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
                return "";
            }
        }
        catch (Exception ex)
        {
            ErrorString = ex.ToString();
            addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
            return "";
        }
    }

    public static string SendRequestContent(String requesUrl, string requestType, String requestContent, out string ErrorString)
    {
        ErrorString = "";
        try
        {

            //string sfpLogin = "EQC15070158MAG";
            //string sfpPassword = "q202211v";

            string sfpLogin = "EQC_Magarental";
            string sfpPassword = "ApiMaga2018@";

            if (CommonUtilities.getSYS_SETTING("is_expedia_content_demo").objToInt32() == 1)
            {
                sfpLogin = "EQCtest12933870";
                sfpPassword = "ew67nk33";
            }

            //set new TLS protocol 1.1
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

            string authInfo = string.Format("{0}:{1}", sfpLogin, sfpPassword);
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            obj.ContentType = "application/json";
            obj.Method = requestType;
            obj.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            if (requestContent != "")
            {
                Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestContent);
                obj.ContentLength = bytes.Length;
                Stream os = obj.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();
            }

            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            String strRSString = _Answer.ReadToEnd().ToString();
            if (CommonUtilities.getSYS_SETTING("rntChnlExpediaDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlExpediaDebug").ToInt32() == 1)
                addLog("Debug", "SendRequest", requestContent, strRSString, requesUrl);
            return strRSString;
        }
        catch (WebException ex)
        {
            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                ErrorString = reader.ReadToEnd();
                addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
                ErrorLog.addLog("", "exp. error", ErrorString);
                return "";
            }
        }
        catch (Exception ex)
        {
            ErrorString = ex.ToString();
            ErrorLog.addLog("", "exp. error", ErrorString);
            addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
            return "";
        }
    }

    public static string SendRequestRoomType(String requesUrl, string requestType, String requestContent, out string ErrorString)
    {
        ErrorString = "";
        try
        {
            addLog("Debug", "SendRequest", requestContent, "start", requesUrl);
            //string sfpLogin = "EQC15070158MAG";
            //string sfpPassword = "q202211v";

            string sfpLogin = "EQC_Magarental";
            string sfpPassword = "ApiMaga2018@";

            if (CommonUtilities.getSYS_SETTING("is_expedia_content_demo").objToInt32() == 1)
            {
                sfpLogin = "EQCtest12933870";
                sfpPassword = "ew67nk33";
            }

            string authInfo = string.Format("{0}:{1}", sfpLogin, sfpPassword);

            //set new TLS protocol 1.1
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            //obj.ContentType = "application/json";
            obj.ContentType = "application/vnd.expedia.eps.product-v2+json";
            obj.Method = requestType;
            obj.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            if (requestContent != "")
            {
                Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestContent);
                obj.ContentLength = bytes.Length;
                Stream os = obj.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();
            }

            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            String strRSString = _Answer.ReadToEnd().ToString();
            if (CommonUtilities.getSYS_SETTING("rntChnlExpediaDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlExpediaDebug").ToInt32() == 1)
                addLog("Debug", "SendRequest", requestContent, strRSString, requesUrl);
            return strRSString;
        }
        catch (WebException ex)
        {
            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                ErrorString = reader.ReadToEnd();
                addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
                ErrorLog.addLog("", "exp. error", ErrorString);
                return "";
            }
        }
        catch (Exception ex)
        {
            ErrorString = ex.ToString();
            ErrorLog.addLog("", "exp. error", ErrorString);
            addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
            return "";
        }
    }

    //public static void updateEstateFromMagarental(RNT_TB_ESTATE currEstate)
    //{
    //    using (DCmodRental dcRnt = new DCmodRental())
    //    using (DCchnlExpedia dcChnl = new DCchnlExpedia())
    //    {
    //        int IdEstate = currEstate.id;
    //        var currExpedia = dcChnl.dbRntChnlExpediaEstateTBs.SingleOrDefault(x => x.id == IdEstate);
    //        if (currExpedia == null)
    //        {
    //            currExpedia = new dbRntChnlExpediaEstateTB() { id = IdEstate };
    //            dcChnl.Add(currExpedia);
    //            dcChnl.SaveChanges();
    //            currExpedia.isActive = 1;
    //            currExpedia.pidMasterEstate = 0;
    //            currExpedia.Name = currEstate.code;
    //            currExpedia.Type = 0;
    //            currExpedia.ArrivalDays = "YYYYYYY";
    //            currExpedia.MinStay = currEstate.nights_min.objToInt32();
    //            dcChnl.SaveChanges();
    //        }
    //        List<RNT_LN_ESTATE> lstEstate = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == IdEstate).ToList();
    //        if (lstEstate != null && lstEstate.Count > 0)
    //        {
    //            foreach (RNT_LN_ESTATE objEstate in lstEstate)
    //            {
    //                dbRntChnlExpediaEstateLN currExpediaLN = dcChnl.dbRntChnlExpediaEstateLNs.SingleOrDefault(x => x.pidEstate == IdEstate && x.pidLang == objEstate.pid_lang);
    //                if (currExpediaLN == null)
    //                {
    //                    currExpediaLN = new dbRntChnlExpediaEstateLN();
    //                    currExpediaLN.pidEstate = currExpedia.id;
    //                    currExpediaLN.pidLang = objEstate.pid_lang;
    //                    dcChnl.Add(currExpediaLN);
    //                    dcChnl.SaveChanges();
    //                }
    //                currExpediaLN.Description = objEstate.description;
    //                dcChnl.SaveChanges();
    //            }
    //        }
    //        var extrasIds = dcRnt.dbRntEstateExtrasRLs.Where(x => x.pidEstate == IdEstate).ToList();
    //        foreach (var extras in extrasIds)
    //        {
    //            var featureValuesTbl = dcChnl.dbRntChnlExpediaLkFeatureValuesTBLs.SingleOrDefault(x => x.refId == extras.pidEstateExtras + "");
    //            if (featureValuesTbl != null)
    //            {
    //                var currRl = dcChnl.dbRntChnlExpediaEstateFeaturesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.code == featureValuesTbl.code);
    //                if (currRl == null)
    //                {
    //                    currRl = new dbRntChnlExpediaEstateFeaturesRL();
    //                    currRl.pidEstate = IdEstate;
    //                    currRl.code = featureValuesTbl.code;
    //                    if (featureValuesTbl.type == "yesno")
    //                        currRl.value = "1";
    //                    if (featureValuesTbl.type == "numeric")
    //                        currRl.value = "0";// TODO, put distance
    //                    dcChnl.Add(currRl);
    //                    dcChnl.SaveChanges();
    //                }
    //            }
    //        }
    //    }
    //}

    public static int GetUnitsForEstate(RNT_TB_ESTATE currEstate, List<RNT_TBL_RESERVATION> currResList, DateTime dtCurrent, string roomTypeId)
    {
        var units = 1 - currResList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count();

        //get inventory/allotment chnage
        using (DCchnlExpedia dc = new DCchnlExpedia())
        {
            var tmpAllotment = dc.dbRntChnlExpediaEstateAllotmentChangeRLs.SingleOrDefault(x => x.pidEstate == currEstate.id && x.changeDate == dtCurrent && x.RoomTypeId == roomTypeId && x.change >= 0);
            if (tmpAllotment != null)
            {
                if (tmpAllotment.change == 0)
                    units = units + tmpAllotment.Units.objToInt32();
                else if (tmpAllotment.change == 1)
                    units = units - tmpAllotment.Units.objToInt32();
                else if (tmpAllotment.change == 2)
                    units = tmpAllotment.Units.objToInt32();

                if (units < 0)
                    units = 0;
            }
        }
        return units;
    }

}
public static class ChnlExpediaProps
{
    public static string IdAdMedia = "Expedia";
}
public class ChnlExpediaUpdate
{
    public class ExpRatesPerDates
    {
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public string RoomTypeId { get; set; }
        public List<ExpRatesPerDatesAndRatePlan> lstRatePlanPrices { get; set; }
        public ExpRatesPerDates(DateTime dtStart, DateTime dtEnd)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            lstRatePlanPrices = new List<ExpRatesPerDatesAndRatePlan>();
        }
        public ExpRatesPerDates(DateTime dtStart, DateTime dtEnd, ExpRatesPerDates copyFrom)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            lstRatePlanPrices = copyFrom.lstRatePlanPrices;
        }
        public bool HasSamePrices(ExpRatesPerDates compareWith)
        {
            if (DtEnd.AddDays(1) != compareWith.DtStart) return false;
            for (int i = 0; i < lstRatePlanPrices.Count; i++)
            {
                if (!lstRatePlanPrices[i].HasSamePrices(compareWith.lstRatePlanPrices[i])) return false;
            }
            return true;
        }
    }

    public class ExpRatesPerDatesAndRatePlan
    {
        public string RatePlanId { get; set; }
        public bool isClosed { get; set; }
        public int MinStay { get; set; }
        public int MaxStay { get; set; }
        public bool ClosedOnArrival { get; set; }
        public bool ClosedOnDeparture { get; set; }
        public Dictionary<int, decimal> Prices { get; set; }

        public ExpRatesPerDatesAndRatePlan(string ratePlanId)
        {
            RatePlanId = ratePlanId;
            isClosed = false;
            MinStay = 0;
            MaxStay = 0;
            Prices = new Dictionary<int, decimal>();
            ClosedOnArrival = false;
            ClosedOnDeparture = false;
        }
        public ExpRatesPerDatesAndRatePlan(string ratePlanId, ExpRatesPerDatesAndRatePlan copyFrom)
        {
            RatePlanId = ratePlanId;
            isClosed = copyFrom.isClosed;
            MinStay = copyFrom.MinStay;
            MaxStay = copyFrom.MaxStay;
            Prices = copyFrom.Prices;
            ClosedOnArrival = copyFrom.ClosedOnArrival;
            ClosedOnDeparture = copyFrom.ClosedOnDeparture;
        }
        public bool HasSamePrices(ExpRatesPerDatesAndRatePlan compareWith)
        {
            if (RatePlanId != compareWith.RatePlanId) return false;
            if (isClosed != compareWith.isClosed) return false;
            if (MinStay != compareWith.MinStay) return false;
            if (MaxStay != compareWith.MaxStay) return false;
            if (ClosedOnDeparture != compareWith.ClosedOnDeparture) return false;
            if (ClosedOnArrival != compareWith.ClosedOnArrival) return false;
            if (Prices.Count != compareWith.Prices.Count) return false;
            foreach (var item in Prices)
                if (compareWith.Prices[item.Key] == null || compareWith.Prices[item.Key] != item.Value)
                    return false;
            return true;
        }
    }

    private class UpdateRates_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        List<string> RatePlans { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dcRnt.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        return;
                    }
                    var chnlTb = dcChnl.RntChnlExpediaEstateTBL.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || string.IsNullOrEmpty(chnlTb.RoomTypeId))
                    {
                        return;
                    }
                    var roomTypeId = chnlTb.RoomTypeId;
                    var roomTypeTbl = dcChnl.RntChnlExpediaRoomTypeTBL.SingleOrDefault(x => x.id == roomTypeId);
                    var HotelId = roomTypeTbl.HotelId;
                    var hotelTbl = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.HotelId == HotelId && x.isActive == 1);
                    if (hotelTbl == null)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "HotelId: " + HotelId + " NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }

                    //change for special rates/restrictions                    
                    // x.code == "RoomOnly"
                    var ratePlans = new List<RntChnlExpediaRoomTypeRatePlanTBL>();
                    if (RatePlans != null && RatePlans.Count > 0)
                        ratePlans = dcChnl.RntChnlExpediaRoomTypeRatePlanTBL.Where(x => x.RoomTypeId == roomTypeId && RatePlans.Contains(x.RatePlanId)).ToList();
                    else
                    {
                        //Change to send only allowed Rates
                        string rntExpediaRateAcquisitionType = CommonUtilities.getSYS_SETTING("rntExpediaRateNotAllowed");
                        List<string> lstExpediaRateNotAllowed = rntExpediaRateAcquisitionType.splitStringToList(",");
                        ratePlans = dcChnl.RntChnlExpediaRoomTypeRatePlanTBL.Where(x => x.RoomTypeId == roomTypeId && x.status == 1 && !lstExpediaRateNotAllowed.Contains(x.rateAcquisitionType)).ToList();
                        //ratePlans = dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.Where(x => x.RoomTypeId == roomTypeId && x.status == 1 && x.rateAcquisitionType == "SellRate").ToList();
                    }

                    if (ratePlans.Count == 0) return;
                    var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                    if (dtStart < DateTime.Now) dtStart = DateTime.Now.Date;
                    var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;
                    int outError;
                    var priceListPerDates = rntUtils.estate_getPriceListPerDates_Expedia(IdEstate, agentID, dtStart, dtEnd, out outError);
                    if (priceListPerDates == null || priceListPerDates.Count == 0)
                    {
                        ErrorLog.addLog("", "ExpediaUpdate_process", "idEstate:" + IdEstate + " has no prices");
                        return;
                    }
                    var list = new List<ExpRatesPerDates>();
                    foreach (var tmp in priceListPerDates)
                    {
                        DateTime dtCurrent = tmp.DtStart;
                        while (dtCurrent <= tmp.DtEnd)
                        {
                            ExpRatesPerDates objRateDate = new ExpRatesPerDates(dtCurrent, dtCurrent);
                            objRateDate.RoomTypeId = roomTypeId;
                            var lstRateRateplans = new List<ExpRatesPerDatesAndRatePlan>();
                            foreach (var ratePlan in ratePlans)
                            {
                                var datePrices = new ExpRatesPerDatesAndRatePlan(ratePlan.RatePlanId);
                                datePrices.isClosed = false;
                                datePrices.MinStay = tmp.MinStay;
                                if (currEstate.nights_max > 0)
                                {
                                    if (currEstate.nights_max > 28)
                                        datePrices.MaxStay = 28;
                                    else
                                        datePrices.MaxStay = currEstate.nights_max.objToInt32();
                                }
                                else
                                    datePrices.MaxStay = 28;
                                datePrices.ClosedOnArrival = false;
                                datePrices.ClosedOnDeparture = false;

                                //for any restriction change
                                var restrictions = dcChnl.RntChnlExpediaEstateClosedDatesRL.SingleOrDefault(x => x.changeDate == dtCurrent && x.RatePlanId == ratePlan.RatePlanId && x.RoomTypeId == roomTypeId && x.pidEstate == IdEstate);
                                if (restrictions != null)
                                {
                                    if (restrictions.isClosed >= 0)
                                        datePrices.isClosed = restrictions.isClosed == 1 ? true : false;
                                    if (restrictions.minStay > 0)
                                        datePrices.MinStay = restrictions.minStay.objToInt32();
                                    if (restrictions.maxStay > 0)
                                        datePrices.MaxStay = restrictions.maxStay.objToInt32();
                                    if (restrictions.isClosedOnArrival >= 0)
                                        datePrices.ClosedOnArrival = restrictions.isClosedOnArrival == 1 ? true : false;
                                    if (restrictions.isClosedOnDeparture >= 0)
                                        datePrices.ClosedOnDeparture = restrictions.isClosedOnDeparture == 1 ? true : false;
                                }

                                Dictionary<int, decimal> DatePrices = new Dictionary<int, decimal>();
                                foreach (var Price in tmp.Prices)
                                {
                                    decimal currentPrice = Price.Value;
                                    decimal changeAmount = 0;

                                    //get rate plan change
                                    if (ratePlan.rate_changeAmount > 0)
                                    {
                                        changeAmount = (ratePlan.rate_changeIsPercentage == 1) ? (currentPrice * ratePlan.rate_changeAmount.objToInt32() / 100) : ratePlan.rate_changeAmount.objToInt32();
                                        if (ratePlan.rate_changeIsDiscount == 1) { changeAmount = -changeAmount; }
                                    }
                                    decimal ratePrice = currentPrice + changeAmount;

                                    //for any special price chnage
                                    decimal price = getRateChange(ratePlan.RatePlanId, roomTypeId, currentPrice, ratePrice, dtCurrent);
                                    DatePrices.Add(Price.Key, price);
                                }
                                datePrices.Prices = DatePrices;
                                objRateDate.lstRatePlanPrices.Add(datePrices);
                            }
                            var lastDatePrice = list.LastOrDefault();
                            if (lastDatePrice == null || !lastDatePrice.HasSamePrices(objRateDate)) list.Add(objRateDate);
                            else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
                            dtCurrent = dtCurrent.AddDays(1);
                        }
                    }

                    if (hotelTbl.isDemo == 1) requesUrl = "https://simulator.expediaquickconnect.com/connect/ar";
                    else requesUrl = "https://services.expediapartnercentral.com/eqc/ar";
                    //else requesUrl = "https://ws.expediaquickconnect.com/connect/ar";
                    var Request = new ChnlExpediaClasses.AvailRateUpdateRQ(hotelTbl);

                    foreach (ExpRatesPerDates tmp in list)
                    {
                        var AvailRateUpdate = new ChnlExpediaClasses.AvailRateUpdateRQ.AvailRateUpdate();
                        Request.AvailRateUpdates.Add(AvailRateUpdate);
                        AvailRateUpdate.DateRange._from.ValueDate = tmp.DtStart;
                        AvailRateUpdate.DateRange._to.ValueDate = tmp.DtEnd;
                        var RoomType = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType();
                        AvailRateUpdate.RoomTypes.Add(RoomType);
                        RoomType._id = roomTypeId;
                        foreach (var ratePlan in tmp.lstRatePlanPrices)
                        {
                            var RatePlan = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType.RatePlan();
                            RoomType.RatePlans.Add(RatePlan);
                            RatePlan._id = ratePlan.RatePlanId;
                            RatePlan._closed = ratePlan.isClosed;
                            var Rate = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType.RatePlan.Rate();
                            RatePlan.Rates.Add(Rate);

                            foreach (var Price in ratePlan.Prices)
                            {
                                var PerOccupancy = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType.RatePlan.Rate.PerOccupancy();
                                Rate.perOccupancy.Add(PerOccupancy);
                                PerOccupancy._occupancy = Price.Key;
                                PerOccupancy._rate.ValueDecimal = Price.Value;
                            }

                            var restriction = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType.RatePlan.Restrictions();
                            RatePlan.Restriction = restriction;
                            restriction._minLOS = ratePlan.MinStay;
                            restriction._closedToArrival = ratePlan.ClosedOnArrival;
                            restriction._closedToDeparture = ratePlan.ClosedOnDeparture;
                            restriction._maxLOS = ratePlan.MaxStay;
                        }
                    }

                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequest(requesUrl, Request.GetXml(), out tmpErrorString);
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                        return;
                    var Response = new ChnlExpediaClasses.AvailRateUpdateRS(responseData);
                    if (Response.success != null) return;
                    foreach (var error in Response.Errors)
                        ErrorString += "\n\r" + error.description;

                    if (ErrorString != "")
                        ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateRates_process Success IdEstate:" + IdEstate, "", Request.GetXml(), "");
                    //ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateRates_process IdEstate:" + IdEstate, "", ErrorString, "");
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateRates_process IdEstate:" + IdEstate, "", ex.ToString(), "");
            }
        }
        public UpdateRates_process(int idEstate, List<string> ratePlans)
        {
            IdEstate = idEstate;
            RatePlans = ratePlans;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaUpdate.UpdateRates_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            Thread.Sleep(2000);//give it a second or two to start

        }
        public UpdateRates_process(int idEstate, DateTime dtStart, DateTime dtEnd, List<string> ratePlans)
        {
            IdEstate = idEstate;
            RatePlans = ratePlans;
            DtStart = dtStart.Date;
            DtEnd = dtEnd.Date;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaUpdate.UpdateRates_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            Thread.Sleep(2000);//give it a second or two to start
        }
        List<RntChnlExpediaEstateRateChangesRL> rateChangesList;
        decimal getRateChange(string ratePlanId, string roomId, decimal originalPrice, decimal ratePrice, DateTime changeDate)
        {
            if (string.IsNullOrEmpty(ratePlanId)) return 0;
            if (string.IsNullOrEmpty(roomId)) return 0;

            using (magaChnlExpediaDataContext dc = maga_DataContext.DC_EXP)
                rateChangesList = dc.RntChnlExpediaEstateRateChangesRL.Where(x => x.pidEstate == IdEstate && x.rate_changeIsDiscount >= 0 && x.RatePlanId == ratePlanId && x.RoomTypeId == roomId).ToList();

            var tmpRate = rateChangesList.SingleOrDefault(x => x.changeDate == changeDate);
            if (tmpRate == null) return ratePrice;
            var currPrice = 0 + originalPrice;
            if (tmpRate.rate_changeAmount.objToInt32() > 0)
            {
                var changeAmount = (tmpRate.rate_changeIsPercentage == 1) ? (currPrice * tmpRate.rate_changeAmount.objToInt32() / 100) : tmpRate.rate_changeAmount.objToInt32();
                if (tmpRate.rate_changeIsDiscount == 1) { changeAmount = -changeAmount; }
                currPrice = currPrice + changeAmount;
            }
            return Math.Round(currPrice, 2);
        }
    }
    public static void UpdateRates_start(int idEstate, List<string> ratePlans)
    {
        UpdateRates_process _tmp = new UpdateRates_process(idEstate, ratePlans);
    }
    public static void UpdateRates_start(int idEstate, DateTime dtStart, DateTime dtEnd, List<string> ratePlans)
    {
        UpdateRates_process _tmp = new UpdateRates_process(idEstate, dtStart, dtEnd, ratePlans);
    }

    #region update split dates
    private class UpdateSplitRates_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        List<string> RatePlans { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dcRnt.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        return;
                    }
                    var chnlTb = dcChnl.RntChnlExpediaEstateTBL.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || string.IsNullOrEmpty(chnlTb.RoomTypeId))
                    {
                        return;
                    }
                    var roomTypeId = chnlTb.RoomTypeId;
                    var roomTypeTbl = dcChnl.RntChnlExpediaRoomTypeTBL.SingleOrDefault(x => x.id == roomTypeId);
                    var HotelId = roomTypeTbl.HotelId;
                    var hotelTbl = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.HotelId == HotelId && x.isActive == 1);
                    if (hotelTbl == null)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "HotelId: " + HotelId + " NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }

                    //change for special rates/restrictions                    
                    // x.code == "RoomOnly"
                    var ratePlans = new List<RntChnlExpediaRoomTypeRatePlanTBL>();
                    if (RatePlans != null && RatePlans.Count > 0)
                        ratePlans = dcChnl.RntChnlExpediaRoomTypeRatePlanTBL.Where(x => x.RoomTypeId == roomTypeId && RatePlans.Contains(x.RatePlanId)).ToList();
                    else
                    {
                        //Change to send only allowed Rates
                        string rntExpediaRateAcquisitionType = CommonUtilities.getSYS_SETTING("rntExpediaRateNotAllowed");
                        List<string> lstExpediaRateNotAllowed = rntExpediaRateAcquisitionType.splitStringToList(",");
                        ratePlans = dcChnl.RntChnlExpediaRoomTypeRatePlanTBL.Where(x => x.RoomTypeId == roomTypeId && x.status == 1 && !lstExpediaRateNotAllowed.Contains(x.rateAcquisitionType)).ToList();
                        //ratePlans = dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.Where(x => x.RoomTypeId == roomTypeId && x.status == 1 && x.rateAcquisitionType == "SellRate").ToList();
                    }

                    if (ratePlans.Count == 0) return;
                    var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                    if (dtStart < DateTime.Now) dtStart = DateTime.Now.Date;
                    var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;

                    #region for splitting 2 years dates in number of date ranges
                    var splitParam = CommonUtilities.getSYS_SETTING("rntExpediaDateSplitParam").objToInt32();
                    if (splitParam == 0) splitParam = 6;

                    var dateCnt = Math.Ceiling(Decimal.Divide(24, splitParam));

                    List<Dates> lstDates = new List<Dates>();
                    for (int i = 1; i <= dateCnt; i++)
                    {
                        var objDate = new Dates();
                        if (i == 1)
                            objDate.dtStart = DateTime.Now.Date;
                        else
                        {
                            var lastDtEnd = lstDates.OrderByDescending(x => x.dtStart).FirstOrDefault();
                            if (lastDtEnd != null)
                                objDate.dtStart = lastDtEnd.dtEnd;
                        }
                        objDate.dtEnd = objDate.dtStart.AddMonths(splitParam);
                        if (objDate.dtEnd > DateTime.Now.Date.AddMonths(24))
                            objDate.dtEnd = DateTime.Now.Date.AddMonths(24);
                        lstDates.Add(objDate);

                    }
                    #endregion

                    foreach (var objDate in lstDates)
                    {
                        int outError;
                        var priceListPerDates = rntUtils.estate_getPriceListPerDates_Expedia(IdEstate, agentID, objDate.dtStart, objDate.dtEnd, out outError);
                        if (priceListPerDates == null || priceListPerDates.Count == 0)
                        {
                            ErrorLog.addLog("", "ExpediaUpdate_process", "idEstate:" + IdEstate + " has no prices");
                            continue;
                        }
                        var list = new List<ExpRatesPerDates>();
                        foreach (var tmp in priceListPerDates)
                        {
                            DateTime dtCurrent = tmp.DtStart;
                            while (dtCurrent <= tmp.DtEnd)
                            {
                                ExpRatesPerDates objRateDate = new ExpRatesPerDates(dtCurrent, dtCurrent);
                                objRateDate.RoomTypeId = roomTypeId;
                                var lstRateRateplans = new List<ExpRatesPerDatesAndRatePlan>();
                                foreach (var ratePlan in ratePlans)
                                {
                                    var datePrices = new ExpRatesPerDatesAndRatePlan(ratePlan.RatePlanId);
                                    datePrices.isClosed = false;
                                    datePrices.MinStay = tmp.MinStay;
                                    if (currEstate.nights_max > 0)
                                    {
                                        if (currEstate.nights_max > 28)
                                            datePrices.MaxStay = 28;
                                        else
                                            datePrices.MaxStay = currEstate.nights_max.objToInt32();
                                    }
                                    else
                                        datePrices.MaxStay = 28;
                                    datePrices.ClosedOnArrival = false;
                                    datePrices.ClosedOnDeparture = false;

                                    //for any restriction change
                                    var restrictions = dcChnl.RntChnlExpediaEstateClosedDatesRL.SingleOrDefault(x => x.changeDate == dtCurrent && x.RatePlanId == ratePlan.RatePlanId && x.RoomTypeId == roomTypeId && x.pidEstate == IdEstate);
                                    if (restrictions != null)
                                    {
                                        if (restrictions.isClosed >= 0)
                                            datePrices.isClosed = restrictions.isClosed == 1 ? true : false;
                                        if (restrictions.minStay > 0)
                                            datePrices.MinStay = restrictions.minStay.objToInt32();
                                        if (restrictions.maxStay > 0)
                                            datePrices.MaxStay = restrictions.maxStay.objToInt32();
                                        if (restrictions.isClosedOnArrival >= 0)
                                            datePrices.ClosedOnArrival = restrictions.isClosedOnArrival == 1 ? true : false;
                                        if (restrictions.isClosedOnDeparture >= 0)
                                            datePrices.ClosedOnDeparture = restrictions.isClosedOnDeparture == 1 ? true : false;
                                    }

                                    Dictionary<int, decimal> DatePrices = new Dictionary<int, decimal>();
                                    foreach (var Price in tmp.Prices)
                                    {
                                        decimal currentPrice = Price.Value;
                                        decimal changeAmount = 0;

                                        //get rate plan change
                                        if (ratePlan.rate_changeAmount > 0)
                                        {
                                            changeAmount = (ratePlan.rate_changeIsPercentage == 1) ? (currentPrice * ratePlan.rate_changeAmount.objToInt32() / 100) : ratePlan.rate_changeAmount.objToInt32();
                                            if (ratePlan.rate_changeIsDiscount == 1) { changeAmount = -changeAmount; }
                                        }
                                        decimal ratePrice = currentPrice + changeAmount;

                                        //for any special price chnage
                                        decimal price = getRateChange(ratePlan.RatePlanId, roomTypeId, currentPrice, ratePrice, dtCurrent);
                                        DatePrices.Add(Price.Key, price);
                                    }
                                    datePrices.Prices = DatePrices;
                                    objRateDate.lstRatePlanPrices.Add(datePrices);
                                }
                                var lastDatePrice = list.LastOrDefault();
                                if (lastDatePrice == null || !lastDatePrice.HasSamePrices(objRateDate)) list.Add(objRateDate);
                                else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
                                dtCurrent = dtCurrent.AddDays(1);
                            }
                        }

                        if (hotelTbl.isDemo == 1) requesUrl = "https://simulator.expediaquickconnect.com/connect/ar";
                        else requesUrl = "https://services.expediapartnercentral.com/eqc/ar";
                        //else requesUrl = "https://ws.expediaquickconnect.com/connect/ar";
                        var Request = new ChnlExpediaClasses.AvailRateUpdateRQ(hotelTbl);

                        foreach (ExpRatesPerDates tmp in list)
                        {
                            var AvailRateUpdate = new ChnlExpediaClasses.AvailRateUpdateRQ.AvailRateUpdate();
                            Request.AvailRateUpdates.Add(AvailRateUpdate);
                            AvailRateUpdate.DateRange._from.ValueDate = tmp.DtStart;
                            AvailRateUpdate.DateRange._to.ValueDate = tmp.DtEnd;
                            var RoomType = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType();
                            AvailRateUpdate.RoomTypes.Add(RoomType);
                            RoomType._id = roomTypeId;
                            foreach (var ratePlan in tmp.lstRatePlanPrices)
                            {
                                var RatePlan = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType.RatePlan();
                                RoomType.RatePlans.Add(RatePlan);
                                RatePlan._id = ratePlan.RatePlanId;
                                RatePlan._closed = ratePlan.isClosed;
                                var Rate = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType.RatePlan.Rate();
                                RatePlan.Rates.Add(Rate);

                                foreach (var Price in ratePlan.Prices)
                                {
                                    var PerOccupancy = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType.RatePlan.Rate.PerOccupancy();
                                    Rate.perOccupancy.Add(PerOccupancy);
                                    PerOccupancy._occupancy = Price.Key;
                                    PerOccupancy._rate.ValueDecimal = Price.Value;
                                }

                                var restriction = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType.RatePlan.Restrictions();
                                RatePlan.Restriction = restriction;
                                restriction._minLOS = ratePlan.MinStay;
                                restriction._closedToArrival = ratePlan.ClosedOnArrival;
                                restriction._closedToDeparture = ratePlan.ClosedOnDeparture;
                                restriction._maxLOS = ratePlan.MaxStay;
                            }
                        }

                        string tmpErrorString = "";
                        var responseData = ChnlExpediaUtils.SendRequest(requesUrl, Request.GetXml(), out tmpErrorString);
                        ErrorString = tmpErrorString;
                        if (responseData == "")
                            continue;
                        var Response = new ChnlExpediaClasses.AvailRateUpdateRS(responseData);
                        if (Response.success != null)
                        {
                            ChnlExpediaUtils.addLog("SUCCESS", "ChnlExpediaUpdate.UpdateSplitRates_process IdEstate:" + IdEstate, "", "", "");
                        }
                        else
                        {
                            foreach (var error in Response.Errors)
                                ErrorString += "\n\r" + error.description;

                            if (ErrorString != "")
                                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateSplitRates_process IdEstate:" + IdEstate, "", ErrorString, "");
                        }
                    }
                    //ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateRates_process IdEstate:" + IdEstate, "", ErrorString, "");
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateSplitRates_process IdEstate:" + IdEstate, "", ex.ToString(), "");
            }
        }
        public UpdateSplitRates_process(int idEstate, List<string> ratePlans)
        {
            IdEstate = idEstate;
            RatePlans = ratePlans;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaUpdate.UpdateRates_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            Thread.Sleep(2000);//give it a second or two to start

        }
        public UpdateSplitRates_process(int idEstate, DateTime dtStart, DateTime dtEnd, List<string> ratePlans)
        {
            IdEstate = idEstate;
            RatePlans = ratePlans;
            DtStart = dtStart.Date;
            DtEnd = dtEnd.Date;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaUpdate.UpdateRates_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            Thread.Sleep(2000);//give it a second or two to start
        }
        List<RntChnlExpediaEstateRateChangesRL> rateChangesList;
        decimal getRateChange(string ratePlanId, string roomId, decimal originalPrice, decimal ratePrice, DateTime changeDate)
        {
            if (string.IsNullOrEmpty(ratePlanId)) return 0;
            if (string.IsNullOrEmpty(roomId)) return 0;

            using (magaChnlExpediaDataContext dc = maga_DataContext.DC_EXP)
                rateChangesList = dc.RntChnlExpediaEstateRateChangesRL.Where(x => x.pidEstate == IdEstate && x.rate_changeIsDiscount >= 0 && x.RatePlanId == ratePlanId && x.RoomTypeId == roomId).ToList();

            var tmpRate = rateChangesList.SingleOrDefault(x => x.changeDate == changeDate);
            if (tmpRate == null) return ratePrice;
            var currPrice = 0 + originalPrice;
            if (tmpRate.rate_changeAmount.objToInt32() > 0)
            {
                var changeAmount = (tmpRate.rate_changeIsPercentage == 1) ? (currPrice * tmpRate.rate_changeAmount.objToInt32() / 100) : tmpRate.rate_changeAmount.objToInt32();
                if (tmpRate.rate_changeIsDiscount == 1) { changeAmount = -changeAmount; }
                currPrice = currPrice + changeAmount;
            }
            return Math.Round(currPrice, 2);
        }
    }
    public static void UpdateSplitRates_start(int idEstate, List<string> ratePlans)
    {
        UpdateSplitRates_process _tmp = new UpdateSplitRates_process(idEstate, ratePlans);
    }
    public static void UpdateSplitRates_start(int idEstate, DateTime dtStart, DateTime dtEnd, List<string> ratePlans)
    {
        UpdateSplitRates_process _tmp = new UpdateSplitRates_process(idEstate, dtStart, dtEnd, ratePlans);
    }
    #endregion


    public class Dates
    {
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }

        public Dates()
        {
            dtStart = DateTime.Now.Date;
            dtEnd = DateTime.Now.Date;
        }
    }
    public static void UpdateRatesWithSplitDates(int idEstate, List<string> ratePlans)
    {
        //for splitting 2 years dates in number of date ranges
        var splitParam = CommonUtilities.getSYS_SETTING("rntExpediaDateSplitParam").objToInt32();
        if (splitParam == 0) splitParam = 6;

        var dateCnt = Math.Ceiling(Decimal.Divide(24, splitParam));

        List<Dates> lstDates = new List<Dates>();
        for (int i = 1; i <= dateCnt; i++)
        {
            var objDate = new Dates();
            if (i == 1)
                objDate.dtStart = DateTime.Now.Date;
            else
            {
                var lastDtEnd = lstDates.OrderByDescending(x => x.dtStart).FirstOrDefault();
                if (lastDtEnd != null)
                    objDate.dtStart = lastDtEnd.dtEnd;
            }
            objDate.dtEnd = objDate.dtStart.AddMonths(splitParam);
            if (objDate.dtEnd > DateTime.Now.Date.AddMonths(24))
                objDate.dtEnd = DateTime.Now.Date.AddMonths(24);
            lstDates.Add(objDate);
        }

        foreach (Dates objDate in lstDates)
        {
            ChnlExpediaUpdate.UpdateRates_start(idEstate, objDate.dtStart, objDate.dtEnd, new List<string>());
        }
    }

    #region update availabity of all rooms of hotels together
    private class UpdateHotelAvailability_process
    {
        public int HotelId { get; set; }
        List<int> EstateIds { get; set; }
        //public int IdEstate { get; set; }
        public long agentID { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dcRnt.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        return;
                    }

                    var roomList = dcChnl.RntChnlExpediaEstateTBL.Where(x => x.HotelId == HotelId && EstateIds.Contains(x.id)).ToList();
                    foreach (var room in roomList)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(room.RoomTypeId))
                                continue;

                            //var chnlTb = dcChnl.dbRntChnlExpediaEstateTBLs.SingleOrDefault(x => x.id == IdEstate);
                            //if (chnlTb == null || string.IsNullOrEmpty(chnlTb.RoomTypeId))
                            //{
                            //    return;
                            //}

                            var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == room.id);
                            if (currEstate == null)
                            {
                                continue;
                            }

                            var roomTypeId = room.RoomTypeId;
                            var roomTypeTbl = dcChnl.RntChnlExpediaRoomTypeTBL.SingleOrDefault(x => x.id == roomTypeId);
                            var HotelId = roomTypeTbl.HotelId;
                            var hotelTbl = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.HotelId == HotelId && x.isActive == 1);
                            if (hotelTbl == null)
                            {
                                ChnlExpediaUtils.addLog("ERROR", "HotelId: " + HotelId + " NOT FOUND OR NOT ACTIVE", "", "", "");
                                continue;
                            }
                            if (hotelTbl.isDemo == 1) requesUrl = "https://simulator.expediaquickconnect.com/connect/ar";
                            else requesUrl = "https://services.expediapartnercentral.com/eqc/ar";
                            //else requesUrl = "https://ws.expediaquickconnect.com/connect/ar";
                            var Request = new ChnlExpediaClasses.AvailRateUpdateRQ(hotelTbl);
                            var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                            var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;
                            var resList = rntUtils.rntEstate_resList(room.id, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                            var list = new List<rntExts.AvvListPerDates>();
                            DateTime dtCurrent = dtStart;
                            while (dtCurrent < dtEnd)
                            {
                                var units = 1 - resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count();
                                bool isAvv = units > 0;
                                var lastDatePrice = list.LastOrDefault();
                                if (lastDatePrice == null || lastDatePrice.IsAvv != isAvv || lastDatePrice.Units != units) list.Add(new rntExts.AvvListPerDates(dtCurrent, dtCurrent, isAvv, units));
                                else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
                                dtCurrent = dtCurrent.AddDays(1);
                            }
                            foreach (var tmp in list)
                            {
                                var AvailRateUpdate = new ChnlExpediaClasses.AvailRateUpdateRQ.AvailRateUpdate();
                                Request.AvailRateUpdates.Add(AvailRateUpdate);
                                AvailRateUpdate.DateRange._from.ValueDate = tmp.DtStart;
                                AvailRateUpdate.DateRange._to.ValueDate = tmp.DtEnd;
                                var RoomType = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType();
                                AvailRateUpdate.RoomTypes.Add(RoomType);
                                RoomType._id = roomTypeId;
                                RoomType.inventory._totalInventoryAvailable = (tmp.IsAvv ? 1 : 0);
                            }
                            string tmpErrorString = "";
                            var responseData = ChnlExpediaUtils.SendRequest(requesUrl, Request.GetXml(), out tmpErrorString);
                            ErrorString = tmpErrorString;
                            if (responseData == "")
                                return;
                            var Response = new ChnlExpediaClasses.AvailRateUpdateRS(responseData);
                            if (Response.success != null) return;
                            foreach (var error in Response.Errors)
                                ErrorString += "\n`111\r" + error.description;

                            if (ErrorString != "")
                                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateAvailability_process IdEstate:" + room.id, "", ErrorString, "");
                            else
                                ChnlExpediaUtils.addLog("SUCCESS", "ChnlExpediaUpdate.UpdateAvailability_process IdEstate:" + room.id, "", "", "");
                        }
                        catch (Exception ex)
                        {
                            ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateAvailability_process success IdEstate:" + room.id, "", ex.ToString(), "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateAvailability_process hotelId:" + HotelId, "", ex.ToString(), "");
            }
        }

        public UpdateHotelAvailability_process(int hotelId, List<int> estateIds)
        {
            HotelId = hotelId;
            EstateIds = estateIds;
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskSchedulerPriority.AddTask(action, "ChnlExpediaUpdate.UpdateAvailability_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Highest;
            t.Start();
            Thread.Sleep(1000);//give it a second or two to start
        }
        public UpdateHotelAvailability_process(int hotelId, List<int> estateIds, DateTime dtStart, DateTime dtEnd)
        {
            HotelId = hotelId;
            DtStart = dtStart.Date;
            DtEnd = dtEnd.Date;
            EstateIds = estateIds;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaUpdate.UpdateAvailability_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Highest;
            t.Start();
            Thread.Sleep(1000);//give it a second or two to start
        }
    }
    public static void UpdateHotelAvailability_start(int hotelId, List<int> estateIds)
    {
        UpdateHotelAvailability_process _tmp = new UpdateHotelAvailability_process(hotelId, estateIds);
    }
    public static void UpdateHotelAvailability_start(int hotelId, List<int> estateIds, DateTime dtStart, DateTime dtEnd)
    {
        UpdateHotelAvailability_process _tmp = new UpdateHotelAvailability_process(hotelId, estateIds, dtStart, dtEnd);
    }
    #endregion

    private class UpdateAvailability_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dcRnt.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        return;
                    }
                    var chnlTb = dcChnl.RntChnlExpediaEstateTBL.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || string.IsNullOrEmpty(chnlTb.RoomTypeId))
                    {
                        return;
                    }
                    var roomTypeId = chnlTb.RoomTypeId;
                    var roomTypeTbl = dcChnl.RntChnlExpediaRoomTypeTBL.SingleOrDefault(x => x.id == roomTypeId);
                    var HotelId = roomTypeTbl.HotelId;
                    var hotelTbl = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.HotelId == HotelId && x.isActive == 1);
                    if (hotelTbl == null)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "HotelId: " + HotelId + " NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    if (hotelTbl.isDemo == 1) requesUrl = "https://simulator.expediaquickconnect.com/connect/ar";
                    else requesUrl = "https://services.expediapartnercentral.com/eqc/ar";
                    //else requesUrl = "https://ws.expediaquickconnect.com/connect/ar";
                    var Request = new ChnlExpediaClasses.AvailRateUpdateRQ(hotelTbl);
                    var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                    var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;
                    var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                    var list = new List<rntExts.AvvListPerDates>();
                    DateTime dtCurrent = dtStart;
                    while (dtCurrent < dtEnd)
                    {
                        var units = 1 - resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count();
                        bool isAvv = units > 0;
                        var lastDatePrice = list.LastOrDefault();
                        if (lastDatePrice == null || lastDatePrice.IsAvv != isAvv || lastDatePrice.Units != units) list.Add(new rntExts.AvvListPerDates(dtCurrent, dtCurrent, isAvv, units));
                        else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
                        dtCurrent = dtCurrent.AddDays(1);
                    }
                    foreach (var tmp in list)
                    {
                        var AvailRateUpdate = new ChnlExpediaClasses.AvailRateUpdateRQ.AvailRateUpdate();
                        Request.AvailRateUpdates.Add(AvailRateUpdate);
                        AvailRateUpdate.DateRange._from.ValueDate = tmp.DtStart;
                        AvailRateUpdate.DateRange._to.ValueDate = tmp.DtEnd;
                        var RoomType = new ChnlExpediaClasses.AvailRateUpdateRQ.RoomType();
                        AvailRateUpdate.RoomTypes.Add(RoomType);
                        RoomType._id = roomTypeId;
                        RoomType.inventory._totalInventoryAvailable = (tmp.IsAvv ? 1 : 0);
                    }
                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequest(requesUrl, Request.GetXml(), out tmpErrorString);
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                        return;
                    var Response = new ChnlExpediaClasses.AvailRateUpdateRS(responseData);
                    if (Response.success != null)
                    {
                        ChnlExpediaUtils.addLog("SUCCESS", "ChnlExpediaUpdate.UpdateAvailability_process success IdEstate:" + IdEstate, "", "", "");
                        //return;
                    }
                    else
                    {
                        foreach (var error in Response.Errors)
                            ErrorString += "\n\r" + error.description;

                        if (ErrorString != "")
                            ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateAvailability_process success IdEstate:" + IdEstate, "", ErrorString, "");
                    }
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateAvailability_process IdEstate:" + IdEstate, "", ex.ToString(), "");
            }
        }

        public UpdateAvailability_process(int idEstate)
        {
            IdEstate = idEstate;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskSchedulerPriority.AddTask(action, "ChnlExpediaUpdate.UpdateAvailability_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Highest;
            t.Start();
            //Thread.Sleep(1000);//give it a second or two to start
        }
        public UpdateAvailability_process(int idEstate, DateTime dtStart, DateTime dtEnd)
        {
            IdEstate = idEstate;
            DtStart = dtStart.Date;
            DtEnd = dtEnd.Date;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaUpdate.UpdateAvailability_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            Thread.Sleep(1000);//give it a second or two to start
        }
    }
    public static void UpdateAvailability_start(int idEstate)
    {
        UpdateAvailability_process _tmp = new UpdateAvailability_process(idEstate);
    }
    public static void UpdateAvailability_start(int idEstate, DateTime dtStart, DateTime dtEnd)
    {
        UpdateAvailability_process _tmp = new UpdateAvailability_process(idEstate, dtStart, dtEnd);
    }

    private class BookingConfirm_process
    {
        public long ReservationId { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dcRnt.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        return;
                    }
                    var chnlBooking = dcChnl.RntChnlExpediaBookingTBL.SingleOrDefault(x => x.reservationId == ReservationId);
                    if (chnlBooking == null)
                    {
                        return;
                    }
                    var HotelId = chnlBooking.HotelId;
                    var hotelTbl = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.HotelId == HotelId && x.isActive == 1);
                    if (hotelTbl == null)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "HotelId: " + HotelId + " NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    if (hotelTbl.isDemo == 1) requesUrl = "https://simulator.expediaquickconnect.com/connect/bc";
                    else requesUrl = "https://services.expediapartnercentral.com/eqc/bc";
                    //else requesUrl = "https://ws.expediaquickconnect.com/connect/bc";
                    var Request = new ChnlExpediaClasses.BookingConfirmRQ(hotelTbl);
                    var bookingConfirmNumber = new ChnlExpediaClasses.BookingConfirmRQ.BookingConfirmNumber();
                    bookingConfirmNumber._bookingID = chnlBooking.id;
                    bookingConfirmNumber._bookingType = chnlBooking.type;
                    bookingConfirmNumber._confirmNumber = "" + ReservationId;

                    Request.BookingConfirmNumbers.Add(bookingConfirmNumber);
                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequest(requesUrl, Request.GetXml(), out tmpErrorString);
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                        return;
                    var Response = new ChnlExpediaClasses.BookingConfirmRS(responseData);
                    if (Response.success != null) return;
                    foreach (var error in Response.Errors)
                        ErrorString += "\n\r" + error.description;

                    ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.BookingConfirm_process confirm xml ReservationId:" + ReservationId, "", Request.GetXml(), "");
                    ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.BookingConfirm_process ReservationId:" + ReservationId, "", ErrorString, "");
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.BookingConfirm_process ReservationId:" + ReservationId, "", ex.ToString(), "");
            }
        }

        public BookingConfirm_process(long reservationId)
        {
            ReservationId = reservationId;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaUpdate.BookingConfirm_process ReservationId:" + ReservationId);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }
    }
    public static void BookingConfirm_start(long reservationId)
    {
        BookingConfirm_process _tmp = new BookingConfirm_process(reservationId);
    }
    private class PropertyNew_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    //ErrorLog.addLog("", "prop new", "s");
                    var properties = new List<ChnlExpediaClasses.PropertyNewRequest>();
                    var property = new ChnlExpediaClasses.PropertyNewRequest();

                    if (currEstate.id == 2212)
                        property.providerPropertyId = currEstate.id + "";
                    else
                        property.providerPropertyId = App.ProjectName + "_" + currEstate.id + "";

                    property.name = currEstate.code;
                    if (!string.IsNullOrEmpty(currEstate.google_maps) && currEstate.google_maps.Split('|').Length > 1)
                    {
                        property.latitude = currEstate.google_maps.Split('|')[0].Replace(",", ".");
                        property.longitude = currEstate.google_maps.Split('|')[1].Replace(",", ".");
                    }

                    #region propertyType
                    if (currEstate.category == "villa")
                        property.structureType = "VILLA";
                    else if (currEstate.category == "apt")
                        property.structureType = "APARTMENT";

                    //using (DCchnlExpedia dcChnlExpedia = new DCchnlExpedia())
                    //using (DCmodRental dcRnt = new DCmodRental())
                    //{
                    //    var propertyCategory = dcChnlExpedia.dbRntChnlExpediaPropertyTypeRLs.SingleOrDefault(x => x.pidCategory == currEstate.pid_category);
                    //    if (propertyCategory != null)
                    //    {
                    //        var category = dcChnlExpedia.dbRntChnlExpediaLkPropertyTypeTBLs.SingleOrDefault(x => x.code == propertyCategory.pidExpediaCategory);
                    //        if (category != null)
                    //        {
                    //            property.structureType = category.code;
                    //        }
                    //    }
                    //}
                    #endregion

                    property.currencyCode = property.billingCurrencyCode = "EUR";
                    property.timeZone = "Europe/Rome";

                    var addresses = new List<ChnlExpediaClasses.address>();
                    var address = new ChnlExpediaClasses.address();
                    address.line1 = currEstate.loc_address;
                    address.city = CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), 2, "");
                    address.postalCode = currEstate.loc_zip_code;

                    magaLocation_DataContext DC_LOCATION = maga_DataContext.DC_LOCATION;
                    var city = DC_LOCATION.LOC_TB_CITies.SingleOrDefault(x => x.id == currEstate.pid_city.objToInt32());
                    if (city != null)
                    {
                        var country = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.SingleOrDefault(x => x.id == city.pid_country);
                        if (country != null)
                            address.countryCode = country.inner_notes;
                    }
                    addresses.Add(address);
                    property.addresses = addresses;

                    #region contact
                    var contact = new ChnlExpediaClasses.contacts();

                    #region property
                    var propertyContact = new ChnlExpediaClasses.Property();
                    string fname = CommonUtilities.getSYS_SETTING("contact_first_name");
                    string lname = CommonUtilities.getSYS_SETTING("contact_last_name");

                    propertyContact.firstName = fname;
                    propertyContact.lastName = lname;

                    string email = CommonUtilities.getSYS_SETTING("expedia_content_email") + "";
                    var emails = new List<string>();
                    emails.Add(email);
                    propertyContact.emails = emails;

                    var phoneNumbers = new List<ChnlExpediaClasses.phoneNumber>();

                    var phoneNumber = new ChnlExpediaClasses.phoneNumber();
                    phoneNumber.number = CommonUtilities.getSYS_SETTING("expedia_property_phone") + "";
                    phoneNumbers.Add(phoneNumber);
                    propertyContact.phoneNumbers = phoneNumbers;
                    contact.Property = propertyContact;
                    #endregion

                    #region reservation Manager

                    var reservationManager = new ChnlExpediaClasses.ReservationManager();
                    reservationManager.firstName = fname;
                    reservationManager.lastName = lname;
                    reservationManager.emails = emails;

                    var resManagerphoneNumbers = new List<ChnlExpediaClasses.phoneNumber>();
                    var resManagerPhoneNumber = new ChnlExpediaClasses.phoneNumber();
                    resManagerPhoneNumber.number = CommonUtilities.getSYS_SETTING("expedia_res_manager_phone") + "";
                    resManagerphoneNumbers.Add(resManagerPhoneNumber);

                    reservationManager.phoneNumbers = resManagerphoneNumbers;
                    contact.ReservationManager = reservationManager;
                    #endregion

                    #region alternate reservation manager
                    var alternateReservationManager = new ChnlExpediaClasses.AlternateReservationManager();
                    alternateReservationManager.firstName = fname;
                    alternateReservationManager.lastName = lname;
                    alternateReservationManager.emails = emails;

                    var alternateResManagerphoneNumbers = new List<ChnlExpediaClasses.phoneNumber>();
                    var alternateResManagerPhoneNumber = new ChnlExpediaClasses.phoneNumber();
                    alternateResManagerPhoneNumber.number = CommonUtilities.getSYS_SETTING("expedia_alternate_res_manager_phone") + "";
                    alternateResManagerphoneNumbers.Add(alternateResManagerPhoneNumber);

                    alternateReservationManager.phoneNumbers = alternateResManagerphoneNumbers;

                    contact.AlternateReservationManager = alternateReservationManager;
                    #endregion

                    property.contacts = contact;
                    #endregion

                    #region content

                    using (DCmodContent dcContent = new DCmodContent())
                    {
                        var contents = new List<ChnlExpediaClasses.content>();
                        var langs = maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.Where(x => x.is_active == 1).ToList();
                        foreach (var lang in langs)
                        {
                            var currEstateLang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == currEstate.id && x.pid_lang == lang.id);
                            if (lang.common_name != "" && currEstateLang != null && currEstateLang.title != "")
                            {
                                var content = new ChnlExpediaClasses.content();
                                //content.locale = "en-US";
                                content.locale = lang.common_name;
                                if (content.locale == "en-GB")
                                    content.locale = "en-US";

                                content.name = currEstateLang.title;

                                #region images
                                var lstImages = new List<ChnlExpediaClasses.image>();
                                var images = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == currEstate.id && x.type == "original").OrderBy(x => x.sequence).ToList();
                                foreach (var image in images)
                                {
                                    var Image = new ChnlExpediaClasses.image();
                                    Image.url = App.HOST_SSL + "/" + image.img_banner;
                                    if (image.sequence == 1)
                                        Image.categoryCode = "FEATURED_IMAGE";
                                    lstImages.Add(Image);
                                }
                                content.images = lstImages;
                                #endregion

                                #region amenities
                                using (DCchnlExpedia dcChnlExpedia = new DCchnlExpedia())
                                using (DCmodRental dcRnt = new DCmodRental())
                                {
                                    var extrasIds = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.pid_config > 0).Select(x => x.pid_config).ToList();
                                    var expediaAmenties = dcChnlExpedia.dbRntChnlExpediaLkAmenitiesTBLs.Where(x => extrasIds.Contains(x.refId.Value)).ToList();

                                    List<ChnlExpediaClasses.amenity> amenities = new List<ChnlExpediaClasses.amenity>();
                                    if (expediaAmenties != null && expediaAmenties.Count > 0)
                                    {
                                        foreach (var objAmenity in expediaAmenties)
                                        {
                                            ChnlExpediaClasses.amenity amenityClass = new ChnlExpediaClasses.amenity();
                                            amenityClass.code = objAmenity.name;
                                            var amenityDetailCode = dcChnlExpedia.dbRntChnlExpediaAmenitiesDetailCodeTBLs.SingleOrDefault(x => x.id == objAmenity.pidDetailCode);
                                            if (amenityDetailCode != null)
                                                amenityClass.detailCode = amenityDetailCode.DetailCode;
                                            amenities.Add(amenityClass);
                                        }
                                    }

                                    //ErrorLog.addLog("", "Expedia Room Amenities extrasIds count", extrasIds.Count + "");
                                    var extrasRoomIds = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.pid_config > 0).Select(x => x.pid_config + "").ToList();
                                    var expediaRoomAmenties = dcChnlExpedia.dbRntChnlExpediaLKRoomAmenitiesTBLs.Where(x => extrasRoomIds.Contains(x.refId + "")).ToList();
                                    //ErrorLog.addLog("", "Expedia Room Amenities count", expediaAmenties.Count + "");

                                    if (expediaRoomAmenties != null && expediaRoomAmenties.Count > 0)
                                    {
                                        foreach (var objAmenity in expediaRoomAmenties)
                                        {
                                            ChnlExpediaClasses.amenity amenityClass = new ChnlExpediaClasses.amenity();
                                            amenityClass.code = objAmenity.name;

                                            var amenityDetailCode = dcChnlExpedia.dbRntChnlExpediaRoomAmenitiesDetailCodeTBLs.SingleOrDefault(x => x.id == objAmenity.pidDetailCode);
                                            if (amenityDetailCode != null)
                                                amenityClass.detailCode = amenityDetailCode.DetailCode;
                                            if (objAmenity.pidValue.objToInt32() > 0)
                                                amenityClass.value = objAmenity.pidValue.objToInt32();
                                            else
                                                amenityClass.value = null;

                                            amenities.Add(amenityClass);
                                        }
                                    }

                                    var amenityLang1 = new ChnlExpediaClasses.amenity();
                                    amenityLang1.code = "LANGUAGES_SPOKEN";
                                    amenityLang1.detailCode = "ENGLISH";
                                    amenities.Add(amenityLang1);

                                    var amenityLang2 = new ChnlExpediaClasses.amenity();
                                    amenityLang2.code = "LANGUAGES_SPOKEN";
                                    amenityLang2.detailCode = "ITALIAN";
                                    amenities.Add(amenityLang2);

                                    content.amenities = amenities;

                                }
                                #endregion

                                List<ChnlExpediaClasses.paragraphs> lstParragraphs = new List<ChnlExpediaClasses.paragraphs>();
                                var estateLN = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == lang.id);
                                if (estateLN != null)
                                {
                                    ChnlExpediaClasses.paragraphs paragragh = new ChnlExpediaClasses.paragraphs();
                                    paragragh.code = "DESCRIPTION";
                                    paragragh.value = estateLN.description;
                                    lstParragraphs.Add(paragragh);

                                    ChnlExpediaClasses.paragraphs paragraghDeposit = new ChnlExpediaClasses.paragraphs();
                                    paragraghDeposit.code = "SPECIAL_CHECKIN_INSTRUCTIONS";
                                    paragraghDeposit.value = "Cash breakage deposit: EUR  " + (currEstate.pr_deposit + "").Replace(",", ".") + " per stay";
                                    lstParragraphs.Add(paragraghDeposit);
                                }
                                content.paragraphs = lstParragraphs;
                                contents.Add(content);
                            }
                        }
                        property.contents = contents;
                    }
                    #endregion

                    #region policies

                    //var defCheckin = DateTime.Parse(currEstate.Def_CheckIn);
                    //var formattedCheckinTime = defCheckin.ToString("h tt", CultureInfo.InvariantCulture);

                    //var defCheckout = DateTime.Parse(currEstate.Def_CheckOut);
                    //var formattedCheckoutTime = defCheckout.ToString("h tt", CultureInfo.InvariantCulture);

                    var lstPolicies = new List<ChnlExpediaClasses.policies>();
                    var policyCheckin = new ChnlExpediaClasses.policies();
                    policyCheckin.code = "CHECKIN_TIME";
                    policyCheckin.value = CommonUtilities.getSYS_SETTING("expedia_checkin_time");
                    //policyCheckin.value = formattedCheckinTime;

                    var policyCheckOut = new ChnlExpediaClasses.policies();
                    policyCheckOut.code = "CHECKOUT_TIME";
                    policyCheckOut.value = CommonUtilities.getSYS_SETTING("expedia_checkout_time");

                    //var policyPet = new ChnlExpediaClasses.policies();
                    //policyPet.code = "PET_POLICY";
                    //policyPet.value = currEstate.num_rooms_bath.objToInt32() + "";

                    var policySmoking = new ChnlExpediaClasses.policies();
                    policySmoking.code = "SMOKING_POLICY";
                    policySmoking.detailCode = "SMOKE_FREE_PROPERTY";
                    policySmoking.value = null;
                    var policyCheckINStart = new ChnlExpediaClasses.policies();
                    policyCheckINStart.code = "CHECKIN_START_TIME";
                    policyCheckINStart.value = CommonUtilities.getSYS_SETTING("expedia_checkin_start_time");

                    var policyCheckINEnd = new ChnlExpediaClasses.policies();
                    policyCheckINEnd.code = "CHECKIN_END_TIME";
                    policyCheckINEnd.value = CommonUtilities.getSYS_SETTING("expedia_checkin_end_time"); ;


                    lstPolicies.Add(policySmoking);

                    if (CommonUtilities.getSYS_SETTING("send_checkin_time").objToInt32() == 1)
                    {
                        lstPolicies.Add(policyCheckINStart);
                        lstPolicies.Add(policyCheckINEnd);
                    }
                    lstPolicies.Add(policyCheckOut);

                    if (CommonUtilities.getSYS_SETTING("send_checkin_time").objToInt32() == 0)
                        lstPolicies.Add(policyCheckin);

                    property.policies = lstPolicies;
                    #endregion

                    #region taxes
                    //var lsttaxes = new List<ChnlExpediaClasses.taxes>();
                    //var taxes_vat = new ChnlExpediaClasses.taxes();
                    //taxes_vat.code = "VAT";
                    //taxes_vat.detailCode = "PERCENT_PER_STAY";
                    //taxes_vat.value = "10";
                    //lsttaxes.Add(taxes_vat);
                    //property.taxes = lsttaxes;

                    //magaLocation_DataContext DC_LOCATION = maga_DataContext.DC_LOCATION;
                    //var cityTb = DC_LOCATION.LOC_TB_CITies.SingleOrDefault(x => x.id == currEstate.pid_city.objToInt32());
                    //if (cityTb != null && cityTb..objToDecimal() > 0)
                    //{
                    //    var lstTaxes = new List<ChnlExpediaClasses.propertyCollectedMandatoryFees>();
                    //    var cityTax = new ChnlExpediaClasses.propertyCollectedMandatoryFees();
                    //    cityTax.code = "LOCAL_CITY_TAX";
                    //    cityTax.scope = "AMOUNT_PER_PERSON";
                    //    cityTax.duration = "PER_NIGHT";
                    //    cityTax.value = (cityTb.cityTaxAmount.objToDecimal() + "").Replace(",", ".");
                    //    lstTaxes.Add(cityTax);
                    //    property.propertyCollectedMandatoryFees = lstTaxes;
                    //}

                    var lstMandatoryFees = new List<ChnlExpediaClasses.propertyCollectedMandatoryFees>();
                    var cleaningFee = new ChnlExpediaClasses.propertyCollectedMandatoryFees();
                    cleaningFee.code = "CLEANING_FEE";
                    cleaningFee.scope = "AMOUNT_PER_ACCOMMODATION";
                    cleaningFee.duration = "PER_STAY";
                    cleaningFee.value = "25.00";
                    lstMandatoryFees.Add(cleaningFee);
                    #endregion

                    var inventorySettings = new ChnlExpediaClasses.inventorySettings();
                    inventorySettings.distributionModels = "EXPEDIA_COLLECT";
                    property.inventorySettings = inventorySettings;

                    #region attributes
                    var lstAttributes = new List<ChnlExpediaClasses.attribute>();
                    var attribute = new ChnlExpediaClasses.attribute();
                    attribute.code = "PROPERTY_MANAGER";
                    attribute.value = CommonUtilities.getSYS_SETTING("expediaPM");

                    var attributeBeds = new ChnlExpediaClasses.attribute();
                    attributeBeds.code = "NUMBER_OF_BEDROOMS";
                    attributeBeds.value = currEstate.num_rooms_bed.objToInt32() + "";

                    var attributeBathrooms = new ChnlExpediaClasses.attribute();
                    attributeBathrooms.code = "NUMBER_OF_BATHROOMS";
                    attributeBathrooms.value = currEstate.num_rooms_bath.objToInt32() + "";

                    var attributeOccupancy = new ChnlExpediaClasses.attribute();
                    attributeOccupancy.code = "OCCUPANCY";
                    attributeOccupancy.value = currEstate.num_persons_max.objToInt32() + "";


                    lstAttributes.Add(attributeOccupancy);
                    lstAttributes.Add(attributeBathrooms);
                    lstAttributes.Add(attributeBeds);
                    if (attribute.value != "")
                        lstAttributes.Add(attribute);

                    //if (!string.IsNullOrEmpty(currEstate.registrationNumber))
                    //{
                    //    var attributeCIR = new ChnlExpediaClasses.attribute();
                    //    attributeCIR.code = "HOTEL_REGISTRY_NUMBER";
                    //    attributeCIR.value = currEstate.registrationNumber;
                    //    lstAttributes.Add(attributeCIR);
                    //}

                    property.attributes = lstAttributes;
                    property.propertyCollectedMandatoryFees = lstMandatoryFees;
                    #endregion

                    properties.Add(property);

                    string requestContent = "";
                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, properties);
                    requestContent = wr.ToString();

                    if (CommonUtilities.getSYS_SETTING("is_expedia_content_demo").objToInt32() == 1)
                        requesUrl = "https://services.expediapartnercentral.com/properties/v1/spoofer";
                    else requesUrl = "https://services.expediapartnercentral.com/properties/v1/Magarental";
                    string tmpErrorString = "";
                    //ErrorLog.addLog("", "prop new", "re");
                    var responseData = ChnlExpediaUtils.SendRequestContent(requesUrl, "PUT", requestContent, out tmpErrorString);
                    //ErrorLog.addLog("", "prop new", "rs");
                    //ErrorLog.addLog("", "expedia request", requestContent);
                    //ErrorLog.addLog("", "expedia response", responseData);

                    ChnlExpediaClasses.PropertyNewResponse response = JsonConvert.DeserializeObject<ChnlExpediaClasses.PropertyNewResponse>(responseData);
                    if (response != null)
                    {
                        //ErrorLog.addLog("", "convert exp respose", response.entity[0].expediaId + "");
                        RntChnlExpediaHotelTBL currHotel = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.pidEstate == currEstate.id);
                        if (currHotel == null)
                        {
                            currHotel = new RntChnlExpediaHotelTBL();
                            currHotel.HotelId = response.entity[0].expediaId.objToInt32();
                            currHotel.pidEstate = currEstate.id;
                            dcChnl.RntChnlExpediaHotelTBL.InsertOnSubmit(currHotel);
                            currHotel.password = "ApiMaga2018@";
                            currHotel.username = "EQC_Magarental";
                            dcChnl.SubmitChanges();
                        }
                        currHotel.password = "ApiMaga2018@";
                        currHotel.username = "EQC_Magarental";
                        currHotel.HotelId = response.entity[0].expediaId.objToInt32();
                        currHotel.pidEstate = currEstate.id;
                        currHotel.city = address.city;
                        currHotel.isActive = 1;
                        currHotel.isDemo = 0;
                        currHotel.name = currEstate.code;
                        dcChnl.SubmitChanges();
                    }
                    ErrorString = tmpErrorString;
                    if (CommonUtilities.getSYS_SETTING("is_expedia_log").objToInt32() == 1)
                        ChnlExpediaUtils.addLog("UPDATE", "ChnlExpediaUpdate.PropertyNew_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.PropertyNew_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyNew_process(int idEstate, string host, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Host = host;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "chnlExpediaUpdate.PropertyNew_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
        public class InsertPropertyItem
        {
            public string owner_key { get; set; }
            public string remote_key { get; set; }
            //public ChnlFlipKeyClasses.Property property { get; set; }
            public InsertPropertyItem()
            {
                //property = new ChnlFlipKeyClasses.Property();
            }
        }
    }
    public static string PropertyNew_start(int idEstate, string host)
    {
        PropertyNew_process _tmp = new PropertyNew_process(idEstate, host, false);
        return _tmp.ErrorString;
    }

    private class GetStatus_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    var currHotel = new RntChnlExpediaHotelTBL();
                    currHotel = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.pidEstate == currEstate.id && x.isActive == 1);
                    if (currHotel == null)
                    {
                        return;
                    }

                    string requestContent = "";
                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();

                    if (CommonUtilities.getSYS_SETTING("is_expedia_content_demo").objToInt32() == 1)
                        requesUrl = "https://services.expediapartnercentral.com/properties/v1/spoofer";

                    else
                    {
                        if (currEstate.id == 2212)
                            requesUrl = "https://services.expediapartnercentral.com/properties/v1/Magarental/" + currEstate.id + "/status";
                        else
                            requesUrl = "https://services.expediapartnercentral.com/properties/v1/Magarental/" + App.ProjectName + "_" + currEstate.id + "/status";
                    }
                    //else requesUrl = "https://services.expediapartnercentral.com/properties/" + currHotel.HotelId + "/roomTypes";
                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequestContent(requesUrl, "GET", requestContent, out tmpErrorString);

                    //ErrorLog.addLog("", "expedia room type request " + currEstate.id, requestContent);
                    //ErrorLog.addLog("", "expedia room type response" + currEstate.id, responseData);

                    ChnlExpediaRoomClasses.GetStatusResponse response = JsonConvert.DeserializeObject<ChnlExpediaRoomClasses.GetStatusResponse>(responseData);
                    if (response != null)
                    {
                        ErrorLog.addLog("", "exp hotel id", response.entity.expediaId + "");
                        currHotel.HotelId = response.entity.expediaId;
                        currHotel.status = response.entity.code;
                        //currHotel.message = response.entity.messages.ToList().listToString(",");
                        dcChnl.SubmitChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.PropertyNew_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public GetStatus_process(int idEstate, string host, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Host = host;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "chnlExpediaUpdate.GetStatus_process idEstate:" + idEstate);
            }
            else
                doThread();
        }

    }
    public static string GetStatus_start(int idEstate, string host)
    {
        GetStatus_process _tmp = new GetStatus_process(idEstate, host, false);
        return _tmp.ErrorString;
    }


    private class RoomTypeNew_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    var currHotel = new dbRntChnlExpediaHotelTBL();
                    using (DCchnlExpedia dcExpedia = new DCchnlExpedia())
                    {
                        currHotel = dcExpedia.dbRntChnlExpediaHotelTBLs.SingleOrDefault(x => x.pidEstate == currEstate.id && x.HotelId != null && x.HotelId != 0 && x.isActive == 1);
                        if (currHotel == null)
                        {
                            return;
                        }
                    }

                    //var properties = new List<ChnlExpediaClasses.>();
                    var room = new ChnlExpediaRoomClasses.RoomTypeRequest();
                    if (currEstate.id == 2212)
                        room.partnerCode = currEstate.id + "";
                    else
                        room.partnerCode = App.ProjectName + "_" + currEstate.id + "";

                    room.status = "Active";

                    #region type of room
                    string roomType = "";
                    if (currEstate.category == "villa")
                        roomType = "Villa";
                    else if (currEstate.category == "apt")
                        roomType = "Apartment";

                    var name = new ChnlExpediaRoomClasses.name();
                    var attributes = new ChnlExpediaRoomClasses.attributes();
                    attributes.typeOfRoom = roomType;
                    name.attributes = attributes;
                    room.name = name;

                    #endregion
                    //using (DCchnlExpedia dcChnlExpedia = new DCchnlExpedia())
                    //using (DCmodRental dcRnt = new DCmodRental())
                    //{
                    //    var RoomCategory = dcChnlExpedia.dbRntChnlExpediaRoomTypeRLs.SingleOrDefault(x => x.pidCategory == currEstate.pid_category);
                    //    if (RoomCategory != null)
                    //    {
                    //        var category = dcChnlExpedia.dbRntChnlExpediaLkRoomTypeTBLs.SingleOrDefault(x => x.code == RoomCategory.pidExpediaRoomCategory);
                    //        if (category != null)
                    //        {
                    //            var name = new ChnlExpediaRoomClasses.name();
                    //            var attributes = new ChnlExpediaRoomClasses.attributes();
                    //            attributes.typeOfRoom = category.code;
                    //            name.attributes = attributes;
                    //            room.name = name;
                    //        }
                    //    }
                    //}


                    #region occupancy
                    var occupancy = new ChnlExpediaRoomClasses.Occupancy();
                    occupancy.adults = currEstate.num_persons_adult.objToInt32();
                    //occupancy.children = currEstate.num_persons_child.objToInt32();
                    occupancy.total = currEstate.num_persons_max.objToInt32();
                    room.maxOccupancy = occupancy;
                    #endregion

                    #region age categories
                    var ageCategories = new List<ChnlExpediaRoomClasses.RoomTypeAgeCategory>();

                    var ageCatAdult = new ChnlExpediaRoomClasses.RoomTypeAgeCategory();
                    ageCatAdult.category = "Adult";
                    ageCatAdult.minAge = 0;
                    ageCategories.Add(ageCatAdult);

                    //var ageCatChildren = new ChnlExpediaRoomClasses.RoomTypeAgeCategory();
                    //ageCatChildren.category = "ChildAgeA";
                    //ageCatChildren.minAge = 0;
                    //ageCategories.Add(ageCatChildren);

                    //var ageCatInfant = new ChnlExpediaRoomClasses.RoomTypeAgeCategory();
                    //ageCatInfant.category = "Infant";
                    //ageCatInfant.minAge = 0;
                    //ageCategories.Add(ageCatInfant);
                    //ageCategories.

                    room.ageCategories = ageCategories;
                    #endregion

                    #region smokingPreferences

                    var smokingPreferences = new List<string>();
                    var estateExtrasRLSmoking = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == currEstate.id && x.pid_config == CommonUtilities.getSYS_SETTING("amenity_smoking_allowed").objToInt32());
                    if (estateExtrasRLSmoking != null)
                    {
                        smokingPreferences.Add("Smoking");
                    }
                    var estateExtrasRLNotSmoking = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == currEstate.id && x.pid_config == CommonUtilities.getSYS_SETTING("amenity_smoking_not_allowed").objToInt32());
                    if (estateExtrasRLNotSmoking != null)
                    {
                        smokingPreferences.Add("Non-Smoking");
                    }
                    room.smokingPreferences = smokingPreferences;
                    if (estateExtrasRLSmoking == null && estateExtrasRLNotSmoking == null)
                    {
                        smokingPreferences.Add("Non-Smoking");
                    }
                    room.smokingPreferences = smokingPreferences;

                    #endregion

                    #region roomsize
                    var roomSize = new ChnlExpediaRoomClasses.roomSize();
                    roomSize.squareMeters = currEstate.mq_inner.objToInt32();
                    roomSize.squareFeet = (currEstate.mq_inner.objToDouble() * 10.764).objToInt32();
                    room.roomSize = roomSize;
                    #endregion

                    #region bedding
                    var standardBeddings = new List<ChnlExpediaRoomClasses.standardBedding>();
                    var standardBedding = new ChnlExpediaRoomClasses.standardBedding();
                    var options = new List<ChnlExpediaRoomClasses.option>();
                    var finalOptions = new List<ChnlExpediaRoomClasses.option>();

                    //int co
                    var EstateInternsIds = dc.RntEstateInternsTB.Where(x => x.pidEstate == currEstate.id && (x.pidInternsType == "Bedroom" || x.pidInternsType == "Livingroom")).Select(x => x.id).ToList();
                    var featureRL = dc.RntEstateInternsFeatureRL.Where(x => EstateInternsIds.Contains(x.pidEstateInterns)).ToList();
                    //var featureRLIds = dc.dbRntEstateInternsFeatureTBs

                    //    using (DCchnlExpedia dcChnlExpedia = new DCchnlExpedia())
                    //    {
                    //    var beds = dcChnlExpedia.dbRntChnlExpediaBedsRLs.ToList();
                    //    foreach(var bed in beds)
                    //    {
                    //    }
                    //}

                    var existingFeature = new List<int>();
                    foreach (var feature in featureRL)
                    {
                        if (!existingFeature.Contains(feature.pidInternsFeature))
                        {
                            //var beds = dc.dbRntChnlExpediaBedsRLs.SingleOrDefault(x => x.pidFeature == feature.pidInternsFeature);
                            int count = featureRL.Where(x => x.pidInternsFeature == feature.pidInternsFeature).ToList().Sum(x => x.count.Value);
                            using (DCchnlExpedia dcChnlExpedia = new DCchnlExpedia())
                            using (DCmodRental dcRnt = new DCmodRental())
                            {
                                var beds = dcChnlExpedia.dbRntChnlExpediaBedsRLs.SingleOrDefault(x => x.pidFeature == feature.pidInternsFeature);
                                if (beds != null)
                                {
                                    var expBed = dcChnlExpedia.dbRntChnlExpediaLkBedsTBLs.SingleOrDefault(x => x.code == beds.pidExpediaBed);
                                    if (expBed != null)
                                    {
                                        var option = new ChnlExpediaRoomClasses.option();
                                        option.type = expBed.code;
                                        option.quantity = count;
                                        options.Add(option);
                                    }
                                }
                            }
                        }
                        existingFeature.Add(feature.pidInternsFeature);
                    }

                    var codes = options.Select(x => x.type).Distinct();
                    foreach (var code in codes)
                    {
                        int count = options.Where(x => x.type == code).ToList().Sum(x => x.quantity);
                        var option = new ChnlExpediaRoomClasses.option();
                        option.type = code;
                        option.quantity = count;
                        finalOptions.Add(option);
                    }
                    standardBedding.option = finalOptions;
                    room.standardBedding.Add(standardBedding);
                    #endregion
                    //room.ageCategories

                    string requestContent = "";
                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, room);
                    requestContent = wr.ToString();

                    if (CommonUtilities.getSYS_SETTING("is_expedia_content_demo").objToInt32() == 1)
                        requesUrl = "https://services.expediapartnercentral.com/properties/v1/spoofer";
                    else requesUrl = "https://services.expediapartnercentral.com/properties/" + currHotel.HotelId + "/roomTypes";
                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequestRoomType(requesUrl, "POST", requestContent, out tmpErrorString);

                    //ErrorLog.addLog("", "expedia room type request " + currEstate.id, requestContent);
                    //ErrorLog.addLog("", "expedia room type response" + currEstate.id, responseData);

                    ChnlExpediaRoomClasses.RoomTypeResponse response = JsonConvert.DeserializeObject<ChnlExpediaRoomClasses.RoomTypeResponse>(responseData);
                    if (response != null)
                    {
                        string roomId = response.entity.resourceId + "";
                        ErrorLog.addLog("", "convert exp rateplan respose", roomId);

                        #region stroed in room type table
                        RntChnlExpediaRoomTypeTBL currRoom = dcChnl.RntChnlExpediaRoomTypeTBL.SingleOrDefault(x => x.HotelId == currHotel.HotelId && x.id == roomId);
                        if (currRoom == null)
                        {
                            currRoom = new RntChnlExpediaRoomTypeTBL();
                            currRoom.HotelId = currHotel.HotelId;
                            currRoom.id = roomId;
                            dcChnl.RntChnlExpediaRoomTypeTBL.InsertOnSubmit(currRoom);
                            dcChnl.SubmitChanges();
                        }
                        currRoom.code = currRoom.name = currEstate.code;
                        currRoom.maxOccupants = room.maxOccupancy.objToInt32();
                        currRoom.smokingPref = room.smokingPreferences.listToString(",");
                        int status = 0;
                        if (response.entity.status == "Active")
                            currRoom.status = 1;
                        else if (response.entity.status == "Inactive")
                            currRoom.status = status;
                        dcChnl.SubmitChanges();
                        ErrorString = tmpErrorString;
                        #endregion

                        #region stroed in estate room type table
                        //dbRntChnlExpediaEstateRoomRL currEsateRooom = dcChnl.dbRntChnlExpediaEstateRoomRLs.SingleOrDefault(x => x.pidEstate == currEstate.id && x.RoomTypeId == roomId);
                        //if (currEsateRooom == null)
                        //{
                        //    currEsateRooom = new dbRntChnlExpediaEstateRoomRL();
                        //    currEsateRooom.pidEstate = currEstate.id;
                        //    currEsateRooom.RoomTypeId = roomId;
                        //    dcChnl.Add(currEsateRooom);
                        //    dcChnl.SaveChanges();
                        //}
                        #endregion

                        RntChnlExpediaEstateTBL currExpediaEstate = dcChnl.RntChnlExpediaEstateTBL.FirstOrDefault(x => x.id == currEstate.id);
                        if (currExpediaEstate == null)
                        {
                            currExpediaEstate = new RntChnlExpediaEstateTBL();
                            currExpediaEstate.id = currEstate.id;
                            currExpediaEstate.RoomTypeId = roomId;
                            currExpediaEstate.HotelId = currHotel.HotelId;
                            dcChnl.RntChnlExpediaEstateTBL.InsertOnSubmit(currExpediaEstate);
                            dcChnl.SubmitChanges();
                        }
                        currExpediaEstate.RoomTypeId = roomId;
                        currExpediaEstate.HotelId = currHotel.HotelId;
                        dcChnl.SubmitChanges();
                        ChnlExpediaUtils.addLog("UPDATE", "ChnlExpediaUpdate.RoomTypeNew_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                    }
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.PropertyNew_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public RoomTypeNew_process(int idEstate, string host, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Host = host;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "chnlExpediaUpdate.PropertyNew_process idEstate:" + idEstate);
            }
            else
                doThread();
        }

    }
    public static string RoomTypeNew_start(int idEstate, string host)
    {
        RoomTypeNew_process _tmp = new RoomTypeNew_process(idEstate, host, false);
        return _tmp.ErrorString;
    }

    private class RoomTypeUpdate_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    var currHotel = new RntChnlExpediaHotelTBL();
                    var currRoom = new RntChnlExpediaEstateTBL();
                    var mainRoom = new RntChnlExpediaRoomTypeTBL();

                    using (magaChnlExpediaDataContext dcExpedia = maga_DataContext.DC_EXP)
                    {
                        currHotel = dcExpedia.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.pidEstate == currEstate.id && x.isActive == 1);
                        if (currHotel == null)
                        {
                            return;
                        }

                        currRoom = dcExpedia.RntChnlExpediaEstateTBL.SingleOrDefault(x => x.id == currEstate.id);
                        if (currRoom == null)
                        {
                            return;
                        }

                        mainRoom = dcChnl.RntChnlExpediaRoomTypeTBL.SingleOrDefault(x => x.id == currRoom.RoomTypeId);
                        //if (mainRoom == null)
                        //{
                        //    return;
                        //}
                    }

                    var room = new ChnlExpediaRoomClasses.RoomTypeUpdateRequest();
                    if (currEstate.id == 2212)
                        room.partnerCode = currEstate.id + "";
                    else
                        room.partnerCode = App.ProjectName + "_" + currEstate.id + "";

                    //room.partnerCode = App.ProjectName + "_" + currEstate.id + "";
                    if (currRoom == null)
                        room.resourceId = CommonUtilities.getSYS_SETTING("roomId").objToInt32();
                    else
                        room.resourceId = currRoom.RoomTypeId.objToInt32();

                    if (mainRoom == null)
                        room.status = "Inactive";
                    else
                    {
                        if (mainRoom.status == 1)
                            room.status = "Active";
                        else
                            room.status = "Inactive";
                    }

                    #region type of room
                    string roomType = "";

                    if (currEstate.category == "villa")
                        roomType = "Villa";
                    else if (currEstate.category == "apt")
                        roomType = "Apartment";


                    var name = new ChnlExpediaRoomClasses.name();
                    var attributes = new ChnlExpediaRoomClasses.attributes();
                    attributes.typeOfRoom = roomType;
                    name.attributes = attributes;
                    room.name = name;

                    //using (DCchnlExpedia dcChnlExpedia = new DCchnlExpedia())
                    //using (DCmodRental dcRnt = new DCmodRental())
                    //{
                    //    var RoomCategory = dcChnlExpedia.dbRntChnlExpediaRoomTypeRLs.SingleOrDefault(x => x.pidCategory == currEstate.pid_category);
                    //    if (RoomCategory != null)
                    //    {
                    //        var category = dcChnlExpedia.dbRntChnlExpediaLkRoomTypeTBLs.SingleOrDefault(x => x.code == RoomCategory.pidExpediaRoomCategory);
                    //        if (category != null)
                    //        {
                    //            var name = new ChnlExpediaRoomClasses.name();
                    //            var attributes = new ChnlExpediaRoomClasses.attributes();
                    //            attributes.typeOfRoom = category.code;
                    //            name.attributes = attributes;
                    //            room.name = name;
                    //        }
                    //    }
                    //}
                    #endregion

                    #region occupancy
                    var occupancy = new ChnlExpediaRoomClasses.Occupancy();
                    occupancy.adults = currEstate.num_persons_adult.objToInt32();
                    //occupancy.children = currEstate.num_persons_child.objToInt32();
                    occupancy.total = currEstate.num_persons_max.objToInt32();
                    room.maxOccupancy = occupancy;
                    #endregion

                    #region age categories
                    var ageCategories = new List<ChnlExpediaRoomClasses.RoomTypeAgeCategory>();

                    var ageCatAdult = new ChnlExpediaRoomClasses.RoomTypeAgeCategory();
                    ageCatAdult.category = "Adult";
                    ageCatAdult.minAge = 0;
                    ageCategories.Add(ageCatAdult);

                    //var ageCatChildren = new ChnlExpediaRoomClasses.RoomTypeAgeCategory();
                    //ageCatChildren.category = "ChildAgeA";
                    //ageCatChildren.minAge = 0;
                    //ageCategories.Add(ageCatChildren);

                    //var ageCatInfant = new ChnlExpediaRoomClasses.RoomTypeAgeCategory();
                    //ageCatInfant.category = "Infant";
                    //ageCatInfant.minAge = 0;
                    //ageCategories.Add(ageCatInfant);
                    //ageCategories.

                    room.ageCategories = ageCategories;
                    #endregion

                    #region smokingPreferences
                    var smokingPreferences = new List<string>();
                    var estateExtrasRLSmoking = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == currEstate.id && x.pid_config == CommonUtilities.getSYS_SETTING("amenity_smoking_allowed").objToInt32());
                    if (estateExtrasRLSmoking != null)
                    {
                        smokingPreferences.Add("Smoking");
                    }
                    var estateExtrasRLNotSmoking = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == currEstate.id && x.pid_config == CommonUtilities.getSYS_SETTING("amenity_smoking_not_allowed").objToInt32());
                    if (estateExtrasRLNotSmoking != null)
                    {
                        smokingPreferences.Add("Non-Smoking");
                    }
                    room.smokingPreferences = smokingPreferences;
                    if (estateExtrasRLSmoking == null && estateExtrasRLNotSmoking == null)
                    {
                        smokingPreferences.Add("Non-Smoking");
                    }
                    room.smokingPreferences = smokingPreferences;
                    #endregion

                    #region roomsize
                    var roomSize = new ChnlExpediaRoomClasses.roomSize();
                    roomSize.squareMeters = currEstate.mq_inner.objToInt32();
                    roomSize.squareFeet = (currEstate.mq_inner.objToDouble() * 10.764).objToInt32();
                    room.roomSize = roomSize;
                    #endregion

                    #region bedding
                    var standardBeddings = new List<ChnlExpediaRoomClasses.standardBedding>();
                    var standardBedding = new ChnlExpediaRoomClasses.standardBedding();
                    var options = new List<ChnlExpediaRoomClasses.option>();
                    var finalOptions = new List<ChnlExpediaRoomClasses.option>();

                    //int co
                    //ErrorLog.addLog("", "interns", "Count");
                    var EstateInternsIds = dc.RntEstateInternsTB.Where(x => x.pidEstate == currEstate.id && (x.pidInternsType == "Bedroom" || x.pidInternsType == "Livingroom")).Select(x => x.id).ToList();
                    //ErrorLog.addLog("", "interns count", EstateInternsIds.Count + "");
                    var featureRL = dc.RntEstateInternsFeatureRL.Where(x => EstateInternsIds.Contains(x.pidEstateInterns)).ToList();
                    //ErrorLog.addLog("", "interns count feature", featureRL.Count + "");
                    var existingFeature = new List<int>();
                    foreach (var feature in featureRL)
                    {
                        if (!existingFeature.Contains(feature.pidInternsFeature))
                        {
                            int count = featureRL.Where(x => x.pidInternsFeature == feature.pidInternsFeature).ToList().Sum(x => x.count.Value);
                            ErrorLog.addLog("", "interns count feature count", feature.pidInternsFeature + " c " + count + "");
                            using (DCchnlExpedia dcChnlExpedia = new DCchnlExpedia())
                            using (DCmodRental dcRnt = new DCmodRental())
                            {
                                var beds = dcChnlExpedia.dbRntChnlExpediaBedsRLs.SingleOrDefault(x => x.pidFeature == feature.pidInternsFeature);
                                if (beds != null)
                                {
                                    var expBed = dcChnlExpedia.dbRntChnlExpediaLkBedsTBLs.SingleOrDefault(x => x.code == beds.pidExpediaBed);
                                    if (expBed != null)
                                    {
                                        var option = new ChnlExpediaRoomClasses.option();
                                        option.type = expBed.code;
                                        option.quantity = count;
                                        options.Add(option);
                                    }
                                }
                            }
                            existingFeature.Add(feature.pidInternsFeature);
                        }
                    }

                    var codes = options.Select(x => x.type).Distinct();
                    foreach (var code in codes)
                    {
                        int count = options.Where(x => x.type == code).ToList().Sum(x => x.quantity);
                        var option = new ChnlExpediaRoomClasses.option();
                        option.type = code;
                        option.quantity = count;
                        finalOptions.Add(option);
                    }
                    standardBedding.option = finalOptions;
                    room.standardBedding.Add(standardBedding);
                    #endregion
                    //room.ageCategories

                    string requestContent = "";
                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, room);
                    requestContent = wr.ToString();

                    if (CommonUtilities.getSYS_SETTING("is_expedia_content_demo").objToInt32() == 1)
                        requesUrl = "https://services.expediapartnercentral.com/properties/v1/spoofer";
                    else requesUrl = "https://services.expediapartnercentral.com/properties/" + currHotel.HotelId + "/roomTypes/" + room.resourceId;
                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequestRoomType(requesUrl, "PUT", requestContent, out tmpErrorString);

                    //ErrorLog.addLog("", "expedia room type update request " + currEstate.id, requestContent);
                    //ErrorLog.addLog("", "expedia room type update response" + currEstate.id, responseData);

                    ChnlExpediaRoomClasses.RoomTypeResponse response = JsonConvert.DeserializeObject<ChnlExpediaRoomClasses.RoomTypeResponse>(responseData);
                    if (response != null)
                    {
                        string roomId = response.entity.resourceId + "";
                        ErrorLog.addLog("", "convert exp rateplan respose", roomId);

                        #region stroed in room type table
                        RntChnlExpediaRoomTypeTBL objRoom = dcChnl.RntChnlExpediaRoomTypeTBL.SingleOrDefault(x => x.HotelId == currHotel.HotelId && x.id == roomId);
                        if (objRoom == null)
                        {
                            objRoom = new RntChnlExpediaRoomTypeTBL();
                            objRoom.HotelId = currHotel.HotelId;
                            objRoom.id = roomId;
                            dcChnl.RntChnlExpediaRoomTypeTBL.InsertOnSubmit(objRoom);
                            dcChnl.SubmitChanges();
                        }
                        objRoom.code = objRoom.name = currEstate.code;
                        objRoom.maxOccupants = room.maxOccupancy.objToInt32();
                        objRoom.smokingPref = room.smokingPreferences.listToString(",");
                        int status = 0;
                        if (response.entity.status == "Active")
                            objRoom.status = 1;
                        else if (response.entity.status == "Inactive")
                            objRoom.status = status;
                        dcChnl.SubmitChanges();
                        ErrorString = tmpErrorString;
                        //ChnlExpediaUtils.addLog("UPDATE", "ChnlExpediaUpdate.RoomTypeUpdate_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);
                        #endregion

                        #region stroed in estate room type table
                        //dbRntChnlExpediaEstateRoomRL currEsateRooom = dcChnl.dbRntChnlExpediaEstateRoomRLs.SingleOrDefault(x => x.pidEstate == currEstate.id && x.RoomTypeId == roomId);
                        //if (currEsateRooom == null)
                        //{
                        //    currEsateRooom = new dbRntChnlExpediaEstateRoomRL();
                        //    currEsateRooom.pidEstate = currEstate.id;
                        //    currEsateRooom.RoomTypeId = roomId;
                        //    dcChnl.Add(currEsateRooom);
                        //    dcChnl.SaveChanges();
                        //}
                        #endregion

                        RntChnlExpediaEstateTBL currExpediaEstate = dcChnl.RntChnlExpediaEstateTBL.FirstOrDefault(x => x.id == currEstate.id);
                        if (currExpediaEstate == null)
                        {
                            currExpediaEstate = new RntChnlExpediaEstateTBL();
                            currExpediaEstate.id = currEstate.id;
                            currExpediaEstate.RoomTypeId = roomId;
                            currExpediaEstate.HotelId = currHotel.HotelId;
                            dcChnl.RntChnlExpediaEstateTBL.InsertOnSubmit(currExpediaEstate);
                            dcChnl.SubmitChanges();
                        }
                        currExpediaEstate.RoomTypeId = roomId;
                        currExpediaEstate.HotelId = currHotel.HotelId;
                        dcChnl.SubmitChanges();
                    }

                    ErrorString = tmpErrorString;

                    ChnlExpediaUtils.addLog("UPDATE", "ChnlExpediaUpdate.RoomTypeUpdate_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.RoomTypeUpdate_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public RoomTypeUpdate_process(int idEstate, string host, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Host = host;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "chnlExpediaUpdate.RoomTypeUpdate_process idEstate:" + idEstate);
            }
            else
                doThread();
        }

    }
    public static string RoomTypeUpdate_start(int idEstate, string host)
    {
        RoomTypeUpdate_process _tmp = new RoomTypeUpdate_process(idEstate, host, false);
        return _tmp.ErrorString;
    }

    private class RatePlanCreate_process
    {
        public ChnlExpediaClasses.RatePlanRequest RatePlan { get; set; }
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    var currHotel = new RntChnlExpediaHotelTBL();
                    var currRoom = new  RntChnlExpediaEstateTBL();

                    using (magaChnlExpediaDataContext dcExpedia = maga_DataContext.DC_EXP)
                    {
                        currHotel = dcExpedia.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.pidEstate == currEstate.id && x.isActive == 1);
                        if (currHotel == null)
                        {
                            return;
                        }

                        currRoom = dcExpedia.RntChnlExpediaEstateTBL.SingleOrDefault(x => x.id == currEstate.id);
                        if (currRoom == null)
                        {
                            return;
                        }
                    }

                    string requestContent = "";
                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, RatePlan);
                    requestContent = wr.ToString();

                    requesUrl = "https://services.expediapartnercentral.com/properties/" + currHotel.HotelId + "/roomTypes/" + currRoom.RoomTypeId + "/ratePlans";


                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequestRoomType(requesUrl, "POST", requestContent, out tmpErrorString);

                    //ErrorLog.addLog("", "expedia rateplan type request " + currEstate.id, requestContent);
                    //ErrorLog.addLog("", "expedia rateplan type response" + currEstate.id, responseData);

                    ChnlExpediaClasses.RatePlanResponse response = JsonConvert.DeserializeObject<ChnlExpediaClasses.RatePlanResponse>(responseData);
                    if (response != null)
                    {
                        string roomId = response.entity.resourceId + "";
                        //ErrorLog.addLog("", "convert exp rateplan respose", roomId);

                        RntChnlExpediaRoomTypeRatePlanTBL currRatePlan = new RntChnlExpediaRoomTypeRatePlanTBL();
                        if (response.entity != null)
                        {
                            currRatePlan.RoomTypeId = currRoom.RoomTypeId;
                            currRatePlan.RatePlanId = response.entity.resourceId;
                            currRatePlan.code = currRatePlan.name = response.entity.name;
                            currRatePlan.rateAcquisitionType = response.entity.rateAcquisitionType;

                            string strDistributionRule = "";
                            var distributionRules = response.entity.distributionRules.ToList();
                            if (distributionRules != null && distributionRules.Count > 0)
                            {
                                foreach (var distributionRule in distributionRules)
                                {
                                    if (strDistributionRule == "")
                                        strDistributionRule = distributionRule.distributionModel;
                                    else
                                        strDistributionRule = " ," + distributionRule.distributionModel;
                                }
                            }
                            currRatePlan.status = response.entity.status == "Active" ? 1 : 0;
                            currRatePlan.type = response.entity.type;
                            currRatePlan.pricingModel = response.entity.pricingModel;
                            currRatePlan.occupantsForBaseRate = response.entity.occupantsForBaseRate;
                            currRatePlan.minLOSDefault = response.entity.minLOSDefault;
                            currRatePlan.maxLOSDefault = response.entity.maxLOSDefault;
                            currRatePlan.minAdvBookDays = response.entity.minAdvBookDays;
                            currRatePlan.maxAdvBookDays = response.entity.maxAdvBookDays;

                            if (response.entity.bookDateStart != "" && response.entity.bookDateStart.Split('-').Length > 2)
                            {
                                int year = response.entity.bookDateStart.Split('-')[0].objToInt32();
                                int month = response.entity.bookDateStart.Split('-')[1].objToInt32();
                                int day = response.entity.bookDateStart.Split('-')[2].objToInt32();

                                currRatePlan.bookDateStart = new DateTime(year, month, day);
                            }

                            if (response.entity.bookDateEnd != "" && response.entity.bookDateEnd.Split('-').Length > 2)
                            {
                                int year = response.entity.bookDateEnd.Split('-')[0].objToInt32();
                                int month = response.entity.bookDateEnd.Split('-')[1].objToInt32();
                                int day = response.entity.bookDateEnd.Split('-')[2].objToInt32();

                                currRatePlan.bookDateEnd = new DateTime(year, month, day);
                            }

                            if (response.entity.travelDateStart != "" && response.entity.travelDateStart.Split('-').Length > 2)
                            {
                                int year = response.entity.travelDateStart.Split('-')[0].objToInt32();
                                int month = response.entity.travelDateStart.Split('-')[1].objToInt32();
                                int day = response.entity.travelDateStart.Split('-')[2].objToInt32();

                                currRatePlan.travelDateStart = new DateTime(year, month, day);
                            }

                            if (response.entity.travelDateEnd != "" && response.entity.travelDateEnd.Split('-').Length > 2)
                            {
                                int year = response.entity.travelDateEnd.Split('-')[0].objToInt32();
                                int month = response.entity.travelDateEnd.Split('-')[1].objToInt32();
                                int day = response.entity.travelDateEnd.Split('-')[2].objToInt32();

                                currRatePlan.travelDateEnd = new DateTime(year, month, day);
                            }
                            currRatePlan.mobileOnly = response.entity.mobileOnly;
                            currRatePlan.createDateTime = response.entity.creationDateTime;
                            currRatePlan.updateDateTime = response.entity.lastUpdateDateTime;
                            dcChnl.RntChnlExpediaRoomTypeRatePlanTBL.InsertOnSubmit(currRatePlan);
                            dcChnl.SubmitChanges();

                            //var cancalltionPolicy = new dbRntChnlExpediaRoomTypeRatePlanCancelPolicyPenaltyTBL();
                            //cancalltionPolicy.insideWindow = 0;
                            //cancalltionPolicy.

                            var policies = RatePlan.cancelPolicy;
                            var defaultPolicies = policies.defaultPenalties;

                            foreach (var penalty in defaultPolicies)
                            {
                                var cancelPolicyTbl = new RntChnlExpediaRoomTypeRatePlanCancelPolicyTBL();
                                cancelPolicyTbl.uid = Guid.NewGuid();
                                cancelPolicyTbl.RoomTypeId = currRatePlan.RoomTypeId;
                                cancelPolicyTbl.RatePlanId = currRatePlan.RatePlanId;
                                cancelPolicyTbl.cancelWindow = penalty.deadline;
                                cancelPolicyTbl.createDateTime = DateTime.Now + "";
                                cancelPolicyTbl.updateDateTime = DateTime.Now + "";
                                dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyTBL.InsertOnSubmit(cancelPolicyTbl);
                                dcChnl.SubmitChanges();

                                var cancelPolicyPenaltyTbl = new RntChnlExpediaRoomTypeRatePlanCancelPolicyPenaltyTBL();
                                cancelPolicyPenaltyTbl.uid = Guid.NewGuid();
                                cancelPolicyPenaltyTbl.CancelPolicyUid = cancelPolicyTbl.uid;
                                cancelPolicyPenaltyTbl.perStayFee = penalty.perStayFee;
                                dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyPenaltyTBL.InsertOnSubmit(cancelPolicyPenaltyTbl);
                                dcChnl.SubmitChanges();
                            }

                        }
                    }
                    ErrorString = tmpErrorString;

                    ChnlExpediaUtils.addLog("RUPDATE", "ChnlExpediaUpdate.RatePlanCreate_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("RERROR", "ChnlExpediaUpdate.RatePlanCreate_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public RatePlanCreate_process(ChnlExpediaClasses.RatePlanRequest ratePlan, int idEstate, bool backgroundProcess)
        {
            RatePlan = ratePlan;
            IdEstate = idEstate;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "chnlExpediaUpdate.RatePlanCreate_process idEstate:" + idEstate);
            }
            else
                doThread();
        }

    }
    public static string RatePlanCreate_start(ChnlExpediaClasses.RatePlanRequest ratePlan, int idEstate)
    {
        RatePlanCreate_process _tmp = new RatePlanCreate_process(ratePlan, idEstate, false);
        return _tmp.ErrorString;
    }

    private class RatePlanDelete_process
    {
        public int IdEstate { get; set; }
        public string IdRatePlan { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                    var ratePlan = dcChnl.RntChnlExpediaRoomTypeRatePlanTBL.SingleOrDefault(x => x.RatePlanId == IdRatePlan);
                    if (ratePlan == null)
                    {
                        return;
                    }

                    var currRoom = dcChnl.RntChnlExpediaEstateTBL.SingleOrDefault(x => x.RoomTypeId == ratePlan.RoomTypeId);
                    {
                        if (currRoom == null) return;
                    }

                    string requestContent = "";
                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    requesUrl = "https://services.expediapartnercentral.com/properties/" + currRoom.HotelId + "/roomTypes/" + currRoom.RoomTypeId + "/ratePlans/" + IdRatePlan;

                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequestRoomType(requesUrl, "DELETE", requestContent, out tmpErrorString);

                    ChnlExpediaClasses.RatePlanResponse response = JsonConvert.DeserializeObject<ChnlExpediaClasses.RatePlanResponse>(responseData);
                    if (response != null)
                    {


                    }
                    ErrorString = tmpErrorString;
                    ChnlExpediaUtils.addLog("RUPDATE", "ChnlExpediaUpdate.RatePlanCreate_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("RERROR", "ChnlExpediaUpdate.RatePlanCreate_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public RatePlanDelete_process(string ratePlanId, int idEstate, bool backgroundProcess)
        {
            IdRatePlan = ratePlanId;
            IdEstate = idEstate;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "chnlExpediaUpdate.RatePlanCreate_process idEstate:" + idEstate);
            }
            else
                doThread();
        }

    }
    public static string RatePlanDelete_start(string ratePlanId, int idEstate)
    {
        RatePlanDelete_process _tmp = new RatePlanDelete_process(ratePlanId, idEstate, false);
        return _tmp.ErrorString;
    }

    private class RoomTypeCreateAmenity_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "APRTMENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    var currHotel = new RntChnlExpediaHotelTBL();
                    var currRoom = new  RntChnlExpediaEstateTBL();
                    var mainRoom = new  RntChnlExpediaRoomTypeTBL();

                    using (magaChnlExpediaDataContext dcExpedia = maga_DataContext.DC_EXP)
                    {
                        currHotel = dcExpedia.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.pidEstate == currEstate.id && x.isActive == 1);
                        if (currHotel == null)
                        {
                            ErrorLog.addLog("", "Expedia Room Amenities", "Expedia Hotel miss");
                            return;
                        }

                        currRoom = dcExpedia.RntChnlExpediaEstateTBL.SingleOrDefault(x => x.id == currEstate.id);
                        if (currRoom == null)
                        {
                            ErrorLog.addLog("", "Expedia Room Amenities", "Expedia Estate miss");
                            return;
                        }

                        mainRoom = dcChnl.RntChnlExpediaRoomTypeTBL.SingleOrDefault(x => x.id == currRoom.RoomTypeId);
                        if (mainRoom == null)
                        {
                            ErrorLog.addLog("", "Expedia Room Amenities", "Expedia room miss");
                            return;
                        }

                        #region amenities
                        List<ChnlExpediaClasses.amenity> amenities = new List<ChnlExpediaClasses.amenity>();
                        using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
                        {
                            var extrasIds = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.pid_config > 0).Select(x => x.pid_config + "").ToList();
                            //ErrorLog.addLog("", "Expedia Room Amenities extrasIds count", extrasIds.Count + "");
                            var expediaAmenties = dcExpedia.RntChnlExpediaLKRoomAmenitiesTBL.Where(x => extrasIds.Contains(x.refId + "")).ToList();

                            //ErrorLog.addLog("", "Expedia Room Amenities count", expediaAmenties.Count + "");

                            if (expediaAmenties != null && expediaAmenties.Count > 0)
                            {
                                foreach (var objAmenity in expediaAmenties)
                                {
                                    ChnlExpediaClasses.amenity amenityClass = new ChnlExpediaClasses.amenity();
                                    amenityClass.code = objAmenity.name;

                                    var amenityDetailCode = dcExpedia.RntChnlExpediaRoomAmenitiesDetailCodeTBL.SingleOrDefault(x => x.id == objAmenity.pidDetailCode);
                                    if (amenityDetailCode != null)
                                        amenityClass.detailCode = amenityDetailCode.DetailCode;
                                    if (objAmenity.pidValue.objToInt32() > 0)
                                        amenityClass.value = objAmenity.pidValue.objToInt32();
                                    else
                                        amenityClass.value = null;

                                    amenities.Add(amenityClass);
                                }
                            }

                        }
                        #endregion

                        string requestContent = "";
                        TextWriter wr = new StringWriter();
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.NullValueHandling = NullValueHandling.Ignore;
                        serializer.Serialize(wr, amenities);
                        requestContent = wr.ToString();

                        if (CommonUtilities.getSYS_SETTING("is_expedia_content_demo").objToInt32() == 1)
                            requesUrl = "https://services.expediapartnercentral.com/properties/v1/spoofer";
                        else requesUrl = "https://services.expediapartnercentral.com/properties/" + currHotel.HotelId + "/roomTypes/" + currRoom.RoomTypeId.objToInt32() + "/amenities";
                        //else requesUrl = "https://services.expediapartnercentral.com/properties/" + currHotel.HotelId + "/roomTypes/" + room.resourceId;
                        string tmpErrorString = "";
                        var responseData = ChnlExpediaUtils.SendRequestRoomType(requesUrl, "PUT", requestContent, out tmpErrorString);

                        ErrorLog.addLog("", "expedia room type amenity request " + currEstate.id, requestContent);
                        ChnlExpediaClasses.amenityResponse response = JsonConvert.DeserializeObject<ChnlExpediaClasses.amenityResponse>(responseData);
                        if (response != null)
                        {
                            ChnlExpediaUtils.addLog("UPDATE", "ChnlExpediaUpdate.RoomTypeCreateAmenity_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);
                        }
                        ErrorString = tmpErrorString;

                    }
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.RoomTypeUpdate_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public RoomTypeCreateAmenity_process(int idEstate, string host, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Host = host;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "chnlExpediaUpdate.RoomTypeCreateAmenity_process idEstate:" + idEstate);
            }
            else
                doThread();
        }

    }
    public static string RoomTypeCreateAmenity_start(int idEstate, string host)
    {
        RoomTypeCreateAmenity_process _tmp = new RoomTypeCreateAmenity_process(idEstate, host, false);
        return _tmp.ErrorString;
    }

}
public class ChnlExpediaImport
{
    private class BookingRetrieval_process
    {
        public int HotelId { get; set; }
        public long agentID { get; set; }
        private int NbDaysInPast { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                using (var dcOld = maga_DataContext.DC_RENTAL)
                using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var hotelTbl = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.HotelId == HotelId);
                    if (hotelTbl == null)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "HotelId: " + HotelId + " NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    if (hotelTbl.isDemo == 1) requesUrl = "https://simulator.expediaquickconnect.com/connect/br";
                    else requesUrl = "https://services.expediapartnercentral.com/eqc/br";
                    //else requesUrl = "https://ws.expediaquickconnect.com/connect/br";
                    var Request = new ChnlExpediaClasses.BookingRetrievalRQ(hotelTbl);

                    if (CommonUtilities.getSYS_SETTING("exp_remove_past_days").objToInt32() == 0)
                        Request.NbDaysInPast = NbDaysInPast;

                    string tmpErrorString = "";

                    var responseData = ChnlExpediaUtils.SendRequest(requesUrl, Request.GetXml(), out tmpErrorString);
                    ChnlExpediaUtils.addLog("BookingRetrievalResponse", responseData, "", "", "");
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                        return;
                    var Response = new ChnlExpediaClasses.BookingRetrievalRS(responseData);
                    if (Response.Errors.Count > 0)
                    {
                        foreach (var error in Response.Errors)
                            ErrorString += "\n\r" + error.description;
                        ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaImport.BookingRetrieval_process HotelId:" + HotelId, "", ErrorString, "");
                        return;
                    }
                    int countCreated = 0;
                    int countUpdated = 0;
                    int countCanceled = 0;
                    if (CommonUtilities.getSYS_SETTING("is_debug").objToInt32() == 1)
                    {
                        var jsonSerialiser = new JavaScriptSerializer();
                        var json = jsonSerialiser.Serialize(Response);
                        try
                        {
                            ChnlExpediaUtils.addLog("Debug", "ChnlExpediaImport.BookingRetrieval_process  Response", " HotelId:" + HotelId, json, "");
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    List<int> lstResChangeEstateId = new List<int>();
                    foreach (var booking in Response.Bookings)
                    {
                        try
                        {
                            #region Import Reservation
                            var roomStay = booking.roomStay;
                            if (roomStay == null) continue;

                            var chnlBooking = dcChnl.RntChnlExpediaBookingTBL.SingleOrDefault(x => x.id == booking._id);
                            if (chnlBooking == null)
                            {
                                chnlBooking = new RntChnlExpediaBookingTBL() { id = booking._id };
                                chnlBooking.HotelId = HotelId;
                                chnlBooking.roomTypeID = roomStay._roomTypeID;
                                chnlBooking.ratePlanID = roomStay._ratePlanID;
                                dcChnl.RntChnlExpediaBookingTBL.InsertOnSubmit(chnlBooking);
                                dcChnl.SubmitChanges();
                            }
                            chnlBooking.type = booking._type;
                            chnlBooking.createDateTime = booking._createDateTime.ValueDate;
                            chnlBooking.source = booking._source;
                            chnlBooking.status = booking._status;
                            chnlBooking.confirmNumber = booking._confirmNumber;

                            //stroing special requests in channel booking table
                            var specialrequests = booking.SpecialRequests;
                            string code_sep = "";
                            string desc_sep = "";

                            chnlBooking.specialRequestCodes = "";
                            chnlBooking.specialRequests = "";

                            foreach (ChnlExpediaClasses.BookingRetrievalRS.Booking.SpecialRequest objRequest in specialrequests)
                            {
                                chnlBooking.specialRequestCodes += code_sep + objRequest._code;
                                code_sep = "|";

                                chnlBooking.specialRequests += code_sep + objRequest.description;
                                desc_sep = "|";
                            }
                            dcChnl.SubmitChanges();

                            if (chnlBooking.type == "Book" || chnlBooking.type == "Modify")
                            {
                                var chnlTb = dcChnl.RntChnlExpediaEstateTBL.FirstOrDefault(x => x.RoomTypeId == roomStay._roomTypeID);
                                if (chnlTb == null || string.IsNullOrEmpty(chnlTb.RoomTypeId))
                                {
                                    ErrorLog.addLog("", "ChnlExpediaImport.BookingRetrieval_process", "HotelId:" + HotelId + ", RoomTypeId:" + roomStay._roomTypeID + " was not found or not active");
                                    return;
                                }
                                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.FirstOrDefault(x => x.id == chnlTb.id);
                                if (currEstate == null)
                                {
                                    ErrorLog.addLog("", "ChnlExpediaImport.BookingRetrieval_process", "HotelId:" + HotelId + ", RoomTypeId:" + roomStay._roomTypeID + " was not found or not active");
                                    continue;
                                }
                                List<int> OtherRooms = new List<int>();
                                try
                                {
                                    OtherRooms = dcChnl.RntChnlExpediaEstateRoomRL.Where(x => x.RoomTypeId == roomStay._roomTypeID).Select(x => x.pidEstate).ToList();
                                }
                                catch (Exception ex)
                                {
                                    ErrorLog.addLog("", "ChnlExpediaImport.BookingRetrieval_process", "HotelId:" + HotelId + ", RoomTypeId:" + roomStay._roomTypeID + " RntChnlExpediaEstateRoomRL   ");
                                }



                                bool isNew = false;
                                bool isDatesChanged = false;

                                RNT_TBL_RESERVATION newRes = dcOld.RNT_TBL_RESERVATION.FirstOrDefault(x => x.id == chnlBooking.reservationId);
                                if (newRes == null)
                                {
                                    newRes = rntUtils.newReservation();
                                    newRes.pid_creator = 1;
                                    newRes.is_booking = 1;
                                    newRes.state_pid = 4;
                                    newRes.state_body = "";
                                    newRes.state_date = DateTime.Now;
                                    newRes.state_pid_user = 1;
                                    newRes.state_subject = "Instant booking online";
                                    rntUtils.rntReservation_setDefaults(ref newRes);
                                    dcOld.RNT_TBL_RESERVATION.InsertOnSubmit(newRes);
                                    dcOld.SubmitChanges();
                                    newRes.code = newRes.id.ToString().fillString("0", 7, false);
                                    chnlBooking.reservationId = newRes.id;
                                    dcOld.SubmitChanges();
                                    dcChnl.SubmitChanges();
                                    countCreated++;
                                    isNew = true;
                                }
                                else
                                {
                                    countUpdated++;
                                    if (newRes.state_pid == 3)
                                    {
                                        rntUtils.rntReservation_onStateChange(newRes);
                                        newRes.state_pid = 4;
                                        newRes.state_body = "Reopen Cancellation by channel";
                                        newRes.state_date = DateTime.Now;
                                        newRes.state_pid_user = 1;
                                        newRes.bcom_cancel = 0;
                                        isNew = true;
                                    }

                                    DateTime? dtStart = roomStay.stayDate._arrival.ValueDate;
                                    DateTime? dtEnd = roomStay.stayDate._departure.ValueDate;

                                    if (dtStart != newRes.dtStart || dtEnd != newRes.dtEnd)
                                        isDatesChanged = true;
                                }
                                string ccData = "";
                                if (roomStay.paymentCard != null && !string.IsNullOrEmpty(roomStay.paymentCard._cardNumber) && !string.IsNullOrEmpty(roomStay.paymentCard._cardCode))
                                {
                                    //for paymet wih credit card
                                    if (roomStay.paymentCard.cardHolder != null)
                                    {
                                        ccData += "\r\n Holder Name: " + roomStay.paymentCard.cardHolder._name;
                                        ccData += "\r\n Holder address: " + roomStay.paymentCard.cardHolder._address;
                                        ccData += "\r\n Holder city: " + roomStay.paymentCard.cardHolder._city;
                                        ccData += "\r\n Holder stateProv: " + roomStay.paymentCard.cardHolder._stateProv;
                                        ccData += "\r\n Holder country: " + roomStay.paymentCard.cardHolder._country;
                                        ccData += "\r\n Holder postalCode: " + roomStay.paymentCard.cardHolder._postalCode;
                                    }
                                    ccData += "\r\n Number: " + roomStay.paymentCard._cardNumber;
                                    ccData += "\r\n Type: " + roomStay.paymentCard._cardCodeDesc + " (" + roomStay.paymentCard._cardCode + ")";
                                    ccData += "\r\n CVC: " + roomStay.paymentCard._seriesCode;
                                    ccData += "\r\n Expire: " + roomStay.paymentCard._expireDate;
                                }
                                string filePath = Path.Combine(App.SRP, "admin/dati_cc");
                                if (!Directory.Exists(filePath))
                                    Directory.CreateDirectory(filePath);
                                filePath = Path.Combine(filePath, newRes.unique_id + ".txt");
                                if (!File.Exists(filePath) || ccData != "")
                                {
                                    try
                                    {
                                        StreamWriter ccWriter = new StreamWriter(filePath, false);
                                        ccWriter.WriteLine(RijndaelSimple_2.Encrypt(ccData)); // Write the file.
                                        ccWriter.Flush();
                                        ccWriter.Close(); // Close the instance of StreamWriter.
                                        ccWriter.Dispose(); // Dispose from memory.
                                    }
                                    catch (Exception exc)
                                    {
                                        ErrorLog.addLog("", "ChnlExpediaImport.BookingRetrieval_process.SavePaymentCard", exc.ToString());
                                    }
                                }

                                newRes.agentID = agentID;
                                newRes.pid_estate = currEstate.id;
                                newRes.pidEstateCity = currEstate.pid_city;

                                newRes.dtStart = roomStay.stayDate._arrival.ValueDate;
                                newRes.dtEnd = roomStay.stayDate._departure.ValueDate;
                                newRes.num_adult = roomStay.guestCount._adult;
                                newRes.num_child_over = roomStay.guestCount._child;
                                newRes.num_child_min = 0;
                                newRes.is_dtStartTimeChanged = 0;
                                newRes.is_dtEndTimeChanged = 0;

                                //newRes.agentCommissionNotInTotal = 0;
                                //newRes.requestFullPayAccepted = 1;
                                //newRes.requestFullPayAccepted = agentTbl.PartPayment.objToDecimal() == 100 || agentTbl.IsAllPayment == 1 ? 1 : 0;

                                newRes.password = CommonUtilities.CreatePassword(8, false, true, false);
                                var primaryGuest = booking.primaryGuest;
                                if (primaryGuest != null)
                                {
                                    newRes.cl_email = primaryGuest.email;
                                    if (primaryGuest.name != null)
                                    {
                                        newRes.cl_name_full = primaryGuest.name._givenName + " " + primaryGuest.name._middleName + " " + primaryGuest.name._surname;
                                    }
                                }
                                newRes.cl_name_honorific = "";
                                if (roomStay.paymentCard != null && roomStay.paymentCard.cardHolder != null)
                                    newRes.cl_loc_country = roomStay.paymentCard.cardHolder._country;
                                newRes.cl_pid_discount = -1;
                                newRes.cl_pid_lang = agentTbl.pidLang;
                                newRes.cl_isCompleted = 0;

                                var totalprice = roomStay.total._amountAfterTaxes.ValueDecimal.objToDecimal();

                                //only update prices if is not similar to previous expedia prices
                                if (newRes.bcom_room_price != totalprice)
                                {
                                    #region Price
                                    newRes.agentDiscountType = agentTbl.pidDiscountType.objToInt32();
                                    newRes.agentDiscountNotPayed = 0;
                                    newRes.requestFullPayAccepted = 1;
                                    if (newRes.agentDiscountType == 0) newRes.agentDiscountType = 1;

                                    #region agent commission change
                                    DateTime createdDate = DateTime.Now;
                                    if (newRes.dtCreation != null)
                                        createdDate = newRes.dtCreation.Value;
                                    decimal agentCommissionPrice = 0;
                                    RntAgentContractTBL AgentContractTBL = new RntAgentContractTBL();
                                    List<RntAgentContractPricesTBL> AgentContractPricesTBLs = new List<RntAgentContractPricesTBL>();

                                    var AgentContractRL = dc.RntEstateAgentContractRL.Where(x => x.pidEstate == currEstate.id && x.pidAgent == agentID).Select(x => x.pidAgentContract).ToList();
                                    AgentContractTBL = dc.RntAgentContractTBL.FirstOrDefault(x => x.dtStart <= createdDate && x.dtEnd >= createdDate && AgentContractRL.Contains(x.id) && x.contractType != null && x.contractType.ToLower() == "ppb");
                                    AgentContractPricesTBLs = dc.RntAgentContractPricesTBL.Where(x => x.dtEnd >= newRes.dtStart.Value && x.dtStart <= newRes.dtEnd.Value && AgentContractRL.Contains(x.pidAgentContract)).ToList();

                                    if (AgentContractTBL != null)
                                    {
                                        decimal agentCommissionPerc = AgentContractTBL.commissionAmount;
                                        var AgentContractPricesTBL = AgentContractPricesTBLs.FirstOrDefault(x => x.pidAgentContract == AgentContractTBL.id);
                                        if (AgentContractPricesTBL != null)
                                            agentCommissionPerc = AgentContractPricesTBL.commissionAmount;
                                        agentCommissionPrice = decimal.Divide(decimal.Multiply(totalprice, agentCommissionPerc), 100);
                                    }
                                    newRes.agentCommissionPrice = agentCommissionPrice;
                                    newRes.bcom_commissionamount = agentCommissionPrice;
                                    #endregion

                                    newRes.bcom_room_price = totalprice;
                                    var bcom_totalForOwner = totalprice - agentCommissionPrice;
                                    newRes.pr_paymentType = "cc";
                                    newRes.pr_total = totalprice;
                                    newRes.pr_part_payment_total = 0;
                                    newRes.pr_part_owner = totalprice;
                                    newRes.pr_part_modified = 1;
                                    newRes.bcom_totalForOwner = bcom_totalForOwner;
                                    newRes.pr_part_agency_fee = 0;
                                    newRes.prTotalRate = totalprice;
                                    newRes.prTotalCommission = currEstate.pr_percentage.objToInt32() * bcom_totalForOwner / 100;
                                    newRes.prTotalOwner = bcom_totalForOwner - newRes.prTotalCommission;
                                    newRes.pr_reservation = totalprice;
                                    newRes.pr_part_forPayment = newRes.pr_total;
                                    #endregion

                                }
                                dcOld.SubmitChanges();
                                long agentClientID = newRes.agentClientID.objToInt64();
                                var _client = dcAuth.AuthClientTBL.FirstOrDefault(x => x.id == agentClientID);
                                if (_client == null)
                                {
                                    _client = dcAuth.AuthClientTBL.FirstOrDefault(x => x.pidAgent == agentID && x.contactEmail == newRes.cl_email);
                                    if (_client == null || newRes.cl_email == "")
                                    {
                                        _client = new AuthClientTBL();
                                        _client.pidAgent = agentID;
                                        _client.uid = Guid.NewGuid();
                                        _client.createdDate = DateTime.Now;
                                        _client.createdUserID = 1;
                                        _client.createdUserNameFull = "System";
                                        _client.contactEmail = newRes.cl_email;
                                        dcAuth.AuthClientTBL.InsertOnSubmit(_client);
                                        dcAuth.SubmitChanges();
                                        _client.code = _client.id.ToString().fillString("0", 6, false);
                                        dcAuth.SubmitChanges();
                                    }
                                    agentClientID = _client.id;
                                }
                                _client.contactEmail = newRes.cl_email;
                                if (primaryGuest != null)
                                {
                                    if (primaryGuest.name != null)
                                    {
                                        _client.nameFirst = primaryGuest.name._givenName;
                                        _client.nameLast = primaryGuest.name._surname;
                                        _client.nameFull = _client.nameFirst + " " + _client.nameLast;
                                    }
                                    if (roomStay.paymentCard != null && roomStay.paymentCard.cardHolder != null)
                                    {
                                        _client.locAddress = roomStay.paymentCard.cardHolder._address;
                                        _client.locCity = roomStay.paymentCard.cardHolder._city;
                                        _client.locCountry = roomStay.paymentCard.cardHolder._country;
                                        _client.locZipCode = roomStay.paymentCard.cardHolder._postalCode;
                                    }
                                    if (primaryGuest.phone != null)
                                        _client.contactPhoneMobile = primaryGuest.phone._countryCode + " " + primaryGuest.phone._cityAreaCode + " " + primaryGuest.phone._extension + " " + primaryGuest.phone._number;
                                }
                                _client.isActive = 1;
                                dcAuth.SubmitChanges();
                                newRes.agentClientID = agentClientID;
                                dcOld.SubmitChanges();
                                if (isNew || isDatesChanged)
                                {
                                    rntUtils.rntReservation_onChange(newRes, false, false, false, true);
                                    lstResChangeEstateId.Add(newRes.pid_estate.objToInt32());
                                }
                                else
                                    rntUtils.rntReservation_onChange(newRes, false, false, false, false);


                                rntUtils.reservation_checkPartPayment(newRes, true);
                                var overBookingRes = rntUtils.rntEstate_isAvailable(currEstate.id, newRes.dtStart.Value, newRes.dtEnd.Value, newRes.id);
                                if (overBookingRes != null && newRes.pid_estate.objToInt32() > 0 && newRes.pid_estate.objToInt32() == currEstate.id && (isNew || isDatesChanged))
                                {
                                    string mSubject = "Attenzione - Prenotazione in OverBooking rif #" + newRes.code;
                                    string mBody = "Abbiamo importato la pren #" + newRes.code + " che risulta in overbooking con #" + overBookingRes.code + ".<br/>Si prega di verificare il motivo e conttare assistenza.";
                                    MailingUtilities.autoSendMailTo(mSubject, mBody, (agentTbl.contactEmail.isEmail() ? agentTbl.contactEmail : "info@rentalinrome.com"), true, "ExpediaImport_process overBookingRes");
                                }

                                if (isNew && (CommonUtilities.getSYS_SETTING("rntExpedia_sendEmails") == "true" || CommonUtilities.getSYS_SETTING("rntExpedia_sendEmails").ToInt32() == 1))
                                {
                                    rntUtils.rntReservation_mailPartPaymentReceive(newRes, false, true, true, true, true, 1); // send mails
                                }
                                ChnlExpediaUpdate.BookingConfirm_start(newRes.id);
                            }
                            else
                            {
                                RNT_TBL_RESERVATION objCancelRes = dcOld.RNT_TBL_RESERVATION.FirstOrDefault(x => x.id == chnlBooking.reservationId);
                                if (objCancelRes.state_pid != 3)
                                {
                                    rntUtils.rntReservation_onStateChange(objCancelRes);
                                    objCancelRes.state_pid = 3;
                                    objCancelRes.state_body = "Cancellato automaticamente dal sistema";
                                    objCancelRes.state_date = DateTime.Now;
                                    objCancelRes.state_pid_user = 1;
                                    objCancelRes.state_subject = "CAN";
                                    objCancelRes.bcom_cancel = 1;
                                    countCanceled++;
                                    dcOld.SubmitChanges();
                                    rntUtils.rntReservation_onChange(objCancelRes, false, false, false, true);
                                    lstResChangeEstateId.Add(objCancelRes.pid_estate.objToInt32());

                                    if ((CommonUtilities.getSYS_SETTING("rntExpedia_sendEmailsCancelled") == "true" || CommonUtilities.getSYS_SETTING("rntExpedia_sendEmailsCancelled").ToInt32() == 1))
                                    {
                                        rntUtils.rntReservation_mailCancelled(objCancelRes, false, true, false, false, true, 1); // send mails
                                    }
                                    ChnlExpediaUpdate.BookingConfirm_start(objCancelRes.id);
                                }
                            }

                            if (countCreated > 0 || countUpdated > 0 || countCanceled > 0)
                            {
                                var requestComments = "IMPORTED countCreated:" + countCreated + ", countUpdated:" + countUpdated + ", countCanceled:" + countCanceled + "";
                                ChnlExpediaUtils.addLog("BookingRetrieval", requestComments, "", "", "");
                            }

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            var jsonSerialiser = new JavaScriptSerializer();
                            var json = jsonSerialiser.Serialize(booking);
                            ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaImport.BookingRetrieval_process HotelId:" + HotelId, "Booking Ref " + json, ex.ToString(), "");
                        }
                    }
                    if (lstResChangeEstateId != null && lstResChangeEstateId.Count > 0)
                        rntUtilsChnlAll.updateExpediaHotels(lstResChangeEstateId);
                }
            }

            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaImport.BookingRetrieval_process HotelId:" + HotelId, "", ex.ToString(), "");

            }
        }

        public BookingRetrieval_process(int hotelId, int nbDaysInPast, bool backgroundProcess)
        {
            HotelId = hotelId;
            NbDaysInPast = nbDaysInPast;
            ErrorString = "";
            if (backgroundProcess)
            {
                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaImport.BookingRetrieval_process hotelId:" + hotelId);

                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();
            }
            else
                doThread();
        }
    }
    public static string BookingRetrieval_start(int hotelId, int nbDaysInPast)
    {
        BookingRetrieval_process _tmp = new BookingRetrieval_process(hotelId, nbDaysInPast, true);
        return _tmp.ErrorString;
    }
    public static void BookingRetrieval_all(int nbDaysInPast)
    {
        using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
        {
            var tmpList = dcChnl.RntChnlExpediaHotelTBL.Where(x => x.isActive == 1).ToList();
            foreach (var tmp in tmpList)
            {
                BookingRetrieval_start(tmp.HotelId, nbDaysInPast);
            }
        }
    }

    private class ProductRetrieval_process
    {
        public int HotelId { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    //var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia && x.is_integrate == 1);
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var hotelTbl = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.HotelId == HotelId);
                    if (hotelTbl == null)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "HotelId: " + HotelId + " NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    if (hotelTbl.isDemo == 1) requesUrl = "https://simulator.expediaquickconnect.com/connect/parr";
                    else requesUrl = "https://services.expediapartnercentral.com/eqc/parr";
                    //else requesUrl = "https://ws.expediaquickconnect.com/connect/parr";
                    var Request = new ChnlExpediaClasses.ProductAvailRateRetrievalRQ(hotelTbl);
                    Request.productRetrieval = new ChnlExpediaClasses.ProductAvailRateRetrievalRQ.ProductRetrieval();
                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequest(requesUrl, Request.GetXml(), out tmpErrorString);
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                        return;
                    var Response = new ChnlExpediaClasses.ProductAvailRateRetrievalRS(responseData);
                    if (Response.ProductLists.hotel != null)
                    {
                        hotelTbl.name = Response.ProductLists.hotel._name;
                        hotelTbl.city = Response.ProductLists.hotel._city;
                        dcChnl.SubmitChanges();
                    }
                    foreach (var roomType in Response.ProductLists.RoomTypes)
                    {
                        var roomTypeId = roomType._id;
                        var roomTypeTbl = dcChnl.RntChnlExpediaRoomTypeTBL.SingleOrDefault(x => x.id == roomTypeId);
                        if (roomTypeTbl == null)
                        {
                            roomTypeTbl = new RntChnlExpediaRoomTypeTBL();
                            roomTypeTbl.id = roomTypeId;
                            roomTypeTbl.HotelId = HotelId;
                            dcChnl.RntChnlExpediaRoomTypeTBL.InsertOnSubmit(roomTypeTbl);
                        }
                        roomTypeTbl.code = roomType._code;
                        roomTypeTbl.name = roomType._name;
                        roomTypeTbl.status = roomType._status;
                        roomTypeTbl.smokingPref = roomType._smokingPref;
                        roomTypeTbl.maxOccupants = roomType._maxOccupants;
                        dcChnl.SubmitChanges();
                        foreach (var bedType in roomType.BedTypes)
                        {
                            var bedTypeTbl = dcChnl.RntChnlExpediaRoomTypeBedTypeTBL.SingleOrDefault(x => x.RoomTypeId == roomTypeId && x.BedTypeId == bedType._id);
                            if (bedTypeTbl == null)
                            {
                                bedTypeTbl = new RntChnlExpediaRoomTypeBedTypeTBL();
                                bedTypeTbl.RoomTypeId = roomTypeId;
                                bedTypeTbl.BedTypeId = bedType._id;
                                dcChnl.RntChnlExpediaRoomTypeBedTypeTBL.InsertOnSubmit(bedTypeTbl);
                            }
                            bedTypeTbl.name = bedType._name;
                            dcChnl.SubmitChanges();
                        }
                        foreach (var occupancyByAge in roomType.OccupancyByAges)
                        {
                            var occupancyByAgeTbl = dcChnl.RntChnlExpediaRoomTypeOccupancyByAgeTBL.SingleOrDefault(x => x.RoomTypeId == roomTypeId && x.ageCategory == occupancyByAge._ageCategory);
                            if (occupancyByAgeTbl == null)
                            {
                                occupancyByAgeTbl = new RntChnlExpediaRoomTypeOccupancyByAgeTBL();
                                occupancyByAgeTbl.RoomTypeId = roomTypeId;
                                occupancyByAgeTbl.ageCategory = occupancyByAge._ageCategory;
                                dcChnl.RntChnlExpediaRoomTypeOccupancyByAgeTBL.InsertOnSubmit(occupancyByAgeTbl);
                            }
                            occupancyByAgeTbl.minAge = occupancyByAge._minAge;
                            occupancyByAgeTbl.maxOccupants = occupancyByAge._maxOccupants;
                            dcChnl.SubmitChanges();
                        }
                        foreach (var rateThreshold in roomType.RateThresholds)
                        {
                            var rateThresholdTbl = dcChnl.RntChnlExpediaRoomTypeRateThresholdTBL.SingleOrDefault(x => x.RoomTypeId == roomTypeId && x.type == rateThreshold._type);
                            if (rateThresholdTbl == null)
                            {
                                rateThresholdTbl = new RntChnlExpediaRoomTypeRateThresholdTBL();
                                rateThresholdTbl.RoomTypeId = roomTypeId;
                                rateThresholdTbl.type = rateThreshold._type;
                                dcChnl.RntChnlExpediaRoomTypeRateThresholdTBL.InsertOnSubmit(rateThresholdTbl);
                            }
                            rateThresholdTbl.minAmount = rateThreshold._minAmount.ValueDecimal.objToDecimal();
                            rateThresholdTbl.maxAmount = rateThreshold._maxAmount.ValueDecimal.objToDecimal();
                            rateThresholdTbl.source = rateThreshold._source;
                            dcChnl.SubmitChanges();
                        }
                        foreach (var ratePlan in roomType.RatePlans)
                        {
                            var ratePlanId = ratePlan._id;
                            var ratePlanTbl = dcChnl.RntChnlExpediaRoomTypeRatePlanTBL.SingleOrDefault(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId);
                            if (ratePlanTbl == null)
                            {
                                ratePlanTbl = new RntChnlExpediaRoomTypeRatePlanTBL();
                                ratePlanTbl.RoomTypeId = roomTypeId;
                                ratePlanTbl.RatePlanId = ratePlanId;
                                dcChnl.RntChnlExpediaRoomTypeRatePlanTBL.InsertOnSubmit(ratePlanTbl);
                            }
                            ratePlanTbl.code = ratePlan._code;
                            ratePlanTbl.name = ratePlan._name;
                            ratePlanTbl.status = ratePlan._status;
                            ratePlanTbl.type = ratePlan._type;
                            ratePlanTbl.distributionModel = ratePlan._distributionModel;
                            ratePlanTbl.rateAcquisitionType = ratePlan._rateAcquisitionType;
                            ratePlanTbl.parentId = ratePlan._parentId;
                            ratePlanTbl.rateLinkStart = ratePlan._rateLinkStart.ValueDate;
                            ratePlanTbl.rateLinkEnd = ratePlan._rateLinkEnd.ValueDate;
                            ratePlanTbl.isAvailStatusLinked = ratePlan._isAvailStatusLinked;
                            ratePlanTbl.areMinMaxLOSLinked = ratePlan._areMinMaxLOSLinked;
                            ratePlanTbl.isCTALinked = ratePlan._isCTALinked;
                            ratePlanTbl.isCTDLinked = ratePlan._isCTDLinked;
                            ratePlanTbl.rateLinkExceptions = ratePlan._rateLinkExceptions;
                            ratePlanTbl.pricingModel = ratePlan._pricingModel;
                            ratePlanTbl.occupantsForBaseRate = ratePlan._occupantsForBaseRate;
                            ratePlanTbl.depositRequired = ratePlan._depositRequired;
                            ratePlanTbl.minLOSDefault = ratePlan._minLOSDefault;
                            ratePlanTbl.maxLOSDefault = ratePlan._maxLOSDefault;
                            ratePlanTbl.minAdvBookDays = ratePlan._minAdvBookDays;
                            ratePlanTbl.maxAdvBookDays = ratePlan._maxAdvBookDays;
                            ratePlanTbl.bookDateStart = ratePlan._bookDateStart.ValueDate;
                            ratePlanTbl.bookDateEnd = ratePlan._bookDateEnd.ValueDate;
                            ratePlanTbl.travelDateStart = ratePlan._travelDateStart.ValueDate;
                            ratePlanTbl.travelDateEnd = ratePlan._travelDateEnd.ValueDate;
                            ratePlanTbl.mobileOnly = ratePlan._mobileOnly;
                            ratePlanTbl.createDateTime = ratePlan._createDateTime;
                            ratePlanTbl.updateDateTime = ratePlan._updateDateTime;
                            dcChnl.SubmitChanges();

                            if (dcChnl.RntChnlExpediaRoomTypeRatePlanLinkDefinitionTBL.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId).Count() > 0)
                            {
                                dcChnl.RntChnlExpediaRoomTypeRatePlanLinkDefinitionTBL.DeleteAllOnSubmit(dcChnl.RntChnlExpediaRoomTypeRatePlanLinkDefinitionTBL.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId));
                                dcChnl.SubmitChanges();
                            }
                            foreach (var ratePlanLinkDefinition in ratePlan.RatePlanLinkDefinitions)
                            {
                                var ratePlanLinkDefinitionTbl = new RntChnlExpediaRoomTypeRatePlanLinkDefinitionTBL();
                                ratePlanLinkDefinitionTbl.uid = Guid.NewGuid();
                                ratePlanLinkDefinitionTbl.RoomTypeId = roomTypeId;
                                ratePlanLinkDefinitionTbl.RatePlanId = ratePlanId;
                                ratePlanLinkDefinitionTbl.linkType = ratePlanLinkDefinition._linkType;
                                ratePlanLinkDefinitionTbl.linkValue = ratePlanLinkDefinition._linkValue.ValueDecimal.objToDecimal();
                                dcChnl.RntChnlExpediaRoomTypeRatePlanLinkDefinitionTBL.InsertOnSubmit(ratePlanLinkDefinitionTbl);
                                dcChnl.SubmitChanges();
                            }

                            if (dcChnl.RntChnlExpediaRoomTypeRatePlanDayOfWeekBookingRestrictionTBL.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId).Count() > 0)
                            {
                                dcChnl.RntChnlExpediaRoomTypeRatePlanDayOfWeekBookingRestrictionTBL.DeleteAllOnSubmit(dcChnl.RntChnlExpediaRoomTypeRatePlanDayOfWeekBookingRestrictionTBL.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId));
                                dcChnl.SubmitChanges();
                            }
                            foreach (var dayOfWeekBookingRestriction in ratePlan.DayOfWeekBookingRestrictions)
                            {
                                var dayOfWeekBookingRestrictionTbl = new RntChnlExpediaRoomTypeRatePlanDayOfWeekBookingRestrictionTBL();
                                dayOfWeekBookingRestrictionTbl.uid = Guid.NewGuid();
                                dayOfWeekBookingRestrictionTbl.RoomTypeId = roomTypeId;
                                dayOfWeekBookingRestrictionTbl.RatePlanId = ratePlanId;
                                dayOfWeekBookingRestrictionTbl.type = dayOfWeekBookingRestriction._type;
                                dayOfWeekBookingRestrictionTbl.sun = dayOfWeekBookingRestriction._sun;
                                dayOfWeekBookingRestrictionTbl.mon = dayOfWeekBookingRestriction._mon;
                                dayOfWeekBookingRestrictionTbl.tue = dayOfWeekBookingRestriction._tue;
                                dayOfWeekBookingRestrictionTbl.wed = dayOfWeekBookingRestriction._wed;
                                dayOfWeekBookingRestrictionTbl.thu = dayOfWeekBookingRestriction._thu;
                                dayOfWeekBookingRestrictionTbl.fri = dayOfWeekBookingRestriction._fri;
                                dayOfWeekBookingRestrictionTbl.sat = dayOfWeekBookingRestriction._sat;
                                dcChnl.RntChnlExpediaRoomTypeRatePlanDayOfWeekBookingRestrictionTBL.InsertOnSubmit(dayOfWeekBookingRestrictionTbl);
                                dcChnl.SubmitChanges();
                            }

                            if (dcChnl.RntChnlExpediaRoomTypeRatePlanCompensationTBL.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId).Count() > 0)
                            {
                                dcChnl.RntChnlExpediaRoomTypeRatePlanCompensationTBL.DeleteAllOnSubmit(dcChnl.RntChnlExpediaRoomTypeRatePlanCompensationTBL.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId));
                                dcChnl.SubmitChanges();
                            }
                            foreach (var compensation in ratePlan.Compensations)
                            {
                                var compensationTbl = new RntChnlExpediaRoomTypeRatePlanCompensationTBL();
                                compensationTbl.uid = Guid.NewGuid();
                                compensationTbl.RoomTypeId = roomTypeId;
                                compensationTbl.RatePlanId = ratePlanId;
                                compensationTbl.@default = compensation._default;
                                compensationTbl.percent = compensation._percent.ValueDecimal.objToDecimal();
                                compensationTbl.minAmount = compensation._minAmount.ValueDecimal;
                                compensationTbl.from = compensation._from.ValueDate;
                                compensationTbl.to = compensation._to.ValueDate;
                                compensationTbl.sun = compensation._sun;
                                compensationTbl.mon = compensation._mon;
                                compensationTbl.tue = compensation._tue;
                                compensationTbl.wed = compensation._wed;
                                compensationTbl.thu = compensation._thu;
                                compensationTbl.fri = compensation._fri;
                                compensationTbl.sat = compensation._sat;
                                dcChnl.RntChnlExpediaRoomTypeRatePlanCompensationTBL.InsertOnSubmit(compensationTbl);
                                dcChnl.SubmitChanges();
                            }

                            if (dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyTBL.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId).Count() > 0)
                            {
                                var cancelPolicyUids = dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyTBL.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId).Select(x => x.uid).ToList();
                                if (dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyPenaltyTBL.Where(x => cancelPolicyUids.Contains(x.CancelPolicyUid)).Count() > 0)
                                    dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyPenaltyTBL.DeleteAllOnSubmit(dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyPenaltyTBL.Where(x => cancelPolicyUids.Contains(x.CancelPolicyUid)));
                                dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyTBL.DeleteAllOnSubmit(dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyTBL.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId));
                                dcChnl.SubmitChanges();
                            }
                            foreach (var cancelPolicy in ratePlan.CancelPolicys)
                            {
                                var cancelPolicyTbl = new RntChnlExpediaRoomTypeRatePlanCancelPolicyTBL();
                                cancelPolicyTbl.uid = Guid.NewGuid();
                                cancelPolicyTbl.RoomTypeId = roomTypeId;
                                cancelPolicyTbl.RatePlanId = ratePlanId;
                                cancelPolicyTbl.@default = cancelPolicy._default;
                                cancelPolicyTbl.startDate = cancelPolicy._startDate.ValueDate;
                                cancelPolicyTbl.endDate = cancelPolicy._endDate.ValueDate;
                                cancelPolicyTbl.cancelWindow = cancelPolicy._cancelWindow;
                                cancelPolicyTbl.nonRefundable = cancelPolicy._nonRefundable;
                                cancelPolicyTbl.createDateTime = cancelPolicy._createDateTime;
                                cancelPolicyTbl.updateDateTime = cancelPolicy._updateDateTime;
                                dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyTBL.InsertOnSubmit(cancelPolicyTbl);
                                dcChnl.SubmitChanges();
                                foreach (var cancelPolicyPenalty in cancelPolicy.Penaltys)
                                {
                                    var cancelPolicyPenaltyTbl = new RntChnlExpediaRoomTypeRatePlanCancelPolicyPenaltyTBL();
                                    cancelPolicyPenaltyTbl.uid = Guid.NewGuid();
                                    cancelPolicyPenaltyTbl.CancelPolicyUid = cancelPolicyTbl.uid;
                                    cancelPolicyPenaltyTbl.insideWindow = cancelPolicyPenalty._insideWindow;
                                    cancelPolicyPenaltyTbl.flatFee = cancelPolicyPenalty._flatFee.ValueDecimal;
                                    cancelPolicyPenaltyTbl.perStayFee = cancelPolicyPenalty._perStayFee;
                                    dcChnl.RntChnlExpediaRoomTypeRatePlanCancelPolicyPenaltyTBL.InsertOnSubmit(cancelPolicyPenaltyTbl);
                                    dcChnl.SubmitChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaImport.ProductRetrieval_process HotelId:" + HotelId, "", ex.ToString(), "");

            }
        }

        public ProductRetrieval_process(int hotelId, bool backgroundProcess)
        {
            HotelId = hotelId;
            ErrorString = "";
            if (backgroundProcess)
            {
                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaImport.ProductRetrieval_process hotelId:" + hotelId);

                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();
            }
            else
                doThread();
        }
    }
    public static string ProductRetrieval_start(int hotelId)
    {
        ProductRetrieval_process _tmp = new ProductRetrieval_process(hotelId, false);
        return _tmp.ErrorString;
    }
    public static void ProductRetrieval_all()
    {
        using (DCchnlExpedia dcChnl = new DCchnlExpedia())
        {
            var tmpList = dcChnl.dbRntChnlExpediaHotelTBLs.Where(x => x.isActive == 1).ToList();
            foreach (var tmp in tmpList)
            {
                ProductRetrieval_start(tmp.HotelId);
            }
        }
    }

    private class AvailRateRetrieval_process
    {
        public int HotelId { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }

        void doThread()
        {
            try
            {
                using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    //var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia && x.is_integrate == 1);
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlExpediaProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var hotelTbl = dcChnl.RntChnlExpediaHotelTBL.SingleOrDefault(x => x.HotelId == HotelId);
                    if (hotelTbl == null)
                    {
                        ChnlExpediaUtils.addLog("ERROR", "HotelId: " + HotelId + " NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    if (hotelTbl.isDemo == 1) requesUrl = "https://simulator.expediaquickconnect.com/connect/parr";
                    else requesUrl = "https://services.expediapartnercentral.com/eqc/parr";
                    //else requesUrl = "https://ws.expediaquickconnect.com/connect/parr";

                    var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                    if (dtStart < DateTime.Now) dtStart = DateTime.Now.Date;
                    var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;

                    var Request = new ChnlExpediaClasses.ProductAvailRateRetrievalRQ(hotelTbl);

                    Request.availRateRetrieval = new ChnlExpediaClasses.ProductAvailRateRetrievalRQ.AvailRateRetrieval();
                    Request.availRateRetrieval._from.ValueDate = dtStart;
                    Request.availRateRetrieval._to.ValueDate = dtEnd;

                    string tmpErrorString = "";
                    var responseData = ChnlExpediaUtils.SendRequest(requesUrl, Request.GetXml(), out tmpErrorString);
                    //XmlDocument docXML = new XmlDocument();
                    //docXML.Load(App.SRP + "sample_avail_rate_xml1.xml");                   
                    //var responseData = docXML.OuterXml;
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                        return;
                    var Response = new ChnlExpediaClasses.ProductAvailRateRetrievalRS(responseData);

                    foreach (var availRate in Response.AvailRateLists.AvailRates)
                    {
                        DateTime date = availRate._date.ValueDate.Value;
                        foreach (var roomType in availRate.AvailRateRoomTypes)
                        {
                            int baseAllocation = 0;
                            int flexibleAllocation = 0;
                            int totalInventoryAvailable = 0;
                            int totalInventorySold = 0;

                            var inventory = roomType._Inventory;
                            if (inventory != null)
                            {
                                baseAllocation = inventory._baseAllocation;
                                flexibleAllocation = inventory._flexibleAllocation;
                                totalInventoryAvailable = inventory._totalInventoryAvailable;
                                totalInventorySold = inventory._totalInventorySold;

                                var availability = dcChnl.RntChnlExpediaAvailabilityTBL.SingleOrDefault(x => x.RoomTypeId == roomType._id && x.date == date);
                                if (availability == null)
                                {
                                    availability = new RntChnlExpediaAvailabilityTBL();
                                    availability.RoomTypeId = roomType._id;
                                    availability.date = date;
                                    dcChnl.RntChnlExpediaAvailabilityTBL.InsertOnSubmit(availability);
                                    dcChnl.SubmitChanges();
                                }
                                availability.baseAllocation = inventory._baseAllocation;
                                availability.flexibleAllocation = inventory._flexibleAllocation;
                                availability.totalInventoryAvailable = inventory._totalInventoryAvailable;
                                availability.totalInventorySold = inventory._totalInventorySold;
                                dcChnl.SubmitChanges();
                            }

                            foreach (var ratePlan in roomType.RatePlans)
                            {
                                string ratePlanid = ratePlan._id;
                                bool? closed = ratePlan._closed;
                                var rate = ratePlan._Rate;
                                string price = "";
                                string sep = "";
                                string currency = "";
                                if (rate != null)
                                {
                                    currency = rate._currency;
                                    foreach (var occupancy in rate.PerOccupancys)
                                    {
                                        price += sep + occupancy._occupancy + "-" + occupancy._rate.ValueDecimal;
                                        sep = "|";
                                    }
                                }

                                int? minLOS = 0;
                                int? maxLOS = 0;
                                bool? closedToArrival = false;
                                bool? closedToDeparture = false;
                                var restrictions = ratePlan._Restrictions;
                                if (restrictions != null)
                                {
                                    minLOS = restrictions._minLOS;
                                    maxLOS = restrictions._maxLOS;
                                    closedToArrival = restrictions._closedToArrival;
                                    closedToDeparture = restrictions._closedToDeparture;
                                }

                                var rateM = dcChnl.RntChnlExpediaRatesTBL.SingleOrDefault(x => x.date == date && x.RatePlanId == ratePlan._id && x.RoomTypeId == roomType._id);
                                if (rateM == null)
                                {
                                    rateM = new RntChnlExpediaRatesTBL();
                                    rateM.RatePlanId = ratePlan._id;
                                    rateM.RoomTypeId = roomType._id;
                                    rateM.date = date;
                                    dcChnl.RntChnlExpediaRatesTBL.InsertOnSubmit(rateM);
                                    dcChnl.SubmitChanges();
                                }
                                rateM.currency = currency;
                                rateM.minLos = minLOS;
                                rateM.maxLos = maxLOS;
                                rateM.isClosed = closed;
                                rateM.isClosedOnArrival = closedToArrival;
                                rateM.isClosedOnDeparture = closedToDeparture;
                                rateM.price = price;
                                dcChnl.SubmitChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaImport.AvailRateRetrieval_process HotelId:" + HotelId, "", ex.ToString(), "");

            }
        }

        public AvailRateRetrieval_process(int hotelId, bool backgroundProcess)
        {
            HotelId = hotelId;
            ErrorString = "";
            if (backgroundProcess)
            {
                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();

                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaImport.AvailRateRetrieval_process hotelId:" + hotelId);    
            }
            else
                doThread();
        }
        public AvailRateRetrieval_process(int hotelId, DateTime? dtStart, DateTime? dtEnd, bool backgroundProcess)
        {
            HotelId = hotelId;
            DtStart = dtStart;
            DtEnd = dtEnd;
            ErrorString = "";
            if (backgroundProcess)
            {
                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlExpediaImport.ProductRetrieval_process hotelId:" + hotelId);

                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();
            }
            else
                doThread();
        }
    }
    public static string AvailRateRetrieval_start(int hotelId, DateTime? dtStart, DateTime? dtEnd)
    {
        if (dtStart != null && dtEnd != null)
        {
            AvailRateRetrieval_process _tmp = new AvailRateRetrieval_process(hotelId, dtStart, dtEnd, false);
            return _tmp.ErrorString;
        }
        else
        {
            AvailRateRetrieval_process _tmp = new AvailRateRetrieval_process(hotelId, false);
            return _tmp.ErrorString;
        }
    }

    public static void AvailRateRetrieval_all(DateTime? dtStart, DateTime? dtEnd)
    {
        using (magaChnlExpediaDataContext dcChnl = maga_DataContext.DC_EXP)
        {
            var tmpList = dcChnl.RntChnlExpediaHotelTBL.Where(x => x.isActive == 1).ToList();
            foreach (var tmp in tmpList)
            {
                AvailRateRetrieval_start(tmp.HotelId, dtStart, dtEnd);
            }
        }
    }

}
public class ChnlExpediaClasses
{
    #region COMMON
    public class Authentication
    {
        public string _username { get; set; }
        public string _password { get; set; }
        public Authentication(RntChnlExpediaHotelTBL hotelTbl)
        {
            _username = hotelTbl.username;
            _password = hotelTbl.password;
        }
        public XElement GetElement(XNamespace ns)
        {
            XElement elm = new XElement(ns + "Authentication");
            elm.Add(new XAttribute("username", _username));
            elm.Add(new XAttribute("password", _password));
            return elm;
        }
    }
    public class Hotel
    {
        public int _id { get; set; }
        public string _name { get; set; }
        public string _city { get; set; }
        public Hotel(RntChnlExpediaHotelTBL hotelTbl)
        {
            _id = hotelTbl.HotelId;
        }
        public Hotel(XElement elm)
        {
            if (elm != null)
            {
                _id = elm.Attribute("id").objToInt32();
                _name = (string)elm.Attribute("name") ?? "";
                _city = (string)elm.Attribute("city") ?? "";
            }
        }
        public XElement GetElement(XNamespace ns)
        {
            XElement elm = new XElement(ns + "Hotel");
            elm.Add(new XAttribute("id", _id));
            return elm;
        }
    }
    public class DateRangeClass
    {
        public Date _from { get; set; }
        public Date _to { get; set; }
        public DateRangeClass()
        {
            _from = new Date();
            _from.ValueDate = DateTime.Now;
            _to = new Date();
            _to.ValueDate = DateTime.Now.AddDays(1);
        }
        public XElement GetElement(XNamespace ns)
        {
            XElement elm = new XElement(ns + "DateRange");
            elm.Add(new XAttribute("from", _from.Value));
            elm.Add(new XAttribute("to", _to.Value));
            return elm;
        }
    }
    public class Date
    {
        public string Value { get { return ValueDate.HasValue ? ValueDate.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") : ""; } set { ValueDate = value + "" != "" ? (value + "").Replace("-", "").JSCal_stringToDate() : (DateTime?)null; } }
        public DateTime? ValueDate { get; set; }
        public Date()
        {
        }
    }
    public class DateAndTime
    {
        public string Value { get { return ValueDate.HasValue ? ValueDate.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "T" + ValueDate.Value.TimeOfDay.JSTime_toString(true, true) + "Z" : ""; } set { ValueDate = value + "" != "" ? Convert.ToDateTime(value) : (DateTime?)null; } }
        public DateTime? ValueDate { get; set; }
        public DateAndTime()
        {
        }
        public XElement GetElement(string name, XNamespace ns)
        {
            XElement record = new XElement(ns + name);
            record.Value = Value;
            return record;
        }
    }
    public class DecimalUnit
    {
        //public string Value { get { return ValueDecimal.HasValue ? (ValueDecimal + "") : ""; } set { ValueDecimal = (value + "") != "" ? (value + "").ToDecimal() : (decimal?)null; } }
        public string Value { get { return ValueDecimal.HasValue ? (ValueDecimal + "").Replace(",", ".") : ""; } set { ValueDecimal = (value + "") != "" ? (value + "").Replace(",", ".").ToDecimal() : (decimal?)null; } }
        public decimal? ValueDecimal { get; set; }
        public DecimalUnit()
        {
        }
        public XElement GetElement(string name, XNamespace ns)
        {
            XElement record = new XElement(ns + name);
            record.Value = Value;
            return record;
        }
    }
    #endregion
    #region PARR
    public class ProductAvailRateRetrievalRQ
    {
        private Authentication authentication { get; set; }
        private Hotel hotel { get; set; }
        public ProductRetrieval productRetrieval { get; set; }
        public AvailRateRetrieval availRateRetrieval { get; set; }
        public ProductAvailRateRetrievalRQ(RntChnlExpediaHotelTBL hotelTbl)
        {
            authentication = new Authentication(hotelTbl);
            hotel = new Hotel(hotelTbl);
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XNamespace ns = "http://www.expediaconnect.com/EQC/PAR/2013/07";
            XElement root = new XElement(ns + "ProductAvailRateRetrievalRQ");
            root.Add(authentication.GetElement(ns));
            root.Add(hotel.GetElement(ns));
            var ParamSet = new XElement(ns + "ParamSet");
            if (productRetrieval != null)
                ParamSet.Add(productRetrieval.GetElement(ns));
            if (availRateRetrieval != null)
                ParamSet.Add(availRateRetrieval.GetElement(ns));
            root.Add(ParamSet);
            _resource.Add(root);
            return _resource.ToString();
        }
        public class ProductRetrieval
        {
            public string _productStatus { get; set; } // Active, Inactive, All
            public bool _returnRateLink { get; set; }
            public bool _returnRoomAttributes { get; set; }
            public bool _returnRatePlanAttributes { get; set; }
            public bool _returnCompensation { get; set; }
            public bool _returnCancelPolicy { get; set; }
            public ProductRetrieval()
            {
                _productStatus = "All";
                _returnRateLink = true;
                _returnRoomAttributes = true;
                _returnRatePlanAttributes = true;
                _returnCompensation = true;
                _returnCancelPolicy = true;
            }
            public XElement GetElement(XNamespace ns)
            {
                XElement elm = new XElement(ns + "ProductRetrieval");
                elm.Add(new XAttribute("productStatus", _productStatus));
                elm.Add(new XAttribute("returnRateLink", _returnRateLink == true ? "true" : "false"));
                elm.Add(new XAttribute("returnRoomAttributes", _returnRoomAttributes == true ? "true" : "false"));
                elm.Add(new XAttribute("returnRatePlanAttributes", _returnRatePlanAttributes == true ? "true" : "false"));
                elm.Add(new XAttribute("returnCompensation", _returnCompensation == true ? "true" : "false"));
                elm.Add(new XAttribute("returnCancelPolicy", _returnCancelPolicy == true ? "true" : "false"));
                return elm;
            }
        }
        public class AvailRateRetrieval
        {
            public Date _from { get; set; }
            public Date _to { get; set; }
            public bool _inventory { get; set; }
            public bool _roomAvailStatus { get; set; }
            public bool _rateAvailStatus { get; set; }
            public bool _restriction { get; set; }
            public bool _rates { get; set; }
            public AvailRateRetrieval()
            {
                _from = new Date();
                _from.ValueDate = DateTime.Now;
                _to = new Date();
                _to.ValueDate = DateTime.Now.AddYears(2);
                _inventory = true;
                _roomAvailStatus = true;
                _rateAvailStatus = true;
                _restriction = true;
                _rates = true;
            }
            public XElement GetElement(XNamespace ns)
            {
                XElement elm = new XElement(ns + "AvailRateRetrieval");
                elm.Add(new XAttribute("from", _from.Value));
                elm.Add(new XAttribute("to", _to.Value));
                elm.Add(new XAttribute("inventory", _inventory == true ? "true" : "false"));
                elm.Add(new XAttribute("roomAvailStatus", _roomAvailStatus == true ? "true" : "false"));
                elm.Add(new XAttribute("rateAvailStatus", _rateAvailStatus == true ? "true" : "false"));
                elm.Add(new XAttribute("restrictions", _restriction == true ? "true" : "false"));
                elm.Add(new XAttribute("rates", _rates == true ? "true" : "false"));
                return elm;
            }
        }
    }
    public class ProductAvailRateRetrievalRS
    {
        public ProductList ProductLists { get; set; }
        public AvailRateList AvailRateLists { get; set; }
        public ProductAvailRateRetrievalRS(string xmlData)
        {
            xmlData = xmlData.Replace("xmlns=", "tmp=");
            XDocument _resource = XDocument.Parse(xmlData);
            var ds = _resource.Element("ProductAvailRateRetrievalRS");
            if (ds.Element("ProductList") != null) ProductLists = new ProductList(ds.Element("ProductList"));
            if (ds.Element("AvailRateList") != null) AvailRateLists = new AvailRateList(ds.Element("AvailRateList"));
        }
        public class ProductList
        {
            public Hotel hotel { get; set; }
            public List<RoomType> RoomTypes { get; set; }
            public ProductList(XElement elm)
            {
                RoomTypes = new List<RoomType>();
                if (elm != null)
                {
                    if (elm.Element("Hotel") != null) hotel = new Hotel(elm.Element("Hotel"));
                    foreach (var tmp in elm.Descendants("RoomType"))
                        RoomTypes.Add(new RoomType(tmp));
                }
            }
        }
        public class RoomType
        {
            public string _id { get; set; }
            public string _code { get; set; }
            public string _name { get; set; }
            public int _status { get; set; }
            public string _smokingPref { get; set; }
            public int _maxOccupants { get; set; }
            public List<BedType> BedTypes { get; set; }
            public List<OccupancyByAge> OccupancyByAges { get; set; }
            public List<RateThreshold> RateThresholds { get; set; }
            public List<RatePlan> RatePlans { get; set; }
            public RoomType(XElement elm)
            {
                BedTypes = new List<BedType>();
                OccupancyByAges = new List<OccupancyByAge>();
                RateThresholds = new List<RateThreshold>();
                RatePlans = new List<RatePlan>();
                if (elm != null)
                {
                    _id = (string)elm.Attribute("id") ?? "";
                    _code = (string)elm.Attribute("code") ?? "";
                    _name = (string)elm.Attribute("name") ?? "";
                    _status = ((string)elm.Attribute("status")).ToLower() == "active" ? 1 : 0;
                    _smokingPref = (string)elm.Attribute("smokingPref") ?? "";
                    _maxOccupants = elm.Attribute("maxOccupants").objToInt32();
                    foreach (var tmp in elm.Descendants("BedType"))
                        BedTypes.Add(new BedType(tmp));
                    foreach (var tmp in elm.Descendants("OccupancyByAge"))
                        OccupancyByAges.Add(new OccupancyByAge(tmp));
                    foreach (var tmp in elm.Descendants("RateThreshold"))
                        RateThresholds.Add(new RateThreshold(tmp));
                    foreach (var tmp in elm.Descendants("RatePlan"))
                        RatePlans.Add(new RatePlan(tmp));
                }
            }
        }
        public class BedType
        {
            public string _id { get; set; }
            public string _name { get; set; }
            public BedType(XElement elm)
            {
                if (elm != null)
                {
                    _id = (string)elm.Attribute("id") ?? "";
                    _name = (string)elm.Attribute("name") ?? "";
                }
            }
        }
        public class OccupancyByAge
        {
            public string _ageCategory { get; set; } // Adult, ChildAgeA, ChildAgeB, ChildAgeC, ChildAgeD or Infant.
            public int _minAge { get; set; }
            public int _maxOccupants { get; set; }
            public OccupancyByAge(XElement elm)
            {
                if (elm != null)
                {
                    _ageCategory = (string)elm.Attribute("ageCategory") ?? "";
                    _minAge = ((string)elm.Attribute("minAge")).objToInt32();
                    _maxOccupants = ((string)elm.Attribute("maxOccupants")).objToInt32();
                }
            }
        }
        public class RateThreshold
        {
            /// <summary>
            /// Possible values are:
            ///- NetRate: returned for ExpediaCollect-only and ETP hotels
            ///- SellRate: returned for HotelCollect-only and ETP hotels
            /// </summary>
            public string _type { get; set; }
            public DecimalUnit _minAmount { get; set; }
            public DecimalUnit _maxAmount { get; set; }
            /// <summary>
            /// Defines how minAmount and maxAmount were calculated. 2 possible values:
            ///- RecentReservations: thresholds calculated using last 10 reservations, and applying multiplication and division factor to find max and min values.
            ///- Manual: manually defined by Expedia
            /// RecentReservation is Expedia’s default method.
            /// </summary>
            public string _source { get; set; }
            public RateThreshold(XElement elm)
            {
                _minAmount = new DecimalUnit();
                _maxAmount = new DecimalUnit();
                if (elm != null)
                {
                    _type = (string)elm.Attribute("type") ?? "";
                    _minAmount.Value = ((string)elm.Attribute("minAmount")) ?? "";
                    _maxAmount.Value = ((string)elm.Attribute("maxAmount")) ?? "";
                    //_minAmount.ValueDecimal = ((string)elm.Attribute("minAmount")).objToDecimal();
                    //_maxAmount.ValueDecimal = ((string)elm.Attribute("maxAmount")).objToDecimal();
                    _source = (string)elm.Attribute("source") ?? "";
                }
            }
        }
        public class RatePlan
        {
            public string _id { get; set; }
            public string _code { get; set; }
            public string _name { get; set; }
            public int _status { get; set; }
            public string _type { get; set; } // Standalone, Package, Corporate.
            public string _distributionModel { get; set; } // ExpediaCollect, HotelCollect.
            public string _rateAcquisitionType { get; set; } // NetRate, LowestAvailableRate, SellRate, Derived, Linked
            public string _parentId { get; set; }
            public Date _rateLinkStart { get; set; }
            public Date _rateLinkEnd { get; set; }
            public bool? _isAvailStatusLinked { get; set; }
            public bool? _areMinMaxLOSLinked { get; set; }
            public bool? _isCTALinked { get; set; }
            public bool? _isCTDLinked { get; set; }
            public bool? _rateLinkExceptions { get; set; }
            /// <summary>
            /// Rate plan’s pricing model. Will be identical across all rate plans unless property is undergoing pricing model conversion. Possible values are:
            /// - PerDayPricing
            /// - PerPersonPricing
            /// - OccupancyBasedPricing
            /// - PerDayPricingByDayOfArrival
            /// - PerDayPricingByLengthOfStay
            /// - PerPersonPricingByDayOfArrival
            /// - OccupancyBasedPricingByDayOfArrival
            /// </summary>
            public string _pricingModel { get; set; }
            public int? _occupantsForBaseRate { get; set; }
            public bool? _depositRequired { get; set; }
            public int? _minLOSDefault { get; set; }
            public int? _maxLOSDefault { get; set; }
            public int? _minAdvBookDays { get; set; }
            public int? _maxAdvBookDays { get; set; }
            public Date _bookDateStart { get; set; }
            public Date _bookDateEnd { get; set; }
            public Date _travelDateStart { get; set; }
            public Date _travelDateEnd { get; set; }
            public bool? _mobileOnly { get; set; }
            public string _createDateTime { get; set; }
            public string _updateDateTime { get; set; }
            public List<RatePlanLinkDefinition> RatePlanLinkDefinitions { get; set; }
            public List<DayOfWeekBookingRestriction> DayOfWeekBookingRestrictions { get; set; }
            public List<Compensation> Compensations { get; set; }
            public List<CancelPolicy> CancelPolicys { get; set; }
            public RatePlan(XElement elm)
            {
                _rateLinkStart = new Date();
                _rateLinkEnd = new Date();
                RatePlanLinkDefinitions = new List<RatePlanLinkDefinition>();
                _bookDateStart = new Date();
                _bookDateEnd = new Date();
                _travelDateStart = new Date();
                _travelDateEnd = new Date();
                DayOfWeekBookingRestrictions = new List<DayOfWeekBookingRestriction>();
                Compensations = new List<Compensation>();
                CancelPolicys = new List<CancelPolicy>();
                if (elm != null)
                {
                    _id = (string)elm.Attribute("id") ?? "";
                    _code = (string)elm.Attribute("code") ?? "";
                    _name = (string)elm.Attribute("name") ?? "";
                    _status = ((string)elm.Attribute("status")).ToLower() == "active" ? 1 : 0;
                    _type = (string)elm.Attribute("type") ?? "";
                    _distributionModel = (string)elm.Attribute("distributionModel") ?? "";
                    _rateAcquisitionType = (string)elm.Attribute("rateAcquisitionType") ?? "";
                    _parentId = (string)elm.Attribute("parentId") ?? "";
                    _rateLinkStart.Value = (string)elm.Attribute("rateLinkStart") ?? "";
                    _rateLinkEnd.Value = (string)elm.Attribute("rateLinkEnd") ?? "";
                    _isAvailStatusLinked = (elm.Attribute("isAvailStatusLinked") != null ? (elm.Attribute("isAvailStatusLinked") + "").ToLower() == "true" : (bool?)null);
                    _areMinMaxLOSLinked = (elm.Attribute("areMinMaxLOSLinked") != null ? (elm.Attribute("areMinMaxLOSLinked") + "").ToLower() == "true" : (bool?)null);
                    _isCTALinked = (elm.Attribute("isCTALinked") != null ? (elm.Attribute("isCTALinked") + "").ToLower() == "true" : (bool?)null);
                    _isCTDLinked = (elm.Attribute("isCTDLinked") != null ? (elm.Attribute("isCTDLinked") + "").ToLower() == "true" : (bool?)null);
                    _rateLinkExceptions = (elm.Attribute("rateLinkExceptions") != null ? (elm.Attribute("rateLinkExceptions") + "").ToLower() == "true" : (bool?)null);
                    _pricingModel = (string)elm.Attribute("pricingModel") ?? "";
                    _occupantsForBaseRate = (elm.Attribute("occupantsForBaseRate") != null ? ((string)elm.Attribute("occupantsForBaseRate")).objToInt32() : (int?)null);
                    _depositRequired = (elm.Attribute("depositRequired") != null ? (elm.Attribute("depositRequired") + "").ToLower() == "true" : (bool?)null);
                    _minLOSDefault = (elm.Attribute("minLOSDefault") != null ? ((string)elm.Attribute("minLOSDefault")).objToInt32() : (int?)null);
                    _maxLOSDefault = (elm.Attribute("maxLOSDefault") != null ? ((string)elm.Attribute("maxLOSDefault")).objToInt32() : (int?)null);
                    _minAdvBookDays = (elm.Attribute("minAdvBookDays") != null ? ((string)elm.Attribute("minAdvBookDays")).objToInt32() : (int?)null);
                    _maxAdvBookDays = (elm.Attribute("maxAdvBookDays") != null ? ((string)elm.Attribute("maxAdvBookDays")).objToInt32() : (int?)null);
                    _bookDateStart.Value = (string)elm.Attribute("bookDateStart") ?? "";
                    _bookDateEnd.Value = (string)elm.Attribute("bookDateEnd") ?? "";
                    _travelDateStart.Value = (string)elm.Attribute("travelDateStart") ?? "";
                    _travelDateEnd.Value = (string)elm.Attribute("travelDateEnd") ?? "";
                    _mobileOnly = (elm.Attribute("mobileOnly") != null ? (elm.Attribute("mobileOnly") + "").ToLower() == "true" : (bool?)null);
                    _createDateTime = (string)elm.Attribute("createDateTime") ?? "";
                    _updateDateTime = (string)elm.Attribute("updateDateTime") ?? "";
                    foreach (var tmp in elm.Descendants("RatePlanLinkDefinition"))
                        RatePlanLinkDefinitions.Add(new RatePlanLinkDefinition(tmp));
                    foreach (var tmp in elm.Descendants("DayOfWeekBookingRestriction"))
                        DayOfWeekBookingRestrictions.Add(new DayOfWeekBookingRestriction(tmp));
                    foreach (var tmp in elm.Descendants("Compensation"))
                        Compensations.Add(new Compensation(tmp));
                    foreach (var tmp in elm.Descendants("CancelPolicy"))
                        CancelPolicys.Add(new CancelPolicy(tmp));
                }
            }
        }
        public class RatePlanLinkDefinition
        {
            public string _linkType { get; set; } // Percent and Amount.
            public DecimalUnit _linkValue { get; set; }
            public RatePlanLinkDefinition(XElement elm)
            {
                _linkValue = new DecimalUnit();
                if (elm != null)
                {
                    _linkType = (string)elm.Attribute("linkType") ?? "";
                    _linkValue.Value = ((string)elm.Attribute("linkValue")) ?? "";
                    // _linkValue.ValueDecimal = ((string)elm.Attribute("linkValue")).objToDecimal();
                }
            }
        }
        /// <summary>
        /// Defines if people looking for this rate must meet certain DOW booking pattern restrictions. Type can be one of 4 (defined below).
        /// By default, for rates with none of these restrictions, this element is not returned at all. 
        /// It will only be returned if there is a specific restriction set that would prevent people for specific checkin or checkout DOW. 
        /// A rate plan can have up to 1 of each type returned (4 occurences of this element).
        /// </summary>
        public class DayOfWeekBookingRestriction
        {
            /// <summary>
            /// Type of restriction. Can take one of 4 values:
            /// - StartOn: People looking for this rate must checkin on these selected day(s) of week.
            /// - EndOn: People looking for this rate must checkout on these selected day(s) of week.
            /// - IncludeOneOf: People looking for this rate must include at least one of these selected day(s) of week.
            /// - IncludeAll: People looking for this rate must include all these selected day(s) of week.
            /// </summary>
            public string _type { get; set; } // Percent and Amount.
            public bool _sun { get; set; }
            public bool _mon { get; set; }
            public bool _tue { get; set; }
            public bool _wed { get; set; }
            public bool _thu { get; set; }
            public bool _fri { get; set; }
            public bool _sat { get; set; }
            public DayOfWeekBookingRestriction(XElement elm)
            {
                if (elm != null)
                {
                    _type = (string)elm.Attribute("type") ?? "";
                    _sun = (elm.Attribute("sun") + "").ToLower() == "true";
                    _mon = (elm.Attribute("mon") + "").ToLower() == "true";
                    _tue = (elm.Attribute("tue") + "").ToLower() == "true";
                    _wed = (elm.Attribute("wed") + "").ToLower() == "true";
                    _thu = (elm.Attribute("thu") + "").ToLower() == "true";
                    _fri = (elm.Attribute("fri") + "").ToLower() == "true";
                    _sat = (elm.Attribute("sat") + "").ToLower() == "true";
                }
            }
        }
        public class Compensation
        {
            public bool _default { get; set; }
            public DecimalUnit _percent { get; set; }
            public DecimalUnit _minAmount { get; set; }
            public Date _from { get; set; }
            public Date _to { get; set; }
            public bool? _sun { get; set; }
            public bool? _mon { get; set; }
            public bool? _tue { get; set; }
            public bool? _wed { get; set; }
            public bool? _thu { get; set; }
            public bool? _fri { get; set; }
            public bool? _sat { get; set; }
            public Compensation(XElement elm)
            {
                _percent = new DecimalUnit();
                _minAmount = new DecimalUnit();
                _from = new Date();
                _to = new Date();
                if (elm != null)
                {
                    _default = (elm.Attribute("default") + "").ToLower() == "true";
                    _percent.Value = ((string)elm.Attribute("percent")) ?? "";
                    //_percent.ValueDecimal = ((string)elm.Attribute("percent")).objToDecimal();
                    if (elm.Attribute("minAmount") != null)
                        _minAmount.Value = ((string)elm.Attribute("minAmount")) ?? "";
                    // _minAmount.ValueDecimal = ((string)elm.Attribute("minAmount")).objToDecimal();
                    if (elm.Attribute("from") != null)
                        _from.Value = (string)elm.Attribute("from") ?? "";
                    if (elm.Attribute("to") != null)
                        _to.Value = (string)elm.Attribute("to") ?? "";
                    if (elm.Attribute("sun") != null)
                        _sun = (elm.Attribute("sun") + "").ToLower() == "true";
                    if (elm.Attribute("mon") != null)
                        _mon = (elm.Attribute("mon") + "").ToLower() == "true";
                    if (elm.Attribute("tue") != null)
                        _tue = (elm.Attribute("tue") + "").ToLower() == "true";
                    if (elm.Attribute("wed") != null)
                        _wed = (elm.Attribute("wed") + "").ToLower() == "true";
                    if (elm.Attribute("thu") != null)
                        _thu = (elm.Attribute("thu") + "").ToLower() == "true";
                    if (elm.Attribute("fri") != null)
                        _fri = (elm.Attribute("fri") + "").ToLower() == "true";
                    if (elm.Attribute("sat") != null)
                        _sat = (elm.Attribute("sat") + "").ToLower() == "true";
                }
            }
        }
        public class CancelPolicy
        {
            public bool _default { get; set; }
            public Date _startDate { get; set; }
            public Date _endDate { get; set; }
            public int? _cancelWindow { get; set; }
            public bool? _nonRefundable { get; set; }
            public string _createDateTime { get; set; }
            public string _updateDateTime { get; set; }
            public List<Penalty> Penaltys { get; set; }
            public CancelPolicy(XElement elm)
            {
                _startDate = new Date();
                _endDate = new Date();
                Penaltys = new List<Penalty>();
                if (elm != null)
                {
                    _default = (elm.Attribute("default") + "").ToLower() == "true";
                    if (elm.Attribute("startDate") != null)
                        _startDate.Value = (string)elm.Attribute("startDate") ?? "";
                    if (elm.Attribute("endDate") != null)
                        _endDate.Value = (string)elm.Attribute("endDate") ?? "";
                    if (elm.Attribute("cancelWindow") != null)
                        _cancelWindow = ((string)elm.Attribute("cancelWindow")).objToInt32();
                    if (elm.Attribute("nonRefundable") != null)
                        _nonRefundable = (elm.Attribute("nonRefundable") + "").ToLower() == "true";
                    _createDateTime = (string)elm.Attribute("createDateTime") ?? "";
                    _updateDateTime = (string)elm.Attribute("updateDateTime") ?? "";
                    foreach (var tmp in elm.Descendants("Penalty"))
                        Penaltys.Add(new Penalty(tmp));
                }
            }
            public class Penalty
            {
                public bool _insideWindow { get; set; }
                public DecimalUnit _flatFee { get; set; }
                /// <summary>
                /// The penalty charged relative to the cost of the stay. Possible values include:
                /// - None
                /// - 1stNightRoomAndTax
                /// - 2ndNightsRoomAndTax
                /// - 10PercentCostOfStay
                /// - 20PercentCostOfStay
                /// - 30PercentCostOfStay
                /// - 40PercentCostOfStay
                /// - 50PercentCostOfStay
                /// - 60PercentCostOfStay
                /// - 70PercentCostOfStay
                /// - 80PercentCostOfStay
                /// - 90PercentCostOfStay
                /// - FullCostOfStay
                /// </summary>
                public string _perStayFee { get; set; }
                public Penalty(XElement elm)
                {
                    _flatFee = new DecimalUnit();
                    if (elm != null)
                    {
                        _insideWindow = (elm.Attribute("insideWindow") + "").ToLower() == "true";
                        _flatFee.Value = ((string)elm.Attribute("flatFee")) ?? "";
                        // _flatFee.ValueDecimal = ((string)elm.Attribute("flatFee")).objToDecimal();
                        _perStayFee = (string)elm.Attribute("perStayFee") ?? "";
                    }
                }
            }
        }
        public class AvailRateList
        {
            public Hotel hotel { get; set; }
            public List<AvailRate> AvailRates { get; set; }
            public AvailRateList(XElement elm)
            {
                AvailRates = new List<AvailRate>();
                if (elm != null)
                {
                    if (elm.Element("Hotel") != null) hotel = new Hotel(elm.Element("Hotel"));
                    foreach (var tmp in elm.Descendants("AvailRate"))
                        AvailRates.Add(new AvailRate(tmp));
                }
            }
        }
        public class AvailRate
        {
            public Date _date { get; set; }
            public List<AvailRateRoomType> AvailRateRoomTypes { get; set; }
            public AvailRate(XElement elm)
            {
                AvailRateRoomTypes = new List<AvailRateRoomType>();
                if (elm != null)
                {
                    _date = new Date();
                    if (elm != null)
                    {
                        _date.Value = (string)elm.Attribute("date") ?? "";
                    }
                    foreach (var tmp in elm.Descendants("RoomType"))
                        AvailRateRoomTypes.Add(new AvailRateRoomType(tmp));
                }
            }
        }
        public class AvailRateRoomType
        {
            public string _id { get; set; }
            public bool? _closed { get; set; }
            public Inventory _Inventory { get; set; }
            public List<AvailRateRatePlan> RatePlans { get; set; }
            public AvailRateRoomType(XElement elm)
            {
                RatePlans = new List<AvailRateRatePlan>();
                _Inventory = new Inventory();
                if (elm != null)
                {
                    _id = (string)elm.Attribute("id") ?? "";
                    _closed = (bool)elm.Attribute("closed");
                    _Inventory = new Inventory(elm.Element("Inventory"));
                    foreach (var tmp in elm.Descendants("RatePlan"))
                        RatePlans.Add(new AvailRateRatePlan(tmp));
                }
            }
        }
        public class Inventory
        {
            public int _baseAllocation { get; set; }
            public int _flexibleAllocation { get; set; }
            public int _totalInventoryAvailable { get; set; }
            public int _totalInventorySold { get; set; }
            public Inventory()
            {
            }
            public Inventory(XElement elm)
            {
                if (elm != null)
                {
                    _baseAllocation = ((string)elm.Attribute("baseAllocation")).objToInt32();
                    _flexibleAllocation = ((string)elm.Attribute("flexibleAllocation")).objToInt32();
                    _totalInventoryAvailable = ((string)elm.Attribute("totalInventoryAvailable")).objToInt32();
                    _totalInventorySold = ((string)elm.Attribute("totalInventorySold")).objToInt32();
                }
            }
        }
        public class AvailRateRatePlan
        {
            public string _id { get; set; }
            public bool? _closed { get; set; }
            public Rate _Rate { get; set; }
            public Restrictions _Restrictions { get; set; }
            public AvailRateRatePlan(XElement elm)
            {
                _Rate = new Rate();
                _Restrictions = new Restrictions();
                if (elm != null)
                {
                    _id = (string)elm.Attribute("id") ?? "";
                    _closed = (bool)elm.Attribute("closed");
                    _Rate = new Rate(elm.Element("Rate"));
                    _Restrictions = new Restrictions(elm.Element("Restrictions"));

                }
            }

        }
        public class Rate
        {
            public string _currency { get; set; }
            public List<PerOccupancy> PerOccupancys { get; set; }
            public Rate()
            {
            }
            public Rate(XElement elm)
            {
                PerOccupancys = new List<PerOccupancy>();
                if (elm != null)
                {
                    _currency = (string)elm.Attribute("currency") ?? "";
                    foreach (var tmp in elm.Descendants("PerOccupancy"))
                        PerOccupancys.Add(new PerOccupancy(tmp));
                }
            }
        }
        public class PerOccupancy
        {
            public int? _occupancy { get; set; }
            public DecimalUnit _rate { get; set; }
            public PerOccupancy()
            {
                _rate = new DecimalUnit();
            }
            public PerOccupancy(XElement elm)
            {
                _rate = new DecimalUnit();
                if (elm != null)
                {
                    _rate.Value = ((string)elm.Attribute("rate")) ?? "";
                    _occupancy = ((string)elm.Attribute("occupancy")).objToInt32();
                }
            }

        }
        public class Restrictions
        {
            public int? _minLOS { get; set; }
            public int? _maxLOS { get; set; }
            public bool? _closedToArrival { get; set; }
            public bool? _closedToDeparture { get; set; }
            public Restrictions()
            {
            }
            public Restrictions(XElement elm)
            {
                if (elm != null)
                {
                    _minLOS = ((string)elm.Attribute("minLOS")).objToInt32();
                    _maxLOS = ((string)elm.Attribute("maxLOS")).objToInt32();
                    _closedToArrival = (bool)(elm.Attribute("closedToArrival"));
                    _closedToDeparture = (bool)(elm.Attribute("closedToDeparture"));
                }

            }

        }
    }
    #endregion
    #region AR
    public class AvailRateUpdateRQ
    {
        private Authentication authentication { get; set; }
        private Hotel hotel { get; set; }
        public List<AvailRateUpdate> AvailRateUpdates { get; set; }
        public AvailRateUpdateRQ(RntChnlExpediaHotelTBL hotelTbl)
        {
            authentication = new Authentication(hotelTbl);
            hotel = new Hotel(hotelTbl);
            AvailRateUpdates = new List<AvailRateUpdate>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XNamespace ns = "http://www.expediaconnect.com/EQC/AR/2011/06";
            XElement root = new XElement(ns + "AvailRateUpdateRQ");
            root.Add(authentication.GetElement(ns));
            root.Add(hotel.GetElement(ns));
            foreach (var tmp in AvailRateUpdates)
                root.Add(tmp.GetElement(ns));
            _resource.Add(root);
            return _resource.ToString();
        }
        public class AvailRateUpdate
        {
            public DateRangeClass DateRange { get; set; }
            public List<RoomType> RoomTypes { get; set; }
            public AvailRateUpdate()
            {
                DateRange = new DateRangeClass();
                RoomTypes = new List<RoomType>();
            }
            public XElement GetElement(XNamespace ns)
            {
                XElement elm = new XElement(ns + "AvailRateUpdate");
                elm.Add(DateRange.GetElement(ns));
                foreach (var tmp in RoomTypes)
                    elm.Add(tmp.GetElement(ns));
                return elm;
            }
        }
        public class RoomType
        {
            public string _id { get; set; }
            public bool? _closed { get; set; }
            public Inventory inventory { get; set; }
            public List<RatePlan> RatePlans { get; set; }
            public RoomType()
            {
                inventory = new Inventory();
                RatePlans = new List<RatePlan>();
            }
            public XElement GetElement(XNamespace ns)
            {
                XElement elm = new XElement(ns + "RoomType");
                elm.Add(new XAttribute("id", _id));
                if (_closed.HasValue)
                    elm.Add(new XAttribute("closed", _closed.Value));
                if (inventory.HasValue())
                    elm.Add(inventory.GetElement(ns));
                foreach (var tmp in RatePlans)
                    elm.Add(tmp.GetElement(ns));
                return elm;
            }
            public class Inventory
            {
                public int? _flexibleAllocation { get; set; }
                public int? _totalInventoryAvailable { get; set; }
                public Inventory()
                {
                }
                public bool HasValue()
                {
                    return _flexibleAllocation.HasValue || _totalInventoryAvailable.HasValue;
                }
                public XElement GetElement(XNamespace ns)
                {
                    XElement elm = new XElement(ns + "Inventory");
                    if (_flexibleAllocation.HasValue)
                        elm.Add(new XAttribute("flexibleAllocation", _flexibleAllocation.Value));
                    if (_totalInventoryAvailable.HasValue)
                        elm.Add(new XAttribute("totalInventoryAvailable", _totalInventoryAvailable.Value));
                    return elm;
                }
            }
            public class RatePlan
            {
                public string _id { get; set; }
                public bool? _closed { get; set; }
                public List<Rate> Rates { get; set; }
                public Restrictions Restriction { get; set; }
                public RatePlan()
                {
                    //_closed = false;
                    Rates = new List<Rate>();
                    Restriction = new Restrictions();
                }
                public XElement GetElement(XNamespace ns)
                {
                    XElement elm = new XElement(ns + "RatePlan");
                    elm.Add(new XAttribute("id", _id));
                    if (_closed.HasValue)
                        elm.Add(new XAttribute("closed", _closed.Value));
                    foreach (var tmp in Rates)
                        elm.Add(tmp.GetElement(ns));
                    if (Restriction.HasValue())
                        elm.Add(Restriction.GetElement(ns));
                    return elm;
                }
                public class Rate
                {
                    public bool? _rateChangeIndicator { get; set; }
                    public string currency { get; set; }
                    public int? _lengthOfStay { get; set; }
                    public PerDay perDay { get; set; }
                    public List<PerOccupancy> perOccupancy { get; set; }
                    public List<PerPerson> perPerson { get; set; }
                    public Rate()
                    {
                        _rateChangeIndicator = true;
                        currency = "EUR";
                        perDay = new PerDay();
                        perOccupancy = new List<PerOccupancy>();
                        perPerson = new List<PerPerson>();
                    }
                    public XElement GetElement(XNamespace ns)
                    {
                        XElement elm = new XElement(ns + "Rate");
                        //if (_rateChangeIndicator.HasValue)
                        //elm.Add(new XAttribute("rateChangeIndicator", _rateChangeIndicator.Value));
                        elm.Add(new XAttribute("currency", currency));
                        if (_lengthOfStay.HasValue)
                            elm.Add(new XAttribute("lengthOfStay", _lengthOfStay.Value));
                        if (perDay.HasValue())
                            elm.Add(perDay.GetElement(ns));
                        foreach (var tmp in perOccupancy)
                            if (tmp.HasValue())
                                elm.Add(tmp.GetElement(ns));
                        foreach (var tmp in perPerson)
                            if (tmp.HasValue())
                                elm.Add(tmp.GetElement(ns));
                        return elm;
                    }
                    public class PerDay
                    {
                        public DecimalUnit _rate { get; set; }
                        public PerDay()
                        {
                            _rate = new DecimalUnit();
                        }
                        public bool HasValue()
                        {
                            return _rate.Value != "";
                        }
                        public XElement GetElement(XNamespace ns)
                        {
                            XElement elm = new XElement(ns + "PerDay");
                            if (_rate.Value != "")
                                elm.Add(new XAttribute("rate", _rate.Value));
                            return elm;
                        }
                    }
                    public class PerOccupancy
                    {
                        public int? _occupancy { get; set; }
                        public DecimalUnit _rate { get; set; }
                        public PerOccupancy()
                        {
                            _rate = new DecimalUnit();
                        }
                        public bool HasValue()
                        {
                            return _rate.Value != "" && _occupancy.HasValue;
                        }
                        public XElement GetElement(XNamespace ns)
                        {
                            XElement elm = new XElement(ns + "PerOccupancy");
                            if (_occupancy.HasValue)
                                elm.Add(new XAttribute("occupancy", _occupancy.Value));
                            if (_rate.Value != "")
                                elm.Add(new XAttribute("rate", _rate.Value));
                            return elm;
                        }

                    }
                    public class PerPerson
                    {
                        public DecimalUnit _rate { get; set; }
                        public PerPerson()
                        {
                            _rate = new DecimalUnit();
                        }
                        public bool HasValue()
                        {
                            return _rate.Value != "";
                        }
                        public XElement GetElement(XNamespace ns)
                        {
                            XElement elm = new XElement(ns + "PerPerson");
                            if (_rate.Value != "")
                                elm.Add(new XAttribute("rate", _rate.Value));
                            return elm;
                        }
                    }

                }
                public class Restrictions
                {
                    public int? _minLOS { get; set; }
                    public int? _maxLOS { get; set; }
                    public bool? _closedToArrival { get; set; }
                    public bool? _closedToDeparture { get; set; }
                    public Restrictions()
                    {
                    }
                    public bool HasValue()
                    {
                        return _minLOS.HasValue || _maxLOS.HasValue || _closedToArrival.HasValue || _closedToDeparture.HasValue;
                    }
                    public XElement GetElement(XNamespace ns)
                    {
                        XElement elm = new XElement(ns + "Restrictions");
                        if (_minLOS.HasValue)
                            elm.Add(new XAttribute("minLOS", _minLOS.Value));
                        if (_maxLOS.HasValue)
                            elm.Add(new XAttribute("maxLOS", _maxLOS.Value));
                        if (_closedToArrival.HasValue)
                            elm.Add(new XAttribute("closedToArrival", _closedToArrival.Value));
                        if (_closedToDeparture.HasValue)
                            elm.Add(new XAttribute("closedToDeparture", _closedToDeparture.Value));
                        return elm;
                    }
                }
            }
        }
    }
    public class AvailRateUpdateRS
    {
        public Success success { get; set; }
        public List<Error> Errors { get; set; }
        public AvailRateUpdateRS(string xmlData)
        {
            Errors = new List<Error>();
            xmlData = xmlData.Replace("xmlns=", "tmp=");
            XDocument _resource = XDocument.Parse(xmlData);
            var ds = _resource.Element("AvailRateUpdateRS");
            if (ds.Element("Success") != null) success = new Success(ds.Element("Success"));
            foreach (var tmp in ds.Descendants("Error"))
                Errors.Add(new Error(tmp));
        }
        public class Success
        {
            public List<Warning> Warnings { get; set; }
            public Success(XElement elm)
            {
                Warnings = new List<Warning>();
                if (elm != null)
                {
                    foreach (var tmp in elm.Descendants("Warning"))
                        Warnings.Add(new Warning(tmp));
                }
            }
        }
        public class Warning
        {
            public string _code { get; set; }
            public string _description { get; set; }
            public Warning(XElement elm)
            {
                if (elm != null)
                {
                    _code = (string)elm.Attribute("code") ?? "";
                    _description = elm.Value;
                }
            }
        }
        public class Error
        {
            public string _code { get; set; }
            public string description { get; set; }
            public Error(XElement elm)
            {
                if (elm != null)
                {
                    _code = (string)elm.Attribute("code") ?? "";
                    description = elm.Value;
                }
            }
        }
    }
    #endregion
    #region BR
    public class BookingRetrievalRQ
    {
        private Authentication authentication { get; set; }
        private Hotel hotel { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<Status> Statuss { get; set; }
        public int? NbDaysInPast { get; set; }
        public BookingRetrievalRQ(RntChnlExpediaHotelTBL hotelTbl)
        {
            authentication = new Authentication(hotelTbl);
            hotel = new Hotel(hotelTbl);
            Bookings = new List<Booking>();
            Statuss = new List<Status>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XNamespace ns = "http://www.expediaconnect.com/EQC/BR/2014/01";
            XElement root = new XElement(ns + "BookingRetrievalRQ");
            root.Add(authentication.GetElement(ns));
            root.Add(hotel.GetElement(ns));
            var ParamSet = new XElement(ns + "ParamSet");
            foreach (var tmp in Bookings)
                root.Add(tmp.GetElement(ns));
            foreach (var tmp in Statuss)
                root.Add(tmp.GetElement(ns));
            if (NbDaysInPast.HasValue)
                ParamSet.Add(new XElement(ns + "NbDaysInPast", NbDaysInPast));
            root.Add(ParamSet);
            _resource.Add(root);
            return _resource.ToString();
        }
        public class Booking
        {
            public int? _id { get; set; }
            public Booking()
            {
            }
            public bool HasValue()
            {
                return _id.HasValue;
            }
            public XElement GetElement(XNamespace ns)
            {
                XElement elm = new XElement(ns + "Booking");
                if (_id.HasValue)
                    elm.Add(new XAttribute("id", _id.Value));
                return elm;
            }
        }
        public class Status
        {
            public string _value { get; set; }
            public Status()
            {
            }
            public bool HasValue()
            {
                return !string.IsNullOrEmpty(_value);
            }
            public XElement GetElement(XNamespace ns)
            {
                XElement elm = new XElement(ns + "Status");
                if (!string.IsNullOrEmpty(_value))
                    elm.Add(new XAttribute("value", _value));
                return elm;
            }
        }
    }
    public class BookingRetrievalRS
    {
        public List<Booking> Bookings { get; set; }
        public List<Error> Errors { get; set; }
        public BookingRetrievalRS(string xmlData)
        {
            Bookings = new List<Booking>();
            Errors = new List<Error>();
            xmlData = xmlData.Replace("xmlns=", "tmp=");
            XDocument _resource = XDocument.Parse(xmlData);
            var ds = _resource.Element("BookingRetrievalRS");
            if (ds.Element("Bookings") != null)
            {
                foreach (var tmp in ds.Element("Bookings").Descendants("Booking"))
                    Bookings.Add(new Booking(tmp));
            }
            foreach (var tmp in ds.Descendants("Error"))
                Errors.Add(new Error(tmp));
        }
        public class Error
        {
            public string _code { get; set; }
            public string description { get; set; }
            public Error(XElement elm)
            {
                if (elm != null)
                {
                    _code = (string)elm.Attribute("code") ?? "";
                    description = elm.Value;
                }
            }
        }
        public class Booking
        {
            public long _id { get; set; }
            /// <summary>
            /// - Book
            /// - Modify
            /// - Cancel
            /// </summary>
            public string _type { get; set; }
            public DateAndTime _createDateTime { get; set; }
            public string _source { get; set; }
            /// <summary>
            /// Status of the booking transaction at the time of the retrieval request. Possible values are:
            /// - pending: message is retrieved for the first time.
            /// - retrieved: message was already retrieved at least once but not confirmed yet
            /// - confirmed: message was already retrieved and confirmed via BC
            /// </summary>
            public string _status { get; set; }
            public string _confirmNumber { get; set; }
            public Hotel hotel { get; set; }
            public RoomStay roomStay { get; set; }
            public PrimaryGuest primaryGuest { get; set; }
            public List<SpecialRequest> SpecialRequests { get; set; }
            public Booking(XElement elm)
            {
                _createDateTime = new DateAndTime();
                if (elm != null)
                {
                    SpecialRequests = new List<SpecialRequest>();
                    _id = elm.Attribute("id") != null ? ((string)elm.Attribute("id")).objToInt64() : 0;
                    _type = (string)elm.Attribute("type") ?? "";
                    _createDateTime.Value = (string)elm.Attribute("createDateTime") ?? "";
                    _source = (string)elm.Attribute("source") ?? "";
                    _status = (string)elm.Attribute("status") ?? "";
                    _confirmNumber = (string)elm.Attribute("confirmNumber") ?? "";
                    hotel = new Hotel(elm.Element("Hotel"));
                    roomStay = new RoomStay(elm.Element("RoomStay"));
                    primaryGuest = new PrimaryGuest(elm.Element("PrimaryGuest"));
                    foreach (var tmp in elm.Descendants("SpecialRequest"))
                        SpecialRequests.Add(new SpecialRequest(tmp));
                }
            }
            public class Hotel
            {
                public int _id { get; set; }
                public Hotel(XElement elm)
                {
                    if (elm != null)
                    {
                        _id = ((string)elm.Attribute("id")).objToInt32();
                    }
                }
            }
            public class RoomStay
            {
                public string _roomTypeID { get; set; }
                public string _ratePlanID { get; set; }
                public StayDate stayDate { get; set; }
                public GuestCount guestCount { get; set; }
                public PerDayRates perDayRates { get; set; }
                public Total total { get; set; }
                public PaymentCard paymentCard { get; set; }
                public RewardProgram rewardProgram { get; set; }
                //public List<SpecialRequest> SpecialRequests { get; set; }
                public RoomStay(XElement elm)
                {
                    //SpecialRequests = new List<SpecialRequest>();
                    if (elm != null)
                    {
                        _roomTypeID = (string)elm.Attribute("roomTypeID") ?? "";
                        _ratePlanID = (string)elm.Attribute("ratePlanID") ?? "";
                        stayDate = new StayDate(elm.Element("StayDate"));
                        guestCount = new GuestCount(elm.Element("GuestCount"));
                        perDayRates = new PerDayRates(elm.Element("PerDayRates"));
                        total = new Total(elm.Element("Total"));
                        paymentCard = new PaymentCard(elm.Element("PaymentCard"));
                        rewardProgram = new RewardProgram(elm.Element("RewardProgram"));
                        //foreach (var tmp in elm.Descendants("SpecialRequest"))
                        //    SpecialRequests.Add(new SpecialRequest(tmp));
                    }
                }
                public class StayDate
                {
                    public Date _arrival { get; set; }
                    public Date _departure { get; set; }
                    public StayDate(XElement elm)
                    {
                        _arrival = new Date();
                        _departure = new Date();
                        if (elm != null)
                        {
                            _arrival.Value = (string)elm.Attribute("arrival") ?? "";
                            _departure.Value = (string)elm.Attribute("departure") ?? "";
                        }
                    }
                }
                public class GuestCount
                {
                    public int _adult { get; set; }
                    public int _child { get; set; }
                    public List<Child> Childs { get; set; }
                    public GuestCount(XElement elm)
                    {
                        Childs = new List<Child>();
                        if (elm != null)
                        {
                            _adult = ((string)elm.Attribute("adult")).objToInt32();
                            _child = ((string)elm.Attribute("child")).objToInt32();
                            foreach (var tmp in elm.Descendants("Child"))
                                Childs.Add(new Child(tmp));
                        }
                    }
                    public class Child
                    {
                        public int _age { get; set; }
                        public Child(XElement elm)
                        {
                            if (elm != null)
                            {
                                _age = ((string)elm.Attribute("age")).objToInt32();
                            }
                        }
                    }
                }
                public class PerDayRates
                {
                    public string _currency { get; set; }
                    public List<PerDayRate> perDayRates { get; set; }
                    public PerDayRates(XElement elm)
                    {
                        perDayRates = new List<PerDayRate>();
                        if (elm != null)
                        {
                            _currency = (string)elm.Attribute("currency") ?? "";
                            foreach (var tmp in elm.Descendants("PerDayRate"))
                                perDayRates.Add(new PerDayRate(tmp));
                        }
                    }
                    public class PerDayRate
                    {
                        public Date _stayDate { get; set; }
                        public DecimalUnit _baseRate { get; set; }
                        public DecimalUnit _extraPersonFees { get; set; }
                        public DecimalUnit _hotelServiceFees { get; set; }
                        public string _promoName { get; set; }
                        public PerDayRate(XElement elm)
                        {
                            _stayDate = new Date();
                            _baseRate = new DecimalUnit();
                            _extraPersonFees = new DecimalUnit();
                            _hotelServiceFees = new DecimalUnit();
                            if (elm != null)
                            {
                                _stayDate.Value = (string)elm.Attribute("stayDate") ?? "";
                                _baseRate.Value = (string)elm.Attribute("baseRate") ?? "";
                                _extraPersonFees.Value = (string)elm.Attribute("extraPersonFees") ?? "";
                                _hotelServiceFees.Value = (string)elm.Attribute("hotelServiceFees") ?? "";
                                _promoName = (string)elm.Attribute("promoName") ?? "";
                            }
                        }
                    }
                }
                public class Total
                {
                    public DecimalUnit _amountAfterTaxes { get; set; }
                    public DecimalUnit _amountOfTaxes { get; set; }
                    public string _currency { get; set; }
                    public Total(XElement elm)
                    {
                        _amountAfterTaxes = new DecimalUnit();
                        _amountOfTaxes = new DecimalUnit();
                        if (elm != null)
                        {
                            _amountAfterTaxes.Value = ((string)elm.Attribute("amountAfterTaxes")) ?? "";
                            _amountOfTaxes.Value = ((string)elm.Attribute("amountOfTaxes")) ?? "";
                            _currency = (string)elm.Attribute("currency") ?? "";
                        }
                    }
                }
                public class PaymentCard
                {
                    public string _cardCode { get; set; }
                    public string _cardCodeDesc { get; set; }
                    public string _cardNumber { get; set; }
                    public string _seriesCode { get; set; }
                    public string _expireDate { get; set; }
                    public CardHolder cardHolder { get; set; }
                    public PaymentCard(XElement elm)
                    {
                        if (elm != null)
                        {
                            _cardCode = (string)elm.Attribute("cardCode") ?? "";
                            _cardCodeDesc = "";
                            if (_cardCode == "VI") _cardCodeDesc = "Visa";
                            if (_cardCode == "MC") _cardCodeDesc = "MasterCard";
                            if (_cardCode == "AX") _cardCodeDesc = "American Express";
                            if (_cardCode == "DS") _cardCodeDesc = "Discover card";
                            if (_cardCode == "CA") _cardCodeDesc = "MasterCard";
                            if (_cardCode == "JC") _cardCodeDesc = "Japan Credit Bureau";
                            if (_cardCode == "DN") _cardCodeDesc = "Diners Club";
                            _cardNumber = (string)elm.Attribute("cardNumber") ?? "";
                            _seriesCode = (string)elm.Attribute("seriesCode") ?? "";
                            _expireDate = (string)elm.Attribute("expireDate") ?? "";
                            cardHolder = new CardHolder(elm.Element("CardHolder"));
                        }
                    }
                    public class CardHolder
                    {
                        public string _name { get; set; }
                        public string _address { get; set; }
                        public string _city { get; set; }
                        public string _stateProv { get; set; }
                        public string _country { get; set; }
                        public string _postalCode { get; set; }
                        public CardHolder(XElement elm)
                        {
                            if (elm != null)
                            {
                                _name = (string)elm.Attribute("name") ?? "";
                                _address = (string)elm.Attribute("address") ?? "";
                                _city = (string)elm.Attribute("city") ?? "";
                                _stateProv = (string)elm.Attribute("stateProv") ?? "";
                                _country = (string)elm.Attribute("country") ?? "";
                                _postalCode = (string)elm.Attribute("postalCode") ?? "";
                            }
                        }
                    }
                }

                public class RewardProgram
                {
                    public string _code { get; set; }
                    public string _number { get; set; }
                    public RewardProgram(XElement elm)
                    {
                        if (elm != null)
                        {
                            _code = (string)elm.Attribute("code") ?? "";
                            _number = (string)elm.Attribute("number") ?? "";
                        }
                    }
                }
                public class SpecialRequest
                {
                    public string _code { get; set; }
                    public string description { get; set; }
                    public SpecialRequest(XElement elm)
                    {
                        if (elm != null)
                        {
                            _code = (string)elm.Attribute("code") ?? "";
                            description = elm.Value;
                        }
                    }
                }
            }
            public class PrimaryGuest
            {
                public Name name { get; set; }
                public Phone phone { get; set; }
                public string email { get; set; }
                public PrimaryGuest(XElement elm)
                {
                    if (elm != null)
                    {
                        name = new Name(elm.Element("Name"));
                        phone = new Phone(elm.Element("Phone"));
                        email = (string)elm.Element("Email") ?? "";
                    }
                }
                public class Name
                {
                    public string _givenName { get; set; }
                    public string _middleName { get; set; }
                    public string _surname { get; set; }
                    public Name(XElement elm)
                    {
                        if (elm != null)
                        {
                            _givenName = (string)elm.Attribute("givenName") ?? "";
                            _middleName = (string)elm.Attribute("middleName") ?? "";
                            _surname = (string)elm.Attribute("surname") ?? "";
                        }
                    }
                }
                public class Phone
                {
                    public int _countryCode { get; set; }
                    public int _cityAreaCode { get; set; }
                    public string _number { get; set; }
                    public int _extension { get; set; }
                    public Phone(XElement elm)
                    {
                        if (elm != null)
                        {
                            _countryCode = ((string)elm.Attribute("countryCode")).objToInt32();
                            _cityAreaCode = ((string)elm.Attribute("cityAreaCode")).objToInt32();
                            _number = (string)elm.Attribute("number") ?? "";
                            _extension = ((string)elm.Attribute("extension")).objToInt32();
                        }
                    }
                }
            }
            public class SpecialRequest
            {
                public string _code { get; set; }
                public string description { get; set; }
                public SpecialRequest(XElement elm)
                {
                    if (elm != null)
                    {
                        _code = (string)elm.Attribute("code") ?? "";
                        description = elm.Value;
                    }
                }
            }
        }
    }
    #endregion
    #region BC
    public class BookingConfirmRQ
    {
        private Authentication authentication { get; set; }
        private Hotel hotel { get; set; }
        public List<BookingConfirmNumber> BookingConfirmNumbers { get; set; }
        public BookingConfirmRQ(RntChnlExpediaHotelTBL hotelTbl)
        {
            authentication = new Authentication(hotelTbl);
            hotel = new Hotel(hotelTbl);
            BookingConfirmNumbers = new List<BookingConfirmNumber>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XNamespace ns = "http://www.expediaconnect.com/EQC/BC/2007/09";
            XElement root = new XElement(ns + "BookingConfirmRQ");
            root.Add(authentication.GetElement(ns));
            root.Add(hotel.GetElement(ns));
            XElement elm = new XElement(ns + "BookingConfirmNumbers");
            foreach (var tmp in BookingConfirmNumbers)
                elm.Add(tmp.GetElement(ns));
            root.Add(elm);
            _resource.Add(root);
            return _resource.ToString();
        }
        public class BookingConfirmNumber
        {
            public long _bookingID { get; set; }
            public string _bookingType { get; set; }
            public string _confirmNumber { get; set; }
            public DateAndTime _confirmTime { get; set; }
            public BookingConfirmNumber()
            {
                _confirmTime = new DateAndTime();
                _confirmTime.ValueDate = DateTime.Now;
            }
            public XElement GetElement(XNamespace ns)
            {
                XElement elm = new XElement(ns + "BookingConfirmNumber");
                elm.Add(new XAttribute("bookingID", _bookingID));
                elm.Add(new XAttribute("bookingType", _bookingType));
                elm.Add(new XAttribute("confirmNumber", _confirmNumber));
                elm.Add(new XAttribute("confirmTime", _confirmTime.Value));
                return elm;
            }
        }
    }
    public class BookingConfirmRS
    {
        public Success success { get; set; }
        public List<Error> Errors { get; set; }
        public BookingConfirmRS(string xmlData)
        {
            Errors = new List<Error>();
            xmlData = xmlData.Replace("xmlns=", "tmp=");
            XDocument _resource = XDocument.Parse(xmlData);
            var ds = _resource.Element("BookingConfirmRS");
            if (ds.Element("Success") != null) success = new Success(ds.Element("Success"));
            foreach (var tmp in ds.Descendants("Error"))
                Errors.Add(new Error(tmp));
        }
        public class Success
        {
            public List<Warning> Warnings { get; set; }
            public Success(XElement elm)
            {
                Warnings = new List<Warning>();
                if (elm != null)
                {
                    foreach (var tmp in elm.Descendants("Warning"))
                        Warnings.Add(new Warning(tmp));
                }
            }
        }
        public class Warning
        {
            public string _code { get; set; }
            public string _description { get; set; }
            public Warning(XElement elm)
            {
                if (elm != null)
                {
                    _code = (string)elm.Attribute("code") ?? "";
                    _description = elm.Value;
                }
            }
        }
        public class Error
        {
            public string _code { get; set; }
            public string description { get; set; }
            public Error(XElement elm)
            {
                if (elm != null)
                {
                    _code = (string)elm.Attribute("code") ?? "";
                    description = elm.Value;
                }
            }
        }
    }
    #endregion


    #region Availability
    public class EstateAvailability
    {
        public DateTime date { get; set; }
        public List<EstaeRooms> EstateRooms { get; set; }
        public EstateAvailability(DateTime _currDate, List<EstaeRooms> _currRooms)
        {
            this.date = _currDate;
            this.EstateRooms = _currRooms;
        }
    }

    public class EstaeRooms
    {
        public int RoomID { get; set; }
        public int minStay { get; set; }
        public int units { get; set; }
        public EstaeRooms(int _RoomID, int _minStay, int _units)
        {
            this.RoomID = _RoomID;
            this.minStay = _minStay;
            this.units = _units;
        }
    }
    #endregion

    public class AvailabilityListPerDates
    {
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public int pidEstate { get; set; }
        public bool isAllAvailable { get; set; }
        public int minStay { get; set; }
        public int units { get; set; }

        public AvailabilityListPerDates(DateTime _dtStart, DateTime _dtEnd, int _pidEstate, bool _isAllAvailable, int _minStay, int _unit)
        {
            DtStart = _dtStart;
            DtEnd = _dtEnd;
            pidEstate = _pidEstate;
            isAllAvailable = _isAllAvailable;
            minStay = _minStay;
            units = _unit;
        }

        public bool HasSameAvailability(AvailabilityListPerDates compareWith)
        {
            if (pidEstate != compareWith.pidEstate) return false;
            if (DtEnd.AddDays(1) != compareWith.DtStart) return false;
            if (isAllAvailable != compareWith.isAllAvailable) return false;
            return true;
        }
    }

    public class Inquirer
    {
        public string locale { get; set; }
        public string name { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public string phoneCountryCode { get; set; }
        public string phoneNumber { get; set; }
        public string phoneExt { get; set; }
        public Inquirer()
        {
        }
        public Inquirer(XElement elm)
        {
            if (elm != null)
            {
                if (elm.Attributes("locale").FirstOrDefault() != null)
                    locale = elm.Attributes("locale").FirstOrDefault().Value;

                if (elm.Descendants("name").FirstOrDefault() != null)
                    name = (string)elm.Descendants("name").FirstOrDefault().Value;

                if (elm.Descendants("firstName").FirstOrDefault() != null)
                    firstName = (string)elm.Descendants("firstName").FirstOrDefault().Value;

                if (elm.Descendants("lastName").FirstOrDefault() != null)
                    lastName = (string)elm.Descendants("lastName").FirstOrDefault().Value;

                if (elm.Descendants("emailAddress").FirstOrDefault() != null)
                    emailAddress = (string)elm.Descendants("emailAddress").FirstOrDefault().Value;

                if (elm.Descendants("phoneCountryCode").FirstOrDefault() != null)
                    phoneCountryCode = (string)elm.Descendants("phoneCountryCode").FirstOrDefault().Value;

                if (elm.Descendants("phoneNumber").FirstOrDefault() != null)
                    phoneNumber = (string)elm.Descendants("phoneNumber").FirstOrDefault().Value;

                if (elm.Descendants("phoneExt").FirstOrDefault() != null)
                    phoneExt = (string)elm.Descendants("phoneExt").FirstOrDefault().Value;

            }
        }

        public XElement GetElement()
        {
            XElement elm = new XElement("inquirer");
            if (!string.IsNullOrEmpty(locale))
                elm.Add(new XElement("locale", locale));
            if (!string.IsNullOrEmpty(name))
                elm.Add(new XElement("name", name));
            if (!string.IsNullOrEmpty(firstName))
                elm.Add(new XElement("firstName", firstName));
            if (!string.IsNullOrEmpty(lastName))
                elm.Add(new XElement("lastName", lastName));
            if (!string.IsNullOrEmpty(emailAddress))
                elm.Add(new XElement("emailAddress", emailAddress));
            if (!string.IsNullOrEmpty(phoneCountryCode))
                elm.Add(new XElement("phoneCountryCode", phoneCountryCode));
            if (!string.IsNullOrEmpty(phoneNumber))
                elm.Add(new XElement("phoneNumber", phoneNumber));
            if (!string.IsNullOrEmpty(phoneExt))
                elm.Add(new XElement("phoneExt", phoneExt));
            return elm;
        }
    }

    #region property classes
    public class PropertyNewRequest
    {
        public string providerPropertyId { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string structureType { get; set; }
        public string currencyCode { get; set; }
        public string billingCurrencyCode { get; set; }
        public string timeZone { get; set; }
        public List<address> addresses { get; set; }
        public contacts contacts { get; set; }
        public List<content> contents { get; set; }
        public List<taxes> taxes { get; set; }
        public List<policies> policies { get; set; }
        public List<propertyCollectedMandatoryFees> propertyCollectedMandatoryFees { get; set; }

        public inventorySettings inventorySettings { get; set; }
        public List<attribute> attributes { get; set; }


        public PropertyNewRequest()
        {
            providerPropertyId = "";
            name = "";
            latitude = "";
            longitude = "";
            structureType = "";
            currencyCode = "EUR";
            billingCurrencyCode = "EUR";
            timeZone = "Europe/Rome";
            addresses = new List<address>();
            contacts = new contacts();
            contents = new List<content>();
            attributes = new List<attribute>();
        }
    }
    public class inventorySettings
    {
        public string distributionModels { get; set; }
    }

    public class PropertyNewResponse
    {
        public List<entity> entity { get; set; }
        public PropertyNewResponse()
        {
            entity = new List<entity>();
        }
    }
    public class entity
    {
        public string providerPropertyId { get; set; }
        public string expediaId { get; set; }
        public string provider { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string structureType { get; set; }
        public string currencyCode { get; set; }
        public string billingCurrencyCode { get; set; }
        public string timeZone { get; set; }
        public List<address> addresses { get; set; }
        public contacts contacts { get; set; }
        public List<content> contents { get; set; }
        public List<attribute> attributes { get; set; }

        public entity()
        {
            providerPropertyId = "";
            expediaId = "";
            provider = "";
            name = "";
            latitude = "";
            longitude = "";
            structureType = "";
            currencyCode = "EUR";
            billingCurrencyCode = "EUR";
            timeZone = "Europe/Rome";
            addresses = new List<address>();
            contacts = new contacts();
            contents = new List<content>();
            attributes = new List<attribute>();
        }
    }
    public class address
    {
        public string line1 { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string countryCode { get; set; }

        public address()
        {
            line1 = "";
            city = "";
            postalCode = "";
            countryCode = "";
        }
    }
    public class contacts
    {
        public Property Property { get; set; }
        public ReservationManager ReservationManager { get; set; }
        public AlternateReservationManager AlternateReservationManager { get; set; }

        public contacts()
        {
            Property = new Property();
            ReservationManager = new ReservationManager();
            AlternateReservationManager = new AlternateReservationManager();
        }
    }
    public class Property
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public List<phoneNumber> phoneNumbers { get; set; }
        public List<string> emails { get; set; }

        public Property()
        {
            firstName = "";
            lastName = "";
            phoneNumbers = new List<phoneNumber>();
            emails = new List<string>();
        }
    }
    public class phoneNumber
    {
        public string phoneNumberType { get; set; }
        public string countryAccessCode { get; set; }
        public string areaCode { get; set; }
        public string number { get; set; }

        public phoneNumber()
        {
            phoneNumberType = "Phone";
            countryAccessCode = CommonUtilities.getSYS_SETTING("expedia_country_code");
            areaCode = CommonUtilities.getSYS_SETTING("expedia_area_code");
            number = "";
        }
    }
    public class ReservationManager
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public List<phoneNumber> phoneNumbers { get; set; }
        public List<string> emails { get; set; }

        public ReservationManager()
        {
            firstName = "";
            lastName = "";
            phoneNumbers = new List<phoneNumber>();
            emails = new List<string>();
        }
    }
    public class AlternateReservationManager
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public List<phoneNumber> phoneNumbers { get; set; }
        public List<string> emails { get; set; }

        public AlternateReservationManager()
        {
            firstName = "";
            lastName = "";
            phoneNumbers = new List<phoneNumber>();
            emails = new List<string>();
        }
    }
    public class content
    {
        public string locale { get; set; }
        public string name { get; set; }
        public List<image> images { get; set; }
        public List<amenity> amenities { get; set; }
        public List<paragraphs> paragraphs { get; set; }
        public content()
        {
            locale = "";
            name = "";
            images = new List<image>();
            amenities = new List<amenity>();
            paragraphs = new List<paragraphs>();
        }
    }
    public class paragraphs
    {
        public string code { get; set; }
        public string value { get; set; }
    }
    public class image
    {
        public string url { get; set; }
        public string categoryCode { get; set; }
        public image()
        {
            url = "";
            categoryCode = "";
        }
    }
    public class amenity
    {
        public string code { get; set; }
        public string detailCode { get; set; }
        public int? value { get; set; }

        public amenity()
        {
            code = "";
            //detailCode = "";
        }
    }

    public class amenityResponse
    {
        public ameityEntity entity { get; set; }
    }
    public class ameityEntity
    {
        public amenity amenity { get; set; }
    }

    public class attribute
    {
        public string code { get; set; }
        public string value { get; set; }

        public attribute()
        {
            code = "";
            value = "";
        }
    }
    public class taxes
    {
        public string code { get; set; }
        public string value { get; set; }
        public string detailCode { get; set; }

        public taxes()
        {
            code = "";
        }
    }
    public class propertyCollectedMandatoryFees
    {
        public string code { get; set; }
        public string value { get; set; }
        public string scope { get; set; }
        public string duration { get; set; }

        public propertyCollectedMandatoryFees()
        {
            code = "";
        }
    }
    public class policies
    {
        public string code { get; set; }
        public string detailCode { get; set; }
        public string value { get; set; }

        public policies()
        {
            code = "";
        }
    }
    #endregion

    #region rateplan classes
    public class RatePlanRequest
    {
        public string name { get; set; }
        public string rateAcquisitionType { get; set; }
        public List<distributionRules> distributionRules { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string pricingModel { get; set; }
        //public int occupantsForBaseRate { get; set; }
        public bool taxInclusive { get; set; }
        public int minLOSDefault { get; set; }
        public int maxLOSDefault { get; set; }
        public int minAdvBookDays { get; set; }
        public int maxAdvBookDays { get; set; }
        public string bookDateStart { get; set; }
        public string bookDateEnd { get; set; }
        public string travelDateStart { get; set; }
        public string travelDateEnd { get; set; }
        public bool mobileOnly { get; set; }
        public cancelPolicy cancelPolicy { get; set; }

        public RatePlanRequest()
        {
            name = "";
            rateAcquisitionType = "";
            distributionRules = new List<distributionRules>();
            status = "Active";
            type = "";
            pricingModel = "PerDayPricingByLengthOfStay";
            //occupantsForBaseRate = 0;
            taxInclusive = false;
            minLOSDefault = 1;
            maxLOSDefault = 28;
            minAdvBookDays = 0;
            maxAdvBookDays = 500;
            bookDateStart = "2018-06-04";
            bookDateEnd = "2079-01-01";
            travelDateStart = "2018-06-04";
            travelDateEnd = "2079-01-01";
            mobileOnly = false;
        }
    }

    public class cancelPolicy
    {
        public List<defaultPenalties> defaultPenalties { get; set; }
    }
    public class defaultPenalties
    {
        public int deadline { get; set; }
        public string perStayFee { get; set; }
        public decimal amount { get; set; }
    }
    public class RatePlanResponse
    {
        public RatePlanEntity entity { get; set; }
    }
    public class RatePlanEntity
    {
        public string resourceId { get; set; }
        public string name { get; set; }
        public string rateAcquisitionType { get; set; }
        public List<distributionRules> distributionRules { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string pricingModel { get; set; }
        public int occupantsForBaseRate { get; set; }
        public bool taxInclusive { get; set; }
        public int minLOSDefault { get; set; }
        public int maxLOSDefault { get; set; }
        public int minAdvBookDays { get; set; }
        public int maxAdvBookDays { get; set; }
        public string bookDateStart { get; set; }
        public string bookDateEnd { get; set; }
        public string travelDateStart { get; set; }
        public string travelDateEnd { get; set; }
        public bool mobileOnly { get; set; }
        public string creationDateTime { get; set; }
        public string lastUpdateDateTime { get; set; }
        public RatePlanEntity()
        {
            name = "";
            rateAcquisitionType = "";
            distributionRules = new List<distributionRules>();
            status = "Active";
            type = "";
            pricingModel = "PerDayPricingByLengthOfStay";
            occupantsForBaseRate = 0;
            taxInclusive = false;
            minLOSDefault = 1;
            maxLOSDefault = 28;
            minAdvBookDays = 0;
            maxAdvBookDays = 500;
            bookDateStart = "2018-06-04";
            bookDateEnd = "2079-01-01";
            travelDateStart = "2018-06-04";
            travelDateEnd = "2079-01-01";
            mobileOnly = false;
            creationDateTime = "";
            lastUpdateDateTime = "";
        }
    }
    public class distributionRules
    {
        //public string partnerCode {get;set;}
        //public string partnerCode {get;set;}
        public string distributionModel { get; set; }
        public string partnerCode { get; set; }

        public distributionRules()
        {
            distributionModel = "";
            partnerCode = "";
        }
    }
    #endregion
}
public class ChnlExpediaRoomClasses
{
    public class entity
    {
        public int resourceId { get; set; }
        public string partnerCode { get; set; }
        public name name { get; set; }
        public string status { get; set; }
        public List<RoomTypeAgeCategory> ageCategories { get; set; }
        public Occupancy maxOccupancy { get; set; }
        public List<standardBedding> standardBedding { get; set; }
        public List<string> smokingPreferences { get; set; }
        public roomSize roomSize { get; set; }
        //public _links _links { get; set; }

        public entity()
        {
            resourceId = 0;
            partnerCode = "";
            status = "";
            name = new name();
            ageCategories = new List<RoomTypeAgeCategory>();
            maxOccupancy = new Occupancy();
            smokingPreferences = new List<string>();
            roomSize = new roomSize();
            standardBedding = new List<standardBedding>();
        }
    }
    public class _links
    {
        public self self { get; set; }
    }
    public class self
    {
        public string href { get; set; }
    }

    #region romm classes
    public class RoomTypeRequest
    {
        //public int resourceId { get; set; }
        public string status { get; set; }
        public string partnerCode { get; set; }
        public name name { get; set; }
        public List<RoomTypeAgeCategory> ageCategories { get; set; }
        public Occupancy maxOccupancy { get; set; }
        public List<standardBedding> standardBedding { get; set; }
        public List<string> smokingPreferences { get; set; }
        public roomSize roomSize { get; set; }
        public RoomTypeRequest()
        {
            //resourceId = 0;
            partnerCode = "";
            name = new name();
            status = "";
            ageCategories = new List<RoomTypeAgeCategory>();
            maxOccupancy = new Occupancy();
            smokingPreferences = new List<string>();
            roomSize = new roomSize();
            standardBedding = new List<standardBedding>();
        }
    }
    public class GetStatusResponse
    {
        public StatusEntity entity { get; set; }
    }
    public class StatusEntity
    {
        public string provider { get; set; }
        public string providerPropertyId { get; set; }
        public int expediaId { get; set; }
        public string code { get; set; }
        //public string reasonCodes { get; set; }
        public string timestampUtc { get; set; }
        //public messages { get; set; }

        public StatusEntity()
        {

        }
    }
    public class RoomTypeUpdateRequest
    {
        public int resourceId { get; set; }
        public string status { get; set; }
        public string partnerCode { get; set; }
        public name name { get; set; }
        public List<RoomTypeAgeCategory> ageCategories { get; set; }
        public Occupancy maxOccupancy { get; set; }
        public List<standardBedding> standardBedding { get; set; }
        public List<string> smokingPreferences { get; set; }
        public roomSize roomSize { get; set; }
        public RoomTypeUpdateRequest()
        {
            //resourceId = 0;
            partnerCode = "";
            name = new name();
            status = "";
            ageCategories = new List<RoomTypeAgeCategory>();
            maxOccupancy = new Occupancy();
            smokingPreferences = new List<string>();
            roomSize = new roomSize();
            standardBedding = new List<standardBedding>();
        }
    }
    public class name
    {
        public attributes attributes { get; set; }
        //public string value { get; set; }

        public name()
        {
            attributes = new attributes();
            //value = "";
        }
    }
    public class RoomTypeAgeCategory
    {
        public string category { get; set; }
        public int minAge { get; set; }
    }
    public class attributes
    {
        public string typeOfRoom { get; set; }
    }
    public class Occupancy
    {
        public int adults { get; set; }
        //public int children { get; set; }
        public int total { get; set; }

        public Occupancy()
        {
            adults = 0;
            //children = 0;
            total = 0;
        }
    }
    public class roomSize
    {
        public int squareFeet { get; set; }
        public int squareMeters { get; set; }

        public roomSize()
        {
            squareFeet = 0;
            squareMeters = 0;
        }
    }
    public class standardBedding
    {
        public List<option> option { get; set; }
        public standardBedding()
        {
            option = new List<option>();
        }
    }
    public class option
    {
        public int quantity { get; set; }
        public string type { get; set; }

        public option()
        {
            quantity = 0;
            type = "";
        }
    }

    //response
    public class RoomTypeResponse
    {
        public entity entity { get; set; }
        public RoomTypeResponse()
        {
            entity = new entity();
        }
    }
    #endregion
}
public static class ChnlExpediaExts
{
    public static void isAvailableForPreviousDate(DateTime dtCurrent, int pidEstate, int minStay, ref List<ChnlExpediaClasses.AvailabilityListPerDates> avvTrackList)
    {
        try
        {
            DateTime dtEnd = dtCurrent.AddDays(-minStay);
            var last = avvTrackList.LastOrDefault();
            if ((last.DtEnd - last.DtStart).TotalDays.objToInt32() == last.minStay)
                return;
            else
            {
                int cnt = 0;
                DateTime dtTemp = dtCurrent.AddDays(-minStay);
                while (dtTemp < dtCurrent)
                {
                    var curr = avvTrackList.SingleOrDefault(x => x.DtStart <= dtTemp && x.DtEnd >= dtTemp);
                    bool check = (curr.pidEstate == 0 || curr.pidEstate == pidEstate);
                    if (!check)
                    {
                        cnt = cnt + 1;
                    }
                    dtTemp = dtTemp.AddDays(1);
                }
                if (cnt > 0)
                {
                    last.pidEstate = 0;
                    last.units = 0;
                }

                //foreach (var item in avvTrackList.OrderByDescending(x => x.DtStart))
                //{
                //    if (item.DtStart >= dtEnd)
                //    {
                //        item.pidEstate = 0;
                //        item.units = 0;
                //    }
                //}

            }

            return;
        }
        catch (Exception ex)
        {

            ChnlExpediaUtils.addLog("ERROR", "ChnlExpediaUpdate.UpdateAvailability_process isAvailableForPreviousDate:" + pidEstate, "dtCurrent " + dtCurrent.ToShortDateString(), ex.ToString(), ""); throw;
        }
        //var tmpCurrent = AvailabilityList.SingleOrDefault(x => x.date == dtCurrent);

    }
}
