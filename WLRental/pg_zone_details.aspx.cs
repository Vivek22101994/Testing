using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.WLRental
{
    public partial class pg_zone_details : mainBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "pg_zone";
            int id = Request.QueryString["id"].ToInt32();
            if (id != 0)
                base.PAGE_REF_ID = id;
            else
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "pg_zone_details", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
            UC_apt_in_rome_bottom1.currZone = PAGE_REF_ID;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clZoneFilter _zf = clUtils.getConfig(CURRENT_SESSION_ID).zoneFilters.SingleOrDefault(x => x.currZone == PAGE_REF_ID);
                if (_zf == null) _zf = new clZoneFilter();
                HF_searchTitle.Value = _zf.searchTitle;
                HF_orderBy.Value = _zf.orderBy;
                HF_orderHow.Value = _zf.orderHow;
                HF_currPage.Value = _zf.currPage.ToString();
                Fill_data();
            }
        }
        protected void Fill_data()
        {
            HF_id.Value = PAGE_REF_ID.ToString();
            HF_lang.Value = CurrentLang.ID.ToString();
            LOC_VIEW_ZONE _stp =
                AppSettings.LOC_ZONEs.SingleOrDefault(item => item.id == PAGE_REF_ID && item.pid_lang == CurrentLang.ID);
            if (_stp != null)
            {
                ltr_meta_description.Text = _stp.meta_description;
                ltr_meta_keywords.Text = _stp.meta_keywords;
                ltr_meta_title.Text = _stp.meta_title;
                ltr_title.Text = _stp.title;
                ltr_description.Text = _stp.description;
                if (ltr_description.Text != "")
                {
                    int startIndex = ltr_description.Text.IndexOf("<iframe");
                    int endIndex = ltr_description.Text.IndexOf("</iframe>");
                    int length = (endIndex - startIndex) + 9;

                    if (startIndex != -1 && endIndex != -1)
                        ltr_description.Text = ltr_description.Text.Replace(ltr_description.Text.Substring(startIndex, length), "");
                }
                ltr_img_banner.Text = _stp.img_banner;
            }
        }
    }
}
