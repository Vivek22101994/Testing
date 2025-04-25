using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class util_paypal_ipn : System.Web.UI.Page
    {
        protected magaInvoice_DataContext DC_INVOICE;
        protected magaPayPal_DataContext DC_PAYPAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            DC_PAYPAL = maga_DataContext.DC_PAYPAL;

            if (!IsPostBack)
            {
                try
                {
                    string txn_type = "" + Request.Form["txn_type"];
                    byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
                    string txn_str = Encoding.ASCII.GetString(param);
                    string _verify = "";
                    bool _isTest = CommonUtilities.getSYS_SETTING("paypal_isTest") == "true";
                    string _url = _isTest ? "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_notify-validate&" : "https://www.paypal.com/cgi-bin/webscr?cmd=_notify-validate&";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url + txn_str);
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";

                    //set new TLS protocol 1.1/1.2
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;  

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode.ToString().ToLower() == "ok")
                    {
                        Stream content = response.GetResponseStream();
                        StreamReader contentReader = new StreamReader(content);
                        _verify = contentReader.ReadToEnd();
                    }
                    PayPal.PP_payment _pp = new PayPal.PP_payment(txn_type, txn_str);
                    PP_TBL_TRANSACTION _txn = new PP_TBL_TRANSACTION();
                    try
                    {
                        _txn.unique_id = Guid.NewGuid();
                        _txn.inv_pid_payment = 0;
                        _txn.date_creation = DateTime.Now;
                        _txn.txn_verify = _verify.ToUpper();
                        _txn.txn_string = txn_str;
                        _txn.txn_id = _pp.txn_id;
                        _txn.txn_type = _pp.txn_type;
                        _txn.txn_business = _pp.txn_business;
                        _txn.txn_charset = _pp.txn_charset;
                        _txn.txn_custom = ("" + _pp.txn_custom).ToLower();
                        _txn.txn_notify_version = _pp.txn_notify_version;
                        _txn.txn_parent_txn_id = _pp.txn_parent_txn_id;
                        _txn.txn_receipt_id = _pp.txn_receipt_id;
                        _txn.txn_receiver_email = _pp.txn_receiver_email;
                        _txn.txn_receiver_id = _pp.txn_receiver_id;
                        _txn.txn_resend = _pp.txn_resend;
                        _txn.txn_residence_country = _pp.txn_residence_country;
                        _txn.txn_verify_sign = _pp.txn_verify_sign;
                        _txn.cl_address_country = _pp.cl_address_country;
                        _txn.cl_address_city = _pp.cl_address_city;
                        _txn.cl_address_country_code = _pp.cl_address_country_code;
                        _txn.cl_address_name = _pp.cl_address_name;
                        _txn.cl_address_state = _pp.cl_address_state;
                        _txn.cl_address_street = _pp.cl_address_street;
                        _txn.cl_address_zip = _pp.cl_address_zip;
                        _txn.cl_contact_phone = _pp.cl_contact_phone;
                        _txn.cl_first_name = _pp.cl_first_name;
                        _txn.cl_last_name = _pp.cl_last_name;
                        _txn.cl_payer_business_name = _pp.cl_payer_business_name;
                        _txn.cl_payer_email = _pp.cl_payer_email;
                        _txn.cl_payer_id = _pp.cl_payer_id;
                        _txn.pay_auth_amount = _pp.pay_auth_amount;
                        _txn.pay_auth_exp = _pp.pay_auth_exp.sqlServerDateCheck();
                        _txn.pay_auth_id = _pp.pay_auth_id;
                        _txn.pay_auth_status = _pp.pay_auth_status;
                        _txn.pay_exchange_rate = _pp.pay_exchange_rate;
                        _txn.pay_invoice = _pp.pay_invoice;
                        _txn.pay_mc_currency = _pp.pay_mc_currency;
                        _txn.pay_mc_fee = _pp.pay_mc_fee;
                        _txn.pay_mc_gross = _pp.pay_mc_gross;
                        _txn.pay_mc_handling = _pp.pay_mc_handling;
                        _txn.pay_mc_shipping = _pp.pay_mc_shipping;
                        _txn.pay_memo = _pp.pay_memo;
                        _txn.pay_num_cart_items = _pp.pay_num_cart_items;
                        _txn.pay_payer_status = _pp.pay_payer_status;
                        _txn.pay_payment_date = _pp.pay_payment_date.sqlServerDateCheck();
                        _txn.pay_payment_gross = _pp.pay_payment_gross;
                        _txn.pay_payment_status = "" + _pp.pay_payment_status;
                        _txn.pay_payment_type = _pp.pay_payment_type;
                        _txn.pay_pending_reason = _pp.pay_pending_reason;
                        _txn.pay_protection_eligibility = _pp.pay_protection_eligibility;
                        _txn.pay_quantity = _pp.pay_quantity;
                        _txn.pay_reason_code = _pp.pay_reason_code;
                        _txn.pay_remaining_settle = _pp.pay_remaining_settle;
                        _txn.pay_settle_amount = _pp.pay_settle_amount;
                        _txn.pay_settle_currency = _pp.pay_settle_currency;
                        _txn.pay_shipping = _pp.pay_shipping;
                        _txn.pay_shipping_method = _pp.pay_shipping_method;
                        _txn.pay_tax = _pp.pay_tax;
                    }
                    catch (Exception ex)
                    {
                        string _ip = "";
                        try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                        catch (Exception ex1) { }
                        ErrorLog.addLog(_ip, "paypal_ipn", ex.ToString());
                    }
                    //_txn. = _pp.;
                    DC_PAYPAL.PP_TBL_TRANSACTIONs.InsertOnSubmit(_txn);
                    DC_PAYPAL.SubmitChanges();
                    if (_txn.txn_verify == "VERIFIED")
                    {
                        if (_txn.txn_custom.StartsWith("invpay_"))
                        {
                            int _id = _txn.txn_custom.Replace("invpay_", "").ToInt32();
                            INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == _id);
                            if (_pay != null && _txn.pay_payment_status.ToUpper() == "COMPLETED" && _pay.pr_total <= _txn.pay_mc_gross)
                            {
                                _pay.pay_pid_txn = _txn.id;
                                if (!_pay.pay_txn_gross.HasValue) _pay.pay_txn_gross = 0;
                                _pay.is_complete = 1;
                                _pay.pay_date = DateTime.Now;
                                _pay.pay_mode = "paypal";
                                _pay.state_pid = 1;
                                _pay.state_date = DateTime.Now;
                                _pay.state_subject = "Completato";
                                _pay.state_pid_user = 1;
                                //_pay.pay_txn_gross = _pay.pr_total; // += _txn.pay_mc_gross;
                                _txn.inv_pid_payment = _pay.id;
                                DC_INVOICE.SubmitChanges();
                                var currRes = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _pay.rnt_pid_reservation);
                                if (currRes != null)
                                    invUtils.payment_onChange(currRes, true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string _ip = "";
                    try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                    catch (Exception ex1) { }
                    string _params = "";
                    foreach (string key in Request.Params.AllKeys)
                        _params += "\n" + key + "=" + Request.Params[key];
                    string errorBody = ex.ToString() + "\n\n______________________\n\nURL: " + Request.Url.AbsoluteUri.ToString() + "\n\n______________________\n\nRequest:\n " + _params;
                    ErrorLog.addLog(_ip, "paypal_ipn_general", errorBody);
                    MailingUtilities.autoSendMailTo("Rir: Errore paypal_ipn_general", errorBody.htmlNoWrap(), "adilet@magadesign.net", false, "Rir: Errore paypal_ipn_general");
                    throw new HttpException("InnerException");
                }
            }
        }
    }
}
