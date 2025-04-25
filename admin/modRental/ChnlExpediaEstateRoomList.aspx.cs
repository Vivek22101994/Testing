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
    public partial class ChnlExpediaEstateRoomList : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

            if (e.CommandName == "edit")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                HF_IdEstate.Value = lbl_id.Text;
                FillData();
            }

        }

        protected void FillData()
        {
            using (DCchnlExpedia dc = new DCchnlExpedia())
            {
                var connectedEstates = dc.dbRntChnlExpediaEstateTBLs.Where(x => x.HotelId > 0).Select(x => x.id).ToList();
                var currTBL = dc.dbRntChnlExpediaEstateTBLs.SingleOrDefault(x => x.id == HF_IdEstate.Value.objToInt32());
                ltr_est_title.Text = GetMagaRentalProeprtyname(HF_IdEstate.Value.objToInt32());

                var removeEstates = dc.dbRntChnlExpediaEstateRoomRLs.Where(x => x.RoomTypeId != currTBL.RoomTypeId ).Select(x => x.pidEstate).Distinct().ToList();
                removeEstates.Add(HF_IdEstate.Value.objToInt32()); // remove current Room
                removeEstates.AddRange(connectedEstates);
                if (currTBL != null)
                {
                    //Rdb_OtherRoom.DataSource = AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == 1 && !removeEstates.Contains(x.id) > 0).ToList();
                    Rdb_OtherRoom.DataSource = AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == 1).ToList();
                    Rdb_OtherRoom.DataTextField = "code";
                    Rdb_OtherRoom.DataValueField = "id";
                    Rdb_OtherRoom.DataBind();

                    var currRooms = dc.dbRntChnlExpediaEstateRoomRLs.FirstOrDefault(x => x.RoomTypeId == currTBL.RoomTypeId && (x.RoomTypeId != null && x.RoomTypeId != ""));
                    if (currRooms != null)
                    {
                        Rdb_OtherRoom.setSelectedValue(currRooms.pidEstate);
                    }
                    //foreach (ListItem cb in chk_OtherRooms.Items)
                    //{
                    //    cb.Selected = currRooms.Contains(cb.Value.objToInt32());

                    //    //if (cb.Selected)
                    //    //{
                    //    //    var ExpediaEstateRoomRL = new dbRntChnlExpediaEstateRoomRL();
                    //    //    ExpediaEstateRoomRL.pidEstate = cb.Value.objToInt32();
                    //    //    ExpediaEstateRoomRL.RoomTypeId = currTBL.RoomTypeId;
                    //    //    dc.Add(ExpediaEstateRoomRL);
                    //    //    dc.SaveChanges();
                    //    //}
                    //}
                }
            }

            rwdDett.VisibleOnPageLoad = true;

        }

        public string GetMagaRentalProeprtyname(int pidEstate)
        {
            var currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == pidEstate);
            if (currEstate != null)
                return currEstate.code;
            else
                return "";
        }

        public static List<dbRntChnlExpediaRoomTypeTBL> ChnlExpediaRoomType;

        public string GetExpediaPropertyName(string pidRoomType)
        {
            if (ChnlExpediaRoomType == null)
            {
                using (DCchnlExpedia DC = new DCchnlExpedia())
                {
                    ChnlExpediaRoomType = DC.dbRntChnlExpediaRoomTypeTBLs.ToList();
                }
            }


            var currRoom = ChnlExpediaRoomType.SingleOrDefault(x => x.id == pidRoomType);
            if (currRoom != null)
                return currRoom.name;
            else
                return "";
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }


        protected void closeDetails(bool pnlFasciaReload)
        {
            HF_IdEstate.Value = "0";
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            rwdDett.VisibleOnPageLoad = false;
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }


        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCchnlExpedia dc = new DCchnlExpedia())
            {
                var currTBL = dc.dbRntChnlExpediaEstateTBLs.SingleOrDefault(x => x.id == HF_IdEstate.Value.objToInt32());
                var ExpediaEstateRoomRL = dc.dbRntChnlExpediaEstateRoomRLs.SingleOrDefault(x => x.RoomTypeId == currTBL.RoomTypeId);
                if (Rdb_OtherRoom.getSelectedValueInt() > 0)
                {
                    if (ExpediaEstateRoomRL != null)
                    {
                        dc.Delete(ExpediaEstateRoomRL);
                        dc.SaveChanges();
                    }
                    ExpediaEstateRoomRL = new dbRntChnlExpediaEstateRoomRL();
                    ExpediaEstateRoomRL.RoomTypeId = currTBL.RoomTypeId;
                    ExpediaEstateRoomRL.pidEstate = Rdb_OtherRoom.getSelectedValueInt();
                    dc.Add(ExpediaEstateRoomRL);
                    dc.SaveChanges();
                }
                else
                {
                    if (ExpediaEstateRoomRL != null)
                    {
                        dc.Delete(ExpediaEstateRoomRL);
                        dc.SaveChanges();
                    }
                }
            }

            closeDetails(true);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            closeDetails(true);
        }
    }
}