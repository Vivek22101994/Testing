using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class BcomHotelEdit : adminBasePage
    {
        protected dbRntBcomHotelTBL currTBL;
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
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntBcomHotelTBLs.SingleOrDefault(x => x.hotelId == lbl_id.Text);
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                    }
                }
                closeDetails(false); 
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
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntBcomHotelTBLs.SingleOrDefault(x => x.hotelId == HfId.Value);

                if (currTBL == null) { currTBL = new dbRntBcomHotelTBL(); txt_hotelId.ReadOnly = false; pnl_update.Visible = false; }
                else { txt_hotelId.ReadOnly = true; pnl_update.Visible = true; rdp_updateDate.SelectedDate = DateTime.Now.AddDays(-1); }

                txt_hotelId.Text = currTBL.hotelId;
                drp_isActive.setSelectedValue(currTBL.isActive);
                txt_title.Text = currTBL.title;
                txt_username.Text = currTBL.username;
                txt_password.Text = currTBL.password;
                txt_rateIdStandard.Text = currTBL.rateIdStandard;

                #region New Standard Rates
                txt_rateIdStandard2.Text = currTBL.rateIdStandard2;
                txt_rateIdStandard3.Text = currTBL.rateIdStandard3;
                txt_rateIdStandard4.Text = currTBL.rateIdStandard4;
                #endregion

                txt_rateIdNotRefund.Text = currTBL.rateIdNotRefund;
                txt_rateIdSpecial.Text = currTBL.rateIdSpecial;
                
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntBcomHotelTBLs.SingleOrDefault(x => x.hotelId == HfId.Value);
                if (currTBL == null)
                {
                    currTBL = new dbRntBcomHotelTBL();
                    dc.Add(currTBL);
                }
                currTBL.hotelId = txt_hotelId.Text;
                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.title = txt_title.Text;
                currTBL.username = txt_username.Text;
                currTBL.password = txt_password.Text;
                currTBL.rateIdStandard = txt_rateIdStandard.Text;

                #region New Standard Rates
                currTBL.rateIdStandard2 = txt_rateIdStandard2.Text;
                currTBL.rateIdStandard3 = txt_rateIdStandard3.Text;
                currTBL.rateIdStandard4 = txt_rateIdStandard4.Text;
                #endregion

                currTBL.rateIdNotRefund = txt_rateIdNotRefund.Text;
                currTBL.rateIdSpecial = txt_rateIdSpecial.Text;

                dc.SaveChanges();

	            rwdDett.VisibleOnPageLoad = true;

            }
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "0";
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
        protected void lnk_update_Click(object sender, EventArgs e)
        {
            if (!rdp_updateDate.SelectedDate.HasValue)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Seleziona una data\", 340, 110);", true);
                return;
            }
            BcomImport.BcomImport_start(txt_username.Text, txt_password.Text, txt_hotelId.Text, rdp_updateDate.SelectedDate.formatCustom("#yy#-#mm#-#dd#", App.DefLangID, "--/--/----"));
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Operazione in corso...\", 340, 110);", true);
        }
    }

}