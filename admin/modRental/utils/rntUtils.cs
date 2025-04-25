using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentalInRome.data;
using ModRental;
using System.Threading;
using System.Runtime.Serialization;

public partial class rntUtils
{
    public static void ReservationLastUpdate(long id)
    {
        using (DCmodRental dc = new DCmodRental())
        {
            var lastUpdatedDate = dc.dbRntReservationLastUpdatedLOGs.SingleOrDefault(x => x.id == id);
            if (lastUpdatedDate == null)
            {
                lastUpdatedDate = new dbRntReservationLastUpdatedLOG() { id = id };
                dc.Add(lastUpdatedDate);
            }
            lastUpdatedDate.lastUpdatedDate = DateTime.Now;
            dc.SaveChanges();
            //Update Log Variable
            //AppSettings.lstRntReservationLastUpdatedLOGs = null;
            //var updateLogVariable = AppSettings.lstRntReservationLastUpdatedLOGs;
        }
    }
    public static RNT_TBL_RESERVATION newReservation()
    {
        RNT_TBL_RESERVATION newRes = new RNT_TBL_RESERVATION();
        newRes.unique_id = Guid.NewGuid();
        newRes.uid_2 = Guid.NewGuid();
        newRes.dtCreation = DateTime.Now;
        newRes.is_deleted = 0;
        newRes.dtStartTime = "000000";
        newRes.dtEndTime = "000000";
        return newRes;
    }
    public static string getDiscountType_code(int id, string alternative)
    {
        var tmp = rntProps.DiscountTypeTBL.SingleOrDefault(x => x.id == id);
        if (tmp != null)
            return tmp.code;
        return alternative;
    }

    public static string getAgent_nameCompany(long id, string alternative)
    {
        var _s = rntProps.AgentTBL.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.nameCompany;
        return alternative;
    }
    public static string getAgent_logoForDetailsPage(string IdAdMedia)
    {
        var _s = rntProps.AgentTBL.SingleOrDefault(x => x.IdAdMedia == IdAdMedia);
        if (_s != null)
            return "<img src='/" + _s.imgLogo + "' class='homeAwayLogo' alt='Integrated with " + _s.nameCompany + "'  style='height: 65px;'/>";
        return "";
    }
    public static string getProblem_title(int id, string alternative)
    {
        dbRntProblemTBL _s = rntProps.ProblemTBL.SingleOrDefault(x => x.id == id);
        if (_s != null)
            return _s.title;
        return alternative;
    }
    public static void checkAgentMonthlyReport(long IdAgent)
    {
        var tmp = rntProps.AgentTBL.SingleOrDefault(x => x.id == IdAgent);
        if (tmp == null)
        {
            return;
        }
        DateTime dtStart = tmp.createdDate.HasValue ? tmp.createdDate.Value : DateTime.Now;
        DateTime dtEnd = DateTime.Now;
        dtStart = new DateTime(dtStart.Year, dtStart.Month, 1);
        while (dtStart <= new DateTime(dtEnd.Year, dtEnd.Month, 1))
        {
            checkAgentMonthlyReport(tmp, dtStart.Year, dtStart.Month);
            dtStart = dtStart.AddMonths(1);
        }
    }
    public static int checkAgentMonthlyReport(dbRntAgentTBL AgentTBL, int reportYear, int reportMonth)
    {
        int returnError = 0;
        // = 0; tutto aposto
        // = 1; ha delle pren non chiusi
        // = 2; report chiuso
        using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
        using (DCmodRental dcNew = new DCmodRental())
        {
            dbRntAgentMonthlyReportTBL currTBL = dcNew.dbRntAgentMonthlyReportTBLs.SingleOrDefault(x => x.pidAgent == AgentTBL.id && x.reportYear == reportYear && x.reportMonth == reportMonth);
            if (currTBL == null)
            {
                currTBL = new dbRntAgentMonthlyReportTBL();
                currTBL.pidAgent = AgentTBL.id;
                currTBL.reportYear = reportYear;
                currTBL.reportMonth = reportMonth;
                dcNew.Add(currTBL);
            }
            else if (currTBL.reportClosedDate.HasValue)
                return 2;
            currTBL.code = "" + reportYear.ToString().fillString("0", 4, false) + reportMonth.ToString().fillString("0", 2, false) + AgentTBL.id.ToString().fillString("0", 6, false);
            DateTime dtStart = new DateTime(reportYear, reportMonth, 1);
            DateTime dtEnd = reportMonth == 12 ? new DateTime(reportYear + 1, 1, 1) : new DateTime(reportYear, reportMonth + 1, 1);
            if (dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 6 && x.agentID == AgentTBL.id && x.dtCreation >= dtStart && x.dtCreation < dtEnd).Count() != 0)
                returnError = 1; // ha delle pren non chiusi
            var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == AgentTBL.id && x.dtCreation >= dtStart && x.dtCreation < dtEnd);
            currTBL.countReservationsTotal = tmpList.Count();
            currTBL.cashReservationsTotal = tmpList.Where(x => x.agentCommissionPrice.HasValue).Count() > 0 ? tmpList.Where(x => x.pr_total.HasValue).Sum(x => x.pr_total.Value) : 0; // totale somma prenotazioni
            foreach (RNT_TBL_RESERVATION tmp in tmpList)
            {
                RNT_TBL_RESERVATION currRes = dcOld.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == tmp.id);
                if (currRes == null) continue;
                rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
                decimal agentTotalResPrice;
                int pidDiscountType = AgentTBL.pidDiscountType.objToInt32();
                if (pidDiscountType == 0) pidDiscountType = 1;
                outPrice.FillFrom(currRes);
                outPrice.agentCommissionPerc = rntUtils.getAgent_discount(AgentTBL.id, tmp.id, pidDiscountType, dtStart, currTBL.cashReservationsTotal.objToDecimal(), 0, out agentTotalResPrice);
                outPrice.agentCommissionPrice = (currRes.pr_total.objToDecimal() * outPrice.agentCommissionPerc / 100);
                rntReservation_onChange(currRes);
            }
            dcOld.SubmitChanges();
            tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID == AgentTBL.id && x.dtCreation >= dtStart && x.dtCreation < dtEnd);
            currTBL.cashCommissionsTotal = tmpList.Where(x => x.agentCommissionPrice.HasValue).Count() > 0 ? tmpList.Where(x => x.agentCommissionPrice.HasValue).Sum(x => x.agentCommissionPrice.Value) : 0; // somma commissioni che devono essere
            currTBL.cashDiscountNotPayed = tmpList.Where(x => x.agentCommissionPrice.HasValue && x.agentDiscountNotPayed == 1).Count() > 0 ? tmpList.Where(x => x.agentCommissionPrice.HasValue && x.agentDiscountNotPayed == 1).Sum(x => x.agentCommissionPrice.Value) : 0; // sconto scalato nel momento di pagamento
            currTBL.cashToPayBack = currTBL.cashCommissionsTotal - currTBL.cashDiscountNotPayed; // da ripagare = somma commissioni che devono essere - sconto scalato nel momento di pagamento
            dcNew.SaveChanges();
        }
        return returnError;
    }
    public static decimal getAgent_discount(long IdAgent, long currResID, int discountType, DateTime checkDate, decimal agentTotalResPrice, decimal addSum, out decimal totalResPrice)
    {
        DateTime dtStart = new DateTime(checkDate.Year, checkDate.Month, 1);
        DateTime dtEnd = checkDate.Month == 12 ? new DateTime(checkDate.Year + 1, 1, 1) : new DateTime(checkDate.Year, checkDate.Month + 1, 1);
        if (agentTotalResPrice != -1)
            totalResPrice = agentTotalResPrice;
        else
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            {
                var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.id != currResID && x.state_pid == 4 && x.agentID == IdAgent && x.dtCreation >= dtStart && x.dtCreation < dtEnd && x.pr_total.HasValue).ToList();
                totalResPrice = tmpList.Count > 0 ? tmpList.Sum(x => x.pr_total.Value) : 0;
            }
        totalResPrice += addSum;
        dbRntDiscountTypeTBL _DiscountType = rntProps.DiscountTypeTBL.SingleOrDefault(x => x.id == discountType);
        if (_DiscountType == null) return 0;
        if (_DiscountType.fase1_end <= 0 || _DiscountType.fase1_end >= totalResPrice)
            return _DiscountType.fase1_discount.objToDecimal();

        if (_DiscountType.fase2_end <= 0 || _DiscountType.fase2_end >= totalResPrice)
            return _DiscountType.fase2_discount.objToDecimal();

        if (_DiscountType.fase3_end <= 0 || _DiscountType.fase3_end >= totalResPrice)
            return _DiscountType.fase3_discount.objToDecimal();

        if (_DiscountType.fase4_end <= 0 || _DiscountType.fase4_end >= totalResPrice)
            return _DiscountType.fase4_discount.objToDecimal();

        if (_DiscountType.fase5_end <= 0 || _DiscountType.fase5_end >= totalResPrice)
            return _DiscountType.fase5_discount.objToDecimal();

        if (_DiscountType.fase6_end <= 0 || _DiscountType.fase6_end >= totalResPrice)
            return _DiscountType.fase6_discount.objToDecimal();

        if (_DiscountType.fase7_end <= 0 || _DiscountType.fase7_end >= totalResPrice)
            return _DiscountType.fase7_discount.objToDecimal();

        if (_DiscountType.fase8_end <= 0 || _DiscountType.fase8_end >= totalResPrice)
            return _DiscountType.fase8_discount.objToDecimal();

        if (_DiscountType.fase9_end <= 0 || _DiscountType.fase9_end >= totalResPrice)
            return _DiscountType.fase9_discount.objToDecimal();

        if (_DiscountType.fase10_end <= 0 || _DiscountType.fase10_end >= totalResPrice)
            return _DiscountType.fase10_discount.objToDecimal();

        return 0;
    }
    public static string getDiscountType_details(int discountType, decimal currSum, DateTime checkDate, int pidLang, string currClass, string uniqueTemplate, string startTemplate, string middleTemplate, string endTemplate, string layoutTemplate)
    {
        string tmp = "" + layoutTemplate;
        string tmpItems = "";
        dbRntDiscountTypeTBL _DiscountType = rntProps.DiscountTypeTBL.SingleOrDefault(x => x.id == discountType);
        if (_DiscountType == null) return tmp;
        tmp = tmp.Replace("#currMonth#", checkDate.formatCustom("#MM# #yy#", pidLang, "- - -"));
        tmp = tmp.Replace("#currSum#", currSum.ToString("N2"));
        tmp = tmp.Replace("#discountList#", getDiscountType_detailsList(_DiscountType, currSum, currClass, uniqueTemplate, startTemplate, middleTemplate, endTemplate));
        return tmp;
    }
    public static string getDiscountType_detailsList(dbRntDiscountTypeTBL _DiscountType, decimal currSum, string currClass, string uniqueTemplate, string startTemplate, string middleTemplate, string endTemplate)
    {
        string tmp = "";
        bool addMoreDiscount = true;

        if (_DiscountType.fase1_end <= 0)
            return uniqueTemplate.Replace("#discount#", _DiscountType.fase1_discount.objToDecimal().ToString("N0")).Replace("#cssClass#", currClass);
        else
            tmp += startTemplate.Replace("#discount#", _DiscountType.fase1_discount.objToDecimal().ToString("N0")).Replace("#end#", _DiscountType.fase1_end.ToString()).Replace("#cssClass#", (_DiscountType.fase1_end >= currSum) ? currClass : "");

        if (_DiscountType.fase2_end <= 0)
        {
            tmp += endTemplate.Replace("#discount#", _DiscountType.fase2_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase2_start.ToString()).Replace("#cssClass#", (_DiscountType.fase2_start <= currSum) ? currClass : "");
            return tmp;
        }
        else
        {
            tmp += middleTemplate.Replace("#discount#", _DiscountType.fase2_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase2_start.ToString()).Replace("#end#", _DiscountType.fase2_end.ToString()).Replace("#cssClass#", (_DiscountType.fase2_start <= currSum && _DiscountType.fase2_end >= currSum) ? currClass : "");
        }

        if (_DiscountType.fase3_end <= 0)
        {
            tmp += endTemplate.Replace("#discount#", _DiscountType.fase3_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase3_start.ToString()).Replace("#cssClass#", (_DiscountType.fase3_start <= currSum) ? currClass : "");
            return tmp;
        }
        else
        {
            tmp += middleTemplate.Replace("#discount#", _DiscountType.fase3_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase3_start.ToString()).Replace("#end#", _DiscountType.fase3_end.ToString()).Replace("#cssClass#", (_DiscountType.fase3_start <= currSum && _DiscountType.fase3_end >= currSum) ? currClass : "");
        }

        if (_DiscountType.fase4_end <= 0)
        {
            tmp += endTemplate.Replace("#discount#", _DiscountType.fase4_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase4_start.ToString()).Replace("#cssClass#", (_DiscountType.fase4_start <= currSum) ? currClass : "");
            return tmp;
        }
        else
        {
            tmp += middleTemplate.Replace("#discount#", _DiscountType.fase4_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase4_start.ToString()).Replace("#end#", _DiscountType.fase4_end.ToString()).Replace("#cssClass#", (_DiscountType.fase4_start <= currSum && _DiscountType.fase4_end >= currSum) ? currClass : "");
        }

        if (_DiscountType.fase5_end <= 0)
        {
            tmp += endTemplate.Replace("#discount#", _DiscountType.fase5_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase5_start.ToString()).Replace("#cssClass#", (_DiscountType.fase5_start <= currSum) ? currClass : "");
            return tmp;
        }
        else
        {
            tmp += middleTemplate.Replace("#discount#", _DiscountType.fase5_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase5_start.ToString()).Replace("#end#", _DiscountType.fase5_end.ToString()).Replace("#cssClass#", (_DiscountType.fase5_start <= currSum && _DiscountType.fase5_end >= currSum) ? currClass : "");
        }

        if (_DiscountType.fase6_end <= 0)
        {
            tmp += endTemplate.Replace("#discount#", _DiscountType.fase6_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase6_start.ToString()).Replace("#cssClass#", (_DiscountType.fase6_start <= currSum) ? currClass : "");
            return tmp;
        }
        else
        {
            tmp += middleTemplate.Replace("#discount#", _DiscountType.fase6_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase6_start.ToString()).Replace("#end#", _DiscountType.fase6_end.ToString()).Replace("#cssClass#", (_DiscountType.fase6_start <= currSum && _DiscountType.fase6_end >= currSum) ? currClass : "");
        }

        if (_DiscountType.fase7_end <= 0)
        {
            tmp += endTemplate.Replace("#discount#", _DiscountType.fase7_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase7_start.ToString()).Replace("#cssClass#", (_DiscountType.fase7_start <= currSum) ? currClass : "");
            return tmp;
        }
        else
        {
            tmp += middleTemplate.Replace("#discount#", _DiscountType.fase7_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase7_start.ToString()).Replace("#end#", _DiscountType.fase7_end.ToString()).Replace("#cssClass#", (_DiscountType.fase7_start <= currSum && _DiscountType.fase7_end >= currSum) ? currClass : "");
        }

        if (_DiscountType.fase8_end <= 0)
        {
            tmp += endTemplate.Replace("#discount#", _DiscountType.fase8_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase8_start.ToString()).Replace("#cssClass#", (_DiscountType.fase8_start <= currSum) ? currClass : "");
            return tmp;
        }
        else
        {
            tmp += middleTemplate.Replace("#discount#", _DiscountType.fase8_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase8_start.ToString()).Replace("#end#", _DiscountType.fase8_end.ToString()).Replace("#cssClass#", (_DiscountType.fase8_start <= currSum && _DiscountType.fase8_end >= currSum) ? currClass : "");
        }

        if (_DiscountType.fase9_end <= 0)
        {
            tmp += endTemplate.Replace("#discount#", _DiscountType.fase9_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase9_start.ToString()).Replace("#cssClass#", (_DiscountType.fase9_start <= currSum) ? currClass : "");
            return tmp;
        }
        else
        {
            tmp += middleTemplate.Replace("#discount#", _DiscountType.fase9_discount.objToDecimal().ToString("N0")).Replace("#start#", _DiscountType.fase9_start.ToString()).Replace("#end#", _DiscountType.fase9_end.ToString()).Replace("#cssClass#", (_DiscountType.fase9_start <= currSum && _DiscountType.fase9_end >= currSum) ? currClass : "");
        }
        return tmp;
    }
    private class agent_mailPwdRecoveryWork
    {
        long IdAgent;
        void doThread()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntAgentTBL currTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == IdAgent);
                if (currTBL == null) return;
                int _lang = currTBL.pidLang.objToInt32();
                if (_lang == 0) _lang = 2;
                string _subject = MailingUtilities.mailTemplate_subject("ag_pwd_recovery", _lang, "Error");
                if (_subject == "Error" && _lang != 2)
                {
                    _lang = 2;
                    _subject = MailingUtilities.mailTemplate_subject("ag_pwd_recovery", _lang, "Error");
                }
                if (_subject == "Error")
                {
                    _lang = 1;
                    _subject = MailingUtilities.mailTemplate_subject("ag_pwd_recovery", _lang, "ag_pwd_recovery");
                }
                string _body = MailingUtilities.mailTemplate_body("ag_pwd_recovery", _lang, "ag_pwd_recovery");
                _body = _body.Replace("#lblAffiliatesArea#", CurrentSource.getSysLangValue("lblAffiliatesArea", _lang));
                _body = _body.Replace("#cl_name_honorific#", currTBL.nameHonor);
                _body = _body.Replace("#cl_name_full#", currTBL.nameFull);
                _body = _body.Replace("#cl_name_company#", currTBL.nameCompany);
                _body = _body.Replace("#cl_login#", "" + currTBL.authUsr);
                _body = _body.Replace("#cl_password#", "" + currTBL.authPwd);
                if (MailingUtilities.autoSendMailTo(_subject, _body, currTBL.contactEmail, false, "rntUtils.agent_mailPwdRecoveryWork"))
                {
                }
            }

        }
        public agent_mailPwdRecoveryWork(long AgentId)
        {
            IdAgent = AgentId;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.agent_mailPwdRecoveryWork agentId:" + AgentId);
            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static bool agent_mailPwdRecovery(long AgentId)
    {
        agent_mailPwdRecoveryWork _tmp = new agent_mailPwdRecoveryWork(AgentId);
        return true;
    }
    private class agent_mailNewCreationWork
    {
        long IdAgent;
        void doThread()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntAgentTBL currTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == IdAgent);
                if (currTBL == null) return;
                int _lang = currTBL.pidLang.objToInt32();
                if (_lang == 0) _lang = 2;
                string _subject = MailingUtilities.mailTemplate_subject("ag_new_created", _lang, "Error");
                if (_subject == "Error" && _lang != 2)
                {
                    _lang = 2;
                    _subject = MailingUtilities.mailTemplate_subject("ag_new_created", _lang, "Error");
                }
                if (_subject == "Error")
                {
                    _lang = 1;
                    _subject = MailingUtilities.mailTemplate_subject("ag_new_created", _lang, "ag_new_created");
                }
                string _body = MailingUtilities.mailTemplate_body("ag_new_created", _lang, "ag_new_created");
                _body = _body.Replace("#lblAffiliatesArea#", CurrentSource.getSysLangValue("lblAffiliatesArea", _lang));
                _body = _body.Replace("#cl_name_honorific#", currTBL.nameHonor);
                _body = _body.Replace("#cl_name_full#", currTBL.nameFull);
                _body = _body.Replace("#cl_name_company#", currTBL.nameCompany);
                _body = _body.Replace("#cl_login#", "" + currTBL.authUsr);
                _body = _body.Replace("#cl_password#", "" + currTBL.authPwd);
                if (MailingUtilities.autoSendMailTo(_subject, _body, currTBL.contactEmail, false, "rntUtils.agent_mailNewCreationWork"))
                {
                    currTBL.mailSentCount = currTBL.mailSentCount.objToInt32() + 1;
                    currTBL.mailSentLast = DateTime.Now;
                    dc.SaveChanges();
                }
            }

        }
        public agent_mailNewCreationWork(long AgentId)
        {
            IdAgent = AgentId;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.agent_mailNewCreationWork agentId:" + AgentId);
            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static bool agent_mailNewCreation(long AgentId)
    {
        agent_mailNewCreationWork _tmp = new agent_mailNewCreationWork(AgentId);
        return true;
    }

    private class estate_addLogWork
    {
        RNT_TB_ESTATE tbBefore;
        RNT_TB_ESTATE tbAfter;
        DateTime logDate;
        int userID;
        string userName;
        int estateID;
        string estateCode;
        void addLog(string changeField, string valueBefore, string valueAfter)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntEstateLOG tmp = new dbRntEstateLOG();
                tmp.uid = Guid.NewGuid();
                tmp.logDate = logDate;
                tmp.userID = userID;
                tmp.userName = userName;
                tmp.estateID = estateID;
                tmp.estateCode = estateCode;
                tmp.changeField = changeField;
                tmp.valueBefore = valueBefore;
                tmp.valueAfter = valueAfter;
                dc.Add(tmp);
                dc.SaveChanges();
            }
        }
        void doThread()
        {

            if (tbBefore.id != tbAfter.id) addLog("id", "" + tbBefore.id, "" + tbAfter.id);
            if (tbBefore.code != tbAfter.code) addLog("code", "" + tbBefore.code, "" + tbAfter.code);
            if (tbBefore.pid_residence != tbAfter.pid_residence) addLog("pid_residence", "" + tbBefore.pid_residence, "" + tbAfter.pid_residence);
            if (tbBefore.category != tbAfter.category) addLog("category", "" + tbBefore.category, "" + tbAfter.category);
            if (tbBefore.pid_type != tbAfter.pid_type) addLog("pid_type", "" + tbBefore.pid_type, "" + tbAfter.pid_type);
            if (tbBefore.pid_category != tbAfter.pid_category) addLog("pid_category", "" + tbBefore.pid_category, "" + tbAfter.pid_category);
            if (tbBefore.pid_owner != tbAfter.pid_owner) addLog("pid_owner", "" + tbBefore.pid_owner, "" + tbAfter.pid_owner);
            if (tbBefore.pid_agent != tbAfter.pid_agent) addLog("pid_agent", "" + tbBefore.pid_agent, "" + tbAfter.pid_agent);
            if (tbBefore.pid_city != tbAfter.pid_city) addLog("pid_city", "" + tbBefore.pid_city, "" + tbAfter.pid_city);
            if (tbBefore.pid_zone != tbAfter.pid_zone) addLog("pid_zone", "" + tbBefore.pid_zone, "" + tbAfter.pid_zone);
            if (tbBefore.loc_zip_code != tbAfter.loc_zip_code) addLog("loc_zip_code", "" + tbBefore.loc_zip_code, "" + tbAfter.loc_zip_code);
            if (tbBefore.loc_address != tbAfter.loc_address) addLog("loc_address", "" + tbBefore.loc_address, "" + tbAfter.loc_address);
            if (tbBefore.loc_inner_bell != tbAfter.loc_inner_bell) addLog("loc_inner_bell", "" + tbBefore.loc_inner_bell, "" + tbAfter.loc_inner_bell);
            if (tbBefore.loc_referer != tbAfter.loc_referer) addLog("loc_referer", "" + tbBefore.loc_referer, "" + tbAfter.loc_referer);
            if (tbBefore.loc_phone_1 != tbAfter.loc_phone_1) addLog("loc_phone_1", "" + tbBefore.loc_phone_1, "" + tbAfter.loc_phone_1);
            if (tbBefore.loc_phone_2 != tbAfter.loc_phone_2) addLog("loc_phone_2", "" + tbBefore.loc_phone_2, "" + tbAfter.loc_phone_2);
            if (tbBefore.mq_inner != tbAfter.mq_inner) addLog("mq_inner", "" + tbBefore.mq_inner, "" + tbAfter.mq_inner);
            if (tbBefore.mq_outer != tbAfter.mq_outer) addLog("mq_outer", "" + tbBefore.mq_outer, "" + tbAfter.mq_outer);
            if (tbBefore.mq_terrace != tbAfter.mq_terrace) addLog("mq_terrace", "" + tbBefore.mq_terrace, "" + tbAfter.mq_terrace);
            if (tbBefore.on_floor != tbAfter.on_floor) addLog("on_floor", "" + tbBefore.on_floor, "" + tbAfter.on_floor);
            if (tbBefore.on_floor_of_total != tbAfter.on_floor_of_total) addLog("on_floor_of_total", "" + tbBefore.on_floor_of_total, "" + tbAfter.on_floor_of_total);
            if (tbBefore.on_levels != tbAfter.on_levels) addLog("on_levels", "" + tbBefore.on_levels, "" + tbAfter.on_levels);
            if (tbBefore.num_bed_single != tbAfter.num_bed_single) addLog("num_bed_single", "" + tbBefore.num_bed_single, "" + tbAfter.num_bed_single);
            if (tbBefore.num_bed_double != tbAfter.num_bed_double) addLog("num_bed_double", "" + tbBefore.num_bed_double, "" + tbAfter.num_bed_double);
            if (tbBefore.num_bed_double_divisible != tbAfter.num_bed_double_divisible) addLog("num_bed_double_divisible", "" + tbBefore.num_bed_double_divisible, "" + tbAfter.num_bed_double_divisible);
            if (tbBefore.num_bed_double_2level != tbAfter.num_bed_double_2level) addLog("num_bed_double_2level", "" + tbBefore.num_bed_double_2level, "" + tbAfter.num_bed_double_2level);
            if (tbBefore.num_sofa_single != tbAfter.num_sofa_single) addLog("num_sofa_single", "" + tbBefore.num_sofa_single, "" + tbAfter.num_sofa_single);
            if (tbBefore.num_sofa_double != tbAfter.num_sofa_double) addLog("num_sofa_double", "" + tbBefore.num_sofa_double, "" + tbAfter.num_sofa_double);
            if (tbBefore.num_persons_adult != tbAfter.num_persons_adult) addLog("num_persons_adult", "" + tbBefore.num_persons_adult, "" + tbAfter.num_persons_adult);
            if (tbBefore.num_persons_child != tbAfter.num_persons_child) addLog("num_persons_child", "" + tbBefore.num_persons_child, "" + tbAfter.num_persons_child);
            if (tbBefore.num_persons_optional != tbAfter.num_persons_optional) addLog("num_persons_optional", "" + tbBefore.num_persons_optional, "" + tbAfter.num_persons_optional);
            if (tbBefore.num_persons_min != tbAfter.num_persons_min) addLog("num_persons_min", "" + tbBefore.num_persons_min, "" + tbAfter.num_persons_min);
            if (tbBefore.num_persons_max != tbAfter.num_persons_max) addLog("num_persons_max", "" + tbBefore.num_persons_max, "" + tbAfter.num_persons_max);
            if (tbBefore.num_rooms_bed != tbAfter.num_rooms_bed) addLog("num_rooms_bed", "" + tbBefore.num_rooms_bed, "" + tbAfter.num_rooms_bed);
            if (tbBefore.num_rooms_bath != tbAfter.num_rooms_bath) addLog("num_rooms_bath", "" + tbBefore.num_rooms_bath, "" + tbAfter.num_rooms_bath);
            if (tbBefore.num_rooms_total != tbAfter.num_rooms_total) addLog("num_rooms_total", "" + tbBefore.num_rooms_total, "" + tbAfter.num_rooms_total);
            if (tbBefore.num_terraces != tbAfter.num_terraces) addLog("num_terraces", "" + tbBefore.num_terraces, "" + tbAfter.num_terraces);
            if (tbBefore.eco_pr_1 != tbAfter.eco_pr_1) addLog("eco_pr_1", "" + tbBefore.eco_pr_1, "" + tbAfter.eco_pr_1);
            if (tbBefore.eco_pr_2 != tbAfter.eco_pr_2) addLog("eco_pr_2", "" + tbBefore.eco_pr_2, "" + tbAfter.eco_pr_2);
            if (tbBefore.eco_time_preview != tbAfter.eco_time_preview) addLog("eco_time_preview", "" + tbBefore.eco_time_preview, "" + tbAfter.eco_time_preview);
            if (tbBefore.eco_notes != tbAfter.eco_notes) addLog("eco_notes", "" + tbBefore.eco_notes, "" + tbAfter.eco_notes);
            if (tbBefore.eco_ext_name_full != tbAfter.eco_ext_name_full) addLog("eco_ext_name_full", "" + tbBefore.eco_ext_name_full, "" + tbAfter.eco_ext_name_full);
            if (tbBefore.eco_ext_email != tbAfter.eco_ext_email) addLog("eco_ext_email", "" + tbBefore.eco_ext_email, "" + tbAfter.eco_ext_email);
            if (tbBefore.eco_ext_phone != tbAfter.eco_ext_phone) addLog("eco_ext_phone", "" + tbBefore.eco_ext_phone, "" + tbAfter.eco_ext_phone);
            if (tbBefore.eco_ext_price != tbAfter.eco_ext_price) addLog("eco_ext_price", "" + tbBefore.eco_ext_price, "" + tbAfter.eco_ext_price);
            if (tbBefore.eco_ext_payInDays != tbAfter.eco_ext_payInDays) addLog("eco_ext_payInDays", "" + tbBefore.eco_ext_payInDays, "" + tbAfter.eco_ext_payInDays);
            if (tbBefore.eco_ext_clientPay != tbAfter.eco_ext_clientPay) addLog("eco_ext_clientPay", "" + tbBefore.eco_ext_clientPay, "" + tbAfter.eco_ext_clientPay);
            if (tbBefore.srs_ext_name_full != tbAfter.srs_ext_name_full) addLog("srs_ext_name_full", "" + tbBefore.srs_ext_name_full, "" + tbAfter.srs_ext_name_full);
            if (tbBefore.srs_ext_email != tbAfter.srs_ext_email) addLog("srs_ext_email", "" + tbBefore.srs_ext_email, "" + tbAfter.srs_ext_email);
            if (tbBefore.srs_ext_phone != tbAfter.srs_ext_phone) addLog("srs_ext_phone", "" + tbBefore.srs_ext_phone, "" + tbAfter.srs_ext_phone);
            if (tbBefore.srs_ext_phone_2 != tbAfter.srs_ext_phone_2) addLog("srs_ext_phone_2", "" + tbBefore.srs_ext_phone_2, "" + tbAfter.srs_ext_phone_2);
            if (tbBefore.srs_ext_phone_3 != tbAfter.srs_ext_phone_3) addLog("srs_ext_phone_3", "" + tbBefore.srs_ext_phone_3, "" + tbAfter.srs_ext_phone_3);
            if (tbBefore.srs_ext_phone_4 != tbAfter.srs_ext_phone_4) addLog("srs_ext_phone_4", "" + tbBefore.srs_ext_phone_4, "" + tbAfter.srs_ext_phone_4);
            if (tbBefore.srs_ext_price != tbAfter.srs_ext_price) addLog("srs_ext_price", "" + tbBefore.srs_ext_price, "" + tbAfter.srs_ext_price);
            if (tbBefore.srs_ext_clientPay != tbAfter.srs_ext_clientPay) addLog("srs_ext_clientPay", "" + tbBefore.srs_ext_clientPay, "" + tbAfter.srs_ext_clientPay);
            if (tbBefore.srs_ext_meetingPoint != tbAfter.srs_ext_meetingPoint) addLog("srs_ext_meetingPoint", "" + tbBefore.srs_ext_meetingPoint, "" + tbAfter.srs_ext_meetingPoint);
            if (tbBefore.pr_basePersons != tbAfter.pr_basePersons) addLog("pr_basePersons", "" + tbBefore.pr_basePersons, "" + tbAfter.pr_basePersons);
            if (tbBefore.pr_1_2pax != tbAfter.pr_1_2pax) addLog("pr_1_2pax", "" + tbBefore.pr_1_2pax, "" + tbAfter.pr_1_2pax);
            if (tbBefore.pr_1_opt != tbAfter.pr_1_opt) addLog("pr_1_opt", "" + tbBefore.pr_1_opt, "" + tbAfter.pr_1_opt);
            if (tbBefore.pr_2_2pax != tbAfter.pr_2_2pax) addLog("pr_2_2pax", "" + tbBefore.pr_2_2pax, "" + tbAfter.pr_2_2pax);
            if (tbBefore.pr_2_opt != tbAfter.pr_2_opt) addLog("pr_2_opt", "" + tbBefore.pr_2_opt, "" + tbAfter.pr_2_opt);
            if (tbBefore.pr_3_2pax != tbAfter.pr_3_2pax) addLog("pr_3_2pax", "" + tbBefore.pr_3_2pax, "" + tbAfter.pr_3_2pax);
            if (tbBefore.pr_3_opt != tbAfter.pr_3_opt) addLog("pr_3_opt", "" + tbBefore.pr_3_opt, "" + tbAfter.pr_3_opt);
            if (tbBefore.pr_tableViewType != tbAfter.pr_tableViewType) addLog("pr_tableViewType", "" + tbBefore.pr_tableViewType, "" + tbAfter.pr_tableViewType);
            if (tbBefore.pr_discount7days != tbAfter.pr_discount7days) addLog("pr_discount7days", "" + tbBefore.pr_discount7days, "" + tbAfter.pr_discount7days);
            if (tbBefore.pr_discount30days != tbAfter.pr_discount30days) addLog("pr_discount30days", "" + tbBefore.pr_discount30days, "" + tbAfter.pr_discount30days);
            if (tbBefore.pr_startDate != tbAfter.pr_startDate) addLog("pr_startDate", "" + tbBefore.pr_startDate, "" + tbAfter.pr_startDate);
            if (tbBefore.pr_dcSUsed != tbAfter.pr_dcSUsed) addLog("pr_dcSUsed", "" + tbBefore.pr_dcSUsed, "" + tbAfter.pr_dcSUsed);
            if (tbBefore.pr_dcS2_1_inDays != tbAfter.pr_dcS2_1_inDays) addLog("pr_dcS2_1_inDays", "" + tbBefore.pr_dcS2_1_inDays, "" + tbAfter.pr_dcS2_1_inDays);
            if (tbBefore.pr_dcS2_1_percent != tbAfter.pr_dcS2_1_percent) addLog("pr_dcS2_1_percent", "" + tbBefore.pr_dcS2_1_percent, "" + tbAfter.pr_dcS2_1_percent);
            if (tbBefore.pr_dcS2_2_inDays != tbAfter.pr_dcS2_2_inDays) addLog("pr_dcS2_2_inDays", "" + tbBefore.pr_dcS2_2_inDays, "" + tbAfter.pr_dcS2_2_inDays);
            if (tbBefore.pr_dcS2_2_percent != tbAfter.pr_dcS2_2_percent) addLog("pr_dcS2_2_percent", "" + tbBefore.pr_dcS2_2_percent, "" + tbAfter.pr_dcS2_2_percent);
            if (tbBefore.pr_dcS2_3_inDays != tbAfter.pr_dcS2_3_inDays) addLog("pr_dcS2_3_inDays", "" + tbBefore.pr_dcS2_3_inDays, "" + tbAfter.pr_dcS2_3_inDays);
            if (tbBefore.pr_dcS2_3_percent != tbAfter.pr_dcS2_3_percent) addLog("pr_dcS2_3_percent", "" + tbBefore.pr_dcS2_3_percent, "" + tbAfter.pr_dcS2_3_percent);
            if (tbBefore.pr_dcS2_4_inDays != tbAfter.pr_dcS2_4_inDays) addLog("pr_dcS2_4_inDays", "" + tbBefore.pr_dcS2_4_inDays, "" + tbAfter.pr_dcS2_4_inDays);
            if (tbBefore.pr_dcS2_4_percent != tbAfter.pr_dcS2_4_percent) addLog("pr_dcS2_4_percent", "" + tbBefore.pr_dcS2_4_percent, "" + tbAfter.pr_dcS2_4_percent);
            if (tbBefore.pr_dcS2_5_inDays != tbAfter.pr_dcS2_5_inDays) addLog("pr_dcS2_5_inDays", "" + tbBefore.pr_dcS2_5_inDays, "" + tbAfter.pr_dcS2_5_inDays);
            if (tbBefore.pr_dcS2_5_percent != tbAfter.pr_dcS2_5_percent) addLog("pr_dcS2_5_percent", "" + tbBefore.pr_dcS2_5_percent, "" + tbAfter.pr_dcS2_5_percent);
            if (tbBefore.pr_dcS2_6_inDays != tbAfter.pr_dcS2_6_inDays) addLog("pr_dcS2_6_inDays", "" + tbBefore.pr_dcS2_6_inDays, "" + tbAfter.pr_dcS2_6_inDays);
            if (tbBefore.pr_dcS2_6_percent != tbAfter.pr_dcS2_6_percent) addLog("pr_dcS2_6_percent", "" + tbBefore.pr_dcS2_6_percent, "" + tbAfter.pr_dcS2_6_percent);
            if (tbBefore.pr_dcS2_7_inDays != tbAfter.pr_dcS2_7_inDays) addLog("pr_dcS2_7_inDays", "" + tbBefore.pr_dcS2_7_inDays, "" + tbAfter.pr_dcS2_7_inDays);
            if (tbBefore.pr_dcS2_7_percent != tbAfter.pr_dcS2_7_percent) addLog("pr_dcS2_7_percent", "" + tbBefore.pr_dcS2_7_percent, "" + tbAfter.pr_dcS2_7_percent);
            if (tbBefore.pr_percentage != tbAfter.pr_percentage) addLog("pr_percentage", "" + tbBefore.pr_percentage, "" + tbAfter.pr_percentage);
            if (tbBefore.pr_deposit != tbAfter.pr_deposit) addLog("pr_deposit", "" + tbBefore.pr_deposit, "" + tbAfter.pr_deposit);
            if (tbBefore.pr_depositWithCard != tbAfter.pr_depositWithCard) addLog("pr_depositWithCard", "" + tbBefore.pr_depositWithCard, "" + tbAfter.pr_depositWithCard);
            if (tbBefore.pr_has_overnight_tax != tbAfter.pr_has_overnight_tax) addLog("pr_has_overnight_tax", "" + tbBefore.pr_has_overnight_tax, "" + tbAfter.pr_has_overnight_tax);
            if (tbBefore.ext_ownerdaysinyear != tbAfter.ext_ownerdaysinyear) addLog("ext_ownerdaysinyear", "" + tbBefore.ext_ownerdaysinyear, "" + tbAfter.ext_ownerdaysinyear);
            if (tbBefore.lm_inhours != tbAfter.lm_inhours) addLog("lm_inhours", "" + tbBefore.lm_inhours, "" + tbAfter.lm_inhours);
            if (tbBefore.lm_discount != tbAfter.lm_discount) addLog("lm_discount", "" + tbBefore.lm_discount, "" + tbAfter.lm_discount);
            if (tbBefore.lm_nights_min != tbAfter.lm_nights_min) addLog("lm_nights_min", "" + tbBefore.lm_nights_min, "" + tbAfter.lm_nights_min);
            if (tbBefore.lm_nights_max != tbAfter.lm_nights_max) addLog("lm_nights_max", "" + tbBefore.lm_nights_max, "" + tbAfter.lm_nights_max);
            if (tbBefore.lpb_is != tbAfter.lpb_is) addLog("lpb_is", "" + tbBefore.lpb_is, "" + tbAfter.lpb_is);
            if (tbBefore.lpb_nights_min != tbAfter.lpb_nights_min) addLog("lpb_nights_min", "" + tbBefore.lpb_nights_min, "" + tbAfter.lpb_nights_min);
            if (tbBefore.lpb_afterdays != tbAfter.lpb_afterdays) addLog("lpb_afterdays", "" + tbBefore.lpb_afterdays, "" + tbAfter.lpb_afterdays);
            if (tbBefore.lpb_onlyhighseason != tbAfter.lpb_onlyhighseason) addLog("lpb_onlyhighseason", "" + tbBefore.lpb_onlyhighseason, "" + tbAfter.lpb_onlyhighseason);
            if (tbBefore.nights_minVHSeason != tbAfter.nights_minVHSeason) addLog("nights_minVHSeason", "" + tbBefore.nights_minVHSeason, "" + tbAfter.nights_minVHSeason);
            if (tbBefore.nights_min != tbAfter.nights_min) addLog("nights_min", "" + tbBefore.nights_min, "" + tbAfter.nights_min);
            if (tbBefore.nights_max != tbAfter.nights_max) addLog("nights_max", "" + tbBefore.nights_max, "" + tbAfter.nights_max);
            if (tbBefore.longTermRent != tbAfter.longTermRent) addLog("longTermRent", "" + tbBefore.longTermRent, "" + tbAfter.longTermRent);
            if (tbBefore.longTermPrMonthly != tbAfter.longTermPrMonthly) addLog("longTermPrMonthly", "" + tbBefore.longTermPrMonthly, "" + tbAfter.longTermPrMonthly);
            if (tbBefore.importance != tbAfter.importance) addLog("importance", "" + tbBefore.importance, "" + tbAfter.importance);
            if (tbBefore.importance_vote != tbAfter.importance_vote) addLog("importance_vote", "" + tbBefore.importance_vote, "" + tbAfter.importance_vote);
            if (tbBefore.importance_stars != tbAfter.importance_stars) addLog("importance_stars", "" + tbBefore.importance_stars, "" + tbAfter.importance_stars);
            if (tbBefore.media_folder != tbAfter.media_folder) addLog("media_folder", "" + tbBefore.media_folder, "" + tbAfter.media_folder);
            if (tbBefore.img_thumb != tbAfter.img_thumb) addLog("img_thumb", "" + tbBefore.img_thumb, "" + tbAfter.img_thumb);
            if (tbBefore.img_preview_1 != tbAfter.img_preview_1) addLog("img_preview_1", "" + tbBefore.img_preview_1, "" + tbAfter.img_preview_1);
            if (tbBefore.img_preview_2 != tbAfter.img_preview_2) addLog("img_preview_2", "" + tbBefore.img_preview_2, "" + tbAfter.img_preview_2);
            if (tbBefore.img_preview_3 != tbAfter.img_preview_3) addLog("img_preview_3", "" + tbBefore.img_preview_3, "" + tbAfter.img_preview_3);
            if (tbBefore.img_banner != tbAfter.img_banner) addLog("img_banner", "" + tbBefore.img_banner, "" + tbAfter.img_banner);
            if (tbBefore.inner_notes != tbAfter.inner_notes) addLog("inner_notes", "" + tbBefore.inner_notes, "" + tbAfter.inner_notes);
            if (tbBefore.sv_yaw != tbAfter.sv_yaw) addLog("sv_yaw", "" + tbBefore.sv_yaw, "" + tbAfter.sv_yaw);
            if (tbBefore.sv_pitch != tbAfter.sv_pitch) addLog("sv_pitch", "" + tbBefore.sv_pitch, "" + tbAfter.sv_pitch);
            if (tbBefore.sv_zoom != tbAfter.sv_zoom) addLog("sv_zoom", "" + tbBefore.sv_zoom, "" + tbAfter.sv_zoom);
            if (tbBefore.sv_coords != tbAfter.sv_coords) addLog("sv_coords", "" + tbBefore.sv_coords, "" + tbAfter.sv_coords);
            if (tbBefore.is_street_view != tbAfter.is_street_view) addLog("is_street_view", "" + tbBefore.is_street_view, "" + tbAfter.is_street_view);
            if (tbBefore.google_maps != tbAfter.google_maps) addLog("google_maps", "" + tbBefore.google_maps, "" + tbAfter.google_maps);
            if (tbBefore.is_google_maps != tbAfter.is_google_maps) addLog("is_google_maps", "" + tbBefore.is_google_maps, "" + tbAfter.is_google_maps);
            if (tbBefore.is_loft != tbAfter.is_loft) addLog("is_loft", "" + tbBefore.is_loft, "" + tbAfter.is_loft);
            if (tbBefore.is_exclusive != tbAfter.is_exclusive) addLog("is_exclusive", "" + tbBefore.is_exclusive, "" + tbAfter.is_exclusive);
            if (tbBefore.is_srs != tbAfter.is_srs) addLog("is_srs", "" + tbBefore.is_srs, "" + tbAfter.is_srs);
            if (tbBefore.is_ecopulizie != tbAfter.is_ecopulizie) addLog("is_ecopulizie", "" + tbBefore.is_ecopulizie, "" + tbAfter.is_ecopulizie);
            if (tbBefore.is_online_booking != tbAfter.is_online_booking) addLog("is_online_booking", "" + tbBefore.is_online_booking, "" + tbAfter.is_online_booking);
            if (tbBefore.is_active != tbAfter.is_active) addLog("is_active", "" + tbBefore.is_active, "" + tbAfter.is_active);
            if (tbBefore.is_deleted != tbAfter.is_deleted) addLog("is_deleted", "" + tbBefore.is_deleted, "" + tbAfter.is_deleted);
            if (tbBefore.sequence != tbAfter.sequence) addLog("sequence", "" + tbBefore.sequence, "" + tbAfter.sequence);
            if (tbBefore.zoneSequence != tbAfter.zoneSequence) addLog("zoneSequence", "" + tbBefore.zoneSequence, "" + tbAfter.zoneSequence);
            if (tbBefore.ext_gps_id != tbAfter.ext_gps_id) addLog("ext_gps_id", "" + tbBefore.ext_gps_id, "" + tbAfter.ext_gps_id);
            if (tbBefore.ext_gps_pid_maga != tbAfter.ext_gps_pid_maga) addLog("ext_gps_pid_maga", "" + tbBefore.ext_gps_pid_maga, "" + tbAfter.ext_gps_pid_maga);
            if (tbBefore.in_attention != tbAfter.in_attention) addLog("in_attention", "" + tbBefore.in_attention, "" + tbAfter.in_attention);

        }
        public estate_addLogWork(RNT_TB_ESTATE Before, RNT_TB_ESTATE After, int UserID, string UserName)
        {
            tbBefore = Before;
            tbAfter = After;
            logDate = DateTime.Now;
            userID = UserID;
            userName = UserName;
            estateID = tbBefore.id;
            estateCode = tbBefore.code;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.estate_addLogWork idEstate:" + estateID);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }
    public static bool estate_addLog(RNT_TB_ESTATE Before, RNT_TB_ESTATE After, int UserID, string UserName)
    {
        estate_addLogWork _tmp = new estate_addLogWork(Before, After, UserID, UserName);
        return true;
    }
    public static bool RNT_TBL_RESERVATION_addLog(RNT_TBL_RESERVATION Before, RNT_TBL_RESERVATION After, int UserID, string UserName)
    {
        RNT_TBL_RESERVATION_addLogWork _tmp = new RNT_TBL_RESERVATION_addLogWork(Before, After, UserID, UserName);
        return true;
    }
    public class RNT_TBL_RESERVATION_addLogWork
    {
        RNT_TBL_RESERVATION tbBefore;
        RNT_TBL_RESERVATION tbAfter;
        DateTime logDate;
        int userID;
        string userName;
        long recordID;
        string recordCode;
        void addLog(string changeField, string valueBefore, string valueAfter)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntReservationLOG tmp = new dbRntReservationLOG();
                tmp.uid = Guid.NewGuid();
                tmp.logDate = logDate;
                tmp.userID = userID;
                tmp.userName = userName;
                tmp.resID = recordID;
                tmp.resCode = recordCode;
                tmp.changeField = changeField;
                tmp.valueBefore = valueBefore;
                tmp.valueAfter = valueAfter;
                dc.Add(tmp);
                dc.SaveChanges();
            }
        }
        void doThread()
        {
            if (tbBefore.id != tbAfter.id) addLog("id", "" + tbBefore.id, "" + tbAfter.id);
            if (tbBefore.unique_id != tbAfter.unique_id) addLog("unique_id", "" + tbBefore.unique_id, "" + tbAfter.unique_id);
            if (tbBefore.uid_2 != tbAfter.uid_2) addLog("uid_2", "" + tbBefore.uid_2, "" + tbAfter.uid_2);
            if (tbBefore.code != tbAfter.code) addLog("code", "" + tbBefore.code, "" + tbAfter.code);
            if (tbBefore.password != tbAfter.password) addLog("password", "" + tbBefore.password, "" + tbAfter.password);
            if (tbBefore.pid_related_request != tbAfter.pid_related_request) addLog("pid_related_request", "" + tbBefore.pid_related_request, "" + tbAfter.pid_related_request);
            if (tbBefore.pid_creator != tbAfter.pid_creator) addLog("pid_creator", "" + tbBefore.pid_creator, "" + tbAfter.pid_creator);
            if (tbBefore.pid_operator != tbAfter.pid_operator) addLog("pid_operator", "" + tbBefore.pid_operator, "" + tbAfter.pid_operator);
            if (tbBefore.operator_date != tbAfter.operator_date) addLog("operator_date", "" + tbBefore.operator_date, "" + tbAfter.operator_date);
            if (tbBefore.pid_estate != tbAfter.pid_estate) addLog("pid_estate", "" + tbBefore.pid_estate, "" + tbAfter.pid_estate);
            if (tbBefore.pidEstateCity != tbAfter.pidEstateCity) addLog("pidEstateCity", "" + tbBefore.pidEstateCity, "" + tbAfter.pidEstateCity);
            if (tbBefore.cl_id != tbAfter.cl_id) addLog("cl_id", "" + tbBefore.cl_id, "" + tbAfter.cl_id);
            if (tbBefore.cl_pid_discount != tbAfter.cl_pid_discount) addLog("cl_pid_discount", "" + tbBefore.cl_pid_discount, "" + tbAfter.cl_pid_discount);
            if (tbBefore.cl_pid_lang != tbAfter.cl_pid_lang) addLog("cl_pid_lang", "" + tbBefore.cl_pid_lang, "" + tbAfter.cl_pid_lang);
            if (tbBefore.cl_email != tbAfter.cl_email) addLog("cl_email", "" + tbBefore.cl_email, "" + tbAfter.cl_email);
            if (tbBefore.cl_name_honorific != tbAfter.cl_name_honorific) addLog("cl_name_honorific", "" + tbBefore.cl_name_honorific, "" + tbAfter.cl_name_honorific);
            if (tbBefore.cl_name_full != tbAfter.cl_name_full) addLog("cl_name_full", "" + tbBefore.cl_name_full, "" + tbAfter.cl_name_full);
            if (tbBefore.cl_loc_country != tbAfter.cl_loc_country) addLog("cl_loc_country", "" + tbBefore.cl_loc_country, "" + tbAfter.cl_loc_country);
            if (tbBefore.cl_isCompleted != tbAfter.cl_isCompleted) addLog("cl_isCompleted", "" + tbBefore.cl_isCompleted, "" + tbAfter.cl_isCompleted);
            if (tbBefore.cl_reminderLast != tbAfter.cl_reminderLast) addLog("cl_reminderLast", "" + tbBefore.cl_reminderLast, "" + tbAfter.cl_reminderLast);
            if (tbBefore.cl_reminderNext != tbAfter.cl_reminderNext) addLog("cl_reminderNext", "" + tbBefore.cl_reminderNext, "" + tbAfter.cl_reminderNext);
            if (tbBefore.cl_reminderCount != tbAfter.cl_reminderCount) addLog("cl_reminderCount", "" + tbBefore.cl_reminderCount, "" + tbAfter.cl_reminderCount);
            if (tbBefore.cl_browserInfo != tbAfter.cl_browserInfo) addLog("cl_browserInfo", "" + tbBefore.cl_browserInfo, "" + tbAfter.cl_browserInfo);
            if (tbBefore.cl_browserIP != tbAfter.cl_browserIP) addLog("cl_browserIP", "" + tbBefore.cl_browserIP, "" + tbAfter.cl_browserIP);
            if (tbBefore.num_adult != tbAfter.num_adult) addLog("num_adult", "" + tbBefore.num_adult, "" + tbAfter.num_adult);
            if (tbBefore.num_child_over != tbAfter.num_child_over) addLog("num_child_over", "" + tbBefore.num_child_over, "" + tbAfter.num_child_over);
            if (tbBefore.num_child_min != tbAfter.num_child_min) addLog("num_child_min", "" + tbBefore.num_child_min, "" + tbAfter.num_child_min);
            if (tbBefore.visa_isRequested != tbAfter.visa_isRequested) addLog("visa_isRequested", "" + tbBefore.visa_isRequested, "" + tbAfter.visa_isRequested);
            if (tbBefore.visa_persons != tbAfter.visa_persons) addLog("visa_persons", "" + tbBefore.visa_persons, "" + tbAfter.visa_persons);
            if (tbBefore.dtStart != tbAfter.dtStart) addLog("dtStart", "" + tbBefore.dtStart, "" + tbAfter.dtStart);
            if (tbBefore.dtEnd != tbAfter.dtEnd) addLog("dtEnd", "" + tbBefore.dtEnd, "" + tbAfter.dtEnd);
            if (tbBefore.dtIn != tbAfter.dtIn) addLog("dtIn", "" + tbBefore.dtIn, "" + tbAfter.dtIn);
            if (tbBefore.dtOut != tbAfter.dtOut) addLog("dtOut", "" + tbBefore.dtOut, "" + tbAfter.dtOut);
            if (tbBefore.dtStartTime != tbAfter.dtStartTime) addLog("dtStartTime", "" + tbBefore.dtStartTime, "" + tbAfter.dtStartTime);
            if (tbBefore.dtEndTime != tbAfter.dtEndTime) addLog("dtEndTime", "" + tbBefore.dtEndTime, "" + tbAfter.dtEndTime);
            if (tbBefore.is_dtStartTimeChanged != tbAfter.is_dtStartTimeChanged) addLog("is_dtStartTimeChanged", "" + tbBefore.is_dtStartTimeChanged, "" + tbAfter.is_dtStartTimeChanged);
            if (tbBefore.is_dtEndTimeChanged != tbAfter.is_dtEndTimeChanged) addLog("is_dtEndTimeChanged", "" + tbBefore.is_dtEndTimeChanged, "" + tbAfter.is_dtEndTimeChanged);
            if (tbBefore.limo_easyShuttleID != tbAfter.limo_easyShuttleID) addLog("limo_easyShuttleID", "" + tbBefore.limo_easyShuttleID, "" + tbAfter.limo_easyShuttleID);
            if (tbBefore.limo_easyShuttleInUID != tbAfter.limo_easyShuttleInUID) addLog("limo_easyShuttleInUID", "" + tbBefore.limo_easyShuttleInUID, "" + tbAfter.limo_easyShuttleInUID);
            if (tbBefore.limo_easyShuttleOutUID != tbAfter.limo_easyShuttleOutUID) addLog("limo_easyShuttleOutUID", "" + tbBefore.limo_easyShuttleOutUID, "" + tbAfter.limo_easyShuttleOutUID);
            if (tbBefore.limo_in_datetime != tbAfter.limo_in_datetime) addLog("limo_in_datetime", "" + tbBefore.limo_in_datetime, "" + tbAfter.limo_in_datetime);
            if (tbBefore.limo_in_isRequested != tbAfter.limo_in_isRequested) addLog("limo_in_isRequested", "" + tbBefore.limo_in_isRequested, "" + tbAfter.limo_in_isRequested);
            if (tbBefore.limo_inPoint_type != tbAfter.limo_inPoint_type) addLog("limo_inPoint_type", "" + tbBefore.limo_inPoint_type, "" + tbAfter.limo_inPoint_type);
            if (tbBefore.limo_inPoint_transportType != tbAfter.limo_inPoint_transportType) addLog("limo_inPoint_transportType", "" + tbBefore.limo_inPoint_transportType, "" + tbAfter.limo_inPoint_transportType);
            if (tbBefore.limo_inPoint_pickupPlace != tbAfter.limo_inPoint_pickupPlace) addLog("limo_inPoint_pickupPlace", "" + tbBefore.limo_inPoint_pickupPlace, "" + tbAfter.limo_inPoint_pickupPlace);
            if (tbBefore.limo_inPoint_pickupPlaceName != tbAfter.limo_inPoint_pickupPlaceName) addLog("limo_inPoint_pickupPlaceName", "" + tbBefore.limo_inPoint_pickupPlaceName, "" + tbAfter.limo_inPoint_pickupPlaceName);
            if (tbBefore.limo_inPoint_details != tbAfter.limo_inPoint_details) addLog("limo_inPoint_details", "" + tbBefore.limo_inPoint_details, "" + tbAfter.limo_inPoint_details);
            if (tbBefore.limo_inPoint_detailsType != tbAfter.limo_inPoint_detailsType) addLog("limo_inPoint_detailsType", "" + tbBefore.limo_inPoint_detailsType, "" + tbAfter.limo_inPoint_detailsType);
            if (tbBefore.limo_out_datetime != tbAfter.limo_out_datetime) addLog("limo_out_datetime", "" + tbBefore.limo_out_datetime, "" + tbAfter.limo_out_datetime);
            if (tbBefore.limo_out_isRequested != tbAfter.limo_out_isRequested) addLog("limo_out_isRequested", "" + tbBefore.limo_out_isRequested, "" + tbAfter.limo_out_isRequested);
            if (tbBefore.limo_outPoint_type != tbAfter.limo_outPoint_type) addLog("limo_outPoint_type", "" + tbBefore.limo_outPoint_type, "" + tbAfter.limo_outPoint_type);
            if (tbBefore.limo_outPoint_transportType != tbAfter.limo_outPoint_transportType) addLog("limo_outPoint_transportType", "" + tbBefore.limo_outPoint_transportType, "" + tbAfter.limo_outPoint_transportType);
            if (tbBefore.limo_outPoint_pickupPlace != tbAfter.limo_outPoint_pickupPlace) addLog("limo_outPoint_pickupPlace", "" + tbBefore.limo_outPoint_pickupPlace, "" + tbAfter.limo_outPoint_pickupPlace);
            if (tbBefore.limo_outPoint_pickupPlaceName != tbAfter.limo_outPoint_pickupPlaceName) addLog("limo_outPoint_pickupPlaceName", "" + tbBefore.limo_outPoint_pickupPlaceName, "" + tbAfter.limo_outPoint_pickupPlaceName);
            if (tbBefore.limo_outPoint_details != tbAfter.limo_outPoint_details) addLog("limo_outPoint_details", "" + tbBefore.limo_outPoint_details, "" + tbAfter.limo_outPoint_details);
            if (tbBefore.limo_outPoint_detailsType != tbAfter.limo_outPoint_detailsType) addLog("limo_outPoint_detailsType", "" + tbBefore.limo_outPoint_detailsType, "" + tbAfter.limo_outPoint_detailsType);
            if (tbBefore.limo_num_case_s != tbAfter.limo_num_case_s) addLog("limo_num_case_s", "" + tbBefore.limo_num_case_s, "" + tbAfter.limo_num_case_s);
            if (tbBefore.limo_num_case_m != tbAfter.limo_num_case_m) addLog("limo_num_case_m", "" + tbBefore.limo_num_case_m, "" + tbAfter.limo_num_case_m);
            if (tbBefore.limo_num_case_l != tbAfter.limo_num_case_l) addLog("limo_num_case_l", "" + tbBefore.limo_num_case_l, "" + tbAfter.limo_num_case_l);
            if (tbBefore.limo_request != tbAfter.limo_request) addLog("limo_request", "" + tbBefore.limo_request, "" + tbAfter.limo_request);
            if (tbBefore.limo_isCompleted != tbAfter.limo_isCompleted) addLog("limo_isCompleted", "" + tbBefore.limo_isCompleted, "" + tbAfter.limo_isCompleted);
            if (tbBefore.state_pid != tbAfter.state_pid) addLog("state_pid", "" + tbBefore.state_pid, "" + tbAfter.state_pid);
            if (tbBefore.state_date != tbAfter.state_date) addLog("state_date", "" + tbBefore.state_date, "" + tbAfter.state_date);
            if (tbBefore.state_pid_user != tbAfter.state_pid_user) addLog("state_pid_user", "" + tbBefore.state_pid_user, "" + tbAfter.state_pid_user);
            if (tbBefore.state_subject != tbAfter.state_subject) addLog("state_subject", "" + tbBefore.state_subject, "" + tbAfter.state_subject);
            if (tbBefore.state_body != tbAfter.state_body) addLog("state_body", "" + tbBefore.state_body, "" + tbAfter.state_body);
            if (tbBefore.dtCreation != tbAfter.dtCreation) addLog("dtCreation", "" + tbBefore.dtCreation, "" + tbAfter.dtCreation);
            if (tbBefore.notesInner != tbAfter.notesInner) addLog("notesInner", "" + tbBefore.notesInner, "" + tbAfter.notesInner);
            if (tbBefore.notesClient != tbAfter.notesClient) addLog("notesClient", "" + tbBefore.notesClient, "" + tbAfter.notesClient);
            if (tbBefore.notesEco != tbAfter.notesEco) addLog("notesEco", "" + tbBefore.notesEco, "" + tbAfter.notesEco);
            if (tbBefore.block_expire != tbAfter.block_expire) addLog("block_expire", "" + tbBefore.block_expire, "" + tbAfter.block_expire);
            if (tbBefore.block_expire_hours != tbAfter.block_expire_hours) addLog("block_expire_hours", "" + tbBefore.block_expire_hours, "" + tbAfter.block_expire_hours);
            if (tbBefore.block_pid_user != tbAfter.block_pid_user) addLog("block_pid_user", "" + tbBefore.block_pid_user, "" + tbAfter.block_pid_user);
            if (tbBefore.block_comments != tbAfter.block_comments) addLog("block_comments", "" + tbBefore.block_comments, "" + tbAfter.block_comments);
            if (tbBefore.srs_ext_meetingPoint != tbAfter.srs_ext_meetingPoint) addLog("srs_ext_meetingPoint", "" + tbBefore.srs_ext_meetingPoint, "" + tbAfter.srs_ext_meetingPoint);
            if (tbBefore.pr_deposit_modified != tbAfter.pr_deposit_modified) addLog("pr_deposit_modified", "" + tbBefore.pr_deposit_modified, "" + tbAfter.pr_deposit_modified);
            if (tbBefore.pr_deposit != tbAfter.pr_deposit) addLog("pr_deposit", "" + tbBefore.pr_deposit, "" + tbAfter.pr_deposit);
            if (tbBefore.pr_depositNotes != tbAfter.pr_depositNotes) addLog("pr_depositNotes", "" + tbBefore.pr_depositNotes, "" + tbAfter.pr_depositNotes);
            if (tbBefore.pr_depositWithCard != tbAfter.pr_depositWithCard) addLog("pr_depositWithCard", "" + tbBefore.pr_depositWithCard, "" + tbAfter.pr_depositWithCard);
            if (tbBefore.pr_total_tf != tbAfter.pr_total_tf) addLog("pr_total_tf", "" + tbBefore.pr_total_tf, "" + tbAfter.pr_total_tf);
            if (tbBefore.pr_total_tax != tbAfter.pr_total_tax) addLog("pr_total_tax", "" + tbBefore.pr_total_tax, "" + tbAfter.pr_total_tax);
            if (tbBefore.pr_total_tax_id != tbAfter.pr_total_tax_id) addLog("pr_total_tax_id", "" + tbBefore.pr_total_tax_id, "" + tbAfter.pr_total_tax_id);
            if (tbBefore.pr_total_margin != tbAfter.pr_total_margin) addLog("pr_total_margin", "" + tbBefore.pr_total_margin, "" + tbAfter.pr_total_margin);
            if (tbBefore.pr_total != tbAfter.pr_total) addLog("pr_total", "" + tbBefore.pr_total, "" + tbAfter.pr_total);
            if (tbBefore.pr_total_desc != tbAfter.pr_total_desc) addLog("pr_total_desc", "" + tbBefore.pr_total_desc, "" + tbAfter.pr_total_desc);
            if (tbBefore.pr_discount_owner != tbAfter.pr_discount_owner) addLog("pr_discount_owner", "" + tbBefore.pr_discount_owner, "" + tbAfter.pr_discount_owner);
            if (tbBefore.pr_discount_commission != tbAfter.pr_discount_commission) addLog("pr_discount_commission", "" + tbBefore.pr_discount_commission, "" + tbAfter.pr_discount_commission);
            if (tbBefore.pr_discount_desc != tbAfter.pr_discount_desc) addLog("pr_discount_desc", "" + tbBefore.pr_discount_desc, "" + tbAfter.pr_discount_desc);
            if (tbBefore.pr_discount_user != tbAfter.pr_discount_user) addLog("pr_discount_user", "" + tbBefore.pr_discount_user, "" + tbAfter.pr_discount_user);
            if (tbBefore.pr_discount_custom != tbAfter.pr_discount_custom) addLog("pr_discount_custom", "" + tbBefore.pr_discount_custom, "" + tbAfter.pr_discount_custom);
            if (tbBefore.pr_reservation != tbAfter.pr_reservation) addLog("pr_reservation", "" + tbBefore.pr_reservation, "" + tbAfter.pr_reservation);
            if (tbBefore.pr_part_modified != tbAfter.pr_part_modified) addLog("pr_part_modified", "" + tbBefore.pr_part_modified, "" + tbAfter.pr_part_modified);
            if (tbBefore.pr_part_modify_notes != tbAfter.pr_part_modify_notes) addLog("pr_part_modify_notes", "" + tbBefore.pr_part_modify_notes, "" + tbAfter.pr_part_modify_notes);
            if (tbBefore.pr_part_commission_tf != tbAfter.pr_part_commission_tf) addLog("pr_part_commission_tf", "" + tbBefore.pr_part_commission_tf, "" + tbAfter.pr_part_commission_tf);
            if (tbBefore.pr_part_commission_total != tbAfter.pr_part_commission_total) addLog("pr_part_commission_total", "" + tbBefore.pr_part_commission_total, "" + tbAfter.pr_part_commission_total);
            if (tbBefore.pr_part_agency_fee != tbAfter.pr_part_agency_fee) addLog("pr_part_agency_fee", "" + tbBefore.pr_part_agency_fee, "" + tbAfter.pr_part_agency_fee);
            if (tbBefore.pr_part_payment_total != tbAfter.pr_part_payment_total) addLog("pr_part_payment_total", "" + tbBefore.pr_part_payment_total, "" + tbAfter.pr_part_payment_total);
            if (tbBefore.pr_part_forPayment != tbAfter.pr_part_forPayment) addLog("pr_part_forPayment", "" + tbBefore.pr_part_forPayment, "" + tbAfter.pr_part_forPayment);
            if (tbBefore.pr_part_owner != tbAfter.pr_part_owner) addLog("pr_part_owner", "" + tbBefore.pr_part_owner, "" + tbAfter.pr_part_owner);
            if (tbBefore.pr_ecoPrice != tbAfter.pr_ecoPrice) addLog("pr_ecoPrice", "" + tbBefore.pr_ecoPrice, "" + tbAfter.pr_ecoPrice);
            if (tbBefore.pr_ecoCount != tbAfter.pr_ecoCount) addLog("pr_ecoCount", "" + tbBefore.pr_ecoCount, "" + tbAfter.pr_ecoCount);
            if (tbBefore.pr_srsPrice != tbAfter.pr_srsPrice) addLog("pr_srsPrice", "" + tbBefore.pr_srsPrice, "" + tbAfter.pr_srsPrice);
            if (tbBefore.payed_total != tbAfter.payed_total) addLog("payed_total", "" + tbBefore.payed_total, "" + tbAfter.payed_total);
            if (tbBefore.payed_part != tbAfter.payed_part) addLog("payed_part", "" + tbBefore.payed_part, "" + tbAfter.payed_part);
            if (tbBefore.payed_date != tbAfter.payed_date) addLog("payed_date", "" + tbBefore.payed_date, "" + tbAfter.payed_date);
            if (tbBefore.payed_user != tbAfter.payed_user) addLog("payed_user", "" + tbBefore.payed_user, "" + tbAfter.payed_user);
            if (tbBefore.payed_mode != tbAfter.payed_mode) addLog("payed_mode", "" + tbBefore.payed_mode, "" + tbAfter.payed_mode);
            if (tbBefore.requestRenewal != tbAfter.requestRenewal) addLog("requestRenewal", "" + tbBefore.requestRenewal, "" + tbAfter.requestRenewal);
            if (tbBefore.requestRenewalDate != tbAfter.requestRenewalDate) addLog("requestRenewalDate", "" + tbBefore.requestRenewalDate, "" + tbAfter.requestRenewalDate);
            if (tbBefore.requestFullPay != tbAfter.requestFullPay) addLog("requestFullPay", "" + tbBefore.requestFullPay, "" + tbAfter.requestFullPay);
            if (tbBefore.requestFullPayDate != tbAfter.requestFullPayDate) addLog("requestFullPayDate", "" + tbBefore.requestFullPayDate, "" + tbAfter.requestFullPayDate);
            if (tbBefore.requestFullPayAccepted != tbAfter.requestFullPayAccepted) addLog("requestFullPayAccepted", "" + tbBefore.requestFullPayAccepted, "" + tbAfter.requestFullPayAccepted);
            if (tbBefore.requestFullPayAcceptedDate != tbAfter.requestFullPayAcceptedDate) addLog("requestFullPayAcceptedDate", "" + tbBefore.requestFullPayAcceptedDate, "" + tbAfter.requestFullPayAcceptedDate);
            if (tbBefore.is_deleted != tbAfter.is_deleted) addLog("is_deleted", "" + tbBefore.is_deleted, "" + tbAfter.is_deleted);
            if (tbBefore.is_booking != tbAfter.is_booking) addLog("is_booking", "" + tbBefore.is_booking, "" + tbAfter.is_booking);
            if (tbBefore.inv_toCreate != tbAfter.inv_toCreate) addLog("inv_toCreate", "" + tbBefore.inv_toCreate, "" + tbAfter.inv_toCreate);
            if (tbBefore.bedSingle != tbAfter.bedSingle) addLog("bedSingle", "" + tbBefore.bedSingle, "" + tbAfter.bedSingle);
            if (tbBefore.bedDouble != tbAfter.bedDouble) addLog("bedDouble", "" + tbBefore.bedDouble, "" + tbAfter.bedDouble);
            if (tbBefore.bedDoubleD != tbAfter.bedDoubleD) addLog("bedDoubleD", "" + tbBefore.bedDoubleD, "" + tbAfter.bedDoubleD);
            if (tbBefore.bedDoubleDConfig != tbAfter.bedDoubleDConfig) addLog("bedDoubleDConfig", "" + tbBefore.bedDoubleDConfig, "" + tbAfter.bedDoubleDConfig);
            if (tbBefore.bedDouble2level != tbAfter.bedDouble2level) addLog("bedDouble2level", "" + tbBefore.bedDouble2level, "" + tbAfter.bedDouble2level);
            if (tbBefore.bedDouble2levelConfig != tbAfter.bedDouble2levelConfig) addLog("bedDouble2levelConfig", "" + tbBefore.bedDouble2levelConfig, "" + tbAfter.bedDouble2levelConfig);
            if (tbBefore.bedSofaSingle != tbAfter.bedSofaSingle) addLog("bedSofaSingle", "" + tbBefore.bedSofaSingle, "" + tbAfter.bedSofaSingle);
            if (tbBefore.bedSofaDouble != tbAfter.bedSofaDouble) addLog("bedSofaDouble", "" + tbBefore.bedSofaDouble, "" + tbAfter.bedSofaDouble);
            if (tbBefore.inv_isDifferent != tbAfter.inv_isDifferent) addLog("inv_isDifferent", "" + tbBefore.inv_isDifferent, "" + tbAfter.inv_isDifferent);
            if (tbBefore.inv_name_honorific != tbAfter.inv_name_honorific) addLog("inv_name_honorific", "" + tbBefore.inv_name_honorific, "" + tbAfter.inv_name_honorific);
            if (tbBefore.inv_name_full != tbAfter.inv_name_full) addLog("inv_name_full", "" + tbBefore.inv_name_full, "" + tbAfter.inv_name_full);
            if (tbBefore.inv_loc_country != tbAfter.inv_loc_country) addLog("inv_loc_country", "" + tbBefore.inv_loc_country, "" + tbAfter.inv_loc_country);
            if (tbBefore.inv_loc_state != tbAfter.inv_loc_state) addLog("inv_loc_state", "" + tbBefore.inv_loc_state, "" + tbAfter.inv_loc_state);
            if (tbBefore.inv_loc_city != tbAfter.inv_loc_city) addLog("inv_loc_city", "" + tbBefore.inv_loc_city, "" + tbAfter.inv_loc_city);
            if (tbBefore.inv_loc_address != tbAfter.inv_loc_address) addLog("inv_loc_address", "" + tbBefore.inv_loc_address, "" + tbAfter.inv_loc_address);
            if (tbBefore.inv_loc_zip_code != tbAfter.inv_loc_zip_code) addLog("inv_loc_zip_code", "" + tbBefore.inv_loc_zip_code, "" + tbAfter.inv_loc_zip_code);
            if (tbBefore.inv_doc_vat_num != tbAfter.inv_doc_vat_num) addLog("inv_doc_vat_num", "" + tbBefore.inv_doc_vat_num, "" + tbAfter.inv_doc_vat_num);
            if (tbBefore.inv_doc_cf_num != tbAfter.inv_doc_cf_num) addLog("inv_doc_cf_num", "" + tbBefore.inv_doc_cf_num, "" + tbAfter.inv_doc_cf_num);
            if (tbBefore.problemID != tbAfter.problemID) addLog("problemID", "" + tbBefore.problemID, "" + tbAfter.problemID);
            if (tbBefore.problemDesc != tbAfter.problemDesc) addLog("problemDesc", "" + tbBefore.problemDesc, "" + tbAfter.problemDesc);
            if (tbBefore.agentID != tbAfter.agentID) addLog("agentID", "" + tbBefore.agentID, "" + tbAfter.agentID);
            if (tbBefore.agentPaymentType != tbAfter.agentPaymentType) addLog("agentPaymentType", "" + tbBefore.agentPaymentType, "" + tbAfter.agentPaymentType);
            if (tbBefore.agentDiscountNotPayed != tbAfter.agentDiscountNotPayed) addLog("agentDiscountNotPayed", "" + tbBefore.agentDiscountNotPayed, "" + tbAfter.agentDiscountNotPayed);
            if (tbBefore.agentDiscountType != tbAfter.agentDiscountType) addLog("agentDiscountType", "" + tbBefore.agentDiscountType, "" + tbAfter.agentDiscountType);
            if (tbBefore.agentCommissionPerc != tbAfter.agentCommissionPerc) addLog("agentCommissionPerc", "" + tbBefore.agentCommissionPerc, "" + tbAfter.agentCommissionPerc);
            if (tbBefore.agentCommissionPrice != tbAfter.agentCommissionPrice) addLog("agentCommissionPrice", "" + tbBefore.agentCommissionPrice, "" + tbAfter.agentCommissionPrice);
            if (tbBefore.agentClientID != tbAfter.agentClientID) addLog("agentClientID", "" + tbBefore.agentClientID, "" + tbAfter.agentClientID);
            if (tbBefore.IdAdMedia != tbAfter.IdAdMedia) addLog("IdAdMedia", "" + tbBefore.IdAdMedia, "" + tbAfter.IdAdMedia);
            if (tbBefore.IdLink != tbAfter.IdLink) addLog("IdLink", "" + tbBefore.IdLink, "" + tbAfter.IdLink);
            if (tbBefore.pr_part_extraServices != tbAfter.pr_part_extraServices) addLog("pr_part_extraServices", "" + tbBefore.pr_part_extraServices, "" + tbAfter.pr_part_extraServices);
            if (tbBefore.prRefundDate != tbAfter.prRefundDate) addLog("prRefundDate", "" + tbBefore.prRefundDate, "" + tbAfter.prRefundDate);
            if (tbBefore.prRefundTotal != tbAfter.prRefundTotal) addLog("prRefundTotal", "" + tbBefore.prRefundTotal, "" + tbAfter.prRefundTotal);
            if (tbBefore.prRefundPayMode != tbAfter.prRefundPayMode) addLog("prRefundPayMode", "" + tbBefore.prRefundPayMode, "" + tbAfter.prRefundPayMode);
            if (tbBefore.prTotalRate != tbAfter.prTotalRate) addLog("prTotalRate", "" + tbBefore.prTotalRate, "" + tbAfter.prTotalRate);
            if (tbBefore.prTotalOwner != tbAfter.prTotalOwner) addLog("prTotalOwner", "" + tbBefore.prTotalOwner, "" + tbAfter.prTotalOwner);
            if (tbBefore.prTotalCommission != tbAfter.prTotalCommission) addLog("prTotalCommission", "" + tbBefore.prTotalCommission, "" + tbAfter.prTotalCommission);
            if (tbBefore.prDiscountSpecialOffer != tbAfter.prDiscountSpecialOffer) addLog("prDiscountSpecialOffer", "" + tbBefore.prDiscountSpecialOffer, "" + tbAfter.prDiscountSpecialOffer);
            if (tbBefore.prDiscountLongStay != tbAfter.prDiscountLongStay) addLog("prDiscountLongStay", "" + tbBefore.prDiscountLongStay, "" + tbAfter.prDiscountLongStay);
            if (tbBefore.prDiscountLongStayDesc != tbAfter.prDiscountLongStayDesc) addLog("prDiscountLongStayDesc", "" + tbBefore.prDiscountLongStayDesc, "" + tbAfter.prDiscountLongStayDesc);
            if (tbBefore.prDiscountLongRange != tbAfter.prDiscountLongRange) addLog("prDiscountLongRange", "" + tbBefore.prDiscountLongRange, "" + tbAfter.prDiscountLongRange);
            if (tbBefore.prDiscountLongRangeDesc != tbAfter.prDiscountLongRangeDesc) addLog("prDiscountLongRangeDesc", "" + tbBefore.prDiscountLongRangeDesc, "" + tbAfter.prDiscountLongRangeDesc);
            if (tbBefore.prDiscountLastMinute != tbAfter.prDiscountLastMinute) addLog("prDiscountLastMinute", "" + tbBefore.prDiscountLastMinute, "" + tbAfter.prDiscountLastMinute);
            if (tbBefore.prDiscountLastMinuteDesc != tbAfter.prDiscountLastMinuteDesc) addLog("prDiscountLastMinuteDesc", "" + tbBefore.prDiscountLastMinuteDesc, "" + tbAfter.prDiscountLastMinuteDesc);

            #region White Label
            if (tbBefore.isWL != tbAfter.isWL) addLog("isWL", "" + tbBefore.isWL, "" + tbAfter.isWL);

            if (tbBefore.WL_changeAmount != tbAfter.WL_changeAmount) addLog("WL_changeAmount", "" + tbBefore.WL_changeAmount, "" + tbAfter.WL_changeAmount);
            if (tbBefore.WL_changeIsDiscount != tbAfter.WL_changeIsDiscount) addLog("WL_changeIsDiscount", "" + tbBefore.WL_changeIsDiscount, "" + tbAfter.WL_changeIsDiscount);
            if (tbBefore.WL_changeIsPercentage != tbAfter.WL_changeIsPercentage) addLog("WL_changeIsPercentage", "" + tbBefore.WL_changeIsPercentage, "" + tbAfter.WL_changeIsPercentage);
            if (tbBefore.WL_changeAmount_Agent != tbAfter.WL_changeAmount_Agent) addLog("WL_changeAmount_Agent", "" + tbBefore.WL_changeAmount_Agent, "" + tbAfter.WL_changeAmount_Agent);
            if (tbBefore.WL_changeIsDiscount_Agent != tbAfter.WL_changeIsDiscount_Agent) addLog("WL_changeIsDiscount_Agent", "" + tbBefore.WL_changeIsDiscount_Agent, "" + tbAfter.WL_changeIsDiscount_Agent);
            if (tbBefore.WL_changeIsPercentage_Agent != tbAfter.WL_changeIsPercentage_Agent) addLog("WL_changeIsPercentage_Agent", "" + tbBefore.WL_changeIsPercentage_Agent, "" + tbAfter.WL_changeIsPercentage_Agent);
            #endregion
        }
        public RNT_TBL_RESERVATION_addLogWork(RNT_TBL_RESERVATION Before, RNT_TBL_RESERVATION After, int UserID, string UserName)
        {
            tbBefore = Before;
            tbAfter = After;
            logDate = DateTime.Now;
            userID = UserID;
            userName = UserName;
            recordID = tbBefore.id;
            recordCode = tbBefore.code;

            //Action<object> action = (object obj) => { doThread(); };
            //AppUtilsTaskScheduler.AddTask(action, "rntUtils.RNT_TBL_RESERVATION_addLogWork resId:" + recordID);

            ThreadStart start = new ThreadStart(doThread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();

        }
    }

}
public partial class rntProps
{
    public static int ImgSmallWidth = 800;
    private static List<dbRntAgentTBL> tmpAgentTBL; // refresh Auto
    public static List<dbRntAgentTBL> AgentTBL
    {
        get
        {
            if (tmpAgentTBL == null)
            {
                using (DCmodRental dc = new DCmodRental())
                    tmpAgentTBL = dc.dbRntAgentTBLs.ToList();
            }
            return new List<dbRntAgentTBL>(tmpAgentTBL);
        }
        set { tmpAgentTBL = value; }
    }

    private static List<dbRntAgentWLPaymentRL> tmpAgentWLPaymentRL; // refresh Auto
    public static List<dbRntAgentWLPaymentRL> AgentWLPaymentRL
    {
        get
        {
            if (tmpAgentWLPaymentRL == null)
            {
                using (DCmodRental dc = new DCmodRental())
                    tmpAgentWLPaymentRL = dc.dbRntAgentWLPaymentRLs.ToList();
            }
            return new List<dbRntAgentWLPaymentRL>(tmpAgentWLPaymentRL);
        }
        set { tmpAgentWLPaymentRL = value; }
    }

    private static List<dbRntProblemTBL> tmpProblemTBL; // refresh Auto
    public static List<dbRntProblemTBL> ProblemTBL
    {
        get
        {
            if (tmpProblemTBL == null)
            {
                using (DCmodRental dc = new DCmodRental())
                    tmpProblemTBL = dc.dbRntProblemTBLs.ToList();
            }
            return new List<dbRntProblemTBL>(tmpProblemTBL);
        }
        set { tmpProblemTBL = value; }
    }
    private static List<dbRntDiscountTypeTBL> tmpDiscountTypeTBL; // refresh Auto
    public static List<dbRntDiscountTypeTBL> DiscountTypeTBL
    {
        get
        {
            if (tmpDiscountTypeTBL == null)
            {
                using (DCmodRental dc = new DCmodRental())
                    tmpDiscountTypeTBL = dc.dbRntDiscountTypeTBLs.ToList();
            }
            return new List<dbRntDiscountTypeTBL>(tmpDiscountTypeTBL);
        }
        set { tmpDiscountTypeTBL = value; }
    }

    private static List<dbRntEstateInternsSubTypeVIEW> tmpEstateInternsSubTypeVIEW; // refresh AUTO
    public static List<dbRntEstateInternsSubTypeVIEW> EstateInternsSubTypeVIEW
    {
        get
        {
            if (tmpEstateInternsSubTypeVIEW == null)
            {
                using (DCmodRental dc = new DCmodRental())
                    tmpEstateInternsSubTypeVIEW = dc.dbRntEstateInternsSubTypeVIEWs.ToList();
            }
            return new List<dbRntEstateInternsSubTypeVIEW>(tmpEstateInternsSubTypeVIEW);
        }
        set { tmpEstateInternsSubTypeVIEW = value; }
    }

}
public static partial class rntExts
{
    [Serializable()]
    public class DateAvvPrice : ISerializable
    {
        public DateTime Date { get; set; }
        public int Pax { get; set; }
        public decimal Price { get; set; }
        public decimal PriceChange { get; set; }
        public string PriceChangeDesc { get; set; }
        public DateAvvPrice(DateTime date, int pax, decimal price, decimal priceChange, string priceChangeDesc)
        {
            Date = date;
            Pax = pax;
            Price = price;
            PriceChange = priceChange;
            PriceChangeDesc = priceChangeDesc;
        }
        //Deserialization constructor.
        public DateAvvPrice(SerializationInfo info, StreamingContext ctxt)
        {
            Date = (DateTime)info.GetValue("Date", typeof(DateTime));
            Pax = (int)info.GetValue("Pax", typeof(int));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
            PriceChange = (decimal)info.GetValue("PriceChange", typeof(decimal));
            PriceChangeDesc = (string)info.GetValue("PriceChangeDesc", typeof(string));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Date", Date);
            info.AddValue("Pax", Pax);
            info.AddValue("Price", Price);
            info.AddValue("PriceChange", PriceChange);
            info.AddValue("PriceChangeDesc", PriceChangeDesc);
        }



    }

    public static void CopyToPriceType(this dbRntExtraPriceTypesLN source, ref dbRntExtraPriceTypesLN copyto)
    {
        copyto.title = source.title;
        copyto.subTitle = source.subTitle;
        copyto.description = source.description;
    }
    public class PriceListPerDates
    {
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public int MinStay { get; set; }
        public int Period { get; set; }

        public Dictionary<int, decimal> Prices { get; set; }
        public string PriceChangeDesc { get; set; }
        public PriceListPerDates(DateTime dtStart, DateTime dtEnd, int minStay, string priceChangeDesc)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            MinStay = minStay;
            Prices = new Dictionary<int, decimal>();

            PriceChangeDesc = priceChangeDesc;
        }
        public PriceListPerDates(DateTime dtStart, DateTime dtEnd, int minNights, PriceListPerDates copyFrom)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            MinStay = minNights;
            Period = copyFrom.Period;

            Prices = copyFrom.Prices;
            PriceChangeDesc = copyFrom.PriceChangeDesc;
        }
        public bool HasSamePrices(Dictionary<int, decimal> comparePrices)
        {
            if (Prices.Count != comparePrices.Count) return false;
            foreach (var item in Prices)
                if (comparePrices[item.Key] == null || comparePrices[item.Key] != item.Value)
                    return false;
            return true;
        }
        public bool HasSamePrices(PriceListPerDates compareWith)
        {
            if (MinStay != compareWith.MinStay) return false;
            if (Prices.Count != compareWith.Prices.Count) return false;

            foreach (var item in Prices)
                if (compareWith.Prices[item.Key] == null || compareWith.Prices[item.Key] != item.Value)
                    return false;
            return true;
        }
    }
    public class AvvListPerDates
    {
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public bool IsAvv { get; set; }
        public int Units { get; set; }
        public AvvListPerDates(DateTime dtStart, DateTime dtEnd, bool isAvv)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            IsAvv = isAvv;
        }
        public AvvListPerDates(DateTime dtStart, DateTime dtEnd, bool isAvv, int units)
        {
            DtStart = dtStart;
            DtEnd = dtEnd;
            IsAvv = isAvv;
            Units = units;
        }
    }
    public static RNT_TBL_RESERVATION Clone(this RNT_TBL_RESERVATION source)
    {
        RNT_TBL_RESERVATION clone = new RNT_TBL_RESERVATION();
        clone.id = source.id;
        clone.unique_id = source.unique_id;
        clone.uid_2 = source.uid_2;
        clone.code = source.code;
        clone.password = source.password;
        clone.pid_related_request = source.pid_related_request;
        clone.pid_creator = source.pid_creator;
        clone.pid_operator = source.pid_operator;
        clone.operator_date = source.operator_date;
        clone.pid_estate = source.pid_estate;
        clone.pidEstateCity = source.pidEstateCity;
        clone.cl_id = source.cl_id;
        clone.cl_pid_discount = source.cl_pid_discount;
        clone.cl_pid_lang = source.cl_pid_lang;
        clone.cl_email = source.cl_email;
        clone.cl_name_honorific = source.cl_name_honorific;
        clone.cl_name_full = source.cl_name_full;
        clone.cl_loc_country = source.cl_loc_country;
        clone.cl_isCompleted = source.cl_isCompleted;
        clone.cl_reminderLast = source.cl_reminderLast;
        clone.cl_reminderNext = source.cl_reminderNext;
        clone.cl_reminderCount = source.cl_reminderCount;
        clone.cl_browserInfo = source.cl_browserInfo;
        clone.cl_browserIP = source.cl_browserIP;
        clone.num_adult = source.num_adult;
        clone.num_child_over = source.num_child_over;
        clone.num_child_min = source.num_child_min;
        clone.visa_isRequested = source.visa_isRequested;
        clone.visa_persons = source.visa_persons;
        clone.dtStart = source.dtStart;
        clone.dtEnd = source.dtEnd;
        clone.dtIn = source.dtIn;
        clone.dtOut = source.dtOut;
        clone.dtStartTime = source.dtStartTime;
        clone.dtEndTime = source.dtEndTime;
        clone.is_dtStartTimeChanged = source.is_dtStartTimeChanged;
        clone.is_dtEndTimeChanged = source.is_dtEndTimeChanged;
        clone.limo_easyShuttleID = source.limo_easyShuttleID;
        clone.limo_easyShuttleInUID = source.limo_easyShuttleInUID;
        clone.limo_easyShuttleOutUID = source.limo_easyShuttleOutUID;
        clone.limo_in_datetime = source.limo_in_datetime;
        clone.limo_in_isRequested = source.limo_in_isRequested;
        clone.limo_inPoint_type = source.limo_inPoint_type;
        clone.limo_inPoint_transportType = source.limo_inPoint_transportType;
        clone.limo_inPoint_pickupPlace = source.limo_inPoint_pickupPlace;
        clone.limo_inPoint_pickupPlaceName = source.limo_inPoint_pickupPlaceName;
        clone.limo_inPoint_details = source.limo_inPoint_details;
        clone.limo_inPoint_detailsType = source.limo_inPoint_detailsType;
        clone.limo_out_datetime = source.limo_out_datetime;
        clone.limo_out_isRequested = source.limo_out_isRequested;
        clone.limo_outPoint_type = source.limo_outPoint_type;
        clone.limo_outPoint_transportType = source.limo_outPoint_transportType;
        clone.limo_outPoint_pickupPlace = source.limo_outPoint_pickupPlace;
        clone.limo_outPoint_pickupPlaceName = source.limo_outPoint_pickupPlaceName;
        clone.limo_outPoint_details = source.limo_outPoint_details;
        clone.limo_outPoint_detailsType = source.limo_outPoint_detailsType;
        clone.limo_num_case_s = source.limo_num_case_s;
        clone.limo_num_case_m = source.limo_num_case_m;
        clone.limo_num_case_l = source.limo_num_case_l;
        clone.limo_request = source.limo_request;
        clone.limo_isCompleted = source.limo_isCompleted;
        clone.state_pid = source.state_pid;
        clone.state_date = source.state_date;
        clone.state_pid_user = source.state_pid_user;
        clone.state_subject = source.state_subject;
        clone.state_body = source.state_body;
        clone.dtCreation = source.dtCreation;
        clone.notesInner = source.notesInner;
        clone.notesClient = source.notesClient;
        clone.notesEco = source.notesEco;
        clone.block_expire = source.block_expire;
        clone.block_expire_hours = source.block_expire_hours;
        clone.block_pid_user = source.block_pid_user;
        clone.block_comments = source.block_comments;
        clone.srs_ext_meetingPoint = source.srs_ext_meetingPoint;
        clone.pr_deposit_modified = source.pr_deposit_modified;
        clone.pr_deposit = source.pr_deposit;
        clone.pr_depositNotes = source.pr_depositNotes;
        clone.pr_depositWithCard = source.pr_depositWithCard;
        clone.pr_total_tf = source.pr_total_tf;
        clone.pr_total_tax = source.pr_total_tax;
        clone.pr_total_tax_id = source.pr_total_tax_id;
        clone.pr_total_margin = source.pr_total_margin;
        clone.pr_total = source.pr_total;
        clone.pr_total_desc = source.pr_total_desc;
        clone.pr_discount_owner = source.pr_discount_owner;
        clone.pr_discount_commission = source.pr_discount_commission;
        clone.pr_discount_desc = source.pr_discount_desc;
        clone.pr_discount_user = source.pr_discount_user;
        clone.pr_discount_custom = source.pr_discount_custom;
        clone.pr_reservation = source.pr_reservation;
        clone.pr_part_modified = source.pr_part_modified;
        clone.pr_part_modify_notes = source.pr_part_modify_notes;
        clone.pr_part_commission_tf = source.pr_part_commission_tf;
        clone.pr_part_commission_total = source.pr_part_commission_total;
        clone.pr_part_agency_fee = source.pr_part_agency_fee;
        clone.pr_part_payment_total = source.pr_part_payment_total;
        clone.pr_part_forPayment = source.pr_part_forPayment;
        clone.pr_part_owner = source.pr_part_owner;
        clone.pr_ecoPrice = source.pr_ecoPrice;
        clone.pr_ecoCount = source.pr_ecoCount;
        clone.pr_srsPrice = source.pr_srsPrice;
        clone.payed_total = source.payed_total;
        clone.payed_part = source.payed_part;
        clone.payed_date = source.payed_date;
        clone.payed_user = source.payed_user;
        clone.payed_mode = source.payed_mode;
        clone.requestRenewal = source.requestRenewal;
        clone.requestRenewalDate = source.requestRenewalDate;
        clone.requestFullPay = source.requestFullPay;
        clone.requestFullPayDate = source.requestFullPayDate;
        clone.requestFullPayAccepted = source.requestFullPayAccepted;
        clone.requestFullPayAcceptedDate = source.requestFullPayAcceptedDate;
        clone.is_deleted = source.is_deleted;
        clone.is_booking = source.is_booking;
        clone.inv_toCreate = source.inv_toCreate;
        clone.bedSingle = source.bedSingle;
        clone.bedDouble = source.bedDouble;
        clone.bedDoubleD = source.bedDoubleD;
        clone.bedDoubleDConfig = source.bedDoubleDConfig;
        clone.bedDouble2level = source.bedDouble2level;
        clone.bedDouble2levelConfig = source.bedDouble2levelConfig;
        clone.bedSofaSingle = source.bedSofaSingle;
        clone.bedSofaDouble = source.bedSofaDouble;
        clone.inv_isDifferent = source.inv_isDifferent;
        clone.inv_name_honorific = source.inv_name_honorific;
        clone.inv_name_full = source.inv_name_full;
        clone.inv_loc_country = source.inv_loc_country;
        clone.inv_loc_state = source.inv_loc_state;
        clone.inv_loc_city = source.inv_loc_city;
        clone.inv_loc_address = source.inv_loc_address;
        clone.inv_loc_zip_code = source.inv_loc_zip_code;
        clone.inv_doc_vat_num = source.inv_doc_vat_num;
        clone.inv_doc_cf_num = source.inv_doc_cf_num;
        clone.problemID = source.problemID;
        clone.problemDesc = source.problemDesc;
        clone.agentID = source.agentID;
        clone.agentPaymentType = source.agentPaymentType;
        clone.agentDiscountNotPayed = source.agentDiscountNotPayed;
        clone.agentDiscountType = source.agentDiscountType;
        clone.agentCommissionPerc = source.agentCommissionPerc;
        clone.agentCommissionPrice = source.agentCommissionPrice;
        clone.agentClientID = source.agentClientID;
        clone.IdAdMedia = source.IdAdMedia;
        clone.IdLink = source.IdLink;
        clone.pr_part_extraServices = source.pr_part_extraServices;
        clone.prRefundDate = source.prRefundDate;
        clone.prRefundTotal = source.prRefundTotal;
        clone.prRefundPayMode = source.prRefundPayMode;
        clone.prTotalRate = source.prTotalRate;
        clone.prTotalOwner = source.prTotalOwner;
        clone.prTotalCommission = source.prTotalCommission;
        clone.prDiscountSpecialOffer = source.prDiscountSpecialOffer;
        clone.prDiscountLongStay = source.prDiscountLongStay;
        clone.prDiscountLongStayDesc = source.prDiscountLongStayDesc;
        clone.prDiscountLongRange = source.prDiscountLongRange;
        clone.prDiscountLongRangeDesc = source.prDiscountLongRangeDesc;
        clone.prDiscountLastMinute = source.prDiscountLastMinute;
        clone.prDiscountLastMinuteDesc = source.prDiscountLastMinuteDesc;
        clone.bcom_resId = source.bcom_resId;
        clone.bcom_roomResId = source.bcom_roomResId;
        clone.bcom_loyalityid = source.bcom_loyalityid;
        clone.bcom_commissionamount = source.bcom_commissionamount;
        clone.bcom_currencycode = source.bcom_currencycode;
        clone.bcom_extrainfo = source.bcom_extrainfo;
        clone.bcom_facilities = source.bcom_facilities;
        clone.bcom_info = source.bcom_info;
        clone.bcom_mealplan = source.bcom_mealplan;
        clone.bcom_rateid = source.bcom_rateid;
        clone.bcom_smoking = source.bcom_smoking;
        clone.bcom_note = source.bcom_note;
        clone.bcom_cancel = source.bcom_cancel;
        clone.bcom_cancelcharge = source.bcom_cancelcharge;
        clone.bcom_guest_name = source.bcom_guest_name;
        clone.bcom_room_remarks = source.bcom_room_remarks;
        clone.bcom_room_price = source.bcom_room_price;
        clone.bcom_status = source.bcom_status;
        clone.bcom_country_code = source.bcom_country_code;
        clone.bcom_pid_parent_booking = source.bcom_pid_parent_booking;
        clone.bcom_maxChidren = source.bcom_maxChidren;
        clone.pr_paymentType = source.pr_paymentType;


        #region White Label
        clone.isWL = source.isWL;

        clone.WL_changeAmount = source.WL_changeAmount;
        clone.WL_changeIsDiscount = source.WL_changeIsDiscount;
        clone.WL_changeIsPercentage = source.WL_changeIsPercentage;
        clone.WL_changeAmount_Agent = source.WL_changeAmount_Agent;
        clone.WL_changeIsDiscount_Agent = source.WL_changeIsDiscount_Agent;
        clone.WL_changeIsPercentage_Agent = source.WL_changeIsPercentage_Agent;
        #endregion
        return clone;
    }
    public static RNT_TB_ESTATE Clone(this RNT_TB_ESTATE source)
    {
        RNT_TB_ESTATE clone = new RNT_TB_ESTATE();
        clone.id = source.id;
        clone.code = source.code;
        clone.pid_residence = source.pid_residence;
        clone.category = source.category;
        clone.pid_type = source.pid_type;
        clone.pid_category = source.pid_category;
        clone.pid_owner = source.pid_owner;
        clone.pid_agent = source.pid_agent;
        clone.pid_city = source.pid_city;
        clone.pid_zone = source.pid_zone;
        clone.loc_zip_code = source.loc_zip_code;
        clone.loc_address = source.loc_address;
        clone.loc_inner_bell = source.loc_inner_bell;
        clone.loc_referer = source.loc_referer;
        clone.loc_phone_1 = source.loc_phone_1;
        clone.loc_phone_2 = source.loc_phone_2;
        clone.mq_inner = source.mq_inner;
        clone.mq_outer = source.mq_outer;
        clone.mq_terrace = source.mq_terrace;
        clone.on_floor = source.on_floor;
        clone.on_floor_of_total = source.on_floor_of_total;
        clone.on_levels = source.on_levels;
        clone.num_bed_single = source.num_bed_single;
        clone.num_bed_double = source.num_bed_double;
        clone.num_bed_double_divisible = source.num_bed_double_divisible;
        clone.num_bed_double_2level = source.num_bed_double_2level;
        clone.num_sofa_single = source.num_sofa_single;
        clone.num_sofa_double = source.num_sofa_double;
        clone.num_persons_adult = source.num_persons_adult;
        clone.num_persons_child = source.num_persons_child;
        clone.num_persons_optional = source.num_persons_optional;
        clone.num_persons_min = source.num_persons_min;
        clone.num_persons_max = source.num_persons_max;
        clone.num_rooms_bed = source.num_rooms_bed;
        clone.num_rooms_bath = source.num_rooms_bath;
        clone.num_rooms_total = source.num_rooms_total;
        clone.num_terraces = source.num_terraces;
        clone.eco_pr_1 = source.eco_pr_1;
        clone.eco_pr_2 = source.eco_pr_2;
        clone.eco_time_preview = source.eco_time_preview;
        clone.eco_notes = source.eco_notes;
        clone.eco_ext_name_full = source.eco_ext_name_full;
        clone.eco_ext_email = source.eco_ext_email;
        clone.eco_ext_phone = source.eco_ext_phone;
        clone.eco_ext_price = source.eco_ext_price;
        clone.eco_ext_payInDays = source.eco_ext_payInDays;
        clone.eco_ext_clientPay = source.eco_ext_clientPay;
        clone.srs_ext_name_full = source.srs_ext_name_full;
        clone.srs_ext_email = source.srs_ext_email;
        clone.srs_ext_phone = source.srs_ext_phone;
        clone.srs_ext_phone_2 = source.srs_ext_phone_2;
        clone.srs_ext_phone_3 = source.srs_ext_phone_3;
        clone.srs_ext_phone_4 = source.srs_ext_phone_4;
        clone.srs_ext_price = source.srs_ext_price;
        clone.srs_ext_clientPay = source.srs_ext_clientPay;
        clone.srs_ext_meetingPoint = source.srs_ext_meetingPoint;
        clone.pr_basePersons = source.pr_basePersons;
        clone.pr_1_2pax = source.pr_1_2pax;
        clone.pr_1_opt = source.pr_1_opt;
        clone.pr_2_2pax = source.pr_2_2pax;
        clone.pr_2_opt = source.pr_2_opt;
        clone.pr_3_2pax = source.pr_3_2pax;
        clone.pr_3_opt = source.pr_3_opt;
        clone.pr_tableViewType = source.pr_tableViewType;
        clone.pr_discount7days = source.pr_discount7days;
        clone.pr_discount30days = source.pr_discount30days;
        clone.pr_startDate = source.pr_startDate;
        clone.pr_dcSUsed = source.pr_dcSUsed;
        clone.pr_dcS2_1_inDays = source.pr_dcS2_1_inDays;
        clone.pr_dcS2_1_percent = source.pr_dcS2_1_percent;
        clone.pr_dcS2_2_inDays = source.pr_dcS2_2_inDays;
        clone.pr_dcS2_2_percent = source.pr_dcS2_2_percent;
        clone.pr_dcS2_3_inDays = source.pr_dcS2_3_inDays;
        clone.pr_dcS2_3_percent = source.pr_dcS2_3_percent;
        clone.pr_dcS2_4_inDays = source.pr_dcS2_4_inDays;
        clone.pr_dcS2_4_percent = source.pr_dcS2_4_percent;
        clone.pr_dcS2_5_inDays = source.pr_dcS2_5_inDays;
        clone.pr_dcS2_5_percent = source.pr_dcS2_5_percent;
        clone.pr_dcS2_6_inDays = source.pr_dcS2_6_inDays;
        clone.pr_dcS2_6_percent = source.pr_dcS2_6_percent;
        clone.pr_dcS2_7_inDays = source.pr_dcS2_7_inDays;
        clone.pr_dcS2_7_percent = source.pr_dcS2_7_percent;
        clone.pr_percentage = source.pr_percentage;
        clone.pr_deposit = source.pr_deposit;
        clone.pr_depositWithCard = source.pr_depositWithCard;
        clone.pr_has_overnight_tax = source.pr_has_overnight_tax;
        clone.ext_ownerdaysinyear = source.ext_ownerdaysinyear;
        clone.lm_inhours = source.lm_inhours;
        clone.lm_discount = source.lm_discount;
        clone.lm_nights_min = source.lm_nights_min;
        clone.lm_nights_max = source.lm_nights_max;
        clone.lpb_is = source.lpb_is;
        clone.lpb_nights_min = source.lpb_nights_min;
        clone.lpb_afterdays = source.lpb_afterdays;
        clone.lpb_onlyhighseason = source.lpb_onlyhighseason;
        clone.nights_minVHSeason = source.nights_minVHSeason;
        clone.nights_min = source.nights_min;
        clone.nights_max = source.nights_max;
        clone.longTermRent = source.longTermRent;
        clone.longTermPrMonthly = source.longTermPrMonthly;
        clone.importance = source.importance;
        clone.importance_vote = source.importance_vote;
        clone.importance_stars = source.importance_stars;
        clone.media_folder = source.media_folder;
        clone.img_thumb = source.img_thumb;
        clone.img_preview_1 = source.img_preview_1;
        clone.img_preview_2 = source.img_preview_2;
        clone.img_preview_3 = source.img_preview_3;
        clone.img_banner = source.img_banner;
        clone.mobileVideoFilePath = source.mobileVideoFilePath;
        clone.inner_notes = source.inner_notes;
        clone.sv_yaw = source.sv_yaw;
        clone.sv_pitch = source.sv_pitch;
        clone.sv_zoom = source.sv_zoom;
        clone.sv_coords = source.sv_coords;
        clone.is_street_view = source.is_street_view;
        clone.google_maps = source.google_maps;
        clone.is_google_maps = source.is_google_maps;
        clone.is_loft = source.is_loft;
        clone.is_exclusive = source.is_exclusive;
        clone.is_srs = source.is_srs;
        clone.is_ecopulizie = source.is_ecopulizie;
        clone.is_online_booking = source.is_online_booking;
        clone.isCedolareSecca = source.isCedolareSecca;
        clone.is_active = source.is_active;
        clone.is_deleted = source.is_deleted;
        clone.sequence = source.sequence;
        clone.zoneSequence = source.zoneSequence;
        clone.ext_gps_id = source.ext_gps_id;
        clone.ext_gps_pid_maga = source.ext_gps_pid_maga;
        clone.in_attention = source.in_attention;
        clone.t_xmlPrezzo = source.t_xmlPrezzo;
        clone.t_piano = source.t_piano;
        clone.t_terrazzo = source.t_terrazzo;
        clone.t_propCognome = source.t_propCognome;
        clone.t_propNome = source.t_propNome;
        clone.t_propEmail = source.t_propEmail;
        clone.t_propContatti = source.t_propContatti;
        clone.t_propCFPI = source.t_propCFPI;
        clone.t_propIndirizzo = source.t_propIndirizzo;
        clone.t_mezzi = source.t_mezzi;
        clone.t_modPagamento = source.t_modPagamento;
        clone.t_propWeb = source.t_propWeb;
        clone.t_metriQ = source.t_metriQ;
        clone.t_extra = source.t_extra;
        clone.t_link = source.t_link;
        clone.isContratto = source.isContratto;
        clone.haPropertyType = source.haPropertyType;
        clone.haListingId = source.haListingId;
        clone.haAdvertiserId = source.haAdvertiserId;
        clone.is_HomeAway = source.is_HomeAway;
        clone.pidSeasonGroup = source.pidSeasonGroup;
        clone.iCalUrl = source.iCalUrl;
        clone.bcomHotelId = source.bcomHotelId;
        clone.bcomRoomId = source.bcomRoomId;
        clone.bcomEnabled = source.bcomEnabled;
        return clone;
    }

    public static void CopyTo(this dbRntEstateInternsSubTypeLN source, ref dbRntEstateInternsSubTypeLN copyto)
    {
        copyto.title = source.title;
        copyto.description = source.description;
    }

    public static void CopyTo(this dbRntEstateInternsFeatureLN source, ref dbRntEstateInternsFeatureLN copyto)
    {
        copyto.title = source.title;
        copyto.description = source.description;
    }
}
