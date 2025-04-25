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
    public partial class ChnlAirbnbAmenity : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                using (DCmodRental dcChnl = new DCmodRental())
                {
                    var list = dcChnl.dbRntChnlAirbnbLkAmenityTBLs.ToList();
                    
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
            using (DCmodRental dc = new DCmodRental())
            {
                var lstCat = dc.dbRntChnlAirbnbLkAmenityCatTBLs.ToList();
                LV_cat.DataSource = lstCat;
                LV_cat.DataBind();

                foreach (ListViewDataItem itemCat in LV_cat.Items)
                {
                    Label lbl_id = itemCat.FindControl("lbl_id") as Label;
                    ListView LV = itemCat.FindControl("LV") as ListView;

                    var list = dc.dbRntChnlAirbnbLkAmenityTBLs.Where(x=>x.pidCategory==lbl_id.Text.objToInt32()).ToList();
                    LV.DataSource = list.OrderBy(x => x.code).ToList();
                    LV.DataBind();
                    foreach (ListViewDataItem item in LV.Items)
                    {
                        Label lbl_code = item.FindControl("lbl_code") as Label;
                        Label lbl_refId = item.FindControl("lbl_refId") as Label;
                        DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;

                        drp_refId.Items.Add(new ListItem("- - -", ""));
                        var extraList = LocalIds.OrderBy(x => x.Value).ToList();
                        foreach (var extra in extraList)
                        {
                            drp_refId.Items.Add(new ListItem(extra.Value, "" + extra.Key));
                        }
                        drp_refId.setSelectedValue(lbl_refId.Text);

                    }
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCmodRental dcChnl = new DCmodRental())
            {
                foreach (ListViewDataItem itemCat in LV_cat.Items)
                {
                    ListView LV = itemCat.FindControl("LV") as ListView;
                    foreach (ListViewDataItem item in LV.Items)
                    {
                        Label lbl_code = item.FindControl("lbl_code") as Label;
                        Label lbl_refId = item.FindControl("lbl_refId") as Label;
                        DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;

                        var featureValuesTbl = dcChnl.dbRntChnlAirbnbLkAmenityTBLs.SingleOrDefault(x => x.code == lbl_code.Text);
                        if (featureValuesTbl != null)
                        {
                            featureValuesTbl.refId = drp_refId.SelectedValue;
                            dcChnl.SaveChanges();
                        }
                    }
                }
            }
            fillData();
        }
    }
}