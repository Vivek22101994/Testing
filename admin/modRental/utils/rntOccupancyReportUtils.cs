using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class rntOccupancyReportUtils
{
    public static DaysAmout GetTotalBookedDaysPerMonth(List<RNT_TBL_RESERVATION> currResList, int currentMonth, int currentYear, int totalMonthDays)
    {
        DaysAmout objDayAmount = new DaysAmout();
        int totalBookedDays = 0;
        decimal totalAmount = 0;

        int totalnumResStartDate = 0;
        decimal totStartDateAmount = 0;

        int totalnumResCreatedDate = 0;
        decimal totCreatedDateAmount = 0;

        //calculate number of reserations of which start date is in current month and its amount
        var ResStartDate = currResList.Where(x => x.dtStart.Value.Month == currentMonth && x.dtStart.Value.Year == currentYear).ToList();
        totStartDateAmount = ResStartDate.Select(x => x.pr_total).ToList().Sum().objToDecimal();
        totalnumResStartDate = ResStartDate.Count;

        //calculate number of reserations of which creation date is in current month and its amount
        var ResCreatedDate = currResList.Where(x => x.dtCreation.Value.Month == currentMonth && x.dtCreation.Value.Year == currentYear).ToList();
        totCreatedDateAmount = ResCreatedDate.Select(x => x.pr_total).ToList().Sum().objToDecimal();
        totalnumResCreatedDate = ResCreatedDate.Count;

        // calculate current month booked days 
        var resList = currResList.Where(x => x.dtStart.Value.Month == currentMonth && x.dtEnd.Value.Month == currentMonth && x.dtStart.Value.Year == currentYear && x.dtEnd.Value.Year == currentYear).ToList();
        totalBookedDays = resList.Select(x => (x.dtEnd.Value - x.dtStart.Value).TotalDays).Sum().objToInt32();
        totalAmount = resList.Select(x => x.pr_total.objToDecimal()).Sum();

        // for reservation have check-in date in current month  and check out date in next month
        var tmpRes = currResList.Where(x => x.dtStart.Value.Month == currentMonth && x.dtEnd.Value.Month != currentMonth && x.dtStart.Value.Year == currentYear).ToList();
        var endBookDays = endBookedDays(tmpRes, totalMonthDays);
        totalBookedDays += endBookDays.days;
        totalAmount += endBookDays.amount;

        // for reservation have check-out date in current month  and check-in date in previous month
        tmpRes = currResList.Where(x => x.dtStart.Value.Month != currentMonth && x.dtEnd.Value.Month == currentMonth && x.dtEnd.Value.Year == currentYear).ToList();
        var startBookDays = startBookedDays(tmpRes);
        totalBookedDays += startBookDays.days;
        totalAmount += startBookDays.amount;

        //for reservation have check-in date in previous months and check-out date in next months
        var currStartDate = new DateTime(currentYear, currentMonth, 1);
        var currEndDate = new DateTime(currentYear, currentMonth, totalMonthDays);
        tmpRes = currResList.Where(x => x.dtStart.Value < currStartDate && x.dtEnd.Value > currEndDate).ToList();
        var middleBookDays = MiddleBookedDays(tmpRes, totalMonthDays);
        totalBookedDays += middleBookDays.days;
        totalAmount += middleBookDays.amount;

        objDayAmount.days = totalBookedDays;
        objDayAmount.amount = totalAmount;

        objDayAmount.startDateAmount = totStartDateAmount;
        objDayAmount.numResStartDate = totalnumResStartDate;

        objDayAmount.createdDateAmount = totCreatedDateAmount;
        objDayAmount.numRescreatedDate = totalnumResCreatedDate;
        return objDayAmount;
    }

    private static DaysAmout endBookedDays(List<RNT_TBL_RESERVATION> currResList, int totalMonthDays)
    {
        DaysAmout objDayAmount = new DaysAmout();
        foreach (RNT_TBL_RESERVATION res in currResList)
        {
            objDayAmount.days += ((totalMonthDays - res.dtStart.Value.Date.Day) + 1);

            #region calculating amount
            int totalResDays = (res.dtEnd.Value - res.dtStart.Value).TotalDays.objToInt32();
            decimal dailyPrice = res.pr_total.objToDecimal() / totalResDays;
            objDayAmount.amount += dailyPrice * objDayAmount.days;
            #endregion
        }
        return objDayAmount;
    }

    private static DaysAmout startBookedDays(List<RNT_TBL_RESERVATION> currResList)
    {
        DaysAmout objDayAmount = new DaysAmout();
        foreach (RNT_TBL_RESERVATION res in currResList)
        {
            objDayAmount.days += (res.dtEnd.Value.Date.Day - 1);

            #region calculating amount
            int totalResDays = (res.dtEnd.Value - res.dtStart.Value).TotalDays.objToInt32();
            decimal dailyPrice = res.pr_total.objToDecimal() / totalResDays;
            objDayAmount.amount += dailyPrice * objDayAmount.days;
            #endregion
        }
        return objDayAmount;
    }

    private static DaysAmout MiddleBookedDays(List<RNT_TBL_RESERVATION> currResList, int totalMonthDays)
    {
        DaysAmout objDayAmount = new DaysAmout();
        foreach (RNT_TBL_RESERVATION res in currResList)
        {
            objDayAmount.days += totalMonthDays;

            #region calculating amount
            int totalResDays = (res.dtEnd.Value - res.dtStart.Value).TotalDays.objToInt32();
            decimal dailyPrice = res.pr_total.objToDecimal() / totalResDays;
            objDayAmount.amount += dailyPrice * objDayAmount.days;
            #endregion
        }
        return objDayAmount;
    }

    public class DaysAmout
    {
        //days - number of days booked of particular month
        //amount - reservation price according to reserved days in particular month - splitted prive 
        public int days { get; set; }
        public decimal amount { get; set; }

        //numResStartDate - numner of reservations of which start date in current month
        //startDateAmount - total reservation amount of which start date in current month
        public int numResStartDate { get; set; }
        public decimal startDateAmount { get; set; }

        //numRescreatedDate - numner of reservations of which created date in current month
        //createdDateAmount - total reservation amount of which created date in current month
        public int numRescreatedDate { get; set; }
        public decimal createdDateAmount { get; set; }

    }

    public class Total
    {
        public int currYear { get; set; }
        public int currMonth { get; set; }
        public int totDays { get; set; }
        public int totMonthDays { get; set; }
        public decimal totAmount { get; set; }

        public decimal totStartDateAmount { get; set; }
        public int totnumResStartDate { get; set; }

        public decimal totCreatedDateAmount { get; set; }
        public int totnumResCreatedDate { get; set; }

    }
}
