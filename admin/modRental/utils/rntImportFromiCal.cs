using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentalInRome.data;
using ModRental;
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;
using System.IO;

public class rntImportFromiCal
{
    public int IdEstate { get; set; }
    public string ICalPath { get; set; }
    public string ICalType { get; set; }
    public dbRntIcalImportLOG CurrLog { get; set; }
    public List<dbRntIcalImportErrorLOG> ErrorDates { get; set; }
    public rntImportFromiCal(int estateId, string iCalPath, string iCalUrl, string channelManager)
    {
        IdEstate = estateId;
        ICalPath = iCalPath;
        ICalType = channelManager;
        CurrLog = new dbRntIcalImportLOG();
        CurrLog.uid = Guid.NewGuid();
        CurrLog.logDateTime = DateTime.Now;
        CurrLog.pidEstate = IdEstate;
        CurrLog.iCalType = ICalType;
        CurrLog.iCalUrl = iCalUrl;
        ErrorDates = new List<dbRntIcalImportErrorLOG>();
    }
    public void StartImport()
    {
        var DC_RENTAL = maga_DataContext.DC_RENTAL;
        iCalendarCollection iCalendars = new iCalendarCollection();
        iCalendars.AddRange(iCalendar.LoadFromFile(ICalPath));
        iCalendar iCal = iCalendars[0] as iCalendar;
        List<long> deleteIdsList = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.pid_estate == IdEstate && x.cl_name_full == "iCal" + ICalType && x.state_pid != 4).Select(x => x.id).ToList();
        Eco_WS.LocationEvent_Delete(deleteIdsList);
        Srs_WS.LocationEvent_Delete(deleteIdsList);

        List<int> lstResChangeEstateId = new List<int>();
        foreach (Event iEvent in iCal.Events)
        {
            DateTime dtStart = iEvent.Start.Value.Date;
            DateTime dtEnd = iEvent.End.Value.Date;
            string commento = iEvent.Summary + "";
            if (dtEnd < DateTime.Now.Date) continue;
            var tmpRes = DC_RENTAL.RNT_TBL_RESERVATION.FirstOrDefault(x => !deleteIdsList.Contains(x.id) && x.pid_estate == IdEstate //
                                                                         && x.state_pid != 3//
                                                                         && x.dtStart.HasValue //
                                                                         && x.dtEnd.HasValue //
                                                                         && ((x.dtStart.Value.Date <= dtStart && x.dtEnd.Value.Date >= dtEnd) //
                                                                             || (x.dtStart.Value.Date >= dtStart && x.dtStart.Value.Date < dtEnd) //
                                                                             || (x.dtEnd.Value.Date > dtStart && x.dtEnd.Value.Date <= dtEnd)));
            if (tmpRes != null)
            {
                var log = new dbRntIcalImportErrorLOG();
                log.uid = Guid.NewGuid();
                log.logDateTime = DateTime.Now;
                log.iCalComment = commento;
                log.iCalDtStart = dtStart;
                log.iCalDtEnd = dtEnd;
                log.reservationId = tmpRes.id;
                log.reservationStateId = tmpRes.state_pid.objToInt32();
                log.reservationStateName = rntUtils.rntReservation_getStateName(tmpRes.state_pid.objToInt32());
                ErrorDates.Add(log);
                continue;
            }

            var _currTBL = rntUtils.newReservation();
            _currTBL.pid_creator = 1;
            var stateTb = AppSettings.RNT_LK_RESERVATION_STATEs.FirstOrDefault(x => x.abbr == "iCal");

            #region store airbnb reservations as prenoto
            if (ICalType == "_airbnb")
            {
                _currTBL.state_pid = 4;
                _currTBL.state_subject = commento != "" ? commento : "Prenotato";
            }
            else
            {
                _currTBL.state_pid = stateTb != null ? stateTb.id : 2;
                _currTBL.state_subject = commento;
            }
            #endregion

            _currTBL.state_body = "Importato da iCal" + ICalType;
            _currTBL.state_date = DateTime.Now;
            _currTBL.state_pid_user = 1;

            _currTBL.is_booking = 0;

            _currTBL.dtStartTime = "000000";
            _currTBL.dtEndTime = "000000";
            _currTBL.is_dtStartTimeChanged = 0;
            _currTBL.is_dtEndTimeChanged = 0;

            _currTBL.dtStart = dtStart;
            _currTBL.dtEnd = dtEnd;

            _currTBL.pid_estate = IdEstate;
            _currTBL.cl_id = -1;
            _currTBL.cl_email = "";
            _currTBL.cl_name_full = "iCal" + ICalType;
            _currTBL.cl_name_honorific = "Prop.";
            _currTBL.cl_pid_discount = 0;
            _currTBL.cl_pid_lang = 1;

            _currTBL.num_adult = 0;
            _currTBL.num_child_over = 0;
            _currTBL.num_child_min = 0;

            _currTBL.pr_reservation = 0;
            _currTBL.pr_total = 0;
            _currTBL.pr_total_desc = "";
            _currTBL.pr_part_commission_tf = 0;
            _currTBL.pr_part_commission_total = 0;
            _currTBL.pr_part_agency_fee = 0;
            _currTBL.pr_part_payment_total = 0;
            _currTBL.pr_part_owner = 0;

            DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
            DC_RENTAL.SubmitChanges();
            _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
            _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL, false, false, false, true);
            //lstResChangeEstateId.Add(_currTBL.pid_estate.objToInt32());
            //rntUtils.rntReservation_onChange(_currTBL);
        }
        DC_RENTAL.RNT_TBL_RESERVATION.DeleteAllOnSubmit(DC_RENTAL.RNT_TBL_RESERVATION.Where(x => deleteIdsList.Contains(x.id)));
        DC_RENTAL.SubmitChanges();
        ChnlExpediaUpdate.UpdateAvailability_start(IdEstate);
        using (DCmodRental dc = new DCmodRental())
        {
            CurrLog.errorCount = ErrorDates.Count;
            if (CurrLog.errorCount > 0)
            {
                dc.Add(CurrLog);
                dc.SaveChanges();
                foreach (var tmp in ErrorDates)
                {
                    tmp.logUid = CurrLog.uid;
                    dc.Add(tmp);
                    dc.SaveChanges();
                }
            }
        }
    }
}
