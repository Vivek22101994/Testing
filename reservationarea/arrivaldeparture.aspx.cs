using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{
    public partial class arrivaldeparture : basePage
    {
        public class TmpPriceClass
        {
            public string tripDirection { get; set; }
            public int id { get; set; }
            public int pax { get; set; }
            public decimal prSingleOneWay { get; set; }
            public decimal prTotalOneWay { get; set; }
            public decimal prSingleRoundTrip { get; set; }
            public decimal prTotalRoundTrip { get; set; }
            public string code { get; set; }
            public string title { get; set; }
            public string imgPreview { get; set; }
            public string paymentType { get; set; }
            public int numGuestMin { get; set; }
            public int numGuestMax { get; set; }
            public TmpPriceClass(int Id, int Pax, decimal PrSingleOneWay, decimal PrTotalOneWay, decimal PrSingleRoundTrip, decimal PrTotalRoundTrip, string TripDirection)
            {
                id = Id;
                pax = Pax;
                prSingleOneWay = PrSingleOneWay;
                prTotalOneWay = PrTotalOneWay;
                prSingleRoundTrip = PrSingleRoundTrip;
                prTotalRoundTrip = PrTotalRoundTrip;
                tripDirection = TripDirection;
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            adminAccess = true;
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                fillData();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "set_transportType", "set_transportType(0, 0);", true);
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            saveData();
        }
        protected void LVtransportType_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (lbl_id == null) return;
            var currChange = lbl_id.Text.objToInt32();
            var currIn = HF_transportTypeIn.Value.objToInt32();
            var currOut = HF_transportTypeOut.Value.objToInt32();
            if (e.CommandName == "inOneWay")
            {
                currIn = currChange;
                if (currOut == currIn) currOut = 0;
            }
            if (e.CommandName == "inRoundTrip")
            {
                currOut = currIn = currChange;
                drp_out_place.setSelectedValue(drp_in_place.SelectedValue);
                rbtList_outPoint_transportType.setSelectedValue(rbtList_inPoint_transportType.SelectedValue);
                drp_out_place.setSelectedValue(drp_in_place.SelectedValue);
                pnl_out_isRequested.Visible = rbtList_outPoint_transportType.SelectedValue.Contains("pickup");
                saveMode("out");
                setMode("out");
                fillTransportTypeList();
            }
            if (e.CommandName == "outOneWay")
            {
                currOut = currChange;
            }
            if (e.CommandName == "outRoundTrip")
            {
                currIn = currOut = currChange;
                drp_in_place.setSelectedValue(drp_out_place.SelectedValue);
                rbtList_inPoint_transportType.setSelectedValue(rbtList_outPoint_transportType.SelectedValue);
                drp_in_place.setSelectedValue(drp_out_place.SelectedValue);
                pnl_in_isRequested.Visible = rbtList_inPoint_transportType.SelectedValue.Contains("pickup");
                saveMode("in");
                setMode("in");
                fillTransportTypeList();
            }

            HF_transportTypeIn.Value = "" + currIn;
            HF_transportTypeOut.Value = "" + currOut;
            pickupRequestSummary(false, false);
        }
        protected USR_TBL_CLIENT getClient(RNT_TBL_RESERVATION currRes)
        {
            USR_TBL_CLIENT tmpClient = new USR_TBL_CLIENT();
            if (currRes.agentID != 0)
            {
                var agClient = authProps.ClientTBL.SingleOrDefault(x => x.id == currRes.agentClientID);
                if (agClient == null)
                    return tmpClient;

                tmpClient.pid_lang = agClient.pidLang.ToInt32();
                tmpClient.name_full = agClient.nameFull;
                tmpClient.name_honorific = agClient.nameHonorific;
                tmpClient.birth_place = agClient.birthPlace;
                tmpClient.birth_date = agClient.birthDate;
                tmpClient.doc_type = agClient.docType;
                tmpClient.doc_cf_num = agClient.docCf;
                tmpClient.doc_vat_num = agClient.docVat;
                tmpClient.doc_num = agClient.docNum;
                tmpClient.doc_issue_place = agClient.docIssuePlace;
                tmpClient.doc_issue_date = agClient.docIssueDate;
                tmpClient.doc_expiry_date = agClient.docExpiryDate;
                tmpClient.loc_address = agClient.locAddress;
                tmpClient.loc_state = agClient.locState;
                tmpClient.loc_zip_code = agClient.locZipCode;
                tmpClient.loc_city = agClient.locCity;
                tmpClient.contact_phone_mobile = agClient.contactPhoneMobile;
                tmpClient.contact_phone_trip = agClient.contactPhoneTrip;
                tmpClient.contact_phone = agClient.contactPhone;
                tmpClient.contact_fax = agClient.contactFax;
                tmpClient.contact_email = agClient.contactEmail;
                tmpClient.loc_country = agClient.locCountry;
                return tmpClient;
            }
            tmpClient = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == currRes.cl_id);
            if (tmpClient == null)
            {
                tmpClient = new USR_TBL_CLIENT();
                tmpClient.pid_lang = currRes.cl_pid_lang;
                tmpClient.name_full = currRes.cl_name_full;
                tmpClient.contact_email = currRes.cl_email;
                tmpClient.loc_country = currRes.cl_loc_country;
            }
            return tmpClient;
        }
        protected void pickupRequestSummary(bool requestOnly, bool saveRequest)
        {
            DateTime inLimo = drp_dtLimoIn.SelectedValue.JSCal_stringToDate();
            if (rtpLimoIn.SelectedDate.HasValue)
                inLimo = inLimo.Add(rtpLimoIn.SelectedDate.Value.TimeOfDay);

            // data partenza da aeroporto
            DateTime outLimo = drp_dtLimoOut.SelectedValue.JSCal_stringToDate();
            if (rtpLimoOut.SelectedDate.HasValue)
                outLimo = outLimo.Add(rtpLimoOut.SelectedDate.Value.TimeOfDay);

            // uscita dal apt
            DateTime dtOut = drp_dtOut.SelectedValue.JSCal_stringToDate();
            if (rtpOut.SelectedDate.HasValue)
                dtOut = dtOut.Add(rtpOut.SelectedDate.Value.TimeOfDay);

            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            string requestString = "<request>";
            requestString += shuttleToken_user();
            requestString += shuttleToken_location(_estTB);

            string tmpStr = "<getReservation>";
            tmpStr += "<saveRequest>" + (saveRequest ? "true" : "false") + "</saveRequest>";
            tmpStr += "<EventID>" + (_currTBL.id) + "</EventID>";
            if (!requestOnly)
            {
                USR_TBL_CLIENT tmpClient = getClient(_currTBL);
                tmpStr += "<pidLang>" + (_currTBL.cl_pid_lang == 1 ? "it" : "en") + "</pidLang>";
                tmpStr += "<nameFull>" + (_currTBL.cl_name_full).urlEncode() + "</nameFull>";
                tmpStr += "<country>" + (_currTBL.cl_loc_country).urlEncode() + "</country>";
                tmpStr += "<email>" + (_currTBL.cl_email).urlEncode() + "</email>";
                tmpStr += "<phone>" + (tmpClient.contact_phone).urlEncode() + "</phone>";
                tmpStr += "<phoneIta>" + (tmpClient.contact_phone_trip).urlEncode() + "</phoneIta>";
                tmpStr += "<phoneMobile>" + (tmpClient.contact_phone_mobile).urlEncode() + "</phoneMobile>";
                tmpStr += "<fax>" + (tmpClient.contact_fax).urlEncode() + "</fax>";
                tmpStr += "<returnUrl>" + (CurrentAppSettings.HOST + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id).urlEncode() + "</returnUrl>";
                //tmpStr += "<EventID>" + (_currTBL.id) + "</EventID>";
            }
            // in 
            tmpStr += "<eventItem>";
            tmpStr += "<uid>" + HF_limo_easyShuttleInUID.Value + "</uid>";
            tmpStr += "<tripDirection>in</tripDirection>";
            if (!requestOnly)
            {
                tmpStr += "<pidTransportType>" + HF_transportTypeIn.Value + "</pidTransportType>";
                tmpStr += "<tripDateTime>" + inLimo.JSCal_dateTimeToString() + "</tripDateTime>";
                tmpStr += "<tripType>inner</tripType>";
                tmpStr += "<tripPlaceID>" + drp_in_place.SelectedValue.Split('|')[1].ToInt32() + "</tripPlaceID>";
                tmpStr += "<tripDetails>" + txt_in_pickupDetails.Text.urlEncode() + "</tripDetails>";
                tmpStr += "<persNumAdult>" + (_currTBL.num_adult.objToInt32() + _currTBL.num_child_over.objToInt32()) + "</persNumAdult>";
                tmpStr += "<persNumChild>" + (_currTBL.num_child_min.objToInt32()) + "</persNumChild>";
                tmpStr += "<caseNumSmall>" + drp_limo_num_case_s.SelectedValue.ToInt32() + "</caseNumSmall>";
                tmpStr += "<caseNumMedium>" + drp_limo_num_case_m.SelectedValue.ToInt32() + "</caseNumMedium>";
                tmpStr += "<caseNumLarge>" + drp_limo_num_case_l.SelectedValue.ToInt32() + "</caseNumLarge>";
            }
            tmpStr += "</eventItem>";
            // out 
            tmpStr += "<eventItem>";
            tmpStr += "<uid>" + HF_limo_easyShuttleOutUID.Value + "</uid>";
            tmpStr += "<tripDirection>out</tripDirection>";
            if (!requestOnly)
            {
                tmpStr += "<pidTransportType>" + HF_transportTypeOut.Value + "</pidTransportType>";
                tmpStr += "<tripDateTime>" + outLimo.JSCal_dateTimeToString() + "</tripDateTime>";
                tmpStr += "<tripType>inner</tripType>";
                tmpStr += "<tripPlaceID>" + drp_out_place.SelectedValue.Split('|')[1].ToInt32() + "</tripPlaceID>";
                tmpStr += "<tripDetails>" + txt_out_pickupDetails.Text.urlEncode() + "</tripDetails>";
                tmpStr += "<persNumAdult>" + (_currTBL.num_adult.objToInt32() + _currTBL.num_child_over.objToInt32()) + "</persNumAdult>";
                tmpStr += "<persNumChild>" + (_currTBL.num_child_min.objToInt32()) + "</persNumChild>";
                tmpStr += "<caseNumSmall>" + drp_limo_num_case_s.SelectedValue.ToInt32() + "</caseNumSmall>";
                tmpStr += "<caseNumMedium>" + drp_limo_num_case_m.SelectedValue.ToInt32() + "</caseNumMedium>";
                tmpStr += "<caseNumLarge>" + drp_limo_num_case_l.SelectedValue.ToInt32() + "</caseNumLarge>";
                tmpStr += "<pickDateTime>" + dtOut.JSCal_dateTimeToString() + "</pickDateTime>";
            }
            tmpStr += "</eventItem>";

            tmpStr += "</getReservation>";
            requestString += tmpStr;


            requestString += "</request>";
            pnl_pickupRequestSummary.Visible = false;
            pnl_pickupRequestSummaryHeader.Visible = false;
            XElement rootElm;
            XDocument xDoc = shuttleRequest(requestString, out rootElm);
            if (xDoc == null)
                return;
            XElement tmpElm;
            rootElm = rootElm.Element("reservation");
            if (rootElm == null) return;
            tmpElm = rootElm.Element("id");
            long id = tmpElm != null ? tmpElm.Value.ToInt64() : 0;
            HF_limo_easyShuttleID.Value = id + "";
            tmpElm = rootElm.Element("uid");
            string uid = tmpElm != null ? tmpElm.Value : "";
            tmpElm = rootElm.Element("prTotal");
            decimal prTotal = tmpElm != null ? tmpElm.Value.Replace(".",",").ToDecimal() : 0;
            tmpElm = rootElm.Element("prForPayment");
            decimal prForPayment = tmpElm != null ? tmpElm.Value.Replace(".",",").ToDecimal() : 0;
            tmpElm = rootElm.Element("paymentUrl");
            HF_limo_paymentUrl.Value = tmpElm != null ? tmpElm.Value.urlDecode() : "";

            tmpStr = "";
            HF_limo_easyShuttleInUID.Value = "";
            HF_limo_easyShuttleOutUID.Value = "";
            foreach (var eventElm in rootElm.Descendants("eventItem"))
            {
                tmpElm = eventElm.Element("uid");
                string eventElm_uid = tmpElm != null ? tmpElm.Value : "";
                tmpElm = eventElm.Element("pidTransportType");
                int eventElm_pidTransportType = tmpElm != null ? tmpElm.Value.ToInt32() : 0;
                tmpElm = eventElm.Element("transportType");
                string eventElm_transportType = tmpElm != null ? tmpElm.Value : "";
                tmpElm = eventElm.Element("tripDirection");
                string eventElm_tripDirection = tmpElm != null ? tmpElm.Value : "";
                tmpElm = eventElm.Element("tripPlaceName");
                string eventElm_tripPlaceName = tmpElm != null ? tmpElm.Value : "";
                tmpElm = eventElm.Element("prIsRoundTrip");
                int eventElm_prIsRoundTrip = tmpElm != null ? tmpElm.Value.ToInt32() : 0;
                tmpElm = eventElm.Element("prIsNcc");
                int eventElm_prIsNcc = tmpElm != null ? tmpElm.Value.ToInt32() : 0;
                tmpElm = eventElm.Element("prSingle");

                decimal eventElm_prSingle = tmpElm != null ? tmpElm.Value.Replace(".",",").ToDecimal() : 0;
                tmpElm = eventElm.Element("prTotal");

                decimal eventElm_prTotal = tmpElm != null ? tmpElm.Value.Replace(".", ",").ToDecimal() : 0;

                tmpStr += "<tr>";
                if (eventElm_tripDirection == "in")
                {
                    tmpStr += "<td>" + eventElm_tripPlaceName + "</td>";
                    tmpStr += "<td>Apartment</td>";
                    HF_transportTypeIn.Value = "" + eventElm_pidTransportType;
                    HF_limo_easyShuttleInUID.Value = eventElm_uid;
                }
                else
                {
                    tmpStr += "<td>Apartment</td>";
                    tmpStr += "<td>" + eventElm_tripPlaceName + "</td>";
                    HF_transportTypeOut.Value = "" + eventElm_pidTransportType;
                    HF_limo_easyShuttleOutUID.Value = eventElm_uid;
                }
                tmpStr += "<td>" + eventElm_transportType + "</td>";
                tmpStr += "<td style=\"text-align: right;\">&euro; " + eventElm_prTotal + "</td>";
                tmpStr += "</tr>";
            }
            if (tmpStr != "")
            {
                pnl_pickupRequestSummary.Visible = true;
                pnl_pickupRequestSummaryHeader.Visible = true;
                tmpStr += "<tr>";
                tmpStr += "<th colspan=\"3\" style=\"text-align: right;\">Total</th>";
                tmpStr += "<th style=\"text-align: right;\">&euro; " + prTotal + "</th>";
                tmpStr += "</tr>";
                if (prForPayment > 0)
                {
                    tmpStr += "<tr>";
                    tmpStr += "<td colspan=\"3\" style=\"text-align: right;\">* to pay online for confirmation</td>";
                    tmpStr += "<td style=\"text-align: right;\">&euro; " + prForPayment + "</td>";
                    tmpStr += "</tr>";
                }
            }
            ltr_pickupRequestSummary.Text = tmpStr;
        }
        protected void Bind_transportType()
        {
            List<LIMO_LK_TRANSPORTTYPE> _list = limoProps.LIMO_LK_TRANSPORTTYPE.Where(x => x.isActive == 1).ToList();
            rbtList_inPoint_transportType.Items.Clear();
            rbtList_outPoint_transportType.Items.Clear();
            foreach (LIMO_LK_TRANSPORTTYPE transportType in _list)
            {
                rbtList_inPoint_transportType.Items.Add(new ListItem(transportType.title, transportType.code));
                rbtList_outPoint_transportType.Items.Add(new ListItem(transportType.title, transportType.code));
            }
            rbtList_inPoint_transportType.Items.Add(new ListItem("Other/Altro", "other"));
            rbtList_outPoint_transportType.Items.Add(new ListItem("Other/Altro", "other"));
        }
        protected void Bind_drp_places()
        {
            List<LIMO_TB_PICKUP_PLACE> _list = limoProps.LIMO_TB_PICKUP_PLACE.Where(x => x.isActive == 1 && x.pidCity == resUtils.currCity && (x.type == "air" || x.type == "sea" || x.type == "train")).OrderBy(x => x.type).ToList();

            // air
            drp_in_place.Items.Clear();
            drp_out_place.Items.Clear();
            foreach (LIMO_TB_PICKUP_PLACE tmp in _list)
            {
                drp_in_place.Items.Add(new ListItem(tmp.title, tmp.type + "|" + tmp.id));
                drp_out_place.Items.Add(new ListItem(tmp.title, tmp.type + "|" + tmp.id));
            }
            drp_in_place.Items.Add(new ListItem("Other", "other|0"));
            drp_out_place.Items.Add(new ListItem("Other", "other|0"));
        }
        protected void fillData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            HF_idZone.Value = _estTB.pid_zone.ToString();
            Bind_drp_places();
            Bind_transportType();

            drp_limo_num_case_s.setSelectedValue(_currTBL.limo_num_case_s);
            drp_limo_num_case_m.setSelectedValue(_currTBL.limo_num_case_m);
            drp_limo_num_case_l.setSelectedValue(_currTBL.limo_num_case_l);

            drp_in_place.setSelectedValue(_currTBL.limo_inPoint_type + "|" + _currTBL.limo_inPoint_pickupPlace);
            txt_in_pickupDetails.Text = _currTBL.limo_inPoint_details;
            rbtList_inPoint_transportType.setSelectedValue(string.IsNullOrEmpty(_currTBL.limo_inPoint_transportType) ? "pickupcar" : _currTBL.limo_inPoint_transportType);
            HF_in_pickupDetailsType.Value = "" + _currTBL.limo_inPoint_detailsType;

            drp_out_place.setSelectedValue(_currTBL.limo_outPoint_type + "|" + _currTBL.limo_outPoint_pickupPlace);
            txt_out_pickupDetails.Text = _currTBL.limo_outPoint_details;
            rbtList_outPoint_transportType.setSelectedValue(string.IsNullOrEmpty(_currTBL.limo_outPoint_transportType) ? "pickupcar" : _currTBL.limo_outPoint_transportType);
            HF_out_pickupDetailsType.Value = "" + _currTBL.limo_outPoint_detailsType;

            // check-in
            DateTime dtStart = _currTBL.dtStart.Value;
            HF_dtMin.Value = dtStart.Add(HF_inTimeDefMin.Value.JSTime_stringToTime()).JSCal_dateTimeToString();
            if (!_currTBL.dtIn.HasValue && !string.IsNullOrEmpty(_currTBL.dtStartTime) && _currTBL.dtStartTime != "000000")
            {
                _currTBL.dtIn = dtStart.Add(_currTBL.dtStartTime.JSTime_stringToTime());
            }
            drp_dtIn.Items.Clear();
            drp_dtIn.Items.Add(new ListItem(dtStart.AddDays(-1).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtStart.AddDays(-1).JSCal_dateToString()));
            drp_dtIn.Items.Add(new ListItem(dtStart.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtStart.JSCal_dateToString()));
            drp_dtIn.Items.Add(new ListItem(dtStart.AddDays(1).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtStart.AddDays(1).JSCal_dateToString()));
            drp_dtIn.setSelectedValue(_currTBL.dtIn.HasValue ? _currTBL.dtIn.JSCal_dateToString() : dtStart.JSCal_dateToString());
            rtpIn.SelectedDate = _currTBL.dtIn;

            // limo-in
            drp_dtLimoIn.Items.Clear();
            drp_dtLimoIn.Items.Add(new ListItem(dtStart.AddDays(-1).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtStart.AddDays(-1).JSCal_dateToString()));
            drp_dtLimoIn.Items.Add(new ListItem(dtStart.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtStart.JSCal_dateToString()));
            drp_dtLimoIn.Items.Add(new ListItem(dtStart.AddDays(1).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtStart.AddDays(1).JSCal_dateToString()));
            drp_dtLimoIn.setSelectedValue(_currTBL.limo_in_datetime.HasValue ? _currTBL.limo_in_datetime.JSCal_dateToString() : dtStart.JSCal_dateToString());
            rtpLimoIn.SelectedDate = _currTBL.limo_in_datetime;
            saveMode("in");
            setMode("in");


            // check-out
            DateTime dtEnd = _currTBL.dtEnd.Value;
            HF_dtMax.Value = dtEnd.Add(HF_outTimeDefMax.Value.JSTime_stringToTime()).JSCal_dateTimeToString();
            if (!_currTBL.dtOut.HasValue && !string.IsNullOrEmpty(_currTBL.dtEndTime) && _currTBL.dtEndTime != "000000")
            {
                _currTBL.dtOut = dtEnd.Add(_currTBL.dtEndTime.JSTime_stringToTime());
            }
            drp_dtOut.Items.Clear();
            drp_dtOut.Items.Add(new ListItem(dtEnd.AddDays(-1).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtEnd.AddDays(-1).JSCal_dateToString()));
            drp_dtOut.Items.Add(new ListItem(dtEnd.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtEnd.JSCal_dateToString()));
            drp_dtOut.Items.Add(new ListItem(dtEnd.AddDays(1).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtEnd.AddDays(1).JSCal_dateToString()));
            drp_dtOut.setSelectedValue(_currTBL.dtOut.HasValue ? _currTBL.dtOut.JSCal_dateToString() : dtEnd.JSCal_dateToString());
            rtpOut.SelectedDate = _currTBL.dtOut;

            // limo-out
            drp_dtLimoOut.Items.Clear();
            drp_dtLimoOut.Items.Add(new ListItem(dtEnd.AddDays(-1).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtEnd.AddDays(-1).JSCal_dateToString()));
            drp_dtLimoOut.Items.Add(new ListItem(dtEnd.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtEnd.JSCal_dateToString()));
            drp_dtLimoOut.Items.Add(new ListItem(dtEnd.AddDays(1).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtEnd.AddDays(1).JSCal_dateToString()));
            drp_dtLimoOut.setSelectedValue(_currTBL.limo_out_datetime.HasValue ? _currTBL.limo_out_datetime.JSCal_dateToString() : dtEnd.JSCal_dateToString());
            rtpLimoOut.SelectedDate = _currTBL.limo_out_datetime;
            saveMode("out");
            setMode("out");

            HF_limo_easyShuttleID.Value = _currTBL.limo_easyShuttleID.objToInt64() + "";
            Guid limo_easyShuttleInUID;
            if (Guid.TryParse(_currTBL.limo_easyShuttleInUID, out limo_easyShuttleInUID))
                HF_limo_easyShuttleInUID.Value = "" + limo_easyShuttleInUID;
            else
                HF_limo_easyShuttleInUID.Value = "";
            Guid limo_easyShuttleOutUID;
            if (Guid.TryParse(_currTBL.limo_easyShuttleOutUID, out limo_easyShuttleOutUID))
                HF_limo_easyShuttleOutUID.Value = "" + limo_easyShuttleOutUID;
            else
                HF_limo_easyShuttleOutUID.Value = "";

            pickupRequestSummary(true, false);
            if (HF_limo_easyShuttleInUID.Value == "" && HF_limo_easyShuttleOutUID.Value == "")
            { HF_limo_easyShuttleID.Value = "0"; HF_limo_easyShuttleSave.Value = "true"; }
            fillTransportTypeList();
            if (HF_limo_easyShuttleID.Value.ToInt64() > 0)
            {
                if (HF_limo_easyShuttleInUID.Value != "")
                {
                    rbtList_inPoint_transportType.Enabled = false;
                    pnl_in_isRequested.Visible = false;
                }
                if (HF_limo_easyShuttleOutUID.Value != "")
                {
                    rbtList_outPoint_transportType.Enabled = false;
                    pnl_out_isRequested.Visible = false;
                }
            }
        }
        protected void saveData()
        {
            if (!rtpLimoIn.SelectedDate.HasValue || !rtpIn.SelectedDate.HasValue || !rtpLimoOut.SelectedDate.HasValue || !rtpOut.SelectedDate.HasValue)
                return;

            bool isStartDateTimeChnaged = false;
            bool isEndDateTimeChnaged = false;

            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            _currTBL.is_dtStartTimeChanged = 1;
            _currTBL.is_dtEndTimeChanged = 1;
            _currTBL.limo_isCompleted = 1;

            _currTBL.limo_num_case_s = drp_limo_num_case_s.SelectedValue.ToInt32();
            _currTBL.limo_num_case_m = drp_limo_num_case_m.SelectedValue.ToInt32();
            _currTBL.limo_num_case_l = drp_limo_num_case_l.SelectedValue.ToInt32();

            saveMode("in");
            _currTBL.limo_inPoint_type = drp_in_place.SelectedValue.Split('|')[0];
            _currTBL.limo_inPoint_transportType = rbtList_inPoint_transportType.SelectedValue;
            _currTBL.limo_inPoint_pickupPlace = drp_in_place.SelectedValue.Split('|')[1].ToInt32();
            _currTBL.limo_inPoint_pickupPlaceName = drp_in_place.getSelectedText("");
            _currTBL.limo_inPoint_details = txt_in_pickupDetails.Text;
            _currTBL.limo_inPoint_detailsType = HF_in_pickupDetailsType.Value.ToInt32();
            //if (_currTBL.limo_inPoint_transportType.Contains("pickup"))
            //    _currTBL.limo_in_isRequested = drp_in_isRequested.getSelectedValueInt(0);
            //else
            //    _currTBL.limo_in_isRequested = 0;

            saveMode("out");
            _currTBL.limo_outPoint_type = drp_out_place.SelectedValue.Split('|')[0];
            _currTBL.limo_outPoint_transportType = rbtList_outPoint_transportType.SelectedValue;
            _currTBL.limo_outPoint_pickupPlace = drp_out_place.SelectedValue.Split('|')[1].ToInt32();
            _currTBL.limo_outPoint_pickupPlaceName = drp_out_place.getSelectedText("");
            _currTBL.limo_outPoint_details = txt_out_pickupDetails.Text;
            _currTBL.limo_outPoint_detailsType = HF_out_pickupDetailsType.Value.ToInt32();
            //if (_currTBL.limo_outPoint_transportType.Contains("pickup"))
            //    _currTBL.limo_out_isRequested = drp_out_isRequested.getSelectedValueInt(0);
            //else
            //    _currTBL.limo_out_isRequested = 0;

            DateTime inLimo = drp_dtLimoIn.SelectedValue.JSCal_stringToDate();
            if (rtpLimoIn.SelectedDate.HasValue)
                inLimo = inLimo.Add(rtpLimoIn.SelectedDate.Value.TimeOfDay);
            DateTime dtIn = drp_dtIn.SelectedValue.JSCal_stringToDate();
            if (rtpIn.SelectedDate.HasValue)
                dtIn = dtIn.Add(rtpIn.SelectedDate.Value.TimeOfDay);

            #region StartTime check if chnaged(To update srs only on date/time chnage)
            if (_currTBL.dtIn != dtIn)
                isStartDateTimeChnaged = true;
            #endregion

            _currTBL.dtStartTime = dtIn.TimeOfDay.JSTime_timeToString();
            _currTBL.dtIn = dtIn;
            _currTBL.is_dtStartTimeChanged = 1;
            _currTBL.limo_in_datetime = inLimo;

            DateTime outLimo = drp_dtLimoOut.SelectedValue.JSCal_stringToDate();
            if (rtpLimoOut.SelectedDate.HasValue)
                outLimo = outLimo.Add(rtpLimoOut.SelectedDate.Value.TimeOfDay);
            DateTime dtOut = drp_dtOut.SelectedValue.JSCal_stringToDate();
            if (rtpOut.SelectedDate.HasValue)
                dtOut = dtOut.Add(rtpOut.SelectedDate.Value.TimeOfDay);

            #region EndTime check if chnaged(To update srs only on date/time chnage)
            if (_currTBL.dtOut != dtOut)
                isEndDateTimeChnaged = true;
            #endregion

            _currTBL.dtEndTime = dtOut.TimeOfDay.JSTime_timeToString();
            _currTBL.dtOut = dtOut;
            _currTBL.is_dtEndTimeChanged = 1;
            _currTBL.limo_out_datetime = outLimo;

            if (HF_limo_easyShuttleSave.Value == "true")
            {
                pickupRequestSummary(false, true);
                _currTBL.limo_easyShuttleID = HF_limo_easyShuttleID.Value.ToInt64();
                _currTBL.limo_easyShuttleInUID = HF_limo_easyShuttleInUID.Value;
                _currTBL.limo_easyShuttleOutUID = HF_limo_easyShuttleOutUID.Value;
            }

            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL,isStartDateTimeChnaged,isEndDateTimeChnaged);
            // todo far arrivare solo 3 gg prima
            //AdminUtilities.rntReservation_mailPartPaymentReceive(_currTBL, _pay, false, false, true, true, true, 1); // send mails
            if (HF_limo_paymentUrl.Value != "")
            {
                Response.Redirect(HF_limo_paymentUrl.Value);
                return;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "callAlert(\"Data was successfully saved. \");", true);
            fillData();
            return;
            if (_currTBL.limo_in_isRequested != 1 && _currTBL.limo_out_isRequested != 1) return;
            if (Request.Url.AbsoluteUri.Contains("http://localhost")) return;
            string _mailBody = "";
            if (_currTBL.limo_in_isRequested == 1)
                _mailBody += "<br/>Pickup da '" + _currTBL.limo_inPoint_pickupPlaceName + "' (" + _currTBL.limo_inPoint_type + ") all'appartamento il " + _currTBL.limo_in_datetime.formatITA(false) + " ore " + _currTBL.limo_in_datetime.Value.TimeOfDay.JSTime_toString(false, true);
            if (_currTBL.limo_out_isRequested == 1)
                _mailBody += "<br/>Pickup dall'appartamento a '" + _currTBL.limo_outPoint_pickupPlaceName + "' (" + _currTBL.limo_outPoint_type + ") il " + _currTBL.limo_out_datetime.formatITA(false) + " ore " + _currTBL.limo_out_datetime.Value.TimeOfDay.JSTime_toString(false, true);
            _mailBody += "<br/><br/>";
            _mailBody += "Per ulteriori informazioni vedere il dettaglio della prenotazione";
            string _mSubject = "";
            _mSubject = "RiR - Richiesta Pickup Service dalla prenotazione #" + _currTBL.code + " - " + _currTBL.cl_name_full;
            //            MailingUtilities.autoSendMailTo(_mSubject, _mailBody, "adilet@magadesign.net", false, "reservationarea.arrivaldeparture a pickupservice@rentalinrome.com");
            MailingUtilities.autoSendMailTo(_mSubject, _mailBody, "pickupservice@rentalinrome.com", false, "reservationarea.arrivaldeparture a pickupservice@rentalinrome.com");
        }
        protected void saveMode(string mode)
        {
            if (mode == "in")
            {
                int pickupDetailsType = 0;
                if (drp_in_place.SelectedValue.Contains("air"))
                {
                    lbl_in_placeDett.InnerText = "Airline / flight n.";
                    //pickupDetailsType = drp_in_air_detailsType.getSelectedValueInt(0).objToInt32();
                }
                if (drp_in_place.SelectedValue.Contains("sea"))
                {
                    lbl_in_placeDett.InnerText = "Shipping company";
                }
                if (drp_in_place.SelectedValue.Contains("train"))
                {
                    lbl_in_placeDett.InnerText = "Train n.";
                }
                if (drp_in_place.SelectedValue.Contains("other"))
                {
                    lbl_in_placeDett.InnerText = "Other place ";
                }
                HF_in_pickupDetailsType.Value = "" + pickupDetailsType;
                pnl_in_isRequested.Visible = rbtList_inPoint_transportType.SelectedValue.Contains("pickup");
            }
            if (mode == "out")
            {
                PH_outAir.Visible = false;
                int pickupDetailsType = 0;
                if (drp_out_place.SelectedValue.Contains("air"))
                {
                    lbl_out_placeDett.InnerText = "Airline / flight n.";
                    PH_outAir.Visible = true;
                    pickupDetailsType = drp_out_air_detailsType.getSelectedValueInt(0).objToInt32();
                }
                if (drp_out_place.SelectedValue.Contains("sea"))
                {
                    lbl_out_placeDett.InnerText = "Shipping company";
                }
                if (drp_out_place.SelectedValue.Contains("train"))
                {
                    lbl_out_placeDett.InnerText = "Train n.";
                }
                if (drp_out_place.SelectedValue.Contains("other"))
                {
                    lbl_out_placeDett.InnerText = "Other place ";
                }
                HF_out_pickupDetailsType.Value = "" + pickupDetailsType;
                pnl_out_isRequested.Visible = rbtList_outPoint_transportType.SelectedValue.Contains("pickup");
            }
        }
        protected void setMode(string mode)
        {
            DateTime dtMin = HF_dtMin.Value.JSCal_stringToDateTime();
            DateTime dtStart = dtMin;
            DateTime dtMax = HF_dtMax.Value.JSCal_stringToDateTime();
            DateTime dtEnd = dtMax;
            if (mode == "in")
            {
                string transportType = rbtList_inPoint_transportType.SelectedValue;
                int pickupPlace = drp_in_place.SelectedValue.Split('|')[1].ToInt32();
                int pickupDetailsType = HF_in_pickupDetailsType.Value.ToInt32();

                DateTime inLimo = drp_dtLimoIn.SelectedValue.JSCal_stringToDate();
                if (rtpLimoIn.SelectedDate.HasValue)
                    inLimo = inLimo.Add(rtpLimoIn.SelectedDate.Value.TimeOfDay);
                TimeSpan pickupDuration = limoUtils.pickupDuration(HF_idZone.Value.ToInt32(), "in", pickupPlace, transportType, inLimo.TimeOfDay.Hours, pickupDetailsType, false);
                DateTime dtIn = drp_dtIn.SelectedValue.JSCal_stringToDate();
                if (rtpIn.SelectedDate.HasValue)
                    dtIn = dtIn.Add(rtpIn.SelectedDate.Value.TimeOfDay);

                if (rtpLimoIn.SelectedDate.HasValue && dtMin < inLimo.Add(pickupDuration))
                    dtMin = inLimo.Add(pickupDuration);
                if (dtIn < dtMin)
                    dtIn = dtMin;

                drp_dtIn.Items.Clear();
                for (int day = 0; day < 3; day++)
                    if (dtMin.Date <= dtStart.Date.AddDays(day)) drp_dtIn.Items.Add(new ListItem(dtStart.Date.AddDays(day).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtStart.Date.AddDays(day).JSCal_dateToString()));
                drp_dtIn.setSelectedValue(dtIn.JSCal_dateToString());
                if (rtpIn.SelectedDate.HasValue)
                {
                    rtpIn.SelectedDate = dtIn;
                    rtpIn.DateInput.SelectedDate = dtIn;
                }
                pnl_inTimeEstimated.Visible = false;
                if (pickupDuration.TotalMinutes != 0 && rtpLimoIn.SelectedDate.HasValue)
                {
                    pnl_inTimeEstimated.Visible = true;
                    HF_inTimeEstimated.Value = HF_inTimeEstimatedFormat.Value.Replace("#time#", inLimo.Add(pickupDuration).TimeOfDay.JSTime_toString(false, true)).Replace("#date#", inLimo.Add(pickupDuration).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""));
                }
                //rtpIn.MinDate = dtMin;
                HF_dtInMin.Value = dtMin.JSCal_dateTimeToString();
            }
            if (mode == "out")
            {
                string transportType = rbtList_outPoint_transportType.SelectedValue;
                int pickupPlace = drp_out_place.SelectedValue.Split('|')[1].ToInt32();
                int pickupDetailsType = HF_out_pickupDetailsType.Value.ToInt32();

                DateTime outLimo = drp_dtLimoOut.SelectedValue.JSCal_stringToDate();
                if (rtpLimoOut.SelectedDate.HasValue)
                    outLimo = outLimo.Add(rtpLimoOut.SelectedDate.Value.TimeOfDay);
                TimeSpan pickupDuration = limoUtils.pickupDuration(HF_idZone.Value.ToInt32(), "out", pickupPlace, transportType, outLimo.TimeOfDay.Hours, pickupDetailsType, drp_out_place.SelectedValue == "air");
                DateTime dtOut = drp_dtOut.SelectedValue.JSCal_stringToDate();
                if (rtpOut.SelectedDate.HasValue)
                    dtOut = dtOut.Add(rtpOut.SelectedDate.Value.TimeOfDay);

                if (rtpLimoOut.SelectedDate.HasValue && dtMax > outLimo.Add(-pickupDuration))
                    dtMax = outLimo.Add(-pickupDuration);
                if (dtOut > dtMax)
                    dtOut = dtMax;
                drp_dtOut.Items.Clear();
                for (int day = 0; day < 3; day++)
                    if (dtMax.Date >= dtEnd.Date.AddDays(-day)) drp_dtOut.Items.Add(new ListItem(dtEnd.Date.AddDays(-day).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""), dtEnd.Date.AddDays(-day).JSCal_dateToString()));
                drp_dtOut.setSelectedValue(dtOut.JSCal_dateToString());
                if (rtpOut.SelectedDate.HasValue)
                {
                    rtpOut.SelectedDate = dtOut;
                    rtpOut.DateInput.SelectedDate = dtOut;
                }
                pnl_outTimeEstimated.Visible = false;
                if (pickupDuration.TotalMinutes != 0 && rtpLimoOut.SelectedDate.HasValue)
                {
                    pnl_outTimeEstimated.Visible = true;
                    HF_outTimeEstimated.Value = HF_outTimeEstimatedFormat.Value.Replace("#time#", outLimo.Add(-pickupDuration).TimeOfDay.JSTime_toString(false, true)).Replace("#date#", outLimo.Add(-pickupDuration).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, ""));
                }
                //rtpOut.MaxDate = dtMax;
                HF_dtOutMax.Value = dtMax.JSCal_dateTimeToString();
            }
        }
        protected void logShuttleRequest(string error)
        {
            string subject = "Errore Shuttle #" + CurrentIdReservation + " " + DateTime.Now;
            MailingUtilities.SendMailToMany(subject, error, new List<string>() { "sviluppo.magadesign@gmail.com" }, new List<string>(), false, "info@rentalinrome.com", "Rir - areares", "Errore Shuttle", true);
        }
        protected XDocument shuttleRequest(string requestString, out XElement rootElm)
        {
            XDocument xDoc = null;
            rootElm = null;
            string responseString = "";
            byte[] buffer = Encoding.UTF8.GetBytes(requestString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.romeasyshuttle.com/webwervice/xmlrequest.aspx");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;
            Stream reqst = request.GetRequestStream(); // add form data to request stream
            reqst.Write(buffer, 0, buffer.Length);
            reqst.Flush();
            reqst.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode.ToString().ToLower() == "ok")
            {
                Stream content = response.GetResponseStream();
                StreamReader contentReader = new StreamReader(content);
                responseString = contentReader.ReadToEnd();
            }
            // per i test
            ltrTmp_shuttleRequest.Text = requestString;
            ltrTmp_shuttleResponse.Text = responseString;
            try
            {
                xDoc = XDocument.Parse(responseString);
            }
            catch (Exception ex)
            {
                logShuttleRequest("<b>Errore:</b><br/>" + ex.Message + "<br/><br/><br/><b>requestString:</b>" + requestString.htmlEncode() + "<br/><br/><br/><b>responseString:</b>" + responseString.htmlEncode());
                return null;
            }
            rootElm = xDoc.Descendants("response").FirstOrDefault();
            if (rootElm == null)
            {
                logShuttleRequest("<b>Errore:</b><br/>Formato Xml Non Valido: Elemento 'response' mancante.<br/><br/><br/>><b>requestString:</b>" + requestString.htmlEncode() + "<br/><br/><br/><b>responseString:</b>" + responseString.htmlEncode());
                return null;
            }
            XElement statusToken = rootElm.Descendants("status").FirstOrDefault();
            if (statusToken == null)
            {
                logShuttleRequest("<b>Errore:</b><br/>Formato Xml Non Valido: Elemento 'status' mancante.<br/><br/><br/>><b>requestString:</b>" + requestString.htmlEncode() + "<br/><br/><br/><b>responseString:</b>" + responseString.htmlEncode());
                return null;
            }
            else if (statusToken.Value != "ok")
            {
                XElement tmpElm = rootElm.Element("errorDesc");
                logShuttleRequest("<b>Errore:</b><br/>Errore Xml: Elemento 'status' != ok.<br/><br/><br/><b>requestString:</b>" + requestString.htmlEncode() + "<br/><br/><br/><b>responseString:</b>" + responseString.htmlEncode());
                return null;
            }
            return xDoc;
        }
        protected void fillTransportTypeList()
        {
            if (HF_limo_easyShuttleID.Value.ToInt64() > 0) return;
            DateTime inLimo = drp_dtLimoIn.SelectedValue.JSCal_stringToDate();
            if (rtpLimoIn.SelectedDate.HasValue)
                inLimo = inLimo.Add(rtpLimoIn.SelectedDate.Value.TimeOfDay);
            getTransportTypeList(inLimo, HF_in_pickupDetailsType.Value == "1" ? "extra" : "inner", drp_in_place.SelectedValue.Split('|')[1].ToInt32(), "in");
            DateTime outLimo = drp_dtLimoOut.SelectedValue.JSCal_stringToDate();
            if (rtpLimoOut.SelectedDate.HasValue)
                outLimo = outLimo.Add(rtpLimoOut.SelectedDate.Value.TimeOfDay);
            getTransportTypeList(outLimo, HF_out_pickupDetailsType.Value == "1" ? "extra" : "inner", drp_out_place.SelectedValue.Split('|')[1].ToInt32(), "out");
            pickupRequestSummary(false, false);
            pnl_luggagesCont.Visible = rbtList_inPoint_transportType.SelectedValue == "pickupcar";
        }
        protected void getTransportTypeList(DateTime tripDateTime, string tripType, int pickupPlace, string tripDirection)
        {
            if (HF_in_pickupPlace_lastLoaded.Value == drp_in_place.SelectedValue.Split('|')[1] && HF_out_pickupPlace_lastLoaded.Value == drp_out_place.SelectedValue.Split('|')[1]) return;
            if (tripDirection == "in") HF_in_pickupPlace_lastLoaded.Value = "" + pickupPlace;
            if (tripDirection == "out") HF_out_pickupPlace_lastLoaded.Value = "" + pickupPlace;
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            string requestString = "<request>";
            requestString += shuttleToken_user();
            requestString += shuttleToken_location(_estTB);
            requestString += shuttleToken_getTransportTypeList(_currTBL, tripDateTime, tripType, pickupPlace, tripDirection, _currTBL.limo_out_isRequested == 1);

            requestString += "</request>";

            XElement rootElm;
            XDocument xDoc = shuttleRequest(requestString, out rootElm);
            if (xDoc == null)
                return;

            XElement tmpElm;
            if (rootElm.Descendants("TransportTypeList").Count() == 0)
            {
                logShuttleRequest("<b>Errore:</b><br/>TransportTypeList namcante<br/><br/><br/>");
                return;
            }
            List<TmpPriceClass> list = new List<TmpPriceClass>();
            foreach (var TransportTypeList in rootElm.Descendants("TransportTypeList"))
            {
                tmpElm = TransportTypeList.Element("tripDirection");
                if (tmpElm == null) continue;
                foreach (var TransportType in TransportTypeList.Descendants("TransportType"))
                {
                    tmpElm = TransportType.Element("id");
                    int id = tmpElm != null ? tmpElm.Value.ToInt32() : 0;
                    tmpElm = TransportType.Element("pax");
                    int pax = tmpElm != null ? tmpElm.Value.ToInt32() : 0;

                    tmpElm = TransportType.Element("prSingleOneWay");                    
                    decimal prSingleOneWay = tmpElm != null ? tmpElm.Value.Replace(".",",").ToDecimal() : 0;                                                                                                                                             
                    ErrorLog.addLog("","aarival-dep prSingleOneWay",tmpElm.Value+"");

                    tmpElm = TransportType.Element("prTotalOneWay");
                    decimal prTotalOneWay = tmpElm != null ? tmpElm.Value.Replace(".", ",").ToDecimal() : 0;
                    ErrorLog.addLog("","aarival-dep prTotalOneWay",tmpElm.Value+"");

                    tmpElm = TransportType.Element("prSingleRoundTrip");
                    decimal prSingleRoundTrip = tmpElm != null ? tmpElm.Value.Replace(".", ",").ToDecimal() : 0;

                    tmpElm = TransportType.Element("prTotalRoundTrip");
                    decimal prTotalRoundTrip = tmpElm != null ? tmpElm.Value.Replace(".",",").ToDecimal() : 0;

                    TmpPriceClass tmp = new TmpPriceClass(id, pax, prSingleOneWay, prTotalOneWay, prSingleRoundTrip, prTotalRoundTrip, tripDirection);
                    tmpElm = TransportType.Element("code");
                    tmp.code = tmpElm != null ? tmpElm.Value : "";
                    tmpElm = TransportType.Element("title");
                    tmp.title = tmpElm != null ? tmpElm.Value : "";
                    tmpElm = TransportType.Element("imgPreview");
                    tmp.imgPreview = tmpElm != null ? tmpElm.Value : "";
                    tmpElm = TransportType.Element("numGuestMin");
                    tmp.numGuestMin = tmpElm != null ? tmpElm.Value.ToInt32() : 0;
                    tmpElm = TransportType.Element("numGuestMax");
                    tmp.numGuestMax = tmpElm != null ? tmpElm.Value.ToInt32() : 0;
                    list.Add(tmp);
                }
            }
            if (tripDirection == "in")
            {
                LVtransportTypeIn.DataSource = list;
                LVtransportTypeIn.DataBind();
                foreach (ListViewDataItem Item in LVtransportTypeIn.Items)
                {
                    Label lbl_id = Item.FindControl("lbl_id") as Label;
                    Label lbl_code = Item.FindControl("lbl_code") as Label;
                    LinkButton lnkOneWay = Item.FindControl("lnkOneWay") as LinkButton;
                    LinkButton lnkRoundTrip = Item.FindControl("lnkRoundTrip") as LinkButton;
                    if (tripDateTime < new DateTime(2012, 6, 1) && lbl_code.Text.Contains("pickup"))
                    {
                        Item.Visible = false;
                        continue;
                    }
                    lnkOneWay.CssClass = "btn_select transportTypePrice transportTypeCont_In_" + lbl_id.Text + "_OneWay";
                    //lnkOneWay.OnClientClick = "set_transportType(" + lbl_id.Text + ", 0); return false;";
                    lnkRoundTrip.CssClass = "btn_select transportTypePrice transportTypeCont_In_" + lbl_id.Text + "_RoundTrip";
                    //lnkRoundTrip.OnClientClick = "set_transportType(" + lbl_id.Text + ", " + lbl_id.Text + "); return false;";
                    //lnkRoundTrip.Visible = HF_in_pickupPlace.Value == HF_out_pickupPlace.Value;
                }
            }
            else
            {
                LVtransportTypeOut.DataSource = list;
                LVtransportTypeOut.DataBind();
                foreach (ListViewDataItem Item in LVtransportTypeOut.Items)
                {
                    Label lbl_id = Item.FindControl("lbl_id") as Label;
                    Label lbl_code = Item.FindControl("lbl_code") as Label;
                    LinkButton lnkOneWay = Item.FindControl("lnkOneWay") as LinkButton;
                    LinkButton lnkRoundTrip = Item.FindControl("lnkRoundTrip") as LinkButton;
                    if (tripDateTime < new DateTime(2012, 6, 1) && lbl_code.Text.Contains("pickup"))
                    {
                        Item.Visible = false;
                        continue;
                    }
                    lnkOneWay.CssClass = "btn_select transportTypePrice transportTypeCont_Out_" + lbl_id.Text + "_OneWay";
                    //lnkOneWay.OnClientClick = "set_transportType(0, " + lbl_id.Text + "); return false;";
                    lnkRoundTrip.CssClass = "btn_select transportTypePrice transportTypeCont_Out_" + lbl_id.Text + "_RoundTrip";
                    //lnkRoundTrip.OnClientClick = "set_transportType(" + lbl_id.Text + ", " + lbl_id.Text + "); return false;";
                    //lnkRoundTrip.Visible = HF_in_pickupPlace.Value == HF_out_pickupPlace.Value;
                }
            }

        }
        public static string shuttleToken_user()
        {
            return "<user><login>rentalinrome</login><password>rome2012</password></user>";
        }
        public static string shuttleToken_location(RNT_TB_ESTATE currEstate)
        {
            return "<location><id>" + currEstate.id + "</id><pidCity>" + currEstate.pid_city + "</pidCity><zipCode>" + currEstate.loc_zip_code.urlEncode() + "</zipCode><locType>apt</locType><locName>" + currEstate.code.urlEncode() + "</locName><locAddressFull>" + currEstate.loc_address.urlEncode() + "</locAddressFull></location>";
        }
        public static string shuttleToken_getTransportTypeList(RNT_TBL_RESERVATION currReservation, DateTime tripDateTime, string tripType, int pickupPlace, string tripDirection, bool IsRoundTrip)
        {
            string tmpStr = "<getTransportTypeList>";
            tmpStr += "<pidLang>" + (currReservation.cl_pid_lang == 1 ? "it" : "en") + "</pidLang>";
            tmpStr += "<persNumAdult>" + (currReservation.num_adult.objToInt32() + currReservation.num_child_over.objToInt32()) + "</persNumAdult>";
            tmpStr += "<persNumChild>" + currReservation.num_child_min.objToInt32() + "</persNumChild>";
            tmpStr += "<persNumWheelchair>" + 0 + "</persNumWheelchair>";
            tmpStr += "<caseNumSmall>" + 0 + "</caseNumSmall>";
            tmpStr += "<caseNumMedium>" + 0 + "</caseNumMedium>";
            tmpStr += "<caseNumLarge>" + 0 + "</caseNumLarge>";
            tmpStr += "<IsRoundTrip>" + (IsRoundTrip ? "1" : "0") + "</IsRoundTrip>";
            tmpStr += "<tripDirection>" + tripDirection + "</tripDirection>";
            tmpStr += "<tripDateTime>" + tripDateTime.JSCal_dateTimeToString() + "</tripDateTime>";
            tmpStr += "<tripType>" + tripType + "</tripType>";
            tmpStr += "<tripPlaceID>" + pickupPlace + "</tripPlaceID>";
            tmpStr += "</getTransportTypeList>";
            return tmpStr;
        }
        protected void drpInMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveMode("in");
            setMode("in");
            fillTransportTypeList();
        }
        protected void drpOutMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveMode("out");
            setMode("out");
            fillTransportTypeList();
        }

        protected void rtpLimoIn_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            setMode("in");
        }
        protected void rtpLimoOut_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            setMode("out");
        }
    }
}