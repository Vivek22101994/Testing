using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_request_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_request";
            UC_rnt_request_operator1.onChange += new EventHandler(UC_rnt_request_operator_onChange);
        }
        private magaRental_DataContext DC_RENTAL;
        protected string listPage = "rnt_request_list.aspx";
        private RNT_TBL_REQUEST _currTBL;
        public int IdRequest
        {
            get
            {
                int _id;
                if (int.TryParse(HF_id.Value, out _id))
                    return _id;
                return 0;
            }
            set
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                HF_id.Value = value.ToString();
                FillControls();
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
            get { return HF_dtStart.Value.JSCal_stringToDate(); }
            set { HF_dtStart.Value = value.JSCal_dateToString(); }
        }
        public DateTime sel_dtEnd
        {
            get { return HF_dtEnd.Value.JSCal_stringToDate(); }
            set { HF_dtEnd.Value = value.JSCal_dateToString(); }
        }
        public int sel_num_persons
        {
            get
            {
                return HF_numPersTotal.Value.ToInt32();
            }
            set
            {
                HF_numPersTotal.Value = value.ToString();
            }
        }
        void UC_rnt_request_operator_onChange(object sender, EventArgs e)
        {
            UC_rnt_rl_request_state1.IdRequest = IdRequest;
            UpdatePanel_UC_rnt_rl_request_state.Update();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = Unique;
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == _id);
                    if (_currTBL == null)
                        Response.Redirect(listPage);
                    IdRequest = _id;
                }
                else
                    Response.Redirect(listPage, true);
                string _items = "";
                string _sep = "";
                List<RNT_TB_ESTATE> _estateList = AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted != 1).OrderBy(x => x.code).ToList();
                foreach (RNT_TB_ESTATE _estate in _estateList)
                {
                    _items += _sep + "{idEstate: \"" + _estate.id + "\", idZone: \"0\",label: \"" + _estate.code + "\",desc: \"\"}";
                    _sep = ",";
                }
                ltr_items.Text = _items;
                Bind_drp_city();
                Bind_lbx_flt_zone();
                Bind_drp_min_num_persons_max();
                Bind_drp_min_num_rooms_bed();
                Bind_drp_min_num_rooms_bath();
                Bind_drp_min_num_bed_double();
                Bind_drp_min_importance_stars();
                drp_adult.bind_Numbers(1, 10, 1, 0);
                drp_child_over.bind_Numbers(1, 10, 1, 0);
                drp_child_over.Items.Insert(0, new ListItem("---", "0"));
                drp_child_min.bind_Numbers(1, 3, 1, 0);
                drp_child_min.Items.Insert(0, new ListItem("---", "0"));
                Bind_chk_accessory();
                drp_pidCity_DataBind();
                FillControls();

            }
            else
            {
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setAutocomplete", "setAutocomplete();", true);
        }
        protected RNT_TBL_RESERVATION estateIsAvailable(int id)
        {
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                return dcOld.RNT_TBL_RESERVATION.FirstOrDefault(y => y.pid_estate == id //
                                                                            && y.state_pid != 3 //
                                                                            && y.state_pid != 3 //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= HF_dtStart.Value.JSCal_stringToDate() && y.dtEnd.Value.Date >= HF_dtEnd.Value.JSCal_stringToDate()) //
                                                                                || (y.dtStart.Value.Date >= HF_dtStart.Value.JSCal_stringToDate() && y.dtStart.Value.Date < HF_dtEnd.Value.JSCal_stringToDate()) //
                                                                                || (y.dtEnd.Value.Date > HF_dtStart.Value.JSCal_stringToDate() && y.dtEnd.Value.Date <= HF_dtEnd.Value.JSCal_stringToDate())));
        }

        protected void drp_pidCity_DataBind()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_pidCity.Items.Clear();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_pidCity.Items.Add(new ListItem("" + t.title, "" + t.id));
            }
            drp_pidCity.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
        protected void Bind_drp_relatedRequests()
        {
            drp_relatedRequests.Items.Clear();
            drp_relatedRequests.Items.Add(new ListItem("-seleziona-","-1"));
            List<RNT_TBL_REQUEST> _list = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.email == _currTBL.email && x.id != _currTBL.id && x.pid_related_request == 0).OrderByDescending(x => x.request_date_created).ToList();
            foreach (RNT_TBL_REQUEST _relRequest in _list)
            {
                drp_relatedRequests.Items.Add(new ListItem("rif. " + _relRequest.id + " - " + _relRequest.name_full + " - del " + _relRequest.request_date_created, "" + _relRequest.id));
            }
        }

        private void FillControls()
        {
            _currTBL = new RNT_TBL_REQUEST();
            if (IdRequest != 0)
            {
                _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == IdRequest);
            }
            if (_currTBL == null)
            {
                Response.Redirect(listPage);
                return;
            }
            pnl_setRelatedRequest.Visible = false;
            Bind_drp_relatedRequests();
            if (_currTBL.pid_related_request == 0)
            {
                HL_related_request.Visible = false;
                LV_relatedRequests.Visible = true;
                List<RNT_TBL_REQUEST> _relatedRequestList = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.pid_related_request == _currTBL.id).ToList();
                LV_relatedRequests.DataSource = _relatedRequestList;
                LV_relatedRequests.DataBind();
                pnl_setRelatedRequest.Visible = _relatedRequestList.Count == 0;

            }
            else
            {
                HL_related_request.Visible = true;
                LV_relatedRequests.Visible = false;
                RNT_TBL_REQUEST _relRequest = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == _currTBL.pid_related_request);
                if (_relRequest!=null)
                {
                    HL_related_request.Text = "Correlata alla Richiesta Primaria rif. " + _relRequest.id + " - " + _relRequest.name_full + " - del " + _relRequest.request_date_created;
                    HL_related_request.Enabled = true;
                    HL_related_request.NavigateUrl = "rnt_request_details.aspx?id=" + _relRequest.id;
                }
                else
                {
                    HL_related_request.Text = "La richiesta correlata Non trovata!";
                    HL_related_request.Enabled = false;
                }
                //UC_rnt_request_operator1.Enabled = false;
            }
            if (HF_cl_pid_lang.Value == "0") HF_cl_pid_lang.Value = "2";
            HF_IdReservation.Value = _currTBL.pid_reservation.objToInt64().ToString();
            HF_state_pid.Value = _currTBL.state_pid.objToInt32().ToString();
            HF_num_child_over.Value = _currTBL.request_child_num.ToString();
            HF_num_child_min.Value = _currTBL.request_child_num_min.ToString();
            HF_num_adult.Value = _currTBL.request_adult_num.ToString();

            ltr_date_request.Text = _currTBL.request_date_created.ToString();
            ltr_area.Text = _currTBL.request_area;
            ltr_price_range.Text = _currTBL.request_price_range;
            ltr_date_is_flexible.Text = _currTBL.request_date_is_flexible == 1 ? "SI" : "NO";
            ltr_transport.Text = _currTBL.request_transport;
            ltr_services.Text = _currTBL.request_services;
            ltr_notes.Text = _currTBL.request_notes;
            ltr_subject.Text = _currTBL.request_subject;
            UC_rnt_rl_request_state1.IdRequest = _currTBL.id;
            UC_rnt_rl_request_state1.cl_email = _currTBL.email;
            UC_rnt_request_operator1.IdRequest = _currTBL.id;
            fill_dates(_currTBL);
            DisableControls();

        }
        protected void fill_dates(RNT_TBL_REQUEST _request)
        {
            txt_name_full.Text = _currTBL.name_full;
            txt_email.Text = _currTBL.email;
            txt_phone.Text = _currTBL.phone;
            drp_country.DataBind();
            drp_country.Items.Insert(0, new ListItem("-seleziona-",""));
            drp_country.setSelectedValue(_currTBL.request_country);
            drp_lang.DataBind();
            drp_lang.Items.Insert(0, new ListItem("-seleziona-", "0"));
            drp_lang.setSelectedValue(_currTBL.pid_lang.ToString());
            HF_cl_pid_lang.Value = _currTBL.pid_lang.objToInt32().ToString();
            drp_adult.setSelectedValue(_request.request_adult_num.ToString());
            drp_child_over.setSelectedValue(_request.request_child_num.ToString());
            drp_child_min.setSelectedValue(_request.request_child_num_min.ToString());
            HF_numPersTotal.Value = (_request.request_adult_num.objToInt32() + _request.request_child_num.objToInt32()).ToString();
            HF_dtStart.Value = _request.request_date_start.JSCal_dateToString();
            HF_dtEnd.Value = _request.request_date_end.JSCal_dateToString();
            drp_pidCity.setSelectedValue(_currTBL.pid_city);
            drp_city.setSelectedValue(_currTBL.pid_city);
            Bind_lbx_flt_zone();
            pnl_datesEdit.Visible = false;
            pnl_datesView.Visible = true;
        }
        protected void save_dates()
        {
            _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == IdRequest);
            if (_currTBL == null)
            {
                Response.Redirect(listPage);
                return;
            }
            _currTBL.name_full = txt_name_full.Text;
            _currTBL.email = txt_email.Text;
            _currTBL.phone = txt_phone.Text;
            _currTBL.request_country = drp_country.SelectedValue;
            _currTBL.pid_lang = drp_lang.getSelectedValueInt(0);
            _currTBL.request_adult_num = drp_adult.getSelectedValueInt(0);
            _currTBL.request_child_num = drp_child_over.getSelectedValueInt(0);
            _currTBL.request_child_num_min = drp_child_min.getSelectedValueInt(0);
            _currTBL.request_date_start = HF_dtStart.Value.JSCal_stringToDate();
            _currTBL.request_date_end = HF_dtEnd.Value.JSCal_stringToDate();
            _currTBL.pid_city = drp_pidCity.getSelectedValueInt();
            DC_RENTAL.SubmitChanges();
            Response.Redirect("rnt_request_details.aspx?id=" + _currTBL.id);
        }
        protected void lnk_datesEdit_Click(object sender, EventArgs e)
        {
            pnl_datesEdit.Visible = true;
            pnl_datesView.Visible = false;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal();", true);
        }
        protected void lnk_datesSave_Click(object sender, EventArgs e)
        {
            save_dates();
        }
        protected void lnk_datesCancel_Click(object sender, EventArgs e)
        {
            _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == IdRequest);
            if (_currTBL == null)
            {
                Response.Redirect(listPage);
                return;
            }
            fill_dates(_currTBL);
        }
        private void Bind_drp_city()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_city.Items.Clear();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_city.Items.Add(new ListItem("" + t.title, "" + t.id));
            }
        }
        protected void drp_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_zone();
        }

        protected void Bind_lbx_flt_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1 && x.pid_city == drp_city.getSelectedValueInt(0)).OrderBy(x => x.title).ToList();
            lbx_flt_zone.DataSource = _list;
            lbx_flt_zone.DataTextField = "title";
            lbx_flt_zone.DataValueField = "id";
            lbx_flt_zone.DataBind();
        }
        protected void Bind_chk_accessory()
        {
            List<RNT_VIEW_CONFIG> _list = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.Where(x => x.pid_lang == CurrentLang.ID && AppSettings._RNT_CUSTOM_CONFIGs.Contains(x.id)).OrderBy(x => x.title).ToList();
            if (_list != null && _list.Count > 0)
            {
                chk_accessory.DataSource = _list;
                chk_accessory.DataTextField = "title";
                chk_accessory.DataValueField = "id";
                chk_accessory.DataBind();
            }
            

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
        protected void lnk_editAptListCancel_Click(object sender, EventArgs e)
        {
            pnl_showAptList.Visible = true;
            pnl_editAptList.Visible = false;
            UC_rnt_rl_request_state1.checkShowBody();
        }
        protected void lnk_editAptListShow_Click(object sender, EventArgs e)
        {
            pnl_showAptList.Visible = false;
            pnl_editAptList.Visible = true;
            UC_rnt_rl_request_state1.checkShowBody();
        }
        protected void lnk_editAptListSelect_Click(object sender, EventArgs e)
        {

            pnl_showAptList.Visible = true;
            pnl_editAptList.Visible = false;
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            filterEstate();
            UC_rnt_rl_request_state1.checkShowBody();
        }
        protected void filterEstate()
        {
            int _dtCount = (sel_dtEnd - sel_dtStart).Days;
            List<int> _zoneList = lbx_flt_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            List<AppSettings.RNT_estate> _list = new List<AppSettings.RNT_estate>();
            if (txt_flt_code.Text.Trim() != "" || _zoneList.Count > 0)
            {
                List<int> _selectedApts = DC_RENTAL.RNT_RL_REQUEST_ITEMs.Where(x => x.pid_request == IdRequest).Select(x => x.pid_estate).ToList();
                _list = AppSettings.RNT_estateListAll.Where(x => !_selectedApts.Contains(x.id) && x.code.ToLower().Contains(txt_flt_code.Text.ToLower().Trim()) && _zoneList.Contains(x.pid_zone)).ToList();

                _list = _list.Where(x => drp_is_exclusive.getSelectedValueInt(0) == -1 || x.is_exclusive == drp_is_exclusive.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_is_online_booking.getSelectedValueInt(0) == -1 || x.is_online_booking == drp_is_online_booking.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_is_ecopulizie.getSelectedValueInt(0) == -1 || x.is_ecopulizie == drp_is_ecopulizie.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_is_srs.getSelectedValueInt(0) == -1 || x.is_srs == drp_is_srs.getSelectedValueInt(0)).ToList();

                _list = _list.Where(x => x.num_persons_min <= HF_numPersTotal.Value.ToInt32()).ToList();
                _list = _list.Where(x => x.num_persons_max >= HF_numPersTotal.Value.ToInt32()).ToList();
                
                _list = _list.Where(x => drp_min_num_persons_max.getSelectedValueInt(0) == -1 || x.num_persons_max >= drp_min_num_persons_max.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_min_num_rooms_bed.getSelectedValueInt(0) == -1 || x.num_rooms_bed >= drp_min_num_rooms_bed.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_min_num_rooms_bath.getSelectedValueInt(0) == -1 || x.num_rooms_bath >= drp_min_num_rooms_bath.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_min_num_bed_double.getSelectedValueInt(0) == -1 || x.num_bed_double >= drp_min_num_bed_double.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_min_importance_stars.getSelectedValueInt(0) == -1 || x.importance_stars >= drp_min_importance_stars.getSelectedValueInt(0)).ToList();
                _list = _list.Where(x => drp_is_loft.getSelectedValueInt(0) == -1 || x.is_loft == drp_is_loft.getSelectedValueInt(0)).ToList();

                _list = _list.Where(x => drp_has_air_condition.getSelectedValueInt(0) == -1
                     || (drp_has_air_condition.getSelectedValueInt(0) == 1 && x.configList.Contains(1))
                     || (drp_has_air_condition.getSelectedValueInt(0) == 0 && !x.configList.Contains(1))
                    ).ToList();
                _list = _list.Where(x => drp_has_internet.getSelectedValueInt(0) == -1
                     || (drp_has_internet.getSelectedValueInt(0) == 1 && (x.configList.Contains(2) || x.configList.Contains(3)))
                     || (drp_has_internet.getSelectedValueInt(0) == 0 && !x.configList.Contains(2) && !x.configList.Contains(3))
                    ).ToList();
                _list = _list.Where(x => drp_has_lift.getSelectedValueInt(0) == -1
                     || (drp_has_lift.getSelectedValueInt(0) == 1 && x.configList.Contains(5))
                     || (drp_has_lift.getSelectedValueInt(0) == 0 && !x.configList.Contains(5))
                    ).ToList();

                List<int> estatesNotAvv = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 //
                                                                                  && x.dtStart.HasValue //
                                                                                  && x.dtEnd.HasValue //
                                                                                  && ((x.dtStart.Value.Date <= sel_dtStart && x.dtEnd.Value.Date >= sel_dtEnd) //
                                                                                      || (x.dtStart.Value.Date >= sel_dtStart && x.dtStart.Value.Date < sel_dtEnd) //
                                                                                      || (x.dtEnd.Value.Date > sel_dtStart && x.dtEnd.Value.Date <= sel_dtEnd))).Select(x => x.pid_estate.objToInt32()).ToList();
                _list = _list.Where(x => x.num_persons_max >= sel_num_persons && x.nights_min <= _dtCount && !estatesNotAvv.Contains(x.id)).ToList();
            }

           
            //filter with accessory
            List<string>lstConfig=chk_accessory.getSelectedValueList();
            List<int> lstConfigs = new List<int>();
            foreach (string objCongig in lstConfig)
            {
                lstConfigs.Add(objCongig.objToInt32());
            }
            //List<int> lstEstate = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => lstConfigs.Contains(x.pid_config)).Select(x=>x.pid_estate).ToList();
            //if (lstEstate != null && lstEstate.Count > 0)
            //{
            //    _list = _list.Where(x =>lstEstate.Contains(x.id)).ToList();
            //}
            foreach (int _conf in lstConfigs)
            {
                _list = _list.Where(x => x.configList.Contains(_conf)).ToList();
            }

            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = HF_dtStart.Value.JSCal_stringToDate();
            outPrice.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            outPrice.numPersCount = HF_num_adult.Value.ToInt32() + HF_num_child_over.Value.ToInt32();
            outPrice.pr_discount_owner = 0;
            outPrice.pr_discount_commission = 0;
            foreach (AppSettings.RNT_estate _rntEst in _list)
            {
                RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == HF_cl_pid_lang.Value.ToInt32() && !string.IsNullOrEmpty(x.title));
                if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 1);
                if (_lang != null)
                {
                    _rntEst.pid_lang = HF_cl_pid_lang.Value.ToInt32();
                    _rntEst.title = _lang.title;
                    _rntEst.summary = _lang.summary;
                    _rntEst.page_path = _lang.page_path;
                }
                else
                {
                    continue;
                }
                outPrice.part_percentage = _rntEst.pr_percentage;
                _rntEst.price = rntUtils.rntEstate_getPrice(0, _rntEst.id, ref outPrice);
                _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), HF_cl_pid_lang.Value.ToInt32(), "");
            }
            _list = _list.OrderBy(x => x.zone).ThenBy(x => x.code).ToList();
            LV_flt.DataSource = _list;
            LV_flt.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "check_aptSelector", "check_aptSelector();", true);
        }
        private void FillDataFromControls()
        {
            _currTBL = new RNT_TBL_REQUEST();
            if (HF_id.Value != "0")
            {
                _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == IdRequest);
            }
            if (_currTBL == null)
                Response.Redirect(listPage);
            DC_RENTAL.SubmitChanges();
        }
        protected void lnk_setRelatedRequest_Click(object sender, EventArgs e)
        {
            if (IdRequest != 0)
            {
                _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == IdRequest);
            }
            if (_currTBL == null)
            {
                Response.Redirect(listPage); 
                return;
            }
            int _relatedRequestId = drp_relatedRequests.getSelectedValueInt(0).objToInt32();
            RNT_TBL_REQUEST _relatedRequest = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == _relatedRequestId);
            if (_relatedRequest == null)
            {
                lbl_relatedRequestError.Visible = true;
                lbl_relatedRequestError.InnerHtml = "Selezionare una Richiesta principale";
                return;
            }
            lbl_relatedRequestError.Visible = false;
            _currTBL.IdAdMedia = _relatedRequest.IdAdMedia;
            _currTBL.IdLink = _relatedRequest.IdLink;
            _currTBL.pid_related_request = _relatedRequestId;
            DC_RENTAL.SubmitChanges();
            rntUtils.rntRequest_addState(_currTBL.id, 0, UserAuthentication.CurrentUserID, "Correlazione alla richiesta Primaria rif. " + _relatedRequestId,"");
            rntUtils.rntRequest_addState(_relatedRequestId, 0, UserAuthentication.CurrentUserID, "Aggiunta Correlazione alla richiesta Secondaria rif. " + _currTBL.id, "");
            rntUtils.rntRequest_updateOperator(_relatedRequestId, _relatedRequest.pid_operator.objToInt32(), true, true, UserAuthentication.CurrentUserID);
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            FillControls();
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            DisableControls();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_modify_Click(object sender, EventArgs e)
        {
            EnableControls();
        }
        protected void DisableControls()
        {
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            //lnk_modify.Visible = true;
        }
        protected void EnableControls()
        {
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
        }

        protected void DeleteRecord(object sender, EventArgs args)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var tbl = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == id);
            if (tbl != null)
            {
                var rl =
                        DC_RENTAL.RNT_RL_REQUEST_ITEMs.Where(
                            item => item.pid_request == tbl.id);
                DC_RENTAL.RNT_RL_REQUEST_ITEMs.DeleteAllOnSubmit(rl);
                DC_RENTAL.RNT_TBL_REQUEST.DeleteOnSubmit(tbl);
                DC_RENTAL.SubmitChanges();
            }
        }
    }
}
