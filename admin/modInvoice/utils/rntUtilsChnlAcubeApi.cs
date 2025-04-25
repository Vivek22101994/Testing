using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ModInvoice;
using ModRental;
using System.Net.Security;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Converters;
using System.Xml.Linq;
using ModAuth;
using HtmlAgilityPack;
using RentalInRome.data;


public class ChnlAcubeApiUtils
{
    public static bool CheckForService()
    {
        if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Response == null) return false;
        var Request = HttpContext.Current.Request;
        var Response = HttpContext.Current.Response;
        if (!Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/acubapi/")) return false;
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetAllowResponseInBrowserHistory(true);
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/json";
        byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
        string json = Encoding.ASCII.GetString(param);
        var requestType = "";
        var requestComments = "";
        var requestContent = json;
        var responseContent = "";
        var requesUrl = "";
        if (Request.Url.LocalPath.ToLower().StartsWith("/chnlutils/acubapi/notification"))
        {
            requestType = "notification";
            responseContent = ChnlAcubeApiService.ReceiveNotifications(json);
        }

        //if (CommonUtilities.getSYS_SETTING("rntChnlacubapiDebug") == "true" || CommonUtilities.getSYS_SETTING("rntChnlacubapiDebug").ToInt32() == 1)
        //addLog(requestType, requestComments, requestContent, responseContent, requesUrl);
        Response.Write(responseContent);
        Response.End();
        return true;
    }
}

public class ChnlAcubeApiService
{
    public static string ReceiveNotifications(string json)
    {
        ErrorLog.addLog("Received json", "ChnlAcubeApiUtils.ReceiveNotifications", json);
        string responseData = "";
        string exstr = "";
        AcubeApiClasses.ErrorList errorList = new AcubeApiClasses.ErrorList("ReceiveNotifications");
        if (json == null)
        {
            errorList.AddError("json is null");
            ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications", "json is null");
            responseData = JsonConvert.SerializeObject(errorList);
            return responseData;
        }
        try
        {

            #region Response1
            {
                AcubeApiClasses.Response1.RootObjectNotification RootObject = JsonConvert.DeserializeObject<AcubeApiClasses.Response1.RootObjectNotification>(json);
                if (RootObject == null)
                {
                    errorList.AddError("RootObject not found!");
                    ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications", "RootObject not found!");
                    responseData = JsonConvert.SerializeObject(errorList);
                    return responseData;
                }
                AcubeApiClasses.Response1.notification jsonNotification = RootObject.notification;

                if (jsonNotification != null)
                {
                    string Message = "";
                    if (jsonNotification.message != null)
                    {
                        if (!string.IsNullOrWhiteSpace(jsonNotification.message.descrizione))
                        {
                            Message += "Message1  " + jsonNotification.message.descrizione;
                        }

                        if (jsonNotification.message.lista_errori != null)
                        {
                            if (jsonNotification.message.lista_errori.Errore != null)
                            {
                                int i = 1;
                                foreach (var _error in jsonNotification.message.lista_errori.Errore)
                                {
                                    Message += "Error" + i + "  " + _error.Descrizione + "    ";
                                    Message += "Suggestion" + i + "  " + _error.suggerimento + "    ";
                                    i++;
                                }
                            }
                        }
                    }







                    using (magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE)
                    {
                        if (DC_INVOICE.INV_TBL_INVOICE.Any(i => i.responseUniqueId == jsonNotification.invoice_uuid))
                        {
                            var INV_TBL_INVOICE = DC_INVOICE.INV_TBL_INVOICE.FirstOrDefault(i => i.responseUniqueId == jsonNotification.invoice_uuid);
                            if (INV_TBL_INVOICE != null)
                            {

                                var currNotification = DC_INVOICE.INV_TBL_INVOICE_NOTIFICATIONs.FirstOrDefault(i => i.invoice_uuid == jsonNotification.invoice_uuid);
                                if (currNotification == null)
                                {
                                    currNotification = new INV_TBL_INVOICE_NOTIFICATION();
                                    currNotification.pid_invoice = INV_TBL_INVOICE.id;
                                    currNotification.uuid = jsonNotification.uuid;
                                    currNotification.invoice_uuid = jsonNotification.invoice_uuid;
                                    currNotification.type = jsonNotification.type;
                                    currNotification.message = Message;
                                    if (!string.IsNullOrWhiteSpace(jsonNotification.created_at))
                                    {
                                        DateTime dateValue;

                                        if (DateTime.TryParseExact(jsonNotification.created_at, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture,
                                     System.Globalization.DateTimeStyles.None, out dateValue))
                                        {
                                            currNotification.created_at = dateValue;

                                        }
                                        else if (DateTime.TryParseExact(jsonNotification.created_at, "yyyy-MM-ddTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture,
                                     System.Globalization.DateTimeStyles.None, out dateValue))
                                        {
                                            //2019-08-21T06:49:41+00:00
                                            currNotification.created_at = dateValue;

                                        }
                                        else if (DateTime.TryParse(jsonNotification.created_at, out dateValue))
                                        {
                                            //2019-08-21T06:49:41+00:00
                                            currNotification.created_at = dateValue;

                                        }
                                        //currNotification.created_at = Convert.ToDateTime(jsonNotification.created_at);
                                    }
                                    DC_INVOICE.INV_TBL_INVOICE_NOTIFICATIONs.InsertOnSubmit(currNotification);
                                    DC_INVOICE.SubmitChanges();
                                }
                                else
                                {
                                    currNotification.pid_invoice = INV_TBL_INVOICE.id;
                                    currNotification.uuid = jsonNotification.uuid;
                                    currNotification.invoice_uuid = jsonNotification.invoice_uuid;
                                    currNotification.type = jsonNotification.type;
                                    currNotification.message = Message;

                                    if (!string.IsNullOrWhiteSpace(jsonNotification.created_at))
                                    {
                                        DateTime dateValue;

                                        if (DateTime.TryParseExact(jsonNotification.created_at, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture,
                                     System.Globalization.DateTimeStyles.None, out dateValue))
                                        {
                                            currNotification.created_at = dateValue;

                                        }
                                        else if (DateTime.TryParseExact(jsonNotification.created_at, "yyyy-MM-ddTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture,
                                     System.Globalization.DateTimeStyles.None, out dateValue))
                                        {
                                            currNotification.created_at = dateValue;

                                        }
                                        else if (DateTime.TryParse(jsonNotification.created_at, out dateValue))
                                        {
                                            currNotification.created_at = dateValue;

                                        }
                                    }

                                    DC_INVOICE.SubmitChanges();
                                }

                            }

                        }
                        else
                        {
                            errorList.AddError("INVOICE not found!");
                            //ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications", "INVOICE not found!");
                        }
                    }
                }
                else
                {
                    errorList.AddError("notification is null");
                    //ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications", "notification is null");
                }
            }
            #endregion

        }
        catch (Exception ex)
        {

            errorList.AddError("OTHER");
            exstr = ex.Message;
            if (ex.InnerException != null)
            {
                exstr += ex.InnerException.Message;
            }
            errorList.AddError(exstr);
            ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications", exstr);
            ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications Exception Json", json);


        }

        if (exstr.Contains("Cannot deserialize JSON object into type")) //'Cannot deserialize JSON object into type 'AcubeApiClasses+RootErrore[]'.'
        {
            #region Response2

            try
            {

                AcubeApiClasses.Response2.RootObjectNotification RootObject2 = JsonConvert.DeserializeObject<AcubeApiClasses.Response2.RootObjectNotification>(json);
                if (RootObject2 == null)
                {
                    errorList.AddError("RootObject2 not found!");
                    ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications", "RootObject not found!");
                    responseData = JsonConvert.SerializeObject(errorList);
                    return responseData;
                }
                AcubeApiClasses.Response2.notification jsonNotification2 = RootObject2.notification;

                if (jsonNotification2 != null)
                {
                    string Message = "";
                    if (jsonNotification2.message != null)
                    {
                        if (!string.IsNullOrWhiteSpace(jsonNotification2.message.descrizione))
                        {
                            Message += "Message1  " + jsonNotification2.message.descrizione;
                        }

                        if (jsonNotification2.message.lista_errori != null)
                        {
                            if (jsonNotification2.message.lista_errori.Errore != null)
                            {
                                int i = 1;
                                Message += "Error" + i + "  " + jsonNotification2.message.lista_errori.Errore.Descrizione + "    ";
                                Message += "Suggestion" + i + "  " + jsonNotification2.message.lista_errori.Errore.suggerimento + "    ";
                            }
                        }
                    }

                    using (magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE)
                    {
                        if (DC_INVOICE.INV_TBL_INVOICE.Any(i => i.responseUniqueId == jsonNotification2.invoice_uuid))
                        {
                            var INV_TBL_INVOICE = DC_INVOICE.INV_TBL_INVOICE.FirstOrDefault(i => i.responseUniqueId == jsonNotification2.invoice_uuid);
                            if (INV_TBL_INVOICE != null)
                            {

                                var currNotification = DC_INVOICE.INV_TBL_INVOICE_NOTIFICATIONs.FirstOrDefault(i => i.invoice_uuid == jsonNotification2.invoice_uuid);
                                if (currNotification == null)
                                {
                                    currNotification = new INV_TBL_INVOICE_NOTIFICATION();
                                    currNotification.pid_invoice = INV_TBL_INVOICE.id;
                                    currNotification.uuid = jsonNotification2.uuid;
                                    currNotification.invoice_uuid = jsonNotification2.invoice_uuid;
                                    currNotification.type = jsonNotification2.type;
                                    currNotification.message = Message;
                                    if (!string.IsNullOrWhiteSpace(jsonNotification2.created_at))
                                    {
                                        DateTime dateValue;

                                        if (DateTime.TryParseExact(jsonNotification2.created_at, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture,
                                     System.Globalization.DateTimeStyles.None, out dateValue))
                                        {
                                            currNotification.created_at = dateValue;

                                        }
                                        else if (DateTime.TryParseExact(jsonNotification2.created_at, "yyyy-MM-ddTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture,
                                     System.Globalization.DateTimeStyles.None, out dateValue))
                                        {
                                            //2019-08-21T06:49:41+00:00
                                            currNotification.created_at = dateValue;

                                        }
                                        else if (DateTime.TryParse(jsonNotification2.created_at, out dateValue))
                                        {
                                            //2019-08-21T06:49:41+00:00
                                            currNotification.created_at = dateValue;

                                        }
                                        //currNotification.created_at = Convert.ToDateTime(jsonNotification.created_at);
                                    }
                                    DC_INVOICE.INV_TBL_INVOICE_NOTIFICATIONs.InsertOnSubmit(currNotification);
                                    DC_INVOICE.SubmitChanges();
                                }
                                else
                                {
                                    currNotification.pid_invoice = INV_TBL_INVOICE.id;
                                    currNotification.uuid = jsonNotification2.uuid;
                                    currNotification.invoice_uuid = jsonNotification2.invoice_uuid;
                                    currNotification.type = jsonNotification2.type;
                                    currNotification.message = Message;

                                    if (!string.IsNullOrWhiteSpace(jsonNotification2.created_at))
                                    {
                                        DateTime dateValue;

                                        if (DateTime.TryParseExact(jsonNotification2.created_at, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture,
                                     System.Globalization.DateTimeStyles.None, out dateValue))
                                        {
                                            currNotification.created_at = dateValue;

                                        }
                                        else if (DateTime.TryParseExact(jsonNotification2.created_at, "yyyy-MM-ddTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture,
                                     System.Globalization.DateTimeStyles.None, out dateValue))
                                        {
                                            currNotification.created_at = dateValue;

                                        }
                                        else if (DateTime.TryParse(jsonNotification2.created_at, out dateValue))
                                        {
                                            currNotification.created_at = dateValue;

                                        }
                                    }

                                    DC_INVOICE.SubmitChanges();
                                }

                            }

                        }
                        else
                        {
                            errorList.AddError("INVOICE not found!");
                            //ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications", "INVOICE not found!");
                        }
                    }
                }
                else
                {
                    errorList.AddError("notification is null");
                    //ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications", "notification is null");
                }

            }
            catch (Exception ex)
            {

                errorList.AddError("OTHER");
                exstr = ex.Message;
                if (ex.InnerException != null)
                {
                    exstr += ex.InnerException.Message;
                }
                errorList.AddError(exstr);
                ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications", exstr);
                ErrorLog.addLog("", "ChnlAcubeApiUtils.ReceiveNotifications Exception Json", json);
            }
            #endregion
        }
        responseData = JsonConvert.SerializeObject(errorList);
        return responseData;
    }
}

public class AcubeApiClasses
{

    public class Response1
    {
        public class notification
        {
            public string uuid { get; set; }
            public string invoice_uuid { get; set; }
            public string created_at { get; set; }
            public string type { get; set; }
            public message message { get; set; }

        }
        public class message
        {
            public string descrizione { get; set; }
            public lista_errori lista_errori { get; set; }
        }
        public class lista_errori
        {
            public RootErrore[] Errore { get; set; }
        }
        public class RootErrore
        {
            public string Descrizione { get; set; }
            public string suggerimento { get; set; }
        }
        public class RootObjectNotification
        {
            public notification notification { get; set; }
            //public object invoice { get; set; } 

        }
    }

    public class Response2
    {
        public class notification
        {
            public string uuid { get; set; }
            public string invoice_uuid { get; set; }
            public string created_at { get; set; }
            public string type { get; set; }
            public message message { get; set; }

        }
        public class message
        {
            public string descrizione { get; set; }
            public lista_errori lista_errori { get; set; }
        }
        public class lista_errori
        {
            public RootErrore Errore { get; set; }
        }
        public class RootErrore
        {
            public string Descrizione { get; set; }
            public string suggerimento { get; set; }
        }
        public class RootObjectNotification
        {
            public notification notification { get; set; }
            //public object invoice { get; set; } 

        }
    }


    #region Error
    public class ErrorList
    {
        public string root { get; set; } // max 1000
        public List<Error> errorList { get; set; }

        public ErrorList(string Root)
        {
            root = Root;
            errorList = new List<Error>();
        }
        public void AddError(string ErrorType)
        {
            var error = new Error();
            error.errorType = ErrorType;
            errorList.Add(error);
        }

    }
    public class Error
    {
        public int? count { get; set; }
        public string dayOfWeek { get; set; } // SUN, MON,TUE,WED, THU, FRI,SAT
        public string errorCode { get; set; } // 
        public string errorType { get; set; } // SEE TYPES
        public string errorMessage { get; set; } //
        public Error()
        {

        }
    }
    #endregion

}

