using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.WLRental.uc
{
    public partial class UC_search : System.Web.UI.UserControl
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
                mainBasePage m = (mainBasePage)this.Page;
                return m.CURRENT_SESSION_ID;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind_drp_num_persons_max();
                Bind_drp_zone();
                checkReservationsCal();

                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                clSearch _ls = _config.lastSearch;

                // imposta le date
                HF_dtStart.Value = _ls.dtStart.JSCal_dateToString();
                HF_dtEnd.Value = _ls.dtEnd.JSCal_dateToString();

                // imposta le persone
                drp_num_persons_max.setSelectedValue((_ls.numPers_adult + _ls.numPers_childOver).ToString());

                // ricerca veloce
                txt_title.Text = _ls.searchTitle;

                if (_ls.currZoneList.Count > 0) drp_zone.setSelectedValue(_ls.currZoneList[0].ToString());
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal_" + Unique + "(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);

        }
        protected void Bind_drp_num_persons_max()
        {
            drp_num_persons_max.Items.Clear();
            int _max = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.num_persons_max.HasValue).Max(x => x.num_persons_max.Value);
            for (int i = 1; i <= _max; i++)
            {
                drp_num_persons_max.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.pid_lang == CurrentLang.ID && AppSettings.RNT_activeZones.Contains(x.id)).OrderBy(x => x.title).ToList();
            drp_zone.DataSource = _list;
            drp_zone.DataValueField = "id";
            drp_zone.DataTextField = "title";
            drp_zone.DataBind();
            drp_zone.Items.Insert(0, new ListItem(" - - - ", "0"));
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

            int _persCount = drp_num_persons_max.getSelectedValueInt(0).objToInt32();
            _ls.numPersCount = _persCount;
            _ls.numPers_adult = _persCount - _ls.numPers_childOver;

            if (TMPdtStart != _ls.dtStart || TMPdtEnd != _ls.dtEnd || TMPnumPersCount != _ls.numPersCount)
            {
                _ls.prMin = 0;
                _ls.prMax = 9999999;
                _ls.voteMin = 0;
                _ls.voteMax = 10;
            }

            if (HF_estatePath.Value != "" && HF_estateId.Value.ToInt32() != 0)
            {

                _config.lastSearch = _ls;
                _config.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(_config);
                Response.Redirect(HF_estatePath.Value + "?bts=true");
                return;
            }
            _ls.searchTitle = txt_title.Text;
            int _currZone = drp_zone.getSelectedValueInt(0).objToInt32();
            if (_currZone != 0)
                _ls.currZoneList.Add(_currZone);
            _config.lastSearch = _ls;
            _config.dtLastUsed = DateTime.Now;
            clUtils.saveConfig(_config);
            Response.Redirect(CurrentSource.getPagePath("6", "stp", CurrentLang.ID.ToString()));
        }
    }
}