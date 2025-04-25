using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

public class PayPal
{
    public static DateTime ConvertPayPalDateTime(string payPalDateTime)
    {
        // accept a few different date formats because of PST/PDT timezone and slight month difference in sandbox vs. prod.
        string[] dateFormats = { "HH:mm:ss MMM dd, yyyy PST", "HH:mm:ss MMM. dd, yyyy PST", "HH:mm:ss MMM dd, yyyy PDT", "HH:mm:ss MMM. dd, yyyy PDT" };
        DateTime outputDateTime;

        DateTime.TryParseExact(payPalDateTime, dateFormats, new CultureInfo("en-US"), DateTimeStyles.None, out outputDateTime);

        // convert to local timezone
        outputDateTime = outputDateTime.AddHours(3);

        return outputDateTime;
    }
    public class PP_payment
    {
        public string txn_type;
        // The kind of transaction for which the IPN message was sent.
        public string txn_id;
        // The merchant’s original transaction identification number for the payment from the buyer, against which the case was registered.
        public string txn_business;
        // Email address or account ID of the payment recipient (that is, the merchant). Equivalent to the values of receiver_email (if payment is sent to primary account) and business set in the Website Payment HTML. Length: 127 characters (Lowercase)
        public string txn_charset;
        // Character set
        public string txn_custom;
        // Custom value as passed by you, the merchant. These are pass-through variables that are never presented to your customer. Length: 255 characters
        public string txn_notify_version;
        // Message’s version number
        public string txn_parent_txn_id;
        // In the case of a refund, reversal, or canceled reversal, this variable contains the txn_id of the original transaction, while txn_id contains a new ID for the new transaction. Length: 19 characters
        public string txn_receipt_id;
        // Unique ID generated during guest checkout (payment by credit card without logging in).
        public string txn_receiver_email;
        // Primary email address of the payment recipient (that is, the merchant). Length: 127 characters (Lowercase)
        public string txn_receiver_id;
        // Unique account ID of the payment recipient (i.e., the merchant). This is the same as the recipient's referral ID. Length: 13 characters
        public bool txn_resend;
        // Whether this IPN message was resent (equals true); otherwise, this is the original message.
        public string txn_residence_country;
        // ISO 3166 country code associated with the country of residence. Length: 2 characters
        public bool txn_test_ipn;
        // Whether the message is a test message. It is one of the following values: 􀁺 1 – the message is directed to the Sandbox
        public string txn_verify_sign;
        // Encrypted string used to validate the authenticity of the transaction
        public string cl_address_country;
        // Country of customer’s address. Length: 64 characters
        public string cl_address_city;
        // City of customer’s address. L: 40 
        public string cl_address_country_code;
        // ISO 3166 country code associated with customer’s address. L: 2
        public string cl_address_name;
        // Name used with address (included when the customer provides a Gift Address). L: 128
        public string cl_address_state;
        // State of customer’s address. L: 40
        public bool cl_address_status;
        // Whether the customer provided a confirmed address. It is one of the following values: 􀁺 confirmed – Customer provided a confirmed address. 􀁺 unconfirmed – Customer provided an unconfirmed address.
        public string cl_address_street;
        // Customer’s street address. L: 200
        public string cl_address_zip;
        // Zip code of customer’s address. L: 20
        public string cl_contact_phone;
        // Customer’s telephone number. L: 20
        public string cl_first_name;
        // Customer’s first name. L: 64
        public string cl_last_name;
        // Customer’s last name. L: 64
        public string cl_payer_business_name;
        // Customer’s company name, if customer is a business. L: 127
        public string cl_payer_email;
        // Customer’s primary email address. Use this email to provide any credits. L: 127
        public string cl_payer_id;
        // Unique customer ID. L: 13
        public decimal pay_auth_amount;
        // Authorization amount
        public DateTime pay_auth_exp;
        // Authorization expiration date and time, in the following format:HH:MM:SS DD Mmm YY, YYYY PST. L: 28
        public string pay_auth_id;
        // Authorization identification number. L: 19
        public string pay_auth_status;
        // Status of authorization
        public decimal pay_exchange_rate;
        // Exchange rate used if a currency conversion occurred.
        public string pay_invoice;
        // Passthrough variable you can use to identify your Invoice Number for this purchase. If omitted, no variable is passed back. L: 127
        public decimal pay_mc_currency;
        //􀁺 For payment IPN notifications, this is the currency of the payment.
        //􀁺 For non-payment subscription IPN notifications (i.e., txn_type=
        //signup, cancel, failed, eot, or modify), this is the currency of the
        //subscription.
        //􀁺 For payment subscription IPN notifications, it is the currency of the
        //payment (i.e., txn_type = subscr_payment)
        public decimal pay_mc_fee;
        //Transaction fee associated with the payment. mc_gross minus mc_fee
        //equals the amount deposited into the receiver_email account.
        //Equivalent to payment_fee for USD payments. If this amount is
        //negative, it signifies a refund or reversal, and either of those payment
        //statuses can be for the full or partial amount of the original transaction fee.
        public decimal pay_mc_gross;
        //Full amount of the customer's payment, before transaction fee is
        //subtracted. Equivalent to payment_gross for USD payments. If this
        //amount is negative, it signifies a refund or reversal, and either of those
        //payment statuses can be for the full or partial amount of the original
        //transaction.
        public decimal pay_mc_handling;
        // Total handling amount associated with the transaction.
        public decimal pay_mc_shipping;
        // Total shipping amount associated with the transaction.
        public string pay_memo;
        // Memo as entered by your customer in PayPal Website Payments note field. Length: 255 characters
        public int pay_num_cart_items;
        // If this is a PayPal Shopping Cart transaction, number of items in cart.
        public string pay_payer_status;
        //Whether the customer has a verified PayPal account.
        //􀁺 verified – Customer has a verified PayPal account.
        //􀁺 unverified – Customer has an unverified PayPal account.
        public DateTime pay_payment_date;
        //Time/Date stamp generated by PayPal, in the following format:
        //HH:MM:SS DD Mmm YY, YYYY PST
        //Length: 28 characters
        //public string pay_payment_fee;
        //USD transaction fee associated with the payment. payment_gross
        //minus payment_fee equals the amount deposited into the receiver
        //email account. Is empty for non-USD payments. If this amount is
        //negative, it signifies a refund or reversal, and either of those payment
        //statuses can be for the full or partial amount of the original transaction fee.
        //NOTE: This is a deprecated field. Use mc_fee instead.
        public decimal pay_payment_gross;
        //Full USD amount of the customer’s payment, before transaction fee is
        //subtracted. Will be empty for non-USD payments. This is a legacy field
        //replaced by mc_gross. If this amount is negative, it signifies a refund or
        //reversal, and either of those payment statuses can be for the full or partial
        //amount of the original transaction.
        public string pay_payment_status;
        //The status of the payment:
        //Canceled_Reversal: A reversal has been canceled. For example, you
        //won a dispute with the customer, and the funds for the transaction that was
        //reversed have been returned to you.
        //Completed: The payment has been completed, and the funds have been
        //added successfully to your account balance.
        //Created: A German ELV payment is made using Express Checkout.
        //Denied: You denied the payment. This happens only if the payment was
        //previously pending because of possible reasons described for the
        //pending_reason variable or the Fraud_Management_Filters_x
        //variable.
        //Expired: This authorization has expired and cannot be captured.
        //Failed: The payment has failed. This happens only if the payment was
        //made from your customer’s bank account.
        //Pending: The payment is pending. See pending_reason for more
        //information.
        //Refunded: You refunded the payment.
        //Reversed: A payment was reversed due to a chargeback or other type of
        //reversal. The funds have been removed from your account balance and
        //returned to the buyer. The reason for the reversal is specified in the
        //ReasonCode element.
        //Processed: A payment has been accepted.
        //Voided: This authorization has been voided.
        public string pay_payment_type;
        //echeck: this payment was funded with an echeck.
        //instant: this payment was funded with paypal balance, credit card, or
        //instant transfer.
        public string pay_pending_reason;
        //This variable is set only if payment_status = Pending.
        //address: The payment is pending because your customer did not include
        //a confirmed shipping address and your Payment Receiving Preferences is
        //set yo allow you to manually accept or deny each of these payments. To
        //change your preference, go to the Preferences section of your Profile.
        //authorization: You set the payment action to Authorization and have
        //not yet captured funds.
        //echeck: The payment is pending because it was made by an eCheck that
        //has not yet cleared.
        //intl: The payment is pending because you hold a non-U.S. account and
        //do not have a withdrawal mechanism. You must manually accept or deny
        //this payment from your Account Overview.
        //multi-currency: You do not have a balance in the currency sent, and
        //you do not have your Payment Receiving Preferences set to
        //automatically convert and accept this payment. You must manually accept
        //or deny this payment.
        //order: You set the payment action to Order and have not yet captured
        //funds.
        //paymentreview: The payment is pending while it is being reviewed by
        //PayPal for risk.
        //unilateral: The payment is pending because it was made to an email
        //address that is not yet registered or confirmed.
        //upgrade: The payment is pending because it was made via credit card
        //and you must upgrade your account to Business or Premier status in order
        //to receive the funds. upgrade can also mean that you have reached the
        //monthly limit for transactions on your account.
        //verify: The payment is pending because you are not yet verified. You
        //must verify your account before you can accept this payment.
        //other: The payment is pending for a reason other than those listed above.
        //For more information, contact PayPal Customer Service.
        public string pay_protection_eligibility;
        //ExpandedSellerProtection: Seller is protected by Expanded seller
        //protection
        //SellerProtection: Seller is protected by PayPal’s Seller Protection
        //Policy
        //None: Seller is not protected under Expanded seller protection nor the
        //Seller Protection Policy
        public int pay_quantity;
        //Quantity as entered by your customer or as passed by you, the merchant.
        //If this is a shopping cart transaction, PayPal appends the number of the
        //item (e.g. quantity1, quantity2).
        public string pay_reason_code;
        //This variable is set if payment_status =Reversed, Refunded, or
        //Canceled_Reversal.
        //adjustment_reversal: Reversal of an adjustment
        //buyer-complaint: A reversal has occurred on this transaction due to a
        //complaint about the transaction from your customer.
        //chargeback: A reversal has occurred on this transaction due to a
        //chargeback by your customer.
        //chargeback_reimbursement: Reimbursement for a chargeback
        //chargeback_settlement: Settlement of a chargeback
        //guarantee: A reversal has occurred on this transaction due to your
        //customer triggering a money-back guarantee.
        //other: Non-specified reason.
        //refund: A reversal has occurred on this transaction because you have
        //given the customer a refund.
        //NOTE: Additional codes may be returned.
        public decimal pay_remaining_settle;
        //Remaining amount that can be captured with Authorization and Capture
        public decimal pay_settle_amount;
        //Amount that is deposited into the account’s primary balance after a
        //currency conversion from automatic conversion (through your Payment
        //Receiving Preferences) or manual conversion (through manually
        //accepting a payment).
        public decimal pay_settle_currency;
        //Currency of settle_amount.
        public decimal pay_shipping;
        //Shipping charges associated with this transaction.
        //Format: unsigned, no currency symbol, two decimal places.
        public string pay_shipping_method;
        //The name of a shipping method from the Shipping Calculations section of
        //the merchant's account profile. The buyer selected the named shipping
        //method for this transaction.
        public decimal pay_tax;
        //Amount of tax charged on payment. PayPal appends the number of the
        //item (e.g., item_name1, item_name2). The taxx variable is included
        //only if there was a specific tax amount applied to a particular shopping
        //cart item. Because total tax may apply to other items in the cart, the sum
        //of taxx might not total to tax.
        public string pay_transaction_entity;
        //Authorization and Capture transaction entity
        public List<KeyValuePair<string, string>> payList_fraud_managment_pending_filters_;
        //"fraud_managment_pending_filters_x"
        //One or more filters that identify a triggering action associated with one of
        //the following payment_status values: Pending, Completed, Denied,
        //where x is a number starting with 1 that makes the IPN variable name
        //unique; x is not the filter’s ID number. The filters and their ID numbers
        //are as follows:
        public List<KeyValuePair<string, string>> payList_item_name;
        //Item name as passed by you, the merchant. Or, if not passed by you, as
        //entered by your customer. If this is a shopping cart transaction, PayPal
        //will append the number of the item (e.g., item_name1, item_name2,
        //and so forth).
        //Length: 127 characters
        public List<KeyValuePair<string, string>> payList_item_number;
        //Pass-through variable for you to track purchases. It will get passed back to
        //you at the completion of the payment. If omitted, no variable will be
        //passed back to you. If this is a shopping cart transaction, PayPal will
        //append the number of the item (e.g., item_number1, item_number2,
        //and so forth)
        //Length: 127 characters
        public List<KeyValuePair<string, string>> payList_mc_gross_;
        //The amount is in the currency of mc_currency, where x is the shopping
        //cart detail item number. The sum of mc_gross_x should total
        //mc_gross.
        public List<KeyValuePair<string, string>> payList_mc_shipping;
        //This is the combined total of shipping1 and shipping2 Website
        //Payments Standard variables, where x is the shopping cart detail item
        //number. The shippingx variable is only shown when the merchant
        //applies a shipping amount for a specific item. Because profile shipping
        //might apply, the sum of shippingx might not be equal to shipping.
        public List<KeyValuePair<string, string>> payList_option_name;
        //Option 1 name as requested by you. PayPal appends the number of the
        //item where x represents the number of the shopping cart detail item (e.g.,
        //option_name1, option_name2).
        //Length: 64 characters
        public List<KeyValuePair<string, string>> payList_option_selection;
        //Option 1 choice as entered by your customer.
        //PayPal appends the number of the item where x represents the number of
        //the shopping cart detail item (e.g., option_selection1,
        //option_selection2).
        //Length: 200 characters
        public List<KeyValuePair<string, string>> payList_payment_fee_;
        //If the payment is USD, then the value is the same as that for mc_fee_x,
        //where x is the record number; if the currency is not USD, then this is an
        //empty string.
        //NOTE: This is a deprecated field. Use mc_fee_x instead.
        public List<KeyValuePair<string, string>> payList_payment_gross_;
        //If the payment is USD, then the value for this is the same as that for the
        //mc_gross_x, where x is the record number the mass pay item. If the
        //currency is not USD, this is an empty string.
        //NOTE: This is a deprecated field. Use mc_gross_x instead.
        public List<KeyValuePair<string, string>> payList_quantity;
        //Quantity as entered by your customer or as passed by you, the merchant.
        //If this is a shopping cart transaction, PayPal appends the number of the
        //item (e.g. quantity1, quantity2).


        /*      
        auc_
        Auction Variables     
        Auction information identifies the auction for which a payment is made and additional
        information about the auction.
        public string auc_auction_buyer_id;
            //The customer’s auction ID.
            //Length: 64 characters
        public string auc_auction_closing_date;
            //The auction’s close date, in the following format: HH:MM:SS DD Mmm
            //YY, YYYY PST
            //Length: 28 characters
        public string auc_auction_multi_item;
            //The number of items purchased in multi-item auction payments. It allows
            //you to count the mc_gross or payment_gross for the first IPN you
            //receive from a multi-item auction (auction_multi_item), since each
            //item from the auction will generate an Instant Payment Notification
            //showing the amount for the entire auction.
        public string auc_for_auction;
            //This is an auction payment—payments made using Pay for eBay Items or
            //Smart Logos—as well as Send Money/Money Request payments with the
            //type eBay items or Auction Goods (non-eBay).
         */



        /*     
        mpay_ , mpayList_
        Mass Pay Variables
        Mass pay information identifies the amounts and status of transactions related to mass
        payments, including fees.
        public List<KeyValuePair<string ,string>> mpayList_masspay_txn_id_;
            //For Mass Payments, a unique transaction ID generated by the PayPal
            //system, where x is the record number of the mass pay item
            //Length: 19 characters
        public List<KeyValuePair<string ,string>> mpayList_mc_currency_;
            //For Mass Payments, the currency of the amount and fee, where x is the record number the mass pay item
        public List<KeyValuePair<string ,string>> mpayList_mc_fee_;
            //For Mass Payments, the transaction fee associated with the payment, where x is the record number the mass pay item
        public List<KeyValuePair<string ,string>> mpayList_mc_gross_;
            //The gross amount for the amount, where x is the record number the mass pay item
        public List<KeyValuePair<string ,string>> mpayList_mc_handling;
            //The x is the shopping cart detail item number. The handling_cart cartwide
            //Website Payments variable is also included in the mc_handling
            //variable; for this reason, the sum of mc_handlingx might not be equal to
            //mc_handling
         */
        //public List<KeyValuePair<string ,string>> mpayList_;
        //public List<KeyValuePair<string ,string>> mpayList_;
        //public List<KeyValuePair<string ,string>> mpayList_;


        //public string mpay_;
        //public string mpay_;
        //public string mpay_;
        //public string mpay_;
        //public string mpay_;
        /*
         txn_type
         ** IPN Transaction Types **
         
         —
         * Credit card chargeback if the case_type variable contains chargeback
         
         adjustment
         * A dispute has been resolved and closed
         
         cart
         * Payment received for multiple items; source is Express Checkout or the PayPal Shopping Cart.
         
         express_checkout
         * Payment received for a single item; source is Express Checkout
         
         masspay
         * Payment sent using MassPay
         
         merch_pmt
         * Monthly subscription paid for Website Payments Pro
         
         new_case
         * A new dispute was filed
         
         pro_hosted
         * PayPal Pro
         
         recurring_payment
         * Recurring payment received
         
         recurring_payment_profile_created
         * Recurring payment profile created
         
         send_money
         * Payment received; source is the Send Money tab on the PayPal website
         
         subscr_cancel
         * Subscription canceled
         
         subscr_eot 
         * Subscription expired
         
         subscr_failed 
         * Subscription signup failed
         
         subscr_modify 
         * Subscription modified
         
         subscr_payment 
         * Subscription payment received
         
         subscr_signup 
         * Subscription started
         
         virtual_terminal 
         * Payment received; source is Virtual Terminal
         
         web_accept 
         * Payment received; source is a Buy Now, Donation, or Auction Smart Logos button
         
         */
        public PP_payment(string _txn_type, string values)
        {
            txn_type = _txn_type;
            string[] _strArr = values.Split('&');

            payList_item_name = new List<KeyValuePair<string, string>>();
            payList_item_number = new List<KeyValuePair<string, string>>();
            payList_mc_gross_ = new List<KeyValuePair<string, string>>();
            payList_mc_shipping = new List<KeyValuePair<string, string>>();
            payList_option_name = new List<KeyValuePair<string, string>>();
            payList_option_selection = new List<KeyValuePair<string, string>>();
            payList_payment_fee_ = new List<KeyValuePair<string, string>>();
            payList_payment_gross_ = new List<KeyValuePair<string, string>>();
            payList_quantity = new List<KeyValuePair<string, string>>();

            /*
            mpayList_masspay_txn_id_ = new List<KeyValuePair<string, string>>();
            mpayList_mc_currency_ = new List<KeyValuePair<string, string>>();
            mpayList_mc_fee_ = new List<KeyValuePair<string, string>>();
            mpayList_mc_gross_ = new List<KeyValuePair<string, string>>();
            mpayList_mc_handling = new List<KeyValuePair<string, string>>();
            */

            for (int i = 0; i < _strArr.Length; i++)
            {
                string[] _pair = _strArr[i].Split('=');
                if (_pair.Length != 2) continue;
                string _var = _pair[0];
                string _value = _pair[1];
                /*
                if(txn_type=="masspay")
                {
                    if (_var.StartsWith("masspay_txn_id_")){ mpayList_masspay_txn_id_.Add(new KeyValuePair<string, string>( _var.Replace("masspay_txn_id_",""),_value.urlDecode()));continue;}
                    if (_var.StartsWith("mc_currency_")){ mpayList_mc_currency_.Add(new KeyValuePair<string, string>( _var.Replace("mc_currency_",""),_value.urlDecode()));continue;}
                    if (_var.StartsWith("mc_fee_")){ mpayList_mc_fee_.Add(new KeyValuePair<string, string>( _var.Replace("mc_fee_",""),_value.urlDecode()));continue;}
                    if (_var.StartsWith("mc_gross_")){ mpayList_mc_gross_.Add(new KeyValuePair<string, string>( _var.Replace("mc_gross_",""),_value.urlDecode()));continue;}
                    if (_var.StartsWith("mc_handling")){ mpayList_mc_handling.Add(new KeyValuePair<string, string>( _var.Replace("mc_handling",""),_value.urlDecode()));continue;}
                }
                */
                if (_var == "txn_id") { txn_id = _value.urlDecode(); continue; }
                if (_var == "business") { txn_business = _value.urlDecode(); continue; }
                if (_var == "charset") { txn_charset = _value.urlDecode(); continue; }
                if (_var == "custom") { txn_custom = _value.urlDecode(); continue; }
                if (_var == "notify_version") { txn_notify_version = _value.urlDecode(); continue; }
                if (_var == "parent_txn_id") { txn_parent_txn_id = _value.urlDecode(); continue; }
                if (_var == "receipt_id") { txn_receipt_id = _value.urlDecode(); continue; }
                if (_var == "receiver_email") { txn_receiver_email = _value.urlDecode(); continue; }
                if (_var == "receiver_id") { txn_receiver_id = _value.urlDecode(); continue; }
                if (_var == "resend") { txn_resend = _value == "true"; continue; }
                if (_var == "residence_country") { txn_residence_country = _value.urlDecode(); continue; }
                if (_var == "test_ipn") { txn_test_ipn = _value == "1"; continue; }
                if (_var == "verify_sign") { txn_verify_sign = _value.urlDecode(); continue; }
                if (_var == "address_country") { cl_address_country = _value.urlDecode(); continue; }
                if (_var == "address_city") { cl_address_city = _value.urlDecode(); continue; }
                if (_var == "address_country_code") { cl_address_country_code = _value.urlDecode(); continue; }
                if (_var == "address_name") { cl_address_name = _value.urlDecode(); continue; }
                if (_var == "address_state") { cl_address_state = _value.urlDecode(); continue; }
                if (_var == "address_status") { cl_address_status = _value.ToLower() == "confirmed"; continue; }
                if (_var == "address_street") { cl_address_street = _value.urlDecode(); continue; }
                if (_var == "address_zip") { cl_address_zip = _value.urlDecode(); continue; }
                if (_var == "contact_phone") { cl_contact_phone = _value.urlDecode(); continue; }
                if (_var == "first_name") { cl_first_name = _value.urlDecode(); continue; }
                if (_var == "last_name") { cl_last_name = _value.urlDecode(); continue; }
                if (_var == "payer_business_name") { cl_payer_business_name = _value.urlDecode(); continue; }
                if (_var == "payer_email") { cl_payer_email = _value.urlDecode(); continue; }
                if (_var == "payer_id") { cl_payer_id = _value.urlDecode(); continue; }
                if (_var == "auth_amount") { pay_auth_amount = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "auth_exp") { pay_auth_exp = ConvertPayPalDateTime(_value); continue; }
                if (_var == "auth_id") { pay_auth_id = _value.urlDecode(); continue; }
                if (_var == "auth_status") { pay_auth_status = _value.urlDecode(); continue; }
                if (_var == "exchange_rate") { pay_exchange_rate = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "invoice") { pay_invoice = _value.urlDecode(); continue; }
                if (_var == "mc_currency") { pay_mc_currency = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "mc_fee") { pay_mc_fee = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "mc_gross") { pay_mc_gross = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "mc_handling") { pay_mc_handling = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "mc_shipping") { pay_mc_shipping = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "memo") { pay_memo = _value.urlDecode(); continue; }
                if (_var == "num_cart_items") { pay_num_cart_items = _value.ToInt32(); continue; }
                if (_var == "payer_status") { pay_payer_status = _value.urlDecode(); continue; }
                if (_var == "payment_date") { pay_payment_date = ConvertPayPalDateTime(_value); continue; }
                if (_var == "payment_gross") { pay_payment_gross = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "payment_status") { pay_payment_status = _value.urlDecode(); continue; }
                if (_var == "payment_type") { pay_payment_type = _value.urlDecode(); continue; }
                if (_var == "pending_reason") { pay_pending_reason = _value.urlDecode(); continue; }
                if (_var == "protection_eligibility") { pay_protection_eligibility = _value.urlDecode(); continue; }
                if (_var == "quantity") { pay_quantity = _value.ToInt32(); continue; }
                if (_var == "reason_code") { pay_reason_code = _value.urlDecode(); continue; }
                if (_var == "remaining_settle") { pay_remaining_settle = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "settle_amount") { pay_settle_amount = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "settle_currency") { pay_settle_currency = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "shipping") { pay_shipping = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "shipping_method") { pay_shipping_method = _value.urlDecode(); continue; }
                if (_var == "tax") { pay_tax = _value.Replace(".", ",").ToDecimal(); continue; }
                if (_var == "transaction_entity") { pay_transaction_entity = _value.urlDecode(); continue; }
                /*
                if (_var == "auction_buyer_id") {auc_auction_buyer_id = _value.urlDecode();continue;}
                if (_var == "auction_closing_date") {auc_auction_closing_date = _value.urlDecode();continue;}
                if (_var == "auction_multi_item") {auc_auction_multi_item = _value.urlDecode();continue;}
                if (_var == "for_auction") {auc_for_auction = _value.urlDecode();continue;}
                */
                if (_var.StartsWith("fraud_managment_pending_filters")) { payList_fraud_managment_pending_filters_.Add(new KeyValuePair<string, string>(_var.Replace("fraud_managment_pending_filters_", ""), _value.urlDecode())); continue; }
                if (_var.StartsWith("item_name")) { payList_item_name.Add(new KeyValuePair<string, string>(_var.Replace("item_name", ""), _value.urlDecode())); continue; }
                if (_var.StartsWith("item_number")) { payList_item_number.Add(new KeyValuePair<string, string>(_var.Replace("item_number", ""), _value.urlDecode())); continue; }
                if (_var.StartsWith("mc_gross_")) { payList_mc_gross_.Add(new KeyValuePair<string, string>(_var.Replace("mc_gross_", ""), _value.urlDecode())); continue; }
                if (_var.StartsWith("mc_shipping")) { payList_mc_shipping.Add(new KeyValuePair<string, string>(_var.Replace("mc_shipping", ""), _value.urlDecode())); continue; }
                if (_var.StartsWith("option_name")) { payList_option_name.Add(new KeyValuePair<string, string>(_var.Replace("option_name", ""), _value.urlDecode())); continue; }
                if (_var.StartsWith("option_selection")) { payList_option_selection.Add(new KeyValuePair<string, string>(_var.Replace("option_selection", ""), _value.urlDecode())); continue; }
                if (_var.StartsWith("payment_fee_")) { payList_payment_fee_.Add(new KeyValuePair<string, string>(_var.Replace("payment_fee_", ""), _value.urlDecode())); continue; }
                if (_var.StartsWith("payment_gross_")) { payList_payment_gross_.Add(new KeyValuePair<string, string>(_var.Replace("payment_gross_", ""), _value.urlDecode())); continue; }
                if (_var.StartsWith("quantity")) { payList_quantity.Add(new KeyValuePair<string, string>(_var.Replace("quantity", ""), _value.urlDecode())); continue; }
            }
        }
    }
}
