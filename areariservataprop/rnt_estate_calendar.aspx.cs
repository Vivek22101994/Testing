using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.areariservataprop
{
    public partial class rnt_estate_calendar : ownerBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_RESERVATION _currTBL;
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
                LV.DataBind();
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "checkCalDates_", ltr_checkCalDates.Text, true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setCal", "setCal();", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                int id = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id && x.pid_owner == OwnerAuthentication.CurrentID);
                if (_est != null)
                {
                    IdEstate = _est.id;
                    ltr_apartment.Text = (_est.pid_owner == 385 ? _est.inner_notes : _est.code) + " / " + "rif. " + _est.id;
                    Bind_drp_state_pid();
                    LoadContent();
                    HF_dtStart.Value = DateTime.Now.AddDays(7).JSCal_dateToString();
                    HF_dtEnd.Value = DateTime.Now.AddDays(10).JSCal_dateToString();
                    HF_ext_ownerdaysinyear.Value = _est.ext_ownerdaysinyear.ToString();
                    UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                }
                else
                {
                    Response.Redirect("rnt_estate_list.aspx");
                }
            }
        }


        protected void Bind_drp_state_pid()
        {
            List<RNT_LK_RESERVATION_STATE> _list = AppSettings.RNT_LK_RESERVATION_STATEs.OrderBy(x => x.title).ToList();
            drp_state_pid.DataSource = _list;
            drp_state_pid.DataTextField = "title";
            drp_state_pid.DataValueField = "id";
            drp_state_pid.DataBind();
            drp_state_pid.Items.Insert(0, new ListItem("- tutti -", "0"));
        }
        protected void checkReservationsCal()
        {
            string _script = "";
            _script += "function checkCalDates(date){var _dtStart = parseInt($('#" + HF_dtStart.ClientID + "').val()); var _dtEnd = parseInt($('#" + HF_dtEnd.ClientID + "').val()); var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
            _script += rntUtils.rntEstate_availableDatesForJSCal(IdEstate, 0);
            _script += "if (dateint > _dtStart && dateint < _dtEnd) { _controls += '<span class=\"rntCal sel_f\"></span>'; }";
            _script += "if (dateint == _dtStart) { _controls += '<span class=\"rntCal sel_1\"></span>'; }";
            _script += "if (dateint == _dtEnd) { _controls += '<span class=\"rntCal sel_2\"></span>'; }";
            _script += "if (_controls.indexOf('<span class=\"rntCal nd_2\"></span>') != -1 && _controls.indexOf('<span class=\"rntCal nd_1\"></span>') != -1) { _enabled = false; }";
            _script += "return [_enabled, _class, _tooltip, _controls];";
            _script += "}";
            ltr_checkCalDates.Text = _script;
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "_JSCal_Range_" + Unique, "var _JSCal_Range_" + Unique + " = new JSCal.Range();", true);
        }
        protected int getResCount(int year)
        {
            int _count = 0;
            DateTime _dtStart = new DateTime(year, 1, 1);
            DateTime _dtEnd = new DateTime(year, 12, 31);
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.pid_estate == IdEstate && x.state_pid == 2 && x.dtEnd.HasValue && x.dtStart.HasValue && ((x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                                                                                       || (x.dtStart.Value.Date >= _dtStart && x.dtStart.Value.Date < _dtEnd) //
                                                                                                                                                       || (x.dtEnd.Value.Date > _dtStart && x.dtEnd.Value.Date <= _dtEnd))).ToList();
            foreach (RNT_TBL_RESERVATION _res in _list)
            {
                DateTime _start = _res.dtStart.Value.Date;
                DateTime _end = _res.dtEnd.Value.Date;
                if (_start < _dtStart)
                    _start = _dtStart;
                if(_end> _dtEnd)
                    _end = new DateTime(year, 12, 31);
                _count += (_end - _start).Days;
            }
            return _count;
        }


        protected void FillDataFromControls()
        {
            lbl_changeSaved.Visible = false;
            lblNewError.Visible = false;
            DateTime _dtStart = HF_dtStart.Value.JSCal_stringToDate();
            DateTime _dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.pid_estate == IdEstate && x.state_pid != 3 && x.dtEnd.HasValue && x.dtStart.HasValue && ((x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                                                                                                                   || (x.dtStart.Value.Date >= _dtStart && x.dtStart.Value.Date < _dtEnd) //
                                                                                                                                                                                   || (x.dtEnd.Value.Date > _dtStart && x.dtEnd.Value.Date <= _dtEnd))).ToList();

            if (_list.Count != 0)
            {
                lblNewError.Visible = true;
                lblNewError.InnerHtml = "La struttura risulta occupata nel periodo selezionato. <br/>Si prega di riprovare con altre date, oppure contattare l'assistenza.";
                return;
            }
            int _resCount = getResCount(_dtStart.Year);
            if ((HF_ext_ownerdaysinyear.Value.ToInt32() - _resCount) <= 0)
            {
                lblNewError.Visible = true;
                lblNewError.InnerHtml = "Avete superato il massimo dei giorni disponibili per anno " + _dtStart.Year + ". <br/>Si prega di contattare l'assistenza.";
                return;
            }
            if ((HF_ext_ownerdaysinyear.Value.ToInt32() - _resCount - (_dtStart.Date - _dtEnd.Date).Days) <= 0)
            {
                lblNewError.Visible = true;
                lblNewError.InnerHtml = "Con il periodo selezionato state superando il massimo dei giorni disponibili per anno " + _dtStart.Year + ". <br/>Si prega di selezionare " + (HF_ext_ownerdaysinyear.Value.ToInt32() - _resCount) + " giorni oppure contattare l'assistenza.";
                return;
            }
            _currTBL = rntUtils.newReservation();
            _currTBL.pid_creator = 1;
            _currTBL.state_pid = 2;
            _currTBL.state_body = "Creato dal proprietario";
            _currTBL.state_date = DateTime.Now;
            _currTBL.state_pid_user = 1;
            _currTBL.state_subject = txt_subject.Text;
            _currTBL.is_booking = 0;

            _currTBL.dtStartTime = "000000";
            _currTBL.dtEndTime = "000000";
            _currTBL.is_dtStartTimeChanged = 0;
            _currTBL.is_dtEndTimeChanged = 0;

            _currTBL.dtStart = _dtStart;
            _currTBL.dtEnd = _dtEnd;

            _currTBL.pid_estate = IdEstate;
            _currTBL.cl_id = -1;
            _currTBL.cl_email = "";
            _currTBL.cl_name_full = OwnerAuthentication.CurrentName;
            _currTBL.cl_name_honorific = "Prop.";
            _currTBL.cl_pid_discount = 0;
            _currTBL.cl_pid_lang = 1;

            _currTBL.num_adult = 0;
            _currTBL.num_child_over = 0;
            _currTBL.num_child_min = 0;

            _currTBL.pr_reservation = 0;
            _currTBL.pr_total = 0;
            _currTBL.pr_total_desc = "";
            _currTBL.pr_part_commission_tf = 0;
            _currTBL.pr_part_commission_total = 0;
            _currTBL.pr_part_agency_fee = 0;
            _currTBL.pr_part_payment_total = 0;
            _currTBL.pr_part_owner = 0;

            DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
            DC_RENTAL.SubmitChanges();
            _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
            _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            LoadContent();
            lblNewError.Visible = true;
            lblNewError.InnerHtml = "Avete prenotato la struttura con successo."
                                    + "<br/>Inizio: " + _dtStart.formatCustom("#DD# #dd# #MM# #yy#", 1, "__/__/____")
                                    + "<br/>Fine: " + _dtEnd.formatCustom("#DD# #dd# #MM# #yy#", 1, "__/__/____")
                                    + "<br/>Codice Pren.: " + _currTBL.code;
        }

        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            LoadContent();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LoadContent();
        }
        protected void lnk_change_Click(object sender, EventArgs e)
        {
        }
        protected void lnk_new_Click(object sender, EventArgs e)
        {
        }
        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            var lbl_id = LV.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
        }
        protected void LoadContent()
        {
            string _filter = "";
            string _sep = "";
            _filter += _sep + "pid_estate = " + IdEstate + "";
            _sep = " and ";
            if (drp_state_pid.getSelectedValueInt(0) != 0)
            {
                _filter += _sep + "state_pid = " + drp_state_pid.SelectedValue + "";
                _sep = " and ";
            }
            if (txt_code.Text.Trim() != "")
            {
                _filter += _sep + "code.Contains(\"" + txt_code.Text.Trim() + "\")";
                _sep = " and ";
            }

            if (HF_dtStart_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtStart >= DateTime.Parse(\"" + HF_dtStart_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_dtStart_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtStart <= DateTime.Parse(\"" + HF_dtStart_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }

            if (HF_dtEnd_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtEnd >= DateTime.Parse(\"" + HF_dtEnd_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_dtEnd_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtEnd <= DateTime.Parse(\"" + HF_dtEnd_to.Value.JSCal_stringToDate() + "\")";
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
            checkReservationsCal();
            HF_resCount.Value = getResCount(DateTime.Now.Year).ToString();
            HF_resCountYear.Value = DateTime.Now.Year.ToString();
            HF_resCount1.Value = getResCount(DateTime.Now.Year + 1).ToString();
            HF_resCountYear1.Value = (DateTime.Now.Year + 1).ToString();
            HF_resCount2.Value = getResCount(DateTime.Now.Year + 2).ToString();
            HF_resCountYear2.Value = (DateTime.Now.Year + 2).ToString();
        }
        protected void lnkFilter_Click(object sender, EventArgs e)
        {
            LoadContent();
        }
        protected void lnkNew_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_state = e.Item.FindControl("lbl_state") as Label;
            Label lbl_dtStart = e.Item.FindControl("lbl_dtStart") as Label;
            
            HyperLink HL_getPdf = e.Item.FindControl("HL_getPdf") as HyperLink;
            if (HL_getPdf == null || lbl_state == null) return;
            if (lbl_state.Text == "4")
            {
                HL_getPdf.Visible = true;
                RNT_TBL_RESERVATION _currRes = (RNT_TBL_RESERVATION)e.Item.DataItem;
                if (_currRes != null)
                {
                    string url_voucher = CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_voucherfo.aspx?uid=" + _currRes.unique_id + _currRes.uid_2;
                    string filename = "reservation_voucher-code_" + _currRes.code + ".pdf";
                    HL_getPdf.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher.urlEncode() + "&filename=" + filename.urlEncode();
                    
                }
            }
            else
            {
                HL_getPdf.Visible = false;
            }

            LinkButton lnk_cancel = e.Item.FindControl("lnk_cancel") as LinkButton;
            if (lnk_cancel == null || lbl_state == null || lbl_dtStart == null) return;
            lnk_cancel.Visible = lbl_state.Text == "2" && lbl_dtStart.Text.JSCal_stringToDate() > DateTime.Now;
        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if(lbl_id==null) return;
            if(e.CommandName=="cancella")
            {
                _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == lbl_id.Text.ToInt64());
                if(_currTBL.state_pid!=2) return;
                rntUtils.rntReservation_onStateChange(_currTBL);
                _currTBL.state_pid = 3;
                _currTBL.state_pid_user = 1;
                _currTBL.state_subject = "Cancellato dal proprietario";
                _currTBL.state_body = "";
                _currTBL.state_date = DateTime.Now;
                DC_RENTAL.SubmitChanges();
                rntUtils.rntReservation_onChange(_currTBL);
                Fill_LV();
            }
        }

        protected void LV_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            Fill_LV();
        }
    }
}