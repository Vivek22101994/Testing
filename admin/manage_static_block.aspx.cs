using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class manage_static_block : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected magaContent_DataContext DC_CONTENT;

        protected List<CONT_RL_BLOCK_LANG> CURRENT_STATIC_BLOCK_LANG_;
        protected List<CONT_RL_BLOCK_LANG> CURRENT_STATIC_BLOCK_LANG
        {
            get {
                if (CURRENT_STATIC_BLOCK_LANG_ == null)
                    if (ViewState["CURRENT_BLOCK_LANG"] != null) {
                        CURRENT_STATIC_BLOCK_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_BLOCK_LANG"],
                                                 typeof(CONT_RL_BLOCK_LANG)).Cast<CONT_RL_BLOCK_LANG>().ToList();
                    } else
                        CURRENT_STATIC_BLOCK_LANG_ = new List<CONT_RL_BLOCK_LANG>();

                return CURRENT_STATIC_BLOCK_LANG_;
            }
            set {
                ViewState["CURRENT_BLOCK_LANG"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_STATIC_BLOCK_LANG_ = value;
            }
        }

        protected CONT_TBL_BLOCK currentBlock;

        protected void Page_Load(object sender, EventArgs e) {
                DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack) {
            }
        }
 
        protected void LV_blocks_SelectedIndexChanging(object sender, ListViewSelectEventArgs e) {
            var lbl_id = (Label)LV_blocks.Items[e.NewSelectedIndex].FindControl("lbl_id");
            HF_id.Value = lbl_id.Text;
            FillControls();
        }

        private void FillControls() {
            currentBlock = new CONT_TBL_BLOCK();
            if (HF_id.Value != "0") {
                int id;
                if (int.TryParse(HF_id.Value, out id)) {
                    currentBlock = DC_CONTENT.CONT_TBL_BLOCKs.SingleOrDefault(item => item.id == id);
                }
            }

            CURRENT_STATIC_BLOCK_LANG = DC_CONTENT.CONT_RL_BLOCK_LANGs.Where(x => x.pid_block == currentBlock.id).ToList();

            HF_lang.Value = "1";
            Fill_lang();
            LV_langs.DataBind();
            
            txt_name.Text = currentBlock.block_name;
            img_thumb.ImageUrl = CurrentAppSettings.ROOT_PATH + currentBlock.img_thumb;
            HF_img_thumb.Value = currentBlock.img_thumb;

            pnlContent.Visible = true;
            RegisterScripts();
            Fill_lang();
        }

        protected void lnk_salva_Click(object sender, EventArgs e) {
            currentBlock = new CONT_TBL_BLOCK();
            if (HF_id.Value != "0") {
                int id;
                if (int.TryParse(HF_id.Value, out id)) {
                    currentBlock = DC_CONTENT.CONT_TBL_BLOCKs.SingleOrDefault(item => item.id == id);
                }
            }
            else {
                DC_CONTENT.CONT_TBL_BLOCKs.InsertOnSubmit(currentBlock);
            }
            currentBlock.block_name = txt_name.Text;
            currentBlock.img_thumb = HF_img_thumb.Value;
            DC_CONTENT.SubmitChanges();
            LV_blocks.SelectedIndex = -1;
            LV_blocks.DataBind();
            Save_RL_langs();
            pnlContent.Visible = false;
        }

        protected void DeleteRecord(object sender, EventArgs args) {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var block = DC_CONTENT.CONT_TBL_BLOCKs.SingleOrDefault(item => item.id == id);
            if (block != null) {
                var rlLang =
                        DC_CONTENT.CONT_RL_BLOCK_LANGs.Where(
                            item => item.pid_block == block.id);
                DC_CONTENT.CONT_RL_BLOCK_LANGs.DeleteAllOnSubmit(rlLang);
                DC_CONTENT.CONT_TBL_BLOCKs.DeleteOnSubmit(block);
                DC_CONTENT.SubmitChanges();
                LV_blocks.DataBind();
            }
            if (pnlContent.Visible)
                RegisterScripts();
        }

        protected void lnk_nuovo_Click(object sender, EventArgs e) {
            LV_blocks.SelectedIndex = -1;
            LV_blocks.DataBind();
            HF_id.Value = "0";
            FillControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e) {
            LV_blocks.SelectedIndex = -1;
            LV_blocks.DataBind();
            pnlContent.Visible = false;
        }

        protected void lnk_carica_thumb_Click(object sender, EventArgs e) {
            if (FU_thumb.HasFile) {
                HF_img_thumb.Value = "images/static-blocks/" + HF_id.Value + "_" + FU_thumb.FileName;
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "images/static-blocks"))) {
                    Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "images/static-blocks"));
                }
                FU_thumb.SaveAs(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_img_thumb.Value));
                img_thumb.ImageUrl = CurrentAppSettings.ROOT_PATH + HF_img_thumb.Value;
            }
            RegisterScripts();
        }

        private void RegisterScripts() {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor();", true);
        }

        protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e) {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
            lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
        }

        protected void LV_langs_ItemCommand(object sender, ListViewCommandEventArgs e) {
            if (e.CommandName == "change_lang") {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                Save_lang();
                HF_lang.Value = lbl_id.Text;
                Fill_lang();
                LV_langs.DataBind();
                RegisterScripts();
            }
        }

        protected void Save_lang() {
            var curr_rl_langs = CURRENT_STATIC_BLOCK_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null) {
                rlLang = new CONT_RL_BLOCK_LANG();
                rlLang.pid_block = int.Parse(HF_id.Value);
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_STATIC_BLOCK_LANG.Add(rlLang);
            }
            rlLang.description = txt_description.Text;
            rlLang.title = txt_title.Text;
            rlLang.sub_title = txt_sub_title.Text;
            rlLang.summary = txt_summary.Text;
            CURRENT_STATIC_BLOCK_LANG = curr_rl_langs;
        }

        protected void Fill_lang() {
            var rlLang = CURRENT_STATIC_BLOCK_LANG.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null) {
                rlLang = new CONT_RL_BLOCK_LANG();
            }
            txt_title.Text = rlLang.title;
            txt_description.Text = rlLang.description;
            txt_sub_title.Text = rlLang.sub_title;
            txt_summary.Text = rlLang.summary;
        }

        protected void Save_RL_langs() {
            Save_lang();
            var curr_rl_langs = DC_CONTENT.CONT_RL_BLOCK_LANGs.Where(x => x.pid_block == currentBlock.id).ToList();
            foreach (var rl in CURRENT_STATIC_BLOCK_LANG) {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang)) {
                    rl.pid_block = currentBlock.id;
                    DC_CONTENT.CONT_RL_BLOCK_LANGs.InsertOnSubmit(rl);
                }
                else {
                    var curr_rl = curr_rl_langs.Single(x => x.pid_lang == rl.pid_lang);
                    curr_rl.description = rl.description;
                    curr_rl.title = rl.title;
                    curr_rl.sub_title = rl.sub_title;
                    curr_rl.summary = rl.summary;
                }
                // --- genarate PagePath & Rewrite Tool
            }
            DC_CONTENT.SubmitChanges();
        }

        protected void lnk_delete_thumb_Click(object sender, EventArgs e) {
            HF_img_thumb.Value = "";
            img_thumb.ImageUrl = "";
        }

    }
}
