using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class manage_static_collection : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected magaContent_DataContext DC_CONTENT;
        protected CONT_TBL_COLLECTION currentCollection;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack)
            {
            }
            else if (!string.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
            {
                int id;
                if (int.TryParse(Request["__EVENTARGUMENT"], out id))
                {
                    var block = DC_CONTENT.CONT_TBL_BLOCKs.SingleOrDefault(item => item.id == id);
                    if (block != null)
                    {
                        CONT_RL_COLLECTION_BLOCK new_rl = new CONT_RL_COLLECTION_BLOCK();
                        new_rl.pid_block = id;
                        new_rl.pid_collection = int.Parse(HF_id.Value);
                        new_rl.sequence = Blocks.Count + 1;
                        Blocks.Add(new_rl);
                        BindBlocks();
                    }
                }

            }
        }
        protected void LV_collection_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            var lbl_id = (Label)LV_collection.Items[e.NewSelectedIndex].FindControl("lbl_id");
            HF_id.Value = lbl_id.Text;
            FillControls();
        }

        private void FillControls()
        {
            currentCollection = new CONT_TBL_COLLECTION();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    currentCollection = DC_CONTENT.CONT_TBL_COLLECTIONs.SingleOrDefault(item => item.id == id);
                }
            }

            img_thumb.ImageUrl = CurrentAppSettings.ROOT_PATH + currentCollection.img_thumb;
            HF_img_thumb.Value = currentCollection.img_thumb;
            img_pre.ImageUrl = CurrentAppSettings.ROOT_PATH + currentCollection.img_preview;
            HF_img_pre.Value = currentCollection.img_preview;
            txt_title.Text = currentCollection.title;
            txt_description.Text = currentCollection.description;

            pnlContent.Visible = true;
            RegisterScripts();

            var blocks = DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.Where(item => item.pid_collection == currentCollection.id).OrderBy(item => item.sequence).ToList();
            Blocks = blocks;
            BindBlocks();
        }

        public List<CONT_RL_COLLECTION_BLOCK> Blocks
        {
            get
            {
                if (Session["static_blocks"] == null)
                    Session["static_blocks"] = new List<CONT_RL_COLLECTION_BLOCK>();
                return Session["static_blocks"] as List<CONT_RL_COLLECTION_BLOCK>;
            }
            set
            {
                Session["static_blocks"] = value;
            }
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            currentCollection = new CONT_TBL_COLLECTION();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    currentCollection = DC_CONTENT.CONT_TBL_COLLECTIONs.SingleOrDefault(item => item.id == id);
                }
            }
            else
            {
                DC_CONTENT.CONT_TBL_COLLECTIONs.InsertOnSubmit(currentCollection);
            }
            currentCollection.img_thumb = HF_img_thumb.Value;
            currentCollection.img_preview = HF_img_pre.Value;
            currentCollection.title = txt_title.Text;
            currentCollection.description = txt_description.Text;
            DC_CONTENT.SubmitChanges();
            LV_collection.SelectedIndex = -1;
            LV_collection.DataBind();
            pnlContent.Visible = false;
            SaveBlocks(currentCollection.id);
        }

        private void SaveBlocks(int id)
        {
            var old_blocks =
                DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.Where(item => item.pid_collection == id).ToList();
            foreach (var block in old_blocks)
            {
                DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.DeleteOnSubmit(block);
            }
            DC_CONTENT.SubmitChanges();
            foreach (var block in Blocks)
            {
                CONT_RL_COLLECTION_BLOCK _b = new CONT_RL_COLLECTION_BLOCK();
                _b.pid_block = block.pid_block;
                _b.pid_collection = id;
                _b.sequence = block.sequence;
                DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.InsertOnSubmit(_b);
            }
            DC_CONTENT.SubmitChanges();
        }

        protected void DeleteRecord(object sender, EventArgs args)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var page = DC_CONTENT.CONT_TBL_COLLECTIONs.SingleOrDefault(item => item.id == id);
            if (page != null)
            {
                DC_CONTENT.CONT_TBL_COLLECTIONs.DeleteOnSubmit(page);
                DC_CONTENT.SubmitChanges();
                LV_collection.DataBind();
            }
            if (pnlContent.Visible)
                RegisterScripts();
        }

        protected void lnk_nuovo_Click(object sender, EventArgs e)
        {
            LV_collection.SelectedIndex = -1;
            LV_collection.DataBind();
            HF_id.Value = "0";
            FillControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LV_collection.SelectedIndex = -1;
            LV_collection.DataBind();
            pnlContent.Visible = false;
        }

        protected void lnk_carica_thumb_Click(object sender, EventArgs e)
        {
            if (FU_thumb.HasFile)
            {
                HF_img_thumb.Value = "images/static-collections/thumb/" + HF_id.Value + "_" + FU_thumb.FileName;
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "images/static-collections/thumb/")))
                {
                    Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "images/static-collections/thumb/"));
                }
                FU_thumb.SaveAs(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_img_thumb.Value));
                img_thumb.ImageUrl = CurrentAppSettings.ROOT_PATH + HF_img_thumb.Value;
            }
            RegisterScripts();
        }

        protected void lnk_carica_pre_Click(object sender, EventArgs e)
        {
            if (FU_img_pre.HasFile)
            {
                HF_img_pre.Value = "images/static-collections/preview/" + HF_id.Value + "_" + FU_img_pre.FileName;
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "images/static-collections/preview/")))
                {
                    Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "images/static-collections/preview/"));
                }
                FU_img_pre.SaveAs(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_img_pre.Value));
                img_pre.ImageUrl = CurrentAppSettings.ROOT_PATH + HF_img_pre.Value;
            }
            RegisterScripts();
        }

        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor();", true);
        }

        protected void ibtn_aggiungi_blocco_Click(object sender, ImageClickEventArgs e)
        {
            if (drp_blocks.Items.Count == 0)
                return;
            var new_block = new CONT_RL_COLLECTION_BLOCK();
            new_block.pid_collection = int.Parse(HF_id.Value);
            new_block.pid_block = int.Parse(drp_blocks.SelectedValue);
            new_block.sequence = Blocks.Count + 1;
            Blocks.Add(new_block);
            BindBlocks();
        }

        private void BindBlocks()
        {
            Blocks = Blocks.OrderBy(item => item.sequence).ToList();
            var blocks_ids = Blocks.Select(item => item.pid_block).ToList();
            DL_blocks.DataSource = Blocks;
            DL_blocks.DataBind();

            var blocks_available = DC_CONTENT.CONT_TBL_BLOCKs.Where(item => !blocks_ids.Contains(item.id)).ToList();
            drp_blocks.DataSource = blocks_available;
            drp_blocks.DataTextField = "block_name";
            drp_blocks.DataValueField = "id";
            drp_blocks.DataBind();
        }

        protected void UpBlock(object sender, EventArgs e)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var block = Blocks.SingleOrDefault(item => item.pid_block == id && item.pid_collection == int.Parse(HF_id.Value));
            var upper_block = Blocks.SingleOrDefault(item => item.sequence == block.sequence - 1 && item.pid_collection == int.Parse(HF_id.Value));
            if (upper_block != null)
            {
                block.sequence = upper_block.sequence;
                upper_block.sequence = upper_block.sequence + 1;
                BindBlocks();
            }
        }

        protected void DownBlock(object sender, EventArgs e)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var block = Blocks.SingleOrDefault(item => item.pid_block == id && item.pid_collection == int.Parse(HF_id.Value));
            var down_block = Blocks.SingleOrDefault(item => item.sequence == block.sequence + 1 && item.pid_collection == int.Parse(HF_id.Value));
            if (down_block != null)
            {
                block.sequence = down_block.sequence;
                down_block.sequence = down_block.sequence - 1;
                BindBlocks();
            }
        }

        protected void DL_blocks_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int id;
                if (int.TryParse(e.CommandArgument.ToString(), out id))
                {

                    var block = Blocks.SingleOrDefault(item => item.pid_block == id);
                    Blocks.Remove(block);
                    BindBlocks();
                }
            }
        }

        protected void DL_blocks_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
        }

        protected void lnk_delete_preview_Click(object sender, EventArgs e)
        {
            HF_img_pre.Value = "";
            img_pre.ImageUrl = "";
        }

        protected void lnk_delete_thumb_Click(object sender, EventArgs e)
        {
            HF_img_thumb.Value = "";
            img_thumb.ImageUrl = "";
        }

    }
}
