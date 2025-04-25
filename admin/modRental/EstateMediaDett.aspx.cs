using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using System.IO;


namespace ModRental.admin.modRental
{
    public partial class EstateMediaDett : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                var currTBL = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
                if (currTBL == null)
                {
                    pnlDett.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
                    return;
                }
                Bind_Tag();
                // todo: se non c'e la foto
                //if (currTBL.img_banner == "" || !File.Exists(Path.Combine(App.SRP, currTBL.img_banner)))
                //{
                //    pnlDett.Visible = false;
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
                //    return;
                //}
                HF_id.Value = Request.QueryString["id"];
                HF_imgPath.Value = currTBL.img_banner;
                drp_tag.setSelectedValue(currTBL.code);
                txt_video_embed.Text = currTBL.video_embed;
                phVideo.Visible = currTBL.type == "video";
            }
        }

        protected void lnk_saveGalleryItem_Click(object sender, EventArgs e)
        {
            var currTBL = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (currTBL == null)
            {
                pnlDett.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
                return;
            }
            currTBL.code = drp_tag.SelectedValue;
            currTBL.video_embed = txt_video_embed.Text;
            DC_RENTAL.SubmitChanges();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
        }

        protected void lnk_deleteGalleryItem_Click(object sender, EventArgs e)
        {
            var currTBL = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (currTBL == null)
            {
                pnlDett.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
                return;
            }
            DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteOnSubmit(currTBL);
            DC_RENTAL.SubmitChanges();
            try
            {
                if (File.Exists(Path.Combine(App.SRP, currTBL.img_banner))) File.Delete(Path.Combine(App.SRP, currTBL.img_banner));
                if (File.Exists(Path.Combine(App.SRP, currTBL.img_thumb))) File.Delete(Path.Combine(App.SRP, currTBL.img_thumb));
            }
            catch (Exception ex) { }

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "parent.closeDett", "parent.closeDett();", true);
        }


        protected void Bind_Tag()
        {
            using (DCmodRental DC = new DCmodRental())
            {
                drp_tag.DataSource = DC.dbRntEstateMediaTAGs.OrderBy(x => x.title).ToList();
                drp_tag.DataTextField = "title";
                drp_tag.DataValueField = "title";
                drp_tag.DataBind();
                drp_tag.Items.Insert(0, new ListItem("------", ""));
            }
        }
    }
}