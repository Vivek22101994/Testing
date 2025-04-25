using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_country_lang : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_country_lang";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind_drp();
                UC_usr_rl_country_lang.IdCountry = drp_country.getSelectedValueInt(0).Value;
                UC_usr_rl_country_lang.RefreshList();
            }
        }
        protected void Bind_drp()
        {
            drp_country.DataBind();
            drp_country.Items.Insert(0, new ListItem("- tutti -", "0"));
            drp_country.Items.Insert(0, new ListItem("- non filtrare -", "-1"));
            List<CONT_TBL_LANG> _langs = maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.Where(x => x.is_active == 1).ToList();
            drp_lang.DataSource = _langs;
            drp_lang.DataTextField = "title";
            drp_lang.DataValueField = "id";
            drp_lang.DataBind();
            drp_lang.Items.Insert(0, new ListItem("- tutti -", "0"));
            drp_lang.Items.Insert(0, new ListItem("- non filtrare -", "-1"));
            drp_admin.Items.Clear();
            List<USR_ADMIN> _admins = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && (x.rnt_canHaveRequest == 1 || x.rnt_canHaveReservation == 1) && x.is_deleted == 0).ToList();
            foreach (USR_ADMIN usrAdmin in _admins)
            {
                drp_admin.Items.Add(new ListItem("" + usrAdmin.name + " " + usrAdmin.surname, "" + usrAdmin.id));
            }
            drp_admin.Items.Insert(0, new ListItem("- non filtrare -", "-1"));
        }

        protected void drp_country_SelectedIndexChanged(object sender, EventArgs e)
        {
            UC_usr_rl_country_lang.IdCountry = drp_country.getSelectedValueInt(0).Value;
            UC_usr_rl_country_lang.isEdit = true;
            UC_usr_rl_country_lang.RefreshList();
        }
        protected void drp_lang_SelectedIndexChanged(object sender, EventArgs e)
        {
            UC_usr_rl_country_lang.IdLang = drp_lang.getSelectedValueInt(0).Value;
            UC_usr_rl_country_lang.isEdit = drp_lang.getSelectedValueInt(0)==0;
            UC_usr_rl_country_lang.RefreshList();
        }
        protected void drp_admin_SelectedIndexChanged(object sender, EventArgs e)
        {
            UC_usr_rl_country_lang.IdAdmin = drp_admin.getSelectedValueInt(0).Value;
            UC_usr_rl_country_lang.isEdit = drp_admin.getSelectedValueInt(0)==0;
            UC_usr_rl_country_lang.RefreshList();
        }
    }
}
