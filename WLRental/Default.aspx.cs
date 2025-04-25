using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.WLRental
{
    public partial class Default : mainBasePage
    {
        private static List<int> lastEstates;
        protected bool FIRST_ACCESS
        {
            get { if (HttpContext.Current.Session["FIRST_ACCESS"] == "false") { return false; } else { HttpContext.Current.Session["FIRST_ACCESS"] = "false"; return true; } }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            //int _lang =Request.QueryString["lang"].ToInt32();
            //if (_lang == 0)
            //{
            //    CurrentLang.ID = 2;
            //    CurrentLang.NAME = "en-GB";
            //    CurrentLang.ABBR = "eng";
            //    CurrentLang.TITLE = "English";
            //}
            CurrentLang.ID = 2;
            CurrentLang.NAME = "en-GB";
            CurrentLang.ABBR = "eng";
            CurrentLang.TITLE = "English";
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "stp";
            base.PAGE_REF_ID = 1;
            RewritePath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["force"] == "desktop")
                //    Session["force_desktop"] = "true";
                //if (Request.IsMobile() && Session["force_desktop"] != "true")
                //{
                //    Response.Redirect("/m" + CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()));
                //    return;
                //}
                Fill_data();
            }
        }
        protected void Fill_data()
        {
            CONT_VIEW_STP _stp = contProps.CONT_STPs.SingleOrDefault(x => x.id == PAGE_REF_ID && x.pid_lang == CurrentLang.ID);
            if (_stp != null)
            {
                ltr_meta_description.Text = _stp.meta_description;
                ltr_meta_keywords.Text = _stp.meta_keywords;
                ltr_meta_title.Text = _stp.meta_title;
                ltr_title.Text = _stp.title;
                ltr_sub_title.Text = _stp.sub_title;
                ltr_description.Text = _stp.description;
                ltr_summary.Text = _stp.summary;
            }
            List<RNT_VIEW_ESTATE_POSITION> _soList = AppSettings.RNT_VIEW_ESTATE_POSITION.Where(x => x.position == "homepage_attention" && x.pid_lang == CurrentLang.ID).OrderBy(x => x.sequence).ToList();
            LV_homepage_attention.DataSource = _soList;
            LV_homepage_attention.DataBind();

            //if (CurrentLang.ID == 1 || 1==1)
            //{
            //    PH_proprietariIta.Visible = true;
            //    PH_proprietariOtherLang.Visible = false;
            //}
            //else
            //{
            //    PH_proprietariIta.Visible = false;
            //    PH_proprietariOtherLang.Visible = true;
            //}
            fillSpecialOfferSelected();
        }
        protected void fillSpecialOfferSelected() 
        {
            int site_homePage_SpOfferCount = CommonUtilities.getSYS_SETTING("site_homePage_SpOfferCount").ToInt32();
            if (site_homePage_SpOfferCount == 0) site_homePage_SpOfferCount = 20;
            List<AppSettings.RNT_estate> spDiscountList = new List<AppSettings.RNT_estate>();

            List<RNT_TB_SPECIAL_OFFER> specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd > DateTime.Now.AddDays(1) && x.is_active == 1 && (HF_spDiscountCheckHomePage.Value == "0" || x.img_thumb == "1") && x.dtStart.HasValue && x.dtEnd.HasValue).OrderBy(x => x.dtEnd).ToList();
            if (specialOfferList.Count == 0) return;
            string currListStr = "";
            foreach (RNT_TB_SPECIAL_OFFER currOffer in specialOfferList)
            {
                if (spDiscountList.FirstOrDefault(x => x.id == currOffer.pid_estate) != null) continue;
                if (site_homePage_SpOfferCount == spDiscountList.Count) break;
                AppSettings.RNT_estate currEstate = AppSettings.RNT_estateList.SingleOrDefault(x => x.id == currOffer.pid_estate);
                if (currEstate == null) continue;
                RNT_LN_ESTATE currLN = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == currEstate.id && x.pid_lang == CurrentLang.ID);
                if (currLN == null) continue;
                currEstate.pid_lang = currLN.pid_lang;
                currEstate.title = currLN.title;
                currEstate.summary = currLN.summary;
                currEstate.page_path = currLN.page_path;
                currEstate.zone = CurrentSource.locZone_title(currEstate.pid_zone.objToInt32(), CurrentLang.ID, "");
                currEstate.spDiscountAmount = currOffer.pr_discount.objToInt32();
                currEstate.spDiscountDateStart = currOffer.dtStart.Value;
                currEstate.spDiscountDateEnd = currOffer.dtEnd.Value;

                spDiscountList.Add(currEstate);
                //decimal price = rntUtils.rntEstate_minPrice(currEstate.id);
                //string specialOffer = ltr_specialOfferTemplate.Text;
                //specialOffer = specialOffer.Replace("#pagePath#", "/" + currLN.page_path + "");
                //specialOffer = specialOffer.Replace("#dateFromTo#", "<span class=\"dalal\">" + CurrentSource.getSysLangValue("lblDateFrom") + " " + currOffer.dtStart.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "") + "<br />" + CurrentSource.getSysLangValue("lblDateTo") + " " + currOffer.dtEnd.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "") + "</span>");
                //specialOffer = specialOffer.Replace("#prPercentage#", "<span class=\"perc\">-" + currOffer.pr_discount.objToInt32() + "%</span>");
                //specialOffer = specialOffer.Replace("#imgPreview#", "<img src=\"http://www.rentalinrome.com/" + currEstate.img_preview_1 + "\" alt=\"\" />");
                //specialOffer = specialOffer.Replace("#title#", "<span class=\"tit\">" + currLN.title + "</span>");
                //specialOffer = specialOffer.Replace("#zoneTitle#", "<span class=\"zona\">" + CurrentSource.locZone_title(currEstate.pid_zone.objToInt32(), CurrentLang.ID, "") + "</span>");
                //specialOffer = specialOffer.Replace("#price#", "<span class=\"price\"><span class=\"eu\">&euro;</span> <strong>" + price.ToString("N0") + "</strong></span>");
                //specialOffer = specialOffer.Replace("#lbl2paxPerDay#", CurrentSource.getSysLangValue("lbl2paxPerDay"));
                //specialOffer = specialOffer.Replace("#lblMinPriceUpTo4Guests#", CurrentSource.getSysLangValue("lblMinPriceUpTo4Guests").Replace("4", currEstate.pr_basePersons.ToString()));
                //specialOffer = specialOffer.Replace("#lblPriceFrom#", CurrentSource.getSysLangValue("lblPriceFrom"));
                //currListStr += "<li>" + specialOffer + "</li>";
            }
            LV_spDiscountList.DataSource = spDiscountList;
            LV_spDiscountList.DataBind();
            PH_spDiscountList.Visible = LV_spDiscountList.Items.Count > 0;
            //PH_proprietariOtherLang.Text = "<ul id='SpecialOfferSelected'>" + currListStr + "</ul>";
        }
        protected void fillSpecialOfferRandom()
        {
            if (lastEstates == null)
                lastEstates = new List<int>();
            if (lastEstates.Count > 5)
            {
                lastEstates.RemoveAt(0);
            }
            List<RNT_TB_SPECIAL_OFFER> specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd > DateTime.Now.AddDays(5) && x.is_active == 1 && !lastEstates.Contains(x.pid_estate.objToInt32())).OrderByDescending(x => x.pr_discount).ToList();
            if (specialOfferList.Count == 0) return;
            RNT_TB_SPECIAL_OFFER currOffer = specialOfferList[0];
            RNT_TB_ESTATE currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.is_active == 1 && x.id == currOffer.pid_estate);
            if (currEstate == null) return;
            RNT_LN_ESTATE currLN = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == currEstate.id && x.pid_lang == CurrentLang.ID);
            if (currLN == null)
                currLN = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == currEstate.id && x.pid_lang == 2);
            if (currLN == null) return;
            lastEstates.Add(currEstate.id);
            decimal price = rntUtils.rntEstate_minPrice(currEstate.id);
            //string specialOffer = ltr_specialOfferTemplate.Text;
            //specialOffer = specialOffer.Replace("#pagePath#", "/" + currLN.page_path + "");
            //specialOffer = specialOffer.Replace("#dateFromTo#", "<span class=\"dalal\">" + CurrentSource.getSysLangValue("lblDateFrom") + " " + currOffer.dtStart.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "") + "<br />" + CurrentSource.getSysLangValue("lblDateTo") + " " + currOffer.dtEnd.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "") + "</span>");
            //specialOffer = specialOffer.Replace("#prPercentage#", "<span class=\"perc\">-" + currOffer.pr_discount.objToInt32() + "%</span>");
            //specialOffer = specialOffer.Replace("#imgPreview#", "<img src=\"http://www.rentalinrome.com/" + currEstate.img_preview_1 + "\" alt=\"\" />");
            //specialOffer = specialOffer.Replace("#title#", "<span class=\"tit\">" + currLN.title + "</span>");
            //specialOffer = specialOffer.Replace("#zoneTitle#", "<span class=\"zona\">" + CurrentSource.locZone_title(currEstate.pid_zone.objToInt32(), CurrentLang.ID, "") + "</span>");
            //specialOffer = specialOffer.Replace("#price#", "<span class=\"price\"><span class=\"eu\">&euro;</span> <strong>" + price.ToString("N0") + "</strong></span>");
            //specialOffer = specialOffer.Replace("#lbl2paxPerDay#", CurrentSource.getSysLangValue("lbl2paxPerDay"));
            //specialOffer = specialOffer.Replace("#lblMinPriceUpTo4Guests#", CurrentSource.getSysLangValue("lblMinPriceUpTo4Guests").Replace("4", currEstate.pr_basePersons.ToString()));
            //specialOffer = specialOffer.Replace("#lblPriceFrom#", CurrentSource.getSysLangValue("lblPriceFrom"));
            //PH_proprietariOtherLang.Text = specialOffer;
        }
    }
}
