using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModBlog.admin.modBlog
{
    public partial class categoryList : adminBasePage
    {
        protected dbBlogCategoryTB currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodBlog dc = new DCmodBlog())
                {
                    currTBL = dc.dbBlogCategoryTBs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(dc.dbBlogCategoryLNs.Where(x => x.pidCategory == currTBL.id));
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                        blogProps.CategoryVIEW = dc.dbBlogCategoryVIEWs.ToList();
                    }
                }
                closeDetails(false);
            }
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        protected void lnkNewTmp_Click(object sender, EventArgs e)
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                var tmpTb = new dbBlogCategoryTB();
                dc.Add(tmpTb);
                dc.SaveChanges();
                var tmpLn = new dbBlogCategoryLN();
                tmpLn.pidCategory = tmpTb.id;
                tmpLn.pidLang = 1;
                tmpLn.title = txt_newTmp.Text;
                dc.Add(tmpLn);
                dc.SaveChanges();
                tmpLn = new dbBlogCategoryLN();
                tmpLn.pidCategory = tmpTb.id;
                tmpLn.pidLang = 2;
                tmpLn.title = txt_newTmp.Text;
                dc.Add(tmpLn);
                dc.SaveChanges();
                txt_newTmp.Text = "";
                LDS.DataBind();
                LV.SelectedIndex = -1;
                LV.DataBind();
            }
        }
    }

}