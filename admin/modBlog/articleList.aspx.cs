using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModBlog.admin.modBlog
{
    public partial class articleList : adminBasePage
    {
        protected dbBlogArticleTB currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                drp_flt_pidCategory_DataBind();
                drp_flt_pidLang_DataBind();
                setFilters();
                closeDetails(false);
                if (Request.QueryString["id"].objToInt64() > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setUrl", "$(document).ready(function () {$(window).load(function () {setUrl('dett', '" + Request.QueryString["id"].objToInt64() + "');});});", true);
                }
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            bool useCode = true;
            string orderAsc = useCode ? "&#9650;" : "▲";
            string orderDesc = useCode ? "&#9660;" : "▼";
            LinkButton lnk;
            List<string> orderByList = new List<string>() { "title", "publicDate" };
            string orderByCurrent = HF_LDS_orderBy.Value;
            foreach (string orderBy in orderByList)
            {
                lnk = LV.FindControl("lnk_orderBy_" + orderBy) as LinkButton;
                if (lnk == null) continue;
                lnk.Text = lnk.Text.Replace(orderAsc, "").Replace(orderDesc, "").Trim();
                if (orderByCurrent.StartsWith(orderBy))
                    lnk.Text = lnk.Text + (orderByCurrent.EndsWith("desc") ? " " + orderDesc : " " + orderAsc);
            }
        }
        protected void LV_OrderBy(string orderBy)
        {
            string orderByCurrent = HF_LDS_orderBy.Value;
            if (orderByCurrent.StartsWith(orderBy))
                HF_LDS_orderBy.Value = orderBy + (orderByCurrent.EndsWith("desc") ? " asc" : " desc");
            else
                HF_LDS_orderBy.Value = orderBy + " desc";
            closeDetails(false);
        }
        protected void drp_flt_pidCategory_DataBind()
        {
            drp_flt_pidCategory.Items.Clear();
            List<dbBlogCategoryVIEW> tmpList = blogProps.CategoryVIEW.Where(x => x.isActive == 1 && x.pidLang == 1).OrderBy(x => x.title).ToList();
            foreach (dbBlogCategoryVIEW tmp in tmpList)
            {
                drp_flt_pidCategory.Items.Add(new ListItem(tmp.title, tmp.id.ToString()));
            }
            drp_flt_pidCategory.Items.Insert(0, new ListItem("- tutti -", "-1"));
        }
        protected void drp_flt_pidLang_DataBind()
        {
            drp_flt_pidLang.DataSource = contProps.LangTBL.Where(x => x.is_active == 1).OrderBy(x => x.id);
            drp_flt_pidLang.DataTextField = "title";
            drp_flt_pidLang.DataValueField= "id";
            drp_flt_pidLang.DataBind();
            drp_flt_pidLang.Items.Add(new ListItem("- tutti -", ""));
            drp_flt_pidLang.setSelectedValue("");
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "orderBy")
            {
                LV_OrderBy(e.CommandArgument.ToString());
                return;
            }
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodBlog dc = new DCmodBlog())
                {
                    currTBL = dc.dbBlogArticleTBs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(dc.dbBlogArticleLNs.Where(x => x.pidArticle == currTBL.id));
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                        blogProps.ArticleVIEW = dc.dbBlogArticleVIEWs.ToList();
                    }
                }
                closeDetails(false);
            }
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            closeDetails(false);
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            LDS.Where = ltrLDSfiltter.Text;
            LDS.OrderBy = HF_LDS_orderBy.Value;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        protected void setFilters()
        {
            string _filter = "";
            string _sep = "";
            _filter += _sep + "pidLang = 1";
            _sep = " and ";
            if (drp_flt_pidCategory.SelectedValue.ToInt32() > 0)
            {
                _filter += _sep + "pidCategory = " + drp_flt_pidCategory.SelectedValue + "";
                _sep = " and ";
            }
            //if (drp_flt_pidLang.SelectedValue != "")
            //{
            //    _filter += _sep + "pidLang = " + drp_flt_pidLang.SelectedValue + "";
            //    _sep = " and ";
            //}
            if (drp_flt_isActive.SelectedValue != "-1")
            {
                _filter += _sep + "isActive = " + drp_flt_isActive.SelectedValue + "";
                _sep = " and ";
            }
            _filter += _sep + "title != String.Empty";
            _sep = " and ";
            if (txt_flt_title.Text.Trim() != "")
            {
                _filter += _sep + "title.Contains(\"" + txt_flt_title.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (rdp_flt_publicDateFrom.SelectedDate.HasValue)
            {
                _filter += _sep + "publicDate >= DateTime.Parse(\"" + rdp_flt_publicDateFrom.SelectedDate.Value + "\")";
                _sep = " and ";
            }
            if (rdp_flt_publicDateTo.SelectedDate.HasValue)
            {
                _filter += _sep + "publicDate < DateTime.Parse(\"" + rdp_flt_publicDateTo.SelectedDate.Value + "\")";
                _sep = " and ";
            }
            _filter += _sep + "1 = 1";
            ltrLDSfiltter.Text = _filter;
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            setFilters();
            closeDetails(false);
        }
    }

}