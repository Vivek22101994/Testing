using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using System.Text;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{

    public partial class masterPage : System.Web.UI.MasterPage
    {
        protected List<dbRntExtrasMacroCategoryTB> currTBLMacroCategory;
        protected List<dbRntExtrasCategoryTB> currTBLCategory;
        protected dbRntExtrasMacroCategoryLN currTBLMacroCategoryLN;
        protected dbRntExtrasCategoryLN currTBLCategoryLN;
        protected List<dbRntExtrasSubCategoryTB> currTBLSubCategory;
        protected dbRntExtrasSubCategoryLN currTBLSubCategoryLN;

        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public int resUtils_currCity
        {
            get
            {
                return resUtils.currCity;
            }
        }
        public string CURRENT_SESSION_ID
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                return m.CURRENT_SESSION_ID;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setToolTip", "setToolTip();", true);
            getCategoryMenu();
            var DC_RENTAL = maga_DataContext.DC_RENTAL;
            RNT_TBL_RESERVATION currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == resUtils.CurrentIdReservation_gl);
            if (currTbl != null)
            {
                hl_payment.Visible = currTbl.id > 150000;
                hl_pdf.Visible = currTbl.id > 150000;

                if (currTbl.isWL == 1) //White Label
                {
                    hl_personaldata.Visible = currTbl.id > 150000;
                    hl_agentclientdata.Visible = false;
                }
                else
                {
                    hl_personaldata.Visible = currTbl.id > 150000 && currTbl.agentID.objToInt64() == 0;
                    hl_agentclientdata.Visible = currTbl.agentID.objToInt64() != 0;
                }
                if (currTbl.visa_isRequested.objToInt32().ToString() != "1")
                {
                    HL_visa_isRequested.Visible = false;
                }
                else
                {
                    HL_visa_isRequested.Visible = true;
                }
                if (currTbl.conversionScriptsShown.objToInt32() == 0 && currTbl.payed_total.objToDecimal() > 0 && !Request.Url.AbsoluteUri.ToLower().Contains("http://localhost") && !Request.Url.AbsoluteUri.ToLower().Contains(".magadesign.net") && !Request.Url.AbsoluteUri.ToLower().Contains(".dev.magarental.com") && currTbl.id > 242157)
                {
                    currTbl.conversionScriptsShown = 1;
                    DC_RENTAL.SubmitChanges();
                    pnl_conversionScripts.Visible = true;
                    pnl_adRollScript.Visible = true;
                }
            }
        }
        protected void getCategoryMenu()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbcat = new StringBuilder();
            StringBuilder sbsubcat1 = new StringBuilder();
            StringBuilder sbsubcat2 = new StringBuilder();
            StringBuilder sbsubcat3 = new StringBuilder();
            StringBuilder sbsubcat = new StringBuilder();
            string macrocat_title = "";
            string macrocat_img = "";
            string cat_title = "";
            string subcat_title = "";
            int langId = App.LangID;
            //if (App.LangID != 1)
            //{
            //    langId = 2;
            //}
            using (DCmodRental dc = new DCmodRental())
            {
                currTBLMacroCategory = dc.dbRntExtrasMacroCategoryTBs.Where(x => x.isActive == 1).ToList();
                if (currTBLMacroCategory != null && currTBLMacroCategory.Count > 0)
                {
                    foreach (dbRntExtrasMacroCategoryTB macrocat in currTBLMacroCategory)
                    {
                        macrocat_img = "/" + macrocat.imgThumb;
                        currTBLMacroCategoryLN = dc.dbRntExtrasMacroCategoryLNs.SingleOrDefault(x => x.pidMacroCategory == macrocat.id && x.pidLang == langId);
                        if (currTBLMacroCategoryLN == null || string.IsNullOrEmpty(currTBLMacroCategoryLN.title))
                            currTBLMacroCategoryLN = dc.dbRntExtrasMacroCategoryLNs.SingleOrDefault(x => x.pidMacroCategory == macrocat.id && x.pidLang == 2);
                        if (currTBLMacroCategoryLN != null)
                        {
                            if (currTBLMacroCategoryLN.title != "")
                            {
                                macrocat_title = currTBLMacroCategoryLN.title;
                            }
                            else
                            {
                                macrocat_title = macrocat.code;
                            }
                        }
                        else
                        {
                            macrocat_title = macrocat.code;
                        }
                        currTBLCategory = dc.dbRntExtrasCategoryTBs.Where(x => x.isActive == 1 && x.pidMacroCategory == macrocat.id).ToList();
                        if (currTBLCategory != null && currTBLCategory.Count > 0)
                        {
                            sbcat = new StringBuilder();

                            foreach (dbRntExtrasCategoryTB cat in currTBLCategory)
                            {
                                currTBLCategoryLN = dc.dbRntExtrasCategoryLNs.SingleOrDefault(x => x.pidCategory == cat.id && x.pidLang == langId);
                                if (currTBLCategoryLN == null || string.IsNullOrEmpty(currTBLCategoryLN.title))
                                    currTBLCategoryLN = dc.dbRntExtrasCategoryLNs.SingleOrDefault(x => x.pidCategory == cat.id && x.pidLang == 2);
                                if (currTBLCategoryLN != null)
                                {
                                    if (currTBLCategoryLN.title != "")
                                    {
                                        cat_title = currTBLCategoryLN.title;
                                    }
                                    else
                                    {
                                        cat_title = cat.code;
                                    }
                                }
                                else
                                {
                                    cat_title = cat.code;
                                }
                                //subcategory string append.
                                currTBLSubCategory = dc.dbRntExtrasSubCategoryTBs.Where(x => x.isActive == 1 && x.pidCategory == cat.id).ToList();
                                //if (currTBLSubCategory != null && currTBLSubCategory.Count > 0)
                                //{
                                //    sbsubcat1 = new StringBuilder();
                                //    sbsubcat3 = new StringBuilder();
                                //    sbsubcat = new StringBuilder();
                                //    sbsubcat1.Append("<ul>");
                                //    foreach (dbRntExtrasSubCategoryTB subcat in currTBLSubCategory)
                                //    {
                                //        sbsubcat2 = new StringBuilder();
                                //        currTBLSubCategoryLN = dc.dbRntExtrasSubCategoryLNs.SingleOrDefault(x => x.pidSubCategory == subcat.id && x.pidLang == CurrentLang.ID);
                                //        if (currTBLSubCategoryLN != null)
                                //        {
                                //            if (currTBLSubCategoryLN.title != "")
                                //            {
                                //                subcat_title = currTBLSubCategoryLN.title;
                                //            }
                                //            else
                                //            {
                                //                subcat_title = subcat.code;
                                //            }
                                //        }
                                //        else
                                //        {
                                //            subcat_title = subcat.code;
                                //        }
                                //        sbsubcat2.Append("<li><a href=" + "\"" + "#" + "\"" + ">" + subcat_title + "</a></li>");
                                //    }
                                //    sbsubcat3.Append("</ul>");
                                //    sbsubcat.Append(sbsubcat1);
                                //    sbsubcat.Append(sbsubcat2);
                                //    sbsubcat.Append(sbsubcat3);
                                //    sbcat.Append("<span class='subSubMenu'><a href=" + "\"" + "extraservicecategory.aspx?id=" + cat.id + "\"" + ">" + cat_title + "</a>" + sbsubcat + "</sapn>");
                                //}
                                //else
                                //{
                                sbcat.Append("<a href=" + "\"" + "extraservicecategory.aspx?id=" + cat.id + "\"" + ">" + cat_title + "</a>");
                                //}


                                //

                            }

                            sb.Append("<li><strong><a href=" + "\"" + "#" + "\"" + "style=" + "\"" + "background: url(" + macrocat_img + ") no-repeat scroll 3px 0 transparent" + "\"" + "><span>" + macrocat_title + "</span></a><span class=" + "\"" + "subMenu" + "\"" + ">" + sbcat + "</sapn><strong>");


                        }
                        else
                        {
                            sb.Append("<li><strong><a href=" + "\"" + "#" + "\"" + "style=" + "\"" + "background: url(" + macrocat_img + ") no-repeat scroll 3px 0 transparent" + "\"" + "><span>" + macrocat_title + "</span></a><strong>");
                        }


                    }

                }
                div_category.InnerHtml = sb.ToString();

            }

        }

    }
}