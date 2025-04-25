using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_cont_block_edit : System.Web.UI.UserControl
    {
        protected magaContent_DataContext DC_CONTENT;

        protected List<CONT_RL_BLOCK_LANG> CURRENT_STATIC_BLOCK_LANG_;
        protected List<CONT_RL_BLOCK_LANG> CURRENT_STATIC_BLOCK_LANG
        {
            get
            {
                if (CURRENT_STATIC_BLOCK_LANG_ == null)
                    if (ViewState["CURRENT_BLOCK_LANG"] != null)
                    {
                        CURRENT_STATIC_BLOCK_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_BLOCK_LANG"],
                                                 typeof(CONT_RL_BLOCK_LANG)).Cast<CONT_RL_BLOCK_LANG>().ToList();
                    }
                    else
                        CURRENT_STATIC_BLOCK_LANG_ = new List<CONT_RL_BLOCK_LANG>();

                return CURRENT_STATIC_BLOCK_LANG_;
            }
            set
            {
                ViewState["CURRENT_BLOCK_LANG"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_STATIC_BLOCK_LANG_ = value;
            }
        }
        public string BlockID
        {
            get
            {
                return HF_id.Value;
            }
            set
            {
                HF_id.Value = value;
                DC_CONTENT = maga_DataContext.DC_CONTENT;
                FillControls();
            }
        }
        public string CollectionID
        {
            get
            {
                return HF_id_collection.Value;
            }
            set
            {
                HF_id_collection.Value = value;
            }
        }
        public bool ShowDelay
        {
            get
            {
                return HF_show_delay.Value=="1";
            }
            set
            {
                HF_show_delay.Value = value ? "1" : "0";
            }
        }
        public bool ShowSubTitle
        {
            get
            {
                return HF_show_sub_title.Value == "1";
            }
            set
            {
                HF_show_sub_title.Value = value ? "1" : "0";
            }
        }
        public bool ShowImg
        {
            get
            {
                return HF_show_img.Value == "1";
            }
            set
            {
                HF_show_img.Value = value ? "1" : "0";
            }
        }
        public bool ShowSummary
        {
            get
            {
                return HF_show_summary.Value == "1";
            }
            set
            {
                HF_show_summary.Value = value ? "1" : "0";
            }
        }
        public bool ShowDesc
        {
            get
            {
                return HF_show_desc.Value == "1";
            }
            set
            {
                HF_show_desc.Value = value ? "1" : "0";
            }
        }
        public string EditorWidth
        {
            get
            {
                return HF_editor_width.Value;
            }
            set
            {
                HF_editor_width.Value = value;
                txt_description.Width = HF_editor_width.Value.ToInt32();
            }
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        protected CONT_TBL_BLOCK currentBlock;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack)
            {
                string _script = "var "+Unique +"_editors = ['"+txt_description.ClientID+"'];";
                _script += "function  " + Unique + "_removeTinyEditor() {";
                _script += "removeTinyEditors( " + Unique + "_editors);}";
                _script += "function  " + Unique + "_setTinyEditor(IsReadOnly) {";
                _script += "setTinyEditors( " + Unique + "_editors, IsReadOnly);}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Unique, _script, true);
            }
        }

        private void FillControls()
        {
            PH_show_delay.Visible = ShowDelay;
            PH_show_img.Visible = ShowImg;
            PH_show_sub_title.Visible = ShowSubTitle;
            PH_show_summary.Visible = ShowSummary;
            PH_show_desc.Visible = ShowDesc;
            txt_description.Width = HF_editor_width.Value.ToInt32();
            currentBlock = new CONT_TBL_BLOCK();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    currentBlock = DC_CONTENT.CONT_TBL_BLOCKs.SingleOrDefault(item => item.id == id);
                }
                DisableControls();
            }
            else
            {
                EnableControls();
            }

            CURRENT_STATIC_BLOCK_LANG = DC_CONTENT.CONT_RL_BLOCK_LANGs.Where(x => x.pid_block == currentBlock.id).ToList();

            HF_lang.Value = "1";
            Fill_lang();
            LV_langs.DataBind();

            txt_name.Text = currentBlock.block_name;
            img_thumb.ImageUrl = CurrentAppSettings.ROOT_PATH + currentBlock.img_thumb;
            HF_img_thumb.Value = currentBlock.img_thumb;
            txt_img.Text = currentBlock.img_thumb;
            chk_is_active.Checked = currentBlock.is_active == 1;
            drp_show_delay.setSelectedValue(currentBlock.show_delay.ToString());

            RegisterScripts();
            Fill_lang();
        }
        private void FillDataFromControls()
        {
            currentBlock = new CONT_TBL_BLOCK();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    currentBlock = DC_CONTENT.CONT_TBL_BLOCKs.SingleOrDefault(item => item.id == id);
                }
            }
            else
            {
                DC_CONTENT.CONT_TBL_BLOCKs.InsertOnSubmit(currentBlock);
                DC_CONTENT.SubmitChanges();
                if (CollectionID != "0")
                {
                    int _sequence = 1;
                    List<CONT_RL_COLLECTION_BLOCK> _allCollection =
                        DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.Where(x=>x.pid_collection==CollectionID.ToInt32()).OrderBy(x => x.sequence).ToList();
                    if(_allCollection.Count!=0)
                    {
                        _sequence = _allCollection.Max(x => x.sequence.Value)+1;
                    }
                    CONT_RL_COLLECTION_BLOCK _collection = new CONT_RL_COLLECTION_BLOCK();
                    _collection.sequence = _sequence;
                    _collection.pid_collection = CollectionID.ToInt32();
                    _collection.pid_block = currentBlock.id;
                    if (DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.SingleOrDefault(x=>x.pid_block==_collection.pid_block && x.pid_collection==_collection.pid_collection)==null)
                        DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.InsertOnSubmit(_collection);
                }
            }
            currentBlock.block_name = txt_name.Text;
            //currentBlock.img_thumb = HF_img_thumb.Value;
            currentBlock.img_thumb = txt_img.Text;

            currentBlock.is_active = chk_is_active.Checked ? 1 : 0;
            currentBlock.show_delay = drp_show_delay.getSelectedValueInt(0);

            DC_CONTENT.SubmitChanges();
            Save_RL_langs();
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            DisableControls();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls(); 
        }
        protected void lnk_modify_Click(object sender, EventArgs e)
        {
            EnableControls();
        }
        protected void DisableControls()
        {
            chk_is_active.Enabled = false;
            drp_show_delay.Enabled = false;
            txt_name.ReadOnly = true;
            txt_title.ReadOnly = true;
            txt_description.ReadOnly = true;
            txt_sub_title.ReadOnly = true;
            txt_summary.ReadOnly = true;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
            if (ShowDesc)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                        Unique + "_tinyEditor", "setTinyEditors( " + Unique + "_editors, true); ", true);
                lnk_modify.OnClientClick = Unique + "_removeTinyEditor()";
                lnk_salva.OnClientClick = Unique + "_removeTinyEditor()";
                lnk_annulla.OnClientClick = Unique + "_removeTinyEditor()";
            }
            else
            {
                lnk_modify.OnClientClick = "";
                lnk_salva.OnClientClick = "";
                lnk_annulla.OnClientClick = "";
            }
        }
        protected void EnableControls()
        {
            chk_is_active.Enabled = true;
            drp_show_delay.Enabled = true;
            txt_name.ReadOnly = false;
            txt_title.ReadOnly = false;
            txt_description.ReadOnly = false;
            txt_sub_title.ReadOnly = false;
            txt_summary.ReadOnly = false;
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
            if (ShowDesc)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                 Unique + "_tinyEditor",
                                                Unique + "_setTinyEditor(false);", true);
                lnk_modify.OnClientClick = Unique + "_removeTinyEditor()";
                lnk_salva.OnClientClick = Unique + "_removeTinyEditor()";
                lnk_annulla.OnClientClick = Unique + "_removeTinyEditor()";
            }
            else
            {
                lnk_modify.OnClientClick = "";
                lnk_salva.OnClientClick = "";
                lnk_annulla.OnClientClick = "";
            }
        }
        protected void DeleteRecord(object sender, EventArgs args)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var block = DC_CONTENT.CONT_TBL_BLOCKs.SingleOrDefault(item => item.id == id);
            if (block != null)
            {
                var rlLang =
                        DC_CONTENT.CONT_RL_BLOCK_LANGs.Where(
                            item => item.pid_block == block.id);
                DC_CONTENT.CONT_RL_BLOCK_LANGs.DeleteAllOnSubmit(rlLang);
                DC_CONTENT.CONT_TBL_BLOCKs.DeleteOnSubmit(block);
                DC_CONTENT.SubmitChanges();
            }
            RegisterScripts();
        }


        protected void lnk_carica_thumb_Click(object sender, EventArgs e)
        {
            if (FU_thumb.HasFile)
            {
                HF_img_thumb.Value = "images/static-blocks/" + HF_id.Value + "_" + FU_thumb.FileName;
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "images/static-blocks")))
                {
                    Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "images/static-blocks"));
                }
                FU_thumb.SaveAs(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_img_thumb.Value));
                img_thumb.ImageUrl = CurrentAppSettings.ROOT_PATH + HF_img_thumb.Value;
            }
            RegisterScripts();
        }

        private void RegisterScripts()
        {
        }

        protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
            lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
            lnk.OnClientClick = Unique + "_removeTinyEditor()";
        }

        protected void LV_langs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                Save_lang();
                HF_lang.Value = lbl_id.Text;
                Fill_lang();
                LV_langs.DataBind();
                DisableControls();
            }
        }

        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_STATIC_BLOCK_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
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

        protected void Fill_lang()
        {
            var rlLang = CURRENT_STATIC_BLOCK_LANG.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new CONT_RL_BLOCK_LANG();
            }
            txt_title.Text = rlLang.title;
            txt_description.Text = rlLang.description;
            txt_sub_title.Text = rlLang.sub_title;
            txt_summary.Text = rlLang.summary;
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_CONTENT.CONT_RL_BLOCK_LANGs.Where(x => x.pid_block == currentBlock.id).ToList();
            foreach (var rl in CURRENT_STATIC_BLOCK_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_block = currentBlock.id;
                    DC_CONTENT.CONT_RL_BLOCK_LANGs.InsertOnSubmit(rl);
                }
                else
                {
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

        protected void lnk_delete_thumb_Click(object sender, EventArgs e)
        {
            HF_img_thumb.Value = "";
            img_thumb.ImageUrl = "";
        }

    }
}
