using ModRental;
using Newtonsoft.Json;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.webserviceNew
{
    public partial class rntEstateSearchZone : System.Web.UI.Page
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
            public FltRangeItem voteRange { get; set; }

            public string orderBy { get; set; }
            public string orderHow { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
            public int totalCount { get; set; }
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
                voteRange = new FltRangeItem();
                orderBy = "";
                orderHow = "";
                page = 0;
                pageSize = 0;
                totalCount = 0;
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

            public string title { get; set; }
            public string summary { get; set; }
            public string pagePath { get; set; }
            public List<string> extrasList { get; set; }
            public string extrasListFormatted { get; set; }
            public List<string> imgList { get; set; }

            public int num_persons_max { get; set; }
            public int num_rooms_bed { get; set; }
            public int num_rooms_bath { get; set; }
            public int importance_vote { get; set; }
            public bool isBP { get; set; }


            public CurrListClass(long Id)
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
                title = "";
                summary = "";
                pagePath = "";
                extrasList = new List<string>();
                extrasListFormatted = "";
                imgList = new List<string>();

                num_persons_max = 0;
                num_rooms_bed = 0;
                num_rooms_bath = 0;
                importance_vote = 0;
                isBP = false;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private string CURRENT_SESSION_ID;
        private int _currLang;
        private long agentID;
        private bool affiliatesarea;
        private string _action;
        private clSearch currSearch;
        private int _currZone;
        private List<rntExts.RNT_estatePriceDetails> _priceDetails;
        private int totResults;
        private bool foradmin;
        private bool formail;

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
                foradmin = Request["foradmin"] == "true";
                formail = Request["formail"] == "true";
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                currSearch = _config.lastZoneSearch;

                _currLang = Request["lang"].objToInt32() == 0 ? CurrentLang.ID : Request["lang"].objToInt32();
                _action = Request["action"] + "";
                agentID = Request["agentID"].objToInt64();

                affiliatesarea = Request["affiliatesarea"] == "true";
                currSearch.currPage = Request["currPage"].objToInt32();
                currSearch.numPerPage = Request["numPerPage"].objToInt32() <= 0 ? 10 : Request["numPerPage"].objToInt32();
                currSearch.searchTitle = (Request["title"] + "").Trim().ToLower().urlDecode();
                currSearch.currCity = Request["currCity"].ToInt32();
                _currZone = Request["currZone"].objToInt32();

                List<string> prRange = Request["prRange"].splitStringToList("|");
                currSearch.prMin = prRange.Count > 0 ? prRange[0].ToInt32() : 0;
                currSearch.prMax = prRange.Count > 1 ? prRange[1].ToInt32() : 9999999;

                List<string> voteRange = Request["voteRange"].splitStringToList("|");
                currSearch.voteMinNew = voteRange.Count > 0 ? voteRange[0].ToInt32() : 0;
                currSearch.voteMaxNew = voteRange.Count > 1 ? voteRange[1].ToInt32() : 10;

                //currSearch.currZoneList = Request["currZoneList"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).ToList();
                currSearch.currZoneList.Add(_currZone);
                currSearch.currConfigList = Request["currConfigList"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).ToList();
                currSearch.orderBy = Request["orderBy"] + "";
                currSearch.orderHow = Request["orderHow"] + "";
                _isWL = Request["isWL"].objToInt32();

                _config.lastZoneSearch = currSearch;
                _config.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(_config);
                getEstateList();
            }
        }

        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList
        {
            get
            {
                //if (AppSettings.RELOAD_CACHE || HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone] == null)
                //{
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
                estateList = estateList.Where(x => x.page_path != "").ToList();
                HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone] = estateList;
                //}
                return (List<AppSettings.RNT_estate>)HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone];
            }
            set { HttpContext.Current.Session["CURRENT_RNT_estateList_" + _currZone] = value; }
        }

        private void getEstateList()
        {
            var currResp = new CurrResponseClass();
            var currList = new List<CurrListClass>();

            string orderBy_title = "";
            string orderBy_vote = "";
            string orderBy_price = "";
            int hf_voteMin = 0;
            int hf_voteMax = 0;
            var estateList = CURRENT_RNT_estateList;
            // agenzie vedono solo apt prenotabili (con prezzo)
            if (affiliatesarea)
                estateList = estateList.Where(x => x.price > 0).ToList();

            if (!string.IsNullOrEmpty(currSearch.searchTitle))
                estateList = estateList.Where(x => !string.IsNullOrEmpty(x.title) && x.title.ToLower().Contains(currSearch.searchTitle)).ToList();

            if (estateList.Count == 0)
            {
                EndRequest(currResp);
                return;
            }

            if (estateList.Count != 0)
            {
                // hf_vote
                hf_voteMin = estateList.Min(x => x.importance_vote.objToInt32());
                hf_voteMax = estateList.Max(x => x.importance_vote.objToInt32());
                currResp.voteRange.min = hf_voteMin;
                currResp.voteRange.max = hf_voteMax;
            }

            List<AppSettings.RNT_estate> _listWithPrice = estateList.Where(x => x.price != 0).OrderByDescending(x => x.is_online_booking).ThenBy(x => x.sequence).ThenBy(x => x.price).ToList();
            List<AppSettings.RNT_estate> _listOnRequest = estateList.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();

            if (_listWithPrice.Count != 0)
            {
                // hf_pr
                currResp.priceRange.min = _listWithPrice.Min(x => x.price.objToInt32());
                currResp.priceRange.max = _listWithPrice.Max(x => x.price.objToInt32());
            }
            // filtra per prezzo
            if (currSearch.prMin > 0 && _listWithPrice.Count > 0)
            {
                if (_listWithPrice.Where(x => x.price >= currSearch.prMin).Count() > 0)
                    _listWithPrice = _listWithPrice.Where(x => x.price >= currSearch.prMin).ToList();
                else
                    currSearch.prMin = 0;
            }
            if (currSearch.prMax > 0 && _listWithPrice.Count > 0)
            {
                if (_listWithPrice.Where(x => x.price <= currSearch.prMax).Count() > 0)
                    _listWithPrice = _listWithPrice.Where(x => x.price <= currSearch.prMax).ToList();
                else
                    currSearch.prMax = 0;
            }
            currResp.priceRange.start = currSearch.prMin;
            currResp.priceRange.end = currSearch.prMax;

            if (currSearch.prMin != 0)
                _listWithPrice = _listWithPrice.Where(x => x.price >= currSearch.prMin).ToList();
            if (currSearch.prMax != 0)
                _listWithPrice = _listWithPrice.Where(x => x.price <= currSearch.prMax).ToList();

            // ordina per prezzo
            if (currSearch.orderBy == "price" && currSearch.orderHow == "asc")
            { _listWithPrice = _listWithPrice.OrderBy(x => x.price).ToList(); orderBy_price = "asc"; }
            if (currSearch.orderBy == "price" && currSearch.orderHow == "desc")
            { _listWithPrice = _listWithPrice.OrderByDescending(x => x.price).ToList(); orderBy_price = "desc"; }

            // ordina per name
            if (currSearch.orderBy == "name" && currSearch.orderHow == "asc")
            { _listWithPrice = _listWithPrice.OrderBy(x => x.title).ToList(); orderBy_price = "asc"; }
            if (currSearch.orderBy == "name" && currSearch.orderHow == "desc")
            { _listWithPrice = _listWithPrice.OrderByDescending(x => x.title).ToList(); orderBy_price = "desc"; }


            estateList = new List<AppSettings.RNT_estate>();
            estateList.AddRange(_listWithPrice);
            estateList.AddRange(_listOnRequest);

            if (estateList.Count == 0)
            {
                EndRequest(currResp);
                return;
            }

            // filtri
            if (currSearch.voteMinNew != 0)
                estateList = estateList.Where(x => x.importance_vote >= currSearch.voteMinNew).ToList();
            if (currSearch.voteMaxNew != 0)
                estateList = estateList.Where(x => x.importance_vote <= currSearch.voteMaxNew).ToList();

            var fltEstateList = estateList.ToList();
            var fltEstateIdsList = fltEstateList.Select(x => x.id).ToList();
            var zoneIds = fltEstateList.Select(x => x.pid_zone.objToInt32()).Distinct().ToList();
            var zoneList = AppSettings.LOC_ZONEs.Where(x => zoneIds.Contains(x.id) && x.pid_lang == App.LangID).ToList();
            foreach (var zone in zoneList)
                currResp.fltZoneList.Add(new FltListItem(zone.title, zone.id + "", currSearch.currZoneList.Contains(zone.id)));
            if (currResp.fltZoneList.Where(x => x.selected).Count() > 0)
            {
                currSearch.currZoneList = currResp.fltZoneList.Where(x => x.selected).Select(x => x.value.objToInt32()).ToList();
                estateList = estateList.Where(x => currSearch.currZoneList.Contains(x.pid_zone.objToInt32())).ToList();
            }

            fltEstateIdsList = estateList.Select(x => x.id).ToList();
            var extrasIds = AppSettings.RNT_RL_ESTATE_CONFIG.Where(x => fltEstateIdsList.Contains(x.pid_estate)).Select(x => x.pid_config).ToList();
            var extrasList = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.Where(x => extrasIds.Contains(x.id) && x.pid_lang == App.LangID && AppSettings._RNT_CUSTOM_CONFIGs.Contains(x.id)).OrderBy(x => x.title).ToList();
            foreach (var extras in extrasList)
                currResp.fltExtrasList.Add(new FltListItem(extras.title, extras.id + "", currSearch.currConfigList.Contains(extras.id)));
            if (currResp.fltExtrasList.Where(x => x.selected).Count() > 0)
            {
                currSearch.currConfigList = currResp.fltExtrasList.Where(x => x.selected).Select(x => x.value.ToInt32()).ToList();
                foreach (int conf in currSearch.currConfigList)
                {
                    estateList = estateList.Where(x => x.configList.Contains(conf)).ToList();
                }
            }

            //if (currSearch.currCity > 0)
            //    estateList = estateList.Where(x => x.pid_city == currSearch.currCity).ToList();
            //if (!currSearch.currZoneList.Contains(0) && currSearch.currZoneList.Count > 0)
            //    estateList = estateList.Where(x => currSearch.currZoneList.Contains(x.pid_zone)).ToList();

            this.totResults = estateList.Count;

            // ordina per voto
            if (currSearch.orderBy == "vote" && currSearch.orderHow == "asc")
            { estateList = estateList.OrderBy(x => x.importance_vote).ToList(); orderBy_vote = "asc"; }
            if (currSearch.orderBy == "vote" && currSearch.orderHow == "desc")
            { estateList = estateList.OrderByDescending(x => x.importance_vote).ToList(); orderBy_vote = "desc"; }
            // ordina per titolo
            if (currSearch.orderBy == "title" && currSearch.orderHow == "asc")
            { estateList = estateList.OrderBy(x => x.title).ToList(); orderBy_title = "asc"; }
            if (currSearch.orderBy == "title" && currSearch.orderHow == "desc")
            { estateList = estateList.OrderByDescending(x => x.title).ToList(); orderBy_title = "desc"; }

            // set defaults, per aggiustare slider            
            if (hf_voteMin > currSearch.voteMinNew && currSearch.voteMinNew != 0) currSearch.voteMinNew = hf_voteMin;
            if (hf_voteMax < currSearch.voteMaxNew && currSearch.voteMaxNew != 0) currSearch.voteMaxNew = hf_voteMax;

            currResp.voteRange.start = currSearch.voteMinNew;
            currResp.voteRange.end = currSearch.voteMaxNew;

            int allCount = estateList.Count;
            int currCount = allCount;
            if (currCount == 0)
            {
                EndRequest(currResp);
                return;
            }

            // show all Properties for Zones - Change  30 may 2016

            //if (currSearch.currPage != 0)
            //{
            //    int _pageCount = 1;
            //    int _currPageCount = currSearch.numPerPage;
            //    while (currCount > _currPageCount)
            //    {
            //        _pageCount++;
            //        currCount -= currSearch.numPerPage;
            //    }
            //    if (_pageCount < currSearch.currPage) currSearch.currPage = _pageCount;
            //    if (_pageCount == currSearch.currPage && currCount < _currPageCount) _currPageCount = currCount;
            //    estateList = estateList.GetRange((currSearch.currPage - 1) * currSearch.numPerPage, _currPageCount);
            //}
            var configList = maga_DataContext.DC_RENTAL.RNT_TB_CONFIGs.Where(x => AppSettings._RNT_CUSTOM_CONFIGs.Contains(x.id)).ToList();
            var mediaList = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == "gallery").ToList();
            foreach (AppSettings.RNT_estate tmpTb in estateList)
            {
                var tmpResp = new CurrListClass(tmpTb.id);
                tmpResp.price = tmpTb.price;
                tmpResp.priceFormatted = tmpTb.price > 0 ? "€" + tmpTb.price.ToString("N2") : CurrentSource.getSysLangValue("lblOnRequest");
                tmpResp.nicePrice = tmpTb.nicePrice;
                tmpResp.pr_agentCommissionPrice = tmpTb.pr_agentCommissionPrice;
                tmpResp.priceError = tmpTb.priceError;
                tmpResp.locZoneFull = tmpTb.zone;
                tmpResp.extraInfo = "";
                tmpResp.imgPreview = "/" + tmpTb.img_preview_1;
                tmpResp.imgPreview2 = "/" + tmpTb.img_preview_2;
                tmpResp.imgPreview3 = "/" + tmpTb.img_preview_3;

                tmpResp.title = tmpTb.title;
                tmpResp.summary = tmpTb.summary;
                tmpResp.pagePath = "/" + tmpTb.page_path;
                tmpResp.imgList = mediaList.Where(x => x.pid_estate == tmpTb.id).OrderBy(x=>x.sequence).Select(x => x.img_thumb).ToList();

                tmpResp.num_persons_max = tmpTb.num_persons_max;
                tmpResp.num_rooms_bed = tmpTb.num_rooms_bed;
                tmpResp.num_rooms_bath = tmpTb.num_rooms_bath;
                tmpResp.importance_vote = tmpTb.importance_vote;
                tmpResp.isBP = tmpTb.isBP;
                tmpResp.gmGmapCoords = "" + tmpTb.google_maps;
                currList.Add(tmpResp);

            }
            currResp.list = currList;
            currResp.totalCount = allCount;
            //   currResp.pages = new listPages(allCount, currSearch.numPerPage, currSearch.currPage, 10, "<li><span class=\"currPage active\">" + contUtils.getLabel("listAllPages") + "</span></li>", "<li><a class=\"listPageLink\" href=\"javascript:AjaxList_changePage(0)\">" + contUtils.getLabel("listAllPages") + "</a></li>", "<li><span class=\"currPage\">{0}</span></li>", "<li><a class=\"listPageLink\" href=\"javascript:AjaxList_changePage({0})\">{0}</a></li>", "<li><a class=\"listPageLink\" href=\"javascript:AjaxList_changePage({0})\">..</a></li>", "<li><a class=\"listPageLink\" href=\"javascript:AjaxList_changePage({0})\">..</a></li>");
            //currResp.page = currSearch.currPage;
            //currResp.pageSize = currResp.pages.pagesCount;
            currResp.orderBy = currSearch.orderBy;
            currResp.orderHow = currSearch.orderHow;

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
    }
}