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
    public partial class ChnlExpediaRoomTypeList : adminBasePage
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
        protected dbRntChnlExpediaRoomTypeTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            IdEstate = Request.QueryString["for"].ToInt32();
            if (!IsPostBack)
            {
                using (DCchnlExpedia dc = new DCchnlExpedia())
                {

                }
            }
        }
        List<dbRntChnlExpediaHotelTBL> hotelList;
        protected string HotelName(int HotelId)
        {
            if(hotelList==null)
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
                hotelList = dcChnl.dbRntChnlExpediaHotelTBLs.ToList();
            var hotelTbl = hotelList.SingleOrDefault(x=>x.HotelId==HotelId);
            return hotelTbl != null ? hotelTbl.name : "- - -";
        }
        protected bool PropertyKeyIsFree(string RoomTypeId)
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                return dcChnl.dbRntChnlExpediaEstateTBLs.FirstOrDefault(x => x.RoomTypeId == RoomTypeId) == null;
            }
        }
        protected int PropertyKeyEstateId(string RoomTypeId)
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                var currTbl = dcChnl.dbRntChnlExpediaEstateTBLs.FirstOrDefault(x => x.RoomTypeId == RoomTypeId);
                return currTbl == null ? 0 : currTbl.id;
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCchnlExpedia dc = new DCchnlExpedia())
                {
                    //currTBL = dc.dbRntChnlExpediaRoomTypeTBLs.SingleOrDefault(x => x.ownerId == lbl_id.Text.ToInt64());
                    //if (currTBL != null)
                    //{
                    //    dc.Delete(currTBL);
                    //    if (UserAuthentication.CurrRoleTBL.viewOnlyNoSave.objToInt32() == 0) { dc.SaveChanges(); }
                    //}
                }
                closeDetails(false);
            }
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        protected void lnkGetPropertys_Click(object sender, EventArgs e)
        {
            ChnlExpediaImport.ProductRetrieval_all();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Elenco aggiornato.\", 340, 110);", true);

            closeDetails(true);
        }
    }

}