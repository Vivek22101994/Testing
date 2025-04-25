using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModAuth.admin.modAuth
{
    public partial class userList : adminBasePage
    {
        protected dbAuthUserTBL currTBL;
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
                    currTBL = dc.dbAuthUserTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        if (currTBL.id < 3) return;
                        dc.Delete(currTBL);
                        dc.SaveChanges();
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
                drp_pidRole.DataSource = authProps.RoleTBL.OrderBy(x=>x.title);
                drp_pidRole.DataTextField = "title";
                drp_pidRole.DataValueField = "id";
                drp_pidRole.DataBind();
                currTBL = dc.dbAuthUserTBLs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null) currTBL = new dbAuthUserTBL();

                txt_nameFull.Text = currTBL.nameFull;
                txt_email.Text = currTBL.email;
                txt_phone.Text = currTBL.phone;
                txt_phoneMobile.Text = currTBL.phoneMobile;
                txt_login.Text = currTBL.login;
                txt_password.Text = currTBL.password;
                //drp_pidRole.setSelectedValue(currTBL.pidRole);
                //drp_isActive.setSelectedValue(currTBL.isActive);
 
                rwdDett.VisibleOnPageLoad = true;
            }
        }
        private void saveData()
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                currTBL = dc.dbAuthUserTBLs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbAuthUserTBL();
                    dc.Add(currTBL);
                }
                currTBL.nameFull = txt_nameFull.Text;
                currTBL.email = txt_email.Text;
                currTBL.phone = txt_phone.Text;
                currTBL.phoneMobile = txt_phoneMobile.Text;
                currTBL.login = txt_login.Text;
                currTBL.password = txt_password.Text;
                //currTBL.pidRole = drp_pidRole.getSelectedValueInt();
                //currTBL.isActive = drp_isActive.getSelectedValueInt();

                dc.SaveChanges();
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
    }

}