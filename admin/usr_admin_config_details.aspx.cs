using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_admin_config_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        private magaUser_DataContext DC_USER;
        protected string listPage = "usr_admin_config_list.aspx";
        private USR_ADMIN_CONFIG _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                USR_ADMIN_CONFIG _page = DC_USER.USR_ADMIN_CONFIGs.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
                if (_page == null)
                    Response.Redirect(listPage);
                HF_id.Value = Request.QueryString["id"];
                FillControls();
            }
        }
        private void FillControls()
        {
            _currTBL = DC_USER.USR_ADMIN_CONFIGs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL==null)
                Response.Redirect(listPage, true);

            txt_mailing_header.Text = _currTBL.mailing_header;
            txt_mailing_signature.Text = _currTBL.mailing_signature;
            DisableControls();
        }
        private void FillDataFromControls()
        {
            _currTBL = DC_USER.USR_ADMIN_CONFIGs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
                Response.Redirect(listPage);

            _currTBL.mailing_header = txt_mailing_header.Text;
            _currTBL.mailing_signature = txt_mailing_signature.Text;
            DC_USER.SubmitChanges();
            if(UserAuthentication.CURRENT_USR_ADMIN_CONFIG!=null && UserAuthentication.CURRENT_USR_ADMIN_CONFIG.id==_currTBL.id)
                UserAuthentication.CURRENT_USR_ADMIN_CONFIG = _currTBL;

        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            DisableControls();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_modify_Click(object sender, EventArgs e)
        {
            EnableControls();
        }
        protected void DisableControls()
        {
            txt_mailing_header.ReadOnly = true;
            txt_mailing_signature.ReadOnly = true;
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
        }
        protected void EnableControls()
        {
            txt_mailing_header.ReadOnly = false;
            txt_mailing_signature.ReadOnly = false;
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(false);", true);
        }
    }
}
