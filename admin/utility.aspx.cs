using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class utility : adminBasePage
    {
        protected static string status = "";
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltrStatus.Text = status;
                if (Request.QueryString["action"] == "UpdateInvoicesTmp")
                {
                    ThreadStart start = new ThreadStart(UpdateInvoicesTmp);
                    Thread t = new Thread(start);
                    t.Priority = ThreadPriority.Lowest;
                    t.Start(); 
                }
                //ExportAllEstateToSrs();
                //ExportAllReservationsToSrs(false);
            }
        }
        protected void UpdateInvoicesTmp()
        {
            var _list = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE.Where(x => x.dtCreation>=new DateTime(2014,3,19) && x.pr_total >= 1).ToList();
            foreach (var _currTBL in _list)
            {

                INV_TBL_PAYMENT _pay = maga_DataContext.DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == _currTBL.inv_pid_payment);
                if (_pay == null)
                {
                    continue;
                }
                using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                {
                    RNT_TBL_RESERVATION _res = dcOld.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _currTBL.rnt_pid_reservation);
                    if (_res == null)
                    {
                        continue;
                    }
                    invUtils.payment_checkInvoice(_pay, _res);
                }
            }
        }
        protected void ExportAllEstateToSrs(bool debug, bool estates, bool res)
        {
            string _coords = "";
            string _address = "";
            List<RNT_TB_ESTATE> _list = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_srs == 1).ToList();
            foreach (RNT_TB_ESTATE _est in _list)
            {
                _coords = _est.google_maps ?? "";
                _coords = _coords.Replace(",", ".").Replace("|", ",");
                _address = _est.loc_address;
                if (estates)
                    Srs_WS.Location_Insert_Update(_est.code, CurrentSource.locZone_title(_est.pid_zone.objToInt32(), 1, "---"), _address, _est.id, 1, _est.num_bed_single.objToInt32(), _est.num_bed_double.objToInt32(), _est.num_rooms_bath.objToInt32(), 0, _coords);
                if (res)
                    Srs_WS.EstateReservations_UpdateAll(_est.id);
            }
        }
        protected void updateSrs()
        {
            List<int?> ids = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_srs == 1).Select(x => (int?)x.id).ToList();
            List<RNT_TBL_RESERVATION> _list = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(x => ids.Contains(x.pid_estate) && x.dtEnd.HasValue && x.dtEnd.Value.Date >= DateTime.Now.AddDays(-30) && x.dtCreation.HasValue && x.dtCreation >= DateTime.Now.AddDays(-90) && (x.state_pid == 4 || x.state_pid == 2)).ToList();
            foreach (RNT_TBL_RESERVATION _res in _list)
            {
                Srs_WS.LocationEvent_Insert_Update(_res);
            }
        }
        protected void lnk_updateSrs_Click(object sender, EventArgs e)
        {
            ThreadStart start = new ThreadStart(updateSrs);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        protected void updateEco()
        {
            List<int?> ids = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_ecopulizie == 1).Select(x => (int?)x.id).ToList();
            List<RNT_TBL_RESERVATION> _list = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(x => ids.Contains(x.pid_estate) && x.dtEnd.HasValue && x.dtEnd.Value.Date >= DateTime.Now.AddDays(-30) && (x.state_pid == 4 || x.state_pid == 2)).ToList();
            int countAll = _list.Count;
            int countCurrent = 0;
            status = "updateEco:" + countCurrent + " of " + countAll;
            foreach (RNT_TBL_RESERVATION _res in _list)
            {
                Eco_WS.LocationEvent_Insert_Update(_res);
                countCurrent++;
                status = "updateEco:" + countCurrent + " of " + countAll;
            }
            status = "";
        }

        protected void lnk_updateEco_Click(object sender, EventArgs e)
        {
            ThreadStart start = new ThreadStart(updateEco);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        protected void updateEcoLocation()
        {
            List<RNT_TB_ESTATE> _list = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_ecopulizie == 1).ToList();
            foreach (RNT_TB_ESTATE _est in _list)
            {
                Eco_WS.Location_Insert_Update(_est);
            }
        }
        protected void lnk_updateEcoLocation_Click(object sender, EventArgs e)
        {
            ThreadStart start = new ThreadStart(updateEcoLocation);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        protected void lnkTemp_Click(object sender, EventArgs e)
        {
            //tmp_UpdateEstatePrices();
            return;
            ThreadStart start = new ThreadStart(updateReservation_prDeposit);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        protected void tmp_UpdateEstatePrices()
        {
            decimal pr2pax = 0;
            decimal prBase = 0;
            decimal prOpt = 0;
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            List<RNT_TB_ESTATE> _list = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.pr_basePersons.HasValue && x.pr_basePersons > 2).ToList();
            foreach (RNT_TB_ESTATE _est in _list)
            {
                pr2pax = _est.pr_1_2pax.objToDecimal();
                prOpt = _est.pr_1_opt.objToDecimal();
                prBase = pr2pax + (prOpt * (_est.pr_basePersons.objToInt32() - 2));
                _est.pr_1_2pax = prBase;

                pr2pax = _est.pr_2_2pax.objToDecimal();
                prOpt = _est.pr_2_opt.objToDecimal();
                prBase = pr2pax + (prOpt * (_est.pr_basePersons.objToInt32() - 2));
                _est.pr_2_2pax = prBase;

                pr2pax = _est.pr_3_2pax.objToDecimal();
                prOpt = _est.pr_3_opt.objToDecimal();
                prBase = pr2pax + (prOpt * (_est.pr_basePersons.objToInt32() - 2));
                _est.pr_3_2pax = prBase;
            }
            DC_RENTAL.SubmitChanges();
            AppSettings._refreshCache_RNT_ESTATEs();
        }
        protected void rntEstate_createPagePath()
        {
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            List<RNT_TB_ESTATE> _list = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1).ToList();
            foreach (RNT_TB_ESTATE _est in _list)
            {
                rntUtils.rntEstate_createPagePath(_est.id);
            }
            AdminUtilities.FillRewriteTool();
        }
        protected void updateReservation_prDeposit()
        {
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.id > 150000).ToList();
            foreach (RNT_TBL_RESERVATION _currTBL in _list)
            {
                RNT_TB_ESTATE _estate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
                if (_estate != null)
                {
                    _currTBL.pr_deposit = _estate.pr_deposit;
                }
            }
            DC_RENTAL.SubmitChanges();
        }
        protected void updateReservationPwd()
        {
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.password == null || x.password == "").ToList();
            foreach (RNT_TBL_RESERVATION _currTBL in _list)
            {
                _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            }
            DC_RENTAL.SubmitChanges();
        }

        protected void lnk_updateReservationPwd_Click(object sender, EventArgs e)
        {
            ThreadStart start = new ThreadStart(updateReservationPwd);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        protected void updateReservationInvoice()
        {
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
            List<INV_TBL_PAYMENT> _listPayment = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.is_complete == 1 && x.direction == 1 && x.pay_date.HasValue && x.rnt_pid_reservation.HasValue && x.rnt_pid_reservation > 150000).OrderBy(x => x.pay_date).ToList();
            foreach (INV_TBL_PAYMENT _pay in _listPayment)
            {
                RNT_TBL_RESERVATION _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _pay.rnt_pid_reservation);
                if (_currTBL != null)
                {
                    invUtils.payment_checkInvoice(_pay, _currTBL);
                }
            }
            string _x = "";
        }

        protected void lnk_updateReservationInvoice_Click(object sender, EventArgs e)
        {
            ThreadStart start = new ThreadStart(updateReservationInvoice);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        protected void updateReservationPayment()
        {
            magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
            List<INV_TBL_PAYMENT> _listPayment = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.is_complete == 1 && x.direction == 1 && x.pay_date.HasValue && x.rnt_pid_reservation.HasValue && x.rnt_pid_reservation > 150000).OrderBy(x => x.pay_date).ToList();
            foreach (INV_TBL_PAYMENT _pay in _listPayment)
            {
                var currRes = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _pay.rnt_pid_reservation);
                if (currRes != null)
                    invUtils.payment_onChange(currRes, true);
            }
        }

        protected void lnk_updateReservationPayment_Click(object sender, EventArgs e)
        {
            ThreadStart start = new ThreadStart(updateReservationPayment);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        protected void updateReservationCity()
        {
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            List<RNT_TBL_RESERVATION> _list = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => !x.pidEstateCity.HasValue || x.pidEstateCity < 1).ToList();
            foreach (RNT_TBL_RESERVATION _currTBL in _list)
            {
                RNT_TB_ESTATE currEst = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
                if (currEst != null && currEst.pid_city.HasValue)
                    _currTBL.pidEstateCity = currEst.pid_city;
            }
            DC_RENTAL.SubmitChanges();
        }

        protected void lnk_updateReservationCity_Click(object sender, EventArgs e)
        {
            ThreadStart start = new ThreadStart(updateReservationCity);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        protected void lnk_sendPaymentMail_Click(object sender, EventArgs e)
        {
            RNT_TBL_RESERVATION _currTBL = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.code == txt_code_sendPaymentMail.Text);
            if (_currTBL == null)
            {
            }
        }
        protected void lnk_reservationOnChangeClick(object sender, EventArgs e)
        {
            RNT_TBL_RESERVATION _currTBL = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == txt_code_reservationOnChange.Text.ToInt64());
            if (_currTBL != null)
            {
                rntUtils.rntReservation_onChange(_currTBL);
            }
        }

    }
}
