using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{
    public partial class requestvisa : basePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (resUtils.CurrentIdReservation_gl != 0 && resUtils.CurrentIdReservation_gl < 150000)
            {
                Response.Redirect("/reservationarea/arrivaldeparture.aspx", true);
                return;
            }
        }
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
        private magaRental_DataContext DC_RENTAL;
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                fillData();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal_" + Unique, "setCal_" + Unique + "();", true);
        }
        protected void Bind_drp_doc_type(ref DropDownList drp)
        {
            List<USR_TB_DOC_TYPE> _list = maga_DataContext.DC_USER.USR_TB_DOC_TYPEs.ToList();
            drp.DataSource = _list;
            drp.DataTextField = "title";
            drp.DataValueField = "code";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("", ""));
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_sequence = e.Item.FindControl("lbl_sequence") as Label;
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
            txt_name_full.CssClass = "txt_name_full_" + lbl_sequence.Text;
            drp_doc_type.CssClass = "drp_doc_type_" + lbl_sequence.Text;
            txt_doc_num.CssClass = "txt_doc_num_" + lbl_sequence.Text;
            RNT_RL_RESERVATION_PERSON _rl = DC_RENTAL.RNT_RL_RESERVATION_PERSON.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
            if (_rl != null)
            {
                txt_name_full.Text = _rl.name_full;
                drp_doc_type.setSelectedValue(_rl.doc_type);
                txt_doc_num.Text = _rl.doc_num;
                //txt_doc_issue_place.Text = _rl.doc_issue_place;
            }
        }
        protected void fillData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            lnk_add.Visible = ((_currTBL.num_adult.objToInt32() + _currTBL.num_child_over.objToInt32() + _currTBL.num_child_min.objToInt32()) > _currTBL.visa_persons.objToInt32());
            visa_persons = _currTBL.visa_persons.objToInt32();
            List<RNT_RL_RESERVATION_PERSON> _list = DC_RENTAL.RNT_RL_RESERVATION_PERSON.Where(x => x.pid_reservation == _currTBL.id).OrderBy(x => x.sequence).ToList();
            if (_list.Count < visa_persons)
            {
                while (_list.Count < visa_persons)
                {
                    int _sequence = 1;
                    List<RNT_RL_RESERVATION_PERSON> _allCollection = _list.OrderBy(x => x.sequence).ToList();
                    if (_allCollection.Count != 0)
                    {
                        _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                    }
                    RNT_RL_RESERVATION_PERSON _rl = new RNT_RL_RESERVATION_PERSON();
                    _rl.sequence = _sequence;
                    _list.Add(_rl);
                }
            }
            LV.DataSource = _list;
            LV.DataBind();
            HF_controls.Value = _list.Select(x => x.sequence.ToString()).ToList().listToString("|");
        }
        protected void saveData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            foreach (ListViewDataItem Item in LV.Items)
            {
                Label lbl_id = Item.FindControl("lbl_id") as Label;
                Label lbl_sequence = Item.FindControl("lbl_sequence") as Label;
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
                    _rl.pid_reservation = CurrentIdReservation;
                    DC_RENTAL.RNT_RL_RESERVATION_PERSON.InsertOnSubmit(_rl);
                }
                _rl.sequence = lbl_sequence.Text.ToInt32();
                _rl.name_full = txt_name_full.Text;
                _rl.doc_type = drp_doc_type.SelectedValue;
                _rl.doc_num = txt_doc_num.Text;
                //_client.birth_place = txt_birth_place.Text;
                //_client.birth_date = HF_birth_date.Value.JSCal_stringToDate();
                //_client.doc_issue_place = txt_doc_issue_place.Text;
                //_client.doc_expiry_date = HF_doc_expiry_date.Value.JSCal_stringToDate();
                DC_RENTAL.SubmitChanges();
            }
            _currTBL.visa_isRequested = 1;
            _currTBL.visa_persons = LV.Items.Count;
            DC_RENTAL.SubmitChanges();
            Response.Redirect("requestvisa.aspx", true);
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            saveData();
        }
        protected void lnk_add_Click(object sender, EventArgs e)
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            if ((_currTBL.num_adult.objToInt32() + _currTBL.num_child_over.objToInt32() + _currTBL.num_child_min.objToInt32()) > _currTBL.visa_persons.objToInt32())
            {
                _currTBL.visa_persons = _currTBL.visa_persons.objToInt32() + 1;
                DC_RENTAL.SubmitChanges();
                Response.Redirect("requestvisa.aspx", true);
            }
        }
    }
}