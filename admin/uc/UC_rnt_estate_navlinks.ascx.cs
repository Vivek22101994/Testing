using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_estate_navlinks : System.Web.UI.UserControl
    {
        public int IdEstate
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected string getDetailClass()
        {
            List<RNT_LN_ESTATE> _rList = AppSettings.RNT_LN_ESTATE.Where(x => x.pid_estate == IdEstate).ToList();
            RNT_LN_ESTATE _lang = _rList.FirstOrDefault(x => x.title == null || x.title.Trim() == "" || x.meta_title == null || x.meta_title.Trim() == "" || x.meta_description == null || x.meta_description.Trim() == "" || x.description == null || x.description.Trim() == "");
            if (_rList.Count == 0 || _rList.Count < contProps.LangTBL.Where(x=>x.is_active==1).Count())
                return "alert1";
            if (_lang != null)
                return "alert2";
            return "";
        }
        protected string getPriceClass()
        {
            //List<TBL_HOTEL_PRICE_PERIOD> _rList = magaestate_DataContext.DC_HOTEL.TBL_HOTEL_PRICE_PERIODs.Where(x => x.pid_hotel == int.Parse(pid)).ToList();
            //if (_rList.Count == 0)
            //    return "alert1";
            //else
            //{
            //    List<int> _rooms = magaestate_DataContext.DC_HOTEL.TBL_HOTEL_ROOMs.Where(x => x.pid_hotel == int.Parse(pid)).Select(x => x.id).ToList();
            //    List<int> _roomCats = magaestate_DataContext.DC_HOTEL.RL_HOTEL_ROOM_CATEGORies.Where(x => _rooms.Contains(x.pid_room)).Select(x => x.pid_category).ToList();
            //    List<int> _catList = magaestate_DataContext.DC_HOTEL.TBL_HOTEL_ROOM_CATEGORies.Where(x => _roomCats.Contains(x.id)).Select(x => x.id).ToList();
            //    if (_catList.Count == 0)
            //        return "alert2";
            //    else
            //    {
            //        foreach (TBL_HOTEL_PRICE_PERIOD _r in _rList)
            //        {
            //            foreach (int _cat in _catList)
            //            {
            //                if (magaestate_DataContext.DC_HOTEL.TBL_HOTEL_ROOM_PRICEs.FirstOrDefault(x => x.pid_hotel_room_category == _cat && x.pid_price_period == _r.id && x.pid_hotel == int.Parse(pid) && x.price != null && x.price != 0) == null)
            //                    return "alert2";
            //            }
            //        }
            //    }
            //}
            return "";
        }
        protected void DeleteRecord(object sender, EventArgs args)
        {
            //int id = int.Parse(((System.Web.UI.WebControls.LinkButton)(sender)).CommandArgument);
            //_currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(item => item.id == id);
            //if (_currTBL != null)
            //{
            //    _currTBL.is_deleted = 1;
            //    _currTBL.is_active = 0;
            //    DC_RENTAL.SubmitChanges();
            //    LV.DataBind();
            //    return;
            //}
        }
    }
}