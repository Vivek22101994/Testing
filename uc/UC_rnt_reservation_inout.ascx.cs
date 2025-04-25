using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.uc
{
    public partial class UC_rnt_reservation_inout : System.Web.UI.UserControl
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
        public long IdReservation
        {
            get
            {
                return HF_IdReservation.Value.ToInt64();
            }
            set
            {
                HF_IdReservation.Value = value.ToString();
            }
        }
        private RNT_TBL_RESERVATION _currTBL;
        private magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                RegisterScripts();
                lnk_saveInOutData.OnClientClick = "return _validateForm_" + Unique;
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setTime_" + Unique, "setTime();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "checkArrivalDeparture" + Unique, "checkArrivalDeparture();", true);
            }
        }
        protected void lnk_saveInOutData_Click(object sender, EventArgs e)
        {
            saveData();
        }
        protected void Bind_drp_places()
        {
            List<LIMO_TB_PICKUP_PLACE> _list = limoProps.LIMO_TB_PICKUP_PLACE.Where(x => x.isActive == 1).ToList();

            // air
            drp_in_air_place.Items.Clear();
            drp_out_air_place.Items.Clear();
            foreach (LIMO_TB_PICKUP_PLACE _place in _list.Where(x=>x.type=="air"))
            {
                drp_in_air_place.Items.Add(_place.title);
                drp_out_air_place.Items.Add(_place.title);
            }
            drp_in_air_place.Items.Insert(0, "");
            drp_out_air_place.Items.Insert(0, "");

            // sea
            drp_in_sea_place.Items.Clear();
            drp_out_sea_place.Items.Clear();
            foreach (LIMO_TB_PICKUP_PLACE _place in _list.Where(x => x.type == "sea"))
            {
                drp_in_sea_place.Items.Add(_place.title);
                drp_out_sea_place.Items.Add(_place.title);
            }
            drp_in_sea_place.Items.Insert(0, "");
            drp_out_sea_place.Items.Insert(0, "");

            // train
            drp_in_train_place.Items.Clear();
            drp_out_train_place.Items.Clear();
            foreach (LIMO_TB_PICKUP_PLACE _place in _list.Where(x => x.type == "train"))
            {
                drp_in_train_place.Items.Add(_place.title);
                drp_out_train_place.Items.Add(_place.title);
            }
            drp_in_train_place.Items.Insert(0, "");
            drp_out_train_place.Items.Insert(0, "");
        }
        public void fillData()
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null) 
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert_" + Unique, "alert('" + IdReservation + "');", true);
                return;
            }
            Bind_drp_places();
            drp_in_place.setSelectedValue(_currTBL.limo_inPoint_type);
            if (drp_in_place.SelectedValue == "air")
            {
                drp_in_air_place.setSelectedValue(_currTBL.limo_inPoint_pickupPlaceName);
                txt_in_air_dett.Text = _currTBL.limo_inPoint_details;
            }
            if (drp_in_place.SelectedValue == "sea")
            {
                drp_in_sea_place.setSelectedValue(_currTBL.limo_inPoint_pickupPlaceName);
                txt_in_sea_dett.Text = _currTBL.limo_inPoint_details;
            }
            if (drp_in_place.SelectedValue == "train")
            {
                drp_in_train_place.setSelectedValue(_currTBL.limo_inPoint_pickupPlaceName);
                txt_in_train_dett.Text = _currTBL.limo_inPoint_details;
            }
            if (drp_in_place.SelectedValue == "other")
            {
                txt_in_other_dett.Text = _currTBL.limo_inPoint_details;
            }
            drp_out_place.setSelectedValue(_currTBL.limo_outPoint_type);
            if (drp_out_place.SelectedValue == "air")
            {
                drp_out_air_place.setSelectedValue(_currTBL.limo_outPoint_pickupPlaceName);
                txt_out_air_dett.Text = _currTBL.limo_outPoint_details;
            }
            if (drp_out_place.SelectedValue == "sea")
            {
                drp_out_sea_place.setSelectedValue(_currTBL.limo_outPoint_pickupPlaceName);
                txt_out_sea_dett.Text = _currTBL.limo_outPoint_details;
            }
            if (drp_out_place.SelectedValue == "train")
            {
                drp_out_train_place.setSelectedValue(_currTBL.limo_outPoint_pickupPlaceName);
                txt_out_train_dett.Text = _currTBL.limo_outPoint_details;
            }
            if (drp_out_place.SelectedValue == "other")
            {
                txt_out_other_dett.Text = _currTBL.limo_outPoint_details;
            }
            // check-in
            txt_dtStart.Text = _currTBL.dtStart.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "");
            drp_dtStartTime_h.bind_Numbers(12, 23, 1, 2);
            drp_dtStartTime_m.bind_Numbers(0, 59, 5, 2);
            TimeSpan _timeStart = _currTBL.dtStartTime.JSTime_stringToTime(); ;
            drp_dtStartTime_h.setSelectedValue(_timeStart.Hours.ToString());
            drp_dtStartTime_m.setSelectedValue(_timeStart.Minutes.ToString());
            HF_in_time_cont.Value = _currTBL.dtStartTime;
            
            // limo-in
            drp_limo_in_datetime_h.bind_Numbers(0, 23, 1, 2);
            drp_limo_in_datetime_m.bind_Numbers(0, 59, 5, 2);
            DateTime _limo_in_datetime = _currTBL.limo_in_datetime.HasValue ? _currTBL.limo_in_datetime.Value : _currTBL.dtStart.Value.Add(_timeStart).AddHours(-2);
            drp_limo_in_datetime_h.setSelectedValue(_limo_in_datetime.TimeOfDay.Hours.ToString());
            drp_limo_in_datetime_m.setSelectedValue(_limo_in_datetime.TimeOfDay.Minutes.ToString());

            // check-out
            txt_dtEnd.Text = _currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "");
            drp_dtEndTime_h.bind_Numbers(0, 11, 1, 2);
            drp_dtEndTime_m.bind_Numbers(0, 59, 5, 2);
            TimeSpan _timeEnd = _currTBL.dtEndTime.JSTime_stringToTime();
            drp_dtEndTime_h.setSelectedValue(_timeEnd.Hours.ToString());
            drp_dtEndTime_m.setSelectedValue(_timeEnd.Minutes.ToString());
            HF_out_time_cont.Value = _currTBL.dtEndTime;

            // limo-out
            drp_limo_out_datetime_h.bind_Numbers(0, 23, 1, 2);
            drp_limo_out_datetime_m.bind_Numbers(0, 59, 5, 2);
            DateTime _limo_out_datetime = _currTBL.limo_out_datetime.HasValue ? _currTBL.limo_out_datetime.Value : _currTBL.dtEnd.Value.Add(_timeEnd).AddHours(2);
            drp_limo_out_datetime_h.setSelectedValue(_limo_out_datetime.TimeOfDay.Hours.ToString());
            drp_limo_out_datetime_m.setSelectedValue(_limo_out_datetime.TimeOfDay.Minutes.ToString());

        }
        protected void saveData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if(_currTBL==null) return;
            _currTBL.is_dtStartTimeChanged = 1;
            _currTBL.is_dtEndTimeChanged = 1;
            _currTBL.limo_isCompleted = 1;
            _currTBL.limo_inPoint_type = drp_in_place.SelectedValue;
            if (drp_in_place.SelectedValue == "air")
            {
                _currTBL.limo_inPoint_pickupPlaceName = drp_in_air_place.SelectedValue;
                _currTBL.limo_inPoint_details = txt_in_air_dett.Text;
            }
            if (drp_in_place.SelectedValue == "sea")
            {
                _currTBL.limo_inPoint_pickupPlaceName = drp_in_sea_place.SelectedValue;
                _currTBL.limo_inPoint_details = txt_in_sea_dett.Text;
            }
            if (drp_in_place.SelectedValue == "train")
            {
                _currTBL.limo_inPoint_pickupPlaceName = drp_in_train_place.SelectedValue;
                _currTBL.limo_inPoint_details = txt_in_train_dett.Text;
            }
            if (drp_in_place.SelectedValue == "other")
            {
                _currTBL.limo_inPoint_pickupPlaceName = "";
                _currTBL.limo_inPoint_details = txt_in_other_dett.Text;
            }
            _currTBL.limo_outPoint_type = drp_out_place.SelectedValue;
            if (drp_out_place.SelectedValue == "air")
            {
                _currTBL.limo_outPoint_pickupPlaceName = drp_out_air_place.SelectedValue;
                _currTBL.limo_outPoint_details = txt_out_air_dett.Text;
            }
            if (drp_out_place.SelectedValue == "sea")
            {
                _currTBL.limo_outPoint_pickupPlaceName = drp_out_sea_place.SelectedValue;
                _currTBL.limo_outPoint_details = txt_out_sea_dett.Text;
            }
            if (drp_out_place.SelectedValue == "train")
            {
                _currTBL.limo_outPoint_pickupPlaceName = drp_out_train_place.SelectedValue;
                _currTBL.limo_outPoint_details = txt_out_train_dett.Text;
            }
            if (drp_out_place.SelectedValue == "other")
            {
                _currTBL.limo_outPoint_pickupPlaceName = "";
                _currTBL.limo_outPoint_details = txt_out_other_dett.Text;
            }
            // check-in
            TimeSpan _timeStart = new TimeSpan(0, drp_dtStartTime_h.getSelectedValueInt(0).objToInt32(), drp_dtStartTime_m.getSelectedValueInt(0).objToInt32(), 0);
            _currTBL.dtStartTime = _timeStart.JSTime_timeToString();

            // limo-in
            DateTime _limo_in_datetime = _currTBL.limo_in_datetime.HasValue ? _currTBL.limo_in_datetime.Value.Date : _currTBL.dtStart.Value.Date;
            _limo_in_datetime = _limo_in_datetime.Add(new TimeSpan(0, drp_limo_in_datetime_h.getSelectedValueInt(0).objToInt32(), drp_limo_in_datetime_m.getSelectedValueInt(0).objToInt32(), 0));
            _currTBL.limo_in_datetime = _limo_in_datetime;

            // check-out
            TimeSpan _timeEnd = new TimeSpan(0, drp_dtEndTime_h.getSelectedValueInt(0).objToInt32(), drp_dtEndTime_m.getSelectedValueInt(0).objToInt32(), 0);
            _currTBL.dtEndTime = _timeEnd.JSTime_timeToString();

            // limo-out
            DateTime _limo_out_datetime = _currTBL.limo_out_datetime.HasValue ? _currTBL.limo_out_datetime.Value.Date : _currTBL.dtEnd.Value.Date;
            _limo_out_datetime = _limo_out_datetime.Add(new TimeSpan(0, drp_limo_out_datetime_h.getSelectedValueInt(0).objToInt32(), drp_limo_out_datetime_m.getSelectedValueInt(0).objToInt32(), 0));
            _currTBL.limo_out_datetime = _limo_out_datetime;

            _currTBL.limo_isCompleted = 1;
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            lbl_ok.Visible = true;
        }

        protected void RegisterScripts()
        {
            string _checkArrivalDeparture = @"
                function checkArrivalDeparture() {
                    var _modeInt = ""3"";
                    $("".mode_out"").addClass(""hide"");
                    $("".mode_in"").addClass(""hide"");
                    if (_modeInt == ""1"") {
                        setMode(""mode_in"");
                    }
                    else if (_modeInt == ""2"") {
                        setMode(""mode_out"");
                    }
                    else {
                        setMode(""mode_in"");
                        setMode(""mode_out"");
                    }

                    function setMode(_mode) {
                        if (_mode == """") return;
                        var _place = """";
                        var _place_select = """";
                        $(""."" + _mode + "".pick"").removeClass(""hide"");
                        $(""."" + _mode + "".dest"").removeClass(""hide"");
                        $(""."" + _mode + "".date"").removeClass(""hide"");
                        $(""."" + _mode + "".time"").removeClass(""hide"");
                        $(""."" + _mode + "".place"").removeClass(""hide"");
                        _place = _mode == ""mode_in"" ? $(""#drp_in_place.ClientID"").val() : $(""#drp_out_place.ClientID"").val();

                        if (_place != """")
                            $(""."" + _mode + ""."" + _place).removeClass(""hide"");
                        if (_mode == ""mode_in"") {
                            if (_place == ""air"")
                                _place_select = ""#drp_in_air_place.ClientID"";
                            if (_place == ""sea"")
                                _place_select = ""#drp_in_sea_place.ClientID"";
                            if (_place == ""train"")
                                _place_select = ""#drp_in_train_place.ClientID"";
                        }
                        else {
                            if (_place == ""air"")
                                _place_select = ""#drp_out_air_place.ClientID"";
                            if (_place == ""sea"")
                                _place_select = ""#drp_out_sea_place.ClientID"";
                            if (_place == ""train"")
                                _place_select = ""#drp_out_train_place.ClientID"";
                        }

                        if ($(_place_select).val() == """" || _place == """")
                            $(""."" + _mode + ""."" + _place + "".dett"").addClass(""hide"");
                        else
                            $(""."" + _mode + ""."" + _place + "".dett"").removeClass(""hide"");
                    }

                }
            ";
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("#reqRequiredField#", CurrentSource.getSysLangValue("reqRequiredField"));
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("#reqInvalidEmailFormat#", CurrentSource.getSysLangValue("reqInvalidEmailFormat"));
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("drp_in_place.ClientID", drp_in_place.ClientID);
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("drp_in_air_place.ClientID", drp_in_air_place.ClientID);
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("drp_in_sea_place.ClientID", drp_in_sea_place.ClientID);
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("drp_in_train_place.ClientID", drp_in_train_place.ClientID);
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("drp_out_place.ClientID", drp_out_place.ClientID);
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("drp_out_air_place.ClientID", drp_out_air_place.ClientID);
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("drp_out_sea_place.ClientID", drp_out_sea_place.ClientID);
            _checkArrivalDeparture = _checkArrivalDeparture.Replace("drp_out_train_place.ClientID", drp_out_train_place.ClientID);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_checkArrivalDeparture_" + Unique, _checkArrivalDeparture, true);
            string _validateForm = @"
			    function _validateForm_#Unique#() {
                   return true;
			    }
            ";
            _validateForm = _validateForm.Replace("#Unique#", Unique);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_validateForm_" + Unique, _validateForm, true);
            string in_time_ = @"
                var in_time;
                var out_time;
			    function setTime() {
                    in_time = timePicker({ timeCont: 'HF_in_time_cont.ClientID', timeView: 'in_time_view.ClientID', timeToggler: 'in_time_toggler', viewAsToggler: true });
                    out_time = timePicker({ timeCont: 'HF_out_time_cont.ClientID', timeView: 'out_time_view.ClientID', timeToggler: 'out_time_toggler', viewAsToggler: true });
			    }
            ";
            in_time_ = in_time_.Replace("#Unique#", Unique);
            in_time_ = in_time_.Replace("HF_in_time_cont.ClientID", HF_in_time_cont.ClientID);
            in_time_ = in_time_.Replace("in_time_view.ClientID", in_time_view.ClientID);
            in_time_ = in_time_.Replace("HF_out_time_cont.ClientID", HF_out_time_cont.ClientID);
            in_time_ = in_time_.Replace("out_time_view.ClientID", out_time_view.ClientID);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "_setTime_" + Unique, in_time_, true);
        }
    }
}