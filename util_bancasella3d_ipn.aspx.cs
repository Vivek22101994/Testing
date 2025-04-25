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
using ModInvoice;

namespace RentalInRome
{
    public partial class util_bancasella3d_ipn : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        protected magaInvoice_DataContext DC_INVOICE;
        protected magaPayPal_DataContext DC_PAYPAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            DC_PAYPAL = maga_DataContext.DC_PAYPAL;
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                string uid = Request.QueryString["uid"];
                var currTxn = dc.dbInvPayBancaSellaVerifiedByVisaTXNs.SingleOrDefault(x => x.uid.ToString() == uid);
                if (currTxn == null)
                {
                    Response.Clear();
                    Response.Write("ERROR!");
                    Response.End();
                    return;
                }
                currTxn.PARes = Request["PARES"];
                dc.SaveChanges();
                var _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == currTxn.reservationId);
                if (_currTBL == null)
                {
                    Response.Redirect("/reservationarea/login.aspx", true);
                    return;
                }
                INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == currTxn.paymentId);
                if (_pay == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAler", "alert(Error: No payment found);", true);
                    return;
                }

                //set new TLS protocol 1.1/1.2
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  

                var sellaservice = new WsBancaSellaS2S.WSs2s();
                string languageId = "2";
                if (_currTBL.cl_pid_lang == 1) languageId = "1";
                if (_currTBL.cl_pid_lang == 3) languageId = "3";
                if (_currTBL.cl_pid_lang == 4) languageId = "4";
                if (_currTBL.cl_pid_lang == 5) languageId = "5";
                decimal pr_total = _pay.pr_total.objToDecimal() + _pay.chargeFee.objToDecimal();
                if (CommonUtilities.getSYS_SETTING("utilsBancaSellaImportoTest") == "true") pr_total = new decimal(0.10);
                System.Xml.XmlNode risultato = sellaservice.callPagamS2S(
                    CommonUtilities.getSYS_SETTING("utilsBancaSellaCodiceEsercente")
                    , "242"
                    , pr_total.ToString("#0.00").Replace(",", ".")
                    , _pay.code
                    , ""
                    , ""
                    , ""
                    , ""
                    , ""
                    , languageId
                    , ""
                    , ""
                    , currTxn.transKey
                    , currTxn.PARes
                    , ""
                    , "");
                //System.Xml.XmlNode risultato = sellaservice.callPagamS2S("GESPAY60276", "242", _pay.pr_total.objToDecimal().ToString("#0.00").Replace(",", "."), _pay.code, txt_cc_numeroCarta.Text, drp_cc_meseScadenza.SelectedValue, drp_cc_annoScadenza.SelectedValue.Substring(2, 2), txt_cc_titolareCarta.Text, _currTBL.cl_email, "", txt_cc_numeroCarta.Text.Replace(" ", ""), "", "", "", "", "" , "", "", "", null, null, null, null, null, null);
                System.Xml.Linq.XDocument xdoc = System.Xml.Linq.XDocument.Parse(risultato.OuterXml);
                if (xdoc.Descendants("TransactionResult").First().Value == "OK")
                {
                    if (!_pay.pay_txn_gross.HasValue) _pay.pay_txn_gross = 0;
                    _pay.is_complete = 1;
                    _pay.pay_date = DateTime.Now;
                    _pay.pay_mode = "sella";
                    _pay.state_pid = 1;
                    _pay.state_date = DateTime.Now;
                    _pay.state_subject = "Completato";
                    _pay.state_pid_user = 1;
                    DC_INVOICE.SubmitChanges();
                    invUtils.payment_onChange(_currTBL, true);
                    dc.Delete(currTxn);
                    dc.SaveChanges();
                    Response.Redirect(CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id);
                    return;
                }
                else
                {
                    Response.Write("<script>alert('Error: " + xdoc.Descendants("ErrorDescription").First().Value + "');</script>");
                    ErrorLog.addLog(Request.browserIP(), "payBancaSella", xdoc.ToString());
                }
                Response.Write("<script>setTimeout(function(){window.location.href='" + CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id + "'}, 0);</script>");
            }
        }
    }
}
