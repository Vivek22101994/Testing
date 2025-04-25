using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class invCashBook : adminBasePage
    {
        protected dbInvCashBookTBL currTBL;
        private List<dbInvCashBookVIEW> tmpCurrList;
        private List<dbInvCashBookVIEW> CurrList
        {
            get
            {
                if (tmpCurrList == null)
                    if (ViewState["CurrList"] != null)
                    {
                        tmpCurrList =
                            PConv.DeserArrToList((object[])ViewState["CurrList"],
                                                 typeof(dbInvCashBookVIEW)).Cast<dbInvCashBookVIEW>().ToList();
                    }
                    else
                        tmpCurrList = new List<dbInvCashBookVIEW>();

                return tmpCurrList;
            }
            set
            {
                ViewState["CurrList"] = PConv.SerialList(value.Cast<object>().ToList());
                tmpCurrList = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                rdp_flt_cashDateFrom.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                chkList_flt_cashPlace_DataBind();
                chkList_flt_cashType_DataBind();
                chkList_flt_docCaseId_DataBind();
                closeDetails(false);
            }
        }
        protected void chkList_flt_cashPlace_DataBind()
        {
            chkList_flt_cashPlace.Items.Clear();
            foreach (dbInvCashPlaceLK CashPlace in invProps.CashPlaceLK.Where(x => x.isActive == 1).OrderBy(x => x.title))
                chkList_flt_cashPlace.Items.Add(new ListItem(CashPlace.title, CashPlace.id + ""));
        }
        protected void chkList_flt_cashType_DataBind()
        {
            chkList_flt_cashType.DataSource = invProps.CashTypeLK.Where(x => x.isActive == 1).OrderBy(x => x.title);
            chkList_flt_cashType.DataTextField = "title";
            chkList_flt_cashType.DataValueField = "id";
            chkList_flt_cashType.DataBind();
        }
        protected void chkList_flt_docCaseId_DataBind()
        {
            chkList_flt_docCaseId.DataSource = invProps.CashDocCaseLK.OrderBy(x => x.code);
            chkList_flt_docCaseId.DataTextField = "code";
            chkList_flt_docCaseId.DataValueField = "id";
            chkList_flt_docCaseId.DataBind();
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodInvoice dc = new DCmodInvoice())
                {
                    currTBL = dc.dbInvCashBookTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                    }
                }
                closeDetails(false);
            }
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            setFilters();
            LV_DataBind();
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        protected void LV_DataBind()
        {
            LV.DataSource = CurrList.OrderBy(x => x.cashDate).ToList();
            LV.DataBind();
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            LV_DataBind();
        }
        protected void setFilters()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                List<dbInvCashBookVIEW> list = dc.dbInvCashBookVIEWs.ToList();
                if (drp_flt_cashInOut.SelectedValue != "-1")
                {
                    list = list.Where(x => x.cashInOut == drp_flt_cashInOut.getSelectedValueInt()).ToList();
                }
                if (rdp_flt_cashDateFrom.SelectedDate.HasValue)
                {
                    list = list.Where(x => x.cashDate >= rdp_flt_cashDateFrom.SelectedDate).ToList();
                }
                if (rdp_flt_cashDateTo.SelectedDate.HasValue)
                {
                    list = list.Where(x => x.cashDate < rdp_flt_cashDateTo.SelectedDate).ToList();
                }
                List<int?> cashPlaceList = chkList_flt_cashPlace.getSelectedValueList().Where(x => x.ToInt32() > 0).Select(x => x.ToInt32() as int?).ToList();
                if (cashPlaceList.Count != 0)
                    list = list.Where(x => cashPlaceList.Contains(x.cashPlace)).ToList();
                List<int?> cashTypeList = chkList_flt_cashType.getSelectedValueList().Where(x => x.ToInt32() > 0).Select(x => x.ToInt32() as int?).ToList();
                if (cashTypeList.Count != 0)
                    list = list.Where(x => cashTypeList.Contains(x.cashType)).ToList();
                List<int?> docCaseCodeIdList = chkList_flt_docCaseId.getSelectedValueList().Where(x => x.ToInt32() > 0).Select(x => x.ToInt32() as int?).ToList();
                if (docCaseCodeIdList.Count != 0)
                    list = list.Where(x => docCaseCodeIdList.Contains(x.docCaseId)).ToList();
                CurrList = list;
                setStats();
            }
        }
        protected void setStats()
        {
            int count_1 = CurrList.Where(x => x.cashInOut == 1).Count();
            decimal cashAmount_1 = CurrList.Where(x => x.cashInOut == 1).Sum(x => x.cashAmount.objToDecimal());
            txt_count_1.Text = count_1.ToString();
            txt_cashAmount_1.Text = cashAmount_1.ToString("N2");

            int count_0 = CurrList.Where(x => x.cashInOut == 0).Count();
            decimal cashAmount_0 = CurrList.Where(x => x.cashInOut == 0).Sum(x => x.cashAmount.objToDecimal());
            txt_count_0.Text = count_0.ToString();
            txt_cashAmount_0.Text = cashAmount_0.ToString("N2");

            txt_count_diff.Text = (count_1 + count_0).ToString();
            txt_cashAmount_diff.Text = (cashAmount_1 - cashAmount_0).ToString("N2");
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            closeDetails(false);
        }
        protected void drp_flt_cashInOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            //drp_docCase_DataBind();
        }
        protected void drp_flt_ownerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //drp_owner_DataBind();
        }
    }

}