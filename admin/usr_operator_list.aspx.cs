using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_operator_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_operator";
        }
        private magaUser_DataContext DC_USER;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnUserDetails.objToInt32() == 1)
                {
                    Response.Redirect("usr_operator_details.aspx?id=" + UserAuthentication.CurrentUserID);
                }
            }
        }
        protected void lnk_new_Click(object sender, EventArgs e)
        {
            //USR_ADMIN _new = new USR_ADMIN();
            //_new.name = "// da cambiare";
            //_new.surname = "";
            //_new.pid_role = 2;
            //_new.is_active = 1;
            //_new.is_deleted = 0;
            //DC_USER.USR_ADMIN.InsertOnSubmit(_new);
            //DC_USER.SubmitChanges();
            //Response.Redirect("usr_operator_details.aspx?id=" + _new.id);
        }
    }
}
