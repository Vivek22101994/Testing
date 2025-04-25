using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;
using ModRental;

namespace RentalInRome.admin.modRental
{
    public partial class EstateExtraOwnerList : adminBasePage
    {
        protected dbRntExtraOwnerTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                pnl_flt_pidReferer.Visible = UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0;
                pnl_flt_pidReferer.Visible = UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0;
                drp_flt_pidReferer_DataBind();
                closeDetails(false);
            }
        }
        protected void drp_flt_pidReferer_DataBind()
        {
            drp_flt_pidReferer.Items.Clear();
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.rnt_canHaveAgent == 1).OrderBy(x => x.name).ToList();
            foreach (USR_ADMIN _admin in _list)
            {
                drp_flt_pidReferer.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
            drp_flt_pidReferer.Items.Insert(0, new ListItem("- tutti -", ""));
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
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntExtraOwnerTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                    }
                }
                closeDetails(false);
            }
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            LV_DataBind();
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            setfilters();
            LV_DataBind();
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        protected void LV_DataBind()
        {
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        protected void setfilters()
        {
            string _filter = "";
            string _sep = "";
            if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0)
            {
                if (drp_flt_pidReferer.SelectedValue != "")
                {
                    _filter += _sep + "pidReferer = " + drp_flt_pidReferer.SelectedValue + "";
                    _sep = " and ";
                }
            }
            else
            {
                _filter += _sep + "pidReferer = " + UserAuthentication.CurrentUserID + "";
                _sep = " and ";
            }
            if (txt_flt_code.Text.Trim() != "")
            {
                _filter += _sep + "code.Contains(\"" + txt_flt_code.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (drp_flt_isActive.SelectedValue != "-1")
            {
                _filter += _sep + "isActive = " + drp_flt_isActive.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_flt_hasAcceptedContract.SelectedValue != "-1")
            {
                _filter += _sep + "hasAcceptedContract = " + drp_flt_hasAcceptedContract.SelectedValue + "";
                _sep = " and ";
            }
            if (rdp_flt_createdDateFrom.SelectedDate.HasValue)
            {
                _filter += _sep + "createdDate >= DateTime.Parse(\"" + rdp_flt_createdDateFrom.SelectedDate + "\")";
                _sep = " and ";
            }
            if (rdp_flt_createdDateTo.SelectedDate.HasValue)
            {
                _filter += _sep + "createdDate < DateTime.Parse(\"" + rdp_flt_createdDateTo.SelectedDate + "\")";
                _sep = " and ";
            }
            if (txt_flt_nameCompany.Text.Trim() != "")
            {
                _filter += _sep + "nameCompany.Contains(\"" + txt_flt_nameCompany.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_flt_nameFull.Text.Trim() != "")
            {
                _filter += _sep + "nameFull.Contains(\"" + txt_flt_nameFull.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_flt_contactEmail.Text.Trim() != "")
            {
                _filter += _sep + "contactEmail.Contains(\"" + txt_flt_contactEmail.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (drp_flt_hasPren.SelectedValue != "-1")
                using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                {
                    int reportYear = DateTime.Now.Year;
                    int reportMonth = DateTime.Now.Month;
                    DateTime dtStart = new DateTime(reportYear, reportMonth, 1);
                    DateTime dtEnd = reportMonth == 12 ? new DateTime(reportYear + 1, 1, 1) : new DateTime(reportYear, reportMonth + 1, 1);
                    var tmpList = dcOld.RNT_TBL_RESERVATION.Where(x => x.state_pid == 4 && x.agentID.HasValue && x.agentID.Value != 0 && x.dtCreation >= dtStart && x.dtCreation < dtEnd).Select(x => x.agentID.Value).Distinct();

                    if (drp_flt_hasPren.SelectedValue != "")
                    {
                        // todo: altri mesi da fare
                    }
                    if (tmpList.Count() != 0)
                    {
                        _filter += _sep + "(";
                        _sep = "";
                        foreach (long _id in tmpList)
                        {
                            _filter += _sep + "id = " + _id + "";
                            _sep = " or ";
                        }
                        _filter += ")";
                        _sep = " and ";
                    }
                }
            _filter += _sep + "1 = 1";
            ltrLDSfiltter.Text = _filter;
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            closeDetails(false);
        }
       
    }
}