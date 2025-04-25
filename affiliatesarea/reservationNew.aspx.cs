using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.affiliatesarea
{
    public partial class reservationNew : agentBasePage
    {
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = string.Empty.createUniqueID();
                return HF_unique.Value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillData();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal();", true);
        }
        protected void fillData()
        {
            checkReservationsCal();
            // carica ultima ricerca oppure nuova predefinita
            clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
            clSearch _ls = _config.lastSearch;

            // imposta le date
            HF_dtStart.Value = HF_dtStartTmp.Value = _ls.dtStart.JSCal_dateToString();
            HF_dtEnd.Value = HF_dtEndTmp.Value = _ls.dtEnd.JSCal_dateToString();

            // imposta le persone
            int _maxAdult = AppSettings.RNT_TB_ESTATE.Where(x => x.num_persons_max.HasValue).Max(x => x.num_persons_max.Value);
            int _maxChildMin = AppSettings.RNT_TB_ESTATE.Where(x => x.num_persons_child.HasValue).Max(x => x.num_persons_child.Value);
            drp_adult.bind_Numbers(1, _maxAdult, 1, 0);
            drp_adult.setSelectedValue(_ls.numPers_adult.ToString());
            drp_child_over.bind_Numbers(1, (_maxAdult - 2), 1, 0);
            drp_child_over.Items.Insert(0, new ListItem("---", "0"));
            drp_child_over.setSelectedValue(_ls.numPers_childOver.ToString());
            drp_child_min.bind_Numbers(1, _maxChildMin, 1, 0);
            drp_child_min.Items.Insert(0, new ListItem("---", "0"));
            drp_child_min.setSelectedValue(_ls.numPers_childMin.ToString());

            // ricerca veloce
            HF_searchTitle.Value = txt_title.Text = _ls.searchTitle;

            HF_currZoneList.Value = _ls.currZoneList.listToString("|");
            HF_currConfigList.Value = _ls.currConfigList.listToString("|");

            // slider
            HF_prRangeTmp.Value = _ls.prMin + "|" + _ls.prMax;
            HF_voteRangeTmp.Value = _ls.voteMin + "|" + _ls.voteMax;

            HF_currPage.Value = _ls.currPage.ToString();
            // OrderByHow
            HF_orderBy.Value = _ls.orderBy;
            HF_orderHow.Value = _ls.orderHow;

            Bind_zoneList();
            Bind_configList();
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
        protected void Bind_zoneList()
        {
            List<string> _currZone = HF_currZoneList.Value.splitStringToList("|");
            string _currZoneStr = "";
            string _currZoneSep = "";
            string _activeZonesStr = "";
            string _activeZonesSep = "";
            string _controlStr = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"listcheck\">";
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.pid_lang == CurrentLang.ID && AppSettings.RNT_activeZones.Contains(x.id)).OrderBy(x => x.title).ToList();
            foreach (LOC_VIEW_ZONE _zone in _list)
            {
                string _checked = _currZone.Contains("" + _zone.id) ? " checked=\"checked\"" : "";
                if (_checked != "")
                {
                    _currZoneStr += _currZoneSep + "" + _zone.id;
                    _currZoneSep = "|";
                }
                _activeZonesStr += _activeZonesSep + "" + _zone.id;
                _activeZonesSep = "|";

                string _control_id = "chk_zone_" + _zone.id;

                _controlStr += "<tr>";
                _controlStr += "    <td>";
                _controlStr += "        <input type=\"checkbox\" class=\"chk_zone\" id=\"" + _control_id + "\" " + _checked + " onclick=\"RNT_currZone_onChange()\" />";
                _controlStr += "        <label for=\"" + _control_id + "\">" + _zone.title + "</label>";
                _controlStr += "    </td>";
                _controlStr += "</tr>";
            }
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
            string _controlStr = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"listcheck\">";
            List<RNT_VIEW_CONFIG> _list = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.Where(x => x.pid_lang == CurrentLang.ID && AppSettings._RNT_CUSTOM_CONFIGs.Contains(x.id)).OrderBy(x => x.title).ToList();
            foreach (RNT_VIEW_CONFIG _config in _list)
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

        protected void btnDummy_Click(object sender, EventArgs e)
        {
            var obj = this.Request.Params.Get("__EVENTARGUMENT");
            string mk= Convert.ToString(obj[1]);
            string url = Convert.ToString(obj[0]).Replace("mk=", "mk=" + mk);

            string fn = "pdfDownLoad('" + url + "')";
            ScriptManager.RegisterStartupScript(this, this.GetType(), new Guid().ToString(), fn, true);
        }
    }
}
