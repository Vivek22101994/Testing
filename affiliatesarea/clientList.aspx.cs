using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using Telerik.Web.UI;

namespace RentalInRome.affiliatesarea
{
    public partial class clientList : agentBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadContent();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            bool useCode = true;
            string orderAsc = useCode ? "&#9650;" : "▲";
            string orderDesc = useCode ? "&#9660;" : "▼";
            LinkButton lnk;
            List<string> orderByList = new List<string>() { "state_date", "dtCreation", "dtStart", "dtEnd", "pr_total", "cl_name_full" };
            string orderByCurrent = ltrLDSorderBy.Text;
            foreach (string orderBy in orderByList)
            {
                lnk = LV.FindControl("lnk_orderBy_" + orderBy) as LinkButton;
                if (lnk == null) continue;
                lnk.Text = lnk.Text.Replace(orderAsc, "").Replace(orderDesc, "").Trim();
                if (orderByCurrent.StartsWith(orderBy))
                    lnk.Text = lnk.Text + (orderByCurrent.EndsWith("desc") ? " " + orderDesc : " " + orderAsc);
            }

            // chkList_flt_problemID_clientMode_DataBind();
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            LoadContent();
        }
        protected void LoadContent()
        {
            string _filter = "";
            string _sep = "";
            _filter += _sep + "pidAgent = " + agentAuth.CurrentID + "";
            _sep = " and ";
            _filter += _sep + "isActive = 1";
            _sep = " and ";

            //if (txt_flt_code.Text.Trim() != "")
            //{
            //    _filter += _sep + "code.Contains(\"" + txt_flt_code.Text.Trim() + "\")";
            //    _sep = " and ";
            //}
            //if (drp_flt_isActive.SelectedValue != "-1")
            //{
            //    _filter += _sep + "isActive = " + drp_flt_isActive.SelectedValue + "";
            //    _sep = " and ";
            //}
            //if (rdp_flt_createdDateFrom.SelectedDate.HasValue)
            //{
            //    _filter += _sep + "createdDate >= DateTime.Parse(\"" + rdp_flt_createdDateFrom.SelectedDate + "\")";
            //    _sep = " and ";
            //}
            //if (rdp_flt_createdDateTo.SelectedDate.HasValue)
            //{
            //    _filter += _sep + "createdDate < DateTime.Parse(\"" + rdp_flt_createdDateTo.SelectedDate + "\")";
            //    _sep = " and ";
            //}
            //if (drp_flt_typeCode.SelectedValue != "")
            //{
            //    _filter += _sep + "typeCode.Contains(\"" + drp_flt_typeCode.SelectedValue + "\")";
            //    _sep = " and ";
            //}
            //if (txt_flt_nameFull.Text.Trim() != "")
            //{
            //    _filter += _sep + "nameFull.Contains(\"" + txt_flt_nameFull.Text.Trim() + "\")";
            //    _sep = " and ";
            //}
            if (_filter == "") _filter = "1=1";
            ltrLDSfiltter.Text = _filter;
            Fill_LV();

        }
        protected void Fill_LV()
        {
            LDS.Where = ltrLDSfiltter.Text;
            LDS.OrderBy = ltrLDSorderBy.Text;
            LDS.DataBind();
            LV.DataBind();
        }
        protected void LV_OrderBy(string orderBy)
        {
            string orderByCurrent = ltrLDSorderBy.Text;
            if (orderByCurrent.StartsWith(orderBy))
                ltrLDSorderBy.Text = orderBy + (orderByCurrent.EndsWith("desc") ? " asc" : " desc");
            else
                ltrLDSorderBy.Text = orderBy + " desc";
            Fill_LV();
        }

        protected void LV_DataBound(object sender, EventArgs e)
        {
            DataPager _dataPager = LV.FindControl("DataPager1") as DataPager;
            Label _lblCount = LV.FindControl("lbl_record_count") as Label;
            Label _lblCount_top = LV.FindControl("lbl_record_count_top") as Label;
            if (_dataPager != null && _lblCount != null && _lblCount_top != null)
                _lblCount.Text = _lblCount_top.Text = "Totale: " + _dataPager.TotalRowCount;
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "orderBy")
            {
                LV_OrderBy(e.CommandArgument.ToString());
                return;
            }
        }
    }
}
