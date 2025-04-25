using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class EstateCommentsDett : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate_comment";
        }
        protected dbRntEstateCommentsTBL currTBL;
        protected int CurrID
        {
            get { return HfId.Value.ToInt32(); }
            set { HfId.Value = value.ToString(); }
        }
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
                    _types_ = new List<customType>();
                    _types_.Add(new customType(1, "mail", "E-mail"));
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
            if (!IsPostBack)
            {
                CurrID = Request.QueryString["id"].ToInt32();
                Bind_drp_type();
                Bind_drp_pers();
                Bind_drp_estate();
                fillData();
            }
        }
        protected void Bind_drp_estate()
        {
            List<RNT_TB_ESTATE> _list = AppSettings.RNT_TB_ESTATE.OrderBy(x => x.code).ToList();
            drp_estate.DataSource = _list;
            drp_estate.DataTextField = "code";
            drp_estate.DataValueField = "id";
            drp_estate.DataBind();
            drp_estate.Items.Insert(0, new ListItem("-tutti-", "0"));
            drp_estate.Items.Insert(0, new ListItem("-seleziona-", "-1"));

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
        private void fillData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntEstateCommentsTBLs.SingleOrDefault(x => x.id == CurrID);
                if (currTBL == null)
                {
                    CurrID = 0;
                    ltrTitle.Text = "Inserimento nuovo commento per appartamento";
                    currTBL = new dbRntEstateCommentsTBL();
                }
                else
                    ltrTitle.Text = "Commento per la struttura: " + CurrentSource.rntEstate_code(currTBL.pidEstate.objToInt32(), "- errore -");
                rdtp_dtComment.SelectedDate = currTBL.dtComment;
                txt_subject.Text = currTBL.subject;
                txt_body.Text = currTBL.body;
                txt_bodyNegative.Text = currTBL.bodyNegative;

                drp_estate.setSelectedValue(currTBL.pidEstate.ToString());
                drp_isActive.setSelectedValue(currTBL.isActive);
                drp_isAnonymous.setSelectedValue(currTBL.isAnonymous);
                drp_isVisibleHome.setSelectedValue(currTBL.isVisibleHomePage);
                txt_cl_name_full.Text = currTBL.cl_name_full;
                txt_cl_country.Text = currTBL.cl_country;
                drp_type.setSelectedValue(currTBL.type);
                drp_pers.setSelectedValue(currTBL.pers);

                rbtn_voteStaff_4.Checked = currTBL.voteStaff.objToInt32() > 0 && currTBL.voteStaff.objToInt32() <= 4;
                rbtn_voteStaff_6.Checked = currTBL.voteStaff.objToInt32() > 4 && currTBL.voteStaff.objToInt32() <= 6;
                rbtn_voteStaff_8.Checked = currTBL.voteStaff.objToInt32() > 6 && currTBL.voteStaff.objToInt32() <= 8;
                rbtn_voteStaff_10.Checked = currTBL.voteStaff.objToInt32() > 8 && currTBL.voteStaff.objToInt32() <= 10;

                rbtn_voteService_4.Checked = currTBL.voteService.objToInt32() > 0 && currTBL.voteService.objToInt32() <= 4;
                rbtn_voteService_6.Checked = currTBL.voteService.objToInt32() > 4 && currTBL.voteService.objToInt32() <= 6;
                rbtn_voteService_8.Checked = currTBL.voteService.objToInt32() > 6 && currTBL.voteService.objToInt32() <= 8;
                rbtn_voteService_10.Checked = currTBL.voteService.objToInt32() > 8 && currTBL.voteService.objToInt32() <= 10;

                rbtn_voteCleaning_4.Checked = currTBL.voteCleaning.objToInt32() > 0 && currTBL.voteCleaning.objToInt32() <= 4;
                rbtn_voteCleaning_6.Checked = currTBL.voteCleaning.objToInt32() > 4 && currTBL.voteCleaning.objToInt32() <= 6;
                rbtn_voteCleaning_8.Checked = currTBL.voteCleaning.objToInt32() > 6 && currTBL.voteCleaning.objToInt32() <= 8;
                rbtn_voteCleaning_10.Checked = currTBL.voteCleaning.objToInt32() > 8 && currTBL.voteCleaning.objToInt32() <= 10;

                rbtn_voteComfort_4.Checked = currTBL.voteComfort.objToInt32() > 0 && currTBL.voteComfort.objToInt32() <= 4;
                rbtn_voteComfort_6.Checked = currTBL.voteComfort.objToInt32() > 4 && currTBL.voteComfort.objToInt32() <= 6;
                rbtn_voteComfort_8.Checked = currTBL.voteComfort.objToInt32() > 6 && currTBL.voteComfort.objToInt32() <= 8;
                rbtn_voteComfort_10.Checked = currTBL.voteComfort.objToInt32() > 8 && currTBL.voteComfort.objToInt32() <= 10;

                rbtn_voteQualityPrice_4.Checked = currTBL.voteQualityPrice.objToInt32() > 0 && currTBL.voteQualityPrice.objToInt32() <= 4;
                rbtn_voteQualityPrice_6.Checked = currTBL.voteQualityPrice.objToInt32() > 4 && currTBL.voteQualityPrice.objToInt32() <= 6;
                rbtn_voteQualityPrice_8.Checked = currTBL.voteQualityPrice.objToInt32() > 6 && currTBL.voteQualityPrice.objToInt32() <= 8;
                rbtn_voteQualityPrice_10.Checked = currTBL.voteQualityPrice.objToInt32() > 8 && currTBL.voteQualityPrice.objToInt32() <= 10;

                rbtn_votePosition_4.Checked = currTBL.votePosition.objToInt32() > 0 && currTBL.votePosition.objToInt32() <= 4;
                rbtn_votePosition_6.Checked = currTBL.votePosition.objToInt32() > 4 && currTBL.votePosition.objToInt32() <= 6;
                rbtn_votePosition_8.Checked = currTBL.votePosition.objToInt32() > 6 && currTBL.votePosition.objToInt32() <= 8;
                rbtn_votePosition_10.Checked = currTBL.votePosition.objToInt32() > 8 && currTBL.votePosition.objToInt32() <= 10;
            }
        }
        private void saveData()
        {
            string errorString = "";
            if (drp_estate.getSelectedValueInt(0) == -1)
                errorString += "<br/>-Selezionare Appartamento";
            if (!rdtp_dtComment.SelectedDate.HasValue)
                errorString += "<br/>-Inserire la data del commento";
            if (errorString != "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!" + errorString + "\", 340, 110);", true);
                return;
            }

            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntEstateCommentsTBLs.SingleOrDefault(x => x.id == CurrID);
                if (currTBL == null)
                {
                    currTBL = new dbRntEstateCommentsTBL();
                    currTBL.dtCreation = DateTime.Now;
                    currTBL.pid_user = authUtils.CurrentUserID;
                    dc.Add(currTBL);
                }
                currTBL.dtComment = rdtp_dtComment.SelectedDate;
                currTBL.subject = txt_subject.Text;
                currTBL.body = txt_body.Text;
                currTBL.bodyNegative = txt_bodyNegative.Text;

                currTBL.pidEstate = drp_estate.getSelectedValueInt(0);
                currTBL.isActive = drp_isActive.getSelectedValueInt();
                currTBL.isAnonymous = drp_isAnonymous.getSelectedValueInt();
                currTBL.isVisibleHomePage = drp_isVisibleHome.getSelectedValueInt();
                currTBL.cl_name_full = txt_cl_name_full.Text;
                currTBL.cl_country = txt_cl_country.Text;
                currTBL.type = drp_type.SelectedValue;
                currTBL.pers = drp_pers.SelectedValue;

                if (rbtn_voteStaff_4.Checked) currTBL.voteStaff = 4;
                else if (rbtn_voteStaff_6.Checked) currTBL.voteStaff = 6;
                else if (rbtn_voteStaff_8.Checked) currTBL.voteStaff = 8;
                else if (rbtn_voteStaff_10.Checked) currTBL.voteStaff = 10;

                if (rbtn_voteService_4.Checked) currTBL.voteService = 4;
                else if (rbtn_voteService_6.Checked) currTBL.voteService = 6;
                else if (rbtn_voteService_8.Checked) currTBL.voteService = 8;
                else if (rbtn_voteService_10.Checked) currTBL.voteService = 10;

                if (rbtn_voteCleaning_4.Checked) currTBL.voteCleaning = 4;
                else if (rbtn_voteCleaning_6.Checked) currTBL.voteCleaning = 6;
                else if (rbtn_voteCleaning_8.Checked) currTBL.voteCleaning = 8;
                else if (rbtn_voteCleaning_10.Checked) currTBL.voteCleaning = 10;

                if (rbtn_voteComfort_4.Checked) currTBL.voteComfort = 4;
                else if (rbtn_voteComfort_6.Checked) currTBL.voteComfort = 6;
                else if (rbtn_voteComfort_8.Checked) currTBL.voteComfort = 8;
                else if (rbtn_voteComfort_10.Checked) currTBL.voteComfort = 10;

                if (rbtn_voteQualityPrice_4.Checked) currTBL.voteQualityPrice = 4;
                else if (rbtn_voteQualityPrice_6.Checked) currTBL.voteQualityPrice = 6;
                else if (rbtn_voteQualityPrice_8.Checked) currTBL.voteQualityPrice = 8;
                else if (rbtn_voteQualityPrice_10.Checked) currTBL.voteQualityPrice = 10;

                if (rbtn_votePosition_4.Checked) currTBL.votePosition = 4;
                else if (rbtn_votePosition_6.Checked) currTBL.votePosition = 6;
                else if (rbtn_votePosition_8.Checked) currTBL.votePosition = 8;
                else if (rbtn_votePosition_10.Checked) currTBL.votePosition = 10;

                currTBL.vote = ((currTBL.voteStaff.objToInt32() + currTBL.voteService.objToInt32() + currTBL.voteCleaning.objToInt32() + currTBL.voteComfort.objToInt32() + currTBL.voteQualityPrice.objToInt32() + currTBL.votePosition.objToInt32()) / 6);
                dc.SaveChanges();
                AppSettings.RNT_TBL_ESTATE_COMMENTs = null;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }
}

