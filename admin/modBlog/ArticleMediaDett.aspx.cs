using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


namespace ModBlog.admin.modBlog
{
    public partial class ArticleMediaDett : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (DCmodBlog dc = new DCmodBlog())
                {
                    var currTBL = dc.dbBlogArticleMediaTBLs.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt64());
                    if (currTBL == null)
                    {
                        pnlDett.Visible = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
                        return;
                    }
                    // todo: se non c'e la foto
                    //if (currTBL.img_banner == "" || !File.Exists(Path.Combine(App.SRP, currTBL.img_banner)))
                    //{
                    //    pnlDett.Visible = false;
                    //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
                    //    return;
                    //}
                    HF_id.Value = Request.QueryString["id"];
                    HF_imgPath.Value = currTBL.img_banner;
                    txt_code.Text = currTBL.code;
                }
            }
        }
        protected void lnk_saveGalleryItem_Click(object sender, EventArgs e)
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                var currTBL = dc.dbBlogArticleMediaTBLs.SingleOrDefault(x => x.id == HF_id.Value.objToInt64());
                if (currTBL == null)
                {
                    pnlDett.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
                    return;
                }
                currTBL.code = txt_code.Text;
                dc.SaveChanges();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
            }
        }

        protected void lnk_deleteGalleryItem_Click(object sender, EventArgs e)
        {
            using (DCmodBlog dc = new DCmodBlog())
            {
                var currTBL = dc.dbBlogArticleMediaTBLs.SingleOrDefault(x => x.id == HF_id.Value.objToInt64());
                if (currTBL == null)
                {
                    pnlDett.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
                    return;
                }
                dc.Delete(currTBL);
                dc.SaveChanges();
                try
                {
                    if (File.Exists(Path.Combine(App.SRP, currTBL.img_banner))) File.Delete(Path.Combine(App.SRP, currTBL.img_banner));
                    if (File.Exists(Path.Combine(App.SRP, currTBL.img_thumb))) File.Delete(Path.Combine(App.SRP, currTBL.img_thumb));
                }
                catch (Exception ex) { }
                int currSeq = currTBL.sequence.objToInt32();
                if (currSeq < 1) currSeq = 1;
                var currArticle = dc.dbBlogArticleTBs.SingleOrDefault(x => x.id == currTBL.pidArticle.objToInt64());
                if (currArticle != null && currSeq == 1)
                {
                    currTBL = dc.dbBlogArticleMediaTBLs.Where(x => x.pidArticle == currArticle.id).OrderBy(x => x.sequence).FirstOrDefault();
                    if (currTBL == null)
                    {
                        currArticle.imgBanner = "";
                        currArticle.imgPreview = "";
                    }
                    else
                    {
                        currArticle.imgBanner = currTBL.img_banner;
                        currArticle.imgPreview = currTBL.img_thumb;
                        currTBL.sequence = 1;
                    }
                    dc.SaveChanges();
                }

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
            }
        }
    }
}