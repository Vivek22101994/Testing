using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.IO;
public class UrlRedirectTool
{
    private static RedirectList CURRENT_TOOL_;
    public static RedirectList CURRENT_TOOL
    {
        get
        {
            if (CURRENT_TOOL_ == null)
                CURRENT_TOOL_ = new RedirectList();
            return CURRENT_TOOL_;
        }
        set
        {
            CURRENT_TOOL_ = value;
        }
    }
}
public class RedirectList
{
    private string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "UrlRedirectTool.xml");
    private List<RedirectItem> _items;

    public List<RedirectItem> Items { get { if (this._items != null)return this._items; else return new List<RedirectItem>(); } set { this._items = value; } }

    public RedirectList()
    {
        this._items = new List<RedirectItem>();
        if (!File.Exists(this._path)) return;
        XDocument _resource = XDocument.Load(this._path);
        var ds = from XElement e in _resource.Descendants("item")
                 select e;
        foreach (XElement e in ds)
        {
            RedirectItem item = new RedirectItem();
            item.From = e.Element("from").Value;
            item.To = e.Element("to").Value;
            this._items.Add(item);
        }
    }
    public void Save()
    {
        //UrlRedirectTool.CURRENT_TOOL = this;
        try
        {
            XDocument _resource = new XDocument();
            XElement rootElement = new XElement("list");
            foreach (RedirectItem item in this._items)
            {
                XElement record = new XElement("item");
                record.Add(new XElement("from", item.From));
                record.Add(new XElement("to", item.To));
                rootElement.Add(record);
            }
            _resource.Add(rootElement);
            _resource.Save(this._path);
        }
        catch (Exception exc)
        {
            string _ip = "";
            try { _ip = HttpContext.Current.Request.ServerVariables.Get("REMOTE_HOST"); }
            catch (Exception ex1) { }
            ErrorLog.addLog(_ip, "RedirectList.Save", exc.ToString());
        }

    }
}
public class RedirectItem
{
    public RedirectItem()
    {
        this.From = "";
        this.To = "";
    }
    public RedirectItem(string From, string To)
    {
        this.From = From;
        this.To = To;
    }

    public string From { get; set; }
    public string To { get; set; }
}