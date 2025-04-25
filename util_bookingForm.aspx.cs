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
    public partial class util_bookingForm : mainBasePage
    {
        private RntReservationTMP TMPcurrResTmp;
        public RntReservationTMP currResTmp
        {
            get
            {
                if (TMPcurrResTmp == null)
                    using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                        TMPcurrResTmp = dc.RntReservationTMP.SingleOrDefault(x => x.id == Request.QueryString["tmpresid"].ToInt64());
                if (TMPcurrResTmp == null)
                {
                    Response.Redirect(App.ERROR_PAGE);
                    Response.End();
                }
                return TMPcurrResTmp ?? new RntReservationTMP();
            }
            set { TMPcurrResTmp = value; }
        }
        private RNT_TB_ESTATE tmpEstateTB;
        public RNT_TB_ESTATE currEstateTB
        {
            get
            {
                if (tmpEstateTB == null)
                    tmpEstateTB = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currResTmp.pidEstate);
                return tmpEstateTB ?? new RNT_TB_ESTATE();
            }
        }
        private RNT_LN_ESTATE tmpEstateLN;
        public RNT_LN_ESTATE currEstateLN
        {
            get
            {
                if (tmpEstateLN == null)
                    tmpEstateLN = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == currResTmp.pidEstate && x.pid_lang == App.LangID);
                return tmpEstateLN ?? new RNT_LN_ESTATE();
            }
        }

        private magaRental_DataContext DC_RENTAL;
        private magaInvoice_DataContext DC_INVOICE;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                App.LangID = CurrentLang.ID;
                ErrorLog.addLog("currlang", CurrentLang.ID + "", "");
                ucBooking.IdEstate = currEstateTB.id;
                fillData();
                showMode("new");

                if (affiliatesarea.agentAuth.CurrentID != 0 && affiliatesarea.agentAuth.hasAcceptedContract == 1)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                        if (agentTBL != null)
                        {
                            showMode("view");
                            ltr_country.Text = agentTBL.locCountry;
                            ltr_email.Text = agentTBL.contactEmail;
                            ltr_honorific.Text = agentTBL.nameHonor;
                            ltr_name_full.Text = agentTBL.nameFull;
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "_RNT_validateBook", "$(function() {$(\"#" + txt_email.ClientID + ",#" + txt_email_conf.ClientID + "\").bind(\"cut copy paste\", function(event) { event.preventDefault();}); });", true);
        }

        private void fillData()
        {
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                var tmpTBL = dc.RntReservationTMP.SingleOrDefault(x => x.id == Request.QueryString["tmpresid"].ToInt64());
                if (tmpTBL == null)
                {
                    Response.Redirect("/");
                    return;
                }
                drp_honorific_DataBind();
                drp_country_DataBind();

            }
        }

        protected void drp_honorific_DataBind()
        {
            List<USR_LK_HONORIFIC> _listAll = maga_DataContext.DC_USER.USR_LK_HONORIFICs.ToList();
            List<USR_LK_HONORIFIC> _list = _listAll.Where(x => x.pid_lang == CurrentLang.ID).ToList();
            if (_list.Count() == 0)
                _list = _listAll.Where(x => x.pid_lang == 2).ToList();
            drp_honorific.DataSource = _list;
            drp_honorific.DataTextField = "title";
            drp_honorific.DataValueField = "title";
            drp_honorific.DataBind();
        }

        protected void drp_country_DataBind()
        {
            drp_country.DataSource = authProps.CountryLK.OrderBy(x => x.title);
            drp_country.DataTextField = "title";
            drp_country.DataValueField = "title";
            drp_country.DataBind();
            drp_country.Items.Insert(0, new ListItem("-- " + contUtils.getLabel("lblCountry") + " --", ""));
            if (App.LangID != 2)
            {
                var currCountry = authProps.CountryLK.SingleOrDefault(x => x.code.ToLower() == App.LangCultureName.Substring(3, 2).ToLower());
                if (currCountry != null)
                    drp_country.setSelectedValue(currCountry.title);
            }
        }

        #region for selecting existing client

        protected void showMode(string mode)
        {
            PH_viewClient.Visible = mode == "view";
            PH_oldClient.Visible = mode == "old";
            PH_newClient.Visible = pnl_termsAndPrivacy.Visible = mode == "new";
            PH_pwdRecover.Visible = mode == "pwd";
            PH_bookingForm.Visible = mode != "old" && mode != "pwd";
            HF_mode.Value = mode;
            lbl_errorAlert.Visible = false;
        }

        protected void lnk_goNewClient_Click(object sender, EventArgs e)
        {
            showMode("new");
        }

        protected void lnk_goOldClient_Click(object sender, EventArgs e)
        {
            showMode("old");
        }

        protected bool newClient()
        {
            if (txt_email.Text.Trim() == "")
            {
                lbl_errorAlert.Text = "The Email field cannot be empty.";
                lbl_errorAlert.Visible = true;
                return false;
            }
            if (txt_email.Text.Trim() != txt_email_conf.Text.Trim())
            {
                lbl_errorAlert.Text = "The two E-mail do not match.";
                lbl_errorAlert.Visible = true;
                return false;
            }

            USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.is_deleted != 1 && x.contact_email == txt_email.Text.Trim());
            if (_client != null && _client.is_active == 1)
            {
                lbl_errorAlert.Text = "The Email is already registered Please try to login.";
                showMode("old");
                lbl_errorAlert.Visible = true;
                return false;
            }
            if (_client != null && _client.is_active != 1)
            {
                lbl_errorAlert.Text = "The Email was already registered previously, but was disabled by Administration.<br/>Please Contact us for more information.";
                lbl_errorAlert.Visible = true;
                return false;
            }
            HF_IdClient.Value = "0";
            ltr_country.Text = drp_country.SelectedValue;
            ltr_email.Text = txt_email.Text;
            ltr_honorific.Text = drp_honorific.SelectedItem.Text;
            ltr_name_full.Text = txt_name_first.Text + " " + txt_name_last.Text;
            ltr_phone_mobile.Text = txt_phone_mobile.Text;
            return true;
        }

        public USR_TBL_CLIENT getIdClient()
        {
            USR_TBL_CLIENT _client;
            if (currResTmp.agentID != 0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == currResTmp.agentID);
                    if (agentTBL != null)
                    {
                        _client = new USR_TBL_CLIENT();
                        _client.id = -1;
                        _client.contact_email = agentTBL.contactEmail;
                        _client.name_full = agentTBL.nameCompany;
                        _client.name_honorific = "";
                        _client.loc_country = agentTBL.locCountry;
                        _client.pid_discount = -1;
                        _client.pid_lang = agentTBL.pidLang;
                        _client.isCompleted = 0;
                        return _client;
                    }
                }
            }
            magaUser_DataContext DC_USER = maga_DataContext.DC_USER;
            _client = DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == HF_IdClient.Value.ToInt32());
            if (_client != null) return _client;
            _client = new USR_TBL_CLIENT();
            _client.loc_country = ltr_country.Text;
            _client.contact_email = ltr_email.Text;
            _client.name_honorific = ltr_honorific.Text;
            _client.name_full = ltr_name_full.Text;
            _client.contact_phone_mobile = ltr_phone_mobile.Text;
            _client.loc_country = ltr_country.Text;
            _client.pid_lang = CurrentLang.ID;
            _client.isCompleted = 0;
            _client.is_deleted = 0;
            _client.is_active = 1;
            _client.date_created = DateTime.Now;
            _client.login = _client.contact_email;
            _client.password = CommonUtilities.CreatePassword(8, false, true, false);
            DC_USER.USR_TBL_CLIENTs.InsertOnSubmit(_client);
            DC_USER.SubmitChanges();
            _client.code = _client.id.ToString().fillString("0", 7, false);
            DC_USER.SubmitChanges();
            AdminUtilities.usrClient_mailNewCreation(_client.id); // send mails
            return _client;
        }

        protected void lnk_goPwd_Click(object sender, EventArgs e)
        {
            showMode("pwd");
        }

        protected void lnk_pwdRevover_Click(object sender, EventArgs e)
        {
            if (txt_pwdRecover.Text.Trim() == "")
            {
                lbl_errorAlert.Text = CurrentSource.getSysLangValue("reqRequiredField");
                lbl_errorAlert.Visible = true;
                return;
            }
            showMode("old");
            USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.FirstOrDefault(x => x.is_deleted != 1 && (x.login == txt_pwdRecover.Text.Trim() || x.contact_email == txt_pwdRecover.Text.Trim()));
            if (_client == null)
            {
                lbl_errorAlert.Text = CurrentSource.getSysLangValue("formUserNameOrEmailNotRegistered");
                lbl_errorAlert.Visible = true;
                return;
            }
            if (_client.is_active != 1)
            {
                lbl_errorAlert.Text = "Your Account was disabled. <br/>Please Contact us for more information.";
                lbl_errorAlert.Visible = true;
                return;
            }
            if (AdminUtilities.usrClient_mailPwdRecovery(_client.id))
            {
                lbl_errorAlert.Text = "You will receive an email with your username and password";
                lbl_errorAlert.Visible = true;
            }
        }

        protected void lnk_login_Click(object sender, EventArgs e)
        {
            showMode("view");
            USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.is_deleted != 1 && x.login == txt_old_email.Text.Trim() && x.password == txt_old_password.Text.Trim());
            if (_client == null)
            {
                showMode("old");
                lbl_errorAlert.Text = CurrentSource.getSysLangValue("formWrongUsernameOrPassword");
                lbl_errorAlert.Visible = true;
                return;
            }
            if (_client.is_active != 1)
            {
                showMode("old");
                lbl_errorAlert.Text = "Your Account was disabled. <br/>Please Contact us for more information.";
                lbl_errorAlert.Visible = true;
                return;
            }
            HF_IdClient.Value = _client.id.ToString();
            ltr_country.Text = _client.loc_country;
            ltr_email.Text = _client.contact_email;
            ltr_honorific.Text = _client.name_honorific;
            ltr_name_full.Text = _client.name_full;
            ltr_phone_mobile.Text = _client.contact_phone_mobile;
        }

        #endregion

        protected void lnkBookNow_Click(object sender, EventArgs e)
        {
            saveReservation(true);
        }

        protected void saveReservation(bool payNow)
        {
            bool _isAvailable = DC_RENTAL.RNT_TBL_RESERVATION.Where(y => y.pid_estate == currEstateTB.id //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= currResTmp.dtStart && y.dtEnd.Value.Date >= currResTmp.dtEnd) //
                                                                                || (y.dtStart.Value.Date >= currResTmp.dtStart && y.dtStart.Value.Date < currResTmp.dtEnd) //
                                                                                || (y.dtEnd.Value.Date > currResTmp.dtStart && y.dtEnd.Value.Date <= currResTmp.dtEnd))).Count() == 0;
            if (currEstateTB == null || !_isAvailable)
            {
                Response.Redirect("/");
                return;
            }

            if (HF_mode.Value == "new" && !newClient()) return;
            USR_TBL_CLIENT _client = getIdClient();
            RNT_TBL_RESERVATION _currTBL = rntUtils.newReservation();
            _currTBL.pid_estate = currEstateTB.id;
            _currTBL.pidEstateCity = currEstateTB.pid_city.objToInt32();
            _currTBL.is_deleted = 0;
            _currTBL.pid_creator = 1;
            _currTBL.state_pid = 6;
            _currTBL.state_body = "";
            _currTBL.state_date = DateTime.Now;
            _currTBL.state_pid_user = 1;
            _currTBL.state_subject = "Nuovo dal sito online";
            _currTBL.is_booking = 1;

            _currTBL.dtStart = currResTmp.dtStart;
            _currTBL.dtEnd = currResTmp.dtEnd;
            _currTBL.dtStartTime = "000000";
            _currTBL.dtEndTime = "000000";

            _currTBL.num_adult = currResTmp.numPers_adult;
            _currTBL.num_child_over = currResTmp.numPers_childOver;
            _currTBL.num_child_min = currResTmp.numPers_childMin;

            currResTmp.CopyTo(ref _currTBL);

            _currTBL.pr_deposit = currEstateTB.pr_deposit;
            _currTBL.srs_ext_meetingPoint = currEstateTB.srs_ext_meetingPoint;
            _currTBL.pr_depositWithCard = currEstateTB.pr_depositWithCard;

            //promo discount
            _currTBL.pr_discount_owner = currResTmp.pr_discount_owner;
            _currTBL.pr_discount_commission = currResTmp.pr_discount_commission;
            _currTBL.pr_discount_desc = "-" + currResTmp.pr_discount_desc;
            _currTBL.pr_discount_custom = currResTmp.pr_discount_custom;

            _currTBL.IdAdMedia = App.IdAdMedia;
            _currTBL.IdLink = App.IdLink;

            // salviamo prima e aggiorniamo la cache per evitare overbooking
            DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(_currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);

            // scadenze instant booking
            int _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHoursOnline").ToInt32();
            _currTBL.block_comments = "Scadenza predefinita InstantBooking [" + _blockHours + " ore]";
            if (_currTBL.agentID != 0)
            {
                _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHoursAgent").ToInt32();
                _currTBL.block_comments = "Scadenza predefinita Agenzie [" + _blockHours + " ore]";
            }
            _currTBL.block_expire = DateTime.Now.AddHours(_blockHours);
            _currTBL.block_expire_hours = _blockHours;
            _currTBL.block_pid_user = 1;

            _currTBL.cl_id = _client.id;
            _currTBL.cl_email = _client.contact_email;
            _currTBL.cl_name_full = _client.name_full;
            _currTBL.cl_name_honorific = _client.name_honorific;
            _currTBL.cl_loc_country = _client.loc_country;
            _currTBL.cl_pid_discount = _client.pid_discount;
            _currTBL.cl_pid_lang = _client.pid_lang;
            _currTBL.cl_isCompleted = _client.isCompleted;
            _currTBL.cl_browserIP = Request.browserIP();

            RNT_TBL_REQUEST _request = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == currResTmp.pid_related_request.objToInt32());
            if (_request != null)
            {
                _currTBL.pid_related_request = _request.id;
                _currTBL.pid_operator = _request.pid_operator;
                _request.pid_reservation = _currTBL.id;
                _request.state_body = "";
                _request.state_subject = "Diventato prenotazione";
                _request.state_date = DateTime.Now;
                _request.state_pid = 5;
                _request.state_pid_user = 1;
                List<RNT_TBL_REQUEST> _list = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.pid_related_request == _request.id).ToList();
                foreach (RNT_TBL_REQUEST _req in _list)
                {
                    _req.state_body = _request.state_body;
                    _req.state_subject = _request.state_subject;
                    _req.state_date = _request.state_date;
                    _req.state_pid = _request.state_pid;
                    _req.state_pid_user = _request.state_pid_user;
                    _req.pid_reservation = _request.pid_reservation;
                }
            }
            else
            {
                // abbinare all'account dell'agenzia, in caso di scaccini o maurizio lecce a rental, id= 37
                int pidOperator = 0;
                if (_currTBL.agentID != 0)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == affiliatesarea.agentAuth.CurrentID);
                        if (agentTBL != null)
                        {
                            pidOperator = agentTBL.pidReferer.objToInt32();
                        }
                    }
                }
                if (pidOperator == 15 && pidOperator == 31) pidOperator = 37;
                if (pidOperator == 0) pidOperator = AdminUtilities.usr_getAvailableOperator(AdminUtilities.zone_countryId(_client.loc_country), _currTBL.cl_pid_lang.objToInt32());
                _currTBL.pid_operator = pidOperator;
            }

            _currTBL.code = _currTBL.id.ToString().fillString("0", 7, false);
            _currTBL.password = CommonUtilities.CreatePassword(8, false, true, false);
            rntUtils.rntReservation_setDefaults(ref _currTBL);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(_currTBL);
            rntUtils.rntReservation_mailNewCreation(_currTBL, true, true, true, true, 1); // send mails
            rntUtils.reservation_checkPartPayment(_currTBL, false);
            Response.Redirect(CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currTBL.unique_id);
        }
    }
}