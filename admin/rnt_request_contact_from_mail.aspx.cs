using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_request_contact_from_mail : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_mail";
        }
        private magaRental_DataContext DC_RENTAL;
        protected magaMail_DataContext DC_MAIL;
        private MAIL_TBL_MESSAGE _currTBL;
        protected string listPage = "temp_prove_mail.aspx";
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public int IdMessage
        {
            get
            {
                int _id;
                if (int.TryParse(HF_id.Value, out _id))
                    return _id;
                return 0;
            }
            set
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                HF_id.Value = value.ToString();
                FillControls();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_MAIL = maga_DataContext.DC_MAIL;
            if (!IsPostBack)
            {
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    _currTBL = DC_MAIL.MAIL_TBL_MESSAGE.SingleOrDefault(x => x.id == _id);
                    if (_currTBL == null)
                        Response.Redirect(listPage);
                    IdMessage = _id;
                    FillControls();
                }
                else
                    Response.Redirect(listPage);
            }
            RegisterScripts();
        }
        protected void Bind_drp_relatedRequests()
        {
            drp_relatedRequests.Items.Clear();
            drp_relatedRequests.Items.Add(new ListItem("-seleziona-", "-1"));
            List<RNT_TBL_REQUEST> _list = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.email == HF_email.Value && x.pid_related_request == 0).OrderByDescending(x => x.request_date_created).ToList();
            foreach (RNT_TBL_REQUEST _relRequest in _list)
            {
                drp_relatedRequests.Items.Add(new ListItem("rif. " + _relRequest.id + " - " + _relRequest.name_full + " - del " + _relRequest.request_date_created, "" + _relRequest.id));
            }
        }
        public void FillControls()
        {
            DC_MAIL = maga_DataContext.DC_MAIL;
            if (IdMessage != 0)
                _currTBL = DC_MAIL.MAIL_TBL_MESSAGE.SingleOrDefault(x => x.id == IdMessage);
            if (_currTBL == null)
                return;
            ltr_body_html_text.Text = _currTBL.body_html_text;
            ltr_body_plain_text.Text = _currTBL.body_plain_text;
            txt_body.Text = (ltr_body_html_text.Text != "") ? ltr_body_html_text.Text : ltr_body_plain_text.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");

            HF_email.Value = _currTBL.from_email;
            HF_pid_request.Value = _currTBL.pid_request.ToString();
            Bind_drp_relatedRequests();
        }

        protected void FillDataFromControls()
        {
            DC_MAIL = maga_DataContext.DC_MAIL;
            if (IdMessage != 0)
                _currTBL = DC_MAIL.MAIL_TBL_MESSAGE.SingleOrDefault(x => x.id == IdMessage);
            if (_currTBL == null)
                return;
            RNT_TBL_REQUEST _request = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == drp_relatedRequests.getSelectedValueInt(0));
            if (_request == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('errore: la richiesta non trovata!');", true);
                return;
            }
            _currTBL.pid_request = _request.id;
            _currTBL.pid_request_state = rntUtils.rntRequest_addState(_request.id, 3, UserAuthentication.CurrentUserID, "Ricevuta Mail dal Cliente", txt_body.Text);
            DC_MAIL.SubmitChanges();
            _request.mail_in = 1;
            DC_RENTAL.SubmitChanges();
            if (_request.pid_operator != 0 && UserAuthentication.CurrentUserID != _request.pid_operator)
            {
                string _mSubject = "Ricevuta Mail dal Cliente alla Richiesta rif." + _request.id + " | email: " + _request.email;
                string _mailSend = "";
                if (MailingUtilities.autoSendMailTo(_mSubject, txt_body.Text, AdminUtilities.usr_adminEmail(_request.pid_operator.objToInt32(), ""), false, "admin_rnt_request_contact_from_mail notifica al account"))
                    _mailSend = "Inviato Nuova Nota del Cliente a " + AdminUtilities.usr_adminName(_request.pid_operator.objToInt32(), "") + " (" + AdminUtilities.usr_adminEmail(_request.pid_operator.objToInt32(), "") + ")";
                else
                    _mailSend = "Errore nel invio mail della Creazione Nuova Nota per Cliente a " + AdminUtilities.usr_adminName(_request.pid_operator.objToInt32(), "") +" (" + AdminUtilities.usr_adminEmail(_request.pid_operator.objToInt32(), "") + ")";
                rntUtils.rntRequest_addState(_request.id, 0, _request.state_pid_user.Value, _mailSend, "");
            }
            Response.Redirect("rnt_request_details.aspx?id=" + _request.id);
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tinyEditor", "setTinyEditor(true);", true);
        }
        protected void lnk_create_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
    }
}
