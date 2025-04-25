using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_operator_config : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "";
        }
        private magaUser_DataContext DC_USER;
        protected string listPage = "usr_operator_details.aspx";
        private USR_ADMIN_CONFIG _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                USR_ADMIN _usr = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32() && x.is_deleted !=1);
                if (_usr == null)
                    Response.Redirect(listPage + "?id=" + Request.QueryString["id"]);

                if (!UserAuthentication.hasPermission("usr_mail_config") && _usr.id != UserAuthentication.CurrentUserID)
                {
                    Response.Redirect("/admin/");
                    return;
                }
                HF_id.Value = _usr.id.ToString();
                FillControls();
            }
        }
        private void FillControls()
        {
            USR_ADMIN _usr = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == HF_id.Value.ToInt32() && x.is_deleted != 1);
            if (_usr == null)
                Response.Redirect(listPage + "?id=" + HF_id.Value);
            txt_email.Text = _usr.email;
            _currTBL = DC_USER.USR_ADMIN_CONFIGs.SingleOrDefault(s => s.id == _usr.pid_config);
            if (_currTBL == null)
            { 
                EnableControls();
                DisableControlsSMTP();
                return;
            }

            txt_mailing_header.Text = _currTBL.mailing_header;
            txt_mailing_signature.Text = _currTBL.mailing_signature;
            viewPwd(_currTBL.mailing_imap_pwd);
            DisableControls();
            DisableControlsSMTP();
        }
        protected void setDefaults(ref USR_ADMIN_CONFIG _currTBL)
        { 
            _currTBL.mailing_smtp_host = "smtp.gmail.com";
            _currTBL.mailing_smtp_port = 587;
            _currTBL.mailing_smtp_pwd = "";
            _currTBL.mailing_smtp_ssl = true;
            _currTBL.mailing_imap_host = "imap.gmail.com";
            _currTBL.mailing_imap_port = 993;
            _currTBL.mailing_imap_pwd = "";
            _currTBL.mailing_imap_ssl = true;
        }
        private void FillDataFromControls()
        {
            USR_ADMIN _usr = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == HF_id.Value.ToInt32() && x.is_deleted != 1);
            if (_usr == null)
                Response.Redirect(listPage + "?id=" + HF_id.Value);
            _currTBL = DC_USER.USR_ADMIN_CONFIGs.SingleOrDefault(s => s.id == _usr.pid_config);
            if (_currTBL == null)
            {
                _currTBL = new USR_ADMIN_CONFIG();
                setDefaults(ref _currTBL);
                DC_USER.USR_ADMIN_CONFIGs.InsertOnSubmit(_currTBL);
                DC_USER.SubmitChanges();
                _usr.pid_config = _currTBL.id;
            }

            _currTBL.mailing_from_mail = _usr.name + " " + _usr.surname;
            _currTBL.mailing_from_mail = _usr.email;
            _currTBL.mailing_smtp_usr = _usr.email;
            _currTBL.mailing_imap_usr = _usr.email;
            _currTBL.code = _usr.email;
            _currTBL.mailing_header = txt_mailing_header.Text;
            _currTBL.mailing_signature = txt_mailing_signature.Text;
            DC_USER.SubmitChanges();
            viewPwd(_currTBL.mailing_imap_pwd);
            if (UserAuthentication.CURRENT_USR_ADMIN_CONFIG != null && UserAuthentication.CURRENT_USR_ADMIN_CONFIG.id == _currTBL.id)
                UserAuthentication.CURRENT_USR_ADMIN_CONFIG = _currTBL;

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
        protected void lnk_template_Click(object sender, EventArgs e)
        {
            txt_mailing_header.Text = ltr_header_template.Text;
            txt_mailing_signature.Text = ltr_footer_template.Text;
            EnableControls();
        }
        protected void DisableControls()
        {
            txt_mailing_header.ReadOnly = true;
            txt_mailing_signature.ReadOnly = true;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_template.Visible = false;
            lnk_modify.Visible = true;
        }
        protected void EnableControls()
        {
            txt_mailing_header.ReadOnly = false;
            txt_mailing_signature.ReadOnly = false;
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
            lnk_template.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(false);", true);
        }
        protected void viewPwd(string pwd) 
        {
            lblPwd.Text = "";
            for (int i = 0; i < pwd.Length; i++)
            {
                lblPwd.Text += "*";
            }
        }
        protected void lnk_salvaSMTP_Click(object sender, EventArgs e)
        {
            if (txt_pwd.Text != txt_pwdConf.Text)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    "alertError",
                                                    "alert('le password non coincidono');", true);
                txt_pwdConf.Text = "";
                txt_pwd.Text = "";
                return;
            }
            USR_ADMIN _usr = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == HF_id.Value.ToInt32() && x.is_deleted != 1);
            if (_usr == null)
                Response.Redirect(listPage + "?id=" + HF_id.Value);
            _currTBL = DC_USER.USR_ADMIN_CONFIGs.SingleOrDefault(s => s.id == _usr.pid_config);
            if (_currTBL == null)
            {
                _currTBL = new USR_ADMIN_CONFIG();
                setDefaults(ref _currTBL);
                DC_USER.USR_ADMIN_CONFIGs.InsertOnSubmit(_currTBL);
                DC_USER.SubmitChanges();
                _usr.pid_config = _currTBL.id;
            }
            _currTBL.mailing_from_mail = _usr.name + " " + _usr.surname;
            _currTBL.mailing_from_mail = _usr.email;
            _currTBL.mailing_smtp_usr = _usr.email;
            _currTBL.mailing_imap_usr = _usr.email;
            _currTBL.code = _usr.email;
            _currTBL.mailing_smtp_pwd = txt_pwd.Text;
            _currTBL.mailing_imap_pwd = txt_pwd.Text;
            DC_USER.SubmitChanges();
            if (UserAuthentication.CURRENT_USR_ADMIN_CONFIG != null && UserAuthentication.CURRENT_USR_ADMIN_CONFIG.id == _currTBL.id)
                UserAuthentication.CURRENT_USR_ADMIN_CONFIG = _currTBL;
            txt_pwdConf.Text = "";
            txt_pwd.Text = "";
            DisableControlsSMTP();
            viewPwd(_currTBL.mailing_imap_pwd);
        }
        protected void lnk_annullaSMTP_Click(object sender, EventArgs e)
        {
            txt_pwdConf.Text = "";
            txt_pwd.Text = "";
            DisableControlsSMTP();
        }
        protected void lnk_modifySMTP_Click(object sender, EventArgs e)
        {
            EnableControlsSMTP();
        }
        protected void DisableControlsSMTP()
        {
            pnl_smtpEdit.Visible = false;
            pnl_smtpView.Visible = true;
            lnk_salvaSMTP.Visible = false;
            lnk_annullaSMTP.Visible = false;
            lnk_modifySMTP.Visible = true;
        }
        protected void EnableControlsSMTP()
        {
            pnl_smtpEdit.Visible = true;
            pnl_smtpView.Visible = false;
            lnk_salvaSMTP.Visible = true;
            lnk_annullaSMTP.Visible = true;
            lnk_modifySMTP.Visible = false;
        }
    }
}
