using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModContent;

namespace RentalInRome.admin
{
    public partial class def_sys_setting : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "def_sys_setting";
        }
        private dbContSysConfigTB currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                disableAll();
                fillAll();
            }
        }
        protected List<string> _NAMEs;
        protected List<string> NAMEs
        {
            get 
            {
                if (_NAMEs == null)
                {
                    _NAMEs = new List<string>();
                    _NAMEs.Add("rnt_request_relation_in_hours");
                    _NAMEs.Add("rnt_request_relation_is_view_date");
                    _NAMEs.Add("rnt_request_relation_view_fld");
                    _NAMEs.Add("rnt_reservationExpire_defaultHours");
                    _NAMEs.Add("rnt_reservationExpire_defaultHoursOnline");
                }
                return _NAMEs;
            }
        }
        protected void fillAll()
        {
            foreach (string _name in NAMEs)
            {
                FillControls(_name);
            }
        }
        protected void saveAll()
        {
            foreach (string _name in NAMEs)
            {
                FillDataFromControls(_name);
            }
            AppSettings.DEF_SYS_SETTINGs = null;
        }
        protected void enableAll()
        {
            lnk_salva_rnt_request_relation_in_hours.Visible = true;
            lnk_modify_rnt_request_relation_in_hours.Visible = false;
            lnk_cancel_rnt_request_relation_in_hours.Visible = true;
            txt_rnt_request_relation_in_hours.ReadOnly = false;
            txt_rnt_reservationExpire_defaultHours.ReadOnly = false;
            txt_rnt_reservationExpire_defaultHoursOnline.ReadOnly = false;
            drp_rnt_request_relation_is_view_date.Enabled = true;
            drp_rnt_request_relation_view_fld.Enabled = true;
        }
        protected void disableAll()
        {
            lnk_salva_rnt_request_relation_in_hours.Visible = false;
            lnk_modify_rnt_request_relation_in_hours.Visible = true;
            lnk_cancel_rnt_request_relation_in_hours.Visible = false;
            txt_rnt_request_relation_in_hours.ReadOnly = true;
            txt_rnt_reservationExpire_defaultHours.ReadOnly = true;
            txt_rnt_reservationExpire_defaultHoursOnline.ReadOnly = true;
            drp_rnt_request_relation_is_view_date.Enabled = false;
            drp_rnt_request_relation_view_fld.Enabled = false;
        }
        private void FillControls(string name)
        {
            using (DCmodContent dc = new DCmodContent())
            {
                currTBL = dc.dbContSysConfigTBs.SingleOrDefault(x => x.name == name);

                if (currTBL == null)
                    return;
                if (name == "rnt_request_relation_in_hours")
                {
                    txt_rnt_request_relation_in_hours.Text = currTBL.value;
                }
                if (name == "rnt_request_relation_is_view_date")
                {
                    drp_rnt_request_relation_is_view_date.setSelectedValue(currTBL.value);
                }
                if (name == "rnt_request_relation_view_fld")
                {
                    drp_rnt_request_relation_view_fld.setSelectedValue(currTBL.value);
                }
                if (name == "rnt_reservationExpire_defaultHours")
                {
                    txt_rnt_reservationExpire_defaultHours.Text = currTBL.value;
                }
                if (name == "rnt_reservationExpire_defaultHoursOnline")
                {
                    txt_rnt_reservationExpire_defaultHoursOnline.Text = currTBL.value;
                }
            }
        }
        private void FillDataFromControls(string name)
        {
            using (DCmodContent dc = new DCmodContent())
            {
                currTBL = dc.dbContSysConfigTBs.SingleOrDefault(x => x.name == name);
                if (currTBL == null)
                    return;
                if (name == "rnt_request_relation_in_hours")
                {
                    currTBL.value = txt_rnt_request_relation_in_hours.Text;
                }
                if (name == "rnt_request_relation_is_view_date")
                {
                    currTBL.value = drp_rnt_request_relation_is_view_date.SelectedValue;
                }
                if (name == "rnt_request_relation_view_fld")
                {
                    currTBL.value = drp_rnt_request_relation_view_fld.SelectedValue;
                }
                if (name == "rnt_reservationExpire_defaultHours")
                {
                    currTBL.value = txt_rnt_reservationExpire_defaultHours.Text;
                }
                if (name == "rnt_reservationExpire_defaultHoursOnline")
                {
                    currTBL.value = txt_rnt_reservationExpire_defaultHoursOnline.Text;
                }
                dc.SaveChanges();
                AppSettings.DEF_SYS_SETTINGs = dc.dbContSysConfigTBs.ToList();
            }
        }

        protected void lnk_salva_rnt_request_relation_in_hours_Click(object sender, EventArgs e)
        {
            disableAll();
            saveAll();
        }
        protected void lnk_cancel_rnt_request_relation_in_hours_Click(object sender, EventArgs e)
        {
            disableAll();
            fillAll();
        }
        protected void lnk_modify_rnt_request_relation_in_hours_Click(object sender, EventArgs e)
        {
            enableAll();
        }
    }
}
