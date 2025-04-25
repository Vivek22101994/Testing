using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_reservation_list_totals : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected string CURRENT_FILTER
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_rnt_reservation_list_totals_FILTER"] != null)
                {
                    return (string)HttpContext.Current.Session["CURRENT_rnt_reservation_list_totals_FILTER"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["CURRENT_rnt_reservation_list_totals_FILTER"] = value;
            }
        }
        private List<RNT_TBL_RESERVATION> CURRENT_LIST;
        private List<RNT_TBL_RESERVATION> CURRENT_LISTGLOBAL;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rep_stats";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                PH_admin.Visible = UserAuthentication.CurrentUserID == 1;
                if (Request.QueryString.ToString() != "")
                {
                    CURRENT_FILTER = Request.QueryString.ToString();
                }
                else if (CURRENT_FILTER != "")
                {
                    Response.Redirect("rnt_reservation_list_totals.aspx?" + CURRENT_FILTER);
                }
                chkList_flt_problemID_DataBind();
                Bind_lbx_flt_zone();
                Bind_lbx_flt_estate();
                Bind_drp_state_pid();
                Bind_drp_country();
                PH_drp_account.Visible = UserAuthentication.CurrRoleTBL.rnt_onlyOwnedReservations.objToInt32() == 0;
                Bind_drp_admin(ref drp_account, false);
                drp_account.Items.Insert(0, new ListItem("- non assegnati -", "0"));
                drp_account.Items.Insert(0, new ListItem("- tutti -", "-1"));
                Bind_drp_admin(ref drp_flt_pid_creator, true);
                drp_flt_pid_creator.Items.Insert(0, new ListItem("- System -", "1"));
                drp_flt_pid_creator.Items.Insert(0, new ListItem("- tutti -", "-1"));
                drp_pidAgent_DataBind();
                string _ref = "";
                Uri referrer = HttpContext.Current.Request.Url;
                if (referrer != null)
                {
                    _ref = referrer.OriginalString.ToLower();
                }
                if (_ref.Contains("flt=true"))
                {
                    HF_url_filter.Value = _ref;
                    SetValuesFromSearch();
                }
                else
                    HF_state_date_from.Value = DateTime.Now.Date.AddDays(-HF_state_date_daysbefore.Value.objToInt32()).JSCal_dateToString(); //new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).JSCal_dateToString();
            }
            else
            {
                if (Request["__EVENTARGUMENT"] == "load_content")
                {
                    LoadContent();
                    UC_loader_list1.Visible = false;
                    LV.Visible = true;
                    pnl_stats.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setCal", "setCal();", true);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            bool useCode = true;
            string orderAsc = useCode ? "&#9650;" : "▲";
            string orderDesc = useCode ? "&#9660;" : "▼";
            LinkButton lnk;
            List<string> orderByList = new List<string>() { "state_date", "dtCreation", "dtStart", "dtEnd", "pr_total", "cl_name_full" };
            //string orderByCurrent = HF_LDS_orderBy.Value;
            //foreach (string orderBy in orderByList)
            //{
            //    lnk = LV.FindControl("lnk_orderBy_" + orderBy) as LinkButton;
            //    if (lnk == null) continue;
            //    lnk.Text = lnk.Text.Replace(orderAsc, "").Replace(orderDesc, "").Trim();
            //    if (orderByCurrent.StartsWith(orderBy))
            //        lnk.Text = lnk.Text + (orderByCurrent.EndsWith("desc") ? " " + orderDesc : " " + orderAsc);
            //}

            chkList_flt_problemID_clientMode_DataBind();
        }
        protected void chkList_flt_problemID_DataBind()
        {
            chkList_flt_problemID.DataSource = rntProps.ProblemTBL.Where(x => x.type == "rnt_res" || x.type == "all").OrderBy(x => x.sequence);
            chkList_flt_problemID.DataTextField = "title";
            chkList_flt_problemID.DataValueField = "id";
            chkList_flt_problemID.DataBind();
        }
        protected void chkList_flt_problemID_clientMode_DataBind()
        {
            string main = "<table border=\"0\" id=\"" + chkList_flt_problemID.ClientID + "\">";
            foreach (ListItem item in chkList_flt_problemID.Items)
            {
                main += "<tr>";
                main += "<td>";
                main += "<input type=\"checkbox\" value=\"" + item.Value + "\" " + (item.Selected ? "checked=\"checked\"" : "") + " id=\"" + chkList_flt_problemID.ClientID + "_" + chkList_flt_problemID.Items.IndexOf(item) + "\">";
                main += "<label for=\"" + chkList_flt_problemID.ClientID + "_" + chkList_flt_problemID.Items.IndexOf(item) + "\">" + item.Text + "</label>";
                main += "</td>";
                main += "</tr>";
            }
            main += "</table>";
            chkList_flt_problemID_clientMode.Text = main;
        }
        protected void lnk_chkListSelectAll_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (sender as LinkButton);
            string arg = lnk.CommandArgument;
            if (arg.Contains("problemID"))
                foreach (ListItem item in chkList_flt_problemID.Items)
                    item.Selected = !arg.Contains("deselect");
        }
        protected List<USR_ADMIN> _adminList;
        protected void Bind_drp_admin(ref DropDownList drp_admin, bool all)
        {
            if (all)
                _adminList = maga_DataContext.DC_USER.USR_ADMIN.Where(x => (x.id > 2) && x.is_deleted != 1 && x.is_active == 1).ToList();
            else
                _adminList = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.rnt_canHaveReservation == 1 && x.is_deleted != 1 && x.is_active == 1).ToList();
            drp_admin.Items.Clear();
            foreach (USR_ADMIN _admin in _adminList)
            {
                drp_admin.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
        }
        protected void Bind_drp_state_pid()
        {
            List<RNT_LK_RESERVATION_STATE> _list = AppSettings.RNT_LK_RESERVATION_STATEs.Where(x => x.type == 1).OrderBy(x => x.title).ToList();
            drp_state_pid.DataSource = _list;
            drp_state_pid.DataTextField = "title";
            drp_state_pid.DataValueField = "id";
            drp_state_pid.DataBind();
            drp_state_pid.Items.Insert(0, new ListItem("- tutti -", "0"));
        }
        protected void Bind_lbx_flt_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1).OrderBy(x => x.title).ToList();
            lbx_flt_zone.DataSource = _list;
            lbx_flt_zone.DataTextField = "title";
            lbx_flt_zone.DataValueField = "id";
            lbx_flt_zone.DataBind();
        }
        protected void lbx_flt_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_estate();
        }
        private void Bind_lbx_flt_estate()
        {
            List<int> _zoneIds = lbx_flt_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            List<RNT_TB_ESTATE> _list = AppSettings.RNT_TB_ESTATE.Where(x => x.pid_zone.HasValue && x.is_active == 1 && x.is_deleted != 1 && _zoneIds.Contains(x.pid_zone.Value)).OrderBy(x => x.code).ToList();
            lbx_flt_estate.DataSource = _list;
            lbx_flt_estate.DataTextField = "code";
            lbx_flt_estate.DataValueField = "id";
            lbx_flt_estate.DataBind();
        }
        protected void Bind_drp_country()
        {
            drp_country.Items.Clear();
            List<LOC_LK_COUNTRY> _list = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.Where(x => x.is_active == 1).OrderBy(x => x.id).ToList();
            foreach (LOC_LK_COUNTRY _country in _list)
            {
                drp_country.Items.Add(new ListItem("" + _country.title, "" + _country.title.ToLower()));
            }
            drp_country.Items.Insert(0, new ListItem("- tutti -", ""));
        }
        private void drp_pidAgent_DataBind()
        {
            var _list = rntProps.AgentTBL.Where(x => x.isActive == 1).OrderBy(x => x.nameCompany).ToList();
            drp_pidAgent.DataSource = _list;
            drp_pidAgent.DataTextField = "nameCompany";
            drp_pidAgent.DataValueField = "id";
            drp_pidAgent.DataBind();
            drp_pidAgent.Items.Insert(0, new ListItem("-tutti Senza Agenzia-", "-1"));
            drp_pidAgent.Items.Insert(0, new ListItem("-tutti Con Agenzia-", "-2"));
            drp_pidAgent.Items.Insert(0, new ListItem("-tutti-", ""));
        }
        protected void SetValuesFromSearch()
        {
            string _str = HF_url_filter.Value;
            if (_str == "") return;
            string _qStr = _str.Split('?')[1];
            string[] _strArr = _qStr.Split('&');
            List<string> _estateIds = new List<string>();
            List<string> _zoneIds = new List<string>();
            for (int i = 0; i < _strArr.Length; i++)
            {
                if (_strArr[i].Split('=')[0] == "is_del") drp_is_deleted.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "state") drp_state_pid.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "creator") drp_flt_pid_creator.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "est")
                {
                    _estateIds = _strArr[i].Split('=')[1].splitStringToList(",");
                }
                if (_strArr[i].Split('=')[0] == "zone")
                {
                    _zoneIds = _strArr[i].Split('=')[1].splitStringToList(",");
                }
                if (_strArr[i].Split('=')[0] == "renewal") drp_renewal.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "payed") drp_payed.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "origin") drp_origin.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "city") drp_city.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "srs") drp_is_srs.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "account") drp_account.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "code") txt_code.Text = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "name_full") txt_name_full.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "email") txt_email.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "country") drp_country.setSelectedValue(Server.UrlDecode(_strArr[i].Split('=')[1].ToLower()));
                if (_strArr[i].Split('=')[0] == "agent") drp_pidAgent.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "idadmedia") txt_IdAdMedia.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "idlink") txt_IdLink.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);

                if (_strArr[i].Split('=')[0] == "dtsf") HF_dtStart_from.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "dtst") HF_dtStart_to.Value = _strArr[i].Split('=')[1];

                if (_strArr[i].Split('=')[0] == "dtef") HF_dtEnd_from.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "dtet") HF_dtEnd_to.Value = _strArr[i].Split('=')[1];

                if (_strArr[i].Split('=')[0] == "dtcf") HF_dtCreation_from.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "dtct") HF_dtCreation_to.Value = _strArr[i].Split('=')[1];

                if (_strArr[i].Split('=')[0] == "dtstf") HF_state_date_from.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "dtstt") HF_state_date_to.Value = _strArr[i].Split('=')[1];

                if (_strArr[i].Split('=')[0] == "problem") chkList_flt_problemID.setSelectedValues(_strArr[i].Split('=')[1].splitStringToList("|"));
                if (_strArr[i].Split('=')[0] == "ical") drp_iCal.setSelectedValue(_strArr[i].Split('=')[1]);
            }
            lbx_flt_zone.setSelectedValues(_zoneIds);
            Bind_lbx_flt_estate();
            lbx_flt_estate.setSelectedValues(_estateIds);
        }
        protected void LoadContent()
        {
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            {
                List<RNT_TBL_RESERVATION> _list = dcOld.RNT_TBL_RESERVATION.ToList();

                if (drp_iCal.getSelectedValueInt() == 0)
                {
                    _list = _list.Where(x => x.cl_name_full != null && !x.cl_name_full.Contains("iCal")).ToList();
                }
                
                if (drp_state_pid.getSelectedValueInt(0) != 0)
                {
                    _list = _list.Where(x => x.state_pid == drp_state_pid.getSelectedValueInt(0)).ToList();
                }
                if (HF_dtStart_from.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.dtStart >= HF_dtStart_from.Value.JSCal_stringToDate()).ToList();
                }
                if (HF_dtStart_to.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.dtStart <= HF_dtStart_to.Value.JSCal_stringToDate()).ToList();
                }

                if (HF_dtEnd_from.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.dtEnd >= HF_dtEnd_from.Value.JSCal_stringToDate()).ToList();
                }
                if (HF_dtEnd_to.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.dtEnd <= HF_dtEnd_to.Value.JSCal_stringToDate()).ToList();
                }

                if (HF_dtCreation_from.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.dtCreation >= HF_dtCreation_from.Value.JSCal_stringToDate()).ToList();
                }
                if (HF_dtCreation_to.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.dtCreation <= HF_dtCreation_to.Value.JSCal_stringToDate()).ToList();
                }

                if (HF_state_date_from.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.state_date >= HF_state_date_from.Value.JSCal_stringToDate()).ToList();
                }
                if (HF_state_date_to.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.state_date <= HF_state_date_to.Value.JSCal_stringToDate()).ToList();
                }
               

                CURRENT_LISTGLOBAL = new List<RNT_TBL_RESERVATION>(_list.Select(x => x.Clone()));
                // fine globale inizio filtrato
                if (UserAuthentication.CurrentUserID == 1)
                {
                    if (drp_is_deleted.getSelectedValueInt(0) != -1)
                    {
                        _list = _list.Where(x => x.is_deleted == drp_is_deleted.getSelectedValueInt(0)).ToList();
                    }
                }
                else
                {
                    _list = _list.Where(x => x.is_deleted != 1).ToList();
                }
                if (drp_origin.getSelectedValueInt(-1) != -1)
                {
                    if (drp_origin.getSelectedValueInt(-1) == 0)
                        _list = _list.Where(x => x.id < 150000).ToList();
                    else
                        _list = _list.Where(x => x.id >= 150000).ToList();
                }
                if (drp_payed.getSelectedValueInt(-1) != -1)
                {
                    if (drp_payed.getSelectedValueInt(-1) == 0)
                        _list = _list.Where(x => !x.payed_total.HasValue || x.payed_total == 0).ToList();
                    else if (drp_payed.getSelectedValueInt(-1) == 2)
                        _list = _list.Where(x => x.payed_total.HasValue && x.payed_total > 0 && x.pr_part_payment_total.HasValue && x.payed_total < x.pr_part_payment_total).ToList();
                    else if (drp_payed.getSelectedValueInt(-1) == 3)
                        _list = _list.Where(x => x.payed_total.HasValue && x.payed_total > 0 && x.pr_total.HasValue && x.payed_total >= x.pr_total).ToList();
                    else
                        _list = _list.Where(x => x.payed_total.HasValue && x.payed_total > 0).ToList();
                }
                if (drp_renewal.SelectedValue != "")
                {
                    if (drp_renewal.SelectedValue == "now")
                        _list = _list.Where(x => x.requestRenewal > 0 && x.state_pid == 3).ToList();
                    else if (drp_renewal.SelectedValue == "story")
                        _list = _list.Where(x => x.requestRenewal == -1).ToList();
                    else if (drp_renewal.SelectedValue == "never")
                        _list = _list.Where(x => !x.requestRenewal.HasValue || x.requestRenewal == 0).ToList();
                }
                if (drp_city.getSelectedValueInt(-1) != -1)
                {
                    _list = _list.Where(x => x.pidEstateCity == drp_city.getSelectedValueInt(0)).ToList();
                }
                if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedReservations.objToInt32() == 0)
                {
                    if (drp_account.getSelectedValueInt(-1) != -1)
                    {
                        _list = _list.Where(x => x.pid_operator == drp_account.getSelectedValueInt(0)).ToList();
                    }
                }
                else
                {
                    _list = _list.Where(x => x.pid_operator == UserAuthentication.CurrentUserID).ToList();
                }
                if (txt_code.Text.Trim() != "")
                {
                    _list = _list.Where(x => x.code != null && x.code.ToLower().Contains(txt_code.Text.ToLower().Trim())).ToList();
                }
                if (drp_flt_pid_creator.SelectedValue != "-1")
                {
                    _list = _list.Where(x => x.pid_creator == drp_flt_pid_creator.getSelectedValueInt(0)).ToList();
                }

                if (drp_pidAgent.getSelectedValueInt() != 0)
                {
                    if (drp_pidAgent.getSelectedValueInt() == -2)
                        _list = _list.Where(x => x.agentID.HasValue && x.agentID > 0).ToList();
                    else if (drp_pidAgent.getSelectedValueInt() == -1)
                        _list = _list.Where(x => !x.agentID.HasValue || x.agentID == 0).ToList();
                    else
                        _list = _list.Where(x => x.agentID == drp_pidAgent.getSelectedValueInt()).ToList();
                }
                if (txt_IdAdMedia.Text.Trim() != "")
                {
                    _list = _list.Where(x => x.IdAdMedia != null && x.IdAdMedia.ToLower() == txt_IdAdMedia.Text.ToLower().Trim()).ToList();
                }
                if (txt_IdLink.Text.Trim() != "")
                {
                    _list = _list.Where(x => x.IdLink != null && x.IdLink.ToLower() == txt_IdLink.Text.ToLower().Trim()).ToList();
                }


                List<int> _estateIds;
                List<int> _zoneIds;
                _zoneIds = lbx_flt_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
                _estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.pid_zone.HasValue && x.is_active == 1 && x.is_deleted != 1 && _zoneIds.Contains(x.pid_zone.Value)).Select(x => x.id).ToList();
                if (_zoneIds.Count != 0 && _estateIds.Count != 0)
                {
                    _list = _list.Where(x => _estateIds.Contains(x.pid_estate.objToInt32())).ToList();
                }
                _estateIds = lbx_flt_estate.getSelectedValueList().Select(x => x.ToInt32()).ToList();
                if (_estateIds.Count != 0)
                {
                    _list = _list.Where(x => _estateIds.Contains(x.pid_estate.objToInt32())).ToList();
                }
                if (drp_is_srs.getSelectedValueInt() != -1)
                {
                    _estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_srs == drp_is_srs.getSelectedValueInt() && x.is_active == 1 && x.is_deleted != 1).Select(x => x.id).ToList();
                    if (_estateIds.Count != 0)
                    {
                        _list = _list.Where(x => _estateIds.Contains(x.pid_estate.objToInt32())).ToList();
                    }
                }
                if (txt_name_full.Text.Trim() != "")
                {
                    _list = _list.Where(x => x.cl_name_full != null && x.cl_name_full.ToLower().Contains(txt_name_full.Text.ToLower().Trim())).ToList();
                }
                if (txt_email.Text.Trim() != "")
                {
                    _list = _list.Where(x => x.cl_email != null && x.cl_email.ToLower().Contains(txt_email.Text.ToLower().Trim())).ToList();
                }
                if (drp_country.SelectedValue != "")
                {
                    _list = _list.Where(x => x.cl_loc_country != null && x.cl_loc_country.ToLower() == drp_country.SelectedValue).ToList();
                }
                List<int> problemIDList = chkList_flt_problemID.getSelectedValueList().Select(x => x.ToInt32()).ToList();
                if (problemIDList.Count != 0)
                {
                    _list = _list.Where(x => x.problemID.HasValue && problemIDList.Contains(x.problemID.objToInt32())).ToList();
                }

                CURRENT_LIST = _list.OrderByDescending(x => x.state_date).ToList();
                Fill_LV();
                Fill_stats();
            }
        }
        protected void Fill_LV()
        {
            LV.DataSource = CURRENT_LIST;
            LV.DataBind();
        }
        protected void Fill_stats()
        {
            decimal _countGlobal_total = CURRENT_LISTGLOBAL.Count;
            txt_countGlobal_total.Text = _countGlobal_total.ToString();
            decimal _prTotalGlobal_total = CURRENT_LISTGLOBAL.Sum(x => x.pr_total.objToDecimal());
            txt_prTotalGlobal_total.Text = _prTotalGlobal_total.ToString("N2");
            decimal _prPartGlobal_total = CURRENT_LISTGLOBAL.Sum(x => x.pr_part_payment_total.objToDecimal());
            txt_prPartGlobal_total.Text = _prPartGlobal_total.ToString("N2");
            decimal _prCommGlobal_total = CURRENT_LISTGLOBAL.Sum(x => x.prTotalCommission.objToDecimal()) + CURRENT_LISTGLOBAL.Sum(x => x.pr_part_agency_fee.objToDecimal());
            txt_prCommGlobal_total.Text = _prCommGlobal_total.ToString("N2");

            decimal _prTotalMediaGlobal_total = _countGlobal_total != 0 ? _prTotalGlobal_total / _countGlobal_total : 0;
            txt_prTotalMediaGlobal_total.Text = _prTotalMediaGlobal_total.ToString("N2");
            decimal _prPartMediaGlobal_total = _countGlobal_total != 0 ? _prPartGlobal_total / _countGlobal_total : 0;
            txt_prPartMediaGlobal_total.Text = _prPartMediaGlobal_total.ToString("N2");
            decimal _prPartMediaPercGlobal_total = _prTotalMediaGlobal_total != 0 ? _prPartMediaGlobal_total / _prTotalMediaGlobal_total * 100 : 0;
            txt_prPartMediaPercGlobal_total.Text = _prPartMediaPercGlobal_total.ToString("N2");
            decimal _prCommMediaGlobal_total = _countGlobal_total != 0 ? _prCommGlobal_total / _countGlobal_total : 0;
            txt_prCommMediaGlobal_total.Text = _prCommMediaGlobal_total.ToString("N2");
            decimal _prCommMediaPercGlobal_total = _prTotalMediaGlobal_total != 0 ? _prCommMediaGlobal_total / _prTotalMediaGlobal_total * 100 : 0;
            txt_prCommMediaPercGlobal_total.Text = _prCommMediaPercGlobal_total.ToString("N2");

            
            decimal _count_total = CURRENT_LIST.Count;
            txt_count_total.Text = _count_total.ToString();
            decimal _prTotal_total = CURRENT_LIST.Sum(x => x.pr_total.objToDecimal());
            txt_prTotal_total.Text = _prTotal_total.ToString("N2");
            decimal _prPart_total = CURRENT_LIST.Sum(x => x.pr_part_payment_total.objToDecimal());
            txt_prPart_total.Text = _prPart_total.ToString("N2");
            decimal _prComm_total = CURRENT_LIST.Sum(x => x.prTotalCommission.objToDecimal()) + CURRENT_LIST.Sum(x => x.pr_part_agency_fee.objToDecimal());
            txt_prComm_total.Text = _prComm_total.ToString("N2");

            decimal _countPerc_total = _countGlobal_total != 0 ? _count_total / _countGlobal_total * 100 : 0;
            txt_countPerc_total.Text = _countPerc_total.ToString("N2");
            decimal _prTotalPerc_total = _prTotalGlobal_total != 0 ? _prTotal_total / _prTotalGlobal_total * 100 : 0;
            txt_prTotalPerc_total.Text = _prTotalPerc_total.ToString("N2");
            decimal _prPartPerc_total = _prPartGlobal_total != 0 ? _prPart_total / _prPartGlobal_total * 100 : 0;
            txt_prPartPerc_total.Text = _prPartPerc_total.ToString("N2");
            decimal _prCommPerc_total = _prCommGlobal_total != 0 ? _prComm_total / _prCommGlobal_total * 100 : 0;
            txt_prCommPerc_total.Text = _prCommPerc_total.ToString("N2");


            decimal _prTotalMedia_total = _count_total != 0 ? _prTotal_total / _count_total : 0;
            txt_prTotalMedia_total.Text = _prTotalMedia_total.ToString("N2");
            decimal _prPartMedia_total = _count_total != 0 ? _prPart_total / _count_total : 0;
            txt_prPartMedia_total.Text = _prPartMedia_total.ToString("N2");
            decimal _prPartMediaPerc_total = _prTotalMedia_total != 0 ? _prPartMedia_total / _prTotalMedia_total * 100 : 0;
            txt_prPartMediaPerc_total.Text = _prPartMediaPerc_total.ToString("N2");
            decimal _prCommMedia_total = _count_total != 0 ? _prComm_total / _count_total : 0;
            txt_prCommMedia_total.Text = _prCommMedia_total.ToString("N2");
            decimal _prCommMediaPerc_total = _prTotalMedia_total != 0 ? _prCommMedia_total / _prTotalMedia_total * 100 : 0;
            txt_prCommMediaPerc_total.Text = _prCommMediaPerc_total.ToString("N2");

            decimal _pr_notPayedTotal = CURRENT_LIST.Where(x => x.payed_total.HasValue && x.payed_total > 0 && x.pr_part_payment_total.HasValue && x.payed_total < x.pr_part_payment_total).Sum(x => (x.pr_part_payment_total.objToDecimal() - x.payed_total.objToDecimal()));
            txt_notPayedTotal.Text = _pr_notPayedTotal.ToString("N2");

            pnl_fltGlobal.Visible = _countGlobal_total != _count_total;

            if (drp_flt_pid_creator.SelectedValue == "-1")
            {
                List<RNT_TBL_RESERVATION> _listSystem = new List<RNT_TBL_RESERVATION>(CURRENT_LIST.Where(x => x.pid_creator == 1).Select(x => x.Clone()));
                decimal _count_total_system = _listSystem.Count;
                pnl_fltSystem.Visible = _count_total_system != 0;
                txt_count_total_system.Text = _count_total_system.ToString();
                decimal _prTotal_total_system = _listSystem.Sum(x => x.pr_total.objToDecimal());
                txt_prTotal_total_system.Text = _prTotal_total_system.ToString("N2");
                decimal _prPart_total_system = _listSystem.Sum(x => x.pr_part_payment_total.objToDecimal());
                txt_prPart_total_system.Text = _prPart_total_system.ToString("N2");
                decimal _prComm_total_system = _listSystem.Sum(x => x.prTotalCommission.objToDecimal()) + _listSystem.Sum(x => x.pr_part_agency_fee.objToDecimal());
                txt_prComm_total_system.Text = _prComm_total_system.ToString("N2");

                decimal _countPerc_total_system = _countGlobal_total != 0 ? _count_total_system / _countGlobal_total * 100 : 0;
                txt_countPerc_total_system.Text = _countPerc_total_system.ToString("N2");
                decimal _prTotalPerc_total_system = _prTotalGlobal_total != 0 ? _prTotal_total_system / _prTotalGlobal_total * 100 : 0;
                txt_prTotalPerc_total_system.Text = _prTotalPerc_total_system.ToString("N2");
                decimal _prPartPerc_total_system = _prPartGlobal_total != 0 ? _prPart_total_system / _prPartGlobal_total * 100 : 0;
                txt_prPartPerc_total_system.Text = _prPartPerc_total_system.ToString("N2");
                decimal _prCommPerc_total_system = _prCommGlobal_total != 0 ? _prComm_total_system / _prCommGlobal_total * 100 : 0;
                txt_prCommPerc_total_system.Text = _prCommPerc_total_system.ToString("N2");

                decimal _countPercFlt_total_system = _count_total != 0 ? _count_total_system / _count_total * 100 : 0;
                txt_countPercFlt_total_system.Text = _countPercFlt_total_system.ToString("N2");
                decimal _prTotalPercFlt_total_system = _prTotal_total != 0 ? _prTotal_total_system / _prTotal_total * 100 : 0;
                txt_prTotalPercFlt_total_system.Text = _prTotalPercFlt_total_system.ToString("N2");
                decimal _prPartPercFlt_total_system = _prPart_total != 0 ? _prPart_total_system / _prPart_total * 100 : 0;
                txt_prPartPercFlt_total_system.Text = _prPartPercFlt_total_system.ToString("N2");
                decimal _prCommPercFlt_total_system = _prComm_total != 0 ? _prComm_total_system / _prComm_total * 100 : 0;
                txt_prCommPercFlt_total_system.Text = _prCommPercFlt_total_system.ToString("N2");

                decimal _prTotalMedia_total_system = _count_total_system != 0 ? _prTotal_total_system / _count_total_system : 0;
                txt_prTotalMedia_total_system.Text = _prTotalMedia_total_system.ToString("N2");
                decimal _prPartMedia_total_system = _count_total_system != 0 ? _prPart_total_system / _count_total_system : 0;
                txt_prPartMedia_total_system.Text = _prPartMedia_total_system.ToString("N2");
                decimal _prPartMediaPerc_total_system = _prTotalMedia_total_system != 0 ? _prPartMedia_total_system / _prTotalMedia_total_system * 100 : 0;
                txt_prPartMediaPerc_total_system.Text = _prPartMediaPerc_total_system.ToString("N2");
                decimal _prCommMedia_total_system = _count_total_system != 0 ? _prComm_total_system / _count_total_system : 0;
                txt_prCommMedia_total_system.Text = _prCommMedia_total_system.ToString("N2");
                decimal _prCommMediaPerc_total_system = _prTotalMedia_total_system != 0 ? _prCommMedia_total_system / _prTotalMedia_total_system * 100 : 0;
                txt_prCommMediaPerc_total_system.Text = _prCommMediaPerc_total_system.ToString("N2");

                List<RNT_TBL_RESERVATION> _listNonSystem = new List<RNT_TBL_RESERVATION>(CURRENT_LIST.Where(x => x.pid_creator != 1).Select(x => x.Clone()));
                decimal _count_total_nonsystem = _listNonSystem.Count;
                pnl_fltNonSystem.Visible = _count_total_nonsystem != 0;
                txt_count_total_nonsystem.Text = _count_total_nonsystem.ToString();
                decimal _prTotal_total_nonsystem = _listNonSystem.Sum(x => x.pr_total.objToDecimal());
                txt_prTotal_total_nonsystem.Text = _prTotal_total_nonsystem.ToString("N2");
                decimal _prPart_total_nonsystem = _listNonSystem.Sum(x => x.pr_part_payment_total.objToDecimal());
                txt_prPart_total_nonsystem.Text = _prPart_total_nonsystem.ToString("N2");
                decimal _prComm_total_nonsystem = _listNonSystem.Sum(x => x.prTotalCommission.objToDecimal()) + _listNonSystem.Sum(x => x.pr_part_agency_fee.objToDecimal());
                txt_prComm_total_nonsystem.Text = _prComm_total_nonsystem.ToString("N2");

                decimal _countPerc_total_nonsystem = _countGlobal_total != 0 ? _count_total_nonsystem / _countGlobal_total * 100 : 0;
                txt_countPerc_total_nonsystem.Text = _countPerc_total_nonsystem.ToString("N2");
                decimal _prTotalPerc_total_nonsystem = _prTotalGlobal_total != 0 ? _prTotal_total_nonsystem / _prTotalGlobal_total * 100 : 0;
                txt_prTotalPerc_total_nonsystem.Text = _prTotalPerc_total_nonsystem.ToString("N2");
                decimal _prPartPerc_total_nonsystem = _prPartGlobal_total != 0 ? _prPart_total_nonsystem / _prPartGlobal_total * 100 : 0;
                txt_prPartPerc_total_nonsystem.Text = _prPartPerc_total_nonsystem.ToString("N2");
                decimal _prCommPerc_total_nonsystem = _prCommGlobal_total != 0 ? _prComm_total_nonsystem / _prCommGlobal_total * 100 : 0;
                txt_prCommPerc_total_nonsystem.Text = _prCommPerc_total_nonsystem.ToString("N2");

                decimal _countPercFlt_total_nonsystem = _count_total != 0 ? _count_total_nonsystem / _count_total * 100 : 0;
                txt_countPercFlt_total_nonsystem.Text = _countPercFlt_total_nonsystem.ToString("N2");
                decimal _prTotalPercFlt_total_nonsystem = _prTotal_total != 0 ? _prTotal_total_nonsystem / _prTotal_total * 100 : 0;
                txt_prTotalPercFlt_total_nonsystem.Text = _prTotalPercFlt_total_nonsystem.ToString("N2");
                decimal _prPartPercFlt_total_nonsystem = _prPart_total != 0 ? _prPart_total_nonsystem / _prPart_total * 100 : 0;
                txt_prPartPercFlt_total_nonsystem.Text = _prPartPercFlt_total_nonsystem.ToString("N2");
                decimal _prCommPercFlt_total_nonsystem = _prComm_total != 0 ? _prComm_total_nonsystem / _prComm_total * 100 : 0;
                txt_prCommPercFlt_total_nonsystem.Text = _prCommPercFlt_total_nonsystem.ToString("N2");

                decimal _prTotalMedia_total_nonsystem = _count_total_nonsystem != 0 ? _prTotal_total_nonsystem / _count_total_nonsystem : 0;
                txt_prTotalMedia_total_nonsystem.Text = _prTotalMedia_total_nonsystem.ToString("N2");
                decimal _prPartMedia_total_nonsystem = _count_total_nonsystem != 0 ? _prPart_total_nonsystem / _count_total_nonsystem : 0;
                txt_prPartMedia_total_nonsystem.Text = _prPartMedia_total_nonsystem.ToString("N2");
                decimal _prPartMediaPerc_total_nonsystem = _prTotalMedia_total_nonsystem != 0 ? _prPartMedia_total_nonsystem / _prTotalMedia_total_nonsystem * 100 : 0;
                txt_prPartMediaPerc_total_nonsystem.Text = _prPartMediaPerc_total_nonsystem.ToString("N2");
                decimal _prCommMedia_total_nonsystem = _count_total_nonsystem != 0 ? _prComm_total_nonsystem / _count_total_nonsystem : 0;
                txt_prCommMedia_total_nonsystem.Text = _prCommMedia_total_nonsystem.ToString("N2");
                decimal _prCommMediaPerc_total_nonsystem = _prTotalMedia_total_nonsystem != 0 ? _prCommMedia_total_nonsystem / _prTotalMedia_total_nonsystem * 100 : 0;
                txt_prCommMediaPerc_total_nonsystem.Text = _prCommMediaPerc_total_nonsystem.ToString("N2");

            }
            else
            {
                pnl_fltSystem.Visible = false;
            }


            if (drp_state_pid.getSelectedValueInt(0) == 0)
                LV_states.DataSource = AppSettings.RNT_LK_RESERVATION_STATEs.Where(x => x.type == 1).OrderBy(x => x.title).ToList();
            else
                LV_states.DataSource = new List<RNT_LK_RESERVATION_STATE>();
            LV_states.DataBind();
            foreach (ListViewDataItem _item in LV_states.Items)
            {
                Label lbl_id = _item.FindControl("lbl_id") as Label;
                TextBox txt_count = _item.FindControl("txt_count") as TextBox;
                TextBox txt_count_perc = _item.FindControl("txt_count_perc") as TextBox;
                TextBox txt_prTotal = _item.FindControl("txt_prTotal") as TextBox;
                TextBox txt_prTotal_perc = _item.FindControl("txt_prTotal_perc") as TextBox;
                TextBox txt_prTotalMedia = _item.FindControl("txt_prTotalMedia") as TextBox;
                TextBox txt_prPart = _item.FindControl("txt_prPart") as TextBox;
                TextBox txt_prPart_perc = _item.FindControl("txt_prPart_perc") as TextBox;
                TextBox txt_prPartMedia = _item.FindControl("txt_prPartMedia") as TextBox;
                TextBox txt_prComm = _item.FindControl("txt_prComm") as TextBox;
                TextBox txt_prComm_perc = _item.FindControl("txt_prComm_perc") as TextBox;
                TextBox txt_prCommMedia = _item.FindControl("txt_prCommMedia") as TextBox;
                if (lbl_id == null || txt_count == null || txt_count_perc == null || txt_prTotal == null || txt_prTotal_perc == null || txt_prTotalMedia == null || txt_prPart == null || txt_prPart_perc == null || txt_prPartMedia == null)
                    continue;
                List<RNT_TBL_RESERVATION> _list = CURRENT_LIST.Where(x => x.state_pid == lbl_id.Text.ToInt32()).ToList();
                decimal _count = _list.Count();
                txt_count.Text = _count.ToString();
                decimal _count_perc = _count_total != 0 ? _count / _count_total * 100:0;
                txt_count_perc.Text = _count_perc.ToString("N2");

                decimal _prTotal = _list.Sum(x => x.pr_total.objToDecimal());
                txt_prTotal.Text = _prTotal.ToString("N2");
                decimal _prTotal_perc = _prPart_total != 0 ? _prTotal / _prTotal_total * 100 : 0;
                txt_prTotal_perc.Text = _prTotal_perc.ToString("N2");
                decimal _prTotalMedia = _count != 0 ? _prTotal / _count : 0;
                txt_prTotalMedia.Text = _prTotalMedia.ToString("N2");

                decimal _prPart = _list.Sum(x => x.pr_part_payment_total.objToDecimal());
                txt_prPart.Text = _prPart.ToString("N2");
                decimal _prPart_perc = _prPart_total != 0 ? _prPart / _prPart_total * 100 : 0;
                txt_prPart_perc.Text = _prPart_perc.ToString("N2");
                decimal _prPartMedia = _count != 0 ? _prPart / _count : 0;
                txt_prPartMedia.Text = _prPartMedia.ToString("N2");

                decimal _prComm = _list.Sum(x => x.prTotalCommission.objToDecimal()) + _list.Sum(x => x.pr_part_agency_fee.objToDecimal());
                txt_prComm.Text = _prComm.ToString("N2");
                decimal _prComm_perc = _prComm_total != 0 ? _prComm / _prComm_total * 100 : 0;
                txt_prComm_perc.Text = _prComm_perc.ToString("N2");
                decimal _prCommMedia = _count != 0 ? _prComm / _count : 0;
                txt_prCommMedia.Text = _prCommMedia.ToString("N2");
            }
        }
        protected void LV_DataBound(object sender, EventArgs e)
        {
            DataPager _dataPager = LV.FindControl("DataPager1") as DataPager;
            Label _lblCount = LV.FindControl("lbl_record_count") as Label;
            Label _lblCount_top = LV.FindControl("lbl_record_count_top") as Label;
            if (_dataPager != null && _lblCount != null && _lblCount_top != null)
                _lblCount.Text = _lblCount_top.Text = "Totale: " + _dataPager.TotalRowCount;
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;

        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (lbl_id == null)
                return;
            if (e.CommandName == "edit_operator")
            {
            }
        }
        protected void drp_mailing_days_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }
        protected void chk_only_availables_CheckedChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }


    }
}
