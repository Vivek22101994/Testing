using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.affiliatesarea
{
    public partial class estatePrice : agentBasePage
    {
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

        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                FillControls();
                FillAgentEstateDetail();
            }
        }

        private void FillAgentEstateDetail()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var agentEstateRL = dc.dbRntAgentEstateRLs.FirstOrDefault(x => x.pidAgent == agentAuth.CurrentID && x.pidEstate == IdEstate);
                if (agentEstateRL != null)
                    txt_estateTitle_agent.Text = agentEstateRL.estateTitle;
            }
        }
 
        private void SaveAgentEstateDetail()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var agentEstateRL = dc.dbRntAgentEstateRLs.FirstOrDefault(x => x.pidAgent == agentAuth.CurrentID && x.pidEstate == IdEstate);
                if (agentEstateRL == null)
                {
                    agentEstateRL = new dbRntAgentEstateRL();
                    agentEstateRL.pidAgent = agentAuth.CurrentID;
                    agentEstateRL.pidEstate = IdEstate;
                    dc.Add(agentEstateRL);
                    dc.SaveChanges();
                }
                agentEstateRL.estateTitle = txt_estateTitle_agent.Text;
                dc.SaveChanges();
            }
        }
     
        protected void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            ltr_apartment.Text = CurrentSource.rntEstate_title(IdEstate, App.LangID, "");
           
            //drp_pidSeasonGroup.setSelectedValue(_currTBL.pidSeasonGroup);  
            dbRntAgentTBL agent = new dbRntAgentTBL();
            using (DCmodRental dc = new DCmodRental())
            {
                //lblSeason.Text = dc.dbRntSeasonGroupTBLs.FirstOrDefault(x => x.id == _currTBL.pidSeasonGroup).code;
                agent = dc.dbRntAgentTBLs.FirstOrDefault(x=>x.id == agentAuth.CurrentID);                
            }
            if (agent == null)
                return;

            int Wl_changeAmount = agent.WL_changeAmount.objToInt32();
            int WL_changeIsDiscount = agent.WL_changeIsDiscount == null ? 0 : agent.WL_changeIsDiscount.objToInt32();
            int WL_changeIsPercentage = agent.WL_changeIsPercentage == null ? 1 : agent.WL_changeIsPercentage.objToInt32();

            int Wl_changeAmount_Agent = agent.WL_changeAmount_Agent.objToInt32();
            int WL_changeIsDiscount_Agent = agent.WL_changeIsDiscount_Agent == null ? 0 : agent.WL_changeIsDiscount_Agent.objToInt32();
            int WL_changeIsPercentage_Agent = agent.WL_changeIsPercentage_Agent == null ? 1 : agent.WL_changeIsPercentage_Agent.objToInt32(); 

            lbl_pr_basePersons.Text = _currTBL.pr_basePersons.objToInt32().ToString();

            decimal price_1 = GetChangedAmt(_currTBL.pr_1_2pax.objToDecimal(), Wl_changeAmount, WL_changeIsDiscount, WL_changeIsPercentage);
            lbl_price_1.Text = price_1.ToString("N2");
            lbl_price_1_agent.Text = GetChangedAmt(price_1, Wl_changeAmount_Agent, WL_changeIsDiscount_Agent, WL_changeIsPercentage_Agent).ToString("N2");

            decimal price_optional_1 = GetChangedAmt(_currTBL.pr_1_opt.objToDecimal(), Wl_changeAmount, WL_changeIsDiscount, WL_changeIsPercentage);
            lbl_price_optional_1.Text = price_optional_1.ToString("N2");
            lbl_price_optional_1_agent.Text = GetChangedAmt(price_optional_1, Wl_changeAmount_Agent, WL_changeIsDiscount_Agent, WL_changeIsPercentage_Agent).ToString("N2");

            decimal price_2 = GetChangedAmt(_currTBL.pr_2_2pax.objToDecimal(), Wl_changeAmount, WL_changeIsDiscount, WL_changeIsPercentage);
            lbl_price_2.Text = price_2.ToString("N2");
            lbl_price_2_agent.Text = GetChangedAmt(price_2, Wl_changeAmount_Agent, WL_changeIsDiscount_Agent, WL_changeIsPercentage_Agent).ToString("N2");

            decimal price_optional_2 = GetChangedAmt(_currTBL.pr_2_opt.objToDecimal(), Wl_changeAmount, WL_changeIsDiscount, WL_changeIsPercentage);
            lbl_price_optional_2.Text = price_optional_2.ToString("N2");
            lbl_price_optional_2_agent.Text = GetChangedAmt(price_optional_2, Wl_changeAmount_Agent, WL_changeIsDiscount_Agent, WL_changeIsPercentage_Agent).ToString("N2");

            decimal price_3 = GetChangedAmt(_currTBL.pr_3_2pax.objToDecimal(), Wl_changeAmount, WL_changeIsDiscount, WL_changeIsPercentage);
            lbl_price_3.Text = price_3.ToString("N2");
            lbl_price_3_agent.Text = GetChangedAmt(price_3, Wl_changeAmount_Agent, WL_changeIsDiscount_Agent, WL_changeIsPercentage_Agent).ToString("N2");

            decimal price_optional_3 = GetChangedAmt(_currTBL.pr_3_opt.objToDecimal(), Wl_changeAmount, WL_changeIsDiscount, WL_changeIsPercentage);
            lbl_price_optional_3.Text = price_optional_3.ToString("N2");
            lbl_price_optional_3_agent.Text = GetChangedAmt(price_optional_3, Wl_changeAmount_Agent, WL_changeIsDiscount_Agent, WL_changeIsPercentage_Agent).ToString("N2");

            decimal price_4 = GetChangedAmt(_currTBL.pr_4_2pax.objToDecimal(), Wl_changeAmount, WL_changeIsDiscount, WL_changeIsPercentage);
            lbl_price_4.Text = price_4.ToString("N2");
            lbl_price_4_agent.Text = GetChangedAmt(price_4, Wl_changeAmount_Agent, WL_changeIsDiscount_Agent, WL_changeIsPercentage_Agent).ToString("N2");

            decimal price_optional_4 = GetChangedAmt(_currTBL.pr_4_opt.objToDecimal(), Wl_changeAmount, WL_changeIsDiscount, WL_changeIsPercentage);
            lbl_price_optional_4.Text = price_optional_4.ToString("N2");
            lbl_price_optional_4_agent.Text = GetChangedAmt(price_optional_4, Wl_changeAmount_Agent, WL_changeIsDiscount_Agent, WL_changeIsPercentage_Agent).ToString("N2");
           

            lbl_lm_inhours.Text = _currTBL.lm_inhours.objToInt32().ToString();
            lbl_lm_discount.Text = _currTBL.lm_discount.objToInt32().ToString();
            lbl_lm_nights_min.Text = _currTBL.lm_nights_min.objToInt32().ToString();
            lbl_lm_nights_max.Text = _currTBL.lm_nights_max.objToInt32().ToString();

            #region 
            //lbl_pr_discount7days.Text = _currTBL.pr_discount7days.objToInt32().ToString();
            //lbl_pr_discount30days.Text = _currTBL.pr_discount30days.objToInt32().ToString();

            //if(_currTBL.pr_dcSUsed == 1)
            //{
            //    lbl_pr_dcSUsed.Text = contUtils.getLabel("lblDisStandard");
            //}
            //else if(_currTBL.pr_dcSUsed == 2)
            //{
            //    lbl_pr_dcSUsed.Text = contUtils.getLabel("lblDisNewPers");
            //}

            //pnl_dcSUsed_1.Visible = (_currTBL.pr_dcSUsed == 1);
            //pnl_dcSUsed_2.Visible = !pnl_dcSUsed_1.Visible;

            //lbl_pr_dcS2_1_inDays.Text = _currTBL.pr_dcS2_1_inDays.objToInt32().ToString();
            //lbl_pr_dcS2_1_percent.Text = _currTBL.pr_dcS2_1_percent.objToInt32().ToString();
            //lbl_pr_dcS2_2_inDays.Text = _currTBL.pr_dcS2_2_inDays.objToInt32().ToString();
            //lbl_pr_dcS2_2_percent.Text = _currTBL.pr_dcS2_2_percent.objToInt32().ToString();
            //lbl_pr_dcS2_3_inDays.Text = _currTBL.pr_dcS2_3_inDays.objToInt32().ToString();
            //lbl_pr_dcS2_3_percent.Text = _currTBL.pr_dcS2_3_percent.objToInt32().ToString();
            //lbl_pr_dcS2_4_inDays.Text = _currTBL.pr_dcS2_4_inDays.objToInt32().ToString();
            //lbl_pr_dcS2_4_percent.Text = _currTBL.pr_dcS2_4_percent.objToInt32().ToString();
            //lbl_pr_dcS2_5_inDays.Text = _currTBL.pr_dcS2_5_inDays.objToInt32().ToString();
            //lbl_pr_dcS2_5_percent.Text = _currTBL.pr_dcS2_5_percent.objToInt32().ToString();
            //lbl_pr_dcS2_6_inDays.Text = _currTBL.pr_dcS2_6_inDays.objToInt32().ToString();
            //lbl_pr_dcS2_6_percent.Text = _currTBL.pr_dcS2_6_percent.objToInt32().ToString();
            //lbl_pr_dcS2_7_inDays.Text = _currTBL.pr_dcS2_7_inDays.objToInt32().ToString();
            //lbl_pr_dcS2_7_percent.Text = _currTBL.pr_dcS2_7_percent.objToInt32().ToString(); 
            #endregion
        }

        protected decimal GetChangedAmt(decimal amt, int changeAmt, int changeIsDiscount,int changeIsPercentage)
        {
            decimal finalChangeAmt = 0;
            if (changeIsPercentage == 1)
            {
                finalChangeAmt = ((amt * changeAmt) / 100);
            }
            else
            {
                finalChangeAmt = changeAmt;
            }
            finalChangeAmt = (changeIsDiscount == 1) ? -finalChangeAmt : finalChangeAmt;
            return amt + finalChangeAmt;
        }


        protected void lnkSave_Click(object sender, EventArgs e)
        {
            SaveAgentEstateDetail();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            FillAgentEstateDetail();
        }
    }
}