using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using System.Drawing;
using EO.Pdf;
using System.Threading;
using System.Text;

namespace RentalInRome.admin
{

    public partial class inv_invoice_export : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "inv_invoice";
        }
        protected string exportName
        {
            get
            {
                return HF_type.Value;
            }
            set { HF_type.Value = value; }
        }
        protected ExportItem _currTBL;
        protected ExportList _exportList
        {
            get
            {
                return new ExportList(exportName);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["type"] == "2")
                {
                    exportName = "exp2";
                }
                fillList();
                if (Request.QueryString["uid"] != null)
                {
                    HF_id.Value = Request.QueryString["uid"];
                    FillControls();
                }
            }
        }
        protected void fillList()
        {
            LV.DataSource = _exportList.Items.OrderByDescending(x => x.Date);
            LV.DataBind();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "seleziona")
            {
                var lbl_id = (Label)e.Item.FindControl("lbl_id");
                HF_id.Value = lbl_id.Text;
                FillControls();
            }
        }

        private void FillControls()
        {
            _currTBL = _exportList.Items.SingleOrDefault(x => x.ID == HF_id.Value);
            if (_currTBL == null)
            {
                LV.SelectedIndex = -1;
                fillList();
                pnlContent.Visible = false;
                return;
            }
            HF_fileName.Value = _currTBL.FileName;
            ltr_name.Text = _currTBL.Name;
            ltr_date.Text = _currTBL.Date.ToString();
            ltr_count.Text = _currTBL.InvoiceIDsCount.ToString();
            ltr_countMail.Text = _currTBL.MailSentCount.ToString();
            pnlGetPdf.Visible = File.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/invoice_export/files/" + _currTBL.FileName));
            pnlContent.Visible = true;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scrolTo", "$.scrollTo($(\"#" + pnlContent.ClientID + "\"), 500);", true);

        }
        private void FillDataFromControls()
        {
            ExportList _list = _exportList;
            _currTBL = _list.Items.SingleOrDefault(x => x.ID == HF_id.Value);
            if (_currTBL != null)
            {
                string _link = CurrentAppSettings.HOST + "/log/invoice_export/files/" + _currTBL.FileName;
                //string serverPath = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/invoice_export/files/" + _currTBL.FileName);
                string _body = "";
                _body += "sono state esportate " + _currTBL.InvoiceIDsCount + " fatture<br/>";
                _body += "<a href=\"" + _link + "\" target=\"_blank\">scarica il file</a><br/>";
                if (MailingUtilities.SendMultiMail("Esportazione Fatture RiR " + _currTBL.Name, _body, new List<string>() { HF_mail_1.Value, HF_mail_2.Value }, new List<string>() { "maurizio.lecce@rentalinrome.com" }, new List<mailUtils.AttachmentItem>() { }, true, "Esportazione Fatture RiR"))
                    _currTBL.MailSentCount = _currTBL.MailSentCount + 1;
                _list.Save();
            }

            LV.SelectedIndex = -1;
            fillList();
            pnlContent.Visible = false;
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LV.SelectedIndex = -1;
            fillList();
            pnlContent.Visible = false;
        }
        protected void lnk_createPdf_Click(object sender, EventArgs e)
        {
            ExportList _list = _exportList;
            _currTBL = _list.Items.SingleOrDefault(x => x.ID == HF_id.Value);
            if (_currTBL == null) return;
            if (exportName == "exp1")
            {
                _currTBL.createPdf();
                _list.Save();
            }
            else if (exportName == "exp2")
            {
                _currTBL.createCsv_CoGe();
                _list.Save();
                pnlGetPdf.Visible = File.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/invoice_export/files/" + _currTBL.FileName));
            }
        }


    }
    public class ExportList
    {
        private string _path = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/invoice_export");
        public string _fileName;
        public List<ExportItem> Items { get; set; }

        private void fillList()
        {
            this.Items = new List<ExportItem>();
            try
            {
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log"))) Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log"));
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/invoice_export"))) Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/invoice_export"));
                if (!Directory.Exists(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/invoice_export/files"))) Directory.CreateDirectory(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, "log/invoice_export/files"));
                if (!File.Exists(Path.Combine(this._path, _fileName))) return;
                XDocument _resource = XDocument.Load(Path.Combine(this._path, _fileName));
                var ds = from XElement e in _resource.Descendants("item")
                         select e;
                foreach (XElement e in ds)
                {
                    ExportItem item = new ExportItem();
                    item.ID = e.Element("ID").Value;
                    item.Creator = e.Element("Creator").Value.htmlDecode();
                    item.Name = e.Element("Name").Value.htmlDecode();
                    item.FileName = e.Element("FileName").Value.htmlDecode();
                    item.Date = e.Element("Date").Value.JSCal_stringToDateTime();
                    item.InvoiceIDsList = e.Element("InvoiceIDsList").Value.splitStringToList("|").Select(x => x.ToInt64()).ToList();
                    item.InvoiceIDsCount = e.Element("InvoiceIDsCount").Value.ToInt32();
                    item.MailSentCount = e.Element("MailSentCount").Value.ToInt32();
                    this.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public ExportList(string fileName)
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
                foreach (ExportItem item in this.Items)
                {
                    XElement record = new XElement("item");
                    record.Add(new XElement("ID", item.ID));
                    record.Add(new XElement("Creator", item.Creator.htmlEncode()));
                    record.Add(new XElement("Name", item.Name.htmlEncode()));
                    record.Add(new XElement("FileName", item.FileName.htmlEncode()));
                    record.Add(new XElement("Date", item.Date.JSCal_dateTimeToString()));
                    record.Add(new XElement("InvoiceIDsList", item.InvoiceIDsList.Select(x => x.ToString()).ToList().listToString("|")));
                    record.Add(new XElement("InvoiceIDsCount", item.InvoiceIDsCount));
                    record.Add(new XElement("MailSentCount", item.MailSentCount));
                    rootElement.Add(record);
                }
                _resource.Add(rootElement);
                try
                {
                    _resource.Save(Path.Combine(this._path, _fileName));
                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            {
            }
        }
    }
    public class ExportItem
    {
        public string ID { get; set; }
        public string Creator { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public List<long> InvoiceIDsList { get; set; }
        public int InvoiceIDsCount { get; set; }
        public int MailSentCount { get; set; }
        public ExportItem()
        {
            ID = Guid.NewGuid().ToString();
            Creator = "";
            Name = "";
            FileName = "";
            Date = DateTime.Now;
            InvoiceIDsList = new List<long>();
            InvoiceIDsCount = InvoiceIDsList.Count;
            MailSentCount = 0;
        }
        public ExportItem(string _Creator, string _Name, string _FileName, DateTime _Date)
        {
            ID = Guid.NewGuid().ToString();
            Creator = _Creator;
            Name = _Name;
            FileName = _FileName;
            Date = _Date;
            InvoiceIDsList = new List<long>();
            InvoiceIDsCount = InvoiceIDsList.Count;
            MailSentCount = 0;
        }
        public void createPdf()
        {
            SRP = CurrentAppSettings.SERVER_ROOT_PATH;
            HOST = CurrentAppSettings.HOST;

            //Action<object> action = (object obj) => { createPdf_Thread(); };
            //AppUtilsTaskScheduler.AddTask(action, "createPdf");
            ThreadStart start = new ThreadStart(createPdf_Thread);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            
        }
        private string SRP;
        private string HOST;
        public void createPdf_Thread()
        {
            try
            {
                List<INV_TBL_INVOICE> _list = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE.Where(x => InvoiceIDsList.Contains(x.id)).ToList();
                string serverPath = Path.Combine(SRP, "log/invoice_export/files/" + FileName);
                string _urlBase = HOST + "/pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=";

                EO.Pdf.Runtime.AddLicense(CurrentAppSettings.EO_PDF_LICENSE);
                SizeF pageSize = PdfPageSizes.A4;
                bool autoFitWidth = true;

                //Set page layout arguments
                HtmlToPdf.Options.PageSize = pageSize;
                HtmlToPdf.Options.OutputArea = new RectangleF(
                    0.3f, 0.3f,
                    pageSize.Width - 0.3f - 0.3f,
                    pageSize.Height - 0.3f - 0.3f);
                //HtmlToPdf.Options.AutoFitWidth = autoFitWidth;
                HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.ScaleToFit;

                PdfDocument doc = new PdfDocument();


                //Convert the first Url into the PdfDocument object
                HtmlToPdf.Options.NoScript = false;
                HtmlToPdf.Options.NoCache = true;
                HtmlToPdf.Options.StartPosition = 0;
                HtmlToPdf.Options.ProxyInfo = EO.Base.ProxyInfo.Direct;
                foreach (INV_TBL_INVOICE _inv in _list)
                {
                    EO.Pdf.HtmlToPdf.ConvertUrl(_urlBase + _inv.uid, doc);
                }

                //Stream fileStream = Stream
                //PdfDocInfo _info = doc.Save()
                //Save the PDF file
                // get the pdf bytes from html string
                //byte[] downloadBytes = doc.Bookmarks.GetPdfBytesFromHtmlString(htmlCodeToConvert, baseUrl);
                doc.Save(serverPath);
            }
            catch (Exception ex)
            { return; }
            return;
        }
        public void createCsv_CoGe()
        {
            //try
            //{
            SRP = CurrentAppSettings.SERVER_ROOT_PATH;
            HOST = CurrentAppSettings.HOST;
            List<INV_TBL_INVOICE> _list = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE.Where(x => InvoiceIDsList.Contains(x.id)).ToList();
            string serverPath = Path.Combine(SRP, "log/invoice_export/files/" + FileName);
            StreamWriter _writer = new StreamWriter(serverPath, false, Encoding.UTF8);
            var countryList = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.ToList();
            var _str = "";
            foreach (var tbl in _list)
            {
                _str = "1"; // RECORD_TESTA
                _str += ";400"; // Codice_ditta_corrente
                _str += ";" + tbl.inv_dtInvoice.formatCustom("#dd##mm##yy#", 1, ""); // Data_registrazione
                _str += ";1"; // Codice_causale - 1=FATTURA VENDITA, 5=NOTA CREDITO, 61=FATTURA VENDITA CEE, 63=NOTA CREDITO CEE
                _str += ";00"; // Codice_sezionale
                _str += ";" + tbl.inv_counter; // Numero_documento
                _str += ";" + tbl.inv_counter; // Numero_documento_origine
                _str += ";" + tbl.inv_dtInvoice.formatCustom("#dd##mm##yy#", 1, ""); // Data_documento_origine
                _str += ";EURO"; // Codice_valuta
                _str += ";CC"; // Codice_pagamento - or BB
                _str += ";" + (tbl.pr_total.objToDecimal() * 100).objToInt32(); // Importo_operazione_EURO
                _str += ";0"; // Indicatore_cliente_fornitore - 0=CLIENTE, 1=FORNITORE
                _str += ";" + tbl.id; // Codice_cliente_fornitore
                _str += ";" + tbl.cl_doc_vat_num; // Partita_iva
                _str += ";" + tbl.cl_doc_cf_num; // Codice_fiscale
                _str += ";" + tbl.cl_name_full; // Ragione_sociale_anagrafica
                _str += ";" + tbl.cl_loc_address; // Indirizzo_anagrafica
                _str += ";"; //  + tbl.cl_loc_city; // Codice_comune_anagrafica
                _str += ";" + tbl.cl_loc_zip_code; // Cap_anagrafica
                _str += ";" + tbl.cl_loc_city; // Citta_anagrafica
                _str += ";" + tbl.cl_loc_state; // Provincia_anagrafica
                var tmpCountry = countryList.SingleOrDefault(x => x.title == tbl.cl_loc_country && x.codeCoGe.objToInt32() > 0);
                _str += ";" + (tmpCountry != null ? tmpCountry.codeCoGe + "" : ""); // Codice_stato_estero
                _str += ";" + (tbl.cl_doc_vat_num + "" != "" ? "0" : "1"); // Flag_persona_fisica
                _str += ";"; // Cognome
                _str += ";"; // Nome
                _str += ";"; // Sesso
                _str += ";"; // Data_nascita
                _str += ";"; // Comune_nascita
                _str += ";"; // Provincia_nascita
                _writer.WriteLine(_str);

                _str = "2"; // RECORD_CORPO
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";"; // FILLER
                _str += ";0900020102"; // Codice_Conto
                _str += ";2"; // Indicatore_Dare_Avere - 1=DARE, 2=AVERE
                _str += ";" + (tbl.pr_tf.objToDecimal() * 100).objToInt32(); // Imponibile_Euro
                _str += ";" + (tbl.id == 2872 || tbl.id == 13136 || tbl.id == 13299 ? "21" : "10"); // Codice_aliquota_IVA
                _str += ";" + (tbl.pr_tax.objToDecimal() * 100).objToInt32(); // Imposta_Iva_Euro
                _writer.WriteLine(_str);
            }
            _writer.Close();
            //}
            //catch (Exception ex)
            //{ return; }
            return;
        }
    }
}
