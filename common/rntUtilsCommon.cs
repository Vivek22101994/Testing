using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading;
using System.Runtime.Serialization;
using System.Globalization;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;
using System.Net;

public partial class rntUtils
{
    private class reservation_checkPartPaymentWork
    {
        RNT_TBL_RESERVATION _currTBL;
        INV_TBL_PAYMENT _pay;
        List<INV_TBL_PAYMENT> _listPay;
        magaInvoice_DataContext DC_INVOICE;
        private INV_TBL_PAYMENT createNew()
        {
            INV_TBL_PAYMENT _pay = new INV_TBL_PAYMENT();
            _pay.rnt_pid_reservation = _currTBL.id;
            _pay.rnt_reservation_code = _currTBL.code;
            _pay.pay_cause = "rnt_part";
            _pay.direction = 1;
            _pay.pay_mode = "";
            _pay.pid_invoice = 0;
            _pay.pid_supplier = 0;
            _pay.pid_client = 0;
            _pay.pid_place = 0;
            _pay.description = "";
            _pay.pr_tf = 0;
            _pay.pr_tax = 0;
            _pay.is_complete = 0;
            _pay.pay_pid_txn = 0;
            _pay.pay_txn_gross = 0;
            _pay.pay_date = null;
            _pay.dtExpire = _currTBL.block_expire;
            _pay.creation_pid_user = 1;
            _pay.dtCreation = DateTime.Now;

            _pay.state_pid = 1;
            _pay.state_body = "";
            _pay.state_date = DateTime.Now;
            _pay.state_pid_user = 1;
            _pay.state_subject = "Nuovo pagamento";
            _pay.state_pid_pptxn = 0;
            return _pay;
        }
        public void doWork()
        {
            try
            {
                RNT_TB_ESTATE _est = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
                DC_INVOICE = maga_DataContext.DC_INVOICE;

                // non eliminare pagamenti creati, potrebbe essere che viene eliminato mentre il cliente sta pagando!
                // per gestire meglio elimina tutti pagamenti non completi o con somma da pagare < 0, dopo si creano di nuovo
                //_listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && (x.is_complete != 1 || !x.pr_total.HasValue || x.pr_total < 0)).ToList();
                //DC_INVOICE.INV_TBL_PAYMENT.DeleteAllOnSubmit(_listPay);
                //DC_INVOICE.SubmitChanges();

                // totale pagato
                _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete == 1 && x.direction == 1 && x.pr_total.HasValue).ToList();

                decimal totalPayed = _listPay.Count == 0 ? 0 : _listPay.Sum(x => x.pr_total.Value);
                decimal totalPayedWithInvoice = 0;
                foreach (var tmp in _listPay)
                {
                    totalPayedWithInvoice += (tmp.pr_total.objToDecimal() - tmp.pr_noInvoice.objToDecimal());
                }
                decimal totalInvoice = _currTBL.pr_part_payment_total.objToDecimal() - totalPayedWithInvoice;

                string payPreString = (_currTBL.requestFullPayAccepted == 1) ? "full" : "part"; // stringa per rappresentare tipo pagamento, acconto oppure saldo

                _pay = DC_INVOICE.INV_TBL_PAYMENT.FirstOrDefault(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete != 1);

                // se gia ha pagato piu di quello che doveva, oppure la somma giusta e se non c'e pagamento registrato, esci, niente da fare
                if (totalPayed >= _currTBL.pr_part_forPayment.objToDecimal() && _pay == null)
                    return;

                // se c'e pagamento registrato
                if (_pay != null)
                {
                    // se differenza non pagata > 0 aggiorna il pagamento
                    if ((_currTBL.pr_part_forPayment.objToDecimal() - totalPayed) > 0)
                    {
                        _pay.rnt_reservation_part = totalPayed == 0 ? payPreString : payPreString + "_diff";
                        _pay.pr_total = _currTBL.pr_part_forPayment.objToDecimal() - totalPayed; // IMPORTO = differenza non pagata
                        //if (_est != null && _est.pid_zone == 9)
                        //    _pay.pr_noInvoice = _pay.pr_total > totalInvoice ? (_pay.pr_total - totalInvoice) : 0;
                        //else
                        //    _pay.pr_noInvoice = 0;
                        _pay.pr_noInvoice = _pay.pr_total > totalInvoice ? (_pay.pr_total - totalInvoice) : 0;
                        DC_INVOICE.SubmitChanges();
                        return;
                    }
                    // altrimenti non ha nulla da pagare oppure ha pagato piu di quello che doveva, elimina il pagamento (per sicurezza prendi una lista dei pagamenti non completi, di solito c'e solo 1)
                   // DC_INVOICE.INV_TBL_PAYMENT.DeleteAllOnSubmit(DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == _currTBL.id && x.is_complete != 1));
                   // DC_INVOICE.SubmitChanges();
                    return;
                }
                // se nessun pagamento registrato, registra ed esci
                _pay = createNew();
                _pay.rnt_reservation_part = totalPayed == 0 ? payPreString : payPreString + "_diff";
                _pay.pr_total = _currTBL.pr_part_forPayment.objToDecimal() - totalPayed; // IMPORTO = differenza non pagata
                //if (_est != null && _est.pid_zone == 9)
                //    _pay.pr_noInvoice = _pay.pr_total > totalInvoice ? (_pay.pr_total - totalInvoice) : 0;
                //else
                //    _pay.pr_noInvoice = 0;
                _pay.pr_noInvoice = _pay.pr_total > totalInvoice ? (_pay.pr_total - totalInvoice) : 0;
                DC_INVOICE.INV_TBL_PAYMENT.InsertOnSubmit(_pay);
                DC_INVOICE.SubmitChanges();
                _pay.code = _pay.id.ToString().fillString("0", 7, false);
                DC_INVOICE.SubmitChanges();
                return;
            }
            catch (Exception exc)
            {
                string _ip = "";
                ErrorLog.addLog(_ip, "rntUtils.reservation_checkPartPaymentWork.doWork", exc.ToString());
            }
        }
        public reservation_checkPartPaymentWork(RNT_TBL_RESERVATION currTBL, bool autoStart)
        {
            _currTBL = currTBL;
            if (autoStart)
            {
                //Action<object> action = (object obj) => { doWork(); };
                //AppUtilsTaskScheduler.AddTask(action, "rntUtils.reservation_checkPartPaymentWork resId:" + currTBL.id);

                ThreadStart start = new ThreadStart(doWork);
                Thread t = new Thread(start);
                t.Priority = ThreadPriority.Lowest;
                t.Start();

            }
        }
    }
    public static bool reservation_checkPartPayment(RNT_TBL_RESERVATION TBL_RESERVATION, bool doThread)
    {
        reservation_checkPartPaymentWork _tmp = new reservation_checkPartPaymentWork(TBL_RESERVATION, doThread);
        if (doThread)
            return true;
        _tmp.doWork();
        return true;
    }
    public static List<RNT_TBL_RESERVATION> rntEstate_resList(int IdEstate, DateTime dtStart, DateTime dtEnd, long CurrResId)
    {
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            return dcOld.RNT_TBL_RESERVATION.Where(x => x.id != CurrResId && x.pid_estate == IdEstate //
                                                                         && x.state_pid != 3//
                                                                         && x.dtStart.HasValue //
                                                                         && x.dtEnd.HasValue //
                                                                         && ((x.dtStart.Value.Date <= dtStart && x.dtEnd.Value.Date >= dtEnd) //
                                                                             || (x.dtStart.Value.Date >= dtStart && x.dtStart.Value.Date < dtEnd) //
                                                                             || (x.dtEnd.Value.Date > dtStart && x.dtEnd.Value.Date <= dtEnd))).ToList();
    }
    public static RNT_TBL_RESERVATION rntEstate_isAvailable(int IdEstate, DateTime dtStart, DateTime dtEnd, long CurrResId)
    {
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            return dcOld.RNT_TBL_RESERVATION.FirstOrDefault(x => x.id != CurrResId && x.pid_estate == IdEstate //
                                                                         && x.state_pid != 3//
                                                                         && x.dtStart.HasValue //
                                                                         && x.dtEnd.HasValue //
                                                                         && ((x.dtStart.Value.Date <= dtStart && x.dtEnd.Value.Date >= dtEnd) //
                                                                             || (x.dtStart.Value.Date >= dtStart && x.dtStart.Value.Date < dtEnd) //
                                                                             || (x.dtEnd.Value.Date > dtStart && x.dtEnd.Value.Date <= dtEnd)));
    }
    public static decimal rntEstate_minPrice(int IdEstate)
    {
        RNT_TB_ESTATE estateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
        if (estateTB == null) return 0;
        return estateTB.pr_1_2pax.objToDecimal();
    }

    public static void rntReservation_calculateParts_WL(long currResID, out decimal _pr_total, ref rntExts.PreReservationPrices outPrice, decimal agentWL_total, decimal rentalWL_total)
    {

        decimal payedPart = 0; // somma pagata

        decimal TMPtotal = outPrice.pr_reservation; // prezzo intero della prenotazione

        TMPtotal = TMPtotal - agentWL_total; //deduct Agent's WL increment

        decimal TMPtotal_rental = TMPtotal - rentalWL_total;//deduct Rental's WL increment

        outPrice.pr_part_commission_tf = outPrice.part_percentage * TMPtotal_rental / 100; //calcolo della commissione

        int _vat = CommonUtilities.getSYS_SETTING("rnt_res_part_payment_vat").ToInt32(); // iva della commissione
        outPrice.pr_part_commission_total = (outPrice.pr_part_commission_tf * _vat / 100) + outPrice.pr_part_commission_tf; // aggiunta iva alla commissione

        outPrice.pr_part_commission_total = outPrice.pr_part_commission_total - outPrice.pr_discount_commission; // applica sconto sulla commissione

        outPrice.pr_part_agency_fee = CommonUtilities.getSYS_SETTING("rnt_res_part_payment_agency_fee").ToInt32(); // agency_fee della commissione
        outPrice.pr_part_payment_total = outPrice.pr_part_commission_total + outPrice.pr_part_agency_fee; // aggiunta agency_fee alla commissione

        outPrice.pr_part_owner = TMPtotal_rental - outPrice.pr_part_commission_tf; // saldo = differenza del prezzo intero e la commissione senza Iva
        outPrice.pr_part_owner = outPrice.pr_part_owner - outPrice.pr_discount_owner; // applica lo sconto del saldo
         
        outPrice.prTotalOwner = outPrice.pr_part_owner + 0;
        outPrice.prTotalCommission = outPrice.pr_part_payment_total - outPrice.pr_part_agency_fee + 0;

        outPrice.pr_part_owner += outPrice.srsPrice + outPrice.ecoPrice; // aggiunta Pulizie e accoglienza, paga al arrivo, non deve essere incluso nel calcolo dell'acconto


        _pr_total = outPrice.pr_part_owner + outPrice.pr_part_payment_total; // Totale da pagare = somma dell'acconto e saldo


        long agentID = outPrice.agentID;
        if (agentID > 0)
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntAgentTBL currAgent = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentID);
                if (currAgent != null)
                {
                    TMPtotal = TMPtotal - outPrice.pr_discount_owner - outPrice.pr_discount_commission;
                    // se è un'agenzia
                    decimal agentTotalResPrice;
                    outPrice.agentCommissionPerc = rntUtils.getAgent_discount(outPrice.agentID, currResID, outPrice.agentDiscountType, outPrice.agentCheckDate, outPrice.agentTotalResPrice, TMPtotal, out agentTotalResPrice);
                    outPrice.agentTotalResPrice = agentTotalResPrice;
                    outPrice.agentCommissionPrice = (TMPtotal * outPrice.agentCommissionPerc / 100);
                    if (currAgent.isAgencyFeeApplied.objToInt32() == 0)
                    {
                        outPrice.pr_part_payment_total -= outPrice.pr_part_agency_fee; // togliamo agency fee dalla commissione
                        _pr_total -= outPrice.pr_part_agency_fee; // togliamo agency fee dal totale
                        outPrice.pr_part_agency_fee = 0; // agency_fee non si applica
                    }
                    if (outPrice.agentDiscountNotPayed == 1) // se deve pagare solo la differenza
                    {
                        outPrice.pr_part_payment_total -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
                        _pr_total -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
                    }
                    outPrice.prTotalCommission -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
                }
                else
                    outPrice.agentID = 0;
            }
        outPrice.prTotalCommission = outPrice.prTotalCommission + rentalWL_total;
        outPrice.agentCommissionPrice = outPrice.agentCommissionPrice + agentWL_total;
        outPrice.pr_part_payment_total = outPrice.pr_part_payment_total + agentWL_total + rentalWL_total;
        _pr_total = _pr_total + agentWL_total + rentalWL_total;
        if (outPrice.requestFullPayAccepted == 1) // se abilitato pagamento di saldo
            outPrice.pr_part_forPayment = _pr_total; // somma da pagare = totale
        else
            outPrice.pr_part_forPayment = outPrice.pr_part_payment_total; // somma da pagare = acconto predefinito
    }


    // HomeAway Customizations
    //public static void rntReservation_calculateParts_HA(long currResID, out decimal _pr_total, ref rntExts.PreReservationPrices outPrice)
    //{
    //    decimal payedPart = 0; // somma pagata
    //    //if (currResID != 0)
    //    //{
    //    //    magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
    //    //    List<INV_TBL_PAYMENT> _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == currResID && x.rnt_reservation_part.StartsWith("part") && x.is_complete == 1 && x.direction == 1).ToList();
    //    //    foreach (INV_TBL_PAYMENT _currPay in _listPay)
    //    //    {
    //    //        payedPart += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
    //    //    }
    //    //}

    //    decimal TMPtotal = outPrice.pr_reservation; // prezzo intero della prenotazione

    //    outPrice.pr_part_commission_tf = outPrice.part_percentage * TMPtotal / 100; //calcolo della commissione

    //    int _vat = CommonUtilities.getSYS_SETTING("rnt_res_part_payment_vat").ToInt32(); // iva della commissione
    //    outPrice.pr_part_commission_total = (outPrice.pr_part_commission_tf * _vat / 100) + outPrice.pr_part_commission_tf; // aggiunta iva alla commissione

    //    outPrice.pr_part_commission_total = outPrice.pr_part_commission_total - outPrice.pr_discount_commission; // applica sconto sulla commissione

    //    outPrice.pr_part_agency_fee = CommonUtilities.getSYS_SETTING("rnt_res_part_payment_agency_fee").ToInt32(); // agency_fee della commissione
    //    outPrice.pr_part_payment_total = outPrice.pr_part_commission_total + outPrice.pr_part_agency_fee; // aggiunta agency_fee alla commissione

    //    outPrice.pr_part_owner = TMPtotal - outPrice.pr_part_commission_tf; // saldo = differenza del prezzo intero e la commissione senza Iva
    //    outPrice.pr_part_owner = outPrice.pr_part_owner - outPrice.pr_discount_owner; // applica lo sconto del saldo

    //    outPrice.prTotalOwner = outPrice.pr_part_owner + 0;
    //    outPrice.prTotalCommission = outPrice.pr_part_payment_total - outPrice.pr_part_agency_fee + 0;

    //    outPrice.pr_part_owner += outPrice.srsPrice + outPrice.ecoPrice; // aggiunta Pulizie e accoglienza, paga al arrivo, non deve essere incluso nel calcolo dell'acconto

    //    // abbiamo deciso diversamente
    //    if (outPrice.pr_part_payment_total < payedPart) // se la somma pagata supera acconto calcolato
    //    {
    //        //_pr_part_payment_total = payedPart; // acconto = somma pagata
    //        // calcoli viceversa tutti importi dipendenti
    //        //_pr_part_commission_total = _pr_part_payment_total - _pr_part_agency_fee;
    //        //_pr_part_commission_total = _pr_part_commission_total + pr_discount_commission;
    //    }
    //    _pr_total = outPrice.pr_part_owner + outPrice.pr_part_payment_total; // Totale da pagare = somma dell'acconto e saldo


    //    long agentID = outPrice.agentID;
    //    if (agentID > 0)
    //        using (DCmodRental dc = new DCmodRental())
    //        {
    //            dbRntAgentTBL currAgent = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentID);
    //            if (currAgent != null)
    //            {
    //                TMPtotal = TMPtotal - outPrice.pr_discount_owner - outPrice.pr_discount_commission;
    //                // se è un'agenzia
    //                decimal agentTotalResPrice;
    //                outPrice.agentCommissionPerc = rntUtils.getAgent_discount(outPrice.agentID, currResID, outPrice.agentDiscountType, outPrice.agentCheckDate, outPrice.agentTotalResPrice, TMPtotal, out agentTotalResPrice);
    //                outPrice.agentTotalResPrice = agentTotalResPrice;
    //                outPrice.agentCommissionPrice = (TMPtotal * outPrice.agentCommissionPerc / 100);
    //                if (currAgent.isAgencyFeeApplied.objToInt32() == 0)
    //                {
    //                    outPrice.pr_part_payment_total -= outPrice.pr_part_agency_fee; // togliamo agency fee dalla commissione
    //                    _pr_total -= outPrice.pr_part_agency_fee; // togliamo agency fee dal totale
    //                    outPrice.pr_part_agency_fee = 0; // agency_fee non si applica
    //                }
    //                if (outPrice.agentDiscountNotPayed == 1) // se deve pagare solo la differenza
    //                {
    //                    outPrice.pr_part_payment_total -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
    //                    _pr_total -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
    //                }
    //                outPrice.prTotalCommission -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
    //            }
    //            else
    //                outPrice.agentID = 0;
    //        }
    //    if (outPrice.requestFullPayAccepted == 1) // se abilitato pagamento di saldo
    //        outPrice.pr_part_forPayment = _pr_total; // somma da pagare = totale
    //    else
    //        outPrice.pr_part_forPayment = outPrice.pr_part_payment_total; // somma da pagare = acconto predefinito
    //}
    //public static void rntReservation_calculateParts_WithLogs(long currResID, out decimal _pr_total, ref rntExts.PreReservationPrices outPrice)
    //{
    //    decimal payedPart = 0; // somma pagata
    //    //if (currResID != 0)
    //    //{
    //    //    magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
    //    //    List<INV_TBL_PAYMENT> _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == currResID && x.rnt_reservation_part.StartsWith("part") && x.is_complete == 1 && x.direction == 1).ToList();
    //    //    foreach (INV_TBL_PAYMENT _currPay in _listPay)
    //    //    {
    //    //        payedPart += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
    //    //    }
    //    //}

    //    decimal TMPtotal = outPrice.pr_reservation; // prezzo intero della prenotazione
    //    //ErrorLog.addLog("rntReservation_calculateParts_HA", "pr_reservation", TMPtotal.ToString());

    //    outPrice.pr_part_commission_tf = outPrice.part_percentage * TMPtotal / 100; //calcolo della commissione
    //    //ErrorLog.addLog("pr_part_commission_tf", "pr_part_commission_tf", outPrice.pr_part_commission_tf.ToString());

    //    //ErrorLog.addLog("pr_part_commission_total", "1", outPrice.pr_part_commission_total.ToString());
    //    int _vat = CommonUtilities.getSYS_SETTING("rnt_res_part_payment_vat").ToInt32(); // iva della commissione
    //    outPrice.pr_part_commission_total = (outPrice.pr_part_commission_tf * _vat / 100) + outPrice.pr_part_commission_tf; // aggiunta iva alla commissione

    //    //ErrorLog.addLog("pr_part_commission_total", "2", outPrice.pr_part_commission_total.ToString());
    //    outPrice.pr_part_commission_total = outPrice.pr_part_commission_total - outPrice.pr_discount_commission; // applica sconto sulla commissione

    //    outPrice.pr_part_agency_fee = CommonUtilities.getSYS_SETTING("rnt_res_part_payment_agency_fee").ToInt32(); // agency_fee della commissione

    //    //ErrorLog.addLog("pr_part_payment_total", "2", outPrice.pr_part_payment_total.ToString());
       
    //    outPrice.pr_part_payment_total = outPrice.pr_part_commission_total + outPrice.pr_part_agency_fee; // aggiunta agency_fee alla commissione
    //    //ErrorLog.addLog("pr_part_payment_total", "2", outPrice.pr_part_payment_total.ToString());

    //    //ErrorLog.addLog("pr_part_agency_fee", "2", outPrice.pr_part_agency_fee.ToString());

    //    outPrice.pr_part_owner = TMPtotal;// -outPrice.pr_part_commission_tf; // saldo = differenza del prezzo intero e la commissione senza Iva
    //    outPrice.pr_part_owner = outPrice.pr_part_owner;// -outPrice.pr_discount_owner; // applica lo sconto del saldo
    //    outPrice.pr_part_owner += outPrice.pr_part_agency_fee; // aggiunta Pulizie e accoglienza, paga al arrivo, non deve essere incluso nel calcolo dell'acconto
         
    //    outPrice.prTotalOwner = outPrice.pr_part_owner + 0;
    //    outPrice.prTotalCommission = outPrice.pr_part_payment_total;// -outPrice.pr_part_agency_fee + 0;

    //    outPrice.pr_part_owner += outPrice.srsPrice + outPrice.ecoPrice; // aggiunta Pulizie e accoglienza, paga al arrivo, non deve essere incluso nel calcolo dell'acconto

    //    // abbiamo deciso diversamente
    //    if (outPrice.pr_part_payment_total < payedPart) // se la somma pagata supera acconto calcolato
    //    {
    //        //_pr_part_payment_total = payedPart; // acconto = somma pagata
    //        // calcoli viceversa tutti importi dipendenti
    //        //_pr_part_commission_total = _pr_part_payment_total - _pr_part_agency_fee;
    //        //_pr_part_commission_total = _pr_part_commission_total + pr_discount_commission;
    //    }
    //    _pr_total = outPrice.pr_part_owner + outPrice.pr_part_payment_total; // Totale da pagare = somma dell'acconto e saldo


    //    long agentID = outPrice.agentID;
    //    if (agentID > 0)
    //        using (DCmodRental dc = new DCmodRental())
    //        {
    //            dbRntAgentTBL currAgent = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentID);
    //            if (currAgent != null)
    //            {
    //                TMPtotal = TMPtotal - outPrice.pr_discount_owner - outPrice.pr_discount_commission;
    //                // se è un'agenzia
    //                decimal agentTotalResPrice;
    //                outPrice.agentCommissionPerc = rntUtils.getAgent_discount(outPrice.agentID, currResID, outPrice.agentDiscountType, outPrice.agentCheckDate, outPrice.agentTotalResPrice, TMPtotal, out agentTotalResPrice);
    //                outPrice.agentTotalResPrice = agentTotalResPrice;
    //                outPrice.agentCommissionPrice = (TMPtotal * outPrice.agentCommissionPerc / 100);
    //                //ErrorLog.addLog("calculateParts", "pr_part_payment_total", "6");
                      
    //                if (currAgent.isAgencyFeeApplied.objToInt32() == 0)
    //                {
    //                   // ErrorLog.addLog("calculateParts", "pr_part_payment_total", outPrice.pr_part_payment_total.ToString());
    //                    outPrice.pr_part_payment_total -= outPrice.pr_part_agency_fee; // togliamo agency fee dalla commissione
                      
    //                    _pr_total -= outPrice.pr_part_agency_fee; // togliamo agency fee dal totale
    //                    outPrice.pr_part_agency_fee = 0; // agency_fee non si applica
    //                }
    //                if (outPrice.agentDiscountNotPayed == 1) // se deve pagare solo la differenza
    //                {
    //                    outPrice.pr_part_payment_total -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
    //                    _pr_total -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
    //                }
    //                outPrice.prTotalCommission -= outPrice.agentCommissionPrice; 
    //                // commissione viene scalata dalla somma da pagare subito

    //                _pr_total = outPrice.pr_part_owner + outPrice.pr_part_payment_total;
    //            }
    //            else
    //                outPrice.agentID = 0;
    //        }
    //    if (outPrice.requestFullPayAccepted == 1) // se abilitato pagamento di saldo
    //        outPrice.pr_part_forPayment = _pr_total; // somma da pagare = totale
    //    else
    //        outPrice.pr_part_forPayment = outPrice.pr_part_payment_total; // somma da pagare = acconto predefinito
    //}
   
    public static void rntReservation_calculateParts(long currResID, out decimal _pr_total, ref rntExts.PreReservationPrices outPrice)
    { 
        decimal payedPart = 0; // somma pagata
        //if (currResID != 0)
        //{
        //    magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
        //    List<INV_TBL_PAYMENT> _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == currResID && x.rnt_reservation_part.StartsWith("part") && x.is_complete == 1 && x.direction == 1).ToList();
        //    foreach (INV_TBL_PAYMENT _currPay in _listPay)
        //    {
        //        payedPart += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
        //    }
        //}

        decimal TMPtotal = outPrice.pr_reservation; // prezzo intero della prenotazione
       // ErrorLog.addLog("_priceTotal", "3", TMPtotal.ToString());

        outPrice.pr_part_commission_tf = outPrice.part_percentage * TMPtotal / 100; //calcolo della commissione
       // ErrorLog.addLog("_priceTotal", "4", TMPtotal.ToString());

        int _vat = CommonUtilities.getSYS_SETTING("rnt_res_part_payment_vat").ToInt32(); // iva della commissione
        outPrice.pr_part_commission_total = (outPrice.pr_part_commission_tf * _vat / 100) + outPrice.pr_part_commission_tf; // aggiunta iva alla commissione
       // ErrorLog.addLog("_priceTotal", "5", outPrice.pr_part_commission_total.ToString());

        outPrice.pr_part_commission_total = outPrice.pr_part_commission_total - outPrice.pr_discount_commission; // applica sconto sulla commissione
        //ErrorLog.addLog("_priceTotal", "6", outPrice.pr_part_commission_total.ToString());

        outPrice.pr_part_agency_fee = CommonUtilities.getSYS_SETTING("rnt_res_part_payment_agency_fee").ToInt32(); // agency_fee della commissione
        outPrice.pr_part_payment_total = outPrice.pr_part_commission_total + outPrice.pr_part_agency_fee; // aggiunta agency_fee alla commissione
       // ErrorLog.addLog("_priceTotal", "7", outPrice.pr_part_agency_fee.ToString());
       // ErrorLog.addLog("_priceTotal", "8", outPrice.pr_part_payment_total.ToString());

       // ErrorLog.addLog("_priceTotal", "9.1", outPrice.pr_part_owner.ToString());
        outPrice.pr_part_owner = TMPtotal - outPrice.pr_part_commission_tf; // saldo = differenza del prezzo intero e la commissione senza Iva
       // ErrorLog.addLog("_priceTotal", "9.2", outPrice.pr_part_owner.ToString());
        outPrice.pr_part_owner = outPrice.pr_part_owner - outPrice.pr_discount_owner; // applica lo sconto del saldo
       // ErrorLog.addLog("_priceTotal", "9", outPrice.pr_part_owner.ToString());

        outPrice.prTotalOwner = outPrice.pr_part_owner + 0;
        outPrice.prTotalCommission = outPrice.pr_part_payment_total - outPrice.pr_part_agency_fee + 0;

        //ErrorLog.addLog("_priceTotal", "10", outPrice.prTotalCommission.ToString());

        outPrice.pr_part_owner += outPrice.srsPrice + outPrice.ecoPrice; // aggiunta Pulizie e accoglienza, paga al arrivo, non deve essere incluso nel calcolo dell'acconto
       // ErrorLog.addLog("_priceTotal", "11", outPrice.pr_part_owner.ToString());

        // abbiamo deciso diversamente
        if (outPrice.pr_part_payment_total < payedPart) // se la somma pagata supera acconto calcolato
        {
            //_pr_part_payment_total = payedPart; // acconto = somma pagata
            // calcoli viceversa tutti importi dipendenti
            //_pr_part_commission_total = _pr_part_payment_total - _pr_part_agency_fee;
            //_pr_part_commission_total = _pr_part_commission_total + pr_discount_commission;
        }
        _pr_total = outPrice.pr_part_owner + outPrice.pr_part_payment_total; // Totale da pagare = somma dell'acconto e saldo
       // ErrorLog.addLog("_priceTotal", "12", outPrice.pr_part_owner.ToString());
        //ErrorLog.addLog("_priceTotal", "12", outPrice.pr_part_payment_total.ToString());


        long agentID = outPrice.agentID;
        if (agentID > 0)
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntAgentTBL currAgent = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentID);
                if (currAgent != null)
                {
                    TMPtotal = TMPtotal - outPrice.pr_discount_owner - outPrice.pr_discount_commission;
                    // se è un'agenzia
                    decimal agentTotalResPrice;
                    outPrice.agentCommissionPerc = rntUtils.getAgent_discount(outPrice.agentID, currResID, outPrice.agentDiscountType, outPrice.agentCheckDate, outPrice.agentTotalResPrice, TMPtotal, out agentTotalResPrice);
                    outPrice.agentTotalResPrice = agentTotalResPrice;
                    outPrice.agentCommissionPrice = (TMPtotal * outPrice.agentCommissionPerc / 100);
                    if (currAgent.isAgencyFeeApplied.objToInt32() == 0)
                    {
                        outPrice.pr_part_payment_total -= outPrice.pr_part_agency_fee; // togliamo agency fee dalla commissione
                        _pr_total -= outPrice.pr_part_agency_fee; // togliamo agency fee dal totale
                        outPrice.pr_part_agency_fee = 0; // agency_fee non si applica
                    }
                    if (outPrice.agentDiscountNotPayed == 1) // se deve pagare solo la differenza
                    {
                        outPrice.pr_part_payment_total -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
                        _pr_total -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
                    }
                    outPrice.prTotalCommission -= outPrice.agentCommissionPrice; // commissione viene scalata dalla somma da pagare subito
                }
                else
                    outPrice.agentID = 0;
            }
        if (outPrice.requestFullPayAccepted == 1) // se abilitato pagamento di saldo
            outPrice.pr_part_forPayment = _pr_total; // somma da pagare = totale
        else
            outPrice.pr_part_forPayment = outPrice.pr_part_payment_total; // somma da pagare = acconto predefinito
    }
    public static decimal rntEstate_getPrice(long currResID, int id_estate, ref rntExts.PreReservationPrices outPrice, string IsMedia = "")
    {
        // = 1 : MinStay, MinStay in altissima stagione
        // = 2 : MinStay: Limitazione delle notti per prenotazioni future
        // = 3 : LongTerm: Per apt LongTerm, se la pren superiore 30 notti
        outPrice.outError = 0;
        bool hasVHSeason = false;
        outPrice.ecoPrice = 0;
        outPrice.ecoCount = 0;
        outPrice.srsPrice = 0;
        decimal _priceTotal = 0;
        decimal _priceSubTotal = 0;
        outPrice.pr_part_commission_tf = 0;
        outPrice.pr_part_commission_total = 0;
        outPrice.pr_part_agency_fee = 0;
        outPrice.pr_part_payment_total = 0;
        outPrice.pr_part_owner = 0;
        outPrice.pr_reservation = 0;
        outPrice.priceDetails = new List<rntExts.RNT_estatePriceDetails>();
        decimal prMiddleSeason = 0;
        outPrice.prTotalRate = 0;
        outPrice.prTotalOwner = 0;
        outPrice.prTotalCommission = 0;

        outPrice.prYouAreSaving = 0;
        outPrice.prDiscountSpecialOffer = 0;
        outPrice.prDiscountLastMinute = 0;
        outPrice.prDiscountLongStay = 0;
        outPrice.prDiscountLongRange = 0;
        outPrice.agentCommissionPrice = 0;

        //magaRental_DataContext _dc = maga_DataContext.DC_RENTAL;
        RNT_TB_ESTATE estateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id_estate);
        if (estateTB == null) return 0;
        int nightsTotal = (outPrice.dtEnd.Date - outPrice.dtStart.Date).TotalDays.objToInt32();

        // Controllo per - MinStay

        // Controllo per - MinStay: Limitazione delle notti per prenotazioni future
        if (estateTB.lpb_is == 1)
        {
            DateTime lpbMax = DateTime.Now.AddDays(estateTB.lpb_afterdays.objToInt32());
            if (lpbMax < outPrice.dtStart && nightsTotal < estateTB.lpb_nights_min)
            {
                outPrice.outError = 2;
                return 0;
            }
        }

        // Controllo per - LongTerm: Per apt LongTerm, se la pren superiore 30 notti
        if (estateTB.longTermRent == 1 && nightsTotal >= 30)
        {
            outPrice.outError = 3;
            return 0;
        }
        if (IsMedia != ChnlHomeAwayProps_V411.IdAdMedia)
        {
            // calcola prezzo pulizia
            if (estateTB.eco_ext_clientPay == 1 && estateTB.is_ecopulizie != 1 && estateTB.eco_ext_price.objToDecimal() > 0)
            {
                int TMPnightsTotal = nightsTotal;
                outPrice.ecoPrice += estateTB.eco_ext_price.objToDecimal();
                outPrice.ecoCount += 1;
                if (estateTB.eco_ext_payInDays.objToInt32() > 0)
                    while (TMPnightsTotal > estateTB.eco_ext_payInDays.objToInt32())
                    {
                        TMPnightsTotal -= estateTB.eco_ext_payInDays.objToInt32();
                        outPrice.ecoPrice += estateTB.eco_ext_price.objToDecimal();
                        outPrice.ecoCount += 1;
                    }
            }
            // calcola prezzo accoglienza
            if (estateTB.srs_ext_clientPay == 1 && estateTB.is_srs != 1 && estateTB.srs_ext_price.objToDecimal() > 0)
            {
                outPrice.srsPrice = estateTB.srs_ext_price.objToDecimal();
            }
        }

        int _persExtra = 0;
        if (outPrice.numPersCount > estateTB.pr_basePersons.objToInt32())
            _persExtra = outPrice.numPersCount - estateTB.pr_basePersons.objToInt32();
        DateTime dtStart = outPrice.dtStart;
        DateTime dtEnd = outPrice.dtEnd;
        DateTime dtCurrent = outPrice.dtStart;
        int _nights = 1;

        #region White Label
        decimal agentWL_total = 0;
        decimal rentalWL_total = 0;
        int WL_changeAmount = outPrice.WL_changeAmount;
        int WL_changeIsDiscount = outPrice.WL_changeIsDiscount;
        int WL_changeIsPercentage = outPrice.WL_changeIsPercentage;

        int WL_changeAmount_Agent = outPrice.WL_changeAmount_Agent;
        int WL_changeIsDiscount_Agent = outPrice.WL_changeIsDiscount_Agent;
        int WL_changeIsPercentage_Agent = outPrice.WL_changeIsPercentage_Agent;

        #region
        //dbRntAgentTBL agentTbl = (dbRntAgentTBL)null;
        //if (outPrice.isWL == 1)
        //{
        //    using (DCmodRental dc = new DCmodRental())
        //    {
        //        agentTbl = dc.dbRntAgentTBLs.FirstOrDefault(x => x.id == App.WLAgent.id);
        //    }
        //    if (agentTbl != null)
        //    {
        //        WL_changeAmount = agentTbl.WL_changeAmount.objToInt32();
        //        WL_changeIsDiscount = agentTbl.WL_changeIsDiscount == null ? 0 : agentTbl.WL_changeIsDiscount.objToInt32();
        //        WL_changeIsPercentage = agentTbl.WL_changeIsPercentage == null ? 1 : agentTbl.WL_changeIsPercentage.objToInt32();

        //        WL_changeAmount_Agent = agentTbl.WL_changeAmount_Agent.objToInt32();
        //        WL_changeIsDiscount_Agent = agentTbl.WL_changeIsDiscount_Agent == null ? 0 : agentTbl.WL_changeIsDiscount_Agent.objToInt32();
        //        WL_changeIsPercentage_Agent = agentTbl.WL_changeIsPercentage_Agent == null ? 1 : agentTbl.WL_changeIsPercentage_Agent.objToInt32();
        //    }
        //}  
        #endregion
        #endregion

        List<RNT_TB_SPECIAL_OFFER> _specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pid_estate == id_estate && x.is_active == 1).ToList();
        var seasonGroup = estateTB.pidSeasonGroup.objToInt32();
        var seasonDateList = new List<dbRntSeasonDatesTBL>();
        using (DCmodRental dc = new DCmodRental())
            seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pidSeasonGroup == seasonGroup).ToList();

        if (hasVHSeason && nightsTotal < estateTB.nights_minVHSeason)
        {
            outPrice.outError = 1;
            return 0;
        }
        else
        {
            if (nightsTotal < estateTB.nights_min.objToInt32())
            {
                outPrice.outError = 4;
                return 0;
            }
        }

        while (dtCurrent < outPrice.dtEnd)
        {
            decimal _price = 0;
            decimal _priceOpt = 0;
            decimal _priceCurr = 0;
            decimal _priceCurrOpt = 0;
            var seasonDate = seasonDateList.FirstOrDefault(x => x.dtEnd >= dtCurrent && x.dtStart <= dtCurrent);
            if (seasonDate == null) return 0;
            //if (isLog)
            //{
            //    ErrorLog.addLog("seasonDate", "seasonDate", seasonDate.id.ToString());
            //}
            if (seasonDate.pidPeriod == 1)
            {
                _price = estateTB.pr_1_2pax.objToDecimal();
                _priceOpt = estateTB.pr_1_opt.objToDecimal();
            }
            if (seasonDate.pidPeriod == 2)
            {
                _price = estateTB.pr_2_2pax.objToDecimal();
                _priceOpt = estateTB.pr_2_opt.objToDecimal();
                //hasVHSeason = true;
            }
            if (seasonDate.pidPeriod == 3)
            {
                _price = estateTB.pr_3_2pax.objToDecimal();
                _priceOpt = estateTB.pr_3_opt.objToDecimal();
                hasVHSeason = true;
            }
            if (seasonDate.pidPeriod == 4)
            {
                _price = estateTB.pr_4_2pax.objToDecimal();
                _priceOpt = estateTB.pr_4_opt.objToDecimal();

            }
            if (_price != 0)
            {
                _priceCurr = _price;
                _priceCurrOpt = (_priceOpt * _persExtra);
            }
            else
            {
                _priceCurr = 0;
                _priceCurrOpt = 0;
            }
            if (_priceCurr == 0) return 0;


            #region White Label & Increase Price
            if (outPrice.isWL == 1)
            {
                decimal finalChangeAmt = 0;
                if (_priceCurr != 0)
                {
                    //Apply Rental's Increment
                    if (WL_changeIsPercentage == 1)
                    {
                        finalChangeAmt = ((_priceCurr * WL_changeAmount) / 100);
                    }
                    else
                    {
                        finalChangeAmt = WL_changeAmount;
                    }
                    finalChangeAmt = (WL_changeIsDiscount == 1) ? -finalChangeAmt : finalChangeAmt;
                    _priceCurr = _priceCurr + finalChangeAmt;
                    rentalWL_total = rentalWL_total + finalChangeAmt;

                    //Apply Agent's Increment
                    finalChangeAmt = 0;
                    if (WL_changeIsPercentage_Agent == 1)
                    {
                        finalChangeAmt = ((_priceCurr * WL_changeAmount_Agent) / 100);
                    }
                    else
                    {
                        finalChangeAmt = WL_changeAmount_Agent;
                    }
                    finalChangeAmt = (WL_changeIsDiscount_Agent == 1) ? -finalChangeAmt : finalChangeAmt;
                    _priceCurr = _priceCurr + finalChangeAmt;
                    agentWL_total = agentWL_total + finalChangeAmt;
                }
                if (_priceCurrOpt != 0)
                {
                    //Apply Rental's Increment
                    if (WL_changeIsPercentage == 1)
                    {
                        finalChangeAmt = ((_priceCurrOpt * WL_changeAmount) / 100);
                    }
                    else
                    {
                        finalChangeAmt = WL_changeAmount;
                    }
                    finalChangeAmt = (WL_changeIsDiscount == 1) ? -finalChangeAmt : finalChangeAmt;
                    _priceCurrOpt = _priceCurrOpt + finalChangeAmt;
                    rentalWL_total = rentalWL_total + finalChangeAmt;

                    //Apply Agent's Increment
                    finalChangeAmt = 0;
                    if (WL_changeIsPercentage_Agent == 1)
                    {
                        finalChangeAmt = ((_priceCurrOpt * WL_changeAmount_Agent) / 100);
                    }
                    else
                    {
                        finalChangeAmt = WL_changeAmount_Agent;
                    }
                    finalChangeAmt = (WL_changeIsDiscount_Agent == 1) ? -finalChangeAmt : finalChangeAmt;
                    _priceCurrOpt = _priceCurrOpt + finalChangeAmt;
                    agentWL_total = agentWL_total + finalChangeAmt;
                }
            }
            #endregion

            decimal currPriceTotal = _priceCurr + _priceCurrOpt;
            //if (isLog)
            //{
            //    ErrorLog.addLog("_priceTotal", "0.1", _priceTotal.ToString());
            //    ErrorLog.addLog("_priceTotal", "0.21", _priceCurr.ToString());
            //    ErrorLog.addLog("_priceTotal", "0.2", _priceCurrOpt.ToString());
            //    ErrorLog.addLog("_priceTotal", "0.2", currPriceTotal.ToString());
            //    ErrorLog.addLog("_priceTotal", "0.3", outPrice.prTotalRate.ToString());

            //}
            outPrice.prTotalRate += currPriceTotal;
            //if (isLog)
            //{ ErrorLog.addLog("_priceTotal", "0.4", outPrice.prTotalRate.ToString()); }
          
            // offerta speciale
            if (IsMedia.ToLower() != ChnlHomeAwayProps_V411.IdAdMedia.ToLower())
            {
                RNT_TB_SPECIAL_OFFER _specialOffer = _specialOfferList.FirstOrDefault(x => x.dtStart <= dtCurrent && x.dtEnd >= dtCurrent);

                if (_specialOffer != null && _specialOffer.pr_discount.objToInt32() > 0 && outPrice.isWL == 0)
                {
                    decimal _specialOfferDiscount = currPriceTotal * _specialOffer.pr_discount.objToInt32() / 100;
                    outPrice.prDiscountSpecialOffer += _specialOfferDiscount;
                    currPriceTotal = currPriceTotal - _specialOfferDiscount;
                    outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 3, -_specialOfferDiscount, 0, "" + dtCurrent.formatCustom("#dd#/#mm#/#yy#", CurrentLang.ID, "") + " (-" + _specialOffer.pr_discount.objToInt32() + "%)"));
                }
            }
                
   
                //if (isLog)
                //{
                //    ErrorLog.addLog("_priceTotal", "0.51", _priceSubTotal.ToString());
                //}
            _priceSubTotal += currPriceTotal;
            //if (isLog)
            //{
            //    ErrorLog.addLog("_priceTotal", "0.5", _priceSubTotal.ToString());
            //}
          
            outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 1, _priceCurr, _priceCurrOpt, CurrentSource.rntPeriod_title(seasonDate.pidPeriod, CurrentLang.ID, "")));
           
                if (estateTB.pr_dcSUsed == 1 && _nights == 7 && estateTB.pr_discount7days.objToInt32() > 0 && outPrice.isWL == 0)
                {
                    decimal _discount = _priceSubTotal * estateTB.pr_discount7days.objToInt32() / 100;
                    _discount = _discount.customRound(false);
                    outPrice.prDiscountLongStay += _discount;
                    outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lbl_discount7daysDesc", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_discount7days.objToInt32());
                    _priceTotal += _priceSubTotal - _discount;
                    _priceSubTotal = 0;
                    _nights = 0;
                    outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "" + dtCurrent.AddDays(-6).formatCustom("#dd#/#mm#/#yy#", CurrentLang.ID, "") + " - " + dtCurrent.formatCustom("#dd#/#mm#/#yy#", CurrentLang.ID, "") + " (-5%)"));
                }
          
            dtCurrent = dtCurrent.AddDays(1);
            _nights++;

        }
        _priceTotal += _priceSubTotal;
        //if (isLog)
        //{
        //    ErrorLog.addLog("_priceTotal", "0.61", _priceTotal.ToString());

        //    ErrorLog.addLog("_priceTotal", "0.6", _priceSubTotal.ToString());
        //}
          
        //ErrorLog.addLog("_priceTotal", "3", _priceTotal.ToString());
        if (outPrice.ecoPrice > 0)
            outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 2, outPrice.ecoPrice, 0, "Cleaning service X" + outPrice.ecoCount));
        if (outPrice.srsPrice > 0)
            outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 2, outPrice.srsPrice, 0, "Welcome service"));

        if (estateTB.pr_dcSUsed == 2 && outPrice.isWL == 0)
        {
            //ErrorLog.addLog("_discount", "4", "_discount");
            decimal _discount;
            if (estateTB.pr_dcS2_7_inDays > 0 && nightsTotal > estateTB.pr_dcS2_7_inDays && estateTB.pr_dcS2_7_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_7_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_7_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_7_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_7_inDays + " nights -" + estateTB.pr_dcS2_7_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_6_inDays > 0 && nightsTotal > estateTB.pr_dcS2_6_inDays && estateTB.pr_dcS2_6_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_6_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_6_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_6_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_6_inDays + " nights -" + estateTB.pr_dcS2_6_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_5_inDays > 0 && nightsTotal > estateTB.pr_dcS2_5_inDays && estateTB.pr_dcS2_5_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_5_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_5_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_5_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_5_inDays + " nights -" + estateTB.pr_dcS2_5_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_4_inDays > 0 && nightsTotal > estateTB.pr_dcS2_4_inDays && estateTB.pr_dcS2_4_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_4_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_4_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_4_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_4_inDays + " nights -" + estateTB.pr_dcS2_4_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_3_inDays > 0 && nightsTotal > estateTB.pr_dcS2_3_inDays && estateTB.pr_dcS2_3_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_3_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_3_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_3_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_3_inDays + " nights -" + estateTB.pr_dcS2_3_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_2_inDays > 0 && nightsTotal > estateTB.pr_dcS2_2_inDays && estateTB.pr_dcS2_2_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_2_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_2_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_2_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_2_inDays + " nights -" + estateTB.pr_dcS2_2_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_1_inDays > 0 && nightsTotal > estateTB.pr_dcS2_1_inDays && estateTB.pr_dcS2_1_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_1_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_1_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_1_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_1_inDays + " nights -" + estateTB.pr_dcS2_1_percent.objToInt32() + "%"));
            }
           
                
            
        }
       //ErrorLog.addLog("_priceTotal", "4", _priceTotal.ToString());
        outPrice.pr_reservation = _priceTotal;
        //if (isLog)
        //{
        //    ErrorLog.addLog("_priceTotal", "4", outPrice.pr_reservation.ToString());
        //}
        if (outPrice.isWL == 1)
        {
            rntReservation_calculateParts_WL(currResID, out _priceTotal, ref outPrice, agentWL_total, rentalWL_total);
        }
        else
        {
            //if (isLog)
            //{
            //    ErrorLog.addLog("_priceTotal", "1", _priceTotal.ToString());
            //}
            rntReservation_calculateParts(currResID, out _priceTotal, ref outPrice);
            //if (isLog)
            //{
            //    ErrorLog.addLog("_priceTotal", "2", _priceTotal.ToString());
            //}
        }

        if (IsMedia == ChnlHomeAwayProps_V411.IdAdMedia)
        {
            using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
            {
                dbRntChnlHomeAwayEstateTB currHAEstate = dcChnl.dbRntChnlHomeAwayEstateTBs.SingleOrDefault(x => x.id == id_estate);
                if (currHAEstate != null)
                {
                    decimal changeAmountGeneric = 0;
                    if (currHAEstate.changeIsDiscount >= 0 && currHAEstate.changeAmount > 0)
                    {
                        changeAmountGeneric = (currHAEstate.changeIsPercentage == 1) ? (_priceTotal * currHAEstate.changeAmount.objToInt32() / 100) : currHAEstate.changeAmount.objToInt32();
                        if (currHAEstate.changeIsDiscount == 1) { changeAmountGeneric = -changeAmountGeneric; }
                    }
                    _priceTotal = _priceTotal + changeAmountGeneric;
                }
            }
        }

        outPrice.prTotal = _priceTotal;
        return _priceTotal;
    }
    public static decimal rntEstate_getPrice_RU(long currResID, int id_estate, ref rntExts.PreReservationPrices outPrice)
    {
        // = 1 : MinStay, MinStay in altissima stagione
        // = 2 : MinStay: Limitazione delle notti per prenotazioni future
        // = 3 : LongTerm: Per apt LongTerm, se la pren superiore 30 notti
        outPrice.outError = 0;
        bool hasVHSeason = false;
        outPrice.ecoPrice = 0;
        outPrice.ecoCount = 0;
        outPrice.srsPrice = 0;
        decimal _priceTotal = 0;
        decimal _priceSubTotal = 0;
        outPrice.pr_part_commission_tf = 0;
        outPrice.pr_part_commission_total = 0;
        outPrice.pr_part_agency_fee = 0;
        outPrice.pr_part_payment_total = 0;
        outPrice.pr_part_owner = 0;
        outPrice.pr_reservation = 0;
        outPrice.priceDetails = new List<rntExts.RNT_estatePriceDetails>();
        decimal prMiddleSeason = 0;
        outPrice.prTotalRate = 0;
        outPrice.prTotalOwner = 0;
        outPrice.prTotalCommission = 0;

        outPrice.prYouAreSaving = 0;
        outPrice.prDiscountSpecialOffer = 0;
        outPrice.prDiscountLastMinute = 0;
        outPrice.prDiscountLongStay = 0;
        outPrice.prDiscountLongRange = 0;
        outPrice.agentCommissionPrice = 0;

        //magaRental_DataContext _dc = maga_DataContext.DC_RENTAL;
        RNT_TB_ESTATE estateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id_estate);
        if (estateTB == null) return 0;
        int nightsTotal = (outPrice.dtEnd.Date - outPrice.dtStart.Date).TotalDays.objToInt32();

        DateTime dtStart = outPrice.dtStart;
        DateTime dtEnd = outPrice.dtEnd;
        DateTime dtCurrent = outPrice.dtStart;

        #region check min stay for RU
        int min_nights = estateTB.nights_min.objToInt32();
        var firstCloseDay_RU = (dbRntChnlRentalsUnitedEstateClosedDatesRL)null;
        using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
        {
            firstCloseDay_RU = dcChnl.dbRntChnlRentalsUnitedEstateClosedDatesRLs.FirstOrDefault(x => x.pidEstate == estateTB.id && x.changeDate == dtCurrent && x.minStay > 0);
            if (firstCloseDay_RU != null)
                min_nights = firstCloseDay_RU.minStay;
        }
        #endregion

        // Controllo per - MinStay
        if (nightsTotal < min_nights)
        {
            outPrice.outError = 1;
            return 0;
        }

        // Controllo per - MinStay: Limitazione delle notti per prenotazioni future
        if (estateTB.lpb_is == 1)
        {
            DateTime lpbMax = DateTime.Now.AddDays(estateTB.lpb_afterdays.objToInt32());
            if (lpbMax < outPrice.dtStart && nightsTotal < estateTB.lpb_nights_min)
            {
                outPrice.outError = 2;
                return 0;
            }
        }

        // Controllo per - LongTerm: Per apt LongTerm, se la pren superiore 30 notti
        if (estateTB.longTermRent == 1 && nightsTotal >= 30)
        {
            outPrice.outError = 3;
            return 0;
        }

        // calcola prezzo pulizia
        if (estateTB.eco_ext_clientPay == 1 && estateTB.is_ecopulizie != 1 && estateTB.eco_ext_price.objToDecimal() > 0)
        {
            int TMPnightsTotal = nightsTotal;
            outPrice.ecoPrice += estateTB.eco_ext_price.objToDecimal();
            outPrice.ecoCount += 1;
            if (estateTB.eco_ext_payInDays.objToInt32() > 0)
                while (TMPnightsTotal > estateTB.eco_ext_payInDays.objToInt32())
                {
                    TMPnightsTotal -= estateTB.eco_ext_payInDays.objToInt32();
                    outPrice.ecoPrice += estateTB.eco_ext_price.objToDecimal();
                    outPrice.ecoCount += 1;
                }
        }
        // calcola prezzo accoglienza
        if (estateTB.srs_ext_clientPay == 1 && estateTB.is_srs != 1 && estateTB.srs_ext_price.objToDecimal() > 0)
        {
            outPrice.srsPrice = estateTB.srs_ext_price.objToDecimal();
        }

        int _persExtra = 0;
        if (outPrice.numPersCount > estateTB.pr_basePersons.objToInt32())
            _persExtra = outPrice.numPersCount - estateTB.pr_basePersons.objToInt32();

        int _nights = 1;

        #region White Label
        decimal agentWL_total = 0;
        decimal rentalWL_total = 0;
        int WL_changeAmount = outPrice.WL_changeAmount;
        int WL_changeIsDiscount = outPrice.WL_changeIsDiscount;
        int WL_changeIsPercentage = outPrice.WL_changeIsPercentage;

        int WL_changeAmount_Agent = outPrice.WL_changeAmount_Agent;
        int WL_changeIsDiscount_Agent = outPrice.WL_changeIsDiscount_Agent;
        int WL_changeIsPercentage_Agent = outPrice.WL_changeIsPercentage_Agent;
        #endregion

        List<RNT_TB_SPECIAL_OFFER> _specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pid_estate == id_estate && x.is_active == 1).ToList();
        var seasonGroup = estateTB.pidSeasonGroup.objToInt32();
        var seasonDateList = new List<dbRntSeasonDatesTBL>();
        using (DCmodRental dc = new DCmodRental())
            seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pidSeasonGroup == seasonGroup).ToList();

        dbRntChnlRentalsUnitedEstateTB currRUEstate = (dbRntChnlRentalsUnitedEstateTB)null;
        List<dbRntChnlRentalsUnitedEstateRateChangesRL> lstEstateRateChanges_RU = new List<dbRntChnlRentalsUnitedEstateRateChangesRL>();

        using (DCchnlRentalsUnited dcRU = new DCchnlRentalsUnited())
        {
            currRUEstate = dcRU.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == id_estate);
            lstEstateRateChanges_RU = dcRU.dbRntChnlRentalsUnitedEstateRateChangesRLs.Where(x => x.pidEstate == id_estate && x.changeDate >= dtStart && x.changeDate < dtEnd).ToList();
        }
        while (dtCurrent < outPrice.dtEnd)
        {
            decimal _price = 0;
            decimal _priceOpt = 0;
            decimal _priceCurr = 0;
            decimal _priceCurrOpt = 0;
            var seasonDate = seasonDateList.FirstOrDefault(x => x.dtEnd >= dtCurrent && x.dtStart <= dtCurrent);
            if (seasonDate == null) return 0;
            if (seasonDate.pidPeriod == 1)
            {
                _price = estateTB.pr_1_2pax.objToDecimal();
                _priceOpt = estateTB.pr_1_opt.objToDecimal();
            }
            if (seasonDate.pidPeriod == 2)
            {
                _price = estateTB.pr_2_2pax.objToDecimal();
                _priceOpt = estateTB.pr_2_opt.objToDecimal();
            }
            if (seasonDate.pidPeriod == 3)
            {
                _price = estateTB.pr_3_2pax.objToDecimal();
                _priceOpt = estateTB.pr_3_opt.objToDecimal();
                hasVHSeason = true;
            }
            if (seasonDate.pidPeriod == 4)
            {
                _price = estateTB.pr_4_2pax.objToDecimal();
                _priceOpt = estateTB.pr_4_opt.objToDecimal();
               // hasVHSeason = true;
            }
            if (_price != 0)
            {
                _priceCurr = _price;
                _priceCurrOpt = (_priceOpt * _persExtra);
            }
            else
            {
                _priceCurr = 0;
                _priceCurrOpt = 0;
            }
            if (_priceCurr == 0) return 0;

            #region White Label & Increase Price
            if (outPrice.isWL == 1)
            {
                decimal finalChangeAmt = 0;
                if (_priceCurr != 0)
                {
                    //Apply Rental's Increment
                    if (WL_changeIsPercentage == 1)
                    {
                        finalChangeAmt = ((_priceCurr * WL_changeAmount) / 100);
                    }
                    else
                    {
                        finalChangeAmt = WL_changeAmount;
                    }
                    finalChangeAmt = (WL_changeIsDiscount == 1) ? -finalChangeAmt : finalChangeAmt;
                    _priceCurr = _priceCurr + finalChangeAmt;
                    rentalWL_total = rentalWL_total + finalChangeAmt;

                    //Apply Agent's Increment
                    finalChangeAmt = 0;
                    if (WL_changeIsPercentage_Agent == 1)
                    {
                        finalChangeAmt = ((_priceCurr * WL_changeAmount_Agent) / 100);
                    }
                    else
                    {
                        finalChangeAmt = WL_changeAmount_Agent;
                    }
                    finalChangeAmt = (WL_changeIsDiscount_Agent == 1) ? -finalChangeAmt : finalChangeAmt;
                    _priceCurr = _priceCurr + finalChangeAmt;
                    agentWL_total = agentWL_total + finalChangeAmt;
                }
                if (_priceCurrOpt != 0)
                {
                    //Apply Rental's Increment
                    if (WL_changeIsPercentage == 1)
                    {
                        finalChangeAmt = ((_priceCurrOpt * WL_changeAmount) / 100);
                    }
                    else
                    {
                        finalChangeAmt = WL_changeAmount;
                    }
                    finalChangeAmt = (WL_changeIsDiscount == 1) ? -finalChangeAmt : finalChangeAmt;
                    _priceCurrOpt = _priceCurrOpt + finalChangeAmt;
                    rentalWL_total = rentalWL_total + finalChangeAmt;

                    //Apply Agent's Increment
                    finalChangeAmt = 0;
                    if (WL_changeIsPercentage_Agent == 1)
                    {
                        finalChangeAmt = ((_priceCurrOpt * WL_changeAmount_Agent) / 100);
                    }
                    else
                    {
                        finalChangeAmt = WL_changeAmount_Agent;
                    }
                    finalChangeAmt = (WL_changeIsDiscount_Agent == 1) ? -finalChangeAmt : finalChangeAmt;
                    _priceCurrOpt = _priceCurrOpt + finalChangeAmt;
                    agentWL_total = agentWL_total + finalChangeAmt;
                }
            }
            #endregion

            decimal currPriceTotal = _priceCurr + _priceCurrOpt;

            //Change for RU price 
            decimal changeAmountRU = 0;
            var rateChange = lstEstateRateChanges_RU.FirstOrDefault(x => x.changeDate == dtCurrent);
            if (rateChange != null && rateChange.changeIsDiscount.objToInt32() >= 0 && rateChange.changeAmount.objToInt32() > 0)
            {
                changeAmountRU = (rateChange.changeIsPercentage == 1) ? (currPriceTotal * rateChange.changeAmount.objToInt32() / 100) : rateChange.changeAmount.objToInt32();
                if (rateChange.changeIsDiscount == 1) { changeAmountRU = -changeAmountRU; }
            }
            else if (currRUEstate != null && currRUEstate.changeIsDiscount.objToInt32() >= 0 && currRUEstate.changeAmount.objToInt32() > 0)
            {
                changeAmountRU = (currRUEstate.changeIsPercentage == 1) ? (currPriceTotal * currRUEstate.changeAmount.objToInt32() / 100) : currRUEstate.changeAmount.objToInt32();
                if (currRUEstate.changeIsDiscount == 1) { changeAmountRU = -changeAmountRU; }
            }

            currPriceTotal = currPriceTotal + changeAmountRU;
            outPrice.prTotalRate += currPriceTotal;

            // offerta speciale
            RNT_TB_SPECIAL_OFFER _specialOffer = _specialOfferList.FirstOrDefault(x => x.dtStart <= dtCurrent && x.dtEnd >= dtCurrent);
            if (_specialOffer != null && _specialOffer.pr_discount.objToInt32() > 0 && outPrice.isWL == 0)
            {
                decimal _specialOfferDiscount = currPriceTotal * _specialOffer.pr_discount.objToInt32() / 100;
                outPrice.prDiscountSpecialOffer += _specialOfferDiscount;
                currPriceTotal = currPriceTotal - _specialOfferDiscount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 3, -_specialOfferDiscount, 0, "" + dtCurrent.formatCustom("#dd#/#mm#/#yy#", CurrentLang.ID, "") + " (-" + _specialOffer.pr_discount.objToInt32() + "%)"));
            }
            _priceSubTotal += currPriceTotal;
            outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 1, _priceCurr, _priceCurrOpt, CurrentSource.rntPeriod_title(seasonDate.pidPeriod, CurrentLang.ID, "")));

            if (estateTB.pr_dcSUsed == 1 && _nights == 7 && estateTB.pr_discount7days.objToInt32() > 0 && outPrice.isWL == 0)
            {
                decimal _discount = _priceSubTotal * estateTB.pr_discount7days.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay += _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lbl_discount7daysDesc", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_discount7days.objToInt32());
                _priceTotal += _priceSubTotal - _discount;
                _priceSubTotal = 0;
                _nights = 0;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "" + dtCurrent.AddDays(-6).formatCustom("#dd#/#mm#/#yy#", CurrentLang.ID, "") + " - " + dtCurrent.formatCustom("#dd#/#mm#/#yy#", CurrentLang.ID, "") + " (-5%)"));
            }
            dtCurrent = dtCurrent.AddDays(1);
            _nights++;
            if (hasVHSeason && nightsTotal < estateTB.nights_minVHSeason)
            {
                outPrice.outError = 1;
                return 0;
            }
        }
        _priceTotal += _priceSubTotal;
        if (outPrice.ecoPrice > 0)
            outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 2, outPrice.ecoPrice, 0, "Cleaning service X" + outPrice.ecoCount));
        if (outPrice.srsPrice > 0)
            outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 2, outPrice.srsPrice, 0, "Welcome service"));

        if (estateTB.pr_dcSUsed == 2 && outPrice.isWL == 0)
        {
            decimal _discount;
            if (estateTB.pr_dcS2_7_inDays > 0 && nightsTotal > estateTB.pr_dcS2_7_inDays && estateTB.pr_dcS2_7_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_7_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_7_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_7_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_7_inDays + " nights -" + estateTB.pr_dcS2_7_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_6_inDays > 0 && nightsTotal > estateTB.pr_dcS2_6_inDays && estateTB.pr_dcS2_6_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_6_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_6_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_6_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_6_inDays + " nights -" + estateTB.pr_dcS2_6_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_5_inDays > 0 && nightsTotal > estateTB.pr_dcS2_5_inDays && estateTB.pr_dcS2_5_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_5_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_5_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_5_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_5_inDays + " nights -" + estateTB.pr_dcS2_5_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_4_inDays > 0 && nightsTotal > estateTB.pr_dcS2_4_inDays && estateTB.pr_dcS2_4_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_4_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_4_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_4_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_4_inDays + " nights -" + estateTB.pr_dcS2_4_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_3_inDays > 0 && nightsTotal > estateTB.pr_dcS2_3_inDays && estateTB.pr_dcS2_3_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_3_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_3_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_3_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_3_inDays + " nights -" + estateTB.pr_dcS2_3_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_2_inDays > 0 && nightsTotal > estateTB.pr_dcS2_2_inDays && estateTB.pr_dcS2_2_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_2_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_2_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_2_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_2_inDays + " nights -" + estateTB.pr_dcS2_2_percent.objToInt32() + "%"));
            }
            else if (estateTB.pr_dcS2_1_inDays > 0 && nightsTotal > estateTB.pr_dcS2_1_inDays && estateTB.pr_dcS2_1_percent > 0)
            {
                _discount = _priceTotal * estateTB.pr_dcS2_1_percent.objToInt32() / 100;
                _discount = _discount.customRound(false);
                outPrice.prDiscountLongStay = _discount;
                outPrice.prDiscountLongStayDesc = contUtils.getLabel_title("lblX-DiscountFor-Y-nights", CurrentLang.ID, "").Replace("#discount#", "" + estateTB.pr_dcS2_1_percent.objToInt32()).Replace("#nights#", "" + (estateTB.pr_dcS2_1_inDays.objToInt32() + 1));
                _priceTotal = _priceTotal - _discount;
                outPrice.priceDetails.Add(new rntExts.RNT_estatePriceDetails(outPrice.priceDetails.Count + 1, dtCurrent, 4, -_discount, 0, "Over " + estateTB.pr_dcS2_1_inDays + " nights -" + estateTB.pr_dcS2_1_percent.objToInt32() + "%"));
            }
        }

        outPrice.pr_reservation = _priceTotal;
        if (outPrice.isWL == 1)
        {
            rntReservation_calculateParts_WL(currResID, out _priceTotal, ref outPrice, agentWL_total, rentalWL_total);
        }
        else
        {
            rntReservation_calculateParts(currResID, out _priceTotal, ref outPrice);
        }

        outPrice.prTotal = _priceTotal;
        return _priceTotal;
    }
    public static List<rntExts.PriceListPerDates> estate_getPriceListPerDates(int IdEstate, long IdAgent, DateTime dtStart, DateTime dtEnd, out int outError, string IdAdMedia = "")
    {
        // = 1 : Apt non trovato
        // = 2 : Non c'e stagionalita
        // = 3 : Non ci sono prezzi
        // = 4 : Stagionalita sbagliata
        outError = 0;
        dbRntAgentPriceChangeLimitTBL discountLimite = null;
        if (IdAgent > 0)
            using (DCmodRental dc = new DCmodRental())
                discountLimite = dc.dbRntAgentPriceChangeLimitTBLs.FirstOrDefault(x => x.pidAgent == IdAgent);
        var list = new List<rntExts.PriceListPerDates>();
        RNT_TB_ESTATE estateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
        if (estateTB == null) { outError = 1; return null; }
        var seasonGroup = estateTB.pidSeasonGroup.objToInt32();
        var seasonDateList = new List<dbRntSeasonDatesTBL>();
        using (DCmodRental dc = new DCmodRental())
            seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pidSeasonGroup == seasonGroup).ToList();
        List<RNT_TB_SPECIAL_OFFER> _specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pid_estate == IdEstate && x.is_active == 1).ToList();
        DateTime dtCurrent = dtStart;


        while (dtCurrent < dtEnd)
        {
            int minStay = estateTB.nights_min.objToInt32();
            decimal _price = 0;
            decimal _priceOpt = 0;
            var seasonDate = seasonDateList.FirstOrDefault(x => x.dtEnd >= dtCurrent && x.dtStart <= dtCurrent);
            if (seasonDate == null) { outError = 2; return list; }
            if (seasonDate.pidPeriod == 1)
            {
                _price = estateTB.pr_1_2pax.objToDecimal();
                _priceOpt = estateTB.pr_1_opt.objToDecimal();
            }
            if (seasonDate.pidPeriod == 2)
            {
                _price = estateTB.pr_2_2pax.objToDecimal();
                _priceOpt = estateTB.pr_2_opt.objToDecimal();
            }
            if (seasonDate.pidPeriod == 3)
            {
                _price = estateTB.pr_3_2pax.objToDecimal();
                _priceOpt = estateTB.pr_3_opt.objToDecimal();
                minStay = estateTB.nights_minVHSeason.objToInt32();
            }
            if (seasonDate.pidPeriod == 4)
            {
                _price = estateTB.pr_4_2pax.objToDecimal();
                _priceOpt = estateTB.pr_4_opt.objToDecimal();
              //  minStay = estateTB.nights_minVHSeason.objToInt32();
            }
            string changeDesc = "";
            // variazione del prezzo - offerte e sconti
            RNT_TB_SPECIAL_OFFER _specialOffer = null;
            if (IdAdMedia.ToLower() != ChnlHomeAwayProps_V411.IdAdMedia.ToLower())
            {
                 _specialOffer = _specialOfferList.FirstOrDefault(x => x.dtStart <= dtCurrent && x.dtEnd >= dtCurrent);
                if (_specialOffer != null && _specialOffer.pr_discount.objToInt32() > 0)
                {
                    changeDesc = _specialOffer.pr_discount.objToInt32() + "%";
                }
            }
            var datePrices = new rntExts.PriceListPerDates(dtCurrent, dtCurrent, minStay, changeDesc);
            Dictionary<int, decimal> Prices = new Dictionary<int, decimal>();
            for (int i = estateTB.pr_basePersons.objToInt32(); i <= estateTB.num_persons_max.objToInt32(); i++)
            {
                int extraPersons = i - estateTB.pr_basePersons.objToInt32();
                decimal currPrice = _price + (_priceOpt * extraPersons);
                if (currPrice == 0) { outError = 3; return list; }
                decimal changeAmount = 0;
                if (IdAdMedia.ToLower() != ChnlHomeAwayProps_V411.IdAdMedia.ToLower())
                {
                    if (_specialOffer != null && _specialOffer.pr_discount.objToInt32() > 0)
                    {
                        changeAmount = _specialOffer.pr_discount.objToInt32();
                        if (discountLimite != null && discountLimite.discountLimit > 0 && discountLimite.numPersons <= i)
                            changeAmount = changeAmount * discountLimite.discountLimit / 100;
                        decimal _specialOfferDiscount = currPrice * changeAmount / 100;
                        currPrice = currPrice - _specialOfferDiscount;
                    }
                 }

                
                Prices.Add(i, Decimal.Round(currPrice, 2));
            }
            datePrices.Period = seasonDate.pidPeriod;
            datePrices.Prices = Prices;
            var lastDatePrice = list.LastOrDefault();
            if (lastDatePrice == null || !lastDatePrice.HasSamePrices(Prices)) list.Add(datePrices);
            else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
            dtCurrent = dtCurrent.AddDays(1);
        }

        return list;
    }

    public static List<rntExts.PriceListPerDates> estate_getPriceListPerDates_Expedia(int IdEstate, long IdAgent, DateTime dtStart, DateTime dtEnd, out int outError)
    {
        // = 1 : Apt non trovato
        // = 2 : Non c'e stagionalita
        // = 3 : Non ci sono prezzi
        // = 4 : Stagionalita sbagliata
        outError = 0;
        dbRntAgentPriceChangeLimitTBL discountLimite = null;
        if (IdAgent > 0)
            using (DCmodRental dc = new DCmodRental())
                discountLimite = dc.dbRntAgentPriceChangeLimitTBLs.FirstOrDefault(x => x.pidAgent == IdAgent);
        var list = new List<rntExts.PriceListPerDates>();
        RNT_TB_ESTATE estateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
        if (estateTB == null) { outError = 1; return null; }
        var seasonGroup = estateTB.pidSeasonGroup.objToInt32();
        var seasonDateList = new List<dbRntSeasonDatesTBL>();
        using (DCmodRental dc = new DCmodRental())
            seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pidSeasonGroup == seasonGroup).ToList();
        List<RNT_TB_SPECIAL_OFFER> _specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pid_estate == IdEstate && x.is_active == 1).ToList();
        DateTime dtCurrent = dtStart;
        while (dtCurrent < dtEnd)
        {
            int minStay = estateTB.nights_min.objToInt32();
            decimal _price = 0;
            decimal _priceOpt = 0;
            var seasonDate = seasonDateList.FirstOrDefault(x => x.dtEnd >= dtCurrent && x.dtStart <= dtCurrent);
            if (seasonDate == null) { outError = 2; return list; }
            if (seasonDate.pidPeriod == 1)
            {
                _price = estateTB.pr_1_2pax.objToDecimal();
                _priceOpt = estateTB.pr_1_opt.objToDecimal();
            }
            if (seasonDate.pidPeriod == 2)
            {
                _price = estateTB.pr_2_2pax.objToDecimal();
                _priceOpt = estateTB.pr_2_opt.objToDecimal();
            }
            if (seasonDate.pidPeriod == 3)
            {
                _price = estateTB.pr_3_2pax.objToDecimal();
                _priceOpt = estateTB.pr_3_opt.objToDecimal();
                minStay = estateTB.nights_minVHSeason.objToInt32();
            }
            if (seasonDate.pidPeriod == 4)
            {
                _price = estateTB.pr_4_2pax.objToDecimal();
                _priceOpt = estateTB.pr_4_opt.objToDecimal();
                //minStay = estateTB.nights_minVHSeason.objToInt32();
            }
            string changeDesc = "";
            // variazione del prezzo - offerte e sconti
            RNT_TB_SPECIAL_OFFER _specialOffer = _specialOfferList.FirstOrDefault(x => x.dtStart <= dtCurrent && x.dtEnd >= dtCurrent);
            if (_specialOffer != null && _specialOffer.pr_discount.objToInt32() > 0)
            {
                changeDesc = _specialOffer.pr_discount.objToInt32() + "%";
            }
            var datePrices = new rntExts.PriceListPerDates(dtCurrent, dtCurrent, minStay, changeDesc);
            Dictionary<int, decimal> Prices = new Dictionary<int, decimal>();
            for (int i = estateTB.num_persons_min.objToInt32(); i <= estateTB.num_persons_max.objToInt32(); i++)
            {
                int extraPersons = 0;
                if (i > estateTB.pr_basePersons.objToInt32())
                    extraPersons = i - estateTB.pr_basePersons.objToInt32();

                decimal currPrice = _price + (_priceOpt * extraPersons);
                if (currPrice == 0) { outError = 3; return list; }
                decimal changeAmount = 0;
                if (_specialOffer != null && _specialOffer.pr_discount.objToInt32() > 0)
                {
                    changeAmount = _specialOffer.pr_discount.objToInt32();
                    if (discountLimite != null && discountLimite.discountLimit > 0 && discountLimite.numPersons <= i)
                        changeAmount = changeAmount * discountLimite.discountLimit / 100;
                    decimal _specialOfferDiscount = currPrice * changeAmount / 100;
                    currPrice = currPrice - _specialOfferDiscount;
                }
                Prices.Add(i, Decimal.Round(currPrice, 2));
            }
            datePrices.Period = seasonDate.pidPeriod;
            datePrices.Prices = Prices;
            var lastDatePrice = list.LastOrDefault();
            if (lastDatePrice == null || !lastDatePrice.HasSamePrices(Prices)) list.Add(datePrices);
            else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
            dtCurrent = dtCurrent.AddDays(1);
        }

        return list;
    }
    public static List<rntExts.PriceListPerDates> estate_getPriceListPerDates_Bcom(int IdEstate, long IdAgent, DateTime dtStart, DateTime dtEnd, out int outError)
    {
        // = 1 : Apt non trovato
        // = 2 : Non c'e stagionalita
        // = 3 : Non ci sono prezzi
        // = 4 : Stagionalita sbagliata
        outError = 0;
        dbRntAgentPriceChangeLimitTBL discountLimite = null;
        if (IdAgent > 0)
            using (DCmodRental dc = new DCmodRental())
                discountLimite = dc.dbRntAgentPriceChangeLimitTBLs.FirstOrDefault(x => x.pidAgent == IdAgent);
        var list = new List<rntExts.PriceListPerDates>();
        RNT_TB_ESTATE estateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
        if (estateTB == null) { outError = 1; return null; }
        var seasonGroup = estateTB.pidSeasonGroup.objToInt32();
        var seasonDateList = new List<dbRntSeasonDatesTBL>();
        using (DCmodRental dc = new DCmodRental())
            seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pidSeasonGroup == seasonGroup).ToList();

        //Removed Special Offer
        //List<RNT_TB_SPECIAL_OFFER> _specialOfferList = AppSettings.RNT_TB_SPECIAL_OFFERs.Where(x => x.dtEnd >= dtStart && x.dtStart <= dtEnd && x.pid_estate == IdEstate && x.is_active == 1).ToList();

        DateTime dtCurrent = dtStart;
        while (dtCurrent < dtEnd)
        {
            int minStay = estateTB.nights_min.objToInt32();
            decimal _price = 0;
            decimal _priceOpt = 0;
            var seasonDate = seasonDateList.FirstOrDefault(x => x.dtEnd >= dtCurrent && x.dtStart <= dtCurrent);
            if (seasonDate == null) { outError = 2; return list; }
            if (seasonDate.pidPeriod == 1)
            {
                _price = estateTB.pr_1_2pax.objToDecimal();
                _priceOpt = estateTB.pr_1_opt.objToDecimal();
            }
            if (seasonDate.pidPeriod == 2)
            {
                _price = estateTB.pr_2_2pax.objToDecimal();
                _priceOpt = estateTB.pr_2_opt.objToDecimal();
            }
            if (seasonDate.pidPeriod == 3)
            {
                _price = estateTB.pr_3_2pax.objToDecimal();
                _priceOpt = estateTB.pr_3_opt.objToDecimal();
                minStay = estateTB.nights_minVHSeason.objToInt32();
            }
            if (seasonDate.pidPeriod == 4)
            {
                _price = estateTB.pr_4_2pax.objToDecimal();
                _priceOpt = estateTB.pr_4_opt.objToDecimal();
              //  minStay = estateTB.nights_minVHSeason.objToInt32();
            }
            string changeDesc = "";

            #region Removed Special Offer
            //// variazione del prezzo - offerte e sconti
            //RNT_TB_SPECIAL_OFFER _specialOffer = _specialOfferList.FirstOrDefault(x => x.dtStart <= dtCurrent && x.dtEnd >= dtCurrent);
            //if (_specialOffer != null && _specialOffer.pr_discount.objToInt32() > 0)
            //{
            //    changeDesc = _specialOffer.pr_discount.objToInt32() + "%";
            //} 
            #endregion

            var datePrices = new rntExts.PriceListPerDates(dtCurrent, dtCurrent, minStay, changeDesc);
            Dictionary<int, decimal> Prices = new Dictionary<int, decimal>();
            for (int i = estateTB.pr_basePersons.objToInt32(); i <= estateTB.num_persons_max.objToInt32(); i++)
            {
                int extraPersons = i - estateTB.pr_basePersons.objToInt32();
                decimal currPrice = _price + (_priceOpt * extraPersons);
                if (currPrice == 0) { outError = 3; return list; }

                #region Removed Special Offer
                //decimal changeAmount = 0;
                //if (_specialOffer != null && _specialOffer.pr_discount.objToInt32() > 0)
                //{
                //    changeAmount = _specialOffer.pr_discount.objToInt32();
                //    if (discountLimite != null && discountLimite.discountLimit > 0 && discountLimite.numPersons <= i)
                //        changeAmount = changeAmount * discountLimite.discountLimit / 100;
                //    decimal _specialOfferDiscount = currPrice * changeAmount / 100;
                //    currPrice = currPrice - _specialOfferDiscount;
                //} 
                #endregion

                Prices.Add(i, Decimal.Round(currPrice, 2));
            }
            datePrices.Period = seasonDate.pidPeriod;
            datePrices.Prices = Prices;
            var lastDatePrice = list.LastOrDefault();
            if (lastDatePrice == null || !lastDatePrice.HasSamePrices(Prices)) list.Add(datePrices);
            else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
            dtCurrent = dtCurrent.AddDays(1);
        }

        return list;
    }

    public static void rntReservation_setDefaults(ref RNT_TBL_RESERVATION _currTBL)
    {
        if (_currTBL.cl_reminderCount == null)
            _currTBL.cl_reminderCount = 0;
        if (_currTBL.cl_isCompleted == null)
            _currTBL.cl_isCompleted = 0;
        if (_currTBL.is_dtStartTimeChanged == null)
            _currTBL.is_dtStartTimeChanged = 0;
        if (_currTBL.is_dtEndTimeChanged == null)
            _currTBL.is_dtEndTimeChanged = 0;
    }
    public static string rntReservation_mailBody_prices(RNT_TBL_RESERVATION _currTBL)
    {
        string _str = "";
        int _lang = _currTBL.cl_pid_lang.objToInt32();
        if (_lang == 0) _lang = 2;
        _str += CurrentSource.getSysLangValue("rnt_pr_part_payment_total", _lang) + ":&nbsp;";
        _str += "<strong>" + _currTBL.pr_part_payment_total.objToDecimal().ToString("N2") + "&nbsp;&euro;</strong>";
        _str += "&nbsp;&nbsp;(";
        //_str += CurrentSource.getSysLangValue("rnt_pr_part_commission", _lang);
        _str += _currTBL.pr_part_commission_tf.objToDecimal().ToString("N2") + "&nbsp;&euro;";
        if (_currTBL.pr_part_commission_tf.objToDecimal() != _currTBL.pr_part_commission_total.objToDecimal())
            _str += "&nbsp;+&nbsp;" + (_currTBL.pr_part_commission_total.objToDecimal() - _currTBL.pr_part_commission_tf.objToDecimal()).ToString("N2") + "&nbsp;&euro;&nbsp;" + CurrentSource.getSysLangValue("rnt_pr_part_vat", _lang);
        _str += "&nbsp;+&nbsp;" + _currTBL.pr_part_agency_fee.objToDecimal().ToString("N2") + "&nbsp;&euro;&nbsp;" + CurrentSource.getSysLangValue("rnt_pr_part_agency_fee", _lang);
        _str += ")";
        _str += "<br/>" + CurrentSource.getSysLangValue("rnt_pr_part_owner", _lang) + ":&nbsp;<strong>" + _currTBL.pr_part_owner.objToDecimal().ToString("N2") + "&nbsp;&euro;</strong>";
        _str += "<br/><strong>" + CurrentSource.getSysLangValue("rnt_pr_total", _lang) + ":&nbsp;" + _currTBL.pr_total.objToDecimal().ToString("N2") + "&nbsp;&euro;</strong>";

        return _str;
    }
    public static void rntReservation_onStateChange(RNT_TBL_RESERVATION _currTBL)
    {
        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
        RNT_RL_RESERVATION_STATE _state = new RNT_RL_RESERVATION_STATE();
        _state.date_state = _currTBL.state_date;
        _state.body = _currTBL.state_body;
        _state.pid_reservation = _currTBL.id;
        _state.pid_state = _currTBL.state_pid;
        _state.pid_user = _currTBL.state_pid_user;
        _state.subject = _currTBL.state_subject;
        DC_RENTAL.RNT_RL_RESERVATION_STATEs.InsertOnSubmit(_state);
        DC_RENTAL.SubmitChanges();
    }
    public static void rntReservation_addState(long pid_reservation, int pid_state, int pid_user, string subject, string body)
    {
        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
        RNT_RL_RESERVATION_STATE _state = new RNT_RL_RESERVATION_STATE();
        _state.date_state = DateTime.Now;
        _state.body = body;
        _state.pid_reservation = pid_reservation;
        _state.pid_state = pid_state;
        _state.pid_user = pid_user;
        _state.subject = subject;
        DC_RENTAL.RNT_RL_RESERVATION_STATEs.InsertOnSubmit(_state);
        DC_RENTAL.SubmitChanges();
    }
    public static string rntReservation_getStateName(int? id, string alternate)
    {
        RNT_LK_RESERVATION_STATE _d = AppSettings.RNT_LK_RESERVATION_STATEs.SingleOrDefault(x => x.id == id.objToInt32());
        if (_d != null && _d.title != "")
            return _d.title;
        return alternate;
    }
    public static string rntReservation_getStateName(int? id)
    {
        RNT_LK_RESERVATION_STATE _d = AppSettings.RNT_LK_RESERVATION_STATEs.SingleOrDefault(x => x.id == id.objToInt32());
        if (_d != null)
            return _d.title;
        return "";
    }
    public static string rntReservation_paymentPartTitle(string code)
    {
        customType _d = AppSettings.RNT_RESERVATION_PAYMENT_PARTs.SingleOrDefault(x => x.code == code);
        if (_d != null)
            return _d.title;
        return "";
    }
    public static string rntEstate_availableDatesForJSCal(int IdEstate, long IdReservation)
    {
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        {
            string _script = "";
            List<RNT_TBL_RESERVATION> _resList = dcOld.RNT_TBL_RESERVATION.Where(x => x.id != IdReservation && x.pid_estate == IdEstate && x.state_pid != 3 && x.dtStart.HasValue && x.dtEnd.HasValue).ToList();
            foreach (RNT_TBL_RESERVATION _res in _resList)
            {
                string _intDateFrom = "" + _res.dtStart.Value.JSCal_dateToInt();
                string _intDateTo = "" + _res.dtEnd.Value.JSCal_dateToInt();
                _script += "if (dateint > " + _intDateFrom + " && dateint < " + _intDateTo + ") { _controls += '<span class=\"rntCal nd_f\"></span>'; _enabled = false; }";
                _script += "if (dateint == " + _intDateFrom + ") { _controls += '<span class=\"rntCal nd_1\"></span>'; }";
                _script += "if (dateint == " + _intDateTo + ") { _controls += '<span class=\"rntCal nd_2\"></span>'; }";
            }
            return _script;
        }
    }
    public static RNT_TBL_REQUEST rntRequest_getRelatedRequest(RNT_TBL_REQUEST _newRequest)
    {
        RNT_TBL_REQUEST _request = null;
        List<RNT_TBL_REQUEST> _list = new List<RNT_TBL_REQUEST>();
        string _rnt_request_relation_view_fld = CommonUtilities.getSYS_SETTING("rnt_request_relation_view_fld");
        //ErrorLog.addLog("", "_rnt_request_relation_view_fld", _rnt_request_relation_view_fld);
        if (_rnt_request_relation_view_fld == "email")
            _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && x.email == _newRequest.email).ToList();
        else if (_rnt_request_relation_view_fld == "name_full")
            _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && x.name_full == _newRequest.name_full).ToList();
        else if (_rnt_request_relation_view_fld == "email or name_full")
            _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && (x.email == _newRequest.email || x.name_full == _newRequest.name_full)).ToList();
        else if (_rnt_request_relation_view_fld == "email and name_full")
            _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && (x.email == _newRequest.email && x.name_full == _newRequest.name_full)).ToList();
        else
            _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && x.email == _newRequest.email).ToList();

        string _rnt_request_relation_in_hours = CommonUtilities.getSYS_SETTING("rnt_request_relation_in_hours");
        DateTime _dtFrom = _newRequest.request_date_created.Value;
        int _hours = _rnt_request_relation_in_hours.ToInt32();
        if (_hours != 0)
            _list = _list.Where(x => x.request_date_created >= _dtFrom.AddHours(-_hours)).ToList();

        string _rnt_request_relation_is_view_date = CommonUtilities.getSYS_SETTING("rnt_request_relation_is_view_date");
        if (_rnt_request_relation_is_view_date == "1")
        {
            if (_newRequest.request_date_start != null && _newRequest.request_date_end != null)
                _list = _list.Where(x => x.request_date_start == _newRequest.request_date_start && x.request_date_end == _newRequest.request_date_end).ToList();
        }

        if (_list.Count > 0)
            _request = _list[0];
        return _request;
    }
    public static int rntRequest_addState(int id_request, int pid_state, int pid_user, string subject, string body)
    {
        magaRental_DataContext _dc = maga_DataContext.DC_RENTAL;
        RNT_RL_REQUEST_STATE _state = new RNT_RL_REQUEST_STATE();
        _state.body = body;
        _state.subject = subject;
        _state.date_state = DateTime.Now;
        _state.pid_request = id_request;
        _state.pid_state = pid_state;
        _state.pid_user = pid_user;
        _dc.RNT_RL_REQUEST_STATEs.InsertOnSubmit(_state);
        _dc.SubmitChanges();
        return _state.id;
    }
    public static void rntEstate_fillRewriteTool(ref UrlList uList, List<int> langIDs)
    {
        List<RNT_VIEW_ESTATE> _lnList = maga_DataContext.DC_RENTAL.RNT_VIEW_ESTATEs.Where(x => langIDs.Contains(x.pid_lang) && x.page_path != "" && x.pid_city == 1 && x.is_active == 1 && x.is_deleted != 1).ToList();
        foreach (RNT_VIEW_ESTATE _ln in _lnList)
        {
            UrlItem item = new UrlItem("pg_estate", CurrentAppSettings.ROOT_PATH + _ln.page_path, "pg_rntEstateDettNew.aspx?id=" + _ln.id + "&lang=" + _ln.pid_lang, "" + _ln.pid_lang);
            uList.Items.Add(item);

            //item = new UrlItem("pg_estate", CurrentAppSettings.ROOT_PATH + "old/" + _ln.page_path, "pg_rntEstateDett.aspx?id=" + _ln.id + "&lang=" + _ln.pid_lang);
            //uList.Items.Add(item);
        }
    }
    public static void rntEstate_fillRewriteTool_WL(ref UrlList uList, List<int> langIDs, long agentID)
    {
        List<RNT_VIEW_ESTATE> _lnList = maga_DataContext.DC_RENTAL.RNT_VIEW_ESTATEs.Where(x => langIDs.Contains(x.pid_lang) && x.page_path != "" && x.pid_city == 1 && x.is_active == 1 && x.is_deleted != 1).ToList();
        using (DCmodRental dc = new DCmodRental())
        {
            List<int> lstAgentEstate = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID).Select(x => x.pidEstate).Distinct().ToList();
            _lnList = _lnList.Where(x => lstAgentEstate.Contains(x.id)).ToList();
        }

        foreach (RNT_VIEW_ESTATE _ln in _lnList)
        {
            UrlItem item = new UrlItem("pg_estate", CurrentAppSettings.ROOT_PATH + _ln.page_path, "WLRental/" + "pg_rntEstateDett.aspx?id=" + _ln.id + "&lang=" + _ln.pid_lang, "" + _ln.pid_lang);
            uList.Items.Add(item);
        }
    }
}
public static partial class rntExts
{
    public static void CopyTo(this dbRntReservationTMP tmpTBL, ref RNT_TBL_RESERVATION newRes)
    {
        newRes.pid_operator = tmpTBL.pidOperator;
        newRes.pid_estate = tmpTBL.pidEstate;
        newRes.pidEstateCity = tmpTBL.pidEstateCity;
        newRes.dtStart = tmpTBL.dtStart;
        newRes.dtEnd = tmpTBL.dtEnd;
        newRes.pr_part_modified = tmpTBL.pr_isManual;
        newRes.dtStart = tmpTBL.dtStart;
        newRes.dtEnd = tmpTBL.dtEnd;

        newRes.num_adult = tmpTBL.numPers_adult;
        newRes.num_child_over = tmpTBL.numPers_childOver;
        newRes.num_child_min = tmpTBL.numPers_childMin;

        newRes.prTotalRate = tmpTBL.prTotalRate;
        newRes.prTotalOwner = tmpTBL.prTotalOwner;
        newRes.prTotalCommission = tmpTBL.prTotalCommission;
        newRes.pr_reservation = tmpTBL.pr_reservation;
        newRes.pr_total = tmpTBL.pr_total;

        newRes.pr_part_commission_tf = tmpTBL.pr_part_payment_total - tmpTBL.pr_part_agency_fee;
        newRes.pr_part_commission_total = tmpTBL.pr_part_payment_total - tmpTBL.pr_part_agency_fee;
        newRes.pr_part_agency_fee = tmpTBL.pr_part_agency_fee;
        newRes.pr_part_payment_total = tmpTBL.pr_part_payment_total;
        newRes.pr_part_owner = newRes.pr_total - newRes.pr_part_payment_total;
        newRes.pr_part_forPayment = tmpTBL.pr_part_forPayment;
        newRes.requestFullPayAccepted = tmpTBL.requestFullPayAccepted;

        newRes.pr_ecoPrice = tmpTBL.pr_ecoPrice;
        newRes.pr_ecoCount = tmpTBL.pr_ecoCount;
        newRes.pr_srsPrice = tmpTBL.pr_srsPrice;

        newRes.agentID = tmpTBL.agentID;
        newRes.agentCommissionPerc = tmpTBL.agentCommissionPerc;
        newRes.agentCommissionPrice = tmpTBL.agentCommissionPrice;
        newRes.agentDiscountType = tmpTBL.agentDiscountType;
        newRes.pr_part_extraServices = tmpTBL.pr_part_extraServices;

        newRes.pr_deposit = tmpTBL.pr_deposit;

        newRes.prDiscountSpecialOffer = tmpTBL.prDiscountSpecialOffer;
        newRes.prDiscountLongStay = tmpTBL.prDiscountLongStay;
        newRes.prDiscountLongStayDesc = tmpTBL.prDiscountLongStayDesc;
        newRes.prDiscountLongRange = tmpTBL.prDiscountLongRange;
        newRes.prDiscountLongRangeDesc = tmpTBL.prDiscountLongRangeDesc;
        newRes.prDiscountLastMinute = tmpTBL.prDiscountLastMinute;
        newRes.prDiscountLastMinuteDesc = tmpTBL.prDiscountLastMinuteDesc;

    }
    public static void CopyTo(this RntReservationTMP tmpTBL, ref RNT_TBL_RESERVATION newRes)
    {
        newRes.pid_operator = tmpTBL.pidOperator;
        newRes.pid_estate = tmpTBL.pidEstate;
        newRes.pidEstateCity = tmpTBL.pidEstateCity;
        newRes.dtStart = tmpTBL.dtStart;
        newRes.dtEnd = tmpTBL.dtEnd;
        newRes.pr_part_modified = tmpTBL.pr_isManual;
        newRes.dtStart = tmpTBL.dtStart;
        newRes.dtEnd = tmpTBL.dtEnd;

        newRes.num_adult = tmpTBL.numPers_adult;
        newRes.num_child_over = tmpTBL.numPers_childOver;
        newRes.num_child_min = tmpTBL.numPers_childMin;

        newRes.prTotalRate = tmpTBL.prTotalRate;
        newRes.prTotalOwner = tmpTBL.prTotalOwner;
        newRes.prTotalCommission = tmpTBL.prTotalCommission;
        newRes.pr_reservation = tmpTBL.pr_reservation;
        newRes.pr_total = tmpTBL.pr_total;

        newRes.pr_part_commission_tf = tmpTBL.pr_part_payment_total - tmpTBL.pr_part_agency_fee;
        newRes.pr_part_commission_total = tmpTBL.pr_part_payment_total - tmpTBL.pr_part_agency_fee;
        newRes.pr_part_agency_fee = tmpTBL.pr_part_agency_fee;
        newRes.pr_part_payment_total = tmpTBL.pr_part_payment_total;
        newRes.pr_part_owner = newRes.pr_total - newRes.pr_part_payment_total;
        newRes.pr_part_forPayment = tmpTBL.pr_part_forPayment;
        newRes.requestFullPayAccepted = tmpTBL.requestFullPayAccepted;

        newRes.pr_ecoPrice = tmpTBL.pr_ecoPrice;
        newRes.pr_ecoCount = tmpTBL.pr_ecoCount;
        newRes.pr_srsPrice = tmpTBL.pr_srsPrice;

        newRes.agentID = tmpTBL.agentID;
        newRes.agentCommissionPerc = tmpTBL.agentCommissionPerc;
        newRes.agentCommissionPrice = tmpTBL.agentCommissionPrice;
        newRes.agentDiscountType = tmpTBL.agentDiscountType;
        newRes.pr_part_extraServices = tmpTBL.pr_part_extraServices;

        newRes.pr_deposit = tmpTBL.pr_deposit;

        newRes.prDiscountSpecialOffer = tmpTBL.prDiscountSpecialOffer;
        newRes.prDiscountLongStay = tmpTBL.prDiscountLongStay;
        newRes.prDiscountLongStayDesc = tmpTBL.prDiscountLongStayDesc;
        newRes.prDiscountLongRange = tmpTBL.prDiscountLongRange;
        newRes.prDiscountLongRangeDesc = tmpTBL.prDiscountLongRangeDesc;
        newRes.prDiscountLastMinute = tmpTBL.prDiscountLastMinute;
        newRes.prDiscountLastMinuteDesc = tmpTBL.prDiscountLastMinuteDesc;

    }

    public static void CopyTo(this dbRntEstateExtrasLN source, ref dbRntEstateExtrasLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.description = source.description;
        copyto.sommario = source.sommario;
    }
    public static void CopyToCategory(this dbRntExtrasCategoryLN source, ref dbRntExtrasCategoryLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.description = source.description;
    }
    public static void CopyToExtraOwner(this dbRntExtraOwnerLN source, ref dbRntExtraOwnerLN copyto)
    {
        copyto.Policy = source.Policy;

    }
    public static void CopyToSubCategory(this dbRntExtrasSubCategoryLN source, ref dbRntExtrasSubCategoryLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.description = source.description;
    }
    public static void CopyToPack(this dbRntExtrasPackLN source, ref dbRntExtrasPackLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.description = source.description;
    }
    public static void CopyToMacroCategory(this dbRntExtrasMacroCategoryLN source, ref dbRntExtrasMacroCategoryLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.description = source.description;
    }
    public class RNT_estatePriceDetails
    {
        public int sequence;
        public DateTime dt;
        public int type;
        public decimal price = 0;
        public decimal priceOpt = 0;
        public string description;
        public RNT_estatePriceDetails(int _sequence, DateTime _dt, int _type, decimal _price, decimal _priceOpt, string _description)
        {
            sequence = _sequence;
            dt = _dt;
            type = _type;
            price = _price;
            priceOpt = _priceOpt;
            description = _description;
        }
    }
    [Serializable()]
    public class PreReservationPrices : ISerializable
    {

        public List<RNT_estatePriceDetails> priceDetails { get; set; }
        public decimal prTotal { get; set; }
        public decimal prTotalRate { get; set; }
        public decimal prTotalOwner { get; set; }
        public decimal prTotalCommission { get; set; }
        public decimal prYouAreSaving { get; set; }
        public decimal prDiscountSpecialOffer { get; set; }
        public decimal prDiscountLongStay { get; set; }
        public string prDiscountLongStayDesc { get; set; }
        public decimal prDiscountLongRange { get; set; }
        public string prDiscountLongRangeDesc { get; set; }
        public decimal prDiscountLastMinute { get; set; }
        public string prDiscountLastMinuteDesc { get; set; }
        public decimal pr_part_commission_tf { get; set; }
        public decimal pr_part_commission_total { get; set; }
        public decimal pr_part_agency_fee { get; set; }
        public decimal pr_part_payment_total { get; set; }
        public decimal pr_part_forPayment { get; set; }
        public decimal pr_part_owner { get; set; }
        public decimal pr_reservation { get; set; }
        public decimal agentCommissionPerc { get; set; }
        public decimal agentCommissionPrice { get; set; }
        public decimal agentTotalResPrice { get; set; }
        public int outError { get; set; }
        public decimal ecoPrice { get; set; }
        public int ecoCount { get; set; }
        public decimal srsPrice { get; set; }

        // da passare valori
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }
        public int dtCount { get; set; } // non serve
        public int numPersCount { get; set; }
        public int numPers_adult { get; set; }
        public int numPers_childOver { get; set; }
        public int numPers_childMin { get; set; }
        public decimal pr_discount_owner { get; set; }
        public decimal pr_discount_commission { get; set; }
        public decimal part_percentage { get; set; }
        public int agentDiscountNotPayed { get; set; }
        public int agentDiscountType { get; set; }
        public long agentID { get; set; }
        public DateTime agentCheckDate { get; set; } // la data (il primo del mese) con cui controlliamo le pren fatte dal agenzia
        public int requestFullPayAccepted { get; set; }
        public int isFreeMinStay { get; set; }
        public int isFreeArrivalDay { get; set; }

        #region White Label
        public int isWL { get; set; }

        public int WL_changeAmount { get; set; }
        public int WL_changeIsDiscount { get; set; }
        public int WL_changeIsPercentage { get; set; }
        public int WL_changeAmount_Agent { get; set; }
        public int WL_changeIsDiscount_Agent { get; set; }
        public int WL_changeIsPercentage_Agent { get; set; }
        #endregion

        public PreReservationPrices()
        {
            priceDetails = new List<RNT_estatePriceDetails>();
            prTotal = 0;
            prTotalRate = 0;
            prTotalOwner = 0;
            prTotalCommission = 0;
            prYouAreSaving = 0;
            prDiscountSpecialOffer = 0;
            prDiscountLongStay = 0;
            prDiscountLongStayDesc = "";
            prDiscountLongRange = 0;
            prDiscountLongRangeDesc = "";
            prDiscountLastMinute = 0;
            prDiscountLastMinuteDesc = "";
            pr_part_commission_tf = 0;
            pr_part_commission_total = 0;
            pr_part_agency_fee = 0;
            pr_part_payment_total = 0;
            pr_part_forPayment = 0;
            pr_part_owner = 0;
            pr_reservation = 0;
            agentCommissionPerc = 0;
            agentCommissionPrice = 0;
            agentTotalResPrice = -1;
            outError = 0;
            ecoPrice = 0;
            ecoCount = 0;
            srsPrice = 0;

            dtStart = DateTime.Now.AddDays(7);
            dtEnd = DateTime.Now.AddDays(10);
            dtCount = 3;
            numPersCount = 2;
            numPers_adult = 2;
            numPers_childOver = 0;
            numPers_childMin = 0;
            pr_discount_owner = 0;
            pr_discount_commission = 0;
            part_percentage = 0;
            agentDiscountNotPayed = 1;
            agentDiscountType = 1;
            agentID = 0;
            agentCheckDate = DateTime.Now;
            requestFullPayAccepted = 0;
            isFreeMinStay = 0;
            isFreeArrivalDay = 0;

            #region White Label
            isWL = 0;

            WL_changeAmount = 0;
            WL_changeIsDiscount = 0;
            WL_changeIsPercentage = 0;
            WL_changeAmount_Agent = 0;
            WL_changeIsDiscount_Agent = 0;
            WL_changeIsPercentage_Agent = 0;
            #endregion
        }
        public PreReservationPrices Clone()
        {
            PreReservationPrices _tmp = new PreReservationPrices();
            _tmp.priceDetails = priceDetails;
            _tmp.prTotal = prTotal;
            _tmp.prTotalRate = prTotalRate;
            _tmp.prTotalOwner = prTotalOwner;
            _tmp.prTotalCommission = prTotalCommission;
            _tmp.prYouAreSaving = prYouAreSaving;
            _tmp.prDiscountSpecialOffer = prDiscountSpecialOffer;
            _tmp.prDiscountLongStay = prDiscountLongStay;
            _tmp.prDiscountLongStayDesc = prDiscountLongStayDesc;
            _tmp.prDiscountLongRange = prDiscountLongRange;
            _tmp.prDiscountLongRangeDesc = prDiscountLongRangeDesc;
            _tmp.prDiscountLastMinute = prDiscountLastMinute;
            _tmp.prDiscountLastMinuteDesc = prDiscountLastMinuteDesc;
            _tmp.pr_part_commission_tf = pr_part_commission_tf;
            _tmp.pr_part_commission_total = pr_part_commission_total;
            _tmp.pr_part_agency_fee = pr_part_agency_fee;
            _tmp.pr_part_payment_total = pr_part_payment_total;
            _tmp.pr_part_forPayment = pr_part_forPayment;
            _tmp.pr_part_owner = pr_part_owner;
            _tmp.pr_reservation = pr_reservation;
            _tmp.agentCommissionPerc = agentCommissionPerc;
            _tmp.agentCommissionPrice = agentCommissionPrice;
            _tmp.agentTotalResPrice = agentTotalResPrice;
            _tmp.outError = outError;
            _tmp.ecoPrice = ecoPrice;
            _tmp.ecoCount = ecoCount;
            _tmp.srsPrice = srsPrice;

            _tmp.dtStart = dtStart;
            _tmp.dtEnd = dtEnd;
            _tmp.dtCount = dtCount;
            _tmp.numPersCount = numPersCount;
            _tmp.numPers_adult = numPers_adult;
            _tmp.numPers_childOver = numPers_childOver;
            _tmp.numPers_childMin = numPers_childMin;
            _tmp.pr_discount_owner = pr_discount_owner;
            _tmp.pr_discount_commission = pr_discount_commission;
            _tmp.part_percentage = part_percentage;
            _tmp.agentDiscountNotPayed = agentDiscountNotPayed;
            _tmp.agentDiscountType = agentDiscountType;
            _tmp.agentCheckDate = agentCheckDate;
            _tmp.isFreeMinStay = isFreeMinStay;
            _tmp.isFreeArrivalDay = isFreeArrivalDay;

            #region White Label
            _tmp.isWL = isWL;

            _tmp.WL_changeAmount = WL_changeAmount;
            _tmp.WL_changeIsDiscount = WL_changeIsDiscount;
            _tmp.WL_changeIsPercentage = WL_changeIsPercentage;
            _tmp.WL_changeAmount_Agent = WL_changeAmount_Agent;
            _tmp.WL_changeIsDiscount_Agent = WL_changeIsDiscount_Agent;
            _tmp.WL_changeIsPercentage_Agent = WL_changeIsPercentage_Agent;
            #endregion

            return _tmp;
            _tmp.agentID = agentID;
        }

        public void fillAgentDetails(dbRntAgentTBL agentTBL)
        {
            if (agentTBL == null) return;
            agentID = agentTBL.id;
            agentDiscountType = agentTBL.pidDiscountType.objToInt32();
            agentDiscountNotPayed = agentTBL.payDiscountNotPayed.objToInt32();
            requestFullPayAccepted = agentTBL.payFullPayment.objToInt32();
            if (agentDiscountType == 0) agentDiscountType = 1;
        }
        public void fillAgentDetails(RntAgentTBL agentTBL)
        {
            if (agentTBL == null) return;
            agentID = agentTBL.id;
            agentDiscountType = agentTBL.pidDiscountType.objToInt32();
            agentDiscountNotPayed = agentTBL.payDiscountNotPayed.objToInt32();
            requestFullPayAccepted = agentTBL.payFullPayment.objToInt32();
            if (agentDiscountType == 0) agentDiscountType = 1;
        }
        public void CopyTo(ref RNT_TBL_RESERVATION copyTo)
        {
            decimal old_prTotal = copyTo.pr_total.objToDecimal(); // if reservation is updated store old pr total
            copyTo.pr_reservation = pr_reservation;
            copyTo.pr_total = prTotal;
            copyTo.pr_total_desc = "";
            copyTo.pr_part_commission_tf = pr_part_commission_tf;
            copyTo.pr_part_commission_total = pr_part_commission_total;
            copyTo.pr_part_agency_fee = pr_part_agency_fee;
            copyTo.pr_part_payment_total = pr_part_payment_total;
            copyTo.pr_part_forPayment = pr_part_forPayment;
            copyTo.pr_part_owner = pr_part_owner;

            copyTo.prTotalRate = prTotalRate;
            copyTo.prTotalOwner = prTotalOwner;
            copyTo.prTotalCommission = prTotalCommission;

            copyTo.prDiscountSpecialOffer = prDiscountSpecialOffer;
            copyTo.prDiscountLongStay = prDiscountLongStay;
            copyTo.prDiscountLongStayDesc = prDiscountLongStayDesc;
            copyTo.prDiscountLongRange = prDiscountLongRange;
            copyTo.prDiscountLongRangeDesc = prDiscountLongRangeDesc;
            copyTo.prDiscountLastMinute = prDiscountLastMinute;
            copyTo.prDiscountLastMinuteDesc = prDiscountLastMinuteDesc;

            copyTo.pr_ecoPrice = ecoPrice;
            copyTo.pr_ecoCount = ecoCount;
            copyTo.pr_srsPrice = srsPrice;

            copyTo.agentID = agentID;
            copyTo.agentDiscountNotPayed = agentDiscountNotPayed;
            copyTo.agentDiscountType = agentDiscountType;
            copyTo.agentCommissionPerc = agentCommissionPerc;
            copyTo.agentCommissionPrice = agentCommissionPrice;

            copyTo.requestFullPayAccepted = copyTo.requestFullPayAccepted == 1 ? 1 : requestFullPayAccepted;

            // ricontrolliamo per sicurezza somma da pagare
            if (copyTo.pr_part_forPayment == 0 || (old_prTotal > 0 && old_prTotal < copyTo.pr_total))
            {
                if (copyTo.requestFullPayAccepted == 1)
                    copyTo.pr_part_forPayment = copyTo.pr_total.objToDecimal(); // somma da pagare = Totale pren - sconto dell'agenzia, sconto potrebbe essere 0 se impostato dall'agenzia
                else
                    copyTo.pr_part_forPayment = copyTo.pr_part_payment_total.objToDecimal(); // somma da pagare = acconto calcolato, potrebbe essere gia applicato sconto dell'agenzia
            }
            //else
            //{
            //    if (copyTo.pr_total > copyTo.payed_total)
            //    {
            //        copyTo.pr_part_forPayment = prTotal;
            //    }
            //}

            #region White Label
            copyTo.isWL = isWL;

            copyTo.WL_changeAmount = WL_changeAmount;
            copyTo.WL_changeIsDiscount = WL_changeIsDiscount;
            copyTo.WL_changeIsPercentage = WL_changeIsPercentage;
            copyTo.WL_changeAmount_Agent = WL_changeAmount_Agent;
            copyTo.WL_changeIsDiscount_Agent = WL_changeIsDiscount_Agent;
            copyTo.WL_changeIsPercentage_Agent = WL_changeIsPercentage_Agent;
            #endregion
        }
        public void FillFrom(RNT_TBL_RESERVATION fillFrom)
        {
            pr_reservation = fillFrom.pr_reservation.objToDecimal();
            prTotal = fillFrom.pr_total.objToDecimal();
            pr_part_commission_tf = fillFrom.pr_part_commission_tf.objToDecimal();
            pr_part_commission_total = fillFrom.pr_part_commission_total.objToDecimal();
            pr_part_agency_fee = fillFrom.pr_part_agency_fee.objToDecimal();
            pr_part_payment_total = fillFrom.pr_part_payment_total.objToDecimal();
            pr_part_forPayment = fillFrom.pr_part_forPayment.objToDecimal();
            pr_part_owner = fillFrom.pr_part_owner.objToDecimal();

            prTotalRate = fillFrom.prTotalRate.objToDecimal();
            prTotalOwner = fillFrom.prTotalOwner.objToDecimal();
            prTotalCommission = fillFrom.prTotalCommission.objToDecimal();

            prDiscountSpecialOffer = fillFrom.prDiscountSpecialOffer.objToDecimal();
            prDiscountLongStay = fillFrom.prDiscountLongStay.objToDecimal();
            prDiscountLongStayDesc = fillFrom.prDiscountLongStayDesc;
            prDiscountLongRange = fillFrom.prDiscountLongRange.objToDecimal();
            prDiscountLongRangeDesc = fillFrom.prDiscountLongRangeDesc;
            prDiscountLastMinute = fillFrom.prDiscountLastMinute.objToDecimal();
            prDiscountLastMinuteDesc = fillFrom.prDiscountLastMinuteDesc;

            ecoPrice = fillFrom.pr_ecoPrice.objToDecimal();
            ecoCount = fillFrom.pr_ecoCount.objToInt32();
            srsPrice = fillFrom.pr_srsPrice.objToDecimal();

            agentID = fillFrom.agentID.objToInt64();
            agentDiscountNotPayed = fillFrom.agentDiscountNotPayed.objToInt32();
            agentDiscountType = fillFrom.agentDiscountType.objToInt32();
            agentCommissionPerc = fillFrom.agentCommissionPerc.objToDecimal();
            agentCommissionPrice = fillFrom.agentCommissionPrice.objToDecimal();

            requestFullPayAccepted = fillFrom.requestFullPayAccepted.objToInt32();
            agentCheckDate = fillFrom.dtCreation.HasValue ? fillFrom.dtCreation.Value : DateTime.Now;

            #region White Label
            isWL = fillFrom.isWL.objToInt32();

            WL_changeAmount = fillFrom.WL_changeAmount.objToInt32();
            WL_changeIsDiscount = fillFrom.WL_changeIsDiscount.objToInt32();
            WL_changeIsPercentage = fillFrom.WL_changeIsPercentage.objToInt32();
            WL_changeAmount_Agent = fillFrom.WL_changeAmount_Agent.objToInt32();
            WL_changeIsDiscount_Agent = fillFrom.WL_changeIsDiscount_Agent.objToInt32();
            WL_changeIsPercentage_Agent = fillFrom.WL_changeIsPercentage_Agent.objToInt32();
            #endregion
        }
        public void CopyTo(ref dbRntReservationTMP copyTo)
        {
            copyTo.pr_reservation = pr_reservation;
            copyTo.pr_total = prTotal;
            copyTo.pr_part_commission_tf = pr_part_commission_tf;
            copyTo.pr_part_commission_total = pr_part_commission_total;
            copyTo.pr_part_payment_total = pr_part_payment_total;
            copyTo.pr_part_agency_fee = pr_part_agency_fee;
            copyTo.pr_part_forPayment = pr_part_forPayment;
            copyTo.pr_part_owner = pr_part_owner;

            copyTo.prTotalRate = prTotalRate;
            copyTo.prTotalOwner = prTotalOwner;
            copyTo.prTotalCommission = prTotalCommission;

            copyTo.prDiscountSpecialOffer = prDiscountSpecialOffer;
            copyTo.prDiscountLongStay = prDiscountLongStay;
            copyTo.prDiscountLongStayDesc = prDiscountLongStayDesc;
            copyTo.prDiscountLongRange = prDiscountLongRange;
            copyTo.prDiscountLongRangeDesc = prDiscountLongRangeDesc;
            copyTo.prDiscountLastMinute = prDiscountLastMinute;
            copyTo.prDiscountLastMinuteDesc = prDiscountLastMinuteDesc;

            copyTo.pr_ecoPrice = ecoPrice;
            copyTo.pr_ecoCount = ecoCount;
            copyTo.pr_srsPrice = srsPrice;

            copyTo.agentID = agentID;
            copyTo.agentCommissionPerc = agentCommissionPerc;
            copyTo.agentCommissionPrice = agentCommissionPrice;
            copyTo.agentCommissionNotInTotal = agentDiscountNotPayed;
            copyTo.agentDiscountType = agentDiscountType;
            copyTo.requestFullPayAccepted = requestFullPayAccepted;

            //if (copyTo.pr_part_forPayment == 0)
            //{
            //    if (copyTo.requestFullPayAccepted == 1)
            //        copyTo.pr_part_forPayment = copyTo.pr_total.objToDecimal(); // somma da pagare = Totale pren - sconto dell'agenzia, sconto potrebbe essere 0 se impostato dall'agenzia
            //    else
            //        copyTo.pr_part_forPayment = copyTo.pr_part_payment_total.objToDecimal(); // somma da pagare = acconto calcolato, potrebbe essere gia applicato sconto dell'agenzia
            //}
        }
        public void CopyTo(ref RntReservationTMP copyTo)
        {
            copyTo.pr_reservation = pr_reservation;
            copyTo.pr_total = prTotal;
            copyTo.pr_part_commission_tf = pr_part_commission_tf;
            copyTo.pr_part_commission_total = pr_part_commission_total;
            copyTo.pr_part_payment_total = pr_part_payment_total;
            copyTo.pr_part_agency_fee = pr_part_agency_fee;
            copyTo.pr_part_forPayment = pr_part_forPayment;
            copyTo.pr_part_owner = pr_part_owner;

            copyTo.prTotalRate = prTotalRate;
            copyTo.prTotalOwner = prTotalOwner;
            copyTo.prTotalCommission = prTotalCommission;

            copyTo.prDiscountSpecialOffer = prDiscountSpecialOffer;
            copyTo.prDiscountLongStay = prDiscountLongStay;
            copyTo.prDiscountLongStayDesc = prDiscountLongStayDesc;
            copyTo.prDiscountLongRange = prDiscountLongRange;
            copyTo.prDiscountLongRangeDesc = prDiscountLongRangeDesc;
            copyTo.prDiscountLastMinute = prDiscountLastMinute;
            copyTo.prDiscountLastMinuteDesc = prDiscountLastMinuteDesc;

            copyTo.pr_ecoPrice = ecoPrice;
            copyTo.pr_ecoCount = ecoCount;
            copyTo.pr_srsPrice = srsPrice;

            copyTo.agentID = agentID;
            copyTo.agentCommissionPerc = agentCommissionPerc;
            copyTo.agentCommissionPrice = agentCommissionPrice;
            copyTo.agentCommissionNotInTotal = agentDiscountNotPayed;
            copyTo.agentDiscountType = agentDiscountType;
            copyTo.requestFullPayAccepted = requestFullPayAccepted;

            //if (copyTo.pr_part_forPayment == 0)
            //{
            //    if (copyTo.requestFullPayAccepted == 1)
            //        copyTo.pr_part_forPayment = copyTo.pr_total.objToDecimal(); // somma da pagare = Totale pren - sconto dell'agenzia, sconto potrebbe essere 0 se impostato dall'agenzia
            //    else
            //        copyTo.pr_part_forPayment = copyTo.pr_part_payment_total.objToDecimal(); // somma da pagare = acconto calcolato, potrebbe essere gia applicato sconto dell'agenzia
            //}
        }
        public void FillFrom(dbRntReservationTMP fillFrom)
        {
            pr_reservation = fillFrom.pr_reservation.objToDecimal();
            prTotal = fillFrom.pr_total.objToDecimal();
            pr_part_payment_total = fillFrom.pr_part_payment_total.objToDecimal();
            pr_part_agency_fee = fillFrom.pr_part_agency_fee.objToDecimal();
            pr_part_forPayment = fillFrom.pr_part_forPayment.objToDecimal();
            pr_part_owner = fillFrom.pr_part_owner.objToDecimal();

            prTotalRate = fillFrom.prTotalRate.objToDecimal();
            prTotalOwner = fillFrom.prTotalOwner.objToDecimal();
            prTotalCommission = fillFrom.prTotalCommission.objToDecimal();

            prDiscountSpecialOffer = fillFrom.prDiscountSpecialOffer.objToDecimal();
            prDiscountLongStay = fillFrom.prDiscountLongStay.objToDecimal();
            prDiscountLongStayDesc = fillFrom.prDiscountLongStayDesc;
            prDiscountLongRange = fillFrom.prDiscountLongRange.objToDecimal();
            prDiscountLongRangeDesc = fillFrom.prDiscountLongRangeDesc;
            prDiscountLastMinute = fillFrom.prDiscountLastMinute.objToDecimal();
            prDiscountLastMinuteDesc = fillFrom.prDiscountLastMinuteDesc;

            ecoPrice = fillFrom.pr_ecoPrice.objToDecimal();
            ecoCount = fillFrom.pr_ecoCount.objToInt32();
            srsPrice = fillFrom.pr_srsPrice.objToDecimal();

            agentID = fillFrom.agentID.objToInt64();
            agentCommissionPerc = fillFrom.agentCommissionPerc.objToDecimal();
            agentCommissionPrice = fillFrom.agentCommissionPrice.objToDecimal();
            agentDiscountNotPayed = fillFrom.agentCommissionNotInTotal.objToInt32();

            requestFullPayAccepted = fillFrom.requestFullPayAccepted.objToInt32();

        }
        //Deserialization constructor.
        public PreReservationPrices(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            //EmpId = (int)info.GetValue("EmployeeId", typeof(int));
            //EmpName = (String)info.GetValue("EmployeeName", typeof(string));
            prTotal = (decimal)info.GetValue("prTotal", typeof(decimal));
            prTotalRate = (decimal)info.GetValue("prTotalRate", typeof(decimal));
            prTotalOwner = (decimal)info.GetValue("prTotalOwner", typeof(decimal));
            prTotalCommission = (decimal)info.GetValue("prTotalCommission", typeof(decimal));
            prYouAreSaving = (decimal)info.GetValue("prYouAreSaving", typeof(decimal));
            prDiscountSpecialOffer = (decimal)info.GetValue("prDiscountSpecialOffer", typeof(decimal));
            prDiscountLongStay = (decimal)info.GetValue("prDiscountLongStay", typeof(decimal));
            prDiscountLongStayDesc = (string)info.GetValue("prDiscountLongStayDesc", typeof(string));
            prDiscountLongRange = (decimal)info.GetValue("prDiscountLongRange", typeof(decimal));
            prDiscountLongRangeDesc = (string)info.GetValue("prDiscountLongRangeDesc", typeof(string));
            prDiscountLastMinute = (decimal)info.GetValue("prDiscountLastMinute", typeof(decimal));
            prDiscountLastMinuteDesc = (string)info.GetValue("prDiscountLastMinuteDesc", typeof(string));
            pr_part_commission_tf = (decimal)info.GetValue("pr_part_commission_tf", typeof(decimal));
            pr_part_commission_total = (decimal)info.GetValue("pr_part_commission_total", typeof(decimal));
            pr_part_agency_fee = (decimal)info.GetValue("pr_part_agency_fee", typeof(decimal));
            pr_part_payment_total = (decimal)info.GetValue("pr_part_payment_total", typeof(decimal));
            pr_part_forPayment = (decimal)info.GetValue("pr_part_forPayment", typeof(decimal));
            pr_part_owner = (decimal)info.GetValue("pr_part_owner", typeof(decimal));
            pr_reservation = (decimal)info.GetValue("pr_reservation", typeof(decimal));
            agentCommissionPerc = (decimal)info.GetValue("agentCommissionPerc", typeof(decimal));
            agentCommissionPrice = (decimal)info.GetValue("agentCommissionPrice", typeof(decimal));
            agentTotalResPrice = (decimal)info.GetValue("agentTotalResPrice", typeof(decimal));
            outError = (int)info.GetValue("outError", typeof(int));
            ecoPrice = (decimal)info.GetValue("ecoPrice", typeof(decimal));
            ecoCount = (int)info.GetValue("ecoCount", typeof(int));
            srsPrice = (decimal)info.GetValue("srsPrice", typeof(decimal));

            dtStart = (DateTime)info.GetValue("dtStart", typeof(DateTime));
            dtEnd = (DateTime)info.GetValue("dtEnd", typeof(DateTime));
            dtCount = (int)info.GetValue("dtCount", typeof(int));
            numPersCount = (int)info.GetValue("numPersCount", typeof(int));
            numPers_adult = (int)info.GetValue("numPers_adult", typeof(int));
            numPers_childOver = (int)info.GetValue("numPers_childOver", typeof(int));
            numPers_childMin = (int)info.GetValue("numPers_childMin", typeof(int));
            pr_discount_owner = (decimal)info.GetValue("pr_discount_owner", typeof(decimal));
            pr_discount_commission = (decimal)info.GetValue("pr_discount_commission", typeof(decimal));
            part_percentage = (decimal)info.GetValue("part_percentage", typeof(decimal));
            agentDiscountNotPayed = (int)info.GetValue("agentDiscountNotPayed", typeof(int));
            agentDiscountType = (int)info.GetValue("agentDiscountType", typeof(int));
            agentID = (long)info.GetValue("agentID", typeof(long));
            agentCheckDate = (DateTime)info.GetValue("agentCheckDate", typeof(DateTime));
            requestFullPayAccepted = (int)info.GetValue("requestFullPayAccepted", typeof(int));
            isFreeMinStay = (int)info.GetValue("isFreeMinStay", typeof(int));
            isFreeArrivalDay = (int)info.GetValue("isFreeArrivalDay", typeof(int));

            #region White Label
            isWL = (int)info.GetValue("isWL", typeof(int));

            WL_changeAmount = (int)info.GetValue("WL_changeAmount", typeof(int));
            WL_changeIsDiscount = (int)info.GetValue("WL_changeIsDiscount", typeof(int));
            WL_changeIsPercentage = (int)info.GetValue("WL_changeIsPercentage", typeof(int));
            WL_changeAmount_Agent = (int)info.GetValue("WL_changeAmount_Agent", typeof(int));
            WL_changeIsDiscount_Agent = (int)info.GetValue("WL_changeIsDiscount_Agent", typeof(int));
            WL_changeIsPercentage_Agent = (int)info.GetValue("WL_changeIsPercentage_Agent", typeof(int));
            #endregion
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("prTotal", prTotal);
            info.AddValue("prTotalRate", prTotalRate);
            info.AddValue("prTotalOwner", prTotalOwner);
            info.AddValue("prTotalCommission", prTotalCommission);
            info.AddValue("prYouAreSaving", prYouAreSaving);
            info.AddValue("prDiscountSpecialOffer", prDiscountSpecialOffer);
            info.AddValue("prDiscountLongStay", prDiscountLongStay);
            info.AddValue("prDiscountLongStayDesc", prDiscountLongStayDesc);
            info.AddValue("prDiscountLongRange", prDiscountLongRange);
            info.AddValue("prDiscountLongRangeDesc", prDiscountLongRangeDesc);
            info.AddValue("prDiscountLastMinute", prDiscountLastMinute);
            info.AddValue("prDiscountLastMinuteDesc", prDiscountLastMinuteDesc);
            info.AddValue("pr_part_commission_tf", pr_part_commission_tf);
            info.AddValue("pr_part_commission_total", pr_part_commission_total);
            info.AddValue("pr_part_agency_fee", pr_part_agency_fee);
            info.AddValue("pr_part_payment_total", pr_part_payment_total);
            info.AddValue("pr_part_forPayment", pr_part_forPayment);
            info.AddValue("pr_part_owner", pr_part_owner);
            info.AddValue("pr_reservation", pr_reservation);
            info.AddValue("agentCommissionPerc", agentCommissionPerc);
            info.AddValue("agentCommissionPrice", agentCommissionPrice);
            info.AddValue("agentTotalResPrice", agentTotalResPrice);
            info.AddValue("outError", outError);
            info.AddValue("ecoPrice", ecoPrice);
            info.AddValue("ecoCount", ecoCount);
            info.AddValue("srsPrice", srsPrice);

            info.AddValue("dtStart", dtStart);
            info.AddValue("dtEnd", dtEnd);
            info.AddValue("dtCount", dtCount);
            info.AddValue("numPersCount", numPersCount);
            info.AddValue("numPers_adult", numPers_adult);
            info.AddValue("numPers_childOver", numPers_childOver);
            info.AddValue("numPers_childMin", numPers_childMin);
            info.AddValue("pr_discount_owner", pr_discount_owner);
            info.AddValue("pr_discount_commission", pr_discount_commission);
            info.AddValue("part_percentage", part_percentage);
            info.AddValue("agentDiscountNotPayed", agentDiscountNotPayed);
            info.AddValue("agentDiscountType", agentDiscountType);
            info.AddValue("agentID", agentID);
            info.AddValue("agentCheckDate", agentCheckDate);
            info.AddValue("requestFullPayAccepted", requestFullPayAccepted);
            info.AddValue("isFreeMinStay", isFreeMinStay);
            info.AddValue("isFreeArrivalDay", isFreeArrivalDay);

            #region White Label
            info.AddValue("isWL", isWL);

            info.AddValue("WL_changeAmount", WL_changeAmount);
            info.AddValue("WL_changeIsDiscount", WL_changeIsDiscount);
            info.AddValue("WL_changeIsPercentage", WL_changeIsPercentage);
            info.AddValue("WL_changeAmount_Agent", WL_changeAmount_Agent);
            info.AddValue("WL_changeIsDiscount_Agent", WL_changeIsDiscount_Agent);
            info.AddValue("WL_changeIsPercentage_Agent", WL_changeIsPercentage_Agent);
            #endregion
        }
    }
}
