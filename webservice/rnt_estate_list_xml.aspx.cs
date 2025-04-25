using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class rnt_estate_list_xml : System.Web.UI.Page
    {
        private List<AppSettings.RNT_estate> _estateList;
        private string _mode;
        private DateTime _dtStart;
        private DateTime _dtEnd;
        private int _numPers;
        private int _currLang;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "text/xml";
            _currLang = Request.QueryString["lang"].objToInt32();
            if (_currLang == 0)
                _currLang = CurrentLang.ID;
            _mode = string.IsNullOrEmpty(Request.QueryString["mode"]) ? "all" : Request.QueryString["mode"];
            if (_mode=="all")
            {
                Response.Write(getEstateList_all());
                Response.End();
            }
            clConfig _config = clUtils.getConfig("");
            _dtStart = _config.lastSearch.dtStart;
            _dtEnd = _config.lastSearch.dtEnd;
            int _dtStartInt = Request.QueryString["dtS"].objToInt32();
            int _dtEndInt = Request.QueryString["dtE"].objToInt32();
            if (_dtStartInt != 0 && _dtEndInt != 0)
            {
                _dtStart = _dtStartInt.JSCal_intToDate();
                _dtEnd = _dtEndInt.JSCal_intToDate();
            }
            _numPers = Request.QueryString["numPers"].objToInt32();
            if (_numPers == 0)
                _numPers = _config.lastSearch.numPersCount;
            if (_mode == "available")
            {
                Response.Write(getEstateList_available());
                Response.End();
            }
        }
        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_RNT_estateList_xml"] == null)
                {
                    List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.ToList();

                    foreach (AppSettings.RNT_estate _rntEst in estateList)
                    {
                        RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == CurrentLang.ID && !string.IsNullOrEmpty(x.title));
                        if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                        if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 1);
                        if (_lang != null)
                        {
                            _rntEst.pid_lang = _currLang;
                            _rntEst.title = _lang.title;
                            _rntEst.summary = _lang.summary;
                            _rntEst.page_path = _lang.page_path;
                        }
                        else
                        {
                            _rntEst.pid_lang = 0;
                            _rntEst.title = _rntEst.code;
                            _rntEst.summary = "";
                            _rntEst.page_path = "";
                        }
                        _rntEst.price = rntUtils.rntEstate_minPrice(_rntEst.id);
                        _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), CurrentLang.ID, "");
                    }
                    HttpContext.Current.Session["CURRENT_RNT_estateList_xml"] = estateList;
                }
                return (List<AppSettings.RNT_estate>)HttpContext.Current.Session["CURRENT_RNT_estateList_xml"];
            }
            set { HttpContext.Current.Session["CURRENT_RNT_estateList_xml"] = value; }
        }
        protected string getEstateList_all()
        {
            _estateList = CURRENT_RNT_estateList;
            if (_estateList.Count > 0 && _estateList[0].pid_lang != CurrentLang.ID)
            {
                foreach (AppSettings.RNT_estate _rntEst in _estateList)
                {
                    RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == CurrentLang.ID && !string.IsNullOrEmpty(x.title));
                    if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                    if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 1);
                    if (_lang != null)
                    {
                        _rntEst.pid_lang = _currLang;
                        _rntEst.title = _lang.title + "";
                        _rntEst.summary = _lang.summary + "";
                        _rntEst.page_path = _lang.page_path + "";
                    }
                    else
                    {
                        _rntEst.pid_lang = 1;
                        _rntEst.title = "";
                        _rntEst.summary = "";
                        _rntEst.page_path = "";
                    }
                    _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), CurrentLang.ID, "");
                }
                CURRENT_RNT_estateList = _estateList;
            }
            //List<AppSettings.RNT_estate> _listWithPrice = CURRENT_RNT_estateList.Where(x => x.price != 0).OrderBy(x => x.sequence).ThenBy(x => x.price).ToList();
            //List<AppSettings.RNT_estate> _listOnRequest = CURRENT_RNT_estateList.Where(x => x.price == 0).OrderBy(x => x.sequence).ToList();
            //_estateList = new List<AppSettings.RNT_estate>();
            //_estateList.AddRange(_listWithPrice);
            //_estateList.AddRange(_listOnRequest);
            _estateList = _estateList.OrderBy(x => x.title).ToList();
            CURRENT_RNT_estateList = _estateList;
            XDocument _resource = new XDocument();
            XElement rootElement = new XElement("list");
            foreach (AppSettings.RNT_estate _rntEst in _estateList)
            {
                XElement record = new XElement("item");
                record.Add(new XElement("id", _rntEst.id));
                record.Add(new XElement("path", _rntEst.page_path));
                record.Add(new XElement("title", _rntEst.title));
                record.Add(new XElement("pid_zone", _rntEst.pid_zone));
                rootElement.Add(record);
            }
            List<RNT_VIEW_RESIDENCE> _residenceList = maga_DataContext.DC_RENTAL.RNT_VIEW_RESIDENCEs.Where(x => x.pid_lang == CurrentLang.ID).ToList();
            foreach (RNT_VIEW_RESIDENCE _residence in _residenceList)
            {
                XElement record = new XElement("item");
                record.Add(new XElement("id", "0"));
                record.Add(new XElement("path", ""));
                record.Add(new XElement("title", _residence.title));
                record.Add(new XElement("pid_zone", _residence.pid_zone));
                rootElement.Add(record);
            }

            _resource.Add(rootElement);
            //string _xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><set>";
            //_xml += "<set>";

            //_xml += "</set>";
            return _resource.ToString();
        }
        protected string getEstateList_available()
        {
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            {
                List<int> estatesNotAvv = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 //
                                                                                && x.dtStart.HasValue //
                                                                                && x.dtEnd.HasValue //
                                                                                && ((x.dtStart.Value.Date <= _dtStart && x.dtEnd.Value.Date >= _dtEnd) //
                                                                                    || (x.dtStart.Value.Date >= _dtStart && x.dtStart.Value.Date < _dtEnd) //
                                                                                    || (x.dtEnd.Value.Date > _dtStart && x.dtEnd.Value.Date <= _dtEnd))).Select(x => x.pid_estate.objToInt32()).ToList();
                _estateList = CURRENT_RNT_estateList.Where(x => x.num_persons_max >= _numPers && !estatesNotAvv.Contains(x.id)).ToList();
            }
            if (_estateList.Count > 0 && _estateList[0].pid_lang != _currLang)
            {
                foreach (AppSettings.RNT_estate _rntEst in _estateList)
                {
                    RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == _currLang);
                    if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                    if (_lang != null)
                    {
                        _rntEst.pid_lang = _currLang;
                        _rntEst.title = _lang.title;
                        _rntEst.summary = _lang.summary;
                        _rntEst.page_path = _lang.page_path;
                    }
                    _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), _currLang, "");
                }
                CURRENT_RNT_estateList = _estateList;
            }
            _estateList = _estateList.OrderBy(x => x.title).ToList();
            CURRENT_RNT_estateList = _estateList;
            XDocument _resource = new XDocument();
            XElement rootElement = new XElement("list");
            foreach (AppSettings.RNT_estate _rntEst in _estateList)
            {
                XElement record = new XElement("item");
                record.Add(new XElement("id", _rntEst.id));
                record.Add(new XElement("path", _rntEst.page_path));
                record.Add(new XElement("title", _rntEst.title));
                record.Add(new XElement("pid_zone", _rntEst.pid_zone));
                rootElement.Add(record);
            }
            _resource.Add(rootElement);
            return _resource.ToString();
        }
    }
}
