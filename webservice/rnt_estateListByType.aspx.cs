using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class rnt_estateListByType : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        private List<AppSettings.RNT_estate> _estateList;
        private string CURRENT_SESSION_ID;
        private int _currLang;
        private string _currType;
        private string _searchTitle;
        private int _currPage;
        private int _numPerPage;
        private string _orderBy;
        private string _orderHow;
        private clTypeFilter _tf;
        private string IMG_ROOT
        {
            get { return Request.Url.AbsoluteUri.Contains("http://localhost") ? "http://192.168.1.150/Rir/rentalinrome/" : "/"; }
        }
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
                CURRENT_SESSION_ID = Request.QueryString["SESSION_ID"];
                _currLang = Request.QueryString["lang"].objToInt32() == 0 ? CurrentLang.ID : Request.QueryString["lang"].objToInt32();
                _currType = Request.QueryString["currType"];
                _tf = new clTypeFilter(_currType);
                _tf.currPage = Request.QueryString["currPage"].objToInt32();
                _tf.numPerPage = Request.QueryString["numPerPage"].objToInt32() <= 0 ? 10 : Request.QueryString["numPerPage"].objToInt32();
                _tf.searchTitle = (Request.QueryString["title"] + "").Trim().ToLower().urlDecode();
                _tf.orderBy = Request.QueryString["orderBy"] + "";
                _tf.orderHow = Request.QueryString["orderHow"] + "";
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                _config.typeFilters.RemoveAll(x => x.currType == _currType);
                _config.typeFilters.Add(_tf);
                _config.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(_config);

                Response.Write(getEstateList());
                Response.End();
            }
        }
        private List<AppSettings.RNT_estate> TMPestateList;
        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList
        {
            get
            {
                if (TMPestateList == null)
                {
                    List<AppSettings.RNT_estate> estateList;
                    if (_currType == "longTerm")
                        estateList = AppSettings.RNT_estateList.Where(x => x.longTermRent == 1).ToList();
                    else
                        estateList = new List<AppSettings.RNT_estate>();
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
                    TMPestateList = estateList;
                }
                return TMPestateList;
            }
            set { TMPestateList = value; }
        }
        protected string getEstateList()
        {
            string orderBy_title = "";
            string orderBy_vote = "";
            string orderBy_price = "";
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

            if (_estateList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                return _errorStr;
            }
            List<AppSettings.RNT_estate> _listWithPrice = _estateList.Where(x => x.price != 0).OrderByDescending(x => x.is_online_booking).ThenBy(x => x.sequence).ThenBy(x => x.price).ToList();
            List<AppSettings.RNT_estate> _listOnRequest = _estateList.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();

            // ordina per prezzo
            if (_tf.orderBy == "price" && _tf.orderHow == "asc")
            { _listWithPrice = (_currType == "longTerm") ? _listWithPrice.OrderBy(x => x.longTermPrMonthly).ToList() : _listWithPrice.OrderBy(x => x.price).ToList(); orderBy_price = "asc"; }
            if (_tf.orderBy == "price" && _tf.orderHow == "desc")
            { _listWithPrice = (_currType == "longTerm") ? _listWithPrice.OrderByDescending(x => x.longTermPrMonthly).ToList() : _listWithPrice.OrderByDescending(x => x.price).ToList(); orderBy_price = "desc"; }


            _estateList = new List<AppSettings.RNT_estate>();
            _estateList.AddRange(_listWithPrice);
            _estateList.AddRange(_listOnRequest);

            // filtri
            if (!string.IsNullOrEmpty(_tf.searchTitle))
                _estateList = _estateList.Where(x => !string.IsNullOrEmpty(x.title) && x.title.ToLower().Contains(_tf.searchTitle)).ToList();

            if (_estateList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                return _errorStr;
            }
            // ordina per voto
            if (_tf.orderBy == "vote" && _tf.orderHow == "asc")
            { _estateList = _estateList.OrderBy(x => x.importance_vote).ToList(); orderBy_vote = "asc"; }
            if (_tf.orderBy == "vote" && _tf.orderHow == "desc")
            { _estateList = _estateList.OrderByDescending(x => x.importance_vote).ToList(); orderBy_vote = "desc"; }
            // ordina per titolo
            if (_tf.orderBy == "title" && _tf.orderHow == "asc")
            { _estateList = _estateList.OrderBy(x => x.title).ToList(); orderBy_title = "asc"; }
            if (_tf.orderBy == "title" && _tf.orderHow == "desc")
            { _estateList = _estateList.OrderByDescending(x => x.title).ToList(); orderBy_title = "desc"; }


            int _allCount = _estateList.Count;
            int _currCount = _allCount;
            if (_currCount == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                return _errorStr;
            }
            if (_tf.currPage != 0)
            {
                int _pageCount = 1;
                int _currPageCount = _tf.numPerPage;
                while (_currCount > _currPageCount)
                {
                    _pageCount++;
                    _currCount -= _tf.numPerPage;
                }
                if (_pageCount < _tf.currPage) _tf.currPage = _pageCount;
                if (_pageCount == _tf.currPage && _currCount < _currPageCount) _currPageCount = _currCount;
                _estateList = _estateList.GetRange((_tf.currPage - 1) * _tf.numPerPage, _currPageCount);
            }
            string _pager = "<div class=\"ordina pageCount\">" + CommonUtilities.getPager(_allCount, _tf.numPerPage, _tf.currPage, 10, "<span class=\"page\">" + CurrentSource.getSysLangValue("listAllPages") + "</span>", "<a class=\"page\" href=\"javascript:RNT_changePage(0)\">" + CurrentSource.getSysLangValue("listAllPages") + "</a>", "<span class=\"page\">{0}</span>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">{0}</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>") + "</div>";
            string itemPlaceHolder = "";
            itemPlaceHolder += "<input type=\"hidden\" id=\"hf_currPage\" value=\"" + _tf.currPage + "\"/><ul class=\"display dett_view\">";
            string _ItemTemplate = ltrItemTemplate.Text;
            foreach (AppSettings.RNT_estate _rntEst in _estateList)
            {
                ///////
                string _ItemTemplateSingle = _ItemTemplate.Clone() as string;
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#id#", "" + _rntEst.id);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_1#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_1) ? _rntEst.img_preview_1 : "images/css/default-apt-list.jpg"));
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_2#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_2) ? _rntEst.img_preview_2 : "images/css/default-apt-list.jpg"));
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_3#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_3) ? _rntEst.img_preview_3 : "images/css/default-apt-list.jpg"));
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#title#", "" + _rntEst.title);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#page_path#", "" + _rntEst.page_path);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#zoneTitle#", "" + _rntEst.zone);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#summary#", "" + _rntEst.summary);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#onlineBooking#", "" + ((_rntEst.is_online_booking == 1) ? "<img src=\"/images/css/book-now.gif\" class=\"ico_book_list\">" : ""));
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#vote#", "" + _rntEst.importance_vote);
                if (_currType == "longTerm" && _rntEst.longTermPrMonthly > 0)
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", "<span class=\"euro\" style=\"height: 45px;\"><span class=\"paxday\">" + CurrentSource.getSysLangValue("lblFrom") + "</span>" + _rntEst.longTermPrMonthly.ToString("N2") + "<span style=\"font-size: 24px; margin-left: 5px;\">€</span><span class=\"paxday\">" + CurrentSource.getSysLangValue("lblMonthly") + "</span></span>");
                else if (_rntEst.price > 0)
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", "<span class=\"euro\" style=\"height: 23px;\">" + _rntEst.price.ToString("N2") + "<span style=\"font-size: 24px;\">€</span></span>");
                else
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", "<span class=\"euro richiesta\" style=\"height: 23px;\"><span>" + CurrentSource.getSysLangValue("lblOnRequest") + "</span></span>");
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("##", "" + _rntEst.id);
                itemPlaceHolder += _ItemTemplateSingle;
            }
            string LayoutTemplate = ltrLayoutTemplate.Text;
            LayoutTemplate = LayoutTemplate.Replace("#orderBy#", _tf.orderBy);
            LayoutTemplate = LayoutTemplate.Replace("#orderHow#", _tf.orderHow);
            LayoutTemplate = LayoutTemplate.Replace("#listType#", _tf.listType);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_title#", orderBy_title);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_vote#", orderBy_vote);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_price#", orderBy_price);
            LayoutTemplate = LayoutTemplate.Replace("#currPage#", _tf.currPage.ToString());
            LayoutTemplate = LayoutTemplate.Replace("#itemPlaceHolder#", itemPlaceHolder);
            LayoutTemplate = LayoutTemplate.Replace("#pagerPlaceHolder#", _pager);
            LayoutTemplate = LayoutTemplate.Replace("#lblOrderBy#", CurrentSource.getSysLangValue("lblOrderBy", _currLang, "#lblOrderBy#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblName#", CurrentSource.getSysLangValue("lblName", _currLang, "#lblName#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblPrice#", CurrentSource.getSysLangValue("lblPrice", _currLang, "#lblPrice#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblRating#", CurrentSource.getSysLangValue("lblRating", _currLang, "#lblRating#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblViewDetails#", CurrentSource.getSysLangValue("lblViewDetails", _currLang, "#lblViewDetails#"));

            return LayoutTemplate;
        }
    }
}
