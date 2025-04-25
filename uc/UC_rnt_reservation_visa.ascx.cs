using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.uc
{
    public partial class UC_rnt_reservation_visa : System.Web.UI.UserControl
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
        public int visa_persons
        {
            get
            {
                return HF_visa_persons.Value.ToInt32();
            }
            set
            {
                HF_visa_persons.Value = value.ToString();
            }
        }
        public long IdReservation
        {
            get
            {
                return HF_IdReservation.Value.ToInt32();
            }
            set
            {
                HF_IdReservation.Value = value.ToString();
            }
        }
        private RNT_TBL_RESERVATION _currTBL;
        private magaRental_DataContext DC_RENTAL;
        private magaUser_DataContext DC_USER;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                lnk_saveClientData.OnClientClick = "return _validateForm_" + Unique;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal_" + Unique, "setCal_" + Unique + "();", true);
            }
        }
        protected void Bind_drp_doc_type(ref DropDownList drp)
        {
            List<USR_TB_DOC_TYPE> _list = DC_USER.USR_TB_DOC_TYPEs.ToList();
            drp.DataSource = _list;
            drp.DataTextField = "title";
            drp.DataValueField = "code";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("", ""));
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            DropDownList drp_doc_type = e.Item.FindControl("drp_doc_type") as DropDownList;
            TextBox txt_name_full = e.Item.FindControl("txt_name_full") as TextBox;
            TextBox txt_doc_num = e.Item.FindControl("txt_doc_num") as TextBox;
            TextBox txt_doc_issue_place = e.Item.FindControl("txt_doc_issue_place") as TextBox;
            TextBox txt_doc_expiry_date = e.Item.FindControl("txt_doc_expiry_date") as TextBox;
            HiddenField HF_doc_expiry_date = e.Item.FindControl("HF_doc_expiry_date") as HiddenField;
            if (drp_doc_type != null)
            {
                Bind_drp_doc_type(ref drp_doc_type);
            }
            RNT_RL_RESERVATION_PERSON _rl = DC_RENTAL.RNT_RL_RESERVATION_PERSON.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
            if (_rl != null)
            {
                txt_name_full.Text = _rl.name_full;
                drp_doc_type.setSelectedValue(_rl.doc_type);
                txt_doc_num.Text = _rl.doc_num;
                //txt_doc_issue_place.Text = _rl.doc_issue_place;
            }
        }

        protected void lnk_saveClientData_Click(object sender, EventArgs e)
        {
            saveData();
        }
        public void fillData()
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_USER = maga_DataContext.DC_USER;
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null) return;
            List<RNT_RL_RESERVATION_PERSON> _list = DC_RENTAL.RNT_RL_RESERVATION_PERSON.Where(x=>x.pid_reservation==_currTBL.id).ToList();
            if (_list.Count < visa_persons)
            {
                while (_list.Count < visa_persons)
                {
                    _list.Add(new RNT_RL_RESERVATION_PERSON()); 
                }
            }
            LV.DataSource = _list;
            LV.DataBind();

        }
        protected void saveData()
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_USER = maga_DataContext.DC_USER;
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currTBL == null) return;
            foreach (ListViewDataItem Item in LV.Items)
            {
                Label lbl_id = Item.FindControl("lbl_id") as Label;
                DropDownList drp_doc_type = Item.FindControl("drp_doc_type") as DropDownList;
                TextBox txt_name_full = Item.FindControl("txt_name_full") as TextBox;
                TextBox txt_doc_num = Item.FindControl("txt_doc_num") as TextBox;
                TextBox txt_doc_issue_place = Item.FindControl("txt_doc_issue_place") as TextBox;
                TextBox txt_doc_expiry_date = Item.FindControl("txt_doc_expiry_date") as TextBox;
                HiddenField HF_doc_expiry_date = Item.FindControl("HF_doc_expiry_date") as HiddenField;
                RNT_RL_RESERVATION_PERSON _rl = DC_RENTAL.RNT_RL_RESERVATION_PERSON.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (_rl == null)
                {
                    _rl = new RNT_RL_RESERVATION_PERSON();
                    _rl.pid_reservation = IdReservation;
                    DC_RENTAL.RNT_RL_RESERVATION_PERSON.InsertOnSubmit(_rl);
                }
                _rl.name_full = txt_name_full.Text;
                _rl.doc_type = drp_doc_type.SelectedValue;
                _rl.doc_num =txt_doc_num.Text;
                //_client.birth_place = txt_birth_place.Text;
                //_client.birth_date = HF_birth_date.Value.JSCal_stringToDate();
                //_client.doc_issue_place = txt_doc_issue_place.Text;
                //_client.doc_expiry_date = HF_doc_expiry_date.Value.JSCal_stringToDate();
                DC_RENTAL.SubmitChanges();
            }
            _currTBL.visa_isRequested = 1;
            _currTBL.visa_persons = LV.Items.Count;
            DC_RENTAL.SubmitChanges();
        }
    }
}