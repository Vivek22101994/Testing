using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class rnt_myPreferedEstateList : System.Web.UI.Page
    {
        private int _currLang;
        private int _currEstate;
        private int _deleteEstate;
        private string _mode;
        private string CURRENT_SESSION_ID;
        private string IMG_ROOT
        {
            get { return Request.Url.AbsoluteUri.Contains("http://localhost") ? "http://192.168.1.150/Rir/rentalinrome/" : "/"; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/html";
                _currLang = Request.QueryString["lang"].objToInt32();
                if (_currLang == 0)
                    _currLang = CurrentLang.ID;
                CURRENT_SESSION_ID = Request.QueryString["SESSION_ID"];
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                _currEstate = Request.QueryString["currEstate"].objToInt32();
                if (_currEstate != 0)
                    _config.addTo_myPreferedEstateList(_currEstate);
                _deleteEstate = Request.QueryString["deleteEstate"].objToInt32();
                if (_deleteEstate != 0)
                    _config.myPreferedEstateList.Remove(_deleteEstate);
                _mode = Request.QueryString["mode"] + "";
                if (_mode == "") _mode = "view";
                string _return = "";
                List<int> _toRemove = new List<int>();
                string _ItemTemplate = ltr_itemTemplate.Text;
                foreach (int id in _config.myPreferedEstateList)
                {
                    RNT_TB_ESTATE _tbl = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == id);
                    if (_tbl == null)
                    {
                        _toRemove.Add(id);
                        continue;
                    }
                    RNT_LN_ESTATE _ln = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == _currLang && !string.IsNullOrEmpty(x.title));
                    if (_ln == null) _ln = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == 2 && !string.IsNullOrEmpty(x.title));
                    if (_ln == null) _ln = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == id && x.pid_lang == 1 && !string.IsNullOrEmpty(x.title));
                    if (_ln == null)
                    {
                        _toRemove.Add(id);
                        continue;
                    }
                    decimal price = rntUtils.rntEstate_minPrice(id);
                    if (_mode == "view")
                    {

                        _return += "<li>";
                        _return += "    <a href=\"/" + _ln.page_path + "\">";
                        _return += "        <img src=\"" + ((("" + _tbl.img_preview_1).Trim() != "") ? IMG_ROOT + _tbl.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" alt=\"" + _ln.title + "\" />";
                        _return += "        <strong>" + _ln.title + "</strong>";
                        _return += "        " + CurrentSource.getSysLangValue("lblPriceFrom") + " " + price.ToString("N2") + "&euro; " + CurrentSource.getSysLangValue("lbl2paxPerDay") + "";
                        _return += "        <span class=\"nulla\"></span>";
                        _return += "    </a>";
                        _return += "</li>";
                    }
                    else if (_mode == "req")
                    {
                        _return += "<li>";
                        _return += "    <span class=\"selected_delete\" >";
                        _return += "        <a href=\"javascript:req_deleteFromMyList(" + id + ")\">" + _ln.title + "</a>";
                        _return += "    </span>";
                        _return += "    <img src=\"" + ((("" + _tbl.img_preview_1).Trim() != "") ? IMG_ROOT + _tbl.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" alt=\"" + _ln.title + "\" />";
                        _return += "    <a href=\"/" + _ln.page_path + "\" target=\"_blank\">";
                        _return += "        <strong>" + _ln.title + "</strong>";
                        _return += "    </a>";
                        if (price != 0)
                            _return += "    " + CurrentSource.getSysLangValue("lblPriceFrom") + " " + price.ToString("N2") + "&euro; " + CurrentSource.getSysLangValue("lbl2paxPerDay") + "";
                        else
                            _return += "    " + CurrentSource.getSysLangValue("lblOnRequest") + "";
                        _return += "    <span class=\"nulla\"></span>";
                        _return += "</li>";
                    }
                    else if (_mode == "req_new")
                    {
                        var mediaList = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.type == "gallery" && x.pid_estate == _tbl.id).Select(x => x.img_thumb).ToList();

                        string _ItemTemplateSingle = _ItemTemplate.Clone() as string;
                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#id#", "" + _tbl.id);
                        //_ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_1#", "" + (!string.IsNullOrEmpty(_tbl.img_preview_1) ? _rntEst.img_preview_1 : "images/css/default-apt-list.jpg"));
                        //_ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_2#", "" + (!string.IsNullOrEmpty(_tbl.img_preview_2) ? _rntEst.img_preview_2 : "images/css/default-apt-list.jpg"));
                        //_ItemTemplateSingle = _ItemTemplateSingle.Replace("#img_preview_3#", "" + (!string.IsNullOrEmpty(_tbl.img_preview_3) ? _rntEst.img_preview_3 : "images/css/default-apt-list.jpg"));
                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#title#", "" + _ln.title);
                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#page_path#", "" + _ln.page_path);

                        var _Zone = AppSettings.LOC_ZONEs.SingleOrDefault(x => x.id == _tbl.pid_zone && x.pid_lang == App.LangID);

                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#zoneTitle#", _Zone == null ? "" : _Zone.title);

                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#summary#", "" + _ln.summary);
                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#lblRemove#", contUtils.getLabel("lblRemove"));
                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#rating#", _tbl.importance_vote.objToInt32() + "");

                        string images = "";
                        foreach (string imgpath in mediaList)
                        {
                            images += ltr_ImageTemplate.Text.Replace("#src#", imgpath);
                        }
                        _ItemTemplateSingle = _ItemTemplateSingle.Replace("#imageList#", images);

                        _return += _ItemTemplateSingle;
                    }
                }
                _config.myPreferedEstateList = _config.myPreferedEstateList.Where(x => !_toRemove.Contains(x)).ToList();
                clUtils.saveConfig(_config);
                //if (_return != "") _return = "<ul>" + _return + "</ul>";
                Response.Write(_return);
                Response.End();
            }
        }
    }
}
