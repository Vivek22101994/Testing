using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class usr_owner_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_owner";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected string CURRENT_FILTER
        {
            get
            {
                if (HttpContext.Current.Session["CURRENT_usr_owner_list_FILTER"] != null)
                {
                    return (string)HttpContext.Current.Session["CURRENT_usr_owner_list_FILTER"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["CURRENT_usr_owner_list_FILTER"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                PH_admin.Visible = UserAuthentication.CurrentUserID == 1;
                //pnl_flt_account.Visible = UserAuthentication.isInPermissions((int)UserAuthentication.PERMISSIONS.ManageAllRequests);
                if (Request.QueryString.ToString() != "")
                {
                    CURRENT_FILTER = Request.QueryString.ToString();
                }
                else if (CURRENT_FILTER != "")
                {
                    Response.Redirect("usr_owner_list.aspx?" + CURRENT_FILTER);
                }
                Bind_drp_lang();
                Bind_drp_country();
                Bind_drp_account();
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
        protected void Bind_drp_account()
        {
            drp_account.Items.Clear();
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && (x.rnt_canHaveRequest == 1 || x.rnt_canHaveReservation == 1) && x.is_deleted == 0).OrderBy(x => x.name).ToList();
            foreach (USR_ADMIN _admin in _list)
            {
                drp_account.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
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
        protected void SetValuesFromSearch()
        {
            string _str = HF_url_filter.Value;
            if (_str == "") return;
            string _qStr = _str.Split('?')[1];
            string[] _strArr = _qStr.Split('&');
            for (int i = 0; i < _strArr.Length; i++)
            {
                if (_strArr[i].Split('=')[0] == "is_del") drp_is_deleted.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "lang") drp_lang.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "country") drp_country.setSelectedValue(Server.UrlDecode(_strArr[i].Split('=')[1].ToLower()));
                if (_strArr[i].Split('=')[0] == "account") drp_account.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "name_full") txt_name_full.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "email") txt_email.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);

                if (_strArr[i].Split('=')[0] == "dtcf") HF_date_created_from.Value = _strArr[i].Split('=')[1];
                if (_strArr[i].Split('=')[0] == "dtct") HF_date_created_to.Value = _strArr[i].Split('=')[1];
            }
        }
        protected void LoadContent()
        {
            string _filter = "";
            string _sep = "";
            //if (UserAuthentication.isInPermissions((int)UserAuthentication.PERMISSIONS.ManageAllRequests))
            //{
            //    if (drp_account.SelectedValue != "")
            //    {
            //        _filter += _sep + "pid_operator = " + drp_account.SelectedValue + "";
            //        _sep = " and ";
            //    }
            //}
            //else
            //{
            //    _filter += _sep + "pid_operator = " + UserAuthentication.CurrentUserID + "";
            //    _sep = " and ";
            //}
            //if (drp_lang.SelectedValue != "0" && drp_lang.SelectedValue != "")
            //{
            //    _filter += _sep + "pid_lang = " + drp_lang.SelectedValue + "";
            //    _sep = " and ";
            //}
            if (drp_country.SelectedValue != "0" && drp_country.SelectedValue != "")
            {
                _filter += _sep + "loc_country = \"" + drp_country.SelectedItem.Text + "\"";
                _sep = " and ";
            }
            if (txt_name_full.Text.Trim() != "")
            {
                _filter += _sep + "name_full.Contains(\"" + txt_name_full.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_email.Text.Trim() != "")
            {
                _filter += _sep + "contact_email.Contains(\"" + txt_email.Text.Trim() + "\")";
                _sep = " and ";
            }

            if (HF_date_created_from.Value != "")
            {
                _filter += _sep + "date_created >= DateTime.Parse(\"" + HF_date_created_from.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (HF_date_created_to.Value != "")
            {
                _filter += _sep + "date_created <= DateTime.Parse(\"" + HF_date_created_to.Value.JSCal_stringToDate() + "\")";
                _sep = " and ";
            }
            if (_filter == "") _filter = "1=1";
            HF_lds_filter.Value = _filter;
            Fill_LV();

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
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
        }
    }
}
