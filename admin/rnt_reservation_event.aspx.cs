using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_reservation_event : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_reservation_event";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected List<int> EDIT_ITEMS
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_rnt_reservation_event_EDIT_ITEMS"] != null)
                {
                    return (List<int>)HttpContext.Current.Session["CURRENT_rnt_reservation_event_EDIT_ITEMS"];
                }
                return new List<int>();
            }
            set
            {
                HttpContext.Current.Session["CURRENT_rnt_reservation_event_EDIT_ITEMS"] = value;
            }
        }
        protected string CURRENT_FILTER
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_rnt_reservation_event_FILTER"] != null)
                {
                    return (string)HttpContext.Current.Session["CURRENT_rnt_reservation_event_FILTER"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["CURRENT_rnt_reservation_event_FILTER"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                PH_admin.Visible = UserAuthentication.CurrentUserID == 1;
                if (Request.QueryString.ToString() != "")
                {
                    CURRENT_FILTER = Request.QueryString.ToString();
                }
                else if (CURRENT_FILTER != "")
                {
                    Response.Redirect("rnt_reservation_event.aspx?" + CURRENT_FILTER);
                }
                EDIT_ITEMS = new List<int>();
                REQUEST_LIST = DC_RENTAL.RNT_TBL_REQUEST.ToList();
                Bind_drp_estate();
                LDS.Where = "1=2";
                string _ref = "";
                Uri referrer = HttpContext.Current.Request.Url;
                if (referrer != null)
                {
                    _ref = referrer.OriginalString.ToLower();
                }
                if (_ref.Contains("flt=true"))
                {
                    HF_url_filter.Value = _ref;
                    SetValuesFromSearch();
                }
                else
                {
                    HF_dtEvent_from.Value = DateTime.Now.AddDays(1).JSCal_dateToString();
                    HF_dtEvent_to.Value = DateTime.Now.AddDays(2).JSCal_dateToString();
                }
            }
            else
            {
                if (Request["__EVENTARGUMENT"] == "load_content")
                {
                    LoadContent();
                    UC_loader_list1.Visible = false;
                    LV.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setCal", "setCal();", true);
        }

        protected List<USR_ADMIN> _adminList;
        protected List<temp_admin_count> _adminCountList;
        public class temp_admin_count
        {

            private int _id;
            private int _count;
            private int _isAvv;
            public temp_admin_count()
            {
                this._id = 0;
                this._count = 0;
                this._isAvv = 0;
            }
            public temp_admin_count(int Id, int Count, int IsAvv)
            {
                this._id = Id;
                this._count = Count;
                this._isAvv = IsAvv;
            }
            public int Id { get { if (this._id != null)return this._id; else return 0; } set { this._id = value; } }
            public int Count { get { if (this._count != null)return this._count; else return 0; } set { this._count = value; } }
            public int IsAvv { get { if (this._isAvv != null)return this._isAvv; else return 0; } set { this._isAvv = value; } }
        }
        private void Bind_drp_estate()
        {
            List<RNT_TB_ESTATE> list = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_srs==1 || x.is_ecopulizie==1).OrderBy(x => x.code).ToList();
            drp_estate.Items.Clear();
            foreach (RNT_TB_ESTATE t in list)
            {
                drp_estate.Items.Add(new ListItem("" + t.code, "" + t.id));
            }
            drp_estate.Items.Insert(0, new ListItem("- tutti -", "0"));
        }
        protected void SetValuesFromSearch()
        {
            string _str = HF_url_filter.Value;
            if (_str == "") return;
            string _qStr = _str.Split('?')[1];
            string[] _strArr = _qStr.Split('&');
            for (int i = 0; i < _strArr.Length; i++)
            {
                if (_strArr[i].Split('=')[0] == "is_del") drp_is_deleted.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "event") drp_event.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "pidest") drp_estate.setSelectedValue(Server.UrlDecode(_strArr[i].Split('=')[1].ToLower()));
                if (_strArr[i].Split('=')[0] == "tith") txt_code.Text = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "name_full") txt_name_full.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "code") txt_code.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "email") txt_email.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);

                if (_strArr[i].Split('=')[0] == "dtsf") HF_dtEvent_from.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "dtst") HF_dtEvent_to.Value = _strArr[i].Split('=')[1];

                if (_strArr[i].Split('=')[0] == "is_exclusive") drp_is_exclusive.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_srs") drp_is_srs.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_ecopulizie") drp_is_ecopulizie.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_online_booking") drp_is_online_booking.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_homeaway") drp_homeAway.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "overnight_tax") drp_pr_has_overnight_tax.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "iscontratto") drp_isContratto.setSelectedValue(_strArr[i].Split('=')[1]);
            }
        }
        protected void LoadContent()
        {
            string _filter = "";
            string _sep = "";

            if (UserAuthentication.CurrentUserID == 1)
            {
                if (drp_is_deleted.SelectedValue != "")
                {
                    _filter += _sep + "is_deleted = " + drp_is_deleted.SelectedValue + "";
                    _sep = " and ";
                }
            }
            else
            {
                _filter += _sep + "is_deleted != 1";
                _sep = " and ";
            }
            if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedPlannerCheckinCheckout.objToInt32() == 1)
            {
                _filter += _sep + "pid_operator = " + UserAuthentication.CurrentUserID + "";
                _sep = " and ";
            }
            if (drp_event.SelectedValue != "0" && drp_event.SelectedValue != "")
            {
                _filter += _sep + "direction = \"" + drp_event.SelectedValue + "\"";
                _sep = " and ";
            }
            if (drp_estate.SelectedValue != "0" && drp_estate.SelectedValue != "")
            {
                _filter += _sep + "pid_estate = " + drp_estate.SelectedValue + "";
                _sep = " and ";
            }
            if (txt_name_full.Text.Trim() != "")
            {
                _filter += _sep + "cl_name_full.Contains(\"" + txt_name_full.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_code.Text.Trim() != "")
            {
                _filter += _sep + "code.Contains(\"" + txt_code.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_email.Text.Trim() != "")
            {
                _filter += _sep + "cl_email.Contains(\"" + txt_email.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (HF_dtEvent_from.Value != "")
            {
                _filter += _sep + "dtEvent >= DateTime.Parse(\"" + HF_dtEvent_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_dtEvent_to.Value != "")
            {
                _filter += _sep + "dtEvent < DateTime.Parse(\"" + HF_dtEvent_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }


            if (drp_is_exclusive.SelectedValue != "-1")
            {
                _filter += _sep + "est_is_exclusive = " + drp_is_exclusive.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_is_srs.SelectedValue != "-1")
            {
                _filter += _sep + "is_srs = " + drp_is_srs.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_is_ecopulizie.SelectedValue != "-1")
            {
                _filter += _sep + "is_ecopulizie = " + drp_is_ecopulizie.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_is_online_booking.SelectedValue != "-1")
            {
                _filter += _sep + "est_is_online_booking = " + drp_is_online_booking.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_pr_has_overnight_tax.SelectedValue != "-1")
            {
                _filter += _sep + "est_pr_has_overnight_tax = " + drp_pr_has_overnight_tax.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_isContratto.SelectedValue != "-1")
            {
                _filter += _sep + "est_isContratto = " + drp_isContratto.SelectedValue + "";
                _sep = " and ";
            }
            if (_filter == "") _filter = "1=1";
            HF_lds_filter.Value = _filter;
            Fill_LV();
        }
        protected void Fill_LV()
        {
            LDS.Where = HF_lds_filter.Value;
            LDS.DataBind();
            LV.DataBind();
        }

        protected void LV_DataBound(object sender, EventArgs e)
        {
            DataPager _dataPager = LV.FindControl("DataPager1") as DataPager;
            Label _lblCount = LV.FindControl("lbl_record_count") as Label;
            Label _lblCount_top = LV.FindControl("lbl_record_count_top") as Label;
            if (_dataPager != null && _lblCount != null && _lblCount_top != null)
                _lblCount.Text = _lblCount_top.Text = "Totale: " + _dataPager.TotalRowCount;
        }
        protected string GetUserName(string id)
        {
            string _name = CommonUtilities.GetUserName("" + Eval("pid_operator"));
            return _name != "" ? _name : " ";
        }
        public static List<RNT_TBL_REQUEST> REQUEST_LIST
        {
            get
            {
                if (HttpContext.Current.Cache["CURRENT_cont_request_list_RNT_TBL_REQUEST"] == null)
                    HttpContext.Current.Cache["CURRENT_cont_request_list_RNT_TBL_REQUEST"] = new List<RNT_TBL_REQUEST>();
                return (List<RNT_TBL_REQUEST>)HttpContext.Current.Cache["CURRENT_cont_request_list_RNT_TBL_REQUEST"];
            }
            set
            {
                HttpContext.Current.Cache["CURRENT_cont_request_list_RNT_TBL_REQUEST"] = value;
            }
        }
        protected void Update_allTime()
        {
            foreach (ListViewDataItem _item in LV.Items)
            {
                DropDownList drp_dtEventTime_h = _item.FindControl("drp_dtEventTime_h") as DropDownList;
                DropDownList drp_dtEventTime_m = _item.FindControl("drp_dtEventTime_m") as DropDownList;
                Label lbl_dtEventTime = _item.FindControl("lbl_dtEventTime") as Label;
                Label lbl_id = _item.FindControl("lbl_id") as Label;
                Label lbl_direction = _item.FindControl("lbl_direction") as Label;
                if (drp_dtEventTime_h == null || drp_dtEventTime_m == null || lbl_dtEventTime == null || lbl_id == null || lbl_direction == null) 
                    continue;
                TimeSpan _time = new TimeSpan(0, drp_dtEventTime_h.getSelectedValueInt(0).objToInt32(), drp_dtEventTime_m.getSelectedValueInt(0).objToInt32(),0);
                if (_time == lbl_dtEventTime.Text.JSTime_stringToTime())
                    continue;
                RNT_TBL_RESERVATION _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if(_currTBL==null)
                    continue;
                if (lbl_direction.Text.ToLower() == "in")
                {
                    DateTime dtIn = _currTBL.dtStart.Value.Date.Add(_time);
                    _currTBL.dtStartTime = dtIn.TimeOfDay.JSTime_timeToString();
                    _currTBL.dtIn = dtIn;
                    _currTBL.is_dtStartTimeChanged = 1;
                }
                if (lbl_direction.Text.ToLower() == "out")
                {
                    DateTime dtOut = _currTBL.dtEnd.Value.Date.Add(_time);
                    _currTBL.dtEndTime = dtOut.TimeOfDay.JSTime_timeToString();
                    _currTBL.dtOut = dtOut;
                    _currTBL.is_dtEndTimeChanged = 1;
                }
                DC_RENTAL.SubmitChanges();
                rntUtils.rntReservation_onChange(_currTBL);
            }
        }
        protected void lnk_updateAllTime_Click(object sender, EventArgs e)
        {
            Update_allTime();
            Fill_LV();
        }
        protected void drp_mailing_days_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }
        protected void chk_only_availables_CheckedChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DropDownList drp_dtEventTime_h = e.Item.FindControl("drp_dtEventTime_h") as DropDownList;
            DropDownList drp_dtEventTime_m = e.Item.FindControl("drp_dtEventTime_m") as DropDownList;
            Label lbl_dtEventTime = e.Item.FindControl("lbl_dtEventTime") as Label;
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_direction = e.Item.FindControl("lbl_direction") as Label;
            if (drp_dtEventTime_h == null || drp_dtEventTime_m == null || lbl_dtEventTime == null || lbl_id == null || lbl_direction == null) return;
            drp_dtEventTime_h.bind_Numbers(0,23,1,2);
            drp_dtEventTime_m.bind_Numbers(0, 59, 1, 2);
            TimeSpan _time = lbl_dtEventTime.Text.JSTime_stringToTime();
            drp_dtEventTime_h.setSelectedValue(_time.Hours.ToString());
            drp_dtEventTime_m.setSelectedValue(_time.Minutes.ToString());
        }
    }
}
