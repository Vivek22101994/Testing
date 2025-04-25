using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class prova : mainBasePage
    {
        protected magaInvoice_DataContext DC_INVOICE;
        protected magaPayPal_DataContext DC_PAYPAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            DC_PAYPAL = maga_DataContext.DC_PAYPAL;

            if (!IsPostBack)
            {
                string txn_type = "" + Request.Form["txn_type"];
                byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
                string txn_str = "mc_gross=316.19&protection_eligibility=Ineligible&address_status=unconfirmed&payer_id=QZCTJBJDTTLSL&tax=0.00&address_street=3/8A Wylde Street&payment_date=20:19:25 Aug 28, 2011 PDT&payment_status=Completed&charset=windows-1252&address_zip=2011&first_name=Susan&mc_fee=10.47&address_country_code=AU&address_name=Susan Skinner&notify_version=3.2&custom=invpay_2129&payer_status=unverified&business=rentalinrome@yahoo.it&address_country=Australia&address_city=Potts Point&quantity=1&verify_sign=AiPC9BjkCyDFQXbSkoZcgqH3hpacAEhvHLme-Fjehve--XpJ8GZ5WtVv&payer_email=srachelskinner@gmail.com&txn_id=1G562956LS602350E&payment_type=instant&last_name=Skinner&address_state=New South Wales&receiver_email=rentalinrome@yahoo.it&payment_fee=&receiver_id=MS7SYAD8C3PRY&txn_type=web_accept&item_name=Rental in Rome - Reservation #0151366&mc_currency=EUR&item_number=1&residence_country=AU&receipt_id=0952-4003-8288-6448&handling_amount=0.00&transaction_subject=invpay_2129&payment_gross=&shipping=0.00&ipn_track_id=Huurnr1Zv9V.x5vy-BUqOQ";
                string _verify = "";
                //HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_notify-validate&" + txn_str);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.paypal.com/cgi-bin/webscr?cmd=_notify-validate&" + txn_str);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode.ToString().ToLower() == "ok")
                {
                    Stream content = response.GetResponseStream();
                    StreamReader contentReader = new StreamReader(content);
                    _verify = contentReader.ReadToEnd();
                }


                //if (!File.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "prove-paypal.txt")))
                //    File.Create(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "prove-paypal.txt"));
                StreamWriter _writer = new StreamWriter(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "prove-paypal.txt"), true);
                _writer.WriteLine(DateTime.Now + " - " + _verify + " - " + txn_str);
                _writer.Close();
                PayPal.PP_payment _pp = new PayPal.PP_payment(txn_type, txn_str);
                PP_TBL_TRANSACTION _txn = new PP_TBL_TRANSACTION();
                _txn.unique_id = Guid.NewGuid();
                _txn.inv_pid_payment = 0;
                _txn.date_creation = DateTime.Now;
                _txn.txn_verify = _verify.ToUpper();
                _txn.txn_string = txn_str;
                _txn.txn_id = _pp.txn_id;
                _txn.txn_type = _pp.txn_type;
                _txn.txn_business = _pp.txn_business;
                _txn.txn_charset = _pp.txn_charset;
                _txn.txn_custom = "" + _pp.txn_custom.ToLower();
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
                //_txn. = _pp.;
                DC_PAYPAL.PP_TBL_TRANSACTIONs.InsertOnSubmit(_txn);
                DC_PAYPAL.SubmitChanges();
                if (_txn.txn_verify == "VERIFIED")
                {
                    if (_txn.txn_custom.StartsWith("invpay_"))
                    {
                        int _id = _txn.txn_custom.Replace("invpay_", "").ToInt32();
                        INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == _id);
                        if (_pay != null && _txn.pay_payment_status.ToUpper() == "COMPLETED")
                        {
                            _pay.pay_pid_txn = _txn.id;
                            if (!_pay.pay_txn_gross.HasValue) _pay.pay_txn_gross = 0;
                            _pay.pay_txn_gross = _pay.pr_total; // += _txn.pay_mc_gross;
                            _pay.pay_mode = "paypal";
                            if (_pay.pr_total <= _pay.pay_txn_gross)
                            {
                                _pay.pay_date = DateTime.Now;
                                _pay.is_complete = 1;
                                if (_pay.rnt_pid_reservation.HasValue)
                                {
                                    magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
                                    RNT_TBL_RESERVATION _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _pay.rnt_pid_reservation.Value);
                                    if (_currTBL != null)
                                    {
                                        rntUtils.rntReservation_mailPartPaymentReceive(_currTBL, true, true, true, true, true, 1); // send mails
                                        _currTBL.state_pid = 4;
                                        _currTBL.state_date = DateTime.Now;
                                        _currTBL.state_subject = "Prenotato";
                                        _currTBL.state_pid_user = 1;
                                        DC_RENTAL.SubmitChanges();
                                        rntUtils.rntReservation_onChange(_currTBL);
                                    }

                                }

                            }
                            _txn.inv_pid_payment = _pay.id;
                            DC_INVOICE.SubmitChanges();
                        }
                    }
                }
            }
        }
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    base.PAGE_TYPE = "static_page";
        //    int id;
        //    if (!string.IsNullOrEmpty(Request.QueryString["id"]) && int.TryParse(Request.QueryString["id"], out id))
        //        base.PAGE_REF_ID = id;
        //    else
        //        base.PAGE_REF_ID = 1;
        //}
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        Fill_data();
        //    }
        //}

        //protected void Fill_data()
        //{
        //    CONT_VIEW_TBL_PAGE _stp =
        //        maga_DataContext.DC_CONTENT.CONT_VIEW_TBL_PAGEs.SingleOrDefault(item => item.id == PAGE_REF_ID && item.pid_lang == CurrentLang.ID);
        //    if (_stp != null)
        //    {
        //        ltr_title.Text = _stp.title;
        //        ltr_sub_title.Text = _stp.sub_title;
        //        ltr_description.Text = _stp.description;
        //    }
        //}
    }
}
