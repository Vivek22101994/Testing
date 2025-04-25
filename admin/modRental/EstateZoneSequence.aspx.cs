using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental
{
    public partial class EstateZoneSequence : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
                fillData();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setSortable", "setSortable();", true);
        }
        protected void fillEstates()
        {
            LV.DataSource = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.pid_city == 1 && x.pid_zone == drp_zone.getSelectedValueInt()).ToList().OrderBy(x => x.zoneSequence).ThenBy(x=>x.sequence);
            LV.DataBind();
        }
        protected void fillData()
        {
            //bind zones
            drp_zone.DataSource = AppSettings.LOC_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == App.LangID && x.pid_city == 1).ToList();
            drp_zone.DataTextField = "title";
            drp_zone.DataValueField = "id";
            drp_zone.DataBind();

            fillEstates();

            //LDS.DataBind();
        }
        protected void saveData()
        {
            List<string> _list = HF_order.Value.splitStringToList("|");
            for (int i = 0; i < _list.Count; i++)
            {
                RNT_TB_ESTATE currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _list[i].ToInt32());
                if (currTBL != null)
                {
                    currTBL.zoneSequence = i + 1;
                }
            }
            DC_RENTAL.SubmitChanges();
            AppSettings.RELOAD_CACHE = true;
            AppSettings._refreshCache_RNT_ESTATEs();
            fillData();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "radAlert", "radalert(\"Ordinamento salvato correttamente.\");", true);
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            fillData();
        }

        protected void drp_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillEstates();
        }
    }
}