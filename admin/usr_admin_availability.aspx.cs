using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_admin_availability : adminBasePage
    {
        void UC_usr_tbl_admin_availability_insert_onChange(object sender, EventArgs e)
        {
            showUC(1);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_operator_avv";
            UC_usr_tbl_admin_availability_insert1.onChange += new EventHandler(UC_usr_tbl_admin_availability_insert_onChange);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind_drp();
                showUC(1);
            }
        }
        protected void Bind_drp()
        {
        }

        protected void drp_country_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void drp_lang_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void drp_admin_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void showUC(int mode)
        {
            if (mode == 1)
            {
                UC_usr_tbl_admin_availability_insert1.Visible = false;
                UC_usr_tbl_admin_availability_view1.Visible = true;
                UC_usr_tbl_admin_availability_view1.RefreshList();
                lnk_view.Visible = false;
                lnk_insert.Visible = true;
            }
            if (mode == 2)
            {
                UC_usr_tbl_admin_availability_insert1.Visible = true;
                UC_usr_tbl_admin_availability_insert1.RefreshList();
                UC_usr_tbl_admin_availability_view1.Visible = false;
                lnk_view.Visible = true;
                lnk_insert.Visible = false;
            }
        }

        protected void lnk_view_Click(object sender, EventArgs e)
        {
            showUC(1) ;
        }

        protected void lnk_insert_Click(object sender, EventArgs e)
        {
            showUC(2);
        }
    }
}
