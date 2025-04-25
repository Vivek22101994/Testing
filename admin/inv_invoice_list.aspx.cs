using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using EO.Pdf;
using RentalInRome.data;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Linq;
using Ionic.Zip;
using System.Net;


namespace RentalInRome.admin
{
    public partial class inv_invoice_list : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected magaInvoice_DataContext DC_INVOICE;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "inv_payment";
        }
        private List<INV_TBL_INVOICE> CURRENT_LIST_;
        private List<INV_TBL_INVOICE> CURRENT_LIST
        {
            get
            {
                if (CURRENT_LIST_ == null)
                    if (ViewState["CURRENT_LIST"] != null)
                    {
                        CURRENT_LIST_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LIST"],
                                                 typeof(INV_TBL_INVOICE)).Cast<INV_TBL_INVOICE>().ToList();
                    }
                    else
                        CURRENT_LIST_ = new List<INV_TBL_INVOICE>();

                return CURRENT_LIST_;
            }
            set
            {
                ViewState["CURRENT_LIST"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_LIST_ = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                HF_inv_dtInvoice_from.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).JSCal_dateToString();
                Bind_payment_type();
                LoadContent();
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setCal", "setCal();", true);
        }

        private void Bind_payment_type()
        {
            drp_payment_type.DataSource = invProps.CashPlaceLK.OrderBy(x => x.title).ToList();
            drp_payment_type.DataTextField = "title";
            drp_payment_type.DataValueField = "code";
            drp_payment_type.DataBind();
            drp_payment_type.Items.Insert(0, new ListItem("Tutti", "-1"));
        }
        protected void lnkFilter_Click(object sender, EventArgs e)
        {
            LoadContent();
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }
        protected void LoadContent()
        {
            List<INV_TBL_INVOICE> _list = DC_INVOICE.INV_TBL_INVOICE.ToList();

            if (txt_code.Text.Trim() != "")
            {
                _list = _list.Where(x => x.code != null && x.code.ToLower().Contains(txt_code.Text.ToLower().Trim())).ToList();
            }
            if (txt_rnt_reservation_code.Text.Trim() != "")
            {
                _list = _list.Where(x => x.rnt_reservation_code != null && x.rnt_reservation_code.ToLower().Contains(txt_rnt_reservation_code.Text.ToLower().Trim())).ToList();
            }
            if (txt_cl_name_full.Text.Trim() != "")
            {
                _list = _list.Where(x => x.cl_name_full != null && x.cl_name_full.ToLower().Contains(txt_cl_name_full.Text.ToLower().Trim())).ToList();
            }
            if (HF_dtCreation_from.Value.ToInt32() != 0)
            {
                _list = _list.Where(x => x.dtCreation >= HF_dtCreation_from.Value.JSCal_stringToDate()).ToList();
            }
            if (HF_dtCreation_to.Value.ToInt32() != 0)
            {
                _list = _list.Where(x => x.dtCreation <= HF_dtCreation_to.Value.JSCal_stringToDate()).ToList();
            }

            if (HF_inv_dtInvoice_from.Value.ToInt32() != 0)
            {
                _list = _list.Where(x => x.inv_dtInvoice >= HF_inv_dtInvoice_from.Value.JSCal_stringToDate()).ToList();
            }
            if (HF_inv_dtInvoice_to.Value.ToInt32() != 0)
            {
                _list = _list.Where(x => x.inv_dtInvoice <= HF_inv_dtInvoice_to.Value.JSCal_stringToDate()).ToList();
            }
            if (drp_has_noInvoice.getSelectedValueInt() != -1)
            {
                var tmpPaymentIds = _list.Select(x => x.inv_pid_payment.objToInt64()).Distinct().ToList();
                if (drp_has_noInvoice.getSelectedValueInt() == 0)
                    tmpPaymentIds = DC_INVOICE.INV_TBL_PAYMENT.Where(x => tmpPaymentIds.Contains(x.id) && (!x.pr_noInvoice.HasValue || x.pr_noInvoice == x.pr_total)).Select(x => x.id).ToList();
                if (drp_has_noInvoice.getSelectedValueInt() == 1)
                    tmpPaymentIds = DC_INVOICE.INV_TBL_PAYMENT.Where(x => tmpPaymentIds.Contains(x.id) && x.pr_noInvoice.HasValue && x.pr_noInvoice > 0).Select(x => x.id).ToList();
                _list = _list.Where(x => tmpPaymentIds.Contains(x.inv_pid_payment.objToInt64())).ToList();
            }
            if (drp_payment_type.getSelectedValueInt() != -1)
            {
                var tmpPaymentIds = _list.Select(x => x.inv_pid_payment.objToInt64()).Distinct().ToList();
                tmpPaymentIds = DC_INVOICE.INV_TBL_PAYMENT.Where(x => tmpPaymentIds.Contains(x.id) && x.pay_mode == drp_payment_type.SelectedValue).Select(x => x.id).ToList();
                _list = _list.Where(x => tmpPaymentIds.Contains(x.inv_pid_payment.objToInt64())).ToList();
            }
            CURRENT_LIST = _list.OrderByDescending(x => x.inv_dtInvoice).ToList();
            Fill_LV();
            Fill_LVExcel();
            Fill_stats();
        }
        protected void Fill_LV()
        {
            LV.DataSource = CURRENT_LIST;
            LV.DataBind();


        }
        protected void Fill_LVExcel()
        {
            LV_Excel.DataSource = CURRENT_LIST;
            LV_Excel.DataBind();
        }
        protected void Fill_stats()
        {
            pnl_stats.Visible = true;
            decimal _count = CURRENT_LIST.Count;
            txt_count.Text = _count.ToString();
            decimal _prTotal = CURRENT_LIST.Sum(x => x.pr_total.objToDecimal());
            txt_prTotal.Text = _prTotal.ToString("N2");

            decimal _prTotalMedia = _count != 0 ? _prTotal / _count : 0;
            txt_prTotalMedia.Text = _prTotalMedia.ToString("N2");

            decimal _countNotExported_1 = CURRENT_LIST.Count();
            //decimal _countNotExported_1 = CURRENT_LIST.Where(x => x.is_exported_1 != 1).Count();
            txt_countNotExported_1.Text = _countNotExported_1.ToString();
            pnl_newExport_1.Visible = _countNotExported_1 > 0;

            decimal _countNotExported_2 = CURRENT_LIST.Count();
            //decimal _countNotExported_2 = CURRENT_LIST.Where(x => x.is_exported_2 != 1).Count();
            txt_countNotExported_2.Text = _countNotExported_2.ToString();
            pnl_newExport_2.Visible = _countNotExported_2 > 0;

        }

        protected void lnk_newExport_1_Click(object sender, EventArgs e)
        {
            //removed change of export only not exported
            List<long> _InvoiceIDsList = CURRENT_LIST.Select(x => x.id).ToList();
            //List<long> _InvoiceIDsList = CURRENT_LIST.Where(x => x.is_exported_1 != 1).Select(x => x.id).ToList();
            ExportList _exp = new ExportList("exp1");
            ExportItem _item = new ExportItem();
            _item.Name = txt_newExportName_1.Text;
            _item.Creator = UserAuthentication.CurrentUserName;
            _item.FileName = _item.ID + ".pdf";
            _item.InvoiceIDsList = _InvoiceIDsList;
            _item.InvoiceIDsCount = _InvoiceIDsList.Count;
            _exp.Items.Add(_item);
            _exp.Save();
            List<INV_TBL_INVOICE> _list = DC_INVOICE.INV_TBL_INVOICE.Where(x => _InvoiceIDsList.Contains(x.id)).ToList();
            foreach (INV_TBL_INVOICE _inv in _list)
            {
                _inv.is_exported_1 = 1;
            }
            DC_INVOICE.SubmitChanges();
            _item.createPdf();
            Response.Redirect("inv_invoice_export.aspx?uid=" + _item.ID, true);
        }
        protected void lnk_newExport_2_Click(object sender, EventArgs e)
        {
            //removed change of export only not exported

            //List<long> _InvoiceIDsList = CURRENT_LIST.Where(x => x.is_exported_2 != 1).Select(x => x.id).ToList();
            List<long> _InvoiceIDsList = CURRENT_LIST.Select(x => x.id).ToList();
            ExportList _exp = new ExportList("exp2");
            ExportItem _item = new ExportItem();
            _item.Name = txt_newExportName_2.Text;
            _item.Creator = UserAuthentication.CurrentUserName;
            _item.FileName = _item.ID + ".txt";
            _item.InvoiceIDsList = _InvoiceIDsList;
            _item.InvoiceIDsCount = _InvoiceIDsList.Count;
            _exp.Items.Add(_item);
            _exp.Save();
            List<INV_TBL_INVOICE> _list = DC_INVOICE.INV_TBL_INVOICE.Where(x => _InvoiceIDsList.Contains(x.id)).ToList();
            foreach (INV_TBL_INVOICE _inv in _list)
            {
                _inv.is_exported_2 = 1;
            }
            DC_INVOICE.SubmitChanges();
            _item.createCsv_CoGe();
            Response.Redirect("inv_invoice_export.aspx?type=2&uid=" + _item.ID, true);
        }
        protected string getPaymentType(int id)
        {
            INV_TBL_PAYMENT currPaymemt = DC_INVOICE.INV_TBL_PAYMENT.SingleOrDefault(x => x.id == id);
            if (currPaymemt != null)
                return invUtils.invPayment_modeTitle(currPaymemt.pay_mode, "");
            else
                return "";

        }
        private void ExcelReport()
        {
            try
            {
                if (LV.Items.Count > 0)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=Invoice.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                    LV_Excel.RenderControl(htmlWrite);
                    Response.Write(stringWrite.ToString());
                    Response.End();
                }

            }
            catch (System.Threading.ThreadAbortException)
            {

            }
            catch (Exception ex)
            {

            }

        }

        protected void lnk_import_excel_Click(object sender, EventArgs e)
        {
            ExcelReport();
        }

        protected void LV_Excel_PagePropertiesChanged(object sender, EventArgs e)
        {
            Fill_LVExcel();
        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            var _inv = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == lbl_id.Text.objToInt64());
            if (e.CommandName == "sendInvoice")
            {
                if (_inv != null)
                {
                    var itemRnt = DC_INVOICE.INV_TBL_INVOICE_ITEM.FirstOrDefault(x => x.pid_invoice == lbl_id.Text.objToInt64() && x.sequence == 1);
                    if (itemRnt != null)
                    {
                        string token = digital_invoice.Fill_data();
                        string response = digital_invoice.Callinvoicefunction(_inv, itemRnt, token, digital_invoice.TipoDocumento.fattura);
                        if (response != "")
                        {
                            _inv.responseUniqueId = response;

                            int counter = _inv.numSentInvoice.objToInt32();
                            _inv.numSentInvoice = counter + 1;
                            DC_INVOICE.SubmitChanges();

                            //if(_inv.responseUniqueId!= null && _inv.responseUniqueId!= "")

                        }

                        var invoiceError = DC_INVOICE.INV_TBL_DIGITAL_INVOICE_ERROR.SingleOrDefault(x => x.id == lbl_id.Text.objToInt64());
                        if (invoiceError != null)
                        {
                            DC_INVOICE.INV_TBL_DIGITAL_INVOICE_ERROR.DeleteOnSubmit(invoiceError);
                            DC_INVOICE.SubmitChanges();
                        }

                        Fill_LV();

                    }
                }
            }else if (e.CommandName == "sendCreditNote")
            {
                if (_inv != null )
                {
                    if (_inv.numSentInvoice.objToInt32() > 0 && _inv.numSentInvoice.objToInt32() > _inv.numSentCreditNotes.objToInt32())
                    {


                        var itemRnt = DC_INVOICE.INV_TBL_INVOICE_ITEM.FirstOrDefault(x => x.pid_invoice == lbl_id.Text.objToInt64() && x.sequence == 1);
                        if (itemRnt != null)
                        {
                            string token = digital_invoice.Fill_data();
                            string response = digital_invoice.Callinvoicefunction(_inv, itemRnt, token, digital_invoice.TipoDocumento.notadicredito);
                            if (response != "")
                            {
                                _inv.responseUniqueId = response;

                                int counter = _inv.numSentCreditNotes.objToInt32();
                                _inv.numSentCreditNotes = counter + 1;
                                DC_INVOICE.SubmitChanges();

                   
                            }

                            var invoiceError = DC_INVOICE.INV_TBL_DIGITAL_CREDITNOTE_ERRORs.SingleOrDefault(x => x.id == lbl_id.Text.objToInt64());
                            if (invoiceError != null)
                            {
                                DC_INVOICE.INV_TBL_DIGITAL_CREDITNOTE_ERRORs.DeleteOnSubmit(invoiceError);
                                DC_INVOICE.SubmitChanges();
                            }

                            Fill_LV();

                        }
                    }
                    else
                    {
                        ErrorLog.addLog("sendCreditNote", "sendCreditNote", "numSentInvoice is not greater than 0 or not greater than numSentCreditNotes ");
                    }
                }
            }

        }

        public static string SendRequest(String requesUrl, String requestType, String requestContent)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = digital_invoice.Fill_data();
                HttpWebRequest obj = (HttpWebRequest)HttpWebRequest.Create(requesUrl);
                obj.Headers.Add("ContentType", "application/xml");
                //obj.Headers["Accept"] = "application/octet-stream";
                obj.Headers["Authorization"] = "Bearer " + token;
                //obj.ContentType = "application/xml";
                obj.Accept = "application/octet-stream";
                obj.Method = requestType;
                if (requestContent != "")
                {
                    //set new TLS protocol 1.1/1.2
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.t;
                    Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestContent);
                    obj.ContentLength = bytes.Length;
                    Stream os = obj.GetRequestStream();
                    os.Write(bytes, 0, bytes.Length);
                    os.Close();
                }
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebResponse obj1 = (HttpWebResponse)obj.GetResponse();
                Stream os1 = obj1.GetResponseStream();
                StreamReader _Answer = new StreamReader(os1);
                String strRSString = _Answer.ReadToEnd().ToString();
                return strRSString;

            }
            catch (WebException ex)
            {
                ErrorLog.addLog("", "import xml send req", ex.ToString());
                return "";
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "import xml send req", ex.ToString());
                return "";
            }
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                LinkButton lnk_send_invoice = e.Item.FindControl("lnk_send_invoice") as LinkButton;
                Label lbl_num_send_invoice = e.Item.FindControl("lbl_num_send_invoice") as Label;

                Label lbl_num_send_credit_note = e.Item.FindControl("lbl_num_send_credit_note") as Label;

                LinkButton lnk_send_credit_note = e.Item.FindControl("lnk_send_credit_note") as LinkButton;
                lnk_send_credit_note.Visible = false;

                magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                HtmlAnchor lbl_res_area = e.Item.FindControl("lbl_res_area") as HtmlAnchor;
                HtmlAnchor lnk_res_detail = e.Item.FindControl("lnk_res_detail") as HtmlAnchor;



                var _inv = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == lbl_id.Text.objToInt64());
                if (_inv != null)
                {

                    if (CommonUtilities.getSYS_SETTING("is_hide_invoice").objToInt32() == 1)
                    {

                        //if (!string.IsNullOrEmpty(_inv.responseUniqueId))
                        //{
                        //    lnk_send_invoice.Visible = true;
                        //}
                        //else
                        //{
                        //    lnk_send_invoice.Visible = false;
                        //}
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(_inv.responseUniqueId))
                        {
                            lnk_send_invoice.Visible = true;
                            lnk_send_credit_note.Visible = false;
                        }
                        else
                        {
                            lnk_send_invoice.Visible = false;
                        }
                    }
                    if (_inv.numSentInvoice.objToInt32() > 0 && (_inv.numSentInvoice.objToInt32() > _inv.numSentCreditNotes.objToInt32()))
                    {
                        lnk_send_credit_note.Visible = true;
                    }
                    lbl_num_send_invoice.Text = " (" + _inv.numSentInvoice.objToInt32() + ")";
                    if (lbl_num_send_credit_note != null)
                    {
                        lbl_num_send_credit_note.Text = " (" + _inv.numSentCreditNotes.objToInt32() + ")";
                    }

                    var currReservation = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == _inv.rnt_pid_reservation);
                    if (currReservation != null)
                    {
                        lbl_res_area.HRef = App.HOST_SSL + "/reservationarea/login.aspx?auth=" + currReservation.unique_id + "&personal=true";
                        lnk_res_detail.HRef = App.HOST + "/admin/rnt_reservation_details.aspx?id=" + currReservation.id;
                    }


                }
            }
        }

        protected void lnk_send_invoices_Click(object sender, EventArgs e)
        {
            foreach (ListViewDataItem objItem in LV.Items)
            {
                CheckBox chk_sent_invoice = objItem.FindControl("chk_sent_invoice") as CheckBox;
                Label lbl_id = objItem.FindControl("lbl_id") as Label;

                if (chk_sent_invoice.Checked == true)
                {
                    var _inv = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == lbl_id.Text.objToInt64());
                    if (_inv != null)
                    {
                        var itemRnt = DC_INVOICE.INV_TBL_INVOICE_ITEM.FirstOrDefault(x => x.pid_invoice == lbl_id.Text.objToInt64() && x.sequence == 1);
                        if (itemRnt != null)
                        {
                            string token = digital_invoice.Fill_data();
                            string response = digital_invoice.Callinvoicefunction(_inv, itemRnt, token,digital_invoice.TipoDocumento.fattura);
                            if (response != "")
                            {
                                _inv.responseUniqueId = response;
                                int counter = _inv.numSentInvoice.objToInt32();
                                _inv.numSentInvoice = counter + 1;
                                DC_INVOICE.SubmitChanges();
                            }

                            var invoiceError = DC_INVOICE.INV_TBL_DIGITAL_INVOICE_ERROR.SingleOrDefault(x => x.id == lbl_id.Text.objToInt64());
                            if (invoiceError != null)
                            {
                                DC_INVOICE.INV_TBL_DIGITAL_INVOICE_ERROR.DeleteOnSubmit(invoiceError);
                                DC_INVOICE.SubmitChanges();

                            }

                        }

                    }

                }
            }
            Fill_LV();
        }

        private void DownloadFile(string filePath)
        {
            Response.ContentType = "application/xml";
            Response.AddHeader("Content-Disposition", "attachment;filename=Customers.xml");
            Response.WriteFile(filePath);
            Response.End();
        }
        string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                // Handle and throw if fatal exception here; don't just ignore them
                return xml;
            }
        }
        //protected void DownloadFiles(object sender, EventArgs e)
        //{
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        //zip = ZipOption.AsNecessary;
        //        zip.AddDirectoryByName("Files");
        //        foreach (ListViewDataItem objItem in LV.Items)
        //        {
        //            CheckBox chk_import_xml = objItem.FindControl("chk_import_xml") as CheckBox;
        //            {
        //                string filePath = (row.FindControl("lblFilePath") as Label).Text;
        //                zip.AddFile(filePath, "Files");
        //            }
        //        }
        //        Response.Clear();
        //        Response.BufferOutput = false;
        //        string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        //        Response.ContentType = "application/zip";
        //        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
        //        zip.Save(Response.OutputStream);
        //        Response.End();
        //    }
        //}
        protected void lnk_import_xml_Click(object sender, EventArgs e)
        {
            using (ZipFile zip = new ZipFile())
            {
                //zip = ZipOption.AsNecessary;
                string folder_name = "Files";
                zip.AddDirectoryByName(folder_name);

                foreach (ListViewDataItem objItem in LV.Items)
                {
                    CheckBox chk_import_xml = objItem.FindControl("chk_import_xml") as CheckBox;
                    Label lbl_id = objItem.FindControl("lbl_id") as Label;

                    if (chk_import_xml.Checked == true)
                    {
                        var _inv = DC_INVOICE.INV_TBL_INVOICE.SingleOrDefault(x => x.id == lbl_id.Text.objToInt64());
                        if (_inv != null)
                        {
                            string url = "https://api-sandbox.acubeapi.com/invoices/" + _inv.responseUniqueId;
                            if (CommonUtilities.getSYS_SETTING("is_live_cube").objToInt32() == 1)
                                url = "https://api.acubeapi.com/invoices/" + _inv.responseUniqueId;
                            string response = SendRequest(url, "GET", "");
                            if (response != "")
                            {
                                _inv.xmltext = response;
                                DC_INVOICE.SubmitChanges();

                                string filePath = Path.Combine(App.SRP, "admin/invoices");
                                if (!Directory.Exists(filePath))
                                    Directory.CreateDirectory(filePath);
                                filePath = Path.Combine(filePath, _inv.code + ".xml");
                                if (!File.Exists(filePath) || response != "")
                                {
                                    StreamWriter ccWriter = new StreamWriter(filePath, false);
                                    ccWriter.WriteLine(FormatXml(response)); // Write the file.
                                    ccWriter.Flush();
                                    ccWriter.Close(); // Close the instance of StreamWriter.
                                    ccWriter.Dispose(); // Dispose from memory.
                                }
                                zip.AddFile(filePath, folder_name);
                                //DownloadFile(filePath);

                            }
                            ErrorLog.addLog("", "xml response " + _inv.code, response);
                        }
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }
}