using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_owner_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_owner";
        }
        private magaUser_DataContext DC_USER;
        protected string listPage = "usr_owner_list.aspx";
        private USR_TBL_OWNER _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                Bind_drp_honorific();
                HF_id.Value = Request.QueryString["id"].objToInt32().ToString();
                FillControls();
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
            _currTBL = DC_USER.USR_TBL_OWNER.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new USR_TBL_OWNER();
                UC_usr_owner_password1.Visible = false;
                EnableControls();
            }
            else
            {
                UC_usr_owner_password1.IdOwner = _currTBL.id;
                UC_usr_owner_password1.Visible = true;
                DisableControls();
            }

            drp_honorific.setSelectedValue(_currTBL.name_honorific);
            txt_name_full.Text = _currTBL.name_full;
            txt_contact_email.Text = _currTBL.contact_email;
            txt_contact_email_2.Text = _currTBL.contact_email_2;
            txt_contact_email_3.Text = _currTBL.contact_email_3;
            txt_contact_email_4.Text = _currTBL.contact_email_4;
            //txt_contact_email_5.Text = _currTBL.contact_email_5;
            txt_contact_phone.Text = _currTBL.contact_phone;
            txt_contact_phone_mobile.Text = _currTBL.contact_phone_mobile;
            drp_country.setSelectedValue(_currTBL.loc_country);
            drp_lang.setSelectedValue(_currTBL.pid_lang.ToString());

            re_notesInner.Content = _currTBL.inner_notes;
        }

        private void FillDataFromControls()
        {
            bool _isNew = false;
            _currTBL = DC_USER.USR_TBL_OWNER.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new USR_TBL_OWNER();
                _currTBL.count_pwd_sent = 0;
                _currTBL.is_active = 1;
                _currTBL.is_deleted = 0;
                _currTBL.date_created = DateTime.Now;
                _currTBL.login = txt_contact_email.Text;
                _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
                DC_USER.USR_TBL_OWNER.InsertOnSubmit(_currTBL);
                DC_USER.SubmitChanges();
                _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
                DC_USER.SubmitChanges();
                _isNew = true;
            }
            _currTBL.name_honorific = drp_honorific.SelectedValue;
            _currTBL.name_full = txt_name_full.Text;
            _currTBL.contact_email = txt_contact_email.Text;
            _currTBL.contact_email_2 = txt_contact_email_2.Text;
            _currTBL.contact_email_3 = txt_contact_email_3.Text;
            _currTBL.contact_email_4 = txt_contact_email_4.Text;
            //_currTBL.contact_email_5 = txt_contact_email_5.Text;
            _currTBL.contact_phone = txt_contact_phone.Text;
            _currTBL.contact_phone_mobile = txt_contact_phone_mobile.Text;
            _currTBL.loc_country = drp_country.SelectedValue;
            _currTBL.pid_lang = drp_lang.getSelectedValueInt(0);

            _currTBL.inner_notes = re_notesInner.Content;

            DC_USER.SubmitChanges();
            Response.Redirect("usr_owner_details.aspx?id=" + _currTBL.id, true);
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
            re_notesInner.Enabled = false;

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
            re_notesInner.Enabled = true;

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
