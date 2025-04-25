﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class EstateCommentsList : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate_comment";
        }
        protected dbRntEstateCommentsTBL currTBL;
        public class customType
        {
            public int id { get; set; }
            public string code { get; set; }
            public string title { get; set; }
            public customType(int _id, string _code, string _title)
            {
                id = _id;
                code = _code;
                title = _title;
            }
        }
        private List<customType> _types_;
        private List<customType> _types
        {
            get
            {
                if (_types_ == null)
                {
                    _types_ = new List<customType>();
                    _types_.Add(new customType(1, "mail", "E-mail"));
                    _types_.Add(new customType(2, "web", "Web"));
                }
                return _types_;
            }
            set { _types_ = value; }
        }
        private List<customType> _pers_;
        private List<customType> _pers
        {
            get
            {
                if (_pers_ == null)
                {
                    _pers_ = new List<customType>();
                    _pers_.Add(new customType(1, "m", "Uomo"));
                    _pers_.Add(new customType(2, "f", "Donna"));
                    _pers_.Add(new customType(3, "co", "Coppia"));
                    _pers_.Add(new customType(4, "fam", "Famiglia"));
                    _pers_.Add(new customType(5, "gr", "Gruppo"));
                }
                return _pers_;
            }
            set { _pers_ = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                rdp_flt_dtFrom.SelectedDate = DateTime.Now.AddMonths(-1);
                drp_flt_pidCity_DataBind();
                drp_flt_pidOwner_DataBind();
                closeDetails(false);
            }
        }
        protected string getType(string type)
        {
            customType _type = _types.SingleOrDefault(x => x.code == type);
            return _type != null ? _type.title : "";
        }
        protected string getPers(string type)
        {
            customType _per = _pers.SingleOrDefault(x => x.code == type);
            return _per != null ? _per.title : "";
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntEstateCommentsTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                        dc.SaveChanges();
                        AppSettings.RNT_TBL_ESTATE_COMMENTs = null;
                    }
                }
                closeDetails(false);
            }
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            closeDetails(false);
        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            setfilters();
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        private void drp_flt_pidCity_DataBind()
        {
            drp_flt_pidCity.DataSource = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).OrderBy(x => x.title).ToList();
            drp_flt_pidCity.DataTextField = "title";
            drp_flt_pidCity.DataValueField = "id";
            drp_flt_pidCity.DataBind();
            drp_flt_pidCity.Items.Insert(0, new ListItem("- tutti -", "-1"));
        }
        private void drp_flt_pidOwner_DataBind()
        {
            using (magaUser_DataContext dc = maga_DataContext.DC_USER)
            {
                var ownerIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1 && x.pid_owner.HasValue).Select(x => x.pid_owner.Value).Distinct().ToList();
                drp_flt_pidOwner.DataSource = dc.USR_TBL_OWNER.Where(x => x.is_active == 1 && ownerIds.Contains(x.id)).OrderBy(x => x.name_full);
                drp_flt_pidOwner.DataTextField = "name_full";
                drp_flt_pidOwner.DataValueField = "id";
                drp_flt_pidOwner.DataBind();
                drp_flt_pidOwner.Items.Insert(0, new ListItem("- tutti -", "-1"));
            }
        }
        protected void drp_flt_pidCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkList_flt_pidZone_DataBind();
        }
        protected void chkList_flt_pidZone_DataBind()
        {
            var zoneIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1 && x.pid_zone.HasValue).Select(x => x.pid_zone.Value).Distinct().ToList();
            chkList_flt_pidZone.DataSource = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1 && zoneIds.Contains(x.id) && (drp_flt_pidCity.getSelectedValueInt(0) == 0 || x.pid_city == drp_flt_pidCity.getSelectedValueInt(0))).OrderBy(x => x.title).ToList();
            chkList_flt_pidZone.DataTextField = "title";
            chkList_flt_pidZone.DataValueField = "id";
            chkList_flt_pidZone.DataBind();
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            closeDetails(false);
        }
        protected void setfilters()
        {
            var zoneIds = chkList_flt_pidZone.getSelectedValueList().Select(x => (int?)x.ToInt32()).ToList();
            var estateIds = new List<int>();
            string _filter = "";
            string _sep = "";
            if (drp_flt_pidCity.getSelectedValueInt(0) > 0)
            {
                estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1
                     && (drp_flt_pidCity.getSelectedValueInt(0) <= 0 || x.pid_city == drp_flt_pidCity.getSelectedValueInt(0))
                     ).Select(x => x.id).ToList();
                if (estateIds.Count != 0)
                {
                    _filter += _sep + "(";
                    _sep = "";
                    foreach (int _id in estateIds)
                    {
                        _filter += _sep + "pidEstate = " + _id + "";
                        _sep = " or ";
                    }
                    _filter += ")";
                    _sep = " and ";
                }
            }
            if (zoneIds.Count > 0)
            {
                estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1
                        && (zoneIds.Count <= 0 || zoneIds.Contains(x.pid_zone))
                     ).Select(x => x.id).ToList();
                if (estateIds.Count != 0)
                {
                    _filter += _sep + "(";
                    _sep = "";
                    foreach (int _id in estateIds)
                    {
                        _filter += _sep + "pidEstate = " + _id + "";
                        _sep = " or ";
                    }
                    _filter += ")";
                    _sep = " and ";
                }
            }
            if (drp_flt_pidOwner.getSelectedValueInt(0) > 0)
            {
                estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1
                        && (drp_flt_pidOwner.getSelectedValueInt(0) <= 0 || x.pid_owner == drp_flt_pidOwner.getSelectedValueInt(0))
                     ).Select(x => x.id).ToList();
                if (estateIds.Count != 0)
                {
                    _filter += _sep + "(";
                    _sep = "";
                    foreach (int _id in estateIds)
                    {
                        _filter += _sep + "pidEstate = " + _id + "";
                        _sep = " or ";
                    }
                    _filter += ")";
                    _sep = " and ";
                }
            }
            if (txt_flt_code.Text.Trim() != "")
            {
                estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1
                        && (txt_flt_code.Text.Trim() == "" || x.code.ToLower().Contains(txt_flt_code.Text.Trim().ToLower()))
                     ).Select(x => x.id).ToList();
                if (estateIds.Count != 0)
                {
                    _filter += _sep + "(";
                    _sep = "";
                    foreach (int _id in estateIds)
                    {
                        _filter += _sep + "pidEstate = " + _id + "";
                        _sep = " or ";
                    }
                    _filter += ")";
                    _sep = " and ";
                }
            }
            if (rdp_flt_dtFrom.SelectedDate.HasValue)
            {
                _filter += _sep + "dtComment >= DateTime.Parse(\"" + rdp_flt_dtFrom.SelectedDate + "\")";
                _sep = " and ";
            }
            if (rdp_flt_dtTo.SelectedDate.HasValue)
            {
                _filter += _sep + "dtComment <= DateTime.Parse(\"" + rdp_flt_dtTo.SelectedDate + "\")";
                _sep = " and ";
            }
            _filter += _sep + "1 = 1";
            ltrLDSfiltter.Text = _filter;
        }
        protected void lnk_chkListSelectAll_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (sender as LinkButton);
            string arg = lnk.CommandArgument;
            if (arg.Contains("pidZone"))
                foreach (ListItem item in chkList_flt_pidZone.Items)
                    item.Selected = !arg.Contains("deselect");
        }
    }

}