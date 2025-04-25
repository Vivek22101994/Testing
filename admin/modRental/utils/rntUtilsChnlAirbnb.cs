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
using Newtonsoft.Json.Converters;
using System.Xml.Linq;
using ModAuth;
using HtmlAgilityPack;
using RentalInRome.data;

public class ChnlAirbnbUtils
{
    public static bool CheckForService()
    {
        //if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Response == null) return false;
        var Request = HttpContext.Current.Request;
        var Response = HttpContext.Current.Response;
        if (!Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/airbnb/")) return false;
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetAllowResponseInBrowserHistory(true);
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/json";

        string responseConent = "";
        long agentID = 0;

        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/airbnb/reservations"))
        {
            ErrorLog.addLog("", "action 1", Request.Url.LocalPath);
            ErrorLog.addLog("", "action", Request.QueryString["action"] + "");
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia);
                if (agentTbl != null)
                    agentID = agentTbl.id;
                if (agentID == 0)
                {
                    ErrorLog.addLog("", "chnlAirbnbReceiveReservationCode", "Agent for Airbnb was not found or not active");
                    responseConent = "";
                }

                if (Request.QueryString["action"] + "" == "check_availability")
                {
                    ErrorLog.addLog("", "check avv", Request.QueryString["listing_id"].objToInt32() + "");
                    RntChnlAirbnbEstateTBL currAirbnbEstateTBL = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.airbnb_id == Request.QueryString["listing_id"].objToInt32());
                    if (currAirbnbEstateTBL == null)
                    {
                        ErrorLog.addLog("", "airbnb reservaion check_availability notification", "Missing AirBnb Estate");
                        responseConent = "{\"succeed\":\"" + false + "\"}";
                    }
                    else
                    {
                        RNT_TB_ESTATE currEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currAirbnbEstateTBL.mr_id.objToInt32());
                        if (currEstateTB == null)
                        {
                            ErrorLog.addLog("", "airbnb reservaion check_availability notification", "Missing AirBnb Estate");
                            responseConent = "{\"succeed\":\"" + false + "\"}";
                        }

                        string date = Request.QueryString["start_date"];
                        int nights = Request.QueryString["nights"].objToInt32();
                        if (date != "" && date.Split('-').Length >= 3)
                        {
                            DateTime dtStart = new DateTime(date.Split('-')[0].objToInt32(), date.Split('-')[1].objToInt32(), date.Split('-')[2].objToInt32());
                            DateTime dtEnd = dtStart.AddDays(nights);

                            bool _isAvailable = ChnlAirbnbUtils.CheckAvailability(currEstateTB, dtStart, dtEnd, agentTbl.id, 0);
                            if (_isAvailable)
                            {
                                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                                outPrice.dtStart = dtStart;
                                outPrice.dtEnd = dtEnd;
                                outPrice.numPersCount = currEstateTB.num_persons_max.objToInt32();
                                outPrice.numPers_adult = currEstateTB.num_persons_adult.objToInt32();
                                outPrice.numPers_childOver = currEstateTB.num_persons_child.objToInt32();

                                outPrice.fillAgentDetails(agentTbl);
                                DateTime checkDate = DateTime.Now;
                                DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                                DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                                var tmpList = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTbl.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                                outPrice.agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                                outPrice.part_percentage = 0;
                                    
                                #region Made change for using values of Acconto and balance configured in agent detail
                                //if (agentTbl.PartPayment.objToDecimal() == 100 || agentTbl.IsAllPayment == 1)
                                //{
                                //    outPrice.part_percentage = 100;
                                //}
                                //else
                                //{
                                //    outPrice.part_percentage = agentTbl.PartPayment.objToDecimal();
                                //}
                                #endregion

                                decimal price = rntUtils.rntEstate_getPrice(0, currEstateTB.id, ref outPrice);
                                if (price <= 0) _isAvailable = false;
                            }
                            if (_isAvailable)
                                responseConent = "{\"available\":true}";
                            else
                            {
                                responseConent = "{\"available\":false,\"failure_code\": null}";
                            }
                            ErrorLog.addLog("", "get check avv", responseConent);
                        }
                    }
                }
                else
                {
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetNoStore();
                    Response.Cache.SetAllowResponseInBrowserHistory(true);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/json";

                    byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
                    string json = Encoding.ASCII.GetString(param);
                    var requestContent = json;

                    ErrorLog.addLog("", "put", requestContent);

                    try
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        if (CommonUtilities.getSYS_SETTING("is_put_test").objToInt32() == 0)
                        {
                            ChnlAirbnbClasses.ReservationConfirmationRequest notificationRequest = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.ReservationConfirmationRequest)) as ChnlAirbnbClasses.ReservationConfirmationRequest;
                            if (notificationRequest != null)
                            {
                                ErrorLog.addLog("", "reservton notification request", requestContent);
                                                                

                                RntChnlAirbnbEstateTBL currAirbnbEstateTBL = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.airbnb_id == notificationRequest.listing_id.objToInt32());
                                if (currAirbnbEstateTBL == null)
                                {
                                    ErrorLog.addLog("", "airbnb reservaion check_availability notification", "Missing AirBnb Estate");
                                    responseConent = "{\"succeed\":false,\"failure_code\": null}";
                                }
                                else
                                {
                                    var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currAirbnbEstateTBL.mr_id);                                    

                                    string date = notificationRequest.start_date;
                                    int nights = notificationRequest.nights;

                                    if (date != "" && date.Split('-').Length >= 3)
                                    {
                                        DateTime dtStart = new DateTime(date.Split('-')[0].objToInt32(), date.Split('-')[1].objToInt32(), date.Split('-')[2].objToInt32());
                                        DateTime dtEnd = dtStart.AddDays(nights);

                                        var url = Request.Url.LocalPath.ToLower();
                                        ErrorLog.addLog("", "url1", url);
                                        //if(url.Split("/")

                                        string confirmationCode = "0";
                                        if (HttpContext.Current.Request.RequestType == "PUT")
                                        {
                                            int url_split_length = url.Split('/').Length - 1;
                                            confirmationCode = url.Split('/')[url_split_length];
                                        }
                                        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
                                        RNT_TBL_RESERVATION currReservation = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.bcom_resId == confirmationCode);
                                        if (currReservation == null)
                                        {
                                            currReservation = rntUtils.newReservation();
                                            currReservation.bcom_resId = notificationRequest.confirmation_code;
                                            currReservation.agentID = agentTbl.id;
                                            currReservation.requestFullPayAccepted = 0;
                                            //currReservation.requestFullPayAccepted = agentTbl.PartPayment.objToDecimal() == 100 || agentTbl.IsAllPayment == 1 ? 1 : 0;
                                            currReservation.dtCreation = DateTime.Now;
                                            currReservation.state_pid = 6;
                                            currReservation.state_body = "";
                                            currReservation.state_date = DateTime.Now;
                                            currReservation.state_pid_user = 1;
                                            currReservation.state_subject = "Instant booking before Acceptance";
                                            currReservation.password = CommonUtilities.CreatePassword(8, false, true, false);

                                            //int roomNumber = 0;
                                            //if (currEstate != null)
                                            //    roomNumber = rntUtils.getEstateRoomNumber(currReservation, currEstate.baseAvailability.objToInt32());
                                            //currReservation.pidEstateRoomNumber = roomNumber;

                                            rntUtils.rntReservation_setDefaults(ref currReservation);
                                            DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(currReservation);
                                            DC_RENTAL.SubmitChanges();
                                            
                                        }
                                        else
                                        {
                                            currReservation.state_date = DateTime.Now;
                                            currReservation.state_subject = "Altered by Airbnb";
                                        }
                                        currReservation.code = currReservation.id.ToString().fillString("0", 7, false);
                                        currReservation.dtStart = dtStart;
                                        currReservation.dtEnd = dtEnd;
                                        currReservation.pid_estate = currAirbnbEstateTBL.mr_id;
                                        currReservation.prTotalRate = notificationRequest.listing_base_price.objToDecimal();
                                        currReservation.pr_total = notificationRequest.listing_base_price.objToDecimal();
                                        currReservation.bcom_guest_name = notificationRequest.guest_id + "";
                                        currReservation.num_adult = notificationRequest.number_of_guests;

                                        #region store MR prices
                                        currReservation.pr_part_payment_total = 0;
                                        currReservation.pr_part_owner = currReservation.pr_total;                                             

                                        #region Made change for using values of Acconto and balance configured in agent detail
                                        //if (agentTbl.PartPayment.objToDecimal() == 100 || agentTbl.IsAllPayment == 1)
                                        //{
                                        //    currReservation.pr_part_payment_total = currReservation.pr_total;
                                        //    currReservation.pr_part_owner = 0;
                                        //}
                                        //else
                                        //{
                                        //    currReservation.pr_part_payment_total = (currReservation.pr_total * agentTbl.PartPayment.objToDecimal()) / 100;
                                        //    currReservation.pr_part_owner = currReservation.pr_total - currReservation.pr_part_payment_total;
                                        //}
                                        //set pr_part_forPayment
                                        currReservation.pr_part_forPayment = currReservation.pr_part_payment_total;
                                        #endregion

                                        currReservation.pr_part_modified = 1;
                                        rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                                        outPrice.dtStart = currReservation.dtStart.Value;
                                        outPrice.dtEnd = currReservation.dtEnd.Value;
                                        outPrice.numPersCount = currReservation.num_adult.objToInt32() + currReservation.num_child_over.objToInt32();
                                        outPrice.pr_discount_owner = 0;
                                        outPrice.pr_discount_commission = 0;
                                        //outPrice.fillAgentDetails(agentTbl);
                                        outPrice.part_percentage = currEstate.pr_percentage.objToInt32();
                                        var prTotal = rntUtils.rntEstate_getPrice(0, currEstate.id, ref outPrice);
                                        //if (newRes.bcom_totalForOwner.objToDecimal() == 0)

                                        currReservation.pr_part_agency_fee = 0;
                                        currReservation.pr_reservation = currReservation.prTotalRate;
                                        //currReservation.prTotalRateOnWeb = prTotal;
                                        var prTotalRate = currReservation.pr_total - outPrice.ecoPrice - outPrice.srsPrice;

                                        //var bcom_totalForOwner = currReservation.pr_total - currReservation.agentCommissionPrice;
                                        //currReservation.bcom_totalForOwner = bcom_totalForOwner;
                                        //currReservation.pr_part_agency_fee = 0;
                                        //currReservation.prTotalCommission = currEstate.pr_percentage.objToInt32() * bcom_totalForOwner / 100;
                                        //currReservation.prTotalOwner = bcom_totalForOwner - currReservation.prTotalCommission;

                                        //if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 1)
                                        //{
                                        //    currReservation.prTotalOwner = outPrice.prTotalOwner;
                                        //    currReservation.prTotalCommission = currReservation.prTotalRate - currReservation.agentCommissionPrice - currReservation.prTotalOwner;
                                        //}
                                        //else if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 0)
                                        //{
                                        //    currReservation.prTotalCommission = outPrice.prTotalCommission;
                                        //    currReservation.prTotalOwner = currReservation.prTotalRate - currReservation.agentCommissionPrice - currReservation.prTotalCommission;
                                        //}
                                        //else if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 2)
                                        //{
                                        //    decimal ownerper = ((outPrice.prTotalOwner * 100) / outPrice.prTotalRate) / 100;
                                        //    currReservation.prTotalOwner = decimal.Multiply((currReservation.prTotalRate.objToDecimal() - currReservation.agentCommissionPrice.objToDecimal()), ownerper);
                                        //    currReservation.prTotalCommission = ((currReservation.prTotalRate.objToDecimal() - currReservation.agentCommissionPrice) - currReservation.prTotalOwner);

                                        //}
                                        #endregion

                                        //if (currReservation.pidEstateRoomNumber.objToInt32() == 0)
                                        //{
                                        //    int roomNumber = rntUtils.getEstateRoomNumber(currReservation, currEstate.baseAvailability.objToInt32());
                                        //    currReservation.pidEstateRoomNumber = roomNumber;
                                        //}

                                        DC_RENTAL.SubmitChanges();
                                        rntUtils.rntReservation_onChange(currReservation);
                                        rntUtils.reservation_checkPartPayment(currReservation, true);
                                        responseConent = "{\"succeed\":true}";
                                    }
                                    else
                                        responseConent = "{\"succeed\":false,\"failure_code\": null}";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.addLog("", "Airbnb Make/update reservation", ex.ToString());
                        responseConent = "{\"succeed\":false,\"failure_code\": null}";
                    }
                }
            }
        }
        Response.StatusCode = 200;
        Response.Write(responseConent);
        Response.End();
        return true;

    }

    public static string SendRequest(String requesUrl, String requestType, String requestContent, out string ErrorString)
    {
        ErrorString = "";
        try
        {
            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);

            ErrorLog.addLog("", "fter url", requesUrl);

            string airbnb_client_id = CommonUtilities.getSYS_SETTING("airbnb_client_id");
            string airbnb_key = CommonUtilities.getSYS_SETTING("airbnb_key");

            string authInfo = airbnb_client_id + ":" + airbnb_key;
            //string authInfo = "2i5j1ga46klepuun6lc54bsex:bu3neqo5opj75dm96vfy1f1iu";
            //string authInfo = "2i5j1ga46klepuun6lc54bsex:e889cro3pbu0j5ultmqbjpxr2";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            obj.Headers["AUTHORIZATION"] = "Basic " + authInfo;

            ErrorLog.addLog("", "After url1", requesUrl);

            obj.ContentType = "application/json";
            obj.Method = requestType;
            obj.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";

            if (requestContent != "")
            {
                Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestContent);
                obj.ContentLength = bytes.Length;
                Stream os = obj.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();
            }

            ErrorLog.addLog("", "b4 response", requesUrl);

            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            ErrorLog.addLog("", "b4 response1", requesUrl);
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            ErrorLog.addLog("", "b4 response nswer", requesUrl);
            String strRSString = _Answer.ReadToEnd().ToString();
            if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").ToInt32() == 1)
                addLog("Debug", "SendRequest", requestContent, strRSString, requesUrl);
            return strRSString;
        }
        catch (WebException ex)
        {
            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                ErrorString = reader.ReadToEnd();
                ErrorLog.addLog("", "airbnb authkey", ErrorString);
                addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
                return "";
            }
        }
        catch (Exception ex)
        {
            ErrorString = ex.ToString();
            ErrorLog.addLog("", "airbnb authkey", ex.ToString());
            addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
            return "";
        }
    }

    public static string SendRequestListing(String requesUrl, String requestType, String requestContent, string accessToken, int pidEstate, out string ErrorString)
    {
        ErrorString = "";
        try
        {

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);

            //obj.Headers["X-Airbnb-API-Key"] = "e889cro3pbu0j5ultmqbjpxr2";
            //obj.Headers["X-Airbnb-API-Key"] = "bu3neqo5opj75dm96vfy1f1iu";
            obj.Headers["X-Airbnb-API-Key"] = CommonUtilities.getSYS_SETTING("airbnb_key");
            obj.Headers["X-Airbnb-OAuth-Token"] = getAccessToken(pidEstate);

            obj.ContentType = "application/json";
            obj.Method = requestType;
            obj.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";

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
            if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").ToInt32() == 1)
                addLog("Debug", "SendRequest", requestContent, strRSString, requesUrl);
            return strRSString;
        }
        catch (WebException ex)
        {
            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                ErrorString = reader.ReadToEnd();
                ErrorLog.addLog("", "airbnb authkey", ErrorString);
                addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
                return "";
            }
        }
        catch (Exception ex)
        {
            ErrorString = ex.ToString();
            ErrorLog.addLog("", "airbnb authkey", ex.ToString());
            addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
            return "";
        }
    }

    public static void addLog(string requestType, string requestComments, string requestContent, string responseContent, string requesUrl)
    {
        try
        {
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                var item = new RntChnlAirbnbRequestLOG();
                item.uid = Guid.NewGuid();
                item.requestUrl = requesUrl;
                item.requestType = requestType;
                item.requestComments = requestComments;
                item.requestContent = requestContent;
                item.responseContent = responseContent;
                item.logDateTime = DateTime.Now;
                dc.RntChnlAirbnbRequestLOG.InsertOnSubmit(item);
                //if (UserAuthentication.CurrRoleTBL.viewOnlyNoSave.objToInt32() == 0) { dc.SaveChanges(); }
            }
        }
        catch (Exception Ex)
        {
            ErrorLog.addLog("", "ChnlAirbnbUtils.addLog", Ex.ToString() + "");
        }
    }

    public static string SendRequestReservations(String requesUrl, String requestType, String requestContent, string accessToken, out string ErrorString)
    {
        ErrorString = "";
        try
        {

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);

            obj.Headers["X-Airbnb-API-Key"] = CommonUtilities.getSYS_SETTING("airbnb_key");
            obj.Headers["X-Airbnb-OAuth-Token"] = accessToken;

            obj.ContentType = "application/json";
            obj.Method = requestType;
            obj.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";

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
            //if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").ToInt32() == 1)
            addLog("Debug", "SendRequest reservation import", requestContent, strRSString, requesUrl);
            return strRSString;
        }
        catch (WebException ex)
        {
            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                ErrorString = reader.ReadToEnd();
                ErrorLog.addLog("", "airbnb authkey web ex reservation import", ErrorString);
                addLog("ERROR", "SendRequest Res web ex reservation import", requestContent, ErrorString, requesUrl);
                return "";
            }
        }
        catch (Exception ex)
        {
            ErrorString = ex.ToString();
            ErrorLog.addLog("", "airbnb authkey reservation import", ex.ToString());
            addLog("ERROR", "SendRequest reservation import", requestContent, ErrorString, requesUrl);
            return "";
        }
    }


    public static string ImageToBase64(string path)
    {
        string base64String = "";
        using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
        {
            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, image.RawFormat);
                byte[] imageBytes = m.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }

    public static bool CheckAvailability(RNT_TB_ESTATE currEstateTB, DateTime dtStart, DateTime dtEnd, long agentID, int supportMultiUnit)
    {
        bool _isAvailable = false;

        var resList = rntUtils.rntEstate_resList(currEstateTB.id, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
        if (supportMultiUnit == 0) //MultiUnit not supported
        {
            bool isAnyReseration = resList.Any(x => x.agentID == agentID);
            _isAvailable = !isAnyReseration;
            if (_isAvailable)
            {
                _isAvailable = CheckAvailabilityByBaseAvv(currEstateTB, dtStart, dtEnd, resList);
            }
        }
        else
        {
            _isAvailable = CheckAvailabilityByBaseAvv(currEstateTB, dtStart, dtEnd, resList);
        }
        return _isAvailable;
    }

    public static bool CheckAvailabilityByBaseAvv(RNT_TB_ESTATE currEstateTB, DateTime dtStart, DateTime dtEnd, List<RNT_TBL_RESERVATION> resList)
    {
        bool _isAvailable = false;
        int baseAvv = currEstateTB.baseAvailability.objToInt32() < 1 ? 1 : currEstateTB.baseAvailability.objToInt32();
        List<int> avvList = new List<int>();
        DateTime dtCurrent = dtStart;
        while (dtCurrent < dtEnd)
        {
            var units = baseAvv - resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count();
            avvList.Add(units);
            dtCurrent = dtCurrent.AddDays(1);
        }
        int finalUnits = 0;
        if (avvList != null && avvList.Count() > 0)
            finalUnits = avvList.Min(x => x.objToInt32());

        _isAvailable = finalUnits > 0;
        return _isAvailable;
    }

    public static string getAccessToken(int pidEstate)
    {
        magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        {
            var currHost = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == pidEstate);
            //var currHost = DC_AIRBNB.RntChnlAirbnbPropertyHostRLs.SingleOrDefault(x => x.pidEstate == pidEstate);
            if (currHost == null)
            {
                ChnlAirbnbUtils.addLog("ERROR", "Missing Host", "", "", "");
                return "";
            }
            var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1 && x.userId == currHost.hostId).OrderByDescending(x => x.date).FirstOrDefault();
            if (currAuthkey == null)
            {
                //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                return "";
            }
            return currAuthkey.accessToken;
        }
    }

    public static string getAccessTokenHost(string hostId)
    {
        magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
        using (DCmodRental dc = new DCmodRental())
        {
            var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1 && x.userId == hostId).OrderByDescending(x => x.date).FirstOrDefault();
            if (currAuthkey == null)
            {
                ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                return "";
            }
            return currAuthkey.accessToken;
        }
    }

}

public static class ChnlAirbnbProps
{
    public static string IdAdMedia = "Airbnb";
}

public class ChnlAirbnbClasses
{
    public class AuthenticationCodeRequest
    {
        public string code { get; set; }

        public AuthenticationCodeRequest()
        {
            code = "";
        }
    }
    public class AuthenticationCodeResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string expires_at { get; set; }
        public string user_id { get; set; }

        public AuthenticationCodeResponse()
        {
            access_token = "";
            refresh_token = "";
            expires_at = "";
            user_id = "";
        }

    }
    public class NotificationRequest
    {
        public string action { get; set; }
    }
    public class ListingApprovalNotificationResponse
    {
        public listing_approval_status listing_approval_status { get; set; }
    }
    public class listing_approval_status
    {
        public string host_id { get; set; }
        public string listing_id { get; set; }
        public string approval_status_category { get; set; }
        public string notes { get; set; }
    }
    public class ListingSyncNotificationResponse
    {
        public string action { get; set; }
        public int host_id { get; set; }
        public List<updates> updates { get; set; }
    }
    public class AuthorizationRevoked
    {
        public string action { get; set; }
        public string host_id { get; set; }
        public string[] listing_ids { get; set; }
    }
    public class updates
    {
        public int listing_id { get; set; }
        public string synchronization_category { get; set; }
        public int after_mapping_listing_id { get; set; }

        public updates()
        {
            listing_id = 0;
            synchronization_category = "";
            after_mapping_listing_id = 0;
        }
    }
    public class Listing
    {
        public string name { get; set; }
        public int bedrooms { get; set; }
        public int bathrooms { get; set; }
        public int beds { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string country_code { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public int person_capacity { get; set; }
        public int total_inventory_count { get; set; }
        //public string property_external_id { get; set; }
        public int listing_price { get; set; }
        public string[] amenity_categories { get; set; }
        public string requested_approval_status_category { get; set; }
        public string property_type_category { get; set; }
        public Listing()
        {

        }
    }
    public class ListingDescription
    {
        public string name { get; set; }
        public string summary { get; set; }
        public string neighborhood_overview { get; set; }
        public string transit { get; set; }
        public string notes { get; set; }

        public ListingDescription()
        {
            name = "";
            summary = "";
            neighborhood_overview = "";
            transit = "";
            notes = "";
        }
    }
    public class ListingSync
    {
        public string requested_approval_status_category { get; set; }
        public string synchronization_category { get; set; }
        public ListingSync()
        {
            synchronization_category = "";
            requested_approval_status_category = "";
        }
    }
    public class ListingPolicy
    {
        public string cancellation_policy_category { get; set; }
        public string check_in_time_start { get; set; }
        public string check_in_time_end { get; set; }
        public string check_out_time { get; set; }

        public ListingPolicy()
        {
            cancellation_policy_category = "";
            check_in_time_start = "";
            check_in_time_end = "";
            check_out_time = "";
        }
    }
    public class booking_setting
    {
        public string cancellation_policy_category { get; set; }
        public string check_in_time_start { get; set; }
        public string check_in_time_end { get; set; }
        public string check_out_time { get; set; }
    }
    public class ListingPolicyResponse
    {
        public booking_setting booking_setting { get; set; }
        public ListingPolicyResponse()
        {
            booking_setting = new booking_setting();
        }
    }
    public class ListingLeadTime
    {
        public booking_lead_time booking_lead_time { get; set; }
        public ListingLeadTime()
        {
            booking_lead_time = new booking_lead_time();
        }
    }

    public class ListingLeadTimeResponse
    {
        public availability_rule availability_rule { get; set; }
        public ListingLeadTimeResponse()
        {
            availability_rule = new availability_rule();
        }
    }
    public class availability_rule
    {
        public booking_lead_time booking_lead_time { get; set; }
    }
    public class booking_lead_time
    {
        public int hours { get; set; }
        public int allow_request_to_book { get; set; }
    }
    public class ListingUnisting
    {
        public bool has_availability { get; set; }
        public ListingUnisting()
        {
            has_availability = true;
        }
    }
    public class ListingCurrency
    {
        public string listing_currency { get; set; }
        public int security_deposit { get; set; }
        public int cleaning_fee { get; set; }
        public ListingCurrency()
        {
            listing_currency = "";
        }
    }

    public class ListingResponseDetails
    {
        public ListingResponse listing { get; set; }
        public metadata metadata { get; set; }

        public ListingResponseDetails()
        {

        }
    }
    public class ListingResponse
    {
        public string name { get; set; }
        public int bedrooms { get; set; }
        public decimal bathrooms { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string country_code { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public int person_capacity { get; set; }
        public int total_inventory_count { get; set; }
        //public string property_external_id { get; set; }
        public int listing_price { get; set; }
        public string[] amenity_categories { get; set; }
        public int id { get; set; }
        public string requested_approval_status_category { get; set; }
        public string synchronization_category { get; set; }
        public bool has_availability { get; set; }
    }
    public class metadata
    {
    }
    public class ListingImage
    {
        public int listing_id { get; set; }
        public string content_type { get; set; }
        public string filename { get; set; }
        public string image { get; set; }
        public string caption { get; set; }
        public int sort_order { get; set; }
    }
    public class ListingImageResponse
    {
        public listing_photo listing_photo { get; set; }
    }
    public class listing_photo
    {
        public string id { get; set; }
        public int listing_id { get; set; }
        public string caption { get; set; }
        public int sort_order { get; set; }
        public string extra_large_url { get; set; }
        public string extra_medium_url { get; set; }
        public string large_url { get; set; }
        public string small_url { get; set; }
        public string thumbnail_url { get; set; }
    }

    public class ReservationConfirmationRequest
    {
        public string confirmation_code { get; set; }
        public string start_date { get; set; }
        public int nights { get; set; }
        public string listing_id { get; set; }
        public int number_of_guests { get; set; }
        public decimal listing_base_price { get; set; }
        public string guest_id { get; set; }
    }
    public class ReservionAcceptanceRequest
    {
        public string action { get; set; }
        public reservation reservation { get; set; }
    }
    public class ReservionRetrieve
    {
        public reservation reservation { get; set; }
    }
    public class AllResevations
    {
        public List<reservation> reservations { get; set; }
    }
    public class reservation
    {
        public string confirmation_code { get; set; }
        public decimal listing_base_price_accurate { get; set; }
        public decimal expected_payout_amount_accurate { get; set; }
        public decimal listing_cleaning_fee_accurate { get; set; }
        public decimal listing_host_fee_accurate { get; set; }
        public decimal listing_security_price_accurate { get; set; }
        public decimal listing_cancellation_host_fee_accurate { get; set; }
        public decimal occupancy_tax_amount_paid_to_host_accurate { get; set; }
        public decimal transient_occupancy_tax_paid_amount_accurate { get; set; }

        public guest_details guest_details { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }

        public string guest_email { get; set; }
        public string guest_first_name { get; set; }
        public string guest_last_name { get; set; }
        public string[] guest_phone_numbers { get; set; }
        public string guest_preferred_locale { get; set; }
        public int number_of_guests { get; set; }
        public string status_type { get; set; }
    }
    public class guest_details
    {
        public int number_of_adults { get; set; }
        public int number_of_children { get; set; }
        public int number_of_infants { get; set; }
    }
    public class ListingAvailability
    {
        public int listing_id { get; set; }
        public operation[] operations { get; set; }
    }
    public class operation
    {
        public string[] dates { get; set; }
        public string availability { get; set; }
        public int available_count { get; set; }
    }
    public class ListingPrice
    {
        public int listing_id { get; set; }
        public operationPrice[] operations { get; set; }
    }
    public class operationPriceDates
    {
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }
        public int daily_price { get; set; }
        public int min_nights { get; set; }
        public int max_nights { get; set; }
        public bool closed_to_arrival { get; set; }
        public bool closed_to_departure { get; set; }

        public operationPriceDates(DateTime DtStart, DateTime DtEnd, int minStay)
        {
            dtStart = DtStart;
            dtEnd = DtEnd;
            min_nights = minStay;
            daily_price = 0;
            max_nights = 0;
            closed_to_arrival = false;
            closed_to_departure = false;
        }

        public bool HasSamePrices(operationPriceDates compareWith)
        {
            return min_nights == compareWith.min_nights &&
                dtEnd.AddDays(1) == compareWith.dtStart &&
                closed_to_arrival == compareWith.closed_to_arrival &&
                closed_to_departure == compareWith.closed_to_departure &&
              max_nights == compareWith.max_nights &&
              daily_price == compareWith.daily_price;
        }
    }
    public class operationPrice
    {
        public string[] dates { get; set; }
        public int daily_price { get; set; }
        public int min_nights { get; set; }
        public int max_nights { get; set; }
        public bool closed_to_arrival { get; set; }
        public bool closed_to_departure { get; set; }
    }
    public class LOSPrice
    {
        public bool los_enabled { get; set; }
        public string[] los_records { get; set; }
    }
}

public class ChnlAirbnbUpdate
{
    private class PropertyNew_process
    {
        public RNT_TB_ESTATE currEstate { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                //using (DCchnlFlipKey dcChnl = new DCchnlFlipKey())
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }

                    ChnlAirbnbClasses.Listing objListing = new ChnlAirbnbClasses.Listing();
                    objListing.bathrooms = currEstate.num_rooms_bath.objToInt32();
                    objListing.bedrooms = currEstate.num_rooms_bed.objToInt32();

                    #region bedding
                    var EstateInternsIds = dc.RntEstateInternsTB.Where(x => x.pidEstate == currEstate.id && (x.pidInternsType == "Bedroom" || x.pidInternsType == "Livingroom")).Select(x => x.id).ToList();
                    var features = dc.RntEstateInternsFeatureRL.Where(x => EstateInternsIds.Contains(x.pidEstateInterns)).ToList();

                    int bedCount = 0;
                    foreach (var feature in features)
                    {
                        bedCount = bedCount + feature.count.objToInt32();
                    }
                    objListing.beds = bedCount;
                    #endregion

                    objListing.city = CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), 2, "");

                    var country = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.Where(x => x.id == currEstate.pid_country).FirstOrDefault();
                    if (country != null)
                        objListing.country_code = country.inner_notes;

                    if (!string.IsNullOrEmpty(currEstate.google_maps) && currEstate.google_maps.Split(',').Length > 1)
                    {
                        objListing.lat = currEstate.google_maps.Split(',')[0] + "";
                        objListing.lng = currEstate.google_maps.Split(',')[1] + "";
                    }

                    objListing.name = currEstate.code;
                    objListing.person_capacity = currEstate.num_persons_max.objToInt32();
                    //objListing.property_external_id = currEstate.id + "";
                    objListing.total_inventory_count = currEstate.baseAvailability;
                    objListing.zipcode = currEstate.loc_zip_code;
                    objListing.listing_price = rntUtils.rntEstate_minPrice(currEstate.id).objToInt32();
                    objListing.requested_approval_status_category = "new";
                    List<string> amenities = new List<string>();
                    var extrasList = dc.RntEstateExtrasRL.Where(x => x.pidEstate == currEstate.id).ToList();
                    foreach (var extra in extrasList)
                    {
                        var currAirbnbExtra = dc.RntChnlAirbnbLkAmenityTBL.FirstOrDefault(x => x.refId == extra.pidEstateExtras + "");
                        if (currAirbnbExtra != null)
                        {
                            amenities.Add(currAirbnbExtra.code.Trim());
                        }
                    }

                    objListing.amenity_categories = amenities.ToArray();

                    //TO DO - right now have set categories manually                 
                    var currAirbnbPropertyType = dc.RntChnlAirbnbLkPropertyTypeTBL.FirstOrDefault(x => x.refId == currEstate.pid_category + "");
                    if (currAirbnbPropertyType != null)
                    {
                        objListing.property_type_category = currAirbnbPropertyType.code;
                    }

                    string requestContent = "";
                    string requestType = "";

                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, objListing);
                    requestContent = wr.ToString();

                    ErrorLog.addLog("", "airbnb listing requestContent", requestContent);

                    requestType = "POST";
                    requesUrl = "https://api.airbnb.com/v2/listings";
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb listing response", responseData);

                    ChnlAirbnbClasses.ListingResponseDetails listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingResponseDetails)) as ChnlAirbnbClasses.ListingResponseDetails;
                    if (listingResponseDetails != null)
                    {
                        var objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                        if (objAirbnbEstate == null)
                        {
                            objAirbnbEstate = new RntChnlAirbnbEstateTBL();
                            objAirbnbEstate.mr_id = currEstate.id;
                            dc.RntChnlAirbnbEstateTBL.InsertOnSubmit(objAirbnbEstate);
                            dc.SubmitChanges();
                        }
                        objAirbnbEstate.airbnb_id = listingResponseDetails.listing.id;
                        objAirbnbEstate.status = listingResponseDetails.listing.requested_approval_status_category;
                        objAirbnbEstate.syncCategory = listingResponseDetails.listing.synchronization_category;
                        objAirbnbEstate.hasAvailbility = listingResponseDetails.listing.has_availability == true ? 1 : 0;
                        objAirbnbEstate.isSentBasicDetails = 1;
                        dc.SubmitChanges();

                        //store also in common db
                        var currEstateHost = DC_AIRBNB.RntChnlAirbnbPropertyHostRL.SingleOrDefault(x => x.pidEstate == currEstate.id && x.hostId == objAirbnbEstate.hostId);
                        if (currEstateHost != null)
                        {
                            currEstateHost.airbnb_id = objAirbnbEstate.airbnb_id.objToInt32();
                            //DC_AIRBNB.RntChnlAirbnbPropertyHostRLs.InsertOnSubmit(currEstateHost);
                            DC_AIRBNB.SubmitChanges();
                        }

                    }

                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyNew_process IdEstate:" + currEstate.id, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb property new", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyNew_process IdEstate:" + currEstate.id, "", ex.ToString(), "");

            }
        }

        public PropertyNew_process(RNT_TB_ESTATE _currEstate, bool backgroundProcess)
        {
            currEstate = _currEstate;

            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyNew_process idEstate:" + currEstate.id);
            }
            else
                doThread();
        }
    }
    public static string PropertyNew_start(RNT_TB_ESTATE _currEstate)
    {
        PropertyNew_process _tmp = new PropertyNew_process(_currEstate, false);
        return _tmp.ErrorString;
    }

    private class PropertyUpdate_process
    {
        public int IdEstate { get; set; }
        //public string requested_approval_status_category { get; set; }
        public string Status { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    ChnlAirbnbClasses.Listing objListing = new ChnlAirbnbClasses.Listing();
                    objListing.bathrooms = currEstate.num_rooms_bath.objToInt32();
                    objListing.bedrooms = currEstate.num_rooms_bed.objToInt32();

                    #region bedding
                    var EstateInternsIds = dc.RntEstateInternsTB.Where(x => x.pidEstate == currEstate.id && (x.pidInternsType == "Bedroom" || x.pidInternsType == "Livingroom")).Select(x => x.id).ToList();
                    var features = dc.RntEstateInternsFeatureRL.Where(x => EstateInternsIds.Contains(x.pidEstateInterns)).ToList();

                    int bedCount = 0;
                    foreach (var feature in features)
                    {
                        bedCount = bedCount + feature.count.objToInt32();
                    }
                    objListing.beds = bedCount;
                    #endregion

                    objListing.city = CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), 2, "");

                    var country = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.Where(x => x.id == currEstate.pid_country).FirstOrDefault();
                    if (country != null)
                        objListing.country_code = country.inner_notes;

                    if (!string.IsNullOrEmpty(currEstate.google_maps) && currEstate.google_maps.Split(',').Length > 1)
                    {
                        objListing.lat = currEstate.google_maps.Split(',')[0] + "";
                        objListing.lng = currEstate.google_maps.Split(',')[1] + "";
                    }

                    objListing.name = currEstate.code;
                    objListing.person_capacity = currEstate.num_persons_max.objToInt32();
                    //objListing.property_external_id = currEstate.id + "";
                    objListing.total_inventory_count = currEstate.baseAvailability;
                    objListing.zipcode = currEstate.loc_zip_code;
                    objListing.listing_price = rntUtils.rntEstate_minPrice(currEstate.id).objToInt32();

                    List<string> amenities = new List<string>();
                    var extrasList = dc.RntEstateExtrasRL.Where(x => x.pidEstate == currEstate.id).ToList();
                    foreach (var extra in extrasList)
                    {
                        var currAirbnbExtra = dc.RntChnlAirbnbLkAmenityTBL.FirstOrDefault(x => x.refId == extra.pidEstateExtras + "");
                        if (currAirbnbExtra != null)
                        {
                            amenities.Add(currAirbnbExtra.code.Trim());
                        }
                    }

                    objListing.amenity_categories = amenities.ToArray();

                    //TO DO - right now have set categories manually                                    
                    var currAirbnbPropertyType = dc.RntChnlAirbnbLkPropertyTypeTBL.FirstOrDefault(x => x.refId == currEstate.pid_category + "");
                    if (currAirbnbPropertyType != null)
                    {
                        objListing.property_type_category = currAirbnbPropertyType.code;
                    }
                    //if (currEstate.pid_category == 1)
                    //    objListing.property_type_category = "apartment";
                    //else if (currEstate.pid_category == 5)
                    //    objListing.property_type_category = "cottage";
                    //else if (currEstate.pid_category == 3)
                    //    objListing.property_type_category = "house";
                    //else if (currEstate.pid_category == 2)
                    //    objListing.property_type_category = "villa";

                    if (Status == "") Status = objAirbnbEstate.status;
                    if (Status == "") Status = "new";

                    objListing.requested_approval_status_category = Status;

                    ErrorLog.addLog("", "status", Status);
                    string requestContent = "";
                    string requestType = "";

                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, objListing);
                    requestContent = wr.ToString();

                    ErrorLog.addLog("", "airbnb update listing requestContent", requestContent);

                    requestType = "PUT";
                    requesUrl = "https://api.airbnb.com/v2/listings/" + objAirbnbEstate.airbnb_id;
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb listing response", responseData);

                    ChnlAirbnbClasses.ListingResponseDetails listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingResponseDetails)) as ChnlAirbnbClasses.ListingResponseDetails;
                    if (listingResponseDetails != null)
                    {
                        if (objAirbnbEstate != null)
                        {
                            //objAirbnbEstate.mr_id = currEstate.id;
                            objAirbnbEstate.airbnb_id = listingResponseDetails.listing.id;
                            objAirbnbEstate.status = listingResponseDetails.listing.requested_approval_status_category;
                            objAirbnbEstate.hasAvailbility = listingResponseDetails.listing.has_availability == true ? 1 : 0;
                            objAirbnbEstate.syncCategory = listingResponseDetails.listing.synchronization_category;
                            dc.SubmitChanges();
                        }

                        //store also in common db
                        var currEstateHost = DC_AIRBNB.RntChnlAirbnbPropertyHostRL.SingleOrDefault(x => x.pidEstate == currEstate.id && x.hostId == objAirbnbEstate.hostId + "");
                        if (currEstateHost != null)
                        {
                            currEstateHost.airbnb_id = objAirbnbEstate.airbnb_id.objToInt32();
                            //DC_AIRBNB.RntChnlAirbnbPropertyHostRLs.InsertOnSubmit(currEstateHost);
                            DC_AIRBNB.SubmitChanges();
                        }

                    }

                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyUpdate_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb property new", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyUpdate_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyUpdate_process(int idEstate, string status, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Status = status;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyUpdate_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyUpdate_start(int idEstate, string status)
    {
        PropertyUpdate_process _tmp = new PropertyUpdate_process(idEstate, status, false);
        return _tmp.ErrorString;
    }

    private class PropertyLeadTime_process
    {
        public int IdEstate { get; set; }
        public int Hours { get; set; }
        public int IsRequestToBook { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        public long agentID { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    string requestContent = "";
                    string requestType = "";
                    JsonSerializer serializer = new JsonSerializer();

                    ChnlAirbnbClasses.ListingLeadTime objListing = new ChnlAirbnbClasses.ListingLeadTime();

                    ChnlAirbnbClasses.booking_lead_time objLeadTime = new ChnlAirbnbClasses.booking_lead_time();
                    objLeadTime.allow_request_to_book = IsRequestToBook;
                    objLeadTime.hours = Hours;
                    objListing.booking_lead_time = objLeadTime;

                    TextWriter wr = new StringWriter();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, objListing);
                    requestContent = wr.ToString();

                    ErrorLog.addLog("", "airbnb lead time listing requestContent", requestContent);

                    requestType = "PUT";
                    requesUrl = "https://api.airbnb.com/v2/availability_rules/" + objAirbnbEstate.airbnb_id;
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb lead time listing response", responseData);

                    ChnlAirbnbClasses.ListingLeadTimeResponse listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingLeadTimeResponse)) as ChnlAirbnbClasses.ListingLeadTimeResponse;
                    if (listingResponseDetails != null)
                    {
                        if (objAirbnbEstate != null)
                        {
                            objAirbnbEstate.hours = listingResponseDetails.availability_rule.booking_lead_time.hours;
                            objAirbnbEstate.isAllowRequest = listingResponseDetails.availability_rule.booking_lead_time.allow_request_to_book;
                            dc.SubmitChanges();
                        }

                        //store also in common db
                        var currEstateHost = DC_AIRBNB.RntChnlAirbnbPropertyHostRL.SingleOrDefault(x => x.pidEstate == currEstate.id && x.hostId == objAirbnbEstate.hostId);
                        if (currEstateHost != null)
                        {
                            currEstateHost.airbnb_id = objAirbnbEstate.airbnb_id.objToInt32();
                            //DC_AIRBNB.RntChnlAirbnbPropertyHostRLs.InsertOnSubmit(currEstateHost);
                            DC_AIRBNB.SubmitChanges();
                        }
                    }

                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyLeadTime_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb PropertyUpdateSync_process", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyLeadTime_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyLeadTime_process(int idEstate, int hours, int isRequestToBook, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Hours = hours;
            IsRequestToBook = isRequestToBook;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyLeadTime_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyLeadTime_start(int idEstate, int hours, int isRequestToBook)
    {
        PropertyLeadTime_process _tmp = new PropertyLeadTime_process(idEstate, hours, isRequestToBook, false);
        return _tmp.ErrorString;
    }

    private class PropertyUpdateSync_process
    {
        public int IdEstate { get; set; }
        //public string requested_approval_status_category { get; set; }
        public string Status { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia );
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    string requestContent = "";
                    string requestType = "";
                    JsonSerializer serializer = new JsonSerializer();

                    if (Status == "null") requestContent = "{\"synchronization_category\":null}";
                    else
                    {
                        ChnlAirbnbClasses.ListingSync objListing = new ChnlAirbnbClasses.ListingSync();
                        objListing.synchronization_category = Status;
                        objListing.requested_approval_status_category = "ready for review";

                        //ErrorLog.addLog("", "status", Status); 
                        TextWriter wr = new StringWriter();
                        serializer.NullValueHandling = NullValueHandling.Ignore;
                        serializer.Serialize(wr, objListing);
                        requestContent = wr.ToString();
                    }
                    ErrorLog.addLog("", "airbnb update sync listing requestContent", requestContent);

                    requestType = "PUT";
                    requesUrl = "https://api.airbnb.com/v2/listings/" + objAirbnbEstate.airbnb_id;
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb update sync listing response", responseData);

                    ChnlAirbnbClasses.ListingResponseDetails listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingResponseDetails)) as ChnlAirbnbClasses.ListingResponseDetails;
                    if (listingResponseDetails != null)
                    {
                        if (objAirbnbEstate != null)
                        {
                            objAirbnbEstate.hasAvailbility = listingResponseDetails.listing.has_availability == true ? 1 : 0;
                            objAirbnbEstate.syncCategory = listingResponseDetails.listing.synchronization_category;
                            dc.SubmitChanges();
                        }

                    }

                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyUpdateSync_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb PropertyUpdateSync_process", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyUpdateSync_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyUpdateSync_process(int idEstate, string status, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Status = status;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyUpdateSync_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyUpdateSync_start(int idEstate, string status)
    {
        PropertyUpdateSync_process _tmp = new PropertyUpdateSync_process(idEstate, status, false);
        return _tmp.ErrorString;
    }

    private class PropertyUpdatePolicy_process
    {
        public int IdEstate { get; set; }
        //public string requested_approval_status_category { get; set; }
        public string Status { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        public string checkinStartTime { get; set; }
        public string checkinEndTime { get; set; }
        public string checkOutTime { get; set; }

        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    string requestContent = "";
                    string requestType = "";
                    JsonSerializer serializer = new JsonSerializer();
                    ChnlAirbnbClasses.ListingPolicy objListing = new ChnlAirbnbClasses.ListingPolicy();
                    objListing.cancellation_policy_category = Status;
                    objListing.check_in_time_start = checkinStartTime;
                    objListing.check_in_time_end = checkinEndTime;
                    objListing.check_out_time = checkOutTime;

                    //ErrorLog.addLog("", "status", Status); 
                    TextWriter wr = new StringWriter();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, objListing);
                    requestContent = wr.ToString();

                    ErrorLog.addLog("", "airbnb update policy listing requestContent", requestContent);

                    requestType = "PUT";
                    requesUrl = "https://api.airbnb.com/v2/booking_settings/" + objAirbnbEstate.airbnb_id;
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb update sync listing response", responseData);

                    ChnlAirbnbClasses.ListingPolicyResponse listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingPolicyResponse)) as ChnlAirbnbClasses.ListingPolicyResponse;
                    if (listingResponseDetails != null)
                    {
                        if (objAirbnbEstate != null)
                        {
                            objAirbnbEstate.cancelltionPolicy = listingResponseDetails.booking_setting.cancellation_policy_category;
                            objAirbnbEstate.checkinEndTime = listingResponseDetails.booking_setting.check_in_time_end;
                            objAirbnbEstate.checkinStartTime = listingResponseDetails.booking_setting.check_in_time_start;
                            objAirbnbEstate.checkOutTime = listingResponseDetails.booking_setting.check_out_time;
                            dc.SubmitChanges();
                        }

                    }

                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyUpdatePolicy_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb PropertyUpdateSync_process", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyUpdatePolicy_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyUpdatePolicy_process(int idEstate, string status, string checkin_start_time, string checkin_end_time, string checkout_time, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Status = status;
            ErrorString = "";
            checkinStartTime = checkin_start_time;
            checkinEndTime = checkin_end_time;
            checkOutTime = checkout_time;
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyUpdatePolicy_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyUpdatePolicy_process_start(int idEstate, string status, string checkin_start_time, string checkin_end_time, string checkout_time)
    {
        PropertyUpdatePolicy_process _tmp = new PropertyUpdatePolicy_process(idEstate, status, checkin_start_time, checkin_end_time, checkout_time, false);
        return _tmp.ErrorString;
    }

    private class PropertyUpdateUnlist_process
    {
        public int IdEstate { get; set; }

        public bool Status { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia );
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    ChnlAirbnbClasses.ListingUnisting objListing = new ChnlAirbnbClasses.ListingUnisting();
                    objListing.has_availability = Status;

                    ErrorLog.addLog("", "status", Status + "");
                    string requestContent = "";
                    string requestType = "";

                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, objListing);
                    requestContent = wr.ToString();

                    ErrorLog.addLog("", "airbnb update unlisting listing requestContent", requestContent);

                    requestType = "PUT";
                    requesUrl = "https://api.airbnb.com/v2/listings/" + objAirbnbEstate.airbnb_id;
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb update sync listing response", responseData);

                    ChnlAirbnbClasses.ListingResponseDetails listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingResponseDetails)) as ChnlAirbnbClasses.ListingResponseDetails;
                    if (listingResponseDetails != null)
                    {
                        if (objAirbnbEstate != null)
                        {
                            objAirbnbEstate.hasAvailbility = listingResponseDetails.listing.has_availability == true ? 1 : 0;
                            dc.SubmitChanges();
                        }

                    }

                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyUpdateSync_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb PropertyUpdateSync_process", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyUpdateSync_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyUpdateUnlist_process(int idEstate, bool status, bool backgroundProcess)
        {
            IdEstate = idEstate;
            Status = status;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyUpdateUnlist_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyUpdateUnlist_process_start(int idEstate, bool status)
    {
        PropertyUpdateUnlist_process _tmp = new PropertyUpdateUnlist_process(idEstate, status, false);
        return _tmp.ErrorString;
    }

    private class PropertyUpdateCurrency_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    ChnlAirbnbClasses.ListingCurrency objListing = new ChnlAirbnbClasses.ListingCurrency();
                    objListing.listing_currency = "EUR";
                    objListing.security_deposit = currEstate.pr_deposit.objToInt32();
                    //can send cleaning fee only if is forfait

                    //var objExtraRL = rntProps.EstateExtraRL.SingleOrDefault(x => x.pidEstate == IdEstate && x.pidEstateExtras == CommonUtilities.getSYS_SETTING("airbnb_cleaning_id").objToInt32());
                    //if (objExtraRL != null)
                    //{
                    //    ErrorLog.addLog("", "cleaning fee", "1");
                    //    List<dbRntExtrasPriceTBL> lstPrice = rntProps.ExtrasPriceTBL.Where(x => x.pidExtras == objExtraRL.pidEstateExtras).ToList();
                    //    //var extras = rntUtils.estate_getReservationExtras(IdEstate, DateTime.Now.Date, DateTime.Now.Date.AddDays(1), agentID, "en", 2, 0, 1);
                    //    //var cleaningFee = extras.FirstOrDefault(x => x.pidExtra == CommonUtilities.getSYS_SETTING("airbnb_cleaning_id").objToInt32());
                    //    if (lstPrice != null && lstPrice.Count > 0)
                    //    {
                    //        var cleaningFee = lstPrice[0].actualPrice;

                            
                    //        ErrorLog.addLog("", "cleaning fee", cleaningFee + "");
                    //        objListing.cleaning_fee = (cleaningFee + "").Replace(",", ".").objToInt32();
                    //        //}
                    //    }
                    //}
                    string requestContent = "";
                    string requestType = "";

                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, objListing);
                    requestContent = wr.ToString();

                    requestType = "PUT";
                    requesUrl = "https://api.airbnb.com/v2/pricing_settings/" + objAirbnbEstate.airbnb_id;
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb update sync listing response", responseData);

                    ChnlAirbnbClasses.ListingCurrency listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingCurrency)) as ChnlAirbnbClasses.ListingCurrency;
                    if (listingResponseDetails != null)
                    {
                        //if (objAirbnbEstate != null)
                        //{
                        //    objAirbnbEstate.hasAvailbility = listingResponseDetails.listing.has_availability == true ? 1 : 0;
                        //    dc.SaveChanges();
                        //}

                    }

                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyUpdateCurrency_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb PropertyUpdateCurrency_process", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyUpdateCurrency_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyUpdateCurrency_process(int idEstate, bool backgroundProcess)
        {
            IdEstate = idEstate;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyUpdateCurrency_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyUpdateCurrency_process_start(int idEstate)
    {
        PropertyUpdateCurrency_process _tmp = new PropertyUpdateCurrency_process(idEstate, false);
        return _tmp.ErrorString;
    }


    private class PropertyDelete_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia );
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    string requestContent = "";
                    string requestType = "";

                    JsonSerializer serializer = new JsonSerializer();

                    requestType = "DELETE";
                    requesUrl = "https://api.airbnb.com/v2/listings/" + objAirbnbEstate.airbnb_id;
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb update sync listing response", responseData);

                    ChnlAirbnbClasses.ListingResponseDetails listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingResponseDetails)) as ChnlAirbnbClasses.ListingResponseDetails;
                    if (listingResponseDetails != null)
                    {
                        if (objAirbnbEstate != null)
                        {
                            dc.RntChnlAirbnbEstateTBL.DeleteOnSubmit(objAirbnbEstate);
                            dc.SubmitChanges();

                            //delete from medias
                            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
                            var mediaIds = new List<long>();
                            var medias = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == currEstate.id).ToList();
                            foreach (var mediea in medias)
                                mediaIds.Add(mediea.id.objToInt64());

                            if (mediaIds != null && mediaIds.Count > 0)
                            {
                                var existingAirbnbMedias = dc.RntChnlAirbnbMediaTBL.Where(x => mediaIds.Contains(x.mediaId)).ToList();
                                if (existingAirbnbMedias != null && existingAirbnbMedias.Count > 0)
                                {
                                    dc.RntChnlAirbnbMediaTBL.DeleteAllOnSubmit(existingAirbnbMedias);
                                    dc.SubmitChanges();
                                }
                            }

                        }
                    }

                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyDelete_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb PropertyUpdateSync_process", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyDelete_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyDelete_process(int idEstate, bool backgroundProcess)
        {
            IdEstate = idEstate;
            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyDelete_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyDelete_process_start(int idEstate)
    {
        PropertyDelete_process _tmp = new PropertyDelete_process(idEstate, false);
        return _tmp.ErrorString;
    }

    private class PropertyDescription_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    var langs = contProps.LangTBL.Where(x => x.is_active == 1).ToList();
                    foreach (var lang in langs)
                    {
                        try
                        {
                            var currEstateLN = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == currEstate.id && x.pid_lang == lang.id);
                            if (currEstateLN == null) continue;

                            ChnlAirbnbClasses.ListingDescription objListing = new ChnlAirbnbClasses.ListingDescription();
                            objListing.name = currEstateLN.title;
                            objListing.summary = currEstateLN.description;
                            //objListing.transit = currEstateLN.villaDiretionsDescription;
                            //objListing.neighborhood_overview = currEstateLN.nbh_description;
                            //objListing.notes = currEstateLN.no;

                            string requestContent = "";
                            string requestType = "";

                            TextWriter wr = new StringWriter();
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            serializer.Serialize(wr, objListing);
                            requestContent = wr.ToString();

                            ErrorLog.addLog("", "airbnb listing description requestContent", requestContent);

                            requestType = "PUT";
                            requesUrl = "https://api.airbnb.com/v2/listing_descriptions/" + objAirbnbEstate.airbnb_id + "/" + lang.id;
                            string tmpErrorString = "";

                            var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                            if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                                ErrorLog.addLog("", "airbnb listing response", responseData);

                            ErrorString = tmpErrorString;

                            objAirbnbEstate.isSentDescription = 1;
                            dc.SubmitChanges();

                            ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyUpdate_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.addLog("", "listing description id" + currEstate.id + " lang " + lang.id, ex.ToString());
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb property update", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyUpdate_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyDescription_process(int idEstate, bool backgroundProcess)
        {
            IdEstate = idEstate;

            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyDescription_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyDescription_start(int idEstate)
    {
        PropertyDescription_process _tmp = new PropertyDescription_process(idEstate, false);
        return _tmp.ErrorString;
    }

    private class PropertyImage_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia );
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }
                    var existingAirbnbMedias = dc.RntChnlAirbnbMediaTBL.Select(x => x.mediaId).ToList();
                    var medias = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == currEstate.id && !existingAirbnbMedias.Contains(x.id) && x.type == "original").OrderBy(x => x.sequence).ToList();
                    foreach (var media in medias)
                    {
                        try
                        {
                            string image = Path.Combine(App.SRP, media.img_banner);
                            ChnlAirbnbClasses.ListingImage objListing = new ChnlAirbnbClasses.ListingImage();
                            objListing.caption = media.code;
                            objListing.filename = Path.GetFileName(image);
                            objListing.image = ChnlAirbnbUtils.ImageToBase64(image);
                            objListing.listing_id = objAirbnbEstate.airbnb_id.objToInt32();
                            objListing.sort_order = media.sequence.objToInt32();
                            objListing.content_type = MimeMapping.GetMimeMapping(image);

                            string requestContent = "";
                            string requestType = "";

                            TextWriter wr = new StringWriter();
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            serializer.Serialize(wr, objListing);
                            requestContent = wr.ToString();

                            ErrorLog.addLog("", "airbnb listing image requestContent", requestContent);

                            requestType = "POST";
                            requesUrl = "https://api.airbnb.com/v2/listing_photos";
                            string tmpErrorString = "";

                            var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                            if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                                ErrorLog.addLog("", "airbnb listing image response", responseData);

                            ChnlAirbnbClasses.ListingImageResponse listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingImageResponse)) as ChnlAirbnbClasses.ListingImageResponse;
                            if (listingResponseDetails != null)
                            {
                                RntChnlAirbnbMediaTBL objAirbnbMedia = new RntChnlAirbnbMediaTBL();
                                objAirbnbMedia.airbnbId = listingResponseDetails.listing_photo.id;
                                objAirbnbMedia.extraLargeUrl = listingResponseDetails.listing_photo.extra_large_url;
                                objAirbnbMedia.extraMediumUrl = listingResponseDetails.listing_photo.extra_medium_url;
                                objAirbnbMedia.large_url = listingResponseDetails.listing_photo.large_url;
                                objAirbnbMedia.mediaId = media.id;
                                objAirbnbMedia.small_url = listingResponseDetails.listing_photo.small_url;
                                objAirbnbMedia.thumbnail_url = listingResponseDetails.listing_photo.thumbnail_url;
                                dc.RntChnlAirbnbMediaTBL.InsertOnSubmit(objAirbnbMedia);
                                dc.SubmitChanges();
                            }

                            ErrorString = tmpErrorString;
                            ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyImage_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.addLog("", "listing image id" + media.id, ex.ToString());
                        }
                    }
                    objAirbnbEstate.isSentImages = 1;
                    dc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb property image", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyImage_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyImage_process(int idEstate, bool backgroundProcess)
        {
            IdEstate = idEstate;

            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyImage_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyImage_start(int idEstate)
    {
        PropertyImage_process _tmp = new PropertyImage_process(idEstate, false);
        return _tmp.ErrorString;
    }

    private class PropertyImageDelete_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia);
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }
                    var mediaIds = new List<long>();
                    var medias = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == currEstate.id && x.type == "original").ToList();
                    foreach (var media in medias)
                        mediaIds.Add(media.id);

                    var existingAirbnbMedias = dc.RntChnlAirbnbMediaTBL.Where(x => mediaIds.Contains(x.mediaId)).ToList();

                    foreach (var media in existingAirbnbMedias)
                    {
                        try
                        {
                            string requestContent = "";
                            string requestType = "";

                            TextWriter wr = new StringWriter();
                            JsonSerializer serializer = new JsonSerializer();

                            requestType = "DELETE";
                            requesUrl = "https://api.airbnb.com/v2/listing_photos/" + media.airbnbId;
                            string tmpErrorString = "";

                            var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                            if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                                ErrorLog.addLog("", "airbnb listing image response", responseData);

                            if (responseData != "")
                            {
                                var airbnbImage = dc.RntChnlAirbnbMediaTBL.SingleOrDefault(x => x.airbnbId == media.airbnbId);
                                if (airbnbImage != null)
                                {
                                    dc.RntChnlAirbnbMediaTBL.DeleteOnSubmit(airbnbImage);
                                    dc.SubmitChanges();
                                }

                            }
                            //ChnlAirbnbClasses.ListingImageResponse listingResponseDetails = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ListingImageResponse)) as ChnlAirbnbClasses.ListingImageResponse;
                            //if (listingResponseDetails != null)
                            //{
                            //    dbRntChnlAirbnbMediaTBL objAirbnbMedia = new dbRntChnlAirbnbMediaTBL();
                            //    objAirbnbMedia.airbnbId = listingResponseDetails.listing_photo.id;
                            //    objAirbnbMedia.extraLargeUrl = listingResponseDetails.listing_photo.extra_large_url;
                            //    objAirbnbMedia.extraMediumUrl = listingResponseDetails.listing_photo.extra_medium_url;
                            //    objAirbnbMedia.large_url = listingResponseDetails.listing_photo.large_url;
                            //    objAirbnbMedia.mediaId = media.id;
                            //    objAirbnbMedia.small_url = listingResponseDetails.listing_photo.small_url;
                            //    objAirbnbMedia.thumbnail_url = listingResponseDetails.listing_photo.thumbnail_url;
                            //    dc.Add(objAirbnbMedia);
                            //    dc.SaveChanges();
                            //}

                            ErrorString = tmpErrorString;
                            ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyImage_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.addLog("", "listing image id", ex.ToString());
                        }
                    }
                    //objAirbnbEstate.isSentImages = 1;
                    dc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb property image delete", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyImageDelete_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyImageDelete_process(int idEstate, bool backgroundProcess)
        {
            IdEstate = idEstate;

            ErrorString = "";
            if (backgroundProcess)
            {
                Action<object> action = (object obj) => { doThread(); };
                AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.PropertyImageDelete_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string PropertyImageDelete_start(int idEstate)
    {
        PropertyImageDelete_process _tmp = new PropertyImageDelete_process(idEstate, false);
        return _tmp.ErrorString;
    }

    private class PropertyAvailabilityUpdate_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }

        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia );
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                    var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;

                    var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                    var list = new List<rntExts.AvvListPerDates>();
                    DateTime dtCurrent = dtStart;

                    ChnlAirbnbClasses.ListingAvailability objListing = new ChnlAirbnbClasses.ListingAvailability();
                    objListing.listing_id = objAirbnbEstate.airbnb_id.objToInt32();

                    List<ChnlAirbnbClasses.operation> operations = new List<ChnlAirbnbClasses.operation>();
                    while (dtCurrent < dtEnd)
                    {
                        int baseAvv = currEstate.baseAvailability.objToInt32() < 1 ? 1 : currEstate.baseAvailability.objToInt32();
                        var units = baseAvv - resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count();
                        if (units <= 0) units = 0;
                        bool isAvv = units > 0;
                        var lastDatePrice = list.LastOrDefault();
                        if (lastDatePrice == null || lastDatePrice.IsAvv != isAvv || lastDatePrice.Units != units) list.Add(new rntExts.AvvListPerDates(dtCurrent, dtCurrent, isAvv, units));
                        else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
                        dtCurrent = dtCurrent.AddDays(1);
                    }

                    foreach (var tmp in list)
                    {
                        ChnlAirbnbClasses.operation operation = new ChnlAirbnbClasses.operation();
                        operation.availability = tmp.Units > 0 ? "default" : "unavailable";
                        operation.available_count = tmp.Units;

                        List<string> lstDates = new List<string>();
                        lstDates.Add(Convert.ToDateTime(tmp.DtStart).ToString("yyyy-MM-dd") + ":" + Convert.ToDateTime(tmp.DtEnd.Date).ToString("yyyy-MM-dd"));

                        operation.dates = lstDates.ToArray();
                        operations.Add(operation);
                    }
                    objListing.operations = operations.ToArray();
                    string requestContent = "";
                    string requestType = "";

                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, objListing);
                    requestContent = wr.ToString();

                    ErrorLog.addLog("", "airbnb update listing requestContent", requestContent);

                    requestType = "POST";
                    requesUrl = "https://api.airbnb.com/v2/calendar_operations?_allow_dates_overlap=true";
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb listing response", responseData);

                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyUpdate_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb property new", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyUpdate_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public PropertyAvailabilityUpdate_process(int idEstate, DateTime dtStart, DateTime dtEnd, bool backgroundProcess)
        {
            IdEstate = idEstate;
            DtStart = dtStart;
            DtEnd = dtEnd;
            ErrorString = "";
            if (backgroundProcess)
            {
                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Highest;
                t.Start();

                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskSchedulerPriority.AddTask(action, "ChnlAirbnbUpdate.PropertyAvailabilityUpdate_process idEstate:" + idEstate);
            }
            else
                doThread();
        }

        public PropertyAvailabilityUpdate_process(int idEstate, bool backgroundProcess)
        {
            IdEstate = idEstate;
            ErrorString = "";
            if (backgroundProcess)
            {
                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Highest;
                t.Start();

                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskSchedulerPriority.AddTask(action, "ChnlAirbnbUpdate.PropertyAvailabilityUpdate_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string UpdateAvailabilityUpdate_start(int idEstate, DateTime dtStart, DateTime dtEnd)
    {
        PropertyAvailabilityUpdate_process _tmp = new PropertyAvailabilityUpdate_process(idEstate, dtStart, dtEnd, true);
        return _tmp.ErrorString;
    }
    public static string UpdateAvailabilityUpdate_start(int idEstate)
    {
        PropertyAvailabilityUpdate_process _tmp = new PropertyAvailabilityUpdate_process(idEstate, true);
        return _tmp.ErrorString;
    }

    private class UpdateRates_start_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }

        void doThread()
        {
            try
            {
                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia );
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                    if (agentID == 0)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
                        return;
                    }
                    var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
                    if (currAuthkey == null)
                    {
                        //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
                        return;
                    }
                    RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
                    if (currEstate == null)
                    {
                        //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                        return;
                    }

                    RntChnlAirbnbEstateTBL objAirbnbEstate = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.mr_id == currEstate.id);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }

                    var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                    var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;

                    var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                    var list = new List<ChnlAirbnbClasses.operationPriceDates>();
                    DateTime dtCurrent = dtStart;

                    ChnlAirbnbClasses.ListingPrice objListing = new ChnlAirbnbClasses.ListingPrice();
                    objListing.listing_id = objAirbnbEstate.airbnb_id.objToInt32();

                    #region Rates

                    bool sendMaxPrice = false;
                    if (!DtStart.HasValue && !DtEnd.HasValue) sendMaxPrice = true;

                    int outError;
                    var priceListPerDates = rntUtils.estate_getPriceListPerDates(IdEstate,0, dtStart, dtEnd, out outError);
                    if (priceListPerDates == null || priceListPerDates.Count == 0)
                    {
                        ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " has no prices");
                        return;
                    }

                    List<ChnlAirbnbClasses.operationPrice> operations = new List<ChnlAirbnbClasses.operationPrice>();
                    foreach (var tmp in priceListPerDates)
                    {
                        var price = tmp.Prices[currEstate.num_persons_max.objToInt32()];
                        decimal currPrice = 0;
                        decimal changeAmount = 0;

                        while (dtCurrent <= tmp.DtEnd)
                        {
                            var datePrices = new ChnlAirbnbClasses.operationPriceDates(dtCurrent, dtCurrent, tmp.MinStay);

                            bool closed_arrival = false;
                            bool closed_departure = false;

                            //if (tmp.lstClosedArrival != null && tmp.lstClosedArrival.Count > 0)
                            //{
                            //    if ((dtCurrent.DayOfWeek == DayOfWeek.Monday && tmp.lstClosedArrival.Contains(1)) || (dtCurrent.DayOfWeek == DayOfWeek.Tuesday && tmp.lstClosedArrival.Contains(2)) || (dtCurrent.DayOfWeek == DayOfWeek.Wednesday && tmp.lstClosedArrival.Contains(3)) || (dtCurrent.DayOfWeek == DayOfWeek.Thursday && tmp.lstClosedArrival.Contains(4)) || (dtCurrent.DayOfWeek == DayOfWeek.Friday && tmp.lstClosedArrival.Contains(5)) || (dtCurrent.DayOfWeek == DayOfWeek.Saturday && tmp.lstClosedArrival.Contains(6)) || (dtCurrent.DayOfWeek == DayOfWeek.Sunday && tmp.lstClosedArrival.Contains(7)))
                            //        closed_arrival = true;
                            //}

                            //if (tmp.lstClosedDeparture != null && tmp.lstClosedDeparture.Count > 0)
                            //{
                            //    if ((dtCurrent.DayOfWeek == DayOfWeek.Monday && tmp.lstClosedDeparture.Contains(1)) || (dtCurrent.DayOfWeek == DayOfWeek.Tuesday && tmp.lstClosedDeparture.Contains(2)) || (dtCurrent.DayOfWeek == DayOfWeek.Wednesday && tmp.lstClosedDeparture.Contains(3)) || (dtCurrent.DayOfWeek == DayOfWeek.Thursday && tmp.lstClosedDeparture.Contains(4)) || (dtCurrent.DayOfWeek == DayOfWeek.Friday && tmp.lstClosedDeparture.Contains(5)) || (dtCurrent.DayOfWeek == DayOfWeek.Saturday && tmp.lstClosedDeparture.Contains(6)) || (dtCurrent.DayOfWeek == DayOfWeek.Sunday && tmp.lstClosedDeparture.Contains(7)))
                            //        closed_departure = true;
                            //}

                            datePrices.max_nights = currEstate.nights_max.objToInt32();
                            if (datePrices.max_nights == 0) datePrices.max_nights = 28;
                            datePrices.closed_to_arrival = closed_arrival;
                            datePrices.closed_to_departure = closed_departure;
                            datePrices.daily_price = price.objToInt32();

                            var lastDatePrice = list.LastOrDefault();
                            if (lastDatePrice == null || !lastDatePrice.HasSamePrices(datePrices)) list.Add(datePrices);
                            else lastDatePrice.dtEnd = lastDatePrice.dtEnd.AddDays(1);
                            dtCurrent = dtCurrent.AddDays(1);
                        }
                    }

                    foreach (var tmp in list)
                    {
                        ChnlAirbnbClasses.operationPrice operation = new ChnlAirbnbClasses.operationPrice();
                        operation.daily_price = tmp.daily_price;

                        List<string> lstDates = new List<string>();
                        lstDates.Add(Convert.ToDateTime(tmp.dtStart).ToString("yyyy-MM-dd") + ":" + Convert.ToDateTime(tmp.dtEnd.Date).ToString("yyyy-MM-dd"));

                        operation.dates = lstDates.ToArray();
                        operation.closed_to_arrival = tmp.closed_to_arrival;
                        operation.closed_to_departure = tmp.closed_to_departure;
                        operation.min_nights = tmp.min_nights;
                        operation.max_nights = tmp.max_nights;
                        operations.Add(operation);
                    }
                    #endregion

                    objListing.operations = operations.ToArray();
                    string requestContent = "";
                    string requestType = "";

                    TextWriter wr = new StringWriter();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(wr, objListing);
                    requestContent = wr.ToString();

                    ErrorLog.addLog("", "airbnb update price listing requestContent", requestContent);

                    requestType = "POST";
                    requesUrl = "https://api.airbnb.com/v2/calendar_operations?_allow_dates_overlap=true";
                    string tmpErrorString = "";

                    var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
                    if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
                        ErrorLog.addLog("", "airbnb listing response", responseData);


                    ErrorString = tmpErrorString;
                    ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyUpdate_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "airbnb property new", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyUpdate_process IdEstate:" + IdEstate, "", ex.ToString(), "");

            }
        }

        public UpdateRates_start_process(int idEstate, DateTime dtStart, DateTime dtEnd, bool backgroundProcess)
        {
            IdEstate = idEstate;
            DtStart = dtStart;
            DtEnd = dtEnd;
            ErrorString = "";
            if (backgroundProcess)
            {
                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Highest;
                t.Start();

                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.UpdateRates_start_process idEstate:" + idEstate);
            }
            else
                doThread();
        }

        public UpdateRates_start_process(int idEstate, bool backgroundProcess)
        {
            IdEstate = idEstate;
            ErrorString = "";
            if (backgroundProcess)
            {
                ThreadStart start = new ThreadStart(doThread);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Highest;
                t.Start();

                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskSchedulerPriority.AddTask(action, "ChnlAirbnbUpdate.UpdateRates_start_process idEstate:" + idEstate);
            }
            else
                doThread();
        }
    }
    public static string UpdateRates_start(int idEstate, DateTime dtStart, DateTime dtEnd)
    {
        UpdateRates_start_process _tmp = new UpdateRates_start_process(idEstate, dtStart, dtEnd, true);
        return _tmp.ErrorString;
    }
    public static string UpdateRates_start(int idEstate)
    {
        UpdateRates_start_process _tmp = new UpdateRates_start_process(idEstate, true);
        return _tmp.ErrorString;
    }

    private class UpdateRatesLOS_start_process
    {
        public int IdEstate { get; set; }
        public long agentID { get; set; }
        public string ErrorString { get; set; }
        public string requesUrl { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }

        //void doThread()
        //{
        //    try
        //    {
        //        magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
        //        using (DCmodRental dc = new DCmodRental())
        //        {
        //            var agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlAirbnbProps.IdAdMedia );
        //            if (agentTbl != null)
        //                agentID = agentTbl.id;
        //            if (agentID == 0)
        //            {
        //                ChnlAirbnbUtils.addLog("ERROR", "AGENT NOT FOUND OR NOT ACTIVE", "", "", "");
        //                return;
        //            }
        //            var currAuthkey = DC_AIRBNB.RntChnlAirbnbAccessTokens.Where(x => x.isActive == 1).OrderByDescending(x => x.date).FirstOrDefault();
        //            if (currAuthkey == null)
        //            {
        //                //ChnlAirbnbUtils.addLog("ERROR", "Missing Auth Token", "", "", "");
        //                return;
        //            }
        //            RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
        //            if (currEstate == null)
        //            {
        //                //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
        //                return;
        //            }

        //            dbRntChnlAirbnbEstateTBL objAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == currEstate.id);
        //            if (objAirbnbEstate == null)
        //            {
        //                ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
        //                ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
        //                return;
        //            }

        //            var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
        //            var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddMonths(18).Date;
        //            //if (IdEstate == 2010)
        //            //    dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddMonths(3).Date;

        //            int initialPerson = currEstate.num_persons_min.objToInt32();
        //            if (initialPerson == 0) initialPerson = 1;

        //            int num_persons_max = currEstate.num_persons_max.objToInt32();
        //            if (num_persons_max == 0) num_persons_max = 1;

        //            List<dbRntDiscountPromoTBL> DiscountPromoTBLs = new List<dbRntDiscountPromoTBL>();
        //            List<dbRntEstatePriceV6DatesVIEW> estatePeriodListMain = new List<dbRntEstatePriceV6DatesVIEW>();
        //            var estatePriceV6 = new List<dbRntEstatePriceV6TBL>();
        //            //var estatePriceV6 = (dbRntEstatePriceV6TBL)null;                                 

        //            //estatePriceV6 = dc.dbRntEstatePriceV6TBLs.Where(x => x.pidEstate == IdEstate).ToList();
        //            //if (estatePriceV6 == null || (estatePriceV6.Count > 0 && estatePriceV6[0].priceForExtraPerson.objToDecimal() <= 0)) { initialPerson = num_persons_max; }

        //            //DiscountPromoTBLs = dc.dbRntDiscountPromoTBLs.ToList();
        //            //estatePeriodListMain = dc.dbRntEstatePriceV6DatesVIEWs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd.AddDays(31) && x.pidEstate == IdEstate).ToList();

        //            //var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
        //            var list = new List<ChnlAirbnbClasses.LOSPrice>();
        //            DateTime dtCurrent = dtStart;

        //            ChnlAirbnbClasses.LOSPrice objListing = new ChnlAirbnbClasses.LOSPrice();
        //            objListing.los_enabled = true;

        //            #region Rates
        //            bool sendMaxPrice = false;
        //            if (!DtStart.HasValue && !DtEnd.HasValue) sendMaxPrice = true;

        //            List<string> lstLOS = new List<string>();
        //            var AgentContractRL = dc.dbRntEstateAgentContractRLs.Where(x => x.pidEstate == IdEstate && x.pidAgent == agentID).Select(x => x.pidAgentContract).ToList();
        //            var AgentContractTBL = dc.dbRntAgentContractTBLs.FirstOrDefault(x => x.dtStart <= DateTime.Now && x.dtEnd >= DateTime.Now && AgentContractRL.Contains(x.id) && x.contractType != null && x.contractType.ToLower() == "ppb");

        //            estatePeriodListMain = dc.dbRntEstatePriceV6DatesVIEWs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd.AddDays(31) && x.pidEstate == IdEstate).ToList();
        //            var nightRanges = dc.dbRntEstateNightRangeTBs.Where(x => x.pidEstate == IdEstate).ToList();

        //            while (dtCurrent <= dtEnd)
        //            {
        //                for (int pers = initialPerson; pers <= num_persons_max; pers++)
        //                {
        //                    string strLOS = "";
        //                    strLOS = dtCurrent.ToString("yyyy-MM-dd") + "," + pers;

        //                    int n = 1;
        //                    while (n <= 28)
        //                    {
        //                        DateTime dtTemp = dtCurrent.AddDays(n);

        //                        var currNightRange = new dbRntEstateNightRangeTB();
        //                        currNightRange = nightRanges.SingleOrDefault(x => x.nightsFrom <= n && x.nightsTo >= n);
        //                        if (currNightRange == null)
        //                        {
        //                            n = n + 1;
        //                            //ErrorLog.addLog("", "bcom los " + IdEstate, "night range empty");
        //                            continue;
        //                        }

        //                        rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
        //                        outPrice.dtStart = dtCurrent;
        //                        outPrice.dtEnd = dtTemp;
        //                        outPrice.numPersCount = pers;
        //                        outPrice.numPers_adult = pers;
        //                        outPrice.numPers_childOver = 0;

        //                        outPrice.fillAgentDetails(agentTbl);
        //                        var estatePeriodList = estatePeriodListMain.Where(x => x.dtEnd >= dtCurrent && x.dtStart <= dtTemp && x.pidNightRange == currNightRange.id).ToList();
        //                        var price = rntUtils.rntEstate_getPrice_LOS(0, IdEstate, currEstate, ref outPrice, AgentContractTBL, estatePeriodList).objToInt32();

        //                        //RNT_TBL_RESERVATION tmpRes;
        //                        //bool _isAvailable = rntUtils.rntEstate_isAvailable(IdEstate, dtCurr, dtTemp, 0, out tmpRes);
        //                        strLOS = strLOS + "," + price;
        //                        n = n + 1;
        //                    }
        //                    lstLOS.Add(strLOS);
        //                }
        //                dtCurrent = dtCurrent.AddDays(1);
        //            }
        //            //}
        //            //}
        //            #endregion

        //            objListing.los_records = lstLOS.ToArray();
        //            string requestContent = "";
        //            string requestType = "";

        //            TextWriter wr = new StringWriter();
        //            JsonSerializer serializer = new JsonSerializer();
        //            serializer.NullValueHandling = NullValueHandling.Ignore;
        //            serializer.Serialize(wr, objListing);
        //            requestContent = wr.ToString();

        //            ErrorLog.addLog("", "airbnb update price los listing requestContent", requestContent);


        //            requestType = "PUT";
        //            requesUrl = "https://api.airbnb.com/v2/los_records/" + objAirbnbEstate.airbnb_id + "?_full_update=false";
        //            string tmpErrorString = "";

        //            var responseData = ChnlAirbnbUtils.SendRequestListing(requesUrl, requestType, requestContent, currAuthkey.accessToken, currEstate.id, out tmpErrorString);
        //            if (CommonUtilities.getSYS_SETTING("rntChnlAirbnbDebug").objToInt32() == 1)
        //                ErrorLog.addLog("", "airbnb price los listing response", responseData);


        //            ErrorString = tmpErrorString;
        //            ChnlAirbnbUtils.addLog("UPDATE", "ChnlAirbnbUpdate.PropertyUpdateLOS_process IdEstate:" + IdEstate, requestContent, responseData, requesUrl);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.addLog("", "airbnb property new", ex.ToString());
        //        ChnlAirbnbUtils.addLog("ERROR", "ChnlAirbnbUpdate.PropertyUpdate_process IdEstate:" + IdEstate, "", ex.ToString(), "");

        //    }
        //}

        public UpdateRatesLOS_start_process(int idEstate, DateTime dtStart, DateTime dtEnd, bool backgroundProcess)
        {
            IdEstate = idEstate;
            DtStart = dtStart;
            DtEnd = dtEnd;
            ErrorString = "";
            if (backgroundProcess)
            {
                //ThreadStart start = new ThreadStart(doThread);
                //Thread t = new Thread(start);
                //t.Priority = ThreadPriority.Highest;
                //t.Start();

                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskScheduler.AddTask(action, "ChnlAirbnbUpdate.UpdateRatesLOS_start_process idEstate:" + idEstate);
            }
            //else
                //doThread();
        }

        public UpdateRatesLOS_start_process(int idEstate, bool backgroundProcess)
        {
            IdEstate = idEstate;
            ErrorString = "";
            if (backgroundProcess)
            {
                //ThreadStart start = new ThreadStart(doThread);
                //Thread t = new Thread(start);
                //t.Priority = ThreadPriority.Highest;
                //t.Start();
                //Action<object> action = (object obj) => { doThread(); };
                //AppUtilsTaskSchedulerPriority.AddTask(action, "ChnlAirbnbUpdate.UpdateRatesLOS_start_process idEstate:" + idEstate);
            }
            //else
            //    doThread();
        }
    }
    public static string UpdateRatesLOS_start(int idEstate, DateTime dtStart, DateTime dtEnd)
    {
        UpdateRatesLOS_start_process _tmp = new UpdateRatesLOS_start_process(idEstate, dtStart, dtEnd, true);
        return _tmp.ErrorString;
    }
    public static string UpdateRatesLOS_start(int idEstate)
    {
        //take 6 parts

        var dtStart1 = DateTime.Now.Date;
        var dtEnd1 = dtStart1.AddMonths(3);
        UpdateRatesLOS_start_process _tmp1 = new UpdateRatesLOS_start_process(idEstate, dtStart1, dtEnd1, true);
        //return _tmp1.ErrorString;

        var dtStart2 = dtEnd1;
        var dtEnd2 = dtStart2.AddMonths(3);
        UpdateRatesLOS_start_process _tmp2 = new UpdateRatesLOS_start_process(idEstate, dtStart2, dtEnd2, true);
        //return _tmp2.ErrorString;

        var dtStart3 = dtEnd2;
        var dtEnd3 = dtStart3.AddMonths(3);
        UpdateRatesLOS_start_process _tmp3 = new UpdateRatesLOS_start_process(idEstate, dtStart3, dtEnd3, true);

        var dtStart4 = dtEnd3;
        var dtEnd4 = dtStart4.AddMonths(3);
        UpdateRatesLOS_start_process _tmp4 = new UpdateRatesLOS_start_process(idEstate, dtStart4, dtEnd4, true);

        var dtStart5 = dtEnd4;
        var dtEnd5 = dtStart5.AddMonths(3);
        UpdateRatesLOS_start_process _tmp5 = new UpdateRatesLOS_start_process(idEstate, dtStart5, dtEnd5, true);

        var dtStart6 = dtEnd5;
        var dtEnd6 = dtStart6.AddMonths(3);
        UpdateRatesLOS_start_process _tmp6 = new UpdateRatesLOS_start_process(idEstate, dtStart6, dtEnd6, true);

        return _tmp1.ErrorString + " ," + _tmp2.ErrorString + " ," + _tmp3.ErrorString + " ," + _tmp4.ErrorString + " ," + _tmp5.ErrorString + " ," + _tmp6.ErrorString;
        //UpdateRatesLOS_start_process _tmp = new UpdateRatesLOS_start_process(idEstate, true);
        //return _tmp.ErrorString;
    }
}
