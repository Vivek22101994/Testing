using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_reservation_list : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected List<int> EDIT_ITEMS
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_rnt_reservation_list_EDIT_ITEMS"] != null)
                {
                    return (List<int>)HttpContext.Current.Session["CURRENT_rnt_reservation_list_EDIT_ITEMS"];
                }
                return new List<int>();
            }
            set
            {
                HttpContext.Current.Session["CURRENT_rnt_reservation_list_EDIT_ITEMS"] = value;
            }
        }
        protected string CURRENT_FILTER
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_rnt_reservation_list_FILTER"] != null)
                {
                    return (string)HttpContext.Current.Session["CURRENT_rnt_reservation_list_FILTER"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["CURRENT_rnt_reservation_list_FILTER"] = value;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_reservation_list";
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
                    Response.Redirect("rnt_reservation_list.aspx?" + CURRENT_FILTER);
                }
                EDIT_ITEMS = new List<int>();
                Bind_drp_flt_city();
                Bind_lbx_flt_zone();
                Bind_lbx_flt_estate();
                Bind_drp_state_pid();
                Bind_drp_account();
                chkList_flt_problemID_DataBind();
                drp_pidAgent_DataBind();
                LDS.Where = "1=2";
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
            string orderByCurrent = HF_LDS_orderBy.Value;
            foreach (string orderBy in orderByList)
            {
                lnk = LV.FindControl("lnk_orderBy_" + orderBy) as LinkButton;
                if (lnk == null) continue;
                lnk.Text = lnk.Text.Replace(orderAsc, "").Replace(orderDesc, "").Trim();
                if (orderByCurrent.StartsWith(orderBy))
                    lnk.Text = lnk.Text + (orderByCurrent.EndsWith("desc") ? " " + orderDesc : " " + orderAsc);
            }

            chkList_flt_problemID_clientMode_DataBind();
        }
        protected List<USR_ADMIN> _adminList;
        protected List<temp_admin_count> _adminCountList;
        public class temp_admin_count
        {

            private int _id;
            private int _count;
            private int _isAvv;
            public temp_admin_count()
            {
                this._id = 0;
                this._count = 0;
                this._isAvv = 0;
            }
            public temp_admin_count(int Id, int Count, int IsAvv)
            {
                this._id = Id;
                this._count = Count;
                this._isAvv = IsAvv;
            }
            public int Id { get { if (this._id != null)return this._id; else return 0; } set { this._id = value; } }
            public int Count { get { if (this._count != null)return this._count; else return 0; } set { this._count = value; } }
            public int IsAvv { get { if (this._isAvv != null)return this._isAvv; else return 0; } set { this._isAvv = value; } }
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
        protected void Bind_drp_account()
        {
            drp_account.Items.Clear();
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && (x.rnt_canHaveReservation == 1) && x.is_deleted == 0).OrderBy(x => x.name).ToList();
            foreach (USR_ADMIN _admin in _list)
            {
                drp_account.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
            drp_account.Items.Insert(0, new ListItem("- non assegnati -", "0"));
            drp_account.Items.Insert(0, new ListItem("- tutti -", ""));
        }
        protected void Bind_drp_admin(ref DropDownList drp_admin)
        {
            if (_adminList == null)
                _adminList = maga_DataContext.DC_USER.USR_ADMIN.Where(x => (x.rnt_canHaveReservation == 1) && x.is_deleted == 0 && x.is_active == 1).ToList();
            if (_adminCountList == null)
                _adminCountList = new List<temp_admin_count>();
            drp_admin.Items.Clear();
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && (x.rnt_canHaveReservation == 1) && x.is_deleted == 0).OrderBy(x => x.name).ToList();
            foreach (USR_ADMIN _admin in _list)
            {
                drp_admin.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
            drp_admin.Items.Insert(0, new ListItem("-! non assegnato !-", "0"));
            //drp_admin.Items.Clear();
            //foreach (USR_ADMIN _utenti in _adminList)
            //{
            //    temp_admin_count _adminCount = _adminCountList.FirstOrDefault(x => x.Id == _utenti.id);
            //    int _mailCount = 0;
            //    if (_adminCount == null)
            //    {
            //        _adminCount = new temp_admin_count();
            //        if (chk_only_availables.Checked)
            //            _adminCount.IsAvv = AdminUtilities.usr_adminIsAvailable(_utenti.id) ? 1 : 0;
            //        _adminCount.Id = _utenti.id;
            //        _adminCount.Count = AdminUtilities.usr_adminMailCount(_utenti.id, drp_mailing_days.getSelectedValueInt(0).Value);
            //        _adminCountList.Add(_adminCount);
            //    }
            //    if (chk_only_availables.Checked && _adminCount.IsAvv == 0)
            //        continue;
            //    _mailCount = _adminCount.Count;
            //    drp_admin.Items.Add(new ListItem("" + _utenti.name + " " + _utenti.surname + "  (" + _mailCount + " mail)", "" + _utenti.id));
            //}
            //drp_admin.Items.Insert(0, new ListItem("-! non assegnato !-", "0"));
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
        private void Bind_drp_flt_city()
        {
            drp_flt_city.Items.Clear();
            drp_flt_city.Items.Add(new ListItem("-tutti-", "0"));
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_flt_city.Items.Add(new ListItem("" + t.title, "" + t.id));
            }
            Bind_lbx_flt_zone();
        }
        protected void drp_flt_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_zone();
        }
        protected void Bind_lbx_flt_zone()
        {
            List<LOC_VIEW_ZONE> _list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1).OrderBy(x => x.title).ToList();
            if (drp_flt_city.getSelectedValueInt() != 0)
                _list = _list.Where(x => x.pid_city == drp_flt_city.getSelectedValueInt()).ToList();
            lbx_flt_zone.DataSource = _list;
            lbx_flt_zone.DataTextField = "title";
            lbx_flt_zone.DataValueField = "id";
            lbx_flt_zone.DataBind();
        }
        protected void lbx_flt_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_estate();
        }
        protected void drp_flt_isActiveEstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lbx_flt_estate();
        }
        private void Bind_lbx_flt_estate()
        {
            List<int> _zoneIds = lbx_flt_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            List<RNT_TB_ESTATE> _list = AppSettings.RNT_TB_ESTATE.Where(x => x.pid_zone.HasValue && (drp_flt_isActiveEstate.SelectedValue == "-1" || x.is_active == drp_flt_isActiveEstate.getSelectedValueInt()) && x.is_deleted != 1 && _zoneIds.Contains(x.pid_zone.Value)).OrderBy(x => x.code).ToList();
            lbx_flt_estate.DataSource = _list;
            lbx_flt_estate.DataTextField = "code";
            lbx_flt_estate.DataValueField = "id";
            lbx_flt_estate.DataBind();
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
                if (_strArr[i].Split('=')[0] == "city") drp_flt_city.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "srs") drp_is_srs.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "isestact") drp_flt_isActiveEstate.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "est")
                {
                    _estateIds = _strArr[i].Split('=')[1].splitStringToList(",");
                }
                if (_strArr[i].Split('=')[0] == "zone")
                {
                    _zoneIds = _strArr[i].Split('=')[1].splitStringToList(",");
                }
                if (_strArr[i].Split('=')[0] == "account") drp_account.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "code") txt_code.Text = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "name_full") txt_name_full.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "email") txt_email.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "agent") drp_pidAgent.setSelectedValue(_strArr[i].Split('=')[1]);

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
            Bind_lbx_flt_zone();
            lbx_flt_zone.setSelectedValues(_zoneIds);
            Bind_lbx_flt_estate();
            lbx_flt_estate.setSelectedValues(_estateIds);
        }
        protected void LoadContent()
        {
            AppSettings.RNT_TB_ESTATE = null;
            string _filter = "";
            string _sep = "";
            //RNT_TBL_RESERVATION currRes=DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x=>x.pr_srsPrice
            if (drp_iCal.getSelectedValueInt() == 0)
            {
                _filter += _sep + "( (!cl_name_full.Contains(\"iCal_\") and cl_name_full!= \"iCal\") OR (cl_name_full = null) )";
                
                _sep = " and ";
            }
            if (UserAuthentication.CurrentUserID == 1)
            {
                if (drp_is_deleted.SelectedValue != "")
                {
                    _filter += _sep + "is_deleted = " + drp_is_deleted.SelectedValue + "";
                    _sep = " and ";
                }
            }
            else
            {
                _filter += _sep + "is_deleted != 1";
                _sep = " and ";
            }
            if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedReservations.objToInt32() == 0)
            {
                if (drp_account.SelectedValue != "")
                {
                    _filter += _sep + "pid_operator = " + drp_account.SelectedValue + "";
                    _sep = " and ";
                }
            }
            else
            {
                _filter += _sep + "pid_operator = " + UserAuthentication.CurrentUserID + "";
                _sep = " and ";
            }
            if (drp_state_pid.SelectedValue != "0")
            {
                _filter += _sep + "state_pid = " + drp_state_pid.SelectedValue + "";
                _sep = " and ";
            }
            if (txt_code.Text.Trim() != "")
            {
                _filter += _sep + "code.Contains(\"" + txt_code.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (drp_flt_pid_creator.SelectedValue != "-1")
            {
                if (drp_flt_pid_creator.SelectedValue == "0")
                    _filter += _sep + "pid_creator != 1";
                else
                    _filter += _sep + "pid_creator == 1";
                _sep = " and ";
            }
            List<int> _estateIdsActiveNon = new List<int>();
            if (drp_flt_isActiveEstate.SelectedValue != "-1")
                _estateIdsActiveNon = AppSettings.RNT_TB_ESTATE.Where(x => x.is_active == drp_flt_isActiveEstate.getSelectedValueInt() && x.is_deleted != 1).Select(x => x.id).ToList();
            if (_estateIdsActiveNon.Count != 0)
            {
                _filter += _sep + "(";
                _sep = "";
                foreach (int _id in _estateIdsActiveNon)
                {
                    _filter += _sep + "pid_estate = " + _id + "";
                    _sep = " or ";
                }
                _filter += ")";
                _sep = " and ";
            }

            List<int> _estateIds;
            List<int> _zoneIds;
            _zoneIds = lbx_flt_zone.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            _estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.pid_zone.HasValue && (drp_flt_isActiveEstate.SelectedValue == "-1" || x.is_active == drp_flt_isActiveEstate.getSelectedValueInt()) && x.is_deleted != 1 && _zoneIds.Contains(x.pid_zone.Value)).Select(x => x.id).ToList();
            if (_zoneIds.Count != 0 && _estateIds.Count != 0)
            {
                _filter += _sep + "(";
                _sep = "";
                foreach (int _id in _estateIds)
                {
                    _filter += _sep + "pid_estate = " + _id + "";
                    _sep = " or ";
                }
                _filter += ")";
                _sep = " and ";
            }
            _estateIds = lbx_flt_estate.getSelectedValueList().Select(x => x.ToInt32()).ToList();
            if (_estateIds.Count != 0)
            {
                _filter += _sep + "(";
                _sep = "";
                foreach (int _id in _estateIds)
                {
                    _filter += _sep + "pid_estate = " + _id + "";
                    _sep = " or ";
                }
                _filter += ")";
                _sep = " and ";
            }
            if (drp_flt_city.getSelectedValueInt() != 0)
            {
                _estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.pid_city== drp_flt_city.getSelectedValueInt() && x.is_active == drp_flt_isActiveEstate.getSelectedValueInt() && x.is_deleted != 1).Select(x => x.id).ToList();
                _filter += _sep + "(";
                _sep = "";
                foreach (int _id in _estateIds)
                {
                    _filter += _sep + "pid_estate = " + _id + "";
                    _sep = " or ";
                }
                _filter += ")";
                _sep = " and ";
            }
            if (drp_is_srs.getSelectedValueInt() != -1)
            {
                _estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_srs == drp_is_srs.getSelectedValueInt() && x.is_active == drp_flt_isActiveEstate.getSelectedValueInt() && x.is_deleted != 1).Select(x => x.id).ToList();
                _filter += _sep + "(";
                _sep = "";
                foreach (int _id in _estateIds)
                {
                    _filter += _sep + "pid_estate = " + _id + "";
                    _sep = " or ";
                }
                _filter += ")";
                _sep = " and ";
            }
            if (txt_name_full.Text.Trim() != "")
            {
                _filter += _sep + "cl_name_full.Contains(\"" + txt_name_full.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_email.Text.Trim() != "")
            {
                _filter += _sep + "cl_email.Contains(\"" + txt_email.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (HF_dtStart_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtStart >= DateTime.Parse(\"" + HF_dtStart_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_dtStart_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtStart <= DateTime.Parse(\"" + HF_dtStart_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }

            if (HF_dtEnd_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtEnd >= DateTime.Parse(\"" + HF_dtEnd_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_dtEnd_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtEnd <= DateTime.Parse(\"" + HF_dtEnd_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }

            if (HF_dtCreation_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtCreation >= DateTime.Parse(\"" + HF_dtCreation_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_dtCreation_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "dtCreation <= DateTime.Parse(\"" + HF_dtCreation_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }

            if (HF_state_date_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "state_date >= DateTime.Parse(\"" + HF_state_date_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_state_date_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "state_date <= DateTime.Parse(\"" + HF_state_date_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (drp_pidAgent.getSelectedValueInt() != 0)
            {
                if (drp_pidAgent.getSelectedValueInt() == -2)
                    _filter += _sep + "(agentID != null AND agentID > 0)";
                else if (drp_pidAgent.getSelectedValueInt() == -1)
                    _filter += _sep + "(agentID == null OR agentID  = 0)";
                else
                    _filter += _sep + "(agentID = " + drp_pidAgent.getSelectedValueInt() + ")";
                _sep = " and ";
            }

            List<string> problemIDList = chkList_flt_problemID.getSelectedValueList();
            if (problemIDList.Count != 0)
            {
                _filter += _sep + "(";
                _sep = "";
                foreach (string _id in problemIDList)
                {
                    _filter += _sep + "problemID = " + _id + "";
                    _sep = " or ";
                }
                _filter += ")";
                _sep = " and ";
            }
            if (_filter == "") _filter = "1=1";
            
            HF_lds_filter.Value = _filter;
            Fill_LV();
            
        }
        protected void Fill_LV()
        {
            LDS.Where = HF_lds_filter.Value;
            LDS.OrderBy = HF_LDS_orderBy.Value;
           
            LDS.DataBind();
            LV.DataBind();
        }
        protected void LV_OrderBy(string orderBy)
        {
            string orderByCurrent = HF_LDS_orderBy.Value;
            if (orderByCurrent.StartsWith(orderBy))
                HF_LDS_orderBy.Value = orderBy + (orderByCurrent.EndsWith("desc") ? " asc" : " desc");
            else
                HF_LDS_orderBy.Value = orderBy + " desc";
            Fill_LV();
        }

        protected void LV_DataBound(object sender, EventArgs e)
        {
            DataPager _dataPager = LV.FindControl("DataPager1") as DataPager;
            Label _lblCount = LV.FindControl("lbl_record_count") as Label;
            Label _lblCount_top = LV.FindControl("lbl_record_count_top") as Label;
            if (_dataPager != null && _lblCount != null && _lblCount_top != null)
                _lblCount.Text = _lblCount_top.Text = "Totale: " + _dataPager.TotalRowCount;
        }
        protected string GetUserName(string id)
        {
            string _name = CommonUtilities.GetUserName("" + Eval("pid_operator"));
            return _name != "" ? _name : " ";
        }
        protected void Update_allTime()
        {
            foreach (ListViewDataItem _item in LV.Items)
            {
                Label lbl_id = _item.FindControl("lbl_id") as Label;
                Label lbl_pid_operator = _item.FindControl("lbl_pid_operator") as Label;
                DropDownList drp_admin = _item.FindControl("drp_admin") as DropDownList;
                if (lbl_id != null && lbl_pid_operator != null && drp_admin != null)
                {
                    if (drp_admin.getSelectedValueInt(0).objToInt32() == 0)
                        continue;
                    List<int> _editItems = EDIT_ITEMS;
                    if (_editItems.Contains(lbl_id.Text.ToInt32()))
                        _editItems.Remove(lbl_id.Text.ToInt32());
                    EDIT_ITEMS = _editItems;
                    rntUtils.rntRequest_updateOperator(lbl_id.Text.ToInt32(), drp_admin.getSelectedValueInt(0).objToInt32(), true, true, UserAuthentication.CurrentUserID);
                }
            }
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_pid_related_request = e.Item.FindControl("lbl_pid_related_request") as Label;
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_pid_operator = e.Item.FindControl("lbl_pid_operator") as Label;

            PlaceHolder PH_edit = e.Item.FindControl("PH_edit") as PlaceHolder;
            PlaceHolder PH_view = e.Item.FindControl("PH_view") as PlaceHolder;
            DropDownList drp_admin = e.Item.FindControl("drp_admin") as DropDownList;
            if (lbl_pid_related_request != null && lbl_id != null && lbl_pid_operator != null && PH_edit != null && PH_view != null && drp_admin != null)
            {
                int pid_operator = lbl_pid_operator.Text.ToInt32();
                int id = lbl_id.Text.ToInt32();
                ImageButton ibtn_edit = e.Item.FindControl("ibtn_edit") as ImageButton;
                if (ibtn_edit != null)
                    ibtn_edit.Visible = UserAuthentication.CurrUserTbl.rnt_canChangeReservationAccount == 1;
                List<int> _editItems = EDIT_ITEMS;
                bool _isNew = !_editItems.Contains(0);
                if (_isNew)
                {
                    if (pid_operator == 0 && !_editItems.Contains(id))
                        _editItems.Add(id);
                }
                EDIT_ITEMS = _editItems;

                PH_view.Visible = !_editItems.Contains(id);
                PH_edit.Visible = _editItems.Contains(id);
                Bind_drp_admin(ref drp_admin);
                drp_admin.setSelectedValue(pid_operator.ToString());
            }
        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "orderBy")
            {
                LV_OrderBy(e.CommandArgument.ToString());
                return;
            }

            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_pid_operator = e.Item.FindControl("lbl_pid_operator") as Label;
            PlaceHolder PH_edit = e.Item.FindControl("PH_edit") as PlaceHolder;
            PlaceHolder PH_view = e.Item.FindControl("PH_view") as PlaceHolder;
            DropDownList drp_admin = e.Item.FindControl("drp_admin") as DropDownList;
            List<int> _editItemsT = EDIT_ITEMS;
            if (!_editItemsT.Contains(0))
                _editItemsT.Add(0);
            EDIT_ITEMS = _editItemsT;
            if (lbl_id != null && lbl_pid_operator != null && PH_edit != null && PH_view != null && drp_admin != null)
            {
                if (e.CommandName == "edit_operator")
                {
                    PH_view.Visible = false;
                    PH_edit.Visible = true;
                    List<int> _editItems = EDIT_ITEMS;
                    if (!_editItems.Contains(lbl_id.Text.ToInt32()))
                        _editItems.Add(lbl_id.Text.ToInt32());
                    EDIT_ITEMS = _editItems;
                }
                if (e.CommandName == "save_operator")
                {
                    int _newAdmin = drp_admin.getSelectedValueInt(0).objToInt32();
                    if (_newAdmin == 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('seleziona Account!');", true);
                        return;
                    }
                    List<int> _editItems = EDIT_ITEMS;
                    if (_editItems.Contains(lbl_id.Text.ToInt32()))
                        _editItems.Remove(lbl_id.Text.ToInt32());
                    EDIT_ITEMS = _editItems;
                    PH_view.Visible = true;
                    PH_edit.Visible = false;
                    if (_newAdmin != lbl_pid_operator.Text.ToInt32())
                    {
                        rntUtils.rntReservation_updateOperator(lbl_id.Text.ToInt32(), _newAdmin, true, true, UserAuthentication.CurrentUserID);
                    }
                }
                if (e.CommandName == "cancel_operator")
                {
                    PH_view.Visible = true;
                    PH_edit.Visible = false;
                    List<int> _editItems = EDIT_ITEMS;
                    if (_editItems.Contains(lbl_id.Text.ToInt32()))
                        _editItems.Remove(lbl_id.Text.ToInt32());
                    EDIT_ITEMS = _editItems;
                }
                Fill_LV();
            }
        }
        protected void Bind_all_drp_admin()
        {
            foreach (ListViewDataItem _item in LV.Items)
            {
                DropDownList drp_admin = _item.FindControl("drp_admin") as DropDownList;
                if (drp_admin != null)
                    Bind_drp_admin(ref drp_admin);
            }
        }
        protected void Update_allOperators()
        {
            foreach (ListViewDataItem _item in LV.Items)
            {
                Label lbl_id = _item.FindControl("lbl_id") as Label;
                Label lbl_pid_operator = _item.FindControl("lbl_pid_operator") as Label;
                DropDownList drp_admin = _item.FindControl("drp_admin") as DropDownList;
                if (lbl_id != null && lbl_pid_operator != null && drp_admin != null)
                {
                    if (drp_admin.getSelectedValueInt(0).objToInt32() == 0)
                        continue;
                    List<int> _editItems = EDIT_ITEMS;
                    if (_editItems.Contains(lbl_id.Text.ToInt32()))
                        _editItems.Remove(lbl_id.Text.ToInt32());
                    EDIT_ITEMS = _editItems;
                    rntUtils.rntReservation_updateOperator(lbl_id.Text.ToInt32(), drp_admin.getSelectedValueInt(0).objToInt32(), true, true, UserAuthentication.CurrentUserID);
                }
            }
        }
        protected void lnk_updateAllOperators_Click(object sender, EventArgs e)
        {
            Fill_LV();
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
