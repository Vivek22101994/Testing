using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ModRental;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{
    public partial class extraservicecategory : basePage
    {
        public int PidCategoryId = 0;
        protected dbRntExtrasCategoryTB currTBL;
        protected dbRntExtrasCategoryLN currTBLCategory;
        protected dbRntExtrasCategoryLN currTBLDefaultCategory;
        protected List<dbRntExtrasSubCategoryTB> lstSubCategoryTB;
        protected List<dbRntExtrasSubCategoryLN> lstSubCategoryLN;
        protected List<dbRntExtrasSubCategoryLN> lstFinalSubCategoryLN = new List<dbRntExtrasSubCategoryLN>();
        protected List<dbRntEstateExtrasTB> lstFinalEstateExtrasTB = new List<dbRntEstateExtrasTB>();
        private RNT_TBL_RESERVATION _currTBL;
        private magaRental_DataContext DC_RENTAL;
        public static int NumOfGuests = 0;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                langId = App.LangID;               
                PidCategoryId = Convert.ToInt32(Request.QueryString["id"]);
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntExtrasCategoryTBs.SingleOrDefault(x => x.id == PidCategoryId);
                    currTBLCategory = dc.dbRntExtrasCategoryLNs.SingleOrDefault(x => x.pidCategory == PidCategoryId && x.pidLang == langId);
                    currTBLDefaultCategory = dc.dbRntExtrasCategoryLNs.SingleOrDefault(x => x.pidCategory == PidCategoryId && x.pidLang == 2);
                    if (currTBLCategory == null)
                        currTBLCategory = currTBLDefaultCategory;
                    if (currTBLCategory != null)
                    {
                        if (!string.IsNullOrEmpty(currTBLCategory.title))
                            lblCateogryName.Text = currTBLCategory.title;
                        else
                            lblCateogryName.Text = currTBLDefaultCategory.title;
                        if (!string.IsNullOrEmpty(currTBLCategory.description))
                            lbl_cat_desc.Text = currTBLCategory.description;
                        else
                            lbl_cat_desc.Text = currTBLDefaultCategory.description;
                        img_category.ImageUrl = "/" + currTBL.imgThumb;
                    }
                }

                List<int> lstCategory = new List<int>();
                if (Session["category"] != null)
                {
                    lstCategory = (List<int>)Session["category"];
                }
                else
                {
                    lstCategory = new List<int>();
                }
                if (!lstCategory.Contains(PidCategoryId))
                {
                    lstCategory.Add(PidCategoryId);
                }
                Session["category"] = lstCategory;
                filldata();

            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "extra_category";
            int id = Request.QueryString["id"].ToInt32();
            if (id != 0)
                base.PAGE_REF_ID = id;
            else
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "extraservicecategory.aspx", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
        }
        protected void filldata()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                hfd_hide.Value = CurrentSource.getSysLangValue("lblHide");
                hfd_visible.Value = CurrentSource.getSysLangValue("lblShowMore");
                hfd_addService.Value = CurrentSource.getSysLangValue("lblAddService");

                lstSubCategoryTB = dc.dbRntExtrasSubCategoryTBs.Where(x => x.pidCategory == PidCategoryId && x.isActive == 1).ToList();
                //foreach (dbRntExtrasSubCategoryTB objSubCateogryTb in lstSubCategoryTB)
                //{
                //    if (objSubCateogryTb.id > 0)
                //    {
                //        lstSubCategoryLN = dc.dbRntExtrasSubCategoryLNs.Where(x => x.pidSubCategory == objSubCateogryTb.id && x.pidLang == App.LangID).ToList();
                //        lstFinalSubCategoryLN.AddRange(lstSubCategoryLN);
                //    }

                //}
                //LVSubCategory.DataSource = lstFinalSubCategoryLN;
                //LVSubCategory.DataBind();
                if (lstSubCategoryTB != null && lstSubCategoryTB.Count > 0)
                {
                    foreach (dbRntExtrasSubCategoryTB objSubCategoryTb in lstSubCategoryTB)
                    {
                        List<dbRntEstateExtrasTB> lstEstateExtras = dc.dbRntEstateExtrasTBs.Where(x => x.pidSubCategory == objSubCategoryTb.id).ToList();
                        lstFinalEstateExtrasTB.AddRange(lstEstateExtras);

                    }
                }

                long ReservationId = UC_sx1.CurrentIdReservation;
                // long ReservationId = 97935;
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == ReservationId);

                if (_currTBL != null)
                {
                    //DateTime dtCheckindate = _currTBL.dtStart.Value;
                    //DateTime dtCheckoutdate = _currTBL.dtEnd.Value;
                    //int person = _currTBL.num_adult.objToInt32() + _currTBL.num_child_over.objToInt32();
                    NumOfGuests = _currTBL.num_adult.objToInt32() + _currTBL.num_child_over.objToInt32();

                    //foreach (dbRntEstateExtrasTB objExtrasTB in lstFinalEstateExtrasTB)
                    //{
                    //    if (objExtrasTB.hasPeriodLimits == 1)
                    //    {
                    //        while (dtCheckindate.Date <= dtCheckoutdate.Date)
                    //        {
                    //            DateTime dtCurrentdate = dtCheckindate.Date;
                    //            var dtCheckDay = dc.dbRntEstateExtrasPeriodTBLs.FirstOrDefault(x => x.service_id == objExtrasTB.id && x.closingday != null && x.closingday.ToLower().Contains(dtCurrentdate.DayOfWeek.ToString().ToLower()) && x.dtstart.Value.Date <= dtCurrentdate.Date && x.dtend.Value.Date >= dtCurrentdate.Date);
                    //            if (dtCheckDay != null)
                    //            {
                    //                if (lstFinalEstateExtrasTB.Contains(objExtrasTB))
                    //                {
                    //                    lstFinalEstateExtrasTB.Remove(objExtrasTB);
                    //                }
                    //            }
                    //            dtCheckindate = dtCheckindate.Date.AddDays(1);
                    //        }
                    //    }


                    //}

                    //filter using prices(mix and max pax calculation)
                    //if (lstFinalEstateExtrasTB != null && lstFinalEstateExtrasTB.Count > 0)
                    //{
                    //    List<dbRntEstateExtrasTB> lstRemoveExtrasPrice = new List<dbRntEstateExtrasTB>();
                    //    foreach (dbRntEstateExtrasTB objExtrasTB in lstFinalEstateExtrasTB)
                    //    {
                    //        List<dbRntExtrasPriceTBL> lstRntExtrasPriceTBlfind = dc.dbRntExtrasPriceTBLs.Where(x => x.minPax != null && x.minPax <= NumOfGuests && x.maxPax >= NumOfGuests && x.pidExtras == objExtrasTB.id).ToList();
                    //        if (lstRntExtrasPriceTBlfind.Count==0)
                    //        {
                    //            lstRemoveExtrasPrice.Add(objExtrasTB);
                    //        }
                    //    }


                    //    if (lstRemoveExtrasPrice != null && lstRemoveExtrasPrice.Count > 0)
                    //    {
                    //        foreach (dbRntEstateExtrasTB objextra in lstRemoveExtrasPrice)
                    //        {
                    //            if (lstFinalEstateExtrasTB.Contains(objextra))
                    //            {
                    //                lstFinalEstateExtrasTB.Remove(objextra);
                    //            }
                    //        }
                    //    }
                    //}


                    //filter for forfait payment type
                    //if (lstFinalEstateExtrasTB != null && lstFinalEstateExtrasTB.Count > 0)
                    //{
                    //    List<dbRntEstateExtrasTB> lstRemoveExtras = new List<dbRntEstateExtrasTB>();
                    //    foreach (dbRntEstateExtrasTB objExtrasTB in lstFinalEstateExtrasTB)
                    //    {

                    //        List<dbRntExtrasPriceTBL> currPrice = dc.dbRntExtrasPriceTBLs.Where(x => x.pidExtras == objExtrasTB.id).ToList();
                    //        if (currPrice != null && currPrice.Count > 0)
                    //        {
                    //            int cnt = 0;
                    //            foreach (dbRntExtrasPriceTBL price in currPrice)
                    //            {
                    //                if (price.paymentType == "forfait")
                    //                {
                    //                    if (price.maxPax < NumOfGuests)
                    //                    {
                    //                        cnt++;
                    //                    }
                    //                }
                    //            }
                    //            if (cnt == currPrice.Count)
                    //            {
                    //                lstRemoveExtras.Add(objExtrasTB);
                    //            }

                    //        }
                    //    }

                    //    if (lstRemoveExtras != null && lstRemoveExtras.Count > 0)
                    //    {
                    //        foreach (dbRntEstateExtrasTB objextra in lstRemoveExtras)
                    //        {
                    //            if (lstFinalEstateExtrasTB.Contains(objextra))
                    //            {
                    //                lstFinalEstateExtrasTB.Remove(objextra);
                    //            }
                    //        }
                    //    }
                    //}
                }

                if (lstFinalEstateExtrasTB != null && lstFinalEstateExtrasTB.Count > 0)
                {
                    LVExtraServices.DataSource = lstFinalEstateExtrasTB;
                    LVExtraServices.DataBind();
                }
            }


        }


        protected void LVSubCategory_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                dbRntExtrasSubCategoryLN objSubCategory = (dbRntExtrasSubCategoryLN)e.Item.DataItem;
                ListView LVExtraServices = (ListView)e.Item.FindControl("LVExtraServices");

            }
        }

        protected void LVExtraServices_ItemCommand(object sender, ListViewCommandEventArgs e)
        {



        }
        protected void LVExtraServices_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                dbRntEstateExtrasTB objExtraCategory = (dbRntEstateExtrasTB)e.Item.DataItem;
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                Label lblServiceName = (Label)e.Item.FindControl("lblServiceName");
                Label lblDescription = (Label)e.Item.FindControl("lblDescription");
                Label lblSommario = (Label)e.Item.FindControl("lblSommario");
                Label lblMinPerson = (Label)e.Item.FindControl("lblMinPerson");
                Label lblMaxPerson = (Label)e.Item.FindControl("lblMaxPerson");
                Label lblhours = (Label)e.Item.FindControl("lblhours");
                Label lblDays = (Label)e.Item.FindControl("lblDays");
                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                DataList dlstIamges = (DataList)e.Item.FindControl("dlstIamges");
                HtmlGenericControl div_request = (HtmlGenericControl)e.Item.FindControl("div_request");
                HtmlGenericControl div_price = (HtmlGenericControl)e.Item.FindControl("div_price");

                using (DCmodRental dc = new DCmodRental())
                {
                    dbRntEstateExtrasLN objDefaultEstateExtrasLN = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == objExtraCategory.id && x.pidLang == 2);
                    dbRntEstateExtrasLN objEstateExtrasLN = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == objExtraCategory.id && x.pidLang == langId);
                    if (objEstateExtrasLN == null)
                        objEstateExtrasLN = objDefaultEstateExtrasLN;

                    List<dbRntExtrasPriceTBL> lstRntExtrasPriceTBl = dc.dbRntExtrasPriceTBLs.Where(x => x.pidExtras == objExtraCategory.id).ToList();
                    if (lstRntExtrasPriceTBl != null && lstRntExtrasPriceTBl.Count > 0)
                    {
                        //List<dbRntExtrasPriceTBL> lstRntExtrasPriceTBlfind = lstRntExtrasPriceTBl.FindAll(x => x.minPax != null && x.minPax <= NumOfGuests && x.maxPax >= NumOfGuests);

                        if (lstRntExtrasPriceTBl.Count >= 2)
                        {
                            var query = lstRntExtrasPriceTBl.GroupBy(x => x.pidExtras)
                                        .Select(group => group.Where(x => x.actualPrice == group.Min(y => y.actualPrice))
                                        .First()).ToList();

                            lblMinPerson.Text = Convert.ToString(query[0].minPax);
                            lblMaxPerson.Text = Convert.ToString(query[0].maxPax);
                            lblhours.Text = Convert.ToString(query[0].Hours);
                            lblDays.Text = Convert.ToString(query[0].Days);
                            lblPrice.Text = Convert.ToString(query[0].actualPrice) + "€";
                        }
                        else
                        {
                            lblMinPerson.Text = Convert.ToString(lstRntExtrasPriceTBl[0].minPax);
                            lblMaxPerson.Text = Convert.ToString(lstRntExtrasPriceTBl[0].maxPax);
                            lblhours.Text = Convert.ToString(lstRntExtrasPriceTBl[0].Hours);
                            lblDays.Text = Convert.ToString(lstRntExtrasPriceTBl[0].Days);
                            lblPrice.Text = Convert.ToString(lstRntExtrasPriceTBl[0].actualPrice) + "€";
                        }
                        //}
                        //else
                        //{
                        //    div_request.Style.Add("display", "block");
                        //    div_price.Style.Add("display", "none");
                        //    //div_request.Visible = true;
                        //    //div_request.Visible = false;
                        //}
                    }
                    else
                    {
                        div_request.Style.Add("display", "block");
                        div_price.Style.Add("display", "none");
                    }
                    if (objEstateExtrasLN != null)
                    {
                        if (!string.IsNullOrEmpty(objEstateExtrasLN.title))
                           lblServiceName.Text = objEstateExtrasLN.title;
                        else
                            lblServiceName.Text = objDefaultEstateExtrasLN.title;
                        if (!string.IsNullOrEmpty(objEstateExtrasLN.description))
                        lblDescription.Text = objEstateExtrasLN.description;
                        else
                            lblDescription.Text = objDefaultEstateExtrasLN.description;
                        if (!string.IsNullOrEmpty(objEstateExtrasLN.sommario))
                        lblSommario.Text = objEstateExtrasLN.sommario;
                        else
                            lblSommario.Text = objDefaultEstateExtrasLN.sommario;
                    }
                    lbl_id.Text = Convert.ToString(objExtraCategory.id);
                    List<dbRntExtrasMediaRL> lstExtrasMediaRL = dc.dbRntExtrasMediaRLs.Where(x => x.pid_estate_extra == objExtraCategory.id).ToList();
                    dlstIamges.DataSource = lstExtrasMediaRL;
                    dlstIamges.DataBind();

                    //List<dbRntExtrasPriceTBL> lstPrice = dc.dbRntExtrasPriceTBLs.Where(x => x.pidExtras == objExtraCategory.id).ToList();
                    //if (lstPrice != null && lstPrice.Count > 0)
                    //{
                    //}


                }

            }
        }
    }
}