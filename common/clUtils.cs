using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class clSearch
{
    public string searchTitle { get; set; }
    public int currType { get; set; }
    public int currCountry { get; set; }
    public int currCity { get; set; }
    public int currZone { get; set; }
    public List<int> currZoneList { get; set; }
    public List<int> currConfigList { get; set; }

    public DateTime dtStart { get; set; }
    public DateTime dtEnd { get; set; }
    public int dtCount { get; set; }
    public int numPersCount { get; set; }
    public int numPers_adult { get; set; }
    public int numPers_childOver { get; set; }
    public int numPers_childMin { get; set; }

    public int prMin { get; set; }
    public int prMax { get; set; }
    public int voteMin { get; set; }
    public int voteMax { get; set; }

    public int voteMinNew { get; set; }
    public int voteMaxNew { get; set; }

    public string orderBy { get; set; }
    public string orderHow { get; set; }
    public string listType { get; set; }
    public int currPage { get; set; }
    public int numPerPage { get; set; }
    public clSearch()
    {
        searchTitle = "";
        currType = -1;
        currCountry = -1;
        currCity = -1;
        currZone = -1;
        currZoneList = new List<int>();
        currConfigList = new List<int>();

        dtStart = DateTime.Now.AddDays(7);
        dtEnd = DateTime.Now.AddDays(10);
        dtCount = 3;
        numPersCount = 2;
        numPers_adult = 2;
        numPers_childOver = 0;
        numPers_childMin = 0;

        prMin = 0;
        prMax = 0;
        voteMin = 0;
        voteMax = 10;

        voteMinNew = 0;
        voteMaxNew = 0;

        orderBy = "";
        orderHow = "";
        listType = "";
        currPage = 1;
        numPerPage = 20;
    }
}
public class clZoneFilter
{
    public string searchTitle { get; set; }
    public int currZone { get; set; }

    public string orderBy { get; set; }
    public string orderHow { get; set; }
    public string listType { get; set; }
    public int currPage { get; set; }
    public int numPerPage { get; set; }
    public clZoneFilter()
    {
        searchTitle = "";
        currZone = -1;

        orderBy = "";
        orderHow = "";
        listType = "";
        currPage = 1;
        numPerPage = 20;
    }
}
public class clTypeFilter
{
    public string searchTitle { get; set; }
    public string currType { get; set; }
    public int currZone { get; set; }
    // = "longTerm"
    public string orderBy { get; set; }
    public string orderHow { get; set; }
    public string listType { get; set; }
    public int currPage { get; set; }
    public int numPerPage { get; set; }
    public clTypeFilter(string Type)
    {
        currType = Type;
        currZone = -1;
        searchTitle = "";

        orderBy = "";
        orderHow = "";
        listType = "";
        currPage = 1;
        numPerPage = 20;
    }
}
public class clConfig
{
    public string UID { get; set; }
    public DateTime dtCreated { get; set; }
    public DateTime dtLastUsed { get; set; }
    public int pid_lang { get; set; }
    public int pid_client { get; set; }
    public clSearch lastSearch { get; set; }
    public clSearch lastZoneSearch { get; set; }
    public clSearch lastCategorySearch { get; set; }
    public List<clZoneFilter> zoneFilters { get; set; }
    public List<clTypeFilter> typeFilters { get; set; }
    public List<int> myPreferedEstateList { get; set; }
    public List<int> myLastVisitedEstateList { get; set; }
    public clConfig(string _UID)
    {
        UID = _UID;
        dtCreated = DateTime.Now.AddMinutes(-10);
        dtLastUsed = dtCreated;
        pid_lang = 2;
        pid_client = 0;
        lastSearch = new clSearch();
        lastZoneSearch = new clSearch();
        lastCategorySearch = new clSearch();
        zoneFilters = new List<clZoneFilter>();
        typeFilters = new List<clTypeFilter>();
        myPreferedEstateList = new List<int>();
        myLastVisitedEstateList = new List<int>();
    }
    public void addTo_myLastVisitedEstateList(int id)
    {
        if (myLastVisitedEstateList.Contains(id))
            myLastVisitedEstateList.Remove(id);
        myLastVisitedEstateList.Insert(0, id);
    }
    public void addTo_myPreferedEstateList(int id)
    {
        if (myPreferedEstateList.Contains(id))
            myPreferedEstateList.Remove(id);
        myPreferedEstateList.Insert(0, id);
    }
    public clConfig Clone()
    {
        clConfig _clone = new clConfig(UID);
        _clone.dtCreated = dtCreated;
        _clone.dtLastUsed = dtLastUsed;
        _clone.pid_lang = pid_lang;
        _clone.pid_client = pid_client;
        _clone.lastSearch = lastSearch;
        _clone.lastZoneSearch = lastZoneSearch;
        _clone.lastCategorySearch = lastCategorySearch;
        _clone.zoneFilters = zoneFilters;
        _clone.typeFilters = typeFilters;
        _clone.myPreferedEstateList = myPreferedEstateList;
        _clone.myLastVisitedEstateList = myLastVisitedEstateList;
        return _clone;
    }
}
public class listPagesItem
{
    public int sequence { get; set; }
    public int pageNum { get; set; }
    public bool isCurrent { get; set; }
    public string html { get; set; }
    public listPagesItem(int Sequence, int PageNum, bool IsCurrent, string Html)
    {
        sequence = Sequence;
        pageNum = PageNum;
        isCurrent = IsCurrent;
        html = Html;
    }
}
public class listPages
{
    public int pagesCount { get; set; }
    public List<listPagesItem> items { get; set; }
    public string pagesHtml
    {
        get
        {
            string tmp = "";
            foreach (var item in items)
                tmp += item.html;
            return tmp;
        }
    }
    public listPages(int allCount, int numPerPage, int currPage, int pagerCount, string allCurrStr, string allLnkStr, string currStr, string lnkStr, string prevStr, string nextStr)
    {
        items = new List<listPagesItem>();
        if (numPerPage <= 0) numPerPage = 1;
        pagesCount = 1;
        while (allCount > numPerPage)
        {
            pagesCount++;
            allCount -= numPerPage;
        }
        if (pagesCount <= pagerCount)
        {
            for (int i = 1; i <= pagesCount; i++)
            {
                items.Add(new listPagesItem(items.Count, i, i == currPage, i == currPage ? string.Format(currStr, i) : string.Format(lnkStr, i)));
            }
            if (pagesCount > 1)
            {
                items.Add(new listPagesItem(items.Count, 0, 0 == currPage, 0 == currPage ? string.Format(allCurrStr, 0) : string.Format(allLnkStr, 0)));
            }
            return;
        }
        int grCount = 0;
        int tmpPage = currPage == 0 ? 1 : currPage;
        while (tmpPage > grCount * pagerCount)
        {
            grCount++;
        }
        int tmpCurrPage = (grCount - 1) * pagerCount;
        if (grCount > 1)
        {
            items.Add(new listPagesItem(items.Count, 1, false, string.Format(lnkStr, "1")));
            items.Add(new listPagesItem(items.Count, (tmpCurrPage - 1), false, string.Format(prevStr, "" + (tmpCurrPage - 1))));
        }
        for (int i = (grCount - 1) * pagerCount; i <= grCount * pagerCount && i <= pagesCount; i++)
        {
            if (i != 0)
                items.Add(new listPagesItem(items.Count, i, i == currPage, i == currPage ? string.Format(currStr, i) : string.Format(lnkStr, i)));
            tmpCurrPage++;
        }
        if (tmpCurrPage <= pagesCount)
        {
            items.Add(new listPagesItem(items.Count, tmpCurrPage, false, string.Format(nextStr, tmpCurrPage)));
            items.Add(new listPagesItem(items.Count, pagesCount, false, string.Format(lnkStr, "" + pagesCount)));
        }
        items.Add(new listPagesItem(items.Count, 0, 0 == currPage, 0 == currPage ? string.Format(allCurrStr, 0) : string.Format(allLnkStr, 0)));
    }
}
public class clUtils
{
    public static clConfig getConfig(string _UID)
    {
        List<clConfig> _list = CURRENT_clConfigList;
        clConfig _config = _list.FirstOrDefault(x => x.UID == _UID);
        if (_config == null)
        {
            _config = new clConfig(_UID);
            _list.Add(_config);
            CURRENT_clConfigList = _list;
        }
        return _config;
    }
    public static bool saveConfig(clConfig _config)
    {
        List<clConfig> _list = CURRENT_clConfigList;
        _list.RemoveAll(x => x.UID == _config.UID);
        _list.Add(_config);
        CURRENT_clConfigList = _list;
        return true;
    }
    private static List<clConfig> _CURRENT_clConfigList;
    public static List<clConfig> CURRENT_clConfigList
    {
        get
        {
            try
            {
                if (_CURRENT_clConfigList == null)
                    _CURRENT_clConfigList = new List<clConfig>();
                else
                    _CURRENT_clConfigList.RemoveAll(x => x.dtCreated < DateTime.Now.AddDays(-1) || x.dtLastUsed < DateTime.Now.AddHours(-1));
            }
            catch (Exception ex) { _CURRENT_clConfigList = new List<clConfig>(); }
            return new List<clConfig>(_CURRENT_clConfigList.Select(x => x.Clone()));
        }
        set
        {
            _CURRENT_clConfigList = value; ;
        }
    }
}