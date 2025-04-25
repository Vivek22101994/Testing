using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_estate : System.Web.UI.UserControl
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
        public int IdEstate
        {
            get { return HF_id.Value.ToInt32(); }
            set { HF_id.Value = value.ToString(); }
        }
        public int PidCity
        {
            get { return HF_pid_city.Value.ToInt32(); }
            set { HF_pid_city.Value = value.ToString(); }
        }
        public long IdReservation
        {
            get { return HF_IdReservation.Value.ToInt32(); }
            set { HF_IdReservation.Value = value.ToString(); }
        }
        public decimal sel_pr_total
        {
            get { return HF_sel_pr_total.Value.ToDecimal(); }
            set { HF_sel_pr_total.Value = value.ToString(); }
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
        public int sel_num_persons
        {
            get
            {
                return HF_sel_num_persons.Value.ToInt32();
            }
            set
            {
                HF_sel_num_persons.Value = value.ToString();
            }
        }
        public decimal pr_deposit
        {
            get
            {
                return HF_pr_deposit.Value.ToDecimal();
            }
            set
            {
                HF_pr_deposit.Value = value.ToString();
            }
        }
        public bool pr_depositWithCard
        {
            get
            {
                return HF_pr_depositWithCard.Value == "1";
            }
            set
            {
                HF_pr_depositWithCard.Value = value ? "1" : "0";
            }
        }
        public decimal pr_percentage
        {
            get
            {
                return HF_pr_percentage.Value.ToDecimal();
            }
            set
            {
                HF_pr_percentage.Value = value.ToString();
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
        public int num_rooms_bed
        {
            get
            {
                return HF_num_rooms_bed.Value.ToInt32();
            }
            set
            {
                HF_num_rooms_bed.Value = value.ToString();
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
        public int importance_stars
        {
            get
            {
                return HF_importance_stars.Value.ToInt32();
            }
            set
            {
                HF_importance_stars.Value = value.ToString();
            }
        }
        public bool is_exclusive
        {
            get { return HF_is_exclusive.Value.ToInt32() == 1; }
            set { HF_is_exclusive.Value = value ? "1" : "0"; }
        }
        public bool is_loft
        {
            get { return HF_is_loft.Value.ToInt32() == 1; }
            set { HF_is_loft.Value = value ? "1" : "0"; }
        }
        public bool is_ecopulizie
        {
            get { return HF_is_ecopulizie.Value.ToInt32() == 1; }
            set { HF_is_ecopulizie.Value = value ? "1" : "0"; }
        }
        public bool is_srs
        {
            get { return HF_is_srs.Value.ToInt32() == 1; }
            set { HF_is_srs.Value = value ? "1" : "0"; }
        }
        //public int num_rooms_bed
        //{
        //    get
        //    {
        //        return HF_num_rooms_bed.Value.ToInt32();
        //    }
        //    set
        //    {
        //        HF_num_rooms_bed.Value = value.ToString();
        //    }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                filterEstate();
                Bind_lbx_flt_zone();
                Bind_drp_min_num_persons_max();
                Bind_drp_min_num_rooms_bed();
                Bind_drp_min_num_rooms_bath();
                Bind_drp_min_num_bed_double();
                Bind_drp_min_importance_stars();
            }
        }
        protected void Bind_lbx_flt_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1).OrderBy(x => x.title).ToList();
            lbx_flt_zone.DataSource = _list;
            lbx_flt_zone.DataTextField = "title";
            lbx_flt_zone.DataValueField = "id";
            lbx_flt_zone.DataBind();
        }
        protected void Bind_drp_min_num_persons_max()
        {
            drp_min_num_persons_max.Items.Clear();
            drp_min_num_persons_max.Items.Add(new ListItem("--", "-1"));
            int _max = AppSettings.RNT_estateList.Max(x => x.num_persons_max);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_num_persons_max.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_min_num_rooms_bed()
        {
            drp_min_num_rooms_bed.Items.Clear();
            drp_min_num_rooms_bed.Items.Add(new ListItem("--", "-1"));
            int _max = AppSettings.RNT_estateList.Max(x => x.num_rooms_bed);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_num_rooms_bed.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_min_num_rooms_bath()
        {
            drp_min_num_rooms_bath.Items.Clear();
            drp_min_num_rooms_bath.Items.Add(new ListItem("--", "-1"));
            int _max = AppSettings.RNT_estateList.Max(x => x.num_rooms_bath);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_num_rooms_bath.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_min_num_bed_double()
        {
            drp_min_num_bed_double.Items.Clear();
            drp_min_num_bed_double.Items.Add(new ListItem("--", "-1"));
            int _max = AppSettings.RNT_estateList.Max(x => x.num_bed_double);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_num_bed_double.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_min_importance_stars()
        {
            drp_min_importance_stars.Items.Clear();
            drp_min_importance_stars.Items.Add(new ListItem("--", "-1"));
            int _max = AppSettings.RNT_estateList.Max(x => x.importance_stars);
            for (int i = 1; i <= _max; i++)
            {
                drp_min_importance_stars.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        public void FillControls()
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            _currTBL = null;
            if (IdEstate != 0)
            {
                _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            }
            if (_currTBL == null)
            {
                IdEstate = 0;
                showModify();
                if (onModify != null) { onModify(this, new EventArgs()); }
            }
            else
            {
                PidCity = _currTBL.pid_city.objToInt32();
                HF_pid_zone.Value = _currTBL.pid_zone.ToString();
                HF_code.Value = _currTBL.code;
                pr_deposit = _currTBL.pr_deposit.objToDecimal();
                pr_depositWithCard = _currTBL.pr_depositWithCard == 1;
                pr_percentage = _currTBL.pr_percentage.objToDecimal();
                num_persons_child = _currTBL.num_persons_child.objToInt32();
                num_persons_min = _currTBL.num_persons_min.objToInt32();
                num_persons_max = _currTBL.num_persons_max.objToInt32();
                num_rooms_bed = _currTBL.num_rooms_bed.objToInt32();
                nights_min = _currTBL.nights_min.objToInt32();
                importance_stars = _currTBL.importance_stars.objToInt32();
                is_exclusive = _currTBL.is_exclusive == 1;
                is_loft = _currTBL.is_loft == 1;
                is_ecopulizie = _currTBL.is_ecopulizie == 1;
                is_srs = _currTBL.is_srs == 1;

                // fill Prices
                if (_currTBL.pr_1_2pax == 0 && _currTBL.pr_2_2pax == 0 && _currTBL.pr_3_2pax == 0)
                {
                    PH_priceOnRequestCont.Visible = true;
                    PH_priceListCont.Visible = false;
                }
                else
                {

                    PH_priceOnRequestCont.Visible = false;
                    PH_priceListCont.Visible = true;
                    fillPrice_v1(_currTBL);
                }

                showView();
            }
        }
        protected void fillPrice_v1(RNT_TB_ESTATE currEstate)
        {
            decimal _pr_discount7daysApply = 1 - (currEstate.pr_discount7days.objToDecimal() / 100);
            decimal _pr_discount30daysApply = 1 - (currEstate.pr_discount30days.objToDecimal() / 100);
            string _priceDetails = "";
            decimal _prTemp = 0;
            int pr_basePersons = currEstate.pr_basePersons.objToInt32();
            for (int i = pr_basePersons; i <= HF_num_persons_max.Value.ToInt32(); i++)
            {
                int extraPersons = i - pr_basePersons;
                string numPers_string = extraPersons == 0 && currEstate.num_persons_min.objToInt32() < pr_basePersons ? i + " " + CurrentSource.getSysLangValue("lblPax") + " or less" : i + " " + CurrentSource.getSysLangValue("lblPax");
                string _prStr = ltr_priceTemplate.Text.Replace("#num_pers#", numPers_string);

                // low
                _prTemp = currEstate.pr_1_2pax.objToDecimal() + (extraPersons * currEstate.pr_1_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_1#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_1_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                // hight
                _prTemp = currEstate.pr_2_2pax.objToDecimal() + (extraPersons * currEstate.pr_2_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_2#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_2_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                // very hight
                _prTemp = currEstate.pr_3_2pax.objToDecimal() + (extraPersons * currEstate.pr_3_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_3#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_3_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _priceDetails += _prStr;
            }
            ltr_priceDetails.Text = _priceDetails;
        }
        protected void showModify()
        {
            pnl_view.Visible = false;
            pnl_edit.Visible = true;
        }
        protected void showView()
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
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            filterEstate();
        }
        protected void filterEstate()
        {
            int _dtCount = (sel_dtEnd - sel_dtStart).Days;
            List<int> _zoneList = lbx_flt_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            List<AppSettings.RNT_estate> _list = new List<AppSettings.RNT_estate>();
            if (txt_flt_code.Text.Trim() != "" || _zoneList.Count > 0)
            {
                _list = AppSettings.RNT_estateListAll.Where(x => x.code.ToLower().Contains(txt_flt_code.Text.ToLower().Trim()) && _zoneList.Contains(x.pid_zone)).ToList();
                _list = _list.Where(x => drp_min_num_persons_max.getSelectedValueInt(0) == -1 || x.num_persons_max >= drp_min_num_persons_max.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_min_num_rooms_bed.getSelectedValueInt(0) == -1 || x.num_rooms_bed >= drp_min_num_rooms_bed.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_min_num_rooms_bath.getSelectedValueInt(0) == -1 || x.num_rooms_bath >= drp_min_num_rooms_bath.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_min_num_bed_double.getSelectedValueInt(0) == -1 || x.num_bed_double >= drp_min_num_bed_double.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_min_importance_stars.getSelectedValueInt(0) == -1 || x.importance_stars >= drp_min_importance_stars.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_is_exclusive.getSelectedValueInt(0) == -1 || x.is_exclusive == drp_is_exclusive.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_is_ecopulizie.getSelectedValueInt(0) == -1 || x.is_ecopulizie == drp_is_ecopulizie.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_is_srs.getSelectedValueInt(0) == -1 || x.is_srs == drp_is_srs.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_is_loft.getSelectedValueInt(0) == -1 || x.is_loft == drp_is_loft.getSelectedValueInt(0)).ToList();
                //_list = _list.Where(x => drp_has_air_condition.getSelectedValueInt(0) == -1 || x.has_air_condition == drp_has_air_condition.getSelectedValueInt(0)).ToList();
                //_list = _list.Where(x => drp_has_internet.getSelectedValueInt(0) == -1 || (drp_has_internet.getSelectedValueInt(0) == 1 && (x.has_adsl == 1 || x.has_wifi == 1)) || (drp_has_internet.getSelectedValueInt(0) == 1 && (x.has_adsl == 0 && x.has_wifi == 0))).ToList();
                //_list = _list.Where(x => drp_has_lift.getSelectedValueInt(0) == -1 || x.has_lift == drp_has_lift.getSelectedValueInt(0)).ToList();

                List<int> estatesNotAvv = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 //
                                                                                  && x.dtStart.HasValue //
                                                                                  && x.dtEnd.HasValue //
                                                                                  && ((x.dtStart.Value.Date <= sel_dtStart && x.dtEnd.Value.Date >= sel_dtEnd) //
                                                                                      || (x.dtStart.Value.Date >= sel_dtStart && x.dtStart.Value.Date < sel_dtEnd) //
                                                                                      || (x.dtEnd.Value.Date > sel_dtStart && x.dtEnd.Value.Date <= sel_dtEnd))).Select(x => x.pid_estate.objToInt32()).ToList();
                _list = _list.Where(x => x.num_persons_max >= sel_num_persons && x.nights_min <= _dtCount && !estatesNotAvv.Contains(x.id)).ToList();
            }
            _list = _list.OrderBy(x => x.code).ToList();
            LV_flt.DataSource = _list;
            LV_flt.DataBind();
        }

        protected void LV_flt_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (lbl_id == null) return;
            IdEstate = lbl_id.Text.ToInt32();
            FillControls();
            if (onSave != null) { onSave(this, new EventArgs()); }
        }
        protected void LV_flt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Literal ltr_price = e.Item.FindControl("ltr_price") as Literal;
            if (lbl_id == null || ltr_price == null) return;
            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart =sel_dtStart;
            outPrice.dtEnd = sel_dtEnd;
            outPrice.numPersCount = sel_num_persons;
            outPrice.pr_discount_owner = 0;
            outPrice.pr_discount_commission = 0;
            outPrice.part_percentage = 20;
            outPrice.agentCommissionPerc = 0;
            decimal price = rntUtils.rntEstate_getPrice(0, lbl_id.Text.ToInt32(), ref outPrice);
            ltr_price.Text = price.ToString("N2") + "&nbsp;&euro;";
            if (price > sel_pr_total)
                ltr_price.Text += "<span style='color: red'>&nbsp;+&nbsp;" + (price - sel_pr_total).ToString("N2") + "</span>";
            if (price < sel_pr_total)
                ltr_price.Text += "<span style='color: green'>&nbsp;-&nbsp;" + (sel_pr_total - price).ToString("N2") + "</span>";
        }
    }
}