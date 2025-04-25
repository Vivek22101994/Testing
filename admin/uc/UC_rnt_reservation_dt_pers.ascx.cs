using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_dt_pers : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;
        public event EventHandler onModify;
        public event EventHandler onSave;
        public event EventHandler onCancel;
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
        public bool IsBooking
        {
            get { return HF_is_booking.Value == "1"; }
            set { HF_is_booking.Value = value ? "1" : "0";
                pnl_persEdit.Visible = pnl_persView.Visible = value;
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
        public DateTime sel_dtStart
        {
            get { return HF_sel_dtStart.Value.JSCal_stringToDate(); }
            set { HF_sel_dtStart.Value = value.JSCal_dateToString(); }
        }
        public DateTime sel_dtEnd
        {
            get { return HF_sel_dtEnd.Value.JSCal_stringToDate(); }
            set { HF_sel_dtEnd.Value = value.JSCal_dateToString(); }
        }
        public int sel_dtCount
        {
            get
            {
                return HF_sel_dtCount.Value.ToInt32();
            }
            set
            {
                HF_sel_dtCount.Value = value.ToString();
            }
        }
        public int sel_num_adult
        {
            get
            {
                return HF_sel_num_adult.Value.ToInt32();
            }
            set
            {
                HF_sel_num_adult.Value = value.ToString();
            }
        }
        public int sel_num_child_over
        {
            get
            {
                return HF_sel_num_child_over.Value.ToInt32();
            }
            set
            {
                HF_sel_num_child_over.Value = value.ToString();
            }
        }
        public int sel_num_child_min
        {
            get
            {
                return HF_sel_num_child_min.Value.ToInt32();
            }
            set
            {
                HF_sel_num_child_min.Value = value.ToString();
            }
        }
        public int num_persons_child
        {
            get
            {
                return HF_num_persons_child.Value.ToInt32();
            }
            set
            {
                HF_num_persons_child.Value = value.ToString();
            }
        }
        public int num_persons_min
        {
            get
            {
                return HF_num_persons_min.Value.ToInt32();
            }
            set
            {
                HF_num_persons_min.Value = value.ToString();
            }
        }
        public int num_persons_max
        {
            get
            {
                return HF_num_persons_max.Value.ToInt32();
            }
            set
            {
                HF_num_persons_max.Value = value.ToString();
            }
        }
        public int nights_min
        {
            get
            {
                return HF_nights_min.Value.ToInt32();
            }
            set
            {
                HF_nights_min.Value = value.ToString();
            }
        }
        public string _checkCalDates
        {
            get
            {
                return ltr_checkCalDates.Text;
            }
            set
            {
                ltr_checkCalDates.Text = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                string _temp = Unique;
            }
        }
        public void FillControls()
        {
            showView();
        }
        public void showModify()
        {
            drp_adult.bind_Numbers(1, num_persons_max, 1, 0);
            drp_adult.setSelectedValue(sel_num_adult.ToString());
            drp_child_over.bind_Numbers(1, (num_persons_max - sel_num_adult), 1, 0);
            drp_child_over.Items.Insert(0, new ListItem("---", "0"));
            drp_child_over.setSelectedValue(sel_num_child_over.ToString());
            drp_child_min.bind_Numbers(1, num_persons_child, 1, 0);
            drp_child_min.Items.Insert(0, new ListItem("---", "0"));
            drp_child_min.setSelectedValue(sel_num_child_min.ToString());
           
            HF_dtStart.Value = sel_dtStart.JSCal_dateToString();
            HF_dtEnd.Value = sel_dtEnd.JSCal_dateToString();
            pnl_view.Visible = false;
            pnl_edit.Visible = true;
            checkReservationsCal();
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void drp_adult_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculateNumPersons();
        }
        protected void calculateNumPersons()
        {
            int _num_persons_min = HF_num_persons_min.Value.ToInt32();
            int _num_persons_max = HF_num_persons_max.Value.ToInt32();
            int _selNum_adult = drp_adult.getSelectedValueInt(0).objToInt32();
            int _selNum_child_over = drp_child_over.getSelectedValueInt(0).objToInt32();
            drp_child_over.Items.Clear();
            int _minChildOver = _num_persons_min - _selNum_adult;
            if (_minChildOver <= 0)
            {
                _minChildOver = 1;
                drp_child_over.Items.Add(new ListItem("---", "0"));
            }
            for (int i = _minChildOver; i <= (_num_persons_max - _selNum_adult); i++)
            {
                drp_child_over.Items.Add(new ListItem("" + i, "" + i));
            }
            if (_selNum_child_over > (_num_persons_max - _selNum_adult)) _selNum_child_over = (_num_persons_max - _selNum_adult);
            drp_child_over.setSelectedValue("" + _selNum_child_over);
        }
        protected void checkReservationsCal()
        {
            string _script = "";
            _script += "function checkCalDates_" + Unique + "(date){var _dtStart = parseInt($('#" + HF_dtStart.ClientID + "').val()); var _dtEnd = parseInt($('#" + HF_dtEnd.ClientID + "').val()); var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";

            _script += _checkCalDates;
            //List<RNT_TBL_RESERVATION> _resList = AppSettings.RNT_TBL_RESERVATION.Where(x => x.pid_estate == IdEstate && x.state_pid != 3 && x.dtStart.HasValue && x.dtEnd.HasValue).ToList();
            //foreach (RNT_TBL_RESERVATION _res in _resList)
            //{
            //    string _intDateFrom = "" + _res.dtStart.Value.JSCal_dateToInt();
            //    string _intDateTo = "" + _res.dtEnd.Value.JSCal_dateToInt();
            //    _script += "if (dateint > " + _intDateFrom + " && dateint < " + _intDateTo + ") { _controls += '<span class=\"rntCal nd_f\"></span>'; _enabled = false; }";
            //    _script += "if (dateint == " + _intDateFrom + ") { _controls += '<span class=\"rntCal nd_1\"></span>'; }";
            //    _script += "if (dateint == " + _intDateTo + ") { _controls += '<span class=\"rntCal nd_2\"></span>'; }";
            //}
            _script += "if (dateint > _dtStart && dateint < _dtEnd) { _controls += '<span class=\"rntCal sel_f\"></span>'; }";
            _script += "if (dateint == _dtStart) { _controls += '<span class=\"rntCal sel_1\"></span>'; }";
            _script += "if (dateint == _dtEnd) { _controls += '<span class=\"rntCal sel_2\"></span>'; }";
            _script += "if (_controls.indexOf('<span class=\"rntCal nd_2\"></span>') != -1 && _controls.indexOf('<span class=\"rntCal nd_1\"></span>') != -1) { _enabled = false; }";
            _script += "return [_enabled, _class, _tooltip, _controls];";
            _script += "}";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "checkCalDates_" + Unique, _script, true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal_" + Unique, "setCal_" + Unique + "(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "_JSCal_Range_" + Unique, "var _JSCal_Range_" + Unique + " = new JSCal.Range();", true);
        }
        public void showView()
        {
            pnl_view.Visible = true;
            pnl_edit.Visible = false;
        }
        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            showView();
            if (onCancel != null) { onCancel(this, new EventArgs()); }
        }
        protected void lnk_edit_Click(object sender, EventArgs e)
        {
            showModify();
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            sel_dtStart = HF_dtStart.Value.JSCal_stringToDate();
            sel_dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            sel_dtCount = txt_dtCount.Text.ToInt32();
            sel_num_adult = drp_adult.getSelectedValueInt(0).objToInt32();
            sel_num_child_min = drp_child_min.getSelectedValueInt(0).objToInt32();
            sel_num_child_over = drp_child_over.getSelectedValueInt(0).objToInt32();
            FillControls();
            if (onSave != null) { onSave(this, new EventArgs()); }
        }
    }
}