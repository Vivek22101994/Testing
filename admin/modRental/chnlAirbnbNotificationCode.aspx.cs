using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using ModAuth;
using RentalInRome.data;

namespace MagaRentalCE.admin.modRental
{
    public partial class chnlAirbnbNotificationCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Response == null) return;
                var Request = HttpContext.Current.Request;
                var Response = HttpContext.Current.Response;
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
                string responseConent = "";

                JsonSerializer serializer = new JsonSerializer();
                ChnlAirbnbClasses.NotificationRequest notificationRequest = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.NotificationRequest)) as ChnlAirbnbClasses.NotificationRequest;
                if (notificationRequest != null)
                {
                    ErrorLog.addLog("", "notification request", requestContent);
                    if (notificationRequest.action == "test_notification")
                    {
                        responseConent = "{\"succeed\":\"" + true + "\"}";

                    }
                    else if (notificationRequest.action == "listing_approval_status_changed")
                    {
                        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                        {
                            ChnlAirbnbClasses.ListingApprovalNotificationResponse listingApprovalNotificationResponse = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.ListingApprovalNotificationResponse)) as ChnlAirbnbClasses.ListingApprovalNotificationResponse;
                            if (listingApprovalNotificationResponse != null)
                            {
                                RntChnlAirbnbEstateTBL currAirbnbEstateTBL = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.airbnb_id == listingApprovalNotificationResponse.listing_approval_status.listing_id.objToInt32());
                                if (currAirbnbEstateTBL == null)
                                {
                                    ErrorLog.addLog("", "airbnb listing approval notification", "Missing AirBnb Estate");
                                    responseConent = "{\"succeed\":\"" + false + "\"}";
                                }
                                else
                                {
                                    responseConent = "{\"succeed\":\"" + true + "\"}";

                                    //update status and date
                                    currAirbnbEstateTBL.airbnb_id = listingApprovalNotificationResponse.listing_approval_status.listing_id.objToInt32();
                                    currAirbnbEstateTBL.status = listingApprovalNotificationResponse.listing_approval_status.approval_status_category;
                                    currAirbnbEstateTBL.date = DateTime.Now;
                                    currAirbnbEstateTBL.notes = listingApprovalNotificationResponse.listing_approval_status.notes;
                                    dc.SubmitChanges();
                                    //insert into status history
                                    RntChnlAirbnbEstateStatusRL objAirbnbEstateStatusRL = new RntChnlAirbnbEstateStatusRL();
                                    objAirbnbEstateStatusRL.airbnbEstate = listingApprovalNotificationResponse.listing_approval_status.listing_id;
                                    objAirbnbEstateStatusRL.notes = listingApprovalNotificationResponse.listing_approval_status.notes;
                                    objAirbnbEstateStatusRL.date = DateTime.Now;
                                    objAirbnbEstateStatusRL.status = listingApprovalNotificationResponse.listing_approval_status.approval_status_category;
                                    objAirbnbEstateStatusRL.pidEstate = currAirbnbEstateTBL.mr_id;
                                    dc.RntChnlAirbnbEstateStatusRL.InsertOnSubmit(objAirbnbEstateStatusRL);
                                    dc.SubmitChanges();
                                }
                            }
                        }
                    }
                    else if (notificationRequest.action == "listing_approval_status_changed")
                    {
                        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                        {
                            ChnlAirbnbClasses.ListingApprovalNotificationResponse listingApprovalNotificationResponse = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.ListingApprovalNotificationResponse)) as ChnlAirbnbClasses.ListingApprovalNotificationResponse;
                            if (listingApprovalNotificationResponse != null)
                            {
                                RntChnlAirbnbEstateTBL currAirbnbEstateTBL = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.airbnb_id == listingApprovalNotificationResponse.listing_approval_status.listing_id.objToInt32());
                                if (currAirbnbEstateTBL == null)
                                {
                                    ErrorLog.addLog("", "airbnb listing approval notification", "Missing AirBnb Estate");
                                    responseConent = "{\"succeed\":\"" + false + "\"}";
                                }
                                else
                                {
                                    responseConent = "{\"succeed\":\"" + true + "\"}";

                                    //update status and date
                                    currAirbnbEstateTBL.airbnb_id = listingApprovalNotificationResponse.listing_approval_status.listing_id.objToInt32();
                                    currAirbnbEstateTBL.status = listingApprovalNotificationResponse.listing_approval_status.approval_status_category;
                                    currAirbnbEstateTBL.date = DateTime.Now;
                                    currAirbnbEstateTBL.notes = listingApprovalNotificationResponse.listing_approval_status.notes;
                                    dc.SubmitChanges();

                                    //insert into status history
                                    RntChnlAirbnbEstateStatusRL objAirbnbEstateStatusRL = new RntChnlAirbnbEstateStatusRL();
                                    objAirbnbEstateStatusRL.airbnbEstate = listingApprovalNotificationResponse.listing_approval_status.listing_id;
                                    objAirbnbEstateStatusRL.notes = listingApprovalNotificationResponse.listing_approval_status.notes;
                                    objAirbnbEstateStatusRL.date = DateTime.Now;
                                    objAirbnbEstateStatusRL.status = listingApprovalNotificationResponse.listing_approval_status.approval_status_category;
                                    objAirbnbEstateStatusRL.pidEstate = currAirbnbEstateTBL.mr_id;
                                    dc.RntChnlAirbnbEstateStatusRL.InsertOnSubmit(objAirbnbEstateStatusRL);
                                    dc.SubmitChanges();
                                }
                            }
                        }
                    }
                    else if (notificationRequest.action == "listing_synchronization_settings_updated")
                    {
                        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                        {
                            ChnlAirbnbClasses.ListingSyncNotificationResponse listingSyncNotificationResponse = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.ListingSyncNotificationResponse)) as ChnlAirbnbClasses.ListingSyncNotificationResponse;
                            if (listingSyncNotificationResponse != null)
                            {
                                var updates = listingSyncNotificationResponse.updates.ToList();
                                foreach (var update in updates)
                                {
                                    RntChnlAirbnbEstateTBL currAirbnbEstateTBL = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.airbnb_id == update.listing_id.objToInt32());
                                    if (currAirbnbEstateTBL == null)
                                    {
                                        ErrorLog.addLog("", "airbnb listing approval notification", "Missing AirBnb Estate");
                                        continue;
                                    }
                                    else
                                    {
                                        //update status and date
                                        currAirbnbEstateTBL.airbnb_id = update.listing_id.objToInt32();
                                        currAirbnbEstateTBL.syncCategory = update.synchronization_category;
                                        currAirbnbEstateTBL.syncDtae = DateTime.Now;
                                        dc.SubmitChanges();

                                        //insert into status history
                                        RntChnlAirbnbEstateSyncRL objAirbnbEstateSyncRL = new RntChnlAirbnbEstateSyncRL();
                                        objAirbnbEstateSyncRL.airbnbEstate = update.listing_id + "";
                                        objAirbnbEstateSyncRL.date = DateTime.Now;
                                        objAirbnbEstateSyncRL.syncCategory = update.synchronization_category;
                                        objAirbnbEstateSyncRL.pidEstate = currAirbnbEstateTBL.mr_id;
                                        dc.RntChnlAirbnbEstateSyncRL.InsertOnSubmit(objAirbnbEstateSyncRL);
                                        dc.SubmitChanges();
                                    }
                                }
                                responseConent = "{\"succeed\":\"" + true + "\"}";
                            }
                        }
                    }
                    else if (notificationRequest.action == "listings_unlinked")
                    {
                        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                        {
                            ChnlAirbnbClasses.AuthorizationRevoked authorizationRevoked = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.AuthorizationRevoked)) as ChnlAirbnbClasses.AuthorizationRevoked;
                            if (authorizationRevoked != null)
                            {
                                var listings = authorizationRevoked.listing_ids.ToList();
                                foreach (var listing in listings)
                                {
                                    ErrorLog.addLog("", "listings_unlinked", listing);
                                    RntChnlAirbnbEstateTBL currAirbnbEstateTBL = dc.RntChnlAirbnbEstateTBL.SingleOrDefault(x => x.airbnb_id == listing.objToInt32());
                                    if (currAirbnbEstateTBL != null)
                                    {
                                        currAirbnbEstateTBL.syncCategory = "Unlinked";
                                        dc.SubmitChanges();
                                    }
                                }
                            }
                        }
                        responseConent = "{\"succeed\":\"" + true + "\"}";
                    }
                    else if (notificationRequest.action == "authorization_revoked")
                    {
                        using (magaChnlAirbnbDataContext dc = maga_DataContext.DC_AIRBNB)
                        {
                            ChnlAirbnbClasses.AuthorizationRevoked authorizationRevoked = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.AuthorizationRevoked)) as ChnlAirbnbClasses.AuthorizationRevoked;
                            if (authorizationRevoked != null)
                            {
                                var currAuthCode = dc.RntChnlAirbnbAccessToken.Where(x => x.isActive == 1 && x.userId == authorizationRevoked.host_id).FirstOrDefault();
                                if (currAuthCode != null)
                                {
                                    currAuthCode.isActive = 0;
                                    dc.SubmitChanges();
                                }
                            }
                            responseConent = "{\"succeed\":\"" + true + "\"}";
                        }
                    }
                    else if (notificationRequest.action == "reservation_acceptance_confirmation")
                    {
                        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
                        ChnlAirbnbClasses.ReservionAcceptanceRequest reservionAcceptanceRequest = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.ReservionAcceptanceRequest)) as ChnlAirbnbClasses.ReservionAcceptanceRequest;
                        if (reservionAcceptanceRequest != null)
                        {
                            var currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.bcom_resId == reservionAcceptanceRequest.reservation.confirmation_code);
                            if (currRes != null)
                            {
                                rntUtils.rntReservation_onStateChange(currRes);
                                currRes.state_pid = 4;
                                currRes.state_body = "";
                                currRes.state_date = DateTime.Now;
                                currRes.state_pid_user = 1;
                                currRes.state_subject = "Prenoto";

                                currRes.cl_name_full = reservionAcceptanceRequest.reservation.guest_first_name + " " + reservionAcceptanceRequest.reservation.guest_last_name;
                                currRes.cl_email = reservionAcceptanceRequest.reservation.guest_email;
                                //currRes.prTotalRate = currRes.pr_total = reservionAcceptanceRequest.reservation.expected_payout_amount_accurate;

                                currRes.pr_total = reservionAcceptanceRequest.reservation.expected_payout_amount_accurate;
                                currRes.prTotalRate = reservionAcceptanceRequest.reservation.listing_base_price_accurate;
                                currRes.agentCommissionPrice = reservionAcceptanceRequest.reservation.listing_host_fee_accurate;
                                currRes.pr_deposit = reservionAcceptanceRequest.reservation.listing_security_price_accurate;

                                #region store MR prices
                                #region Made change for using values of Acconto and balance configured in agent detail
                                var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currRes.pid_estate);
                                currRes.pr_part_payment_total = 0;
                                currRes.pr_part_owner = currRes.pr_total;

                                //var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.IdAdMedia == "airbnb" || x.IdAdMedia == "Airbnb");
                                //if (agentTbl != null)
                                //{
                                //    if (agentTbl.PartPayment.objToDecimal() == 100 || agentTbl.IsAllPayment == 1)
                                //    {
                                //        currRes.pr_part_payment_total = currRes.pr_total;
                                //        currRes.pr_part_owner = 0;
                                //    }
                                //    else
                                //    {
                                //        currRes.pr_part_payment_total = (currRes.pr_total * agentTbl.PartPayment.objToDecimal()) / 100;
                                //        currRes.pr_part_owner = currRes.pr_total - currRes.pr_part_payment_total;
                                //    }
                                //}
                                //set pr_part_forPayment
                                currRes.pr_part_forPayment = currRes.pr_part_payment_total;
                                #endregion

                                currRes.pr_part_modified = 1;
                                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                                outPrice.dtStart = currRes.dtStart.Value;
                                outPrice.dtEnd = currRes.dtEnd.Value;
                                outPrice.numPersCount = currRes.num_adult.objToInt32() + currRes.num_child_over.objToInt32();
                                outPrice.pr_discount_owner = 0;
                                outPrice.pr_discount_commission = 0;
                                //outPrice.fillAgentDetails(agentTbl);
                                outPrice.part_percentage = currEstate.pr_percentage.objToInt32();
                                var prTotal = rntUtils.rntEstate_getPrice(0, currEstate.id, ref outPrice);
                                //if (newRes.bcom_totalForOwner.objToDecimal() == 0)

                                currRes.pr_part_agency_fee = 0;
                                currRes.pr_reservation = currRes.prTotalRate;
                                //currRes.prTotalRateOnWeb = prTotal;
                                var prTotalRate = currRes.pr_total - outPrice.ecoPrice - outPrice.srsPrice;

                                var bcom_totalForOwner = currRes.pr_total - currRes.agentCommissionPrice;
                                currRes.bcom_totalForOwner = bcom_totalForOwner;
                                currRes.pr_part_agency_fee = 0;
                                currRes.prTotalCommission = currEstate.pr_percentage.objToInt32() * bcom_totalForOwner / 100;
                                currRes.prTotalOwner = bcom_totalForOwner - currRes.prTotalCommission;

                                //if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 1)
                                //{
                                //    currRes.prTotalOwner = outPrice.prTotalOwner;
                                //    currRes.prTotalCommission = currRes.prTotalRate - currRes.agentCommissionPrice - currRes.prTotalOwner;
                                //}
                                //else if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 0)
                                //{
                                //    currRes.prTotalCommission = outPrice.prTotalCommission;
                                //    currRes.prTotalOwner = currRes.prTotalRate - currRes.agentCommissionPrice - currRes.prTotalCommission;
                                //}
                                //else if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 2)
                                //{
                                //    decimal ownerper = ((outPrice.prTotalOwner * 100) / outPrice.prTotalRate) / 100;
                                //    currRes.prTotalOwner = decimal.Multiply((currRes.prTotalRate.objToDecimal() - currRes.agentCommissionPrice.objToDecimal()), ownerper);
                                //    currRes.prTotalCommission = ((currRes.prTotalRate.objToDecimal() - currRes.agentCommissionPrice) - currRes.prTotalOwner);

                                //}
                                #endregion

                                //currRes.airbnb_cancellation_host_fee = reservionAcceptanceRequest.reservation.listing_cancellation_host_fee_accurate;
                                //currRes.airbnb_occupancy_tax_amount_paid_to_host = reservionAcceptanceRequest.reservation.occupancy_tax_amount_paid_to_host_accurate;
                                //currRes.airbnb_transient_occupancy_tax_paid_amount = reservionAcceptanceRequest.reservation.transient_occupancy_tax_paid_amount_accurate;
                                //currRes.notesInner = "Host Cancelltion fee: " + reservionAcceptanceRequest.reservation.listing_cancellation_host_fee_accurate + " ,Occupany Tax amount paid to Host:" + reservionAcceptanceRequest.reservation.occupancy_tax_amount_paid_to_host_accurate + " ,Transient Occupany tax paid amount:" + reservionAcceptanceRequest.reservation.transient_occupancy_tax_paid_amount_accurate;

                                currRes.num_adult = reservionAcceptanceRequest.reservation.guest_details.number_of_adults;
                                currRes.num_child_over = reservionAcceptanceRequest.reservation.guest_details.number_of_children;
                                currRes.num_child_min = reservionAcceptanceRequest.reservation.guest_details.number_of_infants;

                                #region store data in agent client table
                                using (DCmodAuth dcAuth = new DCmodAuth())
                                {
                                    var _client = dcAuth.dbAuthClientTBLs.FirstOrDefault(x => x.id == currRes.agentClientID);
                                    if (_client == null)
                                    {
                                        _client = new dbAuthClientTBL();
                                        _client.pidAgent = currRes.agentID;
                                        _client.uid = Guid.NewGuid();
                                        _client.createdDate = DateTime.Now;
                                        _client.createdUserID = 1;
                                        _client.createdUserNameFull = "System";
                                        dcAuth.Add(_client);
                                        dcAuth.SaveChanges();
                                        _client.code = _client.id.ToString().fillString("0", 6, false);
                                        dcAuth.SaveChanges();

                                    }
                                    _client.nameFirst = reservionAcceptanceRequest.reservation.guest_first_name;
                                    _client.nameLast = reservionAcceptanceRequest.reservation.guest_last_name;
                                    _client.nameFull = reservionAcceptanceRequest.reservation.guest_first_name + " " + reservionAcceptanceRequest.reservation.guest_last_name;
                                    _client.contactEmail = currRes.cl_email;
                                    _client.contactPhoneMobile = reservionAcceptanceRequest.reservation.guest_phone_numbers.ToList().listToString(",");
                                    _client.isActive = 1;
                                    _client.pidLang = reservionAcceptanceRequest.reservation.guest_preferred_locale;
                                    dcAuth.SaveChanges();
                                    currRes.agentClientID = _client.id;
                                    rntUtils.rntReservation_onChange(currRes);
                                }
                                #endregion

                                DC_RENTAL.SubmitChanges();
                                currRes.pr_ecoPrice = reservionAcceptanceRequest.reservation.listing_cleaning_fee_accurate;
                                //if (reservionAcceptanceRequest.reservation.listing_cleaning_fee_accurate > 0)
                                //{
                                //    var currReservationExtra = DC_RENTAL.RNT_TBL_EXTRA_RESERVATION.SingleOrDefault(x => x.pidExtra == CommonUtilities.getSYS_SETTING("airbnb_cleaning_id").objToInt32() && x.pidReservation == currRes.id);
                                //    if (currReservationExtra == null)
                                //    {
                                //        currReservationExtra = new RNT_TBL_EXTRA_RESERVATION();
                                //        currReservationExtra.pidReservation = currRes.id;
                                //        currReservationExtra.pidExtra = CommonUtilities.getSYS_SETTING("airbnb_cleaning_id").objToInt32();
                                //        DC_RENTAL.RNT_TBL_EXTRA_RESERVATION.InsertOnSubmit(currReservationExtra);
                                //        DC_RENTAL.SubmitChanges();
                                //    }
                                //    currReservationExtra.price = reservionAcceptanceRequest.reservation.listing_cleaning_fee_accurate;
                                //    DC_RENTAL.SubmitChanges();
                                //}

                                rntUtils.rntReservation_onChange(currRes);
                                rntUtils.reservation_checkPartPayment(currRes, true);

                                if (CommonUtilities.getSYS_SETTING("rntAirbnb_sendEmails") == "true" || CommonUtilities.getSYS_SETTING("rntAirbnb_sendEmails").ToInt32() == 1)
                                {
                                    rntUtils.rntReservation_mailPartPaymentReceive(currRes, false, true, true, true, true, 1); // send mails
                                    //rntUtils.MailReservationConfirmed(currRes, false, true, true, true, true, 1); // send mails
                                }
                                responseConent = "{\"succeed\":true}";
                            }
                            else
                            {
                                responseConent = "{\"succeed\":false}";
                            }

                        }
                    }
                    else if (notificationRequest.action == "reservation_cancellation_confirmation")
                    {
                        //TO DO : keep trck here updtion done on reservation
                        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
                        ChnlAirbnbClasses.ReservionAcceptanceRequest reservionAcceptanceRequest = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.ReservionAcceptanceRequest)) as ChnlAirbnbClasses.ReservionAcceptanceRequest;
                        if (reservionAcceptanceRequest != null)
                        {
                            var currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.bcom_resId == reservionAcceptanceRequest.reservation.confirmation_code);
                            if (currRes != null)
                            {
                                rntUtils.rntReservation_onStateChange(currRes);
                                currRes.state_pid = 3;
                                currRes.state_body = "";
                                currRes.state_date = DateTime.Now;
                                currRes.state_pid_user = 1;
                                currRes.state_subject = reservionAcceptanceRequest.reservation.status_type;
                                DC_RENTAL.SubmitChanges();

                                //send email
                                if ((CommonUtilities.getSYS_SETTING("rntAirbnb_sendEmailsCancelled") == "true" || CommonUtilities.getSYS_SETTING("rntAirbnb_sendEmailsCancelled").ToInt32() == 1))
                                {
                                    rntUtils.rntReservation_mailCancelled(currRes, false, true, false, false, true, 1); // send mails
                                }

                                rntUtils.rntReservation_onChange(currRes);
                                responseConent = "{\"succeed\":true}";
                                //responseConent = "{\"succeed\":\"" + true + "\"}";
                            }
                            else
                            {
                                responseConent = "{\"succeed\":false}";
                                //responseConent = "{\"succeed\":\"" + false + "\"}";
                            }
                        }
                    }
                    else if (notificationRequest.action == "reservation_alteration_confirmation")
                    {
                        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
                        ChnlAirbnbClasses.ReservionAcceptanceRequest reservionAcceptanceRequest = serializer.Deserialize(new StringReader(requestContent), typeof(ChnlAirbnbClasses.ReservionAcceptanceRequest)) as ChnlAirbnbClasses.ReservionAcceptanceRequest;
                        if (reservionAcceptanceRequest != null)
                        {
                            var currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.bcom_resId == reservionAcceptanceRequest.reservation.confirmation_code);
                            if (currRes != null)
                            {
                                currRes.cl_name_full = reservionAcceptanceRequest.reservation.guest_first_name + " " + reservionAcceptanceRequest.reservation.guest_last_name;
                                currRes.cl_email = reservionAcceptanceRequest.reservation.guest_email;

                                currRes.pr_total = reservionAcceptanceRequest.reservation.expected_payout_amount_accurate;
                                currRes.prTotalRate = reservionAcceptanceRequest.reservation.listing_base_price_accurate;
                                currRes.agentCommissionPrice = reservionAcceptanceRequest.reservation.listing_host_fee_accurate;
                                currRes.pr_deposit = reservionAcceptanceRequest.reservation.listing_security_price_accurate;

                                #region store MR prices
                                #region Made change for using values of Acconto and balance configured in agent detail
                                var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currRes.pid_estate);

                                currRes.pr_part_payment_total = 0;
                                currRes.pr_part_owner = currRes.pr_total;

                                //var agentTbl = rntProps.AgentTBL.SingleOrDefault(x => x.IdAdMedia == "airbnb" || x.IdAdMedia == "Airbnb");
                                //if (agentTbl != null)
                                //{
                                //    if (agentTbl.PartPayment.objToDecimal() == 100 || agentTbl.IsAllPayment == 1)
                                //    {
                                //        currRes.pr_part_payment_total = currRes.pr_total;
                                //        currRes.pr_part_owner = 0;
                                //    }
                                //    else
                                //    {
                                //        currRes.pr_part_payment_total = (currRes.pr_total * agentTbl.PartPayment.objToDecimal()) / 100;
                                //        currRes.pr_part_owner = currRes.pr_total - currRes.pr_part_payment_total;
                                //    }
                                //}
                                //set pr_part_forPayment
                                currRes.pr_part_forPayment = currRes.pr_part_payment_total;
                                #endregion

                                currRes.pr_part_modified = 1;
                                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                                outPrice.dtStart = currRes.dtStart.Value;
                                outPrice.dtEnd = currRes.dtEnd.Value;
                                outPrice.numPersCount = currRes.num_adult.objToInt32() + currRes.num_child_over.objToInt32();
                                outPrice.pr_discount_owner = 0;
                                outPrice.pr_discount_commission = 0;
                                //outPrice.fillAgentDetails(agentTbl);
                                outPrice.part_percentage = currEstate.pr_percentage.objToInt32();
                                var prTotal = rntUtils.rntEstate_getPrice(0, currEstate.id, ref outPrice);
                                //if (newRes.bcom_totalForOwner.objToDecimal() == 0)

                                currRes.pr_part_agency_fee = 0;
                                currRes.pr_reservation = currRes.prTotalRate;
                                //currRes.prTotalRateOnWeb = prTotal;
                                var prTotalRate = currRes.pr_total - outPrice.ecoPrice - outPrice.srsPrice;

                                var bcom_totalForOwner = currRes.pr_total - currRes.agentCommissionPrice;
                                currRes.bcom_totalForOwner = bcom_totalForOwner;
                                currRes.pr_part_agency_fee = 0;
                                currRes.prTotalCommission = currEstate.pr_percentage.objToInt32() * bcom_totalForOwner / 100;
                                currRes.prTotalOwner = bcom_totalForOwner - currRes.prTotalCommission;

                                //if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 1)
                                //{
                                //    currRes.prTotalOwner = outPrice.prTotalOwner;
                                //    currRes.prTotalCommission = currRes.prTotalRate - currRes.agentCommissionPrice - currRes.prTotalOwner;
                                //}
                                //else if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 0)
                                //{
                                //    currRes.prTotalCommission = outPrice.prTotalCommission;
                                //    currRes.prTotalOwner = currRes.prTotalRate - currRes.agentCommissionPrice - currRes.prTotalCommission;
                                //}
                                //else if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 2)
                                //{
                                //    decimal ownerper = ((outPrice.prTotalOwner * 100) / outPrice.prTotalRate) / 100;
                                //    currRes.prTotalOwner = decimal.Multiply((currRes.prTotalRate.objToDecimal() - currRes.agentCommissionPrice.objToDecimal()), ownerper);
                                //    currRes.prTotalCommission = ((currRes.prTotalRate.objToDecimal() - currRes.agentCommissionPrice) - currRes.prTotalOwner);

                                //}
                                #endregion

                                //currRes.airbnb_cancellation_host_fee = reservionAcceptanceRequest.reservation.listing_cancellation_host_fee_accurate;
                                //currRes.airbnb_occupancy_tax_amount_paid_to_host = reservionAcceptanceRequest.reservation.occupancy_tax_amount_paid_to_host_accurate;
                                //currRes.airbnb_transient_occupancy_tax_paid_amount = reservionAcceptanceRequest.reservation.transient_occupancy_tax_paid_amount_accurate;
                                //currRes.notesInner = "Host Cancelltion fee: " + reservionAcceptanceRequest.reservation.listing_cancellation_host_fee_accurate + " ,Occupany Tax amount paid to Host:" + reservionAcceptanceRequest.reservation.occupancy_tax_amount_paid_to_host_accurate + " ,Transient Occupany tax paid amount:" + reservionAcceptanceRequest.reservation.transient_occupancy_tax_paid_amount_accurate;

                                currRes.num_adult = reservionAcceptanceRequest.reservation.guest_details.number_of_adults;
                                currRes.num_child_over = reservionAcceptanceRequest.reservation.guest_details.number_of_children;
                                currRes.num_child_min = reservionAcceptanceRequest.reservation.guest_details.number_of_infants;

                                string start_date = reservionAcceptanceRequest.reservation.start_date;
                                string end_date = reservionAcceptanceRequest.reservation.end_date;

                                if (start_date != "" && start_date.Split('-').Length >= 3 && end_date != "" && end_date.Split('-').Length >= 3)
                                {
                                    DateTime dtStart = new DateTime(start_date.Split('-')[0].objToInt32(), start_date.Split('-')[1].objToInt32(), start_date.Split('-')[2].objToInt32());
                                    DateTime dtEnd = new DateTime(end_date.Split('-')[0].objToInt32(), end_date.Split('-')[1].objToInt32(), end_date.Split('-')[2].objToInt32());

                                    currRes.dtStart = dtStart;
                                    currRes.dtEnd = dtEnd;
                                }

                                //currRes.prTotalRate = currRes.pr_total = reservionAcceptanceRequest.reservation.expected_payout_amount_accurate;
                                currRes.state_date = DateTime.Now;
                                currRes.state_subject = "Altered by Airbnb";

                                #region store data in agent client table
                                using (magaAuth_DataContext dcAuth = maga_DataContext.DC_Auth)
                                {
                                    var _client = dcAuth.AuthClientTBL.FirstOrDefault(x => x.id == currRes.agentClientID);
                                    if (_client == null)
                                    {
                                        _client = new AuthClientTBL();
                                        _client.pidAgent = currRes.agentID;
                                        _client.uid = Guid.NewGuid();
                                        _client.createdDate = DateTime.Now;
                                        _client.createdUserID = 1;
                                        _client.createdUserNameFull = "System";
                                        dcAuth.AuthClientTBL.InsertOnSubmit(_client);
                                        dcAuth.SubmitChanges();
                                        _client.code = _client.id.ToString().fillString("0", 6, false);
                                        dcAuth.SubmitChanges();

                                    }
                                    _client.nameFirst = reservionAcceptanceRequest.reservation.guest_first_name;
                                    _client.nameLast = reservionAcceptanceRequest.reservation.guest_last_name;
                                    _client.nameFull = reservionAcceptanceRequest.reservation.guest_first_name + " " + reservionAcceptanceRequest.reservation.guest_last_name;
                                    _client.contactEmail = currRes.cl_email;
                                    _client.contactPhoneMobile = reservionAcceptanceRequest.reservation.guest_phone_numbers.ToList().listToString(",");
                                    _client.isActive = 1;
                                    _client.pidLang = reservionAcceptanceRequest.reservation.guest_preferred_locale;
                                    dcAuth.SubmitChanges();
                                    currRes.agentClientID = _client.id;
                                }
                                #endregion

                                DC_RENTAL.SubmitChanges();

                                currRes.pr_ecoPrice = reservionAcceptanceRequest.reservation.listing_cleaning_fee_accurate;
                                //if (reservionAcceptanceRequest.reservation.listing_cleaning_fee_accurate > 0)
                                //{
                                //    var currReservationExtra = DC_RENTAL.RNT_TBL_EXTRA_RESERVATION.SingleOrDefault(x => x.pidExtra == CommonUtilities.getSYS_SETTING("airbnb_cleaning_id").objToInt32() && x.pidReservation == currRes.id);
                                //    if (currReservationExtra == null)
                                //    {
                                //        currReservationExtra = new RNT_TBL_EXTRA_RESERVATION();
                                //        currReservationExtra.pidReservation = currRes.id;
                                //        currReservationExtra.pidExtra = CommonUtilities.getSYS_SETTING("airbnb_cleaning_id").objToInt32();
                                //        DC_RENTAL.RNT_TBL_EXTRA_RESERVATION.InsertOnSubmit(currReservationExtra);
                                //        DC_RENTAL.SubmitChanges();
                                //    }
                                //    currReservationExtra.price = reservionAcceptanceRequest.reservation.listing_cleaning_fee_accurate;
                                //    DC_RENTAL.SubmitChanges();
                                //}

                                rntUtils.rntReservation_onChange(currRes);
                                responseConent = "{\"succeed\":true}";
                            }
                            else
                            {
                                responseConent = "{\"succeed\":false}";
                            }
                        }
                    }
                    Response.StatusCode = 200;
                    Response.Write(responseConent);
                    Response.End();
                }
            }
        }
    }
}