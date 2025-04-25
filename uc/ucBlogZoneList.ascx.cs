using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModBlog;

namespace RentalInRome.uc
{
    public partial class ucBlogZoneList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) loadList();

        }
        protected void loadList()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                var articleList = blogProps.ArticleVIEW.Where(x => x.pidLang == App.LangID && x.isActive == 1 && !string.IsNullOrEmpty(x.pagePath) && !string.IsNullOrEmpty(x.title)).ToList();
                var zoneIdsList =   articleList.Select(x => x.pidZone.objToInt64()).Distinct().ToList();
                var currList = dc.dbBlogZoneLNs.Where(x => x.pidLang == App.LangID && zoneIdsList.Contains(x.pidZone) && !string.IsNullOrEmpty(x.pagePath)).OrderBy(x => x.title).ToList();
                LV.DataSource = currList;
                LV.DataBind();
                Literal ltrTitle = LV.FindControl("ltrTitle") as Literal;
                if (ltrTitle != null)
                    ltrTitle.Text = "Zone di Roma";//contUtils.getLabel("lblPostRecenti");
            }
        }
    }
}