using MagaRentalCE.data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using System.IO;
using ModAuth;
using RentalInRome.data;

namespace MagaRentalCE.admin.modRental
{
    public partial class chnlAirbnbRetrieveReservation : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnk_retrieve_reservation_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
                string url = "https://api.airbnb.com/v2/reservations/" + txt_confirmation_code.Text;

                var currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.bcom_resId == txt_confirmation_code.Text);
                if (currRes == null) return;

                var currAirbnbHost = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == currRes.pid_estate);
                if (currAirbnbHost == null) return;

                string tmpErrorString = "";
                string responseData = ChnlAirbnbUtils.SendRequestReservations(url, "GET", "", ChnlAirbnbUtils.getAccessTokenHost(currAirbnbHost.hostId), out tmpErrorString);
                JsonSerializer serializer = new JsonSerializer();

                ChnlAirbnbUtils.addLog("", "reservations", "", responseData, "");
                ChnlAirbnbClasses.ReservionRetrieve reservationRetrieve = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.ReservionRetrieve)) as ChnlAirbnbClasses.ReservionRetrieve;
                if (reservationRetrieve != null)
                {
                    rntUtils.rntReservation_onStateChange(currRes);
                    if (reservationRetrieve.reservation.status_type == "cancelled_by_admin" || reservationRetrieve.reservation.status_type == "cancelled_by_host" || reservationRetrieve.reservation.status_type == "cancelled_by_guest" || reservationRetrieve.reservation.status_type == "deny")
                    {
                        currRes.state_pid = 3;
                        currRes.state_body = "";
                        currRes.state_date = DateTime.Now;
                        currRes.state_pid_user = 1;
                        currRes.state_subject = "CAN";
                    }
                    else if (reservationRetrieve.reservation.status_type == "accept")
                    {
                        currRes.state_pid = 4;
                        currRes.state_body = "";
                        currRes.state_date = DateTime.Now;
                        currRes.state_pid_user = 1;
                        currRes.state_subject = "Prenoto";
                    }
                    else
                    {
                        ErrorLog.addLog("", "res import", currRes.id + " " + reservationRetrieve.reservation.status_type);
                    }
                    currRes.cl_name_full = reservationRetrieve.reservation.guest_first_name + " " + reservationRetrieve.reservation.guest_last_name;
                    currRes.cl_email = reservationRetrieve.reservation.guest_email;
                    currRes.pr_total = reservationRetrieve.reservation.expected_payout_amount_accurate;
                    currRes.prTotalRate = reservationRetrieve.reservation.listing_base_price_accurate;
                    currRes.agentCommissionPrice = reservationRetrieve.reservation.listing_host_fee_accurate;
                    currRes.pr_deposit = reservationRetrieve.reservation.listing_security_price_accurate;

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
                    currRes.prTotalRateOnWeb = prTotal;
                    var prTotalRate = currRes.pr_total - outPrice.ecoPrice - outPrice.srsPrice;

                    if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 1)
                    {
                        currRes.prTotalOwner = outPrice.prTotalOwner;
                        currRes.prTotalCommission = currRes.prTotalRate - currRes.agentCommissionPrice - currRes.prTotalOwner;
                    }
                    else if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 0)
                    {
                        currRes.prTotalCommission = outPrice.prTotalCommission;
                        currRes.prTotalOwner = currRes.prTotalRate - currRes.agentCommissionPrice - currRes.prTotalCommission;
                    }
                    else if (currEstate.ownerContractPrice_commissionOnNet.objToInt32() == 2)
                    {
                        decimal ownerper = ((outPrice.prTotalOwner * 100) / outPrice.prTotalRate) / 100;
                        currRes.prTotalOwner = decimal.Multiply((currRes.prTotalRate.objToDecimal() - currRes.agentCommissionPrice.objToDecimal()), ownerper);
                        currRes.prTotalCommission = ((currRes.prTotalRate.objToDecimal() - currRes.agentCommissionPrice) - currRes.prTotalOwner);

                    }
                    #endregion

                    //currRes.notesInner = "Host Cancelltion fee: " + reservationRetrieve.reservation.listing_cancellation_host_fee_accurate + " ,Occupany Tax amount paid to Host:" + reservationRetrieve.reservation.occupancy_tax_amount_paid_to_host_accurate + " ,Transient Occupany tax paid amount:" + reservationRetrieve.reservation.transient_occupancy_tax_paid_amount_accurate;
                            
                    currRes.num_adult = reservationRetrieve.reservation.guest_details.number_of_adults;
                    currRes.num_child_over = reservationRetrieve.reservation.guest_details.number_of_children;
                    currRes.num_child_min = reservationRetrieve.reservation.guest_details.number_of_infants;

                    //var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currRes.pid_estate);
                    //if (currRes.pidEstateRoomNumber.objToInt32() == 0)
                    //{
                    //    int roomNumber = 0;
                    //    if (currEstate != null)
                    //        roomNumber = rntUtils.getEstateRoomNumber(currRes, currEstate.baseAvailability.objToInt32());
                    //    currRes.pidEstateRoomNumber = roomNumber;
                    //}

                    currRes.pr_ecoPrice = reservationRetrieve.reservation.listing_cleaning_fee_accurate;
                    //if (reservationRetrieve.reservation.listing_cleaning_fee_accurate > 0)
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
                    //    currReservationExtra.price = reservationRetrieve.reservation.listing_cleaning_fee_accurate;
                    //    DC_RENTAL.SubmitChanges();
                    //}

                    #region store data in agent client table
                    try
                    {
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
                            _client.nameFirst = reservationRetrieve.reservation.guest_first_name;
                            _client.nameLast = reservationRetrieve.reservation.guest_last_name;
                            _client.nameFull = reservationRetrieve.reservation.guest_first_name + " " + reservationRetrieve.reservation.guest_last_name;
                            _client.contactEmail = currRes.cl_email;
                            _client.contactPhoneMobile = reservationRetrieve.reservation.guest_phone_numbers.ToList().listToString(",");
                            _client.isActive = 1;
                            _client.pidLang = reservationRetrieve.reservation.guest_preferred_locale;
                            dcAuth.SaveChanges();
                            currRes.agentClientID = _client.id;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.addLog("", "import err", "agen");
                    }
                    #endregion

                    DC_RENTAL.SubmitChanges();
                    ErrorLog.addLog("", "import", "finish");
                    rntUtils.rntReservation_onChange(currRes);

                }
            }
        }
    }
}