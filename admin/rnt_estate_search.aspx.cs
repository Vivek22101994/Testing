using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using RNT_estateDate = AppSettings.RNT_estateDate;
using RNT_dateItem = AppSettings.RNT_dateItem;
using ModRental;

namespace RentalInRome.admin
{
    public partial class rnt_estate_search : adminBasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                clConfig _config = clUtils.getConfig(Unique);
                clSearch _ls = _config.lastSearch;

                // imposta le date
                HF_dtStart.Value = _ls.dtStart.JSCal_dateToString();
                HF_dtEnd.Value = _ls.dtEnd.JSCal_dateToString();

                // imposta le persone
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

                drp_pidLang_DataBind();

                // ricerca veloce
                txt_searchTitle.Text = _ls.searchTitle;
                HF_searchTitle.Value = _ls.searchTitle;

                drp_city_DataBind();
                drp_city.setSelectedValue(_ls.currCity);
                HF_currZoneList.Value = _ls.currZoneList.listToString("|");
                chkList_zone_DataBind();
                chkList_zone.setSelectedValues(_ls.currZoneList.Select(x => "" + x).ToList());

                HF_currConfigList.Value = _ls.currConfigList.listToString("|");

                // slider
                HF_prRangeTmp.Value = _ls.prMin + "|" + _ls.prMax;
                HF_voteRangeTmp.Value = _ls.voteMin + "|" + _ls.voteMax;

                HF_currPage.Value = _ls.currPage.ToString();
                // OrderByHow
                HF_orderBy.Value = _ls.orderBy;
                HF_orderHow.Value = _ls.orderHow;

                Bind_configList();
                Bind_drp_min_num_rooms_bed();
                Bind_drp_min_num_rooms_bath();
                drp_pidAgent_DataBind();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setCal_" + Unique, "setCal_" + Unique + "();", true);
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
        protected void drp_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkList_zone_DataBind();
        }
        protected void Bind_drp_min_num_rooms_bed()
        {
            drp_min_num_rooms_bed.Items.Clear();
            drp_min_num_rooms_bed.Items.Add(new ListItem("--", "-1"));
            int _max = AppSettings.RNT_estateList.Max(x => x.num_rooms_bed);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_num_rooms_bed.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_min_num_rooms_bath()
        {
            drp_min_num_rooms_bath.Items.Clear();
            drp_min_num_rooms_bath.Items.Add(new ListItem("--", "-1"));
            int _max = AppSettings.RNT_estateList.Max(x => x.num_rooms_bath);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_num_rooms_bath.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void drp_pidAgent_DataBind()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntAgentTBL> _list = dc.dbRntAgentTBLs.Where(x => x.isActive == 1).ToList();
                if (UserAuthentication.CURRENT_USER_ROLE == 2)
                    _list = _list.Where(x => x.pidReferer == UserAuthentication.CurrentUserID).ToList();
                drp_pidAgent.Items.Clear();
                _list = _list.OrderBy(x => x.nameCompany).ThenBy(x => x.nameFull).ToList();
                foreach (dbRntAgentTBL _agent in _list)
                {
                    drp_pidAgent.Items.Add(new ListItem(_agent.nameCompany + " - " + _agent.nameFull, "" + _agent.id));
                }
                drp_pidAgent.Items.Insert(0, new ListItem("- non abbinata -", "0"));
            }
        }
        protected void drp_pidLang_DataBind()
        {
            drp_pidLang.DataSource = contProps.LangTBL.Where(x => x.is_active == 1 && x.is_public == 1).OrderBy(x => x.id);
            drp_pidLang.DataTextField = "title";
            drp_pidLang.DataValueField = "id";
            drp_pidLang.DataBind();
            drp_pidLang.setSelectedValue(2);
        }
        protected void drp_city_DataBind()
        {
            drp_city.DataSource = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == App.LangID).OrderBy(x => x.title).ToList();
            drp_city.DataTextField = "title";
            drp_city.DataValueField = "id";
            drp_city.DataBind();
            drp_city.Items.Insert(0, new ListItem("- " + contUtils.getLabel("lblAll") + " -", ""));
        }
        protected void chkList_zone_DataBind()
        {
            var zoneList = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == App.LangID && x.pid_city == drp_city.getSelectedValueInt(0)).OrderBy(x => x.title).ToList();
            List<string> _currZone = HF_currZoneList.Value.splitStringToList("|");
            string _currZoneStr = "";
            string _currZoneSep = "";
            string _activeZonesStr = "";
            string _activeZonesSep = "";
            string _controlStr = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"chk\" style=\"margin-top: 6px; margin-left: 10px;\">";
            _controlStr += "<tr>";
            int repeatColumns = 2;
            int currColumn = 1;
            foreach (var _zone in zoneList)
            {
                if (currColumn > repeatColumns)
                {
                    currColumn = 1;
                    _controlStr += "</tr>";
                    _controlStr += "<tr>";
                }
                string _checked = _currZone.Contains("" + _zone.id) ? " checked=\"checked\"" : "";
                if (_checked != "")
                {
                    _currZoneStr += _currZoneSep + "" + _zone.id;
                    _currZoneSep = "|";
                }
                _activeZonesStr += _activeZonesSep + "" + _zone.id;
                _activeZonesSep = "|";

                string _control_id = "chk_zone_" + _zone.id;

                _controlStr += "    <td>";
                _controlStr += "        <input type=\"checkbox\" class=\"chkList_zone_item\" id=\"" + _control_id + "\" value=\"" + _zone.id + "\" " + _checked + " />";
                _controlStr += "        <label for=\"" + _control_id + "\">" + _zone.title + "</label>";
                _controlStr += "    </td>";
                currColumn++;
            }
            _controlStr += "</tr>";
            _controlStr += "</table>";
            HF_activeZones.Value = _activeZonesStr;
            HF_currZoneList.Value = _currZoneStr != "" ? _currZoneStr : "0";
            ltr_activeZonesControl.Text = _controlStr;
        }
        protected void Bind_configList()
        {
            List<string> _currConfig = HF_currConfigList.Value.splitStringToList("|");
            string _currConfigStr = "";
            string _currConfigSep = "";
            string _activeConfigsStr = "";
            string _activeConfigsSep = "";
            string _controlStr = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"chk\" style=\"margin-left: 5px;\">";
            //var _list = rntProps.EstateExtrasVIEW.Where(x => x.pidLang == CurrentLang.ID && AppSettings._RNT_CUSTOM_CONFIGs.Contains(x.id)).OrderBy(x => x.title).ToList();
            var _list = DC_RENTAL.RNT_VIEW_CONFIGs.Where(x => x.pid_lang == CurrentLang.ID && x.inner_type == 1 && x.inner_category == 1 && x.is_important== 1).OrderBy(x => x.title).ToList();
            foreach (var _config in _list)
            {
                string _checked = _currConfig.Contains("" + _config.id) ? " checked=\"checked\"" : "";
                if (_checked != "")
                {
                    _currConfigStr += _currConfigSep + "" + _config.id;
                    _currConfigSep = "|";
                }
                _activeConfigsStr += _activeConfigsSep + "" + _config.id;
                _activeConfigsSep = "|";

                string _control_id = "chk_config_" + _config.id;

                _controlStr += "<tr>";
                _controlStr += "    <td>";
                _controlStr += "        <input type=\"checkbox\" id=\"" + _control_id + "\" " + _checked + " onclick=\"RNT_currConfig_onChange()\" />";
                _controlStr += "        <label for=\"" + _control_id + "\">" + _config.title + "</label>";
                _controlStr += "    </td>";
                _controlStr += "</tr>";
            }
            _controlStr += "</table>";
            HF_activeConfigs.Value = _activeConfigsStr;
            HF_currConfigList.Value = _currConfigStr;
            ltr_activeConfigsControl.Text = _controlStr;
        }

    }
}
