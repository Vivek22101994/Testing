using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateChnlExpedia_manageRateplans : adminBasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                ucNav.IdEstate = IdEstate;

                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }

                //bind deadline dropdown
                for (int i = 0; i <= 999; i++)
                {
                   drp_deadline.Items.Add(new ListItem(i+"", i+""));
                }

                //stay fees insert
                var stayFess = CommonUtilities.getSYS_SETTING("rntExpediaStayFees").splitStringToList(",");
                foreach (var stayFee in stayFess)
                    drp_stay_fee.Items.Add(new ListItem(stayFee, stayFee));
            }
        }

        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currEstate == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }

            ChnlExpediaClasses.RatePlanRequest rateplan = new ChnlExpediaClasses.RatePlanRequest();
            rateplan.name = txt_name.Text;
            rateplan.rateAcquisitionType = drp_rateAcquisitionType.SelectedValue;
            var distributionRules = new List<ChnlExpediaClasses.distributionRules>();

            if (chk_expediaCollect.Checked)
            {
                var distributionRule = new ChnlExpediaClasses.distributionRules();
                distributionRule.distributionModel = "ExpediaCollect";
                distributionRule.partnerCode = txt_expediaCollect.Text;
                distributionRules.Add(distributionRule);
            }

            if (chk_hotelCollect.Checked)
            {
                var distributionRule = new ChnlExpediaClasses.distributionRules();
                distributionRule.distributionModel = "HotelCollect";
                distributionRule.partnerCode = txt_hotellCollect.Text;
                distributionRules.Add(distributionRule);
            }
            rateplan.distributionRules = distributionRules;
            rateplan.status = drp_status.SelectedValue;
            rateplan.type = drp_type.SelectedValue;
            rateplan.pricingModel = drp_pricingModel.SelectedValue;
            //rateplan.occupantsForBaseRate = currEstate.num_persons_max.objToInt32();
            rateplan.taxInclusive = false;
            rateplan.minLOSDefault = currEstate.nights_min.objToInt32();
            if (rateplan.minLOSDefault == 0)
                rateplan.minLOSDefault = 1;

            rateplan.maxLOSDefault = currEstate.nights_max.objToInt32();
            if (rateplan.maxLOSDefault == 0 || rateplan.maxLOSDefault > 28)
                rateplan.maxLOSDefault = 28;

            var cancelPolicy =  new ChnlExpediaClasses.cancelPolicy();
            var defaultPenalties = new List<ChnlExpediaClasses.defaultPenalties>();

            var defaultPenaltyZero = new ChnlExpediaClasses.defaultPenalties();
            defaultPenaltyZero.deadline = 0;
            defaultPenaltyZero.perStayFee = "FullCostOfStay";
            defaultPenaltyZero.amount = 0;
            defaultPenalties.Add(defaultPenaltyZero);

            var defaultPenalty = new ChnlExpediaClasses.defaultPenalties();
            defaultPenalty.deadline = drp_deadline.getSelectedValueInt();
            defaultPenalty.perStayFee = drp_stay_fee.SelectedValue;
            defaultPenalty.amount = 0;
            defaultPenalties.Add(defaultPenalty);
            cancelPolicy.defaultPenalties = defaultPenalties;

            rateplan.cancelPolicy = cancelPolicy;
            ChnlExpediaUpdate.RatePlanCreate_start(rateplan, currEstate.id);

            txt_name.Text = "";
            chk_expediaCollect.Checked = false;
            chk_hotelCollect.Checked = false;
            drp_type.setSelectedValue("NetRate");
            drp_status.setSelectedValue("Active");
            drp_type.setSelectedValue("Standalone");

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"create rateplans in progress ..\", 340, 110);", true);
        }

        protected void lnk_list_Click(object sender, EventArgs e)
        {
            Response.Redirect("EstateChnlExpedia_ratePlans.aspx");
        }
    }
}