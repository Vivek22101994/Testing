using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModBlog.admin.modBlog
{
    public partial class commentDett : adminBasePage
    {
        protected dbBlogCommentTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drp_pidArticle_DataBind();
                HfId.Value = Request.QueryString["id"].ToInt64().ToString();
                fillData();
            }
        }
        protected void drp_pidArticle_DataBind()
        {
            drp_pidArticle.Items.Clear();
            drp_pidArticle.Items.Add(new ListItem("-non abbinato-", "0"));
            drp_pidArticle_fillArticles(0, "");
        }
        protected void drp_pidArticle_fillArticles(long pidParentArticle, string addString)
        {
            List<dbBlogArticleVIEW> tmpList = blogProps.ArticleVIEW.Where(x => x.pidLang == 1 && x.pidParentArticle == pidParentArticle).OrderBy(x => x.title).ToList();
            foreach (dbBlogArticleVIEW tmp in tmpList)
            {
                drp_pidArticle.Items.Add(new ListItem(addString + " " + tmp.title + " #" + tmp.id + " " + (tmp.isActive == 1 ? "OK" : "N/A"), tmp.id.ToString()));
                drp_pidArticle_fillArticles(tmp.id, addString + "-->");
            }
        }
        private void fillData()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                currTBL = dc.dbBlogCommentTBLs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbBlogCommentTBL();
                    currTBL.commentDate = DateTime.Now;
                    ltrTitle.Text = "Nuovo commento";
                }
                else
                {
                    ltrTitle.Text = "Commento #:" + currTBL.id;
                }
                drp_isActive.setSelectedValue(currTBL.isActive);
                drp_typeCode.setSelectedValue(currTBL.typeCode);
                txt_nameFull.Text = currTBL.nameFull;
                txt_email.Text = currTBL.email;
                drp_pidArticle.setSelectedValue(currTBL.pidArticle.ToString());
                rdtp_commentDate.SelectedDate = currTBL.commentDate;
                txt_commentSubject.Text = currTBL.commentSubject;
                re_commentBody.Content = currTBL.commentBody;
            }
        }
        private void saveData()
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                currTBL = dc.dbBlogCommentTBLs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbBlogCommentTBL();
                    currTBL.uid = Guid.NewGuid();
                    currTBL.createdDate = DateTime.Now;
                    currTBL.createdUserID = authUtils.CurrentUserID;
                    currTBL.createdUserNameFull = authUtils.CurrentUserName;
                    currTBL.browserIP = Request.browserIP();
                    dc.Add(currTBL);
                    dc.SaveChanges();
                }
                currTBL.isActive = drp_isActive.getSelectedValueInt();
                currTBL.typeCode = drp_typeCode.SelectedValue;
                currTBL.nameFull = txt_nameFull.Text;
                currTBL.email = txt_email.Text;
                currTBL.pidArticle = drp_pidArticle.SelectedValue.ToInt64();
                currTBL.commentDate = rdtp_commentDate.SelectedDate;
                currTBL.commentSubject = txt_commentSubject.Text;
                currTBL.commentBody = re_commentBody.Content;
                if (currTBL.commentSubject == "")
                    currTBL.commentSubject = currTBL.commentBody.cutString(50).Replace("<br>", " ").Replace("<br/>", " ").Replace("<br />", " ");
                dc.SaveChanges();
                HfId.Value = currTBL.id + "";
                blogProps.CommentTBL = dc.dbBlogCommentTBLs.ToList();

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            fillData();
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "0";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }
}

