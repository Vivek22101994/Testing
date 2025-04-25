using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;
using System.Net;

namespace RentalInRome
{
    public partial class util_paypal_redirect : mainBasePage
    {
        private magaInvoice_DataContext DC_INVOICE;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            bool _isTest = CommonUtilities.getSYS_SETTING("paypal_isTest") == "true";
            string code = Request.QueryString["code"];
            string type = Request.QueryString["type"];
            RemotePost _post = new RemotePost();
            _post.Add("cmd", "_xclick");
            if (CurrentLang.NAME.Length == 5)
                _post.Add("lc", CurrentLang.NAME.Substring(3));
            else
                _post.Add("lc", "EN");

            if (App.WLAgentId > 0)//White Label
            {
                bool _isTestWL = CommonUtilities.getSYS_SETTING("paypal_isTest_WhiteLabel") == "true";
                dbRntAgentWLPaymentRL agentWLPayment = (dbRntAgentWLPaymentRL)null;
                using (DCmodRental dc = new DCmodRental())
                {
                    agentWLPayment = dc.dbRntAgentWLPaymentRLs.FirstOrDefault(x => x.pidAgent == App.WLAgentId && x.paymentType == "paypal");
                }
                if (_isTestWL)
                {
                    if (agentWLPayment != null)
                    {
                        _post.Url = "https://www.sandbox.paypal.com/cgi-bin/webscr";
                        _post.Add("business", agentWLPayment.email);//"NLCRDRHM37SSY");
                    }
                    else
                    {
                        string WL_domainName = "http://" + Request.Url.Host;
                        Response.Redirect(WL_domainName + "/reservationarea/payment.aspx", false);
                    }
                }
                else
                {
                    if (agentWLPayment != null)
                    {
                        _post.Url = "https://www.paypal.com/cgi-bin/webscr";
                        _post.Add("business", agentWLPayment.email);
                    }
                    else
                    {
                        string WL_domainName = "http://" + Request.Url.Host;
                        Response.Redirect(WL_domainName + "/reservationarea/payment.aspx", false);
                    }
                }
            }
            else
            {
                if (_isTest)
                {
                    _post.Url = "https://www.sandbox.paypal.com/cgi-bin/webscr";
                    _post.Add("business", "NLCRDRHM37SSY");
                }
                else
                {
                    _post.Url = "https://www.paypal.com/cgi-bin/webscr";
                    _post.Add("business", "info@rentalinrome.com");
                }
            }

            if (type == "payment")
            {
                int _id = code.ToInt32();
                INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == _id);
                if (_pay == null)
                    return;
                decimal pr_total = _pay.pr_total.objToDecimal() + _pay.chargeFee.objToDecimal();
                if (App.WLAgentId > 0)//White Label
                {
                    _post.Add("item_name", WL.getWLName() + " - Reservation #" + _pay.rnt_reservation_code);
                }
                else
                {
                    _post.Add("item_name", "Rental in Rome - Reservation #" + _pay.rnt_reservation_code);
                }
                _post.Add("item_number", "1");
                _post.Add("amount", ("" + pr_total).Replace(",", "."));
                _post.Add("quantity", "1");
                _post.Add("no_note", "1");
                _post.Add("currency_code", "EUR");

                if (App.WLAgentId > 0)//White Label
                {
                    _post.HeadContent = ltrHeadWL.Text;
                    _post.BodyContent = ltrBodyWL.Text.Replace("#WLLogo#", WL.getWLLogo());
                }
                else if (Request["mobile"] == "true")
                {
                    _post.HeadContent = ltrMobileHead.Text;
                    _post.BodyContent = ltrMobileBody.Text;
                }
                else
                {
                    _post.HeadContent = ltrHead.Text;
                    _post.BodyContent = ltrBody.Text;
                }
                _post.Add("custom", "invpay_" + code);

                //set new TLS protocol 1.1/1.2
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //_post.AutoPost = false;

                _post.Post();
            }
        }
    }
}
