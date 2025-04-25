using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_inout : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        public event EventHandler onModify;
        public event EventHandler onSave;
        public event EventHandler onCancel;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public bool IsLocked
        {
            get { return pnl_lock.Visible; }
            set { pnl_lock.Visible = value; }
        }
        public bool IsEdit
        {
            get { return HF_isEdit.Value == "1"; }
            set { HF_isEdit.Value = value ? "1" : "0"; }
        }
        public bool IsChanged
        {
            get { return HF_isChanged.Value == "1"; }
            set { HF_isChanged.Value = value ? "1" : "0"; }
        }
        public long IdReservation
        {
            get { return HF_IdReservation.Value.ToInt32(); }
            set { HF_IdReservation.Value = value.ToString(); }
        }
        public string UIdReservation
        {
            get { return HF_uid.Value; }
            set { HF_uid.Value = value; }
        }

        public string in_pointType
        {
            get { return HF_in_pointType.Value; }
            set { HF_in_pointType.Value = value; }
        }
        public string in_transportType
        {
            get { return HF_in_transportType.Value; }
            set { HF_in_transportType.Value = value; }
        }
        public int in_pickupPlace
        {
            get { return HF_in_pickupPlace.Value.ToInt32(); }
            set { HF_in_pickupPlace.Value = value.ToString(); }
        }
        public string in_pickupPlaceName
        {
            get { return HF_in_pickupPlaceName.Value; }
            set { HF_in_pickupPlaceName.Value = value; }
        }
        public string in_pickupDetails
        {
            get { return HF_in_pickupDetails.Value; }
            set { HF_in_pickupDetails.Value = value; }
        }
        public int in_pickupDetailsType
        {
            get { return HF_in_pickupDetailsType.Value.ToInt32(); }
            set { HF_in_pickupDetailsType.Value = value.ToString(); }
        }

        public DateTime? limo_in_datetime
        {
            get { return HF_limo_in_datetime.Value != "" ? HF_limo_in_datetime.Value.JSCal_stringToDateTime() : (DateTime?)null; }
            set { HF_limo_in_datetime.Value = value != null ? value.Value.JSCal_dateTimeToString() : ""; }
        }
        public DateTime? dtIn
        {
            get { return HF_dtIn.Value != "" ? HF_dtIn.Value.JSCal_stringToDateTime() : (DateTime?)null; }
            set { HF_dtIn.Value = value != null ? value.Value.JSCal_dateTimeToString() : ""; }
        }

        public string out_pointType
        {
            get { return HF_out_pointType.Value; }
            set { HF_out_pointType.Value = value; }
        }
        public string out_transportType
        {
            get { return HF_out_transportType.Value; }
            set { HF_out_transportType.Value = value; }
        }
        public int out_pickupPlace
        {
            get { return HF_out_pickupPlace.Value.ToInt32(); }
            set { HF_out_pickupPlace.Value = value.ToString(); }
        }
        public string out_pickupPlaceName
        {
            get { return HF_out_pickupPlaceName.Value; }
            set { HF_out_pickupPlaceName.Value = value; }
        }
        public string out_pickupDetails
        {
            get { return HF_out_pickupDetails.Value; }
            set { HF_out_pickupDetails.Value = value; }
        }
        public int out_pickupDetailsType
        {
            get { return HF_out_pickupDetailsType.Value.ToInt32(); }
            set { HF_out_pickupDetailsType.Value = value.ToString(); }
        }

        public DateTime? limo_out_datetime
        {
            get { return HF_limo_out_datetime.Value != "" ? HF_limo_out_datetime.Value.JSCal_stringToDateTime() : (DateTime?)null; }
            set { HF_limo_out_datetime.Value = value != null ? value.Value.JSCal_dateTimeToString() : ""; }
        }
        public DateTime? dtOut
        {
            get { return HF_dtOut.Value != "" ? HF_dtOut.Value.JSCal_stringToDateTime() : (DateTime?)null; }
            set { HF_dtOut.Value = value != null ? value.Value.JSCal_dateTimeToString() : ""; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //fillData();
            }
        }
        //protected void Bind_transportType()
        //{
        //    List<LIMO_LK_TRANSPORTTYPE> _list = limoProps.LIMO_LK_TRANSPORTTYPE.Where(x => x.isActive == 1).ToList();
        //    drp_inPoint_transportType.Items.Clear();
        //    drp_outPoint_transportType.Items.Clear();
        //    foreach (LIMO_LK_TRANSPORTTYPE transportType in _list)
        //    {
        //        drp_inPoint_transportType.Items.Add(new ListItem(transportType.title, transportType.code));
        //        drp_outPoint_transportType.Items.Add(new ListItem(transportType.title, transportType.code));
        //    }
        //    drp_inPoint_transportType.Items.Insert(0, "");
        //    drp_outPoint_transportType.Items.Insert(0, "");
        //}
        //protected void Bind_drp_places()
        //{
        //    List<LIMO_TB_PICKUP_PLACE> _list = limoProps.LIMO_TB_PICKUP_PLACE.Where(x => x.isActive == 1).ToList();

        //    // air
        //    drp_in_air_place.Items.Clear();
        //    drp_out_air_place.Items.Clear();
        //    foreach (LIMO_TB_PICKUP_PLACE _place in _list.Where(x => x.type == "air"))
        //    {
        //        drp_in_air_place.Items.Add(new ListItem(_place.title, _place.id.ToString()));
        //        drp_out_air_place.Items.Add(new ListItem(_place.title, _place.id.ToString()));
        //    }
        //    drp_in_air_place.Items.Insert(0, "");
        //    drp_out_air_place.Items.Insert(0, "");

        //    // sea
        //    drp_in_sea_place.Items.Clear();
        //    drp_out_sea_place.Items.Clear();
        //    foreach (LIMO_TB_PICKUP_PLACE _place in _list.Where(x => x.type == "sea"))
        //    {
        //        drp_in_sea_place.Items.Add(new ListItem(_place.title, _place.id.ToString()));
        //        drp_out_sea_place.Items.Add(new ListItem(_place.title, _place.id.ToString()));
        //    }
        //    drp_in_sea_place.Items.Insert(0, "");
        //    drp_out_sea_place.Items.Insert(0, "");

        //    // train
        //    drp_in_train_place.Items.Clear();
        //    drp_out_train_place.Items.Clear();
        //    foreach (LIMO_TB_PICKUP_PLACE _place in _list.Where(x => x.type == "train"))
        //    {
        //        drp_in_train_place.Items.Add(new ListItem(_place.title, _place.id.ToString()));
        //        drp_out_train_place.Items.Add(new ListItem(_place.title, _place.id.ToString()));
        //    }
        //    drp_in_train_place.Items.Insert(0, "");
        //    drp_out_train_place.Items.Insert(0, "");
        //}
        //protected void fillData()
        //{
        //    Bind_transportType();
        //    Bind_drp_places();
        //    drp_in_place.setSelectedValue(in_pointType);
        //    if (drp_in_place.SelectedValue == "air")
        //    {
        //        drp_in_air_place.setSelectedValue(limo_inPoint_pickupPlaceName);
        //        txt_in_air_dett.Text = limo_inPoint_details;
        //    }
        //    if (drp_in_place.SelectedValue == "sea")
        //    {
        //        drp_in_sea_place.setSelectedValue(limo_inPoint_pickupPlaceName);
        //        txt_in_sea_dett.Text = limo_inPoint_details;
        //    }
        //    if (drp_in_place.SelectedValue == "train")
        //    {
        //        drp_in_train_place.setSelectedValue(limo_inPoint_pickupPlaceName);
        //        txt_in_train_dett.Text = limo_inPoint_details;
        //    }
        //    if (drp_in_place.SelectedValue == "other")
        //    {
        //        txt_in_other_dett.Text = limo_inPoint_details;
        //    }
        //    drp_out_place.setSelectedValue(limo_outPoint_type);
        //    if (drp_out_place.SelectedValue == "air")
        //    {
        //        drp_out_air_place.setSelectedValue(limo_outPoint_pickupPlaceName);
        //        txt_out_air_dett.Text = limo_outPoint_details;
        //    }
        //    if (drp_out_place.SelectedValue == "sea")
        //    {
        //        drp_out_sea_place.setSelectedValue(limo_outPoint_pickupPlaceName);
        //        txt_out_sea_dett.Text = limo_outPoint_details;
        //    }
        //    if (drp_out_place.SelectedValue == "train")
        //    {
        //        drp_out_train_place.setSelectedValue(limo_outPoint_pickupPlaceName);
        //        txt_out_train_dett.Text = limo_outPoint_details;
        //    }
        //    if (drp_out_place.SelectedValue == "other")
        //    {
        //        txt_out_other_dett.Text = limo_outPoint_details;
        //    }
        //    // check-in
        //    txt_dtStart.Text = dtStart.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "");
        //    drp_dtStartTime_h.bind_Numbers(12, 23, 1, 2);
        //    drp_dtStartTime_m.bind_Numbers(0, 59, 5, 2);
        //    TimeSpan _timeStart = dtStartTime.JSTime_stringToTime(); ;
        //    drp_dtStartTime_h.setSelectedValue(_timeStart.Hours.ToString());
        //    drp_dtStartTime_m.setSelectedValue(_timeStart.Minutes.ToString());

        //    // limo-in
        //    drp_limo_in_datetime_h.bind_Numbers(0, 23, 1, 2);
        //    drp_limo_in_datetime_m.bind_Numbers(0, 59, 5, 2);
        //    DateTime _limo_in_datetime = limo_in_datetime;
        //    drp_limo_in_datetime_h.setSelectedValue(_limo_in_datetime.TimeOfDay.Hours.ToString());
        //    drp_limo_in_datetime_m.setSelectedValue(_limo_in_datetime.TimeOfDay.Minutes.ToString());

        //    // check-out
        //    txt_dtEnd.Text = dtEnd.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "");
        //    drp_dtEndTime_h.bind_Numbers(0, 11, 1, 2);
        //    drp_dtEndTime_m.bind_Numbers(0, 59, 5, 2);
        //    TimeSpan _timeEnd = dtEndTime.JSTime_stringToTime();
        //    drp_dtEndTime_h.setSelectedValue(_timeEnd.Hours.ToString());
        //    drp_dtEndTime_m.setSelectedValue(_timeEnd.Minutes.ToString());

        //    // limo-out
        //    drp_limo_out_datetime_h.bind_Numbers(0, 23, 1, 2);
        //    drp_limo_out_datetime_m.bind_Numbers(0, 59, 5, 2);
        //    DateTime _limo_out_datetime = limo_out_datetime;
        //    drp_limo_out_datetime_h.setSelectedValue(_limo_out_datetime.TimeOfDay.Hours.ToString());
        //    drp_limo_out_datetime_m.setSelectedValue(_limo_out_datetime.TimeOfDay.Minutes.ToString());

        //}
        //protected void saveData()
        //{
        //    limo_inPoint_type = drp_in_place.SelectedValue;
        //    if (drp_in_place.SelectedValue == "air")
        //    {
        //        limo_inPoint_pickupPlaceName = drp_in_air_place.SelectedValue;
        //        limo_inPoint_details = txt_in_air_dett.Text;
        //    }
        //    if (drp_in_place.SelectedValue == "sea")
        //    {
        //        limo_inPoint_pickupPlaceName = drp_in_sea_place.SelectedValue;
        //        limo_inPoint_details = txt_in_sea_dett.Text;
        //    }
        //    if (drp_in_place.SelectedValue == "train")
        //    {
        //        limo_inPoint_pickupPlaceName = drp_in_train_place.SelectedValue;
        //        limo_inPoint_details = txt_in_train_dett.Text;
        //    }
        //    if (drp_in_place.SelectedValue == "other")
        //    {
        //        limo_inPoint_pickupPlaceName = "";
        //        limo_inPoint_details = txt_in_other_dett.Text;
        //    }
        //    limo_outPoint_type = drp_out_place.SelectedValue;
        //    if (drp_out_place.SelectedValue == "air")
        //    {
        //        limo_outPoint_pickupPlaceName = drp_out_air_place.SelectedValue;
        //        limo_outPoint_details = txt_out_air_dett.Text;
        //    }
        //    if (drp_out_place.SelectedValue == "sea")
        //    {
        //        limo_outPoint_pickupPlaceName = drp_out_sea_place.SelectedValue;
        //        limo_outPoint_details = txt_out_sea_dett.Text;
        //    }
        //    if (drp_out_place.SelectedValue == "train")
        //    {
        //        limo_outPoint_pickupPlaceName = drp_out_train_place.SelectedValue;
        //        limo_outPoint_details = txt_out_train_dett.Text;
        //    }
        //    if (drp_out_place.SelectedValue == "other")
        //    {
        //        limo_outPoint_pickupPlaceName = "";
        //        limo_outPoint_details = txt_out_other_dett.Text;
        //    }
        //    // check-in
        //    TimeSpan _timeStart = new TimeSpan(0, drp_dtStartTime_h.getSelectedValueInt(0).objToInt32(), drp_dtStartTime_m.getSelectedValueInt(0).objToInt32(), 0);
        //    dtStartTime = _timeStart.JSTime_timeToString();

        //    // limo-in
        //    DateTime _limo_in_datetime = limo_in_datetime.Date;
        //    _limo_in_datetime = _limo_in_datetime.Add(new TimeSpan(0, drp_limo_in_datetime_h.getSelectedValueInt(0).objToInt32(), drp_limo_in_datetime_m.getSelectedValueInt(0).objToInt32(), 0));
        //    limo_in_datetime = _limo_in_datetime;

        //    // check-out
        //    TimeSpan _timeEnd = new TimeSpan(0, drp_dtEndTime_h.getSelectedValueInt(0).objToInt32(), drp_dtEndTime_m.getSelectedValueInt(0).objToInt32(), 0);
        //    dtEndTime = _timeEnd.JSTime_timeToString();

        //    // limo-out
        //    DateTime _limo_out_datetime = limo_out_datetime.Date;
        //    _limo_out_datetime = _limo_out_datetime.Add(new TimeSpan(0, drp_limo_out_datetime_h.getSelectedValueInt(0).objToInt32(), drp_limo_out_datetime_m.getSelectedValueInt(0).objToInt32(), 0));
        //    limo_out_datetime = _limo_out_datetime;

        //}
        public void FillControls()
        {
            showView();
        }
        private void FillDataFromControls()
        {
            //saveData();
            FillControls();
            if (onSave != null) { onSave(this, new EventArgs()); }
        }
        protected void showModify()
        {
            //fillData();
            pnl_view.Visible = false;
            pnl_edit.Visible = true;
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void showView()
        {
            pnl_view.Visible = true;
            pnl_edit.Visible = false;
        }
        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            FillControls();
            if (onCancel != null) { onCancel(this, new EventArgs()); }
        }
        protected void lnk_edit_Click(object sender, EventArgs e)
        {
            showModify();
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
    }
}