using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RentalInRome.data;
using ModRental;
using System.Web.Services.Protocols;
using ModAuth;

namespace RentalInRome.agentapi
{
    /// <summary>
    /// Summary description for properties
    /// </summary>
    [WebService(Namespace = "http://magarental.com/agentapi/v1")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class v01 : agentapi_base
    {
        public class GetActivePropertiesResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public List<PropertyUnit> FetchedData { get; set; }
            public GetActivePropertiesResult()
            {
                Success = false;
                Msg = "";
                FetchedData = new List<PropertyUnit>();
            }
        }
        public class GetActivePropertiesRequest
        {
            public string PropertyId { get; set; }
            public bool GetTexts { get; set; }
            public bool GetPhotos { get; set; }
            public bool GetPrices { get; set; }
            public bool GetAvailability { get; set; }
            public bool GetExtras { get; set; }
            public bool GetComments { get; set; }
            public DateTime? GetDataFrom { get; set; }
            public DateTime? GetDataTo { get; set; }
            public bool GetInterns { get; set; }
            public GetActivePropertiesRequest()
            {
                GetTexts = false;
                GetPhotos = false;
                GetPrices = false;
                GetAvailability = false;
                GetExtras = false;
                GetComments = false;
                GetInterns = false;
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "GetActiveProperties desc")]
        public GetActivePropertiesResult GetActiveProperties(GetActivePropertiesRequest request)
        {
            DateTime createdDate = DateTime.Now;
            var response = new GetActivePropertiesResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }

            using (DCmodRental dc = new DCmodRental())
            {
                List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.Where(x => x.is_online_booking == 1 && x.is_exclusive == 1).ToList();
                var fltEstateIdsList = estateList.Select(x => x.id).ToList();

                var AgentActiveContractIds = dc.dbRntAgentContractTBLs.Where(x => x.dtStart <= createdDate && x.dtEnd >= createdDate).Select(x => x.id).ToList();
                fltEstateIdsList = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && AgentActiveContractIds.Contains(x.pidAgentContract)).Select(x => x.pidEstate).ToList();
                estateList = estateList.Where(x => fltEstateIdsList.Contains(x.id)).ToList();
                fltEstateIdsList = estateList.Select(x => x.id).ToList();
                if (request.PropertyId.ToInt32() > 0)
                {
                    estateList = estateList.Where(x => x.id == request.PropertyId.ToInt32()).ToList();
                    fltEstateIdsList = estateList.Select(x => x.id).ToList();
                    if (estateList.Count == 0)
                    {
                        response.Msg = "Property with ID " + request.PropertyId + " not found";
                        return response;
                    }
                }
                var mediaList = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate.HasValue && fltEstateIdsList.Contains(x.pid_estate.Value) && x.type == "original").ToList();

                var extrasList = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_CONFIG.ToList();
                var dtStart = request.GetDataFrom.HasValue ? request.GetDataFrom.Value : DateTime.Now.Date;
                var dtEnd = request.GetDataTo.HasValue ? request.GetDataTo.Value : DateTime.Now.AddYears(2).Date;
                foreach (var tmpTb in estateList)
                {
                    int IdEstate = tmpTb.id;
                    var tmpTbl = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (tmpTbl == null) continue;
                    var tmpResp = new PropertyUnit();
                    tmpResp.Id = tmpTb.id;
                    tmpResp.SeasonGroupID = tmpTbl.pidSeasonGroup.objToInt32();
                    tmpResp.CityId = tmpTb.pid_city;
                    tmpResp.ZoneId = tmpTb.pid_zone;

                    int categoryId = 0;
                    if (tmpTbl.category == "apt")
                        categoryId = 1;
                    else if (tmpTbl.category == "villa")
                        categoryId = 2;

                    tmpResp.CategoryId = categoryId;
                    tmpResp.ComplexType = "";
                    tmpResp.ComplexId = 0;
                    tmpResp.BaseAvailability = 1;
                    tmpResp.Name = tmpTb.code;
                    tmpResp.ImgPreview = App.HOST + "/" + tmpTb.img_preview_1;
                    tmpResp.ImgBanner = App.HOST + "/" + tmpTb.img_banner;
                    tmpResp.GmapCoords = "" + tmpTb.google_maps;
                    tmpResp.LocZipCode = "" + tmpTbl.loc_zip_code;
                    tmpResp.LocPhone1 = "" + tmpTbl.loc_phone_1;
                    tmpResp.LocPhone2 = "" + tmpTbl.loc_phone_2;
                    tmpResp.LocAddress = "" + tmpTbl.loc_address;
                    tmpResp.LocInnerBell = "" + tmpTbl.loc_inner_bell;
                    tmpResp.num_bed_single = tmpTbl.num_bed_single.objToInt32();
                    tmpResp.num_bed_double = tmpTbl.num_bed_double.objToInt32();
                    tmpResp.num_bed_double_divisible = tmpTbl.num_bed_double_divisible.objToInt32();
                    tmpResp.num_bed_double_2level = tmpTbl.num_bed_double_2level.objToInt32();
                    tmpResp.num_sofa_single = tmpTbl.num_sofa_single.objToInt32();
                    tmpResp.num_sofa_double = tmpTbl.num_sofa_double.objToInt32();
                    tmpResp.num_persons_max = tmpTbl.num_persons_max.objToInt32();
                    tmpResp.num_persons_min = tmpTbl.num_persons_min.objToInt32();
                    tmpResp.num_persons_adult = tmpTbl.num_persons_adult.objToInt32();
                    tmpResp.num_persons_optional = tmpTbl.num_persons_optional.objToInt32();
                    tmpResp.num_persons_child = tmpTbl.num_persons_child.objToInt32();
                    tmpResp.num_rooms_bed = tmpTbl.num_rooms_bed.objToInt32();
                    tmpResp.num_rooms_bath = tmpTbl.num_rooms_bath.objToInt32();
                    tmpResp.pr_deposit = tmpTbl.pr_deposit.objToDecimal();
                    tmpResp.pr_percentage = tmpTbl.pr_percentage.objToInt32();
                    tmpResp.on_floor = tmpTbl.on_floor.objToInt32();
                    tmpResp.mq_inner = tmpTbl.mq_inner.objToDecimal();
                    tmpResp.mq_outer = tmpTbl.mq_outer.objToDecimal();
                    tmpResp.mq_terrace = tmpTbl.mq_terrace.objToDecimal();
                    tmpResp.num_terraces = tmpTbl.num_terraces.objToInt32();
                    tmpResp.nights_min = tmpTbl.nights_min.objToInt32();
                    tmpResp.nights_minVHSeason = tmpTbl.nights_minVHSeason.objToInt32();
                    tmpResp.nights_max = tmpTbl.nights_max.objToInt32();
                    tmpResp.num_parkingClosed = 0;
                    tmpResp.num_parkingOpen = 0;
                    tmpResp.importance_vote = tmpTbl.importance_vote.objToInt32();
                    if (request.GetTexts)
                    {
                        // fill language
                        var tmpLnList = AppSettings.RNT_LN_ESTATE.Where(x => x.pid_estate == tmpTb.id).ToList();
                        foreach (var tmpLn in tmpLnList)
                        {
                            int languageId = tmpLn.pid_lang;
                            string languageCode = "it";

                            if (languageId == 1)
                                languageCode = "it";
                            else if (languageId == 2)
                                languageCode = "en";
                            else if (languageId == 3)
                                languageCode = "de";
                            else if (languageId == 4)
                                languageCode = "es";
                            else if (languageId == 6)
                                languageCode = "fr";

                            var tmpText = new PropertyText(languageCode);
                            tmpText.Title = tmpLn.title;
                            tmpText.Summary = tmpLn.summary;
                            tmpText.Description = tmpLn.description;
                            tmpText.PagePath = tmpLn.page_path;
                            tmpText.metaTitle = tmpLn.meta_title;
                            tmpText.metaKeyWord = tmpLn.meta_keywords;
                            tmpText.metaDescription = tmpLn.meta_description;
                            tmpText.PagePath = tmpLn.page_path;
                            tmpResp.Texts.Add(tmpText);
                            //ErrorLog.addLog("", "maganetwork", languageCode);
                        }
                    }

                    if (request.GetPhotos) //&& agentTbl.chnlMGetPhotos == 1)
                    {
                        tmpResp.Photos = mediaList.Where(x => x.pid_estate == tmpTb.id).OrderBy(x => x.sequence).Select(x => new PropertyPhoto(App.HOST + "/" + x.img_thumb, App.HOST + "/" + x.img_banner)).ToList();
                    }

                    if (request.GetPrices)
                    {
                        int outError;

                        var currContract = dc.dbRntAgentContractTBLs.SingleOrDefault(x => x.dtStart <= createdDate && x.dtEnd >= createdDate && x.pidAgent == agentTbl.id);
                        if (currContract != null && currContract.IsSendPrice == 1)
                        {
                            tmpResp.IsPriceUpdated = true;
                            tmpResp.contract_commision = currContract.commissionAmount;
                            tmpResp.Prices = rntUtils.estate_getPriceListPerDates(IdEstate, agentID, dtStart, dtEnd, out outError).Select(x => new PropertyPriceItem(x)).ToList();
                        }
                    }
                    if (request.GetAvailability)
                    {
                        var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                        var list = new List<PropertyAvailabilityItem>();
                        foreach (RNT_TBL_RESERVATION res in resList)
                        {
                            try
                            {
                                var newRes = new PropertyAvailabilityItem();
                                newRes.SourceResId = res.id;
                                newRes.id = res.id;
                                newRes.DtStart = Convert.ToDateTime(res.dtStart);
                                newRes.DtEnd = Convert.ToDateTime(res.dtEnd);
                                newRes.DtCreation = Convert.ToDateTime(res.dtCreation);
                                newRes.statePid = res.state_pid.objToInt32();
                                newRes.cl_name = "";
                                newRes.cl_email = "";
                                newRes.prTotal = 0;
                                newRes.numPersonAdult = res.num_adult.objToInt32();
                                newRes.numpersonChilOver = res.num_child_over.objToInt32();
                                newRes.numPersonChildMin = res.num_child_min.objToInt32();
                                newRes.stateBody = res.state_body;
                                newRes.stateDate = Convert.ToDateTime(res.state_date);
                                newRes.statePidUser = res.state_pid_user.objToInt32();
                                newRes.stateSubject = res.state_subject;
                                list.Add(newRes);
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.addLog("", "sleep avv send total", "res send");
                            }
                        }
                        tmpResp.AvailabilityList = list;
                    }

                    if (request.GetExtras)
                    {
                        ErrorLog.addLog("", "amenities", extrasList.Count+"");
                        foreach (var tmpExtra in extrasList.Where(x => x.pid_estate == tmpTb.id).ToList())
                        {
                            //var extra = new PropertyExtraItem();
                            //extra.Id = tmpExtra.pid_config;
                            //var tmpExtraPrice = dc.dbRntEstateExtrasPriceTBL.FirstOrDefault(x => x.pidEstate == IdEstate && x.pidExtras == extra.Id);
                            //if (tmpExtraPrice != null)
                            //{
                            //    extra.HasPrice = true;
                            //    extra.minPax = tmpExtraPrice.minPax.objToInt32();
                            //    extra.maxPax = tmpExtraPrice.maxPax.objToInt32();
                            //    extra.Hours = tmpExtraPrice.Hours.objToInt32();
                            //    extra.Days = tmpExtraPrice.Days.objToInt32();
                            //    extra.Price = tmpExtraPrice.Price.objToDecimal();
                            //    extra.priceType = tmpExtraPrice.priceType + "";
                            //    extra.paymentType = tmpExtraPrice.paymentType + "";
                            //    extra.actualPrice = tmpExtraPrice.actualPrice.objToDecimal();
                            //    extra.actualPriceChild = tmpExtraPrice.actualPriceChild.objToDecimal();
                            //    extra.Commission = tmpExtraPrice.Commission.objToDecimal();
                            //    extra.CommissionType = tmpExtraPrice.CommissionType.objToInt32();
                            //    extra.costPrice = tmpExtraPrice.costPrice.objToDecimal();
                            //    extra.costPriceChild = tmpExtraPrice.costPriceChild.objToDecimal();
                            //}
                            //tmpResp.Extras.Add(extra);
                        }
                    }
                    if (request.GetComments)
                    {
                        var currList = dc.dbRntEstateCommentsTBLs.Where(x => x.isActive == 1 && x.pidEstate == tmpTb.id);
                        foreach (var tmp in currList)
                        {
                            var tmpItem = new PropertyCommentItem();
                            tmpItem.Id = tmp.id;
                            tmpItem.dtCreation = Convert.ToDateTime(tmp.dtCreation);
                            tmpItem.dtComment = Convert.ToDateTime(tmp.dtComment);
                            tmpItem.subject = tmp.subject;
                            tmpItem.body = tmp.body;
                            tmpItem.bodyNegative = tmp.bodyNegative;
                            tmpItem.pidEstate = tmp.pidEstate.objToInt32();
                            tmpItem.isActive = 1;
                            tmpItem.isAnonymous = tmp.isAnonymous.objToInt32();
                            tmpItem.cl_name_full = tmp.cl_name_full;
                            tmpItem.cl_country = tmp.cl_country;
                            tmpItem.cl_email = tmp.cl_email;
                            tmpItem.type = tmp.type;
                            tmpItem.pers = tmp.pers;
                            tmpItem.pid_user = 1;
                            tmpItem.pidReservation = tmp.pidReservation.objToInt64();
                            tmpItem.cl_pid_lang = tmp.cl_pid_lang.objToInt32();
                            tmpItem.voteStaff = tmp.voteStaff.objToInt32();
                            tmpItem.voteCleaning = tmp.voteCleaning.objToInt32();
                            tmpItem.voteComfort = tmp.voteComfort.objToInt32();
                            tmpItem.voteQualityPrice = tmp.voteQualityPrice.objToInt32();
                            tmpItem.votePosition = tmp.votePosition.objToInt32();
                            tmpItem.vote = tmp.vote.objToInt32();
                            tmpResp.Comments.Add(tmpItem);
                        }
                    }
                    if (request.GetInterns)
                    {
                        List<PropertyInternItem> features = new List<PropertyInternItem>();
                        var listRooms = dc.dbRntEstateInternsTBs.Where(x => x.pidEstate == tmpTb.id).ToList();
                        foreach (dbRntEstateInternsTB room in listRooms)
                        {
                            var ListEstateInternsFeatureRL = dc.dbRntEstateInternsFeatureRLs.Where(x => x.pidEstateInterns == room.id).ToList();
                            foreach (dbRntEstateInternsFeatureRL feature in ListEstateInternsFeatureRL)
                            {
                                PropertyInternItem objItem = new PropertyInternItem();
                                objItem.internType = room.pidInternsType;
                                objItem.pidInternSubType = room.pidInternsSubType;
                                objItem.featureCount = feature.count.objToInt32();
                                objItem.featureId = feature.pidInternsFeature;
                                tmpResp.features.Add(objItem);
                            }
                        }
                    }
                    ErrorLog.addLog("tmpResp", tmpResp.features.Count + "", "");
                    response.FetchedData.Add(tmpResp);
                }
            }
            response.Success = true;
            return response;
        }

        public class GetActiveCitiesResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public List<City> FetchedData { get; set; }
            public GetActiveCitiesResult()
            {
                Success = false;
                Msg = "";
                FetchedData = new List<City>();
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "GetActiveCities desc")]
        public GetActiveCitiesResult GetActiveCities(string languageCode)
        {
            DateTime createdDate = DateTime.Now;
            var response = new GetActiveCitiesResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }
            magaLocation_DataContext DC_LOCATION = maga_DataContext.DC_LOCATION;
            var currList = DC_LOCATION.LOC_TB_CITies.Where(x => x.is_active == 1);
            foreach (var tmp in currList)
            {
                string title = "";
                int languageId = 1;

                if (languageCode == "it")
                {
                    languageId = 1;
                }
                else if (languageCode == "en")
                {
                    languageId = 2;
                }
                else if (languageCode == "de")
                {
                    languageId = 3;
                }
                else if (languageCode == "es")
                {
                    languageId = 4;
                }
                else if (languageCode == "fr")
                {
                    languageId = 6;
                }

                var tmpLn = DC_LOCATION.LOC_VIEW_CITies.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == languageId);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_LOCATION.LOC_VIEW_CITies.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == App.DefLangID);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_LOCATION.LOC_VIEW_CITies.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == 2);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_LOCATION.LOC_VIEW_CITies.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == 1);
                if (tmpLn != null) title = tmpLn.title;

                var tmpItem = new City();
                tmpItem.Id = tmp.id;
                tmpItem.CountryId = tmp.pid_country.objToInt32();
                tmpItem.Name = tmpLn.title;
                tmpItem.ImgPreview = !string.IsNullOrEmpty(tmp.img_preview) ? App.HOST + "/" + tmp.img_preview : "";
                tmpItem.ImgBanner = !string.IsNullOrEmpty(tmp.img_banner) ? App.HOST + "/" + tmp.img_banner : "";
                tmpItem.GmapCoords = tmp.google_maps;
                response.FetchedData.Add(tmpItem);
            }
            response.Success = true;
            return response;
        }

        public class GetActiveFeaturesResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public List<Feature> FetchedData { get; set; }
            public GetActiveFeaturesResult()
            {
                Success = false;
                Msg = "";
                FetchedData = new List<Feature>();
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "GetActiveFeatures desc")]
        public GetActiveFeaturesResult GetFeatures(string languageCode)
        {
            int languageId = 1;
            if (languageCode == "it")
            {
                languageId = 1;
            }
            else if (languageCode == "en")
            {
                languageId = 2;
            }
            else if (languageCode == "de")
            {
                languageId = 3;
            }
            else if (languageCode == "es")
            {
                languageId = 4;
            }
            else if (languageCode == "fr")
            {
                languageId = 6;
            }
            DateTime createdDate = DateTime.Now;
            var response = new GetActiveFeaturesResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }

            using (DCmodRental dc = new DCmodRental())
            {
                var currList = dc.dbRntEstateInternsFeatureTBs.Where(x => x.isActive == 1);
                foreach (var tmp in currList)
                {
                    var tmpLn = dc.dbRntEstateInternsFeatureVIEWs.SingleOrDefault(x => x.id == tmp.id && x.pidLang == languageId);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = dc.dbRntEstateInternsFeatureVIEWs.SingleOrDefault(x => x.id == tmp.id && x.pidLang == App.DefLangID);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = dc.dbRntEstateInternsFeatureVIEWs.SingleOrDefault(x => x.id == tmp.id && x.pidLang == 1);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = dc.dbRntEstateInternsFeatureVIEWs.SingleOrDefault(x => x.id == tmp.id && x.pidLang == 2);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title))
                        tmpLn = new dbRntEstateInternsFeatureVIEW() { title = tmp.code };
                    var tmpItem = new Feature();
                    tmpItem.Id = tmp.id;
                    tmpItem.Name = tmp.pidInternsType + " - " + tmpLn.title;
                    response.FetchedData.Add(tmpItem);
                }
            }
            response.Success = true;
            return response;
        }

        public class GetActiveInternSubTypeResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public List<InternSubType> FetchedData { get; set; }
            public GetActiveInternSubTypeResult()
            {
                Success = false;
                Msg = "";
                FetchedData = new List<InternSubType>();
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "GetActiveInternSubType desc")]
        public GetActiveInternSubTypeResult GetInternSubType(string languageCode)
        {
            int languageId = 1;
            if (languageCode == "it")
            {
                languageId = 1;
            }
            else if (languageCode == "en")
            {
                languageId = 2;
            }
            else if (languageCode == "de")
            {
                languageId = 3;
            }
            else if (languageCode == "es")
            {
                languageId = 4;
            }
            else if (languageCode == "fr")
            {
                languageId = 6;
            }
            DateTime createdDate = DateTime.Now;
            var response = new GetActiveInternSubTypeResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }

            using (DCmodRental dc = new DCmodRental())
            {
                var currList = dc.dbRntEstateInternsSubTypeTBs.Where(x => x.isActive == 1);
                foreach (var tmp in currList)
                {
                    var tmpLn = dc.dbRntEstateInternsSubTypeVIEWs.SingleOrDefault(x => x.id == tmp.id && x.pidLang == languageId);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = dc.dbRntEstateInternsSubTypeVIEWs.SingleOrDefault(x => x.id == tmp.id && x.pidLang == App.DefLangID);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = dc.dbRntEstateInternsSubTypeVIEWs.SingleOrDefault(x => x.id == tmp.id && x.pidLang == 1);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = dc.dbRntEstateInternsSubTypeVIEWs.SingleOrDefault(x => x.id == tmp.id && x.pidLang == 2);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title))
                        tmpLn = new dbRntEstateInternsSubTypeVIEW() { title = tmp.code };
                    var tmpItem = new InternSubType();
                    tmpItem.Id = tmp.id;
                    tmpItem.Name = tmpLn.title;
                    response.FetchedData.Add(tmpItem);
                }
            }
            response.Success = true;
            return response;
        }
        public class GetActiveZonesResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public List<Zone> FetchedData { get; set; }
            public GetActiveZonesResult()
            {
                Success = false;
                Msg = "";
                FetchedData = new List<Zone>();
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "GetActiveZones desc")]
        public GetActiveZonesResult GetActiveZones(string languageCode)
        {
            DateTime createdDate = DateTime.Now;
            var response = new GetActiveZonesResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }

            magaLocation_DataContext DC_LOCATION = maga_DataContext.DC_LOCATION;
            var currList = DC_LOCATION.LOC_TB_ZONEs.Where(x => x.is_active == 1);

            foreach (var tmp in currList)
            {
                string title = "";
                int languageId = 1;

                if (languageCode == "it")
                {
                    languageId = 1;
                }
                else if (languageCode == "en")
                {
                    languageId = 2;
                }
                else if (languageCode == "de")
                {
                    languageId = 3;
                }
                else if (languageCode == "es")
                {
                    languageId = 4;
                }
                else if (languageCode == "fr")
                {
                    languageId = 6;
                }

                var tmpLn = DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == languageId);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == App.DefLangID);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == 2);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == 1);
                if (tmpLn != null)
                    title = tmpLn.title;

                var tmpItem = new Zone();
                tmpItem.Id = tmp.id;
                tmpItem.CityId = tmp.pid_city.objToInt32();

                string cityTitle = "";
                var tmpCityLn = DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == languageId);
                if (tmpCityLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == App.DefLangID);
                if (tmpCityLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == 2);
                if (tmpCityLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_LOCATION.LOC_VIEW_ZONEs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == 1);
                if (tmpCityLn != null)
                    cityTitle = tmpCityLn.title;

                tmpItem.CityName = cityTitle;
                tmpItem.Name = tmpLn.title;
                tmpItem.ImgPreview = !string.IsNullOrEmpty(tmp.img_preview) ? App.HOST + "/" + tmp.img_preview : "";
                tmpItem.ImgBanner = !string.IsNullOrEmpty(tmp.img_banner) ? App.HOST + "/" + tmp.img_banner : "";
                tmpItem.GmapCoords = tmp.google_maps;
                response.FetchedData.Add(tmpItem);
            }
            response.Success = true;
            return response;
        }

        public class GetActiveDiscountPromosResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public List<DiscountPromo> FetchedData { get; set; }
            public GetActiveDiscountPromosResult()
            {
                Success = false;
                Msg = "";
                FetchedData = new List<DiscountPromo>();
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "GetActiveDiscountPromos desc")]
        public GetActiveDiscountPromosResult GetActiveDiscountPromos(string languageCode)
        {
            DateTime createdDate = DateTime.Now;
            var response = new GetActiveDiscountPromosResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }
            //var currList = rntProps.DiscountPromoTBL.ToList();
            //foreach (var tmp in currList)
            //{
            //    var tmpItem = new DiscountPromo();
            //    tmpItem.Id = tmp.id;
            //    tmpItem.Type = "" + tmp.type;
            //    tmpItem.IsPercentage = tmp.isPercentage ? 1 : 0;
            //    tmpItem.ChangeAmount = tmp.changeAmount.objToDecimal();
            //    tmpItem.NightsBeforeCheckIn = tmp.nightsBeforeCheckIn.objToInt32();
            //    tmpItem.NightsBeforeCheckOut = tmp.nightsBeforeCheckOut.objToInt32();
            //    tmpItem.NightsMinToApply = tmp.nightsMinToApply.objToInt32();
            //    tmpItem.NightsMaxToApply = tmp.nightsMaxToApply.objToInt32();
            //    response.FetchedData.Add(tmpItem);
            //}
            response.Success = true;
            return response;
        }

        public class GetActiveExtrasResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public List<Extra> FetchedData { get; set; }
            public GetActiveExtrasResult()
            {
                Success = false;
                Msg = "";
                FetchedData = new List<Extra>();
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "GetActiveExtras desc")]
        public GetActiveExtrasResult GetActiveExtras(string languageCode)
        {
            DateTime createdDate = DateTime.Now;
            var response = new GetActiveExtrasResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }

            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;

            int languageId = 1;
            if (languageCode == "it")
            {
                languageId = 1;
            }
            else if (languageCode == "en")
            {
                languageId = 2;
            }
            else if (languageCode == "de")
            {
                languageId = 3;
            }
            else if (languageCode == "es")
            {
                languageId = 4;
            }
            else if (languageCode == "fr")
            {
                languageId = 6;
            }

            var currList = DC_RENTAL.RNT_TB_CONFIGs.Where(x => 1 == 1).ToList();
            foreach (var tmp in currList)
            {
                var tmpLn = DC_RENTAL.RNT_LN_CONFIGs.SingleOrDefault(x => x.pid_config == tmp.id && x.pid_lang == languageId);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_RENTAL.RNT_LN_CONFIGs.SingleOrDefault(x => x.pid_config == tmp.id && x.pid_lang == App.DefLangID);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_RENTAL.RNT_LN_CONFIGs.SingleOrDefault(x => x.pid_config == tmp.id && x.pid_lang == 2);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_RENTAL.RNT_LN_CONFIGs.SingleOrDefault(x => x.pid_config == tmp.id && x.pid_lang == 1);
                if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title))
                    continue;
                var tmpItem = new Extra();
                tmpItem.Id = tmp.id;
                tmpItem.Name = tmpLn.title;
                response.FetchedData.Add(tmpItem);
            }
            response.Success = true;
            return response;
        }

        public class GetActiveCategoryResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public List<Category> FetchedData { get; set; }
            public GetActiveCategoryResult()
            {
                Success = false;
                Msg = "";
                FetchedData = new List<Category>();
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "GetActiveCategory desc")]
        public GetActiveCategoryResult GetActiveCategory(string languageCode)
        {
            DateTime createdDate = DateTime.Now;
            var response = new GetActiveCategoryResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }

            int languageId = 1;
            if (languageCode == "it")
            {
                languageId = 1;
            }
            else if (languageCode == "en")
            {
                languageId = 2;
            }
            else if (languageCode == "de")
            {
                languageId = 3;
            }
            else if (languageCode == "es")
            {
                languageId = 4;
            }
            else if (languageCode == "fr")
            {
                languageId = 6;
            }

            var tmpItem1 = new Category();
            tmpItem1.Id = 1;
            if (languageId == 1)
                tmpItem1.Name = "Appartamento";
            else
                tmpItem1.Name = "Appartment";
            response.FetchedData.Add(tmpItem1);

            var tmpItem2 = new Category();
            tmpItem2.Id = 2;
            tmpItem2.Name = "Villa";
            response.FetchedData.Add(tmpItem2);

            //var currList = rntProps.EstateCategoryTB.Where(x => 1 == 1).ToList();
            //foreach (var tmp in currList)
            //{                
            //    var tmpItem = new Category();
            //    tmpItem.Id = tmp.id;
            //    tmpItem.Name = tmp.code;
            //    response.FetchedData.Add(tmpItem);
            //}
            response.Success = true;
            return response;
        }

        public class CreateBookingResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public string ReservationCode { get; set; }
            public decimal AgencyDiscount { get; set; }
            public bool AgencyDiscountAlreadyApplied { get; set; }
            public decimal AdvancePayment { get; set; }
            public decimal OnArrival { get; set; }
            public decimal TotalPrice { get; set; }
            public CreateBookingResult()
            {
                Success = false;
                Msg = "";
                ReservationCode = "";
                AgencyDiscount = 0;
                AgencyDiscountAlreadyApplied = false;
                AdvancePayment = 0;
                OnArrival = 0;
                TotalPrice = 0;
            }
        }
        public class BookingPrice
        {
            public decimal pr_reservation { get; set; }
            public decimal pr_total { get; set; }
            public decimal pr_part_commission_tf { get; set; }
            public decimal pr_part_commission_total { get; set; }
            public decimal pr_part_agency_fee { get; set; }
            public decimal pr_part_payment_total { get; set; }
            public decimal pr_part_forPayment { get; set; }
            public decimal pr_part_owner { get; set; }
            public decimal prTotalRate { get; set; }
            public decimal prTotalOwner { get; set; }
            public decimal prTotalCommission { get; set; }
            public decimal prDiscountSpecialOffer { get; set; }
            public decimal prDiscountLongStay { get; set; }
            public string prDiscountLongStayDesc { get; set; }
            public decimal prDiscountLongRange { get; set; }
            public string prDiscountLongRangeDesc { get; set; }
            public decimal prDiscountLastMinute { get; set; }
            public string prDiscountLastMinuteDesc { get; set; }
            public decimal pr_cityTax { get; set; }
            public decimal pr_ecoPrice { get; set; }
            public decimal pr_ecoCount { get; set; }
            public decimal pr_srsPrice { get; set; }
            public decimal prOptioniExtra { get; set; }
            public decimal pr_part_OptioniExtra { get; set; }
            public decimal pr_optioni_owner { get; set; }
            public decimal pr_optioni_feeling { get; set; }
            public int requestFullPayAccepted { get; set; }
            public int isFreeMinStay { get; set; }
            public int isFreeArrivalDay { get; set; }

        }
        public class CreateBookingRequest
        {
            public string PropertyId { get; set; }
            public DateTime DateStart { get; set; }
            public DateTime DateEnd { get; set; }
            public int NumAdults { get; set; }
            public int NumChilds { get; set; }
            public int NumEnfants { get; set; }
            public string GuestFirstName { get; set; }
            public string GuestLastName { get; set; }
            public string GuestEmail { get; set; }
            public string GuestMobilePhone { get; set; }
            public string GuestCountry { get; set; }
            public string GuestState { get; set; }
            public string GuestCity { get; set; }
            public string GuestAddress { get; set; }
            public BookingPrice ReservationPrices { get; set; }
            public CreateBookingRequest()
            {
                ReservationPrices = new BookingPrice();
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "CreateBooking desc")]
        public CreateBookingResult CreateBooking(CreateBookingRequest request)
        {
            var DC_RENTAL = maga_DataContext.DC_RENTAL;
            DateTime createdDate = DateTime.Now;
            var response = new CreateBookingResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }
            if (
                string.IsNullOrEmpty(request.GuestFirstName)
                //|| string.IsNullOrEmpty(request.GuestLastName)
                || string.IsNullOrEmpty(request.GuestEmail)
                //|| string.IsNullOrEmpty(request.GuestMobilePhone)
                //|| string.IsNullOrEmpty(request.GuestCountry)
                //|| string.IsNullOrEmpty(request.GuestState)
                //|| string.IsNullOrEmpty(request.GuestCity)
                || request.DateStart == null
                || request.DateEnd == null
                || request.NumAdults == null
                || request.NumChilds == null
                || request.NumEnfants == null
                )
            {
                response.Msg = "ALL PARAMETERS ARE REQUIRED";
                return response;
            }
            int IdEstate = request.PropertyId.ToInt32();
            var estateTb = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1 && x.is_online_booking == 1);
            if (estateTb == null)
            {
                response.Msg = "PROPERTY NON FOUND OR NOT ACTIVE";
                return response;
            }
            using (DCmodRental dcRnt = new DCmodRental())
            {
                var agentActiveContractIds = dcRnt.dbRntAgentContractTBLs.Where(x => x.dtStart <= createdDate && x.dtEnd >= createdDate).Select(x => x.id).ToList();
                var fltEstateIdsList = dcRnt.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && agentActiveContractIds.Contains(x.pidAgentContract)).Select(x => x.pidEstate).ToList();
                if (!fltEstateIdsList.Contains(IdEstate))
                {
                    response.Msg = "PROPERTY NOT ACTIVE FOR AGENCY";
                    return response;
                }
            }
            rntExts.PreReservationPrices currOutPrice = new rntExts.PreReservationPrices();
            currOutPrice.dtStart = request.DateStart;
            currOutPrice.dtEnd = request.DateEnd;
            currOutPrice.numPersCount = request.NumAdults + request.NumChilds;
            currOutPrice.pr_discount_owner = 0;
            currOutPrice.pr_discount_commission = 0;
            currOutPrice.part_percentage = estateTb.pr_percentage.objToDecimal();

            currOutPrice.fillAgentDetails(agentTbl);

            decimal _pr_total = rntUtils.rntEstate_getPrice(0, IdEstate, ref currOutPrice);           

            bool _isAvailable = rntUtils.rntEstate_isAvailable(IdEstate, request.DateStart, request.DateEnd, 0) == null;
            if (!_isAvailable)            
            {
                response.Msg = "PROPERTY NOT AVAILABLE IN SELECTED DATES";
                return response;
            }
            currOutPrice.prTotal = _pr_total;

            if (string.IsNullOrEmpty(request.GuestEmail) || !request.GuestEmail.Trim().isEmail())
            {
                response.Msg = "GuestEmail is NOT VALID";
                return response;
            }
            var tmp = new dbAuthClientTBL();
            using (DCmodAuth dc = new DCmodAuth())
            {
                tmp = dc.dbAuthClientTBLs.LastOrDefault(x => x.pidAgent == agentTbl.id && x.contactEmail == request.GuestEmail);
                if (tmp == null)
                {
                    tmp = new dbAuthClientTBL();
                    tmp.uid = Guid.NewGuid();
                    tmp.createdDate = DateTime.Now;
                    tmp.createdUserID = 1;
                    tmp.createdUserNameFull = "System";
                    tmp.isActive = 1;
                    tmp.typeCode = "clientiagenzie";
                    tmp.pidAgent = agentTbl.id;
                    dc.Add(tmp);
                    dc.SaveChanges();
                    tmp.code = tmp.id.ToString().fillString("0", 6, false);
                }
                tmp.nameHonorific = "";
                tmp.nameFull = request.GuestFirstName + " " + request.GuestLastName;
                tmp.contactEmail = request.GuestEmail;
                tmp.contactPhoneMobile = request.GuestMobilePhone;
                tmp.locCountry = request.GuestCountry;
                tmp.locState = request.GuestState;
                tmp.locCity = request.GuestCity;
                tmp.locAddress = request.GuestAddress;
                dc.SaveChanges();
            }
            USR_TBL_CLIENT currClientTBL = new USR_TBL_CLIENT();
            currClientTBL.id = -1;
            currClientTBL.contact_email = agentTbl.contactEmail;
            currClientTBL.name_full = agentTbl.nameCompany;
            currClientTBL.name_honorific = "";
            currClientTBL.loc_country = agentTbl.locCountry;
            currClientTBL.pid_discount = -1;
            currClientTBL.pid_lang = agentTbl.pidLang;
            currClientTBL.isCompleted = 0;


            RNT_TBL_RESERVATION _currTBL = rntUtils.newReservation();
            _currTBL.pid_estate = IdEstate;
            _currTBL.pidEstateCity = estateTb.pid_city.objToInt32();
            _currTBL.pid_creator = CommonUtilities.getSYS_SETTING("rntAgentAPIv1_reservationCreatorId").ToInt32();
            if (_currTBL.pid_creator == 0) _currTBL.pid_creator = 1;
            _currTBL.state_pid = 4;
            _currTBL.state_body = "";
            _currTBL.state_date = DateTime.Now;
            _currTBL.state_pid_user = 1;
            _currTBL.state_subject = "Prenotazione AGENT API";
            _currTBL.is_booking = 1;

            _currTBL.dtStart = request.DateStart;
            _currTBL.dtEnd = request.DateEnd;
            _currTBL.dtStartTime = "000000";
            _currTBL.dtEndTime = "000000";

            _currTBL.num_adult = request.NumAdults;
            _currTBL.num_child_over = request.NumChilds;
            _currTBL.num_child_min = request.NumEnfants;

            currOutPrice.CopyTo(ref _currTBL);

            _currTBL.pr_deposit = estateTb.pr_deposit;
            _currTBL.srs_ext_meetingPoint = estateTb.srs_ext_meetingPoint;
            _currTBL.pr_depositWithCard = estateTb.pr_depositWithCard;

            _currTBL.pr_discount_owner = 0;
            _currTBL.pr_discount_commission = 0;
            _currTBL.pr_discount_desc = "";

            // salviamo prima e aggiorniamo la cache per evitare overbooking
            DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
            DC_RENTAL.SubmitChanges();


            int _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHoursOnline").ToInt32();
            _currTBL.block_comments = "Scadenza predefinita InstantBooking [" + _blockHours + " ore]";
            if (_currTBL.agentID != 0)
            {
                _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaulHoursAgent").ToInt32();
                _currTBL.block_comments = "Scadenza predefinita Agenzie [" + _blockHours + " ore]";
            }
            if (_blockHours == 0)
            {
                _currTBL.block_comments = "Nessuna scadenza";
                _currTBL.block_expire = null;
                _currTBL.block_expire_hours = 0;
                _currTBL.block_pid_user = 1;
            }
            else
            {
                _currTBL.block_expire = DateTime.Now.AddHours(_blockHours);
                _currTBL.block_expire_hours = _blockHours;
                _currTBL.block_pid_user = 1;
            }


            _currTBL.cl_id = currClientTBL.id;
            _currTBL.agentClientID = tmp.id;
            _currTBL.cl_email = string.IsNullOrEmpty(tmp.contactEmail) ? currClientTBL.contact_email : tmp.contactEmail;
            _currTBL.cl_name_full = string.IsNullOrEmpty(tmp.nameFull) ? currClientTBL.name_full : tmp.nameFull;
            _currTBL.cl_name_honorific = string.IsNullOrEmpty(tmp.nameHonorific) ? currClientTBL.name_honorific : tmp.nameHonorific;
            _currTBL.cl_loc_country = string.IsNullOrEmpty(tmp.locCountry) ? currClientTBL.loc_country : tmp.locCountry;
            _currTBL.cl_pid_lang = tmp.pidLang.objToInt32() == 0 ? currClientTBL.pid_lang.objToInt32() : tmp.pidLang.objToInt32();
            _currTBL.cl_isCompleted = currClientTBL.isCompleted;

            _currTBL.cl_browserInfo = "Prenotazione AGENT API";
            _currTBL.cl_browserIP = "";

            int pidOperator = agentTbl.pidReferer.objToInt32();
            if (pidOperator == 0) pidOperator = AdminUtilities.usr_getAvailableOperator(AdminUtilities.zone_countryId(currClientTBL.loc_country), _currTBL.cl_pid_lang.objToInt32());
            _currTBL.pid_operator = pidOperator;

            _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
            _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);           

            rntUtils.rntReservation_setDefaults(ref _currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            rntUtils.rntReservation_mailNewCreation(_currTBL, true, true, true, true, 1); // send mails
            rntUtils.reservation_checkPartPayment(_currTBL, false);

            response.Msg = "OK";
            response.ReservationCode = "" + _currTBL.id;
            response.AgencyDiscount = _currTBL.agentCommissionPrice.objToDecimal();
            //response.AgencyDiscountAlreadyApplied = _currTBL.agentCommissionNotInTotal == 1;
            response.AdvancePayment = _currTBL.pr_part_payment_total.objToDecimal();
            response.OnArrival = _currTBL.pr_part_owner.objToDecimal();
            response.TotalPrice = _currTBL.pr_total.objToDecimal();

            response.Success = true;
            return response;
        }

        public class UpdateBookingResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public decimal AgencyDiscount { get; set; }
            public bool AgencyDiscountAlreadyApplied { get; set; }
            public decimal AdvancePayment { get; set; }
            public decimal OnArrival { get; set; }
            public decimal TotalPrice { get; set; }
            public UpdateBookingResult()
            {
                Success = false;
                Msg = "";
                AgencyDiscount = 0;
                AgencyDiscountAlreadyApplied = false;
                AdvancePayment = 0;
                OnArrival = 0;
                TotalPrice = 0;
            }
        }
        public class UpdateBookingRequest
        {
            public string ReservationCode { get; set; }
            public bool Cancelled { get; set; }
            public DateTime DateStart { get; set; }
            public DateTime DateEnd { get; set; }
            public int NumAdults { get; set; }
            public int NumChilds { get; set; }
            public int NumEnfants { get; set; }
            public string GuestFirstName { get; set; }
            public string GuestLastName { get; set; }
            public string GuestEmail { get; set; }
            public string GuestMobilePhone { get; set; }
            public string GuestCountry { get; set; }
            public string GuestState { get; set; }
            public string GuestCity { get; set; }
            public string GuestAddress { get; set; }
            public BookingPrice ReservationPrices { get; set; }
            public UpdateBookingRequest()
            {
                ReservationPrices = new BookingPrice();
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(EnableSession = false, Description = "UpdateBooking desc")]

        public UpdateBookingResult UpdateBooking(UpdateBookingRequest request)
        {
            var DC_RENTAL = maga_DataContext.DC_RENTAL;
            DateTime createdDate = DateTime.Now;
            var response = new UpdateBookingResult();
            if (!IsAuthOK())
            {
                response.Msg = Authentication.Msg;
                return response;
            }
            if (
                string.IsNullOrEmpty(request.GuestFirstName)
                //|| string.IsNullOrEmpty(request.GuestLastName)
                || string.IsNullOrEmpty(request.GuestEmail)
                //|| string.IsNullOrEmpty(request.GuestMobilePhone)
                //|| string.IsNullOrEmpty(request.GuestCountry)
                //|| string.IsNullOrEmpty(request.GuestState)
                //|| string.IsNullOrEmpty(request.GuestCity)
                || request.DateStart == null
                || request.DateEnd == null
                || request.NumAdults == null
                || request.NumChilds == null
                || request.NumEnfants == null
                )
            {
                response.Msg = "ALL PARAMETERS ARE REQUIRED";
                return response;
            }
            long ResId = request.ReservationCode.ToInt64();
            RNT_TBL_RESERVATION _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == ResId);
            if (_currTBL == null)
            {
                response.Msg = "RESERVATION NOT FOUND";
                return response;
            }
            var IdEstate = _currTBL.pid_estate.objToInt32();
            var estateTb = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (estateTb == null)
            {
                response.Msg = "PROPERTY NON FOUND OR NOT ACTIVE";
                return response;
            }
            if (request.Cancelled == true)
            {
                if (_currTBL.state_pid == 3)
                {
                    response.Success = true;
                    return response;
                }
                rntUtils.rntReservation_onStateChange(_currTBL);
                _currTBL.state_pid = 3;
                _currTBL.state_body = "";
                _currTBL.state_date = DateTime.Now;
                _currTBL.state_pid_user = 1;
                _currTBL.state_subject = "Cancellazione AGENT API";
                DC_RENTAL.SubmitChanges();
                rntUtils.rntReservation_onChange(_currTBL);
                response.Success = true;
                return response;
            }
            using (DCmodRental dcRnt = new DCmodRental())
            {
                var agentActiveContractIds = dcRnt.dbRntAgentContractTBLs.Where(x => x.dtStart <= createdDate && x.dtEnd >= createdDate).Select(x => x.id).ToList();
                var fltEstateIdsList = dcRnt.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && agentActiveContractIds.Contains(x.pidAgentContract)).Select(x => x.pidEstate).ToList();
                if (!fltEstateIdsList.Contains(IdEstate))
                {
                    response.Msg = "PROPERTY NOT ACTIVE FOR AGENCY";
                    return response;
                }
            }
            rntExts.PreReservationPrices currOutPrice = new rntExts.PreReservationPrices();
            currOutPrice.dtStart = request.DateStart;
            currOutPrice.dtEnd = request.DateEnd;
            currOutPrice.numPersCount = request.NumAdults + request.NumChilds;
            currOutPrice.pr_discount_owner = 0;
            currOutPrice.pr_discount_commission = 0;
            currOutPrice.part_percentage = estateTb.pr_percentage.objToDecimal();

            currOutPrice.fillAgentDetails(agentTbl);

            decimal _pr_total = rntUtils.rntEstate_getPrice(0, IdEstate, ref currOutPrice);
            bool _isAvailable = rntUtils.rntEstate_isAvailable(IdEstate, request.DateStart, request.DateEnd, 0) == null;
            if (!_isAvailable)
            {
                response.Msg = "PROPERTY NOT AVAILABLE IN SELECTED DATES";
                return response;
            }
            currOutPrice.prTotal = _pr_total;
            var tmp = new dbAuthClientTBL();
            using (DCmodAuth dc = new DCmodAuth())
            {
                tmp = dc.dbAuthClientTBLs.LastOrDefault(x => x.id == _currTBL.agentClientID);
                if (tmp == null)
                {
                    tmp = new dbAuthClientTBL();
                    tmp.uid = Guid.NewGuid();
                    tmp.createdDate = DateTime.Now;
                    tmp.createdUserID = 1;
                    tmp.createdUserNameFull = "System";
                    tmp.isActive = 1;
                    tmp.typeCode = "clientiagenzie";
                    tmp.pidAgent = agentTbl.id;
                    dc.Add(tmp);
                    dc.SaveChanges();
                    tmp.code = tmp.id.ToString().fillString("0", 6, false);
                }
                tmp.nameHonorific = "";
                tmp.nameFull = request.GuestFirstName + " " + request.GuestLastName;
                tmp.contactEmail = request.GuestEmail;
                tmp.contactPhoneMobile = request.GuestMobilePhone;
                tmp.locCountry = request.GuestCountry;
                tmp.locState = request.GuestState;
                tmp.locCity = request.GuestCity;
                tmp.locAddress = request.GuestAddress;
                dc.SaveChanges();
            }
            USR_TBL_CLIENT currClientTBL = new USR_TBL_CLIENT();
            currClientTBL.id = -1;
            currClientTBL.contact_email = agentTbl.contactEmail;
            currClientTBL.name_full = agentTbl.nameCompany;
            currClientTBL.name_honorific = "";
            currClientTBL.loc_country = agentTbl.locCountry;
            currClientTBL.pid_discount = -1;
            currClientTBL.pid_lang = agentTbl.pidLang;
            currClientTBL.isCompleted = 0;


            _currTBL.dtStart = request.DateStart;
            _currTBL.dtEnd = request.DateEnd;

            _currTBL.num_adult = request.NumAdults;
            _currTBL.num_child_over = request.NumChilds;
            _currTBL.num_child_min = request.NumEnfants;
            currOutPrice.CopyTo(ref _currTBL);

            //set status again booked if is cancelled before
            if (_currTBL.state_pid == 3)
            {
                _currTBL.state_pid = 4;
                _currTBL.state_body = "";
                _currTBL.state_date = DateTime.Now;
                _currTBL.state_pid_user = 1;
                _currTBL.state_subject = "Prenotazione AGENT API";
            }
            DC_RENTAL.SubmitChanges();

            _currTBL.cl_id = currClientTBL.id;
            _currTBL.agentClientID = tmp.id;
            _currTBL.cl_email = string.IsNullOrEmpty(tmp.contactEmail) ? currClientTBL.contact_email : tmp.contactEmail;
            _currTBL.cl_name_full = string.IsNullOrEmpty(tmp.nameFull) ? currClientTBL.name_full : tmp.nameFull;
            _currTBL.cl_name_honorific = string.IsNullOrEmpty(tmp.nameHonorific) ? currClientTBL.name_honorific : tmp.nameHonorific;
            _currTBL.cl_loc_country = string.IsNullOrEmpty(tmp.locCountry) ? currClientTBL.loc_country : tmp.locCountry;
            _currTBL.cl_pid_lang = tmp.pidLang.objToInt32() == 0 ? currClientTBL.pid_lang.objToInt32() : tmp.pidLang.objToInt32();
            _currTBL.cl_isCompleted = currClientTBL.isCompleted;

            //for roomNumber chnage
            //int roomNumber = rntUtils.getEstateRoomNumber(_currTBL, estateTb.baseAvailability.objToInt32());
            //_currTBL.pidEstateRoomNumber = roomNumber;

            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            rntUtils.reservation_checkPartPayment(_currTBL, false);

            response.Msg = "OK";
            response.AgencyDiscount = _currTBL.agentCommissionPrice.objToDecimal();
            //response.AgencyDiscountAlreadyApplied = _currTBL.agentCommissionNotInTotal == 1;
            response.AdvancePayment = _currTBL.pr_part_payment_total.objToDecimal();
            response.OnArrival = _currTBL.pr_part_owner.objToDecimal();
            response.TotalPrice = _currTBL.pr_total.objToDecimal();

            response.Success = true;
            return response;
        }

        //public UpdateBookingResult UpdateBooking(UpdateBookingRequest request)
        //{
        //    var DC_RENTAL = maga_DataContext.DC_RENTAL;
        //    DateTime createdDate = DateTime.Now;
        //    var response = new UpdateBookingResult();
        //    if (!IsAuthOK())
        //    {
        //        response.Msg = Authentication.Msg;
        //        return response;
        //    }
        //    if (
        //        string.IsNullOrEmpty(request.GuestFirstName)                
        //        || string.IsNullOrEmpty(request.GuestEmail)                
        //        || request.DateStart == null
        //        || request.DateEnd == null
        //        || request.NumAdults == null
        //        || request.NumChilds == null
        //        || request.NumEnfants == null
        //        )
        //    {
        //        response.Msg = "ALL PARAMETERS ARE REQUIRED";
        //        return response;
        //    }
        //    long ResId = request.ReservationCode.ToInt64();
        //    RNT_TBL_RESERVATION _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == ResId);
        //    if (_currTBL == null)
        //    {
        //        response.Msg = "RESERVATION NOT FOUND";
        //        return response;
        //    }
        //    var IdEstate = _currTBL.pid_estate.objToInt32();
        //    var estateTb = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
        //    if (estateTb == null)
        //    {
        //        response.Msg = "PROPERTY NON FOUND OR NOT ACTIVE";
        //        return response;
        //    }
        //    if (request.Cancelled == true)
        //    {
        //        if (_currTBL.state_pid == 3)
        //        {
        //            response.Success = true;
        //            return response;
        //        }
        //        rntUtils.rntReservation_onStateChange(_currTBL);
        //        _currTBL.state_pid = 3;
        //        _currTBL.state_body = "";
        //        _currTBL.state_date = DateTime.Now;
        //        _currTBL.state_pid_user = 1;
        //        _currTBL.state_subject = "Cancellazione AGENT API";
        //        DC_RENTAL.SubmitChanges();
        //        rntUtils.rntReservation_onChange(_currTBL);
        //        response.Success = true;
        //        return response;
        //    }
        //    using (DCmodRental dcRnt = new DCmodRental())
        //    {
        //        var agentActiveContractIds = dcRnt.dbRntAgentContractTBLs.Where(x => x.dtStart <= createdDate && x.dtEnd >= createdDate).Select(x => x.id).ToList();
        //        var fltEstateIdsList = dcRnt.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentID && agentActiveContractIds.Contains(x.pidAgentContract)).Select(x => x.pidEstate).ToList();
        //        if (!fltEstateIdsList.Contains(IdEstate))
        //        {
        //            response.Msg = "PROPERTY NOT ACTIVE FOR AGENCY";
        //            return response;
        //        }
        //    }
        //    rntExts.PreReservationPrices currOutPrice = new rntExts.PreReservationPrices();
        //    currOutPrice.dtStart = request.DateStart;
        //    currOutPrice.dtEnd = request.DateEnd;
        //    currOutPrice.numPersCount = request.NumAdults + request.NumChilds;
        //    currOutPrice.pr_discount_owner = 0;
        //    currOutPrice.pr_discount_commission = 0;
        //    currOutPrice.part_percentage = estateTb.pr_percentage.objToDecimal();

        //    currOutPrice.fillAgentDetails(agentTbl);

        //    decimal _pr_total = rntUtils.rntEstate_getPrice(0, IdEstate, ref currOutPrice);
        //    RNT_TBL_RESERVATION tmpRes;
        //    if (!rntUtils.rntEstate_isAvailable(IdEstate, request.DateStart, request.DateEnd, _currTBL.id, 0))
        //    {
        //        response.Msg = "PROPERTY NOT AVAILABLE IN SELECTED DATES";
        //        return response;
        //    }
        //    //  currOutPrice.prTotal = _pr_total;
        //    var currClient = new dbAuthClientTBL();
        //    using (DCmodAuth dc = new DCmodAuth())
        //    {
        //        currClient = dc.dbAuthClientTBLs.LastOrDefault(x => x.id == _currTBL.agentClientID);
        //        if (currClient == null)
        //        {
        //            currClient = new dbAuthClientTBL();
        //            currClient.uid = Guid.NewGuid();
        //            currClient.createdDate = DateTime.Now;
        //            currClient.createdUserID = 1;
        //            currClient.createdUserNameFull = "System";
        //            currClient.isActive = 1;
        //            currClient.typeCode = "clientiagenzie";
        //            currClient.pidAgent = agentTbl.id;
        //            dc.Add(currClient);
        //            dc.SaveChanges();
        //            currClient.code = currClient.id.ToString().fillString("0", 6, false);
        //            dc.SaveChanges();
        //            _currTBL.agentClientID = currClient.id;
        //        }
        //        currClient.nameHonorific = "";
        //        currClient.nameFull = request.GuestFirstName + " " + request.GuestLastName;
        //        currClient.contactEmail = request.GuestEmail;
        //        currClient.contactPhoneMobile = request.GuestMobilePhone;
        //        currClient.locCountry = request.GuestCountry;
        //        currClient.locState = request.GuestState;
        //        currClient.locCity = request.GuestCity;
        //        currClient.locAddress = request.GuestAddress;
        //        dc.SaveChanges();
        //    }

        //    _currTBL.dtStart = request.DateStart;
        //    _currTBL.dtEnd = request.DateEnd;

        //    _currTBL.num_adult = request.NumAdults;
        //    _currTBL.num_child_over = request.NumChilds;
        //    _currTBL.num_child_min = request.NumEnfants;

        //    //currOutPrice.CopyTo(ref _currTBL);
        //    //request.ReservationPrices.CopyBookingPrice(_currTBL);

        //    //decimal commissionamount = 0;

        //    var bcom_totalForOwner = _currTBL.prTotalRate - commissionamount;
        //    _currTBL.prTotalCommission = currEstate.pr_percentage.objToInt32() * bcom_totalForOwner / 100;
        //    _currTBL.prTotalOwner = bcom_totalForOwner - _currTBL.prTotalCommission;
                                  
        //    //if (estateTb.ownerContractPrice_commissionOnNet.objToInt32() == 1)
        //    //{
        //    //    _currTBL.prTotalOwner = currOutPrice.prTotalOwner;
        //    //    _currTBL.prTotalCommission = _currTBL.prTotalRate - commissionamount - _currTBL.prTotalOwner;
        //    //}
        //    //else if (estateTb.ownerContractPrice_commissionOnNet.objToInt32() == 0)
        //    //{
        //    //    _currTBL.prTotalCommission = currOutPrice.prTotalCommission;
        //    //    _currTBL.prTotalOwner = _currTBL.prTotalRate - commissionamount - _currTBL.prTotalCommission;
        //    //}
        //    //else if (estateTb.ownerContractPrice_commissionOnNet.objToInt32() == 2)
        //    //{
        //    //    decimal ownerper = ((currOutPrice.prTotalOwner * 100) / currOutPrice.prTotalRate) / 100;
        //    //    _currTBL.prTotalOwner = decimal.Multiply((_currTBL.prTotalRate.objToDecimal() - commissionamount), ownerper);
        //    //    _currTBL.prTotalCommission = ((_currTBL.prTotalRate.objToDecimal() - commissionamount) - _currTBL.prTotalOwner);

        //    //}

        //    _currTBL.cl_email = currClient.contactEmail;
        //    _currTBL.cl_name_full = currClient.nameFull;
        //    _currTBL.cl_name_honorific = currClient.nameHonorific;
        //    _currTBL.cl_loc_country = currClient.locCountry;

        //    if (currClient.pidLang == "it")
        //        _currTBL.cl_pid_lang = 1;
        //    else if(currClient.pidLang=="en")
        //        _currTBL.cl_pid_lang = 2;
            
        //    _currTBL.cl_isCompleted = 1;

        //    DC_RENTAL.SubmitChanges();
        //    rntUtils.rntReservation_onChange(_currTBL);
        //    //  rntUtils.reservation_checkPartPayment(_currTBL, false); No payment will be there for Sleep 

        //    response.Msg = "OK";
        //    response.AgencyDiscount = _currTBL.agentCommissionPrice.objToDecimal();
        //    response.AgencyDiscountAlreadyApplied = _currTBL.agentCommissionNotInTotal == 1;
        //    response.AdvancePayment = _currTBL.pr_part_payment_total.objToDecimal();
        //    response.OnArrival = _currTBL.pr_part_owner.objToDecimal();
        //    response.TotalPrice = _currTBL.pr_total.objToDecimal();

        //    response.Success = true;
        //    return response;
        //}


    }
}
