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
    public partial class ChnlExpediaHotelEdit : adminBasePage
    {
        protected dbRntChnlExpediaHotelTBL currTBL;
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
                using (DCchnlExpedia dcChnl = new DCchnlExpedia())
                {
                    currTBL = dcChnl.dbRntChnlExpediaHotelTBLs.SingleOrDefault(x => x.HotelId == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        //if (UserAuthentication.CurrRoleTBL.viewOnlyNoSave.objToInt32() != 0) return;
                        var roomTypeIds = dcChnl.dbRntChnlExpediaRoomTypeTBLs.Where(x => x.HotelId == currTBL.HotelId).Select(x => x.id).ToList();
                        dcChnl.Delete(currTBL);
                        if (dcChnl.dbRntChnlExpediaRoomTypeBedTypeTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)).Count() > 0)
                            dcChnl.Delete(dcChnl.dbRntChnlExpediaRoomTypeBedTypeTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)));
                        if (dcChnl.dbRntChnlExpediaRoomTypeOccupancyByAgeTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)).Count() > 0)
                            dcChnl.Delete(dcChnl.dbRntChnlExpediaRoomTypeOccupancyByAgeTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)));
                        if (dcChnl.dbRntChnlExpediaRoomTypeRateThresholdTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)).Count() > 0)
                            dcChnl.Delete(dcChnl.dbRntChnlExpediaRoomTypeRateThresholdTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)));
                        if (dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)).Count() > 0)
                            dcChnl.Delete(dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)));
                        if (dcChnl.dbRntChnlExpediaRoomTypeRatePlanLinkDefinitionTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)).Count() > 0)
                            dcChnl.Delete(dcChnl.dbRntChnlExpediaRoomTypeRatePlanLinkDefinitionTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)));
                        if (dcChnl.dbRntChnlExpediaRoomTypeRatePlanDayOfWeekBookingRestrictionTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)).Count() > 0)
                            dcChnl.Delete(dcChnl.dbRntChnlExpediaRoomTypeRatePlanDayOfWeekBookingRestrictionTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)));
                        if (dcChnl.dbRntChnlExpediaRoomTypeRatePlanCompensationTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)).Count() > 0)
                            dcChnl.Delete(dcChnl.dbRntChnlExpediaRoomTypeRatePlanCompensationTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)));
                        if (dcChnl.dbRntChnlExpediaRoomTypeRatePlanCancelPolicyTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)).Count() > 0)
                            dcChnl.Delete(dcChnl.dbRntChnlExpediaRoomTypeRatePlanCancelPolicyTBLs.Where(x => roomTypeIds.Contains(x.RoomTypeId)));
                        dcChnl.SaveChanges();
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
            using (DCchnlExpedia dc = new DCchnlExpedia())
            {
                currTBL = dc.dbRntChnlExpediaHotelTBLs.SingleOrDefault(x => x.HotelId == HfId.Value.ToInt32());

                if (currTBL == null) { currTBL = new dbRntChnlExpediaHotelTBL(); pnl_update.Visible = false; }
                else { pnl_update.Visible = true;  }

                txt_HotelId.Text = currTBL.HotelId + "";
                drp_isActive.setSelectedValue(currTBL.isActive);
                txt_name.Text = currTBL.name;
                txt_city.Text = currTBL.city;
                drp_isDemo.setSelectedValue(currTBL.isDemo);
                txt_username.Text = currTBL.username;
                txt_password.Text = currTBL.password;
 
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCchnlExpedia dc = new DCchnlExpedia())
            {
                string errorString = "";
                if (txt_HotelId.Text.Trim()=="" )
                    errorString += "<br/>- Inserire HotelId ";
                else if (txt_HotelId.Text != HfId.Value && dc.dbRntChnlExpediaHotelTBLs.SingleOrDefault(x => x.HotelId == txt_HotelId.Text.ToInt32()) != null)
                    errorString += "<br/>- HotelId duplicato";
                if (errorString != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString.htmlNoWrap() + "\", 340, 110);", true);
                    return;
                }
                currTBL = dc.dbRntChnlExpediaHotelTBLs.SingleOrDefault(x => x.HotelId == HfId.Value.ToInt64());
                if (currTBL == null)
                {
                    currTBL = new dbRntChnlExpediaHotelTBL();
                    currTBL.HotelId = txt_HotelId.Text.Trim().ToInt32();
                    dc.Add(currTBL);
                }
                currTBL.isActive = drp_isActive.getSelectedValueInt();
                currTBL.name = txt_name.Text;
                currTBL.city = txt_city.Text;
                currTBL.isDemo = drp_isDemo.getSelectedValueInt();
                currTBL.username = txt_username.Text;
                currTBL.password = txt_password.Text;

                { dc.SaveChanges(); }
                if (txt_name.Text.Trim() == "")
                {
                    errorString = ChnlExpediaImport.ProductRetrieval_start(currTBL.HotelId);
                    if (errorString != "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString.htmlNoWrap() + "\", 340, 110);", true);
                        return;
                    }
                }
                HfId.Value = txt_HotelId.Text.Trim();
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
            ChnlExpediaImport.BookingRetrieval_start(txt_HotelId.Text.Trim().ToInt32(), ntxt_numdays.Value.objToInt32());
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Operazione in corso...\", 340, 110);", true);
        }
    }

}