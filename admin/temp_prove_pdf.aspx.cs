using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EO.Pdf;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class temp_prove_pdf : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PDF_check();
        }

        protected void lnk_create_Click(object sender, EventArgs e)
        {
            createPDF();
            PDF_check();
        }
        protected void PDF_check()
        {
        }
        public void createPDF()
        {
            EO.Pdf.Runtime.AddLicense(CurrentAppSettings.EO_PDF_LICENSE);
            string url1 = txt_content.Text;
            //string url1 = CurrentAppSettings.HOST + "/pdfgenerator/pdf_rnt_reservation_voucher.aspx";
            SizeF pageSize = PdfPageSizes.A4;
            float marginLeft = 0.3f;
            float marginTop = 1;
            float marginRight = 0.3f;
            float marginBottom = 0.5f;
            bool autoFitWidth = true;

            //Set page layout arguments
            HtmlToPdf.Options.PageSize = pageSize;
            HtmlToPdf.Options.OutputArea = new RectangleF(
                marginLeft, marginTop,
                pageSize.Width - marginLeft - marginRight,
                pageSize.Height - marginTop - marginBottom);
           // HtmlToPdf.Options.AutoFitWidth = autoFitWidth;

            PdfDocument doc = new PdfDocument();


            //Convert the first Url into the PdfDocument object
            HtmlToPdf.Options.NoScript = true;
            HtmlToPdf.Options.NoCache = true;
            HtmlToPdf.Options.NoLink = true;
            HtmlToPdf.Options.StartPosition = 0;
            HtmlToPdf.Options.ProxyInfo = EO.Base.ProxyInfo.Direct;
            //Stream fileStream = Stream
            //PdfDocInfo _info = doc.Save()
            //Save the PDF file
            // get the pdf bytes from html string
            //byte[] downloadBytes = doc.Bookmarks.GetPdfBytesFromHtmlString(htmlCodeToConvert, baseUrl);
            
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Type", "binary/octet-stream");
            response.AddHeader("Content-Disposition",
                "attachment; filename=Report.pdf;");
            response.Flush();
            HtmlToPdf.Options.Follow(HtmlToPdf.ConvertUrl(url1, response.OutputStream));
            //doc.Save(response.OutputStream);
            //response.BinaryWrite(downloadBytes);
            response.Flush();
            response.End();
        }
    }
}
