using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome
{
    public partial class stp_blogHomePage : contStpBasePage
    {
        public long currTag
        {
            get { return HF_currTag.Value.ToInt64(); }
            set { HF_currTag.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                currTag = Request.QueryString["tag"].ToInt64();
                var articleList = blogProps.ArticleVIEW.Where(x => x.pidLang == App.LangID && x.isActive == 1 && !string.IsNullOrEmpty(x.pagePath) && !string.IsNullOrEmpty(x.title) && x.publicDate.HasValue).OrderByDescending(x => x.publicDate);
                if (currTag != 0)
                {
                    var tagRL = blogProps.ArticleTagRL.Where(y => y.pidTag == currTag).Select(x => x.pidArticle).ToList();
                    articleList = articleList.Where(x => tagRL.Contains(x.id)).OrderByDescending(x => x.publicDate);
                    ucBlogArticleList.loadLast(2);
                }
                if (Request.QueryString["y"].ToInt32() != 0)
                    articleList = articleList.Where(x => x.publicDate.Value.Year == Request.QueryString["y"].ToInt32()).OrderByDescending(x => x.publicDate);
                if (Request.QueryString["m"].ToInt32() != 0)
                    articleList = articleList.Where(x => x.publicDate.Value.Month == Request.QueryString["m"].ToInt32()).OrderByDescending(x => x.publicDate);
      
                LV.DataSource = articleList;
                LV.DataBind();
            }
        }
    }
}