using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_estate_comment : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate_comment";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_ESTATE_COMMENT _currTBL;

        public class customType
        {
            public int id { get; set; }
            public string code { get; set; }
            public string title { get; set; }
            public customType(int _id, string _code, string _title)
            {
                id = _id;
                code = _code;
                title = _title;
            }
        }
        private List<customType> _types_;
        private List<customType> _types
        {
            get
            {
                if (_types_ == null)
                {
                    _types_=  new List<customType>();
                    _types_.Add(new customType(1,"mail","E-mail"));
                    _types_.Add(new customType(2, "web", "Web"));
                }
                return _types_;
            }
            set { _types_ = value; }
        }
        private List<customType> _pers_;
        private List<customType> _pers
        {
            get
            {
                if (_pers_ == null)
                {
                    _pers_ = new List<customType>();
                    _pers_.Add(new customType(1, "m", "Uomo"));
                    _pers_.Add(new customType(2, "f", "Donna"));
                    _pers_.Add(new customType(3, "co", "Coppia"));
                    _pers_.Add(new customType(4, "fam", "Famiglia"));
                    _pers_.Add(new customType(5, "gr", "Gruppo"));
                }
                return _pers_;
            }
            set { _pers_ = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                lnk_save.Visible = lnk_cancel.Visible = UserAuthentication.hasPermission("rnt_estate", "can_edit");
                Bind_drp_vote();
                Bind_drp_type();
                Bind_drp_pers();
                Bind_drp_estate();
                Bind_drp_dtComment_hour();
                Bind_drp_dtComment_minute();
            }
        }
        protected void Bind_drp_estate()
        {
            List<RNT_TB_ESTATE> _list = DC_RENTAL.RNT_TB_ESTATE.OrderBy(x=>x.code).ToList();
            drp_estate.DataSource = _list;
            drp_estate.DataTextField = "code";
            drp_estate.DataValueField = "id";
            drp_estate.DataBind();
            drp_estate.Items.Insert(0, new ListItem("-tutti-", "0"));
            drp_estate.Items.Insert(0, new ListItem("-seleziona-", "-1"));

        }
        protected void Bind_drp_vote()
        {
            for (int i = 0; i < 11; i++)
            {
                drp_vote.Items.Add(new ListItem("" + i, "" + i));
            }
        }
        protected void Bind_drp_type()
        {
            foreach (customType type in _types)
            {
                drp_type.Items.Add(new ListItem("" + type.title, "" + type.code));
            }
        }
        protected void Bind_drp_pers()
        {
            foreach (customType type in _pers)
            {
                drp_pers.Items.Add(new ListItem("" + type.title, "" + type.code));
            }
        }
        protected void Bind_drp_dtComment_hour()
        {
            for (int i = 0; i < 24; i++)
            {
                drp_dtComment_hour.Items.Add(new ListItem((i < 10) ? "0" + i : "" + i, "" + i));
            }
        }
        protected void Bind_drp_dtComment_minute()
        {
            for (int i = 0; i < 60; i++)
            {
                drp_dtComment_minute.Items.Add(new ListItem((i<10)?"0" + i:"" + i, "" + i));
            }
        }
        protected string getType(string type)
        {
            customType _type = _types.SingleOrDefault(x => x.code == type);
            return _type != null ? _type.title : "";
        }
        protected string getPers(string type)
        {
            customType _per = _pers.SingleOrDefault(x => x.code == type);
            return _per != null ? _per.title : "";
        }

        private void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TBL_ESTATE_COMMENTs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new RNT_TBL_ESTATE_COMMENT();
            }
            DateTime _dtComment = _currTBL.dtComment.HasValue ? _currTBL.dtComment.Value : DateTime.Now;
            HF_dtComment.Value = _dtComment.JSCal_dateToString();
            drp_dtComment_hour.setSelectedValue(_dtComment.Hour.ToString());
            drp_dtComment_minute.setSelectedValue(_dtComment.Minute.ToString());
            txt_subject.Text = _currTBL.subject;
            txt_body.Text = _currTBL.body;
            drp_estate.setSelectedValue(_currTBL.pid_estate.ToString());
            chk_is_active.Checked = _currTBL.is_active == 1;
            txt_cl_name_full.Text = _currTBL.cl_name_full;
            txt_cl_country.Text = _currTBL.cl_country;
            drp_vote.setSelectedValue(_currTBL.vote.ToString());
            drp_type.setSelectedValue(_currTBL.type);
            drp_pers.setSelectedValue(_currTBL.pers);
            pnlContent.Visible = true;
            RegisterScripts();
        }
        private void FillDataFromControls()
        {
            if (drp_estate.getSelectedValueInt(0)==-1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Selezionare Appartamento');", true);
                RegisterScripts();
                return;
            }
            _currTBL = DC_RENTAL.RNT_TBL_ESTATE_COMMENTs.SingleOrDefault(item => item.id == HF_id.Value.ToInt32());
            if (_currTBL == null)
            {
                _currTBL = new RNT_TBL_ESTATE_COMMENT();
                _currTBL.dtCreation = DateTime.Now;
                DC_RENTAL.RNT_TBL_ESTATE_COMMENTs.InsertOnSubmit(_currTBL);
            }
            DateTime _dtComment = HF_dtComment.Value.JSCal_stringToDate();
            _dtComment = _dtComment.AddHours(drp_dtComment_hour.getSelectedValueInt(0).objToInt32());
            _dtComment = _dtComment.AddMinutes(drp_dtComment_minute.getSelectedValueInt(0).objToInt32());
            _currTBL.dtComment = _dtComment;
            _currTBL.subject = txt_subject.Text;
            _currTBL.body = txt_body.Text;
            _currTBL.pid_estate = drp_estate.getSelectedValueInt(0);
            _currTBL.is_active = chk_is_active.Checked ? 1 : 0;
            _currTBL.cl_name_full = txt_cl_name_full.Text;
            _currTBL.cl_country = txt_cl_country.Text;
            _currTBL.vote = drp_vote.getSelectedValueInt(0);
            _currTBL.type = drp_type.SelectedValue;
            _currTBL.pers = drp_pers.SelectedValue;
            DC_RENTAL.SubmitChanges();
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }
        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            var lbl_id = (Label)LV.Items[e.NewSelectedIndex].FindControl("lbl_id");
            HF_id.Value = lbl_id.Text;
            FillControls();
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void DeleteRecord(object sender, EventArgs args)
        {
            _currTBL = DC_RENTAL.RNT_TBL_ESTATE_COMMENTs.SingleOrDefault(item => item.id == (sender as ImageButton).CommandArgument.ToInt32());
            if (_currTBL != null)
            {
                DC_RENTAL.RNT_TBL_ESTATE_COMMENTs.DeleteOnSubmit(_currTBL);
                DC_RENTAL.SubmitChanges();
                LDS_page_blocks.DataBind();
                LV.SelectedIndex = -1;
                LV.DataBind();
                pnlContent.Visible = false;
            }
        }

        protected void lnk_new_Click(object sender, EventArgs e)
        {
            LV.SelectedIndex = -1;
            LV.DataBind();
            HF_id.Value = "0";
            FillControls();
        }

        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tinyEditor", "setCalendar(" + HF_dtComment.Value + ");setTinyEditor();", true);
        }
    }
}
