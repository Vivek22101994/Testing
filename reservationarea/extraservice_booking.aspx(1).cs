using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;
using System.IO;
using System.Web.UI.HtmlControls;

namespace RentalInRome.reservationarea
{
    public partial class extraservice_booking : basePage
    {
        private magaRental_DataContext DC_RENTAL;
        private magaUser_DataContext DC_USER;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "extra_booking";
            //int id = Request.QueryString["id"].ToInt32();
            //if (id != 0)
            //    base.PAGE_REF_ID = id;
            //else
            //{
            //    string _params = "";
            //    string _ip = "";
            //    try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
            //    catch (Exception ex1) { }
            //    foreach (string key in Request.Params.AllKeys)
            //        _params += "\n" + key + "=" + Request.Params[key];
            //    ErrorLog.addLog(_ip, "extraservicecategory.aspx", _params);
            //    Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            //}
            //RewritePath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drp_cc_meseScadenza.Items.Insert(0, new ListItem(contUtils.getLabel("lblMonth"), ""));
                drp_cc_annoScadenza.Items.Insert(0, new ListItem(contUtils.getLabel("lblYear"), ""));
                filldata();
            }
        }
        protected void filldata()
        {
            HFResId.Value = Convert.ToString(UC_sx1.CurrentIdReservation);
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            RNT_TBL_RESERVATION _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == HFResId.Value.objToInt64());
            if (_currTBL != null)
            {
                DC_USER = maga_DataContext.DC_USER;
                USR_TBL_CLIENT _currUser = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currTBL.cl_id);
                if (_currUser != null)
                {
                    if (_currUser.name_full != null && _currUser.name_full != "")
                    {
                        if (_currUser.name_full.Contains(' '))
                        {
                            string[] name = _currUser.name_full.Split(' ');
                            if (name != null)
                            {
                                if (name.Length == 0)
                                {
                                    txt_nameFirst.Text = name[0];
                                }
                                if (name.Length >= 1)
                                {
                                    txt_nameFirst.Text = name[0];
                                    txt_nameLast.Text = name[1];
                                }
                            }
                        }
                        else
                        {
                            txt_nameFirst.Text = _currUser.name_full;
                        }
                    }
                    txt_loc_address.Text = _currUser.loc_address;
                    txt_loc_city.Text = _currUser.loc_city;
                    txt_loc_state.Text = _currUser.loc_state;
                    drp_loc_country.setSelectedValue(_currUser.loc_country);
                    txt_loc_zip_code.Text = _currUser.loc_zip_code;
                    txt_loc_address.Text = _currUser.loc_address;
                    txt_contact_email.Text = _currUser.contact_email;
                    txt_contact_phone_mobile.Text = _currUser.contact_phone_mobile;
                    txt_doc_cf_num.Text = _currUser.doc_cf_num;
                    txt_doc_vat_num.Text = _currUser.doc_vat_num;
                }
                fillservices();

            }


        }
        protected void fillservices()
        {
            if (Session["services"] != null)
            {
                LV.DataSource = Session["services"];
                LV.DataBind();
                lbl_totalPrice.Text = "0";
                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_price = item.FindControl("lbl_price") as Label;
                    HiddenField HF_anticipo = item.FindControl("HF_anticipo") as HiddenField;
                    HiddenField HF_commission = item.FindControl("HF_commission") as HiddenField;

                    lbl_totalPrice.Text = (lbl_totalPrice.Text.objToDecimal() + lbl_price.Text.objToDecimal()).ToString("N2");
                    if (HF_anticipo.Value == "1")
                    {
                        lbl_payNow.Text = (lbl_payNow.Text.objToDecimal() + HF_commission.Value.objToDecimal()).ToString("N2");
                    }
                }
            }

        }
        protected void drp_loc_country_DataBound(object sender, EventArgs e)
        {
            drp_loc_country.Items.Insert(0, new ListItem("- - -", ""));
        }
        //protected void rbtPagamento_OnCheckedChanged(object sender, EventArgs e)
        //{
        //    pnlPagamento_Bonifico.Visible = rbtPagamento_Bonifico.Checked;
        //    pnlPagamento_Paypal.Visible = rbtPagamento_Paypal.Checked;
        //    pnlPagamento_CC.Visible = rbtPagamento_CC.Checked;
        //}
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                Label lbl_date = e.Item.FindControl("lbl_date") as Label;
                if (lbl_id == null) return;
                if (Session["services"] != null)
                {
                    List<util_serviceAdd.Services> lstServices = (List<util_serviceAdd.Services>)Session["Services"];
                    util_serviceAdd.Services objService = lstServices.SingleOrDefault(x => x.serviceId == lbl_id.Text.objToInt32() && x.date == lbl_date.Text);
                    lstServices.Remove(objService);
                    Session["services"] = lstServices;
                    fillservices();

                }

            }
        }
        protected void saveData()
        {
            try
            {
                //if (Session["list"] != null)
                if (LV.Items.Count > 0)
                {
                    DC_RENTAL = maga_DataContext.DC_RENTAL;
                    List<string> lstOwner = new List<string>();
                    RNT_TBL_RESERVATION _currRes = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == HFResId.Value.objToInt64());
                    if (_currRes != null)
                    {
                        //adding extraservices details
                        using (DCmodRental dc = new DCmodRental())
                        {
                            //adding booking details for all extra services
                            var extraBooking = new dbRntReservationExtras();
                            extraBooking.createdDate = DateTime.Now;
                            extraBooking.is_paid = 0;
                            extraBooking.pidReservation = HFResId.Value.objToInt64();
                            extraBooking.TotalPrice = lbl_totalPrice.Text.objToDecimal();
                            extraBooking.PayNow = lbl_payNow.Text.objToDecimal();
                            dc.Add(extraBooking);
                            dc.SaveChanges();
                            HFBookingId.Value = extraBooking.id.ToString();

                            foreach (ListViewDataItem item in LV.Items)
                            {
                                Label lbl_id = item.FindControl("lbl_id") as Label;
                                Label lbl_price = item.FindControl("lbl_price") as Label;
                                Label lbl_priceType = item.FindControl("lbl_priceType") as Label;
                                Label lbl_datevalue = item.FindControl("lbl_datevalue") as Label;
                                Label lbl_adults = item.FindControl("lbl_adults") as Label;
                                Label lbl_childern = item.FindControl("lbl_childern") as Label;
                                HiddenField HF_commission = item.FindControl("HF_commission") as HiddenField;
                                Label lbl_commission = item.FindControl("lbl_commission") as Label;
                                Label lbl_owner = item.FindControl("lbl_owner") as Label;

                                var newReservationExtras = new dbRntReservationExtrasTMP();
                                newReservationExtras.bookingId = HFBookingId.Value.objToInt64();
                                newReservationExtras.pidReservation = HFResId.Value.objToInt64();
                                newReservationExtras.pidExtras = lbl_id.Text.ToInt32();
                                newReservationExtras.Price = lbl_price.Text.objToDecimal();
                                newReservationExtras.pidPriceType = lbl_priceType.Text.objToInt32();
                                newReservationExtras.inDate = lbl_datevalue.Text.JSCal_stringToDate();
                                newReservationExtras.numPersAdult = lbl_adults.Text.objToInt32();
                                newReservationExtras.numPersChild = lbl_childern.Text.objToInt32();
                                newReservationExtras.Commission = HF_commission.Value.objToDecimal();
                                dc.Add(newReservationExtras);
                                dc.SaveChanges();
                                dbRntEstateExtrasTB currExtra = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == lbl_id.Text.objToInt32());
                                if (currExtra != null)
                                {
                                    if (currExtra.pidOwner != null)
                                    {
                                        dbRntExtraOwnerTBL currOwner = dc.dbRntExtraOwnerTBLs.SingleOrDefault(x => x.id == currExtra.pidOwner.objToInt32());
                                        if (currOwner != null)
                                        {
                                            if (currOwner.contactEmail != null && currOwner.contactEmail != "")
                                            {
                                                lstOwner.Add(Convert.ToString(currOwner.id));
                                            }
                                        }
                                    }
                                }
                            }



                        }

                        DC_USER = maga_DataContext.DC_USER;
                        USR_TBL_CLIENT _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _currRes.cl_id);
                        if (_client != null)
                        {
                            //update the data of the user in client table
                            _client.contact_email = txt_contact_email.Text;
                            //_client.name_honorific = tmpTBL.cl_name_honorific;
                            _client.name_full = txt_nameFirst.Text + " " + txt_nameLast.Text;
                            _client.loc_address = txt_loc_address.Text;
                            _client.loc_city = txt_loc_city.Text;
                            _client.loc_state = txt_loc_state.Text;
                            _client.loc_country = drp_loc_country.SelectedValue;
                            _client.loc_zip_code = txt_loc_zip_code.Text;
                            _client.contact_email = txt_contact_email.Text;
                            _client.contact_phone_mobile = txt_contact_phone_mobile.Text;
                            _client.doc_cf_num = txt_doc_cf_num.Text;
                            _client.doc_vat_num = txt_doc_vat_num.Text;
                            _client.pid_lang = CurrentLang.ID;
                            //_client.isCompleted = 0;
                            //_client.is_deleted = 0;
                            //_client.is_active = 1;
                            //_client.date_created = DateTime.Now;
                            _client.login = _client.contact_email;
                            DC_USER.SubmitChanges();

                            //_client.password = CommonUtilities.CreatePassword(8, false, true, false);
                            //DC_USER.USR_TBL_CLIENTs.InsertOnSubmit(_client);

                            //_client.code = _client.id.ToString().fillString("0", 7, false);
                            //DC_USER.SubmitChanges();
                        }
                        //updating client details in reservation table.

                        _currRes.cl_email = _client.contact_email;
                        _currRes.cl_name_full = _client.name_full;
                        _currRes.cl_loc_country = _client.loc_country;
                        _currRes.cl_pid_lang = _client.pid_lang;
                        _currRes.pr_part_extraServices = lbl_totalPrice.Text.objToDecimal();
                        DC_RENTAL.SubmitChanges();

                        //for paymet wih credit card
                        string ccData = "";
                        ccData += "\n Holder: " + txt_cc_titolareCarta.Value;
                        ccData += "\n Number: " + txt_cc_numeroCarta.Value;
                        ccData += "\n Type: " + drp_cc_tipoCarta.getSelectedText("");
                        ccData += "\n Expire: " + drp_cc_meseScadenza.Value + "/" + drp_cc_annoScadenza.Value;
                        ccData += "\n BookingId: " + HFBookingId.Value.objToInt64();

                        string filePath = Path.Combine(App.SRP, "admin/dati_cc");
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);
                        filePath = Path.Combine(filePath, _currRes.unique_id + "_extraservices.txt");
                        StreamWriter ccWriter = new StreamWriter(filePath, true);
                        ccWriter.WriteLine(RijndaelSimple_2.Encrypt(ccData)); // Write the file.
                        ccWriter.Flush();
                        ccWriter.Close(); // Close the instance of StreamWriter.
                        ccWriter.Dispose(); // Dispose from memory.
                        Session["services"] = null;
                        lbl_error.Text = "";
                        bool ans = rntUtils.rntExtraReservation_mailPartPaymentReceive(_currRes, true, true, lstOwner, UserAuthentication.CurrentUserID); // send extra service mails
                        if (ans == true)
                        {
                            Response.Redirect("/reservationarea/arrivaldeparture.aspx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mailUtils.addLog("0", ex.ToString(), "test", "test", null, null, null, false, 0, "est", false, "test", "test", "test", "test", "test");
                lbl_error.Text = ex.ToString();
            }

        }
        protected void lnkBookNow_Click(object sender, EventArgs e)
        {
            saveData();
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                Label lbl_price = e.Item.FindControl("lbl_price") as Label;
                HiddenField HF_anticipo = e.Item.FindControl("HF_anticipo") as HiddenField;
                HiddenField HF_commission = e.Item.FindControl("HF_commission") as HiddenField;
                Label lbl_commission = e.Item.FindControl("lbl_commission") as Label;
                HtmlGenericControl div_payNow = e.Item.FindControl("div_payNow") as HtmlGenericControl;

                util_serviceAdd.Services currService = (util_serviceAdd.Services)e.Item.DataItem;
                if (currService != null)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        //for anticipo message
                        dbRntEstateExtrasTB currExtra = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == lbl_id.Text.objToInt32());
                        if (currExtra != null)
                        {
                            if (currExtra.isInstantPayment == 0)
                            {
                                div_payNow.Visible = false;
                                lbl_price.Text = currService.price;
                                HF_anticipo.Value = "0";

                            }
                            else
                            {
                                lbl_price.Text = currService.price;
                                HF_commission.Value = currService.commission;
                                lbl_commission.Text = currService.commission;
                                HF_anticipo.Value = "1";

                            }
                        }
                    }
                }
            }
        }

    }
}