using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;
using RentalInRome.data;

namespace RentalInRome.xmlapi.ical
{
    public partial class get : System.Web.UI.Page
    {
        private int IdEstate;
        private RNT_TB_ESTATE currTBL;
        private DateTime dtStart;
        private DateTime dtEnd;
        private int year;
        private int month;
        private long pidAgent;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();

            App.LangID = Request["lang"].objToInt32() == 0 ? 2 : Request["lang"].objToInt32();
            IdEstate = Request["id"].objToInt32();
            currTBL = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Write("Accomodation not found.");
                Response.End();
            }
            using (ModRental.DCmodRental dc = new ModRental.DCmodRental())
            {
                var ChannelManager = dc.dbRntChannelManagerTBLs.SingleOrDefault(x => x.code.ToLower() == (Request["channel"] + "").ToLower() && x.isActive == 1);
                if (ChannelManager != null)
                {
                    pidAgent = ChannelManager.pidAgent.objToInt64();
                }
            }



            year = Request.QueryString["year"].objToInt32();
            month = Request.QueryString["month"].objToInt32();

            if (year == 0)
                year = DateTime.Now.Year;

            getList();

            int dtStartInt = (Request.QueryString["start"] + "").Replace("-", "").objToInt32();
            int dtEndInt = (Request.QueryString["end"] + "").Replace("-", "").objToInt32();
            bool noDates = (dtStartInt < DateTime.Now.AddYears(-1).JSCal_dateToInt() || dtStartInt > DateTime.Now.AddYears(2).JSCal_dateToInt() || dtEndInt < DateTime.Now.AddYears(-1).JSCal_dateToInt() || dtEndInt > DateTime.Now.AddYears(2).JSCal_dateToInt());
            if (!noDates && dtStartInt > dtEndInt)
            {
                Response.Write("<root><check>fail</check><error>End date should be greater than start date.</error></root>");
                Response.End();
                return;
            }
            dtStart = dtStartInt.JSCal_intToDate();
            dtEnd = dtEndInt.JSCal_intToDate();
        }
        protected void getList()
        {
            if (year >= 2012)
            {
                if (month > 0 && month < 13)
                {
                    dtStart = new DateTime(year, month, 1);
                    dtEnd = dtStart.AddMonths(1).AddDays(-1);
                }
                else
                {
                    dtStart = new DateTime(year, 1, 1);
                    dtEnd = dtStart.AddYears(5).AddDays(-1);
                }
            }
            var resList = rntUtils.rntEstate_resList(IdEstate, dtStart, dtEnd, 0).OrderBy(x => x.dtStart).ToList();
            iCalendar iCal = new iCalendar();
            iCal.Method = "PUBLISH";
            iCal.Scale = "GREGORIAN";
            iCal.ProductID = "-//" + App.ProjectName + "//iCalendar export//EN";
            foreach (var res in resList)
            {
                if (pidAgent != 0 && pidAgent == res.agentID.objToInt64() && res.state_pid != 4) continue;
                // Create the event, and add it to the iCalendar
                Event evtRes = iCal.Create<Event>();

                // Set information about the event
                evtRes.UID = "" + res.unique_id;
                evtRes.Start = new iCalDateTime(res.dtStart.Value.AddHours(14));
                evtRes.End = new iCalDateTime(res.dtEnd.Value.AddHours(10));
                evtRes.Class = "PUBLIC";
                evtRes.Created = new iCalDateTime(res.dtCreation.Value);
                evtRes.LastModified = new iCalDateTime(res.state_date.Value);
                evtRes.Description = "Reservation #" + res.code;
                evtRes.Location = "";
                evtRes.Status = EventStatus.Confirmed;
                evtRes.Transparency = TransparencyType.Opaque;
                evtRes.Summary = "";
            }
            iCalendarSerializer serializer = new iCalendarSerializer();
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            Encoding encoding = new UTF8Encoding(false);
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();

            response.ContentType = "text/calendar; charset=utf-8";
            response.Charset = "UTF-8";
            response.ContentEncoding = encoding;
            //response.AddHeader("Content-Disposition", "inline; filename=" + currTBL.code.clearPathName() + "_" + dtStart.JSCal_dateToString() + "-" + dtEnd.JSCal_dateToString() + ".ics;");
            serializer.Serialize(iCal, response.OutputStream, encoding);
            response.End();

            return;

            // Create the event, and add it to the iCalendar
            Event evt = iCal.Create<Event>();

            // Set information about the event
            evt.Start = iCalDateTime.Today.AddHours(8);
            evt.End = evt.Start.AddHours(18); // This also sets the duration
            evt.Description = "The event description";
            evt.Location = "Event location";
            evt.Summary = "18 hour event summary";


            // Serialize (save) the iCalendar
            serializer = new iCalendarSerializer();
            serializer.Serialize(iCal, @"iCalendar.ics");
            Console.WriteLine("iCalendar file saved." + Environment.NewLine);

            // Load the calendar from the file we just saved
            IICalendarCollection calendars = iCalendar.LoadFromFile(@"iCalendar.ics");
            Console.WriteLine("iCalendar file loaded.");

        }
    }
}