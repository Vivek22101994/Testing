using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Web.UI.WebControls;
using RentalInRome.data;
using RentalInRome.WS_RiKenya;

public class RiKenya_WS
{
    private static string mode;
    public static void fillRewriteTool()
    {
        //Action<object> action = (object obj) => { Thread_fillRewriteTool(); };
        //AppUtilsTaskScheduler.AddTask(action, "fillRewriteTool");

        ThreadStart start = new ThreadStart(Thread_fillRewriteTool);
        Thread t = new Thread(start);
        t.Priority = ThreadPriority.Lowest;
        t.Start();
        
    }
    public static void Thread_fillRewriteTool()
    {
        try
        {
            AuthHeader auth = new AuthHeader();
            auth.Username = "Rir";
            auth.Password = "Fer90PLkir3W£_,MR";
            WS _location = new WS();
            _location.AuthHeaderValue = auth;
            _location.fillRewriteTool();
        }
        catch (Exception ex) { ErrorLog.addLog("", "RiKenya_WS.fillRewriteTool", ex.ToString()); }
    }
    public static void refreshCache(string Mode)
    {
        mode = Mode;
        //Action<object> action = (object obj) => { Thread_refreshCache(); };
        //AppUtilsTaskScheduler.AddTask(action, "refreshCache");
        ThreadStart start = new ThreadStart(Thread_refreshCache);
        Thread t = new Thread(start);
        t.Priority = ThreadPriority.Lowest;
        t.Start();
        
    }
    public static void Thread_refreshCache()
    {
        try
        {
            AuthHeader auth = new AuthHeader();
            auth.Username = "Rir";
            auth.Password = "Fer90PLkir3W£_,MR";
            WS _location = new WS();
            _location.AuthHeaderValue = auth;
            _location.refreshCache(mode);
        }
        catch (Exception ex) { ErrorLog.addLog("", "RiKenya_WS.refreshCache", ex.ToString()); }
    }
    public static void rntReservation_onChange(long ResId)
    {
        try
        {
            AuthHeader auth = new AuthHeader();
            auth.Username = "Rir";
            auth.Password = "Fer90PLkir3W£_,MR";
            WS _location = new WS();
            _location.AuthHeaderValue = auth;
            _location.rntReservation_onChange(ResId);
        }
        catch (Exception ex) { ErrorLog.addLog("", "RiKenya_WS.rntReservation_onChange " + ResId, ex.ToString()); }
    }
}
