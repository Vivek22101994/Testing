using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using System.Net;
using System.IO;
using MagaRentalCE.data;
using ModAuth;
using Newtonsoft.Json;

namespace MagaRentalCE.admin.modRental
{
    public partial class chnlAirbnbRetrieveReservations : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillHosts();
                fillEstates();
            }
        }
        protected void fillEstates()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var airbnbEstates = dc.dbRntChnlAirbnbEstateTBLs.Where(x => x.airbnb_id != null && x.airbnb_id > 0 && x.hostId == drp_host.SelectedValue).Select(x => x.mr_id).ToList();
                var estates = AppSettings.RNT_TB_ESTATE.Where(x => airbnbEstates.Contains(x.id)).ToList();

                drp_estates.DataSource = estates;
                drp_estates.DataTextField = "code";
                drp_estates.DataValueField = "id";
                drp_estates.DataBind();

                drp_estates.Items.Insert(0, new ListItem("--All--", "0"));

            }
        }
        protected void lnk_retrieve_reservations_Click(object sender, EventArgs e)
        {
            var airbnb_estate = drp_estates.getSelectedValueInt();
            int airbnbId = 0;
            String url = "";
            if (airbnb_estate > 0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var currAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == airbnb_estate);
                    if (currAirbnbEstate != null)
                    {
                        airbnbId = currAirbnbEstate.airbnb_id.objToInt32();
                    }
                }
            }

            url = "https://api.airbnb.com/v2/reservations/?host_id=" + drp_host.SelectedValue + "&all_status=true";

            if (rdp_rate_dtStart.SelectedDate != null)
                url = url + "&start_date=" + Convert.ToDateTime(rdp_rate_dtStart.SelectedDate).ToString("yyyy-MM-dd");

            if (rdp_rate_dtEnd.SelectedDate != null)
                url = url + "&end_date=" + Convert.ToDateTime(rdp_rate_dtEnd.SelectedDate).ToString("yyyy-MM-dd");

            if (airbnbId > 0)
                url = url + "&listing_id=" + airbnbId;

            string tmpErrorString = "";
            string responseData = ChnlAirbnbUtils.SendRequestReservations(url, "GET", "", ChnlAirbnbUtils.getAccessTokenHost(drp_host.SelectedValue), out tmpErrorString);
            JsonSerializer serializer = new JsonSerializer();
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;

            ChnlAirbnbUtils.addLog("", "reservations", "", responseData, "");
            ChnlAirbnbClasses.AllResevations allReservations = serializer.Deserialize(new StringReader(responseData), typeof(ChnlAirbnbClasses.AllResevations)) as ChnlAirbnbClasses.AllResevations;
            if (allReservations != null)
            {
                var reservations = allReservations.reservations;
                foreach (var reservation in reservations)
                {
                    var currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.bcom_resId == reservation.confirmation_code);
                    if (currRes != null)
                    {
                        rntUtils.rntReservation_onStateChange(currRes);
                        if (reservation.status_type == "cancelled_by_admin" || reservation.status_type == "cancelled_by_host" || reservation.status_type == "cancelled_by_guest" || reservation.status_type == "deny")
                        {
                            currRes.state_pid = 3;
                            currRes.state_body = "";
                            currRes.state_date = DateTime.Now;
                            currRes.state_pid_user = 1;
                            currRes.state_subject = "CAN";
                        }
                        else if (reservation.status_type == "accept")
                        {
                            currRes.state_pid = 4;
                            currRes.state_body = "";
                            currRes.state_date = DateTime.Now;
                            currRes.state_pid_user = 1;
                            currRes.state_subject = "Prenoto";
                        }
                        else
                        {
                            ErrorLog.addLog("", "res import", currRes.id + " " + reservation.status_type);
                        }
                        currRes.cl_name_full = reservation.guest_first_name + " " + reservation.guest_last_name;
                        currRes.cl_email = reservation.guest_email;
                        currRes.pr_total = reservation.expected_payout_amount_accurate;
                        currRes.prTotalRate = reservation.listing_base_price_accurate;
                        currRes.agentCommissionPrice = reservation.listing_host_fee_accurate;
                        currRes.pr_deposit = reservation.listing_security_price_accurate;

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

                        currRes.notesInner = "Host Cancelltion fee: " + reservation.listing_cancellation_host_fee_accurate + " ,Occupany Tax amount paid to Host:" + reservation.occupancy_tax_amount_paid_to_host_accurate + " ,Transient Occupany tax paid amount:" + reservation.transient_occupancy_tax_paid_amount_accurate;

                        currRes.num_adult = reservation.guest_details.number_of_adults;
                        currRes.num_child_over = reservation.guest_details.number_of_children;
                        currRes.num_child_min = reservation.guest_details.number_of_infants;

                        //var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currRes.pid_estate);
                        //if (currRes.pidEstateRoomNumber.objToInt32() == 0)
                        //{
                        //    int roomNumber = 0;
                        //    if (currEstate != null)
                        //        roomNumber = rntUtils.getEstateRoomNumber(currRes, currEstate.baseAvailability.objToInt32());
                        //    currRes.pidEstateRoomNumber = roomNumber;
                        //}

                        if (reservation.listing_cleaning_fee_accurate > 0)
                        {
                            var currReservationExtra = DC_RENTAL.RNT_TBL_EXTRA_RESERVATION.SingleOrDefault(x => x.pidExtra == CommonUtilities.getSYS_SETTING("airbnb_cleaning_id").objToInt32() && x.pidReservation == currRes.id);
                            if (currReservationExtra == null)
                            {
                                currReservationExtra = new RNT_TBL_EXTRA_RESERVATION();
                                currReservationExtra.pidReservation = currRes.id;
                                currReservationExtra.pidExtra = CommonUtilities.getSYS_SETTING("airbnb_cleaning_id").objToInt32();
                                DC_RENTAL.RNT_TBL_EXTRA_RESERVATION.InsertOnSubmit(currReservationExtra);
                                DC_RENTAL.SubmitChanges();
                            }
                            currReservationExtra.price = reservation.listing_cleaning_fee_accurate;
                            DC_RENTAL.SubmitChanges();
                        }

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
                            _client.nameFirst = reservation.guest_first_name;
                            _client.nameLast = reservation.guest_last_name;
                            _client.nameFull = reservation.guest_first_name + " " + reservation.guest_last_name;
                            _client.contactEmail = currRes.cl_email;
                            _client.contactPhoneMobile = reservation.guest_phone_numbers.ToList().listToString(",");
                            _client.isActive = 1;
                            _client.pidLang = reservation.guest_preferred_locale;
                            dcAuth.SaveChanges();
                            currRes.agentClientID = _client.id;
                        }
                        #endregion

                        DC_RENTAL.SubmitChanges();
                        rntUtils.rntReservation_onChange(currRes);
                    }
                }
            }
        }
        public static string SendRequest(String requesUrl, String requestType, String requestContent, out string ErrorString)
        {
            ErrorString = "";
            try
            {
                //set new TLS protocol 1.1/1.2
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);
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
                    ChnlAirbnbUtils.addLog("Debug", "SendRequest", requestContent, strRSString, requesUrl);
                return strRSString;
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    ErrorString = reader.ReadToEnd();
                    ErrorLog.addLog("", "airbnb authkey", ErrorString);
                    ChnlAirbnbUtils.addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
                    return "";
                }
            }
            catch (Exception ex)
            {
                ErrorString = ex.ToString();
                ErrorLog.addLog("", "airbnb authkey", ex.ToString());
                ChnlAirbnbUtils.addLog("ERROR", "SendRequest", requestContent, ErrorString, requesUrl);
                return "";
            }
        }
        protected void fillHosts()
        {
            magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
            var hostIds = CommonUtilities.getSYS_SETTING("airbnb_host_ids").splitStringToList(",");
            var hosts = DC_AIRBNB.RntChnlAirbnbAuthenticationCodes.Where(x => x.hostId != "" && hostIds.Contains(x.hostId)).ToList();
            foreach (var host in hosts)
            {
                drp_host.Items.Add(new ListItem(host.hostId + " - " + host.name + "", host.hostId + ""));
            }
            //drp_host.Items.Insert(0, new ListItem("----", "0"));
        }
    }
}