using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using Telerik.Web.UI;

namespace RentalInRome.admin.modRental
{
    public partial class EstateExtraCategoryList1 : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "extras";
        }
        protected dbRntExtrasCategoryTB currTBL;
        protected List<dbRntExtrasSubCategoryTB> currTBLSubCategory;
        protected dbRntExtrasMacroCategoryTB currTBLMacroCategory;
        protected List<dbRntEstateExtrasTB> currTBLExtra;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HfMacroCatId.Value = Request.QueryString["id"].ToInt64().ToString();
                if (HfMacroCatId.Value != "0")
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        currTBLMacroCategory = dc.dbRntExtrasMacroCategoryTBs.SingleOrDefault(x => x.id == HfMacroCatId.Value.ToInt32());
                        lbl_title.Text = "Gestione categorie di" + " " + currTBLMacroCategory.code;
                    }
                }
                else
                {
                    lbl_title.Text = "Gestione categorie";
                }
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
                    currTBL = dc.dbRntExtrasCategoryTBs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());

                    if (currTBL != null)
                    {
                        currTBLSubCategory = dc.dbRntExtrasSubCategoryTBs.Where(x => x.pidCategory == lbl_id.Text.ToInt32()).ToList();
                        currTBLExtra = dc.dbRntEstateExtrasTBs.Where(x => x.pidCategory == lbl_id.Text.ToInt32()).ToList();
                        if (currTBLSubCategory.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Non può essere eliminato perché ha le sottocategoria combinate.\", 340, 110);", true);
                        }
                        else if (currTBLExtra.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Non può essere eliminato perché ha gli extra combinate.\", 340, 110);", true);
                        }
                        else
                        {
                            dc.Delete(currTBL);

                        }
                        dc.SaveChanges();

                    }
                }
            }
                closeDetails(false);
            
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
                _filter += _sep + "code.Contains(\"" + txt_flt_title.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (drp_isActive.SelectedValue != "-1")
            {
                _filter += _sep + "isActive = " + drp_isActive.SelectedValue + "";
                _sep = " and ";
            }
            if(HfMacroCatId.Value!="0")
            _filter += _sep + "pidMacroCategory=" + HfMacroCatId.Value;
           // _filter += _sep + "isActive = 1";
            ltrLDSfiltter.Text = _filter;
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            closeDetails(false);
        }

    }
}