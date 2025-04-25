using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.Xml.Linq;
using RentalInRome.data;


namespace RentalInRome
{
    public class Global : System.Web.HttpApplication
    {
        public System.Timers.Timer timer_1Minute;
        public System.Timers.Timer timer_10Minute;
        public System.Timers.Timer timer_12Hours;
        public string HostIpAdd = "";
        public string timeperiod = "";

        public ModRental.dbRntAgentTBL WLRental = null;

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {

                if (System.IO.File.Exists(System.IO.Path.Combine(App.SRP, "Web.connectionStrings.config")))
                {
                    System.Xml.Linq.XDocument _resource = System.Xml.Linq.XDocument.Load(System.IO.Path.Combine(App.SRP, "Web.connectionStrings.config"));
                    var ds = from System.Xml.Linq.XElement elm in _resource.Descendants("item")
                             select elm;
                    foreach (System.Xml.Linq.XElement elm in ds)
                    {
                        if (elm.Element("name") == null || elm.Element("value") == null) continue;
                        var settings = System.Configuration.ConfigurationManager.ConnectionStrings["" + elm.Element("name").Value];
                        var fi = typeof(System.Configuration.ConfigurationElement).GetField("_bReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                        fi.SetValue(settings, false);
                        settings.ConnectionString = "" + elm.Element("value").Value;
                    }
                }
            }
            catch (Exception exc)
            {
                ErrorLog.addLog("", "Application_Start", exc.ToString());
            }
            try
            {
                //for getting the homeaway inquiries.

                #region homeaway inquiries.
                ////for first 1 minute
                //timer_1Minute = new System.Timers.Timer();
                //timer_1Minute.AutoReset = false;
                //timer_1Minute.Interval = 60000;
                //timer_1Minute.Elapsed += new System.Timers.ElapsedEventHandler(timer_1Minute_Elapsed);
                //timer_1Minute.Start();

                ////for every 12 hours
                //timer_12Hours = new System.Timers.Timer();
                ////timer_12Hours.AutoReset = false;
                //timer_12Hours.Interval = 43200000;
                //timer_12Hours.Elapsed += new System.Timers.ElapsedEventHandler(timer_12Hours_Elapsed);
                //timer_12Hours.Start();

                ////for every 10 minutes
                //timer_10Minute = new System.Timers.Timer();
                ////timer_10Minute.AutoReset = false;
                //timer_10Minute.Interval = 600000;
                //timer_10Minute.Elapsed += new System.Timers.ElapsedEventHandler(timer_10Minute_Elapsed);
                //timer_10Minute.Start(); 
                #endregion

                rntSyncUtils.SetTimers();
                BcomUtils.SetTimers();
                ChnlHolidayUtils.SetTimers();
                ChnlHomeAwayUtils_V411.SetTimers();
                ChnlExpediaUtils.SetTimers();
                AppSyncClasses.StartAll();
            }
            catch (Exception exc)
            {
                ErrorLog.addLog("", "Application_Start", exc.ToString());
            }
        }
        public static bool isOnDev = true;
        void timer_12Hours_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // background OK
            //timer_12Hours.Stop();

            HA_Inquiry objInquiry = new HA_Inquiry();
            objInquiry.CreateInquiryXML_PAST_TWENTY_FOUR_HOURS();
            //timer_12Hours.Start();

        }

        void timer_10Minute_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // background OK
            //timer_10Minute.Stop();
            HA_Inquiry objInquiry = new HA_Inquiry();
            objInquiry.CreateInquiryXML_PAST_THIRTY_MINUTES();
            //timer_10Minute.Start();
        }

        void timer_1Minute_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // background OK
            timer_1Minute.Stop();
            HA_Inquiry objInquiry = new HA_Inquiry();
            objInquiry.CreateInquiryXML_PAST_TWENTY_FOUR_HOURS();


        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (WLRental != null)
            {
                App.WLAgentId = WLRental.id;
                App.WLAgent = WLRental;
            }
        }
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {

            HttpResponse response = this.Context.Response;
            HttpRequest request = this.Context.Request;
            RedirectList _redirectList = UrlRedirectTool.CURRENT_TOOL;
            RedirectItem _item = _redirectList.Items.FirstOrDefault(x => x.From == request.RawUrl.ToLower());
            if (_item != null)
            {
                Response.StatusCode = 301;
                Response.AddHeader("Location", _item.To);
                Response.End();
                return;
            }
            if (request.RawUrl.EndsWith("."))
            {
                string redirectTo = request.RawUrl;
                while (redirectTo.EndsWith("."))
                    redirectTo = redirectTo.Substring(0, redirectTo.Length - 1);
                Response.StatusCode = 301;
                Response.AddHeader("Location", redirectTo);
                Response.End();
                return;
            }
        }
        string getContentType(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return "Image/bmp";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }
        protected void getCachedImage(string sourcePath, int newWidth, int newHeight, string mimeType)
        {
            using (var image = Image.FromFile(sourcePath))
            {
                var oBitmap = new Bitmap(newWidth, newHeight);

                var thumbGraph = Graphics.FromImage(oBitmap);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                MemoryStream mem = new MemoryStream();
                oBitmap.Save(mem, ImageFormat.Jpeg);

                byte[] buffer = mem.ToArray();
                Response.Clear();
                Response.ContentType = mimeType;
                Response.BinaryWrite(buffer);
                Response.Flush();
                Response.End();
            }
        }
        protected void getResizedImage(string sourcePath, int newWidth, int newHeight, string mimeType)
        {
            using (var image = Image.FromFile(sourcePath))
            {
                var oBitmap = new Bitmap(newWidth, newHeight);

                var thumbGraph = Graphics.FromImage(oBitmap);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                MemoryStream mem = new MemoryStream();
                oBitmap.Save(mem, ImageFormat.Jpeg);

                byte[] buffer = mem.ToArray();
                Response.Clear();
                Response.ContentType = mimeType;
                Response.BinaryWrite(buffer);
                Response.Flush();
                Response.End();
            }
        }
        List<string> _extList = new List<string>() { "css", "js", "jpg", "gif", "png" };
        List<string> _httpsAllowedList = new List<string>() { "/util_", "/chnlutils/", "/agentapi", "/admin", "/affiliatesarea", "/reservationarea", "/webservice", "/images", "/index_file", "/css", "/pdfgenerator", "/jquery", "/js", "/Telerik.Web.UI.WebResource.axd", "/WebResource.axd", "/ScriptResource.axd" };
        //List<string> _requestPathInvalidCharacters = new List<string>() { "<", ">", "*", "%", "&", ":", "" };
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            isOnDev = Request.Url.AbsoluteUri.ToLower().Contains("http://localhost") || Request.Url.AbsoluteUri.ToLower().Contains(".magadesign.net") || Request.Url.AbsoluteUri.ToLower().Contains(".dev.magarental.com");
            if (CommonUtilities.getSYS_SETTING("is_HA_new_version").objToInt32() == 1)
            {
                if (ChnlHomeAwayUtils.CheckForService() || ChnlHomeAwayUtils_V411.CheckForService()) return;
            }
            else
            {
                if (ChnlHomeAwayUtilsOld.CheckForService()) return;
            }

            if (ChnlAtraveoUtils.CheckForService()) return;
            if (ChnlRentalsUnitedUtils.CheckForService()) return;
            if (ChnlMagaRentalXmlUtils.CheckForService()) return;
            if (ChnlAirbnbUtils.CheckForService()) return;
            if (ChnlAcubeApiUtils.CheckForService()) return;

            bool isMobile = false;

            string _ip = Request.browserIP();
            if (_ip == "")
                _ip = "...";

            var blockedIp = ModAppServerCommon.BlockedIpTool.CurrList.FirstOrDefault(x => x.ip == _ip);
            if (blockedIp != null)
            {
                Response.StatusCode = 401;
                Response.End();
                return;
            }

            DateTime _lastModified = DateTime.Now.AddHours(-1);
            string _origionalPath = Request.Url.LocalPath;
            if (_origionalPath.EndsWith("/"))
                _origionalPath = _origionalPath.Substring(0, _origionalPath.Length - 1);
            string _queryString = Request.Url.Query;


            // iCal for ChannelManager
            if (_origionalPath.ToLower().StartsWith("/channelmanager/"))
            {
                var tmpPathList = _origionalPath.ToLower().Replace("/channelmanager/", "").splitStringToList("/");
                if (tmpPathList.Count > 3 && tmpPathList[1] == "ical")
                {
                    using (ModRental.DCmodRental dc = new ModRental.DCmodRental())
                    {
                        var ChannelManager = dc.dbRntChannelManagerTBLs.SingleOrDefault(x => x.code.ToLower() == tmpPathList[0] && x.isActive == 1);
                        if (ChannelManager != null && tmpPathList[3].splitStringToList(".").Count > 1)
                        {
                            Context.RewritePath("/xmlapi/ical/get.aspx?id=" + tmpPathList[3].splitStringToList(".")[0] + "&channel=" + ChannelManager.code, false);
                        }
                    }
                }
            }


            RedirectList _redirectList = UrlRedirectTool.CURRENT_TOOL;
            RedirectItem _item = _redirectList.Items.FirstOrDefault(x => x.From == _origionalPath.ToLower());
            if (_item != null)
            {
                Response.StatusCode = 301;
                Response.AddHeader("Location", _item.To);
                Response.End();
                return;
            }

            #region WLRental
            WLRental = rntProps.AgentTBL.FirstOrDefault(x => x.isActive == 1 && !string.IsNullOrEmpty(x.WL_name) && x.WL_domainName == Request.Url.Host);
            if (WLRental != null)
            {
                long agentID = WLRental.id;
                // non far vedere area amministrativa
                if (Request.Url.Host != "localhost" && _origionalPath.ToLower().StartsWith("/admin"))
                {
                    Response.StatusCode = 301;
                    Response.AddHeader("Location", "http://" + Request.Url.Host);
                    Response.End();
                    return;
                }
                // il file sitemap.xml deve caricarsi in base al dominio
                if (_origionalPath.ToLower() == "/sitemap.xml")
                {
                    Context.RewritePath("/sitemap_" + agentID + ".xml", false);
                    return;
                }
                // il file robots.txt deve caricarsi in base al dominio
                if (_origionalPath.ToLower() == "/robots.txt")
                {
                    Context.RewritePath("/robots_" + agentID + ".txt", false);
                    return;
                }
                // immagini del voto sono diverse
                if (_origionalPath.ToLower().StartsWith("/images/estate_vote/"))
                {
                    Context.RewritePath(_origionalPath.ToLower().Replace("/images/estate_vote/", "/WLRental/images/estate_vote/") + _queryString, false);
                    return;
                }
                // immagini accessori sono diverse
                if (_origionalPath.ToLower().StartsWith("/images/estate_config/"))
                {
                    Context.RewritePath(_origionalPath.ToLower().Replace("/images/estate_config/", "/WLRental/images/estate_config/") + _queryString, false);
                    return;
                }
            }

            #endregion

            // caching del browser START
            int _extIndex = _origionalPath.LastIndexOf(".");
            string _extension = "aspx";
            if (_extIndex != -1)
                _extension = _origionalPath.Substring(_extIndex + 1);
            if (_extList.Contains(_extension))
            {
                if (Request.QueryString["resize"] == "true" && _extension == "jpg" && Request.QueryString["w"].ToInt32() > 0 && Request.QueryString["h"].ToInt32() > 0 && File.Exists(Path.Combine(App.SRP, _origionalPath.Substring(1))))
                {
                    var sourcePath = Path.Combine(App.SRP, _origionalPath.Substring(1));
                    // can given width of image as we want
                    var newWidth = Request.QueryString["w"].ToInt32();
                    // can given height of image as we want
                    var newHeight = Request.QueryString["h"].ToInt32();
                    getResizedImage(sourcePath, newWidth, newHeight, "image/jpeg");
                }
                if (_origionalPath.StartsWith("/cache/") && Context.checkCachedImages(Request, App.SRP))
                {
                    HttpContext.Current.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
                    return;
                }
                if (Request.QueryString["maxsize"] == "true" && _extension == "jpg" && File.Exists(Path.Combine(App.SRP, _origionalPath.Substring(1))))
                {
                    var sourcePath = Path.Combine(App.SRP, _origionalPath.Substring(1));
                    getMaxSizeImage(sourcePath, "image/jpeg", Request.QueryString["maxsizepx"].ToInt32());
                }
                HttpContext.Current.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
                return;
            }
            // caching del browser END
            if (CommonUtilities.getSYS_SETTING("EntireWebsiteHttps") == "1" || CommonUtilities.getSYS_SETTING("EntireWebsiteHttps") == "true")
            {
                if (Request.Url.AbsoluteUri.StartsWith("http://"))
                //        && !(_origionalPath.ToLower().StartsWith("/pdfgenerator/pdf_rnt_reservation_voucher_download.aspx") ||
                //_origionalPath.ToLower().StartsWith("/pdfgenerator/pdf_rnt_reservation_invoice_download.aspx") ||
                //_origionalPath.ToLower().StartsWith("/stp_contentonly_download.aspx") ||
                //_origionalPath.ToLower().StartsWith("/pdfgenerator/logo.gif") ||
                //_origionalPath.StartsWith("/pdfgenerator/logo_RiF.gif") ||
                //_origionalPath.StartsWith("/pdfgenerator/logo_RiV.gif") ||
                //_origionalPath.StartsWith("/pdfgenerator/Banner-Rome-Free-Lounge.jpg") ||            
                //_origionalPath.ToLower().StartsWith("/pdfgenerator/paid.png") ||            
                //_origionalPath.ToLower().StartsWith("/pdfgenerator/style_pdf.css") ||
                //_origionalPath.StartsWith("/admin/modRental/ResListPdf_Payments.aspx") ||
                //_origionalPath.StartsWith("/admin/modRental/EstateDettPdf_priceList.aspx") ||
                //_origionalPath.StartsWith("/admin/modRental/ResListPdf_forOwner.aspx") ||
                //_origionalPath.StartsWith("/areariservataprop/EstateDettPdf_priceList.aspx")))
                {
                    _origionalPath += _queryString != "" ? _queryString : "";
                    Response.StatusCode = 301;
                    Response.AddHeader("Location", CurrentAppSettings.HOST.Replace("http://", "https://") + _origionalPath);
                    Response.End();
                    return;
                }
            }
            else
            {
                #region old https
                if (Request.Url.AbsoluteUri.StartsWith("https://"))
                {
                    bool _httpsAllowed = false;
                    foreach (string _path in _httpsAllowedList)
                    {
                        if (_origionalPath.StartsWith(_path))
                        {
                            _httpsAllowed = true;
                            break;
                        }
                    }
                    if (!_httpsAllowed)
                    {
                        _origionalPath += _queryString != "" ? _queryString : "";
                        Response.StatusCode = 301;
                        //Response.AddHeader("Location", "http://www.rentalinrome.com" + _origionalPath);
                        Response.AddHeader("Location", App.HOST + _origionalPath);
                        Response.End();
                        return;
                    }
                    else if (!Request.Url.AbsoluteUri.StartsWith(App.HOST_SSL)) //(!Request.Url.AbsoluteUri.StartsWith("https://rentalinrome.com"))
                    {
                        _origionalPath += _queryString != "" ? _queryString : "";
                        Response.StatusCode = 301;
                        //Response.AddHeader("Location", "https://rentalinrome.com" + _origionalPath);
                        Response.AddHeader("Location", App.HOST_SSL + _origionalPath);
                        Response.End();
                        return;
                    }
                }
                else
                {
                    if (CommonUtilities.getSYS_SETTING("paypal_debug") == "true" && _origionalPath.ToLower().StartsWith("/util_paypal"))
                        ErrorLog.addLog("", "paypal-https not allowed", _origionalPath);

                    if ((CommonUtilities.getSYS_SETTING("sslForceAdmin").ToInt32() == 1 || CommonUtilities.getSYS_SETTING("sslForceAdmin") == "true") && App.HOST_SSL != App.HOST && _origionalPath.ToLower().StartsWith("/admin"))
                    {
                        _origionalPath += _queryString != "" ? _queryString : "";
                        Response.StatusCode = 301;
                        Response.AddHeader("Location", App.HOST_SSL + _origionalPath);
                        Response.End();
                        return;
                    }
                    else if (App.HOST_SSL != App.HOST && _origionalPath.ToLower().StartsWith("/util_paypal"))
                    {
                        if (CommonUtilities.getSYS_SETTING("paypal_debug") == "true")
                            ErrorLog.addLog("", "paypal-https not allowed done", _origionalPath);

                        _origionalPath += _queryString != "" ? _queryString : "";
                        Response.StatusCode = 301;
                        Response.AddHeader("Location", App.HOST_SSL + _origionalPath);
                        Response.End();
                        return;
                    }
                }
                #endregion
            }

            if (_origionalPath.ToLower() == "/pippo2xxx")
            {
                Context.Response.Clear();
                string physicalPath = Path.Combine(App.SRP, "images/press-business.jpg");
                if (File.Exists(physicalPath))
                {
                    Context.Response.ContentType = physicalPath;
                    Context.Response.WriteFile(physicalPath);
                    Context.Response.End();
                    return;
                }
            }

            if (_queryString.Length != 0) _queryString = "&" + _queryString.Substring(1, _queryString.Length - 1);
            else _queryString = "";

            if (_origionalPath == "/" || _origionalPath == "") { Context.RewritePath("/Default.aspx?id=1&" + _queryString, false); return; }

            if (WLRental != null)
            {
                if (_origionalPath == "/" || _origionalPath == "" || (_origionalPath + "").ToLower() == "/default.aspx") { Context.RewritePath("/" + "WLRental" + "/Default.aspx?id=1&" + _queryString, false); return; }
                UrlList list_tool = App.getCurrUrlList(WLRental.id);
                UrlItem item = list_tool.Items.FirstOrDefault(x => x.Url.ToLower() == _origionalPath.ToLower());

                if (item != null)
                    if (isMobile)
                        Context.RewritePath(CurrentAppSettings.ROOT_PATH + "mobile/" + item.Value + _queryString, false);
                    else
                        Context.RewritePath(CurrentAppSettings.ROOT_PATH + item.Value + _queryString, false);
            }
            else
            {
                UrlList list_tool = App.CurrUrlList;
                UrlItem item = list_tool.Items.FirstOrDefault(x => x.Url.ToLower() == _origionalPath.ToLower());

                if (item != null)
                    if (isMobile)
                        Context.RewritePath(CurrentAppSettings.ROOT_PATH + "mobile/" + item.Value + _queryString, false);
                    else
                        Context.RewritePath(CurrentAppSettings.ROOT_PATH + item.Value + _queryString, false);
            }

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            string _ip = "...";
            try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
            catch (Exception ex1) { }

            var blockedIp = ModAppServerCommon.BlockedIpTool.CurrList.FirstOrDefault(x => x.ip == _ip);
            if (blockedIp != null)
            {
                Server.ClearError();
                Response.StatusCode = 401;
                Response.End();
                return;
            }
            Exception exc = Server.GetLastError();
            if (exc.InnerException != null && exc.InnerException.Message != null && exc.InnerException.Message == "InnerException") return;
            if (Request.Url.AbsoluteUri.ToLower().Contains("http://localhost") || Request.Url.AbsoluteUri.ToLower().Contains(".magadesign.net") || Request.Url.AbsoluteUri.ToLower().Contains(".dev.magarental.com")) return;
            try
            {
                string _origionalPath = Request.Url.LocalPath;
                if (_origionalPath.EndsWith("/"))
                    _origionalPath = _origionalPath.Substring(0, _origionalPath.Length - 1);

                if (_origionalPath.StartsWith("/javascript:"))
                {
                    Server.ClearError();
                    Response.StatusCode = 301;
                    Response.AddHeader("Location", "/");
                    Response.End();
                    return;
                }

                // nel caso di motori ricerca cercano pagine indicizzate prima con caratteri non ammesse
                RedirectList _redirectList = UrlRedirectTool.CURRENT_TOOL;
                RedirectItem _item = _redirectList.Items.FirstOrDefault(x => x.From == _origionalPath.ToLower());
                if (_item != null)
                {
                    Server.ClearError();
                    Response.StatusCode = 301;
                    Response.AddHeader("Location", _item.To);
                    Response.End();
                    return;
                }

                // nel caso di motori ricerca cercano pagine con caratteri in cirilico
                UrlList list_tool = App.CurrUrlList;
                UrlItem item = list_tool.Items.FirstOrDefault(x => x.Url.ToLower() == _origionalPath.urlDecode().ToLower());
                if (item != null)
                {
                    Server.ClearError();
                    Response.StatusCode = 301;
                    Response.AddHeader("Location", item.Url);
                    Response.End();
                    return;
                }

                int _extIndex = _origionalPath.LastIndexOf(".");
                string _extension = "aspx";
                if (_extIndex != -1)
                    _extension = _origionalPath.Substring(_extIndex + 1);
                if (_extList.Contains(_extension))
                {
                    Server.ClearError();
                    return;
                }
            }
            catch (Exception ex1) { }
            string _params = "";
            foreach (string key in Request.Params.AllKeys)
                _params += "\n" + key + "=" + Request.Params[key];

            ErrorLog.addLog(_ip, "global", exc.ToString() + "\n\n______________________\n\nURL: " + Request.Url.AbsoluteUri.ToString() + "\n\n______________________\n\nRequest:\n " + _params);
            //exc.t
            //todo toglere dopo
            Server.ClearError();
            Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected void getMaxSizeImage(string sourcePath, string mimeType, int maxSize)
        {
            using (var img = Image.FromFile(sourcePath))
            {
                var newWidth = 0;
                var newHeight = 0;
                if (maxSize == 0) maxSize = 1500;
                if (img.Width < maxSize && img.Height < maxSize) return;

                if (img.Width > img.Height)
                {
                    newWidth = maxSize;
                    newHeight = decimal.Multiply(decimal.Divide(img.Height, img.Width), newWidth).objToInt32();
                }
                else
                {
                    newHeight = maxSize;
                    newWidth = decimal.Multiply(decimal.Divide(img.Width, img.Height), newHeight).objToInt32();
                }
                var oBitmap = new Bitmap(newWidth, newHeight);

                var thumbGraph = Graphics.FromImage(oBitmap);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(img, imageRectangle);
                MemoryStream mem = new MemoryStream();
                oBitmap.Save(mem, ImageFormat.Jpeg);

                byte[] buffer = mem.ToArray();
                Response.Clear();
                Response.ContentType = mimeType;
                Response.BinaryWrite(buffer);
                Response.Flush();
                Response.End();
            }
        }
    }
}