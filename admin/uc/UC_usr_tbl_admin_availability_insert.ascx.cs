using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_usr_tbl_admin_availability_insert : System.Web.UI.UserControl
    {
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public event EventHandler onChange;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind_drp_availability();
                Bind_chkList_utenti();
            }
            RegisterScripts();
        }
        public void RefreshList()
        {
            drp_availability.setSelectedValue("-1");
            drp_admins.setSelectedValue("-1");
        }
        protected void Bind_chkList_utenti()
        {
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_deleted != 1 && x.is_active == 1 && x.id > 2).ToList();
            drp_admins.Items.Clear();
            foreach (USR_ADMIN _utenti in _list)
            {
                drp_admins.Items.Add(new ListItem("" + _utenti.name + " " + _utenti.surname, "" + _utenti.id));
            }
            drp_admins.Items.Insert(0, new ListItem("-seleziona-", "-1"));
        }
        protected void Bind_drp_availability()
        {
            List<USR_LK_ADMIN_AVAILABILITY> _list = maga_DataContext.DC_USER.USR_LK_ADMIN_AVAILABILITies.Where(x => x.title != "").ToList();
            drp_availability.Items.Clear();
            foreach (USR_LK_ADMIN_AVAILABILITY _d in _list)
            {
                drp_availability.Items.Add(new ListItem("" + _d.description, "" + _d.id));
            }
            drp_availability.Items.Insert(0, new ListItem("-seleziona-", "-1"));
        }
        protected void lnk_ins_Click(object sender, EventArgs e)
        {
            List<DateTime> _newDates = new List<DateTime>();
            DateTime _dt = DateTime.Now.AddDays(-30);
            DateTime _dtTo = DateTime.Now;
            if (HF_date_from.Value != "" && HF_date_to.Value != "")
            {
                _dt = HF_date_from.Value.JSCal_stringToDate();
                _dtTo = HF_date_to.Value.JSCal_stringToDate();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('entrambe date inizio/fine devono essere selezionate!');", true);
                return;
            }
            if (drp_admins.getSelectedValueInt(0)==-1)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('seleziona Account!');", true);
                return;
            }
            USR_LK_ADMIN_AVAILABILITY _avv = maga_DataContext.DC_USER.USR_LK_ADMIN_AVAILABILITies.SingleOrDefault(x => x.id == drp_availability.getSelectedValueInt(0));
            if(_avv==null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('seleziona Tipo di Disponibilità!');", true);
                return;
            }
            magaUser_DataContext _db = maga_DataContext.DC_USER;
            while (_dt <= _dtTo)
            {
                _newDates.Add(_dt);
                _dt = _dt.AddDays(1);
            }
            foreach (DateTime _date in _newDates)
            {
                USR_TBL_ADMIN_AVAILABILITY _presenza = _db.USR_TBL_ADMIN_AVAILABILITies.SingleOrDefault(x => x.pid_admin == drp_admins.getSelectedValueInt(0) && x.date_availability == _date);
                if (_presenza == null)
                {
                    _presenza = new USR_TBL_ADMIN_AVAILABILITY();
                    _db.USR_TBL_ADMIN_AVAILABILITies.InsertOnSubmit(_presenza);
                }
                _presenza.pid_availability = _avv.id;
                _presenza.pid_admin = drp_admins.getSelectedValueInt(0);
                _presenza.date_availability = _date;
                _presenza.pid_user = UserAuthentication.CurrentUserID;
                _presenza.is_mailing_day = _avv.is_mailing_day;
                _presenza.is_working_day = _avv.is_working_day;
            }
            _db.SubmitChanges();
            if (onChange != null) { onChange(this, new EventArgs()); }
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cal_dtIns_from", "cal_dtIns_from = new JSCal.Single({ dtFormat: \"d MM yy\", Cont: \"#" + HF_date_from.ClientID + "\", View: \"#cal_dtIns_from\", changeMonth: true, changeYear: true });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cal_dtIns_to", "cal_dtIns_to = new JSCal.Single({ dtFormat: \"d MM yy\", Cont: \"#" + HF_date_to.ClientID + "\", View: \"#cal_dtIns_to\", changeMonth: true, changeYear: true });", true);
        }
    }
}