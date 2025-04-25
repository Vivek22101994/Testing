using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_blockExpire : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_RESERVATION _currTBL;
        public event EventHandler onModify;
        public event EventHandler onSave;
        public event EventHandler onCancel;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public bool IsLocked
        {
            get { return pnl_lock.Visible; }
            set { pnl_lock.Visible = value; }
        }
        public bool IsEdit
        {
            get { return HF_isEdit.Value == "1"; }
            set { HF_isEdit.Value = value ? "1" : "0"; }
        }
        public bool IsChanged
        {
            get { return HF_isChanged.Value == "1"; }
            set { HF_isChanged.Value = value ? "1" : "0"; }
        }
        public long IdReservation
        {
            get { return HF_IdReservation.Value.ToInt32(); }
            set { HF_IdReservation.Value = value.ToString(); }
        }
        public DateTime dttCreation
        {
            get { return HF_dttCreation.Value.JSCal_stringToDateTime(); }
            set { HF_dttCreation.Value = value.JSCal_dateTimeToString(); }
        }
        public int block_expire_hours
        {
            get { return HF_block_expire_hours.Value.ToInt32(); }
            set { HF_block_expire_hours.Value = value.ToString(); }
        }
        public int block_pid_user
        {
            get { return HF_block_pid_user.Value.ToInt32(); }
            set { HF_block_pid_user.Value = value.ToString(); }
        }
        public string block_comments
        {
            get { return HF_block_comments.Value; }
            set { HF_block_comments.Value = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lnk_edit.Visible = lnk_cancel.Visible = lnk_salva.Visible = (UserAuthentication.CURRENT_USER_ROLE == 1 || UserAuthentication.CURRENT_USER_ROLE == 9);
            }
        }
        protected void Bind_drp_block_expire_hours()
        {
            List<int> _list = new List<int>() { 24, 48, 72, 96 };
            drp_block_expire_hours.Items.Clear();
            foreach (int _hours in _list)
            {
                DateTime _dt = dttCreation.AddHours(_hours);
                if (_dt > DateTime.Now)
                    drp_block_expire_hours.Items.Add(new ListItem("[" + _hours + " ore] " + _dt, "" + _hours));
            }
            if(drp_block_expire_hours.Items.Count==0)
                drp_block_expire_hours.Items.Add(new ListItem("!non è possibile, troppo vecchio!", "0"));
        }
        public void FillControls()
        {
            Bind_drp_block_expire_hours();
            DateTime _dtExpire = dttCreation.AddHours(block_expire_hours);
            ltr_date.Text = _dtExpire.formatCustom("#dd# #MM# #yy#", 1, "");
            ltr_time.Text = _dtExpire.TimeOfDay.JSTime_toString(false, true);
            ltr_user.Text = AdminUtilities.usr_adminName(block_pid_user, "");
            ltr_body.Text = block_comments;
            drp_block_expire_hours.setSelectedValue(block_expire_hours.ToString());
            txt_body.Text = block_comments;
            showView();
        }
        private void FillDataFromControls()
        {
            if (block_expire_hours == drp_block_expire_hours.getSelectedValueInt(0).objToInt32())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                    "alertError",
                                    "alert('si prega di cambiare la scadenza, oppure annullare operazione');", true);
                return;
            }
            block_expire_hours = drp_block_expire_hours.getSelectedValueInt(0).objToInt32();
            block_pid_user = UserAuthentication.CurrentUserID;
            block_comments = txt_body.Text;
            FillControls();
            if (onSave != null) { onSave(this, new EventArgs()); }
        }
        protected void showModify()
        {
            pnl_view.Visible = false;
            pnl_edit.Visible = true;
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void showView()
        {
            pnl_view.Visible = true;
            pnl_edit.Visible = false;
        }
        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            FillControls();
            if (onCancel != null) { onCancel(this, new EventArgs()); }
        }
        protected void lnk_edit_Click(object sender, EventArgs e)
        {
            showModify();
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
    }
}