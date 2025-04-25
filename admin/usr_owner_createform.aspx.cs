using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_owner_createform : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        private magaUser_DataContext DC_USER;
        private USR_TBL_OWNER _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                Bind_drp_honorific();
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

        private void FillDataFromControls()
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

            DC_USER.SubmitChanges();

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ReloadContent", "setTimeout(\"parent.ReloadContent_1('drp_owner', '" + _currTBL.id + "')\",0);", true);
        }

        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem("- seleziona -", ""));
            drp_country.setSelectedValue("108");
        }
        protected void drp_lang_DataBound(object sender, EventArgs e)
        {
            drp_lang.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
    }
}
