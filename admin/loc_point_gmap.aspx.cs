using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Subgurim.Controles;

namespace RentalInRome.admin
{
    public partial class loc_point_gmap : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "loc_point";
        }
        private magaLocation_DataContext DC_LOCATION;
        protected string listPage = "loc_point_list.aspx";
        private LOC_TB_POINT _currTBL;
        private List<LOC_LN_POINT> CURRENT_LANG_;
        private List<LOC_LN_POINT> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(LOC_LN_POINT)).Cast<LOC_LN_POINT>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<LOC_LN_POINT>();

                return CURRENT_LANG_;
            }
            set
            {
                ViewState["CURRENT_LANG"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_LANG_ = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_LOCATION = maga_DataContext.DC_LOCATION;
            if (!IsPostBack)
            {
                HF_id.Value = Request.QueryString["id"].objToInt32().ToString();
                FillControls();
            }
            else
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setMap", "initMap();", true);
        }
        private void FillControls()
        {
            _currTBL = DC_LOCATION.LOC_TB_POINT.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                Response.Redirect(listPage);
            }
            LOC_LN_POINT _ln = DC_LOCATION.LOC_LN_POINTs.SingleOrDefault(x => x.pid_point == _currTBL.id && x.pid_lang == 1);
            chk_gmaps_available.Checked = _currTBL.gmaps_available == 1;
            HF_gmaps_coords.Value = _currTBL.gmaps_coords + "";
            if (HF_gmaps_coords.Value == "")
            {
                HF_address.Value = CurrentSource.loc_cityTitle((_currTBL.pid_city.HasValue ? _currTBL.pid_city.Value : 1), 1, "Roma") + " " + (_ln == null ? "Termini" : _ln.title);
                //GeoCode geocode = GMap.geoCodeRequest(CurrentSource.loc_cityTitle((_currTBL.pid_city.HasValue ? _currTBL.pid_city.Value : 1), 1, "Roma") + " " + (_ln == null ? "Termini" : _ln.title), CurrentAppSettings.GOOGLE_MAPS_KEY);
                //HF_gmaps_coords.Value = geocode.Placemark.coordinates.lat + "|" + geocode.Placemark.coordinates.lng;
            }
            // myMap();
        }
        private void FillDataFromControls()
        {
            _currTBL = _currTBL = DC_LOCATION.LOC_TB_POINT.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                Response.Redirect(listPage);
            }
            _currTBL.gmaps_available = chk_gmaps_available.Checked ? 1 : 0;
            _currTBL.gmaps_coords = HF_gmaps_coords.Value;
            DC_LOCATION.SubmitChanges();
            Response.Redirect("loc_point_details.aspx?id=" + _currTBL.id);
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
        private void myMap()
        {
//            GMap1.resetMarkers();
//            GMap1.Key = CurrentAppSettings.GOOGLE_MAPS_KEY;
//            GMap1.addGMapUI(new GMapUI());
//            GMap1.GZoom = 15;
//            GLatLng latlng = new GLatLng(Convert.ToDouble(HF_gmaps_coords.Value.Split('|')[0].Replace(".", ",")), Convert.ToDouble(HF_gmaps_coords.Value.Split('|')[1].Replace(".", ",")));
//            GMap1.setCenter(latlng);
//            GMarker mkr = CommonUtilities.GetGoogleMapsMarker(latlng, true);
//            GMap1.addGMarker(mkr);

//            GMap1.addListener(new GListener(mkr.ID, GListener.Event.dragend,
//                 string.Format(@"
//               function(overlay, point)
//               {{
//                  var ev = new serverEvent('myDragEnd', {0});
//                  ev.addArg({0}.getZoom());
//                  ev.addArg(this.getPoint());
//                  ev.send();
//               }}
//               ", GMap1.GMap_Id)));
        }
        protected string GMap1_ServerEvent(object s, GAjaxServerEventOtherArgs e)
        {
            string _str = "";
            switch (e.eventName)
            {
                case "myDragEnd":
                    string zoomLevel = e.eventArgs[0];
                    GLatLng point = GAjaxServerEventArgs.latlngFromString(e.eventArgs[1]);
                    GLatLng center = e.center;
                    _str = "$get('" + HF_gmaps_coords.ClientID + "').value='" + point.lat + "|" + point.lng + "';";
                    break;
            }
            return _str;
        }
    }
}
