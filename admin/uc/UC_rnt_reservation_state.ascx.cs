using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_reservation_state : System.Web.UI.UserControl
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
        public bool ShowBody
        {
            get
            {
                return HF_show_body.Value == "1";
            }
            set
            {
                HF_show_body.Value = value ? "1" : "0";
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
        public int IdState
        {
            get { return HF_id.Value.ToInt32(); }
            set { HF_id.Value = value.ToString(); }
        }
        public long IdReservation
        {
            get { return HF_IdReservation.Value.ToInt32(); }
            set { HF_IdReservation.Value = value.ToString(); }
        }
        public int state_pid
        {
            get { return HF_state_pid.Value.ToInt32(); }
            set { HF_state_pid.Value = value.ToString(); }
        }
        public string state_subject
        {
            get { return HF_state_subject.Value; }
            set { HF_state_subject.Value = value; }
        }
        public string state_body
        {
            get { return HF_state_body.Value; }
            set { HF_state_body.Value = value; }
        }
        public int HAHAstateCancelledBy
        {
            get { return HF_HAHAstateCancelledBy.Value.ToInt32(); }
            set { HF_HAHAstateCancelledBy.Value = value.ToString(); }
        }
        public RNT_TBL_RESERVATION tblReservation
        {
            get
            {
                if (_currTBL == null)
                    _currTBL = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                return _currTBL ?? new RNT_TBL_RESERVATION();
            }
        }
        protected dbRntAgentTBL ISHAagent;
        public bool IsHA = false;
        public dbRntAgentTBL _agent
        {
            get
            {
                IsHA = false;
                using (DCmodRental dc = new DCmodRental())
                {
                    if (ISHAagent == null)
                    {
                        if (tblReservation.agentID.objToInt64() > 0 && dc.dbRntAgentTBLs.Any(x => x.id == tblReservation.agentID))
                        {
                            ISHAagent = dc.dbRntAgentTBLs.FirstOrDefault(x => x.id == tblReservation.agentID);

                            if (ISHAagent!=null && ISHAagent.IdAdMedia!=null && (ISHAagent.IdAdMedia + "").ToLower() == ChnlHomeAwayProps_V411.IdAdMedia.ToLower())
                            {
                                IsHA = true;
                                drp_reasonBox.Visible = true;

                                drp_reason.Visible = true;
                            }
                        }

                    }
                }

                return ISHAagent ?? new dbRntAgentTBL();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                string _script = "var " + Unique + "_editors = ['" + txt_body.ClientID + "'];";
                _script += "function  " + Unique + "_removeTinyEditor() {";
                _script += "removeTinyEditors( " + Unique + "_editors);}";
                _script += "function  " + Unique + "_setTinyEditor(IsReadOnly) {";
                _script += "setTinyEditors( " + Unique + "_editors, IsReadOnly);}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Unique, _script, true);
            }
            dbRntAgentTBL agent = _agent;
            if (IsHA)
            {
                drp_reasonBox.Visible = true;
                drp_reason.Visible = true;
            }
        }
        protected void Bind_drp_reason(int current)
        {
            drp_reason.Items.Add(new ListItem(contUtils.getLabel("rntReservationState_CancelledbyOwner3", App.LangID, ""), "" + 0));
            drp_reason.Items.Add(new ListItem(contUtils.getLabel("rntReservationState_CancelledbyTraveler3", App.LangID, ""), "" + 1));

            dbRntAgentTBL agent = _agent;
            if (IsHA)
            {
                drp_reasonBox.Visible = true;

                drp_reason.Visible = true;
                drp_reason.setSelectedValue(current);
            }
        }
        protected void Bind_drp_state(int current)
        {
            List<RNT_LK_RESERVATION_STATE> _list = DC_RENTAL.RNT_LK_RESERVATION_STATEs.Where(x => x.type == 1 && x.id != state_pid).ToList();
            if (current == 0)
                _list = _list.Where(x => x.id == 6).ToList();
            else if (current == 3)
            {
                if (UserAuthentication.CURRENT_USER_ROLE == 1)
                {
                    pnlCommentEdit.Visible = true;
                    _list = _list.Where(x => x.id == 3 || x.id == 4 || x.id == 6).ToList();
                    lnk_salva.Visible = true;
                }
                else
                {
                    _list = _list.Where(x => x.id == 3).ToList();
                    lnk_salva.Visible = false;
                }
            }
            else
                _list = _list.Where(x => x.id == 3 || x.id == 4 || x.id == 5).ToList();
            drp_state.DataSource = _list;
            drp_state.DataTextField = "title";
            drp_state.DataValueField = "id";
            drp_state.DataBind();
        }
        public void FillControls()
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            _currTBL = null;
            if (IdReservation != 0)
            {
                _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            }
            if (_currTBL == null)
            {
                if (IsChanged)
                {
                    ltr_user.Text = CommonUtilities.GetUserName("" + UserAuthentication.CurrentUserID);
                    ltr_state.Text = rntUtils.rntReservation_getStateName(state_pid);
                    ltr_date.Text = DateTime.Now.formatCustom("#dd# #MM# #yy#", 1, "");
                    ltr_time.Text = DateTime.Now.TimeOfDay.JSTime_toString(false, true);
                    ltr_subject.Text = state_subject;
                    ltr_body.Text = state_body;
                    showView();
                    return;
                }
                IdReservation = 0;
                // todo OPZIONI o PREN
                Bind_drp_state(0);
                Bind_drp_reason(0);
                showModify();
            }
            else
            {
                Bind_drp_state(_currTBL.state_pid.objToInt32());
                Bind_drp_reason(_currTBL.HAstateCancelledBy.objToInt32());
  
                if (IsChanged)
                {
                    ltr_user.Text = CommonUtilities.GetUserName("" + UserAuthentication.CurrentUserID);
                    ltr_state.Text = rntUtils.rntReservation_getStateName(state_pid);
                    ltr_date.Text = DateTime.Now.formatCustom("#dd# #MM# #yy#", 1, "");
                    ltr_time.Text = DateTime.Now.TimeOfDay.JSTime_toString(false, true);
                    ltr_subject.Text = state_subject;
                    ltr_body.Text = state_body;
                }
                else
                {
                    ltr_user.Text = CommonUtilities.GetUserName("" + _currTBL.state_pid_user);
                    ltr_state.Text = rntUtils.rntReservation_getStateName(_currTBL.state_pid);
                    ltr_date.Text = _currTBL.state_date.formatCustom("#dd# #MM# #yy#", 1, "");
                    ltr_time.Text = _currTBL.state_date.Value.TimeOfDay.JSTime_toString(false, true);
                    ltr_subject.Text = _currTBL.state_subject;
                    ltr_body.Text = _currTBL.state_body;
                }
                LV.SelectedIndex = -1;
                LDS.DataBind();
                LV.DataBind();
                showView();
            }
        }
        private void FillDataFromControls()
        {
            IsChanged = true;
            _currTBL = null;
            state_body = txt_body.Text;
            state_subject = txt_subject.Text;
            state_pid = drp_state.getSelectedValueInt(0).objToInt32();
            HAHAstateCancelledBy = drp_reason.getSelectedValueInt(0).objToInt32();
           
            //_currTBL.state_body = state_body;
            //_currTBL.state_subject = state_subject;
            //_currTBL.state_date = DateTime.Now;
            //_currTBL.state_pid = state_pid;
            //_currTBL.state_pid_user = UserAuthentication.CurrentUserID;
            //DC_RENTAL.SubmitChanges();
            //rntUtils.rntReservation_onStateChange(_currTBL);
            FillControls();
            if (onSave != null) { onSave(this, new EventArgs()); }
        }
        protected void showModify()
        {
            pnl_view.Visible = false;
            pnl_edit.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Unique + "_tinyEditor", "setTinyEditors( " + Unique + "_editors, true); ", true);
            if (onModify != null) { onModify(this, new EventArgs()); }
        }
        protected void showView()
        {
            pnl_view.Visible = true;
            pnl_edit.Visible = false;
        }
        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            showView();
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
            ShowBody = false;
        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "close")
            {
                LV.SelectedIndex = -1;
                ShowBody = false;
            }
        }

        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            Label lbl_id = LV.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            if (lbl_id != null)
            {
            }
            ShowBody = false;
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_pid_state = e.Item.FindControl("lbl_pid_state") as Label;
            System.Web.UI.HtmlControls.HtmlTableRow tr_normal = e.Item.FindControl("tr_normal") as System.Web.UI.HtmlControls.HtmlTableRow;
            LinkButton lnk_select = e.Item.FindControl("lnk_select") as LinkButton;
            if (tr_normal != null && lnk_select != null)
            {
                if (lbl_pid_state != null && lbl_pid_state.Text != "0" && lbl_pid_state.Text != "1")
                {
                    tr_normal.Attributes.Add("onclick", "__doPostBack('" + lnk_select.UniqueID + "','')");
                    tr_normal.Style.Add("cursor", "pointer");
                    lnk_select.Visible = true;
                }
                else
                {
                    lnk_select.Visible = false;
                }
            }
            System.Web.UI.HtmlControls.HtmlTableRow tr_selected = e.Item.FindControl("tr_selected") as System.Web.UI.HtmlControls.HtmlTableRow;
            LinkButton lnk_close = e.Item.FindControl("lnk_close") as LinkButton;
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (tr_selected != null && lnk_close != null && lbl_id != null)
            {
                tr_selected.Attributes.Add("onclick", "__doPostBack('" + lnk_close.UniqueID + "','')");
            }
        }
    }
}