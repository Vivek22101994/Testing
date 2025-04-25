using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.mobile
{
    public partial class rnt_reservationEventTime : System.Web.UI.Page
    {
        public long IdReservation
        {
            get { return HF_id.Value.ToInt32(); }
            set { HF_id.Value = value.ToString(); }
        }
        private magaRental_DataContext DC_RENTAL;
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                if (Request.QueryString["uid"] != null)
                {
                    Uri referrer = HttpContext.Current.Request.UrlReferrer;
                    if (referrer != null)
                    {
                        HL_back.NavigateUrl = referrer.OriginalString.ToLower();
                        HL_back.Visible = true;
                    }
                    else
                        HL_back.Visible = false;
                    if (Request.QueryString["iFrame"]=="true")
                        HL_back.Visible = false;
                    //Guid _unique = new Guid(Request.QueryString["uid"]);
                    //_currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.uid_2 == _unique);
                    HF_inOut.Value = Request.QueryString["evt"] == "in" ? "in" : "out";
                    _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == Request.QueryString["id"].objToInt64());
                    if (_currTBL != null && Request.QueryString["uid"] == "cfab20dd-3b2c-4e57-ba76-597cfa8f450e")
                    {
                        IdReservation = _currTBL.id;
                        drp_time_h.bind_Numbers(0, 23, 1, 2);
                        drp_time_m.bind_Numbers(0, 59, 1, 2);
                        fillData();
                        return;
                    }
                }
                Response.Clear();
                Response.End();
                return;
            }
        }
        public void fillData()
        {
            HF_code.Value = _currTBL.code;
            ltr_date.Text = "dal: " + _currTBL.dtStart.formatCustom("#dd# #MM# #yy#, #DD#", 1, "--/--/--") + "<br/>al: " + _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#, #DD#", 1, "--/--/--");
            if (HF_inOut.Value == "out")
            {
                DateTime dtEnd = _currTBL.dtEnd.Value;
                if (!_currTBL.dtOut.HasValue && !string.IsNullOrEmpty(_currTBL.dtEndTime))
                {
                    _currTBL.dtOut = dtEnd.Add(_currTBL.dtEndTime.JSTime_stringToTime());
                }
                drp_date.Items.Clear();
                for (int day = 0; day < 3; day++)
                    drp_date.Items.Add(new ListItem(dtEnd.AddDays(-day).formatCustom("#dd# #MM# #yy#", 1, ""), dtEnd.AddDays(-day).JSCal_dateToString()));
                drp_date.setSelectedValue(_currTBL.dtOut.HasValue ? _currTBL.dtOut.JSCal_dateToString() : dtEnd.JSCal_dateToString());
                drp_time_h.setSelectedValue(_currTBL.dtOut.Value.TimeOfDay.Hours.ToString());
                drp_time_m.setSelectedValue(_currTBL.dtOut.Value.TimeOfDay.Minutes.ToString());
            }
            else
            {
                DateTime dtStart = _currTBL.dtStart.Value;
                if (!_currTBL.dtIn.HasValue && !string.IsNullOrEmpty(_currTBL.dtStartTime))
                {
                    _currTBL.dtIn = dtStart.Add(_currTBL.dtStartTime.JSTime_stringToTime());
                }
                drp_date.Items.Clear();
                for (int day = 0; day < 3; day++)
                    drp_date.Items.Add(new ListItem(dtStart.AddDays(day).formatCustom("#dd# #MM# #yy#", 1, ""), dtStart.AddDays(day).JSCal_dateToString()));
                drp_date.setSelectedValue(_currTBL.dtIn.HasValue ? _currTBL.dtIn.JSCal_dateToString() : dtStart.JSCal_dateToString());
                drp_time_h.setSelectedValue(_currTBL.dtIn.Value.TimeOfDay.Hours.ToString());
                drp_time_m.setSelectedValue(_currTBL.dtIn.Value.TimeOfDay.Minutes.ToString());
            }
            txt_pr_depositNotes.Text = _currTBL.pr_depositNotes;
        }
        public void saveData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null)
            {
                Response.Clear();
                Response.End();
                return;
            }
            if (HF_inOut.Value == "out")
            {
                DateTime dtOut = drp_date.SelectedValue.JSCal_stringToDate();
                TimeSpan time = new TimeSpan(0, drp_time_h.getSelectedValueInt(0).objToInt32(), drp_time_m.getSelectedValueInt(0).objToInt32(), 0);
                dtOut = dtOut.Add(time);
                _currTBL.dtEndTime = dtOut.TimeOfDay.JSTime_timeToString();
                _currTBL.dtOut = dtOut;
                _currTBL.is_dtEndTimeChanged = 1;
            }
            else
            {
                DateTime dtIn = drp_date.SelectedValue.JSCal_stringToDate();
                TimeSpan time = new TimeSpan(0, drp_time_h.getSelectedValueInt(0).objToInt32(), drp_time_m.getSelectedValueInt(0).objToInt32(), 0);
                dtIn = dtIn.Add(time);
                _currTBL.dtStartTime = dtIn.TimeOfDay.JSTime_timeToString();
                _currTBL.dtIn = dtIn;
                _currTBL.is_dtStartTimeChanged = 1;
            }
            _currTBL.pr_depositNotes = txt_pr_depositNotes.Text;
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            pnlOk.Visible = true;
        }
        protected void lnk_save_Click(object sender, EventArgs args)
        {
            saveData();
        }
    }
}