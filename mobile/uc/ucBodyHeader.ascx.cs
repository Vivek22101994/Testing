using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.mobile.uc
{
    public partial class ucBodyHeader : System.Web.UI.UserControl
    {
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }

        public string CURRENT_SESSION_ID
        {
            get
            {
                mainBasePage mb = (mainBasePage)this.Page;
                return mb.CURRENT_SESSION_ID.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LV.DataSource = contProps.LangTBL.Where(x => x.is_active == 1 && x.is_public == 1).OrderBy(x => x.id);
                LV.DataBind();
                foreach (var item in LV.Items)
                {
                    HyperLink _hl = item.FindControl("HL") as HyperLink;
                    Label _lbl_id = item.FindControl("lbl_id") as Label;
                    Label lbl_abbr = item.FindControl("lbl_abbr") as Label;
                    if (_hl != null)
                    {
                        _hl.CssClass = lbl_abbr.Text + "";
                        _hl.NavigateUrl = null;
                        _hl.Enabled = true;
                        if (_lbl_id == null)
                            continue;
                        if (_lbl_id.Text == App.LangID.ToString())
                        {
                            _hl.CssClass = lbl_abbr.Text + " active";
                            _hl.Enabled = false;
                            continue;
                        }
                        string _path = GetCurrentLangPath(_lbl_id.Text);
                        if (_path != "")
                        {
                            _hl.NavigateUrl = _path;
                            _hl.Enabled = true;
                        }
                        else
                        {
                            _hl.NavigateUrl = null;
                            _hl.Enabled = false;
                        }
                    }
                }

                checkReservationsCal();
                clConfig _config = clUtils.getConfig(this.CURRENT_SESSION_ID);
                clSearch _ls = _config.lastSearch;

                txt_title.Text = _ls.searchTitle;

                HF_dtStart.Value = _ls.dtStart.JSCal_dateToString();
                HF_dtEnd.Value = _ls.dtEnd.JSCal_dateToString();
                txt_dtCount.Text = _ls.dtCount.ToString();

                int min = AppSettings.RNT_TB_ESTATE.Select(x => x.num_persons_min.objToInt32()).Min();
                if (min == 0) min = 1;
                int max = AppSettings.RNT_TB_ESTATE.Select(x => x.num_persons_max.objToInt32()).Max();
                int maxChildMin = AppSettings.RNT_TB_ESTATE.Select(x => x.num_persons_child.objToInt32()).Max();
                drp_numPers_adult.bind_Numbers(min, max, 1, 0);
                drp_numPers_adult.setSelectedValue(_ls.numPers_adult);
                drp_numPers_childOver.bind_Numbers(1, max, 1, 0);
                drp_numPers_childOver.Items.Insert(0, new ListItem("---", "0"));
                drp_numPers_childOver.setSelectedValue(_ls.numPers_childOver);
                calculateNumPersons();
                drp_numPers_childMin.bind_Numbers(1, maxChildMin, 1, 0);
                drp_numPers_childMin.Items.Insert(0, new ListItem("---", "0"));
                drp_numPers_childMin.setSelectedValue(_ls.numPers_childMin.ToString());
                drp_zone_DataBind();
                drp_zone.setSelectedValue(_ls.currZoneList.FirstOrDefault());
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal_" + Unique + "(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);
        }
        protected string GetCurrentLangPath(string pidLang)
        {
            mainBasePage m = (mainBasePage)this.Page;
            return "/m" + AppUtils.getPagePath(m.PAGE_REF_ID + "", m.PAGE_TYPE, pidLang);
        }
        protected void drp_numPers_adult_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculateNumPersons();
        }
        protected void calculateNumPersons()
        {
            int min = AppSettings.RNT_TB_ESTATE.Select(x => x.num_persons_min.objToInt32()).Min();
            if (min == 0) min = 1;
            int max = AppSettings.RNT_TB_ESTATE.Select(x => x.num_persons_max.objToInt32()).Max();
            int selNum_adult = drp_numPers_adult.getSelectedValueInt(0).objToInt32();
            int selNum_child_over = drp_numPers_childOver.getSelectedValueInt(0).objToInt32();
            drp_numPers_childOver.Items.Clear();
            int _minChildOver = min - selNum_adult;
            if (_minChildOver <= 0)
            {
                _minChildOver = 1;
                drp_numPers_childOver.Items.Add(new ListItem("---", "0"));
            }
            for (int i = _minChildOver; i <= (max - selNum_adult); i++)
            {
                drp_numPers_childOver.Items.Add(new ListItem("" + i, "" + i));
            }
            if (selNum_child_over > (max - selNum_adult)) selNum_child_over = (max - selNum_adult);
            drp_numPers_childOver.setSelectedValue("" + selNum_child_over);
        }
        protected void drp_zone_DataBind()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.pid_lang == CurrentLang.ID && AppSettings.RNT_activeZones.Contains(x.id)).OrderBy(x => x.title).ToList();
            drp_zone.DataSource = _list;
            drp_zone.DataTextField = "title";
            drp_zone.DataValueField = "id";
            drp_zone.DataBind();
            if (drp_zone.Items.Count != 1)
                drp_zone.Items.Insert(0, new ListItem("- " + contUtils.getLabel("lblAll") + " -", ""));
        }
        protected void checkReservationsCal()
        {
            string _script = "";
            _script += "function checkCalDates_" + Unique + "(date){var _dtStart = parseInt($('#" + HF_dtStart.ClientID + "').val()); var _dtEnd = parseInt($('#" + HF_dtEnd.ClientID + "').val()); var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
            _script += "if (dateint > _dtStart && dateint < _dtEnd) { _controls += '<span class=\"rntCal sel_f\"></span>'; }";
            _script += "if (dateint == _dtStart) { _controls += '<span class=\"rntCal sel_1\"></span>'; }";
            _script += "if (dateint == _dtEnd) { _controls += '<span class=\"rntCal sel_2\"></span>'; }";
            _script += "if (_controls.indexOf('<span class=\"rntCal nd_2\"></span>') != -1 && _controls.indexOf('<span class=\"rntCal nd_1\"></span>') != -1) { _enabled = false; }";
            _script += "return [_enabled, _class, _tooltip, _controls];";
            _script += "}";
            ltr_checkCalDates.Text = _script;
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "_JSCal_Range_" + Unique, "var _JSCal_Range_" + Unique + " = new JSCal.Range();", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "checkCalDates_" + Unique, _script, true);
        }

        protected void lnk_search_Click(object sender, EventArgs e)
        {
            clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
            clSearch _ls = _config.lastSearch;

            DateTime TMPdtStart = _ls.dtStart;
            DateTime TMPdtEnd = _ls.dtEnd;
            int TMPnumPersCount = _ls.numPersCount;

            _ls.dtStart = HF_dtStart.Value.JSCal_stringToDate();
            _ls.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            _ls.dtCount = (_ls.dtEnd - _ls.dtStart).TotalDays.objToInt32();

            _ls.numPers_adult = drp_numPers_adult.SelectedValue.ToInt32();
            _ls.numPers_childOver = drp_numPers_childOver.SelectedValue.ToInt32();
            _ls.numPersCount = _ls.numPers_adult + _ls.numPers_childOver;
            _ls.numPers_childMin = drp_numPers_childMin.SelectedValue.ToInt32();

            //_ls.currType = drp_tipoAccom.SelectedValue.ToInt32();


            _ls.prMin = 0;
            _ls.prMax = 0;
            _ls.voteMin = 0;
            _ls.voteMax = 10;

            _ls.searchTitle = txt_title.Text;
            _ls.currZoneList = new List<int>() { drp_zone.getSelectedValueInt() };// chkList_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            _config.lastSearch = _ls;
            _config.dtLastUsed = DateTime.Now;
            clUtils.saveConfig(_config);
            Response.Redirect("/m" + CurrentSource.getPagePath("6", "stp", CurrentLang.ID.ToString()));
        }
    }
}