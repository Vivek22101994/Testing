using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.mobile
{
    public partial class Default : contStpHomeBasePage
    {
        public string CURRENT_SESSION_ID
        {
            get
            {
                mainBasePage mb = (mainBasePage)this.Page;
                return mb.CURRENT_SESSION_ID.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clConfig _config = clUtils.getConfig(this.CURRENT_SESSION_ID);
                clSearch _ls = _config.lastSearch;
                drp_zone_DataBind();
                drp_zone.setSelectedValue(_ls.currZoneList.FirstOrDefault());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "AjaxList_filter", "AjaxList_filter();", true);
            }
        }
        protected void drp_zone_DataBind()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.pid_lang == CurrentLang.ID && AppSettings.RNT_activeZones.Contains(x.id)).OrderBy(x => x.title).ToList();
            drp_zone.DataSource = _list;
            drp_zone.DataTextField = "title";
            drp_zone.DataValueField = "id";
            drp_zone.DataBind();
            if (drp_zone.Items.Count != 1)
                drp_zone.Items.Insert(0, new ListItem(contUtils.getLabel("lblSelectZone", App.LangID, "Select a Zone"), ""));
        }
    }
}