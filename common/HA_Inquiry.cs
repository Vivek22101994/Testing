using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using RentalInRome.data;
using System.Net;
using System.Xml.Linq;
using System.Threading;

namespace RentalInRome
{
    public class HA_Inquiry
    {
        private string Timeperiod { get; set; }
        private magaRental_DataContext DC_RENTAL = new magaRental_DataContext();
        private magaLocation_DataContext DC_LOCATION = new magaLocation_DataContext();
        private magaContent_DataContext DC_CONTENT = new magaContent_DataContext();
        private void doThread()
        {
            try
            {
                string xml = "";
                string url = "https://integration.homeaway.com/services/external/inquiries/3.1";
                var haAdvertiserIds = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.haAdvertiserId != null && x.haAdvertiserId != "" && x.haAdvertiserId != "Empty" && x.is_HomeAway == 1).Select(x => x.haAdvertiserId).Distinct().ToList();
                foreach (string haAdvertiserId in haAdvertiserIds)
                {
                    //for particular apartment.
                    xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><inquirySearch><assignedSystemId>PM</assignedSystemId><advertiserAssignedId>" + haAdvertiserId + "</advertiserAssignedId><timePeriod>" + Timeperiod + "</timePeriod><authorizationToken>64cd3c91-615f-4f21-b810-6a0d1fc16a59</authorizationToken></inquirySearch>";
                    //xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><inquirySearch><assignedSystemId>PM</assignedSystemId><timePeriod>" + drp_timePeriod.SelectedValue + "</timePeriod><authorizationToken>64cd3c91-615f-4f21-b810-6a0d1fc16a59</authorizationToken></inquirySearch>";
                    string response = POST(url, xml);
                    Add_Request(response, Timeperiod, haAdvertiserId, xml);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "HA_Inquiry " + Timeperiod, ex.ToString());
            }
        }
        public HA_Inquiry()
        {
            Timeperiod = "";
        }
        public string POST(String strURL, String strPostData)
        {
            try
            {
                HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(strURL);
                obj.Proxy = System.Net.WebRequest.DefaultWebProxy;
                obj.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                obj.ContentType = "application/xml";
                obj.Method = "POST";
                Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strPostData);
                obj.ContentLength = bytes.Length;
                Stream os = obj.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();
                HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
                Stream os1 = obj1.GetResponseStream();
                StreamReader _Answer = new StreamReader(os1);
                String strRSString = _Answer.ReadToEnd().ToString();
                return strRSString;

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public void Add_Request(string xml, string timeperiod, string haAdvertiserId, string requestXml)
        {

            Int64 InquieryCounter = 0;
            Int64 RepeatInquieryCounter = 0;
            Int64 SaveInquieryCounter = 0;
            string error_code = "";
            string error_message = "";
            if (xml == "") return;
            XDocument _resource = XDocument.Parse(xml);
            var ds = _resource.Descendants("inquiries").Elements("inquiry");

            InquieryCounter = ds.Count();
            if (ds != null)
            {
                foreach (var element in ds)
                {
                    string message = (string)element.Element("message") ?? "";
                    string fname = (string)element.Element("inquirer").Descendants("firstName").FirstOrDefault().Value;
                    string lastName = (string)element.Element("inquirer").Descendants("lastName").FirstOrDefault().Value;
                    string name_full = fname + " " + lastName;
                    string lang = (string)element.Element("inquirer").Attribute("locale");
                    string emailAddress = (string)element.Element("inquirer").Descendants("emailAddress").FirstOrDefault().Value;
                    string phoneCountryCode = (string)element.Element("inquirer").Descendants("phoneCountryCode").FirstOrDefault().Value;
                    int country = 0;
                    string country_title = "";
                    string phoneNumber = (string)element.Element("inquirer").Descendants("phoneNumber").FirstOrDefault().Value;
                    var date_ele = element.Element("reservation").Descendants("reservationDates").FirstOrDefault();
                    string st_date = (string)date_ele.Element("beginDate") ?? "";
                    string en_date = (string)date_ele.Element("endDate") ?? "";
                    string adult = (string)element.Element("reservation").Descendants("numberOfAdults").FirstOrDefault().Value;
                    string children = (string)element.Element("reservation").Descendants("numberOfChildren").FirstOrDefault();
                    string id = (string)element.Element("listingExternalId") ?? "";
                    string inquiryId = (string)element.Element("inquiryId") ?? "";
                    st_date = st_date.Replace("-", "");
                    en_date = en_date.Replace("-", "");
                    var node_listing_channel = element.Element("inquirer").Descendants("travelerSource").FirstOrDefault();
                    string listing_channel = node_listing_channel != null ? node_listing_channel.Value : "";
                    //string listing_channel = (string)element.Element("listingChannel") ?? "";
                    string _br = "";
                    bool alternateOld = true;

                    List<RNT_TBL_REQUEST> _currTBL = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.Ha_inquiryId == inquiryId).ToList();
                    if (_currTBL == null || _currTBL.Count == 0)
                    {
                        RNT_TBL_REQUEST _request = new RNT_TBL_REQUEST();
                        if (lang == "de")
                        {
                            _request.pid_lang = 5;
                        }
                        else if (lang == "en")
                        {
                            _request.pid_lang = 2;
                        }
                        else if (lang == "es")
                        {
                            _request.pid_lang = 3;
                        }
                        else if (lang == "fi")
                        {
                            _request.pid_lang = 10;
                        }
                        else if (lang == "fr")
                        {
                            _request.pid_lang = 4;
                        }
                        else if (lang == "it")
                        {
                            _request.pid_lang = 1;
                        }
                        else if (lang == "sv")
                        {
                            _request.pid_lang = 11;
                        }
                        string _mailBody = "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
                        string _row = "";
                        _row += "<tr>";
                        _row += "<td><strong>Lingua</strong></td>";

                        CONT_TBL_LANG objlang = DC_CONTENT.CONT_TBL_LANGs.SingleOrDefault(x => x.id == _request.pid_lang);
                        if (objlang != null)
                        {
                            _row += "<td>" + objlang.title + "</td>";
                        }
                        else
                        {
                            _row += "<td> </td>";
                        }
                        _row += "</tr>";
                        _mailBody += _row;
                        _request.name_first = "";
                        _request.name_last = "";
                        _request.name_full = name_full;
                        _mailBody += MailingUtilities.addMailRow("Nome/Cognome", _request.name_full, alternateOld, out alternateOld, false, false, false);
                        _request.phone = phoneNumber;
                        _mailBody += MailingUtilities.addMailRow("Telefono", "" + _request.phone, alternateOld, out alternateOld, false, false, false);
                        _request.email = emailAddress;
                        _mailBody += MailingUtilities.addMailRow("Email", "" + _request.email, alternateOld, out alternateOld, false, false, false);

                        if (phoneCountryCode != "")
                        {
                            List<LOC_LK_COUNTRY> currCountry = DC_LOCATION.LOC_LK_COUNTRies.Where(x => x.country_code == phoneCountryCode.objToInt32()).ToList();
                            if (currCountry != null && currCountry.Count == 1)
                            {
                                country = currCountry[0].id;
                                country_title = currCountry[0].title;
                            }
                            else
                            {
                                country = 0;
                                country_title = "";
                            }
                        }
                        else
                        {
                            country = 0;
                            country_title = "";
                        }

                        // start area
                        string _area = "";
                        _request.request_area = _area;
                        _mailBody += MailingUtilities.addMailRow("e/o zona", "" + _request.request_area, alternateOld, out alternateOld, false, false, false);
                        // end area
                        _request.request_country = country_title;
                        _mailBody += MailingUtilities.addMailRow("Paese (Location)", "" + _request.request_country, alternateOld, out alternateOld, false, false, false);
                        _request.request_date_start = st_date.JSCal_stringToDate();
                        _mailBody += MailingUtilities.addMailRow("Check-In", "" + _request.request_date_start.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
                        _request.request_date_end = en_date.JSCal_stringToDate();
                        _mailBody += MailingUtilities.addMailRow("Check-Out", "" + _request.request_date_end.Value.formatITA(true), alternateOld, out alternateOld, false, false, false);
                        _request.request_adult_num = adult.objToInt32();
                        _mailBody += MailingUtilities.addMailRow("num. Adulti", "" + _request.request_adult_num, alternateOld, out alternateOld, false, false, false);
                        _request.request_child_num = children.objToInt32();
                        _mailBody += MailingUtilities.addMailRow("num. Bambini", "" + _request.request_child_num, alternateOld, out alternateOld, false, false, false);
                        _request.request_transport = "";
                        _mailBody += MailingUtilities.addMailRow("Trasporto", "" + _request.request_transport, alternateOld, out alternateOld, false, false, false);
                        string _price_range = "";
                        _request.request_price_range = _price_range;
                        _mailBody += MailingUtilities.addMailRow("e/o con prezzo", "" + _request.request_price_range, alternateOld, out alternateOld, false, false, false);
                        string _services = "";
                        _br = "";
                        _request.request_services = _services;
                        _mailBody += MailingUtilities.addMailRow("Servizi", "" + _request.request_services, alternateOld, out alternateOld, false, false, false);
                        _request.request_notes = message;
                        _mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + _request.request_notes, alternateOld, out alternateOld, false, false, false);
                        _request.request_date_created = DateTime.Now;
                        _mailBody += MailingUtilities.addMailRow("Data ora creazione", "" + _request.request_date_created, alternateOld, out alternateOld, false, false, false);
                        _request.state_date = DateTime.Now;
                        _request.state_pid = 1;
                        _request.state_subject = "Creata Richiesta";
                        _request.state_pid_user = 1;
                        _request.request_ip = "";
                        _request.pid_creator = 1;
                        _request.pid_city = AppSettings.RNT_currCity;
                        _request.IdAdMedia = "Ha";
                        _request.IdLink = listing_channel;
                        _request.Ha_inquiryId = inquiryId;
                        //_request.request_choices = "1" + " - " + CurrentSource.rntEstate_title(id.objToInt32(), _request.pid_lang.objToInt32(), "");
                        DC_RENTAL.RNT_TBL_REQUEST.InsertOnSubmit(_request);
                        DC_RENTAL.SubmitChanges();
                        SaveInquieryCounter++;
                        rntUtils.rntRequest_addState(_request.id, _request.state_pid.Value, _request.state_pid_user.Value, _request.state_subject, "");

                        _mailBody += "<br/><br/><br/>Per magiorni informazioni entrate in <a href='http://62.149.238.119/admin/cont_request_details.aspx?id=" + _request.id + "' >area amministrativa del sito</a>";
                        _mailBody += "</table>";
                        _request.mail_body = _mailBody;
                        string _mSubject = "";
                        int pid_operator = 0;
                        _request.pid_related_request = 0;
                        RNT_TBL_REQUEST _relatedRequest = rntUtils.rntRequest_getRelatedRequest(_request);
                        if (_relatedRequest != null)
                        {
                            _request.pid_related_request = _relatedRequest.id;
                            pid_operator = _relatedRequest.pid_operator.Value;
                            _mSubject = "rif." + _request.id + " Correlata a rif." + _relatedRequest.id + " - " + CurrentSource.getSysLangValue("reqHeadTitle", _request.pid_lang.objToInt32()) + " " + _request.name_first + " " + _request.name_last + " - " + _request.request_country + " - " + _request.request_choice_1 + " - " + _request.request_choice_2 + " - Check-In date: " + _request.request_date_start.Value.formatITA(true);
                            rntUtils.rntRequest_addState(_request.id, 0, 1, "Correlazione alla richiesta Primaria rif. " + _relatedRequest.id, "");
                            rntUtils.rntRequest_addState(_relatedRequest.id, 0, 1, "Aggiunta Correlazione alla richiesta Secondaria rif. " + _request.id, "");
                        }
                        else
                        {
                            _request.pid_related_request = 0;
                            pid_operator = AdminUtilities.usr_getAvailableOperator(country, _request.pid_lang.objToInt32());
                            _mSubject = "rif." + _request.id + " - " + CurrentSource.getSysLangValue("reqHeadTitle", _request.pid_lang.objToInt32()) + " " + _request.name_first + " " + _request.name_last + " - " + _request.request_country + " - " + _request.request_choice_1 + " - " + _request.request_choice_2 + " - Check-In date: " + _request.request_date_start.Value.formatITA(true);
                        }
                        if (pid_operator == 0)
                        {
                            _mailBody = "Attenzione! Non è stato Assegnato a nessun account.<br/><br/>" + _mailBody;
                        }
                        else
                        {
                            _request.operator_date = DateTime.Now;
                            string _mailSend = "";
                            //if (MailingUtilities.autoSendMailTo(_mSubject, _mailBody, AdminUtilities.usr_adminEmail(pid_operator, ""), false, "stp_contacts al account"))
                            //    _mailSend = "Assegnato e inviato mail a " + AdminUtilities.usr_adminName(pid_operator, "") + " (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                            //else
                            //    _mailSend = "Assegnato " + AdminUtilities.usr_adminName(pid_operator, "") + " - Errore nel invio mail a (" + AdminUtilities.usr_adminEmail(pid_operator, "") + ")";
                            rntUtils.rntRequest_addState(_request.id, 0, 1, _mailSend, _mailBody);
                            _mailBody = _mailSend + "<br/><br/>" + _mailBody;
                        }

                        _request.pid_operator = pid_operator;

                        RNT_RL_REQUEST_ITEM _item = new RNT_RL_REQUEST_ITEM();
                        _item.pid_estate = id.objToInt32();
                        _item.pid_request = _request.id;
                        _item.sequence = 1;
                        DC_RENTAL.RNT_RL_REQUEST_ITEMs.InsertOnSubmit(_item);
                        DC_RENTAL.SubmitChanges();

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Show_inf_hotel_request_send", "setTimeout('parent.$(\".formreportapp\").colorbox.close();',3000);", true);
                        //MailingUtilities.autoSendMailTo(_mSubject, _mailBody, MailingUtilities.ADMIN_MAIL, false, "admin_rnt_request_new_from_mail al admin");
                    }
                    else
                    {
                        RepeatInquieryCounter++;
                    }
                }
            }
            string _folder = "log/ha_requests";
            if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
            string filename = DateTime.Now.JSCal_dateToInt() + ".txt";
            string filepath = Path.Combine(App.SRP, _folder) + "\\" + filename;
            string line = "\n";
            line += "" + DateTime.Now + " | haAdvertiserId: " + haAdvertiserId + " | timeperiod: " + timeperiod + "\n";
            line += "Total: " + InquieryCounter + " | Saved: " + SaveInquieryCounter + " | Repeating: " + RepeatInquieryCounter.ToString() + "\n";
            foreach (var element in _resource.Descendants("errors").Elements("error"))
            {
                error_code = (string)element.Element("errorCode") ?? "";
                error_message = (string)element.Element("message") ?? "";
                line += "ErrorCode: " + error_code + " | ErrorMessage: " + error_message + "\n";
            }
            if (InquieryCounter == 0 && _resource.Descendants("errors").Elements("error").Count() == 0)
            {
                line += requestXml + "\n";
                line += xml + "\n";
            }
            StreamWriter file = new System.IO.StreamWriter(filepath, true);
            file.WriteLine(line);
            file.Close();
        }
        public void CreateInquiryXML_PAST_THREE_DAYS()
        {
            GetRequests("PAST_THREE_DAYS");
        }
        public void CreateInquiryXML_PAST_TWENTY_FOUR_HOURS()
        {
            GetRequests("PAST_TWENTY_FOUR_HOURS");
        }
        public void CreateInquiryXML_PAST_THIRTY_MINUTES()
        {
            GetRequests("PAST_THIRTY_MINUTES");
        }
        public void GetRequests(string timeperiod)
        {
            Timeperiod = timeperiod;
            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "GetRequests:");

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            

        }
    }

}