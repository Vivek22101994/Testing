using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.uc
{
    public partial class ucBlogArticleList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void loadLast(int count)
        {
            var articleList = blogProps.ArticleVIEW.Where(x => x.pidLang == App.LangID && x.isActive == 1 && !string.IsNullOrEmpty(x.pagePath) && !string.IsNullOrEmpty(x.title)).OrderByDescending(x => x.publicDate).Take(count);
            LV.DataSource = articleList;
            LV.DataBind();
            Literal ltrTitle = LV.FindControl("ltrTitle") as Literal;
            if (ltrTitle != null)
                ltrTitle.Text = contUtils.getLabel("lblPostRecenti");
        }
        public void loadLastWithTag(List<long> tagIds, long excludeArticle, int count)
        {
            var tagRL = blogProps.ArticleTagRL.Where(y => tagIds.Contains(y.pidTag)).Select(x => x.pidArticle).ToList();
            var articleList = blogProps.ArticleVIEW.Where(x => x.id != excludeArticle && x.pidLang == App.LangID && x.isActive == 1 && !string.IsNullOrEmpty(x.pagePath) && !string.IsNullOrEmpty(x.title) && tagRL.Contains(x.id)).OrderByDescending(x => x.publicDate).Take(count);
            LV.DataSource = articleList;
            LV.DataBind();
            Literal ltrTitle = LV.FindControl("ltrTitle") as Literal;
            if (ltrTitle != null)
                ltrTitle.Text = contUtils.getLabel("lblPostCorrelati");
        }
    }
}