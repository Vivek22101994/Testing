using System.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ModAuth;

public class ErrorLog
{
    public static void addLog(string ip, string url, string message)
    {
        try
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                var item = new dbAuthErrorLOG();
                item.uid = Guid.NewGuid();
                item.logIp = ip;
                item.logUrl = url;
                item.errorContent = message;
                item.logDateTime = DateTime.Now;
                dc.Add(item);
                dc.SaveChanges();
            }
        }
        catch (Exception Ex)
        {
            addLogToFile(ip, "addLog", Ex.ToString() + "\n\n______________________\n\nURL: " + url);
        }
    }
    public static void addEcoLog(string ip, string url, string message)
    {
        try
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                var item = new dbAuthErrorEcoLOG();
                item.uid = Guid.NewGuid();
                item.logIp = ip;
                item.logUrl = url;
                item.errorContent = message;
                item.logDateTime = DateTime.Now;
                dc.Add(item);
                dc.SaveChanges();
            }
        }
        catch (Exception Ex)
        {
            addLogToFile(ip, "addLog", Ex.ToString() + "\n\n______________________\n\nURL: " + url);
        }
    }
    public static void addSrsLog(string ip, string url, string message)
    {
        try
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                var item = new dbAuthErrorSrsLOG();
                item.uid = Guid.NewGuid();
                item.logIp = ip;
                item.logUrl = url;
                item.errorContent = message;
                item.logDateTime = DateTime.Now;
                dc.Add(item);
                dc.SaveChanges();
            }
        }
        catch (Exception Ex)
        {
            addLogToFile(ip, "addLog", Ex.ToString() + "\n\n______________________\n\nURL: " + url);
        }
    }
    public static void addLogToFile(string ip, string url, string message)
    {
        try
        {
            string _fileName = DateTime.Now.JSCal_dateToString() + ".xml";
            var _list = new LogList(_fileName);
            LogItem _item = new LogItem(ip, url, message, DateTime.Now.JSCal_dateTimeToString());
            _list.Items.Add(_item);
            _list.Save();
        }
        catch (Exception Ex)
        { 
        }
    }
}
public class LogList
{
    private string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/errors");
    public string _fileName;
    private List<LogItem> _items;

    public List<LogItem> Items { get { if (this._items != null)return this._items; else return new List<LogItem>(); } set { this._items = value; } }
    private void fillList()
    {
        this._items = new List<LogItem>();
        try
        {
            if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log"))) Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log"));
            if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/errors"))) Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/errors"));
            if (!File.Exists(Path.Combine(this._path, _fileName))) return;
            XDocument _resource = XDocument.Load(Path.Combine(this._path, _fileName));
            var ds = from XElement e in _resource.Descendants("server")
                     select e;
            foreach (XElement e in ds)
            {
                LogItem item = new LogItem();
                item.ID = e.Element("id").Value;
                item.IP = e.Element("ip").Value.htmlDecode();
                item.Url = e.Element("url").Value.htmlDecode();
                item.Value = e.Element("value").Value.htmlDecode();
                item.Date = e.Element("date").Value.htmlDecode();
                this._items.Add(item);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public LogList()
    {
        _fileName = DateTime.Now.JSCal_dateToString() + ".xml";
        fillList();
    }
    public LogList(string fileName)
    {
        _fileName = fileName;
        fillList();
    }
    public void Save()
    {
        try
        {
            XDocument _resource = new XDocument();
            XElement rootElement = new XElement("list");
            foreach (LogItem item in this._items)
            {
                XElement record = new XElement("server");
                record.Add(new XElement("id", item.ID));
                record.Add(new XElement("ip", item.IP.htmlEncode()));
                record.Add(new XElement("url", item.Url.htmlEncode()));
                record.Add(new XElement("value", item.Value.htmlEncode()));
                record.Add(new XElement("date", item.Date.htmlEncode()));
                rootElement.Add(record);
            }
            _resource.Add(rootElement);
            try
            {
                _resource.Save(Path.Combine(this._path, _fileName));
            }
            catch(Exception ex)
            {}
        }
        catch (Exception ex)
        {
        }
    }
}
public class LogItem
{
    public string ID { get; set; }
    public string IP { get; set; }
    public string Url { get; set; }
    public string Value { get; set; }
    public string Date { get; set; }
    public LogItem()
    {
        ID = Guid.NewGuid().ToString();
        IP = "";
        Url = "";
        Value = "";
        Date = DateTime.Now.JSCal_dateTimeToString();
    }
    public LogItem(string _IP, string _Url, string _Value, string _Date)
    {
        ID = Guid.NewGuid().ToString();
        IP = _IP;
        Url = _Url;
        Value = _Value;
        Date = _Date;
    }
}
