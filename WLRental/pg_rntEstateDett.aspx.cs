using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;

namespace RentalInRome.WLRental
{
    public partial class pg_rntEstateDett : mainBasePage
    {
        private magaRental_DataContext DC_RENTAL;
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
                ucBooking.IdEstate = value;
            }
        }
        public string bannerImage
        {
            get
            {
                return HF_banner_image.Value;
            }
            set
            {
                HF_banner_image.Value = value;
            }
        }
        public int SeasonDateHeaderColSpan = 0;
        private RNT_TB_ESTATE TMPcurrEstateTB;
        public RNT_TB_ESTATE currEstateTB
        {
            get
            {
                if (TMPcurrEstateTB == null)
                    TMPcurrEstateTB = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                return TMPcurrEstateTB ?? new RNT_TB_ESTATE();
            }
            set { TMPcurrEstateTB = value; }
        }

        public RNT_LN_ESTATE currEstateLN
        {
            get
            {
                if (TMPlnEstate == null)
                    TMPlnEstate = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == CurrentLang.ID);
                return TMPlnEstate ?? new RNT_LN_ESTATE();
            }
            set { TMPlnEstate = value; }
        }
        private RNT_LN_ESTATE TMPlnEstate;
        protected string IMG_ROOT
        {
            get { return "http://www.rentalinrome.com/"; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "pg_estate";
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
                ErrorLog.addLog(_ip, "pg_estate_details", _params);
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
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                PH_back_to_list.Visible = Request.QueryString["bts"] == "true";

                BindControls();
                Fill_data();
            }
        }
        protected void BindControls()
        {
            List<int> _string = new List<int>();
            List<Category> _cats = _string.Select(x => new Category(x.ToString())).ToList();
        }
        public class Category
        {
            public string name { set; get; }
            public Category(string _name)
            {
                name = _name;
            }
        }
        protected void Fill_data()
        {
            TMPcurrEstateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == PAGE_REF_ID && x.is_active == 1 && x.is_deleted != 1);
            if (TMPcurrEstateTB == null)
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "Apt non attivo id=" + PAGE_REF_ID, _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
                return;
            }

            // details
            IdEstate = currEstateTB.id;
            HF_pid_zone.Value = currEstateTB.pid_zone.objToInt32().ToString();

            // gmap
            PH_mapsContAll.Visible = PH_mapCont.Visible = PH_mapCont_toggler.Visible = ("" + currEstateTB.google_maps).Trim() != "" && currEstateTB.is_google_maps == 1;
            // ltr_gmapPointsScript
            ltr_gmapPointsScript.Text = gmapPointsScript();
            // gmap SV
            PH_gmapSvcont.Visible = PH_gmapSvcont_toggler.Visible = ("" + currEstateTB.sv_coords).Trim() != "" && currEstateTB.is_street_view == 1;
            PH_gmapSvcontNo.Visible = !PH_gmapSvcont.Visible;

            // fill Estate Config
            List<int> _configIDs_List = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.is_HomeAway == 0).Select(x => x.pid_config).ToList();
            LV_config.DataSource = DC_RENTAL.RNT_TB_CONFIGs.Where(x => _configIDs_List.Contains(x.id));
            LV_config.DataBind();

            // fill Estate gallery/video
            var gallery_video_list = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate).ToList();

            //biniding with original images
            var images = gallery_video_list.Where(x => x.type == "original").OrderBy(x => x.sequence).ToList();
            if (images == null || images.Count == 0)
                images = gallery_video_list.Where(x => x.type == "gallery").OrderBy(x => x.sequence).ToList();

            if (images != null && images.Count > 0)
            {
                LV_gallery.DataSource = images;
                LV_gallery.DataBind();
                bannerImage = images[0].img_banner;                
            }

            LV_floorplans.DataSource = gallery_video_list.Where(x => x.type == "floorplans").OrderBy(x => x.sequence).ToList();
            LV_floorplans.DataBind();
            var videoList = gallery_video_list.Where(x => x.type == "video").OrderBy(x => x.sequence).ToList();
            LV_videoCont.DataSource = videoList;
            LV_videoCont.DataBind();
            LV_videoToggler.DataSource = videoList;
            LV_videoToggler.DataBind();

            // fill Prices
            if (currEstateTB.pr_1_2pax == 0 && currEstateTB.pr_2_2pax == 0 && currEstateTB.pr_3_2pax == 0)
            {
                pnl_pricePreviewCont.Visible = false; // pricepreview NOT visible
                PH_priceOnRequestCont.Visible = true;
                PH_priceListCont.Visible = false;
            }
            else
            {
                pnl_pricePreviewCont.Visible = true; // pricepreview visible
                PH_priceOnRequestCont.Visible = false;
                PH_priceListCont.Visible = true;
                fillPrice_v1();
            }

            // fill Special Offers
            List<RNT_VIEW_SPECIAL_OFFER> _soList = AppSettings.RNT_VIEW_SPECIAL_OFFERs.Where(x => x.dtPublicStart <= DateTime.Now.Date && x.dtPublicEnd >= DateTime.Now.Date && x.dtEnd > DateTime.Now.AddDays(5) && x.pid_lang == CurrentLang.ID && x.pid_estate == IdEstate && x.is_active == 1).OrderBy(x => x.dtEnd).ToList();
            LV_special_offer.DataSource = _soList;
            LV_special_offer.DataBind();
            Literal _ltr = LV_special_offer.FindControl("ltr_title") as Literal;
            if (_ltr != null) _ltr.Text = CurrentSource.getSysLangValue("lblSpecialOffersAndLastMinute");


            // fill ltr_otherZones
            ltr_otherZones.Text = "";
            List<LOC_VIEW_ZONE> _listZone = AppSettings.LOC_ZONEs.Where(x => x.id != HF_pid_zone.Value.ToInt32() && x.pid_lang == CurrentLang.ID && AppSettings.RNT_activeZones.Contains(x.id)).ToList();
            foreach (LOC_VIEW_ZONE viewZone in _listZone)
            {
                ltr_otherZones.Text += "<a href=\"/" + viewZone.page_path + "\">" + viewZone.title + "</a>";
            }
        }
        protected void fillPrice_v1()
        {
            PH_priceListCont_v1.Visible = true;
            PH_priceListCont_v2.Visible = false;
            decimal _pr_discount7daysApply = 1 - (currEstateTB.pr_discount7days.objToDecimal() / 100);
            decimal _pr_discount30daysApply = 1 - (currEstateTB.pr_discount30days.objToDecimal() / 100);
            decimal _minPrice = rntUtils.rntEstate_minPrice(IdEstate);
            ltr_minPriceDay.Text = _minPrice.ToString("N2");
            decimal _tmpPriceWeek = _minPrice * 7 * _pr_discount7daysApply;
            _tmpPriceWeek = _tmpPriceWeek.customRound(true);
            ltr_minPriceWeek.Text = _tmpPriceWeek.ToString("N2");
            string _priceDetails = "";
            decimal _prTemp = 0;
            int pr_basePersons = currEstateTB.pr_basePersons.objToInt32();


            string strScripthidePriceCol = "function setPriceColumns(){";
            // tabella delle stagioni
            var seasonGroup = currEstateTB.pidSeasonGroup.objToInt32();
            var seasonDateList = new List<dbRntSeasonDatesTBL>();
            using (DCmodRental dc = new DCmodRental())
                seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= DateTime.Now && x.dtStart <= DateTime.Now.AddYears(1) && x.pidSeasonGroup == seasonGroup).OrderBy(x => x.dtStart).ToList();
            LVseasonDates.DataSource = seasonDateList;
            LVseasonDates.DataBind();

            if (seasonDateList.Any(x => x.pidPeriod == 1))
            {
                LVseasonDates_1.DataSource = seasonDateList.Where(x => x.pidPeriod == 1);
                LVseasonDates_1.DataBind();
                LVseasonDates_1.Visible = true;
                th_datePeriod1.Visible = true;
                td_colPeriod1.Visible = true;
                th_PricePeriod1.Visible = true;
                //showPeriod_1_Price = true;
                td_day_1.Visible = true;
                td_week_1.Visible = true;

                SeasonDateHeaderColSpan = SeasonDateHeaderColSpan + 1;
                //td_pr_1.Visible = true;
                //  td_pr_1_w.Visible = true;

            }
            else
                strScripthidePriceCol += "$(\".td_pr_1\").hide();" + "$(\".td_pr_1_w\").hide();";

            if (seasonDateList.Any(x => x.pidPeriod == 2))
            {
                LVseasonDates_2.DataSource = seasonDateList.Where(x => x.pidPeriod == 2);
                LVseasonDates_2.DataBind();
                th_datePeriod2.Visible = true;
                td_colPeriod2.Visible = true;
                th_PricePeriod2.Visible = true;

                // showPeriod_2_Price = true;
                td_day_2.Visible = true;
                td_week_2.Visible = true;
                //td_pr_2.Visible = true;
                // td_pr_2_w.Visible = true;
                SeasonDateHeaderColSpan = SeasonDateHeaderColSpan + 1;
            }
            else
                strScripthidePriceCol += "$(\".td_pr_2\").hide();" + "$(\".td_pr_2_w\").hide();";

            if (seasonDateList.Any(x => x.pidPeriod == 3))
            {
                LVseasonDates_3.DataSource = seasonDateList.Where(x => x.pidPeriod == 3);
                LVseasonDates_3.DataBind();
                th_datePeriod3.Visible = true;
                td_colPeriod3.Visible = true;
                th_PricePeriod3.Visible = true;

                td_day_3.Visible = true;
                td_week_3.Visible = true;
                SeasonDateHeaderColSpan = SeasonDateHeaderColSpan + 1;
                //  td_pr_3.Visible = true;


                // td_pr_3_w.Visible = true;
            }
            else
                strScripthidePriceCol += "$(\".td_pr_3\").hide();" + "$(\".td_pr_3_w\").hide();";

            if (seasonDateList.Any(x => x.pidPeriod == 4))
            {
                LVseasonDates_4.DataSource = seasonDateList.Where(x => x.pidPeriod == 4);
                LVseasonDates_4.DataBind();
                th_datePeriod4.Visible = true;
                td_colPeriod4.Visible = true;
                th_PricePeriod4.Visible = true;

                td_day_4.Visible = true;
                td_week_4.Visible = true;
                SeasonDateHeaderColSpan = SeasonDateHeaderColSpan + 1;
                //   td_pr_4.Visible = true;
                //  td_pr_4_w.Visible = true;
            }
            else
                strScripthidePriceCol += "$(\".td_pr_4\").hide();" + "$(\".td_pr_4_w\").hide();";

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
            strScripthidePriceCol += "}";
            ltr_hidePriceColScript.Text = strScripthidePriceCol;
            //  ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setPriceColumns", "setPriceColumns()", true);

        }
        protected string gmapPointsScript()
        {
            string _script = "";
            //List<int> _listIDs = DC_RENTAL.RNT_RL_ESTATE_POINTs.Where(x => x.pid_estate == IdEstate).Select(x=>x.pid_point).ToList();
            //List<LOC_TB_POINT> _list = maga_DataContext.DC_LOCATION.LOC_TB_POINT.Where(x => _listIDs.Contains(x.id) && !string.IsNullOrEmpty(x.gmaps_coords) && x.gmaps_available==1).ToList();
            List<LOC_TB_POINT> _list = maga_DataContext.DC_LOCATION.LOC_TB_POINT.Where(x => x.pid_city == AppSettings.RNT_currCity && x.gmaps_coords != null && x.gmaps_coords != "" && x.gmaps_available == 1).ToList();
            List<LOC_TB_POINT_TYPE> _listTypes = maga_DataContext.DC_LOCATION.LOC_TB_POINT_TYPEs.ToList();
            List<LOC_LN_POINT> _listLN = maga_DataContext.DC_LOCATION.LOC_LN_POINTs.ToList();
            foreach (LOC_TB_POINT _point in _list)
            {
                string _img = "images/travelpoints/types/sightseeings.png";
                string _title = "Sightseeings";
                LOC_LN_POINT _ln = _listLN.SingleOrDefault(x => x.pid_lang == CurrentLang.ID && x.pid_point == _point.id);
                if (_ln == null)
                    _ln = _listLN.SingleOrDefault(x => x.pid_lang == 2 && x.pid_point == _point.id);
                if (_ln != null)
                    _title = _ln.title;
                LOC_TB_POINT_TYPE _type = _listTypes.SingleOrDefault(x => x.id == _point.pid_point_type);
                if (_type != null && !string.IsNullOrEmpty(_type.img_preview))
                    _img = _type.img_preview;
                string lat = _point.gmaps_coords.Split(new char[] { '|' })[0].Replace(',', '.');
                string lng = _point.gmaps_coords.Split(new char[] { '|' })[1].Replace(',', '.');
                _script += "_point" + _point.id + " = new google.maps.LatLng(" + lat + ", " + lng + ");";
                _script += "_IconImage" + _point.id + " = new google.maps.MarkerImage('http://www.rentalinrome.com" + CurrentAppSettings.ROOT_PATH + _img + "', new google.maps.Size(20, 20), new google.maps.Point(0, 0), new google.maps.Point(0, 20));";
                _script += "_IconShape" + _point.id + " = { coord: [1, 1, 1, 20, 20, 20, 20, 1], type: 'poly'};";
                _script += "markerOptions" + _point.id + " = { position: _point" + _point.id + ", map: map, icon: _IconImage" + _point.id + " , shape: _IconShape" + _point.id + " };";
                _script += "marker" + _point.id + " = new google.maps.Marker(markerOptions" + _point.id + ");";
                _script += "google.maps.event.addListener(marker" + _point.id + ", 'mouseover', function() { showPointToolTip(marker" + _point.id + ",'" + _title.Replace("'", "\\'") + "','" + _point.img_preview + "');  });\n";
                _script += "google.maps.event.addListener(marker" + _point.id + ", 'mouseout', function() { hidePointToolTip();});\n";
            }
            return _script;
        }
    }
}

