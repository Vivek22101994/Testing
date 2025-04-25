using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace RentalInRome.admin
{
    public partial class inv_invoice_formNew : adminBasePage
    {
        protected INV_TBL_INVOICE _currTBL;
        private magaInvoice_DataContext DC_INVOICE;
        public long IdInvoice
        {
            get { return HF_id.Value.ToInt64(); }
            set { HF_id.Value = value.ToString(); }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_reservation_list";

        }
        private List<INV_TBL_INVOICE_ITEM> CURRENT_ITEMS_;
        private List<INV_TBL_INVOICE_ITEM> CURRENT_ITEMS
        {
            get
            {
                if (CURRENT_ITEMS_ == null)
                    if (ViewState["CURRENT_ITEMS"] != null)
                    {
                        CURRENT_ITEMS_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_ITEMS"],
                                                 typeof(INV_TBL_INVOICE_ITEM)).Cast<INV_TBL_INVOICE_ITEM>().ToList();
                    }
                    else
                        CURRENT_ITEMS_ = new List<INV_TBL_INVOICE_ITEM>();

                return CURRENT_ITEMS_;
            }
            set
            {
                ViewState["CURRENT_ITEMS"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_ITEMS_ = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                drp_cl_loc_country.DataBind();
                drp_cl_loc_country.Items.Insert(0, new ListItem("-seleziona-",""));
                IdInvoice = 0;
                fillItems();
            }
        }
        protected void drp_cashTaxID_DataBind(ref DropDownList drp_cashTaxID)
        {
            drp_cashTaxID.DataSource = invProps.CashTaxLK.OrderBy(x => x.id);
            drp_cashTaxID.DataTextField = "code";
            drp_cashTaxID.DataValueField = "id";
            drp_cashTaxID.DataBind();
        }
        protected void fillItems()
        {
            LV.DataSource = CURRENT_ITEMS;
            LV.DataBind();
            foreach (ListViewDataItem _item in LV.Items)
            {
                DropDownList drp_cashTaxID = _item.FindControl("drp_cashTaxID") as DropDownList;
                Label lbl_cashTaxID = _item.FindControl("lbl_cashTaxID") as Label;
                drp_cashTaxID_DataBind(ref drp_cashTaxID);
                if (lbl_cashTaxID.Text.ToInt32() == 0) lbl_cashTaxID.Text = "1";
                drp_cashTaxID.setSelectedValue(lbl_cashTaxID.Text);
            }
        }
        protected string saveItems()
        {
            string _error = "";
            decimal pr_total = 0;
            decimal pr_tf = 0;
            decimal pr_tax = 0;
            if (txt_cl_name_full.Text == "")
                _error += "- inserire 'Denominazione' di fatturazione<br/>";
            if (txt_cl_loc_address.Text == "")
                _error += "- inserire 'Indirizzo' di fatturazione<br/>";
            if (txt_cl_loc_zip_code.Text == "")
                _error += "- inserire 'CAP' di fatturazione<br/>";
            if (txt_cl_loc_city.Text == "")
                _error += "- inserire 'Città' di fatturazione<br/>";
            if (txt_cl_loc_state.Text == "")
                _error += "- inserire 'Provincia/Stato' di fatturazione<br/>";
            if (txt_loc_province.Text == "")
                _error += "- inserire 'Provincia' di fatturazione<br/>";
            if (drp_cl_loc_country.SelectedValue == "")
                _error += "- inserire 'Nazione/Location' di fatturazione<br/>";
            List<INV_TBL_INVOICE_ITEM> _list = new List<INV_TBL_INVOICE_ITEM>();
            foreach (ListViewDataItem _item in LV.Items)
            {
                TextBox txt_code = _item.FindControl("txt_code") as TextBox;
                TextBox txt_description = _item.FindControl("txt_description") as TextBox;
                TextBox txt_quantity = _item.FindControl("txt_quantity") as TextBox;
                TextBox txt_price_total = _item.FindControl("txt_price_total") as TextBox;
                Label lbl_id = _item.FindControl("lbl_id") as Label;
                Label lbl_sequence = _item.FindControl("lbl_sequence") as Label;
                DropDownList drp_cashTaxID = _item.FindControl("drp_cashTaxID") as DropDownList;
                if (txt_quantity.Text.ToInt32() == 0) txt_quantity.Text = "1";

                decimal taxPercentage = 0;
                int taxId = drp_cashTaxID.getSelectedValueInt();
                var currTax = invProps.CashTaxLK.SingleOrDefault(x => x.id == taxId);
                if (currTax != null)
                {
                    taxPercentage = currTax.taxAmount.objToDecimal();
                }

                INV_TBL_INVOICE_ITEM _new = new INV_TBL_INVOICE_ITEM();
                _new.pid_invoice = 0;
                _new.is_deleted = 0;
                _new.code = txt_code.Text;
                _new.description = txt_description.Text;
                _new.id = new Guid(lbl_id.Text);
                _new.sequence = lbl_sequence.Text.ToInt32();
                _new.quantity = txt_quantity.Text.ToInt32();
                _new.quantity_type = "pz";
                _new.price_total = txt_price_total.Text.ToDecimal();
                _new.price_tax_id = taxId;
                decimal TMPcashTaxFreeDecimal = Decimal.Divide(_new.price_total.objToDecimal(), (1 + taxPercentage));
                decimal TMPcashTaxFreeRounded = Decimal.Round(TMPcashTaxFreeDecimal, 2);
                if (TMPcashTaxFreeRounded > TMPcashTaxFreeDecimal)
                    TMPcashTaxFreeRounded -= new Decimal(0.01);
                _new.price_tf = TMPcashTaxFreeRounded;
                _new.price_tax = Decimal.Subtract(_new.price_total.Value, TMPcashTaxFreeRounded);
                _new.price_unit = Decimal.Divide(TMPcashTaxFreeRounded, _new.quantity.Value);
                _list.Add(_new);
                pr_total += _new.price_total.objToDecimal();
                pr_tf += _new.price_tf.objToDecimal();
                pr_tax += _new.price_tax.objToDecimal();
                if (_new.code == "")
                    _error += "- inserire 'Item' del oggetto #" + _new.sequence + "<br/>";
                if (_new.description == "")
                    _error += "- inserire 'Description' del oggetto #" + _new.sequence + "<br/>";
                if (_new.quantity == 0)
                    _error += "- inserire 'Quantity' del oggetto #" + _new.sequence + "<br/>";
                if (_new.price_total == 0)
                    _error += "- inserire 'Prezzo Totale' del oggetto #" + _new.sequence + "<br/>";
            }
            if (_list.Count==0)
                _error += "- inserire Almeno 1 Oggetto della fattura<br/>";
            CURRENT_ITEMS = _list;
            HF_pr_total.Value = pr_total.ToString();
            HF_pr_tf.Value = pr_tf.ToString();
            HF_pr_tax.Value = pr_tax.ToString();
            return _error;
        }
        protected void lnk_add_new_Click(object sender, EventArgs e)
        {
            saveItems();
            List<INV_TBL_INVOICE_ITEM> _list = CURRENT_ITEMS;
            INV_TBL_INVOICE_ITEM _new = new INV_TBL_INVOICE_ITEM();
            _new.pid_invoice = 0;
            _new.is_deleted = 0;
            _new.code = "";
            _new.description = "";
            _new.id = Guid.NewGuid();
            _new.sequence = _list.Count() + 1;
            _new.quantity = 1;
            _new.quantity_type = "pz";
            _new.price_total = 0;
            _new.price_tax_id = 0;
            _new.price_tf = Math.Round((_new.price_total.objToDecimal() / new decimal(1.1)), 2);
            _new.price_tax = _new.price_total - _new.price_tf;
            _new.price_unit = _new.price_tf / _new.quantity;
            _list.Add(_new);
            CURRENT_ITEMS = _list;
            fillItems();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "elimina")
            {
                saveItems();
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                List<INV_TBL_INVOICE_ITEM> _list = CURRENT_ITEMS;
                INV_TBL_INVOICE_ITEM _del = _list.SingleOrDefault(x => x.id == new Guid(lbl_id.Text));
                if (_del != null)
                    _list.Remove(_del);
                List<INV_TBL_INVOICE_ITEM> _listNew = new List<INV_TBL_INVOICE_ITEM>();
                foreach (INV_TBL_INVOICE_ITEM _item in _list)
                {
                    _item.sequence = _listNew.Count + 1;
                    _listNew.Add(_item);
                }
                CURRENT_ITEMS = _listNew;
                fillItems();
            }
        }
        protected void lnk_calculate_Click(object sender, EventArgs e)
        {
            string _error = saveItems() ;
            if (_error != "")
            {
                ltr_error.Text = _error;
                pnl_btnSave.Visible = false;
                pnl_error.Visible = true;
            }
            else
            {
                pnl_btnSave.Visible = true;
                pnl_error.Visible = false;
            }
            fillItems();
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            string _error = saveItems();
            if (_error != "")
            {
                ltr_error.Text = _error;
                pnl_btnSave.Visible = false;
                pnl_error.Visible = true;
                return;
            }
            else
            {
                pnl_btnSave.Visible = true;
                pnl_error.Visible = false;
            }
            DateTime _dtInvoice = DateTime.Now;
            int? _lastCounter = DC_INVOICE.INV_TBL_INVOICE.Where(x => x.inv_dtInvoice.HasValue && x.inv_dtInvoice.Value.Year == _dtInvoice.Year && x.inv_counter.HasValue).Max(x => x.inv_counter);
            if (_lastCounter == null || _lastCounter == 0) _lastCounter = 1;
            _currTBL = new INV_TBL_INVOICE();
            _currTBL.uid = Guid.NewGuid();
            _currTBL.inv_counter = _lastCounter + 1;
            _currTBL.code = DateTime.Now.Year + "-" + (_currTBL.inv_counter + "").fillString("0", 5, false);
            _currTBL.rnt_pid_reservation = 0;
            _currTBL.rnt_reservation_code = "-manuale-";
            _currTBL.inv_pid_payment = 0;
            _currTBL.inv_dtInvoice = _dtInvoice;
            DC_INVOICE.INV_TBL_INVOICE.InsertOnSubmit(_currTBL);
            DC_INVOICE.SubmitChanges();
            _currTBL.dtCreation = DateTime.Now;
            _currTBL.pid_operator = UserAuthentication.CurrentUserID;
            _currTBL.is_active = 1;
            _currTBL.is_deleted = 0;
            _currTBL.is_exported_1 = 0;
            _currTBL.is_exported_2 = 0;
            _currTBL.is_exported_3 = 0;
            _currTBL.pr_tax_id = 0;
            _currTBL.pr_total = HF_pr_total.Value.objToDecimal();
            _currTBL.pr_tf = HF_pr_tf.Value.objToDecimal();
            _currTBL.pr_tax = HF_pr_tax.Value.objToDecimal();

            _currTBL.cl_name_full = txt_cl_name_full.Text;
            _currTBL.cl_loc_country = drp_cl_loc_country.Text;
            _currTBL.cl_loc_state = txt_cl_loc_state.Text;
            _currTBL.cl_loc_province = txt_loc_province.Text;
            _currTBL.cl_loc_city = txt_cl_loc_city.Text;
            _currTBL.cl_loc_address = txt_cl_loc_address.Text;
            _currTBL.cl_loc_zip_code = txt_cl_loc_zip_code.Text;
            _currTBL.cl_doc_vat_num = txt_cl_doc_vat_num.Text;
            _currTBL.cl_doc_cf_num = txt_cl_doc_cf_num.Text;
            _currTBL.idCodice = txt_codice_destinatario.Text;
            _currTBL.cl_id = 0; // todo forse va il cliente
            _currTBL.cl_pid_discount = 0;
            _currTBL.cl_pid_lang = 2;
            _currTBL.cl_name_honorific = "";
            _currTBL.cl_email = "";
            _currTBL.cl_phone = "";
            _currTBL.cl_fax = "";
            _currTBL.cl_doc_type = "";
            _currTBL.cl_doc_num = "";
            _currTBL.cl_doc_issue_place = "";
            _currTBL.cl_doc_issue_date = new DateTime(2010,1,1);
            _currTBL.cl_doc_expiry_date = new DateTime(2015, 1, 1);

            DC_INVOICE.SubmitChanges();
            List<INV_TBL_INVOICE_ITEM> _list = CURRENT_ITEMS;
            foreach (INV_TBL_INVOICE_ITEM _item in _list)
            {
                _item.pid_invoice = _currTBL.id;
                DC_INVOICE.INV_TBL_INVOICE_ITEM.InsertOnSubmit(_item);
            }
            DC_INVOICE.SubmitChanges();

            ErrorLog.addLog("", "crete invoice", "crete invoice1");
            ////send invoice on new creation
            //var itemRnt = DC_INVOICE.INV_TBL_INVOICE_ITEM.FirstOrDefault(x => x.pid_invoice == _currTBL.id && x.sequence == 1);
            //if (itemRnt != null)
            //{
            //    ErrorLog.addLog("", "crete invoice", "crete invoice2");
            //    string token = Fill_data();
            //    ErrorLog.addLog("", "crete invoice", "crete invoice3");
            //    string response = digital_invoice.Callinvoicefunction(_currTBL, itemRnt, token);
            //    if (response != "")
            //    {
            //        _currTBL.responseUniqueId = response;

            //        int counter = _currTBL.numSentInvoice.objToInt32();
            //        _currTBL.numSentInvoice = counter + 1;

            //        DC_INVOICE.SubmitChanges();
            //    }
            //}
            Response.Redirect("inv_invoice_form.aspx?id=" + _currTBL.id);
        }
        public static string Fill_data()
        {
            try
            {
                ErrorLog.addLog("", "myCommandMessage token", "myCommandMessage.token" + "");
                string iCalUrl = "https://api-sandbox.acubeapi.com/login_check";
                if (CommonUtilities.getSYS_SETTING("is_live_cube").objToInt32() == 1)
                    iCalUrl = "https://api.acubeapi.com/login_check";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(iCalUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"email\":\"maurizio.lecce@magarental.com\"," +
                                  "\"password\":\"6N4NyndVZemDejre\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var response = streamReader.ReadToEnd();
                    var myCommandMessage = JsonConvert.DeserializeObject<digital_invoice.CommandMessage>(response);
                    ErrorLog.addLog("", "myCommandMessage", myCommandMessage.token + "");
                    if (myCommandMessage.token != null)
                    {
                        return myCommandMessage.token;
                    }
                    else
                    {
                        return "";
                    }
                    //JArray array = (JArray)ojObject["token"];
                    //ErrorLog.addLog("", "array", array + "");
                    //string token = array[0];
                    //ErrorLog.addLog("", "token", token + "");

                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "token", ex.Message);
                return "";
            }
        }

    }
}
