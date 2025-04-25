using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class SeasonDatesEdit : adminBasePage
    {
        protected int SeasonGroup { get { return HF_SeasonGroup.Value.ToInt32(); } set { HF_SeasonGroup.Value = "" + value; } }
        protected int CurrDtInt { get { return HF_currDtInt.Value.ToInt32(); } set { HF_currDtInt.Value = "" + value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool canView = true;
                CurrDtInt = Request.QueryString["dt"].ToInt32();
                if (CurrDtInt > 20500000 || CurrDtInt < 20100000)
                {
                    CloseRadWindow("");
                    return;
                }
                SeasonGroup = Request.QueryString["pidSeasonGroup"].ToInt32();

                drp_pidPeriod.Items.Clear();
                drp_pidPeriod.Items.Add(new ListItem(contUtils.getLabel_title("rntSeason_1", 1, "Bassa"), "1"));
                drp_pidPeriod.Items.Add(new ListItem(contUtils.getLabel_title("rntSeason_4", 1, "Media"), "4"));
                drp_pidPeriod.Items.Add(new ListItem(contUtils.getLabel_title("rntSeason_2", 1, "Alta"), "2"));
                drp_pidPeriod.Items.Add(new ListItem(contUtils.getLabel_title("rntSeason_3", 1, "Altissima"), "3"));

                drp_pidPeriod.Items.Insert(0, new ListItem("-seleziona stagionalità-", "0"));

                fillData();
            }
        }

        private void fillData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                ltrTitle.Text = "Aggiorna stagionalità del periodo";
                var currTBL = dc.dbRntSeasonDatesTBLs.SingleOrDefault(x => x.pidSeasonGroup == SeasonGroup && x.dtStart <= CurrDtInt.JSCal_intToDate() && x.dtEnd >= CurrDtInt.JSCal_intToDate());
                if (currTBL != null)
                {
                    ltr_thisTitle.Text = CurrentSource.rntPeriod_title(currTBL.pidPeriod, 1, "period_" + currTBL.pidPeriod);
                    ltr_thisDates.Text = "dal: " + currTBL.dtStart.formatITA(false) + " => al: " + currTBL.dtEnd.formatITA(false);
                }
                else
                {
                    ltr_thisTitle.Text = "La data selezionata non ha stagione";
                    ltr_thisDates.Text = "";

                    currTBL = new dbRntSeasonDatesTBL();
                    currTBL.pidSeasonGroup = SeasonGroup;
                    currTBL.dtStart = CurrDtInt.JSCal_intToDate();
                    currTBL.dtEnd = currTBL.dtStart;
                }
                var tblBefore = dc.dbRntSeasonDatesTBLs.Where(x => x.pidSeasonGroup == SeasonGroup && x.dtEnd < currTBL.dtStart).OrderByDescending(x => x.dtEnd).FirstOrDefault();
                if (tblBefore != null)
                {
                    rdp_dtStart.MinDate = tblBefore.dtStart.AddDays(1);
                    ltr_beforeTitle.Text = CurrentSource.rntPeriod_title(tblBefore.pidPeriod, 1, "period_" + tblBefore.pidPeriod);
                    ltr_beforeDates.Text = "dal: " + tblBefore.dtStart.formatITA(false) + " => al: " + tblBefore.dtEnd.formatITA(false);
                }
                else
                {
                    ltr_beforeTitle.Text = "Non ci sono periodi prima";
                    ltr_beforeDates.Text = "";
                }
                var tblAfter = dc.dbRntSeasonDatesTBLs.Where(x => x.pidSeasonGroup == SeasonGroup && x.dtStart > currTBL.dtEnd).OrderBy(x => x.dtStart).FirstOrDefault();
                if (tblAfter != null)
                {
                    ltr_afterTitle.Text = CurrentSource.rntPeriod_title(tblAfter.pidPeriod, 1, "period_" + tblAfter.pidPeriod);
                    ltr_afterDates.Text = "dal: " + tblAfter.dtStart.formatITA(false) + " => al: " + tblAfter.dtEnd.formatITA(false);
                }
                else
                {
                    ltr_afterTitle.Text = "Non ci sono periodi dopo";
                    ltr_afterDates.Text = "";
                }

                drp_pidPeriod.setSelectedValue(0);
                rdp_dtStart.SelectedDate = CurrDtInt.JSCal_intToDate();
                rdp_dtEnd.SelectedDate = CurrDtInt.JSCal_intToDate();
            }
        }
        protected void lnk_saveMain_Click(object sender, EventArgs e)
        {
            string errorString = "";
            if (drp_pidPeriod.getSelectedValueInt() == 0)
                errorString += "<br/>-selezionare una Stagione";
            if (!rdp_dtStart.SelectedDate.HasValue || !rdp_dtEnd.SelectedDate.HasValue)
                errorString += "<br/>-specificare inizio e fine della Stagione";
            else if (rdp_dtStart.SelectedDate > rdp_dtEnd.SelectedDate)
                errorString += "<br/>-la data di fine Stagione deve essere successiva alla data di inizio";
            if (errorString != "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                return;
            }
            
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {

                var currList = dc.RntSeasonDatesTBL.Where(x => x.pidSeasonGroup == SeasonGroup && x.dtStart <= rdp_dtEnd.SelectedDate.Value && x.dtEnd >= rdp_dtStart.SelectedDate.Value).ToList();
                foreach (var tmp in currList)
                {
                    if (tmp.dtStart < rdp_dtStart.SelectedDate.Value && tmp.dtEnd > rdp_dtEnd.SelectedDate.Value)
                    {
                        if (drp_pidPeriod.getSelectedValueInt() == tmp.pidPeriod)
                        {
                            rdp_dtStart.SelectedDate = tmp.dtStart;
                            rdp_dtEnd.SelectedDate = tmp.dtEnd;
                            dc.RntSeasonDatesTBL.DeleteOnSubmit(tmp);
                            continue;
                        }
                        var newTBL = new RntSeasonDatesTBL();
                        newTBL.pidSeasonGroup = SeasonGroup;
                        newTBL.pidPeriod = tmp.pidPeriod;
                        newTBL.dtStart = rdp_dtEnd.SelectedDate.Value.AddDays(1);
                        newTBL.dtEnd = tmp.dtEnd;
                        dc.RntSeasonDatesTBL.InsertOnSubmit(newTBL);
                        tmp.dtEnd = rdp_dtStart.SelectedDate.Value.AddDays(-1);
                    }
                    else if (tmp.dtStart < rdp_dtStart.SelectedDate.Value && tmp.dtEnd >= rdp_dtStart.SelectedDate.Value)
                    {
                        if (drp_pidPeriod.getSelectedValueInt() == tmp.pidPeriod)
                        {
                            rdp_dtStart.SelectedDate = tmp.dtStart;
                            dc.RntSeasonDatesTBL.DeleteOnSubmit(tmp);
                            continue;
                        }
                        tmp.dtEnd = rdp_dtStart.SelectedDate.Value.AddDays(-1);

                    }
                    else if (tmp.dtStart <= rdp_dtEnd.SelectedDate.Value && tmp.dtEnd > rdp_dtEnd.SelectedDate.Value)
                    {
                        if (drp_pidPeriod.getSelectedValueInt() == tmp.pidPeriod)
                        {
                            rdp_dtEnd.SelectedDate = tmp.dtEnd;
                            dc.RntSeasonDatesTBL.DeleteOnSubmit(tmp);
                            continue;
                        }
                        tmp.dtStart = rdp_dtEnd.SelectedDate.Value.AddDays(1);
                    }
                    else
                        dc.RntSeasonDatesTBL.DeleteOnSubmit(tmp);
                }
                var currTBL = new RntSeasonDatesTBL();
                currTBL.pidSeasonGroup = SeasonGroup;
                currTBL.pidPeriod = drp_pidPeriod.getSelectedValueInt();
                currTBL.dtStart = rdp_dtStart.SelectedDate.Value;
                currTBL.dtEnd = rdp_dtEnd.SelectedDate.Value;
                dc.RntSeasonDatesTBL.InsertOnSubmit(currTBL);
                dc.SubmitChanges();
                foreach (var tmp in AppSettings.RNT_TB_ESTATE.Where(x => x.pidSeasonGroup == SeasonGroup && x.is_active == 1 && x.is_deleted == 0).ToList())
                    rntUtilsChnlAll.UpdateRates(tmp.id);
                //BcomUpdate.BcomUpdate_start(tmp.id, "rates");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            CloseRadWindow("");
        }
    }
}

