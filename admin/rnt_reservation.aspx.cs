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
    //        if (_item == null)
    //        {
    //            _item = new Tmp_ResListItem();
    //            _item._date = _date;
    //            _item._resList = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 && x.date_start.HasValue && x.date_end.HasValue).ToList();
    //            _resListItem.Add(_item);
    //        }
    //        return _item;
    //    }
    //}
    public partial class rnt_reservation : adminBasePage
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
                HF_dtStart.Value = _dtStart.JSCal_dateToString();
                HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
                Bind_drp_flt_city();
                Bind_drp_flt_state();
                Bind_drp_min_num_persons_max();
                Bind_drp_min_num_rooms_bed();
                Bind_drp_min_importance_stars();
                Bind_configList();
            }
            RegisterScripts();
        }
        private void Bind_drp_flt_city()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_flt_city.Items.Clear();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_flt_city.Items.Add(new ListItem("" + t.title, "" + t.id));
            }
            Bind_lbx_flt_zone();
        }
        protected void drp_flt_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_zone();
        }
        protected void Bind_lbx_flt_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1 && x.pid_city == drp_flt_city.getSelectedValueInt(0)).OrderBy(x => x.title).ToList();
            lbx_flt_zone.DataSource = _list;
            lbx_flt_zone.DataTextField = "title";
            lbx_flt_zone.DataValueField = "id";
            lbx_flt_zone.DataBind();
            Bind_lbx_flt_estate();
        }
        protected void lbx_flt_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_estate();
        }
        private void Bind_lbx_flt_estate()
        {
            List<int> _zoneIds = lbx_flt_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            List<RNT_TB_ESTATE> _list = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.pid_zone.HasValue && x.is_active == 1 && x.is_deleted != 1 && x.pid_city == drp_flt_city.getSelectedValueInt(0) && (_zoneIds.Count == 0 || _zoneIds.Contains(x.pid_zone.Value))).OrderBy(x => x.code).ToList();
            lbx_flt_estate.DataSource = _list;
            lbx_flt_estate.DataTextField = "code";
            lbx_flt_estate.DataValueField = "id";
            lbx_flt_estate.DataBind();
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
        protected void Bind_configList()
        {
            List<string> _currConfig = HF_currConfig.Value.splitStringToList("|");
            string _currConfigStr = "";
            string _currConfigSep = "";
            string _activeConfigsStr = "";
            string _activeConfigsSep = "";
            string _controlStr = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"listcheck\" style=\"margin: 0 10px 5px 0;\" >";
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
            HF_currConfig.Value = _currConfigStr;
            ltr_activeConfigsControl.Text = _controlStr;
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setCal_" + Unique, "setCal_" + Unique + "();", true);
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
