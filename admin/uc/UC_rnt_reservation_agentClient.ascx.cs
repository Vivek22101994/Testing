using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModAuth;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_agentClient : System.Web.UI.UserControl
    {
        protected AuthClientTBL _currTBL;
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
        public long IdClient
        {
            get { return HF_id.Value.ToInt64(); }
            set { HF_id.Value = value.ToString(); }
        }
        public long IdAgent
        {
            get { return HF_IdAgent.Value.ToInt64(); }
            set { HF_IdAgent.Value = value.ToString(); }
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
        public string cl_loc_country
        {
            get { return HF_country.Value; }
            set { HF_country.Value = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                filterOldClients();
            }
        }
        protected void Bind_drp_honorific()
        {
            List<USR_LK_HONORIFIC> _list = maga_DataContext.DC_USER.USR_LK_HONORIFICs.OrderBy(x => x.title).ToList();
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
            if (mode == "edit")
            {
                drp_country.DataBind();
                drp_country.setSelectedValue(ltr_country.Text);
                txt_email.Text = ltr_email.Text;
                txt_email_conf.Text = "";
                drp_honorific.setSelectedValue(ltr_honorific.Text);
                txt_name_full.Text = ltr_name_full.Text;
                txt_phone_mobile.Text = ltr_phone_mobile.Text;
                drp_lang.DataBind();
                drp_lang.setSelectedValue(HF_pid_lang.Value);
                txt_city.Text = ltr_city.Text;
                txt_zip_code.Text = ltr_zip_code.Text;
                txt_address.Text = ltr_address.Text;
                rdp_docIssueDate.SelectedDate = Convert.ToDateTime(ltr_issue_date.Text);
                txt_docNum.Text = ltr_doc_number.Text;
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
            using (magaAuth_DataContext dc = maga_DataContext.DC_Auth)
            {
                Bind_drp_honorific();
                _currTBL = null;
                _currTBL = dc.AuthClientTBL.SingleOrDefault(x => x.id == IdClient);
                if (_currTBL == null)
                {
                    if (IdClient == 0 || IdClient == -1)
                    {
                        showMode("new");
                        return;
                    }
                    RNT_TBL_RESERVATION _res = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                    RNT_TBL_REQUEST _request = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == IdRequest);
                    if (_res == null && _request == null)
                    {
                        showMode("new");
                        return;
                    }
                    if (_request != null)
                    {
                        _currTBL = dc.AuthClientTBL.FirstOrDefault(x => x.contactEmail == _request.email && x.pidAgent == IdAgent);
                        if (_currTBL == null)
                        {
                            _currTBL = new AuthClientTBL();
                            _currTBL.pidAgent = IdAgent;
                            _currTBL.contactEmail = _request.email;
                            _currTBL.nameHonorific = _request.name_honorific;
                            _currTBL.nameFull = _request.name_full;
                            _currTBL.pidLang = _request.pid_lang + "";
                            _currTBL.contactPhoneMobile = _request.phone;
                            _currTBL.locCountry = _request.request_country;
                            _currTBL.isActive = 1;
                            _currTBL.authUsr = _currTBL.contactEmail;
                            _currTBL.authPwd = CommonUtilities.CreatePassword(8, false, true, false);
                            _currTBL.uid = Guid.NewGuid();
                            _currTBL.createdDate = DateTime.Now;
                            _currTBL.createdUserID = 1;
                            _currTBL.createdUserNameFull = "System";
                            dc.AuthClientTBL.InsertOnSubmit(_currTBL);
                            dc.SubmitChanges();
                            _currTBL.code = _currTBL.id.ToString().fillString("0", 6, false);
                            dc.SubmitChanges();
                            authProps.ClientTBL = dc.AuthClientTBL.ToList();
                        }
                        IdClient = _currTBL.id;
                    }
                    if (_res != null)
                    {
                        _currTBL = dc.AuthClientTBL.FirstOrDefault(x => x.contactEmail == _res.cl_email && x.pidAgent == IdAgent);
                        if (_currTBL == null)
                        {
                            _currTBL = new AuthClientTBL();
                            _currTBL.pidAgent = IdAgent;
                            _currTBL.contactEmail = _res.cl_email;
                            _currTBL.nameHonorific = _res.cl_name_honorific;
                            _currTBL.nameFull = _res.cl_name_full;
                            _currTBL.pidLang = _res.cl_pid_lang + "";
                            _currTBL.isActive = 1;
                            _currTBL.authUsr = _currTBL.contactEmail;
                            _currTBL.authPwd = CommonUtilities.CreatePassword(8, false, true, false);
                            _currTBL.uid = Guid.NewGuid();
                            _currTBL.createdDate = DateTime.Now;
                            _currTBL.createdUserID = 1;
                            _currTBL.createdUserNameFull = "System";
                            dc.AuthClientTBL.InsertOnSubmit(_currTBL);
                            dc.SubmitChanges();
                            _currTBL.code = _currTBL.id.ToString().fillString("0", 6, false);
                            dc.SubmitChanges();
                            authProps.ClientTBL = dc.AuthClientTBL.ToList();
                        }
                        IdClient = _currTBL.id;
                    }
                }
                else if (_currTBL.pidAgent != IdAgent)
                {
                    _currTBL.pidAgent = IdAgent;
                    dc.SubmitChanges();
                }
                HF_name_honorific.Value = _currTBL.nameHonorific;
                HF_name_full.Value = _currTBL.nameFull;
                HF_contact_email.Value = _currTBL.contactEmail;
                HF_country.Value = _currTBL.locCountry;
                HF_pid_lang.Value = _currTBL.pidLang + "";
                ltr_country.Text = _currTBL.locCountry;
                ltr_email.Text = _currTBL.contactEmail;
                ltr_honorific.Text = _currTBL.nameHonorific;
                ltr_name_full.Text = _currTBL.nameFull;
                ltr_phone_mobile.Text = _currTBL.contactPhoneMobile;
                ltr_city.Text = _currTBL.locCity;
                ltr_address.Text = _currTBL.locAddress;
                ltr_doc_number.Text = _currTBL.docNum;
                if (!string.IsNullOrEmpty(_currTBL.docIssueDate + ""))
                    ltr_issue_date.Text = Convert.ToDateTime(_currTBL.docIssueDate).ToString("dd/MM/yyyy");
                ltr_zip_code.Text = _currTBL.locZipCode;
                showMode("view");
            }
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
            if (!txt_email.Text.Trim().isEmail())
            {
                txt_email.CssClass = "required_field";
                lbl_errorAlert.Text = "L'E-mail non valido.";
                lbl_errorAlert.Visible = true;
                return;
            }
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
            using (magaAuth_DataContext dc = maga_DataContext.DC_Auth)
            {
                AuthClientTBL _client;
                if (HF_mode.Value == "edit")
                {
                    _client = dc.AuthClientTBL.SingleOrDefault(x => x.id == IdClient);
                    if (_client != null)
                    {
                        _client.locCountry = drp_country.SelectedValue;
                        _client.contactEmail = txt_email.Text;
                        _client.nameHonorific = drp_honorific.SelectedValue;
                        _client.nameFull = txt_name_full.Text;
                        _client.contactPhoneMobile = txt_phone_mobile.Text;
                        _client.pidLang = drp_lang.SelectedValue;
                        _client.locCity = txt_city.Text;
                        _client.locZipCode = txt_zip_code.Text;
                        _client.docNum = txt_docNum.Text;
                        _client.docIssueDate = rdp_docIssueDate.SelectedDate;
                        _client.locAddress = txt_address.Text;
                        dc.SubmitChanges();
                        authProps.ClientTBL = dc.AuthClientTBL.ToList();
                        IdClient = _client.id;
                        FillControls();
                        if (onSave != null) { onSave(this, new EventArgs()); }
                    }
                    return;
                }
                _client = dc.AuthClientTBL.FirstOrDefault(x => x.contactEmail == txt_email.Text.Trim() && x.pidAgent == IdAgent);
                if (_client == null)
                {
                    _client = new AuthClientTBL();
                    _client.pidAgent = IdAgent;
                    _client.locCountry = drp_country.SelectedValue;
                    _client.contactEmail = txt_email.Text;
                    _client.nameHonorific = drp_honorific.SelectedValue;
                    _client.nameFull = txt_name_full.Text;
                    _client.contactPhoneMobile = txt_phone_mobile.Text;
                    _client.locCity = txt_city.Text;
                    _client.locZipCode = txt_zip_code.Text;
                    _client.docNum = txt_docNum.Text;
                    _client.docIssueDate = rdp_docIssueDate.SelectedDate;
                    _client.locAddress = txt_address.Text;
                    _client.pidLang = drp_lang.SelectedValue;
                    _client.authUsr = _client.contactEmail;
                    _client.authPwd = CommonUtilities.CreatePassword(8, false, true, false);
                    _client.uid = Guid.NewGuid();
                    _client.createdDate = DateTime.Now;
                    _client.createdUserID = 1;
                    _client.createdUserNameFull = "System";
                    dc.AuthClientTBL.InsertOnSubmit(_client);
                    dc.SubmitChanges();
                    _client.code = _client.id.ToString().fillString("0", 6, false);
                    dc.SubmitChanges();
                    authProps.ClientTBL = dc.AuthClientTBL.ToList();
                    IdClient = _client.id;
                    FillControls();
                    if (onSave != null) { onSave(this, new EventArgs()); }
                    return;
                }

                if (_client.isActive == 1)
                {
                    lbl_errorAlert.Text = "L'E-mail risulta registrato, <br/>controllare i dati del cliente e selezionare se sono gli stessi <br/>altrimenti inserire un'altra E-mail.";
                    showMode("old");
                    lbl_errorAlert.Visible = true;
                    txt_flt_email.Text = _client.contactEmail;
                    filterOldClients();
                    return;
                }
                if (_client.isActive != 1)
                {
                    lbl_errorAlert.Text = "L'E-mail risulta registrato, ma Disabilitato.<br/>Controllare.";
                    lbl_errorAlert.Visible = true;
                    return;
                }
            }
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            filterOldClients();
        }
        protected void filterOldClients()
        {
            string _filter = "pidAgent=" + IdAgent;
            string _sep = " and ";
            if (txt_flt_email.Text.Trim() != "")
            {
                _filter += _sep + "contactEmail.Contains(\"" + txt_flt_email.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_flt_name_full.Text.Trim() != "")
            {
                _filter += _sep + "nameFull.Contains(\"" + txt_flt_name_full.Text.Trim() + "\")";
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
            if (lbl_is_active == null || lbl_id == null || lnk_select == null || lbl_not_active == null) return;
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