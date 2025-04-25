using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.WLRental.uc
{
    public partial class UC_apt_in_rome_bottom : System.Web.UI.UserControl
    {
        public int currZone
        {
            get
            {
                return HF_currZone.Value.ToInt32();
            }
            set
            {
                HF_currZone.Value = value.ToString();
            }
        }
        public string currType
        {
            get
            {
                return HF_currType.Value;
            }
            set
            {
                HF_currType.Value = value;
            }
        }
        protected class ZoneClass
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Img { get; set; }
            public string Count { get; set; }
            public string Path { get; set; }
            public ZoneClass()
            {
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<int> lstAgentEstate = new List<int>();
                using (DCmodRental dc = new DCmodRental())
                {
                    lstAgentEstate = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == App.WLAgentId).Select(x => x.pidEstate).Distinct().ToList();
                }
                var estateList = AppSettings.RNT_estateList.Where(x => lstAgentEstate.Contains(x.id)).ToList();
               
                List<int> agentZoneId= estateList.Select(x=>x.pid_zone).Distinct().ToList();
                List<int> finalZoneIds = AppSettings._LOC_CUSTOM_ZONEs.Where(x => agentZoneId.Contains(x)).ToList();
                
                List<ZoneClass> _list = new List<ZoneClass>();
                foreach (int id in finalZoneIds)
                {
                    List<int> estateIds = estateList.Where(x => x.pid_zone == id).Select(x => x.id).ToList();
                    if (currZone == id) continue;
                    int cnt = CurrentSource.locZone_countEstate_WL(id, estateIds);
                    //if (cnt == 0) continue;
                    ZoneClass _zone = new ZoneClass();
                    _zone.Id = id;
                    _zone.Title = CurrentSource.locZone_title(id, CurrentLang.ID, "");
                    _zone.Img = CurrentSource.locZone_img_preview(id, CurrentLang.ID, "");
                    _zone.Count = "<em>" + cnt + "</em>&nbsp;" + CurrentSource.getSysLangValue("reqApartments");
                    _zone.Path = CurrentSource.getPagePath("" + id, "pg_zone", CurrentLang.ID.ToString());
                    _list.Add(_zone);
                }
                if (currZone != 19)
                {
                    List<int> estateIds = estateList.Where(x => x.pid_zone == 19).Select(x => x.id).ToList();
                    if (estateIds.Count > 0)
                    {
                        int cnt = CurrentSource.locZone_countEstate_WL(19, estateIds);                    
                        ZoneClass _zoneOther = new ZoneClass();
                        _zoneOther.Id = 19;
                        _zoneOther.Title = CurrentSource.getSysLangValue("lblOtherArea");
                        _zoneOther.Img = "images/css/tutte_thumb.jpg";
                        _zoneOther.Count = "<em>" + cnt + "</em>&nbsp;" + CurrentSource.getSysLangValue("reqApartments");
                        _zoneOther.Path = CurrentSource.getPagePath("19", "pg_zone", CurrentLang.ID.ToString());
                        _list.Add(_zoneOther);
                    }
                }
                if (currType == "bottom")
                {
                    LV_bottom.DataSource = _list;
                    LV_bottom.DataBind();
                }
                if (currType == "sxZone")
                {
                    LV_sxZone.DataSource = _list;
                    LV_sxZone.DataBind();
                }
                if (currType == "zoneHome")
                {
                    LV_zoneHome.DataSource = _list;
                    LV_zoneHome.DataBind();
                }
            }
        }

        protected void LV_bottom_DataBound(object sender, EventArgs e)
        {
            Literal _ltr = LV_bottom.FindControl("ltr_title") as Literal;
            if (_ltr != null) _ltr.Text = CurrentSource.getSysLangValue("lblApartmentsInRome");
        }
        protected void LV_sxZone_DataBound(object sender, EventArgs e)
        {
            Literal _ltr = LV_sxZone.FindControl("ltr_title") as Literal;
            if (_ltr != null) _ltr.Text = CurrentSource.getSysLangValue("lblDiscoverOtherAreas");
        }
    }
}