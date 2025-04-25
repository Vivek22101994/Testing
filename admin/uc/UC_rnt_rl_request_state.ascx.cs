using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_rl_request_state : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_REQUEST _currTBL;
        public bool ShowBody
        {
            get
            {
                return HF_show_body.Value == "1";
            }
            set
            {
                HF_show_body.Value = value ? "1" : "0";
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
        public string cl_email
        {
            get
            {
                return HF_cl_email.Value;
            }
            set
            {
                HF_cl_email.Value = value;
            }
        }
        public string estatesOK
        {
            get
            {
                return ltr_estatesOK.Text;
            }
            set
            {
                ltr_estatesOK.Text = value;
            }
        }
        public string estatesNO
        {
            get
            {
                return ltr_estatesNO.Text;
            }
            set
            {
                ltr_estatesNO.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                string _script = "var " + Unique + "_editors = ['" + txt_body.ClientID + "'];";
                _script += "function  " + Unique + "_removeTinyEditor() {";
                _script += "removeTinyEditors( " + Unique + "_editors);}";
                _script += "function  " + Unique + "_setTinyEditor(IsReadOnly) {";
                _script += "setTinyEditors( " + Unique + "_editors, IsReadOnly);}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Unique, _script, true);
                _script = "var " + Unique + "_editors1 = ['" + txt_mail_body.ClientID + "'];";
                _script += "function  " + Unique + "_removeTinyEditor1() {";
                _script += "removeTinyEditors( " + Unique + "_editors1);}";
                _script += "function  " + Unique + "_setTinyEditor1(IsReadOnly) {";
                _script += "setTinyEditors( " + Unique + "_editors1, IsReadOnly);}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Unique+"1", _script, true);

                Bind_drp_contract_state();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), Unique + "_checkState", "checkState(); ", true);
        }
        protected void Bind_drp_contract_state()
        {
            List<RNT_LK_REQUEST_STATE> _list = DC_RENTAL.RNT_LK_REQUEST_STATEs.Where(x => x.type == 1).ToList();
            drp_contract_state.DataSource = _list;
            drp_contract_state.DataTextField = "title";
            drp_contract_state.DataValueField = "id";
            drp_contract_state.DataBind();
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
            ltr_user.Text = CommonUtilities.GetUserName("" + _currTBL.state_pid_user);
            ltr_state.Text = rntUtils.rntRequest_getStateName("" + _currTBL.state_pid);
            ltr_date.Text = _currTBL.state_date.Value.ToLongDateString();
            ltr_time.Text = _currTBL.state_date.Value.ToShortTimeString();
            ltr_subject.Text = _currTBL.state_subject;
            ltr_body.Text = _currTBL.state_body;
            DisableControls();
            if (_currTBL.pid_related_request!=0)
            {
                pnl_has_related.Visible = true;
                pnl_buttons.Visible = false;
            }
            if (_currTBL.state_pid == 5 || _currTBL.state_pid==6)
            {
                lnk_salva.Visible = false;
                lnk_annulla.Visible = false;
                lnk_modify.Visible = false;
                lnk_addContact.Visible = false;
                lbl_resError.Visible = true;
                lbl_resError.InnerText = "lo Stato non può essere cambiato alle Richieste concluse!";
            }
            LV.SelectedIndex = -1;
            LDS.DataBind();
            LV.DataBind();
        }
        private void FillDataFromControls()
        {
            lbl_resError.Visible = false;
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
            if (HF_mod_mode.Value == "1")
            {
                if (drp_contract_state.getSelectedValueInt(0)==5)
                {
                    RNT_TBL_RESERVATION _res = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.code == txt_res_code.Text);
                    if (_res==null)
                    {
                        lbl_resError.Visible = true;
                        lbl_resError.InnerText = "Prenotazione non esistente!";
                        return;
                    }
                    if (_res.pid_related_request.objToInt32() != 0 && _res.pid_related_request.objToInt32()!=_currTBL.id)
                    {
                        lbl_resError.Visible = true;
                        lbl_resError.InnerText = "Prenotazione gia abbinata ad un'altra richiesta!";
                        return;
                    }
                    _res.pid_related_request = _currTBL.id;
                    _currTBL.pid_reservation = _res.id;
                }
                _currTBL.state_body = txt_body.Text;
                _currTBL.state_subject = txt_subject.Text;
                _currTBL.state_date = DateTime.Now;
                _currTBL.state_pid = drp_contract_state.getSelectedValueInt(0);
                _currTBL.state_pid_user = UserAuthentication.CurrentUserID;
                DC_RENTAL.SubmitChanges();
                List<RNT_TBL_REQUEST> _list = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.pid_related_request == _currTBL.id).ToList();
                foreach (RNT_TBL_REQUEST _req in _list)
                {
                    _req.state_body = txt_body.Text;
                    _req.state_subject = txt_subject.Text;
                    _req.state_date = DateTime.Now;
                    _req.state_pid = drp_contract_state.getSelectedValueInt(0);
                    _req.state_pid_user = UserAuthentication.CurrentUserID;
                    _req.pid_reservation = _currTBL.pid_reservation;
                }
                DC_RENTAL.SubmitChanges();
                rntUtils.rntRequest_addState(IdRequest, _currTBL.state_pid.Value, _currTBL.state_pid_user.Value, _currTBL.state_subject, _currTBL.state_body);
            }
            else if (HF_mod_mode.Value == "2")
            {
                rntUtils.rntRequest_addState(IdRequest, 3, UserAuthentication.CurrentUserID, drp_subject.SelectedValue, txt_body.Text);
                txt_body.Text = "";
            }
            else if (HF_mod_mode.Value == "3")
            {
                USR_ADMIN_CONFIG _config = UserAuthentication.CURRENT_USR_ADMIN_CONFIG;
                if (_config == null)
                {
                    return;
                }
                if (MailingUtilities.autoSendMailTo_from(txt_mail_subject.Text, txt_mail_body.Text, cl_email, false, _config.mailing_from_mail, _config.mailing_from_name, "admin_uc_UC_rnt_rl_request_state - mail del account al cliente"))
                { 
                    rntUtils.rntRequest_addState(IdRequest, 3, UserAuthentication.CurrentUserID, txt_mail_subject.Text, txt_mail_body.Text);
                    _currTBL.mail_out = 1;
                    DC_RENTAL.SubmitChanges();
                }
                txt_mail_subject.Text = "";
                txt_mail_body.Text = "";
            }
            ShowBody = false;
            FillControls();
        }
        public void checkShowBody()
        {
            if (!ShowBody) return;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Unique + "_tinyEditor", "setTinyEditors( " + Unique + "_editors" + (HF_mod_mode.Value == "3" ? "1" : "") + ", true); ", true);
            lnk_modify.OnClientClick = Unique + "_removeTinyEditor" + (HF_mod_mode.Value == "3" ? "1" : "") + "()";
            lnk_salva.OnClientClick = Unique + "_removeTinyEditor" + (HF_mod_mode.Value == "3" ? "1" : "") + "()";
            lnk_annulla.OnClientClick = Unique + "_removeTinyEditor" + (HF_mod_mode.Value == "3" ? "1" : "") + "()";
            lnk_addContact.OnClientClick = Unique + "_removeTinyEditor" + (HF_mod_mode.Value == "3" ? "1" : "") + "()";
            lnk_copyNO.OnClientClick = "addText('#AvvNO#'); " + Unique + "_removeTinyEditor" + (HF_mod_mode.Value == "3" ? "1" : "") + "()";
            lnk_copyOK.OnClientClick = "addText('#AvvOK#'); " + Unique + "_removeTinyEditor" + (HF_mod_mode.Value == "3" ? "1" : "") + "()";
        }

        protected void DisableControls()
        {
            PH_view.Visible = true;
            PH_modify.Visible = false;
            PH_sendMail.Visible = false;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
            lnk_addContact.Visible = true;
            lnk_sendMail.Visible = true;
            if (UserAuthentication.CURRENT_USR_ADMIN_CONFIG == null)
                lnk_sendMail.Visible = false;
            checkShowBody();
        }
        protected void EnableControls()
        {
            PH_mod_state.Visible = HF_mod_mode.Value == "1";
            PH_add_contact.Visible = HF_mod_mode.Value == "2";
            PH_sendMail.Visible = HF_mod_mode.Value == "3";
            lbl_title.InnerHtml = HF_mod_mode.Value == "1" ? "Cambio Stato" : "Note per il Cliente";
            PH_view.Visible = false;
            PH_modify.Visible = HF_mod_mode.Value != "3";
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
            lnk_addContact.Visible = false;
            lnk_sendMail.Visible = false;
            checkShowBody();
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
        protected void lnk_modify_Click(object sender, EventArgs e)
        {
            HF_mod_mode.Value = "1";
            ShowBody = true;
            EnableControls();
        }
        protected void lnk_addContact_Click(object sender, EventArgs e)
        {
            HF_mod_mode.Value = "2";
            ShowBody = true;
            EnableControls();
        }
        protected void lnk_sendMail_Click(object sender, EventArgs e)
        {
            USR_ADMIN_CONFIG _config = UserAuthentication.CURRENT_USR_ADMIN_CONFIG;
            if (_config == null)
            {
                return;
            }
            txt_mail_body.Text = _config.mailing_header + "<br/><br/>" + _config.mailing_signature;
            HF_mod_mode.Value = "3";
            ShowBody = true;
            EnableControls();
        }
        protected void lnk_copyOK_Click(object sender, EventArgs e)
        {
            txt_mail_body.Text = txt_mail_body.Text.Replace("#AvvOK#", estatesOK).Replace("#AvvNO#", estatesNO);
            HF_mod_mode.Value = "3";
            ShowBody = true;
            EnableControls();
        }

        protected void lnk_copyNO_Click(object sender, EventArgs e)
        {
            return;
            USR_ADMIN_CONFIG _config = UserAuthentication.CURRENT_USR_ADMIN_CONFIG;
            if (_config == null)
            {
                return;
            }
            txt_mail_body.Text = _config.mailing_header + "<br/>TestoFinto: cambiare<br/>" + estatesNO + "<br/>TestoFinto: cambiare<br/>" + _config.mailing_signature;
            HF_mod_mode.Value = "3";
            ShowBody = true;
            EnableControls();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            ShowBody = false;
            FillControls();
            lbl_resError.Visible = false;
        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if(e.CommandName=="close")
            {
                LV.SelectedIndex = -1;
                ShowBody = false;
                DisableControls();
            }
        }

        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            Label lbl_id = LV.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            if (lbl_id != null)
            {
            }
            ShowBody = false;
            DisableControls();
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_pid_state = e.Item.FindControl("lbl_pid_state") as Label;
            System.Web.UI.HtmlControls.HtmlTableRow tr_normal = e.Item.FindControl("tr_normal") as System.Web.UI.HtmlControls.HtmlTableRow;
            LinkButton lnk_select = e.Item.FindControl("lnk_select") as LinkButton;
            if (tr_normal != null && lnk_select != null)
            {
                if (lbl_pid_state != null && lbl_pid_state.Text != "0" && lbl_pid_state.Text != "1")
                {
                    tr_normal.Attributes.Add("onclick", "__doPostBack('" + lnk_select.UniqueID + "','')");
                    tr_normal.Style.Add("cursor", "pointer");
                    lnk_select.Visible = true;
                }
                else
                {
                    lnk_select.Visible = false;
                }
            }
            System.Web.UI.HtmlControls.HtmlTableRow tr_selected = e.Item.FindControl("tr_selected") as System.Web.UI.HtmlControls.HtmlTableRow;
            LinkButton lnk_close = e.Item.FindControl("lnk_close") as LinkButton;
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (tr_selected != null && lnk_close != null && lbl_id != null)
            {
                tr_selected.Attributes.Add("onclick", "__doPostBack('" + lnk_close.UniqueID + "','')");
            }
        }

    }
}
