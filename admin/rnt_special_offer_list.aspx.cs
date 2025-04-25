using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_special_offer_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_special_offer";
        }
        private magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                if (Request.QueryString["id"].ToInt32() > 0 && Request.QueryString["sethome"] == "true")
                {
                    var currTBL = DC_RENTAL.RNT_TB_SPECIAL_OFFER.SingleOrDefault(item => item.id == Request.QueryString["id"].ToInt32());
                    if (currTBL != null)
                    {
                        currTBL.img_thumb = currTBL.img_thumb == "1" ? "0" : "1";
                        DC_RENTAL.SubmitChanges();
                        AppSettings.RNT_TB_SPECIAL_OFFERs = DC_RENTAL.RNT_TB_SPECIAL_OFFER.Where(x => x.dtPublicEnd >= DateTime.Now.AddMonths(-1) || x.dtEnd >= DateTime.Now.AddMonths(-1)).ToList();
                        AppSettings.RNT_VIEW_SPECIAL_OFFERs = DC_RENTAL.RNT_VIEW_SPECIAL_OFFER.Where(x => x.dtPublicEnd >= DateTime.Now.AddMonths(-1) || x.dtEnd >= DateTime.Now.AddMonths(-1)).ToList();
                    }
                }
                rdp_flt_dtFrom.SelectedDate = DateTime.Now;
                drp_flt_pidCity_DataBind();
                drp_flt_pidOwner_DataBind();
                closeDetails();
            }
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                //Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                //if (lbl_id == null) return;
                //using (DCmodRental dc = new DCmodRental())
                //{
                //    currTBL = dc.dbRntEstatePriceChangesTBLs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                //    if (currTBL != null)
                //    {
                //        dc.Delete(currTBL);
                //        dc.SaveChanges();
                //        rntProps.EstatePriceChangesTBL = dc.dbRntEstatePriceChangesTBLs.ToList();
                //    }
                //}
                //closeDetails(false);
            }
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            closeDetails();
        }
        protected void closeDetails()
        {
            setfilters();
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
        }
        private void drp_flt_pidCity_DataBind()
        {
            drp_flt_pidCity.DataSource = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.pid_lang == 1 && x.is_active == 1).OrderBy(x => x.title);
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
            chkList_flt_pidZone.DataSource = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1 && zoneIds.Contains(x.id) && (drp_flt_pidCity.getSelectedValueInt(0) == 0 || x.pid_city == drp_flt_pidCity.getSelectedValueInt(0))).OrderBy(x => x.title);
            chkList_flt_pidZone.DataTextField = "title";
            chkList_flt_pidZone.DataValueField = "id";
            chkList_flt_pidZone.DataBind();
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            closeDetails();
        }
        protected void setfilters()
        {
            var zoneIds = chkList_flt_pidZone.getSelectedValueList().Select(x => (int?)x.ToInt32()).ToList();
            var estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1
                            && (drp_flt_pidCity.getSelectedValueInt(0) <= 0 || x.pid_city == drp_flt_pidCity.getSelectedValueInt(0))
                            && (zoneIds.Count <= 0 || zoneIds.Contains(x.pid_zone))
                            && (drp_flt_pidOwner.getSelectedValueInt(0) <= 0 || x.pid_owner == drp_flt_pidOwner.getSelectedValueInt(0))
                            && (txt_flt_code.Text.Trim() == "" || x.code.ToLower().Contains(txt_flt_code.Text.Trim().ToLower()))
                            ).Select(x => x.id).ToList();


            string _filter = "";
            string _sep = "";

            _filter += "pid_lang = 1";
            _sep = " and ";

            if (estateIds.Count != 0)
            {
                _filter += _sep + "(";
                _sep = "";
                foreach (int _id in estateIds)
                {
                    _filter += _sep + "pid_estate = " + _id + "";
                    _sep = " or ";
                }
                _filter += ")";
                _sep = " and ";
            }
            if (rdp_flt_dtFrom.SelectedDate.HasValue)
            {
                _filter += _sep + "dtEnd >= DateTime.Parse(\"" + rdp_flt_dtFrom.SelectedDate + "\")";
                _sep = " and ";
            }
            if (rdp_flt_dtTo.SelectedDate.HasValue)
            {
                _filter += _sep + "dtStart <= DateTime.Parse(\"" + rdp_flt_dtTo.SelectedDate + "\")";
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
