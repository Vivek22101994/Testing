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

public class ChnlAtraveoUtils
{
    public static bool CheckForService()
    {
        if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Response == null) return false;
        var Request = HttpContext.Current.Request;
        var Response = HttpContext.Current.Response;
        if (!Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/atraveo/")) return false;
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
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/atraveo/masterdata"))
        {
            Response.Write(ChnlAtraveoService.MasterDataRequest());
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/atraveo/priceinfo"))
        {
            Response.Write(ChnlAtraveoService.PriceInformationDataRequest());
        }

        Response.End();
        return true;
    }

    public static void SetTimers()
    {
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
            try
            {
                var dt = DateTime.Now.AddDays(-4);
                using (DCchnlAtraveo dc = new DCchnlAtraveo())
                {
                    dc.Delete(dc.dbRntChnlAtraveoRequestLOGs.Where(x => x.logDateTime <= dt));
                    dc.SaveChanges();
                }
                addLog("CLEAR LOG", "till " + dt, "", "", "");
            }
            catch (Exception Ex)
            {
                ErrorLog.addLog("", "ChnlAtraveoUtils.ClearLog_process", Ex.ToString() + "");
            }
        }
        public ClearLog_process()
        {
            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlAtraveoUtils.ClearLog_process");
        }
    }
    public static void ClearLog_start()
    {
        ClearLog_process _tmp = new ClearLog_process();
    }

    public static void addLog(string requestType, string requestComments, string requestContent, string responseContent, string requesUrl)
    {
        try
        {
            using (DCchnlAtraveo dc = new DCchnlAtraveo())
            {
                var item = new dbRntChnlAtraveoRequestLOG();
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
            ErrorLog.addLog("", "ChnlAtraveoUtils.addLog", Ex.ToString() + "");
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

            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            String strRSString = _Answer.ReadToEnd().ToString();
            if (CommonUtilities.getSYS_SETTING("rntChnlAtraveoDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlAtraveoDebug").ToInt32() == 1)
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

    public static void updateEstateFromMagarental(RNT_TB_ESTATE currEstate)
    {
        using (var dcRntOld = maga_DataContext.DC_RENTAL)
        using (DCmodRental dcRnt = new DCmodRental())
        using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
        {
            int IdEstate = currEstate.id;
            var currAtraveo = dcChnl.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == IdEstate);
            if (currAtraveo == null)
            {
                currAtraveo = new dbRntChnlAtraveoEstateTB() { id = IdEstate };
                dcChnl.Add(currAtraveo);
                dcChnl.SaveChanges();
                currAtraveo.isActive = 1;
                currAtraveo.pidMasterEstate = 0;
                currAtraveo.Name = currEstate.code;
                currAtraveo.Type = 0;
                currAtraveo.ArrivalDays = "YYYYYYY";
                currAtraveo.MinStay = currEstate.nights_min.objToInt32();
                dcChnl.SaveChanges();
            }
            List<RNT_LN_ESTATE> lstEstate = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == IdEstate).ToList();
            if (lstEstate != null && lstEstate.Count > 0)
            {
                foreach (RNT_LN_ESTATE objEstate in lstEstate)
                {
                    var pid_lang = contUtils.getLanguage_code(objEstate.pid_lang);
                    dbRntChnlAtraveoEstateLN currAtraveoLN = dcChnl.dbRntChnlAtraveoEstateLNs.SingleOrDefault(x => x.pidEstate == IdEstate && x.pidLang == pid_lang);
                    if (currAtraveoLN == null)
                    {
                        currAtraveoLN = new dbRntChnlAtraveoEstateLN();
                        currAtraveoLN.pidEstate = currAtraveo.id;
                        currAtraveoLN.pidLang = pid_lang;
                        dcChnl.Add(currAtraveoLN);
                        dcChnl.SaveChanges();
                    }
                    currAtraveoLN.Description = objEstate.description;
                    dcChnl.SaveChanges();
                }
            }
            var extrasIds = dcRntOld.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.is_HomeAway == 0).Select(x => x.pid_config.ToString()).ToList();
            foreach (var extrasId in extrasIds)
            {
                var featureValuesTbl = dcChnl.dbRntChnlAtraveoLkFeatureValuesTBLs.FirstOrDefault(x => x.refId == extrasId);
                if (featureValuesTbl != null)
                {
                    var currRl = dcChnl.dbRntChnlAtraveoEstateFeaturesRLs.FirstOrDefault(x => x.pidEstate == IdEstate && x.code == featureValuesTbl.code);
                    if (currRl == null)
                    {
                        currRl = new dbRntChnlAtraveoEstateFeaturesRL();
                        currRl.pidEstate = IdEstate;
                        currRl.code = featureValuesTbl.code;
                        dcChnl.Add(currRl);
                        dcChnl.SaveChanges();
                    }
                    if (featureValuesTbl.type == "yesno")
                    {
                        currRl.value = "1";
                        dcChnl.SaveChanges();
                    }
                }
            }
            //var extrasIds = dcRnt.dbRntEstateExtrasRLs.Where(x => x.pidEstate == IdEstate).ToList();
            //foreach (var extras in extrasIds)
            //{
            //    var featureValuesTbl = dcChnl.dbRntChnlAtraveoLkFeatureValuesTBLs.FirstOrDefault(x => x.refId == extras.pidEstateExtras + "");
            //    if (featureValuesTbl != null)
            //    {
            //        var extrasTb = rntProps.EstateExtrasTB.SingleOrDefault(x => x.id == extras.pidEstateExtras);
            //        if (extrasTb == null) continue;
            //        var currRl = dcChnl.dbRntChnlAtraveoEstateFeaturesRLs.FirstOrDefault(x => x.pidEstate == IdEstate && x.code == featureValuesTbl.code);
            //        if (currRl == null)
            //        {
            //            currRl = new dbRntChnlAtraveoEstateFeaturesRL();
            //            currRl.pidEstate = IdEstate;
            //            currRl.code = featureValuesTbl.code;
            //            dcChnl.Add(currRl);
            //            dcChnl.SaveChanges();
            //        }
            //        if (featureValuesTbl.type == "yesno")
            //            currRl.value = "1";
            //        if (featureValuesTbl.type == "numeric" && extrasTb.hasDistance.objToInt32() > 0)
            //        {
            //            if (extrasTb.hasDistance.objToInt32() == 1 || extrasTb.hasDistance.objToInt32() == 3)
            //                currRl.value = extras..objToInt32() + "";
            //            if (extrasTb.hasDistance.objToInt32() == 2)
            //                currRl.value = (extras.distance.objToInt32() * 1000).objToInt32() + "";
            //            if (extrasTb.hasDistance.objToInt32() == 4)
            //                currRl.value = (extras.distance.objToInt32() * 1400).objToInt32() + "";
            //            dcChnl.SaveChanges();
            //        }
            //    }
            //}
        }
    }
}
public static class ChnlAtraveoProps
{
    public static string IdAdMedia = "Atraveo";
}
public class ChnlAtraveoService
{
    public static string PriceInformationDataRequest()
    {

        long agentID = 0;

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCmodRental dc = new DCmodRental())
        using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            try
            {
                var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAtraveoProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    return "";
                }
                ChnlAtraveoClasses.PriceInformationDataResponse Response = new ChnlAtraveoClasses.PriceInformationDataResponse();
                List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID).Select(x => x.id).ToList();
                List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                estateIds = dcChnl.dbRntChnlAtraveoEstateTBs.Where(x => x.isActive == 1 && estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                foreach (var estate in estateList)
                {
                    var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == estate.id);
                    var unitChnlEstate = dcChnl.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == estate.id);
                    if (unitChnlEstate != null && currEstate != null)
                        Response.Objects.Add(GetObject(currEstate, unitChnlEstate));
                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlAtraveoService.MasterDataRequest", ex.ToString());
            }
        return "";
    }
    public static string MasterDataRequest()
    {

        long agentID = 0;

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCmodRental dc = new DCmodRental())
        using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            try
            {
                var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAtraveoProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    return "";
                }
                ChnlAtraveoClasses.MasterDataResponse Response = new ChnlAtraveoClasses.MasterDataResponse();
                List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID).Select(x => x.id).ToList();
                List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                estateIds = dcChnl.dbRntChnlAtraveoEstateTBs.Where(x => x.isActive == 1 && estateIds.Contains(x.id) && x.pidMasterEstate == 0).Select(x => (int?)x.id).ToList();
                var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                foreach (var estate in estateList)
                {
                    var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == estate.id);
                    var unitChnlEstate = dcChnl.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == estate.id);
                    if (unitChnlEstate != null && currEstate != null)
                        Response.Facilities.Add(GetFacility(currEstate, unitChnlEstate));
                }

                return Response.GetXml();
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ChnlAtraveoService.MasterDataRequest", ex.ToString());
            }
        return "";
    }
    public static ChnlAtraveoClasses.Facility GetFacility(RNT_TB_ESTATE currEstate, dbRntChnlAtraveoEstateTB currChnlEstate)
    {
        using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
        {
            int IdEstate = currEstate.id;
            var unitChnlEstates = dcChnl.dbRntChnlAtraveoEstateTBs.Where(x => x.id == IdEstate || x.pidMasterEstate == IdEstate).ToList();
            var currLnList = dcChnl.dbRntChnlAtraveoEstateLNs.Where(x => x.pidEstate == IdEstate).ToList();

            ChnlAtraveoClasses.Facility Response = new ChnlAtraveoClasses.Facility();
            Response._ID = IdEstate + "";
            Response.Name = currChnlEstate.Name + "";
            Response.Country = "IT";
            Response.Region = "Lazio";
            Response.Subregion = "";
            Response.City = CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), 2, "Rome");
            Response.CityPosition = new ChnlAtraveoClasses.LatLng();
            Response.District = "";

            foreach (var ln in currLnList)
            {
                Response.Description.Add(ln.DescriptionFacilities, ln.pidLang);
            }

            // Objects
            foreach (var unitChnlEstate in unitChnlEstates)
            {
                var unitEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == unitChnlEstate.id && x.is_active == 1);
                if (unitEstate == null)
                {
                    continue;
                }
                Response.Objects.Add(GetFacilityObject(unitEstate, unitChnlEstate));
            }

            return Response;
        }
    }
    public static ChnlAtraveoClasses.FacilityObject GetFacilityObject(RNT_TB_ESTATE currEstate, dbRntChnlAtraveoEstateTB currChnlEstate)
    {
        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
        {
            int IdEstate = currEstate.id;
            var currLnList = dcChnl.dbRntChnlAtraveoEstateLNs.Where(x => x.pidEstate == IdEstate).ToList();

            var dtStart = DateTime.Now.Date;
            var dtEnd = DateTime.Now.AddYears(2).Date;

            ChnlAtraveoClasses.FacilityObject Response = new ChnlAtraveoClasses.FacilityObject();
            Response._ID = IdEstate + "";
            Response.Type = currChnlEstate.Type.objToInt32();
            Response.Persons = currEstate.num_persons_max.objToInt32();
            //Response.Children = 0;
            //Response.Pets = currEstate.num_pets_max;
            Response.Rooms = currEstate.num_rooms_total.objToInt32();
            Response.Bedrooms = currEstate.num_rooms_bed.objToInt32();
            Response.Bathrooms = currEstate.num_rooms_bath.objToInt32();
            Response.Size = currEstate.mq_inner.objToInt32();
            //Response.Stars = 0;
            Response.ArrivalDays = currChnlEstate.ArrivalDays;
            if (currEstate.num_persons_min.objToInt32() > 0)
                Response.MinOccupancy = currEstate.num_persons_min.objToInt32();
            Response.MinStay = currChnlEstate.MinStay;
            Response.ArrivalDays = currChnlEstate.ArrivalDays;
            Response.Position.Street = currEstate.loc_address;
            Response.Position.ZipCode = currEstate.loc_zip_code;
            Response.Position.City = CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), 2, "Rome");
            if (currEstate.google_maps != "" && currEstate.google_maps != null)
            {
                if (currEstate.google_maps.Split('|').Length > 1)
                {
                    Response.Position.Latitude = currEstate.google_maps.Split('|')[0].Replace(",", ".");
                    Response.Position.Longitude = currEstate.google_maps.Split('|')[1].Replace(",", ".");
                }
                else if (currEstate.google_maps.Split(',').Length > 1)
                {
                    Response.Position.Latitude = currEstate.google_maps.Split(',')[0];
                    Response.Position.Longitude = currEstate.google_maps.Split(',')[1];
                }
            }

            //Response.DirectLink = "";

            //featureValues
            var features = dcChnl.dbRntChnlAtraveoEstateFeaturesRLs.Where(x => x.pidEstate == IdEstate).ToList();
            foreach (var feature in features)
            {
                var featureValue = new ChnlAtraveoClasses.Feature();
                featureValue._Code = feature.code;
                featureValue.Value = feature.value;
                Response.Features.Add(featureValue);
            }

            var images = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == "chnlAtraveo" && x.pid_estate == IdEstate).ToList();
            foreach (var image in images)
            {
                var tmp = new ChnlAtraveoClasses.Picture();
                tmp.URL = App.HOST + "/" + image.img_banner + "?maxsize=true";
                Response.Pictures.Add(tmp);
            }

            foreach (var ln in currLnList)
            {
                Response.Description.Add(ln.Description, ln.pidLang);
                Response.AddInfo.Add(ln.AddInfos, ln.pidLang);
            }

            return Response;
        }
    }
    public static ChnlAtraveoClasses.Object GetObject(RNT_TB_ESTATE currEstate, dbRntChnlAtraveoEstateTB currChnlEstate)
    {
        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
        {
            int IdEstate = currEstate.id;
            var currLnList = dcChnl.dbRntChnlAtraveoEstateLNs.Where(x => x.pidEstate == IdEstate).ToList();

            var dtStart = DateTime.Now.Date;
            var dtEnd = DateTime.Now.AddYears(2).Date;

            ChnlAtraveoClasses.Object Response = new ChnlAtraveoClasses.Object();
            Response._ID = IdEstate + "";
            Response.Vacancy.StartDate.ValueDate = dtStart;
            var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
            DateTime dtCurrent = dtStart;

            var closedList = dcChnl.dbRntChnlAtraveoEstateClosedDatesRLs.Where(x => x.pidEstate == IdEstate && x.changeDate >= dtStart && x.isClosed == 1).ToList();
            var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAtraveoProps.IdAdMedia);
            while (dtCurrent < dtEnd)
            {
                var tmpDate = closedList.FirstOrDefault(x => x.changeDate == dtCurrent);
                bool isAvv = false;
                if (tmpDate != null) isAvv = false;
                else
                    isAvv = resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count() == 0;
                Response.Vacancy.Vacancy.Add(isAvv ? "Y" : "N");
                dtCurrent = dtCurrent.AddDays(1);
            }


            int outError;
            var rateChangeList = dcChnl.dbRntChnlAtraveoEstateRateChangesRLs.Where(x => x.pidEstate == IdEstate && (x.weekday_changeIsDiscount >= 0 || x.weekly_changeIsDiscount >= 0) && x.changeDate >= dtStart).ToList();
            var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, agentTbl.id, dtStart, dtEnd, out outError);
            foreach (var tmp in priceListPerDates)
            {
                decimal currentPrice = 0;
                if (tmp.Prices.ContainsKey(currEstate.num_persons_max.objToInt32()))
                    currentPrice = tmp.Prices[currEstate.num_persons_max.objToInt32()];
                else if (tmp.Prices.Keys.Count > 0)
                    currentPrice = tmp.Prices.Last().Value;
                if (currentPrice == 0) currentPrice = 9999;
                var currRatePeriod = new ChnlAtraveoClasses.PriceClass();
                currRatePeriod._Type = "ta";
                currRatePeriod._DateFrom.ValueDate = tmp.DtStart;
                currRatePeriod._DateTo.ValueDate = tmp.DtEnd;
                var rateChange = rateChangeList.SingleOrDefault(x => x.changeDate >= tmp.DtStart && x.changeDate <= tmp.DtEnd);
                decimal changeAmount = 0;
                if (rateChange != null && rateChange.weekday_changeAmount > 0)
                {
                    changeAmount = (rateChange.weekday_changeIsPercentage == 1) ? (currentPrice * rateChange.weekday_changeAmount.objToInt32() / 100) : rateChange.weekday_changeAmount.objToInt32();
                    if (rateChange.weekday_changeIsDiscount == 1) { changeAmount = -changeAmount; }
                }
                else if (currChnlEstate.weekday_changeAmount > 0)
                {
                    changeAmount = (currChnlEstate.weekday_changeIsPercentage == 1) ? (currentPrice * currChnlEstate.weekday_changeAmount.objToInt32() / 100) : currChnlEstate.weekday_changeAmount.objToInt32();
                    if (currChnlEstate.weekday_changeIsDiscount == 1) { changeAmount = -changeAmount; }
                }
                currRatePeriod.Price.ValueDecimal = currentPrice + changeAmount;
                Response.Prices.Add(currRatePeriod);
            }

            var propertyFees = dcChnl.dbRntChnlAtraveoEstateFeeTBs.Where(x => x.pidEstate == IdEstate).ToList();
            foreach (var fee in propertyFees)
            {
                var propertyFee = new ChnlAtraveoClasses.SideCost();
                propertyFee._Code = fee.Code;
                propertyFee.Unit = fee.Unit;
                propertyFee.Cost.ValueDecimal = fee.Cost;
                propertyFee.CostType = fee.CostType;
                propertyFee.IntervalType = fee.IntervalType;
                propertyFee.MandatoryCode = fee.MandatoryCode;
                propertyFee.LocationOrder = fee.LocationOrder;
                Response.SideCosts.Add(propertyFee);
            }

            return Response;
        }
    }
}
public class ChnlAtraveoClasses
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
    public class Text
    {
        public string Locale { get; set; }
        public string Value { get; set; }
        public Text(string locale)
        {
            Locale = locale;
        }
        public Text(string value, string locale)
        {
            Value = value;
            Locale = locale;
        }
        public XElement GetElement(string name)
        {
            XElement elm = new XElement(name);
            elm.Add(new XAttribute("Language", Locale));
            elm.ReplaceNodes(new XCData(Value));
            return elm;
        }
    }
    public class Texts
    {
        public List<Text> List { get; set; }
        public Texts()
        {
            List = new List<Text>();
        }
        public void Add(string Value, string Locale)
        {
            List.Add(new Text(Value, Locale));
        }
        public bool HasValue()
        {
            return List.Count > 0;
        }
        public XElement GetElement(string name)
        {
            XElement elmParent = new XElement(name + "s");
            foreach (var tmp in List.Where(x => x.Value != null && x.Value.Trim() != ""))
                elmParent.Add(tmp.GetElement(name));
            return elmParent;
        }
    }
    public class LatLng
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public LatLng()
        {
        }
        public bool HasValue()
        {
            return !string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude);
        }
        public XElement GetElement(string name)
        {
            XElement elmParent = new XElement(name);
            elmParent.Add(new XElement("Latitude", Latitude));
            elmParent.Add(new XElement("Longitude", Longitude));
            elmParent.Add(elmParent);
            return elmParent;
        }
    }
    public class Picture
    {
        public string URL { get; set; }
        public Texts Description { get; set; }
        public Picture()
        {
            Description = new Texts();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("Picture");
            elm.Add(new XElement("URL", new XCData(URL)));
            if (Description.HasValue())
                elm.Add(Description.GetElement("Description"));
            return elm;
        }
    }
    public class Feature
    {
        public string _Code { get; set; }
        public string Value { get; set; }
        public Texts Detail { get; set; }
        public Feature()
        {
            Detail = new Texts();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("Feature");
            elm.Add(new XAttribute("Code", _Code));
            elm.Add(new XElement("Value", Value));
            if (Detail.HasValue())
                elm.Add(Detail.GetElement("Detail"));
            return elm;
        }
    }
    public class Position
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Position()
        {
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("Position");
            if (!string.IsNullOrEmpty(Street))
                elm.Add(new XElement("Street", Street));
            if (!string.IsNullOrEmpty(ZipCode))
                elm.Add(new XElement("ZipCode", ZipCode));
            if (!string.IsNullOrEmpty(City))
                elm.Add(new XElement("City", City));
            if (!string.IsNullOrEmpty(Longitude))
                elm.Add(new XElement("Longitude", Longitude));
            if (!string.IsNullOrEmpty(Latitude))
                elm.Add(new XElement("Latitude", Latitude));
            return elm;
        }
    }
    public class FacilityObject
    {
        public string _ID { get; set; }
        public int Type { get; set; }
        public int Persons { get; set; }
        public int? Children { get; set; }
        public int? Pets { get; set; }
        public int Rooms { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int? Size { get; set; }
        public int? Stars { get; set; }
        public string ArrivalDays { get; set; }
        public int? MinOccupancy { get; set; }
        public int? MinStay { get; set; }
        public Texts Description { get; set; }
        public Texts AddInfo { get; set; }
        public Position Position { get; set; }
        public List<Feature> Features { get; set; }
        public List<Picture> Pictures { get; set; }
        public string DirectLink { get; set; }
        public FacilityObject()
        {
            Description = new Texts();
            AddInfo = new Texts();
            Position = new Position();
            Features = new List<Feature>();
            Pictures = new List<Picture>();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("Object");
            elm.Add(new XAttribute("ID", _ID));
            elm.Add(new XElement("Type", Type));
            elm.Add(new XElement("Persons", Persons));
            if (Children.HasValue)
                elm.Add(new XElement("Children", Children));
            if (Pets.HasValue)
                elm.Add(new XElement("Pets", Pets));
            elm.Add(new XElement("Rooms", Rooms));
            elm.Add(new XElement("Bedrooms", Bedrooms));
            elm.Add(new XElement("Bathrooms", Bathrooms));
            if (Size.HasValue)
                elm.Add(new XElement("Size", Size));
            if (Stars.HasValue)
                elm.Add(new XElement("Stars", Stars));
            elm.Add(new XElement("ArrivalDays", ArrivalDays));
            if (MinOccupancy.HasValue)
                elm.Add(new XElement("MinOccupancy", MinOccupancy));
            if (MinStay.HasValue)
                elm.Add(new XElement("MinStay", MinStay));
            if (Description.HasValue())
                elm.Add(Description.GetElement("Description"));
            if (AddInfo.HasValue())
                elm.Add(AddInfo.GetElement("AddInfo"));
            elm.Add(Position.GetElement());
            if (Features.Count > 0)
            {
                var FeaturesElm = new XElement("Features");
                foreach (var tmp in Features)
                    FeaturesElm.Add(tmp.GetElement());
                elm.Add(FeaturesElm);
            }
            if (Pictures.Count > 0)
            {
                var PicturesElm = new XElement("Pictures");
                foreach (var tmp in Pictures)
                    PicturesElm.Add(tmp.GetElement());
                elm.Add(PicturesElm);
            }
            if (!string.IsNullOrEmpty(DirectLink))
                elm.Add(new XElement("DirectLink", new XCData(DirectLink)));
            return elm;
        }
    }
    public class Facility
    {
        public string _ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public string City { get; set; }
        public LatLng CityPosition { get; set; }
        public string District { get; set; }
        public Texts Description { get; set; }
        public List<FacilityObject> Objects { get; set; }
        public Facility()
        {
            CityPosition = new LatLng();
            Description = new Texts();
            Objects = new List<FacilityObject>();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("Facility");
            elm.Add(new XAttribute("ID", _ID));
            if (!string.IsNullOrEmpty(Name))
                elm.Add(new XElement("Name", Name));
            if (!string.IsNullOrEmpty(Country))
                elm.Add(new XElement("Country", Country));
            if (!string.IsNullOrEmpty(Region))
                elm.Add(new XElement("Region", Region));
            if (!string.IsNullOrEmpty(Subregion))
                elm.Add(new XElement("Subregion", Subregion));
            if (!string.IsNullOrEmpty(City))
                elm.Add(new XElement("City", City));
            if (CityPosition.HasValue())
                elm.Add(CityPosition.GetElement("CityPosition"));
            if (!string.IsNullOrEmpty(District))
                elm.Add(new XElement("District", District));
            if (Description.HasValue())
                elm.Add(Description.GetElement("Description"));
            if (Objects.Count > 0)
            {
                var imagesElm = new XElement("Objects");
                foreach (var tmp in Objects)
                    imagesElm.Add(tmp.GetElement());
                elm.Add(imagesElm);
            }
            return elm;
        }
    }
    public class MasterDataResponse
    {
        public List<Facility> Facilities { get; set; }
        public MasterDataResponse()
        {
            Facilities = new List<Facility>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("Facilities");
            if (Facilities.Count > 0)
            {
                foreach (var tmp in Facilities)
                    root.Add(tmp.GetElement());
            }
            _resource.Add(root);
            return _resource.ToString();
        }
    }
    public class SideCost
    {
        public string _Code { get; set; }
        public string Currency { get; set; }
        public string Unit { get; set; }
        public DecimalUnit Cost { get; set; }
        public string CostType { get; set; }
        public string IntervalType { get; set; }
        public int MandatoryCode { get; set; }
        public string LocationOrder { get; set; }
        public SideCost()
        {
            Currency = "EUR";
            Cost = new DecimalUnit();
            CostType = "Cost";
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("SideCost");
            elm.Add(new XAttribute("Code", _Code));
            elm.Add(new XElement("Currency", Currency));
            elm.Add(new XElement(CostType, Cost.Value));
            elm.Add(new XElement("Unit", Unit));
            if (IntervalType != "" && IntervalType != "ko")
                elm.Add(new XElement("IntervalType", IntervalType));
            elm.Add(new XElement("MandatoryCode", MandatoryCode));
            elm.Add(new XElement(LocationOrder, "1"));
            return elm;
        }
    }
    public class PriceClass
    {
        public string _Type { get; set; }
        public Date _DateFrom { get; set; }
        public Date _DateTo { get; set; }
        public int? _Persons { get; set; }
        public DecimalUnit Price { get; set; }
        public DecimalUnit AddPersonPrice { get; set; }
        public bool PerPerson { get; set; }
        public DecimalUnit Reduction2ndWeek { get; set; }
        public PriceClass()
        {
            _DateFrom = new Date();
            _DateTo = new Date();
            Price = new DecimalUnit();
            AddPersonPrice = new DecimalUnit();
            PerPerson = false;
            Reduction2ndWeek = new DecimalUnit();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("Price");
            elm.Add(new XAttribute("Type", _Type));
            elm.Add(new XAttribute("DateFrom", _DateFrom.Value));
            elm.Add(new XAttribute("DateTo", _DateTo.Value));
            if (_Persons.HasValue)
                elm.Add(new XAttribute("Persons", _Persons));
            elm.Add(Price.GetElement("Price"));
            if (AddPersonPrice.Value != "")
                elm.Add(AddPersonPrice.GetElement("AddPersonPrice"));
            if (Reduction2ndWeek.Value != "")
                elm.Add(Reduction2ndWeek.GetElement("Reduction2ndWeek"));
            if (AddPersonPrice.Value != "")
                elm.Add(AddPersonPrice.GetElement("AddPersonPrice"));
            if (PerPerson)
                elm.Add(new XElement("PerPerson", "1"));
            return elm;
        }
    }
    public class VacancyClass
    {
        public Date StartDate { get; set; }
        public List<string> Vacancy { get; set; }
        public VacancyClass()
        {
            StartDate = new Date();
            Vacancy = new List<string>();
        }
        public bool HasValue()
        {
            return !string.IsNullOrEmpty(StartDate.Value) && Vacancy.Count > 0;
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("Vacancy");
            elm.Add(new XElement("StartDate", StartDate.Value));
            elm.Add(new XElement("Vacancy", Vacancy.listToString("")));
            return elm;
        }
    }
    public class Object
    {
        public string _ID { get; set; }
        public VacancyClass Vacancy { get; set; }
        public List<PriceClass> Prices { get; set; }
        public List<SideCost> SideCosts { get; set; }
        public Object()
        {
            Vacancy = new VacancyClass();
            Prices = new List<PriceClass>();
            SideCosts = new List<SideCost>();
        }
        public XElement GetElement()
        {
            XElement elm = new XElement("Object");
            elm.Add(new XAttribute("ID", _ID));
            if (Vacancy.HasValue())
                elm.Add(Vacancy.GetElement());
            if (Prices.Count > 0)
            {
                var PricesElm = new XElement("Prices");
                foreach (var tmp in Prices)
                    PricesElm.Add(tmp.GetElement());
                elm.Add(PricesElm);
            }
            if (SideCosts.Count > 0)
            {
                var SideCostsElm = new XElement("SideCosts");
                foreach (var tmp in SideCosts)
                    SideCostsElm.Add(tmp.GetElement());
                elm.Add(SideCostsElm);
            }
            return elm;
        }
    }
    public class PriceInformationDataResponse
    {
        public List<Object> Objects { get; set; }
        public PriceInformationDataResponse()
        {
            Objects = new List<Object>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("Objects");
            if (Objects.Count > 0)
            {
                foreach (var tmp in Objects)
                    root.Add(tmp.GetElement());
            }
            _resource.Add(root);
            return _resource.ToString();
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
}
public static class ChnlAtraveoExts
{
}
