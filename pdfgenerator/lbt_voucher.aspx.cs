using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using EO.Pdf;
using System.Drawing;

namespace RentalInRome.pdfgenerator
{
    public partial class lbt_voucher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] == "sconto10")
            {
                Guid uid;
                if(!Guid.TryParse(Request.QueryString["uid"], out uid))return;
                var currReservation = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.uid_2 == uid);
                if (currReservation == null) return;
                string fileName = "lbt-voucher-sconto10_" + currReservation.code + ".pdf";
                string content = currReservation.cl_pid_lang == 1 ? ltrSconto10_it.Text : ltrSconto10_en.Text;
                content = content.Replace("#fullName#", "" + currReservation.cl_name_full);
                content = content.Replace("#code#", "" + currReservation.code);
                EO.Pdf.Runtime.AddLicense(CurrentAppSettings.EO_PDF_LICENSE);
                SizeF pageSize = PdfPageSizes.A4;
                bool autoFitWidth = true;

                //Set page layout arguments
                float marginLeft = 0.3f; float marginTop = 0.3f; float marginRight = 0.3f; float marginBottom = 0.3f;
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
                //HtmlToPdf.ConvertUrl(url, doc);
                HtmlToPdf.Options.Follow(HtmlToPdf.ConvertHtml(content, doc));

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
        }
    }
}
