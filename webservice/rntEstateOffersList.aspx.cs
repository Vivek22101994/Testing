using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using System.IO;
using Newtonsoft.Json;
using ModRental;

namespace RentalInRome.webservice
{
    public partial class rntEstateOffersList : System.Web.UI.Page
    {
        protected class FltListItem
        {
            public string text { get; set; }
            public string value { get; set; }
            public bool selected { get; set; }
            public FltListItem(string Text, string Value, bool Selected)
            {
                value = Value;
                text = Text;
                selected = Selected;
            }
        }
        protected class FltRangeItem
        {
            public int min { get; set; }
            public int max { get; set; }
            public int start { get; set; }
            public int end { get; set; }
            public FltRangeItem()
            {
                min = 0;
                max = 0;
                start = 0;
                end = 0;
            }
        }
        protected class CurrResponseClass
        {
            public List<CurrListClass> list { get; set; }
            public listPages pages { get; set; }
            public List<FltListItem> fltCityList { get; set; }
            public List<FltListItem> fltZoneList { get; set; }
            public List<FltListItem> fltTypeList { get; set; }
            public List<FltListItem> fltExtrasList { get; set; }
            public List<FltListItem> fltBedroomList { get; set; }
            public List<FltListItem> fltBathroomList { get; set; }
            public FltRangeItem priceRange { get; set; }
            public FltRangeItem mqRange { get; set; }
            public FltRangeItem numSleeps { get; set; }
            public string orderBy { get; set; }
            public string orderHow { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
            public CurrResponseClass()
            {
                list = new List<CurrListClass>();
                pages = null;
                fltCityList = new List<FltListItem>();
                fltZoneList = new List<FltListItem>();
                fltTypeList = new List<FltListItem>();
                fltExtrasList = new List<FltListItem>();
                fltBedroomList = new List<FltListItem>();
                fltBathroomList = new List<FltListItem>();
                priceRange = new FltRangeItem();
                mqRange = new FltRangeItem();
                numSleeps = new FltRangeItem();
                orderBy = "";
                orderHow = "";
                page = 0;
                pageSize = 0;
            }
        }
        protected class CurrListClass
        {
            public long id { get; set; }
            public decimal price { get; set; }
            public string priceFormatted { get; set; }
            public decimal priceNoDiscount { get; set; }
            public string priceNoDiscountFormatted { get; set; }
            public bool nicePrice { get; set; }
            public decimal pr_agentCommissionPrice { get; set; }
            public int priceError { get; set; }
            public string locZoneFull { get; set; }
            public string extraInfo { get; set; }
            public string imgPreview { get; set; }
            public string imgPreview2 { get; set; }
            public string imgPreview3 { get; set; }
            public string gmGmapCoords { get; set; }
            public string dateSpecOff { get; set; }
            public DateTime dateStart { get; set; }
            public string dateStartFormatted { get; set; }
            public DateTime dateEnd { get; set; }
            public string dateEndFormatted { get; set; }


            public string title { get; set; }
            public string summary { get; set; }
            public string pagePath { get; set; }
            public List<string> extrasList { get; set; }
            public string extrasListFormatted { get; set; }
            public List<string> imgList { get; set; }
            public CurrListClass(long Id, DateTime DateStart, DateTime DateEnd, int PidLang)
            {
                id = Id;
                price = 0;
                priceFormatted = "";
                priceNoDiscount = 0;
                priceNoDiscountFormatted = "";
                nicePrice = false;
                pr_agentCommissionPrice = 0;
                priceError = 0;
                locZoneFull = "";
                extraInfo = "";
                imgPreview = "";
                imgPreview2 = "";
                imgPreview3 = "";
                gmGmapCoords = "";
                dateSpecOff = "";
                dateStart = DateStart;
                dateStartFormatted = DateStart.formatCustom("#dd# #MM# #yy#", PidLang, "--/--/----");
                dateEnd = DateEnd;
                dateEndFormatted = DateEnd.formatCustom("#dd# #MM# #yy#", PidLang, "--/--/----");
                title = "";
                summary = "";
                pagePath = "";
                extrasList = new List<string>();
                extrasListFormatted = "";
                imgList = new List<string>();
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private string CURRENT_SESSION_ID;
        private int _currLang;
        private string _searchTitle;
        private int _currPage;
        private int _numPerPage;
        private string _orderBy;
        private string _orderHow;
        private clTypeFilter currSearch;

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
                CURRENT_SESSION_ID = Request["SESSION_ID"];
                _currLang = Request["lang"].objToInt32() == 0 ? CurrentLang.ID : Request["lang"].objToInt32();
                currSearch = new clTypeFilter("OffersList");
                currSearch.currPage = Request["currPage"].objToInt32();
                currSearch.currZone = Request["currZone"].objToInt32();
                currSearch.numPerPage = Request["numPerPage"].objToInt32() <= 0 ? 10 : Request["numPerPage"].objToInt32();
                currSearch.searchTitle = (Request["title"] + "").Trim().ToLower().urlDecode();
                currSearch.orderBy = Request["orderBy"] + "";
                currSearch.orderHow = Request["orderHow"] + "";

                _isWL = Request["isWL"].objToInt32();

                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                _config.typeFilters.RemoveAll(x => x.currType == "OffersList");
                _config.typeFilters.Add(currSearch);
                _config.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(_config);
                if (Request["json"] == "true")
                    getEstateListJson();
                else if (Request["newsletter"] == "true")
                    Response.Write(getEstateList_newsletter());
                else
                    Response.Write(getEstateList());
                Response.End();
            }
        }
        protected void getEstateListJson()
        {
            List<int?> tmpIds = new List<int?>();
            string orderBy_dtStart = "";
            string orderBy_dtEnd = "";
            string orderBy_price = "";
            tmpIds = AppSettings.RNT_estateList.Where(x => x.pid_city == AppSettings.RNT_currCity).Select(x => (int?)x.id).ToList();
            var specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => tmpIds.Contains(x.pid_estate) && x.dtEnd > DateTime.Now.AddDays(5) && x.is_active == 1 && x.pr_discount > 0 && x.dtStart.HasValue && x.dtEnd.HasValue).OrderBy(x => x.dtEnd).ThenByDescending(x => x.pr_discount).ToList();

            if (currSearch.currZone > 0)
            {
                tmpIds = AppSettings.RNT_estateList.Where(x => x.pid_zone == currSearch.currZone).Select(x => (int?)x.id).ToList();
                specialOfferList = specialOfferList.Where(x => tmpIds.Contains(x.pid_estate)).ToList();
            }


            // ordina per prezzo
            if (currSearch.orderBy == "price" && currSearch.orderHow == "asc")
            { specialOfferList = specialOfferList.OrderBy(x => x.pr_discount).ToList(); orderBy_price = "asc"; }
            if (currSearch.orderBy == "price" && currSearch.orderHow == "desc")
            { specialOfferList = specialOfferList.OrderByDescending(x => x.pr_discount).ToList(); orderBy_price = "desc"; }

            // ordina per datainizio
            if (currSearch.orderBy == "dtStart" && currSearch.orderHow == "asc")
            { specialOfferList = specialOfferList.OrderBy(x => x.dtStart).ToList(); orderBy_dtStart = "asc"; }
            if (currSearch.orderBy == "dtStart" && currSearch.orderHow == "desc")
            { specialOfferList = specialOfferList.OrderByDescending(x => x.dtStart).ToList(); orderBy_dtStart = "desc"; }

            // ordina per datafine
            if (currSearch.orderBy == "dtEnd" && currSearch.orderHow == "asc")
            { specialOfferList = specialOfferList.OrderBy(x => x.dtEnd).ToList(); orderBy_dtEnd = "asc"; }
            if (currSearch.orderBy == "dtEnd" && currSearch.orderHow == "desc")
            { specialOfferList = specialOfferList.OrderByDescending(x => x.dtEnd).ToList(); orderBy_dtEnd = "desc"; }


            int allCount = specialOfferList.Count;
            int currCount = allCount;
            if (currSearch.currPage != 0)
            {
                int _pageCount = 1;
                int _currPageCount = currSearch.numPerPage;
                while (currCount > _currPageCount)
                {
                    _pageCount++;
                    currCount -= currSearch.numPerPage;
                }
                if (_pageCount < currSearch.currPage) currSearch.currPage = _pageCount;
                if (_pageCount == currSearch.currPage && currCount < _currPageCount) _currPageCount = currCount;
                specialOfferList = specialOfferList.GetRange((currSearch.currPage - 1) * currSearch.numPerPage, _currPageCount);
            }
            var configList = maga_DataContext.DC_RENTAL.RNT_TB_CONFIGs.Where(x => AppSettings._RNT_CUSTOM_CONFIGs.Contains(x.id)).ToList();
            var currResp = new CurrResponseClass();
            var currList = new List<CurrListClass>();
            foreach (var tmpOffer in specialOfferList)
            {
                AppSettings.RNT_estate tmpTb = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == tmpOffer.pid_estate);
                if (tmpTb == null) continue;
                RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == tmpTb.id && x.pid_lang == _currLang && !string.IsNullOrEmpty(x.title));
                if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == tmpTb.id && x.pid_lang == 2);
                if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == tmpTb.id && x.pid_lang == 1);
                if (_lang == null) continue;

                tmpTb.pid_lang = _currLang;
                tmpTb.title = _lang.title;
                tmpTb.summary = _lang.summary;
                tmpTb.page_path = _lang.page_path;
                tmpTb.price = tmpOffer.pr_discount.objToDecimal();
                tmpTb.zone = CurrentSource.locZone_title(tmpTb.pid_zone.objToInt32(), _currLang, "");

                var tmpResp = new CurrListClass(tmpTb.id, tmpOffer.dtStart.Value, tmpOffer.dtEnd.Value, _currLang);

                tmpResp.price = tmpTb.price;
                tmpResp.priceFormatted = tmpTb.price.ToString("N2");
                tmpResp.nicePrice = tmpTb.nicePrice;
                tmpResp.pr_agentCommissionPrice = tmpTb.pr_agentCommissionPrice;
                tmpResp.priceError = tmpTb.priceError;
                tmpResp.locZoneFull = tmpTb.zone;
                tmpResp.extraInfo = "";
                tmpResp.imgPreview = "/" + tmpTb.img_preview_1;
                tmpResp.imgPreview2 = "/" + tmpTb.img_preview_2;
                tmpResp.imgPreview3 = "/" + tmpTb.img_preview_3;
                tmpResp.dateSpecOff = "<span class=\"fromTo\">" + CurrentSource.getSysLangValue("lblDateFrom", _currLang, "From") + "</span><span class=\"data\">" + tmpOffer.dtStart.formatCustom("#dd# #MM# #yy#", _currLang, "--/--/----") + "</span><span class=\"fromTo\">" + CurrentSource.getSysLangValue("lblDateTo", _currLang, "To") + "</span><span class=\"data\">" + tmpOffer.dtEnd.formatCustom("#dd# #MM# #yy#", _currLang, "--/--/----") + "</span>";

                tmpResp.title = tmpTb.title;
                tmpResp.summary = tmpTb.summary;
                tmpResp.pagePath = "/" + tmpTb.page_path + "?dtS=" + tmpOffer.dtStart.JSCal_dateToString() + "&dtE=" + tmpOffer.dtEnd.JSCal_dateToString();

                var estateConfigList = configList.Where(x => tmpTb.configList.Contains(x.id));
                string estateConfigString = "";
                foreach (var config in estateConfigList)
                {
                    estateConfigString += "<img src=\"/" + config.img_thumb + "\" alt=\"\" />";
                }
                tmpResp.extrasListFormatted = estateConfigString;

                currList.Add(tmpResp);
            }
            currResp.list = currList;
            currResp.pages = new listPages(allCount, currSearch.numPerPage, currSearch.currPage, 10, "<span class=\"currPage\">" + contUtils.getLabel("listAllPages") + "</span>", "<a class=\"listPageLink\" href=\"javascript:AjaxList_changePage(0)\">" + contUtils.getLabel("listAllPages") + "</a>", "<span class=\"currPage\">{0}</span>", "<a class=\"listPageLink\" href=\"javascript:AjaxList_changePage({0})\">{0}</a>", "<a class=\"listPageLink\" href=\"javascript:AjaxList_changePage({0})\">..</a>", "<a class=\"listPageLink\" href=\"javascript:AjaxList_changePage({0})\">..</a>");
            currResp.page = currSearch.currPage;
            currResp.pageSize = currResp.pages.pagesCount;
            EndRequest(currResp);
        }
        protected void EndRequest(CurrResponseClass resp)
        {
            TextWriter wr = new StringWriter();
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(wr, resp);
            Response.Write(wr.ToString());
            Response.End();
        }
        protected string getEstateList_newsletter()
        {
            List<RNT_TB_SPECIAL_OFFER> specialOfferList;
            if (Request["dts"].ToInt32() > 0 && Request["dtf"].ToInt32() > 0)
                specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtStart >= Request["dts"].JSCal_stringToDate() && x.dtEnd <= Request["dtf"].JSCal_stringToDate() && x.is_active == 1 && x.pr_discount > 0).OrderBy(x => x.dtEnd).ThenByDescending(x => x.pr_discount).ToList();
            else
                specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd > DateTime.Now.AddDays(5) && x.is_active == 1 && x.pr_discount > 0).OrderBy(x => x.dtEnd).ThenByDescending(x => x.pr_discount).ToList();

            List<int> estateIDs = specialOfferList.Select(x => x.pid_estate.objToInt32()).Distinct().ToList();
            List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.Where(x => estateIDs.Contains(x.id)).ToList();
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

            if (specialOfferList.Count == 0 || estateList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                return _errorStr;
            }
            string zonePlaceHolder = "";
            string zoneTemplate = "";
            string itemPlaceHolder = "";
            string itemTemplate = "";
            List<LOC_VIEW_ZONE> zoneList = AppSettings.LOC_ZONEs.Where(x => x.pid_lang == _currLang).OrderBy(x => x.title).ToList();
            foreach (LOC_VIEW_ZONE tmpZone in zoneList)
            {
                List<AppSettings.RNT_estate> tmpEstateList = estateList.Where(x => x.pid_zone == tmpZone.id).ToList();
                if (tmpEstateList.Count == 0) continue;
                itemPlaceHolder = "";
                foreach (AppSettings.RNT_estate _rntEst in tmpEstateList)
                {
                    RNT_TB_SPECIAL_OFFER tmpOffer = specialOfferList.Where(x=>x.pid_estate==_rntEst.id).OrderBy(x=>x.dtEnd).FirstOrDefault();
                    if (tmpOffer == null) continue;
                    itemTemplate = ltrItemTemplate_newsletter.Text;
                    itemTemplate = itemTemplate.Replace("#id#", "" + _rntEst.id);
                    itemTemplate = itemTemplate.Replace("#img_preview_1#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_1) ? _rntEst.img_preview_1 : "images/css/default-apt-list.jpg"));
                    itemTemplate = itemTemplate.Replace("#img_preview_2#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_2) ? _rntEst.img_preview_2 : "images/css/default-apt-list.jpg"));
                    itemTemplate = itemTemplate.Replace("#img_preview_3#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_3) ? _rntEst.img_preview_3 : "images/css/default-apt-list.jpg"));
                    itemTemplate = itemTemplate.Replace("#title#", "" + _rntEst.title);
                    string page_path = "" + _rntEst.page_path;
                    if (!string.IsNullOrEmpty(Request["id_ad_media"])) page_path += page_path.Contains("?") ? "&id_ad_media=" + Request["id_ad_media"] : "?id_ad_media=" + Request["id_ad_media"];
                    if (!string.IsNullOrEmpty(Request["id_link"])) page_path += page_path.Contains("?") ? "&id_link=" + Request["id_link"] : "?id_link=" + Request["id_link"];
                    itemTemplate = itemTemplate.Replace("#page_path#", "" + page_path);
                    itemTemplate = itemTemplate.Replace("#zoneTitle#", "" + _rntEst.zone);
                    itemTemplate = itemTemplate.Replace("#summary#", "" + _rntEst.summary);
                    itemTemplate = itemTemplate.Replace("#onlineBooking#", "" + ((_rntEst.is_online_booking == 1) ? ltr_onlineBookingOK.Text.Replace("#onlineBooking#", "Instant booking") : ltr_onlineBookingNO.Text.Replace("#onlineBooking#", CurrentSource.getSysLangValue("lblEnquire", _currLang, "Enquire"))));
                    itemTemplate = itemTemplate.Replace("#vote#", "" + _rntEst.importance_vote);
                    itemTemplate = itemTemplate.Replace("#price#", "-" + tmpOffer.pr_discount.objToInt32() + "%");
                    itemTemplate = itemTemplate.Replace("#dateSpecOff#", "<span style=\"float:left; display:block; color:#666; padding:0 10px 0 0; margin-top:10px;\">" + CurrentSource.getSysLangValue("lblDateFrom", _currLang, "From") + "</span><span style=\"float:left; display:block; color:#333366; padding:0 20px 0 0; margin-top:10px;\">" + tmpOffer.dtStart.formatCustom("#dd# #MM# #yy#", _currLang, "--/--/----") + "</span><span style=\"float:left; display:block; color:#666; padding:0 10px 0 0; margin-top:10px;\">" + CurrentSource.getSysLangValue("lblDateTo", _currLang, "To") + "</span><span style=\"float:left; display:block; color:#333366; padding:0 20px 0 0; margin-top:10px;\">" + tmpOffer.dtEnd.formatCustom("#dd# #MM# #yy#", _currLang, "--/--/----") + "</span>");
                    itemPlaceHolder += itemTemplate;
                }
                zoneTemplate = ltrZoneTemplate_newsletter.Text;
                zoneTemplate = zoneTemplate.Replace("#title#", "" + tmpZone.title);
                zoneTemplate = zoneTemplate.Replace("#itemPlaceHolder#", "" + itemPlaceHolder);
                zonePlaceHolder += zoneTemplate;
            }
            string LayoutTemplate = ltrLayoutTemplate_newsletter.Text;
            LayoutTemplate = LayoutTemplate.Replace("#lnkPagePath#", "http://www.rentalinrome.com" + CurrentSource.getPagePath("37", "stp", _currLang.ToString()));
            LayoutTemplate = LayoutTemplate.Replace("#lblLinkViewNewsletter#", CurrentSource.getSysLangValue("lblLinkViewNewsletter", _currLang, "Can't see the newsletter? click here"));
            LayoutTemplate = LayoutTemplate.Replace("#lblLinkToRemoveFromNewsletter#", CurrentSource.getSysLangValue("lblLinkToRemoveFromNewsletter", _currLang, "Do you want to remove from our list? click <a href=\"http://www.rentalinrome.com/newsletter/forms/optOutForm.asp\">here:</a>"));
            LayoutTemplate = LayoutTemplate.Replace("#zonePlaceHolder#", zonePlaceHolder);
            string langCode = "en";
            CONT_TBL_LANG lang = contProps.LangTBL.SingleOrDefault(x => x.id == _currLang);
            if (lang != null)
                langCode = lang.abbr.Substring(0, 2);
            LayoutTemplate = LayoutTemplate.Replace("#langCode#", langCode);

            return LayoutTemplate;
        }

        protected string getEstateList()
        {
            string orderBy_dtStart = "";
            string orderBy_dtEnd = "";
            string orderBy_price = "";
            List<RNT_TB_SPECIAL_OFFER> specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd > DateTime.Now.AddDays(5) && x.is_active == 1 && x.pr_discount > 0).OrderBy(x => x.dtEnd).ThenByDescending(x => x.pr_discount).ToList();


            if (specialOfferList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                return _errorStr;
            }

            #region Check White Label
            if (_isWL == 1)
            {
                List<int> lstAgentEstate = new List<int>();
                using (DCmodRental dc = new DCmodRental())
                {
                    lstAgentEstate = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == App.WLAgentId).Select(x => x.pidEstate).Distinct().ToList();
                }
                specialOfferList = specialOfferList.Where(x => lstAgentEstate.Contains(x.pid_estate.objToInt32())).ToList();
            }
            #endregion
            
            if (specialOfferList.Count == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
                _errorStr = _errorStr.Replace("#lblApartmentSearchError#", CurrentSource.getSysLangValue("lblApartmentSearchError", _currLang));
                return _errorStr;
            }

            // ordina per prezzo
            if (currSearch.orderBy == "price" && currSearch.orderHow == "asc")
            { specialOfferList = specialOfferList.OrderBy(x => x.pr_discount).ToList(); orderBy_price = "asc"; }
            if (currSearch.orderBy == "price" && currSearch.orderHow == "desc")
            { specialOfferList = specialOfferList.OrderByDescending(x => x.pr_discount).ToList(); orderBy_price = "desc"; }

            // ordina per datainizio
            if (currSearch.orderBy == "dtStart" && currSearch.orderHow == "asc")
            { specialOfferList = specialOfferList.OrderBy(x => x.dtStart).ToList(); orderBy_dtStart = "asc"; }
            if (currSearch.orderBy == "dtStart" && currSearch.orderHow == "desc")
            { specialOfferList = specialOfferList.OrderByDescending(x => x.dtStart).ToList(); orderBy_dtStart = "desc"; }

            // ordina per datafine
            if (currSearch.orderBy == "dtEnd" && currSearch.orderHow == "asc")
            { specialOfferList = specialOfferList.OrderBy(x => x.dtEnd).ToList(); orderBy_dtEnd = "asc"; }
            if (currSearch.orderBy == "dtEnd" && currSearch.orderHow == "desc")
            { specialOfferList = specialOfferList.OrderByDescending(x => x.dtEnd).ToList(); orderBy_dtEnd = "desc"; }

            int _allCount = specialOfferList.Count;
            int _currCount = _allCount;
            if (_currCount == 0)
            {
                string _errorStr = ltrEmptyDataTemplate.Text;
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
                specialOfferList = specialOfferList.GetRange((currSearch.currPage - 1) * currSearch.numPerPage, _currPageCount);
            }
            string _pager = "<div class=\"ordina pageCount\">" + CommonUtilities.getPager(_allCount, currSearch.numPerPage, currSearch.currPage, 10, "<span class=\"page\">" + CurrentSource.getSysLangValue("listAllPages") + "</span>", "<a class=\"page\" href=\"javascript:RNT_changePage(0)\">" + CurrentSource.getSysLangValue("listAllPages") + "</a>", "<span class=\"page\">{0}</span>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">{0}</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>", "<a class=\"page\" href=\"javascript:RNT_changePage({0})\">..</a>") + "</div>";
            string itemPlaceHolder = "";
            itemPlaceHolder += "<input type=\"hidden\" id=\"hf_currPage\" value=\"" + currSearch.currPage + "\"/><ul class=\"display dett_view\">";
            string _ItemTemplate = ltrItemTemplate.Text;
            foreach (RNT_TB_SPECIAL_OFFER tmpOffer in specialOfferList)
            {
                AppSettings.RNT_estate _rntEst = AppSettings.RNT_estateList.SingleOrDefault(x=>x.id==tmpOffer.pid_estate);
                if (_rntEst == null) continue;
                RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == _currLang && !string.IsNullOrEmpty(x.title));
                if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 1);
                if (_lang == null) continue;

                _rntEst.pid_lang = _currLang;
                _rntEst.title = _lang.title;
                _rntEst.summary = _lang.summary;
                _rntEst.page_path = _lang.page_path;
                _rntEst.price = tmpOffer.pr_discount.objToDecimal();
                _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), _currLang, "");

                string _ItemTemplateSingle = _ItemTemplate.Clone() as string;
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#id#", "" + _rntEst.id);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_1#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_1) ? _rntEst.img_preview_1 : "images/css/default-apt-list.jpg"));
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_2#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_2) ? _rntEst.img_preview_2 : "images/css/default-apt-list.jpg"));
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_3#", "" + (!string.IsNullOrEmpty(_rntEst.img_preview_3) ? _rntEst.img_preview_3 : "images/css/default-apt-list.jpg"));
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#title#", "" + _rntEst.title);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#page_path#", "" + _rntEst.page_path + "?dtS=" + tmpOffer.dtStart.JSCal_dateToString() + "&dtE=" + tmpOffer.dtEnd.JSCal_dateToString());
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#zoneTitle#", "" + _rntEst.zone);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#summary#", "" + _rntEst.summary);
                
                if (_isWL == 0)
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#onlineBooking#", "" + ((_rntEst.is_online_booking == 1) ? "<img src=\"/images/css/book-now.gif\" class=\"ico_book_list\">" : ""));                    
                else
                    _ItemTemplateSingle = _ItemTemplateSingle.Replace("#onlineBooking#", "" + ((_rntEst.is_online_booking == 1) ? "<img src=\"/WLRental/images/book-now.gif\" class=\"ico_book_list\">" : ""));

                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#vote#", "" + _rntEst.importance_vote);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", "<span class=\"euro\">-" + _rntEst.price.objToInt32() + "%</span>");
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#dateSpecOff#", "<span class=\"fromTo\">" + CurrentSource.getSysLangValue("lblDateFrom", _currLang, "From") + "</span><span class=\"data\">" + tmpOffer.dtStart.formatCustom("#dd# #MM# #yy#", _currLang, "--/--/----") + "</span><span class=\"fromTo\">" + CurrentSource.getSysLangValue("lblDateTo", _currLang, "To") + "</span><span class=\"data\">" + tmpOffer.dtEnd.formatCustom("#dd# #MM# #yy#", _currLang, "--/--/----") + "</span>");
                itemPlaceHolder += _ItemTemplateSingle;
            }
            string LayoutTemplate = ltrLayoutTemplate.Text;
            LayoutTemplate = LayoutTemplate.Replace("#orderBy#", currSearch.orderBy);
            LayoutTemplate = LayoutTemplate.Replace("#orderHow#", currSearch.orderHow);
            LayoutTemplate = LayoutTemplate.Replace("#listType#", currSearch.listType);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_dtStart#", orderBy_dtStart);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_dtEnd#", orderBy_dtEnd);
            LayoutTemplate = LayoutTemplate.Replace("#orderBy_price#", orderBy_price);
            LayoutTemplate = LayoutTemplate.Replace("#currPage#", currSearch.currPage.ToString());
            LayoutTemplate = LayoutTemplate.Replace("#itemPlaceHolder#", itemPlaceHolder);
            LayoutTemplate = LayoutTemplate.Replace("#pagerPlaceHolder#", _pager);
            LayoutTemplate = LayoutTemplate.Replace("#lblOrderBy#", CurrentSource.getSysLangValue("lblOrderBy", _currLang, "#lblOrderBy#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblPrice#", CurrentSource.getSysLangValue("lblPrice", _currLang, "#lblPrice#"));
            LayoutTemplate = LayoutTemplate.Replace("#lbldtStart#","Start date");
            LayoutTemplate = LayoutTemplate.Replace("#lbldtEnd#","End date");
            //LayoutTemplate = LayoutTemplate.Replace("#lbldtStart#", CurrentSource.getSysLangValue("lbldtStart", _currLang, "#lbldtStart#"));
            //LayoutTemplate = LayoutTemplate.Replace("#lbldtEnd#", CurrentSource.getSysLangValue("lbldtEnd", _currLang, "#lbldtEnd#"));
            LayoutTemplate = LayoutTemplate.Replace("#lblViewDetails#", CurrentSource.getSysLangValue("lblViewDetails", _currLang, "#lblViewDetails#"));

            return LayoutTemplate;
        }
    }
}
