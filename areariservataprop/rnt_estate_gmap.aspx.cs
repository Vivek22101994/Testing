using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Subgurim.Controles;

namespace RentalInRome.areariservataprop
{
    public partial class rnt_estate_gmap : ownerBasePage
    {
        private magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                HF_referer.Value = "rnt_estate_list.aspx";
                HF_id.Value = Request.QueryString["id"];
                SetCoords();
                myMap();
            }
        }
        protected void SetCoords()
        {
            RNT_TB_ESTATE _d = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == HF_id.Value.ToInt32() && x.pid_owner == OwnerAuthentication.CurrentID);
                if (_d == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('error!');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect", "window.location='" + HF_referer.Value + "';", true);
                    return;
                }
                UC_rnt_estate_navlinks1.IdEstate = HF_id.Value.ToInt32();
                ltr_address.Text = _d.loc_address + " " + CurrentSource.loc_cityTitle(_d.pid_city.objToInt32(), CurrentLang.ID, "") + " Italia";
                ltr_title.Text = "" + (_d.pid_owner == 385 ? _d.inner_notes : _d.code) + " / id=" + _d.id;
                chk_is_google_map.Checked = _d.is_google_maps == 1;
                HF_coord.Value = _d.google_maps;
                PH_chk.Visible = true;
            GeoCode geocode = GMap.geoCodeRequest(ltr_address.Text, CurrentAppSettings.GOOGLE_MAPS_KEY);
            if (HF_coord.Value == "")
                HF_coord.Value = geocode.Placemark.coordinates.lat + "|" + geocode.Placemark.coordinates.lng;
        }
        private void myMap()
        {
            GMap1.resetMarkers();
            GMap1.Key = CurrentAppSettings.GOOGLE_MAPS_KEY;
            GMap1.addGMapUI(new GMapUI());
            GMap1.GZoom = 15;
            GLatLng latlng = new GLatLng(Convert.ToDouble(HF_coord.Value.Split('|')[0].Replace(".", ",")), Convert.ToDouble(HF_coord.Value.Split('|')[1].Replace(".", ",")));
            GMap1.setCenter(latlng);
            GMarker mkr = CommonUtilities.GetGoogleMapsMarker(latlng, true);
            GMap1.addGMarker(mkr);

            GMap1.addListener(new GListener(mkr.ID, GListener.Event.dragend,
                 string.Format(@"
               function(overlay, point)
               {{
                  var ev = new serverEvent('myDragEnd', {0});
                  ev.addArg({0}.getZoom());
                  ev.addArg(this.getPoint());
                  ev.send();
               }}
               ", GMap1.GMap_Id)));
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
                    _str = "$get('" + HF_coord.ClientID + "').value='" + point.lat + "|" + point.lng + "';";
                    break;
            }
            return _str;
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            save();
            Response.Redirect(HF_referer.Value);
        }
        protected void save()
        {
            RNT_TB_ESTATE _currTBL = null;
            if (HF_id.Value != "0" && HF_id.Value != "")
            {
                _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Convert.ToInt32(HF_id.Value));
            }
            if (_currTBL == null)
            {
                //Response.Redirect(HF_referer.Value);
                return;
            }
            _currTBL.google_maps = HF_coord.Value.Replace(".", ",");
            _currTBL.is_google_maps = chk_is_google_map.Checked ? 1 : 0;
            DC_RENTAL.SubmitChanges();
            string _coords = "";
            string _address = "";
            if (_currTBL.is_srs == 1)
            {
                _coords = _currTBL.google_maps ?? "";
                _coords = _coords.Replace(",", ".").Replace("|", ",");
                _address = _currTBL.loc_address;
                Srs_WS.Location_Insert_Update(_currTBL.code, CurrentSource.locZone_title(_currTBL.pid_zone.objToInt32(), 1, "---"), _address, _currTBL.id, 1, _currTBL.num_bed_single.objToInt32(), _currTBL.num_bed_double.objToInt32(), _currTBL.num_rooms_bath.objToInt32(), 0, _coords);
            }
            if (_currTBL.is_ecopulizie == 1)
            {
                Eco_WS.Location_Insert_Update(_currTBL);
            }
        }

        protected void lnk_saveGoSV_Click(object sender, EventArgs e)
        {
            save();
            Response.Redirect("rnt_estate_gmap_sv.aspx?id=" + HF_id.Value);
        }
        protected void lnk_chiudi_Click(object sender, EventArgs e)
        {
            Response.Redirect(HF_referer.Value);
        }
    }
}
