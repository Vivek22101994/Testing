using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental.uc
{
    public partial class ucReservationTmpPriceChange : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void fillData(RNT_TBL_RESERVATION _currTBL, int CommissionPercentage, bool IsEdit)
        {
            drp_pr_part_modified.Enabled = false;
            pnlDiscount.Visible = true;
            pnlDiscount2.Visible = true;
            pnl_ntxt_bcom_totalForOwner.Visible = pnl_ntxt_bcom_totalForOwner2.Visible = _currTBL.bcom_resId.ToInt64() > 0;
            pnlEdit.Visible = IsEdit;
            pnlView.Visible = !IsEdit;
            if (IsPostBack)
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "calculateTotals", "calculateTotals();", true);

            if (_currTBL.prTotalRate.objToDecimal() == 0 && _currTBL.pr_total.objToDecimal() > 0)
            {
                _currTBL.prTotalRate = _currTBL.pr_total.objToDecimal()
                    + _currTBL.pr_discount_commission.objToDecimal()
                    + _currTBL.pr_discount_owner.objToDecimal()
                    + _currTBL.prDiscountSpecialOffer.objToDecimal()
                    + _currTBL.prDiscountLastMinute.objToDecimal()
                    + _currTBL.prDiscountLongStay.objToDecimal()
                    + _currTBL.prDiscountLongRange.objToDecimal()
                    - _currTBL.pr_part_agency_fee.objToDecimal()
                    - _currTBL.pr_srsPrice.objToDecimal()
                    - _currTBL.pr_ecoPrice.objToDecimal();
                if (_currTBL.agentDiscountNotPayed.objToInt32() == 1)
                    _currTBL.prTotalRate += _currTBL.agentCommissionPrice.objToDecimal();
                _currTBL.prTotalCommission = _currTBL.pr_part_payment_total.objToDecimal() - _currTBL.pr_part_agency_fee.objToDecimal();
                _currTBL.prTotalOwner = _currTBL.pr_part_owner.objToDecimal() - _currTBL.pr_srsPrice.objToDecimal() - _currTBL.pr_ecoPrice.objToDecimal();
            }
            ntxt_bcom_totalForOwner.Value = _currTBL.bcom_totalForOwner.objToDouble();
            ntxt_prPercentage.Value = CommissionPercentage.objToDouble();
            ntxt_pr_discount_commission.Value = _currTBL.pr_discount_commission.objToDouble();
            ntxt_pr_discount_owner.Value = _currTBL.pr_discount_owner.objToDouble();
            txt_pr_discount_desc.Text = _currTBL.pr_discount_desc;
            ntxt_prDiscountSpecialOffer.Value = _currTBL.prDiscountSpecialOffer.objToDouble();
            ntxt_prDiscountLastMinute.Value = _currTBL.prDiscountLastMinute.objToDouble();
            txt_prDiscountLastMinuteDesc.Text = _currTBL.prDiscountLastMinuteDesc;
            ntxt_prDiscountLongStay.Value = _currTBL.prDiscountLongStay.objToDouble();
            txt_prDiscountLongStayDesc.Text = _currTBL.prDiscountLongStayDesc;
            ntxt_prDiscountLongRange.Value = _currTBL.prDiscountLongRange.objToDouble();
            txt_prDiscountLongRangeDesc.Text = _currTBL.prDiscountLongRangeDesc;
            ntxt_prTotalRate.Value = _currTBL.prTotalRate.objToDouble();
            ntxt_pr_part_agency_fee.Value = _currTBL.pr_part_agency_fee.objToDouble();
            ntxt_pr_srsPrice.Value = _currTBL.pr_srsPrice.objToDouble();
            ntxt_pr_ecoPrice.Value = _currTBL.pr_ecoPrice.objToDouble();
            ntxt_pr_total.Value = _currTBL.pr_total.objToDouble();
            ntxt_pr_part_payment_total.Value = _currTBL.pr_part_payment_total.objToDouble();
            ntxt_pr_part_owner.Value = _currTBL.pr_part_owner.objToDouble();
            ntxt_prTotalCommission.Value = _currTBL.prTotalCommission.objToDouble();
            ntxt_prTotalOwner.Value = _currTBL.prTotalOwner.objToDouble();
            ntxt_agentCommissionPrice.Value = _currTBL.agentCommissionPrice.objToDouble();
            drp_agentDiscountNotPayed.setSelectedValue(_currTBL.agentDiscountNotPayed);
            HF_estate.Value = Convert.ToString(_currTBL.pid_estate);
            HF_adult.Value = Convert.ToString(_currTBL.num_adult);
            HF_child.Value = Convert.ToString(_currTBL.num_child_over);
            HF_days.Value = Convert.ToString((_currTBL.dtEnd.Value - _currTBL.dtStart.Value).TotalDays.objToInt32());
            HF_resId.Value = Convert.ToString(_currTBL.id);
            HF_tmp.Value = "1";

            drp_requestFullPayAccepted.setSelectedValue(_currTBL.requestFullPayAccepted);
            drp_pr_part_modified.setSelectedValue(_currTBL.pr_part_modified);

            //for optioni
            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = _currTBL.dtStart.Value;
            outPrice.dtEnd = _currTBL.dtEnd.Value;
            outPrice.numPersCount = _currTBL.num_adult.objToInt32() + _currTBL.num_child_over.objToInt32();
            outPrice.numPers_adult = _currTBL.num_adult.objToInt32();
            outPrice.numPers_childOver = _currTBL.num_child_over.objToInt32();

            decimal agentTotalResPrice = 0;
            if (_currTBL.agentID.objToInt64() != 0)
            {
                using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == _currTBL.agentID.objToInt64());
                    if (agentTBL != null)
                    {
                        outPrice.fillAgentDetails(agentTBL);
                        DateTime checkDate = DateTime.Now;
                        DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                        DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                        var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTBL.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                        agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                    }
                }
            }
            outPrice.agentTotalResPrice = agentTotalResPrice;
            var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            outPrice.part_percentage = currEstate.pr_percentage.objToDecimal();
            decimal price = rntUtils.rntEstate_getPrice(0, _currTBL.pid_estate.objToInt32(), ref outPrice);
        }
        public void saveData(ref RNT_TBL_RESERVATION _currTBL)
        {
            _currTBL.pr_discount_commission = ntxt_pr_discount_commission.Value.objToDecimal();
            _currTBL.bcom_totalForOwner = ntxt_bcom_totalForOwner.Value.objToDecimal();
            _currTBL.pr_discount_owner = ntxt_pr_discount_owner.Value.objToDecimal();
            _currTBL.pr_discount_desc = txt_pr_discount_desc.Text;
            _currTBL.prDiscountSpecialOffer = ntxt_prDiscountSpecialOffer.Value.objToDecimal();
            _currTBL.prDiscountLastMinute = ntxt_prDiscountLastMinute.Value.objToDecimal();
            _currTBL.prDiscountLastMinuteDesc = txt_prDiscountLastMinuteDesc.Text;
            _currTBL.prDiscountLongStay = ntxt_prDiscountLongStay.Value.objToDecimal();
            _currTBL.prDiscountLongStayDesc = txt_prDiscountLongStayDesc.Text;
            _currTBL.prDiscountLongRange = ntxt_prDiscountLongRange.Value.objToDecimal();
            _currTBL.prDiscountLongRangeDesc = txt_prDiscountLongRangeDesc.Text;

            _currTBL.prTotalRate = ntxt_prTotalRate.Value.objToDecimal();
            _currTBL.pr_part_agency_fee = ntxt_pr_part_agency_fee.Value.objToDecimal();
            _currTBL.pr_srsPrice = ntxt_pr_srsPrice.Value.objToDecimal();
            _currTBL.pr_ecoPrice = ntxt_pr_ecoPrice.Value.objToDecimal();
            _currTBL.pr_total = ntxt_pr_total.Value.objToDecimal();
            _currTBL.pr_part_payment_total = ntxt_pr_part_payment_total.Value.objToDecimal();
            _currTBL.pr_part_owner = ntxt_pr_part_owner.Value.objToDecimal();
            _currTBL.prTotalCommission = ntxt_prTotalCommission.Value.objToDecimal();
            _currTBL.prTotalOwner = ntxt_prTotalOwner.Value.objToDecimal();
            _currTBL.agentCommissionPrice = ntxt_agentCommissionPrice.Value.objToDecimal();
            _currTBL.agentDiscountNotPayed = drp_agentDiscountNotPayed.getSelectedValueInt();
            _currTBL.pr_reservation = _currTBL.prTotalRate - _currTBL.prDiscountSpecialOffer - _currTBL.prDiscountLastMinute - _currTBL.prDiscountLongStay - _currTBL.prDiscountLongRange;


            if (_currTBL.requestFullPayAccepted != drp_requestFullPayAccepted.getSelectedValueInt())
            {
                _currTBL.requestFullPayAcceptedDate = DateTime.Now;
                _currTBL.requestFullPayAccepted = drp_requestFullPayAccepted.getSelectedValueInt();
                if (_currTBL.requestFullPayAccepted == 0)
                    _currTBL.requestFullPay = 0;
            }
            _currTBL.pr_part_forPayment = _currTBL.requestFullPayAccepted == 1 ? (_currTBL.pr_total.objToDecimal()) : _currTBL.pr_part_payment_total.objToDecimal(); // somma da pagare = Totale pren - sconto dell'agenzia, sconto potrebbe essere 0 se impostato dall'agenzia
            _currTBL.pr_part_modified = drp_pr_part_modified.getSelectedValueInt();
        }
        public void fillData(dbRntReservationTMP _currTBL, int CommissionPercentage, bool IsEdit)
        {
            pnlEdit.Visible = IsEdit;
            pnlView.Visible = !IsEdit;
            if (IsPostBack && IsEdit)
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "calculateTotals", "calculateTotals();", true);

            ntxt_prPercentage.Value = CommissionPercentage.objToDouble();
            ntxt_prDiscountSpecialOffer.Value = _currTBL.prDiscountSpecialOffer.objToDouble();
            ntxt_prDiscountLastMinute.Value = _currTBL.prDiscountLastMinute.objToDouble();
            txt_prDiscountLastMinuteDesc.Text = _currTBL.prDiscountLastMinuteDesc;
            ntxt_prDiscountLongStay.Value = _currTBL.prDiscountLongStay.objToDouble();
            txt_prDiscountLongStayDesc.Text = _currTBL.prDiscountLongStayDesc;
            ntxt_prDiscountLongRange.Value = _currTBL.prDiscountLongRange.objToDouble();
            txt_prDiscountLongRangeDesc.Text = _currTBL.prDiscountLongRangeDesc;

            ntxt_prTotalRate.Value = _currTBL.prTotalRate.objToDouble();
            ntxt_pr_part_agency_fee.Value = _currTBL.pr_part_agency_fee.objToDouble();
            ntxt_pr_srsPrice.Value = _currTBL.pr_srsPrice.objToDouble();
            ntxt_pr_ecoPrice.Value = _currTBL.pr_ecoPrice.objToDouble();
            ntxt_pr_total.Value = _currTBL.pr_total.objToDouble();
            ntxt_pr_part_payment_total.Value = _currTBL.pr_part_payment_total.objToDouble();
            ntxt_pr_part_owner.Value = _currTBL.pr_part_owner.objToDouble();
            ntxt_prTotalCommission.Value = _currTBL.prTotalCommission.objToDouble();
            ntxt_prTotalOwner.Value = _currTBL.prTotalOwner.objToDouble();
            ntxt_agentCommissionPrice.Value = _currTBL.agentCommissionPrice.objToDouble();
            drp_agentDiscountNotPayed.setSelectedValue(_currTBL.agentCommissionNotInTotal);

            drp_requestFullPayAccepted.setSelectedValue(_currTBL.requestFullPayAccepted);
            drp_pr_part_modified.setSelectedValue(_currTBL.pr_isManual);

            //for optioni
                       if (IsEdit == true)
            {
                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                outPrice.dtStart = _currTBL.dtStart.Value;
                outPrice.dtEnd = _currTBL.dtEnd.Value;
                outPrice.numPersCount = _currTBL.numPers_adult.objToInt32() + _currTBL.numPers_childOver.objToInt32();
                outPrice.numPers_adult = _currTBL.numPers_adult.objToInt32();
                outPrice.numPers_childOver = _currTBL.numPers_childOver.objToInt32();

                decimal agentTotalResPrice = 0;
                if (_currTBL.agentID.objToInt64() != 0)
                {
                    using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == _currTBL.agentID.objToInt64());
                        if (agentTBL != null)
                        {
                            outPrice.fillAgentDetails(agentTBL);
                            DateTime checkDate = DateTime.Now;
                            DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                            DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                            var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTBL.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                            agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                        }
                    }
                }
                outPrice.agentTotalResPrice = agentTotalResPrice;
                var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pidEstate);
                outPrice.part_percentage = currEstate.pr_percentage.objToDecimal();
                decimal price = rntUtils.rntEstate_getPrice(0, _currTBL.pidEstate.objToInt32(), ref outPrice);

            }
            else
            {
            }
            //filloptioni();
        }
        public void saveData(ref dbRntReservationTMP _currTBL)
        {
            _currTBL.prDiscountSpecialOffer = ntxt_prDiscountSpecialOffer.Value.objToDecimal();
            _currTBL.prDiscountLastMinute = ntxt_prDiscountLastMinute.Value.objToDecimal();
            _currTBL.prDiscountLastMinuteDesc = txt_prDiscountLastMinuteDesc.Text;
            _currTBL.prDiscountLongStay = ntxt_prDiscountLongStay.Value.objToDecimal();
            _currTBL.prDiscountLongStayDesc = txt_prDiscountLongStayDesc.Text;
            _currTBL.prDiscountLongRange = ntxt_prDiscountLongRange.Value.objToDecimal();
            _currTBL.prDiscountLongRangeDesc = txt_prDiscountLongRangeDesc.Text;

            _currTBL.prTotalRate = ntxt_prTotalRate.Value.objToDecimal();
            _currTBL.pr_part_agency_fee = ntxt_pr_part_agency_fee.Value.objToDecimal();
            _currTBL.pr_srsPrice = ntxt_pr_srsPrice.Value.objToDecimal();
            _currTBL.pr_ecoPrice = ntxt_pr_ecoPrice.Value.objToDecimal();
            _currTBL.pr_total = ntxt_pr_total.Value.objToDecimal();
            _currTBL.pr_part_payment_total = ntxt_pr_part_payment_total.Value.objToDecimal();
            //_currTBL.pr_part_forPayment = ntxt_pr_part_payment_total.Value.objToDecimal();
            _currTBL.pr_part_owner = ntxt_pr_part_owner.Value.objToDecimal();
            _currTBL.prTotalCommission = ntxt_prTotalCommission.Value.objToDecimal();
            _currTBL.prTotalOwner = ntxt_prTotalOwner.Value.objToDecimal();
            _currTBL.agentCommissionPrice = ntxt_agentCommissionPrice.Value.objToDecimal();
            _currTBL.agentCommissionNotInTotal = drp_agentDiscountNotPayed.getSelectedValueInt();
            _currTBL.pr_reservation = _currTBL.prTotalRate - _currTBL.prDiscountSpecialOffer - _currTBL.prDiscountLastMinute - _currTBL.prDiscountLongStay - _currTBL.prDiscountLongRange;


            _currTBL.requestFullPayAccepted = drp_requestFullPayAccepted.getSelectedValueInt();
            _currTBL.pr_part_forPayment = _currTBL.requestFullPayAccepted == 1 ? (_currTBL.pr_total.objToDecimal()) : _currTBL.pr_part_payment_total.objToDecimal(); // somma da pagare = Totale pren - sconto dell'agenzia, sconto potrebbe essere 0 se impostato dall'agenzia
            _currTBL.pr_isManual = drp_pr_part_modified.getSelectedValueInt();

        }


    }

}