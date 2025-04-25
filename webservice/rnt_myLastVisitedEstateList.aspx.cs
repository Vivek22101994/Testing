using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class rnt_myLastVisitedEstateList : System.Web.UI.Page
    {
        private int _currEstate;
        private int _currLang;
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
                string _return = "";
                List<int> _toRemove = new List<int>();
                foreach (int id in _config.myLastVisitedEstateList)
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
                    _return += "<li>";
                    _return += "    <a href=\"/" + _ln.page_path + "\">";
                    _return += "        <img src=\"" + ((("" + _tbl.img_preview_1).Trim() != "") ? IMG_ROOT + _tbl.img_preview_1 : "/images/css/default-apt-list.jpg") + "\" alt=\"" + _ln.title + "\" />";
                    _return += "        <strong>" + _ln.title + "</strong>";
                    if (price != 0)
                        _return += "    " + CurrentSource.getSysLangValue("lblPriceFrom") + " " + price.ToString("N2") + "&euro; " + CurrentSource.getSysLangValue("lbl2paxPerDay") + "";
                    else
                        _return += "    " + CurrentSource.getSysLangValue("lblOnRequest") + "";
                    _return += "        <span class=\"nulla\"></span>";
                    _return += "    </a>";
                    _return += "</li>";
                }
                _config.myPreferedEstateList = _config.myPreferedEstateList.Where(x => !_toRemove.Contains(x)).ToList();
                clUtils.saveConfig(_config);
                Response.Write(_return);
            }
        }
    }
}
