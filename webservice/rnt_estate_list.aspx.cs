using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class rnt_estate_list : System.Web.UI.Page
    {
        protected string CURRENT_FILTER
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_webservice_rnt_estate_list_FILTER_" + _mode] != null)
                {
                    return (string)HttpContext.Current.Session["CURRENT_webservice_rnt_estate_list_FILTER_" + _mode];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["CURRENT_webservice_rnt_estate_list_FILTER_" + _mode] = value;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private List<AppSettings.RNT_estate> _estateList;
        private DateTime _dtStart;
        private DateTime _dtEnd;
        private int _currLang;
        private int _numPers;
        private List<int> _currZone;
        private List<int> _currConfig;
        private int _currPage;
        private int _numPerPage;
        private int _currEstate;
        private int _minPrice;
        private int _maxPrice;
        private string _voteRange;
        private string _mode;
        private string _action;
        private string _orderBy_vote;
        private string _orderBy_price;
        private string _orderBy_title;
        private decimal _prDiscount;
        private List<rntExts.RNT_estatePriceDetails> _priceDetails;
        private string IMG_ROOT
        {
            get { return Request.Url.AbsoluteUri.Contains("http://localhost") ? "http://192.168.1.150/Rir/rentalinrome/" : "/"; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            return;
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                if (Request.QueryString["session"] == "true" && CURRENT_FILTER != "")
                {
                    Response.Redirect("rnt_estate_list.aspx?" + CURRENT_FILTER);
                }
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/html";
                _currLang = Request.QueryString["lang"].objToInt32();
                if (_currLang == 0)
                    _currLang = CurrentLang.ID;
                _dtStart = DateTime.Now.Date.AddDays(7);
                _dtEnd = DateTime.Now.Date.AddDays(10);
                int _dtStartInt = Request.QueryString["dtS"].objToInt32();
                int _dtEndInt = Request.QueryString["dtE"].objToInt32();
                if (_dtStartInt != 0 && _dtEndInt != 0)
                {
                    _dtStart = _dtStartInt.JSCal_intToDate();
                    _dtEnd = _dtEndInt.JSCal_intToDate();
                }
                _numPers = Request.QueryString["numPers"].objToInt32();
                _numPers = (_numPers != 0) ? _numPers : 2;
                _action = Request.QueryString["action"] + "";
                _currPage = Request.QueryString["currPage"].objToInt32();
                _currPage = (_currPage != 0) ? _currPage : 1;
                _numPerPage = Request.QueryString["numPerPage"].objToInt32();
                _numPerPage = (_numPerPage != 0) ? _numPerPage : 10;
                _currEstate = Request.QueryString["currEstate"].objToInt32();
                _minPrice = Request.QueryString["minPrice"].objToInt32();
                _maxPrice = Request.QueryString["maxPrice"].objToInt32();
                _voteRange = Request.QueryString["voteRange"]+"";
                _currZone = Request.QueryString["currZone"].splitStringToList("|").Select(x=>x.ToInt32()).ToList();
                _currConfig = Request.QueryString["currConfig"].splitStringToList("|").Select(x => x.ToInt32()).ToList();
                _orderBy_vote = Request.QueryString["orderBy_vote"] + "";
                _orderBy_price = Request.QueryString["orderBy_price"] + "";
                _orderBy_title = Request.QueryString["orderBy_title"] + "";
                _mode = string.IsNullOrEmpty(Request.QueryString["mode"]) ? "search" : Request.QueryString["mode"];
                if (_action == "list" || _action == "first")
                {
                    if (_mode == "zone")
                        Response.Write(getEstateList_zone());
                    if (_mode == "otherzone")
                        Response.Write(getEstateList_otherzone());
                }
                if (_action == "config")
                {
                    Response.Write("");
                }
                if (_action == "zone")
                {
                    Response.Write("");
                }
            }
        }
        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList_otherzone
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_RNT_estateList_otherzone"] == null)
                {
                    List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.Where(x => !AppSettings._LOC_CUSTOM_ZONEs.Contains( x.pid_zone )).ToList();

                    foreach (AppSettings.RNT_estate _rntEst in estateList)
                    {
                        RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == CurrentLang.ID);
                        if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                        if (_lang != null)
                        {
                            _rntEst.pid_lang = CurrentLang.ID;
                            _rntEst.title = _lang.title;
                            _rntEst.summary = _lang.summary;
                            _rntEst.page_path = _lang.page_path;
                        }
                        List<decimal> _priceList = new List<decimal>();
                        _rntEst.price = rntUtils.rntEstate_minPrice(_rntEst.id);
                        _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), CurrentLang.ID, "");
                    }
                    HttpContext.Current.Session["CURRENT_RNT_estateList_otherzone"] = estateList;
                }
                return (List<AppSettings.RNT_estate>)HttpContext.Current.Session["CURRENT_RNT_estateList_otherzone"];
            }
            set { HttpContext.Current.Session["CURRENT_RNT_estateList_otherzone"] = value; }
        }
        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList_zone
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone[0]] == null)
                {
                    List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.Where(x => x.pid_zone == _currZone[0]).ToList();

                    foreach (AppSettings.RNT_estate _rntEst in estateList)
                    {
                        RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == _currLang);
                        if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                        if (_lang != null)
                        {
                            _rntEst.pid_lang = _currLang;
                            _rntEst.title = _lang.title;
                            _rntEst.summary = _lang.summary;
                            _rntEst.page_path = _lang.page_path;
                        }
                        _rntEst.price = rntUtils.rntEstate_minPrice(_rntEst.id);
                        _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), _currLang, "");
                    }
                    HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone[0]] = estateList;
                }
                return (List<AppSettings.RNT_estate>)HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone[0]];
            }
            set { HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone[0]] = value; }
        }
        protected string getPager(int _allCount)
        {
            int _pageCount = 1;
            int _currPageCount = _numPerPage;
            while (_allCount > _currPageCount)
            {
                _pageCount++;
                _allCount -= _numPerPage;
            }
            string _pager = "";
            for (int i = 1; i <= _pageCount; i++)
            {
                _pager += i == _currPage ? "<span class=\"page\">" + i + "</span>" : "<a class=\"page\" href=\"javascript:RNT_changePage(" + i + ")\">" + i + "</a>";
            }
            return _pager;
        }
        protected string getEstateList_otherzone()
        {
            _estateList = CURRENT_RNT_estateList_otherzone;
            if (_estateList.Count > 0 && _estateList[0].pid_lang != _currLang)
            {
                foreach (AppSettings.RNT_estate _rntEst in _estateList)
                {
                    RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == _currLang);
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
                CURRENT_RNT_estateList_otherzone = _estateList;
            }
            if (_action == "first")
            {
                List<AppSettings.RNT_estate> _listWithPrice = CURRENT_RNT_estateList_otherzone.Where(x => x.price != 0).OrderBy(x => x.sequence).ThenBy(x => x.price).ToList();
                List<AppSettings.RNT_estate> _listOnRequest = CURRENT_RNT_estateList_otherzone.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();
                _estateList = new List<AppSettings.RNT_estate>();
                _estateList.AddRange(_listWithPrice);
                _estateList.AddRange(_listOnRequest);
                CURRENT_RNT_estateList_otherzone = _estateList;
            }
            else
            {
                List<AppSettings.RNT_estate> _listWithPrice = CURRENT_RNT_estateList_otherzone.Where(x => x.price != 0).OrderBy(x => x.sequence).ThenBy(x => x.price).ToList();
                List<AppSettings.RNT_estate> _listOnRequest = CURRENT_RNT_estateList_otherzone.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();
                if (_orderBy_price != "")
                {
                    if (_orderBy_price == "desc")
                    {
                        if (_orderBy_vote != "")
                        {
                            if (_orderBy_vote == "desc")
                            {
                                _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ThenByDescending(x => x.importance_vote).ToList();
                            }
                            else
                            {
                                _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ThenBy(x => x.importance_vote).ToList();
                            }
                        }
                        else
                        {
                            _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ToList();
                        }
                    }
                    else
                    {
                        if (_orderBy_vote != "")
                        {
                            if (_orderBy_vote == "desc")
                            {
                                _listWithPrice = _listWithPrice.OrderBy(x => x.price).ThenByDescending(x => x.importance_vote).ToList();
                            }
                            else
                            {
                                _listWithPrice = _listWithPrice.OrderBy(x => x.price).ThenBy(x => x.importance_vote).ToList();
                            }
                        }
                        else
                        {
                            _listWithPrice = _listWithPrice.OrderBy(x => x.price).ToList();
                        }
                    }
                }
                else if (_orderBy_vote != "")
                {
                    if (_orderBy_vote == "desc")
                    {
                        _listWithPrice = _listWithPrice.OrderByDescending(x => x.importance_vote).ToList();
                    }
                    else
                    {
                        _listWithPrice = _listWithPrice.OrderBy(x => x.importance_vote).ToList();
                    }
                }
                else if (_orderBy_title != "")
                {
                    _listWithPrice.AddRange(_listOnRequest);
                    _listOnRequest = new List<AppSettings.RNT_estate>();
                    if (_orderBy_title == "desc")
                    {
                        _listWithPrice = _listWithPrice.OrderByDescending(x => x.title).ToList();
                    }
                    else
                    {
                        _listWithPrice = _listWithPrice.OrderBy(x => x.title).ToList();
                    }
                }
                _estateList = new List<AppSettings.RNT_estate>();
                _estateList.AddRange(_listWithPrice);
                _estateList.AddRange(_listOnRequest);
            }
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
                return "<div class=\"loadingSrc\"><span>La ricerca non apportato risultati...<br /> <strong>Vi preghiamo di riprova con i parametri diversi.</strong> </span></div>";
            }
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
            string _pager = "<div class=\"ordina pageCount\">" + getPager(_allCount) + "</div>";
            string _response = "";
            // _response += _pager;
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
                    _response += "                  " + _rntEst.price.ToString("N2") + "<span style=\"font-size: 24px;\">&euro;</span>";
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
        protected string getEstateList_zone()
        {
            _estateList = CURRENT_RNT_estateList_zone;
            if (_estateList.Count > 0 && _estateList[0].pid_lang != _currLang)
            {
                foreach (AppSettings.RNT_estate _rntEst in _estateList)
                {
                    RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == _currLang);
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
                CURRENT_RNT_estateList_zone = _estateList;
            }
            if (_action == "first")
            {
                List<AppSettings.RNT_estate> _listWithPrice = CURRENT_RNT_estateList_zone.Where(x => x.price != 0).OrderBy(x => x.sequence).ThenBy(x => x.price).ToList();
                List<AppSettings.RNT_estate> _listOnRequest = CURRENT_RNT_estateList_zone.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();
                _estateList = new List<AppSettings.RNT_estate>();
                _estateList.AddRange(_listWithPrice);
                _estateList.AddRange(_listOnRequest);
                CURRENT_RNT_estateList_zone = _estateList;
            }
            else
            {
                List<AppSettings.RNT_estate> _listWithPrice = CURRENT_RNT_estateList_zone.Where(x => x.price != 0).OrderBy(x => x.sequence).ThenBy(x => x.price).ToList();
                List<AppSettings.RNT_estate> _listOnRequest = CURRENT_RNT_estateList_zone.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();
                if (_orderBy_price != "")
                {
                    if (_orderBy_price == "desc")
                    {
                        _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ThenByDescending(x => x.importance_vote).ToList();
                    }
                    else
                    {
                        _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ThenBy(x => x.importance_vote).ToList();
                    }
                    if (_orderBy_price == "desc")
                    {
                        if (_orderBy_vote != "")
                        {
                        }
                        else
                        {
                            _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ToList();
                        }
                    }
                    else
                    {
                        if (_orderBy_vote != "")
                        {
                            if (_orderBy_vote == "desc")
                            {
                                _listWithPrice = _listWithPrice.OrderBy(x => x.price).ThenByDescending(x => x.importance_vote).ToList();
                            }
                            else
                            {
                                _listWithPrice = _listWithPrice.OrderBy(x => x.price).ThenBy(x => x.importance_vote).ToList();
                            }
                        }
                        else
                        {
                            _listWithPrice = _listWithPrice.OrderBy(x => x.price).ToList();
                        }
                    }
                }
                else if (_orderBy_vote != "")
                {
                    if (_orderBy_vote == "desc")
                    {
                        _listWithPrice = _listWithPrice.OrderByDescending(x => x.importance_vote).ToList();
                    }
                    else
                    {
                        _listWithPrice = _listWithPrice.OrderBy(x => x.importance_vote).ToList();
                    }
                }
                else if (_orderBy_title != "")
                {
                    _listWithPrice.AddRange(_listOnRequest);
                    _listOnRequest = new List<AppSettings.RNT_estate>();
                    if (_orderBy_title == "desc")
                    {
                        _listWithPrice = _listWithPrice.OrderByDescending(x => x.title).ToList();
                    }
                    else
                    {
                        _listWithPrice = _listWithPrice.OrderBy(x => x.title).ToList();
                    }
                }
                _estateList = new List<AppSettings.RNT_estate>();
                _estateList.AddRange(_listWithPrice);
                _estateList.AddRange(_listOnRequest);
            }
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
                return "<div class=\"loadingSrc\"><span>La ricerca non apportato risultati...<br /> <strong>Vi preghiamo di riprova con i parametri diversi.</strong> </span></div>";
            }
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
            string _pager = "<div class=\"ordina pageCount\">" + getPager(_allCount) + "</div>";
            string _response = "";
           // _response += _pager;
            _response += "<input type=\"hidden\" id=\"hf_currPage\" value=\"" + _currPage + "\"/><ul class=\"display dett_view\">";
            foreach (AppSettings.RNT_estate _rntEst in _estateList)
            {
                _response += "<li>";
                _response += "  <a href=\"/" + _rntEst.page_path + "\" class=\"content_block\">";
                _response += "      <img width=\"162\" height=\"89px\" src=\"" + ((("" + _rntEst.img_preview_1).Trim() != "") ? IMG_ROOT + _rntEst.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" class=\"fotosmall\" alt=\"" + _rntEst.title + "\" />";
                _response += "      <span class=\"testi\">";
                _response += "          <span class=\"tit\">";
                _response += "              <h2>" + _rntEst.title + "</h2>";
                if (_rntEst.is_online_booking==1)
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
                    _response += "                  <span class=\"paxday\">" + CurrentSource.getSysLangValue("lbl2paxPerDay") + "</span>";
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
