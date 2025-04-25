using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class rnt_estate_list_bycat : System.Web.UI.Page
    {
        protected string CURRENT_FILTER
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_webservice_rnt_estate_list_bycat_FILTER_" + _currCat] != null)
                {
                    return (string)HttpContext.Current.Session["CURRENT_webservice_rnt_estate_list_bycat_FILTER_" + _currCat];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["CURRENT_webservice_rnt_estate_list_bycat_FILTER_" + _currCat] = value;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private List<AppSettings.RNT_estate> _estateList;
        private string CURRENT_SESSION_ID;
        private DateTime _dtStart;
        private DateTime _dtEnd;
        private int _currLang;
        private int _numPers;
        private string _currCat;
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
        private string IMG_ROOT
        {
            get { return Request.Url.AbsoluteUri.Contains("http://localhost") ? "http://192.168.1.150/Rir/rentalinrome/" : "/"; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                _currCat = Request.QueryString["currCat"];
                _currLang = Request.QueryString["lang"].objToInt32();
                if (_currLang == 0)
                    _currLang = CurrentLang.ID;
                if (Request.QueryString["session"] == "load" && CURRENT_FILTER != "")
                {
                    Response.Redirect("rnt_estate_list_bycat.aspx?" + CURRENT_FILTER.Replace("lang=", "").Replace("session=", "") + "&lang=" + _currLang);
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
                _numPers = Request.QueryString["numPers"].objToInt32();
                if (_numPers == 0)
                    _numPers = _config.lastSearch.numPersCount;
                _action = Request.QueryString["action"] + "";
                _currPage = Request.QueryString["currPage"].objToInt32();
                //_currPage = (_currPage != 0) ? _currPage : 1;
                _numPerPage = Request.QueryString["numPerPage"].objToInt32() <= 0 ? 10 : Request.QueryString["numPerPage"].objToInt32();
                _currEstate = Request.QueryString["currEstate"].objToInt32();
                _searchTitle = (Request.QueryString["title"] + "").Trim().ToLower();
                _minPrice = Request.QueryString["minPrice"].objToInt32();
                _maxPrice = Request.QueryString["maxPrice"].objToInt32();
                _voteRange = Request.QueryString["voteRange"] + "";
                _currConfig = Request.QueryString["currConfig"].splitStringToList("|").Select(x => x.ToInt32()).ToList();
                _orderBy = Request.QueryString["orderBy"] + "";
                _orderHow = Request.QueryString["orderHow"] + "";
                _mode = string.IsNullOrEmpty(Request.QueryString["mode"]) ? "search" : Request.QueryString["mode"];
                if (_action == "list" || _action == "first")
                {
                    CURRENT_FILTER = Request.QueryString.ToString();
                    Response.Write(getEstateList());
                }
            }
        }
        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList
        {
            get
            {
                if (AppSettings.RELOAD_CACHE || HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currCat] == null)
                {
                    List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.Where(x => x.category == _currCat).ToList();
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
                        _rntEst.price = rntUtils.rntEstate_minPrice(_rntEst.id);
                        _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), _currLang, "");
                    }
                    HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currCat] = estateList;
                }
                return (List<AppSettings.RNT_estate>)HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currCat];
            }
            set { HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currCat] = value; }
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
                    _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), _currLang, "");
                }
                CURRENT_RNT_estateList = _estateList;
            }
            if (_action == "first")
            {
                List<AppSettings.RNT_estate> _listWithPrice = CURRENT_RNT_estateList.Where(x => x.price != 0).OrderByDescending(x => x.is_online_booking).ThenBy(x => x.sequence).ThenBy(x => x.price).ToList();
                List<AppSettings.RNT_estate> _listOnRequest = CURRENT_RNT_estateList.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();
                _estateList = new List<AppSettings.RNT_estate>();
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
            if (_searchTitle != "")
                _estateList = _estateList.Where(x => x.title.ToLower().Contains(_searchTitle)).ToList();
            if (_minPrice != 0)
                _estateList = _estateList.Where(x => x.price >= _minPrice).ToList();
            if (_currEstate != 0)
                _estateList = _estateList.Where(x => x.id != _currEstate).ToList();
            if (_action == "list")
            {
                //if (_currConfig.Contains(1))
                //    _estateList = _estateList.Where(x => x.has_air_condition == 1).ToList();
                //if (_currConfig.Contains(2))
                //    _estateList = _estateList.Where(x => x.has_adsl == 1).ToList();
                if (_maxPrice != 0)
                    _estateList = _estateList.Where(x => x.price <= _maxPrice).ToList();
                if (_voteRange != "" && _voteRange != "0|10")
                    _estateList = _estateList.Where(x => x.importance_vote >= _voteRange.Split('|')[0].ToInt32() && x.importance_vote <= _voteRange.Split('|')[1].ToInt32()).ToList();
            }
            int _allCount = _estateList.Count;
            int _currCount = _allCount;
            if (_currCount == 0)
            {
                return "<div class=\"listEmpty error\"><span>" + CurrentSource.getSysLangValue("lblApartmentSearchError") + "</span></div>";
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
            _response += _pager;
            _response += "<input type=\"hidden\" id=\"hf_currPage\" value=\"" + _currPage + "\"/><ul class=\"display dett_view\">";
            foreach (AppSettings.RNT_estate _rntEst in _estateList)
            {
                _response += "<li>";
                _response += "  <a href=\"/" + _rntEst.page_path + "\" class=\"content_block\">";
                _response += "      <img width=\"162\" height=\"89px\" src=\"" + ((("" + _rntEst.img_preview_1).Trim() != "") ? IMG_ROOT + _rntEst.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" class=\"fotosmall\" alt=\"" + _rntEst.title + "\" />";
                _response += "      <span class=\"testi\">";
                _response += "          <span class=\"tit\">";
                _response += "              <h2>" + _rntEst.title + "</h2>";
                if (_rntEst.is_online_booking == 1)
                {
                    _response += "              <img src=\"/images/css/book-now.gif\" class=\"ico_book_list\">";
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
                    _response += "              <span class=\"euro\">";
                    _response += "                  <span class=\"from\">" + CurrentSource.getSysLangValue("lblPriceFrom") + "</span>";
                    _response += "                  " + _rntEst.price.ToString("N2") + "<span style=\"font-size: 24px;\">&euro;</span>";
                    _response += "                  <span class=\"paxday\">" + CurrentSource.getSysLangValue("lblADay") + "</span>";
                    _response += "              </span>";
                }
                else
                {
                    _response += "              <span class=\"euro richiesta\">";
                    _response += "                  <span>" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>";
                    _response += "              </span>";
                }
                _response += "              <span class=\"more\">vedi scheda</span>";
                _response += "          </span>";
                _response += "          <span class=\"nulla\"></span>";
                _response += "      </span>";
                _response += "   </a>";
                _response += "";
                _response += "";
                _response += "";
                _response += "</li>";
            }
            _response += "</ul>";
            _response += _pager;
            return _response;
        }
    }
}
