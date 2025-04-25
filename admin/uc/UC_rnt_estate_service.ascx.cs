using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_estate_service : System.Web.UI.UserControl
    {
        public string Service
        {
            get
            {
                return HF_service.Value;
            }
            set
            {
                HF_service.Value = value;
            }
        }
        public int IdEstate
        {
            get { return HF_IdEstate.Value.ToInt32(); }
            set { HF_IdEstate.Value = value.ToString(); }
        }
        public int IdDate
        {
            get { return HF_id.Value.ToInt32(); }
            set { HF_id.Value = value.ToString(); }
        }
        public void RefreshList()
        {
            LV.Visible = true;
            pnl_content.Visible = false;
            LV.SelectedIndex = -1;
            LDS.DataBind();
            LV.DataBind();
        }

        protected magaRental_DataContext DC_RENTAL;
        protected RNT_RL_ESTATE_SERVICE_PERIOD _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                LV.SelectedIndex = -1;
                LDS.DataBind();
                LV.DataBind();
            }
        }
        protected void setCal()
        { 
        
        }
        private void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_RL_ESTATE_SERVICE_PERIOD.SingleOrDefault(x => x.id == IdDate);
            if (_currTBL == null)
            {
                IdDate = 0;
                _currTBL = new RNT_RL_ESTATE_SERVICE_PERIOD();
            }
            DateTime _dtStart = _currTBL.dtStart.HasValue ? _currTBL.dtStart.Value : DateTime.Now;
            DateTime _dtEnd = _currTBL.dtEnd.HasValue ? _currTBL.dtEnd.Value : _dtStart.AddDays(7);
            HF_dtStart.Value = _dtStart.JSCal_dateToString();
            HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
            List<RNT_RL_ESTATE_SERVICE_PERIOD> _list = DC_RENTAL.RNT_RL_ESTATE_SERVICE_PERIOD.Where(x => x.id != IdDate && x.pid_estate==IdEstate && x.service==Service).OrderBy(x => x.dtStart).ToList();
            bool _isSet = false;
            bool _hasHole = false;
            DateTime _dtLast = _dtStart;
            string _script = "";
            _script += "function CheckDisabledDate(date){dateint = JSCal.dateToInt(date);";
            foreach (RNT_RL_ESTATE_SERVICE_PERIOD _pp in _list)
            {
                if (_isSet)
                {
                    if (_dtEnd.AddDays(1) != _pp.dtStart.Value)
                    {
                        _dtStart = _dtEnd.AddDays(1);
                        _dtEnd = _pp.dtStart.Value.AddDays(-1);
                        _hasHole = true;
                    }
                }
                if (!_hasHole)
                {
                    _dtStart = _pp.dtStart.Value;
                    _dtEnd = _pp.dtEnd.Value;
                }
                _isSet = true;
                _dtLast = _pp.dtEnd.Value;
                string _intDateFrom = "" + _pp.dtStart.Value.JSCal_dateToInt();
                string _intDateTo = "" + _pp.dtEnd.Value.JSCal_dateToInt();
                _script += "if(dateint <= " + _intDateTo + " && dateint >= " + _intDateFrom + ") return [false, \"\"];;";
            }
            _script += "return [true, \"\"];";
            _script += "}";
            if (_hasHole && IdDate == 0)
            {
                HF_dtStart.Value = _dtStart.JSCal_dateToString();
                HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
            }
            else if (IdDate == 0)
            {
                HF_dtStart.Value = _dtLast.AddDays(1).JSCal_dateToString();
                HF_dtEnd.Value = _dtLast.AddDays(8).JSCal_dateToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CheckDisabledDate", _script, true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setCal", "_JSCal_Range = new JSCal.Range({ startCont: \"#"+ HF_dtStart.ClientID +"\", startView: \"#txt_dtStart\", startDateInt: " + HF_dtStart.Value + ", endCont: \"#"+ HF_dtEnd.ClientID +"\", endView: \"#txt_dtEnd\", endDateInt: " + HF_dtEnd.Value + ", beforeShowDay: CheckDisabledDate, changeMonth: true, changeYear: true });", true);
            
        }

        protected void FillDataFromControls()
        {
            _currTBL = DC_RENTAL.RNT_RL_ESTATE_SERVICE_PERIOD.SingleOrDefault(x => x.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new RNT_RL_ESTATE_SERVICE_PERIOD();
                _currTBL.service = Service;
                _currTBL.pid_estate = IdEstate;
                DC_RENTAL.RNT_RL_ESTATE_SERVICE_PERIOD.InsertOnSubmit(_currTBL);
            }
            _currTBL.dtEnd = HF_dtEnd.Value.JSCal_stringToDate();
            _currTBL.dtStart = HF_dtStart.Value.JSCal_stringToDate();
            DC_RENTAL.SubmitChanges();
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            LV.Visible = true;
            pnl_content.Visible = false;
            LDS.DataBind();
            LV.DataBind();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LV.Visible = true;
            pnl_content.Visible = false;
        }

        protected void lnk_new_Click(object sender, EventArgs e)
        {
            HF_id.Value = "0";
            FillControls();
            LV.Visible = false;
            pnl_content.Visible = true;

        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_pid_estate = e.Item.FindControl("lbl_pid_estate") as Label;
            if (e.CommandName == "change")
            {
                LV.Visible = false;
                pnl_content.Visible = true;
                HF_id.Value = lbl_id.Text;
                FillControls();
            }
        }
    }
}