using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace ModRental.admin.modRental
{
    public partial class utility : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    if (Request.QueryString["action"] == "prTotalRate")
                    {
                        //var tmpList = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.pr_total.HasValue && x.pr_total.Value > 0 && (!x.prTotalRate.HasValue || x.prTotalRate == 0)).ToList();
                        //foreach (var tmp in tmpList)
                        //{
                        //    tmp.prTotalRate = tmp.pr_total.objToDecimal() 
                        //        + tmp.pr_discount_commission.objToDecimal() 
                        //        + tmp.pr_discount_owner.objToDecimal()
                        //        + tmp.prDiscountSpecialOffer.objToDecimal()
                        //        + tmp.prDiscountLastMinute.objToDecimal()
                        //        + tmp.prDiscountLongStay.objToDecimal()
                        //        + tmp.prDiscountLongRange.objToDecimal()
                        //        - tmp.pr_part_agency_fee.objToDecimal()
                        //        - tmp.pr_srsPrice.objToDecimal() 
                        //        - tmp.pr_ecoPrice.objToDecimal();
                        //    if (tmp.agentDiscountNotPayed.objToInt32() == 1)
                        //        tmp.prTotalRate += tmp.agentCommissionPrice.objToDecimal();
                        //    tmp.prTotalCommission = tmp.pr_part_payment_total.objToDecimal() - tmp.pr_part_agency_fee.objToDecimal();
                        //    tmp.prTotalOwner = tmp.pr_part_owner.objToDecimal() - tmp.pr_srsPrice.objToDecimal() - tmp.pr_ecoPrice.objToDecimal();
                        //    DC_RENTAL.SubmitChanges();
                        //}
                        //Response.Write("ok:" + tmpList.Count);
                    }
                }
            }
        }
    }
}