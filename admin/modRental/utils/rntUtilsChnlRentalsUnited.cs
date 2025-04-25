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
using RentalInRome.data;
using Newtonsoft.Json.Converters;
using System.Xml.Linq;
using ModAuth;
using HtmlAgilityPack;


public class ChnlRentalsUnitedUtils
{
    public static bool CheckForService()
    {
        if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Response == null) return false;
        var Request = HttpContext.Current.Request;
        var Response = HttpContext.Current.Response;
        if (!Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/")) return false;
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
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/quoterequest"))
        {
            requestType = "quoterequest";
            responseContent = ChnlRentalsUnitedService.QuoteRequest(xml);
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/bookingrequest"))
        {
            requestType = "bookingrequest";
            responseContent = ChnlRentalsUnitedService.BookingRequest(xml);
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/bookings"))
        {
            requestType = "bookings";
            responseContent = ChnlRentalsUnitedService.BookingContentIndexRequest(xml);
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/booking/"))
        {
            var uid = Request.Url.LocalPath.ToLower().Replace("/chnlutils/rentalsunited/booking/", "");
            var reservationTbl = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.unique_id.ToString().ToLower() == uid);
            if (reservationTbl != null)
            {
                requestType = "booking/" + reservationTbl.id;
                responseContent = ChnlRentalsUnitedService.Booking(reservationTbl.id);
            }
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/listings-rate"))
        {
            requestType = "listings-rate";
            responseContent = ChnlRentalsUnitedService.ListingRateContentRequest();
        }
        else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/listings-availability"))
        {
            requestType = "listings-availability";
            responseContent = ChnlRentalsUnitedService.ListingAvailabilityContentRequest();
        }
        else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/listings"))
        {
            requestType = "listings";
            responseContent = ChnlRentalsUnitedService.ListingContentIndexRequest();
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/listing/"))
        {
            var id = Request.Url.LocalPath.ToLower().Replace("/chnlutils/rentalsunited/listing/", "").ToInt32();
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var chnlEstateTbl = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == id && (x.is_slave == null || x.is_slave == 0));
                var estateTb = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == id);
                if (estateTb != null && chnlEstateTbl != null)
                {
                    requestType = "listing/" + estateTb.id;
                    responseContent = ChnlRentalsUnitedService.Listing(estateTb.id);
                }
            }
        }
        else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/unit-rate/"))
        {
            var id = Request.Url.LocalPath.ToLower().Replace("/chnlutils/rentalsunited/unit-rate/", "").ToInt32();
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var chnlEstateTbl = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == id);
                var estateTb = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == id);
                if (estateTb != null && chnlEstateTbl != null)
                {
                    requestType = "unit-rate/" + estateTb.id;
                    responseContent = ChnlRentalsUnitedService.ListingRate(estateTb.id);
                }
            }
        }
        else if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/rentalsunited/unit-availability/"))
        {
            var id = Request.Url.LocalPath.ToLower().Replace("/chnlutils/rentalsunited/unit-availability/", "").ToInt32();
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var chnlEstateTbl = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == id);
                var estateTb = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == id);
                if (estateTb != null && chnlEstateTbl != null)
                {
                    requestType = "unit-availability/" + estateTb.id;
                    responseContent = ChnlRentalsUnitedService.ListingAvailability(estateTb.id);
                }
            }
        }
        if (CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug").ToInt32() == 1)
            addLog(requestType, requestComments, requestContent, responseContent, requesUrl);
        Response.Write(responseContent);
        Response.End();
        return true;
    }

    public static void SetTimers()
    {
        timerChnlRentalsUnitedImport = new System.Timers.Timer();
        if (CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedImportFirst") == "true")
            timerChnlRentalsUnitedImport.Interval = (1000 * 60 * 10); // first after 2 mins
        else
            timerChnlRentalsUnitedImport.Interval = (1000 * 60 * 2); // first after 2 mins
        timerChnlRentalsUnitedImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlRentalsUnitedImport_ElapsedFirstTime);
        timerChnlRentalsUnitedImport.Start();
        if (CommonUtilities.getSYS_SETTING("is_clear_log") == "true" || CommonUtilities.getSYS_SETTING("is_clear_log") == "1")
        {
            timerClearLog = new System.Timers.Timer();
            timerClearLog.Interval = (1000 * 60 * 5); // first after 5 mins
            timerClearLog.Elapsed += new System.Timers.ElapsedEventHandler(timerClearLog_Elapsed);
            timerClearLog.Start();

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
            var dt = DateTime.Now.AddDays(-4);
            using (DCchnlRentalsUnited dc = new DCchnlRentalsUnited())
            {
                dc.Delete(dc.dbRntChnlRentalsUnitedRequestLOGs.Where(x => x.logDateTime <= dt));
                dc.SaveChanges();
            }
            addLog("CLEAR LOG", "till " + dt, "", "", "");
        }
        public ClearLog_process()
        {
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlRentalsUnitedUtils.ClearLog_process");
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

    static System.Timers.Timer timerChnlRentalsUnitedImport;
    static void timerChnlRentalsUnitedImport_ElapsedFirstTime(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        if (RentalInRome.Global.isOnDev && CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedImportOnDev") != "true" && CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedImportOnDev").ToInt32() != 1) return;
        int rntChnlRentalsUnitedImportEachMins = CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedImportEachMins").ToInt32();
        if (rntChnlRentalsUnitedImportEachMins == 0) rntChnlRentalsUnitedImportEachMins = 15;  // each 15 mins default
        if (CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedImportFirst") == "true")
            ChnlRentalsUnitedImport.Enquiries_all(ChnlRentalsUnitedProps.TimePeriods.PAST_SEVEN_DAYS);
        else
            ChnlRentalsUnitedImport.Enquiries_all(ChnlRentalsUnitedProps.TimePeriods.PAST_TWENTY_FOUR_HOURS);
        timerChnlRentalsUnitedImport.Dispose();
        timerChnlRentalsUnitedImport = new System.Timers.Timer();
        timerChnlRentalsUnitedImport.Interval = (1000 * 60 * rntChnlRentalsUnitedImportEachMins);
        timerChnlRentalsUnitedImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlRentalsUnitedImport_Elapsed);
        timerChnlRentalsUnitedImport.Start();
    }
    static void timerChnlRentalsUnitedImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        if (RentalInRome.Global.isOnDev && CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedImportOnDev") != "true" && CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedImportOnDev").ToInt32() != 1) return;
        int rntChnlRentalsUnitedImportEachMins = CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedImportEachMins").ToInt32();
        if (rntChnlRentalsUnitedImportEachMins == 0) rntChnlRentalsUnitedImportEachMins = 15;  // each 15 mins default
        ChnlRentalsUnitedImport.Enquiries_all(ChnlRentalsUnitedProps.TimePeriods.PAST_THIRTY_MINUTES);
        timerChnlRentalsUnitedImport.Dispose();
        timerChnlRentalsUnitedImport = new System.Timers.Timer();
        timerChnlRentalsUnitedImport.Interval = (1000 * 60 * rntChnlRentalsUnitedImportEachMins);
        timerChnlRentalsUnitedImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlRentalsUnitedImport_Elapsed);
        timerChnlRentalsUnitedImport.Start();
    }
    public static void addLog(string requestType, string requestComments, string requestContent, string responseContent, string requesUrl)
    {
        try
        {
            using (DCchnlRentalsUnited dc = new DCchnlRentalsUnited())
            {
                var item = new dbRntChnlRentalsUnitedRequestLOG();
                item.uid = Guid.NewGuid();
                item.requesUrl = requesUrl;
                item.requestType = requestType;
                item.requestComments = requestComments;
                item.requestContent = requestContent;
                item.responseContent = responseContent;
                item.logDateTime = DateTime.Now;
                dc.Add(item);
                dc.SaveChanges();
            }
        }
        catch (Exception Ex)
        {
            ErrorLog.addLog("", "ChnlRentalsUnitedUtils.addLog", Ex.ToString() + "");
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
            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;  
            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            String strRSString = _Answer.ReadToEnd().ToString();
            if (CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug").ToInt32() == 1)
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

    public static List<dbRntChnlRentalsUnitedEstateRoomTB> getRooms(RNT_TB_ESTATE currEstate, string roomType)
    {
        List<dbRntChnlRentalsUnitedEstateRoomTB> rooms = new List<dbRntChnlRentalsUnitedEstateRoomTB>();
        if (roomType == "Bathroom")
        {
            for (int i = 1; i <= currEstate.num_rooms_bath.objToInt32(); i++)
            {
                var currTbl = new dbRntChnlRentalsUnitedEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "FULL_BATH";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
            if (rooms.Count == 0)
            {
                var currTbl = new dbRntChnlRentalsUnitedEstateRoomTB();
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
                var currTbl = new dbRntChnlRentalsUnitedEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = currEstate.id;
                currTbl.roomType = roomType;
                currTbl.roomSubType = "BEDROOM";
                currTbl.features = "";
                rooms.Add(currTbl);
            }
            if (rooms.Count == 0)
            {
                var currTbl = new dbRntChnlRentalsUnitedEstateRoomTB();
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

        using (var dcRntOld = maga_DataContext.DC_RENTAL)
        using (DCmodRental dcRnt = new DCmodRental())
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
        {
            int IdEstate = currEstate.id;
            var currRentalsUnited = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
            if (currRentalsUnited == null)
            {
                currRentalsUnited = new dbRntChnlRentalsUnitedEstateTB() { id = IdEstate };
                dcChnl.Add(currRentalsUnited);
                dcChnl.SaveChanges();
                currRentalsUnited.is_active = 1;
                currRentalsUnited.is_slave = 0;
                dcChnl.SaveChanges();
            }
            currRentalsUnited.mq_inner = currEstate.mq_inner;
            currRentalsUnited.mq_inner_unit = "METERS_SQUARED";
            currRentalsUnited.num_max_sleep = currEstate.num_persons_max;
            currRentalsUnited.propertyType = currEstate.haPropertyType;
            dcChnl.SaveChanges();

            var listToDelete = dcChnl.dbRntChnlRentalsUnitedEstateRoomTBs.Where(x => x.pidEstate == currEstate.id).ToList();
            if (listToDelete.Count > 0)
            {
                foreach (dbRntChnlRentalsUnitedEstateRoomTB RURoomTB in listToDelete)
                {
                    var listLnToDelete = dcChnl.dbRntChnlRentalsUnitedEstateRoomLNs.Where(x => x.pidRoom == RURoomTB.uid).ToList();
                    if (listLnToDelete.Count > 0)
                        dcChnl.Delete(listLnToDelete);
                }
                dcChnl.Delete(listToDelete);
            }
            //copy bathroom/ bedroom type
            var RentalsUnitedInternsRL = dcChnl.dbRntChnlRentalsUnitedInternsRLs.ToList();
            var RentalsUnitedInternSubTypeRL = dcChnl.dbRntChnlRentalsUnitedInternSubTypeRLs.ToList();

            var listRooms = dcRnt.dbRntEstateInternsTBs.Where(x => x.pidEstate == currEstate.id && (x.pidInternsType == "Bedroom" || x.pidInternsType == "Bathroom")).ToList();
            foreach (dbRntEstateInternsTB room in listRooms)
            {
                var temp = RentalsUnitedInternsRL.SingleOrDefault(x => x.pidInternsType == room.pidInternsSubType);
                if (temp != null)
                {
                    var featuresList = new List<FeatureItem>();

                    var ListEstateInternsFeatureRL = dcRnt.dbRntEstateInternsFeatureRLs.Where(x => x.pidEstateInterns == room.id).ToList();
                    foreach (dbRntEstateInternsFeatureRL feature in ListEstateInternsFeatureRL)
                    {
                        var objtemp = RentalsUnitedInternsRL.SingleOrDefault(x => x.pidInternsType == feature.pidInternsFeature);
                        if (objtemp != null)
                            featuresList.Add(new FeatureItem(objtemp.pidHAInternsType, feature.count.objToInt32()));
                    }


                    try
                    {
                        var objNewRoom = new dbRntChnlRentalsUnitedEstateRoomTB();
                        objNewRoom.pidEstate = currEstate.id;
                        objNewRoom.roomType = room.pidInternsType;
                        var objSubType = RentalsUnitedInternSubTypeRL.SingleOrDefault(x => x.pidInternSubType == room.pidInternsSubType);
                        if (objSubType != null)
                            objNewRoom.roomSubType = objSubType.pidHAInternSubType;
                        objNewRoom.features = featuresList.Select(x => x.getString()).ToList().listToString("|");
                        dcChnl.Add(objNewRoom);
                        dcChnl.SaveChanges();

                        var lnData = dcRnt.dbRntEstateInternsLNs.Where(x => x.pidEstateInterns == room.id);
                        foreach (dbRntEstateInternsLN objLang in lnData)
                        {
                            var objRoomLN = new dbRntChnlRentalsUnitedEstateRoomLN();
                            objRoomLN.pidRoom = objNewRoom.uid;
                            objRoomLN.pidLang = objLang.pidLang + "";
                            objRoomLN.title = objLang.title;
                            objRoomLN.description = objLang.description;
                            dcChnl.Add(objRoomLN);
                            dcChnl.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        ErrorLog.addLog("", "ChnlRentalsUnitedUtils.updateEstateFromMagarental RoomTB, IdEstate: " + IdEstate, ex.ToString());
                    }
                }
            }

            //copy property type
            //var propertyType = dcChnl.dbRntChnlRentalsUnitedPropertyTypeRLs.SingleOrDefault(x => x.pidCategory == currEstate.pid_category);
            //if (propertyType != null)
            //{
            //    currRentalsUnited.propertyType = propertyType.pidHACategory;
            //}

            List<RNT_LN_ESTATE> lstEstate = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == IdEstate).ToList();
            if (lstEstate != null && lstEstate.Count > 0)
            {
                foreach (RNT_LN_ESTATE objEstate in lstEstate)
                {
                    var pid_lang = contUtils.getLang_code(objEstate.pid_lang);
                    dbRntChnlRentalsUnitedEstateLN currRentalsUnitedLN = dcChnl.dbRntChnlRentalsUnitedEstateLNs.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == pid_lang);
                    if (currRentalsUnitedLN == null)
                    {
                        currRentalsUnitedLN = new dbRntChnlRentalsUnitedEstateLN();
                        currRentalsUnitedLN.pid_estate = currRentalsUnited.id;
                        currRentalsUnitedLN.pid_lang = pid_lang;
                        dcChnl.Add(currRentalsUnitedLN);
                        dcChnl.SaveChanges();
                    }
                    currRentalsUnitedLN.title = !string.IsNullOrEmpty(objEstate.haHeadLine) ? objEstate.haHeadLine : objEstate.title;
                    currRentalsUnitedLN.unit_name = objEstate.title;
                    currRentalsUnitedLN.summary = objEstate.summary;
                    currRentalsUnitedLN.unit_description = objEstate.description;
                    currRentalsUnitedLN.description = !string.IsNullOrEmpty(objEstate.haDescription) ? objEstate.haDescription : objEstate.description;
                    currRentalsUnitedLN.price_note = objEstate.haRateNote;
                    currRentalsUnitedLN.location_other_activities = objEstate.haOtherActivities;
                    dcChnl.SaveChanges();
                }
            }
            var extrasIds = dcRntOld.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.is_HomeAway == 0).Select(x => x.pid_config.ToString()).ToList();
            foreach (var extrasId in extrasIds)
            {
                var featureValuesTbl = dcChnl.dbRntChnlRentalsUnitedLkFeatureValuesTBLs.FirstOrDefault(x => x.refType == "Extras" && x.refId == extrasId);
                if (featureValuesTbl != null)
                {
                    var currRl = dcChnl.dbRntChnlRentalsUnitedEstateFeaturesRLs.FirstOrDefault(x => x.pidEstate == IdEstate && x.type == featureValuesTbl.type && x.code == featureValuesTbl.code);
                    if (currRl == null)
                    {
                        currRl = new dbRntChnlRentalsUnitedEstateFeaturesRL();
                        currRl.pidEstate = IdEstate;
                        currRl.type = featureValuesTbl.type;
                        currRl.code = featureValuesTbl.code;
                        dcChnl.Add(currRl);
                        dcChnl.SaveChanges();
                    }
                }
            }
        }
    }
}
public class FeatureItem
{
    public string Code { get; set; }
    public int Count { get; set; }
    public FeatureItem(string code, int count)
    {
        Code = code;
        Count = count;
    }
    public FeatureItem(string featuresString)
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
public static class ChnlRentalsUnitedProps
{
    public static string IdAdMedia = "RU";
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

    public static List<string> GetRURestrictedLangs()
    {
        List<string> lst = new List<string>();
        string restrictedLangs = CommonUtilities.getSYS_SETTING("RURestrictedLangs");
        if (!string.IsNullOrEmpty(restrictedLangs))
            lst = restrictedLangs.splitStringToList(",");
        return lst;
    }

    public static string GetHALocale(string pidLang)
    {
        //ita, eng, esp, fra, den, nl, pt, fin, sv
        string locale = "";
        if (pidLang == "ita")
            locale = "it";

        else if (pidLang == "eng")
            locale = "en";

        else if (pidLang == "esp")
            locale = "es";

        else if (pidLang == "fra")
            locale = "fr";

        else if (pidLang == "den")
            locale = "de";

        else if (pidLang == "nl")
            locale = "nl";

        else if (pidLang == "pt")
            locale = "pt";

        else if (pidLang == "fin")
            locale = "fi";

        else if (pidLang == "sv")
            locale = "sv";

        return locale;
    }
}
public class ChnlRentalsUnitedImport
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
                using (DCchnlRentalsUnited dc = new DCchnlRentalsUnited())
                using (DCmodRental dcRnt = new DCmodRental())
                using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
                {
                    var managerTbl = ModAppServerCommon.utils.getChnlHomeAwayManager(ChnlRentalsUnitedProps.IdManager);
                    if (managerTbl == null) return;
                    var agentTbl = dcRnt.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0) return;

                    requesUrl = "https://integration.RentalsUnited.com/services/external/inquiries/3.1";
                    if (managerTbl.isDemo == 1)
                        requesUrl = "https://integration-stage.RentalsUnited.com/services/external/inquiries/3.1";

                    ChnlRentalsUnitedClasses.InquirySearchRequest Request = new ChnlRentalsUnitedClasses.InquirySearchRequest();
                    Request.assignedSystemId = managerTbl.assignedSystemId;
                    Request.advertiserAssignedId = AdvertiserAssignedId;
                    Request.timePeriod = TimePeriod;
                    Request.authorizationToken = managerTbl.authorizationToken;
                    string requestContent = Request.GetXml() + "";
                    string tmpErrorString = "";
                    string responseData = ChnlRentalsUnitedUtils.SendRequest(requesUrl, requestContent, out tmpErrorString);
                    ErrorString = tmpErrorString;
                    if (responseData == "")
                    {
                        requestComments = "ERROR: empty response";
                        ChnlRentalsUnitedUtils.addLog("ChnlRentalsUnitedImport.Enquiries_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);
                        return;
                    }
                    ChnlRentalsUnitedUtils.addLog("ChnlRentalsUnitedImport.Enquiries_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);
                    ChnlRentalsUnitedClasses.InquirySearchResponse Response = new ChnlRentalsUnitedClasses.InquirySearchResponse(responseData);
                    var advertiser = Response.advertisers.FirstOrDefault();
                    if (advertiser == null)
                    {
                        requestComments = "ERROR: empty response";
                        ChnlRentalsUnitedUtils.addLog("ChnlRentalsUnitedImport.Enquiries_process advertiserAssignedId:" + AdvertiserAssignedId, requestComments, requestContent, responseData, requesUrl);
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
                        var _request = DC_RENTAL.RNT_TBL_REQUEST.FirstOrDefault(x => x.chnlRefId == inquiry.inquiryId && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
                        if (_request == null)
                        {
                            _request = new RNT_TBL_REQUEST();
                            _request.IdAdMedia = ChnlRentalsUnitedProps.IdAdMedia;
                            _request.IdLink = inquiry.travelerSourceHA; //inquiry.listingChannel;
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

                            if (string.IsNullOrEmpty(country_title) && inquiry.inquirer.phoneCountryCode != "")
                            {
                                string phoneCountryCode = inquiry.inquirer.phoneCountryCode;
                                //ErrorLog.addLog("", "phoneCountryCode", phoneCountryCode);
                                magaLocation_DataContext DC_LOCATION = new magaLocation_DataContext();
                                List<LOC_LK_COUNTRY> currCountry1 = DC_LOCATION.LOC_LK_COUNTRies.Where(x => x.country_code == phoneCountryCode.objToInt32()).ToList();
                                if (currCountry1 != null && currCountry1.Count == 1)
                                {
                                    country_id = currCountry1[0].id;
                                    country_title = currCountry1[0].title;
                                }
                            }

                            _request.request_country = country_title;
                            _request.request_date_created = DateTime.Now;
                            _request.state_date = DateTime.Now;
                            _request.state_pid = 1;
                            _request.state_subject = "Creata Richiesta";
                            _request.state_pid_user = 1;
                            _request.request_ip = "";
                            _request.pid_creator = 1;
                            _request.pid_city = AppSettings.RNT_currCity;
                            //_request.request_choices = "1" + " - " + CurrentSource.rntEstate_title(id.objToInt32(), _request.pid_lang, "");
                            _request.name_full = string.IsNullOrEmpty(inquiry.inquirer.name) ? inquiry.inquirer.firstName + " " + inquiry.inquirer.lastName : inquiry.inquirer.name;
                            _request.email = inquiry.inquirer.emailAddress;
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
                        _request.pid_city = AppSettings.RNT_currCity;
                        _request.name_full = string.IsNullOrEmpty(inquiry.inquirer.name) ? inquiry.inquirer.firstName + " " + inquiry.inquirer.lastName : inquiry.inquirer.name;
                        _request.phone = inquiry.inquirer.phoneNumber;
                        _request.email = inquiry.inquirer.emailAddress;
                        _request.request_date_start = inquiry.reservation.reservationDates.beginDate.ValueDate;
                        _request.request_date_end = inquiry.reservation.reservationDates.endDate.ValueDate;
                        _request.request_adult_num = inquiry.reservation.numberOfAdults;
                        _request.request_child_num = inquiry.reservation.numberOfChildren;
                        _request.request_transport = "";
                        _request.request_notes = inquiry.message;
                        _request.IdLink = inquiry.travelerSourceHA;
                        DC_RENTAL.SubmitChanges();
                    }
                    requestComments = "IMPORTED countCreated:" + countCreated + ", countUpdated:" + countUpdated + ", countNoProperty:" + countNoProperty;
                    ChnlRentalsUnitedUtils.addLog("ChnlRentalsUnitedImport.Enquiries_process", requestComments, requestContent, responseData, requesUrl);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlRentalsUnitedImport.Enquiries_process advertiserAssignedId:" + AdvertiserAssignedId, ex.ToString());
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
            Thread.Sleep(1000);//give it a second or two to start

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlRentalsUnitedImport.Enquiries_process advertiserAssignedId:" + advertiserAssignedId);
        }
    }
    public static string Enquiries_start(string advertiserAssignedId, string timePeriod)
    {
        Enquiries_process _tmp = new Enquiries_process(advertiserAssignedId, timePeriod);
        return _tmp.ErrorString;
    }
    public static void Enquiries_all(string timePeriod)
    {
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
        {
            var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault();
            if (ownerTbl == null) return;
            if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId)) Enquiries_start(ownerTbl.advertiserAssignedId, timePeriod);
            if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId)) Enquiries_start(ownerTbl.ppb_advertiserAssignedId, timePeriod);
        }
    }
}
public class ChnlRentalsUnitedService
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
        ChnlRentalsUnitedClasses.ErrorList errorList = new ChnlRentalsUnitedClasses.ErrorList("ListingContentIndexRequest");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCmodRental dc = new DCmodRental())
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            try
            {
                var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "ListingContentIndexRequest", "Owner for RentalsUnited was not found or not active");
                    return "";
                }
                var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    errorList.AddError("OTHER");
                    ErrorLog.addLog("", "ListingContentIndexRequest", "Agent for RentalsUnited was not found or not active");
                    return "";
                }
                ChnlRentalsUnitedClasses.ListingContentIndexResponse Response = new ChnlRentalsUnitedClasses.ListingContentIndexResponse();
                List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == null || x.is_slave == 0)).Select(x => (int?)x.id).ToList();
                var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();


                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    var advertiserPPB = new ChnlRentalsUnitedClasses.Advertiser();
                    advertiserPPB.assignedId = ownerTbl.ppb_advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlRentalsUnitedClasses.ListingContentIndexEntry();
                        entry.listingExternalId = estate.id + "";
                        entry.active = true;
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.listingUrl = App.HOST_SSL + "/chnlutils/rentalsunited/listing/" + estate.id;
                        advertiserPPB.listingContentIndexEntry.Add(entry);
                    }
                    Response.advertisers.Add(advertiserPPB);
                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    var ppbIds = estateList.Select(x => x.id).ToList();
                    contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType != "PPB").Select(x => x.id).ToList();
                    estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract) && !ppbIds.Contains(x.pidEstate)).Select(x => (int?)x.pidEstate).ToList();
                    estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == 0 || x.is_slave == null)).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();
                    var advertiserNOPPB = new ChnlRentalsUnitedClasses.Advertiser();
                    advertiserNOPPB.assignedId = ownerTbl.advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlRentalsUnitedClasses.ListingContentIndexEntry();
                        entry.listingExternalId = estate.id + "";
                        entry.active = true;
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.listingUrl = App.HOST_SSL + "/chnlutils/rentalsunited/listing/" + estate.id;
                        advertiserNOPPB.listingContentIndexEntry.Add(entry);
                    }
                    Response.advertisers.Add(advertiserNOPPB);
                }

                //var estateIds = new List<int?>();
                //var estateList = new List<AppSettings.RNT_estate>();
                //if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                //{
                //    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.ppb_advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                //    estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                //    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();
                //    var advertiserPPB = new ChnlRentalsUnitedClasses.Advertiser();
                //    advertiserPPB.assignedId = ownerTbl.ppb_advertiserAssignedId;
                //    foreach (var estate in estateList)
                //    {
                //        var entry = new ChnlRentalsUnitedClasses.ListingContentIndexEntry();
                //        entry.listingExternalId = estate.id + "";
                //        entry.active = true;
                //        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                //        entry.listingUrl = App.HOST_SSL + "/chnlutils/RentalsUnited/listing/" + estate.id;
                //        advertiserPPB.listingContentIndexEntry.Add(entry);
                //    }
                //    Response.advertisers.Add(advertiserPPB);
                //}
                //if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                //{
                //    estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.haAdvertiserId == ownerTbl.advertiserAssignedId && x.is_active == 1 && x.is_deleted != 1).Select(x => (int?)x.id).ToList();
                //    estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && x.is_slave != 1).Select(x => (int?)x.id).ToList();
                //    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();
                //    var advertiserNOPPB = new ChnlRentalsUnitedClasses.Advertiser();
                //    advertiserNOPPB.assignedId = ownerTbl.advertiserAssignedId;
                //    foreach (var estate in estateList)
                //    {
                //        var entry = new ChnlRentalsUnitedClasses.ListingContentIndexEntry();
                //        entry.listingExternalId = estate.id + "";
                //        entry.active = true;
                //        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                //        entry.listingUrl = App.HOST_SSL + "/chnlutils/RentalsUnited/listing/" + estate.id;
                //        advertiserNOPPB.listingContentIndexEntry.Add(entry);
                //    }
                //    Response.advertisers.Add(advertiserNOPPB);
                //}

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "ListingContentIndexRequest", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string ListingRateContentRequest()
    {
        long agentID = 0;
        ChnlRentalsUnitedClasses.ErrorList errorList = new ChnlRentalsUnitedClasses.ErrorList("ListingRateContentRequest");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCmodRental dc = new DCmodRental())
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            try
            {
                var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault();
                var lstEstate_ru = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1).ToList();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "ListingRateContentRequest", "Owner for RentalsUnited was not found or not active");
                    return "";
                }
                var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    errorList.AddError("OTHER");
                    ErrorLog.addLog("", "ListingRateContentRequest", "Agent for RentalsUnited was not found or not active");
                    return "";
                }

                ChnlRentalsUnitedClasses.ListingRateContentResponse Response = new ChnlRentalsUnitedClasses.ListingRateContentResponse();
                List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                //estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == null || x.is_slave == 0)).Select(x => (int?)x.id).ToList();
                //estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1).Select(x => (int?)x.id).ToList();
                estateIds = lstEstate_ru.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();


                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    var advertiserPPB = new ChnlRentalsUnitedClasses.AdvertiserRate();
                    advertiserPPB.assignedId = ownerTbl.ppb_advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlRentalsUnitedClasses.ListingRateContentEntry();
                        var ru_esate = lstEstate_ru.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ru_esate.is_slave == 1 ? ru_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";
                        entry.active = true;
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.unitRateUrl = App.HOST_SSL + "/chnlutils/rentalsunited/unit-rate/" + estate.id;
                        advertiserPPB.listingRateContentEntry.Add(entry);
                    }
                    Response.advertisers.Add(advertiserPPB);
                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    var ppbIds = estateList.Select(x => x.id).ToList();
                    contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType != "PPB").Select(x => x.id).ToList();
                    estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract) && !ppbIds.Contains(x.pidEstate)).Select(x => (int?)x.pidEstate).ToList();
                    //estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == 0 || x.is_slave == null)).Select(x => (int?)x.id).ToList();
                    estateIds = lstEstate_ru.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                    var advertiserNOPPB = new ChnlRentalsUnitedClasses.AdvertiserRate();
                    advertiserNOPPB.assignedId = ownerTbl.advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlRentalsUnitedClasses.ListingRateContentEntry();
                        var ru_esate = lstEstate_ru.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ru_esate.is_slave == 1 ? ru_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";
                        entry.active = true;
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.unitRateUrl = App.HOST_SSL + "/chnlutils/rentalsunited/unit-rate/" + estate.id;
                        advertiserNOPPB.listingRateContentEntry.Add(entry);
                    }
                    Response.advertisers.Add(advertiserNOPPB);
                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "ListingRateContentRequest", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string ListingAvailabilityContentRequest()
    {

        long agentID = 0;
        ChnlRentalsUnitedClasses.ErrorList errorList = new ChnlRentalsUnitedClasses.ErrorList("ListingAvailabilityContentRequest");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCmodRental dc = new DCmodRental())
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            try
            {
                var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault();
                var lstEstate_ru = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1).ToList();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "ListingAvailabilityContentRequest", "Owner for RentalsUnited was not found or not active");
                    return "";
                }
                var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    errorList.AddError("OTHER");
                    ErrorLog.addLog("", "ListingAvailabilityContentRequest", "Agent for RentalsUnited was not found or not active");
                    return "";
                }


                ChnlRentalsUnitedClasses.ListingAvailabilityContentResponse Response = new ChnlRentalsUnitedClasses.ListingAvailabilityContentResponse();
                List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                //estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == null || x.is_slave == 0)).Select(x => (int?)x.id).ToList();
                //estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1).Select(x => (int?)x.id).ToList();
                estateIds = lstEstate_ru.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();


                if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId))
                {
                    var advertiserPPB = new ChnlRentalsUnitedClasses.AdvertiserAvailability();
                    advertiserPPB.assignedId = ownerTbl.ppb_advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlRentalsUnitedClasses.ListingAvailabilityContentEntry();
                        var ru_esate = lstEstate_ru.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ru_esate.is_slave == 1 ? ru_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";
                        entry.active = true;
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.unitAvailabilityUrl = App.HOST_SSL + "/chnlutils/rentalsunited/unit-availability/" + estate.id;
                        advertiserPPB.listingAvailabilityContentEntry.Add(entry);
                    }
                    Response.advertisers.Add(advertiserPPB);
                }
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    var ppbIds = estateList.Select(x => x.id).ToList();
                    contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType != "PPB").Select(x => x.id).ToList();
                    estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract) && !ppbIds.Contains(x.pidEstate)).Select(x => (int?)x.pidEstate).ToList();
                    //estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id) && (x.is_slave == 0 || x.is_slave == null)).Select(x => (int?)x.id).ToList();
                    estateIds = lstEstate_ru.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();
                    var advertiserNOPPB = new ChnlRentalsUnitedClasses.AdvertiserAvailability();
                    advertiserNOPPB.assignedId = ownerTbl.advertiserAssignedId;
                    foreach (var estate in estateList)
                    {
                        var entry = new ChnlRentalsUnitedClasses.ListingAvailabilityContentEntry();
                        var ru_esate = lstEstate_ru.SingleOrDefault(x => x.id == estate.id);
                        entry.listingExternalId = ru_esate.is_slave == 1 ? ru_esate.pid_master_estate + "" : estate.id + "";
                        entry.unitExternalId = estate.id + "";
                        entry.active = true;
                        entry.lastUpdatedDate.ValueDate = DateTime.Now;
                        entry.unitAvailabilityUrl = App.HOST_SSL + "/chnlutils/rentalsunited/unit-availability/" + estate.id;
                        advertiserNOPPB.listingAvailabilityContentEntry.Add(entry);
                    }
                    Response.advertisers.Add(advertiserNOPPB);
                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "ListingAvailabilityContentRequest", ex.ToString());
            }
        return errorList.GetXml();
    }
    //
    public static string BookingContentIndexRequest(string xml)
    {
        long agentID = 0;
        ChnlRentalsUnitedClasses.ErrorList errorList = new ChnlRentalsUnitedClasses.ErrorList("bookingContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCmodRental dc = new DCmodRental())
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            try
            {
                var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault();
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "BookingContentIndexRequest", "Owner for RentalsUnited was not found or not active");
                    return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "bookingContentIndex");
                }
                var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    ErrorLog.addLog("", "BookingContentIndexRequest", "Agent for RentalsUnited was not found or not active");
                    return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "bookingContentIndex");
                }

                //first fetched reservation list
                ChnlRentalsUnitedClasses.BookingContentIndexRequest Request = new ChnlRentalsUnitedClasses.BookingContentIndexRequest(xml);

                var haEstates = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1);

                var reservationList = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => (x.state_pid == 4 || x.state_pid == 3) && x.agentID == agentID && x.state_date.HasValue).ToList();
                if (Request.lastUpdatedDate.ValueDate.HasValue)
                {
                    var resIds = dc.dbRntReservationLastUpdatedLOGs.Where(x => x.lastUpdatedDate >= Request.lastUpdatedDate.ValueDate.Value).Select(x => x.id).ToList();
                    reservationList = reservationList.Where(x => resIds.Contains(x.id) || x.state_date.Value >= Request.lastUpdatedDate.ValueDate.Value).ToList();
                }

                ChnlRentalsUnitedClasses.BookingContentIndexResponse Response = new ChnlRentalsUnitedClasses.BookingContentIndexResponse();

                #region new added for PPB advertiser
                if (ownerTbl.ppb_advertiserAssignedId != "")
                {
                    List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType == "PPB").Select(x => x.id).ToList();
                    List<int?> PPBestateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                    PPBestateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && PPBestateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    PPBestateIds = AppSettings.RNT_estateList.Where(x => PPBestateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    var PPbreservationList = reservationList.Where(x => PPBestateIds.Contains(x.pid_estate)).ToList();

                    var PPBadvertiser = new ChnlRentalsUnitedClasses.Advertiser();
                    PPBadvertiser.assignedId = ownerTbl.ppb_advertiserAssignedId;
                    foreach (var reservation in PPbreservationList)
                    {
                        var entry = new ChnlRentalsUnitedClasses.BookingContentIndexEntry();
                        entry.active = true;
                        var lastUpdatedDate = dc.dbRntReservationLastUpdatedLOGs.SingleOrDefault(x => x.id == reservation.id);
                        entry.lastUpdatedDate.ValueDate = lastUpdatedDate != null ? lastUpdatedDate.lastUpdatedDate : reservation.state_date.Value;
                        entry.bookingUrl = App.HOST_SSL + "/chnlutils/rentalsunited/booking/" + reservation.unique_id;
                        PPBadvertiser.bookingContentIndexEntryList.Add(entry);
                    }
                    Response.advertisers.Add(PPBadvertiser);
                }
                #endregion

                #region new added for without PPB advertiser
                if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                {
                    //var ppbIds = estateList.Select(x => x.id).ToList();
                    List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID && x.contractType != "PPB").Select(x => x.id).ToList();
                    List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                    estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    estateIds = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                    var NotPPbreservationList = reservationList.Where(x => estateIds.Contains(x.pid_estate)).ToList();

                    var advertiser = new ChnlRentalsUnitedClasses.Advertiser();
                    advertiser.assignedId = ownerTbl.advertiserAssignedId;
                    foreach (var reservation in NotPPbreservationList)
                    {
                        var entry = new ChnlRentalsUnitedClasses.BookingContentIndexEntry();
                        entry.active = true;
                        var lastUpdatedDate = dc.dbRntReservationLastUpdatedLOGs.SingleOrDefault(x => x.id == reservation.id);
                        entry.lastUpdatedDate.ValueDate = lastUpdatedDate != null ? lastUpdatedDate.lastUpdatedDate : reservation.state_date.Value;
                        entry.bookingUrl = App.HOST_SSL + "/chnlutils/rentalsunited/booking/" + reservation.unique_id;
                        advertiser.bookingContentIndexEntryList.Add(entry);
                    }
                    Response.advertisers.Add(advertiser);
                }
                #endregion

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "HA_Quote", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string Listing(int IdEstate)
    {
        using (DCmodAuth dcAuth = new DCmodAuth())
        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
        {
            long agentID = 0;
            var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
            if (agentTbl != null)
                agentID = agentTbl.id;
            if (agentID == 0)
            {
                ErrorLog.addLog("", "Listing", "Agent for RentalsUnited was not found or not active");
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
            if (ownerTbl == null)
            {
                ErrorLog.addLog("", "Listing", "Owner for RentalsUnited was not found or not active");
                return "";
            }
            RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
            if (currEstate == null)
            {
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "quoteResponse");
            }
            dbRntChnlRentalsUnitedEstateTB currHAEstate = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
            if (currHAEstate == null)
            {
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "quoteResponse");
            }
            var Units = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.id == IdEstate || (x.is_slave == 1 && x.pid_master_estate == IdEstate)).ToList();

            List<string> lstResrtictedLangs = ChnlRentalsUnitedProps.GetRURestrictedLangs();
            var currLnList = dcChnl.dbRntChnlRentalsUnitedEstateLNs.Where(x => x.pid_estate == IdEstate && !lstResrtictedLangs.Contains(x.pid_lang)).ToList();

            var dtStart = DateTime.Now.Date;
            var dtEnd = DateTime.Now.AddYears(2).Date;

            ChnlRentalsUnitedClasses.ListingClasses.Listing Response = new ChnlRentalsUnitedClasses.ListingClasses.Listing();
            Response.externalId = IdEstate + "";
            Response.active = true;

            //ALL TEXT VALUES
            foreach (var ln in currLnList)
            {
                string pidLang = ChnlRentalsUnitedProps.GetHALocale(ln.pid_lang);
                Response.adContent.accommodationsSummary.Add(ln.summary, pidLang);
                Response.adContent.description.Add(ln.description, pidLang);
                Response.adContent.headline.Add(ln.title, pidLang);
                Response.adContent.ownerListingStory.Add(ln.listing_story, pidLang);
                Response.adContent.propertyName.Add(CurrentSource.rntEstate_title(IdEstate, ln.pid_lang.objToInt32(), currEstate.code), pidLang);
                Response.adContent.uniqueBenefits.Add(ln.unique_benifits, pidLang);
                Response.adContent.whyPurchased.Add(ln.why_purchased, pidLang);
                Response.location.description.Add(ln.location_description, pidLang);
                Response.location.otherActivities.Add(ln.location_other_activities, pidLang);
            }

            //AD CONTENT
            Response.adContent.yearPurchased = currHAEstate.year_purchased;

            //featureValues
            var features = dcChnl.dbRntChnlRentalsUnitedEstateFeaturesRLs.Where(x => x.pidEstate == IdEstate && x.type == "Listing").ToList();
            foreach (var feature in features)
            {
                if (feature.count.objToInt32() == 0) continue;
                var featureValue = new ChnlRentalsUnitedClasses.ListingClasses.FeatureValue();
                featureValue.count = feature.count.objToInt32();
                featureValue.listingFeatureName = feature.code;
                Response.featureValues.Add(featureValue);
            }
            Response.adContent.yearPurchased = currHAEstate.year_purchased;

            //LOCATION
            Response.location.address.addressLine1 = currEstate.loc_address;
            Response.location.address.addressLine2 = currEstate.loc_inner_bell;
            Response.location.address.postalCode = currEstate.loc_zip_code;
            Response.location.address.city = CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), App.DefLangID, "");
            Response.location.address.stateOrProvince = CommonUtilities.getSYS_SETTING("HAStateProvince"); //CurrentSource.loc(currEstate.pid_city.objToInt32(), App.DefLangID, "");
            Response.location.address.country = CommonUtilities.getSYS_SETTING("HACountry"); //locUtils.getCity_codeOfCountry(currEstate.pid_city.objToInt32(), App.DefLangID, "");
            if (currEstate.google_maps != "" && currEstate.google_maps != null)
            {
                if (currEstate.google_maps.Split('|').Length > 1)
                {
                    Response.location.geoCode.latitude = currEstate.google_maps.Split('|')[0].Replace(",", ".");
                    Response.location.geoCode.longitude = currEstate.google_maps.Split('|')[1].Replace(",", ".");
                }
                else if (currEstate.google_maps.Split(',').Length > 1)
                {
                    Response.location.geoCode.latitude = currEstate.google_maps.Split(',')[0];
                    Response.location.geoCode.longitude = currEstate.google_maps.Split(',')[1];
                }
            }
            Response.location.showExactLocation = currHAEstate.show_exact_location == 1;

            var images = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => (x.type == "RentalsUnited" || x.type == "chnlRU") && x.pid_estate == IdEstate).ToList();
            foreach (var image in images)
            {
                var tmp = new ChnlRentalsUnitedClasses.ListingClasses.Image();
                tmp.externalId = image.id + "";
                tmp.title.Add(image.code, "en");
                tmp.uri = App.HOST + "/" + image.img_banner + "?maxsize=true";
                Response.images.Add(tmp);
            }

            // UNITS
            foreach (var unit in Units)
            {
                var unitEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == unit.id && x.is_active == 1);
                if (unitEstate == null)
                {
                    continue;
                }
                var currUnit = new ChnlRentalsUnitedClasses.ListingClasses.Unit();
                currUnit.externalId = unit.id + "";
                currUnit.active = true;
                currUnit.area = unit.mq_inner.objToInt32();
                currUnit.areaUnit = unit.mq_inner_unit;
                currUnit.diningSeating = unit.num_dining_seating;
                currUnit.loungeSeating = unit.num_lounge_seating;
                currUnit.maxSleep = unit.num_max_sleep.objToInt32();
                if (currUnit.maxSleep == 0) currUnit.maxSleep = unitEstate.num_persons_max.objToInt32();
                currUnit.diningSeating = unit.num_dining_seating;
                currUnit.propertyType = unit.propertyType + "";

                // TODO unitMonetaryInformation
                //currUnit.unitMonetaryInformation.cleaningFee.ValueDecimal = 0;
                currUnit.unitMonetaryInformation.contractualBookingDeposit.ValueDecimal = currEstate.pr_percentage.objToDecimal();
                currUnit.unitMonetaryInformation.damageDeposit.ValueDecimal = currEstate.pr_deposit.objToDecimal();
                currUnit.unitMonetaryInformation.nonContractualBookingDeposit.ValueDecimal = currEstate.pr_percentage.objToDecimal();
                //currUnit.unitMonetaryInformation.rateNotes.Add = 0;
                //currUnit.unitMonetaryInformation.tax.ValueDecimal = 0;

                //featureValues
                features = dcChnl.dbRntChnlRentalsUnitedEstateFeaturesRLs.Where(x => x.pidEstate == IdEstate && x.type == "Unit").ToList();
                foreach (var feature in features)
                {
                    if (feature.count.objToInt32() == 0) continue;
                    var featureValue = new ChnlRentalsUnitedClasses.ListingClasses.FeatureValue();
                    featureValue.count = feature.count.objToInt32();
                    featureValue.unitFeatureName = feature.code;
                    currUnit.featureValues.Add(featureValue);
                }

                // bathrooms
                var bathRooms = dcChnl.dbRntChnlRentalsUnitedEstateRoomTBs.Where(x => x.pidEstate == IdEstate && x.roomType == "Bathroom").ToList();
                if (bathRooms.Count == 0) bathRooms = ChnlRentalsUnitedUtils.getRooms(currEstate, "Bathroom");
                foreach (var room in bathRooms)
                {
                    var tmpRoom = new ChnlRentalsUnitedClasses.ListingClasses.Bathroom();
                    tmpRoom.roomSubType = room.roomSubType;
                    var featuresListString = room.features.splitStringToList("|");
                    foreach (var featuresString in featuresListString)
                    {
                        if (featuresString.splitStringToList("%").Count == 2)
                        {
                            ChnlRentalsUnitedClasses.ListingClasses.Amenity amenity = new ChnlRentalsUnitedClasses.ListingClasses.Amenity();
                            amenity.bathroomFeatureName = featuresString.splitStringToList("%")[0];
                            amenity.count = featuresString.splitStringToList("%")[1].ToInt32();
                            tmpRoom.amenities.Add(amenity);
                        }
                    }
                    var tmpRoomLns = dcChnl.dbRntChnlRentalsUnitedEstateRoomLNs.Where(x => x.pidRoom == room.uid && !lstResrtictedLangs.Contains(x.pidLang)).ToList();
                    foreach (var ln in tmpRoomLns)
                    {
                        string pidLang = ChnlRentalsUnitedProps.GetHALocale(ln.pidLang);
                        tmpRoom.name.Add(ln.title, pidLang);
                        tmpRoom.note.Add(ln.description, pidLang);
                    }
                    currUnit.bathrooms.Add(tmpRoom);
                }

                // bedrooms
                var bedRooms = dcChnl.dbRntChnlRentalsUnitedEstateRoomTBs.Where(x => x.pidEstate == IdEstate && x.roomType == "Bedroom").ToList();
                if (bedRooms.Count == 0) bedRooms = ChnlRentalsUnitedUtils.getRooms(currEstate, "Bedroom");
                foreach (var room in bedRooms)
                {
                    var tmpRoom = new ChnlRentalsUnitedClasses.ListingClasses.Bedroom();
                    tmpRoom.roomSubType = room.roomSubType;
                    var featuresListString = room.features.splitStringToList("|");
                    foreach (var featuresString in featuresListString)
                    {
                        if (featuresString.splitStringToList("%").Count == 2)
                        {
                            ChnlRentalsUnitedClasses.ListingClasses.Amenity amenity = new ChnlRentalsUnitedClasses.ListingClasses.Amenity();
                            amenity.bedroomFeatureName = featuresString.splitStringToList("%")[0];
                            amenity.count = featuresString.splitStringToList("%")[1].ToInt32();
                            tmpRoom.amenities.Add(amenity);
                        }
                    }
                    var tmpRoomLns = dcChnl.dbRntChnlRentalsUnitedEstateRoomLNs.Where(x => x.pidRoom == room.uid && !lstResrtictedLangs.Contains(x.pidLang)).ToList();
                    foreach (var ln in tmpRoomLns)
                    {
                        string pidLang = ChnlRentalsUnitedProps.GetHALocale(ln.pidLang);
                        tmpRoom.name.Add(ln.title, pidLang);
                        tmpRoom.note.Add(ln.description, pidLang);
                    }
                    currUnit.bedrooms.Add(tmpRoom);
                }

                #region Removed ratePeriods from content xml
                //// TODO ratePeriods, Finalaize
                //int outError;
                //var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, agentID, dtStart, dtEnd, out outError);
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
                //    var currRatePeriod = new ChnlRentalsUnitedClasses.ListingClasses.RatePeriod();
                //    currRatePeriod.dateRange.beginDate.ValueDate = tmp.DtStart;
                //    currRatePeriod.dateRange.endDate.ValueDate = tmp.DtEnd;
                //    currRatePeriod.minimumStay = tmp.MinStay;
                //    currRatePeriod.rates.Add(new ChnlRentalsUnitedClasses.ListingClasses.Rate("NIGHTLY_WEEKDAY", priceFull));
                //    currRatePeriod.rates.Add(new ChnlRentalsUnitedClasses.ListingClasses.Rate("NIGHTLY_WEEKEND", priceFull));
                //    currRatePeriod.rates.Add(new ChnlRentalsUnitedClasses.ListingClasses.Rate("EXTRA_NIGHT", priceFull));
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
                //    currUnit.unitAvailability.unitAvailabilityConfiguration.minStay.Add(priceList != null ? priceList.MinStay + "" : currEstate.nights_min.objToInt32() + "");
                //    currUnit.unitAvailability.unitAvailabilityConfiguration.stayIncrement.Add("D");
                //    dtCurrent = dtCurrent.AddDays(1);
                //} 
                #endregion

                foreach (var ln in currLnList)
                {
                    string pidLang = ChnlRentalsUnitedProps.GetHALocale(ln.pid_lang);
                    currUnit.bathroomDetails.Add(ln.bathroom_details, pidLang);
                    currUnit.bedroomDetails.Add(ln.bedroom_details, pidLang);
                    currUnit.description.Add(ln.unit_description, pidLang);
                    currUnit.featuresDescription.Add(ln.features_description, pidLang);
                    currUnit.unitName.Add(ln.unit_name, pidLang);
                }

                Response.units.Add(currUnit);
            }

            return Response.GetXml();
        }
    }

    public static string ListingRate(int IdEstate)
    {
        using (DCmodAuth dcAuth = new DCmodAuth())
        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
        {
            long agentID = 0;
            var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
            if (agentTbl != null)
                agentID = agentTbl.id;
            if (agentID == 0)
            {
                ErrorLog.addLog("", "ListingRate", "Agent for RentalsUnited was not found or not active");
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "ListingRate");
            }
            var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
            if (ownerTbl == null)
            {
                ErrorLog.addLog("", "ListingRate", "Owner for RentalsUnited was not found or not active");
                return "";
            }
            RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
            if (currEstate == null)
            {
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "ListingRate");
            }
            dbRntChnlRentalsUnitedEstateTB currRUEstate = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
            if (currRUEstate == null)
            {
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "ListingRate");
            }

            var dtStart = DateTime.Now.Date;
            var dtEnd = DateTime.Now.AddYears(2).Date;
            var dtEndTemp = dtEnd.AddDays(1);

            ChnlRentalsUnitedClasses.ListingClasses.unitRateEntities Response = new ChnlRentalsUnitedClasses.ListingClasses.unitRateEntities();

            Response.listingExternalId = currRUEstate.is_slave == 1 ? currRUEstate.pid_master_estate + "" : currEstate.id + "";
            Response.unitExternalId = currEstate.id + "";

            // TODO ratePeriods, Finalaize
            int outError;

            #region old
            //var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, agentID, dtStart, dtEndTemp, out outError);
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
            //    var currRatePeriod = new ChnlRentalsUnitedClasses.ListingClasses.RatePeriod();
            //    currRatePeriod.dateRange.beginDate.ValueDate = tmp.DtStart;
            //    currRatePeriod.dateRange.endDate.ValueDate = tmp.DtEnd;
            //    currRatePeriod.minimumStay = tmp.MinStay;
            //    currRatePeriod.rates.Add(new ChnlRentalsUnitedClasses.ListingClasses.Rate("NIGHTLY_WEEKDAY", priceFull));
            //    currRatePeriod.rates.Add(new ChnlRentalsUnitedClasses.ListingClasses.Rate("NIGHTLY_WEEKEND", priceFull));
            //    currRatePeriod.rates.Add(new ChnlRentalsUnitedClasses.ListingClasses.Rate("EXTRA_NIGHT", priceFull));
            //    Response.unitRatePeriods.Add(currRatePeriod);
            //}
            #endregion


            #region
            var minStayList = dcChnl.dbRntChnlRentalsUnitedEstateClosedDatesRLs.Where(x => x.pidEstate == IdEstate && x.minStay > 0 && x.changeDate >= dtStart).ToList();
            var changesListTmp = dcChnl.dbRntChnlRentalsUnitedEstateRateChangesRLs.Where(x => x.pidEstate == IdEstate && x.changeIsDiscount >= 0 && x.changeDate >= dtStart).ToList();
            var changesList = new List<dbRntChnlRentalsUnitedEstateRateChangesRL>();

            var priceListPerDatesNew = new List<ChnlRentalsUnitedClasses.PriceListPerDates>();
            var priceListPerDatesTmp = rntUtils.estate_getPriceListPerDates(IdEstate, 0, dtStart, dtEndTemp, out outError);


            foreach (var tmp in priceListPerDatesTmp)
            {
                DateTime dtCurrent = tmp.DtStart;
                while (dtCurrent <= tmp.DtEnd)
                {
                    decimal priceFull = 0;
                    if (tmp.Prices.ContainsKey(currRUEstate.num_max_sleep.objToInt32()))
                        priceFull = tmp.Prices[currRUEstate.num_max_sleep.objToInt32()];
                    else if (tmp.Prices.ContainsKey(currEstate.num_persons_max.objToInt32()))
                        priceFull = tmp.Prices[currEstate.num_persons_max.objToInt32()];
                    else if (tmp.Prices.Keys.Count > 0)
                        priceFull = tmp.Prices.Last().Value;
                    if (priceFull == 0) priceFull = 9999;

                    var datePrices = new ChnlRentalsUnitedClasses.PriceListPerDates(dtCurrent, dtCurrent, tmp.MinStay, priceFull);

                    var minStayDate = minStayList.FirstOrDefault(x => x.changeDate == dtCurrent);
                    if (minStayDate != null) datePrices.MinNights = minStayDate.minStay;

                    var rateChange = changesListTmp.FirstOrDefault(x => x.changeDate == dtCurrent);
                    if (rateChange != null)
                    {
                        decimal changeAmount = (rateChange.changeIsPercentage == 1) ? (priceFull * rateChange.changeAmount.objToInt32() / 100) : rateChange.changeAmount.objToInt32();
                        if (rateChange.changeIsDiscount == 1) { changeAmount = -changeAmount; }
                        datePrices.PriceRU = priceFull + changeAmount;
                    }
                    else
                    {
                        decimal changeAmountGeneric = 0;
                        if (currRUEstate.changeIsDiscount >= 0 && currRUEstate.changeAmount > 0)
                        {
                            changeAmountGeneric = (currRUEstate.changeIsPercentage == 1) ? (priceFull * currRUEstate.changeAmount.objToInt32() / 100) : currRUEstate.changeAmount.objToInt32();
                            if (currRUEstate.changeIsDiscount == 1) { changeAmountGeneric = -changeAmountGeneric; }
                        }
                        datePrices.PriceRU = priceFull + changeAmountGeneric;
                    }

                    var lastDatePrice = priceListPerDatesNew.LastOrDefault();
                    if (lastDatePrice == null || !lastDatePrice.HasSamePricesRU(datePrices)) priceListPerDatesNew.Add(datePrices);
                    else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);

                    dtCurrent = dtCurrent.AddDays(1);
                }
            }

            foreach (var tmp in priceListPerDatesNew)
            {
                decimal priceFull = 0;
                priceFull = tmp.PriceRU;
                if (priceFull == 0) priceFull = 9999;
                var currRatePeriod = new ChnlRentalsUnitedClasses.ListingClasses.RatePeriod();
                currRatePeriod.dateRange.beginDate.ValueDate = tmp.DtStart;
                currRatePeriod.dateRange.endDate.ValueDate = tmp.DtEnd;
                currRatePeriod.minimumStay = tmp.MinNights;
                currRatePeriod.rates.Add(new ChnlRentalsUnitedClasses.ListingClasses.Rate("NIGHTLY_WEEKDAY", priceFull));
                currRatePeriod.rates.Add(new ChnlRentalsUnitedClasses.ListingClasses.Rate("NIGHTLY_WEEKEND", priceFull));
                currRatePeriod.rates.Add(new ChnlRentalsUnitedClasses.ListingClasses.Rate("EXTRA_NIGHT", priceFull));
                Response.unitRatePeriods.Add(currRatePeriod);
            }
            #endregion
            return Response.GetXml();
        }
    }

    public static string ListingAvailability(int IdEstate)
    {
        using (DCmodAuth dcAuth = new DCmodAuth())
        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
        {
            long agentID = 0;
            var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
            if (agentTbl != null)
                agentID = agentTbl.id;
            if (agentID == 0)
            {
                ErrorLog.addLog("", "ListingAvailability", "Agent for RentalsUnited was not found or not active");
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "ListingAvailability");
            }
            var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
            if (ownerTbl == null)
            {
                ErrorLog.addLog("", "ListingAvailability", "Owner for RentalsUnited was not found or not active");
                return "";
            }
            RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
            if (currEstate == null)
            {
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "ListingAvailability");
            }
            dbRntChnlRentalsUnitedEstateTB currHAEstate = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
            if (currHAEstate == null)
            {
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "ListingAvailability");
            }

            var dtStart = DateTime.Now.Date;
            var dtEnd = DateTime.Now.AddYears(2).Date;
            var dtEndTemp = dtEnd.AddDays(1);
            ChnlRentalsUnitedClasses.ListingClasses.unitAvailabilityEntities Response = new ChnlRentalsUnitedClasses.ListingClasses.unitAvailabilityEntities();

            Response.listingExternalId = currHAEstate.is_slave == 1 ? currHAEstate.pid_master_estate + "" : currEstate.id + "";
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
            //var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, dtStart, dtEnd, 0, out outError);
            var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, agentID, dtStart, dtEndTemp, out outError);
            var closedList = dcChnl.dbRntChnlRentalsUnitedEstateClosedDatesRLs.Where(x => x.pidEstate == IdEstate && x.changeDate >= dtStart).ToList();
            while (dtCurrent < dtEndTemp)
            {
                var tmpDate = closedList.FirstOrDefault(x => x.changeDate == dtCurrent);

                bool isAvv = resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count() == 0;
                if (tmpDate != null && tmpDate.isClosed == 1) { isAvv = false; }

                var priceList = priceListPerDates.FirstOrDefault(x => x.DtStart <= dtCurrent && x.DtEnd >= dtCurrent);
                Response.unitAvailability.unitAvailabilityConfiguration.availability.Add(priceList != null ? (isAvv ? "Y" : "N") : "N");
                Response.unitAvailability.unitAvailabilityConfiguration.changeOver.Add("C");

                //Response.unitAvailability.unitAvailabilityConfiguration.availability.Add(isAvv ? "Y" : "N");
                int min_stay = currEstate.nights_min.objToInt32();
                if (priceList != null)
                {
                    min_stay = priceList.MinStay;
                    if (tmpDate != null && tmpDate.minStay > 0)
                        min_stay = tmpDate.minStay;
                }

                Response.unitAvailability.unitAvailabilityConfiguration.minStay.Add(min_stay + "");
                Response.unitAvailability.unitAvailabilityConfiguration.stayIncrement.Add("D");
                dtCurrent = dtCurrent.AddDays(1);
            }
            return Response.GetXml();
        }
    }

    public static string Booking(long id)
    {
        using (DCmodAuth dcAuth = new DCmodAuth())
        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
        {
            var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault();
            if (ownerTbl == null)
            {
                ErrorLog.addLog("", "RU-Booking", "Owner for RentalsUnited was not found or not active");
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            long agentID = 0;
            var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
            if (agentTbl != null)
                agentID = agentTbl.id;
            if (agentID == 0)
            {
                ErrorLog.addLog("", "RU-Booking", "Agent for RentalsUnited was not found or not active");
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            ChnlRentalsUnitedClasses.BookingResponse Response = new ChnlRentalsUnitedClasses.BookingResponse();
            var reservationTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == id);
            if (reservationTbl == null)
            {
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == reservationTbl.pid_estate.objToInt32());
            if (currEstateTB == null)
            {
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            dbRntChnlRentalsUnitedEstateTB currHAEstate = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == reservationTbl.pid_estate.objToInt32());
            if (currHAEstate == null)
            {
                return ChnlRentalsUnitedClasses.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            var listPay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == reservationTbl.id && x.is_complete == 1 && x.direction == 1 && x.pr_total.HasValue).ToList();
            decimal totalPayed = listPay.Count == 0 ? 0 : listPay.Sum(x => x.pr_total.Value);

            Response.reservation.numberOfAdults = reservationTbl.num_adult.objToInt32();
            Response.reservation.numberOfChildren = reservationTbl.num_child_over.objToInt32() + reservationTbl.num_child_min.objToInt32();
            Response.reservation.reservationDates.beginDate.ValueDate = reservationTbl.dtStart;
            Response.reservation.reservationDates.endDate.ValueDate = reservationTbl.dtEnd;

            Response.inquirer = new ChnlRentalsUnitedClasses.Inquirer();
            dbAuthClientTBL clientTbl = dcAuth.dbAuthClientTBLs.SingleOrDefault(x => x.id == reservationTbl.agentClientID);
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
            var order = new ChnlRentalsUnitedClasses.Order();
            Response.orderList.Add(order);
            order.currency = "EUR";

            decimal taxPercentage = 0;
            int taxId = agentTbl.invTaxId.objToInt32();

            if (taxId == 0 || invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId) == null)
                taxPercentage = CommonUtilities.getSYS_SETTING("rnt_DefauultRUTax").objToDecimal();
            else
            {
                dbInvCashTaxLK currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId);
                if (currTax != null)
                    taxPercentage = currTax.taxAmount.objToDecimal();
            }

            var orderItem = new ChnlRentalsUnitedClasses.OrderItem();
            order.orderItemList.Add(orderItem);
            orderItem.feeType = "RENTAL";
            orderItem.description = "Rent";
            orderItem.name = "Rent";
            orderItem.preTaxAmount.ValueDecimal = getPriceWithoutTax((reservationTbl.pr_total.objToDecimal()), taxPercentage);
            orderItem.totalAmount.ValueDecimal = reservationTbl.pr_total.objToDecimal();
            orderItem.status = "ACCEPTED"; // PENDING, ACCEPTED, DECLINED, DECLINED_BY_SYSTEM, CANCELLED, EDITED           

            // TODO, finalise
            //Part Payment
            var paymentScheduleItem = new ChnlRentalsUnitedClasses.PaymentScheduleItem();
            paymentScheduleItem.amount.ValueDecimal = reservationTbl.pr_part_forPayment.objToDecimal();
            paymentScheduleItem.dueDate.ValueDate = reservationTbl.dtCreation; //reservationTbl.dtStart.Value.AddDays(-30);
            if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
            order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

            //Balance Payment
            paymentScheduleItem = new ChnlRentalsUnitedClasses.PaymentScheduleItem();
            paymentScheduleItem.amount.ValueDecimal = reservationTbl.pr_part_owner.objToDecimal();
            paymentScheduleItem.dueDate.ValueDate = reservationTbl.dtStart;
            if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
            order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

            if (currHAEstate.CancellationPolicyCharge.objToDecimal() > 0)
            {
                var cancellationPolicyItem = new ChnlRentalsUnitedClasses.CancellationPolicyItem();
                order.reservationCancellationPolicy.cancellationPolicyItemList.Add(cancellationPolicyItem);
                cancellationPolicyItem.amount.ValueDecimal = currHAEstate.CancellationPolicyCharge.objToDecimal();
                if (currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.objToInt32() != 0)
                    cancellationPolicyItem.deadline.ValueDate = (reservationTbl.dtStart.Value.AddDays(-currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.Value));
                cancellationPolicyItem.penaltyType = currHAEstate.CancellationPolicyType;
            }
            order.reservationCancellationPolicy.description = App.HOST + CurrentSource.getPagePath("19", "stp", App.LangID + "");
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
                Response.reservationStatus = "CANCELLED";
            else if (reservationTbl.dtStart <= DateTime.Now.Date && reservationTbl.dtEnd > DateTime.Now.Date)
                Response.reservationStatus = "STAY_IN_PROGRESS";
            Response.rentalAgreement.agreementText = App.HOST + CurrentSource.getPagePath("2", "stp", App.LangID + "");
            return Response.GetXml();
        }
    }

    public static string QuoteRequest(string xml)
    {
        if (CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug").ToInt32() == 1)
            ErrorLog.addLog("", "QuoteRequest", "before empty xml");
        if (xml == "") return "";
        if (CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug").ToInt32() == 1)
            ErrorLog.addLog("", "QuoteRequest", "after empty xml");
        long agentID = 0;
        ChnlRentalsUnitedClasses.ErrorList errorList = new ChnlRentalsUnitedClasses.ErrorList("quoteResponse");

        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            try
            {
                var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "GetQuoteRequest", "Owner for RentalsUnited was not found or not active");
                    return "";
                }
                var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    ErrorLog.addLog("", "GetQuoteRequest", "Agent for RentalsUnited was not found or not active");
                    return "";
                }

                if (CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug").ToInt32() == 1)
                    ErrorLog.addLog("", "QuoteRequest", "after blank");

                ChnlRentalsUnitedClasses.QuoteRequest Request = new ChnlRentalsUnitedClasses.QuoteRequest(xml);

                //for sending resonse
                RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currEstateTB == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }
                dbRntChnlRentalsUnitedEstateTB currHAEstate = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currHAEstate == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }

                DateTime dtStart = Request.reservation.reservationDates.beginDate.ValueDate.Value;
                DateTime dtEnd = Request.reservation.reservationDates.endDate.ValueDate.Value;

                DateTime dtMax = DateTime.Now.AddYears(2).Date.AddDays(1);
                if (dtEnd >= dtMax)
                {
                    errorList.AddError("END_DAY_MISMATCH");
                    return errorList.GetXml();
                }

                var lastDay = dc.dbRntSeasonDatesTBLs.FirstOrDefault(x => x.dtEnd >= dtEnd && x.dtStart <= dtEnd && x.pidSeasonGroup == currEstateTB.pidSeasonGroup);
                if (lastDay == null)
                {
                    errorList.AddError("END_DAY_MISMATCH");
                    return errorList.GetXml();
                }

                if (Request.reservation.numberOfPets.objToInt32() > 0)
                {
                    var currFeature = dcChnl.dbRntChnlRentalsUnitedEstateFeaturesRLs.SingleOrDefault(x => x.pidEstate == currHAEstate.id && x.code == "SUITABILITY_PETS_ASK");
                    if (currFeature == null || currFeature.count < Request.reservation.numberOfPets.objToInt32())
                    {
                        var currFeature_PETS_CONSIDERED = dcChnl.dbRntChnlRentalsUnitedEstateFeaturesRLs.SingleOrDefault(x => x.pidEstate == currHAEstate.id && x.code == "SUITABILITY_PETS_CONSIDERED");
                        if (currFeature_PETS_CONSIDERED == null || currFeature_PETS_CONSIDERED.count < Request.reservation.numberOfPets.objToInt32())
                        {
                            errorList.AddError("PETS_NOT_ALLOWED");
                        }
                    }
                }


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

                if (CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug").ToInt32() == 1)
                    ErrorLog.addLog("", "QuoteRequest", "after error");

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
                decimal price = rntUtils.rntEstate_getPrice_RU(0, Request.unitExternalId.objToInt32(), ref outPrice);

                if (price == 0)
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

                ChnlRentalsUnitedClasses.QuoteResponse Response = new ChnlRentalsUnitedClasses.QuoteResponse();
                Response.locale = Request.inquirer.locale;
                if (string.IsNullOrEmpty(Response.locale)) Response.locale = "en_GB";
                var order = new ChnlRentalsUnitedClasses.Order();
                Response.orderList.Add(order);
                order.currency = "EUR";

                var orderItem = new ChnlRentalsUnitedClasses.OrderItem();
                order.orderItemList.Add(orderItem);
                orderItem.feeType = "RENTAL";
                orderItem.description = "Rent";
                orderItem.name = "Rent";
                orderItem.preTaxAmount.ValueDecimal = getPriceWithoutTax((price), taxPercentage);
                orderItem.totalAmount.ValueDecimal = price;

                // TODO, finalise
                var paymentScheduleItem = new ChnlRentalsUnitedClasses.PaymentScheduleItem();
                paymentScheduleItem.amount.ValueDecimal = price;
                paymentScheduleItem.dueDate.ValueDate = dtStart.AddDays(-30);
                if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
                order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

                if (currHAEstate.CancellationPolicyCharge.objToDecimal() > 0)
                {
                    var cancellationPolicyItem = new ChnlRentalsUnitedClasses.CancellationPolicyItem();
                    order.reservationCancellationPolicy.cancellationPolicyItemList.Add(cancellationPolicyItem);
                    cancellationPolicyItem.amount.ValueDecimal = currHAEstate.CancellationPolicyCharge.objToDecimal();
                    if (currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.objToInt32() != 0)
                        cancellationPolicyItem.deadline.ValueDate = (dtStart.AddDays(-currHAEstate.CancellationPolicyDeadlineDaysBeforeCheckIn.Value));
                    cancellationPolicyItem.penaltyType = currHAEstate.CancellationPolicyType;
                }
                order.reservationCancellationPolicy.description = App.HOST + CurrentSource.getPagePath("19", "stp", App.LangID + "");

                //set stay fees for city tax
                if (contUtils.getLabel("lblRUStayFess") != "lblRUStayFess" && contUtils.getLabel("lblRUStayFess") != "lblRUStayFess")
                {
                    var stayFessList = new List<ChnlRentalsUnitedClasses.StayFees>();
                    var stayFess = new ChnlRentalsUnitedClasses.StayFees();
                    stayFess.description = contUtils.getLabel("lblRUStayFess");
                    stayFessList.Add(stayFess);
                    order.stayFees = stayFessList;
                }

                Response.rentalAgreement.agreementText = App.HOST + CurrentSource.getPagePath("3", "stp", App.LangID + "");
                return Response.GetXml();
            }
            catch (Exception ex)
            {
                if (CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlRentalsUnitedDebug").ToInt32() == 1)
                    ErrorLog.addLog("", "QuoteRequest", "catch");

                errorList.AddError("OTHER");
                ErrorLog.addLog("", "HA_Quote", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string BookingRequest(string xml)
    {
        if (xml == "") return "";

        long agentID = 0;
        ChnlRentalsUnitedClasses.ErrorList errorList = new ChnlRentalsUnitedClasses.ErrorList("bookingResponse");

        using (DCmodAuth dcAuth = new DCmodAuth())
        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            try
            {
                var ownerTbl = dcChnl.dbRntChnlRentalsUnitedOwnerTBLs.FirstOrDefault(x => x.ppb_advertiserAssignedId != "" || x.advertiserAssignedId != "");
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "GetBookingRequest", "Owner for RentalsUnited was not found or not active");
                    return "";
                }
                var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlRentalsUnitedProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    ErrorLog.addLog("", "GetBookingRequest", "Agent for RentalsUnited was not found or not active");
                    return "";
                }

                ChnlRentalsUnitedClasses.BookingRequest Request = new ChnlRentalsUnitedClasses.BookingRequest(xml);

                var ccData = "";
                ccData += "Holder: " + Request.paymentCard.nameOnCard;
                ccData += "\r\n Holder Billing AdditionalAddressLine1: " + Request.paymentCard.billingAddress.additionalAddressLine1;
                ccData += "\r\n Holder Billing AddressLine1: " + Request.paymentCard.billingAddress.addressLine1;
                ccData += "\r\n Holder Billing AddressLine2: " + Request.paymentCard.billingAddress.addressLine2;
                ccData += "\r\n Holder Billing AddressLine3: " + Request.paymentCard.billingAddress.addressLine3;
                ccData += "\r\n Holder Billing AddressLine4: " + Request.paymentCard.billingAddress.addressLine4;
                ccData += "\r\n Holder Billing AddressLine5: " + Request.paymentCard.billingAddress.addressLine5;
                ccData += "\r\n Holder Billing Country: " + Request.paymentCard.billingAddress.country;
                ccData += "\r\n Holder Billing postalCode: " + Request.paymentCard.billingAddress.postalCode;

                ccData += "\r\n ";
                ccData += "\r\n Masked Number: " + Request.paymentCard.maskedNumber;
                ccData += "\r\n Number: " + Request.paymentCard.number;
                ccData += "\r\n NumberToken: " + Request.paymentCard.numberToken;
                ccData += "\r\n paymentFormType: " + Request.paymentCard.paymentFormType;
                ccData += "\r\n paymentFormTypeCard: " + Request.paymentCard.paymentCardDescriptor.paymentFormType;
                ccData += "\r\n Card Code: " + Request.paymentCard.paymentCardDescriptor.cardCode;
                ccData += "\r\n Type: " + Request.paymentCard.paymentCardDescriptor.cardType;
                ccData += "\r\n CVC: " + Request.paymentCard.cvv;
                ccData += "\r\n Expire: " + Request.paymentCard.expiration;
                RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currEstateTB == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }
                dbRntChnlRentalsUnitedEstateTB currHAEstate = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currHAEstate == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }
                if (Request.reservation.numberOfPets.objToInt32() > 0)
                {
                    var currFeature = dcChnl.dbRntChnlRentalsUnitedEstateFeaturesRLs.SingleOrDefault(x => x.pidEstate == currHAEstate.id && x.code == "SUITABILITY_PETS_ASK");
                    if (currFeature == null || currFeature.count < Request.reservation.numberOfPets.objToInt32())
                    {
                        errorList.AddError("PETS_NOT_ALLOWED");
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
                decimal prTotal = rntUtils.rntEstate_getPrice_RU(0, Request.unitExternalId.objToInt32(), ref outPrice);
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

                    //RU fields
                    newRes.IdAdMedia = ChnlRentalsUnitedProps.IdAdMedia;
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
                    var chnlBooking = dcChnl.dbRntChnlRentalsUnitedBookingTBLs.SingleOrDefault(x => x.id == newRes.id);
                    if (chnlBooking == null)
                    {
                        chnlBooking = new dbRntChnlRentalsUnitedBookingTBL() { id = newRes.id };
                        dcChnl.Add(chnlBooking);
                    }
                    chnlBooking.inquiryId = Request.inquiryId;
                    chnlBooking.trackingUuid = Request.trackingUuid;
                    chnlBooking.numberOfPets = Request.reservation.numberOfPets.objToInt32();
                    chnlBooking.message = Request.message;
                    chnlBooking.listingChannel = Request.listingChannel;
                    chnlBooking.masterListingChannel = Request.masterListingChannel;
                    chnlBooking.commission = Request.commission;
                    chnlBooking.locale = Request.inquirer.locale;
                    dcChnl.SaveChanges();

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
                    newRes.cl_name_full = Request.inquirer.name;
                    //newRes.cl_
                    // newRes.cl_name_honorific = "";
                    newRes.cl_loc_country = Request.inquirer.address.addressLine5;
                    newRes.cl_pid_discount = -1;
                    newRes.cl_pid_lang = App.DefLangID;
                    newRes.cl_isCompleted = 0;

                    outPrice.CopyTo(ref newRes);
                    newRes.pr_paymentType = "cc";

                    //for client
                    var _client = dcAuth.dbAuthClientTBLs.FirstOrDefault(x => x.id == newRes.agentClientID.objToInt64());
                    if (_client == null)
                    {
                        _client = dcAuth.dbAuthClientTBLs.FirstOrDefault(x => x.pidAgent == agentID && x.contactEmail == Request.inquirer.emailAddress);
                        if (_client == null || Request.inquirer.emailAddress == "")
                        {
                            _client = new dbAuthClientTBL();
                            _client.pidAgent = agentID;
                            _client.uid = Guid.NewGuid();
                            _client.createdDate = DateTime.Now;
                            _client.createdUserID = 1;
                            _client.createdUserNameFull = "System";
                            _client.contactEmail = Request.inquirer.emailAddress;
                            dcAuth.Add(_client);

                            dcAuth.SaveChanges();
                            _client.code = _client.id.ToString().fillString("0", 6, false);
                            dcAuth.SaveChanges();

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
                            dcAuth.SaveChanges();
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

                    if ((CommonUtilities.getSYS_SETTING("rntRU_sendEmails") == "true" || CommonUtilities.getSYS_SETTING("rntRU_sendEmails").ToInt32() == 1))
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

}
public class ChnlRentalsUnitedClasses
{
    public class ListingClasses
    {
        public class DecimalUnit
        {
            public string Value { get { return ValueDecimal.HasValue ? (ValueDecimal + "").Replace(",", ".") : ""; } set { ValueDecimal = (value + "") != "" ? (value + "").Replace(".", ",").ToDecimal() : (decimal?)null; } }
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
                if (MinChar > 0 && txtElm.Value.Length < MinChar) txtElm.Value = txtElm.Value.fillString("-", MinChar, true);
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
                elm.Add(new XElement("externalId", externalId));
                elm.Add(new XElement("active", active));
                elm.Add(adContent.GetElement());
                if (featureValues.Count > 0)
                {
                    var featureValuesElm = new XElement("featureValues");
                    foreach (var tmp in featureValues)
                        featureValuesElm.Add(tmp.GetElement());
                    elm.Add(featureValuesElm);
                }
                elm.Add(location.GetElement());
                if (images.Count > 0)
                {
                    var imagesElm = new XElement("images");
                    foreach (var tmp in images)
                        imagesElm.Add(tmp.GetElement());
                    elm.Add(imagesElm);
                }
                if (units.Count > 0)
                {
                    var unitsElm = new XElement("units");
                    foreach (var tmp in units)
                        unitsElm.Add(tmp.GetElement());
                    elm.Add(unitsElm);
                }
                if (contacts.Count > 0)
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
            public int? loungeSeating { get; set; }
            public int maxSleep { get; set; }
            public int? maxSleepInBeds { get; set; }
            public string propertyType { get; set; }
            public MonetaryInformation unitMonetaryInformation { get; set; }
            public Texts unitName { get; set; }
            public List<RatePeriod> ratePeriods { get; set; }
            public Availability unitAvailability { get; set; }
            public Unit()
            {
                areaUnit = "METERS_SQUARED";
                bathroomDetails = new Texts();
                bathrooms = new List<Bathroom>();
                bedroomDetails = new Texts();
                bedrooms = new List<Bedroom>();
                description = new Texts();
                featuresDescription = new Texts();
                featureValues = new List<FeatureValue>();
                unitMonetaryInformation = new MonetaryInformation();
                unitName = new Texts();
                ratePeriods = new List<RatePeriod>();
                unitAvailability = new Availability();
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
                if (loungeSeating.HasValue)
                    elm.Add(new XElement("loungeSeating", loungeSeating));
                elm.Add(new XElement("maxSleep", maxSleep));
                if (maxSleepInBeds.HasValue)
                    elm.Add(new XElement("maxSleepInBeds", maxSleepInBeds));
                elm.Add(new XElement("propertyType", propertyType));
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
            public Texts description { get; set; }
            public string bathroomFeatureName { get; set; }
            public string bedroomFeatureName { get; set; }
            public Amenity()
            {
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("amenity");
                if (count.HasValue)
                    elm.Add(new XElement("count", count));
                if (description != null && description.HasValue())
                    elm.Add(description.GetElement("description"));
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
                if (!string.IsNullOrEmpty(roomSubType))
                    elm.Add(new XElement("roomSubType", roomSubType));
                if (name.HasValue())
                    elm.Add(name.GetElement("name"));
                if (note.HasValue())
                    elm.Add(note.GetElement("note"));
                if (amenities.Count > 0)
                {
                    var amenitiesElm = new XElement("amenities");
                    foreach (var tmp in amenities)
                        amenitiesElm.Add(tmp.GetElement());
                    elm.Add(amenitiesElm);
                }
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
                if (!string.IsNullOrEmpty(roomSubType))
                    elm.Add(new XElement("roomSubType", roomSubType));
                if (name.HasValue())
                    elm.Add(name.GetElement("name"));
                if (note.HasValue())
                    elm.Add(note.GetElement("note"));
                if (amenities.Count > 0)
                {
                    var amenitiesElm = new XElement("amenities");
                    foreach (var tmp in amenities)
                        amenitiesElm.Add(tmp.GetElement());
                    elm.Add(amenitiesElm);
                }
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
        public class Availability
        {
            public string availabilityDefault { get; set; }
            public string changeOverDefault { get; set; }
            public DateRange dateRange { get; set; }
            public int? maxStayDefault { get; set; }
            public int? minPriorNotifyDefault { get; set; }
            public int? minStayDefault { get; set; }
            public string stayIncrementDefault { get; set; }
            public AvailabilityConfiguration unitAvailabilityConfiguration { get; set; }
            public Availability()
            {
                availabilityDefault = "Q";
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
            public List<string> changeOver { get; set; } // A letter code for every day of the range. X=no action possible, C=check-in/out, O=check-out only, I=check-in only. A maximum of 3 years of availability can be given. There cannot be more days given than there are days in the dateRange. An example: CCIIOOXCCIIOOXCCIIOOXCCIIOOXCCIIOOXCCIIOOXCCIIO...
            public List<string> maxStay { get; set; } // Maximum stay is a list of comma-separated values 0 thru 999, with each value representing a day in the range and where 0 = "no max stay". An example: 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 …
            public List<string> minPriorNotify { get; set; } // Min prior notification is a list of comma-separated values 0 thru 999, with each value representing a day in the range and where 0 = "no min prior notification". An example: 2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2 …
            public List<string> minStay { get; set; } // Minimum stay is a list of comma-separated values 0 thru 999, with each value representing a day in the range and where 0 = "no min stay”. An example: 2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2 …
            public List<string> stayIncrement { get; set; } // The increment of stay for every day of the range. D=Day, M=Month, W=Week, Y=Year. Example 1: If minStay = 3 and stayIncrement = W, the min stay is 3 weeks and can increment by 1 week. Example 2: If minStay = 2 and stayIncrement = D, the min stay is 2 days and can increment by any amount of days, up to max, after that. An example: DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
            public AvailabilityConfiguration()
            {
                availability = new List<string>();
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

        public class unitAvailabilityEntities
        {
            public string listingExternalId { get; set; } // master id
            public string unitExternalId { get; set; }
            public Availability unitAvailability { get; set; }
            public unitAvailabilityEntities()
            {
                unitAvailability = new Availability();
            }
            public string GetXml()
            {
                XElement elm = new XElement("unitAvailabilityEntities");
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
                description = new Texts();
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
            /// <summary>
            /// to complete
            /// </summary>
            public Contact()
            {
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
        public PaymentCard paymentCard { get; set; }
        public string travelerSource { get; set; }
        public string commission { get; set; }
        public BookingRequest()
        {
            inquirer = new Inquirer();
            reservation = new Reservation();
            orderItemList = new List<OrderItem>();
            paymentCard = new PaymentCard();
        }
        public BookingRequest(string xmlData)
        {
            inquirer = new Inquirer();
            reservation = new Reservation();
            orderItemList = new List<OrderItem>();
            paymentCard = new PaymentCard();

            XDocument _resource = XDocument.Parse(xmlData);
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
            paymentCard = new PaymentCard(ds.Element("paymentCard"));
            travelerSource = (string)ds.Element("travelerSource") ?? "";
        }
    }
    public class BookingResponse
    {
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
            bookingResponseDetails.Add(inquirer.GetElement());
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
    public class BookingContentIndexRequest
    {
        public DateAndTime lastUpdatedDate { get; set; }
        public List<Advertiser> advertisers { get; set; }
        public BookingContentIndexRequest()
        {
            lastUpdatedDate = new DateAndTime();
            advertisers = new List<Advertiser>();
        }
        public BookingContentIndexRequest(string xmlData)
        {
            lastUpdatedDate = new DateAndTime();
            advertisers = new List<Advertiser>();
            if (xmlData == "") return;
            XDocument _resource = XDocument.Parse(xmlData);
            var ds = _resource.Element("bookingContentIndexRequest");
            lastUpdatedDate.Value = (string)ds.Element("lastUpdatedDate") ?? "";
            var advertisersElm = ds.Element("advertisers");
            if (advertisersElm != null && advertisersElm.HasElements && advertisersElm.Descendants("advertiser").Count() > 0)
                foreach (var advertiser in advertisersElm.Descendants("advertiser"))
                {
                    advertisers.Add(new Advertiser(advertiser));
                }
        }
    }
    public class BookingContentIndexResponse
    {
        public List<Advertiser> advertisers { get; set; }
        public BookingContentIndexResponse()
        {
            advertisers = new List<Advertiser>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("bookingContentIndex");
            if (advertisers.Count > 0)
            {
                var advertisersElm = new XElement("advertisers");
                foreach (var tmp in advertisers)
                    advertisersElm.Add(tmp.GetElement());
                root.Add(advertisersElm);
            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }
    public class ListingContentIndexResponse
    {
        public List<Advertiser> advertisers { get; set; }
        public ListingContentIndexResponse()
        {
            advertisers = new List<Advertiser>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("listingContentIndex");
            if (advertisers.Count > 0)
            {
                var advertisersElm = new XElement("advertisers");
                foreach (var tmp in advertisers)
                    advertisersElm.Add(tmp.GetElement());
                root.Add(advertisersElm);
            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }

    public class ListingRateContentResponse
    {
        public List<AdvertiserRate> advertisers { get; set; }
        public ListingRateContentResponse()
        {
            advertisers = new List<AdvertiserRate>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("listingContentIndex");
            if (advertisers.Count > 0)
            {
                var advertisersElm = new XElement("advertisers");
                foreach (var tmp in advertisers)
                    advertisersElm.Add(tmp.GetElement());
                root.Add(advertisersElm);
            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }

    public class ListingAvailabilityContentResponse
    {
        public List<AdvertiserAvailability> advertisers { get; set; }
        public ListingAvailabilityContentResponse()
        {
            advertisers = new List<AdvertiserAvailability>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("listingContentIndex");
            if (advertisers.Count > 0)
            {
                var advertisersElm = new XElement("advertisers");
                foreach (var tmp in advertisers)
                    advertisersElm.Add(tmp.GetElement());
                root.Add(advertisersElm);
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
        public List<BookingContentIndexEntry> bookingContentIndexEntryList { get; set; }
        public List<Inquirer> inquirers { get; set; }
        public List<Inquiry> inquiries { get; set; }
        public Advertiser()
        {
            listingContentIndexEntry = new List<ListingContentIndexEntry>();
            bookingContentIndexEntryList = new List<BookingContentIndexEntry>();
            inquirers = new List<Inquirer>();
            inquiries = new List<Inquiry>();
        }
        public Advertiser(XElement elm)
        {
            listingContentIndexEntry = new List<ListingContentIndexEntry>();
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

    public class AdvertiserRate
    {
        public string assignedId { get; set; }
        public List<ListingRateContentEntry> listingRateContentEntry { get; set; }
        public List<BookingContentIndexEntry> bookingContentIndexEntryList { get; set; }
        public List<Inquirer> inquirers { get; set; }
        public List<Inquiry> inquiries { get; set; }
        public AdvertiserRate()
        {
            listingRateContentEntry = new List<ListingRateContentEntry>();
            bookingContentIndexEntryList = new List<BookingContentIndexEntry>();
            inquirers = new List<Inquirer>();
            inquiries = new List<Inquiry>();
        }
        public AdvertiserRate(XElement elm)
        {
            listingRateContentEntry = new List<ListingRateContentEntry>();
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
            foreach (var tmp in listingRateContentEntry)
                elm.Add(tmp.GetElement());
            return elm;
        }
    }
    public class AdvertiserAvailability
    {
        public string assignedId { get; set; }
        public List<ListingAvailabilityContentEntry> listingAvailabilityContentEntry { get; set; }
        public List<BookingContentIndexEntry> bookingContentIndexEntryList { get; set; }
        public List<Inquirer> inquirers { get; set; }
        public List<Inquiry> inquiries { get; set; }
        public AdvertiserAvailability()
        {
            listingAvailabilityContentEntry = new List<ListingAvailabilityContentEntry>();
            bookingContentIndexEntryList = new List<BookingContentIndexEntry>();
            inquirers = new List<Inquirer>();
            inquiries = new List<Inquiry>();
        }
        public AdvertiserAvailability(XElement elm)
        {
            listingAvailabilityContentEntry = new List<ListingAvailabilityContentEntry>();
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
            foreach (var tmp in listingAvailabilityContentEntry)
                elm.Add(tmp.GetElement());
            return elm;
        }
    }
    public class ListingRateContentEntry
    {
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public bool active { get; set; }
        public string unitRateUrl { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }
        public ListingRateContentEntry()
        {
            active = true;
            lastUpdatedDate = new DateAndTime();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("listingContentIndexEntry");
            if (!string.IsNullOrEmpty(listingExternalId))
                elm.Add(new XElement("listingExternalId", listingExternalId));

            if (!string.IsNullOrEmpty(unitExternalId))
                elm.Add(new XElement("unitExternalId", unitExternalId));

            elm.Add(new XElement("active", active));

            if (lastUpdatedDate.Value != "")
                elm.Add(lastUpdatedDate.GetElement("lastUpdatedDate"));
            if (!string.IsNullOrEmpty(unitRateUrl))
                elm.Add(new XElement("unitRatesUrl", unitRateUrl));
            return elm;
        }
    }

    public class ListingAvailabilityContentEntry
    {
        public string listingExternalId { get; set; }
        public string unitExternalId { get; set; }
        public bool active { get; set; }
        public string unitAvailabilityUrl { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }
        public ListingAvailabilityContentEntry()
        {
            active = true;
            lastUpdatedDate = new DateAndTime();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("listingContentIndexEntry");
            if (!string.IsNullOrEmpty(listingExternalId))
                elm.Add(new XElement("listingExternalId", listingExternalId));

            if (!string.IsNullOrEmpty(unitExternalId))
                elm.Add(new XElement("unitExternalId", unitExternalId));

            elm.Add(new XElement("active", active));
            if (lastUpdatedDate.Value != "")
                elm.Add(lastUpdatedDate.GetElement("lastUpdatedDate"));
            if (!string.IsNullOrEmpty(unitAvailabilityUrl))
                elm.Add(new XElement("unitAvailabilityUrl", unitAvailabilityUrl));
            return elm;
        }
    }

    public class BookingContentIndexEntry
    {
        public bool active { get; set; }
        public string bookingUrl { get; set; }
        public DateAndTime lastUpdatedDate { get; set; }
        public BookingContentIndexEntry()
        {
            active = true;
            lastUpdatedDate = new DateAndTime();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("bookingContentIndexEntry");
            elm.Add(new XElement("active", active));
            if (!string.IsNullOrEmpty(bookingUrl))
                elm.Add(new XElement("bookingUrl", bookingUrl));
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
        public string travelerSourceHA { get; set; }
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

                if (elm.Descendants("travelerSource").FirstOrDefault() != null)
                    travelerSourceHA = (string)elm.Descendants("travelerSource").FirstOrDefault().Value;

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
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var list = dcChnl.dbRntChnlRentalsUnitedAcceptedPaymentFormTBLs.Where(x => x.isActive == 1).ToList();
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
                    if (acceptedPaymentForm.paymentFormType == "CHECK")
                    {
                        XElement paymentCardDescriptorElm = new XElement("paymentCheckDescriptor");
                        paymentCardDescriptorElm.Add(new XElement("paymentFormType", "CHECK"));
                        acceptedPaymentFormsElm.Add(paymentCardDescriptorElm);
                    }
                    if (acceptedPaymentForm.paymentFormType == "ECHECK")
                    {
                        XElement paymentCardDescriptorElm = new XElement("paymentECheckDescriptor");
                        paymentCardDescriptorElm.Add(new XElement("paymentFormType", "ECHECK"));
                        acceptedPaymentFormsElm.Add(paymentCardDescriptorElm);
                    }
                    if (acceptedPaymentForm.paymentFormType == "INVOICE")
                    {
                        XElement paymentCardDescriptorElm = new XElement("paymentInvoiceDescriptor");
                        paymentCardDescriptorElm.Add(new XElement("paymentFormType", "INVOICE"));
                        acceptedPaymentFormsElm.Add(paymentCardDescriptorElm);
                    }
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
                reservationCancellationPolicyElm.Add(cancellationPolicyItemListElm);
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
    public class PaymentCard
    {
        public string paymentFormType { get; set; } // CARD, CHECK, ECHECK, DEFERRED
        public Address billingAddress { get; set; }
        public string cvv { get; set; }
        public string expiration { get; set; } // MM/YYYY
        public string maskedNumber { get; set; } // Credit card number with the last 4 digits masked.
        public string nameOnCard { get; set; } // The person’s name on the credit card.
        public string number { get; set; } // The full credit card number.
        public string numberToken { get; set; } // The RentalsUnited token representing the credit card number.
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
    //public class ErrorList
    //{
    //    public string root { get; set; } // max 1000
    //    public List<Error> errorList { get; set; }
    //    public ErrorList(string Root)
    //    {
    //        root = Root;
    //        errorList = new List<Error>();
    //    }
    //    public void AddError(string ErrorType)
    //    {
    //        var error = new Error();
    //        error.errorType = ErrorType;
    //        errorList.Add(error);
    //    }
    //    private XElement GetElement()
    //    {
    //        XElement record = new XElement("errorList");
    //        foreach (Error error in errorList)
    //        {
    //            XElement errorElm = new XElement("error");
    //            if (error.count.HasValue)
    //                errorElm.Add(new XElement("count", error.count));
    //            if (!string.IsNullOrEmpty(error.dayOfWeek))
    //                errorElm.Add(new XElement("dayOfWeek", error.dayOfWeek));
    //            if (!string.IsNullOrEmpty(error.errorCode))
    //                errorElm.Add(new XElement("errorCode", error.errorCode));
    //            if (!string.IsNullOrEmpty(error.errorType))
    //                errorElm.Add(new XElement("errorType", error.errorType));
    //            if (!string.IsNullOrEmpty(error.errorMessage))
    //                errorElm.Add(new XElement("errorMessage", error.errorMessage));
    //            record.Add(errorElm);
    //        }
    //        return record;
    //    }
    //    public string GetXml()
    //    {
    //        return "<" + root + ">" + GetElement() + "</" + root + ">";
    //    }
    //    public static string GetErrorXml(string ErrorType, string root)
    //    {
    //        XElement record = new XElement("errorList");
    //        XElement errorElm = new XElement("error");
    //        errorElm.Add(new XElement("ErrorType", ErrorType));
    //        record.Add(errorElm);
    //        return "<" + root + ">" + record + "</" + root + ">";
    //    }
    //}

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
            return "<" + root + ">" + GetElement() + "</" + root + ">";
        }
        public static string GetErrorXml(string ErrorType, string root)
        {
            XElement record = new XElement("errorList");
            XElement errorElm = new XElement("error");
            errorElm.Add(new XElement("ErrorType", ErrorType));
            record.Add(errorElm);
            return "<" + root + ">" + record + "</" + root + ">";
        }
    }

    public class Amount
    {
        public string Currency { get; set; }
        public string Value { get { return ValueDecimal.HasValue ? (ValueDecimal + "").Replace(",", ".") : ""; } set { ValueDecimal = (value + "") != "" ? (value + "").Replace(".", ",").ToDecimal() : (decimal?)null; } }
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

        public decimal PriceRU { get; set; }

        public PriceListPerDates(DateTime dtStart, DateTime dtEnd, int minNights, decimal priceRU)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            MinNights = minNights;
            PriceRU = priceRU;
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

        public bool HasSamePricesRU(PriceListPerDates compareWith)
        {
            if (MinNights != compareWith.MinNights) return false;
            if (PriceRU != compareWith.PriceRU) return false;

            if (DtEnd.AddDays(1) != compareWith.DtStart) return false;

            return true;
        }
    }
}
public static class ChnlRentalsUnitedExts
{
}
