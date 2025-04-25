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
using System.Threading;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ModAuth;
using HtmlAgilityPack;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using RentalInRome.data;

public class ChnlHomeAwayUtils_V411
{
    public static bool CheckForService()
    {
        if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Response == null) return false;
        var Request = HttpContext.Current.Request;
        var Response = HttpContext.Current.Response;
        if (!Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/")) return false;
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
        var requestType = "";
        var requestComments = "";
        var requestContent = xml;
        var responseContent = "";
        var requesUrl = "";
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/newavailabilityrequest"))
        {
            requestType = "newAvailabilityRequest";
            //String data = Encoding.UTF8.GetString(param);
            Response.ContentType = "application/json";
            responseContent = ChnlHomeAwayService_V411.newAvailabilityRequest(requestContent);
        }
        else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/availabilityrequest"))
        {
            requestType = "availabilityrequest";
            //String data = Encoding.UTF8.GetString(param);
            Response.ContentType = "application/json";
            responseContent = ChnlHomeAwayService_V411.AvailabilityRequest(requestContent);
        }
        else
        {
            if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/quoterequest"))
            {
                requestType = "quoterequest";
                responseContent = ChnlHomeAwayService_V411.QuoteRequest(xml);
            }

            if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/bookingrequest"))
            {
                requestType = "bookingrequest";
                responseContent = ChnlHomeAwayService_V411.BookingRequest(xml);
            }

            if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/bookings"))
            {
                requestType = "bookings";
                responseContent = ChnlHomeAwayService_V411.BookingContentIndexRequest(xml);
            }
            if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/booking/"))
            {
                var uid = Request.Url.LocalPath.ToLower().Replace("/chnlutils/homeaway_v4.1.1/booking/", "");
                var reservationTbl = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.unique_id.ToString().ToLower() == uid);
                if (reservationTbl != null)
                {
                    requestType = "booking/" + reservationTbl.id;
                    responseContent = ChnlHomeAwayService_V411.Booking(reservationTbl.id);
                }
            }
            if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/bookingupdate/"))
            {
                var uid = Request.Url.LocalPath.ToLower().Replace("/chnlutils/homeaway_v4.1.1/bookingupdate/", "");
                var reservationTbl = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.unique_id.ToString().ToLower() == uid);
                if (reservationTbl != null)
                {
                    requestType = "bookingupdate/" + reservationTbl.id;
                    responseContent = ChnlHomeAwayService_V411.BookingUpdate(reservationTbl.id);
                }
            }
            if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/advertiserlodgingrates"))
            {
                requestType = "advertiserlodgingrates";
                responseContent = ChnlHomeAwayService_V411.AdvertiserLodgingRateContentRequest();
            }
            else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/advertiserunitavailability"))
            {
                requestType = "listings-availability";
                responseContent = ChnlHomeAwayService_V411.AdvertiserUnitAvailabilityContentIndexRequest();
            }
            else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/listings-advertisercontents"))
            {
                requestType = "listings-advertisercontents";
                responseContent = ChnlHomeAwayService_V411.AdvertiserContentIndexRequest();
            }
            else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/advertisercontacts"))
            {
                requestType = "advertisercontacts";
                responseContent = ChnlHomeAwayService_V411.AdvertiserContactContentIndexRequest();
            }

            else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/listings"))
            {
                requestType = "listings";
                responseContent = ChnlHomeAwayService_V411.ListingContentIndexRequest();
            }

            if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/lodgingconfigurations"))
            {
                requestType = "lodgingconfigurations";
                responseContent = ChnlHomeAwayService_V411.LodgingConfigurationIndexRequest();
            }
            if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/listing/"))
            {
                var id = Request.Url.LocalPath.ToLower().Replace("/chnlutils/homeaway_v4.1.1/listing/", "").ToInt32();
                using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
                {
                    var chnlEstateTbl = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == id && (x.is_slave == null || x.is_slave == 0));
                    var estateTb = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == id);
                    if (estateTb != null && chnlEstateTbl != null)
                    {
                        requestType = "listing/" + estateTb.id;
                        responseContent = ChnlHomeAwayService_V411.Listing(estateTb.id);
                    }
                }
            }
            else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/advertisercontact/"))
            {
                var id = Request.Url.LocalPath.ToLower().Replace("/chnlutils/homeaway_v4.1.1/advertisercontact/", "").ToInt32();
                using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
                {
                    var chnlEstateTbl = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == id && (x.is_slave == null || x.is_slave == 0));
                    var estateTb = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == id);
                    if (estateTb != null && chnlEstateTbl != null)
                    {
                        requestType = "advertisercontact/" + estateTb.id;
                        responseContent = ChnlHomeAwayService_V411.Contact(estateTb.id);
                    }
                }
            }
            else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/lodgingratecontent/"))
            {
                var id = Request.Url.LocalPath.ToLower().Replace("/chnlutils/homeaway_v4.1.1/lodgingratecontent/", "").ToInt32();
                using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
                {
                    var chnlEstateTbl = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == id);
                    var estateTb = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == id);
                    if (estateTb != null && chnlEstateTbl != null)
                    {
                        requestType = "lodgingratecontent/" + estateTb.id;
                        responseContent = ChnlHomeAwayService_V411.LodgingRateContent(estateTb.id);
                    }
                }
            }

            else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/unitavailabilitycontent/"))
            {
                var id = Request.Url.LocalPath.ToLower().Replace("/chnlutils/homeaway_v4.1.1/unitavailabilitycontent/", "").ToInt32();
                using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
                {
                    var chnlEstateTbl = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == id);
                    var estateTb = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == id);
                    if (estateTb != null && chnlEstateTbl != null)
                    {
                        requestType = "unitavailabilitycontent/" + estateTb.id;
                        responseContent = ChnlHomeAwayService_V411.ListingAvailability(estateTb.id);

                        //TESTING UPDATE SERVICE
                        //ChnlHomeAwayUpdateService.UnitAvailabilityUpdate_start(Convert.ToInt32(estateTb.id));

                    }
                }
            }
            else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/homeaway_v4.1.1/lodgingconfigurationcontent/"))
            {
                var id = Request.Url.LocalPath.ToLower().Replace("/chnlutils/homeaway_v4.1.1/lodgingconfigurationcontent/", "").ToInt32();
                using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
                {
                    var chnlEstateTbl = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == id);
                    var estateTb = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == id);
                    if (estateTb != null && chnlEstateTbl != null)
                    {
                        requestType = "lodgingconfigurationcontent/" + estateTb.id;
                        responseContent = ChnlHomeAwayService_V411.LodgingConfigurationContent(estateTb.id);
                    }
                }
            }

        }

        if (CommonUtilities.getSYS_SETTING("rntChnlHomeAwayDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlHomeAwayDebug").ToInt32() == 1)
            addLog(requestType, requestComments, requestContent, responseContent, requesUrl);
        Response.Write(responseContent);
        Response.End();
        return true;
    }

    public static void SetTimers()
    {
        timerChnlHomeAwayImport = new System.Timers.Timer();
        timerChnlHomeAwayImport.Interval = (1000 * 60 * 2); // first after 2 mins
        timerChnlHomeAwayImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlHomeAwayImport_ElapsedFirstTime);
        timerChnlHomeAwayImport.Start();

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
            if (timerChnlHomeAwayImport != null)
            {
                timerChnlHomeAwayImport.Stop();
                timerChnlHomeAwayImport.Dispose();
            }

            if (timerClearLog != null)
            {
                timerClearLog.Stop();
                timerClearLog.Dispose();
            }
        }
        catch (Exception ex)
        {
            ErrorLog.addLog("", "ChnlHomeAwayUtils.StopTimers", ex.ToString() + "");
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
                using (magaChnlHomeAwayDataContext dc = maga_DataContext.DC_HOME)
                {
                    dc.RntChnlHomeAwayRequestLOG.DeleteAllOnSubmit(dc.RntChnlHomeAwayRequestLOG.Where(x => x.logDateTime <= dt));
                    dc.SubmitChanges();
                }
                addLog("CLEAR LOG", "till " + dt, "", "", "");
            }
            catch (Exception Ex)
            {
                ErrorLog.addLog("", "ChnlHomeAwayUtils.ClearLog_process", Ex.ToString() + "");
            }
        }
        public ClearLog_process()
        {
            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            Thread.Sleep(1000);

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler(action, "ChnlHomeAwayUtils.ClearLog_process");
        }
    }
    public static void ClearLog_start()
    {
        ClearLog_process _tmp = new ClearLog_process();
    }

    static System.Timers.Timer timerChnlHomeAwayImport;
    static void timerChnlHomeAwayImport_ElapsedFirstTime(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        if (RentalInRome.Global.isOnDev && CommonUtilities.getSYS_SETTING("rntChnlHomeAwayImportOnDev") != "true" && CommonUtilities.getSYS_SETTING("rntChnlHomeAwayImportOnDev").ToInt32() != 1) return;
        int rntChnlHomeAwayImportEachMins = CommonUtilities.getSYS_SETTING("rntChnlHomeAwayImportEachMins").ToInt32();
        if (rntChnlHomeAwayImportEachMins == 0) rntChnlHomeAwayImportEachMins = 15;  // each 15 mins default
        ChnlHomeAwayImport_V411.Enquiries_all(ChnlHomeAwayProps_V411.TimePeriods.PAST_TWENTY_FOUR_HOURS);
        timerChnlHomeAwayImport.Dispose();
        timerChnlHomeAwayImport = new System.Timers.Timer();
        timerChnlHomeAwayImport.Interval = (1000 * 60 * rntChnlHomeAwayImportEachMins);
        timerChnlHomeAwayImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlHomeAwayImport_Elapsed);
        timerChnlHomeAwayImport.Start();
    }
    static void timerChnlHomeAwayImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        if (RentalInRome.Global.isOnDev && CommonUtilities.getSYS_SETTING("rntChnlHomeAwayImportOnDev") != "true" && CommonUtilities.getSYS_SETTING("rntChnlHomeAwayImportOnDev").ToInt32() != 1) return;
        int rntChnlHomeAwayImportEachMins = CommonUtilities.getSYS_SETTING("rntChnlHomeAwayImportEachMins").ToInt32();
        if (rntChnlHomeAwayImportEachMins == 0) rntChnlHomeAwayImportEachMins = 15;  // each 15 mins default
        ChnlHomeAwayImport_V411.Enquiries_all(ChnlHomeAwayProps_V411.TimePeriods.PAST_THIRTY_MINUTES);
        timerChnlHomeAwayImport.Dispose();
        timerChnlHomeAwayImport = new System.Timers.Timer();
        timerChnlHomeAwayImport.Interval = (1000 * 60 * rntChnlHomeAwayImportEachMins);
        timerChnlHomeAwayImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlHomeAwayImport_Elapsed);
        timerChnlHomeAwayImport.Start();
    }
    public static void addLog(string requestType, string requestComments, string requestContent, string responseContent, string requesUrl)
    {
        try
        {
            using (magaChnlHomeAwayDataContext dc = maga_DataContext.DC_HOME)
            {
                var item = new RntChnlHomeAwayRequestLOG();
                item.uid = Guid.NewGuid();
                item.requesUrl = requesUrl;
                item.requestType = requestType;
                item.requestComments = requestComments;
                item.requestContent = requestContent;
                item.responseContent = responseContent;
                item.logDateTime = DateTime.Now;
                dc.RntChnlHomeAwayRequestLOG.InsertOnSubmit(item);
                dc.SubmitChanges();
            }
        }
        catch (Exception Ex)
        {
            ErrorLog.addLog("", "ChnlHomeAwayUtils.addLog", Ex.ToString() + "");
        }
    }

    public static string SendUpdateRequest(String requesUrl, String requestContent, out string ErrorString)
    {
        ErrorString = "";
        try
        {
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            obj.ContentType = "application/xml";

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

            //set new TLS protocol 1.1
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            String strRSString = _Answer.ReadToEnd().ToString();
            if (CommonUtilities.getSYS_SETTING("rntChnlHomeAwayDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlHomeAwayDebug").ToInt32() == 1)
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

    public static string SendRequest(String requesUrl, String requestContent, out string ErrorString)
    {
        ErrorString = "";
        try
        {
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            obj.ContentType = "application/xml";

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

            //set new TLS protocol 1.1
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            String strRSString = _Answer.ReadToEnd().ToString();
            if (CommonUtilities.getSYS_SETTING("rntChnlHomeAwayDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlHomeAwayDebug").ToInt32() == 1)
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

    public static List<RntChnlHomeAwayEstateRoomTB> getRooms(RNT_TB_ESTATE currEstate, string roomType)
    {
        List<RntChnlHomeAwayEstateRoomTB> rooms = new List<RntChnlHomeAwayEstateRoomTB>();
        if (roomType == "Bathroom")
        {
            for (int i = 1; i <= currEstate.num_rooms_bath.objToInt32(); i++)
            {
                var currTbl = new RntChnlHomeAwayEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "FULL_BATH";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
            if (rooms.Count == 0)
            {
                var currTbl = new RntChnlHomeAwayEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "SHOWER_INDOOR_OR_OUTDOOR";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
        }
        if (roomType == "Bedroom")
        {
            for (int i = 1; i <= currEstate.num_rooms_bed.objToInt32(); i++)
            {
                var currTbl = new RntChnlHomeAwayEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "BEDROOM";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
            if (rooms.Count == 0)
            {
                var currTbl = new RntChnlHomeAwayEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "LIVING_SLEEPING_COMBO";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
        }
        return rooms;
    }
    public static List<dbRntChnlHomeAwayEstateRoomTB> getRooms1(RNT_TB_ESTATE currEstate, string roomType)
    {
        List<dbRntChnlHomeAwayEstateRoomTB> rooms = new List<dbRntChnlHomeAwayEstateRoomTB>();
        if (roomType == "Bathroom")
        {
            for (int i = 1; i <= currEstate.num_rooms_bath.objToInt32(); i++)
            {
                var currTbl = new dbRntChnlHomeAwayEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "FULL_BATH";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
            if (rooms.Count == 0)
            {
                var currTbl = new dbRntChnlHomeAwayEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "SHOWER_INDOOR_OR_OUTDOOR";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
        }
        if (roomType == "Bedroom")
        {
            for (int i = 1; i <= currEstate.num_rooms_bed.objToInt32(); i++)
            {
                var currTbl = new dbRntChnlHomeAwayEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "BEDROOM";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
            if (rooms.Count == 0)
            {
                var currTbl = new dbRntChnlHomeAwayEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "LIVING_SLEEPING_COMBO";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
        }
        return rooms;
    }
    public static void updateEstateFromMagarental(RNT_TB_ESTATE currEstate)
    {
        if (string.IsNullOrEmpty(currEstate.haAdvertiserId) || currEstate.is_HomeAway.objToInt32() == 0)
            return;

        using (var dcRntOld = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        {
            int IdEstate = currEstate.id;
            var currHomeaway = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
            if (currHomeaway == null)
            {
                currHomeaway = new RntChnlHomeAwayEstateTB() { id = IdEstate };
                dcChnl.RntChnlHomeAwayEstateTB.InsertOnSubmit(currHomeaway);
                dcChnl.SubmitChanges();
                currHomeaway.is_active = 1;
                currHomeaway.is_slave = 0;
                dcChnl.SubmitChanges();
            }
            currHomeaway.mq_inner = currEstate.mq_inner;
            currHomeaway.mq_inner_unit = "METERS_SQUARED";
            currHomeaway.num_max_sleep = currEstate.num_persons_max;
            currHomeaway.propertyType = currEstate.haPropertyType;
            dcChnl.SubmitChanges();

            List<RNT_LN_ESTATE> lstEstate = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == IdEstate).ToList();
            if (lstEstate != null && lstEstate.Count > 0)
            {
                foreach (RNT_LN_ESTATE objEstate in lstEstate)
                {
                    var pid_lang = contUtils.getLang_code(objEstate.pid_lang);
                    RntChnlHomeAwayEstateLN currHomeawayLN = dcChnl.RntChnlHomeAwayEstateLN.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == pid_lang);
                    if (currHomeawayLN == null)
                    {
                        currHomeawayLN = new RntChnlHomeAwayEstateLN();
                        currHomeawayLN.pid_estate = currHomeaway.id;
                        currHomeawayLN.pid_lang = pid_lang;
                        dcChnl.RntChnlHomeAwayEstateLN.InsertOnSubmit(currHomeawayLN);
                        dcChnl.SubmitChanges();
                    }
                    currHomeawayLN.title = !string.IsNullOrEmpty(objEstate.haHeadLine) ? objEstate.haHeadLine : objEstate.title;
                    currHomeawayLN.unit_name = objEstate.title;
                    currHomeawayLN.summary = objEstate.summary;
                    currHomeawayLN.unit_description = objEstate.description;
                    currHomeawayLN.description = !string.IsNullOrEmpty(objEstate.haDescription) ? objEstate.haDescription : objEstate.description;
                    currHomeawayLN.price_note = objEstate.haRateNote;
                    currHomeawayLN.location_other_activities = objEstate.haOtherActivities;
                    dcChnl.SubmitChanges();
                }
            }
            var extrasIds = dcRntOld.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.is_HomeAway == 0).Select(x => x.pid_config.ToString()).ToList();
            foreach (var extrasId in extrasIds)
            {
                var featureValuesTbl = dcChnl.RntChnlHomeAwayLkFeatureValuesTBL.FirstOrDefault(x => x.refType == "Extras" && x.refId == extrasId);
                if (featureValuesTbl != null)
                {
                    var currRl = dcChnl.RntChnlHomeAwayEstateFeaturesRL.FirstOrDefault(x => x.pidEstate == IdEstate && x.type == featureValuesTbl.type && x.code == featureValuesTbl.code);
                    if (currRl == null)
                    {
                        currRl = new RntChnlHomeAwayEstateFeaturesRL();
                        currRl.pidEstate = IdEstate;
                        currRl.type = featureValuesTbl.type;
                        currRl.code = featureValuesTbl.code;
                        dcChnl.RntChnlHomeAwayEstateFeaturesRL.InsertOnSubmit(currRl);
                        dcChnl.SubmitChanges();
                    }
                }
            }
        }
    }



    public static bool CheckAvailability(RNT_TB_ESTATE currEstateTB, DateTime dtStart, DateTime dtEnd, long agentID)
    {
        bool _isAvailable = false;
        var resList = rntUtils.rntEstate_resList(currEstateTB.id, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
        bool isAnyReseration = resList.Any(x => x.agentID == agentID);
        _isAvailable = !isAnyReseration;
        if (_isAvailable)
        {
            _isAvailable = CheckAvailabilityByBaseAvv(currEstateTB, dtStart, dtEnd, resList);
        }

        return _isAvailable;
    }

    public static bool CheckAvailabilityByBaseAvv(RNT_TB_ESTATE currEstateTB, DateTime dtStart, DateTime dtEnd, List<RNT_TBL_RESERVATION> resList)
    {
        bool _isAvailable = false;

        List<int> roomNumbers = new List<int>();
        int numRooms = currEstateTB.baseAvailability.objToInt32();
        for (int i = 1; i <= numRooms; i++)
        {
            roomNumbers.Add(i);
        }

        List<int> avvList = new List<int>();
        DateTime dtCurrent = dtStart;
        while (dtCurrent < dtEnd)
        {
            //int removedBaseAvv = 0;           
            int baseAvv = currEstateTB.baseAvailability.objToInt32() < 1 ? 1 : currEstateTB.baseAvailability.objToInt32();
            //baseAvv = baseAvv - removedBaseAvv;
            var currResList = resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count();
            var units = baseAvv - currResList;
            avvList.Add(units);
            dtCurrent = dtCurrent.AddDays(1);
        }
        int finalUnits = 0;
        //foreach (int i in avvList)
        //{
        //    ErrorLog.addLog("test", "count" + i, "");
        //}
        if (avvList != null && avvList.Count() > 0)
        {
            finalUnits = avvList.Max(x => x.objToInt32());
            // ErrorLog.addLog("", "GetQuoteRequest_CA_CheckAvailabilityByBaseAvv", currEstateTB.id + "finalUnits Count" + finalUnits);
        }

        _isAvailable = finalUnits > 0;
        //ErrorLog.addLog("", "GetQuoteRequest_CA_CheckAvailabilityByBaseAvv", currEstateTB.id + "finalUnits Count_isAvailable" + finalUnits);
        return _isAvailable;
    }
}
public class FeatureItem_V411
{
    public string Code { get; set; }
    public int Count { get; set; }
    public FeatureItem_V411(string code, int count)
    {
        Code = code;
        Count = count;
    }
    public FeatureItem_V411(string featuresString)
    {
        if (featuresString.splitStringToList("%").Count == 2)
        {
            Code = featuresString.splitStringToList("%")[0];
            Count = featuresString.splitStringToList("%")[1].ToInt32();
        }
        else
        {
            Code = "";
            Count = 0;
        }
    }
    public string getString()
    {
        return Code + "%" + Count;
    }
}
public static class ChnlHomeAwayProps_V411
{
    public static string IdAdMedia = "HA";
    public static string IdManager = "MAGARENTAL_LIVE";
    public static class TimePeriods
    {
        public static string PAST_FIFTEEN_MINUTES = "PAST_FIFTEEN_MINUTES";
        public static string PAST_TWENTY_FOUR_HOURS = "PAST_TWENTY_FOUR_HOURS";
        public static string PAST_THIRTY_MINUTES = "PAST_THIRTY_MINUTES";
        public static string PAST_THREE_DAYS = "PAST_THREE_DAYS";
        public static string PAST_HOUR = "PAST_HOUR";
        public static string PAST_SEVEN_DAYS = "PAST_SEVEN_DAYS";
        public static string PAST_FOUR_HOURS = "PAST_FOUR_HOURS";
    }
}
public class ChnlHomeAwayImport_V411
{
    private class Enquiries_process
    {
        string AdvertiserAssignedId { get; set; }
        public long agentID { get; set; }
        string TimePeriod { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        private string requestComments;
        void doThread()
        {
            try
            {
                using (magaChnlHomeAwayDataContext dc = maga_DataContext.DC_HOME)
                using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
                using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
                {
                    var managerTbl = ModAppServerCommon.utils.getChnlHomeAwayManager(ChnlHomeAwayProps_V411.IdManager);
                    if (managerTbl == null) return;
                    var agentTbl = dcRnt.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0) return;

                    requesUrl = "https://integration.homeaway.com/services/external/inquiries/3.1";
                    if (managerTbl.isDemo == 1)
                        requesUrl = "https://integration-stage.homeaway.com/services/external/inquiries/3.1";

                    ChnlHomeAwayClasses_V411.InquirySearchRequest Request = new ChnlHomeAwayClasses_V411.InquirySearchRequest();
                    Request.assignedSystemId = managerTbl.assignedSystemId;
                    Request.advertiserAssignedId = AdvertiserAssignedId;
                    Request.timePeriod = TimePeriod;
                    Request.authorizationToken = managerTbl.authorizationToken;
                    string requestContent = Request.GetXml() + "";
                    string tmpErrorString = "";
                    string responseData = ChnlHomeAwayUtils_V411.SendRequest(requesUrl, requestContent, out tmpErrorString);
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                    {
                        requestComments = "ERROR: empty response";
                        ChnlHomeAwayUtils_V411.addLog("  ChnlHomeAwayImport.Enquiries_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);
                        return;
                    }
                    ChnlHomeAwayClasses_V411.InquirySearchResponse Response = new ChnlHomeAwayClasses_V411.InquirySearchResponse(responseData);
                    var advertiser = Response.advertisers.FirstOrDefault();
                    if (advertiser == null)
                    {
                        requestComments = "ERROR: empty response";
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayImport.Enquiries_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);
                        return;
                    }
                    int countCreated = 0;
                    int countNoProperty = 0;
                    int countUpdated = 0;
                    foreach (var inquiry in advertiser.inquiries)
                    {
                        int IdEstate = inquiry.unitExternalId.ToInt32();
                        if (IdEstate == 0) IdEstate = inquiry.listingExternalId.ToInt32();
                        RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                        if (currEstate == null)
                        {
                            countNoProperty++;
                            continue;
                        }
                        var _request = DC_RENTAL.RNT_TBL_REQUEST.FirstOrDefault(x => x.chnlRefId == inquiry.inquiryId && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                        if (_request == null)
                        {
                            _request = new RNT_TBL_REQUEST();
                            _request.IdAdMedia = ChnlHomeAwayProps_V411.IdAdMedia;
                            _request.IdLink = inquiry.listingChannel;
                            _request.chnlRefId = inquiry.inquiryId;
                            _request.pid_lang = contUtils.getLangIdFromLocale(inquiry.inquirer.locale);
                            var country_id = 0;
                            var country_title = "";
                            var currCountry = authProps.CountryLK.FirstOrDefault(x => x.code == inquiry.inquirer.address.country);
                            if (currCountry != null)
                            {
                                country_id = currCountry.id;
                                country_title = currCountry.title;
                            }
                            _request.request_country = country_title;
                            _request.request_date_created = DateTime.Now;
                            _request.state_date = DateTime.Now;
                            _request.state_pid = 1;
                            _request.state_subject = "Creata Richiesta";
                            _request.state_pid_user = 1;
                            _request.request_ip = "";
                            _request.pid_creator = 1;
                            _request.pid_city = 0;
                            //_request.request_choices = "1" + " - " + CurrentSource.rntEstate_title(id.objToInt32(), _request.pid_lang, "");
                            DC_RENTAL.RNT_TBL_REQUEST.InsertOnSubmit(_request);
                            DC_RENTAL.SubmitChanges();
                            RNT_RL_REQUEST_ITEM _item = new RNT_RL_REQUEST_ITEM();
                            _item.pid_estate = IdEstate;
                            _item.pid_request = _request.id;
                            _item.sequence = 1;
                            DC_RENTAL.RNT_RL_REQUEST_ITEMs.InsertOnSubmit(_item);
                            DC_RENTAL.SubmitChanges();
                            string _mSubject = "";
                            _request.pid_related_request = 0;
                            RNT_TBL_REQUEST _relatedRequest = rntUtils.rntRequest_getRelatedRequest(_request);
                            if (_relatedRequest != null)
                            {
                                _request.pid_related_request = _relatedRequest.id;
                                _request.pid_operator = _relatedRequest.pid_operator.Value;
                                rntUtils.rntRequest_addState(_request.id, 0, 1, "Correlazione alla richiesta Primaria rif. " + _relatedRequest.id, "");
                                rntUtils.rntRequest_addState(_relatedRequest.id, 0, 1, "Aggiunta Correlazione alla richiesta Secondaria rif. " + _request.id, "");
                            }
                            else
                            {
                                _request.pid_related_request = 0;
                                _request.pid_operator = AdminUtilities.usr_getAvailableOperator(country_id, _request.pid_lang.objToInt32());
                            }
                            DC_RENTAL.SubmitChanges();
                            countCreated++;
                            rntUtils.rntRequest_addState(_request.id, _request.state_pid.Value, _request.state_pid_user.Value, _request.state_subject, "");
                        }
                        else
                        {
                            countUpdated++;
                        }
                        _request.name_full = string.IsNullOrEmpty(inquiry.inquirer.name) ? inquiry.inquirer.firstName + " " + inquiry.inquirer.lastName : inquiry.inquirer.name;
                        _request.phone = inquiry.inquirer.phoneNumber;
                        _request.email = inquiry.inquirer.emailAddress;
                        _request.request_date_start = inquiry.reservation.reservationDates.beginDate.ValueDate;
                        _request.request_date_end = inquiry.reservation.reservationDates.endDate.ValueDate;
                        _request.request_adult_num = inquiry.reservation.numberOfAdults;
                        _request.request_child_num = inquiry.reservation.numberOfChildren;
                        _request.request_transport = "";
                        _request.request_notes = inquiry.message;
                        DC_RENTAL.SubmitChanges();
                    }
                    requestComments = "IMPORTED countCreated:" + countCreated + ", countUpdated:" + countUpdated + ", countNoProperty:" + countNoProperty;
                    ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayImport.Enquiries_process", requestComments, requestContent, responseData, requesUrl);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHomeAwayImport.Enquiries_process advertiserAssignedId:" + AdvertiserAssignedId, ex.ToString());
            }
        }

        public Enquiries_process(string advertiserAssignedId, string timePeriod)
        {
            AdvertiserAssignedId = advertiserAssignedId;
            TimePeriod = timePeriod;
            Host = App.HOST;
            ErrorString = "";

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            Thread.Sleep(1000);

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHomeAwayImport.Enquiries_process advertiserAssignedId:" + advertiserAssignedId);
        }
    }
    public static string Enquiries_start(string advertiserAssignedId, string timePeriod)
    {
        Enquiries_process _tmp = new Enquiries_process(advertiserAssignedId, timePeriod);
        return _tmp.ErrorString;
    }
    public static void Enquiries_all(string timePeriod)
    {
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        {
            var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
            if (ownerTbl == null) return;
            if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId)) Enquiries_start(ownerTbl.advertiserAssignedId, timePeriod);
            if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId)) Enquiries_start(ownerTbl.ppb_advertiserAssignedId, timePeriod);
        }
    }
}

public class ChnlHomeAwayUpdateService_V411
{

    private class LodgingRateUpdate_process
    {
        static string AdvertiserAssignedId { get; set; }
        public static long agentID { get; set; }
        public string ErrorString { get; set; }
        public static string requesUrl { get; set; }
        private static string requestComments;
        public static string authorizationToken_Stage = "7cca8087-4853-466e-a9e7-85f9cb9e399e";
        public int estateId { get; set; }
        void doThread()
        {
            try
            {
                using (magaChnlHomeAwayDataContext dc = maga_DataContext.DC_HOME)
                using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
                using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
                {
                    var managerTbl = ModAppServerCommon.utils.getChnlHomeAwayManager(ChnlHomeAwayProps_V411.IdManager);
                    if (managerTbl == null) return;
                    var agentTbl = dcRnt.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0) return;

                    //requesUrl = "https://integration.homeaway.com/services/external/lodgingRateUpdate/haxmlListingsVersion/4.1.1";
                    //if (managerTbl.isDemo == 1)
                    //{
                    //managerTbl.authorizationToken = authorizationToken_Stage; // Authorization token for Stage
                    requesUrl = "https://integration-stage.homeaway.com/services/external/lodgingRateUpdate/haxmlListingsVersion/4.1.1";
                    //}


                    ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateUpdateRequest RequestData = ChnlHomeAwayService_V411.LodgingRateUpdateRequestContent(estateId);
                    if (RequestData == null)
                    {
                        requestComments = "ERROR: LodgingRate Detail not available";
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayUpdateService_V411.LodgingRateUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, null, null, requesUrl);

                        return;
                    }
                    ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateUpdateRequest Request = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateUpdateRequest();
                    Request.assignedSystemId = managerTbl.assignedSystemId;
                    Request.advertiserAssignedId = AdvertiserAssignedId;
                    Request.authorizationToken = authorizationToken_Stage; // managerTbl.authorizationToken;
                    Request.listingExternalId = RequestData.listingExternalId;
                    Request.unitExternalId = RequestData.unitExternalId;
                    Request.lodgingRate = RequestData.lodgingRate;

                    string requestContent = Request.GetXml() + "";
                    string tmpErrorString = "";
                    string responseData = ChnlHomeAwayUtils_V411.SendUpdateRequest(requesUrl, requestContent, out tmpErrorString);
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                    {
                        requestComments = "ERROR: empty response";
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayUpdateService_V411.LodgingRateUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);
                        return;
                    }
                    ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateUpdateResponse lodgingRateUpdateResponse = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateUpdateResponse(responseData);
                    var Reponse = lodgingRateUpdateResponse.Reponse;
                    if (Reponse == null)
                    {
                        requestComments = "ERROR: empty response";
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayUpdateService_V411.LodgingRateUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);
                        return;
                    }
                    if (!string.IsNullOrEmpty(Reponse.status))
                    {
                        requestComments = "RESPONSE STATUS:" + Reponse.status;
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayUpdateService_V411.LodgingRateUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);

                    }
                    if (!string.IsNullOrEmpty(Reponse.action))
                    {
                        requestComments = "RESPONSE ACTION:" + Reponse.action;
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayUpdateService_V411.LodgingRateUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);

                    }
                    if (Reponse.errors != null && Reponse.errors.Count > 0)
                    {
                        foreach (ChnlHomeAwayClasses_V411.ListingClasses.UpdateServiceError _error in Reponse.errors)
                        {
                            requestComments = "ERROR:     ERRORCODE: " + _error.errorCode + "    ERROR MESSAGE:" + _error.message;
                            ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayUpdateService_V411.LodgingRateUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);

                        }
                        return;
                    }
                    requestComments = "LodgingRateUpdate_process Finished";
                    ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayUpdateService_V411.LodgingRateUpdate_process", requestComments, requestContent, responseData, requesUrl);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHomeAwayUpdateService_V411.LodgingRateUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, ex.ToString());
            }
        }

        public LodgingRateUpdate_process(string advertiserAssignedId, int EstateId)
        {
            AdvertiserAssignedId = advertiserAssignedId;
            estateId = EstateId;
            ErrorString = "";

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            Thread.Sleep(1000);

            //Action<object> action = (object obj) => { doThread(estateId);  };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHomeAwayUpdateService_V411.LodgingRateUpdate_process advertiserAssignedId:" + advertiserAssignedId);
        }

    }

    public static string LodgingRateUpdate_start(int estateId)
    {
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        {
            string advertiserAssignedId = "";
            var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
            if (ownerTbl == null) return "";
            if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId))
            {
                advertiserAssignedId = ownerTbl.advertiserAssignedId;
            }
            if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
            {
                advertiserAssignedId = ownerTbl.ppb_advertiserAssignedId;
            }
            if (string.IsNullOrEmpty(advertiserAssignedId))
            {
                return "";
            }
            LodgingRateUpdate_process _tmp = new LodgingRateUpdate_process(advertiserAssignedId, estateId);
            return _tmp.ErrorString;
        }
    }

    private class LodgingUnitAvailabilityUpdate_process
    {
        static string AdvertiserAssignedId { get; set; }
        public static long agentID { get; set; }
        public string ErrorString { get; set; }
        public static string requesUrl { get; set; }
        private static string requestComments;
        public int estateId { get; set; }
        public static string authorizationToken_Stage = "7cca8087-4853-466e-a9e7-85f9cb9e399e";

        void doThread()
        {
            try
            {
                using (magaChnlHomeAwayDataContext dc = maga_DataContext.DC_HOME)
                using (magaRental_DataContext dcRnt = maga_DataContext.DC_RENTAL)
                using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
                {
                    var managerTbl = ModAppServerCommon.utils.getChnlHomeAwayManager(ChnlHomeAwayProps_V411.IdManager);
                    if (managerTbl == null) return;
                    var agentTbl = dcRnt.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0) return;

                    //requesUrl = "https://integration.homeaway.com/services/external/unitAvailabilityUpdate/haxmlListingsVersion/4.1.1";
                    //if (managerTbl.isDemo == 1)
                    //{
                    //managerTbl.authorizationToken = authorizationToken_Stage; // Authorization token for Stage
                    requesUrl = "https://integration-stage.homeaway.com/services/external/unitAvailabilityUpdate/haxmlListingsVersion/4.1.1";

                    //}

                    ChnlHomeAwayClasses_V411.ListingClasses.LodgingUnitAvailabilityUpdateRequest RequestData = ChnlHomeAwayService_V411.LodgingAvailabilityUpdateRequestContent(estateId);
                    if (RequestData == null)
                    {
                        requestComments = "ERROR: LodgingUnitAvailability Detail not available";
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayImport.LodgingUnitAvailabilityUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, null, null, requesUrl);
                        return;
                    }
                    ChnlHomeAwayClasses_V411.ListingClasses.LodgingUnitAvailabilityUpdateRequest Request = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingUnitAvailabilityUpdateRequest();
                    Request.assignedSystemId = managerTbl.assignedSystemId;
                    Request.advertiserAssignedId = AdvertiserAssignedId;
                    Request.authorizationToken = authorizationToken_Stage;//managerTbl.authorizationToken;
                    Request.listingExternalId = RequestData.listingExternalId;
                    Request.unitExternalId = RequestData.unitExternalId;
                    Request.unitAvailability = RequestData.unitAvailability;

                    string requestContent = Request.GetXml() + "";
                    string tmpErrorString = "";
                    string responseData = ChnlHomeAwayUtils_V411.SendUpdateRequest(requesUrl, requestContent, out tmpErrorString);
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                    {
                        requestComments = "ERROR: empty response";
                        ChnlHomeAwayUtils_V411.addLog("  ChnlHomeAwayImport.LodgingUnitAvailabilityUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);
                        return;
                    }
                    ChnlHomeAwayClasses_V411.ListingClasses.LodgingUnitAvailabilityUpdateResponse lodgingUnitAvailabilityUpdateResponse = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingUnitAvailabilityUpdateResponse(responseData);
                    var Reponse = lodgingUnitAvailabilityUpdateResponse.Reponse;
                    if (Reponse == null)
                    {
                        requestComments = "ERROR: empty response";
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayImport.LodgingUnitAvailabilityUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);
                        return;
                    }
                    if (!string.IsNullOrEmpty(Reponse.status))
                    {
                        requestComments = "RESPONSE STATUS:" + Reponse.status;
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayImport.LodgingUnitAvailabilityUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);

                    }
                    if (!string.IsNullOrEmpty(Reponse.action))
                    {
                        requestComments = "RESPONSE ACTION:" + Reponse.action;
                        ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayImport.LodgingUnitAvailabilityUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);

                    }
                    if (Reponse.errors != null && Reponse.errors.Count > 0)
                    {
                        foreach (ChnlHomeAwayClasses_V411.ListingClasses.UpdateServiceError _error in Reponse.errors)
                        {
                            requestComments = "ERROR:     ERRORCODE: " + _error.errorCode + "    ERROR MESSAGE:" + _error.message;
                            ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayImport.LodgingUnitAvailabilityUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);

                        }
                        return;
                    }
                    requestComments = "LodgingUnitAvailabilityUpdate_process Finished";
                    ChnlHomeAwayUtils_V411.addLog("ChnlHomeAwayImport.LodgingUnitAvailabilityUpdate_process", requestComments, requestContent, responseData, requesUrl);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHomeAwayUpdateService_V411.LodgingUnitAvailabilityUpdate_process advertiserAssignedId:" + AdvertiserAssignedId, ex.ToString());
            }
        }

        public LodgingUnitAvailabilityUpdate_process(string advertiserAssignedId, int EstateId)
        {
            AdvertiserAssignedId = advertiserAssignedId;
            estateId = EstateId;
            ErrorString = "";

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            Thread.Sleep(1000);

            //Action<object> action = (object obj) => { doThread(estateId); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHomeAwayImport.LodgingUnitAvailabilityUpdate_process advertiserAssignedId:" + advertiserAssignedId);
        }

    }

    public static string UnitAvailabilityUpdate_start(int estateId)
    {
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        {
            string advertiserAssignedId = "";
            var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
            if (ownerTbl == null) return "";
            if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId))
            {
                advertiserAssignedId = ownerTbl.advertiserAssignedId;
            }
            if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
            {
                advertiserAssignedId = ownerTbl.ppb_advertiserAssignedId;
            }
            if (string.IsNullOrEmpty(advertiserAssignedId))
            {
                return "";
            }
            LodgingUnitAvailabilityUpdate_process _tmp = new LodgingUnitAvailabilityUpdate_process(advertiserAssignedId, estateId);
            return _tmp.ErrorString;
        }


    }
}

public class ChnlHomeAwayService_V411
{
    private static decimal getPriceWithoutTax(decimal amount, decimal taxPercentage)
    {
        decimal TMPcashTaxFreeDecimal = Decimal.Divide(amount, (1 + taxPercentage));
        decimal TMPcashTaxFreeRounded = Decimal.Round(TMPcashTaxFreeDecimal, 2);
        if (TMPcashTaxFreeRounded > TMPcashTaxFreeDecimal)
            TMPcashTaxFreeRounded -= new Decimal(0.01);
        return TMPcashTaxFreeRounded;
    }
    public static string ListingContentIndexRequest()
    {

        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("AdvertiserlistingContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "ListingContentIndexRequest", "Owner for HomeAway was not found or not active");
                    return "";
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    errorList.AddError("OTHER");
                    ErrorLog.addLog("", "AdvertiserlistingContentIndex", "Agent for HomeAway was not found or not active");
                    return "";
                }
                var estateIds = new List<int?>();
                var estateList = new List<AppSettings.RNT_estate>();
                ChnlHomeAwayClasses_V411.AdvertiserListingContentIndexResponse Response = new ChnlHomeAwayClasses_V411.AdvertiserListingContentIndexResponse();
                //List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                //List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                //estateIds = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == null || x.is_slave == 0)).Select(x => (int?)x.id).ToList();
                //var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();


                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    Response.Advertiser.assignedId = ownerTbl.ppb_advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.ListingContentIndexEntry();
                        entry.listingExternalId = estate.id + "";
                        entry.active = true;
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.listingUrl = App.HOST_HA + "/chnlutils/homeaway_v4.1.1/listing/" + estate.id;
                        Response.Advertiser.listingContentIndexEntry.Add(entry);
                    }

                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    var ppbIds = estateList.Select(x => x.id).ToList();

                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    Response.Advertiser.assignedId = ownerTbl.advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.ListingContentIndexEntry();
                        entry.listingExternalId = estate.id + "";
                        entry.active = true;
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.listingUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/listing/" + estate.id;
                        Response.Advertiser.listingContentIndexEntry.Add(entry);
                    }

                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "ListingContentIndexRequest", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string LodgingConfigurationIndexRequest()
    {

        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("LodgingConfigurationContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "LodgingConfigurationContentIndex", "Owner for HomeAway was not found or not active");
                    return "";
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    errorList.AddError("OTHER");
                    ErrorLog.addLog("", "LodgingConfigurationContentIndex", "Agent for HomeAway was not found or not active");
                    return "";
                }

                var lstEstates = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1).ToList();
                var masterEstates = lstEstates.Where(x => x.pid_master_estate > 0).Select(x => x.pid_master_estate).ToList();
                var lstEstate_ha = lstEstates.Where(x => !masterEstates.Contains(x.id)).ToList();

                var estateIds = new List<int?>();
                var estateList = new List<AppSettings.RNT_estate>();
                ChnlHomeAwayClasses_V411.LodgingConfigurationContentIndexResponse Response = new ChnlHomeAwayClasses_V411.LodgingConfigurationContentIndexResponse();
                //List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                //List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                //estateIds = lstEstate_ha.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                //var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();


                #region Loging Configuration Defaults
                ChnlHomeAwayClasses_V411.LodgingConfigurationDefaults lodgingConfigurationDefaults = new ChnlHomeAwayClasses_V411.LodgingConfigurationDefaults();

                List<RntChnlHomeAwayAcceptedPaymentFormTBL> chnlPaymentforms = dcChnl.RntChnlHomeAwayAcceptedPaymentFormTBL.Where(i => i.isActive == 1).ToList();
                if (chnlPaymentforms != null && chnlPaymentforms.Count > 0)
                {
                    foreach (RntChnlHomeAwayAcceptedPaymentFormTBL paymentforms in chnlPaymentforms)
                    {
                        ChnlHomeAwayClasses_V411.paymentCardDescriptor paymentCardDescriptor = new ChnlHomeAwayClasses_V411.paymentCardDescriptor();
                        paymentCardDescriptor.paymentFormType = paymentforms.paymentFormType;
                        paymentCardDescriptor.cardType = paymentforms.cardType;
                        paymentCardDescriptor.cardCode = paymentforms.cardCode;
                        lodgingConfigurationDefaults.AcceptedPaymentForm.paymentCardDescriptors.Add(paymentCardDescriptor);
                    }

                }

                lodgingConfigurationDefaults.BookingPolicy.policy = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.bookingPolicy;
                lodgingConfigurationDefaults.CancellationPolicy.policy = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.cancellationPolicy;
                lodgingConfigurationDefaults.checkInTime = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.check_InTime;
                lodgingConfigurationDefaults.checkOutTime = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.check_OutTime;

                lodgingConfigurationDefaults.ChildrenAllowedRule.allowed = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.childrenAllowedRule;
                lodgingConfigurationDefaults.EventsAllowedRule.allowed = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.eventsAllowedRule;
                lodgingConfigurationDefaults.lastUpdatedDate.ValueDate = DateTime.Now;
                lodgingConfigurationDefaults.locale = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.locale;
                lodgingConfigurationDefaults.MaximumOccupancyRule.guests = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.MaximumNoOfGuests;
                lodgingConfigurationDefaults.minimumAgeRule.age = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.minAgeRule;
                lodgingConfigurationDefaults.petsAllowedRule.allowed = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.petsAllowed;

                lodgingConfigurationDefaults.pricingPolicy.policy = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.pricingPolicy;
                lodgingConfigurationDefaults.rentalAgreementFile.locale = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.locale;
                lodgingConfigurationDefaults.rentalAgreementFile.rentalAgreementPdfUrl = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.rentalAgreementPdfUrl;
                lodgingConfigurationDefaults.smokingAllowedRule.allowed = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.smokingAllowedRule;
                Response.Advertiser.LodgingConfigurationDefaults = lodgingConfigurationDefaults;
                #endregion

                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    Response.Advertiser.assignedId = ownerTbl.ppb_advertiserAssignedId;

                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.LodgingConfigurationContentIndexEntry();
                        var ha_esate = lstEstate_ha.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ha_esate.is_slave == 1 ? ha_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.lodgingConfigurationContentUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/lodgingconfigurationcontent/" + estate.id;
                        Response.Advertiser.lodgingConfigurationContentIndexEntry.Add(entry);
                    }
                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    //var ppbIds = estateList.Select(x => x.id).ToList();
                    //contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType != "PPB").Select(x => x.id).ToList();
                    //estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract) && !ppbIds.Contains(x.pidEstate)).Select(x => (int?)x.pidEstate).ToList();
                    //estateIds = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == 0 || x.is_slave == null)).Select(x => (int?)x.id).ToList();
                    //estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    Response.Advertiser.assignedId = ownerTbl.advertiserAssignedId;


                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.LodgingConfigurationContentIndexEntry();
                        var ha_esate = lstEstate_ha.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ha_esate.is_slave == 1 ? ha_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.lodgingConfigurationContentUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/lodgingconfigurationcontent/" + estate.id;
                        Response.Advertiser.lodgingConfigurationContentIndexEntry.Add(entry);
                    }
                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "LodgingConfigurationContentIndex", ex.ToString());
            }
        return errorList.GetXml();
    }



    public static string AdvertiserContactContentIndexRequest()
    {

        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("AdvertiserContactContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "AdvertiserContactContentIndex", "Owner for HomeAway was not found or not active");
                    return "";
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    errorList.AddError("OTHER");
                    ErrorLog.addLog("", "AdvertiserContactContentIndex", "Agent for HomeAway was not found or not active");
                    return "";
                }

                var estateIds = new List<int?>();
                var estateList = new List<AppSettings.RNT_estate>();

                ChnlHomeAwayClasses_V411.AdvertiserContactContentIndexResponse Response = new ChnlHomeAwayClasses_V411.AdvertiserContactContentIndexResponse();
                //List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                //List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                //estateIds = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == null || x.is_slave == 0)).Select(x => (int?)x.id).ToList();
                //var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();


                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    Response.Advertiser.assignedId = ownerTbl.ppb_advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.ContactContentIndexEntry();
                        entry.listingExternalId = estate.id + "";
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.contactContentUrl = App.HOST_HA + "/chnlutils/homeaway_v4.1.1/advertisercontact/" + estate.id;
                        Response.Advertiser.contactContentIndexEntry.Add(entry);
                    }

                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    //var ppbIds = estateList.Select(x => x.id).ToList();
                    //contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType != "PPB").Select(x => x.id).ToList();
                    //estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract) && !ppbIds.Contains(x.pidEstate)).Select(x => (int?)x.pidEstate).ToList();
                    //estateIds = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == 0 || x.is_slave == null)).Select(x => (int?)x.id).ToList();
                    //estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    Response.Advertiser.assignedId = ownerTbl.advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.ContactContentIndexEntry();
                        entry.listingExternalId = estate.id + "";
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.contactContentUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/advertisercontact/" + estate.id;
                        Response.Advertiser.contactContentIndexEntry.Add(entry);
                    }

                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "AdvertiserContactContentIndex", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string AdvertiserContentIndexRequest()
    {

        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("advertiserContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "advertiserContentIndexRequest", "Owner for HomeAway was not found or not active");
                    return "";
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    errorList.AddError("OTHER");
                    ErrorLog.addLog("", "advertiserContentIndexRequest", "Agent for HomeAway was not found or not active");
                    return "";
                }
                ChnlHomeAwayClasses_V411.AdvertiserContentIndexResponse Response = new ChnlHomeAwayClasses_V411.AdvertiserContentIndexResponse();

                var estateIds = new List<int?>();
                var estateList = new List<AppSettings.RNT_estate>();

                //List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                //List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                //estateIds = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == null || x.is_slave == 0)).Select(x => (int?)x.id).ToList();
                //var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.AdvertiserContentIndexEntry();

                        entry.advertiserAssignedId = ownerTbl.ppb_advertiserAssignedId;
                        entry.advertiserName = ownerTbl.username;
                        entry.advertiserContactContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/advertisercontact/" + estate.id;
                        entry.advertiserListingContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/listing/" + estate.id;
                        entry.advertiserLodgingConfigurationContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/lodgingconfigurationcontent/" + estate.id;
                        entry.advertiserLodgingRateContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/lodgingratecontent/" + estate.id;
                        entry.advertiserUnitAvailabilityContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/unitavailabilitycontent/" + estate.id;
                        Response.AdvertiserContentIndexEntries.Add(entry);


                    }

                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    //var ppbIds = estateList.Select(x => x.id).ToList();
                    //contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType != "PPB").Select(x => x.id).ToList();
                    //estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract) && !ppbIds.Contains(x.pidEstate)).Select(x => (int?)x.pidEstate).ToList();
                    //estateIds = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == 0 || x.is_slave == null)).Select(x => (int?)x.id).ToList();
                    //estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.AdvertiserContentIndexEntry();
                        entry.advertiserAssignedId = ownerTbl.advertiserAssignedId;
                        entry.advertiserName = ownerTbl.username;
                        entry.advertiserContactContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/advertisercontact/" + estate.id;
                        entry.advertiserListingContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/listing/" + estate.id;
                        entry.advertiserLodgingConfigurationContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/lodgingconfigurationcontent/" + estate.id;
                        entry.advertiserLodgingRateContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/lodgingratecontent/" + estate.id;
                        entry.advertiserUnitAvailabilityContentIndexUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/unitavailabilitycontent/" + estate.id;
                        Response.AdvertiserContentIndexEntries.Add(entry);
                    }

                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "advertiserContentIndexRequest", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string AdvertiserLodgingRateContentRequest()
    {

        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("advertiserLodgingRateContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
                var lstEstates = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1).ToList();
                var masterEstates = lstEstates.Where(x => x.pid_master_estate > 0).Select(x => x.pid_master_estate).ToList();
                var lstEstate_ha = lstEstates.Where(x => !masterEstates.Contains(x.id)).ToList();

                //var lstEstate_ha = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.is_active == 1 && x.is_slave == 1).ToList();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "AdvertiserLodgingRateContentRequest", "Owner for HomeAway was not found or not active");
                    return "";
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    errorList.AddError("OTHER");
                    ErrorLog.addLog("", "AdvertiserLodgingRateContentRequest", "Agent for HomeAway was not found or not active");
                    return "";
                }

                ChnlHomeAwayClasses_V411.AdvertiserLodgingRateContentIndexResponse Response = new ChnlHomeAwayClasses_V411.AdvertiserLodgingRateContentIndexResponse();
                //List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                //List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                //estateIds = lstEstate_ha.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                //var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                var estateIds = new List<int?>();
                var estateList = new List<AppSettings.RNT_estate>();

                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    var advertiserPPB = new ChnlHomeAwayClasses_V411.AdvertiserRate();
                    advertiserPPB.assignedId = ownerTbl.ppb_advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.LodgingRateContentIndexEntry();
                        var ha_esate = lstEstate_ha.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ha_esate.is_slave == 1 ? ha_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.lodgingRateContentUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/lodgingratecontent/" + estate.id;
                        advertiserPPB.listingRateContentEntry.Add(entry);
                    }
                    Response.advertiser = advertiserPPB;
                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    //var ppbIds = estateList.Select(x => x.id).ToList();
                    //contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType != "PPB").Select(x => x.id).ToList();
                    //estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract) && !ppbIds.Contains(x.pidEstate)).Select(x => (int?)x.pidEstate).ToList();
                    ////estateIds = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == 0 || x.is_slave == null)).Select(x => (int?)x.id).ToList();
                    //estateIds = lstEstate_ha.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    //estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    var advertiserNOPPB = new ChnlHomeAwayClasses_V411.AdvertiserRate();
                    advertiserNOPPB.assignedId = ownerTbl.advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.LodgingRateContentIndexEntry();
                        var ha_esate = lstEstate_ha.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ha_esate.is_slave == 1 ? ha_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.lodgingRateContentUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/lodgingratecontent/" + estate.id;
                        advertiserNOPPB.listingRateContentEntry.Add(entry);
                    }
                    Response.advertiser = advertiserNOPPB;
                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "AdvertiserLodgingRateContent", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string AdvertiserUnitAvailabilityContentIndexRequest()
    {

        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("advertiserUnitAvailabilityContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
                var lstEstates = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1).ToList();
                var masterEstates = lstEstates.Where(x => x.pid_master_estate > 0).Select(x => x.pid_master_estate).ToList();
                var lstEstate_ha = lstEstates.Where(x => !masterEstates.Contains(x.id)).ToList();

                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "AdvertiserUnitAvailabilityContentIndexRequest", "Owner for HomeAway was not found or not active");
                    return "";
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    errorList.AddError("OTHER");
                    ErrorLog.addLog("", "AdvertiserUnitAvailabilityContentIndexRequest", "Agent for HomeAway was not found or not active");
                    return "";
                }
                ChnlHomeAwayClasses_V411.AdvertiserUnitAvailabilityContentIndexResponse Response = new ChnlHomeAwayClasses_V411.AdvertiserUnitAvailabilityContentIndexResponse();
                var estateIds = new List<int?>();
                var estateList = new List<AppSettings.RNT_estate>();
                //List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                //List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                //estateIds = lstEstate_ha.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                //var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();


                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    var advertiserPPB = new ChnlHomeAwayClasses_V411.AdvertiserAvailability();
                    advertiserPPB.assignedId = ownerTbl.ppb_advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.UnitAvailabilityContentIndexEntry();
                        var ha_esate = lstEstate_ha.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ha_esate.is_slave == 1 ? ha_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";

                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.unitAvailabilityContentUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/unitavailabilitycontent/" + estate.id;
                        advertiserPPB.listingAvailabilityContentEntry.Add(entry);
                    }
                    Response.advertisers.Add(advertiserPPB);
                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    //var ppbIds = estateList.Select(x => x.id).ToList();
                    //contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType != "PPB").Select(x => x.id).ToList();
                    //estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract) && !ppbIds.Contains(x.pidEstate)).Select(x => (int?)x.pidEstate).ToList();
                    //estateIds = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == 0 || x.is_slave == null)).Select(x => (int?)x.id).ToList();
                    //estateIds = lstEstate_ha.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    //estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();
                    var advertiserNOPPB = new ChnlHomeAwayClasses_V411.AdvertiserAvailability();
                    advertiserNOPPB.assignedId = ownerTbl.advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.UnitAvailabilityContentIndexEntry();
                        var ha_esate = lstEstate_ha.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ha_esate.is_slave == 1 ? ha_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";

                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.unitAvailabilityContentUrl = App.HOST_SSL + "/chnlutils/homeaway_v4.1.1/unitavailabilitycontent/" + estate.id;
                        advertiserNOPPB.listingAvailabilityContentEntry.Add(entry);
                    }
                    Response.advertisers.Add(advertiserNOPPB);
                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "AdvertiserUnitAvailabilityContentIndexRequest", ex.ToString());
            }
        return errorList.GetXml();
    }
    //
    public static string BookingContentIndexRequest(string xml)
    {
        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("bookingContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "BookingContentIndexRequest", "Owner for HomeAway was not found or not active");
                    return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingContentIndex");
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    ErrorLog.addLog("", "BookingContentIndexRequest", "Agent for HomeAway was not found or not active");
                    return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingContentIndex");
                }

                //first fetched reservation list
                ChnlHomeAwayClasses_V411.BookingContentIndexRequest Request = new ChnlHomeAwayClasses_V411.BookingContentIndexRequest(xml);
                Request.documentVersion = "1.2";
                var haEstates = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.is_active == 1);

                var reservationList = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => (x.state_pid == 4 || x.state_pid == 3) && x.agentID == agentID && x.state_date.HasValue).ToList();
                var resLogs = dc.RntReservationLastUpdatedLOG.ToList();
                //var resLogs = AppSettings.lstRntReservationLastUpdatedLOGs.ToList();
                var resIds = new List<long>();

                if (Request.startDate.ValueDate.HasValue)
                {
                    var resListStartDate = resLogs.Where(x => x.lastUpdatedDate >= Request.startDate.ValueDate.Value).Select(x => x.id).ToList();
                    var resStateListStartDate = reservationList.Where(x => x.state_date.Value >= Request.startDate.ValueDate.Value).Select(x => x.id).ToList();
                    resIds.AddRange(resListStartDate);
                    resIds.AddRange(resStateListStartDate);
                }

                if (Request.endDate.ValueDate.HasValue)
                {
                    var resListEndDate = resLogs.Where(x => x.lastUpdatedDate <= Request.endDate.ValueDate.Value).Select(x => x.id).ToList();
                    var resStateListEndDate = reservationList.Where(x => x.state_date.Value <= Request.endDate.ValueDate.Value).Select(x => x.id).ToList();
                    resIds.AddRange(resListEndDate);
                    resIds.AddRange(resStateListEndDate);
                }

                reservationList = reservationList.Where(x => resIds.Contains(x.id)).ToList();

                ChnlHomeAwayClasses_V411.BookingContentIndexResponse Response = new ChnlHomeAwayClasses_V411.BookingContentIndexResponse();
                var advertiserId = ownerTbl.ppb_advertiserAssignedId;
                Response.documentVersion = "1.2";
                if (Request.advertiser.assignedId == ownerTbl.ppb_advertiserAssignedId && ownerTbl.ppb_advertiserAssignedId != "")
                {
                    var estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = haEstates.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    estateIds = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    var PPBreservationList = reservationList.Where(x => estateIds.Contains(x.pid_estate));
                    var PPBadvertiser = new ChnlHomeAwayClasses_V411.Advertiser();
                    PPBadvertiser.assignedId = ownerTbl.ppb_advertiserAssignedId;
                    foreach (var reservation in PPBreservationList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.BookingContentIndexEntry();
                        //entry.active = true;
                        var lastUpdatedDate = resLogs.SingleOrDefault(x => x.id == reservation.id);
                        entry.lastUpdatedDate.ValueDate = lastUpdatedDate != null ? lastUpdatedDate.lastUpdatedDate : reservation.state_date.Value;
                        entry.bookingUpdateUrl = App.HOST_HA + "/chnlutils/homeaway_v4.1.1/bookingupdate/" + reservation.unique_id;
                        PPBadvertiser.bookingContentIndexEntryList.Add(entry);
                    }
                    Response.advertiser = PPBadvertiser;
                }

                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    var estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                    estateIds = haEstates.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    estateIds = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    var NOPPBreservationList = reservationList.Where(x => estateIds.Contains(x.pid_estate));
                    var advertiser = new ChnlHomeAwayClasses_V411.Advertiser();
                    advertiser.assignedId = ownerTbl.advertiserAssignedId;
                    foreach (var reservation in NOPPBreservationList)
                    {
                        var entry = new ChnlHomeAwayClasses_V411.BookingContentIndexEntry();
                        // entry.active = true;
                        var lastUpdatedDate = resLogs.SingleOrDefault(x => x.id == reservation.id);
                        entry.lastUpdatedDate.ValueDate = lastUpdatedDate != null ? lastUpdatedDate.lastUpdatedDate : reservation.state_date.Value;
                        entry.bookingUpdateUrl = App.HOST_HA + "/chnlutils/homeaway_v4.1.1/bookingupdate/" + reservation.unique_id;
                        advertiser.bookingContentIndexEntryList.Add(entry);
                    }
                    Response.advertiser = advertiser;
                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "HA_Quote", ex.ToString());
            }
        return errorList.GetXml();
    }


    private static List<string> ListingLevelExcludeFeature42version()
    {
        string[] listinglevelFeatureExclude ={
        "ATTRACTIONS_ARBORETUM",
        "ATTRACTIONS_AUTUMN_FOLIAGE",
        "ATTRACTIONS_BOTANICAL_GARDEN",
        "ATTRACTIONS_CAVE",
        "ATTRACTIONS_CHURCHES",
        "ATTRACTIONS_CINEMAS",
        "ATTRACTIONS_FESTIVALS",
        "ATTRACTIONS_FORESTS",
        "ATTRACTIONS_LIBRARY",
        "ATTRACTIONS_LIVE_THEATER",
        "ATTRACTIONS_NUDE_BEACH",
        "ATTRACTIONS_PLAYGROUND",
        "ATTRACTIONS_POND",
        "ATTRACTIONS_RAIN_FORESTS",
        "ATTRACTIONS_REC_CENTER",
        "ATTRACTIONS_REEF",
        "ATTRACTIONS_RESTAURANTS",
        "ATTRACTIONS_RUINS",
        "ATTRACTIONS_SYNAGOGUES",
        "ATTRACTIONS_VOLCANO",
        "ATTRACTIONS_WATERFALLS",
        "LEISURE_BEACHCOMBING",
        "LEISURE_BOWLING",
        "LEISURE_DISCO",
        "LEISURE_HORSESHOES",
        "LEISURE_LUAUS",
        "LEISURE_MINIATURE_GOLF",
        "LEISURE_NATURISME",
        "LEISURE_PHOTOGRAPHY",
        "LEISURE_SCENIC_DRIVES",
        "LEISURE_SHELLING",
        "LEISURE_SHUFFLEBOARD",
        "LEISURE_SIGHT_SEEING",
        "LEISURE_THALASSOTHERAPY",
        "LEISURE_THERMALISME",
        "LEISURE_WALKING",
        "LOCAL_ATM_BANK",
        "LOCAL_BABYSITTING",
        "LOCAL_GROCERIES",
        "LOCAL_MASSAGE_THERAPIST",
        "LOCAL_MEDICAL_SERVICES",
        "LOCATION_TYPE_MONUMENT_VIEW",
        "SPORTS_EQUESTRIAN_EVENTS",
        "SPORTS_HOT_AIR_BALLOONING",
        "SPORTS_PARAGLIDING",
        "SPORTS_RACQUETBALL",
        "SPORTS_ROLLER_BLADING",
        "SPORTS_TENNIS",
        "ACCOMMODATIONS_TYPE_HOTEL",
        "ACCOMMODATIONS_TYPE_MOTEL",
        "ACCOMMODATIONS_BREAKFAST_NOT_AVAILABLE",
        "ACCOMMODATIONS_LUNCH_NOT_AVAILABLE",
        "ACCOMMODATIONS_DINNER_NOT_AVAILABLE",
        "SPORTS_SKI_LIFT_PRIVILEDGES",
        "SPORTS_SKI_LIFT_PRIVILEDGES_OPTIONAL",
        "SUITABILITY_ACCESSIBILITY_LIMITED_ACCESSIBILITY",
        "SUITABILITY_CHILDREN_NOT_ALLOWED"
        };

        return listinglevelFeatureExclude.ToList();
    }
    private static List<string> UnitLevelExcludeFeature42version()
    {
        string[] UnitLevelFeatureExclude ={
"ACCOMMODATIONS_DINNER_BOOKING_POSSIBLE",
"ACCOMMODATIONS_DINNER_INCLUDED_IN_PRICE",
"ACCOMMODATIONS_LUNCH_BOOKING_POSSIBLE",
"ACCOMMODATIONS_LUNCH_INCLUDED_IN_PRICE",
"ACCOMMODATIONS_MEALS_CATERING_AVAILABLE",
"ACCOMMODATIONS_MEALS_GUESTS_FURNISH_OWN",
"ACCOMMODATIONS_OTHER_SERVICES_STAFF",
"ACCOMMODATIONS_TYPE_BED_AND_BREAKFAST",
"ACCOMMODATIONS_TYPE_GUEST_HOUSE",
"ARRIVAL_DAY_FLEXIBLE",
"ARRIVAL_DAY_FRIDAY",
"ARRIVAL_DAY_MONDAY",
"ARRIVAL_DAY_NEGOTIABLE",
"ARRIVAL_DAY_SATURDAY",
"ARRIVAL_DAY_SUNDAY",
"ARRIVAL_DAY_THURSDAY",
"ARRIVAL_DAY_TUESDAY",
"ARRIVAL_DAY_WEDNESDAY",
"KITCHEN_DINING_RACLETTE",
"OUTDOOR_BOAT_DOCK",
"OUTDOOR_PETANQUE",
"THEMES_ADVENTURE",
"THEMES_AWAY_FROM_IT_ALL",
"THEMES_BUDGET",
"THEMES_FARM_HOLIDAYS",
"THEMES_HOLIDAY_COMPLEX",
"THEMES_SPA",
"THEMES_SPORTS_ACTIVITIES",
"THEMES_TOURIST_ATTRACTIONS",
"ACCOMMODATIONS_TYPE_HOTEL",
"ACCOMMODATIONS_TYPE_MOTEL",
"ACCOMMODATIONS_BREAKFAST_NOT_AVAILABLE",
"ACCOMMODATIONS_LUNCH_NOT_AVAILABLE",
"ACCOMMODATIONS_DINNER_NOT_AVAILABLE",
"SPORTS_SKI_LIFT_PRIVILEDGES",
"SPORTS_SKI_LIFT_PRIVILEDGES_OPTIONAL",
"SUITABILITY_ACCESSIBILITY_LIMITED_ACCESSIBILITY",
"SUITABILITY_CHILDREN_NOT_ALLOWED"

};
        return UnitLevelFeatureExclude.ToList();
    }
    public static string Listing(int IdEstate)
    {
        using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        using (magaContent_DataContext DC_CONTENT = new magaContent_DataContext())
        {
            var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
            if (ownerTbl == null)
            {
                ErrorLog.addLog("", "Listing", "Owner for HomeAway was not found or not active");
                return "";
            }
            RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
            if (currEstate == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "listing");
            }
            RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
            if (currHAEstate == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "listing");
            }
            var Units = dcChnl.RntChnlHomeAwayEstateTB.Where(x => x.id == IdEstate || (x.is_slave == 1 && x.pid_master_estate == IdEstate)).ToList();

            //var haEstates = dcChnl.dbRntChnlHomeAwayEstateTBs.ToList();
            //var UnitSlaves = haEstates.Where(x => x.pid_master_estate == IdEstate).ToList();
            //var Units = new List<dbRntChnlHomeAwayEstateTB>();

            //if (UnitSlaves.Count > 0)
            //    Units = UnitSlaves;
            //else
            //    Units = haEstates.Where(x => x.id == IdEstate).ToList();

            if (Units.Count > 30)
            {
                Units = Units.Take(30).ToList();
            }

            //var Units = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.id == IdEstate || (x.is_slave == 1 && x.pid_master_estate == IdEstate)).ToList();
            var notSupportedLangIds = CommonUtilities.getSYS_SETTING("rnt_HANotSupportedLangIds").splitStringToList(",");
            var currLnList = dcChnl.RntChnlHomeAwayEstateLN.Where(x => x.pid_estate == IdEstate).ToList();

            #region not supported language from homeaway
            if (notSupportedLangIds != null && notSupportedLangIds.Count > 0)
                currLnList = currLnList.Where(x => !notSupportedLangIds.Contains(x.pid_lang)).ToList();
            #endregion

            var dtStart = DateTime.Now.Date;
            var dtEnd = DateTime.Now.AddYears(2).Date;

            ChnlHomeAwayClasses_V411.ListingClasses.Listing Response = new ChnlHomeAwayClasses_V411.ListingClasses.Listing();
            Response.externalId = IdEstate + "";
            Response.active = true;
            Response.inserted = currEstate.dtCreation;

            //dbRntEstateLOG lastlog = dc.dbRntEstateLOGs.LastOrDefault(x => x.estateID == IdEstate);
            //if (lastlog != null)
            //{
            //    Response.updated = lastlog.logDate;
            //}
            var app_DefaultLang_Code = "";
            if (DC_CONTENT.CONT_TBL_LANGs.Any(x => x.id == App.DefLangID))
            {
                app_DefaultLang_Code = DC_CONTENT.CONT_TBL_LANGs.FirstOrDefault(x => x.id == App.DefLangID).abbr;
            }
            //ALL TEXT VALUES
            foreach (var ln in currLnList)
            {
                string HACode = contUtils.getLanguage_HAcodeByAbbr(ln.pid_lang);
                if (string.IsNullOrEmpty(HACode))
                    continue;

                Response.adContent.accommodationsSummary.Add(ln.summary, HACode);
                if (string.IsNullOrWhiteSpace(ln.title))
                {
                    ln.title = ln.unit_name;
                }

                if (string.IsNullOrWhiteSpace(ln.description) && (ln.pid_lang == app_DefaultLang_Code))
                {
                    if (!string.IsNullOrWhiteSpace(ln.title))
                    {
                        ln.description = ln.title;
                    }
                    else if (DC_CONTENT.CONT_TBL_LANGs.Any(x => x.abbr == ln.pid_lang))
                    {
                        int langid = DC_CONTENT.CONT_TBL_LANGs.FirstOrDefault(x => x.code == ln.pid_lang).id;
                        ln.description = CurrentSource.rntEstate_title(IdEstate, langid, currEstate.code);
                    }
                }

                ln.description = HaListingUtility_V411.RemoveHtmlTags(ln.description);


                ln.description = HaListingUtility_V411.AdjustLengthOfData(ln.description, Response.adContent.description.MinChar);
                Response.adContent.description.Add(ln.description, HACode);

                ln.title = HaListingUtility_V411.AdjustLengthOfData(ln.title, Response.adContent.headline.MinChar);
                Response.adContent.headline.Add(ln.title, HACode);
                Response.adContent.ownerListingStory.Add(ln.listing_story, HACode);
                Response.adContent.propertyName.Add(CurrentSource.rntEstate_title(IdEstate, ln.pid_lang.objToInt32(), currEstate.code), HACode);
                Response.adContent.uniqueBenefits.Add(ln.unique_benifits, HACode);
                Response.adContent.whyPurchased.Add(ln.why_purchased, HACode);
                Response.location.description.Add(ln.location_description, HACode);
                Response.location.otherActivities.Add(ln.location_other_activities, HACode);
            }

            //AD CONTENT
            Response.adContent.yearPurchased = currHAEstate.year_purchased;
            #region Features Excluded from HA 4.1.1
            List<string> FeatureExcluded = new List<string>();
            FeatureExcluded.Add("SUITABILITY_PETS_ASK");
            FeatureExcluded.Add("SUITABILITY_PETS_CONSIDERED");
            FeatureExcluded.Add("THEMES_LUXURY");
            FeatureExcluded.Add("SUITABILITY_OTHER_LONG_TERM_RENTERS");
            FeatureExcluded.Add("SUITABILITY_SMOKING_NOT_ALLOWED");

            FeatureExcluded.Add("SUITABILITY_CHILDREN_WELCOME");
            FeatureExcluded.Add("SUITABILITY_PETS_NOT_ALLOWED");

            FeatureExcluded.Add("SUITABILITY_ACCESSIBILITY_ASK");
            FeatureExcluded.Add("SUITABILITY_OTHER_EVENTS_ALLOWED");
            FeatureExcluded.Add("SUITABILITY_CHILDREN_ASK");

            FeatureExcluded.Add("SUITABILITY_SMOKING_ALLOWED");
            FeatureExcluded.Add("SUITABILITY_SMOKING_ASK");
            FeatureExcluded.AddRange(ListingLevelExcludeFeature42version());
            FeatureExcluded.AddRange(UnitLevelExcludeFeature42version());

            #endregion

            //featureValues
            var features = dcChnl.RntChnlHomeAwayEstateFeaturesRL.Where(x => x.pidEstate == IdEstate && x.type == "Listing" && !FeatureExcluded.Contains(x.code)).ToList();
            foreach (var feature in features)
            {
                int count = feature.count.objToInt32();
                if (count == 0) count = 1;
                //if (feature.count.objToInt32() == 0) continue;
                var featureValue = new ChnlHomeAwayClasses_V411.ListingClasses.FeatureValue();
                featureValue.count = count;
                featureValue.listingFeatureName = feature.code;
                Response.featureValues.Add(featureValue);
            }
            Response.adContent.yearPurchased = currHAEstate.year_purchased;

            //LOCATION
            Response.location.address.addressLine1 = currEstate.loc_address;
            Response.location.address.addressLine2 = currEstate.loc_inner_bell;
            Response.location.address.postalCode = currEstate.loc_zip_code;
            Response.location.address.city = CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), App.DefLangID, "");
            Response.location.address.stateOrProvince = CommonUtilities.getSYS_SETTING("HAStateProvince");
            if (currEstate.pid_city == AppSettings.RNT_currCity_Florence)
            {
                Response.location.address.stateOrProvince = CommonUtilities.getSYS_SETTING("HAStateProvince_Florence");

            }
            // locUtils.getCity_titleOfRegion(currEstate.pid_city.objToInt32(), App.DefLangID, "");//CommonUtilities.getSYS_SETTING("HAStateProvince"); //
            Response.location.address.country = CommonUtilities.getSYS_SETTING("HACountry"); //locUtils.getCity_codeOfCountry(currEstate.pid_city.objToInt32(), App.DefLangID, "");



            // HA 4.1.1 nearest points not required 
            //if (DC_RENTAL.RNT_RL_ESTATE_POINT.Any(x => x.pid_estate == IdEstate))
            //{
            //    List<RNT_RL_ESTATE_POINT> lstRNT_RL_ESTATE_POINT = DC_RENTAL.RNT_RL_ESTATE_POINT.Where(x => x.pid_estate == IdEstate ).ToList();
            //    List<int> pidPints = lstRNT_RL_ESTATE_POINT.Select(i => i.pid_point).ToList();
            //    List<LOC_VIEW_POINT> locViewPoint = locProps.POI_VIEW.Where(i => pidPints.Contains(i.id) &&  i.pid_city == currEstate.pid_city && i.is_acitve == 1 && i.pid_lang == App.LangID).ToList();
            //    List<int?> pointTypes = locViewPoint.Select(i => i.pid_point_type).Distinct().ToList();

            //    foreach (int point_type in pointTypes)
            //    {
            //        LOC_VIEW_POINT POIData = locViewPoint.FirstOrDefault(i => pidPints.Contains(i.id) && i.pid_point_type == point_type && i.pid_city == currEstate.pid_city && i.is_acitve == 1 && i.pid_lang == App.LangID);

            //        if (POIData != null)
            //        {
            //            RNT_RL_ESTATE_POINT objRNT_RL_ESTATE_POINT = lstRNT_RL_ESTATE_POINT.FirstOrDefault(i => i.pid_point == POIData.id);
            //            ChnlHomeAwayClasses.ListingClasses.NearestPlace nearestPlace = new ChnlHomeAwayClasses.ListingClasses.NearestPlace();
            //            nearestPlace._placeType = locUtils.getPOI_TypeTitle(point_type, App.LangID);
            //            if (!string.IsNullOrEmpty(nearestPlace._placeType))
            //            {
            //                ChnlHomeAwayClasses.ListingClasses.DecimalUnit decimalUnit = new ChnlHomeAwayClasses.ListingClasses.DecimalUnit();

            //                if (!string.IsNullOrEmpty(objRNT_RL_ESTATE_POINT.distance))
            //                {
            //                    decimalUnit.Value = objRNT_RL_ESTATE_POINT.distance;
            //                }
            //                else
            //                {
            //                    decimalUnit.Value = "0";
            //                }
            //                nearestPlace.distance = decimalUnit;
            //                nearestPlace.distanceUnit = "METRES";
            //                nearestPlace.name.Add(POIData.title, POIData.pid_lang);
            //                Response.location.nearestPlaces.Add(nearestPlace);
            //            }

            //        }
            //    }
            //}




            if (currEstate.google_maps != "" && currEstate.google_maps != null)
            {
                if (currEstate.google_maps.Split('|').Length > 1)
                {
                    Response.location.geoCode.latitude = currEstate.google_maps.Split('|')[0].Replace(",", ".").Replace(" ", string.Empty);
                    Response.location.geoCode.longitude = currEstate.google_maps.Split('|')[1].Replace(",", ".").Replace(" ", string.Empty);
                }
                else if (currEstate.google_maps.Split(',').Length > 1)
                {
                    Response.location.geoCode.latitude = currEstate.google_maps.Split(',')[0].Trim().Replace(" ", string.Empty);
                    Response.location.geoCode.longitude = currEstate.google_maps.Split(',')[1].Trim().Replace(" ", string.Empty);
                }
            }
            Response.location.showExactLocation = currHAEstate.show_exact_location == 1;

            var images = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == "chnlHA" && x.pid_estate == IdEstate).OrderBy(x => x.sequence).ToList();
            int imagesMaxCount = 6;

            List<RNT_RL_ESTATE_MEDIA> ProperCountImagelst = new List<RNT_RL_ESTATE_MEDIA>();

            if (images != null && images.Count < imagesMaxCount && images.Count > 0)
            {
                ProperCountImagelst.AddRange(images);

                int actualCount = images.Count;
                int addCount = imagesMaxCount - images.Count;
                int loopcount = 0;
                if (actualCount > addCount)
                {
                    ProperCountImagelst.AddRange(images.Take(addCount));
                }

                else
                {
                    loopcount = addCount / actualCount;
                    do
                    {
                        foreach (RNT_RL_ESTATE_MEDIA obj in images)
                        {
                            ProperCountImagelst.Add(obj);
                        }
                        loopcount--;

                    } while (loopcount > 0);

                };

                images = ProperCountImagelst;
            }

            foreach (var image in images)
            {
                var tmp = new ChnlHomeAwayClasses_V411.ListingClasses.Image();
                tmp.externalId = image.id + "";
                tmp.title.Add(image.code, "en");
                tmp.uri = App.HOST + "/" + image.img_banner;
                //"?maxsize=true"
                Response.images.Add(tmp);
            }
            //if (UnitSlaves.Count > 0)
            //{
            //    foreach (var unit in Units)
            //    {
            //        var unitEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == unit.id && x.is_active == 1);
            //        var unitImages = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == "chnlHA" && x.pid_estate == unitEstate.id).OrderBy(x => x.sequence).ToList();
            //        foreach (var imagenew in unitImages)
            //        {
            //            var tmpnew = new ChnlHomeAwayClasses.ListingClasses.Image();
            //            tmpnew.externalId = imagenew.id + "";
            //            tmpnew.title.Add(imagenew.code, "en");
            //            tmpnew.uri = App.HOST + "/" + imagenew.img_banner;
            //            Response.images.Add(tmpnew);
            //            //"?maxsize=true"
            //            //foreach (var items_ in Response.images)
            //            //{
            //            //    if (items_.externalId != imagenew.id + "") 
            //            //        Response.images.Add(tmpnew);
            //            //}
            //        }
            //    }
            // }
            // UNITS
            foreach (var unit in Units)
            {
                var unitEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == unit.id && x.is_active == 1);
                if (unitEstate == null)
                {
                    continue;
                }
                var currUnit = new ChnlHomeAwayClasses_V411.ListingClasses.Unit();
                currUnit.externalId = unit.id + "";
                currUnit.active = true;
                currUnit.area = unit.mq_inner.objToInt32();
                currUnit.areaUnit = unit.mq_inner_unit;
                currUnit.diningSeating = unit.num_dining_seating;
                currUnit.loungeSeating = unit.num_lounge_seating;

                currUnit.diningSeating = unit.num_dining_seating;
                currUnit.propertyType = unit.propertyType + "";
                currUnit.registrationNumber = unitEstate.registrationNumber;

                // TODO unitMonetaryInformation                
                //currUnit.unitMonetaryInformation.contractualBookingDeposit.ValueDecimal = currEstate.pr_percentage.objToDecimal();
                //currUnit.unitMonetaryInformation.damageDeposit.ValueDecimal = currEstate.pr_deposit.objToDecimal();
                //currUnit.unitMonetaryInformation.nonContractualBookingDeposit.ValueDecimal = currEstate.pr_percentage.objToDecimal();

                //HA update 4.1.1  commented as per pdf as only C
                //currUnit.unitMonetaryInformation.contractualBookingDeposit.ValueDecimal = unitEstate.pr_percentage.objToDecimal();
                //currUnit.unitMonetaryInformation.damageDeposit.ValueDecimal = unitEstate.pr_deposit.objToDecimal() * contUtils.getCurrentConversionRate();
                //currUnit.unitMonetaryInformation.nonContractualBookingDeposit.ValueDecimal = unitEstate.pr_percentage.objToDecimal();


                //featureValues

                features = dcChnl.RntChnlHomeAwayEstateFeaturesRL.Where(x => x.pidEstate == unitEstate.id && x.type == "Unit" && !FeatureExcluded.Contains(x.code)).ToList();
                //features = dcChnl.dbRntChnlHomeAwayEstateFeaturesRLs.Where(x => x.pidEstate == IdEstate && x.type == "Unit").ToList();
                foreach (var feature in features)
                {
                    int count = feature.count.objToInt32();
                    if (count == 0) count = 1;
                    //if (feature.count.objToInt32() == 0) continue;
                    var featureValue = new ChnlHomeAwayClasses_V411.ListingClasses.FeatureValue();
                    featureValue.count = count;
                    featureValue.unitFeatureName = feature.code;
                    currUnit.featureValues.Add(featureValue);
                }

                //Commented HA 4.1.1 update 
                //if (features.Count(x => x.code == "SUITABILITY_PETS_NOT_ALLOWED") < 1)
                //{
                //    var currFeature = features.FirstOrDefault(x => x.code == "SUITABILITY_PETS_ASK" || x.code == "SUITABILITY_PETS_CONSIDERED");
                //    if (currFeature == null)
                //    {
                //        var featureValue = new ChnlHomeAwayClasses.ListingClasses.FeatureValue();
                //        featureValue.count = 1;
                //        featureValue.unitFeatureName = "SUITABILITY_PETS_NOT_ALLOWED";
                //        currUnit.featureValues.Add(featureValue);
                //    }
                //}

                //var unitImages = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == "chnlHA" && x.pid_estate == unitEstate.id).OrderBy(x => x.sequence).ToList();
                //foreach (var image in unitImages)
                //{
                //    var tmp = new ChnlHomeAwayClasses.ListingClasses.Image();
                //    tmp.externalId = image.id + "";
                //    tmp.title.Add(image.code, "en");
                //    tmp.uri = App.HOST + "/" + image.img_banner;
                //    //"?maxsize=true"
                //    currUnit.images.Add(tmp);
                //}

                // bathrooms
                var bathRooms = dcChnl.RntChnlHomeAwayEstateRoomTB.Where(x => x.pidEstate == unitEstate.id && x.roomType == "Bathroom").ToList();
                if (bathRooms.Count == 0)
                    bathRooms = ChnlHomeAwayUtils_V411.getRooms(unitEstate, "Bathroom");
                foreach (var room in bathRooms)
                {
                    var tmpRoom = new ChnlHomeAwayClasses_V411.ListingClasses.Bathroom();
                    tmpRoom.roomSubType = room.roomSubType;
                    var featuresListString = room.features.splitStringToList("|");
                    foreach (var featuresString in featuresListString)
                    {
                        if (featuresString.splitStringToList("%").Count == 2)
                        {
                            ChnlHomeAwayClasses_V411.ListingClasses.Amenity amenity = new ChnlHomeAwayClasses_V411.ListingClasses.Amenity();
                            amenity.bathroomFeatureName = featuresString.splitStringToList("%")[0];
                            amenity.count = featuresString.splitStringToList("%")[1].ToInt32();
                            tmpRoom.amenities.Add(amenity);
                        }
                    }
                    var tmpRoomLns = dcChnl.RntChnlHomeAwayEstateRoomLN.Where(x => x.pidRoom == room.uid).ToList();

                    #region not supported language from homeaway
                    if (notSupportedLangIds != null && notSupportedLangIds.Count > 0)
                        tmpRoomLns = tmpRoomLns.Where(x => !notSupportedLangIds.Contains(x.pidLang)).ToList();
                    #endregion

                    foreach (var ln in tmpRoomLns)
                    {
                        tmpRoom.name.Add(ln.title, ln.pidLang);
                        tmpRoom.note.Add(ln.description, ln.pidLang);
                    }
                    currUnit.bathrooms.Add(tmpRoom);
                }

                // bedrooms
                var bedRooms = dcChnl.RntChnlHomeAwayEstateRoomTB.Where(x => x.pidEstate == unitEstate.id && x.roomType == "Bedroom").ToList();
                if (bedRooms.Count == 0) bedRooms = ChnlHomeAwayUtils_V411.getRooms(unitEstate, "Bedroom");
                foreach (var room in bedRooms)
                {
                    var tmpRoom = new ChnlHomeAwayClasses_V411.ListingClasses.Bedroom();
                    tmpRoom.roomSubType = room.roomSubType;
                    var featuresListString = room.features.splitStringToList("|");
                    foreach (var featuresString in featuresListString)
                    {
                        if (featuresString.splitStringToList("%").Count == 2)
                        {
                            ChnlHomeAwayClasses_V411.ListingClasses.Amenity amenity = new ChnlHomeAwayClasses_V411.ListingClasses.Amenity();
                            amenity.bedroomFeatureName = featuresString.splitStringToList("%")[0];
                            amenity.count = featuresString.splitStringToList("%")[1].ToInt32();
                            tmpRoom.amenities.Add(amenity);
                        }
                    }
                    var tmpRoomLns = dcChnl.RntChnlHomeAwayEstateRoomLN.Where(x => x.pidRoom == room.uid).ToList();

                    #region not supported language from homeaway
                    if (notSupportedLangIds != null && notSupportedLangIds.Count > 0)
                        tmpRoomLns = tmpRoomLns.Where(x => !notSupportedLangIds.Contains(x.pidLang)).ToList();
                    #endregion

                    foreach (var ln in tmpRoomLns)
                    {
                        string HACode = "";
                        int langId;
                        if (int.TryParse(ln.pidLang, out langId))
                        {
                            HACode = contUtils.getLanguage_HAcode(ln.pidLang.objToInt32());

                        }
                        if (string.IsNullOrEmpty(HACode))
                            continue;


                        tmpRoom.name.Add(ln.title, HACode);
                        tmpRoom.note.Add(ln.description, HACode);
                    }
                    currUnit.bedrooms.Add(tmpRoom);
                }

                #region Removed ratePeriods from content xml
                // TODO ratePeriods, Finalaize
                //int outError;
                //var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, dtStart, dtEnd, 0, out outError);
                //foreach (var tmp in priceListPerDates)
                //{
                //    decimal priceFull = 0;
                //    if (tmp.Prices.ContainsKey(currHAEstate.num_max_sleep.objToInt32()))
                //        priceFull = tmp.Prices[currHAEstate.num_max_sleep.objToInt32()];
                //    else if (tmp.Prices.ContainsKey(currEstate.num_persons_max.objToInt32()))
                //        priceFull = tmp.Prices[currEstate.num_persons_max.objToInt32()];
                //    else if (tmp.Prices.Keys.Count > 0)
                //        priceFull = tmp.Prices.Last().Value;
                //    if (priceFull == 0) priceFull = 9999;
                //    var currRatePeriod = new ChnlHomeAwayClasses.ListingClasses.RatePeriod();
                //    currRatePeriod.dateRange.beginDate.ValueDate = tmp.DtStart;
                //    currRatePeriod.dateRange.endDate.ValueDate = tmp.DtEnd;
                //    currRatePeriod.minimumStay = tmp.MinNights;
                //    currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("NIGHTLY_WEEKDAY", priceFull));
                //    currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("NIGHTLY_WEEKEND", priceFull));
                //    currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("EXTRA_NIGHT", priceFull));
                //    //currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("WEEKEND",priceFull));
                //    //currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("WEEKLY",priceFull));
                //    //currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("MONTHLY",priceFull));
                //    currUnit.ratePeriods.Add(currRatePeriod);
                //} 
                #endregion

                #region Removed unitAvailability from content xml
                //// TODO unitAvailability, Finalaize
                //currUnit.unitAvailability.dateRange.beginDate.ValueDate = dtStart;
                //currUnit.unitAvailability.dateRange.endDate.ValueDate = dtEnd;
                //var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                //DateTime dtCurrent = dtStart;
                //while (dtCurrent < dtEnd)
                //{
                //    bool isAvv = resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count() == 0;
                //    var priceList = priceListPerDates.FirstOrDefault(x => x.DtStart <= dtCurrent && x.DtEnd >= dtCurrent);
                //    currUnit.unitAvailability.unitAvailabilityConfiguration.availability.Add(isAvv ? "Y" : "N");
                //    currUnit.unitAvailability.unitAvailabilityConfiguration.changeOver.Add("C");
                //    //currUnit.unitAvailability.unitAvailabilityConfiguration.maxStay.Add("");
                //    //currUnit.unitAvailability.unitAvailabilityConfiguration.minPriorNotify.Add("");
                //    currUnit.unitAvailability.unitAvailabilityConfiguration.minStay.Add(priceList != null ? priceList.MinNights + "" : currEstate.nights_min.objToInt32() + "");
                //    currUnit.unitAvailability.unitAvailabilityConfiguration.stayIncrement.Add("D");
                //    dtCurrent = dtCurrent.AddDays(1);
                //}
                #endregion

                var currunitLnList = dcChnl.RntChnlHomeAwayEstateLN.Where(x => x.pid_estate == unitEstate.id).ToList();

                foreach (var ln in currunitLnList)
                {

                    string HACode = contUtils.getLanguage_HAcodeByAbbr(ln.pid_lang);


                    if (string.IsNullOrEmpty(HACode))
                        continue;

                    if (string.IsNullOrWhiteSpace(ln.title))
                    {
                        ln.title = ln.unit_name;
                    }
                    if (string.IsNullOrWhiteSpace(ln.unit_description) && (ln.pid_lang == app_DefaultLang_Code))
                    {
                        if (!string.IsNullOrWhiteSpace(ln.title))
                        {
                            ln.unit_description = ln.title;
                        }
                        else if (DC_CONTENT.CONT_TBL_LANGs.Any(x => x.abbr == ln.pid_lang))
                        {
                            int langid = DC_CONTENT.CONT_TBL_LANGs.FirstOrDefault(x => x.code == ln.pid_lang).id;
                            ln.unit_description = CurrentSource.rntEstate_title(IdEstate, langid, currEstate.code);
                        }

                    }
                    currUnit.bathroomDetails.Add(ln.bathroom_details, HACode);
                    currUnit.bedroomDetails.Add(ln.bedroom_details, HACode);
                    currUnit.description.Add(ln.unit_description, HACode);
                    currUnit.featuresDescription.Add(HaListingUtility_V411.RemoveHtmlTags(ln.features_description), HACode);
                    currUnit.unitName.Add(ln.unit_name, HACode);
                }

                Response.units.Add(currUnit);
            }

            return Response.GetXml();
        }
    }

    public static string Contact(int IdEstate)
    {
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("Contact");
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaUser_DataContext DC_USER = maga_DataContext.DC_USER)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        {
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "Contact", "Owner for HomeAway was not found or not active");
                    return "";
                }
                RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
                if (currEstate == null)
                {
                    return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "Contact");
                }
                RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
                if (currHAEstate == null)
                {
                    return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "Contact");
                }

                USR_TBL_OWNER ownerDetail = DC_USER.USR_TBL_OWNER.FirstOrDefault(i => i.id == currEstate.pid_owner && i.is_active == 1);
                if (ownerDetail == null)
                {
                    ErrorLog.addLog("", "Contact", "Owner of Estate was not found or not active");
                    return "";
                }
                string lang_Id = "en";
                //if (ownerDetail.pid_lang.objToInt32() > 0)
                //{
                //    lang_Id = contUtils.getLang_commonName(ownerDetail.pid_lang.objToInt32()).Replace("-", "_");
                //}


                ChnlHomeAwayClasses_V411.ListingClasses.Contact Response = new ChnlHomeAwayClasses_V411.ListingClasses.Contact();
                if (!string.IsNullOrEmpty(ownerDetail.contact_email))
                {
                    Response.email = ownerDetail.contact_email;
                }
                else if (!string.IsNullOrEmpty(ownerDetail.contact_email_2))
                {
                    Response.email = ownerDetail.contact_email;
                }
                else if (!string.IsNullOrEmpty(ownerDetail.contact_email_3))
                {
                    Response.email = ownerDetail.contact_email;
                }
                else if (!string.IsNullOrEmpty(ownerDetail.contact_email_4))
                {
                    Response.email = ownerDetail.contact_email;
                }
                else if (!string.IsNullOrEmpty(ownerDetail.contact_email_5))
                {
                    Response.email = ownerDetail.contact_email;
                }
                Response.fax.Add(ownerDetail.contact_fax, lang_Id);


                if (!string.IsNullOrEmpty(ownerDetail.name_full))
                {
                    Response.name.Add(ownerDetail.name_full, lang_Id);
                }
                //else if (!string.IsNullOrEmpty(ownerDetail.companyName))
                //{
                //    Response.name.Add(ownerDetail.companyName, lang_Id);
                //}
                Response.phone1.Add(ownerDetail.contact_phone, lang_Id);
                Response.phone2.Add(ownerDetail.contact_phone_office, lang_Id);
                Response.phone3.Add(ownerDetail.contact_phone_mobile, lang_Id);


                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "Contact", ex.ToString());
            } return errorList.GetXml();
        }
    }


    public static string LodgingConfigurationContent(int IdEstate)
    {


        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("LodgingConfigurationContent");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "LodgingConfigurationContent", "Owner for HomeAway was not found or not active");
                    return "";
                }
                RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
                if (currEstate == null)
                {
                    return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "LodgingConfigurationContent");
                }
                RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
                if (currHAEstate == null)
                {
                    return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "LodgingConfigurationContent");
                }

                ChnlHomeAwayClasses_V411.ListingClasses.LodgingConfigurationContent Response = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingConfigurationContent();
                Response.LodgingConfigurationDefaultEstateWise.IsEstateWise = true;
                ChnlHomeAwayClasses_V411.LodgingConfigurationDefaults lodgingConfigurationDefaultsEstateWise = new ChnlHomeAwayClasses_V411.LodgingConfigurationDefaults();


                Response.listingExternalId = currHAEstate.is_slave == 1 ? currHAEstate.pid_master_estate + "" : currEstate.id + "";
                Response.unitExternalId = currEstate.id + "";

                var estate = AppSettings.RNT_estateList.FirstOrDefault(x => x.id == IdEstate);

                #region Nodes not supplied in Estate Wise as Its taken from Defaults
                //List<dbRntChnlHomeAwayAcceptedPaymentFormTBL> chnlPaymentforms = dcChnl.dbRntChnlHomeAwayAcceptedPaymentFormTBLs.Where(i => i.isActive == 1).ToList();
                //if (chnlPaymentforms != null && chnlPaymentforms.Count > 0)
                //{
                //    foreach (dbRntChnlHomeAwayAcceptedPaymentFormTBL paymentforms in chnlPaymentforms)
                //    {
                //        ChnlHomeAwayClasses.paymentCardDescriptor paymentCardDescriptor = new ChnlHomeAwayClasses.paymentCardDescriptor();
                //        paymentCardDescriptor.paymentFormType = paymentforms.paymentFormType;
                //        paymentCardDescriptor.cardType = paymentforms.cardType;
                //        paymentCardDescriptor.cardCode = paymentforms.cardCode;
                //        lodgingConfigurationDefaultsEstateWise.AcceptedPaymentForm.paymentCardDescriptors.Add(paymentCardDescriptor);
                //    }

                //}

                //lodgingConfigurationDefaults.CancellationPolicy.policy = ChnlHomeAwayClasses.LodgingConfigurationStaticData.cancellationPolicy;
                //lodgingConfigurationDefaultsEstateWise.EventsAllowedRule.allowed = ChnlHomeAwayClasses.LodgingConfigurationStaticData.eventsAllowedRule;
                //lodgingConfigurationDefaultsEstateWise.minimumAgeRule.age = ChnlHomeAwayClasses.LodgingConfigurationStaticData.minAgeRule;
                //lodgingConfigurationDefaultsEstateWise.smokingAllowedRule.allowed = ChnlHomeAwayClasses.LodgingConfigurationStaticData.smokingAllowedRule;
                //lodgingConfigurationDefaultsEstateWise.pricingPolicy.policy = ChnlHomeAwayClasses.LodgingConfigurationStaticData.pricingPolicy;

                #endregion

                lodgingConfigurationDefaultsEstateWise.BookingPolicy.policy = currEstate.is_online_booking == 1 ? "INSTANT" : "QUOTEHOLD";
                lodgingConfigurationDefaultsEstateWise.checkInTime = "15:00";
                lodgingConfigurationDefaultsEstateWise.checkOutTime = "10:00";

                lodgingConfigurationDefaultsEstateWise.ChildrenAllowedRule.allowed = (currEstate.num_persons_child > 0);

                if (estate != null && estate.pid_lang.objToInt32() > 0)
                {
                    lodgingConfigurationDefaultsEstateWise.locale = contUtils.getLanguage_HAcode(estate.pid_lang);
                }
                //num_max_sleep , Numpersonmax
                lodgingConfigurationDefaultsEstateWise.MaximumOccupancyRule.guests = 0;
                if (currHAEstate.num_max_sleep != null && currHAEstate.num_max_sleep > 0)
                {
                    lodgingConfigurationDefaultsEstateWise.MaximumOccupancyRule.guests = currHAEstate.num_max_sleep;
                }
                else if (currEstate.num_persons_max != null && currEstate.num_persons_max > 0)
                {
                    lodgingConfigurationDefaultsEstateWise.MaximumOccupancyRule.guests = currEstate.num_persons_max;
                }
                //if (currEstate.num_pets_max != null && currEstate.num_pets_max > 0)
                //{
                //    lodgingConfigurationDefaultsEstateWise.petsAllowedRule.allowed = true;
                //}

                // Locale of the file can be diff so Url added estate wise
                lodgingConfigurationDefaultsEstateWise.rentalAgreementFile.rentalAgreementPdfUrl = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.rentalAgreementPdfUrl;

                lodgingConfigurationDefaultsEstateWise.rentalAgreementFile.locale = ChnlHomeAwayClasses_V411.LodgingConfigurationStaticData.locale;
                if (estate.pid_lang.objToInt32() > 0)
                {
                    string HACode = contUtils.getLanguage_HAcode(estate.pid_lang);
                    if (!string.IsNullOrEmpty(HACode))
                    {
                        lodgingConfigurationDefaultsEstateWise.rentalAgreementFile.locale = HACode;

                    }
                }
                Response.LodgingConfigurationDefaultEstateWise = lodgingConfigurationDefaultsEstateWise;

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "LodgingConfigurationContent", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string LodgingRateContent(int IdEstate)
    {

        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("LodgingRateContent");

        try
        {

            using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
            using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            {
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl == null)
                {
                    return "";
                }
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "LodgingRateContent", " :Owner for HomeAway was not found or not active");
                    return "";
                }

                RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
                if (currEstate == null)
                {
                    return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "LodgingRateContent");
                }

                RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
                if (currHAEstate == null)
                {
                    return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "LodgingRateContent");
                }

                //var Units = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.id == IdEstate || (x.is_slave == 1 && x.pid_master_estate == IdEstate)).ToList();
                var unit = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
                //var currLnList = dcChnl.dbRntChnlHomeAwayEstateLNs.Where(x => x.pid_estate == IdEstate).ToList();

                var dtStart = DateTime.Now.Date;
                var dtEnd = DateTime.Now.AddYears(2).Date;
                var dtEndTemp = dtEnd.AddDays(1);

                ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateContent Response = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateContent();

                Response.listingExternalId = unit.is_slave == 1 ? unit.pid_master_estate + "" : currEstate.id + "";
                Response.unitExternalId = currEstate.id + "";
                Response.lodgingRate.currency = "EUR"; // We send data in EUR to HA Below Conversation done 
                Response.lodgingRate.language = "en";


                // TODO ratePeriods, Finalaize
                int outError;
                #region

                var priceListPerDatesNew = new List<ChnlHomeAwayClasses_V411.PriceListPerDates>();
                var priceListPerDatesTmp = rntUtils.estate_getPriceListPerDates(IdEstate, agentTbl.id, dtStart, dtEndTemp, out outError, ChnlHomeAwayProps_V411.IdAdMedia);

                foreach (var tmp in priceListPerDatesTmp)
                {
                    DateTime dtCurrent = tmp.DtStart;

                    //var currNightRange = rntProps.EstateNightRangeTB.SingleOrDefault(x => x.id == tmp.Prices.Last().Value);


                    while (dtCurrent <= tmp.DtEnd)
                    {
                        decimal priceFull = 0;
                        if (tmp.Prices.ContainsKey(currHAEstate.num_max_sleep.objToInt32()))
                            priceFull = tmp.Prices[currHAEstate.num_max_sleep.objToInt32()];
                        else if (tmp.Prices.ContainsKey(currEstate.num_persons_max.objToInt32()))
                            priceFull = tmp.Prices[currEstate.num_persons_max.objToInt32()];
                        else if (tmp.Prices.Keys.Count > 0)
                            priceFull = tmp.Prices.Last().Value;
                        if (priceFull == 0) priceFull = 9999;

                        var datePrices = new ChnlHomeAwayClasses_V411.PriceListPerDates(dtCurrent, dtCurrent, tmp.MinStay, priceFull);
                        //if (currNightRange != null)
                        //{
                        //    datePrices.NightsFrom = Convert.ToString(currNightRange.nightsFrom);
                        //    datePrices.NightsTo = Convert.ToString(currNightRange.nightsTo);
                        //    datePrices.Name = datePrices.NightsFrom + "To" + datePrices.NightsTo;
                        //}

                        #region HA 4.1.1 minNights Calculations not reuired hence commented
                        //var minStayDate = minStayList.FirstOrDefault(x => x.changeDate == dtCurrent);
                        //if (minStayDate != null) datePrices.MinNights = minStayDate.minStay;
                        #endregion

                        //var rateChange = changesListTmp.FirstOrDefault(x => x.changeDate == dtCurrent);
                        //if (rateChange != null)
                        //{
                        //    decimal changeAmount = (rateChange.changeIsPercentage == 1) ? (priceFull * rateChange.changeAmount.objToInt32() / 100) : rateChange.changeAmount.objToInt32();
                        //    if (rateChange.changeIsDiscount == 1) { changeAmount = -changeAmount; }
                        //    datePrices.PriceHA = priceFull + changeAmount;
                        //}
                        //else
                        //{
                        decimal changeAmountGeneric = 0;
                        if (currHAEstate.changeIsDiscount >= 0 && currHAEstate.changeAmount > 0)
                        {
                            changeAmountGeneric = (currHAEstate.changeIsPercentage == 1) ? (priceFull * currHAEstate.changeAmount.objToInt32() / 100) : currHAEstate.changeAmount.objToInt32();
                            if (currHAEstate.changeIsDiscount == 1) { changeAmountGeneric = -changeAmountGeneric; }
                        }
                        datePrices.PriceHA = priceFull + changeAmountGeneric;
                        //}

                        var lastDatePrice = priceListPerDatesNew.LastOrDefault();
                        if (lastDatePrice == null || !lastDatePrice.HasSamePricesHA(datePrices)) priceListPerDatesNew.Add(datePrices);
                        else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);

                        dtCurrent = dtCurrent.AddDays(1);
                    }

                }
                //}

                var NightlyOverrides = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightlyOverrides();
                var FlatAmountDiscounts = new ChnlHomeAwayClasses_V411.ListingClasses.FlatAmountDiscounts();
                decimal? firstandDefaultRate = null;


                foreach (var tmp in priceListPerDatesNew)
                {
                    decimal priceFull = 0;

                    priceFull = tmp.PriceHA * 1;
                    if (priceFull == 0) priceFull = 9999;

                    decimal? priceFull_Round = Math.Round(priceFull, 2);

                    if (firstandDefaultRate == null)
                    {
                        var Overrides = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightlyOverrideContent();
                        var Range = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightRange();

                        Range.min.ValueDate = tmp.DtStart;
                        Range.max.ValueDate = tmp.DtEnd;

                        Overrides.LodgingRateNights.Ranges.Add(Range);

                        firstandDefaultRate = priceFull_Round;
                        Overrides.Amount.ValueDecimal = priceFull_Round;
                        NightlyOverrides.LodgingRateNightlyOverrideContents.Add(Overrides);
                    }

                    else
                    {
                        //if (currEstate.priceVersion == 6)
                        //{
                        //    #region Version 6
                        //    if (FlatAmountDiscounts.FltDiscounts.Any(i => i.Amount.ValueDecimal == priceFull_Round))
                        //    {
                        //        ChnlHomeAwayClasses_V411.ListingClasses.FltDiscount FltDiscount = FlatAmountDiscounts.FltDiscounts.FirstOrDefault(i => i.Amount.ValueDecimal == priceFull_Round);
                        //        if (FltDiscount.appliesPerNight.staysOfNight.Ranges.Count < 5 && !FltDiscount.appliesPerNight.staysOfNight.Ranges.Any(x => x.min == tmp.NightsFrom && x.max == tmp.NightsTo))
                        //        {
                        //            if (FltDiscount.Name.Length < 60)
                        //            { FltDiscount.Name += " / " + tmp.Name; };

                        //            ChnlHomeAwayClasses_V411.ListingClasses.NightRange nightRange = new ChnlHomeAwayClasses_V411.ListingClasses.NightRange();
                        //            nightRange.min = tmp.NightsFrom;
                        //            nightRange.max = tmp.NightsTo;
                        //            FltDiscount.appliesPerNight.staysOfNight.Ranges.Add(nightRange);
                        //        }

                        //    }
                        //    else if (FlatAmountDiscounts.FltDiscounts.Count < 15) // 15 Limit of Discount and 1 will go in Nightly override so 16
                        //    {
                        //        ChnlHomeAwayClasses_V411.ListingClasses.FltDiscount FltDiscount = new ChnlHomeAwayClasses_V411.ListingClasses.FltDiscount();
                        //        FltDiscount.Name = "Nights From: " + tmp.Name;
                        //        FltDiscount.Amount.ValueDecimal = priceFull_Round;
                        //        ChnlHomeAwayClasses_V411.ListingClasses.NightRange nightRange = new ChnlHomeAwayClasses_V411.ListingClasses.NightRange();
                        //        nightRange.min = tmp.NightsFrom;
                        //        nightRange.max = tmp.NightsTo;
                        //        FltDiscount.appliesPerNight.staysOfNight.Ranges.Add(nightRange);
                        //        FlatAmountDiscounts.FltDiscounts.Add(FltDiscount);
                        //    }
                        //    #endregion
                        //}
                        //else
                        //{
                        if (NightlyOverrides.LodgingRateNightlyOverrideContents.Any(i => i.Amount.ValueDecimal == priceFull_Round))
                        {
                            ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightlyOverrideContent Overrides = NightlyOverrides.LodgingRateNightlyOverrideContents.FirstOrDefault(i => i.Amount.ValueDecimal == priceFull_Round);
                            if (!Overrides.LodgingRateNights.Ranges.Any(i => i.max.ValueDate == tmp.DtStart && i.min.ValueDate == tmp.DtStart))
                            {
                                var Range = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightRange();

                                Range.min.ValueDate = tmp.DtStart;
                                Range.max.ValueDate = tmp.DtEnd;

                                Overrides.LodgingRateNights.Ranges.Add(Range);
                            }
                        }
                        else
                        {
                            var Overrides = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightlyOverrideContent();
                            var Range = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightRange();

                            Range.min.ValueDate = tmp.DtStart;
                            Range.max.ValueDate = tmp.DtEnd;

                            Overrides.LodgingRateNights.Ranges.Add(Range);
                            Overrides.Amount.ValueDecimal = priceFull_Round;
                            NightlyOverrides.LodgingRateNightlyOverrideContents.Add(Overrides);
                        }

                        //}
                    }


                }

                #endregion

                if (firstandDefaultRate != null)
                {
                    Response.lodgingRate.NightlyRates.DefaultNRateMonToSunday.ValueDecimal = firstandDefaultRate;
                }

                Response.lodgingRate.NightlyRates.LodgingRateNightlyOverrides = NightlyOverrides;

                Response.lodgingRate.Discounts = FlatAmountDiscounts;
                var LodgingRatePayments = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRatePayments();
                LodgingRatePayments.dueType = ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateDefaultData.DueType;
                //LodgingRatePayments HA 4.1.1 Limit 5
                Response.lodgingRate.LodgingRatePaymentSchedule.LodgingRatePayments.Add(LodgingRatePayments);

                #region Current
                //var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, dtStart, dtEnd, 0, out outError);
                ////var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, dtStart, dtEnd, 0, out outError,true);
                //foreach (var tmp in priceListPerDates)
                //{
                //    decimal priceFull = 0;
                //    if (tmp.Prices.ContainsKey(currHAEstate.num_max_sleep.objToInt32()))
                //        priceFull = tmp.Prices[currHAEstate.num_max_sleep.objToInt32()];
                //    else if (tmp.Prices.ContainsKey(currEstate.num_persons_max.objToInt32()))
                //        priceFull = tmp.Prices[currEstate.num_persons_max.objToInt32()];
                //    else if (tmp.Prices.Keys.Count > 0)
                //        priceFull = tmp.Prices.Last().Value;
                //    if (priceFull == 0) priceFull = 9999;
                //    else
                //    {
                //        decimal changeAmount = 0;
                //        if (currHAEstate.changeIsDiscount >= 0 && currHAEstate.changeAmount > 0)
                //        {
                //            changeAmount = (currHAEstate.changeIsPercentage == 1) ? (priceFull * currHAEstate.changeAmount.objToInt32() / 100) : currHAEstate.changeAmount.objToInt32();
                //            if (currHAEstate.changeIsDiscount == 1) { changeAmount = -changeAmount; }
                //        }
                //        priceFull = priceFull + changeAmount;
                //    }
                //    var currRatePeriod = new ChnlHomeAwayClasses.ListingClasses.RatePeriod();
                //    currRatePeriod.dateRange.beginDate.ValueDate = tmp.DtStart;
                //    currRatePeriod.dateRange.endDate.ValueDate = tmp.DtEnd;
                //    currRatePeriod.minimumStay = tmp.MinNights;
                //    currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("NIGHTLY_WEEKDAY", priceFull));
                //    currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("NIGHTLY_WEEKEND", priceFull));
                //    currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("EXTRA_NIGHT", priceFull));
                //    //currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("WEEKEND",priceFull));
                //    //currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("WEEKLY",priceFull));
                //    //currRatePeriod.rates.Add(new ChnlHomeAwayClasses.ListingClasses.Rate("MONTHLY",priceFull));
                //    Response.unitRatePeriods.Add(currRatePeriod);
                //}

                #endregion

                string returnOutput = Response.GetXml();

                return returnOutput;
            }
        }
        catch (Exception ex)
        {
            errorList.AddError("OTHER");
            ErrorLog.addLog("", "LodgingRateContent", ex.ToString());
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                var StackTrace = ex.StackTrace.ToString();
                ErrorLog.addLog("", "LodgingRateContent StackTrace", StackTrace);
            }
        }
        return errorList.GetXml();


    }

    public static ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateUpdateRequest LodgingRateUpdateRequestContent(int IdEstate)
    {

        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("LodgingRateUpdateRequestContent");

        try
        {
            using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
            using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "LodgingRateUpdateRequestContent", "Owner for HomeAway was not found or not active");
                    return null;
                }
                RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
                if (currEstate == null)
                {
                    return null;
                }
                RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
                if (currHAEstate == null)
                {
                    return null;
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl == null) return null;

                //var Units = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.id == IdEstate || (x.is_slave == 1 && x.pid_master_estate == IdEstate)).ToList();
                var unit = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
                //var currLnList = dcChnl.dbRntChnlHomeAwayEstateLNs.Where(x => x.pid_estate == IdEstate).ToList();

                var dtStart = DateTime.Now.Date;
                var dtEnd = DateTime.Now.AddYears(2).Date;
                var dtEndTemp = dtEnd.AddDays(1);

                ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateUpdateRequest Response = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateUpdateRequest();

                Response.listingExternalId = unit.is_slave == 1 ? unit.pid_master_estate + "" : currEstate.id + "";
                Response.unitExternalId = currEstate.id + "";
                Response.lodgingRate.currency = "EUR"; // We send data in EUR to HA Below Conversation done 
                Response.lodgingRate.language = "en";


                // TODO ratePeriods, Finalaize
                int outError;
                #region
                //var minStayList = dcChnl.dbRntChnlHomeAwayEstateClosedDatesRLs.Where(x => x.pidEstate == IdEstate && x.minStay > 0 && x.changeDate >= dtStart).ToList();
                //var changesListTmp = dcChnl.dbRntChnlHomeAwayEstateRateChangesRLs.Where(x => x.pidEstate == IdEstate && x.changeIsDiscount >= 0 && x.changeDate >= dtStart).ToList();
                //var changesList = new List<dbRntChnlHomeAwayEstateRateChangesRL>();

                var priceListPerDatesNew = new List<ChnlHomeAwayClasses_V411.PriceListPerDates>();
                var priceListPerDatesTmp = rntUtils.estate_getPriceListPerDates(IdEstate, agentTbl.id, dtStart, dtEndTemp, out outError);

                foreach (var tmp in priceListPerDatesTmp)
                {
                    DateTime dtCurrent = tmp.DtStart;
                    //var currNightRange = rntProps.EstateNightRangeTB.SingleOrDefault(x => x.id == tmp.Prices.Last().Value);
                    while (dtCurrent <= tmp.DtEnd)
                    {
                        decimal priceFull = 0;
                        if (tmp.Prices.ContainsKey(currHAEstate.num_max_sleep.objToInt32()))
                            priceFull = tmp.Prices[currHAEstate.num_max_sleep.objToInt32()];
                        else if (tmp.Prices.ContainsKey(currEstate.num_persons_max.objToInt32()))
                            priceFull = tmp.Prices[currEstate.num_persons_max.objToInt32()];
                        else if (tmp.Prices.Keys.Count > 0)
                            priceFull = tmp.Prices.Last().Value;
                        if (priceFull == 0) priceFull = 9999;

                        var datePrices = new ChnlHomeAwayClasses_V411.PriceListPerDates(dtCurrent, dtCurrent, tmp.MinStay, priceFull);

                        //if (currNightRange != null)
                        //{
                        //    datePrices.NightsFrom = Convert.ToString(currNightRange.nightsFrom);
                        //    datePrices.NightsTo = Convert.ToString(currNightRange.nightsTo);
                        //    datePrices.Name = datePrices.NightsFrom + "To" + datePrices.NightsTo;
                        //}

                        #region HA 4.1.1 minNights Calculations not reuired hence commented
                        //var minStayDate = minStayList.FirstOrDefault(x => x.changeDate == dtCurrent);
                        //if (minStayDate != null) datePrices.MinNights = minStayDate.minStay;
                        #endregion

                        decimal changeAmountGeneric = 0;
                        if (currHAEstate.changeIsDiscount >= 0 && currHAEstate.changeAmount > 0)
                        {
                            changeAmountGeneric = (currHAEstate.changeIsPercentage == 1) ? (priceFull * currHAEstate.changeAmount.objToInt32() / 100) : currHAEstate.changeAmount.objToInt32();
                            if (currHAEstate.changeIsDiscount == 1) { changeAmountGeneric = -changeAmountGeneric; }
                        }

                        datePrices.PriceHA = priceFull + changeAmountGeneric;

                        var lastDatePrice = priceListPerDatesNew.LastOrDefault();
                        if (lastDatePrice == null || !lastDatePrice.HasSamePricesHA(datePrices)) priceListPerDatesNew.Add(datePrices);
                        else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);

                        dtCurrent = dtCurrent.AddDays(1);
                    }
                }
                //}
                ChnlHomeAwayClasses_V411.ListingClasses.NightlyRates currNightlyRates = new ChnlHomeAwayClasses_V411.ListingClasses.NightlyRates();
                var NightlyOverrides = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightlyOverrides();
                var FlatAmountDiscounts = new ChnlHomeAwayClasses_V411.ListingClasses.FlatAmountDiscounts();

                decimal? firstandDefaultRate = null;

                foreach (var tmp in priceListPerDatesNew)
                {
                    decimal priceFull = 0;

                    priceFull = tmp.PriceHA * 1;
                    if (priceFull == 0) priceFull = 9999;

                    decimal? priceFull_Round = Math.Round(priceFull, 2);

                    if (firstandDefaultRate == null)
                    {

                        var Overrides = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightlyOverrideContent();
                        var Range = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateNightRange();

                        Range.min.ValueDate = tmp.DtStart;
                        Range.max.ValueDate = tmp.DtEnd;

                        Overrides.LodgingRateNights.Ranges.Add(Range);

                        firstandDefaultRate = priceFull_Round;
                        Overrides.Amount.ValueDecimal = priceFull_Round;
                        NightlyOverrides.LodgingRateNightlyOverrideContents.Add(Overrides);
                    }
                    else
                    {
                        if (FlatAmountDiscounts.FltDiscounts.Any(i => i.Amount.ValueDecimal == priceFull_Round && i.appliesPerNight.staysOfNight.Ranges.Count < 5))
                        {
                            ChnlHomeAwayClasses_V411.ListingClasses.FltDiscount FltDiscount = FlatAmountDiscounts.FltDiscounts.FirstOrDefault(i => i.Amount.ValueDecimal == priceFull_Round);
                            if (!FltDiscount.appliesPerNight.staysOfNight.Ranges.Any(x => x.min == tmp.NightsFrom && x.max == tmp.NightsTo))
                            {
                                FltDiscount.Name += " / " + tmp.Name;
                                ChnlHomeAwayClasses_V411.ListingClasses.NightRange nightRange = new ChnlHomeAwayClasses_V411.ListingClasses.NightRange();
                                nightRange.min = tmp.NightsFrom;
                                nightRange.max = tmp.NightsTo;
                                FltDiscount.appliesPerNight.staysOfNight.Ranges.Add(nightRange);
                            }

                        }
                        else if (FlatAmountDiscounts.FltDiscounts.Count < 15) // 15 Limit of Discount and 1 will go in Nightly override so 16
                        {
                            ChnlHomeAwayClasses_V411.ListingClasses.FltDiscount FltDiscount = new ChnlHomeAwayClasses_V411.ListingClasses.FltDiscount();
                            FltDiscount.Name = "Nights From: " + tmp.Name;
                            FltDiscount.Amount.ValueDecimal = priceFull_Round;
                            ChnlHomeAwayClasses_V411.ListingClasses.NightRange nightRange = new ChnlHomeAwayClasses_V411.ListingClasses.NightRange();
                            nightRange.min = tmp.NightsFrom;
                            nightRange.max = tmp.NightsTo;
                            FltDiscount.appliesPerNight.staysOfNight.Ranges.Add(nightRange);
                            FlatAmountDiscounts.FltDiscounts.Add(FltDiscount);
                        }

                    }

                }
                #endregion

                if (firstandDefaultRate != null)
                {
                    Response.lodgingRate.NightlyRates.DefaultNRateMonToSunday.ValueDecimal = firstandDefaultRate;
                }

                Response.lodgingRate.NightlyRates.LodgingRateNightlyOverrides = NightlyOverrides;
                Response.lodgingRate.Discounts = FlatAmountDiscounts;
                var LodgingRatePayments = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingRatePayments();
                LodgingRatePayments.dueType = ChnlHomeAwayClasses_V411.ListingClasses.LodgingRateDefaultData.DueType;
                //LodgingRatePayments HA 4.1.1 Limit 5
                Response.lodgingRate.LodgingRatePaymentSchedule.LodgingRatePayments.Add(LodgingRatePayments);


                return Response;
            }
        }
        catch (Exception ex)
        {
            errorList.AddError("OTHER");
            ErrorLog.addLog("", "LodgingRateUpdateRequestContent", ex.ToString());
        }
        return null;


    }


    public static string ListingAvailability(int IdEstate)
    {
        using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        {
            var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
            if (ownerTbl == null)
            {
                ErrorLog.addLog("", "UnitAvailability", "Owner for HomeAway was not found or not active");
                return "";
            }
            RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
            if (currEstate == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "unitAvailabilityEntities");
            }
            RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
            if (currHAEstate == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "unitAvailabilityEntities");
            }
            //var Units = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.id == IdEstate || (x.is_slave == 1 && x.pid_master_estate == IdEstate)).ToList();
            var unit = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
            //var currLnList = dcChnl.dbRntChnlHomeAwayEstateLNs.Where(x => x.pid_estate == IdEstate && x.pid_lang != "enu").ToList();

            var dtStart = DateTime.Now.Date;
            var dtEnd = DateTime.Now.AddYears(2).Date;
            var dtEndTemp = dtEnd.AddDays(1);

            ChnlHomeAwayClasses_V411.ListingClasses.UnitAvailabilityContent Response = new ChnlHomeAwayClasses_V411.ListingClasses.UnitAvailabilityContent();

            Response.listingExternalId = unit.is_slave == 1 ? unit.pid_master_estate + "" : currEstate.id + "";
            Response.unitExternalId = currEstate.id + "";

            // TODO unitAvailability, Finalaize
            Response.unitAvailability.dateRange.beginDate.ValueDate = dtStart;
            Response.unitAvailability.dateRange.endDate.ValueDate = dtEnd;

            int maxStay = currEstate.nights_max.objToInt32();
            if (maxStay > 0)
            {
                Response.unitAvailability.maxStayDefault = maxStay > 999 ? 999 : maxStay;
            }

            var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEndTemp, 0).OrderBy(x => x.dtStart).ToList();

            DateTime dtCurrent = dtStart;
            int outError;

            var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
            var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, agentTbl.id, dtStart, dtEndTemp, out outError);

            //var closedList = dcChnl.dbRntChnlHomeAwayEstateClosedDatesRLs.Where(x => x.pidEstate == IdEstate && x.changeDate >= dtStart).ToList();



            while (dtCurrent < dtEndTemp)
            {
                //var tmpDate = closedList.FirstOrDefault(x => x.changeDate == dtCurrent);
                bool isAvv = false;

                bool isAnyReseration = resList.Any(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent && x.agentID == agentTbl.id);
                isAvv = !isAnyReseration;
                var units = 0;
                if (isAvv)
                {
                    int baseAvv = currEstate.baseAvailability.objToInt32() < 1 ? 1 : currEstate.baseAvailability.objToInt32();
                    //baseAvv = baseAvv - removedBaseAvv;
                    units = baseAvv - resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count();
                    isAvv = units > 0;
                    //bool isAvv = resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count() == 0;
                }
                var priceList = priceListPerDates.FirstOrDefault(x => x.DtStart <= dtCurrent && x.DtEnd >= dtCurrent);

                Response.unitAvailability.unitAvailabilityConfiguration.availability.Add(priceList != null ? (isAvv ? "Y" : "N") : "N");

                int availableUnitCount = units > 0 ? units : 0;
                Response.unitAvailability.unitAvailabilityConfiguration.availableUnitCount.Add(availableUnitCount + "");
                Response.unitAvailability.unitAvailabilityConfiguration.changeOver.Add("C");
                //currUnit.unitAvailability.unitAvailabilityConfiguration.maxStay.Add("");
                //currUnit.unitAvailability.unitAvailabilityConfiguration.minPriorNotify.Add("");
                Response.unitAvailability.unitAvailabilityConfiguration.minStay.Add(priceList != null ? priceList.MinStay + "" : currEstate.nights_min.objToInt32() + "");
                Response.unitAvailability.unitAvailabilityConfiguration.stayIncrement.Add("D");
                dtCurrent = dtCurrent.AddDays(1);
            }
            return Response.GetXml();
        }
    }

    public static ChnlHomeAwayClasses_V411.ListingClasses.LodgingUnitAvailabilityUpdateRequest LodgingAvailabilityUpdateRequestContent(int IdEstate)
    {
        using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        {
            var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
            if (ownerTbl == null)
            {
                ErrorLog.addLog("", "UnitAvailability", "Owner for HomeAway was not found or not active");
                return null;
            }
            RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
            if (currEstate == null)
            {
                return null;
            }
            RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
            if (currHAEstate == null)
            {
                return null;
            }
            //var Units = dcChnl.dbRntChnlHomeAwayEstateTBs.Where(x => x.id == IdEstate || (x.is_slave == 1 && x.pid_master_estate == IdEstate)).ToList();
            var unit = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == IdEstate);
            //var currLnList = dcChnl.dbRntChnlHomeAwayEstateLNs.Where(x => x.pid_estate == IdEstate && x.pid_lang != "enu").ToList();

            var dtStart = DateTime.Now.Date;
            var dtEnd = DateTime.Now.AddYears(2).Date;
            var dtEndTemp = dtEnd.AddDays(1);

            ChnlHomeAwayClasses_V411.ListingClasses.LodgingUnitAvailabilityUpdateRequest Response = new ChnlHomeAwayClasses_V411.ListingClasses.LodgingUnitAvailabilityUpdateRequest();

            Response.listingExternalId = unit.is_slave == 1 ? unit.pid_master_estate + "" : currEstate.id + "";
            Response.unitExternalId = currEstate.id + "";

            // TODO unitAvailability, Finalaize
            Response.unitAvailability.dateRange.beginDate.ValueDate = dtStart;
            Response.unitAvailability.dateRange.endDate.ValueDate = dtEnd;

            int maxStay = currEstate.nights_max.objToInt32();
            if (maxStay > 0)
            {
                Response.unitAvailability.maxStayDefault = maxStay > 999 ? 999 : maxStay;
            }

            var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEndTemp, 0).OrderBy(x => x.dtStart).ToList();


            DateTime dtCurrent = dtStart;
            int outError;
            var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
            var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, agentTbl.id, dtStart, dtEndTemp, out outError);

            while (dtCurrent < dtEndTemp)
            {
                //var tmpDate = closedList.FirstOrDefault(x => x.changeDate == dtCurrent);
                bool isAvv = false;
                bool isAnyReseration = resList.Any(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent && x.agentID == agentTbl.id);
                isAvv = !isAnyReseration;
                var units = 0;
                if (isAvv)
                {
                    int baseAvv = currEstate.baseAvailability.objToInt32() < 1 ? 1 : currEstate.baseAvailability.objToInt32();
                    //baseAvv = baseAvv - removedBaseAvv;
                    units = baseAvv - resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count();
                    isAvv = units > 0;
                    //bool isAvv = resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count() == 0;
                }
                var priceList = priceListPerDates.FirstOrDefault(x => x.DtStart <= dtCurrent && x.DtEnd >= dtCurrent);

                Response.unitAvailability.unitAvailabilityConfiguration.availability.Add(priceList != null ? (isAvv ? "Y" : "N") : "N");
                Response.unitAvailability.unitAvailabilityConfiguration.availableUnitCount.Add(units + "");
                string changeOver = "C";
                Response.unitAvailability.unitAvailabilityConfiguration.changeOver.Add(changeOver);
                Response.unitAvailability.unitAvailabilityConfiguration.minStay.Add(priceList != null ? priceList.MinStay + "" : currEstate.nights_min.objToInt32() + "");
                Response.unitAvailability.unitAvailabilityConfiguration.stayIncrement.Add("D");
                dtCurrent = dtCurrent.AddDays(1);
            }
            //}
            return Response;
        }
    }

    public static string Booking(long id)
    {
        using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        {
            var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
            if (ownerTbl == null)
            {
                ErrorLog.addLog("", "HA-Booking", "Owner for HomeAway was not found or not active");
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            long agentID = 0;
            var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
            if (agentTbl != null)
                agentID = agentTbl.id;
            if (agentID == 0)
            {
                ErrorLog.addLog("", "HA-Booking", "Agent for HomeAway was not found or not active");
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            ChnlHomeAwayClasses_V411.BookingResponse Response = new ChnlHomeAwayClasses_V411.BookingResponse();
            var reservationTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == id);
            if (reservationTbl == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == reservationTbl.pid_estate.objToInt32());
            if (currEstateTB == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == reservationTbl.pid_estate.objToInt32());
            if (currHAEstate == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            var listPay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == reservationTbl.id && x.is_complete == 1 && x.direction == 1 && x.pr_total.HasValue).ToList();
            decimal totalPayed = listPay.Count == 0 ? 0 : listPay.Sum(x => x.pr_total.Value);
            Response.documentVersion = "1.2";
            Response.reservation.numberOfAdults = reservationTbl.num_adult.objToInt32();
            Response.reservation.numberOfChildren = reservationTbl.num_child_over.objToInt32() + reservationTbl.num_child_min.objToInt32();
            Response.reservation.reservationDates.beginDate.ValueDate = reservationTbl.dtStart;
            Response.reservation.reservationDates.endDate.ValueDate = reservationTbl.dtEnd;
            #region OLB 1.2 update
            Response.reservation.reservationOriginationDate = reservationTbl.dtCreation != null ? reservationTbl.dtCreation.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : "";
            // Response.reservationOriginationDate = reservationTbl.dtCreation != null ? reservationTbl.dtCreation.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : "";

            #endregion
            Response.inquirer = new ChnlHomeAwayClasses_V411.Inquirer();
            AuthClientTBL clientTbl = dcAuth.AuthClientTBL.SingleOrDefault(x => x.id == reservationTbl.agentClientID);
            if (clientTbl != null)
            {
                Response.inquirer.emailAddress = clientTbl.contactEmail;
                Response.inquirer.name = clientTbl.nameFull;
                if (clientTbl.locAddress.splitStringToList(";").Count == 3)
                {
                    Response.inquirer.address.addressLine1 = clientTbl.locAddress.splitStringToList(";")[0];
                    Response.inquirer.address.additionalAddressLine1 = clientTbl.locAddress.splitStringToList(";")[1];
                    Response.inquirer.address.addressLine2 = clientTbl.locAddress.splitStringToList(";")[2];
                }
                else
                {
                    Response.inquirer.address.addressLine1 = clientTbl.locAddress;
                }
                Response.inquirer.address.addressLine3 = clientTbl.locCity;
                Response.inquirer.address.addressLine4 = clientTbl.locState;
                Response.inquirer.address.addressLine5 = clientTbl.locCountry;
                Response.inquirer.address.postalCode = clientTbl.locZipCode;
                if (clientTbl.contactPhoneMobile.splitStringToList("_").Count == 3)
                {
                    Response.inquirer.phoneCountryCode = clientTbl.contactPhoneMobile.splitStringToList("_")[0];
                    Response.inquirer.phoneExt = clientTbl.contactPhoneMobile.splitStringToList("_")[1];
                    Response.inquirer.phoneNumber = clientTbl.contactPhoneMobile.splitStringToList("_")[2];
                }
                else
                {
                    Response.inquirer.address.addressLine1 = clientTbl.locAddress;
                }
            }
            else
            {
                //check in normal client table if we do not find data in agent client table
                if (reservationTbl.cl_id != null && reservationTbl.cl_id > 0)
                {
                    USR_TBL_CLIENT userClientTBL = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == reservationTbl.cl_id);
                    if (userClientTBL != null)
                    {
                        if (!string.IsNullOrEmpty(userClientTBL.contact_email))
                            Response.inquirer.emailAddress = userClientTBL.contact_email;

                        if (!string.IsNullOrEmpty(userClientTBL.name_full))
                            Response.inquirer.name = userClientTBL.name_full;

                        if (!string.IsNullOrEmpty(userClientTBL.loc_address))
                            Response.inquirer.address.addressLine1 = userClientTBL.loc_address;

                        if (!string.IsNullOrEmpty(userClientTBL.loc_city))
                            Response.inquirer.address.addressLine3 = userClientTBL.loc_city;

                        if (!string.IsNullOrEmpty(userClientTBL.loc_state))
                            Response.inquirer.address.addressLine4 = userClientTBL.loc_state;

                        if (!string.IsNullOrEmpty(userClientTBL.loc_country))
                            Response.inquirer.address.addressLine5 = userClientTBL.loc_country;

                        if (!string.IsNullOrEmpty(userClientTBL.loc_zip_code))
                            Response.inquirer.address.postalCode = userClientTBL.loc_zip_code;

                    }
                }

            }

            //at last if data are empty , fetch it from reservation table
            if (string.IsNullOrEmpty(Response.inquirer.emailAddress) && !string.IsNullOrEmpty(reservationTbl.cl_email))
                Response.inquirer.emailAddress = reservationTbl.cl_email;

            if (string.IsNullOrEmpty(Response.inquirer.name) && !string.IsNullOrEmpty(reservationTbl.cl_name_full))
                Response.inquirer.name = reservationTbl.cl_name_full;

            if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
            {
                if (currEstateTB.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId)
                    Response.advertiserAssignedId = ownerTbl.ppb_advertiserAssignedId;
            }
            if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId))
            {
                if (currEstateTB.haAdvertiserId == ownerTbl.advertiserAssignedId)
                    Response.advertiserAssignedId = ownerTbl.advertiserAssignedId;
            }

            if (currHAEstate.is_slave == 1 && currHAEstate.pid_master_estate.objToInt32() > 0)
                Response.listingExternalId = currHAEstate.pid_master_estate.objToInt32() + "";
            else
                Response.listingExternalId = currHAEstate.id.objToInt32() + "";
            Response.unitExternalId = currHAEstate.id + "";
            Response.externalId = reservationTbl.id + "";
            Response.locale = contUtils.getLang_commonName(reservationTbl.cl_pid_lang.objToInt32()).Replace("-", "_");
            if (string.IsNullOrEmpty(Response.locale)) Response.locale = "en_GB";
            var order = new ChnlHomeAwayClasses_V411.Order();
            Response.orderList.Add(order);
            order.currency = "EUR";

            decimal taxPercentage = 0;
            int taxId = agentTbl.invTaxId.objToInt32();

            if (taxId == 0 || invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId) == null)
                taxPercentage = CommonUtilities.getSYS_SETTING("rnt_DefauultHATax").objToDecimal();
            else
            {
                dbInvCashTaxLK currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId);
                if (currTax != null)
                    taxPercentage = currTax.taxAmount.objToDecimal();
            }

            var orderItem = new ChnlHomeAwayClasses_V411.OrderItem();
            order.orderItemList.Add(orderItem);
            orderItem.feeType = "RENTAL";
            orderItem.description = "Rent";
            orderItem.name = "Rent";
            orderItem.preTaxAmount.ValueDecimal = getPriceWithoutTax((reservationTbl.pr_total.objToDecimal()), taxPercentage);
            orderItem.totalAmount.ValueDecimal = reservationTbl.pr_total.objToDecimal();
            orderItem.status = "ACCEPTED"; // PENDING, ACCEPTED, DECLINED, DECLINED_BY_SYSTEM, CANCELLED, EDITED           

            // TODO, finalise
            //Part Payment
            var paymentScheduleItem = new ChnlHomeAwayClasses_V411.PaymentScheduleItem();
            paymentScheduleItem.amount.ValueDecimal = reservationTbl.pr_part_forPayment.objToDecimal();
            paymentScheduleItem.dueDate.ValueDate = reservationTbl.dtCreation; //reservationTbl.dtStart.Value.AddDays(-30);
            if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
            order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

            //Balance Payment
            paymentScheduleItem = new ChnlHomeAwayClasses_V411.PaymentScheduleItem();
            paymentScheduleItem.amount.ValueDecimal = reservationTbl.pr_part_owner.objToDecimal();
            paymentScheduleItem.dueDate.ValueDate = reservationTbl.dtStart;
            if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
            order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

            if (currHAEstate.CancellationPolicyCharge.objToDecimal() > 0)
            {
                var cancellationPolicyItem = new ChnlHomeAwayClasses_V411.CancellationPolicyItem();
                order.reservationCancellationPolicy.cancellationPolicyItemList.Add(cancellationPolicyItem);
                cancellationPolicyItem.amount.ValueDecimal = currHAEstate.CancellationPolicyCharge.objToDecimal();
                if (currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.objToInt32() != 0)
                    cancellationPolicyItem.deadline.ValueDate = (reservationTbl.dtStart.Value.AddDays(-currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.Value));
                cancellationPolicyItem.penaltyType = currHAEstate.CancellationPolicyType;
            }
            order.reservationCancellationPolicy.description = App.HOST + "/" + CommonUtilities.getSYS_SETTING("ha411cancellation-policyPdf");
            //order.reservationCancellationPolicy.description = App.HOST + CurrentSource.getPagePath("19", "stp", App.LangID + "");
            //order.reservationCancellationPolicy.description = currHAEstate.CancellationPolicyDescription;
            Response.reservationPaymentStatus = "UNPAID"; // UNPAID, PARTIAL_PAID, OVERPAID, PAID, EXTERNAL_SOR
            if (totalPayed > reservationTbl.pr_total.objToDecimal())
                Response.reservationPaymentStatus = "OVERPAID";
            else if (totalPayed == reservationTbl.pr_total.objToDecimal())
                Response.reservationPaymentStatus = "PAID";
            else if (totalPayed > 0 && totalPayed < reservationTbl.pr_total.objToDecimal())
                Response.reservationPaymentStatus = "PARTIAL_PAID";

            Response.reservationStatus = "CONFIRMED"; // BUILDING, CANCELLED, CANCELLED_BY_OWNER, CANCELLED_BY_TRAVELER, CONFIRMED, DECLINED_BY_OWNER, DECLINED_BY_SYSTEM, STAY_IN_PROGRESS, UNCONFIRMED, UNCONFIRMED_BY_OWNER, UNCONFIRMED_BY_TRAVELER

            if (reservationTbl.state_pid.objToInt32() == 3)
            {
                Response.reservationStatus = "CANCELLED_BY_OWNER";
                if (reservationTbl.HAstateCancelledBy.objToInt32() == 1)
                {
                    Response.reservationStatus = "CANCELLED_BY_TRAVELER";

                }
            }

            Response.rentalAgreement.agreementText = App.HOST + "/" + CommonUtilities.getSYS_SETTING("ha411terms-and-conditionsPdf");
            // Response.rentalAgreement.agreementText = App.HOST + CurrentSource.getPagePath("2", "stp", App.LangID + "");
            return Response.GetXml();
        }
    }

    public static string BookingUpdate(long id)
    {
        using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
        {
            var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault();
            if (ownerTbl == null)
            {
                ErrorLog.addLog("", "HA-Booking", "Owner for HomeAway was not found or not active");
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            long agentID = 0;
            var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
            if (agentTbl != null)
                agentID = agentTbl.id;
            if (agentID == 0)
            {
                ErrorLog.addLog("", "HA-Booking", "Agent for HomeAway was not found or not active");
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            ChnlHomeAwayClasses_V411.BookingResponseUpdate Response = new ChnlHomeAwayClasses_V411.BookingResponseUpdate();
            var reservationTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == id);
            if (reservationTbl == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == reservationTbl.pid_estate.objToInt32());
            if (currEstateTB == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == reservationTbl.pid_estate.objToInt32());
            if (currHAEstate == null)
            {
                return ChnlHomeAwayClasses_V411.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            var listPay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == reservationTbl.id && x.is_complete == 1 && x.direction == 1 && x.pr_total.HasValue).ToList();
            decimal totalPayed = listPay.Count == 0 ? 0 : listPay.Sum(x => x.pr_total.Value);
            Response.documentVersion = "1.2";
            Response.reservation.numberOfAdults = reservationTbl.num_adult.objToInt32();
            Response.reservation.numberOfChildren = reservationTbl.num_child_over.objToInt32() + reservationTbl.num_child_min.objToInt32();
            Response.reservation.reservationDates.beginDate.ValueDate = reservationTbl.dtStart;
            Response.reservation.reservationDates.endDate.ValueDate = reservationTbl.dtEnd;
            #region OLB 1.2 update
            Response.reservation.reservationOriginationDate = reservationTbl.dtCreation != null ? reservationTbl.dtCreation.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : "";
            // Response.reservationOriginationDate = reservationTbl.dtCreation != null ? reservationTbl.dtCreation.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : "";

            #endregion
            Response.inquirer = new ChnlHomeAwayClasses_V411.InquirerUpdate();
            AuthClientTBL clientTbl = dcAuth.AuthClientTBL.SingleOrDefault(x => x.id == reservationTbl.agentClientID);
            if (clientTbl != null)
            {
                Response.inquirer.emailAddress = clientTbl.contactEmail;
                Response.inquirer.title = clientTbl.nameHonorific;
                Response.inquirer.firstName = clientTbl.nameFirst;
                Response.inquirer.lastName = clientTbl.nameLast;
                if (clientTbl.locAddress.splitStringToList(";").Count == 3)
                {
                    Response.inquirer.address.addressLine1 = clientTbl.locAddress.splitStringToList(";")[0];
                    Response.inquirer.address.additionalAddressLine1 = clientTbl.locAddress.splitStringToList(";")[1];
                    Response.inquirer.address.addressLine2 = clientTbl.locAddress.splitStringToList(";")[2];
                }
                else
                {
                    Response.inquirer.address.addressLine1 = clientTbl.locAddress;
                }
                Response.inquirer.address.addressLine3 = clientTbl.locCity;
                Response.inquirer.address.addressLine4 = clientTbl.locState;
                Response.inquirer.address.addressLine5 = clientTbl.locCountry;
                Response.inquirer.address.postalCode = clientTbl.locZipCode;
                if (clientTbl.contactPhoneMobile.splitStringToList("_").Count == 3)
                {
                    Response.inquirer.phoneCountryCode = clientTbl.contactPhoneMobile.splitStringToList("_")[0];
                    Response.inquirer.phoneExt = clientTbl.contactPhoneMobile.splitStringToList("_")[1];
                    Response.inquirer.phoneNumber = clientTbl.contactPhoneMobile.splitStringToList("_")[2];
                }
                else
                {
                    Response.inquirer.address.addressLine1 = clientTbl.locAddress;
                }
            }
            else
            {
                //check in normal client table if we do not find data in agent client table
                if (reservationTbl.cl_id != null && reservationTbl.cl_id > 0)
                {
                    USR_TBL_CLIENT userClientTBL = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == reservationTbl.cl_id);
                    if (userClientTBL != null)
                    {
                        if (!string.IsNullOrEmpty(userClientTBL.contact_email))
                            Response.inquirer.emailAddress = userClientTBL.contact_email;
                        if (!string.IsNullOrEmpty(userClientTBL.name_honorific))
                            Response.inquirer.title = userClientTBL.name_honorific;

                        if (!string.IsNullOrEmpty(userClientTBL.name_full))
                        {
                            string s = userClientTBL.name_full;
                            string[] result = s.Split(' ');
                            if (!string.IsNullOrEmpty(result[0]))
                                Response.inquirer.firstName = result[0];

                            if (!string.IsNullOrEmpty(result[1]))
                                Response.inquirer.lastName = result[1];
                        }

                        if (!string.IsNullOrEmpty(userClientTBL.loc_address))
                            Response.inquirer.address.addressLine1 = userClientTBL.loc_address;

                        if (!string.IsNullOrEmpty(userClientTBL.loc_city))
                            Response.inquirer.address.addressLine3 = userClientTBL.loc_city;

                        if (!string.IsNullOrEmpty(userClientTBL.loc_state))
                            Response.inquirer.address.addressLine4 = userClientTBL.loc_state;

                        if (!string.IsNullOrEmpty(userClientTBL.loc_country))
                            Response.inquirer.address.addressLine5 = userClientTBL.loc_country;

                        if (!string.IsNullOrEmpty(userClientTBL.loc_zip_code))
                            Response.inquirer.address.postalCode = userClientTBL.loc_zip_code;

                    }
                }

            }

            //at last if data are empty , fetch it from reservation table
            if (string.IsNullOrEmpty(Response.inquirer.emailAddress) && !string.IsNullOrEmpty(reservationTbl.cl_email))
                Response.inquirer.emailAddress = reservationTbl.cl_email;
            if (!string.IsNullOrEmpty(reservationTbl.cl_name_full))
            {
                string s1 = reservationTbl.cl_name_full;
                string[] result1 = s1.Split(' ');
                if (!string.IsNullOrEmpty(result1[0]))
                {
                    if (string.IsNullOrEmpty(Response.inquirer.firstName) && !string.IsNullOrEmpty(reservationTbl.cl_name_full))
                        Response.inquirer.firstName = result1[0];
                }
                if (!string.IsNullOrEmpty(result1[1]))
                {
                    if (string.IsNullOrEmpty(Response.inquirer.lastName) && !string.IsNullOrEmpty(reservationTbl.cl_name_full))
                        Response.inquirer.lastName = result1[1];
                }
            }
            if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
            {
                if (currEstateTB.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId)
                    Response.advertiserAssignedId = ownerTbl.ppb_advertiserAssignedId;
            }
            if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId))
            {
                if (currEstateTB.haAdvertiserId == ownerTbl.advertiserAssignedId)
                    Response.advertiserAssignedId = ownerTbl.advertiserAssignedId;
            }

            if (currHAEstate.is_slave == 1 && currHAEstate.pid_master_estate.objToInt32() > 0)
                Response.listingExternalId = currHAEstate.pid_master_estate.objToInt32() + "";
            else
                Response.listingExternalId = currHAEstate.id.objToInt32() + "";
            Response.unitExternalId = currHAEstate.id + "";
            Response.externalId = reservationTbl.id + "";
            Response.locale = contUtils.getLang_commonName(reservationTbl.cl_pid_lang.objToInt32()).Replace("-", "_");
            if (string.IsNullOrEmpty(Response.locale)) Response.locale = "en_GB";
            var order = new ChnlHomeAwayClasses_V411.OrderUpdate();
            Response.orderList.Add(order);
            order.currency = "EUR";

            decimal taxPercentage = 0;
            int taxId = agentTbl.invTaxId.objToInt32();

            if (taxId == 0 || invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId) == null)
                taxPercentage = CommonUtilities.getSYS_SETTING("rnt_DefauultHATax").objToDecimal();
            else
            {
                dbInvCashTaxLK currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId);
                if (currTax != null)
                    taxPercentage = currTax.taxAmount.objToDecimal();
            }

            var orderItem = new ChnlHomeAwayClasses_V411.OrderItem();
            order.orderItemList.Add(orderItem);
            orderItem.feeType = "RENTAL";
            orderItem.description = "Rent";
            orderItem.name = "Rent";
            orderItem.preTaxAmount.ValueDecimal = getPriceWithoutTax((reservationTbl.pr_total.objToDecimal()), taxPercentage);
            orderItem.totalAmount.ValueDecimal = reservationTbl.pr_total.objToDecimal();
            orderItem.status = "ACCEPTED"; // PENDING, ACCEPTED, DECLINED, DECLINED_BY_SYSTEM, CANCELLED, EDITED           

            // TODO, finalise
            //Part Payment
            var paymentScheduleItem = new ChnlHomeAwayClasses_V411.PaymentScheduleItem();
            paymentScheduleItem.amount.ValueDecimal = reservationTbl.pr_part_forPayment.objToDecimal();
            paymentScheduleItem.dueDate.ValueDate = reservationTbl.dtCreation; //reservationTbl.dtStart.Value.AddDays(-30);
            if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
            order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

            //Balance Payment
            paymentScheduleItem = new ChnlHomeAwayClasses_V411.PaymentScheduleItem();
            paymentScheduleItem.amount.ValueDecimal = reservationTbl.pr_part_owner.objToDecimal();
            paymentScheduleItem.dueDate.ValueDate = reservationTbl.dtStart;
            if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
            order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

            if (currHAEstate.CancellationPolicyCharge.objToDecimal() > 0)
            {
                var cancellationPolicyItem = new ChnlHomeAwayClasses_V411.CancellationPolicyItem();
                // order.reservationCancellationPolicy.cancellationPolicyItemList.Add(cancellationPolicyItem);
                cancellationPolicyItem.amount.ValueDecimal = currHAEstate.CancellationPolicyCharge.objToDecimal();
                if (currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.objToInt32() != 0)
                    cancellationPolicyItem.deadline.ValueDate = (reservationTbl.dtStart.Value.AddDays(-currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.Value));
                cancellationPolicyItem.penaltyType = currHAEstate.CancellationPolicyType;
            }
            // order.reservationCancellationPolicy.description = App.HOST + "/" + CommonUtilities.getSYS_SETTING("ha411cancellation-policyPdf");
            //order.reservationCancellationPolicy.description = App.HOST + CurrentSource.getPagePath("19", "stp", App.LangID + "");
            //order.reservationCancellationPolicy.description = currHAEstate.CancellationPolicyDescription;
            Response.reservationPaymentStatus = "UNPAID"; // UNPAID, PARTIAL_PAID, OVERPAID, PAID, EXTERNAL_SOR
            if (totalPayed > reservationTbl.pr_total.objToDecimal())
                Response.reservationPaymentStatus = "OVERPAID";
            else if (totalPayed == reservationTbl.pr_total.objToDecimal())
                Response.reservationPaymentStatus = "PAID";
            else if (totalPayed > 0 && totalPayed < reservationTbl.pr_total.objToDecimal())
                Response.reservationPaymentStatus = "PARTIAL_PAID";

            Response.reservationStatus = "CONFIRMED"; // BUILDING, CANCELLED, CANCELLED_BY_OWNER, CANCELLED_BY_TRAVELER, CONFIRMED, DECLINED_BY_OWNER, DECLINED_BY_SYSTEM, STAY_IN_PROGRESS, UNCONFIRMED, UNCONFIRMED_BY_OWNER, UNCONFIRMED_BY_TRAVELER

            if (reservationTbl.state_pid.objToInt32() == 3)
            {
                Response.reservationStatus = "CANCELLED_BY_OWNER";
                if (reservationTbl.HAstateCancelledBy.objToInt32() == 1)
                {
                    Response.reservationStatus = "CANCELLED_BY_TRAVELER";

                }
            }

            Response.rentalAgreement.agreementText = App.HOST + "/" + CommonUtilities.getSYS_SETTING("ha411terms-and-conditionsPdf");
            // Response.rentalAgreement.agreementText = App.HOST + CurrentSource.getPagePath("2", "stp", App.LangID + "");
            return Response.GetXml();
        }
    }
    public static string QuoteRequest(string xml)
    {
        if (xml == "") return "";

        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("quoteResponse");

        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "GetQuoteRequest", "Owner for HomeAway was not found or not active");
                    return "";
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    ErrorLog.addLog("", "GetQuoteRequest", "Agent for HomeAway was not found or not active");
                    return "";
                }

                ChnlHomeAwayClasses_V411.QuoteRequest Request = new ChnlHomeAwayClasses_V411.QuoteRequest(xml);
                Request.documentVersion = "1.2";
                //for sending resonse
                RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currEstateTB == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }
                RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currHAEstate == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }

                DateTime dtStart = Request.reservation.reservationDates.beginDate.ValueDate.Value;
                DateTime dtEnd = Request.reservation.reservationDates.endDate.ValueDate.Value;

                DateTime dtMax = DateTime.Now.AddYears(2).Date.AddDays(1);
                //if (dtEnd >= dtMax)
                //{
                //    errorList.AddError("END_DAY_MISMATCH");
                //    return errorList.GetXml();
                //}

                var lastDay = dc.RntSeasonDatesTBL.FirstOrDefault(x => x.dtEnd >= dtEnd && x.dtStart <= dtEnd && x.pidSeasonGroup == currEstateTB.pidSeasonGroup);
                if (lastDay == null)
                {
                    errorList.AddError("END_DAY_MISMATCH");
                    return errorList.GetXml();
                }

                //if (!(currEstateTB.num_pets_max != null && currEstateTB.num_pets_max > 0))
                //{
                if (Request.reservation.numberOfPets.objToInt32() > 0)
                {
                    //if ((currEstateTB.num_pets_max != null && currEstateTB.num_pets_max == 0))
                    //{
                    //    errorList.AddError("PETS_NOT_ALLOWED");
                    //    return errorList.GetXml();
                    //}
                    var currFeature = dcChnl.RntChnlHomeAwayEstateFeaturesRL.SingleOrDefault(x => x.pidEstate == currHAEstate.id && x.code == "SUITABILITY_PETS_ASK");
                    if (currFeature == null || currFeature.count < Request.reservation.numberOfPets.objToInt32())
                    {
                        var currFeature_PETS_CONSIDERED = dcChnl.RntChnlHomeAwayEstateFeaturesRL.SingleOrDefault(x => x.pidEstate == currHAEstate.id && x.code == "SUITABILITY_PETS_CONSIDERED");
                        if (currFeature_PETS_CONSIDERED == null || currFeature_PETS_CONSIDERED.count < Request.reservation.numberOfPets.objToInt32())
                        {
                            errorList.AddError("PETS_NOT_ALLOWED");
                            return errorList.GetXml();
                        }
                    }
                }
                //}

                if (currHAEstate.num_max_sleep.objToInt32() == 0)
                {
                    currHAEstate.num_max_sleep = currEstateTB.num_persons_max;
                }
                if (currEstateTB.nights_max.objToInt32() > 0 && currEstateTB.nights_max < (dtEnd - dtStart).TotalDays.objToInt32())
                {
                    errorList.AddError("EXCEEDS_MAX_STAY");
                    return errorList.GetXml();
                }

                if (currHAEstate.num_max_sleep > 0 && currHAEstate.num_max_sleep < (Request.reservation.numberOfAdults.objToInt32() + Request.reservation.numberOfChildren.objToInt32()))
                {
                    errorList.AddError("EXCEEDS_MAX_OCCUPANCY");
                    return errorList.GetXml();
                }
                //bool _isAvailable = dcOld.RNT_TBL_RESERVATION.Where(y => y.pid_estate == currEstateTB.id //
                //                                                                    && y.state_pid != 3 //
                //                                                                    && y.dtStart.HasValue //
                //                                                                    && y.dtEnd.HasValue //
                //                                                                    && ((y.dtStart <= dtStart && y.dtEnd >= dtEnd) //
                //                                                                        || (y.dtStart >= dtStart && y.dtStart < dtEnd) //
                //                                                                        || (y.dtEnd > dtStart && y.dtEnd <= dtEnd))).Count() == 0;



                bool _isAvailable = ChnlHomeAwayUtils_V411.CheckAvailability(currEstateTB, dtStart, dtEnd, agentTbl.id);

                //DateTime dt_start = dtEnd.AddDays(7);
                //DateTime dt_end = DateTime.Now.Date;
                //int total_days = 0;
                //var check_first_reservation = dcOld.RNT_TBL_RESERVATION.FirstOrDefault(x => (x.dtEnd == dtEnd));
                //var check_next_reservation = dcOld.RNT_TBL_RESERVATION.FirstOrDefault(x => (x.dtStart == check_first_reservation.dtEnd));

                //if (check_next_reservation != null && check_first_reservation != null)
                //    total_days = (check_next_reservation.dtStart.Value - check_first_reservation.dtEnd.Value).TotalDays.objToInt32();

                //int HA_Gap_days = CommonUtilities.getSYS_SETTING("ha_gap_days").ToInt32();
                //if (HA_Gap_days == total_days)
                //    _isAvailable = false;
                if (!_isAvailable)
                {
                    errorList.AddError("PROPERTY_NOT_AVAILABLE");
                    return errorList.GetXml();
                }

                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                outPrice.dtStart = dtStart;
                outPrice.dtEnd = dtEnd;
                outPrice.numPersCount = Request.reservation.numberOfAdults.objToInt32() + Request.reservation.numberOfChildren.objToInt32();
                outPrice.numPers_adult = Request.reservation.numberOfAdults.objToInt32();
                outPrice.numPers_childOver = Request.reservation.numberOfChildren.objToInt32();
                outPrice.agentID = agentID;
                outPrice.fillAgentDetails(agentTbl);
                DateTime checkDate = DateTime.Now;
                DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                var seasonDates = new List<RntSeasonDatesTBL>();
                seasonDates = dc.RntSeasonDatesTBL.Where(x => x.pidSeasonGroup == currEstateTB.pidSeasonGroup).OrderByDescending(x => x.dtStart).ToList();
                if (seasonDates != null && seasonDates.Count > 0)
                {
                    checkDateEnd = seasonDates.FirstOrDefault().dtEnd;
                }
                var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTbl.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                outPrice.agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;

                #region Made change for using values of Acconto and balance configured in agent detail
                outPrice.part_percentage = currEstateTB.pr_percentage.objToDecimal();
                //outPrice.part_percentage = 0;
                #endregion

                //bool sendMaxPrice = true;
                decimal price = rntUtils.rntEstate_getPrice(0, Request.unitExternalId.objToInt32(), ref outPrice, ChnlHomeAwayProps_V411.IdAdMedia);

                if (price == 0)
                {
                    if (outPrice.outError == 1)
                    {
                        errorList.AddError("MIN_STAY_NOT_MET");
                        return errorList.GetXml();
                    }
                    else if (outPrice.outError == 5)
                    {
                        errorList.AddError("START_DAY_MISMATCH");
                        return errorList.GetXml();
                    }
                    else if (outPrice.outError == 7)
                    {
                        errorList.AddError("END_DAY_MISMATCH");
                        return errorList.GetXml();
                    }
                    else
                    {
                        errorList.AddError("PROPERTY_NOT_AVAILABLE_OTHER");
                        return errorList.GetXml();
                    }
                }
                if (errorList.errorList.Count > 0)
                {
                    return errorList.GetXml();
                }

                decimal taxPercentage = 0;
                int taxId = agentTbl.invTaxId.objToInt32();

                if (taxId == 0 || invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId) == null)
                    taxPercentage = CommonUtilities.getSYS_SETTING("rnt_DefauultHATax").objToDecimal();
                else
                {
                    dbInvCashTaxLK currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId);
                    if (currTax != null)
                        taxPercentage = currTax.taxAmount.objToDecimal();
                }

                //if (taxId == 0 || invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId) == null) taxId = 1;

                //dbInvCashTaxLK currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId);
                //if (currTax != null)
                //    taxPercentage = currTax.taxAmount.objToDecimal();

                ChnlHomeAwayClasses_V411.QuoteResponse Response = new ChnlHomeAwayClasses_V411.QuoteResponse();
                Response.documentVersion = "1.2";
                Response.locale = Request.inquirer.locale;
                if (string.IsNullOrEmpty(Response.locale)) Response.locale = "en_GB";
                var order = new ChnlHomeAwayClasses_V411.Order();
                Response.orderList.Add(order);
                order.currency = "EUR";

                var orderItem = new ChnlHomeAwayClasses_V411.OrderItem();
                order.orderItemList.Add(orderItem);
                orderItem.feeType = "RENTAL";
                orderItem.description = "Rent";
                orderItem.name = "Rent";
                orderItem.preTaxAmount.ValueDecimal = getPriceWithoutTax((price), taxPercentage);
                orderItem.totalAmount.ValueDecimal = price;

                // TODO, finalise
                var paymentScheduleItem = new ChnlHomeAwayClasses_V411.PaymentScheduleItem();
                paymentScheduleItem.amount.ValueDecimal = price;
                paymentScheduleItem.dueDate.ValueDate = dtStart.AddDays(-30);
                if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
                order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);
                if (currHAEstate.CancellationPolicyCharge.objToDecimal() > 0)
                {
                    var cancellationPolicyItem = new ChnlHomeAwayClasses_V411.CancellationPolicyItem();
                    order.reservationCancellationPolicy.cancellationPolicyItemList.Add(cancellationPolicyItem);
                    cancellationPolicyItem.amount.ValueDecimal = currHAEstate.CancellationPolicyCharge.objToDecimal() * 1;
                    if (currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.objToInt32() != 0)
                        cancellationPolicyItem.deadline.ValueDate = (dtStart.AddDays(-currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.Value));
                    cancellationPolicyItem.penaltyType = currHAEstate.CancellationPolicyType;
                }
                order.reservationCancellationPolicy.description = App.HOST + "/" + CommonUtilities.getSYS_SETTING("ha411cancellation-policyPdf");

                //set stay fees for city tax
                if (!string.IsNullOrEmpty(contUtils.getLabel("lblHAStayFess")) && contUtils.getLabel("lblHAStayFess") != "lblHAStayFess")
                {
                    var stayFessList = new List<ChnlHomeAwayClasses_V411.StayFees>();
                    var stayFess = new ChnlHomeAwayClasses_V411.StayFees();
                    stayFess.description = contUtils.getLabel("lblHAStayFess");
                    stayFessList.Add(stayFess);
                    order.stayFees = stayFessList;
                }

                Response.rentalAgreement.agreementText = App.HOST + "/" + CommonUtilities.getSYS_SETTING("ha411terms-and-conditionsPdf");
                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "HA_Quote", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string BookingRequest(string xml)
    {
        if (xml == "") return "";

        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorList errorList = new ChnlHomeAwayClasses_V411.ErrorList("bookingResponse");

        using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "GetBookingRequest", "Owner for HomeAway was not found or not active");
                    return "";
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    ErrorLog.addLog("", "GetBookingRequest", "Agent for HomeAway was not found or not active");
                    return "";
                }

                ChnlHomeAwayClasses_V411.BookingRequest Request = new ChnlHomeAwayClasses_V411.BookingRequest(xml);
                Request.documentVersion = "1.2";
                var ccData = "";
                ccData += "Holder: " + Request.paymentForm.paymentCard.nameOnCard;
                ccData += "\r\n Holder Billing AdditionalAddressLine1: " + Request.paymentForm.paymentCard.billingAddress.additionalAddressLine1;
                ccData += "\r\n Holder Billing AddressLine1: " + Request.paymentForm.paymentCard.billingAddress.addressLine1;
                ccData += "\r\n Holder Billing AddressLine2: " + Request.paymentForm.paymentCard.billingAddress.addressLine2;
                ccData += "\r\n Holder Billing AddressLine3: " + Request.paymentForm.paymentCard.billingAddress.addressLine3;
                ccData += "\r\n Holder Billing AddressLine4: " + Request.paymentForm.paymentCard.billingAddress.addressLine4;
                ccData += "\r\n Holder Billing AddressLine5: " + Request.paymentForm.paymentCard.billingAddress.addressLine5;
                ccData += "\r\n Holder Billing Country: " + Request.paymentForm.paymentCard.billingAddress.country;
                ccData += "\r\n Holder Billing postalCode: " + Request.paymentForm.paymentCard.billingAddress.postalCode;

                ccData += "\r\n ";
                ccData += "\r\n Masked Number: " + Request.paymentForm.paymentCard.maskedNumber;
                ccData += "\r\n Number: " + Request.paymentForm.paymentCard.number;
                ccData += "\r\n NumberToken: " + Request.paymentForm.paymentCard.numberToken;
                ccData += "\r\n paymentFormType: " + Request.paymentForm.paymentCard.paymentFormType;
                ccData += "\r\n paymentFormTypeCard: " + Request.paymentForm.paymentCard.paymentCardDescriptor.paymentFormType;
                ccData += "\r\n Card Code: " + Request.paymentForm.paymentCard.paymentCardDescriptor.cardCode;
                ccData += "\r\n Type: " + Request.paymentForm.paymentCard.paymentCardDescriptor.cardType;
                ccData += "\r\n CVC: " + Request.paymentForm.paymentCard.cvv;
                ccData += "\r\n Expire: " + Request.paymentForm.paymentCard.expiration;
                RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currEstateTB == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }
                RntChnlHomeAwayEstateTB currHAEstate = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currHAEstate == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }

                if (Request.reservation.numberOfPets.objToInt32() > 0)
                {
                    var currFeature = dcChnl.RntChnlHomeAwayEstateFeaturesRL.SingleOrDefault(x => x.pidEstate == currHAEstate.id && x.code == "SUITABILITY_PETS_ASK");
                    if (currFeature == null || currFeature.count < Request.reservation.numberOfPets.objToInt32())
                    {
                        var currFeature_PETS_CONSIDERED = dcChnl.RntChnlHomeAwayEstateFeaturesRL.SingleOrDefault(x => x.pidEstate == currHAEstate.id && x.code == "SUITABILITY_PETS_CONSIDERED");
                        if (currFeature_PETS_CONSIDERED == null || currFeature_PETS_CONSIDERED.count < Request.reservation.numberOfPets.objToInt32())
                        {
                            errorList.AddError("PETS_NOT_ALLOWED");
                        }
                    }
                }

                DateTime dtStart = Request.reservation.reservationDates.beginDate.ValueDate.Value;
                DateTime dtEnd = Request.reservation.reservationDates.endDate.ValueDate.Value;

                if (currHAEstate.num_max_sleep.objToInt32() == 0)
                {
                    currHAEstate.num_max_sleep = currEstateTB.num_persons_max;
                }
                if (currEstateTB.nights_max.objToInt32() > 0 && currEstateTB.nights_max < (dtEnd - dtStart).TotalDays.objToInt32())
                {
                    errorList.AddError("EXCEEDS_MAX_STAY");
                }

                if (currHAEstate.num_max_sleep > 0 && currHAEstate.num_max_sleep < (Request.reservation.numberOfAdults.objToInt32() + Request.reservation.numberOfChildren.objToInt32()))
                {
                    errorList.AddError("EXCEEDS_MAX_OCCUPANCY");
                }
                bool _isAvailable = dcOld.RNT_TBL_RESERVATION.Where(y => y.pid_estate == currEstateTB.id //
                                                                                    && y.state_pid != 3 //
                                                                                    && y.dtStart.HasValue //
                                                                                    && y.dtEnd.HasValue //
                                                                                    && ((y.dtStart <= dtStart && y.dtEnd >= dtEnd) //
                                                                                        || (y.dtStart >= dtStart && y.dtStart < dtEnd) //
                                                                                        || (y.dtEnd > dtStart && y.dtEnd <= dtEnd))).Count() == 0;




                if (!_isAvailable)
                {
                    errorList.AddError("PROPERTY_NOT_AVAILABLE");
                }
                if (errorList.errorList.Count > 0)
                {
                    return errorList.GetXml();
                }
                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                outPrice.dtStart = dtStart;
                outPrice.dtEnd = dtEnd;
                outPrice.numPersCount = Request.reservation.numberOfAdults.objToInt32() + Request.reservation.numberOfChildren.objToInt32();
                outPrice.numPers_adult = Request.reservation.numberOfAdults.objToInt32();
                outPrice.numPers_childOver = Request.reservation.numberOfChildren.objToInt32();

                outPrice.fillAgentDetails(agentTbl);
                DateTime checkDate = DateTime.Now;
                DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTbl.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                outPrice.agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                outPrice.part_percentage = currEstateTB.pr_percentage.objToDecimal();
                decimal prTotal = rntUtils.rntEstate_getPrice(0, Request.unitExternalId.objToInt32(), ref outPrice, ChnlHomeAwayProps_V411.IdAdMedia);
                decimal prTotalNoTax = prTotal;
                if (prTotal == 0)
                {
                    if (outPrice.outError == 1)
                        errorList.AddError("MIN_STAY_NOT_MET");
                    else if (outPrice.outError == 5)
                        errorList.AddError("START_DAY_MISMATCH");
                    else
                        errorList.AddError("PROPERTY_NOT_AVAILABLE");
                }
                if (errorList.errorList.Count > 0)
                {
                    return errorList.GetXml();
                }

                decimal taxPercentage = 0;
                int taxId = agentTbl.invTaxId.objToInt32();

                if (taxId == 0 || invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId) == null)
                    taxPercentage = CommonUtilities.getSYS_SETTING("rnt_DefauultHATax").objToDecimal();
                else
                {
                    dbInvCashTaxLK currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId);
                    if (currTax != null)
                        taxPercentage = currTax.taxAmount.objToDecimal();
                }

                var orderItem = Request.orderItemList.FirstOrDefault();
                if (orderItem != null)
                {
                    RNT_TBL_RESERVATION newRes = rntUtils.newReservation();
                    newRes.pid_creator = 1;
                    newRes.state_pid = 4;
                    newRes.state_body = "";
                    newRes.state_date = DateTime.Now;
                    newRes.state_pid_user = 1;
                    newRes.state_subject = "API BOOKING";

                    //ha fields
                    newRes.IdAdMedia = ChnlHomeAwayProps_V411.IdAdMedia;
                    newRes.IdLink = Request.travelerSource;

                    newRes.pid_estate = Request.unitExternalId.objToInt32();
                    newRes.num_adult = Request.reservation.numberOfAdults.objToInt32();
                    newRes.num_child_over = Request.reservation.numberOfChildren.objToInt32();
                    newRes.dtStart = dtStart;
                    newRes.dtEnd = dtEnd;

                    rntUtils.rntReservation_setDefaults(ref newRes);
                    dcOld.RNT_TBL_RESERVATION.InsertOnSubmit(newRes);
                    dcOld.SubmitChanges();
                    newRes.code = newRes.id.ToString().fillString("0", 7, false);
                    var chnlBooking = dcChnl.RntChnlHomeAwayBookingTBL.SingleOrDefault(x => x.id == newRes.id);
                    if (chnlBooking == null)
                    {
                        chnlBooking = new RntChnlHomeAwayBookingTBL() { id = newRes.id };
                        dcChnl.RntChnlHomeAwayBookingTBL.InsertOnSubmit(chnlBooking);
                    }
                    chnlBooking.inquiryId = Request.inquiryId;
                    chnlBooking.trackingUuid = Request.trackingUuid;
                    chnlBooking.numberOfPets = Request.reservation.numberOfPets.objToInt32();
                    chnlBooking.message = Request.message;
                    chnlBooking.listingChannel = Request.listingChannel;
                    chnlBooking.masterListingChannel = Request.masterListingChannel;
                    chnlBooking.commission = Request.commission;
                    chnlBooking.locale = Request.inquirer.locale;
                    dcChnl.SubmitChanges();

                    //make the file for credit card information
                    string filePath = Path.Combine(App.SRP, "admin/dati_cc");
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    filePath = Path.Combine(filePath, newRes.unique_id + ".txt");
                    if (!File.Exists(filePath) || ccData != "")
                    {
                        StreamWriter ccWriter = new StreamWriter(filePath, true);
                        ccWriter.WriteLine(RijndaelSimple_2.Encrypt(ccData)); // Write the file.
                        ccWriter.Flush();
                        ccWriter.Close(); // Close the instance of StreamWriter.
                        ccWriter.Dispose(); // Dispose from memory.
                    }

                    newRes.agentID = agentID;
                    newRes.pid_estate = currEstateTB.id;
                    newRes.pidEstateCity = currEstateTB.pid_city;
                    //newRes.bcom_status = status;
                    dcOld.SubmitChanges();

                    //fill reservation data

                    newRes.is_dtStartTimeChanged = 0;
                    newRes.is_dtEndTimeChanged = 0;

                    //newRes.requestFullPayAccepted = 1;

                    newRes.password = CommonUtilities.CreatePassword(8, false, true, false);
                    newRes.cl_email = Request.inquirer.emailAddress;
                    //newRes.cl_name_full = Request.inquirer.name;
                    newRes.cl_name_full = string.IsNullOrEmpty(Request.inquirer.name) ? Request.inquirer.firstName + " " + Request.inquirer.lastName : Request.inquirer.name;
                    //newRes.cl_
                    // newRes.cl_name_honorific = "";
                    newRes.cl_loc_country = Request.inquirer.address.country;
                    newRes.cl_pid_discount = -1;
                    newRes.cl_pid_lang = App.DefLangID;
                    newRes.cl_isCompleted = 0;

                    try
                    {
                        #region total price save of ha only
                        decimal pr_total = 0;
                        var orderItemList = Request.orderItemList.ToList();
                        if (orderItemList.Count > 0)
                        {
                            foreach (ChnlHomeAwayClasses_V411.OrderItem item in orderItemList)
                            {
                                pr_total = pr_total + (item.totalAmount.ValueDecimal.objToDecimal());
                            }
                        }

                        newRes.pr_total = pr_total;
                        newRes.prTotalRate = pr_total;
                        newRes.prTotalCommission = currEstateTB.pr_percentage.objToInt32() * pr_total / 100;
                        newRes.prTotalOwner = pr_total - newRes.prTotalCommission;
                        newRes.pr_reservation = pr_total;
                        newRes.pr_part_forPayment = newRes.pr_total; 
                        newRes.pr_part_payment_total = 0;
                        newRes.pr_part_owner = pr_total;
                        newRes.pr_part_modified = 1;
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.addLog("", "ha booking request prices", ex.ToString());
                        outPrice.CopyTo(ref newRes);
                    }

                    newRes.pr_paymentType = "cc";

                    //for client
                    var _client = dcAuth.AuthClientTBL.FirstOrDefault(x => x.id == newRes.agentClientID.objToInt64());
                    if (_client == null)
                    {
                        _client = dcAuth.AuthClientTBL.FirstOrDefault(x => x.pidAgent == agentID && x.contactEmail == Request.inquirer.emailAddress);
                        if (_client == null || Request.inquirer.emailAddress == "")
                        {
                            _client = new AuthClientTBL();
                            _client.pidAgent = agentID;
                            _client.uid = Guid.NewGuid();
                            _client.createdDate = DateTime.Now;
                            _client.createdUserID = 1;
                            _client.createdUserNameFull = "System";
                            _client.contactEmail = Request.inquirer.emailAddress;
                            dcAuth.AuthClientTBL.InsertOnSubmit(_client);

                            dcAuth.SubmitChanges();
                            _client.code = _client.id.ToString().fillString("0", 6, false);
                            dcAuth.SubmitChanges();

                            _client.contactEmail = Request.inquirer.emailAddress;
                            if (string.IsNullOrEmpty(Request.inquirer.name))
                            {
                                _client.nameFirst = Request.inquirer.firstName;
                                _client.nameLast = Request.inquirer.lastName;
                                _client.nameFull = Request.inquirer.firstName + " " + Request.inquirer.lastName;
                            }
                            else
                            {
                                _client.nameFirst = Request.inquirer.name;
                                _client.nameLast = "";
                                _client.nameFull = Request.inquirer.name;
                            }


                            _client.locAddress = Request.inquirer.address.addressLine1 + "; " + Request.inquirer.address.additionalAddressLine1 + "; " + Request.inquirer.address.addressLine2;
                            _client.locCity = Request.inquirer.address.addressLine3;
                            _client.locState = Request.inquirer.address.addressLine4;
                            _client.locCountry = Request.inquirer.address.addressLine5;
                            _client.locZipCode = Request.inquirer.address.postalCode;
                            _client.contactPhoneMobile = Request.inquirer.phoneCountryCode + "_" + Request.inquirer.phoneExt + "_" + Request.inquirer.phoneNumber;
                            _client.isActive = 1;
                            dcAuth.SubmitChanges();
                        }
                        newRes.agentClientID = _client.id;
                    }

                    dcOld.SubmitChanges();

                    #region Insert in RNT_TBL_EXTRA_RESERVATION
                    //List<rntExts.OptionExtra> lstRequiredExtra = outPrice.lstExtra.Where(x => x.isRequired == 1).ToList();
                    //if (lstRequiredExtra != null && lstRequiredExtra.Count > 0)
                    //{
                    //    foreach (rntExts.OptionExtra objReq in lstRequiredExtra)
                    //    {
                    //        RNT_TBL_EXTRA_RESERVATION currExtra = new RNT_TBL_EXTRA_RESERVATION();
                    //        currExtra.pidReservation = newRes.id;
                    //        currExtra.pidExtra = objReq.pidExtra;
                    //        currExtra.price = objReq.price.objToDecimal();
                    //        dcOld.RNT_TBL_EXTRA_RESERVATION.InsertOnSubmit(currExtra);
                    //        dcOld.SubmitChanges();
                    //    }
                    //}
                    #endregion

                    rntUtils.rntReservation_onChange(newRes);
                    rntUtils.reservation_checkPartPayment(newRes, true);

                    if ((CommonUtilities.getSYS_SETTING("rntHA_sendEmails") == "true" || CommonUtilities.getSYS_SETTING("rntHA_sendEmails").ToInt32() == 1))
                    {
                        rntUtils.rntReservation_mailNewCreation(newRes, true, true, true, true, 1); // send mails
                        //rntUtils.rntReservation_mailNewCreation(newRes, false, true, true, true, 1); // send mails
                        //rntUtils.MailReservationCreated(newRes, true, true, true, true, 1); // send mails
                    }
                    return Booking(newRes.id);
                }
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "HA_BookingRequest", ex.ToString());
            }
        return errorList.GetXml();
    }
    public static string newAvailabilityRequest(string json)
    {
        var task = System.Threading.Tasks.Task.Run(() =>
        {
            return AvailabilityRequest(json);
        });

        bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(1000));

        if (isCompletedSuccessfully)
        {
            return task.Result;
        }
        else
        {
            return "";
            //throw new TimeoutException("The function has taken longer than the maximum time allowed.");
        }
    }
    //Payal
    public static string AvailabilityRequest(string json)
    {
        if (json == "") return "";


        long agentID = 0;
        ChnlHomeAwayClasses_V411.ErrorJsonReponseAvailabilityRequest errorList = new ChnlHomeAwayClasses_V411.ErrorJsonReponseAvailabilityRequest();

        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            try
            {
                var ownerTbl = dcChnl.RntChnlHomeAwayOwnerTBL.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "availabilityRequest", "Owner for HomeAway was not found or not active");
                    return "";
                }
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    ErrorLog.addLog("", "availabilityRequest", "Agent for HomeAway was not found or not active");
                    return "";
                }

                string advertiserAssignedId = "";
                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    advertiserAssignedId = ownerTbl.ppb_advertiserAssignedId;
                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    advertiserAssignedId = ownerTbl.advertiserAssignedId;
                }


                ChnlHomeAwayClasses_V411.AvailabilityRequest Request = JsonConvert.DeserializeObject<ChnlHomeAwayClasses_V411.AvailabilityRequest>(json);

                if (Request.advertiserExternalId != advertiserAssignedId)
                {
                    ErrorLog.addLog("", "availabilityRequest", "AdvertiserExternalId not matching.");
                    return "";
                }
                int children = 0;
                if (Request.children != null)
                {
                    children = Request.children.objToInt32();
                }
                int pets = 0;
                if (Request.pets != null)
                {
                    pets = Request.pets.objToInt32();
                }
                int adults = 0;
                if (Request.adults != null)
                {
                    adults = Request.adults.objToInt32();
                }



                ChnlHomeAwayClasses_V411.Date arrivalDate = new ChnlHomeAwayClasses_V411.Date();
                arrivalDate.Value = Request.dateRange.arrivalDate;
                ChnlHomeAwayClasses_V411.Date departureDate = new ChnlHomeAwayClasses_V411.Date();
                departureDate.Value = Request.dateRange.departureDate;


                DateTime dtStart = arrivalDate.ValueDate.Value;
                DateTime dtEnd = departureDate.ValueDate.Value;

                DateTime dtMax = DateTime.Now.AddYears(2).Date.AddDays(1);
                //if (dtEnd >= dtMax)
                //{
                //    errorList.AddError("END_DAY_MISMATCH");
                //    return errorList.GetXml();
                //}


                ChnlHomeAwayClasses_V411.AvailabilityResponse Response = new ChnlHomeAwayClasses_V411.AvailabilityResponse();

                foreach (ChnlHomeAwayClasses_V411.AvailabilityRequestUnits Unit in Request.Units)
                {
                    var unitReponse = new ChnlHomeAwayClasses_V411.AvailabilityResponseUnits();
                    unitReponse.unitExternalId = Unit.unitExternalId;


                    int _UnitId = Unit.unitExternalId.ToInt32();
                    unitReponse.unitExternalId = Unit.unitExternalId;
                    unitReponse.available = false;
                    unitReponse.errorCode = null;

                    Response.units.Add(unitReponse);



                    #region check Unit avaibility
                    //for sending resonse
                    RNT_TB_ESTATE currEstateTBUnit = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _UnitId);
                    if (currEstateTBUnit == null)
                    {
                        unitReponse.errorCode = "UNKNOWN_PROPERTY";
                        continue;

                    }
                    RntChnlHomeAwayEstateTB currHAEstateUnit = dcChnl.RntChnlHomeAwayEstateTB.SingleOrDefault(x => x.id == currEstateTBUnit.id);
                    if (currHAEstateUnit == null)
                    {
                        unitReponse.errorCode = "UNKNOWN_PROPERTY";
                        continue;
                    }

                    var lastDay = dc.RntSeasonDatesTBL.FirstOrDefault(x => x.dtEnd >= dtEnd && x.dtStart <= dtEnd && x.pidSeasonGroup == currEstateTBUnit.pidSeasonGroup);
                    if (lastDay == null)
                    {
                        unitReponse.errorCode = "END_DAY_MISMATCH";
                        continue;
                    }

                    //if (!(currEstateTB.num_pets_max != null && currEstateTB.num_pets_max > 0))
                    //{
                    if (Request.pets != null && Request.pets.objToInt32() > 0)
                    {
                        //if ((currEstateTBUnit.num_pets_max != null && currEstateTBUnit.num_pets_max == 0))
                        //{
                        //    unitReponse.errorCode = "PETS_NOT_ALLOWED";
                        //    continue;
                        //}
                        var currFeature = dcChnl.RntChnlHomeAwayEstateFeaturesRL.SingleOrDefault(x => x.pidEstate == currHAEstateUnit.id && x.code == "SUITABILITY_PETS_ASK");
                        if (currFeature == null || currFeature.count < Request.pets.objToInt32())
                        {
                            var currFeature_PETS_CONSIDERED = dcChnl.RntChnlHomeAwayEstateFeaturesRL.SingleOrDefault(x => x.pidEstate == currHAEstateUnit.id && x.code == "SUITABILITY_PETS_CONSIDERED");
                            if (currFeature_PETS_CONSIDERED == null || currFeature_PETS_CONSIDERED.count < Request.pets.objToInt32())
                            {
                                unitReponse.errorCode = "PETS_NOT_ALLOWED";
                                continue;
                            }
                        }
                    }
                    //}

                    if (currHAEstateUnit.num_max_sleep.objToInt32() == 0)
                    {
                        currHAEstateUnit.num_max_sleep = currEstateTBUnit.num_persons_max;
                    }
                    if (currEstateTBUnit.nights_max.objToInt32() > 0 && currEstateTBUnit.nights_max < (dtEnd - dtStart).TotalDays.objToInt32())
                    {
                        unitReponse.errorCode = "EXCEEDS_MAX_STAY";
                        continue;
                    }


                    if (currHAEstateUnit.num_max_sleep > 0 && currHAEstateUnit.num_max_sleep < (Request.adults.objToInt32() + children))
                    {
                        unitReponse.errorCode = "EXCEEDS_MAX_OCCUPANCY";
                        continue;
                    }

                    bool _isAvailableUnit = ChnlHomeAwayUtils_V411.CheckAvailability(currEstateTBUnit, dtStart, dtEnd, agentTbl.id);


                    if (!_isAvailableUnit)
                    {
                        unitReponse.errorCode = "PROPERTY_NOT_AVAILABLE";
                        continue;
                    }
                    #region Min Stay
                    rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                    outPrice.dtStart = dtStart;
                    outPrice.dtEnd = dtEnd;

                    outPrice.numPersCount = adults + children;
                    outPrice.numPers_adult = adults;
                    outPrice.numPers_childOver = children;

                    outPrice.fillAgentDetails(agentTbl);
                    DateTime checkDate = DateTime.Now;
                    DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                    DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                    var seasonDates = new List<RntSeasonDatesTBL>();
                    seasonDates = dc.RntSeasonDatesTBL.Where(x => x.pidSeasonGroup == currEstateTBUnit.pidSeasonGroup).OrderByDescending(x => x.dtStart).ToList();

                    if (seasonDates != null && seasonDates.Count > 0)
                    {
                        checkDateEnd = seasonDates.FirstOrDefault().dtEnd;
                    }
                    var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTbl.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                    outPrice.agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                    #region Made change for using values of Acconto and balance configured in agent detail

                    outPrice.part_percentage = currEstateTBUnit.pr_percentage.objToDecimal();

                    //outPrice.part_percentage = currEstateTB.pr_percentage.objToDecimal();
                    #endregion
                    //bool sendMaxPrice = true;
                    //decimal price = rntUtils.rntEstate_getPrice_HA(0, _UnitId, ref outPrice);

                    decimal price = rntUtils.rntEstate_getPrice(0, _UnitId, ref outPrice, ChnlHomeAwayProps_V411.IdAdMedia);
                    if (price == 0)
                    {
                        if (outPrice.outError == 1)
                        {
                            unitReponse.errorCode = "MIN_STAY_NOT_MET";
                            continue;
                        }
                        else if (outPrice.outError == 5)
                        {
                            unitReponse.errorCode = "START_DAY_MISMATCH";
                            continue;
                        }
                        else if (outPrice.outError == 7)
                        {
                            unitReponse.errorCode = "END_DAY_MISMATCH";
                            continue; ;
                        }
                        else
                        {
                            unitReponse.errorCode = "PROPERTY_NOT_AVAILABLE";
                            continue;
                        }
                    }
                    #endregion



                    #endregion

                    if (unitReponse.errorCode == null)
                    {
                        unitReponse.available = true;
                    }
                }

                var jsonSerialiser = new JavaScriptSerializer();
                var returnReponse = JsonConvert.SerializeObject(Response, new Formatting(), new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                return returnReponse;



            }
            catch (Exception ex)
            {
                errorList.Errorcode = "OTHER";
                ErrorLog.addLog("", "HA_AvailabilityRequest", ex.ToString());

            }


        return "";
    }


}
public static class HaListingUtility_V411
{
    public static string AdjustLengthOfData(string strData, int mimLength)
    {
        if (!string.IsNullOrEmpty(strData))
        {
            string _strData = contUtils.CleanHtmlText(strData);
            if (_strData.Length < mimLength && !string.IsNullOrEmpty(_strData))
            {
                int actualLength = _strData.Length;
                int addLength = 400 - actualLength;
                string Addstring = string.Empty;
                if (addLength < actualLength)
                {
                    Addstring = _strData.Substring(0, addLength - 1);

                }
                else
                {

                    do
                    {
                        Addstring += _strData;

                    } while (Addstring.Length <= addLength);

                    //if (Addstring.Length > addLength)
                    //{
                    //    Addstring = Addstring.Substring(0, addLength - 1);
                    //}
                }


                _strData = _strData + "." + Addstring;
                strData = _strData;

            }

        }
        return strData;
    }
    public static string RemoveHtmlTags(string strData)
    {
        if (!String.IsNullOrWhiteSpace(strData))
        {
            return Regex.Replace(strData, "<[^>]*>", string.Empty);

        }

        return strData;

    }
}
public class ChnlHomeAwayClasses_V411
{
    public class ListingClasses
    {
        public class DecimalUnit
        {
            public string Value
            {
                get { return ValueDecimal.HasValue ? (ValueDecimal + "").Replace(",", ".") : ""; }
                set { ValueDecimal = (value + "") != "" ? (value + "").Replace(".", ",").ToDecimal() : (decimal?)null; }
            }
            public decimal? ValueDecimal { get; set; }
            public DecimalUnit()
            {
            }
            public XElement GetElement(string name)
            {
                XElement record = new XElement(name);
                record.Value = Value;
                return record;
            }
        }
        public class Texts
        {
            public int MaxChar { get; set; }
            public int MinChar { get; set; }
            public List<Text> List { get; set; }
            public Texts()
            {
                List = new List<Text>();
                MaxChar = 0;
                MinChar = 0;
            }
            public Texts(int maxChar)
            {
                List = new List<Text>();
                MaxChar = maxChar;
                MinChar = 0;
            }
            public Texts(int maxChar, int minChar)
            {
                List = new List<Text>();
                MaxChar = maxChar;
                MinChar = minChar;
            }
            public void Add(string Value, string Locale)
            {
                List.Add(new Text(Value, Locale, MaxChar, MinChar));
            }
            public bool HasValue()
            {
                return List.Where(x => x.Value != null && x.Value.Trim() != "" && contUtils.CleanHtmlText(x.Value) != "").Count() > 0;
            }
            public XElement GetElement(string name)
            {
                XElement elmParent = new XElement(name);
                XElement elm = new XElement("texts");
                foreach (var tmp in List.Where(x => x.Value != null && x.Value.Trim() != "" && contUtils.CleanHtmlText(x.Value) != ""))
                    elm.Add(tmp.GetElement());
                elmParent.Add(elm);
                return elmParent;
            }
        }
        public class Text
        {
            public int MaxChar { get; set; }
            public int MinChar { get; set; }
            public string Locale { get; set; }
            public string Value { get; set; }
            public Text(string locale)
            {
                Locale = locale;
                MaxChar = 0;
                MinChar = 0;
            }
            public Text(string value, string locale, int maxChar)
            {
                Value = value;
                Locale = locale;
                MaxChar = maxChar;
                MinChar = 0;
            }
            public Text(string value, string locale, int maxChar, int minChar)
            {
                Value = value;
                Locale = locale;
                MaxChar = maxChar;
                MinChar = minChar;
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("text");
                elm.Add(new XAttribute("locale", Locale));
                XElement txtElm = new XElement("textValue");
                txtElm.Value = contUtils.CleanHtmlText(Value);
                if (MaxChar > 0 && txtElm.Value.Length > MaxChar) txtElm.Value = txtElm.Value.cutString(MaxChar - 4);
                if (MinChar > 0 && txtElm.Value.Length < MinChar) txtElm.Value = txtElm.Value;
                elm.Add(txtElm);
                return elm;
            }
        }
        public class LatLng
        {
            public string latitude { get; set; }
            public string longitude { get; set; }
            public LatLng()
            {
            }
            public bool HasValue()
            {
                return !string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude);
            }
            public XElement GetElement(string name)
            {
                XElement elmParent = new XElement(name);
                XElement elm = new XElement("latLng");
                elm.Add(new XElement("latitude", latitude));
                elm.Add(new XElement("longitude", longitude));
                elmParent.Add(elm);
                return elmParent;
            }
        }
        public class xxxx
        {
            /// <summary>xxxx</summary>
            public string xxxxx { get; set; }
            public xxxx()
            {
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("xxxx");
                if (!string.IsNullOrEmpty(xxxxx))
                    elm.Add(new XElement("xxxx", xxxxx));
                return elm;
            }
        }
        public class Listing
        {
            /// <summary>xxxx</summary>
            public DateTime? inserted { get; set; }
            //public DateTime? updated { get; set; }
            public string externalId { get; set; }
            public bool active { get; set; }
            public AdContent adContent { get; set; }
            public List<FeatureValue> featureValues { get; set; }
            public Location location { get; set; }
            public List<Image> images { get; set; }
            public List<Unit> units { get; set; }
            public List<Contact> contacts { get; set; }
            public Listing()
            {
                adContent = new AdContent();
                featureValues = new List<FeatureValue>();
                location = new Location();
                images = new List<Image>();
                units = new List<Unit>();
                contacts = new List<Contact>();
            }
            public string GetXml()
            {
                XElement elm = new XElement("listing");
                if (inserted != null) { elm.Add(new XElement("inserted", inserted)); }
                //if (updated != null) { elm.Add(new XElement("updated", updated)); }

                elm.Add(new XElement("externalId", externalId));
                elm.Add(new XElement("active", active));
                elm.Add(adContent.GetElement());
                if (featureValues != null && featureValues.Count > 0)
                {
                    var featureValuesElm = new XElement("featureValues");
                    foreach (var tmp in featureValues)
                        featureValuesElm.Add(tmp.GetElement());
                    elm.Add(featureValuesElm);
                }
                elm.Add(location.GetElement());
                if (images != null && images.Count > 0)
                {
                    var imagesElm = new XElement("images");
                    foreach (var tmp in images)
                        imagesElm.Add(tmp.GetElement());
                    elm.Add(imagesElm);
                }
                if (units != null && units.Count > 0)
                {
                    var unitsElm = new XElement("units");
                    foreach (var tmp in units)
                        unitsElm.Add(tmp.GetElement());
                    elm.Add(unitsElm);
                }
                if (contacts != null && contacts.Count > 0)
                {
                    var contactsElm = new XElement("contacts");
                    foreach (var tmp in featureValues)
                        contactsElm.Add(tmp.GetElement());
                    elm.Add(contactsElm);
                }
                return elm + "";
            }
        }
        public class Unit
        {
            public string externalId { get; set; }
            public bool active { get; set; }
            public int? area { get; set; }
            public string areaUnit { get; set; } // METERS_SQUARED or SQUARE_FEET.
            public Texts bathroomDetails { get; set; }
            public List<Bathroom> bathrooms { get; set; }
            public Texts bedroomDetails { get; set; }
            public List<Bedroom> bedrooms { get; set; }
            public Texts description { get; set; }
            public int? diningSeating { get; set; }
            public Texts featuresDescription { get; set; }
            public List<FeatureValue> featureValues { get; set; }
            public List<Image> images { get; set; }
            public int? loungeSeating { get; set; }

            public string propertyType { get; set; }
            public string registrationNumber { get; set; }
            public MonetaryInformation unitMonetaryInformation { get; set; }
            public Texts unitName { get; set; }
            public List<RatePeriod> ratePeriods { get; set; }
            public UnitAvailability unitAvailability { get; set; }

            public Unit()
            {
                areaUnit = "METERS_SQUARED";
                bathroomDetails = new Texts(1000);
                bathrooms = new List<Bathroom>();
                bedroomDetails = new Texts(1000);
                bedrooms = new List<Bedroom>();
                description = new Texts(2000);
                featuresDescription = new Texts(60);
                featureValues = new List<FeatureValue>();
                images = new List<Image>();
                unitMonetaryInformation = new MonetaryInformation();
                unitName = new Texts(250);
                ratePeriods = new List<RatePeriod>();
                unitAvailability = new UnitAvailability();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("unit");
                elm.Add(new XElement("externalId", externalId));
                elm.Add(new XElement("active", active));
                if (area.HasValue)
                {
                    elm.Add(new XElement("area", area));
                    elm.Add(new XElement("areaUnit", areaUnit));
                }
                if (bathroomDetails.HasValue())
                    elm.Add(bathroomDetails.GetElement("bathroomDetails"));
                if (bathrooms.Count > 0)
                {
                    var bathroomsElm = new XElement("bathrooms");
                    foreach (var tmp in bathrooms)
                        bathroomsElm.Add(tmp.GetElement());
                    elm.Add(bathroomsElm);
                }
                if (bedroomDetails.HasValue())
                    elm.Add(bedroomDetails.GetElement("bedroomDetails"));
                if (bedrooms.Count > 0)
                {
                    var bedroomsElm = new XElement("bedrooms");
                    foreach (var tmp in bedrooms)
                        bedroomsElm.Add(tmp.GetElement());
                    elm.Add(bedroomsElm);
                }
                if (description.HasValue())
                    elm.Add(description.GetElement("description"));
                if (diningSeating.HasValue)
                    elm.Add(new XElement("diningSeating", diningSeating));
                if (featuresDescription.HasValue())
                    elm.Add(featuresDescription.GetElement("featuresDescription"));
                if (featureValues.Count > 0)
                {
                    var featureValuesElm = new XElement("featureValues");
                    foreach (var tmp in featureValues)
                        featureValuesElm.Add(tmp.GetElement());
                    elm.Add(featureValuesElm);
                }
                //if (images != null && images.Count > 0)
                //{
                //    var imagesElm = new XElement("images");
                //    foreach (var tmp in images)
                //        imagesElm.Add(tmp.GetElement());
                //    elm.Add(imagesElm);
                //}
                if (loungeSeating.HasValue)
                {
                    elm.Add(new XElement("loungeSeating", loungeSeating));
                }

                elm.Add(new XElement("propertyType", propertyType));

                if (!string.IsNullOrEmpty(registrationNumber))
                {
                    elm.Add(new XElement("registrationNumber", registrationNumber));
                }

                elm.Add(unitMonetaryInformation.GetElement());
                if (unitName.HasValue())
                    elm.Add(unitName.GetElement("unitName"));
                if (ratePeriods.Count > 0)
                {
                    var ratePeriodsElm = new XElement("ratePeriods");
                    foreach (var tmp in ratePeriods)
                        ratePeriodsElm.Add(tmp.GetElement());
                    elm.Add(ratePeriodsElm);
                }
                //elm.Add(unitAvailability.GetElement());
                return elm;
            }
        }
        public class Amenity
        {
            public int? count { get; set; }

            public string bathroomFeatureName { get; set; }
            public string bedroomFeatureName { get; set; }

            public XElement GetElement()
            {
                XElement elm = new XElement("amenity");
                if (count.HasValue)
                    elm.Add(new XElement("count", count));

                if (!string.IsNullOrEmpty(bathroomFeatureName))
                    elm.Add(new XElement("bathroomFeatureName", bathroomFeatureName));
                if (!string.IsNullOrEmpty(bedroomFeatureName))
                    elm.Add(new XElement("bedroomFeatureName", bedroomFeatureName));
                return elm;
            }
        }
        public class Bathroom
        {
            public string roomSubType { get; set; } // FULL_BATH, HALF_BATH (no tub or shower), SHOWER_INDOOR_OR_OUTDOOR
            public Texts name { get; set; }
            public Texts note { get; set; }
            public List<Amenity> amenities { get; set; }
            public Bathroom()
            {
                name = new Texts();
                note = new Texts();
                amenities = new List<Amenity>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("bathroom");
                if (amenities != null && amenities.Count > 0)
                {
                    var amenitiesElm = new XElement("amenities");
                    foreach (var tmp in amenities)
                        amenitiesElm.Add(tmp.GetElement());
                    elm.Add(amenitiesElm);
                }

                if (name.HasValue())
                    elm.Add(name.GetElement("name"));

                if (note.HasValue())
                    elm.Add(note.GetElement("note"));

                if (!string.IsNullOrEmpty(roomSubType))
                    elm.Add(new XElement("roomSubType", roomSubType));
                return elm;
            }
        }
        public class Bedroom
        {
            public string roomSubType { get; set; } // BEDROOM, LIVING_SLEEPING_COMBO (studio), OTHER_SLEEPING_AREA
            public Texts name { get; set; }
            public Texts note { get; set; }
            public List<Amenity> amenities { get; set; }
            public Bedroom()
            {
                name = new Texts();
                note = new Texts();
                amenities = new List<Amenity>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("bedroom");
                if (amenities != null && amenities.Count > 0)
                {
                    var amenitiesElm = new XElement("amenities");
                    foreach (var tmp in amenities)
                        amenitiesElm.Add(tmp.GetElement());
                    elm.Add(amenitiesElm);
                }

                if (name.HasValue())
                    elm.Add(name.GetElement("name"));

                if (note.HasValue())
                    elm.Add(note.GetElement("note"));

                if (!string.IsNullOrEmpty(roomSubType))
                    elm.Add(new XElement("roomSubType", roomSubType));
                return elm;
            }
        }
        public class MonetaryInformation
        {
            public Amount cleaningFee { get; set; } // Decimal/Currency, Amount required for the cleaning fee.
            public DecimalUnit contractualBookingDeposit { get; set; } // Decimal/Percentage, Amount required for the deposit.
            public string currency { get; set; } // ISO 3-digit Currency Code in which monetary values are presented for the unit.
            public Amount damageDeposit { get; set; } // Decimal/ Currency, Amount required for the damage deposit.
            public DecimalUnit gratuity { get; set; } // Decimal/Percentage Amount required for gratuity.
            public DecimalUnit nonContractualBookingDeposit { get; set; } // Decimal/Percentage, Amount required for the non-contractual deposit.
            public Texts rateNotes { get; set; } // Additional note about the rates.
            public DecimalUnit tax { get; set; } // Decimal/Percentage, Amount required for the taxes.
            public MonetaryInformation()
            {
                cleaningFee = new Amount();
                contractualBookingDeposit = new DecimalUnit();
                currency = "EUR";
                damageDeposit = new Amount();
                gratuity = new DecimalUnit();
                nonContractualBookingDeposit = new DecimalUnit();
                rateNotes = new Texts();
                tax = new DecimalUnit();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("unitMonetaryInformation");
                if (!string.IsNullOrEmpty(cleaningFee.Value))
                    elm.Add(cleaningFee.GetElement("cleaningFee"));
                if (!string.IsNullOrEmpty(contractualBookingDeposit.Value))
                    elm.Add(contractualBookingDeposit.GetElement("contractualBookingDeposit"));
                elm.Add(new XElement("currency", currency));
                if (!string.IsNullOrEmpty(damageDeposit.Value))
                    elm.Add(damageDeposit.GetElement("damageDeposit"));
                if (!string.IsNullOrEmpty(gratuity.Value))
                    elm.Add(gratuity.GetElement("gratuity"));
                if (!string.IsNullOrEmpty(nonContractualBookingDeposit.Value))
                    elm.Add(nonContractualBookingDeposit.GetElement("nonContractualBookingDeposit"));
                if (rateNotes.HasValue())
                    elm.Add(rateNotes.GetElement("rateNotes"));
                if (!string.IsNullOrEmpty(tax.Value))
                    elm.Add(tax.GetElement("tax"));
                return elm;
            }
        }
        public class RatePeriod
        {
            public DateRange dateRange { get; set; }
            public int minimumStay { get; set; }
            public Texts name { get; set; }
            public Texts note { get; set; }
            public List<Rate> rates { get; set; }
            public RatePeriod()
            {
                dateRange = new DateRange();
                name = new Texts();
                note = new Texts();
                rates = new List<Rate>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("ratePeriod");
                elm.Add(dateRange.GetElement("dateRange"));
                elm.Add(new XElement("minimumStay", minimumStay));
                if (name.List.Count > 0)
                    elm.Add(name.GetElement("name"));
                if (note.List.Count > 0)
                    elm.Add(note.GetElement("note"));
                if (rates.Count > 0)
                {
                    var ratesElm = new XElement("rates");
                    foreach (var tmp in rates)
                        ratesElm.Add(tmp.GetElement());
                    elm.Add(ratesElm);
                }
                return elm;
            }
        }
        public class Rate
        {
            /// <summary>xxxx</summary>
            public string rateType { get; set; }
            public Amount amount { get; set; }
            public Rate()
            {
                amount = new Amount();
            }
            public Rate(string RateType, decimal Amount)
            {
                rateType = RateType;
                amount = new Amount();
                amount.ValueDecimal = Amount;
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("rate");
                elm.Add(new XAttribute("rateType", rateType));
                elm.Add(amount.GetElement("amount"));
                return elm;
            }
        }
        public class UnitAvailability
        {
            public string availabilityDefault { get; set; }
            public string changeOverDefault { get; set; }
            public DateRange dateRange { get; set; }
            public int? maxStayDefault { get; set; }
            public int? minPriorNotifyDefault { get; set; }
            public int? minStayDefault { get; set; }
            public string stayIncrementDefault { get; set; }
            public AvailabilityConfiguration unitAvailabilityConfiguration { get; set; }
            public UnitAvailability()
            {
                availabilityDefault = "Y";
                changeOverDefault = "X";
                dateRange = new DateRange();
                unitAvailabilityConfiguration = new AvailabilityConfiguration();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("unitAvailability");
                if (!string.IsNullOrEmpty(availabilityDefault))
                    elm.Add(new XElement("availabilityDefault", availabilityDefault));
                if (!string.IsNullOrEmpty(changeOverDefault))
                    elm.Add(new XElement("changeOverDefault", changeOverDefault));
                elm.Add(dateRange.GetElement("dateRange"));
                if (maxStayDefault.HasValue)
                    elm.Add(new XElement("maxStayDefault", maxStayDefault));
                if (minPriorNotifyDefault.HasValue)
                    elm.Add(new XElement("minPriorNotifyDefault", minPriorNotifyDefault));
                if (minStayDefault.HasValue)
                    elm.Add(new XElement("minStayDefault", minStayDefault));
                if (!string.IsNullOrEmpty(stayIncrementDefault))
                    elm.Add(new XElement("stayIncrementDefault", stayIncrementDefault));
                elm.Add(unitAvailabilityConfiguration.GetElement());
                return elm;
            }
        }
        public class AvailabilityConfiguration
        {
            public List<string> availability { get; set; } // A letter code for every day of the range. Y=Available, N=Not Available,Q=Inquiry Only. A maximum of 3 years of availability can be given. There cannot be more days given than there are days in the dateRange. An example:YYYNNNQQYYYNNNQQYYYNNNQQYYYNNNQQYYY...
            public List<string> availableUnitCount { get; set; }
            public List<string> changeOver { get; set; } // A letter code for every day of the range. X=no action possible, C=check-in/out, O=check-out only, I=check-in only. A maximum of 3 years of availability can be given. There cannot be more days given than there are days in the dateRange. An example: CCIIOOXCCIIOOXCCIIOOXCCIIOOXCCIIOOXCCIIOOXCCIIO...
            public List<string> maxStay { get; set; } // Maximum stay is a list of comma-separated values 0 thru 999, with each value representing a day in the range and where 0 = "no max stay". An example: 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 …
            public List<string> minPriorNotify { get; set; } // Min prior notification is a list of comma-separated values 0 thru 999, with each value representing a day in the range and where 0 = "no min prior notification". An example: 2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2 …
            public List<string> minStay { get; set; } // Minimum stay is a list of comma-separated values 0 thru 999, with each value representing a day in the range and where 0 = "no min stay”. An example: 2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2 …
            public List<string> stayIncrement { get; set; } // The increment of stay for every day of the range. D=Day, M=Month, W=Week, Y=Year. Example 1: If minStay = 3 and stayIncrement = W, the min stay is 3 weeks and can increment by 1 week. Example 2: If minStay = 2 and stayIncrement = D, the min stay is 2 days and can increment by any amount of days, up to max, after that. An example: DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
            public AvailabilityConfiguration()
            {
                availability = new List<string>();
                availableUnitCount = new List<string>();
                changeOver = new List<string>();
                maxStay = new List<string>();
                minPriorNotify = new List<string>();
                minStay = new List<string>();
                stayIncrement = new List<string>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("unitAvailabilityConfiguration");
                if (availability.Count > 0)
                    elm.Add(new XElement("availability", availability.listToString("")));
                if (availableUnitCount.Count > 0)
                    elm.Add(new XElement("availableUnitCount", availableUnitCount.listToString(",")));
                if (changeOver.Count > 0)
                    elm.Add(new XElement("changeOver", changeOver.listToString("")));
                if (maxStay.Count > 0)
                    elm.Add(new XElement("maxStay", maxStay.listToString(",")));
                if (minPriorNotify.Count > 0)
                    elm.Add(new XElement("minPriorNotify", minPriorNotify.listToString(",")));
                if (minStay.Count > 0)
                    elm.Add(new XElement("minStay", minStay.listToString(",")));
                if (stayIncrement.Count > 0)
                    elm.Add(new XElement("stayIncrement", stayIncrement.listToString("")));
                return elm;
            }
        }
        public class unitRateEntities
        {
            public string listingExternalId { get; set; } // master id
            public string unitExternalId { get; set; }
            public List<RatePeriod> unitRatePeriods { get; set; }

            public unitRateEntities()
            {
                unitRatePeriods = new List<RatePeriod>();
            }
            public string GetXml()
            {
                XElement elm = new XElement("unitRatePeriods");
                elm.Add(new XElement("listingExternalId", listingExternalId));
                elm.Add(new XElement("unitExternalId", unitExternalId));

                XElement elmPeriods = new XElement("ratePeriods");
                foreach (RatePeriod period in unitRatePeriods)
                {
                    elmPeriods.Add(period.GetElement());
                }
                elm.Add(elmPeriods);
                return elm + "";
            }
        }
        #region Update Service Classes


        public class LodgingUnitAvailabilityUpdateRequest
        {

            public string assignedSystemId { get; set; }
            public string advertiserAssignedId { get; set; }
            public string authorizationToken { get; set; }

            public string listingExternalId { get; set; } // master id
            public string unitExternalId { get; set; }
            public UnitAvailability unitAvailability { get; set; }

            public LodgingUnitAvailabilityUpdateRequest()
            {
                unitAvailability = new UnitAvailability();
            }
            public string GetXml()
            {
                XElement elm = new XElement("unitAvailabilityUpdate");
                if (!string.IsNullOrEmpty(assignedSystemId))
                {
                    elm.Add(new XElement("assignedSystemId", assignedSystemId));
                }
                if (!string.IsNullOrEmpty(advertiserAssignedId))
                {
                    elm.Add(new XElement("advertiserAssignedId", advertiserAssignedId));
                }
                if (!string.IsNullOrEmpty(authorizationToken))
                {
                    elm.Add(new XElement("authorizationToken", authorizationToken));
                }

                if (!string.IsNullOrEmpty(listingExternalId))
                {
                    elm.Add(new XElement("listingExternalId", listingExternalId));
                }
                if (!string.IsNullOrEmpty(unitExternalId))
                {
                    elm.Add(new XElement("unitExternalId", unitExternalId));
                }


                if (unitAvailability != null)
                {
                    elm.Add(unitAvailability.GetElement());
                }

                return elm + "";
            }
        }
        public class LodgingUnitAvailabilityUpdateResponse
        {
            public LodgingRateUpdate_Response Reponse { get; set; }

            public LodgingUnitAvailabilityUpdateResponse(string xmlData)
            {
                XDocument _resource = XDocument.Parse(xmlData);
                var elmResponse = _resource.Element("unitAvailabilityUpdateResponse");
                if (elmResponse != null && elmResponse.HasElements)
                {
                    Reponse = new LodgingRateUpdate_Response();
                    Reponse.action = (string)elmResponse.Element("action") ?? "";
                    Reponse.advertiserAssignedId = (string)elmResponse.Element("advertiserAssignedId") ?? "";

                    var elmErrors = elmResponse.Element("errors");
                    if (elmErrors != null && elmErrors.HasElements && elmErrors.Descendants("error").Count() > 0)
                    {
                        Reponse.errors = new List<UpdateServiceError>();
                        foreach (var error in elmErrors.Descendants("error"))
                        {
                            var _errror = new UpdateServiceError();
                            _errror.errorCode = (string)elmResponse.Element("errorCode") ?? "";
                            _errror.message = (string)elmResponse.Element("message") ?? "";
                            Reponse.errors.Add(_errror);
                        }
                    }

                    Reponse.listingExternalId = (string)elmResponse.Element("listingExternalId") ?? "";
                    Reponse.status = (string)elmResponse.Element("status") ?? "";
                    Reponse.unitExternalId = (string)elmResponse.Element("unitExternalId") ?? "";
                }


            }
        }
        public class LodgingUnitAvailabilityUpdate_Response
        {
            public string action { get; set; }
            public string advertiserAssignedId { get; set; }
            public List<UpdateServiceError> errors { get; set; }
            public string listingExternalId { get; set; }
            public string status { get; set; }
            public string unitExternalId { get; set; }
            public LodgingUnitAvailabilityUpdate_Response()
            {
                errors = new List<UpdateServiceError>();
            }

        }

        public class LodgingRateUpdateRequest
        {
            public string assignedSystemId { get; set; }
            public string advertiserAssignedId { get; set; }
            public string authorizationToken { get; set; }

            public string listingExternalId { get; set; } // master id
            public string unitExternalId { get; set; }
            public LodgingRate lodgingRate { get; set; }

            public LodgingRateUpdateRequest()
            {
                lodgingRate = new LodgingRate();
            }
            public string GetXml()
            {
                XElement elm = new XElement("lodgingRateUpdate");
                if (!string.IsNullOrEmpty(assignedSystemId))
                {
                    elm.Add(new XElement("assignedSystemId", assignedSystemId));
                }
                if (!string.IsNullOrEmpty(advertiserAssignedId))
                {
                    elm.Add(new XElement("advertiserAssignedId", advertiserAssignedId));
                }
                if (!string.IsNullOrEmpty(authorizationToken))
                {
                    elm.Add(new XElement("authorizationToken", authorizationToken));
                }

                if (!string.IsNullOrEmpty(listingExternalId))
                {
                    elm.Add(new XElement("listingExternalId", listingExternalId));
                }
                if (!string.IsNullOrEmpty(unitExternalId))
                {
                    elm.Add(new XElement("unitExternalId", unitExternalId));
                }


                if (lodgingRate != null)
                {
                    elm.Add(lodgingRate.GetElement());
                }


                return elm + "";
            }
        }

        public class LodgingRateUpdateResponse
        {
            public LodgingRateUpdate_Response Reponse { get; set; }

            public LodgingRateUpdateResponse(string xmlData)
            {
                XDocument _resource = XDocument.Parse(xmlData);
                var elmResponse = _resource.Element("lodgingRateUpdateResponse");
                if (elmResponse != null && elmResponse.HasElements)
                {
                    Reponse = new LodgingRateUpdate_Response();
                    Reponse.action = (string)elmResponse.Element("action") ?? "";
                    Reponse.advertiserAssignedId = (string)elmResponse.Element("advertiserAssignedId") ?? "";

                    var elmErrors = elmResponse.Element("errors");
                    if (elmErrors != null && elmErrors.HasElements && elmErrors.Descendants("error").Count() > 0)
                    {
                        Reponse.errors = new List<UpdateServiceError>();
                        foreach (var error in elmErrors.Descendants("error"))
                        {
                            var _errror = new UpdateServiceError();
                            _errror.errorCode = (string)elmResponse.Element("errorCode") ?? "";
                            _errror.message = (string)elmResponse.Element("message") ?? "";
                            Reponse.errors.Add(_errror);
                        }
                    }

                    Reponse.listingExternalId = (string)elmResponse.Element("listingExternalId") ?? "";
                    Reponse.status = (string)elmResponse.Element("status") ?? "";
                    Reponse.unitExternalId = (string)elmResponse.Element("unitExternalId") ?? "";
                }


            }
        }
        public class LodgingRateUpdate_Response
        {
            public string action { get; set; }
            public string advertiserAssignedId { get; set; }
            public List<UpdateServiceError> errors { get; set; }
            public string listingExternalId { get; set; }
            public string status { get; set; }
            public string unitExternalId { get; set; }
            public LodgingRateUpdate_Response()
            {
                errors = new List<UpdateServiceError>();
            }

        }

        public class UpdateServiceError
        {
            public string errorCode { get; set; }
            public string message { get; set; }
        }

        #endregion


        public class LodgingConfigurationContent
        {
            public string listingExternalId { get; set; }
            public string unitExternalId { get; set; }
            public LodgingConfigurationDefaults LodgingConfigurationDefaultEstateWise { get; set; }
            public LodgingConfigurationContent()
            {
                LodgingConfigurationDefaultEstateWise = new LodgingConfigurationDefaults();
                LodgingConfigurationDefaultEstateWise.IsEstateWise = true;
            }

            public string GetXml()
            {
                XElement elm = new XElement("lodgingConfigurationContent");
                if (!string.IsNullOrEmpty(listingExternalId))
                {
                    elm.Add(new XElement("listingExternalId", listingExternalId));
                }
                if (!string.IsNullOrEmpty(unitExternalId))
                {
                    elm.Add(new XElement("unitExternalId", unitExternalId));
                }
                if (LodgingConfigurationDefaultEstateWise != null)
                {
                    LodgingConfigurationDefaultEstateWise.IsEstateWise = true;
                    elm.Add(LodgingConfigurationDefaultEstateWise.GetElement());
                }
                return elm + "";
            }

        }




        public class LodgingRateContent
        {
            public string listingExternalId { get; set; } // master id
            public string unitExternalId { get; set; }
            public LodgingRate lodgingRate { get; set; }

            public LodgingRateContent()
            {
                lodgingRate = new LodgingRate();
            }
            public string GetXml()
            {
                XElement elm = new XElement("lodgingRateContent");
                elm.Add(new XElement("listingExternalId", listingExternalId));
                elm.Add(new XElement("unitExternalId", unitExternalId));

                if (lodgingRate != null)
                {
                    elm.Add(lodgingRate.GetElement());
                }


                return elm + "";
            }
        }
        public class LodgingRate
        {
            public string currency { get; set; }
            public string language { get; set; }
            public FlatAmountDiscounts Discounts { get; set; }
            public NightlyRates NightlyRates { get; set; }
            public LodgingRatePaymentSchedule LodgingRatePaymentSchedule { get; set; }

            public LodgingRate()
            {
                currency = "EUR";
                language = "en";
                Discounts = new FlatAmountDiscounts();
                LodgingRatePaymentSchedule = new LodgingRatePaymentSchedule();
                NightlyRates = new NightlyRates();

            }

            public XElement GetElement()
            {
                XElement elm = new XElement("lodgingRate");
                elm.Add(new XElement("currency", currency));
                if (Discounts != null && Discounts.FltDiscounts.Count > 0)
                {
                    elm.Add(Discounts.GetElement());
                }

                elm.Add(new XElement("language", language));


                if (NightlyRates != null)
                {
                    elm.Add(NightlyRates.GetElement());
                }
                if (LodgingRatePaymentSchedule != null)
                {
                    elm.Add(LodgingRatePaymentSchedule.GetElement());
                }
                return elm;
            }

        }
        public class LodgingRatePaymentSchedule
        {
            public List<LodgingRatePayments> LodgingRatePayments { get; set; }

            public LodgingRatePaymentSchedule()
            {
                LodgingRatePayments = new List<LodgingRatePayments>();
            }

            public XElement GetElement()
            {
                XElement elm = new XElement("paymentSchedule");
                XElement elmPay = new XElement("payments");
                //HA 4.1.1 Limit 5 LodgingRatePayments
                foreach (LodgingRatePayments Payment in LodgingRatePayments)
                {
                    elmPay.Add(Payment.GetElement());
                }
                elm.Add(elmPay);
                return elm;
            }

        }
        public class LodgingRatePayments
        {
            public string dueType { get; set; }

            public XElement GetElement()
            {
                XElement elm = new XElement("payment");
                elm.Add(new XElement("dueType", dueType));
                elm.Add(new XElement("requiresRemainder"));

                return elm;
            }

        }
        public static class LodgingRateDefaultData
        {
            public static string DueType = "AT_BOOKING";
        }
        public class NightlyRates
        {
            public NightlyRateAmount DefaultNRateMonToSunday { get; set; }

            public LodgingRateNightlyOverrides LodgingRateNightlyOverrides { get; set; }
            public NightlyRates()
            {
                LodgingRateNightlyOverrides = new LodgingRateNightlyOverrides();
                DefaultNRateMonToSunday = new NightlyRateAmount();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("nightlyRates");

                if (!string.IsNullOrEmpty(DefaultNRateMonToSunday.Value))
                {
                    elm.Add(DefaultNRateMonToSunday.GetElement("fri"));
                    elm.Add(DefaultNRateMonToSunday.GetElement("mon"));
                }

                if (LodgingRateNightlyOverrides != null)
                {
                    elm.Add(LodgingRateNightlyOverrides.GetElement());
                }
                if (!string.IsNullOrEmpty(DefaultNRateMonToSunday.Value))
                {
                    elm.Add(DefaultNRateMonToSunday.GetElement("sat"));
                    elm.Add(DefaultNRateMonToSunday.GetElement("sun"));
                    elm.Add(DefaultNRateMonToSunday.GetElement("thu"));
                    elm.Add(DefaultNRateMonToSunday.GetElement("tue"));
                    elm.Add(DefaultNRateMonToSunday.GetElement("wed"));
                }

                return elm;
            }

        }

        public class FlatAmountDiscounts
        {
            public List<FltDiscount> FltDiscounts { get; set; }
            public FlatAmountDiscounts()
            {
                FltDiscounts = new List<FltDiscount>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("discounts");
                XElement elmflDiscount = new XElement("flatAmountDiscounts");
                if (FltDiscounts != null && FltDiscounts.Count > 0)
                {
                    foreach (FltDiscount discount in FltDiscounts)
                    {
                        elmflDiscount.Add(discount.GetElement());
                    }
                }
                elm.Add(elmflDiscount);
                return elm;

            }
        }

        public class FltDiscount
        {
            public string Name { get; set; }
            public NightlyRateAmount Amount { get; set; }
            public AppliesPerNight appliesPerNight { get; set; }

            public FltDiscount()
            {
                Amount = new NightlyRateAmount();
                appliesPerNight = new AppliesPerNight();
            }

            public XElement GetElement()
            {
                XElement elm = new XElement("discount");

                if (!string.IsNullOrEmpty(Name))
                {
                    elm.Add(new XElement("name", Name));
                }

                if (!string.IsNullOrEmpty(Amount.Value))
                {
                    elm.Add(Amount.GetElement("amount"));
                }
                if (appliesPerNight != null && appliesPerNight.staysOfNight != null && appliesPerNight.staysOfNight.Ranges != null && appliesPerNight.staysOfNight.Ranges.Count > 0)
                {
                    elm.Add(appliesPerNight.GetElement());
                }
                return elm;

            }
        }
        public class AppliesPerNight
        {
            public StaysOfNight staysOfNight { get; set; }
            public AppliesPerNight()
            {
                staysOfNight = new StaysOfNight();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("appliesPerNight");
                if (staysOfNight != null)
                {
                    elm.Add(staysOfNight.GetElement());
                }

                return elm;
            }
        }
        public class StaysOfNight
        {
            public List<NightRange> Ranges { get; set; }
            public StaysOfNight()
            {
                Ranges = new List<NightRange>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("forStaysOfNights");
                foreach (NightRange Range in Ranges)
                {
                    elm.Add(Range.GetElement());
                }
                return elm;
            }
        }

        public class NightRange
        {
            public string max { get; set; }
            public string min { get; set; }

            public NightRange()
            {
            }

            public XElement GetElement(string Range = "")
            {
                if (string.IsNullOrEmpty(Range))
                {
                    Range = "range";
                }
                XElement elm = new XElement(Range);
                if (!string.IsNullOrEmpty(max))
                {
                    elm.Add(new XElement("max", max));
                }
                if (!string.IsNullOrEmpty(min))
                {
                    elm.Add(new XElement("min", min));
                }
                return elm;
            }
        }
        public class LodgingRateNightlyOverrides
        {
            public List<LodgingRateNightlyOverrideContent> LodgingRateNightlyOverrideContents { get; set; }

            public LodgingRateNightlyOverrides()
            {
                LodgingRateNightlyOverrideContents = new List<LodgingRateNightlyOverrideContent>();
            }

            public XElement GetElement()
            {
                XElement elm = new XElement("nightlyOverrides");
                foreach (LodgingRateNightlyOverrideContent Override in LodgingRateNightlyOverrideContents)
                {
                    elm.Add(Override.GetElement());
                }
                return elm;
            }

        }
        public class LodgingRateNightlyOverrideContent
        {
            public NightlyRateAmount Amount { get; set; }
            public LodgingRateNights LodgingRateNights { get; set; }
            public LodgingRateNightlyOverrideContent()
            {
                LodgingRateNights = new LodgingRateNights();
                Amount = new NightlyRateAmount();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("override");
                if (!string.IsNullOrEmpty(Amount.Value))
                {
                    elm.Add(Amount.GetElement("amount"));
                }
                if (LodgingRateNights != null)
                {
                    elm.Add(LodgingRateNights.GetElement());
                }
                return elm;
            }

        }
        public class LodgingRateNights
        {
            public List<LodgingRateNightRange> Ranges { get; set; }
            public LodgingRateNights()
            {
                Ranges = new List<LodgingRateNightRange>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("nights");
                if (Ranges != null)
                {
                    foreach (LodgingRateNightRange range in Ranges)
                    {
                        elm.Add(range.GetElement());
                    }
                }
                return elm;
            }
        }
        public class LodgingRateNightRange
        {
            public Date max { get; set; }
            public Date min { get; set; }

            public LodgingRateNightRange()
            {
                max = new Date();
                min = new Date();
            }

            public XElement GetElement()
            {
                XElement elm = new XElement("range");
                elm.Add(new XElement("max", max.Value));
                elm.Add(new XElement("min", min.Value));
                return elm;
            }
        }


        public class UnitAvailabilityContent
        {
            public string listingExternalId { get; set; } // master id
            public string unitExternalId { get; set; }
            public UnitAvailability unitAvailability { get; set; }
            public UnitAvailabilityContent()
            {
                unitAvailability = new UnitAvailability();
            }
            public string GetXml()
            {
                XElement elm = new XElement("unitAvailabilityContent");
                elm.Add(new XElement("listingExternalId", listingExternalId));
                elm.Add(new XElement("unitExternalId", unitExternalId));
                elm.Add(unitAvailability.GetElement());

                return elm + "";
            }
        }
        public class AdContent
        {
            public Texts accommodationsSummary { get; set; }
            public Texts description { get; set; } // At least 400 characters are required for the listing to pass the minimum content check.
            public Texts headline { get; set; } // At least 20 characters are required for the listing to pass the minimum content check.
            public Texts ownerListingStory { get; set; } // Story describing how the owner came to own the property and the role the property has played in his/her life. This field applies to VRBO only, and will not be displayed otherwise on other sites.
            public Texts propertyName { get; set; } // Name of the property. (*) This field is required for FeWo listings. This field is also displayed on VRBO but will not otherwise be displayed on other sites.
            public Texts uniqueBenefits { get; set; } // Description of the unique benefits offered by this property over others. This field applies to VRBO only, and will not be displayed otherwise on other sites.
            public Texts whyPurchased { get; set; } // Description of why the owner purchased this particular property. This field applies to VRBO only, and will not be displayed otherwise on other sites.
            public string yearPurchased { get; set; } // Year in which the property was purchased. This field applies to VRBO only, and will not be displayed otherwise on other sites.
            public AdContent()
            {
                accommodationsSummary = new Texts(80);
                description = new Texts(10000, 400);
                headline = new Texts(100, 20);
                ownerListingStory = new Texts(2000);
                propertyName = new Texts(30);
                uniqueBenefits = new Texts(2000);
                whyPurchased = new Texts(2000);
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("adContent");
                if (accommodationsSummary.HasValue())
                    elm.Add(accommodationsSummary.GetElement("accommodationsSummary"));
                if (description.HasValue())
                    elm.Add(description.GetElement("description"));
                if (headline.HasValue())
                    elm.Add(headline.GetElement("headline"));
                if (ownerListingStory.HasValue())
                    elm.Add(ownerListingStory.GetElement("ownerListingStory"));
                if (propertyName.HasValue())
                    elm.Add(propertyName.GetElement("propertyName"));
                if (uniqueBenefits.HasValue())
                    elm.Add(uniqueBenefits.GetElement("uniqueBenefits"));
                if (whyPurchased.HasValue())
                    elm.Add(whyPurchased.GetElement("whyPurchased"));
                if (!string.IsNullOrEmpty(yearPurchased))
                    elm.Add(new XElement("yearPurchased", yearPurchased));
                return elm;
            }
        }
        public class Location
        {
            /// <summary>Physical street address of the property including addressLine1, addressLine2, city, stateOrProvince, country, and postal Code.</summary>
            public Address address { get; set; }
            public Texts description { get; set; } // Description of the location of the property.
            public LatLng geoCode { get; set; } // Composed of Latitude/Longitude coordinates. Latitude value must be between -90 and 90 degrees. Longitude value must be between -180 and 180 degrees. Both values cannot be 0.
            public List<NearestPlace> nearestPlaces { get; set; }
            public Texts otherActivities { get; set; } // Description of other activities located near the listing.
            public bool showExactLocation { get; set; }
            public Location()
            {
                address = new Address();
                description = new Texts();
                geoCode = new LatLng();
                nearestPlaces = new List<NearestPlace>();
                otherActivities = new Texts();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("location");
                elm.Add(address.GetElement());
                if (description.HasValue())
                    elm.Add(description.GetElement("description"));
                if (geoCode.HasValue())
                    elm.Add(geoCode.GetElement("geoCode"));
                foreach (var tmp in nearestPlaces)
                    elm.Add(tmp.GetElement());
                if (otherActivities.HasValue())
                    elm.Add(otherActivities.GetElement("otherActivities"));
                elm.Add(new XElement("showExactLocation", showExactLocation));
                return elm;
            }
        }
        public class Address
        {
            public string addressLine1 { get; set; } // Line one of the street data
            public string addressLine2 { get; set; } // Line two of the street data
            public string city { get; set; }
            public string stateOrProvince { get; set; }
            /// <summary>The two-digit country code</summary>
            public string country { get; set; }
            public string postalCode { get; set; } // The postal code of the address
            public Address()
            {
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("address");
                if (!string.IsNullOrEmpty(addressLine1))
                    elm.Add(new XElement("addressLine1", addressLine1));
                if (!string.IsNullOrEmpty(addressLine2))
                    elm.Add(new XElement("addressLine2", addressLine2));
                if (!string.IsNullOrEmpty(city))
                    elm.Add(new XElement("city", city));
                if (!string.IsNullOrEmpty(stateOrProvince))
                    elm.Add(new XElement("stateOrProvince", stateOrProvince));
                if (!string.IsNullOrEmpty(country))
                    elm.Add(new XElement("country", country));
                if (!string.IsNullOrEmpty(postalCode))
                    elm.Add(new XElement("postalCode", postalCode));
                return elm;
            }
        }
        public class NearestPlace
        {
            public DecimalUnit distance { get; set; }
            public string distanceUnit { get; set; } // KILOMETRES, METRES, MILES, MINUTES
            public Texts name { get; set; }
            public string _placeType { get; set; } // AIRPORT, BAR, BEACH, FERRY, GOLF, HIGHWAY, RESTAURANT, SKI, TRAIN
            public NearestPlace()
            {
                name = new Texts();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("nearestPlace");
                elm.Add(new XAttribute("placeType", _placeType));
                if (!string.IsNullOrEmpty(distance.Value))
                    elm.Add(new XElement("distance", distance.Value));
                if (!string.IsNullOrEmpty(distanceUnit))
                    elm.Add(new XElement("distanceUnit", distanceUnit));
                if (name.HasValue())
                    elm.Add(name.GetElement("name"));
                return elm;
            }
        }
        public class FeatureValue
        {
            public int count { get; set; }
            public string listingFeatureName { get; set; }
            public string unitFeatureName { get; set; }
            public FeatureValue()
            {
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("featureValue");
                elm.Add(new XElement("count", count));
                if (!string.IsNullOrEmpty(listingFeatureName))
                    elm.Add(new XElement("listingFeatureName", listingFeatureName));
                if (!string.IsNullOrEmpty(unitFeatureName))
                    elm.Add(new XElement("unitFeatureName", unitFeatureName));
                return elm;
            }
        }
        public class Image
        {
            public string externalId { get; set; }
            public string uri { get; set; }
            public Texts title { get; set; }
            public Image()
            {
                title = new Texts();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("image");
                elm.Add(new XElement("externalId", externalId));
                if (title.HasValue())
                    elm.Add(title.GetElement("title"));
                elm.Add(new XElement("uri", uri));
                return elm;
            }
        }
        public class Contact
        {

            public string email { get; set; }
            public Texts fax { get; set; }
            public Texts name { get; set; }
            public Texts phone1 { get; set; }
            public Texts phone2 { get; set; }
            public Texts phone3 { get; set; }
            public Contact()
            {
                name = new Texts();
                fax = new Texts();
                phone1 = new Texts();
                phone2 = new Texts();
                phone3 = new Texts();
            }

            public string GetXml()
            {
                XElement elm = new XElement("Contact");
                if (!string.IsNullOrEmpty(email))
                    elm.Add(new XElement("email", email));


                if (fax.HasValue())
                    elm.Add(fax.GetElement("fax"));
                if (name.HasValue())
                    elm.Add(name.GetElement("name"));
                if (phone1.HasValue())
                    elm.Add(phone1.GetElement("phone1"));
                if (phone2.HasValue())
                    elm.Add(phone2.GetElement("phone2"));
                if (phone3.HasValue())
                    elm.Add(phone3.GetElement("phone3"));


                return elm + "";
            }
        }
    }
    public class xxx
    {
        private string root;
        private string fileName;
        private string _path { get { return Path.Combine(root, fileName); } }
        private List<UrlItem> _items;

        public List<UrlItem> Items { get { if (this._items != null)return this._items; else return new List<UrlItem>(); } set { this._items = value; } }
        public xxx(string Root)
        {
            root = Root;
            fileName = "utilsUrlList.xml";
            LoadObject();
        }
        public xxx(string Root, string FileName)
        {
            root = Root;
            fileName = FileName;
            LoadObject();
        }
        public void Cleare()
        {
            this._items = new List<UrlItem>();
            if (File.Exists(this._path)) File.Delete(this._path);
        }
        private void LoadObject()
        {
            this._items = new List<UrlItem>();
            if (!File.Exists(this._path)) return;
            XDocument _resource = XDocument.Load(this._path);
            var ds = from XElement e in _resource.Descendants("urlitem")
                     select e;
            foreach (XElement e in ds)
            {
                UrlItem item = new UrlItem();
                item.Type = e.Attribute("type").Value;
                item.Lang = e.Attribute("lang") != null ? e.Attribute("lang").Value : "";
                item.Url = e.Element("url").Value;
                item.Value = e.Element("value").Value;
                this._items.Add(item);
            }
        }
        public void Save()
        {
            XDocument _resource = new XDocument();
            XElement rootElement = new XElement("urllist");
            foreach (UrlItem item in this._items)
            {
                XElement record = new XElement("urlitem");
                record.Add(new XAttribute("type", item.Type));
                record.Add(new XAttribute("lang", item.Lang));
                record.Add(new XElement("url", item.Url));
                record.Add(new XElement("value", item.Value));
                rootElement.Add(record);
            }
            _resource.Add(rootElement);
            _resource.Save(this._path);
        }
    }
    public class QuoteRequest
    {
        public string documentVersion { get; set; }
        public string advertiserAssignedId { get; set; }
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public string inquiryId { get; set; }
        public string propertyUrl { get; set; }
        public string listingChannel { get; set; }
        public string masterListingChannel { get; set; }
        public string trackingUuid { get; set; }
        public string message { get; set; }
        public Inquirer inquirer { get; set; }
        public Reservation reservation { get; set; }
        public QuoteRequest()
        {
            inquirer = new Inquirer();
            reservation = new Reservation();
        }
        public QuoteRequest(string xmlData)
        {
            inquirer = new Inquirer();
            reservation = new Reservation();

            XDocument _resource = XDocument.Parse(xmlData);
            var ds1 = _resource.Element("quoteRequest").Element("documentVersion");
            documentVersion = (string)ds1.Element("documentVersion") ?? "";
            var ds = _resource.Element("quoteRequest").Element("quoteRequestDetails");
            advertiserAssignedId = (string)ds.Element("advertiserAssignedId") ?? "";
            listingExternalId = (string)ds.Element("listingExternalId") ?? "";
            unitExternalId = (string)ds.Element("unitExternalId") ?? "";
            inquiryId = (string)ds.Element("inquiryId") ?? "";
            propertyUrl = (string)ds.Element("propertyUrl") ?? "";
            listingChannel = (string)ds.Element("listingChannel") ?? "";
            masterListingChannel = (string)ds.Element("masterListingChannel") ?? "";
            trackingUuid = (string)ds.Element("trackingUuid") ?? "";
            message = (string)ds.Element("message") ?? "";

            inquirer = new Inquirer(ds.Element("inquirer"));
            if (inquirer.locale == "") inquirer.locale = "en_US";

            reservation = new Reservation(ds.Element("reservation"));
        }
    }
    public class QuoteResponse
    {
        public string documentVersion { get; set; }
        public string locale { get; set; }
        public List<Order> orderList { get; set; }
        public RentalAgreement rentalAgreement { get; set; }
        public QuoteResponse()
        {
            orderList = new List<Order>();
            rentalAgreement = new RentalAgreement();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement quoteResponse = new XElement("quoteResponse");
            if (!string.IsNullOrEmpty(documentVersion))
                quoteResponse.Add(new XElement("documentVersion", documentVersion));
            XElement quoteResponseDetails = new XElement("quoteResponseDetails");
            if (!string.IsNullOrEmpty(locale))
                quoteResponseDetails.Add(new XElement("locale", locale));
            XElement orderListElm = new XElement("orderList");
            quoteResponseDetails.Add(orderListElm);
            foreach (Order order in orderList)
                orderListElm.Add(order.GetElement());
            if (rentalAgreement.HasValue())
                quoteResponseDetails.Add(rentalAgreement.GetElement());
            quoteResponse.Add(quoteResponseDetails);
            _resource.Add(quoteResponse);
            return _resource.ToString();
        }
    }
    public class BookingRequest
    {
        public string documentVersion { get; set; }
        public string advertiserAssignedId { get; set; }
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public string inquiryId { get; set; }
        public string propertyUrl { get; set; }
        public string listingChannel { get; set; }
        public string masterListingChannel { get; set; }
        public string trackingUuid { get; set; }
        public string message { get; set; }
        public Inquirer inquirer { get; set; }
        public Reservation reservation { get; set; }
        public List<OrderItem> orderItemList { get; set; }
        public PaymentForm paymentForm { get; set; }
        public string travelerSource { get; set; }
        public string commission { get; set; }
        public BookingRequest()
        {
            inquirer = new Inquirer();
            reservation = new Reservation();
            orderItemList = new List<OrderItem>();
            paymentForm = new PaymentForm();
        }
        public BookingRequest(string xmlData)
        {
            inquirer = new Inquirer();
            reservation = new Reservation();
            orderItemList = new List<OrderItem>();
            paymentForm = new PaymentForm();

            XDocument _resource = XDocument.Parse(xmlData);
            var ds1 = _resource.Element("bookingRequest").Element("documentVersion");
            documentVersion = (string)ds1.Element("documentVersion") ?? "";
            var ds = _resource.Element("bookingRequest").Element("bookingRequestDetails");
            advertiserAssignedId = (string)ds.Element("advertiserAssignedId") ?? "";
            listingExternalId = (string)ds.Element("listingExternalId") ?? "";
            unitExternalId = (string)ds.Element("unitExternalId") ?? "";
            inquiryId = (string)ds.Element("inquiryId") ?? "";
            propertyUrl = (string)ds.Element("propertyUrl") ?? "";
            listingChannel = (string)ds.Element("listingChannel") ?? "";
            masterListingChannel = (string)ds.Element("masterListingChannel") ?? "";
            trackingUuid = (string)ds.Element("trackingUuid") ?? "";
            message = (string)ds.Element("message") ?? "";
            inquirer = new Inquirer(ds.Element("inquirer"));
            if (inquirer.locale == "") inquirer.locale = "en_US";
            commission = (string)ds.Element("commission") ?? ""; // ?????
            reservation = new Reservation(ds.Element("reservation"));
            var orderItemListElm = ds.Element("orderItemList");
            if (orderItemListElm != null && orderItemListElm.HasElements && orderItemListElm.Descendants("orderItem").Count() > 0)
                foreach (var orderItemElm in orderItemListElm.Descendants("orderItem"))
                {
                    orderItemList.Add(new OrderItem(orderItemElm));
                }
            paymentForm = new PaymentForm(ds.Element("paymentForm"));
            travelerSource = (string)ds.Element("travelerSource") ?? "";
        }
    }
    public class BookingResponse
    {
        public string documentVersion { get; set; }
        public string advertiserAssignedId { get; set; }
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public string externalId { get; set; }
        public string guestProfileExternalId { get; set; } // The PM or PMSC’s unique identifier for the traveler
        public Inquirer inquirer { get; set; } // Inquirer details are only required in the Booking Response for External Reservation Updates
        public string locale { get; set; }
        public List<Order> orderList { get; set; }
        public string ownerProfileExternalId { get; set; } // The PM or PMSC’s unique identifier for the owner
        public RentalAgreement rentalAgreement { get; set; }
        public string reservationPaymentStatus { get; set; } // UNPAID, PARTIAL_PAID, OVERPAID, PAID, EXTERNAL_SOR
        public Reservation reservation { get; set; }
        public string reservationStatus { get; set; } // BUILDING, CANCELLED, CANCELLED_BY_OWNER, CANCELLED_BY_TRAVELER, CONFIRMED, DECLINED_BY_OWNER, DECLINED_BY_SYSTEM, STAY_IN_PROGRESS, UNCONFIRMED, UNCONFIRMED_BY_OWNER, UNCONFIRMED_BY_TRAVELER
        public BookingResponse()
        {
            inquirer = new Inquirer();
            orderList = new List<Order>();
            rentalAgreement = new RentalAgreement();
            reservation = new Reservation();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement bookingResponse = new XElement("bookingResponse");
            if (!string.IsNullOrEmpty(documentVersion))
                bookingResponse.Add(new XElement("documentVersion", documentVersion));
            XElement bookingResponseDetails = new XElement("bookingResponseDetails");
            if (!string.IsNullOrEmpty(advertiserAssignedId))
                bookingResponseDetails.Add(new XElement("advertiserAssignedId", advertiserAssignedId));
            if (!string.IsNullOrEmpty(listingExternalId))
                bookingResponseDetails.Add(new XElement("listingExternalId", listingExternalId));
            if (!string.IsNullOrEmpty(unitExternalId))
                bookingResponseDetails.Add(new XElement("unitExternalId", unitExternalId));
            if (!string.IsNullOrEmpty(externalId))
                bookingResponseDetails.Add(new XElement("externalId", externalId));
            if (!string.IsNullOrEmpty(guestProfileExternalId))
                bookingResponseDetails.Add(new XElement("guestProfileExternalId", guestProfileExternalId));
            //bookingResponseDetails.Add(inquirer.GetElement());
            if (!string.IsNullOrEmpty(locale))
                bookingResponseDetails.Add(new XElement("locale", locale));
            XElement orderListElm = new XElement("orderList");
            bookingResponseDetails.Add(orderListElm);
            foreach (Order order in orderList)
                orderListElm.Add(order.GetElement());
            if (!string.IsNullOrEmpty(ownerProfileExternalId))
                bookingResponseDetails.Add(new XElement("ownerProfileExternalId", ownerProfileExternalId));
            if (rentalAgreement.HasValue())
                bookingResponseDetails.Add(rentalAgreement.GetElement());
            if (!string.IsNullOrEmpty(reservationPaymentStatus))
                bookingResponseDetails.Add(new XElement("reservationPaymentStatus", reservationPaymentStatus));
            bookingResponseDetails.Add(reservation.GetElement());
            if (!string.IsNullOrEmpty(reservationStatus))
                bookingResponseDetails.Add(new XElement("reservationStatus", reservationStatus));
            bookingResponse.Add(bookingResponseDetails);
            _resource.Add(bookingResponse);
            return _resource.ToString();
        }
    }

    public class AvailabilityRequest
    {
        public string requestVersion { get; set; }
        public string systemExternalId { get; set; }
        public string advertiserExternalId { get; set; }
        public string listingExternalId { get; set; }

        public AvailabilityRequestDateRange dateRange { get; set; }

        public int? adults { get; set; }
        public int? children { get; set; }
        public int? pets { get; set; }
        public List<AvailabilityRequestUnits> Units { get; set; }

        public AvailabilityRequest()
        {
            Units = new List<AvailabilityRequestUnits>();
            dateRange = new AvailabilityRequestDateRange();
        }
    }
    public class AvailabilityRequestDateRange
    {
        public string arrivalDate { get; set; }
        public string departureDate { get; set; }
    }
    //public class AvailabilityRequestDateRange
    //{
    //    public Date arrivalDate { get; set; }
    //    public Date departureDate { get; set; }
    //    public AvailabilityRequestDateRange()
    //    {
    //        arrivalDate = new Date();
    //        departureDate = new Date();
    //    }

    //}
    public class AvailabilityRequestUnits
    {
        public string unitExternalId { get; set; }

        public AvailabilityRequestUnits()
        {

        }
    }
    public class AvailabilityResponse
    {
        public List<AvailabilityResponseUnits> units { get; set; }

        public AvailabilityResponse()
        {
            units = new List<AvailabilityResponseUnits>();
        }

    }
    public class AvailabilityResponseUnits
    {
        public string unitExternalId { get; set; }
        public bool available { get; set; }
        public string errorCode { get; set; }

        public AvailabilityResponseUnits()
        {

        }
    }
    public class BookingResponseUpdate
    {
        public string documentVersion { get; set; }
        public string advertiserAssignedId { get; set; }
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public string externalId { get; set; }
        public string guestProfileExternalId { get; set; } // The PM or PMSC’s unique identifier for the traveler
        public InquirerUpdate inquirer { get; set; } // Inquirer details are only required in the Booking Response for External Reservation Updates
        public string locale { get; set; }
        public List<OrderUpdate> orderList { get; set; }
        public string ownerProfileExternalId { get; set; } // The PM or PMSC’s unique identifier for the owner
        public RentalAgreement rentalAgreement { get; set; }
        public string reservationPaymentStatus { get; set; } // UNPAID, PARTIAL_PAID, OVERPAID, PAID, EXTERNAL_SOR
        public Reservation reservation { get; set; }
        //public string reservationOriginationDate { get; set; } // For offline booking, the date and time the reservation was created in the external system.
        public string reservationStatus { get; set; } // BUILDING, CANCELLED, CANCELLED_BY_OWNER, CANCELLED_BY_TRAVELER, CONFIRMED, DECLINED_BY_OWNER, DECLINED_BY_SYSTEM, STAY_IN_PROGRESS, UNCONFIRMED, UNCONFIRMED_BY_OWNER, UNCONFIRMED_BY_TRAVELER

        public BookingResponseUpdate()
        {
            inquirer = new InquirerUpdate();
            orderList = new List<OrderUpdate>();
            rentalAgreement = new RentalAgreement();
            reservation = new Reservation();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement bookingResponse = new XElement("bookingUpdate");
            if (!string.IsNullOrEmpty(documentVersion))
                bookingResponse.Add(new XElement("documentVersion", documentVersion));
            XElement bookingResponseDetails = new XElement("bookingUpdateDetails");
            if (!string.IsNullOrEmpty(advertiserAssignedId))
                bookingResponseDetails.Add(new XElement("advertiserAssignedId", advertiserAssignedId));
            if (!string.IsNullOrEmpty(listingExternalId))
                bookingResponseDetails.Add(new XElement("listingExternalId", listingExternalId));
            if (!string.IsNullOrEmpty(unitExternalId))
                bookingResponseDetails.Add(new XElement("unitExternalId", unitExternalId));
            if (!string.IsNullOrEmpty(externalId))
                bookingResponseDetails.Add(new XElement("externalId", externalId));
            if (!string.IsNullOrEmpty(guestProfileExternalId))
                bookingResponseDetails.Add(new XElement("guestProfileExternalId", guestProfileExternalId));
            bookingResponseDetails.Add(inquirer.GetElement());
            if (!string.IsNullOrEmpty(locale))
                bookingResponseDetails.Add(new XElement("locale", locale));
            XElement orderListElm = new XElement("orderList");
            bookingResponseDetails.Add(orderListElm);
            foreach (OrderUpdate order in orderList)
                orderListElm.Add(order.GetElement());
            if (!string.IsNullOrEmpty(ownerProfileExternalId))
                bookingResponseDetails.Add(new XElement("ownerProfileExternalId", ownerProfileExternalId));
            if (rentalAgreement.HasValue())
                bookingResponseDetails.Add(rentalAgreement.GetElement());
            if (!string.IsNullOrEmpty(reservationPaymentStatus))
                bookingResponseDetails.Add(new XElement("reservationPaymentStatus", reservationPaymentStatus));
            bookingResponseDetails.Add(reservation.GetElement());

            if (!string.IsNullOrEmpty(reservationStatus))
                bookingResponseDetails.Add(new XElement("reservationStatus", reservationStatus));

            bookingResponse.Add(bookingResponseDetails);
            _resource.Add(bookingResponse);
            return _resource.ToString();
        }
    }
    public class BookingContentIndexRequest
    {
        public string documentVersion { get; set; }
        public DateAndTime startDate { get; set; }
        public DateAndTime endDate { get; set; }
        public Advertiser advertiser { get; set; }
        public BookingContentIndexRequest()
        {
            startDate = new DateAndTime();
            endDate = new DateAndTime();
            advertiser = new Advertiser();
        }
        public BookingContentIndexRequest(string xmlData)
        {
            startDate = new DateAndTime();
            endDate = new DateAndTime();

            advertiser = new Advertiser();
            if (xmlData == "") return;
            XDocument _resource = XDocument.Parse(xmlData);
            var ds1 = _resource.Element("bookingContentIndexRequest").Element("documentVersion");
            documentVersion = (string)ds1.Element("documentVersion") ?? "";
            var ds = _resource.Element("bookingContentIndexRequest");

            startDate.Value = (string)ds.Element("startDate") ?? "";
            endDate.Value = (string)ds.Element("endDate") ?? "";

            advertiser = (new Advertiser(ds.Element("advertiser")));
        }
    }
    public class BookingContentIndexResponse
    {
        public string documentVersion { get; set; }
        public Advertiser advertiser { get; set; }
        public BookingContentIndexResponse()
        {
            advertiser = new Advertiser();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("bookingContentIndex");
            if (!string.IsNullOrEmpty(documentVersion))
                root.Add(new XElement("documentVersion", documentVersion));
            if (advertiser != null)
            {
                root.Add(advertiser.GetElement());
            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }
    public class AdvertiserListingContentIndexResponse
    {
        public string documentVersion { get; set; }
        public Advertiser Advertiser { get; set; }
        public AdvertiserListingContentIndexResponse()
        {
            documentVersion = "4.2";
            Advertiser = new Advertiser();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("advertiserListingContentIndex");
            root.Add(new XElement("documentVersion", documentVersion));
            if (Advertiser != null && Advertiser.listingContentIndexEntry.Count > 0)
            {
                root.Add(Advertiser.GetElement());


            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }
    public class LodgingConfigurationContentIndexEntry
    {
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }
        public string lodgingConfigurationContentUrl { get; set; }

        public LodgingConfigurationContentIndexEntry()
        {
            lastUpdatedDate = new DateAndTime();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("lodgingConfigurationContentIndexEntry");
            if (!string.IsNullOrEmpty(listingExternalId))
                elm.Add(new XElement("listingExternalId", listingExternalId));
            if (!string.IsNullOrEmpty(unitExternalId))
                elm.Add(new XElement("unitExternalId", unitExternalId));
            if (lastUpdatedDate.Value != "")
                elm.Add(lastUpdatedDate.GetElement("lastUpdatedDate"));
            if (!string.IsNullOrEmpty(lodgingConfigurationContentUrl))
                elm.Add(new XElement("lodgingConfigurationContentUrl", lodgingConfigurationContentUrl));
            return elm;
        }

    }
    public class LodgingConfigurationDefaults
    {
        public LodgingConfigurationAcceptedPaymentForms AcceptedPaymentForm { get; set; } // inside tags not required keep it blank Main Tag as of now
        public BookingPolicy BookingPolicy { get; set; }
        public CancellationPolicy CancellationPolicy { get; set; }
        public string checkInTime { get; set; }
        public string checkOutTime { get; set; }
        public ChildrenAllowedRule ChildrenAllowedRule { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }
        public string locale { get; set; }
        public EventsAllowedRule EventsAllowedRule { get; set; }
        public MaximumOccupancyRule MaximumOccupancyRule { get; set; }
        public MinimumAgeRule minimumAgeRule { get; set; }
        public PetsAllowedRule petsAllowedRule { get; set; }
        public PricingPolicy pricingPolicy { get; set; }
        public RentalAgreementFile rentalAgreementFile { get; set; }
        public SmokingAllowedRule smokingAllowedRule { get; set; }
        public bool IsEstateWise { get; set; }


        public LodgingConfigurationDefaults()
        {
            AcceptedPaymentForm = new LodgingConfigurationAcceptedPaymentForms();
            BookingPolicy = new BookingPolicy();
            CancellationPolicy = new CancellationPolicy();
            checkInTime = "15:00";
            checkOutTime = "10:00";
            ChildrenAllowedRule = new ChildrenAllowedRule();
            lastUpdatedDate = new DateAndTime();
            EventsAllowedRule = new EventsAllowedRule();
            locale = "en";
            MaximumOccupancyRule = new MaximumOccupancyRule();
            minimumAgeRule = new MinimumAgeRule();
            petsAllowedRule = new PetsAllowedRule();
            pricingPolicy = new PricingPolicy();
            rentalAgreementFile = new RentalAgreementFile();
            smokingAllowedRule = new SmokingAllowedRule();

        }
        public XElement GetElement()
        {
            string MainNodeName = "lodgingConfigurationDefaults";
            if (IsEstateWise)
            {
                MainNodeName = "lodgingConfiguration";
            }
            XElement elm = new XElement(MainNodeName);
            if (AcceptedPaymentForm != null && !IsEstateWise)
            { elm.Add(AcceptedPaymentForm.GetElement()); }
            if (BookingPolicy != null)
            { elm.Add(BookingPolicy.GetElement()); }
            if (CancellationPolicy != null && !IsEstateWise)
            { elm.Add(CancellationPolicy.GetElement()); }
            if (!string.IsNullOrEmpty(checkInTime))
            { elm.Add(new XElement("checkInTime", checkInTime)); }
            if (!string.IsNullOrEmpty(checkOutTime))
            { elm.Add(new XElement("checkOutTime", checkOutTime)); }

            if (ChildrenAllowedRule != null)
            { elm.Add(ChildrenAllowedRule.GetElement()); }
            if (EventsAllowedRule != null && !IsEstateWise)
            { elm.Add(EventsAllowedRule.GetElement()); }

            if (lastUpdatedDate.Value != "")
            { elm.Add(lastUpdatedDate.GetElement("lastUpdatedDate")); }
            elm.Add(new XElement("locale", locale));

            if (MaximumOccupancyRule != null)
            { elm.Add(MaximumOccupancyRule.GetElement()); }

            if (minimumAgeRule != null && !IsEstateWise)
            { elm.Add(minimumAgeRule.GetElement()); }

            if (petsAllowedRule != null)
            { elm.Add(petsAllowedRule.GetElement()); }

            if (pricingPolicy != null && !IsEstateWise)
            { elm.Add(pricingPolicy.GetElement()); }

            if (rentalAgreementFile != null)
            { elm.Add(rentalAgreementFile.GetElement()); }

            if (smokingAllowedRule != null && !IsEstateWise)
            { elm.Add(smokingAllowedRule.GetElement()); }


            return elm;
        }
    }
    public static class LodgingConfigurationStaticData
    {
        public static string bookingPolicy = "INSTANT";
        public static string cancellationPolicy = "MODERATE";
        public static string check_InTime = "15:00";
        public static string check_OutTime = "10:00";
        public static bool childrenAllowedRule = true;
        public static bool eventsAllowedRule = false;
        public static string locale = "en";
        public static int MaximumNoOfGuests = 3;
        public static int minAgeRule = 18;
        public static string rentalAgreementPdfUrl = App.HOST + "/" + CommonUtilities.getSYS_SETTING("ha411terms-and-conditionsPdf");
        public static string pricingPolicy = "GUARANTEED";
        public static bool petsAllowed = false;
        public static bool smokingAllowedRule = false;
    }
    public class SmokingAllowedRule
    {
        public bool allowed { get; set; }

        public XElement GetElement()
        {
            XElement elm = new XElement("smokingAllowedRule");
            elm.Add(new XElement("allowed", allowed));
            return elm;
        }
    }

    public class RentalAgreementFile
    {
        public string locale { get; set; }
        public string rentalAgreementPdfUrl { get; set; }
        public RentalAgreementFile()
        {
            locale = LodgingConfigurationStaticData.locale;
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("rentalAgreementFile");
            elm.Add(new XAttribute("locale", locale));
            elm.Add(new XElement("rentalAgreementPdfUrl", rentalAgreementPdfUrl));

            return elm;

        }
    }
    public class PricingPolicy
    {
        public string policy { get; set; }

        public XElement GetElement()
        {
            XElement elm = new XElement("pricingPolicy");
            elm.Add(new XElement("policy", policy));
            return elm;

        }
    }
    public class PetsAllowedRule
    {
        public bool allowed { get; set; }

        public XElement GetElement()
        {
            XElement elm = new XElement("petsAllowedRule");
            elm.Add(new XElement("allowed", allowed));
            return elm;
        }
    }
    public class MinimumAgeRule
    {
        public int age { get; set; }

        public XElement GetElement()
        {
            XElement elm = new XElement("minimumAgeRule");
            elm.Add(new XElement("age", age));
            return elm;
        }
    }
    public class MaximumOccupancyRule
    {
        public int? guests { get; set; }
        public XElement GetElement()
        {
            XElement elm = new XElement("maximumOccupancyRule");
            elm.Add(new XElement("guests", guests));
            return elm;
        }
    }
    public class EventsAllowedRule
    {
        public bool allowed { get; set; }

        public XElement GetElement()
        {
            XElement elm = new XElement("eventsAllowedRule");
            elm.Add(new XElement("allowed", allowed));
            return elm;
        }

    }
    public class ChildrenAllowedRule
    {
        public bool allowed { get; set; }

        public XElement GetElement()
        {
            XElement elm = new XElement("childrenAllowedRule");
            elm.Add(new XElement("allowed", allowed));
            return elm;
        }

    }
    public class CancellationPolicy
    {
        public string policy { get; set; }
        public XElement GetElement()
        {
            XElement elm = new XElement("cancellationPolicy");
            elm.Add(new XElement("policy", policy));

            return elm;
        }
    }
    public class BookingPolicy
    {

        public string policy { get; set; }
        public XElement GetElement()
        {
            XElement elm = new XElement("bookingPolicy");
            elm.Add(new XElement("policy", policy));

            return elm;

        }
    }
    public class LodgingConfigurationAcceptedPaymentForms
    {

        public List<paymentCardDescriptor> paymentCardDescriptors { get; set; }

        public LodgingConfigurationAcceptedPaymentForms()
        {
            paymentCardDescriptors = new List<paymentCardDescriptor>();
        }

        public XElement GetElement()
        {
            XElement elm = new XElement("acceptedPaymentForms");
            foreach (paymentCardDescriptor paymentCardDescriptor in paymentCardDescriptors)
            {
                elm.Add(paymentCardDescriptor.GetElement());
            }


            return elm;
        }
    }
    public class paymentCardDescriptor
    {


        public string paymentFormType { get; set; }
        public string cardCode { get; set; }
        public string cardType { get; set; }
        public XElement GetElement()
        {
            XElement elm = new XElement("paymentCardDescriptor");
            if (!string.IsNullOrEmpty(paymentFormType))
            {
                elm.Add(new XElement("paymentFormType", paymentFormType));
            }
            if (!string.IsNullOrEmpty(cardCode))
            {
                elm.Add(new XElement("cardCode", cardCode));
            }
            if (!string.IsNullOrEmpty(cardType))
            {
                elm.Add(new XElement("cardType", cardType));
            }
            return elm;

        }

    }


    public class LodgingConfigurationContentIndexResponse
    {
        public string documentVersion { get; set; }
        public Advertiser Advertiser { get; set; }

        public LodgingConfigurationContentIndexResponse()
        {
            documentVersion = "4.2";
            Advertiser = new Advertiser();
            Advertiser.LodgingConfigurationDefaults = new LodgingConfigurationDefaults();

        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("advertiserLodgingConfigurationContentIndex");
            root.Add(new XElement("documentVersion", documentVersion));
            if (Advertiser != null && Advertiser.LodgingConfigurationDefaults != null)
            {
                root.Add(Advertiser.GetElement());
            }
            _resource.Add(root);
            return _resource.ToString();


        }
    }


    public class AdvertiserContactContentIndexResponse
    {
        public string documentVersion { get; set; }
        public Advertiser Advertiser { get; set; }
        public AdvertiserContactContentIndexResponse()
        {
            documentVersion = "4.1.1";
            Advertiser = new Advertiser();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("advertiserContactContentIndex");
            root.Add(new XElement("documentVersion", documentVersion));
            if (Advertiser != null)
            {
                root.Add(Advertiser.GetElement());
            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }

    public class AdvertiserContentIndexResponse
    {
        public List<AdvertiserContentIndexEntry> AdvertiserContentIndexEntries { get; set; }
        public AdvertiserContentIndexResponse()
        {
            AdvertiserContentIndexEntries = new List<AdvertiserContentIndexEntry>();
            documentVersion = "4.1.1";
        }
        public string documentVersion { get; set; }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("advertiserContentIndex");
            root.Add(new XElement("documentVersion", documentVersion));

            if (AdvertiserContentIndexEntries.Count > 0)
            {
                foreach (var tmp in AdvertiserContentIndexEntries)
                {
                    root.Add(tmp.GetElement());
                }
            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }



    public class AdvertiserLodgingRateContentIndexResponse
    {
        public AdvertiserRate advertiser { get; set; }
        public string documentVersion { get; set; }
        public AdvertiserLodgingRateContentIndexResponse()
        {
            advertiser = new AdvertiserRate();
            documentVersion = "4.2";
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("advertiserLodgingRateContentIndex");
            root.Add(new XElement("documentVersion", documentVersion));
            if (advertiser != null)
            {
                root.Add(advertiser.GetElement());
            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }

    public class AdvertiserUnitAvailabilityContentIndexResponse
    {
        public List<AdvertiserAvailability> advertisers { get; set; }
        public string documentVersion { get; set; }
        public AdvertiserUnitAvailabilityContentIndexResponse()
        {
            advertisers = new List<AdvertiserAvailability>();
            documentVersion = "4.2";
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("advertiserUnitAvailabilityContentIndex");
            root.Add(new XElement("documentVersion", documentVersion));
            if (advertisers.Count > 0)
            {
                foreach (var tmp in advertisers)
                {
                    root.Add(tmp.GetElement());
                }


            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }




    public class InquirySearchRequest
    {
        public string assignedSystemId { get; set; }
        public string advertiserAssignedId { get; set; }
        public string listingExternalId { get; set; }
        public string timePeriod { get; set; } // PAST_FIFTEEN_MINUTES, PAST_TWENTY_FOUR_HOURS, PAST_THIRTY_MINUTES, PAST_THREE_DAYS, PAST_HOUR, PAST_SEVEN_DAYS, PAST_FOUR_HOURS
        public string authorizationToken { get; set; }
        public InquirySearchRequest()
        {
            timePeriod = "PAST_HOUR";
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("inquirySearch");
            if (!string.IsNullOrEmpty(assignedSystemId))
                root.Add(new XElement("assignedSystemId", assignedSystemId));
            if (!string.IsNullOrEmpty(advertiserAssignedId))
                root.Add(new XElement("advertiserAssignedId", advertiserAssignedId));
            if (!string.IsNullOrEmpty(listingExternalId))
                root.Add(new XElement("listingExternalId", listingExternalId));
            if (!string.IsNullOrEmpty(timePeriod))
                root.Add(new XElement("timePeriod", timePeriod));
            if (!string.IsNullOrEmpty(authorizationToken))
                root.Add(new XElement("authorizationToken", authorizationToken));
            _resource.Add(root);
            return _resource.ToString();
        }
    }
    public class InquirySearchResponse
    {
        public List<Advertiser> advertisers { get; set; }
        public InquirySearchResponse()
        {
            advertisers = new List<Advertiser>();
        }
        public InquirySearchResponse(string xmlData)
        {
            XDocument _resource = XDocument.Parse(xmlData);
            var ds = _resource.Element("inquiryBatch");
            advertisers = new List<Advertiser>();
            var advertisersElm = ds.Element("advertisers");
            if (advertisersElm != null && advertisersElm.HasElements && advertisersElm.Descendants("advertiser").Count() > 0)
                foreach (var advertiser in advertisersElm.Descendants("advertiser"))
                {
                    advertisers.Add(new Advertiser(advertiser));
                }
        }
    }

    public class RentalAgreement
    {
        public string agreementText { get; set; }
        public string externalId { get; set; }
        public string url { get; set; }
        public RentalAgreement()
        {
        }
        public bool HasValue()
        {
            return !string.IsNullOrEmpty(agreementText) || !string.IsNullOrEmpty(externalId) || !string.IsNullOrEmpty(url);
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("rentalAgreement");
            if (!string.IsNullOrEmpty(agreementText))
                elm.Add(new XElement("agreementText", agreementText));
            if (!string.IsNullOrEmpty(externalId))
                elm.Add(new XElement("externalId", externalId));
            if (!string.IsNullOrEmpty(url))
                elm.Add(new XElement("url", url));
            return elm;
        }
    }
    public class Advertiser
    {
        public string assignedId { get; set; }
        public List<ListingContentIndexEntry> listingContentIndexEntry { get; set; }
        public List<ContactContentIndexEntry> contactContentIndexEntry { get; set; }
        public LodgingConfigurationDefaults LodgingConfigurationDefaults { get; set; }

        public List<LodgingConfigurationContentIndexEntry> lodgingConfigurationContentIndexEntry { get; set; }



        public List<BookingContentIndexEntry> bookingContentIndexEntryList { get; set; }

        public List<Inquirer> inquirers { get; set; }
        public List<Inquiry> inquiries { get; set; }
        public Advertiser()
        {
            listingContentIndexEntry = new List<ListingContentIndexEntry>();
            contactContentIndexEntry = new List<ContactContentIndexEntry>();
            bookingContentIndexEntryList = new List<BookingContentIndexEntry>();
            lodgingConfigurationContentIndexEntry = new List<LodgingConfigurationContentIndexEntry>();
            inquirers = new List<Inquirer>();
            inquiries = new List<Inquiry>();
        }
        public Advertiser(XElement elm)
        {
            listingContentIndexEntry = new List<ListingContentIndexEntry>();
            contactContentIndexEntry = new List<ContactContentIndexEntry>();
            bookingContentIndexEntryList = new List<BookingContentIndexEntry>();
            inquirers = new List<Inquirer>();
            inquiries = new List<Inquiry>();
            if (elm != null)
            {
                assignedId = (string)elm.Element("assignedId") ?? "";
                var inquirersElm = elm.Element("inquirers");
                if (inquirersElm != null && inquirersElm.HasElements && inquirersElm.Descendants("inquirer").Count() > 0)
                    foreach (var inquirer in inquirersElm.Descendants("inquirer"))
                    {
                        inquirers.Add(new Inquirer(inquirer));
                    }
                var inquiriesElm = elm.Element("inquiries");
                if (inquiriesElm != null && inquiriesElm.HasElements && inquiriesElm.Descendants("inquiry").Count() > 0)
                    foreach (var inquiry in inquiriesElm.Descendants("inquiry"))
                    {
                        inquiries.Add(new Inquiry(inquiry));
                    }
            }
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("advertiser");
            elm.Add(new XElement("assignedId", assignedId));
            foreach (var tmp in bookingContentIndexEntryList)
                elm.Add(tmp.GetElement());
            foreach (var tmp in listingContentIndexEntry)
                elm.Add(tmp.GetElement());
            foreach (var tmp in contactContentIndexEntry)
                elm.Add(tmp.GetElement());
            if (LodgingConfigurationDefaults != null)
            {
                elm.Add(LodgingConfigurationDefaults.GetElement());
            }
            foreach (var tmp in lodgingConfigurationContentIndexEntry)
            { elm.Add(tmp.GetElement()); }
            return elm;
        }
    }
    public class ListingContentIndexEntry
    {
        public string listingExternalId { get; set; }
        public bool active { get; set; }
        public string listingUrl { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }
        public ListingContentIndexEntry()
        {
            active = true;
            lastUpdatedDate = new DateAndTime();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("listingContentIndexEntry");
            if (!string.IsNullOrEmpty(listingExternalId))
                elm.Add(new XElement("listingExternalId", listingExternalId));
            elm.Add(new XElement("active", active));
            if (lastUpdatedDate.Value != "")
                elm.Add(lastUpdatedDate.GetElement("lastUpdatedDate"));
            if (!string.IsNullOrEmpty(listingUrl))
                elm.Add(new XElement("listingUrl", listingUrl));
            return elm;
        }
    }
    public class ContactContentIndexEntry
    {
        public string listingExternalId { get; set; }
        public string contactContentUrl { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }
        public ContactContentIndexEntry()
        {
            lastUpdatedDate = new DateAndTime();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("contactContentIndexEntry");
            if (!string.IsNullOrEmpty(listingExternalId))
                elm.Add(new XElement("listingExternalId", listingExternalId));
            if (lastUpdatedDate.Value != "")
                elm.Add(lastUpdatedDate.GetElement("lastUpdatedDate"));
            if (!string.IsNullOrEmpty(contactContentUrl))
                elm.Add(new XElement("contactContentUrl", contactContentUrl));
            return elm;
        }
    }
    public class AdvertiserContentIndexEntry
    {

        public string advertiserAssignedId { get; set; }
        public string advertiserName { get; set; }
        public string advertiserContactContentIndexUrl { get; set; }
        public string advertiserListingContentIndexUrl { get; set; }
        public string advertiserLodgingConfigurationContentIndexUrl { get; set; }
        public string advertiserLodgingRateContentIndexUrl { get; set; }
        public string advertiserUnitAvailabilityContentIndexUrl { get; set; }



        public XElement GetElement()
        {
            XElement elm = new XElement("AdvertiserContentIndexEntry");

            elm.Add(new XElement("advertiserAssignedId", advertiserAssignedId));
            elm.Add(new XElement("advertiserName", advertiserName));
            elm.Add(new XElement("advertiserContactContentIndexUrl", advertiserContactContentIndexUrl));
            elm.Add(new XElement("advertiserListingContentIndexUrl", advertiserListingContentIndexUrl));
            elm.Add(new XElement("advertiserLodgingConfigurationContentIndexUrl", advertiserLodgingConfigurationContentIndexUrl));
            elm.Add(new XElement("advertiserLodgingRateContentIndexUrl", advertiserLodgingRateContentIndexUrl));
            elm.Add(new XElement("advertiserUnitAvailabilityContentIndexUrl", advertiserUnitAvailabilityContentIndexUrl));


            return elm;
        }
    }
    public class AdvertiserRate
    {
        public string assignedId { get; set; }
        public List<LodgingRateContentIndexEntry> listingRateContentEntry { get; set; }
        public AdvertiserRate()
        {
            listingRateContentEntry = new List<LodgingRateContentIndexEntry>();
        }

        public XElement GetElement()
        {
            XElement elm = new XElement("advertiser");
            elm.Add(new XElement("assignedId", assignedId));
            foreach (var tmp in listingRateContentEntry)
                elm.Add(tmp.GetElement());
            return elm;
        }
    }
    public class AdvertiserAvailability
    {
        public string assignedId { get; set; }
        public List<UnitAvailabilityContentIndexEntry> listingAvailabilityContentEntry { get; set; }
        public AdvertiserAvailability()
        {
            listingAvailabilityContentEntry = new List<UnitAvailabilityContentIndexEntry>();
        }

        public XElement GetElement()
        {
            XElement elm = new XElement("advertiser");
            elm.Add(new XElement("assignedId", assignedId));
            foreach (var tmp in listingAvailabilityContentEntry)
            { elm.Add(tmp.GetElement()); }
            return elm;
        }
    }
    public class LodgingRateContentIndexEntry
    {
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public string lodgingRateContentUrl { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }
        public LodgingRateContentIndexEntry()
        {

            lastUpdatedDate = new DateAndTime();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("lodgingRateContentIndexEntry");
            if (!string.IsNullOrEmpty(listingExternalId))
            { elm.Add(new XElement("listingExternalId", listingExternalId)); }

            if (!string.IsNullOrEmpty(unitExternalId))
            { elm.Add(new XElement("unitExternalId", unitExternalId)); }

            if (lastUpdatedDate.Value != "")
            { elm.Add(lastUpdatedDate.GetElement("lastUpdatedDate")); }
            if (!string.IsNullOrEmpty(lodgingRateContentUrl))
            { elm.Add(new XElement("lodgingRateContentUrl", lodgingRateContentUrl)); }
            return elm;
        }
    }

    public class UnitAvailabilityContentIndexEntry
    {
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public string unitAvailabilityContentUrl { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }

        public UnitAvailabilityContentIndexEntry()
        {
            lastUpdatedDate = new DateAndTime();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("unitAvailabilityContentIndexEntry");
            if (!string.IsNullOrEmpty(listingExternalId))
            { elm.Add(new XElement("listingExternalId", listingExternalId)); }

            if (!string.IsNullOrEmpty(unitExternalId))
            { elm.Add(new XElement("unitExternalId", unitExternalId)); }

            if (lastUpdatedDate.Value != "")
            { elm.Add(lastUpdatedDate.GetElement("lastUpdatedDate")); }
            if (!string.IsNullOrEmpty(unitAvailabilityContentUrl))
            { elm.Add(new XElement("unitAvailabilityContentUrl", unitAvailabilityContentUrl)); }
            return elm;
        }
    }

    public class BookingContentIndexEntry
    {

        public string bookingUpdateUrl { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }
        public BookingContentIndexEntry()
        {

            lastUpdatedDate = new DateAndTime();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("bookingContentIndexEntry");

            if (!string.IsNullOrEmpty(bookingUpdateUrl))
                elm.Add(new XElement("bookingUpdateUrl", bookingUpdateUrl));
            if (lastUpdatedDate.Value != "")
                elm.Add(lastUpdatedDate.GetElement("lastUpdatedDate"));
            return elm;
        }
    }
    public class Inquiry
    {
        public string inquiryId { get; set; }
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public string propertyId { get; set; }
        public DateAndTime updatedDate { get; set; }
        public DateAndTime submittedDate { get; set; }
        public string listingChannel { get; set; }
        public string message { get; set; }
        public Inquirer inquirer { get; set; }
        public Reservation reservation { get; set; }
        public Inquiry()
        {
            updatedDate = new DateAndTime();
            submittedDate = new DateAndTime();
            inquirer = new Inquirer();
            reservation = new Reservation();
        }
        public Inquiry(XElement elm)
        {
            updatedDate = new DateAndTime();
            submittedDate = new DateAndTime();
            inquirer = new Inquirer();
            reservation = new Reservation();
            if (elm != null)
            {
                if (elm.Descendants("inquiryId").FirstOrDefault() != null)
                    inquiryId = (string)elm.Descendants("inquiryId").FirstOrDefault().Value;

                if (elm.Descendants("listingExternalId").FirstOrDefault() != null)
                    listingExternalId = (string)elm.Descendants("listingExternalId").FirstOrDefault().Value;

                if (elm.Descendants("unitExternalId").FirstOrDefault() != null)
                    unitExternalId = (string)elm.Descendants("unitExternalId").FirstOrDefault().Value;

                if (elm.Descendants("propertyId").FirstOrDefault() != null)
                    propertyId = (string)elm.Descendants("propertyId").FirstOrDefault().Value;

                if (elm.Descendants("updatedDate").FirstOrDefault() != null)
                    updatedDate.Value = (string)elm.Descendants("updatedDate").FirstOrDefault().Value;

                if (elm.Descendants("submittedDate").FirstOrDefault() != null)
                    submittedDate.Value = (string)elm.Descendants("submittedDate").FirstOrDefault().Value;

                if (elm.Descendants("listingChannel").FirstOrDefault() != null)
                    listingChannel = (string)elm.Descendants("listingChannel").FirstOrDefault().Value;

                if (elm.Descendants("message").FirstOrDefault() != null)
                    message = (string)elm.Descendants("message").FirstOrDefault().Value;

                inquirer = new Inquirer(elm.Element("inquirer"));
                reservation = new Reservation(elm.Element("reservation"));
            }
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
        public Address address { get; set; }
        public Inquirer()
        {
            address = new Address();
        }
        public Inquirer(XElement elm)
        {
            address = new Address();
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

                address = new Address(elm.Element("address"));
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
            elm.Add(address.GetElement());
            return elm;
        }
    }
    public class InquirerUpdate
    {
        public string locale { get; set; }

        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public string phoneCountryCode { get; set; }
        public string phoneNumber { get; set; }
        public string phoneExt { get; set; }
        public Address address { get; set; }
        public InquirerUpdate()
        {
            address = new Address();
        }
        public InquirerUpdate(XElement elm)
        {
            address = new Address();
            if (elm != null)
            {
                if (elm.Attributes("locale").FirstOrDefault() != null)
                    locale = elm.Attributes("locale").FirstOrDefault().Value;

                if (elm.Descendants("title").FirstOrDefault() != null)
                    title = (string)elm.Descendants(title).FirstOrDefault().Value;

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

                address = new Address(elm.Element("address"));
            }
        }

        public XElement GetElement()
        {
            XElement elm = new XElement("inquirer");
            if (!string.IsNullOrEmpty(locale))
                elm.Add(new XElement("locale", locale));
            if (!string.IsNullOrEmpty(title))
                elm.Add(new XElement("title", title));
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
            elm.Add(address.GetElement());
            return elm;
        }
    }
    public class Address
    {
        public string _rel { get; set; } // HOME, WORK, SHIPPING, BILLING, PRIMARY, SECONDARY, OTHER
        public string additionalAddressLine1 { get; set; } // An additional line of address data. This is to support international addresses ex: PO Box EE 16723
        public string addressLine1 { get; set; } // Line one of the street data
        public string addressLine2 { get; set; } // Line two of the street data
        public string addressLine3 { get; set; } // In the US, this is the city
        public string addressLine4 { get; set; } // In the US, this is the state
        public string addressLine5 { get; set; } // In the US, this is the county
        public string country { get; set; } // The two-digit country code
        public string postalCode { get; set; } // The postal code of the address
        public Address()
        {
        }
        public Address(XElement address)
        {
            if (address != null)
            {
                _rel = (string)address.Attribute("rel") ?? "";
                addressLine1 = (string)address.Element("addressLine1") ?? "";
                addressLine2 = (string)address.Element("addressLine2") ?? "";
                additionalAddressLine1 = (string)address.Element("additionalAddressLine1") ?? "";
                addressLine3 = (string)address.Element("addressLine3") ?? "";
                addressLine4 = (string)address.Element("addressLine4") ?? "";
                addressLine5 = (string)address.Element("addressLine5") ?? "";
                country = (string)address.Element("country") ?? "";
                postalCode = (string)address.Element("postalCode") ?? "";
            }
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("address");
            if (!string.IsNullOrEmpty(_rel))
                elm.Add(new XAttribute("rel", _rel));
            if (!string.IsNullOrEmpty(additionalAddressLine1))
                elm.Add(new XElement("additionalAddressLine1", additionalAddressLine1));
            if (!string.IsNullOrEmpty(addressLine1))
                elm.Add(new XElement("addressLine1", addressLine1));
            if (!string.IsNullOrEmpty(addressLine2))
                elm.Add(new XElement("addressLine2", addressLine2));
            if (!string.IsNullOrEmpty(addressLine3))
                elm.Add(new XElement("addressLine3", addressLine3));
            if (!string.IsNullOrEmpty(addressLine4))
                elm.Add(new XElement("addressLine4", addressLine4));
            if (!string.IsNullOrEmpty(addressLine5))
                elm.Add(new XElement("addressLine5", addressLine5));
            if (!string.IsNullOrEmpty(country))
                elm.Add(new XElement("country", country));
            if (!string.IsNullOrEmpty(postalCode))
                elm.Add(new XElement("postalCode", postalCode));
            return elm;
        }
    }
    public class Reservation
    {
        public int numberOfAdults { get; set; }
        public int numberOfChildren { get; set; }
        public int numberOfPets { get; set; }
        public DateRange reservationDates { get; set; }
        public int? checkinTime { get; set; } // This should be an integer based on a 24 hour cycle. IE 3:00 PM should be 15.
        public int? checkoutTime { get; set; } // This should be an integer based on a 24 hour cycle. IE 3:00 PM should be 15.
        public string reservationOriginationDate { get; set; } // For offline booking, the date and time the reservation was created in the external system.
        public Reservation()
        {
            reservationDates = new DateRange();
        }
        public Reservation(XElement reservationElm)
        {
            reservationDates = new DateRange();
            if (reservationElm.Element("numberOfAdults") != null)
                numberOfAdults = reservationElm.Element("numberOfAdults").Value.ToInt32();
            if (reservationElm.Element("numberOfChildren") != null)
                numberOfChildren = reservationElm.Element("numberOfChildren").Value.ToInt32();
            if (reservationElm.Element("numberOfPets") != null)
                numberOfPets = reservationElm.Element("numberOfPets").Value.ToInt32();
            if (reservationElm.Element("checkinTime") != null)
                checkinTime = reservationElm.Element("checkinTime").Value.ToInt32();
            if (reservationElm.Element("checkoutTime") != null)
                checkoutTime = reservationElm.Element("checkoutTime").Value.ToInt32();
            if (reservationElm.Element("reservationOriginationDate") != null)
                reservationOriginationDate = reservationElm.Element("reservationOriginationDate").Value;
            if (reservationElm.Element("reservationDates") != null)
            {
                if (reservationElm.Element("reservationDates").Element("beginDate") != null)
                    reservationDates.beginDate.Value = reservationElm.Element("reservationDates").Element("beginDate").Value;
                if (reservationElm.Element("reservationDates").Element("endDate") != null)
                    reservationDates.endDate.Value = reservationElm.Element("reservationDates").Element("endDate").Value;
            }
        }
        public XElement GetElement()
        {
            XElement Elm = new XElement("reservation");
            Elm.Add(new XElement("numberOfAdults", numberOfAdults));
            Elm.Add(new XElement("numberOfChildren", numberOfChildren));
            Elm.Add(new XElement("numberOfPets", numberOfPets));
            Elm.Add(reservationDates.GetElement("reservationDates"));
            if (checkinTime.HasValue)
                Elm.Add(new XElement("checkinTime", checkinTime));
            if (checkoutTime.HasValue)
                Elm.Add(new XElement("checkoutTime", checkoutTime));
            if (!string.IsNullOrEmpty(reservationOriginationDate))
                Elm.Add(new XElement("reservationOriginationDate", reservationOriginationDate));
            return Elm;
        }
    }
    public class DateRange
    {
        public Date beginDate { get; set; }
        public Date endDate { get; set; }
        public DateRange()
        {
            beginDate = new Date();
            endDate = new Date();
        }
        public XElement GetElement(string name)
        {
            XElement elm = new XElement(name);
            elm.Add(new XElement("beginDate", beginDate.Value));
            elm.Add(new XElement("endDate", endDate.Value));
            return elm;
        }
    }
    public class Order
    {
        public string bodyText { get; set; }
        public string currency { get; set; } // EUR, GBP, USD
        public string externalId { get; set; }
        public List<OrderItem> orderItemList { get; set; }
        public PaymentSchedule paymentSchedule { get; set; }
        public ReservationCancellationPolicy reservationCancellationPolicy { get; set; }
        public List<StayFees> stayFees { get; set; }
        public Order()
        {
            orderItemList = new List<OrderItem>();
            paymentSchedule = new PaymentSchedule();
            reservationCancellationPolicy = new ReservationCancellationPolicy();
            stayFees = new List<StayFees>();
        }
        public XElement GetElement()
        {
            XElement orderElm = new XElement("order");
            if (!string.IsNullOrEmpty(bodyText))
                orderElm.Add(new XElement("bodyText", bodyText));
            if (!string.IsNullOrEmpty(currency))
                orderElm.Add(new XElement("currency", currency));
            if (!string.IsNullOrEmpty(externalId))
                orderElm.Add(new XElement("externalId", externalId));

            if (orderItemList.Count > 0)
            {
                XElement orderItemListElm = new XElement("orderItemList");
                foreach (OrderItem orderItem in orderItemList)
                {
                    orderItemListElm.Add(orderItem.GetElement());
                }
                orderElm.Add(orderItemListElm);
            }

            XElement paymentScheduleElm = paymentSchedule.GetElement();
            if (paymentScheduleElm != null)
                orderElm.Add(paymentScheduleElm);

            XElement reservationCancellationPolicyElm = reservationCancellationPolicy.GetElement();
            if (reservationCancellationPolicyElm != null)
                orderElm.Add(reservationCancellationPolicyElm);

            #region stayFees
            if (stayFees.Where(x => !string.IsNullOrEmpty(x.description)).Count() > 0)
            {
                XElement stayFeesElm = new XElement("stayFees");
                foreach (StayFees stayFee in stayFees)
                {
                    XElement stayFeeElm = new XElement("stayFee");
                    if (!string.IsNullOrEmpty(stayFee.description))
                        stayFeeElm.Add(new XElement("description", stayFee.description));
                    stayFeesElm.Add(stayFeeElm);
                }
                orderElm.Add(stayFeesElm);
            }
            #endregion
            return orderElm;
        }
    }
    public class OrderUpdate
    {
        public string bodyText { get; set; }
        public string currency { get; set; } // EUR, GBP, USD
        public string externalId { get; set; }
        public List<OrderItem> orderItemList { get; set; }
        public PaymentSchedule paymentSchedule { get; set; }
        //public ReservationCancellationPolicyUpdate reservationCancellationPolicy { get; set; }
        public List<StayFees> stayFees { get; set; }
        public OrderUpdate()
        {
            orderItemList = new List<OrderItem>();
            paymentSchedule = new PaymentSchedule();
            // reservationCancellationPolicy = new ReservationCancellationPolicyUpdate();
            stayFees = new List<StayFees>();
        }
        public XElement GetElement()
        {
            XElement orderElm = new XElement("order");
            if (!string.IsNullOrEmpty(bodyText))
                orderElm.Add(new XElement("bodyText", bodyText));
            if (!string.IsNullOrEmpty(currency))
                orderElm.Add(new XElement("currency", currency));
            if (!string.IsNullOrEmpty(externalId))
                orderElm.Add(new XElement("externalId", externalId));

            if (orderItemList.Count > 0)
            {
                XElement orderItemListElm = new XElement("orderItemList");
                foreach (OrderItem orderItem in orderItemList)
                {
                    orderItemListElm.Add(orderItem.GetElement());
                }
                orderElm.Add(orderItemListElm);
            }

            XElement paymentScheduleElm = paymentSchedule.GetElement();
            if (paymentScheduleElm != null)
                orderElm.Add(paymentScheduleElm);



            #region stayFees
            //if (stayFees.Where(x => !string.IsNullOrEmpty(x.description)).Count() > 0)
            //{
            XElement stayFeesElm = new XElement("stayFees");
            foreach (StayFees stayFee in stayFees)
            {
                XElement stayFeeElm = new XElement("stayFee");
                if (!string.IsNullOrEmpty(stayFee.description))
                    stayFeeElm.Add(new XElement("description", stayFee.description));
                stayFeesElm.Add(stayFeeElm);
            }
            orderElm.Add(stayFeesElm);
            // }
            #endregion
            return orderElm;
        }
    }
    public class OrderItem
    {
        public string description { get; set; }
        public string externalId { get; set; }
        public string feeType { get; set; } // ACTIVITY, DEPOSIT, EQUIPMENT, MISC, PROTECTION, RENTAL, TAX, DISCOUNT
        public string name { get; set; } // max 100 chars
        public Amount preTaxAmount { get; set; } // The pre tax amount associated with this line item including the currency.
        public string status { get; set; } // PENDING, ACCEPTED, DECLINED, DECLINED_BY_SYSTEM, CANCELLED, EDITED
        public string taxRate { get; set; } // The tax rate applied to this line item
        public Amount totalAmount { get; set; } // The total amount associated with this line item including the currency
        public OrderItem()
        {
            preTaxAmount = new Amount();
            totalAmount = new Amount();
        }
        public OrderItem(XElement elm)
        {
            preTaxAmount = new Amount();
            totalAmount = new Amount();
            if (elm != null)
            {
                description = (string)elm.Element("description") ?? "";
                externalId = (string)elm.Element("externalId") ?? "";
                feeType = (string)elm.Element("feeType") ?? "";
                name = (string)elm.Element("name") ?? "";
                preTaxAmount = new Amount(elm.Element("preTaxAmount"));
                status = (string)elm.Element("status") ?? "";
                taxRate = (string)elm.Element("taxRate") ?? "";
                totalAmount = new Amount(elm.Element("totalAmount"));
            }
        }
        public XElement GetElement()
        {
            XElement orderItemElm = new XElement("orderItem");
            if (!string.IsNullOrEmpty(description))
                orderItemElm.Add(new XElement("description", description));
            if (!string.IsNullOrEmpty(externalId))
                orderItemElm.Add(new XElement("externalId", externalId));
            if (!string.IsNullOrEmpty(feeType))
                orderItemElm.Add(new XElement("feeType", feeType));
            if (!string.IsNullOrEmpty(name))
                orderItemElm.Add(new XElement("name", name));
            if (preTaxAmount.Value != "")
                orderItemElm.Add(preTaxAmount.GetElement("preTaxAmount"));
            if (!string.IsNullOrEmpty(status))
                orderItemElm.Add(new XElement("status", status));
            if (!string.IsNullOrEmpty(taxRate))
                orderItemElm.Add(new XElement("taxRate", taxRate));
            if (totalAmount.Value != "")
                orderItemElm.Add(totalAmount.GetElement("totalAmount"));
            return orderItemElm;
        }
    }
    public class PaymentSchedule
    {
        public List<AcceptedPaymentForm> acceptedPaymentForms { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public List<PaymentScheduleItem> paymentScheduleItemList { get; set; }
        public PaymentSchedule()
        {
            acceptedPaymentForms = new List<AcceptedPaymentForm>();
            paymentScheduleItemList = new List<PaymentScheduleItem>();
            using (magaChnlHomeAwayDataContext dcChnl = maga_DataContext.DC_HOME)
            {
                var list = dcChnl.RntChnlHomeAwayAcceptedPaymentFormTBL.Where(x => x.isActive == 1).ToList();
                foreach (var tmpTbl in list)
                {
                    var tmp = new AcceptedPaymentForm();
                    tmp.paymentFormType = tmpTbl.paymentFormType;
                    tmp.cardCode = tmpTbl.cardCode;
                    tmp.cardType = tmpTbl.cardType;
                    acceptedPaymentForms.Add(tmp);
                }
            }
        }
        public XElement GetElement()
        {
            XElement paymentScheduleElm = new XElement("paymentSchedule");
            if (!string.IsNullOrEmpty(description))
                paymentScheduleElm.Add(new XElement("description", description));
            if (!string.IsNullOrEmpty(name))
                paymentScheduleElm.Add(new XElement("name", name));
            if (acceptedPaymentForms.Count > 0)
            {
                XElement acceptedPaymentFormsElm = new XElement("acceptedPaymentForms");
                foreach (AcceptedPaymentForm acceptedPaymentForm in acceptedPaymentForms)
                {
                    if (acceptedPaymentForm.paymentFormType == "CARD")
                    {
                        XElement paymentCardDescriptorElm = new XElement("paymentCardDescriptor");
                        paymentCardDescriptorElm.Add(new XElement("paymentFormType", "CARD"));
                        paymentCardDescriptorElm.Add(new XElement("cardCode", acceptedPaymentForm.cardCode));
                        paymentCardDescriptorElm.Add(new XElement("cardType", acceptedPaymentForm.cardType));
                        acceptedPaymentFormsElm.Add(paymentCardDescriptorElm);
                    }
                    if (acceptedPaymentForm.paymentFormType == "INVOICE")
                    {
                        XElement paymentCardDescriptorElm = new XElement("paymentInvoiceDescriptor");
                        paymentCardDescriptorElm.Add(new XElement("paymentFormType", "INVOICE"));
                        paymentCardDescriptorElm.Add(new XElement("paymentNote", ""));

                        acceptedPaymentFormsElm.Add(paymentCardDescriptorElm);
                    }
                    // Removed on 30 June 2016

                    //if (acceptedPaymentForm.paymentFormType == "CHECK")
                    //{
                    //    XElement paymentCardDescriptorElm = new XElement("paymentCheckDescriptor");
                    //    paymentCardDescriptorElm.Add(new XElement("paymentFormType", "CHECK"));
                    //    acceptedPaymentFormsElm.Add(paymentCardDescriptorElm);
                    //}
                    //if (acceptedPaymentForm.paymentFormType == "ECHECK")
                    //{
                    //    XElement paymentCardDescriptorElm = new XElement("paymentECheckDescriptor");
                    //    paymentCardDescriptorElm.Add(new XElement("paymentFormType", "ECHECK"));
                    //    acceptedPaymentFormsElm.Add(paymentCardDescriptorElm);
                    //}
                    //if (acceptedPaymentForm.paymentFormType == "INVOICE")
                    //{
                    //    XElement paymentCardDescriptorElm = new XElement("paymentInvoiceDescriptor");
                    //    paymentCardDescriptorElm.Add(new XElement("paymentFormType", "INVOICE"));
                    //    acceptedPaymentFormsElm.Add(paymentCardDescriptorElm);
                    //}
                }
                paymentScheduleElm.Add(acceptedPaymentFormsElm);
            }
            if (paymentScheduleItemList.Count > 0)
            {
                XElement paymentScheduleItemListElm = new XElement("paymentScheduleItemList");
                foreach (PaymentScheduleItem paymentScheduleItem in paymentScheduleItemList)
                {
                    XElement paymentScheduleItemElm = new XElement("paymentScheduleItem");
                    if (paymentScheduleItem.amount.Value != "")
                        paymentScheduleItemElm.Add(paymentScheduleItem.amount.GetElement("amount"));
                    if (!string.IsNullOrEmpty(paymentScheduleItem.description))
                        paymentScheduleItemElm.Add(new XElement("description", paymentScheduleItem.description));
                    if (!string.IsNullOrEmpty(paymentScheduleItem.dueDate.Value))
                        paymentScheduleItemElm.Add(new XElement("dueDate", paymentScheduleItem.dueDate.Value));
                    if (!string.IsNullOrEmpty(paymentScheduleItem.externalId))
                        paymentScheduleItemElm.Add(new XElement("externalId", paymentScheduleItem.externalId));
                    if (paymentScheduleItem.refundable.HasValue)
                        paymentScheduleItemElm.Add(new XElement("refundable", paymentScheduleItem.refundable));
                    if (!string.IsNullOrEmpty(paymentScheduleItem.refundDescription))
                        paymentScheduleItemElm.Add(new XElement("refundDescription", paymentScheduleItem.refundDescription));
                    if (paymentScheduleItem.refundPercent.HasValue)
                        paymentScheduleItemElm.Add(new XElement("refundPercent", paymentScheduleItem.refundPercent));
                    paymentScheduleItemListElm.Add(paymentScheduleItemElm);
                }
                paymentScheduleElm.Add(paymentScheduleItemListElm);
            }
            return !paymentScheduleElm.HasElements ? (XElement)null : paymentScheduleElm;
        }
    }
    public class AcceptedPaymentForm
    {
        public string paymentFormType { get; set; } // CARD, CHECK, ECHECK, INVOICE
        public string cardCode { get; set; } // VISA, MASTERCARD, AMEX, DISCOVER, DINERS, CARTE_BLANCHE, JCB, ENROUTE, JAL, MAESTRO_UK
        public string cardType { get; set; } // CREDIT, DEBIT
        public AcceptedPaymentForm()
        {
        }
        public AcceptedPaymentForm(XElement elm)
        {
            if (elm != null)
            {
                paymentFormType = (string)elm.Element("paymentFormType") ?? "";
                cardCode = (string)elm.Element("cardCode") ?? "";
                cardType = (string)elm.Element("cardType") ?? "";
            }
        }
    }
    public class PaymentScheduleItem
    {
        public Amount amount { get; set; }
        public string description { get; set; }
        public Date dueDate { get; set; } // The due date associated with this payment schedule item
        public string externalId { get; set; }
        public bool? refundable { get; set; }
        public string refundDescription { get; set; }
        public int? refundPercent { get; set; }
        public PaymentScheduleItem()
        {
            amount = new Amount();
            dueDate = new Date();
        }
    }
    public class ReservationCancellationPolicy
    {
        public List<CancellationPolicyItem> cancellationPolicyItemList { get; set; }
        public string description { get; set; }
        public ReservationCancellationPolicy()
        {
            cancellationPolicyItemList = new List<CancellationPolicyItem>();
        }
        public XElement GetElement()
        {
            XElement reservationCancellationPolicyElm = new XElement("reservationCancellationPolicy");
            if (cancellationPolicyItemList.Count > 0)
            {
                XElement cancellationPolicyItemListElm = new XElement("cancellationPolicyItemList");
                foreach (CancellationPolicyItem cancellationPolicyItem in cancellationPolicyItemList)
                {
                    XElement cancellationPolicyItemElm = new XElement("cancellationPolicyItem");
                    if (cancellationPolicyItem.amount.Value != "")
                        cancellationPolicyItemElm.Add(cancellationPolicyItem.amount.GetElement("amount"));
                    if (!string.IsNullOrEmpty(cancellationPolicyItem.deadline.Value))
                        cancellationPolicyItemElm.Add(new XElement("deadline", cancellationPolicyItem.deadline.Value));
                    if (!string.IsNullOrEmpty(cancellationPolicyItem.externalId))
                        cancellationPolicyItemElm.Add(new XElement("externalId", cancellationPolicyItem.externalId));
                    if (!string.IsNullOrEmpty(cancellationPolicyItem.penaltyType))
                        cancellationPolicyItemElm.Add(new XElement("penaltyType", cancellationPolicyItem.penaltyType));
                    if (cancellationPolicyItem.percentPenalty.HasValue)
                        cancellationPolicyItemElm.Add(new XElement("percentPenalty", cancellationPolicyItem.percentPenalty));
                    cancellationPolicyItemListElm.Add(cancellationPolicyItemElm);
                }
                // reservationCancellationPolicyElm.Add(cancellationPolicyItemListElm);
            }
            if (!string.IsNullOrEmpty(description))
                reservationCancellationPolicyElm.Add(new XElement("description", description));
            return !reservationCancellationPolicyElm.HasElements ? (XElement)null : reservationCancellationPolicyElm;
        }
    }
    public class CancellationPolicyItem
    {
        public Amount amount { get; set; }
        public Date deadline { get; set; } // The deadline for this particular cancellation policy line item
        public string externalId { get; set; }
        public string penaltyType { get; set; } // PERCENT_ORDER, PERCENT_LINE_ITEMS, CHARGE
        public int? percentPenalty { get; set; }
        public CancellationPolicyItem()
        {
            amount = new Amount();
            deadline = new Date();
        }
    }
    public class StayFees
    {
        public string description { get; set; } // max 1000
        public StayFees()
        {
        }
    }
    public class PaymentForm
    {
        public PaymentCard paymentCard { get; set; }
        public PaymentForm()
        {
            paymentCard = new PaymentCard();
        }
        public PaymentForm(XElement elm)
        {
            paymentCard = new PaymentCard();
            if (elm != null)
            {
                paymentCard = new PaymentCard(elm.Element("paymentCard"));
            }
        }

    }
    public class PaymentCard
    {
        public string paymentFormType { get; set; } // CARD, CHECK, ECHECK, DEFERRED
        public Address billingAddress { get; set; }
        public string cvv { get; set; }
        public string expiration { get; set; } // MM/YYYY
        public string maskedNumber { get; set; } // Credit card number with the last 4 digits masked.
        public string nameOnCard { get; set; } // The person’s name on the credit card.
        public string number { get; set; } // The full credit card number.
        public string numberToken { get; set; } // The HomeAway token representing the credit card number.
        public AcceptedPaymentForm paymentCardDescriptor { get; set; }
        public PaymentCard()
        {
            billingAddress = new Address();
            paymentCardDescriptor = new AcceptedPaymentForm();
        }
        public PaymentCard(XElement elm)
        {
            billingAddress = new Address();
            paymentCardDescriptor = new AcceptedPaymentForm();
            if (elm != null)
            {
                paymentFormType = (string)elm.Element("paymentFormType") ?? "";
                billingAddress = new Address(elm.Element("billingAddress"));
                cvv = (string)elm.Element("cvv") ?? "";
                expiration = (string)elm.Element("expiration") ?? "";
                maskedNumber = (string)elm.Element("maskedNumber") ?? "";
                nameOnCard = (string)elm.Element("nameOnCard") ?? "";
                number = (string)elm.Element("number") ?? "";
                numberToken = (string)elm.Element("numberToken") ?? "";
                paymentCardDescriptor = new AcceptedPaymentForm(elm.Element("paymentCardDescriptor"));
            }
        }
    }
    public class Error
    {
        public int? count { get; set; }
        public string dayOfWeek { get; set; } // SUN, MON,TUE,WED, THU, FRI,SAT
        public string errorCode { get; set; } // 
        public string errorType { get; set; } // SEE TYPES
        public string errorMessage { get; set; } //
        public Error()
        {
            /*
            AGE_RESTRICTION
            BILLING_ERROR
            CHANGE_OVER_DAY_MISMATCH
            END_DAY_MISMATCH EXCEEDS_MAX_OCCUPANCY
             * EXCEEDS_MAX_STAY
            INVALID_PAYMENT_METHOD
            MERCHANT_ACCOUNT_ERROR MIN_ADVANCED_NOTICE_NOT_MET
             * MIN_STAY_NOT_MET
            NO_TRAVELER_EMAIL_FOR_INQUIRY
            OLB_PROVIDER_NOT_PROVISIONED
             * OTHER PETS_NOT_ALLOWED
            PROPERTY_NOT_AVAILABLE
            QUOTE_PRICE_MISMATCH
             * SERVER_ERROR
             * SERVICE_UNAVAILABLE
            START_DAY_MISMATCH
            STAY_DATE_RECOMMENDATION
             * STAY_NIGHT_INCREMENT_MISMATCH
             * UNKNOWN_PROPERTY
            */
        }
    }
    public class ErrorJsonReponseAvailabilityRequest
    {
        public string Errorcode { get; set; }

    }
    public class ErrorList
    {
        public string root { get; set; } // max 1000
        public List<Error> errorList { get; set; }
        public ErrorList(string Root)
        {
            root = Root;
            errorList = new List<Error>();
        }
        public void AddError(string ErrorType)
        {
            var error = new Error();
            error.errorType = ErrorType;
            errorList.Add(error);
        }
        private XElement GetElement()
        {
            XElement record = new XElement("errorList");
            foreach (Error error in errorList)
            {
                XElement errorElm = new XElement("error");
                if (error.count.HasValue)
                    errorElm.Add(new XElement("count", error.count));
                if (!string.IsNullOrEmpty(error.dayOfWeek))
                    errorElm.Add(new XElement("dayOfWeek", error.dayOfWeek));
                if (!string.IsNullOrEmpty(error.errorCode))
                    errorElm.Add(new XElement("errorCode", error.errorCode));
                if (!string.IsNullOrEmpty(error.errorType))
                    errorElm.Add(new XElement("errorType", error.errorType));
                if (!string.IsNullOrEmpty(error.errorMessage))
                    errorElm.Add(new XElement("errorMessage", error.errorMessage));
                record.Add(errorElm);
            }
            return record;
        }
        public string GetXml()
        {
            return "<" + root + "><documentVersion>1.2</documentVersion>" + GetElement() + "</" + root + ">";
        }
        public static string GetErrorXml(string ErrorType, string root)
        {
            string documentversion = "1.2";
            XElement record = new XElement("errorList");
            XElement errorElm = new XElement("error");
            errorElm.Add(new XElement("ErrorType", ErrorType));
            record.Add(errorElm);
            return "<" + root + ">" + "<" + documentversion + ">" + "</" + documentversion + ">" + record + "</" + root + ">";
        }
    }
    public class NightlyRateAmount
    {

        public string Value { get { return ValueDecimal.HasValue ? (ValueDecimal + "").Replace(",", ".") : ""; } set { ValueDecimal = (value + "") != "" ? (value + "").Replace(".", ",").ToDecimal() : (decimal?)null; } }
        public decimal? ValueDecimal { get; set; }
        public NightlyRateAmount()
        {

        }
        public NightlyRateAmount(XElement elm)
        {
            if (elm != null)
            {
                Value = elm.Value;
            }
        }
        public XElement GetElement(string name)
        {
            XElement record = new XElement(name);

            record.Value = Value;
            return record;
        }
    }
    public class Amount
    {
        public string Currency { get; set; }
        public string Value { get { return ValueDecimal.HasValue ? (ValueDecimal + "").Replace(",", ".") : ""; } set { ValueDecimal = (value + "") != "" ? (value + "").Replace(",", ".").ToDecimal() : (decimal?)null; } }
        public decimal? ValueDecimal { get; set; }
        public Amount()
        {
            Currency = "EUR";
        }
        public Amount(XElement elm)
        {
            if (elm != null)
            {
                Currency = (string)elm.Attribute("currency") ?? "";
                Value = elm.Value;
            }
        }
        public XElement GetElement(string name)
        {
            XElement record = new XElement(name);
            record.Add(new XAttribute("currency", Currency));
            record.Value = Value;
            return record;
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
        public XElement GetElement(string name)
        {
            XElement record = new XElement(name);
            record.Value = Value;
            return record;
        }
    }

    public class PriceListPerDates
    {
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public int MinNights { get; set; }

        public decimal PriceHA { get; set; }

        public string Name { get; set; }
        public string NightsFrom { get; set; }
        public string NightsTo { get; set; }

        public PriceListPerDates(DateTime dtStart, DateTime dtEnd, int minNights, decimal priceHA)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            MinNights = minNights;
            PriceHA = priceHA;
        }

        #region
        //public bool HasSamePrices(PriceListPerDates compareWith)
        //{
        //    if (MinNights != compareWith.MinNights) return false;
        //    if (Period != compareWith.Period) return false;
        //    if (PromoList != compareWith.PromoList) return false;
        //    if (DtEnd.AddDays(1) != compareWith.DtStart) return false;
        //    if (lstClosedArrival.Count != compareWith.lstClosedArrival.Count) return false;
        //    if (lstClosedDeparture.Count != compareWith.lstClosedDeparture.Count) return false;
        //    for (int i = 0; i < lstClosedArrival.Count; i++)
        //    {
        //        if (lstClosedArrival[i] != compareWith.lstClosedArrival[i]) return false;
        //    }
        //    for (int i = 0; i < lstClosedDeparture.Count; i++)
        //    {
        //        if (lstClosedDeparture[i] != compareWith.lstClosedDeparture[i]) return false;
        //    }
        //    if (Prices.Count != compareWith.Prices.Count) return false;
        //    if (PricesForAgent.Count != compareWith.PricesForAgent.Count) return false;
        //    foreach (var item in Prices)
        //        if (compareWith.Prices[item.Key] == null || compareWith.Prices[item.Key] != item.Value)
        //            return false;
        //    foreach (var item in PricesForAgent)
        //        if (compareWith.PricesForAgent[item.Key] == null || compareWith.PricesForAgent[item.Key] != item.Value)
        //            return false;
        //    return true;
        //} 
        #endregion

        public bool HasSamePricesHA(PriceListPerDates compareWith)
        {
            if (MinNights != compareWith.MinNights) return false;
            if (PriceHA != compareWith.PriceHA) return false;

            if (DtEnd.AddDays(1) != compareWith.DtStart) return false;

            return true;
        }
    }
}
public static class ChnlHomeAwayExts_V411
{
}

