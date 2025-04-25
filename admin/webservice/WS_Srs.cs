using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Web.UI.WebControls;
using RentalInRome.data;
using RentalInRome.WS_Srs;
using System.Net;

public class Srs_WS
{
    public static int ref_id_caller = 1;
    public static string ref_Username = "Rir";
    public static string ref_Password = "Fer90PLkir3W£_,MR";
    public static int Location_Insert_Update(string code, string zone, string address, int id_estate, int is_active, int num_bed_single, int num_bed_double, int num_rooms_bath, int cleaning_time_minutes, string gmap_coords)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsSrs") != "true" && CommonUtilities.getSYS_SETTING("sysWsSrs").ToInt32() != 1) return 0;
        try
        {
            // gmap format "41.902442,12.479807"
            string _responseText = "";
            AuthHeader auth = new AuthHeader();
            auth.Username = ref_Username;
            auth.Password = ref_Password;
            WS _location = new WS();
            _location.AuthHeaderValue = auth;

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            return _location.Location_Insert_Update(code, zone, address, 1, id_estate, is_active, num_bed_single, num_bed_double, num_rooms_bath, cleaning_time_minutes, gmap_coords, out _responseText);
        }
        catch (Exception ex) { ErrorLog.addSrsLog("" + id_estate, "LocationEvent_Insert_Update", ex.ToString()); }
        return 0;
    }
    public static string LocationEvent_Insert_Update(RNT_TBL_RESERVATION _currTBL, bool _isStartDateTimeChanged = false, bool _isEndDateTimeChanged = false)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsSrs") != "true" && CommonUtilities.getSYS_SETTING("sysWsSrs").ToInt32() != 1) return "";
        string _return = "";
        try
        {
            // gmap format "41.902442,12.479807"
            AuthHeader auth = new AuthHeader();
            auth.Username = ref_Username;
            auth.Password = ref_Password;
            WS _events = new WS();
            _events.AuthHeaderValue = auth;
            DateTime dtStart = _currTBL.dtIn.HasValue ? _currTBL.dtIn.Value : _currTBL.dtStart.Value.Add(_currTBL.dtStartTime.JSTime_stringToTime());
            DateTime dtEnd = _currTBL.dtOut.HasValue ? _currTBL.dtOut.Value : _currTBL.dtEnd.Value.Add(_currTBL.dtEndTime.JSTime_stringToTime());
            string clFullName = _currTBL.cl_name_honorific + " " + _currTBL.cl_name_full;
            string clEmail = _currTBL.cl_email;
            int numPers = _currTBL.num_adult.objToInt32() + _currTBL.num_child_over.objToInt32();
            decimal pr_part_payment_total = _currTBL.pr_part_payment_total.objToDecimal();
            decimal pr_rent_part_owner = _currTBL.pr_part_owner.objToDecimal();
            decimal pr_payed = _currTBL.payed_total.objToDecimal();
            decimal pr_deposit = _currTBL.pr_deposit.objToDecimal();
            LimoParams limoParams = new LimoParams();
            limoParams.limo_in_datetime = _currTBL.limo_in_datetime;
            limoParams.limo_in_isRequested = _currTBL.limo_in_isRequested;
            limoParams.limo_inPoint_type = _currTBL.limo_inPoint_type;
            limoParams.limo_inPoint_transportType = _currTBL.limo_inPoint_transportType;
            limoParams.limo_inPoint_pickupPlace = _currTBL.limo_inPoint_pickupPlace;
            limoParams.limo_inPoint_pickupPlaceName = _currTBL.limo_inPoint_pickupPlaceName;
            limoParams.limo_inPoint_details = _currTBL.limo_inPoint_details;
            limoParams.limo_inPoint_detailsType = _currTBL.limo_inPoint_detailsType;
            limoParams.limo_out_datetime = _currTBL.limo_out_datetime;
            limoParams.limo_out_isRequested = _currTBL.limo_out_isRequested;
            limoParams.limo_outPoint_type = _currTBL.limo_outPoint_type;
            limoParams.limo_outPoint_transportType = _currTBL.limo_outPoint_transportType;
            limoParams.limo_outPoint_pickupPlace = _currTBL.limo_outPoint_pickupPlace;
            limoParams.limo_outPoint_pickupPlaceName = _currTBL.limo_outPoint_pickupPlaceName;
            limoParams.limo_outPoint_details = _currTBL.limo_outPoint_details;
            limoParams.limo_outPoint_detailsType = _currTBL.limo_outPoint_detailsType;

            string voucherLink = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_voucher.aspx?uid=" + _currTBL.uid_2;
            string phoneMobile = "";
            string phoneTrip = "";
            USR_TBL_CLIENT client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currTBL.cl_id);
            if (client != null)
            {
                phoneMobile = client.contact_phone_mobile;
                phoneTrip = client.contact_phone_trip;
            }

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _events.LocationEvent_Insert_Update(ref_id_caller, _currTBL.id.objToInt32(), _currTBL.pid_estate.objToInt32(), 1, dtStart, dtEnd, dtStart, clFullName, phoneMobile, phoneTrip, clEmail, voucherLink, numPers, pr_part_payment_total, pr_rent_part_owner, pr_payed, pr_deposit, _currTBL.notesInner, _currTBL.pr_depositNotes, limoParams, _isStartDateTimeChanged, out _return);
            _events.LocationEvent_Insert_Update(ref_id_caller, _currTBL.id.objToInt32(), _currTBL.pid_estate.objToInt32(), 2, dtStart, dtEnd, dtEnd, clFullName, phoneMobile, phoneTrip, clEmail, voucherLink, numPers, pr_part_payment_total, pr_rent_part_owner, pr_payed, pr_deposit, _currTBL.notesInner, _currTBL.pr_depositNotes, limoParams, _isEndDateTimeChanged, out _return);
        }
        catch (Exception ex) { ErrorLog.addSrsLog("" + _currTBL.id, "LocationEvent_Insert_Update", ex.ToString()); }
        return _return;
    }
    public static string LocationEvent_Delete(RNT_TBL_RESERVATION _currTBL)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsSrs") != "true" && CommonUtilities.getSYS_SETTING("sysWsSrs").ToInt32() != 1) return "";
        string _return = "";
        try
        {
            // gmap format "41.902442,12.479807"
            AuthHeader auth = new AuthHeader();
            auth.Username = ref_Username;
            auth.Password = ref_Password;
            WS _events = new WS();
            _events.AuthHeaderValue = auth;

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _events.LocationEvent_Delete(ref_id_caller, _currTBL.id.objToInt32(), out _return);
        }
        catch (Exception ex) { ErrorLog.addSrsLog("" + _currTBL.id, "LocationEvent_Delete", ex.ToString()); }
        return _return;
    }
    public static string LocationEvent_Delete(List<long> idsList)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsSrs") != "true" && CommonUtilities.getSYS_SETTING("sysWsSrs").ToInt32() != 1) return "";
        string _return = "";
        try
        {
            // gmap format "41.902442,12.479807"
            AuthHeader auth = new AuthHeader();
            auth.Username = ref_Username;
            auth.Password = ref_Password;
            WS _events = new WS();
            _events.AuthHeaderValue = auth;

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _events.LocationEvent_DeleteList(ref_id_caller, idsList.Select(x => (int?)x.objToInt32()).ToArray(), out _return);
        }
        catch (Exception ex) { ErrorLog.addSrsLog(idsList.Select(x => x + "").ToList().listToString("|"), "LocationEvent_Delete", ex.ToString()); }
        return _return;
    }
    public static string EstateReservations_UpdateAll(int IdEstate)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsSrs") != "true" && CommonUtilities.getSYS_SETTING("sysWsSrs").ToInt32() != 1) return "";
        string _return = "";
        try
        {
            List<RNT_TBL_RESERVATION> _list = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.pid_estate == IdEstate && x.dtStart.HasValue && (x.state_pid == 4 || x.state_pid == 2)).ToList();
            AuthHeader auth = new AuthHeader();
            auth.Username = ref_Username;
            auth.Password = ref_Password;
            WS _events = new WS();
            _events.AuthHeaderValue = auth;

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            foreach (RNT_TBL_RESERVATION _res in _list)
            {
                LocationEvent_Insert_Update(_res);
            }
        }
        catch (Exception ex) { ErrorLog.addSrsLog("", "EstateReservations_UpdateAll", ex.ToString()); }
        return _return;
    }
}
