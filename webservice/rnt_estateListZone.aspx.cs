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
    public partial class rnt_estateListZone : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        private List<AppSettings.RNT_estate> _estateList;
        private string CURRENT_SESSION_ID;
        private int _currLang;
        private int _currZone;
        private string _searchTitle;
        private int _currPage;
        private int _numPerPage;
        private string _orderBy;
        private string _orderHow;
        private clZoneFilter _zf;

        private int _isWL = 0;
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
                _currZone = Request.QueryString["currZone"].objToInt32();

                _isWL = Request.QueryString["isWL"].objToInt32();

                _zf = new clZoneFilter();
                _zf.currZone = _currZone;
                _zf.currPage = Request.QueryString["currPage"].objToInt32();
                _zf.numPerPage = Request.QueryString["numPerPage"].objToInt32() <= 0 ? 10 : Request.QueryString["numPerPage"].objToInt32();
                _zf.searchTitle = (Request.QueryString["title"] + "").Trim().ToLower().urlDecode();
                _zf.orderBy = Request.QueryString["orderBy"] + "";
                _zf.orderHow = Request.QueryString["orderHow"] + "";
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                _config.zoneFilters.RemoveAll(x => x.currZone == _currZone);
                _config.zoneFilters.Add(_zf);
                _config.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(_config);

                Response.Write(getEstateList());
                Response.End();
            }
        }
        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList
        {
            get
            {
                if (AppSettings.RELOAD_CACHE || HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone] == null)
                {
                    List<AppSettings.RNT_estate> estateList;
                    if (_currZone == 0)
                        estateList = AppSettings.RNT_estateList.Where(x => !AppSettings._LOC_CUSTOM_ZONEs.Contains(x.pid_zone)).OrderBy(x => x.zoneSequence).ToList();
                    else
                        estateList = AppSettings.RNT_estateList.Where(x => x.pid_zone == _currZone).OrderBy(x => x.zoneSequence).ToList();


                    #region Check White Label
                    int Wl_changeAmount = 0;
                    int WL_changeIsDiscount = 0;
                    int WL_changeIsPercentage = 0;

                    int Wl_changeAmount_Agent = 0;
                    int WL_changeIsDiscount_Agent = 0;
                    int WL_changeIsPercentage_Agent = 0;

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
                            Wl_changeAmount = agentTbl.WL_changeAmount.objToInt32();
                            WL_changeIsDiscount = agentTbl.WL_changeIsDiscount == null ? 0 : agentTbl.WL_changeIsDiscount.objToInt32();
                            WL_changeIsPercentage = agentTbl.WL_changeIsPercentage == null ? 1 : agentTbl.WL_changeIsPercentage.objToInt32();

                            Wl_changeAmount_Agent = agentTbl.WL_changeAmount_Agent.objToInt32();
                            WL_changeIsDiscount_Agent = agentTbl.WL_changeIsDiscount_Agent == null ? 0 : agentTbl.WL_changeIsDiscount_Agent.objToInt32();
                            WL_changeIsPercentage_Agent = agentTbl.WL_changeIsPercentage_Agent == null ? 1 : agentTbl.WL_changeIsPercentage_Agent.objToInt32();
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
                        decimal price = rntUtils.rntEstate_minPrice(_rntEst.id);

                        #region Check White Label & Increase Price
                        if (_isWL == 1 && agentTbl != null && price != 0)
                        {
                            price = CommonUtilities.GetChangedAmt(price, Wl_changeAmount, WL_changeIsDiscount, WL_changeIsPercentage);
                            price = CommonUtilities.GetChangedAmt(price, Wl_changeAmount_Agent, WL_changeIsDiscount_Agent, WL_changeIsPercentage_Agent);
                        }
                        #endregion
                        _rntEst.price = price;
                        _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), _currLang, "");
                    }
                    HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone] = estateList;
                }
                return (List<AppSettings.RNT_estate>)HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone];
            }
            set { HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone] = value; }
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
            var spIds = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd >= DateTime.Now && x.is_active == 1).Select(x => x.pid_estate.objToInt32()).ToList();
            if (CommonUtilities.getSYS_SETTING("is_special_offer_first") == "true")
            {
                foreach (var tmp in _estateList.Where(x => !spIds.Contains(x.id)))
                {
                    tmp.zoneSequence = tmp.zoneSequence + 99999;
                }
            }
            List<AppSettings.RNT_estate> _listWithPrice = _estateList.Where(x => x.price != 0).OrderByDescending(x => x.is_online_booking).ThenBy(x => x.zoneSequence).ThenBy(x => x.price).ToList();
            List<AppSettings.RNT_estate> _listOnRequest = _estateList.Where(x => x.price == 0).OrderBy(x => x.zoneSequence).ToList();

            // ordina per prezzo
            if (_zf.orderBy == "price" && _zf.orderHow == "asc")
            { _listWithPrice = _listWithPrice.OrderBy(x => x.price).ToList(); orderBy_price = "asc"; }
            if (_zf.orderBy == "price" && _zf.orderHow == "desc")
            { _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ToList(); orderBy_price = "desc"; }


            _estateList = new List<AppSettings.RNT_estate>();
            _estateList.AddRange(_listWithPrice);
            _estateList.AddRange(_listOnRequest);

            // filtri
            if (!string.IsNullOrEmpty(_zf.searchTitle))
                _estateList = _estateList.Where(x => !string.IsNullOrEmpty(x.title) && x.title.ToLower().Contains(_zf.searchTitle)).ToList();

            if (_estateList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                return _errorStr;
            }
            // ordina per voto
            if (_zf.orderBy == "vote" && _zf.orderHow == "asc")
            { _estateList = _estateList.OrderBy(x => x.importance_vote).ToList(); orderBy_vote = "asc"; }
            if (_zf.orderBy == "vote" && _zf.orderHow == "desc")
            { _estateList = _estateList.OrderByDescending(x => x.importance_vote).ToList(); orderBy_vote = "desc"; }
            // ordina per titolo
            if (_zf.orderBy == "title" && _zf.orderHow == "asc")
            { _estateList = _estateList.OrderBy(x => x.title).ToList(); orderBy_title = "asc"; }
            if (_zf.orderBy == "title" && _zf.orderHow == "desc")
            { _estateList = _estateList.OrderByDescending(x => x.title).ToList(); orderBy_title = "desc"; }


            int _allCount = _estateList.Count;
            int _currCount = _allCount;
            if (_currCount == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                return _errorStr;
            }
            if (_zf.currPage != 0)
            {
                int _pageCount = 1;
                int _currPageCount = _zf.numPerPage;
                while (_currCount > _currPageCount)
                {
                    _pageCount++;
                    _currCount -= _zf.numPerPage;
                }
                if (_pageCount < _zf.currPage) _zf.currPage = _pageCount;
                if (_pageCount == _zf.currPage && _currCount < _currPageCount) _currPageCount = _currCount;
                _estateList = _estateList.GetRange((_zf.currPage - 1) * _zf.numPerPage, _currPageCount);
            }
            string _pager = "<div class=\"ordina pageCount\">" + CommonUtilities.getPager(_allCount, _zf.numPerPage, _zf.currPage, 10, "<span class=\"page\">" + CurrentSource.getSysLangValue("listAllPages") + "</span>", "<a class=\"page\" href=\"javascript:RNT_changePage(0)\">" + CurrentSource.getSysLangValue("listAllPages") + "</a>", "<span class=\"page\">{0}</span>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">{0}</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>") + "</div>";
            string itemPlaceHolder = "";
            itemPlaceHolder += "<input type=\"hidden\" id=\"hf_currPage\" value=\"" + _zf.currPage + "\"/><ul class=\"display dett_view\">";
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

                if (_isWL == 0)
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#onlineBooking#", "" + ((_rntEst.is_online_booking == 1) ? "<img src=\"/images/css/book-now.gif\" class=\"ico_book_list\">" : ""));
                else
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#onlineBooking#", "" + ((_rntEst.is_online_booking == 1) ? "<img src=\"/WLRental/images/book-now.gif\" class=\"ico_book_list\">" : ""));

                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#vote#", "" + _rntEst.importance_vote);
                if (_rntEst.price > 0)
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", "<span class=\"euro\" style=\"height: 23px;\">" + _rntEst.price.ToString("N2") + "<span style=\"font-size: 24px;\">€</span></span>");
                else
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", "<span class=\"euro richiesta\" style=\"height: 23px;\"><span>" + CurrentSource.getSysLangValue("lblOnRequest") + "</span></span>");
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#specialOfferIco#", "" + ((spIds.Contains(_rntEst.id)) ? "<span class='specialOfferIco'></span>" : ""));
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("##", "" + _rntEst.id);

                itemPlaceHolder += _ItemTemplateSingle;
            }
            string LayoutTemplate = ltrLayoutTemplate.Text;
            LayoutTemplate = LayoutTemplate.Replace("#orderBy#", _zf.orderBy);
            LayoutTemplate = LayoutTemplate.Replace("#orderHow#", _zf.orderHow);
            LayoutTemplate = LayoutTemplate.Replace("#listType#", _zf.listType);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_title#", orderBy_title);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_vote#", orderBy_vote);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_price#", orderBy_price);
            LayoutTemplate = LayoutTemplate.Replace("#currPage#", _zf.currPage.ToString());
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
