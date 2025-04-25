using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.ucMain
{
    public partial class ucBooking : System.Web.UI.UserControl
    {
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }

        public string CURRENT_SESSION_ID
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                return m.CURRENT_SESSION_ID;
            }
        }
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }
        private RNT_TB_ESTATE tmpEstateTB;
        public RNT_TB_ESTATE currEstateTB
        {
            get
            {
                if (tmpEstateTB == null)
                    tmpEstateTB = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                return tmpEstateTB ?? new RNT_TB_ESTATE();
            }
        }
        private RNT_LN_ESTATE tmpEstateLN;
        public RNT_LN_ESTATE currEstateLN
        {
            get
            {
                if (tmpEstateLN == null)
                    tmpEstateLN = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == App.LangID);
                return tmpEstateLN ?? new RNT_LN_ESTATE();
            }
        }
        private clSearch tmp_ls;
        public clSearch curr_ls
        {
            get
            {
                if (tmp_ls == null)
                {
                    clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                    tmp_ls = _config.lastSearch;
                }
                return tmp_ls ?? new clSearch();
            }
            set { tmp_ls = value; }
        }
        private rntExts.PreReservationPrices TMPcurrOutPrice;
        public rntExts.PreReservationPrices currOutPrice
        {
            get
            {
                if (TMPcurrOutPrice == null)
                    TMPcurrOutPrice = (rntExts.PreReservationPrices)ViewState[Unique + "_currOutPrice"];
                return TMPcurrOutPrice ?? new rntExts.PreReservationPrices();
            }
            set { TMPcurrOutPrice = value; ViewState[Unique + "_currOutPrice"] = TMPcurrOutPrice; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                clSearch _ls = _config.lastSearch;
                _config.addTo_myLastVisitedEstateList(IdEstate);
                clUtils.saveConfig(_config);
                fillData();
            }
            else
            {
                if (Request["__EVENTARGUMENT"] == "calculatePrice")
                {
                    checkAvailability();
                }
                if (Request["__EVENTARGUMENT"] == "calculateNumPersons")
                {
                    if (currEstateTB.is_chidren_allowed == 1)
                        calculateNumPersons();
                    checkAvailability();
                }
            }
            setCal();
        }

        protected void fillData()
        {
            HF_dtStart.Value = curr_ls.dtStart.JSCal_dateToString();
            HF_dtEnd.Value = curr_ls.dtEnd.JSCal_dateToString();

            for (int i = 1; i <= currEstateTB.num_persons_max.objToInt32(); i++)
            {
                drp_adult.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("reqAdults"), "" + i));
            }
            drp_adult.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("reqAdults"), "0"));
            drp_adult.setSelectedValue(curr_ls.numPers_adult.ToString());


            if (currEstateTB.is_chidren_allowed == 1)
            {

                //for (int i = 1; i <= (currEstateTB.num_persons_max.objToInt32() - curr_ls.numPers_adult); i++)
                //{
                //    drp_child_over.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("lblChildren3OrOver"), "" + i));
                //}
                //drp_child_over.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblChildren3OrOver"), "0"));
                //drp_child_over.setSelectedValue(curr_ls.numPers_childOver.ToString());

                // calculateNumPersons();

                for (int i = 1; i <= currEstateTB.num_persons_child.objToInt32(); i++)
                {
                    drp_child_min.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("lblChildrenUnder3"), "" + i));
                }
                drp_child_min.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblChildrenUnder3"), "0"));
                drp_child_min.setSelectedValue(curr_ls.numPers_childMin.ToString());

            }
            else
            {
                drp_child_min.Visible = drp_child_over.Visible = false;
                pnlChildrenNotAllowed.Visible = true;
            }
            calculateNumPersons();
            checkReservationsCal();
        }

        protected void setCal()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal_" + Unique, "setCal_" + Unique + "(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setChoosen", "setChoosen();", true);
        }
        protected void calculateNumPersons()
        {
            if (currEstateTB.is_chidren_allowed == 1)
            {
                int _num_persons_min = currEstateTB.num_persons_min.objToInt32();
                int _num_persons_max = currEstateTB.num_persons_max.objToInt32();
                int _selNum_adult = drp_adult.getSelectedValueInt(0).objToInt32();
                int _selNum_child_over = drp_child_over.getSelectedValueInt(0).objToInt32();
                drp_child_over.Items.Clear();
                int _minChildOver = _num_persons_min - _selNum_adult;
                if (_minChildOver <= 0)
                {
                    _minChildOver = 1;
                    drp_child_over.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblChildren3OrOver"), "0"));
                }
                for (int i = _minChildOver; i <= (_num_persons_max - _selNum_adult); i++)
                {
                    drp_child_over.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("lblChildren3OrOver"), "" + i));
                }
                if (_selNum_child_over > (_num_persons_max - _selNum_adult)) _selNum_child_over = (_num_persons_max - _selNum_adult);
                drp_child_over.setSelectedValue("" + _selNum_child_over);
            }
        }

        protected void checkReservationsCal()
        {
            string _script = "";
            _script += "function checkCalDates_" + Unique + "(date){var _dtStart = parseInt($('#" + HF_dtStart.ClientID + "').val()); var _dtEnd = parseInt($('#" + HF_dtEnd.ClientID + "').val()); var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
            _script += rntUtils.rntEstate_availableDatesForJSCal(IdEstate, 0);
            _script += "if (dateint > _dtStart && dateint < _dtEnd) { _controls += '<span class=\"rntCal sel_f\"></span>'; }";
            _script += "if (dateint == _dtStart) { _controls += '<span class=\"rntCal sel_1\"></span>'; }";
            _script += "if (dateint == _dtEnd) { _controls += '<span class=\"rntCal sel_2\"></span>'; }";
            _script += "if (_controls.indexOf('<span class=\"rntCal nd_2\"></span>') != -1 && _controls.indexOf('<span class=\"rntCal nd_1\"></span>') != -1) { _enabled = false; }";
            _script += "return [_enabled, _class, _tooltip, _controls];";
            _script += "}";
            ltr_checkCalDates.Text = _script;
        }

        protected void lnk_calculatePrice_Click(object sender, EventArgs e)
        {
            checkAvailability();
        }
        public void checkAvailability()
        {
            clConfig prevConfig = clUtils.getConfig(CURRENT_SESSION_ID);
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RNT_estateCheckAvvReset" + Unique, "RNT_estateCheckAvvReset();", true);
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);

                _config.lastSearch.numPers_adult = drp_adult.getSelectedValueInt(0).objToInt32();
                _config.lastSearch.numPers_childOver = drp_child_over.getSelectedValueInt(0).objToInt32();
                _config.lastSearch.numPers_childMin = drp_child_min.getSelectedValueInt(0).objToInt32();
                _config.lastSearch.numPersCount = _config.lastSearch.numPers_adult + _config.lastSearch.numPers_childOver;
                _config.lastSearch.dtStart = HF_dtStart.Value.JSCal_stringToDate();
                _config.lastSearch.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
                _config.lastSearch.dtCount = (_config.lastSearch.dtEnd - _config.lastSearch.dtStart).TotalDays.objToInt32();
                _config.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(_config);
                curr_ls = _config.lastSearch;

                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                outPrice.dtStart = curr_ls.dtStart;
                outPrice.dtEnd = curr_ls.dtEnd;
                outPrice.dtCount = curr_ls.dtCount;
                outPrice.numPersCount = curr_ls.numPersCount;
                outPrice.numPers_adult = curr_ls.numPers_adult;
                outPrice.numPers_childOver = curr_ls.numPers_childOver;
                outPrice.numPers_childMin = curr_ls.numPers_childMin;
                outPrice.pr_discount_owner = 0;
                outPrice.pr_discount_commission = 0;
                outPrice.part_percentage = currEstateTB.pr_percentage.objToDecimal();
                if (affiliatesarea.agentAuth.CurrentID != 0 && affiliatesarea.agentAuth.hasAcceptedContract == 1)
                    using (DCmodRental dc = new DCmodRental())
                        outPrice.fillAgentDetails(dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID));
                decimal _pr_total = rntUtils.rntEstate_getPrice(0, IdEstate, ref outPrice);
                bool _isAvailable;
                using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                    _isAvailable = dcOld.RNT_TBL_RESERVATION.Where(y => y.pid_estate == IdEstate //
                                                                                && y.state_pid != 3 //
                                                                                && y.dtStart.HasValue //
                                                                                && y.dtEnd.HasValue //
                                                                                && ((y.dtStart.Value.Date <= curr_ls.dtStart && y.dtEnd.Value.Date >= curr_ls.dtEnd) //
                                                                                    || (y.dtStart.Value.Date >= curr_ls.dtStart && y.dtStart.Value.Date < curr_ls.dtEnd) //
                                                                                    || (y.dtEnd.Value.Date > curr_ls.dtStart && y.dtEnd.Value.Date <= curr_ls.dtEnd))).Count() == 0;


                //                rise error intentionally

                //RNT_TB_ESTATE currestate = null;
                //string code = currestate.code;

                if (txt_promotionalCode.Text.Trim() != "" && _pr_total > 0)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var promoTbl = dc.dbRntDiscountPromoCodeTBLs.FirstOrDefault(x => x.code.ToLower().Trim() == txt_promotionalCode.Text.Trim().ToLower());
                        if (promoTbl != null)
                        {
                            decimal promoDiscount = _pr_total * promoTbl.discountAmount.objToDecimal() / 100;
                            _pr_total = _pr_total - promoDiscount;
                            //outPrice.pr_discount_owner = 0;
                            outPrice.pr_discount_commission = promoDiscount;
                        }
                    }
                }

                outPrice.prTotal = _pr_total;
                currOutPrice = outPrice;

                lbl_priceError0.Visible = outPrice.outError == 0;
                lbl_priceError1.Visible = outPrice.outError == 1;
                lbl_priceError2.Visible = outPrice.outError == 2;
                lbl_priceError3.Visible = outPrice.outError == 3;
                lbl_priceError4.Visible = outPrice.outError == 4;

                HF_tmp_prTotal.Value = _pr_total.objToInt32().ToString();
                PH_bookPriceOK.Visible = _pr_total != 0;
                PH_bookPriceError.Visible = _pr_total == 0;
                PH_bookNotAvailable.Visible = !_isAvailable;
                pnl_dicountCont.Visible = (currOutPrice.prDiscountLongStay + currOutPrice.prDiscountSpecialOffer + currOutPrice.pr_discount_commission) != 0;
                pnl_sendBooking.Visible = (currEstateTB.is_online_booking == 1 && _isAvailable && _pr_total != 0);
                //    pnl_inquire.Visible = (_pr_total == 0 || currEstateTB.is_online_booking != 1) && _isAvailable;

                //yourBookingPrice.Visible = true;
                //pnl_mr_rental_yousave.Visible = currOutPrice.prDiscountSpecialOffer > 0;


                // only for Detail page 
                if (Request.QueryString["tmpresid"] == null)
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RNT_alternativeEstate_fill" + Unique, "RNT_alternativeEstate_fill(" + outPrice.numPersCount + ", " + outPrice.dtStart.JSCal_dateToInt() + ", " + outPrice.dtEnd.JSCal_dateToInt() + ", " + _pr_total.objToInt32() + ", " + outPrice.numPers_adult + ", " + outPrice.numPers_childOver + ", " + outPrice.numPers_childMin + ");", true);
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "ucBooking", ex.ToString());
                drp_adult.setSelectedValue(prevConfig.lastSearch.numPers_adult);
                drp_child_over.getSelectedValueInt(prevConfig.lastSearch.numPers_childOver);
                drp_child_min.setSelectedValue(prevConfig.lastSearch.numPers_childMin);

                prevConfig.lastSearch.numPersCount = prevConfig.lastSearch.numPers_adult + prevConfig.lastSearch.numPers_childOver;
                HF_dtStart.Value = prevConfig.lastSearch.dtStart.JSCal_dateToString();
                HF_dtEnd.Value = prevConfig.lastSearch.dtEnd.JSCal_dateToString();

                prevConfig.lastSearch.dtCount = (prevConfig.lastSearch.dtEnd - prevConfig.lastSearch.dtStart).TotalDays.objToInt32();
                prevConfig.dtLastUsed = DateTime.Now;
                clUtils.saveConfig(prevConfig);
            }

        }

        protected void lnkBookNow_Click(object sender, EventArgs e)
        {
            var tmpTBL = new RntReservationTMP();
            tmpTBL.createdDate = DateTime.Now;
            tmpTBL.createdUserID = UserAuthentication.CurrentUserID;
            tmpTBL.createdUserNameFull = UserAuthentication.CurrentUserName;
            tmpTBL.pidEstate = IdEstate;
            tmpTBL.pidEstateCity = currEstateTB.pid_city.objToInt32();
            tmpTBL.dtStart = HF_dtStart.Value.JSCal_stringToDate();
            tmpTBL.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            tmpTBL.numPers_adult = drp_adult.getSelectedValueInt(0).objToInt32();
            tmpTBL.numPers_childMin = drp_child_min.getSelectedValueInt(0).objToInt32();
            tmpTBL.numPers_childOver = drp_child_over.getSelectedValueInt(0).objToInt32();

            if (tmpTBL.dtStart < DateTime.Now.Date || tmpTBL.dtEnd < DateTime.Now.Date)
            {
                lnkBookNow.Visible = false;
                return;
            }
            tmpTBL.cl_pid_lang = App.LangID;
            currOutPrice.CopyTo(ref tmpTBL);

            RNT_TBL_REQUEST _request = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == Request.QueryString["IdRequest"].ToInt32());
            if (_request != null)
            {
                tmpTBL.pid_related_request = _request.id;
            }

            tmpTBL.pr_discount_owner = 0;
            tmpTBL.pr_discount_commission = 0;
            tmpTBL.pr_discount_desc = "";
            if (txt_promotionalCode.Text.Trim() != "" && currOutPrice.pr_discount_commission > 0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var promoTbl = dc.dbRntDiscountPromoCodeTBLs.FirstOrDefault(x => x.code.ToLower().Trim() == txt_promotionalCode.Text.Trim().ToLower());
                    if (promoTbl != null)
                    {
                        tmpTBL.pr_discount_owner = 0;
                        tmpTBL.pr_discount_commission = currOutPrice.pr_discount_commission;
                        tmpTBL.pr_discount_desc = "-" + promoTbl.discountAmount.objToDecimal() + "% PromoCode: " + promoTbl.code + "";
                        tmpTBL.pr_discount_custom = 1;
                    }
                }
            }

            tmpTBL.pr_deposit = currEstateTB.pr_deposit;
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                dc.RntReservationTMP.InsertOnSubmit(tmpTBL);
                dc.SubmitChanges();
                Response.Redirect(App.HOST_SSL + "/util_bookingForm.aspx?tmpresid=" + tmpTBL.id +"&lang=" + CurrentLang.ID);
            }
        }
    }
}