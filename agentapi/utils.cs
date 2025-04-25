using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.agentapi
{
    /// <summary>
    /// Summary description for properties
    /// </summary>
    [WebService(Namespace = "http://magarental.com/webservices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class agentapi_base : System.Web.Services.WebService
    {
        protected dbRntAgentTBL agentTbl;
        protected long agentID { get { return agentTbl.id; } }
        public class AuthHeader : SoapHeader
        {
            public string Username;
            public string Password;
            public string ApiKey;
            public string Msg;
        }
        public AuthHeader Authentication;
        protected bool IsAuthOK()
        {
            Guid uid;
            if (!Guid.TryParse("" + Authentication.ApiKey, out uid))
            {
                return false;
            }
            using (DCmodRental dc = new DCmodRental())
            {
                agentTbl = dc.dbRntAgentTBLs.SingleOrDefault(x => x.uid == uid && x.authUsr == Authentication.Username && x.authPwd == Authentication.Password);
                if (agentTbl == null)
                {
                    Authentication.Msg = "KO: Authentication Failed, No Agent Found!";
                    return false;
                }
                if (agentTbl.isActive != 1)
                {
                    Authentication.Msg = "KO: Authentication Failed, Agent is blocked!";
                    return false;
                }
                return true;
            }
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "Test of Authentication")]
        public bool Authentication_Test()
        {
            return IsAuthOK();
        }

        #region Public Class
        public class PropertyPhoto
        {
            public string Thumb { get; set; }
            public string Big { get; set; }
            public PropertyPhoto()
            {
                Thumb = "";
                Big = "";
            }
            public PropertyPhoto(string thumb, string big)
            {
                Thumb = thumb;
                Big = big;
            }
        }
        public class PropertyText
        {
            public string Language { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public string Description { get; set; }
            public string metaTitle { get; set; }
            public string metaKeyWord { get; set; }
            public string metaDescription { get; set; }
            public string PagePath { get; set; }
            public PropertyText()
            {
                Language = "";
            }
            public PropertyText(string language)
            {
                Language = language;
            }
        }
        public class PropertyPricePerPersonItem
        {
            public int Person { get; set; }
            public decimal PriceAgent { get; set; }
            public decimal PriceTotal { get; set; }
            public PropertyPricePerPersonItem()
            {
                Person = 0;
                PriceAgent = 0;
                PriceTotal = 0;
            }
            public PropertyPricePerPersonItem(int person, decimal priceAgent, decimal priceTotal)
            {
                Person = person;
                PriceAgent = priceAgent;
                PriceTotal = priceTotal;
            }
        }
        public class PropertyPriceItem
        {
            public Int64 pidSeasonDate { get; set; }
            public DateTime DtStart { get; set; }
            public DateTime DtEnd { get; set; }
            public int MinNights { get; set; }
            public int Period { get; set; }
            public List<int> lstClosedArrival { get; set; }
            public List<PropertyPricePerPersonItem> PricePerPersons { get; set; }
            public string PriceChangeDesc { get; set; }
            public PropertyPriceItem()
            {
                DtStart = DateTime.Now;
                DtEnd = DateTime.Now;
                MinNights = 0;
                PricePerPersons = new List<PropertyPricePerPersonItem>();
                lstClosedArrival = new List<int>();
                PriceChangeDesc = "";
            }

            public PropertyPriceItem(rntExts.PriceListPerDates copyFrom)
            {
                //pidSeasonDate = copyFrom.pidSeasonDate;
                DtStart = copyFrom.DtStart;
                DtEnd = copyFrom.DtEnd;
                MinNights = copyFrom.MinStay;
                Period = copyFrom.Period;
                //lstClosedArrival = copyFrom.lstClosedArrival;
                PricePerPersons = new List<PropertyPricePerPersonItem>();
                foreach (var tmp in copyFrom.Prices)
                {
                    PricePerPersons.Add(new PropertyPricePerPersonItem(tmp.Key, copyFrom.Prices[tmp.Key], tmp.Value));
                }
                PriceChangeDesc = copyFrom.PriceChangeDesc;
            }
        }
        public class PropertyAvailabilityItem
        {
            public Int64 id { get; set; }
            public Int64 SourceResId { get; set; }
            public DateTime DtStart { get; set; }
            public DateTime DtEnd { get; set; }
            public DateTime DtCreation { get; set; }
            public int statePid { get; set; }
            public string cl_name { get; set; }
            public string cl_email { get; set; }
            public decimal prTotal { get; set; }
            public decimal numPersonAdult { get; set; }
            public decimal numpersonChilOver { get; set; }
            public decimal numPersonChildMin { get; set; }

            public string stateBody { get; set; }
            public DateTime stateDate { get; set; }
            public int statePidUser { get; set; }
            public string stateSubject { get; set; }

            public bool IsAvv { get; set; }
            public int Units { get; set; }
            public PropertyAvailabilityItem()
            {
                id = 0;
                SourceResId = 0;
                DtStart = DateTime.Now;
                DtEnd = DateTime.Now;
                DtCreation = DateTime.Now;
                prTotal = 0;
                Units = 0;
                statePid = 0;
                numPersonAdult = 0;
                numpersonChilOver = 0;
                numPersonChildMin = 0;
                cl_name = "";
                cl_email = "";

                stateBody = "";
                stateDate = DateTime.Now;
                statePidUser = 0;
                stateSubject = "";
            }
            public PropertyAvailabilityItem(DateTime _dtStart, DateTime _dtEnd, int _units)
            {
                DtStart = _dtStart;
                DtEnd = _dtEnd;
                DtCreation = DateTime.Now;
                prTotal = 0;
                Units = _units;
                statePid = 0;
                numPersonAdult = 0;
                numpersonChilOver = 0;
                numPersonChildMin = 0;
                cl_name = "";
                cl_email = "";
                stateBody = "";
                stateDate = DateTime.Now;
                statePidUser = 0;
                stateSubject = "Block";
            }
        }
        public class PropertyExtraItem
        {
            public int Id { get; set; }
            public bool HasPrice { get; set; }
            public int minPax { get; set; }
            public int maxPax { get; set; }
            public int Hours { get; set; }
            public int Days { get; set; }
            public decimal Price { get; set; }
            public string priceType { get; set; }
            public string paymentType { get; set; }
            public decimal actualPrice { get; set; }
            public decimal actualPriceChild { get; set; }
            public decimal Commission { get; set; }
            public int CommissionType { get; set; }
            public decimal costPrice { get; set; }
            public decimal costPriceChild { get; set; }
            public PropertyExtraItem()
            {
                Id = 0;
                HasPrice = false;
            }
        }
        public class PropertyCommentItem
        {
            public int Id { get; set; }
            public DateTime dtCreation { get; set; }
            public DateTime dtComment { get; set; }
            public string subject { get; set; }
            public string body { get; set; }
            public string bodyNegative { get; set; }
            public int pidEstate { get; set; }
            public int isActive { get; set; }
            public int isAnonymous { get; set; }
            public string cl_name_full { get; set; }
            public string cl_email { get; set; }
            public string cl_country { get; set; }
            public string type { get; set; }
            public string pers { get; set; }
            public int pid_user { get; set; }
            public long pidReservation { get; set; }
            public int cl_pid_lang { get; set; }
            public int voteStaff { get; set; }
            public int voteService { get; set; }
            public int voteCleaning { get; set; }
            public int voteComfort { get; set; }
            public int voteQualityPrice { get; set; }
            public int votePosition { get; set; }
            public int vote { get; set; }

            public PropertyCommentItem()
            {
                Id = 0;
                dtCreation = DateTime.Now;
                dtComment = DateTime.Now;
                subject = "";
                body = "";
                bodyNegative = "";
                pidEstate = 0;
                isActive = 0;
                isAnonymous = 0;
                cl_name_full = "";
                cl_email = "";
                cl_country = "";
                type = "";
                pers = "";
                pid_user = 0;
                pidReservation = 0;
                cl_pid_lang = 0;
                voteStaff = 0;
                voteService = 0;
                voteCleaning = 0;
                voteComfort = 0;
                voteQualityPrice = 0;
                votePosition = 0;
                vote = 0;
            }
        }
        public class PropertyInternItem
        {
            public int Id { get; set; }
            public string internType { get; set; }
            public int pidInternSubType { get; set; }
            public int featureCount { get; set; }
            public int featureId { get; set; }

        }
        public class PropertyUnit
        {
            public int Id { get; set; }
            public int CityId { get; set; }
            public int ZoneId { get; set; }
            public int CategoryId { get; set; }
            public string ComplexType { get; set; }
            public int ComplexId { get; set; }
            public int BaseAvailability { get; set; }
            public string Name { get; set; }
            public string ImgPreview { get; set; }
            public string ImgBanner { get; set; }
            public string GmapCoords { get; set; }
            public string LocZipCode { get; set; }
            public string LocPhone1 { get; set; }
            public string LocPhone2 { get; set; }
            public string LocAddress { get; set; }
            public string LocInnerBell { get; set; }
            public int num_bed_single { get; set; }
            public int num_bed_double { get; set; }
            public int num_bed_double_divisible { get; set; }
            public int num_bed_double_2level { get; set; }
            public int num_sofa_single { get; set; }
            public int num_sofa_double { get; set; }
            public int num_persons_max { get; set; }
            public int num_persons_min { get; set; }
            public int num_persons_adult { get; set; }
            public int num_persons_optional { get; set; }
            public int num_persons_child { get; set; }
            public int num_rooms_bed { get; set; }
            public int num_rooms_bath { get; set; }
            public decimal pr_deposit { get; set; }
            public int pr_percentage { get; set; }
            public int on_floor { get; set; }
            public decimal mq_inner { get; set; }
            public decimal mq_outer { get; set; }
            public decimal mq_terrace { get; set; }
            public int num_terraces { get; set; }
            public int nights_min { get; set; }
            public int nights_minVHSeason { get; set; }
            public int nights_max { get; set; }
            public int num_parkingClosed { get; set; }
            public int num_parkingOpen { get; set; }
            public int importance_vote { get; set; }
            public List<PropertyText> Texts { get; set; }
            public List<PropertyPhoto> Photos { get; set; }
            public List<PropertyPriceItem> Prices { get; set; }
            public List<PropertyAvailabilityItem> AvailabilityList { get; set; }
            public List<PropertyExtraItem> Extras { get; set; }
            public List<PropertyCommentItem> Comments { get; set; }
            public List<PropertyInternItem> features { get; set; }
            public decimal contract_commision { get; set; }
            public bool IsPriceUpdated { get; set; }
            public int SeasonGroupID
            {
                get;
                set;
            }
            public PropertyUnit()
            {
                Id = 0;
                CityId = 0;
                ZoneId = 0;
                CategoryId = 0;
                ComplexType = "";
                ComplexId = 0;
                BaseAvailability = 0;
                Name = "";
                ImgPreview = "";
                ImgBanner = "";
                GmapCoords = "";
                LocZipCode = "";
                LocPhone1 = "";
                LocPhone2 = "";
                LocAddress = "";
                LocInnerBell = "";
                num_bed_single = 0;
                num_bed_double = 0;
                num_bed_double_divisible = 0;
                num_bed_double_2level = 0;
                num_sofa_single = 0;
                num_sofa_double = 0;
                num_persons_max = 0;
                num_persons_min = 0;
                num_persons_adult = 0;
                num_persons_optional = 0;
                num_persons_child = 0;
                num_rooms_bed = 0;
                num_rooms_bath = 0;
                pr_deposit = 0;
                pr_percentage = 0;
                on_floor = 0;
                mq_inner = 0;
                mq_outer = 0;
                mq_terrace = 0;
                num_terraces = 0;
                nights_min = 0;
                nights_minVHSeason = 0;
                nights_max = 0;
                num_parkingClosed = 0;
                num_parkingOpen = 0;
                importance_vote = 0;
                contract_commision = 0;
                IsPriceUpdated = false;
                SeasonGroupID = 0;
                Texts = new List<PropertyText>();
                Photos = new List<PropertyPhoto>();
                Prices = new List<PropertyPriceItem>();
                AvailabilityList = new List<PropertyAvailabilityItem>();
                Extras = new List<PropertyExtraItem>();
                Comments = new List<PropertyCommentItem>();
                features = new List<PropertyInternItem>();
            }
        }
        public class City
        {
            public int Id { get; set; }
            public int CountryId { get; set; }
            public string Name { get; set; }
            public string ImgPreview { get; set; }
            public string ImgBanner { get; set; }
            public string GmapCoords { get; set; }
            public City()
            {
                Id = 0;
                CountryId = 0;
                Name = "";
                ImgPreview = "";
                ImgBanner = "";
                GmapCoords = "";
            }
        }
        public class Feature
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class InternSubType
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class Zone
        {
            public int Id { get; set; }
            public int CityId { get; set; }
            public string CityName { get; set; }
            public string Name { get; set; }
            public string ImgPreview { get; set; }
            public string ImgBanner { get; set; }
            public string GmapCoords { get; set; }
            public Zone()
            {
                Id = 0;
                CityId = 0;
                CityName = "";
                Name = "";
                ImgPreview = "";
                ImgBanner = "";
                GmapCoords = "";
            }
        }
        public class DiscountPromo
        {
            public long Id { get; set; }
            public string Type { get; set; }
            public int IsPercentage { get; set; }
            public decimal ChangeAmount { get; set; }
            public int NightsBeforeCheckIn { get; set; }
            public int NightsBeforeCheckOut { get; set; }
            public int NightsMinToApply { get; set; }
            public int NightsMaxToApply { get; set; }
            public DiscountPromo()
            {
                Id = 0;
                Type = "";
                IsPercentage = 0;
                ChangeAmount = 0;
                NightsBeforeCheckIn = 0;
                NightsBeforeCheckOut = 0;
                NightsMinToApply = 0;
                NightsMaxToApply = 0;
            }
        }
        public class Extra
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Extra()
            {
                Id = 0;
                Name = "";
            }
        }
        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Category()
            {
                Id = 0;
                Name = "";
            }
        }

        #endregion

    }
}
