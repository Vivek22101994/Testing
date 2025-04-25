using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using RNT_estateDate = AppSettings.RNT_estateDate;
using RNT_dateItem = AppSettings.RNT_dateItem;

namespace RentalInRome.admin
{
    public partial class rnt_reservation_old_2 : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_reservation_planner";
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        protected List<AppSettings.RNT_estate> _estateList;
        protected List<DateTime> _dates;
        protected List<RNT_TBL_RESERVATION> _resList;
        protected string resList_tabDays;
        protected string resList_estate;
        protected string resList_script;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                DateTime _dtStart = DateTime.Now.Date;
                DateTime _dtEnd = DateTime.Now.Date.AddDays(7);
                int _dtStartInt = Request.QueryString["dtStart"].objToInt32();
                int _dtEndInt = Request.QueryString["dtEnd"].objToInt32();
                HF_dtStart.Value = _dtStart.JSCal_dateToString();
                HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
                if (_dtStartInt != 0 && _dtEndInt != 0)
                {
                    _dtStart = _dtStartInt.JSCal_intToDate();
                    _dtEnd = _dtEndInt.JSCal_intToDate();
                }
                else
                {
                    RegisterScripts();
                    return;
                }
                HF_dtStart.Value = _dtStart.JSCal_dateToString();
                HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
                int _nights = (_dtEnd - _dtStart).Days;
                DateTime _dt = _dtStart;
                _dates = new List<DateTime>();
                while (_dt <= _dtEnd)
                {
                    _dates.Add(_dt);
                    _dt = _dt.AddDays(1);
                }

                _resList = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 && x.is_deleted != 1 && x.dtStart.HasValue && x.dtEnd.HasValue && ((x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                                                                                                                        || (x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtStart && x.dtEnd.Value.Date <= _dtEnd) //
                                                                                                                                                                                        || (x.dtStart.Value.Date >= _dtStart && x.dtStart.Value.Date <= _dtEnd && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                                                                                                                        || (x.dtStart.Value.Date >= _dtStart && x.dtEnd.Value.Date <= _dtEnd) //
                                                                                                                                                                                        || 1 == 2)).ToList();
                _estateList = AppSettings.RNT_estateList.Where(x => x.nights_min <= _nights).OrderBy(x => x.code).ToList();
                RNT_estateList_refreshDates();
                Bind_lbx_zone();
                Bind_rbl_month();
                Bind_drp_flt_state();
                Bind_drp_min_num_persons_max();
                Bind_drp_min_num_rooms_bed();
                Bind_drp_min_importance_stars();
            }
            RegisterScripts();
        }
        protected void RNT_estateList_()
        {
        }
        protected void RNT_estateList_refreshDates()
        {
            RNT_estateList_fillToolTip();
            RNT_estateList_fillDays();
            RNT_estateList_fillResList();
            ltr_dayList.Text = resList_tabDays;
            ltr_estateList.Text = resList_estate;
            resList_script += "$(\"#" + lbx_zone.ClientID + "\").change(function() {RNT_EstateList.filter_zoneList();});";
            resList_script += "$(\"#" + lbx_estate.ClientID + "\").change(function() {RNT_EstateList.filter_estateList();});";

            resList_script += "$(\"#" + drp_min_num_persons_max.ClientID + "\").change(function() {RNT_EstateList.filter_estateProps();});";
            resList_script += "$(\"#" + drp_min_num_rooms_bed.ClientID + "\").change(function() {RNT_EstateList.filter_estateProps();});";
            resList_script += "$(\"#" + drp_min_importance_stars.ClientID + "\").change(function() {RNT_EstateList.filter_estateProps();});";
            resList_script += "$(\"#" + drp_has_air_condition.ClientID + "\").change(function() {RNT_EstateList.filter_estateProps();});";
            resList_script += "$(\"#" + drp_is_exclusive.ClientID + "\").change(function() {RNT_EstateList.filter_estateProps();});";
            resList_script += "$(\"#" + drp_is_loft.ClientID + "\").change(function() {RNT_EstateList.filter_estateProps();});";
            resList_script += "$(\"#" + drp_has_internet.ClientID + "\").change(function() {RNT_EstateList.filter_estateProps();});";
            resList_script += "$(\"#" + drp_flt_state.ClientID + "\").change(function() {RNT_EstateList.filter_estateProps();});";
            ltr_script.Text = resList_script;
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "inintAll", resList_script,true);
        }
        protected void RNT_estateList_fillToolTip()
        {
            ltr_tooltip_cont.Text = "";
            foreach (RNT_TBL_RESERVATION _res in _resList)
            {
                string _tp_cont = "<div style=\"display: none;\" id=\"tooltip_" + _res.id + "\">";
                _tp_cont += ltr_tooltip_template.Text;
                _tp_cont += "</div>";
                DateTime _block_expire = _res.block_expire.HasValue ? _res.block_expire.Value.Date : _res.dtCreation.Value.AddDays(2);
                _tp_cont = _tp_cont.Replace("ttp_cl_name_full", _res.cl_name_honorific + " " + _res.cl_name_full).Replace("ttp_code", _res.code).Replace("ttp_persons", "" + (_res.num_adult + _res.num_child_over)).Replace("ttp_pr_total", ("" + _res.pr_total)).Replace("ttp_block_expire", _block_expire.formatITA(true));
                ltr_tooltip_cont.Text += _tp_cont;
            }
        }
        protected void RNT_estateList_fillDays()
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
            resList_tabDays = _tbl;
        }
        protected void RNT_estateList_fillResList()
        {
            resList_estate = "";
            resList_script = "RNT_EstateList = new RNT.EstateList();";
            foreach (AppSettings.RNT_estate _est in _estateList)
            {
                var scheduleStr = "<table id=\"" + _est.id + "_schedule\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-bottom: 2px; margin-top: 2px;\">";
                scheduleStr += "<tr>";
                List<int> _stateList = new List<int>();
                foreach (DateTime _dt in _dates)
                {
                    bool _isLast = _dates.Max() == _dt;
                    // aggiungi la data
                    RNT_estateDate _dtEst = new RNT_estateDate(_est.id, _dt);
                    //_est.Dates.Add(_dtEst);

                    // controlla date
                    bool _1_isSet = false;
                    bool _2_isSet = false;
                    bool _all_isSet = false;
                    List<RNT_TBL_RESERVATION> _resListTmp = _resList.Where(x => x.pid_estate == _est.id && x.dtStart.Value.Date <= _dtEst.Date && x.dtEnd.Value.Date >= _dtEst.Date).ToList();
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
                            if (!_isLast && !_stateList.Contains(_res.state_pid.Value))
                                _stateList.Add(_res.state_pid.Value);

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
                            if (!_isLast && !_stateList.Contains(_res.state_pid.Value))
                                _stateList.Add(_res.state_pid.Value);

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
                    scheduleStr += "<td id=\"" + _est.id + "_" + _dt.JSCal_dateToString() + "_2\" title=\"" + _dtEst.Part_2.Attr_Title + "\" class=\"res_2" + _classList + "\" IdEstate=\"" + _est.id + "\" dtDate=\"" + _dt.JSCal_dateToString() + "\" dtPart=\"2\" IdRes=\"" + _dtEst.Part_2.Attr_IdRes + "\" onclick=\"" + _dtEst.Part_2.onClick + "\"></td>";
                    _classList = "";
                    foreach (string _class in _dtEst.Part_0.ClassList)
                    {
                        _classList += " " + _class;
                    }
                    scheduleStr += "<td id=\"" + _est.id + "_" + _dt.JSCal_dateToString() + "_0\" title=\"" + _dtEst.Part_0.Attr_Title + "\" class=\"res_0" + _classList + "\" IdEstate=\"" + _est.id + "\" dtDate=\"" + _dt.JSCal_dateToString() + "\" dtPart=\"0\" IdRes=\"" + _dtEst.Part_0.Attr_IdRes + "\" onclick=\"" + _dtEst.Part_0.onClick + "\"></td>";
                    _classList = "";
                    foreach (string _class in _dtEst.Part_1.ClassList)
                    {
                        _classList += " " + _class;
                    }
                    scheduleStr += "<td id=\"" + _est.id + "_" + _dt.JSCal_dateToString() + "_1\" title=\"" + _dtEst.Part_1.Attr_Title + "\" class=\"res_1" + _classList + "\" IdEstate=\"" + _est.id + "\" dtDate=\"" + _dt.JSCal_dateToString() + "\" dtPart=\"1\" IdRes=\"" + _dtEst.Part_1.Attr_IdRes + "\" onclick=\"" + _dtEst.Part_1.onClick + "\"></td>";
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

                //scheduleStr += "<div id=\"" + _est.id + "_schedule_loader\" style=\"background-image: url('/images/ico/res_loader_small2.gif');background-repeat: repeat-x;display: block;height: 17px;width: 100%;\"></div>";

                // aggiungi al html elenco
                string _tr = "<tr id=\"" + _est.id + "_trCont\" style=\"display:none;\" onmouseover=\"$(this).addClass('tr_current')\" onmouseout=\"$(this).removeClass('tr_current')\" class=\"estate_row tr_normal\">";
                _tr += "<td class=\"nomiTab\">";
                _tr += "<a target='_blank' href='rnt_estate_details.aspx?id=" + _est.id + "'>";
                _tr += "" + _est.code;
                _tr += "</a>";
                _tr += "</td>";
                _tr += "<td id=\"" + _est.id + "_scheduleCont\">";
                _tr += _est.scheduleStr;
                _tr += "</td></tr>";
                resList_estate += _tr;
                string _states = "";
                string _sep = "";
                foreach (int _state in _stateList)
                {
                    _states += _sep + "" + _state;
                    _sep = ",";
                }
                // genera script
                resList_script += "RNT_EstateList.Items.push(new RNT.Estate({ "//
                    + "id: " + _est.id + ", "//
                    + "pid_zone: " + _est.pid_zone + ", "//
                    + "stateList: [" + _states + "], "//
                    + "is_exclusive: " + _est.is_exclusive + ", "//
                    + "is_loft: " + _est.is_loft + ", "//
                    + "num_persons_max: " + _est.num_persons_max + ", "//
                    + "num_rooms_bed: " + _est.num_rooms_bed + ", "//
                    + "importance_stars: " + _est.importance_stars + ", "//
                    //+ "has_air_condition: " + _est.has_air_condition + ", "//
                    //+ "has_adsl: " + _est.has_adsl + ", "//
                    //+ "has_wifi: " + _est.has_wifi + ", "//
                    + "is_nd: " + _est.is_nd.ToString().ToLower() + ", "//
                    + "is_opz: " + _est.is_opz.ToString().ToLower() + ", "//
                    + "is_can: " + _est.is_can.ToString().ToLower() + ", "//
                    + "is_prt: " + _est.is_prt.ToString().ToLower() + ", "//
                    + "is_mv: " + _est.is_mv.ToString().ToLower() + ", "//
                    + "is_free: " + _est.is_free.ToString().ToLower() + ", "//
                    + "code: \"" + _est.code + "\" }));";
            }
        }
        protected string getStateClass(int? id)
        {
            RNT_LK_RESERVATION_STATE _state = AppSettings.RNT_LK_RESERVATION_STATEs.SingleOrDefault(x => x.id == id);
            if (_state != null)
                return _state.css_class;
            return "res_xxx";
        }

        protected void Bind_drp_flt_state()
        {
            List<RNT_LK_RESERVATION_STATE> _list = maga_DataContext.DC_RENTAL.RNT_LK_RESERVATION_STATEs.Where(x => x.type == 1).OrderBy(x => x.title).ToList();
            drp_flt_state.DataSource = _list;
            drp_flt_state.DataValueField = "id";
            drp_flt_state.DataTextField = "title";
            drp_flt_state.DataBind();
            drp_flt_state.Items.Insert(0, new ListItem("Disponibile", "0"));
            drp_flt_state.Items.Insert(0, new ListItem("--", "-1"));
        }

        protected void Bind_drp_min_num_persons_max()
        {
            drp_min_num_persons_max.Items.Clear();
            drp_min_num_persons_max.Items.Add(new ListItem("--", "-1"));
            int _max = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.num_persons_max.HasValue).Max(x => x.num_persons_max.Value);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_num_persons_max.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_min_num_rooms_bed()
        {
            drp_min_num_rooms_bed.Items.Clear();
            drp_min_num_rooms_bed.Items.Add(new ListItem("--", "-1"));
            int _max = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.num_rooms_bed.HasValue).Max(x => x.num_rooms_bed.Value);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_num_rooms_bed.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_min_importance_stars()
        {
            drp_min_importance_stars.Items.Clear();
            drp_min_importance_stars.Items.Add(new ListItem("--", "-1"));
            int _max = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.importance_stars.HasValue).Max(x => x.importance_stars.Value);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_importance_stars.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_lbx_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1).OrderBy(x => x.title).ToList();
            lbx_zone.DataSource = _list;
            lbx_zone.DataValueField = "id";
            lbx_zone.DataTextField = "title";
            lbx_zone.DataBind();
        }
        protected void Bind_rbl_month()
        {
            return;
            rbl_month.Items.Clear();
            int _year = DateTime.Now.Year;
            int _startMonth = 5;
            int _month = DateTime.Now.Month + _startMonth;
            for (int i = _startMonth; i > -5; i--)
            {
                if (_month == 0)
                {
                    _month = 12;
                    _year--;
                }
                DateTime _dt = new DateTime(_year, _month, 1);
                ListItem _item = new ListItem(_dt.getMonthITA(false) + " " + _year, _dt.JSCal_dateToString());
                if (_month == DateTime.Now.Month)
                    _item.Selected = true;
                rbl_month.Items.Add(_item);
                _month--;
            }
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setCal_" + Unique, "setCal_" + Unique + "();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setToolTip_" + Unique, "setToolTip();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "changeDateMode_" + Unique, "changeDateMode('" + HF_dateMode.Value + "');", true);

        }

        protected void lnk_searchCode_Click(object sender, EventArgs e)
        {
            RNT_TBL_RESERVATION _res = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.code == txt_code.Text);
            if (_res == null)
            {
                lbl_codeError.Visible = true;
                return;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenShadowbox_res", "OpenShadowbox('rnt_reservation_form.aspx?IdRes=" + _res.id + "', 800, 0);", true);
        }

    }
}
