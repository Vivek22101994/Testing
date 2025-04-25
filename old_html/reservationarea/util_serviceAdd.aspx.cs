using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.reservationarea
{
    public partial class util_serviceAdd : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        private RNT_TBL_RESERVATION _currTBL;
        public long serviceId
        {
            get
            {
                return HFserviceId.Value.objToInt64();
            }
            set
            {
                HFserviceId.Value = value.ToString();
            }
        }
        public long resId
        {
            get
            {
                return HFresId.Value.objToInt64();
            }
            set
            {
                HFresId.Value = value.ToString();
            }
        }
        public int langId
        {
            get
            {
                return HFLang.Value.objToInt32();
            }
            set
            {
                HFLang.Value = value.ToString();
            }
        }
        public class Services
        {
            public long serviceId
            {
                get;
                set;
            }

            public string serviceName
            {
                get;
                set;
            }
            public int priceTypeId
            {
                get;
                set;
            }
            public string priceType
            {
                get;
                set;
            }
            public string price
            {
                get;
                set;
            }
            public string commission
            {
                get;
                set;
            }
            public string date
            {
                get;
                set;
            }
            public int adults
            {
                get;
                set;
            }
            public int children
            {
                get;
                set;
            }
            public string dateValue
            {
                get;
                set;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HF_messgae.Value = CurrentSource.getSysLangValue("lbl_message");
                hfd_success.Value = CurrentSource.getSysLangValue("lblRequestSuccess");
                serviceId = Request.QueryString["id"].objToInt64();
                resId = Request.QueryString["res_id"].objToInt64();
                if (Request.QueryString["lang"] != null)
                {
                    langId = Request.QueryString["lang"].objToInt32();
                    App.LangID = langId;
                }
               
                int NumOfGuests = 0;

                // long ReservationId = 97935;
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == resId);

                if (_currTBL != null)
                {
                    DateTime dtStart = _currTBL.dtStart.Value;
                    DateTime dtEnd = _currTBL.dtEnd.Value;
                    drp_adult.bind_Numbers(1, _currTBL.num_adult.Value.objToInt32(), 1, 0);
                    drp_adult.setSelectedValue(_currTBL.num_adult.ToString());
                    drp_child_over.bind_Numbers(1, _currTBL.num_child_over.objToInt32(), 1, 0);
                    drp_child_over.Items.Insert(0, new ListItem("---", "0"));
                    drp_child_over.setSelectedValue(_currTBL.num_child_over.objToInt32());
                    drp_child_min.bind_Numbers(1, _currTBL.num_child_min.objToInt32(), 1, 0);
                    drp_child_min.Items.Insert(0, new ListItem("---", "0"));
                    drp_child_min.setSelectedValue(_currTBL.num_child_min.ToString());
                    lblCheckinDate.Text = dtStart.formatCustom("#dd# #MM# #yy#",langId, "");
                    lblCheckOut.Text = dtEnd.formatCustom("#dd# #MM# #yy#", langId, "");
                    lblTotalnights.Text = Convert.ToString((_currTBL.dtEnd.Value - _currTBL.dtStart.Value).TotalDays.objToInt32());
                    lblAdult.Text = Convert.ToString(_currTBL.num_adult.Value) + " ";
                    lblChildren.Text = "," + Convert.ToString(_currTBL.num_child_over.Value);
                    lblinfants.Text = "," + Convert.ToString(_currTBL.num_child_min.Value);
                    HFRes_adult.Value =  Convert.ToString(_currTBL.num_adult);
                    HFRes_child.Value = Convert.ToString(_currTBL.num_child_over);
                    
                    //fill dates for service
                    drp_date.Items.Clear();
                    for (int i = 0; i <= lblTotalnights.Text.objToInt32(); i++)
                    {
                        drp_date.Items.Add(new ListItem(dtStart.AddDays(i).formatCustom("#dd# #MM# #yy#", langId, ""), dtStart.AddDays(i).JSCal_dateToString()));

                    }

                    //closing days
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var dtCheckDay = dc.dbRntEstateExtrasPeriodTBLs.Where(x => x.service_id == serviceId && x.closingday != null && ((x.dtstart <= _currTBL.dtStart && x.dtend >= _currTBL.dtEnd) || (x.dtstart >= _currTBL.dtStart && x.dtstart <= _currTBL.dtEnd) || (x.dtend >= _currTBL.dtStart && x.dtend <= _currTBL.dtEnd))).ToList();
                        if (dtCheckDay != null && dtCheckDay.Count > 0)
                        {
                            LV_closingDays.DataSource = dtCheckDay;
                            LV_closingDays.DataBind();
                        }
                    }

                    //txt_dtStart.Value = Convert.ToString(_currTBL.dtStart.Value.Date);
                    NumOfGuests = _currTBL.num_adult.Value.objToInt32() + _currTBL.num_child_over.objToInt32();

                    using (DCmodRental dc = new DCmodRental())
                    {

                        //for anticipo message
                        dbRntEstateExtrasTB currExtra = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == serviceId);
                        if (currExtra != null)
                        {
                            if (currExtra.isInstantPayment == 0)
                            {
                                lbl_anticipo_msg.Visible = true;
                                lbl_commission_text.Visible = false;
                                lbl_commission.Visible = false;
                                HFAnticipo.Value = "0";
                            }
                            else
                            {
                                lbl_anticipo_msg.Visible = false;
                                lbl_commission_text.Visible = true;
                                lbl_commission.Visible = true;
                                HFAnticipo.Value = "1";
                            }
                        }


                        //dbRntEstateExtrasTB objExtraService = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == ExtrasId);
                        dbRntEstateExtrasLN objEstateExtrasLN = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == serviceId && x.pidLang == langId);
                        if (objEstateExtrasLN == null || string.IsNullOrEmpty(objEstateExtrasLN.title))
                            objEstateExtrasLN = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == serviceId && x.pidLang == 2);
                        if (objEstateExtrasLN != null)
                        {
                            lblPopupServiceName.Text = objEstateExtrasLN.title;
                        }
                        List<dbRntExtrasPriceTBL> lstRntExtrasPriceTBl = dc.dbRntExtrasPriceTBLs.Where(x => x.pidExtras == serviceId).ToList();

                        if (lstRntExtrasPriceTBl != null && lstRntExtrasPriceTBl.Count > 0)
                        {
                            List<dbRntExtrasPriceTBL> lstRntExtrasPriceTBlfind = lstRntExtrasPriceTBl.FindAll(x => x.minPax != null && x.minPax <= NumOfGuests);
                            if (lstRntExtrasPriceTBlfind != null && lstRntExtrasPriceTBlfind.Count > 0)
                            {
                                
                                   if (lstRntExtrasPriceTBlfind.Count == 1)
                                    {
                                        dvSinglePrice.Visible = true;
                                        dvmultiplePrice.Visible = false;
                                        HFPriceTypeId.Value = lstRntExtrasPriceTBl[0].priceType;
                                        HFPaymentType.Value = lstRntExtrasPriceTBl[0].paymentType;
                                        HFMAxPerson.Value = Convert.ToString(lstRntExtrasPriceTBl[0].maxPax);
                                        lbl_minPax.Text = Convert.ToString(lstRntExtrasPriceTBl[0].minPax);
                                        lbl_maxPax.Text = Convert.ToString(lstRntExtrasPriceTBl[0].maxPax);
                                        
                                        //setting dropdown values of adults and children if payment type is forfait.
                                        //if (lstRntExtrasPriceTBl[0].paymentType == "forfait")
                                        //{
                                            if (lstRntExtrasPriceTBl[0].maxPax < _currTBL.num_adult + _currTBL.num_child_over)
                                            {
                                                if (_currTBL.num_adult >= lstRntExtrasPriceTBl[0].maxPax)
                                                {
                                                    drp_adult.setSelectedValue(lstRntExtrasPriceTBl[0].maxPax);
                                                    drp_child_over.setSelectedValue(0);
                                                    
                                                }
                                                else
                                                {
                                                    int children = lstRntExtrasPriceTBl[0].maxPax.objToInt32() - _currTBL.num_adult.objToInt32();
                                                    drp_adult.setSelectedValue(_currTBL.num_adult);
                                                    drp_child_over.setSelectedValue(children);
                                                }
                                            }
                                        //}
                                        
                                        dbRntExtraPriceTypesLN currPriceType = dc.dbRntExtraPriceTypesLNs.SingleOrDefault(x => x.pidPriceType == HFPriceTypeId.Value.objToInt32() && x.pidLang == langId);
                                        if (currPriceType == null || string.IsNullOrEmpty(currPriceType.title))
                                            currPriceType = dc.dbRntExtraPriceTypesLNs.SingleOrDefault(x => x.pidPriceType == HFPriceTypeId.Value.objToInt32() && x.pidLang == 2);
                                       if (currPriceType != null)
                                        {
                                            lblPriceType.Text = currPriceType.title;
                                            HFPriceType.Value = currPriceType.title;
                                        }
                                        lblPrice.Text = Convert.ToString(lstRntExtrasPriceTBl[0].actualPrice);
                                        HFPrice.Value = Convert.ToString(lstRntExtrasPriceTBl[0].actualPrice);
                                        HFChildPrice.Value = Convert.ToString(lstRntExtrasPriceTBl[0].actualPriceChild);
                                        lblChildPrice.Text = Convert.ToString(lstRntExtrasPriceTBl[0].actualPriceChild);
                                        HFCostPrice.Value = Convert.ToString(lstRntExtrasPriceTBl[0].costPrice);
                                        HFChildCostPrice.Value = Convert.ToString(lstRntExtrasPriceTBl[0].costPriceChild);
                                        //lbl_commission.Text = Convert.ToString(lstRntExtrasPriceTBl[0].actualPrice.objToDecimal() - lstRntExtrasPriceTBl[0].costPrice.objToDecimal()).Replace(".",",");
                                        //HFPaymentType.Value = Convert.ToString(lstRntExtrasPriceTBl[0].paymentType);
                                        //lblTotalPrice.Text = Convert.ToString(lstRntExtrasPriceTBl[0].costPrice);

                                    }
                                    else
                                    {
                                        dvSinglePrice.Visible = false;
                                        dvmultiplePrice.Visible = true;

                                        if (lstRntExtrasPriceTBlfind != null && lstRntExtrasPriceTBlfind.Count > 0)
                                        {
                                            lvPriceType.DataSource = lstRntExtrasPriceTBlfind;
                                            lvPriceType.DataBind();
                                        }

                                        foreach (ListViewDataItem objItem in lvPriceType.Items)
                                        {
                                            RadioButton rdbCheck = (RadioButton)objItem.FindControl("rdbCheck");
                                            Label lvlPrice = (Label)objItem.FindControl("lvlPrice");
                                            Label lvlChildPrice = (Label)objItem.FindControl("lvlChildPrice");
                                            Label lblPriceType = (Label)objItem.FindControl("lblPriceType");
                                            HiddenField HFCurrPriceTypeId = (HiddenField)objItem.FindControl("HFCurrPriceTypeId");
                                            HiddenField HFCurrPaymentType = (HiddenField)objItem.FindControl("HFCurrPaymentType");
                                            HiddenField HFCurrMAxPerson = (HiddenField)objItem.FindControl("HFCurrMAxPerson");
                                            HiddenField HFCurrCostPrice = (HiddenField)objItem.FindControl("HFCurrCostPrice");
                                            HiddenField HFCurrChildCostPrice = (HiddenField)objItem.FindControl("HFCurrChildCostPrice");
                                            
                                            rdbCheck.Checked = true;
                                            HFPrice.Value = lvlPrice.Text;
                                            HFPriceType.Value = lblPriceType.Text;
                                            HFPriceTypeId.Value = HFCurrPriceTypeId.Value;
                                            HFChildPrice.Value = lvlChildPrice.Text;
                                            HFPaymentType.Value = HFCurrPaymentType.Value;
                                            HFMAxPerson.Value = HFCurrMAxPerson.Value;
                                            lbl_commission.Text = Convert.ToString(HFPrice.Value.objToDecimal()-HFCurrCostPrice.Value.objToDecimal()).Replace(",",".");
                                            HFCostPrice.Value = HFCurrCostPrice.Value;
                                            HFChildCostPrice.Value = HFCurrChildCostPrice.Value;

                                            //setting dropdown values of adults and children if payment type is forfait.
                                            //if (HFCurrPaymentType.Value == "forfait")
                                            //{
                                                if (HFCurrMAxPerson.Value.objToInt32() < _currTBL.num_adult + _currTBL.num_child_over)
                                                {
                                                    if (_currTBL.num_adult >= HFCurrMAxPerson.Value.objToInt32())
                                                    {
                                                        drp_adult.setSelectedValue(HFCurrMAxPerson.Value.objToInt32());
                                                        drp_child_over.setSelectedValue(0);
                                                    }
                                                    else
                                                    {
                                                        int children = HFCurrMAxPerson.Value.objToInt32() - _currTBL.num_adult.objToInt32();
                                                        drp_adult.setSelectedValue(_currTBL.num_adult);
                                                        drp_child_over.setSelectedValue(children);
                                                    }
                                                }

                                                
                                            //}
                                            //lblTotalPrice.Text = lvlPrice.Text;
                                            break;
                                        }

                                    }

                                    calculateTotalPrice();
                                
                            }
                            else
                            {
                                dvSinglePrice.Visible = false;
                                dvmultiplePrice.Visible = false;
                                div_request.Style.Add("display", "block");
                                totale.Style.Add("display", "none");
                            }
                        }
                        else
                        {
                            dvSinglePrice.Visible = false;
                            dvmultiplePrice.Visible = false;
                            div_request.Style.Add("display", "block");
                            lbl_anticipo_msg.Visible = false;
                            totale.Style.Add("display", "none");
                        }



                    }
                }
                //DateTime dtCheckindate = _currTBL.dtStart.Value;
                //DateTime dtCheckoutdate = _currTBL.dtEnd.Value;
                //NumOfGuests = _currTBL.num_adult.Value;
                HFOld_adult.Value =Convert.ToString(drp_adult.getSelectedValueInt());
                HFOld_child.Value = Convert.ToString(drp_child_over.getSelectedValueInt());

            }
        }
        protected void lvPriceType_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lblPriceType = (Label)e.Item.FindControl("lblPriceType");
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    dbRntExtrasPriceTBL currPrice = (dbRntExtrasPriceTBL)e.Item.DataItem;
                    if (currPrice != null)
                    {
                        if (currPrice.priceType != null)
                        {

                            dbRntExtraPriceTypesLN currPriceType = dc.dbRntExtraPriceTypesLNs.SingleOrDefault(x => x.pidPriceType == currPrice.priceType.objToInt32() && x.pidLang == langId);
                            if (currPriceType == null || string.IsNullOrEmpty(currPriceType.title))
                                currPriceType = dc.dbRntExtraPriceTypesLNs.SingleOrDefault(x => x.pidPriceType == currPrice.priceType.objToInt32() && x.pidLang == 2);
                            if (currPriceType != null)
                            {
                                lblPriceType.Text = currPriceType.title;
                            }
                        }
                    }
                }
            }
        }
       
        protected void calculateTotalPrice()
        {
            decimal price = 0;
            decimal childprice = 0;
            decimal totalPrice = 0;
            decimal totalcostprice = 0;
            decimal costprice=0;
            decimal childcostprice=0;

            int adult = 0;
            int children = 0;
            string paymentType = "";
            int maxPax = 0;

            adult = drp_adult.getSelectedValueInt();
            children = drp_child_over.getSelectedValueInt();
            price = HFPrice.Value.objToDecimal();
            childprice = HFChildPrice.Value.objToDecimal();
            paymentType = HFPaymentType.Value;
            maxPax = HFMAxPerson.Value.objToInt32();
            costprice = HFCostPrice.Value.objToDecimal();
            childcostprice = HFChildCostPrice.Value.objToDecimal();

            if (paymentType == "forfait")
            {
                totalPrice = price;
                totalcostprice = costprice;
                if (children != 0)
                {
                    totalPrice = totalPrice + childprice;
                    totalcostprice = totalcostprice + childcostprice;
                }
            }
            else
            {
                totalPrice = price * adult;
                totalcostprice = costprice * adult;
                if (children != 0)
                {
                    totalPrice = totalPrice + children * childprice;
                    totalcostprice = totalcostprice + children * childcostprice;
                }
            }
            lblTotalPrice.Text = Convert.ToString(totalPrice.objToDecimal()).Replace(".",",");
            lbl_commission.Text = Convert.ToString(totalPrice.objToDecimal() - totalcostprice.objToDecimal()).Replace(".", ",");
            HFTotalPrice.Value = Convert.ToString(totalPrice.objToDecimal()).Replace(".", ",");
            HFCommission.Value = Convert.ToString(totalPrice.objToDecimal() - totalcostprice.objToDecimal()).Replace(".", ",");
        }

        private void saveData()
        {
            List<Services> lst_services = null;
            int cnt = 0;
            if (Session["services"] != null)
            {
                lst_services = (List<Services>)Session["services"];
                foreach (Services service in lst_services)
                {
                    if (service.date == drp_date.getSelectedText("") && service.serviceId == serviceId)
                    {
                        cnt++;
                    }
                }
                    
             }
                
            
            else
            {
                lst_services = new List<Services>();
            }
            if (cnt == 0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    List<dbRntReservationExtrasTMP> currRes = dc.dbRntReservationExtrasTMPs.Where(x => x.pidExtras == serviceId && x.pidReservation == resId && x.inDate == drp_date.SelectedValue.JSCal_stringToDate()).ToList();
                    if (currRes != null && currRes.Count > 0)
                    {
                        string resMsg = CurrentSource.getSysLangValue("lblReserved");
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert('" + resMsg + "', 340, 110);", true);
                    }
                    else
                    {
                        Services objService = new Services();
                        objService.serviceId = serviceId;
                        objService.serviceName = lblPopupServiceName.Text;
                        objService.priceTypeId = HFPriceTypeId.Value.objToInt32();
                        objService.priceType = HFPriceType.Value;
                        objService.price = HFTotalPrice.Value;
                        objService.commission = HFCommission.Value;
                        //objService.date = Convert.ToString(resId) + "service-" + Convert.ToString(serviceId);
                        //objService.date = Convert.ToString(drp_date.SelectedValue.JSCal_stringToDate().Date);
                        objService.date =drp_date.getSelectedText("");
                        objService.dateValue = drp_date.SelectedValue;
                        objService.adults = drp_adult.getSelectedValueInt();
                        objService.children = drp_child_over.getSelectedValueInt();
                        lst_services.Add(objService);
                        Session["services"] = lst_services;
                        CloseRadWindow(HFcloseArgs.Value);
                    }
                }
            }
            else
            {
                string cartMsg = CurrentSource.getSysLangValue("lblCart");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert('" + cartMsg + "', 340, 110);", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Service is already added for this date.\", 340, 110);", true);
            }

        }
        protected void CloseRadWindow(string args)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CloseRadWindow", "CloseRadWindow('" + args + "');", true);
        }
        protected void CloseRadWindowRequest(string args)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CloseRadWindow", "CloseRadWindowRequest('" + args + "');", true);
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
        }
        protected void lnk_request_save_Click(object sender, EventArgs e)
        {
            lnk_request_save.Enabled = false;
            saveRequest();
        }

        private void saveRequest()
        {
            if (txt_note.Text == "")
            {
                string notemsg = CurrentSource.getSysLangValue("lblNote");
                lnk_request_save.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert('"+notemsg+"', 340, 110);", true);
            }
            else
            {
               
                using (DCmodRental dc = new DCmodRental())
                {
                    string successmsg = CurrentSource.getSysLangValue("lblRequestSuccess");
                    dbRntExtrasRequest currRequest = new dbRntExtrasRequest();
                    currRequest.createdDate = DateTime.Now;
                    currRequest.note = txt_note.Text;
                    currRequest.pidReservation = resId;
                    currRequest.pidExtra = serviceId.objToInt32();
                    dc.Add(currRequest);
                    dc.SaveChanges();
                    txt_note.Text = "";
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('" + successmsg + "', 340, 110);", true);
                    CloseRadWindowRequest(HFcloseArgs.Value);
                    //send mail of service request                                    
                     bool result= rntUtils.rntservice_Request(currRequest);
                     //if (result == true)
                     //{
                      
                     //}


                }
            }

        }

        protected void LV_closingDays_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                dbRntEstateExtrasPeriodTBL currTBLPeriod = (dbRntEstateExtrasPeriodTBL)e.Item.DataItem;
                if (currTBLPeriod != null)
                {
                    Label lbl_dtStart = (Label)e.Item.FindControl("lbl_dtStart");
                    lbl_dtStart.Text = (Convert.ToDateTime(currTBLPeriod.dtstart)).ToString("dd/MM/yyyy");

                    Label lbl_dtEnd = (Label)e.Item.FindControl("lbl_dtEnd");
                    lbl_dtEnd.Text = (Convert.ToDateTime(currTBLPeriod.dtend)).ToString("dd/MM/yyyy");
                }



            }
        }

    }
}