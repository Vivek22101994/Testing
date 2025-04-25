using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{
    public partial class login : mainBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (!Request.Url.AbsoluteUri.ToLower().Contains(".magadesign.net") && !Request.Url.AbsoluteUri.StartsWith("http://localhost") && !Request.Url.AbsoluteUri.ToLower().Contains(".dev.magarental.com") && !IsPostBack && Request.Url.AbsoluteUri.StartsWith("http://"))
            {
                string _auth = Request.QueryString["auth"];
                string _qs = string.IsNullOrEmpty(_auth) ? "" : "?auth=" + _auth;
                Response.Redirect(CurrentAppSettings.HOST_SSL + "/reservationarea/login.aspx" + _qs, true);
                return;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if()
            if (!IsPostBack)
            {
                showMode("new");
                if (!string.IsNullOrEmpty(Request.QueryString["auth"]))
                {
                    try
                    {

                        Guid _unique = new Guid(Request.QueryString["auth"]);
                        RNT_TBL_RESERVATION _res = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.unique_id == _unique && x.state_pid != 2 && x.state_pid != 2);
                        if (_res != null)
                        {
                            if (_res.state_pid == 3)
                            {
                                resUtils.CurrentIdReservation_gl = 0;
                                HF_id.Value = _res.id.ToString();
                                lnk_requestRenewal.Visible = _res.requestRenewal.objToInt32() < 1 && _res.dtCreation.Value.AddHours(96) > DateTime.Now;
                                pnl_RenewalRequested.Visible = _res.requestRenewal.objToInt32() > 0;
                                pnl_loginError.Visible = false;
                                pnl_hasExpired.Visible = true;
                                PH_login.Visible = false;
                                return;
                            }
                            auth(_res);
                        }
                        else
                        {
                            pnl_hasExpired.Visible = false;
                            pnl_loginError.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        pnl_hasExpired.Visible = false;
                        pnl_loginError.Visible = true;
                    }
                }
            }
        }
        protected void auth(RNT_TBL_RESERVATION res)
        {
            CONT_TBL_LANG l = maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.SingleOrDefault(x => x.id == res.cl_pid_lang);
            if (l != null)
            {
                resUtils.CurrentLang_ID = l.id;
                resUtils.CurrentLang_NAME = l.common_name;
                resUtils.CurrentLang_ABBR = l.abbr;
                resUtils.CurrentLang_TITLE = l.lang_title;
            }
            else
            {
                resUtils.CurrentLang_ID = 2;
                resUtils.CurrentLang_NAME = "en-GB";
                resUtils.CurrentLang_ABBR = "eng";
                resUtils.CurrentLang_TITLE = "English";
            }
            resUtils.CurrentIdReservation_gl = res.id;
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == res.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
            }
            resUtils.currCity = _estTB.pid_city.objToInt32();
            if (Session["services"] != null)
            {
                Session["services"] = null;
            }

            if (Request.QueryString["personal"] != null && Request.QueryString["personal"] + "" == "true")
                Response.Redirect("/reservationarea/personaldata.aspx");
            else
                Response.Redirect("/reservationarea");
        }
        protected void lnk_login_Click(object sender, EventArgs e)
        {
            RNT_TBL_RESERVATION _res = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.code == txt_resCode.Text && x.password == txt_resPwd.Text && x.state_pid != 2 && x.state_pid != 2);
            if (_res == null)
            {
                resUtils.CurrentIdReservation_gl = 0;
                pnl_hasExpired.Visible = false;
                pnl_loginError.Visible = true;
                return;
            }
            if (_res.state_pid == 3)
            {
                resUtils.CurrentIdReservation_gl = 0;
                HF_id.Value = _res.id.ToString();
                lnk_requestRenewal.Visible = _res.requestRenewal.objToInt32() < 1 && _res.dtCreation.Value.AddHours(96) > DateTime.Now;
                pnl_RenewalRequested.Visible = _res.requestRenewal.objToInt32() > 0;
                pnl_loginError.Visible = false;
                pnl_hasExpired.Visible = true;
                PH_login.Visible = false;
                return;
            }
            auth(_res);
        }
        protected void lnk_goPwd_Click(object sender, EventArgs e)
        {
            showMode("pwd");
        }
        protected void showMode(string mode)
        {
            // PH_viewClient.Visible = mode == "view";
            if (mode == "old")
            {
                PH_login.Visible = true;
                PH_pwdRecover.Visible = false;
                PH_ForGet.Visible = true;
            }
            if (mode == "new")
            {
                PH_login.Visible = true;
                PH_pwdRecover.Visible = false;
                PH_ForGet.Visible = true;
            }
            if (mode == "pwd")
            {
                PH_ForGet.Visible = false;
                PH_pwdRecover.Visible = true;
            }
            // PH_bookingForm.Visible = mode != "old" && mode != "pwd";
            HF_mode.Value = mode;
            lbl_errorAlert.Visible = false;
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
            RNT_TBL_RESERVATION _client = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.code == txt_pwdRecover.Text);
            //USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.FirstOrDefault(x => x.is_deleted != 1 && (x.login == txt_pwdRecover.Text.Trim() || x.contact_email == txt_pwdRecover.Text.Trim()));
            if (_client == null)
            {
                lbl_errorAlert.Text = CurrentSource.getSysLangValue("formUserNameOrEmailNotRegistered");
                lbl_errorAlert.Visible = true;
                return;
            }
            //if (_client.is_active != 1)
            //{
            //    lbl_errorAlert.Text = "Your Account was disabled. <br/>Please Contact us for more information.";
            //    lbl_errorAlert.Visible = true;
            //    return;
            //}
            //if(AdminUtilities.usrResCode_mailPwdRecovery(_client.id)
            if (AdminUtilities.usrResCode_mailPwdRecovery(_client.id))
            {
                lbl_errorAlert.Text = "You will receive an email with your Reservation Code and password";
                lbl_errorAlert.Visible = true;
            }
        }
        protected void lnk_requestRenewal_Click(object sender, EventArgs e)
        {
            magaRental_DataContext DC_RENTAL = maga_DataContext.DC_RENTAL;
            RNT_TBL_RESERVATION _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == HF_id.Value.ToInt64());
            if (_currTBL == null)
            {
                resUtils.CurrentIdReservation_gl = 0;
                pnl_hasExpired.Visible = false;
                pnl_loginError.Visible = true;
                return;
            }
            if (_currTBL.state_pid == 3)
            {
                _currTBL.requestRenewal = 2;
                _currTBL.requestRenewalDate = DateTime.Now;
                DC_RENTAL.SubmitChanges();
                rntUtils.rntReservation_onChange(_currTBL);
                rntUtils.rntReservation_addState(_currTBL.id, 0, 1, "Richiesta Rinnovo dell'opzione dal Cliente", "");

                resUtils.CurrentIdReservation_gl = 0;
                pnl_loginError.Visible = false;
                pnl_hasExpired.Visible = true;
                lnk_requestRenewal.Visible = false;
                pnl_RenewalRequested.Visible = true;
                PH_login.Visible = false;
                return;
            }
        }
    }
}