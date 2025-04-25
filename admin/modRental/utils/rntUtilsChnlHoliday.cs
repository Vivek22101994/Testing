using ModRental;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Adverts = RentalInRome.WsChnlHoliday_Adverts;
using ServiceRates = RentalInRome.WsChnlHoliday_ServiceRates;
using Photos = RentalInRome.WsChnlHoliday_Photos;
using Enquiries = RentalInRome.WsChnlHoliday_Enquiries;
using RentalInRome.data;
using System.Threading;


public class ChnlHolidayUtils
{
    public static void SetTimers()
    {
        timerChnlHolidayImport = new System.Timers.Timer();
        timerChnlHolidayImport.Interval = (1000 * 60 * 2); // first after 2 mins
        timerChnlHolidayImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlHolidayImport_Elapsed);
        timerChnlHolidayImport.Start();
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
            using (DCmodRental dc = new DCmodRental())
            {
                dc.Delete(dc.dbRntChnlHolidayRequestLOGs.Where(x => x.logDateTime <= dt));
                dc.SaveChanges();
            }
            addLog("CLEAR LOG", "till " + dt, "", "");
        }
        public ClearLog_process()
        {
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayUtils.ClearLog_process");

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

    static System.Timers.Timer timerChnlHolidayImport;
    static void timerChnlHolidayImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        if (RentalInRome.Global.isOnDev && CommonUtilities.getSYS_SETTING("rntChnlHolidayImportOnDev") != "true" && CommonUtilities.getSYS_SETTING("rntChnlHolidayImportOnDev").ToInt32() != 1) return;
        int rntChnlHolidayImportEachMins = CommonUtilities.getSYS_SETTING("rntChnlHolidayImportEachMins").ToInt32();
        if (rntChnlHolidayImportEachMins == 0) rntChnlHolidayImportEachMins = 15;
        ChnlHolidayImport.Enquiries_all(DateTime.Now.AddDays(-1));
        timerChnlHolidayImport.Dispose();
        timerChnlHolidayImport = new System.Timers.Timer();
        timerChnlHolidayImport.Interval = (1000 * 60 * 15); // each 15 mins
        timerChnlHolidayImport.Elapsed += new System.Timers.ElapsedEventHandler(timerChnlHolidayImport_Elapsed);
        timerChnlHolidayImport.Start();
    }

    public static dbRntChnlHolidayOwnerTBL ownerTbl { get; set; }
    protected magaRental_DataContext DC_RENTAL;
    public static string SendRequest(String requesUrl, String requestType, String requestContent)
    {
        try
        {
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);
            obj.ContentType = "text/xml";
            obj.Method = "POST";

            //set new TLS protocol 1.1
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

            Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestContent);
            obj.ContentLength = bytes.Length;
            Stream os = obj.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            String strRSString = _Answer.ReadToEnd().ToString();
            addLog(requesUrl, requestType, requestContent, strRSString);
            return strRSString;

        }
        catch (Exception ex)
        {
            addLog(requesUrl, "ERROR:" + requestType, requestContent, ex.ToString());
            return "";
        }
    }
    public static void addLog(string requesUrl, string requestType, string requestContent, string responseContent)
    {
        try
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var item = new dbRntChnlHolidayRequestLOG();
                item.uid = Guid.NewGuid();
                item.requesUrl = requesUrl;
                item.requestType = requestType;
                item.requestContent = requestContent;
                item.responseContent = responseContent;
                item.logDateTime = DateTime.Now;
                dc.Add(item);
                dc.SaveChanges();
            }
        }
        catch (Exception Ex)
        {
            ErrorLog.addLog("", "rntUtilsHolidayLettings.addLog", Ex.ToString() + "");
        }
    }
    public static dbRntChnlHolidayEstateTB copyEstateToHoliday(int id)
    {
        using (DCmodRental dc = new DCmodRental())
        {
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            var chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == id);
            if (chnlTb == null)
            {
                chnlTb = new ModRental.dbRntChnlHolidayEstateTB();
                chnlTb.id = id;
                dc.Add(chnlTb);
                dc.SaveChanges();
            }
            RNT_TB_ESTATE currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id);
            if (currEstate != null)
            {

                RNT_LN_ESTATE currEstateLN = DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == 2);
                if (currEstateLN != null)
                {
                    chnlTb.homeName = currEstateLN.title;
                    chnlTb.homeSummary = currEstateLN.summary.htmlDecode();
                    chnlTb.homeDescription = currEstateLN.description.htmlDecode();
                }
                chnlTb.loc_address = currEstate.loc_address;
                chnlTb.loc_zip_code = currEstate.loc_zip_code;
                chnlTb.nights_min = currEstate.nights_min;
                chnlTb.nights_max = currEstate.nights_max;
                chnlTb.num_rooms_bed = currEstate.num_rooms_bed;
                chnlTb.num_rooms_bath = currEstate.num_rooms_bath;
                chnlTb.num_family_bath = currEstate.num_rooms_bath;
                chnlTb.num_persons_min = currEstate.num_persons_min;
                chnlTb.num_persons_max = currEstate.num_persons_max;
                chnlTb.num_bed_double = currEstate.num_bed_double;
                chnlTb.num_bed_single = currEstate.num_bed_single;
                chnlTb.num_bed_sofa = currEstate.num_sofa_double + currEstate.num_sofa_double;
                chnlTb.is_google_maps = currEstate.is_google_maps;
                chnlTb.google_maps = currEstate.google_maps;
                chnlTb.homeType = currEstate.category;
                dc.Add(chnlTb);
                dc.SaveChanges();

                //amenities
                List<int> lstExtras = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == id && x.is_HomeAway == 0).Select(x => x.pid_config).ToList();
                if (lstExtras != null && lstExtras.Count > 0)
                {
                    if (lstExtras.Contains(18))
                        chnlTb.has_air_condition = true;
                    //if(lstExtras.Contains(8))
                    //    chnlTb.has_pri=1;
                    if (lstExtras.Contains(22))
                        chnlTb.has_internet_access = true;
                    if (lstExtras.Contains(10))
                        chnlTb.has_wifi = true;
                    if (lstExtras.Contains(9))
                        chnlTb.has_terrace = true;
                    if (lstExtras.Contains(1))
                        chnlTb.has_barbecue = true;
                    if (lstExtras.Contains(20))
                        chnlTb.has_washing_machine = true;
                    //if (lstExtras.Contains(16))
                    //    chnlTb.has_swimming_pool = 1;
                    if (lstExtras.Contains(14))
                        chnlTb.has_sea_view = true;
                    if (lstExtras.Contains(7))
                        chnlTb.has_parking = true;
                    DC_RENTAL.SubmitChanges();


                }
                return chnlTb;

            }
            else
                return null;
        }

    }
    public static int PropertyHasPhotos(int IdEstate)
    {
        try
        {
            long agentID = 0;
            dbRntChnlHolidayEstateTB chnlTb;
            using (DCmodRental dc = new DCmodRental())
            {
                var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdatePhotos_process", "Agent for HolidayLettings was not found or not active");
                    return 0;
                }
                var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdatePhotos_process", "idEstate:" + IdEstate + " not found or not active");
                    return 0;
                }
                chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (chnlTb == null || chnlTb.homeId.ToInt64() == 0)
                {
                    //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdatePhotos_process", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                    return 0;
                }
                ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
                if (ownerTbl == null)
                {
                    ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                    if (ownerTbl == null)
                    {
                        ErrorLog.addLog("", "ChnlHolidayUpdate.UpdatePhotos_process", "idEstate:" + IdEstate + " NO OWNER");
                        return 0;
                    }
                }

                //set new TLS protocol 1.1 before importation
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

                Photos.photos photoClient = new Photos.photos();
                // Create your fetch photos request
                Photos.FetchPicRequest fetchPhotoRequest = new Photos.FetchPicRequest();
                fetchPhotoRequest.HomeId = chnlTb.homeId.objToInt64();
                // Call FetchPhotos method with parameters advertiserId, key and fetchPhotoRequest.
                // That will fetch information on all active photos for a given advert
                Photos.FetchWebServiceResponseOfPicDto fetchResponse = photoClient.FetchPhotos(ownerTbl.ownerId, ownerTbl.ownerKey, fetchPhotoRequest);
                // If the response is successful, display data
                var ErrorString = "";
                if (fetchResponse.WebServiceResponse.Success == true)
                {
                    return fetchResponse.FetchedData.Count();
                }
                else
                {
                    ErrorString = "";
                    foreach (string msg in fetchResponse.WebServiceResponse.Messages)
                    {
                        ErrorString += msg + "\r\n";
                    }
                    ChnlHolidayUtils.addLog("", "PropertyHasPhotos", "IdEstate:" + IdEstate + " code:" + fetchResponse.WebServiceResponse.ErrorCode, ErrorString);
                    return 0;
                }
            }
        }
        catch (Exception Ex)
        {
            ChnlHolidayUtils.addLog("", "PropertyHasPhotos", "IdEstate:" + IdEstate, Ex.ToString());
            return 0;
        }
    }

}
public static class ChnlHolidayProps
{
    public static string IdAdMedia = "HL";
    public static dbRntChnlHolidayEstateRateChangesRL CreateRateChange(this dbRntChnlHolidayEstateTB source, DateTime dt, dbRntChnlHolidayEstateRateChangesRL rateChange)
    {
        dbRntChnlHolidayEstateRateChangesRL currTbl = new dbRntChnlHolidayEstateRateChangesRL();
        currTbl.changeDate = dt;

        currTbl.weekday_changeAmount = rateChange != null && rateChange.weekday_changeIsDiscount >= 0 ? rateChange.weekday_changeAmount : source.weekday_changeAmount.objToInt32();
        currTbl.weekend_changeAmount = rateChange != null && rateChange.weekend_changeIsDiscount >= 0 ? rateChange.weekend_changeAmount : source.weekend_changeAmount.objToInt32();
        currTbl.weekly_changeAmount = rateChange != null && rateChange.weekly_changeIsDiscount >= 0 ? rateChange.weekly_changeAmount : source.weekly_changeAmount.objToInt32();

        currTbl.weekday_changeIsDiscount = rateChange != null && rateChange.weekday_changeIsDiscount >= 0 ? rateChange.weekday_changeIsDiscount : source.weekday_changeIsDiscount.objToInt32();
        currTbl.weekend_changeIsDiscount = rateChange != null && rateChange.weekend_changeIsDiscount >= 0 ? rateChange.weekend_changeIsDiscount : source.weekend_changeIsDiscount.objToInt32();
        currTbl.weekly_changeIsDiscount = rateChange != null && rateChange.weekly_changeIsDiscount >= 0 ? rateChange.weekly_changeIsDiscount : source.weekly_changeIsDiscount.objToInt32();

        currTbl.weekday_changeIsPercentage = rateChange != null && rateChange.weekday_changeIsDiscount >= 0 ? rateChange.weekday_changeIsPercentage : source.weekday_changeIsPercentage.objToInt32();
        currTbl.weekend_changeIsPercentage = rateChange != null && rateChange.weekend_changeIsDiscount >= 0 ? rateChange.weekend_changeIsPercentage : source.weekend_changeIsPercentage.objToInt32();
        currTbl.weekly_changeIsPercentage = rateChange != null && rateChange.weekly_changeIsDiscount >= 0 ? rateChange.weekly_changeIsPercentage : source.weekly_changeIsPercentage.objToInt32();
        return currTbl;
    }
}
public class ChnlHolidayImport
{
    public static long PropertyNew(int IdEstate, ref string errorString)
    {
        errorString = "";
        long agentID = 0;
        dbRntChnlHolidayOwnerTBL ownerTbl;
        dbRntChnlHolidayEstateTB chnlTb;
        using (DCmodRental dc = new DCmodRental())
        {
            var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
            if (agentTbl != null)
                agentID = agentTbl.id;
            if (agentID == 0)
            {
                //ErrorLog.addLog("", "ChnlHolidayImport.PropertyNew", "Agent for HolidayLettings was not found or not active");
                return 0;
            }
            var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currEstate == null)
            {
                //ErrorLog.addLog("", "ChnlHolidayImport.PropertyNew", "idEstate:" + IdEstate + " not found or not active");
                return 0;
            }
            chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
            if (chnlTb == null)
            {
                //ErrorLog.addLog("", "ChnlHolidayImport.PropertyNew", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                return 0;
            }
            ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
            if (ownerTbl == null)
            {
                ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                if (ownerTbl == null)
                {
                    ErrorLog.addLog("", "ChnlHolidayImport.PropertyNew", "idEstate:" + IdEstate + " NO OWNER");
                    return 0;
                }
            }

            //set new TLS protocol 1.1 before importation
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

            Adverts.adverts templateClient = new Adverts.adverts();
            Adverts.NewAdvertWebServiceResponse advertResponse = templateClient.NewStandaloneAdvert(ownerTbl.ownerId, ownerTbl.ownerKey, "" + IdEstate);
            if (advertResponse.WebServiceResponse.Success == true)
            {
                return advertResponse.HomeId.objToInt64();
            }
            else
            {
                string errorMsgs = "";
                foreach (string msg in advertResponse.WebServiceResponse.Messages)
                {
                    errorString += msg + "<br/>";
                }
                return 0;
            }
        }
    }
    private class PropertyInfoFull_process
    {
        public int IdEstate { get; set; }
        dbRntChnlHolidayOwnerTBL ownerTbl { get; set; }
        dbRntChnlHolidayEstateTB chnlTb { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        void doThread()
        {
            try
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoFull_process", "Agent for HolidayLettings was not found or not active");
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoFull_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                    chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || chnlTb.homeId.ToInt64() == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoFull_process", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                        return;
                    }
                    ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
                    if (ownerTbl == null)
                    {
                        ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                        if (ownerTbl == null)
                        {
                            ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoFull_process", "idEstate:" + IdEstate + " NO OWNER");
                            return;
                        }
                    }

                    //set new TLS protocol 1.1 before importation
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

                    Adverts.adverts siteClient = new Adverts.adverts();
                    Adverts.FetchRentalUnitRequest FetchUnit = new Adverts.FetchRentalUnitRequest();
                    // HomeId is mandatory
                    FetchUnit.HomeId = chnlTb.homeId.objToInt64();
                    // Call FetchUnit method with parameters advertiserId, key and FetchUnit. That will return all unit data for the give home Id
                    var unitResponse = siteClient.FetchUnit(ownerTbl.ownerId, ownerTbl.ownerKey, FetchUnit);
                    if (unitResponse.WebServiceResponse.Success == true)
                    {
                        if (unitResponse.FetchedData.Count() == 0)
                        {
                            ErrorString += "UNIT NOT FOUND" + "<br/>";
                        }
                        else
                        {
                            Adverts.RentalUnit unit = unitResponse.FetchedData[0];
                            chnlTb.ActiveState = unit.ActiveState;
                            chnlTb.SiteId = unit.SiteId;
                            chnlTb.homeName = unit.HomeName;

                            if (unit.HomeType == "Apartment")
                                chnlTb.homeType = "apt";
                            else if (unit.HomeType == "Villa")
                                chnlTb.homeType = "villa";
                            else if (unit.HomeType == "House")
                                chnlTb.homeType = "residence";
                            chnlTb.num_rooms_bed = unit.NumberOfBedrooms;
                            chnlTb.num_persons_max = unit.SleepsMax;
                            chnlTb.homeDescription = unit.HomeDescription;
                            chnlTb.homeSummary = unit.HomeSummary;

                            chnlTb.num_bed_double = unit.NumberDoubleBeds;
                            chnlTb.num_bed_single = unit.NumberSingleBeds;
                            chnlTb.num_cot = unit.NumberCots;
                            chnlTb.num_bed_sofa = unit.NumberSofaBeds;
                            chnlTb.num_family_bath = unit.NumberBathrooms;
                            chnlTb.num_en_suite = unit.NumberEnSuiteRooms;
                            chnlTb.num_shower_rooms = unit.NumberShowerRooms;
                            chnlTb.num_lounge_seats = unit.SeatingInLounge;
                            chnlTb.num_dining_seats = unit.SeatingForDining;

                            //facilities        
                            //indoor facilities
                            chnlTb.has_cooker = unit.HasCooker;
                            chnlTb.has_tv = unit.HasTelevision;
                            chnlTb.has_fireplace = unit.HasFireplace;
                            chnlTb.has_fridge = unit.HasFridge;
                            chnlTb.has_satelite_tv = unit.HasSatelliteTV;
                            chnlTb.has_central_heating = unit.HasCentralHeating;
                            chnlTb.has_freezer = unit.HasFreezer;
                            chnlTb.has_video = unit.HasVideo;
                            chnlTb.has_air_condition = unit.HasAirConditioning;
                            chnlTb.has_microwave = unit.HasMicrowave;
                            chnlTb.has_dvd = unit.HasDvd;
                            chnlTb.has_linen = unit.LinenProvided;
                            chnlTb.has_toaster = unit.HasToaster;
                            chnlTb.has_towel = unit.TowelsProvided;
                            chnlTb.has_kettle = unit.HasKettle;
                            chnlTb.has_internet_access = unit.HasInternetAccess;
                            chnlTb.has_sauna = unit.HasSauna;
                            chnlTb.has_dish_washer = unit.HasDishwasher;
                            chnlTb.has_wifi = unit.HasWiFi;
                            chnlTb.has_gym = unit.HasGym;
                            chnlTb.has_cloth_dryer = unit.HasClothesDryer;
                            chnlTb.has_fax = unit.HasFax;
                            chnlTb.has_pool_snooker = unit.HasPoolSnooker;
                            chnlTb.has_iron = unit.HasIron;
                            chnlTb.has_hair_dryer = unit.HasHairdryer;
                            chnlTb.has_games_room = unit.HasGamesRoom;
                            chnlTb.has_high_chair = unit.HasHighchair;
                            chnlTb.has_safe = unit.HasSafe;
                            chnlTb.staffed_property = unit.StaffedProperty;
                            chnlTb.has_telephone = unit.HasTelephone;
                            chnlTb.has_washing_machine = unit.HasWashingMachine;
                            chnlTb.has_pingpong = unit.HasPingPong;
                            chnlTb.has_sea_view = unit.HasSeaView;
                            chnlTb.has_shared_garden = unit.HasSharedGarden;


                            //outdoor facilities
                            chnlTb.has_shared_outdoor_heated = unit.HasPoolSharedOutdoorHeated;
                            chnlTb.has_shared_outdoor_unheated = unit.HasPoolSharedOutdoorUnheated;
                            chnlTb.has_private_outdoor_heated = unit.HasPoolPrivateOutdoorHeated;
                            chnlTb.has_private_outdoor_unheated = unit.HasPoolPrivateOutdoorUnheated;
                            chnlTb.has_pool_private_indoor = unit.HasPoolPrivateIndoor;
                            chnlTb.has_shared_indoor = unit.HasPoolSharedIndoor;
                            chnlTb.has_pool_for_children = unit.HasPoolForChildren;
                            chnlTb.has_jacuzzi_hot_tub = unit.HasJacuzziHotTub;
                            chnlTb.has_shared_tennis_court = unit.HasSharedTennisCourt;
                            chnlTb.has_private_tennis_court = unit.HasPrivateTennisCourt;
                            chnlTb.has_private_garden = unit.HasPrivateGarden;
                            chnlTb.has_climbing_frame = unit.HasClimbingFrame;
                            chnlTb.has_swwing_set = unit.HasSwingSet;
                            chnlTb.has_trampoline = unit.HasTrampoline;
                            chnlTb.has_barbecue = unit.HasBarbecue;
                            chnlTb.has_private_fishing = unit.HasPrivateFishing;
                            chnlTb.has_bicycle = unit.BicyclesAvailable;
                            chnlTb.has_parking = unit.ParkingAvailable == Adverts.ParkingAvailable.Available || unit.ParkingAvailable == Adverts.ParkingAvailable.AvailableAndSecure;
                            chnlTb.has_secure_parking = unit.ParkingAvailable == Adverts.ParkingAvailable.AvailableAndSecure;
                            chnlTb.has_boat = unit.HasBoat;
                            chnlTb.has_solanium_roof_terrace = unit.HasSolariumRoofTerrace;
                            chnlTb.has_terrace = unit.HasBalconyOrTerrace;
                            chnlTb.available_corporate = unit.AvailableForCorporate ? 1 : 0;
                            chnlTb.available_henstag = unit.AvailableForHenStag ? 1 : 0;
                            chnlTb.available_house_swap = unit.AvailableForHouseSwap ? 1 : 0;
                            chnlTb.available_longlet = unit.AvailableForLongLet ? 1 : 0;
                            chnlTb.available_shortbreak = unit.AvailableForShortBreaks ? 1 : 0;


                            if (unit.SuitableForChildren == Adverts.SuitableForChildren.NotSuitable)
                                chnlTb.suitable_children = "0";
                            else if (unit.SuitableForChildren == Adverts.SuitableForChildren.NotForUnder5s)
                                chnlTb.suitable_children = "1";
                            else if (unit.SuitableForChildren == Adverts.SuitableForChildren.Yes)
                                chnlTb.suitable_children = "2";


                            if (unit.RestrictedMobility == Adverts.RestrictedMobility.NotSuitable)
                                chnlTb.restricted_mobility = "0";
                            else if (unit.RestrictedMobility == Adverts.RestrictedMobility.YesWithLiftAccess)
                                chnlTb.restricted_mobility = "1";
                            else if (unit.RestrictedMobility == Adverts.RestrictedMobility.Yes)
                                chnlTb.restricted_mobility = "2";

                            if (unit.AllowPets == Adverts.AllowPets.No)
                                chnlTb.allow_pets = "0";
                            else if (unit.AllowPets == Adverts.AllowPets.PleaseEnquire)
                                chnlTb.allow_pets = "1";
                            else if (unit.AllowPets == Adverts.AllowPets.Yes)
                                chnlTb.allow_pets = "2";

                            if (unit.WheelchairUsers == Adverts.WheelchairUsers.NotSuitable)
                                chnlTb.wheelchair_users = "0";
                            else if (unit.WheelchairUsers == Adverts.WheelchairUsers.Accessible)
                                chnlTb.wheelchair_users = "1";
                            else if (unit.WheelchairUsers == Adverts.WheelchairUsers.AccessibleAndAdapted)
                                chnlTb.wheelchair_users = "2";

                            chnlTb.allow_smoking = unit.AllowSmoking ? 1 : 0;
                            dc.SaveChanges();

                            if (1 == 2)
                            {
                                Photos.photos photoClient = new Photos.photos();
                                // Create your fetch photos request
                                Photos.FetchPicRequest fetchPhotoRequest = new Photos.FetchPicRequest();
                                fetchPhotoRequest.HomeId = chnlTb.homeId.objToInt64();
                                // Call FetchPhotos method with parameters advertiserId, key and fetchPhotoRequest.
                                // That will fetch information on all active photos for a given advert
                                Photos.FetchWebServiceResponseOfPicDto fetchResponse = photoClient.FetchPhotos(ownerTbl.ownerId, ownerTbl.ownerKey, fetchPhotoRequest);
                                // If the response is successful, display data
                                if (fetchResponse.WebServiceResponse.Success == true)
                                {
                                    var DC_RENTAL = maga_DataContext.DC_RENTAL;
                                    var tmpList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == "chnlHL").ToList();
                                    foreach (var tmp in tmpList)
                                    {
                                        try
                                        {
                                            File.Delete(Path.Combine(App.SRP, tmp.img_banner));
                                            File.Delete(Path.Combine(App.SRP, tmp.img_thumb));
                                        }
                                        catch (Exception ex) { }
                                    }
                                    DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteAllOnSubmit(tmpList);
                                    DC_RENTAL.SubmitChanges();
                                    int seq = 1;
                                    foreach (Photos.Pic photoData in fetchResponse.FetchedData)
                                    {
                                        RNT_RL_ESTATE_MEDIA currImg = new RNT_RL_ESTATE_MEDIA();
                                        currImg.sequence = seq;
                                        currImg.pid_estate = IdEstate;
                                        currImg.code = photoData.Caption;
                                        currImg.type = "chnlHL";
                                        currImg.img_banner = photoData.RelativePathToPic;
                                        currImg.img_thumb = photoData.RelativePathToPic;
                                        DC_RENTAL.RNT_RL_ESTATE_MEDIAs.InsertOnSubmit(currImg);
                                        DC_RENTAL.SubmitChanges();
                                        seq++;
                                    }
                                }
                                else
                                {
                                    ErrorString = "";
                                    foreach (string msg in fetchResponse.WebServiceResponse.Messages)
                                    {
                                        ErrorString += msg + "\r\n";
                                    }
                                    ChnlHolidayUtils.addLog("", "ERROR:ChnlHolidayImport.PropertyInfoFull_process", "IdEstate:" + IdEstate + " code:" + fetchResponse.WebServiceResponse.ErrorCode, ErrorString);
                                }
                            }
                            ChnlHolidayUtils.addLog("", "ChnlHolidayImport.PropertyInfoFull_process", "IdEstate:" + IdEstate + " updated successfully", "");
                        }
                    }
                    else
                    {
                        ErrorString = "";
                        foreach (string msg in unitResponse.WebServiceResponse.Messages)
                        {
                            ErrorString += msg + "<br/>";
                        }
                        ChnlHolidayUtils.addLog("", "ERROR:ChnlHolidayImport.PropertyInfoFull_process", "IdEstate:" + IdEstate + " code:" + unitResponse.WebServiceResponse.ErrorCode, ErrorString);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoFull_process IdEstate:" + IdEstate, ex.ToString());
            }
        }

        public PropertyInfoFull_process(int idEstate, string host, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Host = host;
            ErrorString = "";
            if (backgroundProcess)
            {
                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayImport.PropertyInfoFull_process idEstate:" + idEstate);

                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();

            }
            else
                doThread();
        }
    }
    public static string PropertyInfoFull_start(int idEstate, string host)
    {
        PropertyInfoFull_process _tmp = new PropertyInfoFull_process(idEstate, host, false);
        return _tmp.ErrorString;
    }

    private class PropertyInfoActiveState_process
    {
        public int IdEstate { get; set; }
        dbRntChnlHolidayOwnerTBL ownerTbl { get; set; }
        dbRntChnlHolidayEstateTB chnlTb { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        void doThread()
        {
            try
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoActiveState_process", "Agent for HolidayLettings was not found or not active");
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoActiveState_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                    chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || chnlTb.homeId.ToInt64() == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoActiveState_process", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                        return;
                    }
                    ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
                    if (ownerTbl == null)
                    {
                        ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                        if (ownerTbl == null)
                        {
                            ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoActiveState_process", "idEstate:" + IdEstate + " NO OWNER");
                            return;
                        }
                    }

                    //set new TLS protocol 1.1 before importation
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

                    Adverts.adverts siteClient = new Adverts.adverts();
                    Adverts.FetchRentalUnitRequest FetchUnit = new Adverts.FetchRentalUnitRequest();
                    // HomeId is mandatory
                    FetchUnit.HomeId = chnlTb.homeId.objToInt64();
                    // Call FetchUnit method with parameters advertiserId, key and FetchUnit. That will return all unit data for the give home Id
                    var unitResponse = siteClient.FetchUnit(ownerTbl.ownerId, ownerTbl.ownerKey, FetchUnit);
                    if (unitResponse.WebServiceResponse.Success == true)
                    {
                        if (unitResponse.FetchedData.Count() == 0)
                        {
                            ErrorString += "UNIT NOT FOUND" + "<br/>";
                        }
                        else
                        {
                            Adverts.RentalUnit unit = unitResponse.FetchedData[0];
                            chnlTb.ActiveState = unit.ActiveState;
                            chnlTb.SiteId = unit.SiteId;
                            dc.SaveChanges();
                        }
                    }
                    else
                    {
                        // If the response failed, return all the errors returned
                        foreach (string msg in unitResponse.WebServiceResponse.Messages)
                        {
                            ErrorString += msg + "<br/>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHolidayImport.PropertyInfoActiveState_process IdEstate:" + IdEstate, ex.ToString());
            }
        }

        public PropertyInfoActiveState_process(int idEstate, string host, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Host = host;
            ErrorString = "";
            if (backgroundProcess)
            {
                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayImport.PropertyInfoActiveState_process idEstate:" + idEstate);

                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();

            }
            else
                doThread();
        }
    }
    public static string PropertyInfoActiveState_start(int idEstate, string host)
    {
        PropertyInfoActiveState_process _tmp = new PropertyInfoActiveState_process(idEstate, host, false);
        return _tmp.ErrorString;
    }

    private class Enquiries_process
    {
        public int IdEstate { get; set; }
        dbRntChnlHolidayOwnerTBL ownerTbl { get; set; }
        dbRntChnlHolidayEstateTB chnlTb { get; set; }
        public long agentID { get; set; }
        DateTime? FromDate { get; set; }
        public string ErrorString { get; set; }
        void doThread()
        {
            try
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayImport.Enquiries_process", "Agent for HolidayLettings was not found or not active");
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayImport.Enquiries_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                    chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || chnlTb.homeId.ToInt64() == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayImport.Enquiries_process", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                        return;
                    }
                    ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
                    if (ownerTbl == null)
                    {
                        ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                        if (ownerTbl == null)
                        {
                            ErrorLog.addLog("", "ChnlHolidayImport.Enquiries_process", "idEstate:" + IdEstate + " NO OWNER");
                            return;
                        }
                    }

                    //set new TLS protocol 1.1 before importation
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

                    Enquiries.enquiries enquiryClient = new Enquiries.enquiries();
                    // Create your fetch enquiry request
                    Enquiries.FetchEnquiriesRequest FetchEnquiry = new Enquiries.FetchEnquiriesRequest();
                    FetchEnquiry.HomeId = chnlTb.homeId.objToInt64();
                    FetchEnquiry.FromDate = FromDate;
                    FetchEnquiry.FetchedEnquiriesLimit = 99;
                    // Call FetchEnquiries method with parameters advertiserId, key and FetchEnquiry.
                    // That will return all enquiry details that belong to the given home from a given date
                    var enquiryResponse = enquiryClient.FetchEnquiries(ownerTbl.ownerId, ownerTbl.ownerKey, FetchEnquiry);
                    if (enquiryResponse.WebServiceResponse.Success == true)
                    {
                        int countCreated = 0;
                        int countUpdated = 0;
                        int countCanceled = 0;
                        string enquiryData = "";
                        using (var dcOld = maga_DataContext.DC_RENTAL)
                            foreach (var enquiry in enquiryResponse.FetchedData)
                            {

                                var _request = dcOld.RNT_TBL_REQUEST.FirstOrDefault(x => x.chnlRefId == enquiry.EnquiryId + "" && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                                if (_request == null)
                                {
                                    bool alternateOld = true;
                                    _request = new RNT_TBL_REQUEST();
                                    _request.IdAdMedia = ChnlHolidayProps.IdAdMedia;
                                    _request.IdLink = enquiry.Source;
                                    _request.chnlRefId = enquiry.EnquiryId + "";
                                    _request.pid_lang = App.DefLangID;
                                    string _mailBody = "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
                                    _request.name_first = enquiry.FromName;
                                    _request.name_last = "";
                                    _request.name_full = enquiry.FromName;
                                    _mailBody += MailingUtilities.addMailRow("Nome/Cognome", _request.name_full, alternateOld, out alternateOld, false, false, false);
                                    _request.phone = enquiry.FromTelephone;
                                    _mailBody += MailingUtilities.addMailRow("Telefono", "" + _request.phone, alternateOld, out alternateOld, false, false, false);
                                    _request.email = enquiry.FromEmail;
                                    _mailBody += MailingUtilities.addMailRow("Email", "" + _request.email, alternateOld, out alternateOld, false, false, false);


                                    // start area
                                    string _area = "";
                                    _request.request_area = _area;
                                    _mailBody += MailingUtilities.addMailRow("e/o zona", "" + _request.request_area, alternateOld, out alternateOld, false, false, false);
                                    // end area
                                    var country_id = 0;
                                    var country_title = "";
                                    _request.request_country = country_title;
                                    _mailBody += MailingUtilities.addMailRow("Paese (Location)", "" + _request.request_country, alternateOld, out alternateOld, false, false, false);
                                    _request.request_date_start = enquiry.HolidayStartDate;
                                    _mailBody += MailingUtilities.addMailRow("Check-In", "" + _request.request_date_start.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
                                    _request.request_date_end = enquiry.HolidayEndDate;
                                    _mailBody += MailingUtilities.addMailRow("Check-Out", "" + _request.request_date_end.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
                                    _request.request_date_is_flexible = 0;
                                    _request.request_adult_num = enquiry.GuestsAdults;
                                    _mailBody += MailingUtilities.addMailRow("num. Adulti", "" + _request.request_adult_num, alternateOld, out alternateOld, false, false, false);
                                    _request.request_child_num = enquiry.GuestsChildren;
                                    _mailBody += MailingUtilities.addMailRow("num. Bambini", "" + _request.request_child_num, alternateOld, out alternateOld, false, false, false);
                                    _request.request_transport = "";
                                    _mailBody += MailingUtilities.addMailRow("Trasporto", "" + _request.request_transport, alternateOld, out alternateOld, false, false, false);
                                    string _price_range = "";
                                    _request.request_price_range = _price_range;
                                    _mailBody += MailingUtilities.addMailRow("e/o con prezzo", "" + _request.request_price_range, alternateOld, out alternateOld, false, false, false);
                                    string _services = "";
                                    _request.request_services = _services;
                                    _mailBody += MailingUtilities.addMailRow("Servizi", "" + _request.request_services, alternateOld, out alternateOld, false, false, false);
                                    _request.request_notes = enquiry.Message;
                                    _mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + _request.request_notes, alternateOld, out alternateOld, false, false, false);
                                    _request.request_date_created = DateTime.Now;
                                    _mailBody += MailingUtilities.addMailRow("Data ora creazione", "" + _request.request_date_created, alternateOld, out alternateOld, false, false, false);
                                    _request.state_date = DateTime.Now;
                                    _request.state_pid = 1;
                                    _request.state_subject = "Creata Richiesta";
                                    _request.state_pid_user = 1;
                                    _request.request_ip = "";
                                    _request.pid_creator = 1;
                                    _request.pid_city = 0;
                                    //_request.request_choices = "1" + " - " + CurrentSource.rntEstate_title(id.objToInt32(), _request.pid_lang.objToInt32(), "");
                                    dcOld.RNT_TBL_REQUEST.InsertOnSubmit(_request);
                                    dcOld.SubmitChanges();
                                    countCreated++;
                                    rntUtils.rntRequest_addState(_request.id, _request.state_pid.Value, _request.state_pid_user.Value, _request.state_subject, "");

                                    _mailBody += "</table>";
                                    _request.mail_body = _mailBody;
                                    string _mSubject = "";
                                    int pid_operator = 0;
                                    _request.pid_related_request = 0;
                                    RNT_TBL_REQUEST _relatedRequest = rntUtils.rntRequest_getRelatedRequest(_request);
                                    if (_relatedRequest != null)
                                    {
                                        _request.pid_related_request = _relatedRequest.id;
                                        pid_operator = _relatedRequest.pid_operator.Value;
                                        _mSubject = "rif." + _request.id + " Correlata a rif." + _relatedRequest.id + " - " + CurrentSource.getSysLangValue("reqHeadTitle", _request.pid_lang.objToInt32()) + " " + _request.name_first + " " + _request.name_last + " - " + _request.request_country + " - " + _request.request_choice_1 + " - " + _request.request_choice_2 + " - Check-In date: " + _request.request_date_start.Value.formatITA(true);
                                        rntUtils.rntRequest_addState(_request.id, 0, 1, "Correlazione alla richiesta Primaria rif. " + _relatedRequest.id, "");
                                        rntUtils.rntRequest_addState(_relatedRequest.id, 0, 1, "Aggiunta Correlazione alla richiesta Secondaria rif. " + _request.id, "");
                                    }
                                    else
                                    {
                                        _request.pid_related_request = 0;
                                        pid_operator = AdminUtilities.usr_getAvailableOperator(country_id, _request.pid_lang.objToInt32());
                                        _mSubject = "rif." + _request.id + " - " + CurrentSource.getSysLangValue("reqHeadTitle", _request.pid_lang.objToInt32()) + " " + _request.name_first + " " + _request.name_last + " - " + _request.request_country + " - " + _request.request_choice_1 + " - " + _request.request_choice_2 + " - Check-In date: " + _request.request_date_start.Value.formatITA(true);
                                    }
                                    if (pid_operator == 0)
                                    {
                                        _mailBody = "Attenzione! Non è stato Assegnato a nessun account.<br/><br/>" + _mailBody;
                                    }
                                    else
                                    {
                                        _request.operator_date = DateTime.Now;
                                        string _mailSend = "";
                                        //if (MailingUtilities.autoSendMailTo(_mSubject, _mailBody, AdminUtilities.usr_adminEmail(pid_operator, ""), false, "stp_contacts al account"))
                                        //    _mailSend = "Assegnato e inviato mail a " + AdminUtilities.usr_adminName(pid_operator, "") + " (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                                        //else
                                        //    _mailSend = "Assegnato " + AdminUtilities.usr_adminName(pid_operator, "") + " - Errore nel invio mail a (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                                        rntUtils.rntRequest_addState(_request.id, 0, 1, _mailSend, _mailBody);
                                        _mailBody = _mailSend + "<br/><br/>" + _mailBody;
                                    }

                                    _request.pid_operator = pid_operator;
                                    RNT_RL_REQUEST_ITEM _item = new RNT_RL_REQUEST_ITEM();
                                    _item.pid_estate = currEstate.id;
                                    _item.pid_request = _request.id;
                                    _item.sequence = 1;
                                    dcOld.RNT_RL_REQUEST_ITEMs.InsertOnSubmit(_item);
                                    dcOld.SubmitChanges();
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Show_inf_hotel_request_send", "setTimeout('parent.$(\".formreportapp\").colorbox.close();',3000);", true);
                                    //MailingUtilities.autoSendMailTo(_mSubject, _mailBody, MailingUtilities.ADMIN_MAIL, false, "admin_rnt_request_new_from_mail al admin");
                                }
                                else
                                {
                                    _request.name_first = enquiry.FromName;
                                    _request.name_last = "";
                                    _request.name_full = enquiry.FromName;
                                    _request.phone = enquiry.FromTelephone;
                                    _request.email = enquiry.FromEmail;
                                    _request.request_date_start = enquiry.HolidayStartDate;
                                    _request.request_date_end = enquiry.HolidayEndDate;
                                    _request.request_adult_num = enquiry.GuestsAdults;
                                    _request.request_child_num = enquiry.GuestsChildren;
                                    _request.request_notes = enquiry.Message;
                                    dcOld.SubmitChanges();
                                    countUpdated++;
                                }
                            }
                        ChnlHolidayUtils.addLog("", "ChnlHolidayImport.Enquiries_process", "IMPORTED countCreated:" + countCreated + ", countUpdated:" + countUpdated + ", countCanceled:" + countCanceled + "", "");
                    }
                    else
                    {
                        // If the response failed, return all the errors returned
                        foreach (string msg in enquiryResponse.WebServiceResponse.Messages)
                        {
                            ErrorString += msg + "<br/>";
                        }
                        ErrorLog.addLog("", "ChnlHolidayImport.Enquiries_process IdEstate:" + IdEstate, ErrorString);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHolidayImport.Enquiries_process IdEstate:" + IdEstate, ex.ToString());
            }
        }

        public Enquiries_process(int idEstate, DateTime? fromDate)
        {
            IdEstate = idEstate;
            FromDate = fromDate;
            ErrorString = "";

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayImport.Enquiries_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static string Enquiries_start(int idEstate, DateTime? fromDate)
    {
        Enquiries_process _tmp = new Enquiries_process(idEstate, fromDate);
        return _tmp.ErrorString;
    }
    public static void Enquiries_all(DateTime? fromDate)
    {
        using (DCmodRental dc = new DCmodRental())
        {
            var tmpList = dc.dbRntChnlHolidayEstateTBs.Where(x => x.homeId != null && x.homeId != "").ToList();
            foreach (var tmp in tmpList)
            {
                Enquiries_start(tmp.id, fromDate);
            }
        }
    }
}
public class ChnlHolidayUpdate
{
    private class UpdateRates_process
    {
        public int IdEstate { get; set; }
        dbRntChnlHolidayOwnerTBL ownerTbl { get; set; }
        dbRntChnlHolidayEstateTB chnlTb { get; set; }
        public long agentID { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }
        void doThread()
        {
            try
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateRates_process", "Agent for HolidayLettings was not found or not active");
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateRates_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                    chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || chnlTb.homeId.ToInt64() == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateRates_process", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                        return;
                    }
                    ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
                    if (ownerTbl == null)
                    {
                        ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                        if (ownerTbl == null)
                        {
                            ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateRates_process", "idEstate:" + IdEstate + " NO OWNER");
                            return;
                        }
                    }
                    var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                    var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;
                    int outError;
                    var priceListPerDatesTmp = rntUtils.estate_getPriceListPerDates(IdEstate, agentID, dtStart, dtEnd, out outError);
                    if (priceListPerDatesTmp == null || priceListPerDatesTmp.Count == 0)
                    {
                        ErrorLog.addLog("", "HolidayUpdate_process", "idEstate:" + IdEstate + " has no prices");
                        return;
                    }
                    var minStayList = dc.dbRntChnlHolidayEstateClosedDatesRLs.Where(x => x.pidEstate == IdEstate && x.minStay > 0 && x.changeDate >= dtStart).ToList();
                    var changesListTmp = dc.dbRntChnlHolidayEstateRateChangesRLs.Where(x => x.pidEstate == IdEstate && (x.weekday_changeIsDiscount >= 0 || x.weekend_changeIsDiscount >= 0 || x.weekly_changeIsDiscount >= 0) && x.changeDate >= dtStart).ToList();
                    var changesList = new List<dbRntChnlHolidayEstateRateChangesRL>();
                    var priceListPerDates = new List<rntExts.PriceListPerDates>();
                    foreach (var tmp in priceListPerDatesTmp)
                    {
                        DateTime dtCurrent = tmp.DtStart;
                        while (dtCurrent <= tmp.DtEnd)
                        {
                            var datePrices = new rntExts.PriceListPerDates(dtCurrent, dtCurrent, tmp.MinStay, tmp);
                            var minStayDate = minStayList.FirstOrDefault(x => x.changeDate == dtCurrent);
                            if (minStayDate != null) datePrices.MinStay = minStayDate.minStay;
                            var lastDatePrice = priceListPerDates.LastOrDefault();
                            if (lastDatePrice == null || !lastDatePrice.HasSamePrices(datePrices)) priceListPerDates.Add(datePrices);
                            else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);

                            var rateChangeTmp = changesListTmp.FirstOrDefault(x => x.changeDate == dtCurrent);
                            var rateChange = chnlTb.CreateRateChange(dtCurrent, rateChangeTmp);
                            changesList.Add(rateChange);

                            dtCurrent = dtCurrent.AddDays(1);
                        }
                    }
                    // change this for other systems with price per persons

                    List<int> NumPersonsListTmp = new List<int>();
                    for (int i = currEstate.pr_basePersons.objToInt32(); i <= currEstate.num_persons_max.objToInt32(); i++)
                    {
                        NumPersonsListTmp.Add(i);
                    }
                    NumPersonsListTmp.Reverse();
                    List<int> NumPersonsList = new List<int>();
                    if (NumPersonsListTmp.Count >= 5)
                    {
                        NumPersonsList.Add(NumPersonsListTmp[0]);
                        NumPersonsList.Add(NumPersonsListTmp[2]);
                        NumPersonsList.Add(NumPersonsListTmp[4]);
                    }
                    else
                    {
                        NumPersonsList = NumPersonsListTmp.Take(3).ToList();
                    }
                    var list = new List<RatesPerDates>();
                    foreach (var tmp in priceListPerDates)
                    {
                        foreach (int NumPersons in NumPersonsList)
                        {
                            decimal currWeekdayPrice = tmp.Prices[NumPersons];
                            if (currWeekdayPrice == 0) currWeekdayPrice = 999999;
                            decimal currWeekendPrice = currWeekdayPrice;
                            decimal currWeeklyPrice = currWeekdayPrice * 7;
                            decimal changeAmount = 0;

                            DateTime dtCurrent = tmp.DtStart;
                            while (dtCurrent <= tmp.DtEnd)
                            {
                                var datePrices = new RatesPerDates(dtCurrent, dtCurrent, tmp.MinStay, NumPersons);
                                datePrices.SeasonName = CurrentSource.rntPeriod_title(tmp.Period, 2, "");
                                datePrices.WeekdayPrice = currWeekdayPrice;
                                datePrices.WeekendPrice = currWeekendPrice;
                                datePrices.WeeklyPrice = currWeeklyPrice;
                                var rateChange = changesList.SingleOrDefault(x => x.changeDate == dtCurrent);
                                if (rateChange == null)
                                { datePrices.SeasonName = "No Prices"; list.Add(datePrices); dtCurrent = dtCurrent.AddDays(1); continue; }

                                if (rateChange.weekday_changeAmount > 0)
                                {
                                    changeAmount = (rateChange.weekday_changeIsPercentage == 1) ? (currWeekdayPrice * rateChange.weekday_changeAmount.objToInt32() / 100) : rateChange.weekday_changeAmount.objToInt32();
                                    if (rateChange.weekday_changeIsDiscount == 1) { changeAmount = -changeAmount; }
                                    datePrices.WeekdayPrice = currWeekdayPrice + changeAmount;
                                }

                                if (rateChange.weekend_changeAmount > 0)
                                {
                                    changeAmount = (rateChange.weekend_changeIsPercentage == 1) ? (currWeekendPrice * rateChange.weekend_changeAmount.objToInt32() / 100) : rateChange.weekend_changeAmount.objToInt32();
                                    if (rateChange.weekend_changeIsDiscount == 1) { changeAmount = -changeAmount; }
                                    datePrices.WeekendPrice = currWeekendPrice + changeAmount;
                                }

                                if (rateChange.weekly_changeAmount > 0)
                                {
                                    changeAmount = (rateChange.weekly_changeIsPercentage == 1) ? (currWeeklyPrice * rateChange.weekly_changeAmount.objToInt32() / 100) : rateChange.weekly_changeAmount.objToInt32();
                                    if (rateChange.weekly_changeIsDiscount == 1) { changeAmount = -changeAmount; }
                                    datePrices.WeeklyPrice = currWeeklyPrice + changeAmount;
                                }

                                var lastDatePrice = list.LastOrDefault();
                                if (lastDatePrice == null || !lastDatePrice.HasSamePrices(datePrices)) list.Add(datePrices);
                                else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);

                                dtCurrent = dtCurrent.AddDays(1);
                            }
                        }
                    }
                    foreach (int NumPersons in NumPersonsList)
                    {
                        List<string> lstDtStart = new List<string>();
                        List<string> lstDtEnd = new List<string>();
                        List<string> lstWeeklyPrices = new List<string>();
                        List<string> lstWeekdayPrices = new List<string>();
                        List<string> lstWeekendPrices = new List<string>();
                        List<string> lstMinNights = new List<string>();
                        List<string> lstNames = new List<string>();
                        var tmpList = list.Where(x => x.NumPersons == NumPersons).ToList();
                        foreach (var tmp in tmpList)
                        {
                            lstDtStart.Add(tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----"));
                            lstDtEnd.Add(tmp.DtEnd.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----"));
                            lstMinNights.Add(tmp.MinNights + "");
                            lstWeekdayPrices.Add(tmp.WeekdayPrice.objToInt32() + "");
                            lstWeekendPrices.Add(tmp.WeekendPrice.objToInt32() + "");
                            lstWeeklyPrices.Add(tmp.WeeklyPrice.objToInt32() + "");
                            lstNames.Add(tmp.SeasonName);
                        }
                        SetAll(NumPersons + "", lstDtStart, lstDtEnd, lstMinNights, lstWeeklyPrices, lstWeekdayPrices, lstWeekendPrices, lstNames);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateRates_process IdEstate:" + IdEstate, ex.ToString());
            }
        }
        string SetAll(string groupSize, List<string> lstDtStart, List<string> lstDtEnd, List<string> lstMinNights, List<string> lstWeeklyPrice, List<string> lstWeekdayPrice, List<string> lstWeekendPrice, List<string> lstNames)
        {
            //set new TLS protocol 1.1 before importation
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

            ServiceRates.service_rates rateClient = new ServiceRates.service_rates();
            //service_ratesSoapClient rateClient = new service_ratesSoapClient("service_ratesSoap12");
            ServiceRates.WebServiceResponse setAllResponse = rateClient.SetAll(ownerTbl.ownerKey, ownerTbl.ownerId + "", chnlTb.homeId, groupSize, lstDtStart.ToArray(), lstDtEnd.ToArray(), lstNames.ToArray(), lstWeeklyPrice.ToArray(), lstMinNights.ToArray(), lstWeekdayPrice.ToArray(), lstWeekendPrice.ToArray());
            string logMsg = "";
            logMsg += groupSize + "\r\n";
            logMsg += lstDtStart.listToString(",") + "\r\n";
            logMsg += lstDtEnd.listToString(",") + "\r\n";
            logMsg += lstMinNights.listToString(",") + "\r\n";
            logMsg += lstWeeklyPrice.listToString(",") + "\r\n";
            logMsg += lstWeekdayPrice.listToString(",") + "\r\n";
            logMsg += lstWeekendPrice.listToString(",") + "\r\n";
            // If the response is successful, display success message
            if (setAllResponse.Success == true)
            {
                ChnlHolidayUtils.addLog("", "ChnlHolidayUpdate.UpdateRates_process.SetAll", "IdEstate:" + IdEstate + " updated successfully", logMsg);
                return "true";
            }
            else
            {
                string errorMsgs = "";
                foreach (string msg in setAllResponse.Messages)
                {
                    errorMsgs += msg + "\r\n";
                }
                ChnlHolidayUtils.addLog("", "ERROR: ChnlHolidayUpdate.UpdateRates_process.SetAll", errorMsgs, logMsg);
                return errorMsgs;
            }
        }
        //string ClearAll(string groupSize)
        //{
        //    ServiceRates.service_rates rateClient = new ServiceRates.service_rates();
        //    ServiceRates.WebServiceResponse setAllResponse = rateClient.Clear(ownerTbl.ownerKey, ownerTbl.ownerId + "", chnlTb.homeId, groupSize);
        //    if (setAllResponse.Success == true)
        //    {
        //        ChnlHolidayUtils.addLog("", "ChnlHolidayUpdate.UpdateRates_process.ClearAll", "", "IdEstate:" + IdEstate + " updated successfully");
        //        return "true";
        //    }
        //    else
        //    {
        //        string errorMsgs = "";
        //        foreach (string msg in setAllResponse.Messages)
        //        {
        //            errorMsgs += msg + "\r\n";
        //        }
        //        ChnlHolidayUtils.addLog("", "ERROR: ChnlHolidayUpdate.UpdateRates_process.ClearAll", "", errorMsgs);
        //        return errorMsgs;
        //    }
        //}
        public UpdateRates_process(int idEstate)
        {
            IdEstate = idEstate;
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayUpdate.UpdateRates_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
        public UpdateRates_process(int idEstate, DateTime dtStart, DateTime dtEnd)
        {
            IdEstate = idEstate;
            DtStart = dtStart.Date;
            DtEnd = dtEnd.Date;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayUpdate.UpdateRates_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public class RatesPerDates
    {
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public int MinNights { get; set; }
        public int NumPersons { get; set; }

        public decimal WeekdayPrice { get; set; }
        public decimal WeekendPrice { get; set; }
        public decimal WeeklyPrice { get; set; }

        public string SeasonName { get; set; }

        public RatesPerDates(DateTime dtStart, DateTime dtEnd, int minStay, int numPersons)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            MinNights = minStay;
            NumPersons = numPersons;

            WeekdayPrice = 0;
            WeekdayPrice = 0;
            WeeklyPrice = 0;

            SeasonName = "";
        }
        public RatesPerDates(DateTime dtStart, DateTime dtEnd, RatesPerDates copyFrom)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            MinNights = copyFrom.MinNights;
            NumPersons = copyFrom.NumPersons;

            WeekdayPrice = copyFrom.WeekdayPrice;
            WeekdayPrice = copyFrom.WeekdayPrice;
            WeeklyPrice = copyFrom.WeeklyPrice;

            SeasonName = copyFrom.SeasonName;
        }
        public bool HasSamePrices(RatesPerDates compareWith)
        {
            return
                MinNights == compareWith.MinNights &&
                NumPersons == compareWith.NumPersons &&
                WeekdayPrice == compareWith.WeekdayPrice &&
                WeekdayPrice == compareWith.WeekdayPrice &&
                WeeklyPrice == compareWith.WeeklyPrice
                ;
        }
    }
    public static void UpdateRates_start(int idEstate)
    {
        UpdateRates_process _tmp = new UpdateRates_process(idEstate);
    }
    public static void UpdateRates_start(int idEstate, DateTime dtStart, DateTime dtEnd)
    {
        UpdateRates_process _tmp = new UpdateRates_process(idEstate, dtStart, dtEnd);
    }

    private class UpdateAvailability_process
    {
        public int IdEstate { get; set; }
        dbRntChnlHolidayOwnerTBL ownerTbl { get; set; }
        dbRntChnlHolidayEstateTB chnlTb { get; set; }
        public long agentID { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }
        string SendRequest(string from, string to, string status)
        {
            try
            {
                var requestUrl = "https://agentapi.holidaylettings.co.uk/service_avail.aspx?act=update&start=" + from + "&end=" + to + "&status=" + status + "&owner_id=" + ownerTbl.ownerId + "&home_id=" + chnlTb.homeId + "&secret=" + ownerTbl.ownerKey;
                HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                obj.UserAgent = "sapphire";

                //set new TLS protocol 1.1
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;
                
                HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
                Stream os1 = obj1.GetResponseStream();
                StreamReader _Answer = new StreamReader(os1);
                String strRSString = _Answer.ReadToEnd().ToString();
                ChnlHolidayUtils.addLog("", "ChnlHolidayUpdate.UpdateAvailability_process", "IdEstate: " + IdEstate + " from:" + from + " to:" + to + " status:" + status, strRSString);
                return strRSString;

            }
            catch (Exception ex)
            {
                ChnlHolidayUtils.addLog("", "ERROR: ChnlHolidayUpdate.UpdateAvailability_process", "IdEstate: " + IdEstate + "from:" + from + " to:" + to + " status:" + status, ex.ToString());
                return "";
            }
        }
        void doThread()
        {
            try
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateAvailability_process", "Agent for HolidayLettings was not found or not active");
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateAvailability_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                    chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || chnlTb.homeId.ToInt64() == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateAvailability_process", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                        return;
                    }
                    ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
                    if (ownerTbl == null)
                    {
                        ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                        if (ownerTbl == null)
                        {
                            ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateAvailability_process", "idEstate:" + IdEstate + " NO OWNER");
                            return;
                        }
                    }
                    var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                    var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;
                    var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                    var list = new List<rntExts.AvvListPerDates>();
                    DateTime dtCurrent = dtStart;
                    var closedList = dc.dbRntChnlHolidayEstateClosedDatesRLs.Where(x => x.pidEstate == IdEstate && x.isClosed == 1 && x.changeDate >= dtStart).ToList();
                    while (dtCurrent < dtEnd)
                    {
                        var tmpDate = closedList.FirstOrDefault(x => x.changeDate == dtCurrent);
                        bool isAvv = tmpDate != null ? false : resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count() == 0;
                        var lastDatePrice = list.LastOrDefault();
                        if (lastDatePrice == null || lastDatePrice.IsAvv != isAvv) list.Add(new rntExts.AvvListPerDates(dtCurrent, dtCurrent, isAvv));
                        else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
                        dtCurrent = dtCurrent.AddDays(1);
                    }
                    foreach (var tmp in list)
                    {
                        SendRequest(tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----"), tmp.DtEnd.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----"), (tmp.IsAvv ? "a" : "b"));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateAvailability_process IdEstate:" + IdEstate, ex.ToString());
            }
        }

        public UpdateAvailability_process(int idEstate)
        {
            IdEstate = idEstate;
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayUpdate.UpdateAvailability_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
        public UpdateAvailability_process(int idEstate, DateTime dtStart, DateTime dtEnd)
        {
            IdEstate = idEstate;
            DtStart = dtStart.Date;
            DtEnd = dtEnd.Date;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayUpdate.UpdateAvailability_process idEstate:" + idEstate);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

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

    private class UpdatePhotos_process
    {
        public int IdEstate { get; set; }
        dbRntChnlHolidayOwnerTBL ownerTbl { get; set; }
        dbRntChnlHolidayEstateTB chnlTb { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        void doThread()
        {
            try
            {
                string HLImageLog = CommonUtilities.getSYS_SETTING("HLImageLog");

                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdatePhotos_process", "Agent for HolidayLettings was not found or not active");
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdatePhotos_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                    chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || chnlTb.homeId.ToInt64() == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdatePhotos_process", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                        return;
                    }
                    ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
                    if (ownerTbl == null)
                    {
                        ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                        if (ownerTbl == null)
                        {
                            ErrorLog.addLog("", "ChnlHolidayUpdate.UpdatePhotos_process", "idEstate:" + IdEstate + " NO OWNER");
                            return;
                        }
                    }

                    //set new TLS protocol 1.1 before importation
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

                    // Open an photo soap client instance ,  + "", 
                    Photos.photos photoClient = new Photos.photos();
                    // Create your fetch photos request
                    Photos.FetchPicRequest fetchPhotoRequest = new Photos.FetchPicRequest();
                    fetchPhotoRequest.HomeId = chnlTb.homeId.objToInt64();
                    // Call FetchPhotos method with parameters advertiserId, key and fetchPhotoRequest.
                    // That will fetch information on all active photos for a given advert
                    Photos.FetchWebServiceResponseOfPicDto fetchResponse = photoClient.FetchPhotos(ownerTbl.ownerId, ownerTbl.ownerKey, fetchPhotoRequest);
                    // If the response is successful, display data
                    var ErrorString = "";
                    if (fetchResponse.WebServiceResponse.Success == true)
                    {
                        foreach (Photos.Pic photoData in fetchResponse.FetchedData)
                        {
                            Photos.PicDeleteRequest picDeleteRequest = new Photos.PicDeleteRequest();
                            picDeleteRequest.HomeId = chnlTb.homeId.objToInt64();
                            picDeleteRequest.PicId = photoData.PicId.objToInt32();
                            var tmpResponse = photoClient.DeletePhoto(ownerTbl.ownerId, ownerTbl.ownerKey, picDeleteRequest);
                            if (tmpResponse.Success != true)
                            {
                                ErrorString = "";
                                foreach (string msg in tmpResponse.Messages)
                                {
                                    ErrorString += msg + "\r\n";
                                }
                                ChnlHolidayUtils.addLog("", "ERROR:UpdatePhotos_process.PicDeleteRequest", "IdEstate:" + IdEstate + " PicId:" + photoData.PicId + " code:" + tmpResponse.ErrorCode, ErrorString);
                            }
                        }
                    }
                    else
                    {
                        ErrorString = "";
                        foreach (string msg in fetchResponse.WebServiceResponse.Messages)
                        {
                            ErrorString += msg + "\r\n";
                        }
                        ChnlHolidayUtils.addLog("", "ERROR:UpdatePhotos_process.FetchPicRequest", "IdEstate:" + IdEstate + " code:" + fetchResponse.WebServiceResponse.ErrorCode, ErrorString);
                    }
                    var currList = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == "chnlHL").OrderBy(x => x.sequence).ToList();
                    int uploadCount = 0;
                    foreach (var photo in currList)
                    {
                        if (!File.Exists(Path.Combine(App.SRP, photo.img_banner))) continue;
                        System.Drawing.Image img = System.Drawing.Image.FromFile(Path.Combine(App.SRP, photo.img_banner));
                        if (img.Width > img.Height && (img.Width < 640 || img.Height < 480)) continue;
                        if (img.Width < img.Height && (img.Width < 480 || img.Height < 640)) continue;
                        if (uploadCount == 24) break;

                        Photos.PicUploadViaUrlRequest photoURLRequest = new Photos.PicUploadViaUrlRequest();
                        photoURLRequest.HomeId = chnlTb.homeId.objToInt64();
                        photoURLRequest.PicId = photo.id;
                        photoURLRequest.Caption = photo.code;
                        photoURLRequest.Url = Host + "/" + photo.img_banner + "?maxsize=true";
                        photoURLRequest.Email = ownerTbl.photosEmail;
                        photoURLRequest.MakePanoramic = false;
                        // Call UploadPhotoViaUrl method with parameters advertiserId, key and photoURLRequest. That will upload the photo and associated caption using a URL link to the image for a given advert
                        var uploadResponse = photoClient.UploadPhotoViaUrl(ownerTbl.ownerId, ownerTbl.ownerKey, photoURLRequest);

                        if (HLImageLog == "true" || HLImageLog == "1")
                        {
                            ChnlHolidayUtils.addLog("", "UpdatePhotos_process.photoURLRequest", "IdEstate:" + IdEstate + " url:" + photoURLRequest.Url, "");
                        }

                        if (uploadResponse.Success != true)
                        {
                            ErrorString = "";
                            foreach (string msg in uploadResponse.Messages)
                            {
                                ErrorString += msg + "\r\n";
                            }
                            ChnlHolidayUtils.addLog("", "ERROR:UpdatePhotos_process.PicUploadViaUrlRequest", "IdEstate:" + IdEstate + " code:" + uploadResponse.ErrorCode, ErrorString);
                        }
                        uploadCount++;
                    }
                    ChnlHolidayUtils.addLog("", "UpdatePhotos_process", "IdEstate:" + IdEstate + " updated successfully, Number of photos:" + uploadCount, "");

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHolidayUpdate.UpdatePhotos_process IdEstate:" + IdEstate, ex.ToString());
            }
        }

        public UpdatePhotos_process(int idEstate, string host)
        {
            IdEstate = idEstate;
            Host = host;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayUpdate.UpdatePhotos_process idEstate:" + idEstate);
            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static void UpdatePhotos_start(int idEstate, string host)
    {
        UpdatePhotos_process _tmp = new UpdatePhotos_process(idEstate, host);
    }

    private class UpdateActiveState_process
    {
        public int IdEstate { get; set; }
        dbRntChnlHolidayOwnerTBL ownerTbl { get; set; }
        dbRntChnlHolidayEstateTB chnlTb { get; set; }
        public long agentID { get; set; }
        public bool Activate { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        void doThread()
        {
            try
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateActiveState_process", "Agent for HolidayLettings was not found or not active");
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateActiveState_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                    chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || chnlTb.homeId.ToInt64() == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateActiveState_process", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                        return;
                    }
                    ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
                    if (ownerTbl == null)
                    {
                        ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                        if (ownerTbl == null)
                        {
                            ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateActiveState_process", "idEstate:" + IdEstate + " NO OWNER");
                            return;
                        }
                    }

                    //set new TLS protocol 1.1 before importation
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

                    Adverts.adverts advertClient = new Adverts.adverts();
                    if (Activate)
                    {
                        // Create your StandaloneActivation request
                        var advertRequest = new Adverts.StandaloneRequestActivationRequest();
                        // HomeId is mandatory
                        advertRequest.HomeId = chnlTb.homeId.objToInt64();
                        // Call StandaloneActivationRequest method with parameters advertiserId, key.
                        // That will send an activation request for the given home Id
                        var activationResponse = advertClient.StandaloneActivationRequest(ownerTbl.ownerId, ownerTbl.ownerKey, advertRequest);
                        // If the response is successful, display success message
                        if (activationResponse.Success == true)
                        {
                            chnlTb.ActiveState = Activate;
                            dc.SaveChanges();
                            ChnlHolidayUtils.addLog("", "UpdateActiveState_process", "IdEstate:" + IdEstate + " updated successfully", "");
                        }
                        else
                        {
                            // If the response failed, return all the errors returned
                            foreach (string msg in activationResponse.Messages)
                            {
                                ErrorString += msg + "<br/>";
                            }
                            ChnlHolidayUtils.addLog("", "ERROR:UpdateActiveState_process", "IdEstate:" + IdEstate, ErrorString);
                        }
                    }
                    else
                    {
                        // Create your StandaloneActivation request
                        var advertRequest = new Adverts.StandaloneRequestDeactivationRequest();
                        // HomeId is mandatory
                        advertRequest.HomeId = chnlTb.homeId.objToInt64();
                        // Call StandaloneActivationRequest method with parameters advertiserId, key.
                        // That will send an activation request for the given home Id
                        var activationResponse = advertClient.StandaloneDeactivationRequest(ownerTbl.ownerId, ownerTbl.ownerKey, advertRequest);
                        // If the response is successful, display success message
                        if (activationResponse.Success == true)
                        {
                            chnlTb.ActiveState = Activate;
                            dc.SaveChanges();
                            ChnlHolidayUtils.addLog("", "UpdateActiveState_process", "IdEstate:" + IdEstate + " updated successfully", "");
                        }
                        else
                        {
                            // If the response failed, return all the errors returned
                            foreach (string msg in activationResponse.Messages)
                            {
                                ErrorString += msg + "<br/>";
                            }
                            ChnlHolidayUtils.addLog("", "ERROR:UpdateActiveState_process", "IdEstate:" + IdEstate, ErrorString);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateActiveState_process IdEstate:" + IdEstate, ex.ToString());
            }
        }

        public UpdateActiveState_process(int idEstate, string host, string activate, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Host = host;
            Activate = activate == "on";
            ErrorString = "";
            if (backgroundProcess)
            {
                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayUpdate.UpdateActiveState_process idEstate:" + idEstate);
                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();

            }
            else
                doThread();
        }
    }
    public static string UpdateActiveState_start(int idEstate, string host, string activate)
    {
        UpdateActiveState_process _tmp = new UpdateActiveState_process(idEstate, host, activate, false);
        return _tmp.ErrorString;
    }

    private class UpdateUnit_process
    {
        public int IdEstate { get; set; }
        dbRntChnlHolidayOwnerTBL ownerTbl { get; set; }
        dbRntChnlHolidayEstateTB chnlTb { get; set; }
        public long agentID { get; set; }
        public string Host { get; set; }
        public string ErrorString { get; set; }
        void doThread()
        {
            try
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHolidayProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateUnit_process", "Agent for HolidayLettings was not found or not active");
                        return;
                    }
                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateUnit_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                    chnlTb = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                    if (chnlTb == null || chnlTb.homeId.ToInt64() == 0)
                    {
                        //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateUnit_process", "idEstate:" + IdEstate + " not found or not active in HolidayLettings");
                        return;
                    }
                    ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.SingleOrDefault(x => x.ownerId == chnlTb.ownerId && x.isActive == 1);
                    if (ownerTbl == null)
                    {
                        ownerTbl = dc.dbRntChnlHolidayOwnerTBLs.FirstOrDefault(x => x.isActive == 1);
                        if (ownerTbl == null)
                        {
                            ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateUnit_process", "idEstate:" + IdEstate + " NO OWNER");
                            return;
                        }
                    }

                    //set new TLS protocol 1.1 before importation
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;  //SecurityProtocolType.Tls11;// (SecurityProtocolType)786;

                    if (chnlTb.ActiveState)
                    {
                        Adverts.adverts webService = new Adverts.adverts();
                        // Create your inactive unit request, which will be used to update your property
                        Adverts.ActiveUnitUpdateRequest unit = new Adverts.ActiveUnitUpdateRequest();

                        // HomeId is mandatory
                        unit.HomeId = chnlTb.homeId.objToInt64();
                        // Optional parameters below
                        unit.HomeName = chnlTb.homeName;
                        if (chnlTb.num_persons_max.objToInt32() > currEstate.num_persons_max.objToInt32())
                            unit.SleepsMax = currEstate.num_persons_max.objToInt32();
                        else if (chnlTb.num_persons_max.objToInt32() < currEstate.num_persons_min.objToInt32())
                            unit.SleepsMax = currEstate.num_persons_min.objToInt32();
                        else
                            unit.SleepsMax = chnlTb.num_persons_max;
                        // InactiveUnit.AllowSmoking = true;
                        unit.HomeDescription = chnlTb.homeDescription;
                        unit.HomeSummary = chnlTb.homeSummary;

                        unit.NumberDoubleBeds = chnlTb.num_bed_double;
                        unit.NumberSingleBeds = chnlTb.num_bed_single;
                        unit.NumberCots = chnlTb.num_cot;
                        unit.NumberSofaBeds = chnlTb.num_bed_sofa;
                        unit.NumberBathrooms = chnlTb.num_family_bath;
                        unit.NumberEnSuiteRooms = chnlTb.num_en_suite;
                        unit.NumberShowerRooms = chnlTb.num_shower_rooms;
                        unit.SeatingInLounge = chnlTb.num_lounge_seats;
                        unit.SeatingForDining = chnlTb.num_dining_seats;

                        //facilities        
                        //indoor facilities
                        unit.HasCooker = chnlTb.has_cooker == true;
                        unit.HasTelevision = chnlTb.has_tv == true;
                        unit.HasFireplace = chnlTb.has_fireplace == true;
                        unit.HasFridge = chnlTb.has_fridge == true;
                        unit.HasSatelliteTV = chnlTb.has_satelite_tv == true;
                        unit.HasCentralHeating = chnlTb.has_central_heating == true;
                        unit.HasFreezer = chnlTb.has_freezer == true;
                        unit.HasVideo = chnlTb.has_video == true;
                        unit.HasAirConditioning = chnlTb.has_air_condition == true;
                        unit.HasMicrowave = chnlTb.has_microwave == true;
                        unit.HasDvd = chnlTb.has_dvd == true;
                        unit.LinenProvided = chnlTb.has_linen == true;
                        unit.HasToaster = chnlTb.has_toaster == true;
                        //chk_cd.Checked = _chnlTb.c == true;
                        unit.TowelsProvided = chnlTb.has_towel == true;
                        unit.HasKettle = chnlTb.has_kettle == true;
                        unit.HasInternetAccess = chnlTb.has_internet_access == true;
                        unit.HasSauna = chnlTb.has_sauna == true;
                        unit.HasDishwasher = chnlTb.has_dish_washer == true;
                        unit.HasWiFi = chnlTb.has_wifi == true;
                        unit.HasGym = chnlTb.has_gym == true;
                        unit.HasClothesDryer = chnlTb.has_cloth_dryer == true;
                        unit.HasFax = chnlTb.has_fax == true;
                        unit.HasPoolSnooker = chnlTb.has_pool_snooker == true;
                        unit.HasIron = chnlTb.has_iron == true;
                        unit.HasHairdryer = chnlTb.has_hair_dryer == true;
                        unit.HasGamesRoom = chnlTb.has_games_room == true;
                        unit.HasHighchair = chnlTb.has_high_chair == true;
                        unit.HasSafe = chnlTb.has_safe == true;
                        unit.StaffedProperty = chnlTb.staffed_property == true;
                        unit.HasTelephone = chnlTb.has_telephone;
                        unit.HasWashingMachine = chnlTb.has_washing_machine;
                        unit.HasPingPong = chnlTb.has_pingpong;
                        unit.HasSeaView = chnlTb.has_sea_view;
                        unit.HasSharedGarden = chnlTb.has_shared_garden;


                        //outdoor facilities
                        unit.HasPoolSharedOutdoorHeated = chnlTb.has_shared_outdoor_heated == true;
                        unit.HasPoolSharedOutdoorUnheated = chnlTb.has_shared_outdoor_unheated == true;
                        unit.HasPoolPrivateOutdoorHeated = chnlTb.has_private_outdoor_heated == true;
                        unit.HasPoolPrivateOutdoorUnheated = chnlTb.has_private_outdoor_unheated == true;
                        unit.HasPoolPrivateIndoor = chnlTb.has_pool_private_indoor == true;
                        unit.HasPoolSharedIndoor = chnlTb.has_shared_indoor == true;
                        unit.HasPoolForChildren = chnlTb.has_pool_for_children == true;
                        unit.HasJacuzziHotTub = chnlTb.has_jacuzzi_hot_tub == true;
                        unit.HasSharedTennisCourt = chnlTb.has_shared_tennis_court == true;
                        unit.HasPrivateTennisCourt = chnlTb.has_private_tennis_court == true;
                        unit.HasPrivateGarden = chnlTb.has_private_garden == true;
                        unit.HasClimbingFrame = chnlTb.has_climbing_frame == true;
                        unit.HasSwingSet = chnlTb.has_swwing_set == true;
                        unit.HasTrampoline = chnlTb.has_trampoline == true;
                        unit.HasBarbecue = chnlTb.has_barbecue == true;
                        unit.HasPrivateFishing = chnlTb.has_private_fishing == true;
                        unit.BicyclesAvailable = chnlTb.has_bicycle == true;
                        if (chnlTb.has_parking == true)
                        {
                            if (chnlTb.has_secure_parking == true)
                                unit.ParkingAvailable = Adverts.ParkingAvailable.AvailableAndSecure;
                            else
                                unit.ParkingAvailable = Adverts.ParkingAvailable.Available;
                        }
                        else
                            unit.ParkingAvailable = Adverts.ParkingAvailable.NotAvailable;
                        unit.HasBoat = chnlTb.has_boat;
                        unit.HasSolariumRoofTerrace = chnlTb.has_solanium_roof_terrace;
                        unit.HasBalconyOrTerrace = chnlTb.has_terrace;

                        if (chnlTb.available_corporate == 1)
                            unit.AvailableForCorporate = true;
                        else
                            unit.AvailableForCorporate = false;

                        if (chnlTb.available_henstag == 1)
                            unit.AvailableForHenStag = true;
                        else
                            unit.AvailableForHenStag = false;

                        if (chnlTb.available_house_swap == 1)
                            unit.AvailableForHouseSwap = true;
                        else
                            unit.AvailableForHouseSwap = false;

                        if (chnlTb.available_longlet == 1)
                            unit.AvailableForLongLet = true;
                        else
                            unit.AvailableForLongLet = false;

                        if (chnlTb.available_shortbreak == 1)
                            unit.AvailableForShortBreaks = true;
                        else
                            unit.AvailableForShortBreaks = false;

                        if (chnlTb.suitable_children == "0")
                            unit.SuitableForChildren = Adverts.SuitableForChildren.NotSuitable;
                        if (chnlTb.suitable_children == "1")
                            unit.SuitableForChildren = Adverts.SuitableForChildren.NotForUnder5s;
                        if (chnlTb.suitable_children == "2")
                            unit.SuitableForChildren = Adverts.SuitableForChildren.Yes;

                        if (chnlTb.restricted_mobility == "0")
                            unit.RestrictedMobility = Adverts.RestrictedMobility.NotSuitable;
                        if (chnlTb.restricted_mobility == "1")
                            unit.RestrictedMobility = Adverts.RestrictedMobility.YesWithLiftAccess;
                        if (chnlTb.restricted_mobility == "2")
                            unit.RestrictedMobility = Adverts.RestrictedMobility.Yes;

                        if (chnlTb.allow_pets == "0")
                            unit.AllowPets = Adverts.AllowPets.No;
                        if (chnlTb.allow_pets == "1")
                            unit.AllowPets = Adverts.AllowPets.PleaseEnquire;
                        if (chnlTb.allow_pets == "2")
                            unit.AllowPets = Adverts.AllowPets.Yes;

                        if (chnlTb.wheelchair_users == "0")
                            unit.WheelchairUsers = Adverts.WheelchairUsers.NotSuitable;
                        if (chnlTb.wheelchair_users == "1")
                            unit.WheelchairUsers = Adverts.WheelchairUsers.Accessible;
                        if (chnlTb.wheelchair_users == "2")
                            unit.WheelchairUsers = Adverts.WheelchairUsers.AccessibleAndAdapted;

                        if (chnlTb.allow_smoking == 1)
                            unit.AllowSmoking = true;
                        else
                            unit.AllowSmoking = false;


                        Adverts.WebServiceResponse inactiveUnitResponse = webService.ActiveUnitUpdateRequest(ownerTbl.ownerId, ownerTbl.ownerKey, unit);
                        // If the response is successful, display information message
                        if (inactiveUnitResponse.Success == true)
                        {
                            ChnlHolidayUtils.addLog("", "UpdateUnit_process", "IdEstate:" + IdEstate + " updated successfully", "");
                        }
                        else
                        {
                            foreach (string msg in inactiveUnitResponse.Messages)
                            {
                                ErrorString += msg + "\r\n";
                            }
                            ChnlHolidayUtils.addLog("", "ERROR:UpdateUnit_process", "IdEstate:" + IdEstate, ErrorString);
                        }
                    }
                    else
                    {
                        Adverts.adverts webService = new Adverts.adverts();
                        // Create your inactive unit request, which will be used to update your property
                        Adverts.InactiveUnitUpdateRequest unit = new Adverts.InactiveUnitUpdateRequest();



                        // HomeId is mandatory
                        unit.HomeId = chnlTb.homeId.objToInt64();
                        // Optional parameters below
                        unit.HomeName = chnlTb.homeName;
                        if (chnlTb.homeType == "apt")
                            unit.HomeType = Adverts.HomeType.Apartment;
                        else if (chnlTb.homeType == "villa")
                            unit.HomeType = Adverts.HomeType.Villa;
                        else if (chnlTb.homeType == "residence")
                            unit.HomeType = Adverts.HomeType.House;
                        unit.NumberOfBedrooms = chnlTb.num_rooms_bed;
                        if (chnlTb.num_persons_max.objToInt32() > currEstate.num_persons_max.objToInt32())
                            unit.SleepsMax = currEstate.num_persons_max.objToInt32();
                        else if (chnlTb.num_persons_max.objToInt32() < currEstate.num_persons_min.objToInt32())
                            unit.SleepsMax = currEstate.num_persons_min.objToInt32();
                        else
                            unit.SleepsMax = chnlTb.num_persons_max;
                        // InactiveUnit.AllowSmoking = true;
                        unit.HomeDescription = chnlTb.homeDescription;
                        unit.HomeSummary = chnlTb.homeSummary;

                        unit.NumberDoubleBeds = chnlTb.num_bed_double;
                        unit.NumberSingleBeds = chnlTb.num_bed_single;
                        unit.NumberCots = chnlTb.num_cot;
                        unit.NumberSofaBeds = chnlTb.num_bed_sofa;
                        unit.NumberBathrooms = chnlTb.num_family_bath;
                        unit.NumberEnSuiteRooms = chnlTb.num_en_suite;
                        unit.NumberShowerRooms = chnlTb.num_shower_rooms;
                        unit.SeatingInLounge = chnlTb.num_lounge_seats;
                        unit.SeatingForDining = chnlTb.num_dining_seats;

                        //facilities        
                        //indoor facilities
                        unit.HasCooker = chnlTb.has_cooker == true;
                        unit.HasTelevision = chnlTb.has_tv == true;
                        unit.HasFireplace = chnlTb.has_fireplace == true;
                        unit.HasFridge = chnlTb.has_fridge == true;
                        unit.HasSatelliteTV = chnlTb.has_satelite_tv == true;
                        unit.HasCentralHeating = chnlTb.has_central_heating == true;
                        unit.HasFreezer = chnlTb.has_freezer == true;
                        unit.HasVideo = chnlTb.has_video == true;
                        unit.HasAirConditioning = chnlTb.has_air_condition == true;
                        unit.HasMicrowave = chnlTb.has_microwave == true;
                        unit.HasDvd = chnlTb.has_dvd == true;
                        unit.LinenProvided = chnlTb.has_linen == true;
                        unit.HasToaster = chnlTb.has_toaster == true;
                        //chk_cd.Checked = _chnlTb.c == true;
                        unit.TowelsProvided = chnlTb.has_towel == true;
                        unit.HasKettle = chnlTb.has_kettle == true;
                        unit.HasInternetAccess = chnlTb.has_internet_access == true;
                        unit.HasSauna = chnlTb.has_sauna == true;
                        unit.HasDishwasher = chnlTb.has_dish_washer == true;
                        unit.HasWiFi = chnlTb.has_wifi == true;
                        unit.HasGym = chnlTb.has_gym == true;
                        unit.HasClothesDryer = chnlTb.has_cloth_dryer == true;
                        unit.HasFax = chnlTb.has_fax == true;
                        unit.HasPoolSnooker = chnlTb.has_pool_snooker == true;
                        unit.HasIron = chnlTb.has_iron == true;
                        unit.HasHairdryer = chnlTb.has_hair_dryer == true;
                        unit.HasGamesRoom = chnlTb.has_games_room == true;
                        unit.HasHighchair = chnlTb.has_high_chair == true;
                        unit.HasSafe = chnlTb.has_safe == true;
                        unit.StaffedProperty = chnlTb.staffed_property == true;
                        unit.HasTelephone = chnlTb.has_telephone;
                        unit.HasWashingMachine = chnlTb.has_washing_machine;
                        unit.HasPingPong = chnlTb.has_pingpong;
                        unit.HasSeaView = chnlTb.has_sea_view;
                        unit.HasSharedGarden = chnlTb.has_shared_garden;


                        //outdoor facilities
                        unit.HasPoolSharedOutdoorHeated = chnlTb.has_shared_outdoor_heated == true;
                        unit.HasPoolSharedOutdoorUnheated = chnlTb.has_shared_outdoor_unheated == true;
                        unit.HasPoolPrivateOutdoorHeated = chnlTb.has_private_outdoor_heated == true;
                        unit.HasPoolPrivateOutdoorUnheated = chnlTb.has_private_outdoor_unheated == true;
                        unit.HasPoolPrivateIndoor = chnlTb.has_pool_private_indoor == true;
                        unit.HasPoolSharedIndoor = chnlTb.has_shared_indoor == true;
                        unit.HasPoolForChildren = chnlTb.has_pool_for_children == true;
                        unit.HasJacuzziHotTub = chnlTb.has_jacuzzi_hot_tub == true;
                        unit.HasSharedTennisCourt = chnlTb.has_shared_tennis_court == true;
                        unit.HasPrivateTennisCourt = chnlTb.has_private_tennis_court == true;
                        unit.HasPrivateGarden = chnlTb.has_private_garden == true;
                        unit.HasClimbingFrame = chnlTb.has_climbing_frame == true;
                        unit.HasSwingSet = chnlTb.has_swwing_set == true;
                        unit.HasTrampoline = chnlTb.has_trampoline == true;
                        unit.HasBarbecue = chnlTb.has_barbecue == true;
                        unit.HasPrivateFishing = chnlTb.has_private_fishing == true;
                        unit.BicyclesAvailable = chnlTb.has_bicycle == true;
                        if (chnlTb.has_parking == true)
                        {
                            if (chnlTb.has_secure_parking == true)
                                unit.ParkingAvailable = Adverts.ParkingAvailable.AvailableAndSecure;
                            else
                                unit.ParkingAvailable = Adverts.ParkingAvailable.Available;
                        }
                        else
                            unit.ParkingAvailable = Adverts.ParkingAvailable.NotAvailable;
                        unit.HasBoat = chnlTb.has_boat;
                        unit.HasSolariumRoofTerrace = chnlTb.has_solanium_roof_terrace;
                        unit.HasBalconyOrTerrace = chnlTb.has_terrace;

                        if (chnlTb.available_corporate == 1)
                            unit.AvailableForCorporate = true;
                        else
                            unit.AvailableForCorporate = false;

                        if (chnlTb.available_henstag == 1)
                            unit.AvailableForHenStag = true;
                        else
                            unit.AvailableForHenStag = false;

                        if (chnlTb.available_house_swap == 1)
                            unit.AvailableForHouseSwap = true;
                        else
                            unit.AvailableForHouseSwap = false;

                        if (chnlTb.available_longlet == 1)
                            unit.AvailableForLongLet = true;
                        else
                            unit.AvailableForLongLet = false;

                        if (chnlTb.available_shortbreak == 1)
                            unit.AvailableForShortBreaks = true;
                        else
                            unit.AvailableForShortBreaks = false;

                        if (chnlTb.suitable_children == "0")
                            unit.SuitableForChildren = Adverts.SuitableForChildren.NotSuitable;
                        if (chnlTb.suitable_children == "1")
                            unit.SuitableForChildren = Adverts.SuitableForChildren.NotForUnder5s;
                        if (chnlTb.suitable_children == "2")
                            unit.SuitableForChildren = Adverts.SuitableForChildren.Yes;

                        if (chnlTb.restricted_mobility == "0")
                            unit.RestrictedMobility = Adverts.RestrictedMobility.NotSuitable;
                        if (chnlTb.restricted_mobility == "1")
                            unit.RestrictedMobility = Adverts.RestrictedMobility.YesWithLiftAccess;
                        if (chnlTb.restricted_mobility == "2")
                            unit.RestrictedMobility = Adverts.RestrictedMobility.Yes;

                        if (chnlTb.allow_pets == "0")
                            unit.AllowPets = Adverts.AllowPets.No;
                        if (chnlTb.allow_pets == "1")
                            unit.AllowPets = Adverts.AllowPets.PleaseEnquire;
                        if (chnlTb.allow_pets == "2")
                            unit.AllowPets = Adverts.AllowPets.Yes;

                        if (chnlTb.wheelchair_users == "0")
                            unit.WheelchairUsers = Adverts.WheelchairUsers.NotSuitable;
                        if (chnlTb.wheelchair_users == "1")
                            unit.WheelchairUsers = Adverts.WheelchairUsers.Accessible;
                        if (chnlTb.wheelchair_users == "2")
                            unit.WheelchairUsers = Adverts.WheelchairUsers.AccessibleAndAdapted;

                        if (chnlTb.allow_smoking == 1)
                            unit.AllowSmoking = true;
                        else
                            unit.AllowSmoking = false;


                        Adverts.WebServiceResponse inactiveUnitResponse = webService.InactiveUnitUpdateRequest(ownerTbl.ownerId, ownerTbl.ownerKey, unit);
                        // If the response is successful, display information message
                        if (inactiveUnitResponse.Success == true)
                        {
                            ChnlHolidayUtils.addLog("", "UpdateUnit_process", "IdEstate:" + IdEstate + " updated successfully", "");
                        }
                        else
                        {
                            foreach (string msg in inactiveUnitResponse.Messages)
                            {
                                ErrorString += msg + "\r\n";
                            }
                            ChnlHolidayUtils.addLog("", "ERROR:UpdateUnit_process", "IdEstate:" + IdEstate, ErrorString);
                        }


                        Adverts.InactiveSiteUpdateRequest InactiveSite = new Adverts.InactiveSiteUpdateRequest();
                        //HomeId is mandatory
                        InactiveSite.HomeId = chnlTb.homeId.ToInt64();
                        // Optional parameters below
                        InactiveSite.LocationContinent = "Europe";
                        InactiveSite.LocationCountry = "Italy";
                        InactiveSite.LocationRegion = "Lazio";
                        //InactiveSite.LocationSubRegion = "Oxfordshire";
                        InactiveSite.LocationTown = "Rome";// CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), 2, "Rome");
                        InactiveSite.PostalAddress = currEstate.loc_address;
                        InactiveSite.Postcode = currEstate.loc_zip_code;
                        if (currEstate.is_google_maps == 1)
                        {
                            if (!string.IsNullOrEmpty(currEstate.google_maps) && currEstate.google_maps.Split('|').Length > 1)
                            {
                                InactiveSite.MapLatitude = currEstate.google_maps.Split('|')[0].Replace(",", ".").objToDecimal();
                                InactiveSite.MapLongtitude = currEstate.google_maps.Split('|')[1].Replace(",", ".").objToDecimal();
                            }
                        }
                        //InactiveSite.TownDescription = "Enter your town description here";
                        //InactiveSite.RegionDescription = "Enter your region description here";
                        //InactiveSite.RuralHoliday = true;
                        //InactiveSite.MapZoomLevel = 13;
                        Adverts.WebServiceResponse inactiveSiteResponse = webService.InactiveSiteUpdateRequest(ownerTbl.ownerId, ownerTbl.ownerKey, InactiveSite);
                        // If the response is successful, display information message
                        if (inactiveSiteResponse.Success == true)
                        {
                            ChnlHolidayUtils.addLog("", "UpdateSite_process", "IdEstate:" + IdEstate + " updated successfully", "");
                        }
                        else
                        {
                            foreach (string msg in inactiveUnitResponse.Messages)
                            {
                                ErrorString += msg + "\r\n";
                            }
                            ChnlHolidayUtils.addLog("", "ERROR:UpdateSite_process", "IdEstate:" + IdEstate, ErrorString);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateUnit_process IdEstate:" + IdEstate, ex.ToString());
            }
        }

        public UpdateUnit_process(int idEstate, string host, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Host = host;
            ErrorString = "";
            if (backgroundProcess)
            {
                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlHolidayUpdate.UpdateUnit_process idEstate:" + idEstate);

                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();

            }
            else
                doThread();
        }
    }
    public static string UpdateUnit_start(int idEstate, string host)
    {
        UpdateUnit_process _tmp = new UpdateUnit_process(idEstate, host, false);
        return _tmp.ErrorString;
    }
}


