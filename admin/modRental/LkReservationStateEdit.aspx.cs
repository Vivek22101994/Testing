using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using RentalInRome.data;

namespace ModRental.admin.modRental
{
    public partial class LkReservationStateEdit : adminBasePage
    {
        protected RNT_LK_RESERVATION_STATE currTbl;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "dett")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                HfId.Value = lbl_id.Text;
                fillData();
            }
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            rwdDett.VisibleOnPageLoad = false;
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        private void fillData()
        {
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                currTbl = dc.RNT_LK_RESERVATION_STATEs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTbl == null)
                {
                    currTbl = new RNT_LK_RESERVATION_STATE();
                }
                rwdDett.VisibleOnPageLoad = true;
                txt_title.Text = currTbl.title;
                txt_type.Text = currTbl.type + "";
            }
        }
        private void saveData()
        {
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                currTbl = dc.RNT_LK_RESERVATION_STATEs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTbl == null)
                {
                    currTbl = new RNT_LK_RESERVATION_STATE();
                    dc.RNT_LK_RESERVATION_STATEs.InsertOnSubmit(currTbl);
                }
                currTbl.title = txt_title.Text.Trim();
                currTbl.type = txt_type.Text.ToInt32();
                dc.SubmitChanges();
                AppSettings.RNT_LK_RESERVATION_STATEs = dc.RNT_LK_RESERVATION_STATEs.ToList();
            }
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            closeDetails(true);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }

}