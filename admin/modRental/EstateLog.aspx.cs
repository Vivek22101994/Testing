using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class EstateLog : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind_drp_account();
                Bind_lbx_flt_zone();
                Bind_lbx_flt_estate();
                rdp_flt_createdDateFrom.SelectedDate = DateTime.Now.Date.AddDays(-3);
                Fill_LV();
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            Fill_LV();
        }
        protected void Bind_drp_account()
        {
            drp_account.Items.Clear();
            var _list = usrUtils.USR_ADMINs.Where(x => x.is_active == 1 && (x.rnt_canHaveReservation == 1) && x.is_deleted == 0).OrderBy(x => x.name).ToList();
            foreach (var _admin in _list)
            {
                drp_account.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
            drp_account.Items.Insert(0, new ListItem("- tutti -", "0"));
        }
        protected void drp_flt_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_zone();
        }
        protected void Bind_lbx_flt_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1).OrderBy(x => x.title).ToList();
            lbx_flt_zone.DataSource = _list;
            lbx_flt_zone.DataTextField = "title";
            lbx_flt_zone.DataValueField = "id";
            lbx_flt_zone.DataBind();
        }
        protected void lbx_flt_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_estate();
        }
        protected void drp_flt_isActiveEstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_estate();
        }
        private void Bind_lbx_flt_estate()
        {
            List<int> _zoneIds = lbx_flt_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            List<RNT_TB_ESTATE> _list = AppSettings.RNT_TB_ESTATE.Where(x => x.pid_zone.HasValue && x.is_deleted != 1 && _zoneIds.Contains(x.pid_zone.Value)).OrderBy(x => x.code).ToList();
            lbx_flt_estate.DataSource = _list;
            lbx_flt_estate.DataTextField = "code";
            lbx_flt_estate.DataValueField = "id";
            lbx_flt_estate.DataBind();
        }
        protected void Fill_LV()
        {
            setfilters();
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        protected void setfilters()
        {
            string _filter = "";
            string _sep = "";

            List<int> _estateIds;
            List<int> _zoneIds;
            
            _estateIds = lbx_flt_estate.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            if (_estateIds.Count != 0)
            {
                _filter += _sep + "(";
                _sep = "";
                foreach (int _id in _estateIds)
                {
                    _filter += _sep + "estateID = " + _id + "";
                    _sep = " or ";
                }
                _filter += ")";
                _sep = " and ";
            }
            else
            {
                _zoneIds = lbx_flt_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
                _estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.pid_zone.HasValue && x.is_deleted != 1 && _zoneIds.Contains(x.pid_zone.Value)).Select(x => x.id).ToList();
                if (_zoneIds.Count != 0 && _estateIds.Count != 0)
                {
                    _filter += _sep + "(";
                    _sep = "";
                    foreach (int _id in _estateIds)
                    {
                        _filter += _sep + "estateID = " + _id + "";
                        _sep = " or ";
                    }
                    _filter += ")";
                    _sep = " and ";
                }
            }
            if (rdp_flt_createdDateFrom.SelectedDate.HasValue)
            {
                _filter += _sep + "logDate >= DateTime.Parse(\"" + rdp_flt_createdDateFrom.SelectedDate + "\")";
                _sep = " and ";
            }
            if (rdp_flt_createdDateTo.SelectedDate.HasValue)
            {
                _filter += _sep + "logDate < DateTime.Parse(\"" + rdp_flt_createdDateTo.SelectedDate + "\")";
                _sep = " and ";
            }
            
            if (drp_account.SelectedValue.ToInt32() != 0)
            {
                _filter += _sep + "userID = " + drp_account.SelectedValue + "";
                _sep = " and ";
            }
            if (txt_flt_changeField.Text.Trim() != "")
            {
                _filter += _sep + "changeField = \"" + txt_flt_changeField.Text.Trim() + "\"";
                _sep = " and ";
            }
            _filter += _sep + "1 = 1";
            ltrLDSfiltter.Text = _filter;
        }
        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            Label lbl_id = LV.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            if (lbl_id != null)
            {
            }
            Fill_LV();
            LV.SelectedIndex = e.NewSelectedIndex;
        }
        protected void drp_logList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }
        protected void lnk_flt_Click(object sender, EventArgs e)
        {
            Fill_LV();
        }

    }
}
