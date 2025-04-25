using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ModRental;
using RentalInRome.data;

namespace MagaRentalCE.admin.modRental
{
    public partial class ChnlExpediaAmenitiesList : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                using (DCchnlExpedia dcChnl = new DCchnlExpedia())
                {
                    var list = dcChnl.dbRntChnlExpediaLkAmenitiesTBLs.ToList();
                }
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
            if (LocalIds == null)
            {
                LocalIds = new Dictionary<int, string>();
                var LocalIdsList = maga_DataContext.DC_RENTAL.RNT_TB_CONFIGs.Where(x => 1 == 1).ToList();
                foreach (var tmp in LocalIdsList)
                {
                    var tmpLn = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == App.LangID);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == App.DefLangID);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == 2);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = maga_DataContext.DC_RENTAL.RNT_VIEW_CONFIGs.SingleOrDefault(x => x.id == tmp.id && x.pid_lang == 1);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title))
                        continue;
                    LocalIds.Add(tmp.id, tmpLn.title);
                }
            }
            using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            using (DCmodRental dc = new DCmodRental())
            {
                var list = dcChnl.dbRntChnlExpediaLkAmenitiesTBLs.ToList();
                LV.DataSource = list.OrderBy(x => x.name).ToList();
                LV.DataBind();
                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_id = item.FindControl("lbl_id") as Label;
                    Label lbl_name = item.FindControl("lbl_name") as Label;
                    Label lbl_refId = item.FindControl("lbl_refId") as Label;
                    DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;
                    DropDownList drp_detailCode = item.FindControl("drp_detailCode") as DropDownList;

                    drp_refId.Items.Add(new ListItem("- - -", ""));
                    var extraList = LocalIds.OrderBy(x => x.Value).ToList();
                    foreach (var extra in extraList)
                    {
                        drp_refId.Items.Add(new ListItem(extra.Value, "" + extra.Key));
                    }
                    drp_refId.setSelectedValue(lbl_refId.Text);

                    var expediaAmenity = dcChnl.dbRntChnlExpediaLkAmenitiesTBLs.SingleOrDefault(x => x.id == lbl_id.Text.objToInt32());
                    if (expediaAmenity != null)
                    {

                        var amenitiesDetailCodes = dcChnl.dbRntChnlExpediaAmenitiesDetailCodeTBLs.Where(x => x.pidAmenity == lbl_id.Text.objToInt32());
                        foreach (var detailCode in amenitiesDetailCodes)
                        {
                            drp_detailCode.Items.Add(new ListItem("" + detailCode.DetailCode, "" + detailCode.id));
                        }
                        drp_detailCode.Items.Insert(0, new ListItem("No", "0"));
                        drp_detailCode.setSelectedValue(expediaAmenity.pidDetailCode);

                        if (expediaAmenity.isDetailCodeRequired == 1 && drp_detailCode.Items.Count > 1)
                            drp_detailCode.Style.Add("border-color", "red");
                    }

                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCchnlExpedia dcChnl = new DCchnlExpedia())
            {
                int cnt = 0;
                foreach (ListViewDataItem item in LV.Items)
                {
                    DropDownList drp_detailCode = item.FindControl("drp_detailCode") as DropDownList;
                    Label lbl_detail_code_required = item.FindControl("lbl_detail_code_required") as Label;
                    Label lbl_name = item.FindControl("lbl_name") as Label;
                    DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;


                    if (lbl_detail_code_required.Text.objToInt32() == 1 && drp_detailCode.getSelectedValueInt() == 0 && drp_refId.SelectedValue != "" && drp_detailCode.Items.Count > 1)
                    {
                        string text = "Select Dropdown Value for amenity " + lbl_name.Text;
                        //cnt = cnt + 1;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert1", "alert(\"" + text + "\" , 340, 110);", true);
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Select all Mandatory Dropdown Values highlighed in red color for amenity.\", 340, 110);", true);
                        return;
                    }
                }

                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_name = item.FindControl("lbl_name") as Label;
                    Label lbl_refId = item.FindControl("lbl_refId") as Label;
                    DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;
                    DropDownList drp_detailCode = item.FindControl("drp_detailCode") as DropDownList;

                    var featureValuesTbl = dcChnl.dbRntChnlExpediaLkAmenitiesTBLs.SingleOrDefault(x => x.name == lbl_name.Text);
                    if (featureValuesTbl != null)
                    {
                        featureValuesTbl.refId = drp_refId.getSelectedValueInt();
                        featureValuesTbl.pidDetailCode = drp_detailCode.getSelectedValueInt();
                        dcChnl.SaveChanges();
                    }
                }
            }
            fillData();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Data saved successfully.\", 340, 110);", true);
        }
    }
}