using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ModRental.admin.modRental
{
    public partial class EstateDett_iCalImport : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE currTBL;
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);

                if (_est != null)
                {
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                    UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                    txt_iCalUrl.Text = _est.iCalUrl;
                    LvChannelManagers_DataBind();
                }
                else
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                }
            }
        }
        protected void LvChannelManagers_DataBind()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                LvChannelManagers.DataSource = dc.dbRntChannelManagerTBLs.Where(x => x.isActive == 1).OrderBy(x => x.code).ToList();
                LvChannelManagers.DataBind();
                foreach (ListViewDataItem item in LvChannelManagers.Items)
                {
                    var lbl_id = item.FindControl("lbl_id") as Label;
                    var txt_iCalImportUrl = item.FindControl("txt_iCalImportUrl") as TextBox;
                    var drp_iCalImportEnabled = item.FindControl("drp_iCalImportEnabled") as DropDownList;
                    var rl = dc.dbRntChannelManagerEstateRLs.SingleOrDefault(x => x.pidChannelManager == lbl_id.Text && x.pidEstate == IdEstate);
                    if (rl != null)
                    {
                        txt_iCalImportUrl.Text = rl.iCalImportUrl;
                        drp_iCalImportEnabled.setSelectedValue(rl.iCalImportEnabled);
                    }
                }
            }
        }
        protected void LvChannelManagers_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "import")
            {
                var lbl_id = e.Item.FindControl("lbl_id") as Label;
                var txt_iCalImportUrl = e.Item.FindControl("txt_iCalImportUrl") as TextBox;
                if (lbl_id == null) return;
                iCalImport("_" + lbl_id.Text, txt_iCalImportUrl.Text);
            }
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LV.Visible = false;
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);

            if (_est != null)
            {
                _est.iCalUrl = txt_iCalUrl.Text;
                DC_RENTAL.SubmitChanges();
            }
            using (DCmodRental dc = new DCmodRental())
            {
                foreach (ListViewDataItem item in LvChannelManagers.Items)
                {
                    var lbl_id = item.FindControl("lbl_id") as Label;
                    var txt_iCalImportUrl = item.FindControl("txt_iCalImportUrl") as TextBox;
                    var drp_iCalImportEnabled = item.FindControl("drp_iCalImportEnabled") as DropDownList;
                    var rl = dc.dbRntChannelManagerEstateRLs.SingleOrDefault(x => x.pidChannelManager == lbl_id.Text && x.pidEstate == IdEstate);
                    if (rl == null)
                    {
                        rl = new dbRntChannelManagerEstateRL() { pidChannelManager = lbl_id.Text, pidEstate = IdEstate };
                        dc.Add(rl);
                    }
                    rl.iCalImportUrl = txt_iCalImportUrl.Text;
                    rl.iCalImportEnabled = drp_iCalImportEnabled.getSelectedValueInt();
                    dc.SaveChanges();
                }
            }
            LvChannelManagers_DataBind();
        }
        protected void iCalImport(string channelManager, string iCalUrl)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(iCalUrl);
            try
            {
                RNT_TB_ESTATE _est = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (_est == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                if (iCalUrl.Contains(".housetrip.") || iCalUrl.Contains(".airbnb.") || iCalUrl.Contains(".tripadvisor."))
                {
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    request.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
                    //request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    request.Headers.Add("Accept-Language", "en-GB,en;q=0.5");

                    if (iCalUrl.Contains(".airbnb."))
                        request.ContentType = "application/x-www-form-urlencoded";
                    else
                        request.Headers.Add("Accept-Encoding", "gzip, deflate");
                }
                else
                {
                    request.Method = "GET";
                    if (iCalUrl.Contains(".homeaway."))
                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    else
                        request.ContentType = "application/x-www-form-urlencoded";
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string icalData = "";
                if (response.StatusCode.ToString().ToLower() == "ok")
                {
                    Stream content = response.GetResponseStream();
                    StreamReader contentReader = new StreamReader(content);
                    icalData = contentReader.ReadToEnd();
                }
                if (icalData == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert(\"calendario vuoto o formato non valido.\");", true);
                    return;
                }
                if (!Directory.Exists(Path.Combine(App.SRP, "files"))) Directory.CreateDirectory(Path.Combine(App.SRP, "files"));
                if (!Directory.Exists(Path.Combine(App.SRP, "files/tmp"))) Directory.CreateDirectory(Path.Combine(App.SRP, "files/tmp"));
                string filePath = "files/tmp/" + string.Empty.createUniqueID() + ".ics";
                StreamWriter ccWriter = new StreamWriter(Path.Combine(App.SRP, filePath), true);
                ccWriter.WriteLine(icalData); // Write the file.
                ccWriter.Flush();
                ccWriter.Close(); // Close the instance of StreamWriter.
                ccWriter.Dispose(); // Dispose from memory.

                if (!File.Exists(Path.Combine(App.SRP, filePath)))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert(\"Carica il file prima.\");", true);
                    return;
                }
                rntImportFromiCal xx = new rntImportFromiCal(IdEstate, Path.Combine(App.SRP, filePath), iCalUrl, channelManager);
                xx.StartImport();
                LV.DataSource = xx.ErrorDates.OrderBy(x => x.iCalDtStart).ToList();
                LV.DataBind();
                LV.Visible = true;
            }
            catch (Exception exc)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"" + exc.ToString().htmlNoWrap() + ".\", 340, 110);", true);
                //TextWriter wr = new StringWriter();
                //JsonSerializer serializer = new JsonSerializer();
                //serializer.NullValueHandling = NullValueHandling.Ignore;
                //serializer.Serialize(wr, request);
                //string requestContent = wr.ToString();

                //ErrorLog.addLog("", "iCalImport channelManager:" + channelManager + "<br/>iCalUrl:" + iCalUrl, requestContent);
            }
        }
        protected void lnk_import_Click(object sender, EventArgs e)
        {
            iCalImport("", txt_iCalUrl.Text);
        }

    }
}