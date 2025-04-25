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
    public partial class errorSrsLog : adminBasePage
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
                        var item = dc.dbAuthErrorSrsLOGs.SingleOrDefault(x => x.uid == uid);
                        if (item != null)
                        {
                            if (item.logUrl == "Location_Insert_Update")
                            {
                                var _currTBL = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == item.logIp.ToInt32());
                                if (_currTBL != null)
                                {
                                    string _coords = "";
                                    string _address = "";
                                    if (_currTBL.is_srs == 1)
                                    {
                                        _coords = _currTBL.google_maps ?? "";
                                        _coords = _coords.Replace(",", ".").Replace("|", ",");
                                        _address = _currTBL.loc_address;
                                        Srs_WS.Location_Insert_Update(_currTBL.code, CurrentSource.locZone_title(_currTBL.pid_zone.objToInt32(), 1, "---"), _address, _currTBL.id, 1, _currTBL.num_bed_single.objToInt32(), _currTBL.num_bed_double.objToInt32(), _currTBL.num_rooms_bath.objToInt32(), 0, _coords);
                                    }
                                }
                            }
                            if (item.logUrl == "LocationEvent_Insert_Update")
                            {
                                Srs_WS.LocationEvent_Insert_Update(maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == item.logIp.ToInt64()));
                            }
                            if (item.logUrl == "LocationEvent_Delete" && item.logIp.ToInt64() > 0)
                            {
                                Srs_WS.LocationEvent_Delete(maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == item.logIp.ToInt64()));
                            }
                            else if (item.logUrl == "LocationEvent_Delete")
                            {
                                Srs_WS.LocationEvent_Delete(item.logIp.splitStringToList("|").Select(x => x.ToInt64()).ToList());
                            }
                            if (item.logUrl == "EstateReservations_UpdateAll")
                            {
                                Srs_WS.EstateReservations_UpdateAll(item.logIp.ToInt32());
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
        protected void lnk_Resend_all_Click(object sender, EventArgs e)
        {
            ResendSrsWork.ResendSrs_Start();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\" Resend Started successfully\", 340, 110);", true);
        }
        class ResendSrsWork
        {
            private class ResendSrs_Process
            {
                void doThread()
                {
                    ErrorLog.addLog("", "ResendSrs_Process", "Resend Started " + DateTime.Now);
                    using (DCmodAuth DC = new DCmodAuth())
                    {
                        var resList = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.ToList();
                        var tempLogs = DC.dbAuthErrorSrsLOGs.ToList();
                        foreach (dbAuthErrorSrsLOG currLog in tempLogs)
                        {
                            try
                            {
                                #region Update Srs
                                if (currLog != null)
                                {
                                    if (currLog.logUrl == "Location_Insert_Update")
                                    {
                                        var _currTBL = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currLog.logIp.ToInt32());
                                        if (_currTBL != null)
                                        {
                                            string _coords = "";
                                            string _address = "";
                                            if (_currTBL.is_srs == 1)
                                            {
                                                _coords = _currTBL.google_maps ?? "";
                                                //_coords = _coords.Replace(",", ".").Replace("|", ",");
                                                _address = _currTBL.loc_address;
                                                Srs_WS.Location_Insert_Update(_currTBL.code, CurrentSource.locZone_title(_currTBL.pid_zone.objToInt32(), 1, "---"), _address, _currTBL.id, 1, _currTBL.num_bed_single.objToInt32(), _currTBL.num_bed_double.objToInt32(), _currTBL.num_rooms_bath.objToInt32(), 0, _coords);
                                            }
                                        }
                                    }
                                    if (currLog.logUrl == "LocationEvent_Insert_Update")
                                    {
                                        if (currLog.logIp.objToInt64() > 0)
                                        {
                                            var currRes = resList.SingleOrDefault(x => x.id == currLog.logIp.objToInt64());
                                            if (currRes != null)
                                                Srs_WS.LocationEvent_Insert_Update(currRes);
                                        }
                                    }
                                    if (currLog.logUrl == "LocationEvent_Delete" && currLog.logIp.ToInt64() > 0)
                                    {
                                        if (currLog.logIp.objToInt64() > 0)
                                        {
                                            var currRes = resList.SingleOrDefault(x => x.id == currLog.logIp.objToInt64());
                                            if (currRes != null)
                                                Srs_WS.LocationEvent_Delete(currRes);
                                        }
                                    }
                                    else if (currLog.logUrl == "LocationEvent_Delete")
                                    {
                                        if (currLog.logIp.splitStringToList("|").Select(x => x.objToInt64()).Count() > 0)
                                            Srs_WS.LocationEvent_Delete(currLog.logIp.splitStringToList("|").Select(x => x.objToInt64()).ToList());
                                    }
                                    if (currLog.logUrl == "EstateReservations_UpdateAll")
                                    {
                                        if (currLog.logIp.objToInt32() > 0)
                                            Srs_WS.EstateReservations_UpdateAll(currLog.logIp.objToInt32());
                                    }
                                    DC.Delete(currLog);
                                    DC.SaveChanges();
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.addLog("", "ResendSrs_Process" + currLog.uid.ToString(), ex.ToString());
                            }

                        }
                    }
                    ErrorLog.addLog("", "ResendSrs_Process", "Resend Completed " + DateTime.Now);
                }
                public ResendSrs_Process()
                {
                    Action<object> action = (object obj) => { doThread(); };
                    AppUtilsTaskSchedulerPriority.AddTask(action, "ResendSrs");
                }
            }
            public static void ResendSrs_Start()
            {
                ResendSrs_Process _tmp = new ResendSrs_Process();
            }
        }

    }
}
