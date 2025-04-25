using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_estate_list : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;
        public string IdEstate
        {
            get
            {
                return HF_id.Value;
            }
            set
            {
                HF_id.Value = value;
                HF_lds_filter.Value = "id=" + HF_id.Value;
                LDS.Where = HF_lds_filter.Value;
                LDS.DataBind();
                LV.DataBind();
            }
        }
        public string FILTER
        {
            get
            {
                return HF_lds_filter.Value;
            }
            set
            {
                HF_lds_filter.Value = value;
                LDS.Where = HF_lds_filter.Value;
                LDS.DataBind();
                LV.DataBind();
            }
        }
        private string CURRENT_FILTER
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_ESTATE_LIST_PAGE"] != null)
                {
                    return (string)HttpContext.Current.Session["CURRENT_ESTATE_LIST_PAGE"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["CURRENT_ESTATE_LIST_PAGE"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            LDS.Where = HF_lds_filter.Value;
            LDS.OrderBy = HF_sort.Value;
            if (!IsPostBack)
            {
                if (Page.GetType().Name == "admin_rnt_estate_list_aspx")
                {
                    if (Request.QueryString.ToString() != "")
                    {
                        CURRENT_FILTER = Request.QueryString.ToString();
                    }
                    else if (CURRENT_FILTER != "")
                    {
                        Response.Redirect("rnt_estate_list.aspx?" + CURRENT_FILTER);
                    }
                }
                if (HF_id.Value == "")
                    LDS.Where = "1=2";
            }
        }
        protected void LV_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            LV.SelectedIndex = -1;
        }

        private void SetLnkCaptions()
        {
            var lnk_code = LV.FindControl("lnk_code") as LinkButton;
            lnk_code.Text = "Nome";
        }

        protected void lnk_code_Click(object sender, EventArgs e)
        {
            var lnk_name = sender as LinkButton;
            if (lnk_name == null)
                return;
            SetLnkCaptions();
            if (HF_sort.Value == "code")
            {
                LDS.OrderBy = "code desc";
                HF_sort.Value = LDS.OrderBy;
                lnk_name.Text = "Nome &#9650;";
            }
            else
            {
                LDS.OrderBy = "code";
                HF_sort.Value = LDS.OrderBy;
                lnk_name.Text = "Nome &#9660;";
            }
            LV.DataBind();
        }

        protected void lnk_active_Click(object sender, EventArgs e)
        {
            var lnk_name = sender as LinkButton;
            if (lnk_name == null)
                return;
            SetLnkCaptions();
            if (HF_sort.Value == "is_active")
            {
                LDS.OrderBy = "is_active desc";
                HF_sort.Value = LDS.OrderBy;
                lnk_name.Text = "Attivo/Non attivo &#9650;";
            }
            else
            {
                LDS.OrderBy = "is_active";
                HF_sort.Value = LDS.OrderBy;
                lnk_name.Text = "Attivo/Non attivo &#9660;";
            }
            LV.DataBind();
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

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "setBP")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == lbl_id.Text.objToInt32());
                if (_currTBL != null)
                {
                    if (_currTBL.is_best_price.objToInt32() == 0)
                        _currTBL.is_best_price = 1;
                    else
                        _currTBL.is_best_price = 0;

                    DC_RENTAL.SubmitChanges();
                    LV.DataBind();
                    AppSettings._refreshCache_RNT_ESTATEs();
                    AppSettings.RELOAD_SESSION();
                }

            }
        }
    }
}
