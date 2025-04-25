using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_usr_admin_password : System.Web.UI.UserControl
    {
        protected magaUser_DataContext DC_USER;
        protected USR_ADMIN _currTBL;
        public int IdAdmin
        {
            get
            {
                int _id;
                if (int.TryParse(HF_pid_admin.Value, out _id))
                    return _id;
                return 0;
            }
            set
            {
                DC_USER = maga_DataContext.DC_USER;
                HF_pid_admin.Value = value.ToString();
                FillControls();
            }
        }
        public event EventHandler onChange;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
            }
        }
        private void FillControls()
        {
            _currTBL = null;
            if (IdAdmin != 0)
            {
                _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            }
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('error!');", true);
                return;
            }
            if (String.IsNullOrEmpty(_currTBL.password))
            {
                _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
                DC_USER.SubmitChanges();
            }
            HF_pwd.Value = _currTBL.password;
            HF_login.Value = _currTBL.login;
            txt_login.Text = _currTBL.login;
            DisableControls();
        }
        private void FillDataFromControls()
        {
            _currTBL = null;
            if (IdAdmin != 0)
            {
                _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == IdAdmin);
            }
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('error!');", true);
                return;
            }
            _currTBL.password = txt_pwd_new.Text.Trim();
            _currTBL.login = txt_login.Text.Trim();
            DC_USER.SubmitChanges();
            if (onChange != null) { onChange(this, new EventArgs()); }
            FillControls();
        }
        protected void DisableControls()
        {
            lbl_title.InnerText = "Gestione Password del Utente";
            PH_view.Visible = true;
            PH_modify.Visible = false;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
            lnk_send_pwd.Visible = true;
        }
        protected void EnableControls()
        {
            lbl_title.InnerText = "Cambio Password del Utente";
            PH_view.Visible = false;
            PH_modify.Visible = true;
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
            lnk_send_pwd.Visible = false;
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            if (txt_login.Text.Trim() == "")
            {
                lbl_error.Visible = true;
                lbl_error.InnerText = "!Attenzione! Inserire Login di Accesso.";
                return;
            }
            if (txt_pwd_new.Text.Trim() == "")
            {
                lbl_error.Visible = true;
                lbl_error.InnerText = "!Attenzione! Inserire Nuova Password.";
                return;
            }
            if (txt_pwd_new.Text.Trim() != txt_pwd_confirm.Text.Trim())
            {
                lbl_error.Visible = true;
                lbl_error.InnerText = "!Attenzione! Le password non coincidono.";
                return;
            }
            if (txt_pwd_old.Text.Trim() != HF_pwd.Value)
            {
                lbl_error.Visible = true;
                lbl_error.InnerText = "!Attenzione! La password fornita non è corretta.";
                return;
            }
            lbl_error.Visible = false;
            FillDataFromControls();
        }

        protected void lnk_modify_Click(object sender, EventArgs e)
        {
            EnableControls();
        }
        protected void lnk_send_pwd_Click(object sender, EventArgs e)
        {
            string _modificaSMTP = "<a href='" + CurrentAppSettings.HOST + "/admin/' target='_blank'>" + CurrentAppSettings.HOST + "/admin/</a>";
            _modificaSMTP += "<br/>Per modificare la password dell'e-mail:";
            _modificaSMTP += "<br/>nel menù:";
            _modificaSMTP += "<br/>Anagrafica => Gestione Account";
            _modificaSMTP += "<br/>nella pagina:";
            _modificaSMTP += "<br/>cliccare su \"Configurazione Mailing\"";
            _modificaSMTP += "<br/><br/>cliccare su \"Modifica\" nella sezione Configurazione SMTP in fondo alla pagina";
            _modificaSMTP += "<br/>inserire la password di accesso della propria e-mail ";
            _modificaSMTP += "<br/>";
            bool alternateOld = true;
            string _mailBody = "";
            _mailBody += MailingUtilities.addMailRow("Messaggio di notifica password", "<br/>di seguito credenziali di accesso in area amministrativa del sito RentalInRome.com", alternateOld, out alternateOld, true, false, true);
            _mailBody += MailingUtilities.addMailRow("Login di Accesso", "" + HF_login.Value, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Password", "" + HF_pwd.Value, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("", _modificaSMTP, alternateOld, out alternateOld, false, false, true);
            _mailBody += MailingUtilities.addMailRow("", "", alternateOld, out alternateOld, false, true, true);
            if (MailingUtilities.autoSendMailTo("RiR | Notifica Password", _mailBody, AdminUtilities.usr_adminEmail(IdAdmin, ""), false, "admin_uc_UC_usr_admin_password"))
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert(\"le credenziali di accesso sono state inviate alla mail del'Account!\");", true);
            else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert(\"si è verificato un errore nell'invio!\");", true);
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
    }
}
