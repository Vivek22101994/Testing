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
    public partial class EstateChnlExpedia_rates : adminBasePage
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
                    bind_ratePlans(currTbl.RoomTypeId);
                }
            }
        }

        protected void lnk_get_rates_Click(object sender, EventArgs e)
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
                if (rdp_dtEnd.SelectedDate != null && rdp_dtEnd.SelectedDate != null)
                    ChnlExpediaImport.AvailRateRetrieval_start(currTbl.HotelId, rdp_dtStart.SelectedDate.Value, rdp_dtEnd.SelectedDate.Value);
                else
                    ChnlExpediaImport.AvailRateRetrieval_start(currTbl.HotelId, (DateTime?)null, (DateTime?)null);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Elenco aggiornato.\", 340, 110);", true);
            }
        }

        protected void lnk_view_rates_Click(object sender, EventArgs e)
        {
            string errorString = "";
            if (!rdp_view_dtStart.SelectedDate.HasValue || !rdp_view_dtEnd.SelectedDate.HasValue)
                errorString += "<br/>-specificare inizio e fine del range";
            else if (rdp_view_dtStart.SelectedDate > rdp_view_dtEnd.SelectedDate)
                errorString += "<br/>-la data di fine deve essere successiva alla data di inizio";
            if (errorString != "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                return;
            }
            div_rates.Visible = true;
            div_availability.Visible = false;
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

                var rates=  dcChnl.dbRntChnlExpediaRatesTBLs.Where(x => x.RoomTypeId == currTbl.RoomTypeId && x.date >= rdp_view_dtStart.SelectedDate.Value && x.date <= rdp_view_dtEnd.SelectedDate.Value).ToList();
                
                if (drp_ratePlanId.SelectedValue!="0")
                    rates = rates.Where(x => x.RatePlanId == drp_ratePlanId.SelectedValue).ToList();

                LV_rates.DataSource = rates;
                LV_rates.DataBind();
            }
        }

        protected void lnk_view_availability_Click(object sender, EventArgs e)
        {
            string errorString = "";
            if (!rdp_view_dtStart.SelectedDate.HasValue || !rdp_view_dtEnd.SelectedDate.HasValue)
                errorString += "<br/>-specificare inizio e fine del range";
            else if (rdp_view_dtStart.SelectedDate > rdp_view_dtEnd.SelectedDate)
                errorString += "<br/>-la data di fine deve essere successiva alla data di inizio";
            if (errorString != "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                return;
            }

            div_rates.Visible = false;
            div_availability.Visible = true;

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

                LV_availability.DataSource = dcChnl.dbRntChnlExpediaAvailabilityTBLs.Where(x => x.RoomTypeId == currTbl.RoomTypeId && x.date >= rdp_view_dtStart.SelectedDate.Value && x.date <= rdp_view_dtEnd.SelectedDate.Value).ToList();
                LV_availability.DataBind();
            }
        }

        protected void bind_ratePlans(string roomTypeId)
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                var ratePlans = dcChnl.dbRntChnlExpediaRoomTypeRatePlanTBLs.Where(x => x.RoomTypeId == roomTypeId).ToList();

                foreach (dbRntChnlExpediaRoomTypeRatePlanTBL objRatePlan in ratePlans)
                {
                    drp_ratePlanId.Items.Add(new ListItem(objRatePlan.RatePlanId + " - " + objRatePlan.rateAcquisitionType + " - " + (objRatePlan.status.objToInt32()==1?"Active":"InActive"), objRatePlan.RatePlanId));

                }
                drp_ratePlanId.Items.Insert(0, new ListItem("----","0"));
            }
        }
    }
}