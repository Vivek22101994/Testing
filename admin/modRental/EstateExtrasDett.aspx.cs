using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.IO;
using RentalInRome.data;


namespace ModRental.admin.modRental
{
    public partial class EstateExtrasDett : adminBasePage
    {
        protected dbRntEstateExtrasTB currTBL;
        private List<dbRntEstateExtrasLN> TMPcurrLangs;
        private List<dbRntEstateExtrasPeriodTBL> currPeriod;
        private magaRental_DataContext DC_RENTAL;
        private List<dbRntEstateExtrasLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbRntEstateExtrasLN)).Cast<dbRntEstateExtrasLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbRntEstateExtrasLN>();

                return TMPcurrLangs;
            }
            set
            {
                ViewState["currLangs"] = PConv.SerialList(value.Cast<object>().ToList());
                TMPcurrLangs = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                //txt_dtStart.Text = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");
                //txt_dtEnd.Text = DateTime.Now.AddDays(10).ToString("dd/MM/yyyy");
                HfId.Value = Request.QueryString["id"].ToInt64().ToString();
                fillData();
            }
        }
        private void fillData()
        {
            string _folder = "images/estate_config";
            if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRntEstateExtrasTB();
                    ltrTitle.Text = "Inserimento nuovo accessorio";
                }
                else
                {
                    ltrTitle.Text = "Modifca accessorio #" + currTBL.id;
                }
                drp_isImportant.setSelectedValue(currTBL.isImportant);
                drp_isInFilters.setSelectedValue(currTBL.isInFilters);
                //drp_priceType.setSelectedValue(currTBL.priceType);
                //txt_priceAmount.Value = currTBL.priceAmount.objToDouble();
                drp_isRequired.setSelectedValue(currTBL.isRequired);
                drp_isInstantPayment.setSelectedValue(currTBL.isInstantPayment);
                drp_isInInvoice.setSelectedValue(currTBL.isInInvoice);
                drp_hasPeriodLimits.setSelectedValue(currTBL.hasPeriodLimits);
                drp_all_estate.setSelectedValue(currTBL.isForAllApartment);
                if (currTBL.isInResArea != null)
                    drp_isInResArea.setSelectedValue(currTBL.isInResArea);
                Bind_drp_city();
                if (currTBL.pidCity != null)
                    drp_city.setSelectedValue(currTBL.pidCity);
                Bind_drp_macroCategory();
                drp_macroCategory.setSelectedValue(currTBL.pidMacroCategory);
                Bind_drp_category();
                drp_category.setSelectedValue(currTBL.pidCategory);
                Bind_drp_subcategory();
                drp_subCategory.setSelectedValue(currTBL.pidSubCategory);
                Bind_drp_owner();
                if (currTBL.pidOwner != null)
                    drp_owner.setSelectedValue(currTBL.pidOwner);
                imgPreview.ImgPathDef = "";
                imgPreview.ImgRoot = _folder;
                imgPreview.ImgPath = currTBL.imgThumb;
                currLangs = dc.dbRntEstateExtrasLNs.Where(x => x.pidEstateExtras == currTBL.id).ToList();
                //currPeriod = dc.dbRntEstateExtrasPeriodTBLs.Where(x => x.service_id == currTBL.id).ToList();

                fillService();
                Bind_chk_estates();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
                List<ClsDays> lstDays = new List<ClsDays>();
                ClsDays objClsDays = new ClsDays();
                objClsDays.Text = "Sunday";
                objClsDays.Value = "Sunday";

                ClsDays objClsDays1 = new ClsDays();
                objClsDays1.Text = "Monday";
                objClsDays1.Value = "Monday";

                ClsDays objClsDays2 = new ClsDays();
                objClsDays2.Text = "Tuesday";
                objClsDays2.Value = "Tuesday";

                ClsDays objClsDays3 = new ClsDays();
                objClsDays3.Text = "Wednesday";
                objClsDays3.Value = "Wednesday";

                ClsDays objClsDays4 = new ClsDays();
                objClsDays4.Text = "Thursday";
                objClsDays4.Value = "Thursday";

                ClsDays objClsDays5 = new ClsDays();
                objClsDays5.Text = "Friday";
                objClsDays5.Value = "Friday";

                ClsDays objClsDays6 = new ClsDays();
                objClsDays6.Text = "Saturday";
                objClsDays6.Value = "Saturday";

                lstDays.Add(objClsDays);
                lstDays.Add(objClsDays1);
                lstDays.Add(objClsDays2);
                lstDays.Add(objClsDays3);
                lstDays.Add(objClsDays4);
                lstDays.Add(objClsDays5);
                lstDays.Add(objClsDays6);

                foreach (ClsDays objdays in lstDays)
                {
                    chkClosingDays.Items.Add(new ListItem(objdays.Text, objdays.Value));
                }


            }
        }
        private void fillService()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currPeriod = dc.dbRntEstateExtrasPeriodTBLs.Where(x => x.service_id == HfId.Value.ToInt32()).ToList();
                if (HfId.Value.ToInt32() == 0)
                {
                    lnkSaveService.Visible = false;
                }
                else
                {
                    lnkSaveService.Visible = true;
                }

                if (currPeriod != null && currPeriod.Count > 0)
                {
                    LVService.DataSource = currPeriod;
                    LVService.DataBind();
                    txt_dtStart.Text = "";
                    txt_dtEnd.Text = "";
                    div_period.Visible = true;


                }
                else
                {
                    LVService.DataSource = null;
                    LVService.DataBind();



                }

            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRntEstateExtrasTB();
                    dc.Add(currTBL);


                }
                var tmpTbl = new dbRntEstateExtrasTB();

                if (currTBL.isImportant.objToInt32() == 0 && drp_isImportant.getSelectedValueInt() == 1)
                {
                    tmpTbl = dc.dbRntEstateExtrasTBs.Where(x => x.isImportant.HasValue && x.isImportant > 0).OrderByDescending(x => x.isImportant).FirstOrDefault();
                    if (tmpTbl != null)
                        currTBL.isImportant = tmpTbl.isImportant + 1;
                    else
                        currTBL.isImportant = 1;
                }
                else if (drp_isImportant.getSelectedValueInt() == 0)
                    currTBL.isImportant = drp_isImportant.getSelectedValueInt();

                if (currTBL.isInFilters.objToInt32() == 0 && drp_isInFilters.getSelectedValueInt() == 1)
                {
                    tmpTbl = dc.dbRntEstateExtrasTBs.Where(x => x.isInFilters.HasValue && x.isInFilters > 0).OrderByDescending(x => x.isInFilters).FirstOrDefault();
                    if (tmpTbl != null)
                        currTBL.isInFilters = tmpTbl.isInFilters + 1;
                    else
                        currTBL.isInFilters = 1;
                }
                else if (drp_isInFilters.getSelectedValueInt() == 0)
                    currTBL.isInFilters = drp_isInFilters.getSelectedValueInt();

                //currTBL.priceType = drp_priceType.SelectedValue;
                //currTBL.priceAmount = txt_priceAmount.Value.objToDecimal();
                currTBL.hasPeriodLimits = drp_hasPeriodLimits.getSelectedValueInt();
                currTBL.isInInvoice = drp_isInInvoice.getSelectedValueInt();
                currTBL.isInstantPayment = drp_isInstantPayment.getSelectedValueInt();
                currTBL.isRequired = drp_isRequired.getSelectedValueInt();
                currTBL.imgThumb = imgPreview.ImgPath;
                currTBL.pidCity = drp_city.getSelectedValueInt();
                currTBL.isInResArea = drp_isInResArea.getSelectedValueInt();
                currTBL.pidMacroCategory = drp_macroCategory.getSelectedValueInt();
                currTBL.pidCategory = drp_category.getSelectedValueInt();
                currTBL.pidSubCategory = drp_subCategory.getSelectedValueInt();
                currTBL.isForAllApartment = Convert.ToInt32(drp_all_estate.SelectedValue);
                currTBL.pidOwner = drp_owner.getSelectedValueInt();
                dc.SaveChanges();
                HfId.Value = Convert.ToString(currTBL.id);
                SaveAllLangs(currTBL.id);
                currPeriod = dc.dbRntEstateExtrasPeriodTBLs.Where(x => x.service_id == currTBL.id).ToList();
                if (currPeriod == null || currPeriod.Count == 0)
                {
                    AddServiceData(currTBL.id);
                }
                save_estates(currTBL.id);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            fillData();
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "0";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
        }
        protected void lnkSaveService_Click(object sender, EventArgs e)
        {
            SaveServiceData();

        }

        protected void SaveServiceData()
        {
            if (txt_dtEnd.Text == "" || txt_dtStart.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Inizio servizio data e la data di fine è necessario.\", 340, 110);", true);
            }
            //else if (txt_amount.Text == "")
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Importo assistenza è necessaria.\", 340, 110);", true);
            //}
            else
            {
                int cnt = 0;
                string strDays = string.Empty;
                foreach (ListItem chkitem in chkClosingDays.Items)
                {
                    if (chkitem.Selected)
                    {
                        if (string.IsNullOrEmpty(strDays))
                        {
                            strDays = chkitem.Value;
                        }
                        else
                        {
                            strDays += "," + chkitem.Value;
                        }

                    }
                }
                using (DCmodRental dc = new DCmodRental())
                {
                    currPeriod = dc.dbRntEstateExtrasPeriodTBLs.Where(x => x.service_id == HfId.Value.ToInt32()).ToList();
                    if (currPeriod != null && currPeriod.Count > 0)
                    {
                        foreach (dbRntEstateExtrasPeriodTBL price in currPeriod)
                        {
                            if ((Convert.ToDateTime(txt_dtStart.Text) >= price.dtstart) && (Convert.ToDateTime(txt_dtStart.Text) <= price.dtend))
                            {
                                if (price.closingday == strDays)
                                {
                                    cnt++;
                                }
                            }

                            if ((Convert.ToDateTime(txt_dtEnd.Text) >= price.dtstart) && (Convert.ToDateTime(txt_dtEnd.Text) <= price.dtend))
                            {
                                if (price.closingday == strDays)
                                {
                                    cnt++;
                                }
                            }
                        }
                    }
                    if (cnt == 0)
                    {
                        dbRntEstateExtrasPeriodTBL currTBLAdd = new dbRntEstateExtrasPeriodTBL();
                        currTBLAdd.service_id = HfId.Value.ToInt32();
                        currTBLAdd.dtstart = Convert.ToDateTime(txt_dtStart.Text);
                        currTBLAdd.dtend = Convert.ToDateTime(txt_dtEnd.Text);
                        foreach (ListItem chkitem in chkClosingDays.Items)
                        {
                            if (chkitem.Selected)
                            {
                                if (string.IsNullOrEmpty(currTBLAdd.closingday))
                                {
                                    currTBLAdd.closingday = chkitem.Value;
                                }
                                else
                                {
                                    currTBLAdd.closingday += "," + chkitem.Value;
                                }
                            }

                        }
                        //currTBLAdd.priceAmount = Convert.ToDecimal(txt_amount.Text);
                        currTBLAdd.isActive = 1;
                        dc.Add(currTBLAdd);
                        dc.SaveChanges();
                        fillService();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"servizio è alreay esistono per questo periodo di tempo.\", 340, 110);", true);
                    }

                }


            }
        }
        protected void AddServiceData(int id)
        {
            if (drp_hasPeriodLimits.SelectedValue == "1")
            {
                if (txt_dtEnd.Text == "" || txt_dtStart.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Inizio servizio data e la data di fine è necessario.\", 340, 110);", true);
                }
                else
                {
                    dbRntEstateExtrasPeriodTBL currTBLAdd = new dbRntEstateExtrasPeriodTBL();
                    currTBLAdd.service_id = id;
                    currTBLAdd.dtstart = Convert.ToDateTime(txt_dtStart.Text);
                    currTBLAdd.dtend = Convert.ToDateTime(txt_dtEnd.Text);
                    //currTBLAdd.priceAmount = Convert.ToDecimal(txt_amount.Text);
                    currTBLAdd.isActive = 1;
                    //currTBLAdd.priceAmount = 100;
                    currTBLAdd.isActive = 1;
                    foreach (ListItem chkitem in chkClosingDays.Items)
                    {
                        if (chkitem.Selected)
                        {
                            if (string.IsNullOrEmpty(currTBLAdd.closingday))
                            {
                                currTBLAdd.closingday = chkitem.Value;
                            }
                            else
                            {
                                currTBLAdd.closingday += "," + chkitem.Value;
                            }
                        }

                    }
                    using (DCmodRental dc = new DCmodRental())
                    {
                        dc.Add(currTBLAdd);
                        dc.SaveChanges();
                    }
                    fillService();
                }


            }
        }

        protected void LvService_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteService")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                int id = Convert.ToInt32(lbl_id.Text);

                dbRntEstateExtrasPeriodTBL currTBLDelete = new dbRntEstateExtrasPeriodTBL();
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBLDelete = dc.dbRntEstateExtrasPeriodTBLs.SingleOrDefault(x => x.id == id);
                    dc.Delete(currTBLDelete);

                    dc.SaveChanges();

                }
                fillService();

            }
        }

        protected void LvService_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                dbRntEstateExtrasPeriodTBL currTBLActivate = (dbRntEstateExtrasPeriodTBL)e.Item.DataItem;
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

                Label lbl_dtStart = (Label)e.Item.FindControl("lbl_dtStart");
                lbl_dtStart.Text = (Convert.ToDateTime(currTBLActivate.dtstart)).ToString("dd/MM/yyyy");

                Label lbl_dtEnd = (Label)e.Item.FindControl("lbl_dtEnd");
                lbl_dtEnd.Text = (Convert.ToDateTime(currTBLActivate.dtend)).ToString("dd/MM/yyyy");

                Label lblClosingDays = (Label)e.Item.FindControl("lblClosingDays");
                lblClosingDays.Text = currTBLActivate.closingday;


            }
        }


        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected void BindLvLangs()
        {
            LvLangs.DataSource = contProps.LangTBL.Where(x => x.is_active == 1).OrderBy(x => x.id);
            LvLangs.DataBind();
        }
        protected void LvLangs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnkLang");
            lnk.CssClass = HfLang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
        }
        protected void LvLangs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                SaveLang();
                HfLang.Value = lbl_id.Text;
                FillLang();
                BindLvLangs();
            }
        }
        protected void SaveLang()
        {
            var currLangsTmp = currLangs;
            var rlLang = currLangsTmp.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRntEstateExtrasLN();
                rlLang.pidEstateExtras = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.title = txt_title.Text;
            rlLang.sommario = txt_Sommario.Text;
            rlLang.description = txt_description.Content;
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRntEstateExtrasLN();
            }
            txt_title.Text = rlLang.title;
            txt_Sommario.Text = rlLang.sommario;
            txt_description.Content = rlLang.description;
        }
        protected void SaveAllLangs(int id)
        {
            SaveLang();
            using (DCmodRental dc = new DCmodRental())
            {
                foreach (var rl in currLangs)
                {
                    if (dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidEstateExtras = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbRntEstateExtrasLNs.Single(x => x.pidEstateExtras == id && x.pidLang == rl.pidLang);
                        rl.CopyTo(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }

        protected void drp_hasPeriodLimits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drp_hasPeriodLimits.SelectedValue == "0")
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    currPeriod = dc.dbRntEstateExtrasPeriodTBLs.Where(x => x.service_id == HfId.Value.ToInt32()).ToList();
                    if (currPeriod.Count > 0)
                    {
                        dc.Delete(currPeriod);
                        dc.SaveChanges();
                    }
                }
                div_period.Visible = false;
            }
            else if (drp_hasPeriodLimits.SelectedValue == "1")
            {
                div_period.Visible = true;


            }
        }
        private void Bind_drp_city()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_city.Items.Clear();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_city.Items.Add(new ListItem(t.title, "" + t.id));
            }
            drp_city.Items.Insert(0, new ListItem("-seleziona-", "0"));
        }
        private void Bind_drp_owner()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntExtraOwnerTBL> list = dc.dbRntExtraOwnerTBLs.Where(x => x.isActive == 1).ToList();
                drp_owner.Items.Clear();
                foreach (dbRntExtraOwnerTBL t in list)
                {
                    drp_owner.Items.Add(new ListItem(t.nameCompany, "" + t.id));
                }
                drp_owner.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
        }
        private void Bind_drp_macroCategory()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntExtrasMacroCategoryTB> macrocatList = dc.dbRntExtrasMacroCategoryTBs.Where(x => x.isActive == 1).ToList();
                drp_macroCategory.Items.Clear();
                foreach (dbRntExtrasMacroCategoryTB macrocat in macrocatList)
                {
                    drp_macroCategory.Items.Add(new ListItem(macrocat.code, "" + macrocat.id));
                }
                drp_macroCategory.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
        }
        private void Bind_drp_category()
        {
            string macrocategory = drp_macroCategory.SelectedValue;
            List<dbRntExtrasCategoryTB> catList = new List<dbRntExtrasCategoryTB>();
            using (DCmodRental dc = new DCmodRental())
            {
                if (macrocategory != "0")
                {
                    catList = dc.dbRntExtrasCategoryTBs.Where(x => x.isActive == 1 && x.pidMacroCategory == Convert.ToInt32(macrocategory)).ToList();
                }

                drp_category.Items.Clear();
                if (catList != null)
                {
                    foreach (dbRntExtrasCategoryTB cat in catList)
                    {
                        drp_category.Items.Add(new ListItem(cat.code, "" + cat.id));
                    }
                }
                drp_category.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
        }
        private void Bind_drp_subcategory()
        {

            string category = drp_category.SelectedValue;
            List<dbRntExtrasSubCategoryTB> subcatList = new List<dbRntExtrasSubCategoryTB>();
            using (DCmodRental dc = new DCmodRental())
            {
                if (category != "0")
                {
                    subcatList = dc.dbRntExtrasSubCategoryTBs.Where(x => x.isActive == 1 && x.pidCategory == Convert.ToInt32(category)).ToList();
                }

                drp_subCategory.Items.Clear();
                if (subcatList != null)
                {
                    foreach (dbRntExtrasSubCategoryTB subcat in subcatList)
                    {
                        drp_subCategory.Items.Add(new ListItem(subcat.code, "" + subcat.id));
                    }
                }
                drp_subCategory.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
        }

        protected void drp_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_drp_subcategory();

        }

        protected void drp_macroCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_drp_category();

        }

        protected void drp_all_estate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_chk_estates();
        }

        private void Bind_chk_estates()
        {
            if (drp_all_estate.SelectedValue == "0")
            {
                chk_estates.Visible = true;
                List<RNT_TB_ESTATE> estatelist = new List<RNT_TB_ESTATE>();
                estatelist = DC_RENTAL.RNT_TB_ESTATE.Where(item => item.is_active == 1).ToList();
                if (estatelist != null)
                {
                    chk_estates.DataSource = estatelist;
                    chk_estates.DataTextField = "code";
                    chk_estates.DataValueField = "id";
                    chk_estates.DataBind();

                    using (DCmodRental dc = new DCmodRental())
                    {
                        List<dbRntEstateExtrasRL> estateextras = new List<dbRntEstateExtrasRL>();
                        estateextras = dc.dbRntEstateExtrasRLs.Where(item => item.pidEstateExtras == HfId.Value.ToInt32()).ToList();
                        List<string> lstEstates = new List<string>();
                        foreach (dbRntEstateExtrasRL estateextra in estateextras)
                        {
                            lstEstates.Add(Convert.ToString(estateextra.pidEstate));
                        }
                        if (lstEstates.Count > 0)
                            chk_estates.setSelectedValues(lstEstates);
                    }


                }
                else
                {
                    chk_estates.DataSource = null;
                    chk_estates.DataBind();
                }


            }
            else
            {
                chk_estates.Visible = false;
            }
        }

        private void save_estates(int id)
        {


            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntEstateExtrasRL> estateextras = new List<dbRntEstateExtrasRL>();
                estateextras = dc.dbRntEstateExtrasRLs.Where(item => item.pidEstateExtras == HfId.Value.ToInt32()).ToList();
                if (estateextras.Count > 0)
                {
                    dc.Delete(estateextras);
                    dc.SaveChanges();
                }
                if (drp_all_estate.SelectedValue == "0")
                {
                    List<string> lstEstates = new List<string>();
                    lstEstates = chk_estates.getSelectedValueList();
                    if (lstEstates.Count > 0)
                    {

                        foreach (string estate in lstEstates)
                        {
                            dbRntEstateExtrasRL currTBLEstate = new dbRntEstateExtrasRL();
                            currTBLEstate.pidEstate = Convert.ToInt32(estate);
                            currTBLEstate.pidEstateExtras = id;
                            currTBLEstate.isFixed = 1;
                            dc.Add(currTBLEstate);
                            dc.SaveChanges();


                        }

                    }
                }
            }

        }

    }
    public class ClsDays
    {
        public string Text
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }

    }
}

