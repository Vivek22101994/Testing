using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class stp_guestbook : mainBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "stp";
            // errore specifico per i spam
            if (Request.QueryString["id"] == "11,11")
            {
                Response.Redirect(CurrentSource.getPagePath("11", "stp", CurrentLang.ID.ToString()));
                Response.End();
                return;
            }

            int id = Request.QueryString["id"].ToInt32();
            if (id != 0)
                base.PAGE_REF_ID = id;
            else
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "stp_guestbook", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Fill_data();
            }
        }
        protected void Fill_data()
        {
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            CONT_VIEW_STP _stp =
                maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == PAGE_REF_ID && item.pid_lang == CurrentLang.ID);
            if (_stp != null)
            {
                ltr_meta_description.Text = _stp.meta_description;
                ltr_meta_keywords.Text = _stp.meta_keywords;
                ltr_meta_title.Text = _stp.meta_title;
                ltr_title.Text = _stp.title;
                ltr_sub_title.Text = _stp.sub_title;
                ltr_description.Text = _stp.description;
            }
            drp_estate.DataSource = maga_DataContext.DC_RENTAL.RNT_VIEW_ESTATEs.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.pid_lang == CurrentLang.ID && x.pid_city == AppSettings.RNT_currCity).OrderBy(x => x.title);
            drp_estate.DataTextField = "title";
            drp_estate.DataValueField = "id";
            drp_estate.DataBind();
            drp_estate.Items.Insert(0, new ListItem("- - -", "0"));
            drp_estate.setSelectedValue(Request.QueryString["IdEstate"]);
            drp_country.DataBind();

            Guid _unique;
            if (!string.IsNullOrEmpty(Request.QueryString["auth"]) && Guid.TryParse(Request.QueryString["auth"], out _unique))
            {
                List<long> excludeAlreadySent = DC_RENTAL.RNT_RL_RESERVATION_STATEs.Where(x => x.pid_state == 9).Select(x => x.pid_reservation.objToInt64()).Distinct().ToList();
                RNT_TBL_RESERVATION _res = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.unique_id == _unique && x.state_pid == 4);
                if (_res != null && !excludeAlreadySent.Contains(_res.id))
                {
                    HF_pidReservation.Value = _res.id + "";
                    drp_country.setSelectedValue(_res.cl_loc_country);
                    txt_email.Text = _res.cl_email;
                    txt_name_full.Text = _res.cl_name_full;
                    drp_estate.setSelectedValue(_res.pid_estate);
                }
            }
        }
        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem("- - -", ""));
            if (CurrentLang.ID != 2)
            {
                LOC_LK_COUNTRY _c = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.FirstOrDefault(
                        x => x.inner_notes.ToLower() == CurrentLang.NAME.Substring(3, 2).ToLower());
                if (_c != null)
                    drp_country.setSelectedValue(_c.id.ToString());
            }
        }

        protected void lnk_save_Click(object sender, EventArgs e)
        {
            if (!RadCaptcha1.IsValid)
            {
                RadCaptchaCheck.Visible = true;
                txtCaptcha.CssClass = "formErrorState";
                return;
            }
            else
            {
                RadCaptchaCheck.Visible = false;
                txtCaptcha.CssClass = "";
            }
            dbRntEstateCommentsTBL currTBL = new dbRntEstateCommentsTBL();
            currTBL.body = txt_body.Text.htmlEncode();
            currTBL.bodyNegative = txt_bodyNegative.Text.htmlEncode();
            currTBL.cl_country = drp_country.SelectedValue;
            currTBL.cl_email = txt_email.Text;
            currTBL.cl_name_full = txt_name_full.Text;
            currTBL.cl_name_honorific = "";
            currTBL.cl_pid_lang = CurrentLang.ID;
            currTBL.dtComment = DateTime.Now;
            currTBL.dtCreation = DateTime.Now;
            currTBL.isActive = 0;
            currTBL.pidEstate = drp_estate.getSelectedValueInt(0);
            currTBL.pid_user = 1;
            currTBL.subject = "";
            currTBL.type = "web";
            if(rbt_pers_m.Checked)
                currTBL.pers = "m";
            if (rbt_pers_f.Checked)
                currTBL.pers = "f";
            if (rbt_pers_co.Checked)
                currTBL.pers = "co";
            if (rbt_pers_fam.Checked)
                currTBL.pers = "fam";
            if (rbt_pers_gr.Checked)
                currTBL.pers = "gr";

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
            RNT_TBL_RESERVATION _res = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == HF_pidReservation.Value.ToInt64());
            if (_res != null)
            {
                currTBL.pidReservation = _res.id;
                rntUtils.rntReservation_addState(_res.id, 9, 1, "Ha lasciato un commento", "");
            }
            using (DCmodRental dc = new DCmodRental())
            {
                dc.Add(currTBL);
                dc.SaveChanges();
            }
            pnl_cont.Visible = false;
            pnl_sent.Visible = true;
        }
    }
}
