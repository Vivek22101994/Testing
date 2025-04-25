using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.ucMain
{
    public partial class uc_Search : System.Web.UI.UserControl
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
            //magaLocation_DataContext DC = new magaLocation_DataContext();
            checkReservationsCal();

            if (!IsPostBack)
            {

                checkReservationsCal();

                // carica ultima ricerca oppure nuova predefinita
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                clSearch _ls = _config.lastSearch;

                // imposta le date
                HF_dtStart.Value = _ls.dtStart.JSCal_dateToString();
                HF_dtEnd.Value = _ls.dtEnd.JSCal_dateToString();

                // imposta le persone

                int _maxAdult = AppSettings.RNT_TB_ESTATE.Where(x => x.num_persons_max.HasValue).Max(x => x.num_persons_max.Value);
                int _maxChildMin = AppSettings.RNT_TB_ESTATE.Where(x => x.num_persons_child.HasValue).Max(x => x.num_persons_child.Value);
                for (int i = 1; i <= _maxAdult; i++)
                {
                    drp_numPers_adult.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("reqAdults"), "" + i));
                }
                drp_numPers_adult.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("reqAdults"), "0"));
                drp_numPers_adult.setSelectedValue(_ls.numPers_adult.ToString());


                for (int i = 1; i <= (_maxAdult - 2); i++)
                {
                    drp_numPers_children.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("lblChildren3OrOver"), "" + i));
                }
                drp_numPers_children.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblChildren3OrOver"), "0"));
                drp_numPers_children.setSelectedValue(_ls.numPers_childOver.ToString());


                for (int i = 1; i <= _maxChildMin; i++)
                {
                    drp_numPers_infants.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("lblChildrenUnder3"), "" + i));
                }
                drp_numPers_infants.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblChildrenUnder3"), "0"));
                drp_numPers_infants.setSelectedValue(_ls.numPers_childMin.ToString());

                drp_Zone_DataBind();
                if (_ls.currZoneList.Count > 0)
                {
                    drp_zone.setSelectedValue(_ls.currZoneList[0]);
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal_" + Unique + "(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);
        }

        protected void drp_Zone_DataBind()
        {
            drp_zone.DataSource = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.pid_lang == CurrentLang.ID && AppSettings.RNT_activeZones.Contains(x.id)).OrderBy(x => x.title).ToList();
            drp_zone.DataTextField = "title";
            drp_zone.DataValueField = "id";
            drp_zone.DataBind();
            drp_zone.Items.Insert(0, "");
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
            int selNum_child_over = drp_numPers_children.getSelectedValueInt(0).objToInt32();
            drp_numPers_children.Items.Clear();
            int _minChildOver = min - selNum_adult;
            drp_numPers_children.Items.Insert(0, "");
            if (_minChildOver <= 0)
            {
                _minChildOver = 1;
            }
            for (int i = _minChildOver; i <= (max - selNum_adult); i++)
            {
                drp_numPers_children.Items.Add(new ListItem("" + i, "" + i));
            }
            if (selNum_child_over > (max - selNum_adult)) selNum_child_over = (max - selNum_adult);
            drp_numPers_children.setSelectedValue("" + selNum_child_over);
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
            _ls.numPers_adult = drp_numPers_adult.getSelectedValueInt();
            _ls.numPers_childOver = drp_numPers_children.getSelectedValueInt();
            _ls.numPers_childMin = drp_numPers_infants.getSelectedValueInt();
            _ls.numPersCount = _ls.numPers_adult + _ls.numPers_childOver;

            if (TMPdtStart != _ls.dtStart || TMPdtEnd != _ls.dtEnd || TMPnumPersCount != _ls.numPersCount)
            {
                _ls.prMin = 0;
                _ls.prMax = 9999999;
                _ls.voteMinNew = 0;
                _ls.voteMaxNew = 0;
            }

            _ls.searchTitle = "";
            _ls.currZoneList = new List<int>();
            if (drp_zone.getSelectedValueInt() > 0)
                _ls.currZoneList = new List<int>() { drp_zone.getSelectedValueInt() };

            _ls.currConfigList = new List<int>();

            _ls.orderBy = "";
            _ls.orderHow = "";
            _ls.currPage = 1;
            _config.lastSearch = _ls;
            _config.dtLastUsed = DateTime.Now;
            clUtils.saveConfig(_config);
            Response.Redirect(CurrentSource.getPagePath(HF_pidStp.Value, "stp", App.LangID + ""));
        }
    }
}