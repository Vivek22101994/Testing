using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_estate_gmap_sv : adminBasePage
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
                lnk_salva.Visible = UserAuthentication.hasPermission(PAGE_TYPE, "can_edit");
                HF_referer.Value = "rnt_estate_list.aspx";
                HF_id.Value = Request.QueryString["id"];
                SetCoords();
            }
        }
        protected void SetCoords()
        {
            RNT_TB_ESTATE _d = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Convert.ToInt32(HF_id.Value));
            if (_d == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('error!');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect", "window.location='" + HF_referer.Value + "';", true);
                return;
            }
            UC_rnt_estate_navlinks1.IdEstate = HF_id.Value.ToInt32();
            HF_pitch.Value = _d.sv_pitch.HasValue ? _d.sv_pitch.ToString().Replace(",", ".") : "0";
            HF_yaw.Value = _d.sv_yaw.HasValue ? _d.sv_yaw.ToString().Replace(",", ".") : "0";
            HF_zoom.Value = _d.sv_zoom.HasValue ? _d.sv_zoom.ToString().Replace(",", ".") : "0";
            HF_coord.Value = _d.sv_pitch.HasValue ? _d.sv_coords : _d.google_maps;
            chk_is_street_view.Checked = _d.is_street_view==1;
            ltr_sc_setInitialPov.Text = "setInitialPov();";
            ltr_sc_setInitialPov.Visible = true;
            PH_sc_setInitialPov.Visible = true;
            ltr_address.Text = _d.loc_address + " " + CurrentSource.loc_cityTitle(_d.pid_city.objToInt32(), CurrentLang.ID, "") + " Italia";
            ltr_title.Text = "" + _d.code + " / id=" + _d.id;
            PH_chk.Visible = true;
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            RNT_TB_ESTATE _d = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Convert.ToInt32(HF_id.Value));
            if (_d == null)
            {
                //Response.Redirect(HF_referer.Value);
                return;
            }
            _d.sv_pitch = Convert.ToDecimal(HF_pitch.Value.Replace('.', ','));
            _d.sv_yaw = Convert.ToDecimal(HF_yaw.Value.Replace('.', ','));
            _d.sv_zoom = Convert.ToDecimal(HF_zoom.Value.Replace('.', ','));
            _d.sv_coords = HF_coord.Value;
            _d.is_street_view = chk_is_street_view.Checked ? 1 : 0;
            DC_RENTAL.SubmitChanges();
            Response.Redirect(HF_referer.Value);
        }

        protected void lnk_chiudi_Click(object sender, EventArgs e)
        {
            Response.Redirect(HF_referer.Value);
        }
    }
}
