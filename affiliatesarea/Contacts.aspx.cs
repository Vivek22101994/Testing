using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;

namespace RentalInRome.affiliatesarea
{
    public partial class Contacts : agentBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (agentAuth.CurrentID != 0)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentAuth.CurrentID);
                        if (agentTBL != null)
                        {
                            ltr_nameCompany.Text = agentTBL.nameCompany;
                            ltr_nameFull.Text = agentTBL.nameFull;
                            ltr_email.Text = agentTBL.contactEmail;
                            ltr_phone.Text = agentTBL.contactPhone;
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setToolTip", "setToolTip();", true);
        }
        protected void lnkAccept_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntAgentTBL currTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentAuth.CurrentID);
                if (currTBL == null) return;
                currTBL.hasAcceptedContract = 1;
                dc.SaveChanges();
                agentAuth.hasAcceptedContract = 1;
                Response.Redirect("/affiliatesarea/");
            }
        }
        protected void lnk_send_Click(object sender, EventArgs e)
        {
            saveRequest();
            pnl_request_sent.Visible = true;
            pnl_request_cont.Visible = false;
        }

        protected void saveRequest()
        {
            string _br = "";
            bool alternateOld = true;
            string _mailBody = "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
            string _row = "";
            _row += "<tr>";
            _row += "<td><strong>Lingua</strong></td>";
            _row += "<td>" + CurrentLang.TITLE + "</td>";
            _row += "</tr>";
            _mailBody += _row;
            _mailBody += MailingUtilities.addMailRow("Agenzia: ", ltr_nameCompany.Text, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Nome e Cognome: ", ltr_nameFull.Text, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Email", "" + ltr_email.Text, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Telefono", "" + ltr_phone.Text, alternateOld, out alternateOld, false, false, false);
            _mailBody += MailingUtilities.addMailRow("Richiesta:", "" + txt_note.Value.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"), alternateOld, out alternateOld, false, false, false);
            _mailBody += "</table>";
            MailingUtilities.autoSendMailTo("Richiesta dall'area agenzie: " + ltr_email.Text, _mailBody, "agencies@rentalinrome.com", false, "affiliati/contacts alladmin");
        }

    }
}
