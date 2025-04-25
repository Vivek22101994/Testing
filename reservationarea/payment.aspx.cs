using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModInvoice;
using ModRental;
using System.Net;

namespace RentalInRome.reservationarea
{
    public partial class payment : basePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (resUtils.CurrentIdReservation_gl != 0 && resUtils.CurrentIdReservation_gl < 150000)
            {
                Response.Redirect("/reservationarea/arrivaldeparture.aspx", true);
                return;
            }
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                for (int i = 1; i <= 12; i++)
                {
                    drp_cc_meseScadenza.Items.Add(new ListItem(i.ToString().fillString("0", 2, false), i.ToString().fillString("0", 2, false)));
                }
                drp_cc_meseScadenza.Items.Insert(0, new ListItem(contUtils.getLabel("lblMonth"), ""));
                drp_cc_annoScadenza.bind_Numbers(DateTime.Now.Year, (DateTime.Now.Year + 10), 1, 4);
                drp_cc_annoScadenza.Items.Insert(0, new ListItem(contUtils.getLabel("lblYear"), ""));
                fillData();
            }
        }
        protected void fillData()
        {

            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            var currAgency = rntProps.AgentTBL.SingleOrDefault(x => x.id == _currTBL.agentID);

            if (currAgency != null && currAgency.isSendManualEmail == 1 && (_currTBL.isWL == null || _currTBL.isWL == 0)) //White Label
            {
                PH_AgnecyResNoPayment.Visible = true;
                PH_PaymentDetails.Visible = false;
            }
            else
            {
                PH_AgnecyResNoPayment.Visible = false;
                PH_PaymentDetails.Visible = true;
            }
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            LV_DataBind();

            HF_id.Value = _currTBL.id.ToString();
            HF_visa_isRequested.Value = _currTBL.visa_isRequested.objToInt32().ToString();
            ltr_unique_id.Text = _currTBL.unique_id.ToString();
            HF_code.Value = _currTBL.code;
            ltr_cl_name_full.Text = _currTBL.cl_name_full;
            HF_dtStart.Value = _currTBL.dtStart.JSCal_dateToString();
            HF_dtEnd.Value = _currTBL.dtEnd.JSCal_dateToString();
            HF_num_adult.Value = _currTBL.num_adult.ToString();
            HF_num_child_over.Value = _currTBL.num_child_over.ToString();
            HF_num_child_min.Value = _currTBL.num_child_min.ToString();
            HF_pr_total.Value = _currTBL.pr_total.ToString();
            HF_pr_deposit.Value = _currTBL.pr_deposit.ToString();
            HF_pr_part_commission_tf.Value = _currTBL.pr_part_commission_tf.ToString();
            HF_pr_part_commission_total.Value = _currTBL.pr_part_commission_total.ToString();
            HF_pr_part_agency_fee.Value = _currTBL.pr_part_agency_fee.ToString();
            HF_pr_part_payment_total.Value = _currTBL.pr_part_payment_total.ToString();
            HF_pr_part_owner.Value = _currTBL.pr_part_owner.ToString();
            //INV_TBL_PAYMENT _pay = AdminUtilities.rntReservation_checkPartPayment(_currTBL);
            //HF_inv_pay_id.Value = _pay.id.ToString();
            //HF_pr_part_is_payed.Value = _pay.is_complete.ToString();


             bool isbBcomHotel = false;
            if (_currTBL.agentID == 1016)
            {
                #region Old code
                //using (DCmodRental dc = new DCmodRental())
                //{
                //string bcomHotelId = _estTB.bcomHotelId + "";
                //var bcomHotel = dc.dbRntBcomHotelTBLs.FirstOrDefault(x => x.hotelId == bcomHotelId);
                //if (bcomHotel != null)
                //{
                //    ltr_est_code.Text = bcomHotel.title;
                //    isbBcomHotel = true;
                //} 
                //} 
                #endregion

                if (!string.IsNullOrEmpty(_estTB.bcomName))
                {
                    ltr_est_code.Text = _estTB.bcomName;
                    isbBcomHotel = true;
                }                
            }

            if (!isbBcomHotel)
            {
                RNT_LN_ESTATE _estLN = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _currTBL.pid_estate && x.pid_lang == _currTBL.cl_pid_lang);
                if (_estLN == null)
                    _estLN = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _currTBL.pid_estate && x.pid_lang == 2);
                if (_estLN != null)
                {
                    ltr_est_code.Text = _estLN.title;
                }
                else
                {
                    ltr_est_code.Text = _estTB.code;
                }
            }
            ltr_est_address.Text = _estTB.loc_address + " " + _estTB.loc_zip_code + " <br/>" + CurrentSource.loc_cityTitle(_estTB.pid_city.objToInt32(), _currTBL.cl_pid_lang.objToInt32(), "Rome");

        }
        protected void saveData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
        }
        protected void LV_DataBind()
        {
            #region White Label
             dbRntAgentWLPaymentRL agentWLPayment = (dbRntAgentWLPaymentRL)null;
             bool agentPaypalIsOk = false;
             if (App.WLAgentId > 0)
             {
                 using (DCmodRental dc = new DCmodRental())
                 {
                     agentWLPayment = dc.dbRntAgentWLPaymentRLs.FirstOrDefault(x => x.pidAgent == App.WLAgentId && x.paymentType == "paypal");
                     agentPaypalIsOk = (agentWLPayment != null && agentWLPayment.email != null && agentWLPayment.email != "");
                 }
             }
            #endregion

            var BancaSella = invProps.CashPlaceLK.FirstOrDefault(x => x.code == "sella");
            var Paypal = invProps.CashPlaceLK.FirstOrDefault(x => x.code == "paypal");
            List<INV_TBL_PAYMENT> listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && x.rnt_reservation_part.StartsWith("part") && x.pr_total.HasValue && x.pr_total > 0).ToList();
            LV_partPayment.DataSource = listPay.OrderBy(x => x.dtCreation);
            LV_partPayment.DataBind();
            foreach (ListViewDataItem item in LV_partPayment.Items)
            {
                var lnkPayWithBancaSella = item.FindControl("lnkPayWithBancaSella") as LinkButton; 
                if (lnkPayWithBancaSella != null) 
                {
                    if (App.WLAgentId > 0)//White Label
                        lnkPayWithBancaSella.Visible = false;
                    else
                        lnkPayWithBancaSella.Visible = BancaSella != null && BancaSella.isActive == 1 && (CommonUtilities.getSYS_SETTING("utilsBancaSellaIsActive") == "true" || CommonUtilities.getSYS_SETTING("utilsBancaSellaIsActive").ToInt32() == 1); 
                }
                var lnkPayWithPaypal = item.FindControl("lnkPayWithPaypal") as LinkButton; 
                if (lnkPayWithPaypal != null) 
                {
                    if (App.WLAgentId > 0)//White Label
                        lnkPayWithPaypal.Visible = (agentPaypalIsOk && Paypal != null && Paypal.isActive == 1);
                    else
                        lnkPayWithPaypal.Visible = Paypal != null && Paypal.isActive == 1; 
                }
                Label lbl_id = item.FindControl("lbl_id") as Label;
                Label lbl_is_complete = item.FindControl("lbl_is_complete") as Label;
                PlaceHolder PH_payed = item.FindControl("PH_payed") as PlaceHolder;
                PlaceHolder PH_toPay = item.FindControl("PH_toPay") as PlaceHolder;
                if (lbl_is_complete == null || lbl_id == null || PH_payed == null || PH_toPay == null) return;
                PH_payed.Visible = lbl_is_complete.Text == "1";
                PH_toPay.Visible = !PH_payed.Visible;
            }
            pnl_partPaymentView.Visible = LV_partPayment.Items.Count == 0;

            listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && (x.rnt_reservation_part.StartsWith("owner") || x.rnt_reservation_part.StartsWith("full_")) && x.pr_total.HasValue && x.pr_total > 0).ToList();
            LV_partOwner.DataSource = listPay.OrderBy(x => x.dtCreation);
            LV_partOwner.DataBind();
            foreach (ListViewDataItem item in LV_partOwner.Items)
            {
                var lnkPayWithBancaSella = item.FindControl("lnkPayWithBancaSella") as LinkButton;
                if (lnkPayWithBancaSella != null)
                {
                    if (App.WLAgentId > 0)//White Label
                        lnkPayWithBancaSella.Visible = false;
                    else
                        lnkPayWithBancaSella.Visible = BancaSella != null && BancaSella.isActive == 1 && (CommonUtilities.getSYS_SETTING("utilsBancaSellaIsActive") == "true" || CommonUtilities.getSYS_SETTING("utilsBancaSellaIsActive").ToInt32() == 1);
                }
                var lnkPayWithPaypal = item.FindControl("lnkPayWithPaypal") as LinkButton;
                if (lnkPayWithPaypal != null)
                {
                    if (App.WLAgentId > 0)//White Label
                        lnkPayWithPaypal.Visible = (agentPaypalIsOk && Paypal != null && Paypal.isActive == 1);
                    else
                        lnkPayWithPaypal.Visible = Paypal != null && Paypal.isActive == 1; 
                }
                Label lbl_id = item.FindControl("lbl_id") as Label;
                Label lbl_is_complete = item.FindControl("lbl_is_complete") as Label;
                PlaceHolder PH_payed = item.FindControl("PH_payed") as PlaceHolder;
                PlaceHolder PH_toPay = item.FindControl("PH_toPay") as PlaceHolder;
                if (lbl_is_complete == null || lbl_id == null || PH_payed == null || PH_toPay == null) return;
                PH_payed.Visible = lbl_is_complete.Text == "1";
                PH_toPay.Visible = !PH_payed.Visible;
            }
            pnl_ownerPaymentView.Visible = LV_partOwner.Items.Count == 0;
            lnkPayWithBancaSellaFull.Visible = BancaSella != null && BancaSella.isActive == 1 && (CommonUtilities.getSYS_SETTING("utilsBancaSellaIsActive") == "true" || CommonUtilities.getSYS_SETTING("utilsBancaSellaIsActive").ToInt32() == 1);
            lnkPayWithPaypalFull.Visible = Paypal != null && Paypal.isActive == 1;

            pnl_fullPaymentNO.Visible = true;
            pnl_fullPaymentOK.Visible = false;
            pnl_fullPaymentKO.Visible = false;
            if (!pnl_partPaymentView.Visible || !pnl_ownerPaymentView.Visible)
                return;
            INV_TBL_PAYMENT fullPayment = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.rnt_pid_reservation == _currTBL.id && x.rnt_reservation_part == "full");
            if (fullPayment != null)
            {
                HF_fullPaymentCode.Value = fullPayment.code;
                pnl_fullPaymentNO.Visible = false;
                pnl_fullPaymentOK.Visible = fullPayment.is_complete == 1;
                pnl_fullPaymentKO.Visible = pnl_ownerPaymentKO.Visible = pnl_partPaymentKO.Visible = !pnl_fullPaymentOK.Visible;
            }
        }
        protected void LV_payment_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }
        protected void lnkPayWithPaypal_Click(object sender, EventArgs e)
        {
            var Paypal = invProps.CashPlaceLK.FirstOrDefault(x => x.code == "paypal");
            HF_paymentId.Value = ((LinkButton)sender).CommandArgument;
            if (HF_paymentId.Value == "full") HF_paymentId.Value = HF_fullPaymentCode.Value;
            INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == HF_paymentId.Value.ToInt64());
            if (_pay == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAler", "alert('Error: No payment found');", true);
                return;
            }
            _pay.chargeFeePerc = 0;
            _pay.chargeFee = 0;
            if (_pay.rnt_reservation_part.StartsWith("part"))
                _pay.chargeFeePerc = Paypal.chargeFeePart1.objToDecimal();
            else if (_pay.rnt_reservation_part.StartsWith("full"))
                _pay.chargeFeePerc = Paypal.chargeFeeFull.objToDecimal();
            else
                _pay.chargeFeePerc = Paypal.chargeFeePart2.objToDecimal();
            if (_pay.chargeFeePerc != 0)
            {
                decimal chargeFeeDecimal = Decimal.Divide(Decimal.Multiply(_pay.pr_total.objToDecimal(), _pay.chargeFeePerc.objToDecimal()), 100);
                decimal chargeFeeRounded = Decimal.Round(chargeFeeDecimal, 2);
                if (chargeFeeRounded < chargeFeeDecimal)
                    chargeFeeRounded += new Decimal(0.01);
                _pay.chargeFee = chargeFeeRounded;
            }
            _pay.chargeFeeInvoice = Paypal.chargeFeeInvoice;
            DC_INVOICE.SubmitChanges();
            if (_pay.chargeFee.objToDecimal() == 0 && 1 == 2)
            {
                Response.Redirect("/util_paypal_redirect.aspx?type=payment&code=" + _pay.id + "&lang=" + App.LangID);
                return;
            }
            HF_prToPay.Value = _pay.pr_total.objToDecimal().ToString("N2");
            HF_prFee.Value = _pay.chargeFeePerc.objToDecimal().ToString("N2");
            HF_prTotal.Value = (_pay.pr_total.objToDecimal() + _pay.chargeFee.objToDecimal()).ToString("N2");
            pnlSendBancaSella.Visible = false;
            pnlSendPaypal.Visible = true;
            pnlSendPaypalFees.Visible = _pay.chargeFee.objToDecimal() > 0;
        }
        protected void lnkPayWithBancaSella_Click(object sender, EventArgs e)
        {
            var BancaSella = invProps.CashPlaceLK.FirstOrDefault(x => x.code == "sella");
            HF_paymentId.Value = ((LinkButton)sender).CommandArgument;
            if (HF_paymentId.Value == "full") HF_paymentId.Value = HF_fullPaymentCode.Value;
            INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == HF_paymentId.Value.ToInt64());
            if (_pay == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAler", "alert('Error: No payment found');", true);
                return;
            }
            _pay.chargeFeePerc = 0;
            _pay.chargeFee = 0;
            if (_pay.rnt_reservation_part.StartsWith("part"))
                _pay.chargeFeePerc = BancaSella.chargeFeePart1.objToDecimal();
            else if (_pay.rnt_reservation_part.StartsWith("full"))
                _pay.chargeFeePerc = BancaSella.chargeFeeFull.objToDecimal();
            else
                _pay.chargeFeePerc = BancaSella.chargeFeePart2.objToDecimal();
            if (_pay.chargeFeePerc != 0)
            {
                decimal chargeFeeDecimal = Decimal.Divide(Decimal.Multiply(_pay.pr_total.objToDecimal(), _pay.chargeFeePerc.objToDecimal()), 100);
                decimal chargeFeeRounded = Decimal.Round(chargeFeeDecimal, 2);
                if (chargeFeeRounded < chargeFeeDecimal)
                    chargeFeeRounded += new Decimal(0.01);
                _pay.chargeFee = chargeFeeRounded;
            }
            _pay.chargeFeeInvoice = BancaSella.chargeFeeInvoice;
            DC_INVOICE.SubmitChanges();
            HF_prToPay.Value = _pay.pr_total.objToDecimal().ToString("N2");
            HF_prFee.Value = _pay.chargeFeePerc.objToDecimal().ToString("N2");
            HF_prTotal.Value = (_pay.pr_total.objToDecimal() + _pay.chargeFee.objToDecimal()).ToString("N2");
            pnlSendPaypal.Visible = false;
            pnlSendBancaSella.Visible = true;
            pnlSendBancaSellaFees.Visible = _pay.chargeFee.objToDecimal() > 0;
        }
        protected void lnk_payBancaSella_Click(object sender, EventArgs e)
        {
            var BancaSella = invProps.CashPlaceLK.FirstOrDefault(x => x.code == "sella");
            pnlVerifiedByVisa.Visible = false;
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            INV_TBL_PAYMENT _pay = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == HF_paymentId.Value.ToInt64());
            if (_pay == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAler", "alert('Error: No payment found');", true);
                return;
            }

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;  

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
                , txt_cc_numeroCarta.Text
                , drp_cc_meseScadenza.SelectedValue
                , drp_cc_annoScadenza.SelectedValue.Substring(2, 2)
                , txt_cc_titolareCarta.Text
                , _currTBL.cl_email
                , languageId
                , txt_cc_cvv.Text.Replace(" ", "")
                , ""
                , ""
                , ""
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
                Response.Redirect("/reservationarea/payment.aspx");
                return;
            }
            else if (xdoc.Descendants("ErrorCode").First().Value == "8006")
            {
                using (magaInvoice_DataContext dc = maga_DataContext.DC_INVOICE)
                {
                    var currTxn = new InvPayBancaSellaVerifiedByVisaTXN();
                    currTxn.uid = Guid.NewGuid();
                    currTxn.reservationId = _currTBL.id;
                    currTxn.paymentId = _pay.id;
                    currTxn.transKey = xdoc.Descendants("TransactionKey").First().Value;
                    currTxn.VbVRisp = xdoc.Descendants("VbV").First().Descendants("VbVRisp").First().Value;
                    currTxn.createdDate = DateTime.Now;
                    dc.InvPayBancaSellaVerifiedByVisaTXN.InsertOnSubmit(currTxn);
                    dc.SubmitChanges();
                    HF_uidVerifiedByVisa.Value = currTxn.uid + "";
                    pnlVerifiedByVisa.Visible = true;

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAler", "alert('Error: " + xdoc.Descendants("ErrorDescription").First().Value + "');", true);
                ErrorLog.addLog(Request.browserIP(), "payBancaSella", xdoc.ToString());
            }
            fillData();
        }
    }
}