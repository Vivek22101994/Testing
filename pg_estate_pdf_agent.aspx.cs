using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class pg_estate_pdf_agent : mainBasePage
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
            }
        }
        public RNT_TB_ESTATE currEstate
        {
            get
            {
                if (TMPcurrEstate == null)
                    TMPcurrEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                return TMPcurrEstate ?? new RNT_TB_ESTATE();
            }
            set { TMPcurrEstate = value; }
        }
        private RNT_TB_ESTATE TMPcurrEstate;
        protected string IMG_ROOT
        {
            get { return Request.Url.AbsoluteUri.Contains("http://localhost") ? "http://192.168.1.150/Rir/rentalinrome/" : "/"; }
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
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {

                BindControls();
                Fill_data();
                setViewVariables();
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
            currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == PAGE_REF_ID && x.is_active == 1 && x.is_deleted != 1);
            if (currEstate == null)
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

            clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
            clSearch _ls = _config.lastSearch;
            _config.addTo_myLastVisitedEstateList(PAGE_REF_ID);

            // fill Estate MaxPersons
            HF_num_persons_adult.Value = currEstate.num_persons_adult.ToString();
            HF_num_persons_optional.Value = currEstate.num_persons_optional.ToString();
            HF_num_persons_min.Value = currEstate.num_persons_min.ToString();
            HF_num_persons_max.Value = currEstate.num_persons_max.ToString();
            HF_num_persons_child.Value = currEstate.num_persons_child.ToString();

            // details
            IdEstate = currEstate.id;
            HF_pid_zone.Value = currEstate.pid_zone.objToInt32().ToString();
            HF_min_nights.Value = currEstate.nights_min.ToString();
            HF_pr_percentage.Value = currEstate.pr_percentage.ToString();
            HF_pr_deposit.Value = currEstate.pr_deposit.ToString();
            HF_is_online_booking.Value = currEstate.is_online_booking.objToInt32().ToString();
            ltr_img_banner.Text = currEstate.img_banner;

            // gmap
            HF_coord.Value = currEstate.google_maps;
            // ltr_gmapPointsScript
            ltr_gmapPointsScript.Text = gmapPointsScript();
            // gmap SV
            HF_sv_coords.Value = currEstate.sv_coords;
            HF_sv_pitch.Value = currEstate.sv_pitch.objToDecimal().ToString().Replace(",", ".");
            HF_sv_yaw.Value = currEstate.sv_yaw.objToDecimal().ToString().Replace(",", ".");
            HF_sv_zoom.Value = currEstate.sv_zoom.objToDecimal().ToString().Replace(",", ".");

            // fill Estate Config
            List<int> _configIDs_List = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate).Select(x => x.pid_config).ToList();
            LV_config.DataSource = DC_RENTAL.RNT_TB_CONFIGs.Where(x => _configIDs_List.Contains(x.id));
            LV_config.DataBind();



            // fill Lang
            RNT_LN_ESTATE _ln = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == CurrentLang.ID);
            if (_ln != null)
            {
                ltr_meta_description.Text = _ln.meta_description;
                ltr_meta_keywords.Text = _ln.meta_keywords;
                ltr_meta_title.Text = _ln.meta_title;
                ltr_title.Text = _ln.title;
                ltr_sub_title.Text = _ln.sub_title;
                ltr_description.Text = _ln.description;
            }
            else
            {
                // se non c'e in lingua
                _ln = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == 2);
                if (_ln != null)
                {
                    ltr_meta_description.Text = _ln.meta_description;
                    ltr_meta_keywords.Text = _ln.meta_keywords;
                    ltr_meta_title.Text = _ln.meta_title;
                    ltr_title.Text = _ln.title;
                    ltr_sub_title.Text = _ln.sub_title;
                    ltr_description.Text = _ln.description;
                }
            }
            // se non c'e la descrizione in lingua
            if (ltr_description.Text == "")
            {
                _ln = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == 2);
                if (_ln != null)
                {
                    ltr_description.Text = _ln.description;
                }
            }
            // fill Prices
            if (currEstate.pr_1_2pax == 0 && currEstate.pr_2_2pax == 0 && currEstate.pr_3_2pax == 0)
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
            List<RNT_VIEW_SPECIAL_OFFER> _soList = AppSettings.RNT_VIEW_SPECIAL_OFFERs.Where(x => x.dtPublicStart <= DateTime.Now.Date && x.dtPublicEnd >= DateTime.Now.Date && x.pid_lang == CurrentLang.ID && x.pid_estate == IdEstate && x.is_active == 1).OrderBy(x => x.dtEnd).ToList();
            LV_special_offer.DataSource = _soList;
            LV_special_offer.DataBind();


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
            decimal _pr_discount7daysApply = 1 - (currEstate.pr_discount7days.objToDecimal() / 100);
            decimal _pr_discount30daysApply = 1 - (currEstate.pr_discount30days.objToDecimal() / 100);
            decimal _minPrice = rntUtils.rntEstate_minPrice(IdEstate);
            ltr_minPriceDay.Text = _minPrice.ToString("N2");
            decimal _tmpPriceWeek = _minPrice * 7 * _pr_discount7daysApply;
            _tmpPriceWeek = _tmpPriceWeek.customRound(true);
            ltr_minPriceWeek.Text = _tmpPriceWeek.ToString("N2");
            string _priceDetails = "";
            decimal _prTemp = 0;
            int pr_basePersons = currEstate.pr_basePersons.objToInt32();
            for (int i = pr_basePersons; i <= HF_num_persons_max.Value.ToInt32(); i++)
            {
                int extraPersons = i - pr_basePersons;
                string numPers_string = extraPersons == 0 && currEstate.num_persons_min.objToInt32() < pr_basePersons ? i + " " + CurrentSource.getSysLangValue("lblPax") + " or less" : i + " " + CurrentSource.getSysLangValue("lblPax");
                string _prStr = ltr_priceTemplate.Text.Replace("#num_pers#", numPers_string);

                // low
                _prTemp = currEstate.pr_1_2pax.objToDecimal() + (extraPersons * currEstate.pr_1_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_1#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_1_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                // hight
                _prTemp = currEstate.pr_2_2pax.objToDecimal() + (extraPersons * currEstate.pr_2_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_2#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_2_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                // very hight
                _prTemp = currEstate.pr_3_2pax.objToDecimal() + (extraPersons * currEstate.pr_3_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_3#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_3_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _priceDetails += _prStr;
            }
            ltr_priceDetails.Text = _priceDetails;

            // tabella delle stagioni
            var seasonGroup = currEstate.pidSeasonGroup.objToInt32();
            var seasonDateList = new List<dbRntSeasonDatesTBL>();
            using (DCmodRental dc = new DCmodRental())
                seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= DateTime.Now && x.dtStart <= DateTime.Now.AddYears(1) && x.pidSeasonGroup == seasonGroup).OrderBy(x => x.dtStart).ToList();
            LVseasonDates.DataSource = seasonDateList;
            LVseasonDates.DataBind();

            LVseasonDates_1.DataSource = seasonDateList.Where(x => x.pidPeriod == 1);
            LVseasonDates_1.DataBind();
            LVseasonDates_2.DataSource = seasonDateList.Where(x => x.pidPeriod == 2);
            LVseasonDates_2.DataBind();
            LVseasonDates_3.DataSource = seasonDateList.Where(x => x.pidPeriod == 3);
            LVseasonDates_3.DataBind();
            LVseasonDates_4.DataSource = seasonDateList.Where(x => x.pidPeriod == 4);
            LVseasonDates_4.DataBind();
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

        protected void setViewVariables()
        {
            ltr_viewBookNow.Text = (HF_is_online_booking.Value == "1") ? CurrentSource.getSysLangValue("reqBookNow") : CurrentSource.getSysLangValue("reqBookingRequest");
        }


        protected void lnk_viewAlternatives_Click(object sender, EventArgs e)
        {

        }

        protected void lnk_addToList_Click(object sender, EventArgs e)
        {

        }

        protected void LV_special_offer_DataBound(object sender, EventArgs e)
        {
            Literal _ltr = LV_special_offer.FindControl("ltr_title") as Literal;
            if (_ltr != null) _ltr.Text = CurrentSource.getSysLangValue("lblSpecialOffersAndLastMinute");
        }
    }
}

