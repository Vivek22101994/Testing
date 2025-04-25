using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;

namespace RentalInRome.affiliatesarea
{
    public partial class Default : agentBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                fillData();
        }
        protected void fillData()
        {
            string AgentDiscount_layoutTemplate = ltrAgentDiscount_layoutTemplate.Text.Replace("#mese#", CurrentSource.getSysLangValue("lblMonth")).Replace("#currSumLabel#", CurrentSource.getSysLangValue("lblReservationsTotalSum")).Replace("#currSumDesc#", CurrentSource.getSysLangValue("lblComprensivoDiQuestaPren")).Replace("#lblCommissions#", CurrentSource.getSysLangValue("lblCommissions"));
            string AgentDiscount_uniqueTemplate = ltrAgentDiscount_itemTemplate.Text.Replace("#toChange#", CurrentSource.getSysLangValue("lblFixedDiscount"));
            string AgentDiscount_startTemplate = ltrAgentDiscount_itemTemplate.Text.Replace("#toChange#", CurrentSource.getSysLangValue("lblUpTo") + " #end#");
            string AgentDiscount_middleTemplate = ltrAgentDiscount_itemTemplate.Text.Replace("#toChange#", CurrentSource.getSysLangValue("lblFrom") + " #start# " + CurrentSource.getSysLangValue("lblTo") + " #end#");
            string AgentDiscount_endTemplate = ltrAgentDiscount_itemTemplate.Text.Replace("#toChange#", CurrentSource.getSysLangValue("lblFrom") + " #start#");
            int agentDiscountType = 1;
            decimal agentTotalResPrice = 0;
            using (DCmodRental dc = new DCmodRental())
            {
                var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                if (agentTBL != null)
                {
                    agentDiscountType = agentTBL.pidDiscountType.objToInt32();
                    if (agentDiscountType == 0) agentDiscountType = 1;
                    DateTime checkDate = DateTime.Now;
                    DateTime checkDateStart = new DateTime(checkDate.Year, checkDate.Month, 1);
                    DateTime checkDateEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
                    using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                    {
                        var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == agentTBL.id && x.dtCreation >= checkDateStart && x.dtCreation < checkDateEnd && x.pr_total.HasValue).ToList();
                        agentTotalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
                    }
                }
            }

            ltr_stampaRiepilogo.Text = rntUtils.getDiscountType_details(agentDiscountType, agentTotalResPrice, DateTime.Now, CurrentLang.ID, "tuaComm", AgentDiscount_uniqueTemplate, AgentDiscount_startTemplate, AgentDiscount_middleTemplate, AgentDiscount_endTemplate, AgentDiscount_layoutTemplate);
        }
    }
}
