using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModAuth.admin.modAuth
{
    public partial class userReportList : adminBasePage
    {
        protected dbAuthUserReportTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                pnl_flt_pidReferer.Visible = (UserAuthentication.CURRENT_USER_ROLE == 1);
                drp_flt_pidReferer_DataBind();
                drp_flt_repType_DataBind();
                closeDetails(false);
            }
        }
        protected void drp_flt_repType_DataBind()
        {
            drp_flt_repType.Items.Clear();
            drp_flt_repType.DataSource = authProps.UserReportType;
            drp_flt_repType.DataTextField = "title";
            drp_flt_repType.DataValueField = "code";
            drp_flt_repType.DataBind();
            drp_flt_repType.Items.Insert(0, new ListItem("- tutti -", ""));
        }
        protected void drp_flt_pidReferer_DataBind()
        {
            drp_flt_pidUser.Items.Clear();
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.hasAuthUserReport == 1).OrderBy(x => x.name).ToList();
            foreach (USR_ADMIN _admin in _list)
            {
                drp_flt_pidUser.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
            drp_flt_pidUser.Items.Insert(0, new ListItem("- tutti -", ""));
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
                using (DCmodAuth dc = new DCmodAuth())
                {
                    currTBL = dc.dbAuthUserReportTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
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
            setfilters();
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        protected void setfilters()
        {
            string _filter = "";
            string _sep = "";
            if (UserAuthentication.CURRENT_USER_ROLE == 1)
            {
                if (drp_flt_pidUser.SelectedValue != "")
                {
                    _filter += _sep + "pidUser = " + drp_flt_pidUser.SelectedValue + "";
                    _sep = " and ";
                }
            }
            else
            {
                _filter += _sep + "pidUser = " + UserAuthentication.CurrentUserID + "";
                _sep = " and ";
            }
            if (drp_flt_repType.SelectedValue != "")
            {
                _filter += _sep + "repType = \"" + drp_flt_repType.SelectedValue + "\"";
                _sep = " and ";
            }
            if (rdp_flt_repDateTimeFrom.SelectedDate.HasValue)
            {
                _filter += _sep + "repDateTime >= DateTime.Parse(\"" + rdp_flt_repDateTimeFrom.SelectedDate + "\")";
                _sep = " and ";
            }
            if (rdp_flt_repDateTimeTo.SelectedDate.HasValue)
            {
                _filter += _sep + "repDateTime < DateTime.Parse(\"" + rdp_flt_repDateTimeTo.SelectedDate + "\")";
                _sep = " and ";
            }
            _filter += _sep + "1 = 1";
            ltrLDSfiltter.Text = _filter;
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