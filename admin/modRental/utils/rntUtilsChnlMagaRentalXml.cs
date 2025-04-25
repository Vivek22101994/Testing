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



public class ChnlMagaRentalXmlUtils
{
    public static dbRntAgentTBL CheckAuth(string auth)
    {
        Guid uid;
        if (!Guid.TryParse("" + auth, out uid))
            return null;
        using (DCmodRental dc = new DCmodRental())
            return dc.dbRntAgentTBLs.SingleOrDefault(x => x.uid == uid);
    }
    public static bool CheckForService()
    {
        if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Response == null) return false;
        var Request = HttpContext.Current.Request;
        var Response = HttpContext.Current.Response;
        if (!Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/magarental/")) return false;
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
        var agentTbl = CheckAuth(Request.QueryString["auth"]);
        if (agentTbl == null)
        {
            Response.Write("<root><check>fail</check><error>Authentication error.</error></root>");
            Response.End();
            return true;
        }
        var pidLang = Request.QueryString["ln"].objToInt32();
        var langTbl = contProps.LangTBL.SingleOrDefault(x => x.id == pidLang && x.is_active == 1 && x.is_public == 1);
        if (langTbl == null) pidLang = App.DefLangID;
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/magarental/search"))
        {
            requestType = "search";
            responseContent = ChnlMagaRentalXmlService.Search(xml, agentTbl, pidLang);
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/magarental/quoterequest"))
        {
            requestType = "quoterequest";
            responseContent = ChnlMagaRentalXmlService.QuoteRequest(xml, agentTbl, pidLang);
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/magarental/bookingrequest"))
        {
            requestType = "bookingrequest";
            responseContent = ChnlMagaRentalXmlService.BookingRequest(xml, agentTbl, pidLang);
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/magarental/bookings"))
        {
            requestType = "bookings";
            responseContent = ChnlMagaRentalXmlService.BookingContentIndexRequest(xml, agentTbl, pidLang);
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/magarental/booking/"))
        {
            var uid = Request.Url.LocalPath.ToLower().Replace("/chnlutils/magarental/booking/", "");
            var reservationTbl = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.unique_id.ToString().ToLower() == uid);
            if (reservationTbl != null)
            {
                requestType = "booking/" + reservationTbl.id;
                responseContent = ChnlMagaRentalXmlService.Booking(reservationTbl.id, agentTbl, pidLang);
            }
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/magarental/listings"))
        {
            requestType = "listings";
            responseContent = ChnlMagaRentalXmlService.ListingContentIndexRequest(agentTbl, pidLang);
        }
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/magarental/listing/"))
        {
            var id = Request.Url.LocalPath.ToLower().Replace("/chnlutils/magarental/listing/", "").ToInt32();
            responseContent = ChnlMagaRentalXmlService.Listing(id, agentTbl, pidLang);
        }
        if (CommonUtilities.getSYS_SETTING("rntChnlMagaRentalXmlDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlMagaRentalXmlDebug").ToInt32() == 1)
            addLog(requestType, requestComments, requestContent, responseContent, requesUrl);
        Response.Write(responseContent);
        Response.End();
        return true;
    }

    public static void SetTimers()
    {

        timerClearLog = new System.Timers.Timer();
        timerClearLog.Interval = (1000 * 60 * 5); // first after 5 mins
        timerClearLog.Elapsed += new System.Timers.ElapsedEventHandler(timerClearLog_Elapsed);
        timerClearLog.Start();
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
            using (DCchnlMagaRentalXml dc = new DCchnlMagaRentalXml())
            {
                dc.Delete(dc.dbRntChnlMagaRentalXmlRequestLOGs.Where(x => x.logDateTime <= dt));
                dc.SaveChanges();
            }
            addLog("CLEAR LOG", "till " + dt, "", "", "");
        }
        public ClearLog_process()
        {
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "ChnlMagaRentalXmlUtils.ClearLog_process");
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

    public static void addLog(string requestType, string requestComments, string requestContent, string responseContent, string requesUrl)
    {
        try
        {
            using (DCchnlMagaRentalXml dc = new DCchnlMagaRentalXml())
            {
                var item = new dbRntChnlMagaRentalXmlRequestLOG();
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
            ErrorLog.addLog("", "ChnlMagaRentalXmlUtils.addLog", Ex.ToString() + "");
        }
    }
    public static void updateEstateFromMagarental(RNT_TB_ESTATE currEstate)
    {
        using (DCmodRental dcRnt = new DCmodRental())
        using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
        {
            int IdEstate = currEstate.id;
            List<RNT_LN_ESTATE> lstEstate = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == IdEstate).ToList();
            if (lstEstate != null && lstEstate.Count > 0)
            {
                foreach (RNT_LN_ESTATE objEstate in lstEstate)
                {
                    dbRntChnlMagaRentalXmlEstateLN currHomeawayLN = dcChnl.dbRntChnlMagaRentalXmlEstateLNs.SingleOrDefault(x => x.pidEstate == IdEstate && x.pidLang == objEstate.pid_lang);
                    if (currHomeawayLN == null)
                    {
                        currHomeawayLN = new dbRntChnlMagaRentalXmlEstateLN();
                        currHomeawayLN.pidEstate = currEstate.id;
                        currHomeawayLN.pidLang = objEstate.pid_lang;
                        dcChnl.Add(currHomeawayLN);
                        dcChnl.SaveChanges();
                    }
                    currHomeawayLN.title = objEstate.title;
                    currHomeawayLN.summary = objEstate.summary;
                    currHomeawayLN.description = objEstate.description;
                    dcChnl.SaveChanges();
                }
            }
        }
    }
}
public static class ChnlMagaRentalXmlProps
{
    public static string IdAdMedia = "MagaRentalXml";
}
public class ChnlMagaRentalXmlService
{
    private static decimal getPriceWithoutTax(decimal amount, decimal taxPercentage)
    {
        decimal TMPcashTaxFreeDecimal = Decimal.Divide(amount, (1 + taxPercentage));
        decimal TMPcashTaxFreeRounded = Decimal.Round(TMPcashTaxFreeDecimal, 2);
        if (TMPcashTaxFreeRounded > TMPcashTaxFreeDecimal)
            TMPcashTaxFreeRounded -= new Decimal(0.01);
        return TMPcashTaxFreeRounded;
    }
    public static string ListingContentIndexRequest(dbRntAgentTBL agentTbl, int pidLang)
    {
        long agentID = agentTbl.id;
        ChnlMagaRentalXmlClasses.ErrorList errorList = new ChnlMagaRentalXmlClasses.ErrorList("ListingContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCmodRental dc = new DCmodRental())
        using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
            try
            {
                ChnlMagaRentalXmlClasses.ListingContentIndexResponse Response = new ChnlMagaRentalXmlClasses.ListingContentIndexResponse();
                List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID).Select(x => x.id).ToList();
                List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                var estateList = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).ToList();

                foreach (var estate in estateList)
                {
                    var entry = new ChnlMagaRentalXmlClasses.ListingContentIndexEntry();
                    entry.listingExternalId = estate.id + "";
                    entry.active = true;
                    entry.lastUpdatedDate.ValueDate = DateTime.Now;
                    entry.listingUrl = App.HOST_SSL + "/chnlutils/magarental/listing/" + estate.id + "?auth=" + agentTbl.uid + "&ln=" + pidLang;
                    Response.listingContentIndexEntry.Add(entry);
                }
                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "ListingContentIndex", ex.ToString());
            }
        return errorList.GetXml();
    }
    public static string Listing(int IdEstate, dbRntAgentTBL agentTbl, int pidLang)
    {
        using (DCmodAuth dcAuth = new DCmodAuth())
        using (DCmodRental dcRnt = new DCmodRental())
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
        {
            RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
            if (currEstate == null)
            {
                return ChnlMagaRentalXmlClasses.ErrorList.GetErrorXml("OTHER", "Listing");
            }
            var Units = AppSettings.RNT_TB_ESTATE.Where(x => x.id == IdEstate).ToList();
            var currLn = dcChnl.dbRntChnlMagaRentalXmlEstateLNs.SingleOrDefault(x => x.pidEstate == IdEstate && x.pidLang == pidLang);

            var dtStart = DateTime.Now.Date;
            var dtEnd = DateTime.Now.AddYears(2).Date;

            ChnlMagaRentalXmlClasses.ListingClasses.Listing Response = new ChnlMagaRentalXmlClasses.ListingClasses.Listing();
            Response.externalId = IdEstate + "";
            Response.active = true;
            if (agentTbl.chnlMGetTexts == 1 && currLn != null)
            {
                Response.adContent.title = currLn.title;
                Response.adContent.summary = currLn.summary;
                Response.adContent.description = currLn.description;
            }

            //featureValues
            var extrasRls = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.is_HomeAway == 0).ToList();
            //var extrasRls = dcRnt.dbRntEstateExtrasRLs.Where(x => x.pidEstate == IdEstate).ToList();
            if (agentTbl.chnlMGetAmenities == 1)
            {
                List<ChnlMagaRentalXmlClasses.ListingClasses.Amenities> amenities = new List<ChnlMagaRentalXmlClasses.ListingClasses.Amenities>();
                foreach (var extrasRl in extrasRls)
                {
                    var extrasTb = DC_RENTAL.RNT_TB_CONFIGs.SingleOrDefault(x => x.id == extrasRl.pid_config);
                    if (extrasTb == null) continue;
                    var extrasTitle = CurrentSource.rnt_configTitle(extrasTb.id, pidLang, "");
                    if (extrasTitle != "")
                    {
                        ChnlMagaRentalXmlClasses.ListingClasses.Amenities amenity = new ChnlMagaRentalXmlClasses.ListingClasses.Amenities();
                        amenity.title = extrasTitle;
                        amenity.price._type = "";
                        amenity.price._rate.ValueDecimal = 0;
                        //amenity.mandatory = extrasTb.isRequired == 1 ? true : false;
                        //amenity.instantPayment = extrasTb.isInstantPayment == 1 ? true : false;
                        //Dictionary<string, decimal> prices = getAmenityPrice(IdEstate, extrasTb.id, pidLang);
                        //if (prices != null && prices.Count > 0)
                        //{
                        //    amenity.price._type = prices.FirstOrDefault().Key;
                        // amenity.price._rate.ValueDecimal = prices.FirstOrDefault().Value;
                        //}
                        amenities.Add(amenity);
                    }
                    Response.amenities = amenities;
                }
            }
            //LOCATION
            if (agentTbl.chnlMGetAddress == 1)
            {
                Response.location.address.addressLine1 = currEstate.loc_address;
                Response.location.address.addressLine2 = currEstate.loc_inner_bell;
            }
            Response.location.address.postalCode = currEstate.loc_zip_code;
            Response.location.address.city = CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), App.DefLangID, "");
            Response.location.address.stateOrProvince = CommonUtilities.getSYS_SETTING("HAStateProvince"); //CurrentSource.loc(currEstate.pid_city.objToInt32(), App.DefLangID, "");
            Response.location.address.country = CommonUtilities.getSYS_SETTING("HACountry"); //locUtils.getCity_codeOfCountry(currEstate.pid_city.objToInt32(), App.DefLangID, "");

            //Response.hasCityTax = currEstate.pr_has_overnight_tax.objToInt32() == 1 ? true : false;
            //// City Tax
            //if (Response.hasCityTax)
            //{
            //    using (DCmodLocation DCLoc = new DCmodLocation())
            //    {
            //        var currCity = DCLoc.dbLocCityTBs.SingleOrDefault(x => x.id == currEstate.pid_city.objToInt32());
            //        if (currCity == null || currCity.cityTaxType + "" == "")
            //            Response.hasCityTax = false;
            //        else
            //        {
            //            Response.cityTax.adultRate = currCity.cityTaxAmount.objToDecimal();
            //            Response.cityTax.childRate = currCity.cityTaxAmountForChildren.objToDecimal();
            //            Response.cityTax.maxDays = currCity.cityTaxMaxNights.objToInt32();
            //            Response.cityTax.payInAdvance = currCity.cityTaxPayedInAdvance.objToInt32();                        
            //        }
            //    }
            //}

            // rntUtils.estate_getCityTax(

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

            if (agentTbl.chnlMGetPhotos == 1)
            {
                List<RNT_RL_ESTATE_MEDIA> images = new List<RNT_RL_ESTATE_MEDIA>();
                //if (agentTbl.isInternalWebsite.objToInt32() == 1)
                //    images = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == "gallery" && x.pid_estate == IdEstate).OrderBy(x => x.sequence).ToList();
                //else
                images = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == ChnlMagaRentalXmlProps.IdAdMedia && x.pid_estate == IdEstate).OrderBy(x => x.sequence).ToList();

                foreach (var image in images)
                {
                    var tmp = new ChnlMagaRentalXmlClasses.ListingClasses.Image();
                    tmp.uri = App.HOST + "/" + image.img_banner + "?maxsize=true";
                    Response.images.Add(tmp);
                }
            }
            // UNITS
            foreach (var unit in Units)
            {
                var unitEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == unit.id && x.is_active == 1);
                //dbRntEstateExtrasPriceTBL currEstatePrice = rntProps.EstateExtrasPriceTBL.SingleOrDefault(x => x.pidExtras == unit.id && x.pidEstate == IdEstate);
                //List<dbRntExtrasPriceTBL> lstPrice = rntProps.ExtrasPriceTBL.Where(x => x.pidExtras == unit.id).ToList();

                if (unitEstate == null)
                {
                    continue;
                }
                var currUnit = new ChnlMagaRentalXmlClasses.ListingClasses.Unit();
                currUnit.externalId = unit.id + "";
                currUnit.active = true;
                currUnit.area = unit.mq_inner.objToInt32();
                currUnit.bathrooms = unit.num_rooms_bath.objToInt32();
                currUnit.bedrooms = unit.num_rooms_bed.objToInt32();
                currUnit.maxSleep = unit.num_persons_max.objToInt32();
                if (unit.category == "apt")
                    currUnit.propertyType = "Appartamento";
                else if (unit.category == "villa")
                    currUnit.propertyType = "Villa";
                //currUnit.propertyType = unit.   rntUtils.getEstateCategory_title(unit.pid_category.objToInt32(), pidLang, "");
                currUnit.unitName = CurrentSource.rntEstate_title(unit.id, pidLang, "");

                ////featureValues
                //if (agentTbl.chnlMGetAmenities == 1 && unit.id != IdEstate)
                //{
                //    extrasRls = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.is_HomeAway == 0).ToList();
                //    foreach (var extrasRl in extrasRls)
                //    {
                //        var extrasTb = DC_RENTAL.RNT_TB_CONFIGs.SingleOrDefault(x => x.id == extrasRl.pid_config);
                //        if (extrasTb == null) continue;
                //        var extrasTitle = CurrentSource.rnt_configTitle(extrasTb.id, pidLang, "");
                //        if (extrasTitle != "")
                //            currUnit.amenities.Add(extrasTitle);
                //        //var price = getAmenityPrice(unit.id, extrasTb.id, pidLang);
                //    }
                //}

                // TODO ratePeriods, Finalaize
                int outError;
                //var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, dtStart, dtEnd, 0, out outError);
                var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate, agentTbl.id, dtStart, dtEnd, out outError);
                foreach (var tmp in priceListPerDates)
                {
                    var currRatePeriod = new ChnlMagaRentalXmlClasses.ListingClasses.RatePeriod();
                    currRatePeriod.dateRange.beginDate.ValueDate = tmp.DtStart;
                    currRatePeriod.dateRange.endDate.ValueDate = tmp.DtEnd;
                    currRatePeriod.minimumStay = tmp.MinStay;

                    string arrivalDay = "y,y,y,y,y,y,y";
                    string departureDay = "y,y,y,y,y,y,y";
                    string sep = ",";

                    currRatePeriod.allowedDays.checkinAllows = arrivalDay;
                    currRatePeriod.allowedDays.checkoutAllows = departureDay;

                    //if (tmp.lstClosedArrival.Count > 0)
                    //{
                    //    for (int i = 1; i <= 7; i++)
                    //    {
                    //        arrivalDay += (tmp.lstClosedArrival.Contains(i) ? "n" : "y") + sep;
                    //    }
                    //    currRatePeriod.allowedDays.checkinAllows = arrivalDay;
                    //}

                    //if (tmp.lstClosedDeparture.Count > 0)
                    //{
                    //    for (int i = 1; i <= 7; i++)
                    //    {
                    //        departureDay += (tmp.lstClosedDeparture.Contains(i) ? "n" : "y") + sep;
                    //    }
                    //    currRatePeriod.allowedDays.checkoutAllows = departureDay;
                    //}

                    foreach (var Price in tmp.Prices)
                    {
                        var PerOccupancy = new ChnlMagaRentalXmlClasses.ListingClasses.RatePeriod.PerOccupancy();
                        currRatePeriod.perOccupancy.Add(PerOccupancy);
                        PerOccupancy._occupancy = Price.Key;
                        PerOccupancy._rate.ValueDecimal = Price.Value;
                    }
                    currUnit.ratePeriods.Add(currRatePeriod);
                }

                // TODO unitAvailability, Finalaize
                currUnit.unitAvailability.dateRange.beginDate.ValueDate = dtStart;
                currUnit.unitAvailability.dateRange.endDate.ValueDate = dtEnd;
                var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                DateTime dtCurrent = dtStart;
                while (dtCurrent < dtEnd)
                {
                    var units = 1 - resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count();
                    var priceList = priceListPerDates.FirstOrDefault(x => x.DtStart <= dtCurrent && x.DtEnd >= dtCurrent);
                    currUnit.unitAvailability.availability.Add(units + "");
                    currUnit.unitAvailability.minStay.Add(priceList != null ? priceList.MinStay + "" : currEstate.nights_min.objToInt32() + "");
                    dtCurrent = dtCurrent.AddDays(1);
                }

                Response.units.Add(currUnit);
            }
            return Response.GetXml();
        }
    }
    public static string BookingContentIndexRequest(string xml, dbRntAgentTBL agentTbl, int pidLang)
    {
        long agentID = agentTbl.id;
        ChnlMagaRentalXmlClasses.ErrorList errorList = new ChnlMagaRentalXmlClasses.ErrorList("bookingContentIndex");

        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCmodRental dc = new DCmodRental())
        using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
            try
            {
                ChnlMagaRentalXmlClasses.BookingContentIndexRequest Request = new ChnlMagaRentalXmlClasses.BookingContentIndexRequest(xml);
                List<long> contractIds = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == agentID).Select(x => x.id).ToList();
                List<int?> estateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && contractIds.Contains(x.pidAgentContract)).Select(x => (int?)x.pidEstate).ToList();
                estateIds = AppSettings.RNT_estateList.Where(x => estateIds.Contains(x.id)).Select(x => (int?)x.id).ToList();
                var reservationList = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => estateIds.Contains(x.pid_estate) && x.state_pid == 4 && x.agentID == agentID && x.state_date.HasValue).ToList();
                if (Request.lastUpdatedDate.ValueDate.HasValue)
                {
                    var resIds = dc.dbRntReservationLastUpdatedLOGs.Where(x => x.lastUpdatedDate >= Request.lastUpdatedDate.ValueDate.Value).Select(x => x.id).ToList();
                    reservationList = reservationList.Where(x => resIds.Contains(x.id) || x.state_date.Value >= Request.lastUpdatedDate.ValueDate.Value).ToList();
                }

                ChnlMagaRentalXmlClasses.BookingContentIndexResponse Response = new ChnlMagaRentalXmlClasses.BookingContentIndexResponse();
                foreach (var reservation in reservationList)
                {
                    var entry = new ChnlMagaRentalXmlClasses.BookingContentIndexEntry();
                    entry.active = true;
                    var lastUpdatedDate = dc.dbRntReservationLastUpdatedLOGs.SingleOrDefault(x => x.id == reservation.id);
                    entry.lastUpdatedDate.ValueDate = lastUpdatedDate != null ? lastUpdatedDate.lastUpdatedDate : reservation.state_date.Value;
                    entry.bookingUrl = App.HOST_SSL + "/chnlutils/magarental/booking/" + reservation.unique_id + "?auth=" + agentTbl.uid + "&ln=" + pidLang;
                    Response.bookingContentIndexEntryList.Add(entry);
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
    public static string Booking(long id, dbRntAgentTBL agentTbl, int pidLang)
    {
        using (DCmodAuth dcAuth = new DCmodAuth())
        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
        using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
        {
            ChnlMagaRentalXmlClasses.BookingResponse Response = new ChnlMagaRentalXmlClasses.BookingResponse();
            var reservationTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == id && x.agentID == agentTbl.id);
            if (reservationTbl == null)
            {
                return ChnlMagaRentalXmlClasses.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == reservationTbl.pid_estate.objToInt32());
            if (currEstateTB == null)
            {
                return ChnlMagaRentalXmlClasses.ErrorList.GetErrorXml("OTHER", "bookingResponse");
            }
            var listPay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == reservationTbl.id && x.is_complete == 1 && x.direction == 1 && x.pr_total.HasValue).ToList();
            decimal totalPayed = listPay.Count == 0 ? 0 : listPay.Sum(x => x.pr_total.Value);

            Response.reservation.numberOfAdults = reservationTbl.num_adult.objToInt32();
            Response.reservation.numberOfChildren = reservationTbl.num_child_over.objToInt32() + reservationTbl.num_child_min.objToInt32();
            Response.reservation.reservationDates.beginDate.ValueDate = reservationTbl.dtStart;
            Response.reservation.reservationDates.endDate.ValueDate = reservationTbl.dtEnd;

            Response.inquirer = new ChnlMagaRentalXmlClasses.Inquirer();
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
                if (clientTbl.contactPhoneMobile.splitStringToList("_").Count == 2)
                {
                    Response.inquirer.phoneCountryCode = clientTbl.contactPhoneMobile.splitStringToList("_")[0];
                    Response.inquirer.phoneNumber = clientTbl.contactPhoneMobile.splitStringToList("_")[1];
                }
                else
                {
                    Response.inquirer.address.addressLine1 = clientTbl.locAddress;
                }
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

            Response.listingExternalId = currEstateTB.id + "";
            Response.unitExternalId = currEstateTB.id + "";
            Response.externalId = reservationTbl.id + "";
            Response.locale = contUtils.getLang_commonName(reservationTbl.cl_pid_lang.objToInt32()).Replace("-", "_");
            if (string.IsNullOrEmpty(Response.locale)) Response.locale = "en_GB";
            var order = new ChnlMagaRentalXmlClasses.Order();
            Response.orderList.Add(order);

            string currency = CommonUtilities.getSYS_SETTING("currency");
            if (currency == "") currency = "EUR";

            order.currency = currency;
            //decimal totalExtras = 0;
            //var currResExtras = DC_RENTAL.RNT_TBL_EXTRA_RESERVATION.Where(x => x.pidReservation == reservationTbl.id).ToList();
            //foreach (var extra in currResExtras)
            //{
            //    totalExtras += extra.price.objToDecimal();
            //}

            var orderItem = new ChnlMagaRentalXmlClasses.OrderItem();
            order.orderItemList.Add(orderItem);
            orderItem.feeType = "RENTAL";
            orderItem.description = "Rent";
            orderItem.name = "Rent";
            orderItem.commissionAmount.ValueDecimal = reservationTbl.agentCommissionPrice;
            //orderItem.totalAmount.ValueDecimal = reservationTbl.pr_total.objToDecimal() - totalExtras - reservationTbl.pr_cityTax.objToDecimal();
            //orderItem.totalAmount.ValueDecimal = reservationTbl.pr_total.objToDecimal() - totalExtras;
            orderItem.totalAmount.ValueDecimal = reservationTbl.pr_total.objToDecimal();
            orderItem.status = "ACCEPTED"; // PENDING, ACCEPTED, DECLINED, DECLINED_BY_SYSTEM, CANCELLED, EDITED

            //foreach (var extra in currResExtras)
            //{
            //    orderItem = new ChnlMagaRentalXmlClasses.OrderItem();
            //    order.orderItemList.Add(orderItem);
            //    orderItem.feeType = "MISC";
            //    orderItem.name = orderItem.description = rntUtils.getEstateExtras_title(extra.pidExtra, App.DefLangID, "");
            //    orderItem.totalAmount.ValueDecimal = extra.price.objToDecimal();
            //}

            //if (reservationTbl.pr_cityTax.objToDecimal() > 0)
            //{
            //    orderItem = new ChnlMagaRentalXmlClasses.OrderItem();
            //    order.orderItemList.Add(orderItem);
            //    orderItem.feeType = "TAX";
            //    orderItem.name = orderItem.description = "CityTax";
            //    orderItem.totalAmount.ValueDecimal = reservationTbl.pr_cityTax;
            //}

            // TODO, finalise
            //var paymentScheduleItem = new ChnlMagaRentalXmlClasses.PaymentScheduleItem();
            //paymentScheduleItem.amount.ValueDecimal = reservationTbl.pr_total.objToDecimal();
            //paymentScheduleItem.dueDate.ValueDate = reservationTbl.dtStart.Value.AddDays(-30);
            //if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
            //order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

            #region Scheduled Payment
            //Part Payment
            var paymentScheduleItem = new ChnlMagaRentalXmlClasses.PaymentScheduleItem();
            paymentScheduleItem.amount.ValueDecimal = reservationTbl.pr_part_forPayment.objToDecimal();
            paymentScheduleItem.dueDate.ValueDate = reservationTbl.dtCreation; //reservationTbl.dtStart.Value.AddDays(-30);
            if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
            order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

            //Balance Payment
            paymentScheduleItem = new ChnlMagaRentalXmlClasses.PaymentScheduleItem();
            paymentScheduleItem.amount.ValueDecimal = reservationTbl.pr_part_owner.objToDecimal();
            paymentScheduleItem.dueDate.ValueDate = reservationTbl.dtStart;
            if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
            order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);        
            #endregion

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
            Response.rentalAgreement.url = App.HOST + CurrentSource.getPagePath("3", "stp", App.LangID + "");
            return Response.GetXml();
        }
    }

    public static string Search(string xml, dbRntAgentTBL agentTbl, int pidLang)
    {
        if (xml == "") return "";

        long agentID = agentTbl.id;
        ChnlMagaRentalXmlClasses.ErrorList errorList = new ChnlMagaRentalXmlClasses.ErrorList("quoteResponse");

        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
            try
            {
                ChnlMagaRentalXmlClasses.SearchRequest Request = new ChnlMagaRentalXmlClasses.SearchRequest(xml);
                if (!Request.reservationDates.beginDate.ValueDate.HasValue || !Request.reservationDates.endDate.ValueDate.HasValue)
                {
                    return "";
                }

                List<int> estatesNotAvv = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 //
                                                                                                && x.dtStart.HasValue //
                                                                                                && x.dtEnd.HasValue //
                                                                                                && ((x.dtStart.Value.Date <= Request.reservationDates.beginDate.ValueDate.Value && x.dtEnd.Value.Date >= Request.reservationDates.endDate.ValueDate.Value) //
                                                                                                    || (x.dtStart.Value.Date >= Request.reservationDates.beginDate.ValueDate.Value && x.dtStart.Value.Date < Request.reservationDates.endDate.ValueDate.Value) //
                                                                                                    || (x.dtEnd.Value.Date > Request.reservationDates.beginDate.ValueDate.Value && x.dtEnd.Value.Date <= Request.reservationDates.endDate.ValueDate.Value))).Select(x => x.pid_estate.objToInt32()).ToList();

                //List<int> estatesNotAvv = rntUtils.rntEstate_getNotAvvIds(Request.reservationDates.beginDate.ValueDate.Value, Request.reservationDates.endDate.ValueDate.Value);
                List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.Where(x => !estatesNotAvv.Contains(x.id)
                                                                                            && x.is_online_booking == 1
                                                                                            && x.is_exclusive == 1
                                                                                            && x.num_persons_max >= Request.numberOfAdults
                                                                                            && x.num_persons_min <= Request.numberOfAdults
                                                                                            && x.num_persons_child >= Request.numberOfChildren
                                                                                            ).ToList();

                DateTime checkDate = DateTime.Now;
                DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTbl.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                decimal agentTotalResPrice = 0;
                agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                List<int> removeIds = new List<int>();
                foreach (AppSettings.RNT_estate _rntEst in estateList)
                {
                    RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == pidLang);
                    if (_lang == null) { removeIds.Add(_rntEst.id); continue; }
                    _rntEst.pid_lang = pidLang;
                    _rntEst.title = (_lang == null) ? _rntEst.code : _lang.title;
                    var city = CurrentSource.loc_cityTitle(_rntEst.pid_city.objToInt32(), pidLang, "");
                    var zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), pidLang, "");

                    _rntEst.zone = city + (city != "" && zone != "" ? " / " : "") + zone;
                    rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                    outPrice.dtStart = Request.reservationDates.beginDate.ValueDate.Value;
                    outPrice.dtEnd = Request.reservationDates.endDate.ValueDate.Value;
                    outPrice.numPersCount = Request.numberOfAdults;
                    outPrice.pr_discount_owner = 0;
                    outPrice.pr_discount_commission = 0;
                    outPrice.fillAgentDetails(agentTbl);
                    outPrice.agentTotalResPrice = agentTotalResPrice;
                    outPrice.part_percentage = _rntEst.pr_percentage;
                    _rntEst.price = rntUtils.rntEstate_getPrice(0, _rntEst.id, ref outPrice);
                    if (_rntEst.price <= 0) { removeIds.Add(_rntEst.id); continue; }
                    _rntEst.pr_agentID = outPrice.agentID;
                    _rntEst.pr_agentCommissionPrice = outPrice.agentCommissionPrice;
                    //_rntEst.pr_agentCommissionNotInTotal = outPrice.agentCommissionNotInTotal;
                    _rntEst.priceError = outPrice.outError;
                    if (_rntEst.pr_agentCommissionPrice <= 0) { removeIds.Add(_rntEst.id); continue; }
                }
                estateList.RemoveAll(x => removeIds.Contains(x.id));

                ChnlMagaRentalXmlClasses.SearchResponse Response = new ChnlMagaRentalXmlClasses.SearchResponse();
                foreach (AppSettings.RNT_estate _rntEst in estateList)
                {
                    ChnlMagaRentalXmlClasses.SearchResponse.AvailabilityEntry tmpTb = new ChnlMagaRentalXmlClasses.SearchResponse.AvailabilityEntry();
                    tmpTb.listingExternalId = _rntEst.id + "";
                    tmpTb.listingName = _rntEst.title;
                    tmpTb.listingUrl = App.HOST_SSL + "/chnlutils/magarental/listing/" + tmpTb.listingExternalId + "?auth=" + agentTbl.uid + "&ln=" + pidLang;
                    tmpTb.totalAmount.ValueDecimal = _rntEst.price;
                    tmpTb.commissionAmount.ValueDecimal = _rntEst.pr_agentCommissionPrice;
                    Response.availabilityList.Add(tmpTb);
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
    public static string QuoteRequest(string xml, dbRntAgentTBL agentTbl, int pidLang)
    {
        if (xml == "") return "";

        long agentID = agentTbl.id;
        ChnlMagaRentalXmlClasses.ErrorList errorList = new ChnlMagaRentalXmlClasses.ErrorList("quoteResponse");

        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
            try
            {
                ChnlMagaRentalXmlClasses.QuoteRequest Request = new ChnlMagaRentalXmlClasses.QuoteRequest(xml);

                //for sending resonse
                RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currEstateTB == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }
                DateTime dtStart = Request.reservation.reservationDates.beginDate.ValueDate.Value;
                DateTime dtEnd = Request.reservation.reservationDates.endDate.ValueDate.Value;
                if (currEstateTB.nights_max.objToInt32() > 0 && currEstateTB.nights_max < (dtEnd - dtStart).TotalDays.objToInt32())
                {
                    errorList.AddError("EXCEEDS_MAX_STAY");
                }
                RNT_TBL_RESERVATION tmpRes = null;
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
                decimal price = rntUtils.rntEstate_getPrice(0, Request.unitExternalId.objToInt32(), ref outPrice);
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

                ChnlMagaRentalXmlClasses.QuoteResponse Response = new ChnlMagaRentalXmlClasses.QuoteResponse();
                Response.locale = Request.inquirer.locale;
                if (string.IsNullOrEmpty(Response.locale)) Response.locale = "en_GB";
                var order = new ChnlMagaRentalXmlClasses.Order();
                Response.orderList.Add(order);

                string currency = CommonUtilities.getSYS_SETTING("currency");
                if (currency == "") currency = "EUR";

                order.currency = currency;
                var orderItem = new ChnlMagaRentalXmlClasses.OrderItem();
                order.orderItemList.Add(orderItem);
                orderItem.feeType = "RENTAL";
                orderItem.description = "Rent";
                orderItem.name = "Rent";
                orderItem.commissionAmount.ValueDecimal = outPrice.agentCommissionPrice;
                //orderItem.totalAmount.ValueDecimal = price - outPrice.prOptioniExtra - outPrice.cityTax;
                orderItem.totalAmount.ValueDecimal = price;

                //foreach (var extra in outPrice.lstExtra.Where(x => x.isRequired == 1))
                //{
                //    orderItem = new ChnlMagaRentalXmlClasses.OrderItem();
                //    order.orderItemList.Add(orderItem);
                //    orderItem.feeType = "MISC";
                //    orderItem.name = orderItem.description = rntUtils.getEstateExtras_title(extra.pidExtra, App.DefLangID, "");
                //    orderItem.totalAmount.ValueDecimal = extra.price.objToDecimal();
                //}

                //if (outPrice.cityTax.objToDecimal() > 0)
                //{
                //    orderItem = new ChnlMagaRentalXmlClasses.OrderItem();
                //    order.orderItemList.Add(orderItem);
                //    orderItem.feeType = "TAX";
                //    orderItem.name = orderItem.description = "CityTax";
                //    orderItem.totalAmount.ValueDecimal = outPrice.cityTax;
                //}
                // TODO, finalise
                //var paymentScheduleItem = new ChnlMagaRentalXmlClasses.PaymentScheduleItem();
                //paymentScheduleItem.amount.ValueDecimal = price;
                //paymentScheduleItem.dueDate.ValueDate = dtStart.AddDays(-30);
                //if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
                //order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

                #region Scheduled Payment
                //Part Payment
                var paymentScheduleItem = new ChnlMagaRentalXmlClasses.PaymentScheduleItem();
                paymentScheduleItem.amount.ValueDecimal = outPrice.pr_part_payment_total.objToDecimal();
                paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
                order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);

                //Balance Payment
                if (outPrice.pr_part_owner > 0)
                {
                    paymentScheduleItem = new ChnlMagaRentalXmlClasses.PaymentScheduleItem();
                    paymentScheduleItem.amount.ValueDecimal = outPrice.pr_part_owner.objToDecimal();
                    paymentScheduleItem.dueDate.ValueDate = outPrice.dtStart;
                    if (paymentScheduleItem.dueDate.ValueDate < DateTime.Now.Date) paymentScheduleItem.dueDate.ValueDate = DateTime.Now.Date;
                    order.paymentSchedule.paymentScheduleItemList.Add(paymentScheduleItem);
                }
                #endregion

                Response.rentalAgreement.url = App.HOST + CurrentSource.getPagePath("3", "stp", App.LangID + "");
                return Response.GetXml();
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "HA_Quote", ex.ToString());
            }
        return errorList.GetXml();
    }

    public static string BookingRequest(string xml, dbRntAgentTBL agentTbl, int pidLang)
    {
        if (xml == "") return "";

        long agentID = agentTbl.id;
        ChnlMagaRentalXmlClasses.ErrorList errorList = new ChnlMagaRentalXmlClasses.ErrorList("quoteResponse");

        using (DCmodAuth dcAuth = new DCmodAuth())
        using (DCmodRental dc = new DCmodRental())
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
            try
            {

                ChnlMagaRentalXmlClasses.BookingRequest Request = new ChnlMagaRentalXmlClasses.BookingRequest(xml);

                var ccData = "";
                ccData += "Holder: " + Request.paymentCard.nameOnCard;
                ccData += "\r\n Masked Number: " + Request.paymentCard.maskedNumber;
                ccData += "\r\n Number: " + Request.paymentCard.number;
                ccData += "\r\n NumberToken: " + Request.paymentCard.numberToken;
                ccData += "\r\n paymentFormType: " + Request.paymentCard.paymentFormType;
                ccData += "\r\n CVC: " + Request.paymentCard.cvv;
                ccData += "\r\n Expire: " + Request.paymentCard.expiration;
                RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.unitExternalId.ToInt32());
                if (currEstateTB == null)
                {
                    errorList.AddError("UNKNOWN_PROPERTY");
                    return errorList.GetXml();
                }
                DateTime dtStart = Request.reservation.reservationDates.beginDate.ValueDate.Value;
                DateTime dtEnd = Request.reservation.reservationDates.endDate.ValueDate.Value;

                bool _isAvailable = dcOld.RNT_TBL_RESERVATION.Where(y => y.pid_estate == currEstateTB.id //
                                                                                    && y.state_pid != 3 //
                                                                                    && y.dtStart.HasValue //
                                                                                    && y.dtEnd.HasValue //
                                                                                    && ((y.dtStart <= dtStart && y.dtEnd >= dtEnd) //
                                                                                        || (y.dtStart >= dtStart && y.dtStart < dtEnd) //
                                                                                        || (y.dtEnd > dtStart && y.dtEnd <= dtEnd))).Count() == 0;


                //bool _isAvailable = rntUtils.rntEstate_isAvailable(currEstateTB.id, dtStart, dtEnd, 0);
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
                decimal prTotal = rntUtils.rntEstate_getPrice(0, Request.unitExternalId.objToInt32(), ref outPrice);
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
                    newRes.IdAdMedia = ChnlMagaRentalXmlProps.IdAdMedia;
                    newRes.IdLink = agentID + "";

                    newRes.pid_estate = Request.unitExternalId.objToInt32();
                    newRes.num_adult = Request.reservation.numberOfAdults.objToInt32();
                    newRes.num_child_over = Request.reservation.numberOfChildren.objToInt32();
                    newRes.dtStart = dtStart;
                    newRes.dtEnd = dtEnd;

                    rntUtils.rntReservation_setDefaults(ref newRes);
                    dcOld.RNT_TBL_RESERVATION.InsertOnSubmit(newRes);
                    dcOld.SubmitChanges();
                    newRes.code = newRes.id.ToString().fillString("0", 7, false);

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

                    //newRes.agentCommissionNotInTotal = 0;
                    newRes.requestFullPayAccepted = 1;

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
                            _client.nameFirst = Request.inquirer.name;
                            _client.nameLast = "";
                            _client.nameFull = Request.inquirer.name;

                            _client.locAddress = Request.inquirer.address.addressLine1 + "; " + Request.inquirer.address.additionalAddressLine1 + "; " + Request.inquirer.address.addressLine2;
                            _client.locCity = Request.inquirer.address.addressLine3;
                            _client.locState = Request.inquirer.address.addressLine4;
                            _client.locCountry = Request.inquirer.address.addressLine5;
                            _client.locZipCode = Request.inquirer.address.postalCode;
                            _client.contactPhoneMobile = Request.inquirer.phoneCountryCode + "_" + Request.inquirer.phoneNumber;
                            _client.isActive = 1;
                            dcAuth.SaveChanges();
                        }
                        newRes.agentClientID = _client.id;
                    }

                    dcOld.SubmitChanges();
                    rntUtils.rntReservation_onChange(newRes);
                    rntUtils.reservation_checkPartPayment(newRes, true);
                    return Booking(newRes.id, agentTbl, pidLang);
                }
            }
            catch (Exception ex)
            {
                errorList.AddError("OTHER");
                ErrorLog.addLog("", "HA_Booking", ex.ToString());
            }
        return errorList.GetXml();
    }

    //public static Dictionary<string, decimal> getAmenityPrice(int idEstate, int idExtra, int pidLang)
    //{
    //    Dictionary<string, decimal> price = new Dictionary<string, decimal>();
    //    dbRntEstateExtrasPriceTBL currEstatePrice = rntProps.EstateExtrasPriceTBL.SingleOrDefault(x => x.pidExtras == idExtra && x.pidEstate == idEstate);
    //    List<dbRntExtrasPriceTBL> lstPrice = rntProps.ExtrasPriceTBL.Where(x => x.pidExtras == idExtra).ToList();

    //    string type = "";
    //    decimal priceTotal = 0;

    //    if (currEstatePrice != null)
    //    {
    //        if (currEstatePrice.paymentType == "persona")
    //        {
    //            type = contUtils.getLabel("lblpersona", pidLang,"");
    //            priceTotal = currEstatePrice.actualPrice.objToDecimal();
    //        }
    //        else if (currEstatePrice.paymentType == "forfait")
    //        {
    //            type = contUtils.getLabel("lblforfait", pidLang,"");
    //            priceTotal = currEstatePrice.actualPrice.objToDecimal();
    //        }
    //        else if (currEstatePrice.paymentType == "notte")
    //        {
    //            type = contUtils.getLabel("lblnotte", pidLang,"");
    //            priceTotal = currEstatePrice.actualPrice.objToDecimal();
    //        }
    //    }
    //    else if (lstPrice != null && lstPrice.Count > 0)
    //    {
    //        if (lstPrice[0].paymentType == "persona")
    //        {
    //            type = contUtils.getLabel("lblpersona", pidLang,"");
    //            priceTotal = lstPrice[0].actualPrice.objToDecimal();
    //        }
    //        else if (lstPrice[0].paymentType == "forfait")
    //        {
    //            type = contUtils.getLabel("lblforfait", pidLang,"");
    //            priceTotal = lstPrice[0].actualPrice.objToDecimal();
    //        }
    //        else if (lstPrice[0].paymentType == "notte")
    //        {
    //            type = contUtils.getLabel("lblnotte", pidLang,"");
    //            priceTotal = lstPrice[0].actualPrice.objToDecimal();
    //        }
    //    }
    //    price.Add(type, priceTotal);
    //    return price;
    //}
}
public class ChnlMagaRentalXmlClasses
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
        public class Listing
        {
            /// <summary>xxxx</summary>
            public string externalId { get; set; }
            public bool active { get; set; }
            public AdContent adContent { get; set; }
            public List<Amenities> amenities { get; set; }
            public Location location { get; set; }
            public bool hasCityTax { get; set; }
            public CityTax cityTax { get; set; }
            public List<Image> images { get; set; }
            public List<Unit> units { get; set; }
            public Listing()
            {
                adContent = new AdContent();
                amenities = new List<Amenities>();
                location = new Location();
                cityTax = new CityTax();
                images = new List<Image>();
                units = new List<Unit>();
            }
            public string GetXml()
            {
                XElement elm = new XElement("listing");
                elm.Add(new XElement("externalId", externalId));
                elm.Add(new XElement("active", active));
                if (adContent.HasValue())
                    elm.Add(adContent.GetElement());
                if (amenities.Count > 0)
                {
                    var amenitiesElm = new XElement("amenities");
                    foreach (var tmp in amenities)
                        amenitiesElm.Add(tmp.GetElement());
                    elm.Add(amenitiesElm);
                }
                elm.Add(location.GetElement());
                if (images.Count > 0)
                {
                    var imagesElm = new XElement("images");
                    foreach (var tmp in images)
                        imagesElm.Add(tmp.GetElement());
                    elm.Add(imagesElm);
                }

                elm.Add(new XElement("hasCityTax", hasCityTax));

                if (hasCityTax)
                {
                    elm.Add(cityTax.GetElement());
                }

                if (units.Count > 0)
                {
                    var unitsElm = new XElement("units");
                    foreach (var tmp in units)
                        unitsElm.Add(tmp.GetElement());
                    elm.Add(unitsElm);
                }
                return elm + "";
            }
        }
        public class Amenities
        {
            public string title { get; set; }
            public Price price { get; set; }
            public bool mandatory { get; set; }
            public bool instantPayment { get; set; }
            public Amenities()
            {
                title = "";
                price = new Price();
                mandatory = false;
                instantPayment = false;
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("amenity");
                elm.Add(new XElement("title", title));
                elm.Add(price.GetElement());
                elm.Add(new XElement("mandatory", mandatory));
                elm.Add(new XElement("instantPayment", instantPayment));
                return elm;
            }

            public class Price
            {
                public string _type { get; set; }
                public DecimalUnit _rate { get; set; }
                public Price()
                {
                    _type = "";
                    _rate = new DecimalUnit();
                }
                public bool HasValue()
                {
                    return _rate.Value != "";
                }
                public XElement GetElement()
                {
                    XElement Priceelm = new XElement("price");
                    Priceelm.Add(new XAttribute("type", _type));
                    if (_rate.Value != "")
                        Priceelm.Add(new XAttribute("rate", _rate.ValueDecimal));
                    return Priceelm;
                }
            }
        }
        public class Unit
        {
            public string externalId { get; set; }
            public bool active { get; set; }
            public int? area { get; set; }
            public string areaUnit { get; set; } // METERS_SQUARED or SQUARE_FEET.
            public int bathrooms { get; set; }
            public int bedrooms { get; set; }
            public List<string> amenities { get; set; }
            public int maxSleep { get; set; }
            public string propertyType { get; set; }
            public string unitName { get; set; }
            public List<RatePeriod> ratePeriods { get; set; }
            public Availability unitAvailability { get; set; }
            public Unit()
            {
                areaUnit = "METERS_SQUARED";
                amenities = new List<string>();
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
                elm.Add(new XElement("bathrooms", bathrooms));
                elm.Add(new XElement("bedrooms", bedrooms));

                if (amenities.Count > 0)
                {
                    var amenitiesElm = new XElement("amenities");
                    foreach (var tmp in amenities)
                        amenitiesElm.Add(new XElement("amenity", tmp));
                    elm.Add(amenitiesElm);
                }
                elm.Add(new XElement("maxSleep", maxSleep));
                elm.Add(new XElement("propertyType", propertyType));
                elm.Add(new XElement("unitName", unitName));
                if (ratePeriods.Count > 0)
                {

                    var ratePeriodsElm = new XElement("ratePeriods");

                    string currency = CommonUtilities.getSYS_SETTING("currency");
                    if (currency == "") currency = "EUR";
                    ratePeriodsElm.Add(new XElement("currency", currency));

                    foreach (var tmp in ratePeriods)
                        ratePeriodsElm.Add(tmp.GetElement());

                    elm.Add(ratePeriodsElm);
                }
                elm.Add(unitAvailability.GetElement());
                return elm;
            }
        }
        public class RatePeriod
        {
            public DateRange dateRange { get; set; }
            public int minimumStay { get; set; }
            public List<PerOccupancy> perOccupancy { get; set; }
            public AllowedDays allowedDays { get; set; }
            public RatePeriod()
            {
                dateRange = new DateRange();
                perOccupancy = new List<PerOccupancy>();
                allowedDays = new AllowedDays();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("ratePeriod");
                elm.Add(dateRange.GetElement("dateRange"));
                elm.Add(new XElement("minimumStay", minimumStay));

                elm.Add(allowedDays.GetElement());

                foreach (var tmp in perOccupancy)
                    if (tmp.HasValue())
                        elm.Add(tmp.GetElement());
                return elm;
            }
            public class PerOccupancy
            {
                public int _occupancy { get; set; }
                public DecimalUnit _rate { get; set; }
                public PerOccupancy()
                {
                    _rate = new DecimalUnit();
                }
                public bool HasValue()
                {
                    return _rate.Value != "";
                }
                public XElement GetElement()
                {
                    XElement elm = new XElement("PerOccupancy");
                    elm.Add(new XAttribute("occupancy", _occupancy));
                    if (_rate.Value != "")
                        elm.Add(new XAttribute("rate", _rate.Value));
                    return elm;
                }
            }
        }
        public class Availability
        {
            public DateRange dateRange { get; set; }
            public List<string> availability { get; set; } // A letter code for every day of the range. Y=Available, N=Not Available,Q=Inquiry Only. A maximum of 3 years of availability can be given. There cannot be more days given than there are days in the dateRange. An example:YYYNNNQQYYYNNNQQYYYNNNQQYYYNNNQQYYY...
            public List<string> minStay { get; set; } // Minimum stay is a list of comma-separated values 0 thru 999, with each value representing a day in the range and where 0 = "no min stay”. An example: 2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2,3,7,7,7,7,2,2 …
            public Availability()
            {
                dateRange = new DateRange();
                availability = new List<string>();
                minStay = new List<string>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("unitAvailability");
                elm.Add(dateRange.GetElement("dateRange"));
                if (availability.Count > 0)
                    elm.Add(new XElement("availability", availability.listToString(",")));
                if (minStay.Count > 0)
                    elm.Add(new XElement("minStay", minStay.listToString(",")));
                return elm;
            }
        }
        public class AvailabilityConfiguration
        {
            public AvailabilityConfiguration()
            {
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("unitAvailabilityConfiguration");
                return elm;
            }
        }
        public class AdContent
        {
            public string title { get; set; }
            public string summary { get; set; }
            public string description { get; set; }
            public AdContent()
            {
            }
            public bool HasValue()
            {
                return !string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(summary) || !string.IsNullOrEmpty(description);
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("adContent");
                elm.Add(new XElement("title", title));
                elm.Add(new XElement("summary", summary));
                elm.Add(new XElement("description", description));
                return elm;
            }
        }
        public class Location
        {
            public Address address { get; set; }
            public LatLng geoCode { get; set; }
            public List<NearestPlace> nearestPlaces { get; set; }
            public Location()
            {
                address = new Address();
                geoCode = new LatLng();
                nearestPlaces = new List<NearestPlace>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("location");
                elm.Add(address.GetElement());
                if (geoCode.HasValue())
                    elm.Add(geoCode.GetElement("geoCode"));
                foreach (var tmp in nearestPlaces)
                    elm.Add(tmp.GetElement());
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

        //        <citytax>
        // <adultRate>
        // </adultRate>
        // <childRate>
        // </childRate>
        // <maxdays> 
        // </maxdays>
        // <payInAdvance> 1/0
        // </payInAdvance>
        // <limitedPeriods>
        //  <period from="dd/mm/yyyy" to="dd/mm/yyyy">
        //  </period>
        //  ...
        // </limitedPeriods>
        //</citytax>
        public class CityTax
        {
            public decimal adultRate { get; set; }
            public decimal childRate { get; set; }
            public int maxDays { get; set; }
            public int payInAdvance { get; set; }
            public List<limitedPeriod> limitedPeriods { get; set; }

            public CityTax()
            {
                limitedPeriods = new List<limitedPeriod>();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("cityTax");
                elm.Add(new XElement("adultRate", adultRate));
                elm.Add(new XElement("childRate", childRate));
                elm.Add(new XElement("maxDays", maxDays));
                elm.Add(new XElement("payInAdvance", payInAdvance));
                if (limitedPeriods.Count > 0)
                {
                    XElement periodElm = new XElement("limitedPeriods");

                    foreach (limitedPeriod period in limitedPeriods)
                    {
                        periodElm.Add(period.GetElement());
                    }

                    elm.Add(periodElm);
                }
                return elm;
            }
        }

        public class limitedPeriod
        {
            public DateTime fromDate { get; set; }
            public DateTime todate { get; set; }
            public XElement GetElement()
            {
                XElement elm = new XElement("period");
                elm.Add(new XAttribute("from", fromDate.ToString("dd/MM/yyyy")));
                elm.Add(new XAttribute("to", todate.ToString("dd/MM/yyyy")));
                return elm;
            }
        }
        public class NearestPlace
        {
            public string name { get; set; }
            public DecimalUnit distance { get; set; }
            public string distanceUnit { get; set; }
            public NearestPlace()
            {
                distance = new DecimalUnit();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("featureValue");
                elm.Add(new XElement("name", name));
                elm.Add(new XElement("distance", distance.Value));
                elm.Add(new XElement("distanceUnit", distanceUnit));
                return elm;
            }
        }
        public class Image
        {
            public string uri { get; set; }
            public Image()
            {
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("image");
                elm.Add(new XElement("uri", uri));
                return elm;
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
    public class SearchRequest
    {
        public int numberOfAdults { get; set; }
        public int numberOfChildren { get; set; }
        public DateRange reservationDates { get; set; }
        public SearchRequest()
        {
            reservationDates = new DateRange();
        }
        public SearchRequest(string xmlData)
        {
            XDocument _resource = XDocument.Parse(xmlData);
            var reservationElm = _resource.Element("searchRequest");

            reservationDates = new DateRange();
            if (reservationElm.Element("numberOfAdults") != null)
                numberOfAdults = reservationElm.Element("numberOfAdults").Value.ToInt32();
            if (reservationElm.Element("numberOfChildren") != null)
                numberOfChildren = reservationElm.Element("numberOfChildren").Value.ToInt32();
            if (reservationElm.Element("reservationDates") != null)
            {
                if (reservationElm.Element("reservationDates").Element("beginDate") != null)
                    reservationDates.beginDate.Value = reservationElm.Element("reservationDates").Element("beginDate").Value;
                if (reservationElm.Element("reservationDates").Element("endDate") != null)
                    reservationDates.endDate.Value = reservationElm.Element("reservationDates").Element("endDate").Value;
            }
        }
    }
    public class SearchResponse
    {
        public List<AvailabilityEntry> availabilityList { get; set; }
        public SearchResponse()
        {
            availabilityList = new List<AvailabilityEntry>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement quoteResponse = new XElement("searchResponse");
            XElement availabilityListElm = new XElement("availabilityList");
            quoteResponse.Add(availabilityListElm);
            foreach (AvailabilityEntry tmp in availabilityList)
                availabilityListElm.Add(tmp.GetElement());
            _resource.Add(quoteResponse);
            return _resource.ToString();
        }
        public class AvailabilityEntry
        {
            public string listingExternalId { get; set; }
            public string listingName { get; set; }
            public string listingUrl { get; set; }
            public Amount commissionAmount { get; set; } // The pre tax amount associated with this line item including the currency.
            public Amount totalAmount { get; set; } // The total amount associated with this line item including the currency
            public AvailabilityEntry()
            {
                commissionAmount = new Amount();
                totalAmount = new Amount();
            }
            public XElement GetElement()
            {
                XElement elm = new XElement("availabilityEntry");
                if (!string.IsNullOrEmpty(listingExternalId))
                    elm.Add(new XElement("listingExternalId", listingExternalId));
                if (!string.IsNullOrEmpty(listingName))
                    elm.Add(new XElement("listingName", listingName));
                if (!string.IsNullOrEmpty(listingUrl))
                    elm.Add(new XElement("listingUrl", listingUrl));
                if (commissionAmount.Value != "")
                    elm.Add(commissionAmount.GetElement("commissionAmount"));
                if (totalAmount.Value != "")
                    elm.Add(totalAmount.GetElement("totalAmount"));
                return elm;
            }
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
            reservation = new Reservation(ds.Element("reservation"));
            var orderItemListElm = ds.Element("orderItemList");
            if (orderItemListElm != null && orderItemListElm.HasElements && orderItemListElm.Descendants("orderItem").Count() > 0)
                foreach (var orderItemElm in orderItemListElm.Descendants("orderItem"))
                {
                    orderItemList.Add(new OrderItem(orderItemElm));
                }
            paymentCard = new PaymentCard(ds.Element("paymentCard"));
        }
        public class PaymentCard
        {
            public string paymentFormType { get; set; } // CARD, CHECK, ECHECK, DEFERRED
            public string cvv { get; set; }
            public string expiration { get; set; } // MM/YYYY
            public string maskedNumber { get; set; } // Credit card number with the last 4 digits masked.
            public string nameOnCard { get; set; } // The person’s name on the credit card.
            public string number { get; set; } // The full credit card number.
            public string numberToken { get; set; } // The HomeAway token representing the credit card number.
            public PaymentCard()
            {
            }
            public PaymentCard(XElement elm)
            {
                if (elm != null)
                {
                    paymentFormType = (string)elm.Element("paymentFormType") ?? "";
                    cvv = (string)elm.Element("cvv") ?? "";
                    expiration = (string)elm.Element("expiration") ?? "";
                    maskedNumber = (string)elm.Element("maskedNumber") ?? "";
                    nameOnCard = (string)elm.Element("nameOnCard") ?? "";
                    number = (string)elm.Element("number") ?? "";
                    numberToken = (string)elm.Element("numberToken") ?? "";
                }
            }
        }
    }
    public class BookingResponse
    {
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
        public BookingContentIndexRequest()
        {
            lastUpdatedDate = new DateAndTime();
        }
        public BookingContentIndexRequest(string xmlData)
        {
            lastUpdatedDate = new DateAndTime();
            if (xmlData == "") return;
            XDocument _resource = XDocument.Parse(xmlData);
            var ds = _resource.Element("bookingContentIndexRequest");
            lastUpdatedDate.Value = (string)ds.Element("lastUpdatedDate") ?? "";
        }
    }
    public class BookingContentIndexResponse
    {
        public List<BookingContentIndexEntry> bookingContentIndexEntryList { get; set; }
        public BookingContentIndexResponse()
        {
            bookingContentIndexEntryList = new List<BookingContentIndexEntry>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("bookingContentIndex");
            foreach (var tmp in bookingContentIndexEntryList)
                root.Add(tmp.GetElement());
            _resource.Add(root);
            return _resource.ToString();
        }
    }
    public class ListingContentIndexResponse
    {
        public List<ListingContentIndexEntry> listingContentIndexEntry { get; set; }
        public ListingContentIndexResponse()
        {
            listingContentIndexEntry = new List<ListingContentIndexEntry>();
        }
        public string GetXml()
        {
            XDocument _resource = new XDocument();
            XElement root = new XElement("listingContentIndex");
            foreach (var tmp in listingContentIndexEntry)
                root.Add(tmp.GetElement());
            _resource.Add(root);
            return _resource.ToString();
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
                elm.Add(new XElement("rentalAgreement", agreementText));
            if (!string.IsNullOrEmpty(externalId))
                elm.Add(new XElement("externalId", externalId));
            if (!string.IsNullOrEmpty(url))
                elm.Add(new XElement("url", url));
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
    public class AllowedDays
    {
        public string checkinAllows { get; set; }
        public string checkoutAllows { get; set; }
        public AllowedDays()
        {
            checkinAllows = "y,y,y,y,y,y,y";
            checkoutAllows = "y,y,y,y,y,y,y";
        }
        public XElement GetElement()
        {
            XElement allowedDaysElm = new XElement("allowedDays");
            if (!string.IsNullOrEmpty(checkinAllows))
                allowedDaysElm.Add(new XElement("checkinAllows", checkinAllows));
            if (!string.IsNullOrEmpty(checkoutAllows))
                allowedDaysElm.Add(new XElement("checkoutAllows", checkoutAllows));

            return !allowedDaysElm.HasElements ? (XElement)null : allowedDaysElm;
        }
    }

    public class Order
    {
        public string bodyText { get; set; }
        public string currency { get; set; } // EUR, GBP, USD
        public string externalId { get; set; }
        public List<OrderItem> orderItemList { get; set; }
        public PaymentSchedule paymentSchedule { get; set; }
        public Order()
        {
            orderItemList = new List<OrderItem>();
            paymentSchedule = new PaymentSchedule();
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

            return orderElm;
        }
    }
    public class OrderItem
    {
        public string description { get; set; }
        public string externalId { get; set; }
        public string feeType { get; set; } // ACTIVITY, DEPOSIT, EQUIPMENT, MISC, PROTECTION, RENTAL, TAX, DISCOUNT
        public string name { get; set; } // max 100 chars
        public Amount commissionAmount { get; set; } // The pre tax amount associated with this line item including the currency.
        public string status { get; set; } // PENDING, ACCEPTED, DECLINED, DECLINED_BY_SYSTEM, CANCELLED, EDITED
        public string taxRate { get; set; } // The tax rate applied to this line item
        public Amount totalAmount { get; set; } // The total amount associated with this line item including the currency
        public OrderItem()
        {
            commissionAmount = new Amount();
            totalAmount = new Amount();
        }
        public OrderItem(XElement elm)
        {
            commissionAmount = new Amount();
            totalAmount = new Amount();
            if (elm != null)
            {
                description = (string)elm.Element("description") ?? "";
                externalId = (string)elm.Element("externalId") ?? "";
                feeType = (string)elm.Element("feeType") ?? "";
                name = (string)elm.Element("name") ?? "";
                commissionAmount = new Amount(elm.Element("commissionAmount"));
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
            if (commissionAmount.Value != "")
                orderItemElm.Add(commissionAmount.GetElement("commissionAmount"));
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
        public string description { get; set; }
        public string name { get; set; }
        public List<PaymentScheduleItem> paymentScheduleItemList { get; set; }
        public PaymentSchedule()
        {
            paymentScheduleItemList = new List<PaymentScheduleItem>();
        }
        public XElement GetElement()
        {
            XElement paymentScheduleElm = new XElement("paymentSchedule");
            if (!string.IsNullOrEmpty(description))
                paymentScheduleElm.Add(new XElement("description", description));
            if (!string.IsNullOrEmpty(name))
                paymentScheduleElm.Add(new XElement("name", name));
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
            string currency = CommonUtilities.getSYS_SETTING("currency");
            if (currency == "") currency = "EUR";
            Currency = currency;
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
public static class ChnlMagaRentalXmlExts
{
}

public class AminityAttr
{
    public string PriceType { get; set; }
    public decimal PriceAmt { get; set; }
    public string mandatory { get; set; }
    public string instantPayment { get; set; }
    public AminityAttr()
    {
        PriceAmt = 0;
        PriceType = "";
        mandatory = "";
        instantPayment = "";
    }
}
