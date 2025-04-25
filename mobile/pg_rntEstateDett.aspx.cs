using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.mobile
{
    public partial class pg_rntEstateDett : mainBasePage
    {
        magaRental_DataContext DC_RENTAL = new magaRental_DataContext();
        magaLocation_DataContext DC_LOC;

        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }

        public int IdEstate
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
            }
        }

        private RNT_TB_ESTATE TMPcurrEstateTB;
        public RNT_TB_ESTATE currEstateTB
        {
            get
            {
                DC_RENTAL = new magaRental_DataContext();
                if (TMPcurrEstateTB == null)
                    TMPcurrEstateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                return TMPcurrEstateTB ?? new RNT_TB_ESTATE();
            }
            set { TMPcurrEstateTB = value; }
        }

        public RNT_LN_ESTATE currEstateLN
        {
            get
            {
                DC_RENTAL = new magaRental_DataContext();
                if (TMPlnEstate == null)
                    TMPlnEstate = DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == CurrentLang.ID);
                return TMPlnEstate ?? new RNT_LN_ESTATE();
            }
            set { TMPlnEstate = value; }
        }
        private RNT_LN_ESTATE TMPlnEstate;

        public int Num_guestBook
        {
            get
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    return dc.dbRntEstateCommentsTBLs.Where(x => x.isActive == 1 && x.pidEstate == IdEstate).Count();
                }
            }
        }

        protected string IMG_ROOT
        {
            get { return Request.Url.AbsoluteUri.Contains("http://localhost") ? App.HOST + "/" : "/"; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "rntEstateDett";
            int id = Request.QueryString["id"].ToInt32();
            if (id != 0)
                base.PAGE_REF_ID = id;
            else
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "rntEstateDett", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();


            clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
            clSearch _ls = _config.lastSearch;
            _config.addTo_myLastVisitedEstateList(PAGE_REF_ID);
            int _dtStartInt = Request.QueryString["dtS"].objToInt32();
            int _dtEndInt = Request.QueryString["dtE"].objToInt32();
            if (_dtStartInt != 0 && _dtEndInt != 0)
            {
                _ls.dtStart = _dtStartInt.JSCal_intToDate();
                _ls.dtEnd = _dtEndInt.JSCal_intToDate();
            }
            clUtils.saveConfig(_config);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                ucBooking.IdEstate = IdEstate;
                FillData();
            }

        }

        protected void FillData()
        {
            DC_RENTAL = new magaRental_DataContext();
            DC_LOC = new magaLocation_DataContext();
            currEstateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);

            if (currEstateTB == null)
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "Apt non attivo id=" + PAGE_REF_ID, _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=g1");
                return;
            }
            if (currEstateTB.is_online_booking != 1)
            {
                Response.Redirect("/m" + CurrentSource.getPagePath("6", "stp", App.LangID.ToString()));
            }


            if (IdEstate > 0)
            {
                try
                {
                    List<int> listConfigIDs = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.is_HomeAway == 0).Select(x => x.pid_config).ToList();
                    LV_configs.DataSource = DC_RENTAL.RNT_TB_CONFIGs.Where(x => listConfigIDs.Contains(x.id)).ToList();
                    LV_configs.DataBind();

                }
                catch (Exception ex) { }
            }


            fillPrice_v1();

            // fill Special Offers
            List<RNT_VIEW_SPECIAL_OFFER> _soList = AppSettings.RNT_VIEW_SPECIAL_OFFERs.Where(x => x.dtPublicStart <= DateTime.Now.Date && x.dtPublicEnd >= DateTime.Now.Date && x.dtEnd > DateTime.Now.AddDays(5) && x.pid_lang == CurrentLang.ID && x.pid_estate == IdEstate && x.is_active == 1).OrderBy(x => x.dtEnd).ToList();
            LV_special_offer.DataSource = _soList;
            LV_special_offer.DataBind();
            Literal LV_special_offer_ltr = LV_special_offer.FindControl("ltr_title") as Literal;
            if (LV_special_offer_ltr != null) LV_special_offer_ltr.Text = CurrentSource.getSysLangValue("lblSpecialOffersAndLastMinute");

            //if (estatePrice.prSeason1 == 0 && estatePrice.prSeason2 == 0 && estatePrice.prSeason3 == 0)
            //{
            //    //pnl_pricePreviewCont.Visible = false; // pricepreview NOT visible
            //    PH_priceOnRequestCont.Visible = true;
            //    PH_priceListCont.Visible = false;
            //}
            //else
            //{
            //    //pnl_pricePreviewCont.Visible = true; // pricepreview visible
            //    PH_priceOnRequestCont.Visible = false;
            //    PH_priceListCont.Visible = true;
            //}




        }
        protected void fillPrice_v1()
        {
            PH_priceListCont_v1.Visible = true;
            PH_priceListCont_v2.Visible = false;
            decimal _pr_discount7daysApply = 1 - (currEstateTB.pr_discount7days.objToDecimal() / 100);
            decimal _pr_discount30daysApply = 1 - (currEstateTB.pr_discount30days.objToDecimal() / 100);
            decimal _minPrice = rntUtils.rntEstate_minPrice(IdEstate);
            decimal _tmpPriceWeek = _minPrice * 7 * _pr_discount7daysApply;
            _tmpPriceWeek = _tmpPriceWeek.customRound(true);
            string _priceDetails = "";
            decimal _prTemp = 0;
            int pr_basePersons = currEstateTB.pr_basePersons.objToInt32();
            for (int i = pr_basePersons; i <= currEstateTB.num_persons_max.objToInt32(); i++)
            {
                int extraPersons = i - pr_basePersons;
                string numPers_string = extraPersons == 0 && currEstateTB.num_persons_min.objToInt32() < pr_basePersons ? i + " " + CurrentSource.getSysLangValue("lblPax") + " or less" : i + " " + CurrentSource.getSysLangValue("lblPax");
                string _prStr = ltr_priceTemplate.Text.Replace("#num_pers#", numPers_string);

                // low
                _prTemp = currEstateTB.pr_1_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_1_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_1#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_1_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");

                // Medium
                _prTemp = currEstateTB.pr_4_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_4_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_4#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_4_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");

                // hight
                _prTemp = currEstateTB.pr_2_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_2_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_2#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_2_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                // very hight
                _prTemp = currEstateTB.pr_3_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_3_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_3#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_3_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _priceDetails += _prStr;
            }
            ltr_priceDetails.Text = _priceDetails;

            // tabella delle stagioni
            var periodDatesList = new List<dbRntSeasonDatesTBL>();
            using (DCmodRental dc = new DCmodRental())
                periodDatesList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= DateTime.Now && x.dtStart < DateTime.Now.AddYears(1) && x.pidSeasonGroup == currEstateTB.pidSeasonGroup.objToInt32()).OrderBy(x => x.dtStart).ToList();
            /**/
            //LVseasonDates.DataSource = periodDatesList;
            //LVseasonDates.DataBind();

            LVseasonDates_1.DataSource = periodDatesList.Where(x => x.pidPeriod == 1);
            LVseasonDates_1.DataBind();
            LVseasonDates_2.DataSource = periodDatesList.Where(x => x.pidPeriod == 2);
            LVseasonDates_2.DataBind();
            LVseasonDates_3.DataSource = periodDatesList.Where(x => x.pidPeriod == 3);
            LVseasonDates_3.DataBind();
            LVseasonDates_4.DataSource = periodDatesList.Where(x => x.pidPeriod == 4);
            LVseasonDates_4.DataBind();

            string longStayDiscount = "";
            if (currEstateTB.pr_dcSUsed == 1 && currEstateTB.pr_discount7days.objToInt32() > 0 && contUtils.getLabel_title("lbl_discount7daysDesc", App.LangID, "") != "")
            {
                longStayDiscount += "<tr><td colspan=\"7\"><strong>" + contUtils.getLabel_title("lbl_discount7daysDesc", App.LangID, "").Replace("#discount#", "" + currEstateTB.pr_discount7days.objToInt32()) + "</strong></td></tr>";
            }
            else if (currEstateTB.pr_dcSUsed == 2)
            {
                string tmpSep = "";
                if (currEstateTB.pr_dcS2_1_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_1_percent.objToInt32() > 0)
                {
                    longStayDiscount += tmpSep + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", App.LangID, "").Replace("#discount#", "" + currEstateTB.pr_dcS2_1_percent.objToInt32()).Replace("#nights#", "" + (currEstateTB.pr_dcS2_1_inDays.objToInt32() + 1));
                    tmpSep = " - ";
                }
                if (currEstateTB.pr_dcS2_2_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_2_percent.objToInt32() > 0)
                {
                    longStayDiscount += tmpSep + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", App.LangID, "").Replace("#discount#", "" + currEstateTB.pr_dcS2_2_percent.objToInt32()).Replace("#nights#", "" + (currEstateTB.pr_dcS2_2_inDays.objToInt32() + 1));
                    tmpSep = " - ";
                }
                if (currEstateTB.pr_dcS2_3_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_3_percent.objToInt32() > 0)
                {
                    longStayDiscount += tmpSep + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", App.LangID, "").Replace("#discount#", "" + currEstateTB.pr_dcS2_3_percent.objToInt32()).Replace("#nights#", "" + (currEstateTB.pr_dcS2_3_inDays.objToInt32() + 1));
                    tmpSep = " - ";
                }
                if (currEstateTB.pr_dcS2_4_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_4_percent.objToInt32() > 0)
                {
                    longStayDiscount += tmpSep + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", App.LangID, "").Replace("#discount#", "" + currEstateTB.pr_dcS2_4_percent.objToInt32()).Replace("#nights#", "" + (currEstateTB.pr_dcS2_4_inDays.objToInt32() + 1));
                    tmpSep = " - ";
                }
                if (currEstateTB.pr_dcS2_5_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_5_percent.objToInt32() > 0)
                {
                    longStayDiscount += tmpSep + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", App.LangID, "").Replace("#discount#", "" + currEstateTB.pr_dcS2_5_percent.objToInt32()).Replace("#nights#", "" + (currEstateTB.pr_dcS2_5_inDays.objToInt32() + 1));
                    tmpSep = " - ";
                }
                if (currEstateTB.pr_dcS2_6_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_6_percent.objToInt32() > 0)
                {
                    longStayDiscount += tmpSep + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", App.LangID, "").Replace("#discount#", "" + currEstateTB.pr_dcS2_6_percent.objToInt32()).Replace("#nights#", "" + (currEstateTB.pr_dcS2_6_inDays.objToInt32() + 1));
                    tmpSep = " - ";
                }
                if (currEstateTB.pr_dcS2_7_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_7_percent.objToInt32() > 0)
                {
                    longStayDiscount += tmpSep + contUtils.getLabel_title("lblX-DiscountFor-Y-nights", App.LangID, "").Replace("#discount#", "" + currEstateTB.pr_dcS2_7_percent.objToInt32()).Replace("#nights#", "" + (currEstateTB.pr_dcS2_7_inDays.objToInt32() + 1));
                    tmpSep = " - ";
                }
            }
            ltr_price_longStayDiscount.Text = longStayDiscount;
        }
    }
}