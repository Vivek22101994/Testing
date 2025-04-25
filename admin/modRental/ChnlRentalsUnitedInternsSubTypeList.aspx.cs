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
    public partial class ChnlRentalsUnitedInternsSubTypeList : adminBasePage
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

        protected Dictionary<int, string> LocalIds;

        private void fillData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var RoomSubTypes = dc.dbRntEstateInternsSubTypeVIEWs.Where(x => x.pidInternsType == drp_flt_type.SelectedValue && x.pidLang == App.LangID && x.isActive == 1).ToList();
                LV.DataSource = RoomSubTypes;
                LV.DataBind();

                using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
                {
                    var haRoomSubTypes = dcChnl.dbRntChnlRentalsUnitedLkRoomSubTypeTBLs.Where(x => x.type == drp_flt_type.SelectedValue).OrderBy(x => x.title).ToList();
                    var roomSubTypesRL = dcChnl.dbRntChnlRentalsUnitedInternSubTypeRLs.ToList();

                    foreach (ListViewDataItem item in LV.Items)
                    {
                        Label lbl_id = item.FindControl("lbl_id") as Label;
                        DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;

                        drp_refId.DataSource = haRoomSubTypes;
                        drp_refId.DataTextField = "title";
                        drp_refId.DataValueField = "code";
                        drp_refId.DataBind();

                        drp_refId.Items.Insert(0, new ListItem("----", ""));

                        dbRntChnlRentalsUnitedInternSubTypeRL currTBL = roomSubTypesRL.SingleOrDefault(x => x.pidInternSubType == lbl_id.Text.objToInt32());
                        if (currTBL != null)
                            drp_refId.setSelectedValue(currTBL.pidHAInternSubType);
                    }
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                //delete existing category mapping
                //dcChnl.Delete(dcChnl.dbRntChnlRentalsUnitedInternsRLs.Where(x=>).ToList());
                // dcChnl.SaveChanges();

                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_id = item.FindControl("lbl_id") as Label;
                    DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;

                    var currTBL = dcChnl.dbRntChnlRentalsUnitedInternSubTypeRLs.SingleOrDefault(x => x.pidInternSubType == lbl_id.Text.objToInt32());

                    if (currTBL == null)
                    {
                        if (!string.IsNullOrWhiteSpace(drp_refId.SelectedValue))
                        {
                            currTBL = new dbRntChnlRentalsUnitedInternSubTypeRL();
                            currTBL.pidInternSubType = lbl_id.Text.objToInt32();
                            dcChnl.Add(currTBL);
                            currTBL.pidHAInternSubType = drp_refId.SelectedValue;
                            dcChnl.SaveChanges();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(drp_refId.SelectedValue))
                        {
                            // update 
                            currTBL.pidHAInternSubType = drp_refId.SelectedValue;
                            dcChnl.SaveChanges();
                        }
                        else
                        {
                            // Delete if exist and 
                            dcChnl.Delete(currTBL);
                            dcChnl.SaveChanges();
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Data Saved Successfully.\", 340, 110);", true);
            fillData();
        }

        protected void drp_flt_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillData();
        }
    }
}