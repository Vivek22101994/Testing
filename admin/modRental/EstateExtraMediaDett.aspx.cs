using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using System.IO;
using ModRental;

namespace RentalInRome.admin.modRental
{
    public partial class EstateExtraMediaDett : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "extras";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var currTBL = dc.dbRntExtrasMediaRLs.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
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
            using (DCmodRental dc = new DCmodRental())
            {
                var currTBL = dc.dbRntExtrasMediaRLs.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
                if (currTBL == null)
                {
                    pnlDett.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
                    return;
                }
                currTBL.code = txt_code.Text;
                dc.SaveChanges();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
        }

        protected void lnk_deleteGalleryItem_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var currTBL = dc.dbRntExtrasMediaRLs.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
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
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
        }
    }
}