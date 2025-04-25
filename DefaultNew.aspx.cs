using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome
{
    public partial class DefaultNew : contStpBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            int _lang = Request.QueryString["lang"].ToInt32();
            if (_lang == 0)
            {
                CurrentLang.ID = 2;
                CurrentLang.NAME = "en-GB";
                CurrentLang.ABBR = "eng";
                CurrentLang.TITLE = "English";
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "stp";
            base.PAGE_REF_ID = Request.QueryString["id"].ToInt32();
            RewritePath();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillHomeGallery();
                FillSpecialOffers();
            }
        }

        private void FillHomeGallery()
        {
            List<int> estateIds = AppSettings.RNT_VIEW_ESTATE_POSITION.Where(x => x.position == "homepage_attention" && x.pid_lang == App.LangID && x.pid_estate.HasValue).OrderBy(x => x.sequence).Select(x => x.pid_estate.objToInt32()).ToList();
            
            using (magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL)
            {
                List<RNT_VIEW_ESTATE> finalEstateList = DC_RENTAL.RNT_VIEW_ESTATEs.Where(x => estateIds.Contains(x.id) && x.pid_lang == App.LangID && (x.is_deleted == 0 || x.is_deleted == null) && x.is_active == 1 && x.is_exclusive == 1 && x.title != null && x.title != "" && x.page_path != null && x.page_path != "").ToList();
                LVHomeGallery.DataSource = finalEstateList;
                LVHomeGallery.DataBind();
            }
        }

        private void FillSpecialOffers()
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
                decimal minPrice = rntUtils.rntEstate_minPrice(currEstate.id);
                if(minPrice > 0) 
                {
                    currEstate.spDiscountedAmount = minPrice - ((minPrice * currEstate.spDiscountAmount) / 100);
                }
                else
                    continue;
                
                spDiscountList.Add(currEstate);
            }
            LVSpecialOffers.DataSource = spDiscountList;
            LVSpecialOffers.DataBind();
            phSpecialOffer.Visible = LVSpecialOffers.Items.Count > 0;
        }
    }
}