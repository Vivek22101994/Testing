using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_client_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_client";
        }
        private magaUser_DataContext DC_USER;
        protected string listPage = "usr_client_list.aspx";
        private USR_TBL_CLIENT _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                Bind_drp_honorific();
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    USR_TBL_CLIENT _page = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == _id && x.is_deleted == 0);
                    if (_page == null)
                        Response.Redirect(listPage);
                    HF_id.Value = Request.QueryString["id"];
                    FillControls();
                }
                else
                    Response.Redirect(listPage);
            }
        }
        protected void Bind_drp_honorific()
        {
            List<USR_LK_HONORIFIC> _list = DC_USER.USR_LK_HONORIFICs.OrderBy(x => x.title).ToList();
            drp_honorific.DataSource = _list;
            drp_honorific.DataTextField = "title";
            drp_honorific.DataValueField = "title";
            drp_honorific.DataBind();
        }

        private void FillControls()
        {
            drp_country.DataBind();
            drp_lang.DataBind();
            _currTBL = null;
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(item => item.id == id);
                }
            }
            if (_currTBL == null)
                Response.Redirect(listPage);

            UC_usr_client_password1.IdClient = _currTBL.id;
            drp_honorific.setSelectedValue(_currTBL.name_honorific);
            txt_name_full.Text = _currTBL.name_full;
            txt_contact_email.Text = _currTBL.contact_email;
            txt_contact_phone.Text = _currTBL.contact_phone;
            txt_contact_phone_mobile.Text = _currTBL.contact_phone_mobile;
            drp_country.setSelectedValue(_currTBL.loc_country);
            drp_lang.setSelectedValue(_currTBL.pid_lang.ToString());
            DisableControls();
        }

        private void FillDataFromControls()
        {
            _currTBL = null;
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(item => item.id == id);
                }
            }
            if (_currTBL == null)
                Response.Redirect(listPage);
            _currTBL.name_honorific = drp_honorific.SelectedValue;
            _currTBL.name_full = txt_name_full.Text;
            _currTBL.contact_email = txt_contact_email.Text;
            _currTBL.contact_phone = txt_contact_phone.Text;
            _currTBL.contact_phone_mobile = txt_contact_phone_mobile.Text;
            _currTBL.loc_country = drp_country.SelectedValue;
            _currTBL.pid_lang = drp_lang.getSelectedValueInt(0);

            DC_USER.SubmitChanges();
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
            txt_name_full.ReadOnly = true;
            txt_contact_email.ReadOnly = true;
            txt_contact_phone.ReadOnly = true;
            txt_contact_phone_mobile.ReadOnly = true;

            drp_country.Enabled = false;
            drp_lang.Enabled = false;

            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
        }
        protected void EnableControls()
        {
            txt_name_full.ReadOnly = false;
            txt_contact_email.ReadOnly = false;
            txt_contact_phone.ReadOnly = false;
            txt_contact_phone_mobile.ReadOnly = false;

            drp_country.Enabled = true;
            drp_lang.Enabled = true;

            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
        }
        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem("- seleziona -", ""));
        }
        protected void drp_lang_DataBound(object sender, EventArgs e)
        {
            drp_lang.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
    }
}
