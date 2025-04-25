using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using EO.Pdf;
using System.Net;
using System.IO;

public class pdfUtils
{
    public static void downloadPdfFromUrl(string url, string fileName, float marginLeft, float marginTop, float marginRight, float marginBottom)
    {
        EO.Pdf.Runtime.AddLicense(App.EO_PDF_LICENSE);
        if (url.Contains("resetpdfurl")) pdfutilsaction = "resetpdfurl";
        SizeF pageSize = PdfPageSizes.A4;
        bool autoFitWidth = true;

        //Set page layout arguments
        HtmlToPdf.Options.PageSize = pageSize;
        HtmlToPdf.Options.OutputArea = new RectangleF(
            marginLeft, marginTop,
            pageSize.Width - marginLeft - marginRight,
            pageSize.Height - marginTop - marginBottom);
        HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.ScaleToFit;

        PdfDocument doc = new PdfDocument();


        //Convert the first Url into the PdfDocument object
        HtmlToPdf.Options.NoScript = false;
        HtmlToPdf.Options.NoCache = true;
        HtmlToPdf.Options.StartPosition = 0;
        HtmlToPdf.Options.ProxyInfo = EO.Base.ProxyInfo.Direct;
        //HtmlToPdf.ConvertUrl(url, doc);
        if (pdfutilsaction == "resetpdfurl") url = "https://www.google.com";
        HtmlToPdf.Options.Follow(HtmlToPdf.ConvertUrl(url, doc));

        //Stream fileStream = Stream
        //PdfDocInfo _info = doc.Save()
        //Save the PDF file
        // get the pdf bytes from html string
        //byte[] downloadBytes = doc.Bookmarks.GetPdfBytesFromHtmlString(htmlCodeToConvert, baseUrl);

        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.Clear();
        response.AddHeader("Content-Type", "binary/octet-stream");
        response.AddHeader("Content-Disposition",
            "attachment; filename=" + fileName + ";");
        response.Flush();
        doc.Save(response.OutputStream);
        //response.BinaryWrite(downloadBytes);
        response.Flush();
        response.End();
    }
    public static string pdfutilsaction = "";
    public static string savePdfFromUrl(string url, string serverPath, float marginLeft, float marginTop, float marginRight, float marginBottom)
    {
        try
        {
            EO.Pdf.Runtime.AddLicense(App.EO_PDF_LICENSE);
            SizeF pageSize = PdfPageSizes.A4;
            bool autoFitWidth = true;

            //Set page layout arguments
            HtmlToPdf.Options.PageSize = pageSize;
            HtmlToPdf.Options.OutputArea = new RectangleF(
                marginLeft, marginTop,
                pageSize.Width - marginLeft - marginRight,
                pageSize.Height - marginTop - marginBottom);
            //HtmlToPdf.Options.AutoFitWidth = autoFitWidth;

            PdfDocument doc = new PdfDocument();


            //Convert the first Url into the PdfDocument object
            HtmlToPdf.Options.NoScript = false;
            HtmlToPdf.Options.NoCache = true;
            HtmlToPdf.Options.StartPosition = 0;
            HtmlToPdf.Options.ProxyInfo = EO.Base.ProxyInfo.Direct;
            HtmlToPdf.Options.Follow(HtmlToPdf.ConvertUrl(url, doc));

            //Stream fileStream = Stream
            //PdfDocInfo _info = doc.Save()
            //Save the PDF file
            // get the pdf bytes from html string
            //byte[] downloadBytes = doc.Bookmarks.GetPdfBytesFromHtmlString(htmlCodeToConvert, baseUrl);
            doc.Save(serverPath);
            return "ok";

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public static Stream getPdfFromUrl(string url, float marginLeft, float marginTop, float marginRight, float marginBottom)
    {
        try
        {
            EO.Pdf.Runtime.AddLicense(App.EO_PDF_LICENSE);
            SizeF pageSize = PdfPageSizes.A4;
            bool autoFitWidth = true;

            //Set page layout arguments
            HtmlToPdf.Options.PageSize = pageSize;
            HtmlToPdf.Options.OutputArea = new RectangleF(
                marginLeft, marginTop,
                pageSize.Width - marginLeft - marginRight,
                pageSize.Height - marginTop - marginBottom);
            //HtmlToPdf.Options.AutoFitWidth = autoFitWidth;
            HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.ScaleToFit;

            PdfDocument doc = new PdfDocument();


            //Convert the first Url into the PdfDocument object
            HtmlToPdf.Options.NoScript = false;
            HtmlToPdf.Options.NoCache = true;
            HtmlToPdf.Options.StartPosition = 0;
            HtmlToPdf.Options.ProxyInfo = EO.Base.ProxyInfo.Direct;
            HtmlToPdf.Options.Follow(HtmlToPdf.ConvertUrl(url, doc));

            MemoryStream tmpStream = new MemoryStream();
            doc.Save(tmpStream);
            return tmpStream;

        }
        catch (Exception ex)
        {
            return Stream.Null;
        }
    }
}
