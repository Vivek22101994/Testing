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
    public partial class rnt_residence_gmap : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        private magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                HF_referer.Value = "rnt_residence_list.aspx";
                HF_id.Value = Request.QueryString["id"];
                SetCoords();
                myMap();
            }
        }
        protected void SetCoords()
        {
            RNT_TB_RESIDENCE _d = null;
            if (HF_id.Value != "0" && HF_id.Value != "")
            {
                _d = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == Convert.ToInt32(HF_id.Value));
            }
            if (_d == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('error!');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect", "window.location='" + HF_referer.Value + "';", true);
                return;
            }
            UC_rnt_residence_navlinks1.IdResidence = HF_id.Value.ToInt32();
            ltr_address.Text = _d.loc_address + " " + CurrentSource.loc_cityTitle(_d.pid_city.objToInt32(), CurrentLang.ID, "") + " Italia";
            ltr_title.Text = "" + _d.code + " / id=" + _d.id;
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
            RNT_TB_RESIDENCE _d = null;
            if (HF_id.Value != "0" && HF_id.Value != "")
            {
                _d = DC_RENTAL.RNT_TB_RESIDENCEs.SingleOrDefault(x => x.id == Convert.ToInt32(HF_id.Value));
            }
            if (_d == null)
            {
                //Response.Redirect(HF_referer.Value);
                return;
            }
            _d.google_maps = HF_coord.Value.Replace(".", ",");
            _d.is_google_maps = chk_is_google_map.Checked ? 1 : 0;
            DC_RENTAL.SubmitChanges();
        }

        protected void lnk_saveGoSV_Click(object sender, EventArgs e)
        {
            save();
            Response.Redirect("rnt_residence_gmap_sv.aspx?id=" + HF_id.Value);
        }
        protected void lnk_chiudi_Click(object sender, EventArgs e)
        {
            Response.Redirect(HF_referer.Value);
        }
    }
}
