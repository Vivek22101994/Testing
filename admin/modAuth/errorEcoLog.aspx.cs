using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModAuth.admin.modAuth
{
    public partial class errorEcoLog : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Guid uid;
                if (Request.QueryString["resend"] + "" != "" && Guid.TryParse(Request.QueryString["resend"], out uid))
                {
                    using (DCmodAuth dc = new DCmodAuth())
                    {
                        var item = dc.dbAuthErrorEcoLOGs.SingleOrDefault(x => x.uid == uid);
                        if (item != null)
                        {
                            if (item.logUrl == "Location_Insert_Update")
                            {
                                Eco_WS.Location_Insert_Update(AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == item.logIp.ToInt32()));
                            }
                            if (item.logUrl == "LocationEvent_Insert_Update")
                            {
                                Eco_WS.LocationEvent_Insert_Update(maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == item.logIp.ToInt64()));
                            }
                            if (item.logUrl == "LocationEvent_Delete" && item.logIp.ToInt64()>0)
                            {
                                Eco_WS.LocationEvent_Delete(maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == item.logIp.ToInt64()));
                            }
                            else if (item.logUrl == "LocationEvent_Delete")
                            {
                                Eco_WS.LocationEvent_Delete(item.logIp.splitStringToList("|").Select(x => x.ToInt64()).ToList());
                            }
                            if (item.logUrl == "EstateReservations_UpdateAll")
                            {
                                Eco_WS.EstateReservations_UpdateAll(item.logIp.ToInt32());
                            }
                            dc.Delete(item);
                            dc.SaveChanges();
                        }
                    }
                }
                rdtp_DateFrom.SelectedDate = DateTime.Now.Date.AddDays(-7);
                setfilters();
                Fill_LV();
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            Fill_LV();
        }
        protected void setfilters()
        {
            string _filter = "";
            string _sep = "";
            if (txt_IP.Text.Trim() != "")
            {
                _filter += _sep + "logIp.Contains(\"" + txt_IP.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_Url.Text.Trim() != "")
            {
                _filter += _sep + "logUrl.Contains(\"" + txt_Url.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (rdtp_DateFrom.SelectedDate.HasValue)
            {
                _filter += _sep + "logDateTime >= DateTime.Parse(\"" + rdtp_DateFrom.SelectedDate + "\")";
                _sep = " and ";
            }
            if (rdtp_DateTo.SelectedDate.HasValue)
            {
                _filter += _sep + "logDateTime < DateTime.Parse(\"" + rdtp_DateTo.SelectedDate + "\")";
                _sep = " and ";
            }
            _filter += _sep + "1 = 1";
            ltrLDSfiltter.Text = _filter;
        }
        protected void Fill_LV()
        {
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            Label lbl_id = LV.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            if (lbl_id != null)
            {
            }
            Fill_LV();
            LV.SelectedIndex = e.NewSelectedIndex;
        }
        protected void drp_logList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }
        protected void lnk_flt_Click(object sender, EventArgs e)
        {
            setfilters();
            Fill_LV();
        }

    }
}
