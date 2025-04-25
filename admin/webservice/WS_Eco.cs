using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Web.UI.WebControls;
using RentalInRome.data;
using RentalInRome.WS_Eco;
using System.Net;

public class Eco_WS
{
    public static int ref_id_caller = 1;
    public static string ref_Username = "Rir";
    public static string ref_Password = "Fer90PLkir3W£_,MR";
    public static int Location_Insert_Update(RNT_TB_ESTATE _currTBL)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsEco") != "true" && CommonUtilities.getSYS_SETTING("sysWsEco").ToInt32() != 1) return 0;
        try
        {
            // gmap format "41.902442,12.479807"
            string _responseText = "";
            AuthHeader auth = new AuthHeader();
            auth.Username = ref_Username;
            auth.Password = ref_Password;
            WS _location = new WS();
            _location.AuthHeaderValue = auth;
            string _coords = _currTBL.google_maps ?? "";
            _coords = _coords.Replace(",", ".").Replace("|", ",");
            OwnerParams ownerParams = new OwnerParams();
            ownerParams.id = 0;
            USR_TBL_OWNER _owner = maga_DataContext.DC_USER.USR_TBL_OWNER.SingleOrDefault(x => x.id == _currTBL.pid_owner);
            if (_owner != null)
            {
                ownerParams.id = _owner.id;
                ownerParams.nameFull = _owner.name_full;
            }

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            return _location.Location_Insert_Update(_currTBL.code, ref_id_caller, _currTBL.id, _currTBL.is_ecopulizie == 1 ? 1 : 0, _currTBL.num_bed_single.objToInt32(), _currTBL.num_bed_double.objToInt32(), _currTBL.num_rooms_bath.objToInt32(), _currTBL.num_persons_min.objToInt32(), _currTBL.num_persons_max.objToInt32(), _currTBL.mq_inner.objToInt32(), _coords, ownerParams, out _responseText);
        }
        catch (Exception ex) { ErrorLog.addEcoLog("" + _currTBL.id, "Location_Insert_Update", ex.ToString()); }
        return 0;
    }
    public static string LocationEvent_Insert_Update(RNT_TBL_RESERVATION _currTBL)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsEco") != "true" && CommonUtilities.getSYS_SETTING("sysWsEco").ToInt32() != 1) return "";
        string _return = "";
        try
        {
            // gmap format "41.902442,12.479807"
            AuthHeader auth = new AuthHeader();
            auth.Username = ref_Username;
            auth.Password = ref_Password;
            WS _events = new WS();
            _events.AuthHeaderValue = auth;
            string comeDormono = "";
            if (_currTBL.bedSingle.objToInt32() > 0)
                comeDormono += "<tr><th>Letti single:</th><td> " + _currTBL.bedSingle.objToInt32() + "</td></tr>";
            if (_currTBL.bedDouble.objToInt32() > 0)
                comeDormono += "<tr><th>Matrimoniali:</th><td> " + _currTBL.bedDouble.objToInt32() + "</td></tr>";
            if (_currTBL.bedDoubleD.objToInt32() > 0)
                comeDormono += "<tr><th>Matrimoniali divisibili:</th><td> " + _currTBL.bedDoubleD.objToInt32() + (_currTBL.bedDoubleDConfig.objToInt32() > 0 ? " (" + _currTBL.bedDoubleDConfig.objToInt32() + " di cui da dividere)" : "") + "</td></tr>";
            if (_currTBL.bedDouble2level.objToInt32() > 0)
                comeDormono += "<tr><th>Letti a castello:</th><td> " + _currTBL.bedDouble2level.objToInt32() + "</td></tr>";
            if (_currTBL.bedSofaSingle.objToInt32() > 0)
                comeDormono += "<tr><th>Poltrona Letto:</th><td> " + _currTBL.bedSofaSingle.objToInt32() + "</td></tr>";
            if (_currTBL.bedSofaDouble.objToInt32() > 0)
                comeDormono += "<tr><th>Divano Letto:</th><td> " + _currTBL.bedSofaDouble.objToInt32() + "</td></tr>";
            if (comeDormono == "")
                comeDormono = "Non specificato.";
            else
                comeDormono = "<table>" + comeDormono + "</table>";
            string notesEco = _currTBL.notesEco;
            notesEco += "<br/><br/><br/><strong>Come Dormono:</strong><br/>" + comeDormono;

            //set new TLS protocol 1.1/1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _events.LocationEvent_Insert_Update(ref_id_caller, _currTBL.id.objToInt32(), _currTBL.pid_estate.objToInt32(), _currTBL.state_pid == 4 ? 0 : 1, _currTBL.dtStart.Value.Add(_currTBL.dtStartTime.JSTime_stringToTime()), _currTBL.dtEnd.Value.Add(_currTBL.dtEndTime.JSTime_stringToTime()), _currTBL.cl_name_honorific + " " + _currTBL.cl_name_full, _currTBL.num_adult.objToInt32() + _currTBL.num_child_over.objToInt32(), notesEco, out _return);
        }
        catch (Exception ex) { ErrorLog.addEcoLog("" + _currTBL.id, "LocationEvent_Insert_Update", ex.ToString()); }
        return _return;
    }
    public static string LocationEvent_Delete(RNT_TBL_RESERVATION _currTBL)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsEco") != "true" && CommonUtilities.getSYS_SETTING("sysWsEco").ToInt32() != 1) return "";
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
        catch (Exception ex) { ErrorLog.addEcoLog("" + _currTBL.id, "LocationEvent_Delete", ex.ToString()); }
        return _return;
    }
    public static string LocationEvent_Delete(List<long> idsList)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsEco") != "true" && CommonUtilities.getSYS_SETTING("sysWsEco").ToInt32() != 1) return "";
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
        catch (Exception ex) { ErrorLog.addEcoLog(idsList.Select(x => x + "").ToList().listToString("|"), "LocationEvent_Delete", ex.ToString()); }
        return _return;
    }
    public static string EstateReservations_UpdateAll(int IdEstate)
    {
        if (CommonUtilities.getSYS_SETTING("sysWsEco") != "true" && CommonUtilities.getSYS_SETTING("sysWsEco").ToInt32() != 1) return "";
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
                TimeSpan _dtStartTime = _res.dtStartTime.JSTime_stringToTime();
                DateTime _dtStart = _res.dtStart.Value.Add(_dtStartTime);
                TimeSpan _dtEndTime = _res.dtEndTime.JSTime_stringToTime();
                DateTime _dtEnd = _res.dtEnd.Value.Add(_dtEndTime);
                int is_to_verify = _res.state_pid == 4 ? 0 : 1;
                _events.LocationEvent_Insert_Update(1, _res.id.objToInt32(), IdEstate, is_to_verify, _dtStart, _dtEnd, _res.cl_name_honorific + " " + _res.cl_name_full, _res.num_adult.objToInt32() + _res.num_child_over.objToInt32(), _res.notesEco, out _return);
            }
        }
        catch (Exception ex) { ErrorLog.addEcoLog("" + IdEstate, "EstateReservations_UpdateAll", ex.ToString()); }
        return _return;
    }
}
