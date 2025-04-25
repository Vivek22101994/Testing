using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class testHA : System.Web.UI.Page
    {
        magaRental_DataContext DC_RENTAL;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int IdEstate = Request.QueryString["id"].objToInt32();
                int isWL = Request.QueryString["isWL"].objToInt32();
                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                outPrice.dtStart = new DateTime(2015,10,9);
                outPrice.dtEnd = new DateTime(2015, 10, 12);
                outPrice.dtCount = 3;
                outPrice.numPersCount = 2;
                outPrice.numPers_adult = 2;
                outPrice.numPers_childOver = 0;
                outPrice.numPers_childMin = 0;
                outPrice.pr_discount_owner = 0;
                outPrice.pr_discount_commission = 0;
                RNT_TB_ESTATE currEstateTB = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                outPrice.part_percentage = currEstateTB.pr_percentage.objToDecimal();


                if (isWL == 1)
                {
                    outPrice.isWL = 1;
                    using (DCmodRental dc = new DCmodRental())
                        outPrice.fillAgentDetails(dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == App.WLAgentId));
                }
                else
                {
                    using (DCmodRental dc = new DCmodRental())
                        outPrice.fillAgentDetails(dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == 1159));
                }

                decimal _pr_total = rntUtils.rntEstate_getPrice(0, IdEstate, ref outPrice);
            }
        }

        protected void lnk_update_estates_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
                {
                    //List<RNT_TB_HomeAway_Estate> lstHAEstates = maga_DataContext.DC_RENTAL.RNT_TB_HomeAway_Estate.ToList();                    
                    List<RNT_TB_ESTATE> lstEstates = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_HomeAway == 1).ToList();

                    //dbRntAgentTBL currAgent = dc.dbRntAgentTBLs.FirstOrDefault(x => x.IdAdMedia != null && x.IdAdMedia.ToLower() == "ha".ToLower());
                    dbRntAgentTBL currAgent = dc.dbRntAgentTBLs.FirstOrDefault(x => x.isActive == 1 && x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia);
                    if (currAgent == null)
                    {
                        Response.End();
                        return;
                    }

                    //List<int> lstId = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == currAgent.id).Select(x => x.pidEstate).ToList();
                    //var _estateList = AppSettings.RNT_estateList.Where(x => lstId.Contains(x.id)).ToList();                   
                    var _estateList = AppSettings.RNT_estateList.ToList();
                    foreach (var objEstate in _estateList)
                    {
                        var currHomeaway = dcChnl.dbRntChnlHomeAwayEstateTBs.SingleOrDefault(x => x.id == objEstate.id);
                        if (currHomeaway == null)
                        {
                            RNT_TB_ESTATE currEstate = lstEstates.SingleOrDefault(x => x.id == objEstate.id);
                            if (currEstate != null)
                                ChnlHomeAwayUtils_V411.updateEstateFromMagarental(currEstate);

                            //RNT_TB_HomeAway_Estate currOldHA = lstHAEstates.SingleOrDefault(x => x.pid_estate == currEstate.id);
                            //if (currOldHA != null)
                            //{
                            //    var currHomeaway1 = dcChnl.dbRntChnlHomeAwayEstateTBs.SingleOrDefault(x => x.id == objEstate.id);
                            //    if (currHomeaway1 != null)
                            //    {
                            //        currHomeaway1.is_slave = currOldHA.is_slave.objToInt32();
                            //        RNT_TB_HomeAway_Estate currOldHAMaster = lstHAEstates.SingleOrDefault(x => x.id == currOldHA.pid_master_estate);
                            //        if (currOldHAMaster != null)
                            //            currHomeaway1.pid_master_estate = currOldHAMaster.pid_estate.objToInt32();
                            //        dcChnl.SaveChanges();
                            //    }
                            //}
                        }
                    }



                    ////for cloud modification
                    //foreach (var objEstate in _estateList)
                    //{
                    //    RNT_TB_HomeAway_Estate currOldHA = lstHAEstates.SingleOrDefault(x => x.pid_estate == objEstate.id);
                    //    if (currOldHA != null)
                    //    {
                    //        var currHomeaway1 = dcChnl.dbRntChnlHomeAwayEstateTBs.SingleOrDefault(x => x.id == objEstate.id);
                    //        if (currHomeaway1 != null)
                    //        {
                    //            currHomeaway1.is_slave = currOldHA.is_slave;
                    //            RNT_TB_HomeAway_Estate currOldHAMaster = lstHAEstates.SingleOrDefault(x => x.id == currOldHA.pid_master_estate);
                    //            if (currOldHAMaster != null)
                    //                currHomeaway1.pid_master_estate = currOldHAMaster.pid_estate;
                    //            dcChnl.SaveChanges();
                    //        }
                    //    }

                    //}

                }
            }
        }
    }
}