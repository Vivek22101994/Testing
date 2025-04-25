using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;
using System.IO;
using Newtonsoft.Json;

namespace RentalInRome.webservice
{
    public partial class rntEstateCheckAvv : System.Web.UI.Page
    {

        public class SpecialOfferClass
        {
            public long id { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string amount { get; set; }
            public string symbol { get; set; }
            public SpecialOfferClass()
            {
                id = 0;
                start = "";
                end = "";
                amount = "";
                symbol = "";
            }
        }
        public class CurrResponseClass
        {
            public string error { get; set; }
            public bool isAvv { get; set; }
            public bool hasPrice { get; set; }
            public bool isInstantBooking { get; set; }
            public string prTotal { get; set; }
            public string prFull { get; set; }
            public string prExtra { get; set; }
            public string prWithoutDiscount { get; set; }
            public string prDaily { get; set; }
            public string prAgencyFee { get; set; }
            public string prEco { get; set; }
            public SpecialOfferClass priceChange { get; set; }
            public rntExts.PreReservationPrices outPrice { get; set; }
            public CurrResponseClass()
            {
                error = "";
                isAvv = true;
                hasPrice = true;
                isInstantBooking = false;
                prTotal = "";
                prWithoutDiscount = "";
                prDaily = "";
                priceChange = new SpecialOfferClass();
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private string CURRENT_SESSION_ID;
        private int IdEstate;
        private int _currLang;
        private long agentID;
        private bool IsAffiliatesarea;
        private string _action;
        private clSearch curr_ls;
        private List<rntExts.RNT_estatePriceDetails> _priceDetails;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/html";
                CURRENT_SESSION_ID = Request.QueryString["SESSION_ID"];
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                curr_ls = _config.lastSearch;
                var CurrRes = new CurrResponseClass();

                DateTime TMPdtStart = curr_ls.dtStart;
                DateTime TMPdtEnd = curr_ls.dtEnd;
                int TMPnumPersCount = curr_ls.numPersCount;
                IdEstate = Request.QueryString["IdEstate"].objToInt32();
                RNT_TB_ESTATE currEstateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstateTB == null)
                {
                    Response.Write("error");
                    Response.End();
                }
                _currLang = Request.QueryString["lang"].objToInt32() == 0 ? CurrentLang.ID : Request.QueryString["lang"].objToInt32();
                App.LangID = _currLang;
                _action = Request.QueryString["action"] + "";
                agentID = Request.QueryString["agentID"].objToInt64();
                IsAffiliatesarea = Request.QueryString["affiliatesarea"] == "true";
                int dtStartInt = Request.QueryString["dtS"].objToInt32();
                int dtEndInt = Request.QueryString["dtE"].objToInt32();
                if (dtStartInt != 0 && dtEndInt != 0)
                {
                    curr_ls.dtStart = dtStartInt.JSCal_intToDate();
                    curr_ls.dtEnd = dtEndInt.JSCal_intToDate();
                }
                curr_ls.dtCount = (curr_ls.dtEnd - curr_ls.dtStart).TotalDays.objToInt32();
                curr_ls.numPers_adult = Request.QueryString["numPers_adult"].objToInt32() == 0 ? curr_ls.numPers_adult : Request.QueryString["numPers_adult"].objToInt32();
                
                //0 = Request.QueryString["numPers_childOver"].objToInt32() == 0 ? 0 : Request.QueryString["numPers_childOver"].objToInt32();
                curr_ls.numPers_childOver = Request.QueryString["numPers_childOver"].objToInt32() == 0 ? 0 : Request.QueryString["numPers_childOver"].objToInt32();
                
                curr_ls.numPers_childMin = Request.QueryString["numPers_childMin"].objToInt32() == 0 ? 0 : Request.QueryString["numPers_childMin"].objToInt32();
                
                //curr_ls.numPersCount = curr_ls.numPers_adult + 0;
                curr_ls.numPersCount = curr_ls.numPers_adult + curr_ls.numPers_childOver;

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
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                        if (agentTBL != null)
                            outPrice.fillAgentDetails(agentTBL);
                    }
                }
                //CurrRes.prFull = rntUtils.rntEstate_getPrice(0, IdEstate, ref outPrice).ToString("N2");
                CurrRes.prTotal = rntUtils.rntEstate_getPrice(0, IdEstate, ref outPrice).ToString("N2");

                CurrRes.outPrice = outPrice;

                CurrRes.prAgencyFee = outPrice.pr_part_agency_fee.ToString("N2");
                CurrRes.prEco = outPrice.ecoPrice.ToString("N2");

                if (outPrice.prDiscountSpecialOffer > 0)
                    CurrRes.prWithoutDiscount = (CurrRes.prTotal.objToDecimal() + outPrice.prDiscountSpecialOffer + outPrice.prDiscountLongStay).ToString("N2");
                CurrRes.prDaily = decimal.Divide(CurrRes.prTotal.objToDecimal(), curr_ls.dtCount).ToString("N2");
                bool _isAvailable = DC_RENTAL.RNT_TBL_RESERVATION.Where(y => y.pid_estate == IdEstate //
                                                                                && y.state_pid != 3 //
                                                                                && y.dtStart.HasValue //
                                                                                && y.dtEnd.HasValue //
                                                                                && ((y.dtStart.Value.Date <= curr_ls.dtStart && y.dtEnd.Value.Date >= curr_ls.dtEnd) //
                                                                                    || (y.dtStart.Value.Date >= curr_ls.dtStart && y.dtStart.Value.Date < curr_ls.dtEnd) //
                                                                                    || (y.dtEnd.Value.Date > curr_ls.dtStart && y.dtEnd.Value.Date <= curr_ls.dtEnd))).Count() == 0;

                CurrRes.isAvv = _isAvailable;
                CurrRes.hasPrice = CurrRes.prTotal.objToDecimal() > 0;
                CurrRes.isInstantBooking = currEstateTB.is_online_booking == 1;



                string resStr = "";
                string errorStr = "";
                if (!CurrRes.isAvv || !CurrRes.hasPrice)
                {
                    CurrRes.error = "This apartment is not available for the selected dates and number of people. ";
                }

                TextWriter wr = new StringWriter();
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(wr, CurrRes);
                Response.Write(wr.ToString());
                Response.End();
            }
        }

    }
}
