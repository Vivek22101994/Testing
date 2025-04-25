using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_agent : System.Web.UI.UserControl
    {
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
            get { return HF_IdReservation.Value.ToInt64(); }
            set
            {
                HF_IdReservation.Value = value.ToString();
            }
        }
        public long IdAgent
        {
            get { return HF_IdAgent.Value.ToInt64(); }
            set
            {
                HF_IdAgent.Value = value.ToString();
                var _agency = rntProps.AgentTBL.SingleOrDefault(x => x.id == IdAgent);
                if (_agency != null && _agency.isSendManualEmail.objToInt32() == 1)
                {
                    PH_mannualSendEmail.Visible = true;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {


        }
        protected void Bind_drp_agent()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntAgentTBL> _list = dc.dbRntAgentTBLs.Where(x => x.isActive == 1).ToList();
                if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedReservations.objToInt32() == 1)
                    _list = _list.Where(x => x.pidReferer == UserAuthentication.CurrentUserID).ToList();
                drp_agent.Items.Clear();
                _list = _list.OrderBy(x => x.nameCompany).ThenBy(x => x.nameFull).ToList();
                foreach (dbRntAgentTBL _agent in _list)
                {
                    drp_agent.Items.Add(new ListItem(_agent.nameCompany + " - " + _agent.nameFull, "" + _agent.id));
                }
                drp_agent.Items.Insert(0, new ListItem("- non abbinata -", "0"));
            }
        }
        public void FillControls()
        {
            Bind_drp_agent();
            drp_agent.setSelectedValue(IdAgent.ToString());
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntAgentTBL _agent = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == drp_agent.SelectedValue.ToInt64());
                if (_agent != null)
                {
                    ltr_content.Text = _agent.nameCompany + " - " + _agent.nameFull;
                    pnl_ok.Visible = true;
                    pnl_no.Visible = false;
                }
                else
                {
                    drp_agent.setSelectedValue("0");
                    pnl_ok.Visible = false;
                    pnl_no.Visible = true;
                }
            }
            showView();
        }
        private void FillDataFromControls()
        {
            IdAgent = drp_agent.SelectedValue.objToInt64();
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

        protected void lnkSendCustomeNotificaiton_Click(object sender, EventArgs e)
        {
            UC_MannualEmail.Visible = true;
            UC_MannualEmail.IdReservation = IdReservation;
            UC_MannualEmail.FillControls();
        }
    }

}