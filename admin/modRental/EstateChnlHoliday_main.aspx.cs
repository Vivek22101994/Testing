using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental
{
    public partial class EstateChnlHoliday_main : adminBasePage
    {
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }
        protected magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].objToInt32();
                fillData();
            }
        }
        protected void fillData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                lnkGetFromHL.Visible = false;
                pnlActiveState.Visible = false;
                var currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl != null)
                {
                    txt_homeID.Text = currTbl.homeId;
                    lnkGetFromHL.Visible = lnkGetFromRental.Visible = currTbl.homeId.ToInt64() > 0;
                    pnlActiveState.Visible = currTbl.homeId.ToInt64() > 0;
                    lnkActiveStateOn.Visible = !currTbl.ActiveState;
                    lnkActiveStateOff.Visible = currTbl.ActiveState;
                }
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
            }
        }
        protected void lnkSaveExisting_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var currTbl = dc.dbRntChnlHolidayEstateTBs.FirstOrDefault(x => x.id != IdEstate && x.homeId == txt_homeID.Text);
                if (currTbl != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!<br />HomeId Ripetuto.\", 340, 110);", true);
                    return;
                }
                currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    currTbl = new dbRntChnlHolidayEstateTB() { id = IdEstate };
                    dc.Add(currTbl);
                }
                else if (currTbl.homeId.ToInt64() > 0 && currTbl.homeId.ToInt64() != txt_homeID.Text.ToInt64() && currTbl.ActiveState)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!<br />Disattivare HomeId Vecchio prima di abbinare nuovamente.\", 340, 110);", true);
                    return;
                }
                currTbl.homeId = txt_homeID.Text;
                dc.SaveChanges();
                var errorString = ChnlHolidayImport.PropertyInfoActiveState_start(IdEstate, App.HOST);
                if (errorString != "")
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione errore!<br />" + errorString + "\", 340, 110);", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.<br />Ora puoi Importare la scheda da HL su Magarental\", 340, 110);", true);
                fillData();
            }
        }
        protected void lnkCreateNew_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var currTbl = dc.dbRntChnlHolidayEstateTBs.FirstOrDefault(x => x.id != IdEstate && x.homeId == txt_homeID.Text);
                if (currTbl != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!<br />HomeId Ripetuto.\", 340, 110);", true);
                    return;
                }
                currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    currTbl = new dbRntChnlHolidayEstateTB() { id = IdEstate };
                    dc.Add(currTbl);
                    dc.SaveChanges();
                }
                else if (currTbl.homeId.ToInt64() > 0 && currTbl.ActiveState)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!<br />Disattivare HomeId Vecchio prima di abbinare nuovamente.\", 340, 110);", true);
                    return;
                }
                var errorString = "";
                var homeId = ChnlHolidayImport.PropertyNew(IdEstate, ref errorString) + "";
                if (errorString != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione errore!<br />" + errorString + "\", 340, 110);", true);
                    return;
                }
                currTbl.homeId = homeId + "";
                dc.SaveChanges();
                errorString = ChnlHolidayImport.PropertyInfoActiveState_start(IdEstate, App.HOST);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Nuova struttura creata su HL\", 340, 110);", true);
                fillData();
            }
        }
        protected void lnkGetFromHL_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null || currTbl.homeId.ToInt64() <= 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"HomeId NON completo!\", 340, 110);", true);
                    return;
                }
                var errorString = ChnlHolidayImport.PropertyInfoFull_start(IdEstate, App.HOST);
                if (errorString != "")
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione errore!<br />" + errorString + "\", 340, 110);", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Tutti i dati sono stati Importati\", 340, 110);", true);
                fillData();
            }
        }
        protected void lnkGetFromRental_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null || currTbl.homeId.ToInt64() <= 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"HomeId NON completo!\", 340, 110);", true);
                    return;
                }
                ChnlHolidayUtils.copyEstateToHoliday(IdEstate);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Tutti i dati sono stati Importati\", 340, 110);", true);
                fillData();
            }
        }
        protected void lnkActiveState_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null || currTbl.homeId.ToInt64() <= 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"HomeId NON completo!\", 340, 110);", true);
                    return;
                }
                var arg = ((LinkButton)sender).CommandArgument;
                var errorString = ChnlHolidayUpdate.UpdateActiveState_start(IdEstate, App.HOST, arg);
                if (errorString != "")
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione errore!<br />" + errorString + "\", 340, 110);", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"" + (arg=="on"?"Attivato":"Disattivato") + " con successo.\", 340, 110);", true);
                fillData();
            }
        }
    }
}