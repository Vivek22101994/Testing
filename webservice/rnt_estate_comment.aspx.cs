using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;
using System.IO;
using Newtonsoft.Json;

namespace RentalInRome.webservice
{
    public partial class rnt_estate_comment : System.Web.UI.Page
    {
        protected class CurrResponseClass
        {
            public List<CurrListClass> list { get; set; }
            public listPages pages { get; set; }
            public string orderBy { get; set; }
            public string orderHow { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
            public CurrResponseClass()
            {
                list = new List<CurrListClass>();
                pages = null;
                orderBy = "";
                orderHow = "";
                page = 0;
                pageSize = 0;
            }
        }
        protected class CurrListClass
        {
            public long id { get; set; }
            public string nameFull { get; set; }
            public string commentBody { get; set; }
            public DateTime dtComment { get; set; }
            public string dtCommentFormatted { get; set; }
            public string imgAvatar { get; set; }
            public string country { get; set; }
            public int vote { get; set; }

            public CurrListClass(long Id, DateTime DtComment, int PidLang)
            {
                id = Id;
                nameFull = "";
                commentBody = "";
                dtComment = DtComment;
                dtCommentFormatted = dtComment.formatCustom("#dd# #MM# #yy#", PidLang, "--/--/----");
            }
        }
        protected void EndRequest(CurrResponseClass resp)
        {
            TextWriter wr = new StringWriter();
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(wr, resp);
            Response.Write(wr.ToString());
            Response.End();
        }
        private List<dbRntEstateCommentsTBL> _commentList;
        private DateTime _dtStart;
        private DateTime _dtEnd;
        private int _currLang;
        private bool _fullView;
        private int _currEstate;
        private int _currPage;
        private int _numPerPage;
        private string _voteRange;
        private bool IsHomePage = false;
        private bool _isNew = false;
        private bool _isDisplayPagination = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                _currEstate = Request.QueryString["currEstate"].objToInt32();
                _currPage = Request.QueryString["currPage"].objToInt32();
                _currPage = (_currPage != 0) ? _currPage : 1;
                _numPerPage = Request.QueryString["numPerPage"].objToInt32();
                _numPerPage = (_numPerPage != 0) ? _numPerPage : 10;
                _voteRange = Request.QueryString["voteRange"];
                _fullView = Request.QueryString["fullView"].objToInt32() == 1;
                IsHomePage = Request.QueryString["inhomepage"].objToInt32() == 1;
                _isNew = Request.QueryString["isNew"].objToInt32() == 1;
                _isDisplayPagination = Request.QueryString["displayPagination"].objToInt32() == 1;
                if (Request["json"] == "true")
                    getCommentListJson();
                else
                    Response.Write(getCommentList());
            }
        }
        protected void getCommentListJson()
        {
            using (DCmodRental dc = new DCmodRental())
                _commentList = dc.dbRntEstateCommentsTBLs.Where(x => x.isActive == 1 && x.dtComment.HasValue).OrderByDescending(x => x.dtComment).ToList();
            if (_currEstate != 0)
                _commentList = _commentList.Where(x => x.pidEstate == _currEstate).ToList();
            int allCount = _commentList.Count;
            int currCount = allCount;
            int _pageCount = 1;
            int _currPageCount = _numPerPage;
            while (currCount > _currPageCount)
            {
                _pageCount++;
                currCount -= _numPerPage;
            }
            if (_pageCount < _currPage) _currPage = _pageCount;
            if (_pageCount == _currPage && currCount < _currPageCount) _currPageCount = currCount;
            _commentList = _commentList.GetRange((_currPage - 1) * _numPerPage, _currPageCount);
            var currResp = new CurrResponseClass();
            var currList = new List<CurrListClass>();
            foreach (dbRntEstateCommentsTBL tmpTb in _commentList)
            {
                var tmpResp = new CurrListClass(tmpTb.id, tmpTb.dtComment.Value, _currLang);
                if (_isNew)
                {
                    tmpResp.imgAvatar = "/images/css/user-" + tmpTb.pers + ".gif";
                    tmpResp.nameFull = tmpTb.cl_name_full;
                    tmpResp.country = tmpTb.cl_country;
                    tmpResp.vote = tmpTb.vote.objToInt32();
                    tmpResp.commentBody = (tmpTb.body + "").htmlNoWrap();
                }
                else
                {

                    tmpResp.nameFull = tmpTb.cl_name_full + " " + (tmpTb.cl_country != "" ? "(" + tmpTb.cl_country + ")" : "");
                    tmpResp.commentBody = tmpTb.body.htmlNoWrap();
                }
                currList.Add(tmpResp);
            }
            currResp.list = currList;
            currResp.pages = new listPages(allCount, _numPerPage, _currPage, 10, "<span class=\"currPage\">" + contUtils.getLabel("listAllPages") + "</span>", "<a class=\"listPageLink\" href=\"javascript:AjaxList_changePage(0)\">" + contUtils.getLabel("listAllPages") + "</a>", "<span class=\"currPage\">{0}</span>", "<a class=\"listPageLink\" href=\"javascript:AjaxList_changePage({0})\">{0}</a>", "<a class=\"listPageLink\" href=\"javascript:AjaxList_changePage({0})\">..</a>", "<a class=\"listPageLink\" href=\"javascript:AjaxList_changePage({0})\">..</a>");
            currResp.page = _currPage;
            currResp.pageSize = currResp.pages.pagesCount;
            EndRequest(currResp);
        }
        protected string getCommentList()
        {
            _commentList = AppSettings.RNT_TBL_ESTATE_COMMENTs.Where(x => x.isActive == 1).ToList();
            if (IsHomePage)
                _commentList = _commentList.Where(x => x.isVisibleHomePage == 1).ToList();

            if (_currEstate != 0)
                _commentList = _commentList.Where(x => x.pidEstate == _currEstate).ToList();
            int _allCount = _commentList.Count;
            int _currCount = _allCount;
            if (_currCount == 0 && _currEstate == 0)
            {
                return "<div class=\"noComments\"><span>La ricerca non apportato risultati...<br /> <strong>Vi preghiamo di riprova con i parametri diversi.</strong> </span></div>";
            }
            if (_currCount == 0 && _currEstate != 0)
            {
                if (_isNew)
                    return "<h3 style='font-size:16px;padding-left:15%;'>" + CurrentSource.getSysLangValue("voteAptHasNoComments") + "</h3>";
                else
                    return "" + CurrentSource.getSysLangValue("voteAptHasNoComments");
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
            _commentList = _commentList.GetRange((_currPage - 1) * _numPerPage, _currPageCount);

            string _pager = "";
            if (_isNew)
            {
                _pager = "<div class=\"pagination\"><ul>" + CommonUtilities.getPager(_allCount, _numPerPage, _currPage, 10, "<li class=\"active\"><span class=\"currPage active\">" + contUtils.getLabel("listAllPages") + "</span></li>", "", "<li class=\"active\"><span class=\"currPage\">{0}</span></li>", "<li><a class=\"listPageLink\" href=\"javascript:RNT_estComment_changePage({0})\">{0}</a></li>", "<li><a class=\"listPageLink\" href=\"javascript:RNT_estComment_changePage({0})\">..</a></li>", "<li><a class=\"listPageLink\" href=\"javascript:RNT_estComment_changePage({0})\">..</a></li>") + "</ul></div>";

            }
            else
            {
                _pager = "<div class=\"paginazione\">" + CommonUtilities.getPager(_allCount, _numPerPage, _currPage, 10, "", "", "<span class=\"pagCorr\">{0}</span>", "<a class=\"pag\" href=\"javascript:RNT_estComment_changePage({0})\">{0}</a>", "<a class=\"pag\" href=\"javascript:RNT_estComment_changePage({0})\">..</a>", "<a class=\"pag\" href=\"javascript:RNT_estComment_changePage({0})\">..</a>") + "</div>";
            }
            string _response = IsHomePage ? "" : "<input type=\"hidden\" id=\"hf_estComment_currPage\" value=\"" + _currPage + "\"/>";

            foreach (var _comment in _commentList)
            {
                RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _comment.pidEstate && x.pid_lang == _currLang);
                if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _comment.pidEstate && x.pid_lang == 2);

                if (_fullView)
                {
                    _response += "<div class=\"commento list\">";
                    _response += "    <div class=\"commCont\">";
                    //_response += "        <img style=\"border: 1px solid rgb(204, 204, 204); float: left; margin-right: 10px;\" src=\"images/css/user-f.gif\" alt=\"\" />";
                    _response += "        <div style=\"float: left;\">";
                    _response += "          <span class=\"userName\">" + _comment.cl_name_full + " " + (_comment.cl_country != "" ? "(" + _comment.cl_country + ")" : "") + "</span>";
                    //_response += "          <span class=\"userName\">" + _comment.cl_name_full + " " + (_comment.cl_country != "" ? "(" + _comment.cl_country + ")" : "") + ", <em>" + _comment.dtComment.formatCustom("#dd# #MM# #yy#", _currLang, "") + "</em></span>";
                    _response += "          <div class=\"nulla\"></div>";
                    if (_lang != null && _lang.page_path != null && _lang.page_path.Trim() != "")
                        _response += "          <span class=\"aptCommName\">" + CurrentSource.getSysLangValue("voteRelatedToApt") + "&nbsp;<a href=\"" + _lang.page_path + "\">" + _lang.title + "</a></span>";
                    else if (_lang != null)
                        _response += "          <span class=\"aptCommName\">" + CurrentSource.getSysLangValue("voteRelatedToApt") + "&nbsp;" + _lang.title + "</span>";
                    _response += "          <div class=\"nulla\"></div>";
                    _response += "          <span class=\"votazione\"> <img src=\"/images/estate_vote/vote" + _comment.vote.objToInt32() + ".gif\" alt=\"vote" + _comment.vote + "\" /></span>";
                    _response += "        </div>";
                    _response += "        <div class=\"nulla\"></div>";
                    _response += "        <div class=\"commentoTxt\">" + _comment.body + "</div>";
                    _response += "    </div>";
                    _response += "</div>";
                }
                else
                {
                    if (IsHomePage)
                    {
                        _response += "<div class=\"item\">";
                        _response += "<blockquote class=\"text\"><p>" + _comment.body + " </p> </blockquote>";
                        _response += "<div class=\"col-md-5 center\">";
                        _response += " <div class=\"author\">";
                        _response += "<img src=\"/images/user-m.gif\"  />";
                        _response += "<div>";
                        _response += _comment.cl_name_full + "<br/>";
                        _response += "<span> " + _comment.cl_country + "</span>";
                        _response += " </div>";
                        _response += "</div>";
                        _response += "</div>";
                        _response += "</div>";
                    }
                    else
                    {
                        if (_isNew)
                        {
                            _response += "<li>";
                            _response += "<img src=\"/images/css/user-" + _comment.pers + ".gif\" alt=''>";
                            _response += " <div class=\"comment\">";
                            _response += "<h3>" + _comment.cl_name_full + "<small>" + _comment.dtComment.formatCustom("#dd# #MM# #yy#", _currLang, "--/--/----");
                            if (_currEstate == 0 && _lang != null && _lang.page_path != null && _lang.page_path.Trim() != "" && _lang.title != null && _lang.title.Trim() != "")
                                _response += " - <a target=\"_blank\" href=\"/" + _lang.page_path + "\">" + _lang.title + "</a>";
                            _response += "</small></h3>";
                            _response += "<p>" + _comment.body + "</p>";
                            _response += "<img src=\"/images/estate_vote/vote" + _comment.vote.objToInt32() + ".gif\" alt=\"vote" + _comment.vote + "\" class=\"ratingCommentDet\"/>";
                            _response += "</div>";
                            _response += "</li>";
                        }
                        else
                        {
                            _response += "<div class=\"commento list\">";
                            _response += "    <div class=\"commCont\">";
                            //_response += "        <img style=\"border: 1px solid rgb(204, 204, 204); float: left; margin-right: 10px;\" src=\"images/css/user-f.gif\" alt=\"\" />";
                            _response += "        <div style=\"float: left;\">";
                            _response += "          <span class=\"userName\">" + _comment.cl_name_full + " " + (_comment.cl_country != "" ? "(" + _comment.cl_country + ")" : "") + "</span>";
                            //_response += "          <span class=\"userName\">" + _comment.cl_name_full + " " + (_comment.cl_country != "" ? "(" + _comment.cl_country + ")" : "") + ", <em>" + _comment.dtComment.formatCustom("#dd# #MM# #yy#", _currLang, "") + "</em></span>";
                            _response += "          <div class=\"nulla\"></div>";
                            _response += "          <span class=\"votazione\"> <img src=\"/images/estate_vote/vote" + _comment.vote.objToInt32() + ".gif\" alt=\"vote" + _comment.vote + "\" /></span>";
                            _response += "        </div>";
                            _response += "        <div class=\"nulla\"></div>";
                            _response += "        <div class=\"commentoTxt\">" + _comment.body + "</div>";
                            _response += "    </div>";
                            _response += "</div>";
                        }
                    }
                }
            }
            if ((_fullView && !IsHomePage) || _isDisplayPagination)
            {
                _response += "<div class=\"nulla\"></div>";
                _response += _pager;
            }
            return _response;
        }
    }
}
