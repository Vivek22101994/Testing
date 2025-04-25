using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    //public class Tmp_ResListItem
    //{
    //    public List<RNT_TBL_RESERVATION> _resList;
    //    public DateTime _date;
    //    public Tmp_ResListItem()
    //    {
    //        _resList = new List<RNT_TBL_RESERVATION>();
    //        _date = new DateTime();
    //    }
    //}
    //public class Tmp_ResList
    //{
    //    public List<Tmp_ResListItem> _resListItem;
    //    public Tmp_ResList()
    //    {
    //        _resListItem = new List<Tmp_ResListItem>();
    //    }
    //    public Tmp_ResListItem getItem(DateTime _date)
    //    {
    //        Tmp_ResListItem _item = _resListItem.SingleOrDefault(x => x._date == _date);
    //        if (_item==null)
    //        {
    //            _item = new Tmp_ResListItem();
    //            _item._date = _date;
    //            _item._resList = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 && x.date_start.HasValue && x.date_end.HasValue).ToList();
    //            _resListItem.Add(_item);
    //        }
    //        return _item;
    //    }
    //}

    public partial class rnt_reservation_old : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_reservation_planner";
        }
        private magaRental_DataContext DC_RENTAL;
        private List<RNT_TB_ESTATE> TMP_ESTATEs_;
        private List<RNT_TB_ESTATE> TMP_ESTATEs
        {
            get
            {
                if (TMP_ESTATEs_ == null)
                    if (Session["TMP_ESTATEs_rnt_reservation"] != null)
                        TMP_ESTATEs_ = (List<RNT_TB_ESTATE>)Session["TMP_ESTATEs_rnt_reservation"];
                    else
                        TMP_ESTATEs_ = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted == 0).ToList();

                return TMP_ESTATEs_;
            }
            set
            {
                Session["TMP_ESTATEs_rnt_reservation"] = value;
                TMP_ESTATEs_ = value;
            }
        }
        private List<RNT_TB_ESTATE> CURRENT_ESTATEs_;
        private List<RNT_TB_ESTATE> CURRENT_ESTATEs
        {
            get
            {
                if (CURRENT_ESTATEs_ == null)
                    if (Session["CURRENT_ESTATEs_rnt_reservation"] != null)
                        CURRENT_ESTATEs_ = (List<RNT_TB_ESTATE>)Session["CURRENT_ESTATEs_rnt_reservation"];
                    else
                        CURRENT_ESTATEs_ = new List<RNT_TB_ESTATE>();

                return CURRENT_ESTATEs_;
            }
            set
            {
                Session["CURRENT_ESTATEs_rnt_reservation"] = value;
                CURRENT_ESTATEs_ = value;
            }
        }
        private List<RNT_TBL_RESERVATION> CURRENT_RESERVATIONs_;
        private List<RNT_TBL_RESERVATION> CURRENT_RESERVATIONs
        {
            get
            {
                if (CURRENT_RESERVATIONs_ == null)
                    if (Session["CURRENT_RESERVATIONs_rnt_reservation"] != null)
                        CURRENT_RESERVATIONs_ = (List<RNT_TBL_RESERVATION>)Session["CURRENT_RESERVATIONs_rnt_reservation"];
                    else
                        CURRENT_RESERVATIONs_ = new List<RNT_TBL_RESERVATION>();

                return CURRENT_RESERVATIONs_;
            }
            set
            {
                Session["CURRENT_RESERVATIONs_rnt_reservation"] = value;
                CURRENT_RESERVATIONs_ = value;
            }
        }
        protected List<string> _dates_;
        protected List<string> _dates
        {
            get
            {
                if (_dates_ == null)
                    if (ViewState["_dates_"] != null)
                    {
                        _dates_ = ((string[]) ViewState["_dates_"]).ToList();

                    }
                    else
                        _dates_ = new List<string>();

                return _dates_;
            }
            set
            {
                ViewState["_dates_"] = value.ToArray();
                _dates_ = value;
            }
        }
        public int IdEstate
        {
            get
            {
                return HF_pid_estate.Value.ToInt32();
            }
            set
            {
                HF_pid_estate.Value = value.ToString();
            }
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
        public event EventHandler onChange;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                if (IdEstate == 0)
                {
                    ltr_filter_estates.Text = "is_active==1 and is_deleted==0";
                }
                else
                {
                    ltr_filter_estates.Text = "id==" + IdEstate;
                }
                TMP_ESTATEs = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted == 0).ToList();
                Bind_lbx_zone();
                Bind_lbx_estate();
                Bind_rbl_month();
                List<DateTime> _newDates = new List<DateTime>();
                DateTime _dt = DateTime.Now.AddDays(7);
                HF_date_from.Value = DateTime.Now.JSCal_dateToString();
                HF_date_to.Value = _dt.JSCal_dateToString();
                while (_dt <= DateTime.Now)
                {
                    _newDates.Add(_dt);
                    _dt = _dt.AddDays(1);
                }
                _dates = _newDates.Select(_date => _date.JSCal_dateToString()).ToList();
                Bind_drp_flt_state();
                Bind_drp_min_num_persons_max();
                Bind_drp_min_num_rooms_bed();
                Bind_drp_min_importance_stars();
                ltr_filter_estates.Text = "1==2";
                LDS_estates.Where = ltr_filter_estates.Text;
            }
            RegisterScripts();
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
            drp_min_num_persons_max.Items.Add(new ListItem("--","-1"));
            int _max = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.num_persons_max.HasValue).Max(x => x.num_persons_max.Value);
            for(int i =1;i<=_max;i++)
            {
                drp_min_num_persons_max.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_min_num_rooms_bed()
        {
            drp_min_num_rooms_bed.Items.Clear();
            drp_min_num_rooms_bed.Items.Add(new ListItem("--", "-1"));
            int _max = DC_RENTAL.RNT_TB_ESTATE.Where(x=>x.num_rooms_bed.HasValue).Max(x=>x.num_rooms_bed.Value);
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
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang==1).OrderBy(x=>x.title).ToList();
            lbx_zone.DataSource = _list;
            lbx_zone.DataValueField = "id";
            lbx_zone.DataTextField = "title";
            lbx_zone.DataBind();
        }
        protected void lbx_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_estate();
        }
        protected void Bind_lbx_estate()
        {
            List<int?> _zoneIDs = new List<int?>();
            foreach(ListItem _item in lbx_zone.Items)
            {
                if(_item.Selected)
                    _zoneIDs.Add(_item.Value.ToInt32());
            }
            List<RNT_TB_ESTATE> _list = new List<RNT_TB_ESTATE>();
            if (_zoneIDs.Count > 0)
                _list = TMP_ESTATEs.Where(x => x.is_active == 1 && x.is_deleted == 0 && _zoneIDs.Contains(x.pid_zone)).OrderBy(x => x.code).ToList();
            else
                _list = TMP_ESTATEs.Where(x => x.is_active == 1 && x.is_deleted == 0).OrderBy(x => x.code).ToList();
            lbx_estate.DataSource = _list;
            lbx_estate.DataValueField = "id";
            lbx_estate.DataTextField = "code";
            lbx_estate.DataBind();
            if(lbx_estate.Items.Count==0)
                lbx_estate.Items.Add(new ListItem("-nessuna strutture trovata-","-1"));
        }
        protected void Bind_rbl_month()
        {
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
        protected void LV_estates_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            ListView LV = e.Item.FindControl("LV") as ListView;
            LV.DataSource = _dates.Select(_date => _date.JSCal_stringToDate()).ToList(); 
            LV.DataBind();
        }

        protected List<RNT_LK_RESERVATION_STATE> _resStateList;
        //protected Tmp_ResList _Tmp_ResList;
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (_resStateList == null)
                _resStateList = DC_RENTAL.RNT_LK_RESERVATION_STATEs.ToList();
            //if (_Tmp_ResList == null)
                //_Tmp_ResList = new Tmp_ResList();

            Label lbl_date = e.Item.FindControl("lbl_date") as Label;
            Label lbl_stato1 = e.Item.FindControl("lbl_stato1") as Label;
            Label lbl_stato2 = e.Item.FindControl("lbl_stato2") as Label;
            Label lbl_unique_id1 = e.Item.FindControl("lbl_unique_id1") as Label;
            Label lbl_unique_id2 = e.Item.FindControl("lbl_unique_id2") as Label;
            lbl_unique_id1.Text = CommonUtilities.newUniqueID();
            lbl_unique_id2.Text = CommonUtilities.newUniqueID();
            string _ttp1 = "";
            string _ttp2 = "";
            System.Web.UI.HtmlControls.HtmlTableCell td_cont = e.Item.FindControl("td_cont") as System.Web.UI.HtmlControls.HtmlTableCell;

            ListView LV = e.Item.Parent.Parent as ListView;
            ListViewItem _item = LV.Parent as ListViewItem;
            Label lbl_id_estate = _item.FindControl("lbl_id") as Label;
            DateTime _date = lbl_date.Text.JSCal_stringToDate();
            if (_date.DayOfWeek == DayOfWeek.Sunday || _date.DayOfWeek == DayOfWeek.Saturday)
            {
                //td_cont.Attributes.Add("style", "border-left:2px solid red; border-right:2px solid red;");
            }
            string _abbr = "";
            string _css_class = "res_free";
            bool _1_isSet = false;
            bool _2_isSet = false;
            List<RNT_TBL_RESERVATION> _resList = CURRENT_RESERVATIONs.Where(x => x.pid_estate == lbl_id_estate.Text.ToInt32() && x.dtStart <= _date && x.dtEnd >= _date).ToList();
            if (_resList.Count == 0)
            {
                lbl_stato1.Text = _abbr;
                lbl_stato1.Attributes.Add("class", _css_class);
                lbl_stato1.Attributes.Add("style", "width:100%");
                lbl_stato1.Visible = true;
                lbl_stato2.Visible = false;
                return;
            }
            RNT_LK_RESERVATION_STATE _resState;
            RNT_TBL_RESERVATION _res1 = _resList.FirstOrDefault(x => x.dtEnd.Value.Date == _date);
            if (_res1 != null)
            {
                _resState = _resStateList.SingleOrDefault(x => x.id == _res1.state_pid);
                if (_resState != null)
                {
                    _abbr = _resState.abbr;
                    _css_class = _resState.css_class;
                }
                lbl_stato1.Text = _abbr;
                lbl_stato1.Attributes.Add("class", _css_class + " _tooltip");
                _1_isSet = true;
                lbl_stato1.Attributes.Add("title", lbl_unique_id1.Text);
                _ttp1 = "<div style=\"display: none;\" id=\"tooltip_" + lbl_unique_id1.Text + "\">";
                _ttp1 += ltr_tooltip_template.Text.Replace("ttp_cl_name_full", _res1.cl_name_honorific + " " + _res1.cl_name_full).Replace("ttp_code", _res1.code).Replace("ttp_persons", "" + (_res1.num_adult + _res1.num_child_over)).Replace("ttp_pr_total", _res1.pr_total.ToString()).Replace("ttp_block_expire", _res1.block_expire.Value.formatITA(false));
                _ttp1 += "</div>";
                lbl_stato1.Visible = true;
                lbl_stato2.Visible = true;
            }
            RNT_TBL_RESERVATION _res2 = _resList.FirstOrDefault(x => x.dtStart.Value.Date == _date);
            if (_res2 != null)
            {
                _resState = _resStateList.SingleOrDefault(x => x.id == _res2.state_pid);
                if (_resState != null)
                {
                    _abbr = _resState.abbr;
                    _css_class = _resState.css_class;
                }
                lbl_stato2.Text = _abbr;
                lbl_stato2.Attributes.Add("class", _css_class + " _tooltip");
                    _2_isSet = true;
                lbl_stato2.Attributes.Add("title", lbl_unique_id2.Text);
                _ttp2 = "<div style=\"display: none;\" id=\"tooltip_" + lbl_unique_id2.Text + "\">";
                _ttp2 += ltr_tooltip_template.Text.Replace("ttp_cl_name_full", _res2.cl_name_honorific+ " " + _res2.cl_name_full).Replace("ttp_code", _res2.code).Replace("ttp_persons", "" + (_res2.num_adult + _res2.num_child_over)).Replace("ttp_pr_total", _res2.pr_total.ToString()).Replace("ttp_block_expire", _res2.block_expire.Value.formatITA(false));
                _ttp2 += "</div>";
                lbl_stato1.Visible = true;
                lbl_stato2.Visible = true;
            }
            RNT_TBL_RESERVATION _res3 = _resList.FirstOrDefault(x => x.dtEnd.Value.Date > _date && x.dtStart.Value.Date < _date);
            if (_res3 != null)
            {
                _resState = _resStateList.SingleOrDefault(x => x.id == _res3.state_pid);
                if (_resState != null)
                {
                    _abbr = _resState.abbr;
                    _css_class = _resState.css_class;
                }
                lbl_stato1.Text = _abbr;
                lbl_stato1.Attributes.Add("class", _css_class + " _tooltip");
                lbl_stato1.Attributes.Add("style", "width:100%");
                lbl_stato1.Attributes.Add("title", lbl_unique_id1.Text);
                _ttp1 = "<div style=\"display: none;\" id=\"tooltip_" + lbl_unique_id1.Text + "\">";
                _ttp1 += ltr_tooltip_template.Text.Replace("ttp_cl_name_full", _res3.cl_name_honorific + " " + _res3.cl_name_full).Replace("ttp_code", _res3.code).Replace("ttp_persons", "" + (_res3.num_adult + _res3.num_child_over)).Replace("ttp_pr_total", _res3.pr_total.ToString()).Replace("ttp_block_expire", _res3.block_expire.Value.formatITA(false));
                _ttp1 += "</div>";
                lbl_stato1.Visible = true;
                lbl_stato2.Visible = false;
                _1_isSet = true;
                _2_isSet = true;
            }
            if (!_1_isSet) lbl_stato1.Attributes.Add("class", "res_free");
            if (!_2_isSet) lbl_stato2.Attributes.Add("class", "res_free");
            lbl_stato1.Text = "";
            lbl_stato2.Text = "";
            ltr_tooltip_cont.Text += _ttp1 + _ttp2;
        }

        protected void LV_date_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_date = e.Item.FindControl("lbl_date") as Label;
            Label lbl_stato = e.Item.FindControl("lbl_stato") as Label;
            DateTime _date = lbl_date.Text.JSCal_stringToDate();
            lbl_stato.Text = _date.getDayOfWeekITA(true) + " <hr/>" + _date.Day + " " + _date.getMonthITA(true);
            lbl_stato.Text = lbl_stato.Text.htmlNoBreakSpace();
            System.Web.UI.HtmlControls.HtmlTableCell td_cont = e.Item.FindControl("td_cont") as System.Web.UI.HtmlControls.HtmlTableCell;
            return;
            if (_date.DayOfWeek == DayOfWeek.Sunday || _date.DayOfWeek == DayOfWeek.Saturday)
            {
                td_cont.Attributes.Add("style", "border-left:2px solid red; border-right:2px solid red;");
            }

        }

        protected void LV_estates_DataBound(object sender, EventArgs e)
        {
            ListView LV_date = LV_estates.FindControl("LV_date") as ListView;
            if (LV_date == null) return;
            LV_date.DataSource = _dates.Select(_date => _date.JSCal_stringToDate()).ToList(); ;
            LV_date.DataBind();
        }

        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            RefreshList();
        }
        protected void Bind_LV_estates()
        {
            LV_estates.DataSource = CURRENT_ESTATEs;
            LV_estates.DataBind();
        }

        protected void flt_Estates(DateTime _dtStart, DateTime _dtEnd)
        {
            if (IdEstate == 0)
            {
                string sql_flt = "";
                string divisor = "";
                List<int?> _zoneIDs = new List<int?>();
                List<int> _estateIDs = new List<int>();
                List<int> _estateIDs_zone = new List<int>();
                List<int> _estateIDs_estate = new List<int>();
                List<int> _estateIDs_props = new List<int>();
                List<int> _estateIDs_state = new List<int>();
                foreach (ListItem _item in lbx_estate.Items)
                {
                    if (_item.Selected)
                    {
                        _estateIDs_estate.Add(_item.Value.ToInt32());
                    }
                }
                foreach (ListItem _item in lbx_zone.Items)
                {
                    if (_item.Selected)
                    {
                        _zoneIDs.Add(_item.Value.ToInt32());
                    }
                }
                if (_estateIDs_estate.Count == 0 && _zoneIDs.Count == 0)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert_" + Unique, "alert('seleziona almeno una zona o un appartamento');", true);
                    //return;
                }
                List<RNT_RL_ESTATE_CONFIG> _propList = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(y => (y.pid_config == 1 || y.pid_config == 2 || y.pid_config == 3)).ToList();
                _estateIDs_props = TMP_ESTATEs.Where(x => 1 == 1 //
                                                                       && (drp_is_exclusive.getSelectedValueInt(0) == -1 //
                                                                           || x.is_exclusive == drp_is_exclusive.getSelectedValueInt(0)) //
                                                                       && (drp_is_loft.getSelectedValueInt(0) == -1 //
                                                                           || x.is_loft == drp_is_loft.getSelectedValueInt(0)) //
                                                                       && (drp_min_num_persons_max.getSelectedValueInt(0) == -1 //
                                                                           || x.num_persons_max >= drp_min_num_persons_max.getSelectedValueInt(0)) //
                                                                       && (drp_min_num_rooms_bed.getSelectedValueInt(0) == -1 //
                                                                           || x.num_rooms_bed >= drp_min_num_rooms_bed.getSelectedValueInt(0)) //
                                                                       && (drp_min_importance_stars.getSelectedValueInt(0) == -1 //
                                                                           || x.importance_stars >= drp_min_importance_stars.getSelectedValueInt(0)) //
                                                                       && (drp_has_air_condition.getSelectedValueInt(0) == -1 //
                                                                           || (drp_has_air_condition.getSelectedValueInt(0) == 1 && _propList.Where(y => y.pid_config == 1 && y.pid_estate == x.id).Count() != 0) //
                                                                           || (drp_has_air_condition.getSelectedValueInt(0) == 0 && _propList.Where(y => y.pid_config == 1 && y.pid_estate == x.id).Count() == 0)) //
                                                                       && (drp_has_internet.getSelectedValueInt(0) == -1 //
                                                                           || (drp_has_internet.SelectedValue == "2or3" && _propList.Where(y => (y.pid_config == 2 || y.pid_config == 3) && y.pid_estate == x.id).Count() != 0)  //
                                                                           || (drp_has_internet.SelectedValue == "2" && _propList.Where(y => (y.pid_config == 2) && y.pid_estate == x.id).Count() != 0)  //
                                                                           || (drp_has_internet.SelectedValue == "3" && _propList.Where(y => (y.pid_config == 3) && y.pid_estate == x.id).Count() != 0)  //
                                                                           || (drp_has_internet.SelectedValue == "2and3" && _propList.Where(y => (y.pid_config == 2) && y.pid_estate == x.id).Count() != 0 && _propList.Where(y => (y.pid_config == 3) && y.pid_estate == x.id).Count() != 0)) //
                                                                       //&& (drp_has_air_condition.getSelectedValueInt(0) == -1 || DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => y.pid_config == 1).Select(y => y.pid_estate).Contains(x.id))
                                                                       //&& (drp_has_air_condition.getSelectedValueInt(0) == -1 || DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => y.pid_config == 1).Select(y => y.pid_estate).Contains(x.id))
                                                                       && 1 == 1).Select(x => x.id).ToList();
                _estateIDs_state = TMP_ESTATEs.Where(x => 1 == 1 //
                                                                       && (drp_flt_state.getSelectedValueInt(0) == -1 //
                                                                           || (drp_flt_state.getSelectedValueInt(0) == 0 //
                                                                               && CURRENT_RESERVATIONs.Where(y => y.pid_estate == x.id //
                                                                                                                            && y.state_pid != 3 //
                                                                                                                            && y.dtStart.HasValue //
                                                                                                                            && y.dtEnd.HasValue //
                                                                                                                            && ((y.dtStart.Value.Date <= _dtStart && y.dtEnd.Value.Date >= _dtEnd) //
                                                                                                                                || (y.dtStart.Value.Date >= _dtStart && y.dtStart.Value.Date < _dtEnd) //
                                                                                                                                || (y.dtEnd.Value.Date > _dtStart && y.dtEnd.Value.Date <= _dtEnd))).Count() == 0) //
                                                                           || (drp_flt_state.getSelectedValueInt(0) > 0 //
                                                                               && CURRENT_RESERVATIONs.Where(y => y.pid_estate == x.id //
                                                                                                                            && y.state_pid == drp_flt_state.getSelectedValueInt(0) //
                                                                                                                            && y.dtStart.HasValue && y.dtEnd.HasValue //
                                                                                                                            && ((y.dtStart.Value.Date <= _dtStart && y.dtEnd.Value.Date > _dtStart) //
                                                                                                                                || (y.dtStart.Value.Date < _dtStart && y.dtEnd.Value.Date >= _dtStart) //
                                                                                                                                || (y.dtStart.Value.Date > _dtStart && y.dtEnd.Value.Date < _dtStart))).Count() != 0)) //
                                                                       //&& (drp_is_loft.getSelectedValueInt(0) == -1 || x.is_loft == drp_is_loft.getSelectedValueInt(0))
                                                                       //&& (drp_min_num_persons_max.getSelectedValueInt(0) == -1 || x.num_persons_max >= drp_min_num_persons_max.getSelectedValueInt(0))
                                                                       //&& (drp_min_num_rooms_bed.getSelectedValueInt(0) == -1 || x.num_rooms_bed >= drp_min_num_rooms_bed.getSelectedValueInt(0))
                                                                       //&& (drp_min_importance_stars.getSelectedValueInt(0) == -1 || x.importance_stars >= drp_min_importance_stars.getSelectedValueInt(0))
                                                                       //&& (drp_has_air_condition.getSelectedValueInt(0) == -1 || (drp_has_air_condition.getSelectedValueInt(0) == 1 && DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => y.pid_config == 1 && y.pid_estate == x.id).Count() != 0) || (drp_has_air_condition.getSelectedValueInt(0) == 0 && DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => y.pid_config == 1 && y.pid_estate == x.id).Count() == 0))
                                                                       //&& (drp_has_internet.getSelectedValueInt(0) == -1 || (drp_has_internet.SelectedValue == "2or3" && DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => (y.pid_config == 2 || y.pid_config == 3) && y.pid_estate == x.id).Count() != 0) || (drp_has_internet.SelectedValue == "2" && DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => (y.pid_config == 2) && y.pid_estate == x.id).Count() != 0) || (drp_has_internet.SelectedValue == "3" && DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => (y.pid_config == 3) && y.pid_estate == x.id).Count() != 0) || (drp_has_internet.SelectedValue == "2and3" && DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => (y.pid_config == 2) && y.pid_estate == x.id).Count() != 0 && DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => (y.pid_config == 3) && y.pid_estate == x.id).Count() != 0))
                                                                       //&& (drp_has_air_condition.getSelectedValueInt(0) == -1 || DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => y.pid_config == 1).Select(y => y.pid_estate).Contains(x.id))
                                                                       //&& (drp_has_air_condition.getSelectedValueInt(0) == -1 || DC_RENTAL.RNT_RL_ESTATE_PROPERTies.Where(y => y.pid_config == 1).Select(y => y.pid_estate).Contains(x.id))
                                                                       && 1 == 1).Select(x => x.id).ToList();
                _estateIDs = _estateIDs_state.Where(x => _estateIDs_props.Contains(x)).ToList();
                if (_estateIDs_estate.Count != 0 || _zoneIDs.Count != 0)
                {
                    if (_estateIDs_estate.Count == 0)
                        _estateIDs_estate.AddRange(TMP_ESTATEs.Where(x => _zoneIDs.Contains(x.pid_zone)).Select(x => x.id).ToList());
                    _estateIDs = _estateIDs.Where(_estateIDs_estate.Contains).ToList();
                }
                CURRENT_ESTATEs = TMP_ESTATEs.Where(x => _estateIDs.Contains(x.id)).ToList();
                Bind_LV_estates();
                return;
                if (_estateIDs.Count == 0)
                {
                    sql_flt = "1==2";
                }
                else
                {
                    divisor = "";
                    foreach (int _id in _estateIDs)
                    {
                        sql_flt += " " + divisor + " id = " + _id;
                        divisor = "or";
                    }
                }
                ltr_filter_estates.Text = "(" + sql_flt + ") and (1==1)";
            }
            else
            {
                ltr_filter_estates.Text = "id==" + IdEstate;
            }
            // todo
            //ltr_filter_estates.Text = "is_active==1 and is_deleted==0";
            LDS_estates.Where = ltr_filter_estates.Text;
            LV_estates.DataBind();
        }

        public void RefreshList()
        {
            List<DateTime> _newDates = new List<DateTime>();
            DateTime _dt = DateTime.Now.AddDays(-30);
            DateTime _dtStart = _dt;
            DateTime _dtEnd = DateTime.Now;
            if (HF_dateMode.Value == "1" || rbl_month.Items.Count == 0)
            {
                if (HF_date_from.Value != "" && HF_date_to.Value != "")
                {
                    _dt = HF_date_from.Value.JSCal_stringToDate();
                    _dtEnd = HF_date_to.Value.JSCal_stringToDate();
                }
                else if (HF_date_to.Value != "")
                {
                    _dtEnd = HF_date_to.Value.JSCal_stringToDate();
                    _dt = _dtEnd.AddDays(-30);
                }
                else if (HF_date_from.Value != "")
                {
                    _dt = HF_date_from.Value.JSCal_stringToDate();
                    _dtEnd = _dt.AddDays(30);
                }
            }
            else
            {
                _dt = rbl_month.SelectedValue.JSCal_stringToDate();
                _dtEnd = _dt.AddDays(DateTime.DaysInMonth(_dt.Year, _dt.Month) - 1);
            }
            _dtStart = _dt;
            while (_dt <= _dtEnd)
            {
                _newDates.Add(_dt);
                _dt = _dt.AddDays(1);
            }
            _dates = _newDates.Select(_date => _date.JSCal_dateToString()).ToList();
            CURRENT_RESERVATIONs = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 //
                                                                             && x.dtStart.HasValue //
                                                                             && x.dtEnd.HasValue //
                                                                             && ( 1==2
                                                                                 || (x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                 || (x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtStart && x.dtEnd.Value.Date <= _dtEnd) //
                                                                                 || (x.dtStart.Value.Date >= _dtStart && x.dtStart.Value.Date <= _dtEnd && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                 || (x.dtStart.Value.Date >= _dtStart && x.dtEnd.Value.Date <= _dtEnd) //
                                                                                 //|| (x.dtStart >= _dtStart && x.dtEnd <= _dtStart) //
                                                                                 || 1 == 2)).ToList();
            flt_Estates(_dtStart, _dtEnd);
        }
        protected void lnk_stamp_Click(object sender, EventArgs e)
        {
            //List<DateTime> _newDates = new List<DateTime>();
            //DateTime _dt = DateTime.Now.AddDays(-30);
            //DateTime _dtTo = DateTime.Now;
            //if (HF_date_from.Value != "" && HF_date_to.Value != "")
            //{
            //    _dt = HF_date_from.Value.JSCal_stringToDate();
            //    _dtTo = HF_date_to.Value.JSCal_stringToDate();
            //}
            //else if (HF_date_to.Value != "")
            //{
            //    _dtTo = HF_date_to.Value.JSCal_stringToDate();
            //    _dt = _dtTo.AddDays(-30);
            //}
            //else if (HF_date_from.Value != "")
            //{
            //    _dt = HF_date_from.Value.JSCal_stringToDate();
            //    _dtTo = _dt.AddDays(30);
            //}
            //string _redirect = "stampa_presenze.aspx";
            //_redirect += "?from=" + _dt.JSCal_dateToString() + "&to=" + _dtTo.JSCal_dateToString();
            //string sql = "";
            //string divisor = "";
            //foreach (ListItem _item in chkList_estates.Items)
            //{
            //    if (_item.Selected)
            //    {
            //        sql += "" + divisor + "" + _item.Value;
            //        divisor = ";";
            //    }
            //}
            //_redirect += "&users=" + sql;
            //Response.Redirect(_redirect);
        }
        private void RegisterScripts()
        {
            if (!IsPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "cal_date_from_init_" + Unique, "var cal_date_from_" + Unique + " = null;", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "cal_date_to_init_" + Unique, "var cal_date_to_" + Unique + " = null;", true);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cal_date_payment_" + Unique, "cal_date_from_" + Unique + " = new JSCal_single('cal_date_from', '" + HF_date_from.ClientID + "', 'cal_date_from', null, '', 'del_date_from');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cal_date_to_" + Unique, "cal_date_to_" + Unique + " = new JSCal_single('cal_date_to', '" + HF_date_to.ClientID + "', 'cal_date_to', null, '', 'del_date_to');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setToolTip_" + Unique, "setToolTip();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "changeDateMode_" + Unique, "changeDateMode('" + HF_dateMode.Value + "');", true);
            
        }

    }
}
