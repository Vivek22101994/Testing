using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using RentalInRome.data;
using ModRental;
using System.Threading;
using System.Net;
using ModAuth;

public class BcomProps
{
    public static int RateTypeStandard = 1;
    public static int RateTypeNotRefund = 2;
    public static int RateTypeSpecial = 3;

    public static int RateTypeStandard2 = 4;
    public static int RateTypeStandard3 = 5;
    public static int RateTypeStandard4 = 6;

    public static int PriceTypeSingle = 1;
    public static int PriceTypeFull = 2;
}
public class BcomImport
{
    private class BcomImport_process
    {
        string username { get; set; }
        string password { get; set; }
        string hotel_id { get; set; }
        string last_change { get; set; }
        long agentID { get; set; }
        RntAgentTBL agentTbl { get; set; }
        private long GetAgentClientId(XElement customer, long Id)
        {
            long clientId = Id;
            if (customer == null) return 0;
            var email = customer.Element("email") != null ? customer.Element("email").Value : "";
            using (magaAuth_DataContext dc = maga_DataContext.DC_Auth)
            {
                var _client = dc.AuthClientTBL.FirstOrDefault(x => x.id == Id);
                if (_client == null)
                {
                    _client = dc.AuthClientTBL.FirstOrDefault(x => x.pidAgent == agentID && x.contactEmail == email);
                    if (_client == null || email == "")
                    {
                        _client = new AuthClientTBL();
                        _client.pidAgent = agentID;
                        _client.uid = Guid.NewGuid();
                        _client.createdDate = DateTime.Now;
                        _client.createdUserID = 1;
                        _client.createdUserNameFull = "System";
                        _client.contactEmail = email;
                        dc.AuthClientTBL.InsertOnSubmit(_client);
                        dc.SubmitChanges();
                        _client.code = _client.id.ToString().fillString("0", 6, false);
                        dc.SubmitChanges();
                    }
                    clientId = _client.id;
                }
                _client.contactEmail = email;
                _client.nameFirst = customer.Element("first_name") != null ? customer.Element("first_name").Value : "";
                _client.nameLast = customer.Element("last_name") != null ? customer.Element("last_name").Value : "";
                _client.nameFull = _client.nameFirst + " " + _client.nameLast;
                //_client.bcom_remark = customer.Element("remarks") != null ? customer.Element("remarks").Value : "";
                string countrycode = customer.Element("countrycode") != null ? customer.Element("countrycode").Value : "";
                var countryLk = authProps.CountryLK.FirstOrDefault(x => x.code != null && x.code.ToLower() == countrycode.ToLower());
                _client.locAddress = customer.Element("address") != null ? customer.Element("address").Value : "";
                _client.locCity = customer.Element("city") != null ? customer.Element("city").Value : "";
                _client.locCountry = countryLk != null ? countryLk.title : countrycode;
                _client.locZipCode = customer.Element("zip") != null ? customer.Element("zip").Value : "";
                _client.contactPhoneMobile = customer.Element("telephone") != null ? customer.Element("telephone").Value : "";
                _client.docNum = customer.Element("dc_issue_number") != null ? customer.Element("dc_issue_number").Value : "";
                _client.docIssueDate = customer.Element("dc_start_date") != null ? customer.Element("dc_start_date").Value.ToString().Replace("-", "").JSCal_stringToDate() : (DateTime?)null;
                _client.isActive = 1;
                dc.SubmitChanges();
            }
            return clientId;
        }
        private void FillReservationData(XElement temproom, RNT_TB_ESTATE currEstate, ref RNT_TBL_RESERVATION newRes, XElement customer)
        {
            newRes.dtStart = temproom.Element("arrival_date") != null ? temproom.Element("arrival_date").Value.ToString().Replace("-", "").JSCal_stringToDate() : DateTime.Now;
            newRes.dtEnd = temproom.Element("departure_date") != null ? temproom.Element("departure_date").Value.ToString().Replace("-", "").JSCal_stringToDate() : DateTime.Now.AddDays(5);
            //var addons = temproom.Element("addons");
            //if (addons != null)
            //{
            //    booknote = booknote + temproom.Element("addons");
            //}
            newRes.bcom_extrainfo = temproom.Element("extra_info") != null ? temproom.Element("extra_info").Value : "";
            newRes.bcom_facilities = temproom.Element("facilities") != null ? temproom.Element("facilities").Value : "";
            newRes.bcom_info = temproom.Element("info") != null ? temproom.Element("info").Value : "";
            newRes.bcom_mealplan = temproom.Element("meal_plan") != null ? temproom.Element("meal_plan").Value : "";
            newRes.bcom_maxChidren = temproom.Element("max_children") != null ? temproom.Element("max_children").Value.ToInt32() : 0;
            newRes.bcom_smoking = temproom.Element("smoking") != null ? temproom.Element("smoking").Value : "";
            newRes.bcom_loyalityid = temproom.Element("loyalty_id") != null ? temproom.Element("loyalty_id").Value : "";
            if (customer != null)
                newRes.notesEco = customer.Element("remarks") != null ? customer.Element("remarks").Value : "";

            var booknote = "";
            booknote += temproom.Element("extra_info") != null ? "" + temproom.Element("extra_info") : "";
            booknote += temproom.Element("facilities") != null ? "" + temproom.Element("facilities") : "";
            booknote += temproom.Element("info") != null ? "" + temproom.Element("info") : "";
            booknote += temproom.Element("meal_plan") != null ? "" + temproom.Element("meal_plan") : "";
            booknote += temproom.Element("smoking") != null ? "" + temproom.Element("smoking") : "";
            booknote += temproom.Element("loyalty_id") != null ? "" + temproom.Element("loyalty_id") : "";

            #region change for new guest count
            int num_adult = 0;
            int num_child = 0;

            try
            {
                var guestCountList = temproom.Descendants("guest_counts").Elements("guest_count");
                foreach (var guest_counts in guestCountList)
                {
                    string type = guest_counts.Attribute("type").Value + "";
                    if (type == "adult")
                    {
                        num_adult = num_adult + guest_counts.Attribute("count").Value.objToInt32();
                    }
                    else if (type == "child")
                    {
                        num_child = num_child + guest_counts.Attribute("count").Value.objToInt32();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "bcom import guest count", ex.ToString());
            }
            newRes.num_adult = num_adult;
            newRes.num_child_over = num_child;
            if (num_adult == 0)
                newRes.num_adult = temproom.Element("numberofguests") != null ? temproom.Element("numberofguests").Value.ToInt32() : 0;
            #endregion

            newRes.is_dtStartTimeChanged = 0;
            newRes.is_dtEndTimeChanged = 0;

            newRes.agentDiscountType = agentTbl.pidDiscountType.objToInt32();
            newRes.agentDiscountNotPayed = 0;
            newRes.requestFullPayAccepted = 1;
            if (newRes.agentDiscountType == 0) newRes.agentDiscountType = 1;

            var totalprice = temproom.Element("totalprice") != null ? temproom.Element("totalprice").Value.Replace(",", ".").ToDecimal() : 0;
            var commissionamount = temproom.Element("commissionamount") != null ? temproom.Element("commissionamount").Value.Replace(",", ".").ToDecimal() : 0;
            newRes.bcom_commissionamount = commissionamount;
            newRes.bcom_currencycode = temproom.Element("currencycode") != null ? temproom.Element("currencycode").Value : "";
            newRes.bcom_room_price = totalprice;
            newRes.agentCommissionPrice = commissionamount;
            var bcom_totalForOwner = totalprice - commissionamount;
            newRes.pr_paymentType = "cc";
            newRes.pr_total = totalprice;
            newRes.pr_part_payment_total = 0;
            newRes.pr_part_owner = totalprice;
            newRes.pr_part_modified = 1;


            string countrycode = customer.Element("countrycode") != null ? customer.Element("countrycode").Value : "";
            var countryLk = authProps.CountryLK.FirstOrDefault(x => x.code != null && x.code.ToLower() == countrycode.ToLower());
            newRes.password = CommonUtilities.CreatePassword(8, false, true, false);
            newRes.cl_email = customer.Element("email") != null ? customer.Element("email").Value : agentTbl.contactEmail;
            newRes.cl_name_full = temproom.Element("guest_name") != null ? temproom.Element("guest_name").Value : agentTbl.nameCompany;
            newRes.cl_name_honorific = "";
            newRes.cl_loc_country = countryLk != null ? countryLk.title : agentTbl.locCountry;
            newRes.cl_pid_discount = -1;
            newRes.cl_pid_lang = agentTbl.pidLang;
            newRes.cl_isCompleted = 0;
            newRes.bcom_guest_name = temproom.Element("guest_name") != null ? temproom.Element("guest_name").Value : "";
            newRes.bcom_room_remarks = temproom.Element("remarks") != null ? temproom.Element("remarks").Value : "";
            newRes.bcom_note = booknote;

            newRes.bcom_totalForOwner = bcom_totalForOwner;
            newRes.pr_part_agency_fee = 0;
            newRes.prTotalRate = totalprice;
            newRes.prTotalCommission = currEstate.pr_percentage.objToInt32() * bcom_totalForOwner / 100;
            newRes.prTotalOwner = bcom_totalForOwner - newRes.prTotalCommission;
            newRes.pr_reservation = totalprice;
            newRes.pr_part_forPayment = newRes.pr_total;

            //if (outPrice.prDiscountSpecialOffer > 0)
            //    _rntEst.priceNoDiscount = _rntEst.price + outPrice.prDiscountSpecialOffer + outPrice.prDiscountLongStay;
            //_rntEst.pr_agentID = outPrice.agentID;
            //_rntEst.pr_agentCommissionPrice = outPrice.agentCommissionPrice;
            //_rntEst.priceError = outPrice.outError;
            //_rntEst.prTotalCommission = outPrice.prTotalCommission;
            //_rntEst.priceReservation = outPrice.pr_reservation;
            //_rntEst.priceEco = outPrice.ecoPrice;
            //_rntEst.priceSrs = outPrice.srsPrice;
            //_rntEst.priceAgencyFee = outPrice.pr_part_agency_fee;
            //_rntEst.nicePrice = outPrice.prDiscountSpecialOffer > 0;

        }
        void doThread()
        {
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == "bcom");
                if (agentTbl != null)
                    agentID = agentTbl.id;
            }
            if (agentID == 0)
            {
                ErrorLog.addLog("", "BcomImport_process", "Agent for Booking.com was not found or not active");
                return;
            }

            //string postdata = "<request><username>magarentalXML</username><password>magarental1234</password><hotel_id>18349</hotel_id><last_change>2014-01-30 00:00:00</last_change></request>";
            //string postdata = "<request><username>" + username + "</username><password>" + password + "</password><hotel_id>" + hotel_id + "</hotel_id>" + (!string.IsNullOrEmpty(last_change) ? "<last_change>" + last_change + "</last_change>" : "") + "</request>";

            string postdata = "";
            if (CommonUtilities.getSYS_SETTING("bcom_remove_last_update").objToInt32() == 0)
                postdata = "<request><username>" + username + "</username><password>" + password + "</password><hotel_id>" + hotel_id + "</hotel_id>" + (!string.IsNullOrEmpty(last_change) ? "<last_change>" + last_change + "</last_change>" : "") + "</request>";
            else
                postdata = "<request><username>" + username + "</username><password>" + password + "</password><hotel_id>" + hotel_id + "</hotel_id></request>";

            //string filepath1 = Path.Combine(App.SRP, "bf.txt");
            //string responseData = System.IO.File.ReadAllText(filepath1);
            //string responseData = System.IO.File.ReadAllText(@"D:\bf.txt");           
            string responseData = BcomUtils.SendRequest("https://secure-supply-xml.booking.com/hotels/xml/reservations", "getReservations", postdata);
            if (string.IsNullOrEmpty(responseData))
            {
                //ErrorLog.addLog("", "BcomImport_process", "Empty responseData");
                return;
            }
            try
            {
                int countCreated = 0;
                int countUpdated = 0;
                int countCanceled = 0;
                XmlDocument xmlDoc = new XmlDocument();
                using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
                using (var dcOld = maga_DataContext.DC_RENTAL)
                {
                    XDocument _resource = XDocument.Parse(responseData);
                    var reservationList = _resource.Descendants("reservations").Elements("reservation");
                    if (reservationList == null)
                    {
                        //ErrorLog.addLog("", "BcomImport_process", "Empty reservationList");
                        return;
                    }
                    List<int> lstResChangeEstateId = new List<int>();
                    foreach (var reservation in reservationList)
                    {
                        int pid_parent_booking = 0;
                        int cntRoom = 0;
                        string bcomHotelId = reservation.Element("hotel_id").Value.ToString();
                        string status = reservation.Element("status").Value.ToString();
                        string ccData = "";
                        if (status == "new" || status == "modified")
                        {
                            var bcom_resId = reservation.Element("id").Value.ToString();
                            var currIds = dcOld.RNT_TBL_RESERVATION.Where(x => x.bcom_resId == bcom_resId).Select(x => x.id).ToList();

                            var customerList = reservation.Descendants("customer");
                            var customer = customerList.FirstOrDefault();
                            if (customer != null && customer.Element("cc_number") != null && customer.Element("cc_number").Value != "")
                            {
                                //for paymet wih credit card
                                ccData += "Holder: " + (customer.Element("first_name") != null ? customer.Element("first_name").Value : "") + " " + (customer.Element("last_name") != null ? customer.Element("last_name").Value : "");
                                ccData += "\r\n Number: " + (customer.Element("cc_number") != null ? customer.Element("cc_number").Value : "");
                                ccData += "\r\n Type: " + (customer.Element("cc_type") != null ? customer.Element("cc_type").Value : "");
                                ccData += "\r\n CVC: " + (customer.Element("cc_cvc") != null ? customer.Element("cc_cvc").Value : "");
                                ccData += "\r\n Expire: " + (customer.Element("cc_expiration_date") != null ? customer.Element("cc_expiration_date").Value : "");
                            }

                            var room = reservation.Descendants("room");
                            foreach (var temproom in room)
                            {
                                bool isNew = false;
                                bool isDatesChanged = false;

                                var bcomRoomId = temproom.Element("id") != null ? temproom.Element("id").Value : "";
                                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.FirstOrDefault(x => x.bcomHotelId == bcomHotelId && x.bcomRoomId == bcomRoomId && x.bcomEnabled == 1);
                                if (currEstate == null)
                                {
                                    ErrorLog.addLog("", "BcomImport_process", "bcomHotelId:" + bcomHotelId + ", bcomRoomId:" + bcomRoomId + " was not found or not active");
                                    continue;
                                }
                                var bcom_roomResId = temproom.Element("roomreservation_id") != null ? temproom.Element("roomreservation_id").Value : "";
                                RNT_TBL_RESERVATION newRes = dcOld.RNT_TBL_RESERVATION.FirstOrDefault(x => x.bcom_resId == bcom_resId && x.bcom_roomResId == bcom_roomResId);
                                if (newRes == null)
                                {
                                    newRes = rntUtils.newReservation();
                                    newRes.pid_estate = currEstate.id;
                                    newRes.bcom_roomResId = temproom.Element("roomreservation_id").Value.ToString();
                                    newRes.bcom_resId = reservation.Element("id").Value.ToString();
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
                                    countCreated++;
                                    isNew = true;
                                }
                                else
                                {
                                    countUpdated++;
                                    currIds.Remove(newRes.id);
                                    if (newRes.state_pid == 3)
                                    {
                                        newRes.state_pid = 4;
                                        newRes.state_body = "Reopen Cancellation by booking";
                                        newRes.state_date = DateTime.Now;
                                        newRes.state_pid_user = 1;
                                        newRes.bcom_cancel = 0;
                                        isNew = true;
                                    }

                                    DateTime dtStart = temproom.Element("arrival_date") != null ? temproom.Element("arrival_date").Value.ToString().Replace("-", "").JSCal_stringToDate() : DateTime.Now;
                                    DateTime dtEnd = temproom.Element("departure_date") != null ? temproom.Element("departure_date").Value.ToString().Replace("-", "").JSCal_stringToDate() : DateTime.Now.AddDays(5);

                                    if (dtStart != newRes.dtStart || dtEnd != newRes.dtEnd)
                                        isDatesChanged = true;
                                }
                                if (newRes.pid_estate.objToInt32() > 0 && newRes.pid_estate.objToInt32() != currEstate.id) continue;
                                newRes.code = newRes.id.ToString().fillString("0", 7, false);
                                //make the file for credit card information
                                string filePath = Path.Combine(App.SRP, "admin/dati_cc");
                                if (!Directory.Exists(filePath))
                                    Directory.CreateDirectory(filePath);
                                filePath = Path.Combine(filePath, newRes.unique_id + ".txt");
                                if (!File.Exists(filePath) || ccData != "")
                                {
                                    StreamWriter ccWriter = new StreamWriter(filePath, false);
                                    ccWriter.WriteLine(RijndaelSimple_2.Encrypt(ccData)); // Write the file.
                                    ccWriter.Flush();
                                    ccWriter.Close(); // Close the instance of StreamWriter.
                                    ccWriter.Dispose(); // Dispose from memory.
                                }
                                newRes.agentID = agentID;
                                newRes.pidEstateCity = currEstate.pid_city;
                                newRes.bcom_status = status;
                                dcOld.SubmitChanges();

                                if (newRes.pid_estate.objToInt32() > 0 && newRes.pid_estate.objToInt32() == currEstate.id)
                                {
                                    FillReservationData(temproom, currEstate, ref newRes, customer);
                                    newRes.agentClientID = GetAgentClientId(customer, newRes.agentClientID.objToInt64());
                                }
                                dcOld.SubmitChanges();

                                if (isNew || isDatesChanged)
                                {
                                    rntUtils.rntReservation_onChange(newRes, false, false,true, true);
                                    lstResChangeEstateId.Add(newRes.pid_estate.objToInt32());
                                }
                                else
                                    rntUtils.rntReservation_onChange(newRes, false, false,true, false);


                                if (cntRoom == 0)
                                    rntUtils.reservation_checkPartPayment(newRes, true);

                                if (cntRoom == 0)
                                    pid_parent_booking = newRes.id.objToInt32();
                                newRes.bcom_pid_parent_booking = pid_parent_booking;
                                dcOld.SubmitChanges();

                                var overBookingRes = rntUtils.rntEstate_isAvailable(currEstate.id, newRes.dtStart.Value, newRes.dtEnd.Value, newRes.id);
                                if (overBookingRes != null && newRes.pid_estate.objToInt32() > 0 && newRes.pid_estate.objToInt32() == currEstate.id && (isNew || isDatesChanged))
                                {
                                    string mSubject = "Attenzione - Prenotazione in OverBooking rif #" + newRes.code;
                                    string mBody = "Abbiamo importato la pren #" + newRes.code + " che risulta in overbooking con #" + overBookingRes.code + ".<br/>Si prega di verificare il motivo e conttare assistenza.";
                                    MailingUtilities.autoSendMailTo(mSubject, mBody, (agentTbl.contactEmail.isEmail() ? agentTbl.contactEmail : "info@rentalinrome.com"), true, "BcomImport_process overBookingRes");
                                }
                                if (isNew && (CommonUtilities.getSYS_SETTING("rntBcom_sendEmails") == "true" || CommonUtilities.getSYS_SETTING("rntBcom_sendEmails").ToInt32() == 1))
                                {
                                    rntUtils.rntReservation_mailPartPaymentReceive(newRes, false, true, true, true, true, 1); // send mails
                                }
                                cntRoom++;
                                /*
                                var addons = temproom.Element("addons");
                                var addon = element.Descendants("addon");
                                foreach (var objaddon in addon)
                                {
                                    RNT_Booking_Addons currAddons = new RNT_Booking_Addons();
                                    currAddons.totalprice = objaddon.Element("totalprice").Value.objToDecimal();
                                    currAddons.person = objaddon.Element("persons").Value.objToInt32();
                                    currAddons.price_mode = objaddon.Element("price_mode").Value;
                                    currAddons.type = objaddon.Element("type").Value.objToInt32();
                                    currAddons.price_per_unit = objaddon.Element("price_per_unit").Value.objToDecimal();
                                    currAddons.nights = objaddon.Element("nights").Value.objToInt32();
                                    currAddons.name = objaddon.Element("name").Value;
                                    currAddons.pidReservation = newRes.id;
                                    dcOld.RNT_Booking_Addons.InsertOnSubmit(currAddons);
                                    dcOld.SubmitChanges();
                                }
                                var price = temproom.Descendants("price");
                                foreach (var objPrice in price)
                                {
                                    RNT_Booking_Price currPrice = new RNT_Booking_Price();
                                    currPrice.date = objPrice.Attribute("date").Value.ToString().Replace("-", "").JSCal_stringToDate();
                                    currPrice.price = objPrice.Value.objToDecimal();
                                    currPrice.pidReservation = newRes.id;
                                    currPrice.rate_id = objPrice.Attribute("rate_id").Value;
                                    dcOld.RNT_Booking_Price.InsertOnSubmit(currPrice);
                                    dcOld.SubmitChanges();

                                }
                                */
                            }
                            var lstReservations = dcOld.RNT_TBL_RESERVATION.Where(x => currIds.Contains(x.id)).ToList();
                            foreach (RNT_TBL_RESERVATION objCancelRes in lstReservations)
                            {
                                if (objCancelRes.state_pid != 3)
                                {
                                    rntUtils.rntReservation_onStateChange(objCancelRes);
                                    objCancelRes.state_pid = 3;
                                    objCancelRes.bcom_status = status;
                                    objCancelRes.state_body = "Cancellato automaticamente dal sistema";
                                    objCancelRes.state_date = DateTime.Now;
                                    objCancelRes.state_pid_user = 1;
                                    objCancelRes.state_subject = "CAN";
                                    objCancelRes.bcom_cancel = 1;
                                    var cancelcharge = reservation.Element("total_cancellation_fee");
                                    if (cancelcharge != null)
                                    {
                                        objCancelRes.bcom_cancelcharge = reservation.Element("total_cancellation_fee").Value.ToString().objToDecimal();
                                    }
                                    countCanceled++;
                                    dcOld.SubmitChanges();
                                    rntUtils.rntReservation_onChange(objCancelRes, false, false, false, true);
                                    lstResChangeEstateId.Add(objCancelRes.pid_estate.objToInt32());
                                    if ((CommonUtilities.getSYS_SETTING("rntBcom_sendEmailsCancelled") == "true" || CommonUtilities.getSYS_SETTING("rntBcom_sendEmailsCancelled").ToInt32() == 1))
                                    {
                                        rntUtils.rntReservation_mailCancelled(objCancelRes, false, true, true, true, true, 1); // send mails
                                    }
                                }
                            }
                        }
                        else if (status == "cancelled")
                        {
                            var bcom_resId = reservation.Element("id").Value.ToString();
                            var lstReservations = dcOld.RNT_TBL_RESERVATION.Where(x => x.bcom_resId == bcom_resId).ToList();
                            foreach (RNT_TBL_RESERVATION objCancelRes in lstReservations)
                            {
                                if (objCancelRes.state_pid != 3)
                                {
                                    rntUtils.rntReservation_onStateChange(objCancelRes);
                                    objCancelRes.state_pid = 3;
                                    objCancelRes.bcom_status = status;
                                    objCancelRes.state_body = "Cancellato automaticamente dal sistema";
                                    objCancelRes.state_date = DateTime.Now;
                                    objCancelRes.state_pid_user = 1;
                                    objCancelRes.state_subject = "CAN";
                                    objCancelRes.bcom_cancel = 1;
                                    var cancelcharge = reservation.Element("total_cancellation_fee");
                                    if (cancelcharge != null)
                                    {
                                        objCancelRes.bcom_cancelcharge = reservation.Element("total_cancellation_fee").Value.ToString().objToDecimal();
                                    }
                                    countCanceled++;
                                    dcOld.SubmitChanges();
                                    rntUtils.rntReservation_onChange(objCancelRes, false, false, false, true);
                                    lstResChangeEstateId.Add(objCancelRes.pid_estate.objToInt32());
                                    if ((CommonUtilities.getSYS_SETTING("rntBcom_sendEmailsCancelled") == "true" || CommonUtilities.getSYS_SETTING("rntBcom_sendEmailsCancelled").ToInt32() == 1))
                                    {
                                        rntUtils.rntReservation_mailCancelled(objCancelRes, false, true, true, true, true, 1); // send mails
                                    }
                                }
                            }

                        }
                    }
                    if (lstResChangeEstateId != null && lstResChangeEstateId.Count > 0)
                        rntUtilsChnlAll.updateExpediaHotels(lstResChangeEstateId);

                    if (countCreated > 0 || countUpdated > 0 || countCanceled > 0)
                        BcomUtils.addLog("", "", "IMPORTED countCreated:" + countCreated + ", countUpdated:" + countUpdated + ", countCanceled:" + countCanceled + "", "");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "BcomImport_process", ex.ToString());
            }
        }
        public BcomImport_process(string Username, string Password, string HotelId, string LastChange)
        {
            username = Username;
            password = Password;
            hotel_id = HotelId;
            last_change = LastChange;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskSchedulerPriority.AddTask(action, "BcomUpdate.BcomImport HotelId:" + HotelId + " LastChange:" + LastChange);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Highest;
            t.Start();

        }
    }
    public static void BcomImport_start(string Username, string Password, string HotelId, string LastChange)
    {
        BcomImport_process _tmp = new BcomImport_process(Username, Password, HotelId, LastChange);
    }
    public static void BcomImport_all(string LastChange)
    {
        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
        {
            var tmpList = dc.RntBcomHotelTBL.Where(x => x.isActive == 1).ToList();
            foreach (var tmp in tmpList)
            {
                if (!string.IsNullOrEmpty(tmp.username) && !string.IsNullOrEmpty(tmp.password) && !string.IsNullOrEmpty(tmp.hotelId))
                    BcomImport_start(tmp.username, tmp.password, tmp.hotelId, LastChange);
            }
        }
    }

}
public class BcomUtils
{
    public static void SetTimers()
    {
        timerBcomImport = new System.Timers.Timer();
        timerBcomImport.Interval = (1000 * 60 * 2); // first after 2 mins
        timerBcomImport.Elapsed += new System.Timers.ElapsedEventHandler(timerBcomImport_Elapsed);
        timerBcomImport.Start();
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
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                dc.RntBcomRequestLOG.DeleteAllOnSubmit(dc.RntBcomRequestLOG.Where(x => x.logDateTime <= dt));
                dc.SubmitChanges();
            }
            addLog("", "CLEAR LOG: till " + dt, "", "");
        }
        public ClearLog_process()
        {
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "BcomUtils.ClearLog_process");

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

    static System.Timers.Timer timerBcomImport;
    static void timerBcomImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        if (RentalInRome.Global.isOnDev) return;
        int rntBcomImportEachMins = CommonUtilities.getSYS_SETTING("rntBcomImportEachMins").ToInt32();
        if (rntBcomImportEachMins == 0) rntBcomImportEachMins = 15;
        int bcom_import_days = CommonUtilities.getSYS_SETTING("bcom_import_days").objToInt32();
        if (bcom_import_days == 0) bcom_import_days = 1;
        BcomImport.BcomImport_all(DateTime.Now.AddDays(-bcom_import_days).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----"));
        timerBcomImport.Dispose();
        timerBcomImport = new System.Timers.Timer();
        timerBcomImport.Interval = (1000 * 60 * rntBcomImportEachMins); // each 15 mins
        timerBcomImport.Elapsed += new System.Timers.ElapsedEventHandler(timerBcomImport_Elapsed);
        timerBcomImport.Start();
    }
    public static void addLog(string requesUrl, string requestType, string requestContent, string responseContent)
    {
        try
        {
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                var item = new RntBcomRequestLOG();
                item.uid = Guid.NewGuid();
                item.requesUrl = requesUrl;
                item.requestType = requestType;
                item.requestContent = requestContent;
                item.responseContent = responseContent;
                item.logDateTime = DateTime.Now;
                dc.RntBcomRequestLOG.InsertOnSubmit(item);
                dc.SubmitChanges();
            }
        }
        catch (Exception Ex)
        {
            ErrorLog.addLog("", "BcomUtils.addLog", Ex.ToString() + "");
        }
    }

    public static string SendRequest(String requesUrl, String requestType, String requestContent)
    {
        try
        {
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);
            obj.ContentType = "text/xml";
            obj.Method = "POST";
            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
}
public class BcomUpdate
{
    private class BcomUpdate_process
    {
        public int IdEstate { get; set; }
        private DateTime? DtStart { get; set; }
        private DateTime? DtEnd { get; set; }
        public string Type { get; set; }
        public string URL { get { return "https://supply-xml.booking.com/hotels/xml/availability"; } }
        public string username { get; set; }
        public string password { get; set; }
        public string hotel_id { get; set; }
        void SendRequest(string request)
        {
            string postdata = "<request><username>" + username + "</username><password>" + password + "</password><hotel_id>" + hotel_id + "</hotel_id>" + request + "</request>";
            BcomUtils.SendRequest(URL, Type + " - idEstate:" + IdEstate, postdata);
        }
        void doThread()
        {
            try
            {
                long agentID = 0;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var agentTbl = dc.RntAgentTBL.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == "bcom");
                    if (agentTbl != null)
                        agentID = agentTbl.id;
                }
                if (agentID == 0)
                {
                    ErrorLog.addLog("", "BcomUpdate_process", "Agent for Booking.com was not found or not active");
                    return;
                }

                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1 && x.bcomEnabled == 1 && x.bcomHotelId != null && x.bcomHotelId != "" && x.bcomRoomId != null && x.bcomRoomId != "");
                if (currEstate == null)
                {
                    //ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " not found or not active");
                    return;
                }
                hotel_id = currEstate.bcomHotelId;
                RntBcomHotelTBL bcomHotel;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    bcomHotel = dc.RntBcomHotelTBL.SingleOrDefault(x => x.hotelId == hotel_id && x.isActive == 1);
                    if (bcomHotel == null)
                    {
                        ErrorLog.addLog("", "BcomUpdate_process", "hotelId:" + hotel_id + " for idEstate:" + IdEstate + " not found or not active");
                        return;
                    }
                }
                if (string.IsNullOrEmpty(bcomHotel.username) || string.IsNullOrEmpty(bcomHotel.password))
                {
                    ErrorLog.addLog("", "BcomUpdate_process", "hotelId:" + hotel_id + " has no username or password");
                    return;
                }
                if (string.IsNullOrEmpty(bcomHotel.rateIdStandard) && string.IsNullOrEmpty(bcomHotel.rateIdNotRefund) && string.IsNullOrEmpty(bcomHotel.rateIdSpecial))
                {
                    ErrorLog.addLog("", "BcomUpdate_process", "hotelId:" + hotel_id + " has no rates");
                    return;
                }
                username = bcomHotel.username;
                password = bcomHotel.password;
                var dtStart = DtStart.HasValue ? DtStart.Value : DateTime.Now.Date;
                var dtEnd = DtEnd.HasValue ? DtEnd.Value : DateTime.Now.AddYears(2).Date;

                if (Type == "availability")
                {
                    var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                    var list = new List<rntExts.AvvListPerDates>();
                    DateTime dtCurrent = dtStart;
                    using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                    {
                        var closedList = dc.RntBcomEstateClosedDatesRL.Where(x => x.pidEstate == IdEstate && x.isClosed == 1 && x.changeDate >= dtStart).ToList();
                        while (dtCurrent < dtEnd)
                        {
                            var tmpDate = closedList.FirstOrDefault(x => x.changeDate == dtCurrent);
                            bool isAvv = tmpDate != null ? false : resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count() == 0;
                            var lastDatePrice = list.LastOrDefault();
                            if (lastDatePrice == null || lastDatePrice.IsAvv != isAvv) list.Add(new rntExts.AvvListPerDates(dtCurrent, dtCurrent, isAvv));
                            else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
                            dtCurrent = dtCurrent.AddDays(1);
                        }
                    }
                    string tmpStr = "";
                    tmpStr += "<room id='" + currEstate.bcomRoomId + "'>";
                    foreach (var tmp in list)
                    {
                        if (!string.IsNullOrEmpty(bcomHotel.rateIdStandard))
                        {
                            tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                            tmpStr += "<rate id='" + bcomHotel.rateIdStandard + "'></rate>";
                            tmpStr += "<closed>" + (tmp.IsAvv ? "0" : "1") + "</closed>";
                            tmpStr += "<roomstosell>" + (tmp.IsAvv ? "1" : "0") + "</roomstosell>";
                            tmpStr += "</date>";
                        }
                        if (!string.IsNullOrEmpty(bcomHotel.rateIdNotRefund))
                        {
                            tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                            tmpStr += "<rate id='" + bcomHotel.rateIdNotRefund + "'></rate>";
                            tmpStr += "<closed>" + (tmp.IsAvv ? "0" : "1") + "</closed>";
                            tmpStr += "<roomstosell>" + (tmp.IsAvv ? "1" : "0") + "</roomstosell>";
                            tmpStr += "</date>";
                        }
                        if (!string.IsNullOrEmpty(bcomHotel.rateIdSpecial))
                        {
                            tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                            tmpStr += "<rate id='" + bcomHotel.rateIdSpecial + "'></rate>";
                            tmpStr += "<closed>" + (tmp.IsAvv ? "0" : "1") + "</closed>";
                            tmpStr += "<roomstosell>" + (tmp.IsAvv ? "1" : "0") + "</roomstosell>";
                            tmpStr += "</date>";
                        }
                    }
                    tmpStr += "</room>";
                    SendRequest(tmpStr);
                }
                if (Type == "rates")
                {
                    int outError;
                    var priceListPerDates = rntUtils.estate_getPriceListPerDates_Bcom(IdEstate, agentID, dtStart, dtEnd, out outError); //Created new function for bcom without special offer
                    if (priceListPerDates == null || priceListPerDates.Count == 0)
                    {
                        ErrorLog.addLog("", "BcomUpdate_process", "idEstate:" + IdEstate + " has no prices");
                        return;
                    }

                    using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                    {
                        var minStayList = dc.RntBcomEstateClosedDatesRL.Where(x => x.pidEstate == IdEstate && x.minStay > 0 && x.changeDate >= dtStart).ToList();
                        var changesList = dc.RntBcomEstateRateChangesRL.Where(x => x.pidEstate == IdEstate && x.changeIsDiscount >= 0 && x.changeDate >= dtStart).ToList();
                        var list = new List<BcomRatesPerDates>();
                        foreach (var tmp in priceListPerDates)
                        {
                            var priceSingle = tmp.Prices[currEstate.pr_basePersons.objToInt32()];
                            var priceFull = tmp.Prices[currEstate.num_persons_max.objToInt32()];
                            var tmpRate = new BcomRatesPerDates(tmp.DtStart, tmp.DtEnd, tmp.MinStay);


                            decimal currPrice = 0;
                            decimal changeAmount = 0;

                            tmpRate.StandardPricesSingle = getRatePrice(bcomHotel.rateIdStandard, priceSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeStandard);
                            tmpRate.StandardPricesFull = getRatePrice(bcomHotel.rateIdStandard, priceFull, BcomProps.PriceTypeFull, BcomProps.RateTypeStandard);

                            tmpRate.Standard2PricesSingle = getRatePrice(bcomHotel.rateIdStandard2, priceSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeStandard2);
                            tmpRate.Standard2PricesFull = getRatePrice(bcomHotel.rateIdStandard2, priceFull, BcomProps.PriceTypeFull, BcomProps.RateTypeStandard2);

                            tmpRate.Standard3PricesSingle = getRatePrice(bcomHotel.rateIdStandard3, priceSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeStandard3);
                            tmpRate.Standard3PricesFull = getRatePrice(bcomHotel.rateIdStandard3, priceFull, BcomProps.PriceTypeFull, BcomProps.RateTypeStandard3);

                            tmpRate.Standard4PricesSingle = getRatePrice(bcomHotel.rateIdStandard4, priceSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeStandard4);
                            tmpRate.Standard4PricesFull = getRatePrice(bcomHotel.rateIdStandard4, priceFull, BcomProps.PriceTypeFull, BcomProps.RateTypeStandard4);

                            tmpRate.NotRefundPricesSingle = getRatePrice(bcomHotel.rateIdNotRefund, priceSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeNotRefund);
                            tmpRate.NotRefundPricesFull = getRatePrice(bcomHotel.rateIdNotRefund, priceFull, BcomProps.PriceTypeFull, BcomProps.RateTypeNotRefund);
                            tmpRate.SpecialPricesSingle = getRatePrice(bcomHotel.rateIdSpecial, priceSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeSpecial);
                            tmpRate.SpecialPricesFull = getRatePrice(bcomHotel.rateIdSpecial, priceFull, BcomProps.PriceTypeFull, BcomProps.RateTypeSpecial);

                            DateTime dtCurrent = tmp.DtStart;
                            while (dtCurrent <= tmp.DtEnd)
                            {
                                var datePrices = new BcomRatesPerDates(dtCurrent, dtCurrent, tmpRate);

                                var minStayDate = minStayList.FirstOrDefault(x => x.changeDate == dtCurrent);
                                if (minStayDate != null) datePrices.MinStay = minStayDate.minStay;

                                datePrices.StandardPricesSingle = getRateChange(bcomHotel.rateIdStandard, priceSingle, datePrices.StandardPricesSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeStandard, dtCurrent);
                                datePrices.StandardPricesFull = getRateChange(bcomHotel.rateIdStandard, priceFull, datePrices.StandardPricesFull, BcomProps.PriceTypeFull, BcomProps.RateTypeStandard, dtCurrent);

                                datePrices.Standard2PricesSingle = getRateChange(bcomHotel.rateIdStandard2, priceSingle, datePrices.Standard2PricesSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeStandard2, dtCurrent);
                                datePrices.Standard2PricesFull = getRateChange(bcomHotel.rateIdStandard2, priceFull, datePrices.Standard2PricesFull, BcomProps.PriceTypeFull, BcomProps.RateTypeStandard2, dtCurrent);

                                datePrices.Standard3PricesSingle = getRateChange(bcomHotel.rateIdStandard3, priceSingle, datePrices.Standard3PricesSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeStandard3, dtCurrent);
                                datePrices.Standard3PricesFull = getRateChange(bcomHotel.rateIdStandard3, priceFull, datePrices.Standard3PricesFull, BcomProps.PriceTypeFull, BcomProps.RateTypeStandard3, dtCurrent);

                                datePrices.Standard4PricesSingle = getRateChange(bcomHotel.rateIdStandard4, priceSingle, datePrices.Standard4PricesSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeStandard4, dtCurrent);
                                datePrices.Standard4PricesFull = getRateChange(bcomHotel.rateIdStandard4, priceFull, datePrices.Standard4PricesFull, BcomProps.PriceTypeFull, BcomProps.RateTypeStandard4, dtCurrent);

                                datePrices.NotRefundPricesSingle = getRateChange(bcomHotel.rateIdNotRefund, priceSingle, datePrices.NotRefundPricesSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeNotRefund, dtCurrent);
                                datePrices.NotRefundPricesFull = getRateChange(bcomHotel.rateIdNotRefund, priceFull, datePrices.NotRefundPricesFull, BcomProps.PriceTypeFull, BcomProps.RateTypeNotRefund, dtCurrent);
                                datePrices.SpecialPricesSingle = getRateChange(bcomHotel.rateIdSpecial, priceSingle, datePrices.SpecialPricesSingle, BcomProps.PriceTypeSingle, BcomProps.RateTypeSpecial, dtCurrent);
                                datePrices.SpecialPricesFull = getRateChange(bcomHotel.rateIdSpecial, priceFull, datePrices.SpecialPricesFull, BcomProps.PriceTypeFull, BcomProps.RateTypeSpecial, dtCurrent);

                                var lastDatePrice = list.LastOrDefault();
                                if (lastDatePrice == null || !lastDatePrice.HasSamePrices(datePrices)) list.Add(datePrices);
                                else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
                                dtCurrent = dtCurrent.AddDays(1);
                            }
                        }
                        string tmpStr = "";
                        tmpStr += "<room id='" + currEstate.bcomRoomId + "'>";
                        foreach (var tmp in list)
                        {
                            if (tmp.StandardPricesSingle > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdStandard + "'></rate>";
                                tmpStr += "<price1>" + tmp.StandardPricesSingle + "</price1>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }
                            if (tmp.StandardPricesFull > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdStandard + "'></rate>";
                                tmpStr += "<price>" + tmp.StandardPricesFull + "</price>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }

                            #region New Standard Rates
                            if (tmp.Standard2PricesSingle > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdStandard2 + "'></rate>";
                                tmpStr += "<price1>" + tmp.Standard2PricesSingle + "</price1>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }
                            if (tmp.Standard2PricesFull > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdStandard2 + "'></rate>";
                                tmpStr += "<price>" + tmp.Standard2PricesFull + "</price>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }

                            if (tmp.Standard3PricesSingle > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdStandard3 + "'></rate>";
                                tmpStr += "<price1>" + tmp.Standard3PricesSingle + "</price1>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }
                            if (tmp.Standard3PricesFull > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdStandard3 + "'></rate>";
                                tmpStr += "<price>" + tmp.Standard3PricesFull + "</price>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }

                            if (tmp.Standard4PricesSingle > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdStandard4 + "'></rate>";
                                tmpStr += "<price1>" + tmp.Standard4PricesSingle + "</price1>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }
                            if (tmp.Standard4PricesFull > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdStandard4 + "'></rate>";
                                tmpStr += "<price>" + tmp.Standard4PricesFull + "</price>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }
                            #endregion

                            if (tmp.NotRefundPricesSingle > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdNotRefund + "'></rate>";
                                tmpStr += "<price1>" + tmp.NotRefundPricesSingle + "</price1>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }
                            if (tmp.NotRefundPricesFull > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdNotRefund + "'></rate>";
                                tmpStr += "<price>" + tmp.NotRefundPricesFull + "</price>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }

                            if (tmp.SpecialPricesSingle > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdSpecial + "'></rate>";
                                tmpStr += "<price1>" + tmp.SpecialPricesSingle + "</price1>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }
                            if (tmp.SpecialPricesFull > 0)
                            {
                                tmpStr += "<date from='" + tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "' to='" + tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----") + "'>";
                                tmpStr += "<rate id='" + bcomHotel.rateIdSpecial + "'></rate>";
                                tmpStr += "<price>" + tmp.SpecialPricesFull + "</price>";
                                tmpStr += "<minimumstay>" + tmp.MinStay + "</minimumstay>";
                                tmpStr += "</date>";
                            }
                        }
                        tmpStr += "</room>";
                        SendRequest(tmpStr);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "BcomUpdate_process IdEstate:" + IdEstate + " type:" + Type, ex.ToString());
            }
        }
        List<RntBcomEstateRatesRL> ratesList;
        decimal getRatePrice(string rateId, decimal originalPrice, int priceType, int rateType)
        {
            if (string.IsNullOrEmpty(rateId)) return 0;
            if (ratesList == null)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                    ratesList = dc.RntBcomEstateRatesRL.Where(x => x.pidEstate == IdEstate).ToList();
            var tmpRate = ratesList.SingleOrDefault(x => x.pidEstate == IdEstate && x.priceType == priceType && x.rateType == rateType);
            if (tmpRate == null) return 0;
            var currPrice = 0 + originalPrice;
            if (tmpRate.changeAmount.objToInt32() > 0)
            {
                var changeAmount = (tmpRate.changeIsPercentage == 1) ? (currPrice * tmpRate.changeAmount.objToInt32() / 100) : tmpRate.changeAmount.objToInt32();
                if (tmpRate.changeIsDiscount == 1) { changeAmount = -changeAmount; }
                currPrice = currPrice + changeAmount;
            }
            return currPrice;
        }
        List<RntBcomEstateRateChangesRL> rateChangesList;
        decimal getRateChange(string rateId, decimal originalPrice, decimal ratePrice, int priceType, int rateType, DateTime changeDate)
        {
            if (string.IsNullOrEmpty(rateId)) return 0;
            if (rateChangesList == null)
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                    rateChangesList = dc.RntBcomEstateRateChangesRL.Where(x => x.pidEstate == IdEstate && x.changeIsDiscount >= 0).ToList();

            var tmpRate = rateChangesList.SingleOrDefault(x => x.changeDate == changeDate && x.priceType == priceType && x.rateType == rateType);
            if (tmpRate == null) return ratePrice;
            var currPrice = 0 + originalPrice;
            if (tmpRate.changeAmount.objToInt32() > 0)
            {
                var changeAmount = (tmpRate.changeIsPercentage == 1) ? (currPrice * tmpRate.changeAmount.objToInt32() / 100) : tmpRate.changeAmount.objToInt32();
                if (tmpRate.changeIsDiscount == 1) { changeAmount = -changeAmount; }
                currPrice = currPrice + changeAmount;
            }
            return currPrice;

        }
        public BcomUpdate_process(int idEstate, string type)
        {
            IdEstate = idEstate;
            DtStart = (DateTime?)null;
            DtEnd = (DateTime?)null;
            Type = type;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "BcomUpdate.BcomUpdate_process idEstate:" + idEstate + " type:" + type);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
        public BcomUpdate_process(int idEstate, string type, DateTime dtStart, DateTime dtEnd)
        {
            IdEstate = idEstate;
            DtStart = dtStart.Date;
            DtEnd = dtEnd.Date;
            Type = type;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "BcomUpdate.BcomUpdate_process idEstate:" + idEstate + " type:" + type);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public class BcomRatesPerDates
    {
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public int MinStay { get; set; }

        public decimal StandardPricesSingle { get; set; }
        public decimal StandardPricesFull { get; set; }

        public decimal Standard2PricesSingle { get; set; }
        public decimal Standard2PricesFull { get; set; }

        public decimal Standard3PricesSingle { get; set; }
        public decimal Standard3PricesFull { get; set; }

        public decimal Standard4PricesSingle { get; set; }
        public decimal Standard4PricesFull { get; set; }

        public decimal NotRefundPricesSingle { get; set; }
        public decimal NotRefundPricesFull { get; set; }

        public decimal SpecialPricesSingle { get; set; }
        public decimal SpecialPricesFull { get; set; }

        public BcomRatesPerDates(DateTime dtStart, DateTime dtEnd, int minStay)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            MinStay = minStay;

            StandardPricesSingle = 0;
            StandardPricesFull = 0;

            Standard2PricesSingle = 0;
            Standard2PricesFull = 0;

            Standard3PricesSingle = 0;
            Standard3PricesFull = 0;

            Standard4PricesSingle = 0;
            Standard4PricesFull = 0;

            NotRefundPricesSingle = 0;
            NotRefundPricesFull = 0;

            SpecialPricesSingle = 0;
            SpecialPricesFull = 0;
        }
        public BcomRatesPerDates(DateTime dtStart, DateTime dtEnd, BcomRatesPerDates copyFrom)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            MinStay = copyFrom.MinStay;

            StandardPricesSingle = copyFrom.StandardPricesSingle;
            StandardPricesFull = copyFrom.StandardPricesFull;

            Standard2PricesSingle = copyFrom.Standard2PricesSingle;
            Standard2PricesFull = copyFrom.Standard2PricesFull;

            Standard3PricesSingle = copyFrom.Standard3PricesSingle;
            Standard3PricesFull = copyFrom.Standard3PricesFull;

            Standard4PricesSingle = copyFrom.Standard4PricesSingle;
            Standard4PricesFull = copyFrom.Standard4PricesFull;

            NotRefundPricesSingle = copyFrom.NotRefundPricesSingle;
            NotRefundPricesFull = copyFrom.NotRefundPricesFull;

            SpecialPricesSingle = copyFrom.SpecialPricesSingle;
            SpecialPricesFull = copyFrom.SpecialPricesFull;
        }
        public bool HasSamePrices(BcomRatesPerDates compareWith)
        {
            return MinStay == compareWith.MinStay &&
                StandardPricesSingle == compareWith.StandardPricesSingle &&
                StandardPricesFull == compareWith.StandardPricesFull &&

                Standard2PricesSingle == compareWith.Standard2PricesSingle &&
                Standard2PricesFull == compareWith.Standard2PricesFull &&

                Standard3PricesSingle == compareWith.Standard3PricesSingle &&
                Standard3PricesFull == compareWith.Standard3PricesFull &&

                Standard4PricesSingle == compareWith.Standard4PricesSingle &&
                Standard4PricesFull == compareWith.Standard4PricesFull &&

                NotRefundPricesSingle == compareWith.NotRefundPricesSingle &&
                NotRefundPricesFull == compareWith.NotRefundPricesFull &&
                SpecialPricesSingle == compareWith.SpecialPricesSingle &&
                SpecialPricesFull == compareWith.SpecialPricesFull
                ;
        }
    }
    public class BcomRates
    {
        public DateTime Dt { get; set; }

        public decimal StandardSingle_changeAmount { get; set; }
        public int StandardSingle_changeIsDiscount { get; set; }
        public int StandardSingle_changeIsPercentage { get; set; }

        public decimal StandardFull_changeAmount { get; set; }
        public int StandardFull_changeIsDiscount { get; set; }
        public int StandardFull_changeIsPercentage { get; set; }

        #region New Standard Rates
        public decimal Standard2Single_changeAmount { get; set; }
        public int Standard2Single_changeIsDiscount { get; set; }
        public int Standard2Single_changeIsPercentage { get; set; }

        public decimal Standard2Full_changeAmount { get; set; }
        public int Standard2Full_changeIsDiscount { get; set; }
        public int Standard2Full_changeIsPercentage { get; set; }

        public decimal Standard3Single_changeAmount { get; set; }
        public int Standard3Single_changeIsDiscount { get; set; }
        public int Standard3Single_changeIsPercentage { get; set; }

        public decimal Standard3Full_changeAmount { get; set; }
        public int Standard3Full_changeIsDiscount { get; set; }
        public int Standard3Full_changeIsPercentage { get; set; }

        public decimal Standard4Single_changeAmount { get; set; }
        public int Standard4Single_changeIsDiscount { get; set; }
        public int Standard4Single_changeIsPercentage { get; set; }

        public decimal Standard4Full_changeAmount { get; set; }
        public int Standard4Full_changeIsDiscount { get; set; }
        public int Standard4Full_changeIsPercentage { get; set; }
        #endregion

        public decimal NotRefundSingle_changeAmount { get; set; }
        public int NotRefundSingle_changeIsDiscount { get; set; }
        public int NotRefundSingle_changeIsPercentage { get; set; }

        public decimal NotRefundFull_changeAmount { get; set; }
        public int NotRefundFull_changeIsDiscount { get; set; }
        public int NotRefundFull_changeIsPercentage { get; set; }

        public decimal SpecialSingle_changeAmount { get; set; }
        public int SpecialSingle_changeIsDiscount { get; set; }
        public int SpecialSingle_changeIsPercentage { get; set; }

        public decimal SpecialFull_changeAmount { get; set; }
        public int SpecialFull_changeIsDiscount { get; set; }
        public int SpecialFull_changeIsPercentage { get; set; }

        public BcomRates(DateTime dt)
        {
            Dt = dt;

            StandardSingle_changeAmount = 0;
            StandardSingle_changeIsDiscount = 0;
            StandardSingle_changeIsPercentage = 0;

            StandardFull_changeAmount = 0;
            StandardFull_changeIsDiscount = 0;
            StandardFull_changeIsPercentage = 0;

            #region New Standard Rates
            Standard2Single_changeAmount = 0;
            Standard2Single_changeIsDiscount = 0;
            Standard2Single_changeIsPercentage = 0;

            Standard2Full_changeAmount = 0;
            Standard2Full_changeIsDiscount = 0;
            Standard2Full_changeIsPercentage = 0;

            Standard3Single_changeAmount = 0;
            Standard3Single_changeIsDiscount = 0;
            Standard3Single_changeIsPercentage = 0;

            Standard3Full_changeAmount = 0;
            Standard3Full_changeIsDiscount = 0;
            Standard3Full_changeIsPercentage = 0;

            Standard4Single_changeAmount = 0;
            Standard4Single_changeIsDiscount = 0;
            Standard4Single_changeIsPercentage = 0;

            Standard4Full_changeAmount = 0;
            Standard4Full_changeIsDiscount = 0;
            Standard4Full_changeIsPercentage = 0;
            #endregion

            NotRefundSingle_changeAmount = 0;
            NotRefundSingle_changeIsDiscount = 0;
            NotRefundSingle_changeIsPercentage = 0;

            NotRefundFull_changeAmount = 0;
            NotRefundFull_changeIsDiscount = 0;
            NotRefundFull_changeIsPercentage = 0;

            SpecialSingle_changeAmount = 0;
            SpecialSingle_changeIsDiscount = 0;
            SpecialSingle_changeIsPercentage = 0;

            SpecialFull_changeAmount = 0;
            SpecialFull_changeIsDiscount = 0;
            SpecialFull_changeIsPercentage = 0;
        }
        public BcomRates(DateTime dt, BcomRates copyFrom)
        {
            Dt = dt;

            StandardSingle_changeAmount = copyFrom.StandardSingle_changeAmount;
            StandardSingle_changeIsDiscount = copyFrom.StandardSingle_changeIsDiscount;
            StandardSingle_changeIsPercentage = copyFrom.StandardSingle_changeIsPercentage;

            StandardFull_changeAmount = copyFrom.StandardFull_changeAmount;
            StandardFull_changeIsDiscount = copyFrom.StandardFull_changeIsDiscount;
            StandardFull_changeIsPercentage = copyFrom.StandardFull_changeIsPercentage;

            #region New Standard Rates
            Standard2Single_changeAmount = copyFrom.Standard2Single_changeAmount;
            Standard2Single_changeIsDiscount = copyFrom.Standard2Single_changeIsDiscount;
            Standard2Single_changeIsPercentage = copyFrom.Standard2Single_changeIsPercentage;

            Standard2Full_changeAmount = copyFrom.Standard2Full_changeAmount;
            Standard2Full_changeIsDiscount = copyFrom.Standard2Full_changeIsDiscount;
            Standard2Full_changeIsPercentage = copyFrom.Standard2Full_changeIsPercentage;

            Standard3Single_changeAmount = copyFrom.Standard3Single_changeAmount;
            Standard3Single_changeIsDiscount = copyFrom.Standard3Single_changeIsDiscount;
            Standard3Single_changeIsPercentage = copyFrom.Standard3Single_changeIsPercentage;

            Standard3Full_changeAmount = copyFrom.Standard3Full_changeAmount;
            Standard3Full_changeIsDiscount = copyFrom.Standard3Full_changeIsDiscount;
            Standard3Full_changeIsPercentage = copyFrom.Standard3Full_changeIsPercentage;

            Standard4Single_changeAmount = copyFrom.Standard4Single_changeAmount;
            Standard4Single_changeIsDiscount = copyFrom.Standard4Single_changeIsDiscount;
            Standard4Single_changeIsPercentage = copyFrom.Standard4Single_changeIsPercentage;

            Standard4Full_changeAmount = copyFrom.Standard4Full_changeAmount;
            Standard4Full_changeIsDiscount = copyFrom.Standard4Full_changeIsDiscount;
            Standard4Full_changeIsPercentage = copyFrom.Standard4Full_changeIsPercentage;
            #endregion

            NotRefundSingle_changeAmount = copyFrom.NotRefundSingle_changeAmount;
            NotRefundSingle_changeIsDiscount = copyFrom.NotRefundSingle_changeIsDiscount;
            NotRefundSingle_changeIsPercentage = copyFrom.NotRefundSingle_changeIsPercentage;

            NotRefundFull_changeAmount = copyFrom.NotRefundFull_changeAmount;
            NotRefundFull_changeIsDiscount = copyFrom.NotRefundFull_changeIsDiscount;
            NotRefundFull_changeIsPercentage = copyFrom.NotRefundFull_changeIsPercentage;

            SpecialSingle_changeAmount = copyFrom.SpecialSingle_changeAmount;
            SpecialSingle_changeIsDiscount = copyFrom.SpecialSingle_changeIsDiscount;
            SpecialSingle_changeIsPercentage = copyFrom.SpecialSingle_changeIsPercentage;

            SpecialFull_changeAmount = copyFrom.SpecialFull_changeAmount;
            SpecialFull_changeIsDiscount = copyFrom.SpecialFull_changeIsDiscount;
            SpecialFull_changeIsPercentage = copyFrom.SpecialFull_changeIsPercentage;
        }
        public bool HasSamePrices(BcomRates compareWith)
        {
            return
                SpecialSingle_changeAmount == compareWith.SpecialSingle_changeAmount &&
                StandardSingle_changeIsDiscount == compareWith.StandardSingle_changeIsDiscount &&
                StandardSingle_changeIsPercentage == compareWith.StandardSingle_changeIsPercentage &&

                SpecialFull_changeAmount == compareWith.SpecialFull_changeAmount &&
                StandardFull_changeIsDiscount == compareWith.StandardFull_changeIsDiscount &&
                StandardFull_changeIsPercentage == compareWith.StandardFull_changeIsPercentage &&

                NotRefundSingle_changeAmount == compareWith.NotRefundSingle_changeAmount &&
                NotRefundSingle_changeIsDiscount == compareWith.NotRefundSingle_changeIsDiscount &&
                NotRefundSingle_changeIsPercentage == compareWith.NotRefundSingle_changeIsPercentage &&

                NotRefundFull_changeAmount == compareWith.NotRefundFull_changeAmount &&
                NotRefundFull_changeIsDiscount == compareWith.NotRefundFull_changeIsDiscount &&
                NotRefundFull_changeIsPercentage == compareWith.NotRefundFull_changeIsPercentage &&

                SpecialSingle_changeAmount == compareWith.SpecialSingle_changeAmount &&
                SpecialSingle_changeIsDiscount == compareWith.SpecialSingle_changeIsDiscount &&
                SpecialSingle_changeIsPercentage == compareWith.SpecialSingle_changeIsPercentage &&

                SpecialFull_changeAmount == compareWith.SpecialFull_changeAmount &&
                SpecialFull_changeIsDiscount == compareWith.SpecialFull_changeIsDiscount &&
                SpecialFull_changeIsPercentage == compareWith.SpecialFull_changeIsPercentage
                ;
        }
        public void setRates(dbRntBcomEstateRatesRL tmpRate)
        {
            if (tmpRate.rateType == BcomProps.RateTypeStandard)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    StandardSingle_changeAmount = tmpRate.changeAmount;
                    StandardSingle_changeIsDiscount = tmpRate.changeIsDiscount;
                    StandardSingle_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    StandardFull_changeAmount = tmpRate.changeAmount;
                    StandardFull_changeIsDiscount = tmpRate.changeIsDiscount;
                    StandardFull_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }

            #region New Standard Rates
            if (tmpRate.rateType == BcomProps.RateTypeStandard2)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    Standard2Single_changeAmount = tmpRate.changeAmount;
                    Standard2Single_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard2Single_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    Standard2Full_changeAmount = tmpRate.changeAmount;
                    Standard2Full_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard2Full_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }

            if (tmpRate.rateType == BcomProps.RateTypeStandard3)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    Standard3Single_changeAmount = tmpRate.changeAmount;
                    Standard3Single_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard3Single_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    Standard3Full_changeAmount = tmpRate.changeAmount;
                    Standard3Full_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard3Full_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }

            if (tmpRate.rateType == BcomProps.RateTypeStandard4)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    Standard4Single_changeAmount = tmpRate.changeAmount;
                    Standard4Single_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard4Single_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    Standard4Full_changeAmount = tmpRate.changeAmount;
                    Standard4Full_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard4Full_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }
            #endregion

            if (tmpRate.rateType == BcomProps.RateTypeNotRefund)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    NotRefundSingle_changeAmount = tmpRate.changeAmount;
                    NotRefundSingle_changeIsDiscount = tmpRate.changeIsDiscount;
                    NotRefundSingle_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    NotRefundFull_changeAmount = tmpRate.changeAmount;
                    NotRefundFull_changeIsDiscount = tmpRate.changeIsDiscount;
                    NotRefundFull_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }
            if (tmpRate.rateType == BcomProps.RateTypeSpecial)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    SpecialSingle_changeAmount = tmpRate.changeAmount;
                    SpecialSingle_changeIsDiscount = tmpRate.changeIsDiscount;
                    SpecialSingle_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    SpecialFull_changeAmount = tmpRate.changeAmount;
                    SpecialFull_changeIsDiscount = tmpRate.changeIsDiscount;
                    SpecialFull_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }
        }
        public void setRates(dbRntBcomEstateRateChangesRL tmpRate)
        {
            if (tmpRate.rateType == BcomProps.RateTypeStandard)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    StandardSingle_changeAmount = tmpRate.changeAmount;
                    StandardSingle_changeIsDiscount = tmpRate.changeIsDiscount;
                    StandardSingle_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    StandardFull_changeAmount = tmpRate.changeAmount;
                    StandardFull_changeIsDiscount = tmpRate.changeIsDiscount;
                    StandardFull_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }

            #region New Standard Rates
            if (tmpRate.rateType == BcomProps.RateTypeStandard2)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    Standard2Single_changeAmount = tmpRate.changeAmount;
                    Standard2Single_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard2Single_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    Standard2Full_changeAmount = tmpRate.changeAmount;
                    Standard2Full_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard2Full_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }

            if (tmpRate.rateType == BcomProps.RateTypeStandard3)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    Standard3Single_changeAmount = tmpRate.changeAmount;
                    Standard3Single_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard3Single_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    Standard3Full_changeAmount = tmpRate.changeAmount;
                    Standard3Full_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard3Full_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }

            if (tmpRate.rateType == BcomProps.RateTypeStandard4)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    Standard4Single_changeAmount = tmpRate.changeAmount;
                    Standard4Single_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard4Single_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    Standard4Full_changeAmount = tmpRate.changeAmount;
                    Standard4Full_changeIsDiscount = tmpRate.changeIsDiscount;
                    Standard4Full_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }
            #endregion

            if (tmpRate.rateType == BcomProps.RateTypeNotRefund)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    NotRefundSingle_changeAmount = tmpRate.changeAmount;
                    NotRefundSingle_changeIsDiscount = tmpRate.changeIsDiscount;
                    NotRefundSingle_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    NotRefundFull_changeAmount = tmpRate.changeAmount;
                    NotRefundFull_changeIsDiscount = tmpRate.changeIsDiscount;
                    NotRefundFull_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }
            if (tmpRate.rateType == BcomProps.RateTypeSpecial)
            {
                if (tmpRate.priceType == BcomProps.PriceTypeSingle)
                {
                    SpecialSingle_changeAmount = tmpRate.changeAmount;
                    SpecialSingle_changeIsDiscount = tmpRate.changeIsDiscount;
                    SpecialSingle_changeIsPercentage = tmpRate.changeIsPercentage;
                }
                if (tmpRate.priceType == BcomProps.PriceTypeFull)
                {
                    SpecialFull_changeAmount = tmpRate.changeAmount;
                    SpecialFull_changeIsDiscount = tmpRate.changeIsDiscount;
                    SpecialFull_changeIsPercentage = tmpRate.changeIsPercentage;
                }
            }
        }
    }
    public static void BcomUpdate_start(int idEstate, string type)
    {
        BcomUpdate_process _tmp = new BcomUpdate_process(idEstate, type);
    }

    public static void BcomUpdate_start(int idEstate, string type, DateTime dtStart, DateTime dtEnd)
    {
        BcomUpdate_process _tmp = new BcomUpdate_process(idEstate, type, dtStart, dtEnd);
    }
}
