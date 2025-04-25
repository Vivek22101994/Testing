using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_client : System.Web.UI.UserControl
    {
        protected magaUser_DataContext DC_USER;
        protected USR_TBL_CLIENT _currTBL;
        public event EventHandler onModify;
        public event EventHandler onSave;
        public event EventHandler onCancel;
        public bool IsLocked
        {
            get { return pnl_lock.Visible; }
            set { pnl_lock.Visible = value; }
        }
        public bool IsEdit
        {
            get { return HF_isEdit.Value == "1"; }
            set { HF_isEdit.Value = value ? "1" : "0"; }
        }
        public int IdClient
        {
            get { return HF_id.Value.ToInt32(); }
            set { HF_id.Value = value.ToString(); }
        }
        public long IdReservation
        {
            get { return HF_IdReservation.Value.ToInt32(); }
            set { HF_IdReservation.Value = value.ToString(); }
        }
        public long IdRequest
        {
            get { return HF_IdRequest.Value.ToInt64(); }
            set { HF_IdRequest.Value = value.ToString(); }
        }
        public int cl_pid_lang
        {
            get { return HF_pid_lang.Value.ToInt32(); }
            set { HF_pid_lang.Value = value.ToString(); }
        }
        public int cl_pid_discount
        {
            get { return HF_pid_discount.Value.ToInt32(); }
            set { HF_pid_discount.Value = value.ToString(); }
        }
        public string cl_name_honorific
        {
            get { return HF_name_honorific.Value; }
            set { HF_name_honorific.Value = value; }
        }
        public string cl_name_full
        {
            get { return HF_name_full.Value; }
            set { HF_name_full.Value = value; }
        }
        public string cl_email
        {
            get { return HF_contact_email.Value; }
            set { HF_contact_email.Value = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                filterOldClients();
            }
        }
        protected void Bind_drp_honorific()
        {
            List<USR_LK_HONORIFIC> _list = DC_USER.USR_LK_HONORIFICs.OrderBy(x => x.title).ToList();
            drp_honorific.DataSource = _list;
            drp_honorific.DataTextField = "title";
            drp_honorific.DataValueField = "title";
            drp_honorific.DataBind();
        }
        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem("- - -", ""));
        }
        protected void drp_lang_DataBound(object sender, EventArgs e)
        {
            drp_lang.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
        protected void showMode(string mode)
        {
            PH_viewClient.Visible = mode == "view";
            PH_oldClient.Visible = mode == "old";
            PH_newClient.Visible = mode == "new" || mode == "edit";
            PH_bookingForm.Visible = mode != "old";
            HF_mode.Value = mode;
            lbl_errorAlert.Visible = false;
            if(mode == "edit")
            {

                txt_email.Text = ltr_email.Text;
                txt_email_conf.Text = "";
                drp_honorific.setSelectedValue(ltr_honorific.Text);
                txt_name_full.Text = ltr_name_full.Text;
                txt_phone_mobile.Text = ltr_phone_mobile.Text;
            }
            lnk_cancel_1.Visible = IdClient != 0;
            lnk_cancel_2.Visible = IdClient != 0;
        }

        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            FillControls();
            if (onCancel != null) { onCancel(this, new EventArgs()); }
        }
        protected void lnk_changeClient_Click(object sender, EventArgs e)
        {
            showMode("old");
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void lnk_editClient_Click(object sender, EventArgs e)
        {
            showMode("edit");
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void lnk_goNewClient_Click(object sender, EventArgs e)
        {
            showMode("new");
        }
        protected void lnk_goOldClient_Click(object sender, EventArgs e)
        {
            showMode("old");
        }
        public void FillControls()
        {
            DC_USER = maga_DataContext.DC_USER;
            Bind_drp_honorific();
            _currTBL = null;
            _currTBL = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == IdClient);
            if (_currTBL == null)
            {
                if (IdClient == 0 || IdClient == -1)
                {
                    showMode("new");
                    return;
                }
                RNT_TBL_RESERVATION _res = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                RNT_TBL_REQUEST _request = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == IdRequest);
                if (_res == null && _request==null)
                {
                    showMode("new");
                    return;
                }
                if (_request != null)
                {
                    _currTBL = DC_USER.USR_TBL_CLIENTs.FirstOrDefault(x => x.contact_email == _request.email);
                    if (_currTBL == null)
                    {
                        _currTBL = new USR_TBL_CLIENT();
                        _currTBL.contact_email = _request.email;
                        _currTBL.name_honorific = _request.name_honorific;
                        _currTBL.name_full = _request.name_full;
                        _currTBL.pid_lang = _request.pid_lang;
                        _currTBL.contact_phone_mobile = _request.phone;
                        _currTBL.loc_country = _request.request_country;
                        _currTBL.isCompleted = 0;
                        _currTBL.is_deleted = 0;
                        _currTBL.is_active = 1;
                        _currTBL.login = _currTBL.contact_email;
                        _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
                        _currTBL.date_created = DateTime.Now;
                        DC_USER.USR_TBL_CLIENTs.InsertOnSubmit(_currTBL);
                        DC_USER.SubmitChanges();
                        _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
                        DC_USER.SubmitChanges();
                    }
                    IdClient = _currTBL.id;
                }
                if (_res != null)
                {
                    _currTBL = DC_USER.USR_TBL_CLIENTs.FirstOrDefault(x => x.contact_email == _res.cl_email);
                    if (_currTBL == null)
                    {
                        _currTBL = new USR_TBL_CLIENT();
                        _currTBL.contact_email = _res.cl_email;
                        _currTBL.name_honorific = _res.cl_name_honorific;
                        _currTBL.name_full = _res.cl_name_full;
                        _currTBL.pid_lang = _res.cl_pid_lang;
                        _currTBL.isCompleted = 0;
                        _currTBL.is_deleted = 0;
                        _currTBL.is_active = 1;
                        _currTBL.login = _currTBL.contact_email;
                        _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
                        _currTBL.date_created = DateTime.Now;
                        DC_USER.USR_TBL_CLIENTs.InsertOnSubmit(_currTBL);
                        DC_USER.SubmitChanges();
                        _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
                        DC_USER.SubmitChanges();
                    }
                    IdClient = _currTBL.id;
                }
            }
            HF_name_honorific.Value = _currTBL.name_honorific;
            HF_name_full.Value = _currTBL.name_full;
            HF_contact_email.Value = _currTBL.contact_email;
            HF_pid_lang.Value = _currTBL.pid_lang.ToString();
            ltr_country.Text = _currTBL.loc_country;
            ltr_email.Text = _currTBL.contact_email;
            ltr_honorific.Text = _currTBL.name_honorific;
            ltr_name_full.Text = _currTBL.name_full;
            ltr_phone_mobile.Text = _currTBL.contact_phone_mobile;
            drp_country.DataBind();
            drp_country.setSelectedValue(_currTBL.loc_country);
            drp_lang.DataBind();
            drp_lang.setSelectedValue(_currTBL.pid_lang);
            showMode("view");
        }

        protected void lnk_saveNew_Click(object sender, EventArgs e)
        {
            lbl_errorAlert.Visible = false;
            txt_email.CssClass = "";
            txt_email_conf.CssClass = "";
            txt_name_full.CssClass = "";
            drp_lang.CssClass = "";
            drp_country.CssClass = "";
            txt_phone_mobile.CssClass = "";
            if (txt_email.Text.Trim() == "")
            {
                txt_email.CssClass = "required_field";
                lbl_errorAlert.Text = "Campo E-mail deve essere riempito.";
                lbl_errorAlert.Visible = true;
                return;
            }
            //if (!txt_email.Text.Trim().isEmail())
            //{
            //    txt_email.CssClass = "required_field";
            //    lbl_errorAlert.Text = "L'E-mail non valido.";
            //    lbl_errorAlert.Visible = true;
            //    return;
            //}
            if (txt_email.Text.Trim() != txt_email_conf.Text.Trim())
            {
                txt_email.CssClass = "required_field";
                txt_email_conf.CssClass = "required_field";
                lbl_errorAlert.Text = "Le due E-mail non sono uguali.";
                lbl_errorAlert.Visible = true;
                return;
            }
            if (txt_name_full.Text.Trim().Length < 5)
            {
                txt_name_full.CssClass = "required_field";
                lbl_errorAlert.Text = "Campo Nome completo deve avere almeno 5 caratteri.";
                lbl_errorAlert.Visible = true;
                return;
            }
            if (drp_lang.SelectedValue == "0")
            {
                drp_lang.CssClass = "required_field";
                lbl_errorAlert.Text = "Selezionare la Lingua del Cliente.";
                lbl_errorAlert.Visible = true;
                return;
            }
            if (txt_phone_mobile.Text.Trim() == "")
            {
                txt_phone_mobile.CssClass = "required_field";
                lbl_errorAlert.Text = "Campo Cellulare deve essere riempito.";
                lbl_errorAlert.Visible = true;
                return;
            }
            if (drp_country.SelectedValue == "")
            {
                drp_country.CssClass = "required_field";
                lbl_errorAlert.Text = "Selezionare il Paese del Cliente.";
                lbl_errorAlert.Visible = true;
                return;
            }
            DC_USER = maga_DataContext.DC_USER;
            USR_TBL_CLIENT _client;
            if (HF_mode.Value == "edit")
            {
                _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == IdClient);
                if (_client != null)
                {
                    _client.loc_country = drp_country.SelectedValue;
                    _client.contact_email = txt_email.Text;
                    _client.name_honorific = drp_honorific.SelectedValue;
                    _client.name_full = txt_name_full.Text;
                    _client.contact_phone_mobile = txt_phone_mobile.Text;
                    _client.pid_lang = drp_lang.getSelectedValueInt(0);
                    DC_USER.SubmitChanges();
                    IdClient = _client.id;
                    FillControls();
                    if (onSave != null) { onSave(this, new EventArgs()); }
                }
                return;
            }
            _client = DC_USER.USR_TBL_CLIENTs.FirstOrDefault(x => x.is_deleted != 1 && x.contact_email == txt_email.Text.Trim());
            if (_client == null)
            {
                _client = new USR_TBL_CLIENT();
                _client.loc_country = drp_country.SelectedValue;
                _client.contact_email = txt_email.Text;
                _client.name_honorific = drp_honorific.SelectedValue;
                _client.name_full = txt_name_full.Text;
                _client.contact_phone_mobile = txt_phone_mobile.Text;
                _client.pid_lang = drp_lang.getSelectedValueInt(0);
                _client.isCompleted = 0;
                _client.is_deleted = 0;
                _client.is_active = 1;
                _client.login = _client.contact_email;
                _client.password = CommonUtilities.CreatePassword(8, false, true, false);
                DC_USER.USR_TBL_CLIENTs.InsertOnSubmit(_client);
                DC_USER.SubmitChanges();
                _client.code = _client.id.ToString().fillString("0", 7, false);
                DC_USER.SubmitChanges();
                IdClient = _client.id;
                FillControls();
                if (onSave != null) { onSave(this, new EventArgs()); }
                return;
            }

            if (_client.is_active == 1)
            {
                lbl_errorAlert.Text = "L'E-mail risulta registrato, <br/>controllare i dati del cliente e selezionare se sono gli stessi <br/>altrimenti inserire un'altra E-mail.";
                showMode("old");
                lbl_errorAlert.Visible = true;
                txt_flt_email.Text = _client.contact_email;
                filterOldClients();
                return;
            }
            if (_client.is_active != 1)
            {
                lbl_errorAlert.Text = "L'E-mail risulta registrato, ma Disabilitato.<br/>Controllare.";
                lbl_errorAlert.Visible = true;
                return;
            }
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            filterOldClients();
        }
        protected void filterOldClients()
        {
            string _filter = "is_deleted!=1";
            string _sep = " and ";
            if (txt_flt_email.Text.Trim() != "")
            {
                _filter += _sep + "contact_email.Contains(\"" + txt_flt_email.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_flt_name_full.Text.Trim() != "")
            {
                _filter += _sep + "name_full.Contains(\"" + txt_flt_name_full.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_flt_email.Text.Trim() == "" && txt_flt_name_full.Text.Trim() == "")
            {
                _filter = "1=2";
            }
            LDS_flt.Where = _filter;
            LDS_flt.DataBind();
            LV_flt.DataBind();
        }

        protected void LV_flt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            LinkButton lnk_select = e.Item.FindControl("lnk_select") as LinkButton;
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_is_active = e.Item.FindControl("lbl_is_active") as Label;
            Label lbl_not_active = e.Item.FindControl("lbl_not_active") as Label;
            if (lbl_is_active == null || lbl_id == null || lnk_select == null || lbl_not_active==null) return;
            lbl_not_active.Visible = lbl_is_active.Text != "1";
            lnk_select.Visible = lbl_is_active.Text == "1";
        }

        protected void LV_flt_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            LinkButton lnk_select = e.Item.FindControl("lnk_select") as LinkButton;
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_is_active = e.Item.FindControl("lbl_is_active") as Label;
            Label lbl_not_active = e.Item.FindControl("lbl_not_active") as Label;
            if (lbl_is_active == null || lbl_id == null || lnk_select == null || lbl_not_active == null) return;
            IdClient = lbl_id.Text.ToInt32();
            FillControls();
            if (onSave != null) { onSave(this, new EventArgs()); }
        }
    }
}