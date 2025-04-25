using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using System.Web.UI.HtmlControls;

namespace RentalInRome.admin
{
    public partial class rnt_request_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_request";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected List<int> EDIT_ITEMS
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_rnt_request_list_EDIT_ITEMS"] != null)
                {
                    return (List<int>)HttpContext.Current.Session["CURRENT_rnt_request_list_EDIT_ITEMS"];
                }
                return new List<int>();
            }
            set
            {
                HttpContext.Current.Session["CURRENT_rnt_request_list_EDIT_ITEMS"] = value;
            }
        }
        protected string CURRENT_FILTER
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_rnt_request_list_FILTER"] != null)
                {
                    return (string)HttpContext.Current.Session["CURRENT_rnt_request_list_FILTER"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["CURRENT_rnt_request_list_FILTER"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                PH_admin.Visible = UserAuthentication.CurrentUserID == 1;
                pnl_flt_account.Visible = UserAuthentication.CurrRoleTBL.rnt_onlyOwnedRequests.objToInt32() == 0;
                if (Request.QueryString.ToString() != "")
                {
                    CURRENT_FILTER = Request.QueryString.ToString();
                }
                else if (CURRENT_FILTER != "")
                {
                    Response.Redirect("rnt_request_list.aspx?" + CURRENT_FILTER);
                }
                EDIT_ITEMS = new List<int>();
                Bind_drp_city();
                Bind_drp_lang();
                Bind_drp_hotel();
                Bind_drp_contract_state();
                Bind_drp_country();
                Bind_drp_account();
                Bind_drp_mailing_days();
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
                {
                    //HF_date_contract_from.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).JSCal_dateToString();
                    // proviamo a mettere na settimana
                    HF_date_contract_from.Value = DateTime.Now.AddDays(-7).JSCal_dateToString();
                }
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
        private void Bind_drp_city()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang== 1).ToList();
            drp_city.Items.Clear();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_city.Items.Add(new ListItem("" + t.title, "" + t.id));
            }
            drp_city.Items.Insert(0, new ListItem("- tutti -", ""));
        }
        protected void Bind_drp_admin(ref DropDownList drp_admin)
        {
            if (_adminList == null)
                _adminList = maga_DataContext.DC_USER.USR_ADMIN.Where(x => (x.rnt_canHaveRequest == 1) && x.is_deleted == 0 && x.is_active == 1).ToList();
            if (_adminCountList == null)
                _adminCountList = new List<temp_admin_count>();
            drp_admin.Items.Clear();
            foreach (USR_ADMIN _utenti in _adminList)
            {
                temp_admin_count _adminCount = _adminCountList.FirstOrDefault(x => x.Id == _utenti.id);
                int _mailCount = 0;
                if (_adminCount == null)
                {
                    _adminCount = new temp_admin_count();
                    if (chk_only_availables.Checked)
                        _adminCount.IsAvv = AdminUtilities.usr_adminIsAvailable(_utenti.id) ? 1 : 0;
                    _adminCount.Id = _utenti.id;
                    _adminCount.Count = AdminUtilities.usr_adminMailCount(_utenti.id, drp_mailing_days.getSelectedValueInt(0).Value);
                    _adminCountList.Add(_adminCount);
                }
                if (chk_only_availables.Checked && _adminCount.IsAvv == 0)
                    continue;
                _mailCount = _adminCount.Count;
                drp_admin.Items.Add(new ListItem("" + _utenti.name + " " + _utenti.surname + "  (" + _mailCount + " mail)", "" + _utenti.id));
            }
            drp_admin.Items.Insert(0, new ListItem("-! non assegnato !-", "0"));
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
        protected void Bind_drp_mailing_days()
        {
            drp_mailing_days.Items.Clear();
            List<USR_LK_MAIL_COUNT> _list = maga_DataContext.DC_USER.USR_LK_MAIL_COUNTs.OrderBy(x => x.mail_count).ToList();
            foreach (USR_LK_MAIL_COUNT _d in _list)
            {
                drp_mailing_days.Items.Add(new ListItem(_d.title, "" + _d.mail_count));
            }
            drp_mailing_days.Items.Add(new ListItem("mese corrente", "-1"));
            drp_mailing_days.Items.Add(new ListItem("- tutti -", "0"));
            drp_mailing_days.setSelectedValue("-1");
        }
        protected void Bind_drp_account()
        {
            drp_account.Items.Clear();
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && (x.rnt_canHaveRequest == 1) && x.is_deleted == 0).OrderBy(x => x.name).ToList();
            foreach (USR_ADMIN _admin in _list)
            {
                drp_account.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
            drp_account.Items.Insert(0, new ListItem("- non assegnati -", "0"));
            drp_account.Items.Insert(0, new ListItem("- tutti -", ""));
        }
        protected void Bind_drp_lang()
        {
            List<CONT_TBL_LANG> _list = maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.Where(x => x.is_active == 1).OrderBy(x => x.title).ToList();
            drp_lang.DataSource = _list;
            drp_lang.DataTextField = "title";
            drp_lang.DataValueField = "id";
            drp_lang.DataBind();
            drp_lang.Items.Insert(0, new ListItem("- tutti -", ""));
        }
        protected void Bind_drp_contract_state()
        {
            //List<LK_CONTRACT_STATE> _list = DC_LK.LK_CONTRACT_STATEs.Where(x => x.type == 1).ToList();
            //drp_contract_state.DataSource = _list;
            //drp_contract_state.DataTextField = "title";
            //drp_contract_state.DataValueField = "id";
            //drp_contract_state.DataBind();
            drp_contract_state.Items.Insert(0, new ListItem("- tutti -", "0"));
        }
        private void Bind_drp_hotel()
        {
            //List<TBL_HOTEL> list = DC_HOTEL.TBL_HOTELs.OrderBy(x => x.code).ToList();
            //drp_hotel.Items.Clear();
            //foreach (TBL_HOTEL t in list)
            //{
            //    if (DC_HOTEL.TBL_HOTEL_CONTRACTs.FirstOrDefault(x => x.pid_hotel == t.id) != null)
            //        drp_hotel.Items.Add(new ListItem("" + t.code, "" + t.id));
            //}
            drp_hotel.Items.Insert(0, new ListItem("- tutti -", "0"));
        }
        protected void SetValuesFromSearch()
        {
            string _str = HF_url_filter.Value;
            if (_str == "") return;
            string _qStr = _str.Split('?')[1];
            string[] _strArr = _qStr.Split('&');
            for (int i = 0; i < _strArr.Length; i++)
            {
                if (_strArr[i].Split('=')[0] == "is_del") drp_is_deleted.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "ids") drp_contract_state.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "creator") drp_flt_pid_creator.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "rel") drp_flt_related.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "city") drp_city.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "lang") drp_lang.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "country") drp_country.setSelectedValue(Server.UrlDecode(_strArr[i].Split('=')[1].ToLower()));
                if (_strArr[i].Split('=')[0] == "account") drp_account.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "tith") txt_code.Text = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "name_full") txt_name_full.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "email") txt_email.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "ip") txt_request_ip.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);

                if (_strArr[i].Split('=')[0] == "dtsf") HF_date_start_from.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "dtst") HF_date_start_to.Value = _strArr[i].Split('=')[1];

                if (_strArr[i].Split('=')[0] == "dtef") HF_date_end_from.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "dtet") HF_date_end_to.Value = _strArr[i].Split('=')[1];

                if (_strArr[i].Split('=')[0] == "dtcf") HF_date_contract_from.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "dtct") HF_date_contract_to.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "idadmedia") txt_IdAdMedia.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "idlink") txt_IdLink.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "conv") drp_conv.setSelectedValue(_strArr[i].Split('=')[1]);
            }
        }
        protected void LoadContent()
        {
            string _filter = "";
            string _sep = "";

            if (drp_conv.getSelectedValueInt() == 0)
            {
                _filter += _sep + "conversionScriptsShown != 1";
                _sep = " and ";
            }
            if (drp_conv.getSelectedValueInt() == 1)
            {
                _filter += _sep + "conversionScriptsShown == 1";
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
            if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedRequests.objToInt32() == 0)
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
            if (drp_flt_pid_creator.SelectedValue != "-1")
            {
                if (drp_flt_pid_creator.SelectedValue == "0")
                    _filter += _sep + "pid_creator != 1";
                else
                    _filter += _sep + "pid_creator == 1";
                _sep = " and ";
            }
            if (drp_flt_related.SelectedValue != "-1")
            {
                if (drp_flt_related.SelectedValue == "0")
                    _filter += _sep + "pid_related_request = 0";
                else
                    _filter += _sep + "pid_related_request != 0";
                _sep = " and ";
            }
            if (drp_lang.SelectedValue != "0" && drp_lang.SelectedValue != "")
            {
                _filter += _sep + "pid_lang = " + drp_lang.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_city.SelectedValue != "0" && drp_city.SelectedValue != "")
            {
                _filter += _sep + "pid_city = " + drp_city.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_country.SelectedValue != "0" && drp_country.SelectedValue != "")
            {
                _filter += _sep + "request_country = \"" + drp_country.SelectedItem.Text + "\"";
                _sep = " and ";
            }
            if (txt_name_full.Text.Trim() != "")
            {
                _filter += _sep + "name_full.Contains(\"" + txt_name_full.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_email.Text.Trim() != "")
            {
                _filter += _sep + "email.Contains(\"" + txt_email.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_request_ip.Text.Trim() != "")
            {
                _filter += _sep + "request_ip.Contains(\"" + txt_request_ip.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_IdAdMedia.Text.Trim() != "")
            {
                _filter += _sep + "IdAdMedia.Contains(\"" + txt_IdAdMedia.Text.Trim() + "\")";
                _sep = " and ";

            }
            if (txt_IdLink.Text.Trim() != "")
            {
                _filter += _sep + "IdLink.Contains(\"" + txt_IdLink.Text.Trim() + "\")";
                _sep = " and ";

            }

            if (HF_date_start_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "request_date_start >= DateTime.Parse(\"" + HF_date_start_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_date_start_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "request_date_start <= DateTime.Parse(\"" + HF_date_start_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }

            if (HF_date_end_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "request_date_end >= DateTime.Parse(\"" + HF_date_end_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_date_end_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "request_date_end <= DateTime.Parse(\"" + HF_date_end_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }


            if (HF_date_contract_from.Value.ToInt32() != 0)
            {
                _filter += _sep + "request_date_created >= DateTime.Parse(\"" + HF_date_contract_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_date_contract_to.Value.ToInt32() != 0)
            {
                _filter += _sep + "request_date_created <= DateTime.Parse(\"" + HF_date_contract_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (_filter == "") _filter = "1=1";
            HF_lds_filter.Value = _filter;
            Fill_LV();

            if (drp_contract_state.SelectedValue != "0")
            {
                _filter += _sep + "state_pid = " + drp_contract_state.SelectedValue + "";
                _sep = " and ";
            }
            if (txt_code.Text.Trim() != "")
            {
                _filter += _sep + "code.Contains(\"" + txt_code.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (drp_hotel.SelectedValue != "0")
            {
                _filter += _sep + "pid_hotel = " + drp_hotel.SelectedValue + "";
                _sep = " and ";
            }
        }
        protected void Fill_LV()
        {
            LDS.Where = HF_lds_filter.Value;
            LDS.DataBind();
            LV.DataBind();
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
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_pid_related_request = e.Item.FindControl("lbl_pid_related_request") as Label;
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_pid_operator = e.Item.FindControl("lbl_pid_operator") as Label;
            Label lbl_related_request = e.Item.FindControl("lbl_related_request") as Label;


            PlaceHolder PH_edit = e.Item.FindControl("PH_edit") as PlaceHolder;
            PlaceHolder PH_view = e.Item.FindControl("PH_view") as PlaceHolder;
            DropDownList drp_admin = e.Item.FindControl("drp_admin") as DropDownList;


            if (lbl_pid_related_request != null && lbl_related_request != null && lbl_id != null && lbl_pid_operator != null && PH_edit != null && PH_view != null && drp_admin != null)
            {
                int pid_operator = lbl_pid_operator.Text.ToInt32();
                int id = lbl_id.Text.ToInt32();
                if (lbl_pid_related_request.Text.ToInt32() != 0)
                {
                    PH_view.Visible = true;
                    PH_edit.Visible = false;
                    ImageButton ibtn_edit = e.Item.FindControl("ibtn_edit") as ImageButton;
                    if (ibtn_edit != null)
                        ibtn_edit.Visible = false;
                    lbl_related_request.Text = "Secondaria di rif. " + lbl_pid_related_request.Text;
                    return;
                }
                string _relatedRequestString = "";
                string _sep = "Primaria di rif. ";
                List<RNT_TBL_REQUEST> _relatedRequestList = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.pid_related_request == id).ToList();
                foreach (RNT_TBL_REQUEST _relatedRequest in _relatedRequestList)
                {
                    _relatedRequestString += _sep + " " + _relatedRequest.id;
                    _sep = ";";
                }
                lbl_related_request.Text = _relatedRequestString != "" ? _relatedRequestString : "Unica";

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
            Label lbl_pid_related_request = e.Item.FindControl("lbl_pid_related_request") as Label;
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_pid_operator = e.Item.FindControl("lbl_pid_operator") as Label;
            PlaceHolder PH_edit = e.Item.FindControl("PH_edit") as PlaceHolder;
            PlaceHolder PH_view = e.Item.FindControl("PH_view") as PlaceHolder;
            DropDownList drp_admin = e.Item.FindControl("drp_admin") as DropDownList;
            List<int> _editItemsT = EDIT_ITEMS;
            if (!_editItemsT.Contains(0))
                _editItemsT.Add(0);
            EDIT_ITEMS = _editItemsT;
            if (lbl_pid_related_request != null && lbl_id != null && lbl_pid_operator != null && PH_edit != null && PH_view != null && drp_admin != null)
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
                        rntUtils.rntRequest_updateOperator(lbl_id.Text.ToInt32(), _newAdmin, true, true, UserAuthentication.CurrentUserID);
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
                    rntUtils.rntRequest_updateOperator(lbl_id.Text.ToInt32(), drp_admin.getSelectedValueInt(0).objToInt32(), true, true, UserAuthentication.CurrentUserID);
                }
            }
        }
        protected void lnk_updateAllOperators_Click(object sender, EventArgs e)
        {
            Update_allOperators();
            Fill_LV();
        }
        protected void drp_mailing_days_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_all_drp_admin();
            Fill_LV();
        }
        protected void chk_only_availables_CheckedChanged(object sender, EventArgs e)
        {
            Bind_all_drp_admin();
            Fill_LV();
        }

    }
}
