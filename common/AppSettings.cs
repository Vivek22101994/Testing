using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.IO;
using System.Xml.Linq;
using RentalInRome.data;
using ModRental;
using ModContent;
public static class DCExstensions
{
    public static RNT_VIEW_ESTATE_POSITION Clone(this RNT_VIEW_ESTATE_POSITION source)
    {
        try
        {
            RNT_VIEW_ESTATE_POSITION _return = new RNT_VIEW_ESTATE_POSITION();
            _return.id = source.id;
            _return.pid_lang = source.pid_lang;
            _return.position = source.position;
            _return.pid_estate = source.pid_estate;
            _return.css_class = source.css_class;
            _return.sequence = source.sequence;
            _return.title = source.title;
            _return.description = source.description;
            return _return;
        }
        catch (Exception)
        {
            return new RNT_VIEW_ESTATE_POSITION();
        }
    }
}
public class AppSettings
{
    public static bool RELOAD_CACHE
    {
        get
        {
            if (HttpContext.Current.Session["RELOAD_CACHE"] == null)
                HttpContext.Current.Session["RELOAD_CACHE"] = false;
            return (bool)HttpContext.Current.Session["RELOAD_CACHE"];
        }
        set
        {
            HttpContext.Current.Session["RELOAD_CACHE"] = value;
        }
    }
    public static void RELOAD_SESSION()
    {
        List<string> _toRemove = new List<string>();
        foreach (String _key in HttpContext.Current.Session.Keys)
        {
            if (_key.StartsWith("CURRENT_RNT_"))
            {
                _toRemove.Add(_key);
                continue;
            }
        }
        foreach (String _key in _toRemove)
        {
            HttpContext.Current.Session.Remove(_key);
        }

    }

    public static void _refreshCache_RNT_ESTATEs()
    {
        contProps.LangTBL = maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.ToList();
        RNT_LK_RESERVATION_STATEs = null;
        RNT_RL_ESTATE_CONFIG = null;
        RNT_TB_ESTATE = null;
        RNT_LN_ESTATE = null;
        RNT_estateList = null;
        RNT_estateListAll = null;
        RNT_activeZones = null;
        usrUtils.USR_ADMINs = null;
    }

    public class RNT_dateItem
    {
        public int Attr_IdEstate;
        public int Attr_dtDate;
        public int Attr_dtPart;
        public long Attr_IdRes;
        public string Attr_Title;
        public string onClick;
        public List<string> ClassList;
        public RNT_dateItem(int _IdEstate, int _dtDate, int _dtPart, int _IdRes, string _Title)
        {
            Attr_IdEstate = _IdEstate;
            Attr_dtDate = _dtDate;
            Attr_dtPart = _dtPart;
            Attr_IdRes = _IdRes;
            Attr_Title = _Title;
            ClassList = new List<string>();
            onClick = "RNT_openSelection(this)";
        }
    }
    public class RNT_estateDate
    {
        public DateTime Date;
        public int IdEstate;
        public RNT_dateItem Part_2;
        public RNT_dateItem Part_0;
        public RNT_dateItem Part_1;
        public RNT_estateDate(int _IdEstate, DateTime _Date)
        {
            Date = _Date;
            IdEstate = _IdEstate;
            Part_2 = new RNT_dateItem(_IdEstate, _Date.JSCal_dateToInt(), 2, 0, "");
            Part_0 = new RNT_dateItem(_IdEstate, _Date.JSCal_dateToInt(), 0, 0, "");
            Part_1 = new RNT_dateItem(_IdEstate, _Date.JSCal_dateToInt(), 1, 0, "");
        }
    }
    public class RNT_estateReservation
    {
        public long id { get; set; }
        public int? pid_estate { get; set; }
        public int? state_pid { get; set; }
        public DateTime? dtStart { get; set; }
        public DateTime? dtEnd { get; set; }
        public RNT_estateReservation(long _id, int? _pid_estate, int? _state_pid, DateTime? _dtStart, DateTime? _dtEnd)
        {
            id = _id;
            pid_estate = _pid_estate;
            state_pid = _state_pid;
            dtStart = _dtStart;
            dtEnd = _dtEnd;
        }
        public RNT_estateReservation Clone()
        {
            RNT_estateReservation _tmp = new RNT_estateReservation(id, pid_estate, state_pid, dtStart, dtEnd);
            return _tmp;
        }
    }
    public static int RNT_currCity = 1;
    public static int RNT_currCity_Florence = 2; 
    public class RNT_estate
    {
        public int id { get; set; }
        public int pid_lang { get; set; }
        public string category { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string page_path { get; set; }
        
        public string img_banner { get; set; }
        public string img_preview_1 { get; set; }
        public string img_preview_2 { get; set; }
        public string img_preview_3 { get; set; }
        public string zone { get; set; }
        public decimal pr_percentage { get; set; }
        public decimal price { get; set; }
        public decimal priceNoDiscount { get; set; }
        public decimal prTotalCommission { get; set; }
        public decimal priceReservation { get; set; }
        public decimal priceEco { get; set; }
        public decimal priceSrs { get; set; }
        public decimal priceAgencyFee { get; set; }
        public bool nicePrice { get; set; }
        public int priceError { get; set; }
        public int pid_city { get; set; }
        public int pid_zone { get; set; }
        public int pid_owner { get; set; }
        public int is_online_booking { get; set; }
        public int is_exclusive { get; set; }
        public int is_ecopulizie { get; set; }
        public int is_srs { get; set; }
        public int is_loft { get; set; }
        public int is_chidren_allowed { get; set; }

        public int longTermRent { get; set; }
        public decimal longTermPrMonthly { get; set; }

        public int num_persons_min { get; set; }
        public int num_persons_max { get; set; }
        public int num_persons_child { get; set; }
        public int num_rooms_bed { get; set; }
        public int num_rooms_bath { get; set; }
        public int num_bed_double { get; set; }

        public int nights_minVHSeason { get; set; }
        public int nights_min { get; set; }
        public int nights_max { get; set; }


        public int importance_vote { get; set; }
        public int importance_stars { get; set; }
        public int sequence { get; set; }
        public int zoneSequence { get; set; }
        public List<int> configList { get; set; }
        public string scheduleStr { get; set; }
        public string scheduleCont { get; set; }
        public bool is_nd { get; set; }
        public bool is_opz { get; set; }
        public bool is_can { get; set; }
        public bool is_prt { get; set; }
        public bool is_mv { get; set; }
        public bool is_free { get; set; }
        public List<RNT_estateDate> Dates;

        public decimal pr_agentID { get; set; }
        public decimal pr_agentCommissionPrice { get; set; }

        // for special offers
        public decimal spDiscountAmount { get; set; }
        public DateTime spDiscountDateStart { get; set; }
        public DateTime spDiscountDateEnd { get; set; }

        public decimal spDiscountedAmount { get; set; }

        public string google_maps { get; set; }
        public int is_google_maps { get; set; }
        public bool isBP { get; set; }

        public RNT_estate(int _id, string _category, string _code, int _pid_city, int _pid_zone, int _pid_owner)
        {
            id = _id;
            pid_lang = 0;
            category = _category.ToLower();
            code = _code;
            pid_city = _pid_city;
            pid_zone = _pid_zone;
            pid_owner = _pid_owner;
            pr_percentage = 0;
            priceNoDiscount = 0;
            prTotalCommission = 0;
            priceReservation = 0;
            priceEco = 0;
            priceSrs = 0;
            priceAgencyFee = 0;
            nicePrice = false;
            priceError = 0;

            img_banner = "";
            img_preview_1 = "";
            img_preview_2 = "";
            img_preview_3 = "";
            is_exclusive = -1;
            is_loft = -1;
            is_chidren_allowed = -1;

            longTermRent = -1;
            longTermPrMonthly = 0;

            num_persons_min = 2;
            num_persons_max = -1;
            num_persons_child = -1;
            num_rooms_bed = -1;
            num_rooms_bath = -1;
            num_bed_double = -1;

            nights_minVHSeason = -1;
            nights_min = -1;
            nights_max = -1;

            importance_vote = -1;
            importance_stars = -1;
            sequence = -1;
            zoneSequence = -1;
            configList = new List<int>();
            is_nd = false;
            is_opz = false;
            is_can = false;
            is_prt = false;
            is_mv = false;
            is_free = false;
            Dates = new List<RNT_estateDate>();

            pr_agentID = 0;
            pr_agentCommissionPrice = 0;

            spDiscountAmount = 0;
            spDiscountDateStart = DateTime.Now;
            spDiscountDateEnd = DateTime.Now;
            spDiscountedAmount = 0;

            google_maps = "";
            is_google_maps = 0;
            isBP = false;
        }
        public RNT_estate Clone()
        {
            RNT_estate _rntEst = new RNT_estate(id, category, code, pid_city, pid_zone, pid_owner);
            _rntEst.is_online_booking = is_online_booking;
            _rntEst.is_exclusive = is_exclusive;
            _rntEst.is_ecopulizie = is_ecopulizie;
            _rntEst.is_srs = is_srs;
            _rntEst.is_loft = is_loft;
            _rntEst.is_chidren_allowed = is_chidren_allowed;

            _rntEst.longTermRent = longTermRent;
            _rntEst.longTermPrMonthly = longTermPrMonthly;

            _rntEst.num_persons_min = num_persons_min;
            _rntEst.num_persons_max = num_persons_max;
            _rntEst.num_persons_child = num_persons_child;
            _rntEst.num_rooms_bed = num_rooms_bed;
            _rntEst.num_rooms_bath = num_rooms_bath;
            _rntEst.num_bed_double = num_bed_double;


            _rntEst.nights_minVHSeason = nights_minVHSeason;
            _rntEst.nights_min = nights_min;
            _rntEst.nights_max = nights_max;

            _rntEst.importance_vote = importance_vote;
            _rntEst.importance_stars = importance_stars;
            _rntEst.sequence = sequence;
            _rntEst.zoneSequence = zoneSequence;
            _rntEst.configList = configList;

            _rntEst.img_banner = img_banner;
            _rntEst.img_preview_1 = img_preview_1;
            _rntEst.img_preview_2 = img_preview_2;
            _rntEst.img_preview_3 = img_preview_3;
            _rntEst.title = title;
            _rntEst.pr_percentage = pr_percentage;
            _rntEst.priceError = priceError;

            _rntEst.pr_agentID = pr_agentID;
            _rntEst.pr_agentCommissionPrice = pr_agentCommissionPrice;

            _rntEst.google_maps = google_maps;
            _rntEst.is_google_maps = is_google_maps;
            _rntEst.isBP = isBP;

            return _rntEst;
        }
    }

    private static List<RNT_estate> _RNT_estateList; // refresh OK
    public static List<RNT_estate> RNT_estateList
    {
        get
        {
            if (_RNT_estateList == null)
            {
                List<RNT_estate> _tmp = RNT_estateListAll.Select(x => x.Clone()).ToList();
                _RNT_estateList = _tmp;
            }
            return _RNT_estateList.Select(x => x.Clone()).ToList();
        }
        set { _RNT_estateList = value; }
    }
    private static List<RNT_estate> _RNT_estateListAll; // refresh OK
    public static List<RNT_estate> RNT_estateListAll
    {
        get
        {
            if (_RNT_estateListAll == null)
            {
                List<RNT_estate> _tmp = new List<RNT_estate>();
                foreach (RNT_TB_ESTATE _est in RNT_TB_ESTATE)
                {
                    if (_est.is_active != 1) continue;
                    int has_air_condition = RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == _est.id && x.pid_config == 1 && x.is_HomeAway == 0) != null ? 1 : 0;
                    int has_adsl = RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == _est.id && x.pid_config == 2 && x.is_HomeAway == 0) != null ? 1 : 0;
                    int has_wifi = RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == _est.id && x.pid_config == 3 && x.is_HomeAway == 0) != null ? 1 : 0;
                    int has_lift = RNT_RL_ESTATE_CONFIG.SingleOrDefault(x => x.pid_estate == _est.id && x.pid_config == 5 && x.is_HomeAway == 0) != null ? 1 : 0;
                    RNT_estate _rntEst = new RNT_estate(_est.id, _est.category + "", _est.code, _est.pid_city.objToInt32(), _est.pid_zone.objToInt32(), _est.pid_owner.objToInt32());
                    _rntEst.is_online_booking = _est.is_online_booking.objToInt32();
                    _rntEst.is_exclusive = _est.is_exclusive.objToInt32();
                    _rntEst.is_ecopulizie = _est.is_ecopulizie.objToInt32();
                    _rntEst.is_srs = _est.is_srs.objToInt32();
                    _rntEst.is_loft = _est.is_loft.objToInt32();
                    _rntEst.is_chidren_allowed = _est.is_chidren_allowed;

                    _rntEst.longTermRent = _est.longTermRent.objToInt32();
                    _rntEst.longTermPrMonthly = _est.longTermPrMonthly.objToDecimal();

                    _rntEst.num_persons_min = _est.num_persons_min.objToInt32();
                    _rntEst.num_persons_max = _est.num_persons_max.objToInt32();
                    _rntEst.num_persons_child = _est.num_persons_child.objToInt32();
                    _rntEst.num_rooms_bed = _est.num_rooms_bed.objToInt32();
                    _rntEst.num_rooms_bath = _est.num_rooms_bath.objToInt32();
                    _rntEst.num_bed_double = _est.num_bed_double.objToInt32();

                    _rntEst.nights_minVHSeason = _est.nights_minVHSeason.objToInt32();
                    _rntEst.nights_min = _est.nights_min.objToInt32();
                    _rntEst.nights_max = _est.nights_max.objToInt32();

                    _rntEst.importance_vote = _est.importance_vote.objToInt32();
                    _rntEst.importance_stars = _est.importance_stars.objToInt32();
                    _rntEst.sequence = _est.sequence.objToInt32();
                    _rntEst.zoneSequence = _est.zoneSequence.objToInt32();
                    _rntEst.configList = RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == _est.id && x.is_HomeAway == 0).Select(x => x.pid_config).ToList();

                    _rntEst.img_banner = _est.img_banner;
                    _rntEst.img_preview_1 = _est.img_preview_1;
                    _rntEst.img_preview_2 = _est.img_preview_2;
                    _rntEst.img_preview_3 = _est.img_preview_3;
                    _rntEst.title = _est.code;
                    _rntEst.pr_percentage = _est.pr_percentage.objToDecimal();

                    _rntEst.google_maps = _est.google_maps;
                    _rntEst.is_google_maps = _est.is_google_maps.objToInt32();
                    _rntEst.isBP = _est.is_best_price.objToInt32() == 1 ? true : false;

                    _tmp.Add(_rntEst);
                }
                _RNT_estateListAll = _tmp;
            }
            return _RNT_estateListAll.Select(x => x.Clone()).ToList();
        }
        set { _RNT_estateListAll = value; }
    }
    private static List<RNT_TB_ESTATE> _RNT_TB_ESTATE; // refresh OK
    public static List<RNT_TB_ESTATE> RNT_TB_ESTATE
    {
        get
        {
            if (_RNT_TB_ESTATE == null)
                _RNT_TB_ESTATE = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active !=0).ToList();
            return new List<RNT_TB_ESTATE>(_RNT_TB_ESTATE);
        }
        set { _RNT_TB_ESTATE = value; }
    }
    private static List<RNT_LN_ESTATE> _RNT_LN_ESTATE; // refresh OK
    public static List<RNT_LN_ESTATE> RNT_LN_ESTATE
    {
        get
        {
            if (_RNT_LN_ESTATE == null)
            {
                _RNT_LN_ESTATE = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.ToList();
            }
            return _RNT_LN_ESTATE;
        }
        set { _RNT_LN_ESTATE = value; }
    }
    //private static List<RNT_TBL_RESERVATION> _RNT_TBL_RESERVATION; // refresh OK
    //public static List<RNT_TBL_RESERVATION> RNT_TBL_RESERVATION
    //{
    //    get
    //    {
    //        if (_RNT_TBL_RESERVATION == null)
    //        {
    //            _RNT_TBL_RESERVATION = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.ToList();
    //        }
    //        return new List<RNT_TBL_RESERVATION>(_RNT_TBL_RESERVATION.Select(x => x.Clone()));
    //    }
    //    set { _RNT_TBL_RESERVATION = value;  }
    //}
    private static List<RNT_TB_SPECIAL_OFFER> _RNT_TB_SPECIAL_OFFERs; // refresh AUTO
    public static List<RNT_TB_SPECIAL_OFFER> RNT_TB_SPECIAL_OFFERs
    {
        get
        {
            if (_RNT_TB_SPECIAL_OFFERs == null)
            {
                _RNT_TB_SPECIAL_OFFERs = maga_DataContext.DC_RENTAL.RNT_TB_SPECIAL_OFFER.Where(x => x.dtPublicEnd >= DateTime.Now.AddMonths(-1) || x.dtEnd >= DateTime.Now.AddMonths(-1)).ToList();
            }
            return _RNT_TB_SPECIAL_OFFERs;
        }
        set { _RNT_TB_SPECIAL_OFFERs = value; }
    }
    private static List<RNT_VIEW_SPECIAL_OFFER> _RNT_VIEW_SPECIAL_OFFERs; // refresh AUTO
    public static List<RNT_VIEW_SPECIAL_OFFER> RNT_VIEW_SPECIAL_OFFERs
    {
        get
        {
            if (_RNT_VIEW_SPECIAL_OFFERs == null)
            {
                _RNT_VIEW_SPECIAL_OFFERs = maga_DataContext.DC_RENTAL.RNT_VIEW_SPECIAL_OFFER.Where(x => x.dtPublicEnd >= DateTime.Now.AddMonths(-1) || x.dtEnd >= DateTime.Now.AddMonths(-1)).ToList();
            }
            return _RNT_VIEW_SPECIAL_OFFERs;
        }
        set { _RNT_VIEW_SPECIAL_OFFERs = value; }
    }
    public static List<int> _RNT_CUSTOM_CONFIGs = new List<int> { 
        1 // aria condizionata
        , 2 // internet ADSL
        , 3 // internet WiFi
        , 4 // Animali ammessi
        , 5 // Ascensore
        , 6 // Lavatrice
        , 15 // lavastoviglie
        , 54 // Terrazzo/Balcone
    };
    private static List<RNT_RL_ESTATE_CONFIG> _RNT_RL_ESTATE_CONFIG; // refresh OK
    public static List<RNT_RL_ESTATE_CONFIG> RNT_RL_ESTATE_CONFIG
    {
        get
        {
            if (_RNT_RL_ESTATE_CONFIG == null)
            {
                _RNT_RL_ESTATE_CONFIG = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => _RNT_CUSTOM_CONFIGs.Contains(x.pid_config)).ToList();
            }
            return _RNT_RL_ESTATE_CONFIG;
        }
        set { _RNT_RL_ESTATE_CONFIG = value; }
    }
    public static List<int> _LOC_CUSTOM_ZONEs
    {
        get
        {
            return CommonUtilities.getSYS_SETTING("RiR_zoneInHome").splitStringToList(",").Select(x => x.ToInt32()).Where(x => x > 0).ToList();
        }
    }
    private static List<int> _RNT_activeZones;// refresh OK
    public static List<int> RNT_activeZones
    {
        get
        {
            if (_RNT_activeZones == null)
            {
                List<int> _tmp = new List<int>();
                List<LOC_TB_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_TB_ZONEs.Where(x => _LOC_CUSTOM_ZONEs.Contains(x.id)).ToList();

                foreach (LOC_TB_ZONE _zone in _list)
                {
                    if (RNT_TB_ESTATE.FirstOrDefault(x => x.pid_zone == _zone.id) != null)
                        _tmp.Add(_zone.id);
                }
                _RNT_activeZones = _tmp;
            }
            return _RNT_activeZones;
        }
        set { _RNT_activeZones = value; }
    }
    private static List<RNT_LK_RESERVATION_STATE> _RNT_LK_RESERVATION_STATEs; // refresh OK
    public static List<RNT_LK_RESERVATION_STATE> RNT_LK_RESERVATION_STATEs
    {
        get
        {
            if (_RNT_LK_RESERVATION_STATEs == null)
            {
                _RNT_LK_RESERVATION_STATEs = maga_DataContext.DC_RENTAL.RNT_LK_RESERVATION_STATEs.ToList();
            }
            return _RNT_LK_RESERVATION_STATEs;
        }
        set { _RNT_LK_RESERVATION_STATEs = value; }
    }

    //private static List<dbRntReservationLastUpdatedLOG> _lstRntReservationLastUpdatedLOGs; // refresh OK
    //public static List<dbRntReservationLastUpdatedLOG> lstRntReservationLastUpdatedLOGs
    //{
    //    get
    //    {

    //        if (_lstRntReservationLastUpdatedLOGs == null)
    //        {
    //            using (DCmodRental dc = new DCmodRental())
    //            {
    //                _lstRntReservationLastUpdatedLOGs = dc.dbRntReservationLastUpdatedLOGs.ToList();
    //            }
    //        }
    //        return _lstRntReservationLastUpdatedLOGs;
    //    }
    //    set { _lstRntReservationLastUpdatedLOGs = value; }
    //}

    private static List<RNT_VIEW_PERIOD> _RNT_PERIODs; // refresh AUTO
    public static List<RNT_VIEW_PERIOD> RNT_PERIODs
    {
        get
        {
            if (_RNT_PERIODs == null)
            {
                _RNT_PERIODs = maga_DataContext.DC_RENTAL.RNT_VIEW_PERIODs.ToList();
            }
            return _RNT_PERIODs;
        }
        set { _RNT_PERIODs = value; }
    }
    private static List<RNT_VIEW_ESTATE_POSITION> _RNT_VIEW_ESTATE_POSITION; // refresh AUTO
    public static List<RNT_VIEW_ESTATE_POSITION> RNT_VIEW_ESTATE_POSITION
    {
        get
        {
            if (_RNT_VIEW_ESTATE_POSITION == null)
            {
                _RNT_VIEW_ESTATE_POSITION = maga_DataContext.DC_RENTAL.RNT_VIEW_ESTATE_POSITION.ToList();
            }
            return new List<RNT_VIEW_ESTATE_POSITION>(_RNT_VIEW_ESTATE_POSITION.Select(x => x.Clone()));
        }
        set { _RNT_VIEW_ESTATE_POSITION = value; }
    }
    private static List<dbRntEstateCommentsTBL> _RNT_TBL_ESTATE_COMMENTs; // refresh OK
    public static List<dbRntEstateCommentsTBL> RNT_TBL_ESTATE_COMMENTs
    {
        get
        {
            if (_RNT_TBL_ESTATE_COMMENTs == null)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    _RNT_TBL_ESTATE_COMMENTs = dc.dbRntEstateCommentsTBLs.OrderByDescending(x => x.dtComment).ToList();
                }
            }
            return _RNT_TBL_ESTATE_COMMENTs;
        }
        set { _RNT_TBL_ESTATE_COMMENTs = value; }
    }
    public class CONT_PAGE_PATH
    {
        public int id;
        public int lang;
        public string type;
        public string path;
        public CONT_PAGE_PATH(int _id, int _lang, string _type, string _path)
        {
            id = _id;
            lang = _lang;
            type = _type;
            path = _path;
        }
    }
    public class customClass
    {
        public int ID { get; set; }
        public int Name { get; set; }
        public DateTime ClockTime { get; set; }
        public string Direction { get; set; }
        public customClass(int _ID, int _Name, DateTime _ClockTime, string _Direction)
        {
            ID = _ID;
            Name = _Name;
            ClockTime = _ClockTime;
            Direction = _Direction;
        }
    }

    private static List<LOC_VIEW_ZONE> _LOC_ZONEs; // refresh AUTO
    public static List<LOC_VIEW_ZONE> LOC_ZONEs
    {
        get
        {
            if (_LOC_ZONEs == null)
            {
                _LOC_ZONEs = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.ToList();
            }
            return _LOC_ZONEs;
        }
        set { _LOC_ZONEs = value; HttpContext.Current.Application["LOC_ZONEs"] = _LOC_ZONEs; }
    }

    private static List<customType> _LANG_TYPEs;
    public static List<customType> LANG_TYPEs
    {
        get
        {
            if (_LANG_TYPEs == null)
            {
                List<customType> _tmp = new List<customType>();
                _tmp.Add(new customType(1, "sys"));
                _tmp.Add(new customType(2, "req"));
                _tmp.Add(new customType(3, "vote"));
                _tmp.Add(new customType(4, "form"));
                _tmp.Add(new customType(5, "pdf"));
                _LANG_TYPEs = _tmp;
            }
            return _LANG_TYPEs;
        }
        set { _LANG_TYPEs = value; }
    }
    private static List<customType> _RNT_RESERVATION_PAYMENT_PARTs;
    public static List<customType> RNT_RESERVATION_PAYMENT_PARTs
    {
        get
        {
            if (_RNT_RESERVATION_PAYMENT_PARTs == null)
            {
                List<customType> _tmp = new List<customType>();
                _tmp.Add(new customType(1, "part", "Acconto"));
                _tmp.Add(new customType(1, "part_diff", "Differenza Acconto"));
                _tmp.Add(new customType(1, "owner", "Saldo"));
                _tmp.Add(new customType(1, "owner_diff", "Differenza Saldo"));
                _tmp.Add(new customType(1, "full", "Importo Intero"));
                _tmp.Add(new customType(1, "full_diff", "Differenza Importo Intero"));
                _tmp.Add(new customType(1, "res_refund", "Rimborso del pagamento"));
                _RNT_RESERVATION_PAYMENT_PARTs = _tmp;
            }
            return _RNT_RESERVATION_PAYMENT_PARTs;
        }
        set { _RNT_RESERVATION_PAYMENT_PARTs = value; }
    }
    private static List<dbContSysConfigTB> _DEF_SYS_SETTINGs; // refresh OK
    public static List<dbContSysConfigTB> DEF_SYS_SETTINGs
    {
        get
        {
            if (_DEF_SYS_SETTINGs == null)
            {
                using (DCmodContent dc = new DCmodContent())
                    _DEF_SYS_SETTINGs = dc.dbContSysConfigTBs.ToList();
            }
            return _DEF_SYS_SETTINGs;
        }
        set { _DEF_SYS_SETTINGs = value; }
    }
}
public class customType
{
    public int id { get; set; }
    public string code { get; set; }
    public string title { get; set; }
    public customType(int _id, string _title)
    {
        id = _id;
        title = _title;
    }
    public customType(int _id, string _code, string _title)
    {
        id = _id;
        code = _code;
        title = _title;
    }
}
