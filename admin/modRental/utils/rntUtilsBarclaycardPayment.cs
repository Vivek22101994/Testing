using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using RentalInRome.data;
using ModInvoice;

public class BarclaycardPaymentUtils
{
    protected static string generateMac(string str)
    {
        string result_mac = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "sha1");
        return result_mac.ToLower();
    }
    public static string POST(String strURL, String strPostData)
    {
        try
        {
            string host = App.HOST;
            HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(strURL);
            obj.ContentType = "application/x-www-form-urlencoded";
            obj.Method = "POST";
            Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strPostData);
            obj.ContentLength = bytes.Length;
            Stream os = obj.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
            Stream os1 = obj1.GetResponseStream();
            StreamReader _Answer = new StreamReader(os1);
            String strRSString = _Answer.ReadToEnd().ToString();
            if (CommonUtilities.getSYS_SETTING("barclaycard_debug") == "true" || CommonUtilities.getSYS_SETTING("barclaycard_debug").ToInt32() == 1)
                ErrorLog.addLog("Debug", "BarclaycardPaymentUtils.SendRequest", "Request: " + strPostData + "Response: " + strRSString + " url: " + strURL);
            return strRSString;

        }
        catch (Exception ex)
        {
            ErrorLog.addLog("", "BarclaycardPaymentUtils.SendRequest", "error: " + ex.ToString());
            return "";
        }
    }
    public static string sendPayment(RNT_TBL_RESERVATION currRes, long paymentId, CreditCard currCredit)
    {
        try
        {
            magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;

            if (currRes == null)
                return "";

            //if payment is null then finding last payment from reservation.
            INV_TBL_PAYMENT currPay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == paymentId);
            if (currPay == null)
            {
                currPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == currRes.id && x.is_complete == 0 && x.direction == 1).FirstOrDefault();
                if (currPay == null) return "Payment Not found";
            }

            string url = "https://e-payment.postfinance.ch/ncol/prod/orderdirect.asp";
            string amount = Math.Round((currPay.pr_total * 100).objToDecimal()) + "";
            string cardno = currCredit.cardNumber;
            string cn = currRes.cl_name_full;
            string currency = "EUR";
            string cvc = currCredit.cvc;
            string ed = currCredit.ed;
            string email = currRes.cl_email;
            string operation = CommonUtilities.getSYS_SETTING("barclaycard_operation");
            string orderid = "invpay_" + currPay.code;
            string pspid = CommonUtilities.getSYS_SETTING("barclaycard_pspid");
            string password = CommonUtilities.getSYS_SETTING("barclaycard_pswd");
            string userid = CommonUtilities.getSYS_SETTING("barclaycard_userid");
            string phrase = CommonUtilities.getSYS_SETTING("barclaycard_phrase");
            string eci = CommonUtilities.getSYS_SETTING("barclaycard_eci");

            if (CommonUtilities.getSYS_SETTING("is_barclaycard_test") == "true")
            {
                url = "https://e-payment.postfinance.ch/ncol/test/orderdirect.asp";
            }

            if (CommonUtilities.getSYS_SETTING("is_barclaycard_amount_test") == "true")
            {
                amount = "10";
            }

            string strshasign = "AMOUNT=" + amount + phrase + "CARDNO=" + cardno + phrase + "CN=" + cn + phrase + "CURRENCY=" + currency + phrase + "CVC=" + cvc + phrase + "ECI=" + eci + phrase + "ED=" + ed + phrase + "EMAIL=" + email + phrase + "OPERATION=" + operation + phrase + "ORDERID=" + orderid + phrase + "PSPID=" + pspid + phrase + "PSWD=" + password + phrase + "USERID=" + userid + phrase;
            string shasign = generateMac(strshasign);
            string strdata = "PSPID=" + pspid + "&ORDERID=" + orderid + "&USERID=" + userid + "&PSWD=" + password + "&AMOUNT=" + amount + "&CURRENCY=" + currency + "&CARDNO=" + cardno + "&ED=" + ed + "&CN=" + cn + "&EMAIL=" + email + "&ECI=" + eci + "&CVC=" + cvc + "&OPERATION=" + operation + "&SHASIGN=" + shasign;
            string response = POST(url, strdata);
            //response= "<?xml version='1.0'?><ncresponse orderID='invpay_0000512' PAYID='42350001' NCERROR='0' STATUS='9'></ncresponse>";
            System.Xml.Linq.XDocument xdoc = new System.Xml.Linq.XDocument();
            try
            {
                xdoc = System.Xml.Linq.XDocument.Parse(response);
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "BarclaycardPaymentUtils.sendPayment", response + " " + ex.ToString());
                return "";
            }

            string orderId = "";
            string paydId = "";
            string errorCode = "";
            string errorMessage = "";
            string status = "";
            string acceptance = "";
            if (xdoc.Element("ncresponse") != null)
            {
                if (xdoc.Element("ncresponse").Attribute("orderID") != null)
                    orderId = xdoc.Element("ncresponse").Attribute("orderID").Value;
                if (xdoc.Element("ncresponse").Attribute("PAYID") != null)
                    paydId = xdoc.Element("ncresponse").Attribute("PAYID").Value;
                if (xdoc.Element("ncresponse").Attribute("NCERROR") != null)
                    errorCode = xdoc.Element("ncresponse").Attribute("NCERROR").Value;
                if (xdoc.Element("ncresponse").Attribute("NCERRORPLUS") != null)
                    errorMessage = xdoc.Element("ncresponse").Attribute("NCERRORPLUS").Value;
                if (xdoc.Element("ncresponse").Attribute("STATUS") != null)
                    status = xdoc.Element("ncresponse").Attribute("STATUS").Value;
                if (xdoc.Element("ncresponse").Attribute("ACCEPTANCE") != null)
                    acceptance = xdoc.Element("ncresponse").Attribute("ACCEPTANCE").Value;
            }
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                if ((operation == "SAL" && (status == "9" || status == "92")) || (operation == "RES" && status == "5"))
                {
                    if (orderId != "" && orderId.Replace("invpay_", "").objToInt32() != currPay.id)
                    {
                        ErrorLog.addLog("", "BarclaycardPaymentUtils.sendPayment", "Response OrderId " + orderId + " is not similar to payment Id " + currPay.id);
                        return "";
                    }

                    if (!currPay.pay_txn_gross.HasValue) currPay.pay_txn_gross = 0;
                    currPay.is_complete = 1;
                    currPay.pay_date = DateTime.Now;
                    currPay.pay_mode = "barclaycard";
                    currPay.state_pid = 1;
                    currPay.state_date = DateTime.Now;
                    currPay.state_subject = "Completato";
                    currPay.state_pid_user = 1;
                    currPay.barClayPayId = paydId;
                    currPay.barClayStatus = status.objToInt32();
                    currPay.barClayError = errorCode + "-" + errorMessage;
                    currPay.barClayAcceptance = acceptance;
                    if (status == "92")
                        currPay.barClayPayUncertain = 1;
                    else
                        currPay.barClayPayUncertain = 0;
                    DC_INVOICE.SubmitChanges();
                    invUtils.payment_onChange(currRes, true);
                    rntUtils.rntReservation_onChange(currRes);
                    return "success";
                }
                else
                {
                    ErrorLog.addLog("", "BarclaycardPaymentUtils.sendPayment", errorCode + "-" + errorMessage);
                    return errorMessage;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLog.addLog("", "BarclaycardPaymentUtils.sendPayment", ex.ToString());
            return "";
        }
    }
}
public class CreditCard
{
    public string cardNumber { get; set; }
    public string cvc { get; set; }
    public string ed { get; set; }
    public string brand { get; set; }
}