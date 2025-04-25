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
    public partial class EstateChnlExpedia_main : adminBasePage
    {
        public class MsgClass
        {
            public string Msg { get; set; }
            public string Type { get; set; }
            public MsgClass(string msg, string type)
            {
                Msg = msg;
                Type = type;
            }
        }
        private static string LastMsgSessionKey = "EstateChnlExpedia_main_LastMsg";
        protected MsgClass LastMsg
        {
            get
            {
                if (HttpContext.Current.Session[LastMsgSessionKey] != null)
                {
                    return (MsgClass)HttpContext.Current.Session[LastMsgSessionKey];
                }
                return new MsgClass("", "");
            }
            set
            {
                HttpContext.Current.Session[LastMsgSessionKey] = value;
            }
        }
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
                if (!string.IsNullOrEmpty(Request.QueryString["set"]))
                {
                    using (DCchnlExpedia dcChnl = new DCchnlExpedia())
                    {
                        var propertyTbl = dcChnl.dbRntChnlExpediaRoomTypeTBLs.SingleOrDefault(x => x.id == Request.QueryString["set"] + "");
                        if (propertyTbl != null)
                        {
                            var currTbl = dcChnl.dbRntChnlExpediaEstateTBLs.SingleOrDefault(x => x.id == IdEstate);
                            if (currTbl == null)
                            {
                                currTbl = new dbRntChnlExpediaEstateTBL() { id = IdEstate, HotelId = 0, RoomTypeId = "" };
                                dcChnl.Add(currTbl);
                                dcChnl.SaveChanges();
                            }
                            currTbl.RoomTypeId = propertyTbl.id;
                            currTbl.HotelId = propertyTbl.HotelId;
                            dcChnl.SaveChanges();
                        }
                        Response.Redirect("EstateChnlExpedia_main.aspx?id=" + IdEstate);
                        return;
                    }
                }
                if (Request.QueryString["unset"] == "true")
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
                        currTbl.RoomTypeId = "";
                        currTbl.HotelId = 0;
                        dcChnl.SaveChanges();
                        Response.Redirect("EstateChnlExpedia_main.aspx?id=" + IdEstate);
                        return;
                    }
                }
                ucNav.IdEstate = IdEstate;
                fillData();
                if (LastMsg.Msg != "")
                {
                    pnlError.Visible = true;
                    pnlError.Attributes["class"] = LastMsg.Type;
                    ltrErorr.Text = LastMsg.Msg;
                    LastMsg = new MsgClass("", "");
                }
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
                pnlNotConnected.Visible = currTbl.RoomTypeId == "";
                if (currTbl.RoomTypeId == "") return;
                var roomTypeId = currTbl.RoomTypeId;
                var roomTypeTbl = dcChnl.dbRntChnlExpediaRoomTypeTBLs.SingleOrDefault(x => x.id == roomTypeId);
                if (roomTypeTbl == null)
                {
                    currTbl.RoomTypeId = "";
                    currTbl.HotelId = 0;
                    dcChnl.SaveChanges();
                    pnlNotConnected.Visible = currTbl.RoomTypeId == "";
                    return;
                }
                pnlRoomTypeDetails.Visible = true;

                txt_code.Text = roomTypeTbl.code;
                txt_name.Text = roomTypeTbl.name;
                txt_status.Text = roomTypeTbl.status == 1 ? "Active" : "Inactive";
                txt_smokingPref.Text = roomTypeTbl.smokingPref;
                txt_maxOccupants.Text = roomTypeTbl.maxOccupants + "";

                LvBedType.DataSource = dcChnl.dbRntChnlExpediaRoomTypeBedTypeTBLs.Where(x => x.RoomTypeId == roomTypeId).OrderBy(x => x.name).ToList();
                LvBedType.DataBind();

                LvOccupancyByAge.DataSource = dcChnl.dbRntChnlExpediaRoomTypeOccupancyByAgeTBLs.Where(x => x.RoomTypeId == roomTypeId).OrderBy(x => x.minAge).ToList();
                LvOccupancyByAge.DataBind();

                LvRateThreshold.DataSource = dcChnl.dbRntChnlExpediaRoomTypeRateThresholdTBLs.Where(x => x.RoomTypeId == roomTypeId).OrderBy(x => x.type).ToList();
                LvRateThreshold.DataBind();
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