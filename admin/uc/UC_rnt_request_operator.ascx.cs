using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_request_operator : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_REQUEST _currTBL;
        public int IdRequest
        {
            get
            {
                int _id;
                if (int.TryParse(HF_pid_request.Value, out _id))
                    return _id;
                return 0;
            }
            set
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                HF_pid_request.Value = value.ToString();
                FillControls();
            }
        }
        public event EventHandler onChange;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                Bind_drp_admin();
                Bind_drp_mailing_days();
            }
        }
        protected void Bind_drp_mailing_days()
        {
            drp_mailing_days.Items.Clear();
            List<USR_LK_MAIL_COUNT> _list = maga_DataContext.DC_USER.USR_LK_MAIL_COUNTs.OrderBy(x => x.mail_count).ToList();
            foreach (USR_LK_MAIL_COUNT _d in _list)
            {
                drp_mailing_days.Items.Add(new ListItem(_d.title, "" + _d.mail_count));
            }
            drp_mailing_days.Items.Add(new ListItem("mese corrente", "-1"));
            drp_mailing_days.Items.Add(new ListItem("- tutti -", "0"));
            drp_mailing_days.setSelectedValue("-1");
        }
        protected void Bind_drp_admin()
        {
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.rnt_canHaveRequest == 1 && x.is_deleted == 0 && x.is_active == 1).ToList();
            drp_admin.Items.Clear();
            foreach (USR_ADMIN _utenti in _list)
            {
                if (_utenti.id == HF_pid_operator.Value.ToInt32() || (chk_only_availables.Checked && !AdminUtilities.usr_adminIsAvailable(_utenti.id)))
                    continue;
                int _mailCount = AdminUtilities.usr_adminMailCount(_utenti.id, drp_mailing_days.getSelectedValueInt(0).Value);
                drp_admin.Items.Add(new ListItem("" + _utenti.name + " " + _utenti.surname + "  (" + _mailCount + " mail)", "" + _utenti.id));
            }
            drp_admin.Items.Insert(0, new ListItem("-! seleziona !-", "0"));
        }

        private void FillControls()
        {
            _currTBL = null;
            if (IdRequest != 0)
            {
                _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == IdRequest);
            }
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('error!');", true);
                return;
            }
            if (_currTBL.operator_date.HasValue && _currTBL.pid_operator.HasValue && _currTBL.pid_operator != 0)
            {
                HF_pid_operator.Value = _currTBL.pid_operator.ToString();
                ltr_date.Text = _currTBL.operator_date.Value.ToLongDateString();
                ltr_time.Text = _currTBL.operator_date.Value.ToShortTimeString();
                ltr_operator.Text = AdminUtilities.usr_adminName(_currTBL.pid_operator.Value, "");
                lbl_operatorState.InnerText = "Riassegna da " + ltr_operator.Text + " ad un'altro Account";
                lbl_noedit.InnerHtml = "Assegnato a " + ltr_operator.Text + " <br/>possibile riassegnare solo dalla Richiesta Primaria";
                DisableControls();
                ph_sendOld.Visible = true;
            }
            else
            {
                ph_sendOld.Visible = false;
                lbl_operatorState.InnerText = "Non Assegnato a nessun Account!";
                lbl_noedit.InnerHtml = "Non Assegnato a nessun Account!<br/>possibile assegnare solo dalla Richiesta Primaria";
                EnableControls();
            }
            if(_currTBL.pid_related_request!=0)
            {
                PH_noedit.Visible = true;
                PH_view.Visible = false;
                PH_modify.Visible = false;
                lnk_salva.Visible = false;
                lnk_annulla.Visible = false;
                lnk_modify.Visible = false;

            }
        }
        private void FillDataFromControls()
        {
            _currTBL = null;
            if (IdRequest != 0)
            {
                _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == IdRequest);
            }
            if (_currTBL == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('error!');", true);
                return;
            }
            if(_currTBL.pid_operator==drp_admin.getSelectedValueInt(0))
                return;
            rntUtils.rntRequest_updateOperator(IdRequest, drp_admin.getSelectedValueInt(0).objToInt32(), chk_sendMail.Checked, chk_sendOld.Checked, UserAuthentication.CurrentUserID);
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (onChange != null) { onChange(this, new EventArgs()); }
            FillControls();
        }
        protected void DisableControls()
        {
            PH_view.Visible = true;
            PH_modify.Visible = false;
            PH_noedit.Visible = false;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
        }
        protected void EnableControls()
        {
            PH_view.Visible = false;
            PH_modify.Visible = true;
            PH_noedit.Visible = false;
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            if (drp_admin.getSelectedValueInt(0)==0)
            {
                lbl_error.Visible = true;
                return;
            }
            lbl_error.Visible = false;
            FillDataFromControls();
            DisableControls();
        }

        protected void lnk_modify_Click(object sender, EventArgs e)
        {
            EnableControls();
            Bind_drp_admin();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }

        protected void chk_only_availables_CheckedChanged(object sender, EventArgs e)
        {
            Bind_drp_admin();
        }
        protected void drp_mailing_days_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_drp_admin();
        }
    }
}
