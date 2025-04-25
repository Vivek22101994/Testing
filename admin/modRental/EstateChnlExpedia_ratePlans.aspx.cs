using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateChnlExpedia_ratePlans : adminBasePage
    {
        protected long RoomTypeId
        {
            get
            {
                return HF_RoomTypeId.Value.ToInt64();
            }
            set
            {
                HF_RoomTypeId.Value = value + "";
            }
        }
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
                IdEstate = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                ltr_apartment.Text = currEstate.code;
                ucNav.IdEstate = IdEstate;
                fillData();
            }
        }
        protected void fillData()
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

                LvRatePlans.DataSource = dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.Where(x => x.RoomTypeId == roomTypeId).OrderBy(x => x.RatePlanId).ToList();
                LvRatePlans.DataBind();
                foreach (ListViewDataItem item in LvRatePlans.Items)
                {
                    var lbl_id = item.FindControl("lbl_id") as Label;
                    var ratePlanId = lbl_id.Text;
                    var LvRatePlanLinkDefinition = item.FindControl("LvRatePlanLinkDefinition") as ListView;
                    LvRatePlanLinkDefinition.DataSource = dcChnl.dbRntChnlExpediaRoomTypeRatePlanLinkDefinitionTBLs.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId).OrderBy(x => x.linkType).ToList();
                    LvRatePlanLinkDefinition.DataBind();
                    var LvDayOfWeekBookingRestriction = item.FindControl("LvDayOfWeekBookingRestriction") as ListView;
                    LvDayOfWeekBookingRestriction.DataSource = dcChnl.dbRntChnlExpediaRoomTypeRatePlanDayOfWeekBookingRestrictionTBLs.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId).OrderBy(x => x.type).ToList();
                    LvDayOfWeekBookingRestriction.DataBind();
                    var LvCompensation = item.FindControl("LvCompensation") as ListView;
                    LvCompensation.DataSource = dcChnl.dbRntChnlExpediaRoomTypeRatePlanCompensationTBLs.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId).OrderBy(x => x._default).ThenBy(x => x.percent).ToList();
                    LvCompensation.DataBind();
                    var LvCancelPolicy = item.FindControl("LvCancelPolicy") as ListView;
                    LvCancelPolicy.DataSource = dcChnl.dbRntChnlExpediaRoomTypeRatePlanCancelPolicyTBLs.Where(x => x.RoomTypeId == roomTypeId && x.RatePlanId == ratePlanId).OrderBy(x => x._default).ThenBy(x => x.startDate).ToList();
                    LvCancelPolicy.DataBind();
                    foreach (ListViewDataItem item1 in LvCancelPolicy.Items)
                    {
                        var lbl_uid = item1.FindControl("lbl_id") as Label;
                        var LvPenalty = item1.FindControl("LvPenalty") as ListView;
                        LvPenalty.DataSource = dcChnl.dbRntChnlExpediaRoomTypeRatePlanCancelPolicyPenaltyTBLs.Where(x => x.CancelPolicyUid.ToString().ToLower() == lbl_uid.Text.ToLower()).OrderBy(x => x.insideWindow).ThenBy(x => x.flatFee).ToList();
                        LvPenalty.DataBind();
                    }
                }
            }
        }
        protected void saveData()
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