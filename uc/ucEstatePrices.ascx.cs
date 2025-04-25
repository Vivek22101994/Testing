using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.uc
{
    public partial class ucEstatePrices : System.Web.UI.UserControl
    {
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = string.Empty.createUniqueID();
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

        public decimal markup
        {
            get
            {
                return Hf_markup.Value.ToInt32();
            }
            set
            {
                Hf_markup.Value = value.ToString();
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
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                fillData();
            }
        }
        public void fillData()
        {
            List<RNT_VIEW_SPECIAL_OFFER> _soList = AppSettings.RNT_VIEW_SPECIAL_OFFERs.Where(x => x.dtPublicStart <= DateTime.Now.Date && x.dtPublicEnd >= DateTime.Now.Date && x.pid_lang == CurrentLang.ID && x.pid_estate == IdEstate && x.is_active == 1).OrderBy(x => x.dtEnd).ToList();
            LV_special_offer.DataSource = _soList;
            LV_special_offer.DataBind();

            bool addMarkUp = (markup > 0);

            decimal _pr_discount7daysApply = 1 - (currEstateTB.pr_discount7days.objToDecimal() / 100);
            decimal _pr_discount30daysApply = 1 - (currEstateTB.pr_discount30days.objToDecimal() / 100);
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
                if (addMarkUp)
                    _prTemp = _prTemp + (_prTemp * (markup / 100));
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
                if (addMarkUp)
                    _prTemp = _prTemp + (_prTemp * (markup / 100));
                _prStr = _prStr.Replace("#pr_2#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_2_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");

                // very high
                _prTemp = currEstateTB.pr_3_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_3_opt.objToDecimal());
                if (addMarkUp)
                    _prTemp = _prTemp + (_prTemp * (markup / 100));
                _prStr = _prStr.Replace("#pr_3#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_3_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _priceDetails += _prStr;
            }
            ltr_priceDetails.Text = _priceDetails;

            // tabella delle stagioni
            var seasonGroup = currEstateTB.pidSeasonGroup.objToInt32();
            var seasonDateList = new List<dbRntSeasonDatesTBL>();
            using (DCmodRental dc = new DCmodRental())
                seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= DateTime.Now && x.dtStart <= DateTime.Now.AddYears(1) && x.pidSeasonGroup == seasonGroup).OrderBy(x => x.dtStart).ToList();

            LVseasonDates_1.DataSource = seasonDateList.Where(x => x.pidPeriod == 1);
            LVseasonDates_1.DataBind();
            LVseasonDates_2.DataSource = seasonDateList.Where(x => x.pidPeriod == 2);
            LVseasonDates_2.DataBind();
            LVseasonDates_3.DataSource = seasonDateList.Where(x => x.pidPeriod == 3);
            LVseasonDates_3.DataBind();
            LVseasonDates_4.DataSource = seasonDateList.Where(x => x.pidPeriod == 4);
            LVseasonDates_4.DataBind();
        }
        protected void LV_special_offer_DataBound(object sender, EventArgs e)
        {
            Literal _ltr = LV_special_offer.FindControl("ltr_title") as Literal;
            if (_ltr != null) _ltr.Text = CurrentSource.getSysLangValue("lblSpecialOffersAndLastMinute");
        }
    }
}