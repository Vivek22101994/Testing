using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateChnlExpedia_price : adminBasePage
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].objToInt32();
                ucNav.IdEstate = IdEstate;
                fillData();
            }
        }

        private void fillData()
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                ltr_apartment.Text = currEstate.code;
                var currTbl = dcChnl.dbRntChnlExpediaEstateTBLs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    currTbl = new dbRntChnlExpediaEstateTBL() { id = IdEstate, HotelId = 0, RoomTypeId = "" };
                    dcChnl.Add(currTbl);
                    dcChnl.SaveChanges();
                }
                if (currTbl.RoomTypeId == "")
                {
                    Response.Redirect("EstateChnlExpedia_main.aspx?id=" + IdEstate);
                }
                var roomTypeId = currTbl.RoomTypeId;
                var roomTypeTbl = dcChnl.dbRntChnlExpediaRoomTypeTBLs.SingleOrDefault(x => x.id == roomTypeId);
                if (roomTypeTbl == null)
                {
                    currTbl.RoomTypeId = "";
                    currTbl.HotelId = 0;
                    dcChnl.SaveChanges();
                    Response.Redirect("EstateChnlExpedia_main.aspx?id=" + IdEstate);
                    return;
                }
                //var ratePlanList = dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.Where(x => x.RoomTypeId == roomTypeId && x.rateAcquisitionType == "SellRate").OrderBy(x => x.RatePlanId).ToList();
                string rntExpediaRateAcquisitionType = CommonUtilities.getSYS_SETTING("rntExpediaRateNotAllowed");
                List<string> lstExpediaRateNotAllowed = rntExpediaRateAcquisitionType.splitStringToList(",");
                var ratePlanList = dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.Where(x => x.RoomTypeId == roomTypeId && !lstExpediaRateNotAllowed.Contains(x.rateAcquisitionType)).OrderBy(x => x.RatePlanId).ToList();
                LvRatePlans.DataSource = ratePlanList;
                LvRatePlans.DataBind();
                foreach (ListViewDataItem item in LvRatePlans.Items)
                {
                    var lbl_id = item.FindControl("lbl_id") as Label;
                    var ratePlanId = lbl_id.Text;
                    var drp_rate_changeIsDiscount = item.FindControl("drp_rate_changeIsDiscount") as DropDownList;
                    var ntxt_rate_changeAmount = item.FindControl("ntxt_rate_changeAmount") as RadNumericTextBox;
                    var drp_rate_changeIsPercentage = item.FindControl("drp_rate_changeIsPercentage") as DropDownList;
                    var ratePlan = ratePlanList.SingleOrDefault(x => x.RatePlanId == ratePlanId);
                    if (ratePlan == null)
                    {
                        item.Visible = false;
                        continue;
                    }
                    drp_rate_changeIsDiscount.setSelectedValue(ratePlan.rate_changeIsDiscount.objToInt32());
                    ntxt_rate_changeAmount.Value = ratePlan.rate_changeAmount.objToInt32();
                    drp_rate_changeIsPercentage.setSelectedValue(ratePlan.rate_changeIsPercentage.objToInt32());
                }
            } 
        }
        private void saveData()
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                var currTbl = dcChnl.dbRntChnlExpediaEstateTBLs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    currTbl = new dbRntChnlExpediaEstateTBL() { id = IdEstate, HotelId = 0, RoomTypeId = "" };
                    dcChnl.Add(currTbl);
                    dcChnl.SaveChanges();
                }
                if (currTbl.RoomTypeId == "")
                {
                    Response.Redirect("EstateChnlExpedia_main.aspx?id=" + IdEstate);
                }
                var roomTypeId = currTbl.RoomTypeId;
                var roomTypeTbl = dcChnl.dbRntChnlExpediaRoomTypeTBLs.SingleOrDefault(x => x.id == roomTypeId);
                if (roomTypeTbl == null)
                {
                    currTbl.RoomTypeId = "";
                    currTbl.HotelId = 0;
                    dcChnl.SaveChanges();
                    Response.Redirect("EstateChnlExpedia_main.aspx?id=" + IdEstate);
                    return;
                }
                foreach (ListViewDataItem item in LvRatePlans.Items)
                {
                    if (!item.Visible) 
                        continue;
                    var lbl_id = item.FindControl("lbl_id") as Label;
                    var ratePlanId = lbl_id.Text;
                    var drp_rate_changeIsDiscount = item.FindControl("drp_rate_changeIsDiscount") as DropDownList;
                    var ntxt_rate_changeAmount = item.FindControl("ntxt_rate_changeAmount") as RadNumericTextBox;
                    var drp_rate_changeIsPercentage = item.FindControl("drp_rate_changeIsPercentage") as DropDownList;
                    var ratePlan = dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.SingleOrDefault(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId);
                    if (ratePlan == null)
                        continue;
                    ratePlan.rate_changeIsDiscount = drp_rate_changeIsDiscount.getSelectedValueInt();
                    ratePlan.rate_changeAmount = ntxt_rate_changeAmount.Value.objToInt32();
                    ratePlan.rate_changeIsPercentage = drp_rate_changeIsPercentage.getSelectedValueInt();
                    dcChnl.SaveChanges();
                }
                ChnlExpediaUpdate.UpdateSplitRates_start(IdEstate, new List<string>());
                //ChnlExpediaUpdate.UpdateRatesWithSplitDates(IdEstate, new List<string>());
                //ChnlExpediaUpdate.UpdateRates_start(IdEstate,new List<string>());
                //ChnlExpediaUpdate.Property_start(IdEstate, App.HOST);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.<br />Invio prezzi in corso..\", 340, 110);", true);
                fillData();
            } 
        }

        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }
}