using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.ucMain
{
    public partial class ucZone : System.Web.UI.UserControl
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

        
        protected int animationDelay
        {
            get
            {
                return HF_animationDelay.Value.ToInt32();
            }
            set
            {
                HF_animationDelay.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<ZoneClass> _list = new List<ZoneClass>();
                foreach (int id in AppSettings._LOC_CUSTOM_ZONEs)
                {
                    if (currZone == id) continue;
                    ZoneClass _zone = new ZoneClass();
                    _zone.Id = id;
                    _zone.Title = CurrentSource.locZone_title(id, CurrentLang.ID, "");
                    _zone.Img = CurrentSource.locZone_img_preview(id, CurrentLang.ID, "");
                    _zone.Count = "<em>" + CurrentSource.locZone_countEstate(id) + "</em>&nbsp;" + CurrentSource.getSysLangValue("reqApartments");
                    _zone.Path = CurrentSource.getPagePath("" + id, "pg_zone", CurrentLang.ID.ToString());
                    _list.Add(_zone);
                }
                if (currZone != 19)
                {
                    ZoneClass _zoneOther = new ZoneClass();
                    _zoneOther.Id = 19;
                    _zoneOther.Title = CurrentSource.getSysLangValue("lblOtherArea");
                    _zoneOther.Img = "images/css/tutte_thumb.jpg";
                    _zoneOther.Count = "<em>" + CurrentSource.locZone_countEstate(19) + "</em>&nbsp;" + CurrentSource.getSysLangValue("reqApartments");
                    _zoneOther.Path = CurrentSource.getPagePath("19", "pg_zone", CurrentLang.ID.ToString());
                    _list.Add(_zoneOther);
                }
                //if (currType == "bottom")
                //{
                //    LV_bottom.DataSource = _list;
                //    LV_bottom.DataBind();
                //}
                //if (currType == "sxZone")
                //{
                //    LV_sxZone.DataSource = _list;
                //    LV_sxZone.DataBind();
                //}
                if (currType == "zoneHome")
                {
                    LVZoneHome.DataSource = _list;
                    LVZoneHome.DataBind();
                }
            }
        }

        //protected void LV_bottom_DataBound(object sender, EventArgs e)
        //{
        //    Literal _ltr = LV_bottom.FindControl("ltr_title") as Literal;
        //    if (_ltr != null) _ltr.Text = CurrentSource.getSysLangValue("lblApartmentsInRome");
        //}
        //protected void LV_sxZone_DataBound(object sender, EventArgs e)
        //{
        //    Literal _ltr = LV_sxZone.FindControl("ltr_title") as Literal;
        //    if (_ltr != null) _ltr.Text = CurrentSource.getSysLangValue("lblDiscoverOtherAreas");
        //}

        protected int GetAnimationDelay()
        {
            animationDelay = animationDelay + 100;
            return animationDelay;
        }
    }
}