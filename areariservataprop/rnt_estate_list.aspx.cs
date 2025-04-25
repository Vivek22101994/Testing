using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.areariservataprop
{
    public partial class rnt_estate_list : ownerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LDS.Where = "pid_owner = " + OwnerAuthentication.CurrentID + " AND is_active==1";
                LDS.DataBind();
                LV.DataBind();
            }
        }
        protected void LV_DataBound(object sender, EventArgs e)
        {
            DataPager _dataPager = LV.FindControl("DataPager1") as DataPager;
            Label _lblCount = LV.FindControl("lbl_record_count") as Label;
            Label _lblCount_top = LV.FindControl("lbl_record_count_top") as Label;
            if (_dataPager == null) return;
            if (_lblCount_top != null)
                _lblCount_top.Text = "Totale: " + _dataPager.TotalRowCount;
            if (_lblCount != null)
                _lblCount.Text = "Totale: " + _dataPager.TotalRowCount;
        }
    }
}
