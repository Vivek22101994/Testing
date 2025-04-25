using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentalInRome.data;

public class limoUtils
{
    public static string getTransportType_title(string code)
    {
        LIMO_LK_TRANSPORTTYPE _s = limoProps.LIMO_LK_TRANSPORTTYPE.SingleOrDefault(x => x.code == code);
        if (_s != null)
            return _s.title;
        return "";
    }
    public static string getPickupPlace_title(int id)
    {
        LIMO_TB_PICKUP_PLACE _s = limoProps.LIMO_TB_PICKUP_PLACE.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.title;
        return "";
    }
    public static string getPickupType_title(string code)
    {
        if (code == "air") return "Airport";
        if (code == "train") return "Railway station";
        if (code == "sea") return "Seaport";
        if (code == "other") return "Other";
        return "";
    }
    public static TimeSpan pickupDuration(int idZone, string inOut, int pickupPlace, string transportType, int hourAt, int timeType, bool timeExtraNeeded)
    {
        TimeSpan timeDuration = new TimeSpan(0);
        TimeSpan timeExtra = new TimeSpan(0);
        LIMO_TB_PICKUP_PLACE pickupPlaceTBL = limoProps.LIMO_TB_PICKUP_PLACE.SingleOrDefault(x => x.id == pickupPlace);
        if (pickupPlaceTBL != null)
        {
            int _time = 0;
            if (timeType == 1)
                _time = inOut == "out" ? pickupPlaceTBL.outTime1.objToInt32() : pickupPlaceTBL.inTime1.objToInt32();
            if (timeType == 2)
                _time = inOut == "out" ? pickupPlaceTBL.outTime2.objToInt32() : pickupPlaceTBL.inTime2.objToInt32();
            timeExtra = new TimeSpan(0, _time, 0);
        }
        int? _duration = 0;
        string hourStr = hourAt.ToString();
        LIMO_RL_TRANSPORT_DURATION _rl = limoProps.LIMO_RL_TRANSPORT_DURATION.SingleOrDefault(x => x.pidZone == idZone && x.pidPickupPlace == pickupPlace && x.transportType == transportType);
        if (_rl == null) return timeDuration;
        switch (hourStr)
        {
            case "0":
                _duration = _rl.inAt00;
                break;
            case "1":
                _duration = _rl.inAt01;
                break;
            case "2":
                _duration = _rl.inAt02;
                break;
            case "3":
                _duration = _rl.inAt03;
                break;
            case "4":
                _duration = _rl.inAt04;
                break;
            case "5":
                _duration = _rl.inAt05;
                break;
            case "6":
                _duration = _rl.inAt06;
                break;
            case "7":
                _duration = _rl.inAt07;
                break;
            case "8":
                _duration = _rl.inAt08;
                break;
            case "9":
                _duration = _rl.inAt09;
                break;
            case "10":
                _duration = _rl.inAt10;
                break;
            case "11":
                _duration = _rl.inAt11;
                break;
            case "12":
                _duration = _rl.inAt12;
                break;
            case "13":
                _duration = _rl.inAt13;
                break;
            case "14":
                _duration = _rl.inAt14;
                break;
            case "15":
                _duration = _rl.inAt15;
                break;
            case "16":
                _duration = _rl.inAt16;
                break;
            case "17":
                _duration = _rl.inAt17;
                break;
            case "18":
                _duration = _rl.inAt18;
                break;
            case "19":
                _duration = _rl.inAt19;
                break;
            case "20":
                _duration = _rl.inAt20;
                break;
            case "21":
                _duration = _rl.inAt21;
                break;
            case "22":
                _duration = _rl.inAt22;
                break;
            case "23":
                _duration = _rl.inAt23;
                break;
        }
        if (_duration.HasValue)
        {
            timeDuration = new TimeSpan(0, _duration.Value, 0);
        }
        return timeDuration.TotalMinutes > 0 && (!timeExtraNeeded || timeExtra.TotalMinutes > 0) ? timeDuration.Add(timeExtra) : new TimeSpan(0);
    }
}
public class limoProps
{
    private static List<LIMO_RL_TRANSPORT_DURATION> _LIMO_RL_TRANSPORT_DURATION;
    public static List<LIMO_RL_TRANSPORT_DURATION> LIMO_RL_TRANSPORT_DURATION
    {
        get
        {
            if (_LIMO_RL_TRANSPORT_DURATION == null)
            {
                _LIMO_RL_TRANSPORT_DURATION = maga_DataContext.DC_LIMO.LIMO_RL_TRANSPORT_DURATION.ToList();
            }
            return _LIMO_RL_TRANSPORT_DURATION;
        }
        set { _LIMO_RL_TRANSPORT_DURATION = value; }
    }
    private static List<LIMO_LK_TRANSPORTTYPE> _LIMO_LK_TRANSPORTTYPE;
    public static List<LIMO_LK_TRANSPORTTYPE> LIMO_LK_TRANSPORTTYPE
    {
        get
        {
            if (_LIMO_LK_TRANSPORTTYPE == null)
                _LIMO_LK_TRANSPORTTYPE = maga_DataContext.DC_LIMO.LIMO_LK_TRANSPORTTYPE.ToList();
            return _LIMO_LK_TRANSPORTTYPE;
        }
        set
        {
            _LIMO_LK_TRANSPORTTYPE = value; ;
        }
    }
    private static List<LIMO_TB_PICKUP_PLACE> _LIMO_TB_PICKUP_PLACE;
    public static List<LIMO_TB_PICKUP_PLACE> LIMO_TB_PICKUP_PLACE
    {
        get
        {
            if (_LIMO_TB_PICKUP_PLACE == null)
                _LIMO_TB_PICKUP_PLACE = maga_DataContext.DC_LIMO.LIMO_TB_PICKUP_PLACE.ToList();
            return _LIMO_TB_PICKUP_PLACE;
        }
        set
        {
            _LIMO_TB_PICKUP_PLACE = value; ;
        }
    }
    public class PICKUP_PLACE_WITH_TRANSPORTTYPE
    {
        public int PickupPlace { get; set; }
        public string PickupPlace_title { get; set; }
        public string TransportType { get; set; }
        public string TransportType_title { get; set; }
        public int sequence { get; set; }
        public PICKUP_PLACE_WITH_TRANSPORTTYPE(int _PickupPlace, string _PickupPlace_title, string _TransportType, string _TransportType_title, int _sequence)
        {
            PickupPlace = _PickupPlace;
            PickupPlace_title = _PickupPlace_title;
            TransportType = _TransportType;
            TransportType_title = _TransportType_title;
            sequence = _sequence;
        }
    }
    private static List<PICKUP_PLACE_WITH_TRANSPORTTYPE> _PICKUP_PLACE_WITH_TRANSPORTTYPEs;
    public static List<PICKUP_PLACE_WITH_TRANSPORTTYPE> PICKUP_PLACE_WITH_TRANSPORTTYPEs
    {
        get
        {
            if (_PICKUP_PLACE_WITH_TRANSPORTTYPEs == null)
            {
                List<PICKUP_PLACE_WITH_TRANSPORTTYPE> _list = new List<PICKUP_PLACE_WITH_TRANSPORTTYPE>();
                foreach (LIMO_TB_PICKUP_PLACE PickupPlace in LIMO_TB_PICKUP_PLACE.Where(x => x.isActive == 1))
                {
                    foreach (LIMO_LK_TRANSPORTTYPE TransportType in LIMO_LK_TRANSPORTTYPE.Where(x => x.isActive == 1))
                    {
                        PICKUP_PLACE_WITH_TRANSPORTTYPE _new = new PICKUP_PLACE_WITH_TRANSPORTTYPE(PickupPlace.id, PickupPlace.title, TransportType.code, TransportType.title, _list.Count + 1);
                        _list.Add(_new);
                    }
                }
                _PICKUP_PLACE_WITH_TRANSPORTTYPEs = _list;
            }
            return _PICKUP_PLACE_WITH_TRANSPORTTYPEs;
        }
        set
        {
            _PICKUP_PLACE_WITH_TRANSPORTTYPEs = value; ;
        }
    }
}