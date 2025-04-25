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
    public partial class rnt_estate_gmap : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        private magaRental_DataContext DC_RENTAL;
        public int IdEstate
        {
            get
            {
                return HfId.Value.ToInt32();
            }
            set
            {
                HfId.Value = value.ToString();
            }
        }
        protected RNT_TB_ESTATE currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                lnk_salva.Visible = lnk_saveGoSV.Visible = true;// UserAuthentication.hasPermission(PAGE_TYPE, "can_edit");
                HF_referer.Value = "rnt_estate_list.aspx";
                IdEstate = Request.QueryString["id"].ToInt32();
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                fillData();
            }
            else
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setMap", "initMap();", true);
        }
        private void fillData()
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('error!');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect", "window.location='" + HF_referer.Value + "';", true);
                return;
            }
            ltr_address.Text = currTBL.loc_address + ", " + CurrentSource.loc_cityTitle(currTBL.pid_city.objToInt32(), CurrentLang.ID, "") + ", Italia";
            ltr_title.Text = "" + currTBL.code + " / id=" + currTBL.id;
            chk_is_google_map.Checked = currTBL.is_google_maps == 1;
            PH_chk.Visible = true;
            //if (string.IsNullOrEmpty(currTBL.google_maps))
            //{
            //    GeoCode geocode = GMap.geoCodeRequest(ltr_address.Text, App.GOOGLE_MAPS_KEY);
            //    HF_coord.Value = geocode.Placemark.coordinates.lat + "|" + geocode.Placemark.coordinates.lng;
            //    if (HF_coord.Value == "0|0")
            //    {
            //        geocode = GMap.geoCodeRequest(CurrentSource.loc_cityTitle(currTBL.pid_city.objToInt32(), CurrentLang.ID, "") + ", Italia", App.GOOGLE_MAPS_KEY);
            //        HF_coord.Value = geocode.Placemark.coordinates.lat + "|" + geocode.Placemark.coordinates.lng;
            //    }
            //}
            //else
            //{
            //    HF_coord.Value = currTBL.google_maps;
            //}
            if (!string.IsNullOrEmpty(currTBL.google_maps))
            {
                HF_coord.Value = currTBL.google_maps;
            }
           

        }
        private void saveData()
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                //Response.Redirect(HF_referer.Value);
                return;
            }
            currTBL.google_maps = HF_coord.Value.Replace(".", ",");
            currTBL.is_google_maps = chk_is_google_map.Checked ? 1 : 0;
            DC_RENTAL.SubmitChanges();
            string _coords = "";
            string _address = "";
            if (currTBL.is_srs == 1)
            {
                _coords = currTBL.google_maps ?? "";
                _coords = _coords.Replace(",", ".").Replace("|", ",");
                _address = currTBL.loc_address;
                //Srs_WS.Location_Insert_Update(_currTBL.code, CurrentSource.locZone_title(_currTBL.pid_zone.objToInt32(), 1, "---"), _address, _currTBL.id, 1, _currTBL.num_bed_single.objToInt32(), _currTBL.num_bed_double.objToInt32(), _currTBL.num_rooms_bath.objToInt32(), 0, _coords);
            }
            if (currTBL.is_ecopulizie == 1)
            {
                //Eco_WS.Location_Insert_Update(_currTBL);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnk_saveGoSV_Click(object sender, EventArgs e)
        {
            saveData();
            Response.Redirect("rnt_estate_gmap_sv.aspx?id=" + IdEstate);
        }
        protected void lnk_chiudi_Click(object sender, EventArgs e)
        {
            Response.Redirect(HF_referer.Value);
        }
    }
}
