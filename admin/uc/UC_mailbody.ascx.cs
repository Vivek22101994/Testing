using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_mailbody : System.Web.UI.UserControl
    {
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
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
            }
        }
        protected magaMail_DataContext DC_MAIL;
        protected magaRental_DataContext DC_RENTAL;
        protected MAIL_TBL_MESSAGE _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_MAIL = maga_DataContext.DC_MAIL;
            DC_RENTAL = maga_DataContext.DC_RENTAL;
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
            ltr_body_text.Text = (ltr_body_html_text.Text != "") ? ltr_body_html_text.Text : ltr_body_plain_text.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            HF_email.Value = _currTBL.from_email;
            HF_pid_request.Value = _currTBL.pid_request.ToString();
            HF_pid_request_state.Value = _currTBL.pid_request_state.ToString();
            pnl_new_request.Visible = true;
            pnl_new_contact.Visible = true;
            if (HF_pid_request.Value != "0")
            {
                pnl_new_request.Visible = false;
                pnl_new_contact.Visible = false;
                lbl_title.InnerHtml = HF_pid_request_state.Value == "0" ? "Creata la richiesta rif. " + HF_pid_request.Value : "Creata Nota per il cliente alla richiesta rif. " + HF_pid_request.Value;
            }
            else
            {
                RNT_TBL_REQUEST _oldRequest = DC_RENTAL.RNT_TBL_REQUEST.FirstOrDefault(x => x.email == HF_email.Value && x.pid_related_request == 0);
                if(_oldRequest==null)
                    pnl_new_contact.Visible = false;
            }
        }
    }
}