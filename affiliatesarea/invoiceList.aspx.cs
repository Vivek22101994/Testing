using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.affiliatesarea
{
    public partial class invoiceList : agentBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var tmp = rntProps.AgentTBL.SingleOrDefault(x => x.id == agentAuth.CurrentID);
                if (tmp == null)
                {
                    Response.Redirect("/affiliatesarea/Terms.aspx");
                    Response.End();
                    return;
                }
                DateTime dtStart = tmp.createdDate.HasValue ? tmp.createdDate.Value : DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                dtStart = new DateTime(dtStart.Year, dtStart.Month, 1);
                drp_flt_month.Items.Insert(0, new ListItem("- - -", ""));
                while (dtStart <= new DateTime(dtEnd.Year, dtEnd.Month, 1))
                {
                    drp_flt_month.Items.Insert(0, new ListItem(dtStart.formatCustom("#yy# #MM#", CurrentLang.ID, "--/--"), "" + dtStart.Year + "|" + dtStart.Month));
                    dtStart = dtStart.AddMonths(1);
                }

                chkList_flt_state.Items.Add(new ListItem(CurrentSource.getSysLangValue("rntReservationState_4"), "4"));
                chkList_flt_state.Items.Add(new ListItem(CurrentSource.getSysLangValue("rntReservationState_6"), "6"));
                chkList_flt_state.Items.Add(new ListItem(CurrentSource.getSysLangValue("rntReservationState_3"), "3"));
                chkList_flt_state.setSelectedValues(new List<string>() { "4", "6" });


                Fill_LV();
            }
        }
        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    bool useCode = true;
        //    string orderAsc = useCode ? "&#9650;" : "▲";
        //    string orderDesc = useCode ? "&#9660;" : "▼";
        //    LinkButton lnk;
        //    List<string> orderByList = new List<string>() { "dtStart", "dtEnd", "pr_total" };
        //    string orderByCurrent = HF_LDS_orderBy.Value;
        //    foreach (string orderBy in orderByList)
        //    {
        //        lnk = this.Page.FindControl("lnk_orderBy_" + orderBy) as LinkButton;
        //        if (orderBy == "dtStart")
        //            lnk = lnk_orderBy_dtStart;
        //        if (orderBy == "dtEnd")
        //            lnk = lnk_orderBy_dtEnd;
        //        if (orderBy == "pr_total")
        //            lnk = lnk_orderBy_pr_total;
        //        if (lnk == null) continue;
        //        if (!orderByCurrent.StartsWith(orderBy))
        //            continue;
        //        if (orderByCurrent.EndsWith("desc"))
        //            lnk.CssClass = "desc";
        //        else
        //            lnk.CssClass = "asc";
        //        continue;
        //        lnk.Text = lnk.Text.Replace(orderAsc, "").Replace(orderDesc, "").Trim();
        //        if (orderByCurrent.StartsWith(orderBy))
        //            lnk.Text = lnk.Text + (orderByCurrent.EndsWith("desc") ? " " + orderDesc : " " + orderAsc);
        //    }

        //   // chkList_flt_problemID_clientMode_DataBind();
        //}
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            Fill_LV();
        }
        //protected void setFilters()
        //{
        //    string _filter = "";
        //    string _sep = "";
        //    _filter += _sep + "agentID = " + agentAuth.CurrentID + "";
        //    _sep = " and ";
        //    if (txt_flt_code.Text.Trim() != "")
        //    {
        //        _filter += _sep + "code.Contains(\"" + txt_flt_code.Text.Trim() + "\")";
        //        _sep = " and ";
        //    }
        //    List<int?> stateList = chkList_flt_state.getSelectedValueList().Where(x => x.ToInt32() > 0).Select(x => x.ToInt32() as int?).ToList();
        //    if (stateList.Count != 0)
        //    {
        //        _filter += _sep + "(";
        //        _sep = "";
        //        foreach (int _id in stateList)
        //        {
        //            _filter += _sep + "state_pid = " + _id + "";
        //            _sep = " or ";
        //        }
        //        _filter += ")";
        //        _sep = " and ";
        //    }
        //    if (drp_flt_month.SelectedValue != "" && drp_flt_month.SelectedValue.splitStringToList("|").Count==2)
        //    {
        //        int year = drp_flt_month.SelectedValue.splitStringToList("|")[0].ToInt32();
        //        int month = drp_flt_month.SelectedValue.splitStringToList("|")[1].ToInt32();
        //        DateTime dtStart = new DateTime(year, month, 1);
        //        DateTime dtEnd = month == 12 ? new DateTime(year + 1, 1, 1) : new DateTime(year, month + 1, 1);

        //        _filter += _sep + "dtCreation >= DateTime.Parse(\"" + dtStart + "\") and dtCreation < DateTime.Parse(\"" + dtEnd + "\")";
        //        _sep = " and ";
        //    }
        //    HF_lds_filter.Value = _filter;
        //}
        protected void Fill_LV()
        {
            List<long?> resList = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.agentID == agentAuth.CurrentID).Select(x => (long?)x.id).ToList();
            List<INV_TBL_INVOICE> _list = maga_DataContext.DC_INVOICE.INV_TBL_INVOICE.Where(x => resList.Contains(x.rnt_pid_reservation)).ToList();
            //if (txt_code.Text.Trim() != "")
            //{
            //    _list = _list.Where(x => x.code != null && x.code.ToLower().Contains(txt_code.Text.ToLower().Trim())).ToList();
            //}
            //if (txt_rnt_reservation_code.Text.Trim() != "")
            //{
            //    _list = _list.Where(x => x.rnt_reservation_code != null && x.rnt_reservation_code.ToLower().Contains(txt_rnt_reservation_code.Text.ToLower().Trim())).ToList();
            //}
            //if (HF_dtCreation_from.Value.ToInt32() != 0)
            //{
            //    _list = _list.Where(x => x.dtCreation >= HF_dtCreation_from.Value.JSCal_stringToDate()).ToList();
            //}
            //if (HF_dtCreation_to.Value.ToInt32() != 0)
            //{
            //    _list = _list.Where(x => x.dtCreation <= HF_dtCreation_to.Value.JSCal_stringToDate()).ToList();
            //}

            //if (HF_inv_dtInvoice_from.Value.ToInt32() != 0)
            //{
            //    _list = _list.Where(x => x.inv_dtInvoice >= HF_inv_dtInvoice_from.Value.JSCal_stringToDate()).ToList();
            //}
            //if (HF_inv_dtInvoice_to.Value.ToInt32() != 0)
            //{
            //    _list = _list.Where(x => x.inv_dtInvoice <= HF_inv_dtInvoice_to.Value.JSCal_stringToDate()).ToList();
            //}
            LV.DataSource = _list.OrderByDescending(x => x.inv_dtInvoice).ToList();
            LV.DataBind();
        }

    }
}
