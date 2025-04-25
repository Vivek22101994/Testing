using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModAuth.admin.modAuth
{
    public partial class roleList : adminBasePage
    {
        protected dbAuthRoleTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "dett")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                HfId.Value = lbl_id.Text;
                fillData();
                pnlDett.Visible = true;
            }
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodAuth dc = new DCmodAuth())
                {
                    currTBL = dc.dbAuthRoleTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        if (currTBL.id == 1) return;
                        dc.Delete(currTBL);
                        List<dbAuthRolePermissionTBL> currList = dc.dbAuthRolePermissionTBLs.Where(x => x.pidRole == currTBL.id).ToList();
                        foreach (dbAuthRolePermissionTBL rolePermission in currList)
                        {
                            dc.Delete(rolePermission);
                        }
                        dc.SaveChanges();
                        authProps.RoleTBL = dc.dbAuthRoleTBLs.ToList();
                        authProps.RolePermissionTBL = dc.dbAuthRolePermissionTBLs.ToList();
                    }
                }
                closeDetails(false);
            }
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            rwdDett.VisibleOnPageLoad = false;
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        private void fillData()
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                currTBL = dc.dbAuthRoleTBLs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null) currTBL = new dbAuthRoleTBL();

                txt_title.Text = currTBL.title;

                LV_permission.DataSource = authProps.PermissionLK;
                LV_permission.DataBind();
                foreach (ListViewDataItem Item in LV_permission.Items)
                {
                    Label lbl_id = Item.FindControl("lbl_id") as Label;
                    DropDownList drp_has = Item.FindControl("drp_has") as DropDownList;
                    CheckBox chk_can_edit = Item.FindControl("chk_can_edit") as CheckBox;
                    CheckBox chk_can_create = Item.FindControl("chk_can_create") as CheckBox;
                    CheckBox chk_can_delete = Item.FindControl("chk_can_delete") as CheckBox;
                    if (lbl_id == null
                        || drp_has == null
                        || chk_can_edit == null
                        || chk_can_create == null
                        || chk_can_delete == null
                        )
                    {
                        continue;
                    }
                    dbAuthRolePermissionTBL rolePermission = dc.dbAuthRolePermissionTBLs.SingleOrDefault(x => x.pidRole == currTBL.id && x.permission == lbl_id.Text);
                    if (currTBL.id == 1)
                    {
                        //drp_has.setSelectedValue(1);
                        chk_can_edit.Checked = true;
                        chk_can_create.Checked = true;
                        chk_can_delete.Checked = true;
                    }
                    else if (rolePermission != null)
                    {
                        //drp_has.setSelectedValue(1);
                        chk_can_edit.Checked = rolePermission.canEdit == 1;
                        chk_can_create.Checked = rolePermission.canCreate == 1;
                        chk_can_delete.Checked = rolePermission.canDelete == 1;
                    }
                    else
                    {
                        //drp_has.setSelectedValue(0);
                        chk_can_edit.Checked = false;
                        chk_can_create.Checked = false;
                        chk_can_delete.Checked = false;
                    }
                }

                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                currTBL = dc.dbAuthRoleTBLs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbAuthRoleTBL();
                    dc.Add(currTBL);
                }
                currTBL.title = txt_title.Text;
                dc.SaveChanges();

                List<dbAuthRolePermissionTBL> currList = dc.dbAuthRolePermissionTBLs.Where(x => x.pidRole == currTBL.id).ToList();
                foreach (dbAuthRolePermissionTBL rolePermission in currList)
                {
                    dc.Delete(rolePermission);
                }
                dc.SaveChanges();
                foreach (ListViewDataItem Item in LV_permission.Items)
                {
                    Label lbl_id = Item.FindControl("lbl_id") as Label;
                    DropDownList drp_has = Item.FindControl("drp_has") as DropDownList;
                    CheckBox chk_can_edit = Item.FindControl("chk_can_edit") as CheckBox;
                    CheckBox chk_can_create = Item.FindControl("chk_can_create") as CheckBox;
                    CheckBox chk_can_delete = Item.FindControl("chk_can_delete") as CheckBox;
                    if (lbl_id == null
                        || drp_has == null
                        || chk_can_edit == null
                        || chk_can_create == null
                        || chk_can_delete == null
                        )
                    {
                        continue;
                    }
                    if (drp_has.SelectedValue == "1" || currTBL.id == 1)
                    {
                        dbAuthRolePermissionTBL rolePermission = new dbAuthRolePermissionTBL();
                        rolePermission.pidRole = currTBL.id;
                        rolePermission.permission = lbl_id.Text;
                        rolePermission.canCreate = chk_can_create.Checked || currTBL.id == 1 ? 1 : 0;
                        rolePermission.canEdit = chk_can_edit.Checked || currTBL.id == 1 ? 1 : 0;
                        rolePermission.canDelete = chk_can_delete.Checked || currTBL.id == 1 ? 1 : 0;
                        dc.Add(rolePermission);
                    }
                }
                dc.SaveChanges();
                authProps.RoleTBL = dc.dbAuthRoleTBLs.ToList();
                authProps.RolePermissionTBL = dc.dbAuthRolePermissionTBLs.ToList();
            }
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "0";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            closeDetails(true);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected void drp_has_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableChks();
        }
        protected void enableChks()
        {
            foreach (ListViewDataItem Item in LV_permission.Items)
            {
                Label lbl_id = Item.FindControl("lbl_id") as Label;
                DropDownList drp_has = Item.FindControl("drp_has") as DropDownList;
                CheckBox chk_can_edit = Item.FindControl("chk_can_edit") as CheckBox;
                CheckBox chk_can_create = Item.FindControl("chk_can_create") as CheckBox;
                CheckBox chk_can_delete = Item.FindControl("chk_can_delete") as CheckBox;
                if (lbl_id == null
                    || drp_has == null
                    || chk_can_edit == null
                    || chk_can_create == null
                    || chk_can_delete == null
                    )
                {
                    continue;
                }
                chk_can_edit.Enabled = chk_can_create.Enabled = chk_can_delete.Enabled = drp_has.SelectedValue == "1";
            }
        }
    }

}