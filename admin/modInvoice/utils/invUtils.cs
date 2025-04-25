using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModInvoice;
using System.Threading;
using RentalInRome.data;
using ModRental;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using ModAuth;

public class invUtils
{
    public static string getCashType_title(int id, string alternate)
    {
        dbInvCashTypeLK tmp = invProps.CashTypeLK.SingleOrDefault(x => x.id == id);
        if (tmp != null)
            return tmp.title;
        return alternate;
    }
    public static string getCashPlace_title(int id, string alternate)
    {
        dbInvCashPlaceLK tmp = invProps.CashPlaceLK.SingleOrDefault(x => x.id == id);
        if (tmp != null)
            return tmp.title;
        return alternate;
    }
    public static string getCashDocCase_code(int id, string alternate)
    {
        dbInvCashDocCaseLK tmp = invProps.CashDocCaseLK.SingleOrDefault(x => x.id == id);
        if (tmp != null)
            return tmp.code;
        return alternate;
    }
    public static string getCashTax_code(int id, string alternate)
    {
        dbInvCashTaxLK tmp = invProps.CashTaxLK.SingleOrDefault(x => x.id == id);
        if (tmp != null)
            return tmp.code;
        return alternate;
    }
    public static decimal getCashTax_taxAmount(int id)
    {
        dbInvCashTaxLK tmp = invProps.CashTaxLK.SingleOrDefault(x => x.id == id);
        if (tmp != null && tmp.taxAmount.HasValue)
            return tmp.taxAmount.Value;
        return 0;
    }
    public static string getInvoiceNotificationsType(string uuid)
    {
        if (!string.IsNullOrWhiteSpace(uuid))
        {
            var tmp = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE_NOTIFICATIONs.FirstOrDefault(x => x.invoice_uuid == uuid);
            if (tmp != null)
                return tmp.type;
        }
        return "";
    }
    public static string getInvoiceNotificationsMessage(string uuid)
    {
        if (!string.IsNullOrWhiteSpace(uuid))
        {
            var tmp = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE_NOTIFICATIONs.FirstOrDefault(x => x.invoice_uuid == uuid);
            if (tmp != null)
                return tmp.message;
        }
        return "";
    }
    #region OLD
    public static string invPayment_modeTitle(string code, string alt)
    {
        dbInvCashPlaceLK tmp = invProps.CashPlaceLK.FirstOrDefault(x => x.code == code);
        if (tmp != null)
            return tmp.title;
        return alt;
    }

    public static string invPayment_causeTitle(string code, string alt)
    {
        var _d = INV_LK_PAYMENT_CAUSEs.SingleOrDefault(x => x.code == code);
        if (_d != null)
            return _d.title;
        return alt;
    }
    private static List<INV_LK_PAYMENT_CAUSE> _INV_LK_PAYMENT_CAUSEs; // refresh NO
    public static List<INV_LK_PAYMENT_CAUSE> INV_LK_PAYMENT_CAUSEs
    {
        get
        {
            if (_INV_LK_PAYMENT_CAUSEs == null)
                _INV_LK_PAYMENT_CAUSEs = maga_DataContext.DC_INVOICE.INV_LK_PAYMENT_CAUSEs.ToList();
            return _INV_LK_PAYMENT_CAUSEs;
        }
        set { _INV_LK_PAYMENT_CAUSEs = value; }
    }
    private class payment_onChangeWork
    {
        RNT_TBL_RESERVATION currResTmp;
        public void doWork()
        {
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            RNT_TBL_RESERVATION currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == currResTmp.id);
            if (currRes != null && currRes.dtStart.HasValue && currRes.dtEnd.HasValue)
            {
                string rnt_reservationOverbookingAction = CommonUtilities.getSYS_SETTING("rnt_reservationOverbookingAction");
                if (rnt_reservationOverbookingAction == "") rnt_reservationOverbookingAction = "accept";

                decimal _payed_total = 0;
                List<INV_TBL_PAYMENT> _listPayed = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == currRes.id && x.direction == 1).ToList();
                foreach (INV_TBL_PAYMENT _currPay in _listPayed)
                {
                    if (_currPay.is_complete != 1) continue;
                    _payed_total += _currPay.pr_total.objToDecimal();
                }
                INV_TBL_PAYMENT lastPayment = _listPayed.Where(x => x.is_complete == 1 && x.direction == 1).OrderByDescending(x => x.pay_date).FirstOrDefault();
                if (_payed_total >= currRes.pr_part_payment_total && currRes.state_pid == 6)
                {
                    bool _isAvailable = rntUtils.rntEstate_isAvailable(currRes.pid_estate.objToInt32(), currRes.dtStart.Value, currRes.dtEnd.Value, currRes.id) == null;
                    if (!_isAvailable)
                    {
                        rntUtils.rntReservation_onStateChange(currRes);
                        currRes.state_pid = 4;
                        currRes.state_date = DateTime.Now;
                        currRes.state_subject = "Prenotato-OB";
                        currRes.state_pid_user = 1;
                        string mSubject = "Attenzione - Prenotazione in OverBooking rif #" + currRes.code;
                        string mBody = "Abbiamo registrato un pagamento della pren #" + currRes.code + " che risulta in overbooking.<br/>Si prega di verificare il motivo e conttare il cliente.";
                        MailingUtilities.autoSendMailTo(mSubject, mBody, "info@rentalinrome.com", true, "invUtils.payment_onChangeWork alert");
                        rntUtils.rntReservation_mailPartPaymentReceive(currRes, true, true, false, false, false, 1); // send mails, solo cliente e admin
                    }
                    else
                    {
                        rntUtils.rntReservation_onStateChange(currRes);
                        currRes.state_pid = 4;
                        currRes.state_date = DateTime.Now;
                        currRes.state_subject = "Prenotato";
                        currRes.state_pid_user = 1;
                        rntUtils.rntReservation_mailPartPaymentReceive(currRes, true, true, true, true, true, 1); // send mails
                    }
                }
                else if (currRes.state_pid == 3)
                {
                    string mSubject = "Attenzione - Pagamento di una prenotazione cancellata rif #" + currRes.code;
                    string mBody = "Abbiamo registrato un pagamento della pren #" + currRes.code + " che risulta cancellata.<br/>Per i possibili ovverbooking non abbiamo confermato la pren.<br/>Si prega di verificare il motivo e mettere 'Prenotato' manualmente.";
                    MailingUtilities.autoSendMailTo(mSubject, mBody, "info@rentalinrome.com", true, "invUtils.payment_onChangeWork alert");
                }
                if (currRes.state_pid == 4)
                {

                    if (_payed_total >= currRes.pr_total && (currRes.cl_reminderFullPaymentSent == null || currRes.cl_reminderFullPaymentSent.Value == 0))
                    {
                        // Send balance payement received mail 
                        currRes.cl_reminderFullPaymentSent = 1;
                        rntUtils.rntReservation_mailBalancePaymentReceive(currRes, true, true, false, false, false, UserAuthentication.CurrentUserID);
                    }

                    if (rnt_reservationOverbookingAction != "accept") // se accettiam OB non fare nulla
                    {
                        // altrimenti cancella le pren
                        // elenco pren Da confermare che possono diventare OB
                        var resList = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.id != currRes.id && x.pid_estate == currRes.pid_estate.objToInt32() //
                                                         && x.state_pid == 6 //
                                                         && x.dtStart.HasValue //
                                                         && x.dtEnd.HasValue //
                                                         && ((x.dtStart.Value.Date <= currRes.dtStart.Value && x.dtEnd.Value.Date >= currRes.dtEnd.Value) //
                                                             || (x.dtStart.Value.Date >= currRes.dtStart.Value && x.dtStart.Value.Date < currRes.dtEnd.Value) //
                                                             || (x.dtEnd.Value.Date > currRes.dtStart.Value && x.dtEnd.Value.Date <= currRes.dtEnd.Value))).ToList();
                        foreach (var tmpRes in resList)
                        {
                            tmpRes.state_pid = 3;
                            tmpRes.state_body = "Cancellato automaticamente dal sistema per evitare OB";
                            tmpRes.state_date = DateTime.Now;
                            tmpRes.state_pid_user = 1;
                            tmpRes.state_subject = "CANC-OB";
                        }
                    }
                }
                currRes.payed_total = _payed_total;
                if (lastPayment != null)
                {
                    currRes.payed_date = lastPayment.state_date;
                    currRes.payed_user = lastPayment.state_pid_user;
                    currRes.payed_mode = lastPayment.pay_mode;
                }
                DC_RENTAL.SubmitChanges();

                rntUtils.rntReservation_onChange(currRes);
                foreach (var payment in _listPayed.Where(x => x.is_complete == 1 && x.direction == 1))
                {
                    payment_checkInvoice(payment, currRes);
                }

            }

        }
        public payment_onChangeWork(RNT_TBL_RESERVATION CurrRes, bool autoStart)
        {
            currResTmp = CurrRes;
            if (autoStart)
            {
                doWork(); return;
                //Action<object> action = (object obj) => { doWork(); };
                //AppUtilsTaskScheduler.AddTask(action, "payment_onChangeWork");
                ThreadStart start = new ThreadStart(doWork);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();

            }
        }
    }
    public static bool payment_onChange(RNT_TBL_RESERVATION CurrRes, bool doThread)
    {
        payment_onChangeWork _tmp = new payment_onChangeWork(CurrRes, doThread);
        if (doThread)
            return true;
        _tmp.doWork();
        return true;
    }
    private class payment_checkInvoiceWork
    {
        INV_TBL_PAYMENT _pay;
        RNT_TBL_RESERVATION _res;
        public void doWork()
        {
            var CashPlace = invProps.CashPlaceLK.FirstOrDefault(x => x.code == _pay.pay_mode);
            dbInvCompanyTBL currCompany;
            var agentTBL = rntProps.AgentTBL.SingleOrDefault(x => x.id == _res.agentID);
            decimal _inv_pr_total = _pay.pr_total.objToDecimal() - _pay.pr_noInvoice.objToDecimal();

            if (!_pay.pr_total.HasValue || _pay.pr_total <= 0 || _pay.direction != 1 || _inv_pr_total <= 0) return;
            RNT_TB_ESTATE _est = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _res.pid_estate);
            magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
            INV_TBL_INVOICE _inv = DC_INVOICE.INV_TBL_INVOICE.FirstOrDefault(x => x.inv_pid_payment == _pay.id);
            bool isnewinvoice = false;
            if (_inv == null)
            {
                isnewinvoice = true;
                DateTime _dtInvoice = DateTime.Now;
                if (_pay.pay_date.HasValue && _pay.pay_date.Value < new DateTime(2011, 9, 15)) _dtInvoice = _pay.pay_date.Value;
                int inv_year = _dtInvoice.Year;
                int? _lastCounter = DC_INVOICE.INV_TBL_INVOICE.Where(x => x.inv_dtInvoice.HasValue && x.inv_dtInvoice.Value.Year == _dtInvoice.Year && x.inv_counter.HasValue).Max(x => x.inv_counter);
                if (_lastCounter == null) _lastCounter = 0;
                _inv = new INV_TBL_INVOICE();
                _inv.uid = Guid.NewGuid();
                _inv.inv_year = inv_year;
                _inv.inv_counter = _lastCounter + 1;
                _inv.code = inv_year + "-" + (_inv.inv_counter + "").fillString("0", 5, false);
                _inv.rnt_pid_reservation = _pay.rnt_pid_reservation;
                _inv.rnt_reservation_code = _pay.rnt_reservation_code;
                _inv.inv_pid_payment = _pay.id;
                _inv.inv_dtInvoice = _dtInvoice;
                _inv.inv_dtPayment = _pay.pay_date;
                _inv.dtCreation = DateTime.Now;
                DC_INVOICE.INV_TBL_INVOICE.InsertOnSubmit(_inv);
                DC_INVOICE.SubmitChanges();
                _inv.pid_operator = 1;
                _inv.is_active = 1;
                _inv.is_deleted = 0;
                _inv.is_exported_1 = 0;
                _inv.is_exported_2 = 0;
                _inv.is_exported_3 = 0;

                // check if the whole amount has to be in invoice, for agencies
                if (agentTBL != null && _est.pid_zone != 9)
                {
                    //if (agentTBL.IsInvoiceComplete == null || agentTBL.IsInvoiceComplete == 1 || agentTBL.InvoicePercentage.objToDecimal() >= 100)
                    //{
                    //    _pay.pr_noInvoice = 0;
                    //}
                    //else
                    //{
                    //    _pay.pr_noInvoice = Decimal.Divide(Decimal.Multiply(_pay.pr_total.objToDecimal(), (100 - agentTBL.InvoicePercentage.objToDecimal())), 100);
                    //    _pay.pr_noInvoice = _pay.pr_noInvoice.Value.customRound(false);
                    //}
                }
            }
            decimal taxPercentage = new decimal(0.10);
            _inv.pr_tax_id = CommonUtilities.getSYS_SETTING("rntDefaultVatId").ToInt32();
            if (_inv.pr_tax_id == 0) _inv.pr_tax_id = 1;
            dbInvCashTaxLK currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == _inv.pr_tax_id);
            if (currTax != null)
            {
                taxPercentage = currTax.taxAmount.objToDecimal();
            }
            decimal? pr_total = _inv.pr_total == null ? 0 : _inv.pr_total;
            _inv.pr_total = _pay.pr_total.objToDecimal() - _pay.pr_noInvoice.objToDecimal();
            _inv.pr_tf = Math.Round((_inv.pr_total.objToDecimal() / (1 + taxPercentage)), 2);
            _inv.pr_tax = _inv.pr_total - _inv.pr_tf;
            int _currLang = 2;
            // dati cliente
            if (_est != null && _est.pid_zone == 9)
            {
                _inv.cl_id = 0;
                _inv.cl_pid_discount = _res.cl_pid_discount;
                _inv.cl_pid_lang = 1;
                _inv.cl_name_honorific = "";
                _inv.cl_name_full = "SERVIT Srl";
                _inv.cl_email = "servitre@servitre.it";
                _inv.cl_phone = "";
                _inv.cl_fax = "";
                _inv.cl_loc_country = "Italy";
                _inv.cl_loc_state = "MI";
                _inv.cl_loc_city = "Milano";
                _inv.cl_loc_address = "Via Fernanda Wittgens, 3";
                _inv.cl_loc_zip_code = "20123";
                _inv.cl_doc_type = "";
                _inv.cl_doc_num = "";
                _inv.cl_doc_issue_place = "";
                _inv.cl_doc_issue_date = null;
                _inv.cl_doc_expiry_date = null;
                _inv.cl_doc_vat_num = "05329470966";
                _inv.cl_doc_cf_num = "";
                if (_currLang == 0) _currLang = 2;
            }
            else if (_res.agentID.objToInt64() != 0 && agentTBL != null && (_res.agentClientID.objToInt32() > 0 || agentTBL.Fattura.objToInt32() == 1))
            {
                if (agentTBL.Fattura.objToInt32() == 1)
                {
                    _currLang = agentTBL.pidLang.objToInt32();
                    if (_currLang == 0) _currLang = 2;
                    _inv.cl_id = -1;
                    _inv.cl_pid_discount = 0;
                    _inv.cl_pid_lang = _currLang;
                    _inv.cl_name_honorific = "";
                    _inv.cl_name_full = agentTBL.nameCompany;
                    _inv.cl_email = agentTBL.contactEmail;
                    _inv.cl_phone = "";
                    _inv.cl_fax = "";
                    _inv.cl_loc_country = agentTBL.locCountry;
                    _inv.cl_loc_state = agentTBL.locState;
                    _inv.cl_loc_province = agentTBL.cl_loc_province;
                    _inv.cl_loc_city = agentTBL.locCity;
                    _inv.cl_loc_address = agentTBL.locAddress;
                    _inv.cl_loc_zip_code = "00000";
                    _inv.cl_doc_type = "";
                    _inv.cl_doc_num = "";
                    _inv.cl_doc_issue_place = "";
                    _inv.cl_doc_issue_date = null;
                    _inv.cl_doc_expiry_date = null;
                    _inv.cl_doc_vat_num = agentTBL.docVat;
                    _inv.cl_doc_cf_num = agentTBL.docCf;
                }
                else
                {
                    using (DCmodAuth dc = new DCmodAuth())
                    {
                        _inv.cl_id = -1;
                        _inv.cl_pid_discount = _res.cl_pid_discount;
                        _inv.cl_pid_lang = _res.cl_pid_lang;
                        _inv.cl_name_honorific = _res.cl_name_honorific;
                        _inv.cl_name_full = _res.cl_name_full;
                        _inv.cl_email = _res.cl_email;
                        if (_currLang == 0) _currLang = 2;
                        var _cl = dc.dbAuthClientTBLs.FirstOrDefault(x => x.id == _res.agentClientID.objToInt32());
                        if (_cl != null)
                        {
                            _inv.cl_phone = _cl.contactPhoneMobile;
                            _inv.cl_fax = _cl.contactFax;
                            _inv.cl_loc_country = _cl.locCountry;
                            _inv.cl_loc_state = _cl.locState;
                            _inv.cl_loc_province = "";
                            _inv.idCodice = "";
                            _inv.cl_loc_city = _cl.locCity;
                            _inv.cl_loc_address = _cl.locAddress;
                            _inv.cl_loc_zip_code = "00000";
                            _inv.cl_doc_type = _cl.docType;
                            _inv.cl_doc_num = _cl.docNum;
                            _inv.cl_doc_issue_place = _cl.docIssuePlace;
                            _inv.cl_doc_issue_date = _cl.docIssueDate;
                            _inv.cl_doc_expiry_date = _cl.docExpiryDate;
                            _inv.cl_doc_vat_num = _cl.docVat;
                            _inv.cl_doc_cf_num = _cl.docCf;
                        }
                    }

                }
            }
            else
            {
                _inv.cl_id = _res.cl_id;
                _inv.cl_pid_discount = _res.cl_pid_discount;
                _inv.cl_pid_lang = _res.cl_pid_lang;
                _inv.cl_name_honorific = _res.cl_name_honorific;
                _inv.cl_name_full = _res.cl_name_full;
                _inv.cl_email = _res.cl_email;
                //_currLang = _inv.cl_pid_lang.objToInt32();
                if (_currLang == 0) _currLang = 2;
                USR_TBL_CLIENT _cl = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _res.cl_id);
                if (_cl != null)
                {
                    _inv.cl_phone = _cl.contact_phone;
                    _inv.cl_fax = _cl.contact_fax;
                    _inv.cl_loc_country = _cl.loc_country;
                    _inv.cl_loc_state = _cl.loc_state;
                    _inv.cl_loc_province = _cl.loc_province;
                    _inv.idCodice = _cl.idCodice;
                    _inv.cl_loc_city = _cl.loc_city;
                    _inv.cl_loc_address = _cl.loc_address;
                    _inv.cl_loc_zip_code = "00000";
                    _inv.cl_doc_type = _cl.doc_type;
                    _inv.cl_doc_num = _cl.doc_num;
                    _inv.cl_doc_issue_place = _cl.doc_issue_place;
                    _inv.cl_doc_issue_date = _cl.doc_issue_date;
                    _inv.cl_doc_expiry_date = _cl.doc_expiry_date;
                    _inv.cl_doc_vat_num = _cl.doc_vat_num;
                    _inv.cl_doc_cf_num = _cl.doc_cf_num;
                }
                if (_res.inv_isDifferent == 1)
                {
                    _inv.cl_name_honorific = _res.inv_name_honorific;
                    _inv.cl_name_full = _res.inv_name_full;
                    _inv.cl_loc_country = _res.inv_loc_country;
                    _inv.cl_loc_state = _res.inv_loc_state;
                    //_inv.cl_loc_province = _cl.loc_province;
                    _inv.cl_loc_city = _res.inv_loc_city;
                    _inv.cl_loc_address = _res.inv_loc_address;
                    _inv.cl_loc_zip_code = "00000";
                    _inv.cl_doc_vat_num = _res.inv_doc_vat_num;
                    _inv.cl_doc_cf_num = _res.inv_doc_cf_num;
                }
            }

            #region city/state/province empty

            if (_inv.cl_loc_address + "" == "")
                _inv.cl_loc_address = _inv.cl_loc_country + "";

            if (_inv.cl_loc_city + "" == "")
                _inv.cl_loc_city = _inv.cl_loc_country + "";

            if (_inv.cl_loc_state + "" == "")
                _inv.cl_loc_state = _inv.cl_loc_country + "";

            if (_inv.cl_loc_province == "")
            {
                if (_inv.cl_loc_country + "" != "" && _inv.cl_loc_country.Length >= 2)
                {
                    if ((_inv.cl_loc_country + "").ToLower() == "united states")
                        _inv.cl_loc_province = "US";
                    else if ((_inv.cl_loc_country + "").ToLower() == "united kingdom")
                        _inv.cl_loc_province = "UK";
                    else
                        _inv.cl_loc_province = _inv.cl_loc_country.Substring(0, 2);
                }
            }
            #endregion

            _inv.is_payed = _pay.is_complete;
            DC_INVOICE.SubmitChanges();

            // crea items
            INV_TBL_INVOICE_ITEM itemRnt = DC_INVOICE.INV_TBL_INVOICE_ITEM.SingleOrDefault(x => x.pid_invoice == _inv.id && x.sequence == 1);
            if (itemRnt == null)
            {
                itemRnt = new INV_TBL_INVOICE_ITEM();
                itemRnt.id = Guid.NewGuid();
                itemRnt.pid_invoice = _inv.id;
                itemRnt.is_deleted = 0;
                itemRnt.sequence = 1;
                itemRnt.quantity = 1;
                itemRnt.quantity_type = "pz";
                itemRnt.code = "Rental in Rome";
                DC_INVOICE.INV_TBL_INVOICE_ITEM.InsertOnSubmit(itemRnt);
                DC_INVOICE.SubmitChanges();

            }
            itemRnt.price_unit = _inv.pr_tf;
            itemRnt.price_tax_id = _inv.pr_tax_id;
            itemRnt.price_tf = _inv.pr_tf;
            itemRnt.price_tax = _inv.pr_tax;
            itemRnt.price_total = _inv.pr_total;
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            {
                RNT_TBL_RESERVATION tblReservation = dcOld.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _pay.rnt_pid_reservation);
                if (tblReservation == null) return;
                RNT_TB_ESTATE tblEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == tblReservation.pid_estate);
                if (tblEstate == null) tblEstate = new RNT_TB_ESTATE();
                itemRnt.description = tblEstate.code + " " + tblReservation.dtStart.formatCustom("#MM# #dd#", _currLang, "") + " - " + tblReservation.dtEnd.formatCustom("#MM# #dd#, #yy#", _currLang, "");
                if (_pay.rnt_reservation_part == "part")
                    itemRnt.description += " (Part payment)";
                if (_pay.rnt_reservation_part == "part_diff")
                    itemRnt.description += " (Part payment difference)";
                if (_pay.rnt_reservation_part == "owner")
                    itemRnt.description += " (Main payment)";
                if (_pay.rnt_reservation_part == "owner_diff")
                    itemRnt.description += " (Main payment difference)";
                if (_pay.rnt_reservation_part == "full")
                    itemRnt.description += " (Full payment)";
                if (_pay.rnt_reservation_part == "full_diff")
                    itemRnt.description += " (Full payment difference)";
                if (_est != null && _est.pid_zone == 9)
                    itemRnt.description = "Provvigioni " + itemRnt.description;

            }
            DC_INVOICE.SubmitChanges();


            // item for Fee
            if (_pay.chargeFeeInvoice.objToInt32() == 1 && _pay.chargeFee.objToDecimal() > 0 && _inv.pr_total.objToDecimal() > 0)
            {
                INV_TBL_INVOICE_ITEM itemFee = DC_INVOICE.INV_TBL_INVOICE_ITEM.SingleOrDefault(x => x.pid_invoice == _inv.id && x.sequence == 2);
                if (itemFee == null)
                {
                    itemFee = new INV_TBL_INVOICE_ITEM();
                    itemFee.id = Guid.NewGuid();
                    itemFee.pid_invoice = _inv.id;
                    itemFee.is_deleted = 0;
                    itemFee.sequence = 2;
                    itemFee.quantity = 1;
                    itemFee.quantity_type = "pz";
                    itemFee.code = "Rental in Rome";
                    DC_INVOICE.INV_TBL_INVOICE_ITEM.InsertOnSubmit(itemFee);
                    DC_INVOICE.SubmitChanges();
                }
                itemFee.price_tax_id = CashPlace != null && CashPlace.chargeFeeTaxId.objToInt32() > 0 ? CashPlace.chargeFeeTaxId.objToInt32() : 1;
                taxPercentage = taxPercentage = new decimal(0.10);
                currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == itemFee.price_tax_id);
                if (currTax != null)
                {
                    taxPercentage = currTax.taxAmount.objToDecimal();
                }
                itemFee.price_total = _pay.chargeFee;
                itemFee.price_unit = itemFee.price_tf = Math.Round((itemFee.price_total.objToDecimal() / (1 + taxPercentage)), 2);
                itemFee.price_tax = itemFee.price_total - itemFee.price_tf;
                itemFee.description = CashPlace != null ? CashPlace.chargeFeeInvoiceDesc : "Charge Fee";
                DC_INVOICE.SubmitChanges();
                _inv.pr_total += itemFee.price_total;
                _inv.pr_tf += itemFee.price_tf;
                _inv.pr_tax += itemFee.price_tax;
                DC_INVOICE.SubmitChanges();
            }
            else
            {
                DC_INVOICE.INV_TBL_INVOICE_ITEM.DeleteAllOnSubmit(DC_INVOICE.INV_TBL_INVOICE_ITEM.Where(x => x.pid_invoice == _inv.id && x.sequence == 2));
                DC_INVOICE.SubmitChanges();
            }

            //if (isnewinvoice == true)
            //{
            //    if (_inv.cl_loc_country == "Italy")
            //    {
            //        string token = digital_invoice.Fill_data();
            //        ErrorLog.addLog("", "json", token + "");
            //        string response = digital_invoice.Callinvoicefunction(_inv, itemRnt, token);
            //        if (response != "")
            //        {
            //            _inv.responseUniqueId = response;

            //            int counter = _inv.numSentInvoice.objToInt32();
            //            _inv.numSentInvoice = counter + 1;

            //            DC_INVOICE.SubmitChanges();
            //        }
            //    }
            //    else
            //    {
            //        bool _isTest = CommonUtilities.getSYS_SETTING("send_invoice_all") == "true";
            //        if (_isTest)
            //        {

            //            string token = digital_invoice.Fill_data();
            //            ErrorLog.addLog("", "json", token + "");
            //            string response = digital_invoice.Callinvoicefunction(_inv, itemRnt, token);
            //            if (response != "")
            //            {
            //                _inv.responseUniqueId = response;

            //                int counter = _inv.numSentInvoice.objToInt32();
            //                _inv.numSentInvoice = counter + 1;

            //                DC_INVOICE.SubmitChanges();
            //            }
            //        }
            //    }
            //}

        }
        public payment_checkInvoiceWork(INV_TBL_PAYMENT pay, RNT_TBL_RESERVATION res, bool autoStart)
        {
            _pay = pay;
            _res = res;
            if (autoStart)
            {
                //Action<object> action = (object obj) => { doWork(); };
                //AppUtilsTaskScheduler.AddTask(action, "invUtils.payment_checkInvoiceWork pay:" + pay.id + " res:" + pay.rnt_pid_reservation);
                ThreadStart start = new ThreadStart(doWork);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();

            }
        }
    }
    public static bool payment_checkInvoice(INV_TBL_PAYMENT TBL_PAYMENT, RNT_TBL_RESERVATION TBL_RESERVATION)//, bool doThread)
    {
        //if (!doThread) return false;
        payment_checkInvoiceWork _tmp = new payment_checkInvoiceWork(TBL_PAYMENT, TBL_RESERVATION, false);
        _tmp.doWork();
        return true;
    }
    #endregion
}
public class invProps
{
    private static List<dbInvCashTypeLK> tmpCashTypeLK; // refresh Auto
    public static List<dbInvCashTypeLK> CashTypeLK
    {
        get
        {
            if (tmpCashTypeLK == null)
            {
                using (DCmodInvoice dc = new DCmodInvoice())
                    tmpCashTypeLK = dc.dbInvCashTypeLKs.ToList();
            }
            return new List<dbInvCashTypeLK>(tmpCashTypeLK.Select(x => x.Clone()));
        }
        set { tmpCashTypeLK = value; }
    }
    private static List<dbInvCashPlaceLK> tmpCashPlaceLK; // refresh Auto
    public static List<dbInvCashPlaceLK> CashPlaceLK
    {
        get
        {
            if (tmpCashPlaceLK == null)
            {
                using (DCmodInvoice dc = new DCmodInvoice())
                    tmpCashPlaceLK = dc.dbInvCashPlaceLKs.ToList();
            }
            return new List<dbInvCashPlaceLK>(tmpCashPlaceLK.ToList());
        }
        set { tmpCashPlaceLK = value; }
    }
    private static List<dbInvCashDocCaseLK> tmpCashDocCaseLK; // refresh Auto
    public static List<dbInvCashDocCaseLK> CashDocCaseLK
    {
        get
        {
            if (tmpCashDocCaseLK == null)
            {
                using (DCmodInvoice dc = new DCmodInvoice())
                    tmpCashDocCaseLK = dc.dbInvCashDocCaseLKs.ToList();
            }
            return new List<dbInvCashDocCaseLK>(tmpCashDocCaseLK.Select(x => x.Clone()));
        }
        set { tmpCashDocCaseLK = value; }
    }
    private static List<dbInvCashTaxLK> tmpCashTaxLK; // refresh Auto
    public static List<dbInvCashTaxLK> CashTaxLK
    {
        get
        {
            if (tmpCashTaxLK == null)
            {
                using (DCmodInvoice dc = new DCmodInvoice())
                    tmpCashTaxLK = dc.dbInvCashTaxLKs.ToList();
            }
            return new List<dbInvCashTaxLK>(tmpCashTaxLK.Select(x => x.Clone()));
        }
        set { tmpCashTaxLK = value; }
    }
}
public static class invExts
{
    #region Invoice
    public static string itemListHTML(this dbInvInvoiceTBL source, string template)
    {
        string returnValue = "";
        using (DCmodInvoice dc = new DCmodInvoice())
        {
            List<dbInvInvoiceItemTBL> _list = dc.dbInvInvoiceItemTBLs.Where(x => x.pidInvoice == source.id).OrderBy(x => x.sequence).ToList();
            foreach (dbInvInvoiceItemTBL _item in _list)
            {
                string _tmp = template;
                _tmp = _tmp.Replace("#description#", _item.description);
                _tmp = _tmp.Replace("#singleUnitPrice#", _item.singleUnitPrice.objToDecimal().ToString("N2"));
                _tmp = _tmp.Replace("#quantityAmount#", _item.quantityAmount.ToString());
                _tmp = _tmp.Replace("#cashTaxFree#", _item.cashTaxFree.objToDecimal().ToString("N2"));
                _tmp = _tmp.Replace("#cashTaxAmount#", _item.cashTaxAmount.objToDecimal().ToString("N2"));
                _tmp = _tmp.Replace("#cashTotalAmount#", _item.cashTotalAmount.objToDecimal().ToString("N2"));
                returnValue += _tmp;
            }
        }
        return returnValue;
    }
    public static string taxListHTML(this dbInvInvoiceTBL source, string template)
    {
        using (DCmodInvoice dc = new DCmodInvoice())
        {
            return dc.dbInvInvoiceItemTBLs.Where(x => x.pidInvoice == source.id).ToList().taxListHTML(template);
        }
    }
    public static string taxListHTML(this List<dbInvInvoiceItemTBL> source, string template)
    {
        string returnValue = "";
        Dictionary<int, decimal> taxList = new Dictionary<int, decimal>();
        foreach (dbInvInvoiceItemTBL _item in source)
        {
            if (taxList.ContainsKey(_item.cashTaxID.objToInt32()))
            {
                decimal oldValue = taxList[_item.cashTaxID.objToInt32()];
                oldValue += _item.cashTaxAmount.objToDecimal();
                taxList[_item.cashTaxID.objToInt32()] = oldValue;
            }
            else
            {
                taxList.Add(_item.cashTaxID.objToInt32(), _item.cashTaxAmount.Value);
            }
        }
        foreach (KeyValuePair<int, decimal> pair in taxList)
        {
            string _tmp = template;
            _tmp = _tmp.Replace("#taxCode#", invUtils.getCashTax_code(pair.Key, "Iva"));
            _tmp = _tmp.Replace("#taxAmount#", pair.Value.ToString("N2"));
            returnValue += _tmp;
        }
        return returnValue;
    }
    public static void OnChanged(this dbInvInvoiceTBL TMPcurrTBL)
    {
        using (DCmodInvoice dc = new DCmodInvoice())
        {
            dbInvInvoiceTBL currTBL = dc.dbInvInvoiceTBLs.SingleOrDefault(x => x.id == TMPcurrTBL.id);
            if (currTBL == null) return;
            dbInvCashDocumentTBL docTBL = dc.dbInvCashDocumentTBLs.SingleOrDefault(x => x.extType == "invoice" && x.extUid == currTBL.uid);
            if (docTBL == null)
            {
                docTBL = new dbInvCashDocumentTBL();
                docTBL.uid = Guid.NewGuid();
                docTBL.createdDate = currTBL.createdDate;
                docTBL.createdUserID = currTBL.createdUserID;
                docTBL.createdUserNameFull = currTBL.createdUserNameFull;
                dc.Add(docTBL);
                dc.SaveChanges();
                docTBL.code = docTBL.id.ToString().fillString("0", 6, false);
            }
            docTBL.extId = currTBL.id;
            docTBL.extUid = currTBL.uid;
            docTBL.extType = "invoice";
            docTBL.cashInOut = currTBL.cashInOut;
            docTBL.docNum = currTBL.docNum;
            docTBL.docIssueDate = currTBL.docIssueDate;
            docTBL.cashAmount = currTBL.cashTotalAmount;
            docTBL.docCaseId = currTBL.docCaseId;
            docTBL.docCaseCode = currTBL.docCaseCode;

            string itemTemplate = "<tr><td>#description#</td><td>#singleUnitPrice#</td><td> #quantityAmount#</td><td>#cashTaxFree#</td></tr>";
            docTBL.docBody = "<table border=\"0\" cellpadding=\"5\" cellspacing=\"2\"><tr><td><strong>Descrizione</strong></td><td><strong>Prezzo(€)</strong></td><td><strong>Quantità</strong></td><td><strong>Totale(€)</strong></td></tr>" + currTBL.itemListHTML(itemTemplate) + "</table>";

            docTBL.ownerType = currTBL.ownerType;
            docTBL.ownerId = currTBL.ownerId;
            docTBL.ownerNameFull = currTBL.ownerNameFull;
            dc.SaveChanges();
            docTBL.OnChanged();
        }
    }
    public static dbInvInvoiceTBL Clone(this dbInvInvoiceTBL source)
    {
        dbInvInvoiceTBL clone = new dbInvInvoiceTBL();
        clone.id = source.id;
        clone.uid = source.uid;
        clone.cashInOut = source.cashInOut;
        clone.cashTaxFree = source.cashTaxFree;
        clone.cashTaxAmount = source.cashTaxAmount;
        clone.cashTotalAmount = source.cashTotalAmount;
        clone.cashPayed = source.cashPayed;
        clone.cashUnpayed = source.cashUnpayed;
        clone.docCaseId = source.docCaseId;
        clone.docCaseCode = source.docCaseCode;
        clone.docNum = source.docNum;
        clone.docType = source.docType;
        clone.docYear = source.docYear;
        clone.docYearCounter = source.docYearCounter;
        clone.docIssueDate = source.docIssueDate;
        clone.docExpiryDate = source.docExpiryDate;
        clone.notesInner = source.notesInner;
        clone.notesPublic = source.notesPublic;
        clone.createdDate = source.createdDate;
        clone.createdUserID = source.createdUserID;
        clone.createdUserNameFull = source.createdUserNameFull;
        clone.ownerId = source.ownerId;
        clone.ownerUid = source.ownerUid;
        clone.ownerCode = source.ownerCode;
        clone.ownerNameFull = source.ownerNameFull;
        clone.ownerType = source.ownerType;
        clone.owner_docType = source.owner_docType;
        clone.owner_docNum = source.owner_docNum;
        clone.owner_docIssuePlace = source.owner_docIssuePlace;
        clone.owner_docIssueDate = source.owner_docIssueDate;
        clone.owner_docExpiryDate = source.owner_docExpiryDate;
        clone.owner_docVat = source.owner_docVat;
        clone.owner_docCf = source.owner_docCf;
        clone.owner_locCountry = source.owner_locCountry;
        clone.owner_locState = source.owner_locState;
        clone.owner_locCity = source.owner_locCity;
        clone.owner_locAddress = source.owner_locAddress;
        clone.owner_locZipCode = source.owner_locZipCode;
        return clone;
    }
    public static dbInvInvoiceItemTBL Clone(this dbInvInvoiceItemTBL source)
    {
        dbInvInvoiceItemTBL clone = new dbInvInvoiceItemTBL();
        clone.id = source.id;
        clone.sequence = source.sequence;
        clone.pidInvoice = source.pidInvoice;
        clone.description = source.description;
        clone.quantityType = source.quantityType;
        clone.quantityAmount = source.quantityAmount;
        clone.singleUnitPrice = source.singleUnitPrice;
        clone.cashTaxFree = source.cashTaxFree;
        clone.cashTaxAmount = source.cashTaxAmount;
        clone.cashTaxID = source.cashTaxID;
        clone.cashTotalAmount = source.cashTotalAmount;
        return clone;
    }
    #endregion

    #region CashDocument
    public static void OnChanged(this dbInvCashDocumentTBL TMPcurrTBL)
    {
        using (DCmodInvoice dc = new DCmodInvoice())
        {
            dbInvCashDocumentTBL currTBL = dc.dbInvCashDocumentTBLs.SingleOrDefault(x => x.id == TMPcurrTBL.id);
            if (currTBL == null) return;
            List<dbInvCashBookTBL> cashBook = dc.dbInvCashBookTBLs.Where(x => x.pidDocument == currTBL.id).ToList();
            decimal cashPayed = cashBook.Count > 0 ? cashBook.Sum(x => x.cashAmount).objToDecimal() : 0;
            currTBL.cashPayed = cashPayed;
            currTBL.cashUnpayed = currTBL.cashAmount - cashPayed;
            dc.SaveChanges();
        }
    }

    #endregion

    #region CashDocCaseLK
    public static dbInvCashDocCaseLK Clone(this dbInvCashDocCaseLK source)
    {
        dbInvCashDocCaseLK clone = new dbInvCashDocCaseLK();
        clone.id = source.id;
        clone.code = source.code;
        clone.description = source.description;
        clone.viewIn = source.viewIn;
        clone.viewOut = source.viewOut;
        return clone;
    }
    public static void CopyTo(this dbInvCashDocCaseLK source, ref dbInvCashDocCaseLK copyto)
    {
        copyto.code = source.code;
        copyto.description = source.description;
        copyto.viewIn = source.viewIn;
        copyto.viewOut = source.viewOut;
    }
    #endregion

    #region CashType
    public static dbInvCashTypeLK Clone(this dbInvCashTypeLK source)
    {
        dbInvCashTypeLK clone = new dbInvCashTypeLK();
        clone.id = source.id;
        clone.title = source.title;
        clone.placeTypes = source.placeTypes;
        clone.isActive = source.isActive;
        return clone;
    }
    public static void CopyTo(this dbInvCashTypeLK source, ref dbInvCashTypeLK copyto)
    {
        copyto.title = source.title;
        copyto.placeTypes = source.placeTypes;
        copyto.isActive = source.isActive;
    }
    #endregion

    #region CashTax
    public static dbInvCashTaxLK Clone(this dbInvCashTaxLK source)
    {
        dbInvCashTaxLK clone = new dbInvCashTaxLK();
        clone.id = source.id;
        clone.code = source.code;
        clone.taxAmount = source.taxAmount;
        return clone;
    }
    #endregion

    #region OLD
    public static void Create_notaDiCredito(this INV_TBL_INVOICE oldTBL)
    {
        using (magaInvoice_DataContext dc = maga_DataContext.DC_INVOICE)
        {
            InvInvoiceTBL currTBL = dc.InvInvoiceTBL.SingleOrDefault(x => x.docType == "notaDiCredito" && x.refererInvoiceID == oldTBL.id);
            if (currTBL == null)
            {
                currTBL = new InvInvoiceTBL();
                currTBL.uid = Guid.NewGuid();
                currTBL.createdDate = DateTime.Now;
                currTBL.createdUserID = authUtils.CurrentUserID;
                currTBL.createdUserNameFull = authUtils.CurrentUserName;
                currTBL.docIssueDate = DateTime.Now.Date;
                currTBL.docYear = currTBL.docIssueDate.Value.Year;
                currTBL.docType = "notaDiCredito";
                int? _lastCounter = dc.InvInvoiceTBL.Where(x => x.docType == "notaDiCredito" && x.docYear.HasValue && x.docYear == currTBL.docYear && x.docYearCounter.HasValue).Max(x => x.docYearCounter);
                if (_lastCounter == null) _lastCounter = 0;
                currTBL.docYearCounter = _lastCounter + 1;
                currTBL.docNum = "NC" + currTBL.docYear + "-" + (currTBL.docYearCounter + "").fillString("0", 5, false);
                currTBL.docExpiryDate = currTBL.docIssueDate.Value.AddDays(1);
                currTBL.refererInvoiceID = oldTBL.id;
                dc.InvInvoiceTBL.InsertOnSubmit(currTBL);
                dc.SubmitChanges();
            }
            currTBL.docCaseId = 1;
            currTBL.docCaseCode = "Nota di credito";


            currTBL.ownerNameFull = oldTBL.cl_name_full;
            //currTBL.owner_docType = txt_owner_docType.Text;
            //currTBL.owner_docNum = txt_owner_docNum.Text;
            //currTBL.owner_docIssuePlace = txt_owner_docIssuePlace.Text;
            //currTBL.owner_docIssueDate = txt_owner_docIssueDate.Text;
            //currTBL.owner_docExpiryDate = txt_owner_docExpiryDate.Text;
            currTBL.owner_docVat = oldTBL.cl_doc_vat_num;
            currTBL.owner_docCf = oldTBL.cl_doc_cf_num;
            currTBL.owner_locCountry = oldTBL.cl_loc_country;
            currTBL.owner_locState = oldTBL.cl_loc_state;
            currTBL.owner_locCity = oldTBL.cl_loc_city;
            currTBL.owner_locAddress = oldTBL.cl_loc_city;
            currTBL.owner_locZipCode = oldTBL.cl_loc_zip_code;

            // cash
            currTBL.cashInOut = 0;
            currTBL.cashTaxFree = oldTBL.pr_tf;
            currTBL.cashTaxAmount = oldTBL.pr_tax;
            currTBL.cashTotalAmount = oldTBL.pr_total;
            currTBL.cashPayed = 0;
            currTBL.cashUnpayed = 0;

            currTBL.notesInner = "";
            currTBL.notesPublic = "Nota di credito riferimento fattura #" + oldTBL.code + " del " + oldTBL.inv_dtInvoice.formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----");

            dc.SubmitChanges();

            // items
            List<InvInvoiceItemTBL> currlist = dc.InvInvoiceItemTBL.Where(x => x.pidInvoice == currTBL.id).ToList();
            foreach (InvInvoiceItemTBL currItem in currlist)
            {
                dc.InvInvoiceItemTBL.DeleteOnSubmit(currItem);
            }
            dc.SubmitChanges();
            List<INV_TBL_INVOICE_ITEM> _list = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE_ITEM.Where(x => x.pid_invoice == oldTBL.id).ToList();
            foreach (INV_TBL_INVOICE_ITEM _item in _list)
            {
                 InvInvoiceItemTBL _new = new InvInvoiceItemTBL();
                _new.pidInvoice = currTBL.id;
                _new.description = _item.code + " " + _item.description;
                _new.id = Guid.NewGuid();
                _new.sequence = _item.sequence;
                _new.quantityAmount = _item.quantity;
                _new.quantityType = _item.quantity_type;
                _new.cashTotalAmount = _item.price_total;
                _new.cashTaxID = 1;
                _new.cashTaxFree = _item.price_tf;
                _new.cashTaxAmount = _item.price_tax;
                _new.singleUnitPrice = _item.price_unit;
                dc.InvInvoiceItemTBL.InsertOnSubmit(_new);
            }
            dc.SubmitChanges();


        }
    }

    public static string clientDetailsHTML(this INV_TBL_INVOICE source)
    {
        string _tmp = @"
            <table cellpadding=""0"" cellspacing=""0"">
                <tr>
                    <td>
                         <strong style=""color: #fe6634;"">#cl_name_full#</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        #cl_loc_address# 
                    </td>
                </tr>
                <tr>
                    <td>
                        #cl_loc_zip_code#&nbsp;#cl_loc_city##cl_loc_state#
                    </td>
                </tr>
                <tr>
                    <td>
                        #cl_loc_country#
                    </td>
                </tr>
                <tr>
                    <td>
                        #cl_email#
                    </td>
                </tr>
                #cl_doc_cf_num#
                #cl_doc_vat_num#
            </table>
        ";
        _tmp = _tmp.Replace("#cl_name_full#", source.cl_name_full);
        _tmp = _tmp.Replace("#cl_email#", source.cl_email);
        _tmp = _tmp.Replace("#cl_loc_address#", source.cl_loc_address);
        _tmp = _tmp.Replace("#cl_loc_zip_code#", source.cl_loc_zip_code);
        _tmp = _tmp.Replace("#cl_loc_city#", source.cl_loc_city);
        _tmp = _tmp.Replace("#cl_loc_state#", !string.IsNullOrWhiteSpace(source.cl_loc_state) ? "&nbsp;(" + source.cl_loc_state + ")" : "");
        _tmp = _tmp.Replace("#cl_loc_country#", source.cl_loc_country);
        _tmp = _tmp.Replace("#cl_doc_cf_num#", string.IsNullOrEmpty(source.cl_doc_cf_num) ? "" : "<tr><td>CF: " + source.cl_doc_cf_num + "</td></tr>");
        _tmp = _tmp.Replace("#cl_doc_vat_num#", string.IsNullOrEmpty(source.cl_doc_vat_num) ? "" : "<tr><td>VAT: " + source.cl_doc_vat_num + "</td></tr>");

        return _tmp;
    }
    public static string itemsListHTML(this INV_TBL_INVOICE source, string _template)
    {
        string _return = "";
        List<INV_TBL_INVOICE_ITEM> _list = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE_ITEM.Where(x => x.pid_invoice == source.id).ToList();
        foreach (INV_TBL_INVOICE_ITEM _item in _list)
        {
            string _tmp = _template;
            _tmp = _tmp.Replace("#code#", _item.code);
            _tmp = _tmp.Replace("#description#", _item.description);
            _tmp = _tmp.Replace("#price_unit#", _item.price_unit.objToDecimal().ToString("N2"));
            _tmp = _tmp.Replace("#quantity#", _item.quantity.ToString());
            _tmp = _tmp.Replace("#price_tf#", _item.price_tf.objToDecimal().ToString("N2"));
            _return += _tmp;
        }
        return _return;
    }

    public static string taxListHTML(this INV_TBL_INVOICE source, string template)
    {
        return maga_DataContext.DC_INVOICE.INV_TBL_INVOICE_ITEM.Where(x => x.pid_invoice == source.id).ToList().taxListHTML(template);
    }
    public static string taxListHTML(this List<INV_TBL_INVOICE_ITEM> source, string template)
    {
        string returnValue = "";
        Dictionary<int, decimal> taxList = new Dictionary<int, decimal>();
        foreach (INV_TBL_INVOICE_ITEM _item in source)
        {
            if (_item.price_tax_id.objToInt32() == 0) _item.price_tax_id = 1;
            if (taxList.ContainsKey(_item.price_tax_id.objToInt32()))
            {
                decimal oldValue = taxList[_item.price_tax_id.objToInt32()];
                oldValue += _item.price_tax.objToDecimal();
                taxList[_item.price_tax_id.objToInt32()] = oldValue;
            }
            else
            {
                taxList.Add(_item.price_tax_id.objToInt32(), _item.price_tax.objToDecimal());
            }
        }
        foreach (KeyValuePair<int, decimal> pair in taxList)
        {
            string _tmp = template;
            _tmp = _tmp.Replace("#taxCode#", invUtils.getCashTax_code(pair.Key, ""));
            _tmp = _tmp.Replace("#taxPercent#", decimal.Multiply(invUtils.getCashTax_taxAmount(pair.Key), 100).objToInt32() + "");
            _tmp = _tmp.Replace("#taxAmount#", pair.Value.ToString("N2"));
            returnValue += _tmp;
        }
        return returnValue;
    }


    #endregion
}

public class digital_invoice
{

    public static class TipoDocumento
    {
        public static string fattura = "TD01";
        public static string anticiposufattura = "TD02";
        public static string anticiposuparcella = "TD03";
        public static string notadicredito = "TD04";
        public static string notadidebito = "TD05";
        public static string parcella = "TD06";
    }
    public class CommandMessage
    {
        public string token { get; set; }
    }
    public class invoiceResponse
    {
        public string uuid { get; set; }
    }

    public static string Fill_data()
    {
        try
        {
            ErrorLog.addLog("", "myCommandMessage token", "myCommandMessage.token" + "");
            string iCalUrl = "https://api-sandbox.acubeapi.com/login_check";
            if (CommonUtilities.getSYS_SETTING("is_live_cube").objToInt32() == 1)
                iCalUrl = "https://api.acubeapi.com/login_check";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(iCalUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "";
                if (CommonUtilities.getSYS_SETTING("is_live_cube").objToInt32() == 0)
                {
                    json = "{\"email\":\"maurizio.lecce@magarental.com\"," +
                                  "\"password\":\"6N4NyndVZemDejre\"}";
                }
                else
                {
                    json = "{\"email\":\"maurizio.lecce@magarental.com\"," +
                                  "\"password\":\"Napoleone1966@#\"}";
                }

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var response = streamReader.ReadToEnd();
                var myCommandMessage = JsonConvert.DeserializeObject<CommandMessage>(response);
                ErrorLog.addLog("", "myCommandMessage", myCommandMessage.token + "");
                if (myCommandMessage.token != null)
                {
                    return myCommandMessage.token;
                }
                else
                {
                    return "";
                }
                //JArray array = (JArray)ojObject["token"];
                //ErrorLog.addLog("", "array", array + "");
                //string token = array[0];
                //ErrorLog.addLog("", "token", token + "");

            }
        }
        catch (Exception ex)
        {
            ErrorLog.addLog("", "token", ex.Message);
            return "";
        }
    }
    public static string Callinvoicefunction(INV_TBL_INVOICE INVOICE, INV_TBL_INVOICE_ITEM _pay, string token, string tipo_documento)
    {
        try
        {
            if (tipo_documento == "")
            {
                ErrorLog.addLog("", "invUtils.Callinvoicefunction", "tipo_documento is null");
                return "";
            }
            //string iCalUrl = "https://api-sandbox.acubeapi.com/simulate/supplier-invoice";
            //f (CommonUtilities.getSYS_SETTING("is_change_invoice_url").objToInt32() == 1)
            string iCalUrl = "https://api-sandbox.acubeapi.com/invoices";
            if (CommonUtilities.getSYS_SETTING("is_live_cube").objToInt32() == 1)
                iCalUrl = "https://api.acubeapi.com/invoices";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(iCalUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers["Authorization"] = "Bearer " + token;
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ErrorLog.addLog("", "crete invoice", "call invoice 3"); ErrorLog.addLog("", "crete invoice", "crete invoice3");
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json = "";
                bool _isTest = CommonUtilities.getSYS_SETTING("acube_isTest") == "true";
                if (_isTest)
                {

                    json = "{ " +
                                          "  \"fattura_elettronica_header\": {" +
                                          "    \"dati_trasmissione\": {" +
                                          "      \"codice_destinatario\": \"1234567\" " +
                                          "    }," +
                                          "    \"cedente_prestatore\": {" +
                                          "      \"dati_anagrafici\": {" +
                                          "        \"id_fiscale_iva\": {" +
                                          "          \"id_paese\": \"IT\"," +
                                          "          \"id_codice\": \"12345678901\"" +
                                          "        }," +
                                          "        \"anagrafica\": {" +
                                          "          \"denominazione\": \"John Doe\"" +
                                          "        }," +
                                          "        \"regime_fiscale\": \"RF01\"" +
                                          "      }," +
                                          "      \"sede\": {" +
                                          "        \"indirizzo\": \"Via di Qua, 1\"," +
                                          "        \"cap\": \"20145\"," +
                                          "        \"comune\": \"Milano\"," +
                                          "        \"provincia\": \"MI\"," +
                                          "        \"nazione\": \"IT\"" +
                                          "      }" +
                                          "    }," +
                                          "    \"cessionario_committente\": {" +
                                          "      \"dati_anagrafici\": {" +
                                          "       \"id_fiscale_iva\": {" +
                                          "          \"id_paese\": \"IT\"," +
                                          "          \"id_codice\": \"09876543211\"" +
                                          "        }," +
                                          "        \"anagrafica\": {" +
                                          "          \"denominazione\": \"Jane Doe\"" +
                                          "        }" +
                                          "      }," +
                                          "      \"sede\": {" +
                                          "        \"indirizzo\": \"Via di La, 2\"," +
                                          "        \"cap\": \"20145\"," +
                                          "       \"comune\": \"Milano\"," +
                                          "        \"provincia\": \"MI\"," +
                                          "       \"nazione\": \"IT\"" +
                                          "      }" +
                                          "   }" +
                                          "  }," +
                                          " \"fattura_elettronica_body\": [{" +
                                          "    \"dati_generali\": {" +
                                          "      \"dati_generali_documento\": {" +
                                          "       \"tipo_documento\": \"" + tipo_documento + "\"," +
                                          "        \"divisa\": \"EUR\"," +
                                          "       \"data\": \"2018-07-10\"," +
                                          "        \"numero\": \"1\"" +
                                          "      }" +
                                          "    }," +
                                          "    \"dati_beni_servizi\": {" +
                                          "      \"dettaglio_linee\": [{" +
                                          "        \"numero_linea\": 1," +
                                          "       \"descrizione\": \"Descrizione articolo\"," +
                                          "        \"prezzo_unitario\": \"10.00\"," +
                                          "        \"prezzo_totale\": \"10.00\"," +
                                          "        \"aliquota_iva\": \"22.00\"" +
                                          "      }]," +
                                          "      \"dati_riepilogo\": [{" +
                                          "        \"aliquota_iva\": \"22.00\"," +
                                          "        \"imponibile_importo\": \"10.00\"," +
                                          "        \"imposta\": \"2.20\"" +
                                          "      }]" +
                                          "    }" +
                                          "  }]" +
                                          "}";
                }
                else
                {

                    var company = new dbInvCompanyTBL();
                    string codice_destinatario = "";

                    string country_code = "";
                    var country = authProps.CountryLK.SingleOrDefault(x => x.title == INVOICE.cl_loc_country);
                    if (country != null)
                        country_code = country.code;

                    using (DCmodInvoice dc = new DCmodInvoice())
                    {
                        company = dc.dbInvCompanyTBLs.FirstOrDefault();
                    }
                    //if (INVOICE.rnt_reservation_code == "-manuale-")
                    //{
                    //    if (company != null && country_code == "IT")
                    //        codice_destinatario = company.idCodice;
                    //}
                    //else
                    //{
                    if (country_code == "IT")
                        codice_destinatario = INVOICE.idCodice;
                    //}

                    if (string.IsNullOrEmpty(codice_destinatario))
                        codice_destinatario = "0000000";

                    List<INV_TBL_INVOICE_ITEM> source = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE_ITEM.Where(x => x.pid_invoice == INVOICE.id).ToList();

                    string returnValue = "";
                    Dictionary<int, decimal> taxList = new Dictionary<int, decimal>();
                    foreach (INV_TBL_INVOICE_ITEM _item in source)
                    {
                        if (_item.price_tax_id.objToInt32() == 0) _item.price_tax_id = 1;
                        if (taxList.ContainsKey(_item.price_tax_id.objToInt32()))
                        {
                            decimal oldValue = taxList[_item.price_tax_id.objToInt32()];
                            oldValue += _item.price_tax.objToDecimal();
                            taxList[_item.price_tax_id.objToInt32()] = oldValue;
                        }
                        else
                        {
                            ErrorLog.addLog("", "price tax", _item.id + "");
                            ErrorLog.addLog("", "price tax", _item.price_tax + "");
                            ErrorLog.addLog("", "price tax1", _item.price_tax.objToDecimal() + "");
                            taxList.Add(_item.price_tax_id.objToInt32(), _item.price_tax.objToDecimal());
                        }
                    }
                    decimal _taxPercent = 0;
                    decimal taxAmount = 0;
                    foreach (KeyValuePair<int, decimal> pair in taxList)
                    {
                        _taxPercent = decimal.Multiply(invUtils.getCashTax_taxAmount(pair.Key), 100).objToDecimal();
                        taxAmount += pair.Value;
                    }

                    string id_codice = CommonUtilities.getSYS_SETTING("invoice_id_codice");
                    string agent_id_codice = "";
                    string str_agent_id_codice = "";

                    string vat_number = "";

                    string country_code_agent = "";
                    country_code_agent = country_code;

                    if (country_code == "IT")
                        agent_id_codice = INVOICE.cl_doc_cf_num;


                    if (string.IsNullOrEmpty(agent_id_codice))
                        str_agent_id_codice = "";
                    else
                        str_agent_id_codice = "\"codice_fiscale\": \"" + agent_id_codice + "\",";
                    //agent_id_codice = "XXXXXXXXXXX";

                    vat_number = INVOICE.cl_doc_vat_num + "";
                    if (string.IsNullOrEmpty(vat_number))
                        vat_number = "XXXXXXX";

                    string id_fiscale_iva = "\"id_fiscale_iva\": {" +
                                     "        \"id_paese\": \"" + country_code_agent + "\"," +
                                     "          \"id_codice\": \"" + vat_number + "\"" +
                                     "        },";

                    if (vat_number == "XXXXXXX" && country_code == "IT")
                        id_fiscale_iva = "";

                    if (_taxPercent > 0)
                    {
                        json = "{ " +
                                     "  \"fattura_elettronica_header\": {" +
                                     "    \"dati_trasmissione\": {" +
                                     "     \"codice_destinatario\": \"" + codice_destinatario + "\"" +
                                     "    }," +
                                     "    \"cedente_prestatore\": {" +
                                     "      \"dati_anagrafici\": {" +
                                     "        \"id_fiscale_iva\": {" +
                                     "          \"id_paese\": \"IT\"," +
                                     "         \"id_codice\": \"" + id_codice + "\"" +
                                     "        }," +
                                     "        \"anagrafica\": {" +
                                     "          \"denominazione\": \"Rental In Rome S.r.l.\"" +
                                     "        }," +
                                     "        \"regime_fiscale\": \"RF01\"" +
                                     "      }," +
                                     "      \"sede\": {" +
                                     "        \"indirizzo\": \"Via Appia Nuova, 677\"," +
                                     "        \"cap\": \"00179\"," +
                                     "        \"comune\": \"Rome\"," +
                                     "        \"provincia\": \"RM\"," +
                                     "        \"nazione\": \"IT\"" +
                                     "      }" +
                                     "    }," +
                                     "    \"cessionario_committente\": {" +
                                     "      \"dati_anagrafici\": {" + id_fiscale_iva + str_agent_id_codice +
                            //"       \"codice_fiscale\": \"" + agent_id_codice + "\"," +
                                     "        \"anagrafica\": {" +
                                     "          \"denominazione\": \"" + INVOICE.cl_name_full + "\"" +
                                     "        }" +
                                     "      }," +
                                     "      \"sede\": {" +
                                     "        \"indirizzo\": \"" + INVOICE.cl_loc_address + "\"," +
                                     "        \"cap\": \"" + INVOICE.cl_loc_zip_code + "\"," +
                                     "       \"comune\": \"" + INVOICE.cl_loc_city + "\"," +
                            //"        \"provincia\": \"" + INVOICE.cl_loc_province + "\"," +
                                     "       \"nazione\": \"" + country_code + "\"" +
                                     "      }" +
                                     "   }" +
                                     "  }," +
                                     " \"fattura_elettronica_body\": [{" +
                                     "    \"dati_generali\": {" +
                                     "      \"dati_generali_documento\": {" +
                                     "       \"tipo_documento\": \"" + tipo_documento + "\"," +
                                     "        \"divisa\": \"EUR\"," +
                                     "       \"data\": \"" + INVOICE.inv_dtInvoice.formatCustom("#yy#-#mm#-#dd#", 2, "") + "\"," +
                                     "       \"numero\": \"" + INVOICE.inv_counter + "\"," +
                                     "       \"importo_totale_documento\": \"" + Math.Round(INVOICE.pr_total.objToDecimal(), 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"" +
                                     "      }" +
                                     "    }," +
                                     "    \"dati_beni_servizi\": {" +
                                     "      \"dettaglio_linee\": [{" +
                                     "        \"numero_linea\": 1," +
                                     "       \"descrizione\": \"" + _pay.description + "\"," +
                                     "        \"prezzo_unitario\": \"" + Math.Round(INVOICE.pr_tf.objToDecimal(), 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"," +
                                     "        \"prezzo_totale\": \"" + Math.Round(INVOICE.pr_tf.objToDecimal(), 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"," +
                                     "        \"aliquota_iva\": \"" + Math.Round(_taxPercent, 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"" +
                                     "      }]," +
                                     "      \"dati_riepilogo\": [{" +
                                     "        \"aliquota_iva\": \"" + Math.Round(_taxPercent, 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"," +
                                     "        \"imponibile_importo\": \"" + Math.Round(INVOICE.pr_tf.objToDecimal(), 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"," +
                                     "        \"imposta\": \"" + Math.Round(taxAmount, 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"" +
                                     "      }]" +
                                     "    }" +
                                     "  }]" +
                                     "}";
                    }
                    else
                    {
                        json = "{ " +
                                     "  \"fattura_elettronica_header\": {" +
                                     "    \"dati_trasmissione\": {" +
                                     "     \"codice_destinatario\": \"" + codice_destinatario + "\"" +
                                     "    }," +
                                     "    \"cedente_prestatore\": {" +
                                     "      \"dati_anagrafici\": {" +
                                     "        \"id_fiscale_iva\": {" +
                                     "          \"id_paese\": \"IT\"," +
                                     "         \"id_codice\": \"" + id_codice + "\"" +
                                     "        }," +
                                     "        \"anagrafica\": {" +
                                     "          \"denominazione\": \"Rental In Rome S.r.l.\"" +
                                     "        }," +
                                     "        \"regime_fiscale\": \"RF01\"" +
                                     "      }," +
                                     "      \"sede\": {" +
                                     "        \"indirizzo\": \"Via Appia Nuova, 677\"," +
                                     "        \"cap\": \"00179\"," +
                                     "        \"comune\": \"Rome\"," +
                                     "        \"provincia\": \"RM\"," +
                                     "        \"nazione\": \"IT\"" +
                                     "      }" +
                                     "    }," +
                                     "    \"cessionario_committente\": {" +
                                     "      \"dati_anagrafici\": {" + id_fiscale_iva + str_agent_id_codice +
                            //"       \"codice_fiscale\": \"" + agent_id_codice + "\"," +
                                     "        \"anagrafica\": {" +
                                     "          \"denominazione\": \"" + INVOICE.cl_name_full + "\"" +
                                     "        }" +
                                     "      }," +
                                     "      \"sede\": {" +
                                     "        \"indirizzo\": \"" + INVOICE.cl_loc_address + "\"," +
                                     "        \"cap\": \"" + INVOICE.cl_loc_zip_code + "\"," +
                                     "       \"comune\": \"" + INVOICE.cl_loc_city + "\"," +
                            //"        \"provincia\": \"" + INVOICE.cl_loc_province + "\"," +
                                     "       \"nazione\": \"" + country_code + "\"" +
                                     "      }" +
                                     "   }" +
                                     "  }," +
                                     " \"fattura_elettronica_body\": [{" +
                                     "    \"dati_generali\": {" +
                                     "      \"dati_generali_documento\": {" +
                                     "       \"tipo_documento\": \"" + tipo_documento + "\"," +
                                     "        \"divisa\": \"EUR\"," +
                                     "       \"data\": \"" + INVOICE.inv_dtInvoice.formatCustom("#yy#-#mm#-#dd#", 2, "") + "\"," +
                                     "       \"numero\": \"" + INVOICE.inv_counter + "\"," +
                                     "       \"importo_totale_documento\": \"" + Math.Round(INVOICE.pr_total.objToDecimal(), 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"" +
                                     "      }" +
                                     "    }," +
                                     "    \"dati_beni_servizi\": {" +
                                     "      \"dettaglio_linee\": [{" +
                                     "        \"numero_linea\": 1," +
                                     "       \"descrizione\": \"" + _pay.description + "\"," +
                                     "        \"prezzo_unitario\": \"" + Math.Round(INVOICE.pr_tf.objToDecimal(), 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"," +
                                     "        \"prezzo_totale\": \"" + Math.Round(INVOICE.pr_tf.objToDecimal(), 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"," +
                                     "        \"aliquota_iva\": \"" + Math.Round(_taxPercent, 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"," +
                                     "        \"Natura\": \"N4\"" +
                                     "      }]," +
                                     "      \"dati_riepilogo\": [{" +
                                     "        \"aliquota_iva\": \"" + Math.Round(_taxPercent, 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"," +
                                     "         \"Natura\": \"N4\"," +
                                     "        \"imponibile_importo\": \"" + Math.Round(INVOICE.pr_tf.objToDecimal(), 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"," +
                                     "        \"imposta\": \"" + Math.Round(taxAmount, 2).ToString("N2").Replace(".", "").Replace(",", ".") + "\"" +
                                     "      }]" +
                                     "    }" +
                                     "  }]" +
                                     "}";
                    }
                }

                ErrorLog.addLog("", "json", json + "");
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var response = streamReader.ReadToEnd();

                int responseCode = (int)httpResponse.StatusCode;
                ErrorLog.addLog("", "response", response + "");
                ErrorLog.addLog("", "response code", (int)httpResponse.StatusCode + "");

                if (responseCode != 201)
                    addInvoiceError(INVOICE.id, response);

                //if(httpResponse.StatusCode != HttpStatusCode.)

                var myCommandMessage = JsonConvert.DeserializeObject<invoiceResponse>(response);
                if (myCommandMessage != null)
                {
                    ErrorLog.addLog("", "uuid", myCommandMessage.uuid);
                    return myCommandMessage.uuid;
                }

            }
            return "";
        }
        catch (WebException ex)
        {
            ErrorLog.addLog("", "", ex.ToString());
            if (tipo_documento == TipoDocumento.fattura)
            {
                addInvoiceError(INVOICE.id, ex.Message);
            }
            if (tipo_documento == TipoDocumento.notadicredito)
            {
                addInvoiceCreditNotError(INVOICE.id, ex.Message);
            }
            return "";
        }
        catch (Exception ex)
        {
            ErrorLog.addLog("", "", ex.ToString());
            if (tipo_documento == TipoDocumento.fattura)
            {
                addInvoiceError(INVOICE.id, ex.Message);
            }
            if (tipo_documento == TipoDocumento.notadicredito)
            {
                addInvoiceCreditNotError(INVOICE.id, ex.Message);
            }
            return "";
        }
    }

    public static void addInvoiceError(long invoiceId, string error)
    {
        magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;

        INV_TBL_DIGITAL_INVOICE_ERROR currError = new INV_TBL_DIGITAL_INVOICE_ERROR();
        currError.error = error;
        currError.datetime = DateTime.Now;
        currError.pid_invoice = invoiceId;
        DC_INVOICE.INV_TBL_DIGITAL_INVOICE_ERROR.InsertOnSubmit(currError);
        DC_INVOICE.SubmitChanges();
    }

    public static void addInvoiceCreditNotError(long invoiceId, string error)
    {
        magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;

        INV_TBL_DIGITAL_CREDITNOTE_ERROR currError = new INV_TBL_DIGITAL_CREDITNOTE_ERROR();
        currError.error = error;
        currError.datetime = DateTime.Now;
        currError.pid_invoice = invoiceId;
        DC_INVOICE.INV_TBL_DIGITAL_CREDITNOTE_ERRORs.InsertOnSubmit(currError);
        DC_INVOICE.SubmitChanges();
    }
}
