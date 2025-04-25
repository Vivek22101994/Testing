using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class SeasonDates : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_period";
        }
        protected magaRental_DataContext DC_RENTAL;

        protected int SeasonGroup { get { return HF_SeasonGroup.Value.ToInt32(); } set { HF_SeasonGroup.Value = "" + value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                SeasonGroup = Request.QueryString["pidSeasonGroup"].ToInt32();
                if (SeasonGroup == 0)
                    SeasonGroup = 1;
                if (Request.QueryString["deleteId"].ToInt32() > 0)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        if (Request.QueryString["deleteId"].ToInt32() < 4 || DC_RENTAL.RNT_TB_ESTATE.FirstOrDefault(x => x.pidSeasonGroup == Request.QueryString["deleteId"].ToInt32()) != null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "errorAlert", "alert('Questa stagionalita non puo essere eliminata!');", true);

                        }
                        else
                        {
                            var SeasonGroupTBL = dc.dbRntSeasonGroupTBLs.SingleOrDefault(x => x.id == Request.QueryString["deleteId"].ToInt32());
                            if (SeasonGroupTBL != null)
                            {
                                dc.Delete(SeasonGroupTBL);
                                dc.SaveChanges();
                            }
                        }
                    }
                }
                LV_DataBind();
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            LV_DataBind();
        }
        List<dbRntSeasonDatesTBL> currList;
        protected void LV_DataBind()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var SeasonGroupTBL = dc.dbRntSeasonGroupTBLs.SingleOrDefault(x => x.id == SeasonGroup);
                if (SeasonGroupTBL == null)
                {
                    Response.Redirect("SeasonDates.aspx");
                    return;
                }
                ltr_groupTitle.Text = SeasonGroupTBL.code;
                ltr_groupDesc.Text = SeasonGroupTBL.description;
                //RNT_TB_ESTATE
                string AptListString = "";
                var AptListList = AppSettings.RNT_TB_ESTATE.Where(x => x.pidSeasonGroup == SeasonGroup || (SeasonGroup == 0 && !x.pidSeasonGroup.HasValue)).OrderBy(x => x.code).ToList();
                foreach (var tmp in AptListList)
                {
                    AptListString += tmp.code + "<br/>";
                }
                ltr_AptList.Text = AptListString;
                string OtherGroupsString = "";
                var OtherGroupsList = dc.dbRntSeasonGroupTBLs.Where(x => x.id != SeasonGroup).OrderBy(x => x.code).ToList();
                foreach (var tmp in OtherGroupsList)
                {
                    OtherGroupsString += "<a href=\"SeasonDates.aspx?pidSeasonGroup=" + tmp.id + "\">" + tmp.code + "</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"SeasonDates.aspx?pidSeasonGroup=" + SeasonGroup + "&deleteId=" + tmp.id + "\" onclick=\"return confirm('Stai per eliminare definitivamente?');\" class=\"deletegroup\">elimina</a><br/>";
                }
                ltr_OtherGroups.Text = OtherGroupsString;

                if(currList==null)
                    currList = dc.dbRntSeasonDatesTBLs.Where(x => x.pidSeasonGroup == SeasonGroup).OrderBy(x => x.dtStart).ToList();
                //LV.DataSource = currList;
                //LV.DataBind();
                DateTime _dtStart = DateTime.Now;
                DateTime _dtEnd = _dtStart.AddDays(7);
                bool _isSet = false;
                bool _hasHole = false;
                DateTime _dtLast = _dtStart;
                string scriptOnSelect = "";
                string _script = "";
                _script += "function checkCalDates(date){var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
                foreach (var tmp in currList)
                {
                    if (_isSet)
                    {
                        if (_dtEnd.AddDays(1) != tmp.dtStart)
                        {
                            _dtStart = _dtEnd.AddDays(1);
                            _dtEnd = tmp.dtStart.AddDays(-1);
                            _hasHole = true;
                        }
                    }
                    if (!_hasHole)
                    {
                        _dtStart = tmp.dtStart;
                        _dtEnd = tmp.dtEnd;
                    }
                    _isSet = true;
                    _dtLast = tmp.dtEnd;
                    string _intDateFrom = "" + tmp.dtStart.JSCal_dateToInt();
                    string _intDateTo = "" + tmp.dtEnd.JSCal_dateToInt();
                    string type = "nd";
                    if (tmp.pidPeriod == 1) type = "sel";
                    if (tmp.pidPeriod == 2) type = "opz";
                    if (tmp.pidPeriod == 3) type = "prt";
                    if (tmp.pidPeriod == 4) type = "mv";
                    _script += "if(dateint <= " + _intDateTo + " && dateint >= " + _intDateFrom + ") { _controls += '<span class=\"rntCal " + type + "_f\"></span>'; }";
                    scriptOnSelect += "if(dateint <= " + _intDateTo + " && dateint >= " + _intDateFrom + ") { selectDate(" + tmp.id + ", dateint); return false; }";
                }
                _script += "if(_controls=='') { _controls += '<span class=\"rntCal nd_f\"></span>'; }";
                _script += "return [_enabled, _class, _tooltip, _controls];";
                _script += "}";
                _script += "function avvCalendarOnSelect(dateText, inst) {";
                _script += "if ($(this).datepicker(\"getDate\") == null) return;";
                _script += "var dateint = JSCal.dateToInt($(this).datepicker(\"getDate\"));";
                _script += scriptOnSelect;
                _script += " selectDate(0, dateint);";
                _script += "}";
                if (!_hasHole)
                {
                    _dtStart = _dtLast.AddDays(1);
                    _dtEnd = _dtLast.AddDays(8);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CheckDisabledDate", _script, true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setCal", "setCal();", true);
            }
        }
        private void FillControls()
        {
            //RNT_TBL_PERIOD_DATE_1 _date = DC_RENTAL.RNT_TBL_PERIOD_DATE_1.SingleOrDefault(item => item.id == HF_id_date.Value.ToInt32());
            //if (_date == null)
            //{
            //    _date = new RNT_TBL_PERIOD_DATE_1();
            //    HF_id_date.Value = "0";
            //}
            //DateTime _dtStart = _date.dtStart.HasValue ? _date.dtStart.Value : DateTime.Now;
            //DateTime _dtEnd = _date.dtEnd.HasValue ? _date.dtEnd.Value : _dtStart.AddDays(7);
            //HF_dtStart.Value = _dtStart.JSCal_dateToString();
            //HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
            //List<RNT_TBL_PERIOD_DATE_1> _list = DC_RENTAL.RNT_TBL_PERIOD_DATE_1.Where(x => x.id != HF_id_date.Value.ToInt32()).OrderBy(x => x.dtStart).ToList();
            //bool _isSet = false;
            //bool _hasHole = false;
            //DateTime _dtLast = _dtStart;
            //string _script = "";
            //_script += "function CheckDisabledDate(date){dateint = JSCal.dateToInt(date);";
            //foreach (RNT_TBL_PERIOD_DATE_1 _pp in _list)
            //{
            //    if (_isSet)
            //    {
            //        if (_dtEnd.AddDays(1) != _pp.dtStart.Value)
            //        {
            //            _dtStart = _dtEnd.AddDays(1);
            //            _dtEnd = _pp.dtStart.Value.AddDays(-1);
            //            _hasHole = true;
            //        }
            //    }
            //    if (!_hasHole)
            //    {
            //        _dtStart = _pp.dtStart.Value;
            //        _dtEnd = _pp.dtEnd.Value;
            //    }
            //    _isSet = true;
            //    _dtLast = _pp.dtEnd.Value;
            //    string _intDateFrom = "" + _pp.dtStart.Value.JSCal_dateToInt();
            //    string _intDateTo = "" + _pp.dtEnd.Value.JSCal_dateToInt();
            //    _script += "if(dateint <= " + _intDateTo + " && dateint >= " + _intDateFrom + ") return [false, \"\"];;";
            //}
            //_script += "return [true, \"\"];";
            //_script += "}";
            //if (_hasHole && HF_id_date.Value.ToInt32() == 0)
            //{
            //    HF_dtStart.Value = _dtStart.JSCal_dateToString();
            //    HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
            //}
            //else if (HF_id_date.Value.ToInt32() == 0)
            //{
            //    HF_dtStart.Value = _dtLast.AddDays(1).JSCal_dateToString();
            //    HF_dtEnd.Value = _dtLast.AddDays(8).JSCal_dateToString();
            //}
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "CheckDisabledDate", _script, true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "setCal", "setCal(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);
            
            //pnlContent.Visible = true;
        }
        protected void FillDataFromControls()
        {
        //    RNT_TBL_PERIOD_DATE_1 _date = DC_RENTAL.RNT_TBL_PERIOD_DATE_1.SingleOrDefault(item => item.id == HF_id_date.Value.ToInt32());
        //    if (_date == null)
        //    {
        //        _date = new RNT_TBL_PERIOD_DATE_1();
        //        DC_RENTAL.RNT_TBL_PERIOD_DATE_1.InsertOnSubmit(_date);
        //        _date.pid_period = HF_id.Value.ToInt32();
        //    }
        //    _date.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
        //    _date.dtStart = HF_dtStart.Value.JSCal_stringToDate();
        //    DC_RENTAL.SubmitChanges();
        //    AppSettings.RNT_PERIOD_DATE_1 = DC_RENTAL.RNT_TBL_PERIOD_DATE_1.ToList();
        //    LDS_date.DataBind();
        //    LV_DataBind();
        //    pnlContent.Visible = false;
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

       
    }
}
