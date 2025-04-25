using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModBlog;

namespace RentalInRome
{
    public partial class pg_blogArticleDett : blogArticleBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HfId.Value = currArticle.id.ToString();
                HfpidLang.Value = App.LangID + "";
                ucBlogArticleList.loadLastWithTag(blogProps.ArticleTagRL.Where(x => x.pidArticle == currArticle.id).Select(x => x.pidTag).ToList(), currArticle.id, 5);
                using (DCmodBlog dc = new DCmodBlog())
                {
                    LV_gallery.DataSource = dc.dbBlogArticleMediaTBLs.Where(x => x.pidArticle == currArticle.id).OrderBy(x => x.sequence).ToList();
                    LV_gallery.DataBind();
                }
            }
        }

        protected void lnk_commentSend_Click(object sender, EventArgs e)
        {
            if (!RadCaptcha1.IsValid)
            {
                RadCaptchaCheck.Visible = true;
                return;
            }
            RadCaptchaCheck.Visible = false;
            using (DCmodBlog dc = new DCmodBlog())
            {
                dbBlogCommentTBL currTBL = new dbBlogCommentTBL();
                currTBL.uid = Guid.NewGuid();
                currTBL.createdDate = DateTime.Now;
                currTBL.createdUserID = 1;
                currTBL.createdUserNameFull = "System";
                currTBL.browserIP = Request.browserIP();
                currTBL.isActive = 0;
                currTBL.typeCode = rbtList_typeCode.SelectedValue;
                currTBL.nameFull = txt_nameFull.Text;
                currTBL.email = txt_email.Text;
                currTBL.pidArticle = currArticle.id;
                currTBL.commentDate = DateTime.Now;
                currTBL.commentBody = txt_commentBody.Text.htmlNoWrap();
                currTBL.commentSubject = currTBL.commentBody.cutString(50).Replace("<br>", " ").Replace("<br/>", " ").Replace("<br />", " ");
                dc.Add(currTBL);
                dc.SaveChanges();
                blogProps.CommentTBL = dc.dbBlogCommentTBLs.ToList();
                pnlCommentForm.Visible = false;
                pnlCommentSent.Visible = true;
            }
        }
    }
}