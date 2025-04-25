using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class rnt_ad_request_items : System.Web.UI.Page
    {
        private int IdRequest;
        private RNT_TBL_REQUEST _currTBL;
        private int _addEstate;
        private int _delEstate;
        private string _mode;
        private string IMG_ROOT
        {
            get { return Request.Url.AbsoluteUri.Contains("http://localhost") ? "http://192.168.1.150/Rir/rentalinrome/" : "/"; }
        }
        private magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            //DC_RENTAL = maga_DataContext.DC_RENTAL;
            //if (!IsPostBack)
            //{
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ClearContent();
            //    Response.ClearHeaders();
            //    Response.ContentType = "text/html";
            //    IdRequest = Request.QueryString["IdRequest"].objToInt32();
            //    _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == IdRequest);
            //    if (_currTBL == null) return;
            //    List<RNT_RL_REQUEST_ITEM> _items = DC_RENTAL.RNT_RL_REQUEST_ITEMs.Where(x => x.pid_request == IdRequest).ToList();
            //    _addEstate = Request.QueryString["addEstate"].objToInt32();
            //    if (_addEstate != 0)
            //    {
            //        RNT_RL_REQUEST_ITEM _newItem = new RNT_RL_REQUEST_ITEM();
            //        _newItem.pid_estate = IdRequest;
            //    }
            //    //_clConfig.addTo_myPreferedEstateList(_addEstate);
            //    _delEstate = Request.QueryString["delEstate"].objToInt32();
            //    //if (_delEstate != 0)
            //    //    _clConfig.myPreferedEstateList.Remove(_delEstate);
            //    _mode = Request.QueryString["mode"] + "";
            //    if (_mode == "") _mode = "view";
            //    string _return = "";
            //    List<int> _toRemove = new List<int>();
            //    foreach (int id in _clConfig.myPreferedEstateList)
            //    {
            //        RNT_TB_ESTATE _tbl = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id);
            //        if (_tbl == null)
            //        {
            //            _toRemove.Add(id);
            //            continue;
            //        }
            //        RNT_LN_ESTATE _ln = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == CurrentLang.ID && !string.IsNullOrEmpty(x.title));
            //        if (_ln == null) _ln = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == 2 && !string.IsNullOrEmpty(x.title));
            //        if (_ln == null) _ln = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == 1 && !string.IsNullOrEmpty(x.title));
            //        if (_ln == null)
            //        {
            //            _toRemove.Add(id);
            //            continue;
            //        }
            //        List<decimal> _priceList = new List<decimal>();
            //        AppSettings.RNT_estatePrice _price = AdminUtilities.rntEstate_estatePrice(id);
            //        if (_price.price_1 != 0)
            //            _priceList.Add(_price.price_1);
            //        if (_price.price_2 != 0)
            //            _priceList.Add(_price.price_2);
            //        if (_price.price_3 != 0)
            //            _priceList.Add(_price.price_3);

            //        decimal price = _priceList.Count > 0 ? _priceList.Min() : 0;
            //        if (_mode == "view")
            //        {

            //            _return += "<li>";
            //            _return += "    <a href=\"/" + _ln.page_path + "\">";
            //            _return += "        <img src=\"" + ((("" + _tbl.img_preview_1).Trim() != "") ? IMG_ROOT + _tbl.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" alt=\"" + _ln.title + "\" />";
            //            _return += "        <strong>" + _ln.title + "</strong>";
            //            _return += "        " + CurrentSource.getSysLangValue("lblPriceFrom") + " " + price.ToString("N2") + "&euro; " + CurrentSource.getSysLangValue("lbl2paxPerDay") + "";
            //            _return += "        <span class=\"nulla\"></span>";
            //            _return += "    </a>";
            //            _return += "</li>";
            //        }
            //        else if (_mode == "req")
            //        {
            //            _return += "<li>";
            //            _return += "    <span class=\"selected_delete\" >";
            //            _return += "        <a href=\"javascript:req_deleteFromMyList(" + id + ")\">" + _ln.title + "</a>";
            //            _return += "    </span>";
            //            _return += "    <img src=\"" + ((("" + _tbl.img_preview_1).Trim() != "") ? IMG_ROOT + _tbl.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" alt=\"" + _ln.title + "\" />";
            //            _return += "    <a href=\"/" + _ln.page_path + "\" target=\"_blank\">";
            //            _return += "        <strong>" + _ln.title + "</strong>";
            //            _return += "    </a>";
            //            _return += "    " + CurrentSource.getSysLangValue("lblPriceFrom") + " " + price.ToString("N2") + "&euro; " + CurrentSource.getSysLangValue("lbl2paxPerDay") + "";
            //            _return += "    <span class=\"nulla\"></span>";
            //            _return += "</li>";
            //        }
            //    }
            //    _clConfig.myPreferedEstateList = _clConfig.myPreferedEstateList.Where(x => !_toRemove.Contains(x)).ToList();
            //    AppSettings.CURRENT_clientConfig = _clConfig;
            //    //if (_return != "") _return = "<ul>" + _return + "</ul>";
            //    Response.Write(_return);
            //}
        }
    }
}
