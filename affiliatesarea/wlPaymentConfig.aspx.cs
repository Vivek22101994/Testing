using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.affiliatesarea
{
    public partial class wlPaymentConfig : agentBasePage
    {
        protected dbRntAgentWLPaymentRL currTBL;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                FillControls();
            }
        }

        protected void FillControls()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntAgentWLPaymentRLs.FirstOrDefault(x => x.pidAgent == agentAuth.CurrentID);
            }
            if (currTBL != null)
            {
                txt_email.Text = currTBL.email;
            }
         
        }

        protected void FillDataFromControls()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntAgentWLPaymentRLs.FirstOrDefault(x => x.pidAgent == agentAuth.CurrentID);
                if (currTBL == null)
                {
                    currTBL = new dbRntAgentWLPaymentRL();
                    currTBL.pidAgent = agentAuth.CurrentID;
                    currTBL.paymentType = "paypal"; //Here paymentType is code , To display payment title anywhere use table INV_LK_PAYMENT_MODE
                    dc.Add(currTBL);
                }
                currTBL.email = txt_email.Text;
                dc.SaveChanges();
                rntProps.AgentWLPaymentRL = dc.dbRntAgentWLPaymentRLs.ToList();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"" + contUtils.getLabel("lblSavedData") + ".\", 340, 110);", true);                
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            FillControls();
        }
    }
}