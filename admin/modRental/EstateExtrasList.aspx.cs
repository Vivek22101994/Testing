using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using RentalInRome.data;
using System.Web.UI.HtmlControls;

namespace ModRental.admin.modRental
{
    public partial class EstateExtrasList : adminBasePage
    {
        protected dbRntEstateExtrasTB currTBL;
        protected List<dbRntEstateExtrasPeriodTBL> currTBLDelete;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Bind_drp_cat();
                Bind_drp_subcat(0);
                closeDetails(false);
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
           
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodRental dc = new DCmodRental())
                {
                    
                    currTBL = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        //if (dc.dbRntEstateExtrasRLs.FirstOrDefault(x => x.pidEstateExtras == currTBL.id) != null)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorAlert", "alert('Accessorio non puo essere eliminato!');", true);
                        //}
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                    }
                    currTBLDelete = dc.dbRntEstateExtrasPeriodTBLs.Where(x => x.service_id == lbl_id.Text.ToInt32()).ToList();
                    if (currTBLDelete != null)
                    {
                        dc.Delete(currTBLDelete);
                        dc.SaveChanges();
                    }
                }
                closeDetails(false);
            }
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            setfilters();
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        protected void setfilters()
        {
            string _filter = "";
            string _sep = "";
            if (txt_flt_title.Text.Trim() != "")
            {
                _filter += _sep + "title.Contains(\"" + txt_flt_title.Text.Trim() + "\")";
                _sep = " and ";
            }
            //if (drp_flt_priceType.SelectedValue != "")
            //{
            //    _filter += _sep + "priceType = \"" + drp_flt_priceType.SelectedValue + "\"";
            //    _sep = " and ";
            //}
            if (drp_flt_isImportant.getSelectedValueInt() == 0)
            {
                _filter += _sep + "isImportant = 0";
                _sep = " and ";
            }
            else if (drp_flt_isImportant.getSelectedValueInt() == 1)
            {
                _filter += _sep + "isImportant > 0";
                _sep = " and ";
            }
            if (drp_flt_isInFilters.getSelectedValueInt() == 0)
            {
                _filter += _sep + "isInFilters = 0";
                _sep = " and ";
            }
            else if (drp_flt_isInFilters.getSelectedValueInt() == 1)
            {
                _filter += _sep + "isInFilters > 0";
                _sep = " and ";
            }
            if (drp_flt_hasPeriodLimits.SelectedValue != "-1")
            {
                _filter += _sep + "hasPeriodLimits = " + drp_flt_hasPeriodLimits.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_flt_isInInvoice.SelectedValue != "-1")
            {
                _filter += _sep + "isInInvoice = " + drp_flt_isInInvoice.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_flt_isInstantPayment.SelectedValue != "-1")
            {
                _filter += _sep + "isInstantPayment = " + drp_flt_isInstantPayment.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_flt_isRequired.SelectedValue != "-1")
            {
                _filter += _sep + "isRequired = " + drp_flt_isRequired.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_flt_subcategory.SelectedValue != "-1")
            {
                _filter += _sep + "pidSubCategory = " + drp_flt_subcategory.SelectedValue + "";
                _sep = " and ";
            }
            else if (drp_flt_category.SelectedValue != "-1")
            {
                //List<int> lstSubCat = new List<int>();
                //foreach (ListItem subcat in drp_flt_subcategory.Items)
                //{
                //    if (subcat.Value != "-1")
                //    {
                //        lstSubCat.Add(subcat.Value.objToInt32());

                //    }
                //}
                _filter += _sep + "pidCategory = "+ drp_flt_category.SelectedValue + "";
                _sep = " and ";
            }
            
            _filter += _sep + "pidLang = 1";
            ltrLDSfiltter.Text = _filter;
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                dbRntEstateExtrasVIEW currView = (dbRntEstateExtrasVIEW)e.Item.DataItem;
                using (DCmodRental dc = new DCmodRental())
                {
                    dbRntEstateExtrasTB currTBLExtra = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == currView.id);
                    Label lbl_resArea = (Label)e.Item.FindControl("lbl_resArea");
                               
                    if (currTBLExtra.isInResArea != null)
                    {
                        if(currTBLExtra.isInResArea==0)
                        {
                            lbl_resArea.Text = "No";
                            lbl_resArea.Attributes.Add("style", "color:#FF0000");
                        }
                        else if(currTBLExtra.isInResArea==1)
                        {
                            lbl_resArea.Text = "Si";
                            lbl_resArea.Attributes.Add("style", "color:#00FF00");
                        }
                    }
                    Label lbl_city = (Label)e.Item.FindControl("lbl_city");
                    Label lbl_category = (Label)e.Item.FindControl("lbl_category");
                    Label lbl_subCategory = (Label)e.Item.FindControl("lbl_subCategory");
                    Label lbl_macroCategory = (Label)e.Item.FindControl("lbl_macroCategory");
                    HtmlTableRow tr_extra=(HtmlTableRow)e.Item.FindControl("tr_extra");
                
                    if (currTBLExtra != null)
                    {
                        if (currTBLExtra.pidCity != null)
                        {
                            LOC_VIEW_CITY city = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.SingleOrDefault(x => x.pid_lang == 1 && x.id == currTBLExtra.pidCity);
                            if (city != null)
                            {
                                lbl_city.Text = city.title;
                            }
                        }

                        if (currTBLExtra.pidCategory != null && currTBLExtra.pidCategory!=0)
                        {

                            dbRntExtrasCategoryTB category = dc.dbRntExtrasCategoryTBs.SingleOrDefault(x => x.id == currTBLExtra.pidCategory);
                            if (category.isActive == 0)
                            {
                                tr_extra.Style.Add("display", "none");
                            }
                            else
                            {
                                dbRntExtrasCategoryLN categoryLN = dc.dbRntExtrasCategoryLNs.SingleOrDefault(x => x.pidCategory == currTBLExtra.pidCategory && x.pidLang == 1);
                                if (categoryLN != null)
                                {
                                    lbl_category.Text = categoryLN.title;
                                }
                            }

                        }

                        if (currTBLExtra.pidSubCategory != null && currTBLExtra.pidSubCategory != 0)
                        {
                            dbRntExtrasSubCategoryTB subcategory = dc.dbRntExtrasSubCategoryTBs.SingleOrDefault(x => x.id == currTBLExtra.pidSubCategory);
                            if (subcategory.isActive == 0)
                            {
                                tr_extra.Style.Add("display", "none");
                            }
                            else
                            {
                                dbRntExtrasSubCategoryLN subcategoryLN = dc.dbRntExtrasSubCategoryLNs.SingleOrDefault(x => x.pidSubCategory == currTBLExtra.pidSubCategory && x.pidLang==1);
                                if (subcategoryLN != null)
                                {
                                    lbl_subCategory.Text = subcategoryLN.title;
                                }
                            }
                          

                        }
                        if (currTBLExtra.pidMacroCategory != null && currTBLExtra.pidMacroCategory != 0)
                        {
                           
                            dbRntExtrasMacroCategoryTB macrocategory = dc.dbRntExtrasMacroCategoryTBs.SingleOrDefault(x => x.id == currTBLExtra.pidMacroCategory);
                            if (macrocategory.isActive == 0)
                            {
                                tr_extra.Style.Add("display", "none");
                            }
                            else
                            {
                                dbRntExtrasMacroCategoryLN macrocategoryLN = dc.dbRntExtrasMacroCategoryLNs.SingleOrDefault(x => x.pidMacroCategory == currTBLExtra.pidMacroCategory && x.pidLang == 1);
                                if (macrocategoryLN != null)
                                 {
                                     lbl_macroCategory.Text = macrocategoryLN.title;
                                 }
                            }


                        }
                    }

                    Label lbl_price = (Label)e.Item.FindControl("lbl_price");
                    List<dbRntExtrasPriceTBL> currTBLList = dc.dbRntExtrasPriceTBLs.Where(x => x.pidExtras == currTBLExtra.id).ToList();
                    if (currTBLList.Count > 0)
                    {
                        lbl_price.Text = "Si";
                        lbl_price.Attributes.Add("style", "color:#00FF00");
                    }
                    else
                    {
                        lbl_price.Text = "No";
                        lbl_price.Attributes.Add("style", "color:#FF0000");
                        
                    }

                }
            }
        }

        private void Bind_drp_cat()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntExtrasCategoryTB> lstCat = dc.dbRntExtrasCategoryTBs.Where(x => x.isActive == 1).ToList();
                if (lstCat != null && lstCat.Count > 0)
                {
                    drp_flt_category.Items.Clear();
                    foreach (dbRntExtrasCategoryTB t in lstCat)
                    {
                        dbRntExtrasCategoryLN currCat = dc.dbRntExtrasCategoryLNs.SingleOrDefault(x => x.pidCategory == t.id && x.pidLang==App.LangID);
                        drp_flt_category.Items.Add(new ListItem("" + currCat.title, "" + currCat.pidCategory));

                    }
                    drp_flt_category.Items.Insert(0, new ListItem("- tutti -", "-1"));

                }
            }
        }
        private void Bind_drp_subcat(int cat)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntExtrasSubCategoryTB> lstCat = new List<dbRntExtrasSubCategoryTB>();
                if (cat == 0)
                {
                    lstCat = dc.dbRntExtrasSubCategoryTBs.Where(x => x.isActive == 1).ToList();
                }
                else
                {

                    lstCat = dc.dbRntExtrasSubCategoryTBs.Where(x => x.isActive == 1 && x.pidCategory == cat).ToList();
                }
                
                if (lstCat != null && lstCat.Count > 0)
                {
                    drp_flt_subcategory.Items.Clear();
                    foreach (dbRntExtrasSubCategoryTB t in lstCat)
                    {
                        dbRntExtrasSubCategoryLN currcat = dc.dbRntExtrasSubCategoryLNs.SingleOrDefault(x => x.pidSubCategory == t.id && x.pidLang==App.LangID);
                        drp_flt_subcategory.Items.Add(new ListItem("" + currcat.title, "" + currcat.pidSubCategory));

                    }
                    drp_flt_subcategory.Items.Insert(0, new ListItem("- tutti -", "-1"));

                }
            }
        }
        protected void drp_flt_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drp_flt_category.getSelectedValueInt() == -1)
            {
                Bind_drp_subcat(0);
            }
            else
            {
                Bind_drp_subcat(drp_flt_category.getSelectedValueInt());
            }
        }

    }

}