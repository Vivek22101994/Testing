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
    public partial class rnt_estate_list_search : System.Web.UI.Page
    {
        protected string CURRENT_FILTER
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_webservice_rnt_estate_list_search_FILTER_search"] != null)
                {
                    return (string)HttpContext.Current.Session["CURRENT_webservice_rnt_estate_list_search_FILTER_search"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["CURRENT_webservice_rnt_estate_list_search_FILTER_search"] = value;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private List<AppSettings.RNT_estate> _estateList;
        private string CURRENT_SESSION_ID;
        private DateTime _dtStart;
        private DateTime _dtEnd;
        private int _dtCount;
        private int _currLang;
        private int _numPers;
        private List<int> _currZone;
        private List<int> _currConfig;
        private int _currPage;
        private int _numPerPage;
        private int _currEstate;
        private int _minPrice;
        private int _maxPrice;
        private string _searchTitle;
        private string _voteRange;
        private string _mode;
        private string _action;
        private string _orderBy;
        private string _orderHow;
        private decimal _prDiscount;
        private List<rntExts.RNT_estatePriceDetails> _priceDetails;
        private int _isWL = 0;
        private int _isNew = 0;

        private string IMG_ROOT
        {
            get { return Request.Url.AbsoluteUri.Contains("http://localhost") ? "http://192.168.1.150/Rir/rentalinrome/" : "/"; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                _currLang = Request.QueryString["lang"].objToInt32();
                if (_currLang == 0)
                    _currLang = CurrentLang.ID;
                if (Request.QueryString["session"] == "true" && CURRENT_FILTER != "")
                {
                    Response.Redirect("rnt_estate_list_search.aspx?" + CURRENT_FILTER.Replace("lang=", "").Replace("session=", "") + "&lang=" + _currLang);
                }
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/html";
                CURRENT_SESSION_ID = Request.QueryString["SESSION_ID"];
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                _dtStart = _config.lastSearch.dtStart;
                _dtEnd = _config.lastSearch.dtEnd;
                int _dtStartInt = Request.QueryString["dtS"].objToInt32();
                int _dtEndInt = Request.QueryString["dtE"].objToInt32();
                if (_dtStartInt != 0 && _dtEndInt != 0)
                {
                    _dtStart = _dtStartInt.JSCal_intToDate();
                    _dtEnd = _dtEndInt.JSCal_intToDate();
                }
                _dtCount = (_dtEnd - _dtStart).Days;
                _numPers = Request.QueryString["numPers"].objToInt32();
                if (_numPers == 0)
                    _numPers = _config.lastSearch.numPersCount;
                _action = Request.QueryString["action"] + "";
                _currPage = Request.QueryString["currPage"].objToInt32();
                //_currPage = (_currPage != 0) ? _currPage : 1;
                _numPerPage = Request.QueryString["numPerPage"].objToInt32() <= 0 ? 10 : Request.QueryString["numPerPage"].objToInt32();
                _currEstate = Request.QueryString["currEstate"].objToInt32();
                _searchTitle = (Request.QueryString["title"] + "").Trim().ToLower().urlDecode();
                _minPrice = Request.QueryString["minPrice"].objToInt32();
                _maxPrice = Request.QueryString["maxPrice"].objToInt32();
                _voteRange = Request.QueryString["voteRange"].urlDecode();
                _currZone = Request.QueryString["currZone"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).ToList();
                _currConfig = Request.QueryString["currConfig"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).ToList();
                _orderBy = Request.QueryString["orderBy"] + "";
                _orderHow = Request.QueryString["orderHow"] + "";
                _mode = string.IsNullOrEmpty(Request.QueryString["mode"]) ? "search" : Request.QueryString["mode"];
                _isWL = Request.QueryString["isWL"].objToInt32();
                _isNew = Request.QueryString["isNew"].objToInt32();

                if (_action == "list" || _action == "first")
                {
                    if (_mode == "search")
                        CURRENT_FILTER = Request.QueryString.ToString().Replace("session=", "");
                    Response.Write(getEstateList());
                }
                if (_action == "alternative")
                {
                    Response.Write(getEstateList());
                }
                if (_action == "price")
                {
                    _estateList = CURRENT_RNT_estateList;
                    if (!_currZone.Contains(0) && _currZone.Count > 0)
                        _estateList = _estateList.Where(x => _currZone.Contains(x.pid_zone)).ToList();
                    if (_estateList.Where(x => x.price != 0).Count() > 0)
                        Response.Write(_estateList.Where(x => x.price != 0).Min(x => x.price).objToInt32() + "|" + _estateList.Max(x => x.price).objToInt32());
                    else
                        Response.Write("0|0");
                }
                if (_action == "vote")
                {
                    _estateList = CURRENT_RNT_estateList;
                    if (!_currZone.Contains(0) && _currZone.Count > 0)
                        _estateList = _estateList.Where(x => _currZone.Contains(x.pid_zone)).ToList();
                    if (_estateList.Count > 0)
                        Response.Write(_estateList.Min(x => x.importance_vote).objToInt32() + "|" + _estateList.Max(x => x.importance_vote).objToInt32());
                    else
                        Response.Write("0|0");
                }
            }
        }
        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList
        {
            get
            {
                using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                    if (HttpContext.Current.Session["CURRENT_RNT_estateList_" + _dtStart.JSCal_dateToString() + "_" + _dtEnd.JSCal_dateToString() + "_" + _numPers] == null)
                    {
                        List<int> estatesNotAvv = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 //
                                                                                 && x.dtStart.HasValue //
                                                                                 && x.dtEnd.HasValue //
                                                                                 && ((x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                     || (x.dtStart.Value.Date >= _dtStart && x.dtStart.Value.Date < _dtEnd) //
                                                                                     || (x.dtEnd.Value.Date > _dtStart && x.dtEnd.Value.Date <= _dtEnd))).Select(x => x.pid_estate.objToInt32()).ToList();
                        List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.Where(x => x.num_persons_max >= _numPers && x.num_persons_min <= _numPers && x.nights_min <= _dtCount && (x.nights_max == 0 || x.nights_max >= _dtCount)//
                                                                                                        && !estatesNotAvv.Contains(x.id)).ToList();

                        #region Check White Label
                        decimal agentTotalResPrice = 0;
                        dbRntAgentTBL agentTbl = (dbRntAgentTBL)null;
                        if (_isWL == 1)
                        {
                            List<int> lstAgentEstate = new List<int>();
                            using (DCmodRental dc = new DCmodRental())
                            {
                                agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.id == App.WLAgentId);
                                lstAgentEstate = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == App.WLAgentId).Select(x => x.pidEstate).Distinct().ToList();
                            }
                            estateList = estateList.Where(x => lstAgentEstate.Contains(x.id)).ToList();

                            if (agentTbl != null)
                            {
                                DateTime checkDate = DateTime.Now;
                                DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                                DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                                var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTbl.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                                agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                            }
                        }
                        #endregion
                        foreach (AppSettings.RNT_estate _rntEst in estateList)
                        {
                            RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == _currLang && !string.IsNullOrEmpty(x.title));
                            if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                            if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 1);
                            if (_lang != null)
                            {
                                _rntEst.pid_lang = _currLang;
                                _rntEst.title = _lang.title;
                                _rntEst.summary = _lang.summary;
                                _rntEst.page_path = _lang.page_path;
                            }
                            else
                            {
                                _rntEst.pid_lang = 0;
                                _rntEst.title = _rntEst.code;
                                _rntEst.summary = "";
                                _rntEst.page_path = "";
                            }
                            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                            outPrice.dtStart = _dtStart;
                            outPrice.dtEnd = _dtEnd;
                            outPrice.numPersCount = _numPers;
                            outPrice.pr_discount_owner = 0;
                            outPrice.pr_discount_commission = 0;
                            outPrice.part_percentage = _rntEst.pr_percentage;

                            if (_isWL == 1 && agentTbl != null)
                            {
                                outPrice.fillAgentDetails(agentTbl);
                                outPrice.agentTotalResPrice = agentTotalResPrice;
                                outPrice.isWL = 1;
                                outPrice.WL_changeAmount = agentTbl.WL_changeAmount.objToInt32();
                                outPrice.WL_changeIsDiscount = agentTbl.WL_changeIsDiscount == null ? 0 : agentTbl.WL_changeIsDiscount.objToInt32();
                                outPrice.WL_changeIsPercentage = agentTbl.WL_changeIsPercentage == null ? 1 : agentTbl.WL_changeIsPercentage.objToInt32();

                                outPrice.WL_changeAmount_Agent = agentTbl.WL_changeAmount_Agent.objToInt32();
                                outPrice.WL_changeIsDiscount_Agent = agentTbl.WL_changeIsDiscount_Agent == null ? 0 : agentTbl.WL_changeIsDiscount_Agent.objToInt32();
                                outPrice.WL_changeIsPercentage_Agent = agentTbl.WL_changeIsPercentage_Agent == null ? 1 : agentTbl.WL_changeIsPercentage_Agent.objToInt32();
                            }
                            outPrice.agentCommissionPerc = 0;
                            _rntEst.price = rntUtils.rntEstate_getPrice(0, _rntEst.id, ref outPrice);
                            _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), _currLang, "");
                        }
                        HttpContext.Current.Session["CURRENT_RNT_estateList_" + _dtStart.JSCal_dateToString() + "_" + _dtEnd.JSCal_dateToString() + "_" + _numPers] = estateList;
                    }
                return ((List<AppSettings.RNT_estate>)HttpContext.Current.Session["CURRENT_RNT_estateList_" + _dtStart.JSCal_dateToString() + "_" + _dtEnd.JSCal_dateToString() + "_" + _numPers]);//.Select(x => x.Clone()).ToList();
            }
            set { HttpContext.Current.Session["CURRENT_RNT_estateList_" + _dtStart.JSCal_dateToString() + "_" + _dtEnd.JSCal_dateToString() + "_" + _numPers] = value; }
        }
        protected string getEstateList()
        {
            _estateList = CURRENT_RNT_estateList;
            if (_estateList.Count > 0 && _estateList[0].pid_lang != _currLang)
            {
                foreach (AppSettings.RNT_estate _rntEst in _estateList)
                {
                    RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == _currLang && !string.IsNullOrEmpty(x.title));
                    if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                    if (_lang != null)
                    {
                        _rntEst.pid_lang = _currLang;
                        _rntEst.title = _lang.title;
                        _rntEst.summary = _lang.summary;
                        _rntEst.page_path = _lang.page_path;
                    }
                    else
                    {
                        _rntEst.pid_lang = 0;
                        _rntEst.title = _rntEst.code;
                        _rntEst.summary = "";
                        _rntEst.page_path = "";
                    }
                    _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), _currLang, "");
                }
                CURRENT_RNT_estateList = _estateList;
            }
            if (_action == "first")
            {
                List<AppSettings.RNT_estate> _listWithPrice = CURRENT_RNT_estateList.Where(x => x.price != 0).OrderByDescending(x => x.is_online_booking).ThenBy(x => x.sequence).ThenBy(x => x.price).ToList();
                List<AppSettings.RNT_estate> _listOnRequest = CURRENT_RNT_estateList.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();
                _estateList = new List<AppSettings.RNT_estate>();
                if (_mode == "alternative")
                    _listWithPrice = _listWithPrice.OrderBy(x => x.price).ToList();
                _estateList.AddRange(_listWithPrice);
                _estateList.AddRange(_listOnRequest);
                CURRENT_RNT_estateList = _estateList;
            }
            else
            {
                List<AppSettings.RNT_estate> _listWithPrice = CURRENT_RNT_estateList.Where(x => x.price != 0).OrderByDescending(x => x.is_online_booking).ThenBy(x => x.sequence).ThenBy(x => x.price).ToList();
                List<AppSettings.RNT_estate> _listOnRequest = CURRENT_RNT_estateList.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();
                if (_orderBy == "price" && _orderHow == "asc")
                    _listWithPrice = _listWithPrice.OrderBy(x => x.price).ToList();
                if (_orderBy == "price" && _orderHow == "desc")
                    _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ToList();
                _estateList = new List<AppSettings.RNT_estate>();
                _estateList.AddRange(_listWithPrice);
                _estateList.AddRange(_listOnRequest);
                if (_orderBy == "vote" && _orderHow == "asc")
                    _estateList = _estateList.OrderBy(x => x.importance_vote).ToList();
                if (_orderBy == "vote" && _orderHow == "desc")
                    _estateList = _estateList.OrderByDescending(x => x.importance_vote).ToList();
                if (_orderBy == "title" && _orderHow == "asc")
                    _estateList = _estateList.OrderBy(x => x.title).ToList();
                if (_orderBy == "title" && _orderHow == "desc")
                    _estateList = _estateList.OrderByDescending(x => x.title).ToList();

            }
            if (!string.IsNullOrEmpty(_searchTitle))
                _estateList = _estateList.Where(x => !string.IsNullOrEmpty(x.title) && x.title.ToLower().Contains(_searchTitle)).ToList();
            if (!_currZone.Contains(0) && _currZone.Count > 0)
                _estateList = _estateList.Where(x => _currZone.Contains(x.pid_zone)).ToList();
            if (_minPrice != 0)
                _estateList = _estateList.Where(x => x.price >= _minPrice).ToList();
            if (_currEstate != 0)
                _estateList = _estateList.Where(x => x.id != _currEstate).ToList();
            if (_action == "list")
            {
                foreach (int _conf in _currConfig)
                {
                    _estateList = _estateList.Where(x => x.configList.Contains(_conf)).ToList();
                }
                if (_maxPrice != 0)
                    _estateList = _estateList.Where(x => x.price <= _maxPrice).ToList();
                if (_voteRange != "" && _voteRange != "0|10")
                    _estateList = _estateList.Where(x => x.importance_vote >= _voteRange.Split('|')[0].ToInt32() && x.importance_vote <= _voteRange.Split('|')[1].ToInt32()).ToList();
            }
            int _allCount = _estateList.Count;
            int _currCount = _allCount;
            if (_currCount == 0)
            {
                return (_mode == "alternative") ? "" : "<div class=\"listEmpty error\"><span>" + CurrentSource.getSysLangValue("lblApartmentSearchError") + "</span></div>";
            }
            if (_currPage != 0)
            {
                int _pageCount = 1;
                int _currPageCount = _numPerPage;
                while (_currCount > _currPageCount)
                {
                    _pageCount++;
                    _currCount -= _numPerPage;
                }
                if (_pageCount < _currPage) _currPage = _pageCount;
                if (_pageCount == _currPage && _currCount < _currPageCount) _currPageCount = _currCount;
                _estateList = _estateList.GetRange((_currPage - 1) * _numPerPage, _currPageCount);
            }
            string _pager = "<div class=\"ordina pageCount\">" + CommonUtilities.getPager(_allCount, _numPerPage, _currPage, 10, "<span class=\"page\">" + CurrentSource.getSysLangValue("listAllPages") + "</span>", "<a class=\"page\" href=\"javascript:RNT_changePage(0)\">" + CurrentSource.getSysLangValue("listAllPages") + "</a>", "<span class=\"page\">{0}</span>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">{0}</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>") + "</div>";
            string _response = "";
            if (_mode == "search")
            {
                _response += _pager;
                _response += "<input type=\"hidden\" id=\"hf_currPage\" value=\"" + _currPage + "\"/><ul class=\"display dett_view\">";
            }
            int cnt = 1;
            foreach (AppSettings.RNT_estate _rntEst in _estateList)
            {
                if (_mode == "search")
                {
                    _response += "<li>";
                    _response += "  <a href=\"/" + _rntEst.page_path + "?dtS=" + _dtStart.JSCal_dateToString() + "&dtE=" + _dtEnd.JSCal_dateToString() + "&numPers=" + _numPers + "&search=true\" class=\"content_block\">";
                    _response += "      <img width=\"162\" height=\"89px\" src=\"" + ((("" + _rntEst.img_preview_1).Trim() != "") ? IMG_ROOT + _rntEst.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" class=\"fotosmall\" alt=\"" + _rntEst.title + "\" />";
                    _response += "      <span class=\"testi\">";
                    _response += "          <span class=\"tit\">";
                    _response += "              <h2>" + _rntEst.title + "</h2>";
                    if (_rntEst.is_online_booking == 1)
                    {
                        if (_isWL == 0)
                            _response += "              <img src=\"/images/css/book-now.gif\" class=\"ico_book_list\">";
                        else
                            _response += "              <img src=\"/WLRental/images/book-now.gif\" class=\"ico_book_list\">";
                    }
                    _response += "              <span class=\"nulla\"></span>";
                    _response += "              <span class=\"cat\">" + _rntEst.zone + "</span>";
                    _response += "           </span>";
                    _response += "          <span class=\"voto\"><img alt=\"" + _rntEst.importance_vote + "\" src=\"/images/estate_vote/vote" + _rntEst.importance_vote + ".gif\" />" + _rntEst.importance_vote + "/10</span>";
                    _response += "          <span class=\"nulla\"></span>";
                    _response += "      </span>";
                    _response += "      <span class=\"dettaglio\">";
                    _response += "          <span class=\"cont_foto\">";
                    _response += "              <img width=\"162\" height=\"89px\" src=\"" + ((("" + _rntEst.img_preview_1).Trim() != "") ? IMG_ROOT + _rntEst.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" class=\"foto\" alt=\"rome apartments\" />";
                    _response += "              <img width=\"162\" height=\"89px\" src=\"" + ((("" + _rntEst.img_preview_2).Trim() != "") ? IMG_ROOT + _rntEst.img_preview_2 : "/images/css/default-apt-list.jpg") + "\" class=\"foto\" alt=\"" + _rntEst.zone + "\" />";
                    _response += "              <img width=\"162\" height=\"89px\" src=\"" + ((("" + _rntEst.img_preview_3).Trim() != "") ? IMG_ROOT + _rntEst.img_preview_3 : "/images/css/default-apt-list.jpg") + "\" class=\"foto\" alt=\"" + _rntEst.title + "\"  style=\"margin-right:0;\"/>";
                    _response += "          </span>";
                    _response += "          <span class=\"tx_dett\">" + _rntEst.summary + "</span>";
                    _response += "          <span class=\"dx_dett\">";
                    if (_rntEst.price != 0)
                    {
                        _response += "              <span class=\"euro\" style=\"height: 23px;\">";
                        _response += "                  " + _rntEst.price.ToString("N2") + "<span style=\"font-size: 24px;\">€</span>";
                        _response += "              </span>";
                    }
                    else
                    {
                        _response += "              <span class=\"euro richiesta\" style=\"height: 23px;\">";
                        _response += "                  <span>" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>";
                        _response += "              </span>";
                    }
                    _response += "              <span class=\"more search\">" + CurrentSource.getSysLangValue("lblViewDetails") + "</span>";
                    _response += "          </span>";
                    _response += "          <span class=\"nulla\"></span>";
                    _response += "      </span>";
                    _response += "   </a>";
                    _response += "</li>";
                }
                else
                {
                    if (_isNew == 1)
                    {
                        if (cnt <= 6)
                            _response += "<div data-animation-delay=\"200\" data-animation-direction=\"from-bottom\" class=\"item col-md-4 animate-from-bottom animation-from-bottom\">";
                        else
                            _response += "<div data-animation-delay=\"600\" data-animation-direction=\"from-bottom\" class=\"item col-md-4 disabled animate-from-bottom animation-from-bottom\">";

                        _response += "<div class=\"image\">";
                        _response += "<a href=\"/" + _rntEst.page_path + "?dtS=" + _dtStart.JSCal_dateToString() + "&dtE=" + _dtEnd.JSCal_dateToString() + "&numPers=" + _numPers + "\">";
                        _response += "<h3>" + _rntEst.title + "</h3>";
                        _response += "<span class=\"location\">" + _rntEst.zone + "</span>";
                        _response += "</a>";
                        _response += "<img src=\"" + ((("" + _rntEst.img_preview_1).Trim() != "") ? IMG_ROOT + _rntEst.img_preview_1 : "/images/css/default-apt-list.jpg") + "\"  alt=\"" + _rntEst.title + "\" />";
                        _response += "</div>";
                        _response += "<div class=\"price\">";
                        _response += "<span>&euro;" + _rntEst.price.ToString("N2") + "</span>";
                        _response += "</div>";
                        _response += "<ul class=\"amenities\">";
                        _response += "<li><i class=\"fa fa-users\"></i>" + _rntEst.num_persons_max + "</li>";
                        _response += "<li><i class=\"icon-bedrooms\"></i>" + _rntEst.num_rooms_bed + "</li>";
                        _response += "<li><i class=\"icon-bathrooms\"></i>" + _rntEst.num_rooms_bath + "</li>";
                        _response += "</ul>";
                        _response += "</div>";
                        _response += "</div>";
                        _response += "<input type=\"hidden\" value=" + _estateList.Count + " id=\"alternative_cnt\" />";
                    }
                    else
                    {
                        _response += "<a href=\"/" + _rntEst.page_path + "?dtS=" + _dtStart.JSCal_dateToString() + "&dtE=" + _dtEnd.JSCal_dateToString() + "&numPers=" + _numPers + "\" class=\"commento\">";
                        _response += "      <img src=\"" + ((("" + _rntEst.img_preview_1).Trim() != "") ? IMG_ROOT + _rntEst.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" class=\"aptPhoto\" alt=\"" + _rntEst.title + "\" />";
                        _response += "      <span class=\"commCont\">";
                        _response += "          <span class=\"userName\">";
                        _response += "              " + _rntEst.title + ", ";
                        _response += "              " + _rntEst.zone + "";
                        if (_rntEst.is_online_booking == 1)
                        {
                            if (_isWL == 0)
                                _response += "              <img src=\"/images/css/book-now.gif\" class=\"ico_book_list\">";
                            else
                                _response += "              <img src=\"/WLRental/images/book-now.gif\" class=\"ico_book_list\">";
                        }
                        _response += "           </span>";
                        _response += "          <span class=\"votazione\"><img alt=\"" + _rntEst.importance_vote + "\" src=\"/images/estate_vote/vote" + _rntEst.importance_vote + ".gif\" />" + _rntEst.importance_vote + "/10</span>";
                        _response += "          <span class=\"nulla\"></span>";
                        _response += "          <span class=\"commentoTxt\">" + _rntEst.summary + "</span>";
                        _response += "      </span>";
                        if (_rntEst.price != 0)
                            _response += "              <span class=\"alt_estate_euro\">" + _rntEst.price.ToString("N2") + "&euro;</span>";
                        else
                            _response += "              <span class=\"alt_estate_euro richiesta\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>";
                        _response += "</a>";
                    }
                }
                _response += "";
                _response += "";
                _response += "";
                cnt = cnt + 1;
            }
            if (_mode == "search")
            {
                _response += "</ul>";
                _response += _pager;
            }
            return _response;
        }
    }
}
