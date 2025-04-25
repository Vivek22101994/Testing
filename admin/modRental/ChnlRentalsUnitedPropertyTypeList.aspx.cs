using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace RentalInRome.admin.modRental
{
    public partial class ChnlRentalsUnitedPropertyTypeList : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                fillData();
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            fillData();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "get")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;

            }
        }
        protected void lnk_flt_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected Dictionary<int, string> LocalIds;
        private void fillData()
        {
            //LV.DataSource = rntProps.EstateCategoryVIEW.Where(x => x.isActive == 1 && x.pidLang == App.LangID).ToList();
            //LV.DataBind();

            //using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            //{
            //    var haPropertyTypes = dcChnl.dbRntChnlRentalsUnitedLkPropertyTypeTBLs.OrderBy(x => x.code).ToList();
            //    var properyTypesRL = dcChnl.dbRntChnlRentalsUnitedPropertyTypeRLs.ToList();

            //    foreach (ListViewDataItem item in LV.Items)
            //    {
            //        Label lbl_id = item.FindControl("lbl_id") as Label;
            //        DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;

            //        drp_refId.DataSource = haPropertyTypes;
            //        drp_refId.DataTextField = "title";
            //        drp_refId.DataValueField = "code";
            //        drp_refId.DataBind();

            //        drp_refId.Items.Insert(0, new ListItem("----", "0"));

            //        dbRntChnlRentalsUnitedPropertyTypeRL currTBL = properyTypesRL.SingleOrDefault(x => x.pidCategory == lbl_id.Text.objToInt32());
            //        if (currTBL != null)
            //            drp_refId.setSelectedValue(currTBL.pidHACategory);
            //    }
            //}
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                //delete existing category mapping
                dcChnl.Delete(dcChnl.dbRntChnlRentalsUnitedPropertyTypeRLs.ToList());
                dcChnl.SaveChanges();

                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_id = item.FindControl("lbl_id") as Label;
                    DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;

                    dbRntChnlRentalsUnitedPropertyTypeRL currTBL = new dbRntChnlRentalsUnitedPropertyTypeRL();
                    currTBL.pidCategory = lbl_id.Text.objToInt32();
                    currTBL.pidHACategory = drp_refId.SelectedValue;
                    dcChnl.Add(currTBL);
                    dcChnl.SaveChanges();
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Data Saved Successfully.\", 340, 110);", true);
            fillData();
        }
    }
}