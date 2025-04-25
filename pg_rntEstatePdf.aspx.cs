using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;
using System.IO;

namespace FourSprings
{
    public partial class pg_rntEstatePdf : mainBasePage
    {
        magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
        magaLocation_DataContext DC_LOC;
        protected dbRntAgentTBL currAgent;

        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }

        public int IdEstate
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
               // ucBooking.IdEstate = value;
            }
        }

        public int IdAgent
        {
            get
            {
                return Hf_agentid.Value.ToInt32();
            }
            set
            {
                Hf_agentid.Value = value.ToString();
            }
        }

        public decimal markup_per
        {
            get
            {
                return Hf_markup_per.Value.ToInt32();
            }
            set
            {
                Hf_markup_per.Value = value.ToString();
            }
        }

        private RNT_TB_ESTATE TMPcurrEstateTB;
        public RNT_TB_ESTATE currEstateTB
        {
            get
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                if (TMPcurrEstateTB == null)
                    TMPcurrEstateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                return TMPcurrEstateTB ?? new RNT_TB_ESTATE();
            }
            set { TMPcurrEstateTB = value; }
        }

        public RNT_LN_ESTATE currEstateLN
        {
            get
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                if (TMPlnEstate == null)
                    TMPlnEstate = DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == CurrentLang.ID);
                return TMPlnEstate ?? new RNT_LN_ESTATE();
            }
            set { TMPlnEstate = value; }
        }
        private RNT_LN_ESTATE TMPlnEstate;

        public int Num_guestBook
        {
            get
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                var listaCommenti = AppSettings.RNT_TBL_ESTATE_COMMENTs.Where(x => x.pidEstate == IdEstate && x.cl_pid_lang == CurrentLang.ID).ToList();
                return listaCommenti.Count;
            }
        }

        protected string IMG_ROOT
        {
            get { return Request.Url.AbsoluteUri.Contains("http://localhost") ? App.HOST + "/" : "/"; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "rntEstateDett";
            int id = Request.QueryString["id"].ToInt32();
            if (id != 0)
                base.PAGE_REF_ID = id;
            else
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "rntEstateDett", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //if (Request.QueryString["r"] == "s") ucBooking.checkAvailability();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdAgent = Request.QueryString["agentid"].ToInt32();
                if (IdAgent == 0)
                {
                    Response.Redirect("/");
                }

                using (DCmodRental dc = new DCmodRental())
                {
                    currAgent = dc.dbRntAgentTBLs.FirstOrDefault(x => x.id == IdAgent && x.isActive == 1);                    
                }
                if (currAgent == null)
                    Response.Redirect("/");

                IdEstate = Request.QueryString["id"].ToInt32();
                if (!string.IsNullOrEmpty(Request.QueryString["gallery"]))
                {
                    HF_galleryType.Value = Request.QueryString["gallery"];
                }

                markup_per = string.IsNullOrEmpty(Request.QueryString["mk"]) ? 0 : Request.QueryString["mk"].objToDecimal();

                pnlAvailability.Visible = Request.QueryString["pdf"] != "false";
                pnlImages.Visible = Request.QueryString["images"] != "false";

                FillData();
            }
        }

        protected void FillData()
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_LOC = new magaLocation_DataContext();
            currEstateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
            if (currEstateTB == null)
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "Apt non attivo id=" + PAGE_REF_ID, _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=g1");
                return;
            }
            ucEstatePrices.IdEstate = IdEstate;
            ucEstatePrices.markup = markup_per;
            ucEstatePrices.fillData();
            using (DCmodRental dc = new DCmodRental())
            {
                string tmpStr = "";
                var currIdsList = dc.dbRntEstateExtrasRLs.Where(x => x.pidEstate == IdEstate).Select(x => x.pidEstateExtras).ToList();
                foreach (var id in currIdsList)
                {
                    var tmp = dc.dbRntEstateExtrasVIEWs.SingleOrDefault(x => x.id == id && x.pidLang == App.LangID && (x.isImportant > 0));
                    if (tmp == null) tmp = dc.dbRntEstateExtrasVIEWs.SingleOrDefault(x => x.id == id && x.pidLang == 2 && (x.isImportant>0));
                    if (tmp == null) tmp = dc.dbRntEstateExtrasVIEWs.SingleOrDefault(x => x.id == id && x.pidLang == 1 && (x.isImportant >0));
                    if (tmp == null) continue;
                    tmpStr += "<li><span>" + tmp.title + "</span></li>";
                }
                ltrAmenities.Text = tmpStr;
            }

            var galleryList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == currEstateTB.id && x.type == HF_galleryType.Value).OrderBy(x => x.sequence).ThenBy(x => x.id).ToList();
            var galleryListBig = new List<RNT_RL_ESTATE_MEDIA>();
            var galleryListSmall = new List<RNT_RL_ESTATE_MEDIA>();
            foreach (var tmp in galleryList)
            {
                if (File.Exists(Path.Combine(App.SRP, tmp.img_banner)))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(Path.Combine(App.SRP, tmp.img_banner));
                    tmp.id = img.Height;
                    if (img.Width >= rntProps.ImgSmallWidth && galleryListBig.Count < 14) galleryListBig.Add(tmp);
                    else if (galleryListSmall.Count < 12) galleryListSmall.Add(tmp);
                }
            }
            foreach (var tmp in galleryListBig)
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(Path.Combine(App.SRP, tmp.img_banner));
                if(img.Width<=1000) continue;
                var h = decimal.Multiply(decimal.Divide(img.Height, img.Width), 1000);
                tmp.img_banner += "?resize=true&w=1000&h=" + h.objToInt32() + "&ow=" + img.Width + "&oh=" + img.Height; 
            }
            LvGalleryBig.DataSource = galleryListBig.OrderBy(x => x.sequence).ThenBy(x => x.id).ToList();
            LvGalleryBig.DataBind();
            if (galleryListBig.Count < 14)
            {
                LvGallerySmall.DataSource = galleryListSmall.OrderBy(x => x.sequence).ThenBy(x => x.id).ToList();
                LvGallerySmall.DataBind();
            }
            Hf_banner_img.Value = (galleryList != null && galleryList.Count() >0) ? galleryList.FirstOrDefault().img_banner : "";
            checkCalDates();
        }
        protected void checkCalDates()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                DateTime _dtStart = DateTime.Now;
                DateTime _dtEnd = _dtStart.AddDays(7);
                bool _isSet = false;
                bool _hasHole = false;
                DateTime _dtLast = _dtStart;
                var currList = dc.dbRntSeasonDatesTBLs.Where(x => x.pidSeasonGroup == currEstateTB.pidSeasonGroup).OrderBy(x => x.dtStart).ToList();
                string _script = "";
                _script += "function checkCalDates_" + Unique + "(date){var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;var hasSeason=false;";
                _script += rntUtils.rntEstate_availableDatesForJSCal(IdEstate, 0);
                foreach (var tmp in currList)
                {
                    if (_isSet)
                    {
                        if (_dtEnd.AddDays(1) != tmp.dtStart)
                        {
                            _dtStart = _dtEnd.AddDays(1);
                            _dtEnd = tmp.dtStart.AddDays(-1);
                            _hasHole = true;
                        }
                    }
                    if (!_hasHole)
                    {
                        _dtStart = tmp.dtStart;
                        _dtEnd = tmp.dtEnd;
                    }
                    _isSet = true;
                    _dtLast = tmp.dtEnd; string _intDateFrom = "" + tmp.dtStart.JSCal_dateToInt();
                    string _intDateTo = "" + tmp.dtEnd.JSCal_dateToInt();
                    _script += "if(dateint <= " + _intDateTo + " && dateint >= " + _intDateFrom + ") { hasSeason=true;  }";
                }
                _script += "if(_controls=='' && !hasSeason) { _controls += '<span class=\"rntCal nd_f\"></span>'; _enabled = false; }";
                _script += "if (_controls.indexOf('<span class=\"rntCal nd_2\"></span>') != -1 && _controls.indexOf('<span class=\"rntCal nd_1\"></span>') != -1) { _enabled = false; }";
                _script += "return [_enabled, _class, _tooltip, _controls];";
                _script += "}";
                ltrScript_checkCalDates.Text = "<script type='text/javascript'>" + _script + "</script>";
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "checkCalDates_" + Unique, _script, true);
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