using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using Telerik.Web.UI;
using MagaRentalCE.data;


namespace MagaRentalCE.admin.modRental
{
    public partial class chnlAirbnbPropertyTypeList : adminBasePage
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
            if (LocalIds == null)
            {
                LocalIds = new Dictionary<int, string>();
                var LocalIdsList = rntProps.EstateCategoryTB.Where(x => x.isActive == 1).ToList();
                foreach (var tmp in LocalIdsList)
                {
                    var tmpLn = rntProps.EstateCategoryVIEW.SingleOrDefault(x => x.id == tmp.id && x.pidLang == App.LangID);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = rntProps.EstateCategoryVIEW.SingleOrDefault(x => x.id == tmp.id && x.pidLang == App.DefLangID);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = rntProps.EstateCategoryVIEW.SingleOrDefault(x => x.id == tmp.id && x.pidLang == "en");
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = rntProps.EstateCategoryVIEW.SingleOrDefault(x => x.id == tmp.id && x.pidLang == "it");
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title))
                        continue;
                    LocalIds.Add(tmp.id, tmpLn.title);
                }
            }
            using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
            using (DCmodRental dc = new DCmodRental())
            {
                var list = dc.dbRntChnlAirbnbLkPropertyTypeTBLs.ToList();
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
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCmodRental dcChnl = new DCmodRental())
            {
                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_code = item.FindControl("lbl_code") as Label;
                    Label lbl_refId = item.FindControl("lbl_refId") as Label;
                    DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;

                    var featureValuesTbl = dcChnl.dbRntChnlAirbnbLkPropertyTypeTBLs.SingleOrDefault(x => x.code == lbl_code.Text);
                    if (featureValuesTbl != null)
                    {
                        featureValuesTbl.refId = drp_refId.SelectedValue;
                        dcChnl.SaveChanges();
                    }
                }
            }
            fillData();
        }
    }
}