using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome
{
    public partial class pg_zone_details_new : mainBasePage
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
                ErrorLog.addLog(_ip, "pg_zone_details_new", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
            UC_apt_in_rome_bottom1.currZone = PAGE_REF_ID;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!IsPostBack)
                {                              

                    // carica ultima ricerca oppure nuova predefinita
                    clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                    clSearch _ls = _config.lastZoneSearch;    
                   
                    HF_currZoneList.Value = _ls.currZoneList.listToString("|");
                    HF_currExtrasList.Value = _ls.currConfigList.listToString("|");
                    HF_currPriceRange.Value = _ls.prMin + "|" + _ls.prMax;
                    HF_currVoteRange.Value = _ls.voteMinNew + "|" + _ls.voteMaxNew;                   

                    HF_currPage.Value = _ls.currPage.ToString();
                    // OrderByHow
                    HF_orderBy.Value = _ls.orderBy;
                    HF_orderHow.Value = _ls.orderHow;

                    txtTitle.Attributes.Add("placeholder", contUtils.getLabel("lblApartmentName"));
                    txtTitle.Text = _ls.searchTitle;
                }             
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
                ltr_img_banner.Text = _stp.img_banner;
            }
        }
    }
}