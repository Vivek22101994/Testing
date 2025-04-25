using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_admin_role : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected magaUser_DataContext DC_USER;

        protected USR_ADMIN _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                lnk_delete.Visible = UserAuthentication.CurrentUserID == 2;
                pnlSoloMaga.Visible = UserAuthentication.CurrentUserID == 2;
                Bind_drp_pid_role();
            }
        }
        protected void Bind_drp_pid_role()
        {
            drp_pid_role.DataSource = DC_USER.USR_TBL_ROLE.OrderBy(x => x.id);
            drp_pid_role.DataTextField = "title";
            drp_pid_role.DataValueField = "id";
            drp_pid_role.DataBind();
        }

        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            var lbl_id = (Label)LV.Items[e.NewSelectedIndex].FindControl("lbl_id");
            HF_id.Value = lbl_id.Text;
            FillControls();
        }

        private List<USR_RL_ROLE_PERMISSION> _currPermissions;
        private void FillControls()
        {
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new USR_ADMIN();
                _currTBL.password = CommonUtilities.CreatePassword(6, false, true, false);
            }
            txt_name.Text = _currTBL.name;
            txt_surname.Text = _currTBL.surname;
            txt_email.Text = _currTBL.email;
            drp_pid_role.setSelectedValue("" + _currTBL.pid_role);
            txt_login.Text = _currTBL.login;
            txt_password.Text = _currTBL.password;
            drp_is_active.setSelectedValue(_currTBL.is_active.ToString());
            drp_isAgentContact.setSelectedValue(_currTBL.isAgentContact);
            drp_rnt_canHaveReservation.setSelectedValue(_currTBL.rnt_canHaveReservation.ToString());
            drp_rnt_canHaveRequest.setSelectedValue(_currTBL.rnt_canHaveRequest.ToString());
            drp_rnt_canHaveAgent.setSelectedValue(_currTBL.rnt_canHaveAgent.ToString());
            drp_hasAuthUserReport.setSelectedValue(_currTBL.hasAuthUserReport.ToString());
            drp_rnt_canChangeReservationAccount.setSelectedValue(_currTBL.rnt_canChangeReservationAccount.ToString());
            pnlContent.Visible = true;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scrolTo", "$.scrollTo($(\"#" + pnlContent.ClientID + "\"), 500);", true);
     
        }
        private void FillDataFromControls()
        {
            _currTBL = DC_USER.USR_ADMIN.FirstOrDefault(x => x.id != HF_id.Value.ToInt32() && (x.email == txt_email.Text));
            if (_currTBL != null && drp_is_active.getSelectedValueInt(0) == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    "alertError",
                                                    "alert('La E-mail inserito risulta già registrato per utente " + _currTBL.name + " " + _currTBL.surname + "');", true);
                return;
            }
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new USR_ADMIN();
                _currTBL.is_deleted = 0;
                DC_USER.USR_ADMIN.InsertOnSubmit(_currTBL);
            }
            _currTBL.name = txt_name.Text;
            _currTBL.surname = txt_surname.Text;
            _currTBL.email = txt_email.Text;
            _currTBL.pid_role = drp_pid_role.getSelectedValueInt(0);

            _currTBL.login = txt_login.Text;
            _currTBL.password = txt_password.Text;
            _currTBL.is_active = drp_is_active.getSelectedValueInt(0);
            _currTBL.isAgentContact = drp_isAgentContact.getSelectedValueInt();
            _currTBL.rnt_canHaveReservation = drp_rnt_canHaveReservation.getSelectedValueInt(0);
            _currTBL.rnt_canHaveRequest = drp_rnt_canHaveRequest.getSelectedValueInt(0);
            _currTBL.rnt_canHaveAgent = drp_rnt_canHaveAgent.getSelectedValueInt(0);
            _currTBL.hasAuthUserReport = drp_hasAuthUserReport.getSelectedValueInt(0);
            _currTBL.rnt_canChangeReservationAccount = drp_rnt_canChangeReservationAccount.getSelectedValueInt(0);

            DC_USER.SubmitChanges();
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }
        protected void lnk_delete_Click(object sender, EventArgs e)
        {
            _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (_currTBL != null)
            {
                DC_USER.USR_ADMIN.DeleteOnSubmit(_currTBL);
                DC_USER.SubmitChanges();
            }
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }
        protected void lnk_nuovo_Click(object sender, EventArgs e)
        {
            LV.SelectedIndex = -1;
            LV.DataBind();
            HF_id.Value = "0";
            FillControls();
        }
    }
}
