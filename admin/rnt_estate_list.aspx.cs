using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_estate_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        private magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                Bind_drp_city();
                Bind_drp_zone();
                Bind_drp_new_zone();
                Bind_drp_owner();
                string _ref = "";
                Uri referrer = HttpContext.Current.Request.Url;
                if (referrer != null)
                {
                    _ref = referrer.OriginalString.ToLower();
                    if (_ref.Contains("flt=true"))
                    {
                        HF_url_filter.Value = _ref;
                        SetValuesFromSearch();
                        if (Request.QueryString["exportExcel"] == "pricelist")
                        {
                            LoadContent();
                            return;
                        }
                    }
                    else
                    {
                        drp_is_active.setSelectedValue("1");
                    }
                }
                string _items = "";
                string _sep = "";
                List<RNT_TB_ESTATE> _estateList = DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_active == 1 && x.is_deleted != 1).OrderBy(x => x.code).ToList();
                foreach (RNT_TB_ESTATE _estate in _estateList)
                {
                    _items += _sep + "{idEstate: \"" + _estate.id + "\", idZone: \"0\",label: \"" + _estate.code.Replace("\"","") + "\",desc: \"\"}";
                    _sep = ",";
                }
                ltr_items.Text = _items;

                _items = "";
                _sep = "";
                foreach (RNT_TB_ESTATE _estate in _estateList)
                {
                    _items += _sep + "{idEstate: \"" + _estate.id + "\", idZone: \"0\",label: \"" + _estate.loc_address.Replace("\"", "") + "\",desc: \"\"}";
                    _sep = ",";
                }
                ltr_itemsAddr.Text = _items;
            }
            else
            {
                if (Request["__EVENTARGUMENT"] == "load_content")
                {
                    LoadContent();
                    UC_rnt_estate_list1.Visible = true;
                    UC_loader_list1.Visible = false;
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setAutocomplete", "setAutocomplete();", true);
        }
        protected void SetValuesFromSearch()
        {
            string _str = HF_url_filter.Value;
            if (_str == "") return;
            string _qStr = _str.Split('?')[1];
            string[] _strArr = _qStr.Split('&');
            for (int i = 0; i < _strArr.Length; i++)
            {
                if (_strArr[i].Split('=')[0] == "idc")
                {
                    drp_city.setSelectedValue(_strArr[i].Split('=')[1]);
                    Bind_drp_zone();
                }
                if (_strArr[i].Split('=')[0] == "idz") drp_zone.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "tith") txt_code.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "addr") txt_address.Text = Server.UrlDecode(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "ido") drp_owner.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "cat") drp_category.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_active") drp_is_active.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_exclusive") drp_is_exclusive.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_srs") drp_is_srs.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_ecopulizie") drp_is_ecopulizie.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_online_booking") drp_is_online_booking.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "is_homeaway") drp_homeAway.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "overnight_tax") drp_pr_has_overnight_tax.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "iscontratto") drp_isContratto.setSelectedValue(_strArr[i].Split('=')[1]);
                if (_strArr[i].Split('=')[0] == "bcom") drp_bcomStatus.setSelectedValue(_strArr[i].Split('=')[1]);
            }
        }
        protected void LoadContent()
        {
            string _filter = "is_deleted=0";
            string _sep = " and ";
            if (txt_code.Text.Trim() != "")
            {
                _filter += _sep + "code.Contains(\"" + txt_code.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (txt_address.Text.Trim() != "")
            {
                _filter += _sep + "loc_address.Contains(\"" + txt_address.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (drp_category.SelectedValue != "")
            {
                _filter += _sep + "category = \"" + drp_category.SelectedValue + "\"";
                _sep = " and ";
            }
            if (drp_is_active.SelectedValue != "-1")
            {
                _filter += _sep + "is_active = " + drp_is_active.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_is_exclusive.SelectedValue != "-1")
            {
                _filter += _sep + "is_exclusive = " + drp_is_exclusive.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_is_srs.SelectedValue != "-1")
            {
                _filter += _sep + "is_srs = " + drp_is_srs.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_is_ecopulizie.SelectedValue != "-1")
            {
                _filter += _sep + "is_ecopulizie = " + drp_is_ecopulizie.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_is_online_booking.SelectedValue != "-1")
            {
                _filter += _sep + "is_online_booking = " + drp_is_online_booking.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_pr_has_overnight_tax.SelectedValue != "-1")
            {
                _filter += _sep + "pr_has_overnight_tax = " + drp_pr_has_overnight_tax.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_isContratto.SelectedValue != "-1")
            {
                _filter += _sep + "isContratto = " + drp_isContratto.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_city.SelectedValue != "0")
            {
                _filter += _sep + "pid_city = " + drp_city.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_zone.SelectedValue != "0")
            {
                _filter += _sep + "pid_zone = " + drp_zone.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_owner.SelectedValue != "0")
            {
                _filter += _sep + "pid_owner = " + drp_owner.SelectedValue + "";
                _sep = " and ";
            }
            if (drp_homeAway.SelectedValue != "-1")
            {
                if (drp_homeAway.SelectedValue == "1")
                {
                    _filter += _sep + "is_HomeAway = " + drp_homeAway.SelectedValue + "";
                    _sep = " and ";
                }
                else if (drp_homeAway.SelectedValue == "0")
                {
                    _filter += _sep + "(is_HomeAway = " + drp_homeAway.SelectedValue + "" + "OR is_HomeAway =  null)" + "";
                    _sep = " and ";
                }


            }
            if (drp_bcomStatus.getSelectedValueInt() >= 0)
            {
                if (drp_bcomStatus.getSelectedValueInt() == 2)
                {
                    _filter += _sep + "bcomEnabled = 1 and bcomHotelId != null and bcomHotelId != \"\" and bcomRoomId != null and bcomRoomId != \"\" ";
                    _sep = " and ";
                }
                else if (drp_bcomStatus.getSelectedValueInt() == 1)
                {
                    _filter += _sep + "bcomEnabled = 1 and (bcomHotelId = null or bcomHotelId = \"\" or bcomRoomId = null or bcomRoomId = \"\")";
                    _sep = " and ";
                }
                else
                {
                    _filter += _sep + "bcomEnabled != 1";
                    _sep = " and ";
                }
            }
            if (_filter == "") _filter = "is_deleted=0";
            if (Request.QueryString["exportExcel"] == "pricelist")
            {
                var ucEstateListPriceExcel1 = (ModRental.admin.modRental.uc.ucEstateListPriceExcel)LoadControl("~/admin/modRental/uc/ucEstateListPriceExcel.ascx");
                ucEstateListPriceExcel1.discount = Request.QueryString["exportExcelDisc"].ToInt32();
                ucEstateListPriceExcel1.FILTER = _filter;
                return;
            }
            UC_rnt_estate_list1.FILTER = _filter;
        }
        protected void drp_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_drp_zone();
        }
        protected void drp_new_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_drp_new_zone();
        }
        private void Bind_drp_city()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_city.Items.Clear();
            drp_new_city.Items.Clear();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_city.Items.Add(new ListItem("" + t.title, "" + t.id));
                drp_new_city.Items.Add(new ListItem("" + t.title, "" + t.id));
            }
            drp_city.Items.Insert(0, new ListItem("- tutti -", "0"));
            drp_new_city.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
        private void Bind_drp_new_zone()
        {
            List<LOC_VIEW_ZONE> list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1 && x.pid_city == drp_new_city.getSelectedValueInt(0)).ToList();
            drp_new_zone.Items.Clear();
            foreach (LOC_VIEW_ZONE t in list)
            {
                drp_new_zone.Items.Add(new ListItem("" + t.title, "" + t.id));
            }
            drp_new_zone.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }
        private void Bind_drp_zone()
        {
            List<LOC_VIEW_ZONE> list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1 && x.pid_city == drp_city.getSelectedValueInt(0)).ToList();
            drp_zone.Items.Clear();
            foreach (LOC_VIEW_ZONE t in list)
            {
                drp_zone.Items.Add(new ListItem("" + t.title, "" + t.id));
            }
            drp_zone.Items.Insert(0, new ListItem("- tutti -", "0"));
        }
        private void Bind_drp_owner()
        {
            List<USR_TBL_OWNER> list = maga_DataContext.DC_USER.USR_TBL_OWNER.Where(x => x.is_active == 1).OrderBy(x => x.name_full).ToList();
            drp_owner.Items.Clear();
            drp_new_owner.Items.Clear();
            foreach (USR_TBL_OWNER t in list)
            {
                drp_owner.Items.Add(new ListItem("" + t.name_full, "" + t.id));
                drp_new_owner.Items.Add(new ListItem("" + t.name_full, "" + t.id));
            }
            drp_owner.Items.Insert(0, new ListItem("- tutti -", "0"));
            drp_new_owner.Items.Insert(0, new ListItem("- seleziona -", "0"));
        }

        protected void lnk_createNew_Click(object sender, EventArgs e)
        {
            if (txt_new_code.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    "errorAlert",
                                                    "alert('inserire il nome della struttura');", true);
                return;
            }

            RNT_TB_ESTATE _currTBL = new RNT_TB_ESTATE();
            _currTBL.code = txt_new_code.Text;
            _currTBL.pid_owner = drp_new_owner.getSelectedValueInt(0);
            _currTBL.pid_city = drp_new_city.getSelectedValueInt(0);
            _currTBL.pid_zone = drp_new_zone.getSelectedValueInt(0);
            _currTBL.ext_ownerdaysinyear = 365;
            _currTBL.category = "APT";
            _currTBL.is_active = 0;
            DC_RENTAL.RNT_TB_ESTATE.InsertOnSubmit(_currTBL);
            DC_RENTAL.SubmitChanges();
            RNT_LN_ESTATE _ln = new RNT_LN_ESTATE();
            _ln.title = _currTBL.code;
            _ln.pid_estate = _currTBL.id;
            _ln.pid_lang = 1;
            DC_RENTAL.RNT_LN_ESTATE.InsertOnSubmit(_ln);
            DC_RENTAL.SubmitChanges();
            Response.Redirect("rnt_estate_details.aspx?id=" + _currTBL.id, true);
        }
    }
}
