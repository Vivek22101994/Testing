using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class inv_payment_list : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;  
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "inv_payment";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                HF_dtCreation_from.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).JSCal_dateToString();
                LoadContent();
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setCal", "setCal();", true);
        }
        protected void lnkFilter_Click(object sender, EventArgs e)
        {
            LoadContent();
        }
        protected void LoadContent()
        {
            string _filter = "";
            string _sep = "";

            if (txt_code.Text.Trim() != "")
            {
                _filter += _sep + "code.Contains(\"" + txt_code.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_rnt_reservation_code.Text.Trim() != "")
            {
                _filter += _sep + "rnt_reservation_code.Contains(\"" + txt_code.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (HF_dtCreation_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtCreation >= DateTime.Parse(\"" + HF_dtCreation_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_dtCreation_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtCreation <= DateTime.Parse(\"" + HF_dtCreation_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }

            if (HF_pay_date_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "pay_date >= DateTime.Parse(\"" + HF_pay_date_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_pay_date_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "pay_date <= DateTime.Parse(\"" + HF_pay_date_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (_filter == "") _filter = "1=1";
            HF_lds_filter.Value = _filter;
            Fill_LV();

        }
        protected void Fill_LV()
        {
            LDS.Where = HF_lds_filter.Value;
            LDS.DataBind();
            LV.DataBind();
        }

        protected void LV_DataBound(object sender, EventArgs e)
        {
            DataPager _dataPager = LV.FindControl("DataPager1") as DataPager;
            Label _lblCount = LV.FindControl("lbl_record_count") as Label;
            Label _lblCount_top = LV.FindControl("lbl_record_count_top") as Label;
            if (_dataPager != null && _lblCount != null && _lblCount_top != null)
                _lblCount.Text = _lblCount_top.Text = "Totale: " + _dataPager.TotalRowCount;
        }
    }
}