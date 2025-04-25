using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.reservationarea.mobile
{
    public partial class login : mainBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (!Request.Url.AbsoluteUri.StartsWith("http://localhost") && !IsPostBack && Request.Url.AbsoluteUri.StartsWith("http://"))
            {
                string _auth = Request.QueryString["auth"];
                string _qs = string.IsNullOrEmpty(_auth) ? "" : "?auth=" + _auth;
                Response.Redirect(CurrentAppSettings.HOST_SSL + "/reservationarea/mobile/login.aspx" + _qs, true);
                return;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !string.IsNullOrEmpty(Request.QueryString["auth"]))
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
                Response.Redirect("/reservationarea/mobile//login.aspx", true);
            }
            resUtils.currCity = _estTB.pid_city.objToInt32();
            if (res.state_pid != 4)
                Response.Redirect("/reservationarea/mobile/payment.aspx?welcome=true");
            else
                Response.Redirect("/reservationarea/mobile/?welcome=true");
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