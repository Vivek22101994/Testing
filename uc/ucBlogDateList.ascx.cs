using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.uc
{
    public partial class ucBlogDateList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var articleList = blogProps.ArticleVIEW.Where(x => x.pidLang == App.LangID && x.isActive == 1 && !string.IsNullOrEmpty(x.pagePath) && !string.IsNullOrEmpty(x.title) && x.publicDate.HasValue);
                if (articleList.Count() == 0)
                {
                    this.Visible = false;
                    return;
                }
                DateTime dtStart = articleList.Min(x => x.publicDate.Value);
                DateTime dtEnd = articleList.Max(x => x.publicDate.Value);

                DateTime dtTmp = new DateTime(dtStart.Year, 1, 1);
                //string ltrYearString = "";
                //while (dtTmp <= new DateTime(dtEnd.Year, 1, 1))
                //{
                //    if (articleList.FirstOrDefault(x => x.publicDate.Value.Year == dtTmp.Year)!=null)
                //        ltrYearString += "<li><a href=\"" + contUtils.getStp_pagePath(11, App.LangID) + "?y=" + dtTmp.Year + "\">" + dtTmp.Year + "</a></li>";
                //    dtTmp = dtTmp.AddYears(1);
                //}
                //if (ltrYearString != "")
                //    ltrYearList.Text = "<ul class=\"listanno\">" + ltrYearString + "</ul>";

                //dtTmp = new DateTime(dtStart.Year, dtStart.Month, 1);
                //string ltrMonthString = "";
                //while (dtTmp <= new DateTime(dtEnd.Year, dtEnd.Month, 1))
                //{
                //    if (articleList.FirstOrDefault(x => x.publicDate.Value.Year == dtTmp.Year && x.publicDate.Value.Month == dtTmp.Month) != null)
                //        ltrMonthString += "<li><a href=\"" + contUtils.getStp_pagePath(11, App.LangID) + "?y=" + dtTmp.Year + "&amp;m=" + dtTmp.Month + "\">" + dtTmp.formatCustom("#MM# #yy#", App.LangID, "") + "</a></li>";
                //    dtTmp = dtTmp.AddMonths(1);
                //}
                //if (ltrMonthString != "")
                //    ltrMonthList.Text = "<ul class=\"listmese\">" + ltrMonthString + "</ul>";

                //if (ltrMonthString != "" && ltrYearString != "")
                //    ltrYearList.Text += "<div class=\"nulla\"></div>";
            }
        }
    }
}