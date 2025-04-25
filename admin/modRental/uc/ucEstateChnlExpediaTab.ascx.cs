using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental.uc
{
    public partial class ucEstateChnlExpediaTab : System.Web.UI.UserControl
    {
        protected string RoomTypeId
        {
            get
            {
                return HF_RoomTypeId.Value;
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
                using (DCchnlExpedia dcChnl = new DCchnlExpedia())
                {
                    var currTbl = dcChnl.dbRntChnlExpediaEstateTBLs.SingleOrDefault(x => x.id == value);
                    if (currTbl == null)
                    {
                        currTbl = new dbRntChnlExpediaEstateTBL() { id = value, HotelId = 0, RoomTypeId = "" };
                        dcChnl.Add(currTbl);
                        dcChnl.SaveChanges();
                    }
                    RoomTypeId = currTbl.RoomTypeId;
                }
                HF_IdEstate.Value = value.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                displayContentLinks();
            }
        }

        protected void lnk_send_availablity_Click(object sender, EventArgs e)
        {
            ChnlExpediaUpdate.UpdateAvailability_start(IdEstate);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Invio disponibilità in corso..\", 340, 110);", true);
        }

        protected void lnk_send_price_Click(object sender, EventArgs e)
        {
            ChnlExpediaUpdate.UpdateSplitRates_start(IdEstate, new List<string>());
            //ChnlExpediaUpdate.UpdateSplitRates_start(IdEstate, new List<string>());
            //ChnlExpediaUpdate.UpdateRatesWithSplitDates(IdEstate, new List<string>());
            //ChnlExpediaUpdate.UpdateRates_start(IdEstate, new List<string>());
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Invio prezzi in corso..\", 340, 110);", true);
        }

        protected void lnk_send_content_Click(object sender, EventArgs e)
        {
            RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
            if (currEstate == null)
            {
                return;
            }

            List<string> amenities = new List<string>();
            using (DCmodRental dc = new DCmodRental())
            {
                using (DCchnlExpedia dcChnl = new DCchnlExpedia())
                {
                    var extrasList = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == currEstate.id).ToList();
                    foreach (var extra in extrasList)
                    {
                        var currExpExtra = dcChnl.dbRntChnlExpediaLkAmenitiesTBLs.SingleOrDefault(x => x.refId == extra.pid_config);
                        if (currExpExtra != null)
                        {
                            amenities.Add(currExpExtra.name);
                        }

                        var expediaRoomAmenties = dcChnl.dbRntChnlExpediaLKRoomAmenitiesTBLs.SingleOrDefault(x => x.refId == extra.pid_config);
                        if (expediaRoomAmenties != null)
                        {
                            amenities.Add(expediaRoomAmenties.name);
                        }
                    }



                }

                var images = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate).ToList();
                var EstateInternsIds = dc.dbRntEstateInternsTBs.Where(x => x.pidEstate == currEstate.id && (x.pidInternsType == "Bedroom" || x.pidInternsType == "Livingroom")).Select(x => x.id).ToList();
                var internCount = dc.dbRntEstateInternsFeatureRLs.Where(x => EstateInternsIds.Contains(x.pidEstateInterns)).ToList();

                string str = "";
                string sep = "";
                string error = "";

                if (currEstate.code == "")
                {
                    str = sep + "Name";
                    sep = " ,";
                }
                if (string.IsNullOrEmpty(currEstate.google_maps) || ((currEstate.google_maps + "") != "" && currEstate.google_maps.Split(',').Length < 1))
                {
                    str += sep + "Lattitude and Longitude";
                    sep = " ,";
                }
                if (currEstate.loc_address + "" == "")
                {
                    str += sep + "City";
                    sep = " ,";
                }
                if (currEstate.loc_zip_code + "" == "")
                {
                    str += sep + "Zip Code";
                    sep = " ,";
                }
                if (currEstate.pid_city.objToInt32() == 0)
                {
                    str += sep + "City";
                    sep = " ,";
                }
                //if (currEstate.pid_country.objToInt32() == 0)
                //{
                //    str += sep + "Country";
                //    sep = " ,";
                //}
                if (amenities.Count < 8)
                {
                    str += sep + "at least 10 amenity";
                    sep = " ,";
                }
                if (images != null && images.Count < 3)
                {
                    str += sep + "at least 3 images";
                    sep = " ,";
                }
                if (internCount.Count == 0)
                {
                    str += sep + "add interns";
                    sep = " ,";
                }
                if (currEstate.mq_inner.objToDecimal() == 0)
                {
                    str += sep + "add Internal sq.m.";
                    sep = " ,";
                }
                //if (currEstate.sqFeet.objToDecimal() == 0)
                //{
                //    str += sep + "add Internal sq.m.";
                //    sep = " ,";
                //}
                if (str != "")
                {
                    error += "Please add " + str;
                }
                if (error == "")
                {
                    ChnlExpediaUpdate.PropertyNew_start(IdEstate, App.HOST);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert2", "radalert(\"Sending Content in progress ..\", 340, 110);", true);
                    displayContentLinks();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert('" + error + "', 340, 110);", true);
                    return;
                }
            }
        }

        protected void displayContentLinks()
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate != null)
                {
                    //make send content invisible for child apartments
                    //if (currEstate.ComplexId > 0)
                    //lnk_send_content.Style.Add("display", "none");

                    //if (string.IsNullOrEmpty(currEstate.ComplexType) && currEstate.ComplexId.objToInt32() == 0)
                    lnk_create_room.Style.Add("display", "");
                    //else if (!string.IsNullOrEmpty(currEstate.ComplexType))
                    //  lnk_manage_room.Style.Add("display", "");

                    //if (string.IsNullOrEmpty(currEstate.ComplexType))
                    //{
                    var currRoom = dcChnl.dbRntChnlExpediaEstateTBLs.FirstOrDefault(x => x.id == IdEstate && x.RoomTypeId != null && x.RoomTypeId != "");
                    if (currRoom != null)
                    {
                        lnk_create_room.Text = "Update Room";
                        lnk_manage_ratePlans.Style.Add("display", "");
                        lnk_send_room_amenity.Style.Add("display", "");
                    }


                    var currHotel = new dbRntChnlExpediaHotelTBL();
                    currHotel = dcChnl.dbRntChnlExpediaHotelTBLs.SingleOrDefault(x => x.pidEstate == IdEstate);

                    if (currHotel != null)
                    {
                        if (currHotel.HotelId + "" != "")
                            lnk_create_room.Style.Add("display", "");
                        else
                            lnk_create_room.Style.Add("display", "none");

                        lnk_send_status.Style.Add("display", "");
                    }
                    else
                    {
                        lnk_create_room.Style.Add("display", "none");
                        lnk_send_status.Style.Add("display", "none");
                    }
                    //}
                }
            }
        }

        protected void lnk_create_room_Click(object sender, EventArgs e)
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                var currRoom = dcChnl.dbRntChnlExpediaEstateTBLs.FirstOrDefault(x => x.id == IdEstate && x.RoomTypeId != null && x.RoomTypeId != "");
                if (currRoom != null)
                {
                    //var currRoom = dcChnl.dbRntChnlExpediaEstateRoomRLs.FirstOrDefault(x => x.pidEstate == IdEstate);
                    //if (currRoom != null)
                    //{
                    ChnlExpediaUpdate.RoomTypeUpdate_start(IdEstate, App.HOST);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert3", "radalert(\"update rooms in progress ..\", 340, 110);", true);
                }
                else
                {
                    ChnlExpediaUpdate.RoomTypeNew_start(IdEstate, App.HOST);
                    displayContentLinks();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert4", "radalert(\"create rooms in progress ..\", 340, 110);", true);
                }
            }
        }

        protected void lnk_manage_room_Click(object sender, EventArgs e)
        {
            Response.Redirect("/admin/modRental/EstateChnlExpedia_room.aspx?id=" + IdEstate);
        }

        protected void lnk_manage_ratePlans_Click(object sender, EventArgs e)
        {
            Response.Redirect("/admin/modRental/EstateChnlExpedia_manageRateplans.aspx?id=" + IdEstate);
        }


        protected void lnk_send_room_amenity_Click(object sender, EventArgs e)
        {
            ChnlExpediaUpdate.RoomTypeCreateAmenity_start(IdEstate, App.HOST);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert4", "radalert(\"send room amenities in progress ..\", 340, 110);", true);
        }

        protected void lnk_send_status_Click(object sender, EventArgs e)
        {
            ChnlExpediaUpdate.GetStatus_start(IdEstate, App.HOST);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert4", "radalert(\"Get Status in progress ..\", 340, 110);", true);

        }
    }
}