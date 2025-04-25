using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace RentalInRome.admin
{
    public partial class loc_country_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "loc_city";
        }
        private magaLocation_DataContext DC_LOCATION;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_LOCATION = maga_DataContext.DC_LOCATION;
            if (!IsPostBack)
            {
                fillData();
            }
        }
        protected void fillData()
        {
            LV.DataSource = DC_LOCATION.LOC_LK_COUNTRies.OrderBy(x => x.title).ToList();
            LV.DataBind();
            foreach (ListViewDataItem item in LV.Items)
            {
                Label lbl_id = item.FindControl("lbl_id") as Label;
                Label lbl_codeCoGe = item.FindControl("lbl_codeCoGe") as Label;
                Label lbl_country_code = item.FindControl("lbl_country_code") as Label;
                RadNumericTextBox ntxt_codeCoGe = item.FindControl("ntxt_codeCoGe") as RadNumericTextBox;
                RadNumericTextBox ntxt_country_code = item.FindControl("ntxt_country_code") as RadNumericTextBox;
                ntxt_codeCoGe.Value = lbl_codeCoGe.Text.ToInt32();
                ntxt_country_code.Value = lbl_country_code.Text.objToInt32();
            }
        }
        protected void lnk_nuovo_Click(object sender, EventArgs e)
        {
            foreach (ListViewDataItem item in LV.Items)
            {
                Label lbl_id = item.FindControl("lbl_id") as Label;
                Label lbl_codeCoGe = item.FindControl("lbl_codeCoGe") as Label;
                Label lbl_country_code = item.FindControl("lbl_country_code") as Label;
                RadNumericTextBox ntxt_codeCoGe = item.FindControl("ntxt_codeCoGe") as RadNumericTextBox;
                RadNumericTextBox ntxt_country_code = item.FindControl("ntxt_country_code") as RadNumericTextBox;
                if (ntxt_codeCoGe.Value.objToInt32() == 0) continue;
                var currTbl = DC_LOCATION.LOC_LK_COUNTRies.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (currTbl == null) continue;
                currTbl.codeCoGe = ntxt_codeCoGe.Value.objToInt32();
                currTbl.country_code = ntxt_country_code.Value.objToInt32();
                DC_LOCATION.SubmitChanges();
            }
            fillData();
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            fillData();
        }
    }
}