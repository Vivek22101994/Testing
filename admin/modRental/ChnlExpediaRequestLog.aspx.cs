using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class ChnlExpediaRequestLog : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rdtp_DateFrom.SelectedDate = DateTime.Now.Date;
                setfilters();
                Fill_LV();
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            Fill_LV();
        }
        protected void setfilters()
        {
            string _filter = "";
            string _sep = "";
            if (txt_requestType.Text.Trim() != "")
            {
                _filter += _sep + "requestType.Contains(\"" + txt_requestType.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_requestComments.Text.Trim() != "")
            {
                _filter += _sep + "requestComments.Contains(\"" + txt_requestComments.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (rdtp_DateFrom.SelectedDate.HasValue)
            {
                _filter += _sep + "logDateTime >= DateTime.Parse(\"" + rdtp_DateFrom.SelectedDate + "\")";
                _sep = " and ";
            }
            if (rdtp_DateTo.SelectedDate.HasValue)
            {
                _filter += _sep + "logDateTime < DateTime.Parse(\"" + rdtp_DateTo.SelectedDate + "\")";
                _sep = " and ";
            }
            _filter += _sep + "1 = 1";
            ltrLDSfiltter.Text = _filter;
        }
        protected void Fill_LV()
        {
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            Label lbl_id = LV.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            if (lbl_id != null)
            {
            }
            Fill_LV();
            LV.SelectedIndex = e.NewSelectedIndex;
        }
        protected void drp_logList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }
        protected void lnk_flt_Click(object sender, EventArgs e)
        {
            setfilters();
            Fill_LV();
        }

    }
}
