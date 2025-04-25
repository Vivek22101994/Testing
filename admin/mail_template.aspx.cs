using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class mail_template : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "mail_template";
        }
        protected magaMail_DataContext DC_MAIL;

        protected List<MAIL_LN_TEMPLATE> _CURRENT_LANG;
        protected List<MAIL_LN_TEMPLATE> CURRENT_LANG
        {
            get
            {
                if (_CURRENT_LANG == null)
                    if (ViewState["CURRENT_BLOCK_LANG"] != null)
                    {
                        _CURRENT_LANG =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_BLOCK_LANG"],
                                                 typeof(MAIL_LN_TEMPLATE)).Cast<MAIL_LN_TEMPLATE>().ToList();
                    }
                    else
                        _CURRENT_LANG = new List<MAIL_LN_TEMPLATE>();

                return _CURRENT_LANG;
            }
            set
            {
                ViewState["CURRENT_BLOCK_LANG"] = PConv.SerialList(value.Cast<object>().ToList());
                _CURRENT_LANG = value;
            }
        }

        protected MAIL_TB_TEMPLATE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_MAIL = maga_DataContext.DC_MAIL;
            if (!IsPostBack)
            {
            }
        }

        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            var lbl_id = (Label)LV.Items[e.NewSelectedIndex].FindControl("lbl_id");
            HF_id.Value = lbl_id.Text;
            FillControls();
        }

        private void FillControls()
        {
            _currTBL = _currTBL = DC_MAIL.MAIL_TB_TEMPLATEs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL==null)
            {
               _currTBL= new MAIL_TB_TEMPLATE();
            }

            CURRENT_LANG = DC_MAIL.MAIL_LN_TEMPLATEs.Where(x => x.pid_template == _currTBL.id).ToList();

            HF_lang.Value = "1";
            Fill_lang();
            LV_langs.DataBind();

            txt_code.Text = _currTBL.code;
            txt_inner_notes.Text = _currTBL.inner_notes;
            txt_replace_notes.Text = _currTBL.replace_notes;

            pnlContent.Visible = true;
            RegisterScripts();
            Fill_lang();
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            _currTBL = _currTBL = DC_MAIL.MAIL_TB_TEMPLATEs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new MAIL_TB_TEMPLATE();
                DC_MAIL.MAIL_TB_TEMPLATEs.InsertOnSubmit(_currTBL);
            }
            _currTBL.code = txt_code.Text;
            _currTBL.inner_notes = txt_inner_notes.Text;
            _currTBL.replace_notes = txt_replace_notes.Text;
            DC_MAIL.SubmitChanges();
            LV.SelectedIndex = -1;
            LV.DataBind();
            Save_RL_langs();
            MailingUtilities.MAIL_VIEW_TEMPLATEs = DC_MAIL.MAIL_VIEW_TEMPLATEs.ToList();
            pnlContent.Visible = false;
        }

        protected void DeleteRecord(object sender, EventArgs args)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var block = DC_MAIL.MAIL_TB_TEMPLATEs.SingleOrDefault(item => item.id == id);
            if (block != null)
            {
                var rlLang =
                        DC_MAIL.MAIL_LN_TEMPLATEs.Where(
                            item => item.pid_template == block.id);
                DC_MAIL.MAIL_LN_TEMPLATEs.DeleteAllOnSubmit(rlLang);
                DC_MAIL.MAIL_TB_TEMPLATEs.DeleteOnSubmit(block);
                DC_MAIL.SubmitChanges();
                MailingUtilities.MAIL_VIEW_TEMPLATEs = DC_MAIL.MAIL_VIEW_TEMPLATEs.ToList();
                LV.DataBind();
            }
            if (pnlContent.Visible)
                RegisterScripts();
        }

        protected void lnk_nuovo_Click(object sender, EventArgs e)
        {
            LV.SelectedIndex = -1;
            LV.DataBind();
            HF_id.Value = "0";
            FillControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor();", true);
        }

        protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
            lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
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
                RegisterScripts();
            }
        }

        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new MAIL_LN_TEMPLATE();
                rlLang.pid_template = int.Parse(HF_id.Value);
                rlLang.pid_lang = int.Parse(HF_lang.Value);
                CURRENT_LANG.Add(rlLang);
            }
            rlLang.body = re_description.Content;
            rlLang.subject = txt_subject.Text;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang()
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == int.Parse(HF_lang.Value));
            if (rlLang == null)
            {
                rlLang = new MAIL_LN_TEMPLATE();
            }
            re_description.Content = rlLang.body;
            txt_subject.Text = rlLang.subject;
        }

        protected void Save_RL_langs()
        {
            Save_lang();
            var curr_rl_langs = DC_MAIL.MAIL_LN_TEMPLATEs.Where(x => x.pid_template == _currTBL.id).ToList();
            foreach (var rl in CURRENT_LANG)
            {
                if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                {
                    rl.pid_template = _currTBL.id;
                    DC_MAIL.MAIL_LN_TEMPLATEs.InsertOnSubmit(rl);
                }
                else
                {
                    var curr_rl = curr_rl_langs.Single(x => x.pid_lang == rl.pid_lang);
                    curr_rl.body= rl.body;
                    curr_rl.subject = rl.subject;
                }
                // --- genarate PagePath & Rewrite Tool
            }
            DC_MAIL.SubmitChanges();
        }
    }
}
