using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.webservice
{
    public partial class rnt_estateDettXml : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL; 
        private string CURRENT_SESSION_ID;
        private int IdEstate;
        private RNT_TB_ESTATE currTBL;
        private RNT_LN_ESTATE currLN;
        private int _currLang;
        private long agentID;
        private clSearch _ls;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                CURRENT_SESSION_ID = Request["SESSION_ID"];
                _ls = new clSearch();
                _currLang = Request["lang"].objToInt32() == 0 ? CurrentLang.ID : Request["lang"].objToInt32();
                CurrentLang.ID = _currLang;
                IdEstate = Request["id"].objToInt32();
                currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                currLN = DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == _currLang);
                if (currLN == null)
                    currLN = DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == 2);
                if (currLN == null)
                    currLN = DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == 1);
                if (currTBL == null || currLN == null)
                {
                    Response.Write("<error>No data found</error>");
                    Response.End();
                }
                agentID = Request["agentID"].objToInt64();
                if (Request["action"] == "price")
                {
                    Response.Write(getEstatePrices());
                    Response.End();
                    return;
                }
                if (Request["action"] == "guestbook")
                {
                    Response.Write(getEstateComments());
                    Response.End();
                    return;
                }
                
                int dtStartInt = Request["dtS"].objToInt32();
                int dtEndInt = Request["dtE"].objToInt32();
                if (dtStartInt != 0 && dtEndInt != 0)
                {
                    _ls.dtStart = dtStartInt.JSCal_intToDate();
                    _ls.dtEnd = dtEndInt.JSCal_intToDate();
                }
                _ls.dtCount = (_ls.dtEnd - _ls.dtStart).TotalDays.objToInt32();
                _ls.numPers_adult = Request["numPers_adult"].objToInt32() == 0 ? _ls.numPers_adult : Request["numPers_adult"].objToInt32();
                _ls.numPers_childOver = Request["numPers_childOver"].objToInt32() == 0 ? _ls.numPers_childOver : Request["numPers_childOver"].objToInt32();
                _ls.numPers_childMin = Request["numPers_childMin"].objToInt32() == 0 ? _ls.numPers_childMin : Request["numPers_childMin"].objToInt32();
                _ls.numPersCount = _ls.numPers_adult + _ls.numPers_childOver;
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                _config.lastSearch = _ls;
                _config.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(_config);

                Response.Write(getEstate());
                Response.End();
            }
        }
        private string getEstate()
        {
            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = _ls.dtStart;
            outPrice.dtEnd = _ls.dtEnd;
            outPrice.numPersCount = _ls.numPersCount;
            outPrice.pr_discount_owner = 0;
            outPrice.pr_discount_commission = 0;
            outPrice.part_percentage = currTBL.pr_percentage.objToDecimal();
            decimal prTotal = rntUtils.rntEstate_getPrice(0, currTBL.id, ref outPrice);
            bool isAvv = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 //
                                                                                        && x.pid_estate == IdEstate //
                                                                                        && x.dtStart.HasValue //
                                                                                        && x.dtEnd.HasValue //
                                                                                        && ((x.dtStart.Value.Date <= _ls.dtStart && x.dtEnd.Value.Date >= _ls.dtEnd) //
                                                                                            || (x.dtStart.Value.Date >= _ls.dtStart && x.dtStart.Value.Date < _ls.dtEnd) //
                                                                                            || (x.dtEnd.Value.Date > _ls.dtStart && x.dtEnd.Value.Date <= _ls.dtEnd))).Count() == 0;

            string tmpString = "";
            string returnString = "";
            returnString += "<apt_item>";

            // carica dettagli
            returnString += "<apt_dett>";
            returnString += "<apt_id>" + currTBL.id + "</apt_id>";
            returnString += "<apt_name>" + currLN.title.htmlEncode() + "</apt_name>";
            returnString += "<apt_vote>" + currTBL.importance_vote + "</apt_vote>";
            returnString += "<apt_img>" + App.HOST + "/" + currTBL.img_preview_1 + "</apt_img>";
            returnString += "<zone_id>" + currTBL.pid_zone + "</zone_id>";
            returnString += "<zone_name>" + CurrentSource.locZone_title(currTBL.pid_zone.objToInt32(), _currLang, "").htmlEncode() + "</zone_name>";
            returnString += "<description>" + currLN.mobileDescription.htmlEncode() + "</description>";
            if (!string.IsNullOrEmpty(currTBL.google_maps) && currTBL.google_maps.splitStringToList("|").Count == 2)
                returnString += "<gmap lat=\"" + currTBL.google_maps.splitStringToList("|")[0].Replace(",", ".") + "\" lng=\"" + currTBL.google_maps.splitStringToList("|")[1].Replace(",", ".") + "\" />";
            else
                returnString += "<gmap lat=\"\" lng=\"\" />";
            returnString += "<videoFilePath>" + (!string.IsNullOrEmpty(currTBL.mobileVideoFilePath) ? currTBL.mobileVideoFilePath : "") + "</videoFilePath>";
            returnString += "<has_comments>" + (AppSettings.RNT_TBL_ESTATE_COMMENTs.Where(x => x.isActive == 1 && x.pidEstate == IdEstate).Count() > 0 ? "1" : "0") + "</has_comments>";
            returnString += "</apt_dett>";
            // carica disponibilita
            returnString += "<avv_dett>";
            returnString += "<is_avv>" + (isAvv ? "1" : "0") + "</is_avv>";
            returnString += "<pr_total>" + prTotal.ToString("N2") + "</pr_total>";
            returnString += "<pr_part_advance>" + outPrice.pr_part_payment_total.ToString("N2") + "</pr_part_advance>";
            returnString += "<pr_part_onarrival>" + outPrice.pr_part_owner.ToString("N2") + "</pr_part_onarrival>";
            returnString += "<pr_total_rate>" + (outPrice.prTotalRate).ToString("N2") + "</pr_total_rate>";
            returnString += "<pr_discount_longstay>" + outPrice.prDiscountLongStay.ToString("N2") + "</pr_discount_longstay>";
            returnString += "<pr_discount_specialoffer>" + outPrice.prDiscountSpecialOffer.ToString("N2") + "</pr_discount_specialoffer>";
            returnString += "<pr_agency_fee>" + outPrice.pr_part_agency_fee.ToString("N2") + "</pr_agency_fee>";
            returnString += "<pr_welcome_service>" + outPrice.srsPrice.ToString("N2") + "</pr_welcome_service>";
            returnString += "<pr_cleaning_service>" + outPrice.ecoPrice.ToString("N2") + "</pr_cleaning_service>";
            returnString += "</avv_dett>";

            // carica gallery
            returnString += "<gallery>";
            var gallery = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == "gallery").OrderBy(x => x.sequence);
            foreach (var tmp in gallery)
            {
                returnString += "<foto host=\"" + App.HOST + "\">";
                returnString += "<thumb>" + tmp.img_thumb + "</thumb>";
                returnString += "<big>" + tmp.img_banner + "</big>";
                returnString += "</foto>";
            }
            returnString += "</gallery>";

            // carica accessori
            returnString += "<amenities>";
            List<int> _configIDs_List = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate).Select(x => x.pid_config).ToList();
            var amenities = DC_RENTAL.RNT_TB_CONFIGs.Where(x => _configIDs_List.Contains(x.id));
            foreach (var tmp in amenities)
            {
                returnString += "<item title=\"" + CurrentSource.rnt_configTitle(tmp.id, _currLang, "").htmlEncode() + "\" img=\"" + (tmp.img_thumb + "").Replace("images/estate_config/", "img_estate_config_") + "\" />";
            }
            returnString += "</amenities>";

            // carica letti
            if (currTBL.num_persons_optional.objToInt32() != 0)
                tmpString = "" + CurrentSource.getSysLangValue("lblUpTo") + " " + currTBL.num_persons_max + " " + CurrentSource.getSysLangValue("lblPersons") + " (" + currTBL.num_persons_adult + "+" + currTBL.num_persons_optional + ")";
            else
                tmpString = currTBL.num_persons_max + " " + CurrentSource.getSysLangValue("lblSleeps");
            returnString += "<sleeps title=\"" + tmpString + "\" >";
            if (currTBL.num_bed_single.objToInt32() != 0)
                returnString += "<bed>" + ((currTBL.num_bed_single.objToInt32() > 1) ? currTBL.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsSingle") : currTBL.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedSingle")) + "</bed>";
            if (currTBL.num_bed_double.objToInt32() != 0)
                returnString += "<bed>" + ((currTBL.num_bed_double.objToInt32() > 1) ? currTBL.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDouble") : currTBL.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDouble")) + "</bed>";
            if (currTBL.num_bed_double_divisible.objToInt32() != 0)
                returnString += "<bed>" + ((currTBL.num_bed_double_divisible.objToInt32() > 1) ? currTBL.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDoubleDivisible") : currTBL.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDoubleDivisible")) + "</bed>";
            if (currTBL.num_sofa_double.objToInt32() != 0)
                returnString += "<bed>" + (((currTBL.num_sofa_double.objToInt32()) > 1) ? (currTBL.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsDouble") : (currTBL.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedDouble")) + "</bed>";
            if (currTBL.num_sofa_single.objToInt32() != 0)
                returnString += "<bed>" + (((currTBL.num_sofa_single.objToInt32()) > 1) ? (currTBL.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsSingle") : (currTBL.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedSingle")) + "</bed>";
            returnString += "</sleeps>";

            // carica extra info
            returnString += "<extrainfo>";
            returnString += "<info>" + ((currTBL.num_rooms_bed > 1) ? currTBL.num_rooms_bed + " " + CurrentSource.getSysLangValue("lblBedRooms") : currTBL.num_rooms_bed + " " + CurrentSource.getSysLangValue("lblBedRoom")) + "</info>";
            returnString += "<info>" + ((currTBL.num_rooms_bath > 1) ? currTBL.num_rooms_bath + " " + CurrentSource.getSysLangValue("lblBathRooms") : currTBL.num_rooms_bath + " " + CurrentSource.getSysLangValue("lblBathRoom")) + "</info>";
            returnString += "<info>" + CurrentSource.getSysLangValue("lblStayMin") + " " + ((currTBL.nights_min.objToInt32() > 1) ? currTBL.nights_min + " " + CurrentSource.getSysLangValue("lblNights") : currTBL.nights_min + " " + CurrentSource.getSysLangValue("lblNight")) + "</info>";
            returnString += "</extrainfo>";

            returnString += "</apt_item>";
            return returnString;
        }
        private string getEstatePrices()
        {
            string returnString = "";
            returnString += "<apt_price>";
            // carica prezzi
            returnString += "<pax_price>";
            int pr_basePersons = currTBL.pr_basePersons.objToInt32();
            for (int i = pr_basePersons; i <= currTBL.num_persons_max.objToInt32(); i++)
            {
                int extraPersons = i - pr_basePersons;
                // low
                decimal _pr1 = currTBL.pr_1_2pax.objToDecimal() + (extraPersons * currTBL.pr_1_opt.objToDecimal());
                // hight
                decimal _pr2 = currTBL.pr_2_2pax.objToDecimal() + (extraPersons * currTBL.pr_2_opt.objToDecimal());
                // very hight
                decimal _pr3 = currTBL.pr_3_2pax.objToDecimal() + (extraPersons * currTBL.pr_3_opt.objToDecimal());
                returnString += "<price pax=\"" + (extraPersons == 0 && currTBL.num_persons_min.objToInt32() < pr_basePersons ? currTBL.num_persons_min + "-" : "") + i + " pax\" >";
                returnString += "<night_s1>" + _pr1.ToString("N2") + "</night_s1>";
                returnString += "<night_s2>" + _pr1.ToString("N2") + "</night_s2>";
                returnString += "<night_s3>" + _pr1.ToString("N2") + "</night_s3>";
                returnString += "</price>";
            }
            returnString += "</pax_price>";

            // carica stagioni
            var seasonGroup = currTBL.pidSeasonGroup.objToInt32();
            var seasonDateList = new List<dbRntSeasonDatesTBL>();
            using (DCmodRental dc = new DCmodRental())
                seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= DateTime.Now && x.dtStart <= DateTime.Now.AddYears(1) && x.pidSeasonGroup == seasonGroup).OrderBy(x => x.dtStart).ToList();
            returnString += "<season_list>";
            foreach (var period in seasonDateList)
            {
                returnString += "<period>";
                returnString += "<from date=\"" + period.dtStart.JSCal_dateToString() + "\">" + period.dtStart.formatCustom("#dd# #M# #yy#", CurrentLang.ID, "") + "</from>";
                returnString += "<to date=\"" + period.dtEnd.JSCal_dateToString() + "\">" + period.dtEnd.formatCustom("#dd# #M# #yy#", CurrentLang.ID, "") + "</to>";
                returnString += "<season id=\"" + period.pidPeriod + "\">" + CurrentSource.rntPeriod_title(period.pidPeriod.objToInt32(), CurrentLang.ID, "--Season--") + "</season>";
                returnString += "</period>";
            }
            returnString += "</season_list>";

            // carica offerte
            var _soList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtPublicStart <= DateTime.Now.Date && x.dtPublicEnd >= DateTime.Now.Date && x.pid_estate == IdEstate && x.is_active == 1).OrderBy(x => x.dtEnd).ToList();
            returnString += "<offers_list>";
            foreach (var offers in _soList)
            {
                returnString += "<period>";
                returnString += "<from date=\"" + offers.dtStart.JSCal_dateToString() + "\">" + offers.dtStart.formatCustom("#dd# #M# #yy#", CurrentLang.ID, "") + "</from>";
                returnString += "<to date=\"" + offers.dtEnd.JSCal_dateToString() + "\">" + offers.dtEnd.formatCustom("#dd# #M# #yy#", CurrentLang.ID, "") + "</to>";
                returnString += "<discount>" + offers.pr_discount.objToInt32() + "</discount>";
                returnString += "</period>";
            }
            returnString += "</offers_list>";

            returnString += "<long_stay_discount>";
            if (currTBL.pr_dcSUsed == 1 && currTBL.pr_discount7days.objToInt32() > 0 && contUtils.getLabel_title("lbl_discount7daysDesc", _currLang, "") != "")
            {
                returnString += "<period discount=\"" + currTBL.pr_discount7days.objToInt32() + "\" nights=\"7\">" + contUtils.getLabel_title("lbl_discount7daysDesc", _currLang, "").Replace("#discount#", "" + currTBL.pr_discount7days.objToInt32()) + "</period>";
            }
            else if (currTBL.pr_dcSUsed == 1)
            {
                if (currTBL.pr_dcS2_1_inDays.objToInt32() > 0 && currTBL.pr_dcS2_1_percent.objToInt32() > 0)
                {
                    returnString += "<period discount=\"" + currTBL.pr_dcS2_1_percent.objToInt32() + "\" nights=\"" + (currTBL.pr_dcS2_1_inDays.objToInt32() + 1) + "\">" + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", _currLang, "").Replace("#discount#", "" + currTBL.pr_dcS2_1_percent.objToInt32()).Replace("#nights#", "" + (currTBL.pr_dcS2_1_inDays.objToInt32() + 1)) + "</period>";
                }
                if (currTBL.pr_dcS2_2_inDays.objToInt32() > 0 && currTBL.pr_dcS2_2_percent.objToInt32() > 0)
                {
                    returnString += "<period discount=\"" + currTBL.pr_dcS2_2_percent.objToInt32() + "\" nights=\"" + (currTBL.pr_dcS2_2_inDays.objToInt32() + 1) + "\">" + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", _currLang, "").Replace("#discount#", "" + currTBL.pr_dcS2_2_percent.objToInt32()).Replace("#nights#", "" + (currTBL.pr_dcS2_2_inDays.objToInt32() + 1)) + "</period>";
                }
                if (currTBL.pr_dcS2_3_inDays.objToInt32() > 0 && currTBL.pr_dcS2_3_percent.objToInt32() > 0)
                {
                    returnString += "<period discount=\"" + currTBL.pr_dcS2_3_percent.objToInt32() + "\" nights=\"" + (currTBL.pr_dcS2_3_inDays.objToInt32() + 1) + "\">" + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", _currLang, "").Replace("#discount#", "" + currTBL.pr_dcS2_3_percent.objToInt32()).Replace("#nights#", "" + (currTBL.pr_dcS2_3_inDays.objToInt32() + 1)) + "</period>";
                }
                if (currTBL.pr_dcS2_4_inDays.objToInt32() > 0 && currTBL.pr_dcS2_4_percent.objToInt32() > 0)
                {
                    returnString += "<period discount=\"" + currTBL.pr_dcS2_4_percent.objToInt32() + "\" nights=\"" + (currTBL.pr_dcS2_4_inDays.objToInt32() + 1) + "\">" + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", _currLang, "").Replace("#discount#", "" + currTBL.pr_dcS2_4_percent.objToInt32()).Replace("#nights#", "" + (currTBL.pr_dcS2_4_inDays.objToInt32() + 1)) + "</period>";
                }
                if (currTBL.pr_dcS2_5_inDays.objToInt32() > 0 && currTBL.pr_dcS2_5_percent.objToInt32() > 0)
                {
                    returnString += "<period discount=\"" + currTBL.pr_dcS2_5_percent.objToInt32() + "\" nights=\"" + (currTBL.pr_dcS2_5_inDays.objToInt32() + 1) + "\">" + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", _currLang, "").Replace("#discount#", "" + currTBL.pr_dcS2_5_percent.objToInt32()).Replace("#nights#", "" + (currTBL.pr_dcS2_5_inDays.objToInt32() + 1)) + "</period>";
                }
                if (currTBL.pr_dcS2_6_inDays.objToInt32() > 0 && currTBL.pr_dcS2_6_percent.objToInt32() > 0)
                {
                    returnString += "<period discount=\"" + currTBL.pr_dcS2_6_percent.objToInt32() + "\" nights=\"" + (currTBL.pr_dcS2_6_inDays.objToInt32() + 1) + "\">" + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", _currLang, "").Replace("#discount#", "" + currTBL.pr_dcS2_6_percent.objToInt32()).Replace("#nights#", "" + (currTBL.pr_dcS2_6_inDays.objToInt32() + 1)) + "</period>";
                }
                if (currTBL.pr_dcS2_7_inDays.objToInt32() > 0 && currTBL.pr_dcS2_7_percent.objToInt32() > 0)
                {
                    returnString += "<period discount=\"" + currTBL.pr_dcS2_7_percent.objToInt32() + "\" nights=\"" + (currTBL.pr_dcS2_7_inDays.objToInt32() + 1) + "\">" + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", _currLang, "").Replace("#discount#", "" + currTBL.pr_dcS2_7_percent.objToInt32()).Replace("#nights#", "" + (currTBL.pr_dcS2_7_inDays.objToInt32() + 1)) + "</period>";
                }
            }
            returnString += "</long_stay_discount>";
            returnString += "</apt_price>";
            return returnString;
        }
        private string getEstateComments()
        {
            // carica commenti
            string returnString = "";
            returnString += "<apt_questbook>";
            var commentList = AppSettings.RNT_TBL_ESTATE_COMMENTs.Where(x => x.isActive == 1 && x.pidEstate == IdEstate).OrderByDescending(x => x.dtComment).ToList();
            foreach (var comment in commentList)
            {
                returnString += "<comment>";
                returnString += "<dtComment date=\"" + comment.dtComment.JSCal_dateToString() + "\">" + comment.dtComment.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "") + "</dtComment>";
                returnString += "<cl_name_full>" + comment.cl_name_full.htmlEncode() + "</cl_name_full>";
                returnString += "<cl_country>" + comment.cl_country.htmlEncode() + "</cl_country>";
                returnString += "<vote>" + comment.vote.objToInt32() + "</vote>";
                returnString += "<body>" + (comment.body + "").Replace("<br />", "\n").Replace("<br/>", "\n").Replace("<br>", "\n").htmlDecode().htmlEncode() + "</body>";
                returnString += "</comment>";
            }
            returnString += "</apt_questbook>";
            return returnString;
        }
    }
}
