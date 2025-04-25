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
    public partial class rnt_estateListSearch : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        private List<AppSettings.RNT_estate> _estateList;
        private string CURRENT_SESSION_ID;
        private int _currLang;
        private long agentID;
        private bool affiliatesarea;
        private bool mobileXml;
        private string _action;
        private List<int> preferedEstateIds;
        private clSearch currSearch;
        private List<rntExts.RNT_estatePriceDetails> _priceDetails;
        private int totResults;
        private bool foradmin;
        private bool formail;
        private bool showonrequest;
        
        private int _isWL = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/html";
                CURRENT_SESSION_ID = Request["SESSION_ID"];
                foradmin = Request.QueryString["foradmin"] == "true";
                formail = Request.QueryString["formail"] == "true";
                showonrequest = Request.QueryString["showonrequest"] == "true";
                currSearch = new clSearch();
                _currLang = Request["lang"].objToInt32() == 0 ? CurrentLang.ID : Request["lang"].objToInt32();
                _action = Request["action"] + "";
                agentID = Request["agentID"].objToInt64();
                affiliatesarea = Request["affiliatesarea"] == "true";
                mobileXml = Request["mobilexml"] == "true";
                int dtStartInt = Request["dtS"].objToInt32();
                int dtEndInt = Request["dtE"].objToInt32();
                if (dtStartInt != 0 && dtEndInt != 0)
                {
                    currSearch.dtStart = dtStartInt.JSCal_intToDate();
                    currSearch.dtEnd = dtEndInt.JSCal_intToDate();
                }
                currSearch.dtCount = (currSearch.dtEnd - currSearch.dtStart).TotalDays.objToInt32();
                currSearch.numPers_adult = Request["numPers_adult"].objToInt32() == 0 ? currSearch.numPers_adult : Request["numPers_adult"].objToInt32();
                currSearch.numPers_childOver = Request["numPers_childOver"].objToInt32() == 0 ? currSearch.numPers_childOver : Request["numPers_childOver"].objToInt32();
                currSearch.numPers_childMin = Request["numPers_childMin"].objToInt32() == 0 ? currSearch.numPers_childMin : Request["numPers_childMin"].objToInt32();
                currSearch.numPersCount = currSearch.numPers_adult + currSearch.numPers_childOver;
                currSearch.currPage = Request["currPage"].objToInt32();
                currSearch.numPerPage = Request["numPerPage"].objToInt32() == 0 ? 10 : Request["numPerPage"].objToInt32();
                currSearch.searchTitle = (Request["title"] + "").Trim().ToLower().urlDecode();

                List<string> prRange = Request["prRange"].splitStringToList("|");
                currSearch.prMin = prRange.Count > 0 ? prRange[0].ToInt32() : 0;
                currSearch.prMax = prRange.Count > 1 ? prRange[1].ToInt32() : 9999999;

                List<string> voteRange = Request["voteRange"].splitStringToList("|");
                currSearch.voteMin = voteRange.Count > 0 ? voteRange[0].ToInt32() : 0;
                currSearch.voteMax = voteRange.Count > 1 ? voteRange[1].ToInt32() : 10;

                currSearch.currZoneList = Request["currZoneList"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).ToList();
                currSearch.currConfigList = Request["currConfigList"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).ToList();
                currSearch.orderBy = Request["orderBy"] + "";
                currSearch.orderHow = Request["orderHow"] + "";

                preferedEstateIds = Request["preflist"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).Where(x => x != 0).ToList();
                _isWL = Request["isWL"].objToInt32();

                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                _config.lastSearch = currSearch;
                _config.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(_config);

                if (foradmin)
                    Response.Write(getEstateListAdmin());
                else
                    Response.Write(getEstateList());
                Response.End();
            }
        }
        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList
        {
            get
            {
                using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                {
                    List<int> estatesNotAvv = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 //
                                                                                                && x.dtStart.HasValue //
                                                                                                && x.dtEnd.HasValue //
                                                                                                && ((x.dtStart.Value.Date <= currSearch.dtStart && x.dtEnd.Value.Date >= currSearch.dtEnd) //
                                                                                                    || (x.dtStart.Value.Date >= currSearch.dtStart && x.dtStart.Value.Date < currSearch.dtEnd) //
                                                                                                    || (x.dtEnd.Value.Date > currSearch.dtStart && x.dtEnd.Value.Date <= currSearch.dtEnd))).Select(x => x.pid_estate.objToInt32()).ToList();
                    List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.Where(x => !estatesNotAvv.Contains(x.id)
                                                                                                && ((currSearch.numPers_childOver + currSearch.numPers_childMin) == 0 || x.is_chidren_allowed == 1)
                                                                                                && x.num_persons_max >= currSearch.numPersCount
                                                                                                && x.num_persons_min <= currSearch.numPersCount
                                                                                                && x.num_persons_child >= currSearch.numPers_childMin
                                                                                                && x.nights_min <= currSearch.dtCount
                                                                                                && (x.nights_max == 0 || x.nights_max >= currSearch.dtCount)//
                                                                                                ).ToList();
                    #region Check White Label
                    if (_isWL == 1)
                    {
                        List<int> lstAgentEstate = new List<int>();
                        using (DCmodRental dc = new DCmodRental())
                        {
                            lstAgentEstate = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == App.WLAgentId).Select(x => x.pidEstate).Distinct().ToList();
                        }
                        estateList = estateList.Where(x => lstAgentEstate.Contains(x.id)).ToList();
                        agentID = App.WLAgentId;
                    }
                    #endregion

                    // agenzie vedono solo online_booking
                    if (affiliatesarea)
                        estateList = estateList.Where(x => x.is_online_booking == 1).ToList();
                    dbRntAgentTBL agentTbl = null;
                    decimal agentTotalResPrice = 0;
                    if (agentID != 0)
                    {
                        using (DCmodRental dc = new DCmodRental())
                        {
                            agentTbl = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentID);
                            if (agentTbl != null)
                            {
                                DateTime checkDate = DateTime.Now;
                                DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                                DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                                var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTbl.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                                agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                            }
                        }
                    }
                    List<int> removeIds = new List<int>();
                    foreach (AppSettings.RNT_estate _rntEst in estateList)
                    {
                        RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == _currLang && !string.IsNullOrEmpty(x.title));
                        if (_lang == null) { removeIds.Add(_rntEst.id); continue; }
                        _rntEst.pid_lang = _currLang;
                        _rntEst.title = _lang.title;
                        _rntEst.summary = _lang.summary;
                        _rntEst.page_path = _lang.page_path;
                        rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                        outPrice.dtStart = currSearch.dtStart;
                        outPrice.dtEnd = currSearch.dtEnd;
                        outPrice.numPersCount = currSearch.numPersCount;
                        outPrice.pr_discount_owner = 0;
                        outPrice.pr_discount_commission = 0;
                        outPrice.fillAgentDetails(agentTbl);
                        outPrice.agentTotalResPrice = agentTotalResPrice;
                        outPrice.part_percentage = _rntEst.pr_percentage;

                        if (_isWL == 1 && agentTbl != null)
                        {
                            outPrice.isWL = 1;
                            outPrice.WL_changeAmount = agentTbl.WL_changeAmount.objToInt32();
                            outPrice.WL_changeIsDiscount = agentTbl.WL_changeIsDiscount == null ? 0 : agentTbl.WL_changeIsDiscount.objToInt32();
                            outPrice.WL_changeIsPercentage = agentTbl.WL_changeIsPercentage == null ? 1 : agentTbl.WL_changeIsPercentage.objToInt32();

                            outPrice.WL_changeAmount_Agent = agentTbl.WL_changeAmount_Agent.objToInt32();
                            outPrice.WL_changeIsDiscount_Agent = agentTbl.WL_changeIsDiscount_Agent == null ? 0 : agentTbl.WL_changeIsDiscount_Agent.objToInt32();
                            outPrice.WL_changeIsPercentage_Agent = agentTbl.WL_changeIsPercentage_Agent == null ? 1 : agentTbl.WL_changeIsPercentage_Agent.objToInt32();
                        }

                        _rntEst.price = rntUtils.rntEstate_getPrice(0, _rntEst.id, ref outPrice);
                        if (outPrice.prDiscountSpecialOffer > 0)
                            _rntEst.priceNoDiscount = _rntEst.price + outPrice.prDiscountSpecialOffer + outPrice.prDiscountLongStay;
                        _rntEst.pr_agentID = outPrice.agentID;
                        _rntEst.pr_agentCommissionPrice = outPrice.agentCommissionPrice;
                        _rntEst.priceError = outPrice.outError;
                        _rntEst.prTotalCommission = outPrice.prTotalCommission;
                        _rntEst.priceReservation = outPrice.pr_reservation;
                        _rntEst.priceEco = outPrice.ecoPrice;
                        _rntEst.priceSrs = outPrice.srsPrice;
                        _rntEst.priceAgencyFee = outPrice.pr_part_agency_fee;
                        _rntEst.nicePrice = outPrice.prDiscountSpecialOffer > 0;
                        _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), _currLang, "");
                    }
                    estateList.RemoveAll(x => removeIds.Contains(x.id));
                    return estateList.Where(x => x.page_path != "").ToList();//.Select(x => x.Clone()).ToList();
                }
            }
        }
        private string getEstateList()
        {
            string orderBy_title = "";
            string orderBy_vote = "";
            string orderBy_price = "";
            int hf_voteMin = 0;
            int hf_voteMax = 0;
            int hf_prMin = 0;
            int hf_prMax = 0;
            _estateList = CURRENT_RNT_estateList;
            // agenzie vedono solo apt prenotabili (con prezzo)
            if (affiliatesarea)
                _estateList = _estateList.Where(x => x.price > 0).ToList();


            if (_estateList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                _errorStr = _errorStr.Replace("#hf_voteMin#", "" + hf_voteMin);
                _errorStr = _errorStr.Replace("#hf_voteMax#", "" + hf_voteMax);
                _errorStr = _errorStr.Replace("#hf_voteRange#", "" + currSearch.voteMin + "|" + currSearch.voteMax);
                _errorStr = _errorStr.Replace("#hf_prMin#", "" + hf_prMin);
                _errorStr = _errorStr.Replace("#hf_prMax#", "" + hf_prMax);
                _errorStr = _errorStr.Replace("#hf_prRange#", "" + currSearch.prMin + "|" + currSearch.prMax);
                return _errorStr;
            }

            // hf_vote
            hf_voteMin = _estateList.Min(x => x.importance_vote.objToInt32());
            hf_voteMax = _estateList.Max(x => x.importance_vote.objToInt32());

            List<AppSettings.RNT_estate> _listWithPrice = _estateList.Where(x => x.price != 0).OrderByDescending(x => x.is_online_booking).ThenBy(x => x.sequence).ThenBy(x => x.price).ToList();
            List<AppSettings.RNT_estate> _listOnRequest = _estateList.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();

            if (_listWithPrice.Count != 0)
            {
                // hf_pr
                hf_prMin = _listWithPrice.Min(x => x.price.objToInt32());
                hf_prMax = _listWithPrice.Max(x => x.price.objToInt32());
            }
            // filtra per prezzo
            if (currSearch.prMin != 0)
                _listWithPrice = _listWithPrice.Where(x => x.price >= currSearch.prMin).ToList();
            if (currSearch.prMax != 0)
                _listWithPrice = _listWithPrice.Where(x => x.price <= currSearch.prMax).ToList();

            // per mobile solo con prezzo e solo onlinebooking
            if (mobileXml)
            { _listOnRequest = new List<AppSettings.RNT_estate>(); _listWithPrice = _listWithPrice.Where(x => x.is_online_booking == 1 && (preferedEstateIds.Count == 0 || preferedEstateIds.Contains(x.id))).ToList(); }

            // ordina per prezzo
            if (currSearch.orderBy == "price" && currSearch.orderHow == "asc")
            { _listWithPrice = _listWithPrice.OrderBy(x => x.price).ToList(); orderBy_price = "asc"; }
            if (currSearch.orderBy == "price" && currSearch.orderHow == "desc")
            { _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ToList(); orderBy_price = "desc"; }

            _estateList = new List<AppSettings.RNT_estate>();
            _estateList.AddRange(_listWithPrice);
            _estateList.AddRange(_listOnRequest);

            if (_estateList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                _errorStr = _errorStr.Replace("#hf_voteMin#", "" + hf_voteMin);
                _errorStr = _errorStr.Replace("#hf_voteMax#", "" + hf_voteMax);
                _errorStr = _errorStr.Replace("#hf_voteRange#", "" + currSearch.voteMin + "|" + currSearch.voteMax);
                _errorStr = _errorStr.Replace("#hf_prMin#", "" + hf_prMin);
                _errorStr = _errorStr.Replace("#hf_prMax#", "" + hf_prMax);
                _errorStr = _errorStr.Replace("#hf_prRange#", "" + currSearch.prMin + "|" + currSearch.prMax);
                return _errorStr;
            }

            // filtri
            if (currSearch.voteMin != 0)
                _estateList = _estateList.Where(x => x.importance_vote >= currSearch.voteMin).ToList();
            if (currSearch.voteMax != 0)
                _estateList = _estateList.Where(x => x.importance_vote <= currSearch.voteMax).ToList();


            if (!string.IsNullOrEmpty(currSearch.searchTitle))
            {
                var tmpListTitleFilter = _estateList.Where(x => !string.IsNullOrEmpty(x.title) && x.title.ToLower().Contains(currSearch.searchTitle)).ToList();
                if (tmpListTitleFilter.Count > 0)
                    _estateList = tmpListTitleFilter;
            }
            if (!currSearch.currZoneList.Contains(0) && currSearch.currZoneList.Count > 0)
                _estateList = _estateList.Where(x => currSearch.currZoneList.Contains(x.pid_zone)).ToList();
            foreach (int _conf in currSearch.currConfigList)
            {
                _estateList = _estateList.Where(x => x.configList.Contains(_conf)).ToList();
            }

            // ordina per voto
            if (currSearch.orderBy == "vote" && currSearch.orderHow == "asc")
            { _estateList = _estateList.OrderBy(x => x.importance_vote).ToList(); orderBy_vote = "asc"; }
            if (currSearch.orderBy == "vote" && currSearch.orderHow == "desc")
            { _estateList = _estateList.OrderByDescending(x => x.importance_vote).ToList(); orderBy_vote = "desc"; }
            // ordina per titolo
            if (currSearch.orderBy == "title" && currSearch.orderHow == "asc")
            { _estateList = _estateList.OrderBy(x => x.title).ToList(); orderBy_title = "asc"; }
            if (currSearch.orderBy == "title" && currSearch.orderHow == "desc")
            { _estateList = _estateList.OrderByDescending(x => x.title).ToList(); orderBy_title = "desc"; }

            // set defaults, per aggiustare slider
            if (hf_prMin > currSearch.prMin || currSearch.prMin == 0) currSearch.prMin = hf_prMin;
            if (hf_prMax < currSearch.prMax || currSearch.prMax == 0) currSearch.prMax = hf_prMax;
            if (hf_voteMin > currSearch.voteMin || currSearch.voteMin == 0) currSearch.voteMin = hf_voteMin;
            if (hf_voteMax < currSearch.voteMax || currSearch.voteMax == 0) currSearch.voteMax = hf_voteMax;

            int _allCount = _estateList.Count;
            int _currCount = _allCount;
            if (_currCount == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                _errorStr = _errorStr.Replace("#hf_voteMin#", "" + hf_voteMin);
                _errorStr = _errorStr.Replace("#hf_voteMax#", "" + hf_voteMax);
                _errorStr = _errorStr.Replace("#hf_voteRange#", "" + currSearch.voteMin + "|" + currSearch.voteMax);
                _errorStr = _errorStr.Replace("#hf_prMin#", "" + hf_prMin);
                _errorStr = _errorStr.Replace("#hf_prMax#", "" + hf_prMax);
                _errorStr = _errorStr.Replace("#hf_prRange#", "" + currSearch.prMin + "|" + currSearch.prMax);
                return _errorStr;
            }
            if (currSearch.currPage != 0)
            {
                int _pageCount = 1;
                int _currPageCount = currSearch.numPerPage;
                while (_currCount > _currPageCount)
                {
                    _pageCount++;
                    _currCount -= currSearch.numPerPage;
                }
                if (_pageCount < currSearch.currPage) currSearch.currPage = _pageCount;
                if (_pageCount == currSearch.currPage && _currCount < _currPageCount) _currPageCount = _currCount;
                _estateList = _estateList.GetRange((currSearch.currPage - 1) * currSearch.numPerPage, _currPageCount);
            }
            string _pager = "<div class=\"ordina pageCount\">" + CommonUtilities.getPager(_allCount, currSearch.numPerPage, currSearch.currPage, 10, "<span class=\"page\">" + CurrentSource.getSysLangValue("listAllPages") + "</span>", "<a class=\"page\" href=\"javascript:RNT_changePage(0)\">" + CurrentSource.getSysLangValue("listAllPages") + "</a>", "<span class=\"page\">{0}</span>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">{0}</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>") + "</div>";
            int pagesTotalCount = 1;
            int tmpAllCount = _allCount;
            while (tmpAllCount > currSearch.numPerPage)
            {
                pagesTotalCount++;
                tmpAllCount -= currSearch.numPerPage;
            }
            string itemPlaceHolder = "";
            itemPlaceHolder += "";
            string _ItemTemplate = ltrItemTemplate.Text;
            bool alternate = false;
            foreach (AppSettings.RNT_estate _rntEst in _estateList)
            {
                string _ItemTemplateSingle = "";
                if (affiliatesarea)
                {
                    _ItemTemplateSingle = ltrItemTemplate_affiliatesarea.Text;
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#id#", "" + _rntEst.id);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#cssClass#", (alternate ? "alternate" : ""));
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#title#", "" + _rntEst.title);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#page_path#", "" + _rntEst.page_path);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#zoneTitle#", "" + _rntEst.zone);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#vote#", "" + _rntEst.importance_vote);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", _rntEst.price.ToString("N2"));
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#pr_agentCommissionPrice#", _rntEst.pr_agentCommissionPrice.ToString("N2"));
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#bookingLink#", "IdEstate=" + _rntEst.id + "&dtS=" + currSearch.dtStart.JSCal_dateToString() + "&dtE=" + currSearch.dtEnd.JSCal_dateToString() + "&numPers_adult=" + currSearch.numPers_adult + "&numPers_childOver=" + currSearch.numPers_childOver + "&numPers_childMin=" + currSearch.numPers_childMin);

                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#pdflinkid#", "_" + _rntEst.id);
                    string url_pdf = App.HOST + "/pg_rntEstatePdf.aspx?id=" + _rntEst.id + "&agentid=" + agentID + "&gallery=original&mk=";
                    string filename = _rntEst.title.Replace("'", "").clearPathName() + ".pdf";
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#generatepdf#", url_pdf.urlEncode());
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#fname#", filename.urlEncode());
                        
                }
                else if (mobileXml)
                {
                    _ItemTemplateSingle = ltrItemTemplate_mobileXml.Text;
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#id#", "" + _rntEst.id);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_1#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_1) ? _rntEst.img_preview_1 : "images/css/default-apt-list.jpg"));
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#title#", "" + _rntEst.title);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#page_path#", "" + _rntEst.page_path);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#zoneId#", "" + _rntEst.pid_zone);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#zoneTitle#", "" + _rntEst.zone);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#summary#", "" + _rntEst.summary);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#vote#", "" + _rntEst.importance_vote);
                    if (_rntEst.price > 0)
                    {
                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", _rntEst.price.ToString("N2"));
                    }
                    else
                        continue;
                }
                else
                {
                    _ItemTemplateSingle = _ItemTemplate.Clone() as string;
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#id#", "" + _rntEst.id);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_1#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_1) ? _rntEst.img_preview_1 : "images/css/default-apt-list.jpg"));
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_2#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_2) ? _rntEst.img_preview_2 : "images/css/default-apt-list.jpg"));
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_3#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_3) ? _rntEst.img_preview_3 : "images/css/default-apt-list.jpg"));
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#title#", "" + _rntEst.title);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#page_path#", "" + _rntEst.page_path);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#zoneTitle#", "" + _rntEst.zone);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#summary#", "" + _rntEst.summary);
                    
                    if(_isWL == 0)
                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#onlineBooking#", "" + ((_rntEst.is_online_booking == 1) ? "<img src=\"/images/css/book-now.gif\" class=\"ico_book_list\">" : ""));
                    else
                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#onlineBooking#", "" + ((_rntEst.is_online_booking == 1) ? "<img src=\"/WLRental/images/book-now.gif\" class=\"ico_book_list\">" : ""));
                        

                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#vote#", "" + _rntEst.importance_vote);
                    string priceTemplate = "";

                    if (_rntEst.price > 0)
                    {
                        if (_rntEst.pr_agentID != 0 && _rntEst.pr_agentCommissionPrice != 0 && _isWL == 0)
                            priceTemplate = ltrPriceTemplate_agency.Text.Replace("#price#", _rntEst.price.ToString("N2")).Replace("#pr_agentCommissionPrice#", _rntEst.pr_agentCommissionPrice.ToString("N2")).Replace("#lblYourCommission#", CurrentSource.getSysLangValue("lblYourCommission"));
                        else
                        {
                            priceTemplate = ltrPriceTemplate_ok.Text.Replace("#price#", _rntEst.price.ToString("N2")).Replace("#priceWithoutDiscount#", (_rntEst.priceNoDiscount > 0) ? ltrPrezzoBarrato.Text.Replace("#price#", _rntEst.priceNoDiscount.ToString("N2")).Replace("#lblDiscountSpecialOffer#", CurrentSource.getSysLangValue("lblDiscountSpecialOffer")) : "");
                        }
                    }
                    else if (_rntEst.priceError == 1)
                        priceTemplate = ltrPriceTemplate_error1.Text.Replace("#lblOnRequest#", CurrentSource.getSysLangValue("lblOnRequest")).Replace("#lblMinStayVHSeason#", CurrentSource.getSysLangValue("lblMinStayVHSeason").Replace("5", "" + _rntEst.nights_minVHSeason));
                    else
                        priceTemplate = ltrPriceTemplate_request.Text.Replace("#lblOnRequest#", CurrentSource.getSysLangValue("lblOnRequest"));

                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", priceTemplate);
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("##", "" + _rntEst.id);
                }
                itemPlaceHolder += _ItemTemplateSingle;
            }
            string fltDetails = "";
            fltDetails += currSearch.dtStart.formatCustom("#DD#, #dd# #MM# #yy#", _currLang, "") + " - " + currSearch.dtEnd.formatCustom("#DD#, #dd# #MM# #yy#", _currLang, "");
            fltDetails += "<br/>" + CurrentSource.getSysLangValue("lblPersons") + ": " + currSearch.numPersCount;
            string LayoutTemplate = ltrLayoutTemplate.Text;
            if (affiliatesarea) LayoutTemplate = ltrLayoutTemplate_affiliatesarea.Text;
            if (mobileXml) LayoutTemplate = ltrLayoutTemplate_mobileXml.Text;
            LayoutTemplate = LayoutTemplate.Replace("#orderBy#", currSearch.orderBy);
            LayoutTemplate = LayoutTemplate.Replace("#orderHow#", currSearch.orderHow);
            LayoutTemplate = LayoutTemplate.Replace("#listType#", currSearch.listType);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_title#", orderBy_title);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_vote#", orderBy_vote);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_price#", orderBy_price);
            LayoutTemplate = LayoutTemplate.Replace("#currPage#", currSearch.currPage.ToString());
            LayoutTemplate = LayoutTemplate.Replace("#pagesTotalCount#", pagesTotalCount.ToString());
            
            LayoutTemplate = LayoutTemplate.Replace("#itemPlaceHolder#", itemPlaceHolder);
            LayoutTemplate = LayoutTemplate.Replace("#fltDetails#", fltDetails);
            LayoutTemplate = LayoutTemplate.Replace("#pagerPlaceHolder#", _pager);
            LayoutTemplate = LayoutTemplate.Replace("#flt_type#", CurrentSource.getSysLangValue("estEstateType_" + currSearch.currType, _currLang, "#flt_type#"));
            LayoutTemplate = LayoutTemplate.Replace("#estEstateType#", CurrentSource.getSysLangValue("estEstateType", _currLang, "#estEstateType#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblOrderBy#", CurrentSource.getSysLangValue("lblOrderBy", _currLang, "#lblOrderBy#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblName#", CurrentSource.getSysLangValue("lblName", _currLang, "#lblName#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblPrice#", CurrentSource.getSysLangValue("lblPrice", _currLang, "#lblPrice#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblRating#", CurrentSource.getSysLangValue("lblRating", _currLang, "#lblRating#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblViewDetails#", CurrentSource.getSysLangValue("lblViewDetails", _currLang, "#lblViewDetails#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblYourCommission#", CurrentSource.getSysLangValue("lblYourCommission"));

            // range slider values
            int hf_voteRangeMin = currSearch.voteMin;
            if (hf_voteRangeMin < hf_voteMin) hf_voteRangeMin = hf_voteMin;
            int hf_voteRangeMax = currSearch.voteMax;
            if (hf_voteRangeMax > hf_voteMax) hf_voteRangeMax = hf_voteMax;
            string hf_voteRange = hf_voteRangeMin + "|" + hf_voteRangeMax;
            LayoutTemplate = LayoutTemplate.Replace("#hf_voteMin#", "" + hf_voteMin);
            LayoutTemplate = LayoutTemplate.Replace("#hf_voteMax#", "" + hf_voteMax);
            LayoutTemplate = LayoutTemplate.Replace("#hf_voteRange#", "" + hf_voteRange);

            int hf_prRangeMin = currSearch.prMin;
            if (hf_prRangeMin < hf_prMin) hf_prRangeMin = hf_prMin;
            int hf_prRangeMax = currSearch.prMax;
            if (hf_prRangeMax > hf_prMax) hf_prRangeMax = hf_prMax;

            string hf_prRange = hf_prRangeMin + "|" + hf_prRangeMax;
            LayoutTemplate = LayoutTemplate.Replace("#hf_prMin#", "" + hf_prMin);
            LayoutTemplate = LayoutTemplate.Replace("#hf_prMax#", "" + hf_prMax);
            LayoutTemplate = LayoutTemplate.Replace("#hf_prRange#", "" + hf_prRange);
            return LayoutTemplate;
        }
        private string getEstateListAdmin()
        {
            string orderBy_title = "";
            string orderBy_vote = "";
            string orderBy_price = "";
            int hf_voteMin = 0;
            int hf_voteMax = 0;
            int hf_prMin = 0;
            int hf_prMax = 0;
            _estateList = CURRENT_RNT_estateList;
            // agenzie vedono solo apt prenotabili (con prezzo)
            if (affiliatesarea)
                _estateList = _estateList.Where(x => x.price > 0).ToList();

            //agency filter
            //if (agentID != 0)
            //{
            //    using (DCmodRental dc = new DCmodRental())
            //    {
            //        List<int> lstAgentEstate = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID).Select(x => x.pidEstate).ToList();
            //        _estateList = _estateList.Where(x => lstAgentEstate.Contains(x.id)).ToList();
            //    }
            //}

            if (_estateList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                _errorStr = _errorStr.Replace("#hf_voteMin#", "" + hf_voteMin);
                _errorStr = _errorStr.Replace("#hf_voteMax#", "" + hf_voteMax);
                _errorStr = _errorStr.Replace("#hf_voteRange#", "" + currSearch.voteMin + "|" + currSearch.voteMax);
                _errorStr = _errorStr.Replace("#hf_prMin#", "" + hf_prMin);
                _errorStr = _errorStr.Replace("#hf_prMax#", "" + hf_prMax);
                _errorStr = _errorStr.Replace("#hf_prRange#", "" + currSearch.prMin + "|" + currSearch.prMax);
                return _errorStr;
            }

            // hf_vote
            hf_voteMin = _estateList.Min(x => x.importance_vote.objToInt32());
            hf_voteMax = _estateList.Max(x => x.importance_vote.objToInt32());

            List<AppSettings.RNT_estate> _listWithPrice = _estateList.Where(x => x.price != 0).OrderByDescending(x => x.is_online_booking).ThenBy(x => x.sequence).ThenBy(x => x.price).ToList();
            List<AppSettings.RNT_estate> _listOnRequest = _estateList.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();

            if (_listWithPrice.Count != 0)
            {
                // hf_pr
                hf_prMin = _listWithPrice.Min(x => x.price.objToInt32());
                hf_prMax = _listWithPrice.Max(x => x.price.objToInt32());
            }
            // filtra per prezzo
            if (currSearch.prMin != 0)
                _listWithPrice = _listWithPrice.Where(x => x.price >= currSearch.prMin).ToList();
            if (currSearch.prMax != 0)
                _listWithPrice = _listWithPrice.Where(x => x.price <= currSearch.prMax).ToList();

            // ordina per prezzo
            if (currSearch.orderBy == "price" && currSearch.orderHow == "asc")
            { _listWithPrice = _listWithPrice.OrderBy(x => x.price).ToList(); orderBy_price = "asc"; }
            if (currSearch.orderBy == "price" && currSearch.orderHow == "desc")
            { _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ToList(); orderBy_price = "desc"; }


            _estateList = new List<AppSettings.RNT_estate>();
            _estateList.AddRange(_listWithPrice);
            _estateList.AddRange(_listOnRequest);

            if (_estateList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                _errorStr = _errorStr.Replace("#hf_voteMin#", "" + hf_voteMin);
                _errorStr = _errorStr.Replace("#hf_voteMax#", "" + hf_voteMax);
                _errorStr = _errorStr.Replace("#hf_voteRange#", "" + currSearch.voteMin + "|" + currSearch.voteMax);
                _errorStr = _errorStr.Replace("#hf_prMin#", "" + hf_prMin);
                _errorStr = _errorStr.Replace("#hf_prMax#", "" + hf_prMax);
                _errorStr = _errorStr.Replace("#hf_prRange#", "" + currSearch.prMin + "|" + currSearch.prMax);
                return _errorStr;
            }

            // filtri
            if (currSearch.voteMin != 0)
                _estateList = _estateList.Where(x => x.importance_vote >= currSearch.voteMin).ToList();
            if (currSearch.voteMax != 0)
                _estateList = _estateList.Where(x => x.importance_vote <= currSearch.voteMax).ToList();

            if (Request.QueryString["MinNumRooms"].ToInt32() != 0)
                _estateList = _estateList.Where(x => x.num_rooms_bed >= Request.QueryString["MinNumRooms"].ToInt32()).ToList();
            if (Request.QueryString["MinNumBath"].ToInt32() != 0)
                _estateList = _estateList.Where(x => x.num_rooms_bath >= Request.QueryString["MinNumBath"].ToInt32()).ToList();


            if (!string.IsNullOrEmpty(currSearch.searchTitle))
                _estateList = _estateList.Where(x => !string.IsNullOrEmpty(x.title) && x.title.ToLower().Contains(currSearch.searchTitle)).ToList();
            if (currSearch.currCity > 0)
                _estateList = _estateList.Where(x => x.pid_city == currSearch.currCity).ToList();
            if (!currSearch.currZoneList.Contains(0) && currSearch.currZoneList.Count > 0)
                _estateList = _estateList.Where(x => currSearch.currZoneList.Contains(x.pid_zone)).ToList();
            foreach (int _conf in currSearch.currConfigList)
            {
                _estateList = _estateList.Where(x => x.configList.Contains(_conf)).ToList();
            }
            this.totResults = _estateList.Count;

            // ordina per voto
            if (currSearch.orderBy == "vote" && currSearch.orderHow == "asc")
            { _estateList = _estateList.OrderBy(x => x.importance_vote).ToList(); orderBy_vote = "asc"; }
            if (currSearch.orderBy == "vote" && currSearch.orderHow == "desc")
            { _estateList = _estateList.OrderByDescending(x => x.importance_vote).ToList(); orderBy_vote = "desc"; }
            // ordina per titolo
            if (currSearch.orderBy == "title" && currSearch.orderHow == "asc")
            { _estateList = _estateList.OrderBy(x => x.title).ToList(); orderBy_title = "asc"; }
            if (currSearch.orderBy == "title" && currSearch.orderHow == "desc")
            { _estateList = _estateList.OrderByDescending(x => x.title).ToList(); orderBy_title = "desc"; }

            // set defaults, per aggiustare slider
            if (hf_prMin > currSearch.prMin || currSearch.prMin == 0) currSearch.prMin = hf_prMin;
            if (hf_prMax < currSearch.prMax || currSearch.prMax == 0) currSearch.prMax = hf_prMax;
            if (hf_voteMin > currSearch.voteMin || currSearch.voteMin == 0) currSearch.voteMin = hf_voteMin;
            if (hf_voteMax < currSearch.voteMax || currSearch.voteMax == 0) currSearch.voteMax = hf_voteMax;

            if (_estateList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                _errorStr = _errorStr.Replace("#hf_voteMin#", "" + hf_voteMin);
                _errorStr = _errorStr.Replace("#hf_voteMax#", "" + hf_voteMax);
                _errorStr = _errorStr.Replace("#hf_voteRange#", "" + currSearch.voteMin + "|" + currSearch.voteMax);
                _errorStr = _errorStr.Replace("#hf_prMin#", "" + hf_prMin);
                _errorStr = _errorStr.Replace("#hf_prMax#", "" + hf_prMax);
                _errorStr = _errorStr.Replace("#hf_prRange#", "" + currSearch.prMin + "|" + currSearch.prMax);
                return _errorStr;
            }
            string estatesOK = "";
            if (formail)
            {
                foreach (AppSettings.RNT_estate _rntEst in _estateList)
                {
                    if (!formail && _rntEst.price == 0) continue;
                    estatesOK += "       <br/>" + "<a target='_blank' href='" + CurrentAppSettings.HOST + "/" + _rntEst.page_path + "?id_op=" + Request.QueryString["id_op"].ToInt32() + "&dtS=" + currSearch.dtStart.JSCal_dateToString() + "&dtE=" + currSearch.dtEnd.JSCal_dateToString() + "&numPers=" + currSearch.numPersCount + "'>" + _rntEst.title + "</a>";
                    estatesOK += "       &nbsp;&nbsp;" + _rntEst.zone;
                    estatesOK += "       &nbsp;&nbsp;" + (_rntEst.price != 0 ? "&euro; " + _rntEst.price.ToString("N2") : "on request");
                }
                return estatesOK;
            }
            else
            {
                foreach (AppSettings.RNT_estate _rntEst in _estateList)
                {
                    if (!showonrequest && _rntEst.price == 0) continue;
                    estatesOK += "<tr>";
                    estatesOK += "   <td>";
                    estatesOK += "       " + "<a target='_blank' href='" + CurrentAppSettings.HOST + "/" + _rntEst.page_path + "?id_op=" + Request.QueryString["id_op"].ToInt32() + "&dtS=" + currSearch.dtStart.JSCal_dateToString() + "&dtE=" + currSearch.dtEnd.JSCal_dateToString() + "&numPers=" + currSearch.numPersCount + "'>" + _rntEst.title + "</a>";
                    estatesOK += "   </td>";
                    estatesOK += "   <td>";
                    estatesOK += "       " + _rntEst.zone;
                    estatesOK += "   </td>";
                    //if (!formail)
                    //{
                    //    estatesOK += "   <td>";
                    //    estatesOK += "       <a href=\"#\" onclick=\"RNT_openFreePriceReservation('" + _rntEst.id + "','" + _ls.dtStart.JSCal_dateToString() + "','" + _ls.dtEnd.JSCal_dateToString() + "', '" + _ls.numPersCount + "');return false;\"  class=\"apt\" target=\"_blank\">Prenotazione con prezzi modificabili</a>";
                    //    estatesOK += "   </td>";
                    //}
                    if (_rntEst.price > 0)
                    {
                        estatesOK += "   <td>";
                        estatesOK += "       " + (_rntEst.price != 0 ? "&euro; " + _rntEst.price.ToString("N2") : "on request");
                        estatesOK += "   </td>";
                        estatesOK += "   <td>";
                        //estatesOK += "       " + (_rntEst.priceEco != 0 ? "&nbsp;+&nbsp;" + _rntEst.priceEco.ToString("N2") + "&nbsp;(pulizia)" : "");
                        //estatesOK += "       " + (_rntEst.priceSrs != 0 ? "&nbsp;+&nbsp;" + _rntEst.priceSrs.ToString("N2") + "&nbsp;(accoglenza)" : "");
                        //estatesOK += "       " + (_rntEst.priceAgencyFee != 0 ? "&nbsp;+&nbsp;" + _rntEst.priceAgencyFee.ToString("N2") + "&nbsp;(agency&nbsp;fee)" : "");
                        estatesOK += "   </td>";
                        estatesOK += "   <td>";
                        estatesOK += "       " + "&euro; " + _rntEst.prTotalCommission.ToString("N2");
                        //estatesOK += "       " + "&euro; " + _rntEst.prTotalCommission.ToString("N2") + "(" + (_rntEst.priceMargine.ToString("N2") + " %") + ")";
                        estatesOK += "   </td>";
                    }
                    else
                    {
                        estatesOK += "   <td colspan='3'>";
                        estatesOK += "       " + ("on request");
                        estatesOK += "   </td>";
                    }
                    estatesOK += "   <td>";
                    estatesOK += "       <a href=\"#\" onclick=\"RNT_openSelection('" + _rntEst.id + "','" + currSearch.dtStart.JSCal_dateToString() + "','" + currSearch.dtEnd.JSCal_dateToString() + "', '" + currSearch.numPersCount + "', '" + agentID + "');return false;\"  class=\"apt\" target=\"_blank\">Prenota</a>";
                    estatesOK += "   </td>";
                    estatesOK += "</tr>";
                }
                return "<table>" + estatesOK + "</table>";

            }
        }
    }
}
