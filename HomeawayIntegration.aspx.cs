using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using System.Xml;
using System.Text;
using System.IO;
using ModRental;

/*  ---- NOTES

1) I added bathrooms and bedrooms tags beacause they are compulsary and in it added roomsubType tag.In roomsubType tag i added static values from the Enumration at now.

2) Before only if is_google_maps field was 1 then only we were displaying geoCode tag.but geoCode tag is compulsary so i changed that if is_google_maps is 0 then also i added this tag and in it set latitude and longitude values to 0.00 and showExactLocation,enableMap and allowTravelersToZoom to false.

*/

namespace RentalInRome
{
    public partial class HomeawayIntegration : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        private magaLocation_DataContext DC_LOCATION;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_LOCATION = maga_DataContext.DC_LOCATION;
            if (!IsPostBack)
            {
                CreateListingXML();
            }
        }

        private void CreateListingXML()
        {
            try
            {
                List<RNT_TB_ESTATE> _estateList = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.haAdvertiserId != "Empty" && x.haAdvertiserId != null && x.is_HomeAway == 1).OrderBy(x => x.code).ToList();
                TextWriter wr = new StringWriter();

                XmlTextWriter xWriter = new XmlTextWriter(wr);
                xWriter.WriteStartDocument();

                //Root Element
                xWriter.WriteStartElement("listingBatch");

                xWriter.WriteElementString("documentVersion", "3.0");

                //Advertisers Element Start
                xWriter.WriteStartElement("advertisers");

                //Advertiser Element Start
                xWriter.WriteStartElement("advertiser");

                xWriter.WriteElementString("assignedId", _estateList[0].haAdvertiserId);//Need To be set 

                //listings Element Start
                xWriter.WriteStartElement("listings");

                foreach (RNT_TB_ESTATE _estate in _estateList)
                {

                    List<RNT_LN_ESTATE> _rList = DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == _estate.id).ToList();

                    //cheking for haDescription and haHeadline
                    List<string> lstDesc = new List<string>();
                    List<string> lstHeadline = new List<string>();
                    foreach (RNT_LN_ESTATE objDesc in _rList)
                    {
                        if (objDesc.pid_lang != 8 && objDesc.pid_lang != 6 && objDesc.pid_lang != 12)
                        {
                            if (objDesc.haDescription != null && objDesc.haDescription != "")
                            {
                                lstDesc.Add(objDesc.haDescription.Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"));
                            }

                            if (objDesc.haHeadLine != null && objDesc.haHeadLine != "")
                            {
                                lstHeadline.Add(objDesc.haHeadLine.Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"));
                            }

                        }
                    }


                    //listings Element Start

                    if (lstDesc.Count > 0 && lstHeadline.Count > 0)
                    {
                        xWriter.WriteStartElement("listing");

                        xWriter.WriteElementString("externalId", Convert.ToString(_estate.id));//Need to be add

                        //adContent Element Start
                        xWriter.WriteStartElement("adContent");

                        //description Element Start
                        xWriter.WriteStartElement("description");

                        xWriter.WriteStartElement("texts");

                        //texts Element Start
                        foreach (RNT_LN_ESTATE objDesc in _rList)
                        {
                            if (objDesc.pid_lang != 8 && objDesc.pid_lang != 6 && objDesc.pid_lang != 12)
                            {
                                string lang = "";
                                if (objDesc.pid_lang == 1)
                                {
                                    lang = "it";
                                }
                                else if (objDesc.pid_lang == 2)
                                {
                                    lang = "en";
                                }
                                else if (objDesc.pid_lang == 3)
                                {
                                    lang = "es";
                                }
                                else if (objDesc.pid_lang == 4)
                                {
                                    lang = "fr";
                                }
                                else if (objDesc.pid_lang == 5)
                                {
                                    lang = "de";
                                }

                                else if (objDesc.pid_lang == 7)
                                {
                                    lang = "nl";
                                }

                                else if (objDesc.pid_lang == 9)
                                {
                                    lang = "pt";
                                }
                                else if (objDesc.pid_lang == 10)
                                {
                                    lang = "fi";
                                }
                                else if (objDesc.pid_lang == 11)
                                {
                                    lang = "sv";
                                }
                                if (objDesc.haDescription != null && objDesc.haDescription != "")
                                {
                                    //Start text
                                    xWriter.WriteStartElement("text");
                                    xWriter.WriteAttributeString("locale", lang);
                                    xWriter.WriteElementString("textValue", objDesc.haDescription.Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"));
                                    xWriter.WriteEndElement();
                                    //End text
                                }

                            }
                        }
                        //End texts
                        xWriter.WriteEndElement();
                        //End description
                        xWriter.WriteEndElement();


                        //headline Element Start
                        xWriter.WriteStartElement("headline");

                        xWriter.WriteStartElement("texts");


                        //texts Element Start
                        foreach (RNT_LN_ESTATE objDesc in _rList)
                        {
                            if (objDesc.pid_lang != 8 && objDesc.pid_lang != 6 && objDesc.pid_lang != 12)
                            {
                                string lang = "";
                                if (objDesc.pid_lang == 1)
                                {
                                    lang = "it";
                                }
                                else if (objDesc.pid_lang == 2)
                                {
                                    lang = "en";
                                }
                                else if (objDesc.pid_lang == 3)
                                {
                                    lang = "es";
                                }
                                else if (objDesc.pid_lang == 4)
                                {
                                    lang = "fr";
                                }
                                else if (objDesc.pid_lang == 5)
                                {
                                    lang = "de";
                                }

                                else if (objDesc.pid_lang == 7)
                                {
                                    lang = "nl";
                                }

                                else if (objDesc.pid_lang == 9)
                                {
                                    lang = "pt";
                                }
                                else if (objDesc.pid_lang == 10)
                                {
                                    lang = "fi";
                                }
                                else if (objDesc.pid_lang == 11)
                                {
                                    lang = "sv";
                                }
                                if (objDesc.haHeadLine != null && objDesc.haHeadLine != "")
                                {
                                    //text Element Start
                                    xWriter.WriteStartElement("text");
                                    xWriter.WriteAttributeString("locale", lang);
                                    xWriter.WriteElementString("textValue", objDesc.haHeadLine.Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n").htmlDecode());
                                    //End text
                                    xWriter.WriteEndElement();
                                }
                            }
                        }
                        //End texts
                        xWriter.WriteEndElement();
                        //End headline
                        xWriter.WriteEndElement();


                        //End adContent
                        xWriter.WriteEndElement();


                        //location Element Start
                        xWriter.WriteStartElement("location");

                        LOC_LN_CITY objRegionView = DC_LOCATION.LOC_LN_CITies.FirstOrDefault(x => x.pid_city == _estate.pid_city);
                        //location Element Start
                        xWriter.WriteStartElement("address");

                        xWriter.WriteElementString("addressLine1", cleareAddress(_estate.loc_address + ""));
                        xWriter.WriteElementString("addressLine2", "");
                        xWriter.WriteElementString("city", objRegionView.title);
                        xWriter.WriteElementString("stateOrProvince", "Roma");
                        xWriter.WriteElementString("country", "Italy");
                        xWriter.WriteElementString("postalCode", _estate.loc_zip_code);

                        //End address
                        xWriter.WriteEndElement();



                        if (_estate.is_google_maps == 1)
                        {
                            xWriter.WriteElementString("allowTravelersToZoom", "true");

                            xWriter.WriteElementString("enableMap", "true");
                            //geoCode Element Start
                            xWriter.WriteStartElement("geoCode");

                            //latLng Element Start
                            xWriter.WriteStartElement("latLng");
                            if (_estate.google_maps != null)
                            {
                                xWriter.WriteElementString("latitude", _estate.google_maps.Split('|')[0].Replace(",", "."));
                                xWriter.WriteElementString("longitude", _estate.google_maps.Split('|')[1].Replace(",", "."));
                            }
                            else
                            {
                                xWriter.WriteElementString("latitude", "0.00");
                                xWriter.WriteElementString("longitude", "0.00");
                            }

                            //End latLng
                            xWriter.WriteEndElement();
                            xWriter.WriteElementString("precision", "ADDRESS");
                            //End geoCode
                            xWriter.WriteEndElement();

                            xWriter.WriteElementString("showExactLocation", "true");
                        }
                        else
                        {
                            xWriter.WriteElementString("allowTravelersToZoom", "false");

                            xWriter.WriteElementString("enableMap", "false");

                            //geoCode Element Start
                            xWriter.WriteStartElement("geoCode");
                            //latLng Element Start
                            xWriter.WriteStartElement("latLng");

                            xWriter.WriteElementString("latitude", "0.00");
                            xWriter.WriteElementString("longitude", "0.00");

                            //End latLng
                            xWriter.WriteEndElement();
                            xWriter.WriteElementString("precision", "ADDRESS");

                            //End geoCode
                            xWriter.WriteEndElement();

                            xWriter.WriteElementString("showExactLocation", "false");


                        }
                        int cntPoint = 0;
                        List<RNT_RL_ESTATE_POINT> objPoints = DC_RENTAL.RNT_RL_ESTATE_POINTs.Where(x => x.pid_estate == _estate.id).ToList();
                        if (objPoints.Count > 0)
                        {
                            foreach (RNT_RL_ESTATE_POINT objPoint in objPoints)
                            {
                                LOC_TB_POINT objPlaceType = DC_LOCATION.LOC_TB_POINT.FirstOrDefault(x => x.id == objPoint.pid_point);
                                if (!string.IsNullOrEmpty(objPlaceType.haPlaceType) && objPlaceType.haPlaceType != "0" && objPoint.distance.ToInt32() > 0)
                                {
                                    cntPoint++;
                                }
                            }
                            if (cntPoint > 0)
                            {
                                //Start nearestPlaces
                                xWriter.WriteStartElement("nearestPlaces");
                                foreach (RNT_RL_ESTATE_POINT objPoint in objPoints)
                                {
                                    LOC_TB_POINT objPlaceType = DC_LOCATION.LOC_TB_POINT.FirstOrDefault(x => x.id == objPoint.pid_point);
                                    if (objPlaceType.haPlaceType != null && objPlaceType.haPlaceType != "0" && objPoint.distance != null && IsNumber(objPoint.distance) == true)
                                    {
                                        //Start nearestPlace
                                        xWriter.WriteStartElement("nearestPlace");
                                        xWriter.WriteAttributeString("placeType", objPlaceType.haPlaceType);
                                        if (objPoint.distance.ToInt32() < 1000)
                                        {
                                            xWriter.WriteElementString("distance", Convert.ToString(objPoint.distance));
                                            xWriter.WriteElementString("distanceUnit", "METRES");
                                        }
                                        else
                                        {
                                            xWriter.WriteElementString("distance", (objPoint.distance.ToInt32() / 1000).ToString("N1").Replace(".", "").Replace(",", "."));
                                            xWriter.WriteElementString("distanceUnit", "KILOMETRES");
                                        }
                                        List<LOC_LN_POINT> objnames = DC_LOCATION.LOC_LN_POINTs.Where(x => x.pid_point == objPoint.pid_point).ToList();
                                        int cntName = 0;
                                        foreach (LOC_LN_POINT objcountName in objnames)
                                        {
                                            if (objcountName.pid_lang != 8 && objcountName.pid_lang != 6 && objcountName.pid_lang != 12)
                                            {
                                                if (objcountName.title != null && objcountName.title != "")
                                                {
                                                    cntName++;
                                                }

                                            }
                                        }
                                        if (cntPoint > 0)
                                        {
                                            //name Element Start
                                            xWriter.WriteStartElement("name");

                                            //texts Element Start
                                            xWriter.WriteStartElement("texts");

                                            foreach (LOC_LN_POINT objname in objnames)
                                            {
                                                if (objname.pid_lang != 8 && objname.pid_lang != 6 && objname.pid_lang != 12)
                                                {
                                                    string lang = "";
                                                    if (objname.pid_lang == 1)
                                                    {
                                                        lang = "it";
                                                    }
                                                    else if (objname.pid_lang == 2)
                                                    {
                                                        lang = "en";
                                                    }
                                                    else if (objname.pid_lang == 3)
                                                    {
                                                        lang = "es";
                                                    }
                                                    else if (objname.pid_lang == 4)
                                                    {
                                                        lang = "fr";
                                                    }
                                                    else if (objname.pid_lang == 5)
                                                    {
                                                        lang = "de";
                                                    }

                                                    else if (objname.pid_lang == 7)
                                                    {
                                                        lang = "nl";
                                                    }

                                                    else if (objname.pid_lang == 9)
                                                    {
                                                        lang = "pt";
                                                    }
                                                    else if (objname.pid_lang == 10)
                                                    {
                                                        lang = "fi";
                                                    }
                                                    else if (objname.pid_lang == 11)
                                                    {
                                                        lang = "sv";
                                                    }
                                                    if (objname.title != null && objname.title != "")
                                                    {
                                                        //text Element Start
                                                        xWriter.WriteStartElement("text");
                                                        xWriter.WriteAttributeString("locale", lang);
                                                        xWriter.WriteElementString("textValue", objname.title);
                                                        //End text
                                                        xWriter.WriteEndElement();
                                                    }
                                                }
                                            }
                                            //End texts
                                            xWriter.WriteEndElement();
                                            //End name
                                            xWriter.WriteEndElement();

                                        }
                                        //End nearestPlace
                                        xWriter.WriteEndElement();
                                        int cntOtherActivities = 0;
                                        foreach (RNT_LN_ESTATE objcountOtherActivities in _rList)
                                        {
                                            if (objcountOtherActivities.pid_lang != 8 && objcountOtherActivities.pid_lang != 6 && objcountOtherActivities.pid_lang != 12)
                                            {
                                                if (objcountOtherActivities.haOtherActivities != null && objcountOtherActivities.haOtherActivities != "")
                                                {
                                                    cntOtherActivities++;
                                                }

                                            }
                                        }
                                        if (cntOtherActivities > 0)
                                        {
                                            //Start otherActivities
                                            xWriter.WriteStartElement("otherActivities");
                                            //texts Element Start
                                            xWriter.WriteStartElement("texts");

                                            foreach (RNT_LN_ESTATE objOtherActivities in _rList)
                                            {
                                                if (objOtherActivities.pid_lang != 8 && objOtherActivities.pid_lang != 6 && objOtherActivities.pid_lang != 12)
                                                {
                                                    string lang = "";
                                                    if (objOtherActivities.pid_lang == 1)
                                                    {
                                                        lang = "it";
                                                    }
                                                    else if (objOtherActivities.pid_lang == 2)
                                                    {
                                                        lang = "en";
                                                    }
                                                    else if (objOtherActivities.pid_lang == 3)
                                                    {
                                                        lang = "es";
                                                    }
                                                    else if (objOtherActivities.pid_lang == 4)
                                                    {
                                                        lang = "fr";
                                                    }
                                                    else if (objOtherActivities.pid_lang == 5)
                                                    {
                                                        lang = "de";
                                                    }

                                                    else if (objOtherActivities.pid_lang == 7)
                                                    {
                                                        lang = "nl";
                                                    }

                                                    else if (objOtherActivities.pid_lang == 9)
                                                    {
                                                        lang = "pt";
                                                    }
                                                    else if (objOtherActivities.pid_lang == 10)
                                                    {
                                                        lang = "fi";
                                                    }
                                                    else if (objOtherActivities.pid_lang == 11)
                                                    {
                                                        lang = "sv";
                                                    }
                                                    if (objOtherActivities.haOtherActivities != null && objOtherActivities.haOtherActivities != "")
                                                    {
                                                        //text Element Start
                                                        xWriter.WriteStartElement("text");
                                                        xWriter.WriteAttributeString("locale", lang);
                                                        xWriter.WriteElementString("textValue", objOtherActivities.haOtherActivities.Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"));
                                                        //End text
                                                        xWriter.WriteEndElement();
                                                    }
                                                }
                                            }
                                            //End texts
                                            xWriter.WriteEndElement();
                                            //End otherActivities
                                            xWriter.WriteEndElement();
                                        }

                                    }
                                }
                                //End nearestPlaces
                                xWriter.WriteEndElement();
                            }

                        }


                        //End location
                        xWriter.WriteEndElement();

                        //images Element Start
                        xWriter.WriteStartElement("images");

                        List<RNT_RL_ESTATE_MEDIA> _imageList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == _estate.id && x.type == "homeaway").OrderBy(x => x.sequence).Take(50).ToList();
                        foreach (RNT_RL_ESTATE_MEDIA image in _imageList)
                        {
                            //image Element Start
                            xWriter.WriteStartElement("image");

                            xWriter.WriteElementString("externalId", Convert.ToString(image.id));//Need To be set 

                            xWriter.WriteElementString("uri", (App.HOST + "/" + image.img_banner).Replace(" ", "%20"));

                            //End image
                            xWriter.WriteEndElement();
                        }
                        //End images
                        xWriter.WriteEndElement();

                        //need to start from units

                        //units Element Start
                        xWriter.WriteStartElement("units");

                        //unit Element Start
                        xWriter.WriteStartElement("unit");

                        xWriter.WriteElementString("externalId", Convert.ToString(_estate.id)); //Need to be set

                        xWriter.WriteElementString("active", "true");

                        //Bathrooms Element Start
                        xWriter.WriteStartElement("bathrooms");
                        //Bathroom Element Start
                        xWriter.WriteStartElement("bathroom");

                        xWriter.WriteElementString("roomSubType", "FULL_BATH");

                        //End bathroom Element
                        xWriter.WriteEndElement();

                        //End bathrooms Element
                        xWriter.WriteEndElement();


                        if (_estate.num_rooms_bed.objToInt32() > 0)
                        {
                            //bedrooms Element Start
                            xWriter.WriteStartElement("bedrooms");
                            for (int bedroomCount = 1; bedroomCount <= _estate.num_rooms_bed.objToInt32(); bedroomCount++)
                            {
                                //bedroom Element Start
                                xWriter.WriteStartElement("bedroom");
                                xWriter.WriteElementString("roomSubType", "BEDROOM");
                                //End bedroom Element
                                xWriter.WriteEndElement();
                            }
                            //End bedrooms Element
                            xWriter.WriteEndElement();
                        }
                        else
                        {
                            //bedrooms Element Start
                            xWriter.WriteStartElement("bedrooms");
                            //bedroom Element Start
                            xWriter.WriteStartElement("bedroom");
                            xWriter.WriteElementString("roomSubType", "LIVING_SLEEPING_COMBO");
                            //End bedroom Element
                            xWriter.WriteEndElement();
                            //End bedrooms Element
                            xWriter.WriteEndElement();
                        }

                        List<RNT_RL_ESTATE_CONFIG> _currConfig = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == _estate.id && x.is_HomeAway == 1).ToList();
                        if (_currConfig != null && _currConfig.Count > 0)
                        {
                            //featurevalues Element start
                            xWriter.WriteStartElement("featureValues");
                            foreach (RNT_RL_ESTATE_CONFIG config in _currConfig)
                            {
                                //featureValue Element Start
                                xWriter.WriteStartElement("featureValue");
                                xWriter.WriteElementString("count", Convert.ToString(config.Count));
                                RNT_TB_HACONFIG currCongigname = DC_RENTAL.RNT_TB_HACONFIG.SingleOrDefault(x => x.id == config.pid_config);
                                xWriter.WriteElementString("unitFeatureName", currCongigname.inner_notes);
                                //featureValue Element end
                                xWriter.WriteEndElement();
                            }
                            xWriter.WriteEndElement();
                            //featurevalues Element End
                        }


                        xWriter.WriteElementString("maxSleep", Convert.ToString(_estate.num_persons_max));

                        xWriter.WriteElementString("propertyType", _estate.haPropertyType);

                        //unitMonetaryInformation Element Start
                        xWriter.WriteStartElement("unitMonetaryInformation");

                        xWriter.WriteElementString("currency", "EUR");



                        int cntRateNote = 0;
                        foreach (RNT_LN_ESTATE objDesc in _rList)
                        {
                            if (objDesc.pid_lang != 8 && objDesc.pid_lang != 6 && objDesc.pid_lang != 12)
                            {
                                if (objDesc.haRateNote != null && objDesc.haRateNote != "")
                                {
                                    cntRateNote++;
                                }



                            }
                        }

                        if (cntRateNote > 0)
                        {

                            //rateNotes Element Start
                            xWriter.WriteStartElement("rateNotes");
                            //texts Element Start
                            xWriter.WriteStartElement("texts");

                            foreach (RNT_LN_ESTATE objRateNote in _rList)
                            {
                                if (objRateNote.pid_lang != 8 && objRateNote.pid_lang != 6 && objRateNote.pid_lang != 12)
                                {
                                    string lang = "";
                                    if (objRateNote.pid_lang == 1)
                                    {
                                        lang = "it";
                                    }
                                    else if (objRateNote.pid_lang == 2)
                                    {
                                        lang = "en";
                                    }
                                    else if (objRateNote.pid_lang == 3)
                                    {
                                        lang = "es";
                                    }
                                    else if (objRateNote.pid_lang == 4)
                                    {
                                        lang = "fr";
                                    }
                                    else if (objRateNote.pid_lang == 5)
                                    {
                                        lang = "de";
                                    }

                                    else if (objRateNote.pid_lang == 7)
                                    {
                                        lang = "nl";
                                    }

                                    else if (objRateNote.pid_lang == 9)
                                    {
                                        lang = "pt";
                                    }
                                    else if (objRateNote.pid_lang == 10)
                                    {
                                        lang = "fi";
                                    }
                                    else if (objRateNote.pid_lang == 11)
                                    {
                                        lang = "sv";
                                    }
                                    if (objRateNote.haRateNote != null && objRateNote.haRateNote != "")
                                    {
                                        //text Element Start
                                        xWriter.WriteStartElement("text");
                                        xWriter.WriteAttributeString("locale", lang);
                                        xWriter.WriteElementString("textValue", objRateNote.haRateNote.Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"));
                                        //End text
                                        xWriter.WriteEndElement();
                                    }
                                }
                            }
                            //End texts
                            xWriter.WriteEndElement();
                            //rateNotes Element End
                            xWriter.WriteEndElement();
                        }

                        //unitMonetaryInformation Element End
                        xWriter.WriteEndElement();

                        var seasonGroup = _estate.pidSeasonGroup.objToInt32();
                        var seasonDateList = new List<dbRntSeasonDatesTBL>();
                        using (DCmodRental dc = new DCmodRental())
                            seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= DateTime.Now && x.pidSeasonGroup == seasonGroup).OrderBy(x => x.dtStart).ToList();

                        //ratePeriods Element Start
                        xWriter.WriteStartElement("ratePeriods");
                        foreach (var objDate in seasonDateList)
                        {
                            //ratePeriod Element Start
                            xWriter.WriteStartElement("ratePeriod");

                            //dateRange Element Start
                            xWriter.WriteStartElement("dateRange");

                            xWriter.WriteElementString("beginDate", Convert.ToDateTime(objDate.dtStart).ToString("yyyy-MM-dd"));
                            xWriter.WriteElementString("endDate", Convert.ToDateTime(objDate.dtEnd).ToString("yyyy-MM-dd"));

                            //End dateRange
                            xWriter.WriteEndElement();

                            xWriter.WriteStartElement("rates");

                            //Start rate Element for NIGHTLY_WEEKEND rateType.
                            xWriter.WriteStartElement("rate");

                            xWriter.WriteAttributeString("rateType", "NIGHTLY_WEEKDAY");

                            if (objDate.pidPeriod == 1)
                            {

                                xWriter.WriteStartElement("amount");
                                xWriter.WriteAttributeString("currency", "EUR");
                                xWriter.WriteValue(Convert.ToString(_estate.pr_1_2pax).Replace(",", "."));
                                xWriter.WriteEndElement();

                            }
                            else if (objDate.pidPeriod == 2)
                            {
                                xWriter.WriteStartElement("amount");
                                xWriter.WriteAttributeString("currency", "EUR");
                                xWriter.WriteValue(Convert.ToString(_estate.pr_2_2pax).Replace(",", "."));
                                xWriter.WriteEndElement();

                            }
                            else if (objDate.pidPeriod == 3)
                            {
                                xWriter.WriteStartElement("amount");
                                xWriter.WriteAttributeString("currency", "EUR");
                                xWriter.WriteValue(Convert.ToString(_estate.pr_3_2pax).Replace(",", "."));
                                xWriter.WriteEndElement();

                            }
                            else if (objDate.pidPeriod == 4)
                            {
                                xWriter.WriteStartElement("amount");
                                xWriter.WriteAttributeString("currency", "EUR");
                                xWriter.WriteValue(Convert.ToString(_estate.pr_4_2pax).Replace(",", "."));
                                xWriter.WriteEndElement();

                            }
                            //End rate
                            xWriter.WriteEndElement();

                            //Start rate Element for NIGHTLY_WEEKEND rateType.
                            xWriter.WriteStartElement("rate");

                            xWriter.WriteAttributeString("rateType", "NIGHTLY_WEEKEND");

                            if (objDate.pidPeriod == 1)
                            {

                                xWriter.WriteStartElement("amount");
                                xWriter.WriteAttributeString("currency", "EUR");
                                xWriter.WriteValue(Convert.ToString(_estate.pr_1_2pax).Replace(",", "."));
                                xWriter.WriteEndElement();

                            }
                            else if (objDate.pidPeriod == 2)
                            {
                                xWriter.WriteStartElement("amount");
                                xWriter.WriteAttributeString("currency", "EUR");
                                xWriter.WriteValue(Convert.ToString(_estate.pr_2_2pax).Replace(",", "."));
                                xWriter.WriteEndElement();

                            }
                            else if (objDate.pidPeriod == 3)
                            {
                                xWriter.WriteStartElement("amount");
                                xWriter.WriteAttributeString("currency", "EUR");
                                xWriter.WriteValue(Convert.ToString(_estate.pr_3_2pax).Replace(",", "."));
                                xWriter.WriteEndElement();

                            }
                            else if (objDate.pidPeriod == 4)
                            {
                                xWriter.WriteStartElement("amount");
                                xWriter.WriteAttributeString("currency", "EUR");
                                xWriter.WriteValue(Convert.ToString(_estate.pr_4_2pax).Replace(",", "."));
                                xWriter.WriteEndElement();

                            }
                            //End rate
                            xWriter.WriteEndElement();

                            //End rates
                            xWriter.WriteEndElement();

                            //End ratePeriod
                            xWriter.WriteEndElement();
                        }
                        //End ratePeriods
                        xWriter.WriteEndElement();

                        //RNT_TBL_RESERVATION
                        //reservations Element Start 
                        DateTime dtCurrent = DateTime.Now.Date;
                        DateTime dtEnd = DateTime.Now.Date.AddYears(2);
                        var resList = rntUtils.rntEstate_resList(_estate.id, dtCurrent, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
                        var list = new List<rntExts.AvvListPerDates>();
                        while (dtCurrent < dtEnd)
                        {
                            bool isAvv = resList.Where(x => x.dtStart.Value <= dtCurrent && x.dtEnd > dtCurrent).Count() == 0;
                            var lastDatePrice = list.LastOrDefault();
                            if (lastDatePrice == null || lastDatePrice.IsAvv != isAvv) list.Add(new rntExts.AvvListPerDates(dtCurrent, dtCurrent, isAvv));
                            else lastDatePrice.DtEnd = lastDatePrice.DtEnd.AddDays(1);
                            dtCurrent = dtCurrent.AddDays(1);
                        }
                        xWriter.WriteStartElement("reservations");
                        foreach (var tmp in list)
                        {
                            if (tmp.IsAvv) continue;
                            //reservation Element Start 
                            xWriter.WriteStartElement("reservation");
                            //reservationDates Element Start
                            xWriter.WriteStartElement("reservationDates");
                            xWriter.WriteElementString("beginDate", tmp.DtStart.formatCustom("#yy#-#mm#-#dd#", App.LangID, "--/--/----"));
                            xWriter.WriteElementString("endDate", tmp.DtEnd.AddDays(1).formatCustom("#yy#-#mm#-#dd#", App.LangID, "--/--/----"));
                            //End reservationDates
                            xWriter.WriteEndElement();
                            //End reservation
                            xWriter.WriteEndElement();
                        }
                        //End reservations
                        xWriter.WriteEndElement();

                        //End unit
                        xWriter.WriteEndElement();

                        //End units
                        xWriter.WriteEndElement();
                        //End listing
                        xWriter.WriteEndElement();
                    }

                }
                //End listings 
                xWriter.WriteEndElement();
                //End advertiser
                xWriter.WriteEndElement();
                //End advertisers
                xWriter.WriteEndElement();

                //End listingBatch
                xWriter.WriteEndElement();
                xWriter.WriteEndDocument();

                xWriter.Close();


                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                //  Response.AppendHeader("Content-Disposition", "attachment; filename = PropertyListing.xml");
                //Response.AppendHeader("Content-Length", wr..Length.ToString());
                Response.ContentType = "text/xml";
                Response.Write(wr.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", ""));
                Response.End();


            }
            catch (Exception ex)
            {

            }
        }
        public static bool IsNumber(String str)
        {
            try
            {
                Double.Parse(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        protected string cleareAddress(string fullAddress)
        {
            string tmp = "";
            for (int i = 0; i < fullAddress.Length; i++)
            {
                if (("" + fullAddress[i]).ToInt32() > 0) break;
                tmp += "" + fullAddress[i];
            }
            tmp = tmp.Trim();
            if (tmp.EndsWith(","))
                tmp = tmp.Substring(0, tmp.Length - 1);
            return tmp.Trim();
        }
    }
}