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
    public partial class pdf_template : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected magaPdf_DataContext DC_PDF;
        protected PDF_TBL_TEMPLATE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_PDF = maga_DataContext.DC_PDF;
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
            _currTBL = DC_PDF.PDF_TBL_TEMPLATEs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new PDF_TBL_TEMPLATE();
            }
            txt_code.Text = _currTBL.code;
            txt_inner_notes.Text = _currTBL.inner_notes;
            txt_replace_notes.Text = _currTBL.replace_notes;
            txt_body.Text = _currTBL.body;
            txt_subject.Text = _currTBL.title;

            pnlContent.Visible = true;
            RegisterScripts();
            PDF_check();
        }
        protected void lnk_create_Click(object sender, EventArgs e)
        {
            createPDF();
            PDF_check();
        }
        protected void PDF_check()
        {
            HL_open_pdf.Visible = File.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "pdf/prove_template-" + HF_id.Value + ".pdf"));
        }
        public void createPDF()
        {
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            _currTBL = DC_PDF.PDF_TBL_TEMPLATEs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new PDF_TBL_TEMPLATE();
                DC_PDF.PDF_TBL_TEMPLATEs.InsertOnSubmit(_currTBL);
            }
            _currTBL.code = txt_code.Text;
            _currTBL.inner_notes = txt_inner_notes.Text;
            _currTBL.replace_notes = txt_replace_notes.Text;
            _currTBL.body = txt_body.Text;
            _currTBL.title = txt_subject.Text;
            DC_PDF.SubmitChanges();
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
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
    }
}
