using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_estate_service : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                //lnk_save.Visible = lnk_annulla.Visible = UserAuthentication.hasPermission("rnt_estate", "can_edit");
                bool _ok = false;
                int id = Request.QueryString["id"].ToInt32();
                RNT_VIEW_ESTATE _est = DC_RENTAL.RNT_VIEW_ESTATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
                if (_est != null)
                {
                    _ok = true;
                    UC_rnt_estate_service.IdEstate = _est.id;
                    UC_rnt_estate_navlinks1.IdEstate = _est.id;
                    HF_IdEstate.Value = _est.id.ToString();
                    string _service = Request.QueryString["service"];
                    if (_service == "srs")
                        ltr_pageTitle.Text = "Gestione dei periodi di servizio 'Srs' presso la struttura '" + _est.code + " / " + "rif. " + _est.id + "'";
                    else
                    {
                        _service = "eco";
                        ltr_pageTitle.Text = "Gestione dei periodi di servizio 'Ecopulizie' presso la struttura '" + _est.code + " / " + "rif. " + _est.id + "'";
                    }
                    UC_rnt_estate_service.Service = _service;
                    pnl_eco.Visible = _service != "eco";
                    pnl_srs.Visible = _service != "srs";
                }
                if (!_ok)
                {
                    Response.Redirect("rnt_estate_list.aspx");
                }
            }
        }
    }
}