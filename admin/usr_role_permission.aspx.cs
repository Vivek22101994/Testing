using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_role_permission : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected magaUser_DataContext DC_USER;

        protected USR_TBL_ROLE currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                if (Request.QueryString["new"] == "true")
                {
                    HF_id.Value = "0";
                    FillControls();
                }
            }
        }

        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            var lbl_id = (Label)LV.Items[e.NewSelectedIndex].FindControl("lbl_id");
            HF_id.Value = lbl_id.Text;
            FillControls();
        }

        private List<USR_RL_ROLE_PERMISSION> _currPermissions;
        private void FillControls()
        {
            currTBL = DC_USER.USR_TBL_ROLE.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (currTBL == null)
            {
                currTBL = new USR_TBL_ROLE();
                txt_title.ReadOnly = false;
            }
            else
                txt_title.ReadOnly = true;
            txt_title.Text = currTBL.title;
            drp_rnt_onlyOwnedRequests.setSelectedValue(currTBL.rnt_onlyOwnedRequests);
            drp_rnt_onlyOwnedReservations.setSelectedValue(currTBL.rnt_onlyOwnedReservations);
            drp_rnt_onlyOwnedAgents.setSelectedValue(currTBL.rnt_onlyOwnedAgents);
            drp_rnt_onlyOwnUserDetails.setSelectedValue(currTBL.rnt_onlyOwnUserDetails);
            drp_rnt_onlyOwnedPlannerCheckinCheckout.setSelectedValue(currTBL.rnt_onlyOwnedPlannerCheckinCheckout);
            _currPermissions = DC_USER.USR_RL_ROLE_PERMISSIONs.Where(x => x.pid_role == currTBL.id).ToList();
            LV_permission.DataSource = UserAuthentication.PERMISSION_LIST;
            LV_permission.DataBind();
            LV_admins.DataSource = DC_USER.USR_ADMIN.Where(x => x.pid_role == currTBL.id && x.id > 2);
            LV_admins.DataBind();
            
            pnlContent.Visible = true;
        }
        private void FillDataFromControls()
        {
            currTBL = DC_USER.USR_TBL_ROLE.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (currTBL == null)
            {
                currTBL = new USR_TBL_ROLE();
                currTBL.title = txt_title.Text;
                DC_USER.USR_TBL_ROLE.InsertOnSubmit(currTBL);
                DC_USER.SubmitChanges();
                HF_id.Value = currTBL.id + "";
            }
            currTBL.rnt_onlyOwnedRequests = drp_rnt_onlyOwnedRequests.getSelectedValueInt();
            currTBL.rnt_onlyOwnedReservations = drp_rnt_onlyOwnedReservations.getSelectedValueInt();
            currTBL.rnt_onlyOwnedAgents = drp_rnt_onlyOwnedAgents.getSelectedValueInt();
            currTBL.rnt_onlyOwnUserDetails = drp_rnt_onlyOwnUserDetails.getSelectedValueInt();
            currTBL.rnt_onlyOwnedPlannerCheckinCheckout = drp_rnt_onlyOwnedPlannerCheckinCheckout.getSelectedValueInt();
            foreach (ListViewDataItem Item in LV_permission.Items)
            {
                Label lbl_id = Item.FindControl("lbl_id") as Label;
                DropDownList drp_has = Item.FindControl("drp_has") as DropDownList;
                CheckBox chk_can_read = Item.FindControl("chk_can_read") as CheckBox;
                CheckBox chk_can_edit = Item.FindControl("chk_can_edit") as CheckBox;
                CheckBox chk_can_create = Item.FindControl("chk_can_create") as CheckBox;
                CheckBox chk_can_delete = Item.FindControl("chk_can_delete") as CheckBox;
                CheckBox chk_only_owned = Item.FindControl("chk_only_owned") as CheckBox;
                if (lbl_id == null
                    || drp_has == null
                    || chk_can_read == null
                    || chk_can_edit == null
                    || chk_can_create == null
                    || chk_can_delete == null
                    || chk_only_owned == null
                    )
                {
                    continue;
                }
                USR_RL_ROLE_PERMISSION _permission = DC_USER.USR_RL_ROLE_PERMISSIONs.FirstOrDefault(x =>x.pid_role == currTBL.id&& x.permission == lbl_id.Text);
                if (drp_has.SelectedValue=="0")
                {
                    if (_permission != null)
                    {
                        DC_USER.USR_RL_ROLE_PERMISSIONs.DeleteOnSubmit(_permission);
                    }
                    continue;
                }
                if (_permission == null)
                {
                    _permission = new USR_RL_ROLE_PERMISSION();
                    _permission.pid_role = currTBL.id;
                    _permission.permission = lbl_id.Text;
                    DC_USER.USR_RL_ROLE_PERMISSIONs.InsertOnSubmit(_permission);
                }
                _permission.can_read = chk_can_read.Checked ? 1 : 0;
                _permission.can_edit = chk_can_edit.Checked ? 1 : 0;
                _permission.can_create = chk_can_create.Checked ? 1 : 0;
                _permission.can_delete = chk_can_delete.Checked ? 1 : 0;
                _permission.only_owned = chk_only_owned.Checked ? 1 : 0;
            }
            DC_USER.SubmitChanges();
        }
 
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }
        protected void LV_permission_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            DropDownList drp_has = e.Item.FindControl("drp_has") as DropDownList;
            CheckBox chk_can_read = e.Item.FindControl("chk_can_read") as CheckBox;
            CheckBox chk_can_edit = e.Item.FindControl("chk_can_edit") as CheckBox;
            CheckBox chk_can_create = e.Item.FindControl("chk_can_create") as CheckBox;
            CheckBox chk_can_delete = e.Item.FindControl("chk_can_delete") as CheckBox;
            CheckBox chk_only_owned = e.Item.FindControl("chk_only_owned") as CheckBox;
            if(lbl_id==null
                ||drp_has==null
                ||chk_can_read==null
                ||chk_can_edit==null
                ||chk_can_create==null
                ||chk_can_delete==null
                ||chk_only_owned==null
                )
            {
                return;
            }
            USR_RL_ROLE_PERMISSION _permission = _currPermissions.FirstOrDefault(x => x.permission == lbl_id.Text);
            if(_permission!=null)
            {
                drp_has.setSelectedValue("1");
                chk_can_read.Checked = _permission.can_read == 1;
                chk_can_edit.Checked = _permission.can_edit == 1;
                chk_can_create.Checked = _permission.can_create == 1;
                chk_can_delete.Checked = _permission.can_delete == 1;
                chk_only_owned.Checked = _permission.only_owned == 1;
                return;
            }
            drp_has.setSelectedValue("0");
        }

        protected void LV_permission_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            enableChks();
        }
        protected void LV_permission_DataBound(object sender, EventArgs e)
        {
            enableChks();
        }
        protected void enableChks()
        {
            foreach (ListViewDataItem Item in LV_permission.Items)
            {
                Label lbl_id = Item.FindControl("lbl_id") as Label;
                DropDownList drp_has = Item.FindControl("drp_has") as DropDownList;
                CheckBox chk_can_read = Item.FindControl("chk_can_read") as CheckBox;
                CheckBox chk_can_edit = Item.FindControl("chk_can_edit") as CheckBox;
                CheckBox chk_can_create = Item.FindControl("chk_can_create") as CheckBox;
                CheckBox chk_can_delete = Item.FindControl("chk_can_delete") as CheckBox;
                CheckBox chk_only_owned = Item.FindControl("chk_only_owned") as CheckBox;
                if (lbl_id == null
                    || drp_has == null
                    || chk_can_read == null
                    || chk_can_edit == null
                    || chk_can_create == null
                    || chk_can_delete == null
                    || chk_only_owned == null
                    )
                {
                    continue;
                }
                chk_can_read.Enabled = chk_can_edit.Enabled = chk_can_create.Enabled = chk_can_delete.Enabled = chk_only_owned.Enabled = drp_has.SelectedValue == "1";
            }
        }

        protected void drp_has_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableChks();
        }

    }
}
