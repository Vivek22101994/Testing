using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_usr_tbl_admin_availability_view : System.Web.UI.UserControl
    {
        protected List<DateTime> _dates_;
        protected List<DateTime> _dates
        {
            get
            {
                if (_dates_ == null)
                    if (ViewState["_dates_"] != null)
                    {
                        _dates_ =
                            PConv.DeserArrToList((object[])ViewState["_dates_"],
                                                 typeof(DateTime)).Cast<DateTime>().ToList();
                    }
                    else
                        _dates_ = new List<DateTime>();

                return _dates_;
            }
            set
            {
                ViewState["_dates_"] = PConv.SerialList(value.Cast<object>().ToList());
                _dates_ = value;
            }
        }
        public int IdAdmin
        {
            get
            {
                int _id;
                if (int.TryParse(HF_pid_admin.Value, out _id))
                    return _id;
                return 0;
            }
            set
            {
                HF_pid_admin.Value = value.ToString();
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
            if (!IsPostBack)
            {
                if(IdAdmin==0)
                {
                    ltr_filter_users.Text = "is_active==1 and is_deleted!=1 and id > 2";
                    pnl_flt_admins.Visible = true;
                }
                else
                {
                    ltr_filter_users.Text = "id=="+IdAdmin;
                    pnl_flt_admins.Visible = false;
                }
                Bind_chkList_admins();
                Bind_rbl_month();
                List<DateTime> _newDates = new List<DateTime>();
                DateTime _dt = DateTime.Now.AddDays(-30);
                HF_date_from.Value = _dt.JSCal_dateToString();
                HF_date_to.Value = DateTime.Now.JSCal_dateToString();
                while (_dt <= DateTime.Now)
                {
                    _newDates.Add(_dt);
                    _dt = _dt.AddDays(1);
                }
                _dates = _newDates;
                rbl_mode.SelectedValue = "1";
                showFilter();
            }
            LDS_admins.Where = ltr_filter_users.Text;
            RegisterScripts();
        }
        protected void Bind_chkList_admins()
        {
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_deleted != 1 && x.is_active == 1 && x.id > 2).ToList();
            chkList_admins.Items.Clear();
            foreach (USR_ADMIN _utenti in _list)
            {
                ListItem _item = new ListItem("" + _utenti.name + " " + _utenti.surname, "" + _utenti.id);
                _item.Selected = true;
                chkList_admins.Items.Add(_item);
            }
        }
        protected void Bind_rbl_month()
        {
            rbl_month.Items.Clear();
            int _year = DateTime.Now.Year;
            int _month = DateTime.Now.Month;
            for(int i = 0; i > -5;i--)
            {
                if(_month==0)
                {
                    _month = 12;
                    _year--;
                }
                DateTime _dt = new DateTime(_year, _month, 1);
                ListItem _item = new ListItem(_dt.getMonthITA(false) + " " + _year, _dt.JSCal_dateToString());
                if (i == 0)
                    _item.Selected = true;
                rbl_month.Items.Add(_item);
                _month--;
            }
        }
        protected void LV_admins_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            ListView LV = e.Item.FindControl("LV") as ListView;
            LV.DataSource = _dates;
            LV.DataBind();
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_date = e.Item.FindControl("lbl_date") as Label;
            Label lbl_stato = e.Item.FindControl("lbl_stato") as Label;
            ListView LV = e.Item.Parent.Parent as ListView;
            ListViewItem _item = LV.Parent as ListViewItem;
            Label lbl_id_utente = _item.FindControl("lbl_id") as Label;
            DateTime _date = lbl_date.Text.JSCal_stringToDate();
            USR_TBL_ADMIN_AVAILABILITY _presenza = maga_DataContext.DC_USER.USR_TBL_ADMIN_AVAILABILITies.SingleOrDefault(x => x.date_availability == _date && x.pid_admin == lbl_id_utente.Text.ToInt32());
            if (_presenza != null)
            {
                lbl_stato.Text = AdminUtilities.usr_adminAvailabilityTitle(_presenza.pid_availability.Value);
            }
            lbl_stato.Text = lbl_stato.Text != "" ? lbl_stato.Text : ltr_empty.Text;
            System.Web.UI.HtmlControls.HtmlTableCell td_cont = e.Item.FindControl("td_cont") as System.Web.UI.HtmlControls.HtmlTableCell;
            if (td_cont != null) td_cont.Attributes.Add("class", "state_" + lbl_stato.Text.ToLower());
        }
        protected void LV_date_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_date = e.Item.FindControl("lbl_date") as Label;
            Label lbl_stato = e.Item.FindControl("lbl_stato") as Label;
            DateTime _date = lbl_date.Text.JSCal_stringToDate();
            lbl_stato.Text = _date.Day + "/" + _date.Month + "<br/>\r\n" + _date.getDayOfWeekITA(true);
        }

        protected void LV_admins_DataBound(object sender, EventArgs e)
        {
            ListView LV_date = LV_admins.FindControl("LV_date") as ListView;
            if (LV_date == null) return;
            LV_date.DataSource = _dates;
            LV_date.DataBind();
        }

        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            RefreshList();
        }
        public void RefreshList()
        {
            List<DateTime> _newDates = new List<DateTime>();
            DateTime _dt = DateTime.Now.AddDays(-30);
            DateTime _dtTo = DateTime.Now;
            if (rbl_mode.SelectedValue == "1" || rbl_month.Items.Count==0)
            {
                if (HF_date_from.Value != "" && HF_date_to.Value != "")
                {
                    _dt = HF_date_from.Value.JSCal_stringToDate();
                    _dtTo = HF_date_to.Value.JSCal_stringToDate();
                }
                else if (HF_date_to.Value != "")
                {
                    _dtTo = HF_date_to.Value.JSCal_stringToDate();
                    _dt = _dtTo.AddDays(-30);
                }
                else if (HF_date_from.Value != "")
                {
                    _dt = HF_date_from.Value.JSCal_stringToDate();
                    _dtTo = _dt.AddDays(30);
                }
            }
            else
            {
                _dt = rbl_month.SelectedValue.JSCal_stringToDate();
                _dtTo = _dt.AddDays(DateTime.DaysInMonth(_dt.Year, _dt.Month) - 1);
            }
            while (_dt <= _dtTo)
            {
                _newDates.Add(_dt);
                _dt = _dt.AddDays(1);
            }
            _dates = _newDates;
            string sql = "";
            string sql_hotel = "";
            string divisor = "";
            foreach (ListItem _item in chkList_admins.Items)
            {
                if (_item.Selected)
                {
                    sql += " " + divisor + " id = " + _item.Value;
                    divisor = "or";
                }
            }
            if (IdAdmin == 0)
            {
                ltr_filter_users.Text = sql != "" ? sql : "1==2";
            }
            else
            {
                ltr_filter_users.Text = "id==" + IdAdmin;
            }
            LDS_admins.Where = ltr_filter_users.Text;
            LV_admins.DataBind();
        }
        protected void lnk_stamp_Click(object sender, EventArgs e)
        {
            List<DateTime> _newDates = new List<DateTime>();
            DateTime _dt = DateTime.Now.AddDays(-30);
            DateTime _dtTo = DateTime.Now;
            if (HF_date_from.Value != "" && HF_date_to.Value != "")
            {
                _dt = HF_date_from.Value.JSCal_stringToDate();
                _dtTo = HF_date_to.Value.JSCal_stringToDate();
            }
            else if (HF_date_to.Value != "")
            {
                _dtTo = HF_date_to.Value.JSCal_stringToDate();
                _dt = _dtTo.AddDays(-30);
            }
            else if (HF_date_from.Value != "")
            {
                _dt = HF_date_from.Value.JSCal_stringToDate();
                _dtTo = _dt.AddDays(30);
            }
            string _redirect = "stampa_presenze.aspx";
            _redirect += "?from=" + _dt.JSCal_dateToString() + "&to=" + _dtTo.JSCal_dateToString();
            string sql = "";
            string divisor = "";
            foreach (ListItem _item in chkList_admins.Items)
            {
                if (_item.Selected)
                {
                    sql += "" + divisor + "" + _item.Value;
                    divisor = ";";
                }
            }
            _redirect += "&users=" + sql;
            Response.Redirect(_redirect);
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cal_dt_from", "cal_dt_from = new JSCal.Single({ dtFormat: \"d MM yy\", Cont: \"#" + HF_date_from.ClientID + "\", View: \"#cal_date_from\", changeMonth: true, changeYear: true });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cal_dt_to", "cal_dt_to = new JSCal.Single({ dtFormat: \"d MM yy\", Cont: \"#" + HF_date_to.ClientID + "\", View: \"#cal_date_to\", changeMonth: true, changeYear: true });", true);
        }
        protected void showFilter()
        {
            pnl_flt_period.Visible = rbl_mode.SelectedValue == "1";
            pnl_flt_month.Visible = rbl_mode.SelectedValue == "2";
        }

        protected void rbl_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            showFilter();
        }

        protected void chk_admins_all_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in chkList_admins.Items)
            {
                item.Selected = chk_admins_all.Checked;
            }
            chk_admins_all.CssClass = "";
        }

        protected void chkList_admins_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in chkList_admins.Items)
            {
                if(chk_admins_all.Checked && !item.Selected)
                {
                    chk_admins_all.CssClass = "fade";
                    return;
                }
                if (!chk_admins_all.Checked && item.Selected)
                {
                    chk_admins_all.CssClass = "fade";
                    return;
                }
            }
            chk_admins_all.CssClass = "";
        }
    }
}
