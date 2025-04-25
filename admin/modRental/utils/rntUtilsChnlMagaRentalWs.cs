using ModRental;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Api = RentalInRome.WsChnlMagaRentalWs;
using RentalInRome.data;
using ModRental;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

public class ChnlMagaRentalWsUpdate
{
    private class BookingUpdate_process
    {
        public long ResId { get; set; }
        private RNT_TBL_RESERVATION resTbl { get; set; }
        //dbRntChnlMagaRentalWsSourceTBL SourceTbl { get; set; }
        public string ErrorString { get; set; }

        private USR_TBL_CLIENT _currClient;
        private USR_TBL_CLIENT tblClient
        {
          get
            {
                if (_currClient == null)
                    _currClient = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == resTbl.cl_id);
                if (_currClient == null && resTbl.agentClientID.objToInt64() > 0)
                    _currClient = authUtils.getClientFromAgent(resTbl.agentClientID.objToInt64());
                return _currClient ?? new USR_TBL_CLIENT();
            }
        }
        void doThread()
        {
            //try
            //{
            //    using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
            //    {
            //        //using (DCchnlMagaRentalWs dc = new DCchnlMagaRentalWs())
            //        //{
            //        resTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == ResId);
            //        if (resTbl == null)
            //        {
            //            return;
            //        }
            //        if (!string.IsNullOrEmpty(resTbl.chnlMReservationCode) || (resTbl.chnlMReservationLoading.HasValue && resTbl.chnlMReservationLoading.Value >= DateTime.Now.AddHours(-1)))
            //        {
            //            var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == resTbl.pid_estate);
            //            if (currEstate == null)
            //            {
            //                return;
            //            }
            //            SourceTbl = dc.dbRntChnlMagaRentalWsSourceTBLs.SingleOrDefault(x => x.sourceId == currEstate.chnlMSourceId.ToInt64() && x.isActive == 1);
            //            if (SourceTbl == null)
            //            {
            //                return;
            //            }
            //            Api.v01 siteClient = new Api.v01();
            //            siteClient.Url = SourceTbl.apiUrl;
            //            Api.AuthHeader auth = new Api.AuthHeader();
            //            auth.Username = SourceTbl.username;
            //            auth.Password = SourceTbl.password;
            //            auth.ApiKey = SourceTbl.apiKey;
            //            siteClient.AuthHeaderValue = auth;
            //            if (!siteClient.Authentication_Test())
            //            {
            //                ErrorLog.addLog("ChnlMagaRentalWsUpdate", "BookingUpdate_process", "ResId:" + resTbl.id + " idEstate:" + resTbl.pid_estate + " not found or not active");
            //                return;
            //            }
            //            resTbl.chnlMReservationLoading = DateTime.Now;
            //            DC_RENTAL.SubmitChanges();
            //            Api.UpdateBookingRequest request = new Api.UpdateBookingRequest();
            //            request.ReservationCode = resTbl.chnlMReservationCode;
            //            request.Cancelled = resTbl.state_pid == 3;
            //            request.DateStart = resTbl.dtStart.Value;
            //            request.DateEnd = resTbl.dtEnd.Value;
            //            request.NumAdults = resTbl.num_adult.objToInt32();
            //            request.NumChilds = resTbl.num_child_over.objToInt32();
            //            request.NumEnfants = resTbl.num_child_min.objToInt32();
            //            request.GuestFirstName = resTbl.cl_name_full + "";
            //            request.GuestLastName = "";
            //            request.GuestEmail = resTbl.cl_email + "";
            //            request.GuestMobilePhone = tblClient.contact_phone_mobile;
            //            request.GuestCountry = tblClient.loc_country;
            //            request.GuestState = tblClient.loc_state;
            //            request.GuestCity = tblClient.loc_city;
            //            request.GuestAddress = tblClient.loc_address;

            //            var unitResponse = siteClient.UpdateBooking(request);
            //            if (unitResponse.Success == true)
            //            {
            //                resTbl.chnlMReservationLoading = (DateTime?)null;
            //                DC_RENTAL.SubmitChanges();
            //            }
            //            else
            //            {
            //                resTbl.chnlMReservationLoading = (DateTime?)null;
            //                DC_RENTAL.SubmitChanges();
            //                ErrorString = "";
            //                ChnlMagaRentalWsUtils.addLog("ChnlMagaRentalWsUpdate", "ERROR:BookingUpdate_process", "ResId:" + resTbl.id + " idEstate:" + resTbl.pid_estate + " msg:" + unitResponse.Msg, ErrorString);
            //            }
            //        }
            //        else
            //        {
            //            var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == resTbl.pid_estate);
            //            if (currEstate == null)
            //            {
            //                return;
            //            }
            //            SourceTbl = dc.dbRntChnlMagaRentalWsSourceTBLs.SingleOrDefault(x => x.sourceId == currEstate.chnlMSourceId.ToInt64() && x.isActive == 1);
            //            if (SourceTbl == null)
            //            {
            //                return;
            //            }
            //            Api.v01 siteClient = new Api.v01();
            //            siteClient.Url = SourceTbl.apiUrl;
            //            Api.AuthHeader auth = new Api.AuthHeader();
            //            auth.Username = SourceTbl.username;
            //            auth.Password = SourceTbl.password;
            //            auth.ApiKey = SourceTbl.apiKey;
            //            siteClient.AuthHeaderValue = auth;
            //            if (!siteClient.Authentication_Test())
            //            {
            //                ErrorLog.addLog("ChnlMagaRentalWsUpdate", "BookingUpdate_process", "ResId:" + resTbl.id + " idEstate:" + resTbl.pid_estate + " not found or not active");
            //                return;
            //            }
            //            resTbl.chnlMReservationLoading = DateTime.Now;
            //            DC_RENTAL.SubmitChanges();
            //            Api.CreateBookingRequest request = new Api.CreateBookingRequest();
            //            request.PropertyId = currEstate.chnlMPropertyId;
            //            request.DateStart = resTbl.dtStart.Value;
            //            request.DateEnd = resTbl.dtEnd.Value;
            //            request.NumAdults = resTbl.num_adult.objToInt32();
            //            request.NumChilds = resTbl.num_child_over.objToInt32();
            //            request.NumEnfants = resTbl.num_child_min.objToInt32();
            //            request.GuestFirstName = resTbl.cl_name_full + "";
            //            request.GuestLastName = "";
            //            request.GuestEmail = resTbl.cl_email + "";
            //            request.GuestMobilePhone = tblClient.contact_phone_mobile;
            //            request.GuestCountry = tblClient.loc_country;
            //            request.GuestState = tblClient.loc_state;
            //            request.GuestCity = tblClient.loc_city;
            //            request.GuestAddress = tblClient.loc_address;

            //            var unitResponse = siteClient.CreateBooking(request);
            //            if (unitResponse.Success == true)
            //            {
            //                resTbl.chnlMReservationLoading = (DateTime?)null;
            //                resTbl.chnlMSourceId = SourceTbl.sourceId + "";
            //                resTbl.chnlMPropertyId = currEstate.chnlMPropertyId;
            //                resTbl.chnlMReservationCode = unitResponse.ReservationCode;
            //                DC_RENTAL.SubmitChanges();
            //            }
            //            else
            //            {
            //                resTbl.chnlMReservationLoading = (DateTime?)null;
            //                DC_RENTAL.SubmitChanges();
            //                ErrorString = "";
            //                ChnlMagaRentalWsUtils.addLog("ChnlMagaRentalWsUpdate", "ERROR:BookingUpdate_process", "ResId:" + resTbl.id + " idEstate:" + resTbl.pid_estate + " msg:" + unitResponse.Msg, ErrorString);
            //            }
            //        }
            //        //}
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrorLog.addLog("ChnlMagaRentalWsUpdate", "BookingUpdate_process ResId:" + ResId, ex.ToString());
            //}
        }
        public BookingUpdate_process(long resId)
        {
            ResId = resId;
            ErrorString = "";
            Action<object> action = (object obj) => { doThread(); };
            AppUtilsTaskScheduler.AddTask(action, "ChnlMagaRentalWsUpdate.BookingUpdate_process resId:" + resId);
        }
    }
    public static string BookingUpdate_start(long resId)
    {
        BookingUpdate_process _tmp = new BookingUpdate_process(resId);
        return _tmp.ErrorString;
    }

}
