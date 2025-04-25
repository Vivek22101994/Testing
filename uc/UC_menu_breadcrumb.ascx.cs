using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.uc
{
    public partial class UC_menu_breadcrumb : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                this.Visible = setHLs();
            }
        }
        protected bool setHLs()
        {
            mainBasePage m = (mainBasePage)this.Page;
            if (m == null)
                return false;
            string pt = m.PAGE_TYPE;
            int pid = m.PAGE_REF_ID;
            if (pt == "pg_estate")
            {
                RNT_TB_ESTATE _est = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == pid);
                if (_est == null) return false;
                HL_1.Visible = true;
                if (_est.category=="villa")
                {
                    HL_1.Text = CurrentSource.getSysLangValue("menuRomeVillas");
                    HL_1.NavigateUrl = CurrentSource.getPagePath("24", "stp", CurrentLang.ID.ToString());
                }
                else
                {
                    HL_1.Text = CurrentSource.locZone_title(_est.pid_zone.objToInt32(), CurrentLang.ID, "");
                    HL_1.NavigateUrl = CurrentSource.getPagePath(_est.pid_zone.ToString(), "pg_zone", CurrentLang.ID.ToString());
                }
            }
            if (pt == "stp" && pid == 4 && Request.QueryString["IdEstate"].objToInt32()!=0)
            {
                HL_1.Visible = true;
                HL_1.Text = CurrentSource.getPageName(Request.QueryString["IdEstate"].objToInt32().ToString(), "pg_estate", CurrentLang.ID.ToString());
                HL_1.NavigateUrl = CurrentSource.getPagePath(Request.QueryString["IdEstate"].objToInt32().ToString(), "pg_estate", CurrentLang.ID.ToString());
            }
            lbl_current.Text = CurrentSource.getPageName(pid.ToString(), pt, CurrentLang.ID.ToString());
            return true;
        }
    }
}