using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome
{
    public partial class stp_guestbookNew : mainBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "stp";
            // errore specifico per i spam
            if (Request.QueryString["id"] == "46,46")
            {
                Response.Redirect(CurrentSource.getPagePath("46", "stp", CurrentLang.ID.ToString()));
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

        private void Fill_data()
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
            drp_estate.Items.Insert(0, new ListItem("--" + contUtils.getLabel("reqApartment") + "--", "")); 
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
                    txt_user_email.Text = _res.cl_email;
                    txt_user_full_name.Text = _res.cl_name_full;
                    drp_estate.setSelectedValue(_res.pid_estate);
                }
            }

            txt_user_email.Attributes.Add("placeholder", contUtils.getLabel("reqEmail") + "*");
            txt_user_comment.Attributes.Add("placeholder", contUtils.getLabel("reqMessage") + "*");
            txt_user_full_name.Attributes.Add("placeholder", contUtils.getLabel("reqFullName") + "*");

            bindDrp(ref drp_voteStaff);
            bindDrp(ref drp_voteService);
            bindDrp(ref drp_voteCleaning);
            bindDrp(ref drp_voteQuality);
            bindDrp(ref drp_votePosition);
            bindDrp(ref drp_voteComfort);
            bindDrp(ref drp_UsertType);
        }

        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem("--" + contUtils.getLabel("lblCountry") + "--", ""));
            if (CurrentLang.ID != 2)
            {
                LOC_LK_COUNTRY _c = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.FirstOrDefault(
                        x => x.inner_notes.ToLower() == CurrentLang.NAME.Substring(3, 2).ToLower());
                if (_c != null)
                    drp_country.setSelectedValue(_c.id.ToString());
            }
        }

        protected void bindDrp(ref DropDownList drp)
        {
            drp.Items.Clear();
            if (drp.ID == "drp_UsertType")
            {
                drp.Items.Add(new ListItem(contUtils.getLabel("lblIam"), ""));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblMan"), "m"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblWoman"), "f"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblCouple"), "co"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblFamily"), "fm"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblGroups"), "gr"));
            }
            else
            {

                if (drp.ID == "drp_voteStaff") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForStaff") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_voteService") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForService") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_voteCleaning") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForCleaning") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_voteQuality") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForQualityPrice") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_votePosition") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForPosition") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_voteComfort") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForComfort") + " " + contUtils.getLabel("lblRating"), ""));

                drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingCarente"), "4"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingSufficiente"), "6"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingBuono"), "8"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingOttimo"), "10"));
            }
        }

        protected void lnk_send_Comment_Click(object sender, EventArgs e)
        {
            SaveComment();
        }

        public void SaveComment()
        {
            dbRntEstateCommentsTBL currTBL = new dbRntEstateCommentsTBL();
            currTBL.cl_country = drp_country.SelectedValue;
            currTBL.cl_email = txt_user_email.Text;
            currTBL.cl_name_full = txt_user_full_name.Text;
            currTBL.body = txt_user_comment.Text.htmlEncode();
            currTBL.cl_name_honorific = "";
            currTBL.cl_pid_lang = CurrentLang.ID;
            currTBL.dtComment = DateTime.Now;
            currTBL.dtCreation = DateTime.Now;
            currTBL.isActive = 0;
            currTBL.pidEstate = drp_estate.getSelectedValueInt();
            currTBL.pid_user = 1;
            currTBL.subject = "";
            currTBL.type = "web";
            currTBL.pers = drp_UsertType.SelectedValue;

            currTBL.voteStaff = drp_voteStaff.getSelectedValueInt();
            currTBL.voteService = drp_voteService.getSelectedValueInt();
            currTBL.voteQualityPrice = drp_voteQuality.getSelectedValueInt();
            currTBL.votePosition = drp_votePosition.getSelectedValueInt();

            currTBL.voteComfort = drp_voteComfort.getSelectedValueInt();
            currTBL.voteCleaning = drp_voteCleaning.getSelectedValueInt();

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
            pnl_Comment_cont.Visible = false;
            pnl_Comment_sent.Visible = true;
            txt_user_comment.Text = "";
            txt_user_email.Text = "";
            txt_user_full_name.Text = "";
        }
    }
}