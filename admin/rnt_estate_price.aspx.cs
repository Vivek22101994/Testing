using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;
using Telerik.Web.UI;

namespace RentalInRome.admin
{
    public partial class rnt_estate_price : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                lnk_save.Visible = lnk_annulla.Visible = UserAuthentication.hasPermission("rnt_estate", "can_edit");
                //var periods = DC_RENTAL.RNT_VIEW_PERIODs.Where(x => x.pid_lang == 1).ToList();
                //drp_period.DataSource = periods;
                //drp_period.DataBind();
                //drp_period.Items.Insert(0, new ListItem("-seleziona-", "-1"));
                int id = Request.QueryString["id"].ToInt32();
                RNT_VIEW_ESTATE _est = DC_RENTAL.RNT_VIEW_ESTATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);

                if (_est != null)
                {
                    IdEstate = _est.id;
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                    drp_pidSeasonGroup_DataBind();
                    FillControls();
                    UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                }
                else
                {
                    Response.Redirect("rnt_estate_list.aspx");
                }
            }
        }

        public int IdPrice
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
                FillControls();
            }
        }

        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }


        protected void pnl_pidSeasonGroup_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            drp_pidSeasonGroup_DataBind();
            drp_pidSeasonGroup.setSelectedValue(e.Argument);
        }
        protected void drp_pidSeasonGroup_DataBind()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                drp_pidSeasonGroup.DataSource = dc.dbRntSeasonGroupTBLs.Where(x => x.id != 0).OrderBy(x => x.code).ToList();
                drp_pidSeasonGroup.DataTextField = "code";
                drp_pidSeasonGroup.DataValueField = "id";
                drp_pidSeasonGroup.DataBind();
                drp_pidSeasonGroup.Items.Insert(0, new ListItem("-stagionalità predefinita-", "0"));


            }
        }
        protected void FillControls()
        {
            lbl_changeSaved.Visible = false;
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;

            drp_pidSeasonGroup.setSelectedValue(_currTBL.pidSeasonGroup);

            txt_pr_basePersons.Text = _currTBL.pr_basePersons.objToInt32().ToString();

            ntxt_price_1.Value = _currTBL.pr_1_2pax.objToDouble();
            ntxt_price_optional_1.Value = _currTBL.pr_1_opt.objToDouble();
            ntxt_price_2.Value = _currTBL.pr_2_2pax.objToDouble();
            ntxt_price_optional_2.Value = _currTBL.pr_2_opt.objToDouble();
            ntxt_price_3.Value = _currTBL.pr_3_2pax.objToDouble();
            ntxt_price_optional_3.Value = _currTBL.pr_3_opt.objToDouble();
            ntxt_price_4.Value = _currTBL.pr_4_2pax.objToDouble();
            ntxt_price_optional_4.Value = _currTBL.pr_4_opt.objToDouble();            

            txt_pr_discount7days.Text = _currTBL.pr_discount7days.objToInt32().ToString();
            txt_pr_discount30days.Text = _currTBL.pr_discount30days.objToInt32().ToString();

            txt_lm_inhours.Text = _currTBL.lm_inhours.objToInt32().ToString();
            txt_lm_discount.Text = _currTBL.lm_discount.objToInt32().ToString();
            txt_lm_nights_min.Text = _currTBL.lm_nights_min.objToInt32().ToString();
            txt_lm_nights_max.Text = _currTBL.lm_nights_max.objToInt32().ToString();

            drp_pr_dcSUsed.setSelectedValue(_currTBL.pr_dcSUsed.ToString());
            pnl_dcSUsed_1.Visible = drp_pr_dcSUsed.SelectedValue == "1";
            pnl_dcSUsed_2.Visible = !pnl_dcSUsed_1.Visible;

            txt_pr_dcS2_1_inDays.Text = _currTBL.pr_dcS2_1_inDays.objToInt32().ToString();
            txt_pr_dcS2_1_percent.Text = _currTBL.pr_dcS2_1_percent.objToInt32().ToString();
            txt_pr_dcS2_2_inDays.Text = _currTBL.pr_dcS2_2_inDays.objToInt32().ToString();
            txt_pr_dcS2_2_percent.Text = _currTBL.pr_dcS2_2_percent.objToInt32().ToString();
            txt_pr_dcS2_3_inDays.Text = _currTBL.pr_dcS2_3_inDays.objToInt32().ToString();
            txt_pr_dcS2_3_percent.Text = _currTBL.pr_dcS2_3_percent.objToInt32().ToString();
            txt_pr_dcS2_4_inDays.Text = _currTBL.pr_dcS2_4_inDays.objToInt32().ToString();
            txt_pr_dcS2_4_percent.Text = _currTBL.pr_dcS2_4_percent.objToInt32().ToString();
            txt_pr_dcS2_5_inDays.Text = _currTBL.pr_dcS2_5_inDays.objToInt32().ToString();
            txt_pr_dcS2_5_percent.Text = _currTBL.pr_dcS2_5_percent.objToInt32().ToString();
            txt_pr_dcS2_6_inDays.Text = _currTBL.pr_dcS2_6_inDays.objToInt32().ToString();
            txt_pr_dcS2_6_percent.Text = _currTBL.pr_dcS2_6_percent.objToInt32().ToString();
            txt_pr_dcS2_7_inDays.Text = _currTBL.pr_dcS2_7_inDays.objToInt32().ToString();
            txt_pr_dcS2_7_percent.Text = _currTBL.pr_dcS2_7_percent.objToInt32().ToString();
        }

        protected void FillDataFromControls()
        {
            lbl_changeSaved.Visible = false;
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            RNT_TB_ESTATE tbBefore = _currTBL.Clone();
            _currTBL.pidSeasonGroup = drp_pidSeasonGroup.getSelectedValueInt();
            _currTBL.pr_basePersons = txt_pr_basePersons.Text.ToInt32();
            _currTBL.pr_1_2pax = ntxt_price_1.Value.objToDecimal();
            _currTBL.pr_1_opt = ntxt_price_optional_1.Value.objToDecimal();
            _currTBL.pr_2_2pax = ntxt_price_2.Value.objToDecimal();
            _currTBL.pr_2_opt = ntxt_price_optional_2.Value.objToDecimal();
            _currTBL.pr_3_2pax = ntxt_price_3.Value.objToDecimal();
            _currTBL.pr_3_opt = ntxt_price_optional_3.Value.objToDecimal();
            _currTBL.pr_4_2pax = ntxt_price_4.Value.objToDecimal();
            _currTBL.pr_4_opt = ntxt_price_optional_4.Value.objToDecimal();


            _currTBL.pr_discount7days = txt_pr_discount7days.Text.ToDecimal();
            _currTBL.pr_discount30days = txt_pr_discount30days.Text.ToDecimal();

            _currTBL.pr_startDate = _currTBL.pr_startDate.HasValue ? _currTBL.pr_startDate : DateTime.Now;
            // todo registrare nello storico
            _currTBL.pr_dcSUsed = drp_pr_dcSUsed.getSelectedValueInt(0) == 0 ? 1 : drp_pr_dcSUsed.getSelectedValueInt(0);

            _currTBL.pr_dcS2_1_inDays = txt_pr_dcS2_1_inDays.Text.ToInt32();
            _currTBL.pr_dcS2_1_percent = txt_pr_dcS2_1_percent.Text.ToDecimal();
            _currTBL.pr_dcS2_2_inDays = txt_pr_dcS2_2_inDays.Text.ToInt32();
            _currTBL.pr_dcS2_2_percent = txt_pr_dcS2_2_percent.Text.ToDecimal();
            _currTBL.pr_dcS2_3_inDays = txt_pr_dcS2_3_inDays.Text.ToInt32();
            _currTBL.pr_dcS2_3_percent = txt_pr_dcS2_3_percent.Text.ToDecimal();
            _currTBL.pr_dcS2_4_inDays = txt_pr_dcS2_4_inDays.Text.ToInt32();
            _currTBL.pr_dcS2_4_percent = txt_pr_dcS2_4_percent.Text.ToDecimal();
            _currTBL.pr_dcS2_5_inDays = txt_pr_dcS2_5_inDays.Text.ToInt32();
            _currTBL.pr_dcS2_5_percent = txt_pr_dcS2_5_percent.Text.ToDecimal();
            _currTBL.pr_dcS2_6_inDays = txt_pr_dcS2_6_inDays.Text.ToInt32();
            _currTBL.pr_dcS2_6_percent = txt_pr_dcS2_6_percent.Text.ToDecimal();
            _currTBL.pr_dcS2_7_inDays = txt_pr_dcS2_7_inDays.Text.ToInt32();
            _currTBL.pr_dcS2_7_percent = txt_pr_dcS2_7_percent.Text.ToDecimal();

            _currTBL.lm_inhours = txt_lm_inhours.Text.ToInt32();
            _currTBL.lm_discount = txt_lm_discount.Text.ToInt32();
            _currTBL.lm_nights_min = txt_lm_nights_min.Text.ToInt32();
            _currTBL.lm_nights_max = txt_lm_nights_max.Text.ToInt32();

            DC_RENTAL.SubmitChanges();
            if (tbBefore != null) rntUtils.estate_addLog(tbBefore, _currTBL, UserAuthentication.CurrentUserID, UserAuthentication.CurrentUserName);
            AppSettings._refreshCache_RNT_ESTATEs();
            AppSettings.RELOAD_SESSION();
            rntUtilsChnlAll.UpdateRates(IdEstate);
            //if (_currTBL.bcomEnabled == 1)
            //    BcomUpdate.BcomUpdate_start(IdEstate, "rates");
            lbl_changeSaved.Visible = true;
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_change_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_new_Click(object sender, EventArgs e)
        {
            IdPrice = 0;
        }

        protected void drp_pr_dcSUsed_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnl_dcSUsed_1.Visible = drp_pr_dcSUsed.SelectedValue == "1";
            pnl_dcSUsed_2.Visible = !pnl_dcSUsed_1.Visible;
        }
    }
}