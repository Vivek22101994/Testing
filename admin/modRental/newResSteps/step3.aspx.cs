using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental.newResSteps
{
    public partial class step3 : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        private dbRntReservationTMP TMPcurrTBL;
        protected dbRntReservationTMP currTBL
        {
            get
            {
                if (TMPcurrTBL == null)
                    using (DCmodRental dc = new DCmodRental())
                    {
                        TMPcurrTBL = dc.dbRntReservationTMPs.SingleOrDefault(x => x.id == HF_id.Value.ToInt64());
                        if (TMPcurrTBL == null)
                        {
                            CloseRadWindow("");
                            currTBL = new dbRntReservationTMP();
                            UpdatePanel1.Visible = false;
                        }
                    }
                return TMPcurrTBL;
            }
            set
            {
                TMPcurrTBL = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                HF_id.Value = Request.QueryString["id"].ToInt64() + "";
                var tmpTBL = new dbRntReservationTMP();
                using (DCmodRental dc = new DCmodRental())
                {
                    tmpTBL = dc.dbRntReservationTMPs.SingleOrDefault(x => x.id == HF_id.Value.ToInt64());
                    if (tmpTBL == null)
                    {
                        CloseRadWindow("");
                        currTBL = new dbRntReservationTMP();
                        UpdatePanel1.Visible = false;
                        return;
                    }
                }
                ucPrice.fillData(tmpTBL, 0, false);
            }
        }
        public USR_TBL_CLIENT getIdClient(dbRntReservationTMP tmpTBL)
        {
            USR_TBL_CLIENT _client;
            if (tmpTBL.agentID != 0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == tmpTBL.agentID);
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
            _client = new USR_TBL_CLIENT();
            _client.loc_country = tmpTBL.cl_loc_country;
            _client.contact_email = tmpTBL.cl_email;
            _client.name_honorific = tmpTBL.cl_name_honorific;
            _client.name_full = tmpTBL.cl_name_full;
            _client.contact_phone_mobile = tmpTBL.cl_contact_phone_mobile;
            _client.pid_lang = tmpTBL.cl_pid_lang;
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
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var tmpTBL = dc.dbRntReservationTMPs.SingleOrDefault(x => x.id == HF_id.Value.ToInt64());
                if (tmpTBL == null)
                {
                    CloseRadWindow("");
                    currTBL = new dbRntReservationTMP();
                    UpdatePanel1.Visible = false;
                    return;
                }
                bool _isAvailable = rntUtils.rntEstate_isAvailable(tmpTBL.pidEstate.objToInt32(), tmpTBL.dtStart.Value, tmpTBL.dtEnd.Value, 0) == null;
                var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == tmpTBL.pidEstate);
                if (currEstate == null || !_isAvailable)
                {
                    CloseRadWindow("");
                    currTBL = new dbRntReservationTMP();
                    UpdatePanel1.Visible = false;
                    return;
                }
                var DC_USER = maga_DataContext.DC_USER;
                USR_TBL_CLIENT _client = getIdClient(tmpTBL);

                RNT_TBL_RESERVATION newRes = new RNT_TBL_RESERVATION();
                newRes.unique_id = Guid.NewGuid();
                newRes.uid_2 = Guid.NewGuid();
                newRes.dtCreation = DateTime.Now;
                newRes.is_deleted = 0;
                newRes.pid_creator = UserAuthentication.CurrentUserID;
                if (1 == 2 && (tmpTBL.pr_part_payment_total) == 0)
                {
                    newRes.state_pid = 4;
                    newRes.state_body = "";
                    newRes.state_date = DateTime.Now;
                    newRes.state_pid_user = UserAuthentication.CurrentUserID;
                    newRes.state_subject = "Prenotato";
                }
                else
                {
                    newRes.state_pid = 6;
                    newRes.state_body = "";
                    newRes.state_date = DateTime.Now;
                    newRes.state_pid_user = UserAuthentication.CurrentUserID;
                    newRes.state_subject = "Da conf";
                }
                newRes.is_booking = 1;
                tmpTBL.CopyTo(ref newRes);
                // scadenze instant booking
                int _blockHours = CommonUtilities.getSYS_SETTING("rnt_reservationExpire_defaultHours").ToInt32();
                if (_blockHours == 0)
                {
                    newRes.block_comments = "Nessuna scadenza";
                    newRes.block_expire = null;
                    newRes.block_expire_hours = 0;
                    newRes.block_pid_user = 1;
                }
                else
                {
                    newRes.block_comments = "Scadenza predefinita [" + _blockHours + " ore]";
                    newRes.block_expire = DateTime.Now.AddHours(_blockHours);
                    newRes.block_expire_hours = _blockHours;
                    newRes.block_pid_user = 1;
                }

                newRes.dtStartTime = "000000";
                newRes.dtEndTime = "000000";
                newRes.is_dtStartTimeChanged = 0;
                newRes.is_dtEndTimeChanged = 0;

                newRes.pr_part_modified = tmpTBL.pr_isManual;

                DC_RENTAL.RNT_TBL_RESERVATION.InsertOnSubmit(newRes);
                DC_RENTAL.SubmitChanges();
                //rntUtils.rntReservation_onChange(newRes);
                if (tmpTBL.pidRelatedRequest != 0)
                {
                    RNT_TBL_REQUEST _request = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == tmpTBL.pidRelatedRequest);
                    if (_request != null)
                    {
                        _request.pid_reservation = newRes.id;
                        _request.state_body = "";
                        _request.state_subject = "Diventato prenotazione";
                        _request.state_date = DateTime.Now;
                        _request.state_pid = 5;
                        _request.state_pid_user = UserAuthentication.CurrentUserID;
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
                        newRes.pid_related_request = _request.id;
                        newRes.pid_operator = _request.pid_operator;
                        newRes.IdAdMedia = _request.IdAdMedia;
                        newRes.IdLink = _request.IdLink;
                        //newRes.IdLastOperator = _request.IdLastOperator;
                    }

                }
                newRes.code = newRes.id.ToString().fillString("0", 7, false);
                newRes.password = CommonUtilities.CreatePassword(8, false, true, false);

                newRes.srs_ext_meetingPoint = currEstate.srs_ext_meetingPoint;
                newRes.pr_depositWithCard = currEstate.pr_depositWithCard;

                newRes.cl_id = _client.id;
                newRes.cl_email = _client.contact_email;
                newRes.cl_name_full = _client.name_full;
                newRes.cl_name_honorific = _client.name_honorific;
                newRes.cl_loc_country = _client.loc_country;
                newRes.cl_pid_discount = _client.pid_discount;
                newRes.cl_pid_lang = _client.pid_lang;
                newRes.cl_isCompleted = _client.isCompleted;

                rntUtils.rntReservation_setDefaults(ref newRes);
                DC_RENTAL.SubmitChanges();
                rntUtils.rntReservation_onChange(newRes);
                rntUtils.reservation_checkPartPayment(newRes, true);
                dc.Delete(tmpTBL);
                dc.SaveChanges();
                currTBL = new dbRntReservationTMP();
                UpdatePanel1.Visible = false;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalWindow_closeWithRedirect", "modalWindow_closeWithRedirect('/admin/rnt_reservation_details.aspx?id=" + newRes.id + "');", true);
                //Response.Redirect("/admin/rnt_reservation_details.aspx?id=" + newRes.id);
            }
        }

    }
}