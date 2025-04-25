using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using RNT_estateDate = AppSettings.RNT_estateDate;
using RNT_dateItem = AppSettings.RNT_dateItem;

namespace RentalInRome.admin.webservice
{
    public partial class rnt_estateReservation : System.Web.UI.Page
    {
        public class estateReservation_tmp
        {
            public List<AppSettings.RNT_estate> _estateList { get; set; }
            public string id { get; set; }
            public DateTime dtCreation { get; set; }
            public DateTime dtStart { get; set; }
            public DateTime dtEnd { get; set; }
            public estateReservation_tmp(string _id, DateTime _dtStart, DateTime _dtEnd)
            {
                _estateList = new List<AppSettings.RNT_estate>();
                id = _id;
                dtCreation = DateTime.Now;
                dtStart = _dtStart;
                dtEnd = _dtEnd;
            }
            public estateReservation_tmp Clone()
            {
                estateReservation_tmp _tmp = new estateReservation_tmp(id, dtStart, dtEnd);
                _tmp._estateList = _estateList;
                _tmp.dtCreation = dtCreation;
                return _tmp;
            }
        }
        static List<estateReservation_tmp> _CURRENT_LISTs;
        static List<estateReservation_tmp> CURRENT_LISTs
        {
            get { if (_CURRENT_LISTs == null) { _CURRENT_LISTs = new List<estateReservation_tmp>(); } return _CURRENT_LISTs.Select(x => x.Clone()).ToList(); }
            set { _CURRENT_LISTs = value; }
        }
        private string CURRENT_ID;
        private int _currCity;
        private List<int> _currZone;
        private List<int> _currEstate;
        private List<int> _currConfig;
        private int _state;
        private int _minPrice;
        private int _maxPrice;
        private int _minPers;
        private int _minRooms;
        private int _importance_stars;
        private int _is_exclusive;
        private int _is_online_booking;
        //private int _maxPrice;
        private string _searchTitle;
        private string _voteRange;
        protected List<DateTime> _dates;
        protected string resList_tabDays;
        protected string resList_estate;
        protected string resList_script;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/html";
                DateTime _dtStart;
                DateTime _dtEnd;
                CURRENT_ID = Request.QueryString["CURRENT_ID"];
                int _dtStartInt = Request.QueryString["dtS"].objToInt32();
                int _dtEndInt = Request.QueryString["dtE"].objToInt32();
                if (_dtStartInt != 0 && _dtEndInt != 0)
                {
                    _dtStart = _dtStartInt.JSCal_intToDate();
                    _dtEnd = _dtEndInt.JSCal_intToDate();
                }
                else
                {
                    Response.Write("<div class=\"listEmpty error\"><span>Errore nel sistema, contattare assistenza!</span></div>");
                    Response.End();
                    return;
                }
                DateTime _dt = _dtStart;
                _dates = new List<DateTime>();
                while (_dt <= _dtEnd)
                {
                    _dates.Add(_dt);
                    _dt = _dt.AddDays(1);
                }
                _searchTitle = (Request.QueryString["title"] + "").Trim().ToLower().urlDecode();
                _state = Request.QueryString["state"].objToInt32();
                _minPrice = Request.QueryString["minPrice"].objToInt32();
                _maxPrice = Request.QueryString["maxPrice"].objToInt32();
                _minPers = Request.QueryString["minPers"].objToInt32();
                _minRooms = Request.QueryString["minRooms"].objToInt32();
                _importance_stars = Request.QueryString["importance_stars"].objToInt32();
                _is_exclusive = Request.QueryString["is_exclusive"].objToInt32();
                _is_online_booking = Request.QueryString["is_online_booking"].objToInt32();
                _voteRange = Request.QueryString["voteRange"].urlDecode();
                _currCity = Request.QueryString["currCity"].ToInt32();
                _currZone = Request.QueryString["currZone"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).ToList();
                _currEstate = Request.QueryString["currEstate"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).ToList();
                _currConfig = Request.QueryString["currConfig"].urlDecode().splitStringToList("|").Select(x => x.ToInt32()).ToList();

                resList_tabDays = RNT_estateList_fillDays();
                resList_estate = fillList(CURRENT_ID, _dtStart, _dtEnd);
                string _response = "";
                _response += "<tr>";
                _response += "  <td class=\"nomiTab\">";
                _response += "  </td>";
                _response += "  <td id=\"resList_tabDays\">";
                _response += "  " + resList_tabDays;
                _response += "  </td>";
                _response += "</tr>";
                _response += "" + resList_estate;
                Response.Write(_response);
            }
        }
        protected string getStateClass(int? id)
        {
            RNT_LK_RESERVATION_STATE _state = AppSettings.RNT_LK_RESERVATION_STATEs.SingleOrDefault(x => x.id == id);
            if (_state != null)
                return _state.css_class;
            return "res_xxx";
        }
        protected string RNT_estateList_fillDays()
        {
            var _tbl = "<table cellspacing=\"0\" cellpadding=\"1\" class=\"tabDays\">";
            _tbl += "<tr>";

            foreach (DateTime _dt in _dates)
            {
                _tbl += "<td class=\"dateTab\">";
                _tbl += "<span>" + _dt.getDayOfWeekITA(true) + "&nbsp;<hr/>" + _dt.Day + "&nbsp;" + _dt.getMonthITA(true) + "</span>";
                _tbl += "</td>";
            }
            _tbl += "</tr>";
            _tbl += "</table>";
            return _tbl;
        }
        protected estateReservation_tmp getList(string _id, DateTime _dtStart, DateTime _dtEnd)
        {
            List<estateReservation_tmp> _list = CURRENT_LISTs;
            _list.RemoveAll(x => x.dtCreation < DateTime.Now.AddHours(-1));
            CURRENT_LISTs = _list;
            // se esiste l'elenco caricato gia, tira fuori
            estateReservation_tmp _resList = CURRENT_LISTs.FirstOrDefault(x => x.id == _id && x.dtStart == _dtStart && x.dtEnd == _dtEnd);
            if (_resList != null)
                return _resList;
            // se no carica tutto
            _resList = new estateReservation_tmp(_id, _dtStart, _dtEnd);
            int _nights = (_dtEnd - _dtStart).Days;
            List<AppSettings.RNT_estate> _estateList = AppSettings.RNT_estateListAll.Where(x => x.nights_min <= _nights).OrderBy(x => x.code).ToList().Select(x => x.Clone()).ToList();
            foreach (AppSettings.RNT_estate _est in _estateList)
            {
                foreach (DateTime _dt in _dates)
                {
                    RNT_estateDate _dtEst = new RNT_estateDate(_est.id, _dt);
                    _est.Dates.Add(_dtEst);
                }
            }
            _resList._estateList = _estateList;
            _list = CURRENT_LISTs;
            _list.RemoveAll(x => x.id == _id && x.dtStart == _dtStart && x.dtEnd == _dtEnd);
            _list.Add(_resList);
            CURRENT_LISTs = _list;

            return _resList;
        }
        protected string fillList(string _id, DateTime _dtStart, DateTime _dtEnd)
        {
            estateReservation_tmp _resList = getList(_id, _dtStart, _dtEnd);
            List<AppSettings.RNT_estate> _estateList = _resList._estateList;

            // filtri
            if (!string.IsNullOrEmpty(_searchTitle))
                _estateList = _estateList.Where(x => !string.IsNullOrEmpty(x.title) && x.title.ToLower().Contains(_searchTitle)).ToList();
            if (_currCity!=0)
                _estateList = _estateList.Where(x => x.pid_city == _currCity).ToList();
            if (!_currZone.Contains(0) && _currZone.Count > 0)
                _estateList = _estateList.Where(x => _currZone.Contains(x.pid_zone)).ToList();
            if (!_currEstate.Contains(0) && _currEstate.Count > 0)
                _estateList = _estateList.Where(x => _currEstate.Contains(x.id)).ToList();
            if (_minPrice != -1)
                _estateList = _estateList.Where(x => x.price >= _minPrice).ToList();
            if (_maxPrice != -1)
                _estateList = _estateList.Where(x => x.price <= _maxPrice).ToList();
            if (_minPers != -1)
                _estateList = _estateList.Where(x => x.num_persons_max >= _minPers && x.num_persons_min <= _minPers).ToList();
            if (_minRooms != -1)
                _estateList = _estateList.Where(x => x.num_rooms_bed >= _minRooms).ToList();
            if (_importance_stars != -1)
                _estateList = _estateList.Where(x => x.importance_stars >= _importance_stars).ToList();
            if (_is_exclusive != -1)
                _estateList = _estateList.Where(x => x.is_exclusive == _is_exclusive).ToList();
            if (_is_online_booking != -1)
                _estateList = _estateList.Where(x => x.is_online_booking == _is_online_booking).ToList();
            
            if (_voteRange != "" && _voteRange != "0|10")
                _estateList = _estateList.Where(x => x.importance_vote >= _voteRange.Split('|')[0].ToInt32() && x.importance_vote <= _voteRange.Split('|')[1].ToInt32()).ToList();
            foreach (int _conf in _currConfig)
            {
                _estateList = _estateList.Where(x => x.configList.Contains(_conf)).ToList();
            }

            if (_estateList.Count == 0)
            {
                return "<div class=\"listEmpty error\"><span>Nessun Appartamento corrisponde ai parametri di ricerca</span></div>";
            }

            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            {
                List<RNT_TBL_RESERVATION> _resListAll = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 && x.is_deleted != 1 && x.dtStart.HasValue && x.dtEnd.HasValue && (x.dtEnd.Value.Date >= _dtStart.AddDays(-1) && x.dtStart.Value.Date <= _dtEnd.AddDays(+1))).ToList();
                string resList_estate = "";
                List<long> _allResList = new List<long>();
                foreach (AppSettings.RNT_estate _est in _estateList)
                {
                    var scheduleStr = "<table id=\"" + _est.id + "_schedule\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-bottom: 2px; margin-top: 2px;\">";
                    scheduleStr += "<tr>";
                    List<int> _stateList = new List<int>();
                    List<long> _estateResList = new List<long>();
                    for (int i = 0; i < _est.Dates.Count; i++)
                    {
                        RNT_estateDate _dtEst = _est.Dates[i];
                        _dtEst.Part_2.ClassList = new List<string>();
                        _dtEst.Part_0.ClassList = new List<string>();
                        _dtEst.Part_1.ClassList = new List<string>();
                        bool _isFirst = i == 0;
                        bool _isLast = i == _est.Dates.Count - 1;
                        bool _1_isSet = false;
                        bool _2_isSet = false;
                        bool _all_isSet = false;
                        List<RNT_TBL_RESERVATION> _resListTmp = _resListAll.Where(x => x.pid_estate == _est.id && x.dtStart.Value.Date <= _dtEst.Date && x.dtEnd.Value.Date >= _dtEst.Date).ToList();
                        if (_resListTmp.Count == 0)
                        {
                            _dtEst.Part_2.Attr_IdRes = 0;
                            _dtEst.Part_2.ClassList.Add("res_free");

                            _dtEst.Part_0.Attr_IdRes = 0;
                            _dtEst.Part_0.ClassList.Add("res_free");

                            _dtEst.Part_1.Attr_IdRes = 0;
                            _dtEst.Part_1.ClassList.Add("res_free");

                            _1_isSet = true;
                            _2_isSet = true;
                            _all_isSet = true;
                        }
                        if (!_all_isSet)
                        {
                            RNT_TBL_RESERVATION _res = _resListTmp.FirstOrDefault(x => x.dtEnd.Value.Date == _dtEst.Date);
                            if (_res != null)
                            {
                                if (!_isFirst && !_stateList.Contains(_res.state_pid.Value))
                                    _stateList.Add(_res.state_pid.Value);
                                if (!_estateResList.Contains(_res.id))
                                    _estateResList.Add(_res.id);
                                _dtEst.Part_2.Attr_IdRes = _res.id;
                                _dtEst.Part_2.Attr_Title = _res.id.ToString();
                                _dtEst.Part_2.ClassList.Add(getStateClass(_res.state_pid));
                                _dtEst.Part_2.ClassList.Add("_tooltip");

                                _dtEst.Part_0.Attr_IdRes = 0;
                                _dtEst.Part_0.ClassList.Add("free");

                                _2_isSet = true;
                            }
                            _res = _resListTmp.FirstOrDefault(x => x.dtStart.Value.Date == _dtEst.Date);
                            if (_res != null)
                            {
                                if (!_isLast && !_stateList.Contains(_res.state_pid.Value))
                                    _stateList.Add(_res.state_pid.Value);
                                if (!_estateResList.Contains(_res.id))
                                    _estateResList.Add(_res.id);
                                _dtEst.Part_0.Attr_IdRes = 0;
                                _dtEst.Part_0.ClassList.Add("free");

                                _dtEst.Part_1.Attr_IdRes = _res.id;
                                _dtEst.Part_1.Attr_Title = _res.id.ToString();
                                _dtEst.Part_1.ClassList.Add(getStateClass(_res.state_pid));
                                _dtEst.Part_1.ClassList.Add("_tooltip");

                                _1_isSet = true;
                            }
                            _res = _resListTmp.FirstOrDefault(x => x.dtEnd.Value.Date > _dtEst.Date && x.dtStart.Value.Date < _dtEst.Date);
                            if (_res != null)
                            {
                                if (!_stateList.Contains(_res.state_pid.Value))
                                    _stateList.Add(_res.state_pid.Value);
                                if (!_estateResList.Contains(_res.id))
                                    _estateResList.Add(_res.id);
                                _dtEst.Part_2.Attr_IdRes = _res.id;
                                _dtEst.Part_2.Attr_Title = _res.id.ToString();
                                _dtEst.Part_2.ClassList.Add(getStateClass(_res.state_pid));
                                _dtEst.Part_2.ClassList.Add("_tooltip");

                                _dtEst.Part_0.Attr_IdRes = _res.id;
                                _dtEst.Part_0.Attr_Title = _res.id.ToString();
                                _dtEst.Part_0.ClassList.Add(getStateClass(_res.state_pid));
                                _dtEst.Part_0.ClassList.Add("_tooltip");

                                _dtEst.Part_1.Attr_IdRes = _res.id;
                                _dtEst.Part_1.Attr_Title = _res.id.ToString();
                                _dtEst.Part_1.ClassList.Add(getStateClass(_res.state_pid));
                                _dtEst.Part_1.ClassList.Add("_tooltip");

                                _2_isSet = true;
                                _1_isSet = true;
                            }
                            if (!_1_isSet)
                            {
                                _dtEst.Part_1.Attr_IdRes = 0;
                                _dtEst.Part_1.ClassList.Add("res_free");
                            }
                            if (!_2_isSet)
                            {
                                _dtEst.Part_2.Attr_IdRes = 0;
                                _dtEst.Part_2.ClassList.Add("res_free");
                            }
                            if (!_1_isSet || !_2_isSet)
                                _dtEst.Part_0.Attr_IdRes = 0;
                        }
                        // aggiungi al html gg
                        string _classList = "";
                        foreach (string _class in _dtEst.Part_2.ClassList)
                        {
                            _classList += " " + _class;
                        }
                        scheduleStr += "<td id=\"" + _est.id + "_" + _dtEst.Date.JSCal_dateToString() + "_2\" title=\"" + _dtEst.Part_2.Attr_Title + "\" class=\"res_2" + _classList + "\" IdEstate=\"" + _est.id + "\" dtDate=\"" + _dtEst.Date.JSCal_dateToString() + "\" dtPart=\"2\" IdRes=\"" + _dtEst.Part_2.Attr_IdRes + "\" onclick=\"" + _dtEst.Part_2.onClick + "\"></td>";
                        _classList = "";
                        foreach (string _class in _dtEst.Part_0.ClassList)
                        {
                            _classList += " " + _class;
                        }
                        scheduleStr += "<td id=\"" + _est.id + "_" + _dtEst.Date.JSCal_dateToString() + "_0\" title=\"" + _dtEst.Part_0.Attr_Title + "\" class=\"res_0" + _classList + "\" IdEstate=\"" + _est.id + "\" dtDate=\"" + _dtEst.Date.JSCal_dateToString() + "\" dtPart=\"0\" IdRes=\"" + _dtEst.Part_0.Attr_IdRes + "\" onclick=\"" + _dtEst.Part_0.onClick + "\"></td>";
                        _classList = "";
                        foreach (string _class in _dtEst.Part_1.ClassList)
                        {
                            _classList += " " + _class;
                        }
                        scheduleStr += "<td id=\"" + _est.id + "_" + _dtEst.Date.JSCal_dateToString() + "_1\" title=\"" + _dtEst.Part_1.Attr_Title + "\" class=\"res_1" + _classList + "\" IdEstate=\"" + _est.id + "\" dtDate=\"" + _dtEst.Date.JSCal_dateToString() + "\" dtPart=\"1\" IdRes=\"" + _dtEst.Part_1.Attr_IdRes + "\" onclick=\"" + _dtEst.Part_1.onClick + "\"></td>";

                    }
                    scheduleStr += "</tr>";
                    scheduleStr += "</table>";
                    _est.scheduleStr = scheduleStr;

                    // imposta stati disponibili
                    _est.is_nd = _stateList.Contains(1);
                    _est.is_opz = _stateList.Contains(2);
                    _est.is_can = _stateList.Contains(3);
                    _est.is_prt = _stateList.Contains(4);
                    _est.is_mv = _stateList.Contains(5);
                    _est.is_free = !_est.is_nd && !_est.is_opz && !_est.is_can && !_est.is_prt && !_est.is_mv;
                    if (_stateList.Where(x => x != 3).Count() == 0)
                        _stateList.Add(0);
                    if (_state != -1 && !_stateList.Contains(_state))
                        continue;
                    // aagiungi alle pren utilizzate
                    _allResList.AddRange(_estateResList);

                    // aggiungi al html elenco
                    string _tr = "<tr id=\"" + _est.id + "_trCont\" onmouseover=\"$(this).addClass('tr_current')\" onmouseout=\"$(this).removeClass('tr_current')\" class=\"estate_row tr_normal\">";
                    _tr += "<td class=\"nomiTab\">";
                    _tr += "<a target='_blank' href='rnt_estate_details.aspx?id=" + _est.id + "'>";
                    _tr += "" + _est.code;
                    _tr += "</a>";
                    _tr += "</td>";
                    _tr += "<td id=\"" + _est.id + "_scheduleCont\">";
                    _tr += _est.scheduleStr;
                    _tr += "</td></tr>";
                    resList_estate += _tr;

                }
                resList_estate += "<tr><td colspan=\"2\">" + RNT_estateList_fillToolTip(_resListAll.Where(x => _allResList.Contains(x.id)).ToList()) + "</td></tr>";
                return resList_estate;
            }
        }
        protected string RNT_estateList_fillToolTip(List<RNT_TBL_RESERVATION> _resList)
        {
            string _template = "";
            _template += "<strong>ttp_cl_name_full, [Code. ttp_code]</strong>";
            _template += "<br/>";
            _template += "Persone: ttp_persons, Prezzo:&euro; ttp_pr_total";
            _template += "<br/>";
            _template += "Check-in: ttp_dtStart, ore: ttp_dtStartTime";
            _template += "<br/>";
            _template += "Check-out: ttp_dtEnd, ore: ttp_dtEndTime";
            //_template += "<br/>";
            //_template += "Scadenza: ttp_block_expire";
            string _toolTip = "";
            foreach (RNT_TBL_RESERVATION _res in _resList)
            {
                string _tp_cont = "<div style=\"display: none;\" id=\"tooltip_" + _res.id + "\">";
                _tp_cont += _template;
                _tp_cont += "</div>";
                DateTime _block_expire = _res.block_expire.HasValue ? _res.block_expire.Value.Date : _res.dtCreation.Value.AddDays(2);
                _tp_cont = _tp_cont
                    .Replace("ttp_cl_name_full", _res.cl_name_honorific + " " + _res.cl_name_full)
                    .Replace("ttp_code", _res.code)
                    .Replace("ttp_persons", "" + (_res.num_adult + _res.num_child_over))
                    .Replace("ttp_pr_total", ("" + _res.pr_total))
                    .Replace("ttp_dtStartTime", (_res.dtStartTime + "").JSTime_stringToTime().JSTime_toString(false, true))
                    .Replace("ttp_dtEndTime", (_res.dtEndTime + "").JSTime_stringToTime().JSTime_toString(false, true))
                    .Replace("ttp_dtStart", (_res.dtStart.HasValue ? _res.dtStart.Value.formatITA(true) : "- - -"))
                    .Replace("ttp_dtEnd", (_res.dtEnd.HasValue ? _res.dtEnd.Value.formatITA(true) : "- - -"))
                    .Replace("ttp_block_expire", _block_expire.formatITA(true));
                _toolTip += _tp_cont;
            }
            return _toolTip;
        }        
    }
}
