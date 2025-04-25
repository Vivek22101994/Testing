using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_operator_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_operator";

            PH_superadmin.Visible = UserAuthentication.CurrRoleTBL.rnt_onlyOwnUserDetails.objToInt32() == 0;
        }
        private magaUser_DataContext DC_USER;
        protected string listPage = "usr_operator_list.aspx";
        private USR_ADMIN _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                Bind_drp_mailing_days();
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    USR_ADMIN _page = DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == _id && x.is_deleted != 1);
                    if (_page == null)
                        Response.Redirect(listPage);
                    HF_id.Value = Request.QueryString["id"];
                    FillControls();
                    if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnUserDetails.objToInt32() == 1 && _id != UserAuthentication.CurrentUserID)
                        Response.Redirect(listPage);
                }
                else
                    Response.Redirect(listPage);
            }
        }
        protected void Bind_drp_mailing_days()
        {
            drp_mailing_days.Items.Clear();
            List<USR_LK_MAIL_COUNT> _list = maga_DataContext.DC_USER.USR_LK_MAIL_COUNTs.OrderBy(x => x.mail_count).ToList();
            foreach (USR_LK_MAIL_COUNT _d in _list)
            {
                drp_mailing_days.Items.Add(new ListItem(_d.title, "" + _d.mail_count));
            }
            drp_mailing_days.Items.Insert(0, new ListItem("non ha limite", "0"));
        }

        private void FillControls()
        {
            _currTBL = null;
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(item => item.id == id);
                }
            }
            if (_currTBL == null)
                Response.Redirect(listPage);

            UC_usr_rl_country_lang.IdAdmin = _currTBL.id;
            UC_usr_rl_country_lang.isEdit = false;
            UC_usr_rl_country_lang.RefreshList();
            UC_usr_tbl_admin_availability_view1.IdAdmin = _currTBL.id;
            UC_usr_admin_password1.IdAdmin = _currTBL.id;

            drp_mailing_max.setSelectedValue(_currTBL.mailing_max.ToString());
            drp_mailing_days.setSelectedValue(_currTBL.mailing_days.ToString());

            txt_name.Text = _currTBL.name;
            txt_surname.Text = _currTBL.surname;
            txt_email.Text = _currTBL.email;
            txt_phone.Text = _currTBL.phone;
            txt_cell.Text = _currTBL.cell;

            AdminUtilities.Bind_drp_time(ref drp_time_start1, 0);
            drp_time_start1.setSelectedValue(_currTBL.day_1_start.ToString(), "0");
            AdminUtilities.Bind_drp_time(ref drp_time_end1, drp_time_start1.getSelectedValueInt(0).Value);
            drp_time_end1.setSelectedValue(_currTBL.day_1_end.ToString(), "0");

            AdminUtilities.Bind_drp_time(ref drp_time_start2, 0);
            drp_time_start2.setSelectedValue(_currTBL.day_2_start.ToString(), "0");
            AdminUtilities.Bind_drp_time(ref drp_time_end2, drp_time_start2.getSelectedValueInt(0).Value);
            drp_time_end2.setSelectedValue(_currTBL.day_2_end.ToString(), "0");

            AdminUtilities.Bind_drp_time(ref drp_time_start3, 0);
            drp_time_start3.setSelectedValue(_currTBL.day_3_start.ToString(), "0");
            AdminUtilities.Bind_drp_time(ref drp_time_end3, drp_time_start3.getSelectedValueInt(0).Value);
            drp_time_end3.setSelectedValue(_currTBL.day_3_end.ToString(), "0");

            AdminUtilities.Bind_drp_time(ref drp_time_start4, 0);
            drp_time_start4.setSelectedValue(_currTBL.day_4_start.ToString(), "0");
            AdminUtilities.Bind_drp_time(ref drp_time_end4, drp_time_start4.getSelectedValueInt(0).Value);
            drp_time_end4.setSelectedValue(_currTBL.day_4_end.ToString(), "0");

            AdminUtilities.Bind_drp_time(ref drp_time_start5, 0);
            drp_time_start5.setSelectedValue(_currTBL.day_5_start.ToString(), "0");
            AdminUtilities.Bind_drp_time(ref drp_time_end5, drp_time_start5.getSelectedValueInt(0).Value);
            drp_time_end5.setSelectedValue(_currTBL.day_5_end.ToString(), "0");

            AdminUtilities.Bind_drp_time(ref drp_time_start6, 0);
            drp_time_start6.setSelectedValue(_currTBL.day_6_start.ToString(), "0");
            AdminUtilities.Bind_drp_time(ref drp_time_end6, drp_time_start6.getSelectedValueInt(0).Value);
            drp_time_end6.setSelectedValue(_currTBL.day_6_end.ToString(), "0");

            AdminUtilities.Bind_drp_time(ref drp_time_start7, 0);
            drp_time_start7.setSelectedValue(_currTBL.day_7_start.ToString(), "0");
            AdminUtilities.Bind_drp_time(ref drp_time_end7, drp_time_start7.getSelectedValueInt(0).Value);
            drp_time_end7.setSelectedValue(_currTBL.day_7_end.ToString(), "0");
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
                    _currTBL = DC_USER.USR_ADMIN.SingleOrDefault(item => item.id == id);
                }
            }
            if (_currTBL==null)
                Response.Redirect(listPage);

            _currTBL.mailing_max = drp_mailing_max.getSelectedValueInt(0);
            _currTBL.mailing_days = drp_mailing_days.getSelectedValueInt(0);

            _currTBL.name = txt_name.Text;
            _currTBL.surname = txt_surname.Text;
            _currTBL.email = txt_email.Text;
            _currTBL.phone = txt_phone.Text;
            _currTBL.cell = txt_cell.Text;

            _currTBL.day_1_start = drp_time_start1.getSelectedValueInt(0);
            _currTBL.day_1_end = drp_time_end1.getSelectedValueInt(0);
            _currTBL.day_2_start = drp_time_start2.getSelectedValueInt(0);
            _currTBL.day_2_end = drp_time_end2.getSelectedValueInt(0);
            _currTBL.day_3_start = drp_time_start3.getSelectedValueInt(0);
            _currTBL.day_3_end = drp_time_end3.getSelectedValueInt(0);
            _currTBL.day_4_start = drp_time_start4.getSelectedValueInt(0);
            _currTBL.day_4_end = drp_time_end4.getSelectedValueInt(0);
            _currTBL.day_5_start = drp_time_start5.getSelectedValueInt(0);
            _currTBL.day_5_end = drp_time_end5.getSelectedValueInt(0);
            _currTBL.day_6_start = drp_time_start6.getSelectedValueInt(0);
            _currTBL.day_6_end = drp_time_end6.getSelectedValueInt(0);
            _currTBL.day_7_start = drp_time_start7.getSelectedValueInt(0);
            _currTBL.day_7_end = drp_time_end7.getSelectedValueInt(0);
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
            txt_name.ReadOnly = true;
            txt_surname.ReadOnly = true;
            txt_email.ReadOnly = true;
            txt_phone.ReadOnly = true;
            txt_cell.ReadOnly = true;

            drp_mailing_max.Enabled = false;
            drp_mailing_days.Enabled = false;

            
            drp_time_start1.Enabled = false;
            drp_time_end1.Enabled = false;
            drp_time_start2.Enabled = false;
            drp_time_end2.Enabled = false;
            drp_time_start3.Enabled = false;
            drp_time_end3.Enabled = false;
            drp_time_start4.Enabled = false;
            drp_time_end4.Enabled = false;
            drp_time_start5.Enabled = false;
            drp_time_end5.Enabled = false;
            drp_time_start6.Enabled = false;
            drp_time_end6.Enabled = false;
            drp_time_start7.Enabled = false;
            drp_time_end7.Enabled = false;
            lnk_set_time.Visible = false;

            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
        }
        protected void EnableControls()
        {
            txt_name.ReadOnly = false;
            txt_surname.ReadOnly = false;
            txt_email.ReadOnly = false;
            txt_phone.ReadOnly = false;
            txt_cell.ReadOnly = false;

            drp_mailing_max.Enabled = true;
            drp_mailing_days.Enabled = true;

            drp_time_start1.Enabled = true;
            drp_time_end1.Enabled = true;
            drp_time_start2.Enabled = true;
            drp_time_end2.Enabled = true;
            drp_time_start3.Enabled = true;
            drp_time_end3.Enabled = true;
            drp_time_start4.Enabled = true;
            drp_time_end4.Enabled = true;
            drp_time_start5.Enabled = true;
            drp_time_end5.Enabled = true;
            drp_time_start6.Enabled = true;
            drp_time_end6.Enabled = true;
            drp_time_start7.Enabled = true;
            drp_time_end7.Enabled = true;
            lnk_set_time.Visible = true;

            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
        }

        protected void drp_time_start1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminUtilities.Bind_drp_time(ref drp_time_end1, drp_time_start1.getSelectedValueInt(0).Value);
        }
        protected void drp_time_start2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminUtilities.Bind_drp_time(ref drp_time_end2, drp_time_start2.getSelectedValueInt(0).Value);
        }
        protected void drp_time_start3_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminUtilities.Bind_drp_time(ref drp_time_end3, drp_time_start3.getSelectedValueInt(0).Value);
        }
        protected void drp_time_start4_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminUtilities.Bind_drp_time(ref drp_time_end4, drp_time_start4.getSelectedValueInt(0).Value);
        }
        protected void drp_time_start5_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminUtilities.Bind_drp_time(ref drp_time_end5, drp_time_start5.getSelectedValueInt(0).Value);
        }
        protected void drp_time_start6_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminUtilities.Bind_drp_time(ref drp_time_end6, drp_time_start6.getSelectedValueInt(0).Value);
        }
        protected void drp_time_start7_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminUtilities.Bind_drp_time(ref drp_time_end7, drp_time_start7.getSelectedValueInt(0).Value);
        }

        protected void lnk_set_time_Click(object sender, EventArgs e)
        {
            AdminUtilities.Bind_drp_time(ref drp_time_start1, 0);
            drp_time_start1.setSelectedValue("0");
            AdminUtilities.Bind_drp_time(ref drp_time_end1, drp_time_start1.getSelectedValueInt(0).Value);
            drp_time_end1.setSelectedValue("23");

            AdminUtilities.Bind_drp_time(ref drp_time_start2, 0);
            drp_time_start2.setSelectedValue("0");
            AdminUtilities.Bind_drp_time(ref drp_time_end2, drp_time_start2.getSelectedValueInt(0).Value);
            drp_time_end2.setSelectedValue("23");

            AdminUtilities.Bind_drp_time(ref drp_time_start3, 0);
            drp_time_start3.setSelectedValue("0");
            AdminUtilities.Bind_drp_time(ref drp_time_end3, drp_time_start3.getSelectedValueInt(0).Value);
            drp_time_end3.setSelectedValue("23");

            AdminUtilities.Bind_drp_time(ref drp_time_start4, 0);
            drp_time_start4.setSelectedValue("0");
            AdminUtilities.Bind_drp_time(ref drp_time_end4, drp_time_start4.getSelectedValueInt(0).Value);
            drp_time_end4.setSelectedValue("23");

            AdminUtilities.Bind_drp_time(ref drp_time_start5, 0);
            drp_time_start5.setSelectedValue("0");
            AdminUtilities.Bind_drp_time(ref drp_time_end5, drp_time_start5.getSelectedValueInt(0).Value);
            drp_time_end5.setSelectedValue("23");

            AdminUtilities.Bind_drp_time(ref drp_time_start6, 0);
            drp_time_start6.setSelectedValue("0");
            AdminUtilities.Bind_drp_time(ref drp_time_end6, drp_time_start6.getSelectedValueInt(0).Value);
            drp_time_end6.setSelectedValue("23");

            AdminUtilities.Bind_drp_time(ref drp_time_start7, 0);
            drp_time_start7.setSelectedValue("0");
            AdminUtilities.Bind_drp_time(ref drp_time_end7, drp_time_start7.getSelectedValueInt(0).Value);
            drp_time_end7.setSelectedValue("23");
        }
    }
}
